#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSecurityConst.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils.Consts
{
    /// <summary>
    /// This class defines the constant used in security module.
    /// </summary>
    public static class SysXSecurityConst
    {
        #region Security Constants

        /// <summary> Name of the sysx Default Country </summary>
        public const String SYSX_DEFAULT_COUNTRY_NAME = "USA";

        /// <summary> Name of the sysx admin role key </summary>
        public const String SYSX_ADMIN_ROLE_KEY_NAME = "SysXAdminRoleName";

        /// <summary> Name of the sysx system fun menu key </summary>
        public const String SYSX_SYS_FUN_MENU_KEY_NAME = "securityFunctionKey";

        /// <summary> Name of the sitemap root node </summary>
        public const String SITEMAP_ROOT_NODE_NAME = "RootNode";

        /// <summary> The sitemap root node key </summary>
        public const String SITEMAP_ROOT_NODE_KEY = "__ROOT__";

        /// <summary> URL of the sitemap root node </summary>
        public const String SITEMAP_ROOT_NODE_URL = "~/Default.aspx";

        /// <summary> Size of the application name maximum </summary>
        public const Int32 APP_NAME_MAX_SIZE = 256;

        /// <summary> Size of the user name maximum </summary>
        public const Int32 USER_NAME_MAX_SIZE = 256;

        /// <summary> Size of the email maximum </summary>
        public const Int32 EMAIL_MAX_SIZE = 256;

        /// <summary> Size of the role name maximum </summary>
        public const Int32 ROLE_NAME_MAX_SIZE = 256;

        /// <summary> Size of the password maximum </summary>
        public const Int32 PASSWORD_MAX_SIZE = 128;

        /// <summary> Name of the sysx role provider </summary>
        public const String SYSX_ROLE_PROVIDER_NAME = "SysXRoleProvider";

        /// <summary> Name of the sysx membership provider </summary>
        public const String SYSX_MEMBERSHIP_PROVIDER_NAME = "SysXMembershipProvider";

        /// <summary> The sysx search query string key quicksearch option </summary>
        public const String SYSX_SEARCH_QUERY_STRING_KEY_QUICKSEARCH_OPTION = "QuickSearchOption";

        /// <summary> The sysx search query string key fields </summary>
        public const String SYSX_SEARCH_QUERY_STRING_KEY_FIELDS = "Fields";

        /// <summary> The sysx search query string key field values </summary>
        public const String SYSX_SEARCH_QUERY_STRING_KEY_FIELD_VALUES = "FieldValues";

        /// <summary> The sysx search query string key field search mode </summary>
        public const String SYSX_SEARCH_QUERY_STRING_KEY_FIELD_SEARCH_MODE = "SearchMode";

        /// <summary> The enable password retrieval </summary>
        public const String ENABLE_PASSWORD_RETRIEVAL = "enablePasswordRetrieval";

        /// <summary> The enable password reset </summary>
        public const String ENABLE_PASSWORD_RESET = "enablePasswordReset";

        /// <summary> The requires question and answer </summary>
        public const String REQUIRES_QUESTION_AND_ANSWER = "requiresQuestionAndAnswer";

        /// <summary> The requires unique email </summary>
        public const String REQUIRES_UNIQUE_EMAIL = "requiresUniqueEmail";

        /// <summary> The maximum invalid password attempts </summary>
        public const String MAX_INVALID_PASSWORDAT_TEMPTS = "maxInvalidPasswordAttempts";

        /// <summary> The password attempt window </summary>
        public const String PASSWORD_ATTEMPTWINDOW = "passwordAttemptWindow";

        /// <summary> Length of the minimum required password </summary>
        public const String MIN_REQUIRED_PASSWORD_LENGTH = "minRequiredPasswordLength";

        /// <summary> The minimum requirednon alphanumeric characters </summary>
        public const String MIN_REQUIREDNON_ALPHANUMERIC_CHARACTERS = "minRequiredNonalphanumericCharacters";

        /// <summary>
        /// returnedChildSuppliers
        /// </summary>
        public const String RETURNEDCHILDTENANTS = "returnedChildTenantss";

        /// <summary>
        /// SupplierId
        /// </summary>
        public const String TENANTID = "TenantId";

        /// <summary>
        /// SupplierId
        /// </summary>
        public const String INITINSERT = "InitInsert";

        /// <summary>
        /// CheckBoxId
        /// </summary>
        public const String CHECKBOXID = "CheckBoxId";

        /// <summary>
        /// chkHasChild
        /// </summary>
        public const String CHKHASCHILD = "chkHasChild";

        /// <summary>
        /// ManageSubTenant
        /// </summary>
        public const String MANAGESUBTENANT = "ManageSubTenant";

        /// <summary>
        /// User group added
        /// </summary>
        public const String USERGROUPINSERTED = "UserGroupInserted";

        /// <summary>
        /// User group updated
        /// </summary>
        public const String USERGROUPUPDATED = "UserGroupUpdated";

        /// <summary>
        /// User group deleted
        /// </summary>
        public const String USERGROUPDELETED = "UserGroupDeleted";

        /// <summary>
        /// User added in user group
        /// </summary>
        public const String USERADDEDINUSERGROUP = "UserAddedInUserGroup";

        /// <summary>
        /// User removed from user group
        /// </summary>
        public const String USERREMOVEDFROMUSERGROUP = "UserRemovedFromUserGroup";
        public const String ASCIISPACE = "&#32";

        public const String CHECK_WEBSITE_URL_LOGIN = "checkWebsiteURL";
        public const String REDIRECT_TO_MOBILE_SITE = "redirectToMobileSite";
        public const String MOBILE_DEVICE_AUTO_REDIRECT_SITES = "MobileDeviceAutoRedirectSites";


        #endregion
    }
}