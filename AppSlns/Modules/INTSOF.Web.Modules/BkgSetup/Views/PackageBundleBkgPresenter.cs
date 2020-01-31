#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class PackageBundleBkgPresenter : Presenter<IPackageBundleBkgView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetBkgPackageIncludedInBundle()
        {
            View.ListPackageBundle = ComplianceSetupManager.GetPackageIncludedInBundle(View.TenantId, View.ID);
        }

        public void GetPackageBundleNodeMapping()
        {
            PackageBundleNodeMapping packageBundleNodeMapping = ComplianceSetupManager.GetPackageBundleNodeMapping(View.TenantId, View.ID, View.ParentID);
            if (packageBundleNodeMapping.IsNotNull())
            {
                View.IsBundleExclusive = packageBundleNodeMapping.PBNM_IsExclusive.HasValue ? packageBundleNodeMapping.PBNM_IsExclusive.Value : false;
            }
        }
        public Boolean UpdatePackageBundleNodeMapping()
        {
            return ComplianceSetupManager.UpdatePackageBundleNodeMapping(View.TenantId, View.ID, View.ParentID, View.IsBundleExclusive, View.CurrentLoggedInUserId);
        }
    }
}
