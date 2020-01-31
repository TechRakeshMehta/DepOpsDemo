#region Namespaces

#region System Defined Namespaces

using INTSOF.SharedObjects;
using System.Linq;


#endregion

#region User Defined Namespaces

using Business.RepoManagers;


#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public class ApplicantPortfolioSubscriptionPresenter : Presenter<IApplicantPortfolioSubscriptionView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Gets tree data list
        /// </summary>
        public void GetTreeData()
        {
            View.TreeListDetail = Business.RepoManagers.ComplianceSetupManager.GetPortfolioSubscriptionTree(View.CurrentViewContext.TenantId, View.OrganizationUserId).ToList();
        }

    }
}




