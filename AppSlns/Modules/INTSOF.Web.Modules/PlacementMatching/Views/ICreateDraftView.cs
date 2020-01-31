using INTSOF.ServiceDataContracts.Core;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface ICreateDraftView
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
        Dictionary<Int32, String> lstDays { get; set; }
        String TenantName { get; set; }
        Boolean IsAgencyUserLoggedIn { get; }

        List<CustomAttribteContract> CustomAttributeList
        {
            get;
            set;
        }
        List<CustomAttribteContract> SetCustomAttributeList
        {
            get;
            set;
        }
        Int32 CurrentAgencyHierarchyID
        {
            get;set;
        }
    }
}
