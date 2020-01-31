using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;

namespace CoreWeb.ReportsTableau.Views
{
	public partial class LinkTableauServer : BaseUserControl, ILinkTableauServerView
    {
		protected void Page_Load(object sender, EventArgs e)
		{
            Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:OpenTableauServer(); ", true);
        }		
	}
}