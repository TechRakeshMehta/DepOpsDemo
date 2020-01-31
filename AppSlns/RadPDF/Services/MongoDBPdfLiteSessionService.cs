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
    public class MongoDBPdfLiteSessionService:BaseService
    {
        private readonly IMongoCollection<MongoDBPdfLiteSession> _MongoDBPdfLiteSessions;


        public MongoDBPdfLiteSessionService()
        {
            _MongoDBPdfLiteSessions = Database.GetCollection<MongoDBPdfLiteSession>("MongoDBPdfLiteSession");
        }

        public List<MongoDBPdfLiteSession> Get()
        {
            return _MongoDBPdfLiteSessions.Find(MongoDBPdfLiteSession => true).ToList();
        }

        public MongoDBPdfLiteSession Get(String key)
        {
            return _MongoDBPdfLiteSessions.Find(MongoDBPdfLiteSession => MongoDBPdfLiteSession.Key.ToLower() == key.ToLower()).FirstOrDefault();
        }

        public List<MongoDBPdfLiteSession> CreateMany(List<MongoDBPdfLiteSession> MongoDBPdfLiteSessions)
        {
            _MongoDBPdfLiteSessions.InsertMany(MongoDBPdfLiteSessions);
            return MongoDBPdfLiteSessions;
        }


        public MongoDBPdfLiteSession Create(MongoDBPdfLiteSession MongoDBPdfLiteSession)
        {
            _MongoDBPdfLiteSessions.InsertOne(MongoDBPdfLiteSession);
            return MongoDBPdfLiteSession;
        }

        public MongoDBPdfLiteSession Replace(MongoDBPdfLiteSession MongoDBPdfLiteSession)
        {
            _MongoDBPdfLiteSessions.DeleteOne(AU => MongoDBPdfLiteSession.Key == AU.Key);
            _MongoDBPdfLiteSessions.InsertOne(MongoDBPdfLiteSession);
            return MongoDBPdfLiteSession;
        }


        public void Update(string id, MongoDBPdfLiteSession MongoDBPdfLiteSessionIn)
        {
            _MongoDBPdfLiteSessions.ReplaceOne(MongoDBPdfLiteSession => MongoDBPdfLiteSession.ID == id, MongoDBPdfLiteSessionIn);
        }

        public void Remove(MongoDBPdfLiteSession MongoDBPdfLiteSessionIn)
        {
            _MongoDBPdfLiteSessions.DeleteOne(MongoDBPdfLiteSession => MongoDBPdfLiteSession.ID == MongoDBPdfLiteSessionIn.ID);
        }

        public void Remove(string key)
        {
            _MongoDBPdfLiteSessions.DeleteOne(MongoDBPdfLiteSession => MongoDBPdfLiteSession.Key == key);
        }

        public void RemoveAll()
        {
            _MongoDBPdfLiteSessions.DeleteMany(MongoDBPdfLiteSession => true);
        }

     }
}