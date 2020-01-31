#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  BasePage.cs
// Purpose:   Base Page for default page  
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Business.RepoManagers;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.SharedObjects;
using NLog;
using System.Configuration;
#endregion

#endregion

namespace CoreWeb
{
    /// <summary>
    /// Summary description for BasePage
    /// </summary>
    public class BasePage : Page
    {
        #region Variables

        protected PlaceHolder dynamicPlaceHolder;
        protected String ControlName;
        private SysXPageViewStatePersister _pageStatePersister;
        private List<WclGrid> gridList = new List<WclGrid>();
        private SysXSiteMapNode _currentSiteMapNode;

        /// <summary>
        /// Logger instance to log the Order flow steps
        /// </summary>
        private static Logger _orderFlowlogger;

        #endregion

        #region Properties

        #region Public Properties

        public String Titles
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Hides the title bar UI component from the rendered page content.
        /// </summary>
        /// <param name="IncludeCssClass">Includes 'no_error_panel' css class in the form element</param>
        public void HideTitleBars(bool IncludeCssClass = false)
        {
            if (this.Master.IsNull())
            {
                return;
            }

            Shell.MasterPages.IDefaultMasterView master = this.Master as Shell.MasterPages.IDefaultMasterView;
            master.HideTitleBars(IncludeCssClass);
        }

        /// <summary>
        /// Sets Module title
        /// </summary>
        /// <param name="title"></param>
        public void SetModuleTitle(String title)
        {
            if (this.Master.IsNull())
            {
                return;
            }

            Shell.MasterPages.IDefaultMasterView master = this.Master as Shell.MasterPages.IDefaultMasterView;
            master.SetModuleTitle(title);
        }

        /// <summary>
        /// Sets Page / Screen Title
        /// </summary>
        /// <param name="title"></param>
        public void SetPageTitle(String title)
        {
            if (this.Master.IsNull())
            {
                return;
            }

            Shell.MasterPages.IDefaultMasterView master = this.Master as Shell.MasterPages.IDefaultMasterView;
            master.SetPageTitle(title);
        }

        #endregion

        #region Private Methods

        private void SessionExpRedirectToLoginPage(String sessionExp)
        {
            SysXWebSiteUtils.SessionService.ClearSession(true);
            // Remove the forms-authentication ticket from the browser
            FormsAuthentication.SignOut();
            SysXAppDBEntities.ClearContext();
            Dictionary<String, String> encryptedQueryString = new Dictionary<String, String> { { AppConsts.SESSION_EXPIRED, "success" } };
            //String redirectToLoginPage = String.Format(FormsAuthentication.LoginUrl + "?" + AppConsts.SESSION_EXPIRED + "={0}", encryptedQueryString.ToEncryptedQueryString());
            //Response.Redirect(redirectToLoginPage);
            String queryString = String.Format(AppConsts.SESSION_EXPIRED + "={0}", encryptedQueryString.ToEncryptedQueryString());
            FormsAuthentication.RedirectToLoginPage(queryString);
        }

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Log the Order flow related information in different steps
        /// </summary>
        /// <param name="logMessage"></param>
        protected static void LogOrderFlowSteps(String logMessage)
        {
            if (_orderFlowlogger == null)
            {
                _orderFlowlogger = LogManager.GetLogger(NLogLoggerTypes.ORDER_FLOW_LOGGER.GetStringValue());
            }
            _orderFlowlogger.Info(logMessage);
        }
        #endregion

        #endregion

        #region Events

        #region Web Form Designer generated code

        ///<summary>
        ///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        ///</summary>
        ///<param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        override protected void OnInit(EventArgs e)
        {
            if (this.Master.GetType().Name != "shared_popupmaster_master")
            {
                var menu = ((BaseMasterPage)this.Master).GetMenuRow();
                if (menu != null)
                {
                    //TODO - switch out to enum
                    if (!menu.PermissionTypeId.HasValue) //if no permission is set then redirect to login page
                    {
                        //If login user is SysXAdmin then skip this condition
                        if (!SysXWebSiteUtils.SessionService.IsSysXAdmin)
                        {
                            SysXWebSiteUtils.SessionService.ClearSession(true);
                            System.Web.Security.FormsAuthentication.SignOut();
                            Response.Redirect(FormsAuthentication.LoginUrl, false);
                        }
                    }
                    else if (menu.PermissionTypeId == Entity.Navigation.PermissionTypeEnum.NoAccess)
                    {
                        Response.Redirect("~/AccessDenied.aspx", false);
                    }
                }

                base.OnInit(e);
            }
        }

        #endregion

        /// <summary>
        /// This methord returns the wcl grid type control list from the page
        /// </summary>
        /// <param name="c"></param>
        private void GetWclGridcontrol(Control c)
        {
            foreach (Control controlType in c.Controls)
            {

                var wclGrid = controlType as WclGrid;
                if (wclGrid.IsNotNull())
                {
                    gridList.Add(wclGrid);
                }
                if (controlType.Controls.Count > 0)
                {
                    GetWclGridcontrol(controlType);
                }
            }
        }

 
        /// <summary>
        /// Page OnInitComplete event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInitComplete(EventArgs e)
        {

            if (base.Request.Url.ToString().ToLower().Contains("dashboard"))
            {
                return;
            }

            Dictionary<String, String> encryptedQueryString = null;
            UserControl userControl;

            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                encryptedQueryString = new Dictionary<String, String>();
                encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }

            if (!encryptedQueryString.IsNull() && encryptedQueryString.ContainsKey(AppConsts.CHILD))
            {
                userControl = (UserControl)LoadControl(encryptedQueryString[AppConsts.CHILD]);

                if (!userControl.IsNull())
                {
                    userControl.ID = AppConsts.UC_DYNAMIC_CONTROL;
                    dynamicPlaceHolder.Controls.Add(userControl);
                }
                else
                {
                    SessionExpRedirectToLoginPage("success");
                }
            }
            else
            {
                if (this.Master.GetType().Name != "shared_popupmaster_master")
                {
                    var menu = ((BaseMasterPage)this.Master).GetMenuRow();
                    if (!menu.IsNull())
                    {
                        if (Convert.ToString(menu.UIControlID).ToLower().Contains("view="))
                        {
                            return;
                        }
                    }
                    if (!menu.IsNull())
                    {
                        userControl = (UserControl)LoadControl(menu.UIControlID);
                        userControl.ID = AppConsts.UC_DYNAMIC_CONTROL;
                        dynamicPlaceHolder.Controls.Add(userControl);
                    }
                    else if (!ControlName.IsNull())
                    {
                        userControl = (UserControl)LoadControl(@ControlName);
                        userControl.ID = AppConsts.UC_DYNAMIC_CONTROL;
                        dynamicPlaceHolder.Controls.Add(userControl);
                    }
                    else if (!Request.QueryString[AppConsts.UCID].IsNull())
                    {
                        encryptedQueryString = new Dictionary<string, string>();
                        encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.UCID]);
                        string ctrlPath = encryptedQueryString[AppConsts.UCID].Remove(encryptedQueryString[AppConsts.UCID].LastIndexOf(".ascx")) + ".ascx";
                        userControl = (UserControl)LoadControl(ctrlPath);
                        userControl.ID = AppConsts.UC_DYNAMIC_CONTROL;
                        dynamicPlaceHolder.Controls.Add(userControl);
                    }
                    else
                    {
                        SessionExpRedirectToLoginPage("success");
                    }
                }
                
            }

        }

        protected override PageStatePersister PageStatePersister
        {
            get
            {
                if (_pageStatePersister == null)
                {
                    _pageStatePersister = new SysXPageViewStatePersister(this);
                }
                return _pageStatePersister;
            }
        }

        #endregion

        #region Globalization for multi-Language
        
        protected override void InitializeCulture()
        {
            Boolean isLanguageTransaltionEnable = ConfigurationManager.AppSettings["IsLanguageTranslation"].IsNullOrEmpty() ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsLanguageTranslation"]);
            Boolean IsLocationTenant = false;
            if (!Session["IsLocationTenant"].IsNullOrEmpty())
                IsLocationTenant = Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("IsLocationTenant"));

            if (isLanguageTransaltionEnable && IsLocationTenant)
            {
                LanguageTranslateUtils.LanguageTranslateInit();
                base.InitializeCulture();
            }
        }

        protected virtual String GetLanguageCulture()
        {
            return LanguageTranslateUtils.GetCurrentLanguageCultureFromSession();
        }

        #endregion
    }
}