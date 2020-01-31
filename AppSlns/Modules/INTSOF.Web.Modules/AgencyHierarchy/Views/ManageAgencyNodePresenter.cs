using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using Entity.SharedDataEntity;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class ManageAgencyNodePresenter : Presenter<IManageAgencyNodeView>
    {

        #region UAT-2629

        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        #region Methods

        #region Public Methods
        /// <summary>
        /// To check whether the admin is logged in or not.
        /// </summary>
        /// <returns></returns>
        public Boolean IsAdminLoggedIn()
        {
            Int32 currentUserTenantId = GetTenantId();
            //Checked if logged user is admin or not.
            if (currentUserTenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                View.SelectedTenantID = currentUserTenantId;
                return false;
            }
        }

        /// <summary>
        /// Get the tenantId.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// To check whether the nodeID exists or not.
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public Boolean IsNodeExist(String nodeName, Int32? nodeId = null)
        {
            //ServiceResponse<List<AgencyNodeContract>> _response = _agencyHierarchyProxy.GetAgencyNodeList();
            //List<AgencyNodeContract> listNode = AgencyHierarchyManager.GetAgencyNodeList();
            //if (nodeId != null)
            //{
            //    if (listNode.Any(x => x.NodeName.ToLower() == nodeName.ToLower() && x.NodeId != nodeId))
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            //else
            //{
            //    if (listNode.Any(x => x.NodeName.ToLower() == nodeName.ToLower() && !x.IsDeleted))
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            return _agencyHierarchyProxy.IsNodeExist(nodeName, nodeId);
        }

        /// <summary>
        /// To get the list of nodes to bind the grid.
        /// </summary>
        public void GetNodeList()
        {
            View.lstGetNodeList = new List<AgencyNodeContract>();

            ServiceRequest<CustomPagingArgsContract, String, String> serviceRequest = new ServiceRequest<CustomPagingArgsContract, String, String>();
            serviceRequest.Parameter1 = View.GridCustomPaging;
            serviceRequest.Parameter2 = View.AgencyNodeName;
            serviceRequest.Parameter3 = View.Description;
            ServiceResponse<List<AgencyNodeContract>> _response = _agencyHierarchyProxy.GetAgencyNodeRootList(serviceRequest);
            if (!_response.Result.IsNullOrEmpty())
            {
                View.lstGetNodeList = _response.Result;
            }
            //UAT-3652
            if (View.lstGetNodeList.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = View.lstGetNodeList[0].TotalRecordCount;
            }
        }

        /// <summary>
        /// To save the new node details.
        /// </summary>
        /// <returns></returns>
        public Boolean SaveNodeDetail()
        {
            ServiceRequest<AgencyNodeContract> serviceRequest = new ServiceRequest<AgencyNodeContract>();
            if (!IsNodeExist(View.NodeContract.NodeName, View.NodeContract.NodeId))
            {
                serviceRequest.Parameter = View.NodeContract;
                ServiceResponse<Boolean> _response = _agencyHierarchyProxy.SaveNodeDetail(serviceRequest);
                if (_response.Result)
                {
                    View.SuccessMessage = "Node saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }

            else
            {
                View.InfoMessage = "Node already exists.";
                return false;
            }
        }

        /// <summary>
        /// To update the existing node details.
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateNodeDetail()
        {
            ServiceRequest<AgencyNodeContract> serviceRequest = new ServiceRequest<AgencyNodeContract>();
            if (!IsNodeExist(View.NodeContract.NodeName, View.NodeContract.NodeId))
            {
                serviceRequest.Parameter = View.NodeContract;
                ServiceResponse<Boolean> _response = _agencyHierarchyProxy.SaveNodeDetail(serviceRequest);
                if (_response.Result)
                {
                    View.SuccessMessage = "Node saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }

            else
            {
                View.InfoMessage = "Node already exists.";
                return false;
            }

        }

        /// <summary>
        /// To delete the node. Changing the isDeleted to true.
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteNode()
        {
            if (!AgencyHierarchyManager.IsNodeMapped(View.NodeContract.NodeId))
            {
                ServiceRequest<AgencyNodeContract> serviceRequest = new ServiceRequest<AgencyNodeContract>();
                serviceRequest.Parameter = View.NodeContract;
                ServiceResponse<Boolean> _response = _agencyHierarchyProxy.SaveNodeDetail(serviceRequest);
                if (_response.Result)
                {
                    View.SuccessMessage = "Node deleted successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }

            else
            {
                View.InfoMessage = "Node is currently in use.";
                return false;
            }
        }

        #endregion

        #endregion

        #endregion





    }
}
