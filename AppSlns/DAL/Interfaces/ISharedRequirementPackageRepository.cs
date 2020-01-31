using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System.Data;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace DAL.Interfaces
{
    public interface ISharedRequirementPackageRepository
    {
        Boolean AddRequirementPackageToDatabase(RequirementPackage requirementPackage);
        void SaveUpdateRequirementFieldEditable(RequirementFieldContract reqFieldContract, Int32 RequirementField_Id, Int32 currentLoggedInUserId);
        /// <summary>
        /// used to get requirement package details including package name,category name,item name and field name in hierarichal way
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementPackageDetailsContract> GetRequirementPackageDetailsByPackageID(Int32 requirementPackageID);

        RequirementObjectProperty GetReqObjectProperty(Int32 requirementFieldID, Int32 RequirementItemId, Int32 requirementCategoryID);
        List<RequirementPackageContract> GetRequirementPackages(Int32 selectedTenantID, String agencyID);

        List<RequirementPackageContract> GetInstructorRequirementPackages(Int32 selectedTenantID, String agencyId);

        /// <summary>
        /// used to get all requirement package details including package name and comma separated agencyNames with which they are mapped. It also returns unMapped packages too
        /// </summary>
        /// <returns></returns>
        List<RequirementPackageDetailsContract> GetRequirementPackageDetails(RequirementPackageDetailsContract requirementPackageDetailsContract, CustomPagingArgsContract customPagingArgsContract);

        /// <summary>
        /// get complete package details in hierarchal way
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementPackageHierarchicalDetailsContract> GetRequirementPackageHierarchalDetailsByPackageID(Int32 requirementPackageID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <param name="pkgObjectTypeId"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        List<RequirementObjectTreeContract> AddRequirementObjectTree(Int32 requirementPackageID, Int32 pkgObjectTypeId, Int32 currentUserID);

        Boolean IsPackageVersionNeedToCreate(Int32 requirementPackageID);

        //Boolean IsPackageMappedToRotation(Int32 requirementPackageID);

        RequirementPackage GetRequirementPackageByPackageID(Int32 requirementPackageID);

        List<RequirementObjectTree> GetRequirementObjectTreeList(List<Int32> reqObjectTreeIds);

        RequirementObjectTree GetRequirementObjectTree(Int32 reqObjectTreeId);

        Boolean SaveContextIntoDataBase();

        Boolean AddLargeContentToDatabase(List<LargeContent> largeContentList);

        Boolean AddLargeContentToContext(LargeContent largeContent);

        LargeContent GetLargeContentForReqrmntCategory(Int32 reqrmntCategoryID, Int32 objectTypeID, Int32 contentTypeID);

        //Dictionary<Int32, List<Int32>> GetTenantIDsMappedForAgencyUser(Guid userID);

        Int32 SaveRequirementPackageData(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID);

        #region UAT-1837:
        /// <summary>
        /// used to get requirement Item detail
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        RequirementItemContract GetRequirementItemDetailsByItemID(Int32 requirementItemID, Int32? requirementCategoryID = null);

        /// <summary>
        /// Method to use Save/Update requirement item detail.
        /// </summary>
        /// <returns></returns>
        Boolean SaveUpdateRequirementItemData(RequirementItemContract reqItemContract, Int32 currentLoggedInUserId);

        /// <summary>
        /// Method to get All requirement items of category
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementItemContract> GetRequirementItemsByCategoryID(Int32 requirementCatID);

        /// <summary>
        /// Method to delete Requirement category and item mapping
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        String DeleteReqCategoryItemMapping(Int32 requirementCatItemID, Int32 currentLoggedInUserId, Int32 reqPkgId, Boolean isNewPackage = false);

        /// <summary>
        ///Method to delete Requirement Item and Field mapping
        /// </summary>
        /// <returns></returns>
        String DeleteReqItemFieldMapping(Int32 requirementItemFieldID, String ItemHId, Int32 currentLoggedInUserId, Boolean isNewPackage = false, Int32 requirementCategoryID = 0);

        /// <summary>
        /// Method to get all requirement fields of Items
        /// </summary>
        /// <returns></returns>
        List<RequirementFieldContract> GetRequirementFieldsByItemID(Int32 requirementItemID);

        //UAT-3342
        List<RequirementFieldContract> IsCalculatedAttribute(Int32 requirementFieldID);

        /// <summary>
        /// Method to get requirement field
        /// </summary>
        /// <returns></returns>
        RequirementField GetRequirementFieldByFieldID(Int32 requirementFieldID);

        /// <summary>
        /// Method to get requirement field
        /// </summary>
        /// <returns></returns>
        List<UniversalField> GetUniversalFieldByAttributeDataTypeID(Int32 attributeDataTypeID);
        /// <summary>
        /// Method to Save/Update requirement field
        /// </summary>
        /// <returns></returns>
        Boolean SaveUpdateRequirementField(RequirementField reqField, Boolean isNewData, Boolean isNewPackage = false);


        #endregion

        RequirementCategoryContract GetRequirementCategoryDetailByCategoryID(int rqrmntCtgryID);

        List<RequirementCategoryContract> GetRequirementCategoriesByPackageID(int rqrmntPkgID);

        Boolean DeleteReqPackageCategoryMapping(Int32 reqPkgCtgryID, Int32 currentLoggedInUserId);

        Int32 SaveRequirementCategoryDetails(RequirementCategoryContract requirementCategoryContract, int currentLoggedInUserID);

        #region UAT-1837.
        List<RequirementTreeContract> GetRequirementTree(Int32 requirementPackageID);

        List<RequirementRuleContract> GetRequirementRuleDetail(Int32 requirementObjectTreeID);

        RequirementObjectTree GetExistingRequirementObjectTree(Int32 rotId);

        RequirementItem GetRequirementItemDetail(Int32 requirementItemID);

        RequirementObjectTree GetRequirementObjectTreeDetail(Int32 fieldId, Int32 parentObjectTreeId);
        #endregion

        RequirementObjectRule GetRequirementObjectRuleForObjectTreeID(Int32 ROT_ID);

        //UAT-1828
        List<Int32> GetRequirementPackageInstitution(Guid requirementPkgCode);

        //2305
        List<RequirementPackage> GetMasterRequirementPackages(Int32 rotPkgTypeID);

        RequirementObjectTree GetRequirementObjectTreeForField(Int32 fieldId, Int32 itemId);

        #region UAT-2213 Setup Tree
        DataTable GetRotationMappingTreeData(Int32 reqCategoryID);
        List<RequirementPackageContract> GetMasterRequirementPackageDetails(RequirementPackageContract requirementPackageDetailsContract, CustomPagingArgsContract customPagingArgsContract);
        List<RequirementObjectTreeContract> AddNewRequirementObjectTree(Int32 requirementCategoryID, Int32 catObjectTypeId, Int32 currentUserID);
        #endregion

        #region [UAT-2213]

        RequirementCategoryContract GetRequirementMasterCategoryDetailByCategoryID(int ReqCatID);

        Boolean IsMasterCategoryNameExists(string newCategoryName);

        List<RequirementPackageContract> GetAllMasterRequirementPackages();

        Int32 CreateCategoryCopy(CreateCategoryCopyContract createCategoryCopyContract);

        Int32 SaveMasterRotationCategory(RequirementCategoryContract requirementCategoryContract, int currentLoggedInUserID);

        List<RequirementCategoryContract> GetAllRequirmentCategories();

        List<Int32> GetMappedPackageIdsWithCategory(Int32 categoryID);

        List<Int32> GetMappedCategoriesWithPackage(Int32 packageID);

        String DeleteRequirementCategory(Int32 caregoryId);

        List<RequirementPackageContract> GetCategoryPackageMapping(CategoryPackageMappingContract categoryPackageMappingContract, CustomPagingArgsContract customPagingArgsContract);

        Boolean SaveCategoryPackageMapping(Int32 currentOrgUserID, Int32 requirementCategoryID, String requirementPackageIds);

        List<RequirementCategoryContract> GetMasterRequirementCategories(RequirementCategoryContract requirementCategoryDetailsContract, CustomPagingArgsContract customPagingArgsContract);

        List<RequirementCategoryContract> GetPackageCategoryMapping(PackageCategoryMappingContract packageCategoryMappingContract, CustomPagingArgsContract customPagingArgsContract);

        //UAT:4279

        Boolean UpdatePackageCategoryMappingDisplayOrder(List<RequirementCategoryContract> CategoryId, Int32? destinationIndex, Int32 currentUserId, Int32 requirementPkgId);

        Boolean SavePackageCategoryMapping(Int32 currentOrgUserID, Int32 requirementPackageID, String requirementCategoryIds, Boolean IsRotationPkgCopyFromAgencyHierarchy, Int32 ExistingPkgId);
        //Parameter requirementPkgVersioningStatus_DueId added for UAT-4657
        Int32 SaveMasterRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, Boolean IsRotationPkgCopyFromAgencyHierarchy, Int32 requirementPkgVersioningStatus_DueId);

        Boolean ArchivePackage(Dictionary<Int32, Boolean> aryPackageIds, Int32 ArchivePackage);

        //UAT-4054
        Boolean UnArchivePackage(Dictionary<Int32, Boolean> aryPackageIds, Int32 ArchivePackage);

        List<Int32> GetMappedPackageDetails(Int32 reqCatID);
        #endregion


        #region UAT-2514 Copy Package

        /// <summary>
        /// Get Package Hierarchial Details of New Package that need to be Copied in Tenant
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementPackageHierarchicalDetailsContract> GetRequirementPackageHierarchalDetailsByPackageIDNew(Int32 requirementPackageID);

        /// <summary>
        /// UAT : 4526 Get Agency data
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementAgencyData> GetRequirementAgencyData(Int32 requirementPackageID);

        #endregion

        #region UAT-2514:
        String GetRequirementPackageObjectIdsToSync(Int32 tenantId, Int32 ChunkSize, Int32 retryTimeLag);
        Boolean SaveRequirementPackageObjectForSync(String requestDataXML, Int32 currentLoggedInUserId, out Int32 syncReqPkgObjID);
        #endregion

        #region Field Rule
        Int32 GetRequirementObjectTreeIDByprntID(String HID);
        Boolean saveRequirmentFieldRuleDetails();
        List<RequirementObjectTreeProperty> GetRequirementObjectTreePropertyByID(Int32 requirementObjectTreeID);
        List<Int32> GetRequirementObjectTreeIDByReqFieldID(Int32 reqFieldID);
        #endregion

        #region Check Rot. Eff. Start Date & EndDate
        RequirementPackageContract CheckRotEffectiveDate(Int32 reqPackageID);
        #endregion

        #region Category Tree Entry
        List<Int32> GetCategoryIdsForAssignedField(Int32 requrirementFieldID);
        #endregion

        Boolean ScheduleAutoArchivalRequirementPackage(Int32 currentLoggedInUserId); //2533

        //UAT-2533        
        List<RequirementPackageContract> GetRequirementPackageDetail(String RequiremntRotPackageIDs);
        Boolean BulkPackageCopy(String BulkCopyPackgXML, Int32 CurrentLoggedInUserId);

        //UAT-2647
        List<Int32> GetAgencyHierarchyIdsWithPkgId(Int32 reqPackageID);

        List<Int32> GetAgencyHierarchyIdsByRequirementPackageID(Int32 requirementPackageID);

        //Save Update AgencyHirarchyPackage data
        void SaveUpdateAgencyHierarchyPackage(List<Int32> agencyHierarchyIds, RequirementPackage reqPackage, Int32 currentLoggedInUserId);

        #region UAT-2706
        Dictionary<Int32, String> GetRequirementCategoryDataBypackageId(Int32 ReqPackageId);
        List<RequirementItem> GetRequirementItemByCategoryId(Int32 ReqCategoryId);
        List<RequirementPackageContract> GetRequirementPackagesByHierarcyIds(List<Int32> lstAgencyHierarchyIds, Int32 currentLoggedInUserId, CustomPagingArgsContract customPagingContract);
        //UAT-2795
        String GetCategoryDocumentLink(Int32 reqCategoryId);
        #endregion

        #region UAt-2973
        List<RequirementPackageContract> GetRequirementPackagesFromAgencyIds(Int32 selectedTenantId, String agencyId, String reqPkgTypeCode = null);
        #endregion

        #region Requirement Verification Assignment Queue AND User Work Queue
        List<RequirementVerificationQueueContract> GetAssignmentRotationVerificationQueueData(RequirementVerificationQueueContract requirementVerificationQueueContract, CustomPagingArgsContract customPagingArgsContract);

        List<ReqPkgSubscriptionIDList> GetReqPkgSubscriptionIdList(RequirementVerificationQueueContract requirementVerificationQueueContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID);

        Boolean AssignItemsToUser(List<Int32> lstSelectedVerificationItems, Int32 VerSelectedUserId, String verSelectedUserName);
        #endregion

        #region UAT 3052
        List<Agency> GetAgencyHierarchy(String loggedInEmailId);
        List<Agency> GetAgencyUsers(String loggedInEmailId);
        #endregion

        #region UAT-3078
        Boolean updateRequirementItemDisplayOrder(Int32 RequirementItemId, Int32 RequirementCategoryId, Int32 DisplayOrder, Int32 CurrentLoggedInUserID, Boolean isNewPackage);
        Boolean updateRequirementFieldDisplayOrder(Int32 RequirementFieldId, Int32 RequirementItemId, Int32 DisplayOrder, Int32 CurrentLoggedInUserID, Boolean isNewPackage);
        #endregion

        RequirementPackage GetRequirementPackageByCode(Guid rpCode);

        Dictionary<String, String> GetRotationListFilterForLoggedInAgencyUserReports(String SelectedTenantIDs, String loggedInUserEmailId); //UAT -3146

        #region UAT-3112

        List<Int32> GetSystemDocumentsMapped(Int32 ItemID);
        List<BadgeFormSystemDocField> GetSystemDocFieldsMapped(Int32 SystemDocId);

        #endregion


        #region UAT-3176
        Boolean SaveUpdateRotationAttributeGroup(RequirementAttributeGroupContract rotationAttributeGroupContract, Boolean IsAttributeGroupExists, Int32 currentLoggedInUserID);

        List<RequirementAttributeGroupContract> GetAllRotationAttributeGroup(String attributeName, String attributeLabel);
        RequirementAttributeGroupContract GetAttributeGroupById(Int32 rotationAttributeGroupId);

        Boolean IsAttributeGroupMapped(Int32 requirementAttributeGroupId);

        List<RequirementAttributeGroups> GetRequirementAttributeGroups();
        #endregion

        #region UAT-3230
        String GetPendingRequirementPackageObjectIdsForTenant(Int32 tenantId, Int32 chunkSize, Int32 retryCount);
        void UpdateSyncRequirementPackageObjectsCount(Int32 currentUserId, String SyncReqPkgObjectIds);
        void RemoveSyncRequirementPackageObjects(Int32 currentUserId, String SyncReqPkgObjectIds);
        #endregion
        #region UAt-3220
        Boolean HideRequirementSharesDetailLink(Guid userID);
        #endregion

        Boolean CloneRequirementItem(Int32 sourceReqItemID, Int32 currentLoggedInUserId, Int32 reqCatID);

        //UAT-3296
        String GetCategoryExplanatoryNotes(Int32 reqCategoryId);
        //UAT-3295
        ProfileSharingInvitationDetailsContract GetProfileShareDetailsById(Int32 invitationId);

        Boolean UpdateProfileShareInvDetails(ProfileSharingInvitationDetailsContract clinicalRotationpackage, Int32 currentLoggedInUser);

        //UAT-3494 
        Boolean InsertRequirementPackageVersioningData(Int32 currentOrgUserID, Int32 OldPackageID, Int32 NewPackageID, String lstSelectedAgencyIds);

        #region UAT-4254 || Release - 181

        List<RequirementCategoryDocLink> GetRequirementCatDocUrls(Int32 reqCatId);

        #endregion

        #region UAT-4657
        Dictionary<Int32, String> GetPackagesAssociatedWithCategory(Int32 categoryId);
        Boolean SaveCategoryDiassociationDetail(Int32 categoryId, String packageIds, Int32 currentLoggedInUserId);

        void ManageRequirementVersionTenantMapping(Int32 currentOrgUserID);
        List<RequirementPkgVersionTenantMapping> GetRequirementPkgVersionTenantMapping(Int32 requirementPkgVersioningStatus_DueId);

        List<RequirementCategoryDisassociationTenantMapping> GetRequirementCategoryDisassociationTenantMappingForDisassociation();

        Boolean UpdateRequirementPkgVersioningStatusInRequirementPackage(Int32 currentLoggedInUserId, Int32 requirementPkgVersioningStatus_DueId, Int32 requirementPkgVersioningStatus_InProgressId, Int32 requirementPkgVersioningStatus_CompletedId);

       // List<Int32> GetPendingTenantsWhichPkgVersioningOrCategoryDisassociationPending(Int32 requirementPkgVersioningStatus_NoRotationId, Int32 requirementPkgVersioningStatus_CompletedId);

        String IsCategoryDisassociationInProgress(Int32 requirementCategoryId, List<Int32> selectedPkgIds, Int32 requirementPkgVersioningStatus_DueId, Int32 requirementPkgVersioningStatus_InProgressId);

        List<RequirementPackageContract> GetAllPkgVersionsByPkgId(Int32 PkgId);

        void UpdateRequirementCategoryDisassociationStatus(Int32 backgroundProcessUserId);
        List<RequirementPackage> GetMasterRequirementPackages();

        Boolean IsSyncAlreadyInProgress(Int32 objectId, Boolean IsObjectTypePackage, List<lkpObjectType> lkpObjectTypes);
        #endregion
    }
}

