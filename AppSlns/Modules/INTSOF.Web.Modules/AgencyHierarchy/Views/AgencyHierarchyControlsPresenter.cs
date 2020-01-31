using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchyControlsPresenter : Presenter<IAgencyHierarchyControlsView>
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
        //UAT 2633
        public Boolean CheckForLeafNode()
        {
            
            Boolean returnValue = false;
            if (View.SelectedAgencyHierarchyNodeID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                AgencyNodeMappingContract agencyNodeMappingContract = new AgencyNodeMappingContract();
                serviceRequest.Parameter = View.SelectedAgencyHierarchyNodeID;

                var _response = _agencyHierarchyProxy.CheckForLeafNode(serviceRequest);
                returnValue = _response.Result;
            }
            return returnValue;
        }

        #region UAT-2632:
        public void GetAgencyNodeList()
        {
            var response = _agencyHierarchyProxy.GetAgencyNodeListForMapping();
            var tempListAgencyNode = response.Result;

            Int32 selectedAgencyNodeID = AppConsts.NONE;
            if (View.SelectedAgencyHierarchyNodeID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedAgencyHierarchyNodeID;
                var responseAgencyNode = _agencyHierarchyProxy.GetAgencyNodeIDByAgencyHierarchyID(serviceRequest);
                selectedAgencyNodeID = responseAgencyNode.Result;
            }

            List<AgencyNodeContract> tempAgencyNodeList = new List<AgencyNodeContract>();
            if (View.MappedAgencyNodeIds.IsNullOrEmpty())
            {
                View.MappedAgencyNodeIds = new List<Int32>();
            }

            View.MappedAgencyNodeIds.Add(selectedAgencyNodeID);

            if (!tempListAgencyNode.IsNullOrEmpty())
            {
                tempAgencyNodeList = tempListAgencyNode.Where(cnd => !View.MappedAgencyNodeIds.Contains(cnd.NodeId)).ToList();
            }
            tempAgencyNodeList.Insert(AppConsts.NONE, new AgencyNodeContract { NodeId = AppConsts.NONE, NodeName = "--SELECT--" });
            View.AgencyNodeList = tempAgencyNodeList;
        }

        public Boolean SaveNodeMapping()
        {
            ServiceRequest<Int32, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32, String>();
            serviceRequest.Parameter1 = View.SelectedAgencyHierarchyNodeID;
            serviceRequest.Parameter2 = View.SelectedNodeIdToMap;
            serviceRequest.Parameter3 = View.SelectedNodeTextToMap;
            var response = _agencyHierarchyProxy.SaveNodeMapping(serviceRequest);
            View.NewlyAddedHierarchyId = response.Result.Item2;
            return response.Result.Item1;
        }

        public void IsAgencyMappedOnNode()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedAgencyHierarchyNodeID;
            var response = _agencyHierarchyProxy.IsAgencyMappedWithNode(serviceRequest);
            View.IsAgencyMappedOnNode = response.Result;
        }
        #endregion
        #endregion
    }
}
