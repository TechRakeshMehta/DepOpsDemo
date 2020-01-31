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
    public class CategoryListingPresenter : Presenter<ICategoryListingView>
    {

        public override void OnViewLoaded()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        //public List<CompliancePackageCategory> GetComplianceCategoriesByPackage()
        //{
        //    return ComplianceSetupManager.GetcomplianceCategoriesByPackage(View.ViewContract.AssignToPackageId,View.TenantId);
        //}

        public void GetComplianceCategoriesByPackage()
        {
            View.complianceCategories = ComplianceSetupManager.GetcomplianceCategoriesByPackageList(View.ViewContract.AssignToPackageId, View.SelectedTenantId);
        }

        public Boolean DeletePackageCategoryMapping()
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
            else
            {
                ComplianceSetupManager.DeleteCompliancePackageCategoryMapping(View.ViewContract.AssignToPackageId, View.ViewContract.ComplianceCategoryId, View.CurrentLoggedInUserId, View.SelectedTenantId);
                return true;
            }
        }
    }
}




