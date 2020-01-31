using System;
using System.Collections.Generic;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System.Linq;

namespace CoreWeb.ProfileSharing.Views
{
    public class InstructorPreceptorRotationPackageSearchPresenter : Presenter<IInstructorPreceptorRotationPackageSearchView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            //UAT-3596
            List<TenantDetailContract> tenants = GetTenants();
            List<Int32> sharedUserTenantIDs = new List<Int32>(); 
            sharedUserTenantIDs = GetSharedUserTenantIds();
            var tenantList = tenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
            View.lstTenants = tenantList;
           // GetSharedUserAgencies();
            //GetSharedUserAgencies();
            //UAT 3595
            GetSharedUserAgencyHierarchyRootNodes();
        }
        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public List<TenantDetailContract> GetTenants() //UAT-3596
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            return _serviceResponse.Result;
        }

        public List<Int32> GetSharedUserTenantIds() //UAT-3596
        {
            ServiceRequest<List<String>> serviceRequest = new ServiceRequest<List<String>>();
            serviceRequest.Parameter = View.SharedUserTypeCodes;
            var _serviceResponse = _clinicalRotationProxy.GetSharedUserTenantIDs(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        public void GetRotationDetail()
        {
            List<ClinicalRotationDetailContract> rotationData;
            if (View.lstSelectedAgencyIDs.IsNullOrEmpty())
            {
                rotationData = new List<ClinicalRotationDetailContract>();
            }
            else
            {
                ServiceRequest<ClinicalRotationDetailContract> serviceRequest = new
                                        ServiceRequest<ClinicalRotationDetailContract>();
                serviceRequest.Parameter = View.SearchContract;
                var _serviceResponse = _clinicalRotationProxy.GetSharedUserClinicalRotationPackageDetails(serviceRequest);
                rotationData = _serviceResponse.Result;
            }
            View.ClinicalRotationData = rotationData;
        }

        /// <summary>
        /// Method to get shared user agencies for an institution
        /// </summary>
        //public void GetSharedUserAgencies()
        //{
        //    var _serviceResponse = _clinicalRotationProxy.GetSharedUserAgencies();
        //    View.lstAgency = _serviceResponse.Result;
        //}
        public Int32 GetRequirementSubscriptionIdByClinicalRotID(String clinicalRotationID, String tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = Convert.ToInt32(clinicalRotationID);
            serviceRequest.SelectedTenantId = Convert.ToInt32(tenantID);
            var _serviceResponse = _clinicalRotationProxy.GetRequirementSubscriptionIdByClinicalRotID(serviceRequest);
            return _serviceResponse.Result;
        }
        //public void GetRootNodes()
        //{
        //    View.lstAgencyHierarchyRootNodes = new List<AgencyHierarchyContract>();
        //    ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
        //    List<AgencyHierarchyContract> objAgencyHierarchyTree = new List<AgencyHierarchyContract>();
        //    serviceRequest.Parameter1 = AppConsts.NONE;
        //    serviceRequest.Parameter2 = String.Empty;
        //    var _response = _clinicalRotationProxy.GetAgencyHierarchy(serviceRequest);

        //    if (!_response.IsNullOrEmpty())
        //    {
        //        objAgencyHierarchyTree = _response.Result;
        //        View.lstAgencyHierarchyRootNodes = objAgencyHierarchyTree.OrderBy(o => o.DisplayOrder).ThenBy(x => x.NodeID).ThenBy(x => x.ParentNodeID).ToList();
        //    }
        //}

        public void GetSharedUserAgencyHierarchyRootNodes()
        {
            var _serviceResponse = _clinicalRotationProxy.GetSharedUserAgencyHierarchyRootNodes();
            View.lstAgencyHierarchyRootNodes = _serviceResponse.Result;
        }
    }
}
