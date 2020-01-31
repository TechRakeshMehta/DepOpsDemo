using Entity.ClientEntity;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IFingerprintLocationDetailsView
    {
        Int32 CurrentLoggedInUserID { get; }
        LocationContract locationContract { get; set; }
        Int32 SelectedLocationID { get; set; }
        Int32 TenantId { get; set; }
        Boolean IsEditMode { get; set; }
        Boolean IsSaveMode { get; set; }
        //Int32 LocationMappingId { get; set; }

        // Commented below code for task Payment options should be removed from Location screen for everyone
        //  List<Entity.LocationEntity.lkpPaymentOption> ListPaymentOption { set; }
        // List<Int32> SelectedMappedPaymentOptions { get; set; }
        Boolean IsEnroller { get; set; }
        ManageEnrollerMappingContract EnrollerPermission { get; set; }
       // Int32 ScheduleMasterID { get; set; }
    }
}
