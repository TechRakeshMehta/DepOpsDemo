using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.UI.Contract.PlacementMatching;

namespace CoreWeb.PlacementMatching.Views
{
    public class AgencyLocationDepartmentPresenter : Presenter<IAgencyLocationDepartmentView>
    {
        public void GetAgencyRootNode()
        {
            Dictionary<Int32, String> dicAgencyRootNode = new Dictionary<Int32, String>();
            dicAgencyRootNode = PlacementMatchingSetupManager.GetAgencyRootNode(View.UserId);
            if (!dicAgencyRootNode.IsNullOrEmpty())
            {
                View.AgencyRootNodeID = dicAgencyRootNode.Keys.FirstOrDefault();
                View.AgencyRootNode = dicAgencyRootNode.Values.FirstOrDefault();
            }
        }
        
        public void GetAgencyLocations()
        {
            View.lstAgencyLocations = new List<AgencyLocationDepartmentContract>();
            View.lstAgencyLocations = PlacementMatchingSetupManager.GetAgencyLocations(View.AgencyRootNodeID);
        }
    }
}
