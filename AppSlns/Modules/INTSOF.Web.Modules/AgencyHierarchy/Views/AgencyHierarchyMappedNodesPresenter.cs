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
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using Business.RepoManagers;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchyMappedNodesPresenter : Presenter<IAgencyHierarchyMappedNodesView>
    {

        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        public void GetMappedNodes()
        {
            if (View.ParentNodeId > 0)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.ParentNodeId;
                var _serviceResponse = _agencyHierarchyProxy.GetMappedNodesByNodeID(serviceRequest);
                View.lstMappedNodes = _serviceResponse.Result;
            }
            else
            {
                View.lstMappedNodes = new List<AgencyHierarchyDataContract>();
            }
        }

        #region UAT-3237
        public void UpdateNodeDisplayOrder(List<AgencyHierarchyDataContract> lstAgencyHierarchies, Int32? destinationIndex)
        {
            ServiceRequest<List<AgencyHierarchyDataContract>, Int32?, Int32> serviceRequest = new ServiceRequest<List<AgencyHierarchyDataContract>, Int32?, Int32>();
            serviceRequest.Parameter1 = lstAgencyHierarchies;
            serviceRequest.Parameter2 = destinationIndex;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserID;
            var _serviceResponse = _agencyHierarchyProxy.UpdateNodeDisplayOrder(serviceRequest);
        }
        #endregion
        

        public Boolean DeleteNodeMapping()
        {
            if (View.NodeId > 0)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.NodeId;
                var _serviceResponse = _agencyHierarchyProxy.DeleteNodeMapping(serviceRequest);
               
                //UAT-2982: Delete Mappings
                if (_serviceResponse.Result)
                {
                    CallParallelTaskForDeletingAgencyHierarchyMappings();
                }
                return _serviceResponse.Result;
            }
            return false;
        }

        #region UAT-2982
        public void CallParallelTaskForDeletingAgencyHierarchyMappings()
        {
            Int32 agencyHierarchyId = View.NodeId;
            Int32 currentLoggedInUserID = View.CurrentLoggedInUserID;


            Dictionary<String, Object> Data = new Dictionary<String, Object>();
            Data.Add("agencyHierarchyId", agencyHierarchyId);
            Data.Add("currentLoggedInUserID", currentLoggedInUserID);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(DeletingAgencyHierarchyMappings, Data, LoggerService, ExceptiomService);

        }

        public void DeletingAgencyHierarchyMappings(Dictionary<String, Object> data)
        {
            Int32 currentLoggedInUserID = Convert.ToInt32(data.GetValue("currentLoggedInUserID"));
            Int32 agencyHierarchyId = Convert.ToInt32(data.GetValue("agencyHierarchyId"));

            if (agencyHierarchyId > AppConsts.NONE)
            {
                AgencyHierarchyManager.DeletingAgencyHierarchyMappings(agencyHierarchyId, currentLoggedInUserID);
            }
        }
        #endregion
      
    }
}
