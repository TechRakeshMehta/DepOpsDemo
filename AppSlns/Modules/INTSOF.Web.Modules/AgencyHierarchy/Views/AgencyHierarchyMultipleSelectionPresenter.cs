using System;
using System.Collections.Generic;
using System.Text;
#region Namespaces

#region SystemDefined
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;


#endregion

#endregion

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchyMultipleSelectionPresenter : Presenter<IAgencyHierarchyMultipleSelectionView>
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
        public List<Int32> GetAgencyHierarchyIdsByOrgUserID()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.CurrentOrgUserId;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyIdsByOrgUserID(serviceRequest);
            return _response.Result;
        }

        public String GetAgencyHierarchyParent()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.SelectedNodeIds;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyParent(serviceRequest);
            return _response.Result;
        }
        public String GetAgencyHierarchyLabelForMultipleSelection()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.SelectedNodeIds;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyLabelForMultipleSelection(serviceRequest);
            return _response.Result;
        }

        public String GetAgencyDetailByMultipleNodeIds(String NodeIds, String AgencyIds, String HierarchySelectionType)
        {
            ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> serviceRequest = new ServiceRequest<Int32, AgencyHierarchMultiSelectParameter>();
            AgencyHierarchMultiSelectParameter parm = new AgencyHierarchMultiSelectParameter();
            parm.AgencyIds = AgencyIds.IsNull() ? String.Empty : AgencyIds;
            parm.NodeIds = NodeIds.IsNull() ? String.Empty : NodeIds;
            parm.HierarchySelectionType = HierarchySelectionType.IsNull() ? "NHS" : HierarchySelectionType;
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = parm;
            var _response = _agencyHierarchyProxy.GetAgencyDetailByMultipleNodeID(serviceRequest);
            return _response.Result;
        }

        /// <summary>
        /// It will return AgencyHierarchyID and AgencyName Combination base on parent  node id or any node id
        /// Ex : "24 - AgencyName" -- UAT-2926
        /// </summary>
        /// <param name="NodeIds"></param>
        /// <returns></returns>
        public List<String> GetAgencyHierarchyAgencyByMultipleNodeIds(String NodeIds)
        {
            ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> serviceRequest = new ServiceRequest<Int32, AgencyHierarchMultiSelectParameter>();
            AgencyHierarchMultiSelectParameter parm = new AgencyHierarchMultiSelectParameter();
            parm.NodeIds = NodeIds.IsNull() ? String.Empty : NodeIds;
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = parm;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyAgencyByMultipleNodeIds(serviceRequest);
            return _response.Result;
        }
     
        
    }
}
