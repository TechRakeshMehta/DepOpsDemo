using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.BkgSetup.Views
{
    public class VendorServicesPresenter : Presenter<IVendorServicesView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

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

        public int GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void FetchExternalBkgService()
        {
            View.ExternalBkgServices = BackgroundSetupManager.FetchExternalBkgServices(View.DefaultTenantId, View.VendorID);
        }

        public void RetrievingServiceAttributes(int EBS_ID)
        {
            View.ExternalBkgServiceAttributes = BackgroundSetupManager.FetchExternalBkgServiceAttributes(View.DefaultTenantId, EBS_ID);
        }
    }
}
