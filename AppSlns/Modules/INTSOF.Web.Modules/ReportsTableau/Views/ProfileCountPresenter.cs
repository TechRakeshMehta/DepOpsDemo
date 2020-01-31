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
    public class ProfileCountPresenter : Presenter<IProfileCountView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            View.Tenants = DataMartManager.GetMappedTenants(View.UserID);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.ProfileCount.GetStringValue());
            BindReviewStatusAndUserType();
        }

        public void BindReviewStatusAndUserType(Boolean selectAll = true)
        {
            Dictionary<String, String> userTypes = DataMartManager.GetUserTypes();
            View.UserType = userTypes;
            if(selectAll)
            View.SelectedUserType = userTypes.Keys.ToList();
        }

        public Boolean SaveSearchCriteria()
        {
            SavedSearch savedSearch = DataMartManager.SaveProfileCountSearchCriteria(View.SelectedSearch, View.UserID, View.SearchName, View.SearchDescription,
                   View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedUserType, View.FromDate.HasValue ? View.FromDate.Value.ToString() : String.Empty,
                   View.ToDate.HasValue ? View.ToDate.Value.ToString() : String.Empty, View.IncludeUndefinedDataShares, View.IsUniqueResultsOnly);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.ProfileCount.GetStringValue());
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
                View.IncludeUndefinedDataShares = savedSearch.IncludeUndefinedDataShares.IsNullOrEmpty() ? false : Convert.ToBoolean(savedSearch.IncludeUndefinedDataShares);
                View.FromDate = savedSearch.RotationStartDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(savedSearch.RotationStartDate);
                View.ToDate = savedSearch.RotationEndDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(savedSearch.RotationEndDate);
                View.IsUniqueResultsOnly = savedSearch.IsUniqueResultsOnly.IsNullOrEmpty() ? false : Convert.ToBoolean(savedSearch.IsUniqueResultsOnly);
            }
        }

        public void BindAgencies()
        {
            Dictionary<Int32, String> agencies = new Dictionary<Int32, String>();
            agencies = DataMartManager.GetRotationDetailAgencies(View.UserID, View.SelectedTenantIDs);
            View.Agencies = agencies;
        }

        public Dictionary<String, Int32> GetProfileCountContracts()
        {
            var returnData = DataMartManager.GetProfileCountContracts(View.UserID, View.SelectedTenantIDs, View.SelectedAgencyIDs
                , View.SelectedUserType, View.FromDate, View.ToDate, View.IncludeUndefinedDataShares, View.IsUniqueResultsOnly);
            return returnData != null ? returnData : new Dictionary<String, Int32>();
        }
    }
}

