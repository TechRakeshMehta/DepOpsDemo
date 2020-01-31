using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.BkgSetup.Views
{
    public class VendorAccountSettingsPresenter : Presenter<IVendorAccountSettingsView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
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

        public int GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void FetchVendorAccSettings()
        {
            View.VendorsAccountsList = BackgroundSetupManager.FetchVendorsAccountDetail(View.DefaultTenantId, View.VendorId);
        }

        public String SaveVendorsAccountDetail()
        {
            return BackgroundSetupManager.SaveVendorsAccountDetail(View.DefaultTenantId, View.VendorId, View.AccountNumber, View.AccountName, View.CurrentLoggedInUserId);
        }

        public String UpdateVendorsAccountDetail()
        {
            return BackgroundSetupManager.UpdateVendorsAccountDetail(View.DefaultTenantId, View.AccountNumber, View.AccountName, View.EvaID, View.CurrentLoggedInUserId);
        }

        public bool DeleteVendorsAccountDetail()
        {
            return BackgroundSetupManager.DeleteVendorsAccountDetail(View.DefaultTenantId, View.EvaID, View.CurrentLoggedInUserId);
        }
    }
}
