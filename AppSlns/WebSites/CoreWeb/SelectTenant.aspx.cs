using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;

namespace CoreWeb
{
    public partial class SelectTenant : System.Web.UI.Page, ISelectTenantView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SelectTenantPresenter _presenter = new SelectTenantPresenter();

        #endregion

        #endregion

        public SelectTenantPresenter Presenter
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
        /// Property to represent the Institute Selected by the applicant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
        }

        /// <summary>
        /// Property to represent the Id of the current logged in user
        /// </summary>
        public Int32 OrganizationUserId
        {
            get
            {
                return Convert.ToInt32(ViewState["OrganizationUserId"]);
            }
            set
            {
                ViewState["OrganizationUserId"] = value;
            }
        }

        public IPersistViewState ViewStateProvider
        {
            get
            {
                if (ConfigurationManager.AppSettings[SysXCachingConst.CUSTOMVIEWSTATEPROVIDER].IsNotNull())
                {
                    switch (ConfigurationManager.AppSettings[SysXCachingConst.CUSTOMVIEWSTATEPROVIDER].ToString().ToUpper())
                    {
                        case "REDIS":
                            {
                                return new RedisPersistViewStateProvider();
                            }
                        case "SQL":
                            {
                                return new SysXPersistViewStateProvider();
                            }
                    }
                }
                return new SysXPersistViewStateProvider();
            }
        }

        /// <summary>
        /// Property to represent the Id of the current logged in user
        /// </summary>
        private Boolean IsIncorrectUrl
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsIncorrectUrl"]);
            }
            set
            {
                ViewState["IsIncorrectUrl"] = value;
            }
        }


        /// <summary>
        /// Gets the current user Session id.
        /// </summary>
        /// <remarks></remarks>
        public String CurrentSessionId
        {
            get
            {
                return Page.Session.SessionID.IsNullOrEmpty() ? String.Empty : Page.Session.SessionID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            if (user.IsNull())
                Response.Redirect(FormsAuthentication.LoginUrl);

            if (!Page.IsPostBack)
            {
                ddlTenantName.DataSource = Presenter.GetTenants(user);
                ddlTenantName.DataBind();
            }
        }
        protected void btnProceed_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                LogOutUser();

            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            this.OrganizationUserId = user.OrganizationUserId;
             
            if (this.SelectedTenantId > AppConsts.NONE)
            {
                Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
                ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
                appInstData.UserID = Convert.ToString(user.UserId);
                appInstData.TagetInstURL = Presenter.GetInstitutionUrl();
                appInstData.TokenCreatedTime = DateTime.Now;
                appInstData.TenantID = this.SelectedTenantId;
                appInstData.IsIncorrectLogin = user.IncorrectLoginUrlUsed;
                String key = Guid.NewGuid().ToString();

                Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
                if (applicationData != null)
                {
                    applicantData = applicationData;
                    applicantData.Add(key, appInstData);
                    Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
                }
                else
                {
                    applicantData.Add(key, appInstData);
                    Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
                }

                //Log out from application then redirect to selected tenant url, append key in querystring.
                // On login page get data from Application Variable.

                Presenter.DoLogOff(!user.IsNull(), user.UserLoginHistoryID);
                //Redirect to login page
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TokenKey", key  }
                                                                 };
                Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
                //Response.Redirect(String.Format("~/Login.aspx?TokenKey={0}", key));
            }
            else
            {
                LogOutUser();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LogOutUser();
        }

        /// <summary>
        /// Logout User if 'Cancel' clicked or there is no option to select from the Tenants dropdown
        /// </summary>
        private void LogOutUser()
        {
            SysXWebSiteUtils.SessionService.ClearSession(true);
            Response.Redirect(FormsAuthentication.LoginUrl);
        }
    }
}