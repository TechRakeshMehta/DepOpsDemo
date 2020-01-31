using Business.RepoManagers;
using Entity;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class SupplementAutomationConfigurationQueuePresenter : Presenter<ISupplementAutomationConfigurationQueueView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetTenants();
        }

        public void GetTenants()
        {
            View.lstTenant = ComplianceDataManager.getClientTenant();
        }

        /// <summary>
        /// Save/Update Supplement Automation Configuration
        /// </summary>
        public void SaveSupplementAutomationConfiguration()
        {
            SupplementAutomationConfiguration supplementAutomationConfiguration = CreateSupplementAutomationConfiguration();
            BackgroundSetupManager.SaveUpdateSupplementAutomationConfiguration(supplementAutomationConfiguration, View.CurrentLoggedInUserId);
            View.SupplementAutomationConfigurationId = supplementAutomationConfiguration.SAC_ID;
        }

        /// <summary>
        /// Create Supplement Automation Configuration
        /// </summary>
        /// <returns></returns>
        private SupplementAutomationConfiguration CreateSupplementAutomationConfiguration()
        {
            SupplementAutomationConfiguration supplementAutomationConfiguration = new SupplementAutomationConfiguration();
            supplementAutomationConfiguration.SAC_ID = View.SupplementAutomationConfigurationId;
            supplementAutomationConfiguration.SAC_Description = View.Description;
            supplementAutomationConfiguration.SAC_RecordPercentage = View.Percentage;
            supplementAutomationConfiguration.SAC_IsDeleted = false;
            supplementAutomationConfiguration.SAC_CreatedBy = View.CurrentLoggedInUserId;
            supplementAutomationConfiguration.SAC_CreatedOn = DateTime.Now;
            foreach (Int32 tenantID in View.MappedTenantIds)
            {
                supplementAutomationConfiguration.SupplementAutomationConfigurationTenantMappings.Add(
                    new SupplementAutomationConfigurationTenantMapping()
                    {
                        SACTM_IsDeleted = false,
                        SACTM_CreatedOn = DateTime.Now,
                        SACTM_TenantID = tenantID,
                        SACTM_CreatedBy = View.CurrentLoggedInUserId
                    });
            }
            return supplementAutomationConfiguration;
        }

        /// <summary>
        /// Get Current Supplement Automation Configuration
        /// </summary>
        public void GetCurrentSupplementAutomationConfiguration()
        {
            SupplementAutomationConfiguration currentSupplementAutomationConfiguration = BackgroundSetupManager.GetCurrentSupplementAutomationConfiguration();
            if (!currentSupplementAutomationConfiguration.IsNullOrEmpty())
            {
                View.Description = currentSupplementAutomationConfiguration.SAC_Description;
                View.Percentage = currentSupplementAutomationConfiguration.SAC_RecordPercentage.HasValue ? currentSupplementAutomationConfiguration.SAC_RecordPercentage.Value : 0;
                View.MappedTenantIds = currentSupplementAutomationConfiguration.SupplementAutomationConfigurationTenantMappings.IsNullOrEmpty()
                                                                ? new List<Int32>()
                                                                : currentSupplementAutomationConfiguration.SupplementAutomationConfigurationTenantMappings
                                                                .Where(cond => !cond.SACTM_IsDeleted)
                                                                    .Select(cond => cond.SACTM_TenantID).ToList();
                View.SupplementAutomationConfigurationId = currentSupplementAutomationConfiguration.SAC_ID;
            }
        }

    }
}

