using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using System.Text;
using System.Xml;
using Business.RepoManagers;
namespace CoreWeb
{
    public partial class SsoPreHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String hostname = String.Empty;
            Dictionary<String, String> encryptedQueryString = null;
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {// This code will run in second flow. It will obtain the previous host from query string and set in session at new subdomain.
                encryptedQueryString = new Dictionary<String, String>();
                encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                if (!encryptedQueryString.IsNull())
                {
                    if (encryptedQueryString.ContainsKey("HostNameForSSO") && !encryptedQueryString["HostNameForSSO"].IsNullOrEmpty())
                    {
                        hostname = encryptedQueryString["HostNameForSSO"];
                    }
                }
            }
            
            if (hostname.IsEmpty())
            {// This code will run in first pass, when it will get the host name and set in query string. it will redirect to same page with new subdomain.
                Handlessourls();
            }
            else {
                RedirectToIDP(hostname);
            }
        }

        public void Handlessourls()
        {
            String Host = HttpContext.Current.Request.Url.Host;
            //Host = "uconndev.complio.com";
     
           List<String> idpUrlData = SecurityManager.GetClientIdpUrl(Host);
           if (idpUrlData.Count > 0 && !String.IsNullOrEmpty(idpUrlData[1]) && !String.IsNullOrEmpty(idpUrlData[2]))
           {
              
               string idpUrl = idpUrlData[2];
               string clientUrl = idpUrl.Substring(idpUrl.IndexOf("//")+2);
               clientUrl = clientUrl.Substring(0, clientUrl.IndexOf("/"));
               Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "HostNameForSSO", Host},
                                                                 
                                                                 };
               Response.Redirect(String.Format(Request.Url.Scheme + "://" + clientUrl + "/SsoPreHandler.aspx?args={0}", queryString.ToEncryptedQueryString()));
               
           }

            
        }

        private void RedirectToIDP(String Host)
        {
            Session["Session_Host"] = Host;
            List<String> idpUrlData = SecurityManager.GetClientIdpUrl(Host);
            if (idpUrlData.Count > 0 && !String.IsNullOrEmpty(idpUrlData[1]) && !String.IsNullOrEmpty(idpUrlData[2]))
            {
                string clientUrl = idpUrlData[1];
                string idpUrl = idpUrlData[2];
                Response.Redirect(idpUrl);
            }
        }
    }
}