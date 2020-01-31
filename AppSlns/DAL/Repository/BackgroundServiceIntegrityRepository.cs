using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class BackgroundServiceIntegrityRepository : ClientBaseRepository, IBackgroundServiceIntegrityRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public BackgroundServiceIntegrityRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;

        }

        #region Methods

        #region Background Package Management

        public Boolean IfServiceGroupIsAssociated(Int32 svcGrpID)
        {
            if (IfBkgPackageExistForServiceGrp(svcGrpID) || IfBkgOrderExistForServiceGrp(svcGrpID))
            {
                return true;
            }
            return false;
        }

        public Boolean IfBkgPackageExistForServiceGrp(Int32 svcGrpID)
        {
            return _ClientDBContext.BkgPackageSvcGroups.Any(condition => condition.BPSG_BkgSvcGroupID == svcGrpID && !condition.BPSG_IsDeleted);
        }

        public Boolean IfBkgOrderExistForServiceGrp(Int32 svcGrpID)
        {
            return _ClientDBContext.BkgOrderPackageSvcGroups.Any(condition => condition.OPSG_BkgSvcGroupID == svcGrpID && !condition.OPSG_IsDeleted);
        }

        public Boolean IfServiceAttributeIsAssociated(Int32 attributeId)
        {
            return IfServiceAttributeMappingExist(attributeId);
        }

        public Boolean IfServiceAttributeMappingExist(Int32 attributeId)
        {
            return _ClientDBContext.BkgAttributeGroupMappings.Any(condition => condition.BAGM_BkgSvcAtributeID == attributeId && condition.BAGM_IsDeleted == false);
        }

        #endregion

        #region Service Attribute Group Management

        public Boolean IfServiceAttributeGroupIsAssociated(Int32 attributeGrpId)
        {
            if (IfServiceAttributeGroupMappingExist(attributeGrpId) || IfCustomFormAttributeGroupExistForAttributeGroup(attributeGrpId))
            {
                return true;
            }
            return false;
        }

        public Boolean IfServiceAttributeGroupMappingExist(Int32 attributeGrpId)
        {
            return _ClientDBContext.BkgAttributeGroupMappings.Any(condition => condition.BAGM_BkgSvcAttributeGroupId == attributeGrpId && !condition.BAGM_IsDeleted);
        }

        public Boolean IfCustomFormAttributeGroupExistForAttributeGroup(Int32 attributeGrpId)
        {
            return base.SecurityContext.CustomFormAttributeGroups.Any(condition => condition.CFAG_BkgSvcAttributeGroupId == attributeGrpId && !condition.CFAG_IsDeleted);
        }

        public Boolean IfAttributeGroupMappingIsAssociated(Int32 attributeGrpMappingId)
        {
            return IfSvcAttributeGroupMappingExist(attributeGrpMappingId);
        }

        public Boolean IfSvcAttributeGroupMappingExist(Int32 attributeGrpMappingId)
        {
            return _ClientDBContext.BkgSvcAttributeGroupMappings.Any(condition => condition.BSAGM_AttributeGroupMappingID == attributeGrpMappingId && !condition.BSAGM_IsDeleted);
        }

        public Boolean IsServiceMAppedToClient(Int32 bkgSvcMasterID)
        {
            Entity.BackgroundService bkgService = base.SecurityContext.BackgroundServices.Where(x => x.BSE_ID == bkgSvcMasterID && !x.BSE_IsDeleted).FirstOrDefault();
            if (bkgService.IsNotNull() && bkgService.BSE_ClientCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Manage Custom Forms

        /// <summary>
        /// Checks if the CustomForm Service mapping exists in master DB.
        /// </summary>
        /// <param name="customFormId">customFormId</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfBkgSvcCustomFormMappingExist(Int32 customFormId)
        {
            return base.SecurityContext.BkgSvcFormMappings.Any(obj => obj.BSFM_CustomFormId == customFormId && obj.BSFM_IsDeleted == false);
        }

        /// <summary>
        /// Checks if the CustomForm & Service used in clients.
        /// </summary>
        /// <param name="customFormId">customFormId</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfBkgSvcisinUse(Int32 customFormId)
        {
            int bkgSvcId = Convert.ToInt32(base.SecurityContext.BkgSvcFormMappings.Where(obj => obj.BSFM_CustomFormId == customFormId && obj.BSFM_IsDeleted == false).Select(obj => obj.BSFM_BackgroundServiceID));
            return base.SecurityContext.BackgroundServices.Any(obj => obj.BSE_ID == bkgSvcId && obj.BSE_ClientCount > 0);
        }


        /// <summary>
        /// Checks if the CustomForm Can Be Deleted.
        /// </summary>
        /// <param name="customFormId">customFormId</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfCustomFormCanBeDeleted(Int32 customFormId)
        {
            Boolean result;
            result = base.SecurityContext.CustomFormAttributeGroups.Any(obj => obj.CFAG_CustomFormId == customFormId && obj.CFAG_IsDeleted == false) || CheckIfBkgSvcCustomFormMappingExist(customFormId);
            return result;
        }

        #endregion

        #region SetUp BkgInstitution Hirarchy

        public Boolean IsChildNodeExistForNode(Int32 DeptprogMappingId)
        {
            return _ClientDBContext.DeptProgramMappings.Any(x => x.DPM_ParentNodeID == DeptprogMappingId && !x.DPM_IsDeleted);

        }
        public Boolean IsPackageMappedtoNode(Int32 DeptprogMappingId)
        {
            if (_ClientDBContext.DeptProgramPackages.Any(x => x.DPP_DeptProgramMappingID == DeptprogMappingId && !x.DPP_IsDeleted)
                || _ClientDBContext.BkgPackageHierarchyMappings.Any(x => x.BPHM_InstitutionHierarchyNodeID == DeptprogMappingId && !x.BPHM_IsDeleted))
            {
                return true;
            }
            return false;
        }

        public Boolean IsPackageHasOrder(Int32 bkgPackageHierarchyMappingID)
        { //has order you cant delete 
            return _ClientDBContext.BkgOrderPackages.Include("BkgOrder.Order")
                .Where(x => x.BOP_BkgPackageHierarchyMappingID == bkgPackageHierarchyMappingID && !x.BOP_IsDeleted && !x.BkgOrder.BOR_IsDeleted && !x.BkgOrder.Order.IsDeleted).Any();

        }
        #endregion

        #region Background Package Management


        public Boolean IfPackageMappingIsAssociated(Int32 packageID)
        {
            if (IfBkgPackageHierarchyMappingExist(packageID) || IfBkgPackageServiceGroupExist(packageID))
            {
                return true;
            }
            return false;
        }

        public Boolean IsPackageOrderIsPlaced(Int32 packageID)
        {
            return (ClientDBContext.BkgOrderPackages.Any(x => !x.BOP_IsDeleted
                && x.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID == packageID
                && !x.BkgPackageHierarchyMapping.BPHM_IsDeleted));
        }

        public Boolean IfBkgPackageHierarchyMappingExist(Int32 packageID)
        {
            return _ClientDBContext.BkgPackageHierarchyMappings.Any(condition => condition.BPHM_BackgroundPackageID == packageID && !condition.BPHM_IsDeleted);
        }

        public Boolean IfBkgPackageServiceGroupExist(Int32 packageID)
        {
            return _ClientDBContext.BkgPackageSvcGroups.Any(condition => condition.BPSG_BackgroundPackageID == packageID && !condition.BPSG_IsDeleted);
        }

        public Boolean IfServiceGroupMappingIsAssociated(Int32 svcGrpID, Int32 pacakageID)
        {
            Int32 bkgPackageSvcGroupID = _ClientDBContext.BkgPackageSvcGroups
                                        .Where(cond => cond.BPSG_BackgroundPackageID == pacakageID && cond.BPSG_BkgSvcGroupID == svcGrpID && !cond.BPSG_IsDeleted)
                                        .Select(col => col.BPSG_ID).FirstOrDefault();
            return _ClientDBContext.BkgPackageSvcs.Any(cond => cond.BPS_BkgPackageSvcGroupID == bkgPackageSvcGroupID && !cond.BPS_IsDeleted);
        }

        public Boolean IfServiceMappingIsAssociated(Int32 serviceId, Int32 svcGrpID, Int32 packageId)
        {
            //Commneted below code for UAT-1030:WB: Issue in deletion of Services in Complio Background
            //Int32 bkgPackageSvcGroupID = _ClientDBContext.BkgPackageSvcGroups
            //                       .Where(cond => cond.BPSG_BackgroundPackageID == packageId && cond.BPSG_BkgSvcGroupID == svcGrpID && !cond.BPSG_IsDeleted)
            //                       .Select(col => col.BPSG_ID).FirstOrDefault();
            //Int32 bkgPackageSvcID = _ClientDBContext.BkgPackageSvcs
            //                        .Where(cond => cond.BPS_BkgPackageSvcGroupID == bkgPackageSvcGroupID && cond.BPS_BackgroundServiceID == serviceId && !cond.BPS_IsDeleted)
            //                        .Select(col => col.BPS_ID).FirstOrDefault();
            //Commneted below code for UAT-1030:WB: Issue in deletion of Services in Complio Background
            //if (IfBkgPackageSvcAttributeExist(bkgPackageSvcID) || IfPackageServiceItemExist(bkgPackageSvcID) || IsPackageOrderIsPlaced(packageId))
            if (IsPackageOrderIsPlaced(packageId))
            {
                return true;
            }
            return false;
        }

        public Boolean IfBkgPackageSvcAttributeExist(Int32 bkgPackageSvcID)
        {
            return _ClientDBContext.BkgPackageSvcAttributes.Any(condition => condition.BPSA_BkgPackageSvcID == bkgPackageSvcID && !condition.BPSA_IsDeleted);
        }

        public Boolean IfPackageServiceItemExist(Int32 bkgPackageSvcID)
        {
            return _ClientDBContext.PackageServiceItems.Any(condition => condition.PSI_PackageServiceID == bkgPackageSvcID && !condition.PSI_IsDeleted);
        }

        public Boolean IfAttributeMappingIsAssociated(Int32 packageId)
        {
            if (IsPackageOrderIsPlaced(packageId))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Vendor Sevice Mapping

        /// <summary>
        /// Check if vendor service mapping can be deleted
        /// </summary>
        /// <param name="vendorSvcMappingId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        public Boolean IfVendorServiceMappingIsAssociated(Int32 vendorSvcMappingId)
        {
            if (IfExternalSvcAttributeMappingExist(vendorSvcMappingId) || IfClientExtSvcVendorMappingExist(vendorSvcMappingId))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if Vendor Service Attribute Mapping exists
        /// </summary>
        /// <param name="vendorSvcMappingId"></param>
        /// <returns>True/False</returns>
        public Boolean IfExternalSvcAttributeMappingExist(Int32 vendorSvcMappingId)
        {
            return base.SecurityContext.ExternalSvcAtributeMappings.Any(condition => condition.ESAM_ServiceMappingId == vendorSvcMappingId && !condition.ESAM_IsDeleted);
        }

        /// <summary>
        /// Check if Client Mapping with Vendor Service Mapping Exits
        /// </summary>
        /// <param name="vendorSvcMappingId"></param>
        /// <returns>True/False</returns>
        public Boolean IfClientExtSvcVendorMappingExist(Int32 vendorSvcMappingId)
        {
            return base.SecurityContext.ClientExtSvcVendorMappings.Any(condition => condition.CESVM_BkgSvcExtSvcMappingID == vendorSvcMappingId && !condition.CESVM_IsDeleted);
        }

        #endregion



        #region Institution Hierarchy Vendor Account Mapping

        /// <summary>
        /// Check if Institution Hierarchy Vendor Account Mapping can be deleted
        /// </summary>
        /// <param name="nDPMId"></param>
        /// <param name="vendorId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        public Boolean IfVendorAccountMappingIsAssociated(Int32 nDPMId, Int32 vendorId)
        {
            if (IfDeptProgMappedWithOrder(nDPMId) || IfExtVendorMappedWithBkgOrderPackageSvcLineItem(vendorId))
            {
                return true;
            }
            return false;
        }

        public Boolean IfDeptProgMappedWithOrder(Int32 nDPMId)
        {
            return _ClientDBContext.Orders.Any(cond => cond.HierarchyNodeID == nDPMId && !cond.IsDeleted);
        }

        public Boolean IfExtVendorMappedWithBkgOrderPackageSvcLineItem(Int32 vendorId)
        {
            return _ClientDBContext.BkgOrderPackageSvcLineItems.Any(cond => cond.PSLI_VendorID == vendorId && !cond.PSLI_IsDeleted);
        }

        #endregion
        #region Manage Rule Templates

        /// <summary>
        /// check whether Rule Template is associated with a Rule
        /// </summary>
        /// <param name="ruleTemplateId"></param>
        /// <returns></returns>
        public Boolean IfRuleTemplateIsAssociated(Int32 ruleTemplateId)
        {
            if (_ClientDBContext.BkgRuleMappings.Any(condition => condition.BRLM_RuleTemplateID == ruleTemplateId && condition.BRLM_IsDeleted == false))
            {
                List<Int32> ruleSetIds = _ClientDBContext.BkgRuleMappings.Where(condition => condition.BRLM_RuleTemplateID == ruleTemplateId && condition.BRLM_IsDeleted == false)
                                            .Select(x => x.BRLM_RuleSetID).ToList();
                return _ClientDBContext.BkgRuleSets.Where(obj => ruleSetIds.Contains(obj.BRLS_ID)
                                                        && obj.BRLS_IsDeleted == false).Any();
            }
            return false;
        }

        #endregion

        #region Manage Ruleset and Rules

        /// <summary>
        /// Check if RuleSetObject mapping is associated with any rule.
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        public Boolean IfobjectRuleSetMappingIsAssociated(Int32 ruleSetId)
        {
            return IfRuleSetRuleMappingExist(ruleSetId);
        }

        /// <summary>
        /// Check whether for a given ruleset any RuleSetRule mapping exist.
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        public Boolean IfRuleSetRuleMappingExist(Int32 ruleSetId)
        {
            return _ClientDBContext.BkgRuleMappings.Any(condition => condition.BRLM_RuleSetID == ruleSetId && condition.BRLM_IsDeleted == false);
        }

        #endregion

        #region Client Service Vendor
        /// <summary>
        /// check whether This backgroubd service is used in Client to map with the Package and to the Node.
        /// </summary>
        /// <param name="masterBkgSvcID">masterBkgSvcID</param>
        /// <returns></returns>
        public Boolean IsClientSvcVendorStateMappingIsAssociated(Int32 masterBkgSvcID)
        { //if false u can delete
            Boolean IsMAppedToPackage = _ClientDBContext.BkgPackageSvcs.Any(x => x.BPS_BackgroundServiceID == masterBkgSvcID && !x.BPS_IsDeleted);
            //if true check for Package node mapping
            if (IsMAppedToPackage)
            {
                Int32 BkgPackID = _ClientDBContext.BkgPackageSvcs.Include("BkgPackageSvcGroups").Where(x => x.BPS_BackgroundServiceID == masterBkgSvcID && !x.BPS_IsDeleted
                                 && !x.BkgPackageSvcGroup.BPSG_IsDeleted).Select(x => x.BkgPackageSvcGroup.BPSG_BackgroundPackageID).FirstOrDefault();
                if (!(BkgPackID.IsNullOrEmpty()) && BkgPackID > 0)
                {
                    return _ClientDBContext.BkgPackageHierarchyMappings.Any(x => x.BPHM_BackgroundPackageID == BkgPackID && !x.BPHM_IsDeleted);
                }
                return false;
            }
            return false;
        }

        #endregion

        #region Manage Order Color Status
        /// <summary>
        /// Checks if the InstitutionOrderFlags mapping exist with BkgOrders.
        /// </summary>
        /// <param name="institutionOrderFlagId">institutionOrderFlagId</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfInstitutionOrderFlagBkgOrderMappingExist(Int32 institutionOrderFlagId)
        {
            return _ClientDBContext.BkgOrders.Any(obj => obj.BOR_InstitutionStatusColorID == institutionOrderFlagId && !obj.BOR_IsDeleted);
        }
        #endregion


        #region UAT-583 WB: AMS: Ability to delete attributes and attribute groups from the package setup screen (even after the attribute or attribute group is active)

        /// <summary>
        /// Method to check attribute group is associated with external services or not.
        /// </summary>
        /// <param name="attributeGroupId">attributeGroupId</param>
        /// <param name="bkgServiceID">bkgServiceID</param>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        public Boolean IfAttributeGroupAssociatedWithExtSVC(Int32 attributeGroupId, Int32 bkgServiceID, Int32 bkgPackageSvcId)
        {
            List<Int32> lstBkgAttributeMappingIds = new List<Int32>();
            var tempListBkgAttributeMapping = ClientDBContext.BkgPackageSvcAttributes.Where(cond => cond.BPSA_BkgPackageSvcID == bkgPackageSvcId && !cond.BPSA_IsDeleted && cond.BkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId == attributeGroupId && cond.BkgAttributeGroupMapping.BAGM_IsDeleted == false).ToList();
            if (tempListBkgAttributeMapping.IsNotNull() && tempListBkgAttributeMapping.Count > 0)
            {
                lstBkgAttributeMappingIds = tempListBkgAttributeMapping.Select(slct => slct.BkgAttributeGroupMapping.BAGM_ID).ToList();
                if (lstBkgAttributeMappingIds.Count > 0)
                {
                    var tempBkgSvcAttributeMappingList = base.SecurityContext.BkgSvcAttributeGroupMappings.Where(x => lstBkgAttributeMappingIds.Contains(x.BSAGM_AttributeGroupMappingID.Value) && x.BSAGM_ServiceId == bkgServiceID && x.BSAGM_IsDeleted == false).ToList();
                    if (tempBkgSvcAttributeMappingList.IsNotNull() && tempBkgSvcAttributeMappingList.Count > 0)
                    {
                        List<Int32> tempListBkgSvcAttributeMapping = tempBkgSvcAttributeMappingList.Select(slt => slt.BSAGM_ID).ToList();
                        return base.SecurityContext.ExternalSvcAtributeMappings.Include("ams.BkgSvcExtSvcMappings").Any(cnd => tempListBkgSvcAttributeMapping.Contains(cnd.ESAM_BkgSvcAttributeGroupMappingID.Value) && cnd.BkgSvcExtSvcMapping.BSESM_BkgSvcId == bkgServiceID && !cnd.BkgSvcExtSvcMapping.BSESM_IsDeleted && !cnd.ESAM_IsDeleted);
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///  Method to check attribute is associated with external services or not.
        /// </summary>
        /// <param name="bkgPackageSvcAttributeMappingId">bkgPackageSvcAttributeMappingId</param>
        /// <param name="bkgServiceID">bkgServiceID</param>
        /// <returns></returns>
        public Boolean IfAttributeAssociatedWithExtSVC(Int32 bkgPackageSvcAttributeMappingId, Int32 bkgServiceID)
        {
            var bkgPackageSvcAttributes = ClientDBContext.BkgPackageSvcAttributes.FirstOrDefault(cond => cond.BPSA_ID == bkgPackageSvcAttributeMappingId && !cond.BPSA_IsDeleted);
            if (bkgPackageSvcAttributes.IsNotNull())
            {
                var tempBkgSvcAttributeMapping = base.SecurityContext.BkgSvcAttributeGroupMappings.FirstOrDefault(x => x.BSAGM_AttributeGroupMappingID.Value == bkgPackageSvcAttributes.BPSA_BkgAttributeGroupMappingID && x.BSAGM_ServiceId == bkgServiceID && x.BSAGM_IsDeleted == false);
                if (tempBkgSvcAttributeMapping.IsNotNull())
                {
                    return base.SecurityContext.ExternalSvcAtributeMappings.Include("ams.BkgSvcExtSvcMappings").Any(cnd => cnd.ESAM_BkgSvcAttributeGroupMappingID.Value == tempBkgSvcAttributeMapping.BSAGM_ID && cnd.BkgSvcExtSvcMapping.BSESM_BkgSvcId == bkgServiceID && !cnd.BkgSvcExtSvcMapping.BSESM_IsDeleted && !cnd.ESAM_IsDeleted);
                }
            }
            return false;
        }
        #endregion

        public bool IsReviewCriteriaMapped(int revwCriteriaID)
        {
            return _ClientDBContext.BkgReviewCriteriaHierarchyMappings.Any(cond => cond.BRCHM_BkgReviewCriteriaID == revwCriteriaID && !cond.BRCHM_IsDeleted);
        }

        #endregion



    }
}
