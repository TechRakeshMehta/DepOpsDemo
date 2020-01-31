using System;
using System.Security.Policy;
using System.Text;
using System.Web.UI;
//using INTSOF.AuthNet.Business;
using Microsoft.Practices.ObjectBuilder;

namespace CoreWeb.Student.Views
{
    public partial class StudentRegistration : BaseUserControl, IStudentRegistrationView
    {
        private StudentRegistrationPresenter _presenter=new StudentRegistrationPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            //base.SetPageTitle("Register");
        }




        
        public StudentRegistrationPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            try
            {
                //PaymentManager paymentManager = new PaymentManager();
                ////paymentManager.PlaceAuth("4007000000027", "1113", 12000, "Test Transction");
                ////paymentManager.CaptureAuth(100, "2193327821", "AUI3JC");
                //PaymentFormBuilder pfb = paymentManager.SubmitCard("1221", "350");

                //StringBuilder sb = new StringBuilder();
                
                //sb.Append("<div style=\"text-align: center; padding-top: 20%; \"><img src=\"App_Themes/Red/Images/loading_animation.gif\" alt=\"Please wait.\" /><br /><br /><br />");
                //sb.Append("Please wait while we transfer request to payment processor. </div>");
                //sb.Append("<form target='_top' name='authnetform' method='post' action='https://test.authorize.net/gateway/transact.dll'>");

                //for (int i = 0; i < pfb.Inputs.Keys.Count; i++)
                //{
                //    //sb.Append(string.Format("", pfb.Inputs.Keys[i], pfb.Inputs[pfb.Inputs.Keys[i]]));
                //    sb.Append(string.Format("<input type='hidden' name=\"" + pfb.Inputs.Keys[i] + "\" value=\"" + pfb.Inputs[pfb.Inputs.Keys[i]] + "\" />", "Username"));
                //    sb.Append(string.Format("<input type='hidden' name=\"{0}\" value=\"{1}\" />", pfb.Inputs.Keys[i], pfb.Inputs[pfb.Inputs.Keys[i]]));

                //}
              
                ////sb.Append("<script type='text/javascript'> $(document).ready(function() {alert('kkk');}); </script>");
                //sb.Append("</form>");
                
                //poster.InnerHtml = sb.ToString();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "MyScript", "document.forms['authnetform'].submit();", true);

                //////System.Web.HttpContext.Current.Response.End();

            }

            //catch (System.Threading.ThreadAbortException ex)
            //{

            //    // do nothing

            //}

            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
    }
}

