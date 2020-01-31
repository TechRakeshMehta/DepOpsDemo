using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class CategoryListPresenter : Presenter<ICategoryListView>
    {

        public override void OnViewLoaded()
        {
            //View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public override void OnViewInitialized()
        {
            //View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public List<CompliancePackageCategory> GetComplianceCategoriesByPackage()
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            return ComplianceSetupManager.GetcomplianceCategoriesByPackage(View.ViewContract.AssignToPackageId, View.SelectedTenantId, getTenantName);
        }

        public List<ComplianceCategory> GetNotMappedComplianceCategories()
        {
            return ComplianceSetupManager.GetNotMappedComplianceCategories(View.ViewContract.AssignToPackageId, View.SelectedTenantId);
        }

        //public void GetMasterComplianceCategories()
        //{
        //    Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
        //    View.complianceCategories = ComplianceSetupManager.GetComplianceCategories(View.SelectedTenantId);
        //}

        /// <summary>
        /// Adds the category to the selected package after saving the new category added(if any).
        /// </summary>
        public void SaveNewCategory()
        {
            if (View.selectedCategoryId == 0)
            {
                if (ComplianceSetupManager.CheckIfCategoryNameAlreadyExist(View.ViewContract.CategoryName, View.ViewContract.ComplianceCategoryId, View.SelectedTenantId))
                {
                    View.ErrorMessage = String.Format("{0} already exists.", View.ViewContract.CategoryName);
                    return;
                }
                else
                {
                    ComplianceCategory newCategory = new ComplianceCategory
                    {
                        CategoryName = View.ViewContract.CategoryName,
                        CategoryLabel = View.ViewContract.CategoryLabel,
                        ScreenLabel = View.ViewContract.ScreenLabel,
                        Description = View.ViewContract.Description,
                        TenantID = View.SelectedTenantId,
                        IsActive = View.ViewContract.Active,
                        DisplayOrder=View.ViewContract.DisplayOrder,
                        TriggerOtherCategoryRules = View.ViewContract.TriggerOtherCategoryRules,
                        SendItemDocOnApproval = View.ViewContract.SendItemDoconApproval //UAT-3805

                    };
                    //Dictionary notesDetail will contain Content type as keys and content value as values. 
                    Dictionary<String, String> notesDetail = new Dictionary<String, String>();
                    notesDetail.Add(LCContentType.ExplanatoryNotes.GetStringValue(), View.ViewContract.ExplanatoryNotes);
                    View.ViewContract.ComplianceCategoryId = ComplianceSetupManager.SaveCategoryDetail(newCategory, View.CurrentLoggedInUserId, notesDetail);
                    View.ErrorMessage = String.Empty;
                    //UAT-2305: Tracking to Rotation category/item/attribute mapping
                    //SaveUniversalCategoryMapping();
                }
            }
            else
            {
                View.ViewContract.ComplianceCategoryId = View.selectedCategoryId;
            }
            CompliancePackageCategory compliancePackageCategory = new CompliancePackageCategory();
            compliancePackageCategory.CPC_PackageID = View.PackageId;
            compliancePackageCategory.CPC_CategoryID = View.ViewContract.ComplianceCategoryId;
            compliancePackageCategory.CPC_DisplayOrder = View.ViewContract.DisplayOrder;
            compliancePackageCategory.CPC_ComplianceRequired = View.ViewContract.ComplianceRequired;
            compliancePackageCategory.CPC_ComplianceRqdStartDate = View.ViewContract.CmplncRqdStartDate;
            compliancePackageCategory.CPC_ComplianceRqdEndDate = View.ViewContract.CmplncRqdEndDate;
            ComplianceSetupManager.SaveCompliancePackageCategoryMapping(View.CurrentLoggedInUserId, View.SelectedTenantId, compliancePackageCategory);
        }

        public Boolean DeletePackageCategoryMapping()
        {
            ComplianceSetupManager.DeleteCompliancePackageCategoryMapping(View.ViewContract.AssignToPackageId, View.ViewContract.ComplianceCategoryId, View.CurrentLoggedInUserId, View.SelectedTenantId);
            return true;
        }

        public Boolean ifPackageCanBeDelted()
        {
            List<lkpObjectType> lkpObjectType = RuleManager.GetObjectTypeList(View.SelectedTenantId);
            Int32 objectTypeIdForPackage = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.CompliancePackage.GetStringValue()).OT_ID;
            Int32 objectTypeIdForCategory = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).OT_ID;
            IntegrityCheckResponse response = IntegrityManager.IfPackageCategoryMappingCanBeDeleted(View.ViewContract.ComplianceCategoryId, View.ViewContract.AssignToPackageId, View.SelectedTenantId, objectTypeIdForPackage, objectTypeIdForCategory);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceCategory cmpCategory = ComplianceSetupManager.getCurrentCategoryInfo(View.ViewContract.ComplianceCategoryId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpCategory.CategoryName);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 getTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        #region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //public void GetUniversalCategories()
        //{
        //    List<Entity.SharedDataEntity.UniversalCategory> lstUniversalCategory = new List<Entity.SharedDataEntity.UniversalCategory>();
        //    lstUniversalCategory = UniversalMappingDataManager.GetUniversalCategories(View.SelectedTenantId);
        //    lstUniversalCategory.Insert(0, new Entity.SharedDataEntity.UniversalCategory { UC_ID = 0, UC_Name = "--SELECT--" });
        //    View.LstUniversalCategory = lstUniversalCategory;
        //}

        //public void SaveUniversalCategoryMapping()
        //{
        //    if (View.SelectedUniversalCategoryID > AppConsts.NONE)
        //    {
        //        UniversalCategoryMapping ucmObj = new UniversalCategoryMapping();
        //        ucmObj.UCM_UniversalCategoryID = View.SelectedUniversalCategoryID;
        //        ucmObj.UCM_CategoryID = View.ViewContract.ComplianceCategoryId;
        //        ucmObj.UCM_CreatedBy = View.CurrentLoggedInUserId;
        //        ucmObj.UCM_CreatedOn = DateTime.Now;
        //        UniversalMappingDataManager.SaveUpdateCategoryMappingWithUniversalCat(View.SelectedTenantId, ucmObj, UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());
        //    }
        //}
        #endregion
    }
}




