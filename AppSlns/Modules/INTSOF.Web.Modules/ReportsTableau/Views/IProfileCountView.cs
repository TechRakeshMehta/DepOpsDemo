using System;
using System.Collections.Generic;

namespace CoreWeb.ReportsTableau.Views
{
    public interface IProfileCountView
    {
        String UserID { get; set; }
        Dictionary<Int32, String> Tenants { set; }
        Dictionary<Int32, String> Agencies { set; }
        Dictionary<String, String> UserType { set; }
        List<Int32> SelectedTenantIDs { get; set; }
        List<Int32> SelectedAgencyIDs { get; set; }
        List<String> SelectedUserType { get; set; }
        Boolean IncludeUndefinedDataShares { get; set; }
        DateTime? FromDate { get; set; }
        DateTime? ToDate { get; set; }
        Dictionary<String, String> SavedSearches { set; }
        String SelectedSearch { get; set; }
        String SearchName { get; set; }
        String SearchDescription { get; set; }
        Boolean IsUniqueResultsOnly { get; set; }
    }
}
