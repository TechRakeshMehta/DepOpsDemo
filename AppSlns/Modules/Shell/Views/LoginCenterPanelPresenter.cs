#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  LoginCenterPanelPresenter.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Text.RegularExpressions;
#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.UI.Contract.SysXSecurityModel;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.AdminEntryPortal;
using System.Web;
#endregion

#endregion
namespace CoreWeb.Shell.Views
{
    public class LoginCenterPanelPresenter : Presenter<ILoginCenterPanelView>
    {
        /// <summary>
        /// UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        /// </summary>
        /// <param name="isLoggedIn"></param>
        public void DoLogOff(bool isLoggedIn)
        {
            //IssessionTimeout will be false as user is switching institution.
            if (isLoggedIn && !this.View.CurrentSessionId.IsNullOrEmpty())
            {
                View.ViewStateProvider.Delete(this.View.CurrentSessionId);
                SysXWebSiteUtils.SessionService.ClearSession(false);
            }
        }

        public Boolean HasNodePermission(Int32 tenantID, Int32 loggedInUserID, Int32 HierarchyNodeID)
        {
            //List<UserNodePermissionsContract> lstUserNodePermission = 
            String currentPermissionCode = ComplianceSetupManager.GetUserNodePermissionForVerificationAndProfile(tenantID, loggedInUserID).Where(x => x.DPM_ID == HierarchyNodeID).Select(x => x.PermissionCode).FirstOrDefault();
            if (!currentPermissionCode.IsNullOrEmpty() && currentPermissionCode == LkpPermission.FullAccess.GetStringValue() || currentPermissionCode == LkpPermission.ReadOnly.GetStringValue())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to check whether invitee has already account or not
        /// </summary>
        /// <param name="inviteToken"></param>
        public Entity.OrganizationUser IsSharedUserExists(Guid token, Boolean isProfileSharingToken, Int32? agencyUserID)
        {
            return ProfileSharingManager.IsSharedUserExists(token, isProfileSharingToken, agencyUserID);
        }

        public Boolean UpdateAgencyUserAgenciesVerificationCode(Int32 agencyUserID, String verificationCode, Entity.OrganizationUser orgUser)
        {
            return ProfileSharingManager.UpdateAgencyUserAgenciesVerificationCode(agencyUserID, verificationCode, orgUser);
        }

        public Boolean IsUrlExistForInstitutionType()
        {
            if (View.CheckWebsiteURL)
                return WebSiteManager.IsUrlExistForTenantType(View.SiteUrl, TenantType.Institution.GetStringValue());
            return true;
        }

        public Boolean IsUrlAdminType()
        {
            if (View.CheckWebsiteURL)
                return WebSiteManager.IsUrlExistForTenantType(View.SiteUrl, TenantType.Company.GetStringValue());
            return true;
        }

        #region UAT-1218
        public void GetOrganizationUserTypeMapping(Guid userID)
        {
            View.OrganizationUserTypeMapping = SecurityManager.GetOrganizationUserTypeMapping(userID);
        }
        #endregion

        /// <summary>
        /// Method to Add OrgUserTypeMapping
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="userTypeCode"></param>
        public void AddOrgUserTypeMapping(Int32 orgUserID, String userTypeCode)
        {
            SecurityManager.AddOrganizationUserTypeMapping(orgUserID, userTypeCode);
        }

        public void UpdateInviteeOrganizationUserID(Int32 orgUserID, Guid inviteToken, Int32? agencyUserID)
        {
            String profileSharingInvitationIds; //UAT-3400
            ProfileSharingManager.UpdateInviteeOrganizationUserID(orgUserID, inviteToken, agencyUserID, out profileSharingInvitationIds);
        }

        /// <summary>
        /// Assign default roles to agency shared user
        /// </summary>
        /// <param name="orgUser"></param>
        public void AssignDefaultRolesToAgencyUser(OrganizationUser orgUser)
        {
            ApplicationDataManager.AssignDefaultRolesToAgencyUser(orgUser);
        }

        /// <summary>
        /// To validate user and auto login
        /// </summary>
        /// <param name="tokenKey"></param>
        public void ValidateUserAndAutoLogin(String tokenKey)
        {
            Object applicationData = ApplicationDataManager.GetObjectDataByKey("ApplicantInstData");
            if (applicationData != null)
            {
                Dictionary<String, ApplicantInsituteDataContract> applicantData = ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
                ApplicantInsituteDataContract appData = applicantData.GetValue(tokenKey);
                if (appData.IsNotNull())
                {
                    View.IsIncorrectLoginUrl = appData.IsIncorrectLogin;
                    var diffInSeconds = (DateTime.Now - appData.TokenCreatedTime).TotalSeconds;
                    //Token expiry time is 60 seconds or 1 minute. If token is not expired then auto login
                    if (diffInSeconds <= 60)
                    {
                        OrganizationUser orgUser;
                        if (appData.TenantID == 0)
                            orgUser = SecurityManager.GetOrganizationUserInfoByUserId(appData.UserID).FirstOrDefault();
                        else
                            orgUser = SecurityManager.GetOrganizationUserInfoByUserId(appData.UserID).Where(con => con.Organization.TenantID == appData.TenantID).FirstOrDefault();
                        View.UserName = orgUser.aspnet_Users.UserName;
                        SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(View.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;

                        // Checks if the user is locked.
                        if (!user.IsNull() && user.IsLockedOut)
                        {
                            //View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED);
                            View.ErrorMessage = View.AccountLockedMessage;
                            return;
                        }

                        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                        // Checks if the user is Active.
                        if (!orgUser.IsNull() && (orgUser.IsApplicant ?? false) && !orgUser.IsActive)
                        {
                            View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ERROR_INACTIVE_ACCOUNT);
                            return;
                        }

                        //UAT-1346: As an Agency user, I should be able to create and maintain other agency users.
                        //if user type is shared user then set shared user's sysX Block id
                        if (GetUserType(user) == UserType.SHAREDUSER && user.SharedUserTypesCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                        {
                            lkpSysXBlock sysXBlock = SecurityManager.GetSysXBlockByCode(AppConsts.SHARED_USER_SYSX_BLOCK_CODE);
                            if (!sysXBlock.IsNull())
                            {
                                View.SelectedBlockId = (sysXBlock != null) ? sysXBlock.SysXBlockId : View.SelectedBlockId;
                                View.SelectedBlockName = (sysXBlock != null) ? sysXBlock.Name : View.SelectedBlockName;
                            }
                        }
                        else
                        {
                            // Check if the user is assigned to any of the LOB.
                            lkpSysXBlock sysxblock = SecurityManager.GetDefaultLineOfBusinessByUserName(View.UserName, orgUser.Organization.TenantID);

                            //This check is removed, because applicant will not have any business channel and we have to allow him to login 
                            if (!sysxblock.IsNull() || (orgUser != null && ((orgUser.IsApplicant ?? false) || (orgUser.IsSharedUser ?? false))))
                            {
                                View.SelectedBlockId = (sysxblock != null) ? sysxblock.SysXBlockId : View.SelectedBlockId;
                                View.SelectedBlockName = (sysxblock != null) ? sysxblock.Name : View.SelectedBlockName;
                            }
                            else
                            {
                                SysXWebSiteUtils.SessionService.ClearSession(false);
                                View.ErrorMessage = (SysXUtils.GetMessage(ResourceConst.SECURITY_NOTASSIGNED_TO_LINEOFBUSINESS));
                                return;
                            }
                        }
                        SysXWebSiteUtils.SessionService.SetSysXBlockId(View.SelectedBlockId);
                        SysXWebSiteUtils.SessionService.SetSysXBlockName(View.SelectedBlockName);
                        SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
                        SecurityManager.ResetPasswordAttempCount(View.UserName);
                        FormsAuthentication.RedirectFromLoginPage(View.UserName, false);

                        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                        if (!appData.AdminOrgUserID.IsNullOrEmpty() && appData.UserTypeSwitchViewCode == UserTypeSwitchView.Applicant.GetStringValue())
                        {
                            System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"] = Convert.ToString(appData.AdminOrgUserID);
                        }
                        if (!appData.AdminOrgUserID.IsNullOrEmpty() && appData.UserTypeSwitchViewCode == UserTypeSwitchView.AgencyUser.GetStringValue())
                        {
                            System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"] = Convert.ToString(appData.AdminOrgUserID);
                        }
                    }
                    applicantData.Remove(tokenKey);
                    Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(applicantData);
                    ApplicationDataManager.UpdateWebApplicationData("ApplicantInstData", serializedData);
                }
            }
        }

        /// <summary>
        /// Get the type of User trying to login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserType GetUserType(SysXMembershipUser user)
        {
            if (user.IsApplicant.IsNotNull() && user.IsApplicant)
                return UserType.APPLICANT;
            else if (!user.IsApplicant && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                return UserType.CLIENTADMIN;
            else if (!user.IsApplicant && (user.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()))
                return UserType.THIRDPARTYADMIN;
            else if (user.IsSharedUser.IsNotNull() && user.IsSharedUser)
                return UserType.SHAREDUSER;
            else
                return UserType.SUPERADMIN;
        }

        /// <summary>
        /// Retrieves all the Line of Businesses based on current user's Id.
        /// </summary>
        /// <param name="currentUserId">value of current user's Id.</param>
        /// <returns></returns>
        public Int32 GetLineOfBusinessesByUser(String currentUserId, Int32 TenantId)
        {
            Int32 result = 0;
            var LOBList = SecurityManager.GetLineOfBusinessesByUser(currentUserId).ToList();
            if (TenantId > AppConsts.NONE)
            {
                LOBList = LOBList.Where(cond => cond.TenantID == TenantId).ToList();
            }
            if (LOBList.IsNotNull())
            {
                result = LOBList.Count();

                if (LOBList.Count().Equals(AppConsts.ONE))
                {
                    ModuleUtility.ModuleUtils.SessionService.BusinessChannelType =
                        new BusinessChannelTypeMappingData
                        {
                            BusinessChannelTypeID = LOBList.FirstOrDefault().BusinessChannelTypeID.Value,
                            BusinessChannelTypeName = SecurityManager.GetBusinessChannelTypes()
                            .FirstOrDefault(cond => cond.BusinessChannelTypeID == LOBList.FirstOrDefault().BusinessChannelTypeID.Value).Name
                        };
                }

                #region UAT-2458, As an adb admin, i should be logged in directly to the tracking side of the system
                else if (result > AppConsts.ONE)
                {
                    short businessChannelTypeID = 0;
                    String businessChannelTypeName = String.Empty;
                    vw_UserAssignedBlocks userAssignedBussinessChannel = LOBList.Where(x => x.BusinessChannelTypeID == AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE).FirstOrDefault();
                    if (userAssignedBussinessChannel.IsNotNull())
                    {
                        businessChannelTypeID = LOBList.Where(x => x.BusinessChannelTypeID == AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE).FirstOrDefault().BusinessChannelTypeID.Value;
                        businessChannelTypeName = SecurityManager.GetBusinessChannelTypes().FirstOrDefault(cond => cond.BusinessChannelTypeID == businessChannelTypeID).Name;
                    }
                    else
                    {
                        businessChannelTypeID = LOBList.FirstOrDefault().BusinessChannelTypeID.Value;
                        businessChannelTypeName = SecurityManager.GetBusinessChannelTypes().FirstOrDefault(cond => cond.BusinessChannelTypeID == businessChannelTypeID).Name;
                    }

                    ModuleUtility.ModuleUtils.SessionService.BusinessChannelType =
                        new BusinessChannelTypeMappingData
                        {
                            BusinessChannelTypeID = businessChannelTypeID,
                            BusinessChannelTypeName = businessChannelTypeName
                        };
                    result = AppConsts.ONE; //To Skip Select Business Channel Page
                }
                #endregion

            }
            return result;
        }

        public Boolean IsAttestationDocumentAlreadySubmitted(Int32 orgUserID)
        {
            return SecurityManager.IsAttestationDocumentAlreadySubmitted(orgUserID);
        }

        /// <summary>
        /// Method to Check whether Client admin has any Bkg Feature or not
        /// </summary>
        /// <returns></returns>
        public Boolean CheckForClientRoleFeatures(Guid userID)
        {
            return SecurityManager.CheckForClientRoleFeatures(userID);
        }

        /// <summary>
        /// Method to check if client admin has any Employment Node Permission
        /// </summary>
        /// <returns></returns>
        public Boolean CheckEmploymentNodePermission(Int32 tenantID, Int32 userID)
        {
            return ComplianceDataManager.CheckEmploymentNodePermission(tenantID, userID);
        }

        /// <summary>
        /// UAT-1741, 604 notification should only have to be clicked upon login once per 24 hours.
        /// </summary>
        /// <param name="organizationUserID"></param>
        /// <returns></returns>
        public Boolean IsEDFormPreviouslyAccepted(Int32 organizationUserID)
        {
            Double employmentDisclosureIntervalHours = AppConsts.NONE;
            if (!ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"].IsNullOrEmpty())
            {
                employmentDisclosureIntervalHours = Convert.ToDouble(ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"]);
            }
            return SecurityManager.IsEDFormPreviouslyAccepted(organizationUserID, employmentDisclosureIntervalHours);
        }

        /// <summary>
        /// Get the Institute Url to which the applicant should be redirected to
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public String GetInstitutionUrl(Int32 tenantId)
        {
            return WebSiteManager.GetInstitutionUrl(tenantId);
        }

        /// <summary>
        /// Get the applicant data from the 'WebApplicationData' table, before being re-directed to the apporpriate Url,
        /// in case of incorrect url selection for login
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<String, ApplicantInsituteDataContract> GetDataByKey(String key)
        {
            Object applicationData = ApplicationDataManager.GetObjectDataByKey(key);
            return ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
        }

        /// <summary>
        /// Add the applicant data to 'WebApplicationData' table, before being redirected to appropriate Url, 
        /// if it is already not added with the same key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.UpdateWebApplicationData(key, serializedData);
        }

        /// Add the applicant data to 'WebApplicationData' table, before being redirected to appropriate Url, 
        /// if it is already not added with the same key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
        }

        /// <summary>
        /// It validates the user, and then redirect to login page.
        /// </summary>
        public void ValidateUserViaEmailAndRedirect(String verificationCode)
        {
            OrganizationUser orgUser = SecurityManager.GetOrganizationUserByVerificationCode(verificationCode);
            if (orgUser.IsNotNull())
            {
                //UAT-2494, New Account verification enhancements (additional verification step)
                Int32 tenantID = Convert.ToInt32(orgUser.Organization.TenantID);
                String settingCode = Setting.ACCOUNT_VERIFICATION_PROCESS_MAIN.GetStringValue();
                //Get permission for Additional Account Verification, If any value is YES then Show Additional Step.
                Boolean isAdditionAccVerificationSettingEnabled = ComplianceDataManager.GetClientSetting(tenantID, settingCode).IsNullOrEmpty() ? false : (ComplianceDataManager.GetClientSetting(tenantID, settingCode).CS_SettingValue == "1" ? true : false);
                if (isAdditionAccVerificationSettingEnabled)
                {
                    View.ShowAdditionalAccountVerificationPage = true;
                }
                else
                {
                    orgUser.IsActive = true;
                    //UAT-887: WB: Delay Automatic emails going out after activation
                    if (orgUser.ActiveDate == null && orgUser.IsApplicant == true)
                        orgUser.ActiveDate = DateTime.Now;
                    SecurityManager.UpdateOrganizationUser(orgUser);

                    View.VerificationMessage = ResourceConst.SECURITY_VERIFICATION_SUCCESS_MESSAGE;
                }
            }
        }

        /// <summary>
        /// It validates the user email address, and then redirect to login page.
        /// </summary>
        public void ValidateEmailAddressViaEmail(String verificationCode)
        {
            if (SecurityManager.ChangeUserEmailAddressAfterConfirmation(verificationCode.Trim()))
            {
                View.VerificationMessage = "Email Address has been updated successfully.";
            }
            else
            {
                View.ErrorMessage = "This link is invalid or expired.";
            }
        }

        #region UAT-2792

        public bool AutoLogInUsingUserName()
        {
            SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(View.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;
            if (user.IsNotNull())
            {
                GetOrganizationUserTypeMapping(user.UserId);
                IsSharedUserHasOtherRoles(user);
            }

            if (View.CheckWebsiteURL)
            {
                CheckIncorrectLoginURL(user);
            }

            // Checks if the user is locked.
            if (!user.IsNull() && user.IsLockedOut)
            {
                View.ErrorMessage = View.AccountLockedMessage;
                return false;
            }

            Entity.OrganizationUser objOrgUser = null;
            IQueryable<OrganizationUser> lstOrgUsers = SecurityManager.GetOrganizationUserInfoByUserId(user.UserId.ToString());
            objOrgUser = lstOrgUsers.FirstOrDefault(obj => obj.OrganizationID == user.OrganizationId);

            //Added for resolving bug# no validation msg appear for inactive users.
            if (!objOrgUser.IsActive)
            {
                if ((objOrgUser.IsApplicant.IsNotNull() ? objOrgUser.IsApplicant.Value == true : false) && !objOrgUser.UserVerificationCode.IsNullOrEmpty())
                {
                    View.ErrorMessage = "Your account is not active. Please";
                    View.IsAccountInActive = true;

                    Dictionary<String, String> dicEncPValue = new Dictionary<String, String>
                                                         {
                                                            { "EncPValue",  View.Password}
                                                         };
                    View.EncPValue = dicEncPValue.ToEncryptedQueryString();

                    return false;
                }
                else
                {
                    View.ErrorMessage = "Your account is not active. Please contact System Administrator.";
                    return false;
                }
            }

            //UAT-1346: As an Agency user, I should be able to create and maintain other agency users.
            //if user type is shared user then set shared user's sysX Block id
            if (GetUserType(user) == UserType.SHAREDUSER && user.SharedUserTypesCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
            {
                lkpSysXBlock sysXBlock = SecurityManager.GetSysXBlockByCode(AppConsts.SHARED_USER_SYSX_BLOCK_CODE);
                if (!sysXBlock.IsNull())
                {
                    View.SelectedBlockId = (sysXBlock != null) ? sysXBlock.SysXBlockId : View.SelectedBlockId;
                    View.SelectedBlockName = (sysXBlock != null) ? sysXBlock.Name : View.SelectedBlockName;
                }
            }
            else
            {
                // Check if the user is assigned to any of the LOB.
                lkpSysXBlock sysxblock = SecurityManager.GetDefaultLineOfBusinessByUserName(View.UserName, objOrgUser.Organization.TenantID);
                ///This check is removed, because applicant will not have any business channel and we have to allow him to login 
                if (!sysxblock.IsNull() || (objOrgUser != null && ((objOrgUser.IsApplicant ?? false) || (objOrgUser.IsSharedUser ?? false))))
                {
                    View.SelectedBlockId = (sysxblock != null) ? sysxblock.SysXBlockId : View.SelectedBlockId;
                    View.SelectedBlockName = (sysxblock != null) ? sysxblock.Name : View.SelectedBlockName;
                }
                else
                {
                    SysXWebSiteUtils.SessionService.ClearSession(false);
                    View.ErrorMessage = (SysXUtils.GetMessage(ResourceConst.SECURITY_NOTASSIGNED_TO_LINEOFBUSINESS));
                    return false;
                }
            }
            SysXWebSiteUtils.SessionService.SetSysXBlockId(View.SelectedBlockId);
            SysXWebSiteUtils.SessionService.SetSysXBlockName(View.SelectedBlockName);
            SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
            SecurityManager.ResetPasswordAttempCount(View.UserName);
            FormsAuthentication.RedirectFromLoginPage(View.UserName, false);
            return true;
        }
        #endregion

        public void IsSharedUserHasOtherRoles(SysXMembershipUser user)
        {
            List<OrganizationUser> lstOrgUsers = SecurityManager.GetOrganizationUserInfoByUserId(Convert.ToString(user.UserId)).ToList();
            View.IsSharedUserHasOtherRoles = lstOrgUsers.Any(cond => (cond.IsSharedUser ?? false) == false);
        }

        /// <summary>
        /// Method to check whether the user tries to login with Incorrect URL
        /// </summary>
        /// <param name="user"></param>
        private void CheckIncorrectLoginURL(SysXMembershipUser user)
        {
            Int32 orgId = WebSiteManager.GetOrganisationIDByURL(View.SiteUrl);

            //To get Third Party Organization IDs and check if account exists
            var thirdPartyOrgIDs = SecurityManager.GetThirdPartyOrgIDs(orgId);

            List<String> orgUserTypeCode = new List<String>();
            if (!View.OrganizationUserTypeMapping.IsNullOrEmpty())
            {
                orgUserTypeCode = View.OrganizationUserTypeMapping.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
            }

            //Case 1: If User is one of the Applicants Shared User, Agency User, Instructor or Preceptor
            if (!orgUserTypeCode.IsNullOrEmpty()
                && orgUserTypeCode.Count > AppConsts.NONE)
            {
                //If trying to login with other than ProfileSharing URL
                if (View.SiteUrl.ToLower() != View.SharedUserLoginURL.ToLower())
                {
                    if (!View.IsSharedUserHasOtherRoles)
                    {
                        View.IsIncorrectLoginUrl = true;
                        View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INSTITUTION_NO_ACCOUNT);
                    }
                    else//If shared user is also an applicant, client admin, or ADB Admin
                    {
                        if (user.OrganizationId != orgId)
                        {
                            View.IsIncorrectLoginUrl = true;
                            View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INSTITUTION_NO_ACCOUNT);
                        }
                    }
                }
            }

            //Case 2: If User is ONLY ONE of the Applicant, Client admin, ADB ADmin
            else if (orgUserTypeCode.Count == AppConsts.NONE)
            {
                //If trying to login with ProfileSharing URL
                if (View.SiteUrl.ToLower() == View.SharedUserLoginURL.ToLower())
                {
                    View.IsIncorrectLoginUrl = true;
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INSTITUTION_NO_ACCOUNT);
                }
                else
                {
                    if (user.OrganizationId != orgId)
                    {
                        View.IsIncorrectLoginUrl = true;
                        View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INSTITUTION_NO_ACCOUNT);
                    }
                }
            }
        }

        #region UAT-446 implementation

        /// <summary>
        /// Check if the user belongs to Multi Tenants
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean IsMultiTenantUser(Guid userId)
        {
            //return SecurityManager.IsMultiTenantUser(userId);
            return AlumniManager.IsMultiTenantUserExceptAlumni(userId);
        }

        #endregion

        #region UAT-2960
        public void CreateAlumniDefaultSubscription(Int32 organizationUserID, Int32 currentLoggedInUserID, Int32 sourceTenantID, String machineIP)
        {
            AlumniManager.CreateAlumniDefaultSubscription(sourceTenantID, currentLoggedInUserID, organizationUserID, machineIP);
        }

        public Boolean CheckForAlumnAccessStatus(Guid Token, Int32 orgUserId)
        {
            if (orgUserId <= AppConsts.NONE)
                return false;

            String StatusDueCode = lkpAlumniStatus.Due.GetStringValue();
            return AlumniManager.CheckForAlumnAccessStatus(StatusDueCode, Token, orgUserId);
        }

        public Int32 GetAlumniTenantId()
        {
            String alumniSettingTenantCode = AlumniSettings.AlumniTenantID.GetStringValue();
            String targetTenantID = AlumniManager.GetAlumniSettingByCode(alumniSettingTenantCode);
            return !targetTenantID.IsNullOrEmpty() ? Convert.ToInt32(targetTenantID) : AppConsts.NONE;
        }

        public Boolean IsAlumniAcessActivated(Int32 OrgUserID, Int32 TenantId)
        {
            if (TenantId < AppConsts.NONE)
                return false;
            String statusActivatedCode = lkpAlumniStatus.Activated.GetStringValue();
            return AlumniManager.AlumniAccessStatus(OrgUserID, TenantId, statusActivatedCode);
        }

        public Int32 GetWebsiteTenantId(String siteUrl)
        {
            return WebSiteManager.GetWebsiteTenantId(siteUrl);
        }

        #endregion

        public Boolean InsertAceMappLoginIntegrationEntry(Int32 organizationUserID, String ExternalId, Int32 IntegrationClientId)
        {
            return SecurityManager.InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(organizationUserID, IntegrationClientId, ExternalId);
        }

        /// <summary>
        /// It validates the URL,if it's absolute then it returns true -- UAT-5182
        /// </summary>
        /// <param name="url">website path to be checked</param>
        /// <returns>bool (true or false)</returns>
        public bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        /// <summary>
        /// It validates the user, and then redirect to login page.
        /// </summary>
        public void ValidateUserAndRedirect()
        {
            #region UAT-5182: Return URl validation
            if (Extensions.IsHtmlEncodingActivated)
            { 
                String returnUrl = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url?.Query)?.Get("ReturnURL");
                if (returnUrl.IsNotNull())
                {
                    if (IsAbsoluteUrl(returnUrl))
                    {
                        System.Web.HttpContext.Current.Response.Redirect("~/Errors/ResourceNotFound.htm");
                        return;
                    }
                }
            }
            #endregion

            SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(View.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;
            if (user.IsNotNull() && View.IsRequestExteranlLogin && View.StoreExternalLoginTenantId > AppConsts.NONE)
            {
                if (user.TenantId != View.StoreExternalLoginTenantId)
                {
                    View.ErrorMessage = "Complio does not recognize your account in this school. Please try again.";
                    return;
                }
            }
            if (user.IsNotNull())
            {
                GetOrganizationUserTypeMapping(user.UserId);
                IsSharedUserHasOtherRoles(user);
                if (View.IsShibbolethLogin && user.TenantId != View.ShibbolethHostID)
                {
                    View.ErrorMessage = "This Account is not associated with " + View.HostName;
                    return;
                }
                if (View.IsShibbolethLogin && user.IsApplicant != View.IsShibbolethApplicant)
                {
                    View.ErrorMessage = "This account is not compatible with your role.";
                    return;
                }
            }
            if (System.Web.Security.Membership.ValidateUser(Regex.Replace(View.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE), Regex.Replace(View.Password, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)))
            {
                if (View.CheckWebsiteURL)
                {
                    CheckIncorrectLoginURL(user);
                }

                // Checks if the user is locked.
                if (!user.IsNull() && user.IsLockedOut)
                {
                    View.ErrorMessage = View.AccountLockedMessage; //SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED);
                    return;
                }

                Entity.OrganizationUser objOrgUser = null;
                IQueryable<OrganizationUser> lstOrgUsers = SecurityManager.GetOrganizationUserInfoByUserId(user.UserId.ToString());
                objOrgUser = lstOrgUsers.FirstOrDefault(obj => obj.OrganizationID == user.OrganizationId);

                //Added for resolving bug# no validation msg appear for inactive users.
                if (!objOrgUser.IsActive)
                {
                    if ((objOrgUser.IsApplicant.IsNotNull() ? objOrgUser.IsApplicant.Value == true : false) && !objOrgUser.UserVerificationCode.IsNullOrEmpty())
                    {
                        View.ErrorMessage = "Your account is not active. Please";
                        View.IsAccountInActive = true;

                        Dictionary<String, String> dicEncPValue = new Dictionary<String, String>
                                                         {
                                                            { "EncPValue",  View.Password}
                                                         };
                        View.EncPValue = dicEncPValue.ToEncryptedQueryString();

                        return;
                    }
                    else
                    {
                        View.ErrorMessage = "Your account is not active. Please contact System Administrator.";
                        return;
                    }
                }


                //UAT-1346: As an Agency user, I should be able to create and maintain other agency users.
                //if user type is shared user then set shared user's sysX Block id
                if (GetUserType(user) == UserType.SHAREDUSER && user.SharedUserTypesCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                {
                    lkpSysXBlock sysXBlock = SecurityManager.GetSysXBlockByCode(AppConsts.SHARED_USER_SYSX_BLOCK_CODE);
                    if (!sysXBlock.IsNull())
                    {
                        View.SelectedBlockId = (sysXBlock != null) ? sysXBlock.SysXBlockId : View.SelectedBlockId;
                        View.SelectedBlockName = (sysXBlock != null) ? sysXBlock.Name : View.SelectedBlockName;
                    }
                }
                else
                {
                    // Check if the user is assigned to any of the LOB.
                    lkpSysXBlock sysxblock = SecurityManager.GetDefaultLineOfBusinessByUserName(View.UserName, objOrgUser.Organization.TenantID);
                    ///This check is removed, because applicant will not have any business channel and we have to allow him to login 
                    if (!sysxblock.IsNull() || (objOrgUser != null && ((objOrgUser.IsApplicant ?? false) || (objOrgUser.IsSharedUser ?? false))))
                    {
                        View.SelectedBlockId = (sysxblock != null) ? sysxblock.SysXBlockId : View.SelectedBlockId;
                        View.SelectedBlockName = (sysxblock != null) ? sysxblock.Name : View.SelectedBlockName;
                    }
                    else
                    {
                        SysXWebSiteUtils.SessionService.ClearSession(false);
                        View.ErrorMessage = (SysXUtils.GetMessage(ResourceConst.SECURITY_NOTASSIGNED_TO_LINEOFBUSINESS));
                        return;
                    }
                }
                SysXWebSiteUtils.SessionService.SetSysXBlockId(View.SelectedBlockId);
                SysXWebSiteUtils.SessionService.SetSysXBlockName(View.SelectedBlockName);
                SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
                SecurityManager.ResetPasswordAttempCount(View.UserName);


                //UAT-2930
                String authenticationMode = IsTwoFactorAuthenticationRequired(Convert.ToString(user.UserId));
                if (!authenticationMode.IsNullOrEmpty() && authenticationMode == AuthenticationMode.Google_Authenticator.GetStringValue() && !View.IsShibbolethLogin)
                {
                    View.IsTwoFactorAuthenticationRequired = true;
                    SysXWebSiteUtils.SessionService.UserGoogleAuthenticated = GoogleAuthenticationStatus.NotAuthenticated_With_GoogleAuthenticator;
                }
                else if (!authenticationMode.IsNullOrEmpty() && authenticationMode == AuthenticationMode.Text_Message.GetStringValue() && !View.IsShibbolethLogin)
                {
                    View.IsTwoFactorAuthenticationRequired = true;
                    SysXWebSiteUtils.SessionService.UserGoogleAuthenticated = GoogleAuthenticationStatus.NotAuthenticated_With_TextMessage;
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(View.UserName, false);
                }

                //UAT- 2792 If User is Logging through Shibboleth then insert entry in api.IntegrationClientOrganizationUserMap table
                if (View.IsShibbolethLogin && !View.ShibbolethUniqueIdentifier.IsNullOrEmpty() && !View.IntegrationClientId.IsNullOrEmpty())
                {
                    SecurityManager.ShibbolethPeopleSoftIDEntryInIntegrationClientOrganizationUserMap(user.OrganizationUserId, View.IntegrationClientId, View.ShibbolethUniqueIdentifier);
                }
            }
            else
            {
                IfPasswordFailed(user);
            }
        }

        #region UAT-2930

        private String IsTwoFactorAuthenticationRequired(String userId)
        {
            return SecurityManager.IsneedToRedirectToGoogleAuthentication(userId);
        }

        #endregion

        /// <summary>
        /// Method to check Failed Password count
        /// </summary>
        /// <param name="user"></param>
        private void IfPasswordFailed(SysXMembershipUser user)
        {
            View.ErrorMessage = String.Empty;
            SecurityManager.FailedPasswordAttemptCount(View.UserName, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempt"]));

            if (!user.IsNull())
            {
                OrganizationUser orgUser = SecurityManager.GetOrganizationUser(user.OrganizationUserId);

                if (user.IsNewPassword)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_PWD_RECEIVED_IN_EMAIL);
                }
                else
                {
                    //View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
                    View.ErrorMessage = View.InvalidUsernamePswd;
                }
            }
            else
            {
                //View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
                View.ErrorMessage = View.InvalidUsernamePswd;
            }
        }

        #region UAT-4097
        public Boolean IsUserExixtInLocationTenants(Guid userId)
        {
            return SecurityManager.IsUserExixtInLocationTenants(userId);
        }
        public Boolean IsPasswordNeedToBeChanged(Guid userId, Int32 expiryDays)
        {
            return SecurityManager.IsPasswordNeedToBeChanged(userId, expiryDays);
        }
        #endregion


        #region Hr Admin Entry Portal

        /// <summary>
        /// To get membership data for admin entry user
        /// <param name="tokenKey"></param>
        public AdminEntryUserLoginContract ManageHrAdminPortalUserLogin(String tokenKey)
        {
            AdminEntryUserLoginContract applicationData = SecurityManager.GetAdminEntryUserByToken(new Guid(tokenKey));            
            if (!applicationData.IsNullOrEmpty() && !applicationData.organizationUser.IsNullOrEmpty())
            {
                //Dictionary<String, ApplicantInsituteDataContract> applicantData = ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
                // ApplicantInsituteDataContract appData = applicantData.GetValue(tokenKey);
                // if (appData.IsNotNull())
                // {
                View.IsIncorrectLoginUrl = false;
                // var diffInSeconds = (DateTime.Now - appData.TokenCreatedTime).TotalSeconds;
                //Token expiry time is 60 seconds or 1 minute. If token is not expired then auto login
                // if (diffInSeconds <= 60)
                //  {
                //OrganizationUser orgUser;
                //if (appData.TenantID == 0)
                //    orgUser = SecurityManager.GetOrganizationUserInfoByUserId(appData.UserID).FirstOrDefault();
                //else
                //    orgUser = SecurityManager.GetOrganizationUserInfoByUserId(appData.UserID).Where(con => con.Organization.TenantID == appData.TenantID).FirstOrDefault();
                View.UserName = "AdminEntryDummyUser";
                //SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(View.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;

                SysXMembershipUser user = new SysXMembershipUser("SysXMembershipProvider",
                                                   "AdminEntryDummyUser",
                                                  applicationData.organizationUser.UserID.ToString(),
                                                  applicationData.organizationUser.PrimaryEmailAddress,
                                                  null,
                                                  null,
                                                  true,
                                                  false,
                                                  applicationData.organizationUser.CreatedOn,
                                                  DateTime.Now.ToLocalTime(),
                                                  applicationData.organizationUser.LastActivityDate.IsNullOrEmpty()?applicationData.organizationUser.CreatedOn.ToLocalTime() :Convert.ToDateTime(applicationData.organizationUser.LastActivityDate).ToLocalTime(),
                                                  applicationData.organizationUser.CreatedOn,
                                                  applicationData.organizationUser.CreatedOn,
                                                  applicationData.organizationUser.OrganizationUserID,
                                                  applicationData.organizationUser.UserID,
                                                  applicationData.organizationUser.OrganizationID,
                                                  applicationData.organizationUser.FirstName,
                                                  applicationData.organizationUser.LastName,
                                                  applicationData.organizationUser.IsOutOfOffice,
                                                  applicationData.organizationUser.OfficeReturnDateTime,
                                                  applicationData.organizationUser.IsNewPassword,
                                                  !applicationData.organizationUser.IsNewPassword,
                                                  false,
                                                  applicationData.organizationUser.IgnoreIPRestriction,
                                                  applicationData.TenantId,
                                                  1,
                                                  "TTYCLI",
                                                  0,
                                                  applicationData.organizationUser.IsApplicant??false,
                                                  applicationData.organizationUser.IsSystem,
                                                  applicationData.organizationUser.IsSharedUser??false,
                                                  new List<string>());
                // Checks if the user is locked.
                //if (!user.IsNull() && user.IsLockedOut)
                //{
                //    //View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED);
                //    View.ErrorMessage = View.AccountLockedMessage;
                //    return;
                //}

                //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                // Checks if the user is Active.
                if ((applicationData.organizationUser.IsApplicant ?? false) && !applicationData.organizationUser.IsActive)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ERROR_INACTIVE_ACCOUNT);
                    return new AdminEntryUserLoginContract();
                }

                //UAT-1346: As an Agency user, I should be able to create and maintain other agency users.
                //if user type is shared user then set shared user's sysX Block id
                //if (GetUserType(user) == UserType.SHAREDUSER && user.SharedUserTypesCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                //{
                //    lkpSysXBlock sysXBlock = SecurityManager.GetSysXBlockByCode(AppConsts.SHARED_USER_SYSX_BLOCK_CODE);
                //    if (!sysXBlock.IsNull())
                //    {
                //        View.SelectedBlockId = (sysXBlock != null) ? sysXBlock.SysXBlockId : View.SelectedBlockId;
                //        View.SelectedBlockName = (sysXBlock != null) ? sysXBlock.Name : View.SelectedBlockName;
                //    }
                //}
                //else
                //{
                    // Check if the user is assigned to any of the LOB.
                    //lkpSysXBlock sysxblock = SecurityManager.GetDefaultLineOfBusinessByUserName(View.UserName, orgUser.Organization.TenantID);

                    ////This check is removed, because applicant will not have any business channel and we have to allow him to login 
                    //if (!sysxblock.IsNull() || (orgUser != null && ((orgUser.IsApplicant ?? false) || (orgUser.IsSharedUser ?? false))))
                    //{
                    //    View.SelectedBlockId = (sysxblock != null) ? sysxblock.SysXBlockId : View.SelectedBlockId;
                    //    View.SelectedBlockName = (sysxblock != null) ? sysxblock.Name : View.SelectedBlockName;
                    //}
                    //else
                    //{
                    //    SysXWebSiteUtils.SessionService.ClearSession(false);
                    //    View.ErrorMessage = (SysXUtils.GetMessage(ResourceConst.SECURITY_NOTASSIGNED_TO_LINEOFBUSINESS));
                    //    return;
                    //}
               // }
                //SysXWebSiteUtils.SessionService.SetSysXBlockId(View.SelectedBlockId);
               // SysXWebSiteUtils.SessionService.SetSysXBlockName(View.SelectedBlockName);
                SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
               // SecurityManager.ResetPasswordAttempCount(View.UserName);
                FormsAuthentication.RedirectFromLoginPage("adminEntryUser", false);
                
                //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                //if (!applicationData.AdminOrgUserID.IsNullOrEmpty() && appData.UserTypeSwitchViewCode == UserTypeSwitchView.Applicant.GetStringValue())
                //{
                  //  System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"] = Convert.ToString(applicationData.OrganizationUserID);
                //}
                //if (!appData.AdminOrgUserID.IsNullOrEmpty() && appData.UserTypeSwitchViewCode == UserTypeSwitchView.AgencyUser.GetStringValue())
                //{
                //    System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"] = Convert.ToString(appData.AdminOrgUserID);
                //}
                //    }
                //  applicantData.Remove(tokenKey);
                //      Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(applicantData);
                //   ApplicationDataManager.UpdateWebApplicationData("ApplicantInstData", serializedData);
                // }
            }
            return applicationData;
        }
              
        #endregion

    }
}
