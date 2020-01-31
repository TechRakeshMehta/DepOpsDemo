using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IScheduleAppointment
    {

        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        Int32 LocationID { get; set; }
        Int32 ScheduleMasterID { get; set; }
        Int32 NewScheduleMasterID { get; set; }
        List<AppointmentContract> lstAppointmentContract { get; set; }
        AppointmentContract AppointmentContract { get; set; }
        String ErrorMessage { get; set; }
        Boolean IsReadOnly { get; set; }
    }
}
