using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_UserControls_Export : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/pdf";
        Response.Clear();
        Response.TransmitFile(Server.MapPath("~/Resources/Mod/Student/Compliance Report.pdf"));
        Response.End();
    }
}