using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface ICache
    {

        bool Add(string key, object value);
        bool Add(string key, object value, bool persist);
        bool Add(string key, object value, TimeSpan expire);

        bool Update(string key, object value);
        bool Update(string key, object value, bool persist);
        bool Update(string key, object value, TimeSpan expire);

        bool Remove(string key);
        bool Remove(string key, bool persist);

        object Get(string key);
        T GetAndCast<T>(string key);
        T GetAndCast<T>(string key, bool persist);

        bool ExistsKey(string key);
        bool ExistsKey<T>(string key);
        bool ExistsKey<T>(string key, bool persist);

        bool IsUp();
        void Start();

    }
}
