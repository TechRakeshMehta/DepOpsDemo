using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ManageShotSeriesPresenter : Presenter<IManageShotSeriesView>
    {

        public override void OnViewLoaded()
        {
            //View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void getCurrentCategoryInfo()
        {
            View.ComplianceCategory = ComplianceSetupManager.getCurrentCategoryInfo(View.CurrentCategoryId, View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 getTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Gets the value of large content for the given object id and assigns the value in the corresponding field.
        /// </summary>
        public void GetLargeContent()
        {
            LargeContent notesRecord = ComplianceSetupManager.getLargeContentRecord(View.ViewContract.ComplianceCategoryId, LCObjectType.ComplianceCategory.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.SelectedTenantId);
            if (notesRecord != null)
                View.ViewContract.ExplanatoryNotes = notesRecord.LC_Content;
        }

        public Boolean SaveSeriesInfo()
        {
            ItemSery newSeries = new ItemSery
            {
                IS_CategoryID = View.CurrentCategoryId,
                IS_IsDeleted = false,
                IS_CreatedByID = View.CurrentLoggedInUserId,
                IS_CreatedOn = DateTime.Now,
                IS_Description = View.SeriesDescription,
                IS_Details = View.SeriesDetails,
                IS_Label = View.SeriesLabel,
                IS_Name = View.SeriesName,
                IS_IsActive = View.SeriesIsActive,
                IS_IsAvailablePostApproval = View.IsAvailablePostApproval,
                IS_RuleExecutionOrder=View.RuleExecutionOrder
            };

            return ComplianceSetupManager.AddNewShotSeries(View.SelectedTenantId, newSeries);

        }
    }
}
