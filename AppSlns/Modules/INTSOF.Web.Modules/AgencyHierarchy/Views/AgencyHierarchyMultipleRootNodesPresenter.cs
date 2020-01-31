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
    public class AgencyHierarchyMultipleRootNodesPresenter : Presenter<IAgencyHierarchyMultipleRootNodesView>
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
            serviceRequest.Parameter2 = View.AgencyHierarchyNodeIds.IsNull() ? String.Empty : View.AgencyHierarchyNodeIds;
            if (View.TenantId > AppConsts.NONE && View.AgencyHierarchyNodeIds.IsNullOrEmpty())
            {
                View.lstTreeData = new List<AgencyHierarchyContract>();
            }
            else
            {
                var _response = _agencyHierarchyProxy.GetAgencyHierarchy(serviceRequest);
                objAgencyHierarchyTree = _response.Result.OrderBy(o=>o.DisplayOrder).ToList(); //UAT-3237
                View.lstTreeData = objAgencyHierarchyTree;
            }
        }
        public void GetAgencyHierarchyByRootNodeIds()
        {
            ServiceRequest<Int32, String, String> serviceRequest = new ServiceRequest<Int32, String, String>();
            List<AgencyHierarchyContract> objAgencyHierarchyTree = new List<AgencyHierarchyContract>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.RootNodeIds;
            serviceRequest.Parameter3 = View.AgencyHierarchyNodeIds.IsNull() ? String.Empty : View.AgencyHierarchyNodeIds;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyByRootNodeIds(serviceRequest);
            objAgencyHierarchyTree = _response.Result;
            View.lstChildTreeData = objAgencyHierarchyTree.OrderBy(x => x.DisplayOrder).ThenBy(x => x.NodeID).ThenBy(x => x.ParentNodeID).ToList(); //UAT-3237
        }

        public void GetAgencyDetailByMultipleNodeIds()
        {
            ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> serviceRequest = new ServiceRequest<Int32, AgencyHierarchMultiSelectParameter>();
            AgencyHierarchMultiSelectParameter parm = new AgencyHierarchMultiSelectParameter();
            parm.AgencyIds = View.AgencyIds.IsNull() ? String.Empty : View.AgencyIds;
            parm.NodeIds = View.NodeIds.IsNull() ? String.Empty : View.NodeIds;
            parm.HierarchySelectionType = (View.NodeHierarchySelection.IsNotNull() && View.NodeHierarchySelection == true) ? "NHS" : (View.AgencyHierarchyNodeSelection.IsNotNull() && View.AgencyHierarchyNodeSelection == true) ? "AHNS" : String.Empty;
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = parm;
            var _response = _agencyHierarchyProxy.GetAgencyDetailByMultipleNodeID(serviceRequest);
            View.XMLResult = _response.Result;
        }
        public List<Int32> GetAgencyHierarchyIdsByOrgUserID()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.CurrentOrgUserID;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyIdsByOrgUserID(serviceRequest);
            return _response.Result;
        }

        //UAT-2647: Method to get the Hierarchy Mapped to Tenant//
        public List<Int32> GetAgencyHierarchyIdsByTenantID()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.TenantId;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyIdsByTenantID(serviceRequest);
            return _response.Result;
        }
        public void GetAgencyHiearchyIdsByDeptProgMappingID()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.SelectedInstitutionNodeId;
            var _response = _agencyHierarchyProxy.GetAgencyHiearchyIdsByDeptProgMappingID(serviceRequest);

            if (!_response.IsNullOrEmpty() && !_response.Result.IsNullOrEmpty() && _response.Result.Count > 0)
                View.AgencyHierarchyNodeIds = String.Join(",", _response.Result);
            else
                View.AgencyHierarchyNodeIds = string.Empty;
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

        //UAT-4443
        public void IsClientAdmin()
        {
            if (IntegrityManager.IsClientAdmin(View.LoggedInUserTenantId))
            {
                View.IsClientAdmin = true;
            }
        }
    }
}
