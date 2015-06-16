using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fastworks.Repositories
{
    public abstract class DomainRepository : DisposableObject, IDomainRepository
    {
        #region Private Fields
        private volatile bool committed;
        private readonly HashSet<ISourcedAggregateRoot> saveHash = new HashSet<ISourcedAggregateRoot>();
        private readonly Action<ISourcedAggregateRoot> delegatedUpdateAndClearAggregateRoot = ar =>
        {
            ar.GetType().GetMethod(SourcedAggregateRoot.UpdateVersionAndClearUncommittedEventsMethodName,
                BindingFlags.NonPublic | BindingFlags.Instance).Invoke(ar, null);
        };

        #endregion 

        #region Protected Properties
        protected HashSet<ISourcedAggregateRoot> SaveHash
        {
            get { return this.saveHash; }
        }

        protected Action<ISourcedAggregateRoot> DelegatedUpdateAndClearAggregateRoot
        {
            get { return this.delegatedUpdateAndClearAggregateRoot; }
        }

        #endregion 

        #region Ctor

        public DomainRepository()
        {
            this.committed = false;
        }

        #endregion

        #region  Override DisposableObject Members
        protected override void Dispose(bool disposing = false)
        {
        }
        #endregion 

        #region Protected Methods
        protected abstract void DoCommit(); 
        protected TAggregateRoot CreateAggregateRootInstance<TAggregateRoot>()
            where TAggregateRoot : class, ISourcedAggregateRoot, new()
        {
            TAggregateRoot aggregateTRoot = new TAggregateRoot();
            return aggregateTRoot;

            //Type aggregateRootType = typeof(TAggregateRoot);
            //ConstructorInfo constructor = aggregateRootType
            //    .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            //    .Where(p =>
            //    {
            //        var parameters = p.GetParameters();
            //        return parameters == null || parameters.Length == 0;
            //    }).FirstOrDefault();
            //if (constructor != null)
            //    return constructor.Invoke(null) as TAggregateRoot;
            //throw new RepositoryException("At least one parameterless constructor should be defined on the aggregate root type '{0}'.", typeof(TAggregateRoot));
        }
        #endregion 

        #region IDomainRepository Members
        public abstract TAggregateRoot Get<TAggregateRoot>(Guid id)
            where TAggregateRoot : class, ISourcedAggregateRoot, new();

        public virtual void Save<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : class, ISourcedAggregateRoot, new()
        {
            if (!saveHash.Contains(aggregateRoot))
                saveHash.Add(aggregateRoot);
            committed = false;
        }

        public abstract bool DistributedTransactionSupported { get; }

        public bool Committed
        {
            get { return this.committed; }
            protected set { this.committed = value; }
        }

        public void Commit()
        {
            this.DoCommit();
            this.saveHash.ToList().ForEach(this.delegatedUpdateAndClearAggregateRoot);
            this.saveHash.Clear();
            this.committed = true;
        }

        public abstract void Rollback();
        
        #endregion 
    }
}
