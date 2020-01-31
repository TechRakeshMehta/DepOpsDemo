using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public class ViewBkgPackageDetailPresenter:  Presenter<IViewBkgPackageDetailView>
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
        /// Gets the bkgPackage tree data from database and sets it in property TreeListBkgPackagesDetail.
        /// </summary>
        public void GetTreeData()
        {
            View.TreeListBkgPackagesDetail = ComplianceSetupManager.GetBkgPackageDetailTree(View.CurrentViewContext.ManageTenantId, View.BackroundPackageID).ToList();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
