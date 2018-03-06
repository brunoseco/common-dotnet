using Common.Domain.Extensions;
using Common.Domain.Interfaces;
using Common.Infrastructure.ORM.Helpers;
using Common.NoSql.Models;
using Common.NoSql.Repository.Mongo;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Common.NoSql.DbNoSql
{
    class CacheProfileToolUser: ICacheProfileToolUser
    {

        private string _connectionString;
        private string _collection;
        public CacheProfileToolUser()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["DbCache"].ConnectionString;
            this._collection = "PersistProfileToolUser";
        }

        public void Add(string userId, int externalId, int userGroupId, object value)
        {
            try
            {
                var selectSQL = string.Format("Select Top 1 UserGroupId from {0} where ExternalId={1} and UserGroupId={2}", this._collection, externalId, userGroupId);
                var roles = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);
                if (!roles.Any())
                {
                    var valueSerializer = value.SerializeObjectWithIgnore();
                    var insertSQL = string.Format("Insert into {0} Values(@UserId,@ExternalId,@UserGroupId,@Value)", this._collection);

                    AdoNetHelper.ExecuteNonQuery(insertSQL, this._connectionString, new
                    {
                        UserId = userId,
                        ExternalId = externalId,
                        UserGroupId = userGroupId,
                        Value = valueSerializer,
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

        public void RemoveFromUserId(string userId)
        {
            var deleteSQL = string.Format("Delete from {0} where UserId='{1}'", this._collection, userId);
            AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);
        }

        public IEnumerable<T> GetAndCast<T>(int externalId, int userGroupId)
        {
            var selectSQL = string.Format("Select UserId, ExternalId, UserGroupId, Value from {0} where ExternalId={1} and UserGroupId={2}", this._collection, externalId, userGroupId);
            return this.GetByCommandText<T>(selectSQL);
        }

        public IEnumerable<T> GetAndCast<T>(string userId)
        {
            var selectSQL = string.Format("Select UserId, ExternalId, UserGroupId, Value from {0} where UserId={1}", this._collection, userId);
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

        public void RemoveFromUserGroupId(int userGroupId)
        {
            var deleteSQL = string.Format("Delete from {0} where UserGroupId={1}", this._collection, userGroupId);
            AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);
        }

        public void RegisterClassMap<T>()
        {
            
        }
    }
}
