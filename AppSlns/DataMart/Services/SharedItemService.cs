using DataMart.Models;
using DataMart.Utils;
using INTSOF.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMart.Services
{
    public class SharedItemService : BaseService
    {
        private readonly IMongoCollection<SharedItem> _SharedItems;

        private String GetCollectionName(AccessIntent accessIntent)
        {
            CollectionVersionService collectionVersionService = DataMartUtils.GetCollectionVersionServiceInstance();
            CollectionVersion collectionVersion = collectionVersionService.Get(DataMartCollections.SharedItems.GetStringValue());
            if (collectionVersion != null)
                return AccessIntent.Read == accessIntent ? collectionVersion.ReadVersion : collectionVersion.WriteVersion;
            throw new Exception("Shared Item version not initialized.");
        }

        public SharedItemService(AccessIntent accessIntent)
        {
            _SharedItems = Database.GetCollection<SharedItem>(GetCollectionName(accessIntent));
        }

        public IMongoCollection<SharedItem> Get()
        {
            return _SharedItems;
        }

        public IFindFluent<SharedItem, SharedItem> Get(List<Int32> invitationGroups, List<Int32> tenants, List<Int32> agencies)
        {
            return _SharedItems.Find(SharedItem => invitationGroups.Contains(SharedItem.InvitationGroupID)
                                    && tenants.Contains(SharedItem.TenantID) && agencies.Contains(SharedItem.AgencyID));
        }
        public IFindFluent<SharedItem, SharedItem> Get(List<Int32> invitationGroups, List<Int32> tenants, List<Int32> agencies, List<String> categories)
        {
            return _SharedItems.Find(SharedItem => invitationGroups.Contains(SharedItem.InvitationGroupID)
                                    && tenants.Contains(SharedItem.TenantID) && agencies.Contains(SharedItem.AgencyID)
                                    && categories.Contains(SharedItem.CategoryID));
        }


        public List<SharedItem> GetSharedDataForNonCompliance(List<Int32> invitationGroups, List<Int32> tenants, List<Int32> agencies,
            List<String> categories, List<String> userType
            , Boolean includeUndefinedDataShares)
        {
            return _SharedItems.Find(SharedItem => invitationGroups.Contains(SharedItem.InvitationGroupID)
                                    && tenants.Contains(SharedItem.TenantID) && agencies.Contains(SharedItem.AgencyID)
                                    && categories.Contains(SharedItem.CategoryID)
                                    && userType.Contains(SharedItem.UserType)
                                    && SharedItem.RotationID != null
                                    && (includeUndefinedDataShares ||
                                    (SharedItem.RotationStartDate != null && SharedItem.RotationEndDate != null))
                                    && ((SharedItem.RotationEndDate == null || SharedItem.RotationEndDate >= DateTime.Now) && (SharedItem.RotationStartDate == null || SharedItem.RotationStartDate <= DateTime.Now)))
                                    .ToList().GroupBy(x => new { x.TenantID, x.StudentID, x.RotationID, x.CategoryID })
                                    .Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault())
                                    .ToList();
        }

        public List<SharedItem> GetSharedDataPerRotation(List<Int32> invitationGroups, List<Int32> tenants, List<Int32> agencies,
    List<String> categories, List<String> items, string complioID, string customAttribute
    , DateTime? rotationStartDate, DateTime? rotationEndDate, List<String> reviewStatus, List<String> userTypes)
        {
            return _SharedItems.Find(SharedItem => invitationGroups.Contains(SharedItem.InvitationGroupID)
                                    && tenants.Contains(SharedItem.TenantID) && agencies.Contains(SharedItem.AgencyID)
                                    && categories.Contains(SharedItem.CategoryID) && items.Contains(SharedItem.ItemID)
                                    && userTypes.Contains(SharedItem.UserType)
                                    && (String.IsNullOrEmpty(complioID) || complioID.Equals(SharedItem.ComplioID))
                                    && (String.IsNullOrEmpty(customAttribute) || customAttribute.Equals(SharedItem.CustomAttributeValue))
                                    && ((rotationStartDate == null && rotationEndDate == null) || ((rotationStartDate != null && rotationEndDate != null && rotationStartDate <= rotationEndDate && SharedItem.RotationStartDate != null && SharedItem.RotationEndDate != null)
                                                && ((SharedItem.RotationStartDate >= rotationStartDate && SharedItem.RotationStartDate <= rotationEndDate) || (SharedItem.RotationEndDate >= rotationStartDate && SharedItem.RotationEndDate <= rotationEndDate)
                                                || (rotationStartDate >= SharedItem.RotationStartDate && rotationStartDate <= SharedItem.RotationEndDate) || (rotationEndDate >= SharedItem.RotationStartDate && rotationEndDate <= SharedItem.RotationEndDate)
                                                ))))
                                    .ToList().GroupBy(x => new { x.TenantID, x.StudentID, x.RotationID, x.CategoryID, x.ItemID, x.FieldID }).Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault())
                                    .Where(x => reviewStatus.Contains(x.ReviewCode))
                                    .ToList();
        }

        public List<SharedItem> GetSharedDataPerRotation(List<Int32> invitationGroups, List<Int32> tenants, List<Int32> agencies,
        List<String> categories, List<String> items, DateTime? rotationStartDate, DateTime? rotationEndDate, List<String> reviewStatus, List<String> userTypes)
        {
            return _SharedItems.Find(SharedItem => invitationGroups.Contains(SharedItem.InvitationGroupID)
                                    && tenants.Contains(SharedItem.TenantID) && agencies.Contains(SharedItem.AgencyID)
                                    && categories.Contains(SharedItem.CategoryID) && items.Contains(SharedItem.ItemID)
                                    && userTypes.Contains(SharedItem.UserType)
                                    && ((rotationStartDate == null && rotationEndDate == null) || ((rotationStartDate != null && rotationEndDate != null && rotationStartDate <= rotationEndDate && SharedItem.RotationStartDate != null && SharedItem.RotationEndDate != null)
                                    && ((SharedItem.RotationStartDate >= rotationStartDate && SharedItem.RotationStartDate <= rotationEndDate) || (SharedItem.RotationEndDate >= rotationStartDate && SharedItem.RotationEndDate <= rotationEndDate)
                                    || (rotationStartDate >= SharedItem.RotationStartDate && rotationStartDate <= SharedItem.RotationEndDate) || (rotationEndDate >= SharedItem.RotationStartDate && rotationEndDate <= SharedItem.RotationEndDate)
                                    ))))
                                    .ToList().GroupBy(x => new { x.TenantID, x.StudentID, x.RotationID, x.CategoryID, x.ItemID, x.FieldID }).Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault())
                                    .Where(x => reviewStatus.Contains(x.ReviewCode))
                                    .ToList();
        }

        public List<SharedItem> GetItemSubmissionCount(List<Int32> invitationGroups, List<Int32> tenants, List<Int32> agencies, List<String> categories, List<String> items
            , DateTime? fromDate, DateTime? toDate, List<String> userTypes, Boolean isUniqueResultsOnly)
        {
            List<SharedItem> sharedItemsList = new List<SharedItem>();
            sharedItemsList = _SharedItems.Find(SharedItem => invitationGroups.Contains(SharedItem.InvitationGroupID)
                        && tenants.Contains(SharedItem.TenantID) && agencies.Contains(SharedItem.AgencyID)
                        && categories.Contains(SharedItem.CategoryID)
                        && items.Contains(SharedItem.ItemID)
                        && userTypes.Contains(SharedItem.UserType)
                        && ((fromDate == null && toDate == null)
                        || ((fromDate != null && toDate != null && SharedItem.ItemSubmissionDate != null && fromDate <= toDate)
                        && (SharedItem.ItemSubmissionDate >= fromDate && SharedItem.ItemSubmissionDate <= toDate)))).ToList();
            if (!sharedItemsList.IsNullOrEmpty() && sharedItemsList.Count > 0)
            {
                if (isUniqueResultsOnly)
                {
                    sharedItemsList = sharedItemsList.GroupBy(x => new { x.TenantID, x.StudentID, x.CategoryID, x.ItemID })
                                       .Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault()).ToList();
                }
                else
                {
                    sharedItemsList = sharedItemsList.GroupBy(x => new { x.TenantID, x.StudentID, x.RotationID, x.CategoryID, x.ItemID })
                                        .Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault()).ToList();
                }
            }
            return sharedItemsList;
        }

        public List<SharedItem> CreateMany(List<SharedItem> SharedItems)
        {
            _SharedItems.InsertMany(SharedItems);
            return SharedItems;
        }


        public SharedItem Create(SharedItem SharedItem)
        {
            _SharedItems.InsertOne(SharedItem);
            return SharedItem;
        }

        public SharedItem Replace(SharedItem SharedItem)
        {
            _SharedItems.DeleteOne(SI => SharedItem.InvitationGroupID == SI.InvitationGroupID);
            _SharedItems.InsertOne(SharedItem);
            return SharedItem;
        }

        public void Update(string id, SharedItem SharedItemIn)
        {
            _SharedItems.ReplaceOne(SharedItem => SharedItem.ID == id, SharedItemIn);
        }
        public void Remove(Int32 invitationGroup, Int32 tenantID)
        {
            _SharedItems.DeleteMany(SharedItem => SharedItem.InvitationGroupID == invitationGroup && SharedItem.TenantID == tenantID);
        }

        public void RemoveMany(List<Int32> invitationGroups, Int32 tenantID)
        {
            _SharedItems.DeleteMany(SharedItem => invitationGroups.Contains(SharedItem.InvitationGroupID)  && SharedItem.TenantID == tenantID);
        }

        public void Remove(SharedItem SharedItemIn)
        {
            _SharedItems.DeleteOne(SharedItem => SharedItem.ID == SharedItemIn.ID);
        }

        public void Remove(string SharedItemId)
        {
            _SharedItems.DeleteOne(SharedItem => SharedItem.ID == SharedItemId);
        }

        public void RemoveAll()
        {
            _SharedItems.DeleteMany(SharedItem => true);
        }

        public Boolean LockVersion(List<Int32> modifiedInvitationGroups,DateTime syncDate, Boolean isSuccess)
        {
            CollectionVersionService collectionVersionService = DataMartUtils.GetCollectionVersionServiceInstance();
            CollectionVersion collectionVersion = collectionVersionService.Get(DataMartCollections.SharedItems.GetStringValue());
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
                    //Database.GetCollection<SharedItem>(readVersion).DeleteMany(x => modifiedInvitationGroups.Contains(x.InvitationGroupID));
                    //Database.GetCollection<SharedItem>(readVersion).InsertMany(Database.GetCollection<SharedItem>(writeVersion).Find(x => modifiedInvitationGroups.Contains(x.InvitationGroupID)).ToList());
                    return true;
                }
                else
                {
                    collectionVersion.WasLastSyncSuccess = false;
                    collectionVersionService.Update(collectionVersion.ID, collectionVersion);
                    //Database.GetCollection<SharedItem>(collectionVersion.WriteVersion).DeleteMany(x => modifiedInvitationGroups.Contains(x.InvitationGroupID));
                    //Database.GetCollection<SharedItem>(collectionVersion.WriteVersion).InsertMany(Database.GetCollection<SharedItem>(collectionVersion.ReadVersion).Find(x => modifiedInvitationGroups.Contains(x.InvitationGroupID)).ToList());
                    return true;
                }
            }
            return false;
        }
    }
}