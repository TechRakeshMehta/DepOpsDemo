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
  public class ManageAppoinOrderHistoryPopUpPresenter: Presenter<IManageAppointOrderHistoryPopUpView>
    {

        public override void OnViewInitialized()
        {
            base.OnViewInitialized();
        }
        public void GetOrderHistory()
        {
            View.lstAppAuditHistory = new List<LocationServiceAppointmentAuditContract>();
            if (!View.TenantId.IsNullOrEmpty())
                View.lstAppAuditHistory = FingerPrintDataManager.GetOrderHistoryList(View.TenantId,View.OrderID,View.IsCABSAppointment);
           
        }
    }
}
