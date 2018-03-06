using Common.Domain.Interfaces;
using Common.NoSql;
using Common.NoSql.Mongo;
using Enyim.Caching;
using System;
using System.Configuration;
using System.IO;

namespace Common.Infrastructure.Cache
{
    public class RedisCachedComponent : ICache
    {
        private readonly RedisClient cache;
        private readonly CacheKeyValueFactory cacheKeyValue;
        public RedisCachedComponent()
        {
    
            this.cache = new RedisClient();
            this.EnableLogs();
            this.Start();
            this.cacheKeyValue = new CacheKeyValueFactory();

        }

        public void RegisterClassMap<T>()
        {
            this.cacheKeyValue.RegisterClassMap<T>();
        }


        private void EnableLogs()
        {

        }

        public bool Add(string key, object value)
        {
            try
            {
                return cache.Add(key, value);
            }
            catch
            {
                return false;
            }

        }
        public bool Add(string key, object value, bool persists)
        {

            this.cacheKeyValue.Add(key, value);

            return true;
        }
        public bool Add(string key, object value, TimeSpan expire)
        {
            return cache.Add(key, value, expire);
        }

        public bool Update(string key, object value)
        {

            cache.Remove(key);
            return true;
        }
        public bool Update(string key, object value, bool persists)
        {

            this.cacheKeyValue.Update(key, value);

            return true;
        }
        public bool Update(string key, object value, TimeSpan expire)
        {
            return cache.Add(key, value);
        }
        public bool Remove(string key)
        {
            return cache.Remove(key);
        }
        public bool Remove(string key, bool persists)
        {
            this.cacheKeyValue.Remove(key);
            return this.Remove(key);
        }
        public T GetAndCast<T>(string key)
        {
            return cache.GetAndCast<T>(key);
        }
        public T GetAndCast<T>(string key, bool persists)
        {

            var result = this.GetAndCast<T>(key);
            if (result.IsNull())
            {
                var resultPersists = this.cacheKeyValue.GetAndCast<T>(key);
                if (resultPersists.IsNotNull())
                {
                    this.Add(key, resultPersists);
                    return (T)resultPersists;
                }
            }
            return result;
        }

        public object Get(string key)
        {
            return this.GetAndCast<object>(key);
        }

        public bool ExistsKey(string key)
        {
            return this.ExistsKey<object>(key);
        }

        public bool ExistsKey<T>(string key)
        {
            var result = this.GetAndCast<T>(key);
            return result.IsNotNull();
        }
        public bool ExistsKey<T>(string key, bool persists)
        {
            var result = this.GetAndCast<T>(key, persists);
            return result.IsNotNull();
        }
        public void Start()
        {
            if (!this.ExistsKey<string>("PING"))
                this.Add("PING", "POING");
        }
        public bool IsUp()
        {
            return this.ExistsKey<string>("PING");
        }
    }
}
