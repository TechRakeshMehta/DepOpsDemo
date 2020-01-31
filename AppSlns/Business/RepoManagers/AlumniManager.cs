using Entity;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.Alumni;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;

namespace Business.RepoManagers
{
    public static class AlumniManager
    {
        public static Boolean UpdateApplicantForAlumniAccess(Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateApplicantForAlumniAccess(currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void ProcessApplicantDataForEmial(Int32 chunkSize, Int32 currentLoggedInUserId)
        {
            try
            {
                List<Tenant> lstAllTenants = new List<Tenant>();
                List<Tenant> tenantsWithAlumniEmailTurnedOn = new List<Tenant>();

                lstAllTenants = SecurityManager.GetTenantList();
                if (!lstAllTenants.IsNullOrEmpty())
                {
                    foreach (Tenant tenant in lstAllTenants)
                    {
                        List<String> lstCodes = new List<String>();
                        lstCodes.Add(Setting.ALUMNI_ACCESS_EMAIL.GetStringValue());
                        List<Entity.ClientEntity.ClientSetting> lstClientSetting = ComplianceDataManager.GetClientSettingsByCodes(tenant.TenantID, lstCodes);
                        var _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.ALUMNI_ACCESS_EMAIL.GetStringValue());
                        if (!_setting.IsNullOrEmpty() && Convert.ToBoolean(Convert.ToInt32(_setting.CS_SettingValue)))
                        {
                            tenantsWithAlumniEmailTurnedOn.Add(tenant);
                        }
                    }
                }

                if (!tenantsWithAlumniEmailTurnedOn.IsNullOrEmpty())
                {
                    foreach (Tenant tenant in tenantsWithAlumniEmailTurnedOn)
                    {
                        Int32 noRecordsToProcess = chunkSize;
                        while (noRecordsToProcess > AppConsts.NONE)
                        {
                            List<OrganizationUserAlumniAccess> lstOrganizationUserAlumniAccess = new List<OrganizationUserAlumniAccess>();
                            lstOrganizationUserAlumniAccess = BALUtils.GetSecurityRepoInstance().GetApplicantDataForEmail(chunkSize, currentLoggedInUserId, tenant.TenantID);

                            if (!lstOrganizationUserAlumniAccess.IsNullOrEmpty() && lstOrganizationUserAlumniAccess.Count > AppConsts.NONE)
                            {
                                SendEmailToApplicants(lstOrganizationUserAlumniAccess, currentLoggedInUserId);
                            }
                            noRecordsToProcess = lstOrganizationUserAlumniAccess.Count;
                        }
                    }
                }

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static void SendEmailToApplicants(List<OrganizationUserAlumniAccess> lstOrganizationUserAlumniAccess, Int32 currentLoggedInUserId)
        {

            foreach (OrganizationUserAlumniAccess organizationUserAlumniAccess in lstOrganizationUserAlumniAccess)
            {
                Int32 systemCommunicationID = CommunicationManager.SendAlumniAccessNotification(organizationUserAlumniAccess.OUAA_TenantID, organizationUserAlumniAccess, currentLoggedInUserId);
                String emailStatusCode = String.Empty;

                if (systemCommunicationID > AppConsts.NONE)
                {
                    emailStatusCode = lkpEmailNotificationStatus.Sent.GetStringValue();
                }
                else
                {
                    emailStatusCode = lkpEmailNotificationStatus.Failed.GetStringValue();
                }
                BALUtils.GetSecurityRepoInstance().UpdateStatusInOrganizationUserAlumniAccess(organizationUserAlumniAccess.OUAA_ID, emailStatusCode, currentLoggedInUserId);
            }
        }

        public static Boolean CreateAlumniDefaultSubscription(Int32 sourceTenantID, Int32 currentLoggedInUserID, Int32 organizationUserID, String machineIP)
        {
            try
            {
                if (organizationUserID > AppConsts.NONE)
                {
                    String AlumniTenantCode = AlumniSettings.AlumniTenantID.GetStringValue();
                    String targetTenantID = AlumniManager.GetAlumniSettingByCode(AlumniTenantCode);
                    Int32 organizationUserProfileID = AppConsts.NONE;
                    Entity.OrganizationUser _organizationUser = createOrganizationUser(organizationUserID, targetTenantID);

                    if (_organizationUser != null && _organizationUser.OrganizationUserProfiles != null)
                    {
                        organizationUserProfileID = _organizationUser.OrganizationUserProfiles.FirstOrDefault().OrganizationUserProfileID;
                    }
                    //TODO: Organization USer profile Creation
                    //TODO: Activated status & Account Linking 
                    if (organizationUserProfileID > AppConsts.NONE)
                    {
                        //update Status To Activated in OrganizationUserAlumniAccess
                        Int32 aluminAccessID = BALUtils.GetSecurityRepoInstance().GetOrganizationUserAlumniAccessIds(organizationUserID, sourceTenantID).FirstOrDefault();
                        String alumniStatus = lkpAlumniStatus.Initiated.GetStringValue();
                        BALUtils.GetSecurityRepoInstance().UpdateOrganizationUserAlumniAccess(aluminAccessID, alumniStatus, currentLoggedInUserID);

                        var result = BALUtils.GetAlumniRepoInstance(Convert.ToInt32(targetTenantID)).CreateAlumniDefaultSubscription(currentLoggedInUserID, organizationUserProfileID, machineIP);
                        if (result.Item2 > AppConsts.NONE)//Here result.Item2 is orderID
                        {

                            if (result.Item1 > AppConsts.NONE)//Here result.Item1 is alumniPackageSubscriptionID
                            {
                                String complianceDatamovementStatusCode = ComplianceDataMovementStatus.Data_Movement_Due.GetStringValue();
                                Int32 complianceDatamovementStatusID = LookupManager.GetLookUpData<lkpComplianceDataMovementStatu>().FirstOrDefault(condition => condition.CDMS_Code == complianceDatamovementStatusCode).CDMS_ID;
                                BALUtils.GetSecurityRepoInstance().ComplianceDataMovementInsertLog(sourceTenantID, Convert.ToInt32(targetTenantID), result.Item1, complianceDatamovementStatusID, currentLoggedInUserID);
                            }
                            BALUtils.GetSecurityRepoInstance().AddAlumniActivationDetails(sourceTenantID, aluminAccessID, result.Item2, result.Item1, currentLoggedInUserID);
                            String alumniActivatedStatus = lkpAlumniStatus.Activated.GetStringValue();
                            BALUtils.GetSecurityRepoInstance().UpdateOrganizationUserAlumniAccess(aluminAccessID, alumniActivatedStatus, currentLoggedInUserID);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean CheckForAlumnAccessStatus(String StatusInitiatedCode, Guid Token, Int32 orgUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CheckForAlumnAccessStatus(StatusInitiatedCode, Token, orgUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetAlumniSettingByCode(String AlumniTenantCode)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetAlumniSettingByCode(AlumniTenantCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        public static Boolean AlumniAccessStatus(Int32 orgUserId, Int32 tenantId, String statusCode)
        {
            try
            {
                if (BALUtils.GetSecurityRepoInstance().CheckForAlumnAccessStatusDue(orgUserId, tenantId, statusCode))
                {
                    return BALUtils.GetAlumniRepoInstance(tenantId).CheckAllSubscriptionsForApplicant(orgUserId);
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Alumni Helper Region
        public static Entity.OrganizationUser createOrganizationUser(Int32 organizationUserID, String targetTenantID)
        {
            Entity.OrganizationUser _organizationUser = new Entity.OrganizationUser();
            var organizationUser = SecurityManager.GetOrganizationUser(organizationUserID);
            if (!organizationUser.IsNullOrEmpty())
            {

                String usrVerificationCode = MD5Hash(Guid.NewGuid().ToString());
                _organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(Convert.ToInt32(targetTenantID));
                _organizationUser.UserID = organizationUser.UserID;
                _organizationUser.FirstName = organizationUser.FirstName;
                _organizationUser.MiddleName = organizationUser.MiddleName;
                _organizationUser.LastName = organizationUser.LastName;
                _organizationUser.PrimaryEmailAddress = organizationUser.PrimaryEmailAddress;
                _organizationUser.SecondaryEmailAddress = organizationUser.SecondaryEmailAddress;
                _organizationUser.IsNewPassword = false;
                _organizationUser.CreatedOn = DateTime.Now;
                //_organizationUser.SysXBlockID = organizationUser.SysXBlockID;
                _organizationUser.ModifiedOn = null;
                _organizationUser.IsDeleted = false;
                _organizationUser.IsActive = true;
                _organizationUser.IsApplicant = true;
                _organizationUser.IsOutOfOffice = false;
                _organizationUser.IgnoreIPRestriction = true;
                _organizationUser.IsMessagingUser = true;
                _organizationUser.IsSystem = false;
                _organizationUser.IsMessagingUser = false;
                _organizationUser.PhotoName = organizationUser.PhotoName;
                _organizationUser.OriginalPhotoName = organizationUser.OriginalPhotoName;
                _organizationUser.DOB = organizationUser.DOB;
                String SSN = ComplianceSetupManager.GetFormattedString(organizationUser.OrganizationUserID, false, organizationUser.Organization.TenantID.Value);
                _organizationUser.SSN = SSN;
                _organizationUser.Gender = organizationUser.Gender;
                _organizationUser.PhoneNumber = organizationUser.PhoneNumber;
                _organizationUser.SecondaryPhone = organizationUser.SecondaryPhone;
                _organizationUser.UserVerificationCode = usrVerificationCode;
                _organizationUser.AddressHandle = organizationUser.AddressHandle;
                _organizationUser.IsInternationalPhoneNumber = organizationUser.IsInternationalPhoneNumber;
                _organizationUser.IsInternationalSecondaryPhone = organizationUser.IsInternationalSecondaryPhone;

                //UAT-887: WB: Delay Automatic emails going out after activation
                if (_organizationUser.ActiveDate == null && _organizationUser.IsApplicant == true && _organizationUser.IsActive == true)
                    _organizationUser.ActiveDate = DateTime.Now;

                //Adds and updates the Person Alias.
                //organizationUser.PersonAlias.ForEach(s=>_organizationUser.PersonAlias.Add(s));
                List<PersonAliasContract> personAliasList = new List<PersonAliasContract>();
                personAliasList = _organizationUser.PersonAlias.Select(s => new PersonAliasContract()
                {
                    FirstName = s.PA_FirstName,
                    ID = s.PA_ID,
                    LastName = s.PA_LastName,
                    MiddleName = s.PA_LastName
                }).ToList();
                AddUpdatePersonAlias(_organizationUser, personAliasList);

                SecurityManager.AddOrganizationUser(_organizationUser);

                //Add Current Resident History
                AddCurrentResidentialHistory(_organizationUser);
                _organizationUser.SSN = SSN;
                SecurityManager.AddOrganizationUserProfile(_organizationUser);
                // Sets default subscription for user
                SetDefaultSubscription(_organizationUser.OrganizationUserID);

                //Get Website Url
                Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(_organizationUser.Organization.Tenant.TenantID);
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
                List<Int32> productId = SecurityManager.GetProductsForTenant(_organizationUser.Organization.TenantID.Value).Select(obj => obj.TenantProductID).ToList();
                List<String> defaultRoledetailIds = SecurityManager.getDefaultRoleDetailIds(productId);
                SecurityManager.SaveMappingOfRolesWithSelectedUser(_organizationUser, defaultRoledetailIds, emptyArray, String.Empty, false); // View.Password);
                SecurityManager.SetDefaultBusinessChannel(_organizationUser.OrganizationUserID);

                //UAT-1218 - No need to synchronize profile and docs while APPLICANT linked with (ADB ADMINS) or (CLIENT ADMIN) or (SHARED USER)
                if ((organizationUser.IsApplicant ?? false) == true)
                {
                    SecurityManager.SynchoniseUserProfile(organizationUser.OrganizationUserID, organizationUser.Organization.TenantID.Value, organizationUser.OrganizationUserID);
                    SecurityManager.SynchoniseUserDocuments(organizationUser.OrganizationUserID, organizationUser.Organization.TenantID.Value, organizationUser.OrganizationUserID);
                }
                SecurityManager.SendEmailForInstitutionChange(organizationUser, applicationUrl); //NEED TO CONFIRM
                // if (!SecurityManager.SendEmailForInstitutionChange(organizationUser, applicationUrl))
                // {
                //    View.ErrorMessage = "Some error has occured.Please contact administrator.";
                //}
            }
            return _organizationUser;

        }

        public static void AddUpdatePersonAlias(OrganizationUser organizationUser, List<PersonAliasContract> personAliasList)
        {
            if (personAliasList.IsNotNull())
            {
                List<PersonAlia> currentAliasList = organizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
                foreach (PersonAliasContract tempPersonAlias in personAliasList)
                {
                    if (tempPersonAlias.ID > 0)
                    {
                        PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_ID == tempPersonAlias.ID);
                        if (personAlias.IsNotNull())
                        {
                            personAlias.PA_FirstName = tempPersonAlias.FirstName;
                            personAlias.PA_LastName = tempPersonAlias.LastName;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            personAlias.PA_MiddleName = tempPersonAlias.MiddleName;
                            personAlias.PA_ModifiedBy = AppConsts.NONE;
                            personAlias.PA_ModifiedOn = DateTime.Now;
                        }
                    }
                    else
                    {
                        PersonAlia personAlias = new PersonAlia();
                        personAlias.PA_FirstName = tempPersonAlias.FirstName;
                        personAlias.PA_LastName = tempPersonAlias.LastName;
                        personAlias.PA_MiddleName = tempPersonAlias.MiddleName;
                        personAlias.PA_IsDeleted = false;
                        personAlias.PA_CreatedBy = AppConsts.NONE;
                        personAlias.PA_CreatedOn = DateTime.Now;
                        personAlias.PA_AliasIdentifier = Guid.NewGuid();
                        organizationUser.PersonAlias.Add(personAlias);
                    }
                }
                List<Int32> aliasIDToBeDeleted = currentAliasList.Select(x => x.PA_ID).Except(personAliasList.Select(y => y.ID)).ToList();
                foreach (Int32 delAliasID in aliasIDToBeDeleted)
                {
                    PersonAlia delAlias = currentAliasList.FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == delAliasID);
                    delAlias.PA_IsDeleted = true;
                    delAlias.PA_ModifiedBy = AppConsts.NONE;
                    delAlias.PA_ModifiedOn = DateTime.Now;
                }
            }
        }
        public static string MD5Hash(string text)
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

        public static void AddCurrentResidentialHistory(Entity.OrganizationUser organizationUser)
        {
            //Current residential Address
            // ResidentialHistory currentResedentialHistory = organizationUser.ResidentialHistories.FirstOrDefault(x => x.RHI_IsCurrentAddress == true && x.RHI_IsDeleted == false);

            Entity.ResidentialHistory currentResedentialHistory = new Entity.ResidentialHistory();
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

        public static void SetDefaultSubscription(Int32 organizationUserId)
        {
            Int32 NotificationCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.NOTIFICATION.GetStringValue()).CommunicationTypeID;
            Int32 AlertCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.ALERTS.GetStringValue()).CommunicationTypeID;
            Int32 ReminderCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.REMINDERS.GetStringValue()).CommunicationTypeID;

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
        public static List<UserCommunicationSubscriptionSetting> GetMappedUserCommunicationSubscriptionSettings(
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

        public static Boolean UpdateAlumniStatusinOrganizationUserAlumnAccess(Int32 OrgUserId, Int32 TenantId, String alumniStatusCode, Int32 LoggedInUserID)
        {
            Int32 aluminAccessID = BALUtils.GetSecurityRepoInstance().GetOrganizationUserAlumniAccessIds(OrgUserId, TenantId).FirstOrDefault();
            if (aluminAccessID > AppConsts.NONE)
            {
                BALUtils.GetSecurityRepoInstance().UpdateOrganizationUserAlumniAccess(aluminAccessID, alumniStatusCode, LoggedInUserID);
                return true;
            }
            return false;
        }

        public static List<AlumniPackageSubscription> CopyComplianceToCompliance(Int32 backgroundProcessUserId, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().CopyComplianceDataToCompliance(backgroundProcessUserId, chunkSize);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SendEmailForDataMovementComplete(List<AlumniPackageSubscription> lstTarSubscriptionsForEmail)
        {
            try
            {
                foreach (AlumniPackageSubscription alumniPackageSubscription in lstTarSubscriptionsForEmail)
                {
                    Int32 systemCommunicationID = CommunicationManager.SendEmailForAvalableAlumniPacakge(alumniPackageSubscription);
                }
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Check if the user belongs to Multi Tenants
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Boolean IsMultiTenantUserExceptAlumni(Guid userId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsMultiTenantUserExceptAlumni(userId);
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

        public static void HandleAssignments(List<AlumniPackageSubscription> lstTarPackageSubscriptions, Int32 tenantId, Int32 CurrentLoggedInUserID)
        {
            try
            {
                //StringBuilder sbXML = new StringBuilder();
                List<Int32> lstPkgSubscriptionIds = lstTarPackageSubscriptions.Select(sel => sel.TarPackageSubscriptionID).ToList();
                List<Entity.ClientEntity.QueueMetaData> _lstQueueMetaData = LookupManager.GetLookUpData<Entity.ClientEntity.QueueMetaData>(tenantId);
                String queueTypeCode = QueueMetaDataType.Verification_Queue_For_Admin.GetStringValue();
                List<Entity.ClientEntity.lkpItemComplianceStatu> _lstItemComplianceStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpItemComplianceStatu>(tenantId);

                foreach (AlumniPackageSubscription pkgSub in lstTarPackageSubscriptions)
                {
                    //Dictionary<String, Object> dicHandleAssignmentData = new Dictionary<String, Object>();
                    Int32 currentUserId = pkgSub.ApplicantOrgUserID;
                    Int32 packageSubscriptionId = pkgSub.TarPackageSubscriptionID;
                    Int32 packageId = pkgSub.TarPackageID;
                    //sbXML.Append("<Queues>");

                    Entity.Tenant _tenant = SecurityManager.GetOrganizationUser(currentUserId).Organization.Tenant;
                    Boolean isDefaultTenant = _tenant.TenantID.Equals(SecurityManager.DefaultTenantID);

                    Entity.ClientEntity.PackageSubscription currentSubscription = ComplianceDataManager.GetPackageSubscriptionByID(tenantId, packageSubscriptionId);
                    List<Entity.ClientEntity.ApplicantComplianceCategoryData> categoryDataListFromDb = currentSubscription.ApplicantComplianceCategoryDatas.Where(cond => !cond.IsDeleted).ToList();

                    foreach (Entity.ClientEntity.ApplicantComplianceCategoryData categoryData in categoryDataListFromDb)
                    {
                        if (!categoryData.ApplicantComplianceItemDatas.IsNullOrEmpty())
                        {
                            foreach (var itemData in categoryData.ApplicantComplianceItemDatas.Where(cond => !cond.IsDeleted).ToList())
                            {
                                var complianceStatus = new Entity.ClientEntity.lkpItemComplianceStatu();

                                if (itemData.IsNotNull())
                                    complianceStatus = _lstItemComplianceStatus.Where(x => x.IsDeleted == false && x.ItemComplianceStatusID == itemData.StatusID).FirstOrDefault();

                                String itemComplianceStatusText = String.Empty;
                                if (!complianceStatus.IsNullOrEmpty())
                                {
                                    itemComplianceStatusText = complianceStatus.Name;
                                }
                                if (itemData.IsNotNull() && itemData.ApplicantComplianceItemID > AppConsts.NONE)
                                {
                                    if (complianceStatus.Code == ApplicantItemComplianceStatus.Pending_Review.GetStringValue() && itemData.IsNotNull())
                                    {
                                        StringBuilder sbXML = new StringBuilder();
                                        Dictionary<String, Object> dicHandleAssignmentData = new Dictionary<String, Object>();

                                        Entity.ClientEntity.QueueMetaData _queueMetaData = _lstQueueMetaData.Where(qmd => qmd.QMD_Code == queueTypeCode && qmd.QMD_IsDeleted == false).FirstOrDefault();
                                        Int32 queueId = _queueMetaData.QMD_QueueID;

                                        //Added to get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
                                        Dictionary<String, Object> dicQueueFields = new Dictionary<String, Object>();
                                        String ApplicantName = String.Concat(currentSubscription.OrganizationUser.FirstName, " ", currentSubscription.OrganizationUser.LastName);
                                        String CategoryName = !String.IsNullOrEmpty(categoryData.ComplianceCategory.CategoryLabel) ? categoryData.ComplianceCategory.CategoryLabel : categoryData.ComplianceCategory.CategoryName;
                                        String ItemName = !String.IsNullOrEmpty(itemData.ComplianceItem.ItemLabel) ? itemData.ComplianceItem.ItemLabel : itemData.ComplianceItem.Name;
                                        String PkgName = !String.IsNullOrEmpty(currentSubscription.CompliancePackage.PackageLabel) ? currentSubscription.CompliancePackage.PackageLabel : currentSubscription.CompliancePackage.PackageName;
                                        //  String RushOrderStatusCode = currentSubscription.Orders.FirstOrDefault().lkpOrderStatu.Code;
                                        //dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantName, itemDataInDb.ApplicantName);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantName, ApplicantName);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ComplianceItemId, itemData.ComplianceItemID);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.CategoryId, categoryData.ComplianceCategoryID);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.PackageID, packageId);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.SubmissionDate, itemData.SubmissionDate);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.System_Status, itemData.SystemStatusText);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Rush_Order_Status_Code, itemData.RushOrderStatusCode);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantComplianceItemID, itemData.ApplicantComplianceItemID);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.HierarchyNodeID, currentSubscription.Order.HierarchyNodeID.ToString());
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.ApplicantId, currentSubscription.OrganizationUserID.ToString());// itemDataInDb.ApplicantId);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Item_Name, ItemName);//    itemDataInDb.ComplianceItemName);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Category_Name, CategoryName);//.ComplianceCategoryName);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Package_Name, PkgName);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Verification_Status_Text, itemComplianceStatusText);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Rush_Order_Status_Text, itemData.RushOrderStatusText);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Verification_Status, itemData.StatusID);
                                        dicQueueFields.Add(QueuefieldsMetaDataTypeConstants.Assigned_To_User, itemData.AssignedToUser.IsNull() ? 0 : itemData.AssignedToUser);

                                        sbXML.Append("<Queues>");
                                        foreach (KeyValuePair<String, Object> dicQueueField in dicQueueFields)
                                        {
                                            sbXML.Append("<QueueDetail>");
                                            sbXML.Append("<QueueID>" + queueId + "</QueueID>");
                                            sbXML.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");
                                            sbXML.Append("<QueueFieldName>" + dicQueueField.Key + "</QueueFieldName>");
                                            sbXML.Append("<QueueFieldValue>" + dicQueueField.Value + "</QueueFieldValue>");
                                            sbXML.Append("<NotReviewed>" + true + "</NotReviewed>");
                                            sbXML.Append("</QueueDetail>");
                                        }

                                        sbXML.Append("<QueueDetail>");
                                        sbXML.Append("<QueueID>" + queueId + "</QueueID>");
                                        sbXML.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");
                                        sbXML.Append("<QueueFieldName>" + "ResetReviewProcess" + "</QueueFieldName>");
                                        sbXML.Append("<QueueFieldValue>" + false + "</QueueFieldValue>");
                                        sbXML.Append("<NotReviewed>" + true + "</NotReviewed>");
                                        sbXML.Append("</QueueDetail>");

                                        sbXML.Append("<QueueDetail>");
                                        sbXML.Append("<QueueID>" + queueId + "</QueueID>");
                                        sbXML.Append("<RecordID>" + itemData.ApplicantComplianceItemID + "</RecordID>");
                                        sbXML.Append("<QueueFieldName>" + "ByPassInitialReview" + "</QueueFieldName>");
                                        sbXML.Append("<NotReviewed>" + true + "</NotReviewed>");
                                        sbXML.Append("<QueueFieldValue>" + false + "</QueueFieldValue>");
                                        sbXML.Append("</QueueDetail>");

                                        sbXML.Append("</Queues>");
                                        dicHandleAssignmentData.Add("QueueRecordXML", Convert.ToString(sbXML));
                                        dicHandleAssignmentData.Add("CurrentLoggedInUserId", Convert.ToString(CurrentLoggedInUserID));
                                        dicHandleAssignmentData.Add("TenantId", Convert.ToString(tenantId));
                                        BALUtils.GetQueueManagementRepoInstance(tenantId).HandleAssignment(dicHandleAssignmentData);
                                    }
                                }
                            }
                        }
                    }
                }
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
    }
}
