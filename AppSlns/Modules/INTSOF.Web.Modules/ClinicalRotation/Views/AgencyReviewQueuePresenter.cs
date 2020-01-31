using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public class AgencyReviewQueuePresenter : Presenter<IAgencyReviewQueueView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        /// <summary>
        /// Bind the list of Tenants
        /// </summary>
        public void GetTenants()
        {
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = true;
            serviceRequest.Parameter2 = TenantType.Institution.GetStringValue(); ;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            View.lstTenants = _serviceResponse.Result;
        }

        /// <summary>
        /// Bind the list of Tenants
        /// </summary>
        public void GetAgencySearchStatus()
        {
            var _response = _clinicalRotationProxy.GetAgencySearchStatus();
            View.lstAgencySearchStatus = _response.Result;
        }

        public void GetAgencies()
        {
            if (View.lstSelectedTenantIds.IsNullOrEmpty())
            {
                View.lstAgencies = new List<AgencyReviewQueueContract>();
            }
            else
            {
                ServiceRequest<String, String, CustomPagingArgsContract> serviceRequest = new ServiceRequest<String, String, CustomPagingArgsContract>();
                serviceRequest.Parameter1 = View.lstSelectedSrchCodes;
                serviceRequest.Parameter2 = View.lstSelectedTenantIds;
                serviceRequest.Parameter3 = View.GridCustomPaging;

                var _response = _clinicalRotationProxy.GetAgencyQueueData(serviceRequest);
                View.lstAgencies = _response.Result;

                if (!View.lstAgencies.IsNullOrEmpty())
                {
                    if (View.lstAgencies[0].TotalCount > 0)
                    {
                        View.VirtualRecordCount = View.lstAgencies[0].TotalCount;
                    }
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = 0;
                    View.CurrentPageIndex = 1;
                }
            }
        }

        /// <summary>
        /// Returns the list of the 'lkpAgencySearchStatus'
        /// </summary>
        public void SetAgencySearchStatus()
        {
            ServiceRequest<List<Int32>, String> serviceRequest = new ServiceRequest<List<Int32>, String>();
            serviceRequest.Parameter1 = View.lstSelectedAgencyIds;
            serviceRequest.Parameter2 = View.StatusCode;

            var _response = _clinicalRotationProxy.SetAgencySearchStatus(serviceRequest);
        }
    }
}
