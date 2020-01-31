#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Configuration;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ClientContact;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class ClinicalRotationMappingPresenter : Presenter<IClinicalRotationMappingView>
    {

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = ClinicalRotationManager.GetTenants(SortByName, clientCode);
        }

        /// <summary>
        /// Get Clinical Rotation details
        /// </summary>
        public void GetRotationDetail()
        {
            List<ClinicalRotationDetailContract> rotationData;
            if (View.SelectedTenantID == 0)
            {
                rotationData = new List<ClinicalRotationDetailContract>();
            }
            else
            {
                if (View.ScreenMode.ToLower() == "assign")
                {
                    if (IsAdminLoggedIn())
                        rotationData = ClinicalRotationManager.GetClinicalRotationMappingData(View.SelectedTenantID, View.GridCustomPaging);
                    else
                        rotationData = ClinicalRotationManager.GetClinicalRotationMappingData(View.SelectedTenantID, View.GridCustomPaging, null, View.CurrentLoggedInUserId);
                }
                else //Unassign
                {
                    String applicantUserIds = String.Join(",", View.ApplicantUserIds);
                    if (IsAdminLoggedIn())
                        rotationData = ClinicalRotationManager.GetClinicalRotationMappingData(View.SelectedTenantID, View.GridCustomPaging, applicantUserIds);
                    else
                        rotationData = ClinicalRotationManager.GetClinicalRotationMappingData(View.SelectedTenantID, View.GridCustomPaging, applicantUserIds, View.CurrentLoggedInUserId);
                }
            }
            View.ClinicalRotationData = rotationData;

            if (rotationData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = rotationData[0].TotalRecordCount;
            }
        }

        /// <summary>
        /// Save/Update clinical rotation
        /// </summary>
        /// <returns></returns>
        public Boolean SaveUpdateClinicalRotation()
        {
            View.ViewContract.ContactsToSendEmail = View.ClientContactListForAddForm.Where(x => View.SelectedClientContacts.Contains(x.ClientContactID)).ToList();
            int ClinicalRotationId = ClinicalRotationManager.SaveUpdateClinicalRotation(View.ViewContract, View.SaveCustomAttributeList, View.CurrentLoggedInUserId, ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
            if (ClinicalRotationId > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        public void GetCustomAttributeList(Int32? rotationId)
        {
            View.GetCustomAttributeList = ClinicalRotationManager.GetCustomAttributeMappingList(View.SelectedTenantIDForAddForm, CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue(), rotationId);
        }

        /// <summary>
        /// Method to get agencies of an institution
        /// </summary>
        public void GetAllAgencyForAddForm()
        {
            if (View.SelectedTenantIDForAddForm == 0)
                View.lstAgencyForAddForm = new List<AgencyDetailContract>();
            else
            {
                View.lstAgencyForAddForm = ClinicalRotationManager.GetAllAgency(View.SelectedTenantIDForAddForm);
            }
        }

        /// <summary>
        /// Get Client Contacts
        /// </summary>
        public void GetClientContactsForAddForm()
        {
            if (View.SelectedTenantIDForAddForm == 0)
                View.ClientContactListForAddForm = new List<ClientContactContract>();
            else
            {
                View.ClientContactListForAddForm = ClientContactManager.GetClientContacts(View.SelectedTenantIDForAddForm);
            }
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetWeekDaysForAddForm()
        {
            if (View.SelectedTenantIDForAddForm == 0)
                View.WeekDayListForAddForm = new List<WeekDayContract>();
            else
            {
                View.WeekDayListForAddForm = ClinicalRotationManager.GetWeekDayList(View.SelectedTenantIDForAddForm);
            }
        }

        /// <summary>
        /// Get default permission for client admin user
        /// </summary>
        /// <returns></returns>
        public Dictionary<Int32, String> GetDefaultPermissionForClientAdmin()
        {
            return ClinicalRotationManager.GetDefaultPermissionForClientAdmin(View.SelectedTenantID, View.CurrentLoggedInUserId);
        }

        /// <summary>
        /// UAT-1784: Get Granular Permissions for current user
        /// </summary>
        public void GetGranularPermissions()
        {
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions);
            View.dicGranularPermissions = dicPermissions;
        }

        /// <summary>
        /// Check the user group existance
        /// </summary>
        /// <param name="userGroupName"></param>
        /// <param name="userGroupId"></param>
        /// <returns>Boolean</returns>
        public Boolean IsUserGroupExist(String userGroupName, Int32? userGroupId = null)
        {
            var lstUserGroup = ComplianceSetupManager.GetAllUserGroup(View.SelectedTenantID);
            if (userGroupId != null)
            {
                if (lstUserGroup.Any(x => x.UG_Name.ToLower() == userGroupName.ToLower() && x.UG_ID != userGroupId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (lstUserGroup.Any(x => x.UG_Name.ToLower() == userGroupName.ToLower() && !x.UG_IsDeleted))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Assign Rotations to applicant users
        /// </summary>
        /// <returns></returns>
        public Boolean AssignRotations(out String message, out String messageType)
        {    
            return ClinicalRotationManager.AssignRotationsToUsers(View.SelectedTenantID, View.AssignRotationIds, View.ApplicantUserIds, View.CurrentLoggedInUserId, out message, out messageType);
        }

        /// <summary>
        /// Unassign Rotations of applicants
        /// </summary>
        /// <returns></returns>
        public Boolean UnassignRotations()
        {
            return ClinicalRotationManager.UnassignRotations(View.SelectedTenantID, View.AssignRotationIds, View.ApplicantUserIds, View.CurrentLoggedInUserId);
        }

        #region UAT-4147

        /// <summary>
        /// To check is applicant(s) already exists as Instructor(s) in Clinical Rotation
        /// </summary>
        public List<ClinicalRotationMembersContract> IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(string rotationIDs, int tenantID, string selectedOrgUserIDs, string selectedClientContactIDs)
        {
            return ClinicalRotationManager.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(rotationIDs, tenantID, selectedOrgUserIDs, selectedClientContactIDs);
        }
        #endregion

        #region UAT-4323

        /// <summary>
        /// To get Applicant details for selected Clinical Rotations
        /// </summary>
        public List<ClinicalRotationDetailContract> GetApplicantDetailsForSelectedRotations(string rotationIDs, int tenantID)
        {
            return ClinicalRotationManager.GetApplicantDetailsForSelectedRotations(rotationIDs, tenantID);
        }
        #endregion
    }
}




