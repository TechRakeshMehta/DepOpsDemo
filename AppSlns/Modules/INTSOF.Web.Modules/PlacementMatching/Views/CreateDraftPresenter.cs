using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace CoreWeb.PlacementMatching.Views
{
    public class CreateDraftPresenter : Presenter<ICreateDraftView>
    {
        public override void OnViewInitialized()
        {
            GetTenantName();
            View.RequestDetail = new RequestDetailContract();
            View.lstDays = new Dictionary<Int32, String>();
        }
        public void GetOpportunityData(Int32 OppertunityId)
        {
            View.OpportunityDetails = new PlacementMatchingContract();
            View.OpportunityDetails = PlacementMatchingSetupManager.GetOpportunityDetailByID(OppertunityId);
            View.OpportunityDetails.lstShift = PlacementMatchingSetupManager.GetAllShifts().Where(cond => cond.ClinicalInventoryID == OppertunityId).ToList();


        }

        public void GetRequestData(Int32 RequestId)
        {
            View.RequestDetail = new RequestDetailContract();
            View.RequestDetail = PlacementMatchingSetupManager.GetRequestDetail(RequestId);
        }

        public Boolean IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean SaveRequest()
        {
            return PlacementMatchingSetupManager.SaveRequest(View.RequestDetail, View.CurrentLoggedInUser,View.SetCustomAttributeList);
        }

        public Boolean ChangeRequestStatus(Int32 requestID, string statusCode)
        {
            return PlacementMatchingSetupManager.ChangeRequestStatus(View.CurrentLoggedInUser, requestID, statusCode);
        }

        public void GetTenantName()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.TenantName = ClinicalRotationManager.GetTenants(SortByName, clientCode).Where(a => a.TenantID == View.SelectedTenantID).Select(a => a.TenantName).FirstOrDefault();
        }

        public List<PlacementRequestAuditContract> GetPlacementRequestAuditLogs(Int32 requestId)
        {
            List<PlacementRequestAuditContract> lst = new List<PlacementRequestAuditContract>();
            lst = PlacementMatchingSetupManager.GetPlacementRequestAuditLogs(requestId);
            return lst;

        }
        public void GetSharedCustomAttributeList()
        {
            if(!View.RequestId.IsNullOrEmpty() && View.RequestId>AppConsts.NONE)
            View.CustomAttributeList = PlacementMatchingSetupManager.GetSharedCustomAttributeList(View.CurrentAgencyHierarchyID, SharedCustomAttributeUseType.ClinicalInventoryRequest.GetStringValue(),View.RequestId, CustomAttributeValueRecordType.ClinicalInventoryRequest.GetStringValue());
            else
                View.CustomAttributeList = PlacementMatchingSetupManager.GetSharedCustomAttributeList(View.CurrentAgencyHierarchyID, SharedCustomAttributeUseType.ClinicalInventoryRequest.GetStringValue(),null, null);
        }

    }
}
