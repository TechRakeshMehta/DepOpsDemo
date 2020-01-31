using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Pages
{
    public partial class AddEditRotation : BaseWebPage
    {
        #region  Page Events

        protected override void OnInit(EventArgs e)
        {
            base.Title = "Add Rotation";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //Capture Querystring parameters
                //CaptureQuerystringParameters();
            }
        }

        #endregion
    }
}