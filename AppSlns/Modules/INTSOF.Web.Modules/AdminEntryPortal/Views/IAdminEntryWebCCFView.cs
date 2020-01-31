using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public interface IAdminEntryWebCCFView
    {
        String StateAbbreviation { get; set; }
        String ZipCode { get; set; }

        List<Int32> BackgroundPackageIdList { get; set; }

        String ClearStarServiceId { get; set; }

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 TenantId { get; set; }
        List<Int32> BPHMIds { get; set; }
        String ExtVendorAccountNumber { get; set; }
        String IsUSCitizen { get; set; }
        //UAT-3056
        Int32 SelectedNodeId { get; set; }
    }
}
