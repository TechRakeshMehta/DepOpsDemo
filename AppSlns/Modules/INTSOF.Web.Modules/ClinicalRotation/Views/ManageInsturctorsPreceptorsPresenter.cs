using System;
using System.Collections.Generic;
using System.Linq;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.UI.Contract.SysXSecurityModel;
using Entity;
using System.Configuration;

namespace CoreWeb.ClinicalRotation.Views
{
    public class ManageInsturctorsPreceptorsPresenter : Presenter<IManageInsturctorsPreceptorsView>
    {
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

        #region PUBLIC METHODS

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
            var _serviceResponse = _clientContactProxy.GetTenants(serviceRequest);
            View.LstTenants = _serviceResponse.Result;

            //Boolean SortByName = true;
            //String clientCode = TenantType.Institution.GetStringValue();
            //View.LstTenants = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);
        }

        #region Lookup Tables
        public void GetDocumentType()
        {
            var _serviceResponse = _clientContactProxy.GetDocumentTypeList();

            //UAT-1678: Instructor/Preceptor updates
            var documentTypeList = _serviceResponse.Result;
            var attestDocCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT.GetStringValue();
            var verAttestDocCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue();
            var consolidatedAttestDocCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED.GetStringValue();
            var AttestationWithoutSign = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();
            View.DocumentTypeList = documentTypeList.Where(x => x.Code != attestDocCode && x.Code != verAttestDocCode
                                                           && x.Code != consolidatedAttestDocCode && x.Code != AttestationWithoutSign).ToList();


            //View.DocumentTypeList = _serviceResponse.Result;
            //Removed Attestation Document Type from list.
            //SharedSystemDocTypeContract docTypeToRemove = View.DocumentTypeList.Where(x => x.Code == "AAAE").FirstOrDefault();
            //View.DocumentTypeList.Remove(docTypeToRemove);
        }

        public void GetWeekDays()
        {
            var _serviceResponse = _clientContactProxy.GetWeekDaysList();
            View.WeekDayList = _serviceResponse.Result;
            //View.WeekDayList = new List<WeekDayContract>();
        }

        public void GetClientContactTypeList()
        {
            var _serviceResponse = _clientContactProxy.GetClientContactTypeList();
            View.ClientContactTypeList = _serviceResponse.Result;
            //View.ClientContactTypeList = new List<ClientContactTypeContract>();
        }
        #endregion

        public void GetClientContacts()
        {
            if (View.SelectedTenantID != AppConsts.NONE)
            {

                //UAT-4160
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantID;
                var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
                View.ClientContactList = _serviceResponse.Result;
                if (View.IsAccountActivated != 2)
                {
                    if (View.IsAccountActivated == 1)
                        View.ClientContactList = View.ClientContactList.Where(cond => cond.IsRegistered).ToList();
                    else
                        View.ClientContactList = View.ClientContactList.Where(cond => !cond.IsRegistered).ToList();
                }

                //Below code is commeneted in UAT-4160
                //if (View.IsAccountActivated == 2) // If Applicant selects Both on Account activated
                //{
                //    ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                //    serviceRequest.Parameter = View.SelectedTenantID;

                //    var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
                //    View.ClientContactList = View.ClientContactList = _serviceResponse.Result.Select(index => new ClientContactContract
                //    {
                //        ClientContactID = index.ClientContactID,
                //        ClientContactTypeID = index.ClientContactTypeID,
                //        Email = index.Email,
                //        IsInternationalPhone = index.IsInternationalPhone,
                //        ListClientContactAvailibiltyContract = index.ListClientContactAvailibiltyContract,
                //        Name = index.Name,
                //        OrgUserId = index.OrgUserId,
                //        Phone = index.Phone,
                //        TenantID = index.TenantID,
                //        TenantName = index.TenantName,
                //        TokenID = index.TokenID,
                //        UserID = index.UserID,
                //        AccountActivated = index.IsRegistered ? "Yes" : "No", //index.UserID.IsNotNull() ? "Yes" : "No"
                //        IsRegistered = index.IsRegistered
                //    }).ToList();
                //}
                //else //In case of Yes or No on account activated
                //{
                //    ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                //    serviceRequest.Parameter1 = View.SelectedTenantID;
                //    serviceRequest.Parameter2 = View.IsAccountActivated;
                //    var _serviceResponse = _clientContactProxy.GetClientContactSearchData(serviceRequest);
                //    View.ClientContactList = _serviceResponse.Result.Select(index => new ClientContactContract
                //    {
                //        ClientContactID = index.ClientContactID,
                //        ClientContactTypeID = index.ClientContactTypeID,
                //        Email = index.Email,
                //        IsInternationalPhone = index.IsInternationalPhone,
                //        ListClientContactAvailibiltyContract = index.ListClientContactAvailibiltyContract,
                //        Name = index.Name,
                //        OrgUserId = index.OrgUserId,
                //        Phone = index.Phone,
                //        TenantID = index.TenantID,
                //        TenantName = index.TenantName,
                //        TokenID = index.TokenID,
                //        UserID = index.UserID,
                //        AccountActivated = index.IsRegistered ? "Yes" : "No",// index.UserID.IsNotNull() ? "Yes" : "No"
                //        IsRegistered = index.IsRegistered
                //    }).ToList();

                //}

            }
            else
            {
                View.ClientContactList = new List<ClientContactContract>();
            }

        }

        public void GetClientContactAvailibilty()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.ClientContactID;
            var _serviceResponse = _clientContactProxy.GetClientContactAvailibilty(serviceRequest);
            View.ClientAvailibiltyContactList = _serviceResponse.Result;
        }

        public void GetClientContactDocument()
        {
            //ClientContactID
            if (View.ClientContactID != AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.ClientContactID;
                var _serviceResponse = _clientContactProxy.GetClientDocuments(serviceRequest);
                View.UploadedDocumentList = _serviceResponse.Result;
                //View.UploadedDocumentList = ClientContactManager.GetClientDocuments(View.ClientContactID);
            }
        }

        public void SaveClientContact()
        {
            ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>, AppSettingContract> serviceRequest = new ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>, AppSettingContract>();
            serviceRequest.Parameter1 = View.ClientContact;
            serviceRequest.Parameter2 = View.UploadedDocumentList;
            serviceRequest.Parameter3 = View.AppSettingContract;
            var _serviceResponse = _clientContactProxy.SaveClientContact(serviceRequest);
            View.SuccussMessage = _serviceResponse.Result;
        }

        public void IsClientContactAllowedToDelete()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.ClientContactID;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            var _serviceResponse = _clientContactProxy.IsClientContactAllowedToDelete(serviceRequest);
            View.IsClientContactAllowedToDelete = _serviceResponse.Result;
        }

        public void DeleteClientContact()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.ClientContactID;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            var _serviceResponse = _clientContactProxy.DeleteClientContact(serviceRequest);
            View.SuccussMessage = _serviceResponse.Result;
        }

        public void UpdateClientContact()
        {
            ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>> serviceRequest = new ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>>();
            serviceRequest.Parameter1 = View.ClientContact;
            serviceRequest.Parameter2 = View.UploadedDocumentList;
            var _serviceResponse = _clientContactProxy.UpdateClientContact(serviceRequest);
            View.SuccussMessage = _serviceResponse.Result;
        }

        public void GetExistingUserID()
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = View.ClientContactEmailID;
            var _serviceResponse = _clientContactProxy.GetExistingUserID(serviceRequest);
            View.AspNetUserID = _serviceResponse.Result;
        }

        public Boolean IsEmailAlreadyExistForTenant(String emailID)
        {
            ServiceRequest<String, Int32> serviceRequest = new ServiceRequest<String, Int32>();
            serviceRequest.Parameter1 = emailID;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            var _serviceResponse = _clientContactProxy.IsEmailAlreadyExistForTenant(serviceRequest);
            return _serviceResponse.Result;
        }

        #region UAT-4043
        public Boolean ResendInstructorActivationMail(ClientContactContract clientContactToResendActivationMail)
        {
            ServiceRequest<ClientContactContract> serviceRequest = new ServiceRequest<ClientContactContract>();
            serviceRequest.Parameter = clientContactToResendActivationMail;

            var _serviceResponse = _clientContactProxy.ResendInstructorActivationMail(serviceRequest);
            return _serviceResponse.Result;

        }
        #endregion

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region UAT-4120
        /// <summary>
        /// Get Data by Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<String, ApplicantInsituteDataContract> GetDataByKey(String key)
        {
            Object applicationData = ApplicationDataManager.GetObjectDataByKey(key);
            return ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
        }

        /// <summary>
        /// Update Web Application Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.UpdateWebApplicationData(key, serializedData);
        }

        /// <summary>
        /// Add Web Application Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
        }
        #endregion

        public void AddImpersonationHistory(String UserID, Int32 CurrentLoggedInUserID)
        {
            Int32 userId = SecurityManager.GetOrganizationUserInfoByUserId(UserID.ToString()).FirstOrDefault(x => x.IsSharedUser == true).OrganizationUserID;
            SecurityManager.AddImpersonationHistory(userId, CurrentLoggedInUserID);
        }

        #region UAT-4239

        public OrganizationUser GetOrganizationUser(String clientContactUserId)
        {
            Guid userid = new Guid(clientContactUserId);
            return SecurityManager.GetSharedUserOrganizationUser(userid);
        }

        public Boolean ResetPassword(OrganizationUser orgUser)
        {
            if (SecurityManager.UpdateOrganizationUser(ResetUserPassword(orgUser)))
            {
                Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                  {                                                                  
                                                  {EmailFieldConstants.USER_FULL_NAME,orgUser.FirstName+ " " + orgUser.LastName},
                                                      {EmailFieldConstants.PASSWORD,View.Password}
                                                      ,{EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,orgUser.OrganizationUserID}
                    ,{ EmailFieldConstants.INSTITUTION_URL,GetInstitutionURL()} // UAT- 4306
                                                  };
                SecurityManager.PrepareAndSendSystemMail(orgUser.aspnet_Users.aspnet_Membership.Email, contents, CommunicationSubEvents.NOTIFICATION_PASSWORD_RESET_BY_ADMIN, null, true);
            }
            return true;

        }
        private OrganizationUser ResetUserPassword(OrganizationUser orgUser)
        {
            OrganizationUser organizationUser = orgUser;
            organizationUser.aspnet_Users.aspnet_Membership.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.Password, organizationUser.aspnet_Users.aspnet_Membership.PasswordSalt);
            organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = false;
            organizationUser.aspnet_Users.aspnet_Membership.FailedPasswordAttemptCount = AppConsts.NONE;
            organizationUser.aspnet_Users.aspnet_Membership.LastPasswordChangedDate = DateTime.Now;
            organizationUser.IsNewPassword = true;

            //View.EmailAddress = organizationUser.aspnet_Users.aspnet_Membership.Email;
            //View.FirstName = organizationUser.FirstName;
            //View.LastName = organizationUser.LastName;

            return organizationUser;
        }
        #endregion

        #region UAT- 4306
        public string GetInstitutionURL()
        {
            //String institutionURL = WebSiteManager.GetInstitutionUrl(View.SelectedTenantID);
            //return institutionURL;
          String institutionURL =  ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                                        ? String.Empty
                                                        : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);           
            var url = "http://" + String.Format(institutionURL);
            return url;            
        }
        #endregion
    }
}
