using System.Collections.Generic;
using System.Configuration;

namespace Common.NoSql
{
    public class CacheKeyValueFactory 
    {

        private Mongo.CacheKeyValue _cacheKeyValue;
        public CacheKeyValueFactory() 
        {
            this._cacheKeyValue = new Mongo.CacheKeyValue();
        }

        public bool Add(string key, object value)
        {
            return this._cacheKeyValue.Add(key, value);
        }

        public void Update(string key, object value)
        {
            this._cacheKeyValue.Update(key, value);
        }

        public void Remove(string key)
        {
            this._cacheKeyValue.Remove(key);
        }

        public T GetAndCast<T>(string key)
        {
            return this._cacheKeyValue.GetAndCast<T>(key);
        }

        public IEnumerable<string> GetAllKeys()
        {
            return this._cacheKeyValue.GetAllKeys();
        }

        public bool DeleteAll()
        {
            return this._cacheKeyValue.DeleteAll();
        }

        private bool Exists(string key)
        {
            return this._cacheKeyValue.Exists(key);
        }

        public void RegisterClassMap<T>()
        {

        }

        private string CacheType()
        {
            return ConfigurationManager.AppSettings["CacheType"] ?? string.Empty;
        }
    }
}
