using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceAdministration
{
    public partial class ManageUniversalMapping : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //To Do on Page Initialized 
            }

            Response.Redirect("Pages/SetupUniversalMapping.aspx");
        }
    }
}