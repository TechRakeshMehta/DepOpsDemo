using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Script.Serialization;

namespace MobileWebApi
{
    public class IntsofAuthorize : AuthorizeAttribute
    {
        public bool ByPassAuthorization { get; set; }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //HttpContext.Current.Session.Abandon();
            if (AuthorizeRequest(actionContext))
            {
                return;
            }

            HandleUnauthorizedRequest(actionContext);

        }

        private bool AuthorizeRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //If any action/Controller has bypass attribute then authorize that request
            if (ByPassAuthorization || GetApiAuthorizeAttributes(actionContext.ActionDescriptor).Any(x => x.ByPassAuthorization))
                return true;

            //Write your code here to perform authorization
            var headerdata = actionContext;
            if (headerdata.Request.Headers.Authorization != null)
            {                
                if (headerdata.Request.Headers.Authorization.Scheme == MobileWebApiResource.AuthorizationBearerScheme)
                {
                    var oAuthToken = headerdata.Request.Headers.Authorization.Parameter;
                    if (oAuthToken != null)
                    {
                        string token = oAuthToken;
                        AuthenticationTicket ticket = Startup.OAuthBearerOptions.AccessTokenFormat.Unprotect(token);
                        if (ticket == null || ticket.Properties == null || ticket.Properties.ExpiresUtc == null)
                        {
                            return false;
                        }
                        var expirationTime = ticket.Properties.ExpiresUtc;
                        if (expirationTime < DateTime.UtcNow)
                        {
                            return false;
                        }
                        var res = headerdata.Request.Headers.Where(d => d.Key == MobileWebApiResource.ApiSecurityRefreshToken).FirstOrDefault();

                        if (!String.IsNullOrEmpty(res.Value.FirstOrDefault()))
                        {
                            String refresh_Token = res.Value.FirstOrDefault();
                            var serializer = new JavaScriptSerializer();
                            try
                            {
                                if (expirationTime < DateTime.UtcNow.AddMinutes(15))
                                {
                                    Dictionary<string, object> result = (serializer.DeserializeObject(GetNewAccessToken(refresh_Token)) as Dictionary<string, object>);
                                    var new_Token = result["access_token"];
                                   
                                    var refresh_token = result[MobileWebApiResource.ApiSecurityRefreshToken];
                                    //HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "refresh_token");
                                    //HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "auth_token");
                                    //HttpContext.Current.Response.Headers.Add("refresh_token", refresh_token.ToString());
                                    //HttpContext.Current.Response.Headers.Add("auth_token", new_Token.ToString());
                                    AuthenticationTicket updatedTicket = Startup.OAuthBearerOptions.AccessTokenFormat.Unprotect(new_Token.ToString());
                                    //var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                                    //updatedTicket.Identity.AddClaim(new Claim("refresh_token", refresh_token.ToString()));
                                    var claims = updatedTicket.Identity.Claims.ToList();
                                    ClaimsPrincipal principal = Thread.CurrentPrincipal as ClaimsPrincipal;
                                    if (principal != null)
                                    {
                                        UpdateOwinClaims.AddUpdateClaim(principal, MobileWebApiResource.ApiSecurityRefreshToken, refresh_token.ToString());
                                        UpdateOwinClaims.AddUpdateClaim(principal, MobileWebApiResource.ApiSecurityToken, new_Token.ToString());
                                    }
                                    if (claims != null && claims.Where(d => d.Type == "UserName").Any() && updatedTicket.Identity.IsAuthenticated)
                                        return true;
                                }
                                else
                                {
                                    var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();

                                    if (claims != null && claims.Where(d => d.Type == "UserName").Any() && ticket.Identity.IsAuthenticated)
                                        return true;
                                }
                               
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }

                    }
                }

                else if (headerdata.Request.Headers.Authorization.Scheme == MobileWebApiResource.AuthorizationBasicScheme)
                {
                    #region For Future Implementation
                    //var httpRequestHeader = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();

                    //httpRequestHeader = headerdata.Request.Headers.Authorization.Parameter;

                    //// string credentials = Convert.FromBase64String(Dec.ASCII.GetBytes(httpRequestHeader));// UTF8Encoding.UTF8.GetString(Convert.FromBase64String(httpRequestHeader));
                    //string credentials = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(httpRequestHeader));
                    //string[] httpRequestHeaderValues = credentials.Split(':');

                    //string username = httpRequestHeaderValues[0];

                    //string password = httpRequestHeaderValues[1];
                    //if (username == "admin" && password == "password")
                    //    return true;
                    #endregion
                }
            }
            return false;

        }
        private IEnumerable<IntsofAuthorize> GetApiAuthorizeAttributes(HttpActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes<IntsofAuthorize>(true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes<IntsofAuthorize>(true));
        }

        private string GetNewAccessToken(String refresh_Token)
        {
            var request = HttpContext.Current.Request;
            var requestUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + MobileWebApiResource.OAuthAuthorizationTokenUrl;

            // Request the access token.
            string postData = string.Format("grant_type=refresh_token&refresh_token={0}&client_id=", refresh_Token);
            byte[] postDataEncoded = System.Text.Encoding.UTF8.GetBytes(postData);

            WebRequest req = HttpWebRequest.Create(requestUrl);
            req.Method = MobileWebApiResource.PostTypeRequest;
            req.ContentType = MobileWebApiResource.UrlencodedContentType;
            req.ContentLength = postDataEncoded.Length;

            Stream requestStream = req.GetRequestStream();
            requestStream.Write(postDataEncoded, 0, postDataEncoded.Length);

            WebResponse res = req.GetResponse();
            string responseBody = null;
            using (StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
            {
                responseBody = sr.ReadToEnd();
            }
            return responseBody;
        }


    }
}