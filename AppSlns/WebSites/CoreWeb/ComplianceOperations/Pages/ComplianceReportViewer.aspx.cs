using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Pages
{
    public partial class ComplianceReportViewer : System.Web.UI.Page
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private String DocumentType;
        private Int32 TenantID;
        private String ReportType;
        private Int32 PkgSubscriptionIds;
        private String ShrdCategoryIds;
        private Int32 SnpShtId;
        private Int32 UserId;
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["DocumentType"] != null)
                DocumentType = Convert.ToString(Request.QueryString["DocumentType"]);
            if (Request.QueryString["tenantId"] != null && !Request.QueryString["tenantId"].Trim().Equals(""))
                TenantID = Convert.ToInt32(Request.QueryString["tenantId"]);
            if (Request.QueryString["ReportType"] != null && !Request.QueryString["ReportType"].Trim().Equals(""))
                ReportType = Convert.ToString(Request.QueryString["ReportType"]);
            if (Request.QueryString["Psid"] != null && !Request.QueryString["Psid"].Trim().Equals(""))
                PkgSubscriptionIds = Convert.ToInt32(Request.QueryString["Psid"]);
            if (Request.QueryString["ShrdCatIds"] != null && !Request.QueryString["ShrdCatIds"].Trim().Equals(""))
                ShrdCategoryIds = Convert.ToString(Request.QueryString["ShrdCatIds"]);
            if (Request.QueryString["SnpShtId"] != null && !Request.QueryString["SnpShtId"].Trim().Equals(""))
                SnpShtId = Convert.ToInt32(Request.QueryString["SnpShtId"]);
            if (Request.QueryString["UserID"] != null && !Request.QueryString["UserID"].Trim().Equals(""))
                UserId = Convert.ToInt32(Request.QueryString["UserID"]);
            iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&Psid={1}&ShrdCatIds={2}&SnpShtId={3}&tenantId={4}&ReportType={5}&UserID={6}", DocumentType, PkgSubscriptionIds, ShrdCategoryIds, SnpShtId, TenantID, ReportType, UserId);

        }

        #endregion

        #endregion

    }
}