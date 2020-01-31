using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.FingerPrintSetUp.Views
{
  public class FingerPrintAddEditOnsiteEventPresenter : Presenter<IFingerPrintAddEditOnsiteEventView>
    {
        public void GetSelectedEventDetails()
        {
            View.OnsiteEventDetail = new ManageOnsiteEventsContract();
            View.OnsiteEventDetail = FingerPrintSetUpManager.GetSelectedEventDetails(View.SelectedEventId);
        }
        public int SaveOnsiteEvent(ManageOnsiteEventsContract onsiteEventContract)
        {
            return FingerPrintSetUpManager.SaveOnsiteEvent(onsiteEventContract, View.CurrentLoggedInUserId);
        }
        public void GetEventSlots()
        {
            View.lstEventSlots = new List<FingerPrintEventSlotContract>();
            if (!View.SelectedLocationId.IsNullOrEmpty())
                View.lstEventSlots = FingerPrintSetUpManager.GetEventSlots(View.GridCustomPaging, View.SelectedEventId);
            if (View.lstEventSlots.Count > 0)
                View.VirtualRecordCount = View.lstEventSlots.FirstOrDefault().TotalCount;
            else
                View.VirtualRecordCount = AppConsts.NONE;
        }
        public bool SaveOnsiteEventSlot(FingerPrintEventSlotContract eventSlotContract)
        {
            return FingerPrintSetUpManager.SaveOnsiteEventSlot(eventSlotContract, View.CurrentLoggedInUserId);
        }
        public bool DeleteOnsiteEventSlot(int eventSlotId)
        {
            return FingerPrintSetUpManager.DeleteOnsiteEventSlot(eventSlotId, View.CurrentLoggedInUserId);
        }
        public bool PublishEvent()
        {
            return FingerPrintSetUpManager.PublishEvent(View.SelectedEventId, View.CurrentLoggedInUserId);
        }
        public bool IsLocationMapped()
        {
            return FingerPrintSetUpManager.IsLocationMapped(View.SelectedLocationId);            
        }
    }
}
 