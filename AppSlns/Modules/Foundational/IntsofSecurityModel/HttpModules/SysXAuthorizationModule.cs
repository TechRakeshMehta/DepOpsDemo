#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXAuthorizationModule.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Web;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb;

#endregion

#region Application Specific

using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.HttpModules
{
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    class SysXAuthorizationModule : WebClientAuthorizationModule
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
        /// Handle the authorization section.
        /// </summary>
        /// <param name="rootContainer"></param>
        /// <param name="context"></param>
        protected override void HandleAuthorization(CompositionContainer rootContainer, IHttpContext context)
        {
            try
            {
                if (!context.SkipAuthorization)
                {
                    IAuthorizationRulesService authorizationRulesService = rootContainer.Services.Get<IAuthorizationRulesService>();
                    IVirtualPathUtilityService virtualPathUtilityService = rootContainer.Services.Get<IVirtualPathUtilityService>();
                    if (!authorizationRulesService.IsNull())
                    {
                        String[] authorizationRules = authorizationRulesService.GetAuthorizationRules(virtualPathUtilityService.ToAppRelative(context.Request.Url.PathAndQuery));
                        if ((!authorizationRules.IsNull()) && (authorizationRules.Length != 0))
                        {
                            IAuthorizationService authorizationService = rootContainer.Services.Get<IAuthorizationService>(true);
                            Boolean isAuthorized = authorizationRules.Any(str => authorizationService.IsAuthorized(str));
                            if (isAuthorized.Equals(false))
                            {
                                throw new HttpException((Int32)SysXHttpCodes.UserDoesntHaveAccessToTheRequestedResource, SysXUtils.GetMessage(ResourceConst.SECURITY_USERACCESSORNOT));
                            }
                        }
                    }
                }
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
