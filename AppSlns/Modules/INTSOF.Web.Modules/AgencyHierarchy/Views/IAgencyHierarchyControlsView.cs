using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyControlsView
    {
        IAgencyHierarchyControlsView CurrentViewContext { get; }

        Int32 CurrentUserId { get; }

        Int32 TenantId { get; set; }

        Int32 SelectedAgencyHierarchyNodeID { get; }

        List<Int32> MappedAgencyNodeIds
        {
            get;
            set;

        }

        List<AgencyNodeContract> AgencyNodeList
        {
            get;
            set;

        }

        Int32 SelectedNodeIdToMap
        {
            get;
        }

        String SelectedNodeTextToMap
        {
            get;
        }

        Int32 SelectedNodeId_Global
        {
            get;
            set;
        }

        Boolean IsAgencyMappedOnNode { get; set; }
        Int32 NewlyAddedHierarchyId { get; set; }
        
    }
}
