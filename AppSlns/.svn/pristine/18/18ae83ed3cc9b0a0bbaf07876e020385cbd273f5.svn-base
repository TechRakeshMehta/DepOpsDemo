#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Collections.Generic;
//using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;
using System.Text;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.IntsofSecurityModel.Providers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTERSOFT.WEB.UI.WebControls;
using INTERSOFT.WEB.UI.Config;


#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
	public partial class DynamicPageMaster : System.Web.UI.MasterPage, IDynamicPageMasterView
    {
        #region Private Variables
        private DynamicPageMasterPresenter _presenter=new DynamicPageMasterPresenter();
        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
		}
        #endregion

        #region ResorceManager Events
        /// <summary>
        /// Handles the Init event of the rsrMgr control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void rsrMgr_Init(object sender, EventArgs e)
        {
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
        #endregion

        #endregion

        #region Presenter
        
		public DynamicPageMasterPresenter Presenter
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

        #region Properties
        /// <summary>
        /// Property to set the Header of application
        /// </summary>
        public String HeaderHtml
        {
            set { litHeader.Text = value; }
            
        }
        #endregion
    }
}
