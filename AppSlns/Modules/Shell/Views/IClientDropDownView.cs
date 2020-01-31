using Entity.ClientEntity;
using System;
using System.Collections.Generic;

namespace CoreWeb.Shell.Views
{
    public interface IClientDropDownView
    {
        Int32 CurrentLoggedInUserId { get; }
        List<Tenant> lstTenant { get; set; }
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        Boolean IsDefaultTenant { get; }
    }
}




