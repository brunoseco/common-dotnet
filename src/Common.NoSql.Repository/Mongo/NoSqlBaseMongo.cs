using Common.NoSql.Mongo;
using Common.NoSql.Repository.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;

namespace Common.NoSql.Repository.Mongo
{
    public class NoSqlBaseMongo
    {
        protected bool _isLive;
        protected Object _thisLock;
        protected string _connectionString;

        protected MongoClient _client;
        protected IMongoDatabase _database;

        public NoSqlBaseMongo(EMongoServerType mongoServerType)
        {
            this._thisLock = new Object();
            this._connectionString = ConfigConnectionString(mongoServerType);
            var settings = Connect();
            this._client = new MongoClient(settings);
            this._database = this._client.GetDatabase(this.Database());
        }

        private string ConfigConnectionString(EMongoServerType mongoServerType)
        {
            if (mongoServerType == EMongoServerType.Queue)
                return string.Format("MongoServerSettings{0}", mongoServerType.ToString());

            if (mongoServerType == EMongoServerType.Testing)
                return string.Format("MongoServerSettings{0}", mongoServerType.ToString());

            return string.Format("MongoServerSettings");
        }

        private MongoClientSettings Connect()
        {
            if (ConnectionString().Length == 4)
                return ConnectionByConnectionString();

            return ConnectByCredencials();
        }

        private MongoClientSettings ConnectionByConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionString].ToString();
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
            return settings;
        }

        private MongoClientSettings ConnectByCredencials()
        {
            var parts = ConnectionString();

            var host = parts[2];
            var port = parts[3];
            var userName = parts[4];
            var password = parts[5];

            var settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, Convert.ToInt32(port));
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            var identity = new MongoInternalIdentity(this.Database(), userName);
            var evidence = new PasswordEvidence(password);

            settings.Credentials = new List<MongoCredential>()
            {
                new MongoCredential("SCRAM-SHA-1", identity, evidence)
            };
            return settings;
        }

        private string[] ConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[_connectionString].ToString();
            return connectionString.Split('/');
        }

        private string Database()
        {
            var parts = ConnectionString().LastOrDefault();
            return parts.Replace(",ssl=true", "").Replace(",ssl=fase", "");
        }
        protected bool IsUp()
        {
            try
            {
                var mongoIsUpVerify = ConfigurationManager.AppSettings["mongoIsUpVerify"] as string;
                if (mongoIsUpVerify == "PING")
                {
                    this.Start();
                    bool exists = this.ExistsPING();
                    if (exists)
                        return true;
                }
                else if (mongoIsUpVerify == "DISABLED")
                    return false;

                return InitMongo.ProcessIsUp();

            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ExistsPING()
        {
            try
            {
                var _collection = this._database.GetCollection<IsUpKeyValue>("IsUp");
                var exists = _collection.Find<IsUpKeyValue>(_ => _.key == "PING").Count() > 0;
                return exists;
            }
            catch
            {
                return false;
            }
        }

        private void Start()
        {
            try
            {
                if (!this.ExistsPING())
                {
                    var _collection = this._database.GetCollection<IsUpKeyValue>("IsUp");
                    _collection.InsertOne(new IsUpKeyValue { key = "PING", Value = "POING" });
                }
            }
            catch {
            }
        }

        public void RegisterClassMap<T>()
        {
            try
            {
                lock (_thisLock)
                {
                    var anyMap = BsonClassMap.GetRegisteredClassMaps().Where(_ => _.ClassType == typeof(T)).Any();
                    if (!anyMap)
                        BsonClassMap.RegisterClassMap<T>();
                }
            }
            catch { }
        }


    }
}
