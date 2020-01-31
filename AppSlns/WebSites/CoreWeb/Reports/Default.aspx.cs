using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

namespace CoreWeb.Reports.Views
{
    public partial class ReportingDefault : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        protected override void OnInitComplete(EventArgs e)
        {
            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                SetModuleTitle("Reports");

                //Code to format module bar 
                this.Form.Attributes["class"] = "useDefaultModule";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}