using Business.RepoManagers;
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
    public class AppointmentReschedulerPresenter : Presenter<IAppointmentReschedulerView>
    {
        //public void GetTenantIdByOrganizationUserID()
        //{
            // View.TenantId = SecurityManager.GetTenantIdByOrganizationUserID(View.CurrentLoggedInUserID);
            //View.TenantId = 104;
        //}
        //public void GetAppointmentSlotID()
        //{
        //    // View.TenantId = SecurityManager.GetAppointmentSlotID(View.SlotID);
        //    View.SlotId = 1;
        //}

        public void GetAppointmentScheduleBySlotID()
        {
            if (View.SlotId > AppConsts.NONE)
            {
                View.appointmentSlotContract = new AppointmentSlotContract();
                View.appointmentSlotContract = FingerPrintSetUpManager.GetAppointmentScheduleBySlotID(View.SlotId);
            }
        }

        public void GetAppointmentAvailableData()
        {
            if (View.LocationId > AppConsts.NONE)
            {
                View.lstAppointmentSlots = new List<AppointmentSlotContract>();
      
                Boolean IsApplicant= SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).IsApplicant.GetValueOrDefault(false);
                View.lstAppointmentSlots = FingerPrintSetUpManager.GetAppointmentSlotByDate(View.LocationId,IsApplicant);
            }
        }
        //public Boolean SaveUpdateApplicantAppointment(AppointmentSlotContract scheduleAppointmentContract)
        //{
        //    return FingerPrintSetUpManager.SaveUpdateApplicantAppointment(scheduleAppointmentContract, View.CurrentLoggedInUserId);
        //}

        //public ReserveSlotContract ReserveSlot()
        //{
        //     return FingerPrintSetUpManager.ReserveSlot(View.SelectedSlotID, View.CurrentLoggedInUserId);
        //}

    }
}
