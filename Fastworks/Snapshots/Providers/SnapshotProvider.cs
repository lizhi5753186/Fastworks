using System;

namespace Fastworks.Snapshots.Providers
{
    /// <summary>
    /// Represents the base class for all snapshot providers.
    /// </summary>
    public abstract class SnapshotProvider : DisposableObject, ISnapshotProvider
    {
        private readonly SnapshotProviderOption option;
        private bool committed;

        public SnapshotProviderOption Option
        {
            get { return this.option; }
        }
       
        public SnapshotProvider(SnapshotProviderOption option)
        {
            this.option = option;
        }

        #region override Methods

        protected override void Dispose(bool disposing) { }
        #endregion

        #region ISnapshotProvider Members
        
        public abstract bool CanCreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot);
        
        public abstract void CreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot);
        
        public abstract ISnapshot GetSnapshot(Type aggregateRootType, Guid id);
       
        public abstract bool HasSnapshot(Type aggregateRootType, Guid id);
        #endregion

        #region IUnitOfWork Members
       
        public abstract bool DistributedTransactionSupported { get; }
        
        public bool Committed
        {
            get { return this.committed; }
            protected set { this.committed = value; }
        }
       
        public abstract void Commit();
        
        public abstract void Rollback();

        #endregion
    }
}
