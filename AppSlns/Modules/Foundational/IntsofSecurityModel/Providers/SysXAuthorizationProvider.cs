#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXAuthorizationProvider.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security;

#endregion

#region Application Specific

using INTSOF.Utils.Consts;
using Business.RepoManagers;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Providers
{
    /// <summary>
    /// This class handles the operations related to Authorization Provider.
    /// </summary>
    /// <remarks></remarks>
    [ConfigurationElementTypeAttribute(typeof(CustomAuthorizationProviderData))]
    public class SysXAuthorizationProvider : IAuthorizationProvider
    {
        #region Variables

        #region Private Variables

        #endregion

        #region Private Variables

        private String _sysXAdminRoleName;

        SysXRoleProvider _sysXRoleProvider = new SysXRoleProvider();

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

        #region Authorization

        /// <summary>
        /// Initializes a new instance of the <see cref="SysXAuthorizationProvider"/> class.
        /// </summary>
        /// <remarks></remarks>
        public SysXAuthorizationProvider(NameValueCollection config)
        {
            try
            {
                _sysXAdminRoleName = SecurityManager.GetSysXConfigValue(SysXSecurityConst.SYSX_ADMIN_ROLE_KEY_NAME);
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check authorization for principal.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Authorize(IPrincipal principal, String context)
        {
            try
            {
                if (principal.IsNull())
                {
                    throw new ArgumentNullException(SysXUtils.GetMessage(ResourceConst.SECURITY_PRINCIPAL));
                }
                if (context.IsNull() || String.IsNullOrEmpty(context))
                {
                    throw new ArgumentNullException(SysXUtils.GetMessage(ResourceConst.SECURITY_CONTEXT));
                }

                String[] userRoles = _sysXRoleProvider.GetRolesForUser(principal.Identity.Name);

                if (userRoles.IsNull() || userRoles.Length.Equals(AppConsts.NONE))
                {
                    return false;
                }

                return userRoles.Contains(_sysXAdminRoleName) || userRoles.Contains(context);
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}