using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public class ServiceVendorsPresenter : Presenter<IServiceVendorsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
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

        public void FetchServiceVendors()
        {
            View.ServiceVendorsList = BackgroundSetupManager.FetchServiceVendorsList(View.DefaultTenantId);
        }

        public Boolean SaveServiceVendors(VendorsDetailsContract vendorsDetailsContract)
        {
            if (BackgroundSetupManager.CheckIfVendorNameAlreadyExist(vendorsDetailsContract.Name, 0, View.DefaultTenantId))
            {
                View.ErrorMessage = "Service Vendor Name can not be duplicate.";
                return false;
            }
            else
            {
                return BackgroundSetupManager.SaveServiceVendors(View.DefaultTenantId, vendorsDetailsContract, View.CurrentLoggedInUserId);
            }

        }

        public bool UpdateServiceVendors(VendorsDetailsContract vendorsDetailsContract, Int32 serviceVendorsID)
        {
            if (BackgroundSetupManager.CheckIfVendorNameAlreadyExist(vendorsDetailsContract.Name, serviceVendorsID, View.DefaultTenantId))
            {
                View.ErrorMessage = "Service Vendor Name can not be duplicate.";
                return false;
            }
            else
            {
                return BackgroundSetupManager.UpdateServiceVendors(View.DefaultTenantId, vendorsDetailsContract, serviceVendorsID, View.CurrentLoggedInUserId);
            }
        }

        public bool DeleteServiceVendors(Int32 serviceVendorsID)
        {
            return BackgroundSetupManager.DeleteServiceVendors(View.DefaultTenantId, serviceVendorsID, View.CurrentLoggedInUserId);
        }

        public string GetFormattedPhoneNumber(string EVE_ContactPhone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(EVE_ContactPhone);
        }
    }


}
