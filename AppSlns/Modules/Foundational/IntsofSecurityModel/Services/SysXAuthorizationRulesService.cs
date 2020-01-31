#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXAuthorizationRulesService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb.Interfaces;

#endregion

#region Application Specific

using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Services
{
    /// <summary>
    /// Handles the services related to authorization services.
    /// </summary>
    public class SysXAuthorizationRulesService : IAuthorizationRulesService
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
        /// Get Authorization Rules
        /// </summary>
        /// <param name="urlPath">value for urlPath.</param>
        /// <returns></returns>
        public String[] GetAuthorizationRules(String urlPath)
        {
            try
            {
                SiteMapNode node = SiteMap.Provider.FindSiteMapNode(urlPath);
                if (!node.IsNull())
                {
                    IEnumerable<String> enu = (IEnumerable<String>)node.Roles;
                    return enu.ToArray<String>();
                }
                else
                {
                    return new String[] { };
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

        /// <summary>
        /// Register Authorization Rule
        /// </summary>
        /// <param name="urlPath">value for urlPath.</param>
        /// <param name="rule">rule's value.</param>
        public void RegisterAuthorizationRule(String urlPath, String rule)
        {
            try
            {
                SiteMapNode node = SiteMap.Provider.FindSiteMapNode(urlPath);
                if (!node.IsNull())
                {
                    node.Roles.Add(rule);
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