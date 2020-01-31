using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageAppointmentSlotsDetailPresenter : Presenter<IAppointmentSlotsDetailView>
    {

        public void GetAppoinmentSlots()
        {
            var result = new List<AppointmentSlotContract>();            
            if (View.SelectedLocationID > 0)
            {                
                AppointmentSlotContract serchContract = new AppointmentSlotContract
                {
                    SlotDate = View.SelectedSlotDate
                };
                result = FingerPrintSetUpManager.GetLocationAppointmentSlots(serchContract, View.SelectedLocationID);
            }
            View.AppointmentSlots = result;
        }

        public bool UpdateAppoinmentSlot()
        {
            var result = false;
            if(!View.CurrentEntity.IsNull()
                && View.CurrentEntity.SlotID > 0)
            {
                result = FingerPrintSetUpManager.UpdateAppointmentSlot(View.CurrentEntity, View.CurrentLoggedInUserId);
            }
            return result;
        }
    }
}
