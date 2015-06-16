using Fastworks.Events;
using Fastworks.Snapshots;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Fastworks
{
    public abstract class SourcedAggregateRoot : ISourcedAggregateRoot
    {
        #region Private Fields
        private long eventVersion;
        private readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();
        private readonly Dictionary<Type, List<object>> _domainEventHandlers = new Dictionary<Type, List<object>>();
        #endregion 

        internal const string UpdateVersionAndClearUncommittedEventsMethodName = @"UpdateVersionAndClearUncommittedEvents";

        #region Ctor
        public SourcedAggregateRoot()
            : this(new Guid())
        {
        }

        public SourcedAggregateRoot(Guid id)
        {
            this.Id = id;
            this.Version = Constants.DefaultVersion;
            this.eventVersion = this.Version;
        }

        #endregion 

        #region Private Methods
        private IEnumerable<dynamic> GetDomainEventHandlers(IDomainEvent domainEvent)
        {
            Type eventType = domainEvent.GetType();
            if (_domainEventHandlers.ContainsKey(eventType))
                return _domainEventHandlers[eventType];
            else
            {
                List<dynamic> handlers = new List<dynamic>();
                MethodInfo[] allMethods = this.GetType().GetMethods(BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.Instance);
               IEnumerable<object> handlerMethods = from method in allMethods
                                     let returnType = method.ReturnType
                                     let @params = method.GetParameters()
                                     let handlerAttributes = method.GetCustomAttributes(typeof(HandlesAttribute), false)
                                     where returnType == typeof(void) &&
                                     @params != null &&
                                     @params.Count() > 0 &&
                                     @params[0].ParameterType.Equals(eventType) &&
                                     handlerAttributes != null &&
                                     ((HandlesAttribute)handlerAttributes[0]).DomainEventType.Equals(eventType)
                                     select new { MethodInfo = method };

                _domainEventHandlers.Add(eventType, handlerMethods.ToList());
                return handlerMethods;
            }
        }

        private void HandleEvent<TEvent>(TEvent @event)
           where TEvent : class, IDomainEvent
        {
            var handlers = this.GetDomainEventHandlers(@event);
            foreach (var handler in handlers)
            {
                var eventHandler = handler as IDomainEventHandler<TEvent>;
                eventHandler.Handle(@event);
            }
        }

        #endregion 

        #region Protected Methods

        protected virtual void RaiseEvent<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            @event.Id = Guid.NewGuid();
            @event.Version = ++eventVersion;
            @event.Source = this;
            @event.AssemblyQualifiedEventType = typeof(TEvent).AssemblyQualifiedName;
            @event.Timestamp = DateTime.UtcNow;
            this.HandleEvent<TEvent>(@event);
            _uncommittedEvents.Add(@event);
        }

        #endregion 

        #region Public Methods
        #endregion 

        #region ISourcedAggregateRoot Members
        public void BuildFromHistory(IEnumerable<Events.IDomainEvent> historicalEvents)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDomainEvent> UncommittedEvents
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long Version { get; set; }
       
        public virtual Guid Id { get; set; }
        

        #endregion 

        #region ISnapshotOrignator Members
        public void BuildFromSnapshot(ISnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public ISnapshot CreateSnapshot()
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
}
