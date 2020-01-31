#region NameSpace
#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
#region Project Specific
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
#endregion
#endregion

namespace CoreWeb.ClinicalRotation.Views
{
    public class RotationMemberSearchPresenter : Presenter<IRotationMemberSearchView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ClientContactProxy _clientContactProxy
        {
            get
            {
                return new ClientContactProxy();
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
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            if (View.SelectedTenantID == 0)
                View.lstAgency = new List<AgencyDetailContract>();
            else
            {
                //UAT-1881
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                IsAdminLoggedIn();
                if (View.IsAdminLoggedIn)
                {
                    serviceRequest.Parameter = View.SelectedTenantID;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
                else
                {
                    serviceRequest.SelectedTenantId = View.SelectedTenantID;
                    serviceRequest.Parameter = View.CurrentUserID;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
            }
        }

        /// <summary>
        /// To get User Groups
        /// </summary>
        public void GetAllUserGroups()
        {
            if (View.SelectedTenantID == 0)
                View.lstUserGroup = new List<Entity.ClientEntity.UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = View.IsAdminLoggedIn ? (Int32?)null : View.CurrentUserID;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(View.SelectedTenantID,currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        /// <summary>
        /// To get User Types: Applicant / Instr-Preceptor -UAT-3749
        /// </summary>
        public void GetUserTypefrRMS()
        {
            if (View.SelectedTenantID == 0)
                View.dicUserTypes = new Dictionary<String, String>();
            else
            {
                Dictionary<String, String> dicUserTypes = new Dictionary<String, String>();
                dicUserTypes.Add("AAAC", UserTypeSwitchView.Applicant.ToString());
                dicUserTypes.Add("AAAE", "Instructor/Preceptor");
                View.dicUserTypes = dicUserTypes;
                //View.dicUserTypes.Add("AAAC", UserTypeSwitchView.Applicant.ToString());
                //View.dicUserTypes.Add("AAAE", "Instructor/Preceptor");
            }
        }

        public void GetClientContacts()
        {
            if (View.SelectedTenantID == 0)
                View.ClientContactList = new List<ClientContactContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantID;
                var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
                View.ClientContactList = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetWeekDays()
        {

            if (View.SelectedTenantID == 0)
                View.WeekDayList = new List<WeekDayContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantID;
                var _serviceResponse = _clinicalRotationProxy.GetWeekDayList(serviceRequest);
                View.WeekDayList = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);
        }

        public void GetApplicantDocumentToExport()
        {
            if (View.SelectedTenantID == 0 && View.LstApplicantRotationToExport.IsNullOrEmpty())
                View.LstApplicantDocumentToExport = new List<ApplicantDocumentContract>();
            else
            {
                if (View.LstApplicantRotationToExport.Count > AppConsts.NONE)
                {
                    //Direct call manager method without using proxy service, because getting error [entity too large].
                    View.LstApplicantDocumentToExport = ClinicalRotationManager.GetApplicantDocumentToExport(View.LstApplicantRotationToExport, View.SelectedTenantID);
                }
                else
                {
                    View.LstApplicantDocumentToExport = new List<ApplicantDocumentContract>();
                }
            }
        }

        public void GetRotationMemberSearchData()
        {
            List<RotationMemberSearchDetailContract> rotationMemberSearchData;
            if (View.SelectedTenantID == 0)
            {
                rotationMemberSearchData = new List<RotationMemberSearchDetailContract>();
            }
            else
            {
                View.SearchContract.TenantID = View.SelectedTenantID;

                //UAT-3549
                if (SecurityManager.DefaultTenantID != View.TenantID)
                {
                    View.SearchContract.CurrentLoggedInClientUserID = View.CurrentLoggedInUserId;
                }
                else
                {
                    View.SearchContract.CurrentLoggedInClientUserID = null;
                }

                //UAT-2545
                if (View.SelectedArchiveStatusCode.IsNotNull())
                    View.SearchContract.ArchieveStatusId = GetArchiveStateId();
                else
                    View.SearchContract.ArchieveStatusId = null;

                ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract>();
                serviceRequest.Parameter1 = View.SearchContract;
                serviceRequest.Parameter2 = View.GridCustomPaging;
                var _serviceResponse = _clinicalRotationProxy.GetRotationMemberSearchData(serviceRequest);
                rotationMemberSearchData = _serviceResponse.Result;
            }
            View.LstRotationMemberSearchData = rotationMemberSearchData;
            if (rotationMemberSearchData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = rotationMemberSearchData[0].TotalRecordCount;
            }
        }

        public Int32 GetRequirementSubscriptionIdByClinicalRotID(String clinicalRotationID,String organizationUserId,String isApplicant)
        {
            ServiceRequest<Int32,Int32,String> serviceRequest = new ServiceRequest<Int32,Int32,String>();
            serviceRequest.Parameter1 = Convert.ToInt32(clinicalRotationID);
            serviceRequest.Parameter2 = Convert.ToInt32(organizationUserId);
            serviceRequest.Parameter3 = isApplicant;
            serviceRequest.SelectedTenantId = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.GetSubscriptionIdByRotIDAndUserID(serviceRequest);
            return _serviceResponse.Result;
        }

        #region UAT-2545
        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantID);
        }
        private String GetArchiveStateId()
        {
            return ComplianceDataManager.GetArchiveStateList(View.SelectedTenantID).FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStatusCode.FirstOrDefault())).AS_ID.ToString();

        }
        #endregion
    }
}
