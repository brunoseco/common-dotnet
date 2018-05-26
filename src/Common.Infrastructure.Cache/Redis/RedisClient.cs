using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Common.Infrastructure.Cache
{
    public class RedisClient
    {
        protected ConnectionMultiplexer _connection;
        protected IDatabase _cache;
        protected string _cacheGroup;

        protected static string _server;
        protected static string _port;
        protected static string _password;
        protected static string _conf;
        public RedisClient()
        {
            this._connection = Connection;
            this._cache = Connection.GetDatabase();
            this._cacheGroup = "CacheKeyValue";
        }
        static RedisClient()
        {
            _server = ConfigurationManager.AppSettings["ServerRedis"];
            _port = ConfigurationManager.AppSettings["PortRedis"];
            _password = ConfigurationManager.AppSettings["PasswordRedis"];
            _conf = ConfigurationManager.AppSettings["ConfRedis"];
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(string.Format("{0}:{1},password={2},{3}", _server, _port, _password, _conf));

        });



        protected static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public object Get(string key)
        {
            try
            {
                var value = this._cache.StringGet(this.makeKey(key));
                if (value.HasValue)
                    return JsonConvert.DeserializeObject(value);

                return null;
            }
            catch
            {
                return null;
            }
        }

        public T GetAndCast<T>(string key)
        {
            try
            {
                var value = this._cache.StringGet(this.makeKey(key));
                if (value.HasValue)
                    return JsonConvert.DeserializeObject<T>(value);

                return default(T);

            }
            catch
            {
                return default(T);
            }
        }


        public bool Add(string key, object value)
        {

            return this._cache.StringSet(this.makeKey(key), JsonConvert.SerializeObject(value));
        }

        public bool Add(string key, object value, TimeSpan expire)
        {

            return this._cache.StringSet(this.makeKey(key), JsonConvert.SerializeObject(value), expire);
        }


        public void Update(string key, object value)
        {
            this._cache.StringSet(this.makeKey(key), JsonConvert.SerializeObject(value));
        }

        public bool Remove(string key)
        {
            try
            {
                return this._cache.KeyDelete(this.makeKey(key));
            }
            catch
            {
                return false;
            }

        }

        protected string makeKey(string key)
        {
            return string.Format("{0}_{1}", _cacheGroup, key);
        }

        protected IEnumerable<string> GetAllKeys()
        {
            try
            {
                var keys = new List<string>();
                var server = this.GetServer();
                foreach (var key in server.Keys())
                    LimitKeysByCacheGroup(keys, key);

                return keys;
            }
            catch
            {
                return null;
            }

        }

        protected IServer GetServer()
        {
            var endpoints = Connection.GetEndPoints();
            var server = Connection.GetServer(endpoints[0]);
            return server;
        }

        protected virtual void LimitKeysByCacheGroup(List<string> keys, RedisKey key)
        {
            if (key.ToString().Contains(this._cacheGroup))
                keys.Add(key);
        }
    }
}
