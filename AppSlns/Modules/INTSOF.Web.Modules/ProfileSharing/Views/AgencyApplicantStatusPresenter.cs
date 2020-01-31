using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public class AgencyApplicantStatusPresenter : Presenter<IAgencyApplicantStatus>
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
            GetAllAgency();
            GetTenants();
        }
        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            View.lstAgency = ProfileSharingManager.GetAgencyUserMappedAgencies(View.CurrentLoggedInUserId);

            //UAT-1881
            //if (IsAdminLoggedIn())
            //{
            //    ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            //    serviceRequest.Parameter = View.TenantId;
            //    ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgencies(serviceRequest);
            //    //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
            //    View.lstAgency = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            //}
            //else
            //{
            //ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            //serviceRequest.SelectedTenantId = View.TenantID;
            //serviceRequest.Parameter = View.CurrentLoggedInUserId;
            //var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
            //View.lstAgency = _serviceResponse.Result;
            //}
        }


        public void GetTenants()
        {
            View.lstTenant = ProfileSharingManager.GetAgencyHierarchyMappedTenant(View.CurrentLoggedInUserId, View.TenantID);
            //Boolean SortByName = true;
            //String clientCode = TenantType.Institution.GetStringValue();
            //ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            //serviceRequest.Parameter1 = SortByName;
            //serviceRequest.Parameter2 = clientCode;
            //var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            //View.lstTenant = _serviceResponse.Result;
        }
        public void GetApplicantProfileSharingHistory()
        {
            //View.lstAgencyApplicantShareHistory = ProfileSharingManager.GetApplicantProfileSharingHistory(View.ApplicantID, View.TenantID);
        }
        public void GetAgencyApplicantStatus()
        {

            List<AgencyApplicantStatusContract> lstAgencyApplicantStatusContractData = new List<AgencyApplicantStatusContract>();

            AgencyApplicantStatusContract objAgencyApplicantStatusContract = new AgencyApplicantStatusContract();
            objAgencyApplicantStatusContract.TenantId = View.TenantID;
            objAgencyApplicantStatusContract.AgencyId = View.AgencyID;
            objAgencyApplicantStatusContract.ApplicantName = View.ApplicantName;
            objAgencyApplicantStatusContract.CurrentLoggedInUser = View.CurrentLoggedInUserId;
            lstAgencyApplicantStatusContractData = ProfileSharingManager.GetAgencyApplicantStatus(objAgencyApplicantStatusContract, View.GridCustomPaging);

            if (!lstAgencyApplicantStatusContractData.IsNullOrEmpty())
            {
                if (lstAgencyApplicantStatusContractData[0].TotalCount > 0)
                {
                    View.VirtualRecordCount = lstAgencyApplicantStatusContractData[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }

            View.lstAgencyApplicantStatus = lstAgencyApplicantStatusContractData;
        }
    }
}
