using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageCompliancePriorityObjectMappingView
    {
        List<Tenant> lstTenants { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        List<CompliancePriorityObjectContract> lstCompObjMappings { get; set; }
        List<CompliancePriorityObjectContract> lstCompPriorityObjects { get; set; }
        Int32 selectedTenantID { get; set; }
        Int32 selectedCompObjID { get; set; }
        Int32 selectedCategoryID { get; set; }
        List<CompliancePriorityObjectContract> lstCategoryItems { get; set; }
        List<CompliancePriorityObjectContract> lstCategory { get; set; }
        List<CompliancePriorityObjectContract> lstItem { get; set; }
        Int32 TenantId { get; set; }
        Boolean IsAdminLoggedIn { get; set; }
    }
}
