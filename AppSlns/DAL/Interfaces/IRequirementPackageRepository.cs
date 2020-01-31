using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace DAL.Interfaces
{
    public interface IRequirementPackageRepository
    {
        Boolean AddRequirementPackageToDatabase(RequirementPackage requirementPackage);

        /// <summary>
        /// used to get requirement package details including package name,category name,item name and field name in hierarichal way
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementPackageDetailsContract> GetRequirementPackageDetailsByPackageID(Int32 requirementPackageID);

        List<RequirementPackageContract> GetRequirementPackages(String agencyId);

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

        Boolean IsPackageVersionNeedToCreate(Int32 requirementPackageID, String reqPkgTypeCode);

        Boolean IsPackageMappedToRotation(Int32 requirementPackageID);

        RequirementPackage GetRequirementPackageByPackageID(Int32 requirementPackageID);

        RequirementPackage GetRequirementPackageByPackageCode(Guid requirementPackageCode);

        List<RequirementObjectTree> GetRequirementObjectTreeList(List<Int32> reqObjectTreeIds);

        RequirementObjectTree GetRequirementObjectTree(Int32 reqObjectTreeId);

        Boolean SaveContextIntoDataBase();

        Boolean AddLargeContentToDatabase(List<LargeContent> largeContentList);

        Boolean AddLargeContentToContext(LargeContent largeContent);

        LargeContent GetLargeContentForReqrmntCategory(Int32 reqrmntCategoryID, Int32 objectTypeID, Int32 contentTypeID);

        List<RequirementPackageContract> GetInstructorRequirementPackages(String agencyId);

        Boolean SetExistingPackageIsCopiedToTrue(Int32 currentLoggedInUserID, Int32 requirementPackageID);

        Boolean SetExistingPackageIsDeletedToTrue(Int32 currentLoggedInUserID, Int32 requirementPackageID);

        List<RequirementPackageContract> GetAllRequirementPackages();

        DataTable GetRequirementCategoriesRqdForComplianceAction();

        Dictionary<Int32, Boolean> ProcessRotcomplianceRqdChange(Int32 currentUserId, RequirementPackageCategory requirementPackageCategory);

        #region UAT-2514 Copy Package

        /// <summary>
        /// Get Requirement Category If it already exists, check on the bais of CategoryCode
        /// </summary>
        /// <param name="requirementCategoryCode"></param>
        /// <returns></returns>
        RequirementCategory GetRequirementCategoryIfAlreadyExists(Guid requirementCategoryCode);
        /// <summary>
        /// Get Requirement Item If it already exists, check on the bais of CategoryCode
        /// </summary>
        /// <param name="requirementItemCode"></param>
        /// <returns></returns>
        RequirementItem GetRequirementItemIfAlreadyExists(Guid requirementItemCode);
        /// <summary>
        /// Add in RequirementObjectTree
        /// </summary>
        /// <param name="requirementCategoryID"></param>
        /// <param name="catObjectTypeId"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        List<RequirementObjectTreeContract> AddRequirementObjectTreeNew(Int32 requirementCategoryID, Int32 catObjectTypeId, Int32 currentUserID);
        /// <summary>
        /// Check If Category ID Already Exists in Object Tree
        /// </summary>
        /// <param name="requirementCategoryID"></param>
        /// <param name="catObjectTypeId"></param>
        /// <returns></returns>
        Boolean IfCategoryIdAlreadyExistsInObjectTree(Int32 requirementCategoryID, Int32 catObjectTypeId);

        #endregion

        #region UAT-2514
        String RequirementPkgSync(Int32 currentLoggedInUserId, String reqPackageObjectIds);
        #endregion

        List<RequirementCategoryContract> GetRotationPackageCategoryDetailByRotationID(Int32 rotationID, Boolean IsStudentPackage);
        List<RequirementPackageContract> GetRequirementPackagesFromAgencyIds(String agencyId, String reqPkgTypeCode=null);//UAT-2973
        Boolean UpdateRequirementPackageAgencyMappings(List<Int32> lstNewRequirementPkgAgencies, Int32 currentLoggedInUserID, Int32 requirementPackageId);//UAT-2973 
		
        String GetRotationHierarchyIdsBasedOnSubscriptionID(Int32 packageSubscriptionID); //UAT 3120
        String GetReqPackageSubsStatusBySubscriptionID(Int32 packageSubscriptionId); //UAT 3080

        void RequirementVerificationDataToFlatTable(String packageSubscriptionIDs, Int32 currentLoggedInUserId , Int32 taskId);

        void StoreRequirenmentPackageIdsInScheduleTask(String packageSubscriptionIDs, Int32 currentLoggedInUserId);

        List<ScheduledTask> GetRequirenmentPackageIdsInScheduleTask(Int32 tasktypeid, Int32 taskstatustypeIdPending);
       // List<RequirementAttributeGroup> GetAllRequirementPackages(); //3176
    }
}
