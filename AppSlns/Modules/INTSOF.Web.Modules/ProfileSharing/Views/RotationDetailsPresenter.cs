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
    public class RotationDetailsPresenter : Presenter<IRotationDetailsView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public override void OnViewInitialized()
        {
            List<TenantDetailContract> tenants = GetTenants();
            List<Int32> sharedUserTenantIDs = new List<Int32>();

            sharedUserTenantIDs = GetSharedUserTenantIds();
            var tenantList = tenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
            View.lstTenants = tenantList;
            var tenantID = tenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
            GetRotationReviewStatus(tenantID);
            GetRotationStatus(tenantID);
        }

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
                serviceRequest.Parameter2 = View.SearchContract;
                var _serviceResponse = _clinicalRotationProxy.GetSharedUserClinicalRotationDetails(serviceRequest);
                //UAT 1723: Only shares (not assigned rotations) should show up when a rotation is created by a school that associates themselves with the Agency
                rotationData = _serviceResponse.Result.Where(x => x.IsProfileShared).ToList();
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

        public void GetWeekDays(Int32 tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = tenantID;
            var _serviceResponse = _clinicalRotationProxy.GetWeekDayList(serviceRequest);
            View.WeekDayList = _serviceResponse.Result;
        }

        public void GetRotationStatus(Int32 tenantID)
        {
            List<SharedUserRotationReviewStatusContract> rotationStatusList = new List<SharedUserRotationReviewStatusContract>()
            {
                new SharedUserRotationReviewStatusContract { Code = RotationStatus.Active.GetStringValue(), Name = "Active" },
                new SharedUserRotationReviewStatusContract { Code = RotationStatus.Completed.GetStringValue(), Name = "Completed" }
            };
            View.RotationStatusList = rotationStatusList;
        }

        private List<Int32> GetSharedUserTenantIds()
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

        private void GetRotationReviewStatus(Int32 tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = tenantID;
            var _serviceResponse = _clinicalRotationProxy.GetRotationReviewStatus(serviceRequest);
            View.lstRotationReviewStatus = _serviceResponse.Result;
        }

        public void SaveUpdateRotationReviewStatus()
        {
            List<SharedUserRotationReviewContract> lstRotationReviewInfo = View.lstShareduserReviewContract.Where(cond => cond.Checked == true).ToList();
            ServiceRequest<List<SharedUserRotationReviewContract>, Int32> serviceRequest = new ServiceRequest<List<SharedUserRotationReviewContract>, Int32>();
            serviceRequest.Parameter1 = lstRotationReviewInfo;
            serviceRequest.Parameter2 = View.SelectedReviewStatusID;
            var _serviceResponse = _clinicalRotationProxy.SaveUpdateRotationReviewStatus(serviceRequest);


        }

        public void GetAttestationDocumentsToExport(Int32 rotationID, Int32 tenantID, Int32 agencyID = 0)
        {
            ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>>();
            serviceRequest.Parameter1 = new Dictionary<String, Int32> { { AppConsts.ROTATION_ID, rotationID } };
            serviceRequest.Parameter2 = new List<Tuple<Int32, Int32, Int32>> { new Tuple<Int32, Int32, Int32>(rotationID, tenantID, agencyID) };
            View.LstInvitationDocumentContract = _clinicalRotationProxy.GetAttestationDocumentsToExport(serviceRequest).Result;
        }

        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies

        public void GetAgencyList()
        {
            List<AgencyDetailContract> lstAgencyTemp = new List<AgencyDetailContract>();
            ServiceRequest<List<Int32>, String> serviceRequest = new ServiceRequest<List<Int32>, String>();
            if (!View.lstSelectedTenants.IsNullOrEmpty())
            {
                List<Int32> selectedTenantIDs = View.lstSelectedTenants.Select(x => x.TenantID).ToList();
                serviceRequest.Parameter1 = selectedTenantIDs;
                serviceRequest.Parameter2 = View.UserID;
                var serviceResponse = _clinicalRotationProxy.GetInstitutionMappedAgency(serviceRequest);
                lstAgencyTemp = serviceResponse.Result;
            }
            View.lstAgency = lstAgencyTemp;
        }

        #endregion
    }
}
