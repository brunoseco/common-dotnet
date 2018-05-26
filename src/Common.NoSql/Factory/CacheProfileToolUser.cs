using Common.Domain.Interfaces;
using Common.NoSql.Models;
using Common.NoSql.Repository;
using Common.NoSql.Repository.Mongo;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Common.NoSql
{


    public class CacheProfileToolUserFactory : ICacheProfileToolUser
    {
        private ICacheProfileToolUser _cacheProfileToolUser;
        public CacheProfileToolUserFactory()
        {
            this._cacheProfileToolUser = new Mongo.CacheProfileToolUser();
        }
        public void Add(string userId, int externalId, int userGroupId, object value)
        {
            this._cacheProfileToolUser.Add(userId, externalId, userGroupId, value);
        }

        public IEnumerable<T> GetAndCast<T>(string userId)
        {
            return this._cacheProfileToolUser.GetAndCast<T>(userId);
        }

        public IEnumerable<T> GetAndCast<T>(int externalId, int userGroupId)
        {
            return this._cacheProfileToolUser.GetAndCast<T>(externalId, userGroupId);
        }

        public void RegisterClassMap<T>()
        {
            this._cacheProfileToolUser.RegisterClassMap<T>();
        }

        public void RemoveAll()
        {
            this._cacheProfileToolUser.RemoveAll();
        }

        public void RemoveFromUserGroupId(int userGroupId)
        {
            this._cacheProfileToolUser.RemoveFromUserGroupId(userGroupId);
        }

        public void RemoveFromUserId(string userId)
        {
            this._cacheProfileToolUser.RemoveFromUserId(userId);
        }


        private string CacheType()
        {
            return ConfigurationManager.AppSettings["CacheType"] ?? string.Empty;
        }
    }
}
