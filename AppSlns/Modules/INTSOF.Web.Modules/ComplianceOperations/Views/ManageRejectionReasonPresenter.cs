using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageRejectionReasonPresenter : Presenter<IManageRejectionReasonView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetRejectionReasons()
        {
            View.ListRejectionReason = ComplianceSetupManager.GetRejectionReasons();
        }

        public Boolean SaveUpdateRejectionReason()
        {
            RejectionReason rejectionReasonToSave = new RejectionReason();
            rejectionReasonToSave.RR_ReasonText = View.ReasonText;
            rejectionReasonToSave.RR_Name = View.ReasonName;
            rejectionReasonToSave.RR_RejectionReasonCategoryID = View.RejectionReasonCategoryId;
            rejectionReasonToSave.RR_CreatedBy = View.CurrentUserId;
            rejectionReasonToSave.RR_CreatedOn = DateTime.Now;
            rejectionReasonToSave.RR_IsDeleted = false;

            if (View.RejectionReasonID > 0)
            {
                rejectionReasonToSave.RR_ID = View.RejectionReasonID;
                rejectionReasonToSave.RR_ModifiedBy = View.CurrentUserId;
                rejectionReasonToSave.RR_ModifiedOn = DateTime.Now;
            }
            

            return ComplianceSetupManager.SaveUpdateRejectionReason(rejectionReasonToSave);
        }

        public Boolean DeleteRejectionReason()
        {
            return ComplianceSetupManager.DeleteRejectionReason(View.RejectionReasonID,View.CurrentUserId);
        }

        public void GetRejectionCategoryList()
        {
            var listReasonCategory = ComplianceSetupManager.GetRejectionReasonCategoryList();
            listReasonCategory.Insert(0, new lkpRejectionReasonCategory { LRRC_ID = 0, LRRC_Name = "--SELECT--" });
            View.ListRejectionCategory = listReasonCategory;
        }
    }
}
