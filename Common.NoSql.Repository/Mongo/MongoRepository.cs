using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.NoSql.Repository.Mongo
{
    public class MongoRepository<T> : NoSqlBaseMongo
    {
        protected string _collectionName;
        protected IMongoCollection<T> _collection;
        public MongoRepository(EMongoServerType mongoServerType) : base(mongoServerType)
        {
            this._collectionName = typeof(T).Name;
            this._collection = this._database.GetCollection<T>(this._collectionName);
        }

  
        public IEnumerable<T> GetByClauses(Expression<Func<T, bool>> filter)
        {
            var result = this._collection.Find<T>(filter).ToList();
            return result;
        }

        public T GetByClausesLast(Expression<Func<T, bool>> filter)
        {
            var result = this._collection.Find<T>(filter).ToList().LastOrDefault();
            return result;
        }

        public bool Exists(Expression<Func<T, bool>> filter)
        {
            var result = this._collection.Find<T>(filter).Count() > 0;
            return result;
        }
        public IEnumerable<T> GetAll()
        {
            var result = this._collection.Find<T>(_ => true).ToList();
            return result;
        }

        public T GetById(Expression<Func<T, bool>> filter)
        {
            var result = this._collection
                .Find<T>(filter)
                .SingleOrDefault();
            return result;
        }

        public void Add(T document)
        {
            this._collection.InsertOne(document);
        }

        public void Delete(Expression<Func<T, bool>> filter)
        {
            this._collection.DeleteMany<T>(filter);
        }

        public void DeleteAll()
        {
            this._collection.DeleteMany<T>(_ => true);

        }
    }
}
