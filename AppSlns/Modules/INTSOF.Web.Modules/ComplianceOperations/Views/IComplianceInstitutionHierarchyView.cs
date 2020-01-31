using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IComplianceInstitutionHierarchyView
    {

        Int32 TenantId { get; set; }
        String InstitutionNodeId { get; set; }
        String DepProgramMappingId { get; set; }
        String HierarchyLabel { get; set; }
    }
}




