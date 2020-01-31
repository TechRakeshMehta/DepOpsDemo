#region Namespaces

#region SystemDefined
using Microsoft.Practices.ObjectBuilder;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System;
using System.Collections.Generic;
using System.Text;
#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
#endregion

#endregion

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchyListPresenter : Presenter<IAgencyHierarchyListView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetTreeData()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            List<AgencyHierarchyContract> objAgencyHierarchyTree = new List<AgencyHierarchyContract>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.AgencyHierarchyNodeIdsToFilter.IsNull() ? String.Empty : View.AgencyHierarchyNodeIdsToFilter;
            if (View.TenantId > AppConsts.NONE && View.AgencyHierarchyNodeIdsToFilter.IsNullOrEmpty())
            {
                View.lstTreeData = new List<AgencyHierarchyContract>();
            }
            else
            {
                var _response = _agencyHierarchyProxy.GetAgencyHierarchy(serviceRequest);
                objAgencyHierarchyTree = _response.Result.OrderBy(o => o.DisplayOrder).ToList(); //UAT-3237
                View.lstTreeData = objAgencyHierarchyTree;
            }
        }

        public void GetTreeDataByRootNodeID()
        {
            ServiceRequest<Int32, AgencyHierarchPopUpParameter> serviceRequest = new ServiceRequest<Int32, AgencyHierarchPopUpParameter>();
            List<AgencyHierarchyContract> treeDataByRootNodeId = new List<AgencyHierarchyContract>();
            AgencyHierarchPopUpParameter parameters = new AgencyHierarchPopUpParameter();
            serviceRequest.Parameter1 = View.TenantId;
            parameters.RootNodeId = View.RootNodeID;
            parameters.AgencyHierarchyNodeIds = View.IsInstitutionHierarchyFilterApplied == true ? View.AgencyHierarchyNodeIdsToFilter.IsNull() ? String.Empty : View.AgencyHierarchyNodeIdsToFilter : String.Empty;
            serviceRequest.Parameter2 = parameters;
            var _response = _agencyHierarchyProxy.GetTreeDataByRootNodeIDForPopUp(serviceRequest);
            treeDataByRootNodeId = _response.Result;
            View.lstChildTreeData = treeDataByRootNodeId.OrderBy(x => x.DisplayOrder).ThenBy(x => x.NodeID).ThenBy(x => x.ParentNodeID).ToList(); //UAT-3237
        }
        public void GetAgencyDetailByNodeId()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.NodeID;
            var _response = _agencyHierarchyProxy.GetAgencyDetailByNodeId(serviceRequest);
            View.agencyDetial = _response.Result;
        }

        public void GetAgencyHiearchyIdsByDeptProgMappingID()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.SelectedInstitutionNodeId;
            var _response = _agencyHierarchyProxy.GetAgencyHiearchyIdsByDeptProgMappingID(serviceRequest);

            if (!_response.IsNullOrEmpty() && !_response.Result.IsNullOrEmpty() && _response.Result.Count > 0)
                View.AgencyHierarchyNodeIdsToFilter = String.Join(",", _response.Result);
            else
                View.AgencyHierarchyNodeIdsToFilter = string.Empty;
        }

        public void GetAgencyHiearchyIdsByTenantID()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.TenantId;
            var _response = _agencyHierarchyProxy.GetAgencyHiearchyIdsByTenantID(serviceRequest);

            if (!_response.IsNullOrEmpty() && !_response.Result.IsNullOrEmpty())
                View.AgencyHierarchyNodeIdsToFilter = _response.Result;
            else
                View.AgencyHierarchyNodeIdsToFilter = string.Empty;
        }

        //UAT-3245
        public void GetAgencyHierarchyIdsByLstTenantIDs()
        {
            ServiceRequest<List<Int32>> serviceRequest = new ServiceRequest<List<Int32>>();
            serviceRequest.Parameter = View.lstTenantIds;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyIdsByLstTenantIDs(serviceRequest);

            if (!_response.IsNullOrEmpty() && !_response.Result.IsNullOrEmpty())
                View.AgencyHierarchyNodeIdsToFilter = String.Join(",", _response.Result);
            else
                View.AgencyHierarchyNodeIdsToFilter = String.Empty;
        }

        //UAT-3952
        public void BindPageControls()
        {
            List<String> _lstCodeForColumnConfig = new List<String>();
            _lstCodeForColumnConfig.Add((Screen.trlAgencyHierarchy).GetStringValue());
            var lstScreenColumnData = SecurityManager.GetScreenColumns(_lstCodeForColumnConfig, View.CurrentUserId);
            View.isHierarchyCollapsed = !lstScreenColumnData.Select(sel => sel.IsColumnVisible).FirstOrDefault();
            View.screenColumnID = lstScreenColumnData.Select(sel => sel.ColumnID).FirstOrDefault();
        }

        public Boolean SaveUserScreenColumnMapping(Dictionary<Int32, Boolean> columnVisibility)
        {
            return SecurityManager.SaveUserScreenColumnMapping(columnVisibility, View.CurrentUserId, View.CurrentUserId);
        }
    }
}
