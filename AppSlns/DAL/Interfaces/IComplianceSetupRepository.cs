using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using INTSOF.UI.Contract.SystemSetUp;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Data.Entity.Core.EntityClient;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;


namespace DAL.Interfaces
{
    public interface IComplianceSetupRepository
    {
        #region Admin Setup Screens

        #region Manage Compliance Packages

        List<NodesContract> GetListofNodes(Int32 CategoryId, Int32? ItemId);
        List<CompliancePackage> GetCompliancePackages(Boolean getTenantName, List<Entity.Tenant> tenantList);
         List<TrackingPackageRequiredDocURLMapping> GetTrackingPackageRequiredDOCURLMapping();
         List<TrackingPackageRequiredDocURL> GetTrackingPackageRequiredDOCURL();
         List<CompliancePackage> GetCompliancePackagesByPermissionList( Int32 tenantId, String dpmIds, Int32 OrganisationId,Boolean IsAdminLoggedIn);
        CompliancePackage SaveCompliancePackageDetail(CompliancePackage package, Int32 currentUserId);
        Boolean DeleteCompliancePackage(Int32 packageID, Int32 currentUserID);
        Boolean CheckIfPackageNameAlreadyExist(String PackageName, Int32 compliancePackageID, Int32 tenantId);
        Boolean CheckIfPackageNameAlreadyExist(String packageName);
        void UpdateCompliancePackageDetail(CompliancePackage package, Int32 currentUserId);
        CompliancePackage GetCopiedCompliancePackage(Int32 parentPackageId, String packageName);




        #endregion

        #region 3871
        List<CompliancePackage> GetCompliancePackagesForTrackingRequired(Int32 tenantId);
        List<TrackingPackageRequiredContract> GetTrackingPackageRequired(Int32 tenantId, string SelectedPackageIDs);
        Boolean SaveComplianceItem(TrackingPackageRequiredContract trackingPackageRequiredContract, Int32 currentloggedInUserId);
        Boolean CheckDuplicateRecords(TrackingPackageRequiredContract trackingPackageRequiredContract, Int32 currentloggedInUserId);
       Boolean DeleteComplianceItem(TrackingPackageRequiredContract trackingPackageRequiredContract, Int32 currentloggedInUserId);
        #endregion

        #region Manage Compliance Categories

        /// <summary>
        /// Returns all the compliance categories viewable to the current logged in user. 
        /// </summary>
        /// <returns>List of Compliance Categories</returns>
        List<ComplianceCategory> GetComplianceCategories(Boolean getTenantName, List<Entity.Tenant> tenantList);

        /// <summary>
        /// Returns all the compliance categories viewable to the current logged in user. 
        /// </summary>
        /// <returns>List of Compliance Categories</returns>
        List<ComplianceCategory> GetComplianceCategoriesForNodes(string dpmIds);

        /// <summary>
        /// Saves the Compliance Category.
        /// </summary>
        /// <param name="category">ComplianceCategory Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>ComplianceCategory Entity</returns>
        ComplianceCategory SaveCategoryDetail(ComplianceCategory category, Int32 currentLoggedInUserId);

        /// <summary>
        /// Deletes the Compliance Category.
        /// </summary>
        /// <param name="category">ComplianceCategory Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>True or False</returns>
        Boolean DeleteComplianceCategory(Int32 categoryID, Int32 currentUserId);

        /// <summary>
        /// Updates the Compliance Category.
        /// </summary>
        /// <param name="category">ComplianceCategory Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        void UpdateCategoryDetail(ComplianceCategory category, Int32 currentLoggedInUserId, Boolean isMasterScreen = false);

        /// <summary>
        /// Checks if the category name already exists.
        /// </summary>
        /// <param name="categoryName">Category Name</param>
        /// <param name="complianceCategoryId">Compliance Category Id</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>True or false</returns>
        Boolean CheckIfCategoryNameAlreadyExist(String categoryName, Int32 complianceCategoryId, Int32 tenantId);

        /// <summary>
        /// To get Not Mapped Compliance Categories
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        List<ComplianceCategory> GetNotMappedComplianceCategories(Int32 packageId);

        Boolean UpdateCompliancePackageCategoryDisplayOrder(CompliancePackageCategory compliancePackageCategory, Int32 currentloggedInUserId);

        CompliancePackageCategory GetCompliancePackageCategory(Int32 categoryId, Int32 packageId);

        #endregion

        #region Manage Compliance Items

        /// <summary>
        /// Gets the list of Compliance items not deleted
        /// </summary>
        /// <returns>List of the compliance items, not deleted</returns>
        List<ComplianceItem> GetComplianceItems(Boolean getTenantName, List<Entity.Tenant> tenantList);

        /// <summary>
        /// Gets the list of Compliance items not deleted
        /// </summary>
        /// <returns>List of the compliance items, not deleted</returns>
        List<ComplianceItem> GetComplianceItemsForNodes(string dpmIds);

        /// <summary>
        /// Get the list of Items not associated with the current Category in the mapping screen.
        /// </summary>
        /// <param name="currentCategoryId">Id of the category not associated with</param>
        /// <returns>Lit of the compliance items.</returns>
        List<ComplianceItem> GetAvailableComplianceItems(Int32 currentCategoryId);

        /// <summary>
        /// Delete the selected compliance item
        /// </summary>
        /// <param name="complianceItemId">Id of the item to delete</param>
        /// <returns>Status of deletion, if it was success or association exists</returns>
        Boolean DeleteComplianceItem(Int32 complianceItemId, Int32 currentUserId);

        /// <summary>
        /// Save/Update the compliance item
        /// </summary>
        /// <param name="complianceItem">Details of the compliance item to save/update</param>
        ComplianceItem SaveComplianceItem(ComplianceItem complianceItem, Int32 tenantId);

        Boolean CheckIfItemAlreadyExist(String complianceItem, Int32 complianceItemId, Int32 tenantId);


        void UpdateComplianceItem(ComplianceItem complianceItem, Int32 tenantId, Boolean isMasterScreen = false);

        /// <summary>
        /// Save category-item mapping
        /// </summary>
        /// <param name="complianceItem">Details of the compliance item with mapping information to save/update</param>
        /// <param name="tenantId">Id of the tenant to which the current user belongs to</param>
        void SaveCategoryItemMapping(ComplianceCategoryItem complianceCategoryItem);

        List<ComplianceItem> GetComplianceItemsByCategory(List<Int32> lstCategoryIds);

        List<SeriesAttributeContract> GetUnMappedAttributes(Int32 itemSeriesID);
        Boolean SaveUnMappedAttributes(Int32 currentUserID, List<SeriesAttributeContract> lstSeriesAttributeContract);

        Boolean UpdateComplianceCategoryItemDisplayOrder(Int32 itemId, Int32 categoryId, Int32 displayOrder, Int32 currentloggedInUserId);

        #endregion

        #region Manage Compliance Attributes

        List<ComplianceAttribute> GetComplianceAttributes(Boolean getTenantName, List<Entity.Tenant> tenantList);

        List<ComplianceAttribute> GetNotMappedComplianceAttributes(Int32 itemId, Int32 fileUploadAttributeDatatypeId, Int32 viewDocAttributeDataTypeId);

        ComplianceAttribute GetComplianceAttribute(Int32 complianceAttributeID);

        /// <summary>
        /// To get compliance attribute group list
        /// </summary>
        /// <returns></returns>
        List<ComplianceAttributeGroup> GetComplianceAttributeGroup();

        Boolean AddComplianceAttribute(ComplianceAttribute complianceAttribute);

        Boolean UpdateComplianceAttribute(ComplianceAttribute complianceAttribute);

        Boolean DeleteComplianceAttribute(Int32 complianceAttributeID, Int32 modifiedByID);
        Boolean IsFileUploadAttributePresent(Int32 itemId, Int32 fileUploadAttributeDatatypeId);
        Boolean IsViewDocAttributePresent(Int32 itemId, Int32 viewDocAttributeDatatypeId);
        Boolean UpdateComplianceItemAttributeDisplayOrder(Int32 complianceAttributeID, Int32 itemID, Int32 displayOrder, Int32 currentloggedInUserId);

        #endregion

        #region Manage Compliance RuleSet

        RuleSet SaveComplianceRuleSetDetail(RuleSet ruleSet, Int32 currentUserId, Int32 tenantId);
        RuleSet UpdateComplianceRuleSetDetail(RuleSet ruleSet, Int32 currentUserId, Int32 tenantId);
        Boolean DeleteComplianceRuleSetDetail(Int32 ruleSetId, Int32 currentUserId, Int32 tenantId);

        #endregion

        #region Manage Comlianc Rules

        /// <summary>
        /// Gets all the rows from table RuleMappings.
        /// </summary>
        /// <returns>List of type RuleMapping</returns>
        List<RuleMapping> GetAllRuleMappings();

        /// <summary>
        /// Gets all the rows from table RuleTemplateExpression.
        /// </summary>
        /// <returns>List of type RuleTemplateExpression</returns>
        List<RuleTemplateExpression> GetAllRuleTemplateExpression();

        /// <summary>
        /// Gets all the rows from table RuleMappingDetail.
        /// </summary>
        /// <returns>List of type RuleMappingDetail</returns>
        List<RuleMappingDetail> GetAllRuleMappingDetails();

        /// <summary>
        /// Gets all the rows from table RuleMappingObjectTree.
        /// </summary>
        /// <returns>List of type RuleMappingObjectTree</returns>
        List<RuleMappingObjectTree> GetAllRuleMappingObjectTree();

        #endregion
        #endregion

        #region Copy to Client Admin

        ComplianceAttribute SaveComplianceAttribute(ComplianceAttribute complianceAttribute, Int32 currentUserId);

        #endregion

        #region Admin Mapping Screens

        #region Package-Category Mapping
        List<ComplianceCategory> GetComplianceCategoriesByPermissionList(List<Entity.Tenant> tenentIds, Int32 tenantId, List<Int32> selectedPackageIds, String dpm_ID, Int32? OrganisationId,Boolean IsAdminLoggedIn);
        List<CompliancePackageCategory> GetcomplianceCategoriesByPackage(List<Int32> packageIds, Boolean getTenantName, List<Entity.Tenant> tenantList);
        List<ComplianceCategory> GetcomplianceCategoriesByPackageList(Int32 packageId, List<Entity.Tenant> tenantList, Int32 tenantId);
        Boolean SaveCompliancePackageCategoryMapping(CompliancePackageCategory compliancePackageCategory, Int32 currentUserId, Boolean IsCreatedByAdmin);
        ComplianceCategory getCurrentCategoryInfo(Int32 categoryId);
        CompliancePackage GetCurrentPackageInfo(Int32 packageId);
        Boolean DeleteCompliancePackageCategoryMapping(Int32 packageId, Int32 categoryId, Int32 currentUserId);
        Boolean DeletePackageCategoryMappingAndAssociatedData(Int32 packageId, Int32 categoryId, Int32 currentUserId, Int32 tenantId);
        void ProcessOptionalCategory(Int32 packageId, Int32 categoryId, Int32 currentUserId);
        void ProcessRequiredCategory(Int32 packageId, Int32 categoryId, Int32 currentUserId);
        List<dynamic> GetPackageBundleNodeHierarchy(Int32 packageId);
        #endregion

        #region Category-Item Mapping

        /// <summary>
        /// Gets the list of Items related to a category
        /// </summary>
        /// <param name="categoryId">Id of the selected category</param>
        /// <returns>List of the items of that category</returns>
        List<ComplianceCategoryItem> GetComplianceCategoryItems(Int32 categoryId, Boolean ifTenantNameRequired, List<Entity.Tenant> tenantList);
        void DeleteCategoryItemMapping(Int32 categoryItemId, Int32 currentLoggedInUserId);
        Boolean DeleteCategoryItemMappingAndAssociatedData(Int32 categoryId, Int32 itemId, Int32 currentLoggedInUserId, Int32 tenantId);
        ComplianceItem getCurrentItemInfo(Int32 itemId);
        #endregion

        #region Item-Attributes Mapping

        List<ComplianceItemAttribute> GetComplianceItemAttribute(Int32 itemID, Boolean ifTenantNameRequired, List<Entity.Tenant> tenantList);

        ComplianceItemAttribute GetComplianceItemAttributeByID(Int32 cia_ID);

        Boolean AddComplianceItemAttribute(ComplianceItemAttribute complianceItemAttribute);
        Boolean DeleteComplianceItemAttribute(Int32 cia_ID, Int32 modifiedByID);
        Boolean DeleteComplianceItemAttributeAndAssociatedData(Int32 itemId, Int32 attributeId, Int32 modifiedByID, Int32 tenantId);

        #endregion

        #region Rule Set Mapping

        /// <summary>
        /// Gets all the admin rule set objects.
        /// </summary>
        /// <returns>List<RuleSetObject></returns>
        List<RuleSet> GetRuleSet();

        /// <summary>
        /// Gets the rule set for the given Rule Set ID.
        /// </summary>
        /// <param name="ruleSetId">Rule Set Id</param>
        /// <returns>RuleSet entity</returns>
        RuleSet GetRuleSetInfoByID(Int32 ruleSetId);

        /// <summary>
        /// Gets the list of rule set for the selected object.
        /// </summary>
        /// <param name="objectId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <returns>List of RuleSet</returns>
        List<RuleSet> GetRuleSetForObject(Int32 associationHierarchyId, Int32 objectTypeId);

        /// <summary>
        /// Saves the rule set and object mapping.
        /// </summary>
        /// <param name="ruleSetObject">RuleSet Object</param>
        /// <param name="currentUserId">Current User Id</param>
        void SaveRuleSetObjectMapping(RuleSetObject ruleSetObject, Int32 currentUserId);

        /// <summary>
        /// Deletes the rule set and object mapping.
        /// </summary>
        /// <param name="ruleSetId">RuleSet Id</param>
        /// <param name="objectId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="currentUserId">Current User Id</param>
        void DeleteRuleSet(Int32 ruleSetId, Int32 objectId, Int32 objectTypeId, Int32 currentUserId);

        #endregion

        #region Common Methods

        /// <summary>
        /// Get the hierarchical tree data for mapping screen
        /// </summary>
        /// <param name="packageIds"></param>
        /// <returns></returns>
        ObjectResult<GetRuleSetTree> GetRuleSetTree(String packageIds);

        void SaveLargeContentRecord(LargeContent largeContent, Int32 objectTypeID, Int32 contentTypeID, Int32 currentUserId);
        LargeContent getLargeContentRecord(Int32 objectId, Int32 objectTypeID, Int32 contentTypeID);

        /// <summary>
        /// Get the hierarchical tree data for copy to client screen
        /// </summary>
        /// <returns></returns>
        ObjectResult<GetRuleSetTree> GetComplianceTree();

        /// <summary>
        /// Get the package hierarchical tree data for package detail screen
        /// </summary>
        /// <returns>ObjectResult<GetPackageDetail></returns>
        ObjectResult<GetPackageDetail> GetPackageDetailTree(Int32 packageID);

        /// <summary>
        /// To get the Portfolio Subscription tree hierarchical data
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        ObjectResult<GetPortfolioSubscriptionTree> GetPortfolioSubscriptionTree(Int32 organizationUserId);

        /// <summary>
        /// Get the hierarchical Department tree data
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        ObjectResult<GetDepartmentTree> GetDepartmentTree(Int32 departmentId);

        /// <summary>
        /// Get Institute Hierarchy Tree data
        /// </summary>
        /// <returns></returns>
        List<InstituteHierarchyTreeDataContract> GetInstituteHierarchyTree(int? orgUserID, Boolean fetchNoAccessNodes = false);

        /// <summary>
        /// Get Institute Hierarchy Tree data
        /// </summary>
        /// <returns></returns>
        ObjectResult<InstituteHierarchyNodesList> GetInstituteHierarchyNodes(int? orgUserID, Boolean fetchNoAccessNodes = false); //UAT-3369

        /// <summary>
        /// Get Institute Hierarchy Tree data
        /// </summary>
        /// <returns></returns>
        ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreeForConfiguration(int? orgUserID);

        ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreeForBackgroundHierarchyPermissionType(Int32? orgUserID, Boolean fetchNoAccessNodes = false);

        ObjectResult<GetInstituteHierarchyOrderTree> GetInstituteHierarchyOrderTree(int? orgUserID);

        /// <summary>
        /// Get Feature Permission Tree data
        /// </summary>
        /// <returns></returns>
        DataTable GetFeaturePermissionTree(Guid? userID, Int32 tenantId);


        /// <summary>
        /// Get the list of nodes which are associated with client.
        /// </summary>
        /// <returns></returns>
        ObjectResult<ComplianceAssociations> GetComplianceAssociations();

        /// <summary>
        /// Get the list of largeContent
        /// </summary>
        /// <returns></returns>
        List<LargeContent> GetLargeContent();

        /// <summary>
        /// Get Department Program Mapping object.
        /// </summary>
        /// <param name="DepPrgMappingId">DepPrgMappingId</param>
        /// <returns>DeptProgramMapping</returns>
        DeptProgramMapping GetDepartmentProgMapping(Int32 DepPrgMappingId);

        /// <summary>
        /// Get Department Program Mapping object.
        /// </summary>
        /// <param name="delimittedDepPrgMappingId">delimittedDepPrgMappingId</param>
        /// <returns>List of DeptProgramMapping</returns>
        List<DeptProgramMapping> GetDepartmentProgMappingList(String delimittedDepPrgMappingId);
        #endregion

        #endregion

        #region Copy to Client

        /// <summary>
        /// Copies/Removes the elements from Admin to Client
        /// </summary>
        /// <param name="lstElementsToAdd">Elements that are selected by admin for copying to client</param>
        /// <param name="lstElementsToRemove">Elements de-selected by admin to remove from the client</param>
        /// <param name="lstAdminPackages">List of Master packages of Admin, to get the details of particular package, when required.</param>
        /// <param name="lstAdminCategories">List of Master categories of Admin, to get the details of particular category, when required.</param>
        /// <param name="lstAdminItems">List of Master items of Admin, to get the details of particular item, when required.</param>
        /// <param name="lstAdminAttributes">List of Master attributes of Admin, to get the details of particular attribute, when required.</param>
        /// <param name="currentUserId">Id of the currently logged in user.</param>
        void CopyToClient(List<GetRuleSetTree> lstElementsToAdd, List<GetRuleSetTree> lstElementsToRemove, ComplianceSetUpContract adminData, Int32 currentUserId, Int32 tenantID);

        void CopyComplianceToClient(String complianceDetails, Int32 currentUserId, Int32 tenantID);

        CompliancePackage GetPackageDetailsByCode(Guid? copiedFromCode);
        ComplianceCategory GetCategoryDetailsByCode(Guid? copiedFromCode);
        ComplianceItem GetItemDetailsByCode(Guid? copiedFromCode);
        ComplianceAttribute GetAttributeDetailsByCode(Guid? copiedFromCode);

        List<CompliancePackageCategory> GetCompliancePackageCategoryList();

        List<Entity.InstitutionWebPage> GetAdminInstitutionWebPageList(List<GetRuleSetTree> lstComplainceElements, String recordTypeCode, Int32 tenantId);
        #endregion

        #region Methods to get look up data

        List<lkpObjectType> GetlkpObjectType();
        List<lkpRuleObjectMappingType> GetlkpRuleObjMapType();
        List<RuleSetTree> GetAllRuleSetTree();
        List<AssignmentHierarchy> GetAssignmentHierarchy();

        #endregion

        #region assignment properties

        List<Tenant> GetThirdPartyReviewers(Int32 packageID);

        AssignmentProperty GetAssignmentPropertyDetails(Int32 currentDataID, Int32 parentCategoryDataID, Int32 parentPackageDataID, Int32 parentItemDataID, String currentObjectTypeCode, Int32 objectTypeId, List<lkpObjectType> lkpObjectType);

        void UpdateAssignmentProperties(AssignmentProperty assignmentProperty, Int32 currentDataId, Int32 parentPackageId, Int32 parentCategoryId, Int32 parentItemId, String currentRuleSetTreeTypeCode, Int32 loggedInUserId);

        AssignmentProperty FetchAssignmentOptions(Int32 parentPackageDataID, List<lkpObjectType> lkpObjectType, Int32 parentCategoryDataID = 0, Int32 itemDataID = 0);

        List<ListItemAssignmentProperties> GetAssignmentPropertiesByCategoryId(Int32 packageId, Int32 categoryId);

        List<ListItemEditableBies> GetEditableBiesByCategoryId(Int32 packageId, Int32 categoryId);

        /// <summary>
        ///  Gets the list of Editable Bies for all the attributes in all the items in a category
        /// </summary>
        /// <param name="parentPackageDataID"></param>
        /// <param name="parentCategoryDataID"></param>
        /// <param name="isAdmin"></param>
        /// <param name="itemDataID"></param>
        /// <returns></returns>
        List<AssignmentHierarchyEditableByContract> GetEditableBies(Int32 parentPackageDataID, Int32 parentCategoryDataID, Boolean isAdmin, List<lkpObjectType> lkpObjectType, List<lkpEditableBy> lstEditableBy, Int32 itemDataID = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObjectTypeID"></param>
        /// <param name="parentObjectID"></param>
        /// <param name="newObjectTypeID"></param>
        /// <param name="newObjectID"></param>
        /// <param name="loggedInUserId"></param>
        void AddAssociationHierarchyNode(Int32? parentObjectTypeID, Int32? parentObjectID, Int32 newObjectTypeID, Int32 newObjectID, Int32 loggedInUserId, Boolean isDefaultAssgnmntRqud);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObjectTypeID"></param>
        /// <param name="parentObjectID"></param>
        /// <param name="newObjectTypeID"></param>
        /// <param name="newObjectID"></param>
        /// <param name="loggedInUserId"></param>
        void DeleteAssociationHierarchyNode(Int32? parentObjectTypeID, Int32? parentObjectID, Int32 newObjectTypeID, Int32 newObjectID, Int32 loggedInUserId);

        /// <summary>
        /// method to get association hierarchy id
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="itemId"></param>
        /// <param name="attributeId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Int32? getAssociationHierarchyIdForObject(Int32 loggedInUserId, Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attributeId);

        List<ListCategoryEditableBies> GetEditableBiesByPackageId(Int32 packageId);
        #endregion

        #region Subscription Options

        List<SubscriptionOption> GetSubscriptionOptionsList();

        void SaveSubscriptionOption(SubscriptionOption newSubscriptionOption, Int32? subscriptionOptionID = null);

        void DeletSubscriptionOption(SubscriptionOption subscriptionOption);

        #endregion

        #region Department, Program Subscriptions and Price

        /// <summary>
        /// To get Price Adjustment List
        /// </summary>
        /// <returns></returns>
        List<PriceAdjustment> GetPriceAdjustmentList();

        /// <summary>
        /// To get Program Packages by Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        List<DeptProgramPackage> GetProgramPackagesByProgramMapId(Int32 deptProgramMappingID);

        /// <summary>
        /// Get the successor package dropdownlist selectedvalue
        /// </summary>
        /// <param name="DeptProgramMappingID">Source nodeid</param>
        /// <param name="SelectedSuccessorNodeID">target nodeid</param>
        /// <returns></returns>
        List<MobilityPackageRelation> GetSuccessorPackageIds(Int32 DeptProgramMappingID, Int32 SelectedSuccessorNodeID);

        /// <summary>
        /// To get Program Packages for the given list of Program Ids.
        /// </summary>
        /// <param name="programIds"></param>
        /// <returns></returns>
        List<CompliancePackage> GetProgramPackagesByProgramId(Int32 departmentId, List<Int32> programIds);

        /// <summary>
        /// To get not mapped Compliance Packages
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        List<CompliancePackage> GetNotMappedCompliancePackagesByMapId(Int32 deptProgramMappingID);

        /// <summary>
        /// To get Institution Nodes By Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        IQueryable<DeptProgramMapping> GetInstitutionNodesByProgramMapId(Int32 deptProgramMappingID);

        /// <summary>
        /// To get Institution Child Nodes By Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        List<Int32> GetInstitutionChildNodesByProgramMapId(List<Int32> deptProgramMappingIDs);

        /// <summary>
        /// To save Program Package Mapping
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="IsCreatedByAdmin"></param>
        /// <returns></returns>
        Boolean SaveProgramPackageMapping(Int32 deptProgramMappingID, Int32 packageId, Int32 currentUserId, Boolean IsCreatedByAdmin, List<Int32> _lstSelectedOptionIds, Int32 paymentApprovalRequiredID);

        ///// <summary>
        ///// Save/Update the Package Level Payment Options for Compliance Package
        ///// </summary>
        ///// <param name="currentUserId"></param>
        ///// <param name="dppId"></param>
        ///// <param name="_lstSelectedOptionIds"></param>
        //void SaveCompliancePackagePaymentOptions(Int32 currentUserId, Int32 dppId, List<Int32> _lstPaymentOptionIds);

        /// <summary>
        /// To save Program Package Mapping Node
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="nodeId"></param>
        /// <param name="nodeName"></param>
        /// <param name="paymentOptions"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean SaveProgramPackageMappingNode(Int32 tenantId, Int32 deptProgramMappingID, Int32 nodeId, String nodeName, List<Int32> paymentOptions, List<Int32> fileExtensions,
                                                     Int32 currentUserId, String nodeLabel, Boolean isAvailableForOrder, Boolean isEmployment,
                                                     Int32? archivalGracePeriod, Int32? PDFInclusionID, Int32? resultsSentToApplicantID, String splashPageUrl, String ExpirationFrequency,
                                                     Int32? AfterExpirationFrequency, Int32? SubscriptionBeforeExpiry, Int32? SubscriptionAfterExpiry, Int32? SubscriptionExpiryFrequency,
                                                     Int32 paymentApprovalID, Int16 nagEmailNotificationTypeId, Int32? hierarchyNodeExemptedType, Boolean IsCallFromBkgHierarchySetup); //UAT-2501: Added two parameters tenantId,nagEmailNotificationTypeId

        /// <summary>
        /// To delete Program Package Mapping
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean DeleteProgramPackageMapping(Int32 deptProgramMappingID, Int32 packageId, Int32 currentUserId);

        /// <summary>
        /// To get Dept Program Package Subscription List by Dept Program Package Id
        /// </summary>
        /// <param name="DeptProgramPackageId"></param>
        /// <returns></returns>
        List<DeptProgramPackageSubscription> GetDeptProgramPackageSubscriptionByProgPackageId(Int32 DeptProgramPackageId);

        /// <summary>
        /// To get Dept Program Package by Package Id
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        List<DeptProgramPackage> GetDeptProgramPackageByPackageId(Int32 packageId);

        /// <summary>
        /// To save Program Package Subscription Mapping
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="subscriptionIDs"></param>
        /// <param name="priceModelID"></param>
        /// <param name="savedPriceModelId"></param>
        /// <param name="priority"></param>
        /// <param name="currentUserId"></param>
        /// <param name="lstSelectedOptionIds"></param>
        /// <returns></returns>
        Boolean SaveProgramPackageSubscriptionMapping(Int32 deptProgramPackageID, List<Int32> subscriptionIDs, Int32 priceModelID, Int32 savedPriceModelId, Int32 priority,
                                                      Int32 currentUserId, List<Int32> lstSelectedOptionIds, Int32 paymentApprovalID);

        /// <summary>
        /// To save Price and Price Adjustments Detail data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="parentID"></param>
        /// <param name="mappingID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="complianceCategoryID"></param>
        /// <param name="price"></param>
        /// <param name="rushOrderAdditionalPrice"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        PriceContract SavePriceAdjustmentDetail(Int32 ID, Int32 parentID, Int32 mappingID, Int32 parentSubscriptionID, Int32 complianceCategoryID, Decimal price, Decimal? rushOrderAdditionalPrice, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId, String treeNodeType);

        /// <summary>
        /// To update Price Adjustment Detail
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        Boolean UpdatePriceAdjustmentDetail(Int32 ID, Int32 priceID, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId, String treeNodeType);

        /// <summary>
        /// To get Price Adjustment Data by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        List<PriceContract> GetPriceAdjustmentData(Int32 ID, String treeNodeType);

        /// <summary>
        /// To get Dept Program Package By ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <returns></returns>
        DeptProgramPackage GetDeptProgramPackageByID(Int32 deptProgramPackageID);

        /// <summary>
        /// To get Dept Program Package Subscription by ID
        /// </summary>
        /// <param name="deptProgramPackageSubscriptionID"></param>
        /// <returns></returns>
        DeptProgramPackageSubscription GetDeptProgramPackageSubscriptionByID(Int32 deptProgramPackageSubscriptionID);

        /// <summary>
        /// To get Price
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        PriceContract GetPrice(Int32 ID, String treeNodeType, Int32 ParentID = 0, Int32 MappingID = 0, Int32 ParentSubscriptionID = 0, Int32 ComplianceCatagoryID = 0, Int32 ItemID = 0);

        /// <summary>
        /// To check if Price is Disabled
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="mappingID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        Boolean CheckIsPriceDisabled(Int32 parentID, Int32 parentSubscriptionID, Int32 mappingID, String treeNodeType);

        /// <summary>
        /// To show Message
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="mappingID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        Boolean ShowMessage(Int32 parentID, Int32 parentSubscriptionID, Int32 mappingID, String treeNodeType);

        /// <summary>
        /// To delete Price Adjustment Data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        Boolean DeletePriceAdjustmentData(Int32 ID, Int32 priceID, Int32 currentUserId, String treeNodeType);

        #region Institute Hierarchy Nodes

        /// <summary>
        /// To get Institution Node Types
        /// </summary>
        /// <returns></returns>
        IQueryable<InstitutionNodeType> GetInstitutionNodeTypes();

        /// <summary>
        /// To get Institution Nodes
        /// </summary>
        /// <param name="nodeTypeId"></param>
        /// <returns></returns>
        List<InstitutionNode> GetInstitutionNodes(Int32 nodeTypeId);

        /// <summary>
        /// To delete Program Package Mapping Node
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean DeleteProgramPackageMappingByID(Int32 deptProgramMappingID, Int32 currentUserId);

        /// <summary>
        /// To delete dept Program Package by ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean DeleteProgramPackageByID(Int32 deptProgramPackageID, Int32 currentUserId);

        /// <summary>
        /// To save mapped Payment Options and Update the availability of the node, for the Order process
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="paymentOptions"></param>
        /// /// <param name="isAvailableForOrder"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean SaveMappedPaymentOptionsNodeAvailability(Int32 deptProgramMappingID, List<Int32> paymentOptions, List<Int32> fileExtensions, Boolean isAvailableForOrder, Boolean isEmployment, Int32 currentUserId,
                                                                 String splashPageUrl, String ExpirationFrequency, Int32? AfterExpirationFrequency,
                                                                 Int32? SubscriptionBeforeExpiry, Int32? SubscriptionAfterExpiry, Int32? SubscriptionExpiryFrequency,
                                                                 String IsAdminDataEntryAllow, Int32 paymentApprovalID, String OptionalCategorySetting, Int32? PDFInclusionID, Int32? resultsSentToApplicant, Int32? hierarchyNodeExemptedType,
                                                                 Boolean IsCallFromBkgHierarchySetup);

        /// <summary>
        /// To get child nodes with Permission
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        ObjectResult<GetChildNodesWithPermission> GetChildNodesWithPermission(Int32 deptProgramMappingID, Int32? currentUserId);

        #endregion

        #endregion

        #region Price Adjustment
        /// <summary>
        /// Method to return all price adjustment.
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<PriceAdjustment> GetAllPriceAdjustment();

        /// <summary>
        /// Get the Price Adjustment by priceAdjustmentId
        /// </summary>
        /// <param name="priceAdjustmentId">priceAdjustmentId</param>
        /// <returns>PriceAdjustment</returns>
        PriceAdjustment GetPriceAdjustmentById(Int32 priceAdjustmentId);

        /// <summary>
        /// Save PriceAdjustment
        /// </summary>
        /// <param name="priceAdjustment">priceAdjustment</param>
        /// <returns>Boolean</returns>
        Boolean SavePriceAdjustment(PriceAdjustment priceAdjustment);

        /// <summary>
        /// Update PriceAdjustment
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean UpdatePriceAdjustment();
        /// <summary>
        /// Check Price Adjustment Mapping
        /// </summary>
        /// <param name="priceAdjustmentId">priceAdjustmentId</param>
        /// <returns>Boolean</returns>
        Boolean IsPriceAdjustmentMapped(Int32 priceAdjustmentId);

        #endregion

        #region Institution Node Type

        IQueryable<InstitutionNodeType> GetAllInstitutionNodeType();
        Boolean SaveInstitutionNodeType(InstitutionNodeType institutionNodeType);
        Boolean UpdateInstitutionNodeType();
        InstitutionNodeType GetInstitutionNodeTypeById(Int32 institutionNodeTypeId);
        Boolean IsInstitutionNodeTypeMapped(Int32 institutionNodeType);
        String GetLastInstitutionNodeTypeCode();
        void CopyPackageStructure(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Boolean updateExistingSub, Int32 srcNodeId, Int32 trgtNodeId);
        void CopyPackageStructureToMaster(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 tenantID);
        void CopyPackageStructureToClient(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 tenantID);

        #endregion

        #region Manage User Group

        IQueryable<UserGroup> GetAllUserGroup();
        List<UserGroup> GetAllUserGroupWithPermission(Int32? currentUserId);

        List<UserGroup> GetAllUserGroupWithPermissionAll(Int32? currentUserId, String selectedHierarchyIds);

        String ArchiveUnArchiveUserGroups(List<Int32> listUserGroupIds, bool isArchive);
        Boolean SaveUserGroup(UserGroup userGroup);
        Boolean UpdateUserGroup();
        UserGroup GetUserGroupById(Int32 userGroupId);
        Boolean IsUserGroupMapped(Int32 userGroupId);
        #endregion

        #region Map User Hierarchy Permission
        IQueryable<vwHierarchyPermission> GetHierarchyPermissionList(Int32 hierarchyID);

        List<ComplaincePackageDetails> GetPermittedPackagesByUserID(Int32? orgUserId);

        List<ComplianceCategoryDetails> GetPermittedCategoriesByUserID(Int32? orgUserId);

        //IQueryable<vwHierarchyPermission> GetHierarchyPermission(Int32 hierarchyId);

        Boolean SaveHierarchyPermission(HierarchyPermission hierarchyPermission, List<String> lstHierarchyPermissionTypeCode);

        //Boolean UpdateHierarchyPermission(HierarchyPermission hierarchyPermission, List<String> lstHierarchyPermissionTypeCode, Int32 hierarchyPermissionID);
        Boolean UpdateHierarchyPermission();

        Boolean DeleteHierarchyPermission();

        HierarchyPermission GetHierarchyPermissionByID(Int32 hierarchyPermissionID);
        #endregion

        #region ManageComplianceAttributeGroup
        IQueryable<ComplianceAttributeGroup> GetAllComplianceAttributeGroup();
        Boolean SaveAttributeGroup(ComplianceAttributeGroup attributeGroup);
        Boolean UpdateAttributeGroup();
        ComplianceAttributeGroup GetAttributeGroupById(Int32 attributeGroupId);
        Boolean IsAttributeGroupMapped(Int32 attributeGroupId);
        #endregion

        #region ApplicantDataAuditHistory
        List<ApplicantDataAuditHistory> GetApplicantDataAuditHistory(CustomPagingArgsContract gridCustomPaging, SearchItemDataContract searchItemDataContract);
        #endregion

        #region Data Entry Help
        /// <summary>
        /// Gets Data entry help content by web site id and recordId
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        InstitutionWebPage GetDataEntryHelpContentByPackageId(Int32 tenantId, Int32? recordId, String recordType, String webSiteWebPageType);

        /// <summary>
        /// Get WebsiteWebPageType Id by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Int32 GetWebsiteWebPageTypeIdByCode(String code);

        /// <summary>
        /// Get RecordType id by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Int16 GetRecordTypeIdByCode(String code);

        /// <summary>
        /// Get existing package id list.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        List<Int32?> GetExistingPackageIdList(Int32 tenantId, String recordType, String webSiteWebPageType);

        Boolean SaveInstitutionWebPage(InstitutionWebPage InstitutionWebPage);

        /// <summary>
        /// Update Institution Web Page
        /// </summary>
        /// <param name="webSiteWebConfigID"></param>
        /// <returns></returns>
        Boolean UpdateInstitutionWebPage(InstitutionWebPage institutionWebPage);

        /// <summary>
        /// Gets Institution web page
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        InstitutionWebPage GetInstitutionWebPage(Int32 institutionWebPageID);
        /// <summary>
        /// Gets Data entry help content by web site id, recordId, recordTypeCode and websiteWebPageTypeCode
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="packageID"></param>
        /// <param name="recordTypeCode"></param>
        /// <param name="websiteWebPageTypeCode"></param>
        /// <returns></returns>
        InstitutionWebPage GeDateHelpHtmlFromtWebSiteWebPage(Int32 tenantID, Int32 packageID, String recordTypeCode, String websiteWebPageTypeCode);
        #endregion


        #region Manage Package Subscription

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientCompliancePackageID"></param>
        /// <param name="clearContext">To check whether need to get fresh data.</param>
        /// <returns></returns>
        CompliancePackage GetClientCompliancePackageByPackageID(Int32 clientCompliancePackageID, Boolean clearContext);

        /// <summary>
        /// Gets the list of packages for the current applicant for data entry form
        /// </summary>
        /// <param name="organisationUserID"></param>
        /// <returns>List of subscribed packages</returns>
        List<CompliancePackage> GetClientCompliancePackageByClient(Int32 organisationUserID);

        #endregion

        #region DISCLOSURE DOCUMENT
        /// <summary>
        /// Method to save the disclosure document in system Document table.
        /// </summary>
        /// <param name="systemDocument">SystemDocument</param>
        /// <returns>Boolean</returns>
        Boolean SaveDisclosureDocument(SystemDocument systemDocument);
        /// <summary>
        /// Get the disclosure document on the basis of recordId, recordTypeId and websitePageTypeId
        /// </summary>
        /// <param name="recordId">recordId</param>
        /// <param name="recordTypeId">recordTypeId</param>
        /// <param name="websiteWebPageTypeId">websiteWebPageTypeId</param>
        /// <returns>SystemDocument</returns>
        SystemDocument GetDisclosureDocument(Int32 systemDocumentId);
        /// <summary>
        /// Method that Save the changes  for update.
        /// </summary>
        /// <returns>Boolean</returns>
        Boolean UpdateChanges();

        /// <summary>
        /// Get Attached disclosure form list 
        /// </summary>
        /// <param name="websiteId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        DataTable GetAttachedDisclosureFormList(Int32 tenantId, String recordType, String webSiteWebPageType, CustomPagingArgsContract queueAuditArgsContract);

        #endregion

        #region Disclaimer Document
        ApplicantDocument SaveEsignedDocumentAsPdf(String PdfPath, String filename, Int32 fileSize, Int32 documentType, Int32 currentLoggedInUserId, Int32 orgUserID);
        ApplicantDocument GetESignedeDocument(Int32 applicantDocumentId, Int32 documentTypeId);
        #endregion

        #region GET SURVEY MONKEY LINK
        String GetSurveyMonkeyLink(Int32 tenantId, Int32 applicantId, String subEventCode, String packageName, String categoryName, String itemName, Int32 itemId, Int32 packageId, Int32 categoryId);
        #endregion

        #region Node Deadline and Notifications

        IQueryable<NodeDeadline> GetNodeDeadlines();
        List<Int32> GetCheckedUsergroupIds(Int32 mappingId);
        NodeDeadline GetNodeDeadlineByID(Int32 NodeDeadlineID);
        Boolean SaveNodeDeadline(NodeDeadline nodeDeadline);
        Boolean UpdateNodeDeadline(Int32 nodeDeadlineId, NodeNotificationSettingsContract nodeNotificationSettingsContract,
                                        List<Int32> userGroupIDs, Int32 currentUserId);
        Boolean DeleteNodeDeadline();
        Boolean SaveNagEmailNotifications(Int32 tenantId, Int16 nagEmailNotificationTypeId, Int32 hierarchyNodeID, Int32? nagFrequency, Int32 currentUserId, Boolean isActive);
        NodeNotificationMapping GetNodeNotificationMappingByNodeID(Int16 nagEmailNotificationTypeId, Int32 hierarchyNodeID);

        #endregion

        #region Backround Package Detials
        List<Entity.ClientEntity.GetBkgPackageDetailTree> GetBkgPackageDetailTree(Int32 bkgPackageId);
        String GetDeptProgMappingLabel(Int32 NodeId);
        String GetBkgpackageOfNode(Int32 BkgPackageHierarchyMappingID);
        #endregion

        #region Compliance NodePackage Hierarchy
        String GetCompliancePackafeOfNode(Int32 DeptProgramPackageId);
        #endregion

        SystemDocument GetServiceFormDocument(Int32 systemDocumentId);

        List<CommunicationCCUsersList> GetCCusers(Int32 communicationSubEventId, Int32 tenantId,String hierarchyNodeID);

        List<CommunicationCCUsersList> GetCCusersWithNodePermissionAndCCUserSettings(Int32 communicationSubEventId, Int32 tenantId, Int32? hierarchyNodeID, Int32 objectTypeId, Int32 recordId);

        #region Disociation Work

        String GetCategoryDissociationStatus(Int32 categoryId, Int32 packageId);

        String GetItemDissociationStatus(Int32 packageId, Int32 categoryId, Int32 itemId);

        String GetAttributeDissociationStatus(Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attrId);

        Int32 DissociateCategory(Int32 tenantId, Int32 categoryId, String packageIDs, Int32 currentLoggedInUserId);

        Int32 DissociateItem(Int32 tenantId, Int32 packageId, String categoryIds, Int32 itemId, Int32 currentLoggedInUserId);

        Int32 DissociateAttribute(Int32 tenantId, Int32 packageId, Int32 categoryId, String itemIds, Int32 attrId, Int32 currentLoggedInUserId);
        //UAT: 4348
        bool IsAllowedOverrideDate(Int32 ItemId);

        List<CompliancePackage> GetCompliancePackagesAssociatedtoCat(Int32 categoryID, Int32 currentPackageID); //UAT-2582

        List<ComplianceCategory> GetComplianceCategoriesAssociatedtoItem(Int32 itemId, Int32 currentCategoryID); //UAT-2582
        List<ComplianceItem> GetComplianceItemsAssociatedtoAttributes(Int32 itemId, Int32 currentCategoryID,Int32 currentAttributeID); //UAT-4267

        #endregion

        #region Instruction Text
        Int32 GetItemAttributeMappingID(ComplianceItemAttribute itemAttributeMapping);

        Boolean SaveInstructionText(ComplianceAttributeContract complianceAttributeContract, int loggedInUserId);

        Boolean UpdateInstructionText(ComplianceAttributeContract complianceAttributeContract, Int32 AttrID, Int32 ItemId, Int32 CategoryId, Int32 PackageId);

        String GetAttributeInstructionText(ComplianceAttributeContract complianceAttributeContract, Int32 AttrID, Int32 ItemId, Int32 CategoryId, Int32 PackageId);

        Int32 GetAssignmentHierarchyID(Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attributeId);

        /// <summary>
        /// Gets all the rows from AttributeInstruction
        /// </summary>
        /// <returns>List</returns>
        List<AttributeInstruction> GetAllAttributeInstruction();
        #endregion

        /// <summary>
        /// To Set Compliance Package Availability For Order
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="paID"></param>
        /// <returns></returns>
        Boolean SetCompliancePkgAvailabilityForOrder(Int32 deptProgramPackageID, Int32 currentUserId, Int32 paID);

        /// <summary>
        /// To Get whether Compliance Package Available For Order or not
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <returns></returns>
        Boolean IsCompliancePkgAvailableForOrder(Int32 deptProgramPackageID);

        Boolean IsAutoRenewInvoiceOrderForPackage(Int32 deptProgramPackageID);

        Boolean SetAutoRenewInvoiceOrderForPackage(Int32 deptProgramPackageID, Int32 currentUserId, Boolean IsAutoRenewInvoiceOrder);

        DataTable GetUserNodePermissionForVerificationAndProfile(Int32 orgUserID);

        Boolean SaveArchivalGracePeriod(Int32 DPM_ID, Int32? archivalGracePeriod, Int32 currentUserId);

        Dictionary<String, Int32> GetEffectiveArchivalGracePeriod(Int32 DPM_ID, Int32 currentUserId);

        Boolean SaveRecieptDocument(String pdfDocPath, String filename, Int32 fileSize, Int32 documentTypeId, Int32 CurrentLoggedInUserID, Int32 OrderID, Int32 orgUserID);

        Order GetOrderFromOrderID(Int32 orderID);

        ApplicantDocument GetRecieptDocumentDataForOrderID(Int32 orderID);

        DataTable GetCommunicationCopySettingsOverride();

        Boolean CheckIfCommunicationNodeSettingExistForSelectednode(Int32 hierarchyNodeId, Int32 orgUserId);

        Boolean SaveCommunicationNodeCopySetting(CommunicationNodeCopySetting communicationNodeCopySetting, List<INTSOF.UI.Contract.Templates.CommunicationSettingsSubEventsContract> communicationSettingsSubEventsContractList);

        Boolean UpdateCommunicationNodeCopySetting(Int32 communicationNodeCopySettingID, Int32 nodeCopySettingID, Int32 currentLoggedInUserID, List<INTSOF.UI.Contract.Templates.CommunicationSettingsSubEventsContract> communicationSettingsSubEventsContractList);

        Boolean DeleteCommunicationNodeCopySetting(Int32 communicationNodeCopySettingID, Int32 currentLoggedInUserID);

        String GetFormattedString(Int32 orgUserID, Boolean isOrgUserProfileID);
        #region UAT-1185 New Compliance Package Type
        List<lkpCompliancePackageType> GetCompliancePackageTypes();
        #endregion

        #region UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
        List<LargeContent> GetExplanatoryNotesForItems(List<Int32> objectIds, Int32 objectTypeId, Int32 contentTypeId);

        #endregion

        #region UAt-1209: As an application admin, I should be able to enter a date range for when a category should be compliance required/not required.
        /// <summary>
        /// get categories required for compliance action.
        /// </summary>
        /// <returns></returns>
        DataTable GetCategoriesRqdForComplianceAction();

        /// <summary>
        /// Used for deleting old records from history table.
        /// </summary>
        /// <param name="cpc_Id"></param>
        void DeletePreviousComplianceRqdActionHistory(Int32 cpc_Id, Int32 currentUserId);
        #endregion

        InstitutionConfigurationDetailsContract GetInstitutionConfigurationDetails(Int32 hierarchyNodeID);
        ScreeningDetailsForConfigurationContract GetScreeningDetailsForInstitutionConfiguration(Int32 hierarchyNodeID, Int32 packageID, Int32 packageHierarchyNodeID);

        /// <summary>
        /// Method is used to get compliance package Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        CompliancePkgDetailContract GetCompliancePkgDetails(Int32 hierarchyNodeID, Int32 packageId, Int32 packageHierarchyID);

        #region GETTING INSTITUTION HIERARCHY LIST FOR COMMON SCREENS
        ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreeCommon(Int32? currentUserID, string IsRequestFromAddRotationScreen);
        #endregion

        /// <summary>
        /// Returns whether the Compliance is Required for given settings - UAT 1543
        /// </summary>
        /// <param name="isComplianceReq"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Boolean GetComplianceRqdByDateRange(Boolean isComplianceReq, DateTime? startDate, DateTime? endDate);

        List<ClientSystemDocument> GetClientSystemDocumentListByDocTypeID(Int32 docTypeID);

        #region UAT 1559 As an admin, I should be able to attach a form to be completed as a immunization package attribute.

        List<Entity.ClientEntity.ClientSystemDocument> GetComplianceViewDocuments(Int32 docTypeID);

        Boolean DeleteComplianceViewDocument(Int32 systemDocumentID, Int32 currentUserId);

        Boolean IsDocumentMappedWithAttribute(Int32 systemDocumentID);

        Boolean UpdateComplianceViewDocument(Entity.ClientEntity.ClientSystemDocument attributeDocument);

        Boolean SaveComplianceViewDocument(List<Entity.ClientEntity.ClientSystemDocument> lstViewDocuments);

        List<DocumentFieldMapping> GetDocumentFieldMapping(Int32 clientSystemDocumentID);

        Boolean UpdateDocumentFieldMapping(Int32 documentFieldMappingID, Int32 documentFieldTypeID, Int32 loggedInUdserID);

        #endregion

        String GetCategoryNamesByCategoryIds(String categoryIds);

        String GetItemNamesByItemIds(String itemIds);

        #region  Manage Shot Series

        List<ItemSery> GetItemShotSeries(Int32 categoryId);

        Boolean AddNewShotSeries(ItemSery itemSeries);

        Boolean DeleteItemSeries(Int32 itemSeriesId, Int32 currentLoggedInUserId);

        ItemSery GetCurrentItemSeriesInfo(Int32 currentSeriesID);

        void SaveSeriesData(Int32 seriesId, List<Int32> lstItemIds, Dictionary<Int32, Boolean> dicAttributeIds, Int32 currentUserId);

        /// <summary>
        /// Add New ItemSeriesItem, on table mapping new click
        /// </summary>
        void SaveItemSeriesItem(Int32 seriesId, Int32 itemId, Int32 currentUserId);

        /// <summary>
        /// Remove Item from ItemSeriesItem, on table mapping Remove click
        /// </summary>
        void RemoveItemSeriesItem(Int32 itemSeriesItemId, Int32 currentUserId);

        #endregion

        DataSet GetSeriesData(Int32 seriesId);

        /// <summary>
        /// Save-Update the mapping of the Item Attributes with the Series Attributes.
        /// </summary>
        /// <param name="lstSeriesItemContract"></param>
        /// <param name="currentUserId"></param>
        void SaveUpdateSeriesMapping(List<SeriesItemContract> lstSeriesItemContract, Int32 currentUserId);

        List<ComplianceAttribute> GetComplianceAttributesByItemIds(List<Int32> lstItemIds);

        List<GetShotSeriesTree> GetShotSeriesTreeData();

        List<Int32> GetItemSeriesItemsBySeriesId(Int32 seriesId);

        List<Int32> GetItemSeriesAttributeBySeriesId(Int32 seriesId);

        Int32 GetItemSeriesKeyAttributeBySeriesId(Int32 seriesId);

        Boolean UpdateItemSeries(ItemSery itemSeries);

        List<CompliancePackage> GetPackagesRelatedToCategory(Int32 categoryId);

        Boolean CheckIfSeriesMappedAttrExist(Int32 itemSeriesId);

        Dictionary<Int32, Boolean> GetComplianceRqdForPackage(Int32 packageId);

        #region UAT 1560 WB: We should be able to add documents that need to be signed to the order process.
        List<GenericSystemDocumentMappingContract> GetGenericSystemDocumentMapping(Int32 recordID, Int32 recordTypeID);

        String SaveAdditionalDocumentMapping(Int32 recordID, Int32 recordTypeID, List<Int32> lstSelectedDocumentsID, Int32 loggedInUserID);

        String DeleteAdditionalDocumentMapping(Int32 docMappingID, Int32 loggedInUserID);

        ApplicantDocument SaveEsignedAdditionalDocumentAsPdf(String PdfPath, String filename, Int32 fileSize, Int32 documentTypeId,
                                                                 Int32 currentLoggedInUseID, Int32 orgUserID, Int16 dataEntryDocStatusId, Boolean isSearchableOnly);
        #endregion





        Dictionary<Int32, String> GetDefaultPermissionForClientAdmin(Int32 currentUserID);

        #region UAT-1812:Creation of an Approval/rejection summary for applicant logins
        List<DataTable> GetAppSummaryDataAfterLastLogin(Int32 currentLoggedInUserID, DateTime? lastLoginTime);
        #endregion

        List<SeriesAttributeContract> GetSeriesDetailsForShuffleTest(int seriesID);

        List<RuleDetailsForTestContract> GetSeriesRuleDetailsForShuffleTest(int seriesID, Int32 selectedPackageID);

        List<RuleDetailsForTestContract> GetDetailsForComplianceRuleTest(int ruleMappingID);

        Dictionary<ShotSeriesSaveResponse, List<SeriesAttributeContract>> GetSeriesDetailsAfterShuffleTest(int seriesID, int systemUserID, string seriesAttributeXML, string ruleMappingXML, Int32 selectedPackageID);

        //UAT-2043:For Data Entry:  Quick Package Copy Across Tenants
        void CopyPackageStructureToOtherClient(Int32 TenantId, Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 SelectedTenantId);

        //UAT-2159 : Show Category Explanatory note as a mouseover on the category name on the student data entry screen.
        Dictionary<Int32, String> GetExplanatoryNotesForCategory(Int32 packageId);

        List<CopyDataQueue> GetDataForCopyToRequirement(Int32 trackingItmDataObjTypeID, Int32 trackingSubsDataObjTypeID, Int32 rotSubsObjTypeID, Int32 chunkSize);

        DataTable CopyComplianceDataToRequirement(Int32 LoggedInUserID, String ItemDataIds, String RPSIds);

        String GetComplianceAttributeDatatypeByAttributeID(Int32 tenantID, Int32 complianceAttrID);

        List<UniversalCategoryMapping> GetUniversalCategoryMappings(String universalMappingTypeCode);

        List<UniversalItemMapping> GetUniversalItemMappings(List<Int32> universalCategoryIds);

        List<UniversalAttributeMapping> GetUniversalAttributeMappings(List<Int32> universalItemIds);

        List<UniversalAttributeOptionMapping> GetUniversalAttributeOptionMappings(List<Int32> universalAttrIds);

        List<InstitutionConfigurationPackageDetails> GetInstitutionConfigurationBundlePackageDetailsList(Int32 bundlePackageID, Int32 hierarchyID); //UAT-2411

        //UAT-2339
        ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreewithPermissions(Int32? currentUserID);

        //UAT 2506
        List<AdminDataAuditHistory> GetAdminDocumentDataAuditHistory(AdminDataAuditHistory parameterContact, CustomPagingArgsContract customPagingArgsContract);

        List<AdminDataAuditHistory> GetDocumentAssignmentAuditHistory(AdminDataAuditHistory parameterContact);

        //UAT-2717
        List<CompliancePackage> GetTenantCompliancePackage(Int32 tenantId);
        //UAT-2386
        List<DeptProgramMapping> GetChildNodesByNodeID(Int32 NodeID);

        //UAT-2744
        List<GetDepartmentTree> GetInstituteHierarchyPackageTree(Int32? orgUserID, String compliancePackageTypeCode, Boolean IsCompliancePackage, Boolean fetchNoAccessNodes = false);

        DataTable GetAutomaticPackageInvitations(Int32 chunkSize);//UAT-2388
        Boolean UpdateAutomaticPackageInvitationsEmailStatus(List<Int32> AIPML_Ids, Int32 backgroundProcessUserId);//UAT-2388

        //UAT-2924: Add upcoming expirations to Since You Been Gone popup as part of the not compliant categories
        List<UpcomingCategoryExpirationContract> GetUpcomingExpirationcategoryByLoginId(Int32 currentUserID);

        #region UAT-2985
        List<UniversalFieldMapping> GetUniversalFieldMappings(String mappingTypeCode);

        List<UniversalFieldOptionMapping> GetUniversalFieldOptionMappings(List<Int32> UFM_Ids);
        #endregion

        void GetCategoryItemAttributeMappingID(Int32 complianceCategoryID, Int32 complianceItemID, Int32 complianceAttributeID, ref Int32 complianceCategoryItemID, ref Int32 complianceItemAttributeID);

        List<PackageBundleContract> GetPackageIncludedInBundle(Int32 ID);

        String GetLastRuleAppliedDate(Int32 associationHierarchyId); // UAT-3566

        PackageBundleNodeMapping GetPackageBundleNodeMapping(Int32 packageBundleId, Int32 deptProgramMappingId);
        Boolean UpdatePackageBundleNodeMapping(Int32 packageBundleId, Int32 deptProgramMappingId, Boolean isBundleExclusive,Int32 currentLoggedInUserId);

        #region UAT-3896
        String GetHierarchyTextForBundle(Int32 packageId);
        #endregion

        #region UAT-3951: Rejection Reason

        IQueryable<Entity.RejectionReason> GetRejectionReasons();
        Boolean SaveUpdateRejectionReason(Entity.RejectionReason rejectionReasonData);
        Boolean DeleteRejectionReason(Int32 rejectionReasonID, Int32 loggedInUserID);
        List<Entity.RejectionReason> GetRejectionReasonByIDs(List<Int32> lstRejectReasonIDs);
        
        #endregion

        #region UAT-3873
        /// <summary>
        /// To get Program Background Packages by Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        List<NodePackagesDetails> GetProgramAvailablePackagesByProgramMapId(Int32 deptProgramMappingID);
        #endregion

        IQueryable<DeptProgramRestrictedFileExtension> GetMappedDeptProgramFileExtensions(Int32 DeptProgramMappingId);

        //UAT- 4278
        List<DiscardedDocumentAuditContract> GetDiscardedDocumentDataAuditHistory(DiscardedDocumentAuditContract parameterContact, CustomPagingArgsContract customPagingArgsContract);
        #region Admin Entry Portal
        List<DeptProgramAdminEntryAcctSetting> GetDeptProgramAdminEntryAcctSettings(Int32 depProgramMappingId);
        bool SaveNodeSettingsForAdminEntry(Int32 depProgramMappingId, List<DeptProgramAdminEntryAcctSetting> deptProgramAdminEntryAcctSettingList);

        #endregion
        //UAT 4522
        Boolean UserGranularPermissionDigestion(Int32 organizationUserId, String entityCode, Int32 hierarchyNodeId, Int32 currentLoggedInUserId);

        #region UAT-5198
        Dictionary<String, String> IsCategoriesAvailableinSelectedPackages(List<Int32> lstPacakgeIds, List<Int32> lstCategoryIds, List<Tuple<Int32, Int32, Int32>> tuples);

        AssignmentHierarchy GetAssignmentHierarchyByRuleSetId(int ruleSetId);

        #endregion
    }
}
