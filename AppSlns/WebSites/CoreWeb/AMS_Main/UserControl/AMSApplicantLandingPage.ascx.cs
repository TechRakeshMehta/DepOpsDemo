using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.Main
{
    public partial class AMSApplicantLandingPage : System.Web.UI.UserControl
    {
        private String _viewType;

        protected void Page_Load(object sender, EventArgs e)
        {

            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);

            Dictionary<String, String> queryString;

            //Set Module Title
            BasePage basePage = base.Page as BasePage;
            if (basePage != null)
            {
                basePage.SetModuleTitle("AMS Applicant Landing Page");
                basePage.HideTitleBars();
            }
            //lblFirstTimeLoginMessage.Visible = false;
            lblFirstTimeLoginMessage.Text = "Welcome!";

            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();
                encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);

                if (encryptedQueryString.ContainsKey(AppConsts.NEW_REGISTERED_USER))
                {
                    lblFirstTimeLoginMessage.Text = "Thank you for registering!";
                    lblFirstTimeLoginMessage.Visible = true;
                }

            }

            //btnDashBoard.NavigateUrl = String.Format("~/Main/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            btnDashBoard.NavigateUrl = String.Format(AppConsts.DASHBOARD_PAGE_NAME);



            queryString = new Dictionary<String, String>
                                    { 
                                        { AppConsts.CHILD, ChildControls.ApplicantPendingOrder}
                                    };
            btnPendingOrder.NavigateUrl = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());




        }

    }
}