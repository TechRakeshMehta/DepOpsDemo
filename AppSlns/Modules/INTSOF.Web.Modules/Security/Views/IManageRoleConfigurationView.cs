using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.UI.Contract.SysXSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Security.Views
{
    public interface IManageRoleConfigurationView
    {
        Int32 CurrentLoggedInUserId { get; }
        List<Entity.RoleDetail> lstRoles { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<RoleConfigurationContract> lstRoleConfig { get; set; }
        RoleConfigurationContract RolePreferredTenantSetting { get; set; }
    }
}
