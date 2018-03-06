using Common.Domain.Interfaces;
using Common.Infrastructure.ORM.Helpers;
using Common.NoSql.Mongo;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;

namespace Common.Infrastructure.Cache
{
    public class BdCachedComponent : ICache
    {
        private string _connectionString;

        public BdCachedComponent()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["DbCache"].ConnectionString;
        }

     
        public bool Add(string key, object value)
        {
            var exists = this.ExecuteCommand(key) as string;
            if (exists.IsNotNullOrEmpty())
                return true;

            var valueSerializer = JsonConvert.SerializeObject(value);
            var insertSQL = string.Format("Insert into KeyValueCache Values(@Key,@value)");

            AdoNetHelper.ExecuteNonQuery(insertSQL, this._connectionString, new
            {
                Key = key,
                value = valueSerializer
            }, commandType: System.Data.CommandType.Text);

            return true;
        }
        public bool Add(string key, object value, bool persists)
        {
            return this.Add(key, value);
        }
        public bool Add(string key, object value, TimeSpan expire)
        {
            return this.Add(key, value);
        }
        public bool Update(string key, object value)
        {
            this.Remove(key);
            return this.Add(key, value);
        }
        public bool Update(string key, object value, bool persists)
        {
            this.Remove(key);
            return this.Add(key, value);
        }
        public bool Update(string key, object value, TimeSpan expire)
        {
            this.Remove(key);
            return this.Add(key, value);
        }

        public bool Remove(string key)
        {
            var deleteSQL = string.Format("delete from KeyValueCache where [Key]='{0}'", key);
            AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);
            return true;
        }
        public bool Remove(string key, bool persists)
        {
            return this.Remove(key);
        }
       
    
        public T GetAndCast<T>(string key)
        {
            var result = this.ExecuteCommand(key) as string;
            if (result.IsNotNullOrEmpty())
                return JsonConvert.DeserializeObject<T>(result.ToString());

            return default(T);
        }
        public T GetAndCast<T>(string key, bool persist)
        {
            return GetAndCast<T>(key);
        }
        public bool ExistsKey(string key)
        {
            return this.ExistsKey<object>(key);
        }
        public bool ExistsKey<T>(string key)
        {
            var result = this.GetAndCast<T>(key);
            return result.IsNotNull();
        }
        public bool ExistsKey<T>(string key, bool persists)
        {
            var result = this.GetAndCast<T>(key, persists);
            return result.IsNotNull();
        }
        public void Start()
        {
            if (!this.ExistsKey<string>("PING"))
                this.Add("PING", "POING");
        }
        public bool IsUp()
        {
            return this.ExistsKey<string>("PING");
        }

        private object ExecuteCommand(string key)
        {
            var selectSQL = string.Format("Select [Key],Value from KeyValueCache where [Key]='{0}'", key);
            var result = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);

            var value = string.Empty;
            foreach (var item in result)
                value = item.Value;

            return value;
        }

        public object Get(string key)
        {
            return this.GetAndCast<object>(key);
        }

    }
}
