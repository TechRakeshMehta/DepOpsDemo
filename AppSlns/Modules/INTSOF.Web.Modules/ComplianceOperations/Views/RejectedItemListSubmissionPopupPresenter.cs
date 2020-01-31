using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class RejectedItemListSubmissionPopupPresenter : Presenter<IRejectedItemListSubmissionPopup>
    {
        public void GetRejectedItemListForReSubmission()
        {
            View.lstRejectedItemListContract = ComplianceDataManager.GetRejectedItemListForReSubmission(View.TenantId, View.OrgUserId);
        }
        public Boolean ResubmitApplicantComplianceItemData(List<Int32> lstSelectedApplicantComplItemId)
        {
            return ComplianceDataManager.ResubmitApplicantComplianceItemData(lstSelectedApplicantComplItemId, View.CurrenLoggedInUserId, View.TenantId, View.OrgUserId);
        }
    }
}
