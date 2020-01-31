using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel;
using Entity;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;


namespace MobileWebApi.Service
{
    public static class ApplicantAccountService
    {
        public static ApplicantAccountDetails GetApplicantAccountDetails(String username, String password, string hostUrl, Boolean IsAutoLogin = false)
        {
            try
            {
                ApplicantAccountDetails ApplicantAccountDetailsContract = new ApplicantAccountDetails();
                OrganizationUser organizationUser = null;
                Boolean IsUserAuthenticated = false;
                String ResponseMessage = String.Empty;
                //var auser = System.Web.Security.Membership.GetUser(Regex.Replace(username, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE));
                SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(username, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE), false) as SysXMembershipUser;
                if(user.IsLockedOut)
                {
                    ResponseMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED_CONTACT_ADMINISTRATOR);
                }
               else if (IsAutoLogin || System.Web.Security.Membership.ValidateUser(Regex.Replace(username, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE), Regex.Replace(password, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)))
                {
                    // Checks if the user is locked.
                    if (!user.IsNull() && user.IsLockedOut)
                    {
                        ResponseMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED_CONTACT_ADMINISTRATOR);
                    }

                    else
                    {
                        IQueryable<OrganizationUser> lstOrgUsers = SecurityManager.GetOrganizationUserInfoByUserId(user.UserId.ToString());
                        //organizationUser = lstOrgUsers.FirstOrDefault(obj => obj.OrganizationID == user.OrganizationId);
                        var TenantDetails = CommonService.GetTenantDetailsByUrl(hostUrl);
                        var Organization = SecurityManager.GetOrganizations().FirstOrDefault(obj => obj.TenantID == TenantDetails.TenantId);
                        var OrganizationID = Organization.IsNull() ? 0 : Organization.OrganizationID;
                        organizationUser = lstOrgUsers.Where(obj => obj.OrganizationID == OrganizationID || obj.OrganizationID == user.OrganizationId)
                            .OrderByDescending(obj=>obj.OrganizationID == OrganizationID)
                            .FirstOrDefault();
                        //Added for resolving bug# no validation msg appear for inactive users.
                        if (!organizationUser.IsNullOrEmpty() && organizationUser.IsApplicant.Value)
                        {
                            if (!organizationUser.IsActive)
                            {
                                ResponseMessage = "Your account is not active. Please contact System Administrator.";
                            }
                            else
                            {
                               // ApplicantAccountDetailsContract.UserRole= GetUserType(user);
                                IsUserAuthenticated = true;
                            }
                        }
                    }
                }
                else
                {
                    IfPasswordFailed(user,ResponseMessage);
                }

                ApplicantAccountDetailsContract.IsUserAuthenticated = IsUserAuthenticated;
                ApplicantAccountDetailsContract.organizationUserContract = organizationUser;
                ApplicantAccountDetailsContract.ResponseMessage = ResponseMessage;
                return ApplicantAccountDetailsContract;

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                throw ;
            }
        }

        private static void IfPasswordFailed(SysXMembershipUser user,string ResponseMessage)
        {
            SecurityManager.FailedPasswordAttemptCount(user.UserName, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempt"]));
            if (!user.IsNull())
            {
                OrganizationUser orgUser = SecurityManager.GetOrganizationUser(user.OrganizationUserId);
                if (user.IsNewPassword)
                   ResponseMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_PWD_RECEIVED_IN_EMAIL);
                else
                    ResponseMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
            }
            else
                ResponseMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
        }
        public static UserType GetUserType(SysXMembershipUser user)
        {
            if (user.IsApplicant.IsNotNull() && user.IsApplicant)
                return UserType.APPLICANT;
            else if (!user.IsApplicant && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                return UserType.CLIENTADMIN;
            else if (!user.IsApplicant && (user.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()))
                return UserType.THIRDPARTYADMIN;
            else if (user.IsSharedUser.IsNotNull() && user.IsSharedUser)
                return UserType.SHAREDUSER;
            else
                return UserType.SUPERADMIN;
        }


    }
}