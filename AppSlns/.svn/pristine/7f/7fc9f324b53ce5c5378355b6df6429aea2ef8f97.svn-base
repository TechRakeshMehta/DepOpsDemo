using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BackgroundReports
{
    public partial class Default : BasePage
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
                SetModuleTitle("Background Reports");

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