using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.CommonOperations.Views
{
    public interface IManageInstituteHierarchyPackageView
    {
        String CompliancePackageTypeCode { get; set; }

        Int32 TenantID { get; set; }

        Boolean IsCompliancePackage { get; set; }
    }
}
