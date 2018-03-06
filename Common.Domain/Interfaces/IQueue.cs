
using System.Collections.Generic;

namespace Common.Domain.Interfaces
{
    public interface IQueue
    {
        void RegisterClassMap<T>();
        void EnQueue(int queueTypeId, object values);
        void EnQueueError(int queueTypeId, string errorMessage, object values);
        void DeQueue(int queueTypeId);
        IEnumerable<dynamic> Peek(int queueTypeId);
    }
}