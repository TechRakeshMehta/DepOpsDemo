using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using CoreWeb.Shell;
using System.Web.Security;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ReportEmploymentDisclosure : BaseWebPage, IReportEmploymentDisclosureView
    {
        #region VARIABLES
        private ReportEmploymentDisclosurePresenter _presenter = new ReportEmploymentDisclosurePresenter();
        #endregion

        #region PROPERTIES
        public ReportEmploymentDisclosurePresenter Presenter
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

        public Int32 TenantID
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantID"]);
            }
            set
            {
                ViewState["TenantID"] = value;
            }
        }

        public Int32 OrganizationUserID
        {
            get
            {
                return Convert.ToInt32(ViewState["OrganizationUserID"]);
            }
            set
            {
                ViewState["OrganizationUserID"] = value;
            }
        }

        public String DocumentTypeCode
        {
            get
            {
                return Convert.ToString(ViewState["DocumentTypeCode"]);
            }
            set
            {
                ViewState["DocumentTypeCode"] = value;
            }
        }

        public List<Int32> lstAnnouncementID
        {
            get
            {
                if (ViewState["lstAnnouncementID"].IsNotNull())
                {
                    return (List<Int32>)(ViewState["lstAnnouncementID"]);
                }
                return new List<Int32>();
            }
            set
            {
                ViewState["lstAnnouncementID"] = value;
            }
        }


        #endregion

        #region EVENTS

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // get members user
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

            if (user.IsNotNull())
            {
                //Dictionary<String, String> args = new Dictionary<String, String>();
                //if (!Request.QueryString["args"].IsNull())
                //{
                //args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (!Request.QueryString["TenantID"].IsNullOrEmpty())
                {
                    TenantID = Convert.ToInt32(Request.QueryString["TenantID"]);
                    hdnTenantId.Value = Convert.ToString(Request.QueryString["TenantID"]);
                }
                if (!Request.QueryString["OrganizationUserID"].IsNullOrEmpty())
                {
                    OrganizationUserID = Convert.ToInt32(Request.QueryString["OrganizationUserID"]);
                }
                if (!Request.QueryString["DocumentTypeCode"].IsNullOrEmpty())
                {
                    DocumentTypeCode = Convert.ToString(Request.QueryString["DocumentTypeCode"]);
                }
                if (!Request.QueryString["DocumentId"].IsNullOrEmpty())
                {
                    DocumentId = Convert.ToString(Request.QueryString["DocumentId"]);
                    hdnDocumentId.Value = Convert.ToString(Request.QueryString["DocumentId"]);
                }
                if (!Request.QueryString["OrderId"].IsNullOrEmpty())
                {
                    hdnOrderId.Value = Convert.ToString(Request.QueryString["OrderId"]);
                }
                if (!Request.QueryString["PopupType"].IsNullOrEmpty())
                {
                    hdnPopupType.Value = Convert.ToString(Request.QueryString["PopupType"]);
                }
                if (!Request.QueryString["ServiceGroupID"].IsNullOrEmpty())
                {
                    hdnServiceGroupID.Value = Convert.ToString(Request.QueryString["ServiceGroupID"]);
                }
                if (!Request.QueryString["BkgPkgSvcGrpID"].IsNullOrEmpty())
                {
                    hdnBkgPkgSvcGrpID.Value = Convert.ToString(Request.QueryString["BkgPkgSvcGrpID"]);
                }
                //DocumentTypeCode = AppConsts.EMPLOYMENT_DISCLOSURE_DOCUMENT;

                //Presenter.FillEmploymentDisclosureDocumentWithPrePopulatedData();
                iframeEmpDisclosure.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&OrganizationUserID={1}&TenantID={2}", DocumentTypeCode, OrganizationUserID, TenantID);
                // }
            }
            else
            {
                //RedirectToLogin();
            }
        }

        /// <summary>
        /// Submit Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //Make Entry in DB and Redirect to Select Business Channel
            if (Presenter.SaveEDDetails())
            {
                //Response.Redirect(AppConsts.SELECT_BUSINESS_CHANNEL_URL);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ClosePopup(" + "'success'" + ");", true);
            }
            else
            {
                base.ShowErrorMessage("Some error occured. Please try again.");
            }
        }

        #endregion

        /// <summary>
        /// Cancel Button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //RedirectToLogin();
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ClosePopup(" + "'cancel'" + ");", true);
        }

        /// <summary>
        /// Method to Redirect the User to login page
        /// </summary>
        private void RedirectToLogin()
        {
            SysXWebSiteUtils.SessionService.ClearSession(true);
            Response.Redirect(FormsAuthentication.LoginUrl);
        }


        public string DocumentId { get; set; }
    }
}