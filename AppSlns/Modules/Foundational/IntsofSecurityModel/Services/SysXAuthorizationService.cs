#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXAuthorizationService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Threading;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Security;

#endregion

#region Application Specific
using INTSOF.Utils;
#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Services
{
    /// <summary>
    /// This class handles authorization services.
    /// </summary>
    public class SysXAuthorizationService : IAuthorizationService
    {
        #region Constructor

        /// <summary>
        /// Call constructor to initialized _sysXAuthorizationProvider variable
        /// </summary>
        public SysXAuthorizationService()
        {
            try
            {
                _sysXAuthorizationProvider = AuthorizationFactory.GetAuthorizationProvider();
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

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        IAuthorizationProvider _sysXAuthorizationProvider;

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
        /// Check authorization by role and context.
        /// </summary>
        /// <param name="role">role value.</param>
        /// <param name="context">Context value.</param>
        /// <returns></returns>
        public Boolean IsAuthorized(String role, String context)
        {
            try
            {
                return _sysXAuthorizationProvider.Authorize(HttpContext.Current.User, context);
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
        /// Check authorization by context.
        /// </summary>
        /// <param name="context">Context value.</param>
        /// <returns></returns>
        public Boolean IsAuthorized(String context)
        {
            try
            {
                return _sysXAuthorizationProvider.Authorize(Thread.CurrentPrincipal, context);
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

        #region Private Methods

        #endregion

        #endregion
    }
}