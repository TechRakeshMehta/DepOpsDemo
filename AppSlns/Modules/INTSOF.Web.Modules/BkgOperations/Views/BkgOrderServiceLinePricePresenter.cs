using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderServiceLinePricePresenter : Presenter<IBkgOrderServiceLinePriceView>
    {
        /// <summary>
        /// Method is used to get the ServiceLine Price by OrderID
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns>List of BackroundOrderServiceLinePrice</returns>
        public OrderServiceLineItemPriceInfo GetBackroundOrderServiceLinePriceByOrderID()
        {
            return BackgroundProcessOrderManager.GetBackroundOrderServiceLinePriceByOrderID(View.SelectedTenantId, View.OrderID);
        }
    }
}
