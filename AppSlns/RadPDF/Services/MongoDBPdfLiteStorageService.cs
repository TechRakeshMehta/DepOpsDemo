using RadPdfStore.Models;
using RadPdfStore.Utils;
using INTSOF.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RadPdfStore.Services
{
    public class MongoDBPdfLiteStorageService:BaseService
    {
        private readonly IMongoCollection<MongoDBPdfLiteStorage> _MongoDBPdfLiteStorages;


        public MongoDBPdfLiteStorageService()
        {
            _MongoDBPdfLiteStorages = Database.GetCollection<MongoDBPdfLiteStorage>("MongoDBPdfLiteStorage");
        }

        public List<MongoDBPdfLiteStorage> Get()
        {
            return _MongoDBPdfLiteStorages.Find(MongoDBPdfLiteStorage => true).ToList();
        }

        public MongoDBPdfLiteStorage Get(String key)
        {
            return _MongoDBPdfLiteStorages.Find(MongoDBPdfLiteStorage => MongoDBPdfLiteStorage.Key.ToLower() == key.ToLower()).FirstOrDefault();
        }

        public MongoDBPdfLiteStorage Get(String key, Int32 subType)
        {
            return _MongoDBPdfLiteStorages.Find(MongoDBPdfLiteStorage => MongoDBPdfLiteStorage.Key.ToLower() == key.ToLower() && MongoDBPdfLiteStorage.SubType == subType).FirstOrDefault();
        }

        public List<MongoDBPdfLiteStorage> CreateMany(List<MongoDBPdfLiteStorage> MongoDBPdfLiteStorages)
        {
            _MongoDBPdfLiteStorages.InsertMany(MongoDBPdfLiteStorages);
            return MongoDBPdfLiteStorages;
        }


        public MongoDBPdfLiteStorage Create(MongoDBPdfLiteStorage MongoDBPdfLiteStorage)
        {
            _MongoDBPdfLiteStorages.InsertOne(MongoDBPdfLiteStorage);
            return MongoDBPdfLiteStorage;
        }

        public MongoDBPdfLiteStorage Replace(MongoDBPdfLiteStorage MongoDBPdfLiteStorage)
        {
            _MongoDBPdfLiteStorages.DeleteOne(AU => MongoDBPdfLiteStorage.Key == AU.Key);
            _MongoDBPdfLiteStorages.InsertOne(MongoDBPdfLiteStorage);
            return MongoDBPdfLiteStorage;
        }


        public void Update(string id, MongoDBPdfLiteStorage MongoDBPdfLiteStorageIn)
        {
            _MongoDBPdfLiteStorages.ReplaceOne(MongoDBPdfLiteStorage => MongoDBPdfLiteStorage.ID == id, MongoDBPdfLiteStorageIn);
        }

        public void Remove(MongoDBPdfLiteStorage MongoDBPdfLiteStorageIn)
        {
            _MongoDBPdfLiteStorages.DeleteOne(MongoDBPdfLiteStorage => MongoDBPdfLiteStorage.ID == MongoDBPdfLiteStorageIn.ID);
        }

        public void Remove(string key)
        {
            _MongoDBPdfLiteStorages.DeleteOne(MongoDBPdfLiteStorage => MongoDBPdfLiteStorage.Key == key);
        }

        public void RemoveAll()
        {
            _MongoDBPdfLiteStorages.DeleteMany(MongoDBPdfLiteStorage => true);
        }

     }
}