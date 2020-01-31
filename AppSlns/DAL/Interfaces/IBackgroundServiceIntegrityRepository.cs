using System;

namespace DAL.Interfaces
{
    public interface IBackgroundServiceIntegrityRepository
    {

        #region Background  Package Management

        Boolean IfServiceGroupIsAssociated(Int32 svcGrpID);

        Boolean IfServiceAttributeIsAssociated(Int32 attributeId);

        #endregion

        #region Service Attribute Group Management

        Boolean IfServiceAttributeGroupIsAssociated(Int32 attributeGrpId);
        Boolean IfAttributeGroupMappingIsAssociated(Int32 attributeGrpId);

        #endregion

        #region Manage Master Services
        Boolean IsServiceMAppedToClient(Int32 bkgSvcMasterID);
        #endregion

        #region Custom Form Management

        Boolean CheckIfBkgSvcCustomFormMappingExist(Int32 customFormId);
        Boolean CheckIfBkgSvcisinUse(Int32 customFormId);
        Boolean CheckIfCustomFormCanBeDeleted(Int32 customFormId);

        #endregion

        #region SetUp background Institution Hierarchy
        Boolean IsChildNodeExistForNode(Int32 DeptprogMappingId);
        Boolean IsPackageMappedtoNode(Int32 DeptprogMappingId);
        Boolean IsPackageHasOrder(Int32 bkgPackageHierarchyMappingID);
        #endregion

        #region Background  Package Setup
        Boolean IfPackageMappingIsAssociated(Int32 packageID);
        Boolean IfServiceGroupMappingIsAssociated(Int32 svcGrpID, Int32 packageID);
        Boolean IfServiceMappingIsAssociated(Int32 serviceID,Int32 svcGrpID, Int32 packageID);
        Boolean IfAttributeMappingIsAssociated(Int32 packageID);
        #endregion

        #region Vendor Sevice Mapping

        /// <summary>
        /// Check if vendor service mapping can be deleted
        /// </summary>
        /// <param name="vendorSvcMappingId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        Boolean IfVendorServiceMappingIsAssociated(Int32 vendorSvcMappingId);        

        #endregion

        #region Institution Hierarchy Vendor Account Mapping
        
        /// <summary>
        /// Check if Institution Hierarchy Vendor Account Mapping can be deleted
        /// </summary>
        /// <param name="nDPMId"></param>
        /// <param name="vendorId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        Boolean IfVendorAccountMappingIsAssociated(Int32 nDPMId, Int32 vendorId);

        #endregion

        #region Order Status Colour Management
        Boolean CheckIfInstitutionOrderFlagBkgOrderMappingExist(Int32 institutionOrderFlagId);
        #endregion

        #region Manage Rule Templates

        Boolean IfRuleTemplateIsAssociated(Int32 ruleTemplateId);

        #endregion

        #region Manage Ruleset and Rules

        Boolean IfobjectRuleSetMappingIsAssociated(Int32 ruleSetId);

        #endregion

        #region Client Service Vendor
        Boolean IsClientSvcVendorStateMappingIsAssociated(Int32 masterBkgSvcID); 
        #endregion

        
        #region UAT-583 WB: AMS: Ability to delete attributes and attribute groups from the package setup screen (even after the attribute or attribute group is active)
        Boolean IfAttributeGroupAssociatedWithExtSVC(Int32 attributeGroupId, Int32 bkgServiceID, Int32 bkgPackageSvcId);
        Boolean IfAttributeAssociatedWithExtSVC(Int32 bkgPackageSvcAttributeMappingId, Int32 bkgServiceID);
        #endregion

        Boolean IsReviewCriteriaMapped(Int32 revwCriteriaID);
    }
}
