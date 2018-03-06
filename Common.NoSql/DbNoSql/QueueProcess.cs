using Common.Domain.Extensions;
using Common.Infrastructure.ORM.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Common.NoSql.DbNoSql
{
    class QueueProcess
    {

        private string _connectionString;
        private string _collection;
        private string _collectionError;

        public QueueProcess()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["DbCache"].ConnectionString;
            this._collection = "PersistQueue";
            this._collectionError = "PersistQueueError";
        }

        public void EnQueue(int queueTypeId, object value, string identifyId = null, bool peeked = false, bool proccessed = false)
        {

            var guid = identifyId ?? Guid.NewGuid().ToString();
            var valueSerializer = value.SerializeObjectWithIgnore();
            var insertSQL = string.Format("Insert into {0} Values(@IdentifyId, @QueueTypeId, @Proccessed, @Peeked, @Value)", this._collection);

            AdoNetHelper.ExecuteNonQuery(insertSQL, this._connectionString, new
            {
                IdentifyId = guid,
                QueueTypeId = queueTypeId,
                Proccessed = proccessed,
                Peeked = peeked,
                Value = valueSerializer
            }, commandType: System.Data.CommandType.Text);


        }
        public void EnQueueError(int queueTypeId, string errorMessage, object values, string identifyId = null, bool peeked = false, bool proccessed = false)
        {
            var guid = identifyId ?? Guid.NewGuid().ToString();
            var valueSerializer = values.SerializeObjectWithIgnore();
            var insertSQL = string.Format("Insert into {0} Values(@IdentifyId,@QueueTypeId,@Proccessed,@Peeked,@Value)", this._collectionError);

            AdoNetHelper.ExecuteNonQuery(insertSQL, this._connectionString, new
            {
                IdentifyId = guid,
                QueueTypeId = queueTypeId,
                Proccessed = proccessed,
                Peeked = peeked,
                ErrorMessage = errorMessage,
                Value = valueSerializer
            }, commandType: System.Data.CommandType.Text);
        }

        public IEnumerable<dynamic> Peek(int queueTypeId)
        {
            var selectSQL = string.Format("Select QueueTypeId,Value,IdentifyId from {0} where QueueTypeId={1} and Proccessed=0 and Peeked=0", this._collection, queueTypeId);
            var alvos = AdoNetHelper.ExecuteReader(selectSQL, this._connectionString, commandType: System.Data.CommandType.Text);

            foreach (var item in alvos)
            {

                var deleteSQL = string.Format("Delete from {0} where IdentifyId='{1}'", this._collection, item.IdentifyId);
                AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);

                var a = item.QueueTypeId;
                var b = JsonConvert.DeserializeObject<dynamic>(item.Value);
                var c = item.IdentifyId;

                this.EnQueue(a, b, c, true, false);
            }

            return alvos.Select(_ => JsonConvert.DeserializeObject<dynamic>(_.Value));
        }

        public void DeQueue(int queueTypeId)
        {
            var deleteSQL = string.Format("Delete from {0} where QueueTypeId={1} and Proccessed=0 and Peeked=1", this._collection, queueTypeId);
            AdoNetHelper.ExecuteNonQuery(deleteSQL, this._connectionString, commandType: System.Data.CommandType.Text);
        }

    }
}
