#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ComplianceItemMappingDetailPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using Business.RepoManagers;
using System.Linq;

#endregion

#region Application Specific
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
#endregion

#endregion

namespace CoreWeb.Mobility.Views
{
    public class ComplianceItemMappingDetailPresenter : Presenter<IComplianceItemMappingDetailView>
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Get Package Mapping Master Data based on PackageMappingMasterId. 
        /// </summary>
        public void GetPkgMappingMaster()
        {
            var pkgMappingMasterData = MobilityManager.GetPkgMappingMasterData(View.PackageMappingMasterId);
            if (pkgMappingMasterData.IsNotNull())
            {
                var pkgMappingMaster = pkgMappingMasterData.ToList().FirstOrDefault();
                View.FromPackageId = pkgMappingMaster.PMM_FromPackageID.IsNotNull() ? pkgMappingMaster.PMM_FromPackageID.Value : AppConsts.NONE;
                View.ToPackageId = pkgMappingMaster.PMM_ToPackageID.IsNotNull() ? pkgMappingMaster.PMM_ToPackageID.Value : AppConsts.NONE;
                View.FromTenantId = pkgMappingMaster.PMM_FromTenantID.IsNotNull() ? pkgMappingMaster.PMM_FromTenantID.Value : AppConsts.NONE;
                View.ToTenantId = pkgMappingMaster.PMM_ToTenantID.IsNotNull() ? pkgMappingMaster.PMM_ToTenantID.Value : AppConsts.NONE;
                View.MappingStatusID = pkgMappingMaster.PMM_MappingStatusID.IsNotNull() ? pkgMappingMaster.PMM_MappingStatusID.Value : Convert.ToInt16(AppConsts.NONE);
                View.FromTenantName = pkgMappingMaster.Tenant.TenantName;
                View.ToTenantName = pkgMappingMaster.Tenant1.TenantName;
                View.MappingName = pkgMappingMaster.PMM_Name != String.Empty ? pkgMappingMaster.PMM_Name : pkgMappingMaster.PMM_FromPackageName + " > " + pkgMappingMaster.PMM_ToPackageName;
                View.IsMappingDetailsSaved = pkgMappingMaster.PMM_IsDetailsSaved.IsNull() ? false : pkgMappingMaster.PMM_IsDetailsSaved.Value;
                View.IsSkipMapping = pkgMappingMaster.PMM_IsMappingSkipped.IsNull() ? false : pkgMappingMaster.PMM_IsMappingSkipped.Value;
                View.FromNodeID = pkgMappingMaster.PMM_FromNodeID.IsNotNull() ? pkgMappingMaster.PMM_FromNodeID.Value : AppConsts.NONE;
                View.ToNodeID = pkgMappingMaster.PMM_ToNodeID.IsNotNull() ? pkgMappingMaster.PMM_ToNodeID.Value : AppConsts.NONE;
            }
        }

        /// <summary>
        /// Get Source Package Data from Tenant Database based on tenant ID and Package ID.
        /// </summary>
        public void GetFromCompliancePackageData()
        {
            ObjectResult<GetRuleSetTree> objRuleSetTree = MobilityManager.GetRuleSetTreeForPackage(View.FromTenantId, Convert.ToString(View.FromPackageId));
            View.lstFromTreeData = objRuleSetTree.ToList();
        }

        /// <summary>
        /// Get Target Package Data from Tenant Database based on tenant ID and Package ID.
        /// </summary>
        public void GetToCompliancePackageData()
        {
            ObjectResult<GetRuleSetTree> objRuleSetTree = MobilityManager.GetRuleSetTreeForPackage(View.ToTenantId, Convert.ToString(View.ToPackageId));
            View.lstToTreeData = objRuleSetTree.ToList();
        }

        /// <summary>
        /// Get recommendation of mapped Items based on XML string containing source/target Item/Attribute list.
        /// On Package Mapping details screen mappping recommendation provided based on the latest value of either same Item/Attribute Name 
        /// betwen source Item/Attr and Target Item/Attr OR previous mapping saved betwen source Item/Attr and Target Item/Attr.
        /// </summary>
        public void GetMappedItemList()
        {
            ObjectResult<Entity.GetComplianceItemMappingDetails_Result> mappedItemList = MobilityManager.GetMappedItemList(View.ComplianceItemMappingXML);
            View.MappedItemList = mappedItemList.ToList();
        }

        /// <summary>
        /// Get recommendation of already saved mapped Items based on PackageMappingMasterId.
        /// </summary>
        public void GetSavedMappedItemDetails()
        {
            ObjectResult<Entity.GetComplianceItemMappingDetails_Result> mappedItemList = MobilityManager.GetSavedMappedItemDetails(View.PackageMappingMasterId);
            View.MappedItemList = mappedItemList.ToList();
        }

        /// <summary>
        /// Get List of Mapping Status
        /// </summary>
        public void GetPackageMappingStatus()
        {
            //View.lstMappingStatus = MobilityManager.GetMappingStatus().Where(cond => cond.PMS_Code != PkgMappingStatus.Reviewed_instance.GetStringValue()).ToList();
            View.lstMappingStatus = LookupManager.GetLookUpData<Entity.lkpPkgMappingStatu>().Where(cond =>cond.PMS_IsDeleted==false && cond.PMS_Code != PkgMappingStatus.Reviewed_instance.GetStringValue()).ToList();
        }
        
        /// <summary>
        /// Save Complaicne Item attribute mapping on click of Save Mapping button.
        /// </summary>
        /// <returns></returns>
        public Boolean SaveComplianceItmMapping()
        {
            List<Entity.ComplianceItmMappingDetail> cmplnceItmMapngList = new List<Entity.ComplianceItmMappingDetail>();
            if (!View.FromQueueType.IsNullOrEmpty())
            {
                //List<Entity.MappingRequest> lstMappingDetail = new List<Entity.MappingRequest>();
                //lstMappingDetail.Add(new Entity.MappingRequest
                //{
                //    FromPackageID = View.FromPackageId,
                //    ToPackageID = View.ToPackageId,
                //    FromTenantID = View.FromTenantId,
                //    ToTenantID = View.ToTenantId,
                //    FromPackageName = View.FromPackageName,
                //    ToPackageName = View.ToPackageName,
                //    MappingName = View.MappingName,

                //});
                // List<Entity.MappingRequest> lstMappingRequest = MobilityManager.AddInMappingQueue(lstMappingDetail, View.CurrentUserId);
                
                Entity.MappingRequestData mappingRequestData = new Entity.MappingRequestData
                {
                    FromTenantID = View.FromTenantId,
                    ToTenantID = View.ToTenantId,
                    FromPackageID = View.FromPackageId,
                    ToPackageID = View.ToPackageId,
                    FromPackageName = View.FromPackageName,
                    ToPackageName = View.ToPackageName,
                    FromNodeId = View.FromNodeID,
                    ToNodeId = View.ToNodeID
                };
                List<Entity.MappingRequestData> lstMappingRequest = new List<Entity.MappingRequestData>() { mappingRequestData };
                lstMappingRequest = MobilityManager.AddRecordsInMappingQueue(lstMappingRequest, View.CurrentUserId);

                if (lstMappingRequest.IsNotNull() && lstMappingRequest.Count == AppConsts.ONE)
                {
                    View.PackageMappingMasterId = lstMappingRequest.FirstOrDefault().MappingID;
                }
            }

            Entity.PkgMappingMaster pkgMappingMaster = new Entity.PkgMappingMaster();
            var pkgMappingMasterData = MobilityManager.GetPkgMappingMasterData(View.PackageMappingMasterId);
            if (pkgMappingMasterData.IsNotNull())
            {
                pkgMappingMaster = pkgMappingMasterData.ToList().FirstOrDefault();
                pkgMappingMaster.PMM_ModifiedByID = View.CurrentUserId;
                pkgMappingMaster.PMM_ModifiedOn = DateTime.Now;
                pkgMappingMaster.PMM_IsDetailsSaved = true;
                pkgMappingMaster.PMM_MappingStatusID = View.MappingStatusID;
                pkgMappingMaster.PMM_Name = View.MappingName;
                pkgMappingMaster.PMM_IsMappingSkipped = View.IsSkipMapping;
                //Save Reviewd_Date in mapping Master when status is reviewed.
                if (View.MappingStatusID == GetPkgMappingStatusIDByCode(PkgMappingStatus.Reviewed.GetStringValue()))
                {
                    pkgMappingMaster.PMM_ReviewDate = DateTime.Now;
                }
            }
            foreach (var item in View.MappedItemsDetailsList)
            {
                Entity.ComplianceItmMappingDetail cmplnceItmMapng = new Entity.ComplianceItmMappingDetail();
                if (!item.IsDeleted && item.isSaveNeeded)
                {
                    Entity.ComplianceDataPoint cmplaceDataPoint = new Entity.ComplianceDataPoint();
                    cmplaceDataPoint.CDP_IsDeleted = false;
                    cmplaceDataPoint.CDP_CreatedByID = View.CurrentUserId;
                    cmplaceDataPoint.CDP_CreatedOn = DateTime.Now;

                    // From/Destination Tenant Compliance Itm Mapping Details
                    cmplnceItmMapng.CIMD_AttributeID = item.FromAttributeID;
                    cmplnceItmMapng.CIMD_ItemID = item.FromItemID;
                    cmplnceItmMapng.CIMD_IsDeleted = false;
                    cmplnceItmMapng.CIMD_MappingMasterID = View.PackageMappingMasterId;
                    cmplnceItmMapng.CIMD_MappingTypeID = MobilityManager.GetPkgMappingTypeStatusIDByCode(PkgMappingType.From.GetStringValue());
                    cmplnceItmMapng.CIMD_TenantID = View.FromTenantId;
                    cmplnceItmMapng.CIMD_CreatedByID = View.CurrentUserId;
                    cmplnceItmMapng.CIMD_CreatedOn = DateTime.Now;

                    cmplaceDataPoint.ComplianceItmMappingDetails.Add(cmplnceItmMapng);
                    cmplnceItmMapngList.Add(cmplnceItmMapng);

                    // To/Destination Tenant Compliance Itm Mapping Details
                    cmplnceItmMapng = new Entity.ComplianceItmMappingDetail();
                    cmplnceItmMapng.CIMD_AttributeID = item.ToAttributeID;
                    cmplnceItmMapng.CIMD_ItemID = item.ToItemID;
                    cmplnceItmMapng.CIMD_IsDeleted = false;
                    cmplnceItmMapng.CIMD_MappingMasterID = View.PackageMappingMasterId;
                    cmplnceItmMapng.CIMD_MappingTypeID = MobilityManager.GetPkgMappingTypeStatusIDByCode(PkgMappingType.To.GetStringValue());
                    cmplnceItmMapng.CIMD_CreatedByID = View.CurrentUserId;
                    cmplnceItmMapng.CIMD_TenantID = View.ToTenantId;
                    cmplnceItmMapng.CIMD_CreatedOn = DateTime.Now;
                    cmplaceDataPoint.ComplianceItmMappingDetails.Add(cmplnceItmMapng);
                    cmplnceItmMapngList.Add(cmplnceItmMapng);
                }
                else if (item.IsDeleted && !item.isSaveNeeded)
                {
                    var getFromMapping = MobilityManager.GetComplianceItemMappingDetails(View.FromTenantId, View.PackageMappingMasterId, item.FromItemID.Value, item.FromAttributeID.Value);
                    var getToMapping = MobilityManager.GetComplianceItemMappingDetails(View.ToTenantId, View.PackageMappingMasterId, item.ToItemID.Value, item.ToAttributeID.Value);

                    getFromMapping.CIMD_IsDeleted = true;
                    getFromMapping.CIMD_ModifiedByID = View.CurrentUserId;
                    getFromMapping.CIMD_ModifiedOn = DateTime.Now;
                    cmplnceItmMapngList.Add(getFromMapping);
                    getToMapping.CIMD_IsDeleted = true;
                    getToMapping.CIMD_ModifiedByID = View.CurrentUserId;
                    getToMapping.CIMD_ModifiedOn = DateTime.Now;
                    cmplnceItmMapngList.Add(getToMapping);
                }
            }

            //Save Mapping
            MobilityManager.SaveComplianceItmMapping(cmplnceItmMapngList);

            //When skip is checked, update skip column in order table (for those orders which have mappinginstanceid null and skipmapping is null and mapping id matches)
            if (pkgMappingMaster.IsNotNull() && pkgMappingMaster.PMM_IsMappingSkipped.Equals(true) && pkgMappingMaster.PMM_ToTenantID.IsNotNull())
            {
                MobilityManager.UpdateOrderSkippedMapping(pkgMappingMaster, pkgMappingMaster.PMM_ToTenantID.Value);
            }

            return true;
        }

        /// <summary>
        /// Check Whether source attribute and target attribute are of same data type.
        /// </summary>
        /// <returns></returns>
        public Boolean IsAttributeDataTypeSame()
        {
            var fromAttr = ComplianceSetupManager.GetComplianceAttribute(View.FromAttrributeID, View.FromTenantId);
            var toAttr = ComplianceSetupManager.GetComplianceAttribute(View.ToAttrributeID, View.ToTenantId);

            if (fromAttr.IsNotNull() && toAttr.IsNotNull() && fromAttr.ComplianceAttributeDatatypeID == toAttr.ComplianceAttributeDatatypeID)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void generateMappingReviewInstanceAndUpdatePendingSubscription()
        {
            MobilityManager.generateMappingReviewInstanceAndUpdatePendingSubscription(View.PackageMappingMasterId, View.ToTenantId, View.CurrentUserId);
        }

        /// <summary>
        /// Get Package Mapping Status Id based on code value.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Int16? GetPkgMappingStatusIDByCode(String code)
        {

            var lkpPakgMappingStatu = LookupManager.GetLookUpData<Entity.lkpPkgMappingStatu>().Where(cond => cond.PMS_IsDeleted == false && cond.PMS_Code == code).FirstOrDefault();
            if (lkpPakgMappingStatu.IsNotNull())
            {
                return lkpPakgMappingStatu.PMS_ID;
            }
            return null;
            //return MobilityManager.GetPkgMappingStatusIDByCode(code);
        }

        /// <summary>
        /// Get Source and Target node details name/label based on source node ID and target Node ID 
        /// </summary>
        public void GetNodesDetails()
        {
            View.ToNodeName = MobilityManager.GetNodesDetails(View.ToNodeID, View.ToTenantId);
            View.FromNodeName = MobilityManager.GetNodesDetails(View.FromNodeID, View.FromTenantId);
        }
        public Int32 GetMappingMasterId(Int32 sourcePackageId, Int32 targetPackageId, Int32 sourceTenantId, Int32 targetTenantId, Int32 sourceNodeId, Int32? targetNodeId)
        {
            return MobilityManager.GetMappingData(sourcePackageId, targetPackageId, sourceTenantId, targetTenantId, sourceNodeId, targetNodeId);
        }
        #endregion

        #region Private methods

        #endregion

        #endregion
    }
}