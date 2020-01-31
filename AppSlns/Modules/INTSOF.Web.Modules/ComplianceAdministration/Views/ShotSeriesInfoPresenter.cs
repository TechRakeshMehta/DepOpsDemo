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
    public class ShotSeriesInfoPresenter : Presenter<IShotSeriesInfoView>
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

        public void GetCurrentItemSeriesInfo()
        {
            View.CurrentItemSeries = ComplianceSetupManager.GetCurrentItemSeriesInfo(View.CurrentSeriesID, View.SelectedTenantId);
        }

        public void UpdateItemSeries()
        {
            ItemSery newSeries = new ItemSery
            {
                IS_ModifiedByID = View.CurrentLoggedInUserId,
                IS_ModifiedOn = DateTime.Now,
                IS_Description = View.SeriesDescription,
                IS_Details = View.SeriesDetails,
                IS_Label = View.SeriesLabel,
                IS_Name = View.SeriesName,
                IS_IsActive = View.SeriesIsActive,
                IS_ID = View.CurrentSeriesID,
                IS_IsAvailablePostApproval=View.IsAvailablePostApproval,
                IS_RuleExecutionOrder=View.RuleExecutionOrder
            };
            ComplianceSetupManager.UpdateItemSeries(View.SelectedTenantId, newSeries);
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 getTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }
    }
}
