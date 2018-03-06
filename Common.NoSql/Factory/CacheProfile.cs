using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Common.NoSql
{

    public class CacheProfileFactory : ICacheProfile
    {

        private ICacheProfile _cacheProfile;
        public CacheProfileFactory()
        {
            this._cacheProfile = new Mongo.CacheProfile();
        }

        public void Add(string roleId, int externalId, string name, object value)
        {
            this._cacheProfile.Add(roleId, externalId, name, value);
        }

        public IEnumerable<T> GetAndCast<T>(IEnumerable<int> externalsId)
        {
            return this._cacheProfile.GetAndCast<T>(externalsId);
        }

        public IEnumerable<T> GetAndCast<T>(IEnumerable<string> rolesId)
        {
            return this._cacheProfile.GetAndCast<T>(rolesId);
        }

        public void RegisterClassMap<T>()
        {
            this._cacheProfile.RegisterClassMap<T>();
        }

        public void Remove(string roleId)
        {
            this._cacheProfile.Remove(roleId);
        }

        public void Update(string roleId, object value)
        {
            this._cacheProfile.Update(roleId, value);
        }


        private string CacheType()
        {
            return ConfigurationManager.AppSettings["CacheType"] ?? string.Empty;
        }
    }
}
