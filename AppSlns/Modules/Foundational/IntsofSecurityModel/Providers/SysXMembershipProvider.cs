#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXMembershipProvider.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Globalization;
using System.Security.Cryptography;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using Business.RepoManagers;
using System.Configuration;
using System.Collections.Generic;
using INTSOF.UI.Contract.SysXSecurityModel;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Providers
{
    /// <summary>
    /// This class handles membership provider.
    /// </summary>
    /// <remarks></remarks>
    class SysXMembershipProvider : MembershipProvider
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private const Int32 PasswordSize = AppConsts.FOURTEEN;
        private String _appName;
        private Boolean _enablePasswordReset;
        private Boolean _enablePasswordRetrieval;
        private Int32 _maxInvalidPasswordAttempts;
        private Int32 _minRequiredNonalphanumericCharacters;
        private Int32 _minRequiredPasswordLength;
        private Int32 _passwordAttemptWindow;
        private MembershipPasswordFormat _passwordFormat;
        private String _passwordStrengthRegularExpression;
        private Boolean _requiresQuestionAndAnswer;
        private Boolean _requiresUniqueEmail;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the value for password.
        /// </summary>
        /// <remarks></remarks>
        public override Boolean EnablePasswordRetrieval
        {
            get
            {
                return _enablePasswordRetrieval;
            }
        }

        /// <summary>
        /// Gets the value for password reset.
        /// </summary>
        /// <remarks></remarks>
        public override Boolean EnablePasswordReset
        {
            get
            {
                return _enablePasswordReset;
            }
        }

        /// <summary>
        /// Gets the value for Requires Question And Answer.
        /// </summary>
        /// <remarks></remarks>
        public override Boolean RequiresQuestionAndAnswer
        {
            get
            {
                return _requiresQuestionAndAnswer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [requires unique email].
        /// </summary>
        /// <remarks></remarks>
        public override Boolean RequiresUniqueEmail
        {
            get
            {
                return _requiresUniqueEmail;
            }
        }

        /// <summary>
        /// Gets the value for Requires Unique Email.
        /// </summary>
        /// <remarks></remarks>
        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return _passwordFormat;
            }
        }

        /// <summary>
        /// Gets the value for Maximum Invalid Password Attempts.
        /// </summary>
        /// <remarks></remarks>
        public override Int32 MaxInvalidPasswordAttempts
        {
            get
            {
                return _maxInvalidPasswordAttempts;
            }
        }

        /// <summary>
        /// Gets the Password Attempt Window.
        /// </summary>
        /// <remarks></remarks>
        public override Int32 PasswordAttemptWindow
        {
            get
            {
                return _passwordAttemptWindow;
            }
        }

        /// <summary>
        /// Gets the value for minimum password length required.
        /// </summary>
        /// <remarks></remarks>
        public override Int32 MinRequiredPasswordLength
        {
            get
            {
                return _minRequiredPasswordLength;
            }
        }

        /// <summary>
        /// Gets the value for Minimum Required NonAlphanumeric Characters.
        /// </summary>
        /// <remarks></remarks>
        public override Int32 MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return _minRequiredNonalphanumericCharacters;
            }
        }

        /// <summary>
        /// Gets the value for Password Strength RegularExpression.
        /// </summary>
        /// <remarks></remarks>
        public override String PasswordStrengthRegularExpression
        {
            get
            {
                return _passwordStrengthRegularExpression;
            }
        }

        /// <summary>
        /// Gets or sets the value for Application Name.
        /// </summary>
        /// <value>The name of the application.</value>
        /// <remarks></remarks>
        public override String ApplicationName
        {
            get
            {
                return _appName;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }

                if (value.Length > SysXSecurityConst.APP_NAME_MAX_SIZE)
                {
                    throw new ProviderException(SysXUtils.GetMessage(ResourceConst.SECURITY_APPNAMETOOLONG) + SysXSecurityConst.APP_NAME_MAX_SIZE + SysXUtils.GetMessage(ResourceConst.SECURITY_CHARACTERS));
                }
                _appName = value;
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Call base constructor for initialize membership.
        /// </summary>
        /// <param name="name">Value for name.</param>
        /// <param name="config">Configuration's value.</param>
        /// <remarks></remarks>
        public override void Initialize(String name, NameValueCollection config)
        {
            if (config.IsNull())
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = SysXSecurityConst.SYSX_MEMBERSHIP_PROVIDER_NAME;
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "SysXMembershipProvider");
            }

            base.Initialize(name, config);

            _enablePasswordRetrieval = SysXMembershipUtil.GetBooleanValue(config, SysXSecurityConst.ENABLE_PASSWORD_RETRIEVAL, false);
            _enablePasswordReset = SysXMembershipUtil.GetBooleanValue(config, SysXSecurityConst.ENABLE_PASSWORD_RESET, true);
            _requiresQuestionAndAnswer = SysXMembershipUtil.GetBooleanValue(config, SysXSecurityConst.REQUIRES_QUESTION_AND_ANSWER, true);
            _requiresUniqueEmail = SysXMembershipUtil.GetBooleanValue(config, SysXSecurityConst.REQUIRES_UNIQUE_EMAIL, true);
            _maxInvalidPasswordAttempts = SysXMembershipUtil.GetIntValue(config, SysXSecurityConst.MAX_INVALID_PASSWORDAT_TEMPTS, AppConsts.FIVE, false, AppConsts.NONE);
            _passwordAttemptWindow = SysXMembershipUtil.GetIntValue(config, SysXSecurityConst.PASSWORD_ATTEMPTWINDOW, AppConsts.TEN, false, AppConsts.NONE);
            _minRequiredPasswordLength = SysXMembershipUtil.GetIntValue(config, SysXSecurityConst.MIN_REQUIRED_PASSWORD_LENGTH, AppConsts.SEVEN, false, 128);
            _minRequiredNonalphanumericCharacters = SysXMembershipUtil.GetIntValue(config, SysXSecurityConst.MIN_REQUIREDNON_ALPHANUMERIC_CHARACTERS, AppConsts.ONE, true, 128);
            _passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"];

            if (!_passwordStrengthRegularExpression.IsNull())
            {
                _passwordStrengthRegularExpression = _passwordStrengthRegularExpression.Trim();

                if (!_passwordStrengthRegularExpression.Length.Equals(AppConsts.NONE))
                {
                    try
                    {
                        Regex regex = new Regex(_passwordStrengthRegularExpression);
                    }
                    catch (ArgumentException e)
                    {
                        throw new ProviderException(e.Message, e);
                    }
                }
            }
            else
            {
                _passwordStrengthRegularExpression = String.Empty;
            }

            if (_minRequiredNonalphanumericCharacters > _minRequiredPasswordLength)
            {
                throw new HttpException(SysXUtils.GetMessage(ResourceConst.SECURITY_MINREQUIRED_NON_ALPHANUMERICCHARACTERS_GREATER_THAN_MINREQUIREDPASSWORDLENGTH));
            }

            _appName = config["applicationName"];

            if (String.IsNullOrEmpty(_appName))
            {
                _appName = "SystenX";
            }

            if (_appName.Length > SysXSecurityConst.APP_NAME_MAX_SIZE)
            {
                throw new ProviderException(SysXUtils.GetMessage(ResourceConst.SECURITY_APPLICATION_NAME_TOOLONG));
            }

            String strTemp = config["passwordFormat"] ?? "Hashed";

            switch (strTemp)
            {
                case "Clear":
                    _passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                case "Encrypted":
                    _passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Hashed":
                    _passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                default:
                    throw new ProviderException(SysXUtils.GetMessage(ResourceConst.SECURITY_PASSWORD_FORMAT_INVALID));
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed && EnablePasswordRetrieval)
            {
                throw new ProviderException(SysXUtils.GetMessage(ResourceConst.SECURITY_CONFIGURED_SETTING_INVALID));
            }
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <param name="passwordQuestion">The password question.</param>
        /// <param name="passwordAnswer">The password answer.</param>
        /// <param name="isApproved">if set to <c>true</c> [is approved].</param>
        /// <param name="providerUserKey">The provider user key.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override MembershipUser CreateUser(String username,
                                                  String password,
                                                  String email,
                                                  String passwordQuestion,
                                                  String passwordAnswer,
                                                  Boolean isApproved,
                                                  Object providerUserKey,
                                                  out    MembershipCreateStatus status)
        {
            //Use BAL to create user
            status = MembershipCreateStatus.UserRejected;
            return null;
        }

        /// <summary>
        /// Changes the password question and answer.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="newPasswordQuestion">The new password question.</param>
        /// <param name="newPasswordAnswer">The new password answer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override Boolean ChangePasswordQuestionAndAnswer(String username, String password, String newPasswordQuestion, String newPasswordAnswer)
        {
            //Use BAL to change password question and answer
            return false;
        }

        /// <summary>
        /// Not Implemented. We will not be able to reverse stored password as we can storing encrypted password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="passwordAnswer">The password answer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override String GetPassword(String username, String passwordAnswer)
        {
            //We don't not need to implement this function. 
            return String.Empty;
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override Boolean ChangePassword(String username, String oldPassword, String newPassword)
        {
            //Use BAL to change password
            return false;
        }

        /// <summary>
        /// ResetPassword - Not Implemented.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="passwordAnswer">The password answer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override String ResetPassword(String username, String passwordAnswer)
        {
            //Use BAL to ResetPassword
            return String.Empty;
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <remarks></remarks>
        public override void UpdateUser(MembershipUser user)
        {
            //Use BAL to update user
        }

        /// <summary>
        /// Validating the users.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override Boolean ValidateUser(String username, String password)
        {
            try
            {
                if (SysXMembershipUtil.ValidateParameter(ref username, true, true, true, SysXSecurityConst.USER_NAME_MAX_SIZE) &&
                    SysXMembershipUtil.ValidateParameter(ref password, true, true, false, SysXSecurityConst.PASSWORD_MAX_SIZE) &&
                    CheckPassword(username, password, true, true))
                {
                    return true;
                }

                MembershipUser user = GetUser(username, false);

                if (!user.IsNull() && user.IsLockedOut)
                {
                    throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED_CONTACT_ADMINISTRATOR));
                }

                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override Boolean UnlockUser(String username)
        {
            // Use BAL to unlock user
            return false;
        }

        /// <summary>
        /// Get membership user from user key.
        /// </summary>
        /// <param name="providerUserKey">The provider user key.</param>
        /// <param name="userIsOnline">if set to <c>true</c> [user is online].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override MembershipUser GetUser(Object providerUserKey, Boolean userIsOnline)
        {
            if (providerUserKey.IsNull())
            {
                throw new ArgumentNullException("providerUserKey");
            }

            if (!(providerUserKey is Guid))
            {
                throw new ArgumentException(String.Format(SysXUtils.GetMessage(ResourceConst.SECURITY_PROVIDE_USERKEY_INVALID), "providerUserKey"));
            }

            try
            {
                aspnet_Users user = SecurityManager.GetUserById(providerUserKey.ToString(), true, true);

                if (!user.IsNull())
                {
                    aspnet_Membership membership = user.aspnet_Membership;
                    String email = membership.Email;
                    String passwordQuestion = membership.PasswordQuestion;
                    String comment = membership.Comment;
                    Boolean isApproved = membership.IsApproved;
                    DateTime dtCreate = membership.CreateDate.ToLocalTime();
                    DateTime dtLastLogin = membership.LastLoginDate.ToLocalTime();
                    DateTime dtLastActivity = membership.aspnet_Users.LastActivityDate.ToLocalTime();
                    DateTime dtLastPassChange = membership.LastPasswordChangedDate.ToLocalTime();
                    String userName = membership.aspnet_Users.UserName;
                    Boolean isLockedOut = membership.IsLockedOut;
                    DateTime dtLastLockoutDate = membership.LastLockoutDate.ToLocalTime();
                    Guid userId = membership.aspnet_Users.UserId;
                    OrganizationUser organizationUser = user.OrganizationUsers.First();

                    Int32 organizationUserId = organizationUser.OrganizationUserID;
                    Int32 organizationId = organizationUser.Organization.OrganizationID;
                    String firstName = organizationUser.FirstName;
                    String lastName = organizationUser.LastName;
                    Boolean isOutOfOffice = organizationUser.IsOutOfOffice;
                    DateTime? officeReturnDateTime = organizationUser.OfficeReturnDateTime;
                    Boolean isNewPassword = organizationUser.IsNewPassword;
                    Boolean isSystem = organizationUser.IsSystem;

                    //TODO: need to check isOldPassword & passwordReset
                    Boolean isOldPassword = !organizationUser.IsNewPassword;
                    Boolean passwordReset = false;
                    Boolean ignoreIPRestriction = organizationUser.IgnoreIPRestriction;
                    organizationUser.OrganizationReference.Load();
                    Int32? clientId = organizationUser.Organization.TenantID;
                    Int32? tenantTypeId = organizationUser.Organization.Tenant.TenantTypeID;
                    organizationUser.Organization.TenantReference.Load();
                    String tenantTypeCode = organizationUser.Organization.Tenant.lkpTenantType.TenantTypeCode;
                    Int32? productId = (clientId.IsNull() ? null : SecurityManager.GetTenantProductId((Int32)clientId));
                    Boolean isApplicant = Convert.ToBoolean(organizationUser.IsApplicant.HasValue ? organizationUser.IsApplicant.Value : false);
                    Boolean isSharedUser = Convert.ToBoolean(organizationUser.IsSharedUser.HasValue ? organizationUser.IsSharedUser.Value : false);
                    List<String> sharedUserTypeCode = SecurityManager.GetOrganizationUserTypeMapping(organizationUser.UserID).Select(x => x.lkpOrgUserType.OrgUserTypeCode).ToList();

                    return new SysXMembershipUser(Name,
                                                  userName,
                                                  providerUserKey,
                                                  email,
                                                  passwordQuestion,
                                                  comment,
                                                  isApproved,
                                                  isLockedOut,
                                                  dtCreate,
                                                  dtLastLogin,
                                                  dtLastActivity,
                                                  dtLastPassChange,
                                                  dtLastLockoutDate,
                                                  organizationUserId,
                                                  userId,
                                                  organizationId,
                                                  firstName,
                                                  lastName,
                                                  isOutOfOffice,
                                                  officeReturnDateTime,
                                                  isNewPassword,
                                                  isOldPassword,
                                                  passwordReset,
                                                  ignoreIPRestriction,
                                                  clientId,
                                                  tenantTypeId,
                                                  tenantTypeCode,
                                                  productId,
                                                  isApplicant,
                                                  isSystem,
                                                  isSharedUser,
                                                  sharedUserTypeCode);
                }

                return null;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get user from user name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="userIsOnline">if set to <c>true</c> [user is online].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override MembershipUser GetUser(String username, Boolean userIsOnline)
        {
            SysXMembershipUtil.CheckParameter(
                ref username,
                true,
                false,
                true,
                SysXSecurityConst.USER_NAME_MAX_SIZE,
                "username");

            try
            {
                aspnet_Users user = SecurityManager.GetUserByName(username, true, true);
                if (!user.IsNull())
                {
                    aspnet_Membership membership = user.aspnet_Membership;
                    String email = membership.Email;
                    String passwordQuestion = membership.PasswordQuestion;
                    String comment = membership.Comment;
                    Boolean isApproved = membership.IsApproved;
                    DateTime dtCreate = membership.CreateDate.ToLocalTime();
                    DateTime dtLastLogin = membership.LastLoginDate.ToLocalTime();
                    DateTime dtLastActivity = membership.aspnet_Users.LastActivityDate.ToLocalTime();
                    DateTime dtLastPassChange = membership.LastPasswordChangedDate.ToLocalTime();
                    Guid userId = membership.UserId;
                    Boolean isLockedOut = membership.IsLockedOut;
                    DateTime dtLastLockoutDate = membership.LastLockoutDate.ToLocalTime();
                    OrganizationUser organizationUser = null;

                    String tokenKey = String.Empty;
                    ApplicantInsituteDataContract appData = new ApplicantInsituteDataContract();
                    if (HttpContext.Current.IsNotNull() && !HttpContext.Current.Request.QueryString["TokenKey"].IsNullOrEmpty())
                    {
                        tokenKey = HttpContext.Current.Request.QueryString["TokenKey"];
                        Object applicationData = ApplicationDataManager.GetObjectDataByKey("ApplicantInstData");

                        if (applicationData != null)
                        {
                            Dictionary<String, ApplicantInsituteDataContract> applicantData = ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
                            appData = applicantData.GetValue(tokenKey);
                        }
                    }

                    //UAT-1218 If User is get switched
                    if (!appData.IsNullOrEmpty() && !appData.UserTypeSwitchViewCode.IsNullOrEmpty())
                    {
                        if (appData.UserTypeSwitchViewCode == UserTypeSwitchView.ADBAdmin.GetStringValue())
                            //adb admins
                            organizationUser = user.OrganizationUsers.Where(cond => cond.IsApplicant == false && (cond.IsSharedUser ?? false) == false && cond.Organization.TenantID == AppConsts.SUPER_ADMIN_TENANT_ID && cond.IsDeleted == false).FirstOrDefault();
                        else if (appData.UserTypeSwitchViewCode == UserTypeSwitchView.ClientAdmin.GetStringValue())
                            //client admins
                            organizationUser = user.OrganizationUsers.Where(cond => cond.IsApplicant == false && (cond.IsSharedUser ?? false) == false && cond.Organization.TenantID == appData.TenantID && cond.IsDeleted == false).FirstOrDefault();
                        else if (appData.UserTypeSwitchViewCode == UserTypeSwitchView.Applicant.GetStringValue())
                            //applicant
                            organizationUser = user.OrganizationUsers.Where(cond => cond.IsApplicant == true && cond.Organization.TenantID == appData.TenantID && (cond.IsSharedUser ?? false) == false && cond.IsDeleted == false).FirstOrDefault();
                        //UAT-1561: Instructor/Preceptor and Shared User (student share + agency user) should be different system roles.
                        else if (appData.UserTypeSwitchViewCode == UserTypeSwitchView.SharedUser.GetStringValue()
                            || appData.UserTypeSwitchViewCode == UserTypeSwitchView.InstructorOrPreceptor.GetStringValue()
                            || appData.UserTypeSwitchViewCode == UserTypeSwitchView.AgencyUser.GetStringValue())
                        {
                            //shared user
                            organizationUser = user.OrganizationUsers.Where(cond => cond.IsApplicant == false && (cond.IsSharedUser ?? false) == true && cond.IsDeleted == false).FirstOrDefault();
                            HttpContext.Current.Session["SwitchUserType"] = appData.UserTypeSwitchViewCode;
                        }
                        else
                            return null;
                    }
                    else
                    {
                        if (HttpContext.Current.IsNotNull() && HttpContext.Current.Request.IsNotNull() && HttpContext.Current.Request.Url.IsNotNull()
                            && HttpContext.Current.Request.Url.Host.IsNotNull())
                        {
                            //Chnages are made for UAT-1218
                            Int32 orgId = 0;
                            if (!System.Web.HttpContext.Current.Items["GetDataByOrgUserID"].IsNullOrEmpty())
                            {
                                Int32 orgUserID = Convert.ToInt32(System.Web.HttpContext.Current.Items["GetDataByOrgUserID"]);
                                organizationUser = user.OrganizationUsers.Where(cond => cond.IsDeleted == false && cond.OrganizationUserID == orgUserID).FirstOrDefault();
                            }
                            else if (String.Equals(HttpContext.Current.Request.Url.Host.ToString(), ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL], StringComparison.OrdinalIgnoreCase)) //UAT-3377
                            {
                                orgId = AppConsts.SHARED_USER_ORGANIZATION_ID;
                                organizationUser = user.OrganizationUsers.Where(cond => cond.IsDeleted == false && cond.OrganizationID == orgId && cond.IsSharedUser == true).FirstOrDefault();
                            }
                            else
                            {
                                orgId = WebSiteManager.GetOrganisationIDByURL(HttpContext.Current.Request.Url.Host);
                                List<OrganizationUser> lstOrgUsers = user.OrganizationUsers.Where(cond => cond.IsDeleted == false && cond.OrganizationID == orgId).ToList();
                                if (lstOrgUsers.Count > AppConsts.ONE)
                                {
                                    //case 1: adb admin/client admin 
                                    if (lstOrgUsers.Any(cond => cond.OrganizationID == orgId && cond.IsApplicant == false && (cond.IsSharedUser ?? false) == false))
                                    {
                                        organizationUser = lstOrgUsers.Where(cond => cond.OrganizationID == orgId && cond.IsApplicant == false && (cond.IsSharedUser ?? false) == false).FirstOrDefault();
                                    }
                                    //case 2: applicant
                                    else if (lstOrgUsers.Any(cond => cond.OrganizationID == orgId && cond.IsApplicant == true && (cond.IsSharedUser ?? false) == false))
                                    {
                                        organizationUser = lstOrgUsers.Where(cond => cond.OrganizationID == orgId && cond.IsApplicant == true && (cond.IsSharedUser ?? false) == false).FirstOrDefault();
                                    }
                                }
                                else
                                {
                                    organizationUser = lstOrgUsers.Where(cond => cond.IsDeleted == false && cond.OrganizationID == orgId && (cond.IsSharedUser ?? false) == false).FirstOrDefault();
                                }
                            }
                        }

                        //adb admins
                        if (organizationUser.IsNull())
                        {
                            organizationUser = user.OrganizationUsers.Where(cond => cond.IsApplicant == false && (cond.IsSharedUser ?? false) == false && cond.OrganizationID == AppConsts.SUPER_ADMIN_TENANT_ID && cond.IsDeleted == false).FirstOrDefault();
                        }
                        //client admins
                        if (organizationUser.IsNull())
                        {
                            organizationUser = user.OrganizationUsers.Where(cond => cond.IsApplicant == false && (cond.IsSharedUser ?? false) == false && cond.OrganizationID != AppConsts.SUPER_ADMIN_TENANT_ID && cond.IsDeleted == false).FirstOrDefault();
                        }
                        //applicant
                        if (organizationUser.IsNull())
                        {
                            organizationUser = user.OrganizationUsers.Where(cond => cond.IsApplicant == true && (cond.IsSharedUser ?? false) == false && cond.IsDeleted == false).FirstOrDefault();
                        }
                        //shared user
                        if (organizationUser.IsNull())
                        {
                            organizationUser = user.OrganizationUsers.Where(cond => cond.IsApplicant == false && (cond.IsSharedUser ?? false) == true && cond.IsDeleted == false).FirstOrDefault();
                        }
                        //unknown
                        if (organizationUser.IsNull())
                        {
                            return null;
                        }
                    }

                    Int32 organizationUserId = organizationUser.OrganizationUserID;
                    Int32 organizationId = organizationUser.OrganizationID;
                    String firstName = organizationUser.FirstName;
                    String lastName = organizationUser.LastName;
                    Boolean isOutOfOffice = organizationUser.IsOutOfOffice;
                    DateTime? officeReturnDateTime = organizationUser.OfficeReturnDateTime;
                    Boolean isNewPassword = organizationUser.IsNewPassword;
                    Boolean isSystem = organizationUser.IsSystem;
                    //TODO: need to check isOldPassword & passwordReset.
                    Boolean isOldPassword = !organizationUser.IsNewPassword;
                    Boolean passwordReset = false;

                    Boolean ignoreIpRestriction = organizationUser.IgnoreIPRestriction;
                    organizationUser.OrganizationReference.Load();
                    Int32? clientId = organizationUser.Organization.TenantID;
                    organizationUser.Organization.TenantReference.Load();
                    Int32? tenantTypeId = organizationUser.Organization.Tenant.TenantTypeID;
                    organizationUser.Organization.Tenant.lkpTenantTypeReference.Load();
                    String tenantTypeCode = (organizationUser.Organization.IsNull() || organizationUser.Organization.Tenant.IsNull() || organizationUser.Organization.Tenant.lkpTenantType.IsNull()) ? String.Empty : organizationUser.Organization.Tenant.lkpTenantType.TenantTypeCode;
                    Int32? productId = (clientId.IsNull() ? null : SecurityManager.GetTenantProductId((Int32)clientId));
                    Boolean isApplicant = Convert.ToBoolean(organizationUser.IsApplicant.HasValue ? organizationUser.IsApplicant.Value : false);
                    Boolean isSharedUser = Convert.ToBoolean(organizationUser.IsSharedUser.HasValue ? organizationUser.IsSharedUser.Value : false);
                    List<String> sharedUserTypeCode = SecurityManager.GetOrganizationUserTypeMapping(organizationUser.UserID).Select(x => x.lkpOrgUserType.OrgUserTypeCode).ToList();

                    //UAT-1561: Instructor/Preceptor and Shared User (student share + agency user) should be different system roles.
                    if (!appData.IsNullOrEmpty() && !appData.UserTypeSwitchViewCode.IsNullOrEmpty())
                    {
                        if (appData.UserTypeSwitchViewCode == UserTypeSwitchView.SharedUser.GetStringValue())
                        {
                            sharedUserTypeCode.Remove(OrganizationUserType.Instructor.GetStringValue());
                            sharedUserTypeCode.Remove(OrganizationUserType.Preceptor.GetStringValue());
                            sharedUserTypeCode.Remove(OrganizationUserType.AgencyUser.GetStringValue());
                        }
                        else if (appData.UserTypeSwitchViewCode == UserTypeSwitchView.InstructorOrPreceptor.GetStringValue())
                        {
                            sharedUserTypeCode.Remove(OrganizationUserType.AgencyUser.GetStringValue());
                            sharedUserTypeCode.Remove(OrganizationUserType.ApplicantsSharedUser.GetStringValue());
                        }
                        else if (appData.UserTypeSwitchViewCode == UserTypeSwitchView.AgencyUser.GetStringValue())
                        {
                            sharedUserTypeCode.Remove(OrganizationUserType.Instructor.GetStringValue());
                            sharedUserTypeCode.Remove(OrganizationUserType.Preceptor.GetStringValue());
                            sharedUserTypeCode.Remove(OrganizationUserType.ApplicantsSharedUser.GetStringValue());
                        }
                    }
                    //End

                    return new SysXMembershipUser(Name,
                                                  username,
                                                  userId.ToString(),
                                                  email,
                                                  passwordQuestion,
                                                  comment,
                                                  isApproved,
                                                  isLockedOut,
                                                  dtCreate,
                                                  dtLastLogin,
                                                  dtLastActivity,
                                                  dtLastPassChange,
                                                  dtLastLockoutDate,
                                                  organizationUserId,
                                                  userId,
                                                  organizationId,
                                                  firstName,
                                                  lastName,
                                                  isOutOfOffice,
                                                  officeReturnDateTime,
                                                  isNewPassword,
                                                  isOldPassword,
                                                  passwordReset,
                                                  ignoreIpRestriction,
                                                  clientId,
                                                  tenantTypeId,
                                                  tenantTypeCode,
                                                  productId,
                                                  isApplicant,
                                                  isSystem,
                                                  isSharedUser,
                                                  sharedUserTypeCode);
                }
                return null;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Returns username for given email id.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override String GetUserNameByEmail(String email)
        {
            SysXMembershipUtil.CheckParameter(
                ref email,
                false,
                false,
                false,
                SysXSecurityConst.EMAIL_MAX_SIZE,
                "email");
            try
            {
                return SecurityManager.GetUserNameByEmail(email);
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="deleteAllRelatedData">if set to <c>true</c> [delete all related data].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override Boolean DeleteUser(String username, Boolean deleteAllRelatedData)
        {
            //use BAL to DeleteUser
            return false;
        }

        /// <summary>
        /// Get All users.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override MembershipUserCollection GetAllUsers(Int32 pageIndex, Int32 pageSize, out Int32 totalRecords)
        {
            //Use BAL to get all users.
            totalRecords = AppConsts.NONE;
            return null;
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override int GetNumberOfUsersOnline()
        {
            //Not required.
            return AppConsts.NONE;
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="usernameToMatch">The username to match.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override MembershipUserCollection FindUsersByName(String usernameToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords)
        {
            //Use BAL layer
            totalRecords = AppConsts.NONE;
            return null;
        }

        /// <summary>
        /// Find all membership user by mailed.
        /// </summary>
        /// <param name="emailToMatch">The email to match.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override MembershipUserCollection FindUsersByEmail(String emailToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords)
        {
            //Use BAL
            totalRecords = AppConsts.NONE;
            return null;
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="passwordAnswer">The password answer.</param>
        /// <param name="requiresQuestionAndAnswer">if set to <c>true</c> [requires question and answer].</param>
        /// <param name="passwordFormat">The password format.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private String GetPasswordFromDb(String username,
                                         String passwordAnswer,
                                         Boolean requiresQuestionAndAnswer,
                                         out int passwordFormat,
                                         out int status)
        {
            //Not required
            passwordFormat = -AppConsts.ONE;
            status = -AppConsts.ONE;
            return String.Empty;
        }

        #endregion

        #region Internal

        /// <summary>
        /// This method encodes the password.
        /// </summary>
        /// <param name="pass">The pass.</param>
        /// <param name="passwordFormat">The password format.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal String EncodePassword(String pass, Int32 passwordFormat, String salt)
        {
            if (passwordFormat == AppConsts.NONE) // MembershipPasswordFormat.Clear
            {
                return pass;
            }

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet;

            Buffer.BlockCopy(bSalt, AppConsts.NONE, bAll, AppConsts.NONE, bSalt.Length);
            Buffer.BlockCopy(bIn, AppConsts.NONE, bAll, bSalt.Length, bIn.Length);
            if (passwordFormat == AppConsts.ONE) // MembershipPasswordFormat.Hashed
            {
                HashAlgorithm hashAlgorithm = HashAlgorithm.Create("SHA1");
                bRet = hashAlgorithm.ComputeHash(bAll);
            }
            else
            {
                bRet = EncryptPassword(bAll);
            }

            return Convert.ToBase64String(bRet);
        }

        #endregion

        #region Private

        private Boolean CheckPassword(String username, String password, Boolean updateLastLoginActivityDate, Boolean failIfNotApproved)
        {
            String salt;
            Int32 passwordFormat;
            return CheckPassword(username, password, updateLastLoginActivityDate, failIfNotApproved, out salt, out passwordFormat);
        }

        private Boolean CheckPassword(String username,
                                      String password,
                                      Boolean updateLastLoginActivityDate,
                                      Boolean failIfNotApproved,
                                      out String salt,
                                      out Int32 passwordFormat)
        {
            String passwdFromDb;
            Int32 status;
            Int32 failedPasswordAttemptCount;
            Int32 failedPasswordAnswerAttemptCount;
            Boolean isApproved;
            DateTime lastLoginDate, lastActivityDate;

            GetPasswordWithFormat(username, updateLastLoginActivityDate, out status, out passwdFromDb, out passwordFormat, out salt, out failedPasswordAttemptCount,
                                  out failedPasswordAnswerAttemptCount, out isApproved, out lastLoginDate, out lastActivityDate);
            if (!status.Equals(AppConsts.NONE))
            {
                return false;
            }

            if (!isApproved && failIfNotApproved)
            {
                return false;
            }

            String encodedPasswd = EncodePassword(password, passwordFormat, salt);
            bool isPasswordCorrect = passwdFromDb.Equals(encodedPasswd);

            if (isPasswordCorrect && failedPasswordAttemptCount.Equals(AppConsts.NONE) && failedPasswordAnswerAttemptCount.Equals(AppConsts.NONE))
            {
                return true;
            }

            return isPasswordCorrect;
        }

        private void GetPasswordWithFormat(String username,
                                           Boolean updateLastLoginActivityDate,
                                           out Int32 status,
                                           out String password,
                                           out Int32 passwordFormat,
                                           out String passwordSalt,
                                           out Int32 failedPasswordAttemptCount,
                                           out Int32 failedPasswordAnswerAttemptCount,
                                           out Boolean isApproved,
                                           out DateTime lastLoginDate,
                                           out DateTime lastActivityDate)
        {
            try
            {
                aspnet_Users user = SecurityManager.GetUserByName(username, true, true);

                if (user.IsNull())
                {
                    throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD));
                }

                aspnet_Membership aspMembership = user.aspnet_Membership;
                OrganizationUser orgUser = user.OrganizationUsers.FirstOrDefault(obj => obj.IsDeleted == false && obj.IsActive);

                if (!aspMembership.IsNull())
                {
                    user.LastActivityDate = DateTime.Now;
                    SecurityManager.UpdateUser(user);
                    status = AppConsts.NONE;
                    password = aspMembership.Password;
                    passwordFormat = aspMembership.PasswordFormat;
                    passwordSalt = aspMembership.PasswordSalt;
                    failedPasswordAttemptCount = aspMembership.FailedPasswordAttemptCount;
                    failedPasswordAnswerAttemptCount = aspMembership.FailedPasswordAnswerAttemptCount;
                    isApproved = aspMembership.IsApproved;
                    lastLoginDate = aspMembership.LastLoginDate;
                    lastActivityDate = aspMembership.aspnet_Users.LastActivityDate;
                }
                else
                {
                    status = -AppConsts.ONE;
                    password = null;
                    passwordFormat = AppConsts.NONE;
                    passwordSalt = null;
                    failedPasswordAttemptCount = AppConsts.NONE;
                    failedPasswordAnswerAttemptCount = AppConsts.NONE;
                    isApproved = false;
                    lastLoginDate = DateTime.Now;
                    lastActivityDate = DateTime.Now;
                }
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        private String GetEncodedPasswordAnswer(String username, String passwordAnswer)
        {
            if (!passwordAnswer.IsNull())
            {
                passwordAnswer = passwordAnswer.Trim();
            }

            if (String.IsNullOrEmpty(passwordAnswer))
            {
                return passwordAnswer;
            }

            Int32 status, passwordFormat, failedPasswordAttemptCount, failedPasswordAnswerAttemptCount;
            String password, passwordSalt;
            Boolean isApproved;
            DateTime lastLoginDate, lastActivityDate;
            GetPasswordWithFormat(username, false, out status, out password, out passwordFormat, out passwordSalt,
                                  out failedPasswordAttemptCount, out failedPasswordAnswerAttemptCount, out isApproved, out lastLoginDate, out lastActivityDate);

            if (status.Equals(AppConsts.NONE))
            {
                return EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), passwordFormat, passwordSalt);
            }

            throw new ProviderException(GetExceptionText(status));
        }

        public virtual String GeneratePassword()
        {
            return System.Web.Security.Membership.GeneratePassword(
                MinRequiredPasswordLength < PasswordSize ? PasswordSize : MinRequiredPasswordLength,
                MinRequiredNonAlphanumericCharacters);
        }

        private String GetExceptionText(Int32 status)
        {
            String exceptionText;

            switch (status)
            {
                case AppConsts.NONE:
                    return String.Empty;
                case AppConsts.ONE:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_USER_NOT_FOUND);
                    break;
                case AppConsts.TWO:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_SUPPLIED_PASSWORD_WRONG);
                    break;
                case AppConsts.THREE:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_SUPPLIED_ANSWER_PASSWORD_WRONG);
                    break;
                case AppConsts.FOUR:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_SUPPLIED_PASSWORD_INVALID);
                    break;
                case AppConsts.FIVE:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_SUPPLIED_QUESTION_PASSWORD_INVALID);
                    break;
                case AppConsts.SIX:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_SUPPLIED_ANSWER_PASSWORD_INVALID);
                    break;
                case AppConsts.SEVEN:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_SUPPIED_EMAIL_INVALID);
                    break;
                case AppConsts.NINETYNINE:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_USER_ACCOUNT_LOCKED_OUT);
                    break;
                default:
                    exceptionText = SysXUtils.GetMessage(ResourceConst.SECURITY_PROVIDER_ENCOUNTERED_UNKNOWN_ERROR);
                    break;
            }

            return exceptionText;
        }

        private Boolean IsStatusDueToBadPassword(Int32 status)
        {
            return (status >= AppConsts.TWO && status <= AppConsts.SIX || status == AppConsts.NINETYNINE);
        }

        #endregion

        #endregion
    }
}