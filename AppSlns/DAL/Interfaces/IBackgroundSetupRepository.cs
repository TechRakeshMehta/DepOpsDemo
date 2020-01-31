using Entity.ClientEntity;
using INTSOF.UI.Contract;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DAL.Interfaces
{
    public interface IBackgroundSetupRepository
    {
        #region Package Set Up

        List<BackgroundPackage> GetPackageData();
        String SaveEditPackagedetail(BackgroundPackage backgroundPackage, Int32 packageId, Boolean isEdit, Int32 currentLoggedInUserID, List<Int32> targetPackageIds, Int32 months, Boolean isActive);

        BackgroundPackage GetPackageDetail(Int32 packageId);
        Boolean DeletePackageMapping(Int32 packageId);
        List<LocalAttributeGroupMappedToBkgPackage> GetAttributeGroupMappedToBkgPackage(Int32 bkgPackageID);
        BkgPkgAttributeGroupInstruction GetBkgPkgAttributeGroupInstructionText(Int32 bkgPackageId, Int32 attrGrpId);
        Boolean SaveBkgPkgAttributeGroupInstruction(BkgPkgAttributeGroupInstruction bkgPkgAttrGrpInstructionObj);

        #endregion

        #region Map Service group  with Packages

        List<BkgSvcGroup> GetServiceGroupGridData(Int32 packageId);
        String SaveEditServiceGroupDetail(BkgPackageSvcGroup bkgPackageSvcGroup, Int32 serviceGroupId, Boolean isEdit);
        Boolean DeleteServiceGroupMapping(Int32 serviceGroupId, Int32 packageId);

        #endregion

        #region Map Service with service group

        List<BackgroundService> GetServicesForGridData(Int32 serviceGroupId, Int32 packageId);

        BkgPackageSvc GetCurrentBkgPkgService(Int32 serviceGroupId, Int32 packageId, Int32 serviceId);

        List<BackgroundService> GetServicesForDropDown(List<Int32> lstSvcToBeRemoved);
        List<BkgSvcGroup> GetServiceGroupForDropDown(List<Int32> lstSvcToBeRemoved);

        BkgPackageSvcGroup GetServicesGroupForEdit(Int32 serviceGroupId);

        String MapServiceWithServiceGroup(Int32 serviceId, Int32 serviceGroupId, Int32 packageId, Boolean isDelete, String displayName, String notes, Int32? pkgCount, Int32? minOccurrences, Int32? maxOccurrences, Int32? residentialDuration,
                                          Boolean sendDocsToStudent, Boolean isSupplemental, Boolean ignoreRHOnSupplement, Boolean isReportable, String bkgPkgSvcOverrideData = "");
        List<Int32> GetServicesByPackageId(Int32 packageId);

        #endregion

        #region Manage Attribute Group
        DataTable GetMappedAttributeGroupList(Int32 serviceId, Int32 bkgSvcGroupId, Int32 backgroundPackageId);
        Boolean UpdateChanges();
        DataTable GetMappedAttributeList(Int32 serviceId, Int32 bkgSvcGroupId, Int32 attributeGroupId, Int32 backgroundPackageId);
        List<AttributeDataSecurityClient> GetAllAttribute(List<Int32> mappedSvcAttibuteIds, Int32 attributeGroupId);
        Boolean DeletedBkgSvcAttributeMapping(Int32 bkgPackageSvcAttributeId, Int32 currentloggedInUserId);
        #endregion

        String IsmappingOfThisTypeAllowed(String attributeType, Int32 groupId);


        #region Manage Attribute
        BkgSvcAttribute GetBkgSvcAttribute(Int32 attributeId);
        void UpdateBkgSvcAttributeSecurity(ServiceAttributeContract serviceAttributeContract, Int32 currentLoggedInUserId);
        BkgPackageSvcAttribute GetBkgPackageSvcAttribute(Int32 serviceAttributeID);
        ManageServiceAttributeData GetBkgSvcAttributeData(ServiceAttributeParameter serviceAttributeParameter, Int32 tenantId);
        Boolean CopyAttributeAndMappingInTenant(BkgSvcAttribute bkgSvcAttribute, Entity.BkgSvcAttribute bkgSvcAttributeMaster, Int32 attributeGroupId, Int32 bkgPackageSvcId, Int32 currentLoggedInUserId, Boolean isRequired, Boolean isDisplay,Boolean IsHiddenFromUI);
        BkgAttributeGroupMapping GetAttributeMappingByAttributeGroupID(Int32 attributeGroupId, Int32 selectedAttributeId);
        Entity.BkgAttributeGroupMapping CheckAddForSecurityAttribute(Int32 attributeGroupId, Int32 selectedAttributeId);
        Boolean CopyAttributeAndGroupMappingInChild(Entity.BkgAttributeGroupMapping attributeMappingToAdd, Int32 bkgPackageSvcId, Boolean isRequired, Boolean isDisplay);
        Int32 GetBkgPackageSvcId(Int32 serviceId, Int32 bkgSvcGroupId, Int32 backgroundPackageId);
        Boolean SavePackageSvcAttributeMapping(BkgAttributeGroupMapping attributeMappingToAdd, Int32 bkgPackageSvcId, Int32 currentLoggedInUserId, Boolean isRequired, Boolean isDisplay);
        Boolean SaveAttributeOptionInClient(System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> attrOptionListMaster, Int32 currentLoggedInUserId);
        #endregion

        #region Background Package Administration

        List<BkgSvcGroup> GetServiceGroups();

        BkgSvcGroup SaveServiceGroupDetail(BkgSvcGroup category, Int32 currentLoggedInUserId);

        /// <summary>
        /// Checks if the category name already exists.
        /// </summary>
        /// <param name="categoryName">Category Name</param>
        /// <param name="complianceCategoryId">Compliance Category Id</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>True or false</returns>
        Boolean CheckIfServiceGroupNameAlreadyExist(String svcGrpName, Int32 svcGrpID);

        /// <summary>
        /// Updates the Service Group.
        /// </summary>
        /// <param name="svcGrp">BkgSvcGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        void UpdateServiceGroupDetail(BkgSvcGroup svcGroup, Int32 svcGrpID, Int32 currentLoggedInUserId);

        BkgSvcGroup getCurrentServiceGroupInfo(Int32 svcGrpID);

        Boolean DeleteServiceGroup(Int32 svcGrpID, Int32 currentUserId);

        #endregion

        #region SetUpBkgInstitutionhierarchy

        /// <summary>
        /// Get Institute Hierarchy Tree For Background data
        /// </summary>
        /// <param name="orgUserID">optional parameter in case of super admin pass null</param>
        /// <returns></returns>
        List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> GetBackgroundInstituteHierarchyTree(int? orgUserID);

        /// <summary>
        /// To get Program Packages by HierarchyMappingIdId
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        List<BkgPackageHierarchyMapping> GetProgramPackagesByHierarchyMappingId(Int32 deptProgramMappingID);

        /// <summary>
        /// To get not mapped Compliance Packages
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        List<BackgroundPackage> GetNotMappedBackGroungPackagesByMappingId(Int32 deptProgramMappingID);

        // <summary>
        /// To save Program Package Mapping
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="IsCreatedByAdmin"></param>
        /// <returns></returns>
        Boolean SaveHierarchyNodePackageMapping(BkgPackageHierarchyMapping newMapping, List<Int32> lstPaymentOptionIds, Int32 currentUserId);

        /// <summary>
        /// To delete bkg Package HierarchyMapping by ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean DeleteHirarchyPackageMappingByID(Int32 bkgPackageHierarchyMappingID, Int32 currentUserId);
        Boolean UpdateBackgroundPackageSequence(IList<BkgPackageHierarchyMapping> hierarchyPackagesToMove, Int32? destinationIndex, Int32 currentUserId);
        #endregion

        #region Communication

        List<HierarchyContactMapping> GetInstitutionContactUserData(Int32 institutionHierarchyNodeID);

        List<InsContact> GetInstitutionContactList(Int32 institutionHierarchyNodeID, Int32 contactID = AppConsts.MINUS_ONE);

        InstitutionContact GetInstitutionContactList(Int32 instutionContactID);


        List<Int32> DeleteInstitutionContact(Int32 instutionContactID, Int32 currentLoggedInUserId, Int32 nodeId);

        /// <summary>
        /// To get HierarchyContactMapping by MappingIds
        /// </summary>
        /// <param name="hierarchyContactMappingIDs"></param>
        /// <returns></returns>
        List<HierarchyContactMapping> GetHierarchyContactMappingByMappingIds(List<Int32> hierarchyContactMappingIDs);

        #endregion

        #region Backgroung Package Hierarchy Mapping

        /// <summary>
        /// To get background packages by HierarchyMappingIds
        /// </summary>
        /// <param name="bkgHierarchyMappingIds"></param>
        /// <returns></returns>
        List<BackgroundPackagesContract> GetOrderBkgPackageDetails(List<Int32> bkgHierarchyMappingIds, Int32? SelectedHierachyId);

        #endregion

        #region Background Attributes

        List<BkgSvcAttribute> GetServiceAttributes(Int32 tenantId, Int32 defaultTenantId);
        Boolean AddServiceAttributeToClient(BkgSvcAttribute serviceAttribute);
        Boolean UpdateServiceAttribute(Entity.BkgSvcAttribute serviceAttribute, List<BkgSvcAttributeOption> lstAddAttributeOptions);
        BkgSvcAttribute GetServiceAttributeBasedOnAttributeID(Int32 serviceAttributeID);
        Boolean DeleteServiceAttribute(Int32 serviceAttributeID, Int32 modifiedByID);
        Boolean checkIfSvcMappingIsDefinedForAttribute(Int32 attributeId, Int32 tenantId);
        Int32 SaveContact(InstitutionContact institutionContact, Int32 currentLoggedInUserId, Int32 heirarchyNodeID, Boolean isNew, Int32 contactID = 0);
        Boolean UpdateContact(InstitutionContact institutionContact, Int32 contactID, Int32 currentLoggedInUserId, Int32 heirarchyNodeID, Boolean isContact = false);
        Boolean DeleteServiceAttributeTenant(Int32 serviceAttributeID, Int32 modifiedByID);
        void AddBkgSvcAttributeTenant(Entity.BkgSvcAttribute serviceAttribute, Int32 tenantId);
        List<Int32> GetSvcAttributeIDsFromBkgSvcAttributeTenant(Int32 defaultTenantId);
        Boolean AddServiceAttribute(Entity.BkgSvcAttribute serviceAttribute, Int32 tenantId, Int32 defaultTenantId);
        Boolean AddMasterAttributeOptions(List<Entity.BkgSvcAttributeOption> masterAttributeOptions, Entity.BkgSvcAttribute svcAttribute);
        Boolean UpdateMasterServiceAttribute(Entity.BkgSvcAttribute serviceAttribute);
        Entity.BkgSvcAttribute GetMasterServiceAttributeBasedOnAttributeID(Int32 serviceAttributeID);
        List<CascadingAttributeOptionsContract> GetCascadingAttributeOptions(Int32 attributeId);
        CascadingAttributeOption SaveClientCascadingAttributeOption(CascadingAttributeOptionsContract cascadingAttributeOptionsContract, Int32 currentLoggedInUserId);
        Entity.CascadingAttributeOption SaveMasterCascadingAttributeOption(CascadingAttributeOptionsContract cascadingAttributeOptionsContract, Int32 currentLoggedInUserId);
        Boolean DeleteCascadingAttributeOption(Int32 optionId, Int32 currentLoggedInUserId);

        #endregion

        //#region Manage Service Item Setup
        //List<PackageServiceItem> GetPackageServiceItemList(Int32 BkgPackageSvcId);
        //PackageServiceItem GetPackageServiceItemData(Int32 PSI_ID);
        //Boolean SavePackageServiceItemData(PackageServiceItem packageServiceItemData);
        //Boolean IsServiceItemExist(String serviceItemName, Int32? PSI_ID = null);
        //Boolean IsServiceItemMapped(Int32 PSI_ID);
        //#endregion

        #region Manage Master Service Attribute Groups

        List<Entity.BkgSvcAttributeGroup> GetServiceAttributeGroups();
        Boolean CheckIfSvcAttrGrpNameAlreadyExist(String svcAttrGrpName, Int32 svcAttrID);
        Boolean SaveServiceAttributeGroup(Entity.BkgSvcAttributeGroup svcAttrGrp);
        Boolean UpdateServiceAttributeGroup(Entity.BkgSvcAttributeGroup svcAttrGrp, Int32 svcAttrGrpId);
        Boolean DeleteServiceAttributeGroup(Int32 svcAttrGrpId, Int32 modifiedById);
        Entity.BkgSvcAttributeGroup GetServiceAttributeGroupBasedOnAttributeGrpID(Int32 serviceAttributeGrpID);

        /// <summary>
        /// Get Tenant specific Attribute Groups
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        List<Entity.ClientEntity.BkgSvcAttributeGroup> GetServiceAttributeGroupsByTenant();

        #endregion

        #region Service Attribute  Mapping

        List<MapServiceAttributeToGroupContract> GetMappedAttributes(Int32 serviceAttrGrpId);
        List<Entity.BkgSvcAttribute> GetUnmappedAttributes(Int32 attributegrpID, Int32 defaultTenantId);
        List<Entity.BkgSvcAttribute> GetSourceAttributes(Int32 childAttributeId, Int32 attributeGroupId, Int32 defaultTenantId);
        void SaveAttributeGroupMapping(List<Entity.BkgAttributeGroupMapping> lstBkgSvcAttributeGroupMapping, Int32 svcAttributeGrpId);
        Boolean DeleteAttributeGroupMapping(Int32 attributeGrpMappingID, Int32 currentLoggedInUserId);
        Boolean UpdateAttributeSequence(IList<MapServiceAttributeToGroupContract> attributesToMove, Int32? destinationIndex, Int32 currentLoggedInUserId);
        Boolean UpdateAttributeGrpMapping(Int32 attributeGrpMappingID, Int32 currentLoggedInUserId, Boolean IsRequired, Boolean IsDisplay, Int32? sourceAttributeId,Boolean IsHiddenFromUI);
        #endregion

        #region Manage Custom Forms

        List<Entity.CustomForm> GetAllCustomForms();

        Entity.CustomForm SaveCustomFormDetail(Entity.CustomForm customForm, Int32 currentLoggedInUserId);

        Boolean CheckIfCustomFormNameAlreadyExist(String customFormName);

        Boolean UpdateCustomFormDetail(Entity.CustomForm customForm, Int32 customFormID, Int32 currentLoggedInUserId);

        Entity.CustomForm GetCurrentCustomFormInfo(Int32 customFormID);

        Boolean DeleteCustomForm(Int32 customFormID, Int32 currentUserId);

        List<Entity.CustomFormAttributeGroup> GetCustomFormAttrGrpsByCustomFormId(Int32 customFormId);

        List<Entity.BkgSvcAttributeGroup> GetAllBkgSvcAttributeGroup();

        Guid GetCodeForCurrentAttributeGroup(Int32? attributeGrpID);

        Entity.CustomFormAttributeGroup SaveCustomFormAttributeGroupDetail(Entity.CustomFormAttributeGroup customFormAttrGrp, Int32 currentLoggedInUserId);

        Entity.CustomFormAttributeGroup GetCurrentCustomFormAttributeGroup(Int32 customFormAttributeGroupID);

        Boolean UpdateCustomFormAttributeGroupDetail(Entity.CustomFormAttributeGroup customFormAttributeGroup, Int32 customFormAttributeGroupID, Int32 currentLoggedInUserId);

        Boolean DeleteCustomFormAttributeGroup(Int32 customFormAttributeGroupID, Int32 currentUserId);

        Boolean CheckIfCustomFormAttrGrpMappingAlreadyExist(Int32 customFormId, Int32 attrGrpId);

        Boolean UpdateCustomFormSequence(IList<Entity.CustomForm> customFormsToMove, Int32 destinationIndex, Int32 currentLoggedInUserId);

        Boolean UpdateCustomFormAttributeGroupSequence(IList<Entity.CustomFormAttributeGroup> customFormAttributeGroupsToMove, Int32 destinationIndex, Int32 currentLoggedInUserId);

        #endregion

        #region Map Master Services To Client
        #region Background Services
        List<MapServicesToClientContract> GetBackgroundServices(Int32? SvcID = null, String SvcName = null, String ExtCode = null);
        #endregion

        #region Map Services To Client
        Boolean MapServicesToClient(String SelectedServicesList, Int32 SelectedTenantId);
        #endregion

        #region Existing Background Services
        Int32[] GetExistingBackgroundServices();
        #endregion

        #region Deactivate Mapping
        Boolean DeactivateMapping(Int32 SelectedServicesId, Int32 selectedTenantID);
        #endregion

        #region Update Client Count in Master DB
        Boolean UpdateClientCount(String SelectedServices, Boolean isInMappingMode);
        #endregion
        #endregion

        #region Derived From Services

        List<Entity.BackgroundService> GetDerivedFromServiceList(Int32? currentServiceId);
        Boolean IsChildServiceExist(Int32 currentServiceId);
        #endregion

        #region Manage Master Services
        List<Entity.BackgroundService> GetMasterServices();
        String BkgSrvName(Int32 bkgSvcMasterID);
        Boolean CheckIfServiceNameAlreadyExist(String svcName, Int32 svcID);
        Entity.BackgroundService SaveNewServiceDetail(Entity.BackgroundService masterService, Int32 currentLoggedInUserId);
        void UpdateServiceDetail(Entity.BackgroundService masterService, Int32 svcMasterID, Int32 currentLoggedInUserId);
        void DeletebackgroundService(Int32 bkgSvcMasterID, Int32 currentLoggedInUserId);

        #endregion

        #region Service Attribute Group Mapping
        List<ManageServiceAttributeGrpContract> GetAttributeGrps(Int32 serviceId);
        List<Entity.BkgSvcAttributeGroup> GetAllAttributeGroups(Int32 serviceID, Boolean isupdate);
        List<Entity.BkgSvcAttribute> GetAllAttributes(Int32 attributegrpID);
        List<Int32> GetAllAttributesMappingIDs(List<Int32> attributesIDs, Int32 attributegrpID);
        void SaveAttributeGrpMappings(Entity.BkgSvcAttributeGroupMapping newSvcAttributeGrpMapping);
        List<Int32> GetAllAttributeIDsRelatedToAttributeGrpID(Int32 attributegrpID, Int32 serviceId);
        void UpdateAtttributeMappingLst(Int32 attributegrpID, Int32 serviceId, Int32 currentLoggedInUserId, List<Int32> updatedattributeIdLst);
        void DeleteAttributeServiceMappingByAttributeId(Int32 attributegrpID, Int32 attributeId, Int32 serviceId, Int32 currentLoggedInUserId);
        void DeleteAttributMappingwithServicebyAttributeGroupid(Int32 attributegrpID, Int32 serviceId, Int32 currentLoggedInUserId);
        #endregion

        #region Service CustomFormMapping

        List<ManageServiceCustomFormContract> GetCustomFormsForService(Int32 serviceId);
        List<Entity.CustomForm> GetAllCustomForm(Int32 serviceId);
        void SaveSvcFormMapping(Entity.BkgSvcFormMapping newSvcFormMapping);
        void DeleteSvcFormMApping(Int32 svcFormMappingID, Int32 currentLoggedInUserId);
        #endregion

        #region Manage background Setup Attribute
        Entity.BkgSvcAttribute SaveAttributeInMaster(Entity.ClientEntity.BkgSvcAttribute bkgSvcAttribute, Int32 attributeGroupId, Int32 currentLoggedInUserId, Boolean isRequired, Boolean isDisplay, Int32 tenantId,Boolean IsHiddenFromUI);
        Entity.BkgAttributeGroupMapping SaveAttributeAndGroupMappingInMaster(Entity.ClientEntity.BkgAttributeGroupMapping attributeMappingToAdd, Boolean isRequired, Boolean isDisplay);
        System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> AddDeleteServiceAttributeOpt(List<Int32> attributeOptionIdsToDelete, List<Entity.ClientEntity.BkgSvcAttributeOption> attributeOptToAdd, Int32 currentLoggedInUserId, Int32 bkgSvcAttributeId);
        #endregion

        #region Client Service Vendor
        String FetchExternalBkgServiceCodeByID(Int32 ExtSvcID);

        /// <summary>
        ///  Get data to map the grid on the basis of TenantId
        /// </summary>
        /// <param name="SelectedTenantId"></param>
        /// <returns></returns>
        List<ClientServiceVendorContract> GetMappedBkgSvcExtSvcToState(Int32 SelectedTenantId);
        /// <summary>
        /// Get all Background Services(Excluding all mapped).
        /// </summary>
        /// <param name="_isupdate"></param>
        /// <param name="selectedTenantID">TenantID</param>
        /// <returns></returns>
        List<Entity.ClientEntity.BackgroundService> GetBkgService();
        /// <summary>
        /// Get List Of External Service Mapped with Bkgroung Service
        /// </summary>
        /// <param name="SelectedBkgSvcID"></param>
        /// <returns></returns>
        List<Entity.ExternalBkgSvc> GetExtBkgSvcCorrespondsToBkgSvc(Int32 SelectedBkgSvcID, Int32 selectedTenantID, Boolean _isupdate);
        /// <summary>
        /// Get List Of External Service Mapped with Bkgroung Service
        /// </summary>
        /// <returns></returns>
        List<Entity.State> GetAllStates();
        /// <summary>
        /// Get the List of State that mapped with ExtService
        /// </summary>
        /// <param name="ExtSvcId">ExtSvcId</param>
        /// <param name="selectedTenantId"></param>
        /// <returns></returns>
        List<Int32> GetMAppedStatesIdtoExtSvc(Int32 ExtSvcId, Int32 selectedTenantId);
        /// <summary>
        /// get the Mapping Id of External and Background Service mapping.
        /// </summary>
        /// <param name="bkgSvcId">bkgSvcId</param>
        /// <param name="extSvcId">extSvcId</param>
        /// <returns></returns>
        Int32 GetBkgSvcExtSvcMappedId(Int32 bkgSvcId, Int32 extSvcId);
        /// <summary>
        /// Save the Mapped state with Service
        /// </summary>
        /// <param name="clientExtSvcVendorMapping"></param>
        void SaveClientSvcvendormapping(Entity.ClientExtSvcVendorMapping clientExtSvcVendorMapping);
        /// <summary>
        ///Update the mapping list of State with Backgroung/External Service  
        /// </summary>
        /// <param name="updatedMappedStateIds">updatedMappedStateIds</param>
        /// <param name="selectedServiceID">selectedServiceID</param>
        /// <param name="selectedExternalServiceId">selectedExternalServiceId</param>
        /// <param name="selectedTenantID">selectedTenantID</param>
        /// <param name="currentLoggedInUserID">currentLoggedInUserID</param>
        void UpdateClientSvcVendorMapping(List<Int32> updatedMappedStateIds, Int32 selectedServiceID, Int32 selectedExternalServiceId, Int32 selectedTenantID, Int32 currentLoggedInUserID);
        /// <summary>
        /// delete the Mapping in ClientServiceVendorMapping related to the Backgroung Service and tenantId.
        /// </summary>
        /// <param name="bkgSvcID">Background Master service ID</param>
        /// <param name="selectedTenantId">selectedTenantId</param>
        /// <param name="currentLoggedInUserID">currentLoggedInUserID</param>

        void DeleteClientSvcVendorMapping(Int32 bkgSvcID, Int32 ExtServiceID, Int32 selectedTenantId, Int32 currentLoggedInUserID);
        #endregion

        #region Manage Order Color Status

        List<lkpOrderFlag> GetAllOrderFlags();
        List<InstitutionOrderFlag> GetInstituteOrderFlags(Int32 tenantId);
        InstitutionOrderFlag SaveInstitutionOrderFlagDetail(InstitutionOrderFlag institutionOrderFlag, Int32 currentLoggedInUserId);
        InstitutionOrderFlag GetCurrentInstitutionOrderFlag(Int32 institutionOrderFlagID);
        Boolean UpdateInstitutionOrderFlagDetail(InstitutionOrderFlag institutionOrderFlag, Int32 currentLoggedInUserId);
        Boolean DeleteInstitutionOrderFlag(Int32 institutionOrderFlagID, Int32 currentUserId);

        #endregion

        #region Vendor Sevice Mapping

        /// <summary>
        /// Get Vendor Service Mapping
        /// </summary>
        /// <returns>List of BkgSvcExtSvcMapping</returns>
        List<Entity.BkgSvcExtSvcMapping> GetVendorServiceMapping();

        /// <summary>
        /// Delete vendor service mapping
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="modifiedByID"></param>
        /// <returns>True/False</returns>
        Boolean DeleteVendorServiceMapping(Int32 vendorServiceMappingID, Int32 modifiedByID);

        /// <summary>
        /// Get Vendors 
        /// </summary>
        /// <returns>List of ExternalVendor</returns>
        List<Entity.ExternalVendor> GetVendors();

        List<MappedResidentialHistoryAttributeGroupsWithPkg> GetMappedResidentialHistoryAttributeGroupsWithPkg(Int32 pkgId);

        /// <summary>
        /// Get External Service By VendorID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>List of ExternalBkgSvc</returns>
        List<Entity.ExternalBkgSvc> GetExternalBkgSvcByVendorID(Int32 vendorID);

        /// <summary>
        /// Save Vendor service Mapping
        /// </summary>
        /// <param name="bkgSvcExtSvcMapping"></param>
        /// <returns>True/False</returns>
        Boolean SaveVendorServiceMapping(Entity.BkgSvcExtSvcMapping bkgSvcExtSvcMapping);

        /// <summary>
        /// Update Vendor Service Mapping
        /// </summary>
        /// <param name="bkgSvcExtSvcMapping"></param>
        /// <returns>True/False</returns>
        Boolean UpdateVendorServiceMapping(Entity.BkgSvcExtSvcMapping bkgSvcExtSvcMapping);

        /// <summary>
        /// Check If Vendor Service mapping already exists
        /// </summary>
        /// <param name="bkgSvcId"></param>
        /// <param name="extSvcId"></param>
        /// <param name="bsesmID"></param>
        /// <returns>True/False</returns>
        Boolean IfVendorServiceMappingExists(Int32 bkgSvcId, Int32 extSvcId, Int32? bsesmID);

        /// <summary>
        /// Get Vendor Service Attribute Mapping By Vendor Service Mapping ID
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <returns>DataTable</returns>
        DataTable GetVendorServiceAttributeMappingList(Int32 vendorServiceMappingID);

        /// <summary>
        /// Delete Vendor Service Attribute Mapping
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="vendorServiceFieldID"></param>
        /// <param name="modifiedByID"></param>
        /// <returns>True/False</returns>
        Boolean DeleteVendorServiceAttributeMapping(Int32 vendorServiceMappingID, Int32 vendorServiceFieldID, Int32 currentUserId);

        /// <summary>
        /// Get Background And External Service Atributes by Vendor Service MappingID and Vendor Service Field ID
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="vndSvcFieldId"></param>
        /// <returns>DataSet</returns>
        DataSet GetBkgSvcExtSvcAttributes(Int32 vendorServiceMappingID, Int32? vndSvcFieldId);

        /// <summary>
        /// Save Vendor Service Attribute Mapping
        /// </summary>
        /// <param name="extSvcAttMapping"></param>
        /// <returns>True/False</returns>
        Boolean SaveVendorServiceAttributeMapping(List<Entity.ExternalSvcAtributeMapping> extSvcAttMapping);

        #endregion

        #region Manage Rule Templates

        List<BkgRuleTemplate> GetRuleTemplates();
        void DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId);
        BkgRuleTemplate GetRuleTemplateByID(Int32 ruleTemplateId);
        void AddRuleTemplate(Entity.ComplianceRuleTemplate ruleTemplate);
        void UpdateRuleTemplate(Entity.ComplianceRuleTemplate ruleTemplate, List<Int32> expressionIds);
        String ValidateRuleTemplate(String ruleTemplateXML);

        #endregion

        #region Manage Service Settings

        Entity.ApplicableServiceSetting GetServiceSetting(Int32 backgroundServiceId);
        BkgPackageSvc GetCurrentBkgPkgServiceDetail(Int32 backGroundPkgSrvcId);
        //UAT-3109
        String GetCurrentBkgPkgServiceAMERDetail(Int32 backGroundPkgSrvcId);
        #endregion

        #region Order Client Status
        Boolean SaveOrderClientStatus(Int32 SelectedTenantId, String OrderClientStatusTypeName, Int32 currentLoggedInUserId);

        List<BkgOrderClientStatu> FetchOrderClientStatus();

        Boolean UpdateClientStatusSequence(IList<BkgOrderClientStatu> statusToMove, Int32? destinationIndex, Int32 currentLoggedInUserId);

        Boolean DeleteOrderClientStatus(Int32 Id, Int32 CurrentLoggedInUserId);

        Boolean UpdateOrderClientStatus(Int32 OrderClientStatusId, String OrderClientStatusTypeName, Int32 CurrentLoggedInUserId);
        #endregion

        #region Manage Service Item Entity

        List<GetServiceItemEntityList> getServiceItemEntityList(Int32 serviceItemId);
        List<GetAttributeListForServiceItemEntity> getAttribteListForServiceItemEntity(Int32 serviceItemId);
        Boolean SavePackageServiceItemEntity(List<PackageServiceItemEntity> newServiceItemEntityList, Int32 currentloggedInUserId);
        Boolean DeletePackageServiceItemEntityRecord(Int32 packageServiceItemEntityId, Int32 currentloggedInUserId);
        #endregion

        #region Service Vendors

        IList<Entity.ExternalVendor> FetchServiceVendors();

        Boolean SaveServiceVendors(VendorsDetailsContract vendorDetails, Int32 CurrentLoggedInUserId);

        Boolean UpdateServiceVendors(VendorsDetailsContract vendorsDetails, Int32 serviceVendorsID, Int32 CurrentLoggedInUserId);

        Boolean DeleteServiceVendors(Int32 serviceVendorsID, Int32 CurrentLoggedInUserId);

        #endregion

        #region BackgroundPackageDetails
        BkgPackageHierarchyMapping GetBackgroundPackageDetail(Int32 BkgPackageNodeMappingId);

        Boolean UpdatePackageHirarchyDetails(BkgPackageHierarchyMapping bkgPackageHierarchyMapping, Int32 bkgPackageHierarchyMappingId, Int32 currentLoggedInID, List<Int32> lstSelectedOptionIds, List<Int32> targetPackageIds, Int32 months, Boolean isActive);

        #endregion

        #region Institution Hierarchy Vendor Account Mapping

        List<ExternalVendorAccountMappingDetails> GetInstHierarchyVendorAcctMappingDetails(Int32 DPMId);
        List<Entity.ExternalVendorAccount> GetExternalVendorAccountsNotMapped(Int32 DPMId);
        Boolean SaveInstHierarchyVendorAcctMapping(InstHierarchyVendorAcctMapping objInstHierarchyVendorAcctMapping);
        Boolean UpdateTenantChanges();
        InstHierarchyVendorAcctMapping GetInstHierarchyVendorAcctMappingByID(Int32 instHierarchyVendorAcctMappingID);
        #endregion

        #region Map Regulatory Entity

        List<Entity.lkpRegulatoryEntityType> FetchRegulatoryEntityTypeNotMapped(Int32 nDPMID);
        List<InstHierarchyRegulatoryEntityMappingDetails> GetInstHierarchyRegulatoryEntityMappingDetails(Int32 DPMId);
        Boolean SaveInstHierarchyRegulatoryEntityMapping(InstHierarchyRegulatoryEntityMapping instHierarchyRegEntity);
        InstHierarchyRegulatoryEntityMapping GetInstHierarchyRegEntityMappingByID(Int32 mappingID);
        #endregion

        #region Vendor Account Details

        IList<Entity.ExternalVendorAccount> FetchVendorsAccountDetail(Int32 VendorId);

        String SaveVendorsAccountDetail(Int32 VendorId, String AccountNumber, String AccountName, Int32 CurrentLoggedInUserId);

        String UpdateVendorsAccountDetail(String AccountNumber, String AccountName, Int32 EvaId, Int32 CurrentLoggedInUserId);

        Boolean DeleteVendorsAccountDetail(Int32 EvaId, Int32 CurrentLoggedInUserId);

        #endregion

        #region Import ClearStar Services
        IList<Entity.ExternalBkgSvc> FetchExternalBkgServices(Int32 VendorID);

        IList<Entity.ExternalBkgSvcAttribute> FetchExternalBkgServiceAttributes(Int32 EBS_ID);

        IList<Entity.ClearStarService> FetchClearstarServices();

        Boolean ImportClearStarServices(Int32[] SelectedCssIds, Int32 VendorID, Int32 CurrentLoggedInUserID);

        IEnumerable<Entity.ClearStarService> FetchAllClearstarServices();

        Boolean SaveClearStarSevices(List<Entity.ClearStarService> lstClearStarService);

        #endregion



        #region D And R AttributeGroup Mapping

        List<DAndRAttributeGroupMappingContract> GetDAndRAttributeGroupMapping(Int32 DocumentID);
        IQueryable<Entity.BkgSvcAttributeGroup> GetServiceAttributeGroup();
        IQueryable<Entity.BkgSvcAttribute> GetServiceAttributeByServiceGroupID(Int32 BkgSvcAGID);
        //Boolean UpdateMapping(Int32 systemDocumentID, Int32 bkgSvcAttributeGroupID, Int32 bkgSvcAttributeID, Int32 currentLoggedInUserID, Int32 specialFieldType_ID, Boolean rbApplicantAttr);
        Boolean UpdateMapping(DAndRAttributeGroupMappingContract dAndRContract, Int32 currentLoggedInUserID);

        #endregion

        #region Manage D & R Documents

        Boolean SaveDisclosureTemplateDocument(List<Entity.SystemDocument> lstDisclosureDocuments);
        List<Entity.SystemDocument> GetDisclosureTemplateDocuments(Int32 docTypeID);
        Boolean DeleteDisclosureTemplateDocument(Int32 systemDocumentID, Int32 currentUserId);
        Boolean UpdateDisclosureTemplateDocument(Entity.SystemDocument disclosureDocument, Int32 selectedBkgSvcId);

        #endregion

        #region Manage Service Item Custom Forms Mappings

        List<Entity.CustomForm> GetSupplCustomFrmsNotMappedToSvcItem(Int32 pkgSvcItemID);
        Boolean SaveBkgSvcItemFormMapping(BkgSvcItemFormMapping objBkgSvcItemFormMapping);
        List<PkgServiceItemCustomFormMappingDetails> GetPkgServiceItemCustomFormMappingDetails(Int32 pkgServiceItemId);
        BkgSvcItemFormMapping GetBkgSvcItemFormMappingById(Int32 bkgSvcItemFormMappingId);


        #endregion

        #region Disclosure and Release

        List<Entity.SystemDocument> GetDAndRDocuments(List<String> Countries, List<String> States, Int32? HierarchyNodeID, String RegulatoryNodeIDs, String BkgServiceIds,
                                                      Int32 TenantId, String disclosureDocAgeGroupType);

        List<Int32> GetServicesIds(List<Int32> BkgPackages);

        List<SysDocumentFieldMappingContract> GetFieldNames(Dictionary<Int32, String> dictionary, List<Entity.SystemDocument> DocumentList, List<TypeCustomAttributes> lstCustomAttributes, Int32 tenantID);
        #endregion

        #region Contact

        Boolean IsContactExists(String contactEmailAddress, Int32 contactID = AppConsts.NONE);

        #endregion


        #region Special Field Type for D & R
        List<Entity.lkpDisclosureDocumentSpecialFieldType> GetSpecialFieldType();
        #endregion

        #region Copy Background Package
        Boolean CheckIfPackageNameAlreadyExist(String packageName);
        String CopyBackgroundPackage(Int32 sourceNodeId, Int32 targetNodeId, Int32 sourceBPHMId, String targetPackageName, Int32 currentLoggedInUserId);
        #endregion

        Boolean CheckIfVendorNameAlreadyExist(String vendorName, Int32 vendorID);


        /// <summary>
        /// Check if the PackageServiceItem is Quanity group of any another ServiceItem
        /// </summary>
        /// <param name="pkgSvcItemId"></param>
        /// <returns></returns>
        Boolean IsPackageServiceItemEditable(Int32 pkgSvcItemId);

        #region Manage Payment Option Instruction

        List<Entity.lkpPaymentOption> GetSecurityPaymentOptions();

        Entity.lkpPaymentOption GetSecurityPaymentOptionById(Int32 paymentOptionId);

        Boolean UpdateSecurityChanges();

        #endregion

        #region Manual Service Forms
        List<BackgroundService> GetTenantServices();
        Boolean UpdateOrderServiceServiceFormStatus(Int32 orderServiceFormId, Int32 statusId, Int32 currentLoggedInUserId);
        #endregion

        Boolean CheckIfOrderClientStatusIsUsed(Int32 orderClientStatusId);

        #region BkgCompl Package Data Mapping
        List<BackgroundPackage> GetPermittedBackgroundPackagesByUserID();

        List<LookupContract> GetServiceGroupsForPackage(Int32 selectedPackageId);

        List<LookupContract> GetServicesForSvcGroup(int selectedServiceGrp, int selectedBkgPkgID);

        List<LookupContract> GetServiceItemsForSvc(int selectedService);

        List<LookupContract> GetComplianceCatagories(int selectedComplPkgID);

        List<LookupContract> GetCatagoryItems(int selectedCatagoryID);

        List<ComplianceItemAttribute> GetComplianceItemAttributes(int selectedItemID);

        DataTable FetchBkgCompliancePackageMapping(Int32 bkgPackageId, Int32 compPackageId, CustomPagingArgsContract gridCustomPaging);

        Int32 GetComplianceAttributeDataTypeID(string ComplianceAttributeDataTypeCode);

        Boolean SaveBkgComplPkgDataMapping(BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract, Int32 currentLoggedInUserId);

        String DeleteBkgComplPkgDataMapping(int BCPM_ID, int currentLoggedInUserId);

        String UpdateBkgComplPkgDataMapping(int BCPM_ID, int currentLoggedInUserId, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract);
        #endregion

        List<BkgCompliancePkgMappingAttrOption> FetchBkgCompliancePkgMappingAttrOptions(int BCPM_ID);

        #region UAT-583 WB: AMS: Ability to delete attributes and attribute groups from the package setup screen (even after the attribute or attribute group is active)
        Boolean DeletedBkgSvcAttributeGroupMapping(Int32 bkgAttributeGroupId, Int32 bkgPackageSvcId, Int32 currentloggedInUserId);
        #endregion


        Boolean UpdateAttributeDisplaySequence(IList<AttributeSetupContract> statusToMove, Int32? destinationIndex, Int32 currentLoggedInUserId);

        DataTable GetBkgAttributesBasedOnGroup(int bkgSvcGroupId);

        #region UAT-800 Build all missing services into Complio based on spreadsheet of services for College System

        /// <summary>
        /// To Get Service Form Mapping for All and Specific Institute.
        /// </summary>
        /// <param name="serviceFormID"></param>
        /// <param name="serviceID"></param>
        /// <param name="mappingTypeID"></param>
        /// <param name="dpmID"></param>
        /// <returns>DataTable</returns>
        DataTable GetServiceFormMappingAllandSpecificInstitution(Int32? serviceFormID, Int32? serviceID, Int32? mappingTypeID, Int32? dpmID, Int32? selectedTenantID);

        /// <summary>
        /// Get Background Service with Mapping
        /// </summary>
        /// <returns>List of BackgroundServiceMapping</returns>
        List<BackgroundServiceMapping> GetBackgroundServiceMapping();

        /// <summary>
        /// Get Service Attached Forms
        /// </summary>
        /// <returns>List of ServiceForm</returns>
        List<ServiceForm> GetServiceForm();

        /// <summary>
        /// Get Mapping Types
        /// </summary>
        /// <returns>List of SvcFormMappingType</returns>
        List<SvcFormMappingType> GetMappingType();

        /// <summary>
        /// Delete Service Form Institution Mapping
        /// </summary>
        /// <param name="serviceFormMappingID"></param>
        /// <param name="serviceFormHierarchyMappingID"></param>
        /// <param name="mappingTypeID"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True/False</returns>
        Boolean DeleteServiceFormInstitutionMapping(Int32 serviceFormMappingID, Int32? serviceFormHierarchyMappingID, Int32 mappingTypeID, Int32 currentUserId);

        /// <summary>
        /// Save Service Form Institution Mapping
        /// </summary>
        /// <param name="svcFormInstitutionMappingContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True/False</returns>
        String SaveServiceFormInstitutionMapping(ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId);

        /// <summary>
        /// Update Service Form Institution Mapping
        /// </summary>
        /// <param name="svcFormInstitutionMappingContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True/False</returns>
        String UpdateServiceFormInstitutionMapping(ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId);

        /// <summary>
        /// Get Service Ids by Service Form ID 
        /// </summary>
        /// <param name="serviceFormID"></param>
        /// <returns>List of ServiceIDs</returns>
        List<Int32> GetServiceIdsByServiceForm(Int32 serviceFormID);

        #endregion

        #region Service Attached Form

        List<ServiceAttachedFormContract> GetServoceAttachedFormList();
        List<Entity.ServiceAttachedForm> GetParentServiceattachedForm();
        Boolean SaveServiceAttachedForm(Entity.ServiceAttachedForm serviceAttachedForm);
        Entity.ServiceAttachedForm GetServiceAttachedFormByID(Int32 SF_ID);
        Boolean UpdateServiceAttachedForm();
        Boolean CheckIfServiceAttachedFormNameAlreadyExist(String svcFormName, Int32 serviceFormID, Boolean isUpdate);
        IEnumerable<Entity.BkgServiceAttachedFormMapping> GetBkgServiceAttachedFormMappingByServiceFormID(Int32 serviceAttachedFormID);

        Boolean IsServiceAttachedFormVersionsDeleted(Int32 SF_ID);
        #endregion

        #region UAT-803 - BACKGROUND STATE SEARCH CRITERIA
        Boolean SaveBkgPkgStateSearchCriteria(List<BkgPackageStateSearchContract> bkgPkgStateSearchContract, Int32 currentLoggedInUserID);

        List<Entity.ClientEntity.BkgPkgStateSearch> GetBkgPkgStateSearchCriteria(Int32 bkgPackageID);

        Boolean SaveMasterStateSearchCriteria(List<BkgPackageStateSearchContract> bkgPkgStateSearchContract, int currentLoggedInUserID);

        List<Entity.BkgMasterStateSearch> GetMasterStateSearchCriteria();
        #endregion

        List<BkgPkgStateSearch> UpdateStateSearchSettingsFromMaster(Int32 currentLoggedInUserID, Int32 bkgPackageID);

        Boolean IsStateSearchRuleExists(Int32 pkgServiceItemID);

        #region Service Forms

        /// <summary>
        ///Get the Service forms associated with a Background Service, along with their
        ///Dispatch type either Manual or Electronic, at the Root(Form) level
        ///Service level and Package Service Mapping level
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="bkgSvcId"></param>
        /// <param name="bpsId"></param>
        /// <returns></returns->
        DataTable GetServiceFormDispatchType(Int32 packageId, Int32 bkgSvcId, Int32 bpsId);

        /// <summary>
        /// Update the Overriding data when the PacageService mapping is updated
        /// </summary>
        /// <param name="_lstBkgPackageSvcFormOverride"></param>
        /// <param name="bpsId"></param>
        void UpdateBkgPackageSvcFormOverride(List<BkgPackageSvcFormOverride> _lstBkgPackageSvcFormOverride, Int32 bpsId);

        #endregion

        #region Master Review Criteria
        List<BkgReviewCriteria> FetchMasterReviewCriteria();

        Boolean SaveReviewCriteria(BkgReviewCriteria reviewCriteria);

        Boolean UpdateReviewCriteria(BkgReviewCriteria reviewCriteria, Boolean isDeleteMode);
        #endregion

        #region Mapped Review Criteria [UAT-844: Order Review enhancements]
        List<BkgReviewCriteriaHierarchyMapping> GetMappedReviewCriteriaList(Int32 instHierarchyNodeId);

        Boolean SaveReviewCriteriaMapping(List<BkgReviewCriteriaHierarchyMapping> reviewCriteriaListToMap);
        Boolean DeleteReviewCriteriaMapping(Int32 currentloggedInUserId, Int32 BRCHM_ID);

        #endregion

        #region UAT-844 - ORDER REVIEW ENHANCEMENT
        //UPDATE PACKAGE SERVICE GROUP
        Boolean UpdatePackageServiceGroup(BkgPackageSvcGroup bkgPackageSvcGroup, Int32 bkgPackageID, Int32 bkgSvcGroupID);

        //GET PACKAGE SERVICE GROUP DETAILS BY BKG PACKAGEID AND SERVICEGROUPID
        BkgPackageSvcGroup GetPkgServiceGroupDetail(Int32 serviceGroupID, Int32 packageID);
        #endregion

        #region UAT-1451:Data synch mapping screen is almost unusable as of now (UI updates)
        Boolean IsBkgCompDataPointMappingExist(Int32? BCPM_ID, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract);
        #endregion

        #region UAT 1560 WB: We should be able to add documents that need to be signed to the order process
        List<Entity.SystemDocument> GetBothUploadedDisclosureDocuments(List<Int32> lstSatusIDs);
        List<Entity.SystemDocument> GetAdditionalDocuments(List<Int32> backgroundPackageIds, List<Int32> compliancePackageIds, Int32? HierarchyNodeID);
        #endregion

        #region Resolved a existing issue related to attribute options with the implementation of UAT-1738
        List<Entity.ClientEntity.ComplianceAttributeOption> GetComplianceAttributeOption(Int32 attributeId);

        #endregion

        #region UAT-1834: NYU Migration 2 of 3: Applicant Complete Order Process

        BackgroundPackagesContract GetBackgroundPackageByPkgIDAndNodeID(Int32 bkgPackageID, Int32 orderNodeID, Int32 hierarchyNodeID);
        Boolean UpdateBulkOrder(Int32 orderID, Int32 bulkOrderUploadID, Int32 bulkOrderStatusID, Int32 currentUserID);
        Boolean UpdateLastOrderPlacedDate(Int32 bulkOrderUploadID, Int32 currentUserID);

        #endregion

        //UAT-2326: Change SSN on D&A and Additional Documents to be masked.
        Int32 GetBkgAttributeGroupMappingIDforSSN();
        List<BackgroundPackage> GetAutomaticInvitationBackgroundPackages(Int32 packageID);//UAT-2388
        Boolean GetAutomaticPackageInvitationSetting(Int32 packageID);//UAT-2388
        Boolean GetRotationQualifyingSetting(Int32 packageId);//UAT-3268
        List<BkgPackageType> GetAllBkgPackageTypes(String packageTypeName, String packageTypeCode, Int32 bkgPackageTypeId, String bkgPackageColorCode);//UAT-3525
        String DeletePackageType(Int32 bkgPackageTypeId, Int32 loggedInUserId);//UAT-3525
        Boolean SaveUpdatePackageType(BkgPackageTypeContract _packageTypeContract, Int32 loggedInUserId);//UAT-3525
        String GetPackageTypeCode(); //UAT-3525
        Boolean IsPackageMapped(Int32 bkgPackageTypeId, Int32 loggedInUserId);//UAT-3525
        Boolean IsPackageTypeCodeAlreadyExists(String packageTypeCode, Int32 bkgPackageTypeId);//UAT-3525
        Boolean IsPackageTypeNameAlreadyExists(String packageTypeCode, Int32 bkgPackageTypeId);//UAT-3525
        List<SystemDocBkgSvcMapping> GetAddtionalDocBkgSvcMapping(List<Int32> bkgPackagesIds, String additionalDocIds); //UAT-3745
        List<Entity.ExternalVendorAccount> GetExternalVendorAccount(Int32 selectedVendorId);
        int GetContentType(String contentTypeCode);
        int GetContentRecordType(String contentRecordTypeCode);
        Boolean SaveContentData(PageContent objPageContent);
        PageContent GetContentData(Int32 dpmId);
    }
}

