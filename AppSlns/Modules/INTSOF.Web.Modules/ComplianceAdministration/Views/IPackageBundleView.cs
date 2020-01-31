using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract;
namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IPackageBundleView
    {

        Int32 TenantId { get; set; }
        Int32 ID { get; set; }
        Int32 ParentID { get; set; }

        List<INTSOF.UI.Contract.ComplianceOperation.PackageBundleContract> ListPackageBundle { get; set; }

        Boolean IsBundleExclusive { get; set; }

        Int32 CurrentLoggedInUserId { get; }
    }
}
