using System;

namespace Fastworks.Repositories
{
    // 仓储上下文接口
    // 这里把传统的IUnitOfWork接口中方法分别在2个接口定义：一个是IUnitOfWork,另一个就是该接口
    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        // 用来标识仓储上下文
        Guid Id { get; }

        void RegisterNew<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot;

        void RegisterModified<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot;

        void RegisterDeleted<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot;
    }
}
