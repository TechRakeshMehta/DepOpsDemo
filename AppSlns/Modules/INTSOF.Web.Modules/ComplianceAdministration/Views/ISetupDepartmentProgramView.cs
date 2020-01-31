using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SystemSetUp;
namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISetupDepartmentProgramView
    {
        ISetupDepartmentProgramView CurrentViewContext { get; }
        List<InstituteHierarchyTreeDataContract> lstTreeData { set; get; }
        Int32 CurrentUserId { get; }
        Int32 TenantId { get; set; }
        Int32 DepartmentId { get; set; }
        List<Tenant> ListTenants { get; set; }
        Int32 SelectedTenant { get; set; }
        Boolean IsAvailableforOrder { get; set; }
        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
        Boolean IsPackageBundleAvailableforOrder { get; set; }
    }
}




