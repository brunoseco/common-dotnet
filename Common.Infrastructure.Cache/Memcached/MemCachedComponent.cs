using Common.Domain.Interfaces;
using Common.NoSql;
using Common.NoSql.Mongo;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;

namespace Common.Infrastructure.Cache
{
    public class MemCachedComponent : ICache
    {
        private readonly MemcachedClient cache;

        public MemCachedComponent()
        {

            try
            {
                this.cache = new MemcachedClient();
            }
            catch (Exception)
            {
                var tmc = new MemcachedClientConfiguration();
                tmc.AddServer(string.Format("{0}:{1}", "localhost", "11211"));
                tmc.Protocol = MemcachedProtocol.Binary;
                tmc.SocketPool.ReceiveTimeout = new TimeSpan(0, 0, 4);
                tmc.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, 4);
                tmc.KeyTransformer = new DefaultKeyTransformer();
                this.cache = new MemcachedClient(tmc);
            }

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
            var add = cache.Store(StoreMode.Add, key, value);
            return add;
        }

        public bool Add(string key, object value, TimeSpan expire)
        {
            return cache.Store(StoreMode.Add, key, value, expire);
        }

        public bool Add(string key, object value, bool persists)
        {
            return this.Add(key, value);
        }

        public bool Update(string key, object value)
        {
            return cache.Store(StoreMode.Replace, key, value);
        }
        public bool Update(string key, object value, TimeSpan expire)
        {
            return cache.Store(StoreMode.Replace, key, value, expire);
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

        public object Get(string key)
        {
            return this.GetAndCast<object>(key);
        }

        public T GetAndCast<T>(string key)
        {
            var result = cache.Get(key);
            if (result.IsNull())
                return default(T);

            return (T)result;
        }

        public T GetAndCast<T>(string key, bool persist)
        {
            var result = this.GetAndCast<T>(key);
            if (result.IsNull())
                return default(T);

            return (T)result;
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
            var result = this.cache.Get(key);
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
