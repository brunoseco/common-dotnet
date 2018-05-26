using Common.NoSql.Models;
using Common.NoSql.Repository;
using Common.NoSql.Repository.Mongo;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.NoSql.Mongo
{
    class QueueProcess : NoSqlBaseMongo
    {
        private MongoRepository<PersistQueue> rep;
        private MongoRepository<PersistQueueError> repError;
        public int PageSize { get; set; }
        public QueueProcess() : base(EMongoServerType.Queue)
        {
            this.rep = new MongoRepository<PersistQueue>(EMongoServerType.Queue);
            this.repError = new MongoRepository<PersistQueueError>(EMongoServerType.Queue);
            this.RegisterClassMap<PersistQueue>();
            this._isLive = base.IsUp();
            this.PageSize = 100;
        }

        public void EnQueue(int queueTypeId, object values, string identifyId = null, bool peeked = false, bool proccessed = false)
        {
            if (this._isLive)
            {
                var guid = identifyId ?? Guid.NewGuid().ToString();
                this.rep.Add(new PersistQueue
                {
                    IdentifyId = guid,
                    QueueTypeId = queueTypeId,
                    Proccessed = proccessed,
                    Peeked = peeked,
                    Value = values.ToDictionary()
                });
            }
        }
        public void EnQueueError(int queueTypeId, string errorMessage, object values, string identifyId = null, bool peeked = false, bool proccessed = false)
        {
            if (this._isLive)
            {
                var guid = identifyId ?? Guid.NewGuid().ToString();
                this.repError.Add(new PersistQueueError
                {
                    IdentifyId = guid,
                    QueueTypeId = queueTypeId,
                    Proccessed = proccessed,
                    Peeked = peeked,
                    ErrorMessage = errorMessage,
                    Value = values.ToDictionary()
                });
            }
        }

        public IEnumerable<dynamic> Peek(int queueTypeId)
        {
            if (this._isLive)
            {
                var alvos = this.rep
                    .GetByClauses(_ => _.QueueTypeId == queueTypeId
                                       && _.Proccessed == false
                                       && _.Peeked == false);

                foreach (var item in alvos)
                {
                    this.rep.Delete(_ => _.Id == item.Id);

                    var a = (int)item.QueueTypeId;
                    var b = (object)item.Value;
                    var c = (string)item.IdentifyId;

                    this.EnQueue(a, b, c, true, false);
                }

                return alvos.Select(_ => _.Value.DictionaryToObject());
            }

            return null;
        }

        public void DeQueue(int queueTypeId)
        {
            if (this._isLive)
            {
                this.rep.Delete(_ => _.QueueTypeId == queueTypeId
                                       && _.Proccessed == false
                                       && _.Peeked == true);
            }
        }

    }
}
