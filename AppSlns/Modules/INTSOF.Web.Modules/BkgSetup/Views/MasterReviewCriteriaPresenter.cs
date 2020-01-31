using System;
using System.Collections.Generic;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class MasterReviewCriteriaPresenter : Presenter<IMasterReviewCriteriaView>
    {
        public override void OnViewInitialized()
        {
        }


        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantID;
            }
        }


        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            if (View.SelectedTenantID == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Get list of tenants
        /// </summary>
        public void GetTenants()
        {
            View.ListTenants = BackgroundSetupManager.getClientTenant();
        }

        /// <summary>
        /// Get the list of records form BkgReviewCriteria table.
        /// </summary>
        public void GetMasterReviewCriteria()
        {
            if (View.SelectedTenantID == AppConsts.NONE)
            {
                View.ListBkgReviewCriteria = new List<Entity.ClientEntity.BkgReviewCriteria>();
            }
            else
            {
                View.ListBkgReviewCriteria = BackgroundSetupManager.FetchMasterReviewCriteria(View.SelectedTenantID);
            }
        }

        /// <summary>
        /// Add new records of BkgReviewCriteria table.
        /// </summary>
        /// <param name="reviewCriteria"></param>
        /// <returns></returns>
        public bool SaveReviewCriteria(Entity.ClientEntity.BkgReviewCriteria reviewCriteria)
        {
            return BackgroundSetupManager.SaveReviewCriteria(View.SelectedTenantID, reviewCriteria);
        }

        /// <summary>
        /// Update records of BkgReviewCriteria table.
        /// </summary>
        /// <param name="reviewCriteria"></param>
        /// <returns></returns>
        public bool UpdateReviewCriteria(Entity.ClientEntity.BkgReviewCriteria reviewCriteria)
        {
            return BackgroundSetupManager.UpdateReviewCriteria(View.SelectedTenantID, reviewCriteria, reviewCriteria.BRC_IsDeleted);
        }

        public bool IsReviewCriteriaCanBeDeleted(int revwCriteriaID)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IsReviewCriteriaMapped(revwCriteriaID, View.SelectedTenantID);
            if (response.CheckStatus == CheckStatus.True)
            {
                View.ErrorMessage = response.UIMessage;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
