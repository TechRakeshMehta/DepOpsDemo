using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgServiceItemCustomFormView
    {
        Int32 MasterOrderID { get; }
        Int32 SelectedTenantID { get; }
        List<SupplementServicesInformation> lstSupplementServiceList { get; set; }
        List<SupplementServiceItemInformation> lstSupplementServiceItemList { get; set; }

        /// <summary>
        /// OrderPackageServiceGroupId i.e. PK of ams.BkgorderPackageSvcGroup table,
        /// Helps to fetch the Supplement services for the selected Service Group
        /// </summary>
        Int32 OrderPkgSvcGroupId { get; set; }

        List<Int32> SelectedSupplementServices { get; set; }

        List<Int32> SetSupplementServicesToSession { set; }
    }
}
