using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CoreWeb.ApplicantModule.Views;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using System.Web.Services;
using CoreWeb.Shell;
using Business.RepoManagers;

namespace CoreWeb.ApplicantModule.Views
{
    public partial class ApplicantModuleDefault : BasePage, IDefaultView
    {
        private DefaultViewPresenter _presenter = new DefaultViewPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }


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
        /// <summary>
        /// Raises the initialize complete event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInitComplete(EventArgs e)
        {
            base.dynamicPlaceHolder = plcDynamic;
            if (!Request.QueryString[AppConsts.CHILD].IsNull())
            {
                base.ControlName = Request.QueryString[AppConsts.CHILD];
            }
            base.OnInitComplete(e);
            //base.SetModuleTitle("Applicant");
            base.SetModuleTitle(Resources.Language.APPLICANT);
        }

        /// <summary>
        /// UAT-2930
        /// </summary>
        [WebMethod]
        public static string TwofactorAuthenticationLabelupdate()
        {
            String _userId = SysXWebSiteUtils.SessionService.UserId;
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
    }
}
