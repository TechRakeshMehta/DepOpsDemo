using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Microsoft.Reporting.WebForms;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ReportViewer : Page, IReportViewerView
    {

        #region Variables
        private ReportViewerPresenter _presenter = new ReportViewerPresenter();
        #endregion

        #region Presenter
        public ReportViewerPresenter Presenter
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

        protected void Page_Init(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            //Decrypt the TenantId and SubscriptionIDs from Query String.
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }
            if (queryString.ContainsKey("TenantID"))
            {
                ViewState["TenantID"] = Convert.ToInt32(queryString["TenantID"]);
            }
            //UAT-4218 
            if (queryString.ContainsKey("ApplicantName") && !queryString.ContainsKey("ApplicantName").IsNullOrEmpty())
            {

                String ApplicantName = Convert.ToString(queryString["ApplicantName"]);
                if (ApplicantName != "")
                    ApplicantName = ApplicantName.Remove(0, 1);
                List<String> lstApplicantName = ApplicantName.Split(',').ToList();
                ApplicantName = String.Empty;
                foreach (var index in lstApplicantName)
                {
                    ApplicantName += " , " + "\"" + index + "\"";
                }
                ApplicantName = ApplicantName.Remove(1, 1);
                ctlReportViewer.ApplicantName = ApplicantName;
            }

            //UAT 977/1012
            if (!Request.QueryString["rptType"].IsNull())
            {
                if (!Request.QueryString["psid"].IsNull())
                {
                    SubscriptionID = Request.QueryString["psid"];
                }
                if (!Request.QueryString["tid"].IsNull())
                {
                    TenantID = Convert.ToInt32(Request.QueryString["tid"]);
                }

                ctlReportViewer.Parameters = SubscriptionIDForStatusReport;
                if (Request.QueryString["rptType"] == "Status Report")
                {
                    ctlReportViewer.ReportCode = "PRM01";
                }
                else if (Request.QueryString["rptType"] == "Data Report")
                {
                    ctlReportViewer.ReportCode = "PRDO";
                }
            }
            else
            {
                var isApprovedItemsReport = "0";
                if (queryString.ContainsKey("IsApprovedItemsReport"))
                {
                    isApprovedItemsReport = Convert.ToString(queryString["IsApprovedItemsReport"]);
                }

                if (isApprovedItemsReport == "1")
                {
                    ctlReportViewer.ReportCode = "PRMAI01";
                }
                else
                {
                    //earlier functionality
                    ctlReportViewer.ReportCode = "PRM01";
                }
                ctlReportViewer.Parameters = FormattedSubscriptionID;

            }
            ctlReportViewer.Focus();

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Int32 TenantID
        {
            get
            {
                if (ViewState["TenantID"] != null)
                    return Convert.ToInt32(ViewState["TenantID"]);
                else
                    return 0;
            }
            set
            {
                ViewState["TenantID"] = value;
            }
        }

        public string SubscriptionIDs
        {
            get
            {
                if (Session["SubscriptionIDs"] != null)
                {
                    ViewState["SubscriptionIDs"] = Convert.ToString(Session["SubscriptionIDs"]);
                    //Clear Session
                    Session.Remove("SubscriptionIDs");
                }
                if (ViewState["SubscriptionIDs"] != null)
                {
                    return Convert.ToString(ViewState["SubscriptionIDs"]);
                }
                else
                    return String.Empty;
            }
        }

        public string SubscriptionID
        {
            get
            {
                if (ViewState["SubscriptionID"] != null)
                    return Convert.ToString(ViewState["SubscriptionID"]);
                else
                    return String.Empty;
            }
            set
            {
                ViewState["SubscriptionID"] = value;
            }
        }

        public string FormattedSubscriptionID
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                ViewState["FormattedSubscriptionID"] = sb.Append("SchoolID")
                                                         .Append("|||")
                                                         .Append(Convert.ToString(TenantID))
                                                         .Append("$$$")
                                                         .Append("SubscriptionIds")
                                                         .Append("|||")
                                                         .Append(SubscriptionIDs).ToString();
                return Convert.ToString(ViewState["FormattedSubscriptionID"]);
            }
        }



        public string SubscriptionIDForStatusReport
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                ViewState["SubscriptionIDForStatusReport"] = sb.Append("SchoolID")
                                                         .Append("|||")
                                                         .Append(Convert.ToString(TenantID))
                                                         .Append("$$$")
                                                         .Append("SubscriptionIds")
                                                         .Append("|||")
                                                         .Append(SubscriptionID).ToString();
                return Convert.ToString(ViewState["SubscriptionIDForStatusReport"]);
            }
        }
    }
}