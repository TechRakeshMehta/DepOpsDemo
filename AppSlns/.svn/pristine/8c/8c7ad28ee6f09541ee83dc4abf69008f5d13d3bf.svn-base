using System;
using System.Collections.Generic;
using INTSOF.Utils;

namespace CoreWeb.SystemSetUp.Pages
{
    public partial class ClientProfilePage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ucClientProfile.IsFromConfigurationPage = true;
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        ucClientProfile.OrganizationUserID = Convert.ToInt32(args.GetValue("OrganizationUserId"));
                    }
                    if (args.ContainsKey("DeptProgramMappingID"))
                    {
                        ucClientProfile.DeptProgramMappingID = Convert.ToInt32(args.GetValue("DeptProgramMappingID"));
                    }
                }
            }

        }
    }
}