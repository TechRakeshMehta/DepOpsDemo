using DataMart.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMart.Services
{
    public class CollectionVersionService : BaseService
    {
        private readonly IMongoCollection<CollectionVersion> _CollectionVersions;

        public CollectionVersionService()
        {
            _CollectionVersions = Database.GetCollection<CollectionVersion>("CollectionVersions");
        }

        public List<CollectionVersion> Get()
        {
            return _CollectionVersions.Find(CollectionVersion => true).ToList();
        }

        public CollectionVersion Get(String collectionName)
        {
            return _CollectionVersions.Find(CollectionVersion => CollectionVersion.CollectionName == collectionName).FirstOrDefault();
        }

        public List<CollectionVersion> CreateMany(List<CollectionVersion> CollectionVersions)
        {
            _CollectionVersions.InsertMany(CollectionVersions);
            return CollectionVersions;
        }


        public CollectionVersion Create(CollectionVersion CollectionVersion)
        {
            _CollectionVersions.InsertOne(CollectionVersion);
            return CollectionVersion;
        }


        public void Update(string id, CollectionVersion CollectionVersionIn)
        {
            _CollectionVersions.ReplaceOne(CollectionVersion => CollectionVersion.ID == id, CollectionVersionIn);
        }

        public void Remove(CollectionVersion CollectionVersionIn)
        {
            _CollectionVersions.DeleteOne(CollectionVersion => CollectionVersion.ID == CollectionVersionIn.ID);
        }

        public void Remove(string CollectionVersionId)
        {
            _CollectionVersions.DeleteOne(CollectionVersion => CollectionVersion.ID == CollectionVersionId);
        }

        public void RemoveAll()
        {
            _CollectionVersions.DeleteMany(CollectionVersion => true);
        }
    }
}