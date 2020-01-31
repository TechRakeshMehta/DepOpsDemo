using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class AppointmentSlotConfigurationPresenter : Presenter<IAppointmentSlotConfigurationView>
    {

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
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

        public void GetTenants()
        {
            View.Tenants = SecurityManager.GetListOfTenantWithLocationService();
        }

        public void GetFingerprintLocations()
        {
            List<LocationContract> result = new List<LocationContract>();

            if(!View.SelectedTenantIDs.IsNullOrWhiteSpace())
            {                
                result = FingerPrintSetUpManager.GetAssignedFingerprintLocations(View.SelectedTenantIDs,
                    View.CurrentLoggedInUserId,
                    View.GridCustomPaging);
            }            

            if (!result.IsNullOrEmpty())
            {
                View.VirtualRecordCount = result[0].TotalCount;
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }

            View.FingerprintLocations = result;
        }
    }
}
