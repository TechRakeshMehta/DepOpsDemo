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
namespace CoreWeb.ClinicalRotation.Views
{
    public class ManageRotationByAgencyPresenter : Presenter<IManageRotationByAgencyView>
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
            GetTenants();
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
            if (SecurityManager.DefaultTenantID != View.TenantID)
            {
                View.SearchContract.CurrentLoggedInClientUserID = View.CurrentLoggedInUserId;
            }
            else
            {
                View.SearchContract.CurrentLoggedInClientUserID = null;
            }            
            if (View.SelectedArchiveStatusCode.IsNotNull())
                View.SearchContract.ArchieveStatusId = GetArchiveStateId();
            else
                View.SearchContract.ArchieveStatusId = null;

            ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract>();
            serviceRequest.Parameter1 = View.SearchContract;
            serviceRequest.Parameter2 = View.GridCustomPaging;
            var _serviceResponse = _clinicalRotationProxy.GetClinicalRotationDataFromFlatTable(serviceRequest);
            rotationData = _serviceResponse.Result;

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

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);
        }
        
        public void GetAllAgencies()
        {
            IsAdminLoggedIn();
            if (View.IsAdminLoggedIn)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantID;
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgencies(serviceRequest);
                View.lstAgency = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.SelectedTenantID;
                serviceRequest.Parameter = View.CurrentLoggedInUserId;
                var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                View.lstAgency = _serviceResponse.Result;
            }
        }

        public void GetClientContacts()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.GetClientContacts();
            View.ClientContactList = _serviceResponse.Result;
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetWeekDays()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            // serviceRequest.Parameter = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.GetWeekDays();
            View.WeekDayList = _serviceResponse.Result;
        }

        public Boolean DeleteClinicalRotation()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.SelectedRotationID;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.DeleteClinicalRotation(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Is Clinical Rotation Members exist for clinical rotation
        /// </summary>
        /// <returns></returns>
        public Boolean IsClinicalRotationMembersExistForRotation()
        {
            if (View.SelectedTenantID > AppConsts.NONE && View.SelectedRotationID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.SelectedTenantID;
                serviceRequest.Parameter = View.SelectedRotationID;
                var _serviceResponse = _clinicalRotationProxy.IsClinicalRotationMembersExistForRotation(serviceRequest);

                return _serviceResponse.Result;
            }
            return false;
        }
        public Dictionary<Int32, String> GetDefaultPermissionForClientAdmin()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedTenantID;
            ServiceResponse<Dictionary<Int32, String>> _serviceResponse = _clinicalRotationProxy.GetDefaultPermissionForClientAdmin(serviceRequest);
            return _serviceResponse.Result;
        }
        public void GetArchiveStateList()
        {
            View.lstArchiveState = SharedUserClinicalRotationManager.GetArchiveStates();
        }

        private String GetArchiveStateId()
        {
            return SharedUserClinicalRotationManager.GetArchiveStates().FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStatusCode.FirstOrDefault())).AS_ID.ToString();
        }
     
    }
}

