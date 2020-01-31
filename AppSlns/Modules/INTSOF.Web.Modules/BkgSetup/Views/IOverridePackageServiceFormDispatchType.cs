using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IOverridePackageServiceFormDispatchType
    {
        /// <summary>
        /// Id of the Tenant Selected by the admin
        /// </summary>
        Int32 SelectedTenantId { get; set; }

        /// <summary>
        /// Id of the Service selected by the admin
        /// </summary>
        Int32 BkgSvcId { get; set; }

        /// <summary>
        /// CompliancePackageId
        /// </summary>
        Int32 PackageId { get; set; }

        /// <summary>
        /// </summary>
        Int32 BPSId { get; set; }
    }
}
