using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IRequestView
    {

        Int32 OpportunityId { get; set; }
        Int32 CurrentLoggedInUser { get; }
        Int32 RequestId { get; set; }
        Boolean IsSharedUser { get; }
        String PageRequested { get; set; }
        String RequestStatusCode { get; set; }
        PlacementMatchingContract OpportunityDetails { get; set; }
        RequestDetailContract RequestDetail { get; set; }
        Int32 TenantId { get; set; }
        Int32 SelectedTenantID { get; set; }
       
    }
}
