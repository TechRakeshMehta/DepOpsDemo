using CoreWeb.ComplianceOperations.Views;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class RenewOrderPopup : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Capture querystring data.
                if (Request.QueryString.IsNotNull())
                {
                    ucPackageSubscription.IsFromRenewOrderPopup = !Request.QueryString["IsFromRenewOrderPopup"].IsNullOrEmpty() ? Convert.ToBoolean(Request.QueryString["IsFromRenewOrderPopup"]) : false;
                }
            }
        }

    }
}