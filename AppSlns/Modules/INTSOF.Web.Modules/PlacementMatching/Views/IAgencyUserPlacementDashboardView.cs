using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IAgencyUserPlacementDashboardView
    {
        Guid UserId { get; }
        String SelectedStatusCode { get; set; }
        List<RequestDetailContract> lstRequests { get; set; }
        List<RequestDetailContract> lstAllRequests { get; set; }
        RequestDetailContract SearchRequestContract { get; set; }
        List<RequestStatusContract> lstRequestStatus { get; set; }
        Int32 SelectedStatusID { get; set; }
        Int32 AgencyHierarchyRootNodeID { get; set; }
        DateTime? FromDate { get; set; }
        DateTime? ToDate { get; set; }
        List<InstitutionRequestPieChartContract> lstInstitutionRequestsApproved { get; set; }
    }
}
