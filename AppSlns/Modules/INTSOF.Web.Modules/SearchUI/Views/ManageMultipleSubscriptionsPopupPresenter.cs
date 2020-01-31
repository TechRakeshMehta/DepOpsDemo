using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;

namespace CoreWeb.SearchUI.Views
{
    public class ManageMultipleSubscriptionsPopupPresenter : Presenter<IManageMultipleSubscriptionsPopup>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetMultipleSubscriptionDataForPopup()
        {
            String multpleSubscriptionIDs = String.Join(",", View.MultpleSubscriptionIDs);
            View.lstMultipleSubscriptionsData = ComplianceDataManager.GetMultipleSubscriptionDataForPopup(View.SelectedTenantID, multpleSubscriptionIDs, View.CurrentLoggedInUserID);
        }

        public String ArchieveSubscriptions()
        {
            return ComplianceDataManager.ArchieveSubscriptionsManually(View.SelectedSubscriptions, View.SelectedTenantID, View.CurrentLoggedInUserID);
            //{
            //    View.ErrorMessage = String.Empty;
            //}
            //else
            //{
            //    View.ErrorMessage = "Subscriptions can not be archive at this time. Please try again.";
            //}
        }
    }
}
