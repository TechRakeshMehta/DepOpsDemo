#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSessionService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.Security;
using System.IO.Compression;

#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Utils.Consts;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using Entity;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Services
{
    /// <summary>
    /// Handles the operations related to session services.
    /// </summary>
    public class AllClientSessionService : IAllClientSessionService
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
        /// Set the custom data.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="data">Data value.</param>
        public void SetCustomData(String key, Object data)
        {
            HttpContext.Current.Session.Add(key, data);
        }

        /// <summary>
        /// Retrieves the custom data.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns></returns>
        public Object GetCustomData(String key)
        {
            return HttpContext.Current.Session[key];
        }

        /// <summary>
        /// Clears the session value.
        /// </summary>
        /// <param name="doAbandon">value for doAbandon.</param>
        public void ClearSession(Boolean doAbandon)
        {
            if (doAbandon)
            {
                HttpContext.Current.Session.Abandon();
            }
            HttpContext.Current.Session.Clear();
        }

        public Boolean IsAlumniRedirectionDue
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion

    }
}