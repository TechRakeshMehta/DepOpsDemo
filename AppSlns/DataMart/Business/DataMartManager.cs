#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SecurityManager.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataMart.Models;
using DataMart.Services;
using DataMart.UI.Contracts;
using DataMart.Utils;
using Entity;
using Entity.SharedDataEntity;
using MongoDB.Driver;

#endregion

#region Application Specific

using INTSOF.Utils;
#endregion

#endregion

namespace DataMart.Business.RepoManagers
{
    /// <summary>
    /// This is a business class for security module, which handles the operations at business layer.
    /// </summary>
    /// <remarks></remarks>
    public static class DataMartManager
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static DataMartManager()
        {
            BALUtils.ClassModule = DataMartConsts.DATA_MART_MANAGER;
        }

        #endregion

        #region Properties

        #region public Properties

        public static Int32 DefaultTenantID
        {
            get
            {
                return 1;
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get all the agency users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>G
        public static Boolean InitializeCollections()
        {
            IntializeCollection(DataMartCollections.AgencyUsers.GetStringValue());
            IntializeCollection(DataMartCollections.SharedItems.GetStringValue());
            IntializeCollection(DataMartCollections.RotationDetails.GetStringValue());
            return true;
        }

        private static void IntializeCollection(string collectionName)
        {
            CollectionVersion collectionVersion = DataMartUtils.GetCollectionVersionServiceInstance().Get(collectionName);
            if (collectionVersion.IsNull())
            {
                collectionVersion = new CollectionVersion();
                collectionVersion.CollectionName = collectionName;
                collectionVersion.ReadVersion = collectionName + DataMartConsts.COLLECTION_REPLICA_1;
                collectionVersion.WriteVersion = collectionName + DataMartConsts.COLLECTION_REPLICA_2;
                collectionVersion.LastSyncDate = Convert.ToDateTime(DataMartConsts.INITIAL_SYNC_DATE);
                collectionVersion.WasLastSyncSuccess = true;
                DataMartUtils.GetCollectionVersionServiceInstance().Create(collectionVersion);
            }
        }

        #region AgencyUsers

        /// <summary>
        /// Get all the agency users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<DataMart.Models.AgencyUser> GetAgencyUsers(DateTime lastSyncDate)
        {
            try
            {
                List<Int32> modifiedAgencyUsers = BALUtils.GetDataMartRepoInstance().GetModifiedAgencyUsers(lastSyncDate).Select(x => Convert.ToInt32(x["AgencyUserID"])).ToList();
                IEnumerable<DataRow> agencies = BALUtils.GetDataMartRepoInstance().GetAgenciesOfAgencyUser();
                List<lkpInvitationSharedInfoType> sharedInfoTypes = LookupManager.GetSharedDBLookUpData<lkpInvitationSharedInfoType>();
                List<DataMart.Models.AgencyUser> returnObject = new List<Models.AgencyUser>();
                if (modifiedAgencyUsers.IsNotNull())
                {
                    returnObject = BALUtils.GetDataMartRepoInstance().GetAgencyUsers().Where(cond => modifiedAgencyUsers.Contains(cond.AGU_ID))
                    .ToList().Select(agencyUser => new DataMart.Models.AgencyUser
                    {
                        AgencyUserID = agencyUser.AGU_ID,
                        AgencyUserEmail = agencyUser.AGU_Email,
                        AgencyUserName = agencyUser.AGU_Name,
                        UserID = agencyUser.AGU_UserID.HasValue ? agencyUser.AGU_UserID.Value.ToString() : String.Empty,
                        HasComplianceAccess = !sharedInfoTypes.Any(x => x.Code == "AAAC" && x.MasterInfoTypeCode == "IMM" && x.SharedInfoTypeID == agencyUser.AGU_ComplianceSharedInfoTypeID),
                        HasRotationAccess = !sharedInfoTypes.Any(x => x.Code == "AAAJ" && x.MasterInfoTypeCode == "REQROT" && x.SharedInfoTypeID == agencyUser.AGU_ReqRotationSharedInfoTypeID),
                        SharingInvitations = agencyUser.ProfileSharingInvitations.Where(cond => !cond.PSI_IsDeleted).Select(x => new DataMart.Models.SharingInvitation
                        {
                            InvitationGroupID = x.PSI_ProfileSharingInvitationGroupID.HasValue ? x.PSI_ProfileSharingInvitationGroupID.Value : AppConsts.NONE,
                            InvitationID = x.PSI_ID
                        }).ToList(),
                        Agencies = agencies.Where(agency => Convert.ToInt32(agency["AgencyUserID"]) == agencyUser.AGU_ID).Select(agency => new DataMart.Models.Agency
                        {
                            AgencyID = agency["AgencyID"].IsNotNull() ? Convert.ToInt32(agency["AgencyID"]) : AppConsts.NONE,
                            AgencyName = agency["AgencyName"].IsNotNull() ? agency["AgencyName"].ToString() : String.Empty,
                            NodeID = agency["NodeID"].IsNotNull() ? Convert.ToInt32(agency["NodeID"]) : AppConsts.NONE,
                            NodeName = agency["NodeName"].IsNotNull() ? agency["NodeName"].ToString() : String.Empty,
                            TenantID = agency["TenantID"].IsNotNull() ? Convert.ToInt32(agency["TenantID"]) : AppConsts.NONE,
                            TenantName = agency["TenantName"].IsNotNull() ? agency["TenantName"].ToString() : String.Empty
                        }).ToList()
                    }).ToList();
                }

                return returnObject;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get all the agency users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<Int32> GetProfileSharingInvitationGroups(Int32 tenantID, DateTime lastSyncDate)
        {
            try
            {
                List<Int32> returnObject = new List<Int32>();
                var invitationGroups = BALUtils.GetDataMartRepoInstance(tenantID).GetModifiedInvitationGroups(lastSyncDate);
                if (invitationGroups.IsNotNull()) returnObject = invitationGroups.Select(x => Convert.ToInt32(x["PSIG_ID"])).ToList();
                return returnObject;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get all the agency users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<Int32> GetModifiedRotationDetails(Int32 tenantID, DateTime lastSyncDate)
        {
            try
            {
                List<Int32> returnObject = new List<Int32>();
                var invitationGroups = BALUtils.GetDataMartRepoInstance(tenantID).GetModifiedInvitationGroups(lastSyncDate);
                if (invitationGroups.IsNotNull()) returnObject = invitationGroups.Select(x => Convert.ToInt32(x["PSIG_ID"])).ToList();
                return returnObject;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get shared items of agency user
        /// </summary>
        /// <param name="agencyUserEmail"></param>
        /// <returns></returns>
        public static List<DataMart.Models.SharedItem> GetSharedItemsOfInvitationGroup(Int32 tenantID, String invitationGroupIDs)
        {
            try
            {
                var sharedItems = BALUtils.GetDataMartRepoInstance(tenantID).GetSharedItemsOfInvitationGroup(invitationGroupIDs).Select(sharedItem => new DataMart.Models.SharedItem
                {
                    InvitationGroupID = sharedItem["InvitationGroupID"] != DBNull.Value ? Convert.ToInt32(sharedItem["InvitationGroupID"]) : AppConsts.NONE,
                    AgencyID = sharedItem["AgencyID"] != DBNull.Value ? Convert.ToInt32(sharedItem["AgencyID"]) : AppConsts.NONE,
                    TenantID = sharedItem["TenantID"] != DBNull.Value ? Convert.ToInt32(sharedItem["TenantID"]) : AppConsts.NONE,
                    TenantName = sharedItem["TenantName"] != DBNull.Value ? sharedItem["TenantName"].ToString() : String.Empty,
                    ApprovedOverrideStatus = sharedItem["ApprovedOverrideStatus"] != DBNull.Value ? sharedItem["ApprovedOverrideStatus"].ToString() : String.Empty,
                    CategoryComplianceStatus = sharedItem["CategoryComplianceStatus"] != DBNull.Value ? sharedItem["CategoryComplianceStatus"].ToString() : String.Empty,
                    CategoryID = sharedItem["CategoryID"] != DBNull.Value ? sharedItem["TenantID"].ToString() + "_" + sharedItem["CategoryID"].ToString() : String.Empty,
                    CategoryName = sharedItem["CategoryName"] != DBNull.Value ? sharedItem["CategoryName"].ToString() : String.Empty,
                    ComplioID = sharedItem["ComplioID"] != DBNull.Value ? sharedItem["ComplioID"].ToString() : String.Empty,
                    FieldData = sharedItem["FieldData"] != DBNull.Value ? sharedItem["FieldData"].ToString() : String.Empty,
                    FieldDataID = sharedItem["FieldDataID"] != DBNull.Value ? sharedItem["TenantID"].ToString() + "_" + sharedItem["FieldDataID"].ToString() : String.Empty,
                    FieldDisplayOrder = sharedItem["FieldDisplayOrder"] != DBNull.Value ? Convert.ToInt32(sharedItem["FieldDisplayOrder"]) : AppConsts.NONE,
                    FieldID = sharedItem["FieldID"] != DBNull.Value ? sharedItem["TenantID"].ToString() + "_" + sharedItem["FieldID"].ToString() : String.Empty,
                    FieldName = sharedItem["FieldName"] != DBNull.Value ? sharedItem["FieldName"].ToString() : String.Empty,
                    FirstName = sharedItem["FirstName"] != DBNull.Value ? sharedItem["FirstName"].ToString() : String.Empty,
                    ItemDataID = sharedItem["ItemDataID"] != DBNull.Value ? sharedItem["TenantID"].ToString() + "_" + sharedItem["ItemDataID"].ToString() : String.Empty,
                    ItemDisplayOrder = sharedItem["ItemDisplayOrder"] != DBNull.Value ? Convert.ToInt32(sharedItem["ItemDisplayOrder"]) : AppConsts.NONE,
                    ItemID = sharedItem["ItemID"] != DBNull.Value ? sharedItem["TenantID"].ToString() + "_" + sharedItem["ItemID"].ToString() : String.Empty,
                    ItemName = sharedItem["ItemName"] != DBNull.Value ? sharedItem["ItemName"].ToString() : String.Empty,
                    LastName = sharedItem["LastName"] != DBNull.Value ? sharedItem["LastName"].ToString() : String.Empty,
                    RotationEndDate = sharedItem["RotationEndDate"] != DBNull.Value ? Convert.ToDateTime(sharedItem["RotationEndDate"]) : (DateTime?)null,
                    RotationID = sharedItem["RotationID"] != DBNull.Value ? sharedItem["TenantID"].ToString() + "_" + sharedItem["RotationID"].ToString() : String.Empty,
                    RotationStartDate = sharedItem["RotationStartDate"] != DBNull.Value ? Convert.ToDateTime(sharedItem["RotationStartDate"]) : (DateTime?)null,
                    SharedItemType = sharedItem["SharedItemType"] != DBNull.Value ? sharedItem["SharedItemType"].ToString() : String.Empty,
                    StudentID = sharedItem["StudentID"] != DBNull.Value ? Convert.ToInt32(sharedItem["StudentID"]) : AppConsts.NONE,
                    CustomAttributeValue = sharedItem["CustomAttributeValue"] != DBNull.Value ? sharedItem["CustomAttributeValue"].ToString() : String.Empty,
                    InvitationReviewStatusName = sharedItem["InvitationReviewStatusName"] != DBNull.Value ? sharedItem["InvitationReviewStatusName"].ToString() : String.Empty,
                    ReviewCode = sharedItem["ReviewCode"] != DBNull.Value ? sharedItem["ReviewCode"].ToString() : String.Empty,
                    UserType = sharedItem["UserType"] != DBNull.Value ? sharedItem["UserType"].ToString() : String.Empty,
                    ItemSubmissionDate = sharedItem["ItemSubmissionDate"] != DBNull.Value ? Convert.ToDateTime(sharedItem["ItemSubmissionDate"]) : (DateTime?)null,
                    SharedBy = sharedItem["SharedBy"] != DBNull.Value ? sharedItem["SharedBy"].ToString() : String.Empty,
                    SharedByEmail = sharedItem["SharedByEmail"] != DBNull.Value ? sharedItem["SharedByEmail"].ToString() : String.Empty,
                    ReviewedBy = sharedItem["ReviewedBy"] != DBNull.Value ? sharedItem["ReviewedBy"].ToString() : String.Empty,
                    StudentEmail = sharedItem["StudentEmail"] != DBNull.Value ? sharedItem["StudentEmail"].ToString() : String.Empty,
                    OutOfComplianceDate = sharedItem["OutOfComplianceDate"] != DBNull.Value ? Convert.ToDateTime(sharedItem["OutOfComplianceDate"]) : (DateTime?)null,
                    AgencyName = sharedItem["AgencyName"] != DBNull.Value ? sharedItem["AgencyName"].ToString() : String.Empty,
                    ReviewedDate = sharedItem["ReviewedDate"] != DBNull.Value ? Convert.ToDateTime(sharedItem["ReviewedDate"]) : (DateTime?)null
                }).ToList();

                return sharedItems;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get shared items of agency user
        /// </summary>
        /// <param name="agencyUserEmail"></param>
        /// <returns></returns>
        public static List<DataMart.Models.RotationDetail> GetRotationDetailsOfInvitationGroup(Int32 tenantID, String invitationGroupIDs)
        {
            try
            {
                var sharedItems = BALUtils.GetDataMartRepoInstance(tenantID).GetRotationDetailsOfInvitationGroup(invitationGroupIDs).Select(sharedItem => new DataMart.Models.RotationDetail
                {
                    InvitationGroupID = sharedItem["InvitationGroupID"] != DBNull.Value ? Convert.ToInt32(sharedItem["InvitationGroupID"]) : AppConsts.NONE,
                    AgencyID = sharedItem["AgencyID"] != DBNull.Value ? Convert.ToInt32(sharedItem["AgencyID"]) : AppConsts.NONE,
                    TenantID = sharedItem["TenantID"] != DBNull.Value ? Convert.ToInt32(sharedItem["TenantID"]) : AppConsts.NONE,
                    TenantName = sharedItem["TenantName"] != DBNull.Value ? sharedItem["TenantName"].ToString() : String.Empty,
                    ComplioID = sharedItem["ComplioID"] != DBNull.Value ? sharedItem["ComplioID"].ToString() : String.Empty,
                    RotationEndDate = sharedItem["RotationEndDate"] != DBNull.Value ? Convert.ToDateTime(sharedItem["RotationEndDate"]) : (DateTime?)null,
                    RotationID = sharedItem["RotationID"] != DBNull.Value ? sharedItem["TenantID"].ToString() + "_" + sharedItem["RotationID"].ToString() : String.Empty,
                    RotationStartDate = sharedItem["RotationStartDate"] != DBNull.Value ? Convert.ToDateTime(sharedItem["RotationStartDate"]) : (DateTime?)null,
                    StudentID = sharedItem["StudentID"] != DBNull.Value ? Convert.ToInt32(sharedItem["StudentID"]) : AppConsts.NONE,
                    InvitationReviewStatusName = sharedItem["InvitationReviewStatusName"] != DBNull.Value ? sharedItem["InvitationReviewStatusName"].ToString() : String.Empty,
                    ReviewCode = sharedItem["ReviewCode"] != DBNull.Value ? sharedItem["ReviewCode"].ToString() : String.Empty,
                    UserType = sharedItem["UserType"] != DBNull.Value ? sharedItem["UserType"].ToString() : String.Empty,
                    SharedBy = sharedItem["SharedBy"] != DBNull.Value ? sharedItem["SharedBy"].ToString() : String.Empty,
                    SharedByEmail = sharedItem["SharedByEmail"] != DBNull.Value ? sharedItem["SharedByEmail"].ToString() : String.Empty,
                    ReviewedBy = sharedItem["ReviewedBy"] != DBNull.Value ? sharedItem["ReviewedBy"].ToString() : String.Empty,
                    AgencyName = sharedItem["AgencyName"] != DBNull.Value ? sharedItem["AgencyName"].ToString() : String.Empty,
                    Course = sharedItem["Course"] != DBNull.Value ? sharedItem["Course"].ToString() : String.Empty,
                    CustomAttributes = sharedItem["CustomAttributes"] != DBNull.Value ? sharedItem["CustomAttributes"].ToString() : String.Empty,
                    Days = sharedItem["Days"] != DBNull.Value ? sharedItem["Days"].ToString() : String.Empty,
                    Department = sharedItem["Department"] != DBNull.Value ? sharedItem["Department"].ToString() : String.Empty,
                    InvitationSourceCode = sharedItem["InvitationSourceCode"] != DBNull.Value ? sharedItem["InvitationSourceCode"].ToString() : String.Empty,
                    InvitationSourceType = sharedItem["InvitationSourceType"] != DBNull.Value ? sharedItem["InvitationSourceType"].ToString() : String.Empty,
                    Program = sharedItem["Program"] != DBNull.Value ? sharedItem["Program"].ToString() : String.Empty,
                    RotationName = sharedItem["RotationName"] != DBNull.Value ? sharedItem["RotationName"].ToString() : String.Empty,
                    RotationShift = sharedItem["RotationShift"] != DBNull.Value ? sharedItem["RotationShift"].ToString() : String.Empty,
                    StudentEmailAddress = sharedItem["StudentEmailAddress"] != DBNull.Value ? sharedItem["StudentEmailAddress"].ToString() : String.Empty,
                    StudentFirstName = sharedItem["StudentFirstName"] != DBNull.Value ? sharedItem["StudentFirstName"].ToString() : String.Empty,
                    StudentLastName = sharedItem["StudentLastName"] != DBNull.Value ? sharedItem["StudentLastName"].ToString() : String.Empty,
                    Term = sharedItem["Term"] != DBNull.Value ? sharedItem["Term"].ToString() : String.Empty,
                    Times = sharedItem["Times"] != DBNull.Value ? sharedItem["Times"].ToString() : String.Empty,
                    TypeSpecialty = sharedItem["TypeSpecialty"] != DBNull.Value ? sharedItem["TypeSpecialty"].ToString() : String.Empty,
                    UnitFloorLoc = sharedItem["UnitFloorLoc"] != DBNull.Value ? sharedItem["UnitFloorLoc"].ToString() : String.Empty,
                    StudentDOB = sharedItem["StudentDOB"] != DBNull.Value ? sharedItem["StudentDOB"].ToString() : String.Empty,
                    StudentPhoneNumber = sharedItem["StudentPhoneNumber"] != DBNull.Value ? sharedItem["StudentPhoneNumber"].ToString() : String.Empty,
                    StudentAddress = sharedItem["StudentAddress"] != DBNull.Value ? sharedItem["StudentAddress"].ToString() : String.Empty,
                    ReviewedDate = sharedItem["ReviewedDate"] != DBNull.Value ? Convert.ToDateTime(sharedItem["ReviewedDate"]) : (DateTime?)null
                }).ToList();

                return sharedItems;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get shared items of agency user
        /// </summary>
        /// <param name="agencyUserEmail"></param>
        /// <returns></returns>
        public static Boolean SaveSharedItems(List<DataMart.Models.SharedItem> sharedItems, List<Int32> invitationGroups, Int32 tenantID)
        {
            try
            {
                if (sharedItems.IsNotNull() && sharedItems.Count() > 0)
                {
                    SharedItemService sharedItemService = DataMartUtils.GetSharedItemServiceInstance(AccessIntent.Write);
                    sharedItemService.RemoveMany(invitationGroups, tenantID);
                    sharedItemService.CreateMany(sharedItems);
                }
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get shared items of agency user
        /// </summary>
        /// <param name="agencyUserEmail"></param>
        /// <returns></returns>
        public static Boolean SaveRotationDetails(List<DataMart.Models.RotationDetail> rotationDetails, List<Int32> invitationGroups, Int32 tenantID)
        {
            try
            {
                if (rotationDetails.IsNotNull() && rotationDetails.Count() > 0)
                {
                    RotationDetailService rotationDetailService = DataMartUtils.GetRotationDetailServiceInstance(AccessIntent.Write);
                    rotationDetailService.RemoveMany(invitationGroups, tenantID);
                    rotationDetailService.CreateMany(rotationDetails);
                }
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get shared items of agency user
        /// </summary>
        /// <param name="agencyUserEmail"></param>
        /// <returns></returns>
        public static Boolean SaveAgencyUsers(List<DataMart.Models.AgencyUser> agencyUsers)
        {
            try
            {
                DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance(AccessIntent.Write);
                agencyUsers.ForEach(agencyUser => agencyUserService.Replace(agencyUser));
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static DateTime GetLastSyncDate(String collectionName)
        {
            var collectionVersion = DataMartUtils.GetCollectionVersionServiceInstance().Get(collectionName);
            if (collectionVersion.IsNotNull())
            {
                return collectionVersion.LastSyncDate;
            }
            throw new Exception("Version not initialized for collection: " + collectionName);
        }

        /// <summary>
        /// Get shared items of agency user
        /// </summary>
        /// <param name="agencyUserEmail"></param>
        /// <returns></returns>
        public static Boolean LockAgencyUsers(List<Int32> modifiedAgencyUsers,DateTime syncDate, Boolean isSuccess)
        {
            return DataMartUtils.GetAgencyUserServiceInstance().LockVersion(modifiedAgencyUsers, syncDate, isSuccess);
        }

        /// <summary>
        /// Get shared items of agency user
        /// </summary>
        /// <param name="agencyUserEmail"></param>
        /// <returns></returns>
        public static Boolean LockInvitationGroups(List<Int32> modifiedInvitationGroups, DateTime syncDate, Boolean isSuccess)
        {
            return DataMartUtils.GetSharedItemServiceInstance().LockVersion(modifiedInvitationGroups, syncDate, isSuccess);
        }

        public static Boolean LockRotationDetails(List<Int32> modifiedRotationDetails, DateTime syncDate, Boolean isSuccess)
        {
            return DataMartUtils.GetRotationDetailServiceInstance().LockVersion(modifiedRotationDetails, syncDate, isSuccess);
        }


        public static Dictionary<Int32, String> GetMappedTenants(String userID)
        {
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            Dictionary<Int32, String> tenants = new Dictionary<Int32, String>();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull() && agencyUser.Agencies.IsNotNull())
            {
                agencyUser.Agencies.Select(agency => new KeyValuePair<Int32, String>(agency.TenantID, agency.TenantName)).Distinct().ForEach(x =>
                {
                    tenants.Add(x.Key, x.Value);
                });
            }
            return tenants;
        }

        public static List<DropDownContract> GetAgencyUserDropDownContracts(String userID, List<Int32> tenantIDs)
        {
            List<DropDownContract> result = new List<DropDownContract>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                List<String> itemTypes = new List<string>();
                if (agencyUser.HasComplianceAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Tracking.GetStringValue());
                }
                if (agencyUser.HasRotationAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Rotation.GetStringValue());
                }
                result = DataMartUtils.GetSharedItemServiceInstance()
                    .Get(invitationGroups, tenantIDs, agencyUser.Agencies.Select(x => x.AgencyID).ToList())
                    .Project(x => new DropDownContract
                    {
                        AgencyID = x.AgencyID,
                        //AgencyName = agencyUser.Agencies.Where(au=>au.AgencyID == x.AgencyID).FirstOrDefault().AgencyName,
                        CategoryID = x.CategoryID,
                        CategoryName = x.CategoryName,
                        ItemID = x.ItemID,
                        ItemName = x.ItemName
                    }).ToList().DistinctBy(x => new { x.AgencyID, x.CategoryID, x.ItemID }).ToList();

                result.ForEach(x => x.AgencyName = agencyUser.Agencies.Where(au => au.AgencyID == x.AgencyID).FirstOrDefault().AgencyName);
            }
            return result;
        }

        public static Dictionary<Int32, String> GetAgencies(String userID, List<Int32> tenantIDs)
        {
            Dictionary<Int32, String> result = new Dictionary<Int32, String>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull() && agencyUser.Agencies.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                List<String> itemTypes = new List<string>();
                if (agencyUser.HasComplianceAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Tracking.GetStringValue());
                }
                if (agencyUser.HasRotationAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Rotation.GetStringValue());
                }
                List<Int32> agenciesWithData = new List<Int32>();
                agenciesWithData = DataMartUtils.GetSharedItemServiceInstance()
                    .Get(invitationGroups, tenantIDs, agencyUser.Agencies.Select(x => x.AgencyID).ToList())
                    .Project(x => x.AgencyID).ToList();
                result = agencyUser.Agencies.Where(x => agenciesWithData.Contains(x.AgencyID)).DistinctBy(x => x.AgencyID).ToDictionary(x => x.AgencyID, x => x.AgencyName);
            }
            return result;
        }

        public static Dictionary<Int32, String> GetRotationDetailAgencies(String userID, List<Int32> tenantIDs)
        {
            Dictionary<Int32, String> result = new Dictionary<Int32, String>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull() && agencyUser.Agencies.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                List<String> itemTypes = new List<string>();
                if (agencyUser.HasComplianceAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Tracking.GetStringValue());
                }
                if (agencyUser.HasRotationAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Rotation.GetStringValue());
                }
                List<Int32> agenciesWithData = new List<Int32>();
                agenciesWithData = DataMartUtils.GetRotationDetailServiceInstance()
                    .Get(invitationGroups, tenantIDs, agencyUser.Agencies.Select(x => x.AgencyID).ToList())
                    .Project(x => x.AgencyID).ToList();
                result = agencyUser.Agencies.Where(x => agenciesWithData.Contains(x.AgencyID)).DistinctBy(x => x.AgencyID).ToDictionary(x => x.AgencyID, x => x.AgencyName);
            }
            return result;
        }

        public static Dictionary<String, String> GetCategories(String userID, List<Int32> tenantIDs, List<Int32> agencies)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                List<String> itemTypes = new List<string>();
                if (agencyUser.HasComplianceAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Tracking.GetStringValue());
                }
                if (agencyUser.HasRotationAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Rotation.GetStringValue());
                }

                result = DataMartUtils.GetSharedItemServiceInstance()
                    .Get(invitationGroups, tenantIDs, agencies)
                    .Project(x => new { x.CategoryID, x.CategoryName }).ToList().DistinctBy(x => x.CategoryID).ToDictionary(x => x.CategoryID, x => x.CategoryName);
            }
            return result;
        }

        public static Dictionary<String, String> GetItems(String userID, List<Int32> tenantIDs, List<Int32> agencies, List<String> categories)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                List<String> itemTypes = new List<string>();
                if (agencyUser.HasComplianceAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Tracking.GetStringValue());
                }
                if (agencyUser.HasRotationAccess)
                {
                    itemTypes.Add(AgencyUserItemAccess.Rotation.GetStringValue());
                }

                result = DataMartUtils.GetSharedItemServiceInstance()
                    .Get(invitationGroups, tenantIDs, agencies, categories)
                    .Project(x => new { x.ItemID, x.ItemName }).ToList().DistinctBy(x => x.ItemID).ToDictionary(x => x.ItemID, x => x.ItemName);
            }
            return result;
        }

        public static Dictionary<String, String> GetInvitationReviewStatus()
        {
            Dictionary<String, String> status = new Dictionary<String, String>();
            status.Add("AAAA", "Pending Review");
            status.Add("AAAB", "Approved");
            status.Add("AAAC", "Not Approved");
            status.Add("AAAD", "Dropped");
            return status;
        }

        public static Dictionary<String, String> GetUserTypes()
        {
            Dictionary<String, String> userTypes = new Dictionary<String, String>();
            userTypes.Add("Student", "Student");
            userTypes.Add("Instructor", "Instructor");
            return userTypes;
        }

        public static List<CategoryDataReportContract> GetCategoryDataReportContracts(String userID, List<Int32> tenants, List<Int32> agencies, List<String> categories
            , List<String> items, DateTime? rotationStartDate, DateTime? rotationEndDate
            , List<String> reviewStatus, List<String> userType)
        {
            List<CategoryDataReportContract> categoryDataReportContracts = new List<CategoryDataReportContract>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                categoryDataReportContracts = DataMartUtils.GetSharedItemServiceInstance().GetSharedDataPerRotation(invitationGroups, tenants, agencies, categories, items, rotationStartDate, rotationEndDate, reviewStatus, userType)
                                              .Select(sharedItem => new CategoryDataReportContract
                                              {
                                                  ID = sharedItem.ID,
                                                  ApprovedOverrideStatus = sharedItem.ApprovedOverrideStatus,
                                                  CategoryComplianceStatus = sharedItem.CategoryComplianceStatus,
                                                  CategoryName = sharedItem.CategoryName,
                                                  FieldData = sharedItem.FieldData,
                                                  FieldName = sharedItem.FieldName,
                                                  FirstName = sharedItem.FirstName,
                                                  ItemName = sharedItem.ItemName,
                                                  LastName = sharedItem.LastName,
                                                  RotationEndDate = sharedItem.RotationEndDate,
                                                  RotationStartDate = sharedItem.RotationStartDate,
                                                  SharedItemType = sharedItem.SharedItemType,
                                                  TenantName = sharedItem.TenantName,
                                                  ReviewStatus = sharedItem.InvitationReviewStatusName,
                                                  UserType = sharedItem.UserType
                                              }).OrderBy(x => x.TenantName + x.FirstName + x.LastName + x.CategoryName + x.ItemName + x.FieldName).ToList();
            }
            return categoryDataReportContracts;
        }

        public static List<CategoryDataReportWithComplioIDContract> GetCategoryDataReportWithComplioIDContracts(String userID, List<Int32> tenants, List<Int32> agencies, List<String> categories,
            List<String> items, string complioID, string customAttribute, DateTime? rotationStartDate, DateTime? rotationEndDate
            , List<String> reviewStatus, List<String> userType)
        {
            List<CategoryDataReportWithComplioIDContract> categoryDataReportContracts = new List<CategoryDataReportWithComplioIDContract>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                categoryDataReportContracts = DataMartUtils.GetSharedItemServiceInstance().GetSharedDataPerRotation(invitationGroups, tenants, agencies, categories, items, complioID, customAttribute
                                            , rotationStartDate, rotationEndDate, reviewStatus, userType).
                                              Select(sharedItem => new CategoryDataReportWithComplioIDContract
                                              {
                                                  ApprovedOverrideStatus = sharedItem.ApprovedOverrideStatus,
                                                  CategoryComplianceStatus = sharedItem.CategoryComplianceStatus,
                                                  CategoryName = sharedItem.CategoryName,
                                                  FieldData = sharedItem.FieldData,
                                                  FieldName = sharedItem.FieldName,
                                                  FirstName = sharedItem.FirstName,
                                                  ID = sharedItem.ID,
                                                  ItemName = sharedItem.ItemName,
                                                  LastName = sharedItem.LastName,
                                                  RotationEndDate = sharedItem.RotationEndDate,
                                                  RotationStartDate = sharedItem.RotationStartDate,
                                                  SharedItemType = sharedItem.SharedItemType,
                                                  TenantName = sharedItem.TenantName,
                                                  ComplioID = sharedItem.ComplioID,
                                                  CustomAttribute = sharedItem.CustomAttributeValue,
                                                  ReviewStatus = sharedItem.InvitationReviewStatusName,
                                                  UserType = sharedItem.UserType
                                              }).OrderBy(x => x.TenantName + x.FirstName + x.LastName + x.CategoryName + x.ItemName + x.FieldName).ToList();
            }
            return categoryDataReportContracts;
        }

        public static List<ItemDataCountReportContract> GetItemDataCountReportContract(String userID, List<Int32> tenants, List<Int32> agencies, List<String> categories, List<String> items
            , DateTime? fromDate, DateTime? toDate, List<String> userType, Boolean isUniqueResultsOnly)
        {
            List<ItemDataCountReportContract> itemDataCountReportContracts = new List<ItemDataCountReportContract>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                itemDataCountReportContracts = DataMartUtils.GetSharedItemServiceInstance().GetItemSubmissionCount(invitationGroups, tenants, agencies, categories, items, fromDate, toDate, userType, isUniqueResultsOnly)
                    .GroupBy(x => new { x.TenantName, x.SharedItemType, x.ItemName }).
                                              Select(sharedItem => new ItemDataCountReportContract
                                              {
                                                  ItemCount = sharedItem.Count(),
                                                  ItemName = sharedItem.Key.ItemName,
                                                  TenantName = sharedItem.Key.TenantName,
                                                  PackageType = sharedItem.Key.SharedItemType
                                              }).OrderBy(x => x.TenantName + x.ItemName + x.PackageType).ToList();
            }
            return itemDataCountReportContracts;
        }

        public static List<RotationStudentsNonComplianceContract> GetRotationStudentNonComplianceStatusContract(String userID, List<Int32> tenants, List<Int32> agencies, List<String> categories, List<String> userType
            , Boolean includeUndefinedDataShares)
        {
            List<RotationStudentsNonComplianceContract> rotationStudentsNonComplianceContracts = new List<RotationStudentsNonComplianceContract>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                var contracts = DataMartUtils.GetSharedItemServiceInstance().GetSharedDataForNonCompliance(invitationGroups, tenants, agencies, categories, userType, includeUndefinedDataShares)
                    .Where(x => x.InvitationReviewStatusName == "Approved" && x.CategoryComplianceStatus != "Approved" && x.OutOfComplianceDate.IsNotNull())
                    .Select(x => new
                    {
                        x.AgencyName,
                        x.ComplioID,
                        x.TenantName,
                        x.FirstName,
                        x.LastName,
                        x.StudentEmail,
                        x.OutOfComplianceDate,
                        x.InvitationReviewStatusName,
                        x.ReviewedBy,
                        x.CategoryComplianceStatus,
                        x.CategoryName,
                        x.RotationEndDate,
                        x.SharedBy,
                        x.SharedByEmail,
                        x.UserType              //UAT-4901
                    }).ToList();
                rotationStudentsNonComplianceContracts = contracts.GroupBy(x => new
                {
                    x.AgencyName,
                    x.ComplioID,
                    x.TenantName,
                    x.FirstName,
                    x.LastName,
                    x.StudentEmail,
                    x.InvitationReviewStatusName,
                    x.ReviewedBy,
                    x.CategoryComplianceStatus,
                    x.RotationEndDate,
                    x.SharedBy,
                    x.SharedByEmail,
                    x.UserType                //UAT-4901
                }).Select(x => new RotationStudentsNonComplianceContract
                {
                    NonComplianceRequirements = String.Join(",", x.Select(y => y.CategoryName).Distinct()),
                    Agency = x.Key.AgencyName,
                    ComplioID = x.Key.ComplioID,
                    OutOfComplianceDate = x.Max(z=>z.OutOfComplianceDate),
                    ReviewedBy = x.Key.ReviewedBy,
                    ReviewStatus = x.Key.InvitationReviewStatusName,
                    RotationEndDate = x.Key.RotationEndDate,
                    SharedBy = x.Key.SharedBy,
                    SharedByEmail = x.Key.SharedByEmail,
                    StudentEmail = x.Key.StudentEmail,
                    StudentName = x.Key.FirstName + " " + x.Key.LastName,
                    TenantName = x.Key.TenantName,
                    UserType=x.Key.UserType       //UAT-4901
                    
                }).OrderBy(x => x.TenantName + x.Agency + x.StudentName).ToList();
            }




            return rotationStudentsNonComplianceContracts;
        }

        public static List<RotationStudentDetailsContract> GetRotationStudentDetailsContracts(String userID, List<Int32> tenants, List<Int32> agencies, List<String> reviewStatus, List<String> userType
           , DateTime? fromDate, DateTime? toDate, Boolean includeUndefinedDataShares)
        {
            List<RotationStudentDetailsContract> rotationStudentDetailsContracts = new List<RotationStudentDetailsContract>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                rotationStudentDetailsContracts = DataMartUtils.GetRotationDetailServiceInstance().Get().Find(x => invitationGroups.Contains(x.InvitationGroupID) &&
                                                tenants.Contains(x.TenantID) && agencies.Contains(x.AgencyID)
                                                && userType.Contains(x.UserType) && (includeUndefinedDataShares || (x.RotationStartDate != null && x.RotationEndDate != null))
                                                && ((fromDate == null && toDate == null) || ((fromDate != null && toDate != null && fromDate <= toDate && x.RotationStartDate != null && x.RotationEndDate != null)
                                                && ((x.RotationStartDate >= fromDate && x.RotationStartDate <= toDate) || (x.RotationEndDate >= fromDate && x.RotationEndDate <= toDate)
                                                || (fromDate >= x.RotationStartDate && fromDate <= x.RotationEndDate) || (toDate >= x.RotationStartDate && toDate <= x.RotationEndDate)
                                                ))))
                                                .ToList()
                                                .GroupBy(x => new { x.TenantID, x.StudentID, x.RotationID }).Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault())
                                                .Where(x => reviewStatus.Contains(x.ReviewCode))
                                              .Select(x => new RotationStudentDetailsContract
                                              {

                                                  AgencyID = x.AgencyID,
                                                  AgencyName = x.AgencyName,
                                                  ComplioID = x.ComplioID,
                                                  Course = x.Course,
                                                  CustomAttributes = x.CustomAttributes,
                                                  Days = x.Days,
                                                  Department = x.Department,
                                                  ID = x.ID,
                                                  InvitationGroupID = x.InvitationGroupID,
                                                  InvitationReviewStatusName = x.InvitationReviewStatusName,
                                                  InvitationSourceCode = x.InvitationSourceCode,
                                                  InvitationSourceType = x.InvitationSourceType,
                                                  Program = x.InvitationSourceType,
                                                  ReviewCode = x.ReviewCode,
                                                  ReviewedBy = x.ReviewedBy,
                                                  RotationEndDate = x.RotationEndDate,
                                                  RotationID = String.IsNullOrEmpty(x.RotationID) ? x.RotationID : x.RotationName,
                                                  RotationName = x.RotationName,
                                                  RotationShift = x.RotationShift,
                                                  RotationStartDate = x.RotationStartDate,
                                                  SharedBy = x.SharedBy,
                                                  SharedByEmail = x.SharedByEmail,
                                                  StudentEmailAddress = x.StudentEmailAddress,
                                                  StudentFirstName = x.StudentFirstName,
                                                  StudentID = x.StudentID,
                                                  StudentLastName = x.StudentLastName,
                                                  TenantID = x.TenantID,
                                                  TenantName = x.TenantName,
                                                  Term = x.Term,
                                                  Times = x.Times,
                                                  TypeSpecialty = x.TypeSpecialty,
                                                  UnitFloorLoc = x.UnitFloorLoc,
                                                  UserType = x.UserType,
                                                  Address = x.StudentAddress,
                                                  StudentDOB = x.StudentDOB,
                                                  StudentPhoneNumber = x.StudentPhoneNumber
                                              }).OrderBy(x => x.TenantName + x.AgencyName + x.StudentFirstName).ToList();

            }
            return rotationStudentDetailsContracts;
        }

        public static Dictionary<String, Int32> GetProfileCountContracts(String userID, List<Int32> tenants, List<Int32> agencies, List<String> userType
           , DateTime? fromDate, DateTime? toDate, Boolean includeUndefinedDataShares, Boolean isUniqueResultsOnly)
        {
            Dictionary<String, Int32> profileCountContracts = new Dictionary<String, Int32>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();              
              List<RotationDetail> rotationDetails =   DataMartUtils.GetRotationDetailServiceInstance().Get().Find(x => invitationGroups.Contains(x.InvitationGroupID) &&
                                                tenants.Contains(x.TenantID) && agencies.Contains(x.AgencyID) && x.InvitationReviewStatusName == "Approved"
                                                && userType.Contains(x.UserType) && (includeUndefinedDataShares || (x.RotationStartDate != null && x.RotationEndDate != null))
                                                && ((fromDate == null && toDate == null) || ((fromDate != null && toDate != null && fromDate <= toDate && x.RotationStartDate != null && x.RotationEndDate != null)
                                                && ((x.RotationStartDate >= fromDate && x.RotationStartDate <= toDate) || (x.RotationEndDate >= fromDate && x.RotationEndDate <= toDate)
                                                || (fromDate >= x.RotationStartDate && fromDate <= x.RotationEndDate) || (toDate >= x.RotationStartDate && toDate <= x.RotationEndDate)
                                                )))).ToList();
                if (!rotationDetails.IsNullOrEmpty() && rotationDetails.Count > 0)
                {
                    if (isUniqueResultsOnly)
                    {
                        profileCountContracts = rotationDetails.GroupBy(x => new { x.TenantID, x.StudentID })
                                                                .Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault())
                                                                .GroupBy(x => x.TenantName).OrderBy(x => x.Key)
                                                                .ToDictionary(x => x.Key, y => y.Count());
                    }
                    else
                    {
                        profileCountContracts =  rotationDetails.GroupBy(x => new { x.TenantID, x.StudentID, x.RotationID })
                                                                .Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault())
                                                                .GroupBy(x => x.TenantName).OrderBy(x => x.Key)
                                                                .ToDictionary(x => x.Key, y => y.Count());
                    }
                }
            }
            return profileCountContracts;
        }

        public static List<RotationStudentsByDayContract> GetRotationStudentsByDayContracts(String userID, List<Int32> tenants, List<Int32> agencies, List<String> reviewStatus, List<String> userType
          , DateTime? fromDate, DateTime? toDate, List<String> days)
        {
            List<RotationStudentsByDayContract> rotationStudentByDayContracts = new List<RotationStudentsByDayContract>();
            DataMart.Services.AgencyUserService agencyUserService = DataMartUtils.GetAgencyUserServiceInstance();
            DataMart.Models.AgencyUser agencyUser = agencyUserService.Get(userID);
            if (agencyUser.IsNotNull())
            {
                List<Int32> invitationGroups = agencyUser.SharingInvitations.IsNotNull() ?
                                                agencyUser.SharingInvitations.Select(x => x.InvitationGroupID).Distinct().ToList() :
                                                new List<Int32>();
                rotationStudentByDayContracts = DataMartUtils.GetRotationDetailServiceInstance().Get().Find(x => invitationGroups.Contains(x.InvitationGroupID) &&
                                                tenants.Contains(x.TenantID) && agencies.Contains(x.AgencyID)
                                                && userType.Contains(x.UserType)
                                                && ((fromDate == null && toDate == null) || ((fromDate != null && toDate != null && fromDate <= toDate && x.RotationStartDate != null && x.RotationEndDate != null)
                                                && ((x.RotationStartDate >= fromDate && x.RotationStartDate <= toDate) || (x.RotationEndDate >= fromDate && x.RotationEndDate <= toDate)
                                                || (fromDate >= x.RotationStartDate && fromDate <= x.RotationEndDate) || (toDate >= x.RotationStartDate && toDate <= x.RotationEndDate)
                                                )))).ToList()
                                                .GroupBy(x => new { x.TenantID, x.StudentID, x.RotationID }).Select(x => x.OrderBy(z => z.ReviewedDate).LastOrDefault())
                                                .Where(x => reviewStatus.Contains(x.ReviewCode) && (String.IsNullOrEmpty(x.Days) || days.Any(day => x.Days.Contains(day))))
                                                .Select(x => new RotationStudentsByDayContract
                                                {

                                                    AgencyName = x.AgencyName,
                                                    ComplioID = x.ComplioID,
                                                    InvitationReviewStatusName = x.InvitationReviewStatusName,
                                                    RotationID = String.IsNullOrEmpty(x.RotationID) ? x.RotationID : x.RotationName,
                                                    SharedBy = x.SharedBy,
                                                    SharedByEmail = x.SharedByEmail,
                                                    StudentEmailAddress = x.StudentEmailAddress,
                                                    StudentFirstName = x.StudentFirstName,
                                                    StudentID = x.StudentID,
                                                    StudentLastName = x.StudentLastName,
                                                    TenantName = x.TenantName,
                                                    Days = x.Days
                                                }).OrderBy(x => x.TenantName + x.AgencyName + x.StudentFirstName).ToList();

            }
            return rotationStudentByDayContracts;
        }

        /// <summary>
        /// Retrieves all Client DB Configuration
        /// </summary>
        /// <returns>
        /// </returns>
        public static List<ClientDBConfiguration> GetClientDBConfiguration()
        {
            try
            {
                return BALUtils.GetDataMartRepoInstance().GetClientDBConfigurations();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Retrieves all saved searches for a user
        /// </summary>
        /// <returns>
        /// </returns>
        public static Dictionary<String, String> GetSavedSearches(String userID, String searchType)
        {
            try
            {
                return DataMartUtils.GetSavedSearchServiceInstance().Get(userID, searchType).Select(x => new { x.ID, x.SearchName }).ToDictionary(y => y.ID, y => y.SearchName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Retrieves saved search details
        /// </summary>
        /// <returns>
        /// </returns>
        public static SavedSearch GetSavedSearchDetails(String searchID)
        {
            try
            {
                return DataMartUtils.GetSavedSearchServiceInstance().GetSearchDetails(searchID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static Boolean SearchCriteriaAlreadyExists(String userID, String searchName, String searchID, String searchType)
        {
            return DataMartUtils.GetSavedSearchServiceInstance().Get(userID, searchType).Any(x => x.SearchName == searchName && x.ID != searchID);
        }

        public static SavedSearch SaveCategoryDataSearchCriteria(String searchID, String userID, String searchName, String searchDescription,
                List<Int32> selectedTenants, List<Int32> selectedAgencies, List<String> selectedCategories, List<String> selectedItems, List<String> selectedReviewStatus,
               List<String> selectedUserType, String rotationStartDate, String rotationEndDate)
        {
            SavedSearch savedSearch = new SavedSearch();
            if (SearchCriteriaAlreadyExists(userID, searchName, searchID, SearchType.CategoryDataReport.GetStringValue()))
            {
                throw new SysXException("Duplicate Search Name. Please enter a different name for your search.");
            }
            if (searchID.IsNullOrEmpty() || searchID.Equals(AppConsts.ZERO))
            {
                savedSearch = DataMartUtils.GetSavedSearchServiceInstance().Create(savedSearch);
                searchID = savedSearch.ID;
            }
            savedSearch = DataMartUtils.GetSavedSearchServiceInstance().GetSearchDetails(searchID);
            if (savedSearch.IsNotNull())
            {
                savedSearch.UserID = userID;
                savedSearch.SearchType = SearchType.CategoryDataReport.GetStringValue();
                savedSearch.SearchName = searchName;
                savedSearch.SearchDescription = searchDescription;
                savedSearch.Institutes = selectedTenants;
                savedSearch.Agencies = selectedAgencies;
                savedSearch.Categories = selectedCategories;
                savedSearch.Items = selectedItems;
                savedSearch.ReviewStatus = selectedReviewStatus;
                savedSearch.UserTypes = selectedUserType;
                savedSearch.RotationStartDate = rotationStartDate;
                savedSearch.RotationEndDate = rotationEndDate;
                DataMartUtils.GetSavedSearchServiceInstance().Update(savedSearch.ID, savedSearch);
            }
            return savedSearch;
        }

        public static SavedSearch SaveRotationStudentsOverallNonComplianceSearchCriteria(String searchID, String userID, String searchName, String searchDescription,
        List<Int32> selectedTenants, List<Int32> selectedAgencies, List<String> selectedCategories,
       List<String> selectedUserType, Boolean includeUndefinedDataShares)
        {
            SavedSearch savedSearch = new SavedSearch();
            if (SearchCriteriaAlreadyExists(userID, searchName, searchID, SearchType.RotationStudentsOverallNonComplianceStatus.GetStringValue()))
            {
                throw new SysXException("Duplicate Search Name. Please enter a different name for your search.");
            }
            if (searchID.IsNullOrEmpty() || searchID.Equals(AppConsts.ZERO))
            {
                savedSearch = DataMartUtils.GetSavedSearchServiceInstance().Create(savedSearch);
                searchID = savedSearch.ID;
            }
            savedSearch = DataMartUtils.GetSavedSearchServiceInstance().GetSearchDetails(searchID);
            if (savedSearch.IsNotNull())
            {
                savedSearch.UserID = userID;
                savedSearch.SearchType = SearchType.RotationStudentsOverallNonComplianceStatus.GetStringValue();
                savedSearch.SearchName = searchName;
                savedSearch.SearchDescription = searchDescription;
                savedSearch.Institutes = selectedTenants;
                savedSearch.Agencies = selectedAgencies;
                savedSearch.Categories = selectedCategories;
                savedSearch.UserTypes = selectedUserType;
                savedSearch.IncludeUndefinedDataShares = includeUndefinedDataShares.ToString();
                DataMartUtils.GetSavedSearchServiceInstance().Update(savedSearch.ID, savedSearch);
            }
            return savedSearch;
        }

        public static SavedSearch SaveRotationStudentDetailsSearchCriteria(String searchID, String userID, String searchName, String searchDescription,
       List<Int32> selectedTenants, List<Int32> selectedAgencies, List<String> selectedReviewStatus,
      List<String> selectedUserType, String fromDate, String toDate, Boolean includeUndefinedDataShares)
        {
            SavedSearch savedSearch = new SavedSearch();
            if (SearchCriteriaAlreadyExists(userID, searchName, searchID, SearchType.RotationStudentDetails.GetStringValue()))
            {
                throw new SysXException("Duplicate Search Name. Please enter a different name for your search.");
            }
            if (searchID.IsNullOrEmpty() || searchID.Equals(AppConsts.ZERO))
            {
                savedSearch = DataMartUtils.GetSavedSearchServiceInstance().Create(savedSearch);
                searchID = savedSearch.ID;
            }
            savedSearch = DataMartUtils.GetSavedSearchServiceInstance().GetSearchDetails(searchID);
            if (savedSearch.IsNotNull())
            {
                savedSearch.UserID = userID;
                savedSearch.SearchType = SearchType.RotationStudentDetails.GetStringValue();
                savedSearch.SearchName = searchName;
                savedSearch.SearchDescription = searchDescription;
                savedSearch.Institutes = selectedTenants;
                savedSearch.Agencies = selectedAgencies;
                savedSearch.ReviewStatus = selectedReviewStatus;
                savedSearch.UserTypes = selectedUserType;
                savedSearch.RotationStartDate = fromDate;
                savedSearch.RotationEndDate = toDate;
                savedSearch.IncludeUndefinedDataShares = includeUndefinedDataShares.ToString();
                DataMartUtils.GetSavedSearchServiceInstance().Update(savedSearch.ID, savedSearch);
            }
            return savedSearch;
        }

        public static SavedSearch SaveProfileCountSearchCriteria(String searchID, String userID, String searchName, String searchDescription,
       List<Int32> selectedTenants, List<Int32> selectedAgencies,
      List<String> selectedUserType, String fromDate, String toDate, Boolean includeUndefinedDataShares, Boolean isUniqueResultsOnly)
        {
            SavedSearch savedSearch = new SavedSearch();
            if (SearchCriteriaAlreadyExists(userID, searchName, searchID, SearchType.ProfileCount.GetStringValue()))
            {
                throw new SysXException("Duplicate Search Name. Please enter a different name for your search.");
            }
            if (searchID.IsNullOrEmpty() || searchID.Equals(AppConsts.ZERO))
            {
                savedSearch = DataMartUtils.GetSavedSearchServiceInstance().Create(savedSearch);
                searchID = savedSearch.ID;
            }
            savedSearch = DataMartUtils.GetSavedSearchServiceInstance().GetSearchDetails(searchID);
            if (savedSearch.IsNotNull())
            {
                savedSearch.UserID = userID;
                savedSearch.SearchType = SearchType.ProfileCount.GetStringValue();
                savedSearch.SearchName = searchName;
                savedSearch.SearchDescription = searchDescription;
                savedSearch.Institutes = selectedTenants;
                savedSearch.Agencies = selectedAgencies;
                savedSearch.UserTypes = selectedUserType;
                savedSearch.RotationStartDate = fromDate;
                savedSearch.RotationEndDate = toDate;
                savedSearch.IncludeUndefinedDataShares = includeUndefinedDataShares.ToString();
                savedSearch.IsUniqueResultsOnly = isUniqueResultsOnly.ToString();
                DataMartUtils.GetSavedSearchServiceInstance().Update(savedSearch.ID, savedSearch);
            }
            return savedSearch;
        }


        public static SavedSearch SaveRotationStudentsByDaySearchCriteria(String searchID, String userID, String searchName, String searchDescription,
        List<Int32> selectedTenants, List<Int32> selectedAgencies, List<String> selectedReviewStatus,
        List<String> selectedUserType, String fromDate, String toDate, List<String> days)
        {
            SavedSearch savedSearch = new SavedSearch();
            if (SearchCriteriaAlreadyExists(userID, searchName, searchID, SearchType.RotationStudentsByDay.GetStringValue()))
            {
                throw new SysXException("Duplicate Search Name. Please enter a different name for your search.");
            }
            if (searchID.IsNullOrEmpty() || searchID.Equals(AppConsts.ZERO))
            {
                savedSearch = DataMartUtils.GetSavedSearchServiceInstance().Create(savedSearch);
                searchID = savedSearch.ID;
            }
            savedSearch = DataMartUtils.GetSavedSearchServiceInstance().GetSearchDetails(searchID);
            if (savedSearch.IsNotNull())
            {
                savedSearch.UserID = userID;
                savedSearch.SearchType = SearchType.RotationStudentsByDay.GetStringValue();
                savedSearch.SearchName = searchName;
                savedSearch.SearchDescription = searchDescription;
                savedSearch.Institutes = selectedTenants;
                savedSearch.Agencies = selectedAgencies;
                savedSearch.ReviewStatus = selectedReviewStatus;
                savedSearch.UserTypes = selectedUserType;
                savedSearch.RotationStartDate = fromDate;
                savedSearch.RotationEndDate = toDate;
                savedSearch.Days = days;
                DataMartUtils.GetSavedSearchServiceInstance().Update(savedSearch.ID, savedSearch);
            }
            return savedSearch;
        }
        public static SavedSearch SaveItemDataCountSearchCriteria(String searchID, String userID, String searchName, String searchDescription,
                List<Int32> selectedTenants, List<Int32> selectedAgencies, List<String> selectedCategories, List<String> selectedItems,
               List<String> selectedUserType, String fromDate, String toDate, Boolean isUniqueResultsOnly)
        {
            SavedSearch savedSearch = new SavedSearch();
            if (SearchCriteriaAlreadyExists(userID, searchName, searchID, SearchType.ItemDataCountReport.GetStringValue()))
            {
                throw new SysXException("Duplicate Search Name. Please enter a different name for your search.");
            }
            if (searchID.IsNullOrEmpty() || searchID.Equals(AppConsts.ZERO))
            {
                savedSearch = DataMartUtils.GetSavedSearchServiceInstance().Create(savedSearch);
                searchID = savedSearch.ID;
            }
            savedSearch = DataMartUtils.GetSavedSearchServiceInstance().GetSearchDetails(searchID);
            if (savedSearch.IsNotNull())
            {
                savedSearch.UserID = userID;
                savedSearch.SearchType = SearchType.ItemDataCountReport.GetStringValue();
                savedSearch.SearchName = searchName;
                savedSearch.SearchDescription = searchDescription;
                savedSearch.Institutes = selectedTenants;
                savedSearch.Agencies = selectedAgencies;
                savedSearch.Categories = selectedCategories;
                savedSearch.Items = selectedItems;
                savedSearch.UserTypes = selectedUserType;
                savedSearch.RotationStartDate = fromDate;
                savedSearch.RotationEndDate = toDate;
                savedSearch.IsUniqueResultsOnly = isUniqueResultsOnly.ToString();
                DataMartUtils.GetSavedSearchServiceInstance().Update(savedSearch.ID, savedSearch);
            }
            return savedSearch;
        }

        public static SavedSearch SaveCategoryDataWithComplioIDSearchCriteria(String searchID, String userID, String searchName, String searchDescription,
         List<Int32> selectedTenants, List<Int32> selectedAgencies, List<String> selectedCategories, List<String> selectedItems, List<String> selectedReviewStatus,
        List<String> selectedUserType, String rotationStartDate, String rotationEndDate, String complioID, String customAttribute)
        {
            SavedSearch savedSearch = new SavedSearch();
            if (SearchCriteriaAlreadyExists(userID, searchName, searchID, SearchType.CategoryDataReportByComplioID.GetStringValue()))
            {
                throw new SysXException("Duplicate Search Name. Please enter a different name for your search.");
            }
            if (searchID.IsNullOrEmpty() || searchID.Equals(AppConsts.ZERO))
            {
                savedSearch = DataMartUtils.GetSavedSearchServiceInstance().Create(savedSearch);
                searchID = savedSearch.ID;
            }
            savedSearch = DataMartUtils.GetSavedSearchServiceInstance().GetSearchDetails(searchID);
            if (savedSearch.IsNotNull())
            {
                savedSearch.UserID = userID;
                savedSearch.SearchType = SearchType.CategoryDataReportByComplioID.GetStringValue();
                savedSearch.SearchName = searchName;
                savedSearch.SearchDescription = searchDescription;
                savedSearch.Institutes = selectedTenants;
                savedSearch.Agencies = selectedAgencies;
                savedSearch.Categories = selectedCategories;
                savedSearch.Items = selectedItems;
                savedSearch.ReviewStatus = selectedReviewStatus;
                savedSearch.UserTypes = selectedUserType;
                savedSearch.ComplioID = complioID;
                savedSearch.CustomAttribute = customAttribute;
                savedSearch.RotationStartDate = rotationStartDate;
                savedSearch.RotationEndDate = rotationEndDate;
                DataMartUtils.GetSavedSearchServiceInstance().Update(savedSearch.ID, savedSearch);
            }
            return savedSearch;
        }

        #endregion

        #endregion

        #region Private Methods


        #endregion

        #endregion

    }
}