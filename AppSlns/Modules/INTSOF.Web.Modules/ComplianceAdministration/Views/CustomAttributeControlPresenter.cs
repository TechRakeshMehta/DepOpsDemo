﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class CustomAttributeControlPresenter : Presenter<ICustomAttributeControlView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public CustomAttributeControlPresenter([CreateNew] IComplianceAdministrationController controller)
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

        //public void GetAllUserGroup()
        //{
        //    View.lstUserGroups = ComplianceSetupManager.GetAllUserGroup(View.TenantID);
        //}

        //public void GetUserGroupsForUser()
        //{
        //    View.lstUserGroupsForUser = ComplianceDataManager.GetUserGroupsForUser(View.TenantID, View.OrganizationUserId);
        //}
    }
}



