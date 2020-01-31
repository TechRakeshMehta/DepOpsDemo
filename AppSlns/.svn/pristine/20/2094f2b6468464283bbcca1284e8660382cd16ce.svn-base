using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.CommonOperations
{
    public partial class ContractManagement : BaseUserControl
    {
        /// <summary>
        ///  set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.Title = "Contracts";
            base.SetPageTitle("Contracts");
            base.OnInit(e);
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BasePage page = (base.Page) as BasePage;
            page.SetModuleTitle("Contract Management");
        }
    }
}