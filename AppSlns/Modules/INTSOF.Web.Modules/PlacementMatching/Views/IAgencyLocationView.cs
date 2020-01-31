using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IAgencyLocationView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 AgencyRootNodeID { get; set; }
        List<AgencyLocationDepartmentContract> lstAgencyLocations { get; set; }
        AgencyLocationDepartmentContract AgencyLocation { get; set; }
    }
}
