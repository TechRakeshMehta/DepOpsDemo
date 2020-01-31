using Entity.ClientEntity;
using System;
using System.Collections.Generic;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IPackageCopyToLowerNode
    {
        Int32 CurrentLoggedInUserId { get; } 
        Int32 TenantId { get; set; } 
        Int32 CompliancePackageID { get; set; }
        String CompliancePackageName { get; set; }
        String ErrorMessage { get; set; }
        Int32 NodeID { get; set; }
        Int32 SelectedNodeID { get; set; }
        List<DeptProgramMapping> lstDepartmentProgramMapping {get;set;}
    }
}
