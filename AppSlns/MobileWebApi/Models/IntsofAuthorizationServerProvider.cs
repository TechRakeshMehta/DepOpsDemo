using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using CoreWeb.IntsofSecurityModel;
using System.Text.RegularExpressions;
using INTSOF.Utils.Consts;
using INTSOF.Utils;
using MobileWebApi.Service;
using Microsoft.Owin.Security;
using System.Threading;

namespace MobileWebApi
{
    public class IntsofAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //string clientId;
            //string clientSecret;
            //if (context.TryGetBasicCredentials(out clientId, out clientSecret))
            //{
            //    // validate the client Id and secret against database or from configuration file.  
            //    context.Validated();
            //}
            //else
            //{
            //    context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization header");
            //    context.Rejected();
            //}
            context.Validated(); //It means we have validate the client application
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                var form = await context.Request.ReadFormAsync();
                Boolean IsAutoLogin = false;
                if (string.Equals(form["isautologin"], "1", StringComparison.OrdinalIgnoreCase))
                {
                    IsAutoLogin = true;
                }
                
                var requestUrl = context.OwinContext.Request.Host.ToString();
                var userResult = ApplicantAccountService.GetApplicantAccountDetails(context.UserName, context.Password, requestUrl, IsAutoLogin);

                if (userResult.IsUserAuthenticated)
                {
                    // identity.AddClaim(new Claim("Role", userResult.UserRole));
                    identity.AddClaim(new Claim("Fullname", String.Concat(userResult.organizationUserContract.FirstName, " ", userResult.organizationUserContract.LastName)));
                    identity.AddClaim(new Claim("UserName", context.UserName));
                    identity.AddClaim(new Claim("OrganizationUserID", userResult.organizationUserContract.OrganizationUserID.ToString()));
                    identity.AddClaim(new Claim("TenantID", userResult.organizationUserContract.Organization.TenantID.ToString()));
                    context.Validated(identity);
                }
                else
                {
                    if(userResult.ResponseMessage.IsNullOrEmpty())
                    {
                        context.SetError(MobileWebApiResource.InvalidApiAccessMessageType, MobileWebApiResource.InvalidApiAccessMessageValue);
                    }
                    else
                    {
                        context.SetError(MobileWebApiResource.InvalidApiAccessMessageType, userResult.ResponseMessage);
                    }
                    
                    //context.Rejected();
                }
            }
            //catch (Exception ex)
            //{
            //    if(ex.Message.IsNullOrEmpty())
            //    {
            //        context.SetError(MobileWebApiResource.InvalidApiAccessMessageType, MobileWebApiResource.InvalidApiAccessMessageValue);
            //    }
            //    else
            //    {
            //        context.SetError(MobileWebApiResource.InvalidApiAccessMessageType, ex.Message);
            //    }
            //}
            catch (Exception)
            {
                context.SetError(MobileWebApiResource.InvalidApiAccessMessageType, MobileWebApiResource.InvalidApiAccessMessageValue);
            }

        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            //validate your client  
            //var currentClient = context.ClientId;  

            //if (Client does not match)  
            //{  
            //    context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");  
            //    return Task.FromResult<object>(null);  
            //}  

            // Change authentication ticket for refresh token requests  
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);

            context.Validated(newTicket);

            //return Task.FromResult<object>(null);
        }
    }
}