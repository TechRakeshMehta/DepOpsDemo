using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;

using Business.RepoManagers;
using INTSOF.Utils;
namespace CoreWeb.Templates.Views
{
    public class TemplatesGridPresenter : Presenter<ITemplatesGridView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private ITemplatesController _controller;
        // public TemplatesGridPresenter([CreateNew] ITemplatesController controller)
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

        // TODO: Handle other view events and set state in the view

        public void GetEventSpecificTemplates()
        {
            View.EventSpecificTemplates = TemplatesManager.GetEventSpecificTemplates();
        }

        public void GetOtherTemplates()
        {
            View.OtherTemplates = TemplatesManager.GetOtherTemplates();
        }

        public void DeleteTemplate()
        {
            TemplatesManager.DeleteTemplate(View.TemplateId, View.CurrentUserId, View.IsEventSpecific);
        }

        public List<Entity.Tenant> BindTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            return SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        public void GetTenantSpecificEventTemplates()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
                View.EventSpecificTemplates = TemplatesManager.GetTenantSpecificEventTemplates(View.SelectedTenantId);
        }

        #region UAT-3704
        public void GetAgencyHierarchySpecificEventTemplates()
        {
            View.lstAgencyHierarchySpecificTemplates = new List<INTSOF.UI.Contract.Templates.SystemEventTemplatesContract>();
            View.lstAgencyHierarchySpecificTemplates = TemplatesManager.GetAgencyHierarchySpecificEventTemplates();
        }

        public void GetAgencyHierarchyRootNodes()
        {
            View.lstAgencyHierarchyRootNodes = new List<Entity.SharedDataEntity.AgencyHierarchy>();
            View.lstAgencyHierarchyRootNodes = AgencyHierarchyManager.GetAgencyHierarchyRootNodes();
        }

        public Boolean DeleteAgencyHierarchyTemplate()
        {
            return TemplatesManager.DeleteAgencyHierarchyTemplate(View.TemplateId, View.CurrentUserId);
        }

        #endregion
    }
}




