using Business.RepoManagers;
using CoreWeb.ComplianceOperations.Views;
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
    public class CustomAppointmentFormPresenter : Presenter<ICustomAppointmentFormView>
    {
        public void GetLastBookedAppointmentDate(Int32 ScheduleID,Int32 locationId)
        {
            if (ScheduleID > AppConsts.NONE)
            {
                ScheduleInformationContract scheduleInformationContract = new ScheduleInformationContract
                {
                    ScheduleID = ScheduleID,
                    LocationID = locationId
                };
                var result = FingerPrintSetUpManager.GetLastBookedAppointmentDate(scheduleInformationContract);
                //View.LastBookedDate = new DateTime?();
                if(result!= null)
                {
                    View.LastBookedDate = result.LastBookedAppointmentDate;
                    View.PendingChangesToCommit = result.IsPendingChanges;
                }
               
            }

        }
        public Boolean IsAppointmentBookedForTheSelectedDate(DateTime AppointmentDate,Int32 scheduleId)
        {
           return FingerPrintSetUpManager.IsAppointmentBookedForTheSelectedDate(AppointmentDate, scheduleId);
           
        }
    }
}
