using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IVendorServicesView
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

        Int32 VendorID
        {
            get;
            set;
        }

        IList<Entity.ExternalBkgSvc> ExternalBkgServices
        {
            get;
            set;
        }

        IList<Entity.ExternalBkgSvcAttribute> ExternalBkgServiceAttributes
        {
            get;
            set;
        }
    }
}
