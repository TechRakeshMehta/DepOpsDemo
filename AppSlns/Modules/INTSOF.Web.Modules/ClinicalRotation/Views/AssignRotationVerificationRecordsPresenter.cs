using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreWeb.ClinicalRotation.Views
{
    public class AssignRotationVerificationRecordsPresenter : Presenter<IAssignRotationVerificationRecords>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            View.lstTenant = _serviceResponse.Result;
        }

        public void GetAllAgency()
        {
            if (View.SelectedTenantIDs.Count == AppConsts.NONE)
                View.lstAgency = new List<Agency>();
            else
            {
                View.lstAgency = ProfileSharingManager.GetAgenciesByInstitionIDs(View.SelectedTenantIDs).OrderBy(x => x.AG_Name).ToList();
            }
        }

        public void PerformSearch()
        {
            if (View.SelectedTenantIDs.Count == AppConsts.NONE)
            {
                View.ApplicantSearchData = new List<RequirementVerificationQueueContract>();
            }
            else
            {
                RequirementVerificationQueueContract searchDataContract = new RequirementVerificationQueueContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                searchDataContract.RotationStartDate = View.RotationStartDate;
                searchDataContract.RotationEndDate = View.RotationEndDate;
                searchDataContract.SubmissionDate = View.SubmissionDate;
                searchDataContract.RequirementPackageName = String.IsNullOrEmpty(View.PackageName) ? null : View.PackageName;
                searchDataContract.AssignedUserName = String.IsNullOrEmpty(View.AssignedUserName) ? null : View.AssignedUserName;
                searchDataContract.IsCurrentRotation = View.IsCurrent;
                searchDataContract.IsRotationVerificationUserWorkQueue = false; ;
                searchDataContract.SelectedTenantIDs = String.Join(",", View.SelectedTenantIDs);
                searchDataContract.ComplioID = View.ComplioID;
                searchDataContract.RequirementItemVerificationCode = RequirementItemStatus.PENDING_REVIEW.GetStringValue();
                searchDataContract.SelectedRequirementPackageTypes = View.RequirementPackageTypes;
                //searchDataContract.SelectedAgencyIds = View.SelectedAgencyIDs; //UAT-3245
                searchDataContract.DPMIds = View.DeptProgramMappingID; //UAT-3245
                if (View.SelectedAgencyID > AppConsts.NONE)
                    searchDataContract.AgencyID = View.SelectedAgencyID; //UAT-3245
                try
                {
                    ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract> serviceRequest =
                           new ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract>();
                    serviceRequest.Parameter1 = searchDataContract;
                    serviceRequest.Parameter2 = View.GridCustomPaging;
                    var _serviceResponse = _clinicalRotationProxy.GetAssignmentRotationVerificationQueueData(serviceRequest);
                    View.ApplicantSearchData = _serviceResponse.Result;
                    if (View.ApplicantSearchData.Count > AppConsts.NONE)
                    {
                        View.VirtualRecordCount = View.ApplicantSearchData[0].TotalCount;
                        View.CurrentPageIndex = View.ApplicantSearchData[0].CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = AppConsts.NONE;
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }

                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = null;
                    throw e;
                }
            }

        }


        public bool AssignItemsToUser()
        {
            ServiceRequest<List<Int32>, Int32, String> serviceRequest = new ServiceRequest<List<Int32>, Int32, String>();
            serviceRequest.Parameter1 = View.SelectedVerificationItems;
            serviceRequest.Parameter2 = View.VerSelectedUserId;
            serviceRequest.Parameter3 = View.VerSelectedUserName;
            var _serviceResponse = _clinicalRotationProxy.AssignItemsToUser(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetUserListForSelectedTenant()
        {
            Int32 clientId = AppConsts.ONE;
            if (View.SelectedTenantIDs.Count != AppConsts.NONE)
            {
                View.lstOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(clientId, true,false,true,false,false).Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " " + x.LastName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }

        }

        /// <summary>
        /// Method to get requirement package types
        /// </summary>
        public void GetSharedRequirementPackageTypes()
        {
            var _serviceResponse = _clinicalRotationProxy.GetSharedRequirementPackageTypes();
            View.lstRequirementPackageType = _serviceResponse.Result;
        }
    }
}
