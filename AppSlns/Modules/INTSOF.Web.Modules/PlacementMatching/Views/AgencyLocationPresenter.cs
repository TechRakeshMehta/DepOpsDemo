using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public class AgencyLocationPresenter : Presenter<IAgencyLocationView>
    {
        public void GetAgencyLocations()
        {
            View.lstAgencyLocations = new List<AgencyLocationDepartmentContract>();
            View.lstAgencyLocations = PlacementMatchingSetupManager.GetAgencyLocations(View.AgencyRootNodeID);
        }

        public Boolean SaveAgencyLocation()
        {
            return PlacementMatchingSetupManager.SaveAgencyLocation(View.AgencyLocation, View.CurrentLoggedInUserId);
        }

        public Boolean DeleteAgencyLocation()
        {
            return PlacementMatchingSetupManager.DeleteAgencyLocation(View.AgencyLocation.AgencyLocationID, View.CurrentLoggedInUserId);
        }

        public Boolean IsDeptMappedWithLocation(Int32 agencyLocationId)
        {
            return PlacementMatchingSetupManager.IsDeptMappedWithLocation(agencyLocationId);
        }
    }
}
