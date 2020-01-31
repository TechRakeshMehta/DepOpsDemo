using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IServiceVendorsView
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
        /// Gets the default TenantId
        /// </summary>
        Int32 TenantId
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

        /// <summary>
        /// Gets id of current logged in user
        /// </summary>
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

        IList<ExternalVendor> ServiceVendorsList
        {
            get;
            set;
        }

        VendorsDetailsContract VendorsDetailsContract
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

    }

}
