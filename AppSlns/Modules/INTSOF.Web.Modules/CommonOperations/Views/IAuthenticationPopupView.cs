using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.CommonOperations.Views
{
    public interface IAuthenticationPopupView
    {
        String UserId { get; }
        String AuthenticationCode { get; set; }
        String AuthenticationTitle { get; }
        //Boolean IsVerified { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        String AuthenticationBarCodeImage { get; set; }
        String AuthenticationManualCode { get; set; }
    }
}
