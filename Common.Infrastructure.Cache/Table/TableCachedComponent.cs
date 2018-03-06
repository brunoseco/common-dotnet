using Common.Domain.Extensions;
using Common.Domain.Interfaces;
using Common.NoSql;
using Common.NoSql.Mongo;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;

namespace Common.Infrastructure.Cache
{
    class KeyValueCache : TableEntity
    {
        public KeyValueCache(string key)
        {
            this.PartitionKey = key;
            this.RowKey = key;
        }

        public KeyValueCache()
        {

        }

        public string key { get; set; }
        public string value { get; set; }
    }
    public class TableCachedComponent : ICache
    {
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _tableClient;
        private CloudTable _table;
        public TableCachedComponent()
        {
            this._storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            this._tableClient = _storageAccount.CreateCloudTableClient();
            this._table = this._tableClient.GetTableReference("KeyValueCache");
            this._table.CreateIfNotExists();

        }

        public bool Add(string key, object value)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();

            var keyvaluePair = new KeyValueCache(key)
            {
                key = key,
                value = valueSerializer
            };
            var insertOperation = TableOperation.Insert(keyvaluePair);
            this._table.Execute(insertOperation);

            return true;
        }

        public bool Add(string key, object value, TimeSpan expire)
        {
            return this.Add(key, value);
        }

        public bool Add(string key, object value, bool persist)
        {
            return this.Add(key, value);
        }

        public bool ExistsKey(string key)
        {
            return this.ExistsKey<object>(key);
        }
        public bool ExistsKey<T>(string key)
        {
            return this.GetAndCast<T>(key).IsNotNull();
        }

        public bool ExistsKey<T>(string key, bool persist)
        {
            return this.ExistsKey<T>(key);
        }

        public T GetAndCast<T>(string key)
        {
            var retrieveOperation = TableOperation.Retrieve<KeyValueCache>(key, key);
            var retrievedResult = this._table.Execute(retrieveOperation);
            if (retrievedResult.Result.IsNull())
                return default(T);

            var value = ((KeyValueCache)retrievedResult.Result).value;
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }

        public object Get(string key)
        {
            return this.GetAndCast<object>(key);
        }

        public T GetAndCast<T>(string key, bool persist)
        {
            return this.GetAndCast<T>(key);
        }

           
        public bool Remove(string key)
        {
            var retrieveOperation = TableOperation.Retrieve<KeyValueCache>(key, key);
            var retrievedResult = this._table.Execute(retrieveOperation);
            if (retrievedResult.Result.IsNotNull())
            {
                var deleteEntity = ((KeyValueCache)retrievedResult.Result);
                var deleteOperation = TableOperation.Delete(deleteEntity);
                this._table.Execute(deleteOperation);
                return true;
            }
            return false;
        }

        public bool Remove(string key, bool persist)
        {
            return this.Remove(key);
        }


        public bool Update(string key, object value)
        {
            var retrieveOperation = TableOperation.Retrieve<KeyValueCache>(key, key);
            var retrievedResult = this._table.Execute(retrieveOperation);
            if (retrievedResult.Result.IsNotNull())
            {
                var valueSerializer = value.SerializeObjectWithIgnore();
                var updateEntity = ((KeyValueCache)retrievedResult.Result);
                updateEntity.value = valueSerializer;

                var updateOperation = TableOperation.Replace(updateEntity);
                this._table.Execute(updateOperation);

                return true;
            }
            return false;
        }

        public bool Update(string key, object value, TimeSpan expire)
        {
            return this.Update(key, value);
        }

        public bool Update(string key, object value, bool persist)
        {
            return this.Update(key, value);
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
    }
}
