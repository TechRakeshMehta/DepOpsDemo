using CoreWeb.CommonOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.CommonOperations.Pages
{
    public partial class AlumniPopup : BaseWebPage, IAlumniPopupView
    {
        #region Private Variables

        private AlumniPopupPresenter _presenter = new AlumniPopupPresenter();

        #endregion

        #region PUBLIC PROPRTIES
        public AlumniPopupPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
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
        public IAlumniPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IAlumniPopupView.LoggedInUserID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return CurrentViewContext.OrgUserId;
            }
        }
        Int32 IAlumniPopupView.TenantId
        {
            get
            {
                if (ViewState["TenantId"].IsNotNull())
                    return Convert.ToInt32(ViewState["TenantId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }
        Int32 IAlumniPopupView.OrgUserId
        {
            get
            {
                if (ViewState["OrgUserId"].IsNotNull())
                    return Convert.ToInt32(ViewState["OrgUserId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OrgUserId"] = value;
            }
        }

        Int32 IAlumniPopupView.AlumniTenantId
        {
            get
            {
                if (ViewState["AlumniTenantId"].IsNotNull())
                    return Convert.ToInt32(ViewState["AlumniTenantId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AlumniTenantId"] = value;
            }
        }

        /// <summary>
        /// Gets the current user Session id.
        /// </summary>
        /// <remarks></remarks>
        String IAlumniPopupView.CurrentSessionId
        {
            get
            {
                return Page.Session.SessionID.IsNullOrEmpty() ? String.Empty : Page.Session.SessionID;
            }
        }


        #endregion

        #region EVENTS

        #region PAGE EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Presenter.GetAlumniTenantId();
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                CurrentViewContext.OrgUserId = Convert.ToInt32(Request.QueryString["orgUserId"]);
            }
        }
        #endregion

        #endregion

        #region Button click

        protected void fsucCommandBar_ActivateClick(object sender, EventArgs e)
        {
            if (Session["ClientMachineIP"].IsNullOrEmpty())
            {
                Session["ClientMachineIP"] = Request.UserHostAddress;
            }
            String machineIP = Convert.ToString(Session["ClientMachineIP"]);

            if (Presenter.CreateAlumniDefaultSubscription(machineIP))
            {
                //redirect to alumni tenant.
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                if (!user.IsNullOrEmpty() && user.IsApplicant && CurrentViewContext.AlumniTenantId > AppConsts.NONE)
                {
                    SysXWebSiteUtils.AllClientSessionService.IsAlumniRedirectionDue = true;

                    Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
                    ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
                    appInstData.UserID = Convert.ToString(user.UserId);
                    appInstData.TagetInstURL = Presenter.GetApplicationUrl();
                    //"http://localhost:52887";
                    appInstData.TokenCreatedTime = DateTime.Now;
                    appInstData.TenantID = CurrentViewContext.AlumniTenantId;

                    //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                    if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    {
                        appInstData.AdminOrgUserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                    }
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
                    Presenter.DoLogOff(!user.IsNull(), user.UserLoginHistoryID);
                    //Redirect to login page
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TokenKey", key  }
                                                                 };
                    Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
                    //Response.Redirect(String.Format("~/Login.aspx?TokenKey={0}", key));
                }
            }

        }

        protected void fsucCommandBar_DismissClick(object sender, EventArgs e)
        {
            //close popup
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "closePopUp();", true);
        }

        protected void fsucCommandBar_DismissForeverClick(object sender, EventArgs e)
        {
            String alumniStatusCode = lkpAlumniStatus.Dismissed.GetStringValue();
            if (Presenter.UpdateAlumniStatusinOrganizationUserAlumnAccess(alumniStatusCode))
            {
                //popup close
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "closePopUp();", true);
            }

        }

        #endregion
    }
}