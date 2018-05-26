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

    class CacheProfile : NoSqlBaseMongo, ICacheProfile
    {
        private MongoRepository<PersistProfile> rep;
        public CacheProfile() : base(EMongoServerType.Profile)
        {
            this.rep = new MongoRepository<PersistProfile>(EMongoServerType.Profile);
            this.RegisterClassMap<PersistProfile>();
            this._isLive = base.IsUp();

        }

        public void Add(string roleId, int externalId, string name, object value)
        {
            try
            {
                if (this._isLive)
                {
                    var roles = this.rep.GetByClauses(_ => _.RoleId == roleId);

                    if (!roles.Any())
                        this.rep.Add(new PersistProfile
                        {
                            RoleId = roleId,
                            ExternalId = externalId,
                            Name = name,
                            Value = value
                        });
                }
            }
            catch (Exception)
            {

            }
        }

        public void Update(string roleId, object value)
        {
            if (this._isLive)
            {
                var alvos = this.rep.GetByClauses(_ => _.RoleId == roleId);
                foreach (var item in alvos)
                {
                    this.Remove(roleId);
                    this.Add(item.RoleId, item.ExternalId, item.Name, value);
                }
            }
        }

        public void Remove(string roleId)
        {
            if (this._isLive)
                this.rep.Delete(_ => _.RoleId == roleId);
        }
        public IEnumerable<T> GetAndCast<T>(IEnumerable<int> externalsId)
        {
            try
            {
                if (this._isLive)
                {
                    var persists = this.rep
                        .GetByClauses(_ => externalsId.Contains(_.ExternalId))
                        .Select(_ => _.Value);

                    return persists.SelectMany(_ => (IEnumerable<T>)_);

                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public IEnumerable<T> GetAndCast<T>(IEnumerable<string> rolesId)
        {
            try
            {
                if (this._isLive)
                {
                    var persists = this.rep
                        .GetByClauses(_ => rolesId.Contains(_.RoleId))
                       .Select(_ => _.Value);

                    return persists.SelectMany(_ => (IEnumerable<T>)_);

                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

    }
}
