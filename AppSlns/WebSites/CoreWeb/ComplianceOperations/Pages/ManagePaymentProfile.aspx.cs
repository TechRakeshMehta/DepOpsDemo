using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.AuthNet.Business;
using INTSOF.AuthNet.Business.CustomerProfileWS;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Pages
{
    public partial class ManagePaymentProfile : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long customerProfileId = Convert.ToInt64(Request.QueryString["CustomerProfileId"]);
           // token.Value = AuthorizeNetCreditCard.GetToken(customerProfileId);
            //GetToken(customerProfileId);
        }
    }
}