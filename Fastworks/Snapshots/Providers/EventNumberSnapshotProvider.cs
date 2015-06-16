using Fastworks.Events.Storage;
using Fastworks.Specifications;
using Fastworks.Storage;
using System;
using System.Collections.Generic;

namespace Fastworks.Snapshots.Providers
{
    /// <summary>
    /// Represents the snapshot provider that takes the number of stored domain events
    /// as the strategy to implement its snapshot functionalities.
    /// </summary>
    public class EventNumberSnapshotProvider : StorageBasedSnapshotProvider
    {
        #region EventNumberSnapshotMappingKey class

        // Snapshot Cache key 
        sealed class EventNumberSnapshotMappingKey : IEquatable<EventNumberSnapshotMappingKey>
        {
            public string TypeName { get; set; }

            public Guid Id { get; set; }

            public EventNumberSnapshotMappingKey() { }

            public EventNumberSnapshotMappingKey(string typeName, Guid id)
            {
                this.TypeName = typeName;
                this.Id = id;
            }

            #region Object Members
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                if (this.GetType() != obj.GetType())
                    return false;
                return this.Equals(obj as EventNumberSnapshotMappingKey);
            }
            public override int GetHashCode()
            {
                return Utils.GetHashCode(this.Id.GetHashCode(), this.TypeName.GetHashCode());
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}", this.TypeName, this.Id);
            }
            #endregion 

            #region Operator Overrides
            public static bool operator ==(EventNumberSnapshotMappingKey a, EventNumberSnapshotMappingKey b)
            {
                if (object.ReferenceEquals(a, b))
                    return true;
                if (((object)a == null) || ((object)b == null))
                    return false;
                return a.Id == b.Id && a.TypeName == b.TypeName;
            }

            public static bool operator !=(EventNumberSnapshotMappingKey a, EventNumberSnapshotMappingKey b)
            {
                return !(a == b);
            }

            #endregion

            #region IEquatable<EventNumberSnapshotMappingKey> Members
            public bool Equals(EventNumberSnapshotMappingKey other)
            {
                if (object.ReferenceEquals(this, other))
                    return true;
                if (other == null)
                    return false;
                return this.Id == other.Id && this.TypeName == other.TypeName;
            }
            #endregion 
        }
        #endregion

        private readonly int numOfEvents;

        // Snapshot Cache
        private readonly Dictionary<EventNumberSnapshotMappingKey, ISnapshot> snapshotMapping = new Dictionary<EventNumberSnapshotMappingKey, ISnapshot>();

        public int NumberOfEvents
        {
            get { return this.numOfEvents; }
        }
       
        public EventNumberSnapshotProvider(IStorage eventStorage, IStorage snapshotStorage, SnapshotProviderOption option, int numOfEvents)
            : base(eventStorage, snapshotStorage, option)
        {
            this.numOfEvents = numOfEvents;
        }
      
        public override bool DistributedTransactionSupported
        {
            get { return this.SnapshotStorage.DistributedTransactionSupported; }
        }

        #region override Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.Committed)
                {
                    try
                    {
                        this.Commit();
                    }
                    catch
                    {
                        this.Rollback();
                        throw;
                    }
                }
            }

            base.Dispose(disposing);
        }
        #endregion

        #region Public Methods
       
        public override bool CanCreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot)
        {
            if (this.HasSnapshot(aggregateRoot.GetType(), aggregateRoot.Id))
            {
                ISnapshot snapshot = this.GetSnapshot(aggregateRoot.GetType(), aggregateRoot.Id);
                return snapshot.Version + numOfEvents <= aggregateRoot.Version;
            }
            else
            {
                string aggregateRootTypeName = aggregateRoot.GetType().AssemblyQualifiedName;
                Guid aggregateRootId = aggregateRoot.Id;
                long version = aggregateRoot.Version;
                ISpecification<DomainEventDataObject> spec = Specification<DomainEventDataObject>.Eval(
                    p => p.SourceId == aggregateRootId &&
                        p.AssemblyQualifiedSourceType == aggregateRootTypeName &&
                        p.Version <= version);
                int eventCnt = this.EventStorage.GetRecordCount<DomainEventDataObject>(spec);
                return eventCnt >= this.numOfEvents;
            }
        }
        
        public override void CreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot)
        {
            ISnapshot snapshot = aggregateRoot.CreateSnapshot();
            SnapshotDataObject dataObj = SnapshotDataObject.CreateFromAggregateRoot(aggregateRoot);
            PropertyBag insertOrUpdateData = new PropertyBag(dataObj);
            var key = new EventNumberSnapshotMappingKey(aggregateRoot.GetType().AssemblyQualifiedName, aggregateRoot.Id);

            if (this.HasSnapshot(aggregateRoot.GetType(), aggregateRoot.Id))
            {
                string aggregateRootTypeName = aggregateRoot.GetType().AssemblyQualifiedName;
                Guid aggregateRootId = aggregateRoot.Id;
                ISpecification<SnapshotDataObject> spec = Specification<SnapshotDataObject>.Eval(
                    p => p.AggregateRootType == aggregateRootTypeName &&
                        p.AggregateRootId == aggregateRootId);
                this.SnapshotStorage.Update<SnapshotDataObject>(insertOrUpdateData, spec);
                this.Committed = false;
                if (snapshotMapping.ContainsKey(key))
                    snapshotMapping[key] = snapshot;
                else
                    snapshotMapping.Add(key, snapshot);
            }
            else
            {
                this.SnapshotStorage.Insert<SnapshotDataObject>(insertOrUpdateData);
                this.Committed = true;
                snapshotMapping.Add(key, snapshot);
            }
        }
        
        public override ISnapshot GetSnapshot(Type aggregateRootType, Guid id)
        {
            var key = new EventNumberSnapshotMappingKey(aggregateRootType.AssemblyQualifiedName, id);
            if (snapshotMapping.ContainsKey(key))
                return snapshotMapping[key];
            string aggregateRootTypeName = aggregateRootType.AssemblyQualifiedName;
            ISpecification<SnapshotDataObject> spec = Specification<SnapshotDataObject>.Eval(
                p => p.AggregateRootType == aggregateRootTypeName && p.AggregateRootId == id);
            SnapshotDataObject dataObj = this.SnapshotStorage.SelectFirstOnly<SnapshotDataObject>(spec);
            if (dataObj == null)
                return null;
            ISnapshot snapshot = dataObj.ExtractSnapshot();
            this.snapshotMapping.Add(key, snapshot);
            return snapshot;
        }
        
        public override bool HasSnapshot(Type aggregateRootType, Guid id)
        {
            var key = new EventNumberSnapshotMappingKey(aggregateRootType.AssemblyQualifiedName, id);
            if (snapshotMapping.ContainsKey(key))
                return true;
            string aggregateRootTypeName = aggregateRootType.AssemblyQualifiedName;
            ISpecification<SnapshotDataObject> spec = Specification<SnapshotDataObject>.Eval(
                p => p.AggregateRootType == aggregateRootTypeName && p.AggregateRootId == id);
            int snapshotRecordCnt = this.SnapshotStorage.GetRecordCount<SnapshotDataObject>(spec);
            if (snapshotRecordCnt > 0)
                return true;
            else
                return false;
        }
        
        public override void Commit()
        {
            this.SnapshotStorage.Commit();
            this.Committed = true;
        }
        
        public override void Rollback()
        {
            this.SnapshotStorage.Rollback();
            this.Committed = false;
        }
        #endregion
    }
}
