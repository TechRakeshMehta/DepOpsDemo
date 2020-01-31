using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderDetailPresenter : Presenter<IBkgOrderDetailView>
    {


        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public OrderDetailMain GetOrderDetailMenuItem()
        {
            return BackgroundProcessOrderManager.GetOrderDetailMenuItem(View.SelectedTenantId, View.OrderID);
        }
    }
}
