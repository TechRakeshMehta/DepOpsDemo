using DataMart.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMart.Services
{
    public class SavedSearchService : BaseService
    {
        private readonly IMongoCollection<SavedSearch> _SavedSearchs;

        public SavedSearchService()
        {
            _SavedSearchs = Database.GetCollection<SavedSearch>("SavedSearchs");
        }

        public List<SavedSearch> Get()
        {
            return _SavedSearchs.Find(SavedSearch => true).ToList();
        }

        public List<SavedSearch> Get(String userID, String searchType)
        {
            return _SavedSearchs.Find(SavedSearch => SavedSearch.UserID == userID && SavedSearch.SearchType == searchType).ToList();
        }

        public SavedSearch GetSearchDetails(String searchID)
        {
            return _SavedSearchs.Find(SavedSearch => SavedSearch.ID == searchID).FirstOrDefault();
        }

        public List<SavedSearch> CreateMany(List<SavedSearch> SavedSearchs)
        {
            _SavedSearchs.InsertMany(SavedSearchs);
            return SavedSearchs;
        }


        public SavedSearch Create(SavedSearch SavedSearch)
        {
            _SavedSearchs.InsertOne(SavedSearch);
            return SavedSearch;
        }


        public void Update(string id, SavedSearch SavedSearchIn)
        {
            _SavedSearchs.ReplaceOne(SavedSearch => SavedSearch.ID == id, SavedSearchIn);
        }

        public void Remove(SavedSearch SavedSearchIn)
        {
            _SavedSearchs.DeleteOne(SavedSearch => SavedSearch.ID == SavedSearchIn.ID);
        }

        public void Remove(string SavedSearchId)
        {
            _SavedSearchs.DeleteOne(SavedSearch => SavedSearch.ID == SavedSearchId);
        }

        public void RemoveAll()
        {
            _SavedSearchs.DeleteMany(SavedSearch => true);
        }
    }
}