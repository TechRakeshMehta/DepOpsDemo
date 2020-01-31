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
using CoreWeb.Main.Views;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Entity;
using System.Web.Services;
using Business.RepoManagers;
using CoreWeb.Shell;
using System.Collections.Generic;
using CoreWeb.IntsofSecurityModel;
using ModuleUtility;

namespace CoreWeb.AMS_Main
{
    public partial class MainDefault : BasePage, IDefaultView
    {
        private DefaultViewPresenter _presenter = new DefaultViewPresenter();

        public Int32 TenantId
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

            }
            Presenter.OnViewLoaded();
            base.SetModuleTitle("Dashboard");
            //base.HideTitleBars();i


        }
        protected override void OnInitComplete(EventArgs e)
        {
            UserControl userControl = null;

            //Get User from Session
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

            Int32 currentLoggedInUserId = 0; // = user.OrganizationUserId; //SysXWebSiteUtils.SessionService.OrganizationUserId;

            Dictionary<String, String> encryptedQueryString = null;
            //OrganizationUser objUser = Presenter.LoginUser(currentLoggedInUserId);

            if (user.IsNotNull())
            {
                currentLoggedInUserId = user.OrganizationUserId;
                TenantId = user.TenantId.HasValue ? user.TenantId.Value : 0; //objUser.Organization.TenantID.Value;
            }
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                encryptedQueryString = new Dictionary<String, String>();
                encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }

            if (!encryptedQueryString.IsNull() && encryptedQueryString.ContainsKey(AppConsts.CHILD))
            {
                userControl = (UserControl)LoadControl(encryptedQueryString[AppConsts.CHILD]);
            }
            else if (!Request.QueryString[AppConsts.THEMESETTING].IsNull())
            {
                //@Todo- Actual way is to use basepage methods, but couldn't use because of the dashboard page load restriction set on basepage
                userControl = (UserControl)LoadControl(AppConsts.THEMESETTING_CONTROL_NAME);
            }
            //else if (objUser != null && (objUser.IsApplicant ?? false))
            else if (user.IsApplicant)
            {
                userControl = null;

                Int16 businessChannelTypeID = AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE;
                if (SysXWebSiteUtils.SessionService.BusinessChannelType.IsNotNull())
                {
                    businessChannelTypeID = SysXWebSiteUtils.SessionService.BusinessChannelType.BusinessChannelTypeID;
                }

                if (businessChannelTypeID.Equals(AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE))
                {
                    //if (objUser.aspnet_Users.aspnet_Membership.LastLoginDate > objUser.CreatedOn)
                    if (Presenter.CheckIfApplicantHasPlacedOrder(currentLoggedInUserId))
                    {
                        //To check if current Applicant have any order with Payment due.
                        if (Presenter.CheckIfApplicantHasPaymentDue(currentLoggedInUserId))
                        {
                            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.ApplicantBalancePayment}
                                                                    
                                                                 };
                            Response.Redirect(String.Format("~/Mobility/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));


                        }
                        else
                        {
                            //Dictionary<String, String> queryString = new Dictionary<String, String>
                            //                                             { 
                            //                                                { "Child", ChildControls.ExternalDashboard}

                            //                                             };
                            //String url = String.Format(@"~\Dashboard\Default.aspx?ucid={0}&args={1}", Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID], queryString.ToEncryptedQueryString());
                            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
                        }
                    }
                }
            }
            else if (user != null)
            {
                userControl = null;
                //Dictionary<String, String> queryString = new Dictionary<String, String>
                //                                                 { 
                //                                                    { "Child", ChildControls.InternalDashboard}

                //                                                 };
                //String url = String.Format(@"~\Dashboard\Default.aspx?ucid={0}&args={1}", Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID], queryString.ToEncryptedQueryString());
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
            }
            if (!userControl.IsNull())
            {
                userControl.ID = AppConsts.UC_DYNAMIC_CONTROL;
                phDynamic.Controls.Add(userControl);
            }

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
        /// Invoked to delete the message and update the respective folder.
        /// </summary>
        /// <param name="folderID">folderID</param>
        /// <param name="messageId">messageId</param>
        /// <param name="queueOwnerId">queueOwnerId</param>
        /// <param name="queueTypeID">queueTypeID</param>
        /// <returns></returns>
        [WebMethod]
        public static void DeleteMessage(String messageId)
        {
            Guid currentMessageId = new Guid(messageId);
            Boolean isDeleted = MessageManager.DeleteMessageFromDashboard(currentMessageId, SysXWebSiteUtils.SessionService.OrganizationUserId);
        }
    }
}