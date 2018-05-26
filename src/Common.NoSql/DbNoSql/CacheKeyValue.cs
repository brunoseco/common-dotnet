using Common.Domain.Extensions;
using Common.Infrastructure.ORM.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;


namespace Common.NoSql.DbNoSql
{
    class CacheKeyValue
    {
        private string _connectionString;
        private string _collection;
        public CacheKeyValue() 
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["DbCache"].ConnectionString;
            this._collection = "PersistKeyValue";
        }

        public bool Add(string key, object value)
        {
            if (this.Exists(key))
                return true;

            var valueSerializer = value.SerializeObjectWithIgnore();
            var insertSQL = string.Format("Insert into {0} Values(@Key,@value)", this._collection);

            AdoNetHelper.ExecuteNonQuery(insertSQL, this._connectionString, new
            {
                Key = key,
                value = valueSerializer
            }, commandType: System.Data.CommandType.Text);

            return true;

        }

        public void Update(string key, object value)
        {
            this.Remove(key);
            this.Add(key, value);
        }

        public void Remove(string key)
        {
            var deleteSQL = string.Format("Delete from {0} where [Key]='{1}'", this._collection, key);
            AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);

        }

        public T GetAndCast<T>(string key)
        {
            var selectSQL = string.Format("Select [Key],Value from {0} where [Key]='{1}'", this._collection, key);
            var result = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);

            var value = string.Empty;
            foreach (var item in result)
                value = item.Value;

            return JsonConvert.DeserializeObject<T>(value);
        }


        public IEnumerable<string> GetAllKeys()
        {
            var selectSQL = string.Format("Select [Key],Value from {0}", this._collection);
            var result = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);

            var value = string.Empty;
            foreach (var item in result)
                value = item.Value;

            return JsonConvert.DeserializeObject<dynamic>(value);

        }

        public bool DeleteAll()
        {
            var deleteSQL = string.Format("Delete from {0}", this._collection);
            AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);
            return true;
        }

        public bool Exists(string key)
        {
            var selectSQL = string.Format("Select [Key],Value from {0} where [Key]='{1}'", this._collection, key);
            var result = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);
            return result.IsAny();
        }
    }
}
