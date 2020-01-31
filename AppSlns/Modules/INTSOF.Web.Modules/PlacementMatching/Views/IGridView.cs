using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
   public interface IGridView
    {
        Int32 OpportunityId { get; set; }
        Int32 RequestId { get; set; }
        List<RequestDetailContract> lstPlacementMaching { get; set; }
        Int32 AgencyHierarchyID { get; set; }
        String StatusCode { get; set; }
    
    }
}
