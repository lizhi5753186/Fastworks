using Fastworks.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastworks.Repositories
{
    public class RegularDomainRepository :DomainRepository
    {
        #region Private Fields
        private readonly IRepositoryContext _context;
        private readonly HashSet<ISourcedAggregateRoot> _dirtyHash = new HashSet<ISourcedAggregateRoot>();

        #endregion 

        #region Ctor
        public RegularDomainRepository(IRepositoryContext context)
        {
            this._context = context;
        }

        #endregion 

        #region Public Properties
        public IRepositoryContext Context
        {
            get { return this._context; }
        }

        #endregion 

        #region DomainRepository
        protected override void DoCommit()
        {
            foreach (var aggregateRootObj in this.SaveHash)
            {
                this._context.RegisterNew(aggregateRootObj);
            }
            foreach (var aggregateRootObj in this._dirtyHash)
            {
                this._context.RegisterModified(aggregateRootObj);
            }

            this._context.Commit();

            this._dirtyHash.ToList().ForEach(this.DelegatedUpdateAndClearAggregateRoot);
            this._dirtyHash.Clear();
        }

        public override TAggregateRoot Get<TAggregateRoot>(Guid id)
        {
            var querySaveHash = from p in this.SaveHash
                                where p.Id.Equals(id)
                                select p;
            var queryDirtyHash = from p in this._dirtyHash
                                 where p.Id.Equals(id)
                                 select p;
            if (querySaveHash != null && querySaveHash.Count() > 0)
                return querySaveHash.FirstOrDefault() as TAggregateRoot;
            if (queryDirtyHash != null && queryDirtyHash.Count() > 0)
                return queryDirtyHash.FirstOrDefault() as TAggregateRoot;

            IRepository<TAggregateRoot> repository = ServiceLocator.Instance.GetService<IRepository<TAggregateRoot>>(new { context = _context });
            ISpecification<TAggregateRoot> spec = Specification<TAggregateRoot>.Eval(ar => ar.Id.Equals(id));
            var result = repository.GetBySpecification(spec);
            // Clears the aggregate root since version info is not needed in regular repositories.
            this.DelegatedUpdateAndClearAggregateRoot(result);
            return result;
        }

        public override bool DistributedTransactionSupported
        {
            get { return _context.DistributedTransactionSupported; }
        }

        public override void Rollback()
        {
            this._context.Rollback();
        }
        #endregion 

        #region Override Methods

        protected override void Dispose(bool disposing = false)
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

                this._context.Dispose();
            }

            base.Dispose(disposing);

        }

        public override void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
        {
            IRepository<TAggregateRoot> repository = ServiceLocator.Instance.GetService<IRepository<TAggregateRoot>>(new { context = _context });
            ISpecification<TAggregateRoot> spec = Specification<TAggregateRoot>.Eval(ar => ar.Id.Equals(aggregateRoot.Id));
            if (repository.Exists(spec))
            {
                if (!this._dirtyHash.Contains(aggregateRoot))
                    this._dirtyHash.Add(aggregateRoot);
                this.Committed = false;
            }
            else
            {
                base.Save<TAggregateRoot>(aggregateRoot);
            }
        }
        #endregion 
    }
}
