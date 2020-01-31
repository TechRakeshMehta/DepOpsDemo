#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  AppAssetMasterPresenter.cs
// Purpose:   Presenter  for AppAssetMaster.
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using INTSOF.SharedObjects;
using System.Web.Security;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.UI.Contract.SysXSecurityModel;
using CoreWeb.IntsofSecurityModel;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ProfileSharing;


#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    public class AppMasterPresenter : Presenter<IAppMasterView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Called when [view loaded].
        /// </summary>
        /// <remarks></remarks>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        /// <summary>
        /// Called when [view initialized].
        /// </summary>
        /// <remarks></remarks>
        public override void OnViewInitialized()
        {
            object data = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID);
            if (data != null && !View.IsSharedUser)
            {
                int webSiteID = (Int32)(data);
                WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(webSiteID);
                View.SiteTitle = webSiteWebConfig.SiteTitle;
                View.HeaderHtml = webSiteWebConfig.HeaderHtml;

                View.lstWebsitePages = webSiteWebConfig.WebSite.WebSiteWebPages.Where(webPage => !webPage.IsDeleted && webPage.IsActive).OrderBy(linkOrder => linkOrder.LinkOrder).ToList();
                View.FooterHtml = webSiteWebConfig.FooterText;
            }
            //UAT-1110- Profile sharing - Getting Default Website settings for shared user login.
            else if (View.IsSharedUser)
            {
                int defaultWebsiteID = WebSiteManager.GetDefaultTenantWebsite().WebSiteID;
                WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(defaultWebsiteID);
                //View.SiteTitle = webSiteWebConfig.SiteTitle;
                View.HeaderHtml = webSiteWebConfig.HeaderHtml;
                //View.lstWebsitePages = webSiteWebConfig.WebSite.WebSiteWebPages.Where(webPage => !webPage.IsDeleted && webPage.IsActive).OrderBy(linkOrder => linkOrder.LinkOrder).ToList();
                View.FooterHtml = webSiteWebConfig.FooterText;
            }

            if (!String.IsNullOrEmpty(View.CurrentUserId))
            {
                var obj = SecurityManager.GetLineOfBusinessesByUser(View.CurrentUserId).ToList();
                if (obj != null)
                    View.AssignedBlocks = obj;
            }
        }

        /// <summary>
        /// Checks the user status.
        /// </summary>
        /// <param name="organizationUserId">The organization user id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        //public Boolean CheckUserStatus(Int32 organizationUserId)
        //{
        //    Boolean status;
        //    OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(organizationUserId);

        //    if (!organizationUser.IsNull())
        //    {
        //        Boolean aspnetUsersInRoles = SecurityManager.IsCurrentUserRoleExists(organizationUser.UserID);

        //        if (organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut.Equals(false) && organizationUser.IsActive && !aspnetUsersInRoles && !organizationUser.IsDeleted)
        //        {
        //            status = organizationUser.IsActive;
        //        }
        //        else if (SecurityManager.GetDefaultLineOfBusinessByUserName(organizationUser.aspnet_Users.UserName).IsNull())
        //        {
        //            status = false;
        //        }
        //        else
        //        {
        //            status = false;
        //        }
        //    }
        //    else
        //    {
        //        status = false;
        //    }

        //    return status;
        //}

        public Boolean CheckUserStatus(Guid userId, String userName, Int32? tenantId)
        {
            Boolean status;
            Boolean aspnetUsersInRoles = SecurityManager.IsCurrentLoggedInUserRoleExists(userId);

            if (aspnetUsersInRoles)
            {
                status = true;
            }
            else if (SecurityManager.GetDefaultLineOfBusinessByUserName(userName, tenantId).IsNull())
            {
                status = false;
            }
            else
            {
                status = false;
            }

            return status;
        }

        /// <summary>
        /// Gets the Logged-in UserId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public aspnet_Membership GetUserById(Guid userId)
        {
            return SecurityManager.GetAspnetMembershipById(userId);
        }

        public OrganizationUser IsApplicant()
        {
            OrganizationUser orgUser = null;
            if (SysXWebSiteUtils.SessionService.OrganizationUserId.IsNotNull() && SysXWebSiteUtils.SessionService.OrganizationUserId > 0)
            {
                orgUser = SecurityManager.GetOrganizationUser(SysXWebSiteUtils.SessionService.OrganizationUserId);
            }
            return orgUser;
        }

        public Boolean IsDefaultTenant()
        {
            return View.TenantId.Equals(SecurityManager.DefaultTenantID);
        }


        /// <summary>
        /// To get all multiple linked tenants for applicant
        /// </summary>
        public void GetTenants()
        {
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetUserTenants(View.CurrentUserId).Where(x => x.lkpTenantType.TenantTypeCode == clientCode).ToList();
            View.SelectedTenantId = View.TenantId.ToString();
        }

        public String GetInstitutionUrl(Boolean isClientAdmin = false)
        {
            if (isClientAdmin)
            {
                return WebSiteManager.GetInstitutionUrl(Convert.ToInt32(View.SelectedTenantIdForClientAdmin));
            }
            else
            {
                return WebSiteManager.GetInstitutionUrl(Convert.ToInt32(View.SelectedTenantId));
            }
        }

        public Int32 GetTenantId()
        {
            var organizationUser = SecurityManager.GetOrganizationUser(View.CurrentOrgUserId);
            if (organizationUser.IsNotNull() && organizationUser.Organization.IsNotNull())
            {
                return organizationUser.Organization.TenantID.Value;
            }
            return 0;
        }

        /// <summary>
        /// Does the log off.
        /// </summary>
        /// <remarks></remarks>
        public void DoLogOff(bool isLoggedIn, Int32 userLoginHistoryID)
        {
            //IssessionTimeout will be false as user is switching institution.
            UpdateUserLoginActivity(false, View.CurrentOrgUserId, userLoginHistoryID);
            if (isLoggedIn && !this.View.CurrentSessionId.IsNullOrEmpty())
            {
                
                View.ViewStateProvider.Delete(this.View.CurrentSessionId);
            }
            SysXWebSiteUtils.SessionService.ClearSession(true);
            FormsAuthentication.SignOut();
            SysXAppDBEntities.ClearContext();
        }

        public void AddWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
        }

        public Dictionary<String, ApplicantInsituteDataContract> GetDataByKey(String key)
        {
            Object applicationData = ApplicationDataManager.GetObjectDataByKey(key);
            return ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
        }

        public void UpdateWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.UpdateWebApplicationData(key, serializedData);
        }

        #endregion

        #region Private Methods



        #endregion


        #region AMS

        /// <summary>
        /// To get all multiple linked tenants for applicant
        /// </summary>
        public Dictionary<String, String> GetTenantData()
        {
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            Dictionary<String, String> tenants = new Dictionary<String, String>();
            if (user.IsNotNull())
            {
                if (SysXWebSiteUtils.SessionService.IsSysXAdmin)
                {
                    List<KeyValuePair<String, String>> tenantData = SecurityManager.GetBusinessChannelTypes().Where(cond => !cond.Code.Equals(BusinessChannelType.COMMON.GetStringValue())).Select(col => new KeyValuePair<String, String>(col.BusinessChannelTypeID.ToString(), col.Name)).ToList();
                    tenants.AddRange(tenantData);
                    View.InstituteLabelText = "&nbsp;|&nbsp;Line Of Business&nbsp;";
                }
                else if (user.IsApplicant && !View.IsSharedUserLoginURL)
                {
                    String clientCode = TenantType.Institution.GetStringValue();
                    View.lstTenant = SecurityManager.GetUserTenants(View.CurrentUserId).Where(x => x.lkpTenantType.TenantTypeCode == clientCode).ToList();
                    View.SelectedTenantId = View.TenantId.ToString();
                    List<KeyValuePair<String, String>> tenantData = View.lstTenant.Select(col => new KeyValuePair<String, String>(col.TenantID.ToString(), col.TenantName)).ToList();
                    tenants.AddRange(tenantData);
                    View.SelectedTenantId = View.TenantId.ToString();
                }
                else if ((user.IsSystem || user.IsSysXAdmin) && !View.IsSharedUserLoginURL)
                {
                    IQueryable<vw_UserAssignedBlocks> userAssignedBlocks;
                    userAssignedBlocks = SecurityManager.GetLineOfBusinessesByUser(user.UserId.ToString()); //for adb admin
                    List<KeyValuePair<String, String>> tenantData = userAssignedBlocks.ToList().Select(col =>
                        new KeyValuePair<String, String>(
                            col.SysXBlockId + "#" + col.BusinessChannelTypeID
                            , col.NAME + " (" + SecurityManager.GetBusinessChannelTypes()
                            .FirstOrDefault(cond => cond.BusinessChannelTypeID == col.BusinessChannelTypeID.Value).Name + ")")
                            ).ToList();
                    tenants.AddRange(tenantData);
                    View.SelectedTenantId = View.BlockID + "#" + SysXWebSiteUtils.SessionService.BusinessChannelType.BusinessChannelTypeID;
                    View.InstituteLabelText = "&nbsp;|&nbsp;Line Of Business&nbsp;";
                }
                else if (!user.IsApplicant && !View.IsSharedUserLoginURL
                        && (user.TenantTypeCode == TenantType.Institution.GetStringValue() || (!user.IsSystem && !user.IsSysXAdmin)))
                {
                    IQueryable<vw_UserAssignedBlocks> userAssignedBlocks;
                    userAssignedBlocks = SecurityManager.GetLineOfBusinessesByUser(user.UserId.ToString()).Where(col => col.TenantID == View.TenantId);
                    List<KeyValuePair<String, String>> tenantData = userAssignedBlocks.ToList().Select(col =>
                        new KeyValuePair<String, String>(
                            col.SysXBlockId + "#" + col.BusinessChannelTypeID
                            , col.NAME + " (" + SecurityManager.GetBusinessChannelTypes()
                            .FirstOrDefault(cond => cond.BusinessChannelTypeID == col.BusinessChannelTypeID.Value).Name + ")")
                            ).ToList();
                    tenants.AddRange(tenantData);
                    View.SelectedTenantId = View.BlockID + "#" + SysXWebSiteUtils.SessionService.BusinessChannelType.BusinessChannelTypeID;
                    View.InstituteLabelText = "&nbsp;|&nbsp;Line Of Business&nbsp;";
                }
                else if (user.IsSharedUser && View.IsSharedUserLoginURL)
                {
                    return new Dictionary<String, String>();
                }
            }

            return tenants;
        }

        public Dictionary<String, String> GetTenantDataForClientAdmin()
        {
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            Dictionary<String, String> tenants = new Dictionary<String, String>();
            if (user.IsNotNull())
            {
                String clientCode = TenantType.Institution.GetStringValue();
                View.lstTenant = SecurityManager.GetClientAdminTenants(View.CurrentUserId, true).Where(x => x.lkpTenantType.TenantTypeCode == clientCode).ToList();
                View.SelectedTenantIdForClientAdmin = View.TenantId.ToString();
                List<KeyValuePair<String, String>> tenantData = View.lstTenant.Select(col => new KeyValuePair<String, String>(col.TenantID.ToString(), col.TenantName)).ToList();
                tenants.AddRange(tenantData);
                View.SelectedTenantIdForClientAdmin = View.TenantId.ToString();
            }
            return tenants;
        }

        public List<lkpBusinessChannelType> GetBusinessChannelType()
        {
            return SecurityManager.GetBusinessChannelTypes();
        }

        public Int32 GetDefaultTenantID()
        {
            return SecurityManager.DefaultTenantID;
        }

        #endregion

        /// <summary>
        /// Update user logout time in User Login Activity
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="currentSessionId"></param>
        /// <param name="isSessionTimeout"></param>
        public void UpdateUserLoginActivity(Boolean isSessionTimeout, Int32 currentLogedInUserId, Int32 userLoginHistoryID)
        {
            SecurityManager.UpdateUserLoginActivity(currentLogedInUserId, View.CurrentSessionId, isSessionTimeout, userLoginHistoryID);
        }

        #region UAT-1304
        public List<OrganizationUserTypeMapping> GetOrganizationUserTypeMapping(Guid userId)
        {
            //List<OrganizationUserTypeMapping> listOrganizationUserTypeMapping = SecurityManager.GetOrganizationUserTypeMapping(userId);
            //return listOrganizationUserTypeMapping.Any(x => (x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Instructor.GetStringValue()) || (x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue()));
            return SecurityManager.GetOrganizationUserTypeMapping(userId);
        }
        #endregion

        #region UAT-1218

        public void GetOrganizationUserInfoByUserId(Guid userID)
        {
            List<OrganizationUser> lstOrgUser = SecurityManager.GetOrganizationUserInfoByUserId(Convert.ToString(userID)).ToList();
            if (!lstOrgUser.IsNullOrEmpty())
                View.LstOrganizationUser = lstOrgUser;
            else
                View.LstOrganizationUser = new List<OrganizationUser>();
        }

        public void GetUserTypeSwitchView()
        {
            View.lstUserTypeSwitchView = SecurityManager.GetAllUserTypeSwitchView();
        }

        public String GetSwitchingTargetUrl(Int32 tenantID)
        {
            //if (View.UserTypeSwitchViewCode == UserTypeSwitchView.Applicant.GetStringValue())
            return WebSiteManager.GetInstitutionUrl(tenantID);
            //else
            //  return String.Empty;
        }

        #endregion

        public Entity.SharedDataEntity.AgencyUser GetAgencyUserByUserID()
        {
            return ProfileSharingManager.GetAgencyUserByUserID(View.CurrentUserId);
        }
        #region UAT-3316(Ability to create Agency User permission "templates"_
        public AgencyUsrTempPermisisonsContract GetAgencyUserPermisisonTemplateMappings(Int32 templateId)
        {
            List<AgencyUserPermissionTemplateMapping> templatePermisisons = ProfileSharingManager.GetAgencyUsrPerTemplateMappings(templateId).ToList();
            AgencyUsrTempPermisisonsContract objPerTemplates = new AgencyUsrTempPermisisonsContract();
            //GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue());
            GetAgencyUserPermissionAccessTypeID(AgencyUserPermissionAccessType.YES.GetStringValue());

            if (templatePermisisons.Count > 0)
            {
                objPerTemplates.AGU_RotationPackagePermission = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ROTATION_PACKAGE_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_RotationPackageViewPermission = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ROTATION_PACKAGE_VIEW_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_AllowJobPosting = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ALLOW_JOB_POSTING_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_AgencyApplicantStatus = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.AGENCY_APPLICANT_STATUS_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_AgencyUserPermission = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.AGENCY_USER_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_AttestationReport = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AGUPTM_IsDeleted));
            }
            return objPerTemplates;
        }

        public AgencyUsrTempPermisisonsContract GetAgencyUsrPermisisonMappings(Int32 usrId)
        {
            List<AgencyUserPermission> usrPermisisons = ProfileSharingManager.GetAgencyUsrPermisisonMappings(usrId).ToList();
            AgencyUsrTempPermisisonsContract objPerTemplates = new AgencyUsrTempPermisisonsContract();
            GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue());
            GetAgencyUserPermissionAccessTypeID(AgencyUserPermissionAccessType.YES.GetStringValue());

            if (usrPermisisons.Count > 0)
            {
                objPerTemplates.AGU_RotationPackagePermission = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ROTATION_PACKAGE_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_RotationPackageViewPermission = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ROTATION_PACKAGE_VIEW_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_AllowJobPosting = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ALLOW_JOB_POSTING_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_AgencyApplicantStatus = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.AGENCY_APPLICANT_STATUS_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_AgencyUserPermission = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.AGENCY_USER_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_AttestationReport = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId && !x.AUP_IsDeleted));
            }
            return objPerTemplates;
        }
        #endregion


        #endregion

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

        #region UAT:-3032 Sticky Institution
        //Method to get the all tenants.
        public void GetTenantsForPreferredSelection()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenants = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        public void IsUserAllowedPreferredTenant()
        {
            View.IsUserAllowedPreferredTenant = SecurityManager.IsUserAllowedPreferredTenant(View.CurrentUserId);
        }

        #endregion

        #region Globalization

        public String GetLanguageCode(Guid userId)
        {
            var _languageContract = SecurityManager.GetLanguageCulture(userId);
            if (_languageContract.IsNotNull())
            {
                return _languageContract.LanguageCode;
            }
            return Languages.ENGLISH.GetStringValue();
        }

        public bool IsLocationServiceTenant()
        {
            Int32 tenantId = View.TenantId > AppConsts.NONE ? View.TenantId : Convert.ToInt32(View.SelectedTenantId);
            if (tenantId > AppConsts.NONE)
                return SecurityManager.IsLocationServiceTenant(tenantId);
            return false;
        }

        #endregion


        #region UAT-3664

        public void GetAgencyUserReportPermissions(Int32 agencyUserId)
        {

            //lstAgencyUserReportPermissions of type AgencyUserReportPermissionContract
            View.lstAgencyUserReportPermissions = ProfileSharingManager.GetAgencyUserReportPermissions(agencyUserId);
            // IsAllReportsNotVisible();
        }

        public void GetAgencyUserReports()
        {
            View.lstAgencyUserReports = ProfileSharingManager.GetAgencyUserReports();
        }

        public void IsAllReportsNotVisible(Int32 agencyUserId)
        {
            View.IsAllReportsNotVisible = false;
            String noPermissionAccessType = AgencyUserPermissionAccessType.NO.GetStringValue();

            GetAgencyUserReports();
            GetAgencyUserReportPermissions(agencyUserId);

            List<AgencyUserReportPermissionContract> lstNotVisibleAgencyUserReports = View.lstAgencyUserReportPermissions.Where(con => con.PermissionAccessTypeCode == noPermissionAccessType).ToList();
            if (View.lstAgencyUserReports.Count() == lstNotVisibleAgencyUserReports.Count())
                View.IsAllReportsNotVisible = true;
        }
        #endregion

        #region Admin Entry Portal
        public List<aspnet_Roles> GetUserRolesById(String userId)
        {
            return SecurityManager.GetUserRolesById(userId).ToList();
        }

        public bool CheckIfUserIsEnroller(String userID)
        {
            return SecurityManager.CheckIfUserIsEnroller(userID);
        }

        public Dictionary<Boolean,Int32> GetMenuItems(string UserID, List<String> BlockIDs, Int32 BusinessChannelTypeID)
        {
            Boolean isReactAppURl = false;
            Dictionary<Boolean, Int32> dcBlock = new Dictionary<Boolean, Int32>();
            BlockIDs.ForEach(blockId =>
            {
                if (!isReactAppURl)
                {
                    isReactAppURl = SecurityManager.GetMenuItemsForRedirection(UserID, Convert.ToInt32(blockId), BusinessChannelTypeID);
                    dcBlock.Add(isReactAppURl, Convert.ToInt32(blockId));
                }
            });
            return dcBlock;
        }

        #endregion
    }
}