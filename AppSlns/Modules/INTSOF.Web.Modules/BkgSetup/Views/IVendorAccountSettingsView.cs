using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IVendorAccountSettingsView
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

        IList<ExternalVendorAccount> VendorsAccountsList
        {
            get;
            set;
        }

        Int32 VendorId
        {
            get;
            set;
        }

        String AccountNumber
        {
            get;
            set;
        }

        String AccountName
        {
            get;
            set;
        }

        Int32 EvaID
        {
            get;
            set;
        }
    }
}
