using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mailbox.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Mailbox.Dal
{

    public abstract class MongoDbDataContext
    {
        private readonly MongoDatabase _db;

        public MongoClient Client { get; private set; }

        public MongoServer Server { get; private set; }

        public string DatabaseName { get; private set; }

        public MongoDatabase Db
        {
            get
            {
                return this._db;
            }
        }

        protected MongoDbDataContext(string databaseName, string uri)
        {
            this.DatabaseName = databaseName;
            if (string.IsNullOrWhiteSpace(this.DatabaseName))
                throw new InvalidOperationException("You must set the database name.");
            if (uri == null)
                throw new ArgumentNullException("serverName");

            var setting = MongoClientSettings.FromUrl(new MongoUrl(uri));

            this.Client = new MongoClient(setting);
            this.Server = this.Client.GetServer();
            this._db = this.Server.GetDatabase(this.DatabaseName);
        }

        protected MongoCollection<T> GetMongoCollection<T>()
        {
            return this.Db.GetCollection<T>(MongoDbDataContext.GetCollectionName<T>());
        }

        protected IQueryable<T> GetCollection<T>()
        {
            return LinqExtensionMethods.AsQueryable<T>(this.GetMongoCollection<T>());
        }

        public virtual void Delete<T>(ObjectId entityId)
        {
            string collectionName = MongoDbDataContext.GetCollectionName<T>();
            this.Delete(entityId, collectionName);
        }

        public virtual void Delete(ObjectId entityId, string collectionName)
        {
            IMongoQuery query = Query.EQ("_id", (BsonValue)entityId);
            this.Db.GetCollection<object>(collectionName).Remove(query);
        }

        public virtual void Delete<T>(T entity) where T : IMongoEntity
        {
            string collectionName = MongoDbDataContext.GetCollectionName<T>();
            this.Delete<T>(entity, collectionName);
        }

        public virtual void Delete<T>(T entity, string collectionName) where T : IMongoEntity
        {
            this.Delete(entity._id, collectionName);
        }

        public virtual void Save<T>(T entity) where T : class
        {
            string collectionName = MongoDbDataContext.GetCollectionName<T>();
            this.Save<T>(entity, collectionName);
        }

        public virtual void Save<T>(T entity, string collectionName) where T : class
        {
            WriteConcernResult writeConcernResult = this.Db.GetCollection<T>(collectionName).Save(entity);
            if (writeConcernResult.HasLastErrorMessage)
                throw new MongoContextException(writeConcernResult.LastErrorMessage);
        }

        protected static string GetCollectionName<T>()
        {
            return typeof(T).Name;
        }

        public void Clear<T>()
        {
            WriteConcernResult commandResult = (WriteConcernResult)
                this.Db.GetCollection<T>(MongoDbDataContext.GetCollectionName<T>()).RemoveAll();

            if (commandResult.HasLastErrorMessage)
                throw new MongoContextException(commandResult.LastErrorMessage);
        }
    }
}
