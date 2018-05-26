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
    class CacheProfileException : ICacheProfileException
    {
        private string _connectionString;
        private string _collection;
        public CacheProfileException() 
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["DbCache"].ConnectionString;
            this._collection = "PersistProfileException";
        }

        public void Add(string roleId, int externalId, int exceptionGroupId, int toolId, object value)
        {
            try
            {
                var selectSQL = string.Format("Select Top 1 RoleId from {0} where RoleId='{1}'", this._collection, roleId);
                var roles = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);
                if (!roles.Any())
                {
                    var valueSerializer = value.SerializeObjectWithIgnore();
                    var insertSQL = string.Format("Insert into {0} Values(@RoleId,@ExternalId,@exceptionGroupId,@Value)", this._collection);

                    AdoNetHelper.ExecuteNonQuery(insertSQL, this._connectionString, new
                    {
                        RoleId = roleId,
                        ExternalId = externalId,
                        ExceptionGroupId = exceptionGroupId,
                        Value = valueSerializer
                    }, commandType: System.Data.CommandType.Text);
                }
            }
            catch { }
        }

        public void RemoveAll()
        {
            var deleteSQL = string.Format("Delete from {0}", this._collection);
            AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);

        }

        public IEnumerable<object> GetAndCast<T>(IEnumerable<int> externalsId, int exceptionGroupId)
        {
            var idClauses = String.Join(",", externalsId);
            var selectSQL = string.Format("Select RoleId,ExternalId,Name,Value from {0} where ExternalId in({1})", this._collection, idClauses);
            return this.GetByCommandText<T>(selectSQL);

        }

        private List<object> GetByCommandText<T>(string selectSQL)
        {
            var roles = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);
            var result = new List<dynamic>();

            foreach (var item in roles)
            {
                var itemDeserialize = JsonConvert.DeserializeObject<T>(item);
                result.Add(itemDeserialize);
            }

            return result;
        }

        IEnumerable<T> ICacheProfileException.GetAndCast<T>(IEnumerable<int> externalsId, int exceptionGroupId)
        {
            var idClauses = String.Join(",", externalsId);
            var selectSQL = string.Format("Select RoleId,ExternalId,Name,Value from {0} where ExternalId in({1})", this._collection, idClauses);
            return (IEnumerable<T>)this.GetByCommandText<T>(selectSQL);
        }

        public void RegisterClassMap<T>()
        {
            
        }
    }
}
