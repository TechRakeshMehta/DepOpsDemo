#region Header Comment SysxModule

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysxModule.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;

#endregion

#endregion


namespace CoreWeb
{
    /// <summary>
    /// Summary description for SysxModule
    /// </summary>
    public class SysxModule : IHttpModule
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static readonly Regex InputCleaner = new Regex("</", RegexOptions.Compiled);
        private static readonly Regex InputCleanerPercentage = new Regex("%", RegexOptions.Compiled);

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Handles dangerous request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void DangerousRequest(object sender, EventArgs e)
        {
            HttpRequest request = ((HttpApplication)sender).Request;

            if (request.HttpMethod.Equals(SysXUtils.GetMessage(ResourceConst.HTTP_METHOD_POST)))
            {
                if (!request.AppRelativeCurrentExecutionFilePath.Contains("/Messaging/default.aspx"))
                {
                    if (!request.Form.IsNull())
                    {
                        if (request.Form.Count > AppConsts.NONE)
                        {
                            if (ShowErrorMessage(request.Form))
                            {
                                if (CheckLogOffButtonClick(request.Form))
                                {
                                    FormsAuthentication.SignOut();
                                    SysXAppDBEntities.ClearContext();
                                    HttpContext.Current.Response.Redirect("~/login.aspx?logout=module");
                                }
                                else
                                {
                                    String errorMessage = SysXUtils.GetMessage(ResourceConst.RESTRICTED_CHARACTER_MESSAGE);
                                    HttpContext.Current.Response.Redirect("~/Errors/SysxGlobalError.htm?argsError=" + errorMessage);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks></remarks>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(DangerousRequest);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add This Functionality on TFS BUG # 2111
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private Boolean CheckLogOffButtonClick(NameValueCollection collection)
        {
            Int32 buttonLogOffClick = collection.GetValues("__EVENTTARGET").Where(cond => cond.Contains("Logoff")).Count();
            return buttonLogOffClick > AppConsts.NONE;
        }

        private Boolean ShowErrorMessage(NameValueCollection collection)
        {
            Boolean flag = false;

            for (Int32 count = AppConsts.NONE; count < collection.Count; count++)
            {
                if (String.IsNullOrWhiteSpace(collection[count]))
                {
                    continue;
                }

                if (InputCleaner.IsMatch(collection[count]) || InputCleanerPercentage.IsMatch(collection[count]))
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        #endregion

        #endregion
    }
}