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
            if (cache.IsNull())
                cache = new MemCachedComponent();
        }

        public bool Add(string key, object value)
        {
            return cache.Add(key, value);
        }

        public bool Update(string key, object value)
        {
            return cache.Update(key, value);
        }

        public bool Remove(string key)
        {
            return cache.Remove(key);
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public bool ExistsKey(string key)
        {
            return cache.ExistsKey(key);
        }

        public void Start()
        {
            cache.Start();
        }

        public bool IsUp()
        {
            return cache.IsUp();
        }


    }
}
