using Fastworks.Snapshots.Serialization;
using System;
using System.Configuration;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Fastworks.Snapshots
{
    /// <summary>
    /// Represents the snapshot data object.
    /// </summary>
    [Serializable]
    [XmlRoot]
    [DataContract]
    public class SnapshotDataObject : IEntity
    {
        private readonly ISnapshotSerializer serializer;

        #region Public Properties
        [XmlElement]
        [DataMember]
        public byte[] SnapshotData { get; set; }
       
        [XmlElement]
        [DataMember]
        public Guid AggregateRootId { get; set; }
        
        [XmlElement]
        [DataMember]
        public string AggregateRootType { get; set; }
       
        [XmlElement]
        [DataMember]
        public string SnapshotType { get; set; }
        
        [XmlElement]
        [DataMember]
        public long Version { get; set; }
        
        [XmlElement]
        [DataMember]
        public DateTime Timestamp { get; set; }
       
        [XmlElement]
        [DataMember]
        public Guid Id { get; set; }
        #endregion

        public SnapshotDataObject()
        {
            string eventSerializerTypeName = ConfigurationManager.AppSettings["napshotSerializer"];
            if (eventSerializerTypeName == null)
                serializer = new SnapshotXmlSerializer();
            else
            {
                Type serializerType = Type.GetType(eventSerializerTypeName);
                if (serializerType == null)
                    throw new InfrastructureException("The serializer defined by type '{0}' doesn't exist.", eventSerializerTypeName);
                serializer = (ISnapshotSerializer)Activator.CreateInstance(serializerType);
            }
        }
        
        #region Public Methods
        /// <summary>
        /// Extracts the snapshot from the current snapshot data object.
        /// </summary>
        /// <returns>The snapshot instance.</returns>
        public ISnapshot ExtractSnapshot()
        {
            try
            {
                Type snapshotType = Type.GetType(SnapshotType);
                if (snapshotType == null)
                    return null;
                return (ISnapshot)serializer.Deserialize(snapshotType, this.SnapshotData);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Creates the snapshot data object from the aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root for which the snapshot is being created.</param>
        /// <returns>The snapshot data object.</returns>
        public static SnapshotDataObject CreateFromAggregateRoot(ISourcedAggregateRoot aggregateRoot)
        {
            ISnapshotSerializer serializer = null;

            string eventSerializerTypeName = ConfigurationManager.AppSettings["napshotSerializer"];
            if (eventSerializerTypeName == null)
                serializer = new SnapshotXmlSerializer();
            else
            {
                Type serializerType = Type.GetType(eventSerializerTypeName);
                if (serializerType == null)
                    throw new InfrastructureException("The serializer defined by type '{0}' doesn't exist.", eventSerializerTypeName);
                serializer = (ISnapshotSerializer)Activator.CreateInstance(serializerType);
            }

            ISnapshot snapshot = aggregateRoot.CreateSnapshot();

            return new SnapshotDataObject
            {
                AggregateRootId= aggregateRoot.Id,
                AggregateRootType = aggregateRoot.GetType().AssemblyQualifiedName,
                Version = aggregateRoot.Version,
                SnapshotType = snapshot.GetType().AssemblyQualifiedName,
                Timestamp = snapshot.Timestamp,
                SnapshotData = serializer.Serialize(snapshot)
            };
        }
     
        #endregion
    }
}
