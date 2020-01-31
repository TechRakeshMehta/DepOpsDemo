using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceInterface.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Core;
using INTSOF.Utils.Enums;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;


namespace INTSOF.ServiceProxy.Modules.RequirementPackage
{
    public class RequirementPackageProxy : BaseServiceProxy<IRequirementPackage>
    {
        IRequirementPackage _requirementPackageServiceChannel;

        public RequirementPackageProxy()
            : base(ServiceUrlEnum.RequirementPackageSvcUrl.GetStringValue())
        {
            _requirementPackageServiceChannel = base.ServiceChannel;
        }

        /// <summary>
        /// method used to return lkpRequirementFieldDataType values
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        public ServiceResponse<List<RotationFieldDataTypeContract>> GetRotationFieldDataTypes(ServiceRequest<Int32, Boolean> parameters)
        {
            return _requirementPackageServiceChannel.GetRotationFieldDataTypes(parameters);
        }

        /// <summary>
        /// method used to add entries in requirement package,category,item,field and all mapping tables
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>
        public ServiceResponse<Int32> SaveRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.SaveRequirementPackage(requirementPackageParameters);
        }

        /// <summary>
        /// method used to delete entries in requirement package,category,item,field and all mapping tables
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>
        public ServiceResponse<Int32> DeleteRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.DeleteRequirementPackage(requirementPackageParameters);
        }

        /// <summary>
        /// method used to return a single package details including package name,category name and item,field name based on reuirementPackageID
        /// </summary>
        /// <param name="requirementPackageDetailsParameters"></param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementPackageDetailsContract>> GetRequirementPackageDetailsByPackageID(ServiceRequest<Int32, Int32> requirementPackageDetailsParameters)
        {
            return _requirementPackageServiceChannel.GetRequirementPackageDetailsByPackageID(requirementPackageDetailsParameters);
        }

        /// <summary>
        ///  get complete package details in hierarchal way
        /// </summary>
        /// <param name="requirementPackageDetailsParameters"></param>
        /// <returns></returns>
        public ServiceResponse<RequirementPackageContract> GetRequirementPackageHierarchalDetailsByPackageID(ServiceRequest<Int32, Int32, Boolean> requirementPackageDetailsParameters)
        {
            return _requirementPackageServiceChannel.GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageDetailsParameters);
        }

        /// <summary>
        /// used to get all requirement package details including package name and comma separated agencyNames with which they are mapped. It also returns unMapped packages too
        /// </summary>
        /// <returns></returns>
        public ServiceResponse<List<RequirementPackageDetailsContract>> GetRequirementPackageDetails(ServiceRequest<RequirementPackageDetailsContract, CustomPagingArgsContract> requirementPackageDetailsParameters)
        {
            return _requirementPackageServiceChannel.GetRequirementPackageDetails(requirementPackageDetailsParameters);
        }

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="data">AgencyId</param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementPackageContract>> GetRequirementPackages(ServiceRequest<String, Boolean> data)
        {
            return _requirementPackageServiceChannel.GetRequirementPackages(data);
        }

        /// <summary>
        /// method used to return lkpConstantType values
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        public ServiceResponse<List<RulesConstantTypeContract>> GetRulesConstantTypes(ServiceRequest<Int32, Boolean> parameters)
        {
            return _requirementPackageServiceChannel.GetRulesConstantTypes(parameters);
        }

        #region UAT 1352 As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use.
        public ServiceResponse<List<RequirementPackageTypeContract>> GetRequirementPackageType(ServiceRequest<Int32, Boolean> parameter)
        {
            return _requirementPackageServiceChannel.GetRequirementPackageType(parameter);
        }
        #endregion

        //public ServiceResponse<Dictionary<Int32,List<Int32>>> GetTenantIDsMappedForAgencyUser(ServiceRequest<Guid> data)
        //{
        //    return _requirementPackageServiceChannel.GetTenantIDsMappedForAgencyUser(data);
        //}

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="data">AgencyId</param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementPackageContract>> GetInstructorRequirementPackages(ServiceRequest<String, Boolean> data)
        {
            return _requirementPackageServiceChannel.GetInstructorRequirementPackages(data);
        }


        public ServiceResponse<Int32> CopySharedRqrmntPkgToClient(ServiceRequest<Int32, Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.CopySharedRqrmntPkgToClient(requirementPackageParameters);
        }

        public ServiceResponse<Int32> CopyClientRqrmntPkgToShared(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.CopyClientRqrmntPkgToShared(requirementPackageParameters);
        }

        public ServiceResponse<RequirementPackageContract> GetRequirementPackageDataByID(ServiceRequest<Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.GetRequirementPackageDataByID(requirementPackageParameters);
        }

        /// <summary>
        /// method used to add entries in requirement package,category,item,field and all mapping tables
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>
        public ServiceResponse<Int32> SaveRequirementPackageData(ServiceRequest<RequirementPackageContract, Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.SaveRequirementPackageData(requirementPackageParameters);
        }

        public ServiceResponse<Int32> CreateMasterPackageVersion(ServiceRequest<Int32, Int32> serviceRequest)
        {
            ServiceRequest<Int32, Int32, Boolean> requirementPackageDetailsParameters = new ServiceRequest<Int32, Int32, Boolean>();
            requirementPackageDetailsParameters.Parameter1 = serviceRequest.Parameter1;
            requirementPackageDetailsParameters.Parameter2 = 0;
            requirementPackageDetailsParameters.Parameter3 = true;
            RequirementPackageContract requirementPackageContractData = _requirementPackageServiceChannel
                                            .GetRequirementPackageHierarchalDetailsByPackageIDForVersioning(requirementPackageDetailsParameters).Result;

            ServiceRequest<RequirementPackageContract, Int32> versionParameters = new ServiceRequest<RequirementPackageContract, Int32>();
            versionParameters.Parameter1 = requirementPackageContractData;
            versionParameters.Parameter2 = versionParameters.Parameter2;
            return _requirementPackageServiceChannel.CreateMasterPackageVersion(versionParameters);
        }

        #region UAT-1837:ADB Admin streamlined create and edit rotation packages
        /// <summary>
        /// method to save/Update requirement item Data and mapped with category in 'RequirementCategoryItem.
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        public ServiceResponse<Boolean> SaveUpdateRequirementItemData(ServiceRequest<RequirementItemContract> parameters)
        {
            return _requirementPackageServiceChannel.SaveUpdateRequirementItemData(parameters);
        }

        /// <summary>
        /// method to get requirement item detail.
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        public ServiceResponse<RequirementItemContract> GetRequirementItemDetail(ServiceRequest<Int32, Int32,Int32?> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementItemDetail(parameters);
        }

        public ServiceResponse<List<RequirementItemContract>> GetRequirementItemsByCategoryID(ServiceRequest<Int32> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementItemsByCategoryID(parameters);
        }

        public ServiceResponse<String> DeleteReqCategoryItemMapping(ServiceRequest<Int32, Int32, Boolean> parameters)
        {
            return _requirementPackageServiceChannel.DeleteReqCategoryItemMapping(parameters);
        }

        public ServiceResponse<String> DeleteReqItemFieldMapping(ServiceRequest<Int32, String, Boolean, Int32> parameters)
        {
            return _requirementPackageServiceChannel.DeleteReqItemFieldMapping(parameters);
        }

        public ServiceResponse<List<RequirementFieldContract>> GetRequirementFieldsByItemID(ServiceRequest<Int32> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementFieldsByItemID(parameters);
        }

        //UAT-3342
        public ServiceResponse<List<RequirementFieldContract>> IsCalculatedAttribute(ServiceRequest<Int32> parameters)
        {
            return _requirementPackageServiceChannel.IsCalculatedAttribute(parameters);
        }


        /// <summary>
        /// method to save/Update requirement Field Data and mapped with Item in 'RequirementItemField.
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        public ServiceResponse<Int32> SaveUpdateRequirementFieldData(ServiceRequest<RequirementFieldContract> parameters)
        {
            return _requirementPackageServiceChannel.SaveUpdateRequirementFieldData(parameters);
        }

        /// <summary>
        /// method to get requirement field Data.
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        public ServiceResponse<RequirementFieldContract> GetRequirementFieldDataByID(ServiceRequest<Int32,Int32> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementFieldDataByID(parameters);
        }
        #endregion

        public ServiceResponse<Boolean> IsRequirementPackageUsed(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.IsRequirementPackageUsed(serviceRequest);
        }

        public ServiceResponse<RequirementCategoryContract> GetRequirementCategoryDetail(ServiceRequest<int, int> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRequirementCategoryDetail(serviceRequest);
        }

        public ServiceResponse<List<RequirementCategoryContract>> GetRequirementCategoriesByPackageID(ServiceRequest<int> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementCategoriesByPackageID(parameters);
        }

        public ServiceResponse<Boolean> DeleteReqPackageCategoryMapping(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.DeleteReqPackageCategoryMapping(serviceRequest);
        }

        public ServiceResponse<Boolean> SaveRequirementCategoryDetails(ServiceRequest<RequirementCategoryContract> serviceRequest)
        {
            return _requirementPackageServiceChannel.SaveRequirementCategoryDetails(serviceRequest);
        }

        #region UAT-1837.
        public ServiceResponse<List<RequirementTreeContract>> GetRequirementTree(ServiceRequest<Int32> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementTree(parameters);
        }

        public ServiceResponse<List<RequirementRuleContract>> GetRequirementRuleDetail(ServiceRequest<Int32> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementRuleDetail(parameters);
        }

        public ServiceResponse<Boolean> SaveUpdateRequirementRule(ServiceRequest<List<RequirementRuleContract>> parameters)
        {
            return _requirementPackageServiceChannel.SaveUpdateRequirementRule(parameters);
        }
        #endregion

        public ServiceResponse<List<RequirementPackageContract>> GetAllRequirementPackages(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetAllRequirementPackages(serviceRequest);
        }


        public ServiceResponse<RequirementPackageCompletionContract> CheckRequirementPackageCompletionStatus(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.CheckRequirementPackageCompletionStatus(serviceRequest);
        }

        #region UAT-2305

        public ServiceResponse<List<UniversalCategoryContract>> GetUniversalCategorys()
        {
            return _requirementPackageServiceChannel.GetUniversalCategorys();
        }

        public ServiceResponse<UniversalCategoryContract> GetUniversalCategoryByReqCatID(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalCategoryByReqCatID(serviceRequest);
        }

        public ServiceResponse<Boolean> DeleteUnversalCategoryMappings(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.DeleteUnversalCategoryMappings(serviceRequest);
        }

        public ServiceResponse<List<UniversalItemContract>> GetUniversalItemsByUniReqCatID(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalItemsByUniReqCatID(serviceRequest);
        }

        public ServiceResponse<UniversalItemContract> GetUniversalItemsByReqCatItmID(ServiceRequest<Int32, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalItemsByReqCatItmID(serviceRequest);
        }

        public ServiceResponse<Boolean> DeleteUniversalReqItmMapping(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.DeleteUniversalReqItmMapping(serviceRequest);
        }

        public ServiceResponse<List<UniversalAttributeContract>> GetUniversalAttributes(ServiceRequest<Int32, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalAttributes(serviceRequest);
        }
        public ServiceResponse<UniversalAttributeContract> GetUniversalattributeDetails(ServiceRequest<Int32, Int32, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalattributeDetails(serviceRequest);
        }

        public ServiceResponse<UniversalAttributeContract> GetUniversalFieldAttributeDetails(ServiceRequest<Int32, Int32, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalFieldAttributeDetails(serviceRequest);
        }
        public ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> GetAtrInputPriorityByID(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetAtrInputPriorityByID(serviceRequest);
        }
        public ServiceResponse<Boolean> SaveUpdateAttributeInputPriority(ServiceRequest<UniversalAttributeContract> serviceRequest)
        {
            return _requirementPackageServiceChannel.SaveUpdateAttributeInputPriority(serviceRequest);
        }
        public ServiceResponse<Dictionary<Int32, String>> GetUniversalAtrOptionData(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalAtrOptionData(serviceRequest);
        }
        public ServiceResponse<Dictionary<Int32, String>> GetUniversalFieldAtrOptionData(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalFieldAtrOptionData(serviceRequest);
        }
        public ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> GetUniversalFieldAtrInputPriorityByID(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalFieldAtrInputPriorityByID(serviceRequest);
        }
        public ServiceResponse<List<Int32>> GetUniversalAtrOptionSelected(ServiceRequest<Int32, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalAtrOptionSelected(serviceRequest);
        }
        public ServiceResponse<List<Int32>> GetUniversalFieldAtrOptionSelected(ServiceRequest<Int32, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetUniversalFieldAtrOptionSelected(serviceRequest);
        }

        #endregion

        public ServiceResponse<List<DefinedRequirementContract>> GetDefinedRequirement(ServiceRequest<Int32, Boolean> parameter)
        {
            return _requirementPackageServiceChannel.GetDefinedRequirement(parameter);
        }

        #region UAT-2213
        public ServiceResponse<List<RotationMappingContract>> GetRotationMappingTreeData(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRotationMappingTreeData(serviceRequest);
        }
        public ServiceResponse<List<RequirementPackageContract>> GetMasterRequirementPackageDetails(ServiceRequest<RequirementPackageContract, CustomPagingArgsContract> requirementPackageDetailsParameters)
        {
            return _requirementPackageServiceChannel.GetMasterRequirementPackageDetails(requirementPackageDetailsParameters);
        }
        #endregion

        #region [UAT-2213]

        public ServiceResponse<RequirementCategoryContract> GetRequirementMasterCategoryDetailByCategoryID(ServiceRequest<int> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRequirementMasterCategoryDetailByCategoryID(serviceRequest);
        }

        public ServiceResponse<Boolean> SaveMasterRotationCategory(ServiceRequest<RequirementCategoryContract> serviceRequest)
        {
            return _requirementPackageServiceChannel.SaveMasterRotationCategory(serviceRequest);
        }

        public ServiceResponse<Boolean> IsMasterCategoryNameExists(ServiceRequest<String> serviceRequest)
        {
            return _requirementPackageServiceChannel.IsMasterCategoryNameExists(serviceRequest);
        }

        public ServiceResponse<List<RequirementPackageContract>> GetAllMasterRequirementPackages()
        {
            return _requirementPackageServiceChannel.GetAllMasterRequirementPackages();
        }

        public ServiceResponse<Int32> CreateCategoryCopy(ServiceRequest<CreateCategoryCopyContract> serviceRequest)
        {
            return _requirementPackageServiceChannel.CreateCategoryCopy(serviceRequest);
        }

        public ServiceResponse<List<RequirementPackageContract>> GetCategoryPackageMapping(ServiceRequest<CategoryPackageMappingContract, CustomPagingArgsContract> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetCategoryPackageMapping(serviceRequest);
        }

        public ServiceResponse<Boolean> SaveCategoryPackageMapping(ServiceRequest<Int32, Int32, String> serviceRequest)
        {
            return _requirementPackageServiceChannel.SaveCategoryPackageMapping(serviceRequest);
        }

        public ServiceResponse<List<Int32>> GetMappedPackageIdsWithCategory(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetMappedPackageIdsWithCategory(serviceRequest);
        }

        public ServiceResponse<List<Int32>> GetMappedCategoriesWithPackage(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetMappedCategoriesWithPackage(serviceRequest);
        }

        public ServiceResponse<List<RequirementCategoryContract>> GetMasterRequirementCategories(ServiceRequest<RequirementCategoryContract, CustomPagingArgsContract> requirementCategoryDetailsParameters)
        {
            return _requirementPackageServiceChannel.GetMasterRequirementCategories(requirementCategoryDetailsParameters);
        }


        public ServiceResponse<String> DeleteRequirementCategory(ServiceRequest<int> serviceRequest)
        {
            return _requirementPackageServiceChannel.DeleteRequirementCategory(serviceRequest);
        }



        public ServiceResponse<List<RequirementCategoryContract>> GetPackageCategoryMapping(ServiceRequest<PackageCategoryMappingContract, CustomPagingArgsContract> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetPackageCategoryMapping(serviceRequest);
        }
        //UAT:4279
        public ServiceResponse<Boolean> UpdateNodeDisplayOrder(ServiceRequest<List<RequirementCategoryContract>, Int32?, Int32, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.UpdatePackageCategoryMappingDisplayOrder(serviceRequest);
        }
        public ServiceResponse<Boolean> SavePackageCategoryMapping(ServiceRequest<Int32, Int32, String> serviceRequest)
        {
            return _requirementPackageServiceChannel.SavePackageCategoryMapping(serviceRequest);
        }

        public ServiceResponse<List<RequirementCategoryContract>> GetRequirementCategories()
        {
            return _requirementPackageServiceChannel.GetRequirementCategories();
        }

        public ServiceResponse<int> SaveMasterRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Boolean> serviceRequest)
        {
            return _requirementPackageServiceChannel.SaveMasterRequirementPackage(serviceRequest);
        }

        public ServiceResponse<Boolean> ArchivePackage(ServiceRequest<Dictionary<Int32, Boolean>, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.ArchivePackage(serviceRequest);
        }

        //UAT-4054
        public ServiceResponse<Boolean> UnArchivePackage(ServiceRequest<Dictionary<Int32, Boolean>, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.UnArchivePackage(serviceRequest);
        }

        public ServiceResponse<List<Int32>> GetMappedPackageDetails(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetMappedPackageDetails(serviceRequest);
        }
        #endregion

        #region Field Rule
        public ServiceResponse<Int32> GetRequirementObjectTreeIDByprntID(ServiceRequest<String> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRequirementObjectTreeIDByprntID(serviceRequest);
        }
        public ServiceResponse<List<RequirementRuleContract>> GetReqFixedRuleDetailByObjectTreeID(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetReqFixedRuleDetailByObjectTreeID(serviceRequest);
        }
        public ServiceResponse<Boolean> SaveUpdateNewRequirementFieldRule(ServiceRequest<List<RequirementRuleContract>, Int32, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.SaveUpdateNewRequirementFieldRule(serviceRequest);
        }
        public ServiceResponse<List<Int32>> GetRequirementObjectTreeIDByReqFieldID(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRequirementObjectTreeIDByReqFieldID(serviceRequest);
        }
        #endregion

        #region 2213 Copy Package

        public ServiceResponse<Int32> CopySharedRequirementPackageToClientNew(ServiceRequest<Int32, Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.CopySharedRequirementPackageToClientNew(requirementPackageParameters);
        }

        #endregion

        #region Check Rot. Eff. Start Date & EndDate
        public ServiceResponse<List<Int32>, List<String>> CheckRotEffectiveDate(ServiceRequest<Int32, String, Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.CheckRotEffectiveDate(requirementPackageParameters);
        }

        public ServiceResponse<ClinicalRotationDetailContract> GetRotationDetail(ServiceRequest<Int32, Int32> rotationParameters)
        {
            return _requirementPackageServiceChannel.GetRotationDetail(rotationParameters);
        }
        #endregion

        //UAT-2533
        public ServiceResponse<List<RequirementPackageContract>> GetRequirementPackageDetail(ServiceRequest<String> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRequirementPackageDetail(serviceRequest);
        }
        public ServiceResponse<Boolean> BulkPackageCopy(ServiceRequest<String, Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.BulkPackageCopy(serviceRequest);
        }

        /// <summary>
        /// Get the AgencyHierarchyIds Associated with the Requirement Package.
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>
        public ServiceResponse<List<Int32>> GetAgencyHierarchyIdsByRequirementPackageID(ServiceRequest<Int32> requirementPackageParameters)
        {
            return _requirementPackageServiceChannel.GetAgencyHierarchyIdsByRequirementPackageID(requirementPackageParameters);
        }

        /// <summary>
        /// UAT-2423 Get the Rotation Package Category name and ExplanatoryNotes using rotation ID 
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementCategoryContract>> GetRotationPackageCategoryDetail(ServiceRequest<Int32, Int32, Boolean> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRotationPackageCategoryDetailByRotationID(serviceRequest);
        }

        #region UAT-2706
        public ServiceResponse<List<RequirementItemContract>> GetRequirementItemDetailsByCategoryId(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRequirementItemDetailsByCategoryId(serviceRequest);
        }
        public ServiceResponse<Dictionary<Int32, String>> GetSharedRequirementCategoryData(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetSharedRequirementCategoryData(serviceRequest);
        }

        public ServiceResponse<List<RequirementPackageContract>> GetRequirementPackagesByHierarcyIds(ServiceRequest<List<Int32>, Int32, CustomPagingArgsContract> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetRequirementPackagesByHierarcyIds(serviceRequest);
        }

        //UAT-2795

        public ServiceResponse<String> GetCategoryDocumentLink(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetCategoryDocumentLink(serviceRequest);
        }
        #endregion

        #region UAT-2788
        public ServiceResponse<List<RequirementFieldType>> GetAttributeType(ServiceRequest<Int32> parameters)
        {
            return _requirementPackageServiceChannel.GetAttributeType(parameters);
        }
        #endregion

        #region UAT-3078
        public ServiceResponse<Boolean> updateRequirementItemDisplayOrder(ServiceRequest<Int32, Int32, Int32, Int32> parameters, Boolean isNewPackage)
        {
            return _requirementPackageServiceChannel.updateRequirementItemDisplayOrder(parameters, isNewPackage);
        }

        public ServiceResponse<Boolean> updateRequirementFieldDisplayOrder(ServiceRequest<Int32, Int32, Int32, Int32> parameters, Boolean isNewPackage)
        {
            return _requirementPackageServiceChannel.updateRequirementFieldDisplayOrder(parameters, isNewPackage);
        }
        #endregion

        #region UAT-3176
        public ServiceResponse<List<RequirementAttributeGroups>> GetRequirementAttributeGroups(ServiceRequest<Int32> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementAttributeGroups(parameters);
        }
        #endregion

        public ServiceResponse<List<RequirementReviewByContract>> GetRequirementReviewBy(ServiceRequest<Int32, Boolean> parameter)
        {
            return _requirementPackageServiceChannel.GetRequirementReviewBy(parameter);
        }

        public ServiceResponse<Boolean> CloneRequirementItem(ServiceRequest<Int32, Int32, Int32> parameters)
        {
            return _requirementPackageServiceChannel.CloneRequirementItem(parameters);
        }
        #region UAT-3296
        public ServiceResponse<String> GetCategoryExplanatoryNotes(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetCategoryExplanatoryNotes(serviceRequest);
        }
        #endregion

        #region UAT-4001
        public ServiceResponse<List<RequirementDocumentAcroFieldType>> GetDocumentAcroFieldTypeData(ServiceRequest<Int32> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetDocumentAcroFieldTypeData(serviceRequest);
        }
        #endregion

        #region UAT-4254 || Release- 181

        public ServiceResponse<List<RequirementCategoryDocUrl>> GetRequirementCatDocUrls(ServiceRequest<Int32> parameters)
        {
            return _requirementPackageServiceChannel.GetRequirementCatDocUrls(parameters);
        }

        #endregion

        #region UAT-4657
        public ServiceResponse<Dictionary<Int32, String>> GetPackagesAssociatedWithCategory(ServiceRequest<int> serviceRequest)
        {
            return _requirementPackageServiceChannel.GetPackagesAssociatedWithCategory(serviceRequest);
        }
        public ServiceResponse<Boolean> SaveCategoryDiassociationDetail(ServiceRequest<Int32,String> serviceRequest)
        {
            return _requirementPackageServiceChannel.SaveCategoryDiassociationDetail(serviceRequest);
        }
        public ServiceResponse<String> IsCategoryDisassociationInProgress(ServiceRequest<Int32, List<Int32>> serviceRequest)
        {
            return _requirementPackageServiceChannel.IsCategoryDisassociationInProgress(serviceRequest);
        }
        public ServiceResponse<Boolean> IsSyncAlreadyInProgress(ServiceRequest<Int32, Boolean> serviceRequest)
        {
            return _requirementPackageServiceChannel.IsSyncAlreadyInProgress(serviceRequest);
        }
        #endregion
    }

}
