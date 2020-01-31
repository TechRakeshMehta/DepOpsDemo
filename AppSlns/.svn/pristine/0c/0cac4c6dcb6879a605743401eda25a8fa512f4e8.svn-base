#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  Default.aspx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

#endregion

#region Application Specific

using INTSOF.Utils;
using Business.RepoManagers;
using Entity;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations for default page in security module.
    /// </summary>
    public partial class SysXSecurityModelDefault : BasePage, IDefaultView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PageStatePersister _pageStatePersister;
        private DefaultViewPresenter _presenter=new DefaultViewPresenter();
        
        #endregion

        #region Protected Variables

        /// <summary>
        /// PageStatePersister</summary>
        /// <value>
        /// Gets or sets the value for PageStatePersister.</value>
        //protected override PageStatePersister PageStatePersister
        //{
        //    get
        //    {
        //        return _pageStatePersister.IsNull() ? new SessionPageStatePersister(this) : _pageStatePersister;
        //    }
        //}

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter</summary>
        /// <value>
        /// Represents Manage Tenant Presenter.</value>
        
        public DefaultViewPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Raises the initialize complete event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInitComplete(EventArgs e)
        {
            base.dynamicPlaceHolder = this.plcDynamic;
            base.OnInitComplete(e);
        }

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }

            Presenter.OnViewLoaded();
            //base.SetModuleTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_MODULE_TITLE));
            base.SetModuleTitle(Resources.Language.SECURITY);
        }

        /// <summary>
        /// To display the error.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        private void Page_Error(object sender, EventArgs e)
        {
            System.Exception exc = Server.GetLastError();
            Server.ClearError();
            Response.Write("<h2>" + exc.Message + " </h2>\n");
        }

        #endregion

        #region Grid Related Events

        #endregion

        #endregion

        #region Methods
        [System.Web.Services.WebMethod]
        public static Boolean IsMappedRole(String RoleId, Int32 UserGroupId)
        {
          Guid roleid=  Guid.Parse(RoleId);
          Boolean isMappedRole = false;
          List<UserGroupRolePermissionProductFeature> roles = SecurityManager.getUserGroupRolePermissionProductFeatureByRoleId(roleid, UserGroupId);
          if(roles.Count>AppConsts.NONE)
          {
          isMappedRole=true;
          }
            return isMappedRole;
        }

        [System.Web.Services.WebMethod]
        public static Boolean IsPrefixNameExist(String prefixName)
        {
            Boolean flag = false;
            if (SecurityManager.IsUserExist(prefixName))
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// UAT-2930
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string TwofactorAuthenticationLabelupdate()
        {
            String _userId = CoreWeb.Shell.SysXWebSiteUtils.SessionService.UserId;
            if (!_userId.IsNullOrEmpty())
            {
                Entity.UserTwoFactorAuthentication userTwoFactorAuthentication = SecurityManager.GetTwofactorAuthenticationForUserID(_userId);
                if (!userTwoFactorAuthentication.IsNullOrEmpty())
                {
                    if (userTwoFactorAuthentication.UTFA_IsVerified)
                    {
                        return "[Enabled]";
                    }
                    else
                    {
                        return "[Enabled - Not Verified]";
                    }
                }
                return "[Not Enabled]";
            }
            return string.Empty;
        }
        #endregion
    }
}