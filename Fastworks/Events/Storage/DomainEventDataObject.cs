using Fastworks.Events.Serialization;
using System;
using System.Configuration;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Fastworks.Events.Storage
{
    /// <summary>
    /// Domain event Data Transfer Object
    /// </summary>
    [Serializable]
    public class DomainEventDataObject
    {
        private readonly IDomainEventSerializer _serializer;

        #region Public Properties

        /// <summary>
        /// Get or set the data of current domain event object
        /// </summary>
        [XmlElement]
        [DataMember]
        public byte[] Data { get; set; }

        /// <summary>
        /// Get or set the Assembly name of the domain event
        /// </summary>
        [XmlElement]
        [DataMember]
        public string AssemblyQualifiedEventType { get; set; }

        [XmlElement]
        [DataMember]
        public Guid Id { get; set; }

        [XmlElement]
        [DataMember]
        public Guid SourceId { get; set; }

        /// <summary>
        /// Get or set the Assembly name of the Aggregate root
        /// </summary>
        [XmlElement]
        [DataMember]
        public string AssemblyQualifiedSourceType { get; set; }

        [XmlElement]
        [DataMember]
        public DateTime Timestamp { get; set; }

        [XmlElement]
        [DataMember]
        public long Version { get; set; }
        
        #endregion 

        public DomainEventDataObject()
        {
            this._serializer = GetDomainEventSerializer();
        }

        #region Private Static Methods

        private static IDomainEventSerializer GetDomainEventSerializer()
        {
            IDomainEventSerializer serializer = null;
            string eventSerializerTypeName = ConfigurationManager.AppSettings["eventSerializer"];
            if (eventSerializerTypeName == null)
                serializer = new DomainEventXmlSerializer();
            else
            {
                Type serializerType = Type.GetType(eventSerializerTypeName);
                if (serializerType == null)
                    throw new InfrastructureException("The serializer defined by type '{0}' doesn't exist.", eventSerializerTypeName);
                serializer = (IDomainEventSerializer)Activator.CreateInstance(serializerType);
            }
            return serializer;
        }

        #endregion 

        #region Public Methods

        // crate and initializes the domain event data object from given domain event
        public static DomainEventDataObject FromDomainEvent(IDomainEvent @event)
        {
            IDomainEventSerializer serializer = GetDomainEventSerializer();
            DomainEventDataObject domainEventDto = new DomainEventDataObject();
            domainEventDto.Data = serializer.Serialize(@event);
            domainEventDto.Id = @event.Id;
            if (string.IsNullOrEmpty(@event.AssemblyQualifiedEventType))
                domainEventDto.AssemblyQualifiedEventType = @event.GetType().AssemblyQualifiedName;
            else
                domainEventDto.AssemblyQualifiedEventType = @event.AssemblyQualifiedEventType;
            domainEventDto.Timestamp = @event.Timestamp;
            domainEventDto.Version = @event.Version;
            domainEventDto.SourceId = @event.Source.Id;
            domainEventDto.AssemblyQualifiedSourceType = @event.Source.GetType().AssemblyQualifiedName;
            return domainEventDto;
        }

        // Convert the domain event data to it's correponding domain event 
        public IDomainEvent ToDomainEvent()
        {
            if (string.IsNullOrEmpty(this.AssemblyQualifiedEventType))
                throw new ArgumentNullException("AssemblyQualifiedTypeName");
            if (this.Data == null || this.Data.Length == 0)
                throw new ArgumentNullException("Data");

            Type type = Type.GetType(this.AssemblyQualifiedEventType);
            IDomainEvent domainEvent = (IDomainEvent)_serializer.Deserialize(type, this.Data);
            domainEvent.Id = this.Id;
            return domainEvent;
        }

        #endregion 
    }
}
