using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.CommonOperations.Views
{
    public class TwoFactorAuthenticationSettingsPresenter : Presenter<ITwoFactorAuthenticationSettingsView>
    {
        public void CheckForTwoFactorAuthentication()
        {
            UserTwoFactorAuthentication userTwoFactorAuthentication = SecurityManager.GetTwofactorAuthenticationForUserID(View.UserId);
            if (!userTwoFactorAuthentication.IsNullOrEmpty())
            {
                View.IsVerified = userTwoFactorAuthentication.UTFA_IsVerified;
                View.Is2FAEnabled = true;
            }
            else
            {
                View.IsVerified = View.Is2FAEnabled = false;
            }
        }
        public Boolean DeleteTwofactorAuthenticationForUserID()
        {
            return SecurityManager.DeleteTwofactorAuthenticationForUserID(View.UserId, View.CurrentLoggedInUserID);
        }
        public void CheckAuthenticationType()
        {
            View.SelectedAuthenticationType = SecurityManager.GetUserAuthenticationUseTypeForUserID(View.UserId);
        }

        //public Boolean UpdateSubscriptionStatusFromAmazon()
        //{
        //    return SMSNotificationManager.UpdateSubscriptionStatusFromAmazon(View.UserOrgID, View.CurrentLoggedInUserID);
        //}
        public void GetUserSMSNotificationData()
        {
            View.OrganisationUserTextMessageSettingData = SMSNotificationManager.GetSMSDataByApplicantId(View.UserOrgID);
        }


    }
}
