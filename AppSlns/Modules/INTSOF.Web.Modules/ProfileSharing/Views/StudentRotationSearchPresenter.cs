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
using System.Text;
using System.Web;

namespace CoreWeb.ProfileSharing.Views
{
    public class StudentRotationSearchPresenter : Presenter<IStudentRotationSearchView>
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

            sharedUserTenantIDs = GetSharedUserTenantIds();
            var tenantList = tenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
            View.lstTenants = tenantList;
            var tenantID = tenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
            GetRotationReviewStatus(tenantID);
            GetRotationStatus(tenantID);
            GetInvitationArchiveStateList(); //UAT-3470
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
                #region UAT-3165
                StringBuilder customAttributes = new StringBuilder();
                Dictionary<Int32, string> dicCustomAttributes = new Dictionary<int, string>();
                if (!HttpContext.Current.Session["dicCustomAttributes"].IsNullOrEmpty())
                {

                    dicCustomAttributes = (Dictionary<Int32, string>)HttpContext.Current.Session["dicCustomAttributes"];
                    if (!dicCustomAttributes.IsNullOrEmpty())
                    {
                        foreach (var item in dicCustomAttributes)
                        {
                            customAttributes.Append(item.Value);
                        }
                    }
                }
                string customAttribute = customAttributes.ToString();
                #endregion

                ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract, Dictionary<Int32, string>> serviceRequest = new
                                        ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract, Dictionary<Int32, string>>();
                View.SearchContract.SelectedRotationInvitationArchiveStateCode = View.SelectedInvitationArchiveStatusCode.FirstOrDefault();
                serviceRequest.Parameter1 = View.lstSelectedTenants;
                serviceRequest.Parameter2 = View.SearchContract;
                serviceRequest.Parameter3 = dicCustomAttributes; //UAT-3165
                //var _serviceResponse = _clinicalRotationProxy.GetSharedUserClinicalRotationDetails(serviceRequest);
                var _serviceResponse = _clinicalRotationProxy.GetStudentRotationSearchDetails(serviceRequest);
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
                new SharedUserRotationReviewStatusContract { Code = RotationStatus.Active.GetStringValue(), Name = "Active" },
                new SharedUserRotationReviewStatusContract { Code = RotationStatus.Completed.GetStringValue(), Name = "Completed" }
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


        public void GetAttestationDocumentsToExport(Int32 rotationID, Int32 tenantID, Int32 agencyID)
        {
            ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>>();
            serviceRequest.Parameter1 = new Dictionary<String, Int32> { { AppConsts.ROTATION_ID, rotationID } };
            serviceRequest.Parameter2 = new List<Tuple<Int32, Int32, Int32>> { new Tuple<Int32, Int32, Int32>(rotationID, tenantID, agencyID) };
            View.LstInvitationDocumentContract = _clinicalRotationProxy.GetAttestationDocumentsToExport(serviceRequest).Result;
        }

        public void GetRotationReviewStatus(Int32 tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = tenantID;
            var _serviceResponse = _clinicalRotationProxy.GetRotationReviewStatus(serviceRequest);
            View.lstRotationReviewStatus = _serviceResponse.Result;
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

        #region UAT-3220
        public Boolean HideRequirementSharesDetailLink(Guid userID)
        {
            ServiceRequest<Guid> serviceRequest = new ServiceRequest<Guid>();
            serviceRequest.Parameter = userID;
            var _serviceResponse = _clinicalRotationProxy.HideRequirementSharesDetailLink(serviceRequest);
            return _serviceResponse.Result;
        }
        #endregion

        #region UAT-3470
        public void GetInvitationArchiveStateList()
        {
            View.lstInvitationArchiveState = ProfileSharingManager.GetinvitationArchiveStateList();
        }
        #endregion
    }
}
