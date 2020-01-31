using Entity.ClientEntity;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using INTSOF.UI.Contract.Templates;
using INTSOF.UI.Contract.SystemSetUp;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.ProfileSharing;

namespace Business.RepoManagers
{
    public static class ComplianceSetupManager
    {
        #region Admin Setup Screens

        #region Manage Compliance Packages

        public static List<CompliancePackage> GetCompliancePackage(Int32 tenantId, Boolean getTenantName)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                List<CompliancePackage> compliancePackage = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCompliancePackages(getTenantName, tenantList);
                return compliancePackage;
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

        public static List<NodesContract> GetListofNodes(Int32 CategoryId, Int32? ItemId, Int32 TenantId)
        {
            try
            {
                List<NodesContract> listofNodes = BALUtils.GetComplianceSetupRepoInstance(TenantId).GetListofNodes(CategoryId, ItemId);
                return listofNodes;
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

        public static void SaveCompliancePackageDetail(CompliancePackage package, Int32 currentLoggedInUserId, Dictionary<String, String> notesDetail = null)
        {
            try
            {
                //CompliancePackage client = null;

                //if (package.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    client = package.Clone();
                //    var compliancePackage = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetExistingPackage(package.PackageName);

                //    if (compliancePackage.IsNotNull())
                //    {
                //        package.PackageName = AppendSuffixToExistingName(compliancePackage);
                //    }
                //}

                //CompliancePackage resultPackage = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).SaveCompliancePackageDetail(package, currentLoggedInUserId);
                //if (notesDetail.IsNotNull())
                //{
                //    SaveLargeContent(currentLoggedInUserId, SecurityManager.DefaultTenantID, notesDetail, resultPackage.CompliancePackageID, LCObjectType.CompliancePackage.GetStringValue());
                //}

                //CreatePackageAssignmentHierarchy(resultPackage, currentLoggedInUserId, SecurityManager.DefaultTenantID);
                //if (resultPackage.IsNotNull() && package.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    client.CopiedFromCode = resultPackage.Code;

                CompliancePackage compliancePackage = BALUtils.GetComplianceSetupRepoInstance(package.TenantID.Value).SaveCompliancePackageDetail(package, currentLoggedInUserId);

                if (notesDetail.IsNotNull())
                {
                    SaveLargeContent(currentLoggedInUserId, compliancePackage.TenantID.Value, notesDetail, compliancePackage.CompliancePackageID, LCObjectType.CompliancePackage.GetStringValue());
                }
                CreatePackageAssignmentHierarchy(compliancePackage, currentLoggedInUserId, compliancePackage.TenantID.Value);
                //}
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

        private static void CreatePackageAssignmentHierarchy(CompliancePackage package, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            //List<lkpObjectType> lkpObjectType = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetlkpObjectType();
            List<lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
            Int32 objectTypeIdForPackage = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.CompliancePackage.GetStringValue()).OT_ID;
            BALUtils.GetComplianceSetupRepoInstance(tenantId).AddAssociationHierarchyNode(null, null, objectTypeIdForPackage, package.CompliancePackageID, currentLoggedInUserId, true);
        }

        public static void UpdateCompliancePackageDetail(CompliancePackage package, Int32 currentLoggedInUserId, Dictionary<String, String> notesDetail = null)
        {
            try
            {
                //commented for UAT 198
                //CompliancePackage compliancePackage = new CompliancePackage();
                //if (package.TenantID != SecurityManager.DefaultTenantID)// if package being updated is in client database
                //{//this cloning is kept separate for performance
                //    compliancePackage = package.Clone();
                //}

                //Update package in respective database (admin/ client)
                BALUtils.GetComplianceSetupRepoInstance(package.TenantID.Value).UpdateCompliancePackageDetail(package, currentLoggedInUserId);
                if (notesDetail.IsNotNull())
                {
                    SaveLargeContent(currentLoggedInUserId, package.TenantID.Value, notesDetail, package.CompliancePackageID, LCObjectType.CompliancePackage.GetStringValue());
                }

                //commented for UAT 198
                // if package being updated is in client database, then create a version in admin database
                //if (package.TenantID != SecurityManager.DefaultTenantID)
                //{

                //    ManageNameDuplicacyPackageUpdate(package, compliancePackage);
                //    compliancePackage.CopiedFromCode = package.Code;
                //    //create a version of the package in admin database.
                //    compliancePackage = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).SaveCompliancePackageDetail(compliancePackage, currentLoggedInUserId);

                //    //create assginmenthierarchy for newly created package in admin database
                //    CreatePackageAssignmentHierarchy(compliancePackage, currentLoggedInUserId, SecurityManager.DefaultTenantID);
                //    //package.CopiedFromCode = compliancePackage.Code;

                //    if (notesDetail.IsNotNull())
                //    {
                //        SaveLargeContent(currentLoggedInUserId, SecurityManager.DefaultTenantID, notesDetail, compliancePackage.CompliancePackageID, LCObjectType.CompliancePackage.GetStringValue());
                //    }
                //}
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


        public static lkpPackageComplianceStatu GetComplianceStatusByCode(String code, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPackageComplianceStatu>(tenantId).FirstOrDefault(x => x.Code.Equals(code));
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

        public static Boolean DeleteCompliancePackage(Int32 packageID, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteCompliancePackage(packageID, currentUserId);

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

        public static Boolean CheckIfPackageNameAlreadyExist(String packageName, Int32 compliancePackageID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).CheckIfPackageNameAlreadyExist(packageName, compliancePackageID, tenantId);

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

        public static Boolean CheckIfPackageNameAlreadyExist(String packageName, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).CheckIfPackageNameAlreadyExist(packageName);

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
        /// Appends string in the name if the name already exists.
        /// </summary>
        /// <param name="compliancePackage"></param>
        /// <returns></returns>
        private static String AppendSuffixToExistingName(CompliancePackage compliancePackage)
        {
            String delimiter = "-DUP";

            if (compliancePackage.PackageName.Contains(delimiter))
            {
                String[] suffixToAppend = compliancePackage.PackageName.Split(new string[] { delimiter }, StringSplitOptions.None);
                Int32 indexToAppend = Convert.ToInt32(suffixToAppend.Last()) + 1;
                return suffixToAppend.First() + delimiter + indexToAppend;
            }
            else
            {
                return compliancePackage.PackageName + delimiter + "1";
            }
        }

        public static CompliancePackage GetCopiedCompliancePackage(Int32 tenantId, Int32 parentPackageId, String packageName)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCopiedCompliancePackage(parentPackageId, packageName);
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
        /// UAT-2219-Ability to put package detail notes either above or below the package name in order flow
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="selectedPositionCode"></param>
        /// <returns></returns>
        public static lkpPackageNotesPosition GetPackageNotesPosition(Int32 tenantId, String selectedPositionCode)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpPackageNotesPosition>(tenantId).Where(x => x.PNP_Code == selectedPositionCode).FirstOrDefault();
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

        #region Manage Compliance Categories

        /// <summary>
        /// Returns all the compliance categories viewable to the current logged in user.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of Compliance Categories</returns>
        public static List<ComplianceCategory> GetComplianceCategories(Int32 tenantId, Boolean getTenantName)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceCategories(getTenantName, tenantList);
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
        /// UAT-4056 Category Search filtering updates
        /// Filter Categories on selection of Hierarchy Institution
        /// Filter Categories on basis of Client Admin Permission
        /// </summary>
        /// <param name="SelectedPackagesId"></param>
        /// <param name="dpm_Ids"></param>
        /// <param name="OrganisationUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ComplianceCategory> GetComplianceCategoriesByPermission(List<Int32> SelectedPackagesId, String dpm_Ids, Int32? OrganisationUserId, Int32 tenantId, Boolean IsAdminLoggedIn)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceCategoriesByPermissionList(tenantList, tenantId, SelectedPackagesId, dpm_Ids, OrganisationUserId, IsAdminLoggedIn);
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
        /// Returns all the compliance categories viewable to the current logged in user.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of Compliance Categories</returns>
        public static List<ComplianceCategory> GetComplianceCategoriesForNodes(Int32 tenantId, string dpmIds)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceCategoriesForNodes(dpmIds);
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

        public static List<ComplianceItem> GetComplianceItemsByCategory(Int32 categoryId, Int32 tenantId, out Entity.SystemEventSetting systemEventSetting, Int32 subEventId = 0)
        {
            try
            {
                //systemEventSetting = TemplatesManager.GetCategoryLevelTemplate(categoryId, subEventId, tenantId);
                systemEventSetting = null;
                //UAT-2069 :GetComplianceItemsByCategory accepts multiple categoryIDs.
                List<Int32> lstCategoryIds = new List<Int32>();
                lstCategoryIds.Add(categoryId);
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceItemsByCategory(lstCategoryIds);
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
        /// UAT-2069
        /// </summary>
        /// <param name="lstCategoryIds"></param>
        /// <param name="tenantId"></param>
        /// <param name="systemEventSetting"></param>
        /// <param name="subEventId"></param>
        /// <returns></returns>
        public static List<ComplianceItem> GetComplianceItemsByCategoryIds(List<Int32> lstCategoryIds, Int32 tenantId, out Entity.SystemEventSetting systemEventSetting, Int32 subEventId = 0)
        {
            try
            {
                systemEventSetting = null;
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceItemsByCategory(lstCategoryIds);
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
        /// Saves the Compliance Category in th MasterDB followed by ClientDB(in case of client).
        /// </summary>
        /// <param name="category">ComplianceCategory Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <param name="notesDetail">Notes Detail</param>
        public static Int32 SaveCategoryDetail(ComplianceCategory category, Int32 currentLoggedInUserId, Dictionary<String, String> notesDetail = null)
        {
            try
            {
                //commented for UAT 198
                //ComplianceCategory client = null;

                //if (category.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    client = category.Clone();

                //    //if (BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).CheckIfCategoryNameAlreadyExist(category.CategoryName, category.ComplianceCategoryID, SecurityManager.DefaultTenantID))
                //    //{
                //    var complianceCategory = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetExistingCategory(category.CategoryName);

                //    if (complianceCategory.IsNotNull())
                //    {
                //        category.CategoryName = AppendSuffixToExistingName(complianceCategory);
                //    }

                //    //category.CategoryName = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).AppendSuffixToExistingName(category.CategoryName);
                //    //}
                //}

                //ComplianceCategory resultCategory = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).SaveCategoryDetail(category, currentLoggedInUserId);

                //if (notesDetail.IsNotNull())
                //{
                //    SaveLargeContent(currentLoggedInUserId, SecurityManager.DefaultTenantID, notesDetail, resultCategory.ComplianceCategoryID, LCObjectType.ComplianceCategory.GetStringValue());
                //}

                //if (category.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    client.CopiedFromCode = resultCategory.Code;

                ComplianceCategory complianceCategory = BALUtils.GetComplianceSetupRepoInstance(category.TenantID.Value).SaveCategoryDetail(category, currentLoggedInUserId);

                if (notesDetail.IsNotNull())
                {
                    SaveLargeContent(currentLoggedInUserId, complianceCategory.TenantID.Value, notesDetail, complianceCategory.ComplianceCategoryID, LCObjectType.ComplianceCategory.GetStringValue());
                }
                return complianceCategory.ComplianceCategoryID;
                //}
                // return resultCategory.ComplianceCategoryID;
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
        /// Updates the Compliance Category in Master DB in case of Admin or
        /// updates the Compliance Category in Client DB and adds row in Master DB in case of Client Admin..
        /// </summary>
        /// <param name="category">ComplianceCategory Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <param name="notesDetail">Notes Detail</param>
        /// <param name="isMasterScreen ">Will be true if the screen is being opened from 'Master Category' else will be false (i.e. screen is 
        /// opened from 'Compliance Mapping') </param>
        public static void UpdateCategoryDetail(ComplianceCategory category, Int32 currentLoggedInUserId, Dictionary<String, String> notesDetail = null, Boolean isMasterScreen = false)
        {
            try
            {
                //commented for UAT 198
                //ComplianceCategory complianceCategory = null;
                //if (category.TenantID != SecurityManager.DefaultTenantID)// if package being updated is in client database
                //{
                //    //this cloning is kept separate for performance
                //    complianceCategory = category.Clone();
                //}
                //Update package in respective database (admin/ client)
                BALUtils.GetComplianceSetupRepoInstance(category.TenantID.Value).UpdateCategoryDetail(category, currentLoggedInUserId, isMasterScreen);

                if (notesDetail.IsNotNull())
                {
                    SaveLargeContent(currentLoggedInUserId, category.TenantID.Value, notesDetail, category.ComplianceCategoryID, LCObjectType.ComplianceCategory.GetStringValue());
                }

                //commented for UAT 198
                //// if package being updated is in client database, then create a version in admin database
                //if (category.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    var lastComplianceCategory = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetExistingCategory(category.CategoryName);

                //    if (lastComplianceCategory.IsNotNull())
                //    {
                //        complianceCategory.CategoryName = AppendSuffixToExistingName(lastComplianceCategory);
                //    }
                //    complianceCategory.CopiedFromCode = category.Code;
                //    //create a version of the package in admin database.
                //    complianceCategory = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).SaveCategoryDetail(complianceCategory, currentLoggedInUserId);
                //    if (notesDetail.IsNotNull())
                //    {
                //        SaveLargeContent(currentLoggedInUserId, SecurityManager.DefaultTenantID, notesDetail, complianceCategory.ComplianceCategoryID, LCObjectType.ComplianceCategory.GetStringValue());
                //    }
                //}
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
        /// Deletes the Compliance Category in case no dependent is present for the selected Category.
        /// </summary>
        /// <param name="categoryID">category ID</param>
        /// <param name="currentUserId">Current LoggedIn User Id</param>
        /// <param name="tenantId">Tenant Id</param>
        public static Boolean DeleteComplianceCategory(Int32 categoryID, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteComplianceCategory(categoryID, currentUserId);

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
        /// Checks if the category name already exists.
        /// </summary>
        /// <param name="categoryName">Category Name</param>
        /// <param name="complianceCategoryId">Compliance Category Id</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>True or false</returns>
        public static Boolean CheckIfCategoryNameAlreadyExist(String categoryName, Int32 complianceCategoryID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).CheckIfCategoryNameAlreadyExist(categoryName, complianceCategoryID, tenantId);

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
        /// Appends string in the name if the name already exists.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private static String AppendSuffixToExistingName(ComplianceCategory complianceCategory)
        {
            String delimiter = "-DUP";

            if (complianceCategory.CategoryName.Contains(delimiter))
            {
                String[] suffixToAppend = complianceCategory.CategoryName.Split(new string[] { delimiter }, StringSplitOptions.None);
                Int32 indexToAppend = Convert.ToInt32(suffixToAppend.Last()) + 1;
                return suffixToAppend.First() + delimiter + indexToAppend;
            }
            else
            {
                return complianceCategory.CategoryName + delimiter + "1";
            }
        }

        /// <summary>
        /// To get Not Mapped Compliance Categories
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ComplianceCategory> GetNotMappedComplianceCategories(Int32 packageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetNotMappedComplianceCategories(packageId);
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

        public static Boolean UpdateCompliancePackageCategoryDisplayOrder(Int32 tenantId, Int32 currentloggedInUserId, CompliancePackageCategory compliancePackageCategory
                                                                          , CompliancePackageCategory oldCompliancePackageCategory)
        {
            try
            {
                if (BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateCompliancePackageCategoryDisplayOrder(compliancePackageCategory, currentloggedInUserId))
                {
                    //call usp_ProcessOptionalCategory SP when working on tenant db
                    Boolean IsCreatedByAdmin = tenantId == SecurityManager.DefaultTenantID ? true : false;
                    DateTime currentDate = DateTime.Now;
                    //Changes related to UAt-1209
                    DateTime? newStartDate = compliancePackageCategory.CPC_ComplianceRqdStartDate;
                    DateTime? newEndDate = compliancePackageCategory.CPC_ComplianceRqdEndDate;
                    DateTime? oldStartDate = oldCompliancePackageCategory.CPC_ComplianceRqdStartDate;
                    DateTime? oldEndDate = oldCompliancePackageCategory.CPC_ComplianceRqdEndDate;
                    Boolean newCmplcRqd = compliancePackageCategory.CPC_ComplianceRequired;
                    Boolean oldCmplcRqd = oldCompliancePackageCategory.CPC_ComplianceRequired;
                    if (!IsCreatedByAdmin && (newStartDate != oldStartDate || newEndDate != oldEndDate || newCmplcRqd != oldCmplcRqd))
                    {
                        //Update only if either startDate,EndDate or Compliance Rqd is changed
                        if ((newStartDate.IsNull() && newEndDate.IsNull())
                            || (
                                (currentDate.Month > newStartDate.Value.Month || (currentDate.Month == newStartDate.Value.Month && currentDate.Day >= newStartDate.Value.Day))
                                && (currentDate.Month < newEndDate.Value.Month || (currentDate.Month > newEndDate.Value.Month && newEndDate.Value.Month < newStartDate.Value.Month) || (currentDate.Month == newEndDate.Value.Month && currentDate.Day <= newEndDate.Value.Day))
                            ))
                        {
                            if (compliancePackageCategory.CPC_ComplianceRequired)
                            {
                                BALUtils.GetComplianceSetupRepoInstance(tenantId).ProcessRequiredCategory(compliancePackageCategory.CPC_PackageID, compliancePackageCategory.CPC_CategoryID, currentloggedInUserId);
                                return true;
                            }
                            else
                            {
                                #region UAT-3805

                                List<CompliancePackageCategory> lstOptionalCategory = new List<CompliancePackageCategory>();
                                lstOptionalCategory.Add(compliancePackageCategory);
                                List<PackageSubscription> lstPkgSubscriptionBeforeUpdate = ProfileSharingManager.GetCompliancePkgSubscriptionData(tenantId, lstOptionalCategory);
                                Dictionary<Int32, String> dicApprovedCatOfSub = new Dictionary<Int32, String>();

                                lstPkgSubscriptionBeforeUpdate.DistinctBy(dst => dst.PackageSubscriptionID).ForEach(pkgSub =>
                                {
                                    List<Int32> lstApprovedCaegoryIDs = new List<Int32>();
                                    if (!pkgSub.ApplicantComplianceCategoryDatas.IsNullOrEmpty())
                                    {
                                        lstApprovedCaegoryIDs = pkgSub.ApplicantComplianceCategoryDatas.Where(cnd => (cnd.lkpCategoryComplianceStatu.Code ==
                                                                                                                  ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                                                                                                  ||
                                                                                                                  cnd.lkpCategoryComplianceStatu.Code ==
                                                                                                                  ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()
                                                                                                                   )
                                                                                                                   && !cnd.IsDeleted
                                                                                                                  ).Select(slct => slct.ComplianceCategoryID).ToList();
                                    }

                                    if (!dicApprovedCatOfSub.ContainsKey(pkgSub.PackageSubscriptionID))
                                    {
                                        dicApprovedCatOfSub.Add(pkgSub.PackageSubscriptionID, lstApprovedCaegoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", lstApprovedCaegoryIDs));

                                    }

                                });

                                List<ItemDocNotificationRequestDataContract> itemDocNotificationRequestData = new List<ItemDocNotificationRequestDataContract>();
                                #endregion

                                BALUtils.GetComplianceSetupRepoInstance(tenantId).ProcessOptionalCategory(compliancePackageCategory.CPC_PackageID, compliancePackageCategory.CPC_CategoryID, currentloggedInUserId);

                                #region UAT-3805
                                List<PackageSubscription> lstPkgSubscriptionAfterUpdate = ProfileSharingManager.GetCompliancePkgSubscriptionData(tenantId, lstOptionalCategory);

                                lstPkgSubscriptionAfterUpdate.DistinctBy(dst => dst.PackageSubscriptionID).ForEach(pkgSub =>
                                {
                                    String approvedCategories = !dicApprovedCatOfSub.ContainsKey(pkgSub.PackageSubscriptionID) ? String.Empty : dicApprovedCatOfSub.GetValue(pkgSub.PackageSubscriptionID);
                                    List<Int32> lstCategoryIds = pkgSub.ApplicantComplianceCategoryDatas.Where(x => !x.IsDeleted).Select(sl => sl.ComplianceCategoryID).ToList();
                                    if (!lstCategoryIds.IsNullOrEmpty())
                                    {
                                        ItemDocNotificationRequestDataContract itemDocRequestData = new ItemDocNotificationRequestDataContract();
                                        String categoryIds = String.Join(",", lstCategoryIds);

                                        itemDocRequestData.TenantID = tenantId;
                                        itemDocRequestData.CategoryIds = categoryIds;
                                        itemDocRequestData.ApplicantOrgUserID = pkgSub.OrganizationUserID.Value;
                                        itemDocRequestData.ApprovedCategoryIds = approvedCategories;
                                        itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.COMPLIANCE.GetStringValue();
                                        itemDocRequestData.PackageSubscriptionID = pkgSub.PackageSubscriptionID;
                                        itemDocRequestData.RPS_ID = null;
                                        itemDocRequestData.CurrentLoggedInUserID = currentloggedInUserId;
                                        itemDocNotificationRequestData.Add(itemDocRequestData);
                                    }

                                });

                                //UAT-3805
                                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                                Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                                dicParam.Add("CategoryData", itemDocNotificationRequestData);
                                ProfileSharingManager.RunParallelTaskItemDocNotificationOnCatApproval(dicParam, LoggerService, ExceptiomService);
                                #endregion
                                return true;
                            }
                        }
                        //Delete all previous entries of hostory table so that compliance rqd process cam be executed again.
                        else
                        {
                            BALUtils.GetComplianceSetupRepoInstance(tenantId).DeletePreviousComplianceRqdActionHistory(compliancePackageCategory.CPC_ID, currentloggedInUserId);
                        }
                    }
                    return true;
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

        public static CompliancePackageCategory GetCompliancePackageCategory(Int32 tenantId, Int32 categoryId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCompliancePackageCategory(categoryId, packageId);
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

        #region Manage Compliance Items

        /// <summary>
        /// Gets the list of ComplianceItems for display in grid
        /// </summary>
        /// <param name="tenantId">Id of the tenant to which the current user belongs to</param>
        /// <returns>List of the Compliance Items</returns>
        public static List<ComplianceItem> GetComplianceItems(Int32 tenantId, Boolean getTenantName)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceItems(getTenantName, tenantList);
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
        /// Gets the list of ComplianceItems for display in grid
        /// </summary>
        /// <param name="tenantId">Id of the tenant to which the current user belongs to</param>
        /// <returns>List of the Compliance Items</returns>
        public static List<ComplianceItem> GetComplianceItemsForNodes(Int32 tenantId, string dpmIds)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceItemsForNodes(dpmIds);
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
        /// Sets the IsDeleted of the selected compliance item to true.
        /// </summary>
        /// <param name="complianceItemId">Id of compliance item to be deleted</param>
        /// <param name="currentUserId">Id of the current logged in user, for ModifiedBy</param>
        /// <param name="tenantId">Id of the tenant to which the current user belongs to</param>
        /// <returns>Status of deletion, if it was success or association exists</returns>
        public static Boolean DeleteComplianceItem(Int32 complianceItemId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteComplianceItem(complianceItemId, currentUserId);
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
        /// Save/Update the compliance item for ADB admin as well as client admin
        /// </summary>
        /// <param name="complianceItem">Details of the compliance item to save/update</param>
        /// <param name="tenantId">Id of the tenant to which the current user belongs to</param>
        public static ComplianceItem SaveComplianceItem(ComplianceItem complianceItem, String explanatoryNotes)
        {
            try
            {
                //ComplianceItem cmpClientItem = complianceItem.Clone();

                //if (Convert.ToInt32(cmpClientItem.TenantID) != SecurityManager.DefaultTenantID && Convert.ToInt32(cmpClientItem.TenantID) != 0) // Save the data for client admin 
                //{
                //    complianceItem.ComplianceCategoryItems.Clear();
                //}
                //ComplianceItem cmpAdminItemSaved = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).SaveComplianceItem(complianceItem, SecurityManager.DefaultTenantID);
                //SaveExplanatoryNotesItems(cmpAdminItemSaved.ComplianceItemID, explanatoryNotes, cmpAdminItemSaved.CreatedBy, SecurityManager.DefaultTenantID);

                //if (Convert.ToInt32(cmpClientItem.TenantID) != SecurityManager.DefaultTenantID && Convert.ToInt32(cmpClientItem.TenantID) != 0) // Save the data for client admin 
                //{
                //    cmpClientItem.CopiedFromCode = cmpAdminItemSaved.Code;

                BALUtils.GetComplianceSetupRepoInstance(Convert.ToInt32(complianceItem.TenantID)).SaveComplianceItem(complianceItem, Convert.ToInt32(complianceItem.TenantID));
                SaveExplanatoryNotesItems(complianceItem.ComplianceItemID, explanatoryNotes, complianceItem.CreatedBy, Convert.ToInt32(complianceItem.TenantID));
                return complianceItem;
                //}

                //if (complianceItem.TenantID == SecurityManager.DefaultTenantID)
                //{
                //    return complianceItem;
                //}
                //else
                //{
                //    return cmpClientItem;
                //}
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
        /// Checks for the availability of unique Item Name
        /// </summary>
        /// <param name="complianceItem">Name of Item</param>
        /// <param name="complianceItemId">Id of Compliance item</param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean CheckIfItemAlreadyExist(String complianceItem, Int32 complianceItemId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).CheckIfItemAlreadyExist(complianceItem, complianceItemId, tenantId);
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
        /// Save/Update the compliance item for ADB admin as well as client admin
        /// </summary>
        /// <param name="complianceItem">Details of the compliance item to save/update</param>
        /// <param name="tenantId">Id of the tenant to which the current user belongs to</param>
        public static Boolean UpdateComplianceItem(ComplianceItem complianceItem, String explanatoryNotes, Boolean isMasterScreen = false)
        {
            try
            {   //commented for UAT 198
                //ComplianceItem cmpItemCopy = null;
                //if (complianceItem.TenantID != SecurityManager.DefaultTenantID)// if item being updated is in client database
                //{
                //    //this cloning is kept separate for performance
                //    cmpItemCopy = complianceItem.Clone();
                //}
                //Update item in respective database (admin/ client)
                BALUtils.GetComplianceSetupRepoInstance(complianceItem.TenantID.Value).UpdateComplianceItem(complianceItem, complianceItem.TenantID.Value, isMasterScreen);
                SaveExplanatoryNotesItems(complianceItem.ComplianceItemID, explanatoryNotes, complianceItem.ModifiedBy.Value, complianceItem.TenantID.Value);

                //commented for UAT 198
                //// if item being updated is in client database, then create a version in admin database
                //if (complianceItem.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    //create a version of the item in admin database.
                //    cmpItemCopy.CopiedFromCode = complianceItem.Code;
                //    ComplianceItem cmpAdminItemSaved = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).SaveComplianceItem(cmpItemCopy, SecurityManager.DefaultTenantID);
                //    SaveExplanatoryNotesItems(cmpAdminItemSaved.ComplianceItemID, explanatoryNotes, cmpAdminItemSaved.CreatedBy, SecurityManager.DefaultTenantID);
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


        public static Boolean UpdateComplianceCategoryItemDisplayOrder(Int32 tenantId, Int32 itemId, Int32 categoryId, Int32 displayOrder, Int32 currentloggedInUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateComplianceCategoryItemDisplayOrder(itemId, categoryId, displayOrder, currentloggedInUserId);
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

        #region Manage Compliance Attributes

        public static List<ComplianceAttribute> GetComplianceAttributes(Int32 tenantId, Boolean getTenantName)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceAttributes(getTenantName, tenantList);
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

        public static List<ComplianceAttribute> GetNotMappedComplianceAttributes(Int32 itemId, Int32 tenantId)
        {
            try
            {
                String fileUploadAttributeDatatypeCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
                String viewDocAttributeDataTypeCode = ComplianceAttributeDatatypes.View_Document.GetStringValue();
                Int32 fileUploadAttributeDatatypeId = LookupManager.GetLookUpData<lkpComplianceAttributeDatatype>(tenantId).FirstOrDefault(x => x.Code == fileUploadAttributeDatatypeCode).ComplianceAttributeDatatypeID;
                Int32 viewDocAttributeDataTypeId = LookupManager.GetLookUpData<lkpComplianceAttributeDatatype>(tenantId).FirstOrDefault(x => x.Code == viewDocAttributeDataTypeCode).ComplianceAttributeDatatypeID;
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetNotMappedComplianceAttributes(itemId, fileUploadAttributeDatatypeId, viewDocAttributeDataTypeId);
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

        public static ComplianceAttribute GetComplianceAttribute(Int32 complianceAttributeID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceAttribute(complianceAttributeID);
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

        public static List<lkpComplianceAttributeDatatype> GetComplianceAttributeDatatype(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpComplianceAttributeDatatype>(tenantId);
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

        public static List<lkpComplianceAttributeType> GetComplianceAttributeType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpComplianceAttributeType>(tenantId);
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
        /// To get compliance attribute group list
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ComplianceAttributeGroup> GetComplianceAttributeGroup(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceAttributeGroup();
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

        public static ComplianceAttribute AddComplianceAttribute(ComplianceAttribute complianceAttribute, String explanatoryNotes)
        {
            try
            {
                Boolean result = false;
                //ComplianceAttribute clonedComplianceAttribute = complianceAttribute.Clone();
                //if (complianceAttribute.TenantID != SecurityManager.DefaultTenantID)
                //    complianceAttribute.ComplianceItemAttributes.Clear();

                //if (complianceAttribute.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    //If TenantIDs are different then save ComplianceAttributeGroupID = null into master security DB.
                //    complianceAttribute.ComplianceAttributeGroupID = null;
                //}
                //Boolean result = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).AddComplianceAttribute(complianceAttribute);

                //SaveExplanatoryNotes(complianceAttribute.ComplianceAttributeID, explanatoryNotes, complianceAttribute.CreatedByID.Value, SecurityManager.DefaultTenantID);

                //if (result && complianceAttribute.TenantID != SecurityManager.DefaultTenantID)
                //{

                //    clonedComplianceAttribute.CopiedFromCode = complianceAttribute.Code;
                result = BALUtils.GetComplianceSetupRepoInstance(complianceAttribute.TenantID.Value).AddComplianceAttribute(complianceAttribute);
                SaveExplanatoryNotes(complianceAttribute.ComplianceAttributeID, explanatoryNotes,
                        complianceAttribute.CreatedByID.Value, complianceAttribute.TenantID.Value);
                return complianceAttribute;
                //}
                //if (complianceAttribute.TenantID == SecurityManager.DefaultTenantID)
                //{
                //    return complianceAttribute;
                //}
                //else
                //{
                //    return clonedComplianceAttribute;
                //}
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


        public static Boolean UpdateComplianceAttribute(ComplianceAttribute complianceAttribute, String explanatoryNotes)
        {
            try
            {
                Boolean result = false;
                //ComplianceAttribute complianceAttributeForMasterDB = null;
                //if (complianceAttribute.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    //this cloning is kept separate for performance
                //    complianceAttributeForMasterDB = complianceAttribute.Clone();
                //    //If TenantIDs are different then save ComplianceAttributeGroupID = null into master security DB.
                //    complianceAttributeForMasterDB.ComplianceAttributeGroupID = null;
                //}

                result = BALUtils.GetComplianceSetupRepoInstance(complianceAttribute.TenantID.Value).UpdateComplianceAttribute(complianceAttribute);
                if (result)
                {
                    SaveExplanatoryNotes(complianceAttribute.ComplianceAttributeID, explanatoryNotes,
                        complianceAttribute.CreatedByID.Value, complianceAttribute.TenantID.Value);
                }

                //if (result && complianceAttribute.TenantID != SecurityManager.DefaultTenantID)
                //{
                //    complianceAttributeForMasterDB.ComplianceAttributeID = 0;
                //    complianceAttributeForMasterDB.CreatedOn = DateTime.Now;
                //    complianceAttributeForMasterDB.CreatedByID = complianceAttribute.ModifiedByID;
                //    complianceAttributeForMasterDB.CopiedFromCode = complianceAttribute.Code;
                //    result = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).AddComplianceAttribute(complianceAttributeForMasterDB);
                //    SaveExplanatoryNotes(complianceAttributeForMasterDB.ComplianceAttributeID, explanatoryNotes,
                //            complianceAttributeForMasterDB.CreatedByID.Value, SecurityManager.DefaultTenantID);
                //    //complianceAttribute.CopiedFromCode = complianceAttributeForMasterDB.Code;
                //}

                return result;

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



        public static Boolean DeleteComplianceAttribute(Int32 complianceAttributeID, Int32 modifiedByID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteComplianceAttribute(complianceAttributeID, modifiedByID);
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

        public static Boolean IsFileUploadAttributePresent(Int32 itemId, Int32 tenantId)
        {
            try
            {
                String fileUploadAttributeDatatypeCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
                Int32 fileUploadAttributeDatatypeId = LookupManager.GetLookUpData<lkpComplianceAttributeDatatype>(tenantId).FirstOrDefault(x => x.Code == fileUploadAttributeDatatypeCode).ComplianceAttributeDatatypeID;
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsFileUploadAttributePresent(itemId, fileUploadAttributeDatatypeId);
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


        public static Boolean IsViewDocAttributePresent(Int32 itemId, Int32 tenantId)
        {
            try
            {
                String viewDocAttributeDatatypeCode = ComplianceAttributeDatatypes.View_Document.GetStringValue();
                Int32 viewDocAttributeDatatypeId = LookupManager.GetLookUpData<lkpComplianceAttributeDatatype>(tenantId).FirstOrDefault(x => x.Code == viewDocAttributeDatatypeCode).ComplianceAttributeDatatypeID;
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsViewDocAttributePresent(itemId, viewDocAttributeDatatypeId);
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

        public static Boolean UpdateComplianceItemAttributeDisplayOrder(Int32 tenantId, Int32 complianceAttributeID, Int32 itemID, Int32 displayOrder, Int32 currentloggedInUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateComplianceItemAttributeDisplayOrder(complianceAttributeID, itemID, displayOrder, currentloggedInUserId);
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

        #region Private
        private static void SaveExplanatoryNotes(Int32 complianceAttributeID, String explanatoryNotes, Int32 userId, Int32 tenantId)
        {
            LargeContent notesContent = new LargeContent
            {
                LC_ObjectID = complianceAttributeID,
                LC_Content = explanatoryNotes,
                LC_IsDeleted = false
            };

            ComplianceSetupManager.SaveLargeContentRecord(notesContent, LCObjectType.ComplianceATR.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(),
                        userId, tenantId);

        }

        private static void SaveExplanatoryNotesItems(Int32 complianceItemsId, String explanatoryNotes, Int32 userId, Int32 tenantId)
        {
            LargeContent notesContent = new LargeContent
            {
                LC_ObjectID = complianceItemsId,
                LC_Content = explanatoryNotes,
                LC_IsDeleted = false
            };
            ComplianceSetupManager.SaveLargeContentRecord(notesContent, LCObjectType.ComplianceItem.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(),
                        userId, tenantId);
        }
        #endregion



        #endregion


        #region Manage Compliance RuleSet

        public static RuleSet SaveComplianceRuleSetDetail(RuleSet ruleSet, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                RuleSet resultRuleSet = BALUtils.GetComplianceSetupRepoInstance(ruleSet.RLS_TenantID.Value).SaveComplianceRuleSetDetail(ruleSet, currentUserId, ruleSet.RLS_TenantID.Value);
                return resultRuleSet;
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

        public static RuleSet UpdateComplianceRuleSetDetail(RuleSet ruleSet, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                if (ruleSet.RLS_TenantID != SecurityManager.DefaultTenantID)
                {
                    RuleSet ruleSetDetail = ruleSet.Clone();
                    ruleSetDetail.RLS_CopiedFromCode = ruleSet.RLS_Code;
                    ruleSetDetail = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).SaveComplianceRuleSetDetail(ruleSetDetail, currentUserId, ruleSetDetail.RLS_TenantID.Value);
                }
                return BALUtils.GetComplianceSetupRepoInstance(ruleSet.RLS_TenantID.Value).UpdateComplianceRuleSetDetail(ruleSet, currentUserId, ruleSet.RLS_TenantID.Value);
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

        public static Boolean DeleteComplianceRuleSetDetail(Int32 ruleSetId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteComplianceRuleSetDetail(ruleSetId, currentUserId, tenantId);

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

        public static List<lkpRuleType> GetComplianceRuleSetType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleType>(tenantId);
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
        #endregion

        #region Admin Mapping Screens

        #region Package-Category Mapping

        public static List<ComplianceCategory> GetcomplianceCategoriesByPackageList(Int32 packageId, Int32 tenantId)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetcomplianceCategoriesByPackageList(packageId, tenantList, tenantId);

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

        public static List<CompliancePackageCategory> GetcomplianceCategoriesByPackage(Int32 packageId, Int32 tenantId, Boolean getTenantName)
        {
            try
            {
                List<Int32> lstPackageId = new List<Int32>();
                lstPackageId.Add(packageId);
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetcomplianceCategoriesByPackage(lstPackageId, getTenantName, tenantList);

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

        public static List<CompliancePackageCategory> GetcomplianceCategoriesByPackageIds(List<Int32> lstPackageIds, Int32 tenantId, Boolean getTenantName)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetcomplianceCategoriesByPackage(lstPackageIds, getTenantName, tenantList);

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
        public static Boolean SaveCompliancePackageCategoryMapping(Int32 currentUserId, Int32 tenantId, CompliancePackageCategory compliancePackageCategory)
        {
            try
            {
                Boolean IsCreatedByAdmin = tenantId == SecurityManager.DefaultTenantID ? true : false;
                if (BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveCompliancePackageCategoryMapping(compliancePackageCategory, currentUserId, IsCreatedByAdmin))
                {
                    //call usp_ProcessOptionalCategory SP when working on tenant db and compliance is not required
                    DateTime currentDate = DateTime.Now;
                    DateTime? newStartDate = compliancePackageCategory.CPC_ComplianceRqdStartDate;
                    DateTime? newEndDate = compliancePackageCategory.CPC_ComplianceRqdEndDate;
                    if (!IsCreatedByAdmin && !compliancePackageCategory.CPC_ComplianceRequired
                        && ((newStartDate.IsNull() && newEndDate.IsNull())
                            || (//Changes related to UAt-1209.
                                (currentDate.Month > newStartDate.Value.Month || (currentDate.Month == newStartDate.Value.Month && currentDate.Day >= newStartDate.Value.Day))
                                && (currentDate.Month < newEndDate.Value.Month || (currentDate.Month > newEndDate.Value.Month && newEndDate.Value.Month < newStartDate.Value.Month) || (currentDate.Month == newEndDate.Value.Month && currentDate.Day <= newEndDate.Value.Day))
                            ))
                        )
                    {
                        BALUtils.GetComplianceSetupRepoInstance(tenantId).ProcessOptionalCategory(compliancePackageCategory.CPC_PackageID, compliancePackageCategory.CPC_CategoryID, currentUserId);
                    }
                    CreateCategoryAssignmentHierarchy(compliancePackageCategory.CPC_PackageID, compliancePackageCategory.CPC_CategoryID, currentUserId, tenantId);
                    return true;
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

        public static ComplianceCategory getCurrentCategoryInfo(Int32 categoryId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).getCurrentCategoryInfo(categoryId);
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

        public static CompliancePackage GetCurrentPackageInfo(Int32 packageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCurrentPackageInfo(packageId);
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

        public static List<dynamic> GetPackageBundleNodeHierarchy(Int32 packageId, Int32 tenantId)
        {
            try
            {
                var result = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPackageBundleNodeHierarchy(packageId);
                return result;
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

        public static Boolean DeleteCompliancePackageCategoryMapping(Int32 packageId, Int32 categoryId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                if (tenantId == SecurityManager.DefaultTenantID)
                {
                    if (BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteCompliancePackageCategoryMapping(packageId, categoryId, currentUserId))
                    {
                        DeleteCategoryAssignmentHierarchy(packageId, categoryId, currentUserId, tenantId);
                        return true;
                    }
                }
                else
                {
                    Boolean isScheduleActionRecordInserted = BALUtils.GetComplianceSetupRepoInstance(tenantId).DeletePackageCategoryMappingAndAssociatedData(packageId, categoryId, currentUserId, tenantId);
                    if (isScheduleActionRecordInserted)
                    {
                        InsertSystemSeriveTrigger(currentUserId, tenantId, LkpSystemService.EXECUTE_MULTI_RULES_SERVICE);
                    }
                    return true;
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


        #endregion

        #region Category-Item Mapping

        /// <summary>
        /// Gets the list of Items related to a category
        /// </summary>
        /// <param name="categoryId">Id of the selected category</param>
        /// <returns>List of the items of that category</returns>
        public static List<ComplianceCategoryItem> GetComplianceCategoryItems(Int32 categoryId, Int32 tenantId, Boolean ifTenantNameRequired)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceCategoryItems(categoryId, ifTenantNameRequired, tenantList);
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
        /// Save category-item mapping
        /// </summary>
        /// <param name="complianceItem">Details of the compliance item with mapping information to save/update</param>
        /// <param name="tenantId">Id of the tenant to which the current user belongs to</param>
        public static void SaveCategoryItemMapping(ComplianceCategoryItem complianceCategoryItem, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveCategoryItemMapping(complianceCategoryItem);
                CreateItemAssignmentHierarchy(complianceCategoryItem, tenantId);
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

        public static void DeleteCategoryItemMapping(Int32 complianceCategoryItemId, Int32 categoryId, Int32 itemId, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                if (SecurityManager.DefaultTenantID == tenantId)
                {
                    BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteCategoryItemMapping(complianceCategoryItemId, currentLoggedInUserId);
                    DeleteItemAssignmentHierarchy(categoryId, itemId, currentLoggedInUserId, tenantId);
                }
                else
                {
                    Boolean isScheduleActionRecordInserted = BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteCategoryItemMappingAndAssociatedData(categoryId, itemId, currentLoggedInUserId, tenantId);
                    if (isScheduleActionRecordInserted)
                    {
                        InsertSystemSeriveTrigger(currentLoggedInUserId, tenantId, LkpSystemService.EXECUTE_MULTI_RULES_SERVICE);
                    }
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

        public static ComplianceItem getCurrentItemInfo(Int32 itemId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).getCurrentItemInfo(itemId);
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
        /// Get the list of Items not associated with the current Category in the mapping screen.
        /// </summary>
        /// <param name="currentCategoryId">Id of the category not associated with</param>
        /// <returns>Lit of the compliance items.</returns>
        public static List<ComplianceItem> GetAvailableComplianceItems(Int32 currentCategoryId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAvailableComplianceItems(currentCategoryId);
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

        #region Item-Attributes Mapping

        public static List<ComplianceItemAttribute> GetComplianceItemAttribute(Int32 itemID, Int32 tenantId, Boolean ifTenantNameRequired)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceItemAttribute(itemID, ifTenantNameRequired, tenantList);
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

        public static ComplianceItemAttribute GetComplianceItemAttributeByID(Int32 cia_ID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceItemAttributeByID(cia_ID);
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

        public static Boolean AddComplianceItemAttribute(ComplianceItemAttribute complianceItemAttribute, Int32 loggedInUserId, Int32 tenantId)
        {
            try
            {
                if (BALUtils.GetComplianceSetupRepoInstance(tenantId).AddComplianceItemAttribute(complianceItemAttribute))
                {
                    CreateAttributeAssignmentHierarchy(complianceItemAttribute, loggedInUserId, tenantId);
                    return true;
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
                throw ex;
            }
        }

        public static Boolean DeleteComplianceItemAttribute(Int32 cia_id, Int32 itemId, Int32 attributeId, Int32 modifiedByID, Int32 tenantId)
        {
            try
            {

                if (SecurityManager.DefaultTenantID == tenantId)
                {
                    if (BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteComplianceItemAttribute(cia_id, modifiedByID))
                    {
                        DeleteAttributeAssignmentHierarchy(itemId, attributeId, modifiedByID, tenantId);
                        return true;
                    }
                    return false;
                }
                else
                {
                    Boolean isScheduleActionRecordInserted = BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteComplianceItemAttributeAndAssociatedData(itemId, attributeId, modifiedByID, tenantId);
                    if (isScheduleActionRecordInserted)
                    {
                        InsertSystemSeriveTrigger(modifiedByID, tenantId, LkpSystemService.EXECUTE_MULTI_RULES_SERVICE);
                    }
                    return true;
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
                throw ex;
            }
        }

        #endregion

        #region Rule Set Mapping

        /// <summary>
        /// Gets the rule set for the given Rule Set ID.
        /// </summary>
        /// <param name="ruleSetId">Rule Set Id</param>
        /// <param name="tenantId">TenantId</param>
        /// <returns>RuleSet entity</returns>
        public static RuleSet GetRuleSetInfoByID(Int32 ruleSetId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetRuleSetInfoByID(ruleSetId);
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
        /// Returns the list of rule set types.
        /// </summary>
        /// <param name="tenantId">TenantId</param>
        /// <returns>List of Rule Set Type</returns>
        public static List<lkpRuleType> GetRuleSetTypeList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleType>(tenantId).Where(x => x.RLT_IsDeleted == false).ToList();
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
        /// Gets the list of rule set for the selected object.
        /// </summary>
        /// <param name="associationHierarchyId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="tenantId">TenantId</param>
        /// <returns>List of RuleSet</returns>
        public static List<RuleSet> GetRuleSetForObject(Int32 associationHierarchyId, Int32 objectTypeId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetRuleSetForObject(associationHierarchyId, objectTypeId);
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
        /// Saves the rule set and object mapping.
        /// </summary>
        /// <param name="ruleSetObject">RuleSet Object</param>
        /// <param name="currentUserId">Current User Id</param>
        /// <param name="tenantId">TenantId</param>
        public static void SaveRuleSetObjectMapping(RuleSetObject ruleSetObject, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveRuleSetObjectMapping(ruleSetObject, currentUserId);
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
        /// Deletes the rule set and object mapping.
        /// </summary>
        /// <param name="ruleSetId">RuleSet Id</param>
        /// <param name="objectId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="currentUserId">Current User Id</param>
        /// <param name="tenantId">TenantId</param>
        public static void DeleteRuleSet(Int32 ruleSetId, Int32 objectId, Int32 objectTypeId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteRuleSet(ruleSetId, objectId, objectTypeId, currentUserId);
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

        #region Common Methods

        /// <summary>
        /// Get the hierarchical tree data for mapping screen
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="packageIdList"></param>
        /// <returns></returns>
        public static ObjectResult<GetRuleSetTree> GetRuleSetTree(Int32 tenantId, List<Int32> packageIdList)
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Tenant Id: " + tenantId.ToString() + "";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: GetRuleSetTree with " + parametersPassed + "");

                //UAT-1116: Package selection combo box on package screens
                string packageIds = string.Join(",", packageIdList);
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetRuleSetTree(packageIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void SaveLargeContentRecord(LargeContent largeContent, String objectTypeCode, String contentTypeCode, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                Int32 contentTypeId = LookupManager.GetLookUpData<lkpLargeContentType>(tenantId).Where(obj => obj.LCT_Code == contentTypeCode && obj.LCT_IsDeleted == false).FirstOrDefault().LCT_ID;
                Int32 objectTypeId = LookupManager.GetLookUpData<lkpObjectType>(tenantId).Where(obj => obj.OT_Code == objectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
                BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveLargeContentRecord(largeContent, objectTypeId, contentTypeId, currentUserId);
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

        public static LargeContent getLargeContentRecord(Int32 objectId, String objectTypeCode, String contentTypeCode, Int32 tenantId)
        {
            try
            {
                Int32 contentTypeId = LookupManager.GetLookUpData<lkpLargeContentType>(tenantId).Where(obj => obj.LCT_Code == contentTypeCode && obj.LCT_IsDeleted == false).FirstOrDefault().LCT_ID;
                Int32 objectTypeId = LookupManager.GetLookUpData<lkpObjectType>(tenantId).Where(obj => obj.OT_Code == objectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).getLargeContentRecord(objectId, objectTypeId, contentTypeId);
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
        /// Get the hierarchical tree data for copy to client screen
        /// </summary>
        /// <returns></returns>
        public static ObjectResult<GetRuleSetTree> GetComplianceTree(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceTree();
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
        /// Get the package hierarchical tree data for package detail screen
        /// </summary>
        /// <returns>ObjectResult<GetPackageDetail></returns>
        public static ObjectResult<GetPackageDetail> GetPackageDetailTree(Int32 tenantId, Int32 packageID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPackageDetailTree(packageID);
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
        /// To get the Portfolio Subscription tree hierarchical data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        public static ObjectResult<GetPortfolioSubscriptionTree> GetPortfolioSubscriptionTree(Int32 tenantId, Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPortfolioSubscriptionTree(organizationUserId);
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
        /// Get the hierarchical Department tree data
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public static ObjectResult<GetDepartmentTree> GetDepartmentTree(Int32 tenantId, Int32 departmentId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDepartmentTree(departmentId);
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
        /// Get Department Program Mapping object.
        /// </summary>
        /// <param name="DepPrgMappingId">DepPrgMappingId</param>
        /// <returns>DeptProgramMapping</returns>
        public static DeptProgramMapping GetDepartmentProgMapping(Int32 tenantId, Int32 DepPrgMappingId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDepartmentProgMapping(DepPrgMappingId);
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
        /// Get Department Program Mapping object.
        /// </summary>
        /// <param name="DepPrgMappingId">DepPrgMappingId</param>
        /// <returns>List of DeptProgramMapping</returns>
        public static List<DeptProgramMapping> GetDepartmentProgMappingList(Int32 tenantId, String delimittedDepPrgMappingId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDepartmentProgMappingList(delimittedDepPrgMappingId);
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
        /// Get Institute Hierarchy Tree data
        /// </summary>
        /// <returns></returns>
        public static ObjectResult<InstituteHierarchyNodesList> GetInstituteHierarchyNodes(Int32 tenantId, int? orgUserID, Boolean fetchNoAccessNodes = false) //UAT-3369
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstituteHierarchyNodes(orgUserID, fetchNoAccessNodes);
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
        /// Get Institute Hierarchy Tree data
        /// </summary>
        /// <returns></returns>
        public static List<InstituteHierarchyTreeDataContract> GetInstituteHierarchyTree(Int32 tenantId, int? orgUserID, Boolean fetchNoAccessNodes = false)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstituteHierarchyTree(orgUserID, fetchNoAccessNodes);
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
        /// Get Institute Hierarchy Tree data
        /// </summary>
        /// <returns></returns>
        public static ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreeForConfiguration(Int32 tenantId, int? orgUserID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstituteHierarchyTreeForConfiguration(orgUserID);
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
        /// Get Institute Hierarchy Tree data
        /// </summary>
        /// <returns></returns>
        public static ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreeForBackgroundHierarchyPermissionType(Int32 tenantId, Int32? orgUserID, Boolean fetchNoAccessNodes = false)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstituteHierarchyTreeForBackgroundHierarchyPermissionType(orgUserID, fetchNoAccessNodes);
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
        /// Get Institute Hierarchy Order Tree data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orgUserID"></param>
        /// <returns></returns>
        public static ObjectResult<GetInstituteHierarchyOrderTree> GetInstituteHierarchyOrderTree(Int32 tenantId, int? orgUserID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstituteHierarchyOrderTree(orgUserID);
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
        /// Get Feature Permission Tree data
        /// </summary>
        /// <returns></returns>
        public static List<FeatureActionContract> GetFeaturePermissionTree(Int32 tenantId, Guid? userID)
        {
            try
            {
                DataTable tblFeaturePermissionTree = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetFeaturePermissionTree(userID, tenantId);
                return SetDataForTree(tblFeaturePermissionTree);
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

        private static List<FeatureActionContract> SetDataForTree(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(col => new FeatureActionContract
                {
                    DataID = col["DataID"] == DBNull.Value ? String.Empty : Convert.ToString(col["DataID"]),
                    ParentDataID = col["ParentDataID"] == DBNull.Value ? String.Empty : Convert.ToString(col["ParentDataID"]),
                    NodeId = col["NodeID"] == DBNull.Value ? String.Empty : Convert.ToString(col["NodeID"]),
                    ParentNodeID = col["ParentNodeID"] == DBNull.Value ? String.Empty : Convert.ToString(col["ParentNodeID"]),
                    Name = col["Value"] == DBNull.Value ? String.Empty : Convert.ToString(col["Value"]),
                    SelectedPermission = col["Permission"] == DBNull.Value ? String.Empty : Convert.ToString(col["Permission"]),
                    Code = col["UICode"] == DBNull.Value ? String.Empty : Convert.ToString(col["UICode"])
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


        /// <summary>
        /// Get the list of nodes which are associated with client.
        /// </summary>
        /// <returns></returns>
        public static ObjectResult<ComplianceAssociations> GetComplianceAssociations(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceAssociations();
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
        /// Saves large content corresponding to the given object ID in the database.
        /// </summary>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="notesDetail">Notes Detail</param>
        /// <param name="lcObjectID">Large Content Object ID</param>
        /// <param name="lcObjectType">Large Content Object Type</param>
        private static void SaveLargeContent(Int32 currentLoggedInUserId, Int32 tenantId, Dictionary<String, String> notesDetail, Int32 lcObjectID, String lcObjectType)
        {
            foreach (var notes in notesDetail)
            {
                LargeContent notesContent = new LargeContent
                {
                    LC_ObjectID = lcObjectID,
                    LC_Content = notes.Value,
                    LC_IsDeleted = false
                };
                ComplianceSetupManager.SaveLargeContentRecord(notesContent, lcObjectType, notes.Key, currentLoggedInUserId, tenantId);
            }
        }
        #endregion

        #region Create Association Hierarchy

        /// <summary>
        /// Creates assignment hierarchy nodes for a mapping of package and category
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void CreateCategoryAssignmentHierarchy(Int32 packageId, Int32 categoryId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                List<lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                Int32 objectTypeIdForPackage = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.CompliancePackage.GetStringValue()).OT_ID;
                Int32 objectTypeIdForCategory = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).OT_ID;
                BALUtils.GetComplianceSetupRepoInstance(tenantId).AddAssociationHierarchyNode(objectTypeIdForPackage, packageId, objectTypeIdForCategory, categoryId, currentUserId, true);
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
        /// Creates assignment hierarchy nodes for a mapping of item and category
        /// </summary>
        /// <param name="complianceCategoryItem"></param>
        /// <param name="tenantId"></param>
        public static void CreateItemAssignmentHierarchy(ComplianceCategoryItem complianceCategoryItem, Int32 tenantId)
        {
            try
            {
                List<lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                Int32 objectTypeIdForCategory = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).OT_ID;
                Int32 objectTypeIdForItem = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).OT_ID;
                BALUtils.GetComplianceSetupRepoInstance(tenantId).AddAssociationHierarchyNode(objectTypeIdForCategory, complianceCategoryItem.CCI_CategoryID, objectTypeIdForItem, complianceCategoryItem.CCI_ItemID, complianceCategoryItem.CCI_CreatedByID, false);
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
        /// Creates assignment hierarchy nodes for a mapping of item and attribute.
        /// </summary>
        /// <param name="complianceItemAttribute"></param>
        /// <param name="tenantId"></param>
        public static void CreateAttributeAssignmentHierarchy(ComplianceItemAttribute complianceItemAttribute, Int32 loggedInUserId, Int32 tenantId)
        {
            try
            {
                List<lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                Int32 objectTypeIdForItem = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).OT_ID;
                Int32 objectTypeIdForAttribute = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceATR.GetStringValue()).OT_ID;
                BALUtils.GetComplianceSetupRepoInstance(tenantId).AddAssociationHierarchyNode(objectTypeIdForItem, complianceItemAttribute.CIA_ItemID, objectTypeIdForAttribute, complianceItemAttribute.CIA_AttributeID, loggedInUserId, false);
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
        /// deletes assignment hierarchy nodes while deleting a mapping of package and category
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void DeleteCategoryAssignmentHierarchy(Int32 packageId, Int32 categoryId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                List<lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                Int32 objectTypeIdForPackage = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.CompliancePackage.GetStringValue()).OT_ID;
                Int32 objectTypeIdForCategory = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).OT_ID;
                BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteAssociationHierarchyNode(objectTypeIdForPackage, packageId, objectTypeIdForCategory, categoryId, currentUserId);
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
        /// deletes assignment hierarchy nodes while deleting a mapping of category and item.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="itemId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void DeleteItemAssignmentHierarchy(Int32 categoryId, Int32 itemId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                List<lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                Int32 objectTypeIdForCategory = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).OT_ID;
                Int32 objectTypeIdForItem = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).OT_ID;
                BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteAssociationHierarchyNode(objectTypeIdForCategory, categoryId, objectTypeIdForItem, itemId, currentUserId);
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
        /// deletes assignment hierarchy nodes while deleting a mapping of  item and attribute.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="attributeId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void DeleteAttributeAssignmentHierarchy(Int32 itemId, Int32 attributeId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                List<lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                Int32 objectTypeIdForItem = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).OT_ID;
                Int32 objectTypeIdForAttribute = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceATR.GetStringValue()).OT_ID;
                BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteAssociationHierarchyNode(objectTypeIdForItem, itemId, objectTypeIdForAttribute, attributeId, currentUserId);
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
        #endregion

        #region Copy to Client

        public static void CopyToClient(List<GetRuleSetTree> lstElementsToAdd, List<GetRuleSetTree> lstElementsToRemove, Int32 currentUserId, Int32 manageTenantId, Int32 tenantId)
        {
            try
            {
                //TODO: refine this code to get only required data from database, and not the whole data.
                ComplianceSetUpContract adminData = new ComplianceSetUpContract();
                adminData.CompliancePackageList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetCompliancePackages(false, null);
                adminData.lstTrackingPackageRequiredDocURLMapping = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetTrackingPackageRequiredDOCURLMapping();
                adminData.lstTrackingPackageRequiredDocURL = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetTrackingPackageRequiredDOCURL();
                adminData.ComplianceCategoryList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetComplianceCategories(false, null);
                adminData.ComplianceItemList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetComplianceItems(false, null);
                adminData.ComplianceAttributeList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetComplianceAttributes(false, null);
                adminData.LargeContentList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetLargeContent();
                adminData.RuleSetList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetRuleSet();
                adminData.RuleMappingList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllRuleMappings();
                adminData.RuleMappingDetailList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllRuleMappingDetails();
                adminData.RuleMappingObjectTreeList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllRuleMappingObjectTree();
                adminData.RuleTemplateExpressionList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllRuleTemplateExpression();
                adminData.LkpObjectTypeList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(SecurityManager.DefaultTenantID).Where(cond => cond.OT_IsDeleted == false).ToList();//BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetlkpObjectType();
                adminData.LkpRuleObjectMappingTypeList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRuleObjectMappingType>(SecurityManager.DefaultTenantID).Where(cond => cond.RMT_IsDeleted == false).ToList(); //BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetlkpRuleObjMapType();
                adminData.RuleSetTreeList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllRuleSetTree();
                adminData.AssignmentHierarchyList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetAssignmentHierarchy();
                adminData.CompliancePackageCategoryList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetCompliancePackageCategoryList();
                //Filters all the package elements.
                List<GetRuleSetTree> lstPackageElements = lstElementsToAdd.Where(obj => obj.UICode.ToLower() == RuleSetTreeNodeType.Package.ToLower()).ToList();
                //Gets InstitutionWebpage table data from security database for the filtered packages
                adminData.InstitutionWebPageList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetAdminInstitutionWebPageList(lstPackageElements, RecordType.Package.GetStringValue(), SecurityManager.DefaultTenantID);
                adminData.AttributeInstructionList = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllAttributeInstruction();

                //UAT-2985
                /*List<UniversalCategoryMapping> lstUniversalCategoryMapping = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetUniversalCategoryMappings("AAAA");
                List<UniversalItemMapping> lstUniversalItemMapping = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetUniversalItemMappings(lstUniversalCategoryMapping.Select(sel => sel.UCM_ID).ToList());
                List<UniversalAttributeMapping> lstUniversalAttrMapping = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetUniversalAttributeMappings(lstUniversalItemMapping.Select(sel => sel.UIM_ID).ToList());
                List<UniversalAttributeOptionMapping> lstUniversalAttrOptionMapping = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetUniversalAttributeOptionMappings(lstUniversalAttrMapping.Select(sel => sel.UAM_ID).ToList());
                */
                List<UniversalFieldMapping> lstUniversalFieldMapping = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetUniversalFieldMappings("AAAA");
                List<UniversalFieldOptionMapping> lstUniversalFieldOptionMapping = BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetUniversalFieldOptionMappings(lstUniversalFieldMapping.Select(sel => sel.UFM_ID).ToList());

                adminData.lstUniversalAttrMapping = lstUniversalFieldMapping;
                adminData.lstUniversalAttrOptionMapping = lstUniversalFieldOptionMapping;
                BALUtils.GetComplianceSetupRepoInstance(manageTenantId).CopyToClient(lstElementsToAdd, lstElementsToRemove, adminData, currentUserId, manageTenantId);
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

        public static void CopyComplianceToClient(String complianceDetails, Int32 currentUserId, Int32 manageTenantId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(manageTenantId).CopyComplianceToClient(complianceDetails, currentUserId, manageTenantId);
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
        /// Get the Package details for the given code.
        /// </summary>
        /// <returns></returns>
        public static CompliancePackage GetPackageDetailsByCode(Int32 tenantId, Guid? copiedFromCode)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPackageDetailsByCode(copiedFromCode);
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
        /// Get the Package details for the given code.
        /// </summary>
        /// <returns></returns>
        public static ComplianceCategory GetCategoryDetailsByCode(Int32 tenantId, Guid? copiedFromCode)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCategoryDetailsByCode(copiedFromCode);
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
        /// Get the Package details for the given code.
        /// </summary>
        /// <returns></returns>
        public static ComplianceItem GetItemDetailsByCode(Int32 tenantId, Guid? copiedFromCode)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetItemDetailsByCode(copiedFromCode);
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
        /// Get the Package details for the given code.
        /// </summary>
        /// <returns></returns>
        public static ComplianceAttribute GetAttributeDetailsByCode(Int32 tenantId, Guid? copiedFromCode)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAttributeDetailsByCode(copiedFromCode);
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

        #region assignment properties

        ///// <summary>
        ///// Gets the master list of the User types who can edit the compliance item being created/updated
        ///// </summary>
        ///// <returns>Master list of the users</returns>
        public static List<lkpEditableBy> GetComplianceEditableBy(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpEditableBy>(tenantId).Where(editableBy => editableBy.IsDeleted == false).ToList();
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

        ///// <summary>
        ///// Gets the master list of the User types who can review the compliance item being created/updated
        ///// </summary>
        ///// <returns>Master list of the users</returns>
        public static List<lkpReviewerType> GetComplianceReviwedBy(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpReviewerType>(tenantId).Where(reviwedBy => reviwedBy.IsDeleted == false).ToList();
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
        /// Gets the list of the third party reviewers as per tenant id.
        /// </summary>
        /// <returns>list of the third party reviewers</returns>
        public static List<Tenant> GetThirdPartyReviewers(Int32 tenantID, Int32 packageID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetThirdPartyReviewers(packageID);
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

        public static void UpdateAssignmentProperties(AssignmentProperty assignmentProperty, Int32 currentDataId, Int32 parentPackageId, Int32 parentCategoryId, Int32 parentItemId, String currentRuleSetTreeTypeCode, Int32 loggedInUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateAssignmentProperties(assignmentProperty, currentDataId, parentPackageId, parentCategoryId, parentItemId, currentRuleSetTreeTypeCode, loggedInUserId);
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

        public static AssignmentProperty GetAssignmentPropertyDetails(Int32 currentDataID, Int32 parentCategoryDataID, Int32 parentPackageDataID, Int32 parentItemDataID, Int32 tenantId,
            String currentRuleSetTreeTypeCode)
        {
            var lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();

            String objectTypeCode = GetObjectTypeByRuleTreeCode(currentRuleSetTreeTypeCode, tenantId, lkpObjectType);
            Int32 objectTypeId = GetObjectTypeIdByRuleTreeCode(currentRuleSetTreeTypeCode, tenantId, lkpObjectType);
            return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAssignmentPropertyDetails(currentDataID, parentCategoryDataID, parentPackageDataID, parentItemDataID, objectTypeCode, objectTypeId, lkpObjectType);
        }

        public static String GetObjectTypeByRuleTreeCode(String currentRuleSetTreeTypeCode, Int32 tenantId, List<lkpObjectType> lkpObjectType)
        {
            switch (currentRuleSetTreeTypeCode)
            {
                case RuleSetTreeNodeType.Package:
                    return lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.CompliancePackage.GetStringValue()).OT_Code;
                case RuleSetTreeNodeType.Category:
                    return lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).OT_Code;
                case RuleSetTreeNodeType.Item:
                    return lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).OT_Code;
                //Changes for Editable By for ATR
                case RuleSetTreeNodeType.Attribute:
                    return lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceATR.GetStringValue()).OT_Code;
            }
            return null;
        }

        public static Int32 GetObjectTypeIdByRuleTreeCode(String currentRuleSetTreeTypeCode, Int32 tenantId, List<lkpObjectType> lkpObjectType)
        {
            switch (currentRuleSetTreeTypeCode)
            {
                case RuleSetTreeNodeType.Package:
                    return lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.CompliancePackage.GetStringValue()).OT_ID;
                case RuleSetTreeNodeType.Category:
                    return lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).OT_ID;
                case RuleSetTreeNodeType.Item:
                    return lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).OT_ID;
                //Changes for Editable By for ATR
                case RuleSetTreeNodeType.Attribute:
                    return lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceATR.GetStringValue()).OT_ID;
            }
            return AppConsts.NONE;
        }

        public static AssignmentProperty FetchAssignmentOptions(Int32 tenantId, Int32 parentPackageDataID, Int32 parentCategoryDataID = 0, Int32 itemDataID = 0)
        {
            var lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
            return BALUtils.GetComplianceSetupRepoInstance(tenantId).FetchAssignmentOptions(parentPackageDataID, lkpObjectType, parentCategoryDataID, itemDataID);
        }

        public static List<ListItemAssignmentProperties> GetAssignmentPropertiesByCategoryId(Int32 packageId, Int32 categoryId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAssignmentPropertiesByCategoryId(packageId, categoryId);
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
        ///  Gets the list of Editable Bies for all the attributes in all the items in a category
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ListItemEditableBies> GetEditableBiesByCategoryId(Int32 packageId, Int32 categoryId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetEditableBiesByCategoryId(packageId, categoryId);
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

        public static List<AssignmentHierarchyEditableByContract> GetEditableBies(Int32 parentPackageDataID, Int32 parentCategoryDataID, Boolean isApplicant, Int32 tenantId, Int32 itemDataID = 0)
        {
            try
            {
                var lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                List<lkpEditableBy> lstEditableBy = LookupManager.GetLookUpData<Entity.ClientEntity.lkpEditableBy>(tenantId).Where(x => x.IsDeleted == false).ToList();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetEditableBies(parentPackageDataID, parentCategoryDataID, isApplicant, lkpObjectType, lstEditableBy, itemDataID);
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
        /// method to get association hierarchy id
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="itemId"></param>
        /// <param name="attributeId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public static Int32? getAssociationHierarchyIdForObject(Int32 loggedInUserId, Int32 tenantId, Int32 packageId, Int32 categoryId = 0, Int32 itemId = 0, Int32 attributeId = 0)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).getAssociationHierarchyIdForObject(loggedInUserId, packageId, categoryId, itemId, attributeId);
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

        public static List<ListCategoryEditableBies> GetEditableBiesByPackageId(Int32 parentPackageDataID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetEditableBiesByPackageId(parentPackageDataID);
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

        #region Subscription Options

        /// <summary>
        /// Gets the SubscriptionOptions list for a tenant.
        /// </summary>
        /// <returns>SubscrptOptions</returns>
        public static List<SubscriptionOption> GetSubscriptionOptionsList(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetSubscriptionOptionsList();
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
        /// Saves the SubscriptionOptions for a tenant.
        /// </summary>
        /// <returns>SubscrptOptions</returns>
        public static void SaveSubscriptionOption(Int32 tenantId, SubscriptionOption newSubscriptionOption, Int32? subscriptionOptionID = null)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveSubscriptionOption(newSubscriptionOption, subscriptionOptionID);
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
        /// Delete the SubscriptionOptions for a tenant.
        /// </summary>
        /// <returns>SubscrptOptions</returns>
        public static void DeletSubscriptionOption(Int32 tenantId, SubscriptionOption subscriptionOption)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).DeletSubscriptionOption(subscriptionOption);
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

        #region Department, Program Subscriptions and Price

        /// <summary>
        /// To get Price Model List
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<lkpPriceModel> GetPriceModelList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPriceModel>(tenantId).Where(x => x.IsDeleted == false).ToList();
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
        /// To get Price Adjustment List
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<PriceAdjustment> GetPriceAdjustmentList(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPriceAdjustmentList();
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
        /// To get Program Packages by Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<DeptProgramPackage> GetProgramPackagesByProgramMapId(Int32 deptProgramMappingID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetProgramPackagesByProgramMapId(deptProgramMappingID);

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
        /// Get the successor package dropdownlist selectedvalue
        /// </summary>
        /// <param name="DeptProgramMappingID">Source nodeid</param>
        /// <param name="SelectedSuccessorNodeID">target nodeid</param>
        /// <returns></returns>
        public static List<MobilityPackageRelation> GetSuccessorPackageIds(Int32 DeptProgramMappingID, Int32 SelectedSuccessorNodeID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetSuccessorPackageIds(DeptProgramMappingID, SelectedSuccessorNodeID);

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
        /// To get Program Packages for the given list of Program Ids.
        /// </summary>
        /// <param name="programIds"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<CompliancePackage> GetProgramPackagesByProgramId(Int32 departmentId, List<Int32> programIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetProgramPackagesByProgramId(departmentId, programIds);

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
        /// To get not mapped Compliance Packages
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<CompliancePackage> GetNotMappedCompliancePackagesByMapId(Int32 deptProgramMappingID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetNotMappedCompliancePackagesByMapId(deptProgramMappingID);
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
        /// To get Institution Nodes By Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IQueryable<DeptProgramMapping> GetInstitutionNodesByProgramMapId(Int32 deptProgramMappingID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstitutionNodesByProgramMapId(deptProgramMappingID);
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
        /// To get Institution Child Nodes By Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingIDs"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<Int32> GetInstitutionChildNodesByProgramMapId(List<Int32> deptProgramMappingIDs, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstitutionChildNodesByProgramMapId(deptProgramMappingIDs);
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
        /// To save Program Package Mapping
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean SaveProgramPackageMapping(Int32 deptProgramMappingID, Int32 packageId, Int32 currentUserId, List<Int32> _lstSelectedOptionIds, Int32 paymentApprovalRequiredID, Int32 tenantId)
        {
            try
            {
                Boolean IsCreatedByAdmin = tenantId == SecurityManager.DefaultTenantID ? true : false;
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveProgramPackageMapping(deptProgramMappingID, packageId, currentUserId, IsCreatedByAdmin, _lstSelectedOptionIds, paymentApprovalRequiredID);
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

        ///// <summary>
        ///// Save/Update the Package Level Payment Options for Compliance Package
        ///// </summary>
        ///// <param name="currentUserId"></param>
        ///// <param name="dppId"></param>
        ///// <param name="_lstSelectedOptionIds"></param>
        //public static void SaveCompliancePackagePaymentOptions(Int32 currentUserId, Int32 dppId, List<Int32> _lstPaymentOptionIds, Int32 tenantId)
        //{
        //    try
        //    {
        //        BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveCompliancePackagePaymentOptions(currentUserId, dppId, _lstPaymentOptionIds);
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

        /// <summary>
        /// To save Program Package Mapping Node
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="nodeId"></param>
        /// <param name="nodeName"></param>
        /// <param name="paymentOptions"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean SaveProgramPackageMappingNode(Int32 deptProgramMappingID, Int32 nodeId, String nodeName, List<Int32> paymentOptions, List<Int32> fileExtensions, Int32 currentUserId, Int32 tenantId, String nodeLabel,
            Boolean isAvailableForOrder, Boolean isEmployment, String splashPageUrl, String BeforeExpirationFrequency,
            Int32? AfterExpirationFrequency,Int32? SubscriptionBeforeExpiry, Int32? SubscriptionAfterExpiry, Int32? SubscriptionExpiryFrequency,
            Int32 paymentApprovalID, Boolean IsCallFromBkgHierarchySetup = false, Int32? archivedGracePeriod = null, Int32? PDFInclusionID = null, Int32? resultsSentToApplicantID = null, Int32? hierarchyNodeExemptedType = null)
        {
            try
            {
                //UAT-2501 : Update default nag email settings for new nodes 
                Int16 nagEmailNotificationTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpNodeNotificationType>(tenantId).FirstOrDefault(x => x.NNT_Code == lkpNodeNotificationTypesContext.NAGEMAILS.GetStringValue()).NNT_ID;

                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveProgramPackageMappingNode(tenantId, deptProgramMappingID, nodeId, nodeName, paymentOptions, fileExtensions, currentUserId, nodeLabel, isAvailableForOrder,
                        isEmployment, archivedGracePeriod, PDFInclusionID, resultsSentToApplicantID, splashPageUrl, BeforeExpirationFrequency,AfterExpirationFrequency,SubscriptionBeforeExpiry,SubscriptionAfterExpiry,SubscriptionExpiryFrequency, paymentApprovalID, nagEmailNotificationTypeId, hierarchyNodeExemptedType, IsCallFromBkgHierarchySetup); //UAT-2501: Added two parameters tenantId,nagEmailNotificationTypeId
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
        /// To delete Program Package Mapping
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean DeleteProgramPackageMapping(Int32 deptProgramMappingID, Int32 packageId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteProgramPackageMapping(deptProgramMappingID, packageId, currentUserId);
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
        /// To get Dept Program Package Subscription List by Dept Program Package Id
        /// </summary>
        /// <param name="DeptProgramPackageId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<DeptProgramPackageSubscription> GetDeptProgramPackageSubscriptionByProgPackageId(Int32 DeptProgramPackageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDeptProgramPackageSubscriptionByProgPackageId(DeptProgramPackageId);
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
        /// To get Dept Program Package by Package Id
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<DeptProgramPackage> GetDeptProgramPackageByPackageId(Int32 packageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDeptProgramPackageByPackageId(packageId);
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
        /// To save Program Package Subscription Mapping
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="subscriptionIDs"></param>
        /// <param name="priceModelID"></param>
        /// <param name="savedPriceModelId"></param>
        /// <param name="priority"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="lstSelectedOptionIds"></param>
        /// <returns></returns>
        public static Boolean SaveProgramPackageSubscriptionMapping(Int32 deptProgramPackageID, List<Int32> subscriptionIDs, Int32 priceModelID,
                                                                    Int32 savedPriceModelId, Int32 priority, Int32 currentUserId, List<Int32> lstSelectedOptionIds,
                                                                    Int32 paymentApprovalID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveProgramPackageSubscriptionMapping(deptProgramPackageID, subscriptionIDs, priceModelID, savedPriceModelId, priority, currentUserId, lstSelectedOptionIds, paymentApprovalID);
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
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public static PriceContract SavePriceAdjustmentDetail(Int32 ID, Int32 parentID, Int32 mappingID, Int32 parentSubscriptionID, Int32 complianceCategoryID, Decimal price, Decimal? rushOrderAdditionalPrice, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId, Int32 tenantId, String treeNodeType)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SavePriceAdjustmentDetail(ID, parentID, mappingID, parentSubscriptionID, complianceCategoryID, price, rushOrderAdditionalPrice, selectedPriceAdjustmentID, priceAdjustmentValue, currentUserId, treeNodeType);
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
        public static Boolean UpdatePriceAdjustmentDetail(Int32 ID, Int32 priceID, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId, Int32 tenantId, String treeNodeType)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdatePriceAdjustmentDetail(ID, priceID, selectedPriceAdjustmentID, priceAdjustmentValue, currentUserId, treeNodeType);
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
        /// To get Price Adjustment Data by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public static List<PriceContract> GetPriceAdjustmentData(Int32 ID, Int32 tenantId, String treeNodeType)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPriceAdjustmentData(ID, treeNodeType);
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
        ///  To get Dept Program Package By ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static DeptProgramPackage GetDeptProgramPackageByID(Int32 deptProgramPackageID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDeptProgramPackageByID(deptProgramPackageID);
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
        /// To get Dept Program Package Subscription by ID
        /// </summary>
        /// <param name="deptProgramPackageSubscriptionID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static DeptProgramPackageSubscription GetDeptProgramPackageSubscriptionByID(Int32 deptProgramPackageSubscriptionID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDeptProgramPackageSubscriptionByID(deptProgramPackageSubscriptionID);
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
        /// To get Price
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public static PriceContract GetPrice(Int32 ID, Int32 tenantId, String treeNodeType, Int32 ParentID = 0, Int32 MappingID = 0, Int32 ParentSubscriptionID = 0, Int32 ComplianceCatagoryID = 0, Int32 ItemID = 0)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPrice(ID, treeNodeType, ParentID, MappingID, ParentSubscriptionID, ComplianceCatagoryID, ItemID);
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
        /// To check if Price is Disabled
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="mappingID"></param>
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public static Boolean CheckIsPriceDisabled(Int32 parentID, Int32 parentSubscriptionID, Int32 mappingID, Int32 tenantId, String treeNodeType)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).CheckIsPriceDisabled(parentID, parentSubscriptionID, mappingID, treeNodeType);
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
        /// To show Message
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="mappingID"></param>
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public static Boolean ShowMessage(Int32 parentID, Int32 parentSubscriptionID, Int32 mappingID, Int32 tenantId, String treeNodeType)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).ShowMessage(parentID, parentSubscriptionID, mappingID, treeNodeType);
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
        /// To delete Price Adjustment Data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public static Boolean DeletePriceAdjustmentData(Int32 ID, Int32 priceID, Int32 currentUserId, Int32 tenantId, String treeNodeType)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeletePriceAdjustmentData(ID, priceID, currentUserId, treeNodeType);
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



        #region Institute Hierarchy Nodes

        /// <summary>
        ///  To get Institution Node Types
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IQueryable<InstitutionNodeType> GetInstitutionNodeTypes(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstitutionNodeTypes();
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
        /// To get Institution Nodes
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nodeTypeId"></param>
        /// <returns></returns>
        public static List<InstitutionNode> GetInstitutionNodes(Int32 tenantId, Int32 nodeTypeId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstitutionNodes(nodeTypeId);
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
        /// To delete Program Package Mapping Node
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean DeleteProgramPackageMappingByID(Int32 deptProgramMappingID, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteProgramPackageMappingByID(deptProgramMappingID, currentUserId);
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
        /// To delete dept Program Package by ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean DeleteProgramPackageByID(Int32 deptProgramPackageID, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteProgramPackageByID(deptProgramPackageID, currentUserId);
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
        /// To save mapped Payment Options and Update the availability of the node, for the Order process
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="paymentOptions"></param>
        /// <param name="currentUserId"></param>
        /// <param name="isAvailableForOrder"></param> 
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean SaveMappedPaymentOptionsNodeAvailability(Int32 deptProgramMappingID, List<Int32> paymentOptions, List<Int32> fileExtensions, Int32 currentUserId, Boolean isAvailableForOrder, Boolean isEmployment,
                                                                       Int32 tenantId, String splashPageUrl, String ExpirationFrequency, Int32? AfterExpirationFrequency,
                                                                       Int32? SubscriptionBeforeExpiry, Int32? SubscriptionAfterExpiry, Int32? SubscriptionExpiryFrequency,
                                                                       String IsAdminDataEntryAllow, Int32 paymentApprovalID, String OptionalCategorySetting, Int32? PDFInclusionID = null,
                                                                       Int32? resultsSentToApplicant = null, Int32? hierarchyNodeExemptedType = null, Boolean IsCallFromBkgHierarchySetup = false)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveMappedPaymentOptionsNodeAvailability(deptProgramMappingID, paymentOptions, fileExtensions, isAvailableForOrder, isEmployment, currentUserId, splashPageUrl,
                                                                                                                   ExpirationFrequency, AfterExpirationFrequency, SubscriptionBeforeExpiry, SubscriptionAfterExpiry, SubscriptionExpiryFrequency,
                                                                                                                   IsAdminDataEntryAllow, paymentApprovalID, OptionalCategorySetting, PDFInclusionID,
                                                                                                                    resultsSentToApplicant, hierarchyNodeExemptedType, IsCallFromBkgHierarchySetup);
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
        /// To get child nodes with Permission
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static ObjectResult<GetChildNodesWithPermission> GetChildNodesWithPermission(Int32 deptProgramMappingID, Int32? currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetChildNodesWithPermission(deptProgramMappingID, currentUserId);
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

        #endregion

        #region Price Adjustment
        /// <summary>
        /// Method to return all price adjustment of tenant.
        /// </summary>
        /// <returns>IQueryable</returns>
        public static IQueryable<PriceAdjustment> GetAllPriceAdjustment(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAllPriceAdjustment();
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
        /// Get the Price Adjustment by priceAdjustmentId
        /// </summary>
        /// <param name="priceAdjustmentId">priceAdjustmentId</param>
        /// <returns>PriceAdjustment</returns>
        public static PriceAdjustment GetPriceAdjustmentById(Int32 tenantId, Int32 priceAdjustmentId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPriceAdjustmentById(priceAdjustmentId);
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
        /// Save PriceAdjustment
        /// </summary>
        /// <param name="priceAdjustment">priceAdjustment</param>
        /// <returns>Boolean</returns>
        public static Boolean SavePriceAdjustment(Int32 tenantId, PriceAdjustment priceAdjustment)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SavePriceAdjustment(priceAdjustment);
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
        /// Update PriceAdjustment
        /// </summary>
        /// <returns>Boolean</returns>
        public static Boolean UpdatePriceAdjustment(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdatePriceAdjustment();
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
        /// Check Price Adjustment Mapping
        /// </summary>
        /// <param name="priceAdjustmentId">priceAdjustmentId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsPriceAdjustmentMapped(Int32 tenantId, Int32 priceAdjustmentId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsPriceAdjustmentMapped(priceAdjustmentId);
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

        #region InstitutionNodeType

        /// <summary>
        /// Method to return all price adjustment of tenant.
        /// </summary>
        /// <returns>IQueryable</returns>
        public static IQueryable<InstitutionNodeType> GetAllInstitutionNodeType(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAllInstitutionNodeType();
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
        /// Save InstitutionNodeType
        /// </summary>
        /// <param name="priceAdjustment">InstitutionNodeType</param>
        /// <returns>Boolean</returns>
        public static Boolean SaveInstitutionNodeType(Int32 tenantId, InstitutionNodeType institutionNodetype)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveInstitutionNodeType(institutionNodetype);
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
        /// Update InstitutionNodeType
        /// </summary>
        /// <returns>Boolean</returns>
        public static Boolean UpdateInstitutionNodeType(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateInstitutionNodeType();
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

        public static InstitutionNodeType GetInstitutionNodeTypeById(Int32 tenantId, Int32 institutionNodeTypetId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstitutionNodeTypeById(institutionNodeTypetId);
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
        /// Check Institution Node Type Mapping
        /// </summary>
        /// <param name="priceAdjustmentId">institutionNodeTypeId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsInstitutionNodeTypeMapped(Int32 tenantId, Int32 institutionNodeTypeId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsInstitutionNodeTypeMapped(institutionNodeTypeId);
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

        public static String GetLastInstitutionNodeTypeCode(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetLastInstitutionNodeTypeCode();
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
        /// To copy package structure and data
        /// </summary>
        /// <param name="compliancePackageID"></param>
        /// <param name="compliancePackageName"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void CopyPackageStructure(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 tenantId, Boolean updateExistingSub, Int32 srcNodeId, Int32 trgtNodeId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).CopyPackageStructure(compliancePackageID, compliancePackageName, currentUserId, updateExistingSub, srcNodeId, trgtNodeId);
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

        public static void CopyPackageStructureToMaster(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).CopyPackageStructureToMaster(compliancePackageID, compliancePackageName, currentUserId, tenantId);
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
        /// To copy package structure to client i.e. Assign package to client
        /// </summary>
        /// <param name="compliancePackageID"></param>
        /// <param name="compliancePackageName"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void CopyPackageStructureToClient(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).CopyPackageStructureToClient(compliancePackageID, compliancePackageName, currentUserId, tenantId);
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

        #region ManageUserGroup

        public static IQueryable<UserGroup> GetAllUserGroup(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAllUserGroup();
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

        public static String ArchiveUnArchiveUserGroups(Int32 tenantId, List<Int32> listUserGroupIds, bool isArchive)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).ArchiveUnArchiveUserGroups(listUserGroupIds, isArchive);
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

        public static List<UserGroup> GetAllUserGroupWithPermission(Int32 tenantId, Int32? currentUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAllUserGroupWithPermission(currentUserId);
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
        /// This method will get all user groups including archived for Create User Group screen
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static List<UserGroup> GetAllUserGroupWithPermissionAll(Int32 tenantId, Int32? currentUserId, String selectedHierarchyIds)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAllUserGroupWithPermissionAll(currentUserId, selectedHierarchyIds);
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
        /// Save UserGroup
        /// </summary>
        /// <param name="priceAdjustment">UserGroup</param>
        /// <returns>Boolean</returns>
        public static Boolean SaveUserGroup(Int32 tenantId, UserGroup userGroup)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveUserGroup(userGroup);
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
        public static UserGroup GetUserGroupById(Int32 tenantId, Int32 userGroupId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetUserGroupById(userGroupId);
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
        /// Update InstitutionNodeType
        /// </summary>
        /// <returns>Boolean</returns>
        public static Boolean UpdateUserGroup(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateUserGroup();
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
        /// Check User Group Mapping
        /// </summary>
        /// <param name="priceAdjustmentId">userGroupId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsUserGroupMapped(Int32 tenantId, Int32 userGroupId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsUserGroupMapped(userGroupId);
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

        #region Map User Permission

        public static IQueryable<vwHierarchyPermission> GetHierarchyPermissionList(Int32 tenantId, Int32 hierarchyId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetHierarchyPermissionList(hierarchyId);
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
        /// Filter Compliance Package  on selection of Hierarchy Institution
        /// Filter Compliance Package on basis of Client Admin Permission
        /// </summary>
        /// <param name="dpm_Ids"></param>
        /// <param name="OrganisationUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<CompliancePackage> GetCompliancePackagesByPermission(String dpm_Ids, Int32 OrganisationUserId, Int32 tenantId, Boolean IsAdminLoggedIn)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCompliancePackagesByPermissionList(tenantId, dpm_Ids, OrganisationUserId, IsAdminLoggedIn);
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

        public static List<ComplaincePackageDetails> GetPermittedPackagesByUserID(Int32 tenantId, Int32? orgUserId = null)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPermittedPackagesByUserID(orgUserId);
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

        public static List<ComplianceCategoryDetails> GetPermittedCategoriesByUserID(Int32 tenantId, Int32? orgUserId = null)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPermittedCategoriesByUserID(orgUserId);
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

        public static IQueryable<lkpPermission> GetPermissionList(Int32 tenantId, Boolean onlyPackagePermission = false)
        {
            try
            {
                //UAT 2834 Get Package Permission only for compliance hierarchy
                if (onlyPackagePermission)
                {
                    return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPermission>(tenantId).Where(cond => cond.PER_IsDeleted == false && (cond.PER_Code.Equals(LkpPermission.AdministrativePackagePermission.GetStringValue()) ||
                        cond.PER_Code.Equals(LkpPermission.ImmunizationPackagePermission.GetStringValue())
                         || cond.PER_Code.Equals(LkpPermission.BothPackagePermission.GetStringValue()))).AsQueryable();
                }
                else
                {
                    return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPermission>(tenantId).Where(cond => cond.PER_IsDeleted == false && !cond.PER_Code.Equals(LkpPermission.AdministrativePackagePermission.GetStringValue()) &&
                        !cond.PER_Code.Equals(LkpPermission.ImmunizationPackagePermission.GetStringValue())
                         && !cond.PER_Code.Equals(LkpPermission.BothPackagePermission.GetStringValue())).AsQueryable();
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

        public static Boolean SaveHierarchyPermission(Int32 tenantId, HierarchyPermission hierarchyPermission, List<String> lstHierarchyPermissionTypeCode)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveHierarchyPermission(hierarchyPermission, lstHierarchyPermissionTypeCode);
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

        public static HierarchyPermission GetHierarchyPermissionByID(Int32 tenantId, Int32 hierarchyPermissionID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetHierarchyPermissionByID(hierarchyPermissionID);
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

        //public static Boolean UpdateHierarchyPermission(Int32 tenantId, HierarchyPermission hierarchyPermission, List<String> lstHierarchyPermissionTypeCode, Int32 hierarchyPermissionID)
        public static Boolean UpdateHierarchyPermission(Int32 tenantId)
        {
            try
            {
                //return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateHierarchyPermission(hierarchyPermission, lstHierarchyPermissionTypeCode, hierarchyPermissionID);
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateHierarchyPermission();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteHierarchyPermission(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteHierarchyPermission();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region  ManageComplianceAttributeGroup
        public static IQueryable<ComplianceAttributeGroup> GetAllComplianceAttributeGroup(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAllComplianceAttributeGroup();
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
        /// Save Attribute Group
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="ComplianceAttributeGroup"></param>
        /// <returns></returns>
        public static Boolean SaveAttributeGroup(Int32 tenantId, ComplianceAttributeGroup attributeGroup)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveAttributeGroup(attributeGroup);
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
        public static ComplianceAttributeGroup GetAttributeGroupById(Int32 tenantId, Int32 attributeGroupId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAttributeGroupById(attributeGroupId);
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
        /// Update ComplianceAttributeGroup
        /// </summary>
        /// <returns>Boolean</returns>
        public static Boolean UpdateAttributeGroup(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateAttributeGroup();
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
        /// Check AttributeGroup Mapping
        /// </summary>
        /// <param name="attributeGroupId">attributeGroupId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsAttributeGroupMapped(Int32 tenantId, Int32 attributeGroupId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsAttributeGroupMapped(attributeGroupId);
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

        public static InstitutionWebPage GeDateHelpHtmlFromtWebSiteWebPage(Int32 tenantID, Int32 packageID, String recordTypeCode, String websiteWebPageTypeCode)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GeDateHelpHtmlFromtWebSiteWebPage(tenantID, packageID, recordTypeCode, websiteWebPageTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region ApplicantDataAuditHistory
        /// <summary>
        /// Gets the most recent PackageSubscription Details for a Applicant.
        /// </summary>
        /// <returns>PackageSubscription</returns>
        public static List<ApplicantDataAuditHistory> GetApplicantDataAuditHistory(Int32 tenantId, CustomPagingArgsContract gridCustomPaging, SearchItemDataContract searchItemDataContract)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetApplicantDataAuditHistory(gridCustomPaging, searchItemDataContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Data Entry Help
        /// <summary>
        /// Gets Data Entry Help Content.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static InstitutionWebPage GetDataEntryHelpContentByPackageId(Int32 tenantId, Int32? recordId, String recordType, String webSiteWebPageType)
        {
            try
            {
                //String websiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDataEntryHelpContentByPackageId(tenantId, recordId, recordType, webSiteWebPageType);
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
        /// Get WebsiteWebPageType Id by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Int32 GetWebsiteWebPageTypeIdByCode(String code, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpWebsiteWebPageType>(tenantId).Where(cond => cond.Code == code && !cond.IsDeleted).FirstOrDefault().WebsiteWebPageTypeID;
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
        /// Get RecordType id by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Int16 GetRecordTypeIdByCode(String code, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRecordType>(tenantId).Where(cond => cond.Code == code && !cond.IsDeleted).FirstOrDefault().RecordTypeID;
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
        /// Get existing package id list.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        public static List<Int32> GetExistingPackageIdList(Int32 tenantId, String recordType, String webSiteWebPageType)
        {
            try
            {
                List<Int32?> tempExistingPackageIdList = new List<Int32?>();
                tempExistingPackageIdList = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetExistingPackageIdList(tenantId, recordType, webSiteWebPageType);
                //set -1 for null because null record Id is saved in db for default value.
                return tempExistingPackageIdList.Select(cond => cond.HasValue ? cond.Value : -1).ToList();
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

        public static Boolean SaveWebSitePage(InstitutionWebPage institutionWebPage, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveInstitutionWebPage(institutionWebPage);
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
        ///Update the InstitutionWebPage table
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        public static Boolean UpdateWebPageHtml(InstitutionWebPage institutionWebPage, Int32 tenantId)
        {
            return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateInstitutionWebPage(institutionWebPage);
        }

        /// <summary>
        /// Gets web site web page
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        public static InstitutionWebPage GetInstitutionWebPage(Int32 InstitutionWebPageID, Int32 tenantId)
        {
            return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstitutionWebPage(InstitutionWebPageID);
        }

        #endregion


        #region Manage Package Subscription

        /// <summary>
        /// Gets the list of packages for the current applicant for data entry form
        /// </summary>
        /// <param name="organisationUserID"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<CompliancePackage> GetClientCompliancePackageByClient(Int32 organisationUserID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetClientCompliancePackageByClient(organisationUserID);
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
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="clientCompliancePackageID"></param>
        /// <returns></returns>
        public static CompliancePackage GetClientCompliancePackageByPackageID(Int32 tenantID, Int32 clientCompliancePackageID, Boolean clearContext = false)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetClientCompliancePackageByPackageID(clientCompliancePackageID, clearContext);
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

        #region DISCLOSURE DOCUMENT
        public static Boolean SaveDisclosureDocument(SystemDocument systemDocument, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveDisclosureDocument(systemDocument);
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

        public static SystemDocument GetDisclosureDocument(Int32 tenantId, Int32 systemDocumentId)
        {
            try
            {

                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDisclosureDocument(systemDocumentId);
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

        public static Boolean UpdateChanges(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateChanges();
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
        /// Get Attached disclosure form list 
        /// </summary>
        /// <param name="websiteId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static List<DisclosureDocument> GetAttachedDisclosureFormList(Int32 tenantId, String recordType, String webSiteWebPageType, CustomPagingArgsContract queueAuditArgsContractn)
        {
            try
            {
                DataTable table = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAttachedDisclosureFormList(tenantId, recordType, webSiteWebPageType, queueAuditArgsContractn);
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new DisclosureDocument
                {
                    InstitutionWebPageID = x["InstitutionWebsiteWebPageID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["InstitutionWebsiteWebPageID"]),
                    PackageName = x["PackageName"].ToString(),
                    DocumentName = x["DocumentName"].ToString(),
                    TotalCount = Convert.ToInt32(x["TotalCount"]),

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

        #region Esigned Documents
        public static List<lkpDocumentType> GetDocumentType(Int32 tenantID)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpDocumentType>(tenantID);
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

        public static ApplicantDocument SaveEsignedDocumentAsPdf(Int32 tenantID, String PdfPath, String filename, Int32 fileSize, String documentTypeCode, Int32 currentLoggedInUseID, Int32 orgUserID)
        {
            try
            {
                List<lkpDocumentType> lkpDocumentType = GetDocumentType(tenantID);
                Int32 documentTypeId = lkpDocumentType.Where(cond => cond.DMT_Code == documentTypeCode && !cond.DMT_IsDeleted).FirstOrDefault().DMT_ID;
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).SaveEsignedDocumentAsPdf(PdfPath, filename, fileSize, documentTypeId, currentLoggedInUseID, orgUserID);
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

        public static byte[] FillSignatureInDisClaimerPDFDocument(byte[] pdfDocumentDataToBeFilledIn, byte[] imageToAddToDocument, List<SysDocumentFieldMappingContract> LstSpecialFields = null)
        {
            byte[] signedDocument = null;

            try
            {
                PdfReader reader = new PdfReader(pdfDocumentDataToBeFilledIn);
                MemoryStream ms = new MemoryStream();
                PdfStamper stamper = new PdfStamper(reader, ms);
                AcroFields.FieldPosition signatureImagePosition = null;

                //Fill-in the form values
                AcroFields af = stamper.AcroFields;

                af.SetField("checkBoxAgree", "Yes");
                stamper.FormFlattening = true;
                float left = 0;
                float right = 0;
                float top = 0;
                float heigth = 0;


                if (LstSpecialFields.IsNotNull())
                {
                    foreach (var item in LstSpecialFields)
                    {
                        if (item.SpecialFieldTypeCode == "AAAC")
                        {
                            af.SetField(item.FieldName, item.FieldValue);
                        }
                        else if (item.SpecialFieldTypeCode == "AAAD")
                        {
                            //Setup signature
                            try { signatureImagePosition = af.GetFieldPositions(item.FieldName)[0]; }
                            catch { }

                            if (signatureImagePosition != null && imageToAddToDocument != null)
                            {
                                left = signatureImagePosition.position.Left;
                                right = signatureImagePosition.position.Right;
                                top = signatureImagePosition.position.Top;
                                heigth = signatureImagePosition.position.Height;

                                iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(imageToAddToDocument);
                                //PdfContentByte contentByte = stamper.GetOverContent(1);
                                PdfContentByte contentByte = stamper.GetOverContent(signatureImagePosition.page); // uat - 856 : WB: Signature on disclosure is not placing properly when there are multiple pages in the document
                                float currentImageHeigth = 0;
                                currentImageHeigth = signatureImage.Height;
                                float ratio = 0;
                                ratio = heigth / currentImageHeigth;
                                float width = signatureImage.Width * ratio;
                                signatureImage.ScaleAbsoluteHeight(heigth);
                                signatureImage.ScaleAbsoluteWidth(width);
                                signatureImage.SetAbsolutePosition(left, top - signatureImage.ScaledHeight);
                                contentByte.AddImage(signatureImage);
                            }
                        }
                    }
                }
                else
                {
                    //Used to fill the Current Date in Disclosure and Disclaimer PDF file.
                    String currentDate = DateTime.Now.ToString("MM/dd/yyyy");
                    if (af.GetField("DateSigned").IsNotNull())
                    {
                        af.SetField("DateSigned", currentDate);
                    }

                    try { signatureImagePosition = af.GetFieldPositions("SignatureImage")[0]; }
                    catch { }

                    if (signatureImagePosition != null && imageToAddToDocument != null)
                    {
                        left = signatureImagePosition.position.Left;
                        right = signatureImagePosition.position.Right;
                        top = signatureImagePosition.position.Top;
                        heigth = signatureImagePosition.position.Height;

                        iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(imageToAddToDocument);
                        //PdfContentByte contentByte = stamper.GetOverContent(1);
                        PdfContentByte contentByte = stamper.GetOverContent(signatureImagePosition.page); // uat - 856 : WB: Signature on disclosure is not placing properly when there are multiple pages in the document
                        float currentImageHeigth = 0;
                        currentImageHeigth = signatureImage.Height;
                        float ratio = 0;
                        ratio = heigth / currentImageHeigth;
                        float width = signatureImage.Width * ratio;
                        signatureImage.ScaleAbsoluteHeight(heigth);
                        signatureImage.ScaleAbsoluteWidth(width);
                        signatureImage.SetAbsolutePosition(left, top - signatureImage.ScaledHeight);
                        contentByte.AddImage(signatureImage);
                    }
                }

                stamper.Close();
                signedDocument = ms.ToArray();
                ms.Close();

                //Recompress final document to further shrink.
                signedDocument = CompressPDFDocument(signedDocument);
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
            return signedDocument;
        }

        public static byte[] CompressPDFDocument(byte[] signedDocument)
        {
            try
            {
                PdfReader compressionReader = new PdfReader(signedDocument);
                MemoryStream compressionsMS = new MemoryStream();
                PdfStamper compressionStamper = new PdfStamper(compressionReader, compressionsMS);
                compressionStamper.FormFlattening = true;
                compressionStamper.SetFullCompression();
                compressionStamper.Close();
                signedDocument = compressionsMS.ToArray();
                compressionsMS.Close();
                compressionReader.Close();
                return signedDocument;
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

        public static ApplicantDocument GetESignedeDocument(Int32 tenantId, Int32 applicantDocumentId, String documentTypeCode)
        {
            try
            {
                Int32 documentTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantId).Where(x => x.DMT_Code == documentTypeCode && !x.DMT_IsDeleted).FirstOrDefault().DMT_ID;
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetESignedeDocument(applicantDocumentId, documentTypeId);
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

        #region GET SURVEY MONKEY LINK

        /// <summary>
        ///  Get Survey Monkey Link 
        /// </summary>
        public static String GetSurveyMonkeyLink(Int32 tenantId, Int32 applicantId, String subEventCode, String packageName, String categoryName, String itemName, Int32 itemId, Int32 packageId, Int32 categoryId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetSurveyMonkeyLink(tenantId, applicantId, subEventCode, packageName, categoryName, itemName, itemId, packageId, categoryId);

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

        #region Node Deadline and Notifications

        /// <summary>
        /// To get node deadlines
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IQueryable<NodeDeadline> GetNodeDeadlines(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetNodeDeadlines();
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
        /// To get node deadlines
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<Int32> GetCheckedUsergroupIds(Int32 mappingId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCheckedUsergroupIds(mappingId);
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
        /// To get node deadline by ID
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="NodeDeadlineID"></param>
        /// <returns></returns>
        public static NodeDeadline GetNodeDeadlineByID(Int32 tenantId, Int32 NodeDeadlineID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetNodeDeadlineByID(NodeDeadlineID);
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
        /// To save/insert Node Deadline
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nodeDeadline"></param>
        /// <returns></returns>
        public static Boolean SaveNodeDeadline(Int32 tenantId, NodeDeadline nodeDeadline)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveNodeDeadline(nodeDeadline);
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
        /// To update Node Deadline
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean UpdateNodeDeadline(Int32 tenantId, Int32 nodeDeadlineId, NodeNotificationSettingsContract nodeNotificationSettingsContract,
                                        List<Int32> userGroupIDs, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdateNodeDeadline(nodeDeadlineId, nodeNotificationSettingsContract, userGroupIDs, currentUserId);
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
        /// To delete Node Deadline
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nodeDeadline"></param>
        /// <returns></returns>
        public static Boolean DeleteNodeDeadline(Int32 tenantId, Int32 nodeNotificationMappingId, Int32 loggedInUserId, DateTime modifiedOn)
        {
            try
            {
                //To delete Communication Template
                CommunicationManager.DeleteCommunicationTemplates(tenantId, nodeNotificationMappingId, loggedInUserId, modifiedOn);
                //To delete Node Deadline and Node mappings
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteNodeDeadline();
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
        /// To save Nag Email Notifications
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nagEmailNotificationTypeId"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <param name="nagFrequency"></param>
        /// <param name="currentUserId"></param>
        public static Boolean SaveNagEmailNotifications(Int32 tenantId, Int16 nagEmailNotificationTypeId, Int32 hierarchyNodeID, Int32? nagFrequency, Int32 currentUserId, Boolean isActive)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveNagEmailNotifications(tenantId, nagEmailNotificationTypeId, hierarchyNodeID, nagFrequency, currentUserId, isActive);
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
        /// To get Node Notification Mapping by NodeID 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nagEmailNotificationTypeId"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <returns></returns>
        public static NodeNotificationMapping GetNodeNotificationMappingByNodeID(Int32 tenantId, Int16 nagEmailNotificationTypeId, Int32 hierarchyNodeID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetNodeNotificationMappingByNodeID(nagEmailNotificationTypeId, hierarchyNodeID);
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

        #region Backround Package Detials

        /// <summary>
        /// Get list of Bkg Package Detail Tree
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="bkgPackageId">bkgPackageId</param>
        /// <returns>List<GetBkgPackageDetailTree></returns>
        public static List<Entity.ClientEntity.GetBkgPackageDetailTree> GetBkgPackageDetailTree(Int32 tenantId, Int32 bkgPackageId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetBkgPackageDetailTree(bkgPackageId);
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

        public static String GetDeptProgMappingLabel(Int32 NodeId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDeptProgMappingLabel(NodeId);
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

        public static String GetBkgpackageOfNode(Int32 BkgPackageHierarchyMappingID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetBkgpackageOfNode(BkgPackageHierarchyMappingID);
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

        public static String GetCompliancePackafeOfNode(Int32 DeptProgramPackageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCompliancePackafeOfNode(DeptProgramPackageId);
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

        public static SystemDocument GetServiceFormDocument(Int32 tenantId, Int32 systemDocumentId)
        {
            try
            {

                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetServiceFormDocument(systemDocumentId);
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

        public static List<CommunicationCCUsersList> GetCCusers(Int32 communicationSubEventId, Int32 tenantId, Int32? hierarchyNodeID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCCusers(communicationSubEventId, tenantId, hierarchyNodeID.ToString());
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

        #region Dissociation Work

        public static String GetCategoryDissociationStatus(Int32 tenantId, Int32 categoryId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCategoryDissociationStatus(categoryId, packageId);
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

        public static String GetItemDissociationStatus(Int32 tenantId, Int32 packageId, Int32 categoryId, Int32 itemId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetItemDissociationStatus(packageId, categoryId, itemId);
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

        public static String GetAttributeDissociationStatus(Int32 tenantId, Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attrId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAttributeDissociationStatus(packageId, categoryId, itemId, attrId);
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

        public static Int32 DissociateCategory(Int32 tenantId, Int32 categoryId, String packageIds, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DissociateCategory(tenantId, categoryId, packageIds, currentLoggedInUserId);
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

        public static Int32 DissociateItem(Int32 tenantId, Int32 packageId, String categoryId, Int32 itemId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DissociateItem(tenantId, packageId, categoryId, itemId, currentLoggedInUserId);
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
        public static bool IsAllowedOverrideDate(Int32 ItemId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsAllowedOverrideDate(ItemId);
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

        public static Int32 DissociateAttribute(Int32 tenantId, Int32 packageId, Int32 categoryId, String itemIds, Int32 attrId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DissociateAttribute(tenantId, packageId, categoryId, itemIds, attrId, currentLoggedInUserId);
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

        //UAT-2582
        public static List<CompliancePackage> GetCompliancePackagesAssociatedtoCat(Int32 tenantID, Int32 categoryID, Int32 CurrentPackageID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetCompliancePackagesAssociatedtoCat(categoryID, CurrentPackageID);
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

        //UAT-2582
        public static List<ComplianceCategory> GetComplianceCategoriesAssociatedtoItem(Int32 tenantID, Int32 itemId, Int32 currentCategoryID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetComplianceCategoriesAssociatedtoItem(itemId, currentCategoryID);
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

        public static List<ComplianceItem> GetComplianceItemAssociatedtoAttributes(Int32 tenantID, Int32 itemId, Int32 currentCategoryID,Int32 currentAttributeID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetComplianceItemsAssociatedtoAttributes(itemId, currentCategoryID,currentAttributeID);
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

        #region Instruction Text
        public static Boolean SaveInstructionText(ComplianceAttributeContract complianceAttributeContract, int loggedInUserId, int tenantId, ComplianceItemAttribute itemAttributeMapping)
        {
            try
            {
                complianceAttributeContract.AssignmentHierarchyID = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAssignmentHierarchyID(complianceAttributeContract.PackageID.Value, complianceAttributeContract.CatagoryID.Value, itemAttributeMapping.CIA_ItemID, itemAttributeMapping.CIA_AttributeID);
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveInstructionText(complianceAttributeContract, loggedInUserId);
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

        public static int GetItemAttributeMappingID(ComplianceItemAttribute itemAttributeMapping, int tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetItemAttributeMappingID(itemAttributeMapping);
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

        public static bool UpdateInstructionText(ComplianceAttributeContract complianceAttributeContract, Int32 AttrID, Int32 ItemId, Int32 CategoryId, Int32 PackageId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(complianceAttributeContract.TenantID).UpdateInstructionText(complianceAttributeContract, AttrID, ItemId, CategoryId, PackageId);
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

        public static String GetAttributeInstructionText(ComplianceAttributeContract complianceAttributeContract, int AttrID, int ItemId, int CategoryId, int PackageId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(complianceAttributeContract.TenantID).GetAttributeInstructionText(complianceAttributeContract, AttrID, ItemId, CategoryId, PackageId);
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

        /// <summary>
        /// To Set Compliance Package Availability For Order
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="availability"></param>
        /// <returns></returns>
        public static Boolean SetCompliancePkgAvailabilityForOrder(Int32 deptProgramPackageID, Int32 currentUserId, Int32 tenantId, Boolean availability)
        {
            try
            {
                List<lkpPackageAvailability> pkgAvailability = GetPackageAvailablity(tenantId);
                Int32 paID = 0;
                String code = String.Empty;
                if (availability)
                {
                    code = PackageAvailability.AVAILABLE_FOR_ORDER.GetStringValue();
                    paID = pkgAvailability.FirstOrDefault(x => x.PA_Code == code).PA_ID;
                }
                else
                {
                    code = PackageAvailability.NOT_AVAILABLE_FOR_ORDER.GetStringValue();
                    paID = pkgAvailability.FirstOrDefault(x => x.PA_Code == code).PA_ID;
                }

                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SetCompliancePkgAvailabilityForOrder(deptProgramPackageID, currentUserId, paID);
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
        /// To Get whether Compliance Package Available For Order or not
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <returns></returns>
        public static Boolean IsCompliancePkgAvailableForOrder(Int32 deptProgramPackageID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsCompliancePkgAvailableForOrder(deptProgramPackageID);
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
        /// To Get whether Compliance Package Available For Order or not
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <returns></returns>
        public static List<lkpPackageAvailability> GetPackageAvailablity(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPackageAvailability>(tenantId).Where(x => x.PA_IsDeleted == false).ToList();
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

        #region Parallel Task For Executing Post Submit Rules
        /// <summary>
        /// Execute the Post Submit Rules For Multi using Parallel task.
        /// </summary>
        /// <param name="ruleXml"></param>
        /// <param name="tenantId"></param>
        /// <param name="currentloggedInUserId"></param>
        public static void RunParallelTaskForPostSubmitRuleMulti(String ruleXml, Int32 tenantId, Int32 currentloggedInUserId)
        {
            Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
            dataDict.Add("ruleXml", ruleXml);
            dataDict.Add("tenantId", tenantId);
            dataDict.Add("currentLoggedInUserId", currentloggedInUserId);
            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            ParallelTaskContext.PerformParallelTask(ExecutePostSubmitRulesForMulti, dataDict, LoggerService, ExceptiomService);
        }

        private static void ExecutePostSubmitRulesForMulti(Dictionary<String, Object> data)
        {
            String rulexml = Convert.ToString(data.GetValue("ruleXml"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("currentLoggedInUserId"));
            StoredProcedureManagers.EvaluatePostSubmitRulesForMulti(rulexml, currentLoggedInUserId, tenantId);
        }

        #endregion

        /// <summary>
        /// To Get whether to do Auto Renew Invoice Order For Package 
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <returns></returns>
        public static Boolean IsAutoRenewInvoiceOrderForPackage(Int32 deptProgramPackageID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsAutoRenewInvoiceOrderForPackage(deptProgramPackageID);
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
        /// To Set whether to do Auto Renew Invoice Order For Package 
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <returns></returns>
        public static Boolean SetAutoRenewInvoiceOrderForPackage(Int32 deptProgramPackageID, Int32 tenantId, Int32 currentUserId, Boolean IsAutoRenewInvoiceOrder)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SetAutoRenewInvoiceOrderForPackage(deptProgramPackageID, currentUserId, IsAutoRenewInvoiceOrder);
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
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="orgUserID"></param>
        /// <returns></returns>
        public static List<UserNodePermissionsContract> GetUserNodePermissionForVerificationAndProfile(Int32 tenantID, Int32 orgUserID)
        {
            try
            {
                IEnumerable<DataRow> rows = BALUtils.GetComplianceSetupRepoInstance(tenantID).GetUserNodePermissionForVerificationAndProfile(orgUserID).AsEnumerable();
                return rows.Select(col => new UserNodePermissionsContract
                {
                    DPM_ID = col["DPM_ID"] == DBNull.Value ? -1 : Convert.ToInt32(col["DPM_ID"]),
                    PermissionID = col["PermissionID"] == DBNull.Value ? -1 : Convert.ToInt32(col["PermissionID"]),
                    ProfilePermissionID = col["ProfilePermissionID"] == DBNull.Value ? -1 : Convert.ToInt32(col["ProfilePermissionID"]),
                    VerificationPermissionID = col["VerificationPermissionID"] == DBNull.Value ? -1 : Convert.ToInt32(col["VerificationPermissionID"]),
                    PermissionCode = col["PermissionCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["PermissionCode"]),
                    ProfilePermissionCode = col["ProfilePermissionCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["ProfilePermissionCode"]),
                    VerificationPermissionCode = col["VerificationPermissionCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["VerificationPermissionCode"]),
                    ParentNodeID = col["ParentNodeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["ParentNodeID"])
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

        #region UAT-422
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DMP_ID"></param>
        /// <param name="archivalGracePeriod"></param>
        /// <param name="currentUserID"></param>
        /// <param name="tenandID"></param>
        public static Boolean SaveArchivalGracePeriod(Int32 DPM_ID, Int32? archivalGracePeriod, Int32 currentUserID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).SaveArchivalGracePeriod(DPM_ID, archivalGracePeriod, currentUserID);
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

        public static Dictionary<String, Int32> GetEffectiveArchivalGracePeriod(Int32 DPM_ID, Int32 currentUserID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetEffectiveArchivalGracePeriod(DPM_ID, currentUserID);
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


        public static Boolean SaveRecieptDocument(Int32 tenantID, String pdfDocPath, String filename, Int32 fileSize, String documentTypeCode, Int32 CurrentLoggedInUserID, Int32 OrderID, Int32 orgUserID)
        {
            try
            {
                List<lkpDocumentType> lkpDocumentType = GetDocumentType(tenantID);
                Int32 documentTypeId = lkpDocumentType.Where(cond => cond.DMT_Code == documentTypeCode && !cond.DMT_IsDeleted).FirstOrDefault().DMT_ID;
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).SaveRecieptDocument(pdfDocPath, filename, fileSize, documentTypeId, CurrentLoggedInUserID, OrderID, orgUserID);
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

        public static bool IsReciptAlreadySaved(int tenantID, int orderID)
        {
            try
            {
                Order _order = BALUtils.GetComplianceSetupRepoInstance(tenantID).GetOrderFromOrderID(orderID);
                if (_order.IsNotNull() && _order.ApplicantDocumentID.IsNull())
                {
                    return false; //Doc is not saved earlier
                }
                else
                {
                    return true;
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

        public static ApplicantDocument GetRecieptDocumentDataForOrderID(int tenantID, int orderID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetRecieptDocumentDataForOrderID(orderID);
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

        public static List<lkpNodeCopySetting> GetNodeCopySettings(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpNodeCopySetting>(tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static List<CommunicationCopySettingsOverrideContract> GetCommunicationCopySettingsOverride(Int32 tenantID)
        {
            try
            {
                IEnumerable<DataRow> rows = BALUtils.GetComplianceSetupRepoInstance(tenantID).GetCommunicationCopySettingsOverride().AsEnumerable();
                return rows.Select(col => new CommunicationCopySettingsOverrideContract
                {
                    CommunicationNodeCopySettingID = Convert.ToInt32(col["CommunicationNodeCopySettingID"]),
                    Email = col["Email"] == DBNull.Value ? String.Empty : Convert.ToString(col["Email"]),
                    HierarchyLabel = col["HierarchyLabel"] == DBNull.Value ? String.Empty : Convert.ToString(col["HierarchyLabel"]),
                    HierarchyNodeID = col["HierarchyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(col["HierarchyNodeID"]),
                    NodeCopySettingCode = col["NodeCopySettingCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["NodeCopySettingCode"]),
                    NodeCopySettingID = col["NodeCopySettingID"] == DBNull.Value ? 0 : Convert.ToInt32(col["NodeCopySettingID"]),
                    NodeCopySettingName = col["NodeCopySettingName"] == DBNull.Value ? String.Empty : Convert.ToString(col["NodeCopySettingName"]),
                    OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                    UserName = col["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(col["UserName"])
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

        public static Boolean CheckIfCommunicationNodeSettingExistForSelectednode(Int32 hierarchyNodeId, Int32 orgUserId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).CheckIfCommunicationNodeSettingExistForSelectednode(hierarchyNodeId, orgUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveCommunicationNodeCopySetting(CommunicationNodeCopySetting communicationNodeCopySetting, Int32 tenantID, List<CommunicationSettingsSubEventsContract> communicationSettingsSubEventsContractList)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).SaveCommunicationNodeCopySetting(communicationNodeCopySetting, communicationSettingsSubEventsContractList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateCommunicationNodeCopySetting(Int32 communicationNodeCopySettingID, Int32 nodeCopySettingID, Int32 currentLoggedInUserID, Int32 tenantID, List<CommunicationSettingsSubEventsContract> communicationSettingsSubEventsContractList)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).UpdateCommunicationNodeCopySetting(communicationNodeCopySettingID, nodeCopySettingID, currentLoggedInUserID, communicationSettingsSubEventsContractList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteCommunicationNodeCopySetting(Int32 communicationNodeCopySettingID, Int32 currentLoggedInUserID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).DeleteCommunicationNodeCopySetting(communicationNodeCopySettingID, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetFormattedString(Int32 orgUserID, Boolean isOrgUserProfileID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetFormattedString(orgUserID, isOrgUserProfileID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1185 New Compliance Package Type
        public static List<lkpCompliancePackageType> GetCompliancePackageTypes(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetCompliancePackageTypes();
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

        #region UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
        public static List<String> GetExplanatoryNotesForItems(List<ComplianceItem> objectData, String objectTypeCode, String contentTypeCode, Int32 tenantId)
        {
            try
            {
                Int32 contentTypeId = LookupManager.GetLookUpData<lkpLargeContentType>(tenantId).Where(obj => obj.LCT_Code == contentTypeCode && obj.LCT_IsDeleted == false).FirstOrDefault().LCT_ID;
                Int32 objectTypeId = LookupManager.GetLookUpData<lkpObjectType>(tenantId).Where(obj => obj.OT_Code == objectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
                List<Int32> objectIds = objectData.Select(slct => slct.ComplianceItemID).ToList();
                List<LargeContent> lstExpNotes = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetExplanatoryNotesForItems(objectIds, objectTypeId, contentTypeId);
                //List<ComplianceItemsContract> expalanatoryNotesItems = new List<ComplianceItemsContract>();
                List<String> lstExplanatoryNote = new List<String>();

                objectData.ForEach(compItem =>
                {
                    //ComplianceItemsContract compItemExpNoteData = new ComplianceItemsContract();
                    //compItemExpNoteData.ComplianceItemId = compItem.ComplianceItemID;
                    //compItemExpNoteData.Name = compItem.Name;
                    //compItemExpNoteData.ItemLabel = compItem.ItemLabel;

                    String ExplanatoryNotes = lstExpNotes.FirstOrDefault(x => x.LC_ObjectID == compItem.ComplianceItemID).IsNotNull() ?
                                                           lstExpNotes.FirstOrDefault(x => x.LC_ObjectID == compItem.ComplianceItemID).LC_Content : String.Empty;
                    lstExplanatoryNote.Add(String.Format("<span class='expl-title expNotes'>{0}</span><span class='expl-dur expNotes'>: </span>{1}", compItem.ItemLabel.IsNullOrEmpty() ? compItem.Name : compItem.ItemLabel,
                                  ExplanatoryNotes));
                    //expalanatoryNotesItems.Add(compItemExpNoteData);
                });

                return lstExplanatoryNote;
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


        public static void InsertSystemSeriveTrigger(Int32 currentUserId, Int32 tenantId, String systemServiceCode)
        {
            try
            {
                Entity.lkpSystemService reOccurRuleService = SecurityManager.GetSystemServiceByCode(systemServiceCode);
                Entity.SystemServiceTrigger systemServiceTrigger = new Entity.SystemServiceTrigger();
                if (reOccurRuleService != null)
                    systemServiceTrigger.SST_SystemServiceID = reOccurRuleService.SS_ID;
                systemServiceTrigger.SST_TenantID = tenantId;
                systemServiceTrigger.SST_IsActive = true;
                systemServiceTrigger.SST_CreatedByID = currentUserId;
                systemServiceTrigger.SST_CreatedOn = DateTime.Now;
                SecurityManager.AddSystemServiceTrigger(systemServiceTrigger);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<CompliancePackageCategory> GetCategoriesRqdForComplianceAction(Int32 tenantId)
        {
            try
            {
                DataTable categoriesRqdForComplianceAction = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCategoriesRqdForComplianceAction();

                IEnumerable<DataRow> rows = categoriesRqdForComplianceAction.AsEnumerable();
                return rows.Select(col => new CompliancePackageCategory
                {
                    CPC_ID = col["CPC_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["CPC_ID"]),
                    CPC_ComplianceRequired = col["CPC_ComplianceRequired"] == DBNull.Value ? false : Convert.ToBoolean(col["CPC_ComplianceRequired"]),
                    CPC_PackageID = col["CPC_PackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["CPC_PackageID"]),
                    CPC_CategoryID = col["CPC_CategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["CPC_CategoryID"])
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

        public static void ProcessOptionalCategory(Int32 currentUserId, Int32 tenantId, CompliancePackageCategory compliancePackageCategory)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).ProcessOptionalCategory(compliancePackageCategory.CPC_PackageID, compliancePackageCategory.CPC_CategoryID, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void ProcessRequiredCategory(Int32 currentUserId, Int32 tenantId, CompliancePackageCategory compliancePackageCategory)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).ProcessRequiredCategory(compliancePackageCategory.CPC_PackageID, compliancePackageCategory.CPC_CategoryID, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static InstitutionConfigurationDetailsContract GetInstitutionConfigurationDetails(Int32 hierarchyNodeID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetInstitutionConfigurationDetails(hierarchyNodeID);
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

        public static ScreeningDetailsForConfigurationContract GetScreeningDetailsForInstitutionConfiguration(Int32 hierarchyNodeID, Int32 packageID, Int32 tenantID, Int32 packageHierarchyNodeID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetScreeningDetailsForInstitutionConfiguration(hierarchyNodeID, packageID, packageHierarchyNodeID);
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

        // /// <summary>
        /// Method is used to get compliance package Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>

        public static CompliancePkgDetailContract GetCompliancePkgDetails(Int32 hierarchyNodeID, Int32 packageId, Int32 tenantID, Int32 packageHierarchyID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetCompliancePkgDetails(hierarchyNodeID, packageId, packageHierarchyID);
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

        #region GETTING INSTITUTION HIERARCHY LIST FOR COMMON SCREENS
        public static ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreeCommon(Int32 tenantID, Int32? currentUserID, string IsRequestFromAddRotationScreen)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetInstituteHierarchyTreeCommon(currentUserID, IsRequestFromAddRotationScreen);
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

        /// <summary>
        /// Returns whether the Compliance is Required for given settings - UAT 1543
        /// </summary>
        /// <param name="isComplianceReq"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static Boolean GetComplianceRqdByDateRange(Boolean isComplianceReq, DateTime? startDate, DateTime? endDate, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceRqdByDateRange(isComplianceReq, startDate, endDate);
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

        public static List<ClientSystemDocument> GetComplianceViewDocumentSysDocs(Int32 tenantID)
        {
            try
            {
                Int32 docTypeID;
                if (tenantID == SecurityManager.DefaultTenantID)
                {
                    return new List<ClientSystemDocument>();
                }
                else
                {
                    docTypeID = GetDocumentTypeIDByCode(DocumentType.COMPLIANCE_VIEW_DOCUMENT.GetStringValue(), tenantID);
                    return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetClientSystemDocumentListByDocTypeID(docTypeID);
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

        /// <summary>
        /// Genric Method to Get DocumentTypeID By DocumentTypeCode
        /// </summary>
        /// <param name="dislkpDocumentType"></param>
        public static Int32 GetDocumentTypeIDByCode(String docTypeCode, Int32 tenantID)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpDocumentType>(tenantID).FirstOrDefault(x => x.DMT_Code == docTypeCode).DMT_ID;
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

        #region UAT 1559 As an admin, I should be able to attach a form to be completed as a immunization package attribute
        public static int GetDocumentTypeIDByCode(String code)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpDocumentType>().Where(x => x.DT_Code == code).FirstOrDefault().DT_ID;
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

        public static List<Entity.ClientEntity.ClientSystemDocument> GetComplianceViewDocuments(Int32 tenantID, Int32 docTypeID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetComplianceViewDocuments(docTypeID);
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

        public static Boolean DeleteComplianceViewDocument(Int32 tenantID, Int32 systemDocumentID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).DeleteComplianceViewDocument(systemDocumentID, currentUserId);
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

        public static Boolean IsDocumentMappedWithAttribute(Int32 tenantID, Int32 systemDocumentID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).IsDocumentMappedWithAttribute(systemDocumentID);
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

        public static bool UpdateComplianceViewDocument(Int32 tenantID, Entity.ClientEntity.ClientSystemDocument attributeDocument)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).UpdateComplianceViewDocument(attributeDocument);
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

        public static Int32 GetDocumentStatusIDByCode(String code)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpDocumentStatu>().Where(x => x.DS_Code == code).FirstOrDefault().DS_ID;
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

        public static Boolean SaveComplianceViewDocument(Int32 tenantID, List<Entity.ClientEntity.ClientSystemDocument> lstViewDocuments)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).SaveComplianceViewDocument(lstViewDocuments);
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

        public static List<lkpDocumentFieldType_> GetDocumentFieldTypes(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentFieldType_>(tenantId).Where(condition => !condition.DFT_IsDeleted).ToList();
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

        public static List<DocumentFieldMapping> GetDocumentFieldMapping(Int32 tenantID, Int32 clientSystemDocumentID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetDocumentFieldMapping(clientSystemDocumentID);
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

        public static bool UpdateDocumentFieldMapping(Int32 tenantID, Int32 documentFieldMappingID, Int32 documentFieldTypeID, Int32 loggedInUdserID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).UpdateDocumentFieldMapping(documentFieldMappingID, documentFieldTypeID, loggedInUdserID);
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

        public static String GetCategoryNamesByCategoryIds(String categoryIds, int tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCategoryNamesByCategoryIds(categoryIds);
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

        public static String GetItemNamesByItemIds(String itemIds, int tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetItemNamesByItemIds(itemIds);
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

        public static List<CommunicationCCUsersList> GetCCusersWithNodePermissionAndCCUserSettings(Int32 communicationSubEventId, Int32 tenantId, Int32? hierarchyNodeID, Int32 objectTypeId, Int32 recordId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCCusersWithNodePermissionAndCCUserSettings(communicationSubEventId, tenantId, hierarchyNodeID, objectTypeId, recordId);
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

        #region Shot Series Setup Screens


        /// <summary>
        /// Get the hierarchical tree data for mapping screen
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="packageIdList"></param>
        /// <returns></returns>
        public static List<GetShotSeriesTree> GetShotSeriesTreeData(Int32 tenantId)
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Tenant Id: " + tenantId.ToString() + "";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: GetShotSeriesTreeData with " + parametersPassed + "");
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetShotSeriesTreeData();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the ComplianceItems of the selected Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ComplianceItem> GetComplianceItemsByCategory(Int32 categoryId, Int32 tenantId)
        {
            try
            {  //UAT-2069 :GetComplianceItemsByCategory accepts multiple categoryIDs.
                List<Int32> lstCategoryIds = new List<Int32>();
                lstCategoryIds.Add(categoryId);
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceItemsByCategory(lstCategoryIds);
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
        /// Get Series Un-Mapped Attributes
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="itemSeriesID"></param>
        /// <returns></returns>
        public static List<SeriesAttributeContract> GetUnMappedAttributes(Int32 tenantID, Int32 itemSeriesID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetUnMappedAttributes(itemSeriesID);
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
        /// Save Series Un-Mapped Attributes
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="currentUserID"></param>
        /// <param name="lstSeriesAttributeContract"></param>
        /// <returns></returns>
        public static Boolean SaveUnMappedAttributes(Int32 tenantID, Int32 currentUserID, List<SeriesAttributeContract> lstSeriesAttributeContract)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).SaveUnMappedAttributes(currentUserID, lstSeriesAttributeContract);
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


        #region Series data return

        public static Tuple<List<SeriesItemContract>, List<SeriesAttributeContract>> GetSeriesDetails(Int32 seriesId, Int32 tenantId)
        {
            try
            {
                List<SeriesItemContract> seriesItemContract = new List<SeriesItemContract>();
                List<SeriesAttributeContract> seriesAttributeContract = new List<SeriesAttributeContract>();

                DataSet ds = new DataSet();
                ds = GetSeriesData(seriesId, tenantId);

                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var _itemSeriesItemId = Convert.ToInt32(dt.Rows[i]["ItemSeriesItemId"]);
                    var _uid = Guid.NewGuid();

                    var _seriesItemContract = seriesItemContract.Where(sic => sic.ItemSeriesItemId == _itemSeriesItemId).FirstOrDefault();
                    if (_seriesItemContract.IsNotNull())
                    {
                        _uid = _seriesItemContract.UniqueIdentifier;
                    }

                    seriesItemContract.Add(new SeriesItemContract
                    {
                        CmpItemId = Convert.ToInt32(dt.Rows[i]["CmpItemId"]),
                        CmpItemName = Convert.ToString(dt.Rows[i]["CmpItemName"]),
                        ItemSeriesItemOrder = Convert.ToInt32(dt.Rows[i]["ItemSeriesItemOrder"]),
                        ItemSeriesId = dt.Rows[i]["ItemSeriesId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["ItemSeriesId"]) : AppConsts.NONE,
                        ItemSeriesAttributeMapId = dt.Rows[i]["ItemSeriesAttributeMapId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["ItemSeriesAttributeMapId"]) : AppConsts.NONE,
                        ItemSeriesItemId = Convert.ToInt32(dt.Rows[i]["ItemSeriesItemId"]),
                        ItemSeriesName = Convert.ToString(dt.Rows[i]["ItemSeriesName"]),
                        SelectedAttributeId = dt.Rows[i]["SelectedAttributeId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["SelectedAttributeId"]) : AppConsts.NONE,
                        ISAM_ItemSeriesAttrId = dt.Rows[i]["ISAM_ItemSeriesAttrId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["ISAM_ItemSeriesAttrId"]) : AppConsts.NONE,
                        PostShuffleStatusCode = Convert.ToString(dt.Rows[i]["PostShuffleStatusCode"]),
                        IsTempRow = false,
                        UniqueIdentifier = _uid
                    });
                }

                DataTable dtSeriesAttributes = ds.Tables[1];
                for (int i = 0; i < dtSeriesAttributes.Rows.Count; i++)
                {
                    seriesAttributeContract.Add(new SeriesAttributeContract
                    {
                        ItemSeriesAttributeId = Convert.ToInt32(dtSeriesAttributes.Rows[i]["ItemSeriesAttributeId"]),
                        CmpAttributeId = Convert.ToInt32(dtSeriesAttributes.Rows[i]["CmpAttributeId"]),
                        CmpAttributeName = Convert.ToString(dtSeriesAttributes.Rows[i]["CmpAttributeName"]),
                        CmpAttributeDataType = Convert.ToString(dtSeriesAttributes.Rows[i]["CmpAttributeDataType"]),
                        IsKeyAttribute = Convert.ToBoolean(dtSeriesAttributes.Rows[i]["IsKeyAttribute"]),
                        IsSeriesAttribute = Convert.ToBoolean(dtSeriesAttributes.Rows[i]["IsSeriesAttribute"]),
                    });
                }

                DataTable dtItemAttributes = ds.Tables[2];
                for (int i = 0; i < dtItemAttributes.Rows.Count; i++)
                {
                    seriesAttributeContract.Add(new SeriesAttributeContract
                    {
                        CmpAttributeId = Convert.ToInt32(dtItemAttributes.Rows[i]["CmpAttributeId"]),
                        CmpAttributeName = Convert.ToString(dtItemAttributes.Rows[i]["CmpAttributeName"]),
                        CmpAttributeDataType = Convert.ToString(dtItemAttributes.Rows[i]["CmpAttributeDataType"]),
                        CmpItemId = Convert.ToInt32(dtItemAttributes.Rows[i]["CmpItemId"]),
                        IsSeriesAttribute = Convert.ToBoolean(dtItemAttributes.Rows[i]["IsSeriesAttribute"]),
                    });
                }
                return new Tuple<List<SeriesItemContract>, List<SeriesAttributeContract>>(seriesItemContract, seriesAttributeContract);
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

        public static DataSet GetSeriesData(Int32 seriesId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetSeriesData(seriesId);
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
        /// Save-Update the mapping of the Item Attributes with the Series Attributes.
        /// </summary>
        /// <param name="lstSeriesItemContract"></param>
        /// <param name="currentUserId"></param>
        public static void SaveUpdateSeriesMapping(List<SeriesItemContract> lstSeriesItemContract, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveUpdateSeriesMapping(lstSeriesItemContract, currentUserId);
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

        public static List<ComplianceAttribute> GetComplianceAttributesByItemIds(List<Int32> lstItemIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetComplianceAttributesByItemIds(lstItemIds);
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

        #region Manage Shot Series

        public static List<ItemSery> GetItemShotSeries(Int32 categoryId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetItemShotSeries(categoryId);
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


        public static Boolean AddNewShotSeries(Int32 tenantID, ItemSery itemSeries)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).AddNewShotSeries(itemSeries);
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


        public static Boolean DeleteItemSeries(Int32 itemSeriesId, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).DeleteItemSeries(itemSeriesId, currentLoggedInUserId);
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

        public static ItemSery GetCurrentItemSeriesInfo(Int32 currentSeriesID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCurrentItemSeriesInfo(currentSeriesID);
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

        public static Boolean UpdateItemSeries(Int32 tenantID, ItemSery itemSeries)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).UpdateItemSeries(itemSeries);
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
        /// Save/Update the Item Series Items and Attributes
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="lstItemIds"></param>
        /// <param name="dicAttributeIds"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static void SaveSeriesData(Int32 seriesId, List<Int32> lstItemIds, Dictionary<Int32, Boolean> dicAttributeIds, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveSeriesData(seriesId, lstItemIds, dicAttributeIds, currentUserId);
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
        /// Add New ItemSeriesItem, on table mapping new click
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="itemId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void SaveItemSeriesItem(Int32 seriesId, Int32 itemId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveItemSeriesItem(seriesId, itemId, currentUserId);
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
        /// Remove Item from ItemSeriesItem, on table mapping Remove click
        /// </summary>
        /// <param name="itemSeriesItemId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void RemoveItemSeriesItem(Int32 itemSeriesItemId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).RemoveItemSeriesItem(itemSeriesItemId, currentUserId);
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
        /// get the item id from ItemSeriesItem table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<Int32> GetItemSeriesItemsBySeriesId(Int32 seriesId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetItemSeriesItemsBySeriesId(seriesId);
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
        /// get the Attribute id from ItemSeriesAttribute table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<Int32> GetItemSeriesAttributeBySeriesId(Int32 seriesId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetItemSeriesAttributeBySeriesId(seriesId);
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
        /// get the Attribute id from ItemSeriesAttribute table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Int32 GetItemSeriesKeyAttributeBySeriesId(Int32 seriesId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetItemSeriesKeyAttributeBySeriesId(seriesId);
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

        public static List<CompliancePackage> GetPackagesRelatedToCategory(Int32 categoryId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPackagesRelatedToCategory(categoryId);
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
        /// Check if Series Mapped Attribute exist
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="itemSeriesId"></param>
        /// <returns></returns>
        public static Boolean CheckIfSeriesMappedAttrExist(Int32 tenantId, Int32 itemSeriesId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).CheckIfSeriesMappedAttrExist(itemSeriesId);
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

        public static Dictionary<Int32, Boolean> GetComplianceRqdForPackage(Int32 packageId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetComplianceRqdForPackage(packageId);
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

        #region UAT 1560 WB: We should be able to add documents that need to be signed to the order process
        public static List<GenericSystemDocumentMappingContract> GetGenericSystemDocumentMapping(Int32 tenantID, Int32 recordID, Int32 recordTypeID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetGenericSystemDocumentMapping(recordID, recordTypeID);
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

        public static string SaveAdditionalDocumentMapping(Int32 tenantID, Int32 recordID, Int32 recordTypeID, List<Int32> lstSelectedDocumentsID, Int32 loggedInUserID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).SaveAdditionalDocumentMapping(recordID, recordTypeID, lstSelectedDocumentsID, loggedInUserID);
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

        public static string DeleteAdditionalDocumentMapping(int tenantID, int docMappingID, int loggedInUserID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).DeleteAdditionalDocumentMapping(docMappingID, loggedInUserID);
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

        public static ApplicantDocument SaveEsignedAdditionalDocumentAsPdf(Int32 tenantID, String PdfPath, String filename, Int32 fileSize, String documentTypeCode,
                                                               Int32 currentLoggedInUseID, Int32 orgUserID, Boolean isSetDataEntryDocStatusToCompleted, Boolean isSearchableOnly)
        {
            try
            {
                List<lkpDocumentType> lkpDocumentType = GetDocumentType(tenantID);
                List<lkpDataEntryDocumentStatu> dataEntryDocStatus = LookupManager.GetLookUpData<lkpDataEntryDocumentStatu>(tenantID);
                String dataEntryDocStatuscode = isSetDataEntryDocStatusToCompleted ? DataEntryDocumentStatus.COMPLETE.GetStringValue() :
                                                                                    DataEntryDocumentStatus.NEW.GetStringValue();
                Int16 dataEntryDocStatusId = AppConsts.NONE;

                dataEntryDocStatusId = dataEntryDocStatus.FirstOrDefault(cnd => cnd.LDEDS_Code == dataEntryDocStatuscode && !cnd.LDEDS_IsDeleted).LDEDS_ID;

                Int32 documentTypeId = lkpDocumentType.Where(cond => cond.DMT_Code == documentTypeCode && !cond.DMT_IsDeleted).FirstOrDefault().DMT_ID;

                return BALUtils.GetComplianceSetupRepoInstance(tenantID).SaveEsignedAdditionalDocumentAsPdf(PdfPath, filename, fileSize, documentTypeId,
                                                                                                   currentLoggedInUseID, orgUserID, dataEntryDocStatusId, isSearchableOnly);
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

        #region UAT-1812:Creation of an Approval/rejection summary for applicant logins
        public static Tuple<List<ApplicantDataSummaryContract>, List<ApplicantBackgroundSummaryContract>> GetAppSummaryDataAfterLastLogin(Int32 currentLoggedInUserID, Int32 tenantID, DateTime? lastLoginTime)
        {
            try
            {
                List<DataTable> dataTable = BALUtils.GetComplianceSetupRepoInstance(tenantID).GetAppSummaryDataAfterLastLogin(currentLoggedInUserID, lastLoginTime);
                List<ApplicantDataSummaryContract> lstComplianceData = new List<ApplicantDataSummaryContract>();
                List<ApplicantBackgroundSummaryContract> lstBackgroundData = new List<ApplicantBackgroundSummaryContract>();
                if (!dataTable.IsNullOrEmpty())
                {
                    IEnumerable<DataRow> rows = dataTable[0].AsEnumerable();
                    rows.ForEach(x =>
                   {
                       ApplicantDataSummaryContract compData = new ApplicantDataSummaryContract();
                       compData.ApprovedItemCount = x["ApprovedItemsCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ApprovedItemsCount"]);
                       compData.PendingReviewItemCount = x["PendingReviewItemsCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["PendingReviewItemsCount"]);
                       compData.IncompleteCategoryName = x["CategoryName"].ToString();
                       compData.RejectedItemCount = x["NotApprovedItemsCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["NotApprovedItemsCount"]);
                       lstComplianceData.Add(compData);
                   });

                    IEnumerable<DataRow> bkgRows = dataTable[1].AsEnumerable();
                    bkgRows.ForEach(x =>
                    {
                        ApplicantBackgroundSummaryContract bkgData = new ApplicantBackgroundSummaryContract();
                        bkgData.OrderID = x["OrderID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrderID"]);
                        bkgData.IsServiceGroupFlagged = x["IsServiceGroupFlagged"].GetType().Name == "DBNull" ? (Boolean?)null : Convert.ToBoolean(x["IsServiceGroupFlagged"]);
                        bkgData.DPM_Label = x["DPM_Label"].ToString();
                        bkgData.OrderNumber = x["OrderNumber"].ToString();
                        bkgData.ServiceGroupName = x["ServiceGroupName"].ToString();
                        bkgData.SvcGrpReviewStatusName = x["SvcGrpReviewStatusName"].ToString();
                        bkgData.SvcGrpStatusName = x["SvcGrpStatusName"].ToString();
                        bkgData.RecentlyCompletedOrderID = x["RecentlyCompletedOrderID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["RecentlyCompletedOrderID"]);
                        bkgData.OrdPkgSvcGroupID = x["OrderPackageSvcGrpID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrderPackageSvcGrpID"]);
                        if (bkgData.IsServiceGroupFlagged.IsNotNull()
                            && String.Compare(x["SvcGrpStatusCode"].ToString(), BkgSvcGrpStatusType.COMPLETED.GetStringValue(), true) == AppConsts.NONE)
                        {
                            if (bkgData.IsServiceGroupFlagged.Value)
                            {
                                bkgData.SvcGrpFlaggedStatusImgPath = "~/images/small/Red.gif";
                                bkgData.svcGroupFlaggedStatusAltText = "Red";
                            }
                            else
                            {
                                bkgData.SvcGrpFlaggedStatusImgPath = "~/images/small/Green.gif";
                                bkgData.svcGroupFlaggedStatusAltText = "Green";
                            }
                        }
                        else
                        {
                            bkgData.SvcGrpFlaggedStatusImgPath = "~/images/small/blank.gif";
                        }
                        lstBackgroundData.Add(bkgData);
                    });

                }
                return new Tuple<List<ApplicantDataSummaryContract>, List<ApplicantBackgroundSummaryContract>>(lstComplianceData, lstBackgroundData);
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

        public static List<SeriesAttributeContract> GetSeriesDetailsForShuffleTest(Int32 seriesID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetSeriesDetailsForShuffleTest(seriesID);
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

        public static Dictionary<ShotSeriesSaveResponse, List<SeriesAttributeContract>> GetSeriesDetailsAfterShuffleTest(Int32 seriesID, Int32 systemUserID,
                                                                                                        String seriesAttributeXML, String ruleMappingXML, Int32 tenantID, Int32 selectedPackageID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetSeriesDetailsAfterShuffleTest(seriesID, systemUserID, seriesAttributeXML, ruleMappingXML, selectedPackageID);
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

        public static List<RuleDetailsForTestContract> GetSeriesRuleDetailsForShuffleTest(Int32 seriesID, Int32 tenantID, Int32 selectedPackageID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetSeriesRuleDetailsForShuffleTest(seriesID, selectedPackageID);
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

        public static List<RuleDetailsForTestContract> GetDetailsForComplianceRuleTest(Int32 ruleMappingID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetDetailsForComplianceRuleTest(ruleMappingID);
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
        /// UAT-2043:For Data Entry:  Quick Package Copy Across Tenants
        /// </summary>
        /// <param name="compliancePackageID"></param>
        /// <param name="compliancePackageName"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void CopyPackageStructureToOtherClient(Int32 TenantId, Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 SelectedTenantId)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).CopyPackageStructureToOtherClient(TenantId, compliancePackageID, compliancePackageName, currentUserId, SelectedTenantId);
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

        #region UAT-2159 : Show Category Explanatory note as a mouseover on the category name on the student data entry screen.
        public static Dictionary<Int32, String> GetExplanatoryNotesForCategory(Int32 packageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetExplanatoryNotesForCategory(packageId);
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

        #region UAT-2305
        public static List<CopyDataQueue> GetDataForCopyToRequirement(Int32 tenantID, Int32 trackingItmDataObjTypeID, Int32 trackingSubsDataObjTypeID, Int32 rotSubsObjTypeID, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetDataForCopyToRequirement(trackingItmDataObjTypeID, trackingSubsDataObjTypeID, rotSubsObjTypeID, chunkSize);
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

        public static List<RequirementRuleData> CopyComplianceDataToRequirement(Int32 tenantID, Int32 LoggedInUserID, String ItemDataIds, String RPSIds)
        {
            try
            {
                DataTable table = BALUtils.GetComplianceSetupRepoInstance(tenantID).CopyComplianceDataToRequirement(LoggedInUserID, ItemDataIds, RPSIds);

                IEnumerable<DataRow> rows = table.AsEnumerable();
                List<RequirementRuleData> lstRequirementRuleData = new List<RequirementRuleData>();
                lstRequirementRuleData.AddRange(rows.Select(col => new RequirementRuleData
                {
                    Rps_Id = col["RPSID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPSID"]),
                    PackageId = col["ReqPkgID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqPkgID"]),
                    CategoryId = col["ReqCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqCategoryID"]),
                    ItemId = col["ReqItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqItemID"]),
                    ApplicantUserID = col["OrgUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["OrgUserID"]),
                    IsNewPackage = col["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(col["IsNewPackage"]),
                    FieldId = col["ReqFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqFieldID"]),
                    ApplicantRequirementItemDataID = col["ApplicantRequirementItemDataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ApplicantRequirementItemDataID"]),
                }).ToList());
                return lstRequirementRuleData;
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

        public static void ExecuteRequirementRules(Int32 tenantID, Int32 LoggedInUserID, List<RequirementRuleData> lstRequirementRuleData)
        {
            try
            {
                foreach (RequirementRuleData requirementRuleData in lstRequirementRuleData)
                {
                    List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

                    ruleObjectMappingList.Add(new RequirementRuleObject
                    {
                        RuleObjectTypeCode = ObjectType.Compliance_Package.GetStringValue(),
                        RuleObjectId = Convert.ToString(requirementRuleData.PackageId),
                        RuleObjectParentId = Convert.ToString(AppConsts.NONE)
                    });

                    ruleObjectMappingList.Add(new RequirementRuleObject
                    {
                        RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                        RuleObjectId = Convert.ToString(requirementRuleData.CategoryId),
                        RuleObjectParentId = Convert.ToString(requirementRuleData.PackageId)
                    });

                    ruleObjectMappingList.Add(new RequirementRuleObject
                    {
                        RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                        RuleObjectId = Convert.ToString(requirementRuleData.ItemId),
                        RuleObjectParentId = Convert.ToString(requirementRuleData.CategoryId)
                    });
                    RequirementRuleManager.ExecuteRequirementObjectBuisnessRules(ruleObjectMappingList, requirementRuleData.Rps_Id, requirementRuleData.ApplicantUserID, tenantID);
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

        public static String GetComplianceAttributeDatatypeByAttributeID(int tenantID, int complianceAttrID)
        {
            try
            {
                if (complianceAttrID > AppConsts.NONE)
                {
                    return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetComplianceAttributeDatatypeByAttributeID(tenantID, complianceAttrID);
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
        #region UAT-2411
        public static List<InstitutionConfigurationPackageDetails> GetInstitutionConfigurationBundlePackageDetailsList(Int32 bundlePackageID, Int32 tenantID, Int32 HierarchyID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetInstitutionConfigurationBundlePackageDetailsList(bundlePackageID, HierarchyID);
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

        //UAT-2339
        public static ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreewithPermissions(Int32 tenantID, Int32? currentUserID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetInstituteHierarchyTreewithPermissions(currentUserID);
                //return SetDataForTreeWithPermissions(InstitutionHierarchywithPerData);

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
        //UAT 2506
        public static List<AdminDataAuditHistory> GetAdminDocumentDataAuditHistory(AdminDataAuditHistory parameterContact, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(parameterContact.SelectedTenantID).GetAdminDocumentDataAuditHistory(parameterContact, customPagingArgsContract);
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
        public static List<AdminDataAuditHistory> GetDocumentAssignmentAuditHistory(AdminDataAuditHistory parameterContact)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(parameterContact.SelectedTenantID).GetDocumentAssignmentAuditHistory(parameterContact);
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


        public static List<DeptProgramMapping> GetChildNodesByNodeID(Int32 NodeID, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(TenantId).GetChildNodesByNodeID(NodeID);
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
        //UAT-2717
        public static List<CompliancePackage> GetTenantCompliancePackage(Int32 tenantId)
        {
            try
            {
                List<Entity.Tenant> tenantList = LookupManager.GetLookUpData<Entity.Tenant>().Where(condition => condition.IsActive && !condition.IsDeleted).ToList();
                List<CompliancePackage> compliancePackage = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetTenantCompliancePackage(tenantId);
                return compliancePackage;
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

        #region UAT-2744
        /// Get Institute Hierarchy Tree data
        /// </summary>
        /// <returns></returns>
        public static List<GetDepartmentTree> GetInstituteHierarchyPackageTree(Int32 tenantId, Int32? orgUserID, String compliancePackageTypeCode, Boolean IsCompliancePackage, Boolean fetchNoAccessNodes = false)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetInstituteHierarchyPackageTree(orgUserID, compliancePackageTypeCode, IsCompliancePackage, fetchNoAccessNodes);
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

        #region UAT-2388
        public static DataTable GetAutomaticPackageInvitations(Int32 tenantID, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetAutomaticPackageInvitations(chunkSize);
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

        public static Boolean UpdateAutomaticPackageInvitationsEmailStatus(Int32 tenantID, List<Int32> AIPML_Ids, Int32 backgroundProcessUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).UpdateAutomaticPackageInvitationsEmailStatus(AIPML_Ids, backgroundProcessUserId);
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

        #region "UAT-2924: Add upcoming expirations to Since You Been Gone popup as part of the not compliant categories"

        public static List<UpcomingCategoryExpirationContract> GetUpcomingExpirationcategoryByLoginId(Int32 currentUserID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantID).GetUpcomingExpirationcategoryByLoginId(currentUserID);
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

        public static void GetCategoryItemAttributeMappingID(Int32 tenantId, Int32 complianceCategoryID, Int32 complianceItemID, Int32 complianceAttributeID, ref Int32 complianceCategoryItemID, ref Int32 complianceItemAttributeID)
        {
            try
            {
                BALUtils.GetComplianceSetupRepoInstance(tenantId).GetCategoryItemAttributeMappingID(complianceCategoryID, complianceItemID, complianceAttributeID, ref complianceCategoryItemID, ref complianceItemAttributeID);
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


        public static List<PackageBundleContract> GetPackageIncludedInBundle(Int32 tenantId, Int32 ID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPackageIncludedInBundle(ID);
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

        #region UAT-3566
        public static String GetLastRuleAppliedDate(Int32 tenantId, Int32 associationHierarchyId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetLastRuleAppliedDate(associationHierarchyId);
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

        public static PackageBundleNodeMapping GetPackageBundleNodeMapping(Int32 tenantId, Int32 packageBundleId, Int32 deptProgramMappingId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetPackageBundleNodeMapping(packageBundleId, deptProgramMappingId);
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

        public static Boolean UpdatePackageBundleNodeMapping(Int32 tenantId, Int32 packageBundleId, Int32 deptProgramMappingId, Boolean isBundleExclusive, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).UpdatePackageBundleNodeMapping(packageBundleId, deptProgramMappingId, isBundleExclusive, currentLoggedInUserId);
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
        #region UAT-3896
        public static String GetHierarchyTextForBundle(Int32 packageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetHierarchyTextForBundle(packageId);
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

        #region UAT-3951: Rejection Reason

        public static List<Entity.lkpRejectionReasonCategory> GetRejectionReasonCategoryList()
        {
            return LookupManager.GetLookUpData<Entity.lkpRejectionReasonCategory>();
        }


        public static IQueryable<Entity.RejectionReason> GetRejectionReasons()
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetRejectionReasons();
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

        public static Boolean SaveUpdateRejectionReason(Entity.RejectionReason rejectionReasonData)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).SaveUpdateRejectionReason(rejectionReasonData);
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

        public static Boolean DeleteRejectionReason(Int32 rejectionReasonID, Int32 loggedInUserID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteRejectionReason(rejectionReasonID, loggedInUserID);
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

        public static List<Entity.RejectionReason> GetRejectionReasonByIDs(List<Int32> lstRejectReasonIDs)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(SecurityManager.DefaultTenantID).GetRejectionReasonByIDs(lstRejectReasonIDs);
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

        #region UAT-3873
        /// <summary>
        /// To get Program Background Packages by Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<NodePackagesDetails> GetProgramAvailablePackagesByProgramMapId(Int32 deptProgramMappingID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetProgramAvailablePackagesByProgramMapId(deptProgramMappingID);

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

        /// <summary>
        /// Method to get file extensions
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IQueryable<lkpFileExtension> BindFileExtensions(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpFileExtension>(tenantId).Where(cond => !cond.IsDeleted).AsQueryable();
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

        public static IQueryable<DeptProgramRestrictedFileExtension> GetSelectedFileExtensions(Int32 tenantId, Int32 depProgramMappingId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetMappedDeptProgramFileExtensions(depProgramMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }       

        //UAT-4278
        public static List<DiscardedDocumentAuditContract> GetDiscardedDocumentDataAuditHistory(DiscardedDocumentAuditContract parameterContact, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(parameterContact.SelectedTenantID).GetDiscardedDocumentDataAuditHistory(parameterContact, customPagingArgsContract);
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

        #region Admin Entry Portal 
        public static List<DeptProgramAdminEntryAcctSetting> GetDeptProgramAdminEntryAcctSettings(Int32 tenantId, Int32 depProgramMappingId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetDeptProgramAdminEntryAcctSettings(depProgramMappingId);
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
        public static bool SaveNodeSettingsForAdminEntry(Int32 tenantId, Int32 depProgramMappingId, List<DeptProgramAdminEntryAcctSetting> deptProgramAdminEntryAcctSettingList)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).SaveNodeSettingsForAdminEntry(depProgramMappingId, deptProgramAdminEntryAcctSettingList);
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

        #region UAT-5198
        public static Dictionary<String, String> IsCategoriesAvailableinSelectedPackages(List<Int32> lstPacakgeIds, List<Int32> lstCategoryIds, List<Tuple<Int32, Int32, Int32>> tuples, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).IsCategoriesAvailableinSelectedPackages(lstPacakgeIds, lstCategoryIds, tuples);
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

        public static AssignmentHierarchy GetAssignmentHierarchyByRuleSetId(int ruleSetId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAssignmentHierarchyByRuleSetId(ruleSetId);
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
