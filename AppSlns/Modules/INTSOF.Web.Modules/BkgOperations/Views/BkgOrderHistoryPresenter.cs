using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderHistoryPresenter : Presenter<IBkgOrderHistoryView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {

        }

        public void GetOrderEventHistory()
        {
            List<OrderEventHistoryContract> tempLstOrderEventHistory = BackgroundProcessOrderManager.GetOrderEventHistory(View.OrderID, View.SelectedTenantId);
            View.lstOrderEventHistory = tempLstOrderEventHistory;
        }


    }
}