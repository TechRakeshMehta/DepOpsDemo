using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CoreWeb.FingerPrintSetUp.Views
{
    public class AppointmentLocationUpdatePresenter : Presenter<IAppointmentLocationUpdateView>
    {
        public override void OnViewInitialized()
        {

        }

        public void GetLocationAvailable(String lng, String lat)
        {
            View.lstLocations = new List<LocationContract>();
            if (!View.TenantId.IsNullOrEmpty() && !String.IsNullOrEmpty(lng) && !String.IsNullOrEmpty(lat))
            {
                View.lstLocations = FingerPrintSetUpManager.GetApplicantAvailableLocation(View.TenantId, lng, lat).Take(20).ToList();
            }
        }

        public void GetLocationForRescheduling(Int32 orderId, String lng, String lat)
        {
            View.lstLocations = new List<LocationContract>();
            if (!View.TenantId.IsNullOrEmpty() && !String.IsNullOrEmpty(lng) && !String.IsNullOrEmpty(lat))
            {
                View.lstLocations = FingerPrintSetUpManager.GetLocationForRescheduling(View.TenantId, orderId, lng, lat).Take(20).ToList();
            }
        }

        public void GetValidateEventCodeStatusAndEventDetails(FingerPrintAppointmentContract fingerPrintAppointmentContract)
        {

            View.lstLocations = new List<LocationContract>();
            if (!fingerPrintAppointmentContract.IsNullOrEmpty())
            {

                View.lstLocations = FingerPrintSetUpManager.GetValidateEventCodeStatusAndEventDetails(fingerPrintAppointmentContract, View.TenantId).ToList();
            }

        }
        public Boolean IsLocationServiceTenant()
        {
            return SecurityManager.IsLocationServiceTenant(View.TenantId);
        }
    }
}
