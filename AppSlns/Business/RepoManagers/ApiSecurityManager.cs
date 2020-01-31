using Entity;

using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.UI.Contract.MobileAPI;
using INTSOF.UI.Contract.SystemSetUp;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.RepoManagers
{
    public static class ApiSecurityManager
    {
        #region Property
        /// <summary>
        /// 
        /// </summary>
        static int alertCommunicationTypeId = 0;
        static private int AlertCommunicationTypeId
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
        static int notificationCommunicationTypeId = 0;
        static private int NotificationCommunicationTypeId
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
        static int reminderCommunicationTypeId = 0;
        static private int ReminderCommunicationTypeId
        {
            get
            {
                if (reminderCommunicationTypeId == 0)
                    reminderCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.REMINDERS.GetStringValue()).CommunicationTypeID;
                return reminderCommunicationTypeId;
            }
        }

        #endregion

        #region User Registeration Methods

        public static bool CreateAccount(UserContract user)
        {
            user.IsAutoActive = IsAutoActivateAndLogin(user.TenantID);


            aspnet_Applications application = SecurityManager.GetApplication();
            aspnet_Users aspnetUsers = new aspnet_Users();
            aspnet_Membership memberShip = new aspnet_Membership();
            OrganizationUser organizationUser = new OrganizationUser();
            Organization org = new Organization();
            string usrVerificationCode = MD5Hash(Guid.NewGuid().ToString());

            aspnetUsers.MobileAlias = user.PrimaryPhone;
            aspnetUsers.LastActivityDate = DateTime.MaxValue;

            // Email address will be used as a username when the Organization is of Supplier, else the username filed value will be used as username.
            aspnetUsers.UserName = user.UserName;
            aspnetUsers.LoweredUserName = aspnetUsers.UserName.ToLower();
            aspnetUsers.ApplicationId = application.ApplicationId;
            aspnetUsers.UserId = Guid.NewGuid();

            memberShip.PasswordSalt = SysXMembershipUtil.GenerateSalt();
            memberShip.Password = SysXMembershipUtil.HashPasswordIWithSalt(user.Password, memberShip.PasswordSalt);
            memberShip.Email = user.PrimaryEmail;
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

            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(user.TenantID);
            organizationUser.aspnet_Users = aspnetUsers;
            organizationUser.FirstName = user.FirstName;
            organizationUser.MiddleName = user.MiddleName;
            organizationUser.LastName = user.LastName;
            organizationUser.PrimaryEmailAddress = user.PrimaryEmail;
            organizationUser.SecondaryEmailAddress = user.SecondaryEmail;
            organizationUser.IsNewPassword = false;
            organizationUser.CreatedOn = DateTime.Now;
            organizationUser.ModifiedOn = null;
            organizationUser.IsDeleted = false;

            if (user.IsAutoActive)
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
            organizationUser.PhotoName = user.FilePath;
            organizationUser.OriginalPhotoName = user.OriginalFileName;
            organizationUser.DOB = user.DOB;
            organizationUser.SSN = user.SSN.IsNotNull() ? user.SSN.Replace("-", "") : string.Empty;
            organizationUser.Gender = user.SelectedGenderId;
            organizationUser.PhoneNumber = user.PrimaryPhone;
            organizationUser.SecondaryPhone = user.SecondaryPhone;
            organizationUser.UserVerificationCode = usrVerificationCode;
            organizationUser.AddressHandle = GetAddressHandle(user);
            organizationUser.IsInternationalPhoneNumber = user.IsMaskingOfPrimaryPhoneNumber;
            organizationUser.IsInternationalSecondaryPhone = user.IsMaskingOfSecondaryPhoneNumber;
            //organizationUser.UserTypeID = user.SelectedSuffixID.IsNullOrEmpty() ? (Int32?)null : user.SelectedSuffixID;
            Boolean IsSuffixDropDown = ApiSecurityManager.IsSuffixDropDown();
            if (IsSuffixDropDown)
            {
                organizationUser.UserTypeID = user.SelectedSuffixID > 0 ? user.SelectedSuffixID : (Int32?)null;
            }
            if (!IsSuffixDropDown)
            {
                organizationUser.UserTypeID = GetSuffixIdBasedOnSuffixText(user.Suffix.IsNullOrEmpty() ? string.Empty : user.Suffix);
            }
            //Adds and updates the Person Alias.
            AddUpdatePersonAlias(organizationUser, user);


            SecurityManager.AddOrganizationUser(organizationUser, aspnetUsers);

            //Add Current Resident History
            AddCurrentResidentialHistory(organizationUser);

            SecurityManager.AddOrganizationUserProfile(organizationUser, user.SelectedCommLang);
            // Sets default subscription for user
            SetDefaultSubscription(organizationUser.OrganizationUserID);

            //Get Website Url
            Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(organizationUser.Organization.Tenant.TenantID);
            string applicationUrl = string.Empty;
            if (webSite.IsNotNull() && webSite.WebSiteID != SysXDBConsts.NONE)
            {
                applicationUrl = webSite.URL;
            }
            else
            {
                webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
                applicationUrl = webSite.URL;
            }
            string applicationUrlWithoutVerification = applicationUrl;
            applicationUrl = applicationUrl + "/Login.aspx?UsrVerCode=" + usrVerificationCode;


            string[] emptyArray = new string[0];
            List<int> productId = SecurityManager.GetProductsForTenant(organizationUser.Organization.TenantID.Value).Select(obj => obj.TenantProductID).ToList();
            List<string> defaultRoledetailIds = SecurityManager.getDefaultRoleDetailIds(productId);
            SecurityManager.SaveMappingOfRolesWithSelectedUser(organizationUser, defaultRoledetailIds, emptyArray, user.Password);
            SecurityManager.SetDefaultBusinessChannel(organizationUser.OrganizationUserID);

            // send applicant creation EMail twice
            SecurityManager.SendEmailForNewApplicant(organizationUser, applicationUrlWithoutVerification, user.Password);
            SecurityManager.SendEmailForNewApplicant(organizationUser, applicationUrlWithoutVerification, user.Password);

            return true;
        }

        public static Boolean AddLinkedUserProfile(UserContract user, OrganizationUser ExistingOrganisationUser)
        {
            OrganizationUser organizationUser = new OrganizationUser();
            String usrVerificationCode = MD5Hash(Guid.NewGuid().ToString());
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(user.TenantID);
            organizationUser.UserID = ExistingOrganisationUser.UserID;
            organizationUser.FirstName = !String.IsNullOrEmpty(user.FirstName) ? user.FirstName : ExistingOrganisationUser.FirstName;
            organizationUser.MiddleName = !String.IsNullOrEmpty(user.MiddleName) ? user.MiddleName : ExistingOrganisationUser.MiddleName;
            organizationUser.LastName = !String.IsNullOrEmpty(user.LastName) ? user.LastName : ExistingOrganisationUser.LastName;
            organizationUser.PrimaryEmailAddress = user.PrimaryEmail;
            organizationUser.SecondaryEmailAddress = user.SecondaryEmail;
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
            organizationUser.PhotoName = user.FilePath;
            organizationUser.OriginalPhotoName = user.OriginalFileName;
            organizationUser.DOB = user.DOB;
            organizationUser.SSN = user.SSN;
            organizationUser.Gender = user.SelectedGenderId > 0 ? user.SelectedGenderId : ExistingOrganisationUser.Gender;
            organizationUser.PhoneNumber = user.PrimaryPhone;
            organizationUser.SecondaryPhone = user.SecondaryPhone;
            organizationUser.UserVerificationCode = usrVerificationCode;
            //organizationUser.CreatedByID = View.CurrentUserId;
            organizationUser.AddressHandle = GetAddressHandle(user);
            organizationUser.IsInternationalPhoneNumber = user.IsMaskingOfPrimaryPhoneNumber;
            organizationUser.IsInternationalSecondaryPhone = user.IsMaskingOfSecondaryPhoneNumber;

            //UAT-887: WB: Delay Automatic emails going out after activation
            if (organizationUser.ActiveDate == null && organizationUser.IsApplicant == true && organizationUser.IsActive == true)
                organizationUser.ActiveDate = DateTime.Now;

            //Adds and updates the Person Alias.
            AddUpdatePersonAlias(organizationUser, user);

            SecurityManager.AddOrganizationUser(organizationUser);

            //Add Current Resident History
            AddCurrentResidentialHistory(organizationUser);

            SecurityManager.AddOrganizationUserProfile(organizationUser, user.SelectedCommLang);
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
            SecurityManager.SaveMappingOfRolesWithSelectedUser(organizationUser, defaultRoledetailIds, emptyArray, user.Password, false);
            SecurityManager.SetDefaultBusinessChannel(organizationUser.OrganizationUserID);


            //#region UAT-2515 - AceMapp Login integration
            //if (!View.ExternalId.IsNullOrEmpty() && View.IntegrationClientId > AppConsts.NONE)
            //    SecurityManager.InsertExternalUserRegistrationLogInIntegrationClientOrganizationUserMap(organizationUser.OrganizationUserID, View.IntegrationClientId, View.ExternalId);
            //#endregion

            ////UAT-2792
            //if (View.IsShibbolethLogin && !View.ShibbolethUniqueIdentifier.IsNullOrEmpty())
            //{
            //    SecurityManager.ShibbolethPeopleSoftIDEntryInIntegrationClientOrganizationUserMap(organizationUser.OrganizationUserID, View.IntegrationClientId, View.ShibbolethUniqueIdentifier);
            //}

            //UAT-1218 - No need to synchronize profile and docs while APPLICANT linked with (ADB ADMINS) or (CLIENT ADMIN) or (SHARED USER)
            if ((ExistingOrganisationUser.IsApplicant ?? false) == true)
            {
                SecurityManager.SynchoniseUserProfile(ExistingOrganisationUser.OrganizationUserID, ExistingOrganisationUser.Organization.TenantID.Value, ExistingOrganisationUser.OrganizationUserID);
                SecurityManager.SynchoniseUserDocuments(ExistingOrganisationUser.OrganizationUserID, ExistingOrganisationUser.Organization.TenantID.Value, ExistingOrganisationUser.OrganizationUserID);
            }
            if (!SecurityManager.SendEmailForInstitutionChange(organizationUser, applicationUrl))
            {
                //View.ErrorMessage = "Some error has occured.Please contact administrator.";
                return false;
            }
            return true;
        }

        public static bool IsExistsUserName(string UserName)
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(UserName);
            return user != null;
        }
        public static bool IsAutoActivateAndLogin(int tenantID)
        {

            Entity.ClientEntity.ClientSetting clientSetting = ComplianceDataManager.GetClientSetting(tenantID, Setting.Auto_Activate_and_Login.GetStringValue());
            if (!clientSetting.IsNullOrEmpty())
            {
                if (clientSetting.CS_SettingValue == "1")
                    return true;
                return false;
            }
            return false;
        }

        public static List<LookupContract> IsExistingUser(UserContract user)
        {
            List<LookupContract> lstExistingUser = SecurityManager.GetExistingUserLists(user.UserName, user.DOB == null ? DateTime.Now : user.DOB.Value, user.SSN, user.FirstName, user.LastName, true, user.PrimaryEmail);
            if (lstExistingUser.Count > 0)
            {
                return lstExistingUser;
            }
            return new List<LookupContract>();
        }

        public static bool IsExistsPrimaryEmail(string primaryEmail)
        {
            string userName = System.Web.Security.Membership.GetUserNameByEmail(primaryEmail);
            if (userName.IsNullOrEmpty())
            {
                UserAuthRequest tempObject = SecurityManager.GetUserAuthRequestByEmail(primaryEmail);
                if (tempObject.IsNotNull())
                    return true;
                return false;
            }
            return true;
            //return !string.IsNullOrEmpty(userName);
        }

        private static string MD5Hash(string text)
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

        private static AddressHandle GetAddressHandle(UserContract user)
        {
            Guid addressHandleId = Guid.NewGuid();

            AddressHandle addressHandle = new AddressHandle();
            addressHandle.AddressHandleID = addressHandleId;
            addressHandle.Addresses = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Address>();
            addressHandle.Addresses.Add(new Address()
            {

                AddressHandleID = addressHandleId,
                Address1 = user.Address,
                Address2 = string.Empty,
                ZipCodeID = user.ZipId,
                CreatedByID = 0,
                IsActive = true,
                CreatedOn = DateTime.Now
            });

            if (user.ZipId == 0)
            {
                addressHandle.Addresses.FirstOrDefault().AddressExts = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<AddressExt>();
                addressHandle.Addresses.FirstOrDefault().AddressExts.Add(new AddressExt()
                {
                    AE_CountryID = AppConsts.ONE,
                    AE_StateName = user.StateName,
                    AE_CityName = user.CityName,
                    AE_ZipCode = user.PostalCode,
                    AE_County = Convert.ToString(user.CountryId),
                });
            }


            return addressHandle;
        }



        private static void AddUpdatePersonAlias(OrganizationUser organizationUser, UserContract user)
        {
            if (user.PersonAliasList.IsNotNull())
            {
                List<PersonAlia> currentAliasList = organizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
                foreach (PersonAliasContract tempPersonAlias in user.PersonAliasList)
                {
                    if (tempPersonAlias.ID > 0)
                    {
                        PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_ID == tempPersonAlias.ID);
                        if (personAlias.IsNotNull())
                        {
                            personAlias.PA_FirstName = tempPersonAlias.FirstName;
                            personAlias.PA_LastName = tempPersonAlias.LastName;
                            personAlias.PA_MiddleName = tempPersonAlias.MiddleName.IsNullOrEmpty() ? string.Empty : tempPersonAlias.MiddleName;
                            personAlias.PA_ModifiedBy = AppConsts.NONE;
                            personAlias.PA_ModifiedOn = DateTime.Now;


                            //CBI || CABS
                            //if (user.IsLocationServiceTenant)
                            //{
                            if (!personAlias.PersonAliasExtensions.IsNullOrEmpty() 
                                && personAlias.PersonAliasExtensions.Any(Cond => !Cond.PAE_IsDeleted))
                            {
                                PersonAliasExtension personAliasExtension = personAlias.PersonAliasExtensions.FirstOrDefault(Cond => !Cond.PAE_IsDeleted);
                                if (!personAliasExtension.IsNullOrEmpty())
                                {
                                    if (!tempPersonAlias.IsNullOrEmpty())
                                    {
                                        personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;
                                        personAliasExtension.PAE_ModifiedBy = user.OrganizationUserID;
                                        personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                    }
                                    else
                                    {
                                        personAliasExtension.PAE_IsDeleted = true;
                                        personAliasExtension.PAE_ModifiedBy = user.OrganizationUserID;
                                        personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                    }
                                }
                            }
                            else
                            {
                                if (!tempPersonAlias.IsNullOrEmpty() && !tempPersonAlias.Suffix.IsNullOrEmpty())
                                {
                                    PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                                    personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;
                                    personAliasExtension.PAE_CreatedBy = user.OrganizationUserID;
                                    personAliasExtension.PAE_CreatedOn = DateTime.Now;
                                    personAlias.PersonAliasExtensions.Add(personAliasExtension);
                                }
                            }
                            //  }
                        }
                    }
                    else
                    {
                        PersonAlia personAlias = new PersonAlia();
                        personAlias.PA_FirstName = tempPersonAlias.FirstName;
                        personAlias.PA_LastName = tempPersonAlias.LastName;
                        personAlias.PA_MiddleName = tempPersonAlias.MiddleName.IsNullOrEmpty() ? string.Empty : tempPersonAlias.MiddleName;
                        personAlias.PA_IsDeleted = false;
                        personAlias.PA_CreatedBy = AppConsts.NONE;
                        personAlias.PA_CreatedOn = DateTime.Now;
                        personAlias.PA_AliasIdentifier = Guid.NewGuid();
                        organizationUser.PersonAlias.Add(personAlias);

                        //Added 
                        //if (user.IsLocationServiceTenant && !tempPersonAlias.IsNullOrEmpty() && !tempPersonAlias.Suffix.IsNullOrEmpty())
                        if (!tempPersonAlias.IsNullOrEmpty() && !tempPersonAlias.Suffix.IsNullOrEmpty())
                        {
                            PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                            personAliasExtension.PAE_Suffix = tempPersonAlias.Suffix;
                            personAliasExtension.PAE_CreatedBy = AppConsts.NONE;
                            personAliasExtension.PAE_CreatedOn = DateTime.Now;

                            personAlias.PersonAliasExtensions.Add(personAliasExtension);
                        }

                    }
                }
                List<int> aliasIDToBeDeleted = currentAliasList.Select(x => x.PA_ID).Except(user.PersonAliasList.Select(y => y.ID)).ToList();
                foreach (int delAliasID in aliasIDToBeDeleted)
                {
                    PersonAlia delAlias = currentAliasList.FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == delAliasID);
                    delAlias.PA_IsDeleted = true;
                    delAlias.PA_ModifiedBy = AppConsts.NONE;
                    delAlias.PA_ModifiedOn = DateTime.Now;

                    //to delete 
                    // if (user.IsLocationServiceTenant && !delAlias.PersonAliasExtensions.IsNullOrEmpty())
                    if (!delAlias.PersonAliasExtensions.IsNullOrEmpty())
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

        private static void AddCurrentResidentialHistory(OrganizationUser organizationUser)
        {
            ResidentialHistory currentResedentialHistory = new ResidentialHistory();
            currentResedentialHistory.RHI_IsCurrentAddress = true;
            currentResedentialHistory.RHI_IsPrimaryResidence = false;
            currentResedentialHistory.RHI_IsDeleted = false;
            currentResedentialHistory.RHI_CreatedByID = AppConsts.NONE;
            currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
            currentResedentialHistory.RHI_OrganizationUserID = organizationUser.OrganizationUserID;
            currentResedentialHistory.RHI_AddressId = organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressID;
            currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
            organizationUser.ResidentialHistories.Add(currentResedentialHistory);
            SecurityManager.UpdateOrganizationUser(organizationUser);
        }

        private static void SetDefaultSubscription(int organizationUserId)
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

        private static List<UserCommunicationSubscriptionSetting> GetMappedUserCommunicationSubscriptionSettings(int organizationUserId, int ById,
                                                              int communicationTypeId, IEnumerable<lkpCommunicationEvent> communicationEvents)
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

        public static String ValidateUserViaEmailAndRedirect(String verificationCode)
        {
            OrganizationUser orgUser = SecurityManager.GetOrganizationUserByVerificationCode(verificationCode);
            String verficationMessage = String.Empty;
            if (orgUser.IsNotNull())
            {
                //UAT-2494, New Account verification enhancements (additional verification step)
                Int32 tenantID = Convert.ToInt32(orgUser.Organization.TenantID);
                String settingCode = Setting.ACCOUNT_VERIFICATION_PROCESS_MAIN.GetStringValue();
                //Get permission for Additional Account Verification, If any value is YES then Show Additional Step.
                Boolean isAdditionAccVerificationSettingEnabled = ComplianceDataManager.GetClientSetting(tenantID, settingCode).IsNullOrEmpty() ? false : (ComplianceDataManager.GetClientSetting(tenantID, settingCode).CS_SettingValue == "1" ? true : false);
                if (isAdditionAccVerificationSettingEnabled)
                {
                    verficationMessage = "ShowAdditionalAccountVerificationPage";
                }
                else
                {
                    orgUser.IsActive = true;
                    if (orgUser.ActiveDate == null && orgUser.IsApplicant == true)
                        orgUser.ActiveDate = DateTime.Now;
                    SecurityManager.UpdateOrganizationUser(orgUser);
                    verficationMessage = ResourceConst.SECURITY_VERIFICATION_SUCCESS_MESSAGE;
                }
            }
            return verficationMessage;
        }

        public static Dictionary<String, String> GetAccountVerificationSettings(Int32 tenantID, String verificationCode)
        {
            String SSN;
            Int32 OrganizationUserID;
            Boolean AccountVerificationMainSetting;
            Boolean AccVerificationProcessResponseReqdSetting;
            Boolean AccVerificationProcessDOBSetting;
            Boolean AccVerificationProcessSSNSetting;
            Boolean AccVerificationProcessLSSNSetting;
            String AccVerificationProcessDOBTextSetting;
            Boolean AccVerificationProcessProfCustAttrSetting;
            String AccVerificationProcessSSNTextSetting;
            String AccVerificationProcessLSSNTextSetting;
            String AccVerificationProcessProfCustAttrTextSetting;

            OrganizationUser orgUser = SecurityManager.GetOrganizationUserByVerificationCode(verificationCode);
            if (orgUser.IsNotNull())
            {
                SSN = SecurityManager.GetFormattedString(orgUser.OrganizationUserID, false);
                OrganizationUserID = orgUser.OrganizationUserID;
            }

            List<Entity.ClientEntity.ClientSetting> lstAllClientSettings = ComplianceDataManager.GetClientSetting(tenantID);
            Entity.ClientEntity.ClientSetting accVerificationProcessMainSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_MAIN.GetStringValue()).FirstOrDefault();

            if (!accVerificationProcessMainSetting.IsNullOrEmpty())
            {
                AccountVerificationMainSetting = accVerificationProcessMainSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                AccountVerificationMainSetting = false;
            }

            Entity.ClientEntity.ClientSetting accVerificationProcessResponseReqdSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_RESPONSE_REQD.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessResponseReqdSetting.IsNullOrEmpty())
            {
                AccVerificationProcessResponseReqdSetting = accVerificationProcessResponseReqdSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                AccVerificationProcessResponseReqdSetting = false;
            }

            Entity.ClientEntity.ClientSetting accVerificationProcessDOBSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessDOBSetting.IsNullOrEmpty())
            {
                AccVerificationProcessDOBSetting = accVerificationProcessDOBSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                AccVerificationProcessDOBSetting = false;
            }

            Entity.ClientEntity.ClientSetting accVerificationProcessSSNSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessSSNSetting.IsNullOrEmpty())
            {
                AccVerificationProcessSSNSetting = accVerificationProcessSSNSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                AccVerificationProcessSSNSetting = false;
            }

            Entity.ClientEntity.ClientSetting accVerificationProcessLSSNSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessLSSNSetting.IsNullOrEmpty())
            {
                AccVerificationProcessLSSNSetting = accVerificationProcessLSSNSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                AccVerificationProcessLSSNSetting = false;
            }

            Entity.ClientEntity.ClientSetting accVerificationProcessProfCustAttrSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_PROF_CUST_ATTR_PERMISSION.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessProfCustAttrSetting.IsNullOrEmpty())
            {
                AccVerificationProcessProfCustAttrSetting = accVerificationProcessProfCustAttrSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                AccVerificationProcessProfCustAttrSetting = false;
            }

            Entity.ClientEntity.ClientSetting accVerificationProcessDOBTextSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_OVERRIDE_TEXT.GetStringValue()).FirstOrDefault();

            AccVerificationProcessDOBTextSetting = accVerificationProcessDOBTextSetting.IsNullOrEmpty() ? String.Empty : accVerificationProcessDOBTextSetting.CS_SettingValue;

            Entity.ClientEntity.ClientSetting accVerificationProcessSSNTextSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_OVERRIDE_TEXT.GetStringValue()).FirstOrDefault();

            AccVerificationProcessSSNTextSetting = accVerificationProcessSSNTextSetting.IsNullOrEmpty() ? String.Empty : accVerificationProcessSSNTextSetting.CS_SettingValue;

            Entity.ClientEntity.ClientSetting accVerificationProcessLSSNTextSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_OVERRIDE_TEXT.GetStringValue()).FirstOrDefault();
            AccVerificationProcessLSSNTextSetting = accVerificationProcessLSSNTextSetting.IsNullOrEmpty() ? String.Empty : accVerificationProcessLSSNTextSetting.CS_SettingValue;

            Entity.ClientEntity.ClientSetting accVerificationProcessProfCustAttrTextSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_PROF_CUST_ATTR_OVERRIDE_TEXT.GetStringValue()).FirstOrDefault();

            AccVerificationProcessProfCustAttrTextSetting = accVerificationProcessProfCustAttrTextSetting.IsNullOrEmpty() ? String.Empty : accVerificationProcessProfCustAttrTextSetting.CS_SettingValue;

            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic.Add("AccountVerificationMainSetting", Convert.ToString(AccountVerificationMainSetting));
            dic.Add("AccVerificationProcessResponseReqdSetting", AccVerificationProcessResponseReqdSetting.ToString());
            dic.Add("AccVerificationProcessDOBSetting", AccVerificationProcessDOBSetting.ToString());
            dic.Add("AccVerificationProcessSSNSetting", AccVerificationProcessSSNSetting.ToString());
            dic.Add("AccVerificationProcessLSSNSetting", AccVerificationProcessLSSNSetting.ToString());
            dic.Add("AccVerificationProcessDOBTextSetting", AccVerificationProcessDOBTextSetting);
            dic.Add("AccVerificationProcessProfCustAttrSetting", AccVerificationProcessProfCustAttrSetting.ToString());
            dic.Add("AccVerificationProcessSSNTextSetting", AccVerificationProcessSSNTextSetting);
            dic.Add("AccVerificationProcessLSSNTextSetting", AccVerificationProcessLSSNTextSetting);
            dic.Add("AccVerificationProcessProfCustAttrTextSetting", AccVerificationProcessProfCustAttrTextSetting);

            return dic;
        }

        public static List<Entity.ClientEntity.lkpSetting> GetAccountVerficationQuestions(Int32 tenantID, String verificationCode)
        {

            Dictionary<String, String> lstofPermission = GetAccountVerificationSettings(tenantID, verificationCode);

            List<String> lstCodes = new List<String>();
            List<ClientSettingCustomAttributeContract> lstClientSettingcustAttributes = new List<ClientSettingCustomAttributeContract>();

            Boolean AccVerificationProcessDOBSetting;
            Boolean AccVerificationProcessSSNSetting;
            Boolean AccVerificationProcessLSSNSetting;
            String AccVerificationProcessDOBTextSetting;
            Boolean AccVerificationProcessProfCustAttrSetting;
            String AccVerificationProcessSSNTextSetting;
            String AccVerificationProcessLSSNTextSetting;

            lstofPermission.TryGetValue("AccVerificationProcessDOBSetting", out AccVerificationProcessDOBSetting);
            lstofPermission.TryGetValue("AccVerificationProcessSSNSetting", out AccVerificationProcessSSNSetting);
            lstofPermission.TryGetValue("AccVerificationProcessLSSNSetting", out AccVerificationProcessLSSNSetting);
            lstofPermission.TryGetValue("AccVerificationProcessProfCustAttrSetting", out AccVerificationProcessProfCustAttrSetting);
            lstofPermission.TryGetValue("AccVerificationProcessDOBTextSetting", out AccVerificationProcessDOBTextSetting);
            lstofPermission.TryGetValue("AccVerificationProcessSSNTextSetting", out AccVerificationProcessSSNTextSetting);
            lstofPermission.TryGetValue("AccVerificationProcessLSSNTextSetting", out AccVerificationProcessLSSNTextSetting);

            if (AccVerificationProcessDOBSetting)
            {
                lstCodes.Add(Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue());
            }
            if (AccVerificationProcessSSNSetting)
            {
                lstCodes.Add(Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue());
            }
            if (AccVerificationProcessLSSNSetting)
            {
                lstCodes.Add(Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue());
            }
            if (AccVerificationProcessProfCustAttrSetting)
            {
                lstClientSettingcustAttributes = GetClientSettingCustomAttribute(tenantID);
            }
            List<Entity.ClientEntity.lkpSetting> lstQuestions = ComplianceDataManager.GetLkpSetting(tenantID).Where(x => lstCodes.Contains(x.Code) && !x.IsDeleted).ToList();

            if (!lstClientSettingcustAttributes.IsNullOrEmpty())
            {
                foreach (ClientSettingCustomAttributeContract custAttr in lstClientSettingcustAttributes)
                {
                    Entity.ClientEntity.lkpSetting setting = new Entity.ClientEntity.lkpSetting();
                    if (custAttr.SettingValue)
                    {
                        setting.Name = custAttr.SettingOverrideText.IsNullOrEmpty() ? custAttr.SettingName : custAttr.SettingOverrideText;
                        setting.Code = Convert.ToString(custAttr.CustomAttributeID);
                        lstQuestions.Add(setting);
                    }
                }
            }

            foreach (Entity.ClientEntity.lkpSetting item in lstQuestions)
            {
                if (item.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue())
                {
                    item.Name = AccVerificationProcessDOBTextSetting.IsNullOrEmpty() ? "Date of Birth" : AccVerificationProcessDOBTextSetting;
                }
                if (item.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue())
                {
                    item.Name = AccVerificationProcessSSNTextSetting.IsNullOrEmpty() ? "SSN" : AccVerificationProcessSSNTextSetting;
                }
                if (item.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue())
                {
                    item.Name = AccVerificationProcessLSSNTextSetting.IsNullOrEmpty() ? "Last four SSN" : AccVerificationProcessLSSNTextSetting;
                }
            }

            return lstQuestions;
        }

        public static List<ClientSettingCustomAttributeContract> GetClientSettingCustomAttribute(Int32 tenantID)
        {

            List<ClientSettingCustomAttributeContract> LstProfileCustomAttributeOverride = ComplianceDataManager.GetClientSettingCustomAttribute(tenantID).ToList();
            List<Entity.ClientEntity.TypeCustomAttributes> lstTypeCustomAttributes = ComplianceDataManager.GetCustomAttributes(0, 0, CustomAttributeUseTypeContext.Profile.GetStringValue(), 0, tenantID);

            Dictionary<Int32, String> map = LstProfileCustomAttributeOverride.Where(cond => cond.SettingValue).ToDictionary(x => x.CustomAttributeID, x => (x.SettingOverrideText.IsNullOrEmpty() ? x.SettingName : x.SettingOverrideText));

            List<ClientSettingCustomAttributeContract> lstfinalCustomAttributes = new List<ClientSettingCustomAttributeContract>();
            foreach (var item in lstTypeCustomAttributes)
            {
                if (map.ContainsKey(item.CAId))
                {
                    ClientSettingCustomAttributeContract custattribute = new ClientSettingCustomAttributeContract();
                    custattribute.CustomAttributeID = item.CAId;
                    custattribute.SettingName = map.GetValue(item.CAId);
                    custattribute.SettingOverrideText = String.Empty;
                    custattribute.SettingValue = true;
                    custattribute.CustomAttributeDatatypeCode = item.CADataTypeCode;
                    custattribute.IsRequired = item.IsRequired.Value;
                    custattribute.ValidateExpression = item.RegularExpression;
                    custattribute.ValidationMessage = item.RegExpErrorMsg;
                    lstfinalCustomAttributes.Add(custattribute);
                }
            }
            return lstfinalCustomAttributes;
        }

        public static Boolean ValidateandActivateUser(Int32 tenantID, UserContract user, List<AttributesForCustomFormContract> lstCustomAttributes, String verificationCode, String PermissionCode)
        {
            Boolean AccountVerificationMainSetting;
            Boolean AccVerificationProcessResponseReqdSetting;
            Boolean AccVerificationProcessDOBSetting;
            Boolean AccVerificationProcessSSNSetting;
            Boolean AccVerificationProcessLSSNSetting;
            String AccVerificationProcessDOBTextSetting;
            Boolean AccVerificationProcessProfCustAttrSetting;
            String AccVerificationProcessSSNTextSetting;
            String AccVerificationProcessLSSNTextSetting;
            String AccVerificationProcessProfCustAttrTextSetting;
            OrganizationUser orgUser = SecurityManager.GetOrganizationUserByVerificationCode(verificationCode);
            Dictionary<String, String> settings = GetAccountVerificationSettings(tenantID, verificationCode);
            settings.TryGetValue("AccountVerificationMainSetting", out AccountVerificationMainSetting);
            settings.TryGetValue("AccVerificationProcessResponseReqdSetting", out AccVerificationProcessResponseReqdSetting);
            settings.TryGetValue("AccVerificationProcessDOBSetting", out AccVerificationProcessDOBSetting);
            settings.TryGetValue("AccVerificationProcessSSNSetting", out AccVerificationProcessSSNSetting);
            settings.TryGetValue("AccVerificationProcessLSSNSetting", out AccVerificationProcessLSSNSetting);
            settings.TryGetValue("AccVerificationProcessDOBTextSetting", out AccVerificationProcessDOBTextSetting);
            settings.TryGetValue("AccVerificationProcessProfCustAttrSetting", out AccVerificationProcessProfCustAttrSetting);
            settings.TryGetValue("AccVerificationProcessSSNTextSetting", out AccVerificationProcessSSNTextSetting);
            settings.TryGetValue("AccVerificationProcessLSSNTextSetting", out AccVerificationProcessLSSNTextSetting);
            settings.TryGetValue("AccVerificationProcessProfCustAttrTextSetting", out AccVerificationProcessProfCustAttrTextSetting);
            Boolean isValidUser = false;
            String SSN = String.Empty;
            if (orgUser.IsNotNull())
            {
                SSN = SecurityManager.GetFormattedString(orgUser.OrganizationUserID, false);
            }

            if (AccVerificationProcessResponseReqdSetting)
            {
                if (AccVerificationProcessDOBSetting)
                {
                    //Check DOB
                    if (orgUser.DOB.IsNotNull() && orgUser.DOB.Value.Date == user.DOB.Value.Date)
                    {
                        isValidUser = true;
                    }
                    else
                    {
                        return isValidUser = false;
                    }
                }
                if (AccVerificationProcessSSNSetting)
                {
                    //Check SSN
                    if (!SSN.IsNullOrEmpty() && SSN == user.SSN.Replace("-", ""))
                    {
                        isValidUser = true;
                    }
                    else
                    {
                        return isValidUser = false;
                    }
                }

                if (AccVerificationProcessLSSNSetting)
                {
                    //Check Last Four SSN
                    if (orgUser.SSNL4.IsNotNull() && orgUser.SSNL4 == user.SSNL4)
                    {
                        isValidUser = true;
                    }
                    else
                    {
                        return isValidUser = false;
                    }
                }
                if (AccVerificationProcessProfCustAttrSetting)
                {
                    List<Entity.ClientEntity.TypeCustomAttributes> lstCustomAttrUserData = ComplianceDataManager.GetCustomAttributes(AppConsts.NONE, AppConsts.NONE, CustomAttributeUseTypeContext.Profile.GetStringValue(), orgUser.OrganizationUserID, tenantID);
                    if (!lstCustomAttrUserData.IsNullOrEmpty())
                    {
                        foreach (var newProfAttrEnteredData in lstCustomAttributes)
                        {
                            Entity.ClientEntity.TypeCustomAttributes prevProfAttrEnteredData = lstCustomAttrUserData.Where(x => x.CAId == newProfAttrEnteredData.AttributeId).FirstOrDefault();
                            if (prevProfAttrEnteredData.IsNotNull() && prevProfAttrEnteredData.CAValue == newProfAttrEnteredData.AttributeDataValue)
                            {
                                isValidUser = true;
                            }
                            else
                            {
                                return isValidUser = false;
                            }
                        }
                    }
                }

            }
            else
            {
                if (PermissionCode == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue())
                {

                    if (orgUser.DOB.IsNotNull() && orgUser.DOB.Value.Date == user.DOB.Value.Date)
                    {
                        isValidUser = true;
                    }
                }
                if (PermissionCode == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue())
                {
                    if (!SSN.IsNullOrEmpty() && SSN == user.SSN.Replace("-", ""))
                    {
                        isValidUser = true;
                    }
                }
                if (PermissionCode == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue())
                {
                    if (orgUser.SSNL4.IsNotNull() && orgUser.SSNL4 == user.SSNL4)
                    {
                        isValidUser = true;
                    }
                }
                int selectedCustomAttrID;
                if (Int32.TryParse(PermissionCode, out selectedCustomAttrID))
                {
                    List<Entity.ClientEntity.TypeCustomAttributes> lstCustomAttrUserData = ComplianceDataManager.GetCustomAttributes(AppConsts.NONE, AppConsts.NONE, CustomAttributeUseTypeContext.Profile.GetStringValue(), orgUser.OrganizationUserID, tenantID);
                    if (!lstCustomAttrUserData.IsNullOrEmpty())
                    {
                        foreach (var newProfAttrEnteredData in lstCustomAttributes)
                        {
                            Entity.ClientEntity.TypeCustomAttributes prevProfAttrEnteredData = lstCustomAttrUserData.Where(x => x.CAId == newProfAttrEnteredData.AttributeId).FirstOrDefault();
                            if (prevProfAttrEnteredData.IsNotNull() && prevProfAttrEnteredData.CAValue == newProfAttrEnteredData.AttributeDataValue)
                            {
                                isValidUser = true;
                            }
                            else
                            {
                                return isValidUser = false;
                            }
                        }
                    }
                }
            }

            if (isValidUser)
            {
                orgUser.IsActive = true;
                if (orgUser.ActiveDate == null && orgUser.IsApplicant == true)
                    orgUser.ActiveDate = DateTime.Now;

                SecurityManager.UpdateOrganizationUser(orgUser);
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region Order Flow Methonds

        public static Dictionary<string, string> IsValidCBIUniqueID(int TenantID, string CBIUniqueID)
        {
            return FingerPrintDataManager.ValidateCBIUniqueID(TenantID, CBIUniqueID);
        }

        public static Dictionary<int, int> GetLocationHierarchy(int tenantID, int locationID)
        {
            return FingerPrintDataManager.GetLocationHierarchy(tenantID, locationID);
        }

        public static List<BackgroundPackagesContract> GetBackgroundPackages(string xmlDPMIds, int orgainizatuionUserId, int tenantId, bool IslocationServiceTenant)
        {
            return ComplianceDataManager.GetBackgroundPackages(xmlDPMIds, orgainizatuionUserId, tenantId , IslocationServiceTenant);
        }

        public static List<LocationContract> GetApplicantAvailableLocation(int tenantId, string lng, string lat,string orderRequestType=null)
        {
            return FingerPrintSetUpManager.GetApplicantAvailableLocation(tenantId, lng, lat,orderRequestType).Take(20).ToList();
        }
        public static List<LocationContract> GetValidateEventCodeStatusAndEventDetails(int TenantId, FingerPrintAppointmentContract fingerPrintAppointmentContract)
        {
            if (!fingerPrintAppointmentContract.IsNullOrEmpty())
            {
                return FingerPrintSetUpManager.GetValidateEventCodeStatusAndEventDetails(fingerPrintAppointmentContract, TenantId).ToList();
            }
            else
            {
                return new List<LocationContract>();
            }

        }

        public static CustomFormDataContract GetCustomAttributes(int tenantID, int packageID, string CBIUniqueID, string LangCode)
        {
            return FingerPrintDataManager.GetCustomAttributes(tenantID, packageID, CBIUniqueID, LangCode);
        }

        public static List<LookupContract> GetCustomAttributeOptionsData(int tenantID, string attributeName)
        {
            return BackgroundProcessOrderManager.GetCustomAttributeOptionsData(tenantID, attributeName);
        }

        public static List<string> GetCascadingAttributeData(int tenantID, int attributeGroupID, int attributeId, string SearchID)
        {
            return BackgroundProcessOrderManager.GetDataForBindingCascadingDropDown(tenantID, attributeGroupID, attributeId, SearchID);
        }


        public static List<CustomFormAutoFillDataContract> GetConditionsforCustomAttributes(int tenantID, StringBuilder xmlStringData, string langCode)
        {
            return BackgroundProcessOrderManager.GetConditionsforAttributes(tenantID, xmlStringData, langCode);
        }

        public static UserContract GetOrganizationUser(int tenantID, int organizationUserID)
        {
            UserContract userDetailsContract = new UserContract();
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUserDetailByOrganizationUserId(organizationUserID);
            if (!organizationUser.IsNullOrEmpty())
            {

                userDetailsContract.FirstName = organizationUser.FirstName;
                userDetailsContract.MiddleName = organizationUser.MiddleName;
                userDetailsContract.LastName = organizationUser.LastName;
                userDetailsContract.DOB = organizationUser.DOB;
                userDetailsContract.OrganizationUserID = organizationUser.OrganizationUserID;
                userDetailsContract.PrimaryEmail = organizationUser.PrimaryEmailAddress;
                userDetailsContract.SecondaryEmail = organizationUser.SecondaryEmailAddress;
                userDetailsContract.PswdRecoveryEmail = organizationUser.aspnet_Users.aspnet_Membership.Email;
                userDetailsContract.PrimaryPhone = organizationUser.PhoneNumber;
                userDetailsContract.SecondaryPhone = organizationUser.SecondaryPhone;
                userDetailsContract.SSN = ComplianceSetupManager.GetFormattedString(organizationUserID, false, tenantID);
                if (userDetailsContract.SSN == "111111111")
                {
                    userDetailsContract.SSN = string.Empty;
                }
                userDetailsContract.SelectedGenderId = organizationUser.Gender;
                userDetailsContract.SelectedGenderDefaultKeyID = organizationUser.lkpGender.DefaultLanguageKeyID;
                userDetailsContract.UserID = Convert.ToString(organizationUser.UserID);
                userDetailsContract.UserName = organizationUser.aspnet_Users.UserName;
                userDetailsContract.SelectedSuffixID = organizationUser.UserTypeID;
                userDetailsContract.Gender = organizationUser.lkpGender.GenderName;
                userDetailsContract.SelectedCommLang = SecurityManager.GetSelectedlang(organizationUser.UserID);
                OrganisationUserTextMessageSetting SMSData = SMSNotificationManager.GetSMSDataByApplicantId(organizationUserID);
                if (SMSData != null)
                {
                    userDetailsContract.IsReceiveTextNotification = SMSData.OUTMS_ReceiveTextNotification.IsNullOrEmpty() ? false : SMSData.OUTMS_ReceiveTextNotification;
                    userDetailsContract.SMSPhoneNumber = SMSData.OUTMS_MobileNumber;
                }
                else
                {
                    userDetailsContract.IsReceiveTextNotification = true;
                    userDetailsContract.SMSPhoneNumber = organizationUser.PhoneNumber;
                }

                if (userDetailsContract.SelectedSuffixID > 0)
                {
                    var suffix = SecurityManager.GetSuffixes().Where(cond => cond.SuffixID == userDetailsContract.SelectedSuffixID).FirstOrDefault();
                    if(suffix!= null)
                    {
                        userDetailsContract.Suffix = suffix.Suffix;
                        userDetailsContract.IsSuffixDropDown = suffix.IsSystem;
                    }
                }
                if (!organizationUser.OrganizationUserProfiles.IsNullOrEmpty())
                {
                    OrganizationUserProfile orgUserprofile = organizationUser.OrganizationUserProfiles.Where(cond => !cond.IsDeleted && cond.IsActive).LastOrDefault();
                    if (!orgUserprofile.IsNullOrEmpty())
                        userDetailsContract.OrganizationUserProfileID = orgUserprofile.OrganizationUserProfileID;
                }

                if (!organizationUser.Organization.IsNullOrEmpty()
                    && !organizationUser.Organization.Tenant.IsNullOrEmpty())
                {
                    userDetailsContract.InstitutionName = organizationUser.Organization.Tenant.TenantName;
                }

                //Fetch Address Details
                if (!organizationUser.AddressHandle.IsNullOrEmpty()
                    && !organizationUser.AddressHandle.Addresses.FirstOrDefault().IsNullOrEmpty())
                {
                    userDetailsContract.Address = organizationUser.AddressHandle.Addresses.FirstOrDefault().Address1;
                    userDetailsContract.ZipId = organizationUser.AddressHandle.Addresses.FirstOrDefault().ZipCodeID;
                    if (!organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressExts.IsNullOrEmpty())
                    {
                        userDetailsContract.StateName = organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressExts.FirstOrDefault().AE_StateName;
                        userDetailsContract.CityName = organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressExts.FirstOrDefault().AE_CityName;
                        userDetailsContract.PostalCode = organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressExts.FirstOrDefault().AE_ZipCode;
                        userDetailsContract.CountryId = Convert.ToInt32(organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressExts.FirstOrDefault().AE_County);
                        string countryName = SecurityManager.GetCountryByCountryId(userDetailsContract.CountryId);
                        userDetailsContract.CountryName = countryName;
                    }
                }

                //Fetch Personal Alias Details
                PersonAliasContract personAliasContract = null;
                userDetailsContract.PersonAliasList = new List<PersonAliasContract>();
                foreach (PersonAlia personAlias in organizationUser.PersonAlias.Where(cond => !cond.PA_IsDeleted))
                {
                    personAliasContract = new PersonAliasContract();
                    personAliasContract.ID = personAlias.PA_ID;
                    personAliasContract.FirstName = personAlias.PA_FirstName;
                    personAliasContract.LastName = personAlias.PA_LastName;
                    personAliasContract.MiddleName = !personAlias.PA_MiddleName.IsNullOrEmpty() ? personAlias.PA_MiddleName : string.Empty;

                    PersonAliasExtension personAliasExtension = personAlias.PersonAliasExtensions.Where(cond => !cond.PAE_IsDeleted).FirstOrDefault();
                    if (!personAliasExtension.IsNullOrEmpty())
                    {
                        personAliasContract.Suffix = personAliasExtension.PAE_Suffix;
                    }
                    if (personAliasContract != null)
                    {
                        userDetailsContract.PersonAliasList.Add(personAliasContract);
                    }

                }
            }
            return userDetailsContract;
        }

        public static string GetTotalPrice(ApplicantOrderContract applicantOrderContract, int tenantId)
        {
            List<BackgroundOrderData> lstBackgroundOrder = GetBackgroudOrderData(applicantOrderContract);
            List<BackgroundPackagesContract> lstBackgroundPackagesContract = new List<BackgroundPackagesContract>();
            BackgroundPackagesContract package = new BackgroundPackagesContract();
            package.BPAId = applicantOrderContract.bkgPackageID;
            package.BPHMId = applicantOrderContract.bkgPkgHierarchyMappingID;
            lstBackgroundPackagesContract.Add(package);
            bool _isXMLGenerated;
            string _pricingInputXML = StoredProcedureManagers.GetPricingDataInputXML(null, tenantId, lstBackgroundOrder,
              lstBackgroundPackagesContract, out _isXMLGenerated);

            string _pricingOutputXML = StoredProcedureManagers.GetPricingData(_pricingInputXML, tenantId);

            return _pricingOutputXML;
        }
        public static List<BackgroundOrderData> GetBackgroudOrderData(ApplicantOrderContract applicantOrderContract)
        {
            List<AttributesForCustomFormContract> lstCustomAttribute = applicantOrderContract.lstCustomAttribute;
            List<BackgroundOrderData> lstBkgOrderData = new List<BackgroundOrderData>();
            BackgroundOrderData bkgOrderDataContract = null;
            if (!lstCustomAttribute.IsNullOrEmpty() && lstCustomAttribute.Count > AppConsts.NONE)
            {
                foreach (var lst in lstCustomAttribute.GroupBy(x => x.AttributeGroupId).ToList())
                {
                    bkgOrderDataContract = new BackgroundOrderData();
                    Dictionary<int, string> CustomFormData = new Dictionary<int, string>();
                    bkgOrderDataContract.InstanceId = AppConsts.ONE;
                    bkgOrderDataContract.BkgSvcAttributeGroupId = lst.FirstOrDefault().AttributeGroupId;
                    bkgOrderDataContract.CustomFormId = applicantOrderContract.customFormID;

                    lst.ForEach(x =>
                    {
                        CustomFormData.Add(x.AtrributeGroupMappingId, x.AttributeDataValue);
                    });
                    bkgOrderDataContract.CustomFormData = CustomFormData;
                    lstBkgOrderData.Add(bkgOrderDataContract);
                }

                return lstBkgOrderData;
            }
            return null;

        }

        public static ReserveSlotContract ReserveSlot(FingerPrintAppointmentContract locationContract, int currentLoggedInUserId)
        {
            return FingerPrintSetUpManager.ReserveSlot(locationContract.ReserverSlotID, locationContract.SlotID.Value, currentLoggedInUserId);
        }
        #endregion

        #region Look UP Methods

        public static List<lkpGender> GetGender(string langCode)
        {
            var _lkpLanguages = LookupManager.GetLookUpData<Entity.lkpLanguage>();
            var _languageID = _lkpLanguages.Where(col => col.LAN_Code == langCode).FirstOrDefault();
            return SecurityManager.GetGender().Where(col => col.LanguageID == _languageID.LAN_ID).ToList();
        }

        public static List<LookupContract> GetListofCountries()
        {
            return SecurityManager.GetLocationSpecifictenantAllCountriesList(false, AppConsts.NONE);
        }

        public static List<LookupContract> GetListofStatesbyCountryID(int countryID)
        {
            return SecurityManager.GetLocationSpecifictenantAllCountriesList(true, countryID);
        }

        #endregion

        #region Payment
        public static string GetAgreementText(int tenantID, string LangCode)
        {
            string creditCardAgreementStatement = string.Empty;
            Entity.ClientEntity.AppConfiguration appConfiguration = ComplianceDataManager.GetAppConfiguration(LangCode == "AAAA" ? AppConsts.CREDIT_CARD_AGREEMENT_STATEMENT_APPCONFIGKEY : AppConsts.CREDIT_CARD_AGREEMENT_STATEMENT_IN_SPANISH_APPCONFIGKEY, tenantID);

            if (appConfiguration.IsNotNull())
            {
                string schoolName = ClientSecurityManager.GetTenantName(tenantID);
                creditCardAgreementStatement = appConfiguration.AC_Value;
                creditCardAgreementStatement = creditCardAgreementStatement.Replace(AppConsts.PSIEMAIL_SCHOOLNAME, schoolName);
            }
            return creditCardAgreementStatement;
        }


        public static List<PkgList> GetPaymentOptions(int dppIds, string bphmIds, int dpmId, int tenantID)
        {
            return StoredProcedureManagers.GetPaymentOptions(dppIds, bphmIds, dpmId, tenantID);
        }

        public static List<Entity.lkpPaymentOption> GetMasterPaymentSetting(int tenantID, string billingCodeAmt, bool isBillingCode)
        {
            List<Entity.lkpPaymentOption> lstPaymentOptions = new List<Entity.lkpPaymentOption>();
            List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
            List<Entity.lkpPaymentOption> _lstMasterPaymentOptns = ComplianceDataManager.GetMasterPaymentOptions(tenantID, out lstClientPaymentOptns);
            //    List<String> lstPaymentOptnCode = new List<String>();
            //    List<Entity.ClientEntity.lkpPaymentOption> _lstClientPaymentOptn = new List<Entity.ClientEntity.lkpPaymentOption>();
            lstClientPaymentOptns = ComplianceDataManager.GetPaymentTypeList(tenantID);

            //Balance Amount case

            //Entity.ClientEntity.lkpPaymentOption _clientPaymentOptnData = lstClientPaymentOptns.Where(x => !x.IsDeleted).FirstOrDefault();
            //_lstClientPaymentOptn.Add(_clientPaymentOptnData);


            //Payment by institution case
            if (!isBillingCode && billingCodeAmt.IsNullOrEmpty())
            {
                var _paymentByInstitutionOptionCode = PaymentOptions.InvoiceWithOutApproval.GetStringValue();
                Entity.ClientEntity.lkpPaymentOption _clientPaymentOptn = lstClientPaymentOptns.Where(po => po.Code == _paymentByInstitutionOptionCode
                                                           && !po.IsDeleted).FirstOrDefault();
                lstClientPaymentOptns.Remove(_clientPaymentOptn);
            }

            if (!lstClientPaymentOptns.IsNullOrEmpty() && lstClientPaymentOptns.Count > AppConsts.NONE)
            {
                foreach (Entity.ClientEntity.lkpPaymentOption _clientPaymentOptn in lstClientPaymentOptns)
                {

                    if (_clientPaymentOptn.IsNotNull())
                    {
                        var _masterPaymentOption = _lstMasterPaymentOptns.Where(mpo => mpo.Code == _clientPaymentOptn.Code).First();

                        if (!_masterPaymentOption.InstructionText.IsNullOrEmpty())
                        {
                            Entity.lkpPaymentOption paymentOptionData = new lkpPaymentOption();
                            paymentOptionData.InstructionText = _masterPaymentOption.InstructionText == null ? (_clientPaymentOptn.InstructionText == null ? "" : _clientPaymentOptn.InstructionText) : _masterPaymentOption.InstructionText;
                            paymentOptionData.Name = _clientPaymentOptn.Name;
                            paymentOptionData.PaymentOptionID = _clientPaymentOptn.PaymentOptionID;
                            paymentOptionData.Code = _clientPaymentOptn.Code;
                            lstPaymentOptions.Add(paymentOptionData);
                        }

                    }
                }
            }

            return lstPaymentOptions;
        }
        #endregion

        #region Submit Order

        #region OrganizationUser Details Entry

        public static Boolean AddUpdateUser(UserContract userContract, int currentLoggedInuserID, int tenantID)
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUserDetailByOrganizationUserId(userContract.OrganizationUserID);
            string CurrentPrimaryEmail = organizationUser.PrimaryEmailAddress;
            organizationUser.aspnet_Users.MobileAlias = userContract.PrimaryPhone;
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(tenantID);
            organizationUser.FirstName = userContract.FirstName;
            organizationUser.LastName = userContract.LastName;
            organizationUser.MiddleName = userContract.MiddleName;
            organizationUser.IsApplicant = true;
            organizationUser.ModifiedByID = currentLoggedInuserID;
            organizationUser.ModifiedOn = DateTime.Now;
            organizationUser.DOB = userContract.DOB;
            organizationUser.Gender = userContract.SelectedGenderId;
            organizationUser.PhoneNumber = userContract.PrimaryPhone;
            organizationUser.PrimaryEmailAddress = userContract.PrimaryEmail;
            organizationUser.SecondaryEmailAddress = userContract.SecondaryEmail;
            if (!userContract.SSN.IsNullOrEmpty())
            {
                organizationUser.SSN = userContract.SSN.Replace("-", "");
            }
            else
            {
                organizationUser.SSN = string.Empty;
            }

            organizationUser.Suffix = userContract.Suffix;
            Boolean IsSuffixDropDown = ApiSecurityManager.IsSuffixDropDown();
            if (IsSuffixDropDown)
            {
                organizationUser.UserTypeID = userContract.SelectedSuffixID > 0 ? userContract.SelectedSuffixID : (Int32?)null;
            }
            if (!IsSuffixDropDown)
            {
                organizationUser.UserTypeID = GetSuffixIdBasedOnSuffixText(userContract.Suffix.IsNullOrEmpty() ? string.Empty : userContract.Suffix);
            }
            organizationUser.SecondaryPhone = userContract.SecondaryPhone;
            organizationUser.aspnet_Users.UserName = userContract.UserName;

            //organizationUser.IsInternationalPhoneNumber = View.IsInternationalPhoneNumber;

            if (userContract.UpdateAspnetEmail && userContract.PrimaryEmail.ToLower() == CurrentPrimaryEmail.ToLower())
            {
                organizationUser.aspnet_Users.aspnet_Membership.Email = userContract.PrimaryEmail;
                organizationUser.aspnet_Users.aspnet_Membership.LoweredEmail = userContract.PrimaryEmail.ToLower();

            }

            //Adds and updates the Person Alias.
            AddUpdatePersonAlias(organizationUser, userContract);

            Address addressNew = null;
            AddressExt addressExtNew = null;
            //Check if Address Handle is not null
            if (organizationUser.AddressHandle.IsNotNull())
            {
                var address = organizationUser.AddressHandle.Addresses.FirstOrDefault();
                //Check if Address not null
                if (address.IsNotNull())
                {
                    //Check if current address has been modified.
                    if (CheckIfAddressUpdated(address, userContract))
                    {
                        addressNew = new Address();
                        Dictionary<string, object> dicAddressData = GetAddressDataDictionary(userContract);
                        if (userContract.ZipId == 0 || SecurityManager.IsLocationServiceTenant(tenantID))
                        {
                            addressExtNew = new AddressExt();
                            addressExtNew.AE_CountryID = AppConsts.ONE;//UAT-3910 
                            addressExtNew.AE_StateName = userContract.StateName;
                            addressExtNew.AE_CityName = userContract.CityName;
                            addressExtNew.AE_ZipCode = userContract.PostalCode;
                            addressExtNew.AE_County = Convert.ToString(userContract.CountryId);//UAT-3910
                        }
                        Guid addressHandleId = Guid.NewGuid();
                        SecurityManager.AddAddressHandle(addressHandleId);
                        SecurityManager.AddAddress(dicAddressData, addressHandleId, currentLoggedInuserID, addressNew, addressExtNew);

                        SecurityManager.UpdateChanges();
                        ClientSecurityManager.AddAddressHandle(tenantID, addressHandleId);
                        ClientSecurityManager.AddAddress(tenantID, dicAddressData, addressHandleId, currentLoggedInuserID, addressNew, addressExtNew);
                        organizationUser.AddressHandleID = addressHandleId;
                        organizationUser.AddressHandle.Addresses.Add(addressNew);
                    }
                }
            }
            ////Adds and updates the residential histories.
            //AddUpdateResidentialHistory(organizationUser, addressNew,currentLoggedInuserID);

            organizationUser.IsDeleted = false;
            SMSNotificationManager.SaveUpdateSMSData(userContract.OrganizationUserID, userContract.SMSPhoneNumber, userContract.OrganizationUserID, userContract.IsReceiveTextNotification);
            BackgroundProcessOrderManager.DeleteOldOrganizationUserProfileData(new Guid(userContract.UserID), tenantID);

            OrganizationUserProfile organizationUserProfile = SecurityManager.AddOrganizationUserProfile(organizationUser, userContract.SelectedCommLang);
            userContract.OrganizationUserProfileID = organizationUserProfile.OrganizationUserProfileID;
            organizationUser.IsDeleted = false;

            //string result="";
            //UserAuthRequest tempUserAuthRequest = null;
            //if (userContract.PrimaryEmail.ToLower() != CurrentPrimaryEmail.ToLower() && organizationUser.IsApplicant.HasValue? organizationUser.IsApplicant.Value:false)
            //{
            //    Int16 authTypeId = SecurityManager.GetAuthRequestTypeIdByCode(AuthRequestType.Email_Confirmation.GetStringValue());
            //    tempUserAuthRequest = SecurityManager.GenerateEmailConfirmationReq(currentLoggedInuserID, organizationUserProfile.OrganizationUserProfileID, CurrentPrimaryEmail, userContract.UpdateAspnetEmail, userContract.OrganizationUserID, authTypeId);
            //    if (tempUserAuthRequest.IsNotNull())
            //    {
            //        //Get Website Url
            //        Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(tenantID);
            //        String applicationUrl = String.Empty;
            //        if (webSite.IsNotNull() && webSite.WebSiteID != SysXDBConsts.NONE)
            //        {
            //            applicationUrl = webSite.URL;
            //        }
            //        else
            //        {
            //            webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
            //            applicationUrl = webSite.URL;
            //        }
            //        applicationUrl = applicationUrl + "/Login.aspx?AuthReqVerCode=" + tempUserAuthRequest.UAR_VerificationCode;
            //        if (!SecurityManager.SendEmailForEmailChange(organizationUser, applicationUrl, userContract.PrimaryEmail))
            //        {
            //            // View.ErrorMessage ="Some error has occured.Please contact administrator.";
            //            result = "ERROCCNTCTADMNSTR";


            //        }
            //        else
            //        {
            //            // View.SuccessMessage="An email has been sent with a verification link to your email, " + userContract.PrimaryEmail + ". Please click the link to update your primary email address.";
            //            result = "VERIEMAILSENT";
            //        }
            //        SecurityManager.SendAlertForEmailChange(organizationUser);

            //    }
            //}


            return SecurityManager.UpdateOrganizationUser(organizationUser);
            //if (SecurityManager.UpdateOrganizationUser(organizationUser))
            //{
            //SecurityManager.DeleteAdminOrganizationUser(organizationUser, currentLoggedInuserID);
            //if (organizationUser.OrganizationUserID > AppConsts.NONE)
            //{
            //    SecurityManager.DeleteAdminOrganizationUser(organizationUser, currentLoggedInuserID);
            //  //  GetOrganizationUserDetailsByUserID(organizationUser.UserID, userContract);
            //}
            //return true;
            //}
            // else
            // {
            //   return false;
            //}
        }
        //private static void GetOrganizationUserDetailsByUserID(Guid UserID,UserContract userContract)
        //{
        //    userContract.OrganizationUser = BackgroundProcessOrderManager.GetOrganisationUserByUserID(UserID, View.SelectedTenantId);
        //    if (!View.OrganizationUser.OrganizationUserProfiles.IsNullOrEmpty())
        //    {
        //        View.OrganizationUserProfile = View.OrganizationUser.OrganizationUserProfiles.Where(cond => !cond.IsDeleted).FirstOrDefault();
        //    }
        //    View.ResidentialHistoryListAll = GetResidentialHistories(View.OrganizationUser.UserID);
        //}

        public static Boolean AddUpdateUserDetails(ApplicantOrderContract applicantOrderContract, int tenantID, int currentLoggedInUserID)
        {

            return AddUpdateUser(applicantOrderContract.userInfo, currentLoggedInUserID, tenantID);


        }

        private static void AddUpdateResidentialHistory(OrganizationUser organizationUser, Address addressNew, int currentLoggedInUserId)
        {
            //Current residential Address
            ResidentialHistory currentResedentialHistory = organizationUser.ResidentialHistories.FirstOrDefault(x => x.RHI_IsCurrentAddress == true && x.RHI_IsDeleted == false);
            if (currentResedentialHistory.IsNotNull())
            {
                //Update current residential address.
                currentResedentialHistory.Address = addressNew == null ? organizationUser.AddressHandle.Addresses.FirstOrDefault() : addressNew;
                currentResedentialHistory.RHI_ResidenceStartDate = DateTime.Now;
                currentResedentialHistory.RHI_ModifiedByID = currentLoggedInUserId;
                currentResedentialHistory.RHI_CreatedByID = currentLoggedInUserId;
                currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                currentResedentialHistory.RHI_ModifiedOn = DateTime.Now;
            }
            else
            {
                currentResedentialHistory = new ResidentialHistory();
                currentResedentialHistory.RHI_IsCurrentAddress = true;
                currentResedentialHistory.RHI_IsPrimaryResidence = false;
                //currentResedentialHistory.RHI_ResidenceStartDate = View.DateResidentFrom;
                currentResedentialHistory.RHI_IsDeleted = false;
                currentResedentialHistory.RHI_CreatedByID = currentLoggedInUserId;
                currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
                currentResedentialHistory.RHI_OrganizationUserID = organizationUser.OrganizationUserID;
                currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                currentResedentialHistory.Address = addressNew == null ? organizationUser.AddressHandle.Addresses.FirstOrDefault() : addressNew;
                organizationUser.ResidentialHistories.Add(currentResedentialHistory);
            }

            //else
            //{
            //    //ADd current residential address.
            //    currentResedentialHistory = new ResidentialHistory();
            //    currentResedentialHistory.RHI_IsCurrentAddress = true;
            //    currentResedentialHistory.RHI_IsPrimaryResidence = false;
            //    currentResedentialHistory.RHI_IsDeleted = false;
            //    currentResedentialHistory.RHI_CreatedByID = AppConsts.NONE;
            //    currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
            //    currentResedentialHistory.RHI_OrganizationUserID = organizationUser.OrganizationUserID;
            //    currentResedentialHistory.RHI_AddressId = organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressID;
            //    currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
            //    organizationUser.ResidentialHistories.Add(currentResedentialHistory);


            //    SecurityManager.UpdateOrganizationUser(organizationUser);
            //}

            AddUpdateResidentialHistories(organizationUser);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAddress"></param>
        /// <returns></returns>
        private static bool CheckIfAddressUpdated(Address objAddress, UserContract userCOntract)
        {
            if (objAddress.Address1.ToLower().Trim() != userCOntract.Address.ToLower().Trim()
                 //    || objAddress.Address2.ToLower().Trim() != View.Address2.ToLower().Trim()
                 || objAddress.ZipCodeID != userCOntract.ZipId)
            {
                return true;
            }
            var addressExt = objAddress.AddressExts.FirstOrDefault();
            if (addressExt == null && (userCOntract.CountryId != 0
                || !string.IsNullOrWhiteSpace(userCOntract.StateName)
                || !string.IsNullOrWhiteSpace(userCOntract.CityName)
                || !string.IsNullOrWhiteSpace(userCOntract.PostalCode)))
            {
                return true;
            }
            if (addressExt.IsNotNull())
            {
                if (addressExt.AE_CountryID != userCOntract.CountryId
                 || addressExt.AE_StateName.ToLower().Trim() != userCOntract.StateName.ToLower().Trim()
                 || addressExt.AE_CityName.ToLower().Trim() != userCOntract.CityName.ToLower().Trim()
                 || addressExt.AE_ZipCode.ToLower().Trim() != userCOntract.PostalCode.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }
        public static int GetSuffixIdBasedOnSuffixText(string suffix)
        {
            return SecurityManager.GetSuffixIdBasedOnSuffixText(suffix);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgUser"></param>
        private static void AddOrganizationUserProfile(Entity.ClientEntity.OrganizationUser orgUser)
        {
            bool isAddMode = false;
            Entity.ClientEntity.OrganizationUserProfile orgUserProfile = orgUser.OrganizationUserProfiles.FirstOrDefault();

            if (orgUserProfile.IsNullOrEmpty())
            {
                orgUserProfile = new Entity.ClientEntity.OrganizationUserProfile();
                isAddMode = true;
            }

            orgUserProfile.AddressHandleID = orgUser.AddressHandleID;
            orgUserProfile.FirstName = orgUser.FirstName;
            orgUserProfile.MiddleName = orgUser.MiddleName;
            orgUserProfile.LastName = orgUser.LastName;
            orgUserProfile.IsDeleted = false;
            orgUserProfile.IsActive = orgUser.IsActive;
            orgUserProfile.ExpireDate = orgUser.ExpireDate;
            orgUserProfile.CreatedByID = orgUser.CreatedByID;
            orgUserProfile.CreatedOn = orgUser.CreatedOn;
            orgUserProfile.ModifiedByID = orgUser.ModifiedByID;
            orgUserProfile.ModifiedOn = orgUser.ModifiedOn;
            orgUserProfile.DOB = orgUser.DOB;
            orgUserProfile.SSN = orgUser.SSN;
            orgUserProfile.Gender = orgUser.Gender;
            orgUserProfile.PhoneNumber = orgUser.PhoneNumber;
            orgUserProfile.PrimaryEmailAddress = orgUser.PrimaryEmailAddress;

            //if (orgUser.PersonAlias.IsNotNull())
            //{
            //    List<Entity.ClientEntity.PersonAlia> currentAliasList = orgUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
            //    foreach (Entity.ClientEntity.PersonAlia tempPersonAlias in currentAliasList)
            //    {
            //        Entity.ClientEntity.PersonAliasProfile personAliasProfile = new Entity.ClientEntity.PersonAliasProfile();
            //        personAliasProfile.PAP_FirstName = tempPersonAlias.PA_FirstName;
            //        personAliasProfile.PAP_LastName = tempPersonAlias.PA_LastName;
            //        personAliasProfile.PAP_MiddleName = tempPersonAlias.PA_MiddleName;
            //        personAliasProfile.PAP_IsDeleted = false;
            //        personAliasProfile.PAP_CreatedBy = tempPersonAlias.PA_CreatedBy;
            //        personAliasProfile.PAP_CreatedOn = DateTime.Now;
            //        orgUserProfile.PersonAliasProfiles.Add(personAliasProfile);
            //    }
            //}
            if (isAddMode)
            {
                orgUser.OrganizationUserProfiles.Add(orgUserProfile);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUser"></param>
        private static void AddUpdateResidentialHistories(OrganizationUser organizationUser)
        {
            //List of residential histories.
            //   List<PreviousAddressContract> previousAddressList = View.ResidentialHistoryList;
            //if (previousAddressList.IsNotNull())
            //{
            //    previousAddressList = previousAddressList.Where(x => x.isDeleted == true || x.isNew == true || x.isUpdated == true).ToList();
            //    if (previousAddressList.Count > 0)
            //    {
            //        // List of Resedential Histories associated with the organisaion User ID.
            //        List<ResidentialHistory> lstResedentialHistory = organizationUser.ResidentialHistories.Where(x => x.RHI_IsDeleted == false).ToList();
            //        // List of Resedential Histories to be deleted.
            //        List<Int32> lstResHisIDToBeDel = previousAddressList.Where(x => x.isDeleted == true).Select(y => y.ID).ToList();
            //        List<ResidentialHistory> lstResHisToBeDel = lstResedentialHistory.Where(x => lstResHisIDToBeDel.Contains(x.RHI_ID)).ToList();
            //        foreach (var prevAddress in lstResHisToBeDel)
            //        {
            //            prevAddress.RHI_IsDeleted = true;
            //            prevAddress.RHI_ModifiedByID = currentLoggedInUserID;
            //            prevAddress.RHI_ModifiedOn = DateTime.Now;
            //        }

            //        // List of Resedential Histories to be added.
            //        List<PreviousAddressContract> lstResHisIDToBeAdded = previousAddressList.Where(x => x.isNew == true).ToList();
            //        foreach (var prevAddress in lstResHisIDToBeAdded)
            //        {
            //            Address addressPervious = AddNewPreviousAddress(prevAddress);

            //            ResidentialHistory newResidentialHistory = new ResidentialHistory();
            //            newResidentialHistory.RHI_AddressId = addressPervious.AddressID;
            //            newResidentialHistory.RHI_ResidenceStartDate = prevAddress.ResidenceStartDate;
            //            newResidentialHistory.RHI_ResidenceEndDate = prevAddress.ResidenceEndDate;
            //            newResidentialHistory.RHI_IsCurrentAddress = false;
            //            newResidentialHistory.RHI_IsDeleted = false;
            //            newResidentialHistory.RHI_CreatedByID = currentLoggedInUserID;
            //            newResidentialHistory.RHI_CreatedOn = DateTime.Now;
            //            newResidentialHistory.RHI_SequenceOrder = prevAddress.ResHistorySeqOrdID;
            //            organizationUser.ResidentialHistories.Add(newResidentialHistory);
            //        }

            //        // List of Resedential Histories to be updated.
            //        List<PreviousAddressContract> lstResHisToBeUpdated = previousAddressList.Where(x => x.isUpdated == true).ToList();
            //        foreach (var prevAddress in lstResHisToBeUpdated)
            //        {
            //            ResidentialHistory resHistory = lstResedentialHistory.FirstOrDefault(x => x.RHI_ID == prevAddress.ID && x.RHI_IsDeleted == false);
            //            if (resHistory.IsNotNull())
            //            {
            //                if (CheckIfPreviousAddressUpdated(resHistory.Address, prevAddress))
            //                {
            //                    Address addressPervious = AddNewPreviousAddress(prevAddress);
            //                    resHistory.RHI_AddressId = addressPervious.AddressID;
            //                }
            //                resHistory.RHI_ResidenceStartDate = prevAddress.ResidenceStartDate;
            //                resHistory.RHI_ResidenceEndDate = prevAddress.ResidenceEndDate;
            //                resHistory.Address.ModifiedByID = View.CurrentLoggedInUserId;
            //                resHistory.RHI_ModifiedByID = View.CurrentLoggedInUserId;
            //                resHistory.RHI_CreatedByID = View.CurrentLoggedInUserId;
            //                resHistory.Address.ModifiedOn = DateTime.Now;
            //                resHistory.RHI_ModifiedOn = DateTime.Now;
            //                resHistory.RHI_SequenceOrder = prevAddress.ResHistorySeqOrdID;
            //            }
            //        }
            //    }
            //}
        }


        public static Dictionary<string, object> GetAddressDataDictionary(UserContract userContract)
        {
            var dicAddressData = new Dictionary<string, object>
            {
                { "address1", userContract.Address },
                { "address2", "" },
                { "zipcodeid", 0 }
            };
            return dicAddressData;
        }
        #endregion


        public static string SubmitOrderPayTypeChanged(int orderId, int paymentModeId, int tenantId, int OrgUserId)
        {
            var _redirectUrlType = string.Empty;
            var _paymentModeCode = string.Empty;
            var _errorMessage = string.Empty;
            var _redirectUrl = string.Empty;
            var order = ComplianceDataManager.GetOrderById(tenantId, orderId);
            //var queryString = new Dictionary<string, string>();
            //var applicantOrderCart = new ApplicantOrderCart();//= SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            var _orgUserProfile = new Entity.ClientEntity.OrganizationUserProfile();
            //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
            //Int32 organizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
            //Int32 _tenantId = SecurityManager.GetOrganizationUser(organizationUserID).Organization.TenantID.Value;
            var _tenantId = SecurityManager.GetOrganizationUser(OrgUserId).Organization.TenantID.Value;
            var organizationUserID = OrgUserId;
            _paymentModeCode = ComplianceDataManager.GetPaymentOptionCodeById(paymentModeId, _tenantId);
            var orderStatuscode = string.Empty;
            //If payment mode is Credit Card or Paypal
            if (_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower() ||
                _paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
            {
                orderStatuscode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
            }
            else //else if payment mode is Money Order Or Invoice
            {
                orderStatuscode = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();
            }

            //foreach (var applicantOrder in applicantOrderCart.lstApplicantOrder)
            //{
            //    _orgUserProfile = applicantOrder.OrganizationUserProfile;
            //}

            var _applicantOrder = ComplianceDataManager.GetOrderById(_tenantId, orderId);
            _applicantOrder.PaymentOptionID = paymentModeId;
            _applicantOrder.ModifiedByID = organizationUserID;

            var _generatedInvoiceNumber = string.Empty;
            var _insertedOPDetailId = AppConsts.NONE;
            var ordPaymentDetailId = order.OrderPaymentDetails.FirstOrDefault(opd => opd.lkpPaymentOption.Code != PaymentOptions.InvoiceWithOutApproval.GetStringValue()).OPD_ID;
            _generatedInvoiceNumber = ComplianceDataManager.UpdateOrderByID(_applicantOrder, orderStatuscode, ordPaymentDetailId,
                                                                                paymentModeId, out _insertedOPDetailId, _tenantId);

            //applicantOrderCart.OrderPaymentdetailId = _insertedOPDetailId;
            var _dicInvoiceNumbers = new Dictionary<string, string>
            {
                { _paymentModeCode, _generatedInvoiceNumber }
            };
            //if (applicantOrderCart.IsLocationServiceTenant)
            FingerPrintDataManager.SavePaymentTypeAuditChange(_paymentModeCode, _applicantOrder.OrderID, _tenantId,
                                                                organizationUserID, ordPaymentDetailId);
            #region UAT-1697: Add new client setting to make it where all subscription renewals nees to be approved, even if payment method is invoice without approval
            var ifRenewalOrderApprovalRequired = false;
            var orderRequestTypeCode = _applicantOrder.lkpOrderRequestType.ORT_Code;
            if (orderRequestTypeCode == OrderRequestType.RenewalOrder.GetStringValue() &&
                _paymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
            {
                //Check for client settings
                var rnwlOrderAprvlRqdCode = Setting.SUBSCRIPTION_RENEWAL_NEED_APPROVAL.GetStringValue();
                var rnwlOrderAprvlRqdSetting = ComplianceDataManager.GetClientSetting(_tenantId).FirstOrDefault(cond => cond.lkpSetting.Code
                                                                                           == rnwlOrderAprvlRqdCode && !cond.CS_IsDeleted);
                if (!rnwlOrderAprvlRqdSetting.IsNullOrEmpty() &&
                    !rnwlOrderAprvlRqdSetting.CS_SettingValue.IsNullOrEmpty())
                {
                    ifRenewalOrderApprovalRequired = Convert.ToBoolean(Convert.ToInt32(rnwlOrderAprvlRqdSetting.CS_SettingValue));
                }
            }
            #endregion

            if (_paymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower()
                && !ifRenewalOrderApprovalRequired)
            {
                DateTime expirydate = DateTime.Now;
                if (_applicantOrder.SubscriptionYear.HasValue)
                {
                    expirydate = expirydate.AddYears(_applicantOrder.SubscriptionYear.Value);
                }
                if (_applicantOrder.SubscriptionMonth.HasValue)
                {
                    expirydate = expirydate.AddMonths(_applicantOrder.SubscriptionMonth.Value);
                }

                int _packageId = AppConsts.NONE;

                // Handle case when no Compliance package is selected
                if (!_applicantOrder.DeptProgramPackage.IsNullOrEmpty())
                    _packageId = _applicantOrder.DeptProgramPackage.DPP_CompliancePackageID;

                string refrenceNumber = "N/A";

                //ComplianceDataManager.UpdateOrderStatus(_tenantId, _applicantOrder.OrderID, ApplicantOrderStatus.Paid.GetStringValue(), _packageId,
                //                   _applicantOrder.CreatedByID), _applicantOrder.OrganizationUserProfile.OrganizationUserID, refrenceNumber
                //                  , expirydate, _insertedOPDetailId);
                ComplianceDataManager.UpdateOrderStatus(_tenantId, _applicantOrder.OrderID, ApplicantOrderStatus.Paid.GetStringValue(), _packageId,
                                  Convert.ToInt32(_applicantOrder.ModifiedByID), _applicantOrder.OrganizationUserProfile.OrganizationUserID,
                                                    refrenceNumber, expirydate, _insertedOPDetailId);

                // OrderPaymentDetail orderPaymentDetail = ComplianceDataManager.GetOrderDetailById(_tenantId, _applicantOrder.OrderID);
                var orderPaymentDetail = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(_tenantId, _applicantOrder.OrderID).
                                            FirstOrDefault(x => x.lkpPaymentOption.Code.Equals(_dicInvoiceNumbers.Keys.FirstOrDefault()));                
                //Send E-mail to user: 
                // not for CBI
                //if (!orderPaymentDetail.IsNullOrEmpty())
                //{
                //    string orderPackageType = orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code;
                //    int? systemCommunicationID = null;
                //    Guid? messageID = null;

                //    //Update EdS Data.
                //    //UpdateEDSDataForChangePaymentType(orderPaymentDetail, _applicantOrder.OrderID, _applicantOrder, _tenantId);

                //    //Send Print Scan notification
                //    //UAT-1358:Complio Notification to applicant for PrintScan
                //    //SendPrintScanNotification(_applicantOrder.OrderID, _applicantOrder, orderPaymentDetail, true, _tenantId);

                //    //Send mail
                //    //if (!applicantOrderCart.IsLocationServiceTenant)
                //    //    systemCommunicationID = CommunicationManager.SendOrderApprovalMail(orderPaymentDetail, _applicantOrder.CreatedByID, _tenantId);

                //    #region UAT-3389
                //    // not for CBI
                //    //var dicMessageParam = new Dictionary<string, object>();
                //    //if (
                //    //    (
                //    //    orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code.Equals(OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
                //    //    ||
                //    //    orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code.Equals(OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue())
                //    //    )
                //    //    &&
                //    //    systemCommunicationID.HasValue && systemCommunicationID > AppConsts.NONE
                //    //   )
                //    //{
                //    //    var res = ComplianceDataManager.AttachOrderApprovalDocuments(_tenantId, _applicantOrder.OrderID, 
                //    //                                orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, systemCommunicationID.Value);
                //    //    if (res.Item1)
                //    //    {
                //    //        dicMessageParam = res.Item2;
                //    //    }
                //    //}
                //    #endregion
                //    //if (!applicantOrderCart.IsLocationServiceTenant)
                //    //    messageID = CommunicationManager.SendOrderApprovalMessage(orderPaymentDetail, _applicantOrder.CreatedByID, _tenantId, dicMessageParam);
                //}
            }

            //Insert data to Invoice tables
            ComplianceDataManager.SaveOrderPaymentInvoice(_tenantId, _applicantOrder.OrderID, _applicantOrder.OrganizationUserProfile.OrganizationUserID, false);

            //applicantOrderCart.InvoiceNumber = _dicInvoiceNumbers;
            //applicantOrderCart.ChangePaymentTypeCode = _paymentModeCode;

            //var _onlineInvoiceNumber = string.Empty;
            //var _onlinePaymentType = _dicInvoiceNumbers.FirstOrDefault(x => x.Key == PaymentOptions.Credit_Card.GetStringValue()
            //                                                        ||
            //                                                       x.Key == PaymentOptions.Paypal.GetStringValue()
            //                                                 );
            //if (_onlinePaymentType.IsNotNull())
            //    _onlineInvoiceNumber = _onlinePaymentType.Value;

            //queryString = new Dictionary<string, string>
            //                                            {
            //                                               { "invnum", _onlineInvoiceNumber },
            //                                               {"OrderId", Convert.ToString(orderId)}
            //                                            };
            //If payment mode is Invoice Or Money Order
            //if (String.IsNullOrEmpty(generatedInvoiceNumber) || (!(_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower()) &&
            ////!(_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())))
            //if (!(_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower()) &&
            //    !(_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower()))
            //{
            //    // In case, crash in order generation
            //    //applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
            //    _errorMessage = (_dicInvoiceNumbers.Count == 0
            //                        ||
            //                     _dicInvoiceNumbers.Any(d => d.Value.IsNullOrEmpty()))
            //     ? "Error in order placement." : String.Empty;
            //    _redirectUrl = RedirectConfirmationPage(_errorMessage);
            //}
            //else if (_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower())
            //{
            //    _redirectUrl = "Pages/CIMAccountSelection.aspx";
            //    _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
            //    _redirectUrlType = "internal";
            //}
            //else if (_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
            //{
            //    _redirectUrl = "Pages/PaypalPaymentSubmission.aspx";
            //    _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
            //}

            //Send Mail and Message Notification
            // Not For CBI
            //if (string.IsNullOrEmpty(_errorMessage))
            //{
            //    if (_paymentModeCode.Equals(PaymentOptions.Money_Order.GetStringValue(), StringComparison.OrdinalIgnoreCase) && !applicantOrderCart.IsLocationServiceTenant)
            //    {
            //        CommunicationManager.SendOrderCreationMailMoneyOrder(_applicantOrder, _orgUserProfile, _tenantId, _paymentModeCode);
            //        CommunicationManager.SendOrderCreationMessageMoneyOrder(_applicantOrder, _orgUserProfile, _tenantId, _paymentModeCode);
            //    }
            //    else if (_paymentModeCode.Equals(PaymentOptions.InvoiceWithApproval.GetStringValue(), StringComparison.OrdinalIgnoreCase)
            //        || (_paymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower()
            //            && ifRenewalOrderApprovalRequired))
            //    {
            //        CommunicationManager.SendOrderCreationMailInvoice(_applicantOrder, _orgUserProfile, _tenantId, _paymentModeCode);
            //        CommunicationManager.SendOrderCreationMessageInvoice(_applicantOrder, _orgUserProfile, _tenantId, _paymentModeCode);
            //    }
            //}
            //if (!url.ToLower().StartsWith("http"))
            //{
            //    url = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + url;
            //}
            //if (_paymentModeCode == PaymentOptions.Credit_Card.GetStringValue() || _paymentModeCode == PaymentOptions.Paypal.GetStringValue())
            //{
            //    Int32 timeout = GetSessionTimeoutValue();
            //    ApplicationDataManager.AddWebApplicationData(_onlineInvoiceNumber, url, timeout);
            //}
            ////return HttpUtility.UrlEncode(_redirectUrl);
            ////return _redirectUrl;
            //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //return javaScriptSerializer.Serialize(new { redirectUrl = _redirectUrl, redirectUrlType = _redirectUrlType });
            return string.Empty;
        }

        public static Boolean CompleteApplicantOrder(ApplicantOrderContract applicantOrderCart, int tenantID, int organizationuserID)
        {
            ApplicantOrderDataContract _applicantOrderDataContract = new ApplicantOrderDataContract();
            var _dicInvoiceNumber = new Dictionary<String, String>();
            String _paymentModeCode = String.Empty;
            if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.CbiBillingCode.IsNullOrEmpty() && !applicantOrderCart.BillingCodeAmount.IsNullOrEmpty() && applicantOrderCart.BillingCodeAmount != AppConsts.ZERO)
            {
                _applicantOrderDataContract.IsBillingCodeAmountAvlbl = true;
                _applicantOrderDataContract.BillingCodeAmount = Convert.ToDecimal(applicantOrderCart.BillingCodeAmount);
            }

            //Entity.ClientEntity.OrganizationUserProfile _orgUserProfile = new Entity.ClientEntity.OrganizationUserProfile();
            List<Package_PricingData> _lstPricingData = new List<Package_PricingData>();
            string _pricingDataXML = GetTotalPrice(applicantOrderCart, tenantID);
            if (!string.IsNullOrEmpty(_pricingDataXML))
            {
                _lstPricingData = new List<Package_PricingData>();
                _lstPricingData = GenerateDataFromPricingXML(_pricingDataXML);
            }

            Int32 _orderId = applicantOrderCart.OrderID;
            Entity.ClientEntity.Order _order = new Entity.ClientEntity.Order();
            _order = ComplianceDataManager.GetOrderById(tenantID, _orderId);
            List<Int32> newlyAddedOPDIdList = new List<Int32>();
            GenerateGroupedAmount(applicantOrderCart, tenantID);
            var cbiBillingCode = string.Empty;
            var cbiUniqueCode = string.Empty;
            if (applicantOrderCart.lstPaymentGrouping.Any(x => x.PaymentModeCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue()) && !applicantOrderCart.CbiBillingCode.IsNullOrEmpty() && !applicantOrderCart.CbiUniqueId.IsNullOrEmpty())
            {
                cbiBillingCode = applicantOrderCart.CbiBillingCode;
                cbiUniqueCode = applicantOrderCart.CbiUniqueId;
            }
            //Handle Payment by institution case
            decimal _totalAmount = 0;
            foreach (var poId in applicantOrderCart.lstPaymentGrouping)
            {
                var _price = applicantOrderCart.GrandTotal;
                _totalAmount = _price.IsNull() ? AppConsts.NONE : _price;             
                if (!cbiBillingCode.IsNullOrEmpty() && !cbiUniqueCode.IsNullOrEmpty())
                {
                    if (_totalAmount >= Convert.ToDecimal(applicantOrderCart.BillingCodeAmount))
                    {
                        if (poId.PaymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
                            poId.TotalAmount = _totalAmount = Convert.ToDecimal(applicantOrderCart.BillingCodeAmount);
                        else
                            poId.TotalAmount = _totalAmount = _totalAmount - Convert.ToDecimal(applicantOrderCart.BillingCodeAmount);
                    }
                }               
            }
            _applicantOrderDataContract.IsCompliancePackageSelected = false;
            _applicantOrderDataContract.lstGroupedData = applicantOrderCart.lstPaymentGrouping;
            _applicantOrderDataContract.TenantId = tenantID;
            _applicantOrderDataContract.LastNodeDPMId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
            _applicantOrderDataContract.lstPricingData = _lstPricingData;
            _applicantOrderDataContract.lstOrderPackageTypes = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderPackageType>(tenantID);
            _applicantOrderDataContract.lstOrderStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatu>(tenantID);
            _applicantOrderDataContract.OrganizationUserProfile = _order.OrganizationUserProfile;
            _dicInvoiceNumber = ComplianceDataManager.UpdateApplicantCompletingOrderProcess(_order, _applicantOrderDataContract, out _paymentModeCode,
                                                                                organizationuserID, out newlyAddedOPDIdList, null);

            Boolean isCreditCardPayment = applicantOrderCart.lstPaymentGrouping.Any(x => x.PaymentModeCode == PaymentOptions.Credit_Card.GetStringValue());
            ReserveSlotContract reserveSlotContract = SubmitApplicantAppointment(_order.OrderID, applicantOrderCart, tenantID, organizationuserID, cbiBillingCode, cbiUniqueCode,true);

            if (isCreditCardPayment)
            {
                FingerPrintSetUpManager.ChangeAppointmentStatus(_order.OrderID, organizationuserID, true, applicantOrderCart.LocationDetail.ReserverSlotID,false);
            }
            //Insert data to Invoice tables
            ComplianceDataManager.SaveOrderPaymentInvoice(tenantID, _order.OrderID, organizationuserID, false);

            if (!reserveSlotContract.IsNullOrEmpty() && reserveSlotContract.ApplicantAppointmentID > AppConsts.NONE && !applicantOrderCart.lstPaymentGrouping.IsNullOrEmpty() && !applicantOrderCart.lstPaymentGrouping.Any(x => x.PaymentModeCode == PaymentOptions.Credit_Card.GetStringValue()))
            {
                if (string.IsNullOrEmpty(reserveSlotContract.ErrorMsg))
                {
                    var appointmentOderScheduleData = FingerPrintSetUpManager.GetAppointmentOrderDetailData(organizationuserID, false, Convert.ToString(tenantID), reserveSlotContract.ApplicantAppointmentID);
                    AppointmentSlotContract AppSlotContract = new AppointmentSlotContract();
                    if (applicantOrderCart.LocationDetail.SlotID > AppConsts.ONE)
                    {
                        AppSlotContract.SlotDate = applicantOrderCart.LocationDetail.SlotDate;
                        AppSlotContract.SlotStartTime = applicantOrderCart.LocationDetail.StartTime.ToString("HH:mm");
                        AppSlotContract.SlotEndTime = applicantOrderCart.LocationDetail.EndTime.ToString("HH:mm");
                    }
                    AppSlotContract.ApplicantOrgUserId = organizationuserID;
                    AppSlotContract.IsEventType = applicantOrderCart.LocationDetail.IsEventCode;
                    if (applicantOrderCart.LocationDetail.IsEventCode)
                    {
                        AppSlotContract.EventName = applicantOrderCart.LocationDetail.EventName;
                        AppSlotContract.EventDescription = applicantOrderCart.LocationDetail.EventDescription;
                    }
                    AppSlotContract.IsOutOfStateAppointment = applicantOrderCart.LocationDetail.IsOutOfState;
                    FingerPrintSetUpManager.SendOrderCreateMail(appointmentOderScheduleData, AppSlotContract);
                }
            }

            if (!_dicInvoiceNumber.IsNullOrEmpty())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string AddUpdateOrderDetails(ApplicantOrderContract applicantOrderContract, int tenantID, int currentLoggedInUserID, List<BackgroundPackagesContract> lstPackages)
        {
            List<Package_PricingData> _lstPricingData = new List<Package_PricingData>();
            string _pricingDataXML = GetTotalPrice(applicantOrderContract, tenantID);
            if (!string.IsNullOrEmpty(_pricingDataXML))
            {
                _lstPricingData = new List<Package_PricingData>();
                _lstPricingData = GenerateDataFromPricingXML(_pricingDataXML);
            }

            List<Entity.ClientEntity.lkpOrderLineItemStatu> _lstOrderLineItemStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderLineItemStatu>(tenantID);
            string _orderLineItemStatusType = OrderLineItemStatusType.NEW.GetStringValue();
            int OrderLineItemStatusId = _lstOrderLineItemStatus.Where(olists => olists.OLIS_Code == _orderLineItemStatusType
                                                                                        && !olists.OLIS_IsDeleted).FirstOrDefault().OLIS_ID;
            //Get SvcLineItem Status Id from lookup
            string dispatchedExternalVendorCode = SvcLineItemDispatchStatus.NOT_DISPATCHED.GetStringValue();
            short PSLI_DispatchedExternalVendor = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSvcLineItemDispatchStatu>(tenantID)
                                                              .FirstOrDefault(cnd => cnd.SLIDS_Code == dispatchedExternalVendorCode).SLIDS_ID;


            Entity.ClientEntity.Order order = new Entity.ClientEntity.Order();

            GenerateGroupedAmount(applicantOrderContract, tenantID);
            //Don't save billing code in case payment type is not invoice without approval
            var cbiBillingCode = string.Empty;
            var cbiUniqueCode = string.Empty;
            if (applicantOrderContract.lstPaymentGrouping.Any(x => x.PaymentModeCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue()) && !applicantOrderContract.CbiBillingCode.IsNullOrEmpty() && !applicantOrderContract.CbiUniqueId.IsNullOrEmpty())
            {
                cbiBillingCode = applicantOrderContract.CbiBillingCode;
                cbiUniqueCode = applicantOrderContract.CbiUniqueId;
            }
            // update user cellular information
            SMSNotificationManager.SaveUpdateSMSData(Convert.ToInt32(applicantOrderContract.userInfo.OrganizationUserID), applicantOrderContract.userInfo.SMSPhoneNumber, currentLoggedInUserID, applicantOrderContract.userInfo.IsReceiveTextNotification);

            //Update user 
            AddUpdateUser(applicantOrderContract.userInfo, currentLoggedInUserID, tenantID);
            order.OrganizationUserProfileID = applicantOrderContract.userInfo.OrganizationUserProfileID;
            order.IsDeleted = false;
            order.CreatedByID = currentLoggedInUserID;
            order.CreatedOn = DateTime.Now;
            order.SelectedNodeID = applicantOrderContract.SelectedHierarchyNodeID;
            order.HierarchyNodeID = GetHierarchyNodeIDByPackageType(tenantID, applicantOrderContract);
            order.ArchiveStateID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(tenantID).Where(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()).FirstOrDefault().AS_ID;
            if (applicantOrderContract.GrandTotal > AppConsts.NONE)
            {
                if (applicantOrderContract.lstPaymentGrouping.FirstOrDefault().PaymentModeCode.Contains("Additional"))
                {
                    var paymentCode = applicantOrderContract.lstPaymentGrouping.FirstOrDefault().PaymentModeCode.Split('-')[0];
                    order.OrderStatusID = GetOrderStatusId(tenantID, paymentCode);
                }
                else
                    order.OrderStatusID = GetOrderStatusId(tenantID, applicantOrderContract.lstPaymentGrouping.FirstOrDefault().PaymentModeCode);
            }
            else
            {
                order.OrderStatusID = GetOrderStatusCode(tenantID, ApplicantOrderStatus.Paid.GetStringValue());
            }
            //  order.OrderStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatu>(tenantID).Where(cond => cond.Code == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()).FirstOrDefault().OrderStatusID;

            order.OrderRequestTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderRequestType>(tenantID).Where(cond => cond.ORT_Code == OrderRequestType.NewOrder.GetStringValue()).FirstOrDefault().ORT_ID;
            order.OrderPackageType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderPackageType>(tenantID).Where(cond => cond.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            order.OrderDate = DateTime.Now;
            order.OrderMachineIP = applicantOrderContract.ClientMachineIP;
            order.TotalPrice = Convert.ToDecimal("0.0000");
            order.GrandTotal = applicantOrderContract.GrandTotal;
            order.PaymentOptionID = applicantOrderContract.lstPaymentGrouping.FirstOrDefault().PaymentModeId;
            order.OriginalSettlementPrice = Convert.ToDecimal("0.0000");

            string _orderNumber = string.Empty;
            _orderNumber = "#OrderID#" + "-" + tenantID + "-" + SysXUtils.GenerateRandomNo(2) + "-" + SysXUtils.RandomString(2, false) + "-" + applicantOrderContract.LocationDetail.LocationId;
            order.OrderNumber = _orderNumber;

            Entity.ClientEntity.TransactionGroup _transactionGrp = new Entity.ClientEntity.TransactionGroup();
            _transactionGrp.Order = order;
            _transactionGrp.TG_TxnDate = DateTime.Now;
            _transactionGrp.TG_CreatedByID = currentLoggedInUserID;
            _transactionGrp.TG_CreatedOn = DateTime.Now;

            if (applicantOrderContract.bkgPackageID > AppConsts.NONE)
            {
                Entity.ClientEntity.BkgOrder BkgOrder = order.BkgOrders.FirstOrDefault();
                BkgOrder = new Entity.ClientEntity.BkgOrder();

                BkgOrder.BOR_IsAdminOrder = false;
                BkgOrder.BOR_IsDeleted = false;
                BkgOrder.BOR_CreatedByID = currentLoggedInUserID;
                BkgOrder.BOR_CreatedOn = DateTime.Now;
                BkgOrder.BOR_ArchiveStateID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(tenantID).Where(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()).FirstOrDefault().AS_ID;
                BkgOrder.BOR_OrderStatusTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatusType>(tenantID).Where(cond => cond.Code == OrderStatusType.PAYMENTPENDING.GetStringValue()).FirstOrDefault().OrderStatusTypeID;
                BkgOrder.BOR_OrderResultsRequestedByApplicant = false;
                BkgOrder.BOR_IsArchived = false;
                BkgOrder.BOR_TotalPrice = applicantOrderContract.TotalPrice;
                BkgOrder.BOR_GrandTotal = applicantOrderContract.GrandTotal;
                BkgOrder.BOR_OrganizationUserProfileID = applicantOrderContract.userInfo.OrganizationUserProfileID;

                if (BkgOrder.BkgOrderPackages.Count <= AppConsts.NONE || BkgOrder.BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted).ToList().Count <= AppConsts.NONE)
                {
                    BkgOrder.BkgOrderPackages = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackage>();
                }

                int pkgId = applicantOrderContract.bkgPackageID;
                var _pkgPricingData = _lstPricingData.Where(pd => pd.PackageId == pkgId).FirstOrDefault();

                List<Entity.ClientEntity.BkgPackageSvcGroup> lkpSvcGroupIds = BackgroundProcessOrderManager.GetBkgSvcGroupByBkgPkgId(pkgId, tenantID);

                applicantOrderContract.lstSvcAttributeGrps = BackgroundSetupManager.GetServiceAttributeGroupsByTenant(tenantID);
                //BkgOrderPackge
                Entity.ClientEntity.BkgOrderPackage bkgOrderPackage = new Entity.ClientEntity.BkgOrderPackage();
                bkgOrderPackage.BOP_BasePrice = Convert.ToDecimal("0.0000");
                bkgOrderPackage.BOP_TotalLineItemPrice = BkgOrder.BOR_TotalPrice;
                bkgOrderPackage.BOP_CreatedByID = currentLoggedInUserID;
                bkgOrderPackage.BOP_CreatedOn = DateTime.Now;
                bkgOrderPackage.BOP_IsDeleted = false;
                bkgOrderPackage.BOP_BkgPackageHierarchyMappingID = applicantOrderContract.bkgPkgHierarchyMappingID;

                //BkgOrderpackageSvcGroup
                bkgOrderPackage.BkgOrderPackageSvcGroups = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackageSvcGroup>();
                foreach (Entity.ClientEntity.BkgPackageSvcGroup svcGrp in lkpSvcGroupIds)
                {
                    Entity.ClientEntity.BkgOrderPackageSvcGroup bkgOrderPkgSvcGroup = new Entity.ClientEntity.BkgOrderPackageSvcGroup();

                    bkgOrderPkgSvcGroup.OPSG_IsDeleted = false;
                    bkgOrderPkgSvcGroup.OPSG_CreatedByID = currentLoggedInUserID;
                    bkgOrderPkgSvcGroup.OPSG_CreatedOn = DateTime.Now;
                    bkgOrderPkgSvcGroup.OPSG_BkgSvcGroupID = svcGrp.BPSG_BkgSvcGroupID ?? 0;
                    bkgOrderPkgSvcGroup.OPSG_SvcGrpStatusTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpStatusType>(tenantID)
                                            .Where(cond => cond.BSGS_StatusCode == BkgSvcGrpStatusType.NEW.GetStringValue()).FirstOrDefault().BSGS_ID;
                    bkgOrderPkgSvcGroup.OPSG_SvcGrpReviewStatusTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpReviewStatusType>(tenantID)
                                            .Where(cond => cond.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.NEW.GetStringValue()).FirstOrDefault().BSGRS_ID;

                    bkgOrderPkgSvcGroup.BkgOrderPackageSvcs = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackageSvc>();


                    List<int> lkpSvcIds = BackgroundProcessOrderManager.GetBackgroundServiceIdsBysvcGrpId(svcGrp.BPSG_BkgSvcGroupID.Value, pkgId, tenantID);

                    int BPS_ID = svcGrp.BkgPackageSvcs.FirstOrDefault().BPS_ID;


                    foreach (int SvcId in lkpSvcIds)
                    {
                        Entity.ClientEntity.BkgOrderPackageSvc bkgOrderPkgSvc = new Entity.ClientEntity.BkgOrderPackageSvc();
                        bkgOrderPkgSvc.BOPS_IsDeleted = false;
                        bkgOrderPkgSvc.BOPS_CreatedByID = currentLoggedInUserID;
                        bkgOrderPkgSvc.BOPS_CreatedOn = DateTime.Now;
                        bkgOrderPkgSvc.BOPS_BackgroundServiceID = SvcId;

                        if (!_pkgPricingData.IsNullOrEmpty() && !_pkgPricingData.lstOrderLineItems.IsNullOrEmpty())
                        {
                            List<OrderLineItem_PricingData> _lstLineItems = _pkgPricingData.lstOrderLineItems.Where(oli => oli.PackageServiceId == BPS_ID).ToList();
                            foreach (var _lineItem in _lstLineItems)
                            {
                                #region Add BkgOrderPackageSvcLineItem Table Data
                                Entity.ClientEntity.BkgOrderPackageSvcLineItem _bkgOrdPkgSvcLineItem = new Entity.ClientEntity.BkgOrderPackageSvcLineItem();
                                _bkgOrdPkgSvcLineItem.BkgOrderPackageSvc = bkgOrderPkgSvc;
                                _bkgOrdPkgSvcLineItem.PSLI_OrderLineItemStatusID = OrderLineItemStatusId;
                                _bkgOrdPkgSvcLineItem.PSLI_ServiceItemID = _lineItem.PackageServiceItemId;
                                _bkgOrdPkgSvcLineItem.PSLI_IsDeleted = false;
                                _bkgOrdPkgSvcLineItem.PSLI_CreatedByID = currentLoggedInUserID;
                                _bkgOrdPkgSvcLineItem.PSLI_CreatedOn = DateTime.Now;
                                _bkgOrdPkgSvcLineItem.PSLI_DispatchedExternalVendor = PSLI_DispatchedExternalVendor;
                                _bkgOrdPkgSvcLineItem.PSLI_NeedsExternalDispatch = true;
                                _bkgOrdPkgSvcLineItem.PSLI_Description = _lineItem.Description;

                                #endregion

                                if (!_lineItem.Price.IsNullOrEmpty())
                                {
                                    #region Add Transaction Table Data if Line Item Price is available
                                    if (!_lineItem.PackageOrderItemPriceId.IsNullOrEmpty() && _lineItem.PackageOrderItemPriceId != AppConsts.NONE)
                                    {
                                        Entity.ClientEntity.Transaction _transaction = new Entity.ClientEntity.Transaction();
                                        _transaction.TransactionGroup = _transactionGrp;
                                        _transaction.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                        _transaction.TD_PackageServiceItemPriceID = _lineItem.PackageOrderItemPriceId;
                                        _transaction.TD_Amount = _lineItem.Price;
                                        _transaction.TD_IsDeleted = false;
                                        _transaction.TD_CreatedByID = currentLoggedInUserID;
                                        _transaction.TD_CreatedOn = DateTime.Now;
                                        _transaction.TD_Description = _lineItem.PriceDescription;
                                        _bkgOrdPkgSvcLineItem.Transactions.Add(_transaction);
                                    }
                                    #endregion
                                }
                                if (!_lineItem.lstFees.IsNullOrEmpty())
                                {
                                    foreach (var fee in _lineItem.lstFees)
                                    {
                                        #region Add Transaction Table Data if Line Item Fees is available
                                        if (!fee.PackageOrderItemFeeId.IsNullOrEmpty() && fee.PackageOrderItemFeeId != AppConsts.NONE)
                                        {
                                            Entity.ClientEntity.Transaction _transaction = new Entity.ClientEntity.Transaction();
                                            _transaction.TransactionGroup = _transactionGrp;
                                            _transaction.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                            _transaction.TD_PackageServiceItemFeeID = fee.PackageOrderItemFeeId;
                                            _transaction.TD_Amount = fee.Amount;
                                            _transaction.TD_IsDeleted = false;
                                            _transaction.TD_CreatedByID = currentLoggedInUserID;
                                            _transaction.TD_CreatedOn = DateTime.Now;
                                            _transaction.TD_Description = fee.Description;
                                            _bkgOrdPkgSvcLineItem.Transactions.Add(_transaction);
                                        }
                                        #endregion
                                    }
                                }

                                foreach (var _bkgSvcAttDataGroup in _lineItem.lstBkgSvcAttributeDataGroup)
                                {
                                    int? _instanceId = AppConsts.NONE;
                                    if (_bkgSvcAttDataGroup.InstanceId != AppConsts.NONE)
                                        _instanceId = GetInstanceId(applicantOrderContract, _bkgSvcAttDataGroup.AttributeGroupId, _bkgSvcAttDataGroup.InstanceId);
                                    else
                                        _instanceId = null;

                                    if (_instanceId != AppConsts.NONE)
                                    {
                                        Entity.ClientEntity.BkgOrderLineItemDataMapping _lineItemDataMapping = new Entity.ClientEntity.BkgOrderLineItemDataMapping();

                                        _lineItemDataMapping.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                        _lineItemDataMapping.OLIDM_BkgSvcAttributeGroupID = _bkgSvcAttDataGroup.AttributeGroupId;
                                        _lineItemDataMapping.OLIDM_InstanceID = _instanceId;
                                        _lineItemDataMapping.OLIDM_CreatedByID = currentLoggedInUserID;
                                        _lineItemDataMapping.OLIDM_CreatedOn = DateTime.Now;
                                        _lineItemDataMapping.OLIDM_IsDeleted = false;
                                        _bkgOrdPkgSvcLineItem.BkgOrderLineItemDataMappings.Add(_lineItemDataMapping);

                                        foreach (var _attrData in _bkgSvcAttDataGroup.lstAttributeData)
                                        {
                                            Entity.ClientEntity.BkgOrderLineItemDataUsedAttrb _lineItemDataUsedAttr = new Entity.ClientEntity.BkgOrderLineItemDataUsedAttrb();
                                            _lineItemDataUsedAttr.BkgOrderLineItemDataMapping = _lineItemDataMapping;
                                            _lineItemDataUsedAttr.OLIDUA_BkgAttributeGroupMappingID = _attrData.AttributeGroupMappingID;
                                            _lineItemDataUsedAttr.OLIDUA_AttributeValue = _attrData.AttributeValue;

                                            _lineItemDataMapping.BkgOrderLineItemDataUsedAttrbs.Add(_lineItemDataUsedAttr);
                                        }
                                    }
                                }
                            }
                        }
                        bkgOrderPkgSvcGroup.BkgOrderPackageSvcs.Add(bkgOrderPkgSvc);
                    }
                    bkgOrderPackage.BkgOrderPackageSvcGroups.Add(bkgOrderPkgSvcGroup);
                }
                BkgOrder.BkgOrderPackages.Add(bkgOrderPackage);

                #region Add BkgOrderEventHistory table data

                Entity.ClientEntity.BkgOrderEventHistory _bkgOrderEventHistory = new Entity.ClientEntity.BkgOrderEventHistory
                {
                    BkgOrder = BkgOrder,
                    BOEH_OrderEventDetail = AppConsts.Bkg_Order_Created,
                    BOEH_IsDeleted = false,
                    BOEH_CreatedByID = currentLoggedInUserID,
                    BOEH_CreatedOn = DateTime.Now,
                    BOEH_EventHistoryId = order.OrderStatusID
                };

                #endregion

                order.BkgOrders.Add(BkgOrder);
            }

            order = BackgroundProcessOrderManager.SaveAdminOrderDetails(order, tenantID);
            lstPackages[0].TotalBkgPackagePrice = applicantOrderContract.TotalPrice;
            ComplianceDataManager.SaveCABSServiceOrderDetails(tenantID, lstPackages, order, currentLoggedInUserID, null, true, false);
            int bkgOrderID = order.BkgOrders.FirstOrDefault().BOR_ID;
            SaveCustomFormDetails(applicantOrderContract, bkgOrderID, tenantID, currentLoggedInUserID,order.OrderID);
            var lstOrderPackageTypes = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderPackageType>(tenantID);

            var _bkgPkgTypeId = lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            var _compliancePkgTypeId = lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            Dictionary<string, string> _dicInvoiceNumbers = new Dictionary<string, string>();
            // Handle Payment by institution case
            decimal _totalAmount = 0;
            foreach (var poId in applicantOrderContract.lstPaymentGrouping)
            {
                var _price = applicantOrderContract.GrandTotal;
                _totalAmount = _price.IsNull() ? AppConsts.NONE : _price;

                // UAT-3850
                if (!cbiBillingCode.IsNullOrEmpty() && !cbiUniqueCode.IsNullOrEmpty())
                {
                    if (_totalAmount >= Convert.ToDecimal(applicantOrderContract.BillingCodeAmount))
                    {
                        if (poId.PaymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
                            poId.TotalAmount = _totalAmount = Convert.ToDecimal(applicantOrderContract.BillingCodeAmount);
                        else
                            poId.TotalAmount = _totalAmount = _totalAmount - Convert.ToDecimal(applicantOrderContract.BillingCodeAmount);
                    }
                }

                //end 
            }
            foreach (var poId in applicantOrderContract.lstPaymentGrouping)
            {
                _dicInvoiceNumbers.AddRange(ComplianceDataManager.SaveOrderPaymentDetail(tenantID, poId, order));
            }            

            if (applicantOrderContract.bkgPackageID > AppConsts.NONE)
                StoredProcedureManagers.UpdateExtServiceVendorforLineItems(order.OrderID, tenantID);
            string _prevStatus = ApplicantOrderStatus.Paid.GetStringValue();
            int orderStatusId = ComplianceDataManager.GetOrderStatusList(tenantID)
                                                       .Where(orderSts => orderSts.Code.ToLower() == _prevStatus.ToLower() && !orderSts.IsDeleted)
                                                       .FirstOrDefault().OrderStatusID;
            var _lstOPDs = order.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false).ToList();

            List<Entity.ClientEntity.lkpEventHistory> lstEventHistory = LookupManager.GetLookUpData<Entity.ClientEntity.lkpEventHistory>(tenantID).Where(eh => !eh.EH_IsDeleted).ToList();
            List<Entity.ClientEntity.lkpOrderStatusType> lstOrderStatusType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatusType>(tenantID).ToList();

            foreach (var opd in _lstOPDs)
            {
                var _orderPaymentDetailId = opd.OPD_ID;
                var _orderStatusId = opd.OPD_OrderStatusID;
                var _paymentTypeCode = opd.lkpPaymentOption.Code;
                if (_orderStatusId == orderStatusId || (_paymentTypeCode == PaymentOptions.Credit_Card.GetStringValue() && opd.OPD_Amount == 0) || (_paymentTypeCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                {
                    ComplianceDataManager.UpdateOrderStatusForInvoiceWithoutApproval(tenantID, order.OrderID, ApplicantOrderStatus.Paid.GetStringValue(),
                                              currentLoggedInUserID, applicantOrderContract.userInfo.OrganizationUserID, _orderPaymentDetailId, lstEventHistory, lstOrderStatusType);
                }
            }

            if (!cbiBillingCode.IsNullOrEmpty())
            {
                FingerPrintDataManager.SubmitOrderBillingCodeMapping(tenantID, order.OrderID, cbiBillingCode, currentLoggedInUserID);
            }
            ReserveSlotContract reserveSlotContract = SubmitApplicantAppointment(order.OrderID, applicantOrderContract, tenantID, currentLoggedInUserID, cbiBillingCode, cbiUniqueCode);
            Boolean isCreditCardPayment = applicantOrderContract.lstPaymentGrouping.Any(x => x.PaymentModeCode == PaymentOptions.Credit_Card.GetStringValue());
            if (isCreditCardPayment)
            {
                FingerPrintSetUpManager.ChangeAppointmentStatus(order.OrderID, currentLoggedInUserID, true, applicantOrderContract.LocationDetail.ReserverSlotID, false);
            }
            //Insert data to Invoice tables
            ComplianceDataManager.SaveOrderPaymentInvoice(tenantID, order.OrderID, currentLoggedInUserID, false);

            if (!reserveSlotContract.IsNullOrEmpty() && reserveSlotContract.ApplicantAppointmentID > AppConsts.NONE && !applicantOrderContract.lstPaymentGrouping.IsNullOrEmpty() && !applicantOrderContract.lstPaymentGrouping.Any(x => x.PaymentModeCode == PaymentOptions.Credit_Card.GetStringValue()))
            {
                if (string.IsNullOrEmpty(reserveSlotContract.ErrorMsg))
                {
                    var appointmentOderScheduleData = FingerPrintSetUpManager.GetAppointmentOrderDetailData(currentLoggedInUserID, false, Convert.ToString(tenantID), reserveSlotContract.ApplicantAppointmentID);
                    AppointmentSlotContract AppSlotContract = new AppointmentSlotContract();
                    if (applicantOrderContract.LocationDetail.SlotID > AppConsts.ONE)
                    {
                        AppSlotContract.SlotDate = applicantOrderContract.LocationDetail.SlotDate;
                        AppSlotContract.SlotStartTime = applicantOrderContract.LocationDetail.StartTime.ToString("HH:mm");
                        AppSlotContract.SlotEndTime = applicantOrderContract.LocationDetail.EndTime.ToString("HH:mm");
                    }
                    AppSlotContract.ApplicantOrgUserId = currentLoggedInUserID;
                    AppSlotContract.IsEventType = applicantOrderContract.LocationDetail.IsEventCode;
                    if (applicantOrderContract.LocationDetail.IsEventCode)
                    {
                        AppSlotContract.EventName = applicantOrderContract.LocationDetail.EventName;
                        AppSlotContract.EventDescription = applicantOrderContract.LocationDetail.EventDescription;
                    }
                    AppSlotContract.IsOutOfStateAppointment = applicantOrderContract.LocationDetail.IsOutOfState;

                    ///// Send Order Create Mail
                    FingerPrintSetUpManager.SendOrderCreateMail(appointmentOderScheduleData, AppSlotContract);
                }
            }           
            applicantOrderContract.OrderID = order.OrderID;
            string OrderNumber = order.OrderNumber.Replace("#OrderID#", Convert.ToString(order.OrderID));

            return OrderNumber;
        }



        public static ApplicantOrderContract GetOrderNumberDetails(string orderNumber, int tenantId)
        {
            return BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderNumberDetails(orderNumber);
        }

        private static void GenerateGroupedAmount(ApplicantOrderContract applicantOrderContract, int tenantID)
        {
            List<ApplicantOrderPaymentOptions> lstPkgPaymentOptns = applicantOrderContract.selectedPaymentModeData;
            var _lstClientPaymentOptions = ComplianceDataManager.GetClientPaymentOptions(tenantID);
            applicantOrderContract.lstPaymentGrouping = new List<PkgPaymentGrouping>();

            var _distinctPOIds = lstPkgPaymentOptns.DistinctBy(x => x.poid).ToList();
            foreach (var poItem in _distinctPOIds)
            {
                var _lstPkgs = lstPkgPaymentOptns.Where(po => po.poid == poItem.poid).ToList();

                PkgPaymentGrouping _pkgPayGroup = new PkgPaymentGrouping();
                _pkgPayGroup.PaymentModeCode = _lstClientPaymentOptions.Where(po => po.PaymentOptionID == poItem.poid).FirstOrDefault().Code;
                _pkgPayGroup.PaymentModeId = poItem.poid;
                _pkgPayGroup.TotalAmount = applicantOrderContract.GrandTotal;
                _pkgPayGroup.lstPackages = new Dictionary<string, bool>();
               // _pkgPayGroup.lstPackages.Add(applicantOrderContract.bkgPackageID + "_" + Guid.NewGuid().ToString() + "_Additional", true);
                _pkgPayGroup.lstPackages.Add(applicantOrderContract.bkgPackageID + "_" + Guid.NewGuid().ToString(), true);

                applicantOrderContract.lstPaymentGrouping.Add(_pkgPayGroup);
            }
        }

        public static ReserveSlotContract SubmitApplicantAppointment(int orderId, ApplicantOrderContract appOrderContract, int tenantID, int OrganizationUserID, string CbiBillingCode = null, string CbiUniqueId = null, Boolean isCompleteYourOrderClick = false)
        {
            ReserveSlotContract reserveSlotContract = new ReserveSlotContract();
            reserveSlotContract.SlotID = appOrderContract.LocationDetail.SlotID;
            reserveSlotContract.TenantID = tenantID;
            reserveSlotContract.AppOrgUserID = OrganizationUserID;
            reserveSlotContract.OrderID = orderId;
            reserveSlotContract.LocationID = appOrderContract.LocationDetail.LocationId;
            reserveSlotContract.ReservedSlotID = appOrderContract.LocationDetail.ReserverSlotID;
            reserveSlotContract.IsEventTypeCode = appOrderContract.LocationDetail.IsEventCode;
            reserveSlotContract.IsOutOfState = appOrderContract.LocationDetail.IsOutOfState;
            reserveSlotContract.BillingCode = CbiBillingCode;
            reserveSlotContract.CbiUniqueId = CbiUniqueId;
            reserveSlotContract.IsLocationUpdate = appOrderContract.LocationDetail.IsLocationUpdate;
            if (!reserveSlotContract.IsNullOrEmpty())
                return FingerPrintDataManager.SubmitApplicantAppointment(reserveSlotContract, OrganizationUserID, isCompleteYourOrderClick);
            else
                return new ReserveSlotContract();
        }

        /// <summary>
        /// Get Data of Custom forms and mvr to save.
        /// </summary>
        private static void SaveCustomFormDetails(ApplicantOrderContract applicantOrderContract, int bkgOrderID, int tenantID, int currentLoggedInUserID,int OrderID)
        {
            List<AttributesForCustomFormContract> lstCustomAttribute = applicantOrderContract.lstCustomAttribute;
            List<BackgroundOrderData> lstBkgOrderData = new List<BackgroundOrderData>();
            BackgroundOrderData bkgOrderDataContract = null;
            if (!lstCustomAttribute.IsNullOrEmpty() && lstCustomAttribute.Count > AppConsts.NONE)
            {
                foreach (var lst in lstCustomAttribute.GroupBy(x => x.AttributeGroupId).ToList())
                {
                    bkgOrderDataContract = new BackgroundOrderData();
                    Dictionary<int, string> CustomFormData = new Dictionary<int, string>();
                    bkgOrderDataContract.InstanceId = AppConsts.ONE;
                    bkgOrderDataContract.BkgSvcAttributeGroupId = lst.FirstOrDefault().AttributeGroupId;
                    bkgOrderDataContract.CustomFormId = applicantOrderContract.customFormID;

                    lst.ForEach(x =>
                    {
                        CustomFormData.Add(x.AtrributeGroupMappingId, x.AttributeDataValue);
                    });
                    bkgOrderDataContract.CustomFormData = CustomFormData;
                    lstBkgOrderData.Add(bkgOrderDataContract);
                }
            }
            BackgroundProcessOrderManager.SaveCustomFormDetails(lstBkgOrderData, bkgOrderID, tenantID, currentLoggedInUserID);

            //UAT-4360
            var attributesToFetch = new List<string>
                    {
                        AppConsts.CBIUniqueID,
                        AppConsts.ReasonFingerprinted,
                        AppConsts.BillingORI,
                        AppConsts.AcctName
                    };

            var lstLookUpContract = BALUtils.GetComplianceDataRepoInstance(tenantID)
                .FetchFingerprintOrderKeyData(lstBkgOrderData, attributesToFetch);

            FingerPrintSetUpManager.SaveFingerPrintOrderKeyData(tenantID ,currentLoggedInUserID, lstLookUpContract,OrderID);
        }

        /// <summary>
        /// Generate the Data from the Pricing stored procedure XML
        /// </summary>
        /// <returns></returns>
        private static List<Package_PricingData> GenerateDataFromPricingXML(string _pricingDataXML)
        {
            XDocument doc = XDocument.Parse(_pricingDataXML);

            // GET <package> TAG'S INSIDE <Packages> TAG
            var _packages = doc.Root.Descendants("Packages")
                               .Descendants("Package")
                               .Select(element => element)
                               .ToList();

            List<Package_PricingData> _lstData = new List<Package_PricingData>();
            foreach (var pkg in _packages)
            {

                int _packageId = Convert.ToInt32(pkg.Element("PackageId").Value);
                Package_PricingData _packagePricingData = new Package_PricingData();
                _packagePricingData.PackageId = _packageId;

                // To be removed
                _packagePricingData.TotalBkgPackagePrice = pkg.Element("TotalPrice").Value.IsNullOrEmpty() ? 0 : Convert.ToDecimal(pkg.Element("TotalPrice").Value);

                #region ADD DATA OF <OrderLineItem> TAG'S INSIDE <OrderLineItems> TAG

                var _orderLineItems = pkg.Descendants("OrderLineItems").Descendants("OrderLineItem")
                                         .Select(element => element)
                                         .ToList();

                _packagePricingData.lstOrderLineItems = new List<OrderLineItem_PricingData>();

                foreach (var _ordLineItem in _orderLineItems)
                {
                    OrderLineItem_PricingData _orderLineItem = new OrderLineItem_PricingData();
                    _orderLineItem.PackageSvcGrpID = Convert.ToInt32(_ordLineItem.Element("PackageSvcGrpID").Value);
                    _orderLineItem.PackageServiceId = Convert.ToInt32(_ordLineItem.Element("PackageServiceID").Value);
                    _orderLineItem.PackageServiceItemId = Convert.ToInt32(_ordLineItem.Element("PackageServiceItemID").Value);
                    _orderLineItem.Description = _ordLineItem.Element("Description").Value;

                    _orderLineItem.PackageOrderItemPriceId = string.IsNullOrEmpty(_ordLineItem.Element("PackageOrderItemPriceID").Value) ? AppConsts.NONE :
                                                             Convert.ToInt32(_ordLineItem.Element("PackageOrderItemPriceID").Value);

                    _orderLineItem.Price = string.IsNullOrEmpty(_ordLineItem.Element("Price").Value) ?
                                           AppConsts.NONE : Convert.ToDecimal(_ordLineItem.Element("Price").Value);

                    _orderLineItem.PriceDescription = _ordLineItem.Element("PriceDescription").Value;

                    #region ADD DATA OF <Fee> TAG'S INSIDE  <Fees> TAG

                    var _fees = _ordLineItem.Descendants("Fees").Descendants("Fee")
                                                 .Select(element => element)
                                                 .ToList();

                    _orderLineItem.lstFees = new List<Fee_PricingData>();
                    foreach (var _fee in _fees)
                    {
                        _orderLineItem.lstFees.Add(new Fee_PricingData
                        {
                            Amount = string.IsNullOrEmpty(_fee.Element("Amount").Value)
                                        ? AppConsts.NONE
                                        : Convert.ToDecimal(_fee.Element("Amount").Value),
                            Description = _fee.Element("Description").Value,

                            PackageOrderItemFeeId = string.IsNullOrEmpty(_fee.Element("PackageOrderItemFeeID").Value)
                                                       ? (int?)null
                                                       : Convert.ToInt32(_fee.Element("PackageOrderItemFeeID").Value),
                        });
                    }

                    #endregion

                    #region ADD DATA OF <BkgSvcAttributeDataGroup> TAG

                    var _bkgAttrDataGrps = _ordLineItem.Descendants("BkgSvcAttributeDataGroup")
                                                                   .Select(element => element)
                                                                   .ToList();

                    _orderLineItem.lstBkgSvcAttributeDataGroup = new List<BkgSvcAttributeDataGroup_PricingData>();
                    foreach (var _bkgAttrDataGrp in _bkgAttrDataGrps)
                    {
                        int _instanceId = AppConsts.NONE;

                        if (!string.IsNullOrEmpty(_bkgAttrDataGrp.Element("InstanceID").Value))
                            _instanceId = Convert.ToInt32(_bkgAttrDataGrp.Element("InstanceID").Value);

                        BkgSvcAttributeDataGroup_PricingData _bkgSvcAttrDataGrpPricingData = new BkgSvcAttributeDataGroup_PricingData
                        {
                            AttributeGroupId = Convert.ToInt32(_bkgAttrDataGrp.Element("AttributeGroupID").Value),
                            InstanceId = _instanceId
                        };

                        //if (String.IsNullOrEmpty(_instanceId))
                        var _attributeData = _bkgAttrDataGrp.Descendants("BkgSvcAttributes").Descendants("BkgSvcAttributeData")
                                                      .Select(element => element)
                                                      .ToList();

                        _bkgSvcAttrDataGrpPricingData.lstAttributeData = new List<AttributeData_PricingData>();
                        foreach (var _attrData in _attributeData)
                        {
                            #region ADD DATA OF BkgSvcAttributeData TAG

                            string _attributeGrpMappingId = _attrData.Element("AttributeGroupMapingID").Value;

                            if (!string.IsNullOrEmpty(_attributeGrpMappingId))
                            {
                                _bkgSvcAttrDataGrpPricingData.lstAttributeData.Add(new AttributeData_PricingData
                                {
                                    AttributeGroupMappingID = Convert.ToInt32(_attributeGrpMappingId),
                                    AttributeValue = _attrData.Element("Value").Value
                                });
                            }

                            #endregion
                        }

                        _orderLineItem.lstBkgSvcAttributeDataGroup.Add(_bkgSvcAttrDataGrpPricingData);
                    }
                    #endregion

                    _packagePricingData.lstOrderLineItems.Add(_orderLineItem);
                }

                #endregion

                _lstData.Add(_packagePricingData);
            }
            return _lstData;
        }

        /// <summary>
        /// Get the instanceId, based on the SvcAttributeGroup
        /// </summary>
        /// <param name="orderDataContract"></param>
        /// <param name="attributeGrpId"></param>
        /// <param name="uniqueIdentifier"></param>
        /// <returns></returns>
        private static int GetInstanceId(ApplicantOrderContract orderDataContract, int attributeGrpId, int sequenceId)
        {
            Entity.ClientEntity.BkgSvcAttributeGroup _attributeGrp = orderDataContract.lstSvcAttributeGrps.Where(attrGrp => attrGrp.BSAD_ID == attributeGrpId
                                                                        && !attrGrp.BSAD_IsDeleted).FirstOrDefault();

            if (_attributeGrp.IsNullOrEmpty())
                return AppConsts.MINUS_ONE;

            if (_attributeGrp.BSAD_Name == SvcAttributeGroups.PERSONAL_INFORMATION.GetStringValue())
            {
                return orderDataContract.userInfo.OrganizationUserProfileID;
            }
            //else if (_attributeGrp.BSAD_Name == SvcAttributeGroups.RESIDENTIAL_HISTORY.GetStringValue())
            //{
            //    return orderDataContract.lstResidentialHistoryProfile.Where(rhp => rhp.RHIP_SequenceOrder == sequenceId).FirstOrDefault().RHIP_ID;
            //}
            else if (_attributeGrp.BSAD_Name == SvcAttributeGroups.PERSONAL_ALIAS.GetStringValue())
            {
                return orderDataContract.userInfo.PersonAliasList.Where(pap => pap.AliasSequenceId == sequenceId).FirstOrDefault().ID;
            }
            return sequenceId;
        }

        #endregion

        public static List<AppointmentSlotContract> GetAppointmentSlotsAvailable(int locationID)
        {
            List<AppointmentSlotContract> lstAppointmentSlotDeatils = FingerPrintSetUpManager.GetAppointmentSlotByDate(locationID);
            return lstAppointmentSlotDeatils;
        }
        public static string GetUserIdFromOrgUserId(int currentUserId)
        {
            return SecurityManager.GetUserIdFromOrgUserId(currentUserId);
        }
        public static void UpdateOrderForOnlinePayment(Entity.ClientEntity.OnlinePaymentTransaction onlinePaymentTransaction, NameValueCollection transactionDetails, int authorizeDotNetUserId, int orderID, int tenantID, int currentLoggedInUserId, Boolean isAlreadyPlacedOrder, List<Entity.ClientEntity.OrderPaymentDetail> lstOrderPaymentDetails)
        {
            string amountPaid = transactionDetails["x_amount"];
            string responseCode = transactionDetails["x_response_code"];
            string responseReasonCode = transactionDetails["x_response_reason_code"];
            string responseReasonText = transactionDetails["x_response_reason_text"].ToLower();
            string successResponseText = "This transaction has been approved.";

            if (responseCode == "1" && responseReasonCode == "1" && responseReasonText == successResponseText.ToLower() && orderID > AppConsts.NONE)
            {

                Entity.ClientEntity.Order updatedOrder = ComplianceDataManager.UpdateOrderForOnlinePayment(amountPaid, authorizeDotNetUserId, orderID, tenantID, onlinePaymentTransaction);
                FingerPrintSetUpManager.ChangeAppointmentStatus(orderID, currentLoggedInUserId, false,-1,true);
                AppointmentSlotContract AppSlotContract = FingerPrintDataManager.GetBkgOrderWithAppointmentData(tenantID, orderID, currentLoggedInUserId);
                if (!AppSlotContract.IsNullOrEmpty() && AppSlotContract.ApplicantAppointmentId > AppConsts.NONE)
                {
                    var appointmentOderScheduleData = FingerPrintSetUpManager.GetAppointmentOrderDetailData(currentLoggedInUserId, false, Convert.ToString(tenantID), AppSlotContract.ApplicantAppointmentId);
                    //        AppointmentSlotContract AppSlotContract = new AppointmentSlotContract();
                    var creditCardCode = PaymentOptions.Credit_Card.GetStringValue();                   
                    if (!isAlreadyPlacedOrder || (lstOrderPaymentDetails.Any(x => x.lkpPaymentOption.IsNotNull() && x.lkpPaymentOption.Code == creditCardCode ))) //SendOrderCreateMail in case of new order
                    {
                        if (AppSlotContract.SlotID > AppConsts.ONE || AppSlotContract.IsOutOfStateAppointment)
                        {
                            AppSlotContract.ApplicantOrgUserId = currentLoggedInUserId;
                            AppSlotContract.IsEventType = AppSlotContract.IsOnsiteAppointment.HasValue ? AppSlotContract.IsOnsiteAppointment.Value : false;
                            ///// Send Order Create Mail                  
                            FingerPrintSetUpManager.SendOrderCreateMail(appointmentOderScheduleData, AppSlotContract);
                        }
                    }
                    //Method to update EDS related data in CustomFormOrderData and bkgorderPackageServiceLineItem

                    ComplianceDataManager.InsertAutomaticInvitationLog(tenantID, orderID, AppConsts.NONE); //UAT-2388

                }
            }
        }
        public static long CreateNewAuthNetCustomerProfile(Entity.AuthNetCustomerProfile authNetCustomerProfile, int tenantId)
        {
            return ComplianceDataManager.CreateNewAuthNetCustomerProfile(authNetCustomerProfile, tenantId);
        }

        #region Create Receipt

        //Order confirmation HTML file.
        public static string CreateHtmlFile(ApplicantOrderContract orderData, int tenantID
                                        , Dictionary<string, string> dictLocalizedKeyValues)
        {
            string strHtmlCode = string.Empty;
            bool isEnglishLanguage = true;
            string receiptTemplatePath = "";
            if (!string.IsNullOrEmpty(orderData.LanguageCode))
                isEnglishLanguage = orderData.LanguageCode == Languages.ENGLISH.GetStringValue();

            orderData.OrderID = Convert.ToInt32(orderData.OrderNumber.Split('-').First());

            if (isEnglishLanguage)
            {
                receiptTemplatePath = ConfigurationManager.AppSettings["ReceiptTemplateLocation"].IsNullOrEmpty() ?
                                       string.Empty : ConfigurationManager.AppSettings["ReceiptTemplateLocation"].ToString();
            }
            else
            {
                receiptTemplatePath = ConfigurationManager.AppSettings["ReceiptTemplateLocationSpanish"].IsNullOrEmpty() ?
                                       string.Empty : ConfigurationManager.AppSettings["ReceiptTemplateLocationSpanish"].ToString();
            }

            if (receiptTemplatePath.IsNullOrEmpty())
            {
                throw new Exception("Please provide receipt template location.");
            }
            //if (!receiptTemplatePath.EndsWith("\\"))
            //{
            //    receiptTemplatePath += "\\";
            //}

            strHtmlCode = File.ReadAllText(receiptTemplatePath);

            //strHtmlCode = File.ReadAllText("C:\\OfficialData\\Projects\\ADB_Release161\\OrderSummary_Receipt_Template_With_PlaceHolder.txt");
            strHtmlCode = ReplacePlaceHolders(strHtmlCode, orderData, tenantID, dictLocalizedKeyValues);

            return SavePageHtmlContet(tenantID, strHtmlCode, orderData.OrderID.ToString(), orderData.OrderNumber);
        }

        private static string SavePageHtmlContet(int tenantID, string pageHtml, string orderNo, string orderNumber, string fileGuid = null)
        {
            if (!string.IsNullOrEmpty(pageHtml))
            {
                // ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

                Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(tenantID);
                string applicationUrl = string.Empty;
                int WebSiteId = webSite.WebSiteID; //Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID));
                string LoginPageImageUrl = string.Empty;
                //if (WebSiteId > AppConsts.NONE && applicantOrderCart.IsLocationServiceTenant)
                LoginPageImageUrl = WebSiteManager.GetWebSiteHeaderHtml(WebSiteId);
                if (!LoginPageImageUrl.IsNullOrEmpty())
                {
                    LoginPageImageUrl = LoginPageImageUrl.Substring(LoginPageImageUrl.IndexOf("src=") + 4, (LoginPageImageUrl.IndexOf("/></div>") - (LoginPageImageUrl.IndexOf("src=") + 4)));
                    LoginPageImageUrl = LoginPageImageUrl.Replace("..", "").Replace("/", "\\").Replace("\"", "");
                }
                string LocTenantCompanyName = SecurityManager.GetLocationTenantCompanyName();
                string filePath = string.Empty;
                string fileIdentifier = string.Empty;
                string orderConfirnmationfileName = string.Empty;
                string applicantFileLocation = string.Empty;
                filePath = ConfigurationManager.AppSettings["TemporaryFileLocation"].ToString();
                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }
                filePath += @"HTMLConversion\";
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                Guid Id;
                if (!fileGuid.IsNullOrEmpty())
                {
                    Id = Guid.Parse(fileGuid);
                }
                else
                {
                    Id = Guid.NewGuid();
                }

                string fileName = "TempFile_" + Id + ".txt";
                if (!string.IsNullOrEmpty(orderNo))
                {
                    orderConfirnmationfileName = "OrderConfirmation" + "_" + orderNumber;
                }
                fileIdentifier = Convert.ToString(Id);
                if (!File.Exists(fileName))
                {
                    filePath = Path.Combine(filePath, fileName);

                    using (FileStream fs = File.Create(filePath))
                    {
                        //UAT 1212 Complio Receipt (Order Summary) Enhancement
                        StringBuilder customHtmlString = new StringBuilder();

                        //With address header
                        //if (applicantOrderCart.IsLocationServiceTenant)
                        customHtmlString.Append("<div><img src=\"" + LoginPageImageUrl + "\" style=\"height: 80px;\"><div style=\"float: right; height: 80px; width: 200px;\"><p style=\"line-height:2px;\">" + LocTenantCompanyName + "</p><p style=\"line-height:2px;\"> 110 16th Street 8th Floor</p><p style=\"line-height:2px;\"> Denver, CO 80202</p> <p style=\"line-height:2px;\">Phone: (720) 292-2722</p> </div><hr /></div>");
                        //else
                        //    customHtmlString.Append("<div><img src=\"\\Resources\\Mod\\Shared\\images\\login\\adbRecieptLogo.png\" style=\"height: 80px;\"><div style=\"float: right; height: 80px; width: 200px;\"><p style=\"line-height:2px;\">American DataBank, L.L.C.</p><p style=\"line-height:2px;\"> 110 16th Street 8th Floor</p><p style=\"line-height:2px;\"> Denver, CO 80202</p> <p style=\"line-height:2px;\">Phone: (800) 200-0853</p> </div><hr /></div>");
                        int startIndex = pageHtml.IndexOf("150%");
                        StringBuilder tmp = new StringBuilder();
                        tmp.Append(pageHtml);
                        if (startIndex > -1)
                        {
                            tmp.Replace("150%", "120%", startIndex, 4);
                        }

                        customHtmlString.Append(tmp.ToString());
                        byte[] info = new UTF8Encoding(true).GetBytes(customHtmlString.ToString());
                        fs.Write(info, 0, info.Length);
                    }
                    //Check whether use AWS S3, true if need to use
                    bool aWSUseS3 = false;
                    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    {
                        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                    }
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"].ToString();
                        if (!applicantFileLocation.EndsWith("\\"))
                        {
                            applicantFileLocation += "\\";
                        }
                        applicantFileLocation += @"HTMLConversion\";
                        if (!Directory.Exists(applicantFileLocation))
                        {
                            Directory.CreateDirectory(applicantFileLocation);
                        }
                        applicantFileLocation = Path.Combine(applicantFileLocation, fileName);
                        MoveFile(filePath, applicantFileLocation);
                    }
                    else
                    {
                        applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"].ToString();
                        if (!applicantFileLocation.EndsWith("//"))
                        {
                            applicantFileLocation += "//";
                        }
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        string destFolder = applicantFileLocation + @"HTMLConversion/";
                        string returnFilePath = objAmazonS3.SaveDocument(filePath, fileName, destFolder);
                        applicantFileLocation = returnFilePath; //destFolder + fileName;
                        try
                        {
                            if (!string.IsNullOrEmpty(filePath))
                                File.Delete(filePath);
                        }
                        catch (Exception) { }
                    }
                    //SaveFilePath(_filePath, Id, orderConfirnmationfileName);
                    SaveFilePath(applicantFileLocation, Id, orderConfirnmationfileName);
                    return fileIdentifier;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        private static bool SaveFilePath(string filePath, Guid Id, string fileName)
        {
            TempFile tempFile = new TempFile();
            tempFile.TF_Path = filePath;
            tempFile.TF_Identifier = Id;
            tempFile.TF_IsDeleted = false;
            tempFile.TF_CreatedOn = DateTime.Now;
            tempFile.TF_CreatedByID = 1;
            tempFile.TF_FileName = fileName;
            return SecurityManager.SavePageHtmlContentLocation(tempFile);
        }

        /// <summary>
        /// Move file to other location
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        private static void MoveFile(string sourceFilePath, string destinationFilePath)
        {
            if (!string.IsNullOrEmpty(sourceFilePath))
            {
                File.Copy(sourceFilePath, destinationFilePath);
            }
            try
            {
                if (!string.IsNullOrEmpty(sourceFilePath))
                    File.Delete(sourceFilePath);
            }
            catch (Exception) { }
        }

        private static string ReplacePlaceHolders(string strHtmlCode, ApplicantOrderContract orderData, int tenantID,
                                                    Dictionary<string, string> dictLocalizedKeyValues)
        {
            Entity.ClientEntity.Order order = ComplianceDataManager.GetOrderById(tenantID, orderData.OrderID);

            if (!order.IsNullOrEmpty())
            {
                var ShowMailingAddress = "none";
                var ShowUserAgreement = "none";
                var ShowPaymentInstruction = "none";


                //Order Selection Detail
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.InstitutionHierarchy, order.DeptProgramMapping1.DPM_Label);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.OrderNumber, orderData.OrderNumber);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.TotalPrice, "$ " + Convert.ToString(orderData.GrandTotal));
                //Order Details

                Entity.ClientEntity.lkpPaymentOption paymentOption = order.OrderPaymentDetails.FirstOrDefault().lkpPaymentOption;
                List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
                List<Entity.lkpPaymentOption> masterPaymentOptions = ComplianceDataManager.GetMasterPaymentOptions(tenantID, out lstClientPaymentOptns);
                string paymentInstruction = string.Empty;
                string decryptedSSN = ComplianceSetupManager.GetFormattedString(order.OrganizationUserProfileID, true, tenantID);
                string maskedSSN = ApplicationDataManager.GetMaskedSSN(decryptedSSN);

                if (!masterPaymentOptions.IsNullOrEmpty())
                {
                    var masterPaymentOption = masterPaymentOptions.FirstOrDefault(x => x.Code == paymentOption.Code);
                    paymentInstruction = masterPaymentOption.IsNullOrEmpty() ? string.Empty : masterPaymentOption.InstructionText;
                }



                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PackageName, Convert.ToString(orderData.packageName));
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PaymentType, paymentOption.IsNullOrEmpty() ? string.Empty : paymentOption.Name);

                //Payment Types
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.Amount, "$ " + Convert.ToString(orderData.GrandTotal));
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.TotalFee, "$ " + Convert.ToString(orderData.GrandTotal));


                //Profile Details
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.FirstName, orderData.userInfo.FirstName);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.MiddleName, orderData.userInfo.MiddleName);
                if (!orderData.userInfo.Suffix.IsNullOrEmpty())
                {
                    orderData.userInfo.LastName = orderData.userInfo.LastName + " " + '-' + " " + orderData.userInfo.Suffix;
                }
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.LastName, orderData.userInfo.LastName);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.Gender, Convert.ToString(orderData.userInfo.Gender));
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.DateOfBirth, orderData.userInfo.DOB.HasValue
                    ? ApplicationDataManager.GetMaskDOB(Convert.ToString(orderData.userInfo.DOB.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()))))
                    : string.Empty);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.EmailAddress, Convert.ToString(orderData.userInfo.PrimaryEmail));
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PhoneNumber, string.Format("{0:(###)-###-####}", Convert.ToInt64(orderData.userInfo.PrimaryPhone)));
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.Address, orderData.userInfo.Address);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.City, orderData.userInfo.CityName);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.STATE, orderData.userInfo.StateName);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.Country, orderData.userInfo.CountryName);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.Zip, orderData.userInfo.PostalCode);

                var SSN = ManageSSN(maskedSSN);
                if (!SSN.IsNullOrEmpty())
                {
                    strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ShowSSN, "block");
                    strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.SSN, SSN);

                }
                else
                {
                    strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ShowSSN, "none");

                }
                if (orderData.userInfo.StateName.IsNullOrEmpty())
                    strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ShowState, "none");
                else
                    strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ShowState, "block");

                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ZipLabel, dictLocalizedKeyValues.GetValue("Zip"));


                var employeInfoSectionTitle = string.Empty;
                StringBuilder xmlStringData = new StringBuilder();
                if (orderData.lstCustomAttribute != null && orderData.lstCustomAttribute.Any())
                {
                    xmlStringData.Append("<Attributes>");
                    foreach (var item in orderData.lstCustomAttribute)
                    {
                        xmlStringData.Append("<Attribute><InstanceID>" + item.InstanceID + "</InstanceID><AttributeID>" + item.InstanceID + "</AttributeID><AttributeValue>" + System.Security.SecurityElement.Escape(item.AttributeDataValue) + "</AttributeValue></Attribute>");

                    }
                    xmlStringData.Append("</Attributes>");

                    List<CustomFormAutoFillDataContract> lstAttributes = BackgroundProcessOrderManager.GetConditionsforAttributes(tenantID, xmlStringData, orderData.LanguageCode);
                    if (lstAttributes.Any())
                    {
                        lstAttributes.Where(attr => !string.IsNullOrWhiteSpace(attr.HeaderLabel) && !attr.IsAttributeGroupHidden).ToList().ForEach(attr =>
                        {
                            orderData.lstCustomAttribute.Where(cond => cond.AttributeGroupId == attr.AttributeGroupID).ForEach(cond =>
                            {
                                employeInfoSectionTitle = attr.HeaderLabel;
                                ShowMailingAddress = "block";
                            });
                        });
                    }
                }

                var PaymentType = "Payment Type";
                //  var Amount = "Amount";
                var PaymentInstructionLabel = "Payment Instruction";
                var PaymentInstructionName = "Payment Instruction";

                if (orderData.LanguageCode == "AAAB")
                {
                    PaymentType = "Tipo de pago";
                    //Amount = "Cantidad";
                    PaymentInstructionLabel = "Instrucciones de pago";
                    PaymentInstructionName = "Instrucciones de pago";
                }
                StringBuilder finalpaymentTypeHTML = new StringBuilder();
                StringBuilder PaymentInstructionHTML = new StringBuilder();
                foreach (var item in orderData.lstPaymentDataDetail)
                {
                    var spnAmt = dictLocalizedKeyValues.GetValue("Amount");

                    if (!orderData.BillingCodeAmount.IsNullOrEmpty() && Convert.ToDecimal(orderData.BillingCodeAmount) > AppConsts.NONE)
                    {
                        if (item.PaymentTypeCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                            spnAmt = dictLocalizedKeyValues.GetValue("PaidByInstLabel");
                        else
                            spnAmt = dictLocalizedKeyValues.GetValue("BalanceAmntLabel");
                    }

                    if (item.PaymentTypeCode == "PTCC")
                    {

                        ShowUserAgreement = "block";

                        strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.UserAgreementLabel, dictLocalizedKeyValues.GetValue("userAgreementLabel"));
                        strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.UserAgreement, dictLocalizedKeyValues.GetValue("userAgreement"));
                    }


                    finalpaymentTypeHTML.Append("<div style=\"display: inline - block; width: 30 %; vertical - align: top; float: left\"><span> " + PaymentType + " </span>:&nbsp;<span style =\"font-weight: bold;\"><span>");
                    finalpaymentTypeHTML.Append(item.PaymentType);
                    finalpaymentTypeHTML.Append("</span></span></div>");
                    finalpaymentTypeHTML.Append("<div style = \"display: inline-block; width: 30%; vertical-align: top; float: right;\"><span> " + spnAmt + ":&nbsp;<span style = \"font-weight: bold\"><span>");
                    finalpaymentTypeHTML.Append("$");
                    finalpaymentTypeHTML.Append(item.Amount);
                    finalpaymentTypeHTML.Append("</span></span></span ></div>");
                    finalpaymentTypeHTML.Append("<div style=\"clear: both\"></div>");

                    if (!item.InstructionText.IsNullOrEmpty())
                    {
                        ShowPaymentInstruction = "block";
                        PaymentInstructionHTML.Append("<div>");
                        PaymentInstructionHTML.Append("<h3 style =\"font - size:120 % !important; margin: 0 0 5px !important; padding: 0; cursor: pointer; font - weight:700; color:#8c1921;position:relative;word-spacing:2px;\"> " + PaymentInstructionName + " " + "(" + item.PaymentType + ")" + "</h3>");
                        PaymentInstructionHTML.Append("<div>");
                        PaymentInstructionHTML.Append("<div>");
                        PaymentInstructionHTML.Append("<p style =\"font - weight: bold;\">");
                        PaymentInstructionHTML.Append("<span style=\"color: #000000; font-family: 'Times New Roman'; font-size: medium; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; float: none; display: inline !important;\">" + item.InstructionText + "</span>");
                        PaymentInstructionHTML.Append("</p></div></div></div>");
                    }
                }

                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ShowUserAgreement, ShowUserAgreement);

                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PaymentInstructionContent, PaymentInstructionHTML.ToString());
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ShowPaymentInstruction, ShowPaymentInstruction);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PaymentInstructionLabel, PaymentInstructionLabel);

                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PaymentTypes, finalpaymentTypeHTML.ToString());
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ShowMailingAddress, ShowMailingAddress);
                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.EmployeeInfoSectionTitle, employeInfoSectionTitle);



                //Information For finger Printing and Service Details
                orderData.lstCustomAttribute.ForEach(cstAttribute =>
                {
                    switch (cstAttribute.AttributeName)
                    {
                        case "Place Of Birth (Country)":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PlaceOfBirth_Country, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Place Of Birth (State)":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PlaceOfBirth_State, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Citizenship":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.Citizenship, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Race":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.Race, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Eye Color":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.EyeColor, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Hair Color":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.HairColor, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Height Feet":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.HeightFeet, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Height Inches":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.HeightInches, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Weight":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.Weight, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Reason Fingerprinted":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ReasonFingerprinted, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Daycare License #":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.DaycareLicense, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "AcctNam (Literal)":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.AcctName, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "AcctAdr":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.AcctAdress, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "AcctCty":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.AcctCty, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "ACCTSTA":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.AcctState, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "AcctZip":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.AcctZip, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Reason Fingerprinted Colorado Revised Statute (C.R.S.)":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.ReasonFingerprintedColoradoRevisedStatute, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Name":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.MailingAddressName, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Address":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.MailingAddress, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "City":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.MailingAddressCity, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "State":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.MailingAddressState, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "Zip Code":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.MailingAddressZipCode, cstAttribute.AttributeDataValue);
                                break;
                            }
                        case "CBI Unique ID":
                            {
                                strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.CBIUniqueID, cstAttribute.AttributeDataValue);
                                break;
                            }
                    }
                });

                //strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.CBIUniqueID, orderData.CbiUniqueId);

                //Payment Instruction 
                //String _paymentInstructionName = "Payment Instructions (" + (paymentOption.IsNullOrEmpty() ? String.Empty : paymentOption.Name) + ")";
                //strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PaymentInstructionName, _paymentInstructionName);
                //strHtmlCode = strHtmlCode.Replace(ReceiptTemplatePlaceHolder.PaymentInstructionContent, paymentOption.IsNullOrEmpty() ? String.Empty : paymentInstruction);
            }

            return strHtmlCode;
        }

        public static Entity.ClientEntity.ApplicantDocument GetReceiptDocumentData(int orderID, int tenantID)
        {

            return ComplianceSetupManager.GetRecieptDocumentDataForOrderID(tenantID, orderID);
        }

        public static byte[] GetFileBytes(string documentPath)
        {
            return CommonFileManager.RetrieveDocument(documentPath, string.Empty);
        }

        public static bool IsOrderReceiptSaved(int tenantID, int orderID)
        {
            bool isOrderReceiptSaved = false;
            var receiptData = ComplianceSetupManager.GetRecieptDocumentDataForOrderID(tenantID, orderID);

            if (!receiptData.IsNullOrEmpty())
            {
                if (receiptData.ApplicantDocumentID > 0)
                {
                    isOrderReceiptSaved = true;
                }
            }
            return isOrderReceiptSaved;
        }

        private static string ManageSSN(string maskedSSN)
        {
            string AppSSN = maskedSSN.Trim();
            AppSSN = AppSSN.Replace(@"-", "");
            if (AppSSN == AppConsts.DefaultSSN || AppSSN == "#####1111")
            {
                maskedSSN = string.Empty;
            }
            return maskedSSN;
        }

        #endregion

        public static WebSiteWebConfig GetWebSiteWebConfig(int tenantID)
        {
            int webSiteID = WebSiteManager.GetTenantWebsiteMapping(tenantID).TWM_WebSiteID;
            WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(webSiteID);
            return webSiteWebConfig;
        }

        public static string GettenantName(int tenantId)
        {
            var tenant = SecurityManager.GetTenant(tenantId);
            return tenant.TenantName;
        }

        public static int GetOrderStatusId(int tenantID, string paymentModeCode)
        {
            var _statusCode = string.Empty;
            if (paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower() || paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
                _statusCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
            else
                _statusCode = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();

            return GetOrderStatusCode(tenantID, _statusCode.ToLower());
            // return GetOrderStatusCode(_statusCode);
        }
        public static int GetOrderStatusCode(int tenantID, string status)
        {
            return ComplianceDataManager.GetOrderStatusList(tenantID).Where(orderSts => orderSts.Code.ToLower() == status.ToLower() && !orderSts.IsDeleted)
                             .FirstOrDefault().OrderStatusID;
        }

        /// <summary>
        /// Gets the HierarchyNodeID on basis of Order Package type(the packages selected)
        /// </summary>
        /// <param name="_tenantId"></param>
        /// <param name="applicantOrderCart"></param>
        /// <returns>HierarchyID</returns>
        private static int GetHierarchyNodeIDByPackageType(int tenantId, ApplicantOrderContract applicantOrderContract)
        {
            if (applicantOrderContract.bkgPackageID > AppConsts.NONE)
            {
                int? dppID = null;
                int? bphmID = null;
                bphmID = applicantOrderContract.bkgPkgHierarchyMappingID;
                return ComplianceDataManager.GetHierarchyNodeID(dppID, bphmID, tenantId);
            }
            return AppConsts.MINUS_ONE;
        }

        #region Order History
        public static List<OrderDetailsContract> GetOrderHistoryDetailsByOrganizationUserID(int tenantID, int organizationUserID)
        {
            List<OrderDetailsContract> lstOrderDetailsContract = ComplianceDataManager.GetOrderHistory(tenantID, organizationUserID);
            var SubmittedToCBI = FingerPrintSetUpManager.GetAllAppointmentStatus().ToList().Where(t => t.AS_Code == FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue()).FirstOrDefault().AS_Name;
            foreach (var item in lstOrderDetailsContract)
            {
                if (item.AppointmentStatusCode == FingerPrintAppointmentStatus.TECHNICAL_REVIEW.GetStringValue())
                {
                    item.AppointmentStatus = SubmittedToCBI;
                    item.BkgOrderStatus = SubmittedToCBI;
                }
            }
            //ComplianceDataManager.GetOrderDetailListByOrgUserID(tenantID, organizationUserID);
            //OrderDetailsContract ordDetails = null;
            //lstOrderDetails.ForEach(x =>
            //{
            //    ordDetails = new OrderDetailsContract();
            //    ordDetails.OrderDate = x.OrderDate;
            //    ordDetails.OrderId = x.OrderId;
            //    ordDetails.OrderNumber = x.OrderNumber;
            //    ordDetails.PackageName = x.PackageName;
            //    ordDetails.PackageID = x.PackageID;
            //    ordDetails.BkgOrderStatusID = x.BkgOrderStatusID;
            //    ordDetails.BkgOrderStatusCode = x.BkgOrderStatusCode;
            //    ordDetails.BkgOrderStatus = x.BkgOrderStatus;
            //    if (x.AppointmentStatusCode != null && x.AppointmentStatusCode != string.Empty)
            //    {
            //        if (x.AppointmentStatusCode != FingerPrintAppointmentStatus.ACTIVE.GetStringValue())
            //        {
            //            ordDetails.BkgOrderStatus = x.AppointmentStatus;
            //            ordDetails.BkgOrderStatusID = x.AppointmentStatusID;
            //            ordDetails.BkgOrderStatusCode = x.AppointmentStatusCode;
            //        }
            //    }                
            //    ordDetails.InstituteHierarchy = x.InstituteHierarchy;
            //    ordDetails.DeptProgramMappingID = x.DeptProgramMappingID;
            //    ordDetails.DeptProgramPackageID = x.DeptProgramPackageID;
            //    ordDetails.OrderStatusCode = x.OrderStatusCode;
            //    ordDetails.OrderStatusName = x.OrderStatusName;
            //    ordDetails.PaymentType = x.PaymentType;
            //    ordDetails.Amount = x.Amount;
            //    ordDetails.IsFileSentToCBI = FingerPrintDataManager.IsFileSentToCbi(tenantID, x.OrderId);
            //    lstOrderDetailsContract.Add(ordDetails);
            //});
            //Parallel.ForEach(lstOrderDetailsContract, ord =>
            //{
            //    if (ord.PaymentTypeCode.Contains(PaymentOptions.Credit_Card.GetStringValue())
            //            && ord.OrderStatusCode.Contains(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
            //    {
            //        ord.OrderNumber = "";
            //    }
            //});
            return lstOrderDetailsContract.OrderByDescending(ord => ord.OrderId).ToList();
        }

        public static AppointmentSlotContract GetBkgOrderWithAppointmentData(int tenantID, int orderId, int organizationuserID)
        {
            return FingerPrintDataManager.GetBkgOrderWithAppointmentData(tenantID, orderId, organizationuserID);
        }

        public static ReserveSlotContract RescheduleAppointment(int tenantID, int organizationUserID, FingerPrintAppointmentContract selectedappointment, int orderID, bool isFingerPrintRejected, bool isLocationUpdateAllowed)
        {
            ReserveSlotContract reserveSlotContractRes = new ReserveSlotContract();
            reserveSlotContractRes = FingerPrintSetUpManager.ReserveSlot(selectedappointment.ReserverSlotID, selectedappointment.SlotID.Value, organizationUserID);

            if (!reserveSlotContractRes.IsNullOrEmpty() && reserveSlotContractRes.ReservedSlotID > AppConsts.NONE && reserveSlotContractRes.IsAvailable)
            {
                ReserveSlotContract reserveSlotContract = new ReserveSlotContract();
                reserveSlotContract.SlotID = selectedappointment.SlotID;
                reserveSlotContract.TenantID = tenantID;
                reserveSlotContract.AppOrgUserID = organizationUserID;
                reserveSlotContract.OrderID = orderID;
                reserveSlotContract.LocationID = selectedappointment.LocationId;
                reserveSlotContract.ReservedSlotID = reserveSlotContractRes.ReservedSlotID;
                reserveSlotContract.IsEventTypeCode = selectedappointment.IsEventCode;
                reserveSlotContract.IsLocationUpdate = isLocationUpdateAllowed;
                reserveSlotContract.IsRejectedReschedule = isFingerPrintRejected;

                if (!reserveSlotContract.IsNullOrEmpty())
                    return FingerPrintDataManager.SubmitApplicantAppointment(reserveSlotContract, organizationUserID);
                else
                    return new ReserveSlotContract();
            }
            return new ReserveSlotContract();
        }

        public static void SendAppointmentRescheduleNotification(int SelectedTenantId, int CurrentLoggedInUserId, FingerPrintAppointmentContract selectedappointment, int applicantAppointmentID, bool isLocationUpdateAllowed)
        {
            AppointmentOrderScheduleContract appOrdSchdContract = FingerPrintSetUpManager.GetAppointmentOrderDetailData(CurrentLoggedInUserId, false, SelectedTenantId.ToString(), applicantAppointmentID);
            if (!appOrdSchdContract.IsNullOrEmpty() && !selectedappointment.IsNullOrEmpty())
            {
                AppointmentSlotContract appointmentSlotContract = new AppointmentSlotContract();
                appointmentSlotContract.IsLocationUpdate = isLocationUpdateAllowed;
                appointmentSlotContract.LocationId = appOrdSchdContract.LocationId;
                appointmentSlotContract.SlotDate = appOrdSchdContract.AppointmentDate;
                appointmentSlotContract.SlotStartTime = appOrdSchdContract.StartTime;
                appointmentSlotContract.SlotEndTime = appOrdSchdContract.EndTime;
                appointmentSlotContract.ApplicantOrgUserId = appOrdSchdContract.ApplicantOrgUserId;
                appointmentSlotContract.IsEventType = selectedappointment.IsEventCode;
                appointmentSlotContract.EventName = selectedappointment.EventName;
                appointmentSlotContract.EventDescription = selectedappointment.EventDescription;
                var res = FingerPrintSetUpManager.SendFingerPrintAppointmentMailNotification(appOrdSchdContract, appointmentSlotContract, true);
            }
        }

        public static bool IsFingerPrintRejected(OrderDetailsContract orderDetail)
        {
            if (orderDetail.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_ERROR.GetStringValue()
                   || orderDetail.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_REJECTED.GetStringValue()
                   || orderDetail.AppointmentStatusCode == FingerPrintAppointmentStatus.CBI_FINGERPRINT_FILE_REJECTED.GetStringValue()
                   || orderDetail.AppointmentStatusCode == FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue())
            {
                return true;
            }

            return false;
        }

        public static bool LocationUpdateAllowed(OrderDetailsContract orderDetail)
        {
            if (IsFingerPrintRejected(orderDetail))
            {
                return true;
            }

            if (orderDetail.IsOnsiteAppointment || orderDetail.IsColoradoFingerPrinting)
            {
                return true;
            }

            return false;
        }

        #endregion


        public static List<Entity.lkpLanguage> GetCommLang()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpLanguage>().ToList();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Cancel order

        public static CancelOrderDataContract GetCancelOrderData(OrderDetailsContract orderData, int tenantId, int currentLoggedInUserID)
        {
            CancelOrderDataContract cancelOrderDataContract = new CancelOrderDataContract();
            cancelOrderDataContract.MaxLocScheduleAllowedDays = SecurityManager.GetLocTenMaxAllowedDays();
            cancelOrderDataContract.orderPaymentDetailList = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(tenantId, orderData.OrderId);
            cancelOrderDataContract.AppointSlotContract = FingerPrintDataManager.GetBkgOrderWithAppointmentData(tenantId, orderData.OrderId, currentLoggedInUserID);
            var creditCardOrderPaymentDetail = cancelOrderDataContract.orderPaymentDetailList.FirstOrDefault(cnd => cnd.lkpPaymentOption != null
                                                                                 && cnd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue() && cnd.OPD_Amount > AppConsts.NONE);
            if (!creditCardOrderPaymentDetail.IsNullOrEmpty())
            {
                var UserId = creditCardOrderPaymentDetail.Order.OrganizationUserProfile.OrganizationUser.UserID;
                cancelOrderDataContract.customerProfile = ComplianceDataManager.GetCustomerProfile(UserId, tenantId);
            }
            cancelOrderDataContract.Description = ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == tenantId).FirstOrDefault().TenantName;
            return cancelOrderDataContract;
        }

        public static bool CancelBkgOrder(OrderDetailsContract orderData, int tenantId, int currentLoggedInUserID, int ApplicantAppointmentId, decimal? RefundAmount)
        {
            var orderStatusCode = ApplicantOrderStatus.Cancelled.GetStringValue();
            bool isCancelledByApplicant = true;
            return FingerPrintDataManager.CancelBkgOrder(tenantId, orderData.OrderId, orderStatusCode, currentLoggedInUserID, isCancelledByApplicant, ApplicantAppointmentId, RefundAmount, currentLoggedInUserID);

        }
        public static void AddRefundHistory(Entity.ClientEntity.RefundHistory refundHistory, int tenantid, int OrgUserId, int currentLoggedInUserId)
        {
            ComplianceDataManager.AddRefundHistory(refundHistory, tenantid);
            FingerPrintDataManager.SaveUpdateApointmentRefundAudit(refundHistory, tenantid, OrgUserId, currentLoggedInUserId);

        }

        #endregion

        //UAT-4164
        public static Boolean IsUserExixtInLocationTenants(Guid UserID)
        {
            return SecurityManager.IsUserExixtInLocationTenants(UserID);
        }

        //UAT-4164
        public static Boolean IsPasswordNeedToBeChanged(Guid UserID)
        {
            return SecurityManager.IsPasswordNeedToBeChanged(UserID, 90);
        }

        public static CustomFormDataContract GetAttributesDataFromOrderId(Int32 TenantId, Int32 masterOrderId, String bopIds)
        {
            BkgOrderDetailCustomFormDataContract bkgOrderDetailCustomFormDataContract = BackgroundProcessOrderManager.GetBkgORDCustomFormAttrDataForCompletingOrder
                                                                                        (TenantId, masterOrderId, bopIds, false);
            CustomFormDataContract customData = new CustomFormDataContract();
            customData.lstCustomFormAttributes = new List<AttributesForCustomFormContract>();
            List<BkgOrderDetailCustomFormUserData> lstBkgOrderData = new List<BkgOrderDetailCustomFormUserData>();
            if (bkgOrderDetailCustomFormDataContract.IsNotNull() && bkgOrderDetailCustomFormDataContract.lstDataForCustomForm.IsNotNull())
            {
                lstBkgOrderData = bkgOrderDetailCustomFormDataContract.lstDataForCustomForm;
                customData.customFormId = lstBkgOrderData.DistinctBy(cond => cond.CustomFormID).FirstOrDefault().CustomFormID;
                foreach (BkgOrderDetailCustomFormUserData item in lstBkgOrderData)
                {
                    AttributesForCustomFormContract customAttribute = new AttributesForCustomFormContract();
                    customAttribute.AttributeGroupId = item.AttributeGroupID;
                    customAttribute.AtrributeGroupMappingId = item.AttributeGroupMappingID;
                    customAttribute.AttributeDataValue = item.Value;
                    customAttribute.AttributeName = item.AttributName;
                    customAttribute.PackageID = Convert.ToInt32(bopIds);
                    customData.lstCustomFormAttributes.Add(customAttribute);
                }
            }
            return customData;
        }
        public static Boolean IsSuffixDropDown()
        {
            AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.SUFFIX_TYPE.ToString());
            return !appConfig.IsNullOrEmpty() && appConfig.AC_Value == "1" ? true : false;

        }

        public static List<lkpSuffix> GetSuffixes()
        {
            return SecurityManager.GetSuffixes().ToList();
        }

        // UAT-4271
        public static List<LookupContract> GetCBIUniqueIdByAcctNameOrNumber(Int32 tenantID, String acctNameOrNumber)
        {
            return FingerPrintDataManager.GetCBIUniqueIdByAcctNameOrNumber(tenantID, acctNameOrNumber);
        }
    }



}
