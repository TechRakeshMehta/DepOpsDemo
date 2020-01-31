using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ClinicalRotation.Views;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Pages
{
    public partial class RotationStudentDetailPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);

                RotationStudentDetailsPopup ucRotationStudentDetailsPopup = Page.LoadControl("~/ClinicalRotation/UserControl/RotationStudentDetailsPopup.ascx") as RotationStudentDetailsPopup;
                ucRotationStudentDetailsPopup.TenantId = args.ContainsKey("TenantId") ? Convert.ToInt32(args["TenantId"]) : AppConsts.NONE;
                ucRotationStudentDetailsPopup.clinicalRotationId = args.ContainsKey("RotationID") ? Convert.ToInt32(args["RotationID"]) : AppConsts.NONE;
                ucRotationStudentDetailsPopup.AgencyID = args.ContainsKey("AgencyId") ? Convert.ToInt32(args["AgencyId"]) : AppConsts.NONE;
                ucRotationStudentDetailsPopup.CurrentLoggedInUserId = args.ContainsKey("CurrentLoggedInUserId") ? Convert.ToInt32(args["CurrentLoggedInUserId"]) : AppConsts.NONE;
                pnl.Controls.Add(ucRotationStudentDetailsPopup);
            }
            Page.Title = "# of Students";
        }
    }
}