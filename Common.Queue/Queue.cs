using Common.Domain.Interfaces;
using Common.NoSql;
using Common.NoSql.Mongo;
using System.Collections.Generic;

namespace Common.Queue
{
    public class Queue : IQueue
    {
        private readonly QueueProcessFactory queueProcess;
        public Queue()
        {
            this.queueProcess = new QueueProcessFactory();
        }
        public void RegisterClassMap<T>()
        {
            this.queueProcess.RegisterClassMap<T>();
        }
        public void EnQueue(int queueTypeId, object values)
        {
            this.queueProcess.EnQueue(queueTypeId, values);
        }
        public void EnQueueError(int queueTypeId, string errorMessage, object values)
        {
            this.queueProcess.EnQueueError(queueTypeId, errorMessage, values);
        }
        public void DeQueue(int queueTypeId)
        {
            this.queueProcess.DeQueue(queueTypeId);
        }
        public IEnumerable<dynamic> Peek(int queueTypeId)
        {
            return this.queueProcess.Peek(queueTypeId);
        }

    }
}
