using DataMart.Models;
using DataMart.Utils;
using INTSOF.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMart.Services
{
    public class AgencyUserService:BaseService
    {
        private readonly IMongoCollection<AgencyUser> _AgencyUsers;

        private String GetCollectionName(AccessIntent accessIntent)
        {
            CollectionVersionService collectionVersionService = DataMartUtils.GetCollectionVersionServiceInstance();
            CollectionVersion collectionVersion = collectionVersionService.Get(DataMartCollections.AgencyUsers.GetStringValue());
            if (collectionVersion != null)
                return AccessIntent.Read == accessIntent ? collectionVersion.ReadVersion : collectionVersion.WriteVersion;
            throw new Exception("Agency User version not initialized.");
        }

        public AgencyUserService(AccessIntent accessIntent)
        {
            _AgencyUsers = Database.GetCollection<AgencyUser>(GetCollectionName(accessIntent));
        }

        public List<AgencyUser> Get()
        {
            return _AgencyUsers.Find(AgencyUser => true).ToList();
        }

        public AgencyUser Get(String userID)
        {
            return _AgencyUsers.Find(AgencyUser => AgencyUser.UserID.ToLower() == userID.ToLower()).FirstOrDefault();
        }

        public List<AgencyUser> CreateMany(List<AgencyUser> AgencyUsers)
        {
            _AgencyUsers.InsertMany(AgencyUsers);
            return AgencyUsers;
        }


        public AgencyUser Create(AgencyUser AgencyUser)
        {
            _AgencyUsers.InsertOne(AgencyUser);
            return AgencyUser;
        }

        public AgencyUser Replace(AgencyUser AgencyUser)
        {
            _AgencyUsers.DeleteOne(AU => AgencyUser.AgencyUserID == AU.AgencyUserID);
            _AgencyUsers.InsertOne(AgencyUser);
            return AgencyUser;
        }


        public void Update(string id, AgencyUser AgencyUserIn)
        {
            _AgencyUsers.ReplaceOne(AgencyUser => AgencyUser.ID == id, AgencyUserIn);
        }

        public void Remove(AgencyUser AgencyUserIn)
        {
            _AgencyUsers.DeleteOne(AgencyUser => AgencyUser.ID == AgencyUserIn.ID);
        }

        public void Remove(string AgencyUserId)
        {
            _AgencyUsers.DeleteOne(AgencyUser => AgencyUser.ID == AgencyUserId);
        }

        public void RemoveAll()
        {
            _AgencyUsers.DeleteMany(AgencyUser => true);
        }

        public Boolean LockVersion(List<Int32> modifiedAgencyUsers,DateTime syncDate, Boolean isSuccess)
        {
            CollectionVersionService collectionVersionService = DataMartUtils.GetCollectionVersionServiceInstance();
            CollectionVersion collectionVersion = collectionVersionService.Get(DataMartCollections.AgencyUsers.GetStringValue());
            if (collectionVersion != null)
            {
                if (isSuccess)
                {
                    String readVersion = collectionVersion.ReadVersion;
                    String writeVersion = collectionVersion.WriteVersion;
                    collectionVersion.ReadVersion = writeVersion;
                    collectionVersion.WriteVersion = readVersion;
                    collectionVersion.LastSyncDate = syncDate;
                    collectionVersion.WasLastSyncSuccess = true;
                    collectionVersionService.Update(collectionVersion.ID, collectionVersion);
                    //Database.GetCollection<AgencyUser>(readVersion).DeleteMany(x => modifiedAgencyUsers.Contains(x.AgencyUserID));
                    //Database.GetCollection<AgencyUser>(readVersion).InsertMany(Database.GetCollection<AgencyUser>(writeVersion).Find(x => modifiedAgencyUsers.Contains(x.AgencyUserID)).ToList());
                    return true;
                }
                else
                {
                    collectionVersion.WasLastSyncSuccess = false;
                    collectionVersionService.Update(collectionVersion.ID, collectionVersion);
                    //Database.GetCollection<AgencyUser>(collectionVersion.WriteVersion).DeleteMany(x => modifiedAgencyUsers.Contains(x.AgencyUserID));
                    //Database.GetCollection<AgencyUser>(collectionVersion.WriteVersion).InsertMany(Database.GetCollection<AgencyUser>(collectionVersion.ReadVersion)
                    //                                                                  .Find(x => modifiedAgencyUsers.Contains(x.AgencyUserID)).ToList());
                    return true;
                }
            }
            return false;
        }
    }
}