#region Namespaces

#region System Defined

using System;
using System.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using System.Xml.Linq;
using Telerik.Web.UI;
using System.Linq;
using System.Web.Security;
using System.Web;
#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Utils.Consts;
using Entity;
using CoreWeb.IntsofSecurityModel;

using Business;
using Business.RepoManagers;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using CoreWeb.CommonControls.Views;
using INTERSOFT.WEB.UI.Config;


#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
	public partial class PopupMaster : System.Web.UI.MasterPage, IPopupMasterView
	{
		private PopupMasterPresenter _presenter=new PopupMasterPresenter();
        private aspnet_Membership _aspnetMembership;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();

		}

		
		public PopupMasterPresenter Presenter
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

        protected void CommandContent_PreRender(object sender, EventArgs e)
        {
            if (CommandContent.HasControls())
            {
                rdpnCommands.Collapsed = false;
            }
        }

        protected void rsrMgr_Init(object sender, EventArgs e)
        {
            //string themeName = (string)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME);
            //if (!(themeName.ToLower().Equals("red") || themeName.ToLower().Equals("green")))
            //    themeName = "Green";

            //WclTheme currentTheme = (System.Configuration.ConfigurationManager.GetSection("adbThemes") as WclThemeSection).Themes[themeName];

            //rsrMgr.ThemeName = themeName;
            //rsrMgr.SkinCollection = !currentTheme.IsNull() ? currentTheme.Skins : null;


            try
            {
                String _userPreferenceTheme = String.Empty;

                _userPreferenceTheme = (String)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME);

                string themeSection = System.Configuration.ConfigurationManager.AppSettings["ThemeSection"];
                string defaultTheme = System.Configuration.ConfigurationManager.AppSettings["DefaultTheme"];

                WclTheme currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                if (currentTheme == null)
                {
                    _userPreferenceTheme = defaultTheme;
                    currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                }
                rsrMgr.ThemeName = _userPreferenceTheme;
                rsrMgr.SkinCollection = !currentTheme.IsNull() ? currentTheme.Skins : null;
            }
            catch (System.Exception ex)
            {
                SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, Resource Manager Initialisation", ex);
                SysXWebSiteUtils.ExceptionService.HandleError("Unable to build menus for user : " + SysXWebSiteUtils.SessionService.SysXMembershipUser.UserName, ex);
            }
        }
	}
}
