using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageShotSeriesSetup : BaseUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

            }
            Response.Redirect("Pages/SetupShotSeries.aspx");
        }
    }
}