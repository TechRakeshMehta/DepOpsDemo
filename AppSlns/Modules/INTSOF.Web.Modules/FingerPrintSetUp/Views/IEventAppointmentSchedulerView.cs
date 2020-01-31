using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
   public interface IEventAppointmentSchedulerView
    {
        Int32 SlotId { get; set; }
        Int32 TenantId { get; set; }
        Int32 LocationId { get; set; }
        DateTime? SelectedSlotDate { get; set; }
        AppointmentSlotContract appointmentSlotContract { get; set; }
        List<AppointmentSlotContract> lstAppointmentSlots { get; set; }
        Int32 ApplicantAppointmentId { get; set; }
        Int32 ApplicantOrgUserId { get; set; }
        Int32 OrderId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Boolean IsApplicant { get; set; }
        AppointmentSlotContract SlotRescheduleContract { get; set; }
        Boolean IsCreateOrderScreen { get; set; }
        Boolean IsOrderPaymentDetailScreen { get; set; }
        //DateTime selecteddate { get; set; }
        String SelectedSlotStartTime { get; set; }
        Boolean IsAppointmentOrderDetailScreen { get; set; }
        Int32 SelectedSlotID { get; set; }
        //Int32 ReservedSlotID { get;set;}
        Int32 EventSlotId { get; set; }
    }
}
