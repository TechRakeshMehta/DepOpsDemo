using DataMart.Business.RepoManagers;
using DataMart.Models;
using DataMart.UI.Contracts;
using DataMart.Utils;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreWeb.ReportsTableau.Views
{
    public class RotationStudentsByDayPresenter : Presenter<IRotationStudentsByDayView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            View.Tenants = DataMartManager.GetMappedTenants(View.UserID);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.RotationStudentsByDay.GetStringValue());
            BindReviewStatusAndUserType();
        }

        public void BindReviewStatusAndUserType(Boolean selectAll = true)
        {
            Dictionary<String, String> userTypes = DataMartManager.GetUserTypes();
            View.UserType = userTypes;
            

            Dictionary<String, String> reviewStatus = DataMartManager.GetInvitationReviewStatus();
            View.ReviewStatus = reviewStatus;
            View.SelectedReviewStatus = reviewStatus.Keys.ToList();

            Dictionary<String, String> days = new Dictionary<string, string>();
            days.Add("Monday", "Monday");
            days.Add("Tuesday", "Tuesday");
            days.Add("Wednesday", "Wednesday");
            days.Add("Thursday", "Thursday");
            days.Add("Friday", "Friday");
            days.Add("Saturday", "Saturday");
            days.Add("Sunday", "Sunday");
            View.Days = days;
            if (selectAll)
            {
                View.SelectedReviewStatus = reviewStatus.Keys.ToList();
                View.SelectedDays = days.Keys.ToList();
                View.SelectedUserType = userTypes.Keys.ToList();
            }
        }

        public Boolean SaveSearchCriteria()
        {
            SavedSearch savedSearch = DataMartManager.SaveRotationStudentsByDaySearchCriteria(View.SelectedSearch, View.UserID, View.SearchName, View.SearchDescription,
                   View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedReviewStatus, View.SelectedUserType, View.FromDate.HasValue ? View.FromDate.Value.ToString() : String.Empty,
                   View.ToDate.HasValue ? View.ToDate.Value.ToString() : String.Empty, View.SelectedDays);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.RotationStudentsByDay.GetStringValue());
            View.SelectedSearch = savedSearch.ID;

            return true;
        }

        public void FillSearchCriteria()
        {
            SavedSearch savedSearch = DataMartManager.GetSavedSearchDetails(View.SelectedSearch);
            if (savedSearch.IsNotNull())
            {
                View.SearchName = savedSearch.SearchName;
                View.SearchDescription = savedSearch.SearchDescription;
                View.Tenants = DataMartManager.GetMappedTenants(View.UserID);
                View.SelectedTenantIDs = savedSearch.Institutes;
                BindAgencies();
                View.SelectedAgencyIDs = savedSearch.Agencies;
                BindReviewStatusAndUserType(false);
                View.SelectedUserType = savedSearch.UserTypes;
                View.SelectedReviewStatus = savedSearch.ReviewStatus;
                View.FromDate = savedSearch.RotationStartDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(savedSearch.RotationStartDate);
                View.ToDate = savedSearch.RotationEndDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(savedSearch.RotationEndDate);
                View.SelectedDays = savedSearch.Days;
            }
        }

        public void BindAgencies()
        {
            Dictionary<Int32, String> agencies = new Dictionary<Int32, String>();
            agencies = DataMartManager.GetRotationDetailAgencies(View.UserID, View.SelectedTenantIDs);
            View.Agencies = agencies;
        }

        public List<RotationStudentsByDayContract> GetRotationStudentsByDayContract()
        {
            var returnData = DataMartManager.GetRotationStudentsByDayContracts(View.UserID, View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedReviewStatus
                , View.SelectedUserType, View.FromDate, View.ToDate, View.SelectedDays);
            return returnData != null ? returnData : new List<RotationStudentsByDayContract>();
        }
    }
}

