using System;
using System.Collections.Generic;

namespace CoreWeb.ReportsTableau.Views
{
    public interface IRotationStudentsByDayView
    {
        String UserID { get; set; }

        Dictionary<Int32, String> Tenants { set; }
        Dictionary<Int32, String> Agencies { set; }
        Dictionary<String, String> UserType { set; }
        Dictionary<String, String> ReviewStatus { set; }
        Dictionary<String, String> Days { set; }
        List<Int32> SelectedTenantIDs { get; set; }

        List<Int32> SelectedAgencyIDs { get; set; }

        List<String> SelectedUserType { get; set; }

        List<String> SelectedReviewStatus { get; set; }

        List<String> SelectedDays { get; set; }

        DateTime? FromDate { get; set; }
        DateTime? ToDate { get; set; }

        Dictionary<String, String> SavedSearches { set; }

        String SelectedSearch { get; set; }

        String SearchName { get; set; }

        String SearchDescription { get; set; }
    }
}
