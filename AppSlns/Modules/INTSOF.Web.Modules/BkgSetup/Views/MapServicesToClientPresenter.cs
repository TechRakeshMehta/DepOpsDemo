using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.BkgSetup.Views
{
    public class MapServicesToClientPresenter : Presenter<IMapServicesToClientView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        /// <summary>
        /// Method for retrieving list of all the tenants.
        /// </summary>
        public void GetTenants()
        {

            List<Entity.ClientEntity.Tenant> tempList = BackgroundSetupManager.getClientTenant().ToList();
            tempList.Insert(0, new Entity.ClientEntity.Tenant { TenantID = 0, TenantName = "--Select--" });
            View.ListTenants = tempList;
             //View.ListTenants = BackgroundSetupManager.getClientTenant();
        }


        /// <summary>
        /// Method for retrieving list of all the Services for Master.
        /// </summary>
        public void GetServices(Int32? SvcID = null, String SvcName = null, String ExtCode = null)
        {
            View.BackgroundServicesList = BackgroundSetupManager.GetBackgroundServices(View.DefaultTenantId, SvcID, SvcName, ExtCode);
        }

        /// <summary>
        /// Get Mappings from Client DB.
        /// </summary>
        public void GetAlreadyMappedServices()
        {
            View.PreMappedServicesIds = BackgroundSetupManager.GetExistingBackgroundServices(View.SelectedTenantId);
        }

        /// <summary>
        /// To Save Mapping.
        /// </summary>
        public void SaveMappings()
        {
            View.IsServicesMapped = BackgroundSetupManager.MapServicesToClient(View.SelectedTenantId, View.SelectedServices, View.DefaultTenantId);
        }


        /// <summary>
        /// Used To Deactivate Mapping
        /// </summary>
        public void DeactivateMapping()
        {
            View.IsServiceDeactivated = BackgroundSetupManager.DeactivateMapping(View.SelectedTenantId, View.DeactivateServiceId, View.DefaultTenantId);
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

    }
}
