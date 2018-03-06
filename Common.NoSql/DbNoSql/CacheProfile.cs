using Common.Domain.Extensions;
using Common.Domain.Interfaces;
using Common.Infrastructure.ORM.Helpers;
using Common.NoSql.Models;
using Common.NoSql.Repository.Mongo;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Common.NoSql.DbNoSql
{
    class CacheProfile : ICacheProfile
    {
        private string _connectionString;
        private string _collection;
        public CacheProfile()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["DbCache"].ConnectionString;
            this._collection = "PersistProfile";

        }

        public void RegisterClassMap<T>()
        {

        }

        public void Add(string roleId, int externalId, string name, object value)
        {
            try
            {
                var selectSQL = string.Format("Select Top 1 RoleId from {0} where RoleId='{1}'", this._collection, roleId);
                var roles = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);
                if (!roles.Any())
                {
                    var valueSerializer = value.SerializeObjectWithIgnore();
                    var insertSQL = string.Format("Insert into {0} Values(@RoleId,@ExternalId,@Name,@Value)", this._collection);

                    AdoNetHelper.ExecuteNonQuery(insertSQL, this._connectionString, new
                    {
                        RoleId = roleId,
                        ExternalId = externalId,
                        Name = name,
                        Value = valueSerializer
                    }, commandType: System.Data.CommandType.Text);
                }
            }
            catch { }
        }

        public void Update(string roleId, object value)
        {

        }

        public void Remove(string roleId)
        {
            var deleteSQL = string.Format("delete from {0} where RoleId='{1}'", this._collection, roleId);
            AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);
        }
        public IEnumerable<T> GetAndCast<T>(IEnumerable<int> externalsId)
        {
            var idClauses = String.Join(",", externalsId);
            var selectSQL = string.Format("Select RoleId,ExternalId,Name,Value from {0} where ExternalId in({1})", this._collection, idClauses);
            return this.GetByCommandText<T>(selectSQL);
        }
        public IEnumerable<T> GetAndCast<T>(IEnumerable<string> rolesId)
        {
            var idClauses = String.Join(",", string.Format("'{0}'", rolesId));
            var selectSQL = string.Format("Select RoleId,ExternalId,Name,Value from {0} where RolesId in({1})", this._collection, idClauses);
            return this.GetByCommandText<T>(selectSQL);
        }

        private IEnumerable<T> GetByCommandText<T>(string selectSQL)
        {
            var roles = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);
            var result = new List<T>();

            foreach (var item in roles)
            {
                var itemDeserialize = JsonConvert.DeserializeObject<IEnumerable<T>>(item.Value);
                result.AddRange(itemDeserialize);
            }

            return result;
        }

    }
}
