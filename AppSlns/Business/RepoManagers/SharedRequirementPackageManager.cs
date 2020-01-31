using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace Business.RepoManagers
{
    public class SharedRequirementPackageManager
    {

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static SharedRequirementPackageManager()
        {
            BALUtils.ClassModule = "Shared Requirement Package Manager";
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Method to get all possible data types of rotation fields
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<RotationFieldDataTypeContract> GetRotationFieldDataTypes()
        {
            try
            {
                List<Entity.SharedDataEntity.lkpRequirementFieldDataType> dataTypeList = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpRequirementFieldDataType>();
                return dataTypeList.Select(col => new RotationFieldDataTypeContract
                {
                    DataTypeCode = col.RFDT_Code,
                    DataTypeName = col.RFDT_Name,
                    DataTypeID = col.RFDT_ID
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 SaveRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID
                                            , Boolean isPackageMappedToRotation = false, Boolean isCopy = false, Boolean isCreateMasterVersion = false)
        {
            try
            {
                Boolean isNewPackage = requirementPackageContract.RequirementPackageID == AppConsts.NONE;
                Boolean isPackageVersionNeedToCreate = false;

                if (!requirementPackageContract.IsCopyWithInMaster)
                    isPackageVersionNeedToCreate = isCreateMasterVersion || IsPackageVersionNeedToCreate(requirementPackageContract.RequirementPackageID);

                if (isNewPackage || isPackageVersionNeedToCreate || isCopy)
                {
                    Int32 RequirementPkgID = AddNewRequirementPackage(requirementPackageContract, currentLoggedInUserID, isPackageVersionNeedToCreate, isPackageMappedToRotation, isCopy);
                    if (isPackageVersionNeedToCreate)
                    {
                        SendRotationPackageVersioningNotification(requirementPackageContract);
                    }
                    return RequirementPkgID;
                }
                else
                {
                    //call method to update existing package package by passing contract to it
                    return UpdateRequirementPackage(requirementPackageContract, currentLoggedInUserID);
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 DeleteRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID)
        {
            try
            {
                Boolean isPackageVersionNeedToCreate = IsPackageVersionNeedToCreate(requirementPackageContract.RequirementPackageID);
                if (requirementPackageContract.IsDeleted && isPackageVersionNeedToCreate)
                {
                    return AppConsts.NONE;
                }
                else if (isPackageVersionNeedToCreate)
                {

                    return AddNewRequirementPackage(requirementPackageContract, currentLoggedInUserID, isPackageVersionNeedToCreate);
                }
                else
                {
                    //call method to update existing package package by passing contract to it
                    return DeleteRequirementPackageEntities(requirementPackageContract, currentLoggedInUserID);
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static Int32 DeleteRequirementPackageEntities(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID)
        {
            List<RequirementFieldContract> requirementFieldContractList = new List<RequirementFieldContract>();
            List<RequirementItemContract> requirementItemContractList = new List<RequirementItemContract>();

            Int32 objectTreeIDToBeDeleted = 0;

            //Get existing requirement package(from DB) which is to be deleted 
            RequirementPackage existingRequirementPackage = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                    .GetRequirementPackageByPackageID(requirementPackageContract.RequirementPackageID);

            //if package does not exists
            if (existingRequirementPackage.IsNullOrEmpty())
            {
                return AppConsts.NONE;
            }
            else if (requirementPackageContract.IsDeleted)
            {
                List<RequirementPackageAgency> lstMappedAgencies = existingRequirementPackage.RequirementPackageAgencies
                                                            .Where(cond => !cond.RPA_IsDeleted).ToList();
                List<Int32> lstAgenciesMappedWithAgencyUser = new List<Int32>();

                if (!requirementPackageContract.IsNewPackage)
                {
                    lstAgenciesMappedWithAgencyUser = ProfileSharingManager.GetAgencies(AppConsts.NONE, false, true, requirementPackageContract.CurrentUserId).Select(n => n.AG_ID).ToList();
                }

                if (requirementPackageContract.IsManageMasterPackage)
                {
                    existingRequirementPackage.RP_IsDeleted = true;
                    existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                    existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
                    //Remove AgencyHierarchyPackage Records
                    existingRequirementPackage.AgencyHierarchyPackages.Where(cmd => !cmd.AHP_IsDeleted).ForEach(x =>
                    {
                        x.AHP_IsDeleted = true;
                        x.AHP_ModifiedBy = currentLoggedInUserID;
                        x.AHP_ModifiedOn = DateTime.Now;
                    });
                    objectTreeIDToBeDeleted = requirementPackageContract.PackageObjectTreeID;
                }
                else if (existingRequirementPackage.RP_IsCopied.HasValue && existingRequirementPackage.RP_IsCopied.Value
                     || (!lstMappedAgencies.IsNullOrEmpty() && lstMappedAgencies.Count > AppConsts.ONE))
                {
                    //delete agency mapping
                    List<RequirementPackageAgency> lstAgencyMappingForCurrentUser = existingRequirementPackage.RequirementPackageAgencies
                                                                               .Where(cond => lstAgenciesMappedWithAgencyUser.Contains(cond.RPA_AgencyID) && !cond.RPA_IsDeleted).ToList();

                    foreach (RequirementPackageAgency agencyMappingForCurrentUser in lstAgencyMappingForCurrentUser)
                    {
                        agencyMappingForCurrentUser.RPA_IsDeleted = true;
                        agencyMappingForCurrentUser.RPA_ModifiedByID = currentLoggedInUserID;
                        agencyMappingForCurrentUser.RPA_ModifiedOn = DateTime.Now;
                    }
                }
                else
                {
                    existingRequirementPackage.RP_IsDeleted = true;
                    existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                    existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
                    //Remove AgencyHierarchyPackage Records
                    existingRequirementPackage.AgencyHierarchyPackages.Where(cmd => !cmd.AHP_IsDeleted).ForEach(x =>
                    {
                        x.AHP_IsDeleted = true;
                        x.AHP_ModifiedBy = currentLoggedInUserID;
                        x.AHP_ModifiedOn = DateTime.Now;
                    });
                    objectTreeIDToBeDeleted = requirementPackageContract.PackageObjectTreeID;
                }
            }
            else
            {
                foreach (RequirementCategoryContract categoryContract in requirementPackageContract.LstRequirementCategory)
                {
                    RequirementPackageCategory existingRequrmntPkgCategory = existingRequirementPackage.RequirementPackageCategories
                                                                                         .Where(cond => cond.RPC_ID == categoryContract.RequirementPackageCategoryID && !cond.RPC_IsDeleted).FirstOrDefault();
                    //if category is deleted
                    if (categoryContract.IsDeleted)
                    {
                        existingRequrmntPkgCategory.RPC_IsDeleted = true;
                        existingRequrmntPkgCategory.RPC_ModifiedByID = currentLoggedInUserID;
                        existingRequrmntPkgCategory.RPC_ModifiedOn = DateTime.Now;
                        objectTreeIDToBeDeleted = categoryContract.CategoryObjectTreeID;
                        break;
                    }
                    else
                    {
                        foreach (RequirementItemContract itemContract in categoryContract.LstRequirementItem)
                        {
                            requirementItemContractList.Add(itemContract);
                            RequirementCategoryItem existingRequrmntCategoryItem = existingRequrmntPkgCategory.RequirementCategory.RequirementCategoryItems
                                                                                               .Where(cond => cond.RCI_ID == itemContract.RequirementCategoryItemID && !cond.RCI_IsDeleted).FirstOrDefault();

                            if (itemContract.IsDeleted)
                            {
                                RequirementObjectRule rule = BALUtils.GetSharedRequirementPackageRepoInstance()
                                    .GetRequirementObjectRuleForObjectTreeID(categoryContract.CategoryObjectTreeID);
                                if (!rule.IsNullOrEmpty() && !rule.ROR_UIExpression.IsNullOrEmpty())
                                {
                                    String sqlExpression = rule.ROR_SqlExpression;
                                    string itemString = String.Concat("$" + itemContract.RequirementItemID + "#");
                                    Boolean ifItemIsusedInRule = sqlExpression.Contains(itemString);
                                    if (ifItemIsusedInRule)
                                    {
                                        return -1;
                                    }
                                }
                                existingRequrmntCategoryItem.RCI_IsDeleted = true;
                                existingRequrmntCategoryItem.RCI_ModifiedByID = currentLoggedInUserID;
                                existingRequrmntCategoryItem.RCI_ModifiedOn = DateTime.Now;
                                objectTreeIDToBeDeleted = itemContract.ItemObjectTreeID;
                                break;
                            }
                            else
                            {
                                foreach (RequirementFieldContract fieldContract in itemContract.LstRequirementField)
                                {
                                    if (fieldContract.IsDeleted)
                                    {
                                        RequirementItemField existingReqrmntItemField = existingRequrmntCategoryItem.RequirementItem.RequirementItemFields
                                                                                                         .Where(cond => cond.RIF_ID == fieldContract.RequirementItemFieldID && !cond.RIF_IsDeleted)
                                                                                                          .FirstOrDefault();
                                        existingReqrmntItemField.RIF_IsDeleted = true;
                                        existingReqrmntItemField.RIF_ModifiedByID = currentLoggedInUserID;
                                        existingReqrmntItemField.RIF_ModifiedOn = DateTime.Now;
                                        objectTreeIDToBeDeleted = fieldContract.FieldObjectTreeID;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (objectTreeIDToBeDeleted > 0)
            {
                DeleteRequirementObjectTree(objectTreeIDToBeDeleted, currentLoggedInUserID);
            }

            BALUtils.GetSharedRequirementPackageRepoInstance().SaveContextIntoDataBase();

            return existingRequirementPackage.RP_ID;
        }

        /// <summary>
        /// used to get requirement package details including package name,category name,item name and field name in hierarichal way
        /// </summary>
        /// <returns></returns>
        public static List<RequirementPackageDetailsContract> GetRequirementPackageDetailsByPackageID(Int32 requirementPackageID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackageDetailsByPackageID(requirementPackageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// used to get all requirement package details including package name and comma separated agencyNames with which they are mapped. It also returns unMapped packages too
        /// </summary>
        /// <returns></returns>
        public static List<RequirementPackageDetailsContract> GetRequirementPackageDetails(RequirementPackageDetailsContract requirementPackageDetailsContract
                                                                                           , CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance()
                               .GetRequirementPackageDetails(requirementPackageDetailsContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<RequirementPackageContract> GetRequirementPackages(Int32 selectedTenantID, String agencyID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackages(selectedTenantID, agencyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// To get Instructor/Preceptor Requirement Packages
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<RequirementPackageContract> GetInstructorRequirementPackages(Int32 selectedTenantID, String agencyId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetInstructorRequirementPackages(selectedTenantID, agencyId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// get complete package details in hierarchal way
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        public static RequirementPackageContract GetRequirementPackageHierarchalDetailsByPackageID(Int32 requirementPackageID, Guid userID, Boolean isCopy = false)
        {
            try
            {
                List<RequirementPackageHierarchicalDetailsContract> requirmntPkgHrchicalContract = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                                                           .GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageID);

                RequirementPackageContract requirementPackageDetailsContract = AssignPackageHierarchalDetailsToSessionContract(requirmntPkgHrchicalContract, userID, isCopy);

                //UAT-2647
                List<Int32> lstAgencyHierarchyIDs = BALUtils.GetSharedRequirementPackageRepoInstance().GetAgencyHierarchyIdsWithPkgId(requirementPackageID);
                requirementPackageDetailsContract.SelectedAgencyHierarchyDeatils = new Dictionary<Int32, String>();
                requirementPackageDetailsContract.LstAgencyHierarchyIDs = new List<Int32>();
                foreach (var AgencyHierarchyID in lstAgencyHierarchyIDs.Distinct().ToList())
                {
                    requirementPackageDetailsContract.SelectedAgencyHierarchyDeatils.Add(AgencyHierarchyID, String.Empty);
                    requirementPackageDetailsContract.LstAgencyHierarchyIDs.Add(AgencyHierarchyID);
                }

                #region UAT-4254

                requirementPackageDetailsContract.LstRequirementCategory.ForEach(x =>
                {
                    List<RequirementCategoryDocUrl> lstCatUrls = GetRequirementCatDocUrls(x.RequirementCategoryID);
                    x.lstReqCatDocUrls.AddRange(lstCatUrls);
                });

                #endregion

                return requirementPackageDetailsContract;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// method used to return lkpConstantType values
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        public static List<RulesConstantTypeContract> GetRulesConstantTypes()
        {
            try
            {
                List<Entity.SharedDataEntity.lkpConstantType> constantTypeList = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpConstantType>();
                return constantTypeList.Select(col => new RulesConstantTypeContract
                {
                    ConstantTypeID = col.ID,
                    ConstantTypeName = col.Name,
                    ConstantTypeCode = col.Code,
                    ConstantTypeGroup = col.Group
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Private Methods

        private static RequirementPackageContract AssignPackageHierarchalDetailsToSessionContract(List<RequirementPackageHierarchicalDetailsContract> LstRequirementPackageHierarchicalDetailsContract, Guid userID, Boolean isCopy)
        {
            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();

            if (!LstRequirementPackageHierarchicalDetailsContract.IsNullOrEmpty())
            {
                List<Int32> lstAgenciesMappedWithAgencyUser = new List<int>();
                if (!isCopy)
                {
                    lstAgenciesMappedWithAgencyUser = ProfileSharingManager.GetAgencies(AppConsts.NONE, false, true, userID).Select(n => n.AG_ID).ToList();
                }
                //Add data into top level contract - RequirementPackageContract
                RequirementPackageHierarchicalDetailsContract packageHierarchicalDetailsContract = LstRequirementPackageHierarchicalDetailsContract.FirstOrDefault();
                requirementPackageContract.IsSharedUserLoggedIn = true;
                requirementPackageContract.RequirementPackageID = packageHierarchicalDetailsContract.RequirementPackageID;
                requirementPackageContract.RequirementPackageName = packageHierarchicalDetailsContract.RequirementPackageName;
                requirementPackageContract.IsSharedUserPackage = true;
                requirementPackageContract.RequirementPackageLabel = packageHierarchicalDetailsContract.RequirementPackageLabel;
                requirementPackageContract.DefinedRequirementID = packageHierarchicalDetailsContract.DefinedRequirementID;
                //requirementPackageContract.ReqReviewByID = packageHierarchicalDetailsContract.reqre;

                //requirementPackageContract.RequirementPackageDescription = packageHierarchicalDetailsContract.RequirementPackageDescription;
                requirementPackageContract.LstAgencyIDs = LstRequirementPackageHierarchicalDetailsContract
                                                        .Where(cond => cond.RequirementPackageAgencyID > 0
                                                        && (isCopy || lstAgenciesMappedWithAgencyUser.Contains(cond.AgencyID)))
                                                        .Select(col => col.AgencyID).Distinct().ToList();
                if (!isCopy)
                {
                    List<Int32> allAgencyIDs = LstRequirementPackageHierarchicalDetailsContract
                                                        .Where(cond => cond.RequirementPackageAgencyID > 0)
                                                        .Select(col => col.AgencyID).ToList();
                    requirementPackageContract.LstAgencyIDsWithNoAgencyUerPermission = allAgencyIDs.Except(lstAgenciesMappedWithAgencyUser).ToList();
                }
                requirementPackageContract.LstAgencyNames = LstRequirementPackageHierarchicalDetailsContract
                                                        .Where(cond => cond.RequirementPackageAgencyID > 0
                                                    && (isCopy || lstAgenciesMappedWithAgencyUser.Contains(cond.AgencyID)))
                                                        .DistinctBy(col => col.AgencyID).Select(col => col.AgencyName).ToList();
                requirementPackageContract.LstSelectedTenantIDs = LstRequirementPackageHierarchicalDetailsContract.Where(cond => cond.RequirementPackageInstitutionID > 0)
                                        .Select(col => col.MappedTenantID).Distinct().ToList();
                requirementPackageContract.LstSelectedTenantNames = LstRequirementPackageHierarchicalDetailsContract.Where(cond => cond.RequirementPackageInstitutionID > 0)
                                                        .DistinctBy(col => col.MappedTenantID).Select(col => col.MappedTenantName).ToList();
                requirementPackageContract.RequirementPackageCode = packageHierarchicalDetailsContract.RequirementPackageCode;
                requirementPackageContract.PackageRuleTypeCode = packageHierarchicalDetailsContract.RequirementPackageRuleTypeCode;
                requirementPackageContract.PackageObjectTreeID = packageHierarchicalDetailsContract.PackageObjectTreeID;

                //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
                requirementPackageContract.RequirementPkgTypeID = packageHierarchicalDetailsContract.RequirementPkgTypeID;
                requirementPackageContract.RequirementPkgTypeCode = packageHierarchicalDetailsContract.RequirementPkgTypeCode;

                //Add data into Category contract - RequirementCategoryContract

                requirementPackageContract.LstRequirementCategory = new List<RequirementCategoryContract>();

                List<RequirementPackageHierarchicalDetailsContract> lstPackagesDistinctByCategory = LstRequirementPackageHierarchicalDetailsContract
                                                                                                    .Where(cond => cond.RequirementCategoryID != AppConsts.NONE)
                                                                                                    .DistinctBy(col => col.RequirementPackageCategoryID).ToList();

                foreach (RequirementPackageHierarchicalDetailsContract packageDetailDistinctByCategory in lstPackagesDistinctByCategory)
                {
                    RequirementCategoryContract categoryData = new RequirementCategoryContract();
                    categoryData.RequirementCategoryID = packageDetailDistinctByCategory.RequirementCategoryID;
                    categoryData.RequirementPackageCategoryID = packageDetailDistinctByCategory.RequirementPackageCategoryID;
                    categoryData.RequirementCategoryName = packageDetailDistinctByCategory.RequirementCategoryName;

                    //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes).
                    categoryData.RequirementDocumentLink = packageDetailDistinctByCategory.RequirementDocumentLink;
                    //UAT-3161
                    categoryData.RequirementDocumentLinkLabel = packageDetailDistinctByCategory.RequirementDocumentLinkLabel;
                    categoryData.RequirementCategoryCode = packageDetailDistinctByCategory.RequirementCategoryCode;
                    categoryData.CategoryRuleTypeCode = packageDetailDistinctByCategory.RequirementCategoryRuleTypeCode;
                    categoryData.CategoryObjectTreeID = packageDetailDistinctByCategory.CategoryObjectTreeID;
                    categoryData.ExplanatoryNotes = packageDetailDistinctByCategory.CategoryExplanatoryNotes;

                    #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                    //Filling RequirementPackageContract for ComplianceRequired Setting.
                    categoryData.IsComplianceRequired = packageDetailDistinctByCategory.IsComplianceRequired;
                    categoryData.ComplianceReqStartDate = packageDetailDistinctByCategory.ComplianceReqStartDate.HasValue ? packageDetailDistinctByCategory.ComplianceReqStartDate : null;
                    categoryData.ComplianceReqEndDate = packageDetailDistinctByCategory.ComplianceReqEndDate.HasValue ? packageDetailDistinctByCategory.ComplianceReqEndDate : null;
                    #endregion

                    //Add data into Item contract - RequirementItemContract
                    List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByItem = LstRequirementPackageHierarchicalDetailsContract
                                                                .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                       && cond.RequirementItemID != AppConsts.NONE)
                                                                .DistinctBy(col => col.RequirementItemID).ToList();

                    categoryData.RuleUIExpression = packageDetailDistinctByCategory.RequirementCategoryUIExpression;
                    categoryData.RuleSqlExpression = packageDetailDistinctByCategory.RequirementCategorySQLExpression;
                    categoryData.SendItemDoconApproval = packageDetailDistinctByCategory.SendItemDocOnApproval;

                    categoryData.LstRequirementItem = new List<RequirementItemContract>();

                    foreach (RequirementPackageHierarchicalDetailsContract packageDetailsDistinctByItem in LstPackagesDistinctByItem)
                    {
                        RequirementItemContract itemData = new RequirementItemContract();
                        itemData.RequirementCategoryItemID = packageDetailsDistinctByItem.RequirementCategoryItemID;
                        itemData.RequirementItemID = packageDetailsDistinctByItem.RequirementItemID;
                        itemData.RequirementItemLabel = packageDetailsDistinctByItem.RequirementItemLabel;
                        itemData.RequirementItemDisplayOrder = packageDetailsDistinctByItem.RequirementCategoryItemDisplayOrder; //UAT-3078
                        itemData.RequirementItemName = packageDetailsDistinctByItem.RequirementItemName;
                        itemData.RequirementItemCode = packageDetailsDistinctByItem.RequirementItemCode;
                        itemData.ItemObjectTreeID = packageDetailsDistinctByItem.ItemObjectTreeID;
                        itemData.ExplanatoryNotes = packageDetailsDistinctByItem.RequirementItemDescription;
                        itemData.RequirementItemSampleDocumentFormURL = packageDetailsDistinctByItem.RequirementItemSampleDocumentFormURL; //UAT-3309
                        itemData.RequirementItemExpiration = new RequirementItemExpirationContract();

                        List<RequirementPackageHierarchicalDetailsContract> lstCurrentItemRows = LstRequirementPackageHierarchicalDetailsContract
                                                                    .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                             && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                             && cond.RequirementItemID != AppConsts.NONE).ToList();

                        if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()))
                        {
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItem = lstCurrentItemRows.
                                                    Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
                                                    .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItem.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItem.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.ExpirationDate = currentItem.ExpirationValue;
                        }
                        else if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue()))
                        {
                            //UAT-2165
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItem = lstCurrentItemRows.
                                                    Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
                                                    .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItem.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItem.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.ExpirationDate = currentItem.ExpirationValue;
                            itemData.RequirementItemExpiration.ExpirationCondStartDate = currentItem.ExpirationCondStartDate;
                            itemData.RequirementItemExpiration.ExpirationCondEndDate = currentItem.ExpirationCondEndDate;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldID = currentItem.DateTypeRequirementFieldIDForExpiration;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldCode = currentItem.DateTypeRequirementFieldCodeForExpiration;
                        }
                        else if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()))
                        {
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItemWithDateAttribute = lstCurrentItemRows.
                                                                            Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                            && cond.DateTypeRequirementFieldIDForExpiration != 0 && cond.ExpirationValue == String.Empty)
                                                                            .FirstOrDefault();
                            RequirementPackageHierarchicalDetailsContract currentItemWithDateAttributeValue = lstCurrentItemRows.
                                                                            Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                            && cond.DateTypeRequirementFieldIDForExpiration == 0)
                                                                            .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItemWithDateAttributeValue.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItemWithDateAttributeValue.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldCode = currentItemWithDateAttribute.DateTypeRequirementFieldCodeForExpiration;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldID = currentItemWithDateAttribute.DateTypeRequirementFieldIDForExpiration;
                            itemData.RequirementItemExpiration.ExpirationValue = Convert.ToInt32(currentItemWithDateAttributeValue.ExpirationValue);
                        }
                        else
                        {
                            itemData.IsRequirementItemNeededExpiration = false;
                            itemData.RequirementItemExpiration = null;
                        }

                        itemData.RequirementItemRuleTypeCode = lstCurrentItemRows.
                                                            Where(cond => cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()
                                                                      && cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                      && cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue()
                                                                                ).FirstOrDefault().RequirementItemRuleTypeCode;

                        List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField = LstRequirementPackageHierarchicalDetailsContract
                                                                    .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                             && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                              && cond.RequirementFieldID != AppConsts.NONE)
                                                                    .DistinctBy(col => col.RequirementFieldID).ToList();
                        itemData.LstRequirementField = new List<RequirementFieldContract>();
                        //Add data into field contract - RequirementFieldContract
                        foreach (RequirementPackageHierarchicalDetailsContract packagedetailsDistinctByField in LstPackagesDistinctByField)
                        {
                            RequirementFieldContract fieldContract = new RequirementFieldContract();
                            fieldContract.RequirementFieldID = packagedetailsDistinctByField.RequirementFieldID;
                            fieldContract.RequirementItemFieldID = packagedetailsDistinctByField.RequirementItemFieldID;
                            fieldContract.RequirementFieldName = packagedetailsDistinctByField.RequirementFieldName;
                            fieldContract.AttributeTypeID = packagedetailsDistinctByField.RequirementFieldAttributeTypeID;
                            //fieldContract.RequirementFieldLabel = packagedetailsDistinctByField.RequirementFieldLabel;
                            fieldContract.RequirementFieldDisplayOrder = packagedetailsDistinctByField.RequirementItemFieldDisplayOrder; //UAT-3078

                            fieldContract.RequirementFieldAttributeGroupId = packagedetailsDistinctByField.RequirementAttributeGroupID; //UAT-3176
                            //fieldContract.RequirementFieldDescription = packagedetailsDistinctByField.RequirementFieldDescription;
                            fieldContract.RequirementFieldCode = packagedetailsDistinctByField.RequirementFieldCode;
                            fieldContract.FieldObjectTreeID = packagedetailsDistinctByField.FieldObjectTreeID;

                            //UAT-2164
                            fieldContract.IsBackgroundDocument = packagedetailsDistinctByField.IsBackgroundDocument;

                            //UAT-2366
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue())
                            {
                                fieldContract.UiRequirementItemID = packagedetailsDistinctByField.UiRequirementItemID;
                                fieldContract.UiRequirementFieldID = packagedetailsDistinctByField.UiRequirementFieldID;
                                fieldContract.RequirementFieldUIRuleTypeCode = packagedetailsDistinctByField.RequirementFieldFixedRuleTypeCode;
                                fieldContract.UiRuleErrorMessage = packagedetailsDistinctByField.UiRuleErrorMessage;
                            }

                            List<RequirementPackageHierarchicalDetailsContract> currentFieldRows = LstRequirementPackageHierarchicalDetailsContract
                                                .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                      && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                      && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID && cond.RequirementFieldID != AppConsts.NONE)
                                                         .ToList();

                            if (!Convert.ToBoolean(currentFieldRows.Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.REQUIRED.GetStringValue()).IsNullOrEmpty()))
                            {
                                fieldContract.IsFieldRequired = Convert.ToBoolean(currentFieldRows.Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.REQUIRED.GetStringValue())
                                                                            .FirstOrDefault().RequirementFieldRuleValue);
                            }
                            else
                            {
                                fieldContract.IsFieldRuleNotDefined = true;
                            }

                            //Add data into Field data contract - RequirementFieldDataContract

                            RequirementFieldDataContract fieldDataContract = new RequirementFieldDataContract();
                            fieldDataContract.RequirementFieldDataTypeCode = packagedetailsDistinctByField.RequirementFieldDataTypeCode;
                            fieldDataContract.RequirementFieldDataTypeID = packagedetailsDistinctByField.RequirementFieldDataTypeID;
                            fieldDataContract.RequirementFieldDataTypeName = packagedetailsDistinctByField.RequirementFieldDataTypeName;

                            //Save data into video contract - RequirementFieldVideoData
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                            {
                                RequirementFieldVideoData videoData = new RequirementFieldVideoData();
                                videoData.RequirementFieldVideoID = packagedetailsDistinctByField.RequirementFieldVideoID;
                                RequirementPackageHierarchicalDetailsContract videoFieldRules_OpenRequired = currentFieldRows
                                    .Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.REQUIRED_TO_OPEN.GetStringValue()).FirstOrDefault();
                                if (!videoFieldRules_OpenRequired.IsNullOrEmpty())
                                {
                                    videoData.IsVideoRequiredToBeOpened = Convert.ToBoolean(videoFieldRules_OpenRequired.RequirementFieldRuleValue);
                                }
                                if (videoData.IsVideoRequiredToBeOpened)
                                {
                                    videoData.VideoOpenTimeDuration = Convert.ToInt32(currentFieldRows
                                        .Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue())
                                        .FirstOrDefault().RequirementFieldRuleValue);
                                    videoData.VideoOpenedMinutes = videoData.VideoOpenTimeDuration / 60;
                                    videoData.VideoOpenedSeconds = videoData.VideoOpenTimeDuration % 60;
                                }

                                videoData.VideoURL = packagedetailsDistinctByField.FieldVideoURL;
                                videoData.VideoName = packagedetailsDistinctByField.RequirementFieldVideoName;
                                fieldDataContract.VideoFieldData = videoData;
                            }

                            //Save data into options contract - RequirementFieldOptionsData
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                            {
                                fieldDataContract.LstRequirementFieldOptions = new List<RequirementFieldOptionsData>();
                                List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField_OptionData = LstRequirementPackageHierarchicalDetailsContract
                                                                   .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                            && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                            && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID
                                                                            ).DistinctBy(col => col.RequirementFieldOptionsID).ToList();
                                StringBuilder formatOptions = new StringBuilder();

                                foreach (RequirementPackageHierarchicalDetailsContract packageFieldOptionData in LstPackagesDistinctByField_OptionData)
                                {
                                    RequirementFieldOptionsData optionData = new RequirementFieldOptionsData();
                                    optionData.RequirementFieldOptionsID = packageFieldOptionData.RequirementFieldOptionsID;
                                    optionData.OptionText = packageFieldOptionData.OptionText;
                                    optionData.OptionValue = packageFieldOptionData.OptionValue;
                                    fieldDataContract.LstRequirementFieldOptions.Add(optionData);

                                    //code to generate FormatOptions to be displayed during edit mode
                                    formatOptions.AppendFormat("{0}={1}|", packageFieldOptionData.OptionText, packageFieldOptionData.OptionValue);
                                }

                                //code to generate FormatOptions to be displayed during edit mode
                                Int32 index = formatOptions.ToString().LastIndexOf('|');
                                if (index >= 0)
                                    formatOptions.Remove(index, 1);

                                fieldDataContract.RequiredFieldOptionsFormattedValue = Convert.ToString(formatOptions);
                            }

                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                            {
                                RequirementFieldViewDocumentData documentData = new RequirementFieldViewDocumentData();
                                documentData.ClientSystemDocumentID = packagedetailsDistinctByField.ClientSystemDocumentID;
                                documentData.DocumentFileName = packagedetailsDistinctByField.ClientSystemDocumentFileName;
                                documentData.DocumentPath = packagedetailsDistinctByField.ClientSystemDocumentPath;
                                documentData.DocumentSize = packagedetailsDistinctByField.ClientSystemDocumentSize;

                                List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField_DocFieldData = LstRequirementPackageHierarchicalDetailsContract
                                                                   .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                            && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                            && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID
                                                                            ).DistinctBy(col => col.DocumentAcroFieldTypeID).ToList();
                                documentData.LstDocumentAcroFieldTypeCodes = new List<String>();
                                documentData.LstRequirementDocumentAcroFieldIDs = new List<Int32>();

                                foreach (RequirementPackageHierarchicalDetailsContract docFieldData in LstPackagesDistinctByField_DocFieldData)
                                {
                                    if (!docFieldData.DocumentAcroFieldTypeCode.IsNullOrEmpty())
                                    {
                                        documentData.LstDocumentAcroFieldTypeCodes.Add(docFieldData.DocumentAcroFieldTypeCode);
                                        documentData.LstRequirementDocumentAcroFieldIDs.Add(docFieldData.RequirementDocumentAcroFieldID);
                                    }
                                }
                                fieldDataContract.FieldViewDocumentData = documentData;
                            }

                            fieldContract.RequirementFieldData = fieldDataContract;
                            itemData.LstRequirementField.Add(fieldContract);
                        }
                        categoryData.LstRequirementItem.Add(itemData);
                    }

                    requirementPackageContract.LstRequirementCategory.Add(categoryData);
                }
            }

            return requirementPackageContract;
        }

        #region RequirementObjectTree related methods

        private static void InsertDataIntoRequirementObjectTree(RequirementPackageContract requirementPackageContract
            , Int32 currentLoggedInUserID, RequirementPackage requirementPackage, List<RequirementFieldContract> requirementFieldContractList
            , List<RequirementItemContract> requirementItemContractList, Boolean isPackageVersionNeedToCreate, Boolean isCopy = false)
        {
            //Add entries into requirement object tree.
            List<RequirementObjectTreeContract> requirmntObjTreeCntrct = AddRequirementObjectTree(requirementPackage.RP_ID, currentLoggedInUserID);
            if (!requirmntObjTreeCntrct.IsNullOrEmpty())
            {
                List<Int32> reqObjectTreeIds = requirmntObjTreeCntrct.Select(sel => sel.RequirementObjectTreeID).ToList();
                List<RequirementObjectTree> lstRequirmntObjTreeFromDb = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeList(reqObjectTreeIds);

                #region Get required Lkp values
                List<lkpObjectType> lstlkpObjectType = LookupManager.GetSharedDBLookUpData<lkpObjectType>().ToList();
                List<lkpObjectAttribute> lstlkpObjectAttribue = LookupManager.GetSharedDBLookUpData<lkpObjectAttribute>().
                                                                                            Where(cond => !cond.OA_IsDeleted).ToList();
                List<lkpFixedRuleType> lstFixedRuleType = LookupManager.GetSharedDBLookUpData<lkpFixedRuleType>().
                                                                                            Where(cond => !cond.FRLT_IsDeleted).ToList();
                List<lkpRuleActionType> lstRuleActionType = LookupManager.GetSharedDBLookUpData<lkpRuleActionType>().
                                                                                              Where(cond => !cond.ACT_IsDeleted).ToList();

                List<lkpRuleObjectMappingType> lstRuleObjectMappingType = LookupManager.GetSharedDBLookUpData<lkpRuleObjectMappingType>().
                                                                                             Where(cond => !cond.RMT_IsDeleted).ToList();


                String requiredObjectAttributeCode = ObjectAttribute.REQUIRED.GetStringValue();
                String signatureRequiredObjectAttributeCode = ObjectAttribute.SIGNATURE_REQUIRED.GetStringValue();
                String requiredToViewObjectAttributeCode = ObjectAttribute.REQUIRED_TO_VIEW.GetStringValue();
                String requiredToOpeAttributeCode = ObjectAttribute.REQUIRED_TO_OPEN.GetStringValue();
                String boxStayOpenObjectAttributeCode = ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue();
                lkpObjectAttribute requiredObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredObjectAttributeCode);
                lkpObjectAttribute signatureRequiredObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == signatureRequiredObjectAttributeCode);
                lkpObjectAttribute requiredToViewObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredToViewObjectAttributeCode);
                lkpObjectAttribute requiredToOpenObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredToOpeAttributeCode);
                lkpObjectAttribute boxStayOpenObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == boxStayOpenObjectAttributeCode);

                String attrObjTypeCode = ObjectType.Compliance_ATR.GetStringValue();
                String itmObjTypeCode = ObjectType.Compliance_Item.GetStringValue();
                String catObjTypeCode = ObjectType.Compliance_Category.GetStringValue();
                String pkgObjTypeCode = ObjectType.Compliance_Package.GetStringValue();
                lkpObjectType attrObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == attrObjTypeCode);
                lkpObjectType itmObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == itmObjTypeCode);
                lkpObjectType catObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == catObjTypeCode);
                lkpObjectType pkgObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == pkgObjTypeCode);

                lkpRuleType buisnessRuleType = LookupManager.GetSharedDBLookUpData<lkpRuleType>().FirstOrDefault(cond => cond.RLT_Code ==
                                                                                                        ComplianceRuleType.BuisnessRules.GetStringValue());

                lkpRuleType uiRuleType = LookupManager.GetSharedDBLookUpData<lkpRuleType>().FirstOrDefault(cond => cond.RLT_Code ==
                                                                                                      ComplianceRuleType.UIRules.GetStringValue());
                lkpRuleActionType defaultActionType = lstRuleActionType.FirstOrDefault(cond => cond.ACT_Code == "DFLT");
                lkpRuleActionType itemExpiratnActionType = lstRuleActionType.FirstOrDefault(cond => cond.ACT_Code == "SEXP");

                #endregion

                RequirementObjectTreeContract pkgRequirmntObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == pkgObjectType.OT_ID
                                                                                                            && cond.ObjectID == requirementPackage.RP_ID
                                                                                                            && cond.ParentObjectID == null);
                if (!pkgRequirmntObjTree.IsNullOrEmpty())
                {
                    RequirementObjectTree pkgRequirmntObjTreeFromDb = lstRequirmntObjTreeFromDb
                                                                        .FirstOrDefault(cond => cond.ROT_ID == pkgRequirmntObjTree.RequirementObjectTreeID);
                    CreateePackageObjectRule(requirementPackageContract, currentLoggedInUserID, requirementPackage, pkgRequirmntObjTree
                                            , pkgRequirmntObjTreeFromDb, lstFixedRuleType, pkgObjectType, buisnessRuleType, defaultActionType);
                }

                IEnumerable<RequirementPackageCategory> requirementPackageCategories = requirementPackage.RequirementPackageCategories;

                requirementPackageCategories.ForEach(rpc =>
                {
                    RequirementCategory requirementCatgry = rpc.RequirementCategory;
                    if (requirementCatgry.IsNotNull())
                    {
                        RequirementCategoryContract requirmntCatContractObj = requirementPackageContract.LstRequirementCategory
                                                                                                                .Where(cond => cond.RequirementCategoryCode == requirementCatgry.RC_Code)
                                                                                                                .FirstOrDefault();
                        if (!requirmntCatContractObj.IsNullOrEmpty() && (requirmntCatContractObj.IsNewCategory || requirmntCatContractObj.IsUpdated || isPackageVersionNeedToCreate || isCopy))
                        {
                            RequirementObjectTreeContract catRequirmntObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == catObjectType.OT_ID
                                                                                                  && cond.ObjectID == requirementCatgry.RC_ID
                                                                                                  && cond.ParentObjectID == requirementPackage.RP_ID);
                            if (!catRequirmntObjTree.IsNullOrEmpty())
                            {
                                RequirementObjectTree catRequirmntObjTreeFromDb = lstRequirmntObjTreeFromDb
                                                                                    .FirstOrDefault(cond => cond.ROT_ID == catRequirmntObjTree.RequirementObjectTreeID);


                                //Updating SQL expression for category Custom Rule
                                foreach (var reqItem in requirmntCatContractObj.LstRequirementItem)
                                {
                                    int reqItemId = reqItem.RequirementItemID;
                                    string reqItemCode = reqItem.RequirementItemCode.ToString();

                                    foreach (var reqcatItem in requirementCatgry.RequirementCategoryItems)
                                    {
                                        RequirementItem reqItemFromDB = reqcatItem.RequirementItem;

                                        if (reqItemFromDB.RI_Code.ToString() == reqItemCode)
                                        {
                                            int reqItemIdFromDB = reqItemFromDB.RI_ID;

                                            if (!requirmntCatContractObj.RuleSqlExpression.IsNullOrEmpty())
                                                requirmntCatContractObj.RuleSqlExpression = requirmntCatContractObj.RuleSqlExpression.Replace(reqItemId.ToString(), reqItemIdFromDB.ToString());

                                            if (!requirmntCatContractObj.RuleUIExpression.IsNullOrEmpty())
                                                requirmntCatContractObj.RuleUIExpression = requirmntCatContractObj.RuleUIExpression.Replace(reqItemId.ToString(), reqItemIdFromDB.ToString());
                                        }
                                    }
                                }

                                CreateCategoryRequirementObjectRule(currentLoggedInUserID, catRequirmntObjTree, requirmntCatContractObj, catRequirmntObjTreeFromDb
                                                                    , lstFixedRuleType, buisnessRuleType, defaultActionType);
                            }
                        }

                        IEnumerable<RequirementCategoryItem> reqrmntCatgryItems = requirementCatgry.RequirementCategoryItems;
                        reqrmntCatgryItems.ForEach(rci =>
                        {
                            RequirementItem requirmntItem = rci.RequirementItem;
                            IEnumerable<RequirementItemField> requirmntItmFields = requirmntItem.RequirementItemFields;
                            if (requirmntItem.IsNotNull())
                            {
                                RequirementItemContract requirementItemContractObj = requirementItemContractList.Where(cond => cond.RequirementItemCode
                                                                                                             == requirmntItem.RI_Code)
                                                                                                            .FirstOrDefault();
                                if (!requirementItemContractObj.IsNullOrEmpty() && (requirementItemContractObj.IsNewItem || requirementItemContractObj.IsUpdated || isPackageVersionNeedToCreate || isCopy))
                                {
                                    RequirementObjectTreeContract itmRequirmntObjTree = requirmntObjTreeCntrct
                                                                                       .FirstOrDefault(cond => cond.ObjectTypeID == itmObjectType.OT_ID
                                                                                           && cond.ObjectID == requirmntItem.RI_ID
                                                                                           && cond.ParentObjectID == rci.RCI_RequirementCategoryID);
                                    if (!itmRequirmntObjTree.IsNullOrEmpty())
                                    {
                                        RequirementObjectTree itmRequirmntObjTreeFromDb = lstRequirmntObjTreeFromDb
                                                                                          .FirstOrDefault(cond => cond.ROT_ID == itmRequirmntObjTree.RequirementObjectTreeID);

                                        CreateItemApprovalObjectRule(currentLoggedInUserID, itmRequirmntObjTree, lstFixedRuleType, buisnessRuleType
                                                                    , defaultActionType, requirementItemContractObj, itmRequirmntObjTreeFromDb);
                                        //expiration based rule.
                                        CreateItemExpirationObjectRule(currentLoggedInUserID, requirmntObjTreeCntrct, lstRuleObjectMappingType, lstFixedRuleType
                                                                       , attrObjectType, buisnessRuleType, itemExpiratnActionType
                                                                       , requirmntItem, requirmntItmFields, requirementItemContractObj, itmRequirmntObjTree
                                                                       , itmRequirmntObjTreeFromDb);
                                    }
                                }

                                requirmntItmFields.ForEach(rfi =>
                                {
                                    RequirementField requiremntField = rfi.RequirementField;
                                    if (requiremntField.IsNotNull())
                                    {
                                        RequirementFieldContract requirementFieldContractObj = requirementFieldContractList
                                                                                              .Where(cond => cond.RequirementFieldCode
                                                                                                             == requiremntField.RF_Code)
                                                                                              .FirstOrDefault();
                                        if (!requirementFieldContractObj.IsNullOrEmpty() && (requirementFieldContractObj.IsNewField || requirementFieldContractObj.IsUpdated || isPackageVersionNeedToCreate || isCopy))
                                        {
                                            RequirementObjectTreeContract attrRequirmntObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                        && cond.ObjectID == requiremntField.RF_ID
                                                                                                                        && cond.ParentObjectID == rfi.RIF_RequirementItemID);
                                            if (!attrRequirmntObjTree.IsNullOrEmpty())
                                            {
                                                RequirementObjectTree attrRequirmntObjTreeFromDb = lstRequirmntObjTreeFromDb.FirstOrDefault(cond => cond.ROT_ID == attrRequirmntObjTree.RequirementObjectTreeID);
                                                if (requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue()
                                                   || requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue()
                                                   || requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()
                                                   || requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue()
                                                    || requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                                                {
                                                    InsertUpdateRequiredObjectProperty(currentLoggedInUserID, requiredObjectAttribute, requirementFieldContractObj, attrRequirmntObjTree, attrRequirmntObjTreeFromDb);
                                                }

                                                //if (requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                                                //{
                                                //    if (attrRequirmntObjTree.IsNewRecordInserted)
                                                //    {
                                                //        InsertUpdateSignatureRequiredObjectProperty(currentLoggedInUserID, signatureRequiredObjectAttribute, requirementFieldContractObj, attrRequirmntObjTree, attrRequirmntObjTreeFromDb);

                                                //        InsertUpdateRequiredToViewObjectProperty(currentLoggedInUserID, requiredToViewObjectAttribute, requirementFieldContractObj, attrRequirmntObjTree, attrRequirmntObjTreeFromDb);
                                                //    }
                                                //}

                                                if (requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                                                {
                                                    RequirementFieldVideoData VideoFieldData = requirementFieldContractObj.RequirementFieldData.VideoFieldData;
                                                    if (!VideoFieldData.IsNullOrEmpty())
                                                    {
                                                        InsertUpdateRequiredToOpenObjectProperty(currentLoggedInUserID, requiredToOpenObjectAttribute, requirementFieldContractObj, attrRequirmntObjTree, attrRequirmntObjTreeFromDb, VideoFieldData);
                                                        if (!VideoFieldData.VideoOpenTimeDuration.IsNullOrEmpty())// && VideoFieldData.VideoOpenTimeDuration == AppConsts.NONE)
                                                        {
                                                            InsertUpdateBoxStayOpenObjectProperty(currentLoggedInUserID, boxStayOpenObjectAttribute, attrRequirmntObjTree, attrRequirmntObjTreeFromDb, VideoFieldData);
                                                        }
                                                    }
                                                }

                                                if (requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue()
                                                   && (requirementFieldContractObj.RequirementFieldUIRuleTypeCode == RequirementFixedRuleType.GREATER_THAN.GetStringValue()
                                                   || requirementFieldContractObj.RequirementFieldUIRuleTypeCode == RequirementFixedRuleType.LESS_THAN.GetStringValue()))
                                                {
                                                    var item = requirmntCatContractObj.LstRequirementItem.FirstOrDefault(sel => sel.RequirementItemID == requirementFieldContractObj.UiRequirementItemID);

                                                    if (!item.IsNullOrEmpty())
                                                    {
                                                        var field = item.LstRequirementField.FirstOrDefault(sel => sel.RequirementFieldID == requirementFieldContractObj.UiRequirementFieldID);
                                                        //RequirementFieldContract fieldContractObj = requirementFieldContractList
                                                        //                                     .Where(cond => cond.RequirementFieldCode == field.RequirementFieldCode)
                                                        //                                     .FirstOrDefault();
                                                        ////var item = fieldUsedInRule.RequirementItem;
                                                        //RequirementItemContract itemContractObj = requirementItemContractList
                                                        //                                    .Where(cond => cond.RequirementItemCode == item.RequirementItemCode)
                                                        //                                    .FirstOrDefault();

                                                        Int32 NewUiDataFieldId = AppConsts.NONE;
                                                        foreach (var reqcatItem in requirementCatgry.RequirementCategoryItems)
                                                        {
                                                            RequirementItem reqItemFromDB = reqcatItem.RequirementItem;

                                                            if (reqItemFromDB.RI_Code.ToString() == item.RequirementItemCode.ToString())
                                                            {
                                                                foreach (var reqField in reqItemFromDB.RequirementItemFields)
                                                                {
                                                                    RequirementField reqFieldFromDB = reqField.RequirementField;
                                                                    if (reqFieldFromDB.RF_Code.ToString() == field.RequirementFieldCode.ToString())
                                                                    {
                                                                        NewUiDataFieldId = reqFieldFromDB.RF_ID;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }


                                                        RequirementObjectTreeContract uiRuleDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                                          && cond.ObjectID == NewUiDataFieldId);
                                                        String ruleName = requirementFieldContractObj.RequirementFieldName + "_FieldUIRule";
                                                        //UAT-2366: Add the ability for "Item 1 must be dated before/after item 2" ui rules in rotation packages.
                                                        InsertUpdateFieldUiRule(currentLoggedInUserID, lstRuleObjectMappingType, lstFixedRuleType
                                                                           , uiRuleType, defaultActionType, ruleName, attrRequirmntObjTreeFromDb
                                                                            , requirementFieldContractObj, attrRequirmntObjTree, uiRuleDateObjTree);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                });
                            }
                        });
                    }
                });
                //if (!requirementObjectTreePropertyList.IsNullOrEmpty())
                //    BALUtils.GetSharedRequirementPackageRepoInstance(selectedTenantID).AddRequirementObjectTreePropertyToContext(requirementObjectTreePropertyList);
                //BALUtils.GetSharedRequirementPackageRepoInstance(selectedTenantID).AddRequirementObjectRuleToContext(requirementObjectRuleList);
                BALUtils.GetSharedRequirementPackageRepoInstance().SaveContextIntoDataBase();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <param name="currentUserID"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        private static List<RequirementObjectTreeContract> AddRequirementObjectTree(Int32 requirementPackageID, Int32 currentUserID)
        {
            try
            {

                String rpkgObjectTypeCode = ObjectType.Compliance_Package.GetStringValue();
                Int32 pkgObjectTypeId = LookupManager.GetSharedDBLookUpData<lkpObjectType>()
                                                            .Where(cond => cond.OT_Code == rpkgObjectTypeCode).FirstOrDefault().OT_ID;

                return BALUtils.GetSharedRequirementPackageRepoInstance().AddRequirementObjectTree(requirementPackageID, pkgObjectTypeId, currentUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static void CreateePackageObjectRule(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage requirementPackage,
                                                                                   RequirementObjectTreeContract pkgRequirmntObjTree, RequirementObjectTree pkgRequirmntObjTreeFromDb, List<lkpFixedRuleType> lstFixedRuleType,
                                                                                   lkpObjectType pkgObjectType, lkpRuleType buisnessRuleType, lkpRuleActionType defaultActionType)
        {
            //Package compliance rule.
            if (pkgRequirmntObjTree.IsNewRecordInserted)
            {
                //insert new rule.
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = requirementPackage.RP_PackageName + "_PackageComplianceRule";
                newObjectRule.ROR_ObjectTreeId = pkgRequirmntObjTree.RequirementObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = requirementPackageContract.PackageRuleTypeCode == "" ? 0 : lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementPackageContract.PackageRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                pkgRequirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                if (pkgRequirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    //update existing rule
                    RequirementObjectRule existingObjectRule = pkgRequirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted);
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementPackageContract.PackageRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                    }
                }
            }
        }

        private static void CreateCategoryRequirementObjectRule(Int32 currentLoggedInUserID, RequirementObjectTreeContract catRequirmntObjTree
                                                                , RequirementCategoryContract requirementcategoryContractObj
                                                                , RequirementObjectTree catRequirmntObjTreeFromDb
                                                                , List<lkpFixedRuleType> lstFixedRuleType, lkpRuleType buisnessRuleType
                                                                , lkpRuleActionType defaultActionType)
        {
            //Category Approval rule.
            if (catRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = requirementcategoryContractObj.RequirementCategoryName + "_CategoryApprovalRule";
                newObjectRule.ROR_ObjectTreeId = catRequirmntObjTree.RequirementObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementcategoryContractObj.CategoryRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                newObjectRule.ROR_SqlExpression = requirementcategoryContractObj.RuleSqlExpression;
                newObjectRule.ROR_UIExpression = requirementcategoryContractObj.RuleUIExpression;
                catRequirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                //update existing rule
                if (catRequirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    RequirementObjectRule existingObjectRule = catRequirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted);
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType
                                                               .FirstOrDefault(cond => cond.FRLT_Code == requirementcategoryContractObj.CategoryRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                        existingObjectRule.ROR_Name = requirementcategoryContractObj.RequirementCategoryName + "_CategoryApprovalRule";
                        existingObjectRule.ROR_SqlExpression = requirementcategoryContractObj.RuleSqlExpression;
                        existingObjectRule.ROR_UIExpression = requirementcategoryContractObj.RuleUIExpression;
                    }
                }
            }
        }

        private static void CreateItemApprovalObjectRule(Int32 currentLoggedInUserID, RequirementObjectTreeContract itmRequirmntObjTree
                                                        , List<lkpFixedRuleType> lstFixedRuleType
                                                        , lkpRuleType buisnessRuleType
                                                        , lkpRuleActionType defaultActionType
                                                        , RequirementItemContract requirementItemContractObj, RequirementObjectTree itmRequirmntObjTreeFromDb)
        {
            //Approval required rule.
            if (itmRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = requirementItemContractObj.RequirementItemName + "_ItemApprovalRule";
                newObjectRule.ROR_ObjectTreeId = itmRequirmntObjTree.RequirementObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementItemContractObj.RequirementItemRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                itmRequirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                //update existing rule
                if (itmRequirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    List<String> lstItemRuleTypeCodes = new List<String>();
                    lstItemRuleTypeCodes.Add(RequirementFixedRuleType.ENTERED.GetStringValue());
                    lstItemRuleTypeCodes.Add(RequirementFixedRuleType.ENTERED_AND_APPROVED.GetStringValue());
                    RequirementObjectRule existingObjectRule = itmRequirmntObjTreeFromDb.RequirementObjectRules
                                                                                        .FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                            && lstItemRuleTypeCodes.Contains(cond.lkpFixedRuleType.FRLT_Code));
                    //&& cond.RequirementObjectRuleDetails == null);
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType
                                                               .FirstOrDefault(cond => cond.FRLT_Code == requirementItemContractObj.RequirementItemRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                        existingObjectRule.ROR_Name = requirementItemContractObj.RequirementItemName + "_ItemApprovalRule";
                    }
                }
            }
        }

        private static void CreateItemExpirationObjectRule(Int32 currentLoggedInUserID, List<RequirementObjectTreeContract> requirmntObjTreeCntrct, List<lkpRuleObjectMappingType> lstRuleObjectMappingType,
                                                                                        List<lkpFixedRuleType> lstFixedRuleType, lkpObjectType attrObjectType, lkpRuleType buisnessRuleType, lkpRuleActionType itemExpiratnActionType,
                                                                                        RequirementItem requirmntItem, IEnumerable<RequirementItemField> requirmntItmFields, RequirementItemContract requirementItemContractObj,
                                                                                        RequirementObjectTreeContract itmRequirmntObjTree, RequirementObjectTree itmRequirmntObjTreeFromDb)
        {
            lkpRuleObjectMappingType constMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "CONST");
            lkpRuleObjectMappingType dataValueMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "DVAL");

            RequirementObjectRule existingObjectRule = itmRequirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                                       && cond.ROR_ActionType == itemExpiratnActionType.ACT_ID);
            String dateConstantTypeCode = ConstantType.Date.GetStringValue();
            lkpConstantType dateConstantType = LookupManager.GetSharedDBLookUpData<lkpConstantType>().
                                                                                            Where(cond => !cond.IsDeleted).FirstOrDefault();
            if (requirementItemContractObj.IsRequirementItemNeededExpiration && !requirementItemContractObj.RequirementItemExpiration.IsNullOrEmpty())
            {
                if (itmRequirmntObjTree.IsNewRecordInserted || existingObjectRule.IsNullOrEmpty())
                {
                    InsertItemExpirationObjectRule(currentLoggedInUserID, requirmntObjTreeCntrct, constMappingType, lstFixedRuleType, attrObjectType, buisnessRuleType,
                        itemExpiratnActionType, requirmntItem, requirmntItmFields, requirementItemContractObj, itmRequirmntObjTree, itmRequirmntObjTreeFromDb, dateConstantType, dataValueMappingType);
                }
                else
                {
                    String exitingFixedRuleTypeCode = existingObjectRule.lkpFixedRuleType.FRLT_Code;

                    existingObjectRule.ROR_Name = requirementItemContractObj.RequirementItemName + "_ItemExpirationRule";
                    existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode).FRLT_ID;
                    existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                    existingObjectRule.ROR_ModifiedOn = DateTime.Now;

                    List<RequirementObjectRuleDetail> requirementObjectRuleDetailList = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).ToList();
                    if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
                        == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
                    {
                        RequirementObjectRuleDetail requirementObjectRuleDetail = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                        requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                        requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationDate;
                        requirementObjectRuleDetail.RORD_ObjectTreeID = null;
                        requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                        if (exitingFixedRuleTypeCode != requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode)
                        {
                            RequirementObjectRuleDetail requirementObjectRuleDetailToBeDeleted = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            requirementObjectRuleDetailToBeDeleted.RORD_IsDeleted = true;
                            requirementObjectRuleDetailToBeDeleted.RORD_ModifiedByID = currentLoggedInUserID;
                            requirementObjectRuleDetailToBeDeleted.RORD_ModifiedOn = DateTime.Now;
                        }
                    }
                    else if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
                                == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
                    {
                        //UAT-2165
                        RequirementObjectRuleDetail requirementObjectRuleDetail = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                        requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                        requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationDate;
                        requirementObjectRuleDetail.RORD_ExpirationCondStartDate = requirementItemContractObj.RequirementItemExpiration.ExpirationCondStartDate;
                        requirementObjectRuleDetail.RORD_ExpirationCondEndDate = requirementItemContractObj.RequirementItemExpiration.ExpirationCondEndDate;
                        RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_Code == requirementItemContractObj.RequirementItemExpiration.SelectedDateTypeFieldCode)
                                                                                                                                                                    .FirstOrDefault().RequirementField;
                        if (exprationDateField.IsNotNull())
                        {
                            RequirementObjectTreeContract exprationDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                          && cond.ObjectID == exprationDateField.RF_ID
                                                                                                                          && cond.ParentObjectID == requirmntItem.RI_ID);
                            requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.RequirementObjectTreeID;
                        }
                        requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                        if (exitingFixedRuleTypeCode != requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode)
                        {
                            RequirementObjectRuleDetail requirementObjectRuleDetailToBeDeleted = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            requirementObjectRuleDetailToBeDeleted.RORD_IsDeleted = true;
                            requirementObjectRuleDetailToBeDeleted.RORD_ModifiedByID = currentLoggedInUserID;
                            requirementObjectRuleDetailToBeDeleted.RORD_ModifiedOn = DateTime.Now;
                        }
                    }
                    else
                    {
                        RequirementObjectRuleDetail requirementObjectRuleDetailForDataValue = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetailForDataValue.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                        requirementObjectRuleDetailForDataValue.RORD_ConstantType = null;
                        requirementObjectRuleDetailForDataValue.RORD_ConstantValue = null;
                        requirementObjectRuleDetailForDataValue.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailForDataValue.RORD_ModifiedOn = DateTime.Now;
                        RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_Code == requirementItemContractObj.RequirementItemExpiration.SelectedDateTypeFieldCode)
                                                                                                                                                                    .FirstOrDefault().RequirementField;
                        if (exprationDateField.IsNotNull())
                        {
                            RequirementObjectTreeContract exprationDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                          && cond.ObjectID == exprationDateField.RF_ID
                                                                                                                          && cond.ParentObjectID == requirmntItem.RI_ID);
                            requirementObjectRuleDetailForDataValue.RORD_ObjectTreeID = exprationDateObjTree.RequirementObjectTreeID;
                        }
                        RequirementObjectRuleDetail requirementObjectRuleDetailForContantValue;
                        if (exitingFixedRuleTypeCode != requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode)
                        {
                            requirementObjectRuleDetailForContantValue = new RequirementObjectRuleDetail();
                            requirementObjectRuleDetailForContantValue.RORD_ParameterOrder = AppConsts.TWO;
                            requirementObjectRuleDetailForContantValue.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantType = requirementItemContractObj.RequirementItemExpiration.ExpirationValueTypeID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationValue.ToString();
                            requirementObjectRuleDetailForContantValue.RORD_IsDeleted = false;
                            requirementObjectRuleDetailForContantValue.RORD_CreatedByID = currentLoggedInUserID;
                            requirementObjectRuleDetailForContantValue.RORD_CreatedOn = DateTime.Now;
                            existingObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetailForContantValue);
                        }
                        else
                        {
                            requirementObjectRuleDetailForContantValue = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            requirementObjectRuleDetailForContantValue.RORD_ParameterOrder = AppConsts.TWO;
                            requirementObjectRuleDetailForContantValue.RORD_ModifiedByID = currentLoggedInUserID;
                            requirementObjectRuleDetailForContantValue.RORD_ModifiedOn = DateTime.Now;
                            requirementObjectRuleDetailForContantValue.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantType = requirementItemContractObj.RequirementItemExpiration.ExpirationValueTypeID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationValue.ToString();
                        }
                    }
                }
            }
            if (!requirementItemContractObj.IsRequirementItemNeededExpiration && !existingObjectRule.IsNullOrEmpty())
            {
                //if expiration is not needed and rule already exist for expiration.
                existingObjectRule.ROR_IsDeleted = true;
                existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                List<RequirementObjectRuleDetail> requirementObjectRuleDetailList = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted)
                                                                                                                                               .ToList();
                foreach (RequirementObjectRuleDetail requirementObjectRuleDetail in requirementObjectRuleDetailList)
                {
                    requirementObjectRuleDetail.RORD_IsDeleted = true;
                    requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                    requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                }
            }

        }

        private static void InsertItemExpirationObjectRule(Int32 currentLoggedInUserID, List<RequirementObjectTreeContract> requirmntObjTreeCntrct, lkpRuleObjectMappingType constMappingType, List<lkpFixedRuleType> lstFixedRuleType,
                                                                                        lkpObjectType attrObjectType, lkpRuleType buisnessRuleType, lkpRuleActionType itemExpiratnActionType, RequirementItem requirmntItem,
                                                                                        IEnumerable<RequirementItemField> requirmntItmFields, RequirementItemContract requirementItemContractObj,
                                                                                        RequirementObjectTreeContract itmRequirmntObjTree, RequirementObjectTree itmRequirmntObjTreeFromDb, lkpConstantType dateConstantType
                                                                                        , lkpRuleObjectMappingType dataValueMappingType)
        {
            RequirementObjectRule newExpirationObjectRule = new RequirementObjectRule();
            newExpirationObjectRule.ROR_RuleTemplateID = null;
            newExpirationObjectRule.ROR_Code = Guid.NewGuid();
            newExpirationObjectRule.ROR_Name = requirementItemContractObj.RequirementItemName + "_ItemExpirationRule";
            newExpirationObjectRule.ROR_ObjectTreeId = itmRequirmntObjTree.RequirementObjectTreeID;
            newExpirationObjectRule.ROR_RuleTemplateID = null;
            newExpirationObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode).FRLT_ID;
            newExpirationObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
            newExpirationObjectRule.ROR_ActionType = itemExpiratnActionType.ACT_ID;
            newExpirationObjectRule.ROR_FirstVersionID = null;
            newExpirationObjectRule.ROR_IsCurrent = true;
            newExpirationObjectRule.ROR_IsActive = true;
            newExpirationObjectRule.ROR_IsDeleted = false;
            newExpirationObjectRule.ROR_CreatedByID = currentLoggedInUserID;
            newExpirationObjectRule.ROR_CreatedOn = DateTime.Now;

            RequirementObjectRuleDetail requirementObjectRuleDetail = new RequirementObjectRuleDetail();
            requirementObjectRuleDetail.RORD_PlaceHolderName = null;
            requirementObjectRuleDetail.RORD_ObjectTreeID = null;
            requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
            if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
                           == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
            {
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationDate;
            }
            else if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
                       == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
            {
                //UAT-2165
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationDate;
                requirementObjectRuleDetail.RORD_ExpirationCondStartDate = requirementItemContractObj.RequirementItemExpiration.ExpirationCondStartDate;
                requirementObjectRuleDetail.RORD_ExpirationCondEndDate = requirementItemContractObj.RequirementItemExpiration.ExpirationCondEndDate;
                RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_Code == requirementItemContractObj.RequirementItemExpiration.SelectedDateTypeFieldCode)
                                                                                                                                                        .FirstOrDefault().RequirementField;
                if (exprationDateField.IsNotNull())
                {
                    RequirementObjectTreeContract exprationDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                                           && cond.ObjectID == exprationDateField.RF_ID
                                                                                                                                           && cond.ParentObjectID == requirmntItem.RI_ID);
                    requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.RequirementObjectTreeID;
                }
            }
            else
            {
                requirementObjectRuleDetail.RORD_ParameterOrder = 2;
                requirementObjectRuleDetail.RORD_ConstantType = requirementItemContractObj.RequirementItemExpiration.ExpirationValueTypeID;
                requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationValue.ToString();
            }
            requirementObjectRuleDetail.RORD_IsDeleted = false;
            requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
            requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;
            newExpirationObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);

            if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
               == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue())
            {
                requirementObjectRuleDetail = new RequirementObjectRuleDetail();
                requirementObjectRuleDetail.RORD_PlaceHolderName = null;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_ConstantType = null;
                requirementObjectRuleDetail.RORD_ConstantValue = null;
                requirementObjectRuleDetail.RORD_ObjectTreeID = null;
                requirementObjectRuleDetail.RORD_IsDeleted = false;
                requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
                requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;

                RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_Code == requirementItemContractObj.RequirementItemExpiration.SelectedDateTypeFieldCode)
                .FirstOrDefault().RequirementField;
                if (exprationDateField.IsNotNull())
                {
                    RequirementObjectTreeContract exprationDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                                           && cond.ObjectID == exprationDateField.RF_ID
                                                                                                                                           && cond.ParentObjectID == requirmntItem.RI_ID);
                    requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.RequirementObjectTreeID;
                }
                newExpirationObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);
            }
            itmRequirmntObjTreeFromDb.RequirementObjectRules.Add(newExpirationObjectRule);
        }

        private static void InsertUpdateRequiredObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute requiredObjectAttribute, RequirementFieldContract requirementFieldContractObj, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                newRequirementObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredObjectAttribute.IsNotNull() ? requiredObjectAttribute.OA_ID : AppConsts.NONE;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
            }
            else
            {
                RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.
                                                                                                        FirstOrDefault(cond => !cond.ROTP_IsDeleted);
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredObjectAttribute.IsNotNull() ? requiredObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateSignatureRequiredObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute signatureRequiredObjectAttribute, RequirementFieldContract requirementFieldContractObj, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            if (requirementFieldContractObj.RequirementFieldData.FieldViewDocumentData.LstDocumentAcroFieldTypeCodes.IsNotNull()
               && requirementFieldContractObj.RequirementFieldData.FieldViewDocumentData.LstDocumentAcroFieldTypeCodes.
                                                                                Contains(DocumentAcroFieldType.SIGNATURE.GetStringValue()))
            {
                if (attrRequirmntObjTree.IsNewRecordInserted)
                {
                    RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                    newRequirementObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                    newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                    newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                    newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                    newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = signatureRequiredObjectAttribute.IsNotNull() ?
                                                                            signatureRequiredObjectAttribute.OA_ID : AppConsts.NONE;
                    newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                    attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
                }
                else
                {
                    RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                            && cond.ROTP_ObjectAttributeID == signatureRequiredObjectAttribute.OA_ID);
                    existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = signatureRequiredObjectAttribute.IsNotNull() ?
                                                                            signatureRequiredObjectAttribute.OA_ID : AppConsts.NONE;
                    existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                    existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                    existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
                }
            }
        }

        private static void InsertUpdateRequiredToViewObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute requiredToViewObjectAttribute, RequirementFieldContract requirementFieldContractObj, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectTreeProperty requiredToViewObjectTreeProperty = new RequirementObjectTreeProperty();
                requiredToViewObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                requiredToViewObjectTreeProperty.ROTP_IsDeleted = false;
                requiredToViewObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                requiredToViewObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                requiredToViewObjectTreeProperty.ROTP_ObjectAttributeID = requiredToViewObjectAttribute.IsNotNull() ?
                                                                           requiredToViewObjectAttribute.OA_ID : AppConsts.NONE;
                requiredToViewObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(requiredToViewObjectTreeProperty);
            }
            else
            {
                RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                        && cond.ROTP_ObjectAttributeID == requiredToViewObjectAttribute.OA_ID);
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredToViewObjectAttribute.IsNotNull() ?
                                                                           requiredToViewObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateBoxStayOpenObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute boxStayOpenObjectAttribute, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb, RequirementFieldVideoData VideoFieldData)
        {
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                newRequirementObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = boxStayOpenObjectAttribute.IsNotNull() ?
                                                                           boxStayOpenObjectAttribute.OA_ID : AppConsts.NONE;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = VideoFieldData.VideoOpenTimeDuration.ToString();
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
            }
            else
            {
                RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                        && cond.ROTP_ObjectAttributeID == boxStayOpenObjectAttribute.OA_ID);
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = boxStayOpenObjectAttribute.IsNotNull() ?
                                                                       boxStayOpenObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = VideoFieldData.VideoOpenTimeDuration.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateRequiredToOpenObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute requiredToOpenObjectAttribute, RequirementFieldContract requirementFieldContractObj, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb, RequirementFieldVideoData VideoFieldData)
        {
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                newRequirementObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredToOpenObjectAttribute.IsNotNull() ? requiredToOpenObjectAttribute.OA_ID : AppConsts.NONE;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = VideoFieldData.IsVideoRequiredToBeOpened.ToString();
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
            }
            else
            {
                RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                        && cond.ROTP_ObjectAttributeID == requiredToOpenObjectAttribute.OA_ID);
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredToOpenObjectAttribute.IsNotNull() ?
                                                                           requiredToOpenObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = VideoFieldData.IsVideoRequiredToBeOpened.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }


        private static void InsertUpdateFieldUiRule(Int32 currentLoggedInUserID, List<lkpRuleObjectMappingType> lstRuleObjectMappingType,
                                                List<lkpFixedRuleType> lstFixedRuleType, lkpRuleType uiRuleType, lkpRuleActionType defaultActionType,
                                                String ruleName, RequirementObjectTree requirmntObjTreeFromDb, RequirementFieldContract requirementFieldContractObj
                                                 , RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTreeContract uiRuleDateObjTree)
        {
            lkpRuleObjectMappingType dataValueMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "DVAL");
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                //insert new rule.
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = ruleName;
                newObjectRule.ROR_ObjectTreeId = attrRequirmntObjTree.RequirementObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementFieldContractObj.RequirementFieldUIRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = uiRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                newObjectRule.ROR_ErroMessage = requirementFieldContractObj.UiRuleErrorMessage;

                RequirementObjectRuleDetail requirementObjectRuleDetail = new RequirementObjectRuleDetail();
                requirementObjectRuleDetail.RORD_PlaceHolderName = null;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                requirementObjectRuleDetail.RORD_ObjectTreeID = uiRuleDateObjTree.RequirementObjectTreeID;
                requirementObjectRuleDetail.RORD_IsDeleted = false;
                requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
                requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;
                newObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);
                requirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                if (requirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    //update existing rule
                    RequirementObjectRule existingObjectRule = requirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                            && (cond.lkpFixedRuleType.FRLT_Code == RequirementFixedRuleType.LESS_THAN.GetStringValue()
                                                                                            || cond.lkpFixedRuleType.FRLT_Code == RequirementFixedRuleType.GREATER_THAN.GetStringValue()));
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementFieldContractObj.RequirementFieldUIRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ErroMessage = requirementFieldContractObj.UiRuleErrorMessage;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;

                        RequirementObjectRuleDetail requirementObjectRuleDetailInDb = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).FirstOrDefault();
                        requirementObjectRuleDetailInDb.RORD_PlaceHolderName = null;
                        requirementObjectRuleDetailInDb.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                        requirementObjectRuleDetailInDb.RORD_ObjectTreeID = uiRuleDateObjTree.RequirementObjectTreeID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedOn = DateTime.Now;
                    }
                }
            }
        }

        #endregion

        public static Boolean IsPackageVersionNeedToCreate(Int32 requirementPackageID)
        {
            if (requirementPackageID == AppConsts.NONE)
            {
                return false;
            }
            return BALUtils.GetSharedRequirementPackageRepoInstance().IsPackageVersionNeedToCreate(requirementPackageID);
        }

        private static Int32 AddNewRequirementPackage(RequirementPackageContract requirementPackageContract
                                                       , Int32 currentLoggedInUserID, Boolean isPackageVersionNeedToCreate, Boolean isPackageMappedToRotation = false, Boolean isCopy = false)
        {
            #region Add data into RequirementPackage table

            RequirementPackage requirementPackage = new RequirementPackage();
            CreateRequirementPackage(requirementPackageContract, currentLoggedInUserID, isPackageVersionNeedToCreate, requirementPackage, isPackageMappedToRotation, isCopy);


            //CreateRqrmntPkgAgency(requirementPackageContract, currentLoggedInUserID, requirementPackage);

            //Add entries in table RequirementPackageInstitution. N rows will be inserted for N selected tenants
            CreateRqrmntPkgInstitution(requirementPackageContract, currentLoggedInUserID, requirementPackage);
            //Add entries in table RequirementPackageAgency. N rows will be inserted for N selected agencies
            //CreateRqrmntPkgAgency(requirementPackageContract, currentLoggedInUserID, requirementPackage);

            //Method to map AgencyHierarchyPacakge.
            CreateAgencyHierarchyPkg(requirementPackageContract, currentLoggedInUserID, requirementPackage);

            #endregion

            #region Add data into mapping,detail tables table

            List<RequirementFieldContract> requirementFieldContractList = new List<RequirementFieldContract>();
            List<RequirementItemContract> requirementItemContractList = new List<RequirementItemContract>();

            foreach (RequirementCategoryContract categoryContract in requirementPackageContract.LstRequirementCategory.Where(cond => !cond.IsDeleted))
            {
                RequirementPackageCategory reqrmntPackageCategory = GetRequirementPackageCategory(currentLoggedInUserID, requirementPackage
                                                                                                 , requirementFieldContractList, requirementItemContractList
                                                                                                 , categoryContract);
                requirementPackage.RequirementPackageCategories.Add(reqrmntPackageCategory);
            }

            //Code to SAVE RequirementPackageCategory AND RequirementPackage into DB
            BALUtils.GetSharedRequirementPackageRepoInstance().AddRequirementPackageToDatabase(requirementPackage);

            #endregion

            #region Add category explanatory notes

            if (requirementPackage.RP_ID > AppConsts.NONE)
            {
                List<RequirementCategory> addedreqrmntCatgoriesList = requirementPackage.RequirementPackageCategories.Select(col => col.RequirementCategory).ToList();

                List<LargeContent> largeContentList = new List<LargeContent>();

                foreach (RequirementCategory requirementcategory in addedreqrmntCatgoriesList)
                {
                    String explanatoryNotes = requirementPackageContract.LstRequirementCategory
                                                .Where(cond => cond.RequirementCategoryCode == requirementcategory.RC_Code).FirstOrDefault().ExplanatoryNotes;
                    if (!explanatoryNotes.IsNullOrEmpty())
                    {
                        LargeContent largeContent = CreateLargeContentForReqrmntCategory(requirementcategory.RC_ID, explanatoryNotes, currentLoggedInUserID);
                        largeContentList.Add(largeContent);
                    }
                }

                if (!largeContentList.IsNullOrEmpty())
                {
                    BALUtils.GetSharedRequirementPackageRepoInstance().AddLargeContentToDatabase(largeContentList);
                }
            }

            #endregion


            if (requirementPackage.RP_ID > AppConsts.NONE)
            {
                InsertDataIntoRequirementObjectTree(requirementPackageContract, currentLoggedInUserID, requirementPackage, requirementFieldContractList, requirementItemContractList, isPackageVersionNeedToCreate, isCopy);
            }

            return requirementPackage.RP_ID;
        }

        private static RequirementPackageCategory GetRequirementPackageCategory(Int32 currentLoggedInUserID
                                                                                , RequirementPackage requirementPackage
                                                                                , List<RequirementFieldContract> requirementFieldContractList
                                                                                , List<RequirementItemContract> requirementItemContractList
                                                                                , RequirementCategoryContract categoryContract)
        {
            //Add data into category and package category mapping tables
            //Add new category
            RequirementCategory requirementCategory = CreateRequrmntCategory(currentLoggedInUserID, categoryContract);

            foreach (RequirementItemContract itemContract in categoryContract.LstRequirementItem.Where(cond => !cond.IsDeleted))
            {
                requirementItemContractList.Add(itemContract);
                RequirementCategoryItem requrmntCategoryItem = GetRequirementCategoryItem(currentLoggedInUserID
                                                                                           , requirementFieldContractList
                                                                                           , requirementCategory, itemContract);

                requirementCategory.RequirementCategoryItems.Add(requrmntCategoryItem);
            }

            //Add new package category mapping
            RequirementPackageCategory reqrmntPackageCategory = CreateRequrmntPackageCategory(currentLoggedInUserID, requirementPackage, requirementCategory);
            #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            //Added ComplianceRequired Setting in RequirementPackageCategory Table.
            reqrmntPackageCategory.RPC_ComplianceRequired = categoryContract.IsComplianceRequired;
            reqrmntPackageCategory.RPC_ComplianceRqdStartDate = categoryContract.ComplianceReqStartDate;
            reqrmntPackageCategory.RPC_ComplianceRqdEndDate = categoryContract.ComplianceReqEndDate;
            #endregion
            return reqrmntPackageCategory;
        }

        private static RequirementCategoryItem GetRequirementCategoryItem(Int32 currentLoggedInUserID
                                                                          , List<RequirementFieldContract> requirementFieldContractList
                                                                          , RequirementCategory requirementCategory, RequirementItemContract itemContract)
        {
            //Add new Item
            RequirementItem requirementItem = CreateRequirementItem(currentLoggedInUserID, itemContract);

            #region Define common variables and get lkp values

            List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType = LookupManager.GetSharedDBLookUpData<lkpDocumentAcroFieldType>().ToList();

            String requirementFieldViewDocumentTypeCode = DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue();
            Int32 requirementFieldViewDocumentTypeID = LookupManager.GetSharedDBLookUpData<lkpDocumentType>()
                                                        .Where(cond => cond.DMT_Code == requirementFieldViewDocumentTypeCode).FirstOrDefault().DMT_ID;

            List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType = LookupManager.GetSharedDBLookUpData<lkpRequirementFieldDataType>().ToList();

            #endregion

            //Add data into field and item field mapping tables
            foreach (RequirementFieldContract fieldContract in itemContract.LstRequirementField.Where(cond => !cond.IsDeleted))
            {
                RequirementItemField requrmntItemField = GetRequirementItemField(currentLoggedInUserID, requirementFieldContractList
                                                                                , requirementItem, lstLkpDocumentAcroFieldType, requirementFieldViewDocumentTypeID
                                                                                , lstLkpRequirementFieldDataType, fieldContract);
                requirementItem.RequirementItemFields.Add(requrmntItemField);
            }

            //Add new category item mapping
            RequirementCategoryItem requrmntCategoryItem = CreateRequrmntCategoryItem(currentLoggedInUserID, requirementCategory, requirementItem, itemContract);
            return requrmntCategoryItem;
        }

        private static RequirementItemField GetRequirementItemField(Int32 currentLoggedInUserID, List<RequirementFieldContract> requirementFieldContractList, RequirementItem requirementItem, List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType, Int32 requirementFieldViewDocumentTypeID, List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType, RequirementFieldContract fieldContract)
        {
            requirementFieldContractList.Add(fieldContract);

            //Add new field
            RequirementField requirementField = CreateRequirementField(currentLoggedInUserID, requirementFieldViewDocumentTypeID,
                                                                       lstLkpRequirementFieldDataType, lstLkpDocumentAcroFieldType, fieldContract);

            //Add new item field mapping
            RequirementItemField requrmntItemField = CreateRequirementItemField(currentLoggedInUserID, requirementItem, requirementField, fieldContract.IsBackgroundDocument, fieldContract.RequirementFieldDisplayOrder);
            return requrmntItemField;
        }

        private static RequirementField CreateRequirementField(Int32 currentLoggedInUserID, Int32 requirementFieldViewDocumentTypeID
                                                               , List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType
                                                               , List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType, RequirementFieldContract fieldContract)
        {
            RequirementField requirementField = new RequirementField();

            requirementField.RF_FieldName = fieldContract.RequirementFieldName;
            if (!fieldContract.RequirementFieldLabel.IsNullOrEmpty())
            {
                requirementField.RF_FieldLabel = fieldContract.RequirementFieldLabel;
            }
            requirementField.RF_AttributeTypeID = fieldContract.AttributeTypeID;
            requirementField.RF_RequirementAttributeGroupID = fieldContract.RequirementFieldAttributeGroupId; //UAT-3176
            //if (!fieldContract.RequirementFieldDescription.IsNullOrEmpty())
            //{
            //    requirementField.RF_Description = fieldContract.RequirementFieldDescription;
            //}
            requirementField.RF_CreatedByID = currentLoggedInUserID;
            requirementField.RF_CreatedOn = DateTime.Now;
            requirementField.RF_IsDeleted = false;
            requirementField.RF_FieldDataTypeID = lstLkpRequirementFieldDataType
                                                    .Where(cond => cond.RFDT_Code == fieldContract.RequirementFieldData.RequirementFieldDataTypeCode)
                                                    .FirstOrDefault().RFDT_ID;

            requirementField.RF_Code = fieldContract.RequirementFieldCode;

            if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue()) //UAT-2701
            {
                requirementField.RF_MaximumCharacters = fieldContract.RequirementFieldMaxLength;
            }
            //code to save field data values
            if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
            {
                RequirementFieldVideo requirementFieldVideo = CreateReqirmntFieldVideo(currentLoggedInUserID, fieldContract);
                requirementField.RequirementFieldVideos.Add(requirementFieldVideo);
            }
            else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
            {
                AddReqrmntFieldOptionsForReqrmntField(currentLoggedInUserID, fieldContract, requirementField);
            }
            else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
            {
                RequirementFieldDocument requrmntFieldDocument = CreateRequirementFieldDocumentType(currentLoggedInUserID, requirementFieldViewDocumentTypeID
                                                                                                     , lstLkpDocumentAcroFieldType, fieldContract);
                requirementField.RequirementFieldDocuments.Add(requrmntFieldDocument);
            }
            return requirementField;
        }

        private static Int32 UpdateRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID)
        {
            List<RequirementFieldContract> requirementFieldContractList = new List<RequirementFieldContract>();
            List<RequirementItemContract> requirementItemContractList = new List<RequirementItemContract>();

            //Get existing requirement package(from DB) which is to be updated 
            RequirementPackage existingRequirementPackage = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                    .GetRequirementPackageByPackageID(requirementPackageContract.RequirementPackageID);

            //if package does not exists
            if (existingRequirementPackage.IsNullOrEmpty())
            {
                return 0;
            }
            else if (requirementPackageContract.IsUpdated)
            {
                existingRequirementPackage.RP_PackageName = requirementPackageContract.RequirementPackageName;
                existingRequirementPackage.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
                existingRequirementPackage.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
                existingRequirementPackage.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;
                existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
                //Save Update AgencyHirarchyPackage Data
                BALUtils.GetSharedRequirementPackageRepoInstance().SaveUpdateAgencyHierarchyPackage(requirementPackageContract.LstAgencyHierarchyIDs.Distinct().ToList(), existingRequirementPackage, currentLoggedInUserID);

                UpdateReqrmntPackageAgency(requirementPackageContract, currentLoggedInUserID, existingRequirementPackage);
                UpdateReqrmntPackageInstitutions(requirementPackageContract, currentLoggedInUserID, existingRequirementPackage);
                BALUtils.GetSharedRequirementPackageRepoInstance().SaveContextIntoDataBase();
            }
            else
            {
                #region Define common variables and get lkp values

                String requirementFieldViewDocumentTypeCode = DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue();

                Int32 requirementFieldViewDocumentTypeID = LookupManager.GetSharedDBLookUpData<lkpDocumentType>()
                                                            .Where(cond => cond.DMT_Code == requirementFieldViewDocumentTypeCode).FirstOrDefault().DMT_ID;

                List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType = LookupManager.GetSharedDBLookUpData<lkpRequirementFieldDataType>().ToList();

                List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType = LookupManager.GetSharedDBLookUpData<lkpDocumentAcroFieldType>().ToList();

                #endregion

                foreach (RequirementCategoryContract categoryContract in requirementPackageContract.LstRequirementCategory)
                {
                    //if category is newly added
                    if (categoryContract.IsNewCategory)
                    {
                        RequirementPackageCategory reqrmntPackageCategory = GetRequirementPackageCategory(currentLoggedInUserID, existingRequirementPackage
                                                                                                     , requirementFieldContractList, requirementItemContractList
                                                                                                     , categoryContract);

                        existingRequirementPackage.RequirementPackageCategories.Add(reqrmntPackageCategory);
                    }
                    else
                    {
                        RequirementCategory existingRequrmntCategory = existingRequirementPackage.RequirementPackageCategories
                                                                                         .Where(cond => cond.RPC_ID == categoryContract.RequirementPackageCategoryID && !cond.RPC_IsDeleted)
                                                                                         .Select(col => col.RequirementCategory).FirstOrDefault();

                        if (categoryContract.IsUpdated)
                        {
                            //code to update category table objecte
                            existingRequrmntCategory.RC_CategoryName = categoryContract.RequirementCategoryName;
                            //existingRequrmntCategory.RC_CategoryLabel = categoryContract.RequirementCategoryLabel;
                            //existingRequrmntCategory.RC_Description = categoryContract.RequirementCategoryDescription;
                            existingRequrmntCategory.RC_ModifiedByID = currentLoggedInUserID;
                            existingRequrmntCategory.RC_ModifiedOn = DateTime.Now;
                            //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
                            if (!categoryContract.RequirementDocumentLink.IsNullOrEmpty())
                            {
                                existingRequrmntCategory.RC_SampleDocFormURL = categoryContract.RequirementDocumentLink;
                            }
                            else
                            {
                                existingRequrmntCategory.RC_SampleDocFormURL = null;
                            }
                            //UAT-3161
                            if (!categoryContract.RequirementDocumentLinkLabel.IsNullOrEmpty())
                            {
                                existingRequrmntCategory.RC_SampleDocFormUrlLabel = categoryContract.RequirementDocumentLinkLabel;
                            }
                            else
                            {
                                existingRequrmntCategory.RC_SampleDocFormUrlLabel = null;
                            }

                            UpdateLargeContentForReqrmntCategory(existingRequrmntCategory.RC_ID, categoryContract.ExplanatoryNotes, currentLoggedInUserID);

                            #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                            //Updated the ComplianceRequired Setting on Update of Category Data.
                            RequirementPackageCategory requirementPackageCategoryToUpdate = existingRequrmntCategory.RequirementPackageCategories.Where(cond => cond.RPC_RequirementCategoryID == categoryContract.RequirementCategoryID
                                                                                                                                                        && cond.RPC_RequirementPackageID == requirementPackageContract.RequirementPackageID
                                                                                                                                                        && !cond.RPC_IsDeleted).FirstOrDefault();
                            if (!requirementPackageCategoryToUpdate.IsNullOrEmpty())
                            {
                                requirementPackageCategoryToUpdate.RPC_ComplianceRequired = categoryContract.IsComplianceRequired;
                                requirementPackageCategoryToUpdate.RPC_ComplianceRqdStartDate = categoryContract.ComplianceReqStartDate;
                                requirementPackageCategoryToUpdate.RPC_ComplianceRqdEndDate = categoryContract.ComplianceReqEndDate;
                                requirementPackageCategoryToUpdate.RPC_ModifiedByID = currentLoggedInUserID;
                                requirementPackageCategoryToUpdate.RPC_ModifiedOn = DateTime.Now;
                            }
                            #endregion

                            break;
                        }
                        else
                        {
                            foreach (RequirementItemContract itemContract in categoryContract.LstRequirementItem)
                            {
                                //code to add items,its fields and fields data into database
                                requirementItemContractList.Add(itemContract);

                                //if item is newly added 
                                if (itemContract.IsNewItem)
                                {
                                    RequirementCategoryItem requrmntCategoryItem = GetRequirementCategoryItem(currentLoggedInUserID
                                                                                                               , requirementFieldContractList
                                                                                                               , existingRequrmntCategory, itemContract);
                                    existingRequrmntCategory.RequirementCategoryItems.Add(requrmntCategoryItem);
                                }
                                else
                                {
                                    RequirementItem existingRequrmntItem = existingRequrmntCategory.RequirementCategoryItems
                                                                                                   .Where(cond => cond.RCI_ID == itemContract.RequirementCategoryItemID && !cond.RCI_IsDeleted)
                                                                                                   .Select(col => col.RequirementItem).FirstOrDefault();
                                    //if item is updated
                                    if (itemContract.IsUpdated)
                                    {
                                        existingRequrmntItem.RI_ItemName = itemContract.RequirementItemName;
                                        existingRequrmntItem.RI_Description = itemContract.ExplanatoryNotes;
                                        //existingRequrmntItem.RI_ItemLabel = itemContract.RequirementItemLabel;
                                        existingRequrmntItem.RI_ModifiedByID = currentLoggedInUserID;
                                        existingRequrmntItem.RI_ModifiedOn = DateTime.Now;
                                        break;
                                    }
                                    else
                                    {
                                        foreach (RequirementFieldContract fieldContract in itemContract.LstRequirementField)
                                        {
                                            //if field is newly added
                                            if (fieldContract.IsNewField)
                                            {
                                                //code to add fields and its data into database
                                                RequirementItemField requrmntItemField = GetRequirementItemField(currentLoggedInUserID, requirementFieldContractList
                                                                                            , existingRequrmntItem, lstLkpDocumentAcroFieldType, requirementFieldViewDocumentTypeID
                                                                                            , lstLkpRequirementFieldDataType, fieldContract);
                                                existingRequrmntItem.RequirementItemFields.Add(requrmntItemField);
                                            }
                                            else
                                            {
                                                requirementFieldContractList.Add(fieldContract);
                                                RequirementField existingReqrmntField = existingRequrmntItem.RequirementItemFields
                                                                                                         .Where(cond => cond.RIF_ID == fieldContract.RequirementItemFieldID && !cond.RIF_IsDeleted)
                                                                                                          .Select(col => col.RequirementField)
                                                                                                          .FirstOrDefault();
                                                if (fieldContract.IsUpdated)
                                                {
                                                    //code to update field and field data table object
                                                    existingReqrmntField.RF_FieldName = fieldContract.RequirementFieldName;
                                                    //existingReqrmntField.RF_FieldLabel = fieldContract.RequirementFieldLabel;
                                                    //existingReqrmntField.RF_Description = fieldContract.RequirementFieldDescription;
                                                    existingReqrmntField.RF_ModifiedByID = currentLoggedInUserID;
                                                    existingReqrmntField.RF_ModifiedOn = DateTime.Now;
                                                    if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                                                    {
                                                        UpdateRequirementFieldVideo(currentLoggedInUserID, fieldContract, existingReqrmntField);
                                                    }
                                                    else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                                                    {
                                                        UpdateRequirementFieldOptions(currentLoggedInUserID, fieldContract, existingReqrmntField);
                                                    }
                                                    else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue()
                                                        && fieldContract.RequirementFieldData.FieldViewDocumentData.IsDocumentUpdated)
                                                    {
                                                        UpdateRequirementFieldDocumentData(currentLoggedInUserID, requirementFieldViewDocumentTypeID, lstLkpDocumentAcroFieldType, fieldContract, existingReqrmntField);
                                                    }

                                                    //UAT-2164
                                                    else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
                                                    {
                                                        RequirementItemField existingReqrmntItemFieldMapping = existingReqrmntField.RequirementItemFields.Where(x => x.RIF_RequirementFieldID == existingReqrmntField.RF_ID && !x.RIF_IsDeleted).FirstOrDefault();
                                                        if (existingReqrmntItemFieldMapping.IsNotNull())
                                                        {
                                                            existingReqrmntItemFieldMapping.RIF_IsBackgroundDocument = fieldContract.IsBackgroundDocument;
                                                            existingReqrmntItemFieldMapping.RIF_ModifiedByID = currentLoggedInUserID;
                                                            existingReqrmntItemFieldMapping.RIF_ModifiedOn = DateTime.Now;
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                BALUtils.GetSharedRequirementPackageRepoInstance().SaveContextIntoDataBase();

                #region Add category explanatory notes

                List<LargeContent> largeContentList = new List<LargeContent>();

                foreach (RequirementCategoryContract categoryContract in requirementPackageContract.LstRequirementCategory.Where(cond => cond.IsNewCategory
                                                                            && !cond.ExplanatoryNotes.IsNullOrEmpty()))
                {
                    RequirementCategory addedCategory = existingRequirementPackage.RequirementPackageCategories
                                                .Where(cond => cond.RequirementCategory.RC_Code == categoryContract.RequirementCategoryCode)
                                                .Select(col => col.RequirementCategory).FirstOrDefault();

                    LargeContent largeContent = CreateLargeContentForReqrmntCategory(addedCategory.RC_ID, categoryContract.ExplanatoryNotes, currentLoggedInUserID);
                    largeContentList.Add(largeContent);

                }

                if (!largeContentList.IsNullOrEmpty())
                {
                    BALUtils.GetSharedRequirementPackageRepoInstance().AddLargeContentToDatabase(largeContentList);
                }

                #endregion
            }

            if (existingRequirementPackage.RP_ID > AppConsts.NONE)
            {
                InsertDataIntoRequirementObjectTree(requirementPackageContract, currentLoggedInUserID, existingRequirementPackage, requirementFieldContractList, requirementItemContractList, false);
            }
            return existingRequirementPackage.RP_ID;
        }

        private static void UpdateReqrmntPackageAgency(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage existingRequirementPackage)
        {
            IEnumerable<RequirementPackageAgency> existingRequirementPackageAgencies = existingRequirementPackage.RequirementPackageAgencies
                                                                                                                 .Where(cond => !cond.RPA_IsDeleted);
            List<Int32> lstAgenciesMappedWithAgencyUser = ProfileSharingManager.GetAgencies(AppConsts.NONE, false, true, requirementPackageContract.CurrentUserId).Select(n => n.AG_ID).ToList();
            foreach (RequirementPackageAgency existingRequirementPackageAgency in existingRequirementPackageAgencies)
            {
                if (!requirementPackageContract.LstAgencyIDs.Contains(existingRequirementPackageAgency.RPA_AgencyID)
                        && lstAgenciesMappedWithAgencyUser.Contains(existingRequirementPackageAgency.RPA_AgencyID))
                {
                    existingRequirementPackageAgency.RPA_IsDeleted = true;
                    existingRequirementPackageAgency.RPA_ModifiedByID = currentLoggedInUserID;
                    existingRequirementPackageAgency.RPA_ModifiedOn = DateTime.Now;
                }
                else
                {
                    requirementPackageContract.LstAgencyIDs.Remove(existingRequirementPackageAgency.RPA_AgencyID);
                }
            }

            foreach (Int32 agencyID in requirementPackageContract.LstAgencyIDs)
            {
                RequirementPackageAgency newRequirementPackageAgency = new RequirementPackageAgency()
                {
                    RPA_AgencyID = agencyID,
                    RPA_IsDeleted = false,
                    RPA_CreatedByID = currentLoggedInUserID,
                    RPA_CreatedOn = DateTime.Now,
                };
                existingRequirementPackage.RequirementPackageAgencies.Add(newRequirementPackageAgency);
            }
        }


        private static void UpdateReqrmntPackageInstitutions(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage existingRequirementPackage)
        {
            IEnumerable<RequirementPackageInstitution> existingRequirementPackageInstitutions = existingRequirementPackage.RequirementPackageInstitutions
                                                                                                                 .Where(cond => !cond.RPI_IsDeleted);
            IEnumerable<Int32> existingRequirementPackageTenantIds = existingRequirementPackage.RequirementPackageInstitutions
                                                                                              .Where(cond => !cond.RPI_IsDeleted).Select(col => col.RPI_TenantID);
            //foreach (RequirementPackageInstitution existingRequirementPackageInstitution in existingRequirementPackageInstitutions)
            //{
            //    if (!requirementPackageContract.LstSelectedTenantIDs.Contains(existingRequirementPackageInstitution.RPI_TenantID))
            //    {
            //        existingRequirementPackageInstitution.RPI_IsDeleted = true;
            //        existingRequirementPackageInstitution.RPI_ModifiedByID = currentLoggedInUserID;
            //        existingRequirementPackageInstitution.RPI_ModifiedOn = DateTime.Now;
            //    }
            //    else
            //    {
            //        requirementPackageContract.LstSelectedTenantIDs.Remove(existingRequirementPackageInstitution.RPI_TenantID);
            //    }-
            //}

            foreach (Int32 tenantID in requirementPackageContract.LstSelectedTenantIDs)
            {
                if (existingRequirementPackageTenantIds.IsNullOrEmpty() || !existingRequirementPackageTenantIds.Contains(tenantID))
                {
                    RequirementPackageInstitution newRequirementPackageInstitution = new RequirementPackageInstitution()
                    {
                        RPI_TenantID = tenantID,
                        RPI_IsDeleted = false,
                        RPI_CreatedByID = currentLoggedInUserID,
                        RPI_CreatedOn = DateTime.Now,
                    };
                    existingRequirementPackage.RequirementPackageInstitutions.Add(newRequirementPackageInstitution);
                }
            }

            if (!requirementPackageContract.LstRemovedTenantIDs.IsNullOrEmpty())
            {
                foreach (RequirementPackageInstitution existingRequirementPackageInstitution in existingRequirementPackageInstitutions)
                {
                    if (requirementPackageContract.LstRemovedTenantIDs.Contains(existingRequirementPackageInstitution.RPI_TenantID))
                    {
                        existingRequirementPackageInstitution.RPI_IsDeleted = true;
                        existingRequirementPackageInstitution.RPI_ModifiedByID = currentLoggedInUserID;
                        existingRequirementPackageInstitution.RPI_ModifiedOn = DateTime.Now;
                    }
                }
            }
        }


        private static void UpdateRequirementFieldDocumentData(Int32 currentLoggedInUserID, Int32 requirementFieldViewDocumentTypeID, List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType, RequirementFieldContract fieldContract, RequirementField existingReqrmntField)
        {
            existingReqrmntField.RequirementFieldDocuments.ForEach(cond =>
            {
                cond.RFD_IsDeleted = true;
                cond.RFD_ModifiedByID = currentLoggedInUserID;
                cond.RFD_ModifiedOn = DateTime.Now;
                cond.RequirementDocumentAcroFields.ForEach(raf =>
                {
                    raf.RDAF_IsDeleted = true;
                    raf.RDAF_ModifiedByID = currentLoggedInUserID;
                    raf.RDAF_ModifiedOn = DateTime.Now;

                });
                cond.ClientSystemDocument.CSD_IsDeleted = true;
                cond.RFD_ModifiedByID = currentLoggedInUserID;
                cond.RFD_ModifiedOn = DateTime.Now;
            });

            RequirementFieldDocument requrmntFieldDocument = CreateRequirementFieldDocumentType(currentLoggedInUserID, requirementFieldViewDocumentTypeID
                                                            , lstLkpDocumentAcroFieldType, fieldContract);
            existingReqrmntField.RequirementFieldDocuments.Add(requrmntFieldDocument);
        }

        private static void UpdateRequirementFieldVideo(Int32 currentLoggedInUserID, RequirementFieldContract fieldContract, RequirementField existingReqrmntField)
        {
            RequirementFieldVideo existingRequirementFieldVideo = existingReqrmntField.RequirementFieldVideos
                                                                     .Where(cond => !cond.RFV_IsDeleted)
                                                                    .FirstOrDefault();
            existingRequirementFieldVideo.RFV_VideoURL = fieldContract.RequirementFieldData.VideoFieldData.VideoURL;
            existingRequirementFieldVideo.RFV_VideoName = fieldContract.RequirementFieldData.VideoFieldData.VideoName;
            existingRequirementFieldVideo.RFV_ModifiedByID = currentLoggedInUserID;
            existingRequirementFieldVideo.RFV_ModifiedOn = DateTime.Now;
        }

        private static void UpdateRequirementFieldOptions(Int32 currentLoggedInUserID, RequirementFieldContract fieldContract, RequirementField existingReqrmntField)
        {
            List<RequirementFieldOptionsData> lstUpdatedOptionDataContract = fieldContract.RequirementFieldData.LstRequirementFieldOptions;
            List<RequirementFieldOption> lstFieldOptionsInDb = existingReqrmntField.RequirementFieldOptions.Where(cond => !cond.RFO_IsDeleted).ToList();
            foreach (RequirementFieldOption fieldOptionInDb in lstFieldOptionsInDb)
            {
                if (lstUpdatedOptionDataContract.Any(x => x.OptionText == fieldOptionInDb.RFO_OptionText
                    && x.OptionValue == fieldOptionInDb.RFO_OptionValue))
                    continue;

                fieldOptionInDb.RFO_IsDeleted = true;
                fieldOptionInDb.RFO_ModifiedOn = DateTime.Now;
                fieldOptionInDb.RFO_ModifiedByID = currentLoggedInUserID;
            }

            foreach (RequirementFieldOptionsData optionContract in lstUpdatedOptionDataContract)
            {
                if (lstFieldOptionsInDb.Any(x => x.RFO_OptionText == optionContract.OptionText
                    && x.RFO_OptionValue == optionContract.OptionValue))
                    continue;

                existingReqrmntField.RequirementFieldOptions.Add(new RequirementFieldOption()
                {
                    RFO_CreatedByID = currentLoggedInUserID,
                    RFO_CreatedOn = DateTime.Now,
                    RFO_IsDeleted = false,
                    RFO_OptionText = optionContract.OptionText,
                    RFO_OptionValue = optionContract.OptionValue,

                });
            }
        }

        private static Boolean UpdateLargeContentForReqrmntCategory(Int32 reqrmntCategoryID, String explanatoryNotes,
                                                                            Int32 currentLoggedInUserID)
        {
            String explanatoryNotesContentTypeCode = LCContentType.ExplanatoryNotes.GetStringValue();
            String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();
            Int32 contentTypeId = LookupManager.GetSharedDBLookUpData<lkpLargeContentType>()
                                            .Where(obj => obj.LCT_Code == explanatoryNotesContentTypeCode && !obj.LCT_IsDeleted).FirstOrDefault().LCT_ID;
            Int32 objectTypeId = LookupManager.GetSharedDBLookUpData<lkpObjectType>()
                                            .Where(obj => obj.OT_Code == reqrmnCategoryObjectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
            LargeContent existingLargeContent = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                        .GetLargeContentForReqrmntCategory(reqrmntCategoryID, objectTypeId, contentTypeId);
            if (!existingLargeContent.IsNullOrEmpty())
            {
                existingLargeContent.LC_Content = explanatoryNotes;
                existingLargeContent.LC_ModifiedByID = currentLoggedInUserID;
                existingLargeContent.LC_ModifiedOn = DateTime.Now;
            }
            else if (!explanatoryNotes.IsNullOrEmpty())
            {
                LargeContent largeContent = new LargeContent()
                {
                    LC_ObjectID = reqrmntCategoryID,
                    LC_ObjectTypeID = objectTypeId,
                    LC_LargeContentTypeID = contentTypeId,
                    LC_Content = explanatoryNotes,
                    LC_IsDeleted = false,
                    LC_CreatedByID = currentLoggedInUserID,
                    LC_CreatedOn = DateTime.Now,
                };
                BALUtils.GetSharedRequirementPackageRepoInstance().AddLargeContentToContext(largeContent);
            }
            return true;
        }

        private static void CreateRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID
                                                   , Boolean isPackageVersionNeedToCreate, RequirementPackage requirementPackage, Boolean isPackageMappedToRotation = false, Boolean isCopy = false)
        {
            //set package type to "roatation package type"
            //String rotationPackageTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
            //requirementPackage.RP_RequirementPackageTypeID = LookupManager.GetSharedDBLookUpData<lkpRequirementPackageType>()
            //                                                    .Where(col => col.RPT_Code == rotationPackageTypeCode).FirstOrDefault().RPT_ID;
            //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use

            requirementPackage.RP_RequirementPackageTypeID = requirementPackageContract.RequirementPkgTypeID;
            requirementPackage.RP_PackageName = requirementPackageContract.RequirementPackageName;
            requirementPackage.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
            requirementPackage.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
            requirementPackage.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;
            requirementPackage.RP_IsDeleted = false;
            requirementPackage.RP_IsActive = true;

            if (!requirementPackageContract.IsCopyWithInMaster)
            {
                requirementPackage.RP_IsUsed = isCopy && isPackageMappedToRotation ? true : (Boolean?)null;
                requirementPackage.RP_IsCopied = isCopy ? isCopy : (Boolean?)null;
            }

            if (isPackageVersionNeedToCreate)
            {
                requirementPackage.RP_FirstVersionID = requirementPackageContract.RequirementPackageID;
                //code to update RP_IsActive column of existing package to false
                SetExistingPackageIsActiveToFalse(currentLoggedInUserID, requirementPackageContract.RequirementPackageID);
                requirementPackage.RP_Code = Guid.NewGuid();
            }
            else
            {
                requirementPackage.RP_Code = requirementPackageContract.RequirementPackageCode;
            }
            requirementPackage.RP_CreatedByID = currentLoggedInUserID;
            requirementPackage.RP_CreatedOn = DateTime.Now;

        }

        private static void CreateRqrmntPkgAgency(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage requirementPackage)
        {
            foreach (Int32 agencyID in requirementPackageContract.LstAgencyIDs)
            {
                RequirementPackageAgency requirementPackageAgency = new RequirementPackageAgency()
                {
                    RPA_AgencyID = agencyID,
                    RPA_IsDeleted = false,
                    RPA_CreatedByID = currentLoggedInUserID,
                    RPA_CreatedOn = DateTime.Now,
                };
                requirementPackage.RequirementPackageAgencies.Add(requirementPackageAgency);
            }
        }

        private static void CreateRqrmntPkgInstitution(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage requirementPackage)
        {
            foreach (Int32 tenantID in requirementPackageContract.LstSelectedTenantIDs)
            {
                RequirementPackageInstitution requirementPackageInstitution = new RequirementPackageInstitution()
                {
                    RPI_TenantID = tenantID,
                    RPI_IsDeleted = false,
                    RPI_CreatedByID = currentLoggedInUserID,
                    RPI_CreatedOn = DateTime.Now,
                };
                requirementPackage.RequirementPackageInstitutions.Add(requirementPackageInstitution);
            }
        }

        private static RequirementCategory CreateRequrmntCategory(Int32 currentLoggedInUserID, RequirementCategoryContract categoryContract)
        {
            RequirementCategory requirementCategory = new RequirementCategory();
            requirementCategory.RC_CategoryName = categoryContract.RequirementCategoryName;
            //if (!categoryContract.RequirementCategoryLabel.IsNullOrEmpty())
            //{
            //    requirementCategory.RC_CategoryLabel = categoryContract.RequirementCategoryLabel;
            //}
            //if (!categoryContract.RequirementCategoryDescription.IsNullOrEmpty())
            //{
            //    requirementCategory.RC_Description = categoryContract.RequirementCategoryDescription;
            //}
            requirementCategory.RC_Code = categoryContract.RequirementCategoryCode;
            requirementCategory.RC_CreatedByID = currentLoggedInUserID;
            requirementCategory.RC_CreatedOn = DateTime.Now;
            requirementCategory.RC_IsDeleted = false;
            //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
            if (!categoryContract.RequirementDocumentLink.IsNullOrEmpty())
            {
                requirementCategory.RC_SampleDocFormURL = categoryContract.RequirementDocumentLink;
            }
            else
            {
                requirementCategory.RC_SampleDocFormURL = null;
            }
            //UAT-3161
            if (!categoryContract.RequirementDocumentLinkLabel.IsNullOrEmpty())
            {
                requirementCategory.RC_SampleDocFormUrlLabel = categoryContract.RequirementDocumentLinkLabel;
            }
            else
            {
                requirementCategory.RC_SampleDocFormUrlLabel = null;
            }

            //UAT-3805
            requirementCategory.RC_SendItemDocOnApproval = categoryContract.SendItemDoconApproval;
            return requirementCategory;
        }

        private static RequirementItem CreateRequirementItem(Int32 currentLoggedInUserID, RequirementItemContract itemContract)
        {
            RequirementItem requirementItem = new RequirementItem();
            requirementItem.RI_ItemName = itemContract.RequirementItemName;
            //if (!itemContract.RequirementItemLabel.IsNullOrEmpty())
            //{
            //    requirementItem.RI_ItemLabel = itemContract.RequirementItemLabel;
            //}
            //if (!itemContract.RequirementItemDescription.IsNullOrEmpty())
            //{
            //    requirementItem.RI_Description = itemContract.RequirementItemDescription;
            //}
            requirementItem.RI_Code = itemContract.RequirementItemCode;
            requirementItem.RI_Description = itemContract.ExplanatoryNotes; // UAT-2676
            requirementItem.RI_CreatedByID = currentLoggedInUserID;
            requirementItem.RI_CreatedOn = DateTime.Now;
            requirementItem.RI_IsDeleted = false;
            requirementItem.RI_SampleDocFormURL = itemContract.RequirementItemSampleDocumentFormURL; //UAT-3309

            return requirementItem;
        }

        private static RequirementFieldVideo CreateReqirmntFieldVideo(Int32 currentLoggedInUserID, RequirementFieldContract fieldContract)
        {
            RequirementFieldVideo fieldVideo = new RequirementFieldVideo();
            //fieldVideo.RFV_IsVideoOpenRequired = fieldContract.RequirementFieldData.VideoFieldData.IsVideoRequiredToBeOpened;
            fieldVideo.RFV_VideoName = fieldContract.RequirementFieldData.VideoFieldData.VideoName;
            fieldVideo.RFV_VideoURL = fieldContract.RequirementFieldData.VideoFieldData.VideoURL;
            //if (fieldContract.RequirementFieldData.VideoFieldData.IsVideoRequiredToBeOpened)
            //{
            //    Int32 openMinutes = fieldContract.RequirementFieldData.VideoFieldData.VideoOpenedMinutes.IsNullOrEmpty() ? 0 : fieldContract.RequirementFieldData.VideoFieldData.VideoOpenedMinutes;
            //    openMinutes += fieldContract.RequirementFieldData.VideoFieldData.VideoOpenedHours.IsNullOrEmpty() ? 0 : fieldContract.RequirementFieldData.VideoFieldData.VideoOpenedHours * 60;
            //    fieldVideo.RFV_VideoOpenTimeDuration = openMinutes;
            //}
            fieldVideo.RFV_IsDeleted = false;
            fieldVideo.RFV_CreatedByID = currentLoggedInUserID;
            fieldVideo.RFV_CreatedOn = DateTime.Now;

            return fieldVideo;
        }

        private static void AddReqrmntFieldOptionsForReqrmntField(Int32 currentLoggedInUserID, RequirementFieldContract fieldContract
                                                                                          , RequirementField reqirmntField)
        {
            if (fieldContract.RequirementFieldData.IsNotNull())
            {
                foreach (RequirementFieldOptionsData optionData in fieldContract.RequirementFieldData.LstRequirementFieldOptions)
                {
                    RequirementFieldOption requirementFieldOption = CreateReqrmntFieldOptions(currentLoggedInUserID, optionData);
                    reqirmntField.RequirementFieldOptions.Add(requirementFieldOption);
                }
            }
        }

        private static RequirementFieldOption CreateReqrmntFieldOptions(Int32 currentLoggedInUserID, RequirementFieldOptionsData optionData)
        {
            RequirementFieldOption requirementFieldOption = new RequirementFieldOption()
            {
                RFO_OptionText = optionData.OptionText,
                RFO_OptionValue = optionData.OptionValue,
                RFO_CreatedByID = currentLoggedInUserID,
                RFO_CreatedOn = DateTime.Now,
                RFO_IsDeleted = false,
            };

            return requirementFieldOption;
        }

        private static RequirementFieldDocument CreateRequirementFieldDocumentType(Int32 currentLoggedInUserID, Int32 requirementFieldViewDocumentTypeID
                                                                                   , List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType
                                                                                   , RequirementFieldContract fieldContract)
        {
            RequirementFieldDocument requirementFieldDocument = CreateRequrmntFieldDocument(currentLoggedInUserID);

            ClientSystemDocument clientSystemDocument = CreateClientSysDocument(currentLoggedInUserID, requirementFieldViewDocumentTypeID, fieldContract);

            requirementFieldDocument.ClientSystemDocument = clientSystemDocument;

            foreach (String acroFieldTypeCode in fieldContract.RequirementFieldData.FieldViewDocumentData.LstDocumentAcroFieldTypeCodes)
            {
                RequirementDocumentAcroField requirementDocumentAcroField = CreateRequrmntDocumentAcroField(currentLoggedInUserID, lstLkpDocumentAcroFieldType, acroFieldTypeCode);
                requirementFieldDocument.RequirementDocumentAcroFields.Add(requirementDocumentAcroField);
            }
            return requirementFieldDocument;
        }

        private static RequirementDocumentAcroField CreateRequrmntDocumentAcroField(Int32 currentLoggedInUserID, List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType, String acroFieldTypeCode)
        {
            RequirementDocumentAcroField requirementDocumentAcroField = new RequirementDocumentAcroField()
            {
                RDAF_IsDeleted = false,
                RDAF_CreatedByID = currentLoggedInUserID,
                RDAF_CreatedOn = DateTime.Now,
                RDAF_DocumentAcroFieldTypeID = lstLkpDocumentAcroFieldType
                                              .Where(cond => cond.DAFT_Code == acroFieldTypeCode).FirstOrDefault().DAFT_ID

            };
            return requirementDocumentAcroField;
        }

        private static RequirementFieldDocument CreateRequrmntFieldDocument(Int32 currentLoggedInUserID)
        {
            RequirementFieldDocument requirementFieldDocument = new RequirementFieldDocument()
            {
                RFD_IsDeleted = false,
                RFD_CreatedByID = currentLoggedInUserID,
                RFD_CreatedOn = DateTime.Now
            };
            return requirementFieldDocument;
        }

        private static ClientSystemDocument CreateClientSysDocument(Int32 currentLoggedInUserID, Int32 requirementFieldViewDocumentTypeID, RequirementFieldContract fieldContract)
        {
            ClientSystemDocument clientSystemDocument = new ClientSystemDocument()
            {
                CSD_CreatedByID = currentLoggedInUserID,
                CSD_CreatedOn = DateTime.Now,
                CSD_FileName = fieldContract.RequirementFieldData.FieldViewDocumentData.DocumentFileName,
                CSD_DocumentPath = fieldContract.RequirementFieldData.FieldViewDocumentData.DocumentPath,
                CSD_Size = fieldContract.RequirementFieldData.FieldViewDocumentData.DocumentSize,
                CSD_DocumentTypeID = requirementFieldViewDocumentTypeID,
                CSD_IsDeleted = false
            };
            return clientSystemDocument;
        }

        /// <summary>
        /// UAT-2164,AgencyUserGranularPermission.  Added IsBackgroundDocument parameter.
        /// </summary>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="requirementItem"></param>
        /// <param name="requirementField"></param>
        /// <param name="isBackgroundDocument"></param>
        /// <returns></returns>
        private static RequirementItemField CreateRequirementItemField(Int32 currentLoggedInUserID, RequirementItem requirementItem, RequirementField requirementField, Boolean isBackgroundDocument, Int32 fieldDisplayOrder)
        {
            RequirementItemField requrmntItemField = new RequirementItemField();
            requrmntItemField.RIF_CreatedByID = currentLoggedInUserID;
            requrmntItemField.RIF_CreatedOn = DateTime.Now;
            requrmntItemField.RIF_IsDeleted = false;
            requrmntItemField.RequirementItem = requirementItem;
            requrmntItemField.RequirementField = requirementField;
            requrmntItemField.RIF_DisplayOrder = fieldDisplayOrder; //UAT-3078
            //UAT-2164
            requrmntItemField.RIF_IsBackgroundDocument = isBackgroundDocument;

            return requrmntItemField;
        }

        private static RequirementCategoryItem CreateRequrmntCategoryItem(Int32 currentLoggedInUserID, RequirementCategory requirementCategory
                                                                                 , RequirementItem requirementItem, RequirementItemContract itemcontract)
        {
            RequirementCategoryItem requrmntCategoryItem = new RequirementCategoryItem();
            requrmntCategoryItem.RCI_CreatedByID = currentLoggedInUserID;
            requrmntCategoryItem.RCI_CreatedOn = DateTime.Now;
            requrmntCategoryItem.RCI_IsDeleted = false;
            requrmntCategoryItem.RequirementCategory = requirementCategory;
            requrmntCategoryItem.RequirementItem = requirementItem;
            requrmntCategoryItem.RCI_DisplayOrder = itemcontract.RequirementItemDisplayOrder; //requirementCategory.RequirementCategoryItems.Select(sl => sl.RCI_DisplayOrder).FirstOrDefault(); //UAT-3078
            return requrmntCategoryItem;
        }

        private static RequirementPackageCategory CreateRequrmntPackageCategory(Int32 currentLoggedInUserID, RequirementPackage requirementPackage
                                                                                      , RequirementCategory requirementCategory)
        {
            RequirementPackageCategory reqrmntPackageCategory = new RequirementPackageCategory();
            reqrmntPackageCategory.RPC_CreatedByID = currentLoggedInUserID;
            reqrmntPackageCategory.RPC_CreatedOn = DateTime.Now;
            reqrmntPackageCategory.RPC_IsDeleted = false;
            reqrmntPackageCategory.RequirementPackage = requirementPackage;
            reqrmntPackageCategory.RequirementCategory = requirementCategory;
            return reqrmntPackageCategory;
        }

        private static LargeContent CreateLargeContentForReqrmntCategory(Int32 reqrmntCategoryID, String explanatoryNotes,
                                                                            Int32 currentLoggedInUserID)
        {
            String explanatoryNotesContentTypeCode = LCContentType.ExplanatoryNotes.GetStringValue();
            String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();
            Int32 contentTypeId = LookupManager.GetSharedDBLookUpData<lkpLargeContentType>()
                                            .Where(obj => obj.LCT_Code == explanatoryNotesContentTypeCode && !obj.LCT_IsDeleted).FirstOrDefault().LCT_ID;
            Int32 objectTypeId = LookupManager.GetSharedDBLookUpData<lkpObjectType>()
                                            .Where(obj => obj.OT_Code == reqrmnCategoryObjectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
            return new LargeContent()
            {
                LC_ObjectID = reqrmntCategoryID,
                LC_ObjectTypeID = objectTypeId,
                LC_LargeContentTypeID = contentTypeId,
                LC_Content = explanatoryNotes,
                LC_IsDeleted = false,
                LC_CreatedByID = currentLoggedInUserID,
                LC_CreatedOn = DateTime.Now,
            };

        }

        private static void SetExistingPackageIsActiveToFalse(Int32 currentLoggedInUserID, Int32 requirementPackageID)
        {
            //Get existing requirement package(from DB) which is to be updated 
            RequirementPackage existingRequirementPackage = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackageByPackageID(requirementPackageID);
            if (!existingRequirementPackage.IsNullOrEmpty())
            {
                existingRequirementPackage.RP_IsActive = false;
                existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
            }
        }

        public static void SetExistingPackageIsUsedToTrue(Int32 currentLoggedInUserID, Int32 requirementPackageID)
        {
            //Get existing requirement package(from DB) which is to be updated 
            RequirementPackage existingRequirementPackage = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackageByPackageID(requirementPackageID);
            if (!existingRequirementPackage.IsNullOrEmpty())
            {
                existingRequirementPackage.RP_IsUsed = true;
                existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
                BALUtils.GetSharedRequirementPackageRepoInstance().SaveContextIntoDataBase();
            }

        }


        private static void DeleteRequirementObjectTree(Int32 reqObjectTreeId, Int32 currentLoggedInUserID)
        {
            RequirementObjectTree existingRequirementObjectTree = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTree(reqObjectTreeId);
            if (!existingRequirementObjectTree.IsNullOrEmpty())
            {
                RequirementObjectTreeProperty requirementObjectTreePropertyInDb = existingRequirementObjectTree.RequirementObjectTreeProperties.Where(cond => !cond.ROTP_IsDeleted)
                                                                                                                                            .FirstOrDefault();
                if (!requirementObjectTreePropertyInDb.IsNullOrEmpty())
                {
                    requirementObjectTreePropertyInDb.ROTP_IsDeleted = false;
                    requirementObjectTreePropertyInDb.ROTP_ModifiedByID = currentLoggedInUserID;
                    requirementObjectTreePropertyInDb.ROTP_ModifiedOn = DateTime.Now;
                }

                List<RequirementObjectRule> requirementObjectRuleListInDb = existingRequirementObjectTree.RequirementObjectRules.Where(cond => !cond.ROR_IsDeleted)
                                                                                                                                             .ToList();
                foreach (RequirementObjectRule requirementObjectRuleInDb in requirementObjectRuleListInDb)
                {
                    requirementObjectRuleInDb.ROR_IsDeleted = false;
                    requirementObjectRuleInDb.ROR_ModifiedByID = currentLoggedInUserID;
                    requirementObjectRuleInDb.ROR_ModifiedOn = DateTime.Now;
                    List<RequirementObjectRuleDetail> requirementObjectRuleDetailListInDb = requirementObjectRuleInDb.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).ToList();
                    foreach (RequirementObjectRuleDetail requirementObjectRuleDetailInDb in requirementObjectRuleDetailListInDb)
                    {
                        requirementObjectRuleDetailInDb.RORD_IsDeleted = false;
                        requirementObjectRuleDetailInDb.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedOn = DateTime.Now;
                    }
                }
            }
        }


        #endregion

        #endregion

        #region UAT 1352 As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use.
        public static List<RequirementPackageTypeContract> GetRequirementPackageType()
        {
            try
            {
                List<lkpRequirementPackageType> dataTypeList = LookupManager.GetSharedDBLookUpData<lkpRequirementPackageType>().Where(x => !x.RPT_IsDeleted).ToList();
                return dataTypeList.Select(con => new RequirementPackageTypeContract
                {
                    ID = con.RPT_ID,
                    Code = con.RPT_Code,
                    Name = con.RPT_Name
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        //public static Dictionary<Int32, List<Int32>> GetTenantIDsMappedForAgencyUser(Guid userID)
        //{
        //    return BALUtils.GetSharedRequirementPackageRepoInstance().GetTenantIDsMappedForAgencyUser(userID);
        //}

        public static RequirementPackageContract GetRequirementPackageDataByID(Int32 requirementPkgID)
        {
            try
            {
                RequirementPackage existingRequirementPackage = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                     .GetRequirementPackageByPackageID(requirementPkgID);
                RequirementPackageContract requirementPackageContract = new RequirementPackageContract();
                if (!existingRequirementPackage.IsNullOrEmpty())
                {
                    requirementPackageContract.RequirementPackageID = existingRequirementPackage.RP_ID;
                    requirementPackageContract.RequirementPackageName = existingRequirementPackage.RP_PackageName;
                    requirementPackageContract.RequirementPackageLabel = existingRequirementPackage.RP_PackageLabel;
                    requirementPackageContract.LstAgencyIDs = existingRequirementPackage.RequirementPackageAgencies.IsNullOrEmpty()
                                                                                                ? new List<Int32>()
                                                                                                : existingRequirementPackage.RequirementPackageAgencies
                                                                                                .Where(cond => !cond.RPA_IsDeleted)
                                                                                                .Select(col => col.RPA_AgencyID).ToList();

                    requirementPackageContract.EffectiveStartDate = existingRequirementPackage.RP_EffectiveStartDate;
                    requirementPackageContract.EffectiveEndDate = existingRequirementPackage.RP_EffectiveEndDate;

                    var packageCategories = existingRequirementPackage.RequirementPackageCategories.Where(cond => !cond.RPC_IsDeleted);
                    requirementPackageContract.LstRequirementCategory = new List<RequirementCategoryContract>();

                    if (!packageCategories.IsNullOrEmpty())
                    {
                        foreach (var category in packageCategories)
                        {
                            requirementPackageContract.LstRequirementCategory.Add(new RequirementCategoryContract
                            {
                                RequirementCategoryID = category.RPC_RequirementCategoryID,
                                RequirementCategoryName = category.RequirementCategory.RC_CategoryName,
                                RequirementCategoryLabel = category.RequirementCategory.RC_CategoryLabel,
                                IsComplianceRequired = category.RequirementCategory.RC_ComplianceRequired,
                                ComplianceReqEndDate = category.RequirementCategory.RC_ComplianceRqdEndDate,
                                ComplianceReqStartDate = category.RequirementCategory.RC_ComplianceRqdStartDate,
                                SendItemDoconApproval = category.RequirementCategory.RC_SendItemDocOnApproval.IsNullOrEmpty() ? false : category.RequirementCategory.RC_SendItemDocOnApproval.Value//UAT-3805

                            });
                        }
                    }

                    requirementPackageContract.RequirementPkgTypeID = existingRequirementPackage.RP_RequirementPackageTypeID;
                    requirementPackageContract.DefinedRequirementID = existingRequirementPackage.RP_DefinedRequirementID;
                    requirementPackageContract.ReqReviewByID = existingRequirementPackage.RP_ReqReviewByID;
                    //UAT-2560
                    var agencyHieraryPackage = existingRequirementPackage.AgencyHierarchyPackages.Where(cond => !cond.AHP_IsDeleted).ToList();
                    requirementPackageContract.SelectedAgencyHierarchyDeatils = new Dictionary<Int32, String>();
                    foreach (var agHierarchyId in agencyHieraryPackage.Select(sel => sel.AHP_AgencyHierarchyID).Distinct().ToList())
                    {
                        requirementPackageContract.SelectedAgencyHierarchyDeatils.Add(agHierarchyId, String.Empty);
                    }
                }
                return requirementPackageContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 SaveRequirementPackageData(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID)
        {
            try
            {
                Int32 addedPkgID = BALUtils.GetSharedRequirementPackageRepoInstance().SaveRequirementPackageData(requirementPackageContract, currentLoggedInUserID);
                if (addedPkgID != requirementPackageContract.RequirementPackageID)
                {
                    AddRequirementObjectTree(addedPkgID, currentLoggedInUserID);
                }
                return addedPkgID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1837:ADB Admin streamlined create and edit rotation packages
        /// <summary>
        /// used to get requirement Item detail
        /// </summary>
        /// <returns></returns>
        public static RequirementItemContract GetRequirementItemDetailsByItemID(Int32 requirementItemID, Int32? requirementCategoryID = null)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementItemDetailsByItemID(requirementItemID, requirementCategoryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to use Save/Update requirement item detail.
        /// </summary>
        /// <returns></returns>
        public static Boolean SaveUpdateRequirementItemData(RequirementItemContract reqItemContract, Int32 currentLoggedInUserId)
        {
            try
            {
                Boolean isSavedSuccessfully = BALUtils.GetSharedRequirementPackageRepoInstance().SaveUpdateRequirementItemData(reqItemContract, currentLoggedInUserId);
                #region UAT-2213
                //UAT-2213:New Rotation Package Process: Master Setup, conditionally called the method of RequirementObjectTree Entry
                if (isSavedSuccessfully && reqItemContract.RequirementItemID == AppConsts.NONE)
                {
                    if (reqItemContract.IsNewPackage)
                    {
                        AddNewRequirementObjectTree(reqItemContract.RequirementCategoryID, currentLoggedInUserId);
                    }
                    else
                    {
                        AddRequirementObjectTree(reqItemContract.RequirementPackageID, currentLoggedInUserId);
                    }
                }
                #endregion

                //if (!reqItemContract.UniversalItemContract.IsNullOrEmpty())
                //{
                //    UniversalMappingDataManager.SaveUniversalRequirmentItemMappingData(ConvertToUniversalRotationMappingContract(reqItemContract), currentLoggedInUserId);
                //}
                return isSavedSuccessfully;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to get all requirement items of category
        /// </summary>
        /// <returns></returns>
        public static List<RequirementItemContract> GetRequirementItemsByCategoryID(Int32 requirementCategoryID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementItemsByCategoryID(requirementCategoryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        ///Method to delete Requirement category and item mapping
        /// </summary>
        /// <returns></returns>
        public static String DeleteReqCategoryItemMapping(Int32 requirementCatItemID, Int32 currentLoggedInUserId, Int32 reqPkgId, Boolean isNewPackage = false)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().DeleteReqCategoryItemMapping(requirementCatItemID, currentLoggedInUserId, reqPkgId, isNewPackage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        ///Method to delete Requirement Item and Field mapping
        /// </summary>
        /// <returns></returns>
        public static String DeleteReqItemFieldMapping(Int32 requirementItemFieldID, String ItemHId, Int32 currentLoggedInUserId, Boolean isNewPackage = false, Int32 requirementCategoryID = 0)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().DeleteReqItemFieldMapping(requirementItemFieldID, ItemHId, currentLoggedInUserId, isNewPackage, requirementCategoryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to get all requirement fields of Items
        /// </summary>
        /// <returns></returns>
        public static List<RequirementFieldContract> GetRequirementFieldsByItemID(Int32 requirementItemID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementFieldsByItemID(requirementItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-3342
        public static List<RequirementFieldContract> IsCalculatedAttribute(Int32 requirementFieldID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().IsCalculatedAttribute(requirementFieldID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to use Save/Update requirement Field detail.
        /// </summary>
        /// <returns></returns>
        public static Int32 SaveUpdateRequirementFieldData(RequirementFieldContract reqFieldContract, Int32 currentLoggedInUserId)
        {
            try
            {
                Boolean isDataSaved = false;
                #region Define common variables and get lkp values

                List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType = LookupManager.GetSharedDBLookUpData<lkpDocumentAcroFieldType>().ToList();

                String requirementFieldViewDocumentTypeCode = DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue();
                Int32 requirementFieldViewDocumentTypeID = LookupManager.GetSharedDBLookUpData<lkpDocumentType>()
                                                            .Where(cond => cond.DMT_Code == requirementFieldViewDocumentTypeCode).FirstOrDefault().DMT_ID;

                List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType = LookupManager.GetSharedDBLookUpData<lkpRequirementFieldDataType>().ToList();

                #endregion

                #region UAT-2305
                Int32 ReqItemFieldID = 0;
                #endregion

                Boolean IsNewPackage = false;
                if (reqFieldContract.IsNewPackage)
                    IsNewPackage = true;

                //Newly created field.
                if (reqFieldContract.IsNewField)
                {
                    //Add new field
                    RequirementField requirementField = CreateRequirementField(currentLoggedInUserId, requirementFieldViewDocumentTypeID,
                                                                               lstLkpRequirementFieldDataType, lstLkpDocumentAcroFieldType, reqFieldContract);

                    //Create Item Field Mapping.
                    RequirementItemField requrmntItemField = new RequirementItemField();
                    requrmntItemField.RIF_CreatedByID = currentLoggedInUserId;
                    requrmntItemField.RIF_CreatedOn = DateTime.Now;
                    requrmntItemField.RIF_IsDeleted = false;

                    requrmntItemField.RIF_RequirementItemID = reqFieldContract.RequirementItemID;
                    //UAT-2164
                    requrmntItemField.RIF_IsBackgroundDocument = reqFieldContract.IsBackgroundDocument;

                    //UAt-3078
                    requrmntItemField.RIF_DisplayOrder = reqFieldContract.RequirementFieldDisplayOrder;
                    //Assign RequirementItemFields to RequirementField.
                    requirementField.RequirementItemFields.Add(requrmntItemField);
                    isDataSaved = BALUtils.GetSharedRequirementPackageRepoInstance().SaveUpdateRequirementField(requirementField, true, IsNewPackage);
                    #region UAT-2213
                    //UAT-2213:New Rotation Package Process: Master Setup, conditionally called RequirementObjectTree entry method
                    if (isDataSaved)
                    {
                        BALUtils.GetSharedRequirementPackageRepoInstance().SaveUpdateRequirementFieldEditable(reqFieldContract, requrmntItemField.RIF_ID, currentLoggedInUserId);
                        if (reqFieldContract.IsNewPackage)
                        {
                            List<Int32> lstCategoryIds = BALUtils.GetSharedRequirementPackageRepoInstance().GetCategoryIdsForAssignedField(requirementField.RF_ID);
                            foreach (var reqCatID in lstCategoryIds)
                            {
                                //AddNewRequirementObjectTree(reqFieldContract.RequirementCategoryID, currentLoggedInUserId);
                                AddNewRequirementObjectTree(reqCatID, currentLoggedInUserId);
                            }
                        }
                        else
                        {
                            AddRequirementObjectTree(reqFieldContract.RequirementPackageID, currentLoggedInUserId);
                        }
                    }
                    #endregion

                    ReqItemFieldID = requirementField.RequirementItemFields.FirstOrDefault().RIF_ID;
                    //UAT-2402
                    reqFieldContract.RequirementFieldID = requirementField.RF_ID;

                }
                //If Field is updated.
                else
                {
                    //Get Existing Requirement field from DB.
                    RequirementField existingReqrmntField = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementFieldByFieldID(reqFieldContract.RequirementFieldID);
                    //code to update field and field data table object
                    if (!existingReqrmntField.IsNullOrEmpty())
                    {
                        existingReqrmntField.RF_FieldName = reqFieldContract.RequirementFieldName;
                        existingReqrmntField.RF_FieldLabel = reqFieldContract.RequirementFieldLabel;
                        existingReqrmntField.RF_ModifiedByID = currentLoggedInUserId;
                        existingReqrmntField.RF_ModifiedOn = DateTime.Now;
                        existingReqrmntField.RF_AttributeTypeID = reqFieldContract.AttributeTypeID;
                        existingReqrmntField.RF_RequirementAttributeGroupID = reqFieldContract.RequirementFieldAttributeGroupId; //UAT-3176
                        #region UAT-3078
                        RequirementItemField existingRequiremtItemField = existingReqrmntField.RequirementItemFields.FirstOrDefault(cnd => cnd.RIF_RequirementFieldID == reqFieldContract.RequirementFieldID && !cnd.RIF_IsDeleted);
                        if (existingRequiremtItemField.IsNotNull() && !reqFieldContract.RequirementFieldDisplayOrder.IsNullOrEmpty())
                        {
                            existingRequiremtItemField.RIF_DisplayOrder = reqFieldContract.RequirementFieldDisplayOrder;
                        }

                        #endregion
                        if (!reqFieldContract.RequirementFieldLabel.IsNullOrEmpty())
                        {
                            existingReqrmntField.RF_FieldLabel = reqFieldContract.RequirementFieldLabel;
                        }
                        if (reqFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue()) //UAT-2701
                        {
                            existingReqrmntField.RF_MaximumCharacters = reqFieldContract.RequirementFieldMaxLength;
                        }
                        if (reqFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                        {
                            UpdateRequirementFieldVideo(currentLoggedInUserId, reqFieldContract, existingReqrmntField);
                        }
                        else if (reqFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                        {
                            UpdateRequirementFieldOptions(currentLoggedInUserId, reqFieldContract, existingReqrmntField);
                        }
                        else if (reqFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue()
                            && !reqFieldContract.RequirementFieldData.FieldViewDocumentData.IsNullOrEmpty()
                            && reqFieldContract.RequirementFieldData.FieldViewDocumentData.IsDocumentUpdated)
                        {
                            UpdateRequirementFieldDocumentData(currentLoggedInUserId, requirementFieldViewDocumentTypeID, lstLkpDocumentAcroFieldType, reqFieldContract, existingReqrmntField);
                        }
                        else if (reqFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
                        {
                            if (existingRequiremtItemField.IsNotNull())
                            {
                                existingRequiremtItemField.RIF_DisplayOrder = reqFieldContract.RequirementFieldDisplayOrder; //UAT-3078
                                existingRequiremtItemField.RIF_IsBackgroundDocument = reqFieldContract.IsBackgroundDocument;
                                existingRequiremtItemField.RIF_ModifiedByID = currentLoggedInUserId;
                                existingRequiremtItemField.RIF_ModifiedOn = DateTime.Now;
                            }
                        }

                        isDataSaved = BALUtils.GetSharedRequirementPackageRepoInstance().SaveUpdateRequirementField(existingReqrmntField, false, IsNewPackage);
                        ReqItemFieldID = existingReqrmntField.RequirementItemFields.FirstOrDefault().RIF_ID;
                        BALUtils.GetSharedRequirementPackageRepoInstance().SaveUpdateRequirementFieldEditable(reqFieldContract, ReqItemFieldID, currentLoggedInUserId);
                    }
                }
                if (!reqFieldContract.UniversalAttributeData.IsNullOrEmpty())
                {
                    UniversalMappingDataManager.SaveUniversalRequirmentAttributeMappingData(ConvertToUniversalRotationMappingContract(reqFieldContract, ReqItemFieldID), currentLoggedInUserId);
                }
                return reqFieldContract.RequirementFieldID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to requirement field 
        /// </summary>
        /// <returns></returns>
        public static RequirementFieldContract GetRequirementFieldDataByID(Int32 requirementFieldID, Int32 requirementCategoryID)
        {
            try
            {
                RequirementField reqmentField = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementFieldByFieldID(requirementFieldID);
                RequirementFieldContract requirementFieldContract = new RequirementFieldContract();
                if (!reqmentField.IsNullOrEmpty())
                {
                    requirementFieldContract.RequirementFieldData = new RequirementFieldDataContract();
                    requirementFieldContract.RequirementFieldName = reqmentField.RF_FieldName;
                    requirementFieldContract.RequirementFieldLabel = reqmentField.RF_FieldLabel;
                    requirementFieldContract.RequirementFieldData.RequirementFieldDataTypeCode = reqmentField.lkpRequirementFieldDataType.RFDT_Code;
                    requirementFieldContract.RequirementFieldCode = reqmentField.RF_Code.HasValue ? reqmentField.RF_Code.Value : Guid.Empty;
                    requirementFieldContract.RequirementFieldData.AttributeTypeID = reqmentField.RF_AttributeTypeID;
                    requirementFieldContract.RequirementFieldAttributeGroupId = Convert.ToInt32(reqmentField.RF_RequirementAttributeGroupID); //UAT-3176
                    #region UAT-3078

                    RequirementItemField requiremtItemField = reqmentField.RequirementItemFields.FirstOrDefault(cnd => cnd.RIF_RequirementFieldID == requirementFieldID && !cnd.RIF_IsDeleted);
                    if (!requiremtItemField.IsNullOrEmpty())
                    {
                        requirementFieldContract.RequirementFieldDisplayOrder = requiremtItemField.RIF_DisplayOrder.HasValue ? requiremtItemField.RIF_DisplayOrder.Value : AppConsts.NONE; //UAT-3078
                    }
                    RequirementObjectProperty reqObjectProperty = BALUtils.GetSharedRequirementPackageRepoInstance().GetReqObjectProperty(requiremtItemField.RIF_ID, requiremtItemField.RIF_RequirementItemID, requirementCategoryID);
                    if (!reqObjectProperty.IsNullOrEmpty())
                    {
                        Dictionary<String, Boolean> dicData = new Dictionary<String, Boolean>();

                        if (!reqObjectProperty.ROTP_IsCustomSettings.IsNullOrEmpty() && Convert.ToBoolean(reqObjectProperty.ROTP_IsCustomSettings))
                        {
                            dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), reqObjectProperty.ROTP_IsEditableByAdmin);
                            dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), reqObjectProperty.ROTP_IsEditableByClientAdmin);
                            dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), reqObjectProperty.ROTP_IsEditableByApplicant);
                            requirementFieldContract.IsCustomSetting = true;
                            requirementFieldContract.SelectedEditableBy = dicData;
                        }
                        else
                        {
                            dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), false);
                            dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), false);
                            dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), false);
                            requirementFieldContract.IsCustomSetting = false;
                            requirementFieldContract.SelectedEditableBy = dicData;
                        }
                    }
                    #endregion

                    if (requirementFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                    {
                        requirementFieldContract.RequirementFieldData.VideoFieldData = new RequirementFieldVideoData();

                        RequirementFieldVideo reqFieldVideo = reqmentField.RequirementFieldVideos.FirstOrDefault(cnd => !cnd.RFV_IsDeleted);

                        requirementFieldContract.RequirementFieldData.VideoFieldData.VideoName = reqFieldVideo.RFV_VideoName;
                        requirementFieldContract.RequirementFieldData.VideoFieldData.VideoURL = reqFieldVideo.RFV_VideoURL;
                    }
                    else if (requirementFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue())
                    {
                        requirementFieldContract.RequirementFieldMaxLength = reqmentField.RF_MaximumCharacters;
                    }
                    else if (requirementFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                    {
                        List<RequirementFieldOption> requirementFieldOptionList = reqmentField.RequirementFieldOptions.Where(x => !x.RFO_IsDeleted).ToList();
                        requirementFieldContract.RequirementFieldData.LstRequirementFieldOptions = new List<RequirementFieldOptionsData>();
                        if (!requirementFieldOptionList.IsNullOrEmpty())
                        {
                            requirementFieldOptionList.ForEach(optDB =>
                            {
                                RequirementFieldOptionsData optData = new RequirementFieldOptionsData();
                                optData.OptionText = optDB.RFO_OptionText;
                                optData.OptionValue = optDB.RFO_OptionValue;
                                optData.RequirementFieldOptionsID = optDB.RFO_ID;
                                requirementFieldContract.RequirementFieldData.LstRequirementFieldOptions.Add(optData);
                            });
                        }
                    }
                    else if (requirementFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                    {
                        RequirementFieldDocument reqFieldDoc = reqmentField.RequirementFieldDocuments.FirstOrDefault(cnd => !cnd.RFD_IsDeleted);
                        requirementFieldContract.RequirementFieldData.FieldViewDocumentData = new RequirementFieldViewDocumentData();
                        if (!reqFieldDoc.IsNullOrEmpty())
                        {
                            requirementFieldContract.RequirementFieldData.FieldViewDocumentData.ClientSystemDocumentID = reqFieldDoc.RFD_ClientSystemDocumentID.Value;
                            requirementFieldContract.RequirementFieldData.FieldViewDocumentData.DocumentPath = reqFieldDoc.ClientSystemDocument.CSD_DocumentPath;
                            requirementFieldContract.RequirementFieldData.FieldViewDocumentData.DocumentSize = reqFieldDoc.ClientSystemDocument.CSD_Size.HasValue ?
                                                                                                               reqFieldDoc.ClientSystemDocument.CSD_Size.Value : AppConsts.NONE;
                            requirementFieldContract.RequirementFieldData.FieldViewDocumentData.DocumentFileName = reqFieldDoc.ClientSystemDocument.CSD_FileName;
                            requirementFieldContract.RequirementFieldData.FieldViewDocumentData.DocumentDescription = reqFieldDoc.ClientSystemDocument.CSD_Description;
                        }
                    }
                    else if (requirementFieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
                    {
                        if (requiremtItemField.IsNotNull())
                        {
                            requirementFieldContract.IsBackgroundDocument = requiremtItemField.RIF_IsBackgroundDocument.IsNotNull() ? requiremtItemField.RIF_IsBackgroundDocument.Value : false;
                        }
                    }
                }
                return requirementFieldContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #endregion

        public static RequirementCategoryContract GetRequirementCategoryDetailByCategoryID(int rqrmntCtgryID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementCategoryDetailByCategoryID(rqrmntCtgryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementCategoryContract> GetRequirementCategoriesByPackageID(Int32 rqrmntPkgID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementCategoriesByPackageID(rqrmntPkgID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteReqPackageCategoryMapping(Int32 reqPkgCtgryID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().DeleteReqPackageCategoryMapping(reqPkgCtgryID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveRequirementCategoryDetails(RequirementCategoryContract requirementCategoryContract, Int32 currentLoggedInUserID)
        {
            try
            {
                Int32 addedCategoryID = BALUtils.GetSharedRequirementPackageRepoInstance().
                                                    SaveRequirementCategoryDetails(requirementCategoryContract, currentLoggedInUserID);
                if (addedCategoryID != requirementCategoryContract.RequirementCategoryID)
                {
                    AddRequirementObjectTree(requirementCategoryContract.RequirementPackageID, currentLoggedInUserID);
                }
                //UAT-2305
                //if ((requirementCategoryContract.UniversalCategoryID > AppConsts.NONE || requirementCategoryContract.UniversalReqCatMappingID > AppConsts.NONE)
                //        && addedCategoryID > AppConsts.NONE)
                //{
                //    UniversalMappingDataManager.SaveUniversalRequirmentCategoryMappingData(ConvertToUniversalRequirementContract(requirementCategoryContract, addedCategoryID), currentLoggedInUserID);
                //}
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1837.
        public static List<RequirementTreeContract> GetRequirementTree(Int32 requirementPackageID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementTree(requirementPackageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementRuleContract> GetRequirementRuleDetail(Int32 requirementObjectTreeID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementRuleDetail(requirementObjectTreeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateRequirementRule(List<RequirementRuleContract> lstRequirementRule, Int32 currentLoggedInUserID)
        {
            try
            {
                RequirementRuleContract defaultRule = lstRequirementRule.FirstOrDefault();

                RequirementObjectTree requirmntObjTreeFromDb = BALUtils.GetSharedRequirementPackageRepoInstance().GetExistingRequirementObjectTree(defaultRule.ObjectTreeID);
                if (!requirmntObjTreeFromDb.IsNullOrEmpty())
                {
                    List<lkpObjectType> lstlkpObjectType = LookupManager.GetSharedDBLookUpData<lkpObjectType>().ToList();

                    List<lkpFixedRuleType> lstFixedRuleType = LookupManager.GetSharedDBLookUpData<lkpFixedRuleType>().
                                                                                                Where(cond => !cond.FRLT_IsDeleted).ToList();
                    List<lkpRuleActionType> lstRuleActionType = LookupManager.GetSharedDBLookUpData<lkpRuleActionType>().
                                                                                                  Where(cond => !cond.ACT_IsDeleted).ToList();

                    List<lkpRuleObjectMappingType> lstRuleObjectMappingType = LookupManager.GetSharedDBLookUpData<lkpRuleObjectMappingType>().
                                                                                                 Where(cond => !cond.RMT_IsDeleted).ToList();

                    String attrObjTypeCode = ObjectType.Compliance_ATR.GetStringValue();
                    String itmObjTypeCode = ObjectType.Compliance_Item.GetStringValue();
                    String catObjTypeCode = ObjectType.Compliance_Category.GetStringValue();
                    String pkgObjTypeCode = ObjectType.Compliance_Package.GetStringValue();
                    lkpObjectType attrObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == attrObjTypeCode);
                    lkpObjectType itmObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == itmObjTypeCode);
                    lkpObjectType catObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == catObjTypeCode);
                    lkpObjectType pkgObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == pkgObjTypeCode);

                    lkpRuleType buisnessRuleType = LookupManager.GetSharedDBLookUpData<lkpRuleType>().FirstOrDefault(cond => cond.RLT_Code ==
                                                                                                  ComplianceRuleType.BuisnessRules.GetStringValue());

                    lkpRuleType uiRuleType = LookupManager.GetSharedDBLookUpData<lkpRuleType>().FirstOrDefault(cond => cond.RLT_Code ==
                                                                                                  ComplianceRuleType.UIRules.GetStringValue());

                    lkpRuleActionType defaultActionType = lstRuleActionType.FirstOrDefault(cond => cond.ACT_Code == "DFLT");
                    lkpRuleActionType itemExpiratnActionType = lstRuleActionType.FirstOrDefault(cond => cond.ACT_Code == "SEXP");

                    String objectTypeCode = defaultRule.ObjectTypeCode;

                    if (objectTypeCode == ObjectType.Compliance_Package.GetStringValue())
                    {

                        //Package compliance rule.
                        String pkgRuleName = String.Empty;
                        CreateApprovalRule(currentLoggedInUserID, defaultRule, requirmntObjTreeFromDb, lstFixedRuleType, buisnessRuleType, defaultActionType, pkgRuleName);
                    }

                    else if (objectTypeCode == ObjectType.Compliance_Category.GetStringValue())
                    {
                        //Category compliance rule.
                        String catRuleName = String.Empty;
                        CreateApprovalRule(currentLoggedInUserID, defaultRule, requirmntObjTreeFromDb, lstFixedRuleType, buisnessRuleType, defaultActionType, catRuleName);
                    }

                    else if (objectTypeCode == ObjectType.Compliance_Item.GetStringValue())
                    {
                        RequirementItem requirementItem = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementItemDetail(defaultRule.ObjectID);

                        List<RequirementItemField> requirementItemFields = requirementItem.RequirementItemFields.Where(cond => !cond.RIF_IsDeleted).ToList();

                        //Category compliance rule.
                        String itmRuleName = requirementItem.RI_ItemName + "_ItemApprovalRule";

                        RequirementRuleContract itemApprovalRule = lstRequirementRule.Where(cond => cond.FixedRuleTypeCode != RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()
                                                                         && cond.FixedRuleTypeCode != RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                         && cond.FixedRuleTypeCode != RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue() //UAT-2165
                                                                         ).FirstOrDefault();


                        CreateApprovalRule(currentLoggedInUserID, itemApprovalRule, requirmntObjTreeFromDb, lstFixedRuleType, buisnessRuleType, defaultActionType, itmRuleName);

                        RequirementRuleContract itemExpiration = lstRequirementRule.Where(cond => cond.FixedRuleTypeCode == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()
                                                                          || cond.FixedRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                          || cond.FixedRuleTypeCode == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue() //UAT-2165
                                                                          ).FirstOrDefault();
                        if (!itemExpiration.IsNullOrEmpty())
                        {
                            CreateUpdateItemExpirationRule(currentLoggedInUserID, lstRuleObjectMappingType, lstFixedRuleType, attrObjectType
                                                                                , buisnessRuleType, itemExpiratnActionType, requirementItem,
                                                                                requirementItemFields, itemExpiration, requirmntObjTreeFromDb);
                        }
                    }

                    else if (objectTypeCode == ObjectType.Compliance_ATR.GetStringValue())
                    {
                        List<lkpObjectAttribute> lstlkpObjectAttribue = LookupManager.GetSharedDBLookUpData<lkpObjectAttribute>().
                                                                                          Where(cond => !cond.OA_IsDeleted).ToList();
                        SaveUpdateRequirementFieldRule(lstRequirementRule, currentLoggedInUserID, defaultRule, requirmntObjTreeFromDb, lstlkpObjectAttribue);


                        RequirementField requirementField = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementFieldByFieldID(defaultRule.ObjectID);


                        //Category compliance rule.
                        String fieldUIRuleName = requirementField.RF_FieldName + "_UIRule";

                        RequirementRuleContract fieldUIRule = lstRequirementRule.Where(cond => cond.FixedRuleTypeCode == RequirementFixedRuleType.GREATER_THAN.GetStringValue()
                                                                         || cond.FixedRuleTypeCode == RequirementFixedRuleType.LESS_THAN.GetStringValue()
                                                                         ).FirstOrDefault();

                        CreateUpdateFieldUiRule(currentLoggedInUserID, fieldUIRule, lstRuleObjectMappingType, requirmntObjTreeFromDb, lstFixedRuleType, uiRuleType, defaultActionType, fieldUIRuleName);



                    }

                    return BALUtils.GetSharedRequirementPackageRepoInstance().SaveContextIntoDataBase();
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        private static void CreateApprovalRule(Int32 currentLoggedInUserID, RequirementRuleContract defaultRule,
                                                                             RequirementObjectTree requirmntObjTreeFromDb, List<lkpFixedRuleType> lstFixedRuleType,
                                                                             lkpRuleType buisnessRuleType, lkpRuleActionType defaultActionType, String ruleName)
        {
            if (defaultRule.RequirementObjectRuleId == AppConsts.NONE)
            {
                //insert new rule.
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = ruleName;
                newObjectRule.ROR_ObjectTreeId = defaultRule.ObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;


                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == defaultRule.FixedRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;

                newObjectRule.ROR_UIExpression = !string.IsNullOrEmpty(defaultRule.RuleUIExpression) ? defaultRule.RuleUIExpression : null;
                newObjectRule.ROR_SqlExpression = !string.IsNullOrEmpty(defaultRule.RuleSqlExpression) ? defaultRule.RuleSqlExpression : null;

                requirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                if (requirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    //update existing rule
                    RequirementObjectRule existingObjectRule = requirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                            && cond.lkpFixedRuleType.FRLT_Code != RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()
                                                                                            && cond.lkpFixedRuleType.FRLT_Code != RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                                            && cond.lkpFixedRuleType.FRLT_Code != RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue());
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == defaultRule.FixedRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;

                        existingObjectRule.ROR_UIExpression = !string.IsNullOrEmpty(defaultRule.RuleUIExpression) ? defaultRule.RuleUIExpression : null;
                        existingObjectRule.ROR_SqlExpression = !string.IsNullOrEmpty(defaultRule.RuleSqlExpression) ? defaultRule.RuleSqlExpression : null;
                    }
                }
            }
        }

        private static void CreateUpdateItemExpirationRule(Int32 currentLoggedInUserID, List<lkpRuleObjectMappingType> lstRuleObjectMappingType, List<lkpFixedRuleType> lstFixedRuleType,
                                                                                        lkpObjectType attrObjectType, lkpRuleType buisnessRuleType, lkpRuleActionType itemExpiratnActionType,
                                                                                        RequirementItem requirmntItem, IEnumerable<RequirementItemField> requirmntItmFields,
                                                                                        RequirementRuleContract requirementItemExpirationRule, RequirementObjectTree itmRequirmntObjTreeFromDb)
        {

            lkpRuleObjectMappingType constMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "CONST");
            lkpRuleObjectMappingType dataValueMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "DVAL");

            RequirementObjectRule existingObjectRule = itmRequirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                                       && cond.ROR_ActionType == itemExpiratnActionType.ACT_ID);
            String dateConstantTypeCode = ConstantType.Date.GetStringValue();
            lkpConstantType dateConstantType = LookupManager.GetSharedDBLookUpData<lkpConstantType>().
                                                                                            Where(cond => !cond.IsDeleted).FirstOrDefault();

            if (!requirementItemExpirationRule.IsNullOrEmpty())
            {
                if (existingObjectRule.IsNullOrEmpty())
                {
                    InsertItemExpirationRule(currentLoggedInUserID, constMappingType, dataValueMappingType, lstFixedRuleType,
                                            attrObjectType, buisnessRuleType, itemExpiratnActionType, dateConstantType
                                            , requirmntItmFields, requirementItemExpirationRule
                                            , itmRequirmntObjTreeFromDb, requirmntItem);
                }
                else
                {
                    String exitingFixedRuleTypeCode = existingObjectRule.lkpFixedRuleType.FRLT_Code;

                    existingObjectRule.ROR_Name = requirmntItem.RI_ItemName + "_ItemExpirationRule";
                    existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementItemExpirationRule.RequirementItemExpirationTypeCode).FRLT_ID;
                    existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                    existingObjectRule.ROR_ModifiedOn = DateTime.Now;

                    List<RequirementObjectRuleDetail> requirementObjectRuleDetailList = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).ToList();
                    if (requirementItemExpirationRule.RequirementItemExpirationTypeCode
                        == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
                    {
                        RequirementObjectRuleDetail requirementObjectRuleDetail = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                        requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                        requirementObjectRuleDetail.RORD_ConstantValue = requirementItemExpirationRule.ExpirationDate;
                        requirementObjectRuleDetail.RORD_ObjectTreeID = null;
                        requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                        if (exitingFixedRuleTypeCode != requirementItemExpirationRule.RequirementItemExpirationTypeCode)
                        {
                            RequirementObjectRuleDetail requirementObjectRuleDetailToBeDeleted = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            if (!requirementObjectRuleDetailToBeDeleted.IsNullOrEmpty())
                            {
                                requirementObjectRuleDetailToBeDeleted.RORD_IsDeleted = true;
                                requirementObjectRuleDetailToBeDeleted.RORD_ModifiedByID = currentLoggedInUserID;
                                requirementObjectRuleDetailToBeDeleted.RORD_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                    else if (requirementItemExpirationRule.RequirementItemExpirationTypeCode
                        == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
                    {
                        //UAT-2165
                        RequirementObjectRuleDetail requirementObjectRuleDetail = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                        requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                        requirementObjectRuleDetail.RORD_ConstantValue = requirementItemExpirationRule.ExpirationDate;
                        requirementObjectRuleDetail.RORD_ExpirationCondEndDate = requirementItemExpirationRule.ExpirationCondEndDate;
                        requirementObjectRuleDetail.RORD_ExpirationCondStartDate = requirementItemExpirationRule.ExpirationCondStartDate;
                        RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_ID == requirementItemExpirationRule.SelectedDateTypeFieldId)
                                                                                                                                                                    .FirstOrDefault().RequirementField;
                        if (exprationDateField.IsNotNull())
                        {
                            RequirementObjectTree exprationDateObjTree = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeDetail(exprationDateField.RF_ID, requirementItemExpirationRule.ObjectTreeID);
                            requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.ROT_ID;
                        }
                        requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                        if (exitingFixedRuleTypeCode != requirementItemExpirationRule.RequirementItemExpirationTypeCode)
                        {
                            RequirementObjectRuleDetail requirementObjectRuleDetailToBeDeleted = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            if (!requirementObjectRuleDetailToBeDeleted.IsNullOrEmpty())
                            {
                                requirementObjectRuleDetailToBeDeleted.RORD_IsDeleted = true;
                                requirementObjectRuleDetailToBeDeleted.RORD_ModifiedByID = currentLoggedInUserID;
                                requirementObjectRuleDetailToBeDeleted.RORD_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        RequirementObjectRuleDetail requirementObjectRuleDetailForDataValue = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetailForDataValue.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                        requirementObjectRuleDetailForDataValue.RORD_ConstantType = null;
                        requirementObjectRuleDetailForDataValue.RORD_ConstantValue = null;
                        requirementObjectRuleDetailForDataValue.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailForDataValue.RORD_ModifiedOn = DateTime.Now;
                        RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_ID == requirementItemExpirationRule.SelectedDateTypeFieldId)
                                                                                                                                                                    .FirstOrDefault().RequirementField;
                        if (exprationDateField.IsNotNull())
                        {
                            RequirementObjectTree exprationDateObjTree = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeDetail(exprationDateField.RF_ID, requirementItemExpirationRule.ObjectTreeID);
                            requirementObjectRuleDetailForDataValue.RORD_ObjectTreeID = exprationDateObjTree.ROT_ID;
                        }
                        RequirementObjectRuleDetail requirementObjectRuleDetailForContantValue;
                        if (exitingFixedRuleTypeCode != requirementItemExpirationRule.RequirementItemExpirationTypeCode)
                        {
                            requirementObjectRuleDetailForContantValue = new RequirementObjectRuleDetail();
                            requirementObjectRuleDetailForContantValue.RORD_ParameterOrder = AppConsts.TWO;
                            requirementObjectRuleDetailForContantValue.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantType = requirementItemExpirationRule.ExpirationValueTypeID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantValue = requirementItemExpirationRule.ExpirationValue.ToString();
                            requirementObjectRuleDetailForContantValue.RORD_IsDeleted = false;
                            requirementObjectRuleDetailForContantValue.RORD_CreatedByID = currentLoggedInUserID;
                            requirementObjectRuleDetailForContantValue.RORD_CreatedOn = DateTime.Now;
                            existingObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetailForContantValue);
                        }
                        else
                        {
                            requirementObjectRuleDetailForContantValue = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            requirementObjectRuleDetailForContantValue.RORD_ParameterOrder = AppConsts.TWO;
                            requirementObjectRuleDetailForContantValue.RORD_ModifiedByID = currentLoggedInUserID;
                            requirementObjectRuleDetailForContantValue.RORD_ModifiedOn = DateTime.Now;
                            requirementObjectRuleDetailForContantValue.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantType = requirementItemExpirationRule.ExpirationValueTypeID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantValue = requirementItemExpirationRule.ExpirationValue.ToString();
                        }
                    }
                }
            }
            if (!requirementItemExpirationRule.IsRequirementItemNeededExpiration && !existingObjectRule.IsNullOrEmpty())
            {
                //if expiration is not needed and rule already exist for expiration.
                existingObjectRule.ROR_IsDeleted = true;
                existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                List<RequirementObjectRuleDetail> requirementObjectRuleDetailList = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted)
                                                                                                                                               .ToList();
                foreach (RequirementObjectRuleDetail requirementObjectRuleDetail in requirementObjectRuleDetailList)
                {
                    requirementObjectRuleDetail.RORD_IsDeleted = true;
                    requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                    requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                }
            }

        }

        private static void InsertItemExpirationRule(Int32 currentLoggedInUserID, lkpRuleObjectMappingType constMappingType, lkpRuleObjectMappingType dataValueMappingType,
                                                                                        List<lkpFixedRuleType> lstFixedRuleType, lkpObjectType attrObjectType, lkpRuleType buisnessRuleType
                                                                                        , lkpRuleActionType itemExpiratnActionType, lkpConstantType dateConstantType, IEnumerable<RequirementItemField> requirmntItmFields
                                                                                       , RequirementRuleContract itmExpirationRule, RequirementObjectTree itmRequirmntObjTreeFromDb, RequirementItem requirementItem)
        {
            RequirementObjectRule newExpirationObjectRule = new RequirementObjectRule();
            newExpirationObjectRule.ROR_RuleTemplateID = null;
            newExpirationObjectRule.ROR_Code = Guid.NewGuid();
            newExpirationObjectRule.ROR_Name = requirementItem.RI_ItemName + "_ItemExpirationRule";
            newExpirationObjectRule.ROR_ObjectTreeId = itmExpirationRule.ObjectTreeID;
            newExpirationObjectRule.ROR_RuleTemplateID = null;
            newExpirationObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == itmExpirationRule.RequirementItemExpirationTypeCode).FRLT_ID;
            newExpirationObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
            newExpirationObjectRule.ROR_ActionType = itemExpiratnActionType.ACT_ID;
            newExpirationObjectRule.ROR_FirstVersionID = null;
            newExpirationObjectRule.ROR_IsCurrent = true;
            newExpirationObjectRule.ROR_IsActive = true;
            newExpirationObjectRule.ROR_IsDeleted = false;
            newExpirationObjectRule.ROR_CreatedByID = currentLoggedInUserID;
            newExpirationObjectRule.ROR_CreatedOn = DateTime.Now;

            RequirementObjectRuleDetail requirementObjectRuleDetail = new RequirementObjectRuleDetail();
            requirementObjectRuleDetail.RORD_PlaceHolderName = null;
            requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
            requirementObjectRuleDetail.RORD_ObjectTreeID = null;
            if (itmExpirationRule.RequirementItemExpirationTypeCode
                           == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
            {
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                requirementObjectRuleDetail.RORD_ConstantValue = itmExpirationRule.ExpirationDate;
            }
            else if (itmExpirationRule.RequirementItemExpirationTypeCode
                == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
            {
                //UAT-2165
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                requirementObjectRuleDetail.RORD_ConstantValue = itmExpirationRule.ExpirationDate;
                requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                requirementObjectRuleDetail.RORD_ExpirationCondStartDate = itmExpirationRule.ExpirationCondStartDate;
                requirementObjectRuleDetail.RORD_ExpirationCondEndDate = itmExpirationRule.ExpirationCondEndDate;
                RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_ID == itmExpirationRule.SelectedDateTypeFieldId)
                                                                                                                           .FirstOrDefault().RequirementField;
                if (exprationDateField.IsNotNull())
                {
                    RequirementObjectTree exprationDateObjTree = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeDetail(exprationDateField.RF_ID, itmExpirationRule.ObjectTreeID);
                    requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.ROT_ID;
                }
            }
            else
            {
                requirementObjectRuleDetail.RORD_ParameterOrder = 2;
                requirementObjectRuleDetail.RORD_ConstantType = itmExpirationRule.ExpirationValueTypeID;
                requirementObjectRuleDetail.RORD_ConstantValue = itmExpirationRule.ExpirationValue.ToString();
            }
            requirementObjectRuleDetail.RORD_IsDeleted = false;
            requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
            requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;
            newExpirationObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);

            if (itmExpirationRule.RequirementItemExpirationTypeCode
               == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue())
            {
                requirementObjectRuleDetail = new RequirementObjectRuleDetail();
                requirementObjectRuleDetail.RORD_PlaceHolderName = null;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_ConstantType = null;
                requirementObjectRuleDetail.RORD_ConstantValue = null;
                requirementObjectRuleDetail.RORD_ObjectTreeID = null;
                requirementObjectRuleDetail.RORD_IsDeleted = false;
                requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
                requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;

                RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_ID == itmExpirationRule.SelectedDateTypeFieldId)
                                                                                                                           .FirstOrDefault().RequirementField;
                if (exprationDateField.IsNotNull())
                {
                    RequirementObjectTree exprationDateObjTree = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeDetail(exprationDateField.RF_ID, itmExpirationRule.ObjectTreeID);
                    requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.ROT_ID;
                }
                newExpirationObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);
            }
            itmRequirmntObjTreeFromDb.RequirementObjectRules.Add(newExpirationObjectRule);
        }


        private static void SaveUpdateRequirementFieldRule(List<RequirementRuleContract> lstRequirementRule, Int32 currentLoggedInUserID, RequirementRuleContract defaultRule, RequirementObjectTree requirmntObjTreeFromDb, List<lkpObjectAttribute> lstlkpObjectAttribue)
        {
            String requiredObjectAttributeCode = ObjectAttribute.REQUIRED.GetStringValue();
            String signatureRequiredObjectAttributeCode = ObjectAttribute.SIGNATURE_REQUIRED.GetStringValue();
            String requiredToViewObjectAttributeCode = ObjectAttribute.REQUIRED_TO_VIEW.GetStringValue();
            String requiredToOpeAttributeCode = ObjectAttribute.REQUIRED_TO_OPEN.GetStringValue();
            String boxStayOpenObjectAttributeCode = ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue();
            lkpObjectAttribute requiredObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredObjectAttributeCode);
            lkpObjectAttribute signatureRequiredObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == signatureRequiredObjectAttributeCode);
            lkpObjectAttribute requiredToViewObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredToViewObjectAttributeCode);
            lkpObjectAttribute requiredToOpenObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredToOpeAttributeCode);
            lkpObjectAttribute boxStayOpenObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == boxStayOpenObjectAttributeCode);

            RequirementObjectTree attrRequirmntObjTreeFromDb = requirmntObjTreeFromDb;
            if (defaultRule.RequirementFieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue()
               || defaultRule.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue()
               || defaultRule.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()
               || defaultRule.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue()
                || defaultRule.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue()
                || defaultRule.RequirementFieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue()
                )
            {

                RequirementRuleContract requiredRule = lstRequirementRule.FirstOrDefault(cond => cond.RequirementFieldRuleTypeCode == requiredObjectAttributeCode);
                InsertUpdateRequiredFieldProperty(currentLoggedInUserID, requiredObjectAttribute, requiredRule, attrRequirmntObjTreeFromDb);
            }

            if (defaultRule.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
            {
                RequirementRuleContract requiredToOpeAttributeRule = lstRequirementRule.FirstOrDefault(cond => cond.RequirementFieldRuleTypeCode == requiredToOpeAttributeCode);
                if (!requiredToOpeAttributeRule.IsNullOrEmpty())
                {
                    InsertUpdateRequiredToOpenFieldProperty(currentLoggedInUserID, requiredToOpenObjectAttribute, requiredToOpeAttributeRule, attrRequirmntObjTreeFromDb);

                    RequirementRuleContract boxStayOpenObjectAttributeRule = lstRequirementRule.FirstOrDefault(cond => cond.RequirementFieldRuleTypeCode == boxStayOpenObjectAttributeCode);

                    if (!boxStayOpenObjectAttributeRule.IsNullOrEmpty())// && VideoFieldData.VideoOpenTimeDuration == AppConsts.NONE)
                    {
                        InsertUpdateBoxStayOpenFieldProperty(currentLoggedInUserID, boxStayOpenObjectAttribute, boxStayOpenObjectAttributeRule, attrRequirmntObjTreeFromDb);
                    }
                }
            }
        }


        private static void InsertUpdateRequiredFieldProperty(Int32 currentLoggedInUserID, lkpObjectAttribute requiredObjectAttribute,
                                                              RequirementRuleContract requirementFieldContractObj, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.
                                                                                                        FirstOrDefault(cond => !cond.ROTP_IsDeleted);
            if (existingRequirementObjectTreeProperty.IsNullOrEmpty())
            {
                RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                newRequirementObjectTreeProperty.ROTP_ObjectTreeID = requirementFieldContractObj.ObjectTreeID;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredObjectAttribute.IsNotNull() ? requiredObjectAttribute.OA_ID : AppConsts.NONE;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
            }
            else
            {
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredObjectAttribute.IsNotNull() ? requiredObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateRequiredToOpenFieldProperty(Int32 currentLoggedInUserID, lkpObjectAttribute requiredToOpenObjectAttribute,
                                                                                        RequirementRuleContract requirementFieldContractObj, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                        && cond.ROTP_ObjectAttributeID == requiredToOpenObjectAttribute.OA_ID);
            if (existingRequirementObjectTreeProperty.IsNullOrEmpty())
            {
                RequirementObjectTreeProperty requiredToViewObjectTreeProperty = new RequirementObjectTreeProperty();
                requiredToViewObjectTreeProperty.ROTP_ObjectTreeID = requirementFieldContractObj.ObjectTreeID;
                requiredToViewObjectTreeProperty.ROTP_IsDeleted = false;
                requiredToViewObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                requiredToViewObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                requiredToViewObjectTreeProperty.ROTP_ObjectAttributeID = requiredToOpenObjectAttribute.IsNotNull() ?
                                                                           requiredToOpenObjectAttribute.OA_ID : AppConsts.NONE;
                requiredToViewObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.RequirementFieldRuleTypeValue;
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(requiredToViewObjectTreeProperty);
            }
            else
            {
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredToOpenObjectAttribute.IsNotNull() ?
                                                                           requiredToOpenObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.RequirementFieldRuleTypeValue;
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateBoxStayOpenFieldProperty(Int32 currentLoggedInUserID, lkpObjectAttribute boxStayOpenObjectAttribute,
                                                                 RequirementRuleContract requirementFieldContractObj, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                      && cond.ROTP_ObjectAttributeID == boxStayOpenObjectAttribute.OA_ID);
            if (existingRequirementObjectTreeProperty.IsNullOrEmpty())
            {
                RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                newRequirementObjectTreeProperty.ROTP_ObjectTreeID = requirementFieldContractObj.ObjectTreeID;
                newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = boxStayOpenObjectAttribute.IsNotNull() ?
                                                                           boxStayOpenObjectAttribute.OA_ID : AppConsts.NONE;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.VideoOpenTimeDuration.ToString();
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
            }
            else
            {
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = boxStayOpenObjectAttribute.IsNotNull() ?
                                                                       boxStayOpenObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.VideoOpenTimeDuration.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void CreateUpdateFieldUiRule(Int32 currentLoggedInUserID, RequirementRuleContract uiRule, List<lkpRuleObjectMappingType> lstRuleObjectMappingType,
                                                RequirementObjectTree requirmntObjTreeFromDb, List<lkpFixedRuleType> lstFixedRuleType,
                                               lkpRuleType uiRuleType, lkpRuleActionType defaultActionType, String ruleName)
        {
            lkpRuleObjectMappingType dataValueMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "DVAL");
            if (uiRule.IsNotNull() && uiRule.RequirementObjectRuleId == AppConsts.NONE)
            {
                //insert new rule.
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = ruleName;
                newObjectRule.ROR_ObjectTreeId = uiRule.ObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == uiRule.FixedRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_ErroMessage = uiRule.UiRuleErrorMessage;
                newObjectRule.ROR_RuleType = uiRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                newObjectRule.ROR_UIExpression = !string.IsNullOrEmpty(uiRule.RuleUIExpression) ? uiRule.RuleUIExpression : null;
                newObjectRule.ROR_SqlExpression = !string.IsNullOrEmpty(uiRule.RuleSqlExpression) ? uiRule.RuleSqlExpression : null;

                RequirementObjectRuleDetail requirementObjectRuleDetail = new RequirementObjectRuleDetail();
                requirementObjectRuleDetail.RORD_PlaceHolderName = null;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;

                RequirementObjectTree dateFieldObjTree = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeForField(uiRule.UiRequirementFieldID.Value, uiRule.UiRequirementItemID.Value);
                requirementObjectRuleDetail.RORD_ObjectTreeID = dateFieldObjTree.ROT_ID;
                requirementObjectRuleDetail.RORD_IsDeleted = false;
                requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
                requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;
                newObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);
                requirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                if (requirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    //update existing rule
                    RequirementObjectRule existingObjectRule = requirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                            && (cond.lkpFixedRuleType.FRLT_Code == RequirementFixedRuleType.LESS_THAN.GetStringValue()
                                                                                            || cond.lkpFixedRuleType.FRLT_Code == RequirementFixedRuleType.GREATER_THAN.GetStringValue()));
                    if (existingObjectRule.IsNotNull() && uiRule.IsNullOrEmpty())
                    {
                        //Delete exiting rule.
                        existingObjectRule.ROR_IsDeleted = true;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;

                        RequirementObjectRuleDetail requirementObjectRuleDetailInDb = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).FirstOrDefault();
                        requirementObjectRuleDetailInDb.RORD_IsDeleted = true;
                        requirementObjectRuleDetailInDb.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedOn = DateTime.Now;

                    }
                    else if (existingObjectRule.IsNotNull() && !uiRule.IsNullOrEmpty())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == uiRule.FixedRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ErroMessage = uiRule.UiRuleErrorMessage;
                        existingObjectRule.ROR_UIExpression = !string.IsNullOrEmpty(uiRule.RuleUIExpression) ? uiRule.RuleUIExpression : null;
                        existingObjectRule.ROR_SqlExpression = !string.IsNullOrEmpty(uiRule.RuleSqlExpression) ? uiRule.RuleSqlExpression : null;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;

                        RequirementObjectRuleDetail requirementObjectRuleDetailInDb = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).FirstOrDefault();
                        requirementObjectRuleDetailInDb.RORD_PlaceHolderName = null;
                        requirementObjectRuleDetailInDb.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;

                        RequirementObjectTree dateFieldObjTree = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeForField(uiRule.UiRequirementFieldID.Value, uiRule.UiRequirementItemID.Value);
                        requirementObjectRuleDetailInDb.RORD_ObjectTreeID = dateFieldObjTree.ROT_ID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedOn = DateTime.Now;

                    }
                }
            }
        }
        #endregion

        #region UAT-1828
        private static void SendRotationPackageVersioningNotification(RequirementPackageContract RequirementPkgContract)
        {
            //Create Dictionary
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, RequirementPkgContract.RequirementPackageName);

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = "Admin";
            mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
            mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

            List<Int32> tenantIds = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackageInstitution(RequirementPkgContract.RequirementPackageCode);

            foreach (Int32 tenantId in tenantIds)
            {
                //Send mail
                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.ROTATION_PACKAGE_VERSIONING_NOTIFICATION, dictMailData, mockData, tenantId, -1, null, null, true);
            }
        }
        #endregion

        /// <summary>
        /// Run a check on new rotation packages and see if they are complete/ready to be assigned from the Master Rotation Package screen
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        public static RequirementPackageCompletionContract CheckRequirementPackageCompletionStatus(Int32 requirementPackageID)
        {
            try
            {
                List<RequirementPackageHierarchicalDetailsContract> requirmntPkgHrchicalContract = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                                                           .GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageID);
                RequirementPackageContract requirementPackageDetailsContract = AssignPackageHierarchalDetailsToSessionContract(requirmntPkgHrchicalContract, Guid.Empty, true);
                RequirementPackageCompletionContract rqrmntCmpltnContract = new RequirementPackageCompletionContract();

                if (!requirementPackageDetailsContract.IsNullOrEmpty())
                {
                    rqrmntCmpltnContract.RequirementPackageID = requirementPackageDetailsContract.RequirementPackageID;
                    rqrmntCmpltnContract.RequirementPackageName = requirementPackageDetailsContract.RequirementPackageName;

                    rqrmntCmpltnContract.IncompleteCategoryNames = new List<String>();
                    rqrmntCmpltnContract.CategoriesWithoutRule = new List<String>();
                    rqrmntCmpltnContract.IncompleteItemNames = new List<String>();
                    rqrmntCmpltnContract.ItemsWithoutRule = new List<String>();
                    rqrmntCmpltnContract.IncompleteFieldNames = new List<String>();
                    rqrmntCmpltnContract.FieldsWithoutRule = new List<String>();

                    //check 1 : Check if package contain any rule
                    if (requirementPackageDetailsContract.PackageRuleTypeCode.IsNullOrEmpty())
                    {
                        rqrmntCmpltnContract.IsPackageRuleInComplete = true;
                    }

                    //check 2: check if package contain any category
                    if (requirementPackageDetailsContract.LstRequirementCategory.IsNullOrEmpty())
                    {
                        rqrmntCmpltnContract.IsPackageWithoutCategory = true;
                    }

                    foreach (RequirementCategoryContract category in requirementPackageDetailsContract.LstRequirementCategory)
                    {
                        //check 3: Check if category contain rule
                        if (category.CategoryRuleTypeCode.IsNullOrEmpty())
                        {
                            rqrmntCmpltnContract.CategoriesWithoutRule.Add(category.RequirementCategoryName);
                        }

                        //check 4 : Check if category contains any item
                        if (category.LstRequirementItem.IsNullOrEmpty())
                        {
                            rqrmntCmpltnContract.IncompleteCategoryNames.Add(category.RequirementCategoryName);
                        }

                        foreach (RequirementItemContract item in category.LstRequirementItem)
                        {
                            //check 5: Check if item contain rule
                            if (item.RequirementItemRuleTypeCode.IsNullOrEmpty())
                            {
                                rqrmntCmpltnContract.ItemsWithoutRule.Add(String.Concat(category.RequirementCategoryName, " > ", item.RequirementItemName));
                            }

                            //check 6 : Check if item contains any field
                            if (item.LstRequirementField.IsNullOrEmpty())
                            {
                                rqrmntCmpltnContract.IncompleteItemNames.Add(String.Concat(category.RequirementCategoryName, " > ", item.RequirementItemName));
                            }

                            foreach (RequirementFieldContract field in item.LstRequirementField)
                            {
                                //check 7: Check if field contain rule
                                if (field.IsFieldRuleNotDefined)
                                {
                                    rqrmntCmpltnContract.FieldsWithoutRule.Add(String.Concat(category.RequirementCategoryName, " > ", item.RequirementItemName, " > ", field.RequirementFieldName));
                                }
                            }
                        }
                    }
                }

                return rqrmntCmpltnContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-2305
        public static String GetRequirementFieldByFieldID(Int32 requirementFieldID)
        {
            try
            {
                RequirementField reqmentField = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementFieldByFieldID(requirementFieldID);
                if (reqmentField.IsNotNull())
                {
                    return reqmentField.lkpRequirementFieldDataType.RFDT_Code;
                }
                return String.Empty;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static List<RequirementPackage> GetMasterRequirementPackages()
        {
            try
            {
                String code = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                Int32 rotPkgTypeID = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpRequirementPackageType>().Where(x => x.RPT_Code == code).FirstOrDefault().RPT_ID;
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetMasterRequirementPackages(rotPkgTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #region UAT-2305 Master Rotation Package

        /// <summary>
        /// Convert Requirement category contract to universalRotationMappingViewContract
        /// </summary>
        /// <param name="requirementCategoryContract"></param>
        /// <param name="currentAddedCategoryID"></param>
        /// <returns></returns>
        private static UniversalRotationMappingViewContract ConvertToUniversalRequirementContract(RequirementCategoryContract requirementCategoryContract, Int32 currentAddedCategoryID)
        {
            UniversalRotationMappingViewContract contract = new UniversalRotationMappingViewContract();
            if (!requirementCategoryContract.IsNullOrEmpty() && currentAddedCategoryID > AppConsts.NONE)
            {
                contract.UniversalCategoryID = requirementCategoryContract.UniversalCategoryID;
                contract.UniversalReqCatMappingID = requirementCategoryContract.UniversalReqCatMappingID;
                contract.RequirementCategoryID = currentAddedCategoryID;
            }
            return contract;
        }

        private static UniversalRotationMappingViewContract ConvertToUniversalRotationMappingContract(RequirementItemContract requirementItemContract)
        {
            UniversalRotationMappingViewContract uniRotationMappingContract = new UniversalRotationMappingViewContract();
            if (!requirementItemContract.IsNullOrEmpty()
                && !requirementItemContract.UniversalItemContract.IsNullOrEmpty()
                && requirementItemContract.RequirementCategoryItemID > AppConsts.NONE)
            {
                uniRotationMappingContract.RequirementItemID = requirementItemContract.RequirementItemID;
                uniRotationMappingContract.RequirementCategoryItemID = requirementItemContract.RequirementCategoryItemID;
                uniRotationMappingContract.UniversalCatItemMappingID = requirementItemContract.UniversalItemContract.UniCatItmMappingID;
                uniRotationMappingContract.UniversalReqCatMappingID = requirementItemContract.UniversalItemContract.UniReqCatMappingID;
                uniRotationMappingContract.UniversalReqItemMappingID = requirementItemContract.UniversalItemContract.UniReqItmMappingID;
            }
            return uniRotationMappingContract;
        }

        private static UniversalRotationMappingViewContract ConvertToUniversalRotationMappingContract(RequirementFieldContract requirementFieldContract, Int32 ReqItmFieldID)
        {
            UniversalRotationMappingViewContract uniRotationMappingContract = new UniversalRotationMappingViewContract();
            List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> lstAtrInputData = new List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping>();

            if (!requirementFieldContract.IsNullOrEmpty()
                && !requirementFieldContract.UniversalAttributeData.IsNullOrEmpty()
                && ReqItmFieldID > AppConsts.NONE)
            {
                uniRotationMappingContract.RequirementItemFieldID = ReqItmFieldID;
                uniRotationMappingContract.UniversalFieldMappingID = requirementFieldContract.UniversalAttributeData.UniversalFieldMappingID;
                uniRotationMappingContract.UniversalFieldID = requirementFieldContract.UniversalAttributeData.UniversalFieldID;
                uniRotationMappingContract.RequirementCategoryID = requirementFieldContract.RequirementCategoryID;
                uniRotationMappingContract.RequirementItemID = requirementFieldContract.RequirementItemID;
                uniRotationMappingContract.RequirementPackageID = requirementFieldContract.RequirementPackageID;
                //uniRotationMappingContract.UniversalItemAttrMappingID = requirementFieldContract.UniversalAttributeData.UniItmAttrMappingID;
                //uniRotationMappingContract.UniversalReqItemMappingID = requirementFieldContract.UniversalAttributeData.UniReqItemMappingID;
                //uniRotationMappingContract.UniversalReqAttrMappingID = requirementFieldContract.UniversalAttributeData.UniReqAttrMappingID;
                if (!requirementFieldContract.UniversalAttributeData.lstAttributeInputData.IsNullOrEmpty())
                {
                    requirementFieldContract.UniversalAttributeData.lstAttributeInputData.ForEach(x =>
                    {
                        Entity.SharedDataEntity.UniversalFieldInputTypeMapping data = new Entity.SharedDataEntity.UniversalFieldInputTypeMapping();
                        data.UFITM_UniversalFieldID = x.ID;
                        data.UFITM_UniversalFieldMappingID = requirementFieldContract.UniversalAttributeData.UniversalFieldMappingID;
                        data.UFITM_InputPriority = x.InputPriority;
                        lstAtrInputData.Add(data);
                    });
                    uniRotationMappingContract.lstUniReqAttrInputMapping = lstAtrInputData;
                }
                if (!requirementFieldContract.UniversalAttributeData.lstOptionMapping.IsNullOrEmpty())
                {
                    RequirementField requirementFieldOption = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementFieldByFieldID(requirementFieldContract.RequirementFieldID);
                    Dictionary<Int32, String> RequirementFieldOption = new Dictionary<Int32, String>();
                    requirementFieldOption.RequirementFieldOptions.Where(cond => !cond.RFO_IsDeleted).ForEach(x =>
                    {
                        RequirementFieldOption.Add(x.RFO_ID, x.RFO_OptionText);
                    });
                    uniRotationMappingContract.lstUniversalRequirementAttributeOptionMapping = new List<Entity.SharedDataEntity.UniversalFieldOptionMapping>();
                    requirementFieldContract.UniversalAttributeData.lstOptionMapping.ForEach(x =>
                    {
                        UniversalFieldOptionMapping universalReqAtrOption = new UniversalFieldOptionMapping();
                        universalReqAtrOption.UFOM_AttributeOptionID = RequirementFieldOption.Where(cond => cond.Value == x.RequirementOptionText).FirstOrDefault().Key;
                        universalReqAtrOption.UFOM_UniversalFieldMappingID = x.UniversalReqAtrMappingID;
                        universalReqAtrOption.UFOM_UniversalFieldOptionID = x.UniversalAtrOptionID;
                        uniRotationMappingContract.lstUniversalRequirementAttributeOptionMapping.Add(universalReqAtrOption);
                    });
                }
            }
            return uniRotationMappingContract;
        }

        #endregion

        #region UAT-2332
        public static List<DefinedRequirementContract> GetDefinedRequirement()
        {
            try
            {
                List<lkpDefinedRequirement> dataTypeList = LookupManager.GetSharedDBLookUpData<lkpDefinedRequirement>().Where(x => !x.DR_IsDeleted && x.DR_IsActive).ToList();
                return dataTypeList.Select(con => new DefinedRequirementContract
                {
                    ID = con.DR_ID,
                    Code = con.DR_Code,
                    Description = con.DR_Description
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-3071

        public static List<DefinedRequirementContract> GetFieldType()
        {
            try
            {

                List<lkpJobFieldType> dataTypeList = LookupManager.GetSharedDBLookUpData<lkpJobFieldType>().Where(x => !x.JFT_IsDeleted).ToList();
                return dataTypeList.Select(con => new DefinedRequirementContract
                {
                    ID = con.JFT_ID,
                    Code = con.JFT_Code,
                    Description = con.JFT_Name
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        #region UAT-2213 Setup Tree Methods
        /// <summary>
        /// Fetch Setup Tree Data by Requirement Category ID
        /// </summary>
        /// <param name="reqCategoryID"></param>
        /// <returns></returns>
        public static List<RotationMappingContract> GetRotationMappingTreeData(Int32 reqCategoryID)
        {
            try
            {
                IEnumerable<DataRow> rows = BALUtils.GetSharedRequirementPackageRepoInstance().GetRotationMappingTreeData(reqCategoryID).AsEnumerable();
                return ConvertToRotationMappingContract(rows);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<RequirementPackageContract> GetMasterRequirementPackageDetails(RequirementPackageContract requirementPackageDetailsContract
                                                                                           , CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                List<RequirementPackageContract> lstReqPkgContract = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                        .GetMasterRequirementPackageDetails(requirementPackageDetailsContract, customPagingArgsContract);

                List<RequirementPackage> requirementPackages = BALUtils.GetSharedRequirementPackageRepoInstance().GetMasterRequirementPackages();

                lstReqPkgContract.ForEach(x =>
                {
                    List<Int32> agencyHIerarchyIds = BALUtils.GetSharedRequirementPackageRepoInstance().GetAgencyHierarchyIdsWithPkgId(x.RequirementPackageID);

                    //UAT-4657
                    if (!agencyHIerarchyIds.IsNullOrEmpty() && agencyHIerarchyIds.Count > AppConsts.NONE)
                        x.HierarchyIds = String.Join(",", agencyHIerarchyIds);

                    x.ParentPackageName = requirementPackages.Where(con => con.RP_Code == x.ParentPackageCode).Select(sel => sel.RP_PackageName).FirstOrDefault();

                    x.SelectedAgencyHierarchyDeatils = new Dictionary<Int32, String>();
                    foreach (Int32 agencyHierachyId in agencyHIerarchyIds.Distinct())
                    {
                        x.SelectedAgencyHierarchyDeatils.Add(agencyHierachyId, String.Empty);
                    }
                });

                return lstReqPkgContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Convert dataRow to RotationMappingContact for Tree
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private static List<RotationMappingContract> ConvertToRotationMappingContract(IEnumerable<DataRow> rows)
        {
            return rows.Select(x => new RotationMappingContract
            {
                TreeNodeTypeID = x["TreeNodeTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["TreeNodeTypeID"]),
                NodeID = x["NodeID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["NodeID"]),
                ParentNodeID = x["ParentNodeID"].GetType().Name == "DBNull" ? null : Convert.ToString(x["ParentNodeID"]),
                Level = x["Level"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["Level"]),
                DataID = x["DataID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["DataID"]),
                ParentDataID = x["ParentDataID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ParentDataID"]),
                UICode = x["UICode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["UICode"]),
                Value = x["Value"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["Value"]),
                Description = x["Description"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["Description"])
            }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <param name="currentUserID"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        private static List<RequirementObjectTreeContract> AddNewRequirementObjectTree(Int32 requirementCategoryID, Int32 currentUserID)
        {
            try
            {

                String rcatObjectTypeCode = ObjectType.Compliance_Category.GetStringValue();
                Int32 catObjectTypeId = LookupManager.GetSharedDBLookUpData<lkpObjectType>()
                                                            .Where(cond => cond.OT_Code == rcatObjectTypeCode).FirstOrDefault().OT_ID;

                return BALUtils.GetSharedRequirementPackageRepoInstance().AddNewRequirementObjectTree(requirementCategoryID, catObjectTypeId, currentUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        #region [UAT-2213]

        public static RequirementCategoryContract GetRequirementMasterCategoryDetailByCategoryID(int ReqCatID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementMasterCategoryDetailByCategoryID(ReqCatID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsMasterCategoryNameExists(String newCategoryName)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().IsMasterCategoryNameExists(newCategoryName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementPackageContract> GetAllMasterRequirementPackages()
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetAllMasterRequirementPackages();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Int32 CreateCategoryCopy(CreateCategoryCopyContract createCategoryCopyContract)
        {
            try
            {
                Int32 addedCategoryID = BALUtils.GetSharedRequirementPackageRepoInstance().CreateCategoryCopy(createCategoryCopyContract);

                //UAT-2305
                if (addedCategoryID > AppConsts.NONE && createCategoryCopyContract.OldRequirementCategoryID > AppConsts.NONE)// Commeneted code For UAT-2985 -> && createCategoryCopyContract.UniversalCategoryID > AppConsts.NONE
                {
                    UniversalMappingDataManager.CopyUniversalDataByCategoryIds(addedCategoryID, createCategoryCopyContract.OldRequirementCategoryID, createCategoryCopyContract.CurrentLoggedInUserId);
                }

                return addedCategoryID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveMasterRotationCategory(RequirementCategoryContract requirementCategoryContract, Int32 currentLoggedInUserID)
        {
            try
            {
                Int32 addedCategoryID = BALUtils.GetSharedRequirementPackageRepoInstance().
                                                    SaveMasterRotationCategory(requirementCategoryContract, currentLoggedInUserID);
                if (addedCategoryID != requirementCategoryContract.RequirementCategoryID)
                {
                    AddNewRequirementObjectTree(addedCategoryID, currentLoggedInUserID);

                }

                //UAT-2305
                //if ((requirementCategoryContract.UniversalCategoryID > AppConsts.NONE || requirementCategoryContract.UniversalReqCatMappingID > AppConsts.NONE)
                //        && addedCategoryID > AppConsts.NONE)
                //{
                //    UniversalMappingDataManager.SaveUniversalRequirmentCategoryMappingData(ConvertToUniversalRequirementContract(requirementCategoryContract, addedCategoryID), currentLoggedInUserID);
                //}
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementCategoryContract> GetRequirementCategories()
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance()
                               .GetAllRequirmentCategories();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementPackageContract> GetCategoryPackageMapping(CategoryPackageMappingContract categoryPackageMappingContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetCategoryPackageMapping(categoryPackageMappingContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveCategoryPackageMapping(Int32 currentOrgUserID, Int32 requirementCategoryID, String requirementPackageIds)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().SaveCategoryPackageMapping(currentOrgUserID, requirementCategoryID, requirementPackageIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetMappedPackageIdsWithCategory(Int32 categoryID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetMappedPackageIdsWithCategory(categoryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetMappedCategoriesWithPackage(Int32 packageID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetMappedCategoriesWithPackage(packageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementCategoryContract> GetMasterRequirementCategories(RequirementCategoryContract requirementCategoryDetailsContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance()
                               .GetMasterRequirementCategories(requirementCategoryDetailsContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static String DeleteRequirementCategory(int caregoryId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance()
                                .DeleteRequirementCategory(caregoryId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT: 4279
        public static Boolean UpdatePackageCategoryMappingDisplayOrder(List<RequirementCategoryContract> CategoryId, Int32? destinationIndex, Int32 currentUserId, Int32 requirementPkgId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().UpdatePackageCategoryMappingDisplayOrder(CategoryId, destinationIndex, currentUserId, requirementPkgId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementCategoryContract> GetPackageCategoryMapping(PackageCategoryMappingContract packageCategoryMappingContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetPackageCategoryMapping(packageCategoryMappingContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SavePackageCategoryMapping(Int32 currentOrgUserID, Int32 requirementPackageID, String requirementCategoryIds, Boolean IsRotationPkgCopyFromAgencyHierarchy, int ExistingPkgId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().SavePackageCategoryMapping(currentOrgUserID, requirementPackageID, requirementCategoryIds, IsRotationPkgCopyFromAgencyHierarchy, ExistingPkgId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Int32 SaveMasterRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, Boolean IsRotationPkgCopyFromAgencyHierarchy)
        {
            try
            {
                //UAT-4657
                String requirementPkgVersioningStatus_DueCode = lkpRequirementPkgVersioningStatus.DUE.GetStringValue();
                Int32 requirementPkgVersioningStatus_DueId = LookupManager.GetSharedDBLookUpData<lkpRequirementPkgVersioningStatu>().
                                                             Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_DueCode && !cond.LRPVS_IsDeleted).Select(col => col.LRPVS_ID).FirstOrDefault();
                //Requirement Package as well as RequirementPackageAgency Mapping Entry
                //Parameter requirementPkgVersioningStatus_DueId added for UAT-4657
                Int32 addedPkgID = BALUtils.GetSharedRequirementPackageRepoInstance().SaveMasterRequirementPackage(requirementPackageContract, currentLoggedInUserID, IsRotationPkgCopyFromAgencyHierarchy, requirementPkgVersioningStatus_DueId);
                string requirmentCategoryIds = GetRequirmentCategoryIdsCSV(requirementPackageContract.LstRequirementCategory);


                #region UAT-3494
                if (IsRotationPkgCopyFromAgencyHierarchy)
                {
                    InsertRequirementPackageVersioningData(requirementPackageContract.existingPackageId, addedPkgID, currentLoggedInUserID, requirementPackageContract.ListAgencyIdsForCopyPkg);
                }
                #endregion

                SavePackageCategoryMapping(currentLoggedInUserID, addedPkgID, requirmentCategoryIds, IsRotationPkgCopyFromAgencyHierarchy, requirementPackageContract.existingPackageId);


                return addedPkgID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static String GetRequirmentCategoryIdsCSV(List<RequirementCategoryContract> lstRequirementCategories)
        {
            return String.Join(",", lstRequirementCategories.Select(x => x.RequirementCategoryID).Distinct().ToList());
        }

        public static Boolean ArchivePackage(Dictionary<Int32, Boolean> aryPackageIds, Int32 ArchivePackage)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().ArchivePackage(aryPackageIds, ArchivePackage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-4054
        public static Boolean UnArchivePackage(Dictionary<Int32, Boolean> aryPackageIds, Int32 ArchivePackage)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().UnArchivePackage(aryPackageIds, ArchivePackage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetMappedPackageDetails(Int32 reqCatID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetMappedPackageDetails(reqCatID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region
        /// <summary>
        /// UAT-4526 : Fetch package/agency/institution in seperate query
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        public static List<RequirementAgencyData> GetRequirementAgencyData(Int32 requirementPackageID)
        {
            try
            {
                List<RequirementAgencyData> requirmntPkgHrchicalContract = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                                                           .GetRequirementAgencyData(requirementPackageID);

                return requirmntPkgHrchicalContract;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        #region UAT-2514 Copy Package
        /// <summary>
        /// New Method for Getting Shared Package Details that is to be copied in Tenant
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <param name="userID"></param>
        /// <param name="isCopy"></param>
        /// <returns></returns>
        public static RequirementPackageContract GetRequirementPackageHierarchalDetailsByPackageIDNew(Int32 requirementPackageID, Guid userID, Boolean isCopy = false)
        {
            try
            {
                List<RequirementPackageHierarchicalDetailsContract> requirmntPkgHrchicalContract = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                                                           .GetRequirementPackageHierarchalDetailsByPackageIDNew(requirementPackageID);

                RequirementPackageContract requirementPackageDetailsContract = AssignPackageHierarchalDetailsToSessionContractNew(requirmntPkgHrchicalContract, userID, isCopy);

                #region UAT-4254

                requirementPackageDetailsContract.LstRequirementCategory.ForEach(x =>
                {
                    x.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
                    List<RequirementCategoryDocUrl> lstCatUrls = GetRequirementCatDocUrls(x.RequirementCategoryID);
                    x.lstReqCatDocUrls.AddRange(lstCatUrls);
                });

                #endregion

                return requirementPackageDetailsContract;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Convert PackageHierarchialDetailsContract to RequirementPackageContract
        /// </summary>
        /// <param name="LstRequirementPackageHierarchicalDetailsContract"></param>
        /// <param name="userID"></param>
        /// <param name="isCopy"></param>
        /// <returns></returns>
        private static RequirementPackageContract AssignPackageHierarchalDetailsToSessionContractNew(List<RequirementPackageHierarchicalDetailsContract> LstRequirementPackageHierarchicalDetailsContract, Guid userID, Boolean isCopy)
        {
            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();

            if (!LstRequirementPackageHierarchicalDetailsContract.IsNullOrEmpty())
            {
                List<Int32> lstAgenciesMappedWithAgencyUser = new List<int>();
                if (!isCopy)
                {
                    lstAgenciesMappedWithAgencyUser = ProfileSharingManager.GetAgencies(AppConsts.NONE, false, true, userID).Select(n => n.AG_ID).ToList();
                }
                //Add data into top level contract - RequirementPackageContract
                RequirementPackageHierarchicalDetailsContract packageHierarchicalDetailsContract = LstRequirementPackageHierarchicalDetailsContract.FirstOrDefault();
                requirementPackageContract.IsSharedUserLoggedIn = true;
                requirementPackageContract.RequirementPackageID = packageHierarchicalDetailsContract.RequirementPackageID;
                requirementPackageContract.RequirementPackageName = packageHierarchicalDetailsContract.RequirementPackageName;
                requirementPackageContract.IsSharedUserPackage = true;
                requirementPackageContract.RequirementPackageLabel = packageHierarchicalDetailsContract.RequirementPackageLabel;
                requirementPackageContract.DefinedRequirementID = packageHierarchicalDetailsContract.DefinedRequirementID;
                requirementPackageContract.ReqReviewByID = packageHierarchicalDetailsContract.ReqReviewByID;
                requirementPackageContract.EffectiveStartDate = packageHierarchicalDetailsContract.RequirementPackageEffectiveStartDate;
                requirementPackageContract.EffectiveEndDate = packageHierarchicalDetailsContract.RequirementPackageEffectiveEndDate;
                requirementPackageContract.IsNewPackage = packageHierarchicalDetailsContract.IsNewRequirementPackage;
                requirementPackageContract.IsArchivedPackage = packageHierarchicalDetailsContract.IsPackageArchived;
                //UAT-4657
                if (!packageHierarchicalDetailsContract.ParentPackageCode.IsNullOrEmpty())
                    requirementPackageContract.ParentPackageCode = packageHierarchicalDetailsContract.ParentPackageCode.Value;

                //UAT4121 Start here

                requirementPackageContract.RItemURLSampleDocURL = packageHierarchicalDetailsContract.RItemURLSampleDocURL;
                requirementPackageContract.RItemURLLabel = packageHierarchicalDetailsContract.RItemURLLabel;
                //END Here 

                //requirementPackageContract.RequirementPackageDescription = packageHierarchicalDetailsContract.RequirementPackageDescription;

                List<RequirementAgencyData> requirementAgencyData = GetRequirementAgencyData(packageHierarchicalDetailsContract.RequirementPackageID);

                requirementPackageContract.LstAgencyIDs = requirementAgencyData
                                                        .Where(cond => cond.RequirementPackageAgencyID > 0
                                                        && (isCopy || lstAgenciesMappedWithAgencyUser.Contains(cond.AgencyID)))
                                                        .Select(col => col.AgencyID).Distinct().ToList();
                if (!isCopy)
                {
                    List<Int32> allAgencyIDs = requirementAgencyData
                                                        .Where(cond => cond.RequirementPackageAgencyID > 0)
                                                        .Select(col => col.AgencyID).ToList();
                    requirementPackageContract.LstAgencyIDsWithNoAgencyUerPermission = allAgencyIDs.Except(lstAgenciesMappedWithAgencyUser).ToList();
                }
                requirementPackageContract.LstAgencyNames = requirementAgencyData
                                                        .Where(cond => cond.RequirementPackageAgencyID > 0
                                                    && (isCopy || lstAgenciesMappedWithAgencyUser.Contains(cond.AgencyID)))
                                                        .DistinctBy(col => col.AgencyID).Select(col => col.AgencyName).ToList();

                requirementPackageContract.LstSelectedTenantIDs = LstRequirementPackageHierarchicalDetailsContract.Where(cond => cond.RequirementPackageInstitutionID > 0)
                                        .Select(col => col.MappedTenantID).Distinct().ToList();
                requirementPackageContract.LstSelectedTenantNames = LstRequirementPackageHierarchicalDetailsContract.Where(cond => cond.RequirementPackageInstitutionID > 0)
                                                        .DistinctBy(col => col.MappedTenantID).Select(col => col.MappedTenantName).ToList();
                requirementPackageContract.RequirementPackageCode = packageHierarchicalDetailsContract.RequirementPackageCode;
                requirementPackageContract.PackageRuleTypeCode = packageHierarchicalDetailsContract.RequirementPackageRuleTypeCode;
                requirementPackageContract.PackageObjectTreeID = packageHierarchicalDetailsContract.PackageObjectTreeID;

                //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
                requirementPackageContract.RequirementPkgTypeID = packageHierarchicalDetailsContract.RequirementPkgTypeID;
                requirementPackageContract.RequirementPkgTypeCode = packageHierarchicalDetailsContract.RequirementPkgTypeCode;

                //Add data into Category contract - RequirementCategoryContract

                requirementPackageContract.LstRequirementCategory = new List<RequirementCategoryContract>();

                List<RequirementPackageHierarchicalDetailsContract> lstPackagesDistinctByCategory = LstRequirementPackageHierarchicalDetailsContract
                                                                                                    .Where(cond => cond.RequirementCategoryID != AppConsts.NONE)
                                                                                                    .DistinctBy(col => col.RequirementPackageCategoryID).ToList();

                foreach (RequirementPackageHierarchicalDetailsContract packageDetailDistinctByCategory in lstPackagesDistinctByCategory)
                {
                    RequirementCategoryContract categoryData = new RequirementCategoryContract();
                    categoryData.RequirementCategoryID = packageDetailDistinctByCategory.RequirementCategoryID;
                    categoryData.RequirementPackageCategoryID = packageDetailDistinctByCategory.RequirementPackageCategoryID;
                    categoryData.CategoryDisplayOrder = packageDetailDistinctByCategory.CategoryDisplayOrder;
                    categoryData.RequirementCategoryName = packageDetailDistinctByCategory.RequirementCategoryName;
                    categoryData.RequirementCategoryLabel = packageDetailDistinctByCategory.RequirementCategoryLabel;
                    categoryData.IsNewCategory = packageDetailDistinctByCategory.IsNewRequirementCategory;
                    categoryData.ComplianceReqStartDate = packageDetailDistinctByCategory.ComplianceReqStartDate;
                    categoryData.ComplianceReqEndDate = packageDetailDistinctByCategory.ComplianceReqEndDate;
                    //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes).
                    categoryData.RequirementDocumentLink = packageDetailDistinctByCategory.RequirementDocumentLink;
                    //UAT-3161
                    categoryData.RequirementDocumentLinkLabel = packageDetailDistinctByCategory.RequirementDocumentLinkLabel;
                    categoryData.RequirementCategoryCode = packageDetailDistinctByCategory.RequirementCategoryCode;
                    categoryData.CategoryRuleTypeCode = packageDetailDistinctByCategory.RequirementCategoryRuleTypeCode;
                    categoryData.CategoryObjectTreeID = packageDetailDistinctByCategory.CategoryObjectTreeID;
                    categoryData.ExplanatoryNotes = packageDetailDistinctByCategory.CategoryExplanatoryNotes;

                    //UAT-2603
                    categoryData.AllowDataMovement = packageDetailDistinctByCategory.ReqCatAllowDataMovement;

                    //UAT-3805
                    categoryData.SendItemDoconApproval = packageDetailDistinctByCategory.SendItemDocOnApproval;

                    #region UAT-4165
                    if (!packageDetailDistinctByCategory.RequirementCategoryPropIsEditableByAdmin.IsNullOrEmpty() && !packageDetailDistinctByCategory.RequirementCategoryPropIsEditableByApplicant.IsNullOrEmpty() && !packageDetailDistinctByCategory.RequirementCategoryPropIsEditableByClientAdmin.IsNullOrEmpty())
                    {
                        RequirementObjectPropertiesContract requirementObjectPropertiesData = new RequirementObjectPropertiesContract();
                        requirementObjectPropertiesData.RequirementObjPropCategoryID = packageDetailDistinctByCategory.RequirementObjPropCategoryID;
                        requirementObjectPropertiesData.RequirementObjPropCategoryItemID = null;
                        requirementObjectPropertiesData.RequirementObjPropIsCustomSettings = packageDetailDistinctByCategory.RequirementCategoryPropIsCustomSettings;
                        requirementObjectPropertiesData.RequirementObjPropIsEditableByAdmin = Convert.ToBoolean(packageDetailDistinctByCategory.RequirementCategoryPropIsEditableByAdmin);
                        requirementObjectPropertiesData.RequirementObjPropIsEditableByApplicant = Convert.ToBoolean(packageDetailDistinctByCategory.RequirementCategoryPropIsEditableByApplicant);
                        requirementObjectPropertiesData.RequirementObjPropIsEditableByClientAdmin = Convert.ToBoolean(packageDetailDistinctByCategory.RequirementCategoryPropIsEditableByClientAdmin);
                        categoryData.RequirementObjectProperties = requirementObjectPropertiesData;
                    }
                    #endregion

                    #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                    //Filling RequirementPackageContract for ComplianceRequired Setting.
                    categoryData.IsComplianceRequired = packageDetailDistinctByCategory.IsComplianceRequired;
                    categoryData.ComplianceReqStartDate = packageDetailDistinctByCategory.ComplianceReqStartDate.HasValue ? packageDetailDistinctByCategory.ComplianceReqStartDate : null;
                    categoryData.ComplianceReqEndDate = packageDetailDistinctByCategory.ComplianceReqEndDate.HasValue ? packageDetailDistinctByCategory.ComplianceReqEndDate : null;
                    #endregion

                    //Add data into Item contract - RequirementItemContract
                    List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByItem = LstRequirementPackageHierarchicalDetailsContract
                                                                .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                       && cond.RequirementItemID != AppConsts.NONE)
                                                                .DistinctBy(col => col.RequirementItemID).ToList();

                    categoryData.RuleUIExpression = packageDetailDistinctByCategory.RequirementCategoryUIExpression;
                    categoryData.RuleSqlExpression = packageDetailDistinctByCategory.RequirementCategorySQLExpression;

                    categoryData.LstRequirementItem = new List<RequirementItemContract>();

                    foreach (RequirementPackageHierarchicalDetailsContract packageDetailsDistinctByItem in LstPackagesDistinctByItem)
                    {
                        RequirementItemContract itemData = new RequirementItemContract();
                        itemData.RequirementCategoryItemID = packageDetailsDistinctByItem.RequirementCategoryItemID;
                        itemData.RequirementItemID = packageDetailsDistinctByItem.RequirementItemID;
                        itemData.RequirementItemName = packageDetailsDistinctByItem.RequirementItemName;
                        itemData.RequirementItemCode = packageDetailsDistinctByItem.RequirementItemCode;
                        itemData.ItemObjectTreeID = packageDetailsDistinctByItem.ItemObjectTreeID;
                        itemData.RequirementItemLabel = packageDetailsDistinctByItem.RequirementItemLabel;
                        itemData.ExplanatoryNotes = packageDetailsDistinctByItem.RequirementItemDescription; //UAT-2676
                        //UAT-2603
                        itemData.AllowDataMovement = packageDetailsDistinctByItem.ReqItmAllowDataMovement;
                        itemData.RequirementItemDisplayOrder = packageDetailsDistinctByItem.RequirementCategoryItemDisplayOrder; //UAT-3078
                        //UAT-3077
                        itemData.IsPaymentType = packageDetailsDistinctByItem.IsPaymentTypeItem;
                        itemData.Amount = packageDetailsDistinctByItem.ItemAmount;
                        //UAT 3792
                        itemData.AllowItemDataEntry = packageDetailsDistinctByItem.AllowItemDataEntry;
                        itemData.RequirementItemSampleDocumentFormURL = packageDetailsDistinctByItem.RequirementItemSampleDocumentFormURL; //UAT-3309

                        //itemData.listRequirementItemURLContract
                        List<RequirementItemURLContract> requirementitemurlContract = new List<RequirementItemURLContract>();
                        if (packageDetailsDistinctByItem.RItemURLSampleDocURL != string.Empty)
                        {
                            var ListRItemURLSampleDocURL = packageDetailsDistinctByItem.RItemURLSampleDocURL.Split('@');
                            var ListRItemURLSampleDocURLLabel = packageDetailsDistinctByItem.RItemURLLabel.Split('@');

                            for (int i = 0; i < ListRItemURLSampleDocURL.Length; i++)
                            {
                                string URLLablel = string.Empty;
                                if (ListRItemURLSampleDocURLLabel.Length >= i)
                                {
                                    URLLablel = ListRItemURLSampleDocURLLabel[i];
                                }

                                requirementitemurlContract.Add(new RequirementItemURLContract
                                {

                                    RItemURLLabel = URLLablel,
                                    RItemURLSampleDocURL = ListRItemURLSampleDocURL[i]
                                });
                            }
                        }
                        itemData.listRequirementItemURLContract = requirementitemurlContract;

                        #region UAT-4165
                        if (!packageDetailsDistinctByItem.RequirementItemPropIsCustomSettings.IsNullOrEmpty())
                        {
                            RequirementObjectPropertiesContract requirementItemPropertiesData = new RequirementObjectPropertiesContract();
                            requirementItemPropertiesData.RequirementObjPropCategoryID = packageDetailsDistinctByItem.RequirementItemPropCategoryID;
                            requirementItemPropertiesData.RequirementObjPropCategoryItemID = packageDetailsDistinctByItem.RequirementItemPropCategoryItemID;
                            requirementItemPropertiesData.RequirementObjPropIsCustomSettings = packageDetailsDistinctByItem.RequirementItemPropIsCustomSettings;
                            requirementItemPropertiesData.RequirementObjPropIsEditableByAdmin = Convert.ToBoolean(packageDetailsDistinctByItem.RequirementItemPropIsEditableByAdmin);
                            requirementItemPropertiesData.RequirementObjPropIsEditableByApplicant = Convert.ToBoolean(packageDetailsDistinctByItem.RequirementItemPropIsEditableByApplicant);
                            requirementItemPropertiesData.RequirementObjPropIsEditableByClientAdmin = Convert.ToBoolean(packageDetailsDistinctByItem.RequirementItemPropIsEditableByClientAdmin);
                            itemData.RequirementObjectProperties = requirementItemPropertiesData;
                        }

                        #endregion

                        itemData.RequirementItemExpiration = new RequirementItemExpirationContract();

                        List<RequirementPackageHierarchicalDetailsContract> lstCurrentItemRows = LstRequirementPackageHierarchicalDetailsContract
                                                                    .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                             && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                             && cond.RequirementItemID != AppConsts.NONE).ToList();

                        if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()))
                        {
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItem = lstCurrentItemRows.
                                                    Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
                                                    .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItem.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItem.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.ExpirationDate = currentItem.ExpirationValue;
                        }
                        else if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue()))
                        {
                            //UAT-2165
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItem = lstCurrentItemRows.
                                                    Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
                                                    .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItem.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItem.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.ExpirationDate = currentItem.ExpirationValue;
                            itemData.RequirementItemExpiration.ExpirationCondStartDate = currentItem.ExpirationCondStartDate;
                            itemData.RequirementItemExpiration.ExpirationCondEndDate = currentItem.ExpirationCondEndDate;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldID = currentItem.DateTypeRequirementFieldIDForExpiration;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldCode = currentItem.DateTypeRequirementFieldCodeForExpiration;
                        }
                        else if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()))
                        {
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItemWithDateAttribute = lstCurrentItemRows.
                                                                            Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                            && cond.DateTypeRequirementFieldIDForExpiration != 0 && cond.ExpirationValue == String.Empty)
                                                                            .FirstOrDefault();
                            RequirementPackageHierarchicalDetailsContract currentItemWithDateAttributeValue = lstCurrentItemRows.
                                                                            Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                            && cond.DateTypeRequirementFieldIDForExpiration == 0)
                                                                            .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItemWithDateAttributeValue.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItemWithDateAttributeValue.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldCode = currentItemWithDateAttribute.DateTypeRequirementFieldCodeForExpiration;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldID = currentItemWithDateAttribute.DateTypeRequirementFieldIDForExpiration;
                            itemData.RequirementItemExpiration.ExpirationValue = Convert.ToInt32(currentItemWithDateAttributeValue.ExpirationValue);
                        }
                        else
                        {
                            itemData.IsRequirementItemNeededExpiration = false;
                            itemData.RequirementItemExpiration = null;
                        }

                        itemData.RequirementItemRuleTypeCode = lstCurrentItemRows.
                                                            Where(cond => cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()
                                                                      && cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                      && cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue()
                                                                                ).FirstOrDefault().RequirementItemRuleTypeCode;

                        List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField = LstRequirementPackageHierarchicalDetailsContract
                                                                    .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                             && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                              && cond.RequirementFieldID != AppConsts.NONE)
                                                                    .DistinctBy(col => col.RequirementFieldID).ToList();
                        itemData.LstRequirementField = new List<RequirementFieldContract>();
                        //Add data into field contract - RequirementFieldContract
                        foreach (RequirementPackageHierarchicalDetailsContract packagedetailsDistinctByField in LstPackagesDistinctByField)
                        {
                            RequirementFieldContract fieldContract = new RequirementFieldContract();
                            fieldContract.RequirementFieldID = packagedetailsDistinctByField.RequirementFieldID;
                            fieldContract.RequirementItemFieldID = packagedetailsDistinctByField.RequirementItemFieldID;
                            fieldContract.RequirementFieldName = packagedetailsDistinctByField.RequirementFieldName;
                            fieldContract.RequirementFieldLabel = packagedetailsDistinctByField.RequirementFieldLabel;
                            fieldContract.AttributeTypeID = packagedetailsDistinctByField.RequirementFieldAttributeTypeID;
                            //fieldContract.RequirementFieldDescription = packagedetailsDistinctByField.RequirementFieldDescription;
                            fieldContract.RequirementFieldCode = packagedetailsDistinctByField.RequirementFieldCode;
                            fieldContract.FieldObjectTreeID = packagedetailsDistinctByField.FieldObjectTreeID;

                            fieldContract.RequirementFieldDisplayOrder = packagedetailsDistinctByField.RequirementItemFieldDisplayOrder; //UAT-3078

                            //UAT-2164
                            fieldContract.IsBackgroundDocument = packagedetailsDistinctByField.IsBackgroundDocument;
                            //UAT-2701
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue())
                            {
                                fieldContract.RequirementFieldMaxLength = packagedetailsDistinctByField.ReqFieldMaxLength;
                            }
                            //UAT-2366
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue())
                            {
                                fieldContract.UiRequirementItemID = packagedetailsDistinctByField.UiRequirementItemID;
                                fieldContract.UiRequirementFieldID = packagedetailsDistinctByField.UiRequirementFieldID;
                                fieldContract.RequirementFieldUIRuleTypeCode = packagedetailsDistinctByField.RequirementFieldFixedRuleTypeCode;
                                fieldContract.UiRuleErrorMessage = packagedetailsDistinctByField.UiRuleErrorMessage;
                            }

                            List<RequirementPackageHierarchicalDetailsContract> currentFieldRows = LstRequirementPackageHierarchicalDetailsContract
                                                .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                      && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                      && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID && cond.RequirementFieldID != AppConsts.NONE)
                                                         .ToList();

                            if (!Convert.ToBoolean(currentFieldRows.Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.REQUIRED.GetStringValue()).IsNullOrEmpty()))
                            {
                                fieldContract.IsFieldRequired = Convert.ToBoolean(currentFieldRows.Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.REQUIRED.GetStringValue())
                                                                            .FirstOrDefault().RequirementFieldRuleValue);
                            }
                            else
                            {
                                fieldContract.IsFieldRuleNotDefined = true;
                            }

                            //Add data into Field data contract - RequirementFieldDataContract

                            RequirementFieldDataContract fieldDataContract = new RequirementFieldDataContract();
                            fieldDataContract.RequirementFieldDataTypeCode = packagedetailsDistinctByField.RequirementFieldDataTypeCode;
                            fieldDataContract.RequirementFieldDataTypeID = packagedetailsDistinctByField.RequirementFieldDataTypeID;
                            fieldDataContract.RequirementFieldDataTypeName = packagedetailsDistinctByField.RequirementFieldDataTypeName;

                            //Save data into video contract - RequirementFieldVideoData
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                            {
                                RequirementFieldVideoData videoData = new RequirementFieldVideoData();
                                videoData.RequirementFieldVideoID = packagedetailsDistinctByField.RequirementFieldVideoID;
                                RequirementPackageHierarchicalDetailsContract videoFieldRules_OpenRequired = currentFieldRows
                                    .Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.REQUIRED_TO_OPEN.GetStringValue()).FirstOrDefault();
                                if (!videoFieldRules_OpenRequired.IsNullOrEmpty())
                                {
                                    videoData.IsVideoRequiredToBeOpened = Convert.ToBoolean(videoFieldRules_OpenRequired.RequirementFieldRuleValue);
                                }
                                if (videoData.IsVideoRequiredToBeOpened)
                                {
                                    videoData.VideoOpenTimeDuration = Convert.ToInt32(currentFieldRows
                                        .Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue())
                                        .FirstOrDefault().RequirementFieldRuleValue);
                                    videoData.VideoOpenedMinutes = videoData.VideoOpenTimeDuration / 60;
                                    videoData.VideoOpenedSeconds = videoData.VideoOpenTimeDuration % 60;
                                }

                                videoData.VideoURL = packagedetailsDistinctByField.FieldVideoURL;
                                videoData.VideoName = packagedetailsDistinctByField.RequirementFieldVideoName;
                                fieldDataContract.VideoFieldData = videoData;
                            }

                            //Save data into options contract - RequirementFieldOptionsData
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                            {
                                fieldDataContract.LstRequirementFieldOptions = new List<RequirementFieldOptionsData>();
                                List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField_OptionData = LstRequirementPackageHierarchicalDetailsContract
                                                                   .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                            && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                            && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID
                                                                            ).DistinctBy(col => col.RequirementFieldOptionsID).ToList();
                                StringBuilder formatOptions = new StringBuilder();

                                foreach (RequirementPackageHierarchicalDetailsContract packageFieldOptionData in LstPackagesDistinctByField_OptionData)
                                {
                                    RequirementFieldOptionsData optionData = new RequirementFieldOptionsData();
                                    optionData.RequirementFieldOptionsID = packageFieldOptionData.RequirementFieldOptionsID;
                                    optionData.OptionText = packageFieldOptionData.OptionText;
                                    optionData.OptionValue = packageFieldOptionData.OptionValue;
                                    fieldDataContract.LstRequirementFieldOptions.Add(optionData);

                                    //code to generate FormatOptions to be displayed during edit mode
                                    formatOptions.AppendFormat("{0}={1},", packageFieldOptionData.OptionText, packageFieldOptionData.OptionValue);
                                }

                                //code to generate FormatOptions to be displayed during edit mode
                                Int32 index = formatOptions.ToString().LastIndexOf(',');
                                if (index >= 0)
                                    formatOptions.Remove(index, 1);

                                fieldDataContract.RequiredFieldOptionsFormattedValue = Convert.ToString(formatOptions);
                            }

                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                            {
                                RequirementFieldViewDocumentData documentData = new RequirementFieldViewDocumentData();
                                documentData.ClientSystemDocumentID = packagedetailsDistinctByField.ClientSystemDocumentID;
                                documentData.DocumentFileName = packagedetailsDistinctByField.ClientSystemDocumentFileName;
                                documentData.DocumentPath = packagedetailsDistinctByField.ClientSystemDocumentPath;
                                documentData.DocumentSize = packagedetailsDistinctByField.ClientSystemDocumentSize;

                                List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField_DocFieldData = LstRequirementPackageHierarchicalDetailsContract
                                                                   .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                            && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                            && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID
                                                                            ).DistinctBy(col => col.DocumentAcroFieldTypeID).ToList();
                                documentData.LstDocumentAcroFieldTypeCodes = new List<String>();
                                documentData.LstRequirementDocumentAcroFieldIDs = new List<Int32>();

                                foreach (RequirementPackageHierarchicalDetailsContract docFieldData in LstPackagesDistinctByField_DocFieldData)
                                {
                                    if (!docFieldData.DocumentAcroFieldTypeCode.IsNullOrEmpty())
                                    {
                                        documentData.LstDocumentAcroFieldTypeCodes.Add(docFieldData.DocumentAcroFieldTypeCode);
                                        documentData.LstRequirementDocumentAcroFieldIDs.Add(docFieldData.RequirementDocumentAcroFieldID);
                                    }
                                }
                                fieldDataContract.FieldViewDocumentData = documentData;
                            }

                            fieldContract.RequirementFieldData = fieldDataContract;
                            //UAT  4380
                            if (!packagedetailsDistinctByField.RequirementItemFieldPropIsCustomSettings.IsNullOrEmpty())
                            {
                                RequirementObjectPropertiesContract requirementItemPropertiesData = new RequirementObjectPropertiesContract();
                                requirementItemPropertiesData.RequirementObjPropCategoryID = packageDetailsDistinctByItem.RequirementItemPropCategoryID;
                                requirementItemPropertiesData.RequirementObjPropCategoryItemID = packageDetailsDistinctByItem.RequirementItemPropCategoryItemID;
                                requirementItemPropertiesData.RequirementObjPropItemFieldID = packagedetailsDistinctByField.RequirementItemFieldID;
                                requirementItemPropertiesData.RequirementObjPropIsCustomSettings = packagedetailsDistinctByField.RequirementItemFieldPropIsCustomSettings;
                                requirementItemPropertiesData.RequirementObjPropIsEditableByAdmin = Convert.ToBoolean(packagedetailsDistinctByField.RequirementItemFieldPropIsEditableByAdmin);
                                requirementItemPropertiesData.RequirementObjPropIsEditableByApplicant = Convert.ToBoolean(packagedetailsDistinctByField.RequirementItemFieldPropIsEditableByApplicant);
                                requirementItemPropertiesData.RequirementObjPropIsEditableByClientAdmin = Convert.ToBoolean(packagedetailsDistinctByField.RequirementItemFieldPropIsEditableByClientAdmin);
                                fieldContract.RequirementObjectProperties = requirementItemPropertiesData;
                            }

                            itemData.LstRequirementField.Add(fieldContract);
                        }
                        categoryData.LstRequirementItem.Add(itemData);
                    }

                    requirementPackageContract.LstRequirementCategory.Add(categoryData);
                }
            }

            return requirementPackageContract;
        }

        #endregion
        #region UAT-2514:
        public static String GetRequirementPackageObjectIds(Int32 tenantId, Int32 ChunkSize, Int32 retryTimeLag)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackageObjectIdsToSync(tenantId, ChunkSize, retryTimeLag);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveRequirementPackageObjectForSync(String requestDataXML, Int32 currentLoggedInUserId)
        {
            try
            {
                Int32 syncObjectId = 0;
                return BALUtils.GetSharedRequirementPackageRepoInstance().SaveRequirementPackageObjectForSync(requestDataXML, currentLoggedInUserId, out syncObjectId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Field Rule

        public static Int32 GetRequirementObjectTreeIDByprntID(String HID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeIDByprntID(HID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveUpdateNewRequirementFieldRule(List<RequirementRuleContract> lstRequirementRule, Int32 CurrentLoggedInUserId, Int32 requirementObjectTreeId)
        {
            try
            {
                List<lkpObjectAttribute> lstlkpObjectAttribue = LookupManager.GetSharedDBLookUpData<lkpObjectAttribute>().ToList();


                String requiredObjectAttributeCode = ObjectAttribute.REQUIRED.GetStringValue();
                String signatureRequiredObjectAttributeCode = ObjectAttribute.SIGNATURE_REQUIRED.GetStringValue();
                String requiredToViewObjectAttributeCode = ObjectAttribute.REQUIRED_TO_VIEW.GetStringValue();
                String requiredToOpeAttributeCode = ObjectAttribute.REQUIRED_TO_OPEN.GetStringValue();
                String boxStayOpenObjectAttributeCode = ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue();

                lkpObjectAttribute requiredObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredObjectAttributeCode);
                lkpObjectAttribute signatureRequiredObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == signatureRequiredObjectAttributeCode);
                lkpObjectAttribute requiredToViewObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredToViewObjectAttributeCode);
                lkpObjectAttribute requiredToOpenObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredToOpeAttributeCode);
                lkpObjectAttribute boxStayOpenObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == boxStayOpenObjectAttributeCode);


                RequirementRuleContract requiredRule = lstRequirementRule.FirstOrDefault(cond => cond.RequirementFieldRuleTypeCode == requiredObjectAttributeCode);

                RequirementObjectTree attrRequirmntObjTreeFromDb = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTree(requirementObjectTreeId);

                if (requiredRule.RequirementFieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue()
                   || requiredRule.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue()
                   || requiredRule.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()
                   || requiredRule.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue()
                   || requiredRule.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue()
                     || requiredRule.RequirementFieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue()
                      || requiredRule.RequirementFieldDataTypeCode == RequirementFieldDataType.SIGNATURE.GetStringValue()
                    )
                {
                    InsertUpdateRequiredFieldProperty(CurrentLoggedInUserId, requiredObjectAttribute, requiredRule, attrRequirmntObjTreeFromDb);
                }

                if (requiredRule.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                {
                    RequirementRuleContract requiredToOpeAttributeRule = lstRequirementRule.FirstOrDefault(cond => cond.RequirementFieldRuleTypeCode == requiredToOpeAttributeCode);
                    if (!requiredToOpeAttributeRule.IsNullOrEmpty())
                    {
                        InsertUpdateRequiredToOpenFieldProperty(CurrentLoggedInUserId, requiredToOpenObjectAttribute, requiredToOpeAttributeRule, attrRequirmntObjTreeFromDb);

                        RequirementRuleContract boxStayOpenObjectAttributeRule = lstRequirementRule.FirstOrDefault(cond => cond.RequirementFieldRuleTypeCode == boxStayOpenObjectAttributeCode);

                        if (!boxStayOpenObjectAttributeRule.IsNullOrEmpty())// && VideoFieldData.VideoOpenTimeDuration == AppConsts.NONE)
                        {
                            InsertUpdateBoxStayOpenFieldProperty(CurrentLoggedInUserId, boxStayOpenObjectAttribute, boxStayOpenObjectAttributeRule, attrRequirmntObjTreeFromDb);
                        }
                    }
                }

                return BALUtils.GetSharedRequirementPackageRepoInstance().saveRequirmentFieldRuleDetails();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<RequirementRuleContract> GetReqFixedRuleDetailByObjectTreeID(Int32 requirementObjectTreeID)
        {
            try
            {
                List<RequirementObjectTreeProperty> lstReqObjectTree = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreePropertyByID(requirementObjectTreeID);

                return lstReqObjectTree.Select(x => new RequirementRuleContract
                {
                    ObjectTreeID = x.ROTP_ObjectTreeID,
                    ObjectTypeCode = ObjectType.Compliance_ATR.GetStringValue(),
                    RequirementFieldRuleTypeCode = x.lkpObjectAttribute.OA_Code,
                    RequirementFieldRuleTypeValue = x.ROTP_ObjectAttributeValue
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<Int32> GetRequirementObjectTreeIDByReqFieldID(Int32 reqFieldID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementObjectTreeIDByReqFieldID(reqFieldID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Check Rot. Eff. Start Date & EndDate
        public static Tuple<List<Int32>, List<String>> CheckRotEffectiveDate(Int32 requirementPkgId, String rotationIds, Int32 tenantID)
        {
            try
            {
                var pkgDetails = BALUtils.GetSharedRequirementPackageRepoInstance().CheckRotEffectiveDate(requirementPkgId);
                var rotationList = BALUtils.GetClinicalRotationRepoInstance(tenantID).GetClinicalRotationByIds(rotationIds);
                List<Int32> invalidRotationIds = new List<Int32>();
                List<String> invalidRotationNames = new List<String>();
                foreach (var item in rotationList)
                {
                    if ((pkgDetails.EffectiveEndDate > item.StartDate || pkgDetails.EffectiveEndDate.IsNull())
                        && (pkgDetails.EffectiveStartDate.IsNull() || pkgDetails.EffectiveStartDate < item.EndDate))
                    {
                        //Valid Rotation & Package
                        List<RequirementPackageContract> lstRequirementPackageContracts = BALUtils.GetSharedRequirementPackageRepoInstance().GetAllPkgVersionsByPkgId(requirementPkgId);
                        if (lstRequirementPackageContracts.Any() && lstRequirementPackageContracts.Count > AppConsts.ONE)
                        {
                            lstRequirementPackageContracts = lstRequirementPackageContracts.Where(con => con.EffectiveStartDate < item.StartDate).ToList();
                            if (lstRequirementPackageContracts.IsNullOrEmpty() || (pkgDetails.EffectiveStartDate != lstRequirementPackageContracts[0].EffectiveStartDate))
                            {
                                //Invalid Rotation & Package
                                invalidRotationIds.Add(item.RotationID);
                                invalidRotationNames.Add(item.RotationName);
                            }
                        }
                    }
                    else
                    {
                        //Invalid Rotation & Package
                        invalidRotationIds.Add(item.RotationID);
                        invalidRotationNames.Add(item.RotationName);
                    }
                }

                return new Tuple<List<Int32>, List<String>>(invalidRotationIds, invalidRotationNames);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ClinicalRotationDetailContract GetRotationDetail(Int32 tenantID, Int32 rotationID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetClinicalRotationById(rotationID, null);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        //UAT-2533
        public static List<RequirementPackageContract> GetRequirementPackageDetail(String RequiremntRotPackageIDs)
        {
            try
            {
                List<RequirementPackageContract> lstReqPkgContact = BALUtils.GetSharedRequirementPackageRepoInstance()
                                                                                            .GetRequirementPackageDetail(RequiremntRotPackageIDs);

                lstReqPkgContact.ForEach(x =>
                {
                    List<Int32> agencyHIerarchyIds = BALUtils.GetSharedRequirementPackageRepoInstance().GetAgencyHierarchyIdsWithPkgId(x.RequirementPackageID);
                    x.SelectedAgencyHierarchyDeatils = new Dictionary<Int32, String>();
                    foreach (Int32 agencyHierachyId in agencyHIerarchyIds.Distinct())
                    {
                        x.SelectedAgencyHierarchyDeatils.Add(agencyHierachyId, String.Empty);
                    }
                });

                return lstReqPkgContact;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean BulkPackageCopy(String BulkCopyPackgXML, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().BulkPackageCopy(BulkCopyPackgXML, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ScheduleAutoArchivalRequirementPackage(Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().ScheduleAutoArchivalRequirementPackage(currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetAgencyHierarchyIdsByRequirementPackageID(Int32 requirementPackageID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetAgencyHierarchyIdsByRequirementPackageID(requirementPackageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-2648// AgencyHierarchyPackage Mapping

        private static void CreateAgencyHierarchyPkg(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage requirementPackage)
        {
            foreach (Int32 agencyHierarchyID in requirementPackageContract.LstAgencyHierarchyIDs)
            {
                AgencyHierarchyPackage agencyHierarchyPackage = new AgencyHierarchyPackage()
                {
                    AHP_AgencyHierarchyID = agencyHierarchyID,
                    AHP_IsDeleted = false,
                    AHP_CreatedBy = currentLoggedInUserID,
                    AHP_CreatedOn = DateTime.Now,
                };
                requirementPackage.AgencyHierarchyPackages.Add(agencyHierarchyPackage);
            }
        }

        public static List<RequirementFieldType> GetAttributeType()
        {
            try
            {
                List<Entity.SharedDataEntity.lkpComplianceAttributeType> attrTypeList = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpComplianceAttributeType>();

                return attrTypeList.Select(col => new RequirementFieldType
                {
                    Code = col.Code,
                    ComplianceAttributeTypeID = col.ComplianceAttributeTypeID,
                    Name = col.Name
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #region UAT-3078
        public static Boolean updateRequirementItemDisplayOrder(Int32 RequirementItemId, Int32 RequirementCategoryId, Int32 DisplayOrder, Int32 CurrentLoggedInUserID, Boolean isNewPackage)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().updateRequirementItemDisplayOrder(RequirementItemId, RequirementCategoryId, DisplayOrder, CurrentLoggedInUserID, isNewPackage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean updateRequirementFieldDisplayOrder(Int32 RequirementFieldId, Int32 RequirementItemId, Int32 DisplayOrder, Int32 CurrentLoggedInUserID, Boolean isNewPackage)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().updateRequirementFieldDisplayOrder(RequirementFieldId, RequirementItemId, DisplayOrder, CurrentLoggedInUserID, isNewPackage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion


        #region UAT-3176
        public static Boolean SaveUpdateRotationAttributeGroup(RequirementAttributeGroupContract rotationAttributeGroupContract, Boolean IsAttributeGroupExists, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().SaveUpdateRotationAttributeGroup(rotationAttributeGroupContract, IsAttributeGroupExists, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<RequirementAttributeGroupContract> GetAllRotationAttributeGroup(String attributeName, String attributeLabel)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetAllRotationAttributeGroup(attributeName, attributeLabel);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static RequirementAttributeGroupContract GetAttributeGroupById(Int32 rotationAttributeGroupId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetAttributeGroupById(rotationAttributeGroupId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean IsAttributeGroupMapped(Int32 rotationAttributeGroupId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().IsAttributeGroupMapped(rotationAttributeGroupId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }


        public static List<RequirementAttributeGroups> GetRequirementAttributeGroups()
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementAttributeGroups();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        #region UAT-3230
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="chunkSize"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public static String GetPendingRequirementPackageObjectIdsForTenant(Int32 tenantId, Int32 chunkSize, Int32 retryCount)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetPendingRequirementPackageObjectIdsForTenant(tenantId, chunkSize, retryCount);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserId"></param>
        public static void UpdateSyncRequirementPackageObjectsCount(Int32 currentUserId, String SyncReqPkgObjectIds)
        {
            try
            {
                BALUtils.GetSharedRequirementPackageRepoInstance().UpdateSyncRequirementPackageObjectsCount(currentUserId, SyncReqPkgObjectIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserId"></param>
        public static void RemoveSyncRequirementPackageObjects(Int32 currentUserId, String SyncReqPkgObjectIds)
        {
            try
            {
                BALUtils.GetSharedRequirementPackageRepoInstance().RemoveSyncRequirementPackageObjects(currentUserId, SyncReqPkgObjectIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        public static List<RequirementReviewByContract> GetRequirementReviewBy()
        {
            try
            {
                List<lkpReqReviewBy> dataTypeList = LookupManager.GetSharedDBLookUpData<lkpReqReviewBy>().Where(x => !x.RRB_IsDeleted).ToList();
                return dataTypeList.Select(con => new RequirementReviewByContract
                {
                    ID = con.RRB_ID,
                    Code = con.RRB_Code,
                    Name = con.RRB_Name,
                    Description = con.RRB_Description
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean CloneRequirementItem(Int32 sourceReqItemID, Int32 currentLoggedInUserId, Int32 reqCatID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().CloneRequirementItem(sourceReqItemID, currentLoggedInUserId, reqCatID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-3295
        public static ProfileSharingInvitationDetailsContract GetProfileShareDetailsById(Int32 invitationId)
        {
            try
            {
                //return BALUtils.GetSharedRequirementPackageRepoInstance().GetAllRotationAttributeGroup(attributeName, attributeLabel);
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetProfileShareDetailsById(invitationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean UpdateProfileShareInvDetails(ProfileSharingInvitationDetailsContract clinicalRotationContract, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().UpdateProfileShareInvDetails(clinicalRotationContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        #region UAT-3494

        public static Boolean InsertRequirementPackageVersioningData(Int32 OldPackageID, Int32 NewPackageID, Int32 currentOrgUserID, String lstSelectedAgencyIds)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().InsertRequirementPackageVersioningData(currentOrgUserID, OldPackageID, NewPackageID, lstSelectedAgencyIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-4254

        public static List<RequirementCategoryDocUrl> GetRequirementCatDocUrls(Int32 reqCatId)
        {
            try
            {
                List<RequirementCategoryDocUrl> lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
                List<RequirementCategoryDocLink> lstReqCategoryLinks = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementCatDocUrls(reqCatId);

                if (!lstReqCategoryLinks.IsNullOrEmpty() && lstReqCategoryLinks.Count > AppConsts.NONE)
                {
                    lstReqCategoryLinks.ForEach(item =>
                    {
                        RequirementCategoryDocUrl requirementCategoryDocUrl = new RequirementCategoryDocUrl();
                        //Map properties on contract class here//
                        requirementCategoryDocUrl.RequirementCatDocLinkID = item.RCDL_ID;
                        requirementCategoryDocUrl.RequirementCatID = item.RCDL_RequirementCategoryID;
                        requirementCategoryDocUrl.RequirementCatDocUrlLabel = item.RCDL_SampleDocFormUrlLabel;
                        requirementCategoryDocUrl.RequirementCatDocUrl = item.RCDL_SampleDocFormURL;

                        lstReqCatDocUrls.Add(requirementCategoryDocUrl);
                    });
                }
                return lstReqCatDocUrls;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion
        #region UAT-4657

        public static Dictionary<Int32, String> GetPackagesAssociatedWithCategory(Int32 categoryId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetPackagesAssociatedWithCategory(categoryId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveCategoryDiassociationDetail(Int32 categoryId, String packageIds, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().SaveCategoryDiassociationDetail(categoryId, packageIds, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //public static List<Int32> GetPendingTenantsWhichPkgVersioningOrCategoryDisassociationPending()
        //{
        //    try
        //    {
        //        List<lkpRequirementPkgVersioningStatu> lstlkpRequirementPkgVersioningStatu = LookupManager.GetSharedDBLookUpData<lkpRequirementPkgVersioningStatu>()
        //                                                                                                   .Where(cond => !cond.LRPVS_IsDeleted).ToList();

        //        if (!lstlkpRequirementPkgVersioningStatu.IsNullOrEmpty() && lstlkpRequirementPkgVersioningStatu.Any())
        //        {
        //            String requirementPkgVersioningStatus_NoRotationsCode = lkpRequirementPkgVersioningStatus.NO_ROTATIONS_FOUND_FOR_THE_PACKAGE_MOVEMENT.GetStringValue();
        //            Int32 requirementPkgVersioningStatus_NoRotationId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_NoRotationsCode)
        //                                                                                        .Select(col => col.LRPVS_ID).FirstOrDefault();

        //            String requirementPkgVersioningStatus_CompletedCode = lkpRequirementPkgVersioningStatus.COMPLETED.GetStringValue();
        //            Int32 requirementPkgVersioningStatus_CompletedId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_CompletedCode)
        //                                                                                    .Select(col => col.LRPVS_ID).FirstOrDefault();

        //            return BALUtils.GetSharedRequirementPackageRepoInstance().GetPendingTenantsWhichPkgVersioningOrCategoryDisassociationPending(requirementPkgVersioningStatus_NoRotationId, requirementPkgVersioningStatus_CompletedId);

        //        }
        //        return new List<Int32>();
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static String IsCategoryDisassociationInProgress(Int32 requirementCategoryId, List<Int32> selectedPkgIds)
        {
            try
            {
                List<lkpRequirementPkgVersioningStatu> lstlkpRequirementPkgVersioningStatu = LookupManager.GetSharedDBLookUpData<lkpRequirementPkgVersioningStatu>()
                                                                                                             .Where(cond => !cond.LRPVS_IsDeleted).ToList();

                String requirementPkgVersioningStatus_DueCode = lkpRequirementPkgVersioningStatus.DUE.GetStringValue();
                Int32 requirementPkgVersioningStatus_DueId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_DueCode)
                                                                                            .Select(col => col.LRPVS_ID).FirstOrDefault();

                String requirementPkgVersioningStatus_InProgressCode = lkpRequirementPkgVersioningStatus.IN_PROGRESS.GetStringValue();

                Int32 requirementPkgVersioningStatus_InProgressId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_InProgressCode)
                                                                                            .Select(col => col.LRPVS_ID).FirstOrDefault();

                return BALUtils.GetSharedRequirementPackageRepoInstance().IsCategoryDisassociationInProgress(requirementCategoryId, selectedPkgIds, requirementPkgVersioningStatus_DueId, requirementPkgVersioningStatus_InProgressId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Boolean IsSyncAlreadyInProgress(Int32 objectId, Boolean IsObjectTypePackage)
        {
            try
            {
                List<lkpObjectType> lkpObjectTypes = LookupManager.GetSharedDBLookUpData<lkpObjectType>().ToList();
                return BALUtils.GetSharedRequirementPackageRepoInstance().IsSyncAlreadyInProgress(objectId, IsObjectTypePackage, lkpObjectTypes);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion
    }
}