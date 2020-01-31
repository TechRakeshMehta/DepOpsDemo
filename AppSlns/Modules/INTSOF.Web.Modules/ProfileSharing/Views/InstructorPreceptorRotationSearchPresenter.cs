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
using System.Linq;

namespace CoreWeb.ProfileSharing.Views
{
    public class InstructorPreceptorRotationSearchPresenter : Presenter<IInstructorPreceptorRotationSearchView>
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
            List<TenantDetailContract> tenants = GetTenants();
            List<Int32> sharedUserTenantIDs = new List<Int32>();

            //if ((View.SharedUserTypeCodes.Contains(OrganizationUserType.Instructor.GetStringValue())
            //    || View.SharedUserTypeCodes.Contains(OrganizationUserType.Preceptor.GetStringValue())))
            //{
            sharedUserTenantIDs = GetSharedUserTenantIds();
            var tenantList = tenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
            View.lstTenants = tenantList;
            var tenantID = tenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
            GetRotationReviewStatus(tenantID);
            GetRotationStatus(tenantID);
            GetSharedUserAgencyHierarchyRootNodes();
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
            if (View.lstSelectedTenants.IsNullOrEmpty())
            {
                rotationData = new List<ClinicalRotationDetailContract>();
            }
            else
            {
                ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract> serviceRequest = new
                                        ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract>();
                serviceRequest.Parameter1 = View.lstSelectedTenants;
                View.SearchContract.IsInstructor = true;
                serviceRequest.Parameter2 = View.SearchContract;
                var _serviceResponse = _clinicalRotationProxy.GetSharedUserClinicalRotationDetails(serviceRequest);
                rotationData = _serviceResponse.Result;
            }
            View.ClinicalRotationData = rotationData;

            if (rotationData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = rotationData[0].TotalRecordCount;
            }
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public List<TenantDetailContract> GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Method to get shared user agencies for an institution
        /// </summary>
        //public void GetSharedUserAgencies()
        //{
        //    if (View.lstSelectedTenants.IsNullOrEmpty())
        //    {
        //        View.lstAgency = new List<AgencyDetailContract>();
        //    }
        //    else
        //    {
        //        String selectedTenants = String.Join(",", View.lstSelectedTenants.Select(x => x.TenantID).ToArray());
        //        ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
        //        serviceRequest.Parameter = selectedTenants;

        //        var _serviceResponse = _clinicalRotationProxy.GetSharedUserAgencies(serviceRequest);
        //        View.lstAgency = _serviceResponse.Result;
        //    }
        //}

        /// <summary>
        /// Method to get week days of an institution
        /// </summary>
        /// <param name="tenantID"></param>
        public void GetWeekDays(Int32 tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = tenantID;
            var _serviceResponse = _clinicalRotationProxy.GetWeekDayList(serviceRequest);
            View.WeekDayList = _serviceResponse.Result;
        }

        /// <summary>
        /// Method to get rotation statuses
        /// </summary>
        /// <param name="tenantID"></param>
        public void GetRotationStatus(Int32 tenantID)
        {
            List<SharedUserRotationReviewStatusContract> rotationStatusList = new List<SharedUserRotationReviewStatusContract>()
            {
                 new SharedUserRotationReviewStatusContract { Code =String.Empty, Name = "All" },
                  new SharedUserRotationReviewStatusContract { Code = RotationStatus.Completed.GetStringValue(), Name = "Inactive" },
                new SharedUserRotationReviewStatusContract { Code = RotationStatus.Active.GetStringValue(), Name = "Active" },
               
            };
            View.RotationStatusList = rotationStatusList;
        }

        public List<Int32> GetSharedUserTenantIds()
        {
            ServiceRequest<List<String>> serviceRequest = new ServiceRequest<List<String>>();
            serviceRequest.Parameter = View.SharedUserTypeCodes;
            var _serviceResponse = _clinicalRotationProxy.GetSharedUserTenantIDs(serviceRequest);
            return _serviceResponse.Result;
        }

        public Int32 GetRequirementSubscriptionIdByClinicalRotID(String clinicalRotationID, String tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = Convert.ToInt32(clinicalRotationID);
            serviceRequest.SelectedTenantId = Convert.ToInt32(tenantID);
            var _serviceResponse = _clinicalRotationProxy.GetRequirementSubscriptionIdByClinicalRotID(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetRotationReviewStatus(Int32 tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = tenantID;
            var _serviceResponse = _clinicalRotationProxy.GetRotationReviewStatus(serviceRequest);
            View.lstRotationReviewStatus = _serviceResponse.Result;
        }

        /// <summary>
        /// UAT 1409: The Agency User should be able to filter their search by those rotations that are active or completed (expired) AND by pending revieew or reviewed. 
        /// </summary>
        public void SaveUpdateRotationReviewStatus()
        {
            List<SharedUserRotationReviewContract> lstRotationReviewInfo = View.lstShareduserReviewContract.Where(cond => cond.Checked == true).ToList();
            ServiceRequest<List<SharedUserRotationReviewContract>, Int32> serviceRequest = new ServiceRequest<List<SharedUserRotationReviewContract>, Int32>();
            serviceRequest.Parameter1 = lstRotationReviewInfo;
            serviceRequest.Parameter2 = View.SelectedReviewStatusID;
            var _serviceResponse = _clinicalRotationProxy.SaveUpdateRotationReviewStatus(serviceRequest);


        }

        //public void GetAttestationDocumentsToExport(Int32 rotationID, Int32 tenantID)
        //{
        //    ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32>>>();
        //    serviceRequest.Parameter1 = new Dictionary<String, Int32> { { AppConsts.ROTATION_ID, rotationID } };
        //    serviceRequest.Parameter2 = new List<Tuple<Int32, Int32>> { new Tuple<Int32, Int32>(rotationID, tenantID) };
        //    View.LstInvitationDocumentContract = _clinicalRotationProxy.GetAttestationDocumentsToExport(serviceRequest).Result;
        //}
        public void GetSharedUserAgencyHierarchyRootNodes()
        {
            var _serviceResponse = _clinicalRotationProxy.GetSharedUserAgencyHierarchyRootNodes();
            View.lstAgencyHierarchyRootNodes = _serviceResponse.Result;
        }
    }
}
