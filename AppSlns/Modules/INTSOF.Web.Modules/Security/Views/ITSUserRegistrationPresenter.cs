using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using INTSOF.Utils;
using System.Text.RegularExpressions;
using INTSOF.Utils.Consts;
using System.Security.Cryptography;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Configuration;
using INTSOF.Utils.SsoHandlers;


namespace CoreWeb.IntsofSecurityModel.Views
{
    public class ITSUserRegistrationPresenter : Presenter<IITSUserRegistrationView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IIntsofSecurityModelController _controller;
        // public ITSUserRegistrationPresenter([CreateNew] IIntsofSecurityModelController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads

            GetGender();
            GetCommLang();
            //UAT-3601
            //GetClientSettings();
            //View.AdminProgramStudies = new List<Program>();
        }

        public Boolean IsUsernameExistInTenantDB()
        {
            return SecurityManager.IsUsernameExistInTenantDB(View.LoginUserName, View.SelectedTenantId);
        }

        public bool IsExistsUserName()
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(View.UserName);
            return user != null;
        }

        public bool IsExistsPrimaryEmail()
        {
            string userName = System.Web.Security.Membership.GetUserNameByEmail(View.PrimaryEmail);
            if (userName.IsNullOrEmpty())
            {
                UserAuthRequest tempObject = SecurityManager.GetUserAuthRequestByEmail(View.PrimaryEmail);
                if (tempObject.IsNotNull())
                    return true;
                return false;
            }
            return true;
            //return !string.IsNullOrEmpty(userName);
        }

        public bool IsExistsSSN()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUsersBySSN(View.SSN);
            return organizationUser != null;
        }

        public void GetTenants()
        {
            Boolean SortByName = true;
            String tenantTypeCodeForClient = TenantType.Institution.GetStringValue();
            Int32 defaultTenantId = SecurityManager.DefaultTenantID;
            View.Tenants = SecurityManager.GetTenants(SortByName, false, tenantTypeCodeForClient);

        }

        private void GetGender()
        {
            View.Gender = SecurityManager.GetGender().ToList();
        }

        public Boolean IsValidateUser()
        {
            if (IsExistsUserName())
            {
                //View.ErrorMessage = "This username is already in use. Try another?";
                View.ErrorMessage = View.UsernameAlreadyInUse;
                return false;
            }
            if (IsExistsPrimaryEmail())
            {
                //View.ErrorMessage = "This email address is already in use. Try another?";
                View.ErrorMessage = View.EmailIdAlreadyInUse;
                return false;
            }

            //if (IsExistsSSN())
            //{
            //    View.ErrorMessage = "This SSN is already in use. Try another?";
            //    return false;
            //}
            return true;
        }

        public Boolean AddUser()
        {
            if (IsValidateUser())
            {
                aspnet_Applications application = SecurityManager.GetApplication();
                aspnet_Users aspnetUsers = new aspnet_Users();
                aspnet_Membership memberShip = new aspnet_Membership();
                OrganizationUser organizationUser = new OrganizationUser();
                Organization org = new Organization();
                //DeptProgramMapping dptPrgMap = new DeptProgramMapping();
                String usrVerificationCode = MD5Hash(Guid.NewGuid().ToString());

                aspnetUsers.MobileAlias = View.PrimaryPhone;
                aspnetUsers.LastActivityDate = DateTime.MaxValue;

                // Email address will be used as a username when the Organization is of Supplier, else the username filed value will be used as username.
                aspnetUsers.UserName = View.UserName;
                aspnetUsers.LoweredUserName = aspnetUsers.UserName.ToLower();
                aspnetUsers.ApplicationId = application.ApplicationId;
                aspnetUsers.UserId = Guid.NewGuid();

                memberShip.PasswordSalt = SysXMembershipUtil.GenerateSalt();
                memberShip.Password = SysXMembershipUtil.HashPasswordIWithSalt(View.Password, memberShip.PasswordSalt);
                memberShip.Email = View.PrimaryEmail;
                memberShip.LoweredEmail = memberShip.Email.ToLower();
                memberShip.aspnet_Applications = application;
                memberShip.IsApproved = true;
                memberShip.PasswordFormat = AppConsts.ONE;
                memberShip.CreateDate = DateTime.Now;
                memberShip.LastLockoutDate = DateTime.Now.AddDays(-1);
                memberShip.LastLoginDate = DateTime.Now.AddDays(-1);
                memberShip.LastPasswordChangedDate = DateTime.Now.AddDays(-1);
                memberShip.FailedPasswordAttemptWindowStart = DateTime.Now.AddDays(-1);
                memberShip.FailedPasswordAnswerAttemptWindowStart = DateTime.Now.AddDays(-1);
                memberShip.IsLockedOut = false;
                aspnetUsers.aspnet_Membership = memberShip;

                organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(View.SelectedTenantId);
                organizationUser.aspnet_Users = aspnetUsers;
                organizationUser.FirstName = View.FirstName;
                organizationUser.MiddleName = View.MiddleName;
                organizationUser.LastName = View.LastName;
                organizationUser.PrimaryEmailAddress = View.PrimaryEmail;
                organizationUser.SecondaryEmailAddress = View.SecondaryEmail;
                //organizationUser.Alias1 = View.Alias1;
                //organizationUser.Alias2 = View.Alias2;
                //organizationUser.Alias3 = View.Alias3;
                organizationUser.IsNewPassword = false;
                organizationUser.CreatedOn = DateTime.Now;
                organizationUser.ModifiedOn = null;
                organizationUser.IsDeleted = false;

                //UAT-2853:UCONN SSO Account activation requirement
                if (View.IsShibbolethLogin || View.IsAutoActive)
                {
                    organizationUser.IsActive = true;
                    organizationUser.ActiveDate = DateTime.Now;
                }
                else
                {
                    organizationUser.IsActive = false;
                }
                organizationUser.IsApplicant = true;
                organizationUser.IsOutOfOffice = false;
                organizationUser.IgnoreIPRestriction = true;
                organizationUser.IsMessagingUser = true;
                organizationUser.IsSystem = false;
                organizationUser.IsMessagingUser = false;
                organizationUser.PhotoName = View.FilePath;
                organizationUser.OriginalPhotoName = View.OriginalFileName;
                organizationUser.DOB = View.DOB;
                organizationUser.SSN = View.SSN;
                organizationUser.Gender = View.SelectedGenderId;
                organizationUser.PhoneNumber = View.PrimaryPhone;
                organizationUser.SecondaryPhone = View.SecondaryPhone;
                organizationUser.UserVerificationCode = usrVerificationCode;
                //organizationUser.CreatedByID = View.CurrentUserId;
                organizationUser.AddressHandle = GetAddressHandle();
                //UAT-2447
                organizationUser.IsInternationalPhoneNumber = View.IsMaskingOfPrimaryPhoneNumber;
                organizationUser.IsInternationalSecondaryPhone = View.IsMaskingOfSecondaryPhoneNumber;
                //CBI|| CABS || Add Suffix ID in BillingAddressID
                //  organizationUser.UserTypeID = View.SelectedSuffixID.IsNullOrEmpty() ? (Int32?)null : View.SelectedSuffixID;
                if (View.Suffix.IsNullOrEmpty() && (View.SelectedSuffixID==0 || View.SelectedSuffixID==null))
                {
                    organizationUser.UserTypeID = null;
                }
                else
                {
                    if (View.IsSuffixDropDownType)
                    {
                        organizationUser.UserTypeID = View.SelectedSuffixID.Value;
                    }
                    else
                    {
                        organizationUser.UserTypeID = GetSuffixIdBasedOnSuffixText(View.Suffix);
                    }
                }
                //Adds and updates the Person Alias.
                AddUpdatePersonAlias(organizationUser);


                SecurityManager.AddOrganizationUser(organizationUser, aspnetUsers);

                //Add Current Resident History
                AddCurrentResidentialHistory(organizationUser);

                SecurityManager.AddOrganizationUserProfile(organizationUser, View.SelectedCommLang);
                // Sets default subscription for user
                SetDefaultSubscription(organizationUser.OrganizationUserID);

                //Get Website Url
                Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(organizationUser.Organization.Tenant.TenantID);
                String applicationUrl = String.Empty;
                if (webSite.IsNotNull() && webSite.WebSiteID != SysXDBConsts.NONE)
                {
                    applicationUrl = webSite.URL;
                }
                else
                {
                    webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
                    applicationUrl = webSite.URL;
                }
                String applicationUrlWithoutVerification = applicationUrl;//UAT-2958
                applicationUrl = applicationUrl + "/Login.aspx?UsrVerCode=" + usrVerificationCode;


                // ClientComplianceManagementManager.AddPackageSubscription(organizationUser.OrganizationUserID, organizationUser.Organization.TenantID.Value);
                String[] emptyArray = new String[0];
                List<Int32> productId = SecurityManager.GetProductsForTenant(organizationUser.Organization.TenantID.Value).Select(obj => obj.TenantProductID).ToList();
                List<String> defaultRoledetailIds = SecurityManager.getDefaultRoleDetailIds(productId);
                SecurityManager.SaveMappingOfRolesWithSelectedUser(organizationUser, defaultRoledetailIds, emptyArray, View.Password);
                SecurityManager.SetDefaultBusinessChannel(organizationUser.OrganizationUserID);
                //if (!SecurityManager.SendEmailForNewApplicant(organizationUser, applicationUrl, View.Password))
                //{
                //    View.ErrorMessage = "Some error has occured.Please contact administrator.";
                //}
                #region UAT-2515 - AceMapp Login integration
                if (!View.ExternalId.IsNullOrEmpty() && View.IntegrationClientId > AppConsts.NONE)
                    SecurityManager.InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(organizationUser.OrganizationUserID, View.IntegrationClientId, View.ExternalId);
                #endregion

                //UAT-2958
                if (View.IsShibbolethLogin && View.IsRandomGeneratedPassword)
                {
                    // SendEmail Applicant Account Creation through SSO
                    SecurityManager.SendEmailForNewApplicantThroughSSO(organizationUser, applicationUrlWithoutVerification, View.Password);
                }

                //UAT-2792
                if (View.IsShibbolethLogin && !View.ShibbolethUniqueIdentifier.IsNullOrEmpty())
                {
                    SecurityManager.ShibbolethPeopleSoftIDEntryInIntegrationClientOrganizationUserMap(organizationUser.OrganizationUserID, View.IntegrationClientId, View.ShibbolethUniqueIdentifier);
                }

                //UAT-2853:UCONN SSO Account activation requirement
                if (!View.IsShibbolethLogin)
                {
                    //UAT - 1037 :  Send account confirmation email (the one with the activation link) twice
                    Boolean ismailSentSuccessfullyFirstTime = SecurityManager.SendEmailForNewApplicant(organizationUser, applicationUrl, View.Password);
                    Boolean ismailSentSuccessfullySecondTime = SecurityManager.SendEmailForNewApplicant(organizationUser, applicationUrl, View.Password);

                    if (!ismailSentSuccessfullyFirstTime && !ismailSentSuccessfullySecondTime)
                    {
                        View.ErrorMessage = "Some error has occured.Please contact administrator.";
                    }
                }

                ComplianceDataManager.AddUpdateProfileCustomAttributes(View.SaveCustomAttributeList, organizationUser.OrganizationUserID, organizationUser.OrganizationUserID, View.SelectedTenantId);
                return true;
            }
            return false;
        }

        private AddressHandle GetAddressHandle()
        {
            Guid addressHandleId = Guid.NewGuid();

            AddressHandle addressHandle = new AddressHandle();
            addressHandle.AddressHandleID = addressHandleId;
            addressHandle.Addresses = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Address>();
            addressHandle.Addresses.Add(new Address()
            {

                AddressHandleID = addressHandleId,
                Address1 = View.Address1,
                Address2 = View.Address2,
                ZipCodeID = View.ZipId,
                CreatedByID = 0,
                IsActive = true,
                CreatedOn = DateTime.Now
            });
            if (View.ZipId == 0)
            {
                addressHandle.Addresses.FirstOrDefault().AddressExts = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<AddressExt>();
                addressHandle.Addresses.FirstOrDefault().AddressExts.Add(new AddressExt()
                {
                    //AddressExt addressExtNew = null;
                    //addressExtNew = new AddressExt();
                    AE_CountryID = View.IsLocationServiceTenant ? AppConsts.ONE : View.CountryId,//UAT-3910 
                    AE_StateName = View.StateName,
                    AE_CityName = View.CityName,
                    AE_ZipCode = View.PostalCode,
                    AE_County = View.IsLocationServiceTenant ? Convert.ToString(View.CountryId) : null,//UAT-3910
                });
            }


            return addressHandle;
        }

        public void GetWebsiteTenantId()
        {
            View.SelectedTenantId = WebSiteManager.GetWebsiteTenantId(View.WebsiteUrl);
        }

        public Boolean IsExistingUser()
        {
            List<LookupContract> lstExistingUser = SecurityManager.GetExistingUserLists(View.UserName, View.DOB == null ? DateTime.Now : View.DOB.Value, View.SSN, View.FirstName, View.LastName, true, View.PrimaryEmail, languageCode: View.LanguageCode);
            if (lstExistingUser.Count > 0)
            {
                View.ExistingUsersList = lstExistingUser;
                return true;
            }
            return false;
        }

        /// <summary>
        /// It validates the user, and then redirect to login page.
        /// </summary>
        public void ValidateUserNameAndPassword()
        {
            SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(View.LoginUserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;

            if (System.Web.Security.Membership.ValidateUser(Regex.Replace(View.LoginUserName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE), Regex.Replace(View.LoginPassword, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)))
            {
                // Checks if the user is locked.
                if (!user.IsNull() && user.IsLockedOut)
                {
                    View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED);
                    View.ExistingOrganisationUser = null;
                }
                SecurityManager.ResetPasswordAttempCount(View.LoginUserName);
                View.ExistingOrganisationUser = SecurityManager.GetOrganizationUser(user.OrganizationUserId);
            }
            else
            {
                View.LoginErrorMessage = String.Empty;
                SecurityManager.FailedPasswordAttemptCount(View.LoginUserName, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempt"]));
                //View.setSubmitbuttonText = "Try Again";
                View.setSubmitbuttonText = View.TryAgain;
                if (!user.IsNull())
                {
                    OrganizationUser orgUser = SecurityManager.GetOrganizationUser(user.OrganizationUserId);

                    if (user.IsNewPassword)
                    {
                        View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_PWD_RECEIVED_IN_EMAIL);
                    }
                    else if (!orgUser.IsActive)
                    {
                        View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ERROR_INACTIVE_ACCOUNT);
                    }
                    else
                    {
                        //View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
                        View.ErrorMessage = View.InvalidUsernamePswd;
                    }
                }
                else
                {
                    //View.LoginErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_USRNAME_PWD);
                    View.ErrorMessage = View.InvalidUsernamePswd;
                }
                View.ExistingOrganisationUser = null;
            }
        }

        public Boolean AddLinkedUserProfile()
        {
            OrganizationUser organizationUser = new OrganizationUser();
            String usrVerificationCode = MD5Hash(Guid.NewGuid().ToString());
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(View.SelectedTenantId);
            organizationUser.UserID = View.ExistingOrganisationUser.UserID;
            organizationUser.FirstName = !String.IsNullOrEmpty(View.FirstName) ? View.FirstName : View.ExistingOrganisationUser.FirstName;
            organizationUser.MiddleName = !String.IsNullOrEmpty(View.MiddleName) ? View.MiddleName : View.ExistingOrganisationUser.MiddleName;
            organizationUser.LastName = !String.IsNullOrEmpty(View.LastName) ? View.LastName : View.ExistingOrganisationUser.LastName;
            organizationUser.PrimaryEmailAddress = View.PrimaryEmail;
            organizationUser.SecondaryEmailAddress = View.SecondaryEmail;
            //organizationUser.Alias1 = View.Alias1;
            //organizationUser.Alias2 = View.Alias2;
            //organizationUser.Alias3 = View.Alias3;
            organizationUser.IsNewPassword = false;
            organizationUser.CreatedOn = DateTime.Now;
            organizationUser.ModifiedOn = null;
            organizationUser.IsDeleted = false;
            organizationUser.IsActive = true;
            organizationUser.IsApplicant = true;
            organizationUser.IsOutOfOffice = false;
            organizationUser.IgnoreIPRestriction = true;
            organizationUser.IsMessagingUser = true;
            organizationUser.IsSystem = false;
            organizationUser.IsMessagingUser = false;
            organizationUser.PhotoName = View.FilePath;
            organizationUser.OriginalPhotoName = View.OriginalFileName;
            organizationUser.DOB = View.DOB;
            organizationUser.SSN = View.SSN;
            organizationUser.Gender = View.SelectedGenderId > 0 ? View.SelectedGenderId : View.ExistingOrganisationUser.Gender;
            organizationUser.PhoneNumber = View.PrimaryPhone;
            organizationUser.SecondaryPhone = View.SecondaryPhone;
            organizationUser.UserVerificationCode = usrVerificationCode;
            //organizationUser.CreatedByID = View.CurrentUserId;
            organizationUser.AddressHandle = GetAddressHandle();
            organizationUser.IsInternationalPhoneNumber = View.IsMaskingOfPrimaryPhoneNumber;
            organizationUser.IsInternationalSecondaryPhone = View.IsMaskingOfSecondaryPhoneNumber;

            //UAT-887: WB: Delay Automatic emails going out after activation
            if (organizationUser.ActiveDate == null && organizationUser.IsApplicant == true && organizationUser.IsActive == true)
                organizationUser.ActiveDate = DateTime.Now;

            //Adds and updates the Person Alias.
            AddUpdatePersonAlias(organizationUser);

            SecurityManager.AddOrganizationUser(organizationUser);

            //Add Current Resident History
            AddCurrentResidentialHistory(organizationUser);

            SecurityManager.AddOrganizationUserProfile(organizationUser, View.SelectedCommLang);
            // Sets default subscription for user
            SetDefaultSubscription(organizationUser.OrganizationUserID);



            //Get Website Url
            Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(organizationUser.Organization.Tenant.TenantID);
            String applicationUrl = String.Empty;
            if (webSite.IsNotNull() && webSite.WebSiteID != SysXDBConsts.NONE)
            {
                applicationUrl = webSite.URL;
            }
            else
            {
                webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
                applicationUrl = webSite.URL;
            }
            applicationUrl = applicationUrl + "/Login.aspx";


            // ClientComplianceManagementManager.AddPackageSubscription(organizationUser.OrganizationUserID, organizationUser.Organization.TenantID.Value);
            String[] emptyArray = new String[0];
            List<Int32> productId = SecurityManager.GetProductsForTenant(organizationUser.Organization.TenantID.Value).Select(obj => obj.TenantProductID).ToList();
            List<String> defaultRoledetailIds = SecurityManager.getDefaultRoleDetailIds(productId);
            SecurityManager.SaveMappingOfRolesWithSelectedUser(organizationUser, defaultRoledetailIds, emptyArray, View.LoginPassword, false);
            SecurityManager.SetDefaultBusinessChannel(organizationUser.OrganizationUserID);


            #region UAT-2515 - AceMapp Login integration
            if (!View.ExternalId.IsNullOrEmpty() && View.IntegrationClientId > AppConsts.NONE)
                SecurityManager.InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(organizationUser.OrganizationUserID, View.IntegrationClientId, View.ExternalId);
            #endregion

            //UAT-2792
            if (View.IsShibbolethLogin && !View.ShibbolethUniqueIdentifier.IsNullOrEmpty())
            {
                SecurityManager.ShibbolethPeopleSoftIDEntryInIntegrationClientOrganizationUserMap(organizationUser.OrganizationUserID, View.IntegrationClientId, View.ShibbolethUniqueIdentifier);
            }

            //UAT-1218 - No need to synchronize profile and docs while APPLICANT linked with (ADB ADMINS) or (CLIENT ADMIN) or (SHARED USER)
            if ((View.ExistingOrganisationUser.IsApplicant ?? false) == true)
            {
                SecurityManager.SynchoniseUserProfile(View.ExistingOrganisationUser.OrganizationUserID, View.ExistingOrganisationUser.Organization.TenantID.Value, View.ExistingOrganisationUser.OrganizationUserID);
                SecurityManager.SynchoniseUserDocuments(View.ExistingOrganisationUser.OrganizationUserID, View.ExistingOrganisationUser.Organization.TenantID.Value, View.ExistingOrganisationUser.OrganizationUserID);
            }
            //SecurityManager.RotationDataMovementOnAccountLinking(View.ExistingOrganisationUser.UserID, View.CurrentUserId); //UAT-4407:- Currently we will not copy data on linking
            if (!SecurityManager.SendEmailForInstitutionChange(organizationUser, applicationUrl))
            {
                View.ErrorMessage = "Some error has occured.Please contact administrator.";
            }
            return true;
        }
        #region UAT-2515 - AceMapp Login integration
        public void InsertAceMappLoginIntegrationEntry(Int32 organizationUserID)
        {
            if (!View.ExternalId.IsNullOrEmpty() && View.IntegrationClientId > AppConsts.NONE)
                SecurityManager.InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(organizationUserID, View.IntegrationClientId, View.ExternalId);
        }
        #endregion
        // TODO: Handle other view events and set state in the view

        #region Set Default Subscription

        /// <summary>
        /// 
        /// </summary>
        Int32 notificationCommunicationTypeId = 0;
        private Int32 NotificationCommunicationTypeId
        {
            get
            {
                if (notificationCommunicationTypeId == 0)
                    notificationCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.NOTIFICATION.GetStringValue()).CommunicationTypeID;
                return notificationCommunicationTypeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 alertCommunicationTypeId = 0;
        private Int32 AlertCommunicationTypeId
        {
            get
            {
                if (alertCommunicationTypeId == 0)
                    alertCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.ALERTS.GetStringValue()).CommunicationTypeID;
                return alertCommunicationTypeId;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 reminderCommunicationTypeId = 0;
        private Int32 ReminderCommunicationTypeId
        {
            get
            {
                if (reminderCommunicationTypeId == 0)
                    reminderCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.REMINDERS.GetStringValue()).CommunicationTypeID;
                return reminderCommunicationTypeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        private void SetDefaultSubscription(Int32 organizationUserId)
        {

            List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
            List<UserCommunicationSubscriptionSetting> mappedSubscriptionSettings = null;
            IEnumerable<lkpCommunicationEvent> communicationEvents = null;

            communicationEvents = CommunicationManager.GetCommunicationEvents(AlertCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, organizationUserId, AlertCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            communicationEvents = CommunicationManager.GetCommunicationEvents(NotificationCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, organizationUserId, NotificationCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            communicationEvents = CommunicationManager.GetCommunicationEvents(ReminderCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, organizationUserId, ReminderCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            if (userCommunicationSubscriptionSettings != null && userCommunicationSubscriptionSettings.Count > 0)
                CommunicationManager.AddUserCommunicationSubscriptionSettings(userCommunicationSubscriptionSettings);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="ById"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="communicationEvents"></param>
        /// <returns></returns>
        private List<UserCommunicationSubscriptionSetting> GetMappedUserCommunicationSubscriptionSettings(
            Int32 organizationUserId,
            Int32 ById,
            Int32 communicationTypeId,
            IEnumerable<lkpCommunicationEvent> communicationEvents)
        {
            List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings = null;
            if (communicationEvents != null && communicationEvents.Count() > 0)
            {
                userCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
                foreach (lkpCommunicationEvent communicationEvent in communicationEvents)
                {
                    userCommunicationSubscriptionSettings.Add(new UserCommunicationSubscriptionSetting()
                    {
                        OrganizationUserID = organizationUserId,
                        CommunicationTypeID = communicationTypeId,
                        CommunicationEventID = communicationEvent.CommunicationEventID,
                        IsSubscribedToAdmin = true,
                        IsSubscribedToUser = true,
                        CreatedByID = ById,
                        CreatedOn = DateTime.Now,
                        ModifiedByID = ById,
                        ModifiedOn = DateTime.Now
                    });
                }
            }
            return userCommunicationSubscriptionSettings;
        }
        #endregion

        private string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        //personal alias
        private void AddUpdatePersonAlias(OrganizationUser organizationUser)
        {
            if (View.PersonAliasList.IsNotNull())
            {
                List<PersonAlia> currentAliasList = organizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
                foreach (PersonAliasContract tempPersonAlias in View.PersonAliasList)
                {
                    if (tempPersonAlias.ID > 0)
                    {
                        PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_ID == tempPersonAlias.ID);
                        if (personAlias.IsNotNull())
                        {
                            personAlias.PA_FirstName = tempPersonAlias.FirstName;
                            personAlias.PA_LastName = tempPersonAlias.LastName;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            personAlias.PA_MiddleName = tempPersonAlias.MiddleName.IsNullOrEmpty() ? View.NoMiddleNameText : tempPersonAlias.MiddleName;
                            personAlias.PA_ModifiedBy = AppConsts.NONE;
                            personAlias.PA_ModifiedOn = DateTime.Now;

                            //CBI|| CABS ||
                            //Edit
                            if (View.IsLocationServiceTenant && !personAlias.PersonAliasExtensions.IsNullOrEmpty() && !tempPersonAlias.IsNullOrEmpty())
                            {

                                PersonAliasExtension personAliasExtension = personAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                                if (!personAliasExtension.IsNullOrEmpty())
                                {
                                    personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;//!tempPersonAlias.SuffixID.IsNullOrEmpty() && tempPersonAlias.SuffixID > AppConsts.NONE ? View.lstSuffixes.Where(cond => cond.SuffixID == tempPersonAlias.SuffixID).FirstOrDefault().Suffix : null;
                                    personAliasExtension.PAE_ModifiedBy = AppConsts.NONE;
                                    personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                }

                            }
                        }
                    }
                    else
                    {
                        PersonAlia personAlias = new PersonAlia();
                        personAlias.PA_FirstName = tempPersonAlias.FirstName;
                        personAlias.PA_LastName = tempPersonAlias.LastName;
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        personAlias.PA_MiddleName = tempPersonAlias.MiddleName.IsNullOrEmpty() ? View.NoMiddleNameText : tempPersonAlias.MiddleName;
                        personAlias.PA_IsDeleted = false;
                        personAlias.PA_CreatedBy = AppConsts.NONE;
                        personAlias.PA_CreatedOn = DateTime.Now;
                        personAlias.PA_AliasIdentifier = Guid.NewGuid();
                        organizationUser.PersonAlias.Add(personAlias);

                        //Added 
                        if (View.IsLocationServiceTenant && !tempPersonAlias.IsNullOrEmpty() && !tempPersonAlias.Suffix.IsNullOrEmpty())
                        {
                            PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                            personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;//!tempPersonAlias.SuffixID.IsNullOrEmpty() && tempPersonAlias.SuffixID > AppConsts.NONE ? View.lstSuffixes.Where(cond => cond.SuffixID == tempPersonAlias.SuffixID).FirstOrDefault().Suffix : null;
                            personAliasExtension.PAE_CreatedBy = AppConsts.NONE;
                            personAliasExtension.PAE_CreatedOn = DateTime.Now;

                            personAlias.PersonAliasExtensions.Add(personAliasExtension);
                        }

                    }
                }
                List<Int32> aliasIDToBeDeleted = currentAliasList.Select(x => x.PA_ID).Except(View.PersonAliasList.Select(y => y.ID)).ToList();
                foreach (Int32 delAliasID in aliasIDToBeDeleted)
                {
                    PersonAlia delAlias = currentAliasList.FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == delAliasID);
                    delAlias.PA_IsDeleted = true;
                    delAlias.PA_ModifiedBy = AppConsts.NONE;
                    delAlias.PA_ModifiedOn = DateTime.Now;

                    //to delete 
                    if (View.IsLocationServiceTenant && !delAlias.PersonAliasExtensions.IsNullOrEmpty())
                    {
                        PersonAliasExtension delPersonAliasExtension = delAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                        if (!delPersonAliasExtension.IsNullOrEmpty())
                        {
                            delPersonAliasExtension.PAE_IsDeleted = true;
                            delPersonAliasExtension.PAE_ModifiedBy = AppConsts.NONE;
                            delPersonAliasExtension.PAE_ModifiedOn = DateTime.Now;
                        }
                    }
                }
            }
        }


        public Dictionary<String, Object> GetAddressDataDictionary()
        {
            Dictionary<String, Object> dicAddressData = new Dictionary<String, Object>();
            dicAddressData.Add("address1", View.Address1);
            dicAddressData.Add("address2", View.Address2);
            dicAddressData.Add("zipcodeid", View.ZipId);
            return dicAddressData;
        }

        private void AddCurrentResidentialHistory(OrganizationUser organizationUser)
        {
            //Current residential Address
            // ResidentialHistory currentResedentialHistory = organizationUser.ResidentialHistories.FirstOrDefault(x => x.RHI_IsCurrentAddress == true && x.RHI_IsDeleted == false);

            ResidentialHistory currentResedentialHistory = new ResidentialHistory();
            currentResedentialHistory.RHI_IsCurrentAddress = true;
            currentResedentialHistory.RHI_IsPrimaryResidence = false;
            //currentResedentialHistory.RHI_ResidenceStartDate = View.DateResidentFrom;
            currentResedentialHistory.RHI_IsDeleted = false;
            currentResedentialHistory.RHI_CreatedByID = AppConsts.NONE;
            currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
            currentResedentialHistory.RHI_OrganizationUserID = organizationUser.OrganizationUserID;
            //currentResedentialHistory.Address = addressNew == null ? organizationUser.AddressHandle.Addresses.FirstOrDefault() : addressNew;
            currentResedentialHistory.RHI_AddressId = organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressID;
            currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
            organizationUser.ResidentialHistories.Add(currentResedentialHistory);
            SecurityManager.UpdateOrganizationUser(organizationUser);

        }

        public void GetSSNSetting()
        {
            View.IsSSNDisabled = (View.SelectedTenantId > 0 && View.SelectedTenantId != SecurityManager.DefaultTenantID) ? ComplianceDataManager.GetSSNSetting(View.SelectedTenantId, Setting.DISABLE_SSN.GetStringValue()) : false;
        }

        #region UAT-2515
        public void GetMatchingOrganizationUserDetails()
        {
            INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract objExternalLoginDataContract = new INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract();
            objExternalLoginDataContract.FirstName = View.FirstName;
            objExternalLoginDataContract.LastName = View.LastName;
            objExternalLoginDataContract.DOB = Convert.ToDateTime(View.DOB);
            objExternalLoginDataContract.SSN = View.SSN;
            objExternalLoginDataContract.Email1 = View.PrimaryEmail;
            objExternalLoginDataContract.Email2 = View.SecondaryEmail;
            objExternalLoginDataContract.UserName = View.UserName;
            objExternalLoginDataContract.TenantID = View.SelectedTenantId;
            objExternalLoginDataContract.IntegrationClientID = View.IntegrationClientId;

            //UAT - 4159    CORE account linking
            if (!String.IsNullOrEmpty(View.MappingCode) && View.MappingCode == AppConsts.CORE_ACCOUNT_LINKING_MAPPING_GROUP_CODE)
            {
                View.matchingUserList = SecurityManager.GetMatchingOrganisationUserListForCoreLinking(objExternalLoginDataContract);
            }
            else
            {
                //Get Matching user list
                View.matchingUserList = SecurityManager.GetMatchingOrganisationUserList(objExternalLoginDataContract);
            }
            if (View.matchingUserList.Count > AppConsts.NONE)
            {
                View.ExistingUsersList = View.matchingUserList.Select(d => new LookupContract
                {
                    Code = d.UserName,
                    ID = d.TenantID,//Here we need tenant id
                    Name = View.IamText + " " + d.FirstName + " " + d.LastName + " " + View.FromText + " " + d.TenantName + "'.",
                    UserID = d.OrganizationUserID
                }).ToList();
            }
            //  Name = "I am '" + d.FirstName + " " + d.LastName + "' from '" + d.TenantName + "'."
        }
        #endregion

        #region UAT-2792
        public void GetMatchingUsersForShibboleth(String EmailID, Int32 TenantID, Boolean isApplicant)
        {
            //TODO
            View.matchingUserList = SecurityManager.GetMatchingUsersForShibboleth(View.ShibbolethUniqueIdentifier, View.ShibbolethAttributeId, EmailID, TenantID, isApplicant, View.ShibbolethHandlerType, View.ShibbolethFirstName, View.ShibbolethLastName,"");
            if (View.matchingUserList.Count > AppConsts.NONE)
            {
                View.ExistingUsersList = View.matchingUserList.Select(d => new LookupContract
                {
                    Code = d.UserName,
                    ID = d.TenantID,//Here we need tenant id
                    Name = View.IamText + " " + d.FirstName + " " + d.LastName + " " + View.FromText + " " + d.TenantName + "'.",
                    UserID = d.OrganizationUserID,
                    IsFirstLogin = d.IsFirstLogin
                }).ToList();
            }

            //Name = "I am '" + d.FirstName + " " + d.LastName + "' from '" + d.TenantName + "'.",
        }

        public Int32 GetTenantIDByURL(String Host)
        {
            return WebSiteManager.GetWebsiteTenantId(Host);
        }
        #endregion

        //UAT-2883
        public void GetWebApplicationData(String key)
        {
            Object userSsoData = ApplicationDataManager.GetObjectDataByKey(key);
            if (userSsoData != null)
            {
                Dictionary<String, SsoHandlerContract> userData = ApplicationDataManager.DeserializeDictionaryValues<SsoHandlerContract>(userSsoData);
                SsoHandlerContract appData = userData.GetValue("UserSsoData");
                if (appData.IsNotNull())
                {
                    View.ShibbolethEmail = appData.Email;
                    View.ShibbolethUniqueIdentifier = appData.UniqueID;
                    View.ShibbolethAttributeId = appData.AtributeID;
                    View.ShibbolethFirstName = appData.FirstName;
                    View.ShibbolethLastName = appData.LastName;
                    View.ShibbolethUserName = appData.UserName;
                    View.ShibbolethHandlerType = appData.HandlerType;
                    View.ShibbolethHost = appData.Host;
                    View.ShibbolethRoleString = appData.Role;//UAT-3540
                    if (!appData.Role.IsNullOrEmpty() && appData.HandlerType != AppConsts.SHIBBOLETH_UPENN && appData.HandlerType != AppConsts.SHIBBOLETH_UPENN_DENTAL)
                    {
                        appData.Role = appData.Role.ToLower();
                        View.lstShibbolethRole = new List<String>(appData.Role.Split(';'));
                    }
                    //UAT-3272
                    else if (!appData.Role.IsNullOrEmpty() && (appData.HandlerType == AppConsts.SHIBBOLETH_UPENN || appData.HandlerType == AppConsts.SHIBBOLETH_UPENN_DENTAL))
                    {
                        appData.Role = appData.Role.ToLower();
                        View.lstShibbolethRole = new List<String>(appData.Role.ToLower().Split(':'));
                    }  
                    
                    View.IntegrationClientId = appData.IntegrationClientID;
                    if (!appData.AttributesWithID.IsNullOrEmpty())
                    {
                        View.AttributesWithID = appData.AttributesWithID;//UAT-3607
                    }
                }
                ApplicationDataManager.RemoveWebApplicationData(key);
            }
        }

        #region UAT-2958
        public void IsRandomGeneratedPassword()
        {
            if (!View.ShibbolethHandlerType.IsNullOrEmpty())
            {
                View.IsRandomGeneratedPassword = SecurityManager.IsRandomGeneratedPassword(View.ShibbolethHandlerType);
            }
        }

        public void GenerateRandomPassword()
        {
            // Define default min and max password lengths.
            int minLength = 8;
            int maxLength = 15;

            // Define supported password characters divided into groups.
            // You can add (or remove) characters to (from) these groups.
            string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
            string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            string PASSWORD_CHARS_NUMERIC = "23456789";
            string PASSWORD_CHARS_SPECIAL = "@#$%^-+~!?\':/,(){}[]_";
            // string PASSWORD_CHARS_SPECIAL1 = "$-+?_=!%{}/"; 

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][]
                {
                    PASSWORD_CHARS_LCASE.ToCharArray(),
                    PASSWORD_CHARS_UCASE.ToCharArray(),
                    PASSWORD_CHARS_NUMERIC.ToCharArray(),
                    PASSWORD_CHARS_SPECIAL.ToCharArray()
                };

            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            //return new String(password);
            View.RandomGeneratedPassword = new String(password);

        }

        #endregion

        #region UAT-3540
        public Boolean CheckRoleForShibbolethNYU(Boolean IsApplicantRoleCheck)
        {
            return SecurityManager.CheckRoleForShibbolethNYU(IsApplicantRoleCheck, View.ShibbolethRoleString);
        }
        #endregion


        public Boolean IsAutoActivateAndLogin()
        {

            Entity.ClientEntity.ClientSetting clientSetting = ComplianceDataManager.GetClientSetting(View.SelectedTenantId, Setting.Auto_Activate_and_Login.GetStringValue());
            if (!clientSetting.IsNullOrEmpty())
            {
                if (clientSetting.CS_SettingValue == "1")
                    return true;
                return false;
            }
            return false;
        }


        #region UAT-3601
        public void GetClientSettings()
        {
            List<String> lstCodes = new List<String>();
            lstCodes.Add(Setting.APPLICANT_IS_PASSWORD_RETAIN.GetStringValue());
            List<Entity.ClientEntity.ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(View.SelectedTenantId, lstCodes);
            var _setting = lstClientSetting.FirstOrDefault(x => x.lkpSetting.Code == Setting.APPLICANT_IS_PASSWORD_RETAIN.GetStringValue());
            if (_setting.IsNullOrEmpty())
            {
                View.IsPasswordRetain = false;
            }
            else
            {
                View.IsPasswordRetain = _setting.CS_SettingValue == "1";
            }
        }
        #endregion


        public Boolean CheckRoleForShibbolethUCONN(Boolean IsApplicantRoleCheck)
        {
            return SecurityManager.CheckRoleForShibbolethUCONN(IsApplicantRoleCheck, View.ShibbolethRoleString);
        }


        //CBI || CABS  || Check whether the tenant is location service tenant or not.
        public void IsLocationServiceTenant()
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.SelectedTenantId);
        }

        public void GetSuffixes()
        {
            View.lstSuffixes = new List<lkpSuffix>();
            View.lstSuffixes = SecurityManager.GetSuffixes();
        }

        public void GetCommLang()
        {
            View.CommLanguage = SecurityManager.GetCommLang().ToList();
        }
        public int GetSuffixIdBasedOnSuffixText(string suffix)
        {
            return SecurityManager.GetSuffixIdBasedOnSuffixText(suffix);
        }
        public void IsDropDownSuffixType()
        {
            AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.SUFFIX_TYPE.ToString());
            View.IsSuffixDropDownType= !appConfig.IsNullOrEmpty() && appConfig.AC_Value == "1" ? true : false;
        }
        //Release 181:4998
        public Boolean CheckRoleForShibbolethNSC(Boolean IsApplicantRoleCheck)
        {
            return SecurityManager.CheckRoleForShibbolethNSC(IsApplicantRoleCheck, View.ShibbolethRoleString);
        }

        public Boolean CheckRoleForShibbolethRoss(Boolean IsApplicantRoleCheck)
        {
            return SecurityManager.CheckRoleForShibbolethRoss(IsApplicantRoleCheck, View.ShibbolethRoleString);
        }
        public Boolean CheckRoleForShibbolethUpennDental(Boolean IsApplicantRoleCheck)
        {
            return SecurityManager.CheckRoleForShibbolethUpennDental(IsApplicantRoleCheck, View.ShibbolethRoleString);
        }
        public Boolean CheckRoleForShibbolethBSU(Boolean IsApplicantRoleCheck)
        {
            return SecurityManager.CheckRoleForShibbolethBSU(IsApplicantRoleCheck, View.ShibbolethRoleString);
        }
    }
}




