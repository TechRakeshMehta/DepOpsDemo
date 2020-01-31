using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchyPresenter : Presenter<IAgencyHierarchyView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        #region Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetRootNodes()
        {
            View.lstAgencyHierarchyRootNodes = new List<AgencyHierarchyContract>();
            ServiceRequest<Int32,String> serviceRequest = new ServiceRequest<Int32, String>();
            List<AgencyHierarchyContract> objAgencyHierarchyTree = new List<AgencyHierarchyContract>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = String.Empty;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchy(serviceRequest);

            if (!_response.IsNullOrEmpty())
            {
                objAgencyHierarchyTree = _response.Result;
                View.lstAgencyHierarchyRootNodes = objAgencyHierarchyTree.OrderBy(o => o.DisplayOrder).ThenBy(x => x.NodeID).ThenBy(x => x.ParentNodeID).ToList();
            }
        }

        public void GetTreeDataByRootNodeID()
        {
            View.lstAgencyHierarchyTreeData = new List<AgencyHierarchyContract>();

            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            List<AgencyHierarchyContract> treeDataByRootNodeId = new List<AgencyHierarchyContract>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.SelectedRootNodeID;

            var response = _agencyHierarchyProxy.GetTreeDataByRootNodeID(serviceRequest);

            if (!response.Result.IsNullOrEmpty())
            {
                View.lstAgencyHierarchyTreeData = response.Result.OrderBy(o => o.DisplayOrder).ThenBy(x => x.NodeID).ThenBy(x => x.ParentNodeID).ToList();
            }
        }

        #endregion
    }
}
