using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class BkgPackageCopyPresenter : Presenter<IBkgPackageCopyView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
 
        }

        /// <summary>
        /// To copy package structure and data
        /// </summary>
        public void CopyPackageStructure()
        {
            //While copy/insert package, packageID should be 0 to verify Package Name exists
            if (BackgroundSetupManager.CheckIfPackageNameAlreadyExist(View.BackGroundPackageName, View.TenantId))
            {
                View.ErrorMessage = "Package Name already exists.";
                return;
            }
            View.ErrorMessage = String.Empty;
            BackgroundSetupManager.CopyBackgroundPackage(View.SourceHierarchyNodeId, View.TargetHierarchyNodeId,View.BPHM_ID,View.BackGroundPackageName, View.CurrentLoggedInUserId, View.TenantId);
        }
    }
}
