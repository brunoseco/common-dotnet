using Common.Domain.Interfaces;
using Common.NoSql.Models;
using Common.NoSql.Repository;
using Common.NoSql.Repository.Mongo;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.NoSql.Mongo
{


    class CacheProfileToolUser : NoSqlBaseMongo, ICacheProfileToolUser
    {
        private MongoRepository<PersistProfileToolUser> rep;
        public CacheProfileToolUser() : base(EMongoServerType.Profile)
        {
            this.rep = new MongoRepository<PersistProfileToolUser>(EMongoServerType.Profile);
            this.RegisterClassMap<PersistProfileToolUser>();
            this._isLive = base.IsUp();
        }

        public void Add(string userId, int externalId, int userGroupId, object value)
        {
            try
            {
                if (this._isLive)
                {
                    var toolUser = this.rep.GetByClauses(_ => _.ExternalId == externalId && _.UserGroupId == userGroupId);
                    if (!toolUser.Any())
                        this.rep.Add(new PersistProfileToolUser
                        {
                            UserId = userId,
                            ExternalId = externalId,
                            UserGroupId = userGroupId,
                            Value = value,
                        });
                }
            }
            catch { }
        }

        public void RemoveAll()
        {
            if (this._isLive)
                this.rep.DeleteAll();
        }


        public void RemoveFromUserId(string userId)
        {
            if (this._isLive)
                this.rep.Delete(_ => _.UserId == userId);
        }

        public void RemoveFromUserGroupId(int userGroupId)
        {
            if (this._isLive)
                this.rep.Delete(_ => _.UserGroupId == userGroupId);
        }

        public IEnumerable<T> GetAndCast<T>(int externalId, int userGroupId)
        {
            try
            {
                if (this._isLive)
                {
                    var persists = this.rep
                        .GetByClauses(_ => _.ExternalId == externalId && _.UserGroupId == userGroupId)
                        .Select(_ => _.Value);

                    return persists.SelectMany(_ => (IEnumerable<T>)_);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<T> GetAndCast<T>(string userId)
        {
            try
            {
                if (this._isLive)
                {
                    var persists = this.rep
                        .GetByClauses(_ => _.UserId == userId)
                        .Select(_ => _.Value);

                    return (IEnumerable<T>)persists;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
