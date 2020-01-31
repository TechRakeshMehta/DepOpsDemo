using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

namespace CoreWeb.CommonOperations.Views
{
    public partial class SavedReports : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("Pages/SavedReportsPage.aspx");
        }
    }
}