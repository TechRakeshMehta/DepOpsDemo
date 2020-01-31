using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class CategoryPackageListingPresenter : Presenter<ICategoryPackageListingView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetPackagesRelatedToCategory()
        {
            View.LstPackages = ComplianceSetupManager.GetPackagesRelatedToCategory(View.CategoryID, View.SelectedTenantId);
        }
    }
}
