using Business.RepoManagers;
using Entity;
using Google.Authenticator;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.CommonOperations.Views
{
    public class GoogleAuthenticatorPresenter : Presenter<IGoogleAuthenticatorView>
    {
        public Boolean ValidateTwoFactorPIN(String pin)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(View.AuthenticationCode, pin);
        }

        public void GetTwofactorAuthenticationForUserID()
        {
            UserTwoFactorAuthentication userTwoFactorAuthentication = SecurityManager.GetTwofactorAuthenticationForUserID(View.UserId);
            if (!userTwoFactorAuthentication.IsNullOrEmpty())
            {
                View.AuthenticationCode = userTwoFactorAuthentication.UTFA_AuthenticationData;
            }
        }
        public String SendOTP()
        {
            Guid guid = Guid.NewGuid();
            String uniqueUserKey = Convert.ToString(guid).Replace("-", "").Substring(0, 4).ToUpper();

            //SEND SMS
            Dictionary<String, object> dictSMSData = new Dictionary<String, object>();
            dictSMSData.Add(EmailFieldConstants.OTP, uniqueUserKey);

            CommunicationMockUpData mockSMSData = new CommunicationMockUpData();
            mockSMSData.UserName = View.UserFullName;
            mockSMSData.EmailID = View.UserPrimaryEmailAddress;
            mockSMSData.ReceiverOrganizationUserID = View.OrgUserId;
            CommunicationManager.SaveDataForSMSNotification(CommunicationSubEvents.NOTIFICATION_FOR_LOGIN_VIA_2FA_SMS, mockSMSData,
                                                            dictSMSData, View.TenantId, AppConsts.NONE);


            return uniqueUserKey;
        }

        #region Globalization

        public int GetWebsiteTenantId(string websiteUrl)
        {
            return WebSiteManager.GetWebsiteTenantId(websiteUrl);
        }

        public bool IsLocationServiceTenant(int tenantId)
        {
            return SecurityManager.IsLocationServiceTenant(tenantId);
        }

        #endregion
    }
}
