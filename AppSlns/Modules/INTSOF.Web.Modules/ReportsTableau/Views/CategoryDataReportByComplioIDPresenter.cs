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
    public class CategoryDataReportByComplioIDPresenter : Presenter<ICategoryDataReportByComplioIDView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            View.Tenants = DataMartManager.GetMappedTenants(View.UserID);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.CategoryDataReportByComplioID.GetStringValue());
            BindReviewStatusAndUserType();
        }

        public void BindReviewStatusAndUserType(Boolean selectAll = true)
        {
            Dictionary<String, String> invitationReviewStatus = DataMartManager.GetInvitationReviewStatus();
            View.ReviewStatus = invitationReviewStatus;
            
            Dictionary<String, String> userTypes = DataMartManager.GetUserTypes();
            View.UserType = userTypes;

            if (selectAll)
            {
                View.SelectedReviewStatus = invitationReviewStatus.Keys.ToList();
                View.SelectedUserType = userTypes.Keys.ToList();
            }
        }

        public Boolean SaveSearchCriteria()
        {
            SavedSearch savedSearch = DataMartManager.SaveCategoryDataWithComplioIDSearchCriteria(View.SelectedSearch, View.UserID, View.SearchName, View.SearchDescription,
                   View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedCategories, View.SelectedItems, View.SelectedReviewStatus,
                   View.SelectedUserType, View.RotationStartDate.HasValue ? View.RotationStartDate.Value.ToString() : String.Empty,
                   View.RotationEndDate.HasValue ? View.RotationEndDate.Value.ToString() : String.Empty, View.ComplioID, View.CustomAttribute);
            View.SavedSearches = DataMartManager.GetSavedSearches(View.UserID, SearchType.CategoryDataReportByComplioID.GetStringValue());
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
                View.SelectedReviewStatus = savedSearch.ReviewStatus;
                View.SelectedUserType = savedSearch.UserTypes;
                View.RotationStartDate = savedSearch.RotationStartDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(savedSearch.RotationStartDate);
                View.RotationEndDate = savedSearch.RotationEndDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(savedSearch.RotationEndDate);
                View.ComplioID = savedSearch.ComplioID;
                View.CustomAttribute = savedSearch.CustomAttribute;
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

        public List<CategoryDataReportWithComplioIDContract> GetCategoryDataReportWithComplioIDContracts()
        {
            var returnData = DataMartManager.GetCategoryDataReportWithComplioIDContracts(View.UserID, View.SelectedTenantIDs, View.SelectedAgencyIDs, View.SelectedCategories, View.SelectedItems
               , View.ComplioID, View.CustomAttribute, View.RotationStartDate, View.RotationEndDate, View.SelectedReviewStatus, View.SelectedUserType);
            return returnData != null ? returnData : new List<CategoryDataReportWithComplioIDContract>();
        }
    }
}

