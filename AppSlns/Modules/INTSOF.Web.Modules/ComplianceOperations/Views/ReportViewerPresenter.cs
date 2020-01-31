using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ReportViewerPresenter : Presenter<IReportViewerView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        // TODO: Handle other view events and set state in the view

        //public void GetPackageSubscription()
        //{
        //  View.ListPackageSubscription = ComplianceDataManager.GetPackageSubscription(View.TenantID, View.SubscriptionIDs);
        //}
    }
}
