using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Dashboard.Views
{
    public interface IDefaultView
    {
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 LoggedInUserTenantId
        {
            get;
            set;
        }

        String SiteUrl
        {
            get;
        }

        String thirdPartyCode
        {
            get;
        }
    }
}
