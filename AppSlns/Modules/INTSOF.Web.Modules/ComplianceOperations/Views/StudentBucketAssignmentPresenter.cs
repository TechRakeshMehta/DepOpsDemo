using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;

namespace CoreWeb.ComplianceOperations.Views
{
    public class StudentBucketAssignmentPresenter : Presenter<IStudentBucketAssignmentView>
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

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
            GetGranularPermissionForDOBandSSN();
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// To get Tenant Id
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// To get Client Id
        /// </summary>
        private Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }

        /// <summary>
        /// To get Admin Program Study
        /// </summary>
        public void GetAllUserGroups()
        {
            if (ClientId == 0)
                View.lstUserGroup = new List<UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID ? (Int32?)null : View.CurrentLoggedInUserId;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(ClientId, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }


        public void PerformSearch()
        {
            if (ClientId == 0)
            {
                View.GridSearchData = new List<StudentBucketAssignmentContract>();
            }
            else
            {
                SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                if (View.OrganizationUserID > SysXDBConsts.NONE)
                {
                    searchDataContract.OrganizationUserId = View.OrganizationUserID;
                }
                searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
                searchDataContract.DateOfBirth = View.DateOfBirth;
                //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
                if (!View.DPM_IDs.IsNullOrEmpty())
                {
                    searchDataContract.SelectedDPMIds = View.DPM_IDs;
                }
                searchDataContract.CustomFields = String.IsNullOrEmpty(View.CustomFields) ? null : View.CustomFields;
                if (View.MatchUserGroupId > SysXDBConsts.NONE)
                {
                    searchDataContract.MatchUserGroupID = View.MatchUserGroupId;
                }
                if (View.FilterUserGroupId > SysXDBConsts.NONE)
                {
                    //if (View.IsResult)
                    //{
                    searchDataContract.FilterUserGroupID = View.FilterUserGroupId;
                    //}
                }

                if (View.TenantId != SecurityManager.DefaultTenantID)
                {
                    searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                }

                searchDataContract.LoggedInUserTenantId = View.TenantId;


                searchDataContract.AgencyIDToFilterRotation = View.AgencyID;
                searchDataContract.ComplioID = View.ComplioID.ToFormatApostrophe();
                searchDataContract.RotationName = View.RotationName.ToFormatApostrophe();
                searchDataContract.Department = View.Department.ToFormatApostrophe();
                searchDataContract.Program = View.Program.ToFormatApostrophe();
                searchDataContract.Course = View.Course.ToFormatApostrophe();
                searchDataContract.Term = View.Term.ToFormatApostrophe();
                searchDataContract.UnitFloorLoc = View.UnitFloorLoc.ToFormatApostrophe();
                searchDataContract.RecommendedHours = View.RecommendedHours;
                searchDataContract.Students = View.Students;
                searchDataContract.Shift = View.Shift.ToFormatApostrophe();
                searchDataContract.StartTime = View.StartTime;
                searchDataContract.EndTime = View.EndTime;
                searchDataContract.StartDate = View.StartDate;
                searchDataContract.EndDate = View.EndDate;
                searchDataContract.DaysIdList = View.DaysIdList;
                searchDataContract.ContactIdList = View.ContactIdList;
                searchDataContract.TypeSpecialty = View.TypeSpecialty.ToFormatApostrophe();
                searchDataContract.RotationCustomAttributes = View.RotationCustomAttributesXML;

                #region UAT-1088
                //if (!View.OrderCreatedFrom.IsNullOrEmpty() && View.OrderCreatedFrom != DateTime.MinValue)
                //{
                //    searchDataContract.OrderCreatedFrom = View.OrderCreatedFrom;
                //}
                //else
                //{
                //    searchDataContract.OrderCreatedFrom = null;
                //}
                //if (!View.OrderCreatedTo.IsNullOrEmpty() && View.OrderCreatedTo != DateTime.MinValue)
                //{
                //    searchDataContract.OrderCreatedTo = View.OrderCreatedTo;
                //}
                //else
                //{
                //    searchDataContract.OrderCreatedTo = null;
                //}
                #endregion

                //if (View.SelectedArchiveStateCode.IsNotNull())
                //{
                //    searchDataContract.LstArchiveState = View.SelectedArchiveStateCode;
                //    searchDataContract.ArchieveStateId = GetXMLString(GetArchiveStateId());
                //}
                try
                {
                    View.GridSearchData = ComplianceDataManager.GetStudentBucketAssignmentSearch(ClientId, searchDataContract, View.GridCustomPaging);
                    if (View.GridSearchData.IsNotNull() && View.GridSearchData.Count > 0)
                    {
                        if (View.GridSearchData[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.GridSearchData[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                        //if (View.CurrentPageIndex == 1 && View.AssignOrganizationUserIds.Count == 0)
                        //{
                        //    SetAssignedUsersDic();
                        //}
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                    }
                }
                catch (Exception e)
                {
                    View.GridSearchData = null;
                    throw e;
                }
            }
        }

        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }

        public string GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.STUDENT_BUCKET_ASSIGNMENT.GetStringValue()))
                {
                    View.LstStudentBucketAssigmentPermissions = dicPermissions[EnumSystemEntity.STUDENT_BUCKET_ASSIGNMENT.GetStringValue()].Split(',').ToList();
                }

                //UAT-3010:-  Granular Permission for Client Admin Users to Archive.
                if (dicPermissions.ContainsKey(EnumSystemEntity.ARCHIVE_ABILITY.GetStringValue()))
                {
                    View.ArchivePermissionCode = dicPermissions[EnumSystemEntity.ARCHIVE_ABILITY.GetStringValue()];
                }
            }
        }

        public void SendScheduleRotationNotification(Dictionary<String, Object> conversionData)
        {
            String orgUserIds = Convert.ToString(conversionData["orgUserIds"]);
            Int32 tenantId = Convert.ToInt32(conversionData["tenantId"]);
            Int32 CurentLoggedInUserId = Convert.ToInt32(conversionData["CurentLoggedInUserId"]);

            String tenantName = SecurityManager.GetTenant(tenantId).TenantName;
            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

            List<ClinicalRotationMemberDetail> lstRotationDeatils = ClinicalRotationManager.GetRotationDetailsByOrgUserIds(orgUserIds, tenantId);

            foreach (ClinicalRotationMemberDetail clinicalRotationMemberDetail in lstRotationDeatils)
            {
                //Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, clinicalRotationMemberDetail.ApplicantName);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());
                dictMailData.Add(EmailFieldConstants.ROTATION_NAME, clinicalRotationMemberDetail.RotationName);
                dictMailData.Add(EmailFieldConstants.ROTATION_START_DATE, Convert.ToDateTime(clinicalRotationMemberDetail.StartDate).ToString("MM/dd/yyyy"));
                dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateRotationDetailsHTML(clinicalRotationMemberDetail));

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = clinicalRotationMemberDetail.ApplicantName;
                mockData.EmailID = clinicalRotationMemberDetail.PrimaryEmailaddress;
                mockData.ReceiverOrganizationUserID = clinicalRotationMemberDetail.OrganizationUserId;

                //send assign to rotation sms --UAT-3688
                CommunicationManager.SaveDataForSMSNotification(CommunicationSubEvents.NOTIFICATION_CLINICAL_ROTATION_ASSIGNED_SMS, mockData,
                                                                new Dictionary<String, object>(), tenantId, AppConsts.NONE);

                //Send mail
                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.ROTATION_ABOUT_TO_START, dictMailData, mockData, tenantId, -1, null, null, true, false, null, clinicalRotationMemberDetail.RotationHirarchyIds, clinicalRotationMemberDetail.RotationID);

                //Send Message
                CommunicationManager.SaveMessageContent(CommunicationSubEvents.ROTATION_ABOUT_TO_START, dictMailData, clinicalRotationMemberDetail.OrganizationUserId, tenantId);

                ClinicalRotationManager.UpdateClinicalRotationMenberForNagMail(clinicalRotationMemberDetail.RotationMemberId, CurentLoggedInUserId, tenantId);
            }
        }

        public void SendScheduleRequirementsNotification(Dictionary<String, Object> conversionData)
        {
            String orgUserIds = Convert.ToString(conversionData["orgUserIds"]);
            Int32 tenantId = Convert.ToInt32(conversionData["tenantId"]);
            Int32 CurentLoggedInUserId = Convert.ToInt32(conversionData["CurentLoggedInUserId"]);
            String tenantName = SecurityManager.GetTenant(tenantId).TenantName;
            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

            List<ClinicalRotationMemberDetail> lstRotationDeatils = ClinicalRotationManager.GetRotationDetailsByOrgUserIds(orgUserIds, tenantId);

            foreach (ClinicalRotationMemberDetail clinicalRotationMemberDetail in lstRotationDeatils)
            {
                //Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, clinicalRotationMemberDetail.ApplicantName);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());
                dictMailData.Add(EmailFieldConstants.ROTATION_NAME, clinicalRotationMemberDetail.RotationName);
                dictMailData.Add(EmailFieldConstants.ROTATION_START_DATE, Convert.ToDateTime(clinicalRotationMemberDetail.StartDate).ToString("MM/dd/yyyy"));
                //UAT-2191
                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, clinicalRotationMemberDetail.AgencyName);
                //UAT-2290
                if (!clinicalRotationMemberDetail.DeadlineDate.IsNullOrEmpty())
                {
                    dictMailData.Add(EmailFieldConstants.DEADLINE_DATE, Convert.ToDateTime(clinicalRotationMemberDetail.DeadlineDate).ToString("MM/dd/yyyy"));
                }
                else
                {
                    dictMailData.Add(EmailFieldConstants.DEADLINE_DATE, String.Empty);
                }

                dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateRotationDetailsHTML(clinicalRotationMemberDetail));

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = clinicalRotationMemberDetail.ApplicantName;
                mockData.EmailID = clinicalRotationMemberDetail.PrimaryEmailaddress;
                mockData.ReceiverOrganizationUserID = clinicalRotationMemberDetail.OrganizationUserId;

                //Send mail
                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFY_OF_ROTATION_SCHEDULE_AND_REQUIREMENTS, dictMailData, mockData, tenantId, -1, null, null, true, false, null, clinicalRotationMemberDetail.RotationHirarchyIds, clinicalRotationMemberDetail.RotationID);

                //Send Message
                CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFY_OF_ROTATION_SCHEDULE_AND_REQUIREMENTS, dictMailData, clinicalRotationMemberDetail.OrganizationUserId, tenantId);
            }
        }

        public Boolean ArchieveSubscriptions()
        {
            String compArchiveResult = "";
            String bkgArchiveResult = "";
            Dictionary<String, List<Int32>> data = ComplianceDataManager.GetSubscriptionsListForArchival(View.SelectedTenantId, View.SelectedOrgUsersToList);
            List<Int32> subscriptionIds = new List<Int32>();
            if (data.IsNotNull() && data.ContainsKey("lstMultipleSubscriptions"))
            {
                foreach (var subId in data.GetValue("lstMultipleSubscriptions"))
                {
                    subscriptionIds.Add(subId);
                }
            }
            if (data.IsNotNull() && data.ContainsKey("lstSingleSubscriptions"))
            {
                foreach (var subId in data.GetValue("lstSingleSubscriptions"))
                {
                    subscriptionIds.Add(subId);
                }
            }
            if (!subscriptionIds.IsNullOrEmpty())
                compArchiveResult = ComplianceDataManager.ArchieveSubscriptionsManually(subscriptionIds, View.SelectedTenantId, View.CurrentLoggedInUserId);

            String ArchiveCode = ArchiveState.Active.GetStringValue();
            List<Int32> orgUserIds = View.SelectedOrgUsersToList.Keys.ToList();
            List<Int32> bkgOrderId = StoredProcedureManagers.GetBkgOrderIdByOrgUsers(orgUserIds, ArchiveCode, View.SelectedTenantId);
            if (!bkgOrderId.IsNullOrEmpty())
                bkgArchiveResult = StoredProcedureManagers.ArchieveBkgOrderIds(bkgOrderId, View.SelectedTenantId, View.CurrentLoggedInUserId);

            if ((compArchiveResult == "true" || subscriptionIds.IsNullOrEmpty())
                && (bkgArchiveResult == "true" || bkgOrderId.IsNullOrEmpty()))
                return true;
            return false;
        }

        public Boolean UnarchiveSubscription()
        {
            return ComplianceDataManager.SetUnArchiveStatusByOrgUserIds(View.SelectedOrgUsersToList.Keys.ToList(), View.SelectedTenantId, View.CurrentLoggedInUserId);
        }

        public void GetAllAgency()
        {
            if (View.SelectedTenantId == 0)
                View.lstAgency = new List<AgencyDetailContract>();
            else
            {
                //UAT-1881
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();

                if (IsDefaultTenant)
                {
                    serviceRequest.Parameter = View.SelectedTenantId;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
                else
                {
                    serviceRequest.SelectedTenantId = View.SelectedTenantId;
                    serviceRequest.Parameter = View.CurrentLoggedInUserId;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
            }
        }

        public void GetClientContacts()
        {
            if (View.SelectedTenantId == 0)
                View.ClientContactList = new List<ClientContactContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantId;
                var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
                View.ClientContactList = _serviceResponse.Result;
            }
        }

        public void GetWeekDays()
        {

            if (View.SelectedTenantId == 0)
                View.WeekDayList = new List<WeekDayContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantId;
                var _serviceResponse = _clinicalRotationProxy.GetWeekDayList(serviceRequest);
                View.WeekDayList = _serviceResponse.Result;
            }
        }

        public void GetRotationCustomAttributeList(Int32? rotationId)
        {
            ServiceRequest<Int32, String, Int32?> serviceRequest = new ServiceRequest<Int32, String, Int32?>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue();
            serviceRequest.Parameter3 = rotationId;
            var _serviceResponse = _clinicalRotationProxy.GetCustomAttributeListMapping(serviceRequest);
            View.RotationCustomAttributeList = _serviceResponse.Result;
        }

        #region UAT-2422
        public void SetQueueImaging()
        {
            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.SelectedTenantId);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }
        #endregion
    }
}
