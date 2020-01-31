using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SearchUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.SearchUI.Views
{
    public class SupportPortalNotesPresenter : Presenter<ISupportPortalNotesView>
    {
        public void GetCurrentLoggedInUserName()
        {
            if (View.OrganizationUser != null)
                View.CurrentLoggedInUserName = View.OrganizationUser.FirstName + " " + View.OrganizationUser.LastName;
        }

        public void GetSupportPortalBkgOrderNotes()
        {
            View.lstBkgOrderNotes = new List<BkgOrderQueueNotesContract>();
            if (!View.ApplicantOrganizationUserID.IsNullOrEmpty() && View.ApplicantOrganizationUserID > AppConsts.NONE && !View.SelectedTenantID.IsNullOrEmpty() && View.SelectedTenantID > AppConsts.NONE)
            {
                View.lstBkgOrderNotes = ComplianceDataManager.GetSupportPortalBkgOrderNotes(View.SelectedTenantID, View.ApplicantOrganizationUserID);
            }
        }

        public Boolean SaveSupportPortalBkgOrderNotes(Int32 OrderID, String notes)
        {
            BkgOrderQueueNote supportPortalBkgOrderNotesToSave = new BkgOrderQueueNote();
            supportPortalBkgOrderNotesToSave.BOQN_MasterOrderID = OrderID;
            supportPortalBkgOrderNotesToSave.BOQN_Notes = notes;
            supportPortalBkgOrderNotesToSave.BOQN_CreatedBy = View.CurrentLoggedInUserId;
            supportPortalBkgOrderNotesToSave.BOQN_CreatedOn = DateTime.Now;
            supportPortalBkgOrderNotesToSave.BOQN_IsDeleted = false;
            return ComplianceDataManager.SaveSupportPortalBkgOrderNotes(View.SelectedTenantID,supportPortalBkgOrderNotesToSave);
        }

        public Boolean UpdateSupportPortalBkgOrderNotes(Int32 supportPortalBkgOrderNoteID, String updatedNote, Int32 OrderID)
        {
            BkgOrderQueueNote supportPortalBkgOrderNoteToUpdate = ComplianceDataManager.GetSupportPortalBkgOrderNotesByNoteID(View.SelectedTenantID, supportPortalBkgOrderNoteID);

            if (!supportPortalBkgOrderNoteToUpdate.IsNullOrEmpty())
            {
                supportPortalBkgOrderNoteToUpdate.BOQN_MasterOrderID = OrderID;
                supportPortalBkgOrderNoteToUpdate.BOQN_Notes = updatedNote;
                supportPortalBkgOrderNoteToUpdate.BOQN_ModifiedBy = View.CurrentLoggedInUserId;
                supportPortalBkgOrderNoteToUpdate.BOQN_ModifiedOn = DateTime.Now;
                return ComplianceDataManager.UpdateSupportPortalBkgOrderNotes(View.SelectedTenantID);
            }
            return false;
        }

        public Boolean DeleteSupportPortalBkgOrderNote(Int32 supportPortalBkgOrderNoteID)
        {
            BkgOrderQueueNote supportPortalNotesToDelete = ComplianceDataManager.GetSupportPortalBkgOrderNotesByNoteID(View.SelectedTenantID, supportPortalBkgOrderNoteID);
            if (supportPortalNotesToDelete.IsNotNull())
            {
                supportPortalNotesToDelete.BOQN_IsDeleted = true;
                supportPortalNotesToDelete.BOQN_ModifiedBy = View.CurrentLoggedInUserId;
                supportPortalNotesToDelete.BOQN_ModifiedOn = DateTime.Now;
                return ComplianceDataManager.UpdateSupportPortalBkgOrderNotes(View.SelectedTenantID);
            }
            return false;
        }
    }
}
