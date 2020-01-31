#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.WebSite.Views
{
    public interface IClientSettingsView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        Boolean ShowClientDropDown
        {
            get;
            set;
        }

        List<Tenant> lstTenant
        {
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 SettingId
        {
            get;
            set;
        }

        Int32 SettingValue
        {
            get;
            set;
        }

        Int32 ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// Sets or gets the varchar Setting Value.
        /// </summary>
        String StringSettingValue
        {
            get;
            set;
        }

        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
        Boolean IsLocationServiceTenant { get; set; }
    }
}




