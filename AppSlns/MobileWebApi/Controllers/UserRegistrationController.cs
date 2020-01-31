using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel;
using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.MobileAPI;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using MobileWebApi.Models;
using MobileWebApi.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Linq;
using INTSOF.UI.Contract.SystemSetUp;
using INTSOF.UI.Contract.BkgOperations;

namespace MobileWebApi.Controllers
{

    [RoutePrefix("UserRegistration")]
    public class UserRegistrationController : AnonymousApiController
    {
        private static readonly HttpClient client = new HttpClient();
        // GET: UserRegistration
        [Route("CreateUserAccount")]
        [HttpPost]
        public HttpResponseMessage CreateUserAccount(UserContract user)
        {
            //client.Timeout = TimeSpan.FromMinutes(30);
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                    //ResponseObject = responseData
                };
                List<LookupContract> lstExistingUser = ApiSecurityManager.IsExistingUser(user);

                if (lstExistingUser.Count > AppConsts.NONE)
                {
                    var noneOfAbove = CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "NONEOFABOVE");
                    LookupContract lookupContract = new LookupContract();
                    lookupContract.Name = noneOfAbove;
                    lookupContract.Code = noneOfAbove;
                    lookupContract.UserID = -1;
                    lstExistingUser.Add(lookupContract);
                    apiResponse.ErrorMessage = "ShowExistingUsers";
                    apiResponse.ResponseObject = lstExistingUser;
                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                }

                return NormalRegistration(user);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        private HttpResponseMessage NormalRegistration(UserContract user)
        {
            var apiResponse = new ApiResponseContract
            {
                HasError = true
                //ResponseObject = responseData
            };
            AutoLoginDashboardContract responseData = new AutoLoginDashboardContract();
            responseData.IsLocationServiceTenant = false;

            if (ApiSecurityManager.IsExistsUserName(user.UserName))
                return Request.CreateResponse(HttpStatusCode.OK, new ApiResponseContract(CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "USERNAMEALREADYINUSE")) { ResponseCode = "USERNAMEALREADYINUSE" });

            if (ApiSecurityManager.IsExistsPrimaryEmail(user.PrimaryEmail))
                return Request.CreateResponse(HttpStatusCode.OK,
                                        new ApiResponseContract(CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "EMAILALREADYINUSE")) { ResponseCode = "EMAILALREADYINUSE" });

            var _user = SecurityManager.GetUserByName(user.UserName, true, true);
            if (!_user.IsNullOrEmpty())
                return Request.CreateResponse(HttpStatusCode.OK,
                                    new ApiResponseContract(CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "USERNAMEEXIST")) { ResponseCode = "USERNAMEEXIST" });
            if (!IsValidAddress(user.MasterZipcodeID, user.CountryId))
                return Request.CreateResponse(HttpStatusCode.OK,
                                    new ApiResponseContract(CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "PLSSELVALIDZIP")) { ResponseCode = "PLSSELVALIDZIP" });

            TenantContract url = new TenantContract();
            url = CommonService.GetTenantDetails();
            user.TenantID = url.TenantId;
            if (ApiSecurityManager.CreateAccount(user))
            {
                apiResponse.HasError = false;
                apiResponse.Message = "User has been created successfully";
                // some userinfo to be added here
                responseData.User = new UserContract();
                if (user.IsAutoActive)
                {
                    var values = new Dictionary<string, string>
                        {
                           { "grant_type", "password" },
                           { "username", user.UserName },
                           { "isautologin", "1" },
                           { "password", "" }
                        };
                    var data = new FormUrlEncodedContent(values);
                    var OriginalPath = Request.RequestUri.OriginalString;
                    var BaseUrl = OriginalPath.Replace(Request.RequestUri.PathAndQuery, "");
                    //client.Timeout = TimeSpan.FromMinutes(30);
                    // var data = "grant_type=password&username=" + user.UserName + "&isautologin=1" + "&password=";
                    var response = client.PostAsync(BaseUrl + "/mobileWebApi/GetAuthToken", data);

                    var responseString = response.Result.Content.ReadAsStringAsync().Result;
                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
                    {
                        // Deserialization from JSON  
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(AutoLoginDashboardContract));
                        responseData = (AutoLoginDashboardContract)deserializer.ReadObject(ms);
                    }
                }
                responseData.User = user;
                responseData.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(url.TenantId);
                apiResponse.ResponseObject = responseData;
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK,
                                    new ApiResponseContract("Please try again!!"));
        }

        private ApiResponseContract ValidateUserNameAndPassword(string LoginUserName, string LoginPassword, string SelectedLanguageCode)
        {
            //OrganizationUser ExistingOrganisationUser = null;
            var apiResponse = new ApiResponseContract
            {
                HasError = true
                //,
                //ResponseObject = ExistingOrganisationUser
            };
            SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(LoginUserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;

            try
            {
                if (System.Web.Security.Membership.ValidateUser(Regex.Replace(LoginUserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE), Regex.Replace(LoginPassword, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)))
                {
                    // Checks if the user is locked.
                    if (!user.IsNull() && user.IsLockedOut)
                    {
                        apiResponse.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED);
                    }
                    else
                    {
                        SecurityManager.ResetPasswordAttempCount(LoginUserName);
                        apiResponse.ResponseObject = SecurityManager.GetOrganizationUser(user.OrganizationUserId);
                        apiResponse.HasError = apiResponse.ResponseObject == null;
                    }
                }
                else
                {
                    apiResponse.ErrorMessage = String.Empty;
                    SecurityManager.FailedPasswordAttemptCount(LoginUserName, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempt"]));
                    //View.setSubmitbuttonText = "Try Again";                
                    if (!user.IsNull())
                    {
                        OrganizationUser orgUser = SecurityManager.GetOrganizationUser(user.OrganizationUserId);

                        if (user.IsNewPassword)
                        {
                            apiResponse.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_PWD_RECEIVED_IN_EMAIL);
                        }
                        else if (!orgUser.IsActive)
                        {
                            apiResponse.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ERROR_INACTIVE_ACCOUNT);
                        }
                        else
                        {
                            //View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);                        
                            apiResponse.ErrorMessage = CommonService.GetResourceKeyValue(SelectedLanguageCode, "INCRTUSERNAMEPSWD");
                        }
                    }
                    else
                    {
                        //View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
                        apiResponse.ErrorMessage = CommonService.GetResourceKeyValue(SelectedLanguageCode, "INCRTUSERNAMEPSWD");
                    }
                }
            }
            catch (SysXException ex)
            {
                if (ex.Message == SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED_CONTACT_ADMINISTRATOR))
                {
                    apiResponse.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED);
                }
            }
            return apiResponse;
        }

        [Route("AccountSelected/{doNormalRegistration}")]
        [HttpPost]
        public HttpResponseMessage AccountSelected(LinkAccountContract linkAccountContract, Boolean doNormalRegistration)
        {
            AutoLoginDashboardContract responseData = new AutoLoginDashboardContract();
            responseData.IsLocationServiceTenant = false;

            TenantContract url = new TenantContract();
            url = CommonService.GetTenantDetails();

            var apiResponse = new ApiResponseContract
            {
                HasError = true,
                ResponseObject = responseData
            };

            try
            {
                var user = linkAccountContract.User;
                var noneOfAbove = CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "NONEOFABOVE");
                if (doNormalRegistration
                    || linkAccountContract.LookupContract.Code == noneOfAbove)
                {
                    return NormalRegistration(user);
                }
                else
                {
                    if (SecurityManager.IsUsernameExistInTenantDB(linkAccountContract.LookupContract.Code, url.TenantId))
                    {
                        apiResponse.ResponseCode = "ACCWITHUSERNAMEEXISTS";
                        apiResponse.ErrorMessage = string.Format("{0} {1} {2}"
                            , CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "ACCWITHUSERNAME")
                            , linkAccountContract.LookupContract.Code
                            , CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "ALREADYEXISTSUNDERINST")); ;
                        return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                    }
                    apiResponse.ResponseCode = "VERIFYPASSWORD";
                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                }
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("LinkAccount")]
        [HttpPost]
        public HttpResponseMessage LinkAccount(LinkAccountContract linkAccountContract)
        {
            TenantContract url = new TenantContract();
            url = CommonService.GetTenantDetails();

            //AutoLoginDashboardContract responseData = new AutoLoginDashboardContract();
            //responseData.IsLocationServiceTenant = false;

            var apiResponse = new ApiResponseContract
            {
                HasError = true
                //ResponseObject = responseData
            };

            try
            {
                var user = linkAccountContract.User;
                var lookupContract = linkAccountContract.LookupContract;
                var validUser = ValidateUserNameAndPassword(lookupContract.Code, linkAccountContract.VerificationPassword, user.SelectedLanguageCode);
                if (validUser.HasError)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, validUser);
                }

                if (validUser.ResponseObject != null)
                {
                    user.TenantID = url.TenantId;
                    if (ApiSecurityManager.AddLinkedUserProfile(user, validUser.ResponseObject as OrganizationUser))
                    {
                        apiResponse.HasError = false;
                        apiResponse.ErrorMessage = string.Empty;
                        apiResponse.ResponseObject = user.PrimaryEmail;
                        return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                    }
                    else
                    {
                        apiResponse.HasError = true;
                        apiResponse.ErrorMessage = CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "ERROCCNTCTADMNSTR");
                        return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                                    new ApiResponseContract(CommonService.GetResourceKeyValue(user.SelectedLanguageCode, "INCRTUSERNAMEPSWD")));
                }

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                apiResponse.ErrorMessage = MobileWebApi.MobileWebApiResource.InternalServerErrorMessage;
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
        }

        private Boolean IsValidAddress(Int32 MasterZipcodeID, Int32 CountryId)
        {
            if (MasterZipcodeID == AppConsts.NONE && CountryId == AppConsts.COUNTRY_USA_ID)
                return false;
            return true;
        }

        [Route("IsLocationServiceTenant")]
        [HttpGet]
        public HttpResponseMessage IsLocationServiceTenant()
        {
            TenantContract url = new TenantContract();
            url = CommonService.GetTenantDetails();
            Boolean result = SecurityManager.IsLocationServiceTenant(url.TenantId);
            return Request.CreateErrorResponse(HttpStatusCode.OK, result.ToString());
        }

        [Route("CheckUserName/{UserName}")]
        [HttpGet]
        public HttpResponseMessage CheckUserName(String UserName)
        {
            try
            {

                Boolean result = ApiSecurityManager.IsExistsUserName(UserName);
                return Request.CreateErrorResponse(HttpStatusCode.OK, result.ToString());
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("ValidateUserViaEmailAndRedirect/{verificationCode}")]
        [HttpGet]
        public HttpResponseMessage ValidateUserViaEmailAndRedirect(String verificationCode)
        {
            try
            {
                String result = ApiSecurityManager.ValidateUserViaEmailAndRedirect(verificationCode);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetAccountVerificationSettings/{verificationCode}")]
        [HttpGet]
        public HttpResponseMessage GetAccountVerificationSettings(String verificationCode)
        {
            try
            {
                Int32 tenantID = CommonService.GetTenantDetails().TenantId;
                Dictionary<String, String> result = ApiSecurityManager.GetAccountVerificationSettings(tenantID, verificationCode);
                return Request.CreateResponse(HttpStatusCode.OK, result.ToList());
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetAccountVerficationQuestions/{verificationCode}")]
        [HttpGet]
        public HttpResponseMessage GetAccountVerficationQuestions(String verificationCode)
        {
            try
            {
                Int32 tenantID = CommonService.GetTenantDetails().TenantId;
                List<Entity.ClientEntity.lkpSetting> result = ApiSecurityManager.GetAccountVerficationQuestions(tenantID, verificationCode);
                List<LookupContract> data = new List<LookupContract>();
                foreach (var item in result)
                {
                    LookupContract tempData = new LookupContract();
                    tempData.Name = item.Name;
                    tempData.Code = item.Code;
                    data.Add(tempData);
                }

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetClientSettingCustomAttribute")]
        [HttpGet]
        public HttpResponseMessage GetClientSettingCustomAttribute()
        {
            try
            {
                Int32 tenantID = CommonService.GetTenantDetails().TenantId;
                List<ClientSettingCustomAttributeContract> data = ApiSecurityManager.GetClientSettingCustomAttribute(tenantID);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        [Route("ValidateandActivateUser/{verificationCode}")]
        [HttpPost]
        public HttpResponseMessage ValidateandActivateUser(VerificationAccountContract verficationData, String verificationCode)
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                Int32 tenantID = CommonService.GetTenantDetails().TenantId;
                Boolean IsActivated = ApiSecurityManager.ValidateandActivateUser(tenantID, verficationData.User, verficationData.lstCustomAttributes, verificationCode, verficationData.User.VerificationPermissionCode);

                if (IsActivated)
                {
                    apiResponse.HasError = false;
                }
                else
                {
                    apiResponse.ErrorMessage = "Information provided by you doesn't match with our record, please try again";
                }

                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
    }
}