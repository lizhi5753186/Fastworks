using Fastworks.Bus;
using Fastworks.Snapshots;
using Fastworks.Specifications;
using Fastworks.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Fastworks.Repositories
{
    public class SnapshotDomainRepository : EventPublisherDomainRepository
    {
        private readonly IStorage storage;
        
        public IStorage Storage
        {
            get { return this.storage; }
        }
       
        public SnapshotDomainRepository(IStorage storage, IEventBus eventBus)
            : base(eventBus)
        {
            this.storage = storage;
        }
       

        #region Override Methods

        protected override void DoCommit()
        {
            foreach (ISourcedAggregateRoot aggregateRoot in this.SaveHash)
            {
                SnapshotDataObject snapshotDataObject = SnapshotDataObject.CreateFromAggregateRoot(aggregateRoot);
                var aggregateRootId = aggregateRoot.Id;
                var aggregateRootType = aggregateRoot.GetType().AssemblyQualifiedName;
                ISpecification<SnapshotDataObject> spec = Specification<SnapshotDataObject>.Eval(p => p.AggregateRootId == aggregateRootId && p.AggregateRootType == aggregateRootType);
                var firstMatch = this.storage.SelectFirstOnly<SnapshotDataObject>(spec);
                if (firstMatch != null)
                    this.storage.Update<SnapshotDataObject>(new PropertyBag(snapshotDataObject), spec);
                else
                    this.storage.Insert<SnapshotDataObject>(new PropertyBag(snapshotDataObject));
                foreach (var evnt in aggregateRoot.UncommittedEvents)
                {
                    this.EventBus.Publish(evnt);
                }
            }
            if (this.DistributedTransactionSupported)
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    this.storage.Commit();
                    this.EventBus.Commit();
                    ts.Complete();
                }
            }
            else
            {
                this.storage.Commit();
                this.EventBus.Commit();
            }
        }
       
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
                this.storage.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion 

        #region IDomainRepository Members
        
        public override TAggregateRoot Get<TAggregateRoot>(Guid id)
        {
            string aggregateRootType = typeof(TAggregateRoot).AssemblyQualifiedName;
            ISpecification<SnapshotDataObject> spec = Specification<SnapshotDataObject>.Eval(p => p.AggregateRootId == id && p.AggregateRootType == aggregateRootType);
            SnapshotDataObject snapshotDataObject = this.storage.SelectFirstOnly<SnapshotDataObject>(spec);
            if (snapshotDataObject == null)
                throw new RepositoryException("The aggregate (id={0}) cannot be found in the domain repository.", id);
            ISnapshot snapshot = snapshotDataObject.ExtractSnapshot();
            TAggregateRoot aggregateRoot = this.CreateAggregateRootInstance<TAggregateRoot>();
            aggregateRoot.BuildFromSnapshot(snapshot);
            return aggregateRoot;
        }
        #endregion

        #region IUnitOfWork Members
        
        public override bool DistributedTransactionSupported
        {
            get { return this.storage.DistributedTransactionSupported && base.DistributedTransactionSupported; }
        }
        
        public override void Rollback()
        {
            base.Rollback();
            this.storage.Rollback();
        }
        #endregion
    }
}
