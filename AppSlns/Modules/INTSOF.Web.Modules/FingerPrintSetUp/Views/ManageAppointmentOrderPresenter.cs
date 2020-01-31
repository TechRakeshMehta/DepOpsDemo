using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageAppointmentOrderPresenter : Presenter<IManageAppointmentOrder>
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
         
        public void GetAppointmentOrders()
        {
            View.lstAppointmentOrder = new List<AppointmentOrderScheduleContract>();
            if (!View.TenantIDs.IsNullOrEmpty())
                View.lstAppointmentOrder = FingerPrintSetUpManager.GetAppointmentOrders(View.TenantIDs, View.IsAdminScreen, View.GridCustomPaging, View.CurrentLoggedInUserID, View.filterContract);
            if (View.lstAppointmentOrder.Count > 0)
                View.VirtualRecordCount = View.lstAppointmentOrder.FirstOrDefault().TotalCount;
            else
                View.VirtualRecordCount = AppConsts.NONE;
        }
        public void GetLocationList()
        {
            Boolean IsEnroller = false;
            if (!View.IsAdminScreen)
            {
                IsEnroller = true;
            }
            List<LocationContract> lstLocations = new List<LocationContract>();
            View.lstAvailableLocations = FingerPrintSetUpManager.GetFingerprintLocations(View.CurrentLoggedInUserID, IsEnroller).ToList();
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public Boolean RevokeAppointments()
        {
           return FingerPrintSetUpManager.RevokeSelectedAppointments(View.lstAppointmentOrderScheduleContract, View.CurrentLoggedInUserID);            
        }
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(Convert.ToInt32(View.TenantIDs), orderID);
        }

        //public void GetAllAppointmentStatus()
        //{
        //    View.lstAppointmentStatus = FingerPrintSetUpManager.GetAllAppointmentStatus().ToList();
        //}

        public void GetAllFingerPrintAppointmentStatus()
        {
            View.lstAppointmentStatus = FingerPrintSetUpManager.GetAllFingerPrintAppointmentStatus(View.lstTenant.Select(sel => sel.TenantID).FirstOrDefault());
        }
        #region Uat - 4025
        public void GetPermittedCBIId()
        {
            View.lstPermittedCBIUniqueIds = FingerPrintSetUpManager.GetPermittedCBIId(View.CurrentLoggedInUserID);            
        }
        public void GetHrAdminAppointmentOrders()
        {
            View.lstAppointmentOrder = new List<AppointmentOrderScheduleContract>();
            if (!View.TenantIDs.IsNullOrEmpty())
                View.lstAppointmentOrder = FingerPrintSetUpManager.GetHrAdminAppointmentOrders(View.TenantIDs, View.GridCustomPaging, View.CurrentLoggedInUserID, View.filterContract, View.IsHrAdminEnroller);
            if (View.lstAppointmentOrder.Count > 0)
                View.VirtualRecordCount = View.lstAppointmentOrder.FirstOrDefault().TotalCount;
            else
                View.VirtualRecordCount = AppConsts.NONE;
        }
        #endregion

        #region ABI Review
        public Boolean ChangeSendToCBIAppointmentStatus()
        {           
                return FingerPrintDataManager.ChangeSendToCBIAppointmentStatus(View.lstAppointmentOrderScheduleContract, View.CurrentLoggedInUserID);         
        }
        #endregion
    }
}


