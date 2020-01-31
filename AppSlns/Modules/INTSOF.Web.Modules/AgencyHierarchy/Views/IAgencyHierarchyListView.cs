using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyListView
    {
        IAgencyHierarchyListView CurrentViewContext { get; }
        List<AgencyHierarchyContract> lstTreeData { set; get; }
        List<AgencyHierarchyContract> lstChildTreeData { set; get; }
        Int32 CurrentUserId { get; }
        AgencyHierarchyContract agencyDetial { get; set; }
        Int32 TenantId { get; }
        Int32 NodeID { get; set; }
        Int32 RootNodeID { get; set; }
        Int32 AgencyId { get; set; }
        String HierarchyLabel { get; set; }
        String AgencyHierarchyNodeIds { get; set; }
        String SelectedAgencyId { get; set; }

        String SelectedRootNodeId
        {
            get;
            set;
        }

        String SelectedInstitutionNodeId { get; set; }

        String AgencyHierarchyNodeIdsToFilter { get; set; }
        Boolean IsInstitutionHierarchyFilterApplied { get; set; }
        List<Int32> lstTenantIds { get; set; } //UAT-3245

        //UAT-3952
        Boolean isHierarchyCollapsed { get; set; }
        Int32 screenColumnID { get; set; }
    }
}
