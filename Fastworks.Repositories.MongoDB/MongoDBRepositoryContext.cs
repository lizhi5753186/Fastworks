using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fastworks.Repositories.MongoDB
{
    public class MongoDBRepositoryContext : RepositoryContext, IMongoDBRepositoryContext
    {
        #region Private Fields
        private readonly MongoServer _server;
        private readonly MongoDatabase _database;
        private readonly IMongoDBRepositoryContextSettings _settings;
        private readonly object syncObj = new object();
        private readonly Dictionary<Type, MongoCollection> _mongoCollections = new Dictionary<Type, MongoCollection>();

        #endregion 

        #region Ctor
        public MongoDBRepositoryContext(IMongoDBRepositoryContextSettings settings)
        {
            this._settings = settings;
            this._server = new MongoServer(settings.ServerSettings);
            _database = _server.GetDatabase(settings.DatabaseName, settings.GetDatabaseSettings(this._server));
        }

        #endregion 

        #region DisposableObject Members
        protected override void Dispose(bool disposing = false)
        {
            if (disposing)
            {
                _server.Disconnect();
            }

            base.Dispose(disposing);
        }

        #endregion 
    
        #region IMongoDBRepositoryContext Members

        public IMongoDBRepositoryContextSettings Settings
        {
            get { return _settings; }
        }

        public MongoCollection GetCollectionForType(Type type)
        {
            lock (syncObj)
            {
                if (this._mongoCollections.ContainsKey(type))
                    return this._mongoCollections[type];
                else
                {
                    MongoCollection mongoCollection = null;
                    if (_settings.MapTypeToCollectionName != null)
                        mongoCollection = this._database.GetCollection(_settings.MapTypeToCollectionName(type));
                    else
                        mongoCollection = this._database.GetCollection(type.Name);

                    this._mongoCollections.Add(type, mongoCollection);
                    return mongoCollection;
                }
            }
        }

        #region  IUnitOfWork Members
       
        public override void Commit()
        {
            lock (syncObj)
            {
                foreach (var newObj in this.NewCollection)
                {
                    MongoCollection collection = this.GetCollectionForType(newObj.GetType());
                    collection.Insert(newObj);
                }
                foreach (var modifiedObj in this.ModifiedCollection)
                {
                    MongoCollection collection = this.GetCollectionForType(modifiedObj.GetType());
                    collection.Save(modifiedObj);
                }
                foreach (var delObj in this.DeletedCollection)
                {
                    Type objType = delObj.GetType();
                    PropertyInfo propertyInfo = objType.GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (propertyInfo == null)
                        throw new InvalidOperationException("Cannot delete an object which doesn't contain an ID property.");
                    Guid id = (Guid)propertyInfo.GetValue(delObj, null);
                    MongoCollection collection = this.GetCollectionForType(objType);
                    IMongoQuery query = Query.EQ("_id", id);
                    collection.Remove(query);
                }

                this.ClearRegistrations();
                this.Committed = true;
            }
        }

        public override void Rollback()
        {
            this.Committed = false;
        }
        #endregion 

        #endregion 
    }
}
