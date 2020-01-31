using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using System.Linq;
using System.Configuration;
using INTSOF.Utils;

namespace CoreWeb.Messaging.Views
{
    public class TransferRulesMaintenanceFormPresenter : Presenter<ITransferRulesMaintenanceFormView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMessagingController _controller;
        // public TransferRulesMaintenanceFormPresenter([CreateNew] IMessagingController controller)
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

        // TODO: Handle other view events and set state in the view
        public void BindInstitutions()
        {
            View.TenantID = SecurityManager.GetOrganizationUser(View.CurrentUserID).Organization.TenantID.Value;
            View.Institutions = MessageManager.GetTenantsForRules(View.TenantID).ToList();
        }

        public void BindInstitutionLocations()
        {
            View.InstitutionLocations = MessageManager.GetInstitutionLocations(Convert.ToInt32(View.InstitutionId), View.CurrentUserID, View.TenantID);
        }

        public void BindInstitutionPrograms()
        {
            //Commented by Sachin Singh for flexible hierarchy.
            //View.InstitutionPrograms = MessageManager.GetInstitutionPrograms(Convert.ToInt32(View.InstitutionId), Convert.ToInt32(View.LocationId), View.CurrentUserID, View.TenantID);
        }
        /// <summary>
        /// Invoked to get the folder as per userid
        /// </summary>
        public void GetFolders()
        {
            View.FolderList = MessageManager.GetFolders(View.CurrentUserID, View.UserGroupID);
        }

        /// <summary>
        /// Invoked to Save the new message rule
        /// </summary>
        public void SaveMessageRules()
        {
            String databaseName = MessageManager.GetDatabaseName(ConfigurationManager.ConnectionStrings[AppConsts.APPLICATION_CONNECTION_STRING].ConnectionString);
            MessageManager.SaveMessageRules(View.ViewContract, databaseName);
        }
    }
}




