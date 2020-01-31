using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyMultipleSelectionView
    {
        //AgencyhierarchyCollection AgencyHierarchyCollectionAS { get; }

        Dictionary<String, Object> AgencyHierarchyCollection { get; }

        Int32 TenantId { get; set; }

        Int32 CurrentOrgUserId { get; }

        String AgencyHierarchyIds { get; set; }

        Boolean AgencyHierarchyNodeSelection { get; set; }

        Boolean NodeHierarchySelection { get; set; }

        Int32 SelectedRootNodeId { get; set; }
        String SelectedNodeIds { get; set; }
        String SelectedAgecnyIds { get; set; }
        Boolean IsChildBackButtonDisabled { get; set; }
        Boolean AddDisableStyle { set; }

    }
}
