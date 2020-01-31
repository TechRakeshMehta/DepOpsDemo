using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgOperations.Views;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public partial class ManageCustomAtrribute : BaseUserControl, IManageCustomAtrribute
    {
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Custom Attributes";
                base.SetPageTitle("Custom Attributes");
                base.OnInit(e);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}