using Common.Domain.Extensions;
using Common.Domain.Interfaces;
using Common.NoSql;
using Common.NoSql.Mongo;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;

namespace Common.Infrastructure.Cache
{
    public class MemCachedComponentJson : ICache
    {
        private readonly MemcachedClient cache;

        public MemCachedComponentJson()
        {

            this.cache = new MemcachedClient();
            this.EnableLogs();
            this.Start();

        }

        public void RegisterClassMap<T>()
        {

        }
        private void EnableLogs()
        {
            var log = ConfigurationManager.AppSettings["logs"].ToUpper();
            if (log == "MEMCACHED")
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("log-memcached-{0}.log", DateTime.Now.ToString("dd-MM-yyyy")));
                LogManager.AssignFactory(new Log4NetFactory());
                LogManager.AssignFactory(new DiagnosticsLogFactory(path));
            }
        }

        public bool Add(string key, object value)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();
            return cache.Store(StoreMode.Add, key, valueSerializer);
        }
        public bool Add(string key, object value, TimeSpan expire)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();
            return cache.Store(StoreMode.Add, key, valueSerializer, expire);
        }
        public bool Add(string key, object value, bool persists)
        {
            return this.Add(key, value);
        }

        public bool Update(string key, object value)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();
            return cache.Store(StoreMode.Replace, key, value);
        }
        public bool Update(string key, object value, TimeSpan expire)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();
            return cache.Store(StoreMode.Replace, key, valueSerializer, expire);
        }
        public bool Update(string key, object value, bool persists)
        {
            return this.Update(key, value);
        }

        public bool Remove(string key)
        {
            return this.Remove(key, false);
        }
        public bool Remove(string key, bool persists)
        {
            return this.cache.Remove(key);
        }

        public void FlushAll()
        {
            this.cache.FlushAll();
        }

        public T GetAndCast<T>(string key)
        {
            var result = cache.Get(key) as string;
            if (result.IsNullOrEmpaty())
                return default(T);

            return JsonConvert.DeserializeObject<T>(result);
        }

        public T GetAndCast<T>(string key, bool persist)
        {
            var result = cache.Get(key) as string;
            if (result.IsNull())
                return default(T);

            return JsonConvert.DeserializeObject<T>(result);
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
            return this.ExistsKey<T>(key, false);
        }
        public bool ExistsKey<T>(string key, bool persists)
        {
            var result = this.cache.Get(key) as string;
            return  result.IsNotNullOrEmpty();
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
