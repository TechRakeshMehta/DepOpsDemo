using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderProfileMappingPopupView
    {
        /// <summary>
        /// stores Tenant Id of the processing order.
        /// </summary>
        Int32 TenantID { get; set; }

        /// <summary>
        /// Stores whether the link the profile with exisiting line item or not.
        /// </summary>
        Boolean IsLinkProfile { get; set; }

        /// <summary>
        /// Store Master OrderID for the process.
        /// </summary>
        Int32 OrderID { get; set; }

        /// <summary>
        /// Store BkgOrderPackageSvcLineItemID.
        /// </summary>
        Int32 PackageServiceLineItemID { get; set; }
    }
}
