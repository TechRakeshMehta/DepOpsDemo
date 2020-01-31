using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceOperations.Views;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Pages
{
    public partial class CompliancePackageDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            MaintainScrollPositionOnPostBack = true;
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);

                CompliancePackageDetails ucCompliancePackageDetails = Page.LoadControl("~/ComplianceOperations/UserControl/CompliancePackageDetails.ascx") as CompliancePackageDetails;
                ucCompliancePackageDetails.TenantID = args.ContainsKey("TenantId") ? Convert.ToInt32(args["TenantId"]) : AppConsts.NONE; ;
                ucCompliancePackageDetails.ClientCompliancePackageID = args.ContainsKey("PackageId") ? Convert.ToInt32(args["PackageId"]) : AppConsts.NONE; ;
                ucCompliancePackageDetails.ApplicantId = args.ContainsKey("ApplicantId") ? Convert.ToInt32(args["ApplicantId"]) : AppConsts.NONE; ;
                ucCompliancePackageDetails.WorkQueue = "ComplianceSearch";
                ucCompliancePackageDetails.PackageSubscriptionId = args.ContainsKey("PackageSubscriptionId") ? Convert.ToInt32(args["PackageSubscriptionId"]) : AppConsts.NONE; ;
                ucCompliancePackageDetails.ControlUseType = INTSOF.Utils.AppConsts.DASHBOARD;
                ucCompliancePackageDetails.IsCallingFromRotationDetailScreen = true;
                pnl.Controls.Add(ucCompliancePackageDetails);
            }
            Page.Title = "School Compliance";
        }
    }
}