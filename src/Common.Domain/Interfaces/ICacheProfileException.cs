using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface ICacheProfileException
    {

        void Add(string roleId, int externalId, int toolId, int exceptionGroupId, object value);
        void RemoveAll();
        IEnumerable<T> GetAndCast<T>(IEnumerable<int> externalsId, int exceptionGroupId);
        void RegisterClassMap<T>();

    }
}
