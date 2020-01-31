using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IBkgPackageServiceDetailsView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        Int32 OrderID { get; set; }
        List<ServiceLevelDetailsForOrderContract> LstServiceDetails { get; set; }
    }
}
