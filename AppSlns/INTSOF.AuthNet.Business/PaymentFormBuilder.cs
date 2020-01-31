using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace INTSOF.AuthNet.Business
{
    public class PaymentFormBuilder : BaseAuthNetManager
    {
        public System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();
        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";

        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public void Post()
        {
            try
            {
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

                //System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>window.location.href('http://google.co.in');</script>");
                //System.Web.HttpContext.Current.Response.Write("<body     onload=document.forms[0].submit();>");
                //System.Web.HttpContext.Current.Response.Write("<div style=\"text-align: center; padding-top: 20%; \"><img src=\"App_Themes/Red/Images/loading_animation.gif\" alt=\"Please wait.\" /><br /><br /><br />");
                //System.Web.HttpContext.Current.Response.Write("Please wait while we transfer request to payment processor. <div>");
                //System.Web.HttpContext.Current.Response.Write("<form name=\"" + FormName + "\"  method=post     action=\"" + Url + "\">");

                //for (int i = 0; i < Inputs.Keys.Count; i++)
                //{
                //    System.Web.HttpContext.Current.Response.Write(string.Format("", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                //    //System.Web.HttpContext.Current.Response.Write(string.Format("<input type=hidden name=\""+ Inputs.Keys[i]+ "\" value=\""+Inputs[Inputs.Keys[i]]+"\">", "Username"));
                //    System.Web.HttpContext.Current.Response.Write(string.Format("<input type=hidden name=\"{0}\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                //}

                //System.Web.HttpContext.Current.Response.Write("</form>");
                //System.Web.HttpContext.Current.Response.Write("</body>");
                ////System.Web.HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
