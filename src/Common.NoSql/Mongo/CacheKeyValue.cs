using Common.Domain;
using Common.NoSql.Models;
using Common.NoSql.Repository;
using Common.NoSql.Repository.Mongo;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.NoSql.Mongo
{
    class CacheKeyValue : NoSqlBaseMongo
    {
        private MongoRepository<PersistKeyValue> rep;
        public CacheKeyValue() : base(EMongoServerType.Profile)
        {
            this.rep = new MongoRepository<PersistKeyValue>(EMongoServerType.Profile);
            this.RegisterClassMap<PersistKeyValue>();
            this.RegisterClassMap<CurrentUser>();
            this._isLive = base.IsUp();
        }

        public bool Add(string key, object value)
        {
            try
            {
                if (this._isLive)
                {
                    if (!this.Exists(key))
                        this.rep.Add(new PersistKeyValue { key = key, Value = value });

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Update(string key, object value)
        {
            if (this._isLive)
            {
                this.Remove(key);
                this.Add(key, value);
            }
        }

        public void Remove(string key)
        {
            if (this._isLive)
                this.rep.Delete(_ => _.key == key);
        }

        public T GetAndCast<T>(string key)
        {
            try
            {
                if (this._isLive)
                {
                    var persists = this.rep.GetByClausesLast(_ => _.key == key);
                    if (persists.IsNotNull())
                        return (T)persists.Value;
                }

                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        public IEnumerable<string> GetAllKeys()
        {
            try
            {
                if (this._isLive)
                {
                    var result = this.rep.GetAll();
                    if (result.IsNotNull())
                        result.Select(_ => _.key);
                }

                return null;
            }
            catch
            {
                return null;
            }

        }

        public bool DeleteAll()
        {
            try
            {
                if (this._isLive)
                {
                    this.rep.DeleteAll();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Exists(string key)
        {
            try
            {
                if (this._isLive)
                {
                    var persists = this.rep.Exists(_ => _.key == key);
                    return persists;
                }

                return default(bool);
            }
            catch
            {
                return default(bool);
            }
        }
    }
}
