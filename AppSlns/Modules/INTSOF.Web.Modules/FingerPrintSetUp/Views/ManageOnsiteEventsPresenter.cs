using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreWeb.FingerPrintSetUp.Views
{
   public class ManageOnsiteEventsPresenter : Presenter<IManageOnsiteEventsView>
    {
        public void GetOnsiteEvents()
        {
            View.GridCustomPaging.DefaultSortExpression = "LocationEventId";
            View.GridCustomPaging.SecondarySortExpression = "LocationId"; 
            View.lstEvents = new List<ManageOnsiteEventsContract>();
            View.lstEvents = FingerPrintSetUpManager.GetOnsiteEvents(View.GridCustomPaging, View.LocationId);
            if (View.lstEvents.IsNullOrEmpty())
                View.VirtualRecordCount = AppConsts.NONE;
            else               
            {
                View.VirtualRecordCount = View.lstEvents.FirstOrDefault().TotalCount;
                View.CurrentPageIndex = View.lstEvents.FirstOrDefault().CurrentPageIndex;
            }
        }

        public bool DeleteOnsiteEvent(int eventId)
        {
            return FingerPrintSetUpManager.DeleteOnsiteEvent(eventId, View.LocationId,View.CurrentLoggedInUserID);
        }
       
    }
}
