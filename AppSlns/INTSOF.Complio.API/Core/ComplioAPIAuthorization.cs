using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using INTSOF.Complio.API.ComplioAPIBusiness;

namespace INTSOF.Complio.API.Core
{
    public class ComplioAPIAuthorization : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var _isAuthenticated = false;
            string _userName = string.Empty;
            string _password = string.Empty;

            //Getting user credientials from request hedaer
            ComplioAPIManager.GetCredientials(ref _userName, ref  _password);

            try
            {
                if (System.Web.Security.Membership.ValidateUser(Regex.Replace(_userName, ComplioAPIConstants.CredentialValidationExpression, ComplioAPIConstants.ASCIISPACE),
                                                                        Regex.Replace(_password, ComplioAPIConstants.CredentialValidationExpression, ComplioAPIConstants.ASCIISPACE)))
                {
                    _isAuthenticated = true;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                }
            }
            catch (Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
            }
            return _isAuthenticated;
        }
    }
}