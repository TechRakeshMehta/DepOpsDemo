using CoreWeb.Shell;
using INTSOF.AuthNet.Business;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Pages
{
    public partial class ItemPaymentPopup : BaseWebPage
    {
        #region  Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            { 
            }
        }

        #endregion
    }
}