using Common.Domain.Interfaces;
using System.Collections.Generic;
using System.Configuration;

namespace Common.NoSql
{


    public class CacheProfileExceptionFactory : ICacheProfileException
    {
        private ICacheProfileException _cacheProfileException;

        public CacheProfileExceptionFactory()
        {
            this._cacheProfileException = new Mongo.CacheProfileException();
        }
        public void Add(string roleId, int externalId, int toolId, int exceptionGroupId, object value)
        {
            this._cacheProfileException.Add(roleId, externalId, toolId, exceptionGroupId, value);
        }

        public IEnumerable<T> GetAndCast<T>(IEnumerable<int> externalsId, int exceptionGroupId)
        {
            return this._cacheProfileException.GetAndCast<T>(externalsId, exceptionGroupId);
        }

        public void RegisterClassMap<T>()
        {
            this._cacheProfileException.RegisterClassMap<T>();
        }

        public void RemoveAll()
        {
            this._cacheProfileException.RemoveAll();
        }


        private string CacheType()
        {
            return ConfigurationManager.AppSettings["CacheType"] ?? string.Empty;
        }
    }
}
