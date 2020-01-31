using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ExistingOrderInformationPage : BaseWebPage
    {
        #region [Events]
        #region [Page Events]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["ParentDivId"] != null && !Request.QueryString["ParentDivId"].Trim().Equals(""))
                {
                    hdnParentDiv.Value = Request.QueryString["ParentDivId"].ToString();
                }
                String OrderNumber = String.Empty;
                if (Request.QueryString["OrderNumber"] != null && !Request.QueryString["OrderNumber"].Trim().Equals(""))
                {
                    OrderNumber = Request.QueryString["OrderNumber"].ToString();
                }
                //IsCompletePaymentPopup
                if (Request.QueryString["IsCompletePaymentPopup"] != null && !Request.QueryString["IsCompletePaymentPopup"].Trim().Equals(""))
                {
                    var IsCompletePaymentPopup = Convert.ToBoolean(Request.QueryString["IsCompletePaymentPopup"]);
                    if (IsCompletePaymentPopup)
                    {
                        lblHeader.Text = "Complete your payment";
                        lblOrderInfo.Text = "You currently have an incomplete order for this package. Please click on “OK” and go to your Order History screen to complete payment for order " + OrderNumber + ". Please click “Ignore” if you wish to proceed with a new order.";
                    }
                    else
                    {
                        lblHeader.Text = "Renew your subscription";
                        lblOrderInfo.Text = "You currently have an expired subscription for this package. Please click “OK” and go to your Order History screen to renew that subscription. Please click “Ignore” if you wish to proceed with a new order.";
                    }

                }
            }
        }
        #endregion

        #region [Button Events]
        protected void cbExistingOrderInformation_CancelClick(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "closeExisitngOrderInformationClosePopUpEvent(false);", true);
        }
        protected void cbExistingOrderInformation_SaveClick(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "closeExisitngOrderInformationClosePopUpEvent(true);", true);
        }
        #endregion
        #endregion
    }
}