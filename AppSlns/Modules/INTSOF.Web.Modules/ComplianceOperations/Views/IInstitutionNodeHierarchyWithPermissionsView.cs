using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IInstitutionNodeHierarchyWithPermissionsView
    {
        IInstitutionNodeHierarchyWithPermissionsView CurrentViewContext { get; }
        List<GetDepartmentTree> lstTreeData { get; set; }
        Int32 CurrentUserId { get; }
        String InstitutionNodeId { get; set; }
        String HierarchyLabel { get; set; }
        Int32 SelectedTenant { get; set; }
        Int32 DepartmentPrgMappingId { get; set; }
        String DelemittedDepartmentPrgMappingIds { get; set; }
        String ScreenName { get; set; }

        //UAT-3952
        Boolean isHierarchyCollapsed { get; set; }
        Int32 screenColumnID { get; set; }
    }
}
