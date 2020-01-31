using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyMultipleRootNodesView
    {

        List<AgencyHierarchyContract> lstTreeData { set; get; }
        Int32 TenantId { get; }
        String RootNodeIds { set; get; }
        String NodeIds { set; get; }
        List<AgencyHierarchyContract> lstChildTreeData { set; get; }
        List<AgencyHierarchyContract> AgencyList { set; get; }
        String XMLResult { set; get; }
        String AgencyHierarchyNodeIds { get; set; }

        Boolean AgencyHierarchyNodeSelection { get; set; }
        Boolean NodeHierarchySelection { get; set; }
        Int32 CurrentOrgUserID { get; set; }
        String AgencyIds { get; set; }
        String SelectedInstitutionNodeId { get; set; }

        //UAT-3952
        Boolean isHierarchyCollapsed { get; set; }
        Int32 screenColumnID { get; set; }
        Int32 CurrentUserId { get; }

        //UAT-4597
        Boolean IsBackButtonDisabled { get; set; }

        //UAT-4443
        Boolean IsClientAdmin { get; set; }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        Int32 LoggedInUserTenantId
        {
            get;
            set;
        }
    }
}
