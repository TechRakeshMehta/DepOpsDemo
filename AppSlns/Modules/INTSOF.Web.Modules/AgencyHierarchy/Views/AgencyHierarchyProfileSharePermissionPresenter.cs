using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchyProfileSharePermissionPresenter : Presenter<IAgencyHierarchyProfileSharePermissionView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);

            if (!_serviceResponse.Result.IsNullOrEmpty())
            {
                View.lstTenant = _serviceResponse.Result;
            }
            else
            {
                View.lstTenant = new List<TenantDetailContract>();
            }
        }

        public void GetProfileSharePermissionByAgencyHierarchyID()
        {

            View.lstAgencyHierarchyProfileSharePermission = new List<AgencyHierarchyProfileSharePermissionDataContract>();
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();

            //List<AgencyHierarchyProfileSharePermissionDataContract> lstAgencyHierarchyProfileSharePermissionDataContract = new List<AgencyHierarchyProfileSharePermissionDataContract>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = View.AgencyHierarchyID;

            var _response = _agencyHierarchyProxy.GetProfileSharePermissionByAgencyHierarchyID(serviceRequest);

            if (!_response.IsNullOrEmpty())
            {
                View.lstAgencyHierarchyProfileSharePermission = _response.Result;
            }
        }

        public Boolean SaveUpdateProfileSharePermission()
        {
            ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract> serviceRequest = new ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract>();
            serviceRequest.Parameter1 = View.SelectedTenantID;
            serviceRequest.Parameter2 = View.AgencyHierarchyProfileSharePermissionDataContract;

            var _response = _agencyHierarchyProxy.SaveUpdateProfileSharePermission(serviceRequest);

            if (!_response.IsNullOrEmpty())
            {
                return _response.Result;
            }
            else
            {
                return false;
            }
        }

        public Boolean RemoveProfileSharePermission()
        {
            ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract> serviceRequest = new ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract>();

            serviceRequest.Parameter1 = View.SelectedTenantID;
            serviceRequest.Parameter2 = View.AgencyHierarchyProfileSharePermissionDataContract;

            var _response = _agencyHierarchyProxy.RemoveProfileSharePermission(serviceRequest);

            if (!_response.IsNullOrEmpty())
            {
                return _response.Result;
            }
            else
            {
                return false;
            }
        }
    }
}
