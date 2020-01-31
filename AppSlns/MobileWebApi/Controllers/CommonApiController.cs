using MobileWebApi.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using INTSOF.Utils;
using System.Collections;
using MobileWebApi.Models;
using Business.RepoManagers;
using Entity;
using System.Text;

namespace MobileWebApi.Controllers
{
    [RoutePrefix("CommonApi")]
    public class CommonApiController : AnonymousApiController
    {
        [Route("GetTenantName")]
        [HttpGet]
        public HttpResponseMessage GetTenantName()
        {
            try
            {
                TenantContract url = new TenantContract();
                url = CommonService.GetTenantDetails();
                return Request.CreateResponse(HttpStatusCode.OK, url);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }

        }
        //[Route("GetTenantName")]
        //[HttpPost]
        //public HttpResponseMessage GetTenantName([FromBody]TenantContract url)
        //{
        //    try
        //    {
        //        if (url != null && !String.IsNullOrEmpty(url.TenantUrl))
        //        {
        //            CommonService.GetTenantDetails(url.TenantUrl);
        //            return Request.CreateResponse(HttpStatusCode.OK, url);
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, MobileWebApiResource.MissingTenantUrl);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogExceptionService.LogError(ex);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
        //    }

        //}

        [Route("GetTranslatedList")]
        [HttpPost]
        //public HttpResponseMessage GetTranslatedList(String key, String languageCode = default(String))
        public HttpResponseMessage GetTranslatedList(LanguageTranslatedContract languageTranslatedContract)//(List<ApiResourceContract> lstKeysToGetTranslateValue, String languageCode = default(String))
        {
            try
            {
                List<ApiResourceContract> lstLanguageContract = new List<ApiResourceContract>();
                if (languageTranslatedContract != null)
                {
                    LanguageContract currentLanguage = new LanguageContract();
                    string languageCode = languageTranslatedContract.languageCode;
                    List<ApiResourceContract> lstKeysToGetTranslateValue = languageTranslatedContract.lstApiResourceContract;
                    if (languageCode == null || languageCode == "")
                    {
                        Int32 organizationUserID = 0;
                        Int32 tenantID = 0;
                        Guid userId = new Guid();
                        //Get user specific id 
                        var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                        if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                        {
                            organizationUserID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "OrganizationUserID").Value);
                            tenantID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "TenantID").Value);
                            userId = new Guid(claims.FirstOrDefault(d => d.Type == "UserID").Value);

                            if (organizationUserID > 0 && tenantID > 0 && userId != null)
                            {
                                // Get user language mapping on the basis of organization user id.
                                currentLanguage = CommonService.GetLanguageContract(userId: userId);
                            }
                        }
                    }

                    if (languageCode != null && languageCode != "")
                    {
                        //to get language contract.
                        currentLanguage = CommonService.GetLanguageContract(languageCode: languageCode);
                    }


                    if (currentLanguage != null)
                    {
                        CultureInfo cultureInfo = new CultureInfo(currentLanguage.LanguageCulture);
                        ResourceManager langRes = new ResourceManager("Resources.Language", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                        ResourceSet resSet = langRes.GetResourceSet(cultureInfo, true, true);
                        if (resSet != null && lstKeysToGetTranslateValue != null)
                        {
                            Dictionary<String, String> resSetDic = resSet.Cast<DictionaryEntry>().ToDictionary(r => r.Key.ToString(), r => r.Value.ToString());
                            lstLanguageContract = GetListTranslatedTextKeyValue(resSetDic, lstKeysToGetTranslateValue);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lstLanguageContract);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        private static List<ApiResourceContract> GetListTranslatedTextKeyValue(Dictionary<String, String> resSetDic, List<ApiResourceContract> lstKeysToGetTranslateValue)
        {
            //List<ApiResourceContract> lstLanguageContract = new List<ApiResourceContract>();

            if (resSetDic != null)
            {
                foreach (ApiResourceContract apiResourceContract in lstKeysToGetTranslateValue)
                {
                    if (apiResourceContract != null)
                    {
                        apiResourceContract.value = resSetDic.Where(cond => cond.Key == apiResourceContract.Key).FirstOrDefault().Value;
                    }
                }
            }

            return lstKeysToGetTranslateValue;
        }

        [Route("GetWebSiteWebConfig")]
        [HttpGet]
        public HttpResponseMessage GetWebSiteWebConfig()
        {
            try
            {
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    WebSiteWebConfig webSiteWebConfig = ApiSecurityManager.GetWebSiteWebConfig(tenantID);
                    var response = Request.CreateResponse(HttpStatusCode.OK, webSiteWebConfig.HeaderHtml);
                    return response;
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Location details not found");

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        [Route("GenerateVerificationCode/{email}/{selectedType}")]
        [HttpGet]
        public HttpResponseMessage GenerateVerificationCode(String email, Int32 selectedType)
        {
            try
            {
                byte[] data = Convert.FromBase64String(email);
                Boolean isUserNameReset = selectedType == 1;
                email = Encoding.UTF8.GetString(data);
                var response = new HttpResponseMessage();
                var responseContract = new ApiResponseContract();
                String verificationCode = RandomString(10);
                List<OrganizationUser> lstOrganizationUser = SecurityManager.GetOrganizationUsersByEmail(email);

                if (lstOrganizationUser.IsNotNull() && lstOrganizationUser.Count > 0)
                {
                    foreach (OrganizationUser organizationUser in lstOrganizationUser)
                    {
                        organizationUser.VerificationCode = verificationCode;
                        SecurityManager.UpdateOrganizationUser(organizationUser);
                    }
                    OrganizationUser tempOrgUser = lstOrganizationUser.FirstOrDefault();
                    String UserName = tempOrgUser.FirstName + " " + tempOrgUser.LastName;
                    Int32 OrganizationUserId = tempOrgUser.OrganizationUserID;
                    var tenantID = tempOrgUser.Organization.TenantID ?? 0;
                    Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                                  {
                                                                      //{"UserName",CurrentViewContext.ViewContract.UserName},
                                                                      //{"Code",CurrentViewContext.ViewContract.VerificationCode},
                                                                      // {"TenantName",CurrentViewContext.ViewContract.TenantName}
                                                                      {EmailFieldConstants.USER_FULL_NAME,UserName},
                                                                      {EmailFieldConstants.VERIFICATION_CODE,verificationCode},
                                                                      {EmailFieldConstants.INSTITUTE_NAME, ApiSecurityManager.GettenantName(tenantID) },
                                                                      {EmailFieldConstants.TENANT_ID,tenantID },
                                                                      {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,OrganizationUserId}
                                                                  };
                    if (isUserNameReset)
                    {
                        //SysXEmailService.SendMail(contents, SysXUtils.GetMessage(ResourceConst.SECURITY_VERIFICATION_CODE_DETAILS) + CurrentViewContext.ViewContract.UserName, CurrentViewContext.ViewContract.Email, AppConsts.VerficationCodeUserName, AppConsts.Normal);
                        SecurityManager.PrepareAndSendSystemMail(email, contents, CommunicationSubEvents.NOTIFICATION_VERIFICATION_CODE_USERNAME, tenantID, false);

                    }
                    else
                    {
                        //mailStatus = SysXEmailService.SendMail(contents, SysXUtils.GetMessage(ResourceConst.SECURITY_VERIFICATION_CODE_DETAILS) + CurrentViewContext.ViewContract.UserName
                        //    , CurrentViewContext.ViewContract.Email, AppConsts.VerficationCode, AppConsts.Normal);
                        SecurityManager.PrepareAndSendSystemMail(email, contents, CommunicationSubEvents.NOTIFICATION_VERIFICATION_CODE_PASSWORD, tenantID, false);

                    }
                    responseContract.HasError = false;
                    responseContract.ErrorMessage = "VERIFICATIONCODEEMAILSNTINFOMSG";
                    response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                    return response;
                }
                else
                {
                    responseContract.HasError = true;
                    responseContract.ErrorMessage = "REGISTEREDEMAILREQUIRED";
                    response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                    return response;
                }


            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }

        }

        [Route("ValidateVerificationCode/{email}/{code}/{selectedType}")]
        [HttpGet]
        public HttpResponseMessage ValidateVerificationCode(String email, String code, Int32 selectedType)
        {
            try
            {
                byte[] data = Convert.FromBase64String(email);
                Boolean isUserNameReset = selectedType == 1;
                email = Encoding.UTF8.GetString(data);
                var response = new HttpResponseMessage();
                var responseContract = new ApiResponseContract();

                List<OrganizationUser> lstOrganizationUser = SecurityManager.GetOrganizationUsersByEmail(email);


                if (lstOrganizationUser.IsNotNull() && lstOrganizationUser.Count > 0 && !lstOrganizationUser.Any(x => x.VerificationCode != code))
                {
                    OrganizationUser tempOrgUser = lstOrganizationUser.FirstOrDefault();
                    String UserName = tempOrgUser.FirstName + " " + tempOrgUser.LastName;
                    Int32 OrganizationUserId = tempOrgUser.OrganizationUserID;
                    var tenantID = tempOrgUser.Organization.TenantID ?? 0;
                    if (isUserNameReset)
                    {
                        String LoginUserName = tempOrgUser.aspnet_Users.UserName;
                        Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                              {
                                                                  //{"UserName", CurrentViewContext.ViewContract.UserName},
                                                                  //{"LoginUserName", CurrentViewContext.ViewContract.LoginUserName},
                                                                  //{"TenantName",CurrentViewContext.ViewContract.TenantName}
                                                                  {EmailFieldConstants.USER_FULL_NAME, UserName},
                                                                  {EmailFieldConstants.USER_NAME,LoginUserName},
                                                                  {EmailFieldConstants.INSTITUTE_NAME,ApiSecurityManager.GettenantName(tenantID)},
                                                                  {EmailFieldConstants.TENANT_ID,tenantID},
                                                                  {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,OrganizationUserId}
                                                              };

                        //c
                        SecurityManager.PrepareAndSendSystemMail(email, contents, CommunicationSubEvents.NOTIFICATION_FORGET_USERNAME_RESET, tenantID, false);

                    }
                    responseContract.HasError = false;
                    response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                    return response;
                }

                else
                {
                    responseContract.HasError = true;
                    responseContract.ErrorMessage = "ENTRCRCTVERIFICATIONCODE";
                    response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("UpdatePassword/{email}/{password}")]
        [HttpGet]

        public HttpResponseMessage UpdatePassword(String email, String password)
        {
            try
            {
                byte[] data = Convert.FromBase64String(email);

                email = Encoding.UTF8.GetString(data);
                var response = new HttpResponseMessage();
                var responseContract = new ApiResponseContract();
                //OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);
                List<OrganizationUser> lstOrganizationUser = null;
                lstOrganizationUser = SecurityManager.GetOrganizationUsersByEmail(email);
                String oldPassword = lstOrganizationUser.FirstOrDefault().aspnet_Users.aspnet_Membership.Password;

                //To prevent that new password is not same as old password.
                aspnet_Membership memberShip = new aspnet_Membership();                
                memberShip.Password = SysXMembershipUtil.HashPasswordIWithSalt(password, lstOrganizationUser.FirstOrDefault().aspnet_Users.aspnet_Membership.PasswordSalt);
                if (oldPassword== memberShip.Password)
                {
                    responseContract.HasError = true;
                    responseContract.ErrorMessage = "OLDPASWRDCNTSAMETONEWPASWRD";
                    response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                    return response;
                }

                //UAT-4164
                Boolean isUserExixtInLocationTenants = false;
                isUserExixtInLocationTenants = ApiSecurityManager.IsUserExixtInLocationTenants(lstOrganizationUser.FirstOrDefault().UserID);

                if (SecurityManager.IsPasswordExistsInHistory(lstOrganizationUser.FirstOrDefault().UserID, password, isUserExixtInLocationTenants))
                {
                    responseContract.HasError = true;
                    responseContract.ErrorMessage = "PASWRDSHOULDNTSAMETOOLDTENPASWRD";
                    response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                    return response;
                }

                SecurityManager.UpdatePasswordDetails(lstOrganizationUser.FirstOrDefault(), lstOrganizationUser.FirstOrDefault().OrganizationUserID, oldPassword); // This will update the PasswordHistory table too.

                foreach (OrganizationUser organizationUser in lstOrganizationUser)
                {
                    organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(password, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
                    organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate = DateTime.Now;
                    organizationUser.IsNewPassword = false;
                    SecurityManager.UpdateOrganizationUser(organizationUser);
                }
                
                responseContract.HasError = false;
                responseContract.ErrorMessage = "PSWRDCHNGESUCSESFLY";
                response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                return response;
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
            //SysXUtils.GetMessage(ResourceConst.SECURITY_CHANGE_PASSWORD_SUCCESSFULLY);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [Route("ChangePassword/{email}/{password}/{oldPassword}")]
        [HttpGet]
        public HttpResponseMessage ChangePassword(String email, String password, string oldPassword)
        {
            try
            {
                byte[] data = Convert.FromBase64String(email);

                email = Encoding.UTF8.GetString(data);
                var response = new HttpResponseMessage();
                var responseContract = new ApiResponseContract();
                //OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);
                List<OrganizationUser> lstOrganizationUser = null;

                lstOrganizationUser = SecurityManager.GetOrganizationUsersByEmail(email);

                String OldPassword = SysXMembershipUtil.HashPasswordIWithSalt(oldPassword, lstOrganizationUser.FirstOrDefault().aspnet_Users.aspnet_Membership.PasswordSalt);

                //if (!organizationUser.aspnet_Users.aspnet_Membership.Password.Equals(oldPassword))
                if (!lstOrganizationUser.Any(x => x.aspnet_Users.aspnet_Membership.Password.Equals(OldPassword)))
                {
                    //View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_OLD_PASSWORD_MATCHING);
                    responseContract.HasError = true;
                    responseContract.ErrorMessage = "OLDPASWRDNOTMATCH";
                    response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                    return response;
                }

                //UAT-4164
                Boolean isUserExixtInLocationTenants = false;
                isUserExixtInLocationTenants = ApiSecurityManager.IsUserExixtInLocationTenants(lstOrganizationUser.FirstOrDefault().UserID);

                if (SecurityManager.IsPasswordExistsInHistory(lstOrganizationUser.FirstOrDefault().UserID, password, isUserExixtInLocationTenants))
                {
                    responseContract.HasError = true;
                    responseContract.ErrorMessage = "PASWRDSHOULDNTSAMETOOLDTENPASWRD";
                    response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                    return response;
                }

                SecurityManager.UpdatePasswordDetails(lstOrganizationUser.FirstOrDefault(), lstOrganizationUser.FirstOrDefault().OrganizationUserID, OldPassword); // This will update the PasswordHistory table too.

                foreach (OrganizationUser organizationUser in lstOrganizationUser)
                {
                    organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(password, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
                    organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate = DateTime.Now;
                    organizationUser.IsNewPassword = false;
                    SecurityManager.UpdateOrganizationUser(organizationUser);
                }

                responseContract.HasError = false;
                responseContract.ErrorMessage = "PSWRDCHNGESUCSESFLY";
                response = Request.CreateResponse(HttpStatusCode.OK, responseContract);
                return response;
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
            //SysXUtils.GetMessage(ResourceConst.SECURITY_CHANGE_PASSWORD_SUCCESSFULLY);
        }


        //UAT-4164 -- Password expiry implementation in mobile.
        [Route("IsPasswordExpired")]
        [HttpGet]
        public HttpResponseMessage IsPasswordExpired()
        {          
            try
            {
                Boolean result = false;
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                Int32 organizationUserID = 0;
                Guid userId = Guid.Empty;
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    organizationUserID = Convert.ToInt32(claims.First(d => d.Type == "OrganizationUserID").Value);
                    userId = Guid.Parse(SecurityManager.GetUserIdFromOrgUserId(organizationUserID));
                }

                if (ApiSecurityManager.IsUserExixtInLocationTenants(userId))
                {                    
                    result = ApiSecurityManager.IsPasswordNeedToBeChanged(userId);                    
                }

                return Request.CreateErrorResponse(HttpStatusCode.OK, result.ToString());
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
        [Route("IsSuffixDropDown")]
        [HttpGet]
        public HttpResponseMessage IsSuffixDropDown()
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                
                Boolean IsDropDown = ApiSecurityManager.IsSuffixDropDown();
                return Request.CreateResponse(HttpStatusCode.OK, IsDropDown.ToString());
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

    }       
}
