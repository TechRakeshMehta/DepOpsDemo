using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyView
    {
        IAgencyHierarchyView CurrentViewContext { get; }

        Int32 CurrentUserId { get; }

        Int32 TenantId { get; set; }

        List<AgencyHierarchyContract> lstAgencyHierarchyRootNodes { set; get; }

        List<AgencyHierarchyContract> lstAgencyHierarchyTreeData { set; get; }

        Int32 SelectedRootNodeID { get; set; }
    }
}
