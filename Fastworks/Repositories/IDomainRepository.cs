using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastworks.Repositories
{
    public interface IDomainRepository : IUnitOfWork,  IDisposable
    {
        TAggregateRoot Get<TAggregateRoot>(Guid id)
          where TAggregateRoot : class, ISourcedAggregateRoot, new();

        void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
           where TAggregateRoot : class, ISourcedAggregateRoot, new();
    }
}
