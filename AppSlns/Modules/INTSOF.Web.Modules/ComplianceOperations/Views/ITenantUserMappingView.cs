using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface ITenantUserMappingView
    {
        List<Tenant> lstTenants { get; set; }
        Int32 selectedTenantID { get; set; }
        List<Entity.OrganizationUser> lstOrganizationUser { get; set; }
        List<TenantUserMappingContract> lstTenantUserMappings { get; set; }
        Int32 CurrentLoggedInUserID { get; } 
    }
}
