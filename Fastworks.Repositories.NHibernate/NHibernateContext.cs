using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fastworks.Repositories.NHibernate
{
    public class NHibernateContext : RepositoryContext, INHibernateContext
    {
        #region Private Fields
        private ISession _session = null;
        private readonly ISessionFactory _sessionFactory = null;
        private ITransaction _transaction = null;
        #endregion 

        #region Ctor
        public NHibernateContext(ISessionFactory sessionFactory)
        {
            this._sessionFactory = sessionFactory;
        }

        #endregion 

        #region Private Methods
        private void EnsureSession()
        {
            if (this._session == null || !this._session.IsOpen)
            {
                this._session = this._sessionFactory.OpenSession();
                this._transaction = this._session.BeginTransaction();
            }
            else
            {
                if (this._transaction == null || !this._transaction.IsActive)
                {
                    this._transaction = this._session.BeginTransaction();
                }
            }
        }
        #endregion

        #region DisposableObject Members
        protected override void Dispose(bool disposing = false)
        {
            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
                if (_session != null)
                {
                    _session.Dispose();
                    _session = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion 
    
        #region IRepositoryContext Members

        public override void RegisterNew<TAggregateRoot>(TAggregateRoot entity)
        {
            EnsureSession();
            _session.Save(entity);
        }

        public override void RegisterModified<TAggregateRoot>(TAggregateRoot entity) 
        {
            EnsureSession();
            _session.Update(entity);
        }

        public override void RegisterDeleted<TAggregateRoot>(TAggregateRoot entity)
        {
            EnsureSession();
            _session.Delete(entity);
        }

        #region IUnitOfWork Members
        public override bool DistributedTransactionSupported
        {
            get { return false;  }
        }

        public override bool Committed
        {
            get
            {
                return _transaction != null &&
                    _transaction.WasCommitted;
            }
            protected set { }
        }

        public override void Commit()
        {
            EnsureSession();
            _transaction.Commit();
        }

        public override void Rollback()
        {
            EnsureSession();
            _transaction.Rollback();
        }

        #endregion 

        #endregion

        #region INHibernateContext Members
        public TAggregateRoot GetByKey<TAggregateRoot>(object key) where TAggregateRoot : class, IAggregateRoot
        {
            EnsureSession();
            var result = (TAggregateRoot)this._session.Get(typeof(TAggregateRoot), key);
            // Use of implicit transactions is discouraged.
            // For more information please refer to: http://www.hibernatingrhinos.com/products/nhprof/learn/alert/DoNotUseImplicitTransactions
            Commit();
            return result;
        }

        public IQueryable<TAggregateRoot> FindAll<TAggregateRoot>(Specifications.ISpecification<TAggregateRoot> specification, System.Linq.Expressions.Expression<Func<TAggregateRoot, dynamic>> sortPredicate, Storage.SortOrder sortOrder) where TAggregateRoot : class, IAggregateRoot
        {
            EnsureSession();
            IQueryable<TAggregateRoot> result = null;
            var query = this._session.Query<TAggregateRoot>()
                .Where(specification.Expression);
            switch (sortOrder)
            {
                case Storage.SortOrder.Ascending:
                    if (sortPredicate != null)
                        result = query.OrderBy(sortPredicate);
                    break;
                case Storage.SortOrder.Descending:
                    if (sortPredicate != null)
                        result = query.OrderByDescending(sortPredicate);
                    break;
                default:
                    result = query;
                    break;
            }
          
            Commit();
            return result;
        }

        public PagedResult<TAggregateRoot> FindAll<TAggregateRoot>(Specifications.ISpecification<TAggregateRoot> specification, System.Linq.Expressions.Expression<Func<TAggregateRoot, dynamic>> sortPredicate, Storage.SortOrder sortOrder, int pageNumber, int pageSize) where TAggregateRoot : class, IAggregateRoot
        {
            EnsureSession();
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");
            if (sortPredicate == null)
                throw new ArgumentNullException("sortPredicate");

            var query = this._session.Query<TAggregateRoot>()
                .Where(specification.Expression);

            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            int totalCount = 0;
            int totalPages = 0;
            List<TAggregateRoot> pagedData = null;
            PagedResult<TAggregateRoot> result = null;

            switch (sortOrder)
            {
                case Storage.SortOrder.Ascending:
                    totalCount = query.ToFutureValue(x => x.Count()).Value;
                    totalPages = (totalCount + pageSize - 1) / pageSize;
                    pagedData = query.OrderBy(sortPredicate).Skip(skip).Take(take).ToFuture().ToList();
                    result = new PagedResult<TAggregateRoot>(totalCount, totalPages, pageSize, pageNumber, pagedData);
                    break;
                case Storage.SortOrder.Descending:
                    totalCount = query.ToFutureValue(x => x.Count()).Value;
                    totalPages = (totalCount + pageSize - 1) / pageSize;
                    pagedData = query.OrderByDescending(sortPredicate).Skip(skip).Take(take).ToFuture().ToList();
                    result = new PagedResult<TAggregateRoot>(totalCount, totalPages, pageSize, pageNumber, pagedData);
                    break;
                default:
                    break;

            }
           
            Commit();
            return result;
        }

        public TAggregateRoot Find<TAggregateRoot>(Specifications.ISpecification<TAggregateRoot> specification) where TAggregateRoot : class, IAggregateRoot
        {
            EnsureSession();
            var result = this._session.Query<TAggregateRoot>().Where(specification.Expression).FirstOrDefault();
            
            Commit();
            return result;
        }

        #endregion 
    }
}
