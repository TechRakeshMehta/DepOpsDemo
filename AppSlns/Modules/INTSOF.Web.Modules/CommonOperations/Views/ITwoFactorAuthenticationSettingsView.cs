using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.CommonOperations.Views
{
    public interface ITwoFactorAuthenticationSettingsView
    {
        String UserId { get; }
        Boolean IsVerified { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        Boolean Is2FAEnabled { get; set; }
        OrganisationUserTextMessageSetting OrganisationUserTextMessageSettingData { get; set; }
        Int32 UserOrgID { get; }
        Boolean IsApplicant { get; }
        String SelectedAuthenticationType { get; set; }
    }
}
