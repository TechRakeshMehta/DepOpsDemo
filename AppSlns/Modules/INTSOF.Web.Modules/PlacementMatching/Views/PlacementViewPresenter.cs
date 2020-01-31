using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public class PlacementViewPresenter : Presenter<IPlacementView>
    {
        //public void IsAdminLoggedIn()
        //{
        //    View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);
        //}

        public void GetAgencyRootNodes()
        {
            View.LstAgencies = new List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract>();
            Int32  tenantID = (SecurityManager.DefaultTenantID == View.TenantID)?AppConsts.NONE:View.TenantID;
            View.LstAgencies = PlacementMatchingSetupManager.GetAgencyHierarchyRootNodes(tenantID);
        }

        public List<RequestStatusContract> GetRequestStatusBarCounts(Int32 AgencyID)
        {
          return  PlacementMatchingSetupManager.GetRequestStatusBarCount(AgencyID);
        }
    }
}
