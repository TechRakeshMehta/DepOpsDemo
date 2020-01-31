using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IScheduleApplicantAppointmentView
    {
        Int32 SelectedSlotID { get; set; }
        Int32 ReservedSlotID { get; set; }
        Int32 CurrentLoggedInUserId { get;  }
        String LanguageCode { get; }
        Boolean IsFromOrderHistoryScreen { get; set; }
        Int32 TenantId { get; set; }
        String OrderTypeCode { get; set; } //// 4331 : change schedule appointment to step 2 of order flow
    }
}
