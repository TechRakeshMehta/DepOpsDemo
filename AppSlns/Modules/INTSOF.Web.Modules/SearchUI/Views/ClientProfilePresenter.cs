using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;
using INTSOF.UI.Contract.SearchUI; //UAT-3326

namespace CoreWeb.Search.Views
{
    public class ClientProfilePresenter : Presenter<IClientProfile>
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
        /// Method to get Organization User Data
        /// </summary>
        public void GetOrganizationUserData()
        {
            View.OrganizationUserData = SecurityManager.GetOrganizationUser(View.OrganizationUserID);
        }

        /// <summary>
        /// Method to get Notification settings Data
        /// </summary>
        /// <returns></returns>
        public List<Entity.CommunicationCCUser> GetNotificationSettingData()
        {
            return CommunicationManager.GetNotificationSettingData(View.OrganizationUserID);
        }

        public void GetSystemEntityUserPermissionData()
        {
            View.SystemEntityUserPermissionData = SecurityManager.GetSystemEntityUserPermissionData(View.OrganizationUserID);
        }

        /// <summary>
        /// Method to get Instituion Hierarchy tree data
        /// </summary>
        public void GetHierarchyTreeData()
        {
            List<Entity.ClientEntity.InstituteHierarchyNodesList> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyNodes(View.ClientTenantID, View.OrganizationUserID, true).ToList(); //UAT-3369
            View.lstTreeHierarchyData = objInstituteHierarchyTree.OrderBy(x => x.Value).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();

            List<Entity.ClientEntity.GetDepartmentTree> objBkgInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTreeForBackgroundHierarchyPermissionType(View.ClientTenantID, View.OrganizationUserID, true).ToList();
            View.LstBackgroundTreeData = objBkgInstituteHierarchyTree.OrderBy(x => x.Value).ThenBy(x => x.TreeNodeTypeID).ToList();
        }

        public Boolean ResetPassword(String password)
        {
            GetOrganizationUserData();
            return SecurityManager.UpdateOrganizationUser(ResetUserPassword(View.OrganizationUserData, password));
        }


        /// <summary>
        /// Method to reset password of client user.
        /// </summary>
        /// <param name="orgUser"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private OrganizationUser ResetUserPassword(OrganizationUser orgUser, String password)
        {
            OrganizationUser organizationUser = orgUser;
            organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(password, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
            organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = false;
            organizationUser.aspnet_Users.aspnet_Membership.FailedPasswordAttemptCount = AppConsts.NONE;
            organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate = DateTime.Now;
            organizationUser.IsNewPassword = true;
            return organizationUser;
        }

        /// <summary>
        /// Method to send Password Reset mail
        /// </summary>
        /// <param name="password"></param>
        public void SendPasswordResetMail(String password)
        {
            Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                              {                                                                  
                                                              {EmailFieldConstants.USER_FULL_NAME,View.ClientFirstName + " " + View.ClientLastName},
                                                                  {EmailFieldConstants.PASSWORD,password},
                                                                {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,View.OrganizationUserID},
                                                                { EmailFieldConstants.INSTITUTION_URL,GetInstitutionURL()}  // UAT- 4306
                                                              };
            SecurityManager.PrepareAndSendSystemMail(View.EmailAddress, contents, CommunicationSubEvents.NOTIFICATION_PASSWORD_RESET_BY_ADMIN, null, true);
        }

        /// <summary>
        /// Method to get Feature Permission tree data
        /// </summary>
        public void GetFeaturePermissionTreeData()
        {
            List<FeatureActionContract> objInstituteHierarchyTree = ComplianceSetupManager.GetFeaturePermissionTree(View.ClientTenantID, View.OrganizationUserData.UserID).ToList();
            View.FeaturePermissionData = objInstituteHierarchyTree.OrderBy(x => x.Name).ToList();
        }

        public void LockUnlockUser()
        {
            OrganizationUser organizationUser = GetOrganizationUser();
            if (!organizationUser.IsNullOrEmpty())
            {
                organizationUser.ModifiedByID = View.CurrentLoggedInUserId;
                organizationUser.ModifiedOn = DateTime.Now;
                if (View.LockUser == AppConsts.CMD_LOCK)
                {
                    organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = true;
                    View.LockUserStatus = AppConsts.LOCKED;
                }
                else if (View.LockUser == AppConsts.CMD_UNLOCK)
                {
                    organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = false;
                    View.LockUserStatus = AppConsts.UNLOCKED;
                }
            }
            if (SecurityManager.UpdateOrganizationUser(organizationUser))
            {
                if (View.LockUserStatus == AppConsts.LOCKED)
                {
                    View.LockUserCommandName = AppConsts.CMD_UNLOCK;
                    View.LockUnlockUserTooltip = AppConsts.UNLOCK_TOOLTIP;
                }
                else if (View.LockUserStatus == AppConsts.UNLOCKED)
                {
                    View.LockUserCommandName = AppConsts.CMD_LOCK;
                    View.LockUnlockUserTooltip = AppConsts.LOCK_TOOLTIP;
                }
            }

        }

        public OrganizationUser GetOrganizationUser()
        {
            return SecurityManager.GetOrganizationUser(View.OrganizationUserID);
        }

        public void GetAssignedUserRoleNames()
        {
            View.AssignedRoles = SecurityManager.GetAssignedUserRoleNames(View.OrganizationUserID, View.ClientTenantID);
        }

        public Boolean SaveUpdateProfileCustomAttributes()
        {
            return ComplianceDataManager.AddUpdateProfileCustomAttributes(View.ProfileCustomAttributeList, View.OrganizationUserID, View.CurrentLoggedInUserId, View.ClientTenantID);
        }

        #region MyRegion
        public Boolean ShowhideTwoFactorAuthentication()
        {
            Boolean IsClientSettingsEnabledForAnyTenant = SecurityManager.ShowhideTwoFactorAuthentication(Convert.ToString(View.OrganizationUserData.UserID));
            return IsClientSettingsEnabledForAnyTenant;
        }

        public UserTwoFactorAuthentication SetTwoFactorAuthButtonText(String userId)
        {
            GetOrganizationUserData();
            UserTwoFactorAuthentication userTwoFactorAuth = SecurityManager.GetTwofactorAuthenticationForUserID(Convert.ToString(userId));
            return userTwoFactorAuth;
        }

        #endregion

        #region UAT- 4306
        public string GetInstitutionURL()
        {
            String institutionURL = WebSiteManager.GetInstitutionUrl(View.ClientTenantID);
            return institutionURL;
        }
        #endregion
    }
}
