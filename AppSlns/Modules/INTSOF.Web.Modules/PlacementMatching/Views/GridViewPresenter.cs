using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace CoreWeb.PlacementMatching.Views
{
    public class GridViewPresenter : Presenter<IGridView>
    {

        public void GetSearchRequestData(Int32 AgencyHierarchyID, String StatusCode)
        {
            try
            {
                PlacementSearchContract request = new PlacementSearchContract();
                request.AgencyHierarchyID = AgencyHierarchyID;
                request.StatusCode = StatusCode;
                View.lstPlacementMaching = new List<RequestDetailContract>();
                View.lstPlacementMaching = PlacementMatchingSetupManager.GetPlacementRequests(request);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
