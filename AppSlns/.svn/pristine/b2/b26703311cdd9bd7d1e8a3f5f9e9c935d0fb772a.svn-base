using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.AgencyHierarchy.Views;
using INTSOF.Utils;

namespace CoreWeb.AgencyHierarchy.Pages
{
    public partial class PackageCategoryDetailPopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);

                PackageCategoryDetailsPopUp ucPackageCategoryDetailsPopUp = Page.LoadControl("~/AgencyHierarchy/UserControls/PackageCategoryDetailsPopUp.ascx") as PackageCategoryDetailsPopUp;
                ucPackageCategoryDetailsPopUp.RequirementPackageID = args.ContainsKey("RequirementPackageID") ? Convert.ToInt32(args["RequirementPackageID"]) : AppConsts.NONE;
                pnl.Controls.Add(ucPackageCategoryDetailsPopUp);
            }
            Page.Title = "Associated Categories";
        }
    }
}