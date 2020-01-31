using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IInstitutionHierarchyListView
    {
        IInstitutionHierarchyListView CurrentViewContext { get; }
        //List<GetDepartmentTree> lstTreeData { set; get; }
        List<InstituteHierarchyNodesList> lstTreeHierarchyData { set; get; } //UAT-3369 
        Int32 CurrentUserId { get; }
        Int32 InstitutionNodeId { get; set; }
        String HierarchyLabel { get; set; }
        Int32 SelectedTenant { get; set; }
        Int32 DepartmentPrgMappingId
        {
            get;
            set;
        }

        //UAT-3952
        Boolean isHierarchyCollapsed { get; set; }
        Int32 screenColumnID { get; set; }
    }
}




