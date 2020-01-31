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
   public class EventAppointmentSchedulerPresenter: Presenter<IEventAppointmentSchedulerView>
    {

       //public void GetAppointmentScheduleBySlotID()
       //{
          
       //        View.appointmentSlotContract = new AppointmentSlotContract();
       //        View.appointmentSlotContract = FingerPrintSetUpManager.GetEventAppointmentScheduleBySlotID(18133);
          
       //}

        public void GetAppointmentAvailableData()
        {
           
                View.lstAppointmentSlots = new List<AppointmentSlotContract>();
                View.lstAppointmentSlots = FingerPrintSetUpManager.GetEventAppointmentSlotsAvailable(View.EventSlotId);
           
        }

    }
}
