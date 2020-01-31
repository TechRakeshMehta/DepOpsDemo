using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchySelectionView
    {
        Int32 AgencyId { get; set; }
        String Label { get; set; }
        Int32 NodeId { get; set; }
        Int32 TenantId { get; set; }

        String AgencyName { get; set; }
        Int32 SelectedRootNodeId { get; set; }
        List<Int32> lstTenantId { get; set; }
    }
}
