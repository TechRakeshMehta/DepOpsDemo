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

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageOrderFulfillmentQueuePresenter: Presenter<IManageOrderFulfillmentQueueView>
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

        public void GetOrderFulFillment()
        {
            View.lstOrderFulFillment = new List<AppointmentOrderScheduleContract>();
            if (!((View.TenantIDs).IsNullOrEmpty()))
                View.lstOrderFulFillment = FingerPrintSetUpManager.GetOrderFulFillment(Convert.ToString(View.TenantIDs), View.IsAdminScreen, View.GridCustomPaging, View.CurrentLoggedInUserID, View.filterContract);
            if (View.lstOrderFulFillment.IsNotNull() && View.lstOrderFulFillment.Count > 0)
            {

                if (View.lstOrderFulFillment[0].TotalCount > 0)
                {
                    View.VirtualPageCount = View.lstOrderFulFillment[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }

            else
            {
                View.VirtualPageCount = AppConsts.NONE;
                View.CurrentPageIndex = 1;
            }         

            //UAT-1456 related changes.
            View.GridCustomPaging.VirtualPageCount = View.VirtualPageCount;
            View.GridCustomPaging.CurrentPageIndex = View.CurrentPageIndex;
            //searchBkgOrdeContract.GridCustomPagingArguments = View.GridCustomPaging;
            //View.SetBkgOrderSearchContract = searchBkgOrdeContract;

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

        //public void GetAllAppointmentStatus()
        //{
        //    View.lstAppointmentStatus = FingerPrintSetUpManager.GetAllAppointmentStatus().ToList();
        //}

        /// <summary>
        /// Get Bkg Order Service Details xml
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(Convert.ToInt32(View.TenantIDs), orderID);
        }

        public void GetAllFingerPrintAppointmentStatus()
        {
            View.lstAppointmentStatus = FingerPrintSetUpManager.GetAllFingerPrintAppointmentStatus(View.lstTenant.Select(sel => sel.TenantID).FirstOrDefault());
        }

        public void GetAllFingerPrintShipmentStatus()
        {            
            View.lstShipmentStatus = FingerPrintDataManager.GetServiceStatues(View.lstTenant.Select(sel => sel.TenantID).FirstOrDefault());
        }

        #region Uat - 4025
        public void GetPermittedCBIId()
        {
            View.lstPermittedCBIUniqueIds = FingerPrintSetUpManager.GetPermittedCBIId(View.CurrentLoggedInUserID);
        }
        public void GetHrAdminAppointmentOrders()
        {
            View.lstOrderFulFillment = new List<AppointmentOrderScheduleContract>();
            if (!View.TenantIDs.IsNullOrEmpty())
                View.lstOrderFulFillment = FingerPrintSetUpManager.GetHrAdminAppointmentOrders(View.TenantIDs, View.GridCustomPaging, View.CurrentLoggedInUserID, View.filterContract, View.IsHrAdminEnroller);
            if (View.lstOrderFulFillment.Count > 0)
                View.VirtualPageCount = View.lstOrderFulFillment.FirstOrDefault().TotalCount;
            else
                View.VirtualPageCount = AppConsts.NONE;
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
