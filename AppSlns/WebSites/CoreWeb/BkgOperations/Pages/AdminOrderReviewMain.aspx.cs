using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BkgOperations.Views
{
    public partial class AdminOrderReviewMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 TenantId = 0;
            if (!Request.QueryString["TenantId"].IsNullOrEmpty())
            {
                TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
            }
            System.Web.UI.Control CustomFormLoad = Page.LoadControl("~/BkgOperations/UserControl/AdminOrderReview.ascx");
            (CustomFormLoad as AdminOrderReview).TenantId = TenantId;
            (CustomFormLoad as AdminOrderReview).IsAdminOrderScreen = true;
            pnlAdminOrderReview.Controls.Add(CustomFormLoad);
        }
    }
}