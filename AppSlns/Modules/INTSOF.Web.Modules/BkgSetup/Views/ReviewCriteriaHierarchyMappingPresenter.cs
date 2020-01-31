using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;

namespace CoreWeb.BkgSetup.Views
{
    public class ReviewCriteriaHierarchyMappingPresenter : Presenter<IReviewCriteriaHierarchyMappingView>
    {
        #region Methods

        #region Public Methods
        public void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void OnViewLoad()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }


        public void GetMappedReviewCriteria()
        {
            List<BkgReviewCriteriaHierarchyMapping> tempMappedList = new List<BkgReviewCriteriaHierarchyMapping>();
            if (View.TenantId > 0)
                tempMappedList = BackgroundSetupManager.GetMappedReviewCriteriaList(View.DeptProgramMappingID, View.TenantId);
            View.MappedReviewCriteriaList = tempMappedList;
            if (tempMappedList != null && tempMappedList.Count > 0)
                View.MappedReviewCriteriaIds = tempMappedList.Select(slct => slct.BRCHM_BkgReviewCriteriaID).ToList();
        }

        public void GetReviewCriteria()
        {
            List<BkgReviewCriteria> tempReviewCriteriaList = new List<BkgReviewCriteria>();
            if (View.TenantId > 0)
                tempReviewCriteriaList = BackgroundSetupManager.FetchMasterReviewCriteria(View.TenantId).Where(x => !View.MappedReviewCriteriaIds.Contains(x.BRC_ID)).ToList();
            View.ReviewCriteriaList = tempReviewCriteriaList;
        }

        public Boolean SaveMapping()
        {
            Boolean status = false;
            if (View.TenantId > 0)
                status = BackgroundSetupManager.SaveReviewCriteriaMapping(View.ReviewCriteriaIDList, View.CurrentLoggedInUserId, View.DeptProgramMappingID, View.TenantId);
            return status;
        }

        public Boolean DeleteMapping(Int32 BRCHM_ID)
        {
            if (View.TenantId > 0)
                return BackgroundSetupManager.DeleteReviewCriteriaMapping(View.CurrentLoggedInUserId, BRCHM_ID, View.TenantId);
            return false;
        }
        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}
