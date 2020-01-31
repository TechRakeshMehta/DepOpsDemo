using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IAgencyLocationDepartmentView
    {
        Guid UserId { get; }
        Int32 AgencyRootNodeID { get; set; }
        String AgencyRootNode { get; set; }
        List<AgencyLocationDepartmentContract> lstAgencyLocations  { get; set; }
        Int32 AgencyLocationID { get; set; }
    }
}
