using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ItemsListingPresenter : Presenter<IItemsListingView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public ItemsListingPresenter([CreateNew] IComplianceAdministrationController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            //View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetComplianceItems()
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            View.CurrentViewContext.lstComplianceItems = ComplianceSetupManager.GetComplianceCategoryItems(Convert.ToInt32(View.ViewContract.CCI_CategoryId), View.CurrentViewContext.SelectedTenantId, getTenantName);
        }

        public Boolean DeleteCategoryItemMapping(Int32 itemId, Int32 parentCategoryId)
        {
            ComplianceSetupManager.DeleteCategoryItemMapping(Convert.ToInt32(View.ViewContract.CCI_Id), parentCategoryId, itemId, View.CurrentViewContext.CurrentLoggedInUserId, View.CurrentViewContext.SelectedTenantId);
            return true;
        }

        public Boolean IfItemCanBeRemoved(Int32 itemId, Int32 parentCategoryId)
        {
            List<lkpObjectType> lkpObjectType = RuleManager.GetObjectTypeList(View.SelectedTenantId);
            Int32 objectTypeIdForItem = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).OT_ID;
            Int32 objectTypeIdForCategory = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceCategory.GetStringValue()).OT_ID;
            IntegrityCheckResponse response = IntegrityManager.IfCategoryItemMappingCanBeDeleted(itemId, parentCategoryId, View.SelectedTenantId, objectTypeIdForItem, objectTypeIdForCategory);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceItem cmpItem = ComplianceSetupManager.getCurrentItemInfo(itemId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpItem.Name);
                return false;
            }
            return true;
        }
    }
}




