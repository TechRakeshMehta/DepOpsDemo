using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ScheduleAppointmentPresenter : Presenter<IScheduleAppointment>
    {

        public void GetFingerPrintAppointmentScheduleData()
        {
            if (!View.LocationID.IsNullOrEmpty() && View.LocationID > AppConsts.NONE )
                View.lstAppointmentContract = FingerPrintSetUpManager.GetFingerPrintAppointmentScheduleData(View.NewScheduleMasterID.IsNotNull() ? View.NewScheduleMasterID : View.ScheduleMasterID, View.LocationID);

            if (View.ScheduleMasterID == AppConsts.NONE && View.NewScheduleMasterID == AppConsts.NONE && !View.lstAppointmentContract.IsNullOrEmpty())
            {
                View.ScheduleMasterID = View.lstAppointmentContract[0].OldScheduleMasterID;
            }
        }

        public void ScheduleAppointment()
        {
            Int32 _newScheduleMasterID = FingerPrintSetUpManager.ScheduleAppointment(View.CurrentLoggedInUserID, View.AppointmentContract);

            if (View.NewScheduleMasterID == AppConsts.NONE && _newScheduleMasterID != AppConsts.NONE)
            {
                View.NewScheduleMasterID = _newScheduleMasterID;
            }
        }

        public void UpdateScheduleAppointment()
        {
            Int32 _newScheduleMasterID = FingerPrintSetUpManager.UpdateScheduleAppointment(View.AppointmentContract, View.CurrentLoggedInUserID);
            if (View.NewScheduleMasterID == 0)
            {
                View.NewScheduleMasterID = _newScheduleMasterID;
            }
        }

        public void DeleteScheduleAppointment()
        {
            Int32 _newScheduleMasterID = FingerPrintSetUpManager.DeleteScheduleAppointment(View.AppointmentContract, View.CurrentLoggedInUserID);
            if (View.NewScheduleMasterID == 0)
            {
                View.NewScheduleMasterID = _newScheduleMasterID;
            }
        }

        public void CommitScheduleAppointment()
        {
            View.ErrorMessage = FingerPrintSetUpManager.CommitScheduleAppointment(View.LocationID, View.ScheduleMasterID, View.NewScheduleMasterID, View.CurrentLoggedInUserID);
            if (View.ErrorMessage.IsNullOrEmpty())
            {
                FingerPrintSetUpManager.CallDigestionProcess(View.NewScheduleMasterID, View.CurrentLoggedInUserID,View.ScheduleMasterID);
            }
        }

        public void DiscardScheduleAppointment()
        {
            FingerPrintSetUpManager.DiscardScheduleAppointment(View.NewScheduleMasterID);
        }
    }
}
