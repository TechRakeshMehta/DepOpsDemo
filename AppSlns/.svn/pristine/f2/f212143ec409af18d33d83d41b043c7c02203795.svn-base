#region Header Comment Master

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  BaseMasterPage.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Web.Security;
using System.Linq;


#endregion

#region Application Specific

using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTSOF.Utils;
using Telerik.Web.UI;
//PREM starts
using Entity;
using INTSOF.Contracts;
using System.Collections.Generic;
using Entity.Navigation;

//PREM ends
#endregion

#endregion


namespace CoreWeb
{
    /// <summary>
    /// Summary description for BaseMasterPage
    /// </summary>
    public class BaseMasterPage : System.Web.UI.MasterPage
    {
        #region Constructor

        public BaseMasterPage()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region Variables

        private ISysXSessionService _sessionService = SysXWebSiteUtils.SessionService;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        public String CurrentUserId
        {
            get
            {
                return _sessionService.UserId;
            }
        }

        /// <summary>
        /// Gets the current organization user id.
        /// </summary>
        /// <remarks></remarks>
        public Int32 CurrentOrgUserId
        {
            get
            {
                return _sessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Gets a Locked Transaction ID if any.
        /// </summary>
        public Int32? LockedTransactionId
        {
            get
            {
                return (Int32?)_sessionService.GetCustomData(AppConsts.TransactionID);
            }
            set
            {
                _sessionService.SetCustomData(AppConsts.TransactionID, value);
            }
        }

        #endregion

        #endregion

        #region Events
        //PREM starts
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if ((!Context.User.Identity.IsAuthenticated || Context.User.Identity.Name.IsNullOrEmpty())
               && (Session["UserVerified"].IsNotNull() && !Session["UserVerified"].Equals("PostResetPassword"))) // UAT 700 : Overhaul of Change Password  Implementation
            {
                ISysXSessionService sessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService;
                sessionService.ClearSession(true);
                FormsAuthentication.SignOut();
                SysXAppDBEntities.ClearContext();

                //Response.Redirect(FormsAuthentication.LoginUrl);
                Server.Execute(FormsAuthentication.LoginUrl);
            }
        }
        //PREM ends
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Redirects to login page.
        /// </summary>
        /// <remarks></remarks>
        public void RedirectToLoginPage()
        {
            Response.Redirect(FormsAuthentication.LoginUrl);
        }

        #endregion

        #region Private Methods

        private void BuildChildPanelMenus(SiteMapNode siteMapNode, RadPanelItem parentMenuItem)
        {
            foreach (SiteMapNode node in siteMapNode.ChildNodes)
            {
                RadPanelItem menuItem = new RadPanelItem(node.Title, node.Url);
                menuItem.ToolTip = node.Description;
                parentMenuItem.Items.Add(menuItem);

                if (node.ChildNodes.Count > AppConsts.NONE)
                {
                    BuildChildPanelMenus(node, menuItem);
                }
            }
        }

        private void BuildChildMenus(SiteMapNode siteMapNode, RadMenuItem parentMenuItem)
        {
            foreach (SiteMapNode node in siteMapNode.ChildNodes)
            {
                RadMenuItem menuItem = new RadMenuItem(node.Title, node.Url);
                menuItem.ToolTip = node.Description;
                parentMenuItem.Items.Add(menuItem);

                if (node.ChildNodes.Count > AppConsts.NONE)
                {
                    BuildChildMenus(node, menuItem);
                }
            }
        }

        #endregion

        #endregion

        #region New Menu Implementation

        List<Entity.Navigation.MenuViewItem> _menuFeatures;

        public List<Entity.Navigation.MenuViewItem> MenuFeatures
        {
            get
            {
                if (HttpContext.Current.Items[AppConsts.SESSIONMENU] == null)
                {
                    Menu siteMenu;
                    String logInfo = String.Empty;
                    logInfo += "**Entering into Getting Menu features. User Id Is : " + _sessionService.UserId + " and SysXblockId is:" + _sessionService.SysXBlockId + ".**";
                    if (!_sessionService.UserId.IsNullOrEmpty())
                    {
                        if (!_sessionService.IsSysXAdmin)
                            siteMenu = new Menu(_sessionService.UserId, _sessionService.SysXBlockId, _sessionService.BusinessChannelType);
                        else
                            siteMenu = new Menu(_sessionService.BusinessChannelType);
                        HttpContext.Current.Items[AppConsts.SESSIONMENU] = siteMenu.MenuItems;
                    }
                    else
                        logInfo += "**Menu was not binded because UserId was empty or null**";

                    SysXWebSiteUtils.LoggerService.GetLogger().Info(logInfo);
                }

                return (HttpContext.Current.Items[AppConsts.SESSIONMENU] as List<MenuViewItem>);
            }
            set
            {
                HttpContext.Current.Items[AppConsts.SESSIONMENU] = value;
            }
        }

        private Entity.Navigation.MenuViewItem _mr;
        public Entity.Navigation.MenuViewItem GetMenuRow()
        {
            //if we've already looked up for this page just return the same result
            if (_mr != null)
                return _mr;

            var menu = MenuFeatures;

            if (menu == null)
                return null;

            string currentUrl = getFormatedUrl();

            Entity.Navigation.MenuViewItem dr = null;

            var m = menu.FirstOrDefault(x => x.URL == currentUrl);

            if (m != null)
            {
                return m;
            }

            _mr = dr;
            return dr;
        }

        private string getFormatedUrl()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("~");
            sb.Append(this.Request.Url.AbsolutePath);

            if (this.Request.QueryString.Count > 0)
            {
                bool first = true;
                foreach (string key in this.Request.QueryString.Keys)
                {
                    if (first)
                    {
                        first = false;
                        sb.Append("?");
                    }
                    else
                    {
                        sb.Append("&");
                    }
                    sb.Append(key);
                    sb.Append("=");
                    sb.Append(this.Request.QueryString.Get(key).ToString());
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}