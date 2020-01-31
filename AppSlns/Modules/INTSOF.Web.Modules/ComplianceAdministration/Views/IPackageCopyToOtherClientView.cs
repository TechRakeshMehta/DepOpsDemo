using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IPackageCopyToOtherClientView
    {
        Int32 CurrentLoggedInUserId { get; }
        IPackageCopyToOtherClientView CurrentViewContext { get; }
        Int32 TenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 CompliancePackageID { get; set; }
        String CompliancePackageName { get; set; }
        String ErrorMessage { get; set; }
        String MenuItemValue { get; set; }
        List<Entity.Tenant> Tenants { set; }
        Int32 SelectedTenantId { get; }
        String CopiedPackageId { get; set; }
        String CopiedPackageName { get; set; }
    }
}
