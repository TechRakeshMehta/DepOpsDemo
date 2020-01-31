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
    public class ItemDataCountReportPresenter : Presenter<IItemDataCountReportView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            View.Tenants = DataMartManager.GetMappedTenants(View.UserID);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.ItemDataCountReport.GetStringValue());
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
            SavedSearch savedSearch = DataMartManager.SaveItemDataCountSearchCriteria(View.SelectedSearch, View.UserID, View.SearchName, View.SearchDescription,
                   View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedCategories, View.SelectedItems,
                   View.SelectedUserType, View.FromDate.HasValue ? View.FromDate.Value.ToString() : String.Empty,
                   View.ToDate.HasValue ? View.ToDate.Value.ToString() : String.Empty, View.IsUniqueResultsOnly);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.ItemDataCountReport.GetStringValue());
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
                BindItems();
                View.SelectedItems = savedSearch.Items;
                BindReviewStatusAndUserType(false);
                View.SelectedUserType = savedSearch.UserTypes;
                View.FromDate = savedSearch.RotationStartDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(savedSearch.RotationStartDate);
                View.ToDate = savedSearch.RotationEndDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(savedSearch.RotationEndDate);
                View.IsUniqueResultsOnly= savedSearch.IsUniqueResultsOnly.IsNullOrEmpty() ? false : Convert.ToBoolean(savedSearch.IsUniqueResultsOnly);
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

        public void BindItems()
        {
            Dictionary<String, String> items = new Dictionary<String, String>();
            items = DataMartManager.GetItems(View.UserID, View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedCategories);
            View.Items = items;
        }

        public List<ItemDataCountReportContract> GetItemDataCountReportContracts()
        {
            var returnData = DataMartManager.GetItemDataCountReportContract(View.UserID, View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedCategories, View.SelectedItems
                , View.FromDate, View.ToDate, View.SelectedUserType, View.IsUniqueResultsOnly);
            return returnData != null ? returnData : new List<ItemDataCountReportContract>();
        }
    }
}

