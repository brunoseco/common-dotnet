using Common.Domain.Interfaces;
using Common.Infrastructure.Log;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Cache
{
    public class MemCachedComponent : ICache
    {
        private readonly MemcachedClient cache;

        public MemCachedComponent()
        {
            this.cache = new MemcachedClient();
            this.EnableLogs();
            this.Start();
        }

        private void EnableLogs()
        {
            var log = ConfigurationManager.AppSettings["logs"].ToUpperCase();
            if (log == "MEMCACHED" || log == "true")
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("log-memcached-{0}.log", DateTime.Now.ToString("dd-MM-yyyy")));
                LogManager.AssignFactory(new Log4NetFactory());
                LogManager.AssignFactory(new DiagnosticsLogFactory(path));
            }
        }

        public bool Add(string key, object value)
        {
            var added = cache.Store(StoreMode.Set, key, value);
            return added;
        }

        public bool Update(string key, object value)
        {
            var replaced = cache.Store(StoreMode.Replace, key, value);
            return replaced;
        }

        public bool Remove(string key)
        {
            return cache.Remove(key);
        }

        public object Get(string key)
        {
            var result = cache.Get(key);
            return result;
        }

        public bool ExistsKey(string key)
        {
            var result = this.Get(key);
            return result.IsNotNull();
        }

        public void Start()
        {
            if (!this.ExistsKey("PING"))
                this.Add("PING", "POING");
        }

        public bool IsUp()
        {
            return this.ExistsKey("PING");
        }
    }
}
