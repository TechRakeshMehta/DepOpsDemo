using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IInstitutionNodeHierarchyListView
    {
        IInstitutionNodeHierarchyListView CurrentViewContext { get; }
        List<GetDepartmentTree> lstTreeData { get; set; }
        List<InstituteHierarchyNodesList> lstTreeHierarchyData { get; set; } //UAT-3369
        List<GetInstituteHierarchyOrderTree> lstOrderTreeData { get; set; }
        Int32 CurrentUserId { get; }
        String InstitutionNodeId { get; set; }
        String HierarchyLabel { get; set; }
        Int32 SelectedTenant { get; set; }
        Int32 DepartmentPrgMappingId { get; set; }
        String DelemittedDepartmentPrgMappingIds { get; set; }
        String ScreenName { get; set; }
        string ScreenNameForPermission { get; set; }
        string IsRequestFromAddRotationScreen { get; set; }

        //UAT-3952
        Boolean isHierarchyCollapsed { get; set; }
        Int32 screenColumnID { get; set; }
    }
}