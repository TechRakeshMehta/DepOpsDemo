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
    public class ShotSeriesListingPresenter : Presenter<IShotSeriesListingView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetItemShotSeries()
        {
            View.CurrentViewContext.LstShotSeries = ComplianceSetupManager.GetItemShotSeries(View.CategoryID, View.CurrentViewContext.SelectedTenantId);
        }

        public Boolean DeleteItemSeries(Int32 itemSeriesId)
        {
            return ComplianceSetupManager.DeleteItemSeries(itemSeriesId, View.CurrentViewContext.CurrentLoggedInUserId, View.CurrentViewContext.SelectedTenantId);
        }
    }
}