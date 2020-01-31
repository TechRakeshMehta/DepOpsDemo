using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.Utils;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IAppointmentSlotsDetailView
    {
        List<AppointmentSlotContract> AppointmentSlots { get; set; }
        Int32 SelectedLocationID { get; set; }
        AppointmentSlotContract CurrentEntity { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        DateTime SelectedSlotDate { get; set; }
        String PermissionCode { get; set; }
    }
}
