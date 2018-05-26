using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Cache
{
    public class FactoryCache : ICache
    {
        private static ICache cache;

        public FactoryCache()
        {

            if (CacheType().ToUpper() == "REDIS")
            {
                if (cache.IsNull())
                    cache = new RedisCachedComponent();

            }

            if (CacheType().ToUpper() == "BDCACHE")
            {
                if (cache.IsNull())
                    cache = new BdCachedComponent();

            }

            if (CacheType().ToUpper() == "TABLE")
            {
                if (cache.IsNull())
                    cache = new TableCachedComponent();

            }

            if (CacheType().ToUpper() == "MEMCACHEDJSON")
                cache = new MemCachedComponentJson();

            if (cache.IsNull())
                cache = new MemCachedComponent();

        }

        private static string CacheType()
        {
            return ConfigurationManager.AppSettings["CacheType"] ?? string.Empty;
        }

      

        public bool Add(string key, object value)
        {
            return cache.Add(key, value);
        }
        public bool Add(string key, object value, bool persist)
        {
            return cache.Add(key, value, persist);
        }
        public bool Add(string key, object value, TimeSpan expire)
        {
            return cache.Add(key, value, expire);
        }

        public bool Update(string key, object value)
        {
            return cache.Update(key, value);
        }
        public bool Update(string key, object value, TimeSpan expire)
        {
            return cache.Update(key, value, expire);
        }
        public bool Update(string key, object value, bool persist)
        {
            return cache.Update(key, value, persist);
        }

        public bool Remove(string key)
        {
            return cache.Remove(key);
        }
        public bool Remove(string key, bool persist)
        {
            return cache.Remove(key, persist);
        }

        public bool ExistsKey(string key)
        {
            return this.ExistsKey<object>(key);
        }
        public bool ExistsKey<T>(string key)
        {
            return cache.ExistsKey<T>(key);
        }
        public bool ExistsKey<T>(string key, bool persist)
        {
            return cache.ExistsKey<T>(key, persist);
        }

        public void Start()
        {
            cache.Start();
        }

        public bool IsUp()
        {
            return cache.IsUp();
        }

        public T GetAndCast<T>(string key)
        {
            return cache.GetAndCast<T>(key);
        }

        public T GetAndCast<T>(string key, bool persist)
        {
            return cache.GetAndCast<T>(key,persist);
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }
    }
}
