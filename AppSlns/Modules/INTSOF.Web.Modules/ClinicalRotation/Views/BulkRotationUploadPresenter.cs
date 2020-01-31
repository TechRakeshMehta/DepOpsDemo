using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public class BulkRotationUploadPresenter : Presenter<IBulkRotationUploadView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantID;
            }
        }

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.LstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }


        public Boolean BulkRotationUpload(List<BatchRotationUploadContract> RotationDetails, String fileName)
        {
            ServiceRequest<List<BatchRotationUploadContract>, String, Int32, Int32> serviceRequest = new ServiceRequest<List<BatchRotationUploadContract>, String, Int32, Int32>();
            serviceRequest.Parameter1 = RotationDetails;
            serviceRequest.Parameter2 = fileName;
            serviceRequest.Parameter3 = View.SelectedTenantId;
            serviceRequest.Parameter4 = View.CurrentLoggedInUserId;
            var _serviceResponse = _clinicalRotationProxy.SaveBatchRotationUploadDetails(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetBatchRotationList(BatchRotationUploadContract SearchBatchRotationUploadContract)
        {

            if (View.SelectedTenantId == 0)
            {
                List<BatchRotationUploadContract> RotationDataList = new List<BatchRotationUploadContract>();
                View.RotationDataList = RotationDataList;
            }
            else
            {
                ServiceRequest<Int32, BatchRotationUploadContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<Int32, BatchRotationUploadContract, CustomPagingArgsContract>();
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = SearchBatchRotationUploadContract;
                serviceRequest.Parameter3 = View.GridCustomPaging;
                var _serviceResponse = _clinicalRotationProxy.GetBatchRotationList(serviceRequest);
                View.RotationDataList = _serviceResponse.Result;
            }
        }


        public List<String> GetAgencyHierarchyAgencyList(List<Tuple<Int32, Int32>> AgencyHierarchyIdAgencys)
        {
            ServiceRequest<List<Tuple<Int32, Int32>>> serviceRequest = new ServiceRequest<List<Tuple<Int32, Int32>>>();
            serviceRequest.Parameter = AgencyHierarchyIdAgencys;
            var _serviceResponse = _clinicalRotationProxy.GetAgencyHierarchyAgencyList(serviceRequest);
            return _serviceResponse.Result;
        }
    }
}
