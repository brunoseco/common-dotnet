using Common.Domain;
using Common.Domain.Extensions;
using Common.Domain.Interfaces;
using Common.NoSql;
using Common.NoSql.Mongo;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;

namespace Common.Infrastructure.Cache
{
    public class MemCachedComponentSplit : ICache
    {
        private readonly MemcachedClient cache;
        private readonly int _limit;

        public MemCachedComponentSplit()
        {
            this._limit = 1000000;
            if (ConfigurationManager.AppSettings["limitCaractersMemcached"] != null)
                this._limit = Convert.ToInt32(ConfigurationManager.AppSettings["limitCaractersMemcached"]);

            this.cache = new MemcachedClient();
            this.EnableLogs();
            this.Start();

        }

        public void RegisterClassMap<T>()
        {

        }
        private void EnableLogs()
        {
            var log = ConfigurationManager.AppSettings["logs"].ToUpper();
            if (log == "MEMCACHED")
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("log-memcached-{0}.log", DateTime.Now.ToString("dd-MM-yyyy")));
                LogManager.AssignFactory(new Log4NetFactory());
                LogManager.AssignFactory(new DiagnosticsLogFactory(path));
            }
        }

        public bool Add(string key, object value)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();

            if (split(key, valueSerializer, StoreMode.Set))
                return true;

            return cache.Store(StoreMode.Set, key, valueSerializer);
        }



        public bool Add(string key, object value, bool persists)
        {
            return this.Add(key, value);
        }
        public bool Add(string key, object value, TimeSpan expire)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();

            if (split(key, valueSerializer, StoreMode.Set, expire))
                return true;

            return cache.Store(StoreMode.Set, key, valueSerializer, expire);
        }

        public bool Update(string key, object value)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();

            if (split(key, valueSerializer, StoreMode.Replace))
                return true;

            return cache.Store(StoreMode.Replace, key, value);
        }
        public bool Update(string key, object value, bool persists)
        {
            return this.Update(key, value);
        }
        public bool Update(string key, object value, TimeSpan expire)
        {
            var valueSerializer = value.SerializeObjectWithIgnore();

            if (split(key, valueSerializer, StoreMode.Replace, expire))
                return true;

            return cache.Store(StoreMode.Replace, key, valueSerializer, expire);
        }

        public bool Remove(string key)
        {
            return this.Remove(key, false);
        }
        public bool Remove(string key, bool persists)
        {
            var qtd = this.cache.Get(defineKeySplit(key));
            if (qtd.IsNotNull())
            {
                for (int i = 0; i < Convert.ToInt32(qtd); i++)
                {
                    this.cache.Remove(defineKeySplitItem(key, i));
                }
                this.Remove(defineKeySplit(key));
                return true;
            }

            return this.cache.Remove(key);

        }

        public void FlushAll()
        {
            this.cache.FlushAll();
        }

        public T GetAndCast<T>(string key)
        {
            var result = string.Empty;
            var qtd = cache.Get(defineKeySplit(key));
            if (qtd.IsNotNull())
            {
                for (int i = 0; i < Convert.ToInt32(qtd); i++)
                {
                    result += cache.Get(defineKeySplitItem(key, i));
                }
            }
            else
                result = cache.Get(key) as string;


            if (result.IsNull())
                return default(T);

            return JsonConvert.DeserializeObject<T>(result);
        }
        public object Get(string key)
        {
            return this.GetAndCast<object>(key);
        }

        public T GetAndCast<T>(string key, bool persist)
        {
            var result = this.GetAndCast<T>(key);
            return (T)result;
        }

        public bool ExistsKey(string key)
        {
            return this.ExistsKey<object>(key);
        }

        public bool ExistsKey<T>(string key)
        {
            return this.ExistsKey<T>(key, false);
        }
        public bool ExistsKey<T>(string key, bool persists)
        {
            var found = false;
            var result = this.cache.Get(key) as string;
            found = result.IsNotNullOrEmpty();
            if (!found)
            {
                result = this.cache.Get(defineKeySplit(key)) as string;
                found = result.IsNotNullOrEmpty();
            }
            return found;
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

        private bool split(string key, string valueSerializer, StoreMode storeMode, TimeSpan? expire = null)
        {
            var splited = false;
           
            var length = valueSerializer.Length;

            if (length > this._limit)
            {
                var pieces = Math.Ceiling(length / (decimal)this._limit);
                var keyGroup = defineKeySplit(key);
                cache.Store(storeMode, keyGroup, pieces.ToString());
                for (int i = 0; i < pieces; i++)
                {
                    var startIndex = i * this._limit;
                    var delta = (length - startIndex);
                    var newlimit = delta < this._limit ? delta : this._limit;
                    var textPiece = valueSerializer.Substring(startIndex, newlimit);
                    if (expire.IsNotNull())
                        cache.Store(storeMode, defineKeySplitItem(key, i), textPiece, expire.Value);
                    else
                        cache.Store(storeMode, defineKeySplitItem(key, i), textPiece);
                }
                splited = true;
            }
            return splited;
        }

        private string defineKeySplitItem(string key, int i)
        {
            return string.Format("{0}-{1}", key, i);
        }

        private string defineKeySplit(string key)
        {
            return string.Format("{0}-SL", key);
        }
    }
}
