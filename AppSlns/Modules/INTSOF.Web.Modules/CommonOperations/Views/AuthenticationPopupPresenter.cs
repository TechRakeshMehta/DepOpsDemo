using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity;
using Google.Authenticator;

namespace CoreWeb.CommonOperations.Views
{
    public class AuthenticationPopupPresenter : Presenter<IAuthenticationPopupView>
    {

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        public Boolean GenerateTwoFactorAuthentication()
        {
            Guid guid = Guid.NewGuid();
            String uniqueUserKey = Convert.ToString(guid).Replace("-", "").Substring(0, 10);
            View.AuthenticationCode = uniqueUserKey;

            Dictionary<String, String> result = new Dictionary<String, String>();
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("Complio", View.AuthenticationTitle, View.AuthenticationCode, 300, 300);
            if (!setupInfo.IsNullOrEmpty())
            {
                View.AuthenticationBarCodeImage = setupInfo.QrCodeSetupImageUrl;
                View.AuthenticationManualCode = setupInfo.ManualEntryKey;
                return true;
            }
            return false;
        }

        public Boolean ValidateTwoFactorPIN(String pin)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(View.AuthenticationCode, pin);
        }

        public void SaveTwoFactorAuthenticationData()
        {
            SecurityManager.SaveTwoFactorAuthenticationData(View.UserId, View.AuthenticationTitle, View.AuthenticationCode, View.CurrentLoggedInUserID, AuthenticationMode.Google_Authenticator.GetStringValue());
        }

        public Boolean verifyTwofactorAuthenticationForUserID()
        {
            return SecurityManager.verifyTwofactorAuthenticationForUserID(View.UserId, View.CurrentLoggedInUserID);
        }

        public Boolean DeleteTwofactorAuthenticationForUserID()
        {
            return SecurityManager.DeleteTwofactorAuthenticationForUserID(View.UserId, View.CurrentLoggedInUserID);
        }

    }
}
