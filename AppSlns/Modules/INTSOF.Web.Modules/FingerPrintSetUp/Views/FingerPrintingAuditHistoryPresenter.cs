using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class FingerPrintingAuditHistoryPresenter : Presenter<IFingerPrintingAuditHistoryView>
    {
        public override void OnViewInitialized()
        {
            GetTenants();
        }
        private void GetTenants()
        {
            View.lstTenant = new List<Tenant>();
            View.lstTenant = SecurityManager.GetListOfTenantWithLocationService();
        }
        public void GetLocationList()
        {            
            List<LocationContract> lstLocations = new List<LocationContract>();
            View.lstAvailableLocations = FingerPrintSetUpManager.GetFingerprintLocations(View.CurrentLoggedInUserID, false).ToList();
        }
        public void GetAuditHistory()
        {
            View.lstAppAuditHistory = new List<LocationServiceAppointmentAuditContract>();
            if (!View.TenantIDs.IsNullOrEmpty())
                View.lstAppAuditHistory = FingerPrintSetUpManager.GetAuditHistoryList(View.TenantIDs, View.GridCustomPaging, View.CurrentLoggedInUserID, View.filterContract);
            if (View.lstAppAuditHistory.Count > 0)
                View.VirtualRecordCount = View.lstAppAuditHistory.FirstOrDefault().TotalCount;
            else
                View.VirtualRecordCount = AppConsts.NONE;
        }
    }
}
