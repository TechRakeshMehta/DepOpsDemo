using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.Utils.Consts;

namespace CoreWeb.BkgOperations.Views
{
    public class ServiceLevelDetailsForOrderPresenter : Presenter<IServiceLevelDetailsForOrderView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        public void GetServiceLevelDetailsForOrder()
        {
            if (View.SelectedTenantId > 0)
            {
                View.LstServiceDetails = BackgroundProcessOrderManager.GetServiceLevelDetailsForOrder(View.SelectedTenantId, View.OrderID);
            }
        }

        public OrganizationUser GetOrganizationUserByOrderID()
        {
            return BackgroundProcessOrderManager.GetOrganisationUserByOrderId(View.SelectedTenantId, View.OrderID);

        }
    }
}

