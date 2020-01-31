#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity;
using INTSOF.UI.Contract.ProfileSharing;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public class ApplicantPortfolioDetailsPresenter : Presenter<IApplicantPortfolioDetailsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
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

        /// <summary>
        /// Performs an update operation for User with it's details.
        /// </summary>
        public Boolean UpdateUser()
        {
            OrganizationUser organizationUser = GetOrganizationUser();
            if (!organizationUser.IsNullOrEmpty())
            {
                organizationUser.IsActive = View.IsActive;
                //UAT-887: WB: Delay Automatic emails going out after activation
                if (View.IsActive && organizationUser.ActiveDate == null && organizationUser.IsApplicant == true)
                    organizationUser.ActiveDate = DateTime.Now;
                organizationUser.ModifiedByID = View.CurrentLoggedInUserId;
                organizationUser.ModifiedOn = DateTime.Now;
                if (organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut != View.IsLocked)
                {
                    organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = View.IsLocked;
                }
                #region UAT-2930
                //UAT-3068
                //if (View.IsUserTwoFactorAuthenticatedPrevious && !View.IsTwoFactorAuthentication)
                //{
                //    if (SecurityManager.DeleteTwofactorAuthenticationForUserID(Convert.ToString(organizationUser.UserID), View.OrganizationUserId))
                //    {
                //        CommunicationManager.SendMailOnTwoFactorAuthentication(organizationUser, View.SelectedTenantId);
                //    }
                //}
                #endregion

                #region UAT-3068
                if (!View.IsUserTwoFactorAuthenticatedPrevious.Equals(View.SelectedAuthenticationType) && View.SelectedAuthenticationType == AuthenticationMode.None.GetStringValue())
                {
                    if (SecurityManager.SaveAuthenticationData(Convert.ToString(organizationUser.UserID),View.SelectedAuthenticationType, View.OrganizationUserId))
                    {
                        CommunicationManager.SendMailOnTwoFactorAuthentication(organizationUser, View.SelectedTenantId);
                    }
                    View.IsUserTwoFactorAuthenticatedPrevious = View.SelectedAuthenticationType;
                }
                #endregion
                return SecurityManager.UpdateOrganizationUser(organizationUser);
            }
            return false;
            //Utils.GetMessage(ResourceConst.SPACE) + organizationUser.FirstName + SysXUtils.GetMessage(ResourceConst.SPACE) + organizationUser.LastName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
        }

        public void SetPageControls()
        {
            OrganizationUser organizationUser = GetOrganizationUser();

            if (!organizationUser.IsNullOrEmpty())
            {
                View.IsActive = organizationUser.IsActive;
                View.IsLocked = organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut;
                View.OrganizationUser = organizationUser;
            }
            //UAT-3068
            String authenticationMode = SecurityManager.GetUserAuthenticationUseTypeForUserID(Convert.ToString(organizationUser.UserID));
            View.SelectedAuthenticationType = View.IsUserTwoFactorAuthenticatedPrevious = authenticationMode;

            //if (!userTwoFactorAuth.IsNullOrEmpty())
            //{
            //    View.IsTwoFactorAuthentication = true;               
            //    //if (userTwoFactorAuth.UTFA_IsVerified == false)
            //    //    View.IsTwoFactorAuthenticationVerified = false;
            //    //else
            //    //    View.IsTwoFactorAuthenticationVerified = true;
            //}
            //else
            //{
            //    View.IsTwoFactorAuthentication = false;
            //    //View.IsTwoFactorAuthenticationVerified = false;
            //}
        }

        //UAT 2367
        /// <summary>
        /// Gets the list of invitations that has been sent by the applicant
        /// </summary>
        public void BindInvitations()
        {
            View.lstInvitationsSent = ProfileSharingManager.GetApplicantInvitations(View.OrganizationUserId, View.SelectedTenantId, AppConsts.ONE);
        }

        /// <summary>
        /// UAT 1882
        /// </summary>
        /// <param name="rotationID"></param>
        public void GetAttestationDocumentsToExport(Int32 psiID)
        {
            ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>>();
            serviceRequest.Parameter1 = new Dictionary<String, Int32> { 
                                                                       { AppConsts.PROFILE_SHARING_INVITATION_ID, psiID },
                                                                       { AppConsts.IGNORE_AGENCY_USER_CHECK, AppConsts.ONE } 
                                                                      };
            serviceRequest.Parameter2 = new List<Tuple<Int32, Int32, Int32>>();
            View.LstInvitationDocumentContract = ClinicalRotationManager.GetAttestationDocumentsToExport(serviceRequest, View.OrganizationUserId);
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            Int32 AdminTenantID = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            //Checked if logged user is admin or not.
            if (AdminTenantID == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }
        //UAT-2930
        public Boolean ShowhideTwoFactorAuthentication()
        {
            Boolean IsClientSettingsEnabledForAnyTenant = SecurityManager.ShowhideTwoFactorAuthentication(Convert.ToString(View.OrganizationUser.UserID));
            return IsClientSettingsEnabledForAnyTenant;
        }

        #region UAT- 4306
        public string GetInstitutionURL()
        {
            String institutionURL = WebSiteManager.GetInstitutionUrl(View.SelectedTenantId);
            return institutionURL;
        }
        #endregion
    }
}





