using DataMart.Models;
using DataMart.Utils;
using INTSOF.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMart.Services
{
    public class RotationDetailService : BaseService
    {
        private readonly IMongoCollection<RotationDetail> _RotationDetails;

        private String GetCollectionName(AccessIntent accessIntent)
        {
            CollectionVersionService collectionVersionService = DataMartUtils.GetCollectionVersionServiceInstance();
            CollectionVersion collectionVersion = collectionVersionService.Get(DataMartCollections.RotationDetails.GetStringValue());
            if (collectionVersion != null)
                return AccessIntent.Read == accessIntent ? collectionVersion.ReadVersion : collectionVersion.WriteVersion;
            throw new Exception("Rotation Details version not initialized.");
        }

        public RotationDetailService(AccessIntent accessIntent)
        {
            _RotationDetails = Database.GetCollection<RotationDetail>(GetCollectionName(accessIntent));
        }

        public IMongoCollection<RotationDetail> Get()
        {
            return _RotationDetails;
        }

        public IFindFluent<RotationDetail, RotationDetail> Get(List<Int32> invitationGroups, List<Int32> tenants, List<Int32> agencies)
        {
            return _RotationDetails.Find(RotationDetail => invitationGroups.Contains(RotationDetail.InvitationGroupID)
                                    && tenants.Contains(RotationDetail.TenantID) && agencies.Contains(RotationDetail.AgencyID));
        }

        public List<RotationDetail> CreateMany(List<RotationDetail> RotationDetails)
        {
            _RotationDetails.InsertMany(RotationDetails);
            return RotationDetails;
        }


        public RotationDetail Create(RotationDetail RotationDetail)
        {
            _RotationDetails.InsertOne(RotationDetail);
            return RotationDetail;
        }

        public RotationDetail Replace(RotationDetail RotationDetail)
        {
            _RotationDetails.DeleteOne(SI => RotationDetail.InvitationGroupID == SI.InvitationGroupID);
            _RotationDetails.InsertOne(RotationDetail);
            return RotationDetail;
        }

        public void Update(string id, RotationDetail RotationDetailIn)
        {
            _RotationDetails.ReplaceOne(RotationDetail => RotationDetail.ID == id, RotationDetailIn);
        }
        public void Remove(Int32 invitationGroup, Int32 tenantID)
        {
            _RotationDetails.DeleteMany(RotationDetail => RotationDetail.InvitationGroupID == invitationGroup && RotationDetail.TenantID == tenantID);
        }

        public void RemoveMany(List<Int32> invitationGroups, Int32 tenantID)
        {
            _RotationDetails.DeleteMany(RotationDetail => invitationGroups.Contains(RotationDetail.InvitationGroupID) && RotationDetail.TenantID == tenantID);
        }

        public void Remove(RotationDetail RotationDetailIn)
        {
            _RotationDetails.DeleteOne(RotationDetail => RotationDetail.ID == RotationDetailIn.ID);
        }

        public void Remove(string RotationDetailId)
        {
            _RotationDetails.DeleteOne(RotationDetail => RotationDetail.ID == RotationDetailId);
        }

        public void RemoveAll()
        {
            _RotationDetails.DeleteMany(RotationDetail => true);
        }

        public Boolean LockVersion(List<Int32> modifiedRotationDetails,DateTime syncDate, Boolean isSuccess)
        {
            CollectionVersionService collectionVersionService = DataMartUtils.GetCollectionVersionServiceInstance();
            CollectionVersion collectionVersion = collectionVersionService.Get(DataMartCollections.RotationDetails.GetStringValue());
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
                    //Database.GetCollection<RotationDetail>(readVersion).DeleteMany(x => modifiedRotationDetails.Contains(x.InvitationGroupID));
                    //Database.GetCollection<RotationDetail>(readVersion).InsertMany(Database.GetCollection<RotationDetail>(writeVersion).Find(x => modifiedRotationDetails.Contains(x.InvitationGroupID)).ToList());
                    return true;
                }
                else
                {
                    collectionVersion.WasLastSyncSuccess = false;
                    collectionVersionService.Update(collectionVersion.ID, collectionVersion);
                    //Database.GetCollection<RotationDetail>(collectionVersion.WriteVersion).DeleteMany(x => modifiedRotationDetails.Contains(x.InvitationGroupID));
                    //Database.GetCollection<RotationDetail>(collectionVersion.WriteVersion).InsertMany(Database.GetCollection<RotationDetail>(collectionVersion.ReadVersion).Find(x => modifiedRotationDetails.Contains(x.InvitationGroupID)).ToList());
                    return true;
                }
            }
            return false;
        }
    }
}