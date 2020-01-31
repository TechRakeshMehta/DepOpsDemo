using System;
using System.Collections.Generic;
using System.Web.UI;
using INTSOF.Utils;
using CoreWeb.ComplianceAdministration.Views;

namespace CoreWeb.ComplianceAdministration.Pages
{
    public partial class TrackingPackageDetailPopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);

                TrackingPackageDetailsPopUp ucTrackingPackageDetailsPopUp = Page.LoadControl("~/ComplianceAdministration/UserControl/TrackingPackageDetailsPopUp.ascx") as TrackingPackageDetailsPopUp;
                ucTrackingPackageDetailsPopUp.TenantId = args.ContainsKey("TenantId") ? Convert.ToInt32(args["TenantId"]) : AppConsts.NONE;
                ucTrackingPackageDetailsPopUp.trackingPackageRequiredDOCURLId = args.ContainsKey("trackingPackageRequiredDOCURLId") ? Convert.ToInt32(args["trackingPackageRequiredDOCURLId"]) : AppConsts.NONE;
                pnl.Controls.Add(ucTrackingPackageDetailsPopUp);
            }
            Page.Title = "Name of Packages Associated";
        }
    }
}