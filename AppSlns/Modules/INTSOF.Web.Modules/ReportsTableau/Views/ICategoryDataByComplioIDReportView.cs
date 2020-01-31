using System;
using System.Collections.Generic;

namespace CoreWeb.ReportsTableau.Views
{
    public interface ICategoryDataReportByComplioIDView
    {
        String UserID { get; set; }

        Dictionary<Int32, String> Tenants { set; }
        Dictionary<Int32, String> Agencies { set; }
        Dictionary<String, String> Categories { set; }
        Dictionary<String, String> Items { set; }
        Dictionary<String, String> ReviewStatus { set; }
        Dictionary<String, String> UserType { set; }
        List<Int32> SelectedTenantIDs { get; set; }

        List<Int32> SelectedAgencyIDs { get; set; }

        List<String> SelectedCategories { get; set; }

        List<String> SelectedReviewStatus { get; set; }

        List<String> SelectedUserType { get; set; }

        List<String> SelectedItems { get; set; }

        DateTime? RotationStartDate { get; set; }

        DateTime? RotationEndDate { get; set; }

        Dictionary<String, String> SavedSearches { set; }

        String SelectedSearch { get; set; }

        String SearchName { get; set; }

        String SearchDescription { get; set; }

        string ComplioID { get; set; }

        string CustomAttribute { get; set; }
    }
}
