#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXRoleProvider.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Collections.Specialized;
using System.Configuration.Provider;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity;
using INTSOF.Utils;
using INTSOF.Utils.Consts;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Providers
{
    /// <summary>
    /// Handles the operations related to role provider.
    /// </summary>
    class SysXRoleProvider : RoleProvider
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the value for Application Name.
        /// </summary>
        public override String ApplicationName 
        { 
            get; 
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Call base initialize method of RoleProvider
        /// </summary>
        /// <param name="name">value for name.</param>
        /// <param name="config">value for configurations.</param>
        public override void Initialize(String name, NameValueCollection config)
        {
            if (config.IsNull())
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
                name = SysXSecurityConst.SYSX_ROLE_PROVIDER_NAME;

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom SysX Role Provider");
            }

            base.Initialize(name, config);

            ApplicationName = config["applicationName"];

            if (String.IsNullOrEmpty(ApplicationName))
                ApplicationName = SysXMembershipUtil.GetDefaultAppName();

            if (ApplicationName.Length > SysXSecurityConst.APP_NAME_MAX_SIZE)
            {
                throw new ProviderException(String.Format(SysXUtils.GetMessage(ResourceConst.SECURITY_APPLICATION_NAME_TOLONG)));
            }

            config.Remove("applicationName");
            config.Remove("commandTimeout");

            if (config.Count > AppConsts.NONE)
            {
                String attribUnrecognized = config.GetKey(AppConsts.NONE);
               
                if (!String.IsNullOrEmpty(attribUnrecognized))
                {
                    throw new ProviderException(String.Format("The'{0}' is Unrecognized", attribUnrecognized));
                }
            }
        }

        /// <summary>
        /// Find user in assigned role.
        /// </summary>
        /// <param name="roleName">value for role's name.</param>
        /// <param name="userNameToMatch">value for matching user's name.</param>
        /// <returns></returns>
        public override String[] FindUsersInRole(String roleName, String userNameToMatch)
        {
            SysXMembershipUtil.CheckParameter(ref roleName, true, true, true, SysXSecurityConst.ROLE_NAME_MAX_SIZE, "roleName");
            SysXMembershipUtil.CheckParameter(ref userNameToMatch, true, true, false, SysXSecurityConst.USER_NAME_MAX_SIZE, "userNameToMatch");

            try
            {
                return SecurityManager.FindUsersInRole(roleName, userNameToMatch);
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
        /// Retrieves a list of roles.
        /// </summary>
        /// <returns></returns>
        public override String[] GetAllRoles()
        {
            try
            {
                List<aspnet_Roles> roles = SecurityManager.GetRoles(false).ToList();
                var role = from r in roles
                           select r.RoleName;
                return role.ToArray();
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
        /// Get list of roles for particular user.
        /// </summary>
        /// <param name="username">Value for user's name.</param>
        /// <returns></returns>
        public override String[] GetRolesForUser(String username)
        {
            SysXMembershipUtil.CheckParameter(ref username, true, false, true, SysXSecurityConst.USER_NAME_MAX_SIZE, "username");
            
            if (username.Length < AppConsts.ONE)
            {
                return new String[0];
            }

            try
            {
                if (String.IsNullOrEmpty(username))
                {
                    throw new ArgumentException("username");
                }

                List<aspnet_Roles> roles = SecurityManager.GetUserRoles(username);
                var role = from r in roles
                           select r.RoleName;
                return role.ToArray();
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
        /// Get list of users in particular role. 
        /// </summary>
        /// <param name="roleName">Value for role's name.</param>
        /// <returns></returns>
        public override String[] GetUsersInRole(String roleName)
        {
            SysXMembershipUtil.CheckParameter(ref roleName, true, true, true, SysXSecurityConst.ROLE_NAME_MAX_SIZE, "roleName");
            
            try
            {
                if (roleName.IsNull())
                {
                    throw new ArgumentNullException("rolename");
                }

                if (String.IsNullOrEmpty(roleName))
                {
                    throw new ArgumentException("rolename");
                }

                List<aspnet_Users> users = SecurityManager.GetUsersInRole(roleName);
                var userNames = from u in users
                                select u.UserName;

                return userNames.ToArray();
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
        /// check user for particular role.
        /// </summary>
        /// <param name="username">Value for user's name.</param>
        /// <param name="roleName">Value for role's name.</param>
        /// <returns></returns>
        public override Boolean IsUserInRole(String username, String roleName)
        {
            SysXMembershipUtil.CheckParameter(ref roleName, true, true, true, SysXSecurityConst.ROLE_NAME_MAX_SIZE, "roleName");
            SysXMembershipUtil.CheckParameter(ref username, true, false, true, SysXSecurityConst.USER_NAME_MAX_SIZE, "username");
            
            if (username.Length < AppConsts.ONE)
            {
                return false;
            }

            try
            {
                return SecurityManager.IsUserInRole(username, roleName);
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
        /// Remove user from role.
        /// </summary>
        /// <param name="usernames">Value for user's name.</param>
        /// <param name="roleNames">Value for role's name.</param>
        public override void RemoveUsersFromRoles(String[] usernames, String[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check is role exists or not.
        /// </summary>
        /// <param name="roleName">Value for role's name.</param>
        /// <returns></returns>
        public override Boolean RoleExists(String roleName)
        {
            SysXMembershipUtil.CheckParameter(ref roleName, true, true, true, 256, "roleName");

            try
            {
                return SecurityManager.RoleExists(roleName);
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
        /// Assigned user to multiple roles
        /// </summary>
        /// <param name="usernames">Value for user's name.</param>
        /// <param name="roleNames">Value for role's name.</param>
        public override void AddUsersToRoles(String[] usernames, String[] roleNames)
        {
            //USE BAL
            throw new NotImplementedException();
        }

        /// <summary>
        /// handles the operation to create roles.
        /// </summary>
        /// <param name="roleName">Value for role's name.</param>
        public override void CreateRole(String roleName)
        {
            //USE BAL
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete role.
        /// </summary>
        /// <param name="roleName">Value for role's name.</param>
        /// <param name="throwOnPopulatedRole">Value for throwOnPopulatedRole's name.</param>
        /// <returns></returns>
        public override Boolean DeleteRole(String roleName, Boolean throwOnPopulatedRole)
        {
            //USE BAL
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}