using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ScheduleApplicantAppointmentPresenter : Presenter<IScheduleApplicantAppointmentView>
    {

        public ReserveSlotContract ReserveSlot()
        {
           
            return FingerPrintSetUpManager.ReserveSlot(View.ReservedSlotID,View.SelectedSlotID, View.CurrentLoggedInUserId);
        }
        public Boolean IsPrinterAvailableAtOldLoc(Int32 OrderId)
        {
            return FingerPrintSetUpManager.IsPrinterAvailableAtOldLoc(OrderId,View.TenantId);
        }

        

        public Boolean IsPrinterAvailableNewLoc(Int32 LocationId)
        {

            return FingerPrintSetUpManager.IsPrinterAvailableNewLoc(LocationId);
        }

        /// <summary>
        /// Get Bkg Order Service Details xml
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(Convert.ToInt32(View.TenantId), orderID);
        }

    }
}
