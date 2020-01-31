using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
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
    public class ManageAgencyHierarchyTenantAccessPresenter : Presenter<IManageAgencyHierarchyTenantAccessView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        public void GetAllTenant()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _agencyHierarchyProxy.GetTenants(serviceRequest);

            if (!_serviceResponse.Result.IsNullOrEmpty())
            {
                View.lstTenant = _serviceResponse.Result;
            }
            else
            {
                View.lstTenant = new List<TenantDetailContract>();
            }
        }

        public Boolean SaveUpdateAgencyHierarchyTenantAccessMapping()
        {
            ServiceRequest<Int32, List<Int32>, Int32> serviceRequest = new ServiceRequest<Int32, List<Int32>,Int32>();
            serviceRequest.Parameter1 = View.NodeId;
            serviceRequest.Parameter2 = View.lstSelectedTenantIds;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserID;
            ServiceResponse<Boolean> serviceResponse = _agencyHierarchyProxy.SaveUpdateAgencyHierarchyTenantAccessMapping(serviceRequest);
            return serviceResponse.Result;
        }

        public void GetAgencyHierarchyTenantAccessDetails()
        { 
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.NodeId;
            ServiceResponse<List<Int32>> serviceResponse = _agencyHierarchyProxy.GetAgencyHierarchyTenantAccessDetails(serviceRequest);
            View.lstSelectedTenantIds = View.lstSelectedTenantPrevsIds = serviceResponse.Result;
        }
    }
}
