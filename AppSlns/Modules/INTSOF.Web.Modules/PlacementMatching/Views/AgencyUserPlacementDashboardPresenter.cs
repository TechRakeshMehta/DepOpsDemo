using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.PlacementMatching.Views
{
    public class AgencyUserPlacementDashboardPresenter : Presenter<IAgencyUserPlacementDashboardView>
    {
        public List<Entity.SharedDataEntity.lkpDurationOption> GetDurationOptions()
        {
            return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpDurationOption>().Where(cond => !cond.DO_IsDeleted).ToList();
        }

        public void GetSelectedStatusTypeRequests()
        {
            //Get all the requests whose status is not draft. Same method should be called as on Manage Request Screen.
            View.lstAllRequests = new List<RequestDetailContract>();
            View.lstRequests =  new List<RequestDetailContract>();
            View.lstAllRequests = PlacementMatchingSetupManager.GetAgencyPlacementDashboardRequests(View.AgencyHierarchyRootNodeID);
            if (!View.lstAllRequests.IsNullOrEmpty() && !View.SelectedStatusID.IsNullOrEmpty())
                View.lstRequests = View.lstAllRequests.Where(cond => cond.StatusID == View.SelectedStatusID).ToList();
        }

        public void GetRequestStatuses()
        {
            View.lstRequestStatus = PlacementMatchingSetupManager.GetRequestStatuses();
        }

        public void GetAgencyRootNode()
        {
            Dictionary<Int32, String> dicAgencyRootNode = new Dictionary<Int32, String>();
            dicAgencyRootNode = PlacementMatchingSetupManager.GetAgencyRootNode(View.UserId);
            if (!dicAgencyRootNode.IsNullOrEmpty())
            {
                View.AgencyHierarchyRootNodeID = dicAgencyRootNode.Keys.FirstOrDefault();
            }
        }

        public void GetIntitutionsRequestsApproved()
        {
            View.lstInstitutionRequestsApproved = new List<InstitutionRequestPieChartContract>();
            View.lstInstitutionRequestsApproved = PlacementMatchingSetupManager.GetIntitutionsRequestsApproved(View.AgencyHierarchyRootNodeID, View.FromDate, View.ToDate);
        }
    }
}
