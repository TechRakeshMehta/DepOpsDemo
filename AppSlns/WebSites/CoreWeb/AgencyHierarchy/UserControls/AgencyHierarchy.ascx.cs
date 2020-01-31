using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INTSOF.Utils;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AgencyHierarchy.UserControls
{
    public partial class AgencyHierarchy : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                Int32 selectedRootNodeID = Convert.ToInt32(args.GetValue("AgencyHierarchyID"));
                Int32 selectedHierarchyID = Convert.ToInt32(args.GetValue("SelectedHierarchyID"));
                Response.Redirect(string.Format("Pages/AgencyHierarchy.aspx?SelectedRootNodeID={0}&SelectedHierarchyID={1}&test=true", selectedRootNodeID.ToString(), selectedHierarchyID.ToString()));
            }


        }
    }
}