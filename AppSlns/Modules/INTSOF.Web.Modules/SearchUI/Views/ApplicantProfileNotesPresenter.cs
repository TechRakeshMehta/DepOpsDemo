using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using CoreWeb.BkgOperations.Views;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;

namespace CoreWeb.SearchUI.Views
{
    public class ApplicantProfileNotesPresenter : Presenter<IApplicantProfileNotesView>
    {
        /// <summary>
        /// Method to get current logged in user name.
        /// </summary>
        public void GetCurrentLoggedInUserName()
        {
            if (View.OrganizationUser != null)
                View.CurrentLoggedInUserName = View.OrganizationUser.FirstName + " " + View.OrganizationUser.LastName;
            //Entity.OrganizationUser currentOrganizationUser= SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            //if (currentOrganizationUser.IsNotNull())
            //    View.CurrentLoggedInUserName = currentOrganizationUser.FirstName + " " + currentOrganizationUser.LastName;
        }

        /// <summary>
        /// Gets the profile Notes list for the selected user.
        /// </summary>
        /// <returns></returns>
        public void GetApplicantProfileNotesList()
        {
            View.ApplicantProfileNoteList = new List<ApplicantProfileNotesContract>();
            if (View.SelectedTenantId > 0)
            {
                if (View.IsAdminNotes) //UAT-3326
                    View.ApplicantProfileNoteList = SecurityManager.GetAdminProfileNotesList(View.ApplicantUserID);
                else
                    View.ApplicantProfileNoteList = ComplianceDataManager.GetApplicantProfileNotesList(View.SelectedTenantId, View.ApplicantUserID, View.IsClientAdmin); //IsClientAdmin- UAT-5052
            }
        }

        /// <summary>
        /// Method to save and update applicant profile notes.
        /// </summary>
        /// <returns></returns>
        public Boolean SaveUpdateProfileNote()
        {
            Boolean saveUpdateStatus = false;
            if (View.IsAdminNotes) //UAT-3326
                saveUpdateStatus = SecurityManager.SaveUpdateAdminProfileNotes(View.ApplicantProfileNoteList);
            else
                saveUpdateStatus = ComplianceDataManager.SaveUpdateApplicantProfileNotes(View.SelectedTenantId, View.ApplicantProfileNoteList);
            return saveUpdateStatus;
        }

        /// <summary>
        /// Save Profile Notes
        /// </summary>
        /// <returns></returns>
        public Boolean SaveApplicantProfileNotes(String notes)
        {
            if (View.IsAdminNotes) //UAT-3326
            {
                Entity.AdminProfileNote adminNotesToSave = new Entity.AdminProfileNote();
                adminNotesToSave.APN_OrganizationUserID = View.ApplicantUserID;
                adminNotesToSave.APN_ProfileNotes = notes;
                adminNotesToSave.APN_CreatedBy = View.CurrentLoggedInUserId;
                adminNotesToSave.APN_CreatedOn = DateTime.Now;
                adminNotesToSave.APN_IsDeleted = false;
                return SecurityManager.SaveAdminProfileNotes(adminNotesToSave);
            }
            else
            {
                ApplicantProfileNote applicantNotesToSave = new ApplicantProfileNote();
                applicantNotesToSave.APN_OrganizationUserID = View.ApplicantUserID;
                applicantNotesToSave.APN_ProfileNotes = notes;
                applicantNotesToSave.APN_CreatedBy = View.CurrentLoggedInUserId;
                applicantNotesToSave.APN_CreatedOn = DateTime.Now;
                applicantNotesToSave.APN_IsDeleted = false;
                //Start UAT-5052
                if (View.IsClientAdmin)
                {
                    applicantNotesToSave.APN_IsVisibleToClientAdmin = true; 
                }
                //End UAT-5052
                return ComplianceDataManager.SaveApplicantProfileNotes(View.SelectedTenantId, applicantNotesToSave);
            }
        }

        /// <summary>
        /// Update Profile Notes
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateApplicantProfileNote(Int32 applicantProfileNoteID, String updatedNote)
        {
            if (View.IsAdminNotes) //UAT-3326
            {
                Entity.AdminProfileNote adminNotesToUpdate = SecurityManager.GetAdminProfileNotesByNoteID(applicantProfileNoteID);
                if (adminNotesToUpdate.IsNotNull())
                {
                    adminNotesToUpdate.APN_OrganizationUserID = View.ApplicantUserID;
                    adminNotesToUpdate.APN_ProfileNotes = updatedNote;
                    adminNotesToUpdate.APN_ModifiedBy = View.CurrentLoggedInUserId;
                    adminNotesToUpdate.APN_ModifiedOn = DateTime.Now;
                    return SecurityManager.UpdateAdminProfileNote();
                }
                return false;
            }
            else
            {
                ApplicantProfileNote applicantNotesToUpdate = ComplianceDataManager.GetApplicantProfileNotesByNoteID(View.SelectedTenantId, applicantProfileNoteID);
                if (applicantNotesToUpdate.IsNotNull())
                {
                    applicantNotesToUpdate.APN_OrganizationUserID = View.ApplicantUserID;
                    applicantNotesToUpdate.APN_ProfileNotes = updatedNote;
                    applicantNotesToUpdate.APN_ModifiedBy = View.CurrentLoggedInUserId;
                    applicantNotesToUpdate.APN_ModifiedOn = DateTime.Now;
                    //Start UAT-5052
                    if (View.IsClientAdmin)
                    {
                        applicantNotesToUpdate.APN_IsVisibleToClientAdmin = true;
                    }
                    //End UAT-5052
                    return ComplianceDataManager.UpdateApplicantProfileNote(View.SelectedTenantId);
                }
                return false;
            }
        }

        /// <summary>
        /// Delete Profile Notes
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteApplicantProfileNote(Int32 applicantProfileNoteID)
        {
            if (View.IsAdminNotes) //UAT-3326
            {
                Entity.AdminProfileNote adminNotesToDelete = SecurityManager.GetAdminProfileNotesByNoteID(applicantProfileNoteID);
                if (adminNotesToDelete.IsNotNull())
                {
                    adminNotesToDelete.APN_IsDeleted = true;
                    adminNotesToDelete.APN_ModifiedBy = View.CurrentLoggedInUserId;
                    adminNotesToDelete.APN_ModifiedOn = DateTime.Now;
                    return SecurityManager.UpdateAdminProfileNote();
                }
                return false;
            }
            else
            {
                ApplicantProfileNote applicantNotesToDelete = ComplianceDataManager.GetApplicantProfileNotesByNoteID(View.SelectedTenantId, applicantProfileNoteID);
                if (applicantNotesToDelete.IsNotNull())
                {
                    applicantNotesToDelete.APN_IsDeleted = true;
                    applicantNotesToDelete.APN_ModifiedBy = View.CurrentLoggedInUserId;
                    applicantNotesToDelete.APN_ModifiedOn = DateTime.Now;
                    return ComplianceDataManager.UpdateApplicantProfileNote(View.SelectedTenantId);
                }
                return false;
            }
        }

        //Start UAT-5052
        public void IsClientAdmin()
        {
            if (IntegrityManager.IsClientAdmin(View.LoggedInUserTenantId))
            {
                View.IsClientAdmin = true;
            }
        }
        //End UAT-5052
    }
}
