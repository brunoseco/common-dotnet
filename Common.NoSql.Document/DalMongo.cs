using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Authentication;

namespace Common.NoSql.Document
{
    public class DalMongo : IDisposable
    {
        //private MongoServer mongoServer = null;
        private bool disposed = false;

        // To do: update the connection string with the DNS name
        // or IP address of your server. 
        //For example, "mongodb://testlinux.cloudapp.net
        private string connectionString = "mongodb://cnabox:Pt4liwLY4M6VYJSjFwYuXi5Scsh9wOSAxjxWtm3wco3ljY9jiuSdLxBwsZxpIADRFd1NLXR4OAiR9kun8v4ukA==@cnabox.documents.azure.com:10250/?ssl=true";
        private string userName = "cnabox";
        private string host = "cnabox.documents.azure.com";
        private string password = "Pt4liwLY4M6VYJSjFwYuXi5Scsh9wOSAxjxWtm3wco3ljY9jiuSdLxBwsZxpIADRFd1NLXR4OAiR9kun8v4ukA==";

        // This sample uses a database named "Tasks" and a 
        //collection named "TasksList".  The database and collection 
        //will be automatically created if they don't already exist.
        private string dbName = "Tasks";
        private string collectionName = "TasksList";

        // Default constructor.        
        public DalMongo()
        {
        }

        // Gets all Task items from the MongoDB server.        
        public List<MyTask> GetAllTasks()
        {
            try
            {
                var collection = GetTasksCollection();
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<MyTask>();
            }
        }

        // Creates a Task and inserts it into the collection in MongoDB.
        public void CreateTask(MyTask task)
        {
            var collection = GetTasksCollectionForEdit();
            try
            {
                collection.InsertOne(task);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        private IMongoCollection<MyTask> GetTasksCollection()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10250);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credentials = new List<MongoCredential>()
            {
                new MongoCredential("SCRAM-SHA-1", identity, evidence)
            };

            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var todoTaskCollection = database.GetCollection<MyTask>(collectionName);
            return todoTaskCollection;
        }

        private IMongoCollection<MyTask> GetTasksCollectionForEdit()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10250);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credentials = new List<MongoCredential>()
            {
                new MongoCredential("SCRAM-SHA-1", identity, evidence)
            };
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var todoTaskCollection = database.GetCollection<MyTask>(collectionName);
            return todoTaskCollection;
        }

        # region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }

            this.disposed = true;
        }

        # endregion
    }
}