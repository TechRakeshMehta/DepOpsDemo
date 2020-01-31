using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface ISetUpServiceForServicegroupView
    {
        List<BackgroundService> lstServices { get; set; }
        String NodeId { get; set; }
        String ErrorMessage { get; set; }
        Int32 TenantId { get; set; }
        Int32 ServiceGroupId { get; }
        Int32 ServiceId { get; }
        Int32 PackageId { get; }
        String DisplayName { get; }
        String Notes { get; }

        Int32? PkgCount { get; }
        Int32? ResidenceDuration { get; }
        Int32? MinOccurrences { get; }
        Int32? MaxOccurrences { get; }
        Boolean SendDocsToStudent { get; }
        Boolean IsSupplemental { get; }
        Boolean IgnoreRHOnSupplement { get; }

        Boolean IsReportable { get; }
    }
}
