using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IOrderClientStatusView
    {
        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> TenantsList
        {
            set;
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Gets the TenantId
        /// </summary>
        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        String OrderClientStatusTypeName
        {
            get;
            set;
        }

        Boolean IsOrderClientStatusSaved
        {
            get;
            set;
        }

        List<BkgOrderClientStatu> OrderClientStatusList
        {
            get;
            set;
        }

        List<OrderClientStatusContract> OrderClientStatusContractList
        {
            get;
            set;
        }

        OrderClientStatusContract ViewContract
        {
            get;
        }

        Int32 OrderClientStatusId
        {
            get;
            set;
        }
    }
}
