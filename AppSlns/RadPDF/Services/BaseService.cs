using INTSOF.Utils;
using MongoDB.Driver;
using System.Configuration;

namespace RadPdfStore.Services
{
    public class BaseService
    {
        MongoClient _client;
        IMongoDatabase _database;

        public BaseService()
        {
            string connectionString = ConfigurationManager.AppSettings["RadPdfStoreConnectionString"].IsNotNull() ?
                                                            ConfigurationManager.AppSettings["RadPdfStoreConnectionString"].ToString() : "mongodb://127.0.0.1:27017";
            _client = new MongoClient(connectionString);
            

            string databaseName = ConfigurationManager.AppSettings["RadPdfStoreDatabaseName"].IsNotNull() ?
                                                            ConfigurationManager.AppSettings["RadPdfStoreDatabaseName"].ToString() : "RadPdf";
            _database = _client.GetDatabase(databaseName);
        }
        protected MongoClient Client
        {
            get
            {
                return _client;
            }
        }

        protected IMongoDatabase Database
        {
            get
            {
                return _database;
            }
        }
    }
}