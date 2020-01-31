using System;
using System.Collections.Generic;
using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.CommonControls;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IAccountLinkingView
    {
        List<LookupContract> ExistingUsersList
        {
            set;
        }
        AccountLinkingProfileContract accountLinkingProfileContract { get; set; }
    }

}




