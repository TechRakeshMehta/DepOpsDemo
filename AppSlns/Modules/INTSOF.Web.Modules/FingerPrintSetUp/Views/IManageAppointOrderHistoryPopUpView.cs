using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;


namespace CoreWeb.FingerPrintSetUp.Views
{
   public interface IManageAppointOrderHistoryPopUpView
    {
        Int32 TenantId { get; set; }
        List<LocationServiceAppointmentAuditContract> lstAppAuditHistory { get; set; }
        Int32 OrderID { get; set; }
        bool IsCABSAppointment { get; set; }
    }
}
