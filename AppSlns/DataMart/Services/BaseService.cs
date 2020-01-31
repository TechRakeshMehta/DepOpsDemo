using INTSOF.Utils;
using MongoDB.Driver;
using System.Configuration;

namespace DataMart.Services
{
    public class BaseService
    {
        MongoClient _client;
        IMongoDatabase _database;

        public BaseService()
        {
            string connectionString = ConfigurationManager.AppSettings["DataMartConnectionString"].IsNotNull() ?
                                                            ConfigurationManager.AppSettings["DataMartConnectionString"].ToString() : "mongodb://ADB:TacyG1D5@localhost:27017/ADBDataMart";
            _client = new MongoClient(connectionString);
            

            string databaseName = ConfigurationManager.AppSettings["DataMartDatabaseName"].IsNotNull() ?
                                                            ConfigurationManager.AppSettings["DataMartDatabaseName"].ToString() : "ADBDataMart";
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