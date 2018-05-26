using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface ICacheProfileToolUser
    {
        void Add(string userId, int externalId, int userGroupId, object value);
        void RemoveFromUserId(string userId);
        void RemoveFromUserGroupId(int userGroupId);
        void RemoveAll();
        IEnumerable<T> GetAndCast<T>(int externalId, int userGroupId);
        IEnumerable<T> GetAndCast<T>(string userId);
        void RegisterClassMap<T>();
    }
}
