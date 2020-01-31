using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RotationCustomAttributePresenter : Presenter<IRotationCustomAttribute>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ClientContactProxy _clientContactProxy
        {
            get
            {
                return new ClientContactProxy();
            }
        }

        public void GetCustomAttributeList()
        {
            if (View.TenantID > 0)
            {
                ServiceRequest<Int32, String, Int32?> serviceRequest = new ServiceRequest<Int32, String, Int32?>();
                serviceRequest.Parameter1 = View.TenantID;
                serviceRequest.Parameter2 = CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue();
                //serviceRequest.Parameter3 = View.ClinicalRotationId;
                var _serviceResponse = _clinicalRotationProxy.GetCustomAttributeListMapping(serviceRequest);
                View.CustomAttributeList = _serviceResponse.Result;
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
            View.lstTenantDetail = _serviceResponse.Result;
        }
    }
}
