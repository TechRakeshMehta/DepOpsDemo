using System;
using System.Collections.Generic;
using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using System.Linq;
using INTSOF.Utils;
using Entity;
using System.Configuration;

namespace CoreWeb.ProfileSharing.Views
{
    public class AgencyUserDetailPresenter : Presenter<IAgencyUserDetailView>
    {
        /// <summary>
        /// Check whether logged in user is admin or not.
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public void GetAgencyUserData()
        {
            View.AgencyUserInfo = ProfileSharingManager.GetAgencyUserDetailByID(View.TenantId, View.AgencyUserId);
            if (!View.AgencyUserInfo.IsNullOrEmpty() && !View.AgencyUserInfo.AGU_UserID.IsNullOrEmpty())
            {
                List<String> lstUserID = new List<String>();
                lstUserID.Add(Convert.ToString(View.AgencyUserInfo.AGU_UserID));
                View.UserPermissionStatus = SecurityManager.GetOrganizationUserListByUserIds(lstUserID).FirstOrDefault();
            }

            ////Commented Code Ralated 1641:TODO
            ////View.AgencyId = View.AgencyUserInfo.AGU_AgencyID;
            ////Get Permission Type
            //GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue());
            ////Get Permission Access Type
            //GetAgencyUserPermissionAccessTypeID(AgencyUserPermissionAccessType.YES.GetStringValue());

            //GetAgencyUserSharedDataForAgencyID(View.AgencyUserId);
            //GetInvitationSharedInfoTypeByAgencyUserID(View.AgencyUserId);           

            //if (View.AgencyUserInfo.AgencyUserPermissions.IsNullOrEmpty())
            //{
            //    View.AttestationRptPermission = false;
            //}
            //else
            //{
            //    View.AttestationRptPermission = (View.AgencyUserInfo.AgencyUserPermissions
            //        .Where(x => !x.AUP_IsDeleted && x.AUP_PermissionTypeID == View.AgencyUserPermissionTypeId).FirstOrDefault()
            //        .AUP_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId); // If permission == YES then true else false
            //}
        }

        ///// <summary>
        ///// Get list of Agencies for Dropdown.
        ///// Unused
        ///// </summary>
        //public void GetAgencies()
        //{
        //    View.LstAgency = ProfileSharingManager.GetAgencies(View.TenantId, true, false, Guid.Empty).OrderBy(x => x.AG_Name).ToList(); //UAT- sort dropdowns by Name
        //}

        ///// <summary>
        ///// Get list of Agency Institutions based on selected Agency.
        ///// Unused
        ///// </summary>
        //public void GetAgencyInstitutionForAgencies()
        //{
        //    List<Int32> lstAgencyID = new List<Int32>();
        //    lstAgencyID.Add(View.AgencyId);
        //    List<AgencyInstitution> tmplstInstitutions = ProfileSharingManager.GetAgencyInstitutionForAgencies(View.TenantId, lstAgencyID);
        //    TranslateToContract(tmplstInstitutions);
        //}

        ///// <summary>
        ///// Get list of Invitation meta Data which user want to share.
        ///// Unused
        ///// </summary>
        //public void GetApplicantInvitationMetaData()
        //{
        //    View.LstSharedInfo = ProfileSharingManager.GetApplicantMetaData();
        //}

        ///// <summary>
        ///// Get permissions for Compliance and Backaground
        ///// Unused
        ///// </summary>
        //public void GetSharedInfo()
        //{
        //    View.LstSharedInfoType = ProfileSharingManager.GetSharedInfoType();
        //}

        #region Private Methods

        /// <summary>
        /// Translate AgencyInstitute to AgencyUserTenantCmbContract to bind Agency Dropdown.
        /// </summary>
        /// <param name="ListInstitutions"></param>
        private void TranslateToContract(List<AgencyInstitution> ListInstitutions)
        {
            List<AgencyUserTenantCmbContract> _tmplst = new List<AgencyUserTenantCmbContract>();
            foreach (AgencyInstitution _agencyInst in ListInstitutions)
            {
                AgencyUserTenantCmbContract _agContract = new AgencyUserTenantCmbContract();
                _agContract.AGI_ID = _agencyInst.AGI_ID;//AGI_TenantID.Value;
                List<Int32?> lstTenant = new List<Int32?>();
                lstTenant.Add(_agencyInst.AGI_TenantID.Value);
                _agContract.TenantName = GetInstitutionName(lstTenant);
                _agContract.Tenant_ID = _agencyInst.AGI_TenantID.Value;
                _tmplst.Add(_agContract);
            }
            View.LstAgencyInstitutions = _tmplst;
        }

        private static string GetInstitutionName(List<Int32?> lstTenantIDs)
        {
            var lstTenant = ComplianceDataManager.getClientTenant();
            return String.Join(",", lstTenant.Where(x => lstTenantIDs.Contains(x.TenantID)).Select(col => col.TenantName));
        }

        /// <summary>
        /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        /// </summary>
        /// <param name="code"></param>
        public void GetAgencyUserPermissionAccessTypeID(String code)
        {
            View.AgencyUserPermissionAccessTypeId = ProfileSharingManager.GetAgencyUserPermissionAccessTypeID(code);
        }

        /// <summary>
        /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        /// </summary>
        /// <param name="code"></param>
        public void GetAgencyUserPermissionTypeID(String code)
        {
            View.AgencyUserPermissionTypeId = ProfileSharingManager.GetAgencyUserPermissionTypeID(code);
        }

        private void GetAgencyUserSharedDataForAgencyID(Int32 agencyUserID)
        {
            View.lstApplicationInvitationMetaDataID = ProfileSharingManager.GetAgencyUserSharedDataForAgencyUserID(View.TenantId, agencyUserID);
        }

        private void GetInvitationSharedInfoTypeByAgencyUserID(Int32 agencyUserID)
        {
            View.lstInvitationSharedInfoType = ProfileSharingManager.GetInvitationSharedInfoTypeByAgencyUserID(View.TenantId, agencyUserID);
        }

        private String GetAgencyInstitution(List<Int32> agencyID, List<Int32> selectedAGIID)
        {
            //Admin
            if (IsDefaultTenant)
            {
                List<AgencyInstitution> tmplstInstitutions = ProfileSharingManager.GetAgencyInstitutionForAgencies(View.TenantId, agencyID).Where(item => selectedAGIID.Contains(item.AGI_ID)).ToList();
                //return String.Join(",", tmplstInstitutions.Select(x => x.Tenant.TenantName).ToList());

                List<Int32?> lstTenantIDs = tmplstInstitutions.Select(x => x.AGI_TenantID).ToList();
                return GetInstitutionName(lstTenantIDs);
            }
            else //Client Admin
            {
                return GetCurrentTenantName();
            }

        }

        private String GetCurrentTenantName()
        {
            return ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == View.TenantId).FirstOrDefault().TenantName;
        }

        //private void GetAgencyUserInstitutesForAgencyUserID(Int32 agencyUserID)
        //{
        //    List<Int32> ProfileSharingManager.GetAgencyUserInstitutesForAgencyUserID(View.TenantId, agencyUserID);
        //}
        #endregion

        public bool UpdateUser()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.OrganizationUserId);
            if (!organizationUser.IsNullOrEmpty())
            {
                organizationUser.IsActive = View.IsActive;
                organizationUser.ModifiedByID = View.CurrentLoggedInUserId;
                organizationUser.ModifiedOn = DateTime.Now;
                if (organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut != View.IsLocked)
                {
                    organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = View.IsLocked;
                }
                return SecurityManager.UpdateOrganizationUser(organizationUser);
            }
            return false;
        }

        public Boolean SendAgencyUserAccountCreationMail(AgencyUserContract agencyUser)
        {
            return ProfileSharingManager.SendAgencyUserAccountCreationMail(agencyUser, View.CurrentLoggedInUserId, agencyUser.AGU_ID);
        }

        public String GetFormattedPhoneNumber(String unformattedPhoneNumber)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(unformattedPhoneNumber);
        }

        public OrganizationUser GetOrganizationUser()
        {
            return SecurityManager.GetOrganizationUser(View.OrganizationUserId);
        }

        public Boolean ResetPassword(OrganizationUser orgUser)
        {
            return SecurityManager.UpdateOrganizationUser(ResetUserPassword(orgUser));
        }

        private OrganizationUser ResetUserPassword(OrganizationUser orgUser)
        {
            OrganizationUser organizationUser = orgUser;
            organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.Password, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
            organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = false;
            organizationUser.aspnet_Users.aspnet_Membership.FailedPasswordAttemptCount = AppConsts.NONE;
            organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate = DateTime.Now;
            organizationUser.IsNewPassword = true;

            View.EmailAddress = organizationUser.aspnet_Users.aspnet_Membership.Email;
            View.FirstName = organizationUser.FirstName;
            View.LastName = organizationUser.LastName;

            return organizationUser;
        }

        public void SendPasswordResetEmail()
        {
            Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                              {                                                                  
                                                                  {EmailFieldConstants.USER_FULL_NAME,  View.FirstName +" "+ View.LastName},
                                                                  {EmailFieldConstants.PASSWORD, View.Password},
                                                                  {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,View.OrganizationUserId},
                                                                  { EmailFieldConstants.INSTITUTION_URL,GetInstitutionURL()}  // UAT- 4306
                                                              };
            SecurityManager.PrepareAndSendSystemMail(View.EmailAddress, contents, CommunicationSubEvents.NOTIFICATION_PASSWORD_RESET_BY_ADMIN, null,true);
        }
        #region UAT- 4306
        public string GetInstitutionURL()
        {
            //String institutionURL = WebSiteManager.GetInstitutionUrl(View.TenantId);
            //return institutionURL;
            Int32 agencyUserID = View.AgencyUserId;            
            String institutionURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                            ? String.Empty
                                            : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);           
            var url = "http://" + String.Format(institutionURL);
            return url;
        }
        #endregion
    }
}
