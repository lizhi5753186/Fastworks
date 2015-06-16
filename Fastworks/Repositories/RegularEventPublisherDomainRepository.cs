using Fastworks.Bus;
using Fastworks.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Fastworks.Repositories
{
    public class RegularEventPublisherDomainRepository : EventPublisherDomainRepository
    {
        #region Private Fields
        private readonly IRepositoryContext context;
        private readonly HashSet<ISourcedAggregateRoot> dirtyHash = new HashSet<ISourcedAggregateRoot>();
        #endregion

        #region Public Properties
        public IRepositoryContext Context
        {
            get { return this.context; }
        }
        #endregion


        #region Ctor

        public RegularEventPublisherDomainRepository(IRepositoryContext context, IEventBus eventBus)
            : base(eventBus)
        {
            this.context = context;
        }
        #endregion

        #region Private Methods
        private void PublishAggregateRootEvents(ISourcedAggregateRoot aggregateRoot)
        {
            foreach (var evt in aggregateRoot.UncommittedEvents)
            {
                this.EventBus.Publish(evt);
            }
        }
        #endregion

        #region Override Methods
        protected override void DoCommit()
        {
            foreach (var aggregateRootObj in this.SaveHash)
            {
                this.context.RegisterNew(aggregateRootObj);
                this.PublishAggregateRootEvents(aggregateRootObj);
            }
            foreach (var aggregateRootObj in this.dirtyHash)
            {
                this.context.RegisterModified(aggregateRootObj);
                this.PublishAggregateRootEvents(aggregateRootObj);
            }
            if (this.DistributedTransactionSupported)
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    this.context.Commit();
                    this.EventBus.Commit();
                    ts.Complete();
                }
            }
            else
            {
                this.context.Commit();
                this.EventBus.Commit();
            }
            this.dirtyHash.ToList().ForEach(this.DelegatedUpdateAndClearAggregateRoot);
            this.dirtyHash.Clear();
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
                this.context.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region IDomainRepository Members

        public override TAggregateRoot Get<TAggregateRoot>(Guid id)
        {
            var querySaveHash = from p in this.SaveHash
                                where p.Id.Equals(id)
                                select p;
            var queryDirtyHash = from p in this.dirtyHash
                                 where p.Id.Equals(id)
                                 select p;
            if (querySaveHash != null && querySaveHash.Count() > 0)
                return querySaveHash.FirstOrDefault() as TAggregateRoot;
            if (queryDirtyHash != null && queryDirtyHash.Count() > 0)
                return queryDirtyHash.FirstOrDefault() as TAggregateRoot;

            IRepository<TAggregateRoot> repository = ServiceLocator.Instance.GetService<IRepository<TAggregateRoot>>(new { context = this.context });
            ISpecification<TAggregateRoot> spec = Specification<TAggregateRoot>.Eval(ar => ar.Id.Equals(id));
            var result = repository.GetBySpecification(spec);
            // Clears the aggregate root since version info is not needed in regular repositories.
            this.DelegatedUpdateAndClearAggregateRoot(result);
            return result;
        }

        public override void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
        {
            IRepository<TAggregateRoot> repository = ServiceLocator.Instance.GetService<IRepository<TAggregateRoot>>(new { context = this.context });
            ISpecification<TAggregateRoot> spec = Specification<TAggregateRoot>.Eval(ar => ar.Id.Equals(aggregateRoot.Id));
            if (repository.Exists(spec))
            {
                if (!this.dirtyHash.Contains(aggregateRoot))
                    this.dirtyHash.Add(aggregateRoot);
                this.Committed = false;
            }
            else
            {
                base.Save<TAggregateRoot>(aggregateRoot);
            }
        }

        public override bool DistributedTransactionSupported
        {
            get
            {
                return this.context.DistributedTransactionSupported && base.DistributedTransactionSupported;
            }
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            base.Rollback();
            this.context.Rollback();
        }
        #endregion
    }
}
