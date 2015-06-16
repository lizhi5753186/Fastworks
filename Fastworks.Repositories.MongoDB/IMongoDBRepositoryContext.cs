using MongoDB.Driver;
using System;

namespace Fastworks.Repositories.MongoDB
{
    public interface IMongoDBRepositoryContext : IRepositoryContext
    {
        IMongoDBRepositoryContextSettings Settings { get; }

        MongoCollection GetCollectionForType(Type type);
    }
}
