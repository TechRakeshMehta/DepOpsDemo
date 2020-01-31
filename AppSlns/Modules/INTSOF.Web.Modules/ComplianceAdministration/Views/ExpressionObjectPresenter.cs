using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ExpressionObjectPresenter : Presenter<IExpressionObjectView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public ExpressionObjectPresenter([CreateNew] IComplianceAdministrationController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        //public List<CompliancePackageCategory> GetCompliancePackageCategory(Int32 packageId)
        //{
        //    return ComplianceSetupManager.GetcomplianceCategoriesByPackage(packageId, View.SelectedTenantId);
        //}

        //public List<ComplianceCategoryItem> GetComplianceCategoryItem(Int32 cagteogryId)
        //{
        //    return ComplianceSetupManager.GetComplianceCategoryItems(cagteogryId, View.SelectedTenantId);
        //}

        //public List<ComplianceItemAttribute> GetComplianceItemAttribute(Int32 itemId)
        //{
        //    return ComplianceSetupManager.GetComplianceItemAttribute(itemId, View.SelectedTenantId);
        //}

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void GetRuleObjectMappingType()
        {
            View.CurrentViewContext.lstRuleObjectMappingType = RuleManager.GetRuleObjectMappingType(View.CurrentViewContext.SelectedTenantId,false);
        }

        public void GetObjectType()
        {
            View.CurrentViewContext.lstObjectType = RuleManager.GetObjectTypes(View.CurrentViewContext.SelectedMappingTypeCode, View.CurrentViewContext.SelectedTenantId);
        }

        public ComplianceCategory GetComplianceCategory(Int32 categoryId)
        {
            return ComplianceSetupManager.getCurrentCategoryInfo(categoryId, View.SelectedTenantId);
        }
    }
}




