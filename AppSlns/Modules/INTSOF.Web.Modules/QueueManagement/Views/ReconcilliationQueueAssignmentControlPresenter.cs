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

namespace CoreWeb.QueueManagement.Views
{
    public class ReconcilliationQueueAssignmentControlPresenter : Presenter<IReconcilliationQueueAssignmentControlView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetTenants()
        {
            //List<Entity.ClientEntity.Tenant> lstTenant = new List<Entity.ClientEntity.Tenant>();
            //if (!View.lstManageRandomReviews.IsNullOrEmpty())
            //{
            //    if (!View.SelectedTenantID.IsNullOrEmpty() && View.SelectedTenantID > AppConsts.NONE)
            //    {
            //        lstTenant.Add(ComplianceDataManager.getClientTenant().Where(f => f.TenantID == View.SelectedTenantID).FirstOrDefault());
            //    }
            //    else
            //    {
            //        foreach (var tenant in ComplianceDataManager.getClientTenant())
            //        {
            //          if (!View.lstManageRandomReviews.Where(f => f.TenantID == tenant.TenantID).Any())
            //          {
            //              lstTenant.Add(tenant);
            //          }
            //        }
            //    }
            //}
            //else
            //{
            //lstTenant.AddRange(ComplianceDataManager.getClientTenant());
            //}
            View.lstTenant = ComplianceDataManager.getClientTenant();
        }

        public void SaveAssignmentConfiguration()
        {

        }

        private ReconciliationQueueConfiguration CreateAssignmentConfiguration()
        {
            ReconciliationQueueConfiguration queueAssignmentConfiguration = new ReconciliationQueueConfiguration();
            return queueAssignmentConfiguration;
        }

        public void GetCurrentReconciliationAssignmentConfiguration()
        {

        }

        #region UAT-4114
        public void GetManageRandomReviewsList()
        {
            View.lstManageRandomReviews = SecurityManager.GetAllManageRandomReviewsList();
        }
        public String SaveReconciliationQueueConfiguration()
        {
            return ComplianceDataManager.SaveReconciliationQueueConfiguration(View.CurrentAssignmentConfigurationId, View.SelectedTenantID, View.AssignmentDescription, View.Percentage, View.NumberOfReviews, View.InstitutionHierarchyID, View.CurrentLoggedInUserId);
        }
        public Boolean DeleteReconciliationQueueConfiguration()
        {
            return ComplianceDataManager.DeleteReconciliationQueueConfiguration(View.SelectedTenantID,View.CurrentAssignmentConfigurationId, View.CurrentLoggedInUserId);
        }
        public Boolean IsHierarchyNodeSettingAlreadyExists()
        {
            Int32 InstitutionHierarchyID = Convert.ToInt32(View.InstitutionHierarchyID);
            return ComplianceDataManager.IsHierarchyNodeSettingAlreadyExists(View.SelectedTenantID, View.CurrentAssignmentConfigurationId, InstitutionHierarchyID);
        }
        #endregion
    }
}

