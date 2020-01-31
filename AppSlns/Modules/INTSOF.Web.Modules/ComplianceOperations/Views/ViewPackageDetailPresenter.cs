#region Namespaces

#region System Defined Namespaces

using INTSOF.SharedObjects;
using System.Linq;


#endregion

#region User Defined Namespaces

using Business.RepoManagers;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class ViewPackageDetailPresenter : Presenter<IViewPackageDetailView>
    {
        #region Public Methods

        /// <summary>
        /// Sets the TenantID of Current LoggedIn User in property TenantId.
        /// </summary>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        /// <summary>
        /// Called when viwe is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Gets the assigned tree data from database and sets it in property AssignedTreeData.
        /// </summary>
        public void GetTreeData()
        {
            View.TreeListPackagesDetail = ComplianceSetupManager.GetPackageDetailTree(View.CurrentViewContext.ManageTenantId, View.PackageID).ToList();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}









