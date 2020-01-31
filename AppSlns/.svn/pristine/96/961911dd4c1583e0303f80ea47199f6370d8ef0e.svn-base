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
using INTSOF.Utils.SsoHandlers;
using System.Web.Script.Serialization;

namespace CoreWeb
{
    public partial class SsoPostHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region CommentedCOde
            //GetData();
            //String Host = String.Empty;
            //String netid = "ZZZ1234";
            //String email = "zzz00422@uconn.edu";
            //String role = "member;student";
            //String peopleSoftID = "zzz00422";
            //String targetURL = "Institution4.ComplioTest14.com" + "/secure/SsoPostHandler.aspx";

            //    String Host = String.Empty;
            //    String netid = String.Empty;
            //    String role = String.Empty;
            //    String peopleSoftID = String.Empty;
            //    String targetURL = String.Empty;
            //    String eppn = String.Empty;
            //    String email = String.Empty;

            //    //if (!HttpContext.Current.Request.Url.Host.IsNullOrEmpty())
            //    //{
            //    //    Host = HttpContext.Current.Request.Url.Host;
            //    //}


            //    if (Session["Session_Host"] != null)
            //    {
            //        Host = Convert.ToString(Session["Session_Host"]);
            //    }

            //    if (!Request.ServerVariables["HTTP_UID"].IsNullOrEmpty())
            //    {
            //        netid = Request.ServerVariables["HTTP_UID"];
            //    }
            //    if (!Request.ServerVariables["HTTP_EPPN"].IsNullOrEmpty())
            //    {
            //        eppn = Request.ServerVariables["HTTP_EPPN"];
            //    }
            //    if (!Request.ServerVariables["HTTP_MAIL"].IsNullOrEmpty())
            //    {
            //        email = Request.ServerVariables["HTTP_MAIL"];
            //    }
            //    if (!Request.ServerVariables["HTTP_UNSCOPEDAFFILIATION"].IsNullOrEmpty())
            //    {
            //        role = Request.ServerVariables["HTTP_UNSCOPEDAFFILIATION"];
            //    }
            //    if (!Request.ServerVariables["HTTP_EMPLOYEENUMBER"].IsNullOrEmpty())
            //    {
            //        peopleSoftID = Request.ServerVariables["HTTP_EMPLOYEENUMBER"];
            //    }
            //    //if (!Request.ServerVariables["HTTP_HOST"].IsNullOrEmpty())
            //    //{
            //    //    targetURL = Request.ServerVariables["HTTP_HOST"] + "/secure/SsoPostHandler.aspx";
            //    //}
            //    if (!Host.IsNullOrEmpty())
            //    {
            //        targetURL = Host + "/secure/SsoPostHandler.aspx";
            //    }


            //   // UconnSsoHandler handler=new UconnSsoHandler ();

            //    //Save history of session data in security > api.ShibbolethSSOSessionData table
            //    if (SecurityManager.GetShibbolethSettingForHistoryLogging())
            //    {
            //        XmlDocument doc = new XmlDocument();
            //        XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("sessionData"));
            //        el.AppendChild(doc.CreateElement("NetID")).InnerText = netid;
            //        el.AppendChild(doc.CreateElement("Host")).InnerText = Host;
            //        el.AppendChild(doc.CreateElement("Email")).InnerText = email;
            //        el.AppendChild(doc.CreateElement("eduPersonPrincipalName")).InnerText = eppn;
            //        el.AppendChild(doc.CreateElement("Role")).InnerText = role;
            //        el.AppendChild(doc.CreateElement("PeopleSoftID")).InnerText = peopleSoftID;
            //        String sessionData = doc.OuterXml.ToString();
            //        Boolean res = SecurityManager.SaveShibbolethSSOSessionData(sessionData, targetURL);
            //    }
            //    //Get Integartion clientID
            //    Int32 integrationClientID = SecurityManager.GetIntegrationClientForShibboleth();


            //    //Redirect to logins or user registration screen depending upen the match cases.
            //    String EmailID = String.Empty;
            //    if (!email.IsNullOrEmpty())
            //    {
            //        EmailID = email;
            //    }
            //    else
            //    {
            //        EmailID = eppn;
            //    }
            //    if (!peopleSoftID.IsNullOrEmpty() && !Host.IsNullOrEmpty() && !netid.IsNullOrEmpty() && !EmailID.IsNullOrEmpty() && !role.IsNullOrEmpty())
            //    {
            //        Dictionary<String, String> queryString = new Dictionary<String, String>
            //                                                         {
            //                                                            { "IsShibbolethLogin", true.ToString()},
            //                                                            { "PeopleSoftID", peopleSoftID},
            //                                                            { "NetID", netid},
            //                                                            { "EmailID",EmailID},
            //                                                            { "Host",Host},
            //                                                            { "Role", role },
            //                                                            {"IntegrationClientID",Convert.ToString(integrationClientID)}
            //                                                         };
            //        if (!Host.IsNullOrEmpty())
            //        {
            //            string url = string.Empty;
            //            HttpRequest request = HttpContext.Current.Request;

            //            if (request.IsSecureConnection)
            //                url = "https://";
            //            else
            //                url = "http://";

            //            url += Host;
            //            url += String.Format("/UserRegistration.aspx?args={0}", queryString.ToEncryptedQueryString());
            //            Response.Redirect(url);
            //        }
            //    }
            //}

            ////public void GetData()
            ////{
            ////    string netid = Request.ServerVariables["HTTP_UID"];
            ////    string email = Request.ServerVariables["HTTP_EPPN"];
            ////    string role = Request.ServerVariables["HTTP_UNSCOPEDAFFILIATION"];

            ////    Response.Write(netid + "----" + email + "---" + role);

            ////    Response.Write("\n");

            ////    int loop1, loop2;
            ////    NameValueCollection coll;

            ////    // Load ServerVariable collection into NameValueCollection object.
            ////    coll = Request.ServerVariables;
            ////    // Get names of all keys into a string array. 
            ////    String[] arr1 = coll.AllKeys;
            ////    for (loop1 = 0; loop1 < arr1.Length; loop1++)
            ////    {
            ////        Response.Write("Key: " + arr1[loop1] + "<br>");
            ////        String[] arr2 = coll.GetValues(arr1[loop1]);
            ////        for (loop2 = 0; loop2 < arr2.Length; loop2++)
            ////        {
            ////            Response.Write("Value " + loop2 + ": " + Server.HtmlEncode(arr2[loop2]) + "<br>");
            ////        }
            ////    }
            ////    Response.Write("\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");

            ////    //Console.WriteLine("\r\nThe following headers were received in the response:");
            ////    // Displays each header and it's key associated with the response. 
            ////    //for (int i = 0; i < Response.Headers.Count; ++i)
            ////    //    Response.Write(Response.Headers.Keys[i] + " -- " + Response.Headers[i]);
            ////    // Releases the resources of the response.
            ////    //myHttpWebResponse.Close(); 

            ////    //string server = Request.ServerVariables["persistent-id"];
            ////    //Response.Write(server);
            ////    //string server = Request.ServerVariables["HTTP_GIVENNAME"];
            ////    //Response.Write(server);
            ////    //foreach (string key in Request.ServerVariables)
            ////    //{
            ////    //    Response.Write(key);
            ////    //}

            ////    Response.Write("------------------------------------------------------------------------------------------");
            ////} 
            #endregion

            SsoHandler ssoHandler = null;
            Int32 integrationClientId = AppConsts.NONE;
            String hostName = String.Empty;//for QA

            if (Session["Session_Host"] != null)
            {
                hostName = Convert.ToString(Session["Session_Host"]);
            }

            if (!hostName.IsNullOrEmpty())
            {
                //Get the Code by Host
                Boolean isNeedToMaintainHistory = SecurityManager.GetShibbolethSettingForHistoryLogging();
                String clientName = SecurityManager.GetClientByHostName(hostName);

                if (!clientName.IsNullOrEmpty())
                {
                    if (clientName.ToLower() == AppConsts.SHIBBOLETH_UCONN)
                    {
                        ssoHandler = new UconnSsoHandler();
                        integrationClientId = SecurityManager.GetIntegrationClientForShibboleth(AppConsts.MAPPING_GROUP_CODE_UCONN);
                    }
                    else if (clientName.ToLower() == AppConsts.SHIBBOLETH_WGU)
                    {
                        ssoHandler = new WGUSsoHandler();
                        integrationClientId = SecurityManager.GetIntegrationClientForShibboleth(AppConsts.MAPPING_GROUP_CODE_WGU);
                    }
                    //UAT-3272
                    else if (clientName.ToLower() == AppConsts.SHIBBOLETH_UPENN)
                    {
                        ssoHandler = new UPENNSsoHandler();
                        integrationClientId = SecurityManager.GetIntegrationClientForShibboleth(AppConsts.MAPPING_GROUP_CODE_UPENN);
                    }
                    //UAT-3540
                    else if (clientName.ToLower() == AppConsts.SHIBBOLETH_NYU)
                    {
                        ssoHandler = new NyuSsoHandler();
                        integrationClientId = SecurityManager.GetIntegrationClientForShibboleth(AppConsts.MAPPING_GROUP_CODE_NYU);
                    }
                    //Release 175 NSC SSO
                    else if (clientName.ToLower() == AppConsts.SHIBBOLETH_NSC)
                    {
                        ssoHandler = new NscSsoHandler();
                        integrationClientId = SecurityManager.GetIntegrationClientForShibboleth(AppConsts.MAPPING_GROUP_CODE_NSC);
                    }
                    //Release 181:4998 ROSS 
                    else if (clientName.ToLower() == AppConsts.SHIBBOLETH_ROSS)
                    {
                        ssoHandler = new RossSSOHandler();
                        integrationClientId = SecurityManager.GetIntegrationClientForShibboleth(AppConsts.MAPPING_GROUP_CODE_ROSS);
                    }

                    //UPENN DENTAL SSO
                    else if (clientName.ToLower() == AppConsts.SHIBBOLETH_UPENN_DENTAL)
                    {
                        ssoHandler = new UPENNDENATLSsoHandler();
                        integrationClientId = SecurityManager.GetIntegrationClientForShibboleth(AppConsts.MAPPING_GROUP_CODE_UPENN_DENTAL);
                    }
                    //UAT-4694 BALL STATE SSO
                    else if (clientName.ToLower() == AppConsts.SHIBBOLETH_BALL_STATE)
                    {
                        ssoHandler = new BsuSsoHandler();
                        integrationClientId = SecurityManager.GetIntegrationClientForShibboleth(AppConsts.MAPPING_GROUP_CODE_BALL_STATE);
                    }
                    if (!ssoHandler.IsNullOrEmpty())
                    {
                        String sessionData = ssoHandler.ProcessSessiondata(isNeedToMaintainHistory);
                        String targetURL = Convert.ToString(ssoHandler.TargetUrl);

                        if (!sessionData.IsNullOrEmpty() && isNeedToMaintainHistory)
                            SecurityManager.SaveShibbolethSSOSessionData(sessionData, targetURL);

                        String key = Guid.NewGuid().ToString();
                        Dictionary<String, SsoHandlerContract> ssoData = new Dictionary<String, SsoHandlerContract>();
                        //Generate Web Application data.
                        SsoHandlerContract ssoHandlerContract = ssoHandler.GenerateWebApplcationData(integrationClientId);
                        if (clientName.ToLower() == AppConsts.SHIBBOLETH_BALL_STATE)
                        {
                            if (ssoHandlerContract.Role.IsNullOrEmpty())
                                ssoHandlerContract.Role = SecurityManager.CheckRoleForBSU(ssoHandlerContract.Email, ssoHandlerContract.FirstName, ssoHandlerContract.LastName); 
                        }

                        //Release 181:4998 As per requirement we need get the Role, so i get the URL based on email+FirstName+LastName
                        if (clientName.ToLower() == AppConsts.SHIBBOLETH_ROSS)
                        {
                            if (ssoHandlerContract!=null && ssoHandlerContract.Role.IsNullOrEmpty())
                                ssoHandlerContract.Role = SecurityManager.CheckRoleForROSS(ssoHandlerContract.Email, ssoHandlerContract.FirstName, ssoHandlerContract.LastName,ssoHandlerContract.UserName);
                               
                        }
                        if (!ssoHandlerContract.IsNullOrEmpty())
                        {
                            //UAT-3272
                            //user which does not contains any role between "complioStudents" or "complioAdmin" 
                            if ((clientName.ToLower() == AppConsts.SHIBBOLETH_UPENN || clientName.ToLower() == AppConsts.SHIBBOLETH_UPENN_DENTAL) && IsUPENNUserDontHaveApplicantOrAdminRole(ssoHandlerContract.Role))
                            {
                                dvShibbolethUPENNErrorMessage.Style["display"] = "block";
                            }
                            //Release 175
                            else if (clientName.ToLower() == AppConsts.SHIBBOLETH_NSC && IsNSCUserDontHaveApplicantOrAdminRole(ssoHandlerContract.Role))
                            {
                                dvShibbolethNSCErrorMessage.Style["display"] = "block";
                            }
                            else
                            {
                                //UAT-3607
                                Dictionary<String, String> dcCustomAttributesWithID = GetSSOCustomAttributeData(clientName);
                                if (!dcCustomAttributesWithID.IsNullOrEmpty())
                                {
                                    JavaScriptSerializer js = new JavaScriptSerializer();
                                    String jsonData = js.Serialize(dcCustomAttributesWithID);
                                    ssoHandlerContract.AttributesWithID = jsonData;
                                }
                                ssoData.Add("UserSsoData", ssoHandlerContract);

                                Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<SsoHandlerContract>(ssoData);
                                ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);

                                //Redirect to User registration page
                                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "IsShibbolethLogin", true.ToString()},
                                                                    { "TokenKey", key  }
                                                                 };
                                if (!hostName.IsNullOrEmpty())
                                {
                                    string url = string.Empty;
                                    HttpRequest request = HttpContext.Current.Request;

                                    if (request.IsSecureConnection)
                                        url = "https://";
                                    else
                                        url = "http://";

                                    url += hostName;
                                    url += String.Format("/UserRegistration.aspx?args={0}", queryString.ToEncryptedQueryString());
                                    Response.Redirect(url);
                                }
                            }
                        }
                        else
                        {
                            if (clientName.ToLower() == AppConsts.SHIBBOLETH_ROSS || clientName.ToLower() == AppConsts.SHIBBOLETH_WGU || clientName.ToLower() == AppConsts.SHIBBOLETH_UPENN || clientName.ToLower() == AppConsts.SHIBBOLETH_NYU || clientName.ToLower() == AppConsts.SHIBBOLETH_NSC || clientName.ToLower() == AppConsts.SHIBBOLETH_UPENN_DENTAL || clientName.ToLower() == AppConsts.SHIBBOLETH_BALL_STATE)
                            {
                                dvShibbolethWGUErrorMessage.Style["display"] = "block";
                            }
                        }
                    }
                }
            }
        }

        //UAT-3067
        //get custom atrributes values from  session on the basis of key which mapping exists.
        private Dictionary<String, String> GetSSOCustomAttributeData(String clientName)
        {
            String mappingGroupCode = String.Empty;
            Dictionary<String, String> dcCustomAttrMappings = new Dictionary<String, String>();
            if (clientName.ToLower() == AppConsts.SHIBBOLETH_UCONN)
            {
                mappingGroupCode = AppConsts.MAPPING_GROUP_CODE_UCONN;
            }
            else if (clientName.ToLower() == AppConsts.SHIBBOLETH_WGU)
            {
                mappingGroupCode = AppConsts.MAPPING_GROUP_CODE_WGU;
            }

            if (!mappingGroupCode.IsNullOrEmpty())
                dcCustomAttrMappings = SecurityManager.GetCustomAttrMappedWithShibbolethAttr(mappingGroupCode);

            Dictionary<String, String> dcAttributesWithID = new Dictionary<String, String>();
            if (!dcCustomAttrMappings.IsNullOrEmpty())
            {
                dcCustomAttrMappings.ForEach(x =>
                                {
                                    if (!HttpContext.Current.Request.ServerVariables[x.Key].IsNullOrEmpty())
                                    {
                                        dcAttributesWithID.Add(x.Value, HttpContext.Current.Request.ServerVariables[x.Key]);
                                    }
                                });
            }
            return dcAttributesWithID;
        }

        //UAT-3272
        private Boolean IsUPENNUserDontHaveApplicantOrAdminRole(String shibbolethUpennRole)
        {
            List<String> lstRoles = new List<String>(shibbolethUpennRole.ToLower().Split(':'));
            if (lstRoles.Contains("compliostudents") || lstRoles.Contains("complioadmins"))
            {
                return false;
            }
            return true;
        }
        private Boolean IsNSCUserDontHaveApplicantOrAdminRole(String shibbolethUpennRole)
        {
            List<String> lstRoles = new List<String>(shibbolethUpennRole.ToLower().Split(';'));
            if (lstRoles.Contains("nsc.edu/staff profiles") || lstRoles.Contains("nsc.edu/student accounts"))
            {
                return false;
            }
            return true;
        }
    }
}