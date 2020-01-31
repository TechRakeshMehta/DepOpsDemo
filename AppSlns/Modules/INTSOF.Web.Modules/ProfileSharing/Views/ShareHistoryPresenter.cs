using System;
using System.Collections.Generic;
using System.Linq;
using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using INTSOF.Utils.Consts;

namespace CoreWeb.ProfileSharing.Views
{
    public class ShareHistoryPresenter : Presenter<IShareHistory>
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
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        public Int32 GetClientID()
        {
            Int32 clientID = 0;
            if (View.IsAdminLoggedIn)
            {
                clientID = View.SelectedTenantID;
            }
            else
            {
                clientID = View.ClientTenantID;
            }
            return clientID;
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.LstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.ClientTenantID);
        }

        /// <summary>
        /// To get User Groups
        /// </summary>
        public void GetAllUserGroups()
        {
            Int32 clientID = GetClientID();
            if (clientID == 0)
                View.LstUserGroup = new List<Entity.ClientEntity.UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = View.IsAdminLoggedIn ? (Int32?)null : View.CurrentLoggedInUserId;
                View.LstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(clientID, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        public void GetAllAgency()
        {
            Int32 clientID = GetClientID();
            if (clientID == 0)
                View.LstAgency = new List<Agency>();
            else
            {
                //UAT-1881
                IsAdminLoggedIn();
                if (View.IsAdminLoggedIn)
                {
                    View.LstAgency = ProfileSharingManager.GetAllAgency(clientID).OrderBy(x => x.AG_Name).ToList();
                }
                else
                {
                    View.LstAgency = ProfileSharingManager.GetAllAgencyForOrgUser(View.SelectedTenantID, View.CurrentLoggedInUserId).OrderBy(x => x.AG_Name).ToList();
                }
            }
        }

        public void GetShareHistoryData()
        {
            Int32 clientID = GetClientID();
            if (clientID == 0)
            {
                View.ShareHistoryData = new List<ShareHistoryDataContract>();
            }
            else
            {
                //View.DataGridCustomPaging.DefaultSortExpression = "InvitationGroupId";
                //UAT-3142
                View.DataGridCustomPaging.DefaultSortExpression = "StartDate";

                if (View.DataGridCustomPaging.SortExpression.IsNullOrEmpty())
                    View.DataGridCustomPaging.SortDirectionDescending = false;

                ShareHistorySearchContract searchDataContract = new ShareHistorySearchContract();
                searchDataContract.TenantId = View.SelectedTenantID;
                searchDataContract.AgencyID = View.SelectedAgencyID;

                if (View.SelectedUserGroupID > SysXDBConsts.NONE)
                    searchDataContract.UserGroupID = View.SelectedUserGroupID;

                if (!View.SelectedHierarchyIDs.IsNullOrEmpty())
                    searchDataContract.DPMIds = View.SelectedHierarchyIDs;

                if (View.OrganizationUserID > SysXDBConsts.NONE)
                    searchDataContract.OrganizationUserID = View.OrganizationUserID;

                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                searchDataContract.ApplicantSSN = View.SSN;
                searchDataContract.DateOfBirth = View.DOB;
                searchDataContract.CustomFields = String.IsNullOrEmpty(View.CustomFields) ? null : View.CustomFields;

                searchDataContract.RotationName = View.RotationName;
                searchDataContract.TypeSpecialty = View.TypeSpecialty;
                searchDataContract.Department = View.Department;
                searchDataContract.Program = View.Program;
                searchDataContract.Course = View.Course;
                searchDataContract.Term = View.Term;
                searchDataContract.UnitFloorLoc = View.UnitFloorLoc;
                searchDataContract.DaysIdList = View.DaysIdList;
                searchDataContract.StartTime = View.StartTime.HasValue ? View.StartTime.Value.ToString() : string.Empty;
                searchDataContract.EndTime = View.EndTime.HasValue ? View.EndTime.Value.ToString() : string.Empty;
                searchDataContract.StartDate = View.StartDate;
                searchDataContract.EndDate = View.EndDate;
                searchDataContract.SelectedClientContacts = View.SelectedClientContacts;
                searchDataContract.RotationCustomAttributes = String.IsNullOrEmpty(View.RotationCustomAttributes) ? null : View.RotationCustomAttributes;
                searchDataContract.LoggedInUserId = View.IsAdminLoggedIn ? null : (Int32?)View.CurrentLoggedInUserId;
                //UAT-1895 Added filter to select all shares or Aduit Requested shares 
                searchDataContract.IsAuditRequested = View.IsAuditRequested;
                try
                {
                    View.ShareHistoryData = ProfileSharingManager.GetShareHistoryData(View.SelectedTenantID, searchDataContract, View.DataGridCustomPaging);

                    if (View.ShareHistoryData.IsNotNull() && View.ShareHistoryData.Count > 0)
                    {
                        if (View.ShareHistoryData[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ShareHistoryData[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.DataGridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = AppConsts.NONE;
                    }
                }
                catch (Exception e)
                {
                    View.ShareHistoryData = null;
                    throw e;
                }
            }
        }

        public InvitationDocument GetInvitationDocument()
        {
            return ProfileSharingManager.GetInvitationDocumentByProfileSharingInvitationID(View.InvitationIdToDownloadReport);
        }

        public void GetClientContacts()
        {
            if (View.SelectedTenantID == 0)
            {
                View.ClientContactList = new List<INTSOF.ServiceDataContracts.Modules.ClientContact.ClientContactContract>();
            }
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantID;
                var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
                View.ClientContactList = _serviceResponse.Result;
            }
        }

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

        public void GetCustomAttributeList()
        {
            ServiceRequest<Int32, String, Int32?> serviceRequest = new ServiceRequest<Int32, String, Int32?>();
            serviceRequest.Parameter1 = View.SelectedTenantID;
            serviceRequest.Parameter2 = CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue();
            serviceRequest.Parameter3 = null;
            var _serviceResponse = _clinicalRotationProxy.GetCustomAttributeListMapping(serviceRequest);
            View.GetCustomAttributeList = _serviceResponse.Result;
        }

        public void GetAttestationDocumentsToExport(Int32 InvitationGroupID, Int32 invitationID)
        {
            ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>>();
            serviceRequest.Parameter1 = new Dictionary<String, Int32> { { AppConsts.PROFILE_SHARING_INVITATION_GROUP_ID, InvitationGroupID },
                                                                                  { AppConsts.PROFILE_SHARING_INVITATION_ID, invitationID }, 
                                                                                  { AppConsts.IGNORE_AGENCY_USER_CHECK, AppConsts.ONE },
                                                                                  { AppConsts.IS_ADMIN,AppConsts.ONE }};
            serviceRequest.Parameter2 = new List<Tuple<Int32, Int32, Int32>>();
            View.LstInvitationDocumentContract = _clinicalRotationProxy.GetAttestationDocumentsToExport(serviceRequest).Result;
        }
    }
}
