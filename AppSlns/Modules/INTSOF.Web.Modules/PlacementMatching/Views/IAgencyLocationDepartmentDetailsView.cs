using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IAgencyLocationDepartmentDetailsView
    {
        Boolean IsLocationClick { get; set; }
        Int32 AgencyRootNodeID { get; set; }
        Int32 AgencyLocationID { get; set; }
    }
}
