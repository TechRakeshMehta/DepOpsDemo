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
    public class RotationStudentsNonComplianceStatusPresenter : Presenter<IRotationStudentsNonComplianceStatusView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            View.Tenants = DataMartManager.GetMappedTenants(View.UserID);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.RotationStudentsOverallNonComplianceStatus.GetStringValue());
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
            SavedSearch savedSearch = DataMartManager.SaveRotationStudentsOverallNonComplianceSearchCriteria(View.SelectedSearch, View.UserID, View.SearchName, View.SearchDescription,
                   View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedCategories, View.SelectedUserType, View.IncludeUndefinedDataShares);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.RotationStudentsOverallNonComplianceStatus.GetStringValue());
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
                BindCategories();
                View.SelectedCategories = savedSearch.Categories;
                BindReviewStatusAndUserType(false);
                View.SelectedUserType = savedSearch.UserTypes;
                View.IncludeUndefinedDataShares = savedSearch.IncludeUndefinedDataShares.IsNullOrEmpty() ? false : Convert.ToBoolean(savedSearch.IncludeUndefinedDataShares);
            }
        }

        public void BindAgencies()
        {
            Dictionary<Int32, String> agencies = new Dictionary<Int32, String>();
            agencies = DataMartManager.GetAgencies(View.UserID, View.SelectedTenantIDs);
            View.Agencies = agencies;
        }

        public void BindCategories()
        {
            Dictionary<String, String> categories = new Dictionary<String, String>();
            categories = DataMartManager.GetCategories(View.UserID, View.SelectedTenantIDs, View.SelectedAgencyIDs);
            View.Categories = categories;
        }

        public List<RotationStudentsNonComplianceContract> GetRotationStudentNonComplianceStatusContracts()
        {
            var returnData = DataMartManager.GetRotationStudentNonComplianceStatusContract(View.UserID, View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedCategories
                , View.SelectedUserType, View.IncludeUndefinedDataShares);
            return returnData != null ? returnData : new List<RotationStudentsNonComplianceContract>();
        }
    }
}

