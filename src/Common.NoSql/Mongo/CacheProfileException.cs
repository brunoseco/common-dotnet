using Common.Domain.Interfaces;
using Common.NoSql.Models;
using Common.NoSql.Repository;
using Common.NoSql.Repository.Mongo;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.NoSql.Mongo
{

    class CacheProfileException : NoSqlBaseMongo, ICacheProfileException
    {
        private MongoRepository<PersistProfileException> rep;
        public CacheProfileException() : base(EMongoServerType.Profile)
        {
            this.rep = new MongoRepository<PersistProfileException>(EMongoServerType.Profile);
            this.RegisterClassMap<PersistProfileException>();
            this._isLive = base.IsUp();
        }

        public void Add(string roleId, int externalId, int exceptionGroupId, int toolId, object value)
        {
            try
            {
                if (this._isLive)
                {
                    var roles = this.rep.GetByClauses(_ => _.RoleId == roleId);
                    if (!roles.Any())
                    {
                        this.rep.Add(new PersistProfileException
                        {
                            RoleId = roleId,
                            ExternalId = externalId,
                            ToolId = toolId,
                            ExceptionGroupId = exceptionGroupId,
                            Value = value
                        });
                    }
                }
            }
            catch { }
        }

        public void RemoveAll()
        {
            if (this._isLive)
                this.rep.DeleteAll();
        }

        public IEnumerable<T> GetAndCast<T>(IEnumerable<int> externalsId, int exceptionGroupId)
        {
            try
            {
                if (this._isLive)
                {
                    var persists = this.rep
                        .GetByClauses(_ => externalsId.Contains(_.ExternalId) && _.ExceptionGroupId == exceptionGroupId)
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
