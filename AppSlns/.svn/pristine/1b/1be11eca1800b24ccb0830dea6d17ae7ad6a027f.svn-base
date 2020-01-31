using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Pages
{
    public partial class BkgReportViewer : System.Web.UI.Page
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private String ReportName;
        //private Int32 ReportID;
        private Int32 OrderID;
        private Int32 TenantID;
        private Int32 ApplicantID;
        private Int32 ServiceGroupID;
        private String FromDate;
        private String ToDate;
        private Boolean IsFromReport;
        private String UserIdForReport;
        private String Institute;
        private String Hierarchy;
        private String UserType;
        private String HierarchyNodeID;
        private String IsReportSentToStudent;
        private String OrganizationUserID;
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                if (Request.QueryString["args"] != "PermissionVoilated")
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("ReportType"))
                        ReportName = Convert.ToString(args["ReportType"]);
                    if (args.ContainsKey("OrderID"))
                        OrderID = Convert.ToInt32(args["OrderID"]);
                    if (args.ContainsKey("TenantId"))
                        TenantID = Convert.ToInt32(args["TenantId"]);
                    if (args.ContainsKey("ApplicantID"))
                        ApplicantID = Convert.ToInt32(args["ApplicantID"]);
                    if (args.ContainsKey("PackageGroupID"))
                        ServiceGroupID = Convert.ToInt32(args["PackageGroupID"]);
                    if (args.ContainsKey("FromDate"))
                        FromDate = Convert.ToString(args["FromDate"]);
                    if (args.ContainsKey("ToDate"))
                        ToDate = Convert.ToString(args["ToDate"]);
                    if (args.ContainsKey("UserID"))
                        UserIdForReport = Convert.ToString(args["UserID"]);
                    if (args.ContainsKey("Institute"))
                        Institute = Convert.ToString(args["Institute"]);
                    if (args.ContainsKey("Hierarchy"))
                        Hierarchy = Convert.ToString(args["Hierarchy"]);
                    if (args.ContainsKey("UserType"))
                        UserType = Convert.ToString(args["UserType"]);
                    if (args.ContainsKey("HierarchyNodeID"))
                        HierarchyNodeID = Convert.ToString(args["HierarchyNodeID"]);
                    if (args.ContainsKey("IsReportSentToStudent"))
                        IsReportSentToStudent = Convert.ToString(args["IsReportSentToStudent"]);
                    if (args.ContainsKey("OrganizationUserID"))
                        OrganizationUserID = Convert.ToString(args["OrganizationUserID"]);

                    IsFromReport = true;

                    iframeBkgReportViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?ReportName={0}&tenantId={1}&OrderID={2}&ApplicantID={3}&ServiceGroupID={4}&FromDate={5}&ToDate={6}&IsFromReport={7}&UserIdForReport={8}&Institute={9}&Hierarchy={10}&UserType={11}&HierarchyNodeID={12}&IsReportSentToStudent={13}&OrganizationUserID={14}"
                        , ReportName, TenantID, OrderID, ApplicantID, ServiceGroupID, FromDate, ToDate, IsFromReport, UserIdForReport, Institute, Hierarchy, UserType, HierarchyNodeID, IsReportSentToStudent, OrganizationUserID);
                }
                else
                {
                    dvError.Visible = true;
                    iframeBkgReportViewer.Visible = false;
                }
            }
        }

        #endregion

        protected void btnGoToDashboard_Click(object sender, EventArgs e)
        {
            String url = String.Format(AppConsts.APPLICANT_MAIN_PAGE_NAME);
            Response.Redirect(url);
        }

        #endregion
    }
}