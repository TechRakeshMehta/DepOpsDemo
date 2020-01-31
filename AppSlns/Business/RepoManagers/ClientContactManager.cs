using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.Templates;
using System.Configuration;

namespace Business.RepoManagers
{
    public class ClientContactManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static ClientContactManager()
        {
            BALUtils.ClassModule = "Client Contact Manager";
        }

        #endregion

        #region PUBLIC METHODS

        #region ManageClientContact Screen (Instructor/Perceptor)

        /// <summary>
        /// Returns list of Client Contacts (Instructor/Preceptors) based on Tenant ID and account activated filter.
        /// UAT-4153 Add "Account Activated" (can be Yes/No) info in Support Portal and Manage Preceptor results grids.
        /// </summary>
        /// <param name="TenantID">Tenant ID of logged in Client Admin or Selected Tenant for Admin.</param>
        /// <returns>List of ClientContacts</returns>
        public static List<ClientContactContract> GetClientContactSearchData(Int32 tenantID, Int32 IsActive)
        {
            try
            {
                return ConvertClientContactEntityToContact(BALUtils.GetClientContactRepoInstance().GetClientContactSearchData(tenantID, IsActive));
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
        /// Returns list of Client Contacts (Instructor/Preceptors) based on Tenant ID.
        /// </summary>
        /// <param name="TenantID">Tenant ID of logged in Client Admin or Selected Tenant for Admin.</param>
        /// <returns>List of ClientContacts</returns>
        public static List<ClientContactContract> GetClientContacts(Int32 tenantID)
        {
            try
            {
                return ConvertClientContactEntityToContact(BALUtils.GetClientContactRepoInstance().GetClientContacts(tenantID));
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
        /// Returns list of ClientContactAvailibiltyContract based on ClientContact ID.
        /// </summary>
        /// <param name="clientContactID"></param>
        /// <returns></returns>
        public static List<ClientContactAvailibiltyContract> GetClientContactAvailibilty(Int32 clientContactID)
        {
            try
            {
                return ConvertClientContactAvailibiltyEntityToContact(BALUtils.GetClientContactRepoInstance().GetClientContactAvailibilty(clientContactID));
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
        /// Create new Client Contact
        /// </summary>
        /// <param name="clientContactContarct"></param>
        /// <param name="clientContactavailibiltyContractList"></param>
        /// <param name="uploadedDocuments"></param>
        /// <param name="loggedInUserID"></param>
        /// <returns></returns>
        public static Boolean SaveClientContact(ClientContactContract clientContactContarct, List<ClientContactAvailibiltyContract> clientContactavailibiltyContractList, List<SharedSystemDocumentContract> uploadedDocuments, Int32 loggedInUserID, AppSettingContract appSettingContract)
        {
            try
            {
                //1.Create a record in ClientContact (SharedData DB).
                ClientContact clientContact = GetClientContactEntityObject(clientContactContarct, loggedInUserID);

                foreach (ClientContactAvailibiltyContract clientContactavailibiltyContract in clientContactavailibiltyContractList)
                {
                    //2.Create a record in ClientContactAvailabilty (SharedData DB).
                    ClientContactAvailibilty clientContactAvailibilty = GetClientContactAvailibiltyEntityObject(clientContactavailibiltyContract, loggedInUserID);
                    clientContact.ClientContactAvailibilties.Add(clientContactAvailibilty);
                }

                #region UAT- Client Contact Profile Synchronization
                //Getting ClientContactProfileSynchronization object
                ClientContactProfileSynchronization clientContactProfileSynchronization = GetClientContactProfileSynchronizationObject(clientContact, loggedInUserID);
                clientContact.ClientContactProfileSynchronizations.Add(clientContactProfileSynchronization);
                #endregion

                AddUploadedClientDocument(loggedInUserID, clientContact, uploadedDocuments);
                if (BALUtils.GetClientContactRepoInstance().SaveClientContact(clientContact))
                {
                    //Send mail to do
                    List<String> subEventCodes = new List<String>();
                    subEventCodes.Add(AppConsts.CLIENT_CONTACT_INVITATION_SUBEVNT_CODE.ToLower());
                    Int32 subEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
                    List<CommunicationTemplatePlaceHolder> placeHoldersToFetch = CommunicationManager.GetTemplatePlaceHolders(subEventID);
                    Int32 templateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(subEventID);
                    string tenantName = SecurityManager.GetTenant(clientContact.CC_TenantID).TenantName;

                    //Contains info for mail subject and content
                    SystemEventTemplatesContract systemEventTemplate = TemplatesManager.GetTemplateDetails(templateId);
                    var queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {AppConsts.QUERY_STRING_CLIENTCONTACT_INVITE_TOKEN, Convert.ToString(clientContact.CC_TokenID)},
                                                                    {AppConsts.QUERY_STRING_USER_TYPE_CODE, appSettingContract.OrganizationUserType}
                                                                 };

                    var url = "http://" + String.Format(appSettingContract.ClientContactInvitationURL + "?args={0}", queryString.ToEncryptedQueryString());


                    //String applicationUrl = WebSiteManager.GetInstitutionUrl(clientContact.CC_TenantID);
                    Dictionary<String, String> dictMailData = new Dictionary<string, String>();
                    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, clientContact.CC_Name);
                    dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, url);
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);

                    //Send mail if user is not added previously to any tenant.
                    if (!CheckIsContactAlreadyRecievdEmail(subEventID, clientContact.CC_Email) || CheckIfClientContactsAlreadyDeleted(clientContact.CC_Email.Trim(), clientContact.CC_TenantID))
                    {
                        //a. Create entry in [Messaging] SystemCommunication table 
                        //b. Create entry in [Messaging] SystemCommunicationDelivery table 
                        SystemCommunication systemCommunication = new SystemCommunication();
                        systemCommunication.SenderName = AppConsts.CLIENT_CONTACT_SYSTEM_NAME;
                        systemCommunication.SenderEmailID = appSettingContract.SenderEmailID;
                        systemCommunication.Subject = systemEventTemplate.Subject;
                        systemCommunication.CommunicationSubEventID = subEventID;
                        systemCommunication.CreatedByID = loggedInUserID;
                        systemCommunication.CreatedOn = DateTime.Now;
                        systemCommunication.Content = systemEventTemplate.TemplateContent;
                        //replace the placeholder
                        foreach (var placeHolder in placeHoldersToFetch)
                        {
                            Object obj = dictMailData.GetValue(placeHolder.Property);
                            systemCommunication.Content = systemCommunication.Content.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                        }

                        SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery();
                        systemCommunicationDelivery.SystemCommunicationTypeID = systemCommunication.SystemCommunicationID;
                        systemCommunicationDelivery.ReceiverOrganizationUserID = clientContact.CC_ID;
                        systemCommunicationDelivery.RecieverEmailID = clientContact.CC_Email;
                        systemCommunicationDelivery.RecieverName = clientContact.CC_Name;
                        systemCommunicationDelivery.IsDispatched = false;
                        systemCommunicationDelivery.IsCC = null;
                        systemCommunicationDelivery.IsBCC = null;
                        systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
                        systemCommunicationDelivery.CreatedOn = DateTime.Now;
                        systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);

                        List<SystemCommunication> lstSystemCommunicationToBeSaved = new List<SystemCommunication>();
                        lstSystemCommunicationToBeSaved.Add(systemCommunication);
                        return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
                    }
                    return true;
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

        private static ClientContactProfileSynchronization GetClientContactProfileSynchronizationObject(ClientContact clientContact, Int32 loggedInUserID)
        {
            //Int32? orgUserID = AppConsts.NONE;
            //if (clientContact.CC_UserID.IsNotNull())
            //{
            //    orgUserID = BALUtils.GetSecurityRepoInstance().GetOrganizationUserInfoByUserId(Convert.ToString(clientContact.CC_UserID)).Where(cond => cond.IsSharedUser == true).FirstOrDefault().OrganizationUserID;
            //}
            ClientContactProfileSynchronization ccps = new ClientContactProfileSynchronization();
            ccps.CCPS_IsProfileSynched = false;
            ccps.CCPS_OrgUserID = null;
            ccps.CCPS_CreatedByID = loggedInUserID;
            ccps.CCPS_CreatedOn = DateTime.Now;
            ccps.CCPS_IsDeleted = false;
            return ccps;
        }

        /// <summary>
        /// Update Client Contact
        /// </summary>
        /// <param name="clientContactContarct"></param>
        /// <param name="clientContactavailibiltyContract"></param>
        /// <param name="uploadedDocuments"></param>
        /// <param name="loggedInUserID"></param>
        /// <returns></returns>
        public static Boolean UpdateClientContact(ClientContactContract clientContactContarct, List<ClientContactAvailibiltyContract> clientContactavailibiltyContract, List<SharedSystemDocumentContract> uploadedDocuments, Int32 loggedInUserID)
        {
            try
            {
                ClientContact clientContact = BALUtils.GetClientContactRepoInstance().UpdateClientContact(clientContactContarct.ClientContactID);
                clientContact.CC_Name = clientContactContarct.Name;
                clientContact.CC_Email = clientContactContarct.Email;
                clientContact.CC_Phone = clientContactContarct.Phone;
                //UAT-2447
                clientContact.CC_IsInternationalPhone = clientContactContarct.IsInternationalPhone;

                clientContact.CC_TenantID = clientContactContarct.TenantID;
                clientContact.CC_UserID = clientContactContarct.UserID;
                clientContact.CC_ClientContactTypeID = clientContactContarct.ClientContactTypeID;
                clientContact.CC_IsDeleted = false;
                clientContact.CC_ModifiedByID = loggedInUserID;
                clientContact.CC_ModifiedOn = DateTime.Now;


                List<ClientContactAvailibiltyContract> previousContactAvailibilty = ConvertClientContactAvailibiltyEntityToContact(clientContact.ClientContactAvailibilties.ToList());

                List<Int32> listPreviousWeekDayIDS = previousContactAvailibilty.Select(x => x.WeekDayID).ToList();
                List<Int32> listNewWeekDayIDS = clientContactavailibiltyContract.Select(x => x.WeekDayID).ToList();

                List<Int32> listPreviousDocumentIDs = clientContact.ClientContactDocuments.Select(x => x.CCD_SharedSystemDocumentID.Value).ToList();
                List<Int32> listNewDocumentIDs = uploadedDocuments.Select(x => x.DocumentID).ToList();

                List<Int32> weekDays_Added = listNewWeekDayIDS.Except(listPreviousWeekDayIDS).ToList();
                List<Int32> weekDays_Removed = listPreviousWeekDayIDS.Except(listNewWeekDayIDS).ToList();

                List<Int32> documentIds_Removed = listPreviousDocumentIDs.Except(listNewDocumentIDs).ToList();
                List<SharedSystemDocumentContract> newUploadedDocuments = uploadedDocuments.Where(x => x.DocumentID == AppConsts.NONE).ToList();

                UpdateDeletePreviousContactAvailibilty(clientContactavailibiltyContract, loggedInUserID, clientContact, weekDays_Removed);
                AddNewClientContactAvailibilty(clientContactavailibiltyContract, loggedInUserID, clientContact, weekDays_Added);

                RemoveDeletedClientDocuments(loggedInUserID, clientContact, documentIds_Removed);
                AddUploadedClientDocument(loggedInUserID, clientContact, newUploadedDocuments);

                return BALUtils.GetClientContactRepoInstance().UpdateClientContact(clientContact);
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
        /// Delete Client Contact
        /// </summary>
        /// <param name="clientContactID"></param>
        /// <param name="tenantID"></param>
        /// <param name="LoggedInUserID"></param>
        /// <returns></returns>
        public static Boolean DeleteClientContact(Int32 clientContactID, Int32 tenantID, Int32 LoggedInUserID)
        {
            try
            {
                return BALUtils.GetClientContactRepoInstance().DeleteClientContact(clientContactID, tenantID, LoggedInUserID);
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
        /// Returns the list of uploaded document by Client Contact.
        /// </summary>
        /// <param name="clientContactID"></param>
        /// <returns>List of SharedSystemDocumentContract</returns>
        public static List<SharedSystemDocumentContract> GetClientDocuments(Int32 clientContactID)
        {
            try
            {
                return ConvertSharedSystemDocumentEntityToContact(BALUtils.GetClientContactRepoInstance().GetClientDocuments(clientContactID));
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
        /// Gets the Lookup ClientContact Types i.e. lkpClientContactType
        /// </summary>  
        /// <returns></returns>
        public static List<ClientContactTypeContract> GetClientContactType()
        {
            try
            {
                return ConvertLkpClientContactTypeToContract(LookupManager.GetSharedDBLookUpData<lkpClientContactType>().Where(x => !x.CCT_IsDeleted).ToList());
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
        /// Gets the Lookup lkpWeekDay Types
        /// </summary>  
        /// <returns></returns>
        public static List<WeekDayContract> GetWeekDaysList()
        {
            try
            {
                return ConvertLkpWeekDayToContract(LookupManager.GetSharedDBLookUpData<lkpWeekDay>().Where(x => !x.WD_IsDeleted).ToList());
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
        /// Gets the Lookup lkpSharedSystemDocType Types i.e. lkpSharedSystemDocType
        /// </summary>  
        /// <returns></returns>
        public static List<SharedSystemDocTypeContract> GetDocumentTypeList()
        {
            try
            {
                return ConvertlkpShrdSysDocToContract(LookupManager.GetSharedDBLookUpData<lkpSharedSystemDocType>().Where(x => !x.SSDT_IsDeleted).ToList());
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

        public static Guid? GetOrganizationUsersByEmail(String emailID)
        {
            try
            {
                OrganizationUser orguser = SecurityManager.GetOrganizationUsersByEmail(emailID).FirstOrDefault();
                if (!orguser.IsNullOrEmpty())
                {
                    return orguser.UserID;
                }
                return null;
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

        public static Boolean IsEmailAlreadyExistForTenant(String EmailID, Int32 TenantID)
        {
            try
            {
                return BALUtils.GetClientContactRepoInstance().IsEmailAlreadyExistForTenant(EmailID, TenantID);
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

        public static Boolean IsClientContactAllowedToDelete(Int32 clientContactID, Int32 tenantID)
        {
            try
            {
                if (BALUtils.GetClinicalRotationRepoInstance(tenantID).IsClientRotationClientContactMappingExist(clientContactID))
                    return false;
                else
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

        #region UAT-4043
        public static Boolean ResendInstructorActivationMail(ClientContactContract clientContact, Int32 loggedInUserID)
        {

            #region Set App Setting Contract
            AppSettingContract appSettingContract = new AppSettingContract();
            appSettingContract.ClientContactInvitationURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                                                            ? String.Empty
                                                                            : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

            appSettingContract.SenderEmailID = System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];

            List<ClientContactTypeContract> ClientContactTypeList = GetClientContactType();

            String selectedUserTypeCode = ClientContactTypeList.Where(x => x.ClientContactTypeID == Convert.ToInt32(clientContact.ClientContactTypeID)).Select(sel => sel.Code).FirstOrDefault();
            if (selectedUserTypeCode == ClientContactType.Instructor.GetStringValue())
            {
                appSettingContract.OrganizationUserType = OrganizationUserType.Instructor.GetStringValue();
            }
            else if (selectedUserTypeCode == ClientContactType.Preceptor.GetStringValue())
            {
                appSettingContract.OrganizationUserType = OrganizationUserType.Preceptor.GetStringValue();
            }

            #endregion



            //Send mail to do
            List<String> subEventCodes = new List<String>();
            subEventCodes.Add(AppConsts.CLIENT_CONTACT_INVITATION_SUBEVNT_CODE.ToLower());
            Int32 subEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
            List<CommunicationTemplatePlaceHolder> placeHoldersToFetch = CommunicationManager.GetTemplatePlaceHolders(subEventID);
            Int32 templateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(subEventID);
            string tenantName = SecurityManager.GetTenant(clientContact.TenantID).TenantName;

            //Contains info for mail subject and content
            SystemEventTemplatesContract systemEventTemplate = TemplatesManager.GetTemplateDetails(templateId);
            var queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {AppConsts.QUERY_STRING_CLIENTCONTACT_INVITE_TOKEN, Convert.ToString(clientContact.TokenID)},
                                                                    {AppConsts.QUERY_STRING_USER_TYPE_CODE, appSettingContract.OrganizationUserType}
                                                                 };

            var url = "http://" + String.Format(appSettingContract.ClientContactInvitationURL + "?args={0}", queryString.ToEncryptedQueryString());


            //String applicationUrl = WebSiteManager.GetInstitutionUrl(clientContact.CC_TenantID);
            Dictionary<String, String> dictMailData = new Dictionary<string, String>();
            dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, clientContact.Name);
            dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, url);
            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);

            //a. Create entry in [Messaging] SystemCommunication table 
            //b. Create entry in [Messaging] SystemCommunicationDelivery table 
            SystemCommunication systemCommunication = new SystemCommunication();
            systemCommunication.SenderName = AppConsts.CLIENT_CONTACT_SYSTEM_NAME;
            systemCommunication.SenderEmailID = appSettingContract.SenderEmailID;
            systemCommunication.Subject = systemEventTemplate.Subject;
            systemCommunication.CommunicationSubEventID = subEventID;
            systemCommunication.CreatedByID = loggedInUserID;
            systemCommunication.CreatedOn = DateTime.Now;
            systemCommunication.Content = systemEventTemplate.TemplateContent;
            //replace the placeholder
            foreach (var placeHolder in placeHoldersToFetch)
            {
                Object obj = dictMailData.GetValue(placeHolder.Property);
                systemCommunication.Content = systemCommunication.Content.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
            }

            SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery();
            systemCommunicationDelivery.SystemCommunicationTypeID = systemCommunication.SystemCommunicationID;
            systemCommunicationDelivery.ReceiverOrganizationUserID = clientContact.ClientContactID;
            systemCommunicationDelivery.RecieverEmailID = clientContact.Email;
            systemCommunicationDelivery.RecieverName = clientContact.Name;
            systemCommunicationDelivery.IsDispatched = false;
            systemCommunicationDelivery.IsCC = null;
            systemCommunicationDelivery.IsBCC = null;
            systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
            systemCommunicationDelivery.CreatedOn = DateTime.Now;
            systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);

            List<SystemCommunication> lstSystemCommunicationToBeSaved = new List<SystemCommunication>();
            lstSystemCommunicationToBeSaved.Add(systemCommunication);
            return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);

        }

        #endregion

        #endregion

        #region Client Contact Profile

        public static ClientContactContract GetClientContactByEmail(Int32 tenantID, String email)
        {
            try
            {
                return ConvertClientContactEntityToContact(BALUtils.GetClientContactRepoInstance().GetClientContactByEmail(tenantID, email)).FirstOrDefault();
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

        public static List<Int32> GetClientContactTenantsIDByEmail(string email)
        {
            try
            {
                return BALUtils.GetClientContactRepoInstance().GetClientContactTenantsIDByEmail(email);
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

        public static Boolean SaveUploadedDocument(SharedSystemDocumentContract sharedSystemDocumentContract, Int32 tenantID, Int32 contactID, Int32 loggedInUserID)
        {
            try
            {

                SharedSystemDocument sharedSystemDocument = GetSharedSystemDocumentObj(loggedInUserID, sharedSystemDocumentContract);
                ClientContactDocument clientContactDocument = GetClientContactDocumentObj(loggedInUserID);
                clientContactDocument.CCD_ClientContactID = contactID;
                sharedSystemDocument.ClientContactDocuments.Add(clientContactDocument);

                return BALUtils.GetClientContactRepoInstance().SaveUploadedDocument(sharedSystemDocument);
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

        public static Boolean DeleteUploadedDocument(Int32 SharedsystemDocumentID, Int32 ClientContactID, Int32 LoggedInUserID)
        {
            try
            {
                return BALUtils.GetClientContactRepoInstance().DeleteUploadedDocument(SharedsystemDocumentID, ClientContactID, LoggedInUserID);
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

        public static OrganizationUserContract GetUserData(int organisationUserID)
        {
            try
            {
                return ConvertEntityToOrganizationUserContract(SecurityManager.GetOrganizationUser(organisationUserID));
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

        public static Boolean UpdateClientContactOrganisationUser(OrganizationUserContract organizationUserContract, int tenantID, String userID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().UpdateClientContactOrganisationUser(organizationUserContract, userID);
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

        public static SharedSystemDocument GetSharedSystemDocument(Int32 sharedSystemDocID)
        {
            try
            {
                return BALUtils.GetClientContactRepoInstance().GetSharedSystemDocument(sharedSystemDocID);
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

        #endregion

        #endregion

        #region PRIVATE METHODS

        #region ManageClientContact Screen (Instructor/Perceptor)

        private static ClientContact GetClientContactEntityObject(ClientContactContract clientContactContarct, Int32 loggedInUserID)
        {
            ClientContact clientContact = new ClientContact();
            clientContact.CC_Name = clientContactContarct.Name;
            clientContact.CC_Email = clientContactContarct.Email;
            clientContact.CC_Phone = clientContactContarct.Phone;
            clientContact.CC_TenantID = clientContactContarct.TenantID;
            clientContact.CC_UserID = clientContactContarct.UserID;
            clientContact.CC_TokenID = clientContactContarct.TokenID;
            clientContact.CC_ClientContactTypeID = clientContactContarct.ClientContactTypeID;
            clientContact.CC_IsDeleted = false;
            clientContact.CC_CreatedByID = loggedInUserID;
            clientContact.CC_CreatedOn = DateTime.Now;
            //UAt-2447
            clientContact.CC_IsInternationalPhone = clientContactContarct.IsInternationalPhone;

            return clientContact;
        }

        private static ClientContactAvailibilty GetClientContactAvailibiltyEntityObject(ClientContactAvailibiltyContract clientContactavailibiltyContract, Int32 loggedInUserID)
        {
            ClientContactAvailibilty clientContactAvailibilty = new ClientContactAvailibilty();
            //clientContactAvailibilty.CCA_ClientContactID = clientContactavailibiltyContract.ClientContactID;
            clientContactAvailibilty.CCA_WeekDayID = clientContactavailibiltyContract.WeekDayID;
            clientContactAvailibilty.CCA_StartTime = new TimeSpan(clientContactavailibiltyContract.StartTime.Value.Hour, clientContactavailibiltyContract.StartTime.Value.Minute, clientContactavailibiltyContract.StartTime.Value.Second);
            clientContactAvailibilty.CCA_EndTime = new TimeSpan(clientContactavailibiltyContract.EndTime.Value.Hour, clientContactavailibiltyContract.EndTime.Value.Minute, clientContactavailibiltyContract.EndTime.Value.Second);
            clientContactAvailibilty.CCA_ClientContactID = clientContactavailibiltyContract.ClientContactID;
            clientContactAvailibilty.CCA_IsDeleted = false;
            clientContactAvailibilty.CCA_CreatedByID = loggedInUserID;
            clientContactAvailibilty.CCA_CreatedOn = DateTime.Now;

            return clientContactAvailibilty;
        }

        /// <summary>
        /// Convert Entity Object to Contract
        /// </summary>
        /// <param name="temporaryContactList">List of ClientContact Entity</param>
        /// <returns>List of ClientContactContract</returns>
        private static List<ClientContactContract> ConvertClientContactEntityToContact(List<ClientContact> temporaryContactList)
        {
            List<Tenant> listTenants = BALUtils.GetSecurityRepoInstance().GetTenants(true).ToList();
            List<ClientContactContract> clientContactContractList = new List<ClientContactContract>();
            if (!temporaryContactList.IsNullOrEmpty())
            {
                foreach (ClientContact clientContact in temporaryContactList)
                {
                    ClientContactContract clientContactContract = new ClientContactContract();
                    clientContactContract.ClientContactID = clientContact.CC_ID;
                    clientContactContract.Email = clientContact.CC_Email;
                    clientContactContract.Name = clientContact.CC_Name;
                    clientContactContract.Phone = clientContact.CC_Phone;
                    clientContactContract.TenantID = clientContact.CC_TenantID;
                    clientContactContract.TenantName = listTenants.Where(x => x.TenantID == clientContact.CC_TenantID && !x.IsDeleted && x.IsActive).Select(x => x.TenantName).FirstOrDefault();
                    clientContactContract.UserID = clientContact.CC_UserID;
                    clientContactContract.TokenID = clientContact.CC_TokenID;
                    clientContactContract.ClientContactTypeID = clientContact.CC_ClientContactTypeID.HasValue ? clientContact.CC_ClientContactTypeID.Value : AppConsts.NONE;
                    //UAT-2447
                    clientContactContract.IsInternationalPhone = clientContact.CC_IsInternationalPhone;
                    //UAT-4160
                    clientContactContract.IsRegistered = clientContact.IsRegistered;
                    clientContactContract.AccountActivated = clientContact.IsRegistered ? "Yes" : "No";
                    clientContactContractList.Add(clientContactContract);
                }
            }
            return clientContactContractList;
        }

        /// <summary>
        /// Convert Entity Object to Contract
        /// </summary>
        /// <param name="temporaryContactList">List of SharedSystemDocument Entity</param>
        /// <returns>List of SharedSystemDocumentContract</returns>
        private static List<SharedSystemDocumentContract> ConvertSharedSystemDocumentEntityToContact(List<SharedSystemDocument> sharedSystemDocumentList)
        {
            List<SharedSystemDocumentContract> sharedSystemDocumentContractList = new List<SharedSystemDocumentContract>();
            if (!sharedSystemDocumentList.IsNullOrEmpty())
            {
                List<lkpSharedSystemDocType> listSharedDocType = LookupManager.GetSharedDBLookUpData<lkpSharedSystemDocType>().Where(x => !x.SSDT_IsDeleted).ToList();
                foreach (SharedSystemDocument sharedSystemDocument in sharedSystemDocumentList)
                {
                    SharedSystemDocumentContract sharedSystemDocumentContract = new SharedSystemDocumentContract();
                    sharedSystemDocumentContract.DocumentID = sharedSystemDocument.SSD_ID;
                    sharedSystemDocumentContract.DocumentPath = sharedSystemDocument.SSD_DocumentPath;
                    sharedSystemDocumentContract.DocumentTypeID = sharedSystemDocument.SSD_DocumentTypeID.HasValue ? sharedSystemDocument.SSD_DocumentTypeID.Value : AppConsts.NONE;
                    sharedSystemDocumentContract.DocumentTypeName = listSharedDocType.Where(x => x.SSDT_ID == sharedSystemDocumentContract.DocumentTypeID).Select(x => x.SSDT_Name).FirstOrDefault();
                    sharedSystemDocumentContract.Description = sharedSystemDocument.SSD_Description;
                    sharedSystemDocumentContract.FileName = sharedSystemDocument.SSD_FileName;
                    sharedSystemDocumentContract.FileSize = sharedSystemDocument.SSD_Size.HasValue ? sharedSystemDocument.SSD_Size.Value : AppConsts.NONE;
                    //Add temp document ids
                    if (sharedSystemDocumentContractList.IsNullOrEmpty())
                        sharedSystemDocumentContract.TempDocumentID = AppConsts.ONE; //List is empty, means 1st record.
                    else
                        sharedSystemDocumentContract.TempDocumentID = sharedSystemDocumentContractList.Count() + AppConsts.ONE;
                    sharedSystemDocumentContractList.Add(sharedSystemDocumentContract);
                }
            }
            return sharedSystemDocumentContractList;
        }

        private static List<ClientContactTypeContract> ConvertLkpClientContactTypeToContract(List<lkpClientContactType> clientContactTypeList)
        {
            List<ClientContactTypeContract> lstClientContactContractType = new List<ClientContactTypeContract>();
            if (!clientContactTypeList.IsNullOrEmpty())
            {
                foreach (lkpClientContactType clientContactType in clientContactTypeList)
                {
                    ClientContactTypeContract clientContactContractType = new ClientContactTypeContract();
                    clientContactContractType.ClientContactTypeID = clientContactType.CCT_ID;
                    clientContactContractType.Name = clientContactType.CCT_Name;
                    clientContactContractType.Code = clientContactType.CCT_Code;
                    clientContactContractType.Description = clientContactType.CCT_Description;
                    clientContactContractType.IsDeleted = clientContactType.CCT_IsDeleted;
                    lstClientContactContractType.Add(clientContactContractType);
                }
            }
            return lstClientContactContractType;
        }

        private static List<WeekDayContract> ConvertLkpWeekDayToContract(List<lkpWeekDay> weekDaylist)
        {
            List<WeekDayContract> lstWeekDayContract = new List<WeekDayContract>();
            if (!weekDaylist.IsNullOrEmpty())
            {
                foreach (lkpWeekDay weekDay in weekDaylist)
                {
                    WeekDayContract weekDayContract = new WeekDayContract();
                    weekDayContract.WeekDayID = weekDay.WD_ID;
                    weekDayContract.Name = weekDay.WD_Name;
                    weekDayContract.Code = weekDay.WD_Code;
                    weekDayContract.Description = weekDay.WD_Description;
                    lstWeekDayContract.Add(weekDayContract);
                }
            }
            return lstWeekDayContract;
        }

        private static List<SharedSystemDocTypeContract> ConvertlkpShrdSysDocToContract(List<lkpSharedSystemDocType> sharedSystemDocTypelist)
        {
            List<SharedSystemDocTypeContract> lstSharedSystemDocTypeContract = new List<SharedSystemDocTypeContract>();
            if (!sharedSystemDocTypelist.IsNullOrEmpty())
            {
                foreach (lkpSharedSystemDocType sharedSystemDocType in sharedSystemDocTypelist)
                {
                    SharedSystemDocTypeContract sharedSystemDocTypeContract = new SharedSystemDocTypeContract();
                    sharedSystemDocTypeContract.SharedSystemDocTypeID = sharedSystemDocType.SSDT_ID;
                    sharedSystemDocTypeContract.Name = sharedSystemDocType.SSDT_Name;
                    sharedSystemDocTypeContract.Code = sharedSystemDocType.SSDT_Code;
                    sharedSystemDocTypeContract.Description = sharedSystemDocType.SSDT_Description;
                    lstSharedSystemDocTypeContract.Add(sharedSystemDocTypeContract);
                }
            }
            return lstSharedSystemDocTypeContract;
        }

        private static List<ClientContactAvailibiltyContract> ConvertClientContactAvailibiltyEntityToContact(List<ClientContactAvailibilty> clientContactAvailibiltylist)
        {
            List<ClientContactAvailibiltyContract> lstClientContactAvailibiltyContract = new List<ClientContactAvailibiltyContract>();
            if (!clientContactAvailibiltylist.IsNullOrEmpty())
            {
                foreach (ClientContactAvailibilty clientContactAvailibilty in clientContactAvailibiltylist.Where(x => !x.CCA_IsDeleted))
                {
                    ClientContactAvailibiltyContract clientContactAvailibiltyContract = new ClientContactAvailibiltyContract();
                    clientContactAvailibiltyContract.ClientContactAvailibiltyID = clientContactAvailibilty.CCA_ID;
                    clientContactAvailibiltyContract.ClientContactID = clientContactAvailibilty.CCA_ClientContactID.HasValue ? clientContactAvailibilty.CCA_ClientContactID.Value : AppConsts.NONE;
                    if (clientContactAvailibilty.CCA_StartTime != null)
                    {
                        clientContactAvailibiltyContract.StartTime = new DateTime(clientContactAvailibilty.CCA_StartTime.Value.Ticks);
                    }
                    else
                    {
                        clientContactAvailibiltyContract.StartTime = new DateTime();
                    }

                    if (clientContactAvailibilty.CCA_EndTime != null)
                    {
                        clientContactAvailibiltyContract.EndTime = new DateTime(clientContactAvailibilty.CCA_EndTime.Value.Ticks);
                    }
                    else
                    {
                        clientContactAvailibiltyContract.EndTime = new DateTime();
                    }

                    clientContactAvailibiltyContract.WeekDayID = clientContactAvailibilty.CCA_WeekDayID.HasValue ? clientContactAvailibilty.CCA_WeekDayID.Value : AppConsts.NONE;
                    lstClientContactAvailibiltyContract.Add(clientContactAvailibiltyContract);
                }
            }
            return lstClientContactAvailibiltyContract;
        }

        private static ClientContactDocument GetClientContactDocumentObj(Int32 loggedInUserID)
        {
            ClientContactDocument clientContactDocument = new ClientContactDocument();
            //clientContactDocument.CCD_ClientContactID = ClientContactID;
            clientContactDocument.CCD_IsDeleted = false;
            clientContactDocument.CCD_CreatedByID = loggedInUserID;
            clientContactDocument.CCD_CreatedOn = DateTime.Now;
            return clientContactDocument;
        }

        private static SharedSystemDocument GetSharedSystemDocumentObj(Int32 loggedInUserID, SharedSystemDocumentContract document)
        {
            SharedSystemDocument sharedSystemDocument = new SharedSystemDocument();
            sharedSystemDocument.SSD_FileName = document.FileName;
            sharedSystemDocument.SSD_Size = document.FileSize;
            sharedSystemDocument.SSD_DocumentPath = document.DocumentPath;
            sharedSystemDocument.SSD_DocumentTypeID = document.DocumentTypeID;
            sharedSystemDocument.SSD_Description = document.Description;
            sharedSystemDocument.SSD_IsDeleted = false;
            sharedSystemDocument.SSD_CreatedByID = loggedInUserID;
            sharedSystemDocument.SSD_CreatedOn = DateTime.Now;
            return sharedSystemDocument;
        }

        private static void AddNewClientContactAvailibilty(List<ClientContactAvailibiltyContract> clientContactavailibiltyContract, Int32 loggedInUserID, ClientContact clientContact, List<Int32> weekDays_Added)
        {
            foreach (Int32 weekdayID in weekDays_Added)
            {
                foreach (ClientContactAvailibiltyContract objclientContactavailibiltyContract in clientContactavailibiltyContract)
                {
                    if (weekdayID == objclientContactavailibiltyContract.WeekDayID)
                    {
                        ClientContactAvailibilty clientContactAvailibilty = new ClientContactAvailibilty();
                        //clientContactAvailibilty.CCA_ClientContactID = clientContactavailibiltyContract.ClientContactID;
                        clientContactAvailibilty.CCA_WeekDayID = weekdayID;
                        clientContactAvailibilty.CCA_StartTime = new TimeSpan(objclientContactavailibiltyContract.StartTime.Value.Hour, objclientContactavailibiltyContract.StartTime.Value.Minute, objclientContactavailibiltyContract.StartTime.Value.Second);
                        clientContactAvailibilty.CCA_EndTime = new TimeSpan(objclientContactavailibiltyContract.EndTime.Value.Hour, objclientContactavailibiltyContract.EndTime.Value.Minute, objclientContactavailibiltyContract.EndTime.Value.Second);
                        clientContactAvailibilty.CCA_IsDeleted = false;
                        clientContactAvailibilty.CCA_CreatedByID = loggedInUserID;
                        clientContactAvailibilty.CCA_CreatedOn = DateTime.Now;

                        clientContact.ClientContactAvailibilties.Add(clientContactAvailibilty);
                    }
                }
            }
        }

        private static void UpdateDeletePreviousContactAvailibilty(List<ClientContactAvailibiltyContract> clientContactavailibiltyContract, Int32 loggedInUserID, ClientContact clientContact, List<Int32> weekDays_Removed)
        {
            foreach (ClientContactAvailibilty clientContactAvailibilty in clientContact.ClientContactAvailibilties)
            {
                if (clientContactavailibiltyContract.Count > 0)
                {
                    foreach (ClientContactAvailibiltyContract objclientContactavailibiltyContract in clientContactavailibiltyContract)
                    {

                        if (clientContactAvailibilty.CCA_ID == objclientContactavailibiltyContract.ClientContactAvailibiltyID)
                        {
                            if (clientContactavailibiltyContract[0].StartTime != null)
                            {
                                //clientContactAvailibilty.CCA_StartTime = new TimeSpan(clientContactavailibiltyContract.Any() ? clientContactavailibiltyContract[0].StartTime.Value.Hour : 00, clientContactavailibiltyContract.Any() ? clientContactavailibiltyContract[0].StartTime.Value.Minute : 00, clientContactavailibiltyContract.Any() ? clientContactavailibiltyContract[0].StartTime.Value.Second : 00);
                                clientContactAvailibilty.CCA_StartTime = objclientContactavailibiltyContract.IsNotNull() ? new TimeSpan(objclientContactavailibiltyContract.StartTime.Value.Hour, objclientContactavailibiltyContract.StartTime.Value.Minute, objclientContactavailibiltyContract.StartTime.Value.Second) : new TimeSpan(00, 00, 00);
                            }
                            else
                            {
                                clientContactAvailibilty.CCA_StartTime = null;
                            }
                            if (clientContactavailibiltyContract[0].EndTime != null)
                            {
                                //clientContactAvailibilty.CCA_EndTime = new TimeSpan(clientContactavailibiltyContract.Any() ? clientContactavailibiltyContract[0].EndTime.Value.Hour : 00, clientContactavailibiltyContract.Any() ? clientContactavailibiltyContract[0].EndTime.Value.Minute : 00, clientContactavailibiltyContract.Any() ? clientContactavailibiltyContract[0].EndTime.Value.Second : 00);
                                clientContactAvailibilty.CCA_EndTime = objclientContactavailibiltyContract.IsNotNull() ? new TimeSpan(objclientContactavailibiltyContract.EndTime.Value.Hour, objclientContactavailibiltyContract.EndTime.Value.Minute, objclientContactavailibiltyContract.EndTime.Value.Second) : new TimeSpan(00, 00, 00);
                            }
                            else
                            {
                                clientContactAvailibilty.CCA_EndTime = null;
                            }
                        }
                    }
                }

                clientContactAvailibilty.CCA_ModifiedByID = loggedInUserID;
                clientContactAvailibilty.CCA_ModifiedOn = DateTime.Now;
                //Remove the unselected Weekdays
                if (weekDays_Removed.Contains(clientContactAvailibilty.CCA_WeekDayID.Value))
                {
                    clientContactAvailibilty.CCA_IsDeleted = true;
                }
            }
        }

        private static void AddUploadedClientDocument(Int32 loggedInUserID, ClientContact clientContact, List<SharedSystemDocumentContract> newUploadedDocuments)
        {
            foreach (SharedSystemDocumentContract document in newUploadedDocuments)
            {
                SharedSystemDocument sharedSystemDocument = GetSharedSystemDocumentObj(loggedInUserID, document);
                ClientContactDocument clientContactDocument = GetClientContactDocumentObj(loggedInUserID);

                sharedSystemDocument.ClientContactDocuments.Add(clientContactDocument);
                clientContact.ClientContactDocuments.Add(clientContactDocument);
            }
        }

        private static void RemoveDeletedClientDocuments(Int32 loggedInUserID, ClientContact clientContact, List<Int32> documentIds_Removed)
        {
            foreach (ClientContactDocument clientContactDocument in clientContact.ClientContactDocuments)
            {
                if (documentIds_Removed.Contains(clientContactDocument.CCD_SharedSystemDocumentID.Value))
                {
                    clientContactDocument.CCD_IsDeleted = true;
                    clientContactDocument.CCD_ModifiedByID = loggedInUserID;
                    clientContactDocument.CCD_ModifiedOn = DateTime.Now;

                    clientContactDocument.SharedSystemDocument.SSD_IsDeleted = true;
                    clientContactDocument.SharedSystemDocument.SSD_ModifiedByID = loggedInUserID;
                    clientContactDocument.SharedSystemDocument.SSD_ModifiedOn = DateTime.Now;
                }
            }
        }

        private static Boolean CheckIsContactAlreadyRecievdEmail(Int32 subEventID, String emailID)
        {
            return BALUtils.GetCommunicationRepoInstance().CheckIsContactAlreadyRecievdEmail(subEventID, emailID);
        }

        #region UAT-4160
        private static Boolean CheckIfClientContactsAlreadyDeleted(String emailId,Int32 tenantId)
        {
            return BALUtils.GetClientContactRepoInstance().CheckIfClientContactsAlreadyDeleted(emailId, tenantId);
        }
        #endregion

        ///// <summary>
        ///// Send the email for the Invitation
        ///// </summary>
        ///// <param name="toAddress"></param>
        ///// <param name="emailContent"></param>
        ///// <param name="subject"></param>
        ///// <param name="dicContent"></param>
        ///// <param name="isContentReplaced"></param>
        //public static Boolean SendInvitationEmail(String toAddress, String emailContent, String subject)
        //{
        //    Dictionary<String, String> dicEmailContent = new Dictionary<String, String>
        //        {
        //            {"EmailBody", emailContent} 
        //        };

        //    return SysXEmailService.SendSystemMail(dicEmailContent, subject, toAddress);
        //}

        #endregion

        #region Client Contact Profile
        private static OrganizationUserContract ConvertEntityToOrganizationUserContract(OrganizationUser organizationUser)
        {
            OrganizationUserContract organizationUserContract = new OrganizationUserContract();
            organizationUserContract.OrganizationUserID = organizationUser.OrganizationUserID;
            organizationUserContract.UserID = organizationUser.UserID;
            organizationUserContract.OrganizationID = organizationUser.OrganizationID;
            organizationUserContract.FirstName = organizationUser.FirstName;
            organizationUserContract.MiddleName = organizationUser.MiddleName;
            organizationUserContract.LastName = organizationUser.LastName;
            organizationUserContract.Email = organizationUser.PrimaryEmailAddress;
            organizationUserContract.IsOutOfOffice = organizationUser.IsOutOfOffice;
            organizationUserContract.IsNewPassword = organizationUser.IsNewPassword;
            organizationUserContract.IgnoreIPRestriction = organizationUser.IgnoreIPRestriction;
            organizationUserContract.IsMessagingUser = organizationUser.IsMessagingUser;
            organizationUserContract.IsSystem = organizationUser.IsSystem;
            organizationUserContract.SSN = organizationUser.SSNL4;
            organizationUserContract.IsDeleted = organizationUser.IsDeleted;
            organizationUserContract.IsActive = organizationUser.IsActive;
            organizationUserContract.CreatedByID = organizationUser.CreatedByID;
            organizationUserContract.CreatedOn = organizationUser.CreatedOn;
            organizationUserContract.IsSubscribeToEmail = organizationUser.IsSubscribeToEmail;

            return organizationUserContract;
        }
        #endregion

        #endregion


        public static List<Int32> GetClientContactTenantIdList(Guid clientContactTokenID)
        {
            try
            {
                return BALUtils.GetClientContactRepoInstance().GetClientContactTenantIdList(clientContactTokenID);
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

        #region UAT-2042, Phase 4 (24): Preceptor Clinical Rotations
        public static Int32 GetClientContactByUserID(Int32 tenantID, String userID)
        {
            try
            {
                return BALUtils.GetClientContactRepoInstance().GetClientContactByUserID(tenantID, userID);
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
        #endregion
    }
}
