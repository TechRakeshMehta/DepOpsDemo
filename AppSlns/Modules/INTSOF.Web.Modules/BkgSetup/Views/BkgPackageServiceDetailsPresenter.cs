using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class BkgPackageServiceDetailsPresenter : Presenter<IBkgPackageServiceDetailsView>
    {



        public void GetServiceLevelDetailsForOrder()
        {
            if (View.TenantId > 0)
            {
                View.LstServiceDetails = BackgroundProcessOrderManager.GetServiceLevelDetailsForOrder(View.TenantId, View.OrderID);
            }
        }

    }
}
