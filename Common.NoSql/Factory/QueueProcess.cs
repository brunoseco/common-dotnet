using Common.NoSql.Models;
using Common.NoSql.Repository;
using Common.NoSql.Repository.Mongo;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Common.NoSql
{
    public class QueueProcessFactory 
    {

        private Mongo.QueueProcess _queueProcess;
        public QueueProcessFactory() 
        {
            this._queueProcess = new Mongo.QueueProcess();
        }

        public void EnQueue(int queueTypeId, object values, string identifyId = null, bool peeked = false, bool proccessed = false)
        {
            this._queueProcess.EnQueue(queueTypeId, values, identifyId, peeked, proccessed);
        }
        public void EnQueueError(int queueTypeId, string errorMessage, object values, string identifyId = null, bool peeked = false, bool proccessed = false)
        {
            this._queueProcess.EnQueueError(queueTypeId, errorMessage, values, identifyId , peeked, proccessed);
        }

        public IEnumerable<dynamic> Peek(int queueTypeId)
        {
            return this._queueProcess.Peek(queueTypeId);
        }

        public void DeQueue(int queueTypeId)
        {
            this._queueProcess.DeQueue(queueTypeId);
        }

        public void RegisterClassMap<T>()
        {
            
        }


    }
}
