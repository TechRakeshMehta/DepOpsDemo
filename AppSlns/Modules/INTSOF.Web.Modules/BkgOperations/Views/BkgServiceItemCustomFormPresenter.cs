using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgServiceItemCustomFormPresenter : Presenter<IBkgServiceItemCustomFormView>
    {
        /// <summary>
        /// Gets the Supplemental Service for a particular OrderPackageServiceGroup
        /// </summary>
        public void GetSupplementServices()
        {
            //View.lstSupplementServiceList = BackgroundProcessOrderManager.GetSupplementServices(View.MasterOrderID, View.OrderPkgSvcGroupId, View.SelectedTenantID);
            View.lstSupplementServiceList = BackgroundProcessOrderManager.GetSupplementServices(View.MasterOrderID, View.SelectedTenantID);
        }

        public void GetSupplementServiceItem(Int32 serviceId)
        {
            View.lstSupplementServiceItemList = BackgroundProcessOrderManager.GetSupplementServiceItem(View.SelectedTenantID, View.MasterOrderID, serviceId);
        }

        public SourceServiceDetailForSupplement CheckSourceServicesForSupplement()
        {
          return  BackgroundProcessOrderManager.CheckSourceServicesForSupplement(View.SelectedTenantID, View.MasterOrderID);
        }
    }
}
