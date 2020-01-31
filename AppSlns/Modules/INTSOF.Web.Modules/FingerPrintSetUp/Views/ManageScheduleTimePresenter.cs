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
   public class ManageScheduleTimePresenter : Presenter<IManageScheduleTimeView>
    {
        public void GetTimeOffDataBasedOnLocationId()
        {
            View.lstSchedlueTimeOff = new List<ManageScheduleTimeContract>();
            View.lstSchedlueTimeOff = FingerPrintSetUpManager.GetManageScheduleTimeData( View.SelectedLocationId);
            
            if (View.lstSchedlueTimeOff.IsNullOrEmpty())
                View.VirtualRecordCount = AppConsts.NONE;
            else
                View.VirtualRecordCount = View.lstSchedlueTimeOff.Count;
        }

        public Boolean SaveTimeOffData(ManageScheduleTimeContract manageScheduleTimeContract)
        {
            manageScheduleTimeContract.LocationId = View.SelectedLocationId;
            return FingerPrintSetUpManager.SaveAndUpdateTimeOff(manageScheduleTimeContract, View.CurrentLoggedInUserID);
        }
        public Boolean DeleteTimeOff(ManageScheduleTimeContract manageScheduleTimeContract)
        {
            return FingerPrintSetUpManager.DeleteTimeOff(View.CurrentLoggedInUserID, manageScheduleTimeContract);
        }

        public Int32 PublishTimeOff(ManageScheduleTimeContract manageScheduleTimeContract)
        {
            manageScheduleTimeContract.LocationId = View.SelectedLocationId;
            return FingerPrintSetUpManager.PublishTimeOff(View.CurrentLoggedInUserID, manageScheduleTimeContract);
        }
    }
}
