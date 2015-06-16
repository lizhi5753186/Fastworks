using Fastworks.Specifications;
using Fastworks.Storage;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fastworks.Repositories.NHibernate
{
    public interface INHibernateContext : IRepositoryContext
    {
        TAggregateRoot GetByKey<TAggregateRoot>(object key)
            where TAggregateRoot : class, IAggregateRoot;

        IQueryable<TAggregateRoot> FindAll<TAggregateRoot>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
           where TAggregateRoot : class, IAggregateRoot;

        PagedResult<TAggregateRoot> FindAll<TAggregateRoot>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, Storage.SortOrder sortOrder, int pageNumber, int pageSize)
            where TAggregateRoot : class, IAggregateRoot;

        TAggregateRoot Find<TAggregateRoot>(ISpecification<TAggregateRoot> specification)
            where TAggregateRoot : class, IAggregateRoot;
    }
}
