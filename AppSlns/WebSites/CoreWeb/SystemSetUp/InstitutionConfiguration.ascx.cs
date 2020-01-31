using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

namespace CoreWeb.SystemSetUp.Views
{
    public partial class InstitutionConfiguration : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("Pages/InstitutionConfigurationPage.aspx");
        }
    }
}