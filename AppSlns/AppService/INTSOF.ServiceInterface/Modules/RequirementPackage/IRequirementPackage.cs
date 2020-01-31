using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;


namespace INTSOF.ServiceInterface.Modules.RequirementPackage
{
    [ServiceContract]
    public interface IRequirementPackage
    {
        /// <summary>
        /// method used to return lkpRequirementFieldDataType values
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<RotationFieldDataTypeContract>> GetRotationFieldDataTypes(ServiceRequest<Int32, Boolean> tenantParameter);

        /// <summary>
        /// method used to add entries in requirement package,category,item,field and all mapping tables
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<Int32> SaveRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters);

        [OperationContract]
        ServiceResponse<Int32> SaveRequirementPackageData(ServiceRequest<RequirementPackageContract, Int32> requirementPackageParameters);

        /// <summary>
        /// method used to delete entries in requirement package,category,item,field and all mapping tables
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<Int32> DeleteRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters);

        /// <summary>
        /// method used to return a single package details including package name,category name and item,field name based on reuirementPackageID
        /// </summary>
        /// <param name="requirementPackageDetailsParameters"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<RequirementPackageDetailsContract>> GetRequirementPackageDetailsByPackageID(ServiceRequest<Int32, Int32> requirementPackageDetailsParameters);

        /// <summary>
        /// get complete package details in hierarchal way
        /// </summary>
        /// <param name="requirementPackageDetailsParameters"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<RequirementPackageContract> GetRequirementPackageHierarchalDetailsByPackageID(ServiceRequest<Int32, Int32, Boolean> requirementPackageDetailsParameters);



        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetRequirementPackages(ServiceRequest<String, Boolean> data);


        /// <summary>
        /// used to get all requirement package details including package name and comma separated agencyNames with which they are mapped. It also returns unMapped packages too
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<RequirementPackageDetailsContract>> GetRequirementPackageDetails(ServiceRequest<RequirementPackageDetailsContract, CustomPagingArgsContract> requirementPackageDetailsParameters);

        /// <summary>
        /// method used to return lkpConstantType values
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<RulesConstantTypeContract>> GetRulesConstantTypes(ServiceRequest<Int32, Boolean> parameters);


        #region UAT 1352 As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use.
        [OperationContract]
        ServiceResponse<List<RequirementPackageTypeContract>> GetRequirementPackageType(ServiceRequest<Int32, Boolean> parameter);
        #endregion

        //[OperationContract]
        //ServiceResponse<Dictionary<Int32, List<Int32>>> GetTenantIDsMappedForAgencyUser(ServiceRequest<Guid> data);


        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetInstructorRequirementPackages(ServiceRequest<String, Boolean> data);

        [OperationContract]
        ServiceResponse<Int32> CopySharedRqrmntPkgToClient(ServiceRequest<Int32, Int32> requirementPackageParameters);

        [OperationContract]
        ServiceResponse<Int32> CopyClientRqrmntPkgToShared(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters);

        [OperationContract]
        ServiceResponse<RequirementPackageContract> GetRequirementPackageDataByID(ServiceRequest<Int32> requirementPackageParameters);

        [OperationContract]
        ServiceResponse<Int32> CreateMasterPackageVersion(ServiceRequest<RequirementPackageContract, int> versionParameters);

        #region UAT-1837:ADB Admin streamlined create and edit rotation packages
        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateRequirementItemData(ServiceRequest<RequirementItemContract> parameters);

        [OperationContract]
        ServiceResponse<RequirementItemContract> GetRequirementItemDetail(ServiceRequest<Int32, Int32,Int32?> parameters);
        [OperationContract]
        ServiceResponse<List<RequirementItemContract>> GetRequirementItemsByCategoryID(ServiceRequest<Int32> parameters);
        [OperationContract]
        ServiceResponse<String> DeleteReqCategoryItemMapping(ServiceRequest<Int32, Int32, Boolean> parameters);
        [OperationContract]
        ServiceResponse<List<RequirementFieldContract>> GetRequirementFieldsByItemID(ServiceRequest<Int32> parameters);
        //UAT-3342
        [OperationContract]
        ServiceResponse<List<RequirementFieldContract>> IsCalculatedAttribute(ServiceRequest<Int32> parameters);
        [OperationContract]
        ServiceResponse<String> DeleteReqItemFieldMapping(ServiceRequest<Int32, String, Boolean, Int32> parameters);
        #endregion

        [OperationContract]
        ServiceResponse<Boolean> IsRequirementPackageUsed(ServiceRequest<int> serviceRequest);

        [OperationContract]
        ServiceResponse<RequirementCategoryContract> GetRequirementCategoryDetail(ServiceRequest<int, int> serviceRequest);

        [OperationContract]
        ServiceResponse<List<RequirementCategoryContract>> GetRequirementCategoriesByPackageID(ServiceRequest<int> parameters);

        [OperationContract]
        ServiceResponse<Boolean> DeleteReqPackageCategoryMapping(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> SaveRequirementCategoryDetails(ServiceRequest<RequirementCategoryContract> serviceRequest);

        /// <summary>
        /// method to save/Update requirement Field Data and mapped with Item in 'RequirementItemField.
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<Int32> SaveUpdateRequirementFieldData(ServiceRequest<RequirementFieldContract> parameters);

        /// <summary>
        /// method to get requirement field Data.
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<RequirementFieldContract> GetRequirementFieldDataByID(ServiceRequest<Int32,Int32> parameters);

        #region UAT-1837.
        [OperationContract]
        ServiceResponse<List<RequirementTreeContract>> GetRequirementTree(ServiceRequest<Int32> parameters);

        [OperationContract]
        ServiceResponse<List<RequirementRuleContract>> GetRequirementRuleDetail(ServiceRequest<Int32> parameters);

        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateRequirementRule(ServiceRequest<List<RequirementRuleContract>> parameters);
        #endregion

        [OperationContract]
        ServiceResponse<RequirementPackageContract> GetRequirementPackageHierarchalDetailsByPackageIDForVersioning(ServiceRequest<Int32, Int32, Boolean> requirementPackageDetailsParameters);

        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetAllRequirementPackages(ServiceRequest<int> serviceRequest);

        [OperationContract]
        ServiceResponse<RequirementPackageCompletionContract> CheckRequirementPackageCompletionStatus(ServiceRequest<int> serviceRequest);

        #region UAT-2305

        [OperationContract]
        ServiceResponse<List<UniversalCategoryContract>> GetUniversalCategorys();

        [OperationContract]
        ServiceResponse<UniversalCategoryContract> GetUniversalCategoryByReqCatID(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> DeleteUnversalCategoryMappings(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<UniversalItemContract>> GetUniversalItemsByUniReqCatID(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<UniversalItemContract> GetUniversalItemsByReqCatItmID(ServiceRequest<Int32, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> DeleteUniversalReqItmMapping(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<UniversalAttributeContract>> GetUniversalAttributes(ServiceRequest<Int32, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<UniversalAttributeContract> GetUniversalattributeDetails(ServiceRequest<Int32, Int32, Int32> serviceRequest);
        [OperationContract]
        ServiceResponse<UniversalAttributeContract> GetUniversalFieldAttributeDetails(ServiceRequest<Int32, Int32, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> GetAtrInputPriorityByID(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateAttributeInputPriority(ServiceRequest<UniversalAttributeContract> serviceRequest);

        [OperationContract]
        ServiceResponse<Dictionary<Int32, String>> GetUniversalAtrOptionData(ServiceRequest<Int32> serviceRequest);
        [OperationContract]
        ServiceResponse<Dictionary<Int32, String>> GetUniversalFieldAtrOptionData(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> GetUniversalFieldAtrInputPriorityByID(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<Int32>> GetUniversalAtrOptionSelected(ServiceRequest<Int32, Int32> serviceRequest);
        [OperationContract]
        ServiceResponse<List<Int32>> GetUniversalFieldAtrOptionSelected(ServiceRequest<Int32, Int32> serviceRequest);
        #endregion

        [OperationContract]
        ServiceResponse<List<DefinedRequirementContract>> GetDefinedRequirement(ServiceRequest<Int32, Boolean> parameters);

        #region UAT-2213

        [OperationContract]
        ServiceResponse<List<RotationMappingContract>> GetRotationMappingTreeData(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetMasterRequirementPackageDetails(ServiceRequest<RequirementPackageContract, CustomPagingArgsContract> requirementPackageDetailsParameters);

        #endregion

        #region [UAT-2213]

        [OperationContract]
        ServiceResponse<RequirementCategoryContract> GetRequirementMasterCategoryDetailByCategoryID(ServiceRequest<int> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> IsMasterCategoryNameExists(ServiceRequest<String> serviceRequest);

        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetAllMasterRequirementPackages();

        [OperationContract]
        ServiceResponse<Int32> CreateCategoryCopy(ServiceRequest<CreateCategoryCopyContract> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> SaveMasterRotationCategory(ServiceRequest<RequirementCategoryContract> serviceRequest);


        [OperationContract]
        ServiceResponse<List<RequirementCategoryContract>> GetRequirementCategories();

        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetCategoryPackageMapping(ServiceRequest<CategoryPackageMappingContract, CustomPagingArgsContract> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> SaveCategoryPackageMapping(ServiceRequest<Int32, Int32, String> serviceRequest);

        [OperationContract]
        ServiceResponse<List<RequirementCategoryContract>> GetMasterRequirementCategories(ServiceRequest<RequirementCategoryContract, CustomPagingArgsContract> requirementCategoryDetailsParameters);

        [OperationContract]
        ServiceResponse<String> DeleteRequirementCategory(ServiceRequest<int> serviceRequest);

        [OperationContract]
        ServiceResponse<List<Int32>> GetMappedPackageIdsWithCategory(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<Int32>> GetMappedCategoriesWithPackage(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<RequirementCategoryContract>> GetPackageCategoryMapping(ServiceRequest<PackageCategoryMappingContract, CustomPagingArgsContract> serviceRequest);

        //UAT:4279
        [OperationContract]
        ServiceResponse<Boolean> UpdatePackageCategoryMappingDisplayOrder(ServiceRequest<List<RequirementCategoryContract>, Int32?, Int32, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> SavePackageCategoryMapping(ServiceRequest<Int32, Int32, String> serviceRequest);

        [OperationContract]
        ServiceResponse<int> SaveMasterRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Boolean> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> ArchivePackage(ServiceRequest<Dictionary<Int32, Boolean>, Int32> serviceRequest);

        //UAT-4054
        [OperationContract]
        ServiceResponse<Boolean> UnArchivePackage(ServiceRequest<Dictionary<Int32, Boolean>, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<Int32>> GetMappedPackageDetails(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Int32> GetRequirementObjectTreeIDByprntID(ServiceRequest<String> serviceRequest);
        [OperationContract]
        ServiceResponse<List<RequirementRuleContract>> GetReqFixedRuleDetailByObjectTreeID(ServiceRequest<Int32> serviceRequest);
        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateNewRequirementFieldRule(ServiceRequest<List<RequirementRuleContract>, Int32, Int32> serviceRequest);
        [OperationContract]
        ServiceResponse<List<Int32>> GetRequirementObjectTreeIDByReqFieldID(ServiceRequest<Int32> serviceRequest);
        #endregion

        #region UAT-2514 Copy Package
        /// <summary>
        /// Copy Shared Package to Tenant
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>

        [OperationContract]
        ServiceResponse<Int32> CopySharedRequirementPackageToClientNew(ServiceRequest<Int32, Int32> requirementPackageParameters);

        #endregion

        #region Check Rot. Eff. Start Date & EndDate
        [OperationContract]
        ServiceResponse<List<Int32>, List<String>> CheckRotEffectiveDate(ServiceRequest<Int32, String, Int32> requirementPackageParameters);

        [OperationContract]
        ServiceResponse<ClinicalRotationDetailContract> GetRotationDetail(ServiceRequest<Int32, Int32> rotationParameters);

        #endregion

        //UAT-2533
        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetRequirementPackageDetail(ServiceRequest<String> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> BulkPackageCopy(ServiceRequest<String, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<Int32>> GetAgencyHierarchyIdsByRequirementPackageID(ServiceRequest<Int32> requirementPackageParameters);

        [OperationContract]
        ServiceResponse<List<RequirementCategoryContract>> GetRotationPackageCategoryDetailByRotationID(ServiceRequest<Int32, Int32, Boolean> serviceRequest);

        #region UAT-2706
        [OperationContract]
        ServiceResponse<List<RequirementItemContract>> GetRequirementItemDetailsByCategoryId(ServiceRequest<Int32> serviceRequest);
        [OperationContract]
        ServiceResponse<Dictionary<Int32, String>> GetSharedRequirementCategoryData(ServiceRequest<Int32> serviceRequest);
        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetRequirementPackagesByHierarcyIds(ServiceRequest<List<Int32>, Int32, CustomPagingArgsContract> serviceRequest);
        //UAT-2795
        [OperationContract]
        ServiceResponse<String> GetCategoryDocumentLink(ServiceRequest<Int32> serviceRequest);
        #endregion

        #region UAT-2788
        [OperationContract]
        ServiceResponse<List<RequirementFieldType>> GetAttributeType(ServiceRequest<Int32> tenantParameter);
        #endregion

        #region UAT-3078
        [OperationContract]
        ServiceResponse<Boolean> updateRequirementItemDisplayOrder(ServiceRequest<Int32, Int32, Int32, Int32> parameters, Boolean isNewPackage);
        [OperationContract]
        ServiceResponse<Boolean> updateRequirementFieldDisplayOrder(ServiceRequest<Int32, Int32, Int32, Int32> parameters, Boolean isNewPackage);
        #endregion

        #region UAT-3176
        [OperationContract]
        ServiceResponse<List<RequirementAttributeGroups>> GetRequirementAttributeGroups(ServiceRequest<Int32> serviceRequest);
        #endregion

        [OperationContract]
        ServiceResponse<List<RequirementReviewByContract>> GetRequirementReviewBy(ServiceRequest<Int32, Boolean> parameters);

        [OperationContract]
        ServiceResponse<Boolean> CloneRequirementItem(ServiceRequest<Int32, Int32, Int32> parameters);
        //UAT-3296 
        [OperationContract]
        ServiceResponse<String> GetCategoryExplanatoryNotes(ServiceRequest<Int32> serviceRequest);
        //4001
        [OperationContract]
        ServiceResponse<List<RequirementDocumentAcroFieldType>> GetDocumentAcroFieldTypeData(ServiceRequest<Int32> serviceRequest);

        #region UAT-4254 || Release- 181

        [OperationContract]
        ServiceResponse<List<RequirementCategoryDocUrl>> GetRequirementCatDocUrls(ServiceRequest<Int32> parameters);
        #endregion
        #region UAT-4657
        ServiceResponse<Dictionary<Int32, String>> GetPackagesAssociatedWithCategory(ServiceRequest<Int32> parameters);
        ServiceResponse<Boolean> SaveCategoryDiassociationDetail(ServiceRequest<Int32,String> parameters);
        ServiceResponse<String> IsCategoryDisassociationInProgress(ServiceRequest<Int32, List<Int32>> serviceRequest);
        ServiceResponse<Boolean> IsSyncAlreadyInProgress(ServiceRequest<Int32, Boolean> serviceRequest);
        #endregion
    }
}

