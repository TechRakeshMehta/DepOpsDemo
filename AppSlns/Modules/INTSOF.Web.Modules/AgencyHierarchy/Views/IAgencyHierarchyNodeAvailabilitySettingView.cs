using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyNodeAvailabilitySettingView
    {
        IAgencyHierarchyNodeAvailabilitySettingView CurrentViewContext { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 AgencyHierarchyID { get; set; }

        Boolean IsRootNode { get; set; }

        AgencyHierarchySettingContract AgencyHierarchyNodeAvailabilitySettingContract { get; set; }
        
        Int32 SelectedRootNodeID { get; set; }
        
        Boolean IsAgencyHierachyNodeAvailabilitySettingExisted { get; set; }
    }
}
