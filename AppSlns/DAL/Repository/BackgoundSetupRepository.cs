using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.ComplianceOperation;
//using Entity;
using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace DAL.Repository
{
    public class BackgoundSetupRepository : ClientBaseRepository, IBackgroundSetupRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public BackgoundSetupRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #region Package Setup
        public List<BackgroundPackage> GetPackageData()
        {
            return _ClientDBContext.BackgroundPackages.Where(x => !x.BPA_IsDeleted).ToList();
        }

        public List<BkgSvcGroup> GetServiceGroupGridData(Int32 packageId)
        {
            return _ClientDBContext.BkgPackageSvcGroups.Where(x => x.BPSG_BackgroundPackageID == packageId && !x.BPSG_IsDeleted && x.BkgSvcGroup.BSG_Active && !x.BkgSvcGroup.BSG_IsDeleted).Select(x => x.BkgSvcGroup).ToList();
        }


        #region Map Service with service group

        public List<BackgroundService> GetServicesForGridData(Int32 serviceGroupId, Int32 packageId)
        {
            BkgPackageSvcGroup bkgPackageSvcGroup = _ClientDBContext.BkgPackageSvcGroups.FirstOrDefault(x => x.BPSG_BackgroundPackageID == packageId && x.BPSG_BkgSvcGroupID == serviceGroupId);
            Int32 pkgSvcGroupId = bkgPackageSvcGroup.BPSG_ID;
            return _ClientDBContext.BkgPackageSvcs.Where(x => x.BPS_BkgPackageSvcGroupID == pkgSvcGroupId && !x.BPS_IsDeleted && !x.BackgroundService.BSE_IsDeleted).Select(y => y.BackgroundService).ToList();
        }

        public BkgPackageSvc GetCurrentBkgPkgService(Int32 serviceGroupId, Int32 packageId, Int32 serviceId)
        {
            BkgPackageSvcGroup bkgPackageSvcGroup = _ClientDBContext.BkgPackageSvcGroups.FirstOrDefault(x => x.BPSG_BackgroundPackageID == packageId && x.BPSG_BkgSvcGroupID == serviceGroupId);
            Int32 pkgSvcGroupId = bkgPackageSvcGroup.BPSG_ID;
            return _ClientDBContext.BkgPackageSvcs.Where(x => x.BPS_BkgPackageSvcGroupID == pkgSvcGroupId && !x.BPS_IsDeleted && !x.BackgroundService.BSE_IsDeleted && x.BPS_BackgroundServiceID == serviceId).FirstOrDefault();
        }

        public List<BackgroundService> GetServicesForDropDown(List<Int32> lstSvcToBeRemoved)
        {
            return _ClientDBContext.BackgroundServices.Where(x => !lstSvcToBeRemoved.Contains(x.BSE_ID) && !x.BSE_IsDeleted && x.BSE_ParentServiceID == null).Select(y => y).ToList();
        }

        public List<BkgSvcGroup> GetServiceGroupForDropDown(List<Int32> lstSvcToBeRemoved)
        {
            return _ClientDBContext.BkgSvcGroups.Where(x => !lstSvcToBeRemoved.Contains(x.BSG_ID) && x.BSG_Active && !x.BSG_IsDeleted).Select(y => y).ToList();
        }

        public BkgPackageSvcGroup GetServicesGroupForEdit(Int32 serviceGroupId)
        {
            return _ClientDBContext.BkgPackageSvcGroups.FirstOrDefault(x => x.BPSG_ID == serviceGroupId);
        }

        public String MapServiceWithServiceGroup(Int32 serviceId, Int32 serviceGroupId, Int32 packageId, Boolean isDelete, String displayName, String notes, Int32? pkgCount, Int32? minOccurrences, Int32? maxOccurrences, Int32? residentialDuration,
                                                 Boolean sendDocsToStudent, Boolean isSupplemental, Boolean ignoreRHOnSupplement, Boolean isReportable, String bkgPkgSvcOverrideData = "")
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_InsertRecordsForServiceGroupMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@serviceId", serviceId);
                command.Parameters.AddWithValue("@serviceGroupId", serviceGroupId);
                command.Parameters.AddWithValue("@packageId", packageId);
                command.Parameters.AddWithValue("@IsDelete", isDelete);
                command.Parameters.AddWithValue("@displayName", displayName);
                command.Parameters.AddWithValue("@notes", notes);
                command.Parameters.AddWithValue("@pkgCount", pkgCount);
                command.Parameters.AddWithValue("@minOccurrences", minOccurrences);
                command.Parameters.AddWithValue("@maxOccurrences", maxOccurrences);
                command.Parameters.AddWithValue("@residentialDuration", residentialDuration);
                command.Parameters.AddWithValue("@sendDocsToStudent", sendDocsToStudent);
                command.Parameters.AddWithValue("@isSupplemental", isSupplemental);
                command.Parameters.AddWithValue("@ignoreRHOnSupplement", ignoreRHOnSupplement);
                command.Parameters.AddWithValue("@isReportable", isReportable);
                if (!isDelete && !bkgPkgSvcOverrideData.IsNullOrEmpty())
                    command.Parameters.AddWithValue("@bkgPkgSvcOverrideData", bkgPkgSvcOverrideData);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return String.Empty;
            }
        }
        public List<Int32> GetServicesByPackageId(Int32 packageId)
        {
            List<Int32> listServiceIdToremove = new List<Int32>();
            List<BkgPackageSvcGroup> bkgPackageSvcGroup = _ClientDBContext.BkgPackageSvcGroups.Where(x => x.BPSG_BackgroundPackageID == packageId && x.BPSG_IsDeleted == false).ToList();
            if (!bkgPackageSvcGroup.IsNullOrEmpty())
            {
                List<Int32> bkgPackageSvcGroupIdList = bkgPackageSvcGroup.Select(cnd => cnd.BPSG_ID).ToList();
                List<BkgPackageSvc> bkgPackageSvcList = _ClientDBContext.BkgPackageSvcs.Where(x => bkgPackageSvcGroupIdList.Contains(x.BPS_BkgPackageSvcGroupID) && !x.BPS_IsDeleted && !x.BackgroundService.BSE_IsDeleted).ToList();
                if (!bkgPackageSvcList.IsNullOrEmpty())
                    listServiceIdToremove = bkgPackageSvcList.Select(slct => slct.BPS_BackgroundServiceID).ToList();
            }
            return listServiceIdToremove;
        }

        #endregion

        #region Manage Package Set screen

        public BackgroundPackage GetPackageDetail(Int32 packageId)
        {
            return _ClientDBContext.BackgroundPackages.FirstOrDefault(x => x.BPA_ID == packageId);
        }

        public String SaveEditPackagedetail(BackgroundPackage backgroundPackage, Int32 packageId, Boolean isEdit, Int32 currentLoggedInUserID, List<Int32> targetPackageIds, Int32 months, Boolean isActive)
        {
            if (backgroundPackage.IsNotNull())
            {
                Boolean isNewPackage = false;
                if (_ClientDBContext.BackgroundPackages.Any(x => x.BPA_Name.Equals(backgroundPackage.BPA_Name) && !x.BPA_IsDeleted) && !isEdit)
                {
                    return "Package already exists.";
                }
                else if (isEdit)
                {
                    BackgroundPackage bckGroundPckg = _ClientDBContext.BackgroundPackages.FirstOrDefault(x => x.BPA_ID == packageId);
                    if (bckGroundPckg.IsNullOrEmpty())
                    {
                        return "Package Not Updated successfully";
                    }
                    bckGroundPckg.BPA_Name = backgroundPackage.BPA_Name;
                    bckGroundPckg.BPA_IsActive = backgroundPackage.BPA_IsActive;
                    bckGroundPckg.BPA_IsViewDetailsInOrderEnabled = backgroundPackage.BPA_IsViewDetailsInOrderEnabled;
                    bckGroundPckg.BPA_Description = backgroundPackage.BPA_Description;
                    bckGroundPckg.BPA_ModifiedBy = 1;
                    bckGroundPckg.BPA_IsDeleted = false;
                    bckGroundPckg.BPA_ModifiedDate = DateTime.Now;
                    bckGroundPckg.BPA_Label = backgroundPackage.BPA_Label;
                    //UAT-947:WB: Add ability to show custom details below each package name on package selection screen
                    bckGroundPckg.BPA_PackageDetail = backgroundPackage.BPA_PackageDetail;
                    bckGroundPckg.BPA_NotesDisplayPositionId = backgroundPackage.BPA_NotesDisplayPositionId;
                    //UAT-2194: Invite only packages
                    bckGroundPckg.BPA_IsInviteOnlyPackage = backgroundPackage.BPA_IsInviteOnlyPackage;
                    //UAT-2842
                    bckGroundPckg.BPA_IsAvailableForApplicantOrder = backgroundPackage.BPA_IsAvailableForApplicantOrder;
                    bckGroundPckg.BPA_IsAvailableForAdminOrder = backgroundPackage.BPA_IsAvailableForAdminOrder;
                    //UAT-3268
                    bckGroundPckg.BPA_IsReqToQualifyInRotation = backgroundPackage.BPA_IsReqToQualifyInRotation;

                    bckGroundPckg.BPA_BkgPackageTypeId = backgroundPackage.BPA_BkgPackageTypeId; //UAT-3525
                    bckGroundPckg.BPA_Passcode = backgroundPackage.BPA_Passcode; //UAT-3771
                    #region UAT-2388 : On Package Setup screen
                    // if (months > AppConsts.NONE && targetPackageIds.Count > AppConsts.NONE)
                    // {
                    var bckAutomaticInvitationPkg = _ClientDBContext.PackageInvitationSettings.FirstOrDefault(d => d.PIS_BkgPkgID == bckGroundPckg.BPA_ID && !d.PIS_IsDeleted);
                    if (bckAutomaticInvitationPkg.IsNullOrEmpty())
                    {
                        //insert
                        var PackageInvitationSetting = new PackageInvitationSetting();
                        PackageInvitationSetting.PIS_BkgPkgID = bckGroundPckg.BPA_ID;
                        PackageInvitationSetting.PIS_CreatedByID = currentLoggedInUserID;
                        PackageInvitationSetting.PIS_CreatedOn = DateTime.Now;
                        PackageInvitationSetting.PIS_IsDeleted = false;
                        PackageInvitationSetting.PIS_Months = months;

                        PackageInvitationSetting.PIS_IsActive = isActive;
                        foreach (var item in targetPackageIds)
                        {
                            var PackageInvitationSettingPackage = new PackageInvitationSettingPackage();
                            PackageInvitationSettingPackage.PISP_CreatedByID = currentLoggedInUserID;
                            PackageInvitationSettingPackage.PISP_CreatedOn = DateTime.Now;
                            PackageInvitationSettingPackage.PISP_IsDeleted = false;
                            PackageInvitationSettingPackage.PISP_TargetBkgPkgID = item;
                            PackageInvitationSetting.PackageInvitationSettingPackages.Add(PackageInvitationSettingPackage);
                        }
                        _ClientDBContext.PackageInvitationSettings.AddObject(PackageInvitationSetting);
                    }
                    else
                    {
                        //update
                        bckAutomaticInvitationPkg.PIS_ModifiedByID = currentLoggedInUserID;
                        bckAutomaticInvitationPkg.PIS_ModifiedOn = DateTime.Now;
                        bckAutomaticInvitationPkg.PIS_IsActive = isActive;
                        if (isActive)
                        {
                            bckAutomaticInvitationPkg.PIS_Months = months;

                            var oldMappingList = bckAutomaticInvitationPkg.PackageInvitationSettingPackages.Where(cond => !cond.PISP_IsDeleted).ToList();
                            //get those mapping list which need to remove
                            var deleteMappingList = oldMappingList.Where(cond => !targetPackageIds.Contains(cond.PISP_TargetBkgPkgID)).ToList();
                            //Get existed mapping list
                            List<Int32> existMapping = oldMappingList.Where(cond => targetPackageIds.Contains(cond.PISP_TargetBkgPkgID)).Select(sel => sel.PISP_TargetBkgPkgID).ToList();
                            //delete old mapping
                            deleteMappingList.ForEach(s => s.PISP_IsDeleted = true);
                            //Add new mapping which are not existed
                            foreach (var item in targetPackageIds)
                            {
                                if (!existMapping.Contains(item))
                                {
                                    var PackageInvitationSettingPackage = new PackageInvitationSettingPackage();
                                    PackageInvitationSettingPackage.PISP_CreatedByID = AppConsts.ONE;
                                    PackageInvitationSettingPackage.PISP_CreatedOn = DateTime.Now;
                                    PackageInvitationSettingPackage.PISP_IsDeleted = false;
                                    PackageInvitationSettingPackage.PISP_TargetBkgPkgID = item;
                                    bckAutomaticInvitationPkg.PackageInvitationSettingPackages.Add(PackageInvitationSettingPackage);
                                }
                            }
                        }
                    }
                    // }

                    #endregion
                }
                else
                {
                    //Set Sequence of new Package
                    isNewPackage = true;
                    BackgroundPackage lastBackgroundPackage = _ClientDBContext.BackgroundPackages.Where(x => !x.BPA_IsDeleted).OrderByDescending(x => x.BPA_DisplayOrder).FirstOrDefault();
                    Int32? displayOrder = (lastBackgroundPackage.IsNotNull()) ? lastBackgroundPackage.BPA_DisplayOrder + 1 : 1;
                    backgroundPackage.BPA_DisplayOrder = displayOrder;
                    _ClientDBContext.BackgroundPackages.AddObject(backgroundPackage);
                }
                Int32 isPackageSaved = _ClientDBContext.SaveChanges();
                if (isNewPackage && isPackageSaved > 0)
                {
                    CopyMasterStateSearchSettings(backgroundPackage.BPA_ID);
                }
                return String.Empty;
            }
            return "Package Not Saved successfully";
        }

        /// <summary>
        /// Method to Copy Master State Search settings for background package.
        /// </summary>
        /// <param name="currentBkgpackageID"></param>
        /// <returns></returns>
        private void CopyMasterStateSearchSettings(Int32 currentBkgpackageID)
        {
            #region UAT-803
            List<Entity.BkgMasterStateSearch> lstBkgMasterStateSearch = SecurityContext.BkgMasterStateSearches.Where(x => !x.BMSS_IsDeleted).ToList();
            List<Entity.State> lstStates = GetAllStates().Where(x => x.CountryID == AppConsts.COUNTRY_USA_ID && x.StateID > 0).ToList();

            //None entry exists in master state search.
            if (lstBkgMasterStateSearch.IsNull())
            {
                DateTime createdOn = DateTime.Now;
                foreach (Entity.State item in lstStates)
                {
                    BkgPkgStateSearch bkgPkgStateSearch = new BkgPkgStateSearch();
                    bkgPkgStateSearch.BPSS_BPAID = currentBkgpackageID;
                    bkgPkgStateSearch.BPSS_StateID = item.StateID;
                    bkgPkgStateSearch.BPSS_CreatedBy = 1; // TO BE UPDATE
                    bkgPkgStateSearch.BPSS_CreatedOn = createdOn;
                    bkgPkgStateSearch.BPSS_IsStateSearch = false;
                    bkgPkgStateSearch.BPSS_IsCountySearch = false;
                    _ClientDBContext.BkgPkgStateSearches.AddObject(bkgPkgStateSearch);
                }
            }

            //Entry exists in master for each state
            else if (lstBkgMasterStateSearch.IsNotNull() && lstStates.IsNotNull() && lstBkgMasterStateSearch.Count == lstStates.Count)
            {
                DateTime createdOn = DateTime.Now;
                foreach (Entity.BkgMasterStateSearch item in lstBkgMasterStateSearch)
                {
                    BkgPkgStateSearch bkgPkgStateSearch = new BkgPkgStateSearch();
                    bkgPkgStateSearch.BPSS_BPAID = currentBkgpackageID;
                    bkgPkgStateSearch.BPSS_StateID = item.BMSS_StateID;
                    bkgPkgStateSearch.BPSS_CreatedBy = 1; // TO BE UPDATE
                    bkgPkgStateSearch.BPSS_CreatedOn = createdOn;
                    bkgPkgStateSearch.BPSS_IsStateSearch = item.BMSS_IsStateSearch;
                    bkgPkgStateSearch.BPSS_IsCountySearch = item.BMSS_IsCountySearch;
                    _ClientDBContext.BkgPkgStateSearches.AddObject(bkgPkgStateSearch);
                }
            }

            //Entry exists in master for some states
            List<Entity.State> lstStatesObj = lstStates.Where(x => !lstBkgMasterStateSearch.Select(cond => cond.BMSS_StateID).Contains(x.StateID)).ToList();
            if (lstStatesObj.IsNotNull() && lstStatesObj.Count > 0)
            {
                DateTime createdOn = DateTime.Now;
                foreach (Entity.State item in lstStatesObj)
                {
                    BkgPkgStateSearch bkgPkgStateSearch = new BkgPkgStateSearch();
                    bkgPkgStateSearch.BPSS_BPAID = currentBkgpackageID;
                    bkgPkgStateSearch.BPSS_StateID = item.StateID;
                    bkgPkgStateSearch.BPSS_CreatedBy = 1; // TO BE UPDATE
                    bkgPkgStateSearch.BPSS_CreatedOn = createdOn;
                    bkgPkgStateSearch.BPSS_IsStateSearch = false;
                    bkgPkgStateSearch.BPSS_IsCountySearch = false;
                    _ClientDBContext.BkgPkgStateSearches.AddObject(bkgPkgStateSearch);
                }
            }
            _ClientDBContext.SaveChanges();
            #endregion
        }

        public Boolean DeleteServiceGroupMapping(Int32 serviceGroupId, Int32 packageId)
        {
            BkgPackageSvcGroup bkgPackageSvcGroup = _ClientDBContext.BkgPackageSvcGroups.FirstOrDefault(x => x.BPSG_BackgroundPackageID == packageId && x.BPSG_BkgSvcGroupID == serviceGroupId && !x.BPSG_IsDeleted);
            bkgPackageSvcGroup.BPSG_IsDeleted = true;
            _ClientDBContext.SaveChanges();
            return true;

        }

        public Boolean DeletePackageMapping(Int32 packageId)
        {
            BackgroundPackage backgroundPackage = _ClientDBContext.BackgroundPackages.FirstOrDefault(x => x.BPA_ID == packageId);
            backgroundPackage.BPA_IsDeleted = true;

            //int backgroundPackageDisplayOrder = backgroundPackage.BPA_DisplayOrder ?? 0;
            //backgroundPackage.BPA_DisplayOrder = AppConsts.NONE;
            //int currentUserId = AppConsts.ONE;
            //////Re-Order Sequence of the Records on deletion.
            //List<BackgroundPackage> lstBackgroundPackage = _ClientDBContext.BackgroundPackages.Where(obj => obj.BPA_IsDeleted == false &&
            //     obj.BPA_DisplayOrder > backgroundPackageDisplayOrder).OrderBy(obj => obj.BPA_DisplayOrder).ToList();
            ////Cal the Method/SP to update the Sequence
            //UpdateBackgroundPackageSequence(lstBackgroundPackage, backgroundPackageDisplayOrder, currentUserId);
            _ClientDBContext.SaveChanges();
            return true;
        }


        /// <summary>
        /// Get LocalAttributeGroupMappedToBkgPackage record  Based On Package ID
        /// </summary>
        /// <param name="bkgPackageID">bkgPackageID</param>
        /// <returns></returns>
        public List<LocalAttributeGroupMappedToBkgPackage> GetAttributeGroupMappedToBkgPackage(Int32 bkgPackageID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.GetMappedAttributeGroupsWithPkg", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BackgroundPackageId", bkgPackageID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<LocalAttributeGroupMappedToBkgPackage> lstLocalAttributeGroupMappedToBkgPackage = new List<LocalAttributeGroupMappedToBkgPackage>();

                lstLocalAttributeGroupMappedToBkgPackage = ds.Tables[0].AsEnumerable().Select(col =>
                      new LocalAttributeGroupMappedToBkgPackage
                      {
                          AttributeGroupId = Convert.ToInt32(col["AttributeGroupId"]),
                          AttributeGroupName = Convert.ToString(col["AttributeGroupName"]),
                          IsSystemPreConfigured = Convert.ToBoolean(col["IsSystemPreConfigured"]),
                          IsEditable = Convert.ToBoolean(col["IsEditable"]),
                          BackgroundPackageId = Convert.ToInt32(col["BackgroundPackageId"]),
                      }).ToList();

                return lstLocalAttributeGroupMappedToBkgPackage;
            }

        }

        /// <summary>
        /// Get BkgPkgAttributeGroupInstruction by AttrGrpId and PkgId
        /// </summary>
        /// <param name="bkgPackageID">bkgPackageID</param>
        /// <param name="attrGrpId">attrGrpId</param>
        /// <returns></returns>
        public BkgPkgAttributeGroupInstruction GetBkgPkgAttributeGroupInstructionText(Int32 bkgPackageId, Int32 attrGrpId)
        {
            return _ClientDBContext.BkgPkgAttributeGroupInstructions.Where(cond => cond.BPAGI_BackgroundPackageID == bkgPackageId && cond.BPAGI_BkgSvcAttributeGroupID == attrGrpId && cond.BPAGI_IsDeleted == false).FirstOrDefault();
        }

        public Boolean SaveBkgPkgAttributeGroupInstruction(BkgPkgAttributeGroupInstruction bkgPkgAttrGrpInstructionObj)
        {
            _ClientDBContext.BkgPkgAttributeGroupInstructions.AddObject(bkgPkgAttrGrpInstructionObj);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        #endregion

        public String SaveEditServiceGroupDetail(BkgPackageSvcGroup bkgPackageSvcGroup, Int32 serviceGroupId, Boolean isEdit)
        {
            if (bkgPackageSvcGroup.IsNotNull())
            {
                _ClientDBContext.BkgPackageSvcGroups.AddObject(bkgPackageSvcGroup);
                _ClientDBContext.SaveChanges();
                return String.Empty;
            }
            return "Service Group Not Saved successfully";
        }

        #endregion

        #region Manage Attribute Group

        DataTable IBackgroundSetupRepository.GetMappedAttributeGroupList(Int32 serviceId, Int32 bkgSvcGroupId, Int32 backgroundPackageId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("GetMappedAttributeGroupsWithSvc", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BkgSvcGroupID", bkgSvcGroupId);
                command.Parameters.AddWithValue("@BackgroundServiceId", serviceId);
                command.Parameters.AddWithValue("@BackgroundPackageId", backgroundPackageId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    //queueAuditArgsContract.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                    //queueAuditArgsContract.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    return ds.Tables[0];
                }
            }
            return new DataTable();
            //return _ClientDBContext.BkgSvcAttributeGroupMappings.Where(cond => cond.BSAGM_IsDeleted == false && cond.BSAGM_ServiceId == serviceId).GroupBy(grpBy=>grpBy.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId).Select(slct=>slct.FirstOrDefault());
        }

        Boolean IBackgroundSetupRepository.UpdateChanges()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        DataTable IBackgroundSetupRepository.GetMappedAttributeList(Int32 serviceId, Int32 bkgSvcGroupId, Int32 attributeGroupId, Int32 backgroundPackageId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("GetMappedAttributesWithGroup", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BkgSvcGroupID", bkgSvcGroupId);
                command.Parameters.AddWithValue("@BackgroundServiceId", serviceId);
                command.Parameters.AddWithValue("@AttributeGroupId", attributeGroupId);
                command.Parameters.AddWithValue("@BackgroundPackageId", backgroundPackageId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
            //return _ClientDBContext.BkgSvcAttributeGroupMappings.Where(cond => cond.BSAGM_IsDeleted == false && cond.BSAGM_ServiceId == serviceId).GroupBy(grpBy=>grpBy.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId).Select(slct=>slct.FirstOrDefault());
        }

        List<AttributeDataSecurityClient> IBackgroundSetupRepository.GetAllAttribute(List<Int32> mappedSvcAttibuteIds, Int32 attributeGroupId)
        {
            //List<Int32> listBkgAttributeidTo = _ClientDBContext.BkgAttributeGroupMappings.Where(cnd => cnd.BAGM_IsDeleted == false && !mappedSvcAttibuteIds.Contains(cnd.BAGM_ID)).Select(slct => slct.BAGM_BkgSvcAtributeID).Distinct().ToList();
            //listBkgAttributeGrpMappingid = listBkgAttributeGrpMappingid.Where(cnd => !mappedSvcAttibuteGroupIds.Contains(cnd)).ToList();
            List<String> specialType = new List<String> { "Country", "State", "County", "City", "Zip Code" };
            List<String> existingType = _ClientDBContext.BkgSvcAttributes.Where(cond => cond.BSA_IsDeleted == false && mappedSvcAttibuteIds.Contains(cond.BSA_ID) && specialType.Contains(cond.lkpSvcAttributeDataType.SADT_Name)).Select(x => x.lkpSvcAttributeDataType.SADT_Name).ToList();

            List<AttributeDataSecurityClient> clientAttributes = _ClientDBContext.BkgSvcAttributes.Where(cond => cond.BSA_IsDeleted == false
                && !mappedSvcAttibuteIds.Contains(cond.BSA_ID)
                && !existingType.Contains(cond.lkpSvcAttributeDataType.SADT_Name)).Select(x => new AttributeDataSecurityClient { AttributeName = x.BSA_Name, AttributeId = x.BSA_ID }).ToList();
            List<Int32> attributeIdInClient = clientAttributes.DistinctBy(y => y.AttributeId).Select(x => x.AttributeId).ToList();
            List<AttributeDataSecurityClient> securityAttributes = base.SecurityContext.BkgAttributeGroupMappings.Where(x => x.BAGM_BkgSvcAttributeGroupId == attributeGroupId
                && !attributeIdInClient.Contains(x.BAGM_BkgSvcAtributeID)
                && !mappedSvcAttibuteIds.Contains(x.BAGM_BkgSvcAtributeID)
                && !existingType.Contains(x.BkgSvcAttribute.lkpSvcAttributeDataType.SADT_Name) && !x.BAGM_IsDeleted)
                .Select(y => new AttributeDataSecurityClient { AttributeId = y.BkgSvcAttribute.BSA_ID, AttributeName = y.BkgSvcAttribute.BSA_Name }).ToList();


            clientAttributes.AddRange(securityAttributes);
            return clientAttributes.OrderBy(x => x.AttributeName).ToList();
        }

        Boolean IBackgroundSetupRepository.DeletedBkgSvcAttributeMapping(Int32 bkgPackageSvcAttributeId, Int32 currentloggedInUserId)
        {
            BkgPackageSvcAttribute bkgPackageSvcAttributeMapping = _ClientDBContext.BkgPackageSvcAttributes.FirstOrDefault(cond => cond.BPSA_ID == bkgPackageSvcAttributeId && cond.BPSA_IsDeleted == false);
            bkgPackageSvcAttributeMapping.BPSA_IsDeleted = true;
            bkgPackageSvcAttributeMapping.BPSA_ModifiedByID = currentloggedInUserId;
            bkgPackageSvcAttributeMapping.BPSA_ModifiedOn = DateTime.Now;
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        String IBackgroundSetupRepository.IsmappingOfThisTypeAllowed(String attributeType, Int32 groupId)
        {
            List<String> specialType = new List<String> { "Country", "State", "County", "City", "Zip Code" };
            if (specialType.Contains(attributeType))
            {
                if (base.SecurityContext.BkgAttributeGroupMappings.Any(x => x.BAGM_BkgSvcAttributeGroupId == groupId && !x.BAGM_IsDeleted && x.BkgSvcAttribute.lkpSvcAttributeDataType.SADT_Name == attributeType))
                {
                    if (_ClientDBContext.BkgAttributeGroupMappings.Any(x => x.BAGM_BkgSvcAttributeGroupId == groupId && !x.BAGM_IsDeleted && x.BkgSvcAttribute.lkpSvcAttributeDataType.SADT_Name == attributeType))
                        return "Attribute of type " + attributeType + " already exists in this group. ";
                    return "Attribute of type " + attributeType + " already exists in this group at framework. ";
                }
            }
            return "";

        }


        #endregion

        #region Manage Attribute
        BkgSvcAttribute IBackgroundSetupRepository.GetBkgSvcAttribute(Int32 attributeId)
        {
            return _ClientDBContext.BkgSvcAttributes.Where(cond => cond.BSA_ID == attributeId && cond.BSA_IsDeleted == false).FirstOrDefault();
        }

        void IBackgroundSetupRepository.UpdateBkgSvcAttributeSecurity(ServiceAttributeContract serviceAttributeContract, Int32 currentLoggedInUserId)
        {
            Entity.BkgSvcAttribute serviceAttribute = serviceAttributeContract.TranslateToMasterEntity();
            Entity.BkgSvcAttribute serviceAttributeInDb = base.SecurityContext.BkgSvcAttributes.Where(cond => cond.BSA_ID == serviceAttribute.BSA_ID && cond.BSA_IsDeleted == false).FirstOrDefault();
            if (serviceAttributeInDb != null)
            {
                serviceAttributeInDb.BSA_DataTypeID = serviceAttribute.BSA_DataTypeID;
                serviceAttributeInDb.BSA_Name = serviceAttribute.BSA_Name;
                serviceAttributeInDb.BSA_Label = serviceAttribute.BSA_Label;
                serviceAttributeInDb.BSA_Description = serviceAttribute.BSA_Description;
                serviceAttributeInDb.BSA_MaxDateValue = serviceAttribute.BSA_MaxDateValue;
                serviceAttributeInDb.BSA_MaxIntValue = serviceAttribute.BSA_MaxIntValue;
                serviceAttributeInDb.BSA_MaxLength = serviceAttribute.BSA_MaxLength;
                serviceAttributeInDb.BSA_MinDateValue = serviceAttribute.BSA_MinDateValue;
                serviceAttributeInDb.BSA_MinIntValue = serviceAttribute.BSA_MinIntValue;
                serviceAttributeInDb.BSA_MinLength = serviceAttribute.BSA_MinLength;
                serviceAttributeInDb.BSA_Active = serviceAttribute.BSA_Active;
                serviceAttributeInDb.BSA_IsEditable = serviceAttribute.BSA_IsEditable;
                //serviceAttributeInDb.BSA_IsSystemPreConfiguredq = serviceAttribute.BSA_IsSystemPreConfiguredq;
                serviceAttributeInDb.BSA_IsRequired = serviceAttribute.BSA_IsRequired;
                // serviceAttributeInDb.BSA_ReqValidationMessage = serviceAttribute.BSA_ReqValidationMessage;
                //complianceAttributeInDb.CopiedFromCode = complianceAttribute.Code;
                serviceAttributeInDb.BSA_ModifiedBy = currentLoggedInUserId;
                serviceAttributeInDb.BSA_ModifiedDate = DateTime.Now;
            }
            base.SecurityContext.SaveChanges();
        }

        BkgPackageSvcAttribute IBackgroundSetupRepository.GetBkgPackageSvcAttribute(Int32 serviceAttributeId)
        {
            return _ClientDBContext.BkgPackageSvcAttributes.Where(cond => cond.BPSA_ID == serviceAttributeId && cond.BPSA_IsDeleted == false).FirstOrDefault();
        }

        ManageServiceAttributeData IBackgroundSetupRepository.GetBkgSvcAttributeData(ServiceAttributeParameter serviceAttributeParameter, Int32 tenantId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[ams].[usp_GetEditAttributeData]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@packageID", serviceAttributeParameter.PackageId);
                command.Parameters.AddWithValue("@tenantId", tenantId);
                command.Parameters.AddWithValue("@serviceGroupId", serviceAttributeParameter.ServiceGroupId);
                command.Parameters.AddWithValue("@serviceID", serviceAttributeParameter.ServiceId);
                command.Parameters.AddWithValue("@attributeGroupId", serviceAttributeParameter.AttributeGroupId);
                command.Parameters.AddWithValue("@attributeId", serviceAttributeParameter.AttributeId);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetAttributeData(ds.Tables[0]);
                }
            }
            return new ManageServiceAttributeData();
        }

        private ManageServiceAttributeData SetAttributeData(DataTable table)
        {
            ManageServiceAttributeData manageServiceAttributeData = new ManageServiceAttributeData();
            manageServiceAttributeData.Name = Convert.ToString(table.Rows[0]["Name"]);
            manageServiceAttributeData.Description = Convert.ToString(table.Rows[0]["Description"]);
            manageServiceAttributeData.Active = Convert.ToString(table.Rows[0]["Active"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(table.Rows[0]["Active"]));
            manageServiceAttributeData.Lable = Convert.ToString(table.Rows[0]["Lable"]);
            manageServiceAttributeData.OptionValues = Convert.ToString(table.Rows[0]["OptionValues"]);
            manageServiceAttributeData.MaxLength = table.Rows[0]["MaxLength"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(table.Rows[0]["MaxLength"]);
            manageServiceAttributeData.MinLength = table.Rows[0]["MinLength"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(table.Rows[0]["MinLength"]);
            manageServiceAttributeData.MaxIntValue = table.Rows[0]["MaxIntValue"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(table.Rows[0]["MaxIntValue"]);
            manageServiceAttributeData.MinIntValue = table.Rows[0]["MinIntValue"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(table.Rows[0]["MinIntValue"]);
            manageServiceAttributeData.MinDateValue = Convert.ToString(table.Rows[0]["MinDateValue"]).IsNullOrEmpty() ? "" : Convert.ToDateTime(Convert.ToString(table.Rows[0]["MinDateValue"])).ToShortDateString();
            manageServiceAttributeData.MaxDateValue = Convert.ToString(table.Rows[0]["MaxDateValue"]).IsNullOrEmpty() ? "" : Convert.ToDateTime(Convert.ToString(table.Rows[0]["MaxDateValue"])).ToShortDateString();
            manageServiceAttributeData.IsSystmComfigered = Convert.ToString(table.Rows[0]["IsSystemComfigered"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(table.Rows[0]["IsSystemComfigered"]));
            manageServiceAttributeData.AttributeDataType = Convert.ToString(table.Rows[0]["AttributeDataType"]);
            manageServiceAttributeData.ShowAllDataInEditForm = Convert.ToString(table.Rows[0]["ShowAllDataInEditForm"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(table.Rows[0]["ShowAllDataInEditForm"]));
            manageServiceAttributeData.IsDisplay = Convert.ToString(table.Rows[0]["IsDisplay"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(table.Rows[0]["IsDisplay"]));
            manageServiceAttributeData.IsHiddenFromUI = Convert.ToString(table.Rows[0]["IsHiddenFromUI"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(table.Rows[0]["IsHiddenFromUI"]));
            manageServiceAttributeData.IsRequired = Convert.ToString(table.Rows[0]["IsRequired"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(table.Rows[0]["IsRequired"]));
            manageServiceAttributeData.IsEditable = Convert.ToString(table.Rows[0]["IsEditable"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(table.Rows[0]["IsEditable"]));
            manageServiceAttributeData.ServiceIdToBeUpdated = table.Rows[0]["ServiceIdToBeUpdated"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(table.Rows[0]["ServiceIdToBeUpdated"]);
            manageServiceAttributeData.DataTypeID = table.Rows[0]["DataTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(table.Rows[0]["DataTypeID"]);
            manageServiceAttributeData.AttributeGroupCode = Convert.ToString(table.Rows[0]["AttributeGroupCode"]);
            manageServiceAttributeData.ValidationExpression = table.Rows[0]["ValidationExpression"].ToString();
            manageServiceAttributeData.ValidationMessage = table.Rows[0]["ValidationMessage"].ToString();
            return manageServiceAttributeData;

        }

        Boolean IBackgroundSetupRepository.CopyAttributeAndMappingInTenant(BkgSvcAttribute bkgSvcAttribute, Entity.BkgSvcAttribute bkgSvcAttributeMaster, Int32 attributeGroupId, Int32 bkgPackageSvcId, Int32 currentLoggedInUserId, Boolean isRequired, Boolean isDisplay,Boolean isHiddenFromUI)
        {
            Int32? maxDisplaySequence = ClientDBContext.BkgPackageSvcAttributes.Where(cond => cond.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId == attributeGroupId && cond.BkgAttributeGroupMapping.BAGM_IsDeleted == false && cond.BPSA_BkgPackageSvcID == bkgPackageSvcId && cond.BPSA_IsDeleted == false).Max(X => (Int32?)X.BkgAttributeGroupMapping.BAGM_DisplaySequence);
            if (maxDisplaySequence.IsNull())
                maxDisplaySequence = 1;
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> clientAttributeOptionList = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption>();
            bkgSvcAttribute.BSA_ID = bkgSvcAttributeMaster.BSA_ID;
            bkgSvcAttribute.BSA_CreatedDate = DateTime.Now;
            bkgSvcAttribute.BSA_CreatedById = currentLoggedInUserId;
            bkgSvcAttribute.BSA_IsDeleted = false;

            //Int32 BAGM_ID=bkgSvcAttribute.BkgAttributeGroupMappings.Where(cond=>cond.BAGM_BkgSvcAtributeID==bkgSvcAttribute.BSA_ID).FirstOrDefault().BAGM_ID;
            BkgAttributeGroupMapping bkgAttributeGroupMapping = new BkgAttributeGroupMapping();
            bkgAttributeGroupMapping.BAGM_ID = bkgSvcAttributeMaster.BkgAttributeGroupMappings.FirstOrDefault().BAGM_ID;
            bkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId = attributeGroupId;
            //bkgAttributeGroupMapping.BAGM_BkgSvcAtributeID = bkgSvcAttributeMaster.BSA_ID;
            bkgAttributeGroupMapping.BAGM_Code = Guid.NewGuid();
            bkgAttributeGroupMapping.BAGM_CopiedFromCode = null;
            bkgAttributeGroupMapping.BAGM_CreatedOn = DateTime.Now;
            bkgAttributeGroupMapping.BAGM_CreatedBy = currentLoggedInUserId;
            bkgAttributeGroupMapping.BAGM_IsDeleted = false;
            bkgAttributeGroupMapping.BAGM_IsEditable = false;
            bkgAttributeGroupMapping.BAGM_IsSystemPreConfigured = false;
            bkgAttributeGroupMapping.BAGM_IsDisplay = isDisplay;
            bkgAttributeGroupMapping.BAGM_IsHiddenFromUI = isHiddenFromUI; 
            bkgAttributeGroupMapping.BAGM_IsRequired = isRequired;
            bkgAttributeGroupMapping.BAGM_DisplaySequence = maxDisplaySequence + 1; //bkgSvcAttributeMaster.BkgAttributeGroupMappings.FirstOrDefault().BAGM_DisplaySequence;

            BkgPackageSvcAttribute bkgPackageSvcAttribute = new BkgPackageSvcAttribute();
            bkgPackageSvcAttribute.BPSA_BkgPackageSvcID = bkgPackageSvcId;
            //bkgPackageSvcAttribute.BPSA_BkgAttributeGroupMappingID = bkgAttributeGroupMapping.BAGM_ID;
            bkgPackageSvcAttribute.BPSA_CreatedOn = DateTime.Now;
            bkgPackageSvcAttribute.BPSA_CreatedByID = currentLoggedInUserId;
            bkgPackageSvcAttribute.BPSA_IsRequired = isRequired;
            bkgPackageSvcAttribute.BPSA_IsDisplay = isDisplay;
            bkgPackageSvcAttribute.BPSA_IsHiddenFromUI= isHiddenFromUI;
            bkgPackageSvcAttribute.BPSA_IsDeleted = false;
           
            bkgAttributeGroupMapping.BkgPackageSvcAttributes.Add(bkgPackageSvcAttribute);
            bkgSvcAttribute.BkgAttributeGroupMappings.Add(bkgAttributeGroupMapping);
            bkgSvcAttribute.BkgSvcAttributeOptions.Clear();
            foreach (Entity.BkgSvcAttributeOption tempMasterAttributeOption in bkgSvcAttributeMaster.BkgSvcAttributeOptions)
            {
                BkgSvcAttributeOption clientAttributeOption = new BkgSvcAttributeOption();
                // clientAttributeOption.EBSAO_BkgSvcAttributeID = tempMasterAttributeOption.EBSAO_BkgSvcAttributeID;
                clientAttributeOption.EBSAO_CreatedByID = currentLoggedInUserId;
                clientAttributeOption.EBSAO_CreatedOn = DateTime.Now;
                clientAttributeOption.EBSAO_ID = tempMasterAttributeOption.EBSAO_ID;
                clientAttributeOption.EBSAO_IsActive = tempMasterAttributeOption.EBSAO_IsActive;
                clientAttributeOption.EBSAO_IsDeleted = tempMasterAttributeOption.EBSAO_IsDeleted;
                clientAttributeOption.EBSAO_OptionText = tempMasterAttributeOption.EBSAO_OptionText;
                clientAttributeOption.EBSAO_OptionValue = tempMasterAttributeOption.EBSAO_OptionValue;
                bkgSvcAttribute.BkgSvcAttributeOptions.Add(clientAttributeOption);
                //_ClientDBContext.BkgSvcAttributeOptions.AddObject(clientAttributeOption);
            }
            _ClientDBContext.BkgSvcAttributes.AddObject(bkgSvcAttribute);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }
        BkgAttributeGroupMapping IBackgroundSetupRepository.GetAttributeMappingByAttributeGroupID(Int32 attributeGroupId, Int32 selectedAttributeId)
        {
            return _ClientDBContext.BkgAttributeGroupMappings.FirstOrDefault(cond => cond.BAGM_BkgSvcAttributeGroupId == attributeGroupId && cond.BAGM_BkgSvcAtributeID == selectedAttributeId && cond.BAGM_IsDeleted == false);
        }
        Entity.BkgAttributeGroupMapping IBackgroundSetupRepository.CheckAddForSecurityAttribute(Int32 attributeGroupId, Int32 selectedAttributeId)
        {
            if (!_ClientDBContext.BkgSvcAttributes.Any(x => x.BSA_ID == selectedAttributeId))
            {
                Entity.BkgSvcAttribute securityAttribute = base.SecurityContext.BkgSvcAttributes.FirstOrDefault(x => x.BSA_ID == selectedAttributeId);
                BkgSvcAttribute clientAttribute = new BkgSvcAttribute
                {
                    BSA_ID = securityAttribute.BSA_ID,
                    BSA_Name = securityAttribute.BSA_Name,
                    BSA_Active = true,
                    BSA_Code = securityAttribute.BSA_Code,
                    BSA_CopiedFromCode = securityAttribute.BSA_CopiedFromCode,
                    BSA_CreatedById = securityAttribute.BSA_CreatedById,
                    BSA_CreatedDate = DateTime.Now,
                    BSA_DataTypeID = securityAttribute.BSA_DataTypeID,
                    BSA_IsDeleted = false,
                    BSA_IsEditable = securityAttribute.BSA_IsEditable,
                    BSA_Description = securityAttribute.BSA_Description,
                    BSA_IsRequired = securityAttribute.BSA_IsRequired,
                    BSA_IsSystemPreConfiguredq = securityAttribute.BSA_IsSystemPreConfiguredq,
                    BSA_Label = securityAttribute.BSA_Label,
                    BSA_MaxDateValue = securityAttribute.BSA_MaxDateValue,
                    BSA_MaxIntValue = securityAttribute.BSA_MaxIntValue,
                    BSA_MaxLength = securityAttribute.BSA_MaxLength,
                    BSA_MinDateValue = securityAttribute.BSA_MinDateValue,
                    BSA_MinIntValue = securityAttribute.BSA_MinIntValue,
                    BSA_MinLength = securityAttribute.BSA_MinLength,
                    BSA_ReqValidationMessage = securityAttribute.BSA_ReqValidationMessage
                };

                _ClientDBContext.BkgSvcAttributes.AddObject(clientAttribute);
                _ClientDBContext.SaveChanges();
                return base.SecurityContext.BkgAttributeGroupMappings.FirstOrDefault(cond => cond.BAGM_BkgSvcAttributeGroupId == attributeGroupId && cond.BAGM_BkgSvcAtributeID == selectedAttributeId && cond.BAGM_IsDeleted == false);
            }
            return null;
        }



        Boolean IBackgroundSetupRepository.CopyAttributeAndGroupMappingInChild(Entity.BkgAttributeGroupMapping attributeMappingMaster, Int32 bkgPackageSvcId, Boolean isRequired, Boolean isDisplay)
        {
            BkgAttributeGroupMapping bkgAttributeGroupMappingToAdd = new BkgAttributeGroupMapping();
            bkgAttributeGroupMappingToAdd.BAGM_BkgSvcAtributeID = attributeMappingMaster.BAGM_BkgSvcAtributeID;
            bkgAttributeGroupMappingToAdd.BAGM_BkgSvcAttributeGroupId = attributeMappingMaster.BAGM_BkgSvcAttributeGroupId;
            bkgAttributeGroupMappingToAdd.BAGM_Code = attributeMappingMaster.BAGM_Code;
            bkgAttributeGroupMappingToAdd.BAGM_CopiedFromCode = attributeMappingMaster.BAGM_CopiedFromCode;
            bkgAttributeGroupMappingToAdd.BAGM_CreatedBy = attributeMappingMaster.BAGM_CreatedBy;
            bkgAttributeGroupMappingToAdd.BAGM_CreatedOn = DateTime.Now;
            bkgAttributeGroupMappingToAdd.BAGM_DisplaySequence = attributeMappingMaster.BAGM_DisplaySequence;
            bkgAttributeGroupMappingToAdd.BAGM_ID = attributeMappingMaster.BAGM_ID;
            bkgAttributeGroupMappingToAdd.BAGM_IsDeleted = attributeMappingMaster.BAGM_IsDeleted;
            bkgAttributeGroupMappingToAdd.BAGM_IsDisplay = isDisplay;
            bkgAttributeGroupMappingToAdd.BAGM_IsEditable = attributeMappingMaster.BAGM_IsEditable;
            bkgAttributeGroupMappingToAdd.BAGM_IsRequired = isRequired;
            bkgAttributeGroupMappingToAdd.BAGM_IsSystemPreConfigured = false;

            BkgPackageSvcAttribute bkgPackageSvcAttribute = new BkgPackageSvcAttribute();
            bkgPackageSvcAttribute.BPSA_BkgPackageSvcID = bkgPackageSvcId;
            bkgPackageSvcAttribute.BPSA_BkgAttributeGroupMappingID = attributeMappingMaster.BAGM_ID;
            bkgPackageSvcAttribute.BPSA_CreatedByID = attributeMappingMaster.BAGM_CreatedBy;
            bkgPackageSvcAttribute.BPSA_CreatedOn = DateTime.Now;
            bkgPackageSvcAttribute.BPSA_IsDeleted = false;
            bkgPackageSvcAttribute.BPSA_IsDisplay = isDisplay;
            bkgPackageSvcAttribute.BPSA_IsRequired = isRequired;
            bkgAttributeGroupMappingToAdd.BkgPackageSvcAttributes.Add(bkgPackageSvcAttribute);
            _ClientDBContext.BkgAttributeGroupMappings.AddObject(bkgAttributeGroupMappingToAdd);

            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        Int32 IBackgroundSetupRepository.GetBkgPackageSvcId(Int32 serviceId, Int32 bkgSvcGroupId, Int32 backgroundPackageId)
        {
            return _ClientDBContext.BkgPackageSvcs.Include("BkgPackageSvcGroup").Where(cond => cond.BkgPackageSvcGroup.BPSG_BackgroundPackageID == backgroundPackageId && cond.BkgPackageSvcGroup.BPSG_BkgSvcGroupID == bkgSvcGroupId && cond.BPS_BackgroundServiceID == serviceId && cond.BPS_IsDeleted == false).FirstOrDefault().BPS_ID;
        }

        Boolean IBackgroundSetupRepository.SavePackageSvcAttributeMapping(BkgAttributeGroupMapping attributeMappingToAdd, Int32 bkgPackageSvcId, Int32 currentLoggedInUserId, Boolean isRequired, Boolean isDisplay)
        {
            BkgPackageSvcAttribute bkgPackageSvcAttributeMapping = new BkgPackageSvcAttribute();
            bkgPackageSvcAttributeMapping.BPSA_BkgAttributeGroupMappingID = attributeMappingToAdd.BAGM_ID;
            bkgPackageSvcAttributeMapping.BPSA_BkgPackageSvcID = bkgPackageSvcId;
            bkgPackageSvcAttributeMapping.BPSA_CreatedByID = currentLoggedInUserId;
            bkgPackageSvcAttributeMapping.BPSA_CreatedOn = DateTime.Now;
            bkgPackageSvcAttributeMapping.BPSA_IsDeleted = false;
            bkgPackageSvcAttributeMapping.BPSA_IsDisplay = isDisplay;
            bkgPackageSvcAttributeMapping.BPSA_IsRequired = isRequired;
            _ClientDBContext.BkgPackageSvcAttributes.AddObject(bkgPackageSvcAttributeMapping);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        Boolean IBackgroundSetupRepository.SaveAttributeOptionInClient(System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> attrOptionListMaster, Int32 currentLoggedInUserId)
        {
            foreach (Entity.BkgSvcAttributeOption addAttributeOption in attrOptionListMaster)
            {
                BkgSvcAttributeOption bkgSVcAttrOption = new BkgSvcAttributeOption();
                bkgSVcAttrOption.EBSAO_BkgSvcAttributeID = addAttributeOption.EBSAO_BkgSvcAttributeID;
                bkgSVcAttrOption.EBSAO_CreatedByID = currentLoggedInUserId;
                bkgSVcAttrOption.EBSAO_CreatedOn = DateTime.Now;
                bkgSVcAttrOption.EBSAO_IsActive = addAttributeOption.EBSAO_IsActive;
                bkgSVcAttrOption.EBSAO_IsDeleted = false;
                bkgSVcAttrOption.EBSAO_OptionText = addAttributeOption.EBSAO_OptionText;
                bkgSVcAttrOption.EBSAO_OptionValue = addAttributeOption.EBSAO_OptionValue;
                bkgSVcAttrOption.EBSAO_ID = addAttributeOption.EBSAO_ID;
                _ClientDBContext.BkgSvcAttributeOptions.AddObject(bkgSVcAttrOption);
            }

            return true;

        }
        #endregion

        #region Background Package Administration

        /// <summary>
        /// Returns all the service groups viewable to the current logged in user. 
        /// </summary>
        /// <returns>List of Compliance Categories</returns>
        public List<BkgSvcGroup> GetServiceGroups()
        {
            List<BkgSvcGroup> serviceGroups = _ClientDBContext.BkgSvcGroups.Where(obj => obj.BSG_IsDeleted == false).ToList();
            return serviceGroups;
        }

        /// <summary>
        /// Saves the Service Group.
        /// </summary>
        /// <param name="category">BkgSvcGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>BkgSvcGroup Entity</returns>
        public BkgSvcGroup SaveServiceGroupDetail(BkgSvcGroup svcGroup, Int32 currentLoggedInUserId)
        {
            svcGroup.BSG_CreatedById = currentLoggedInUserId;
            svcGroup.BSG_CreatedDate = DateTime.Now;
            svcGroup.BSG_IsDeleted = false;
            _ClientDBContext.BkgSvcGroups.AddObject(svcGroup);
            _ClientDBContext.SaveChanges();
            return svcGroup;
        }

        /// <summary>
        /// Checks if the service group name already exists.
        /// </summary>
        /// <param name="svcGrpName">service group Name</param>
        /// <param name="svcGrpID">Service Group Id</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfServiceGroupNameAlreadyExist(String svcGrpName, Int32 svcGrpID)
        {
            return _ClientDBContext.BkgSvcGroups.Any(obj => obj.BSG_Name.Trim().ToUpper() == svcGrpName.Trim().ToUpper() && obj.BSG_ID != svcGrpID && obj.BSG_IsDeleted == false);
        }

        /// <summary>
        /// Updates the Compliance Category.
        /// </summary>
        /// <param name="svcGrp">BkgSvcGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public void UpdateServiceGroupDetail(BkgSvcGroup svcGrp, Int32 svcGrpID, Int32 currentLoggedInUserId)
        {
            BkgSvcGroup svcGroup = _ClientDBContext.BkgSvcGroups.FirstOrDefault(obj => obj.BSG_ID == svcGrpID && obj.BSG_IsDeleted == false);
            svcGroup.BSG_Name = svcGrp.BSG_Name;
            svcGroup.BSG_Description = svcGrp.BSG_Description;
            svcGroup.BSG_Active = svcGrp.BSG_Active;
            svcGroup.BSG_ModifiedBy = currentLoggedInUserId;
            svcGroup.BSG_ModifiedDate = DateTime.Now;
            _ClientDBContext.SaveChanges();
        }

        public BkgSvcGroup getCurrentServiceGroupInfo(Int32 svcGrpID)
        {
            return _ClientDBContext.BkgSvcGroups.Where(obj => obj.BSG_ID == svcGrpID && obj.BSG_IsDeleted == false).FirstOrDefault();
        }

        public Boolean DeleteServiceGroup(Int32 svcGrpID, Int32 currentUserId)
        {
            //Boolean packageDependency = false;
            //Boolean itemDependency = false;
            //packageDependency = _ClientDBContext.CompliancePackageCategories.Any(obj => obj.CPC_CategoryID == categoryID && obj.CPC_IsDeleted == false);
            //itemDependency = _ClientDBContext.ComplianceCategoryItems.Any(obj => obj.CCI_CategoryID == categoryID && obj.CCI_IsDeleted == false);
            //if (!(packageDependency || itemDependency))
            //{
            BkgSvcGroup svcGroup = ClientDBContext.BkgSvcGroups.FirstOrDefault(obj => obj.BSG_ID == svcGrpID && obj.BSG_IsDeleted == false);
            svcGroup.BSG_IsDeleted = true;
            svcGroup.BSG_ModifiedDate = DateTime.Now;
            svcGroup.BSG_ModifiedBy = currentUserId;
            _ClientDBContext.SaveChanges();
            return true;
            //}
            //return false;
        }

        #endregion

        #region SetUpBkgInstitutionhierarchy

        /// <summary>
        /// Get Institute Hierarchy Tree For Background data
        /// </summary>
        /// <param name="orgUserID">optional parameter in case of super admin pass null</param>
        /// <returns></returns>
        public List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> GetBackgroundInstituteHierarchyTree(int? orgUserID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[ams].[usp_GetInstituteHierarchyTreeBkg]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserID", orgUserID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> instituteHierarchyTreeData = new List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract>();


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    instituteHierarchyTreeData = ds.Tables[0].AsEnumerable().Select(col =>
                         new INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract
                         {
                             TreeNodeTypeID = Convert.ToInt32(col["TreeNodeTypeID"]),
                             NodeID = Convert.ToString(col["NodeID"]),
                             ParentNodeID = col["ParentNodeID"] == DBNull.Value ? null : Convert.ToString(col["ParentNodeID"]),
                             Level = Convert.ToInt32(col["Level"]),
                             DataID = Convert.ToInt32(col["DataID"]),
                             Value = Convert.ToString(col["Value"]),
                             ParentDataID = col["ParentDataID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["ParentDataID"]),
                             UICode = Convert.ToString(col["UICode"]),
                             IsLabel = Convert.ToBoolean(col["IsLabel"]),
                             NodeCode = Convert.ToString(col["NodeCode"]),
                             ParentNodeCode = Convert.ToString(col["ParentNodeCode"]),
                             Associated = Convert.ToBoolean(col["Associated"]),
                             MappingID = Convert.ToInt32(col["MappingID"]),
                             EntityID = Convert.ToInt32(col["EntityID"]),
                             PermissionCode = Convert.ToString(col["PermissionCode"]),
                             //Used = Convert.ToBoolean(col["Used"]),
                             PermissionName = Convert.ToString(col["PermissionName"]),
                             ProfilePermissionCode = Convert.ToString(col["ProfilePermissionCode"]),
                             ProfilePermissionName = Convert.ToString(col["ProfilePermissionName"]),
                             VerificationPermissionCode = Convert.ToString(col["VerificationPermissionCode"]),
                             VerificationPermissionName = Convert.ToString(col["VerificationPermissionName"]),
                             OrderPermissionCode = Convert.ToString(col["OrderPermissionCode"]),
                             OrderPermissionName = Convert.ToString(col["OrderPermissionName"]),
                             DPM_DisplayOrder = col["DPM_DisplayOrder"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["DPM_DisplayOrder"]),
                             IsPackageAvailableForOrder = col["IsPackageAvailableForOrder"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(col["IsPackageAvailableForOrder"]),
                             IsPackageBundleAvailableForOrder = col["IsPackageBundleAvailableForOrder"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(col["IsPackageBundleAvailableForOrder"]),
                             PackageColorCode = Convert.ToString(col["PackageColorCode"])

                         }).ToList();
                }
                return instituteHierarchyTreeData;
            }
        }

        /// <summary>
        /// To get Program Packages by HierarchyMappingIdId
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public List<BkgPackageHierarchyMapping> GetProgramPackagesByHierarchyMappingId(Int32 deptProgramMappingID)
        {
            List<BkgPackageHierarchyMapping> mappedPackages = _ClientDBContext.BkgPackageHierarchyMappings.Include("BackgroundPackage")
                                                                       .Include("BkgOrderPackages").Include("BkgOrderPackages.BkgOrder").Include("BkgOrderPackages.BkgOrder.Order")
                                                                       .Where(obj => obj.DeptProgramMapping.DPM_ID == deptProgramMappingID
                                                                       && obj.BPHM_IsDeleted == false && obj.DeptProgramMapping.DPM_IsDeleted == false
                                                                       && obj.BackgroundPackage.BPA_IsDeleted == false && obj.BackgroundPackage.BPA_IsActive == true)
                                                                       .OrderBy(x => x.BPHM_Sequence).ToList();
            _ClientDBContext.Refresh(RefreshMode.ClientWins, mappedPackages);

            return mappedPackages;
        }

        /// <summary>
        /// To get not mapped Compliance Packages
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public List<BackgroundPackage> GetNotMappedBackGroungPackagesByMappingId(Int32 deptProgramMappingID)
        {
            return _ClientDBContext.usp_GetInstituteHierarchyTreePackagesBkg(deptProgramMappingID).ToList();
        }

        /// <summary>
        /// To save Program Package Mapping
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="IsCreatedByAdmin"></param>
        /// <returns></returns>
        public Boolean SaveHierarchyNodePackageMapping(BkgPackageHierarchyMapping newMapping, List<Int32> lstPaymentOptionIds, Int32 currentUserId)
        {
            var _currentDateTime = DateTime.Now;
            newMapping.BPHM_IsDeleted = false;
            newMapping.BPHM_CreatedByID = currentUserId;
            newMapping.BPHM_CreatedOn = _currentDateTime;

            //Set Sequence of new Package
            BkgPackageHierarchyMapping lastBkgPackageHierarchyMapping = _ClientDBContext.BkgPackageHierarchyMappings.Where(x => !x.BPHM_IsDeleted
                                        && x.BPHM_InstitutionHierarchyNodeID == newMapping.BPHM_InstitutionHierarchyNodeID).OrderByDescending(x => x.BPHM_Sequence).FirstOrDefault();

            Int32? displayOrder = (lastBkgPackageHierarchyMapping.IsNotNull() && lastBkgPackageHierarchyMapping.BPHM_Sequence > 0) ? lastBkgPackageHierarchyMapping.BPHM_Sequence + 1 : 1;
            newMapping.BPHM_Sequence = displayOrder;

            for (int i = 0; i < lstPaymentOptionIds.Count; i++)
            {
                BkgPackagePaymentOption _bppo = new BkgPackagePaymentOption();
                _bppo.BPPO_PaymentOptionID = lstPaymentOptionIds[i];
                _bppo.BPPO_BPHMID = newMapping.BPHM_ID;
                _bppo.BPPO_CreatedByID = currentUserId;
                _bppo.BPPO_CreatedOn = _currentDateTime;
                _bppo.BPPO_IsDeleted = false;
                newMapping.BkgPackagePaymentOptions.Add(_bppo);
            }

            _ClientDBContext.BkgPackageHierarchyMappings.AddObject(newMapping);
            _ClientDBContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// To delete bkg Package HierarchyMapping by ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean DeleteHirarchyPackageMappingByID(Int32 bkgPackageHierarchyMappingID, Int32 currentUserId)
        {
            BkgPackageHierarchyMapping existingMapping = _ClientDBContext.BkgPackageHierarchyMappings.FirstOrDefault(obj => obj.BPHM_ID == bkgPackageHierarchyMappingID && obj.BPHM_IsDeleted == false);
            if (existingMapping.IsNotNull())
            {
                existingMapping.BPHM_IsDeleted = true;
                existingMapping.BPHM_ModifiedByID = currentUserId;
                existingMapping.BPHM_ModifiedOn = DateTime.Now;
                int backgroundHierarchyPackageDisplayOrder = existingMapping.BPHM_Sequence ?? 0;
                existingMapping.BPHM_Sequence = AppConsts.NONE;
                List<BkgPackageHierarchyMapping> lstHierarchyBackgroundPackageMapping = null;
                //if deleted id gas Sequence zero it will not affect the other sequences
                if (backgroundHierarchyPackageDisplayOrder > 0)
                {
                    //Re-Order Sequence of the Records on deletion.
                    lstHierarchyBackgroundPackageMapping = _ClientDBContext.BkgPackageHierarchyMappings.Where(obj => !obj.BPHM_IsDeleted
                        && obj.BPHM_Sequence > backgroundHierarchyPackageDisplayOrder && obj.BPHM_InstitutionHierarchyNodeID == existingMapping.BPHM_InstitutionHierarchyNodeID).OrderBy(obj => obj.BPHM_Sequence).ToList();

                    //Cal the Method/SP to update the Sequence
                    UpdateBackgroundPackageSequence(lstHierarchyBackgroundPackageMapping, backgroundHierarchyPackageDisplayOrder, currentUserId);
                }
            }

            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean UpdateBackgroundPackageSequence(IList<BkgPackageHierarchyMapping> hierarchyPackagesToMove, Int32? destinationIndex, Int32 currentLoggedInUserId)
        {

            DataTable HierarchyBackgroundPacakageList = new DataTable();

            HierarchyBackgroundPacakageList.Columns.Add("BPA_ID", typeof(Int32));
            HierarchyBackgroundPacakageList.Columns.Add("BPA_DisplayOrder", typeof(Int32));
            HierarchyBackgroundPacakageList.Columns.Add("BPA_ModifiedBy", typeof(Int32));

            foreach (BkgPackageHierarchyMapping hierarchyPackage in hierarchyPackagesToMove)
            {
                HierarchyBackgroundPacakageList.Rows.Add(new object[] { hierarchyPackage.BPHM_ID, destinationIndex, currentLoggedInUserId });
                destinationIndex += 1;
            }


            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_UpdateHierarchyBackgroundPackageSequence", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeParameter", HierarchyBackgroundPacakageList);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    con.Close();
                    _ClientDBContext.Refresh(RefreshMode.StoreWins, hierarchyPackagesToMove);
                    return true;
                }
            }
            return false;
        }



        #endregion

        #region Communication

        /// <summary>
        /// Method is used to get the Contact list based on the institutionHierarchyNodeID
        /// </summary>
        /// <param name="institutionHierarchyNodeID"></param>
        /// <returns></returns>
        List<HierarchyContactMapping> IBackgroundSetupRepository.GetInstitutionContactUserData(Int32 institutionHierarchyNodeID)
        {
            return _ClientDBContext.HierarchyContactMappings.Include("InstitutionContact").Where(cond => cond.ICM_InstitutionHierarchyNodeID == institutionHierarchyNodeID && cond.ICM_IsDeleted == false).ToList();
        }


        /// <summary>
        /// Method is used to get the list of contact except selected node
        /// </summary>
        /// <param name="institutionHierarchyNodeID"></param>
        /// <returns></returns>
        List<InsContact> IBackgroundSetupRepository.GetInstitutionContactList(Int32 institutionHierarchyNodeID, Int32 contactID = AppConsts.MINUS_ONE)
        {

            if (contactID == AppConsts.MINUS_ONE)
            {

                List<Int32> lstContactIds = _ClientDBContext.InstitutionContacts.Include("HierarchyContactMappings").Where(cond => cond.HierarchyContactMappings.Any(con => con.ICM_InstitutionHierarchyNodeID == institutionHierarchyNodeID && con.ICM_IsDeleted == false))
                     .Select(cond => cond.ICO_ID).ToList();

                if (lstContactIds.Count == AppConsts.NONE)
                {
                    return _ClientDBContext.InstitutionContacts.Include("HierarchyContactMappings")
                      .Where(cond => cond.HierarchyContactMappings.Any(con => con.ICM_InstitutionHierarchyNodeID != institutionHierarchyNodeID && con.ICM_IsDeleted == false)).ToList()
                      .Select(col => new InsContact
                      {
                          ICO_ID = col.ICO_ID,
                          ICO_Name = col.ICO_FirstName + "," + col.ICO_LastName
                      }).ToList();
                }
                else
                {
                    return _ClientDBContext.InstitutionContacts.Include("HierarchyContactMappings")
                          .Where(cond => cond.HierarchyContactMappings.Any(con => !lstContactIds.Contains(cond.ICO_ID) && con.ICM_InstitutionHierarchyNodeID != institutionHierarchyNodeID && con.ICM_IsDeleted == false)).ToList()
                          .Select(col => new InsContact
                          {
                              ICO_ID = col.ICO_ID,
                              ICO_Name = col.ICO_FirstName + "," + col.ICO_LastName
                          }).ToList();
                }
            }
            else
            {
                return _ClientDBContext.InstitutionContacts.Include("HierarchyContactMappings")
                   .Where(cond => cond.ICO_ID == contactID || (cond.HierarchyContactMappings.Any(con => con.ICM_InstitutionHierarchyNodeID != institutionHierarchyNodeID && con.ICM_IsDeleted == false))).ToList()
                   .Select(col => new InsContact
                   {
                       ICO_ID = col.ICO_ID,
                       ICO_Name = col.ICO_FirstName + "," + col.ICO_LastName
                   }).ToList();
            }

        }




        /// <summary>
        /// Method is used to get the InstitutionContact based on instutionContactID
        /// </summary>
        /// <param name="instutionContactID"></param>
        /// <returns></returns>
        InstitutionContact IBackgroundSetupRepository.GetInstitutionContactList(Int32 instutionContactID)
        {
            return _ClientDBContext.InstitutionContacts.Where(cond => cond.ICO_ID == instutionContactID).FirstOrDefault();
        }


        /// <summary>
        ///  Method is used to Delete contact based on contactID
        /// </summary>
        /// <param name="institutionContactID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        List<Int32> IBackgroundSetupRepository.DeleteInstitutionContact(Int32 institutionContactID, Int32 currentLoggedInUserId, Int32 nodeId)
        {

            InstitutionContact insContact = _ClientDBContext.InstitutionContacts.Include("HierarchyContactMappings").Where(cond => cond.ICO_ID == institutionContactID).FirstOrDefault();

            // Resolved Bug #6997 ADB Contacts – On deleting a contact from a node, the contact gets removed from all the nodes of the application.
            // If Contact is mapped with Multiple Nodes then only remove current node mapping else remove contact + mapping
            if (!insContact.IsNull())
            {
                List<Int32> hierarchyContactMappingIds = insContact.HierarchyContactMappings.Select(cond => cond.ICM_ID).ToList();
                if (hierarchyContactMappingIds.Count > 1)
                {
                    HierarchyContactMapping hierarchyContactMapping = _ClientDBContext.HierarchyContactMappings.Where(obj => obj.ICM_InstitutionContactID == institutionContactID && obj.ICM_InstitutionHierarchyNodeID == nodeId && obj.ICM_IsDeleted == false).FirstOrDefault();
                    hierarchyContactMapping.ICM_IsDeleted = true;
                    hierarchyContactMapping.ICM_ModifiedByID = currentLoggedInUserId;
                    hierarchyContactMapping.ICM_ModifiedOn = DateTime.Now;
                    hierarchyContactMappingIds = hierarchyContactMappingIds.Where(cond => cond == hierarchyContactMapping.ICM_ID).ToList();
                }
                else
                {
                    insContact.ICO_IsDeleted = true;
                    insContact.ICO_ModifiedByID = currentLoggedInUserId;
                    insContact.ICO_ModifiedOn = DateTime.Now;
                    insContact.HierarchyContactMappings.ForEach(cond =>
                    {
                        cond.ICM_IsDeleted = true;
                        cond.ICM_ModifiedByID = currentLoggedInUserId;
                        cond.ICM_ModifiedOn = DateTime.Now;
                    });
                }

                if (_ClientDBContext.SaveChanges() > 0)
                    return hierarchyContactMappingIds;

            }

            return null;

        }


        /// <summary>
        /// Saves the Service Group.
        /// </summary>
        /// <param name="category">BkgSvcGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>BkgSvcGroup Entity</returns>
        public Int32 SaveContact(InstitutionContact institutionContact, Int32 currentLoggedInUserId, Int32 hierarchyNodeId, Boolean isNew, Int32 contactID = 0)
        {
            if (isNew == true)
            {
                _ClientDBContext.InstitutionContacts.AddObject(institutionContact);
                _ClientDBContext.SaveChanges();

                HierarchyContactMapping hierarchyContactMapping = new HierarchyContactMapping();
                hierarchyContactMapping.ICM_InstitutionContactID = institutionContact.ICO_ID;
                hierarchyContactMapping.ICM_InstitutionHierarchyNodeID = hierarchyNodeId;
                hierarchyContactMapping.ICM_OrganizationUserID = null;
                hierarchyContactMapping.ICM_IsDeleted = false;
                hierarchyContactMapping.ICM_CreatedOn = DateTime.Now;
                hierarchyContactMapping.ICM_CreatedByID = currentLoggedInUserId;
                _ClientDBContext.HierarchyContactMappings.AddObject(hierarchyContactMapping);
                _ClientDBContext.SaveChanges();
                return hierarchyContactMapping.ICM_ID;

            }
            else
            {
                InstitutionContact institutionContactData = _ClientDBContext.InstitutionContacts.Include("HierarchyContactMappings").Where(x => x.ICO_ID == contactID).FirstOrDefault();

                institutionContactData.ICO_FirstName = institutionContact.ICO_FirstName;
                institutionContactData.ICO_LastName = institutionContact.ICO_LastName;
                institutionContactData.ICO_Title = institutionContact.ICO_Title;
                institutionContactData.ICO_PrimaryPhone = institutionContact.ICO_PrimaryPhone;
                institutionContactData.ICO_PrimaryEmailAddress = institutionContact.ICO_PrimaryEmailAddress;
                institutionContactData.ICO_Address1 = institutionContact.ICO_Address1;
                institutionContactData.ICO_Address2 = institutionContact.ICO_Address2;
                institutionContactData.ICO_ZipCodeID = institutionContact.ICO_ZipCodeID;
                institutionContactData.ICO_IsDeleted = false;
                institutionContactData.ICO_CreatedByID = currentLoggedInUserId;
                institutionContactData.ICO_CreatedOn = DateTime.Now;



                institutionContactData.HierarchyContactMappings.Add(new HierarchyContactMapping
                {
                    ICM_InstitutionHierarchyNodeID = hierarchyNodeId,
                    ICM_OrganizationUserID = null,
                    ICM_IsDeleted = false,
                    ICM_CreatedOn = DateTime.Now,
                    ICM_CreatedByID = currentLoggedInUserId
                });
                _ClientDBContext.SaveChanges();
                return institutionContactData.HierarchyContactMappings.Where(cond => cond.ICM_InstitutionHierarchyNodeID == hierarchyNodeId).FirstOrDefault().ICM_ID;


            }

        }


        /// <summary>
        /// Saves the Service Group.
        /// </summary>
        /// <param name="category">BkgSvcGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>BkgSvcGroup Entity</returns>
        public Boolean UpdateContact(InstitutionContact institutionContact, Int32 contactID, Int32 currentLoggedInUserId, Int32 hierarchyNodeId, Boolean isContact = false)
        {
            InstitutionContact institutionContactDetail = null;

            institutionContactDetail = isContact == false ?
                _ClientDBContext.InstitutionContacts.Include("HierarchyContactMappings").Where(x => x.ICO_ID == contactID).FirstOrDefault()
                : _ClientDBContext.InstitutionContacts.Where(x => x.ICO_ID == contactID).FirstOrDefault();
            institutionContactDetail.ICO_FirstName = institutionContact.ICO_FirstName;
            institutionContactDetail.ICO_LastName = institutionContact.ICO_LastName;
            institutionContactDetail.ICO_Title = institutionContact.ICO_Title;
            institutionContactDetail.ICO_PrimaryPhone = institutionContact.ICO_PrimaryPhone;
            institutionContactDetail.ICO_PrimaryEmailAddress = institutionContact.ICO_PrimaryEmailAddress;
            institutionContactDetail.ICO_Address1 = institutionContact.ICO_Address1;
            institutionContactDetail.ICO_Address2 = institutionContact.ICO_Address2;
            institutionContactDetail.ICO_ZipCodeID = institutionContact.ICO_ZipCodeID;
            institutionContactDetail.ICO_IsDeleted = false;
            institutionContactDetail.ICO_ModifiedByID = currentLoggedInUserId;
            institutionContactDetail.ICO_ModifiedOn = DateTime.Now;

            //if (isContact == false)
            //{

            //    institutionContactDetail.HierarchyContactMappings.Add(new HierarchyContactMapping
            //    {
            //        ICM_InstitutionHierarchyNodeID = hierarchyNodeId,
            //        ICM_OrganizationUserID = null,
            //        ICM_IsDeleted = false,
            //        ICM_CreatedOn = DateTime.Now,
            //        ICM_CreatedByID = currentLoggedInUserId
            //    });
            //}

            _ClientDBContext.SaveChanges();


            return true;

        }

        /// <summary>
        /// To get HierarchyContactMapping by MappingIds
        /// </summary>
        /// <param name="hierarchyContactMappingIDs"></param>
        /// <returns></returns>
        public List<HierarchyContactMapping> GetHierarchyContactMappingByMappingIds(List<Int32> hierarchyContactMappingIDs)
        {
            return _ClientDBContext.HierarchyContactMappings.Include("InstitutionContact").Where(x => hierarchyContactMappingIDs.Contains(x.ICM_ID)
                && x.ICM_IsDeleted == false && x.InstitutionContact.ICO_IsDeleted == false).ToList();
        }

        #endregion

        #region Backgroung Package Hierarchy Mapping

        /// <summary>
        /// To get background packages by HierarchyMappingIds
        /// </summary>
        /// <param name="bkgHierarchyMappingIds"></param>
        /// <returns></returns>
        public List<BackgroundPackagesContract> GetOrderBkgPackageDetails(List<Int32> bkgHierarchyMappingIds,Int32 ? SelectedHierachyId)
        {
            List<BkgPackageHierarchyMapping> lstBkgPackageHierarchyMappings = _ClientDBContext.BkgPackageHierarchyMappings.Include("BackgroundPackage").Where(condition => bkgHierarchyMappingIds.Contains(condition.BPHM_ID)).ToList();
            List<BackgroundPackagesContract> lstBackgroundPackagesContract = new List<BackgroundPackagesContract>();
            foreach (BkgPackageHierarchyMapping bphm in lstBkgPackageHierarchyMappings)
            {
                BackgroundPackagesContract bkgPackageContract = new BackgroundPackagesContract();
                bkgPackageContract.BPHMId = bphm.BPHM_ID;
                bkgPackageContract.BPAId = bphm.BPHM_BackgroundPackageID;
                bkgPackageContract.BPAName = String.IsNullOrEmpty(bphm.BackgroundPackage.BPA_Label) ?
                                            bphm.BackgroundPackage.BPA_Name : bphm.BackgroundPackage.BPA_Label;
                bkgPackageContract.IsExclusive = false;
                bkgPackageContract.BasePrice = bphm.BPHM_PackageBasePrice.HasValue ? bphm.BPHM_PackageBasePrice.Value : 0;
                //UAT-3268
                bkgPackageContract.AdditionalPrice = bphm.BPHM_AdditionalPrice.HasValue ? bphm.BPHM_AdditionalPrice : 0;
                bkgPackageContract.IsReqToQualifyInRotation = bphm.BackgroundPackage.BPA_IsReqToQualifyInRotation;
                List<lkpPaymentOption> lstPaymentOptions = new List<lkpPaymentOption>();
                List<BkgPackagePaymentOption> lstBkgPackagePaymentOptions = bphm.BkgPackagePaymentOptions.Where(cond => !cond.BPPO_IsDeleted).ToList();
                if (lstBkgPackagePaymentOptions.IsNotNull() && lstBkgPackagePaymentOptions.Count > 0)
                {
                    lstPaymentOptions = lstBkgPackagePaymentOptions.Select(col => col.lkpPaymentOption).ToList();
                }
                if (lstPaymentOptions.Count == 0)
                {
                    bkgPackageContract.IsInvoiceOnlyAtPackageLevel = null; 
                    Int32 ? selectedHierarchyNodeID = !SelectedHierachyId.IsNullOrEmpty() ? SelectedHierachyId : 0 ;
                    List<DeptProgramPaymentOption> deptProgramPaymentOptions = _ClientDBContext.DeptProgramPaymentOptions.
                        Where(cond => !cond.DPPO_IsDeleted && cond.DPPO_DeptProgramMappingID == selectedHierarchyNodeID).ToList();
                    if (deptProgramPaymentOptions.All(sel => sel.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || sel.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                    {
                        bkgPackageContract.IsInvoiceOnlyAtPackageLevel = true;
                        bkgPackageContract.IsInvoiceToInstitutionType = true;
                        bkgPackageContract.ListPackagePaymentContract = new PackageServiceItemContract()
                        {
                            PackageID = bphm.BPHM_BackgroundPackageID,
                            BackgroundServiceID = !bphm.PackageServiceItems.IsNullOrEmpty() ? bphm.PackageServiceItems.Select(x => x.PSI_PackageServiceID).ToList() :
                                            new List<Int32>()
                        };
                    }
                }
                else if (lstPaymentOptions.Count == 1)
                {
                    bkgPackageContract.IsInvoiceOnlyAtPackageLevel = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                    bkgPackageContract.IsInvoiceToInstitutionType = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                    bkgPackageContract.ListPackagePaymentContract = new PackageServiceItemContract()
                    {
                        PackageID = bphm.BPHM_BackgroundPackageID,
                        BackgroundServiceID = !bphm.PackageServiceItems.IsNullOrEmpty() ? bphm.PackageServiceItems.Select(x => x.PSI_PackageServiceID).ToList() :
                                            new List<Int32>()
                    };
                }
                else if (lstPaymentOptions.Count == 2)
                {
                    bkgPackageContract.IsInvoiceOnlyAtPackageLevel = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                    bkgPackageContract.IsInvoiceToInstitutionType = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                    bkgPackageContract.ListPackagePaymentContract = new PackageServiceItemContract()
                    {
                        PackageID = bphm.BPHM_BackgroundPackageID,
                        BackgroundServiceID = !bphm.PackageServiceItems.IsNullOrEmpty() ? bphm.PackageServiceItems.Select(x => x.PSI_PackageServiceID).ToList() :
                                            new List<Int32>()
                    };
                }
                else
                {
                    bkgPackageContract.IsInvoiceOnlyAtPackageLevel = false;
                }

                lstBackgroundPackagesContract.Add(bkgPackageContract);
            }
            return lstBackgroundPackagesContract;
        }

        #endregion

        #region Background Attributes

        public List<BkgSvcAttribute> GetServiceAttributes(Int32 tenantId, Int32 defaultTenantId)
        {
            if (tenantId == defaultTenantId)
            {
                List<Int32> svcAttributeIDs = GetSvcAttributeIDsFromBkgSvcAttributeTenant(defaultTenantId);

                return _ClientDBContext.BkgSvcAttributes
                      .Where(cond =>
                          !cond.BSA_IsDeleted && svcAttributeIDs.Contains(cond.BSA_ID)).ToList();


            }
            else
            {

                return _ClientDBContext.BkgSvcAttributes
                     .Where(cond =>
                         !cond.BSA_IsDeleted
                         ).ToList();
            }

        }

        public BkgSvcAttribute GetServiceAttributeBasedOnAttributeID(Int32 serviceAttributeID)
        {
            return _ClientDBContext.BkgSvcAttributes.Include("BkgSvcAttributeOptions")
             .FirstOrDefault(cond => cond.BSA_ID.Equals(serviceAttributeID)
                && !cond.BSA_IsDeleted);
        }

        public Boolean AddServiceAttributeToClient(BkgSvcAttribute serviceAttribute)
        {
            serviceAttribute.BSA_Code = serviceAttribute.BSA_Code.Equals(Guid.Empty) ? Guid.NewGuid() : serviceAttribute.BSA_Code;
            _ClientDBContext.BkgSvcAttributes.AddObject(serviceAttribute);
            _ClientDBContext.SaveChanges();
            return true;
        }

        public Boolean UpdateServiceAttribute(Entity.BkgSvcAttribute serviceAttribute, List<BkgSvcAttributeOption> lstAttributeOption)
        {
            BkgSvcAttribute serviceAttributeInDb = GetServiceAttributeBasedOnAttributeID(serviceAttribute.BSA_ID);

            if (serviceAttributeInDb != null)
            {
                serviceAttributeInDb.BSA_DataTypeID = serviceAttribute.BSA_DataTypeID;
                serviceAttributeInDb.BSA_Name = serviceAttribute.BSA_Name;
                serviceAttributeInDb.BSA_Label = serviceAttribute.BSA_Label;
                serviceAttributeInDb.BSA_Description = serviceAttribute.BSA_Description;
                serviceAttributeInDb.BSA_MaxDateValue = serviceAttribute.BSA_MaxDateValue;
                serviceAttributeInDb.BSA_MaxIntValue = serviceAttribute.BSA_MaxIntValue;
                serviceAttributeInDb.BSA_MaxLength = serviceAttribute.BSA_MaxLength;
                serviceAttributeInDb.BSA_MinDateValue = serviceAttribute.BSA_MinDateValue;
                serviceAttributeInDb.BSA_MinIntValue = serviceAttribute.BSA_MinIntValue;
                serviceAttributeInDb.BSA_MinLength = serviceAttribute.BSA_MinLength;
                serviceAttributeInDb.BSA_Active = serviceAttribute.BSA_Active;
                serviceAttributeInDb.BSA_IsEditable = serviceAttribute.BSA_IsEditable;
                //serviceAttributeInDb.BSA_IsSystemPreConfiguredq = serviceAttribute.BSA_IsSystemPreConfiguredq;
                //serviceAttributeInDb.BSA_IsRequired = serviceAttribute.BSA_IsRequired;
                //serviceAttributeInDb.BSA_ReqValidationMessage = serviceAttribute.BSA_ReqValidationMessage;
                //complianceAttributeInDb.CopiedFromCode = complianceAttribute.Code;
                serviceAttributeInDb.BSA_ModifiedBy = serviceAttribute.BSA_ModifiedBy;
                serviceAttributeInDb.BSA_ModifiedDate = serviceAttribute.BSA_ModifiedDate;
                UpdateServiceAttributeOption(serviceAttributeInDb, serviceAttribute, lstAttributeOption);
                _ClientDBContext.SaveChanges();
                //get values from database which may be useful in operations afterwards.
                serviceAttribute.BSA_Code = serviceAttributeInDb.BSA_Code;
                return true;
            }
            return false;
        }

        private Boolean UpdateServiceAttributeOption(BkgSvcAttribute serviceAttributeInDb, Entity.BkgSvcAttribute serviceAttribute, List<BkgSvcAttributeOption> lstAttributeOption)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> attributeOptions = serviceAttribute.BkgSvcAttributeOptions;

            IEnumerable<BkgSvcAttributeOption> attributeOptionsInDb = serviceAttributeInDb.BkgSvcAttributeOptions
                .Where(x => x.EBSAO_IsActive && !x.EBSAO_IsDeleted);

            // Deletes attribute options
            foreach (BkgSvcAttributeOption attributeOptionIndb in attributeOptionsInDb)
            {
                if (attributeOptions.Any(x => x.EBSAO_OptionText == attributeOptionIndb.EBSAO_OptionText
                    && x.EBSAO_OptionValue == attributeOptionIndb.EBSAO_OptionValue))
                    continue;

                attributeOptionIndb.EBSAO_IsDeleted = true;
                attributeOptionIndb.EBSAO_ModifiedOn = DateTime.Now;
                attributeOptionIndb.EBSAO_ModifiedByID = serviceAttribute.BSA_ModifiedBy;
            }

            foreach (BkgSvcAttributeOption addAttributeOption in lstAttributeOption)
                serviceAttributeInDb.BkgSvcAttributeOptions.Add(addAttributeOption);

            return true;
        }

        public Boolean DeleteServiceAttribute(Int32 serviceAttributeID, Int32 modifiedByID)
        {
            //if (ClientDBContext.RuleSetObjects.Any(x => x.RLSO_ObjectID == serviceAttributeID && !x.RLSO_IsDeleted))
            //    return false;
            BkgSvcAttribute svcAttribute = GetServiceAttributeBasedOnAttributeID(serviceAttributeID);
            IEnumerable<BkgSvcAttributeOption> attributeOptionsInDb = svcAttribute.BkgSvcAttributeOptions
                .Where(x => x.EBSAO_IsActive && !x.EBSAO_IsDeleted && x.EBSAO_BkgSvcAttributeID == serviceAttributeID);
            if (svcAttribute != null)
            {
                svcAttribute.BSA_IsDeleted = true;
                svcAttribute.BSA_ModifiedBy = modifiedByID;
                svcAttribute.BSA_ModifiedDate = DateTime.Now;
                foreach (BkgSvcAttributeOption attributeOptionIndb in attributeOptionsInDb)
                {
                    attributeOptionIndb.EBSAO_IsDeleted = true;
                    attributeOptionIndb.EBSAO_ModifiedOn = DateTime.Now;
                    attributeOptionIndb.EBSAO_ModifiedByID = modifiedByID;
                }
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public Boolean checkIfSvcMappingIsDefinedForAttribute(Int32 attributeId, Int32 tenantId)
        {
            return _ClientDBContext.BkgAttributeGroupMappings.Any(x => x.BAGM_BkgSvcAtributeID == attributeId
                                                         && !x.BAGM_IsDeleted);
        }

        public Boolean DeleteServiceAttributeTenant(Int32 serviceAttributeID, Int32 modifiedByID)
        {
            Entity.BkgSvcAttributeTenant svcAttributeTenants = base.SecurityContext.BkgSvcAttributeTenants.Where(cond => cond.BSAT_BkgSvcAttributeID == serviceAttributeID && !cond.BSAT_IsDeleted).FirstOrDefault();
            if (svcAttributeTenants.IsNotNull())
            {
                svcAttributeTenants.BSAT_IsDeleted = true;
                svcAttributeTenants.BSAT_ModifiedByID = modifiedByID;
                svcAttributeTenants.BSAT_ModifiedOn = DateTime.Now;
                base.SecurityContext.SaveChanges();
            }
            return true;
        }

        public List<Int32> GetSvcAttributeIDsFromBkgSvcAttributeTenant(Int32 defaultTenantId)
        {
            return base.SecurityContext.BkgSvcAttributeTenants.Where(cond => cond.BSAT_TenantID == defaultTenantId)
                                                                                     .Select(col => col.BSAT_BkgSvcAttributeID).ToList();
        }

        public Entity.BkgSvcAttribute GetMasterServiceAttributeBasedOnAttributeID(Int32 serviceAttributeID)
        {
            return base.SecurityContext.BkgSvcAttributes.Include("BkgSvcAttributeOptions")
             .FirstOrDefault(cond => cond.BSA_ID.Equals(serviceAttributeID)
                && !cond.BSA_IsDeleted);
        }

        public Boolean AddServiceAttribute(Entity.BkgSvcAttribute serviceAttribute, Int32 tenantId, Int32 defaultTenantId)
        {
            serviceAttribute.BSA_Code = serviceAttribute.BSA_Code.Equals(Guid.Empty) ? Guid.NewGuid() : serviceAttribute.BSA_Code;
            base.SecurityContext.BkgSvcAttributes.AddObject(serviceAttribute);
            base.SecurityContext.SaveChanges();
            AddBkgSvcAttributeTenant(serviceAttribute, tenantId);
            return true;
        }

        public void AddBkgSvcAttributeTenant(Entity.BkgSvcAttribute serviceAttribute, Int32 tenantId)
        {
            Entity.BkgSvcAttributeTenant bkgSvcAttributeTenant = new Entity.BkgSvcAttributeTenant();
            bkgSvcAttributeTenant.BSAT_TenantID = tenantId;
            bkgSvcAttributeTenant.BSAT_BkgSvcAttributeID = serviceAttribute.BSA_ID;
            bkgSvcAttributeTenant.BSAT_IsDeleted = false;
            bkgSvcAttributeTenant.BSAT_CreatedByID = serviceAttribute.BSA_CreatedById;
            bkgSvcAttributeTenant.BSAT_CreatedOn = serviceAttribute.BSA_CreatedDate;
            bkgSvcAttributeTenant.BSAT_ModifiedByID = null;
            bkgSvcAttributeTenant.BSAT_ModifiedOn = null;
            base.SecurityContext.BkgSvcAttributeTenants.AddObject(bkgSvcAttributeTenant);
            base.SecurityContext.SaveChanges();
        }

        public Boolean AddMasterAttributeOptions(List<Entity.BkgSvcAttributeOption> masterAttributeOptions, Entity.BkgSvcAttribute svcAttribute)
        {

            if (svcAttribute.IsNotNull())
            {
                foreach (Entity.BkgSvcAttributeOption addAttributeOption in masterAttributeOptions)
                    svcAttribute.BkgSvcAttributeOptions.Add(addAttributeOption);
                base.SecurityContext.SaveChanges();
                return true;

            }

            return false; ;
        }




        public Boolean UpdateMasterServiceAttribute(Entity.BkgSvcAttribute serviceAttribute)
        {
            Entity.BkgSvcAttribute serviceAttributeInDb = GetMasterServiceAttributeBasedOnAttributeID(serviceAttribute.BSA_ID);

            if (serviceAttributeInDb != null)
            {
                serviceAttributeInDb.BSA_DataTypeID = serviceAttribute.BSA_DataTypeID;
                serviceAttributeInDb.BSA_Name = serviceAttribute.BSA_Name;
                serviceAttributeInDb.BSA_Label = serviceAttribute.BSA_Label;
                serviceAttributeInDb.BSA_Description = serviceAttribute.BSA_Description;
                serviceAttributeInDb.BSA_MaxDateValue = serviceAttribute.BSA_MaxDateValue;
                serviceAttributeInDb.BSA_MaxIntValue = serviceAttribute.BSA_MaxIntValue;
                serviceAttributeInDb.BSA_MaxLength = serviceAttribute.BSA_MaxLength;
                serviceAttributeInDb.BSA_MinDateValue = serviceAttribute.BSA_MinDateValue;
                serviceAttributeInDb.BSA_MinIntValue = serviceAttribute.BSA_MinIntValue;
                serviceAttributeInDb.BSA_MinLength = serviceAttribute.BSA_MinLength;
                serviceAttributeInDb.BSA_Active = serviceAttribute.BSA_Active;
                serviceAttributeInDb.BSA_IsEditable = serviceAttribute.BSA_IsEditable;
                // serviceAttributeInDb.BSA_IsSystemPreConfiguredq = serviceAttribute.BSA_IsSystemPreConfiguredq;
                // serviceAttributeInDb.BSA_IsRequired = serviceAttribute.BSA_IsRequired;
                //  serviceAttributeInDb.BSA_ReqValidationMessage = serviceAttribute.BSA_ReqValidationMessage;
                //complianceAttributeInDb.CopiedFromCode = complianceAttribute.Code;
                serviceAttributeInDb.BSA_ModifiedBy = serviceAttribute.BSA_ModifiedBy;
                serviceAttributeInDb.BSA_ModifiedDate = serviceAttribute.BSA_ModifiedDate;
                UpdateMasterServiceAttributeOption(serviceAttributeInDb, serviceAttribute);
                base.SecurityContext.SaveChanges();
                //get values from database which may be useful in operations afterwards.
                serviceAttribute.BSA_Code = serviceAttributeInDb.BSA_Code;
                return true;
            }
            return false;
        }

        private void UpdateMasterServiceAttributeOption(Entity.BkgSvcAttribute serviceAttributeInDb, Entity.BkgSvcAttribute serviceAttribute)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> attributeOptions = serviceAttribute.BkgSvcAttributeOptions;

            IEnumerable<Entity.BkgSvcAttributeOption> attributeOptionsInDb = serviceAttributeInDb.BkgSvcAttributeOptions
                .Where(x => x.EBSAO_IsActive && !x.EBSAO_IsDeleted);

            // Deletes attribute options
            foreach (Entity.BkgSvcAttributeOption attributeOptionIndb in attributeOptionsInDb)
            {
                if (attributeOptions.Any(x => x.EBSAO_OptionText == attributeOptionIndb.EBSAO_OptionText
                    && x.EBSAO_OptionValue == attributeOptionIndb.EBSAO_OptionValue))
                    continue;

                attributeOptionIndb.EBSAO_IsDeleted = true;
                attributeOptionIndb.EBSAO_ModifiedOn = DateTime.Now;
                attributeOptionIndb.EBSAO_ModifiedByID = serviceAttribute.BSA_ModifiedBy;
            }

            List<Entity.BkgSvcAttributeOption> needToAddAttributeOptions = new List<Entity.BkgSvcAttributeOption>();
            // Adds attribute options
            foreach (Entity.BkgSvcAttributeOption attributeOption in attributeOptions)
            {
                if (attributeOptionsInDb.Any(x => x.EBSAO_OptionText == attributeOption.EBSAO_OptionText
                    && x.EBSAO_OptionValue == attributeOption.EBSAO_OptionValue))
                    continue;

                needToAddAttributeOptions.Add(attributeOption);
            }

            foreach (Entity.BkgSvcAttributeOption addAttributeOption in needToAddAttributeOptions)
                serviceAttributeInDb.BkgSvcAttributeOptions.Add(addAttributeOption);
        }

        public List<CascadingAttributeOptionsContract> GetCascadingAttributeOptions(Int32 attributeId)
        {
            return ClientDBContext.CascadingAttributeOptions.Where(op => !op.CAO_IsDeleted
            && op.CAO_AttributeID == attributeId)
            .Select(op => new CascadingAttributeOptionsContract
            {
                Id = op.CAO_ID,
                AttributeId = op.CAO_AttributeID,
                Value = op.CAO_Value,
                SourceValue = op.CAO_SourceValue,
                DisplaySequence = op.CAO_DisplaySequence ?? 0
            }).OrderBy(op => op.DisplaySequence).ToList();
        }

        public Entity.CascadingAttributeOption SaveMasterCascadingAttributeOption(CascadingAttributeOptionsContract cascadingAttributeOptionsContract, Int32 currentLoggedInUserId)
        {
            Boolean bResult = false;
            Entity.CascadingAttributeOption cascadingAttributeOption = null;

            if (cascadingAttributeOptionsContract.Id > Convert.ToInt32(DefaultNumbers.None))
            {
                cascadingAttributeOption = base.SecurityContext.CascadingAttributeOptions.FirstOrDefault(op => !op.CAO_IsDeleted
                            && op.CAO_ID == cascadingAttributeOptionsContract.Id);
            }

            if (cascadingAttributeOption == null)
            {
                cascadingAttributeOption = new Entity.CascadingAttributeOption();

                cascadingAttributeOption.CAO_DisplaySequence = (ClientDBContext.CascadingAttributeOptions
                                        .Where(cao => !cao.CAO_IsDeleted
                                        && cao.CAO_AttributeID == cascadingAttributeOptionsContract.AttributeId)
                                        .Select(cao => cao.CAO_DisplaySequence)
                                        .OrderByDescending(cao => cao)
                                        .FirstOrDefault() ?? 0) + 1;

                base.SecurityContext.CascadingAttributeOptions.AddObject(cascadingAttributeOption);
                cascadingAttributeOption.CAO_AttributeID = cascadingAttributeOptionsContract.AttributeId;
                cascadingAttributeOption.CAO_CreatedById = currentLoggedInUserId;
                cascadingAttributeOption.CAO_CreatedDate = DateTime.Now;

            }
            else
            {
                cascadingAttributeOption.CAO_ModifiedBy = currentLoggedInUserId;
                cascadingAttributeOption.CAO_ModifiedDate = DateTime.Now;
            }

            cascadingAttributeOption.CAO_Value = cascadingAttributeOptionsContract.Value;
            cascadingAttributeOption.CAO_SourceValue = cascadingAttributeOptionsContract.SourceValue;
            //cascadingAttributeOption.CAO_DisplaySequence = cascadingAttributeOptionsContract.DisplaySequence;

            bResult = base.SecurityContext.SaveChanges() > Convert.ToInt32(DefaultNumbers.None);

            return bResult ? cascadingAttributeOption : null;
        }

        public CascadingAttributeOption SaveClientCascadingAttributeOption(CascadingAttributeOptionsContract cascadingAttributeOptionsContract, Int32 currentLoggedInUserId)
        {
            Boolean bResult = false;
            CascadingAttributeOption cascadingAttributeOption = null;

            if (cascadingAttributeOptionsContract.Id > Convert.ToInt32(DefaultNumbers.None))
            {
                cascadingAttributeOption = ClientDBContext.CascadingAttributeOptions.FirstOrDefault(op => !op.CAO_IsDeleted
                            && op.CAO_ID == cascadingAttributeOptionsContract.Id);
            }

            if (cascadingAttributeOption == null)
            {
                cascadingAttributeOption = new CascadingAttributeOption();
                cascadingAttributeOption.CAO_ID = cascadingAttributeOptionsContract.Id;
                ClientDBContext.CascadingAttributeOptions.AddObject(cascadingAttributeOption);
                cascadingAttributeOption.CAO_AttributeID = cascadingAttributeOptionsContract.AttributeId;
                cascadingAttributeOption.CAO_CreatedById = currentLoggedInUserId;
                cascadingAttributeOption.CAO_CreatedDate = DateTime.Now;

            }
            else
            {
                cascadingAttributeOption.CAO_ModifiedBy = currentLoggedInUserId;
                cascadingAttributeOption.CAO_ModifiedDate = DateTime.Now;
            }

            cascadingAttributeOption.CAO_Value = cascadingAttributeOptionsContract.Value;
            cascadingAttributeOption.CAO_SourceValue = cascadingAttributeOptionsContract.SourceValue;
            cascadingAttributeOption.CAO_DisplaySequence = cascadingAttributeOptionsContract.DisplaySequence;

            bResult = ClientDBContext.SaveChanges() > Convert.ToInt32(DefaultNumbers.None);

            return bResult ? cascadingAttributeOption : null;
        }

        public Boolean DeleteCascadingAttributeOption(Int32 optionId, Int32 currentLoggedInUserId)
        {
            Boolean bResult = false;
            var entity = ClientDBContext.CascadingAttributeOptions.FirstOrDefault(op => !op.CAO_IsDeleted
                            && op.CAO_ID == optionId);
            if (entity != null)
            {
                entity.CAO_IsDeleted = true;
                entity.CAO_ModifiedBy = currentLoggedInUserId;
                entity.CAO_ModifiedDate = DateTime.Now;
                bResult = ClientDBContext.SaveChanges() > Convert.ToInt32(DefaultNumbers.None);
            }
            return bResult;
        }

        #endregion

        #region Manage Services Attribute Group

        public List<Entity.BkgSvcAttributeGroup> GetServiceAttributeGroups()
        {
            List<Entity.BkgSvcAttributeGroup> svcAttributeGroups = base.SecurityContext.BkgSvcAttributeGroups.Where(x => !x.BSAD_IsDeleted).ToList();
            return svcAttributeGroups;
        }

        public List<Entity.ClientEntity.BkgSvcAttributeGroup> GetServiceAttributeGroupsByTenant()
        {
            return _ClientDBContext.BkgSvcAttributeGroups.Where(x => !x.BSAD_IsDeleted).ToList();
        }

        public Boolean CheckIfSvcAttrGrpNameAlreadyExist(String svcAttrGrpName, Int32 svcAttrGrpID)
        {
            return base.SecurityContext.BkgSvcAttributeGroups.Any(obj => obj.BSAD_Name.Trim().ToUpper() == svcAttrGrpName.Trim().ToUpper() && obj.BSAD_ID != svcAttrGrpID && !obj.BSAD_IsDeleted);
        }

        public Boolean SaveServiceAttributeGroup(Entity.BkgSvcAttributeGroup svcAttrGrp)
        {
            svcAttrGrp.BSAD_Code = svcAttrGrp.BSAD_Code.Equals(Guid.Empty) ? Guid.NewGuid() : svcAttrGrp.BSAD_Code;
            base.SecurityContext.BkgSvcAttributeGroups.AddObject(svcAttrGrp);
            base.SecurityContext.SaveChanges();
            return true;
        }

        public Boolean UpdateServiceAttributeGroup(Entity.BkgSvcAttributeGroup svcAttrGrp, Int32 attrGrpId)
        {
            Entity.BkgSvcAttributeGroup svcAttrGrpInDB = base.SecurityContext.BkgSvcAttributeGroups.FirstOrDefault(obj => obj.BSAD_ID == attrGrpId && !obj.BSAD_IsDeleted);
            svcAttrGrpInDB.BSAD_Name = svcAttrGrp.BSAD_Name;
            svcAttrGrpInDB.BSAD_Description = svcAttrGrp.BSAD_Description;
            //svcAttrGrpInDB.BSAD_IsRequired = svcAttrGrp.BSAD_IsRequired;
            //svcAttrGrpInDB.BSAD_IsDisplay = svcAttrGrp.BSAD_IsDisplay;
            svcAttrGrpInDB.BSAD_ModifiedBy = svcAttrGrp.BSAD_ModifiedBy;
            svcAttrGrpInDB.BSAD_ModifiedDate = svcAttrGrp.BSAD_ModifiedDate;
            base.SecurityContext.SaveChanges();
            return true;
        }

        public Boolean DeleteServiceAttributeGroup(Int32 svcAttrGrpID, Int32 currentUserId)
        {
            Entity.BkgSvcAttributeGroup svcAttrGroup = base.SecurityContext.BkgSvcAttributeGroups.FirstOrDefault(obj => obj.BSAD_ID == svcAttrGrpID && !obj.BSAD_IsDeleted);
            svcAttrGroup.BSAD_IsDeleted = true;
            svcAttrGroup.BSAD_ModifiedDate = DateTime.Now;
            svcAttrGroup.BSAD_ModifiedBy = currentUserId;
            base.SecurityContext.SaveChanges();
            return true;
        }

        public Entity.BkgSvcAttributeGroup GetServiceAttributeGroupBasedOnAttributeGrpID(Int32 serviceAttributeGrpID)
        {
            return base.SecurityContext.BkgSvcAttributeGroups.FirstOrDefault(cond => cond.BSAD_ID.Equals(serviceAttributeGrpID)
                && !cond.BSAD_IsDeleted);
        }

        #endregion

        #region Service Attribute Group Mapping

        public List<MapServiceAttributeToGroupContract> GetMappedAttributes(Int32 serviceAttrGrpId)
        {
            return base.SecurityContext.BkgAttributeGroupMappings.Include("lkpSvcAttributeDataType").Include("BkgSvcAttributeGroup")
                          .Where(cond => !cond.BAGM_IsDeleted && cond.BAGM_BkgSvcAttributeGroupId == serviceAttrGrpId && !cond.BkgSvcAttributeGroup.BSAD_IsDeleted && !cond.BkgSvcAttribute.BSA_IsDeleted)
                          .Select(col => new MapServiceAttributeToGroupContract
                          {
                              AttributeGroupMappingID = col.BAGM_ID,
                              AttributeGroupID = col.BkgSvcAttributeGroup.BSAD_ID,
                              AttributeID = col.BkgSvcAttribute.BSA_ID,
                              AttributeName = col.BkgSvcAttribute.BSA_Name,
                              DisplaySequence = col.BAGM_DisplaySequence,
                              IsDisplay = col.BAGM_IsDisplay,
                              IsRequired = col.BAGM_IsRequired,
                              IsEditable = col.BAGM_IsEditable,
                              IsDeleted = col.BAGM_IsDeleted,
                              AttributeDataTypeCode = col.BkgSvcAttribute.lkpSvcAttributeDataType.SADT_Code,
                              SourceAttributeID = col.BAGM_SourceAttributeID,         
                              IsHiddenFromUI=col.BAGM_IsHiddenFromUI
                          }).Distinct().OrderBy(o => o.DisplaySequence)
                          .ToList();

        }

        public List<Entity.BkgSvcAttribute> GetUnmappedAttributes(Int32 attributegrpID, Int32 defaultTenantId)
        {
            List<String> specialTypes = new List<String> { "Country", "State", "County", "City", "Zip code" };
            List<Int32> mappedAttriIdstoAttributeGrp = base.SecurityContext.BkgAttributeGroupMappings.Where(x => x.BAGM_BkgSvcAttributeGroupId == attributegrpID && !x.BAGM_IsDeleted)
                                                           .Select(x => x.BAGM_BkgSvcAtributeID).ToList();

            List<String> exstingTypes = base.SecurityContext.BkgSvcAttributes.Where(x => mappedAttriIdstoAttributeGrp.Contains(x.BSA_ID) && specialTypes.Contains(x.lkpSvcAttributeDataType.SADT_Name))
                                                                                                           .Select(y => y.lkpSvcAttributeDataType.SADT_Name).ToList();

            List<Int32> masterAttributeIds = GetSvcAttributeIDsFromBkgSvcAttributeTenant(defaultTenantId);
            return base.SecurityContext.BkgSvcAttributes.Where(x => !x.BSA_IsDeleted && !mappedAttriIdstoAttributeGrp.Contains(x.BSA_ID) && masterAttributeIds.Contains(x.BSA_ID) && !exstingTypes.Contains(x.lkpSvcAttributeDataType.SADT_Name)).Distinct()
                        .OrderBy(con => con.BSA_Name).ToList();
        }

        public List<Entity.BkgSvcAttribute> GetSourceAttributes(Int32 childAttributeId, Int32 attributeGroupId, Int32 defaultTenantId)
        {

            var cascadingType = SvcAttributeDataType.CASCADING.GetStringValue();

            return base.SecurityContext.BkgAttributeGroupMappings.Where(a => !a.BAGM_IsDeleted
            && a.BAGM_BkgSvcAtributeID != childAttributeId
            && a.BAGM_BkgSvcAttributeGroupId == attributeGroupId
            && a.BkgSvcAttribute.lkpSvcAttributeDataType.SADT_Code == cascadingType
            ).Distinct()
            .OrderBy(con => con.BkgSvcAttribute.BSA_Name).Select(a => a.BkgSvcAttribute).ToList();
        }

        public void SaveAttributeGroupMapping(List<Entity.BkgAttributeGroupMapping> lstBkgSvcAttributeGroupMapping, Int32 svcAttributeGrpId)
        {
            // Set Sequence of new form
            Entity.BkgAttributeGroupMapping lastAttribute = base.SecurityContext.BkgAttributeGroupMappings.Where(obj => obj.BAGM_BkgSvcAttributeGroupId == svcAttributeGrpId && !obj.BAGM_IsDeleted).OrderByDescending(i => i.BAGM_DisplaySequence).FirstOrDefault();
            Int32 displaySequence = (lastAttribute.IsNotNull()) ? lastAttribute.BAGM_DisplaySequence.Value + 1 : 1;
            for (int i = 0; i < lstBkgSvcAttributeGroupMapping.Count(); i++)
            {
                lstBkgSvcAttributeGroupMapping[i].BAGM_DisplaySequence = displaySequence + i;
                // newAttributeGrpMapping.BAGM_DisplaySequence = (lastAttribute.IsNotNull()) ?((lastAttribute.BAGM_DisplaySequence.IsNotNull())? lastAttribute.BAGM_DisplaySequence + 1 : 1) : 1;
                base.SecurityContext.BkgAttributeGroupMappings.AddObject(lstBkgSvcAttributeGroupMapping[i]);
            }
            base.SecurityContext.SaveChanges();

        }

        public Entity.BkgAttributeGroupMapping getAttributeGroupMappingById(Int32 attributeGrpMappingID)
        {
            return base.SecurityContext.BkgAttributeGroupMappings.Where(cond => cond.BAGM_ID == attributeGrpMappingID && !cond.BAGM_IsDeleted).FirstOrDefault();
        }

        public Boolean DeleteAttributeGroupMapping(Int32 attributeGrpMappingID, Int32 currentLoggedInUserId)
        {

            Entity.BkgAttributeGroupMapping attributeGroupMappingInDb = getAttributeGroupMappingById(attributeGrpMappingID);
            if (attributeGroupMappingInDb.IsNotNull())
            {
                attributeGroupMappingInDb.BAGM_IsDeleted = true;
                attributeGroupMappingInDb.BAGM_ModifiedBy = currentLoggedInUserId;
                attributeGroupMappingInDb.BAGM_ModifiedOn = DateTime.Now;
                //Re-Order Display Sequence of the Records.
                List<Entity.BkgAttributeGroupMapping> lstAttributes = base.SecurityContext.BkgAttributeGroupMappings.Where(cond => !cond.BAGM_IsDeleted &&
                    cond.BAGM_BkgSvcAttributeGroupId == attributeGroupMappingInDb.BAGM_BkgSvcAttributeGroupId && cond.BAGM_DisplaySequence > attributeGroupMappingInDb.BAGM_DisplaySequence).ToList();

                DataTable dtAttributes = new DataTable();
                dtAttributes.Columns.Add("AttributeGroupMappingID", typeof(Int32));
                dtAttributes.Columns.Add("DestinationIndex", typeof(Int32));
                dtAttributes.Columns.Add("CurrentLoggedInUserId", typeof(Int32));
                foreach (Entity.BkgAttributeGroupMapping attribute in lstAttributes)
                {
                    dtAttributes.Rows.Add(new object[] { attribute.BAGM_ID, (attribute.BAGM_DisplaySequence - 1), currentLoggedInUserId });
                }

                EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand _command = new SqlCommand("ams.UpdateBkgAttributeGroupMappingSequence", con);
                    _command.CommandType = CommandType.StoredProcedure;
                    _command.Parameters.AddWithValue("@typeParameter", dtAttributes);
                    con.Open();
                    _command.ExecuteNonQuery();
                    con.Close();
                }
                base.SecurityContext.SaveChanges();
            }
            return true;
        }

        public Boolean UpdateAttributeSequence(IList<MapServiceAttributeToGroupContract> attributesToMove, Int32? destinationIndex, Int32 currentLoggedInUserId)
        {
            DataTable dtAttributes = new DataTable();
            dtAttributes.Columns.Add("AttributeGroupMappingID", typeof(Int32));
            dtAttributes.Columns.Add("DestinationIndex", typeof(Int32));
            dtAttributes.Columns.Add("CurrentLoggedInUserId", typeof(Int32));
            foreach (MapServiceAttributeToGroupContract attribute in attributesToMove)
            {
                dtAttributes.Rows.Add(new object[] { attribute.AttributeGroupMappingID, destinationIndex, currentLoggedInUserId });
                destinationIndex += 1;
            }

            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.UpdateBkgAttributeGroupMappingSequence", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeParameter", dtAttributes);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    con.Close();
                    return true;
                }
            }
            return false;
        }
        public Boolean UpdateAttributeGrpMapping(Int32 attributeGrpMappingID, Int32 currentLoggedInUserId, Boolean IsRequired, Boolean IsDisplay, Int32? sourceAttributeId,Boolean IsHiddenFromUI)
        {
            Entity.BkgAttributeGroupMapping attributeGroupMappingInDb = getAttributeGroupMappingById(attributeGrpMappingID);
            if (attributeGroupMappingInDb.IsNotNull())
            {
                attributeGroupMappingInDb.BAGM_IsRequired = IsRequired;
                attributeGroupMappingInDb.BAGM_IsDisplay = IsDisplay;
                attributeGroupMappingInDb.BAGM_SourceAttributeID = sourceAttributeId;
                attributeGroupMappingInDb.BAGM_ModifiedOn = DateTime.Now;
                attributeGroupMappingInDb.BAGM_ModifiedBy = currentLoggedInUserId;
                attributeGroupMappingInDb.BAGM_IsHiddenFromUI = IsHiddenFromUI;

                base.SecurityContext.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region Manage Master Services
        /// <summary>
        /// Get all the service in master
        /// </summary>
        /// <returns></returns>
        public List<Entity.BackgroundService> GetMasterServices()
        {
            List<Entity.BackgroundService> masterServices = base.SecurityContext.BackgroundServices.Include("ApplicableServiceSettings").Include("lkpBkgSvcType").Where(x => x.BSE_IsDeleted == false).ToList();
            return masterServices;
        }

        /// <summary>
        /// Saves New Service.
        /// </summary>
        /// <param name="category">BackgroundService Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>BackgroundService Entity</returns>
        public Entity.BackgroundService SaveNewServiceDetail(Entity.BackgroundService masterService, Int32 currentLoggedInUserId)
        {
            masterService.BSE_CreatedById = currentLoggedInUserId;
            masterService.BSE_CreatedDate = DateTime.Now;
            masterService.BSE_IsDeleted = false;
            base.SecurityContext.BackgroundServices.AddObject(masterService);
            base.SecurityContext.SaveChanges();
            return masterService;

        }

        /// <summary>
        /// Checks if the service name already exists.
        /// </summary>
        /// <param name="svcGrpName">service Name</param>
        /// <param name="svcGrpID">Service Id</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfServiceNameAlreadyExist(String svcName, Int32 svcID)
        {
            return base.SecurityContext.BackgroundServices.Any(obj => obj.BSE_Name.Trim().ToUpper() == svcName.Trim().ToUpper() && obj.BSE_ID != svcID && obj.BSE_IsDeleted == false);
        }
        /// <summary>
        /// update Service
        /// </summary>
        /// <param name="masterService"></param>
        /// <param name="svcMasterID"></param>
        /// <param name="currentLoggedInUserId"></param>
        public void UpdateServiceDetail(Entity.BackgroundService masterService, Int32 svcMasterID, Int32 currentLoggedInUserId)
        {
            Entity.BackgroundService bkgService = base.SecurityContext.BackgroundServices.Include("ApplicableServiceSettings").FirstOrDefault(obj => obj.BSE_ID == svcMasterID && obj.BSE_IsDeleted == false);
            bkgService.BSE_Name = masterService.BSE_Name;

            //UAT-1728:Create ability to add cofigurable text to the result report (and flagged only and service group reports) by service. 
            bkgService.BSE_ConfigurableServiceText = masterService.BSE_ConfigurableServiceText;

            bkgService.BSE_Description = masterService.BSE_Description;
            bkgService.BSE_SvcTypeID = masterService.BSE_SvcTypeID;
            bkgService.BSE_ParentServiceID = masterService.BSE_ParentServiceID;
            bkgService.BSE_ModifiedBy = currentLoggedInUserId;
            bkgService.BSE_ModifiedDate = DateTime.Now;
            Entity.ApplicableServiceSetting applicableServiceSettingInDb = bkgService.ApplicableServiceSettings.FirstOrDefault();
            Entity.ApplicableServiceSetting applicableServiceSetting = masterService.ApplicableServiceSettings.FirstOrDefault();
            if (applicableServiceSettingInDb.IsNotNull())
            {
                applicableServiceSettingInDb.ASSE_ShowIgnoreResidentialHistory = applicableServiceSetting.ASSE_ShowIgnoreResidentialHistory;
                applicableServiceSettingInDb.ASSE_ShowIsSupplemental = applicableServiceSetting.ASSE_ShowIsSupplemental;
                applicableServiceSettingInDb.ASSE_ShowMaxOcuurence = applicableServiceSetting.ASSE_ShowMaxOcuurence;
                applicableServiceSettingInDb.ASSE_ShowMinOcuurence = applicableServiceSetting.ASSE_ShowMinOcuurence;
                applicableServiceSettingInDb.ASSE_ShowPackageCount = applicableServiceSetting.ASSE_ShowPackageCount;
                applicableServiceSettingInDb.ASSE_ShowResidenceYears = applicableServiceSetting.ASSE_ShowResidenceYears;
                applicableServiceSettingInDb.ASSE_ShowSendDocument = applicableServiceSetting.ASSE_ShowSendDocument;
                applicableServiceSettingInDb.ASSE_ModifiedBy = applicableServiceSetting.ASSE_ModifiedBy;
                applicableServiceSettingInDb.ASSE_ModifiedDate = applicableServiceSetting.ASSE_ModifiedDate;
            }
            else
            {
                Entity.ApplicableServiceSetting newApplicableServiceSetting = new Entity.ApplicableServiceSetting();
                newApplicableServiceSetting.ASSE_BackgroundServiceID = svcMasterID;
                newApplicableServiceSetting.ASSE_ShowIgnoreResidentialHistory = applicableServiceSetting.ASSE_ShowIgnoreResidentialHistory;
                newApplicableServiceSetting.ASSE_ShowIsSupplemental = applicableServiceSetting.ASSE_ShowIsSupplemental;
                newApplicableServiceSetting.ASSE_ShowMaxOcuurence = applicableServiceSetting.ASSE_ShowMaxOcuurence;
                newApplicableServiceSetting.ASSE_ShowMinOcuurence = applicableServiceSetting.ASSE_ShowMinOcuurence;
                newApplicableServiceSetting.ASSE_ShowPackageCount = applicableServiceSetting.ASSE_ShowPackageCount;
                newApplicableServiceSetting.ASSE_ShowResidenceYears = applicableServiceSetting.ASSE_ShowResidenceYears;
                newApplicableServiceSetting.ASSE_ShowSendDocument = applicableServiceSetting.ASSE_ShowSendDocument;
                newApplicableServiceSetting.ASSE_CreatedBy = currentLoggedInUserId;
                newApplicableServiceSetting.ASSE_CreatedDate = DateTime.Now;
                newApplicableServiceSetting.ASSE_IsDeleted = false;
                base.SecurityContext.ApplicableServiceSettings.AddObject(newApplicableServiceSetting);
            }
            //EntityCollection<Entity.ApplicableServiceSetting> applicableServiceSetting=bkgService.
            base.SecurityContext.SaveChanges();
        }

        public void DeletebackgroundService(Int32 bkgSvcMasterID, Int32 currentLoggedInUserId)
        {

            //_dbNavigation.BkgSvcAttributeGroupMappings.Where(x => x.BSAGM_ServiceId == bkgSvcMasterID).ForEach(x => { x.BSAGM_IsDeleted = true; x.BSAGM_ModifiedBy = 1; x.BSAGM_ModifiedOn = DateTime.Now; });

            Entity.BackgroundService masterBkgService = base.SecurityContext.BackgroundServices.Include("BkgSvcAttributeGroupMappings").Include("BkgSvcFormMappings").FirstOrDefault(x => x.BSE_ID == bkgSvcMasterID && !x.BSE_IsDeleted);
            if (masterBkgService.IsNotNull())
            {
                masterBkgService.BSE_IsDeleted = true;
                masterBkgService.BSE_ModifiedBy = currentLoggedInUserId;
                masterBkgService.BSE_ModifiedDate = DateTime.Now;
                foreach (var bkgServiceAttrMapping in masterBkgService.BkgSvcAttributeGroupMappings.Where(x => !x.BSAGM_IsDeleted))
                {
                    bkgServiceAttrMapping.BSAGM_IsDeleted = true;
                    bkgServiceAttrMapping.BSAGM_ModifiedBy = currentLoggedInUserId;
                    bkgServiceAttrMapping.BSAGM_ModifiedOn = DateTime.Now;
                }
                foreach (var bkgServiceFormMapping in masterBkgService.BkgSvcFormMappings.Where(x => !x.BSFM_IsDeleted))
                {
                    bkgServiceFormMapping.BSFM_IsDeleted = true;
                    bkgServiceFormMapping.BSFM_ModifiedBy = currentLoggedInUserId;
                    bkgServiceFormMapping.BSFM_ModifiedOn = DateTime.Now;
                }
                Entity.ApplicableServiceSetting applicableServiceSettingInDb = masterBkgService.ApplicableServiceSettings.FirstOrDefault();
                if (applicableServiceSettingInDb.IsNotNull())
                {
                    applicableServiceSettingInDb.ASSE_IsDeleted = true;
                    applicableServiceSettingInDb.ASSE_ModifiedBy = currentLoggedInUserId;
                    applicableServiceSettingInDb.ASSE_ModifiedDate = DateTime.Now;
                }
            }
            base.SecurityContext.SaveChanges();
        }
        public String BkgSrvName(Int32 bkgSvcMasterID)
        {
            Entity.BackgroundService bkgSvc = base.SecurityContext.BackgroundServices.FirstOrDefault(x => x.BSE_ID == bkgSvcMasterID && !x.BSE_IsDeleted);
            if (bkgSvc.IsNotNull())
            {
                return bkgSvc.BSE_Name;
            }
            return String.Empty;
        }

        #endregion

        #region Map Service to Attribute Group
        /// <summary>
        /// Get the records for Manage Attribute group Mapping Grid 
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public List<ManageServiceAttributeGrpContract> GetAttributeGrps(Int32 serviceId)
        {
            return base.SecurityContext.BkgSvcAttributeGroupMappings.Include("BkgAttributeGroupMapping.BkgSvcAttribute").Include("BkgAttributeGroupMapping.BkgSvcAttributeGroup")
                                    .Where(x => x.BSAGM_ServiceId == serviceId && x.BSAGM_IsDeleted == false && x.BkgAttributeGroupMapping.BkgSvcAttributeGroup.BSAD_IsDeleted == false && x.BkgAttributeGroupMapping.BkgSvcAttribute.BSA_IsDeleted == false)
                                    .Select(x => new ManageServiceAttributeGrpContract
                                    {
                                        ServiceAttGrpName = x.BkgAttributeGroupMapping.BkgSvcAttributeGroup.BSAD_Name,
                                        ServiceattGrpID = x.BkgAttributeGroupMapping.BkgSvcAttributeGroup.BSAD_ID,
                                        // ServiceAttGrpDesc = x.BkgAttributeGroupMapping.BkgSvcAttributeGroup.BSAD_Description,
                                        IsEditable = x.BSAGM_IsEditable,
                                        AttributeID = x.BkgAttributeGroupMapping.BkgSvcAttribute.BSA_ID,
                                        AttributeName = x.BkgAttributeGroupMapping.BkgSvcAttribute.BSA_Name,
                                    }).Distinct()
                                    .ToList();

        }
        //get all attribute grp list and exclude already mapped attribute group on the basis of boolean variable
        public List<Entity.BkgSvcAttributeGroup> GetAllAttributeGroups(Int32 serviceID, Boolean isupdate)
        {
            List<Entity.BkgSvcAttributeGroup> bkgAttrGrpList = null;
            bkgAttrGrpList = base.SecurityContext.BkgSvcAttributeGroups.Where(x => x.BSAD_IsDeleted == false).ToList();
            if (isupdate == false)
            {
                List<Int32> alreadtmappedbkgSvcAttrgrpLst = base.SecurityContext.BkgSvcAttributeGroupMappings.Include("BkgAttributeGroupMapping")
                                        .Where(x => x.BSAGM_ServiceId == serviceID && x.BSAGM_IsDeleted == false).Distinct().Select(x => x.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId).Distinct().ToList();

                if (alreadtmappedbkgSvcAttrgrpLst.Count > AppConsts.NONE)
                {
                    foreach (Int32 bkgAttrgrpmapping in alreadtmappedbkgSvcAttrgrpLst)
                    {
                        bkgAttrGrpList.Remove(bkgAttrGrpList.Where(x => x.BSAD_ID == bkgAttrgrpmapping).FirstOrDefault());
                    }
                }
            }
            return bkgAttrGrpList.OrderBy(con => con.BSAD_Name).ToList();
        }

        public List<Entity.BkgSvcAttribute> GetAllAttributes(Int32 attributegrpID)
        {
            List<Int32> mappedAttriIdstoAttributeGrp = base.SecurityContext.BkgAttributeGroupMappings.Where(x => x.BAGM_BkgSvcAttributeGroupId == attributegrpID && !x.BAGM_IsDeleted)
                                                            .Select(x => x.BAGM_BkgSvcAtributeID).ToList();
            return base.SecurityContext.BkgSvcAttributes.Where(x => x.BSA_IsDeleted == false && mappedAttriIdstoAttributeGrp.Contains(x.BSA_ID)).OrderBy(con => con.BSA_Name).ToList();
        }
        //return mapping ids of Attribute grp mapping table
        public List<Int32> GetAllAttributesMappingIDs(List<Int32> attributesIDs, Int32 attributegrpID)
        {
            List<Int32> _attributesGroupMappingIds = base.SecurityContext.BkgAttributeGroupMappings
                            .Where(x => x.BAGM_IsDeleted == false && x.BAGM_BkgSvcAttributeGroupId == attributegrpID && attributesIDs.Contains(x.BAGM_BkgSvcAtributeID))
                            .Select(x => x.BAGM_ID).ToList();
            return _attributesGroupMappingIds;
        }
        /// <summary>
        /// Save new Attributes with Service in Attribute group mapping grid 
        /// </summary>
        /// <param name="newSvcAttributeGrpMapping"></param>
        public void SaveAttributeGrpMappings(Entity.BkgSvcAttributeGroupMapping newSvcAttributeGrpMapping)
        {
            base.SecurityContext.BkgSvcAttributeGroupMappings.AddObject(newSvcAttributeGrpMapping);
            base.SecurityContext.SaveChanges();

        }

        public List<Int32> GetAllAttributeIDsRelatedToAttributeGrpID(Int32 attributegrpID, Int32 serviceId)
        {
            return GetAllAttrIdsmappedAttributegrpwithservice(attributegrpID, serviceId);

        }

        public void UpdateAtttributeMappingLst(Int32 attributegrpID, Int32 serviceId, Int32 currentLoggedInUserId, List<Int32> updatedattributeIdLst)
        {
            //previous saved iD corresponds to Attributegrp and service
            List<Int32> _previousAttrIdsList = GetAllAttrIdsmappedAttributegrpwithservice(attributegrpID, serviceId);
            //list of attribute id to be deleted on updation operation
            List<Int32> _previousAttrIdsMappingToDelete = (from td in _previousAttrIdsList
                                                           where !updatedattributeIdLst.Contains(td)
                                                           select td).ToList();
            if (_previousAttrIdsMappingToDelete.Count > 0)
            {
                //List of Attributegrpmapping table ids to de deleted
                List<Int32> _attriGrpmappingIdsToDelete = base.SecurityContext.BkgAttributeGroupMappings.Where(x => x.BAGM_IsDeleted == false && x.BAGM_BkgSvcAttributeGroupId == attributegrpID && _previousAttrIdsMappingToDelete.Contains(x.BAGM_BkgSvcAtributeID))
                    .Select(x => x.BAGM_ID).ToList();

                //to delete the unmapped mapping
                foreach (Int32 AttriGrpmappingIdsToDelete in _attriGrpmappingIdsToDelete)
                {
                    Entity.BkgSvcAttributeGroupMapping bkgSvcAtttrGrpMapping = base.SecurityContext.BkgSvcAttributeGroupMappings.FirstOrDefault(x => x.BSAGM_AttributeGroupMappingID == AttriGrpmappingIdsToDelete && x.BSAGM_IsDeleted == false && x.BSAGM_ServiceId == serviceId);
                    bkgSvcAtttrGrpMapping.BSAGM_IsDeleted = true;
                    bkgSvcAtttrGrpMapping.BSAGM_ModifiedOn = DateTime.Now;
                    bkgSvcAtttrGrpMapping.BSAGM_ModifiedBy = currentLoggedInUserId;
                }
            }

            //list of attribute id to be inserted in mapping table on updation operation
            List<Int32> _updatedAttrIdsMappingToInsert = (from td in updatedattributeIdLst
                                                          where !_previousAttrIdsList.Contains(td)
                                                          select td).ToList();
            if (_updatedAttrIdsMappingToInsert.Count > 0)
            {
                //List of Attributegrpmapping table ids to de Inserted
                //List<Int32> _attriGrpmappingIdsToInsert = _dbNavigation.BkgSvcAttributeGroupMappings.Include("BkgAttributeGroupMapping").Where(x => x.BkgAttributeGroupMapping.BAGM_IsDeleted == false && !_updatedAttrIdsMappingToInsert.Contains(x.BkgAttributeGroupMapping.BAGM_BkgSvcAtributeID))
                //    .Select(x => x.BkgAttributeGroupMapping.BAGM_ID).ToList();
                List<Int32> _attriGrpmappingIdsToInsert = base.SecurityContext.BkgAttributeGroupMappings.Where(x => x.BAGM_IsDeleted == false && x.BAGM_BkgSvcAttributeGroupId == attributegrpID && _updatedAttrIdsMappingToInsert.Contains(x.BAGM_BkgSvcAtributeID))
                    .Select(x => x.BAGM_ID).ToList();


                //to insert the new mapped mapping attributesIds
                foreach (Int32 AttriGrpmappingIdsToInsert in _attriGrpmappingIdsToInsert)
                {
                    Entity.BkgSvcAttributeGroupMapping bkgSvcAtttrGrpMapping = new Entity.BkgSvcAttributeGroupMapping();
                    bkgSvcAtttrGrpMapping.BSAGM_ServiceId = serviceId;
                    bkgSvcAtttrGrpMapping.BSAGM_IsEditable = true;
                    bkgSvcAtttrGrpMapping.BSAGM_IsSystemPreConfigured = false;
                    bkgSvcAtttrGrpMapping.BSAGM_AttributeGroupMappingID = AttriGrpmappingIdsToInsert;
                    bkgSvcAtttrGrpMapping.BSAGM_IsDeleted = false;
                    bkgSvcAtttrGrpMapping.BSAGM_CreatedOn = DateTime.Now;
                    bkgSvcAtttrGrpMapping.BSAGM_CreatedBy = currentLoggedInUserId;
                    bkgSvcAtttrGrpMapping.BSAGM_Code = Guid.NewGuid();
                    base.SecurityContext.BkgSvcAttributeGroupMappings.AddObject(bkgSvcAtttrGrpMapping);
                }
            }
            base.SecurityContext.SaveChanges();

        }

        private List<Int32> GetAllAttrIdsmappedAttributegrpwithservice(Int32 attributegrpID, Int32 serviceId)
        {
            List<Int32> _attributesIds = base.SecurityContext.BkgSvcAttributeGroupMappings.Include("BkgAttributeGroupMappings").Where(x => x.BSAGM_ServiceId == serviceId && x.BSAGM_IsDeleted == false
                   && x.BkgAttributeGroupMapping.BAGM_IsDeleted == false && x.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId == attributegrpID)
                   .Select(x => x.BkgAttributeGroupMapping.BAGM_BkgSvcAtributeID).ToList();
            //_ClientDBContext.BkgAttributeGroupMappings.Where(x => x.BAGM_BkgSvcAttributeGroupId == attributegrpID && x.BAGM_IsDeleted == false)
            //                                                .Select(x=>x.BAGM_BkgSvcAtributeID).ToList();
            return _attributesIds;
        }

        public void DeleteAttributeServiceMappingByAttributeId(Int32 attributegrpID, Int32 attributeId, Int32 serviceId, Int32 currentLoggedInUserId)
        {
            Entity.BkgSvcAttributeGroupMapping bkgSvcAttrGrpMapping = base.SecurityContext.BkgSvcAttributeGroupMappings.Include("BkgAttributeGroupMapping")
                              .FirstOrDefault(x => x.BkgAttributeGroupMapping.BAGM_BkgSvcAtributeID == attributeId && x.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId == attributegrpID
                                  && x.BkgAttributeGroupMapping.BAGM_IsDeleted == false && x.BSAGM_IsDeleted == false && x.BSAGM_ServiceId == serviceId);
            bkgSvcAttrGrpMapping.BSAGM_IsDeleted = true;
            bkgSvcAttrGrpMapping.BSAGM_ModifiedOn = DateTime.Now;
            bkgSvcAttrGrpMapping.BSAGM_ModifiedBy = currentLoggedInUserId;
            base.SecurityContext.SaveChanges();
        }

        public void DeleteAttributMappingwithServicebyAttributeGroupid(Int32 attributegrpID, Int32 serviceId, Int32 currentLoggedInUserId)
        {
            List<Int32> bKgSvcAttributegrpmappingids = base.SecurityContext.BkgSvcAttributeGroupMappings.Include("BkgAttributeGroupMapping")
                                                         .Where(x => x.BSAGM_ServiceId == serviceId && !x.BSAGM_IsDeleted && x.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId == attributegrpID && !x.BkgAttributeGroupMapping.BAGM_IsDeleted)
                                                         .Select(x => x.BSAGM_ID).ToList();
            if (bKgSvcAttributegrpmappingids.Count > 0)
            {
                foreach (Int32 bKgSvcAttributegrpmapping in bKgSvcAttributegrpmappingids)
                {
                    Entity.BkgSvcAttributeGroupMapping bkgMasterSvcAttrGrpMapping = base.SecurityContext.BkgSvcAttributeGroupMappings.Where(x => x.BSAGM_ID == bKgSvcAttributegrpmapping && !x.BSAGM_IsDeleted).FirstOrDefault();
                    bkgMasterSvcAttrGrpMapping.BSAGM_IsDeleted = true;
                    bkgMasterSvcAttrGrpMapping.BSAGM_ModifiedBy = currentLoggedInUserId;
                    bkgMasterSvcAttrGrpMapping.BSAGM_ModifiedOn = DateTime.Now;
                }
                base.SecurityContext.SaveChanges();
            }
        }


        #endregion

        #region Service CustomFormMapping

        public List<ManageServiceCustomFormContract> GetCustomFormsForService(Int32 serviceId)
        {
            return base.SecurityContext.BkgSvcFormMappings.Include("CustomForm").Where(x => x.BSFM_BackgroundServiceID == serviceId && x.BSFM_IsDeleted == false && x.CustomForm.CF_IsDeleted == false)
                 .Select(x => new ManageServiceCustomFormContract
                 {
                     SvcFormMappingID = x.BSFM_ID,
                     CustomFormID = x.CustomForm.CF_ID,
                     CustomFormName = x.CustomForm.CF_Name,
                     CustomFormDesc = x.CustomForm.CF_Description,
                     IsEditable = x.CustomForm.CF_IsEditable,
                     IsSystemPreConfigured = x.CustomForm.CF_IsSystemPreConfigured,
                     SequenceOrder = x.CustomForm.CF_Sequence,
                 }).ToList();
        }

        public List<Entity.CustomForm> GetAllCustomForm(Int32 serviceId)
        {
            List<Entity.CustomForm> customFormList = null;
            String _customFormtypeCode = CustomFormType.Supplement_Order_Form.GetStringValue();
            customFormList = base.SecurityContext.CustomForms.Include("lkpCustomFormType").Where(x => x.CF_IsDeleted == false && x.lkpCustomFormType.CFT_Code != _customFormtypeCode).ToList();
            List<Int32> alreadtmappedSvcCustomFormLst = base.SecurityContext.BkgSvcFormMappings.Where(x => x.BSFM_BackgroundServiceID == serviceId && !x.BSFM_IsDeleted).Select(x => x.BSFM_CustomFormId ?? 0).ToList();
            if (alreadtmappedSvcCustomFormLst.Count > AppConsts.NONE)
            {
                foreach (Int32 bkgCustomFormId in alreadtmappedSvcCustomFormLst)
                {
                    customFormList.Remove(customFormList.Where(x => x.CF_ID == bkgCustomFormId).FirstOrDefault());
                }
            }
            return customFormList.OrderBy(con => con.CF_Name).ToList();
        }

        public void SaveSvcFormMapping(Entity.BkgSvcFormMapping newSvcFormMapping)
        {
            base.SecurityContext.BkgSvcFormMappings.AddObject(newSvcFormMapping);
            base.SecurityContext.SaveChanges();
        }

        public void DeleteSvcFormMApping(Int32 svcFormMappingID, Int32 currentLoggedInUserId)
        {
            Entity.BkgSvcFormMapping bkgSvcFormmapping = base.SecurityContext.BkgSvcFormMappings.FirstOrDefault(x => x.BSFM_ID == svcFormMappingID && x.BSFM_IsDeleted == false);
            if (bkgSvcFormmapping.IsNotNull())
            {
                bkgSvcFormmapping.BSFM_IsDeleted = true;
                bkgSvcFormmapping.BSFM_ModifiedBy = currentLoggedInUserId;
                bkgSvcFormmapping.BSFM_ModifiedOn = DateTime.Now;
                base.SecurityContext.SaveChanges();
            }
        }

        #endregion

        #region Manage Custom Forms

        /// <summary>
        /// Returns all the CustomForms viewable to the current logged in user. 
        /// </summary>
        /// <returns>List of Custom Forms</returns>
        public List<Entity.CustomForm> GetAllCustomForms()
        {
            //SysXAppDBEntities.ClearContext();
            List<Entity.CustomForm> lstCustomForms = base.SecurityContext.CustomForms.Include("lkpCustomFormType").Where(obj => obj.CF_IsDeleted == false).OrderBy(obj => obj.CF_Sequence).ToList();
            base.SecurityContext.Refresh(RefreshMode.StoreWins, lstCustomForms);
            return lstCustomForms;

        }

        /// <summary>
        /// Saves the CustomForm from master DB.
        /// </summary>
        /// <param name="category">CustomForm Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>CustomForm Entity</returns>
        public Entity.CustomForm SaveCustomFormDetail(Entity.CustomForm customForm, Int32 currentLoggedInUserId)
        {
            customForm.CF_CreatedById = currentLoggedInUserId;
            customForm.CF_CreatedDate = DateTime.Now;
            customForm.CF_IsDeleted = false;
            // Set Sequence of new form
            Entity.CustomForm lastCustomFrm = base.SecurityContext.CustomForms.Where(obj => obj.CF_IsDeleted == false).OrderByDescending(i => i.CF_Sequence).FirstOrDefault();
            customForm.CF_Sequence = (lastCustomFrm.IsNotNull()) ? lastCustomFrm.CF_Sequence + 1 : 1;

            base.SecurityContext.CustomForms.AddObject(customForm);
            base.SecurityContext.SaveChanges();
            return customForm;
        }

        /// <summary>
        /// Checks if the custom Form Name already exists from master DB.
        /// </summary>
        /// <param name="customFormName">custom Form Name</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfCustomFormNameAlreadyExist(String customFormName)
        {
            return base.SecurityContext.CustomForms.Any(obj => obj.CF_Name.Trim().ToUpper() == customFormName.Trim().ToUpper() && obj.CF_IsDeleted == false);
        }

        /// <summary>
        /// Updates the custom Form from master DB.
        /// </summary>
        /// <param name="customForm">CustomForm Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean UpdateCustomFormDetail(Entity.CustomForm customForm, Int32 customFormID, Int32 currentLoggedInUserId)
        {
            Entity.CustomForm customFrm = GetCurrentCustomFormInfo(customFormID);
            if (customFrm.IsNull())
                return false;

            customFrm.CF_Name = customForm.CF_Name;
            customFrm.CF_Description = customForm.CF_Description;
            customFrm.CF_Title = customForm.CF_Title;
            customFrm.CF_ModifiedBy = currentLoggedInUserId;
            customFrm.CF_ModifiedDate = DateTime.Now;
            customFrm.CF_CustomFormTypeID = customForm.CF_CustomFormTypeID;
            base.SecurityContext.SaveChanges();
            return true;

        }

        /// <summary>
        /// Gets specific custom Form from master DB.
        /// </summary>
        /// <param name="customFormId">CustomFormID</param>
        public Entity.CustomForm GetCurrentCustomFormInfo(Int32 customFormID)
        {
            return base.SecurityContext.CustomForms.Where(obj => obj.CF_ID == customFormID && obj.CF_IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// Deletes the custom Form from master DB.
        /// </summary>
        /// <param name="customFormId">CustomFormID</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean DeleteCustomForm(Int32 customFormID, Int32 currentUserId)
        {
            // Set Delete true first in CustomFormAttributeGroup table for Custom Form ID
            Boolean result = true;

            List<Entity.CustomFormAttributeGroup> customFormAttrGrpMappings = base.SecurityContext.CustomFormAttributeGroups.Where(obj => obj.CFAG_CustomFormId == customFormID && obj.CFAG_IsDeleted == false).ToList();
            if (customFormAttrGrpMappings != null && customFormAttrGrpMappings.Count() > 0)
            {
                List<Boolean> allMappingResult = new List<Boolean>();
                foreach (var mapping in customFormAttrGrpMappings)
                {
                    allMappingResult.Add(DeleteCustomFormAttributeGroup(mapping.CFAG_ID, currentUserId));
                }
                //customFormAttrGrpMappings.ForEach(obj => obj.CFAG_IsDeleted = true);
                result = allMappingResult.Any(x => x == false) ? false : true;
            }
            if (result)
            {
                // Update Custom Form Table Set Delete true
                Entity.CustomForm customFrm = GetCurrentCustomFormInfo(customFormID);
                int customFrmSequence = customFrm.CF_Sequence;
                customFrm.CF_IsDeleted = true;
                customFrm.CF_ModifiedBy = currentUserId;
                customFrm.CF_ModifiedDate = DateTime.Now;
                customFrm.CF_Sequence = 0;

                //Re-Order Sequence of the Records.
                List<Entity.CustomForm> lstCustoms = base.SecurityContext.CustomForms.Where(obj => obj.CF_IsDeleted == false &&
                     obj.CF_Sequence > customFrmSequence).OrderBy(obj => obj.CF_Sequence).ToList();
                UpdateCustomFormSequence(lstCustoms, customFrmSequence, currentUserId);
                //lstCustoms.ForEach(obj => obj.CF_Sequence -= 1);
                base.SecurityContext.SaveChanges();


            }
            return result;
        }

        /// <summary>
        ///Updates Custom Form Sequence.
        /// </summary>
        /// <param name="customFormsToMove">IList of CustomForm Entity</param>
        /// <param name="destinationIndex">Index</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean UpdateCustomFormSequence(IList<Entity.CustomForm> customFormsToMove, Int32 destinationIndex, Int32 currentLoggedInUserId)
        {
            DataTable CustomFormList = new DataTable();
            CustomFormList.Columns.Add("CF_ID", typeof(Int32));
            CustomFormList.Columns.Add("CF_Sequence", typeof(Int32));
            CustomFormList.Columns.Add("CF_ModifiedBy", typeof(Int32));

            foreach (Entity.CustomForm customFrm in customFormsToMove)
            {
                CustomFormList.Rows.Add(new object[] { customFrm.CF_ID, destinationIndex, currentLoggedInUserId });
                destinationIndex += 1;
            }

            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_UpdateCustomFormSequence", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeParameter", CustomFormList);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = _command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return true;
            }
        }


        /// <summary>
        /// Returns all the CustomFormAttributeGroup for specific CustomForm ID from master DB. 
        /// </summary>
        /// <returns>List of CustomFormAttributeGroups</returns>
        public List<Entity.CustomFormAttributeGroup> GetCustomFormAttrGrpsByCustomFormId(Int32 customFormId)
        {
            List<Entity.CustomFormAttributeGroup> lstCustomFormAttributeGroups = base.SecurityContext.CustomFormAttributeGroups.Where(obj => obj.CFAG_IsDeleted == false && obj.CFAG_CustomFormId == customFormId).OrderBy(obj => obj.CFAG_Sequence).ToList();
            base.SecurityContext.Refresh(RefreshMode.StoreWins, lstCustomFormAttributeGroups);
            return lstCustomFormAttributeGroups;
        }

        /// <summary>
        /// Saves the CustomFormAttributeGroup from master DB.
        /// </summary>
        /// <param name="category">CustomFormAttributeGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>CustomFormAttributeGroup Entity</returns>
        public Entity.CustomFormAttributeGroup SaveCustomFormAttributeGroupDetail(Entity.CustomFormAttributeGroup customFormAttrGrp, Int32 currentLoggedInUserId)
        {
            customFormAttrGrp.CFAG_CreatedById = currentLoggedInUserId;
            customFormAttrGrp.CFAG_CreatedDate = DateTime.Now;
            customFormAttrGrp.CFAG_IsDeleted = false;
            // Set Sequence of new form
            Entity.CustomFormAttributeGroup lastCustomFrm = base.SecurityContext.CustomFormAttributeGroups.Where(obj => obj.CFAG_CustomFormId == customFormAttrGrp.CFAG_CustomFormId && obj.CFAG_IsDeleted == false).OrderByDescending(i => i.CFAG_Sequence).FirstOrDefault();
            customFormAttrGrp.CFAG_Sequence = (lastCustomFrm.IsNotNull()) ? lastCustomFrm.CFAG_Sequence + 1 : 1;

            base.SecurityContext.CustomFormAttributeGroups.AddObject(customFormAttrGrp);
            base.SecurityContext.SaveChanges();
            return customFormAttrGrp;
        }

        /// <summary>
        /// Checks if the CustomFormAttributeGroup already exists from master DB.
        /// </summary>
        /// <param name="customFormId">customFormId</param>
        /// <param name="attrGrpId">AttributeGroupId</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfCustomFormAttrGrpMappingAlreadyExist(Int32 customFormId, Int32 attrGrpId)
        {
            return base.SecurityContext.CustomFormAttributeGroups.Any(obj => obj.CFAG_CustomFormId == customFormId && obj.CFAG_BkgSvcAttributeGroupId == attrGrpId && obj.CFAG_IsDeleted == false);
        }

        /// <summary>
        /// Updates the CustomFormAttributeGroup from master DB.
        /// </summary>
        /// <param name="customFormAttributeGroup">CustomFormAttributeGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean UpdateCustomFormAttributeGroupDetail(Entity.CustomFormAttributeGroup customFormAttributeGroup, Int32 customFormAttributeGroupID, Int32 currentLoggedInUserId)
        {
            Entity.CustomFormAttributeGroup customFrmAttrGrp = GetCurrentCustomFormAttributeGroup(customFormAttributeGroupID);
            if (customFormAttributeGroup == null)
                return false;
            customFrmAttrGrp.CFAG_BkgSvcAttributeGroupId = customFormAttributeGroup.CFAG_BkgSvcAttributeGroupId;
            customFrmAttrGrp.CFAG_SectionTitle = customFormAttributeGroup.CFAG_SectionTitle;
            customFrmAttrGrp.CFAG_DisplayColumn = customFormAttributeGroup.CFAG_DisplayColumn;
            customFrmAttrGrp.CFAG_CustomHTML = customFormAttributeGroup.CFAG_CustomHTML;
            customFrmAttrGrp.CFAG_Occurrence = customFormAttributeGroup.CFAG_Occurrence;
            customFrmAttrGrp.CFAG_Sequence = customFormAttributeGroup.CFAG_Sequence == 0 ? customFrmAttrGrp.CFAG_Sequence : customFormAttributeGroup.CFAG_Sequence;
            customFrmAttrGrp.CFAG_ModifiedBy = currentLoggedInUserId;
            customFrmAttrGrp.CFAG_ModifiedDate = DateTime.Now;
            base.SecurityContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Gets specific Entity.CustomFormAttributeGroup from master DB.
        /// </summary>
        /// <param name="customFormAttributeGroupID">customFormAttributeGroupID</param>
        public Entity.CustomFormAttributeGroup GetCurrentCustomFormAttributeGroup(Int32 customFormAttributeGroupID)
        {
            return base.SecurityContext.CustomFormAttributeGroups.Where(obj => obj.CFAG_ID == customFormAttributeGroupID && obj.CFAG_IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// Deletes the CustomFormAttributeGroup from master DB.
        /// </summary>
        /// <param name="customFormAttributeGroupID">customFormAttributeGroupID</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean DeleteCustomFormAttributeGroup(Int32 customFormAttributeGroupID, Int32 currentUserId)
        {
            Entity.CustomFormAttributeGroup customFrmAttributeGroup = GetCurrentCustomFormAttributeGroup(customFormAttributeGroupID);
            if (customFrmAttributeGroup == null)
                return false;

            int customFormAttributeGroupSequence = customFrmAttributeGroup.CFAG_Sequence;
            //lstCustomFrmAttrGroups.ForEach(obj => obj.CFAG_Sequence -= 1);
            customFrmAttributeGroup.CFAG_IsDeleted = true;
            customFrmAttributeGroup.CFAG_ModifiedBy = currentUserId;
            customFrmAttributeGroup.CFAG_ModifiedDate = DateTime.Now;

            //Re-Order Sequence of the Records.
            IList<Entity.CustomFormAttributeGroup> lstCustomFrmAttrGroups = base.SecurityContext.CustomFormAttributeGroups.Where(obj => obj.CFAG_IsDeleted == false &&
                obj.CFAG_CustomFormId == customFrmAttributeGroup.CFAG_CustomFormId && obj.CFAG_Sequence > customFormAttributeGroupSequence).OrderBy(obj => obj.CFAG_Sequence).ToList();

            UpdateCustomFormAttributeGroupSequence(lstCustomFrmAttrGroups, customFormAttributeGroupSequence, currentUserId);
            //lstCustomFrmAttrGroups.ForEach(obj => obj.CFAG_Sequence -= 1);
            base.SecurityContext.SaveChanges();



            return true;
        }

        /// <summary>
        ///Updates Custom Form Attribute Group Mapping Sequence.
        /// </summary>
        /// <param name="customFormsToMove">IList of CustomFormAttributeGroup Entity</param>
        /// <param name="destinationIndex">Index</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean UpdateCustomFormAttributeGroupSequence(IList<Entity.CustomFormAttributeGroup> customFormAttributeGroupsToMove, Int32 destinationIndex, Int32 currentLoggedInUserId)
        {
            DataTable CustomFormList = new DataTable();
            CustomFormList.Columns.Add("CF_ID", typeof(Int32));
            CustomFormList.Columns.Add("CF_Sequence", typeof(Int32));
            CustomFormList.Columns.Add("CF_ModifiedBy", typeof(Int32));

            foreach (Entity.CustomFormAttributeGroup customFrm in customFormAttributeGroupsToMove)
            {
                CustomFormList.Rows.Add(new object[] { customFrm.CFAG_ID, destinationIndex, currentLoggedInUserId });
                destinationIndex += 1;
            }

            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_UpdateCustomFormAttributeGrpSequence", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeParameter", CustomFormList);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = _command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return true;
            }
        }

        /// <summary>
        /// Returns all the BkgSvcAttributeGroup from master DB. 
        /// </summary>
        /// <returns>List of BkgSvcAttributeGroups</returns>
        public List<Entity.BkgSvcAttributeGroup> GetAllBkgSvcAttributeGroup()
        {
            List<Entity.BkgSvcAttributeGroup> lstBkgSvcAttributeGroups = base.SecurityContext.BkgSvcAttributeGroups.Where(obj => obj.BSAD_IsDeleted == false).ToList();
            return lstBkgSvcAttributeGroups;
        }
        public Guid GetCodeForCurrentAttributeGroup(Int32? attributeGrpID)
        {
            return base.SecurityContext.BkgSvcAttributeGroups.Where(x => x.BSAD_ID == attributeGrpID && !x.BSAD_IsDeleted).Select(x => x.BSAD_Code).FirstOrDefault();
        }

        #endregion

        #region Background Packages

        /// <summary>
        /// Get the list of All the background packages, which have not been purchased by Applicant, on a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <param name="orgainizatuionUserId"></param>
        /// <returns></returns>
        public DataTable GetBackgroundPackages(String xmlDPMIds, Int32 orgainizatuionUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("usp_GetAvailableBackgroundPackages", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@OrgainizatuionUserId", orgainizatuionUserId);
                _command.Parameters.AddWithValue("@DPMIds", xmlDPMIds);
                SqlDataAdapter _adp = new SqlDataAdapter();
                _adp.SelectCommand = _command;
                DataSet _ds = new DataSet();
                _adp.Fill(_ds);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
            }

            return new DataTable();
        }

        #endregion

        #region Map Master Services to Client
        #region Background Services

        public List<MapServicesToClientContract> GetBackgroundServices(Int32? SvcID = null, String SvcName = null, String ExtCode = null)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetBackgroundServices]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BSE_ID", SvcID);
                command.Parameters.AddWithValue("@SvcName", SvcName);
                command.Parameters.AddWithValue("@ExtCode", ExtCode);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<MapServicesToClientContract> lstMapServicesToClientContract = new List<MapServicesToClientContract>();
                lstMapServicesToClientContract = ds.Tables[0].AsEnumerable().Select(col =>
                      new MapServicesToClientContract
                      {
                          BSE_ID = col["BSE_ID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["BSE_ID"]),
                          BSE_Name = col["BSE_Name"] == DBNull.Value ? String.Empty : Convert.ToString(col["BSE_Name"]),
                          EBS_ExternalCode = col["EBS_ExternalCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["EBS_ExternalCode"])
                      }).ToList();

                return lstMapServicesToClientContract;
            }
            // return _ClientDBContext.BackgroundServices.Where(x => x.BSE_ParentServiceID == null && x.BSE_IsDeleted == false).ToList();
        }
        #endregion

        #region Map Services to Clients
        public Boolean MapServicesToClient(String SelectedServices, Int32 SelectedTenantId)
        {
            String[] temp = SelectedServices.Split(',');
            Int32[] SelectedServicesList = Array.ConvertAll(temp, int.Parse);
            List<BackgroundService> backgroundSvc = _ClientDBContext.BackgroundServices.Where(x => x.BSE_IsDeleted == true && SelectedServicesList.Contains(x.BSE_ID)).ToList();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_CopyMasterServiceDataToClient", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@ServiceIDs", SelectedServices.ToString());
                _command.Parameters.AddWithValue("@TenantID", SelectedTenantId);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    foreach (BackgroundService item in backgroundSvc)
                    {
                        if (item.IsNotNull())
                        {
                            item.BSE_IsDeleted = false;
                        }
                    }
                    _ClientDBContext.SaveChanges();
                    return true;
                }
                con.Close();
            }
            return false;
        }
        #endregion

        #region Existing Background Services

        public Int32[] GetExistingBackgroundServices()
        {
            return _ClientDBContext.BackgroundServices.Where(x => x.BSE_ParentServiceID == null && x.BSE_IsDeleted == false).Select(x => x.BSE_ID).ToArray();
        }
        #endregion

        #region Deactivate Mapping

        public Boolean DeactivateMapping(Int32 SelectedServicesId, Int32 selectedTenantID)
        {
            BackgroundService backgroundSvc = _ClientDBContext.BackgroundServices.FirstOrDefault(x => x.BSE_ID == SelectedServicesId);
            Entity.BkgSvcExtSvcMapping bkgSvcExtSvcMapping = base.SecurityContext.BkgSvcExtSvcMappings.FirstOrDefault(x => x.BSESM_BkgSvcId == SelectedServicesId && x.BSESM_IsDeleted == false);
            if (bkgSvcExtSvcMapping.IsNotNull())
            {
                List<Entity.ClientExtSvcVendorMapping> lstClientExtSvcVendorMapping = base.SecurityContext.ClientExtSvcVendorMappings.Where(x => x.CESVM_BkgSvcExtSvcMappingID == bkgSvcExtSvcMapping.BSESM_ID && x.CESVM_TenantID == selectedTenantID && x.CESVM_IsDeleted == false).ToList();
                if (lstClientExtSvcVendorMapping.IsNotNull())
                {
                    foreach (Entity.ClientExtSvcVendorMapping item in lstClientExtSvcVendorMapping)
                    {
                        item.CESVM_IsDeleted = true;
                    }
                }
                base.SecurityContext.SaveChanges();
            }

            if (backgroundSvc.IsNotNull())
            {
                backgroundSvc.BSE_IsDeleted = true;

                if (_ClientDBContext.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Update Client Count
        /// <summary>
        /// Update Client Count in Master Background Service
        /// </summary>
        /// <param name="SelectedServicesList"></param>
        /// <returns></returns>
        public Boolean UpdateClientCount(String SelectedServices, Boolean isInMappingMode)
        {
            String[] temp = SelectedServices.Split(',');
            Int32[] SelectedServicesList = Array.ConvertAll(temp, int.Parse);
            List<BackgroundService> backgroundSvc = _ClientDBContext.BackgroundServices.Where(x => SelectedServicesList.Contains(x.BSE_ID)).ToList();
            if (isInMappingMode)
            {
                foreach (BackgroundService item in backgroundSvc)
                {
                    if (item.BSE_ClientCount.HasValue)
                    {
                        item.BSE_ClientCount = item.BSE_ClientCount + 1;
                    }
                    else
                    {
                        item.BSE_ClientCount = 1;
                    }
                }
                _ClientDBContext.SaveChanges();
            }
            else
            {
                foreach (BackgroundService item in backgroundSvc)
                {
                    if (item.BSE_ClientCount.HasValue)
                    {
                        item.BSE_ClientCount = item.BSE_ClientCount - 1;
                    }
                    else
                    {
                        item.BSE_ClientCount = null;
                    }

                }
                _ClientDBContext.SaveChanges();
            }
            return true;
        }
        #endregion
        #endregion

        #region Derived From Services

        List<Entity.BackgroundService> IBackgroundSetupRepository.GetDerivedFromServiceList(Int32? currentServiceId)
        {
            if (currentServiceId.IsNull())
            {
                return base.SecurityContext.BackgroundServices.Where(cond => !cond.BSE_IsDeleted && cond.BSE_ParentServiceID == null).OrderBy(con => con.BSE_Name).ToList();
            }
            else
            {
                return base.SecurityContext.BackgroundServices.Where(cond => !cond.BSE_IsDeleted && cond.BSE_ID != currentServiceId && cond.BSE_ParentServiceID == null).OrderBy(con => con.BSE_Name).ToList();
            }
        }

        Boolean IBackgroundSetupRepository.IsChildServiceExist(Int32 currentServiceId)
        {
            return base.SecurityContext.BackgroundServices.Any(cond => cond.BSE_ParentServiceID == currentServiceId && !cond.BSE_IsDeleted);

        }
        #endregion

        #region Manage background Setup Attribute
        public Entity.BkgSvcAttribute SaveAttributeInMaster(Entity.ClientEntity.BkgSvcAttribute bkgSvcAttribute, Int32 attributeGroupId, Int32 currentLoggedInUserId, Boolean isRequired, Boolean isDisplay, Int32 tenantId,Boolean IsHiddenFromUI)
        {
            Int32? maxDisplaySequence = base.SecurityContext.BkgAttributeGroupMappings.Where(cond => cond.BAGM_BkgSvcAttributeGroupId == attributeGroupId && cond.BAGM_IsDeleted == false).Max(X => (Int32?)X.BAGM_DisplaySequence);
            if (maxDisplaySequence.IsNull())
                maxDisplaySequence = 1;
            else
                maxDisplaySequence = maxDisplaySequence + 1;
            //Assign Client entity BkgSvcAttribute to Security entity
            Entity.BkgSvcAttribute bkgSvcAttributeMaster = new Entity.BkgSvcAttribute();
            bkgSvcAttributeMaster.BSA_Name = bkgSvcAttribute.BSA_Name;
            bkgSvcAttributeMaster.BSA_Description = bkgSvcAttribute.BSA_Description;
            bkgSvcAttributeMaster.BSA_IsDeleted = bkgSvcAttribute.BSA_IsDeleted;
            bkgSvcAttributeMaster.BSA_Active = bkgSvcAttribute.BSA_Active;
            bkgSvcAttributeMaster.BSA_Code = bkgSvcAttribute.BSA_Code;
            bkgSvcAttributeMaster.BSA_CopiedFromCode = bkgSvcAttribute.BSA_CopiedFromCode;
            bkgSvcAttributeMaster.BSA_CreatedById = currentLoggedInUserId;
            bkgSvcAttributeMaster.BSA_CreatedDate = DateTime.Now;
            bkgSvcAttributeMaster.BSA_DataTypeID = bkgSvcAttribute.BSA_DataTypeID;
            bkgSvcAttributeMaster.BSA_IsEditable = bkgSvcAttribute.BSA_IsEditable;
            //bkgSvcAttributeMaster.BSA_IsRequired = bkgSvcAttribute.BSA_IsRequired;
            bkgSvcAttributeMaster.BSA_Label = bkgSvcAttribute.BSA_Label;
            bkgSvcAttributeMaster.BSA_IsSystemPreConfiguredq = bkgSvcAttribute.BSA_IsSystemPreConfiguredq;
            bkgSvcAttributeMaster.BSA_MaxDateValue = bkgSvcAttribute.BSA_MaxDateValue;
            bkgSvcAttributeMaster.BSA_MaxIntValue = bkgSvcAttribute.BSA_MaxIntValue;
            bkgSvcAttributeMaster.BSA_MaxLength = bkgSvcAttribute.BSA_MaxLength;
            bkgSvcAttributeMaster.BSA_MinDateValue = bkgSvcAttribute.BSA_MinDateValue;
            bkgSvcAttributeMaster.BSA_MinIntValue = bkgSvcAttribute.BSA_MinIntValue;
            bkgSvcAttributeMaster.BSA_MinLength = bkgSvcAttribute.BSA_MinLength;
            // bkgSvcAttributeMaster.BSA_ReqValidationMessage = bkgSvcAttribute.BSA_ReqValidationMessage;

            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> listAttributeOptionMaster = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption>();
            //Assign Client entity AttributeOptions to Security entity
            foreach (Entity.ClientEntity.BkgSvcAttributeOption clientOptionData in bkgSvcAttribute.BkgSvcAttributeOptions)
            {
                Entity.BkgSvcAttributeOption masterAttrOption = new Entity.BkgSvcAttributeOption();
                masterAttrOption.EBSAO_IsActive = clientOptionData.EBSAO_IsActive;
                masterAttrOption.EBSAO_IsDeleted = clientOptionData.EBSAO_IsDeleted;
                masterAttrOption.EBSAO_OptionText = clientOptionData.EBSAO_OptionText;
                masterAttrOption.EBSAO_OptionValue = clientOptionData.EBSAO_OptionValue;
                masterAttrOption.EBSAO_CreatedByID = currentLoggedInUserId;
                masterAttrOption.EBSAO_CreatedOn = DateTime.Now;
                listAttributeOptionMaster.Add(masterAttrOption);
            }
            bkgSvcAttributeMaster.BkgSvcAttributeOptions = listAttributeOptionMaster;

            Entity.BkgAttributeGroupMapping bkgAttributeGroupMapping = new Entity.BkgAttributeGroupMapping();
            bkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId = attributeGroupId;
            bkgAttributeGroupMapping.BAGM_Code = Guid.NewGuid();
            bkgAttributeGroupMapping.BAGM_CopiedFromCode = null;
            bkgAttributeGroupMapping.BAGM_CreatedOn = DateTime.Now;
            bkgAttributeGroupMapping.BAGM_CreatedBy = currentLoggedInUserId;
            bkgAttributeGroupMapping.BAGM_IsDeleted = false;
            bkgAttributeGroupMapping.BAGM_IsEditable = true;
            bkgAttributeGroupMapping.BAGM_IsRequired = isRequired;
            bkgAttributeGroupMapping.BAGM_IsDisplay = isDisplay;
            bkgAttributeGroupMapping.BAGM_IsHiddenFromUI = IsHiddenFromUI;
            bkgAttributeGroupMapping.BAGM_IsSystemPreConfigured = false;
            bkgAttributeGroupMapping.BAGM_DisplaySequence = maxDisplaySequence;
            bkgSvcAttributeMaster.BkgAttributeGroupMappings.Add(bkgAttributeGroupMapping);
            base.SecurityContext.BkgSvcAttributes.AddObject(bkgSvcAttributeMaster);
            base.SecurityContext.SaveChanges();
            Entity.BkgSvcAttributeTenant bkgSvcAttributeTenant = new Entity.BkgSvcAttributeTenant();
            bkgSvcAttributeTenant.BSAT_BkgSvcAttributeID = bkgSvcAttributeMaster.BSA_ID;
            bkgSvcAttributeTenant.BSAT_TenantID = tenantId;
            bkgSvcAttributeTenant.BSAT_IsDeleted = false;
            bkgSvcAttributeTenant.BSAT_CreatedByID = bkgSvcAttributeMaster.BSA_CreatedById;
            bkgSvcAttributeTenant.BSAT_CreatedOn = DateTime.Now;
            base.SecurityContext.BkgSvcAttributeTenants.AddObject(bkgSvcAttributeTenant);
            base.SecurityContext.SaveChanges();

            return bkgSvcAttributeMaster;
        }
        public Entity.BkgAttributeGroupMapping SaveAttributeAndGroupMappingInMaster(Entity.ClientEntity.BkgAttributeGroupMapping attributeMappingToAdd, Boolean isRequired, Boolean isDisplay)
        {
            List<Entity.BkgAttributeGroupMapping> bkgAttributeGrpMappingList = new List<Entity.BkgAttributeGroupMapping>();
            bkgAttributeGrpMappingList = base.SecurityContext.BkgAttributeGroupMappings.Where(cond => cond.BAGM_BkgSvcAttributeGroupId == attributeMappingToAdd.BAGM_BkgSvcAttributeGroupId && cond.BAGM_IsDeleted == false).ToList();
            Int32? maxDisplaySequence = bkgAttributeGrpMappingList.Max(X => (Int32?)X.BAGM_DisplaySequence);
            if (maxDisplaySequence.IsNull())
                maxDisplaySequence = 1;
            else
                maxDisplaySequence = maxDisplaySequence + 1;
            Entity.BkgAttributeGroupMapping masterAttributeGroupMapping = bkgAttributeGrpMappingList.FirstOrDefault(cond => cond.BAGM_BkgSvcAtributeID == attributeMappingToAdd.BAGM_BkgSvcAtributeID);
            if (masterAttributeGroupMapping.IsNullOrEmpty())
            {
                masterAttributeGroupMapping = new Entity.BkgAttributeGroupMapping();
                masterAttributeGroupMapping.BAGM_BkgSvcAtributeID = attributeMappingToAdd.BAGM_BkgSvcAtributeID;
                masterAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId = attributeMappingToAdd.BAGM_BkgSvcAttributeGroupId;
                masterAttributeGroupMapping.BAGM_Code = attributeMappingToAdd.BAGM_Code;
                masterAttributeGroupMapping.BAGM_CopiedFromCode = attributeMappingToAdd.BAGM_CopiedFromCode;
                masterAttributeGroupMapping.BAGM_CreatedBy = attributeMappingToAdd.BAGM_CreatedBy;
                masterAttributeGroupMapping.BAGM_CreatedOn = DateTime.Now;
                masterAttributeGroupMapping.BAGM_DisplaySequence = maxDisplaySequence;
                masterAttributeGroupMapping.BAGM_IsDeleted = attributeMappingToAdd.BAGM_IsDeleted;
                masterAttributeGroupMapping.BAGM_IsDisplay = isDisplay;
                masterAttributeGroupMapping.BAGM_IsEditable = attributeMappingToAdd.BAGM_IsEditable;
                masterAttributeGroupMapping.BAGM_IsRequired = isRequired;
                masterAttributeGroupMapping.BAGM_IsSystemPreConfigured = attributeMappingToAdd.BAGM_IsSystemPreConfigured;
                //masterListAttributeMapping.Add(masterAttributeGroupMapping);
                base.SecurityContext.BkgAttributeGroupMappings.AddObject(masterAttributeGroupMapping);
                base.SecurityContext.SaveChanges();
            }
            return masterAttributeGroupMapping;
        }

        public System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> AddDeleteServiceAttributeOpt(List<Int32> attributeOptionIdsToDelete, List<Entity.ClientEntity.BkgSvcAttributeOption> attributeOptToAdd, Int32 currentLoggedInUserId, Int32 bkgSvcAttributeId)
        {
            List<Entity.BkgSvcAttributeOption> AttributeOptionToDelete = base.SecurityContext.BkgSvcAttributeOptions.Where(cond => attributeOptionIdsToDelete.Contains(cond.EBSAO_ID)).ToList();
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> listAttributeOptionMaster = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption>();
            foreach (Entity.BkgSvcAttributeOption attributeOptToDelete in AttributeOptionToDelete)
            {
                attributeOptToDelete.EBSAO_IsDeleted = true;
                attributeOptToDelete.EBSAO_ModifiedOn = DateTime.Now;
                attributeOptToDelete.EBSAO_ModifiedByID = currentLoggedInUserId;
            }

            foreach (Entity.ClientEntity.BkgSvcAttributeOption attOptionToAdd in attributeOptToAdd)
            {
                Entity.BkgSvcAttributeOption BkgSvcAttOption = new Entity.BkgSvcAttributeOption();
                BkgSvcAttOption.EBSAO_BkgSvcAttributeID = bkgSvcAttributeId;
                BkgSvcAttOption.EBSAO_CreatedByID = currentLoggedInUserId;
                BkgSvcAttOption.EBSAO_CreatedOn = DateTime.Now;
                BkgSvcAttOption.EBSAO_IsActive = attOptionToAdd.EBSAO_IsActive;
                BkgSvcAttOption.EBSAO_IsDeleted = false;
                BkgSvcAttOption.EBSAO_OptionText = attOptionToAdd.EBSAO_OptionText;
                BkgSvcAttOption.EBSAO_OptionValue = attOptionToAdd.EBSAO_OptionValue;
                listAttributeOptionMaster.Add(BkgSvcAttOption);
                base.SecurityContext.BkgSvcAttributeOptions.AddObject(BkgSvcAttOption);
            }
            base.SecurityContext.SaveChanges();
            return listAttributeOptionMaster;
        }
        #endregion

        #region Client Service Vendor
        /// <summary>
        /// bind the client service grid 
        /// </summary>
        /// <param name="SelectedTenantId"></param>
        /// <returns></returns>
        public List<ClientServiceVendorContract> GetMappedBkgSvcExtSvcToState(Int32 SelectedTenantId)
        {
            return base.SecurityContext.ClientExtSvcVendorMappings.Include("States").Include("BkgSvcExtSvcMappings.BackgroundServices")
                                  .Include("BkgSvcExtSvcMappings.ExternalBkgSvcs").Where(x => x.CESVM_TenantID == SelectedTenantId && !x.CESVM_IsDeleted && !x.BkgSvcExtSvcMapping.BSESM_IsDeleted
                                  && !x.BkgSvcExtSvcMapping.BackgroundService.BSE_IsDeleted && !x.BkgSvcExtSvcMapping.ExternalBkgSvc.EBS_IsDeleted)
                                  .Select(x => new ClientServiceVendorContract
                                  {

                                      BkgServiceName = x.BkgSvcExtSvcMapping.BackgroundService.BSE_Name,
                                      BkgServiceID = x.BkgSvcExtSvcMapping.BackgroundService.BSE_ID,
                                      ExtServiceName = x.BkgSvcExtSvcMapping.ExternalBkgSvc.EBS_Name,
                                      ExtServiceID = x.BkgSvcExtSvcMapping.ExternalBkgSvc.EBS_ID,
                                      ExtServiceCode = x.BkgSvcExtSvcMapping.ExternalBkgSvc.EBS_ExternalCode,
                                      State = x.CESVM_StateID == null ? "ALL States" : x.State.StateName,
                                      //x.CESVM_StateID==null? "ALL" :
                                  }).Distinct().ToList();

        }
        /// <summary>
        /// Get all Background Services(Excluding all mapped).
        /// </summary>
        /// <param name="_isupdate"></param>
        /// <param name="selectedTenantID"></param>
        /// <returns></returns>
        public List<Entity.ClientEntity.BackgroundService> GetBkgService()
        {
            List<Entity.ClientEntity.BackgroundService> bkgservices = null;
            bkgservices = _ClientDBContext.BackgroundServices.Where(x => x.BSE_IsDeleted == false).ToList();
            //if (!_isupdate)
            //{
            //    //list of Bkgservice that are already mapped.
            //    List<Int32> alreadymappedBkgSvc = base.SecurityContext.ClientExtSvcVendorMappings.Include("BkgSvcExtSvcMappings").Where(x => x.CESVM_IsDeleted == false
            //        && !x.BkgSvcExtSvcMapping.BSESM_IsDeleted && x.CESVM_TenantID == selectedTenantID).Select(x => x.BkgSvcExtSvcMapping.BSESM_BkgSvcId).Distinct().ToList();
            //    if (alreadymappedBkgSvc.Count > 0)
            //    {
            //        foreach (Int32 bkgsvcId in alreadymappedBkgSvc)
            //        {
            //            //removing already mapped Background service.
            //            bkgservices.Remove(bkgservices.Where(x => x.BSE_ID == bkgsvcId).FirstOrDefault());
            //        }
            //    }
            //}
            return bkgservices.OrderBy(x => x.BSE_Name).ToList();
        }

        //get the Ext Bkg Services corresponding to Selected Background Service
        public List<Entity.ExternalBkgSvc> GetExtBkgSvcCorrespondsToBkgSvc(Int32 SelectedBkgSvcID, Int32 selectedTenantID, Boolean _isupdate)
        {
            List<Entity.ExternalBkgSvc> bkgservices = null;
            bkgservices = base.SecurityContext.BkgSvcExtSvcMappings.Where(x => x.BSESM_BkgSvcId == SelectedBkgSvcID && !x.BSESM_IsDeleted).Select(x => x.ExternalBkgSvc).ToList();
            if (!_isupdate)
            {
                //list of Bkgservice that are already mapped.
                List<Int32> alreadymappedExtVendorSvc = base.SecurityContext.ClientExtSvcVendorMappings.Include("BkgSvcExtSvcMappings").Where(x => x.CESVM_IsDeleted == false
                    && !x.BkgSvcExtSvcMapping.BSESM_IsDeleted && x.CESVM_TenantID == selectedTenantID && x.BkgSvcExtSvcMapping.BSESM_BkgSvcId == SelectedBkgSvcID).Select(x => x.BkgSvcExtSvcMapping.BSESM_ExtSvcId ?? 0).Distinct().ToList();
                if (alreadymappedExtVendorSvc.Count > 0)
                {
                    foreach (Int32 bkgsvcId in alreadymappedExtVendorSvc)
                    {
                        //removing already mapped Background service.
                        bkgservices.Remove(bkgservices.Where(x => x.EBS_ID == bkgsvcId).FirstOrDefault());
                    }
                }
            }
            return bkgservices.OrderBy(con => con.EBS_Name).ToList();
        }

        //get All the States
        public List<Entity.State> GetAllStates()
        {
            return base.SecurityContext.States.Where(x => x.IsActive == true).OrderBy(x => x.StateName).OrderByDescending(x => x.CountryID).ToList();
        }
        //Get the List of State that mapped with ExtService
        public List<Int32> GetMAppedStatesIdtoExtSvc(Int32 ExtSvcId, Int32 selectedTenantId)
        {
            return base.SecurityContext.ClientExtSvcVendorMappings.Include("BkgSvcExtSvcMappings").Where(x => x.CESVM_IsDeleted == false && x.CESVM_TenantID == selectedTenantId
                                    && x.BkgSvcExtSvcMapping.BSESM_ExtSvcId == ExtSvcId && !x.BkgSvcExtSvcMapping.BSESM_IsDeleted).Select(x => x.CESVM_StateID ?? 0).ToList();

        }
        /// <summary>
        /// get the Mapping Id of External and Background Service mapping.
        /// </summary>
        /// <param name="bkgSvcId">bkgSvcId</param>
        /// <param name="extSvcId">extSvcId</param>
        /// <returns></returns>
        public Int32 GetBkgSvcExtSvcMappedId(Int32 bkgSvcId, Int32 extSvcId)
        {
            return base.SecurityContext.BkgSvcExtSvcMappings.Where(x => x.BSESM_ExtSvcId == extSvcId && x.BSESM_BkgSvcId == bkgSvcId && !x.BSESM_IsDeleted).Select(x => x.BSESM_ID).FirstOrDefault();
        }
        /// <summary>
        /// Save the Mapped state with Service
        /// </summary>
        /// <param name="clientExtSvcVendorMapping">clientExtSvcVendorMapping</param>
        public void SaveClientSvcvendormapping(Entity.ClientExtSvcVendorMapping clientExtSvcVendorMapping)
        {
            base.SecurityContext.ClientExtSvcVendorMappings.AddObject(clientExtSvcVendorMapping);
            base.SecurityContext.SaveChanges();
        }
        /// <summary>
        ///Update the mapping list of State with Backgroung/External Service  
        /// </summary>
        /// <param name="updatedMappedStateIds">updatedMappedStateIds</param>
        /// <param name="selectedServiceID">selectedServiceID</param>
        /// <param name="selectedExternalServiceId">selectedExternalServiceId</param>
        /// <param name="selectedTenantID">selectedTenantID</param>
        /// <param name="currentLoggedInUserID">currentLoggedInUserID</param>
        public void UpdateClientSvcVendorMapping(List<Int32> updatedMappedStateIds, Int32 selectedServiceID, Int32 selectedExternalServiceId, Int32 SelectedTenantID, Int32 currentLoggedInUserID)
        {
            List<Int32> _previousMappedStateList = base.SecurityContext.ClientExtSvcVendorMappings.Include("BkgSvcExtSvcMappings")
                                                        .Where(x => x.BkgSvcExtSvcMapping.BSESM_BkgSvcId == selectedServiceID && x.BkgSvcExtSvcMapping.BSESM_ExtSvcId == selectedExternalServiceId
                                                         && !x.BkgSvcExtSvcMapping.BSESM_IsDeleted && x.CESVM_TenantID == SelectedTenantID && !x.CESVM_IsDeleted).Select(x => x.CESVM_StateID ?? 0).ToList();
            //list of State IDs to delete
            List<Int32> _previousStateIdsToDelete = (from td in _previousMappedStateList
                                                     where !updatedMappedStateIds.Contains(td)
                                                     select td).ToList();
            Int32 bkgSvcExtSvcMappingId = base.SecurityContext.BkgSvcExtSvcMappings.Where(x => x.BSESM_BkgSvcId == selectedServiceID && x.BSESM_ExtSvcId == selectedExternalServiceId).Select(x => x.BSESM_ID).FirstOrDefault();

            if (_previousStateIdsToDelete.Count > 0)
            {
                //Delete the unmapped State to service
                foreach (Int32 StateIdToDelete in _previousStateIdsToDelete)
                {
                    Entity.ClientExtSvcVendorMapping clientExtSvcVendorMapping = null;
                    if (StateIdToDelete != AppConsts.NONE)
                    {
                        clientExtSvcVendorMapping = base.SecurityContext.ClientExtSvcVendorMappings.Where(x => x.CESVM_BkgSvcExtSvcMappingID == bkgSvcExtSvcMappingId && x.CESVM_TenantID == SelectedTenantID && x.CESVM_StateID == StateIdToDelete && !x.CESVM_IsDeleted).FirstOrDefault();
                    }
                    else
                    {
                        clientExtSvcVendorMapping = base.SecurityContext.ClientExtSvcVendorMappings.Where(x => x.CESVM_BkgSvcExtSvcMappingID == bkgSvcExtSvcMappingId && x.CESVM_TenantID == SelectedTenantID && x.CESVM_StateID == null && !x.CESVM_IsDeleted).FirstOrDefault();
                    }
                    clientExtSvcVendorMapping.CESVM_IsDeleted = true;
                    clientExtSvcVendorMapping.CESVM_ModifiedOn = DateTime.Now;
                    clientExtSvcVendorMapping.CESVM_ModifiedBy = currentLoggedInUserID;
                }
            }
            //list of state IDs to insert in mapping table
            List<Int32> _updatedStateIdsToInsert = (from td in updatedMappedStateIds
                                                    where !_previousMappedStateList.Contains(td)
                                                    select td).ToList();
            if (_updatedStateIdsToInsert.Count > 0)
            {
                foreach (Int32 StateIDMappingToInsert in _updatedStateIdsToInsert)
                {
                    //insert new Mapped State to service
                    Entity.ClientExtSvcVendorMapping clientExtSvcVendorMapping = new Entity.ClientExtSvcVendorMapping();
                    clientExtSvcVendorMapping.CESVM_BkgSvcExtSvcMappingID = bkgSvcExtSvcMappingId;
                    if (StateIDMappingToInsert == AppConsts.NONE)
                    {
                        clientExtSvcVendorMapping.CESVM_StateID = null;
                    }
                    else
                    {
                        clientExtSvcVendorMapping.CESVM_StateID = StateIDMappingToInsert;
                    }
                    //clientExtSvcVendorMapping.CESVM_StateID = StateIDMappingToInsert;
                    clientExtSvcVendorMapping.CESVM_TenantID = SelectedTenantID;
                    clientExtSvcVendorMapping.CESVM_IsDeleted = false;
                    clientExtSvcVendorMapping.CESVM_CreatedBy = currentLoggedInUserID;
                    clientExtSvcVendorMapping.CESVM_CreatedOn = DateTime.Now;
                    base.SecurityContext.ClientExtSvcVendorMappings.AddObject(clientExtSvcVendorMapping);
                }
            }
            base.SecurityContext.SaveChanges();
        }
        /// <summary>
        /// delete the Mapping in ClientServiceVendorMapping related to the Backgroung Service and tenantId.
        /// </summary>
        /// <param name="bkgSvcID">bkgSvcID</param>
        /// <param name="selectedTenantId">selectedTenantId</param>
        /// <param name="currentLoggedInUserID">currentLoggedInUserID</param>
        public void DeleteClientSvcVendorMapping(Int32 bkgSvcID, Int32 ExtServiceID, Int32 selectedTenantId, Int32 currentLoggedInUserID)
        {
            //get the Mapping Ids List of External and Background service. 
            Int32 bkgSvcExtSvcMapingId = base.SecurityContext.BkgSvcExtSvcMappings
                                    .Where(x => x.BSESM_BkgSvcId == bkgSvcID && x.BSESM_ExtSvcId == ExtServiceID && !x.BSESM_IsDeleted).FirstOrDefault().BSESM_ID;

            if (bkgSvcExtSvcMapingId.IsNotNull() && bkgSvcExtSvcMapingId != AppConsts.NONE)
            {

                //get the list of mappings of Stateids and MappingId of ExtBkgSvcMapping on the basis of Seleted TenantId.
                List<Int32> clientExtVendorMappingIdslst = base.SecurityContext.ClientExtSvcVendorMappings.Where(x => x.CESVM_BkgSvcExtSvcMappingID == bkgSvcExtSvcMapingId
                                                                && x.CESVM_TenantID == selectedTenantId && !x.CESVM_IsDeleted).Select(x => x.CESVM_ID).ToList();
                if (clientExtVendorMappingIdslst.Count > 0)
                {
                    foreach (Int32 clientExtVendorMappingId in clientExtVendorMappingIdslst)
                    {
                        Entity.ClientExtSvcVendorMapping clientExtVendorMapping = base.SecurityContext.ClientExtSvcVendorMappings.FirstOrDefault(x => x.CESVM_ID == clientExtVendorMappingId && !x.CESVM_IsDeleted);
                        clientExtVendorMapping.CESVM_IsDeleted = true;
                        clientExtVendorMapping.CESVM_ModifiedBy = currentLoggedInUserID;
                        clientExtVendorMapping.CESVM_ModifiedOn = DateTime.Now;
                    }
                }


            }
            base.SecurityContext.SaveChanges();
        }
        #endregion

        #region Manage Order Color Status

        /// <summary>
        /// Returns all OrderFlags which are not deleted 
        /// </summary>
        /// <returns>List of lkpOrderFlag</returns>
        public List<lkpOrderFlag> GetAllOrderFlags()
        {
            return _ClientDBContext.lkpOrderFlags.Where(obj => !obj.OFL_IsDeleted).ToList();
        }

        /// <summary>
        /// Returns InstitutionOrderFlag for specific Tenant. 
        /// </summary>
        /// <returns>List of InstitutionOrderFlag</returns>
        public List<InstitutionOrderFlag> GetInstituteOrderFlags(Int32 tenantId)
        {
            return _ClientDBContext.InstitutionOrderFlags.Include("lkpOrderFlag").Where(obj => obj.IOF_TenantID == tenantId && !obj.IOF_IsDeleted).ToList();
        }


        /// <summary>
        /// Saves the InstitutionOrderFlag details.
        /// </summary>
        /// <param name="institutionOrderFlag">institutionOrderFlag Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>institutionOrderFlag Entity</returns>
        public InstitutionOrderFlag SaveInstitutionOrderFlagDetail(InstitutionOrderFlag institutionOrderFlag, Int32 currentLoggedInUserId)
        {
            institutionOrderFlag.IOF_CreatedByID = currentLoggedInUserId;
            institutionOrderFlag.IOF_CreatedOn = DateTime.Now;
            institutionOrderFlag.IOF_IsDeleted = false;

            _ClientDBContext.InstitutionOrderFlags.AddObject(institutionOrderFlag);
            _ClientDBContext.SaveChanges();
            return institutionOrderFlag;
        }

        /// <summary>
        /// Gets specific InstitutionOrderFlag.
        /// </summary>
        /// <param name="InstitutionOrderFlagID">InstitutionOrderFlagID</param>
        public InstitutionOrderFlag GetCurrentInstitutionOrderFlag(Int32 institutionOrderFlagID)
        {
            return _ClientDBContext.InstitutionOrderFlags.Where(obj => obj.IOF_ID == institutionOrderFlagID && obj.IOF_IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// Updates the InstitutionOrderFlag.
        /// </summary>
        /// <param name="InstitutionOrderFlag">InstitutionOrderFlag Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean UpdateInstitutionOrderFlagDetail(InstitutionOrderFlag institutionOrderFlag, Int32 currentLoggedInUserId)
        {
            InstitutionOrderFlag currentInstitutionOrderFlag = GetCurrentInstitutionOrderFlag(institutionOrderFlag.IOF_ID);
            if (currentInstitutionOrderFlag.IsNull())
                return false;

            currentInstitutionOrderFlag.IOF_OrderFlagID = institutionOrderFlag.IOF_OrderFlagID;
            currentInstitutionOrderFlag.IOF_Description = institutionOrderFlag.IOF_Description;
            currentInstitutionOrderFlag.IOF_IsSuccessIndicator = institutionOrderFlag.IOF_IsSuccessIndicator;
            currentInstitutionOrderFlag.IOF_ModifiedByID = currentLoggedInUserId;
            currentInstitutionOrderFlag.IOF_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
            return true;

        }


        /// <summary>
        /// Deletes the InstitutionOrderFlag.
        /// </summary>
        /// <param name="customFormId">institutionOrderFlagID</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public Boolean DeleteInstitutionOrderFlag(Int32 institutionOrderFlagID, Int32 currentUserId)
        {
            // Set Delete true first in CustomFormAttributeGroup table for Custom Form ID
            Boolean result = true;

            // Update InstitutionOrderFlag Table Set Delete true
            InstitutionOrderFlag institutionOrderFlag = GetCurrentInstitutionOrderFlag(institutionOrderFlagID);

            institutionOrderFlag.IOF_IsDeleted = true;
            institutionOrderFlag.IOF_ModifiedByID = currentUserId;
            institutionOrderFlag.IOF_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
            return result;
        }

        #endregion

        #region Vendor Sevice Mapping

        /// <summary>
        /// Get Vendor Service Mapping
        /// </summary>
        /// <returns>List of BkgSvcExtSvcMapping</returns>
        public List<Entity.BkgSvcExtSvcMapping> GetVendorServiceMapping()
        {
            return base.SecurityContext.BkgSvcExtSvcMappings.Include("BackgroundService").Include("ExternalBkgSvc").Include("ExternalBkgSvc.ExternalVendor")
                .Include("ClientExtSvcVendorMappings").Where(x => !x.BSESM_IsDeleted && !x.BackgroundService.BSE_IsDeleted && !x.ExternalBkgSvc.EBS_IsDeleted
                    && !x.ExternalBkgSvc.ExternalVendor.EVE_IsDeleted).ToList();
        }

        /// <summary>
        /// Delete vendor service mapping
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="modifiedByID"></param>
        /// <returns>True/False</returns>
        public Boolean DeleteVendorServiceMapping(Int32 vendorServiceMappingID, Int32 currentUserId)
        {
            Entity.BkgSvcExtSvcMapping venSvcMapping = base.SecurityContext.BkgSvcExtSvcMappings.FirstOrDefault(obj => obj.BSESM_ID == vendorServiceMappingID && !obj.BSESM_IsDeleted);
            venSvcMapping.BSESM_IsDeleted = true;
            venSvcMapping.BSESM_ModifiedOn = DateTime.Now;
            venSvcMapping.BSESM_ModifiedBy = currentUserId;
            base.SecurityContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Get Vendors 
        /// </summary>
        /// <returns>List of ExternalVendor</returns>
        public List<Entity.ExternalVendor> GetVendors()
        {
            return base.SecurityContext.ExternalVendors.Where(x => !x.EVE_IsDeleted).ToList();
        }

        /// <summary>
        /// Get External Service By VendorID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>List of ExternalBkgSvc</returns>
        public List<Entity.ExternalBkgSvc> GetExternalBkgSvcByVendorID(Int32 vendorID)
        {
            return base.SecurityContext.ExternalBkgSvcs.Where(x => !x.EBS_IsDeleted && x.EBS_VendorId == vendorID).ToList();
        }

        /// <summary>
        /// Save Vendor service Mapping
        /// </summary>
        /// <param name="bkgSvcExtSvcMapping"></param>
        /// <returns>True/False</returns>
        public Boolean SaveVendorServiceMapping(Entity.BkgSvcExtSvcMapping bkgSvcExtSvcMapping)
        {
            base.SecurityContext.BkgSvcExtSvcMappings.AddObject(bkgSvcExtSvcMapping);

            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Update Vendor Service Mapping
        /// </summary>
        /// <param name="bkgSvcExtSvcMapping"></param>
        /// <returns>True/False</returns>
        public Boolean UpdateVendorServiceMapping(Entity.BkgSvcExtSvcMapping bkgSvcExtSvcMapping)
        {
            Entity.BkgSvcExtSvcMapping venSvcMapping = base.SecurityContext.BkgSvcExtSvcMappings.FirstOrDefault(obj => obj.BSESM_ID == bkgSvcExtSvcMapping.BSESM_ID && !obj.BSESM_IsDeleted);
            venSvcMapping.BSESM_BkgSvcId = bkgSvcExtSvcMapping.BSESM_BkgSvcId;
            venSvcMapping.BSESM_ExtSvcId = bkgSvcExtSvcMapping.BSESM_ExtSvcId;
            venSvcMapping.BSESM_ModifiedBy = bkgSvcExtSvcMapping.BSESM_ModifiedBy;
            venSvcMapping.BSESM_ModifiedOn = bkgSvcExtSvcMapping.BSESM_ModifiedOn;

            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check If Vendor Service mapping already exists
        /// </summary>
        /// <param name="bkgSvcId"></param>
        /// <param name="extSvcId"></param>
        /// <param name="bsesmID"></param>
        /// <returns>True/False</returns>
        public Boolean IfVendorServiceMappingExists(Int32 bkgSvcId, Int32 extSvcId, Int32? bsesmID)
        {
            if (bsesmID == null)
                return base.SecurityContext.BkgSvcExtSvcMappings.Any(condition => condition.BSESM_BkgSvcId == bkgSvcId && condition.BSESM_ExtSvcId == extSvcId && !condition.BSESM_IsDeleted);
            else
                return base.SecurityContext.BkgSvcExtSvcMappings.Any(condition => condition.BSESM_BkgSvcId == bkgSvcId && condition.BSESM_ExtSvcId == extSvcId && condition.BSESM_ID != bsesmID && !condition.BSESM_IsDeleted);
        }

        /// <summary>
        /// Get Vendor Service Attribute Mapping By Vendor Service Mapping ID
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetVendorServiceAttributeMappingList(Int32 vendorServiceMappingID)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetVendorServiceAttributeMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorServiceMappingID", vendorServiceMappingID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Delete Vendor Service Attribute Mapping
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="vendorServiceFieldID"></param>
        /// <param name="modifiedByID"></param>
        /// <returns>True/False</returns>
        public Boolean DeleteVendorServiceAttributeMapping(Int32 vendorServiceMappingID, Int32 vendorServiceFieldID, Int32 currentUserId)
        {
            List<Entity.ExternalSvcAtributeMapping> venSvcAttMapping = base.SecurityContext.ExternalSvcAtributeMappings
                .Where(obj => obj.ESAM_ServiceMappingId == vendorServiceMappingID && obj.ESAM_ExternalBkgSvcAttributeID == vendorServiceFieldID && !obj.ESAM_IsDeleted).ToList();
            if (venSvcAttMapping.IsNotNull() && venSvcAttMapping.Count > 0)
            {
                venSvcAttMapping.ForEach(condition =>
                {
                    condition.ESAM_IsDeleted = true;
                    condition.ESAM_ModifiedBy = currentUserId;
                    condition.ESAM_ModifiedOn = DateTime.Now;
                });

                base.SecurityContext.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Get Background And External Service Atributes by Vendor Service MappingID and Vendor Service Field ID
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="vndSvcFieldId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetBkgSvcExtSvcAttributes(Int32 vendorServiceMappingID, Int32? vndSvcFieldId)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgSvcExtSvcAttributes", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorServiceMappingID", vendorServiceMappingID);
                if (vndSvcFieldId != null)
                    command.Parameters.AddWithValue("@VendorServiceFieldID", vndSvcFieldId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
            }
            return new DataSet();
        }

        /// <summary>
        /// Save Vendor Service Attribute Mapping
        /// </summary>
        /// <param name="extSvcAttMapping"></param>
        /// <returns>True/False</returns>
        public Boolean SaveVendorServiceAttributeMapping(List<Entity.ExternalSvcAtributeMapping> extSvcAttMapping)
        {
            foreach (var item in extSvcAttMapping)
                base.SecurityContext.ExternalSvcAtributeMappings.AddObject(item);

            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region Manage Rule Templates

        /// <summary>
        /// To get Rule Template List
        /// </summary>
        /// <returns></returns>
        public List<BkgRuleTemplate> GetRuleTemplates()
        {
            return _ClientDBContext.BkgRuleTemplates.Include("lkpBkgRuleResultType").Where(r => r.BRLT_IsDeleted == false).ToList();
        }

        /// <summary>
        /// To get Rule Template by rule Template Id
        /// </summary>
        /// <param name="ruleTemplateId"></param>
        /// <returns></returns>
        public BkgRuleTemplate GetRuleTemplateByID(Int32 ruleTemplateId)
        {
            return _ClientDBContext.BkgRuleTemplates.Include("BkgRuleTemplateExpressions.BkgExpression").Where(r => r.BRLT_ID == ruleTemplateId).FirstOrDefault();
        }

        /// <summary>
        /// To delete Rule Template record
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="currentUserId"></param>
        public void DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId)
        {
            BkgRuleTemplate ruleTemplate = _ClientDBContext.BkgRuleTemplates.Where(r => r.BRLT_ID == ruleId).FirstOrDefault();
            ruleTemplate.BRLT_IsDeleted = true;
            ruleTemplate.BRLT_ModifiedByID = currentUserId;
            ruleTemplate.BRLT_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// Add Rule Template
        /// </summary>
        /// <param name="complianceRuleTemplate"></param>
        public void AddRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate)
        {
            String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
            String delimiters = "|||";
            delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;
            BkgRuleTemplate ruleTemplate = new BkgRuleTemplate();

            ruleTemplate.BRLT_Code = ruleTemplate.BRLT_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleTemplate.BRLT_Code;
            ruleTemplate.BRLT_UIExpression = complianceRuleTemplate.RuleGroupExpression;
            ruleTemplate.BRLT_Name = complianceRuleTemplate.RLT_Name;
            ruleTemplate.BRLT_Description = complianceRuleTemplate.RLT_Description;
            ruleTemplate.BRLT_ResultType = complianceRuleTemplate.RLT_ResultType;
            ruleTemplate.BRLT_ObjectCount = complianceRuleTemplate.RLT_ObjectCount;
            ruleTemplate.BRLT_Notes = complianceRuleTemplate.RLT_Notes;
            ruleTemplate.BRLT_IsActive = complianceRuleTemplate.RLT_IsActive;
            ruleTemplate.BRLT_IsDeleted = complianceRuleTemplate.RLT_IsDeleted;
            ruleTemplate.BRLT_CreatedByID = complianceRuleTemplate.RLT_CreatedByID;
            ruleTemplate.BRLT_CreatedOn = complianceRuleTemplate.RLT_CreatedOn;

            foreach (Entity.ComplianceRuleExpressionTemplate rExp in complianceRuleTemplate.ComplianceRuleExpressionTemplates)
            {
                String expression = String.Empty;
                foreach (Entity.ComplianceRuleExpressionElement complianceRuleExpressionElement in rExp.RuleExpressionElements)
                {
                    String equalOperator = "EQUAL";

                    if (complianceRuleExpressionElement.ElementValue.ToUpper().StartsWith("E", StringComparison.OrdinalIgnoreCase) && !(equalOperator.Equals(complianceRuleExpressionElement.ElementValue.ToUpper())))
                    {
                        if (expression == String.Empty)
                        {
                            expression += complianceRuleExpressionElement.ElementValue;
                        }
                        else
                        {
                            expression += delimiterKey + complianceRuleExpressionElement.ElementValue;
                        }
                    }
                    else
                    {
                        if (expression == String.Empty)
                        {
                            expression += complianceRuleExpressionElement.ElementOperator;
                        }
                        else
                        {
                            expression += delimiterKey + complianceRuleExpressionElement.ElementOperator;
                        }
                    }
                }
                expression = expression.Trim();

                ruleTemplate.BkgRuleTemplateExpressions.Add
                (
                    new BkgRuleTemplateExpression()
                    {
                        BRLE_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                        BRLE_CreatedOn = DateTime.Now,
                        BRLE_IsActive = complianceRuleTemplate.RLT_IsActive,
                        BRLE_ExpressionOrder = rExp.ExpressionOrder,
                        BkgExpression = new BkgExpression()
                        {
                            BEX_ID = rExp.EX_ID,
                            BEX_Name = rExp.EX_Name,
                            BEX_Description = rExp.EX_Description,
                            BEX_ResultType = complianceRuleTemplate.RLT_ResultType,
                            BEX_Expression = expression,
                            BEX_IsActive = complianceRuleTemplate.RLT_IsActive,
                            BEX_IsDeleted = rExp.EX_IsDeleted,
                            BEX_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                            BEX_CreatedOn = DateTime.Now
                        }
                    }
                );
            }
            _ClientDBContext.BkgRuleTemplates.AddObject(ruleTemplate);
            _ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// Update Rule Template
        /// </summary>
        /// <param name="complianceRuleTemplate"></param>
        /// <param name="expressionIds"></param>
        public void UpdateRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate, List<Int32> expressionIds)
        {
            BkgRuleTemplate ruleTemplate = _ClientDBContext.BkgRuleTemplates.Include("BkgRuleTemplateExpressions.BkgExpression").Where(rr => rr.BRLT_ID == complianceRuleTemplate.RLT_ID && rr.BRLT_IsActive == true && rr.BRLT_IsDeleted == false).FirstOrDefault();
            String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
            String delimiters = "|||";
            delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;

            ruleTemplate.BRLT_Code = ruleTemplate.BRLT_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleTemplate.BRLT_Code;
            ruleTemplate.BRLT_UIExpression = complianceRuleTemplate.RuleGroupExpression;
            ruleTemplate.BRLT_Name = complianceRuleTemplate.RLT_Name;
            ruleTemplate.BRLT_Description = complianceRuleTemplate.RLT_Description;
            ruleTemplate.BRLT_ResultType = complianceRuleTemplate.RLT_ResultType;
            ruleTemplate.BRLT_ObjectCount = complianceRuleTemplate.RLT_ObjectCount;
            ruleTemplate.BRLT_Notes = complianceRuleTemplate.RLT_Notes;
            ruleTemplate.BRLT_IsActive = complianceRuleTemplate.RLT_IsActive;
            ruleTemplate.BRLT_IsDeleted = complianceRuleTemplate.RLT_IsDeleted;
            ruleTemplate.BRLT_ModifiedByID = complianceRuleTemplate.RLT_ModifiedByID;
            ruleTemplate.BRLT_ModifiedOn = complianceRuleTemplate.RLT_ModifiedOn;

            foreach (Entity.ComplianceRuleExpressionTemplate rExp in complianceRuleTemplate.ComplianceRuleExpressionTemplates)
            {
                string expression = string.Empty;
                foreach (Entity.ComplianceRuleExpressionElement ruleExpressionElement in rExp.RuleExpressionElements)
                {
                    String equalOperator = "EQUAL";

                    if (ruleExpressionElement.ElementValue.ToUpper().StartsWith("E", StringComparison.OrdinalIgnoreCase) && !(equalOperator.Equals(ruleExpressionElement.ElementValue.ToUpper())))
                    {
                        if (expression == String.Empty)
                        {
                            expression += ruleExpressionElement.ElementValue;
                        }
                        else
                        {
                            expression += delimiterKey + ruleExpressionElement.ElementValue;
                        }
                    }
                    else
                    {
                        if (expression == String.Empty)
                        {
                            expression += ruleExpressionElement.ElementOperator;
                        }
                        else
                        {
                            expression += delimiterKey + ruleExpressionElement.ElementOperator;
                        }
                    }
                }
                expression = expression.Trim();
                if (rExp.EX_ID == 0)
                {
                    ruleTemplate.BkgRuleTemplateExpressions.Add
                    (
                        new BkgRuleTemplateExpression()
                        {
                            BRLE_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                            BRLE_CreatedOn = complianceRuleTemplate.RLT_CreatedOn,
                            BRLE_IsActive = true,
                            BRLE_ExpressionOrder = rExp.ExpressionOrder,
                            BkgExpression = new BkgExpression()
                            {
                                BEX_ID = rExp.EX_ID,
                                BEX_Name = rExp.EX_Name,
                                BEX_Description = rExp.EX_Description,
                                BEX_ResultType = complianceRuleTemplate.RLT_ResultType,
                                BEX_Expression = expression,
                                BEX_IsActive = true,
                                BEX_IsDeleted = false,
                                BEX_CreatedByID = complianceRuleTemplate.RLT_CreatedByID,
                                BEX_CreatedOn = complianceRuleTemplate.RLT_CreatedOn,
                            }
                        });
                }
                else
                {
                    BkgRuleTemplateExpression rex = ruleTemplate.BkgRuleTemplateExpressions.FirstOrDefault(fx => fx.BRLE_ExpressionID == rExp.EX_ID
                                                 && fx.BRLE_IsActive == true && fx.BRLE_IsDeleted == false);
                    if (rex.IsNotNull())
                    {
                        rex.BRLE_ModifiedByID = complianceRuleTemplate.RLT_ModifiedByID;
                        rex.BRLE_ModifiedOn = complianceRuleTemplate.RLT_ModifiedOn;
                        rex.BRLE_IsActive = true;
                        rex.BRLE_ExpressionOrder = rExp.ExpressionOrder;

                        rex.BkgExpression.BEX_ID = rExp.EX_ID;
                        rex.BkgExpression.BEX_Name = rExp.EX_Name;
                        rex.BkgExpression.BEX_Description = rExp.EX_Description;
                        rex.BkgExpression.BEX_ResultType = complianceRuleTemplate.RLT_ResultType;
                        rex.BkgExpression.BEX_Expression = expression;
                        rex.BkgExpression.BEX_IsActive = true;
                        rex.BkgExpression.BEX_IsDeleted = false;
                        rex.BkgExpression.BEX_ModifiedByID = complianceRuleTemplate.RLT_ModifiedByID;
                        rex.BkgExpression.BEX_ModifiedOn = complianceRuleTemplate.RLT_ModifiedOn;
                    }
                }
            }
            //Delete old records from saved records
            BkgRuleTemplateExpression ruleTemplateExpression = null;
            if (expressionIds.IsNotNull())
            {
                foreach (var exId in expressionIds)
                {
                    ruleTemplateExpression = ruleTemplate.BkgRuleTemplateExpressions.FirstOrDefault(x => x.BRLE_ExpressionID == exId);
                    if (ruleTemplateExpression.IsNotNull())
                    {
                        //Update RuleTemplateExpression table
                        ruleTemplateExpression.BRLE_IsDeleted = true;
                        ruleTemplateExpression.BRLE_ModifiedByID = ruleTemplate.BRLT_ModifiedByID;
                        ruleTemplateExpression.BRLE_ModifiedOn = ruleTemplate.BRLT_ModifiedOn;

                        //Update Expression table
                        if (ruleTemplateExpression.BkgExpression.IsNotNull())
                        {
                            ruleTemplateExpression.BkgExpression.BEX_IsDeleted = true;
                            ruleTemplateExpression.BkgExpression.BEX_ModifiedByID = ruleTemplate.BRLT_ModifiedByID;
                            ruleTemplateExpression.BkgExpression.BEX_ModifiedOn = ruleTemplate.BRLT_ModifiedOn;
                        }
                    }
                }
            }
            _ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// Validate Rule Template
        /// </summary>
        /// <param name="ruleTemplateXML"></param>
        /// <returns></returns>
        public String ValidateRuleTemplate(String ruleTemplateXML)
        {
            return ClientDBContext.ValidateRuleTemplate(ruleTemplateXML).FirstOrDefault();
        }

        #endregion

        #region Manage Service Settings

        public Entity.ApplicableServiceSetting GetServiceSetting(Int32 backgroundServiceId)
        {
            return base.SecurityContext.ApplicableServiceSettings.Where(obj => obj.ASSE_BackgroundServiceID == backgroundServiceId).FirstOrDefault();
        }


        BkgPackageSvc IBackgroundSetupRepository.GetCurrentBkgPkgServiceDetail(Int32 backGroundPkgSrvcId)
        {
            return ClientDBContext.BkgPackageSvcs.Include("BackgroundService").Where(obj => !obj.BPS_IsDeleted && obj.BPS_ID == backGroundPkgSrvcId).FirstOrDefault();
        }

        //UAT-3109 --Add Service AMER# when clicking service name on hierarchy and package screens
        public String GetCurrentBkgPkgServiceAMERDetail(Int32 backgroundServiceId)
        {
            var BkgSvcExtSvcMappings = base.SecurityContext.BkgSvcExtSvcMappings.Where(cond => cond.BSESM_BkgSvcId == backgroundServiceId && !cond.BSESM_IsDeleted).FirstOrDefault();
            if (!BkgSvcExtSvcMappings.IsNullOrEmpty())
            {
                var ExternalBkgSvcs = base.SecurityContext.ExternalBkgSvcs.Where(cond => !cond.EBS_IsDeleted && cond.EBS_ID == BkgSvcExtSvcMappings.BSESM_ExtSvcId).FirstOrDefault();
                if (!ExternalBkgSvcs.IsNullOrEmpty())
                    return ExternalBkgSvcs.EBS_ExternalCode;
            }
            return String.Empty;
        }

        #endregion

        #region Order Client Status
        public Boolean SaveOrderClientStatus(Int32 SelectedTenantId, String OrderClientStatusTypeName, Int32 currentLoggedInUserId)
        {
            BkgOrderClientStatu bkgOrderClientStatus = new BkgOrderClientStatu();
            //not implemented yet

            BkgOrderClientStatu lastOrderStatus = _ClientDBContext.BkgOrderClientStatus.OrderByDescending(x => x.BOCS_DisplayOrder).Where(x => x.BOCS_IsDeleted == false).FirstOrDefault();
            bkgOrderClientStatus.BOCS_DisplayOrder = (lastOrderStatus.IsNotNull()) ? lastOrderStatus.BOCS_DisplayOrder + 1 : 1;
            bkgOrderClientStatus.BOCS_InstitutionID = SelectedTenantId;
            bkgOrderClientStatus.BOCS_OrderClientStatusTypeName = OrderClientStatusTypeName;
            bkgOrderClientStatus.BOCS_CreatedByID = currentLoggedInUserId;
            bkgOrderClientStatus.BOCS_CreatedOn = DateTime.Now;
            bkgOrderClientStatus.BOCS_ModifiedByID = null;
            bkgOrderClientStatus.BOCS_ModifiedOn = null;
            bkgOrderClientStatus.BOCS_IsDeleted = false;
            _ClientDBContext.AddToBkgOrderClientStatus(bkgOrderClientStatus);
            if (_ClientDBContext.SaveChanges() > 0)
            {
                _ClientDBContext.Refresh(RefreshMode.StoreWins, bkgOrderClientStatus);
                return true;
            }
            else
                return false;
        }

        public List<BkgOrderClientStatu> FetchOrderClientStatus()
        {
            List<BkgOrderClientStatu> bkgOrderClientStatu = _ClientDBContext.BkgOrderClientStatus.Include("Tenant").Where(x => x.BOCS_IsDeleted == false).OrderBy(x => x.BOCS_DisplayOrder).ToList();

            if (!bkgOrderClientStatu.IsNullOrEmpty())
            {
                return bkgOrderClientStatu;
            }
            else
            {
                return new List<BkgOrderClientStatu>();
            }
        }

        public Boolean UpdateClientStatusSequence(IList<BkgOrderClientStatu> statusToMove, int? destinationIndex, int currentLoggedInUserId)
        {
            DataTable dtClientStatus = new DataTable();
            dtClientStatus.Columns.Add("StatusID", typeof(Int32));
            dtClientStatus.Columns.Add("DestinationIndex", typeof(Int32));
            dtClientStatus.Columns.Add("CurrentLoggedInUserId", typeof(Int32));
            foreach (BkgOrderClientStatu status in statusToMove)
            {
                dtClientStatus.Rows.Add(new object[] { status.BOCS_ID, destinationIndex, currentLoggedInUserId });
                destinationIndex += 1;
            }

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.UpdateClientStatusSequence", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeParameter", dtClientStatus);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    con.Close();
                    _ClientDBContext.Refresh(RefreshMode.StoreWins, statusToMove);
                    return true;
                }
            }
            return false;
        }


        public bool DeleteOrderClientStatus(int Id, int CurrentLoggedInUserId)
        {
            BkgOrderClientStatu bkgOrderClientStatus = _ClientDBContext.BkgOrderClientStatus.Where(x => x.BOCS_ID == Id && x.BOCS_IsDeleted == false).OrderBy(x => x.BOCS_DisplayOrder).FirstOrDefault();
            if (bkgOrderClientStatus.IsNotNull())
            {
                bkgOrderClientStatus.BOCS_IsDeleted = true;
                bkgOrderClientStatus.BOCS_ModifiedByID = CurrentLoggedInUserId;
                bkgOrderClientStatus.BOCS_ModifiedOn = DateTime.Now;
                //Re-Order Display Sequence of the Records.
                List<BkgOrderClientStatu> lstBkgOrderStatus = _ClientDBContext.BkgOrderClientStatus.Where(cond => !cond.BOCS_IsDeleted && cond.BOCS_DisplayOrder > bkgOrderClientStatus.BOCS_DisplayOrder).ToList();
                DataTable dtClientStatus = new DataTable();
                dtClientStatus.Columns.Add("StatusID", typeof(Int32));
                dtClientStatus.Columns.Add("DestinationIndex", typeof(Int32));
                dtClientStatus.Columns.Add("CurrentLoggedInUserId", typeof(Int32));
                foreach (BkgOrderClientStatu status in lstBkgOrderStatus)
                {
                    dtClientStatus.Rows.Add(new object[] { status.BOCS_ID, status.BOCS_DisplayOrder - 1, CurrentLoggedInUserId });
                }
                EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand _command = new SqlCommand("ams.UpdateClientStatusSequence", con);
                    _command.CommandType = CommandType.StoredProcedure;
                    _command.Parameters.AddWithValue("@typeParameter", dtClientStatus);
                    con.Open();
                    Int32 rowsAffected = _command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        con.Close();
                    }
                    _ClientDBContext.SaveChanges();
                    _ClientDBContext.Refresh(RefreshMode.StoreWins, lstBkgOrderStatus);
                }

            }
            return true;
        }

        public bool UpdateOrderClientStatus(int OrderClientStatusId, string OrderClientStatusTypeName, int CurrentLoggedInUserId)
        {
            BkgOrderClientStatu bkgOrderClientStatus = _ClientDBContext.BkgOrderClientStatus.Where(x => x.BOCS_ID == OrderClientStatusId && x.BOCS_IsDeleted == false).FirstOrDefault();
            if (bkgOrderClientStatus.IsNotNull())
            {
                bkgOrderClientStatus.BOCS_OrderClientStatusTypeName = OrderClientStatusTypeName;
                bkgOrderClientStatus.BOCS_ModifiedByID = CurrentLoggedInUserId;
                bkgOrderClientStatus.BOCS_ModifiedOn = System.DateTime.Now;
                if (_ClientDBContext.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Manage Service Item Entity

        public List<GetServiceItemEntityList> getServiceItemEntityList(Int32 serviceItemId)
        {
            return ClientDBContext.usp_GetServiceItemEntityList(serviceItemId).ToList();
        }

        public List<GetAttributeListForServiceItemEntity> getAttribteListForServiceItemEntity(Int32 serviceItemId)
        {
            return ClientDBContext.usp_GetAttributeListForServiceItemEntity(serviceItemId).ToList();
        }

        public Boolean SavePackageServiceItemEntity(List<PackageServiceItemEntity> newServiceItemEntityList, Int32 currentloggedInUserId)
        {
            foreach (PackageServiceItemEntity serviceItemEntity in newServiceItemEntityList)
            {
                serviceItemEntity.PSIE_CreatedByID = currentloggedInUserId;
                serviceItemEntity.PSIE_CreatedOn = DateTime.Now;
                ClientDBContext.PackageServiceItemEntities.AddObject(serviceItemEntity);
            }
            if (ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
        public Boolean DeletePackageServiceItemEntityRecord(Int32 packageServiceItemEntityId, Int32 currentloggedInUserId)
        {
            PackageServiceItemEntity serviceItemEntity = ClientDBContext.PackageServiceItemEntities.FirstOrDefault(x => x.PSIE_ID == packageServiceItemEntityId && !x.PSIE_IsDeleted);
            if (serviceItemEntity != null)
            {
                serviceItemEntity.PSIE_IsDeleted = true;
                serviceItemEntity.PSIE_ModifiedByID = currentloggedInUserId;
                serviceItemEntity.PSIE_ModifiedOn = DateTime.Now;
                ClientDBContext.SaveChanges();
                return true;
            }

            return false;
        }
        #endregion

        #region Service Vendors
        public IList<Entity.ExternalVendor> FetchServiceVendors()
        {
            return base.SecurityContext.ExternalVendors.Where(x => x.EVE_IsDeleted == false).ToList();
        }

        public Boolean SaveServiceVendors(VendorsDetailsContract vendorDetails, Int32 CurrentLoggedInUserId)
        {
            Entity.ExternalVendor previousCode = base.SecurityContext.ExternalVendors.OrderByDescending(x => x.EVE_Code).FirstOrDefault();
            String nextEVECode = GetNextCode(previousCode.EVE_Code);
            Entity.ExternalVendor externalVendor = new Entity.ExternalVendor();
            externalVendor.EVE_Name = vendorDetails.Name;
            externalVendor.EVE_Description = vendorDetails.Description;
            externalVendor.EVE_ContactName = vendorDetails.ContactName;
            externalVendor.EVE_ContactPhone = vendorDetails.ContactPhone;
            externalVendor.EVE_ContactEmail = vendorDetails.ContactEmail;
            externalVendor.EVE_Code = nextEVECode;
            externalVendor.EVE_CreatedBy = CurrentLoggedInUserId;
            externalVendor.EVE_CreatedOn = DateTime.Now;
            externalVendor.EVE_IsDeleted = false;
            base.SecurityContext.AddToExternalVendors(externalVendor);
            if (base.SecurityContext.SaveChanges() > 0)
                return true;

            else
                return false;
        }

        public Boolean UpdateServiceVendors(VendorsDetailsContract vendorDetails, Int32 serviceVendorId, Int32 CurrentLoggedInUserId)
        {
            Entity.ExternalVendor externalVendor = base.SecurityContext.ExternalVendors.Where(x => x.EVE_ID == serviceVendorId && x.EVE_IsDeleted == false).FirstOrDefault();
            externalVendor.EVE_Name = vendorDetails.Name;
            externalVendor.EVE_Description = vendorDetails.Description;
            externalVendor.EVE_ContactName = vendorDetails.ContactName;
            externalVendor.EVE_ContactPhone = vendorDetails.ContactPhone;
            externalVendor.EVE_ContactEmail = vendorDetails.ContactEmail;
            externalVendor.EVE_ModifiedBy = CurrentLoggedInUserId;
            externalVendor.EVE_ModifiedOn = DateTime.Now;
            if (base.SecurityContext.SaveChanges() > 0)
                return true;

            else
                return false;
        }


        public Boolean DeleteServiceVendors(Int32 serviceVendorsID, Int32 CurrentLoggedInUserId)
        {
            Entity.ExternalVendor externalVendor = base.SecurityContext.ExternalVendors.Where(x => x.EVE_ID == serviceVendorsID && x.EVE_IsDeleted == false).FirstOrDefault();
            externalVendor.EVE_IsDeleted = true;
            externalVendor.EVE_ModifiedBy = CurrentLoggedInUserId;
            externalVendor.EVE_ModifiedOn = DateTime.Now;
            if (base.SecurityContext.SaveChanges() > 0)
                return true;

            else
                return false;
        }

        private String GetNextCode(String id)
        {
            // if id is invalid return an empty string, say
            if (id == null || id.Length != 4) return "";
            for (int i = 0; i < 4; i++)
            {
                if (id[i] < 'A' || id[i] > 'Z') return "";
            }
            int code = (id[0] - 65) * 26 * 26 * 26 + (id[1] - 65) * 26 * 26 + (id[2] - 65) * 26 + (id[3] - 65);
            code++;
            char[] chars = new char[4];
            for (int i = 3; i >= 0; i--)
            {
                chars[i] = (char)(code % 26 + 65);
                code /= 26;
            }
            return new string(chars);
        }
        #endregion

        #region BackgroundPackageDetails
        public BkgPackageHierarchyMapping GetBackgroundPackageDetail(Int32 BkgPackageNodeMappingId)
        {
            return _ClientDBContext.BkgPackageHierarchyMappings.Include("BackgroundPackage").Include("lkpPackageAvailability")
                                    .Where(x => x.BPHM_ID == BkgPackageNodeMappingId && !x.BPHM_IsDeleted && !x.BackgroundPackage.BPA_IsDeleted).FirstOrDefault();

        }
        public Boolean UpdatePackageHirarchyDetails(BkgPackageHierarchyMapping bkgPackageHierarchyMapping, Int32 bkgPackageHierarchyMappingId, Int32 currentLoggedInID, List<Int32> lstSelectedOptionIds, List<Int32> targetPackageIds, Int32 months, Boolean isActive)
        {
            BkgPackageHierarchyMapping bkgPackageHierarchyMappingToUpdate = _ClientDBContext.BkgPackageHierarchyMappings.Where(x => x.BPHM_ID == bkgPackageHierarchyMappingId && !x.BPHM_IsDeleted).FirstOrDefault();
            if (bkgPackageHierarchyMappingToUpdate.IsNull())
                return false;

            bkgPackageHierarchyMappingToUpdate.BPHM_PackageBasePrice = bkgPackageHierarchyMapping.BPHM_PackageBasePrice;
            bkgPackageHierarchyMappingToUpdate.BPHM_IsExclusive = bkgPackageHierarchyMapping.BPHM_IsExclusive;
            bkgPackageHierarchyMappingToUpdate.BPHM_NeedFirstReview = bkgPackageHierarchyMapping.BPHM_NeedFirstReview;
            bkgPackageHierarchyMappingToUpdate.BPHM_TransmitToVendor = bkgPackageHierarchyMapping.BPHM_TransmitToVendor;
            bkgPackageHierarchyMappingToUpdate.BPHM_PkgSupplementalTypeID = bkgPackageHierarchyMapping.BPHM_PkgSupplementalTypeID;
            bkgPackageHierarchyMappingToUpdate.BPHM_Instructions = bkgPackageHierarchyMapping.BPHM_Instructions;
            bkgPackageHierarchyMappingToUpdate.BPHM_ModifiedByID = bkgPackageHierarchyMapping.BPHM_ModifiedByID;
            bkgPackageHierarchyMappingToUpdate.BPHM_ModifiedOn = bkgPackageHierarchyMapping.BPHM_ModifiedOn;
            bkgPackageHierarchyMappingToUpdate.BPHM_CustomPriceText = bkgPackageHierarchyMapping.BPHM_CustomPriceText;
            bkgPackageHierarchyMappingToUpdate.BPHM_MaxNumberOfYearforResidence = bkgPackageHierarchyMapping.BPHM_MaxNumberOfYearforResidence;
            bkgPackageHierarchyMappingToUpdate.BPHM_PackageAvailabilityID = bkgPackageHierarchyMapping.BPHM_PackageAvailabilityID;
            bkgPackageHierarchyMappingToUpdate.BPHM_IsAvailableForAdminEntry = bkgPackageHierarchyMapping.BPHM_IsAvailableForAdminEntry;
            //UAT-2073
            bkgPackageHierarchyMappingToUpdate.BPHM_PaymentApprovalID = bkgPackageHierarchyMapping.BPHM_PaymentApprovalID;

            bkgPackageHierarchyMappingToUpdate.BackgroundPackage.BPA_Name = bkgPackageHierarchyMapping.BackgroundPackage.BPA_Name;
            bkgPackageHierarchyMappingToUpdate.BackgroundPackage.BPA_Label = bkgPackageHierarchyMapping.BackgroundPackage.BPA_Label;
            bkgPackageHierarchyMappingToUpdate.BackgroundPackage.BPA_BkgPackageTypeId = bkgPackageHierarchyMapping.BackgroundPackage.BPA_BkgPackageTypeId; //UAT-3525
            bkgPackageHierarchyMappingToUpdate.BackgroundPackage.BPA_Passcode = bkgPackageHierarchyMapping.BackgroundPackage.BPA_Passcode; //UAT-3771

            //Uat-3268
            bkgPackageHierarchyMappingToUpdate.BPHM_IsAdditionalPriceAvailable = bkgPackageHierarchyMapping.BPHM_IsAdditionalPriceAvailable;
            bkgPackageHierarchyMappingToUpdate.BPHM_AdditionalPrice = bkgPackageHierarchyMapping.BPHM_AdditionalPrice;
            bkgPackageHierarchyMappingToUpdate.BPHM_AdditionalPricePaymentOptionID = bkgPackageHierarchyMapping.BPHM_AdditionalPricePaymentOptionID;

            #region UAT-2388 : On Hierarchy Setup Screen
            var bckAutomaticInvitationPkg = _ClientDBContext.PackageInvitationSettings.FirstOrDefault(d => d.PIS_BkgPkgID == bkgPackageHierarchyMappingToUpdate.BackgroundPackage.BPA_ID && !d.PIS_IsDeleted);
            if (bckAutomaticInvitationPkg.IsNullOrEmpty())
            {
                //insert
                var PackageInvitationSetting = new PackageInvitationSetting();
                PackageInvitationSetting.PIS_BkgPkgID = bkgPackageHierarchyMappingToUpdate.BackgroundPackage.BPA_ID;
                PackageInvitationSetting.PIS_CreatedByID = currentLoggedInID;
                PackageInvitationSetting.PIS_CreatedOn = DateTime.Now;
                PackageInvitationSetting.PIS_IsDeleted = false;
                PackageInvitationSetting.PIS_Months = months;
                PackageInvitationSetting.PIS_IsActive = isActive;
                foreach (var item in targetPackageIds)
                {
                    var PackageInvitationSettingPackage = new PackageInvitationSettingPackage();
                    PackageInvitationSettingPackage.PISP_CreatedByID = currentLoggedInID;
                    PackageInvitationSettingPackage.PISP_CreatedOn = DateTime.Now;
                    PackageInvitationSettingPackage.PISP_IsDeleted = false;
                    PackageInvitationSettingPackage.PISP_TargetBkgPkgID = item;
                    PackageInvitationSetting.PackageInvitationSettingPackages.Add(PackageInvitationSettingPackage);
                }
                _ClientDBContext.PackageInvitationSettings.AddObject(PackageInvitationSetting);
            }
            else
            {
                //update
                bckAutomaticInvitationPkg.PIS_ModifiedByID = currentLoggedInID;
                bckAutomaticInvitationPkg.PIS_ModifiedOn = DateTime.Now;
                bckAutomaticInvitationPkg.PIS_IsActive = isActive;
                if (isActive)
                {
                    bckAutomaticInvitationPkg.PIS_Months = months;
                    var oldMappingList = bckAutomaticInvitationPkg.PackageInvitationSettingPackages.Where(cond => !cond.PISP_IsDeleted).ToList();
                    //get those mapping list which need to remove
                    var deleteMappingList = oldMappingList.Where(cond => !targetPackageIds.Contains(cond.PISP_TargetBkgPkgID)).ToList();
                    //Get existed mapping list
                    List<Int32> existMapping = oldMappingList.Where(cond => targetPackageIds.Contains(cond.PISP_TargetBkgPkgID)).Select(sel => sel.PISP_TargetBkgPkgID).ToList();
                    //delete old mapping
                    deleteMappingList.ForEach(s => s.PISP_IsDeleted = true);
                    //Add new mapping which are not existed
                    foreach (var item in targetPackageIds)
                    {
                        if (!existMapping.Contains(item))
                        {
                            var PackageInvitationSettingPackage = new PackageInvitationSettingPackage();
                            PackageInvitationSettingPackage.PISP_CreatedByID = AppConsts.ONE;
                            PackageInvitationSettingPackage.PISP_CreatedOn = DateTime.Now;
                            PackageInvitationSettingPackage.PISP_IsDeleted = false;
                            PackageInvitationSettingPackage.PISP_TargetBkgPkgID = item;
                            bckAutomaticInvitationPkg.PackageInvitationSettingPackages.Add(PackageInvitationSettingPackage);
                        }
                    }
                }

            }
            #endregion

            #region Save/Update Package Level Payment Options
            var _currentUserId = Convert.ToInt32(bkgPackageHierarchyMapping.BPHM_ModifiedByID);
            var _currentDateTime = Convert.ToDateTime(bkgPackageHierarchyMapping.BPHM_ModifiedOn);

            List<BkgPackagePaymentOption> mappedList = bkgPackageHierarchyMappingToUpdate.BkgPackagePaymentOptions
                                                       .Where(bppo => bppo.BPPO_IsDeleted == false)
                                                       .ToList();

            List<BkgPackagePaymentOption> optionsToDelete = mappedList.Where(dpppo => !lstSelectedOptionIds.Any(optnId => optnId == dpppo.BPPO_PaymentOptionID)
                                                                          && dpppo.BPPO_IsDeleted == false).ToList();

            List<Int32> optionsToInsert = lstSelectedOptionIds.Where(optnId => !mappedList.Any(dpppo => dpppo.BPPO_PaymentOptionID == optnId)).ToList();

            optionsToDelete.ForEach(cond =>
            {
                cond.BPPO_IsDeleted = true;
                cond.BPPO_ModifiedByID = bkgPackageHierarchyMapping.BPHM_ModifiedByID;
                cond.BPPO_ModifiedOn = bkgPackageHierarchyMapping.BPHM_ModifiedOn;
            });

            for (int i = 0; i < optionsToInsert.Count; i++)
            {
                _ClientDBContext.BkgPackagePaymentOptions.AddObject
                    (
                    new BkgPackagePaymentOption
                    {
                        BPPO_BPHMID = bkgPackageHierarchyMappingId,
                        BPPO_PaymentOptionID = optionsToInsert[i],
                        BPPO_IsDeleted = false,
                        BPPO_CreatedByID = _currentUserId,
                        BPPO_CreatedOn = _currentDateTime
                    });
            }
            #endregion

            _ClientDBContext.SaveChanges();
            return true;
        }
        #endregion

        #region Institution Hierarchy Vendor Account Mapping

        /// <summary>
        /// Get the list of External Vendor Account and Institution Hierarchy Vendor Account Mapping for a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public List<ExternalVendorAccountMappingDetails> GetInstHierarchyVendorAcctMappingDetails(Int32 DPMId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetInstHierarchyVendorAcctMappingDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DPMID", DPMId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                IEnumerable<DataRow> rows = ds.Tables[0].AsEnumerable();
                return rows.Select(col => new ExternalVendorAccountMappingDetails
                {
                    DPMEVAM_ID = Convert.ToInt32(col["DPMEVAM_ID"]),
                    DPMEVAM_ExternalVendorAccountID = Convert.ToInt32(Convert.ToString(col["DPMEVAM_ExternalVendorAccountID"])),
                    EVE_Name = Convert.ToString(col["EVE_Name"]),
                    EVA_AccountNumber = Convert.ToString(col["EVA_AccountNumber"]),
                    EVA_AccountName = Convert.ToString(col["EVA_AccountName"]),
                    EVA_VendorID = Convert.ToInt32(Convert.ToString(col["EVA_VendorID"]))
                }).ToList();
            }
        }

        /// <summary>
        /// Get the list of Filtered (yet not selected) External Vendor Account for a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public List<Entity.ExternalVendorAccount> GetExternalVendorAccountsNotMapped(Int32 nDPMId)
        {
            List<Int32> lstMappedExternalVendorAccountIDs = _ClientDBContext.InstHierarchyVendorAcctMappings.Where(cond => cond.DPMEVAM_DeptProgramMappingID == nDPMId && !cond.DPMEVAM_IsDeleted)
                                                            .Select(obj => obj.DPMEVAM_ExternalVendorAccountID).ToList();
            if (lstMappedExternalVendorAccountIDs.Count > 0)
                return base.SecurityContext.ExternalVendorAccounts.Where(cond => !lstMappedExternalVendorAccountIDs.Contains(cond.EVA_ID) && !cond.EVA_IsDeleted).ToList();
            else
                return base.SecurityContext.ExternalVendorAccounts.Where(cond => !cond.EVA_IsDeleted).ToList();

        }

        public List<MappedResidentialHistoryAttributeGroupsWithPkg> GetMappedResidentialHistoryAttributeGroupsWithPkg(Int32 pkgId)
        {
            return ClientDBContext.GetMappedResidentialHistoryAttributeGroupsWithPkg(pkgId).ToList();
        }

        /// <summary>
        /// Save the External Vendor Account mapping for a given node
        /// </summary>
        /// <param name="objInstHierarchyVendorAcctMapping">object InstHierarchyVendorAcctMapping</param>
        /// <returns></returns>
        public Boolean SaveInstHierarchyVendorAcctMapping(InstHierarchyVendorAcctMapping objInstHierarchyVendorAcctMapping)
        {
            _ClientDBContext.InstHierarchyVendorAcctMappings.AddObject(objInstHierarchyVendorAcctMapping);

            if (_ClientDBContext.SaveChanges() > 0)
                return true;

            else
                return false;
        }

        /// <summary>
        /// Update Tenant after modifying record.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateTenantChanges()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get the  External Vendor Account mapping for a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public InstHierarchyVendorAcctMapping GetInstHierarchyVendorAcctMappingByID(Int32 instHierarchyVendorAcctMappingID)
        {
            return _ClientDBContext.InstHierarchyVendorAcctMappings.Where(obj => obj.DPMEVAM_ID == instHierarchyVendorAcctMappingID && !obj.DPMEVAM_IsDeleted).FirstOrDefault();
        }

        #endregion

        #region Vendors Account Details

        public IList<Entity.ExternalVendorAccount> FetchVendorsAccountDetail(Int32 VendorId)
        {
            return base.SecurityContext.ExternalVendorAccounts.Where(x => x.EVA_VendorID == VendorId && x.EVA_IsDeleted == false).ToList();
        }

        public String SaveVendorsAccountDetail(Int32 VendorId, String AccountNumber, String AccountName, Int32 CurrentLoggedInUserId)
        {
            //Check the Existing Account Number
            Entity.ExternalVendorAccount _accountNameAlreadyExist = base.SecurityContext.ExternalVendorAccounts.Where(cond => cond.EVA_AccountName == AccountName && cond.EVA_IsDeleted == false).FirstOrDefault();
            Entity.ExternalVendorAccount _accountNumberAlreadyExist = base.SecurityContext.ExternalVendorAccounts.Where(cond => cond.EVA_AccountNumber == AccountNumber && cond.EVA_IsDeleted == false).FirstOrDefault();
            if (_accountNameAlreadyExist.IsNotNull())
            {
                return "Account name " + AccountName + " is already exist, please enter different account name.";
            }
            else if (_accountNumberAlreadyExist.IsNotNull())
            {
                return "Account number " + AccountNumber + " is already exist, please enter different account number.";
            }
            else
            {
                Entity.ExternalVendorAccount externalVendorAccount = new Entity.ExternalVendorAccount();
                externalVendorAccount.EVA_VendorID = VendorId;
                externalVendorAccount.EVA_AccountName = AccountName;
                externalVendorAccount.EVA_AccountNumber = AccountNumber;
                externalVendorAccount.EVA_IsDeleted = false;
                externalVendorAccount.EVA_CreatedById = CurrentLoggedInUserId;
                externalVendorAccount.EVA_CreatedOn = DateTime.Now;
                base.SecurityContext.AddToExternalVendorAccounts(externalVendorAccount);
            }
            if (base.SecurityContext.SaveChanges() > 0)
                return "Account is successfully created.";

            else
                return "Some error occurred.";
        }

        public String UpdateVendorsAccountDetail(String AccountNumber, String AccountName, Int32 EvaId, Int32 CurrentLoggedInUserId)
        {
            Boolean _updateFlag = false;
            Entity.ExternalVendorAccount _accountAlreadyExist = base.SecurityContext.ExternalVendorAccounts.Where(cond => cond.EVA_ID == EvaId && cond.EVA_IsDeleted == false).FirstOrDefault();
            if (AccountNumber != _accountAlreadyExist.EVA_AccountNumber)
            {
                Entity.ExternalVendorAccount _accountNumberAlreadyExist = base.SecurityContext.ExternalVendorAccounts.Where(cond => cond.EVA_AccountNumber == AccountNumber && cond.EVA_IsDeleted == false).FirstOrDefault();
                if (_accountNumberAlreadyExist.IsNotNull())
                    return "Account number " + AccountNumber + " is already exist, please enter different account number.";
                else
                    _updateFlag = true;
            }

            if (AccountName != _accountAlreadyExist.EVA_AccountName)
            {
                Entity.ExternalVendorAccount _accountNameAlreadyExist = base.SecurityContext.ExternalVendorAccounts.Where(cond => cond.EVA_AccountName == AccountName && cond.EVA_IsDeleted == false).FirstOrDefault();
                if (_accountNameAlreadyExist.IsNotNull())
                    return "Account name " + AccountName + " is already exist, please enter different account name.";
                else
                    _updateFlag = true;
            }

            if (_updateFlag)
            {
                Entity.ExternalVendorAccount externalVendorAccount = base.SecurityContext.ExternalVendorAccounts.Where(x => x.EVA_ID == EvaId && x.EVA_IsDeleted == false).FirstOrDefault();
                externalVendorAccount.EVA_AccountName = AccountName;
                externalVendorAccount.EVA_AccountNumber = AccountNumber;
                externalVendorAccount.EVA_IsDeleted = false;
                externalVendorAccount.EVA_ModifiedByID = CurrentLoggedInUserId;
                externalVendorAccount.EVA_ModifiedOn = DateTime.Now;
            }
            if (!_updateFlag || base.SecurityContext.SaveChanges() > 0)
                return "Account is successfully updated.";

            else
                return "Some error occurred.";
        }

        public bool DeleteVendorsAccountDetail(int EvaId, int CurrentLoggedInUserId)
        {
            Entity.ExternalVendorAccount externalVendorAccount = base.SecurityContext.ExternalVendorAccounts.Where(x => x.EVA_ID == EvaId && x.EVA_IsDeleted == false).FirstOrDefault();
            externalVendorAccount.EVA_IsDeleted = true;
            externalVendorAccount.EVA_ModifiedByID = CurrentLoggedInUserId;
            externalVendorAccount.EVA_ModifiedOn = DateTime.Now;
            if (base.SecurityContext.SaveChanges() > 0)
                return true;

            else
                return false;
        }

        #endregion

        #region Map Regulatory Entity

        public List<Entity.lkpRegulatoryEntityType> FetchRegulatoryEntityTypeNotMapped(Int32 nDPMID)
        {
            List<Int16> lstRegulatoryEntityTypeMapped = _ClientDBContext.InstHierarchyRegulatoryEntityMappings.Where(cond => cond.IHRE_InstitutionHierarchyID == nDPMID && !cond.IHRE_IsDeleted).Select(obj => obj.IHRE_RegulatoryEntityTypeID).ToList();
            return base.SecurityContext.lkpRegulatoryEntityTypes.Where(cond => !lstRegulatoryEntityTypeMapped.Contains(cond.RET_ID) && !cond.RET_IsDeleted).ToList();
        }

        public Boolean SaveInstHierarchyRegulatoryEntityMapping(InstHierarchyRegulatoryEntityMapping instHierarchyRegEntity)
        {
            _ClientDBContext.InstHierarchyRegulatoryEntityMappings.AddObject(instHierarchyRegEntity);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get the list of External Vendor Account and Institution Hierarchy Vendor Account Mapping for a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public List<InstHierarchyRegulatoryEntityMappingDetails> GetInstHierarchyRegulatoryEntityMappingDetails(Int32 nDPMId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetInstHierarchyRegulatoryEntityMappingDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DPMID", nDPMId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                IEnumerable<DataRow> rows = ds.Tables[0].AsEnumerable();
                return rows.Select(col => new InstHierarchyRegulatoryEntityMappingDetails
                {
                    IHRE_ID = Convert.ToInt32(col["IHRE_ID"]),
                    IHRE_RegulatoryEntityTypeID = Convert.ToInt32(Convert.ToString(col["IHRE_RegulatoryEntityTypeID"])),
                    RET_Name = Convert.ToString(col["RET_Name"]),
                    IHRE_InstitutionHierarchyID = Convert.ToInt32(Convert.ToString(col["IHRE_InstitutionHierarchyID"]))
                }).ToList();
            }
        }

        public InstHierarchyRegulatoryEntityMapping GetInstHierarchyRegEntityMappingByID(Int32 mappingID)
        {
            return _ClientDBContext.InstHierarchyRegulatoryEntityMappings.Where(cond => cond.IHRE_ID == mappingID && !cond.IHRE_IsDeleted).FirstOrDefault();
        }

        #endregion

        #region Import ClearStar Services
        public IList<Entity.ExternalBkgSvc> FetchExternalBkgServices(Int32 VendorID)
        {
            return base.SecurityContext.ExternalBkgSvcs.Where(x => x.EBS_VendorId == VendorID && x.EBS_IsDeleted == false).ToList();
        }

        public String FetchExternalBkgServiceCodeByID(Int32 ExtSvcID)
        {
            return base.SecurityContext.ExternalBkgSvcs.Where(x => x.EBS_ID == ExtSvcID && x.EBS_IsDeleted == false).Select(obj => obj.EBS_ExternalCode).FirstOrDefault();
        }

        public IList<Entity.ExternalBkgSvcAttribute> FetchExternalBkgServiceAttributes(Int32 EBS_ID)
        {
            return base.SecurityContext.ExternalBkgSvcAttributes.Where(x => x.EBSA_SvcId == EBS_ID && x.EBSA_IsDeleted == false).ToList();
        }

        public IList<Entity.ClearStarService> FetchClearstarServices()
        {
            List<String> lstExternalbkgSVCs = base.SecurityContext.ExternalBkgSvcs.Where(x => !x.EBS_IsDeleted).Select(x => x.EBS_ExternalCode).ToList();
            return base.SecurityContext.ClearStarServices.Where(x => !(lstExternalbkgSVCs.Contains(x.CSS_Number))).ToList();
        }

        public Boolean ImportClearStarServices(Int32[] SelectedCssIds, Int32 VendorID, Int32 CurrentLoggedInUserID)
        {
            Boolean isExternalBkgSvcInserted = true;
            Entity.ExternalBkgSvcAttribute extBkgSvcAttribute = base.SecurityContext.ExternalBkgSvcAttributes.Where(x => !x.EBSA_IsDeleted).OrderByDescending(x => x.EBSA_DisplayOrder).FirstOrDefault();
            Int32? displayOrder = (extBkgSvcAttribute.IsNotNull()) ? extBkgSvcAttribute.EBSA_DisplayOrder + 1 : 1;
            Entity.ExternalBkgSvc externalBkgSvc = null;

            List<Entity.ExternalBkgSvc> lstExternalBkgSvc = new List<Entity.ExternalBkgSvc>();
            List<Entity.ClearStarService> clearStarServices = base.SecurityContext.ClearStarServices.Where(x => SelectedCssIds.Contains(x.CSS_ID)).ToList();
            foreach (Entity.ClearStarService item in clearStarServices)
            {
                externalBkgSvc = new Entity.ExternalBkgSvc();
                externalBkgSvc.EBS_Name = item.CSS_Name;
                externalBkgSvc.EBS_VendorId = VendorID;
                externalBkgSvc.EBS_ExternalCode = item.CSS_Number;
                externalBkgSvc.EBS_CreatedBy = CurrentLoggedInUserID;
                externalBkgSvc.EBS_CreatedOn = System.DateTime.Now;
                externalBkgSvc.EBS_IsDeleted = false;
                externalBkgSvc.EBS_Code = Guid.NewGuid();
                externalBkgSvc.EBS_CopiedFromCode = null;
                externalBkgSvc.ClearStarService_ID = item.CSS_ID;
                lstExternalBkgSvc.Add(externalBkgSvc);

                base.SecurityContext.ExternalBkgSvcs.AddObject(externalBkgSvc);
            }
            if (base.SecurityContext.SaveChanges() > 0)
            {
                isExternalBkgSvcInserted = true;
                List<Entity.ClearStarServiceField> clearStarFields = base.SecurityContext.ClearStarServiceFields.Where(x => SelectedCssIds.Contains(x.CSSF_ClearStarServiceID.Value)).ToList();
                foreach (Entity.ClearStarServiceField item in clearStarFields)
                {
                    var currentExternalBkgSvc = lstExternalBkgSvc.Where(x => x.ClearStarService_ID == item.CSSF_ClearStarServiceID).FirstOrDefault();
                    Entity.ExternalBkgSvcAttribute externalBkgSvcAttributes = new Entity.ExternalBkgSvcAttribute();
                    externalBkgSvcAttributes.EBSA_Name = item.CSSF_Name;
                    externalBkgSvcAttributes.EBSA_Description = null;
                    externalBkgSvcAttributes.EBSA_SvcId = currentExternalBkgSvc.EBS_ID; //item.CSSF_ClearStarServiceID.Value;
                    externalBkgSvcAttributes.EBSA_Code = Guid.NewGuid();
                    externalBkgSvcAttributes.EBSA_CopiedFromCode = null;
                    externalBkgSvcAttributes.EBSA_IsDeleted = false;
                    externalBkgSvcAttributes.EBSA_CreatedById = CurrentLoggedInUserID;
                    externalBkgSvcAttributes.EBSA_CreatedDate = System.DateTime.Now;
                    externalBkgSvcAttributes.EBSA_FieldID = item.CSSF_FieldID;
                    externalBkgSvcAttributes.EBSA_Label = item.CSSF_Label;
                    externalBkgSvcAttributes.EBSA_LocationField = item.CSSF_LocationField;
                    externalBkgSvcAttributes.EBSA_DefaultValue = item.CSSF_DefaultValue;
                    externalBkgSvcAttributes.EBSA_IsRequired = item.CSSF_IsRequired;
                    externalBkgSvcAttributes.EBSA_IsVisible = item.CSSF_IsVisible;
                    externalBkgSvcAttributes.EBSA_PurgeData = item.CSSF_PurgeData;
                    externalBkgSvcAttributes.EBSA_PurgeReplaceValue = item.CSSF_PurgeReplaceValue;
                    externalBkgSvcAttributes.EBSA_DisplayOrder = displayOrder;
                    base.SecurityContext.ExternalBkgSvcAttributes.AddObject(externalBkgSvcAttributes);
                    displayOrder = displayOrder + 1;
                }
                if (base.SecurityContext.SaveChanges() > 0)
                    return true;
            }
            if (isExternalBkgSvcInserted)
            {
                return true;
            }
            return false;
        }


        IEnumerable<Entity.ClearStarService> IBackgroundSetupRepository.FetchAllClearstarServices()
        {
            return base.SecurityContext.ClearStarServices;
        }

        Boolean IBackgroundSetupRepository.SaveClearStarSevices(List<Entity.ClearStarService> lstClearStarService)
        {
            lstClearStarService.ForEach(cond =>
            {
                base.SecurityContext.ClearStarServices.AddObject(cond);
            });
            base.SecurityContext.SaveChanges();
            return true;
        }

        #endregion

        #region Manage D & R Documents

        public Boolean SaveDisclosureTemplateDocument(List<Entity.SystemDocument> lstDisclosureDocuments)
        {
            foreach (Entity.SystemDocument sysDoc in lstDisclosureDocuments)
            {
                base.SecurityContext.SystemDocuments.AddObject(sysDoc);
            }

            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        public List<Entity.SystemDocument> GetDisclosureTemplateDocuments(Int32 docTypeID)
        {
            return base.SecurityContext.SystemDocuments.Where(x => x.SD_DocType_ID == docTypeID && !x.IsDeleted).ToList();
        }

        public Boolean DeleteDisclosureTemplateDocument(Int32 systemDocumentID, Int32 currentUserId)
        {
            Entity.SystemDocument sysDoc = base.SecurityContext.SystemDocuments.FirstOrDefault(condition => condition.SystemDocumentID == systemDocumentID);
            sysDoc.IsDeleted = true;
            sysDoc.ModifiedBy = currentUserId;
            sysDoc.ModifiedOn = DateTime.Now;

            //UAT-3745
            Entity.ExtBkgSvcDocumentMapping extBkgSvcDocumentMapping = base.SecurityContext.ExtBkgSvcDocumentMappings.FirstOrDefault(con => con.EBSDM_SystemDocumentID == systemDocumentID && !con.EBSDM_IsDeleted);
            if (!extBkgSvcDocumentMapping.IsNullOrEmpty())
            {
                extBkgSvcDocumentMapping.EBSDM_IsDeleted = true;
                extBkgSvcDocumentMapping.EBSDM_ModifiedBy = currentUserId;
                extBkgSvcDocumentMapping.EBSDM_ModifiedOn = DateTime.Now;
            }
            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        public Boolean UpdateDisclosureTemplateDocument(Entity.SystemDocument disclosureDocument, Int32 selectedBkgSvcId)
        {
            Entity.SystemDocument sysDoc = base.SecurityContext.SystemDocuments.FirstOrDefault(condition => condition.SystemDocumentID == disclosureDocument.SystemDocumentID);

            sysDoc.Description = disclosureDocument.Description;
            sysDoc.ModifiedBy = disclosureDocument.ModifiedBy;
            sysDoc.ModifiedOn = DateTime.Now;

            //UAT-3745
            if (!selectedBkgSvcId.IsNullOrEmpty() && selectedBkgSvcId > AppConsts.NONE)
            {
                Entity.ExtBkgSvcDocumentMapping extBkgSvcDocumentMapping = base.SecurityContext.ExtBkgSvcDocumentMappings.FirstOrDefault(cond => !cond.EBSDM_IsDeleted && cond.EBSDM_SystemDocumentID == disclosureDocument.SystemDocumentID);

                if (!extBkgSvcDocumentMapping.IsNullOrEmpty())
                {
                    extBkgSvcDocumentMapping.EBSDM_ExtBkgSvcID = selectedBkgSvcId;
                    extBkgSvcDocumentMapping.EBSDM_ModifiedBy = disclosureDocument.ModifiedBy;
                    extBkgSvcDocumentMapping.EBSDM_ModifiedOn = DateTime.Now; ;
                }
                else
                {
                    extBkgSvcDocumentMapping = new Entity.ExtBkgSvcDocumentMapping();
                    extBkgSvcDocumentMapping.EBSDM_DocumentTypeID = Convert.ToInt32(sysDoc.SD_DocType_ID);
                    extBkgSvcDocumentMapping.EBSDM_SystemDocumentID = sysDoc.SystemDocumentID;
                    extBkgSvcDocumentMapping.EBSDM_ExtBkgSvcID = selectedBkgSvcId;
                    extBkgSvcDocumentMapping.EBSDM_CreatedBy = Convert.ToInt32(disclosureDocument.ModifiedBy);
                    extBkgSvcDocumentMapping.EBSDM_CreatedOn = DateTime.Now;

                    base.SecurityContext.ExtBkgSvcDocumentMappings.AddObject(extBkgSvcDocumentMapping);
                }
            }
            else
            {
                //Delete code if selected ext bkg service is 0.
                Entity.ExtBkgSvcDocumentMapping extBkgSvcDocumentMapping = base.SecurityContext.ExtBkgSvcDocumentMappings.FirstOrDefault(con => con.EBSDM_SystemDocumentID == disclosureDocument.SystemDocumentID && !con.EBSDM_IsDeleted);
                if (!extBkgSvcDocumentMapping.IsNullOrEmpty())
                {
                    extBkgSvcDocumentMapping.EBSDM_IsDeleted = true;
                    extBkgSvcDocumentMapping.EBSDM_ModifiedBy = disclosureDocument.ModifiedBy;
                    extBkgSvcDocumentMapping.EBSDM_ModifiedOn = DateTime.Now;
                }
            }
            //End UAT-3745

            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region D And R AttributeGroup Mapping

        public List<DAndRAttributeGroupMappingContract> GetDAndRAttributeGroupMapping(Int32 systemDocumentID)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetDAndRAttributeGroupMappingByDocumentId]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SystemDocumentID", systemDocumentID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<DAndRAttributeGroupMappingContract> lstDAndRAttributeGroupMappingContract = new List<DAndRAttributeGroupMappingContract>();
                lstDAndRAttributeGroupMappingContract = ds.Tables[0].AsEnumerable().Select(col =>
                      new DAndRAttributeGroupMappingContract
                      {
                          Attribute = col["Attribute"] == DBNull.Value ? String.Empty : Convert.ToString(col["Attribute"]),
                          AttributeGroup = col["AttributeGroup"] == DBNull.Value ? String.Empty : Convert.ToString(col["AttributeGroup"]),
                          FieldName = col["FieldName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FieldName"]),
                          ID = col["ID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["ID"]),
                          SvcAttGroupID = col["SvcAttGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SvcAttGroupID"]),
                          SvcAttrID = col["SvcAttrID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SvcAttrID"]),
                          AttributeGroupMappingID = col["AttributeGroupMappingID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["AttributeGroupMappingID"]),
                          SpecialFieldTypeID = col["SpecialFieldTypeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["SpecialFieldTypeID"]),
                          SpecialFieldTypeName = col["SpecialFieldTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SpecialFieldTypeName"]),
                          TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                          TenantID = col["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TenantID"]),
                          CustomAttributeID = col["CustomAttributeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["CustomAttributeID"])
                      }).ToList();

                return lstDAndRAttributeGroupMappingContract;
            }
        }

        IQueryable<Entity.BkgSvcAttributeGroup> IBackgroundSetupRepository.GetServiceAttributeGroup()
        {
            //UAT-1744:Forms filled out at the time of order should be able to pull in data from custom forms within the order.
            //List<Guid> codes = new List<Guid> { new Guid("CC184FC4-5401-445D-90AA-E77167227904"), new Guid("338F1CA2-6B0A-43C1-B900-A8F6B058678F"), new Guid("CF76960D-2120-46FE-9E03-01C218F8A336"), new Guid("A26014A2-ED57-47CC-B68F-17C02E376D60") };  //4E568549-FEBE-4918-8561-A99F49AC543D
            //return base.SecurityContext.BkgSvcAttributeGroups.Where(cond => codes.Contains(cond.BSAD_Code));

            return base.SecurityContext.BkgSvcAttributeGroups.Where(cond => !cond.BSAD_IsDeleted);
        }

        IQueryable<Entity.BkgSvcAttribute> IBackgroundSetupRepository.GetServiceAttributeByServiceGroupID(Int32 BkgSvcAGID)
        {
            List<Int32> lstSvcAttributeID = base.SecurityContext.BkgAttributeGroupMappings.Where(cond => cond.BAGM_BkgSvcAttributeGroupId == BkgSvcAGID && !cond.BAGM_IsDeleted)
                           .Select(cond => cond.BAGM_BkgSvcAtributeID).ToList();
            return base.SecurityContext.BkgSvcAttributes.Where(cond => lstSvcAttributeID.Contains(cond.BSA_ID));
        }

        //Boolean IBackgroundSetupRepository.UpdateMapping(Int32 systemDocumentID, Int32 bkgSvcAttributeGroupID, Int32 bkgSvcAttributeID, Int32 currentLoggedInUserID, Int32 specialFieldType_ID, Boolean rbApplicantAttr)
        Boolean IBackgroundSetupRepository.UpdateMapping(DAndRAttributeGroupMappingContract dAndRContract, Int32 currentLoggedInUserID)
        {
            Entity.BkgAttributeGroupMapping bkgAttributeGroupMapping = base.SecurityContext.BkgAttributeGroupMappings
                                .Where(cond => cond.BAGM_BkgSvcAttributeGroupId == dAndRContract.SvcAttGroupID
                                    && cond.BAGM_BkgSvcAtributeID == dAndRContract.SvcAttrID
                                 && !cond.BAGM_IsDeleted).FirstOrDefault();
            Entity.SysDocumentFieldMapping sysDocumentFieldMapping = base.SecurityContext.SysDocumentFieldMappings
                                        .Where(cond => cond.SDFM_ID == dAndRContract.ID).FirstOrDefault();

            if (dAndRContract.IsApplicantAttribute)
            {
                if (!bkgAttributeGroupMapping.IsNull())
                {
                    sysDocumentFieldMapping.SDFM_AttributeGroupMappingID = bkgAttributeGroupMapping.BAGM_ID;
                    sysDocumentFieldMapping.SDFM_ModifiedBy = currentLoggedInUserID;
                    sysDocumentFieldMapping.SDFM_SpecialFieldTypeID = null;
                    sysDocumentFieldMapping.SDFM_TenantID = null;
                    sysDocumentFieldMapping.SDFM_CustomAttributeID = null;
                    sysDocumentFieldMapping.SDFM_ModifiedOn = DateTime.Now;
                    base.SecurityContext.SaveChanges();
                    return true;
                }
            }
            else if (dAndRContract.IsCustomAttribute)
            {
                sysDocumentFieldMapping.SDFM_AttributeGroupMappingID = null;
                sysDocumentFieldMapping.SDFM_SpecialFieldTypeID = null;
                sysDocumentFieldMapping.SDFM_TenantID = dAndRContract.TenantID;
                sysDocumentFieldMapping.SDFM_CustomAttributeID = dAndRContract.CustomAttributeID;
                sysDocumentFieldMapping.SDFM_ModifiedBy = currentLoggedInUserID;
                sysDocumentFieldMapping.SDFM_ModifiedOn = DateTime.Now;
                base.SecurityContext.SaveChanges();
                return true;
            }
            else
            {
                if (!sysDocumentFieldMapping.IsNull())
                {
                    sysDocumentFieldMapping.SDFM_SpecialFieldTypeID = dAndRContract.SpecialFieldTypeID;
                    sysDocumentFieldMapping.SDFM_AttributeGroupMappingID = null;
                    sysDocumentFieldMapping.SDFM_TenantID = null;
                    sysDocumentFieldMapping.SDFM_CustomAttributeID = null;
                    sysDocumentFieldMapping.SDFM_ModifiedBy = currentLoggedInUserID;
                    sysDocumentFieldMapping.SDFM_ModifiedOn = DateTime.Now;
                    base.SecurityContext.SaveChanges();
                    return true;
                }
            }


            return false;
        }

        #endregion

        #region Disclosure And Release
        public List<Entity.SystemDocument> GetDAndRDocuments(List<string> Countries, List<string> States, int? HierarchyNodeID, String RegulatoryNodeIDs, String BkgServiceIds,
                                                             Int32 TenantId, String disclosureDocAgeGroupType)
        {
            StringBuilder Country = new StringBuilder();
            foreach (String item in Countries.Distinct())
            {
                Country.Append(item);
                Country.Append(",");
            }

            StringBuilder State = new StringBuilder();
            foreach (String item in States.Distinct())
            {
                State.Append(item);
                State.Append(",");
            }

            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetDisclosureAndReleaseDocuments]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Country", Country.ToString());
                command.Parameters.AddWithValue("@State", State.ToString());
                command.Parameters.AddWithValue("@Services", BkgServiceIds);
                command.Parameters.AddWithValue("@HierarchyNodeID", HierarchyNodeID);
                command.Parameters.AddWithValue("@RegulatoryNodeIDs", RegulatoryNodeIDs);
                command.Parameters.AddWithValue("@TenantId", TenantId);
                command.Parameters.AddWithValue("@DisclosureDocAgeGroupTypeCode", disclosureDocAgeGroupType);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                IEnumerable<DataRow> rows = ds.Tables[0].AsEnumerable();
                return rows.Select(col => new Entity.SystemDocument
                {
                    SystemDocumentID = Convert.ToInt32(col["SystemDocumentID"]),
                    DocumentPath = Convert.ToString(col["DocumentPath"]),
                    FileName = Convert.ToString(col["FileName"])
                }).ToList();
            }
        }

        public List<Int32> GetServicesIds(List<Int32> BkgPackages)
        {
            List<Int32> lstBkgPackageSvcGroupIds = _ClientDBContext.BkgPackageSvcGroups
                                                .Where(x => (BkgPackages.Contains(x.BPSG_BackgroundPackageID) && x.BPSG_IsDeleted == false)).Select(cond => cond.BPSG_ID).ToList();
            if (lstBkgPackageSvcGroupIds.IsNotNull() && lstBkgPackageSvcGroupIds.Count > 0)
            {
                return _ClientDBContext.BkgPackageSvcs.Where(cond => lstBkgPackageSvcGroupIds.Contains(cond.BPS_BkgPackageSvcGroupID) && cond.BPS_IsDeleted == false).
                    Select(x => x.BPS_BackgroundServiceID).ToList();
            }
            return null;
        }

        public List<SysDocumentFieldMappingContract> GetFieldNames(Dictionary<Int32, String> DictAttributeGroup, List<Entity.SystemDocument> DocumentList, List<TypeCustomAttributes> lstCustomAttributes, Int32 tenantID)
        {
            Dictionary<String, String> fieldMapList = new Dictionary<string, string>();
            List<SysDocumentFieldMappingContract> documentFieldMapping = new List<SysDocumentFieldMappingContract>();
            List<Entity.lkpDisclosureDocumentSpecialFieldType> lstSpecialFields = base.SecurityContext.lkpDisclosureDocumentSpecialFieldTypes.Where(cond => cond.DDSFT_IsDeleted == false).ToList();
            List<Entity.SysDocumentFieldMapping> sysDocumentFieldMappingAllRecords = base.SecurityContext.SysDocumentFieldMappings.Where(obj => obj.SDFM_IsDeleted == false).ToList();

            foreach (Entity.SystemDocument document in DocumentList)
            {
                foreach (var keyValue in DictAttributeGroup)
                {
                    //List<Entity.SysDocumentFieldMapping> sysDocumentFieldMapping = base.SecurityContext.SysDocumentFieldMappings.Where(obj => obj.SDFM_SystemDocumentID == document.SystemDocumentID && obj.SDFM_AttributeGroupMappingID == keyValue.Key && obj.SDFM_IsDeleted == false).ToList();
                    List<Entity.SysDocumentFieldMapping> sysDocumentFieldMapping = sysDocumentFieldMappingAllRecords.Where(obj => obj.SDFM_SystemDocumentID == document.SystemDocumentID && obj.SDFM_AttributeGroupMappingID == keyValue.Key && obj.SDFM_IsDeleted == false).ToList();
                    foreach (Entity.SysDocumentFieldMapping documentFieldMappings in sysDocumentFieldMapping)
                    {
                        documentFieldMapping.Add(new SysDocumentFieldMappingContract
                        {
                            ID = documentFieldMappings.SDFM_SystemDocumentID,
                            FieldName = documentFieldMappings.SDFM_FieldName,
                            FieldValue = keyValue.Value,
                            DocumentPath = document.DocumentPath,
                            SpecialFieldTypeID = documentFieldMappings.SDFM_SpecialFieldTypeID,
                            SpecialFieldTypeName = lstSpecialFields.Where(x => x.DDSFT_ID == documentFieldMappings.SDFM_SpecialFieldTypeID).Select(x => x.DDSFT_Name).FirstOrDefault(),
                            SpecialFieldTypeCode = lstSpecialFields.Where(x => x.DDSFT_ID == documentFieldMappings.SDFM_SpecialFieldTypeID).Select(x => x.DDSFT_Code).FirstOrDefault(),
                        });
                    }

                }
                //List<Entity.SysDocumentFieldMapping> sysDocumentSpecialFieldMapping = base.SecurityContext.SysDocumentFieldMappings.Where(obj => obj.SDFM_SystemDocumentID == document.SystemDocumentID && obj.SDFM_SpecialFieldTypeID != null && obj.SDFM_IsDeleted == false).ToList();
                List<Entity.SysDocumentFieldMapping> sysDocumentSpecialFieldMapping = sysDocumentFieldMappingAllRecords.Where(obj => obj.SDFM_SystemDocumentID == document.SystemDocumentID && obj.SDFM_SpecialFieldTypeID != null && obj.SDFM_IsDeleted == false).ToList();
                foreach (Entity.SysDocumentFieldMapping documentFieldMappings in sysDocumentSpecialFieldMapping)
                {
                    documentFieldMapping.Add(new SysDocumentFieldMappingContract
                    {
                        ID = documentFieldMappings.SDFM_SystemDocumentID,
                        FieldName = documentFieldMappings.SDFM_FieldName,
                        DocumentPath = document.DocumentPath,
                        SpecialFieldTypeID = documentFieldMappings.SDFM_SpecialFieldTypeID,
                        SpecialFieldTypeName = lstSpecialFields.Where(x => x.DDSFT_ID == documentFieldMappings.SDFM_SpecialFieldTypeID).Select(x => x.DDSFT_Name).FirstOrDefault(),
                        SpecialFieldTypeCode = lstSpecialFields.Where(x => x.DDSFT_ID == documentFieldMappings.SDFM_SpecialFieldTypeID).Select(x => x.DDSFT_Code).FirstOrDefault(),
                    });
                }

                List<Entity.SysDocumentFieldMapping> sysDocumentCustomAttributeMapping = sysDocumentFieldMappingAllRecords
                                                            .Where(obj => obj.SDFM_SystemDocumentID == document.SystemDocumentID
                                                                && obj.SDFM_TenantID != null
                                                                && obj.SDFM_TenantID == tenantID
                                                                && obj.SDFM_CustomAttributeID != null
                                                                && obj.SDFM_IsDeleted == false).ToList();

                foreach (TypeCustomAttributes customAttribute in lstCustomAttributes)
                {
                    List<Entity.SysDocumentFieldMapping> lstFieldMapping = sysDocumentCustomAttributeMapping
                                                                            .Where(cond => cond.SDFM_CustomAttributeID == customAttribute.CAId).ToList();
                    foreach (Entity.SysDocumentFieldMapping fieldMapping in lstFieldMapping)
                    {
                        SysDocumentFieldMappingContract sysDocFieldContract = new SysDocumentFieldMappingContract();
                        sysDocFieldContract.ID = fieldMapping.SDFM_SystemDocumentID;
                        sysDocFieldContract.FieldName = fieldMapping.SDFM_FieldName;
                        if (customAttribute.CADataTypeCode.ToLower() == CustomAttributeDatatype.Boolean.GetStringValue().ToLower())
                        {
                            if (customAttribute.CAValue == "0")
                            {
                                sysDocFieldContract.FieldValue = "NA";
                            }
                            else if (customAttribute.CAValue == "1")
                            {
                                sysDocFieldContract.FieldValue = "Yes";
                            }
                            else
                            {
                                sysDocFieldContract.FieldValue = "No";
                            }
                        }


                        else
                        {
                            sysDocFieldContract.FieldValue = customAttribute.CAValue;
                        }
                        sysDocFieldContract.DocumentPath = document.DocumentPath;
                        sysDocFieldContract.SpecialFieldTypeID = fieldMapping.SDFM_SpecialFieldTypeID;
                        sysDocFieldContract.SpecialFieldTypeName = lstSpecialFields.Where(x => x.DDSFT_ID == fieldMapping.SDFM_SpecialFieldTypeID).Select(x => x.DDSFT_Name).FirstOrDefault();
                        sysDocFieldContract.SpecialFieldTypeCode = lstSpecialFields.Where(x => x.DDSFT_ID == fieldMapping.SDFM_SpecialFieldTypeID).Select(x => x.DDSFT_Code).FirstOrDefault();
                        documentFieldMapping.Add(sysDocFieldContract);
                    }
                }

            }
            return documentFieldMapping;
        }
        #endregion

        #region Manage Service Item Custom Forms Mappings

        public List<Entity.CustomForm> GetSupplCustomFrmsNotMappedToSvcItem(Int32 pkgSvcItemID)
        {
            List<Entity.CustomForm> lstSupplCustomForms = null;
            String Code = CustomFormType.Supplement_Order_Form.GetStringValue();
            List<Int32> lstCustomFormIds = _ClientDBContext.BkgSvcItemFormMappings.Where(cond => cond.BSIFM_PackageServiceItemID == pkgSvcItemID && !cond.BSIFM_IsDeleted).Select(obj => obj.BSIFM_CustomFormID).ToList();
            if (lstCustomFormIds.Count > 0)
                lstSupplCustomForms = base.SecurityContext.CustomForms.Include("lkpCustomFormType").Where(cond => cond.lkpCustomFormType.CFT_Code == Code && !lstCustomFormIds.Contains(cond.CF_ID) && !cond.CF_IsDeleted).ToList();
            else
                lstSupplCustomForms = base.SecurityContext.CustomForms.Include("lkpCustomFormType").Where(cond => cond.lkpCustomFormType.CFT_Code == Code && !cond.CF_IsDeleted).ToList();

            return lstSupplCustomForms;
        }

        public BkgSvcItemFormMapping GetBkgSvcItemFormMappingById(Int32 bkgSvcItemFormMappingId)
        {
            return _ClientDBContext.BkgSvcItemFormMappings.Where(cond => cond.BSIFM_ID == bkgSvcItemFormMappingId && !cond.BSIFM_IsDeleted).FirstOrDefault();
        }

        public Boolean SaveBkgSvcItemFormMapping(BkgSvcItemFormMapping objBkgSvcItemFormMapping)
        {
            _ClientDBContext.BkgSvcItemFormMappings.AddObject(objBkgSvcItemFormMapping);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get the list of Pkg Service Item Custom Form Mapping Details for a given service item
        /// </summary>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public List<PkgServiceItemCustomFormMappingDetails> GetPkgServiceItemCustomFormMappingDetails(Int32 pkgServiceItemId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetPkgServiceItemCustomFormMappingDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PkgServiceItemID", pkgServiceItemId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                IEnumerable<DataRow> rows = ds.Tables[0].AsEnumerable();
                return rows.Select(col => new PkgServiceItemCustomFormMappingDetails
                {
                    BSIFM_ID = Convert.ToInt32(col["BSIFM_ID"]),
                    CF_ID = Convert.ToInt32(col["CF_ID"]),
                    CF_Title = Convert.ToString(col["CF_Title"]),
                    CF_Name = Convert.ToString(col["CF_Name"]),
                    CF_Description = Convert.ToString(col["CF_Description"]),
                    CF_IsEditable = Convert.ToBoolean(col["CF_IsEditable"]),
                    CF_Sequence = Convert.ToInt32(col["CF_Sequence"]),
                    CF_CustomFormTypeID = Convert.ToInt32(col["CF_CustomFormTypeID"]),
                    CFT_Code = Convert.ToString(col["CFT_Code"]),
                    CFT_Name = Convert.ToString(col["CFT_Name"])

                }).ToList();
            }
        }

        #endregion

        /// <summary>
        /// Method is used to check the contact with supplied email exists or not
        /// </summary>
        /// <param name="contactEmailAddress"></param>
        /// <returns></returns>
        Boolean IBackgroundSetupRepository.IsContactExists(String contactEmailAddress, Int32 contactID = AppConsts.NONE)
        {
            if (contactID == AppConsts.NONE)
            {
                return _ClientDBContext.InstitutionContacts.Any(cond => cond.ICO_PrimaryEmailAddress == contactEmailAddress && !cond.ICO_IsDeleted);
            }
            else
            {
                Int32 existingContactID = _ClientDBContext.InstitutionContacts.Where(cond => cond.ICO_PrimaryEmailAddress == contactEmailAddress && !cond.ICO_IsDeleted).Select(cond => cond.ICO_ID).FirstOrDefault();
                if (contactID == existingContactID)
                {
                    return false;
                }
                else if (existingContactID > AppConsts.NONE)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }


        #region Special Field Type for D & R
        public List<Entity.lkpDisclosureDocumentSpecialFieldType> GetSpecialFieldType()
        {
            return base.SecurityContext.lkpDisclosureDocumentSpecialFieldTypes.Where(cond => cond.DDSFT_IsDeleted == false).ToList();
        }
        #endregion

        #region Copy Background Package
        public Boolean CheckIfPackageNameAlreadyExist(String packageName)
        {
            return _ClientDBContext.BackgroundPackages.Any(x => x.BPA_Name.Equals(packageName) && !x.BPA_IsDeleted);
        }

        public String CopyBackgroundPackage(Int32 sourceNodeId, Int32 targetNodeId, Int32 sourceBPHMId, String targetPackageName, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CopyBackgroundPackage", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SourceNodeId", sourceNodeId);
                command.Parameters.AddWithValue("@TargetNodeId", targetNodeId);
                command.Parameters.AddWithValue("@OldBPHMId", sourceBPHMId);
                command.Parameters.AddWithValue("@CurrentUserId", currentLoggedInUserId);
                command.Parameters.AddWithValue("@PackageName", targetPackageName);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return "Package copied sucessfully";
            }
        }

        #endregion

        public Boolean CheckIfVendorNameAlreadyExist(String vendorName, Int32 vendorID)
        {
            return base.SecurityContext.ExternalVendors.Any(obj => obj.EVE_Name.Trim().ToUpper() == vendorName.Trim().ToUpper() && obj.EVE_ID != vendorID && !obj.EVE_IsDeleted);
        }

        /// <summary>
        /// Check if the PackageServiceItem is Quanity group of any another ServiceItem
        /// </summary>
        /// <param name="pkgSvcItemId"></param>
        /// <returns></returns>
        public Boolean IsPackageServiceItemEditable(Int32 pkgSvcItemId)
        {
            if (_ClientDBContext.PackageServiceItems.Where(psi => psi.PSI_QuantityGroup == pkgSvcItemId
                    && psi.PSI_ID != pkgSvcItemId // To avoid the Item which is SELF Quantity group
                    && !psi.PSI_IsDeleted).Any())
                return false;
            else
                return true;
        }

        #region Manage Payment Option Instruction
        /// <summary>
        /// Get lkpPaymentOption from Security
        /// </summary>
        /// <returns>Security Payment Options</returns>
        public List<Entity.lkpPaymentOption> GetSecurityPaymentOptions()
        {
            String OfflineSettlement = PaymentOptions.OfflineSettlement.GetStringValue();
            return base.SecurityContext.lkpPaymentOptions.Where(obj => obj.IsDeleted == false && !obj.Code.Equals(OfflineSettlement)).ToList();
        }

        /// <summary>
        /// Get lkpPaymentOption from Security by Id
        /// </summary>
        /// <param name="paymentOptionId"></param>
        /// <returns>Security Payment Option Id</returns>
        public Entity.lkpPaymentOption GetSecurityPaymentOptionById(Int32 paymentOptionId)
        {
            return base.SecurityContext.lkpPaymentOptions.Where(obj => obj.IsDeleted == false && obj.PaymentOptionID == paymentOptionId).FirstOrDefault();
        }

        /// <summary>
        /// Update Security after modifying record.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateSecurityChanges()
        {
            if (base.SecurityContext.SaveChanges() > 0)
                return true;
            return false;
        }


        #endregion

        #region Manual Service Forms
        /// <summary>
        /// Get all the service in Tenant
        /// </summary>
        /// <returns></returns>
        public List<BackgroundService> GetTenantServices()
        {
            List<BackgroundService> tenantServices = _ClientDBContext.BackgroundServices.Where(x => !x.BSE_IsDeleted).ToList();
            return tenantServices;
        }

        public Boolean UpdateOrderServiceServiceFormStatus(Int32 orderServiceFormId, Int32 statusId, Int32 currentLoggedInUserId)
        {
            BkgOrderServiceForm bkgOrderServiceForm = _ClientDBContext.BkgOrderServiceForms.Where(cond => cond.OSF_ID == orderServiceFormId && !cond.OSF_IsDeleted).FirstOrDefault();
            if (!bkgOrderServiceForm.IsNullOrEmpty())
            {
                bkgOrderServiceForm.OSF_ServiceFormStatusID = statusId;
                bkgOrderServiceForm.OSF_ModifiedBy = currentLoggedInUserId;
                bkgOrderServiceForm.OSF_ModifiedOn = DateTime.UtcNow;
            }
            _ClientDBContext.SaveChanges();
            return true;
        }

        #endregion

        public Boolean CheckIfOrderClientStatusIsUsed(Int32 orderClientStatusId)
        {
            return _ClientDBContext.BkgOrders.Any(cond => cond.BOR_BkgOrderClientStatus == orderClientStatusId && !cond.BOR_IsDeleted);
        }


        #region BkgCompl Package Data Mapping
        public List<BackgroundPackage> GetPermittedBackgroundPackagesByUserID()
        {
            return _ClientDBContext.BackgroundPackages.Where(cond => cond.BPA_IsDeleted == false).ToList();
        }

        public List<LookupContract> GetServiceGroupsForPackage(Int32 selectedPackageId)
        {
            List<BkgPackageSvcGroup> tmpLst = _ClientDBContext.BkgPackageSvcGroups.Include("BkgSvcGroup").
                   Where(cond => cond.BPSG_BackgroundPackageID == selectedPackageId && !cond.BPSG_IsDeleted).ToList();

            List<LookupContract> serviceGrpList = new List<LookupContract>();
            foreach (BkgPackageSvcGroup item in tmpLst)
            {
                LookupContract serviceGroup = new LookupContract();
                serviceGroup.ID = item.BkgSvcGroup.BSG_ID;
                serviceGroup.Name = item.BkgSvcGroup.BSG_Name;
                serviceGrpList.Add(serviceGroup);
            }
            return serviceGrpList;
        }

        public List<LookupContract> GetServicesForSvcGroup(int selectedServiceGrp, Int32 SelectedBkgPkgID)
        {
            List<int> bpsg = _ClientDBContext.BkgPackageSvcGroups.Where(x => x.BPSG_BackgroundPackageID == SelectedBkgPkgID && x.BPSG_BkgSvcGroupID == selectedServiceGrp && !x.BPSG_IsDeleted).Select(x => x.BPSG_ID).ToList();
            List<BkgPackageSvc> tmpLst = _ClientDBContext.BkgPackageSvcs.Include("BackgroundService").Where(x => bpsg.Contains(x.BPS_BkgPackageSvcGroupID) && x.BPS_IsDeleted == false && x.BackgroundService.BSE_IsDeleted == false).ToList();
            //Where(cond => cond.BPS_BkgPackageSvcGroupID == selectedServiceGrp && cond.BPS_IsDeleted == false).ToList();

            //var result = _ClientDBContext.BkgPackageSvcs.Join(_ClientDBContext.BkgPackageSvcGroups,
            //                                                  exp1 => exp1.BPS_BkgPackageSvcGroupID,
            //                                                  exp2 => exp2.BPSG_ID,
            //                                                  (exp1, exp2) => new { exp1, exp2 })
            //                                                  .Where( exp=> exp.exp2.BPSG_ID == selectedServiceGrp)
            //                                                  .ToList();

            List<LookupContract> servicesList = new List<LookupContract>();
            foreach (BkgPackageSvc item in tmpLst)
            {
                LookupContract serviceGroup = new LookupContract();
                serviceGroup.ID = item.BackgroundService.BSE_ID;
                serviceGroup.Name = item.BackgroundService.BSE_Name;
                servicesList.Add(serviceGroup);
            }
            return servicesList;
        }

        public List<LookupContract> GetServiceItemsForSvc(int selectedService)
        {
            //List<PackageServiceItem> tmpLst = _ClientDBContext.BkgPackageSvcs.Include("BackgroundService").
            //        Where(cond => cond.BPS_BkgPackageSvcGroupID == selectedServiceGrp && cond.BPS_IsDeleted == false).ToList();

            List<LookupContract> servicesList = new List<LookupContract>();
            //foreach (BkgPackageSvc item in tmpLst)
            //{
            //    LookupContract serviceGroup = new LookupContract();
            //    serviceGroup.ID = item.BPS_ID;
            //    serviceGroup.Name = item.BackgroundService.BSE_Name;
            //    servicesList.Add(serviceGroup);
            //}
            return servicesList;
        }

        public List<LookupContract> GetComplianceCatagories(int selectedComplPkgID)
        {
            List<CompliancePackageCategory> tmpLst = _ClientDBContext.CompliancePackageCategories.Include("ComplianceCategory").
                    Where(cond => cond.CPC_PackageID == selectedComplPkgID && cond.CPC_IsDeleted == false).ToList();

            List<LookupContract> catagoryList = new List<LookupContract>();
            foreach (CompliancePackageCategory item in tmpLst)
            {
                LookupContract serviceGroup = new LookupContract();
                serviceGroup.ID = item.ComplianceCategory.ComplianceCategoryID;
                serviceGroup.Name = item.ComplianceCategory.CategoryName;
                catagoryList.Add(serviceGroup);
            }
            return catagoryList;
        }

        public List<LookupContract> GetCatagoryItems(int selectedCatagoryID)
        {
            List<ComplianceCategoryItem> tmpLst = _ClientDBContext.ComplianceCategoryItems.Include("ComplianceItem").
                    Where(cond => cond.CCI_CategoryID == selectedCatagoryID && cond.CCI_IsDeleted == false).ToList();

            List<LookupContract> catagoryItemsList = new List<LookupContract>();
            foreach (ComplianceCategoryItem item in tmpLst)
            {
                //UAT-3077
                if (!item.ComplianceItem.IsPaymentType.Value)
                {
                    LookupContract serviceGroup = new LookupContract();
                    serviceGroup.ID = item.ComplianceItem.ComplianceItemID;
                    serviceGroup.Name = item.ComplianceItem.Name;
                    catagoryItemsList.Add(serviceGroup);
                }
            }
            return catagoryItemsList;
        }

        public List<ComplianceItemAttribute> GetComplianceItemAttributes(int selectedItemID)
        {
            return _ClientDBContext.ComplianceItemAttributes.Include("ComplianceAttribute")
                    .Where(cond => cond.CIA_ItemID == selectedItemID && cond.CIA_IsDeleted == false).ToList();


        }

        public DataTable FetchBkgCompliancePackageMapping(Int32 bkgPackageId, Int32 compPackageId, CustomPagingArgsContract gridCustomPaging)
        {
            //string orderBy = null;
            //string ordDirection = null;
            Int32? backgroundPackageId = null;
            Int32? compliancePackageId = null;


            //orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? null : gridCustomPaging.SortExpression;
            //ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";
            backgroundPackageId = bkgPackageId == AppConsts.NONE ? (Int32?)null : bkgPackageId;
            compliancePackageId = compPackageId == AppConsts.NONE ? (Int32?)null : compPackageId;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgComplianceDataSyncMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BkgPackageID", backgroundPackageId);
                command.Parameters.AddWithValue("@CompPackageID", compliancePackageId);
                //command.Parameters.AddWithValue("@OrderBy", orderBy);
                //command.Parameters.AddWithValue("@OrderDirection", ordDirection);
                //command.Parameters.AddWithValue("@PageIndex", gridCustomPaging.CurrentPageIndex);
                //command.Parameters.AddWithValue("@PageSize", gridCustomPaging.PageSize);
                command.Parameters.AddWithValue("@sortingAndFilteringData", gridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gridCustomPaging.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        gridCustomPaging.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    return ds.Tables[1];
                    // return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        public int GetComplianceAttributeDataTypeID(string ComplianceAttributeDataTypeCode)
        {
            return _ClientDBContext.lkpComplianceAttributeDatatypes.Where(cond => cond.Code == ComplianceAttributeDataTypeCode).FirstOrDefault().ComplianceAttributeDatatypeID;
        }

        public int GetDataPointTypeID(string DatapointTypeCode)
        {
            return _ClientDBContext.lkpBkgDataPointTypes.Where(cond => cond.BDPT_Code == DatapointTypeCode).FirstOrDefault().BDPT_ID;
        }

        public Boolean SaveBkgComplPkgDataMapping(BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract, Int32 currentLoggedInUserId)
        {
            Boolean isMappingSaved = false;
            Int32 dataPointID = GetDataPointTypeID(bkgComplPkgDataMappingContract.DataPointCode);

            Entity.ClientEntity.BkgCompliancePackageMapping bkgCompliancePackageMapping = new Entity.ClientEntity.BkgCompliancePackageMapping();
            bkgCompliancePackageMapping.BCPM_BkgPackageID = bkgComplPkgDataMappingContract.BkgPackageID;
            bkgCompliancePackageMapping.BCPM_BkgDataPointTypeID = dataPointID;
            bkgCompliancePackageMapping.BCPM_BkgSvcGroupID = bkgComplPkgDataMappingContract.ServiceGroupID;
            bkgCompliancePackageMapping.BCPM_BkgSvcID = bkgComplPkgDataMappingContract.ServiceID;
            bkgCompliancePackageMapping.BCPM_PackageSvcItemID = null;
            bkgCompliancePackageMapping.BCPM_CompliancePkgID = bkgComplPkgDataMappingContract.ComplPackageID.Value;
            bkgCompliancePackageMapping.BCPM_ComplianceCategoryID = bkgComplPkgDataMappingContract.CatagoryID.Value;
            bkgCompliancePackageMapping.BCPM_ComplianceItemID = bkgComplPkgDataMappingContract.ItemID.Value;
            bkgCompliancePackageMapping.BCPM_ComplianceAttributeID = bkgComplPkgDataMappingContract.AttributeID.Value;
            bkgCompliancePackageMapping.BCPM_IsDeleted = false;
            bkgCompliancePackageMapping.BCPM_CreatedByID = currentLoggedInUserId;
            bkgCompliancePackageMapping.BCPM_CreatedOn = System.DateTime.UtcNow;
            _ClientDBContext.BkgCompliancePackageMappings.AddObject(bkgCompliancePackageMapping);
            if (_ClientDBContext.SaveChanges() > 0)
            {
                isMappingSaved = true;
            }
            if (isMappingSaved && !bkgComplPkgDataMappingContract.NonFlaggedValue.IsNullOrEmpty() && !bkgComplPkgDataMappingContract.FlaggedValue.IsNullOrEmpty())
            {
                for (int i = 0; i < AppConsts.TWO; i++)
                {
                    Entity.ClientEntity.BkgCompliancePkgMappingAttrOption bkgCompliancePkgMappingAttrOption = new Entity.ClientEntity.BkgCompliancePkgMappingAttrOption();
                    bkgCompliancePkgMappingAttrOption.BCPAO_BkgComplaincePkgMappingID = bkgCompliancePackageMapping.BCPM_ID;
                    if (i == AppConsts.NONE)
                    {
                        bkgCompliancePkgMappingAttrOption.BCPAO_BKgValue = "1";
                        bkgCompliancePkgMappingAttrOption.BCPAO_ComplianceAttrOptionValue = bkgComplPkgDataMappingContract.FlaggedValue;
                    }
                    if (i == AppConsts.ONE)
                    {
                        bkgCompliancePkgMappingAttrOption.BCPAO_BKgValue = "0";
                        bkgCompliancePkgMappingAttrOption.BCPAO_ComplianceAttrOptionValue = bkgComplPkgDataMappingContract.NonFlaggedValue;
                    }
                    bkgCompliancePkgMappingAttrOption.BCPAO_CreatedByID = currentLoggedInUserId;
                    bkgCompliancePkgMappingAttrOption.BCPAO_CreatedOn = System.DateTime.UtcNow;
                    bkgCompliancePkgMappingAttrOption.BCPAO_IsDeleted = false;
                    _ClientDBContext.BkgCompliancePkgMappingAttrOptions.AddObject(bkgCompliancePkgMappingAttrOption);
                    _ClientDBContext.SaveChanges();
                }
                return true;
            }
            if (isMappingSaved)
            {
                return true;
            }
            return false;
        }

        public String DeleteBkgComplPkgDataMapping(int BCPM_ID, int currentLoggedInUserId)
        {
            List<Entity.ClientEntity.BkgCompliancePkgMappingAttrOption> bkgCompliancePkgMappingAttrOptionList = _ClientDBContext.BkgCompliancePkgMappingAttrOptions.Where(x => x.BCPAO_BkgComplaincePkgMappingID == BCPM_ID && x.BCPAO_IsDeleted == false).ToList();
            if (bkgCompliancePkgMappingAttrOptionList.Count > AppConsts.NONE)
            {
                foreach (Entity.ClientEntity.BkgCompliancePkgMappingAttrOption item in bkgCompliancePkgMappingAttrOptionList)
                {
                    item.BCPAO_IsDeleted = true;
                    item.BCPAO_ModifiedByID = currentLoggedInUserId;
                    item.BCPAO_ModifiedOn = System.DateTime.UtcNow;
                    _ClientDBContext.SaveChanges();
                }
            }
            Entity.ClientEntity.BkgCompliancePackageMapping bkgCompliancePackageMapping = _ClientDBContext.BkgCompliancePackageMappings.Where(cond => cond.BCPM_ID == BCPM_ID && !cond.BCPM_IsDeleted).FirstOrDefault();
            if (bkgCompliancePackageMapping.IsNotNull())
            {
                bkgCompliancePackageMapping.BCPM_IsDeleted = true;
                bkgCompliancePackageMapping.BCPM_ModifiedByID = currentLoggedInUserId;
                bkgCompliancePackageMapping.BCPM_ModifiedOn = System.DateTime.UtcNow;
                if (_ClientDBContext.SaveChanges() > 0)
                {
                    return "Mapping deleted successfully.";
                }
            }
            return "Mapping deletion failed.";

        }

        public String UpdateBkgComplPkgDataMapping(int BCPM_ID, int currentLoggedInUserId, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract)
        {
            Boolean isUpdated = false;
            Int32 dataPointID = GetDataPointTypeID(bkgComplPkgDataMappingContract.DataPointCode);

            Entity.ClientEntity.BkgCompliancePackageMapping bkgCompliancePackageMapping = _ClientDBContext.BkgCompliancePackageMappings.Where(x => x.BCPM_ID == BCPM_ID && !x.BCPM_IsDeleted).FirstOrDefault();
            bkgCompliancePackageMapping.BCPM_BkgPackageID = bkgComplPkgDataMappingContract.BkgPackageID;
            bkgCompliancePackageMapping.BCPM_BkgDataPointTypeID = dataPointID;
            bkgCompliancePackageMapping.BCPM_BkgSvcGroupID = bkgComplPkgDataMappingContract.ServiceGroupID;
            bkgCompliancePackageMapping.BCPM_BkgSvcID = bkgComplPkgDataMappingContract.ServiceID;
            bkgCompliancePackageMapping.BCPM_PackageSvcItemID = null;
            bkgCompliancePackageMapping.BCPM_CompliancePkgID = bkgComplPkgDataMappingContract.ComplPackageID.Value;
            bkgCompliancePackageMapping.BCPM_ComplianceCategoryID = bkgComplPkgDataMappingContract.CatagoryID.Value;
            bkgCompliancePackageMapping.BCPM_ComplianceItemID = bkgComplPkgDataMappingContract.ItemID.Value;
            bkgCompliancePackageMapping.BCPM_ComplianceAttributeID = bkgComplPkgDataMappingContract.AttributeID.Value;
            bkgCompliancePackageMapping.BCPM_IsDeleted = false;
            bkgCompliancePackageMapping.BCPM_ModifiedByID = currentLoggedInUserId;
            bkgCompliancePackageMapping.BCPM_ModifiedOn = System.DateTime.UtcNow;
            isUpdated = _ClientDBContext.SaveChanges() > 0;
            if (isUpdated)
            {
                for (int i = 0; i < AppConsts.TWO; i++)
                {
                    List<Entity.ClientEntity.BkgCompliancePkgMappingAttrOption> bkgCompliancePkgMappingAttrOptionList = _ClientDBContext.BkgCompliancePkgMappingAttrOptions
                                                                                            .Where(x => x.BCPAO_BkgComplaincePkgMappingID == BCPM_ID && !x.BCPAO_IsDeleted).ToList();
                    if (bkgCompliancePkgMappingAttrOptionList.Count > AppConsts.NONE)
                    {
                        foreach (var item in bkgCompliancePkgMappingAttrOptionList)
                        {
                            item.BCPAO_BkgComplaincePkgMappingID = bkgCompliancePackageMapping.BCPM_ID;
                            if (item.BCPAO_BKgValue == "1")
                            {
                                item.BCPAO_BKgValue = "1";
                                item.BCPAO_ComplianceAttrOptionValue = bkgComplPkgDataMappingContract.FlaggedValue;
                            }
                            if (item.BCPAO_BKgValue == "0")
                            {
                                item.BCPAO_BKgValue = "0";
                                item.BCPAO_ComplianceAttrOptionValue = bkgComplPkgDataMappingContract.NonFlaggedValue;
                            }

                            item.BCPAO_ModifiedByID = currentLoggedInUserId;
                            item.BCPAO_ModifiedOn = System.DateTime.UtcNow;
                            item.BCPAO_IsDeleted = false;
                        }
                        isUpdated = _ClientDBContext.SaveChanges() > 0;
                    }

                }

                if (isUpdated)
                    return "Mapping Updated Successfully.";
            }

            return "Mapping Updation Failed.";
        }
        #endregion

        public List<BkgCompliancePkgMappingAttrOption> FetchBkgCompliancePkgMappingAttrOptions(int BCPM_ID)
        {
            return _ClientDBContext.BkgCompliancePkgMappingAttrOptions.Where(x => x.BCPAO_BkgComplaincePkgMappingID == BCPM_ID && !x.BCPAO_IsDeleted).ToList();
        }

        #region UAT-583 WB: AMS: Ability to delete attributes and attribute groups from the package setup screen (even after the attribute or attribute group is active)

        Boolean IBackgroundSetupRepository.DeletedBkgSvcAttributeGroupMapping(Int32 bkgAttributeGroupId, Int32 bkgPackageSvcId, Int32 currentloggedInUserId)
        {
            List<BkgPackageSvcAttribute> bkgPackageSvcAttributeMappingLst = _ClientDBContext.BkgPackageSvcAttributes.Where(cond => cond.BPSA_BkgPackageSvcID == bkgPackageSvcId && cond.BPSA_IsDeleted == false && cond.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId == bkgAttributeGroupId && cond.BkgAttributeGroupMapping.BAGM_IsDeleted == false).ToList();
            bkgPackageSvcAttributeMappingLst.ForEach(cnd =>
            {
                cnd.BPSA_IsDeleted = true;
                cnd.BPSA_ModifiedByID = currentloggedInUserId;
                cnd.BPSA_ModifiedOn = DateTime.Now;
            });
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }
        #endregion


        public Boolean UpdateAttributeDisplaySequence(IList<AttributeSetupContract> statusToMove, Int32? destinationIndex, Int32 currentLoggedInUserId)
        {
            DataTable dtBkgAttribute = new DataTable();
            dtBkgAttribute.Columns.Add("StatusID", typeof(Int32));
            dtBkgAttribute.Columns.Add("DestinationIndex", typeof(Int32));
            dtBkgAttribute.Columns.Add("CurrentLoggedInUserId", typeof(Int32));
            foreach (AttributeSetupContract status in statusToMove)
            {
                dtBkgAttribute.Rows.Add(new object[] { status.BkgAttributeGroupMappingId, destinationIndex, currentLoggedInUserId });
                destinationIndex += 1;
            }

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_UpdateBkgAttributeSequence", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeParameter", dtBkgAttribute);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    con.Close();
                    _ClientDBContext.SaveChanges();
                    // _ClientDBContext.Refresh(RefreshMode.StoreWins, statusToMove);
                    return true;
                }
            }
            return false;
        }


        public DataTable GetBkgAttributesBasedOnGroup(int bkgSvcGroupId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAttributesAssociatedWithGroup", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AttributeGroupId", bkgSvcGroupId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        #region UAT-800 Build all missing services into Complio based on spreadsheet of services for College System

        /// <summary>
        /// To Get Service Form Mapping for All and Specific Institute.
        /// </summary>
        /// <param name="serviceFormID"></param>
        /// <param name="serviceID"></param>
        /// <param name="mappingTypeID"></param>
        /// <param name="dpmID"></param>
        /// <returns>DataTable</returns>
        DataTable IBackgroundSetupRepository.GetServiceFormMappingAllandSpecificInstitution(Int32? serviceFormID, Int32? serviceID, Int32? mappingTypeID, Int32? dpmID, Int32? selectedTenantID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetServiceFormMappingAllandSpecificInstitution]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ServiceFormID", serviceFormID);
                command.Parameters.AddWithValue("@ServiceID", serviceID);
                command.Parameters.AddWithValue("@MappingTypeID", mappingTypeID);
                command.Parameters.AddWithValue("@DPM_ID", dpmID);
                command.Parameters.AddWithValue("@TenantID", selectedTenantID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Get Background Service with Mapping
        /// </summary>
        /// <returns>List of BackgroundServiceMapping</returns>
        List<BackgroundServiceMapping> IBackgroundSetupRepository.GetBackgroundServiceMapping()
        {
            List<BackgroundServiceMapping> lstBkgSvcMapping = new List<BackgroundServiceMapping>();
            List<Entity.BackgroundService> lstBkgService = base.SecurityContext.BackgroundServices
                .Include("BkgSvcExtSvcMappings").Include("BkgSvcExtSvcMappings.ExternalBkgSvc").Where(x => x.BSE_IsDeleted == false).ToList();
            if (lstBkgService.IsNotNull() && lstBkgService.Count > 0)
            {
                foreach (Entity.BackgroundService bkgService in lstBkgService)
                {
                    BackgroundServiceMapping bkgSvc = new BackgroundServiceMapping();
                    bkgSvc.BSE_ID = bkgService.BSE_ID;
                    bkgSvc.BSE_Name = bkgService.BSE_Name;
                    if (bkgService.BkgSvcExtSvcMappings.IsNotNull() && bkgService.BkgSvcExtSvcMappings.Count > 0)
                    {
                        String externalCode = String.Empty;
                        foreach (var item in bkgService.BkgSvcExtSvcMappings.Where(x => x.BSESM_IsDeleted == false))
                        {
                            if (item.ExternalBkgSvc.EBS_IsDeleted == false)
                                externalCode = externalCode + item.ExternalBkgSvc.EBS_ExternalCode + ",";
                        }
                        if (!String.IsNullOrEmpty(externalCode))
                        {
                            bkgSvc.BSE_Name = bkgSvc.BSE_Name + "(" + externalCode.Substring(0, externalCode.Length - 1) + ")";
                        }
                    }
                    lstBkgSvcMapping.Add(bkgSvc);
                }
            }
            return lstBkgSvcMapping;
        }

        /// <summary>
        ///  Get Service Attached Forms
        /// </summary>
        /// <returns>List of ServiceForm</returns>
        List<ServiceForm> IBackgroundSetupRepository.GetServiceForm()
        {
            List<ServiceForm> lstServiceForm = new List<ServiceForm>();
            List<Entity.ServiceAttachedForm> lstSvcAtachedForm = base.SecurityContext.ServiceAttachedForms.Where(x => x.SF_IsDeleted == false).ToList();
            if (lstSvcAtachedForm.IsNotNull() && lstSvcAtachedForm.Count > 0)
            {
                foreach (Entity.ServiceAttachedForm svcAtachedForm in lstSvcAtachedForm)
                {
                    ServiceForm svcForm = new ServiceForm();
                    svcForm.SF_ID = svcAtachedForm.SF_ID;
                    svcForm.SF_Name = svcAtachedForm.SF_Name;

                    lstServiceForm.Add(svcForm);
                }
            }
            return lstServiceForm;
        }

        /// <summary>
        /// Get Mapping Types
        /// </summary>
        /// <returns>List of SvcFormMappingType</returns>
        List<SvcFormMappingType> IBackgroundSetupRepository.GetMappingType()
        {
            List<SvcFormMappingType> lstSvcFormMappingType = new List<SvcFormMappingType>();
            List<Entity.lkpMappingType> lstMappingType = base.SecurityContext.lkpMappingTypes.Where(x => x.MT_IsDeleted == false).ToList();
            if (lstMappingType.IsNotNull() && lstMappingType.Count > 0)
            {
                foreach (Entity.lkpMappingType mappingType in lstMappingType)
                {
                    SvcFormMappingType svcFormMappingType = new SvcFormMappingType();
                    svcFormMappingType.MT_ID = mappingType.MT_ID;
                    svcFormMappingType.MT_Name = mappingType.MT_Name;
                    svcFormMappingType.MT_Code = mappingType.MT_Code;

                    lstSvcFormMappingType.Add(svcFormMappingType);
                }
            }
            return lstSvcFormMappingType;
        }

        /// <summary>
        /// Delete Service Form Institution Mapping
        /// </summary>
        /// <param name="serviceFormMappingID"></param>
        /// <param name="serviceFormHierarchyMappingID"></param>
        /// <param name="currentUserID"></param>
        /// <returns>True/False</returns>
        Boolean IBackgroundSetupRepository.DeleteServiceFormInstitutionMapping(Int32 serviceFormMappingID, Int32? serviceFormHierarchyMappingID, Int32 mappingTypeID, Int32 currentUserID)
        {

            if (serviceFormHierarchyMappingID.HasValue && serviceFormHierarchyMappingID.Value > 0)
            {
                BkgServiceAttachedFormHierarchyMapping svcAttachedFormHierMapping = _ClientDBContext.BkgServiceAttachedFormHierarchyMappings
                    .Where(x => x.SAFHM_ID == serviceFormHierarchyMappingID.Value && x.SAFHM_IsDeleted == false).FirstOrDefault();
                //delete service form hierarchy mapping from tenenat table
                if (svcAttachedFormHierMapping.IsNotNull())
                {
                    svcAttachedFormHierMapping.SAFHM_IsDeleted = true;
                    svcAttachedFormHierMapping.SAFHM_ModifiedByID = currentUserID;
                    svcAttachedFormHierMapping.SAFHM_ModifiedOn = DateTime.Now;

                    _ClientDBContext.SaveChanges();
                }
            }

            Entity.BkgServiceAttachedFormMapping svcAttachedFormMapping = base.SecurityContext.BkgServiceAttachedFormMappings
                .Where(x => x.SFM_ID == serviceFormMappingID && x.SFM_IsDeleted == false).FirstOrDefault();
            //if service form is being deleted from all institution level then directly delete its mapping 
            if (svcAttachedFormMapping.IsNotNull() && ((serviceFormHierarchyMappingID.HasValue && serviceFormHierarchyMappingID.Value == 0) || (!serviceFormHierarchyMappingID.HasValue)))
            {
                svcAttachedFormMapping.SFM_IsDeleted = true;
                svcAttachedFormMapping.SFM_ModifiedBy = currentUserID;
                svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;

                base.SecurityContext.SaveChanges();
            }
            else //if service form is being deleted from specific institution
            {
                EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("[dbo].[usp_GetAllTenantServiceFormHierarchyMapping]", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServiceFormMappingID", serviceFormMappingID);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    //check if mapping exists in other tenants too? if exists then change its mapping type to specific instituion
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Int16 mpngTypeID = base.SecurityContext.lkpMappingTypes.Where(x => x.MT_Code == "AAAB").FirstOrDefault().MT_ID;

                        svcAttachedFormMapping.SFM_MappingTypeID = mpngTypeID;
                        svcAttachedFormMapping.SFM_ModifiedBy = currentUserID;
                        svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;

                        base.SecurityContext.SaveChanges();
                    }
                    else // previously : if mapping does not exists then change its mapping type to all instituion ==> right way now : delete its mapping directly from security table as done in case of all institution level
                    {
                        //Int16 mpngTypeID = base.SecurityContext.lkpMappingTypes.Where(x => x.MT_Code == "AAAA").FirstOrDefault().MT_ID;
                        //svcAttachedFormMapping.SFM_MappingTypeID = mpngTypeID;
                        //svcAttachedFormMapping.SFM_ModifiedBy = currentUserID;
                        //svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;

                        //base.SecurityContext.SaveChanges();

                        svcAttachedFormMapping.SFM_IsDeleted = true;
                        svcAttachedFormMapping.SFM_ModifiedBy = currentUserID;
                        svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;

                        base.SecurityContext.SaveChanges();
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Save Service Form Institution Mapping
        /// </summary>
        /// <param name="svcFormInstitutionMappingContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True/False</returns>
        String IBackgroundSetupRepository.SaveServiceFormInstitutionMapping(ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId)
        {
            Entity.ServiceAttachedForm serviceForm = base.SecurityContext.ServiceAttachedForms.Where(x => x.SF_ID == svcFormInstitutionMappingContract.SF_ID
               && x.SF_IsDeleted == false).FirstOrDefault();

            Entity.BkgServiceAttachedFormMapping svcAttachedFormMapping = base.SecurityContext.BkgServiceAttachedFormMappings
                .Where(x => x.SFM_ServiceAttachedFormID == svcFormInstitutionMappingContract.SF_ID
                    && x.SFM_BackgroundServiceID == svcFormInstitutionMappingContract.BSE_ID && x.SFM_IsDeleted == false).FirstOrDefault();

            //if new mapping is added
            if (svcAttachedFormMapping.IsNull())
            {
                svcAttachedFormMapping = new Entity.BkgServiceAttachedFormMapping();
                svcAttachedFormMapping.SFM_ServiceAttachedFormID = svcFormInstitutionMappingContract.SF_ID;
                svcAttachedFormMapping.SFM_BackgroundServiceID = svcFormInstitutionMappingContract.BSE_ID;
                svcAttachedFormMapping.SFM_IsDeleted = false;
                svcAttachedFormMapping.SFM_CreatedBy = currentUserId;
                svcAttachedFormMapping.SFM_CreatedOn = DateTime.Now;
                svcAttachedFormMapping.SFM_ModifiedBy = null;
                svcAttachedFormMapping.SFM_ModifiedOn = null;
                svcAttachedFormMapping.SFM_MappingTypeID = svcFormInstitutionMappingContract.MappingTypeID;


                base.SecurityContext.BkgServiceAttachedFormMappings.AddObject(svcAttachedFormMapping);
                base.SecurityContext.SaveChanges();
            }

            //if mapping already exists and we are trying to add it in specific tenant
            else if (svcAttachedFormMapping.IsNotNull() && svcFormInstitutionMappingContract.DPM_ID.HasValue && svcFormInstitutionMappingContract.DPM_ID.Value > 0)
            {
                BkgServiceAttachedFormHierarchyMapping svcAttFormHierMapping = _ClientDBContext.BkgServiceAttachedFormHierarchyMappings
                    .Where(x => x.SAFHM_InstitutionHierarchyNodeID == svcFormInstitutionMappingContract.DPM_ID.Value
                        && x.SAFHM_SvcAttachedFormMappingID == svcAttachedFormMapping.SFM_ID && x.SAFHM_IsDeleted == false).FirstOrDefault();

                // if we are trying to add mapping on the same node as of the existing mapping
                if (svcAttFormHierMapping.IsNotNull())
                {
                    return "Service Form Institution Mapping for this selection already exists on this node";
                }

                // if we are trying to add mapping on different node as of the existing mapping and existing mapping is on all institute level
                else if (svcAttachedFormMapping.lkpMappingType.IsNotNull() && svcAttachedFormMapping.lkpMappingType.MT_Code.Equals(ServiceFormMappingType.AllInstitute.GetStringValue()))
                {
                    return "Service Form Mapping for this selection already exists on All Institution Level";
                }

                else // if we are trying to add mapping on different node as of the existing mapping and existisng mapping is on tenant Level
                {
                    svcAttachedFormMapping.SFM_ModifiedBy = currentUserId;
                    svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;
                    svcAttachedFormMapping.SFM_MappingTypeID = svcFormInstitutionMappingContract.MappingTypeID;
                    base.SecurityContext.SaveChanges();
                }
            }

            //if mapping already exists and we are trying to add it in all institute level
            else
            {
                svcAttachedFormMapping.SFM_ModifiedBy = currentUserId;
                svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;
                svcAttachedFormMapping.SFM_MappingTypeID = svcFormInstitutionMappingContract.MappingTypeID;
                base.SecurityContext.SaveChanges();
            }

            //logic to add record in BkgServiceAttachedFormHierarchyMapping table
            // if we are trying to add mapping on tenant level
            Int32 svcAttFormHierMappingId = 0;
            if (svcFormInstitutionMappingContract.DPM_ID.HasValue && svcFormInstitutionMappingContract.DPM_ID.Value > 0)
            {
                BkgServiceAttachedFormHierarchyMapping svcAttFormHierMapping = _ClientDBContext.BkgServiceAttachedFormHierarchyMappings
                    .Where(x => x.SAFHM_InstitutionHierarchyNodeID == svcFormInstitutionMappingContract.DPM_ID.Value
                        && x.SAFHM_SvcAttachedFormMappingID == svcAttachedFormMapping.SFM_ID && x.SAFHM_IsDeleted == false).FirstOrDefault();

                if (svcAttFormHierMapping.IsNull())
                {
                    svcAttFormHierMapping = new BkgServiceAttachedFormHierarchyMapping();
                    svcAttFormHierMapping.SAFHM_InstitutionHierarchyNodeID = svcFormInstitutionMappingContract.DPM_ID.Value;
                    svcAttFormHierMapping.SAFHM_SvcAttachedFormMappingID = svcAttachedFormMapping.SFM_ID;
                    svcAttFormHierMapping.SAFHM_IsDeleted = false;
                    svcAttFormHierMapping.SAFHM_CreatedByID = currentUserId;
                    svcAttFormHierMapping.SAFHM_CreatedOn = DateTime.Now;
                    svcAttFormHierMapping.SAFHM_ModifiedByID = null;
                    svcAttFormHierMapping.SAFHM_ModifiedOn = null;

                    _ClientDBContext.BkgServiceAttachedFormHierarchyMappings.AddObject(svcAttFormHierMapping);
                    _ClientDBContext.SaveChanges();
                }
                svcAttFormHierMappingId = svcAttFormHierMapping.SAFHM_ID;
            }

            // Logic for Overriding automatic form to manual : do overriding only when service form is automatic
            AddUpdateOverrideStatusForServiceForm(svcFormInstitutionMappingContract, currentUserId, serviceForm, svcAttachedFormMapping, svcAttFormHierMappingId);

            //logic to add CommunicationTemplates and subevents
            //AddUpdateTemplateEntityBasedOnServiceForm(currentUserId, serviceForm);

            return String.Empty;
        }

        /// <summary>
        /// Update Service Form Institution Mapping
        /// </summary>
        /// <param name="svcFormInstitutionMappingContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True/False</returns>
        String IBackgroundSetupRepository.UpdateServiceFormInstitutionMapping(ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId)
        {
            Entity.ServiceAttachedForm serviceForm = base.SecurityContext.ServiceAttachedForms.Where(x => x.SF_ID == svcFormInstitutionMappingContract.SF_ID
              && x.SF_IsDeleted == false).FirstOrDefault();

            //Get mapping based on mapping id
            Entity.BkgServiceAttachedFormMapping svcAttachedFormMapping = base.SecurityContext.BkgServiceAttachedFormMappings
                .Where(x => x.SFM_ID == svcFormInstitutionMappingContract.SFM_ID && x.SFM_IsDeleted == false).FirstOrDefault();
            String tenantSpecificCode = ServiceFormMappingType.SpecificInstitute.GetStringValue();
            Int32 serviceFormMappingID = svcAttachedFormMapping.IsNotNull() ? svcAttachedFormMapping.SFM_ID : 0;//105
            List<Entity.lkpMappingType> lstLkpMappingType = base.SecurityContext.lkpMappingTypes.Where(cond => !cond.MT_IsDeleted).ToList();
            Int32 tenantSpecificMappingId = lstLkpMappingType.Where(cond => cond.MT_Code == tenantSpecificCode).FirstOrDefault().MT_ID;

            //if form and service are not changed (only mapping type is updated here)
            if (svcAttachedFormMapping.IsNotNull() && svcAttachedFormMapping.SFM_ServiceAttachedFormID == svcFormInstitutionMappingContract.SF_ID
                && svcAttachedFormMapping.SFM_BackgroundServiceID == svcFormInstitutionMappingContract.BSE_ID)
            {
                //if form and service are not changed (only mapping type is updated here)
                svcAttachedFormMapping.SFM_ModifiedBy = currentUserId;
                svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;

                // if we are trying to add mapping on tenant level and existing mapping is on all institute level
                if (svcAttachedFormMapping.lkpMappingType.IsNotNull()
                    && svcAttachedFormMapping.lkpMappingType.MT_Code.Equals(ServiceFormMappingType.AllInstitute.GetStringValue())
                   && svcFormInstitutionMappingContract.MappingTypeID == tenantSpecificMappingId)
                {
                    return "Service Form Mapping for this selection already exists on All Institution Level";
                }
                else
                {
                    svcAttachedFormMapping.SFM_MappingTypeID = svcFormInstitutionMappingContract.MappingTypeID;
                }

                base.SecurityContext.SaveChanges();
            }

            //if form or service(or both) are updated on tenant specific level
            else if (svcFormInstitutionMappingContract.DPM_ID.HasValue && svcFormInstitutionMappingContract.DPM_ID.Value > 0)
            {
                Entity.BkgServiceAttachedFormMapping svcAttachedFormMappingOld = svcAttachedFormMapping;
                svcAttachedFormMapping = base.SecurityContext.BkgServiceAttachedFormMappings
                 .Where(x => x.SFM_ServiceAttachedFormID == svcFormInstitutionMappingContract.SF_ID
                     && x.SFM_BackgroundServiceID == svcFormInstitutionMappingContract.BSE_ID && x.SFM_IsDeleted == false).FirstOrDefault();//null

                if (svcAttachedFormMapping.IsNull())
                {
                    if (serviceFormMappingID != 0)
                    {
                        EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
                        using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                        {
                            SqlCommand command = new SqlCommand("[dbo].[usp_GetAllTenantServiceFormHierarchyMapping]", con);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ServiceFormMappingID", serviceFormMappingID);
                            SqlDataAdapter adp = new SqlDataAdapter();
                            adp.SelectCommand = command;
                            DataSet ds = new DataSet();
                            adp.Fill(ds);
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
                            {
                                svcAttachedFormMappingOld.SFM_ModifiedBy = currentUserId;
                                svcAttachedFormMappingOld.SFM_ModifiedOn = DateTime.Now;
                            }
                            else
                            {
                                svcAttachedFormMappingOld.SFM_IsDeleted = true;
                                svcAttachedFormMappingOld.SFM_ModifiedBy = currentUserId;
                                svcAttachedFormMappingOld.SFM_ModifiedOn = DateTime.Now;
                            }
                        }
                    }

                    svcAttachedFormMapping = new Entity.BkgServiceAttachedFormMapping();
                    svcAttachedFormMapping.SFM_ServiceAttachedFormID = svcFormInstitutionMappingContract.SF_ID;
                    svcAttachedFormMapping.SFM_BackgroundServiceID = svcFormInstitutionMappingContract.BSE_ID;
                    svcAttachedFormMapping.SFM_IsDeleted = false;
                    svcAttachedFormMapping.SFM_CreatedBy = currentUserId;
                    svcAttachedFormMapping.SFM_CreatedOn = DateTime.Now;
                    svcAttachedFormMapping.SFM_ModifiedBy = null;
                    svcAttachedFormMapping.SFM_ModifiedOn = null;
                    svcAttachedFormMapping.SFM_MappingTypeID = svcFormInstitutionMappingContract.MappingTypeID;
                    base.SecurityContext.BkgServiceAttachedFormMappings.AddObject(svcAttachedFormMapping);
                    base.SecurityContext.SaveChanges();
                }
                else
                {
                    BkgServiceAttachedFormHierarchyMapping svcAttFormHierMapping = _ClientDBContext.BkgServiceAttachedFormHierarchyMappings
                     .Where(x => x.SAFHM_InstitutionHierarchyNodeID == svcFormInstitutionMappingContract.DPM_ID.Value
                         && x.SAFHM_SvcAttachedFormMappingID == svcAttachedFormMapping.SFM_ID && x.SAFHM_IsDeleted == false).FirstOrDefault();
                    if (svcAttFormHierMapping.IsNotNull())
                    {
                        return "Service Form Institution Mapping for this selection already exists on this node";
                    }
                    // if we are trying to add mapping on different node as of the existing mapping and existing mapping is on all institute level
                    else if (svcAttachedFormMapping.lkpMappingType.IsNotNull() && svcAttachedFormMapping.lkpMappingType.MT_Code.Equals(ServiceFormMappingType.AllInstitute.GetStringValue()))
                    {
                        return "Service Form Mapping for this selection already exists on All Institution Level";
                    }
                    else
                    {
                        svcAttachedFormMapping.SFM_ModifiedBy = currentUserId;
                        svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;
                        svcAttachedFormMapping.SFM_MappingTypeID = svcFormInstitutionMappingContract.MappingTypeID;
                        base.SecurityContext.SaveChanges();
                    }
                }
            }
            //if form or service(or both) are updated on all institute level : in this case tenant specific mapping will be deleted
            else if (svcAttachedFormMapping.IsNotNull())
            {
                svcAttachedFormMapping.SFM_ServiceAttachedFormID = svcFormInstitutionMappingContract.SF_ID;
                svcAttachedFormMapping.SFM_BackgroundServiceID = svcFormInstitutionMappingContract.BSE_ID;
                svcAttachedFormMapping.SFM_ModifiedBy = currentUserId;
                svcAttachedFormMapping.SFM_ModifiedOn = DateTime.Now;
                svcAttachedFormMapping.SFM_MappingTypeID = svcFormInstitutionMappingContract.MappingTypeID;
                base.SecurityContext.SaveChanges();
            }

            //logic to add record in BkgServiceAttachedFormHierarchyMapping table
            // if we are trying to add mapping on tenant level
            Int32 svcAttFormHierMappingId = 0;
            if (svcFormInstitutionMappingContract.SAFHM_ID.HasValue && svcFormInstitutionMappingContract.SAFHM_ID.Value > 0)
            {
                // if the hierarchy node is already mapped to the new mapping id, then do not do anything
                BkgServiceAttachedFormHierarchyMapping svcAttFormHierMpng = _ClientDBContext.BkgServiceAttachedFormHierarchyMappings
                    .Where(x => x.SAFHM_InstitutionHierarchyNodeID == svcFormInstitutionMappingContract.DPM_ID.Value
                        && x.SAFHM_SvcAttachedFormMappingID == svcAttachedFormMapping.SFM_ID
                        && x.SAFHM_ID != svcFormInstitutionMappingContract.SAFHM_ID.Value
                        && x.SAFHM_IsDeleted == false).FirstOrDefault();//null
                if (svcAttFormHierMpng.IsNotNull())
                {
                    return "Service Form Institution Mapping for this selection already exists on this node";
                }

                //update node id and service form mapping id for service form hierarchy mapping.
                BkgServiceAttachedFormHierarchyMapping svcAttachedFormHierMapping = _ClientDBContext.BkgServiceAttachedFormHierarchyMappings
                    .Where(x => x.SAFHM_ID == svcFormInstitutionMappingContract.SAFHM_ID.Value && x.SAFHM_IsDeleted == false).FirstOrDefault();

                if (!svcAttachedFormHierMapping.IsNullOrEmpty())
                {
                    svcAttachedFormHierMapping.SAFHM_InstitutionHierarchyNodeID = svcFormInstitutionMappingContract.DPM_ID.Value;
                    svcAttachedFormHierMapping.SAFHM_SvcAttachedFormMappingID = svcAttachedFormMapping.SFM_ID;
                    svcAttachedFormHierMapping.SAFHM_ModifiedByID = currentUserId;
                    svcAttachedFormHierMapping.SAFHM_ModifiedOn = DateTime.Now;
                }
                else//create node id and service form mapping id for service form hierarchy mapping.
                {
                    svcAttachedFormHierMapping = new BkgServiceAttachedFormHierarchyMapping();
                    svcAttachedFormHierMapping.SAFHM_InstitutionHierarchyNodeID = svcFormInstitutionMappingContract.DPM_ID.Value;
                    svcAttachedFormHierMapping.SAFHM_SvcAttachedFormMappingID = svcAttachedFormMapping.SFM_ID;
                    svcAttachedFormHierMapping.SAFHM_IsDeleted = false;
                    svcAttachedFormHierMapping.SAFHM_CreatedByID = currentUserId;
                    svcAttachedFormHierMapping.SAFHM_CreatedOn = DateTime.Now;
                    svcAttachedFormHierMapping.SAFHM_ModifiedByID = null;
                    svcAttachedFormHierMapping.SAFHM_ModifiedOn = null;
                    _ClientDBContext.BkgServiceAttachedFormHierarchyMappings.AddObject(svcAttachedFormHierMapping);
                }
                _ClientDBContext.SaveChanges();
                svcAttFormHierMappingId = svcAttachedFormHierMapping.SAFHM_ID;
            }

            // Logic for Overriding automatic form to manual : do overriding only when service form is automatic
            AddUpdateOverrideStatusForServiceForm(svcFormInstitutionMappingContract, currentUserId, serviceForm, svcAttachedFormMapping, svcAttFormHierMappingId);

            // handle template and template mapping         
            //AddUpdateTemplateEntityBasedOnServiceForm(currentUserId, serviceForm);

            return String.Empty;
        }

        private void AddUpdateOverrideStatusForServiceForm(ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId, Entity.ServiceAttachedForm serviceForm, Entity.BkgServiceAttachedFormMapping svcAttachedFormMapping, Int32 svcAttFormHierMappingId)
        {
            if (!serviceForm.IsNullOrEmpty() && serviceForm.SF_SendAutomatically)
            {
                //if mapping is added on tenant specific level
                if (svcFormInstitutionMappingContract.DPM_ID.HasValue && svcFormInstitutionMappingContract.DPM_ID.Value > 0)
                {
                    BkgServiceAttachedFormHierarchyMappingOverride svcFormHierarchyOverride = _ClientDBContext.BkgServiceAttachedFormHierarchyMappingOverrides
                        .Where(cond => cond.SFHMO_ServiceAttachedFormHierarchyMapping == svcAttFormHierMappingId && !cond.SFHMO_IsDeleted).FirstOrDefault();

                    //if no overriding exists : and only when automatic is overrided to manual, then add the new row in BkgServiceAttachedFormHierarchyMappingOverride table
                    if (svcFormHierarchyOverride.IsNullOrEmpty() && svcFormInstitutionMappingContract.EnforceManual == true)
                    {
                        svcFormHierarchyOverride = new BkgServiceAttachedFormHierarchyMappingOverride();
                        svcFormHierarchyOverride.SFHMO_CreatedBy = currentUserId;
                        svcFormHierarchyOverride.SFHMO_CreatedOn = DateTime.Now;
                        svcFormHierarchyOverride.SFHMO_EnforceManual = svcFormInstitutionMappingContract.EnforceManual;
                        svcFormHierarchyOverride.SFHMO_IsDeleted = false;
                        svcFormHierarchyOverride.SFHMO_ServiceAttachedFormHierarchyMapping = svcAttFormHierMappingId;
                        _ClientDBContext.BkgServiceAttachedFormHierarchyMappingOverrides.AddObject(svcFormHierarchyOverride);
                    }
                    // if overriding exists for the newly created mapping then update its EnforceManual status
                    else if (svcFormHierarchyOverride.IsNotNull())
                    {
                        svcFormHierarchyOverride.SFHMO_ModifiedBy = currentUserId;
                        svcFormHierarchyOverride.SFHMO_ModifiedOn = DateTime.Now;
                        svcFormHierarchyOverride.SFHMO_EnforceManual = svcFormInstitutionMappingContract.EnforceManual;
                    }
                    _ClientDBContext.SaveChanges();
                }
                //if mapping is added on all tenant level
                else
                {
                    Entity.BkgServiceAttachedFormMappingOverride svcFormMappingOverride = base.SecurityContext.BkgServiceAttachedFormMappingOverrides
                                        .Where(cond => cond.SFMO_ServiceAttachedFormMapping == svcAttachedFormMapping.SFM_ID && !cond.SFMO_IsDeleted).FirstOrDefault();
                    //if no overriding exists : and only when automatic is overrided to manual, then add the new row in BkgServiceAttachedFormMappingOverride table
                    if (svcFormMappingOverride.IsNullOrEmpty() && svcFormInstitutionMappingContract.EnforceManual == true)
                    {
                        svcFormMappingOverride = new Entity.BkgServiceAttachedFormMappingOverride();
                        svcFormMappingOverride.SFMO_CreatedBy = currentUserId;
                        svcFormMappingOverride.SFMO_CreatedOn = DateTime.Now;
                        svcFormMappingOverride.SFMO_EnforceManual = svcFormInstitutionMappingContract.EnforceManual;
                        svcFormMappingOverride.SFMO_IsDeleted = false;
                        svcFormMappingOverride.SFMO_ServiceAttachedFormMapping = svcAttachedFormMapping.SFM_ID;
                        base.SecurityContext.BkgServiceAttachedFormMappingOverrides.AddObject(svcFormMappingOverride);
                    }
                    // if overriding exists for the newly created mapping then update its EnforceManual status
                    else if (svcFormMappingOverride.IsNotNull())
                    {
                        svcFormMappingOverride.SFMO_ModifiedBy = currentUserId;
                        svcFormMappingOverride.SFMO_ModifiedOn = DateTime.Now;
                        svcFormMappingOverride.SFMO_EnforceManual = svcFormInstitutionMappingContract.EnforceManual;
                    }
                    base.SecurityContext.SaveChanges();
                }
            }
        }

        private void AddUpdateTemplateEntityBasedOnServiceForm(Int32 currentUserId, Entity.ServiceAttachedForm serviceForm)
        {
            if (serviceForm.IsNotNull() && serviceForm.SF_SendAutomatically)
            {
                Int32 entityTypeID = base.MessagingContext.lkpCommunicationEntityTypes.Where(x => x.CET_Code == "AAAA").FirstOrDefault().CET_ID;

                Entity.CommunicationTemplateEntity commTemplateEntity = base.MessagingContext.CommunicationTemplateEntities
                    .Where(x => x.CTE_EntityID == serviceForm.SF_ID && x.CTE_IsDeleted == false).FirstOrDefault();

                if (commTemplateEntity.IsNull())
                {
                    commTemplateEntity = base.MessagingContext.CommunicationTemplateEntities
                        .Where(x => x.CTE_EntityID == serviceForm.SF_ID && x.CTE_CommunicationEntityTypeID == entityTypeID).FirstOrDefault();
                    if (commTemplateEntity.IsNull())
                    {
                        commTemplateEntity = base.MessagingContext.CommunicationTemplateEntities
                        .Where(x => x.CTE_EntityID == serviceForm.SF_ParentServiceFormID && x.CTE_CommunicationEntityTypeID == entityTypeID).FirstOrDefault();
                    }

                    if (commTemplateEntity.IsNotNull())
                    {
                        Entity.CommunicationTemplateEntity communicationTemplateEntity = new Entity.CommunicationTemplateEntity();
                        communicationTemplateEntity.CTE_TemplateID = commTemplateEntity.CTE_TemplateID;
                        communicationTemplateEntity.CTE_CommunicationEntityTypeID = entityTypeID;
                        communicationTemplateEntity.CTE_EntityID = serviceForm.SF_ID;
                        communicationTemplateEntity.CTE_IsDeleted = false;
                        communicationTemplateEntity.CTE_CreatedBy = currentUserId;
                        communicationTemplateEntity.CTE_CreatedOn = DateTime.Now;
                        communicationTemplateEntity.CTE_ModifiedBy = null;
                        communicationTemplateEntity.CTE_ModifiedOn = null;

                        base.MessagingContext.CommunicationTemplateEntities.AddObject(communicationTemplateEntity);
                        base.MessagingContext.SaveChanges();
                    }
                }
                else
                {
                    commTemplateEntity.CTE_TemplateID = commTemplateEntity.CTE_TemplateID;
                    commTemplateEntity.CTE_CommunicationEntityTypeID = entityTypeID;
                    commTemplateEntity.CTE_EntityID = serviceForm.SF_ID;
                    commTemplateEntity.CTE_IsDeleted = false;
                    commTemplateEntity.CTE_ModifiedBy = currentUserId;
                    commTemplateEntity.CTE_ModifiedOn = DateTime.Now;

                    base.MessagingContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Get Service Ids by Service Form ID 
        /// </summary>
        /// <param name="serviceFormID"></param>
        /// <returns>List of ServiceIDs</returns>
        List<Int32> IBackgroundSetupRepository.GetServiceIdsByServiceForm(Int32 serviceFormID)
        {
            List<Int32> serviceIDs = base.SecurityContext.BkgServiceAttachedFormMappings
                .Where(x => x.SFM_ServiceAttachedFormID == serviceFormID && x.SFM_IsDeleted == false).Select(x => x.SFM_BackgroundServiceID).Distinct().ToList();
            return serviceIDs;
        }

        #endregion

        #region Service Attached Form
        List<ServiceAttachedFormContract> IBackgroundSetupRepository.GetServoceAttachedFormList()
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetServiceAttachedForm]", con);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<ServiceAttachedFormContract> lstServiceAttachedForm = new List<ServiceAttachedFormContract>();
                lstServiceAttachedForm = ds.Tables[0].AsEnumerable().Select(col =>
                      new ServiceAttachedFormContract
                      {
                          SF_ID = Convert.ToInt32(col["SF_ID"]),
                          FormName = col["FormName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FormName"]),
                          ParentFormName = col["ParentFormName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ParentFormName"]),
                          ServiceFormDispatchMode = col["FormDispatchMode"] == DBNull.Value ? false : Convert.ToBoolean(col["FormDispatchMode"]),
                          ParentSvcFormID = col["ParentSvcFormID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ParentSvcFormID"]),
                          SystemDocumentID = col["SystemDocumentID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SystemDocumentID"]),
                          SystemDocumentFileName = col["SystemDocumentFileName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SystemDocumentFileName"]),
                          ServiceFormDispatchType = col["FormDispatchMode"] == DBNull.Value ? "" : Convert.ToBoolean(col["FormDispatchMode"]) ? "Automatic" : "Manual"
                      }).ToList();

                return lstServiceAttachedForm;
            }
            // return _ClientDBContext.BackgroundServices.Where(x => x.BSE_ParentServiceID == null && x.BSE_IsDeleted == false).ToList();
        }

        List<Entity.ServiceAttachedForm> IBackgroundSetupRepository.GetParentServiceattachedForm()
        {
            return SecurityContext.ServiceAttachedForms.Where(cond => cond.SF_ParentServiceFormID == null && cond.SF_IsDeleted == false && cond.SF_SendAutomatically == true).ToList();
        }

        Boolean IBackgroundSetupRepository.SaveServiceAttachedForm(Entity.ServiceAttachedForm serviceAttachedForm)
        {
            SecurityContext.ServiceAttachedForms.AddObject(serviceAttachedForm);
            if (SecurityContext.SaveChanges() > 0)
                return true;
            return false;
        }

        Entity.ServiceAttachedForm IBackgroundSetupRepository.GetServiceAttachedFormByID(Int32 SF_ID)
        {
            return SecurityContext.ServiceAttachedForms.FirstOrDefault(cond => cond.SF_ID == SF_ID && cond.SF_IsDeleted == false);
        }

        //UAT-2480
        Boolean IBackgroundSetupRepository.IsServiceAttachedFormVersionsDeleted(Int32 SF_ID)
        {
            Entity.ServiceAttachedForm ServiceAttachedForm = SecurityContext.ServiceAttachedForms.Where(cond => cond.SF_ParentServiceFormID == SF_ID && cond.SF_IsDeleted == false).FirstOrDefault();

            if (ServiceAttachedForm.IsNullOrEmpty())
            {
                //// Int32 ParentServiceFormID = SecurityContext.ServiceAttachedForms.Where(con => con.SF_ID == SF_ID && !con.SF_IsDeleted).Select(sel => sel.SF_ParentServiceFormID.Value).FirstOrDefault();
                //if (!SF_ParentServiceFormID.IsNullOrEmpty() && SF_ParentServiceFormID > AppConsts.NONE)
                //{
                //    ServiceAttachedForm = SecurityContext.ServiceAttachedForms.Where(cond => cond.SF_ID == SF_ParentServiceFormID && cond.SF_IsDeleted == false).FirstOrDefault();

                //    if (ServiceAttachedForm.IsNullOrEmpty())
                //    {
                return true;
                //}
                //}
            }
            return false;
        }


        Boolean IBackgroundSetupRepository.UpdateServiceAttachedForm()
        {
            SecurityContext.SaveChanges();
            return true;
        }

        Boolean IBackgroundSetupRepository.CheckIfServiceAttachedFormNameAlreadyExist(String serviceFormName, Int32 serviceFormID, Boolean isUpdate)
        {
            if (isUpdate)
            {
                return SecurityContext.ServiceAttachedForms.Any(obj => obj.SF_Name.Trim().ToUpper() == serviceFormName.Trim().ToUpper()
                        && obj.SF_ID != serviceFormID
                        && obj.SF_IsDeleted == false);
            }
            return SecurityContext.ServiceAttachedForms.Any(obj => obj.SF_Name.Trim().ToUpper() == serviceFormName.Trim().ToUpper() && obj.SF_IsDeleted == false);
        }

        IEnumerable<Entity.BkgServiceAttachedFormMapping> IBackgroundSetupRepository.GetBkgServiceAttachedFormMappingByServiceFormID(Int32 serviceAttachedFormID)
        {
            return SecurityContext.BkgServiceAttachedFormMappings.Where(cond => cond.SFM_ServiceAttachedFormID == serviceAttachedFormID && cond.SFM_IsDeleted == false);

        }


        #endregion

        #region UAT-803 - BACKGROUND PACKAGE STATE SEARCH CRITERIA
        public Boolean SaveBkgPkgStateSearchCriteria(List<BkgPackageStateSearchContract> bkgPkgStateSearchContract, Int32 currentLoggedInUserID)
        {
            Int32 currentBkgPkgID;
            Int32 currentStateID;
            DateTime createdOn = DateTime.Now;
            DateTime modifiedOn = DateTime.Now;

            for (Int32 i = 0; i < bkgPkgStateSearchContract.Count; i++)
            {
                Boolean isExistingRecord = false;
                currentBkgPkgID = bkgPkgStateSearchContract[i].BkgPackageID;
                currentStateID = bkgPkgStateSearchContract[i].StateID;

                Entity.ClientEntity.BkgPkgStateSearch bkgPkgStateSearch = new Entity.ClientEntity.BkgPkgStateSearch();
                Entity.ClientEntity.BkgPkgStateSearch bkgPkgStateSearchObj = _ClientDBContext.BkgPkgStateSearches.FirstOrDefault(x => x.BPSS_BPAID == currentBkgPkgID && x.BPSS_StateID == currentStateID && !x.BPSS_IsDeleted);

                //If Record already exists corresponding to the packageID and StateID
                if (bkgPkgStateSearchObj.IsNotNull())
                {
                    bkgPkgStateSearch = bkgPkgStateSearchObj;
                    bkgPkgStateSearch.BPSS_ModifiedBy = currentLoggedInUserID;
                    bkgPkgStateSearch.BPSS_ModifiedOn = modifiedOn;
                    isExistingRecord = true;
                }
                else
                {
                    bkgPkgStateSearch.BPSS_BPAID = currentBkgPkgID;
                    bkgPkgStateSearch.BPSS_StateID = currentStateID;
                    bkgPkgStateSearch.BPSS_CreatedBy = currentLoggedInUserID;
                    bkgPkgStateSearch.BPSS_CreatedOn = createdOn;
                    isExistingRecord = false;
                }

                bkgPkgStateSearch.BPSS_IsStateSearch = bkgPkgStateSearchContract[i].IsStateSearchChecked;
                bkgPkgStateSearch.BPSS_IsCountySearch = bkgPkgStateSearchContract[i].IsCountySearchChecked;

                if (!isExistingRecord)
                {
                    _ClientDBContext.BkgPkgStateSearches.AddObject(bkgPkgStateSearch);
                }
            }
            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public List<Entity.ClientEntity.BkgPkgStateSearch> GetBkgPkgStateSearchCriteria(Int32 bkgPackageID)
        {
            List<Entity.ClientEntity.BkgPkgStateSearch> lstBkgPackageStateSearchObj = new List<Entity.ClientEntity.BkgPkgStateSearch>();
            lstBkgPackageStateSearchObj = _ClientDBContext.BkgPkgStateSearches.Where(x => x.BPSS_BPAID == bkgPackageID && !x.BPSS_IsDeleted).ToList();
            if (lstBkgPackageStateSearchObj.IsNotNull() && lstBkgPackageStateSearchObj.Count > 0)
            {
                return lstBkgPackageStateSearchObj;
            }
            return new List<BkgPkgStateSearch>();
        }

        public Boolean SaveMasterStateSearchCriteria(List<BkgPackageStateSearchContract> bkgPkgStateSearchContract, Int32 currentLoggedInUserID)
        {
            Int32 currentStateID = 0;
            DateTime createdOn = DateTime.Now;
            DateTime modifiedOn = DateTime.Now;

            for (Int32 i = 0; i < bkgPkgStateSearchContract.Count; i++)
            {
                Boolean isExistingRecord = false;
                currentStateID = bkgPkgStateSearchContract[i].StateID;

                Entity.BkgMasterStateSearch bkgMasterStateSearch = new Entity.BkgMasterStateSearch();
                Entity.BkgMasterStateSearch bkgMasterStateSearchObj = SecurityContext.BkgMasterStateSearches.FirstOrDefault(x => x.BMSS_StateID == currentStateID && !x.BMSS_IsDeleted);

                //If Record already exists corresponding to the StateID
                if (bkgMasterStateSearchObj.IsNotNull())
                {
                    bkgMasterStateSearch = bkgMasterStateSearchObj;
                    bkgMasterStateSearch.BMSS_ModifiedBy = currentLoggedInUserID;
                    bkgMasterStateSearch.BMSS_ModifiedOn = modifiedOn;
                    isExistingRecord = true;
                }
                else
                {
                    bkgMasterStateSearch.BMSS_StateID = currentStateID;
                    bkgMasterStateSearch.BMSS_CreatedBy = currentLoggedInUserID;
                    bkgMasterStateSearch.BMSS_CreatedOn = createdOn;
                    isExistingRecord = false;
                }

                bkgMasterStateSearch.BMSS_IsStateSearch = bkgPkgStateSearchContract[i].IsStateSearchChecked;
                bkgMasterStateSearch.BMSS_IsCountySearch = bkgPkgStateSearchContract[i].IsCountySearchChecked;

                if (!isExistingRecord)
                {
                    SecurityContext.BkgMasterStateSearches.AddObject(bkgMasterStateSearch);
                }
            }
            if (SecurityContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public List<Entity.BkgMasterStateSearch> GetMasterStateSearchCriteria()
        {
            List<Entity.BkgMasterStateSearch> lstBkgMasterStateSearchObj = new List<Entity.BkgMasterStateSearch>();
            lstBkgMasterStateSearchObj = SecurityContext.BkgMasterStateSearches.Where(x => !x.BMSS_IsDeleted).ToList();
            if (lstBkgMasterStateSearchObj.IsNotNull() && lstBkgMasterStateSearchObj.Count > 0)
            {
                return lstBkgMasterStateSearchObj;
            }
            return new List<Entity.BkgMasterStateSearch>();
        }

        public List<BkgPkgStateSearch> UpdateStateSearchSettingsFromMaster(Int32 currentLoggedInUserID, Int32 bkgPackageID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_UpdateStateSearchSettingsFromMaster", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@bkgPackageID", bkgPackageID);
                command.Parameters.AddWithValue("@currentLoggedInUserID", currentLoggedInUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IEnumerable<DataRow> rows = ds.Tables[0].AsEnumerable();
                        return rows.Select(col => new BkgPkgStateSearch
                        {
                            BPSS_BPAID = bkgPackageID,
                            BPSS_StateID = Convert.ToInt32(col["BPSS_StateID"]),
                            BPSS_IsStateSearch = Convert.ToBoolean(col["BPSS_IsStateSearch"]),
                            BPSS_IsCountySearch = Convert.ToBoolean(col["BPSS_IsCountySearch"]),
                        }).ToList();
                    }
                }
                return new List<BkgPkgStateSearch>();
            }
        }

        public Boolean IsStateSearchRuleExists(Int32 pkgServiceItemID)
        {
            Int32 BkgSrchSrvcItemRuleMappingID = _ClientDBContext.BkgSrchSrvcItemRuleMappings.Include("BkgRuleMapping").Where(x => x.SIRM_PackageServiceItemId == pkgServiceItemID && !x.SIRM_IsDeleted && !x.BkgRuleMapping.BRLM_IsDeleted).Select(x => x.SIRM_ID).FirstOrDefault();
            if (BkgSrchSrvcItemRuleMappingID.IsNotNull() && BkgSrchSrvcItemRuleMappingID > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Service Forms

        /// <summary>
        ///Get the Service forms associated with a Background Service, along with their
        ///Dispatch type either Manual or Electronic, at the Root(Form) level
        ///Service level and Package Service Mapping level
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="bkgSvcId"></param>
        /// <param name="bpsId"></param>
        /// <returns></returns>
        public DataTable GetServiceFormDispatchType(Int32 packageId, Int32 bkgSvcId, Int32 bpsId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgSvcFormsDispatchTypes", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageId", packageId);
                command.Parameters.AddWithValue("@BkgSvcId", bkgSvcId);
                command.Parameters.AddWithValue("@BPSId", bpsId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// Update the Overriding data when the PacageService mapping is updated
        /// </summary>
        /// <param name="_lstBkgPackageSvcFormOverride"></param>
        /// <param name="bpsId"></param>
        public void UpdateBkgPackageSvcFormOverride(List<BkgPackageSvcFormOverride> _lstBkgPackageSvcFormOverride, Int32 bpsId)
        {
            List<Int32> _lst = new List<Int32>();
            foreach (var item in _lstBkgPackageSvcFormOverride)
            {
                if (item.BPSO_ID == AppConsts.NONE)
                {
                    item.BPSO_PackageService = bpsId;
                    _ClientDBContext.BkgPackageSvcFormOverrides.AddObject(item);
                }
                else
                    _lst.Add(item.BPSO_ID);
            }
            List<BkgPackageSvcFormOverride> _lstToUpdate = _ClientDBContext.BkgPackageSvcFormOverrides.Where(x => _lst.Contains(x.BPSO_ID)).ToList();

            foreach (var itemToUpdate in _lstToUpdate)
            {
                var _newData = _lstBkgPackageSvcFormOverride.Where(x => x.BPSO_ID == itemToUpdate.BPSO_ID).First();

                // Update only if there is no change
                if (itemToUpdate.BPSO_IsAutomatic != _newData.BPSO_IsAutomatic || itemToUpdate.BPSO_HideServiceForm != _newData.BPSO_HideServiceForm)
                {
                    itemToUpdate.BPSO_ModifiedByID = _newData.BPSO_ModifiedByID;
                    itemToUpdate.BPSO_ModifiedOn = _newData.BPSO_ModifiedOn;
                    itemToUpdate.BPSO_HideServiceForm = _newData.BPSO_HideServiceForm;
                    itemToUpdate.BPSO_IsAutomatic = _newData.BPSO_IsAutomatic;
                }
            }
        }

        #endregion

        #region UAT-844 - ORDER REVIEW ENHANCEMENT
        //UPDATE PACKAGE SERVICE GROUP
        public Boolean UpdatePackageServiceGroup(BkgPackageSvcGroup bkgPackageSvcGroup, Int32 bkgPackageID, Int32 bkgSvcGroupID)
        {
            Int32 result = 0;
            BkgPackageSvcGroup bkgPackageSvcGroupObj = _ClientDBContext.BkgPackageSvcGroups.FirstOrDefault(cond => cond.BPSG_BackgroundPackageID == bkgPackageID
                                                                        && cond.BPSG_BkgSvcGroupID == bkgSvcGroupID);
            if (bkgPackageSvcGroupObj.IsNotNull())
            {
                bkgPackageSvcGroupObj.BPSG_IsFirstReviewTrigger = bkgPackageSvcGroup.BPSG_IsFirstReviewTrigger;
                bkgPackageSvcGroupObj.BPSG_IsSecondReviewTrigger = bkgPackageSvcGroup.BPSG_IsSecondReviewTrigger;
                result = _ClientDBContext.SaveChanges();
            }

            if (result > 0)
                return true;
            return false;
        }

        //GET PACKAGE SERVICE GROUP DETAILS BY BKG PACKAGEID AND SERVICEGROUPID
        public BkgPackageSvcGroup GetPkgServiceGroupDetail(Int32 serviceGroupID, Int32 packageID)
        {
            return _ClientDBContext.BkgPackageSvcGroups.FirstOrDefault(cond => cond.BPSG_BackgroundPackageID == packageID && cond.BPSG_BkgSvcGroupID == serviceGroupID);
        }
        #endregion

        #region Master Review Criteria
        public List<BkgReviewCriteria> FetchMasterReviewCriteria()
        {
            return _ClientDBContext.BkgReviewCriterias.Where(cond => !cond.BRC_IsDeleted).ToList();
        }


        public Boolean SaveReviewCriteria(BkgReviewCriteria reviewCriteria)
        {
            _ClientDBContext.BkgReviewCriterias.AddObject(reviewCriteria);
            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        public bool UpdateReviewCriteria(BkgReviewCriteria oldReviewCriteria, Boolean isDeleteMode)
        {
            BkgReviewCriteria newBkgReviewCriteria = _ClientDBContext.BkgReviewCriterias.Where(x => x.BRC_ID == oldReviewCriteria.BRC_ID && !x.BRC_IsDeleted).FirstOrDefault();
            if (!isDeleteMode)
            {
                newBkgReviewCriteria.BRC_Name = oldReviewCriteria.BRC_Name;
                newBkgReviewCriteria.BRC_Description = oldReviewCriteria.BRC_Description;
            }
            newBkgReviewCriteria.BRC_IsDeleted = oldReviewCriteria.BRC_IsDeleted;
            newBkgReviewCriteria.BRC_ModifiedByID = oldReviewCriteria.BRC_ModifiedByID;
            newBkgReviewCriteria.BRC_ModifiedDate = oldReviewCriteria.BRC_ModifiedDate;
            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Mapped Review Criteria [UAT-844: Order Review enhancements]
        List<BkgReviewCriteriaHierarchyMapping> IBackgroundSetupRepository.GetMappedReviewCriteriaList(Int32 instHierarchyNodeId)
        {
            return _ClientDBContext.BkgReviewCriteriaHierarchyMappings.Where(cnd => cnd.BRCHM_InstitutionHierarchyNodeID == instHierarchyNodeId && cnd.BRCHM_IsDeleted == false).ToList();
        }

        Boolean IBackgroundSetupRepository.SaveReviewCriteriaMapping(List<BkgReviewCriteriaHierarchyMapping> reviewCriteriaListToMap)
        {
            foreach (BkgReviewCriteriaHierarchyMapping item in reviewCriteriaListToMap)
            {
                _ClientDBContext.BkgReviewCriteriaHierarchyMappings.AddObject(item);
            }

            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        Boolean IBackgroundSetupRepository.DeleteReviewCriteriaMapping(Int32 currentloggedInUserId, Int32 BRCHM_ID)
        {
            BkgReviewCriteriaHierarchyMapping reviewCriteriaMappingToDelete = _ClientDBContext.BkgReviewCriteriaHierarchyMappings.FirstOrDefault(cnd => cnd.BRCHM_ID == BRCHM_ID && cnd.BRCHM_IsDeleted == false);
            if (reviewCriteriaMappingToDelete.IsNotNull())
            {
                reviewCriteriaMappingToDelete.BRCHM_IsDeleted = true;
                reviewCriteriaMappingToDelete.BRCHM_ModifiedBy = currentloggedInUserId;
                reviewCriteriaMappingToDelete.BRCHM_ModifiedOn = DateTime.Now;
                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region UAT-1451:Data synch mapping screen is almost unusable as of now (UI updates)
        Boolean IBackgroundSetupRepository.IsBkgCompDataPointMappingExist(Int32? BCPM_ID, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract)
        {
            Int32 dataPointID = GetDataPointTypeID(bkgComplPkgDataMappingContract.DataPointCode);
            BkgCompliancePackageMapping bkgCompPkgMapping = new BkgCompliancePackageMapping();
            bkgCompPkgMapping = _ClientDBContext.BkgCompliancePackageMappings.Where(x => x.BCPM_BkgPackageID == bkgComplPkgDataMappingContract.BkgPackageID
                                                                                    && x.BCPM_BkgDataPointTypeID == dataPointID
                                                                                    && x.BCPM_ComplianceAttributeID == bkgComplPkgDataMappingContract.AttributeID
                                                                                    && x.BCPM_ComplianceCategoryID == bkgComplPkgDataMappingContract.CatagoryID
                                                                                    && x.BCPM_ComplianceItemID == bkgComplPkgDataMappingContract.ItemID
                                                                                    && x.BCPM_CompliancePkgID == bkgComplPkgDataMappingContract.ComplPackageID
                                                                                    && (bkgComplPkgDataMappingContract.ServiceGroupID == null
                                                                                        || x.BCPM_BkgSvcGroupID == bkgComplPkgDataMappingContract.ServiceGroupID)
                                                                                    && (bkgComplPkgDataMappingContract.ServiceID == null
                                                                                        || x.BCPM_BkgSvcID == bkgComplPkgDataMappingContract.ServiceID)
                                                                                    && (BCPM_ID == null || x.BCPM_ID != BCPM_ID)
                                                                                    && !x.BCPM_IsDeleted).FirstOrDefault();
            if (bkgCompPkgMapping.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion



        #region UAT 1560 WB: We should be able to add documents that need to be signed to the order process.
        public List<Entity.SystemDocument> GetBothUploadedDisclosureDocuments(List<Int32> lstDocSatusIds)
        {
            return base.SecurityContext.SystemDocuments.Where(x => x.SD_DocType_ID.HasValue && lstDocSatusIds.Contains(x.SD_DocType_ID.Value) && !x.IsDeleted).ToList();
        }
        public List<Entity.SystemDocument> GetAdditionalDocuments(List<Int32> backgroundPackageIds, List<Int32> compliancePackageIds, Int32? HierarchyNodeID)
        {
            StringBuilder BPAIds = new StringBuilder();
            foreach (Int32 BPAID in backgroundPackageIds.Distinct())
            {
                BPAIds.Append(BPAID);
                BPAIds.Append(",");
            }

            StringBuilder compPackageIds = new StringBuilder();
            foreach (Int32 pkgId in compliancePackageIds.Distinct())
            {
                compPackageIds.Append(pkgId);
                compPackageIds.Append(",");
            }

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[usp_GetAdditionalDocumentsForSignature]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CompPackageIds", compPackageIds.ToString());
                command.Parameters.AddWithValue("@BkgPackageIds", BPAIds.ToString());
                command.Parameters.AddWithValue("@HierarchyNodeID", HierarchyNodeID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                IEnumerable<DataRow> rows = ds.Tables[0].AsEnumerable();
                return rows.Select(col => new Entity.SystemDocument
                {
                    SystemDocumentID = Convert.ToInt32(col["SystemDocumentID"]),
                    DocumentPath = Convert.ToString(col["DocumentPath"]),
                    FileName = Convert.ToString(col["FileName"]),
                    IsOperational = col["IsOperational"] == DBNull.Value ? false : Convert.ToBoolean(col["IsOperational"]),
                    SendToStudent = col["SendToStudent"] == DBNull.Value ? false : Convert.ToBoolean(col["SendToStudent"])
                }).ToList();
            }
        }
        #endregion

        #region Resolved a existing issue related to attribute options with the implementation of UAT-1738
        public List<Entity.ClientEntity.ComplianceAttributeOption> GetComplianceAttributeOption(Int32 attributeId)
        {
            return _ClientDBContext.ComplianceAttributeOptions.Where(cond => cond.IsActive && !cond.IsDeleted && cond.ComplianceItemAttributeID == attributeId).ToList();
        }
        #endregion

        #region UAT-1834: NYU Migration 2 of 3: Applicant Complete Order Process

        /// <summary>
        /// Get Background Package by Background Package ID and Hierarchy Node ID
        /// </summary>
        /// <param name="bkgPackageID"></param>
        /// <param name="orderNodeID"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <returns></returns>
        BackgroundPackagesContract IBackgroundSetupRepository.GetBackgroundPackageByPkgIDAndNodeID(Int32 bkgPackageID, Int32 orderNodeID, Int32 hierarchyNodeID)
        {
            BackgroundPackagesContract backgroundPackagesContract = new BackgroundPackagesContract();
            var packageAvailabilityCode = PackageAvailability.AVAILABLE_FOR_ORDER.GetStringValue();
            //If mapping does not exist for orderNodeID then check mapping for hierarchyNodeID
            var orderNodeBkgPackageHierarchyMapping = _ClientDBContext.BkgPackageHierarchyMappings.FirstOrDefault(con => con.BPHM_BackgroundPackageID == bkgPackageID && con.BPHM_InstitutionHierarchyNodeID == orderNodeID
                && !con.BPHM_IsDeleted && con.BPHM_IsActive && con.lkpPackageAvailability.PA_Code == packageAvailabilityCode);

            if (orderNodeBkgPackageHierarchyMapping.IsNull())
            {
                var hierarchyNodeBkgPackageHierarchyMapping = _ClientDBContext.BkgPackageHierarchyMappings.FirstOrDefault(con => con.BPHM_BackgroundPackageID == bkgPackageID && con.BPHM_InstitutionHierarchyNodeID == hierarchyNodeID
                    && !con.BPHM_IsDeleted && con.BPHM_IsActive && con.lkpPackageAvailability.PA_Code == packageAvailabilityCode);

                if (hierarchyNodeBkgPackageHierarchyMapping.IsNotNull())
                {
                    backgroundPackagesContract.BPHMId = hierarchyNodeBkgPackageHierarchyMapping.BPHM_ID;
                    backgroundPackagesContract.BPAId = hierarchyNodeBkgPackageHierarchyMapping.BPHM_BackgroundPackageID;
                    backgroundPackagesContract.IsExclusive = hierarchyNodeBkgPackageHierarchyMapping.BPHM_IsExclusive;
                    backgroundPackagesContract.BasePrice = hierarchyNodeBkgPackageHierarchyMapping.BPHM_PackageBasePrice ?? AppConsts.NONE;
                    backgroundPackagesContract.MaxNumberOfYearforResidence = hierarchyNodeBkgPackageHierarchyMapping.BPHM_MaxNumberOfYearforResidence ?? AppConsts.MINUS_ONE;
                }
            }
            else
            {
                backgroundPackagesContract.BPHMId = orderNodeBkgPackageHierarchyMapping.BPHM_ID;
                backgroundPackagesContract.BPAId = orderNodeBkgPackageHierarchyMapping.BPHM_BackgroundPackageID;
                backgroundPackagesContract.IsExclusive = orderNodeBkgPackageHierarchyMapping.BPHM_IsExclusive;
                backgroundPackagesContract.BasePrice = orderNodeBkgPackageHierarchyMapping.BPHM_PackageBasePrice ?? AppConsts.NONE;
                backgroundPackagesContract.MaxNumberOfYearforResidence = orderNodeBkgPackageHierarchyMapping.BPHM_MaxNumberOfYearforResidence ?? AppConsts.MINUS_ONE;
            }

            return backgroundPackagesContract;
        }

        /// <summary>
        /// Update Order ID and Status in BulkOrderUpload table
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="bulkOrderUploadID"></param>
        /// <param name="bulkOrderStatusID"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        Boolean IBackgroundSetupRepository.UpdateBulkOrder(Int32 orderID, Int32 bulkOrderUploadID, Int32 bulkOrderStatusID, Int32 currentUserID)
        {
            var bulkOrderUpload = _ClientDBContext.BulkOrderUploads.FirstOrDefault(x => x.BOU_ID == bulkOrderUploadID && x.BOU_IsActive && !x.BOU_IsDeleted);
            if (bulkOrderUpload.IsNotNull())
            {
                bulkOrderUpload.BOU_OrderID = orderID;
                bulkOrderUpload.BOU_BulkOrderStatusID = bulkOrderStatusID;
                bulkOrderUpload.BOU_LastOrderPlacedDate = DateTime.Now;
                bulkOrderUpload.BOU_ModifiedByID = currentUserID;
                bulkOrderUpload.BOU_ModifiedOn = DateTime.Now;

                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            return false;

        }
        Boolean IBackgroundSetupRepository.UpdateLastOrderPlacedDate(Int32 bulkOrderUploadID, Int32 currentUserID)
        {
            var bulkOrderUpload = _ClientDBContext.BulkOrderUploads.FirstOrDefault(x => x.BOU_ID == bulkOrderUploadID && x.BOU_IsActive && !x.BOU_IsDeleted);
            if (bulkOrderUpload.IsNotNull())
            {
                bulkOrderUpload.BOU_LastOrderPlacedDate = DateTime.Now;
                bulkOrderUpload.BOU_ModifiedByID = currentUserID;
                bulkOrderUpload.BOU_ModifiedOn = DateTime.Now;

                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        #endregion


        /// <summary>
        /// UAT-2326: Change SSN on D&A and Additional Documents to be masked.
        /// Method to get mapping ID of SSN from ams.BkgAttributeGroupMapping.
        /// </summary>
        /// <returns></returns>
        public Int32 GetBkgAttributeGroupMappingIDforSSN()
        {
            String BkgAttributeGroupMappingCodeForSSN = PersonalInformationAttGroup.SOCIAL_SECURITY_NUMBER.GetStringValue();
            return _ClientDBContext.BkgAttributeGroupMappings.Where(cond => cond.BAGM_Code.ToString() == BkgAttributeGroupMappingCodeForSSN && !cond.BAGM_IsDeleted).Select(sel => sel.BAGM_ID).FirstOrDefault();
        }

        #region UAT-2388 : GetAutomaticInvitationBackgroundPackages
        List<BackgroundPackage> IBackgroundSetupRepository.GetAutomaticInvitationBackgroundPackages(Int32 packageID)
        {
            var _lstBackgroundPackage = new List<BackgroundPackage>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@PackageID", packageID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAutomaticInvitationPackages", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BackgroundPackage pkgDetails = new BackgroundPackage();
                            pkgDetails.BPA_ID = dr["BPA_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["BPA_ID"]);
                            pkgDetails.BPA_Name = dr["BPA_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["BPA_Name"]);
                            _lstBackgroundPackage.Add(pkgDetails);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return _lstBackgroundPackage;
        }

        Boolean IBackgroundSetupRepository.GetAutomaticPackageInvitationSetting(Int32 packageID)
        {
            var automaticInvitationSettings = _ClientDBContext.PackageInvitationSettings.Where(s => s.PIS_BkgPkgID == packageID && !s.PIS_IsDeleted).FirstOrDefault();
            if (automaticInvitationSettings.IsNullOrEmpty())
                return false;
            else
                return automaticInvitationSettings.PIS_IsActive;
        }
        #endregion

        #region UAT-3268:- Get package rotation qualifying setting.
        public Boolean GetRotationQualifyingSetting(Int32 packageID)
        {
            return _ClientDBContext.BackgroundPackages.Where(cond => !cond.BPA_IsDeleted && cond.BPA_ID == packageID).Select(sel => sel.BPA_IsReqToQualifyInRotation).FirstOrDefault();
        }
        #endregion

        #region UAT-3525
        public List<BkgPackageType> GetAllBkgPackageTypes(String packageTypeName, String packageTypeCode, Int32 bkgPackageTypeId,String bkgPackageColorCode)
        {

            var _lstBackgroundPackageType = new List<BkgPackageType>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@PackageTypeId", bkgPackageTypeId),
                    new SqlParameter("@PackageTypeName", packageTypeName),
                    new SqlParameter("@PackageTypeCode", packageTypeCode),
                    new SqlParameter("@PackageTypeColorCode", bkgPackageColorCode)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAllBkgPackageType", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BkgPackageType pkgType = new BkgPackageType();
                            pkgType.BPT_Id = dr["BkgPackageTypeId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["BkgPackageTypeId"]);
                            pkgType.BPT_Name = dr["Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Name"]);
                            pkgType.BPT_Code = dr["Code"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Code"]);
                            pkgType.BPT_Color = dr["ColorCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ColorCode"]);
                            _lstBackgroundPackageType.Add(pkgType);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return _lstBackgroundPackageType;
        }

        public string DeletePackageType(int BkgPackageTypeId, int LoggedInUserId)
        {
            BkgPackageType pkgType = _ClientDBContext.BkgPackageTypes.Where(x => x.BPT_Id == BkgPackageTypeId && x.BPT_IsDeleted == false).FirstOrDefault();
            pkgType.BPT_IsDeleted = true;
            pkgType.BPT_ModifiedBy = LoggedInUserId;
            pkgType.BPT_ModifiedOn = DateTime.Now;

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return AppConsts.BPT_DELETED_SUCCESS_MSG;
            }
            else
            {
                return AppConsts.BPT_DELETED_ERROR_MSG;
            }
        }

        public bool SaveUpdatePackageType(BkgPackageTypeContract _packageTypeContract, int LoggedInUserId)
        {
            if (_packageTypeContract.BkgPackageTypeId != AppConsts.NONE)
            {
                BkgPackageType objPackageType = _ClientDBContext.BkgPackageTypes.Where(x => x.BPT_Id == _packageTypeContract.BkgPackageTypeId && x.BPT_IsDeleted == false).FirstOrDefault();
                objPackageType.BPT_Name = _packageTypeContract.BkgPackageTypeName;
                objPackageType.BPT_Code = _packageTypeContract.BkgPackageTypeCode;
                objPackageType.BPT_Color = _packageTypeContract.BkgPackageTypeColorCode;
                objPackageType.BPT_IsDeleted = false;
                objPackageType.BPT_ModifiedBy = LoggedInUserId;
                objPackageType.BPT_ModifiedOn = DateTime.Now;
            }
            else
            {
                BkgPackageType objPkgType = new BkgPackageType();
                objPkgType.BPT_Name = _packageTypeContract.BkgPackageTypeName;
                objPkgType.BPT_Code = _packageTypeContract.BkgPackageTypeCode;
                objPkgType.BPT_Color = _packageTypeContract.BkgPackageTypeColorCode;
                objPkgType.BPT_IsDeleted = false;
                objPkgType.BPT_CreatedBy = LoggedInUserId;
                objPkgType.BPT_CreatedOn = DateTime.Now;
                _ClientDBContext.BkgPackageTypes.AddObject(objPkgType);
            }
            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        public Boolean IsPackageMapped(int BkgPackageTypeId, int LoggedInUserId)
        {
            var lstBkgType = _ClientDBContext.BackgroundPackages.Where(x => x.BPA_BkgPackageTypeId == BkgPackageTypeId && x.BPA_IsDeleted == false).ToList();
            if (lstBkgType.Count > 0)
            {
                return true;
            }

            return false;

        }
        /// <summary>
        /// Get the Last code used for PackageType  
        /// </summary>
        /// <returns>String</returns>
        public String GetPackageTypeCode()
        {
            String code = String.Empty;

            if (_ClientDBContext.BkgPackageTypes.IsNotNull())
            {
                code = _ClientDBContext.BkgPackageTypes.OrderByDescending(cond => cond.BPT_CreatedOn).FirstOrDefault().BPT_Code;
            }

            return code;
        }

        public Boolean IsPackageTypeCodeAlreadyExists(String packageTypeCode, Int32 bkgPackageTypeId)
        {
            if (bkgPackageTypeId > AppConsts.NONE)
            {
                var lstBkgType = _ClientDBContext.BkgPackageTypes.Where(x => x.BPT_Code == packageTypeCode && x.BPT_Id != bkgPackageTypeId && x.BPT_IsDeleted == false).ToList();
                if (lstBkgType.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lstBkgType = _ClientDBContext.BkgPackageTypes.Where(x => x.BPT_Code == packageTypeCode && x.BPT_IsDeleted == false).ToList();
                if (lstBkgType.Count > 0)
                {
                    return true;
                }
            }

            return false;

        }
        public Boolean IsPackageTypeNameAlreadyExists(String packageTypeName, Int32 bkgPackageTypeId)
        {
            if (bkgPackageTypeId > AppConsts.NONE)
            {
                var lstBkgType = _ClientDBContext.BkgPackageTypes.Where(x => x.BPT_Name == packageTypeName && x.BPT_Id != bkgPackageTypeId && x.BPT_IsDeleted == false).ToList();
                if (lstBkgType.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lstBkgType = _ClientDBContext.BkgPackageTypes.Where(x => x.BPT_Name == packageTypeName && x.BPT_IsDeleted == false).ToList();
                if (lstBkgType.Count > 0)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

        #region UAT-3745

        public List<SystemDocBkgSvcMapping> GetAddtionalDocBkgSvcMapping(List<Int32> backgroundPackageIds, String additionalDocIds)
        {
            StringBuilder BPAIds = new StringBuilder();
            foreach (Int32 BPAID in backgroundPackageIds.Distinct())
            {
                BPAIds.Append(BPAID);
                BPAIds.Append(",");
            }
            List<SystemDocBkgSvcMapping> lstSystemDocBkgSvcMapping = new List<SystemDocBkgSvcMapping>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@BkgPackagesIDs", BPAIds.ToString()),
                            new SqlParameter("@AdditionalDocIDs", additionalDocIds.ToString()),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetAddtionalDocBkgSvcMapping", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SystemDocBkgSvcMapping systemDocBkgSvcMapping = new SystemDocBkgSvcMapping();

                            systemDocBkgSvcMapping.SystemDocumentID = dr["SystemDocumentID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SystemDocumentID"]);
                            systemDocBkgSvcMapping.BkgServiceID = dr["BkgServiceID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgServiceID"]);
                            systemDocBkgSvcMapping.ExtServiceID = dr["ExtServiceID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ExtServiceID"]);
                            systemDocBkgSvcMapping.BackgroundPackageID = dr["BackgroundPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BackgroundPackageID"]);

                            lstSystemDocBkgSvcMapping.Add(systemDocBkgSvcMapping);
                        }
                    }
                }
                return lstSystemDocBkgSvcMapping;
            }
        }

        #endregion

        #region UAT-4004

        List<Entity.ExternalVendorAccount> IBackgroundSetupRepository.GetExternalVendorAccount(Int32 vendorId)
        {
            return base.SecurityContext.ExternalVendorAccounts.Where(x => !x.EVA_IsDeleted && x.EVA_VendorID == vendorId).ToList();
        }

        #endregion
        #region UAT-4775
        public int GetContentType(String contentTypeCode)
        {
            return _ClientDBContext.lkpContentTypes.FirstOrDefault(x => x.CT_Code.Equals(contentTypeCode) && !x.CT_IsDeleted).CT_ID;
        }

        public int GetContentRecordType(String contentRecordTypeCode)
        {
            return _ClientDBContext.lkpContentRecordTypes.FirstOrDefault(x => x.CRT_Code.Equals(contentRecordTypeCode) && !x.CRT_IsDeleted).CRT_ID;
        }

        public Boolean SaveContentData(PageContent objPageContent)
        {
            PageContent content = _ClientDBContext.PageContents.Where(x => x.PC_ContentRecordID == objPageContent.PC_ContentRecordID && !x.PC_IsDeleted).FirstOrDefault();
            if (!content.IsNullOrEmpty())
            {
                content.PC_ContentRecordID = objPageContent.PC_ContentRecordID;
                content.PC_ContentRecordTypeID = objPageContent.PC_ContentRecordTypeID;
                content.PC_ContentTypeID = objPageContent.PC_ContentTypeID;
                content.PC_Content = objPageContent.PC_Content;
                content.PC_ModifiedBy = objPageContent.PC_CreatedBy;
                content.PC_ModifiedOn = objPageContent.PC_CreatedOn;
            }
            else
            {
                _ClientDBContext.PageContents.AddObject(objPageContent);
            }
            _ClientDBContext.SaveChanges();
            return true;
        }

        public PageContent GetContentData(Int32 dpmId)
        {
            return _ClientDBContext.PageContents.Where(Cond => Cond.PC_ContentRecordID == dpmId && !Cond.PC_IsDeleted).FirstOrDefault();
        }

        #endregion
    }
}


