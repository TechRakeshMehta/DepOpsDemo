using System;
using System.Collections.Generic;
using System.Linq;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RotationStudentSearchPresenter : Presenter<IRotationStudentSearchView>
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
            View.lstTenant = tenantList;
            View.lstTenantIds = String.Join(",", View.lstTenant.Select(x => x.TenantID));

            var tenantID = tenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
        }

        /// <summary>
        /// Method to get the id of shared user tenants
        /// </summary>
        /// <returns></returns>
        public List<Int32> GetSharedUserTenantIds()
        {
            ServiceRequest<List<String>> serviceRequest = new ServiceRequest<List<String>>();
            serviceRequest.Parameter = View.SharedUserTypeCodes;
            var _serviceResponse = _clinicalRotationProxy.GetSharedUserTenantIDs(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Methods to get the list of tenants 
        /// </summary>
        /// <returns></returns>
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
        /// Method to get the list of Rotation students after searching 
        /// </summary>
        public void GetRotationStudentDetails()
        {
            List<RotationMemberSearchDetailContract> rotationStudentData;

            ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract> serviceRequest = new
                                    ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract>();

            serviceRequest.Parameter1 = View.SearchParameterContract;
            serviceRequest.Parameter2 = View.StudentGridCustomPaging;

            if (!View.IsResetClicked)
                serviceRequest.Parameter1.lstTenantIDs = View.SelectedTenantIDs.IsNullOrEmpty() ? String.Join(",", View.lstTenant.Select(x => x.TenantID))
                                                            : String.Join(",", View.SelectedTenantIDs.Select(x => x.TenantID));

            var _serviceResponse = _clinicalRotationProxy.GetInstrctrPreceptrRotationStudents(serviceRequest);
            rotationStudentData = _serviceResponse.Result;
            View.lstRotationMemberSearchData = rotationStudentData;

            if (rotationStudentData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = rotationStudentData[0].TotalRecordCount;
            }
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unMaskedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unMaskedSSN;
            var _serviceResponse = _clinicalRotationProxy.GetMaskedSSN(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Getting Masked DOB
        /// </summary>
        /// <param name="unMaskedDOB"></param>
        /// <returns></returns>
        public String GetMaskDOB(String unMaskedDOB)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unMaskedDOB;
            var _serviceResponse = _clinicalRotationProxy.GetMaskDOB(serviceRequest);
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

        //UAT-4013
        public void GetSelectedOrganizatioUserIDs()
        {
            List<Int32> lstOrgUserIds = View.CustomMessageOrgUserIds.Keys.Select(x => Convert.ToInt32(x)).ToList();
            View.lstSelectedOrgUserIDs = GetOrganizationUserDetailsByOrgUserIDs(lstOrgUserIds);// GetOrgUserIDsListByRotationMemberIDs(tmpRotationIDs);
        }
        private Dictionary<Int32, String> GetOrganizationUserDetailsByOrgUserIDs(List<Int32> lstOrgUserIds)
        {
            return ProfileSharingManager.GetOrgUserDetailsByOrgUserID(lstOrgUserIds);
        }
        public Int32 GetRequirementSubscriptionIdByClinicalRotID(String clinicalRotationID, String tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = Convert.ToInt32(clinicalRotationID);
            serviceRequest.SelectedTenantId = Convert.ToInt32(tenantID);
            var _serviceResponse = _clinicalRotationProxy.GetRequirementSubscriptionIdByClinicalRotID(serviceRequest);
            return _serviceResponse.Result;
        }
    }
}
