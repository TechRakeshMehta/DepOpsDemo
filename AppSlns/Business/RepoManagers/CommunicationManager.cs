using Business.Observer;
using Entity;
using Entity.ClientEntity;
using INTSOF.Services.Observer;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Alumni;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Business.RepoManagers
{
    public class CommunicationManager
    {

        #region Communication Subscription Settings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="selectedCompositeEventKeys"></param>
        /// <param name="communicationTypeIds"></param>
        /// <param name="modifiedByID"></param>
        /// <returns></returns>
        public static bool AddUserCommunicationSubscriptionSettings(
            Int32 organizationUserId,
            List<Int32> selectedNotificationEventIds,
            List<Int32> selectedReminderEventIds,
            int modifiedByID)
        {
            try
            {
                string notificationCode = lkpCommunicationTypeContext.NOTIFICATION.GetStringValue();
                string reminderCode = lkpCommunicationTypeContext.REMINDERS.GetStringValue();
                Int32 notificationCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(notificationCode).CommunicationTypeID;
                Int32 reminderCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(reminderCode).CommunicationTypeID;
                List<lkpCommunicationEvent> communicationEvents = GetCommunicationEvents(notificationCommunicationTypeId);
                return BALUtils.GetCommunicationRepoInstance().AddUserCommunicationSubscriptionSettings(
                    organizationUserId,
                    selectedNotificationEventIds,
                    selectedReminderEventIds,
                    notificationCommunicationTypeId,
                    reminderCommunicationTypeId,
                    communicationEvents,
                    modifiedByID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiedById"></param>
        /// <param name="selectedSubscriptionSettings"></param>
        /// <param name="unSelectedSubscriptionSettings"></param>
        /// <returns></returns>
        public static bool AddUserCommunicationSubscriptionSettings(Int32 modifiedById, List<UserCommunicationSubscriptionSetting> selectedSubscriptionSettings, List<UserCommunicationSubscriptionSetting> unSelectedSubscriptionSettings)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().AddUserCommunicationSubscriptionSettings(
                    modifiedById,
                    selectedSubscriptionSettings,
                    unSelectedSubscriptionSettings);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userCommunicationSubscriptionSettings"></param>
        /// <returns></returns>
        public static bool AddUserCommunicationSubscriptionSettings(List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().AddUserCommunicationSubscriptionSettings(userCommunicationSubscriptionSettings);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="communicationTypeId"></param>
        /// <returns></returns>
        public static IEnumerable<UserCommunicationSubscriptionSetting> GetUserCommunicationSubscriptionSettings(Int32 organizationUserId, Int32 communicationTypeId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetUserCommunicationSubscriptionSettings(organizationUserId, communicationTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="communicationEventId"></param>
        /// <returns></returns>
        public static UserCommunicationSubscriptionSetting GetUserCommunicationSubscriptionSettings(Int32 organizationUserId, Int32 communicationTypeId, Int32 communicationEventId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetUserCommunicationSubscriptionSettings(organizationUserId, communicationTypeId, communicationEventId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        #endregion

        #region Communication Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="communicationTypeId"></param>
        /// <returns></returns>
        public static List<lkpCommunicationEvent> GetCommunicationEvents(Int32 communicationTypeId)
        {
            try
            {
                IEnumerable<Int32> communicationEventIds = LookupManager.GetMessagingLookUpData<lkpCommunicationSubEvent>()
                    .Where(condition => condition.CommunicationTypeID.Equals(communicationTypeId) && condition.IsDeleted == false)
                    .Select(condition => condition.CommunicationEventID).Distinct();

                return LookupManager.GetMessagingLookUpData<lkpCommunicationEvent>()
                    .Where(condition => communicationEventIds.Contains(condition.CommunicationEventID)).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;

        }

        /// <summary>
        /// Gets the list of the communication types
        /// </summary>
        /// <returns>List of communication types</returns>
        public static List<lkpCommunicationType> GetCommunicationTypes()
        {
            try
            {
                String _messageCode = lkpCommunicationTypeContext.MESSAGE.GetStringValue();
                String _emailCode = lkpCommunicationTypeContext.EMAIL.GetStringValue();
                String _smsCode = lkpCommunicationTypeContext.SMS.GetStringValue();

                return LookupManager.GetMessagingLookUpData<lkpCommunicationType>().Where(condition => !condition.Code.ToLower().Equals(_messageCode.ToLower().Trim())
                    && !condition.Code.ToLower().Equals(_emailCode.ToLower().Trim()) && !condition.Code.ToLower().Equals(_smsCode.ToLower().Trim())).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        // <summary>
        /// Gets the list of the languages
        /// </summary>
        /// <returns>List of languages</returns>
        public static List<Entity.lkpLanguage> GetLanguages()
        {
            try
            {
                return LookupManager.GetLanguageLookUpData<Entity.lkpLanguage>().Where(condition => !condition.LAN_IsDeleted).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// Gets the list of the Sub-Events 
        /// </summary>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        public static List<lkpCommunicationSubEvent> GetCommunicationTypeSubEvents(Int32 communicationType, Int32 eventTypeId)
        {
            try
            {
                //var defaultLanguageCode = CommunicationLanguages.DEFAULT.GetStringValue();
                //var defaultLanguageId = LookupManager.GetLanguageLookUpData<lkpLanguage>()
                //    .Where(l=>l.LAN_Code == defaultLanguageCode)
                //    .First()
                //    .LAN_ID;

                return BALUtils.GetCommunicationRepoInstance().GetCommunicationTypeSubEvents(communicationType, eventTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subEventId"></param>
        /// <returns></returns>
        public static List<SystemEventSetting> GetSystemEventSetting(Int32 subEventId)
        {
            try
            {
                //return LookupManager.GetMessagingLookUpData<SystemEventSetting>().Where(condition => condition.CommunicationSubEventID.Equals(subEventId) && condition.IsDeleted == false).ToList();
                return BALUtils.GetCommunicationRepoInstance().GetSystemEventSetting(subEventId);
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
        /// 
        /// </summary>
        /// <param name="subEventId"></param>
        /// <returns></returns>
        public static List<CommunicationTemplate> GetCommunicationTemplate(Int32 subEventId)
        {
            try
            {
                //return LookupManager.GetMessagingLookUpData<CommunicationTemplate>().Where(condition => condition.CommunicationSubEventID.Equals(subEventId) && condition.IsDeleted == false).ToList();
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationTemplate(subEventId);
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
        /// 
        /// </summary>
        /// <param name="subEventId"></param>
        /// <returns></returns>
        public static List<CommunicationTemplatePlaceHolderSubEvent> GetCommunicationTemplatePlaceHolderSubEvents(Int32 subEventId)
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<CommunicationTemplatePlaceHolderSubEvent>().Where(condition => condition.CommunicationSubEventID.Equals(subEventId)).ToList();
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
        /// 
        /// </summary>
        /// <param name="subEventId"></param>
        /// <returns></returns>
        public static List<CommunicationTemplatePlaceHolder> GetCommunicationTemplatePlaceHolders()
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<CommunicationTemplatePlaceHolder>().ToList();
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

        public static List<lkpCommunicationSubEvent> GetCommunicationTypeSubEventsSpecific(Int32 communicationType, Int32 eventId, List<String> lstSubeventSpecificCodes)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationTypeSubEventsSpecific(communicationType, eventId, lstSubeventSpecificCodes);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the list of the ids of the subevents, for which codes are provided
        /// </summary>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        public static List<Int32> GetCommunicationSubEventIdsByCodes(List<String> lstCodes)
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpCommunicationSubEvent>()
                .Where(condition => lstCodes.Contains(condition.Code.ToLower()) && condition.IsDeleted == false)
                .Select(condition => condition.CommunicationSubEventID).ToList();
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

        public static List<Int32?> GetItemIdsInUse(Int32 categoryId, Int32 subEventId, Int32 tenantId)
        {
            try
            {
                List<SystemEventSetting> systemEventSetting = GetSystemEventSetting(subEventId);
                return BALUtils.GetCommunicationRepoInstance().GetItemIdsInUse(categoryId, tenantId, systemEventSetting);
                // return BALUtils.GetCommunicationRepoInstance().GetItemIdsInUse(categoryId, subEventId, tenantId);
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
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static lkpCommunicationSubEvent GetCommunicationTypeSubEvents(string code)
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpCommunicationSubEvent>().Where(condition => condition.Code.Equals(code) && condition.IsDeleted == false).FirstOrDefault();
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
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static lkpCommunicationSubEvent GetCommunicationTypeSubEvents(Int32 subEventId)
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpCommunicationSubEvent>().Where(condition => condition.CommunicationSubEventID.Equals(subEventId) && condition.IsDeleted == false).FirstOrDefault();
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
        /// To delete Communication Template by nodeNotificationMappingID and tenantId
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nodeNotificationMappingID"></param>
        /// <returns></returns>
        public static Boolean DeleteCommunicationTemplates(Int32 tenantId, Int32 nodeNotificationMappingID, Int32 loggedInUserId, DateTime modifiedOn)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().DeleteCommunicationTemplateByMapID(tenantId, nodeNotificationMappingID, loggedInUserId, modifiedOn);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
        }

        #endregion

        #region Communication Summary
        ///Methode before  UAT-1427: WB:  commented by :charanjot ,Update behavior of CommunicationSummary search
        //public static IQueryable<CommunicationTemplateContract> GetCommunicationSummary()
        //{
        //    try
        //    {
        //        return BALUtils.GetCommunicationRepoInstance().GetCommunicationSummary();
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Created By : Charanjot for UAT-1427: WB: 
        /// </summary>
        /// <param name="searchDataContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        public static List<CommunicationTemplateContract> GetCommunicationSummarysearch(SearchCommunicationTemplateContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationSummarySearch(searchDataContract, gridCustomPaging);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Created By Pawan Kapoor for UAT-3333 
        /// </summary>
        /// <param name="searchDataContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        public static List<CommunicationTemplateContract> GetCommunicationSummarysearchArchive(SearchCommunicationTemplateContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationSummarySearchArchive(searchDataContract, gridCustomPaging);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }


        public static SystemCommunication GetSystemNotificationDetails(Int32 systemCommunicationId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetSystemNotificationDetails(systemCommunicationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new SystemCommunication();
        }

        public static SystemCommunication GetSystemNotificationDetailsArchive(Int32 systemCommunicationId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetSystemNotificationDetailsArchive(systemCommunicationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new SystemCommunication();
        }

        public static void QueueReSendingEmails(List<Int32> systemCommunicationDeliveryIds, Int32 currentUserId, Boolean isAttachment = false)
        {
            List<SystemCommunicationDelivery> lstCommunicationDelivery = BALUtils.GetCommunicationRepoInstance().GetSysCommunicationDeliveriesByIds(systemCommunicationDeliveryIds);
            if (!lstCommunicationDelivery.IsNullOrEmpty() && lstCommunicationDelivery.Count > 0)
            {
                foreach (var systemComDeliveryId in systemCommunicationDeliveryIds)
                {
                    CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract
                    {
                        ReceiverOrganizationUserId = Convert.ToInt32(lstCommunicationDelivery.Where(sysComDelivery => sysComDelivery.SystemCommunicationDeliveryID == systemComDeliveryId).FirstOrDefault().ReceiverOrganizationUserID),
                        CommunicationSubEventID = lstCommunicationDelivery.Where(sysComDelivery => sysComDelivery.SystemCommunicationDeliveryID == systemComDeliveryId).FirstOrDefault().SystemCommunication.CommunicationSubEventID,
                    };
                    //Send mail
                    CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NONE, null, communicationTemplateContract, 0, 0, null, null, false, systemComDeliveryId, currentUserId, false, null, false, null, null, null, isAttachment);
                }
            }
            else
            {
                List<CommunicationTemplateContract> lstCommunicationTemplateContract = BALUtils.GetCommunicationRepoInstance().GetSysCommunicationDeliveriesByIdsArchive(systemCommunicationDeliveryIds);
                foreach (var systemComDeliveryId in systemCommunicationDeliveryIds)
                {
                    CommunicationTemplateContract communicationTemplateContract = lstCommunicationTemplateContract.Where(cond => cond.SystemCommunicationDeliveryID == systemComDeliveryId).FirstOrDefault();
                    //Send mail
                    CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NONE, null, communicationTemplateContract, 0, 0, null, null, false, systemComDeliveryId, currentUserId, false, null, false, null, null, null, isAttachment);
                }
            }
        }

        #endregion

        #region Communication Mock Up

        /// <summary>
        /// Gets the Mock data of the user for mock up screen
        /// </summary>
        /// <returns>Data of a random user</returns>
        public static CommunicationMockUpData GetMockUpData(String subEventCode)
        {
            try
            {
                Int32 subEventId = GetCommunicationTypeSubEvents(subEventCode).CommunicationSubEventID;
                return BALUtils.GetCommunicationRepoInstance().GetMockUpData(subEventId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        #endregion

        #region Mail and Message

        public static void SendOrderCreationMailMoneyOrder(Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, int tenantId, String _paymentModeCode)
        {
            try
            {
                String entityTypeCode = null;
                Int32? entityID = null;
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;
                if (applicantOrder.lkpOrderPackageType != null)
                {
                    switch (applicantOrder.lkpOrderPackageType.OPT_Code)
                    {
                        case "AAAA":
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;
                            break;
                        case "AAAB":
                            entityID = applicantOrder.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_MONEY_ORDER_AMSPACKAGES;
                            break;
                        case "AAAC":
                            entityID = applicantOrder.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;
                            break;
                    }
                }
                else
                {
                    commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;
                }

                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = GetOrderCreationMoneyOrderMailData(applicantOrder, orgUserProfile, tenantId, _paymentModeCode);

                mockData.UserName = string.Concat(orgUserProfile.FirstName, " ", orgUserProfile.LastName);
                mockData.EmailID = orgUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orgUserProfile.OrganizationUserID;
                //Send mail
                //SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, applicantOrder.HierarchyNodeID.Value, entityID, entityTypeCode);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, applicantOrder.SelectedNodeID.HasValue ? applicantOrder.SelectedNodeID.Value : 0, entityID, entityTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderCreationMessageMoneyOrder(Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, Int32 tenantId, String _paymentModeCode)
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Applicant Order Id: " + (applicantOrder.IsNotNull() ? applicantOrder.OrderID.ToString() : String.Empty) + "";
                parametersPassed += ", Organisation User Profile Id: " + (orgUserProfile.IsNotNull() ? orgUserProfile.OrganizationUserProfileID.ToString() : String.Empty) + "";
                parametersPassed += ", Tenant Id: " + tenantId.ToString() + "";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: SendOrderCreationMessageMoneyOrder with " + parametersPassed + "");

                String entityTypeCode = null;
                Int32? entityID = null;
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;
                //Create Dictionary for Messaging Contract
                Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();

                if (applicantOrder.lkpOrderPackageType.IsNotNull())
                {
                    switch (applicantOrder.lkpOrderPackageType.OPT_Code)
                    {
                        case "AAAA":
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;
                            break;
                        case "AAAB":
                            entityID = applicantOrder.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            dicMessageParam.Add("EntityID", entityID);
                            dicMessageParam.Add("EntityTypeCode", entityTypeCode);
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_MONEY_ORDER_AMSPACKAGES;
                            break;
                        case "AAAC":
                            entityID = applicantOrder.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            dicMessageParam.Add("EntityID", entityID);
                            dicMessageParam.Add("EntityTypeCode", entityTypeCode);
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;
                            break;
                    }
                }
                else
                {
                    commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;
                }

                Dictionary<String, object> dictMailData = GetOrderCreationMoneyOrderMailData(applicantOrder, orgUserProfile, tenantId, _paymentModeCode);
                SaveMessageContent(commSubEvent, dictMailData, orgUserProfile.OrganizationUserID, tenantId, dicMessageParam);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderCreationMailInvoice(Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, int tenantId, String _paymentModeCode)
        {
            try
            {
                //UAT-3675
                Boolean isLocationSpecificTenant = SecurityManager.IsLocationServiceTenant(tenantId);

                String entityTypeCode = null;
                Int32? entityID = null;
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;
                if (applicantOrder.lkpOrderPackageType.IsNotNull())
                {
                    switch (applicantOrder.lkpOrderPackageType.OPT_Code)
                    {
                        case "AAAA":
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;
                            break;
                        case "AAAB":
                            //UAT-3675
                            if (isLocationSpecificTenant)
                                commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_CASH_AMSPACKAGES;
                            else
                            {
                                entityID = applicantOrder.lkpOrderPackageType.OPT_ID;
                                entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                                commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_INVOICE_AMSPACKAGES;
                            }
                            break;
                        case "AAAC":
                            entityID = applicantOrder.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;
                            break;
                    }
                }
                else
                {
                    commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;
                }
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = GetOrderCreationInvoiceMailData(applicantOrder, orgUserProfile, tenantId, _paymentModeCode);

                mockData.UserName = string.Concat(orgUserProfile.FirstName, " ", orgUserProfile.LastName);
                mockData.EmailID = orgUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orgUserProfile.OrganizationUserID;
                //Send mail
                //SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, applicantOrder.HierarchyNodeID.Value, entityID, entityTypeCode);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                if (!isLocationSpecificTenant || commSubEvent != CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_CASH_AMSPACKAGES)
                    SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, applicantOrder.SelectedNodeID.HasValue ? applicantOrder.SelectedNodeID.Value : 0, entityID, entityTypeCode);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderCreationMessageInvoice(Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, int tenantId, String _paymentModeCode)
        {
            try
            {
                String entityTypeCode = null;
                Int32? entityID = null;
                Boolean isLocationSpecificTenant = SecurityManager.IsLocationServiceTenant(tenantId);
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;
                Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                if (applicantOrder.lkpOrderPackageType.IsNotNull())
                {
                    switch (applicantOrder.lkpOrderPackageType.OPT_Code)
                    {
                        case "AAAA":
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;
                            break;
                        case "AAAB":
                            entityID = applicantOrder.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            dicMessageParam.Add("EntityID", entityID);
                            dicMessageParam.Add("EntityTypeCode", entityTypeCode);
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_INVOICE_AMSPACKAGES;
                            break;
                        case "AAAC":
                            entityID = applicantOrder.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            dicMessageParam.Add("EntityID", entityID);
                            dicMessageParam.Add("EntityTypeCode", entityTypeCode);
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;
                            break;
                    }
                }
                else
                {
                    commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;
                }
                Dictionary<String, object> dictMailData = GetOrderCreationInvoiceMailData(applicantOrder, orgUserProfile, tenantId, _paymentModeCode);
                if (!isLocationSpecificTenant || commSubEvent != CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_INVOICE_AMSPACKAGES)
                    SaveMessageContent(commSubEvent, dictMailData, orgUserProfile.OrganizationUserID, tenantId, dicMessageParam);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        //public static void SendOrderApprovalMail(PackageSubscription packageSubscription, int currentLoggedInUserId, int tenantId)
        public static Int32? SendOrderApprovalMail(OrderPaymentDetail orderPaymentDetail, int currentLoggedInUserId, int tenantId)
        {
            try
            {
                String entityTypeCode = null;
                Int32? entityID = null;
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                //Get paymentOption from OrderPaymentDetail [UAT-916]
                //String paymentOption = orderPaymentDetail.Order.lkpPaymentOption.Code;
                String paymentOption = orderPaymentDetail.lkpPaymentOption.IsNotNull() ? orderPaymentDetail.lkpPaymentOption.Code : String.Empty;
                //get OrderPackageTyepCode on the basis of OrderPaymentDetail [UAT-916]
                String orderPackageTypeCode = String.Empty;
                Int32 orderPackageTypeID = 0;
                List<lkpOrderPackageType> lstOrderPackageType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderPackageType>(tenantId).ToList();
                Boolean _isCompPackageInclude = orderPaymentDetail.OrderPkgPaymentDetails.Any(x => x.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue());
                Boolean _isBkgPackageInclude = orderPaymentDetail.OrderPkgPaymentDetails.Any(x => x.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue());
                Boolean _isBkgPackageToQualifyInRotInclude = orderPaymentDetail.OrderPkgPaymentDetails.Any(x => x.lkpOrderPackageType.OPT_Code == OrderPackageTypes.ADDITIONAL_PRICE_BACKGROUND_PACKAGE.GetStringValue()); //UAT-3268

                if (_isCompPackageInclude && _isBkgPackageInclude)
                {
                    orderPackageTypeCode = OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue();
                    orderPackageTypeID = lstOrderPackageType.FirstOrDefault(cnd => cnd.OPT_Code == orderPackageTypeCode && cnd.OPT_IsDeleted == false).OPT_ID;
                }
                else if (!_isCompPackageInclude && _isBkgPackageInclude)
                {
                    orderPackageTypeCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
                    orderPackageTypeID = lstOrderPackageType.FirstOrDefault(cnd => cnd.OPT_Code == orderPackageTypeCode && cnd.OPT_IsDeleted == false).OPT_ID;
                }
                else if (_isCompPackageInclude && !_isBkgPackageInclude)
                {
                    orderPackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
                    orderPackageTypeID = lstOrderPackageType.FirstOrDefault(cnd => cnd.OPT_Code == orderPackageTypeCode && cnd.OPT_IsDeleted == false).OPT_ID;
                }
                //UAT-3268
                else if (_isBkgPackageToQualifyInRotInclude)
                {
                    orderPackageTypeCode = OrderPackageTypes.ADDITIONAL_PRICE_BACKGROUND_PACKAGE.GetStringValue();
                    orderPackageTypeID = lstOrderPackageType.FirstOrDefault(cnd => cnd.OPT_Code == orderPackageTypeCode && cnd.OPT_IsDeleted == false).OPT_ID;
                }

                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                if (paymentOption == PaymentOptions.Credit_Card.GetStringValue())
                {
                    switch (orderPackageTypeCode)
                    {
                        case "AAAA":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;
                            break;
                        case "AAAB":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_PACKAGES;
                            break;
                        case "AAAC":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;
                            break;
                    }
                    dictMailData = GetCCOrderApprovalMailData(orderPaymentDetail, tenantId, orderPackageTypeCode);
                }
                else
                {
                    switch (orderPackageTypeCode)
                    {
                        case "AAAA":
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                            break;
                        case "AAAB":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_PACKAGES;
                            break;
                        case "AAAC":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_COMPLIO_PACKAGES;
                            break;
                        //UAT-3268
                        case "AAAG":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_PACKAGES;
                            break;
                        //END UAT-3268
                        default:
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                            break;
                    }
                    dictMailData = GetOrderApprovalMailData(orderPaymentDetail, tenantId, orderPackageTypeCode);
                }
                /*Commented below code for the implementation UAT-916
                 * if (paymentOption == PaymentOptions.Credit_Card.GetStringValue())
                {
                    switch (orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code)
                    {
                        case "AAAA":
                            entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;
                            break;
                        case "AAAB":
                            entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_PACKAGES;
                            break;
                        case "AAAC":
                            entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;
                            break;
                    }
                    dictMailData = GetCCOrderApprovalMailData(orderPaymentDetail, tenantId);
                }
                else
                {
                    switch (orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code)
                    {
                        case "AAAA":
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                            break;
                        case "AAAB":
                            entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_PACKAGES;
                            break;
                        case "AAAC":
                            entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                            break;
                    }
                    dictMailData = GetOrderApprovalMailData(orderPaymentDetail, tenantId);
                }*/

                mockData.UserName = string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName);
                mockData.EmailID = orderPaymentDetail.Order.OrganizationUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;
                //Send mail
                //SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL, mockData, dictMailData, tenantId, packageSubscription.Order.DeptProgramPackage.DeptProgramMapping.DPM_ID, null);
                //return SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, orderPaymentDetail.Order.HierarchyNodeID.Value, entityID, entityTypeCode);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                //return SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : 0, entityID, entityTypeCode);
                return SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : 0, null);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        //public static void SendOrderApprovalMessage(PackageSubscription orderPaymentDetail, int currentLoggedInUserId, int tenantId)
        public static Guid? SendOrderApprovalMessage(OrderPaymentDetail orderPaymentDetail, int currentLoggedInUserId, int tenantId, Dictionary<String, object> dicMessageAttachmentParam = null)
        {
            try
            {
                String entityTypeCode = null;
                Int32? entityID = null;
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                //Get paymentOption from OrderPaymentDetail [UAT-916]
                //String paymentOption = orderPaymentDetail.Order.lkpPaymentOption.Code;
                String paymentOption = orderPaymentDetail.lkpPaymentOption.IsNotNull() ? orderPaymentDetail.lkpPaymentOption.Code : String.Empty;
                //get OrderPackageTyepCode on the basis of OrderPaymentDetail [UAT-916]
                String orderPackageTypeCode = null;
                Int32? orderPackageTypeID = null;
                List<lkpOrderPackageType> lstOrderPackageType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderPackageType>(tenantId).ToList();
                Boolean _isCompPackageInclude = orderPaymentDetail.OrderPkgPaymentDetails.Any(x => x.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue());
                Boolean _isBkgPackageInclude = orderPaymentDetail.OrderPkgPaymentDetails.Any(x => x.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue());
                if (_isCompPackageInclude && _isBkgPackageInclude)
                {
                    orderPackageTypeCode = OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue();
                    orderPackageTypeID = lstOrderPackageType.FirstOrDefault(cnd => cnd.OPT_Code == orderPackageTypeCode && cnd.OPT_IsDeleted == false).OPT_ID;
                }
                else if (!_isCompPackageInclude && _isBkgPackageInclude)
                {
                    orderPackageTypeCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
                    orderPackageTypeID = lstOrderPackageType.FirstOrDefault(cnd => cnd.OPT_Code == orderPackageTypeCode && cnd.OPT_IsDeleted == false).OPT_ID;
                }
                else if (_isCompPackageInclude && !_isBkgPackageInclude)
                {
                    orderPackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
                    orderPackageTypeID = lstOrderPackageType.FirstOrDefault(cnd => cnd.OPT_Code == orderPackageTypeCode && cnd.OPT_IsDeleted == false).OPT_ID;
                }
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();

                if (paymentOption == PaymentOptions.Credit_Card.GetStringValue())
                {
                    switch (orderPackageTypeCode)
                    {
                        case "AAAA":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;
                            break;
                        case "AAAB":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_PACKAGES;
                            break;
                        case "AAAC":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;
                            break;
                    }
                    dictMailData = GetCCOrderApprovalMailData(orderPaymentDetail, tenantId, orderPackageTypeCode);
                }
                else
                {
                    switch (orderPackageTypeCode)
                    {
                        case "AAAA":
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                            break;
                        case "AAAB":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            dicMessageParam.Add("EntityID", entityID);
                            dicMessageParam.Add("EntityTypeCode", entityTypeCode);
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_PACKAGES;
                            break;
                        case "AAAC":
                            entityID = orderPackageTypeID;
                            entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                            dicMessageParam.Add("EntityID", entityID);
                            dicMessageParam.Add("EntityTypeCode", entityTypeCode);
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_COMPLIO_PACKAGES;
                            break;
                        default:
                            commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                            break;
                    }
                    /*Commented below code for the implementation UAT-916
                     * if (paymentOption == PaymentOptions.Credit_Card.GetStringValue())
                    {
                        switch (orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code)
                        {
                            case "AAAA":
                                entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                                entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                                commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;
                                break;
                            case "AAAB":
                                entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                                entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                                commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_PACKAGES;
                                break;
                            case "AAAC":
                                entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                                entityTypeCode = CommunicationEntityType.CREDIT_CARD_ORDER_PACAKGE_TYPE.GetStringValue();
                                commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_COMPLIO_PACKAGES;
                                break;
                            default:
                                commSubEvent = CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;
                                break;
                        }
                        dictMailData = GetCCOrderApprovalMailData(orderPaymentDetail, tenantId);
                    }
                    else
                    {
                        switch (orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code)
                        {
                            case "AAAA":
                                commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                                break;
                            case "AAAB":
                                entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                                entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                                dicMessageParam.Add("EntityID", entityID);
                                dicMessageParam.Add("EntityTypeCode", entityTypeCode);
                                commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_PACKAGES;
                                break;
                            case "AAAC":
                                entityID = orderPaymentDetail.Order.lkpOrderPackageType.OPT_ID;
                                entityTypeCode = CommunicationEntityType.ORDER_PACAKGE_TYPE.GetStringValue();
                                dicMessageParam.Add("EntityID", entityID);
                                dicMessageParam.Add("EntityTypeCode", entityTypeCode);
                                commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_COMPLIO_PACKAGES;
                                break;
                            default:
                                commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;
                                break;
                        }*/
                    dictMailData = GetOrderApprovalMailData(orderPaymentDetail, tenantId, orderPackageTypeCode);
                }
                if (dicMessageAttachmentParam != null && dicMessageAttachmentParam.Count > 0)
                {
                    if (dicMessageAttachmentParam.ContainsKey("DocumentName"))
                        dicMessageParam.Add("DocumentName", Convert.ToString(dicMessageAttachmentParam.GetValue("DocumentName")));

                    if (dicMessageAttachmentParam.ContainsKey("IgnoreSpecificTemplate"))
                        dicMessageParam.Add("IgnoreSpecificTemplate", Convert.ToString(dicMessageAttachmentParam.GetValue("IgnoreSpecificTemplate")));
                }
                //SaveMessageContent(CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL, dictMailData, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, tenantId);
                return SaveMessageContent(commSubEvent, dictMailData, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, tenantId, dicMessageParam);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #region UAT-2073

        /// <summary>
        /// Send Mail when oreder created and its payment type is credit card.
        /// </summary>
        /// <param name="applicantOrder"></param>
        /// <param name="orgUserProfile"></param>
        /// <param name="tenantId"></param>
        public static void SendOrderCreationMailForCreditCard(Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, Int32 tenantId)
        {
            try
            {
                String _paymentModeCode = PaymentOptions.Credit_Card.GetStringValue();
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_CREDIT_CARD;
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = GetOrderCreationMoneyOrderMailData(applicantOrder, orgUserProfile, tenantId, _paymentModeCode);

                mockData.UserName = string.Concat(orgUserProfile.FirstName, " ", orgUserProfile.LastName);
                mockData.EmailID = orgUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orgUserProfile.OrganizationUserID;

                SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, applicantOrder.SelectedNodeID.HasValue ? applicantOrder.SelectedNodeID.Value : 0, null);
                //SendPackageNotificationMail(commSubEvent, dictMailData, mockData, tenantId, applicantOrder.SelectedNodeID.HasValue ? applicantOrder.SelectedNodeID.Value : 0, entityID, entityTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        /// <summary>
        /// send internal message when order is careted and its payment type is credit card
        /// </summary>
        /// <param name="applicantOrder"></param>
        /// <param name="orgUserProfile"></param>
        /// <param name="tenantId"></param>
        public static void SendOrderCreationMessageForCreditCard(Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, Int32 tenantId)
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Applicant Order Id: " + (applicantOrder.IsNotNull() ? applicantOrder.OrderID.ToString() : String.Empty) + "";
                parametersPassed += ", Organisation User Profile Id: " + (orgUserProfile.IsNotNull() ? orgUserProfile.OrganizationUserProfileID.ToString() : String.Empty) + "";
                parametersPassed += ", Tenant Id: " + tenantId.ToString() + "";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: SendOrderCreationMessageOnlinePayment with " + parametersPassed + "");

                String _paymentModeCode = PaymentOptions.Credit_Card.GetStringValue();
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_CREDIT_CARD;
                Dictionary<String, object> dictMailData = GetOrderCreationMoneyOrderMailData(applicantOrder, orgUserProfile, tenantId, _paymentModeCode);

                SaveMessageContent(commSubEvent, dictMailData, orgUserProfile.OrganizationUserID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        #endregion

        public static void SendOrderCancellationMail(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = GetOrderCancellationMailData(orderPaymentDetail, tenantId);

                mockData.UserName = string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName);
                mockData.EmailID = orderPaymentDetail.Order.OrganizationUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;
                //Send mail
                //SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION, mockData, dictMailData, tenantId, orderPaymentDetail.Order.HierarchyNodeID.Value, null);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION, mockData, dictMailData, tenantId, orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : 0, null);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderCancellationMessage(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                Dictionary<String, object> dictMailData = GetOrderCancellationMailData(orderPaymentDetail, tenantId);
                SaveMessageContent(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION, dictMailData, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderCancellationApprovedMail(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = GetOrderCancellationApprovedMailData(orderPaymentDetail, tenantId);

                mockData.UserName = string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName);
                mockData.EmailID = orderPaymentDetail.Order.OrganizationUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;
                //Send mail
                //SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_APPROVED, mockData, dictMailData, tenantId, orderPaymentDetail.Order.HierarchyNodeID.Value, null);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_APPROVED, mockData, dictMailData, tenantId, orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : 0, null);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderCancellationApprovedMessage(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                Dictionary<String, object> dictMailData = GetOrderCancellationApprovedMailData(orderPaymentDetail, tenantId);
                SaveMessageContent(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_APPROVED, dictMailData, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderCancellationRejectedMail(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = GetOrderCancellationRejectedMailData(orderPaymentDetail, tenantId);

                mockData.UserName = string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName);
                mockData.EmailID = orderPaymentDetail.Order.OrganizationUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;
                //Send mail
                //SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_REJECTED, mockData, dictMailData, tenantId, orderPaymentDetail.Order.HierarchyNodeID.Value, null);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_REJECTED, mockData, dictMailData, tenantId, orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : 0, null);


            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderCancellationRejectedMessage(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                Dictionary<String, object> dictMailData = GetOrderCancellationRejectedMailData(orderPaymentDetail, tenantId);
                SaveMessageContent(CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_REJECTED, dictMailData, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendRushOrderConfirmationMail(PackageSubscription packageSubscription, int tenantId, int onlinePaymentDetailID)
        {
            try
            {
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                String paymentOption = packageSubscription.Order.OrderPaymentDetails.FirstOrDefault(
                             x => x.OPD_ID == onlinePaymentDetailID).lkpPaymentOption.Code;
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_RUSH_ORDER_CONFIRMATION;

                if (paymentOption == PaymentOptions.Credit_Card.GetStringValue())
                {
                    commSubEvent = CommunicationSubEvents.CREDIT_CARD_RUSH_ORDER_CONFIRMATION;
                    dictMailData = GetCCRushOrderConfirmationMailData(packageSubscription, tenantId, onlinePaymentDetailID);
                }
                else
                {
                    commSubEvent = CommunicationSubEvents.NOTIFICATION_RUSH_ORDER_CONFIRMATION;
                    dictMailData = GetRushOrderConfirmationMailData(packageSubscription, tenantId, onlinePaymentDetailID);
                }

                mockData.UserName = string.Concat(packageSubscription.Order.OrganizationUserProfile.FirstName, " ", packageSubscription.Order.OrganizationUserProfile.LastName);
                mockData.EmailID = packageSubscription.Order.OrganizationUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = packageSubscription.Order.OrganizationUserProfile.OrganizationUserID;
                //Send mail
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                //SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, packageSubscription.Order.HierarchyNodeID.Value, null, null, false);
                SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, packageSubscription.Order.SelectedNodeID.HasValue ? packageSubscription.Order.SelectedNodeID.Value : 0, null, null, false);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendRushOrderConfirmationMessage(PackageSubscription packageSubscription, int tenantId, int onlinePaymentDetailID)
        {
            try
            {
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                String paymentOption = packageSubscription.Order.OrderPaymentDetails.FirstOrDefault(
                             x => x.OPD_ID == onlinePaymentDetailID).lkpPaymentOption.Code;
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_RUSH_ORDER_CONFIRMATION;

                if (paymentOption == PaymentOptions.Credit_Card.GetStringValue())
                {
                    commSubEvent = CommunicationSubEvents.CREDIT_CARD_RUSH_ORDER_CONFIRMATION;
                    dictMailData = GetCCRushOrderConfirmationMailData(packageSubscription, tenantId, onlinePaymentDetailID);
                }
                else
                {
                    commSubEvent = CommunicationSubEvents.NOTIFICATION_RUSH_ORDER_CONFIRMATION;
                    dictMailData = GetRushOrderConfirmationMailData(packageSubscription, tenantId, onlinePaymentDetailID);
                }

                SaveMessageContent(commSubEvent, dictMailData, packageSubscription.Order.OrganizationUserProfile.OrganizationUserID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderRejectionMail(OrderPaymentDetail orderPaymentDetail, int tenantId, Boolean isCompliancePackageInclude)
        {
            try
            {
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = GetOrderRejectionMailData(orderPaymentDetail, tenantId, isCompliancePackageInclude);

                mockData.UserName = string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName);
                mockData.EmailID = orderPaymentDetail.Order.OrganizationUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;
                //Send mail
                //SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_REJECTION, mockData, dictMailData, tenantId, orderPaymentDetail.Order.HierarchyNodeID.Value, null);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ORDER_REJECTION, mockData, dictMailData, tenantId, orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : 0, null);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void SendOrderRejectionMessage(OrderPaymentDetail orderPaymentDetail, int tenantId, Boolean isCompliancePackageInclude)
        {
            try
            {
                Dictionary<String, object> dictMailData = GetOrderRejectionMailData(orderPaymentDetail, tenantId, isCompliancePackageInclude);
                SaveMessageContent(CommunicationSubEvents.NOTIFICATION_ORDER_REJECTION, dictMailData, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        #endregion

        #region Private Methods

        #region Mail and Message Data

        private static Dictionary<String, object> GetOrderCreationMoneyOrderMailData(Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, int tenantId, String _paymentModeCode)
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Applicant Order Id: " + (applicantOrder.IsNotNull() ? applicantOrder.OrderID.ToString() : String.Empty) + "";
                parametersPassed += ", Organisation User Profile Id: " + (orgUserProfile.IsNotNull() ? orgUserProfile.OrganizationUserProfileID.ToString() : String.Empty) + "";
                parametersPassed += ", Tenant Id: " + tenantId.ToString() + "";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: GetOrderCreationMoneyOrderMailData with " + parametersPassed + "");
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderCreationMoneyOrderMailData(applicantOrder, orgUserProfile, tenantId, _paymentModeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetOrderCreationInvoiceMailData(Order applicantOrder, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, int tenantId, String _paymentModeCode)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderCreationInvoiceMailData(applicantOrder, orgUserProfile, tenantId, _paymentModeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetOrderApprovalMailData(OrderPaymentDetail orderPaymentDetail, int tenantId, String orderPackageTypeCode)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderApprovalMailData(orderPaymentDetail, tenantId, orderPackageTypeCode);
            }

            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetCCOrderApprovalMailData(OrderPaymentDetail orderPaymentDetail, int tenantId, String orderPackageTypeCode)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetCCOrderApprovalMailData(orderPaymentDetail, tenantId, orderPackageTypeCode);
            }

            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetOrderCancellationMailData(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderCancellationMailData(orderPaymentDetail, tenantId);
            }

            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetOrderCancellationApprovedMailData(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderCancellationApprovedMailData(orderPaymentDetail, tenantId);
            }

            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetOrderCancellationRejectedMailData(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderCancellationRejectedMailData(orderPaymentDetail, tenantId);
            }

            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetRushOrderConfirmationMailData(PackageSubscription packageSubscription, int tenantId, int onlinePaymentDetailID)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetRushOrderConfirmationMailData(packageSubscription, tenantId, onlinePaymentDetailID);
            }

            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetCCRushOrderConfirmationMailData(PackageSubscription packageSubscription, int tenantId, int onlinePaymentDetailID)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetCCRushOrderConfirmationMailData(packageSubscription, tenantId, onlinePaymentDetailID);
            }

            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        private static Dictionary<String, object> GetOrderRejectionMailData(OrderPaymentDetail orderPaymentDetail, int tenantId, Boolean isCompliancePackageInclude)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderRejectionMailData(orderPaymentDetail, tenantId, isCompliancePackageInclude);
            }

            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        #endregion

        #region Send Message to Queue

        public static void SendMessageContentToQueue(MessagingContract messagingContract)
        {
            try
            {
                BALUtils.GetCommunicationRepoInstance().SendMessageContentToQueue(messagingContract);
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

        //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
        //private static Int32? SaveMailCommunicationContract(CommunicationSubEvents communicationSubEvents, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, Int32 tenantId, Int32 hierarchyNodeID, Int32? entityID, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, List<OrganizationUserContract> lstClientAdmins = null)
        //{
        private static Int32? SaveMailCommunicationContract(CommunicationSubEvents communicationSubEvents, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, Int32 tenantId, Int32 hierarchyNodeID, Int32? entityID, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, Boolean isExternalUserRequired = true, List<CommunicationTemplateContract> lstcommunicationTemplateContract = null, List<ExternalCopyUsersContract> lstCCUsers = null, String rotationHierarchyID = null, Int32? rotationID = AppConsts.NONE, Boolean IsEnroller = false)
        {

            Int32? systemCommunicationID = null;
            Entity.AppConfiguration appConfiguration = communicationSubEvents.GetStringValue() == CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED.GetStringValue() ?
                 SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID) : SecurityManager.GetAppConfiguration(AppConsts.SYSTEM_COMMUNICATION_USER_ID);
            Int32 systemCommunicationUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
            if (appConfiguration.IsNotNull())
            {
                systemCommunicationUserId = Convert.ToInt32(appConfiguration.AC_Value);
            }

            ISubject subject = new Subject();

            CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
            communicationTemplateContract.CurrentUserId = systemCommunicationUserId;
            communicationTemplateContract.SenderName = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME]);
            communicationTemplateContract.SenderEmailID = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
            communicationTemplateContract.RecieverName = mockData.UserName;
            communicationTemplateContract.RecieverEmailID = mockData.EmailID;
            communicationTemplateContract.ReceiverOrganizationUserId = mockData.ReceiverOrganizationUserID;
            //UAT-1578:WB: Addition of SMS notification
            if (communicationSubEvents != CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS
                && communicationSubEvents != CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION)
            {
                SaveDataForSMSNotification(communicationSubEvents, mockData, dictMailData, tenantId, hierarchyNodeID, IsEnroller);
            }

            subject.Attach(
               new Observer<object>(subject, new { dictionary = dictMailData, details = communicationTemplateContract }, (theSubject, theElement) =>
               {
                   //Send mail
                   //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons(Remove lstClientAdmin)
                   systemCommunicationID = SaveMailContentBasisOnSubscription(communicationSubEvents, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, hierarchyNodeID, tenantId, entityID, entityTypeCode, ignoreSubscriptionSeting, 0, 0, overrideCcBcc, lstBackgroundOrderDailyReport, isExternalUserRequired, lstcommunicationTemplateContract, lstCCUsers, rotationHierarchyID, false, rotationID);

               }));
            subject.Notify();
            return systemCommunicationID;
        }

        public static MessagingContract GetMessageContract(String communicationSubEventCode, SystemCommunication systemCommunication, int applicantId, Int32 communicationSubEventId, Int32 tenantId)
        {
            try
            {
                Entity.AppConfiguration appConfiguration = communicationSubEventCode == CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED.GetStringValue() ?
                    SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID) : SecurityManager.GetAppConfiguration(AppConsts.SYSTEM_COMMUNICATION_USER_ID);
                Int32 systemCommunicationUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    systemCommunicationUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }

                String key = communicationSubEventId.ToString() + "_" + tenantId.ToString();

                List<Entity.ClientEntity.CommunicationCCUsersList> defaultCommunicationCCusers = new List<Entity.ClientEntity.CommunicationCCUsersList>();
                if (System.Web.HttpContext.Current != null)
                {
                    if (System.Web.HttpContext.Current.Items[key] != null)
                    {
                        defaultCommunicationCCusers = (List<Entity.ClientEntity.CommunicationCCUsersList>)System.Web.HttpContext.Current.Items[key];
                    }
                    else
                    {
                        defaultCommunicationCCusers = ComplianceSetupManager.GetCCusers(communicationSubEventId, tenantId, null);
                        if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        {
                            System.Web.HttpContext.Current.Items.Add(key, defaultCommunicationCCusers);
                        }
                    }
                }
                else if (ServiceContext.Current != null)
                {
                    if (ServiceContext.Current.DataDict != null && ServiceContext.Current.DataDict.ContainsKey(key))
                    {
                        defaultCommunicationCCusers = (List<Entity.ClientEntity.CommunicationCCUsersList>)ServiceContext.Current.DataDict.GetValue(key);
                    }
                    else
                    {
                        defaultCommunicationCCusers = ComplianceSetupManager.GetCCusers(communicationSubEventId, tenantId, null);
                        if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        {
                            if (ServiceContext.Current.DataDict == null)
                                ServiceContext.Current.DataDict = new Dictionary<string, object>();
                            ServiceContext.Current.DataDict.Add(key, defaultCommunicationCCusers);
                        }
                    }
                }
                else if (ParallelTaskContext.Current != null)
                {
                    if (ParallelTaskContext.Current.DataDict != null && ParallelTaskContext.Current.DataDict.ContainsKey(key))
                    {
                        defaultCommunicationCCusers = (List<Entity.ClientEntity.CommunicationCCUsersList>)ParallelTaskContext.Current.DataDict.GetValue(key);
                    }
                    else
                    {
                        defaultCommunicationCCusers = ComplianceSetupManager.GetCCusers(communicationSubEventId, tenantId, null);
                        if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        {
                            if (ParallelTaskContext.Current.DataDict == null)
                                ParallelTaskContext.Current.DataDict = new Dictionary<string, object>();
                            ParallelTaskContext.Current.DataDict.Add(key, defaultCommunicationCCusers);
                        }
                    }
                }

                //changes done regarding UAt-
                String defaultCCUserIds = String.Empty;
                String defaultCCuserEmailIds = String.Empty;
                String defaultBCCUserIds = String.Empty;
                String defaultBCCuserEmailIds = String.Empty;
                if (!defaultCommunicationCCusers.IsNullOrEmpty() && defaultCommunicationCCusers.Count > AppConsts.NONE)
                {
                    defaultCommunicationCCusers = defaultCommunicationCCusers.Where(condition => condition.IsCommunicationCentre).ToList();
                    String ccCode = CopyType.CC.GetStringValue();
                    String bccCode = CopyType.BCC.GetStringValue();

                    defaultCCUserIds = String.Join(";", defaultCommunicationCCusers.Where(condition => condition.CopyCode == ccCode).Select(x => x.UserID));
                    defaultCCuserEmailIds = String.Join(";", defaultCommunicationCCusers.Where(condition => condition.CopyCode == ccCode).Select(x => x.EmailAddress));

                    defaultBCCUserIds = String.Join(";", defaultCommunicationCCusers.Where(condition => condition.CopyCode == bccCode).Select(x => x.UserID));
                    defaultBCCuserEmailIds = String.Join(";", defaultCommunicationCCusers.Where(condition => condition.CopyCode == bccCode).Select(x => x.EmailAddress));
                }
                MessagingContract messagingContract = new MessagingContract();
                messagingContract.Action = MessagingAction.NewMail;
                messagingContract.ApplicationDatabaseName = MessageManager.GetDatabaseName(ConfigurationManager.ConnectionStrings[AppConsts.APPLICATION_CONNECTION_STRING].ConnectionString);
                messagingContract.CommunicationType = "CT01";
                messagingContract.FolderId = MessageManager.GetFolderIdByCode(lkpMessageFolderContext.INBOX.GetStringValue());
                messagingContract.Content = systemCommunication.Content.IsNotNull() ? systemCommunication.Content.Replace("<p>", "<p style=\"margin-bottom:10px;\">") : String.Empty;
                messagingContract.CurrentUserId = systemCommunicationUserId;
                if (applicantId != AppConsts.BACKGROUND_PROCESS_USER_VALUE)
                {
                    messagingContract.ToList = MessageManager.GetEmailId(applicantId);
                    messagingContract.ToUserIds = applicantId.ToString();
                }
                else
                {
                    messagingContract.ToList = String.Join(";", defaultCommunicationCCusers.Where(condition => String.IsNullOrEmpty(condition.CopyCode)).Select(x => x.EmailAddress));
                    messagingContract.ToUserIds = String.Join(";", defaultCommunicationCCusers.Where(condition => String.IsNullOrEmpty(condition.CopyCode)).Select(x => x.UserID));
                }

                //else
                //{
                //    messagingContract.ToList = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                //    messagingContract.ToUserIds = applicantId.ToString();
                //}
                messagingContract.From = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]); ;
                messagingContract.IsHighImportance = false;
                messagingContract.MessageMode = "S";
                messagingContract.Subject = systemCommunication.Subject;

                if (!String.IsNullOrEmpty(defaultCCUserIds))
                {
                    messagingContract.CcList = defaultCCuserEmailIds;
                    messagingContract.CcUserIds = defaultCCUserIds;
                }
                if (!String.IsNullOrEmpty(defaultBCCUserIds))
                {
                    messagingContract.BCcList = defaultBCCuserEmailIds;
                    messagingContract.BccUserIds = defaultBCCUserIds;
                }
                return messagingContract;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calls the communication repository to Save the content of the Email to be sent, based on the communication sub event received
        /// </summary>
        /// <param name="subEventId">Id of the event for which the email content is to be generated.</param>
        private static SystemCommunication PrepareMessageContent(String communicationSubEventCode, Dictionary<String, Object> dicContent, Int32? entityID = null, String entityTypeCode = null)
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Communication Sub Event: " + communicationSubEventCode + "";
                if (dicContent.IsNotNull())
                    parametersPassed += ", Dictionary Count: " + dicContent.Count.ToString() + "";
                else
                    parametersPassed += ", Dictionary Obejct passed is NULL";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: PrepareMessageContent with " + parametersPassed + "");

                Int32 subEventId = GetCommunicationTypeSubEvents(communicationSubEventCode).CommunicationSubEventID;
                CommunicationTemplate communicationTemplate = new CommunicationTemplate();
                List<CommunicationTemplatePlaceHolder> placeHoldersToFetch = null;

                Int32? tenantId = null, categoryId = null, itemId = null, nodeMappingId = null;
                List<Int32> templateId = new List<Int32>(); Int32? receiverOrgUserId = null;
                Int32? agencyHierarchyId = null; //UAT-3704

                //UAT-3656
                Boolean isNotificationBlocked = false;

                if (dicContent.ContainsKey(EmailFieldConstants.TENANT_ID))
                    tenantId = Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.TENANT_ID));
                if (dicContent.ContainsKey(EmailFieldConstants.COMPLIANCE_CATEGORY_ID))
                    categoryId = Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.COMPLIANCE_CATEGORY_ID));
                if (dicContent.ContainsKey(EmailFieldConstants.COMPLIANCE_ITEM_ID))
                    itemId = Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.COMPLIANCE_ITEM_ID));
                if (dicContent.ContainsKey(EmailFieldConstants.SES_NODE_NOTIFICATION_MAPPING_ID))
                    nodeMappingId = Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.SES_NODE_NOTIFICATION_MAPPING_ID));
                //UAT-3704
                if (dicContent.ContainsKey(EmailFieldConstants.AGENCY_HIERARCHY_ID))
                    agencyHierarchyId = Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.AGENCY_HIERARCHY_ID));
                //End UAT-3704
                //UAT-3824
                Int32 LanguageID = 0;
                Int32 DefaultLanguageID = 0;
                //// get receiver organisation user id for getting users's preferred language
                if (dicContent.ContainsKey(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID))
                    receiverOrgUserId = Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID));

                //get user language id here, if user prefered language is specified than get that language id otherwise get default(english) language id  
                LanguageID = BALUtils.GetSecurityRepoInstance().GetLanguageIdByGuid(receiverOrgUserId, tenantId, out DefaultLanguageID);


                if (entityID != null && !String.IsNullOrEmpty(entityTypeCode))
                {
                    templateId = BALUtils.GetCommunicationRepoInstance().GetCommunicationTemplateIDByEntity(entityID.Value, entityTypeCode, communicationSubEventCode);
                }
                else
                {
                    List<SystemEventSetting> lstSysTemEventSettings = GetSystemEventSetting(subEventId);
                    List<SystemEventSetting> systemEventSetting = new List<SystemEventSetting>();
                    //UAT-3704 // Preference to agency hierarchy specific templates.
                    if (!agencyHierarchyId.IsNullOrEmpty() && agencyHierarchyId > AppConsts.NONE)
                    {
                        systemEventSetting = lstSysTemEventSettings.Where(condition => condition.AgencyHierarchyID == agencyHierarchyId && condition.IsDeleted != true).ToList();
                        if (systemEventSetting.IsNotNull() && systemEventSetting.Count > AppConsts.NONE)
                        {
                            templateId = systemEventSetting.Select(x => x.CommunicationTemplateID).ToList();
                            isNotificationBlocked = systemEventSetting.Any(x => x.IsNotificationBlocked);
                        }
                    }
                    //end UAT-3704

                    else
                    {
                        systemEventSetting = lstSysTemEventSettings.Where(condition => condition.TenantID == tenantId && condition.ComplianceCategoryID == categoryId
                            && condition.ComplianceItemID == itemId && condition.SES_NodeNotificationMappingID == nodeMappingId).ToList();
                        if (systemEventSetting.IsNotNull() && systemEventSetting.Count > AppConsts.NONE)
                        {
                            templateId = systemEventSetting.Select(x => x.CommunicationTemplateID).ToList();
                            isNotificationBlocked = systemEventSetting.Any(x => x.IsNotificationBlocked);
                        }
                    }
                    //UAT-3656
                    if (isNotificationBlocked)
                        return null;

                    if (templateId.IsNullOrEmpty() && templateId.Count == AppConsts.NONE && categoryId.IsNotNull() && categoryId > AppConsts.NONE)
                    {
                        List<SystemEventSetting> catSystemEventSetting = lstSysTemEventSettings.Where(condition => condition.TenantID == tenantId && condition.ComplianceCategoryID == categoryId
                       && condition.ComplianceItemID.IsNull()).ToList();
                        if (catSystemEventSetting.IsNotNull())
                        {
                            templateId = catSystemEventSetting.Select(x => x.CommunicationTemplateID).ToList();
                        }
                    }
                }

                if (templateId.Count > AppConsts.NONE)
                {
                    //get user language specified template
                    var commTemplate = GetCommunicationTemplate(subEventId).Where(cTemplate => templateId.Contains(cTemplate.CommunicationTemplateID)).OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).ToList();
                    if (!commTemplate.IsNullOrEmpty() && commTemplate.Count > 0)
                    {
                        if (commTemplate.Any(x => x.LanguageId == LanguageID))
                        {
                            communicationTemplate = commTemplate.Where(x => x.LanguageId == LanguageID).FirstOrDefault();
                        }
                        else
                        {
                            communicationTemplate = commTemplate.Where(x => x.LanguageId == DefaultLanguageID).FirstOrDefault();
                        }
                    }
                    //communicationTemplate = GetCommunicationTemplate(subEventId).Where(cTemplate => templateId.Contains(cTemplate.CommunicationTemplateID) && (cTemplate.LanguageId == LanguageID || cTemplate.LanguageId == DefaultLanguageID)).
                    //OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();
                }
                else
                {
                    //get user language specified template
                    var commTemplate = GetCommunicationTemplate(subEventId).Where(cTemplate => !cTemplate.SystemEventSettings.Any()).OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).ToList();
                    if (!commTemplate.IsNullOrEmpty() && commTemplate.Count > 0)
                    {
                        if (commTemplate.Any(x => x.LanguageId == LanguageID))
                        {
                            communicationTemplate = commTemplate.Where(x => x.LanguageId == LanguageID).FirstOrDefault();
                        }
                        else
                        {
                            communicationTemplate = commTemplate.Where(x => x.LanguageId == DefaultLanguageID).FirstOrDefault();
                        }
                    }
                    //communicationTemplate = GetCommunicationTemplate(subEventId).Where(cTemplate => !cTemplate.SystemEventSettings.Any() && (cTemplate.LanguageId == LanguageID || cTemplate.LanguageId == DefaultLanguageID)).
                    //    OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();

                    //communicationTemplate = GetCommunicationTemplate(subEventId).OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();
                }

                if (communicationTemplate.IsNotNull())
                {
                    placeHoldersToFetch = GetTemplatePlaceHolders(subEventId);
                }
                return BALUtils.GetCommunicationRepoInstance().PrepareMessageContent(dicContent, communicationTemplate, placeHoldersToFetch);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }

        }

        /// <summary>
        /// Calls the communication repository to Save the content of the Email to be sent, based on the communication sub event received
        /// </summary>
        /// <param name="subEventId">Id of the event for which the email content is to be generated.</param>
        //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
        //private static Int32 SaveMailContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 CommunicationSubEventID, Int32 tenantId, Int32 hierarchyNodeID, CommunicationSubEvents communicationSubEvents, List<OrganizationUserContract> lstClientAdmins = null)
        //{
        private static Int32 SaveMailContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 CommunicationSubEventID, Int32 tenantId, Int32 hierarchyNodeID, CommunicationSubEvents communicationSubEvents, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, Boolean isExternalUserRequired = true, List<CommunicationTemplateContract> lstcommunicationTemplateContract = null, List<ExternalCopyUsersContract> lstCCUsers = null, String rotationHierarchyID = null, Int32? rotationID = AppConsts.NONE)
        {
            try
            {
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //UAT-807 Addition of a flagged report only notification
                List<ExternalCopyUsersContract> externalCopyUsers = new List<ExternalCopyUsersContract>();
                if (communicationSubEvents != CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS
                    && communicationSubEvents != CommunicationSubEvents.NOTIFICATION_FOR_FLAGGED_ORDER_RESULTS
                    && communicationSubEvents != CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS
                    && isExternalUserRequired)
                    externalCopyUsers = GetExternalUserContactDetail(CommunicationSubEventID, hierarchyNodeID, tenantId);

                Int32 rotationCreatedByID = rotationID.HasValue && tenantId > AppConsts.NONE && rotationID > AppConsts.NONE ? BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationCreatorByRotationID(rotationID.Value) : AppConsts.NONE;
                return BALUtils.GetCommunicationRepoInstance().SaveMailContent(systemCommunication, communicationTemplateContract, CommunicationSubEventID, tenantId, hierarchyNodeID, externalCopyUsers, communicationSubEvents, overrideCcBcc, lstBackgroundOrderDailyReport, lstcommunicationTemplateContract, lstCCUsers, rotationHierarchyID, rotationID, rotationCreatedByID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Calls the communication repository to Save the content of the Email to be sent, based on the communication sub event received
        /// </summary>
        /// <param name="subEventId">Id of the event for which the email content is to be generated.</param>
        public static SystemCommunication PrepareMessageContentForSystemMails(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, Int32 tenantId, out Int32 communicationSubEventId, bool? isPasswordToBeMask = null) //UAT-4570 
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Communication Sub Event: " + (communicationSubEvent.IsNotNull() ? communicationSubEvent.ToString() : String.Empty) + "";
                if (dicContent.IsNotNull())
                    parametersPassed += ", Dictionary Count: " + dicContent.Count.ToString() + "";
                else
                    parametersPassed += ", Dictionary Obejct passed is NULL";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: PrepareMessageContent with " + parametersPassed + "");
                String subEventCode = communicationSubEvent.GetStringValue();
                Int32 subEventId = GetCommunicationTypeSubEvents(subEventCode).CommunicationSubEventID;
                CommunicationTemplate communicationTemplate = new CommunicationTemplate();
                List<CommunicationTemplatePlaceHolder> placeHoldersToFetch = null;
                //UAT-3601
                Int32? categoryId = null, itemId = null, nodeMappingId = null;
                List<Int32> templateId = new List<Int32>();
                Int32 LanguageID = 0;
                Int32 DefaultLanguageID = 0;
                Int32? receiverOrgUserId = null;
                //if (tenantId > AppConsts.NONE && !subEventCode.IsNullOrEmpty())
                //{
                //int     templateId = BALUtils.GetCommunicationRepoInstance().GetCommunicationTemplateIDByEntity(tenantId, null, subEventCode);
                //}
                if (dicContent.ContainsKey(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID))
                    receiverOrgUserId = Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID));

                LanguageID = BALUtils.GetSecurityRepoInstance().GetLanguageIdByGuid(receiverOrgUserId, tenantId, out DefaultLanguageID);


                List<SystemEventSetting> lstSysTemEventSettings = GetSystemEventSetting(subEventId);
                List<SystemEventSetting> systemEventSetting = lstSysTemEventSettings.Where(condition => condition.TenantID == tenantId && condition.ComplianceCategoryID == categoryId
                    && condition.ComplianceItemID == itemId && condition.SES_NodeNotificationMappingID == nodeMappingId).ToList();
                if (systemEventSetting.IsNotNull() && systemEventSetting.Count > AppConsts.NONE)
                {
                    templateId = systemEventSetting.Select(x => x.CommunicationTemplateID).ToList();
                }

                if (templateId.Count > AppConsts.NONE)
                {
                    communicationTemplate = GetCommunicationTemplate(subEventId).Where(cTemplate => templateId.Contains(cTemplate.CommunicationTemplateID) && (cTemplate.LanguageId == LanguageID || cTemplate.LanguageId == DefaultLanguageID)).
                        OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();
                    string applicationUrl = dicContent.GetValue(EmailFieldConstants.APPLICATION_URL).ToString();
                    applicationUrl = applicationUrl.Split('?')[0];
                    dicContent.Remove(EmailFieldConstants.APPLICATION_URL);
                    dicContent.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                }
                else
                {
                    communicationTemplate = GetCommunicationTemplate(subEventId).Where(cTemplate => !cTemplate.SystemEventSettings.Any() && (cTemplate.LanguageId == LanguageID || cTemplate.LanguageId == DefaultLanguageID)).
                        OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();

                }
                //communicationTemplate = GetCommunicationTemplate(subEventId).Where(cTemplate => !cTemplate.SystemEventSettings.Any()).
                //    OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();

                if (communicationTemplate.IsNotNull())
                {
                    placeHoldersToFetch = GetTemplatePlaceHolders(subEventId);
                }
                communicationSubEventId = subEventId;

                return BALUtils.GetCommunicationRepoInstance().PrepareMessageContentForSystemMails(communicationSubEvent, dicContent, communicationTemplate, placeHoldersToFetch, isPasswordToBeMask); //UAT-4570
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }

        }

        #endregion

        #region Mail and Message

        /// <summary>
        /// Send Notification Email
        /// </summary>
        /// <param name="communicationSubEvents"></param>
        /// <param name="dictMailData"></param>
        /// <param name="mockData"></param>
        /// <param name="currentLoggedInUserId"></param>
        //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
        //public static Int32? SendPackageNotificationMail(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, CommunicationMockUpData mockData, Int32 tenantId, Int32 hierarchyNodeID, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, List<OrganizationUserContract> lstClientAdmins = null)
        //{
        public static Int32? SendPackageNotificationMail(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, CommunicationMockUpData mockData, Int32 tenantId, Int32 hierarchyNodeID, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Boolean overrideCcBcc = false, List<ExternalCopyUsersContract> lstCCUsers = null, String rotationHierarchyID = null, Int32? rotationID = AppConsts.NONE)
        {
            try
            {
                return SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, tenantId, hierarchyNodeID, entityID, entityTypeCode, ignoreSubscriptionSeting, overrideCcBcc, null, true, null, lstCCUsers, rotationHierarchyID, rotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Send Internal Message Mail
        /// </summary>
        /// <param name="toUserIds"></param>
        /// <param name="ccUserIds"></param>
        public static void SendInternalMessageMail(string toUserIds, string ccUserIds = null, string bccUserIds = null)
        {
            try
            {
                String[] mailToUsers = null;
                String[] mailCcUsers = null;
                String[] mailBccUsers = null;
                if (toUserIds.IsNotNull())
                {
                    mailToUsers = toUserIds.Split(';');
                }
                if (ccUserIds.IsNotNull())
                {
                    mailCcUsers = ccUserIds.Split(';');
                }
                if (bccUserIds.IsNotNull())
                {
                    mailBccUsers = ccUserIds.Split(';');
                }
                List<Int32> lstUserIds = new List<int>();
                String[] finalList = new String[(mailToUsers.IsNotNull() ? mailToUsers.Length : 0) + (mailCcUsers.IsNotNull() ? mailCcUsers.Length : 0) + (mailBccUsers.IsNotNull() ? mailBccUsers.Length : 0)];
                if (mailToUsers.IsNotNull())
                {
                    Array.Copy(mailToUsers, finalList, mailToUsers.Length);
                }
                if (mailCcUsers.IsNotNull())
                {
                    Array.Copy(mailCcUsers, 0, finalList, mailToUsers.IsNotNull() ? mailToUsers.Length : 0, mailCcUsers.Length);
                }
                if (mailBccUsers.IsNotNull())
                {
                    Array.Copy(mailBccUsers, 0, finalList, mailToUsers.IsNotNull() ? mailToUsers.Length : 0, mailBccUsers.Length);
                }
                for (int i = 0; i < finalList.Length; i++)
                {
                    if (!finalList[i].IsNullOrEmpty())
                        lstUserIds.Add(Convert.ToInt32(finalList[i]));
                }
                List<Entity.OrganizationUser> lstOrganizationUsers = MessageManager.GetOrganizationUsers(lstUserIds);
                for (int users = 0; users < lstOrganizationUsers.Count; users++)
                {
                    String messageToEmail = lstOrganizationUsers.Where(orgUser => orgUser.OrganizationUserID == Convert.ToInt32(lstUserIds[users])).FirstOrDefault().aspnet_Users.aspnet_Membership.Email;
                    String messageToName = string.Concat(lstOrganizationUsers.Where(orgUser => orgUser.OrganizationUserID == Convert.ToInt32(lstUserIds[users])).FirstOrDefault().FirstName, " ", lstOrganizationUsers.Where(orgUser => orgUser.OrganizationUserID == Convert.ToInt32(lstUserIds[users])).FirstOrDefault().LastName);

                    if (!lstUserIds[users].IsNullOrEmpty())
                    {
                        Dictionary<String, Object> dictMailData = new Dictionary<String, Object>();
                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                        int tenantId = SecurityManager.GetOrganizationUser(lstUserIds[users]).Organization.TenantID.Value;

                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, messageToName);
                        dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, SecurityManager.GetOrganizationUser(lstUserIds[users]).Organization.Tenant.TenantName);
                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, WebSiteManager.GetInstitutionUrl(tenantId));

                        mockData.EmailID = messageToEmail;
                        mockData.UserName = messageToName;
                        mockData.ReceiverOrganizationUserID = Convert.ToInt32(lstUserIds[users]);
                        //Send mail
                        SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_INTERNAL_MESSAGES, mockData, dictMailData, tenantId, 0, null);
                    }
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static bool SetDispatchedTrue(List<SystemCommunicationDelivery> systemCommunicationDeliveries, Int32 userId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().SetDispatchedTrue(systemCommunicationDeliveries, userId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Guid? SaveMessageContent(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, Int32 applicantId, Int32 tenantId, Dictionary<String, object> dicMessageParam = null)
        {
            String communicationSubeventCode = communicationSubEvent.IsNotNull() ? communicationSubEvent.GetStringValue() : String.Empty;
            Guid? messageID = null;
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Communication Sub Event: " + communicationSubeventCode + "";
                parametersPassed += ", Applicant Id: " + applicantId.ToString() + "";
                if (dicContent.IsNotNull())
                    parametersPassed += ", Dictionary Count: " + dicContent.Count.ToString() + "";
                else
                    parametersPassed += ", Dictionary Obejct passed is NULL";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: SaveMessageContent with " + parametersPassed + "");

                Int32? entityID = null;
                String entityTypeCode = null;
                String documentName = String.Empty;
                //Added this check to send attachment for template whic are not specificaly defined for entity.
                Boolean ignoreSpecificTemplate = false;

                lkpCommunicationSubEvent communicationSubEventInDb = new lkpCommunicationSubEvent();
                if (dicMessageParam != null && dicMessageParam.Count > 0)
                {
                    if (dicMessageParam.ContainsKey("EntityID"))
                        entityID = Convert.ToInt32(dicMessageParam.GetValue("EntityID"));
                    if (dicMessageParam.ContainsKey("EntityTypeCode"))
                        entityTypeCode = Convert.ToString(dicMessageParam.GetValue("EntityTypeCode"));
                    if (dicMessageParam.ContainsKey("DocumentName"))
                        documentName = Convert.ToString(dicMessageParam.GetValue("DocumentName"));

                    if (dicMessageParam.ContainsKey("IgnoreSpecificTemplate"))
                        ignoreSpecificTemplate = Convert.ToBoolean(dicMessageParam.GetValue("IgnoreSpecificTemplate"));

                    if (ignoreSpecificTemplate)
                    {
                        communicationSubEventInDb = GetCommunicationTypeSubEvents(communicationSubeventCode);
                    }
                    else
                    {
                        communicationSubEventInDb = GetCommSubEventByEntity(entityID.Value, entityTypeCode, communicationSubeventCode);
                        if (communicationSubEventInDb.IsNotNull())
                        {
                            communicationSubeventCode = communicationSubEventInDb.Code;
                        }
                    }
                }
                else
                {
                    communicationSubEventInDb = GetCommunicationTypeSubEvents(communicationSubeventCode);
                }

                if (communicationSubEventInDb != null)
                {
                    SystemCommunication systemCommunication = PrepareMessageContent(communicationSubeventCode, dicContent, entityID, entityTypeCode);
                    if (systemCommunication.IsNotNull())
                    {
                        MessagingContract messagingContract = GetMessageContract(communicationSubeventCode, systemCommunication, applicantId, communicationSubEventInDb.CommunicationSubEventID, tenantId);
                        if (!String.IsNullOrEmpty(documentName))
                            messagingContract.DocumentName = documentName;

                        SendMessageContentToQueue(messagingContract);

                        messageID = messagingContract.MessageId;
                    }

                    //return true;
                }
                //return false;
                return messageID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //public static Boolean SaveMailContentBasisOnSubscription(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, CommunicationTemplateContract communicationTemplateContract, Int32 hierarchyNodeID, Int32 tenantId = 0, Int32? serviceAtachedFormID = null, out Int32 systemCommunicationID, Int32 systemCommunicationDeliveryId = 0, Int32 currentUserId = 0)
        //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
        //public static Int32? SaveMailContentBasisOnSubscription(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, CommunicationTemplateContract communicationTemplateContract, Int32 hierarchyNodeID, Int32 tenantId = 0, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, List<OrganizationUserContract> lstClientAdmins = null, Int32 systemCommunicationDeliveryId = 0, Int32 currentUserId = 0)
        //{
        public static Int32? SaveMailContentBasisOnSubscription(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, CommunicationTemplateContract communicationTemplateContract, Int32 hierarchyNodeID, Int32 tenantId = 0, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Int32 systemCommunicationDeliveryId = 0, Int32 currentUserId = 0, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, Boolean isExternalUserRequired = true, List<CommunicationTemplateContract> lstcommunicationTemplateContract = null, List<ExternalCopyUsersContract> lstCCUsers = null, String rotationHierarchyID = null, Boolean isAttachment = false, Int32? rotationID = AppConsts.NONE)
        {
            Int32? systemCommunicationID = null;
            String parametersPassed = String.Empty;
            String communicationSubeventCode = communicationSubEvent.IsNotNull() ? communicationSubEvent.GetStringValue() : String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Communication Sub Event: " + communicationSubeventCode + "";
                parametersPassed += ", System Communication Delivery Id: " + systemCommunicationDeliveryId.ToString() + "";
                parametersPassed += ", Current User Id: " + currentUserId.ToString() + "";
                if (dicContent.IsNotNull())
                    parametersPassed += ", Dictionary Count: " + dicContent.Count.ToString() + "";
                else
                    parametersPassed += ", Dictionary Obejct passed is NULL";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: SaveMailContentBasisOnSubscription with " + parametersPassed + "");

                lkpCommunicationSubEvent communicationSubEventInDb = new lkpCommunicationSubEvent();

                if (entityID != null && !String.IsNullOrEmpty(entityTypeCode))
                {
                    communicationSubEventInDb = GetCommSubEventByEntity(entityID.Value, entityTypeCode, communicationSubeventCode);
                    if (communicationSubEventInDb.IsNotNull())
                    {
                        communicationSubeventCode = communicationSubEventInDb.Code;
                    }
                }
                else if (communicationSubEvent == CommunicationSubEvents.NONE)
                {
                    lkpCommunicationSubEvent lkpSubEvent = GetCommunicationTypeSubEvents(Convert.ToInt32(communicationTemplateContract.CommunicationSubEventID));//BALUtils.GetCommunicationRepoInstance().GetCommunicationTypeSubEvents(Convert.ToInt32(communicationTemplateContract.CommunicationSubEventID));
                    communicationSubEventInDb = lkpSubEvent;
                }
                else
                {
                    communicationSubEventInDb = GetCommunicationTypeSubEvents(communicationSubeventCode);
                }
                if (communicationSubEventInDb != null)
                {
                    UserCommunicationSubscriptionSetting userCommunicationSubscriptionSetting = GetUserCommunicationSubscriptionSettings(communicationTemplateContract.ReceiverOrganizationUserId, communicationSubEventInDb.CommunicationTypeID, communicationSubEventInDb.CommunicationEventID);
                    if ((ignoreSubscriptionSeting || (userCommunicationSubscriptionSetting != null
                        && userCommunicationSubscriptionSetting.IsSubscribedToAdmin
                        && userCommunicationSubscriptionSetting.IsSubscribedToUser) || (communicationTemplateContract.ReceiverOrganizationUserId == AppConsts.BACKGROUND_PROCESS_USER_VALUE))
                       || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION.GetStringValue())
                       || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_FAILED_APPLICANT_DOCUMENT_MERGING.GetStringValue())//UAT-2628
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_SUBMITTED_ITEM_INTO_REQUIREMENT_VERIFICATION_QUEUE.GetStringValue()) //UAT-2905
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_ITEM_PAYMENT.GetStringValue()) //UAT-3077
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_BADGE_FORM_ATTACHMENT.GetStringValue()) //UAT-3077
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_ALUMNI_ACCESS_DUE.GetStringValue())//UAT-2960
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_ALUMNI_PACKAGE_AVALIABILITY.GetStringValue()//UAT-2960
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_TO_AGENCY_USER_FOR_E_SIGN_COMPLETED_DOCUMENT.GetStringValue())
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_PENDING_RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS.GetStringValue())
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_IN_PROCESS_AGENCY_STATUS.GetStringValue())//4613
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_To_Agency_User_Upon_Student_Fall_Out_Of_Compliance_Tracking_Package.GetStringValue())//UAT-4400
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_To_Agency_User_Upon_Student_Fall_Out_Of_Compliance.GetStringValue())//UAT-4400
                        )//UAT-3820
                        || (communicationSubEventInDb.Code == CommunicationSubEvents.NOTIFICATION_FOR_FINGER_PRINTING_EXCEEDED_TAT.GetStringValue()) //UAT 4710
                        )
                    {
                        if (systemCommunicationDeliveryId == 0)
                        {
                            SystemCommunication systemCommunication = PrepareMessageContent(communicationSubeventCode, dicContent, entityID, entityTypeCode);
                            if (systemCommunication.IsNotNull())
                            {
                                //Send mail
                                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                                systemCommunicationID = SaveMailContent(systemCommunication, communicationTemplateContract, communicationSubEventInDb.CommunicationSubEventID, tenantId, hierarchyNodeID, communicationSubEvent, overrideCcBcc, lstBackgroundOrderDailyReport, isExternalUserRequired, lstcommunicationTemplateContract, lstCCUsers, rotationHierarchyID, rotationID);
                            }
                        }
                        else
                            BALUtils.GetCommunicationRepoInstance().ReSendEmail(systemCommunicationDeliveryId, currentUserId, isAttachment);
                        //return true;
                    }
                }
                //return false;
                return systemCommunicationID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// To Get Communication Sub Event by EntityID, EntityType and CommunicationSubEvent code.
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="communicationSubEventCode"></param>
        /// <returns>lkpCommunicationSubEvent</returns>
        public static lkpCommunicationSubEvent GetCommSubEventByEntity(Int32 entityID, String entityTypeCode, String communicationSubEventCode)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommSubEventByEntity(entityID, entityTypeCode, communicationSubEventCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdOn"></param>
        /// <returns></returns>
        //public static IEnumerable<SystemCommunicationDelivery> GetSystemCommunicationDelivery(DateTime createdOn)
        public static List<SystemCommunication> GetSystemCommunications(DateTime createdOn, Int32 maxRetryCount, Int32 EmailChunkSize)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetSystemCommunications(createdOn, maxRetryCount, EmailChunkSize);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        #endregion

        #region Communication event

        /// <summary>
        /// Gets the list of the Placeholders
        /// </summary>
        /// <param name="subEevntId">Sube event for which the place hodlers are to be fetched</param>
        /// <returns>List of Place holders of the selected sub-event</returns>
        public static List<CommunicationTemplatePlaceHolder> GetTemplatePlaceHolders(Int32 subEevntId)
        {
            try
            {
                List<Int32> placeHolderIds = GetCommunicationTemplatePlaceHolderSubEvents(subEevntId)
                                            .Select(ctphSubEvents => ctphSubEvents.CommunicationTemplatePlaceHolderID)
                                            .ToList();
                List<CommunicationTemplatePlaceHolder> templatePlaceHolders = GetCommunicationTemplatePlaceHolders()
                                                                              .Where(ctPlaceHolders => placeHolderIds.Contains(ctPlaceHolders.CommunicationTemplatePlaceHolderID))
                                                                              .ToList();
                return templatePlaceHolders;
                //return BALUtils.GetCommunicationRepoInstance().GetTemplatePlaceHolders(templatePlaceHolders);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }


        #endregion

        #region Subscription Settings

        public static IQueryable<vw_ApplicantUser> GetApplicantUsers()
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetApplicantUser();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        #endregion

        #region Send Mail On Profile Change

        public static void SendMailOnProfileChange(Entity.OrganizationUser orgUserNew, Int32 tenantId)
        {
            try
            {
                if (!SecurityManager.IsLocationServiceTenant(tenantId))
                {
                    if (!orgUserNew.IsNullOrEmpty())
                    {
                        //Create Dictionary
                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(orgUserNew.FirstName, " ", orgUserNew.LastName));
                        dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, orgUserNew.Organization.Tenant.TenantName);

                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                        mockData.UserName = string.Concat(orgUserNew.FirstName, " ", orgUserNew.LastName);
                        mockData.EmailID = orgUserNew.PrimaryEmailAddress;
                        mockData.ReceiverOrganizationUserID = orgUserNew.OrganizationUserID;

                        //Send mail
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_PROFILE_CHANGE, dictMailData, mockData, tenantId, 0);

                        //Send Message
                        CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_PROFILE_CHANGE, dictMailData, orgUserNew.OrganizationUserID, tenantId);
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

        #endregion

        #region Communication CC Settings

        public static List<CommunicationCCMaster> GetTenantSpecificCommunicationCCMaster(Int32 tenantID, Int32 DefaultTenantID, Int32 CurrentUserId, String UserType = null)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetTenantSpecificCommunicationCCMaster(tenantID, DefaultTenantID, CurrentUserId, UserType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static IEnumerable<CommunicationCCUser> GetCommunicationCCUser(Int32 communicationCCMasterID, Int32 DefaultTenantID, Int32 CurrentUserId, String UserType = null)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationCCUser(communicationCCMasterID, DefaultTenantID, CurrentUserId, UserType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static List<CommunicationCCUserContract> GetCommunicationCCUserAndSettings(Int32 communicationCCMasterID, Int32 CurrentUserId, String UserType = null)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationCCUserAndSettings(communicationCCMasterID, CurrentUserId, UserType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static Boolean SaveCommunicationCCMaster(CommunicationCCMaster communicationCCMasterObj)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().SaveCommunicationCCMaster(communicationCCMasterObj);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static CommunicationCCMaster GetCommunicationCCMaster(Int32 communicationCCMasterID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationCCMaster(communicationCCMasterID, tenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static Boolean SaveCommunicationCcUsers(List<CommunicationCCUser> communicationCcUserList, Int32 currentUserId, Int32 communicationCCMasterID)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().SaveCommunicationCcUsers(communicationCcUserList, currentUserId, communicationCCMasterID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }
        public static Boolean DeleteCommunicationCcMaster(Int32 selectedTenantId, Int32 communicationCCMasterID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().DeleteCommunicationCcMaster(selectedTenantId, communicationCCMasterID, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static List<lkpCopyType> GetCopyTypes()
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpCopyType>().Where(x => x.CT_IsDeleted == false).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static Boolean IsUserAlreadyMappedToSubEvent(Int32 communicationCCMasterID, Int32 currentUserID, Int32 subEventID)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().IsUserAlreadyMappedToSubEvent(communicationCCMasterID, currentUserID, subEventID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        #endregion

        #region SEND NOTIFICATION ON FIRST ITEM SUBMITT

        /// <summary>
        /// Method to check the notification detail exist on submitt of first item of a subscription.
        /// </summary>
        /// <param name="subEventCode">SubEventCode</param>
        /// <param name="userId">CurrentLoggedInUserId</param>
        /// <returns>Boolean</returns>
        public static Boolean IsFirstItemNotificationExist(String subEventCode, Int32 userId)
        {
            try
            {
                Int32 subEventId = GetCommunicationTypeSubEvents(subEventCode).CommunicationSubEventID;
                return BALUtils.GetCommunicationRepoInstance().IsFirstItemNotificationExist(subEventId, userId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return true;
        }

        #endregion

        #region External User BCC

        /// <summary>
        /// Gets the list of the Sub-Events 
        /// </summary>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        public static List<lkpCommunicationSubEvent> GetCommunicationSubEventsType(List<String> subEventCode)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationSubEventsType(subEventCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static List<ExternalCopyUser> GetBCCExternalUserData(Int32 tenantId, Int32 hierarchyNodeId)
        {
            try
            {
                List<lkpCopyType> copyType = GetCopyTypes();
                Int16 copyTypeId = copyType.Where(obj => obj.CT_Code.Equals(CopyType.BCC.GetStringValue())).FirstOrDefault().CT_Id;
                return BALUtils.GetCommunicationRepoInstance().GetBCCExternalUserData(tenantId, hierarchyNodeId, copyTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static List<lkpSubEventCategoryType> GetSubEventCategoryType()
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpSubEventCategoryType>().ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static Boolean SaveRecordExternalUserBCC(Entity.ExternalCopyUser externalCopyuser)
        {
            try
            {
                List<lkpCopyType> copyType = GetCopyTypes();
                Int16 copyTypeId = copyType.Where(obj => obj.CT_Code.Equals(CopyType.BCC.GetStringValue())).FirstOrDefault().CT_Id;
                externalCopyuser.ECU_CopyTypeID = copyTypeId;
                return BALUtils.GetCommunicationRepoInstance().SaveRecordExternalUserBCC(externalCopyuser);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Boolean UpdateRecordExternalUserBCC(ExternalCopyUserContract externalCopyUserContract, Int32 externalCopyUserID, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().UpdateRecordExternalUserBCC(externalCopyUserContract, externalCopyUserID, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }
        public static Boolean DeleteExternalUserBCC(Int32 externalCopyUserID, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().DeleteExternalUserBCC(externalCopyUserID, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Boolean IsExternalUserExistForSubEvent(ExternalCopyUserContract externalCopyUserContract, Int32 hierarchyNodeId, Int32 tenantId, Int32? externalCopyUserId)
        {
            try
            {
                List<lkpCopyType> copyType = GetCopyTypes();
                Int16 copyTypeId = copyType.Where(obj => obj.CT_Code.Equals(CopyType.BCC.GetStringValue())).FirstOrDefault().CT_Id;
                return BALUtils.GetCommunicationRepoInstance().IsExternalUserExistForSubEvent(externalCopyUserContract, hierarchyNodeId, tenantId, copyTypeId, externalCopyUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        #endregion


        #region Communication

        /// <summary>
        /// Method is used to get the list of subevent based on the tenantId and hierarchyNodeID 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <returns></returns>
        public static List<HierarchyNotificationMapping> GetHierarchyNotificationMappingData(Int32 tenantId, Int32 hierarchyNodeID, Int32 hierarchyContactMappingID)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetHierarchyNotificationMappingData(tenantId, hierarchyNodeID, hierarchyContactMappingID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }


        /// <summary>
        /// Method is used to get the list of SubEvents based on the code + common event
        /// </summary>
        /// <param name="subEventCategoryTypeCode"></param>
        /// <returns></returns>
        public static List<lkpCommunicationSubEvent> GetlkpCommunicationSubEvent(String subEventCategoryBusinessChannel)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetlkpCommunicationSubEvent(subEventCategoryBusinessChannel);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }



        /// <summary>
        /// Method is used to update the communication sub event
        /// </summary>
        /// <param name="hNotificationMappingContract"></param>
        /// <param name="hNotificationMappingID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static Boolean UpdateCommunicationSubEvent(Int32 tenantId, HierarchyNotificationMappingContract hNotificationMappingContract, Int32 hNotificationMappingID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().UpdateCommunicationSubEvent(tenantId, hNotificationMappingContract, hNotificationMappingID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return false;

        }


        /// <summary>
        /// Method is used to save the communication event
        /// </summary>
        /// <param name="hNotificationMapping"></param>
        /// <returns></returns>
        public static Boolean SaveCommunicationSubEvent(HierarchyNotificationMapping hNotificationMapping)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().SaveCommunicationSubEvent(hNotificationMapping);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return false;
        }



        /// <summary>
        /// Method is used to delete the CommunicationSubEvents
        /// </summary>
        /// <param name="hNotificationMappingID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static Boolean DeleteCommunicationSubEvent(Int32 tenantID, Int32 hNotificationMappingID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().DeleteCommunicationSubEvent(tenantID, hNotificationMappingID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return false;
        }

        /// <summary>
        /// Get list of External User Contact Detail
        /// </summary>
        /// <param name="communicationSubEventId"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ExternalCopyUsersContract> GetExternalUserContactDetail(Int32 communicationSubEventId, Int32 hierarchyNodeID, Int32 tenantId)
        {
            try
            {
                String externalEmailkey = communicationSubEventId.ToString() + "_" + tenantId.ToString() + "_" + hierarchyNodeID;
                //To get external BCC users on the basis of Hierarchy Node ID, Tenant ID and Communication Sub Event ID
                List<ExternalCopyUsersContract> externalCopyUsers = new List<ExternalCopyUsersContract>();

                if (System.Web.HttpContext.Current != null)
                {
                    if (System.Web.HttpContext.Current.Items[externalEmailkey] != null)
                    {
                        externalCopyUsers = (List<ExternalCopyUsersContract>)System.Web.HttpContext.Current.Items[externalEmailkey];
                    }
                    else
                    {
                        List<HierarchyNotificationMapping> lstHrchyNotificationMapping = BALUtils.GetCommunicationRepoInstance().GetHierarchyNotificationMapping(communicationSubEventId, hierarchyNodeID, tenantId);
                        if (lstHrchyNotificationMapping.IsNotNull() && lstHrchyNotificationMapping.Count > AppConsts.NONE)
                        {
                            List<Int32> hierarchyContactMappingIDs = lstHrchyNotificationMapping.Select(x => x.HNM_HierarchyContactMappingID.Value).ToList();
                            List<HierarchyContactMapping> lstHrchyContactMapping = BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetHierarchyContactMappingByMappingIds(hierarchyContactMappingIDs);

                            if (lstHrchyContactMapping.IsNotNull() && lstHrchyContactMapping.Count > AppConsts.NONE)
                            {
                                externalCopyUsers = (from HNM in lstHrchyNotificationMapping
                                                     join HCM in lstHrchyContactMapping
                                                     on HNM.HNM_HierarchyContactMappingID equals HCM.ICM_ID
                                                     select new ExternalCopyUsersContract
                                                     {
                                                         UserName = HCM.InstitutionContact.ICO_FirstName + " " + HCM.InstitutionContact.ICO_LastName,
                                                         UserEmailAddress = HCM.InstitutionContact.ICO_PrimaryEmailAddress,
                                                         CopyTypeCode = HNM.lkpCopyType.CT_Code,
                                                         UserID = !HCM.ICM_OrganizationUserID.HasValue ? HCM.InstitutionContact.ICO_ID : HCM.ICM_OrganizationUserID.Value,
                                                     }).ToList();

                                System.Web.HttpContext.Current.Items.Add(externalEmailkey, externalCopyUsers);
                            }
                        }
                    }
                }
                else if (ServiceContext.Current != null)
                {
                    if (ServiceContext.Current.DataDict != null && ServiceContext.Current.DataDict.ContainsKey(externalEmailkey))
                    {
                        externalCopyUsers = (List<ExternalCopyUsersContract>)ServiceContext.Current.DataDict.GetValue(externalEmailkey);
                    }
                    else
                    {
                        List<HierarchyNotificationMapping> lstHrchyNotificationMapping = BALUtils.GetCommunicationRepoInstance().GetHierarchyNotificationMapping(communicationSubEventId, hierarchyNodeID, tenantId);
                        if (lstHrchyNotificationMapping.IsNotNull() && lstHrchyNotificationMapping.Count > AppConsts.NONE)
                        {
                            List<Int32> hierarchyContactMappingIDs = lstHrchyNotificationMapping.Select(x => x.HNM_HierarchyContactMappingID.Value).ToList();
                            List<HierarchyContactMapping> lstHrchyContactMapping = BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetHierarchyContactMappingByMappingIds(hierarchyContactMappingIDs);

                            if (lstHrchyContactMapping.IsNotNull() && lstHrchyContactMapping.Count > AppConsts.NONE)
                            {
                                externalCopyUsers = (from HNM in lstHrchyNotificationMapping
                                                     join HCM in lstHrchyContactMapping
                                                     on HNM.HNM_HierarchyContactMappingID equals HCM.ICM_ID
                                                     select new ExternalCopyUsersContract
                                                     {
                                                         UserName = HCM.InstitutionContact.ICO_FirstName + " " + HCM.InstitutionContact.ICO_LastName,
                                                         UserEmailAddress = HCM.InstitutionContact.ICO_PrimaryEmailAddress,
                                                         CopyTypeCode = HNM.lkpCopyType.CT_Code
                                                     }).ToList();

                                if (ServiceContext.Current.DataDict == null)
                                    ServiceContext.Current.DataDict = new Dictionary<String, object>();
                                ServiceContext.Current.DataDict.Add(externalEmailkey, externalCopyUsers);
                            }
                        }
                    }
                }
                else if (ParallelTaskContext.Current != null)
                {
                    if (ParallelTaskContext.Current.DataDict != null && ParallelTaskContext.Current.DataDict.ContainsKey(externalEmailkey))
                    {
                        externalCopyUsers = (List<ExternalCopyUsersContract>)ParallelTaskContext.Current.DataDict.GetValue(externalEmailkey);
                    }
                    else
                    {
                        List<HierarchyNotificationMapping> lstHrchyNotificationMapping = BALUtils.GetCommunicationRepoInstance().GetHierarchyNotificationMapping(communicationSubEventId, hierarchyNodeID, tenantId);
                        if (lstHrchyNotificationMapping.IsNotNull() && lstHrchyNotificationMapping.Count > AppConsts.NONE)
                        {
                            List<Int32> hierarchyContactMappingIDs = lstHrchyNotificationMapping.Select(x => x.HNM_HierarchyContactMappingID.Value).ToList();
                            List<HierarchyContactMapping> lstHrchyContactMapping = BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetHierarchyContactMappingByMappingIds(hierarchyContactMappingIDs);

                            if (lstHrchyContactMapping.IsNotNull() && lstHrchyContactMapping.Count > AppConsts.NONE)
                            {
                                externalCopyUsers = (from HNM in lstHrchyNotificationMapping
                                                     join HCM in lstHrchyContactMapping
                                                     on HNM.HNM_HierarchyContactMappingID equals HCM.ICM_ID
                                                     select new ExternalCopyUsersContract
                                                     {
                                                         UserName = HCM.InstitutionContact.ICO_FirstName + " " + HCM.InstitutionContact.ICO_LastName,
                                                         UserEmailAddress = HCM.InstitutionContact.ICO_PrimaryEmailAddress,
                                                         CopyTypeCode = HNM.lkpCopyType.CT_Code
                                                     }).ToList();

                                if (ParallelTaskContext.Current.DataDict == null)
                                    ParallelTaskContext.Current.DataDict = new Dictionary<String, object>();
                                ParallelTaskContext.Current.DataDict.Add(externalEmailkey, externalCopyUsers);
                            }
                        }
                    }
                }

                return externalCopyUsers;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<lkpCopyType> GetlkpCopyType()
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetlkpCopyType();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        public static List<Entity.lkpCommunicationSubEvent> GetlkpCommunicationSubEvent()
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetlkpCommunicationSubEvent();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        #endregion

        public static List<lkpDocumentAttachmentType> GetDocumentAttachmentType()
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpDocumentAttachmentType>()
                    .Where(condition => condition.DAT_IsDeleted == false).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Int32 SaveSystemCommunicationAttachment(SystemCommunicationAttachment sysCommAttachment)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().SaveSystemCommunicationAttachment(sysCommAttachment);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static bool SetRetryCountAndMessage(List<SystemCommunicationDelivery> systemCommunicationDeliveries, Int32 userId, String errorMessage)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().SetRetryCountAndMessage(systemCommunicationDeliveries, userId, errorMessage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        #region UserMessageGrid

        public static List<MessageDetail> GetUserMessages(Int32 userId, Int32 pageSize, Int32 pageIndex, String defaultExpression, Boolean isSortDirectionDescending, Int32 ApplicantDashboard = 0)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetUserMessages(userId, pageSize, pageIndex, defaultExpression, isSortDirectionDescending, ApplicantDashboard);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        public static List<CommunicationCCUser> GetNotificationSettingData(Int32 oganizationUserID)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetNotificationSettingData(oganizationUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="communicationSubEvents"></param>
        /// <param name="dictMailData"></param>
        /// <param name="mockData"></param>
        /// <param name="currentLoggedInUserId"></param> 
        public static Int32? SendRecurringBackgroundReports(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, CommunicationMockUpData mockData, Int32 tenantId, Int32 hierarchyNodeID, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, Boolean isExternalUserRequired = true)
        {
            try
            {
                return SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, tenantId, hierarchyNodeID, entityID, entityTypeCode, ignoreSubscriptionSeting, overrideCcBcc, lstBackgroundOrderDailyReport, isExternalUserRequired);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #region Service Attached Form

        public static List<CommunicationTemplatePlaceHolder> GetTemplatePlaceHolders(List<String> communicationTemplatePlaceHoldersProperty)
        {
            try
            {
                return GetCommunicationTemplatePlaceHolders().Where(ctPlaceHolders => communicationTemplatePlaceHoldersProperty
                                                             .Contains(ctPlaceHolders.Property)).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean SaveServiceAttachedFormCommunicationTemplate(List<Entity.lkpCommunicationSubEvent> commSubEventsList)
        {
            try
            {
                BALUtils.GetCommunicationRepoInstance().SaveServiceAttachedFormCommunicationTemplate(commSubEventsList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            return true;
        }

        public static Boolean UpdateServiceAttachedFormCommunicationTemplate()
        {
            try
            {
                BALUtils.GetCommunicationRepoInstance().UpdateServiceAttachedFormCommunicationTemplate();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            return true;
        }


        public static IEnumerable<CommunicationTemplateEntity> GetCommunicationTemplateEntityData(Int32 serviceFormID)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationTemplateEntityData(serviceFormID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static CommunicationTemplate GetCommunicationTemplateData(Int32 communicationTemplateID)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetCommunicationTemplateData(communicationTemplateID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean DeleteServiceAttachedFormCommunicationData(Int32 serviceFormID, Int32 currntUserID, Boolean VersionServiceFormToDelete)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().DeleteServiceAttachedFormCommunicationData(serviceFormID, currntUserID, VersionServiceFormToDelete);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean InsertCommunicationTemplatesEntities(List<Entity.CommunicationTemplateEntity> communicationTemplateEntitiesToBeAdded)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().InsertCommunicationTemplatesEntities(communicationTemplateEntitiesToBeAdded);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        public static void SendPartialOrderCancellationMailAndMessage(OrderPaymentDetail orderPaymentDetail, int tenantId, String packageNames)
        {
            try
            {
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = GetPartialOrderCancellationMailData(orderPaymentDetail, tenantId, packageNames);

                mockData.UserName = string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName);
                mockData.EmailID = orderPaymentDetail.Order.OrganizationUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;
                //Send mail
                //SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_PARTIAL_ORDER_CANCELLATION, mockData, dictMailData, tenantId, orderPaymentDetail.Order.HierarchyNodeID.Value, null);
                //UAT - 1067 : Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_PARTIAL_ORDER_CANCELLATION, mockData, dictMailData, tenantId, orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : 0, null);
                //Send message
                SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_PARTIAL_ORDER_CANCELLATION, dictMailData, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, tenantId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        private static Dictionary<String, object> GetPartialOrderCancellationMailData(OrderPaymentDetail orderPaymentDetail, int tenantId, String packageNames)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetPartialOrderCancellationMailData(orderPaymentDetail, tenantId, packageNames);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        #region Employment Notification For FlaggedOrder

        public static List<Entity.ClientEntity.CommunicationCCUsersList> GetClientAdminsForEmploymentNotification(CommunicationSubEvents communicationSubEventCode, Int32 tenantId, Int32 hierarchyNodeID)
        {
            try
            {
                lkpCommunicationSubEvent communicationSubEvent = GetCommunicationTypeSubEvents(communicationSubEventCode.GetStringValue());
                Int32 communicationSubEventId = AppConsts.NONE;
                if (communicationSubEvent.IsNotNull())
                    communicationSubEventId = communicationSubEvent.CommunicationSubEventID;
                return BALUtils.GetCommunicationRepoInstance().GetClientAdminsForEmploymentNotification(communicationSubEventId, tenantId, communicationSubEventCode, hierarchyNodeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }
        #endregion

        public static int GetCommunicationTemplateIDForSubEventID(int subEventID)
        {
            return BALUtils.GetCommunicationRepoInstance().GetCommunicationTemplateIDForSubEventID(subEventID);
        }

        #region UAT-1358:Complio Notification to applicant for PrintScan
        public static void SendNotificationForPrintScan(OrderPaymentDetail orderPaymentDetail, int tenantId)
        {
            try
            {
                CommunicationMockUpData mockData = new CommunicationMockUpData();
                Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ",
                                                                                   orderPaymentDetail.Order.OrganizationUserProfile.LastName));
                dictMailData.Add(EmailFieldConstants.ORDER_NO, orderPaymentDetail.Order.OrderNumber);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, WebSiteManager.GetInstitutionUrl(tenantId));


                mockData.UserName = string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName);
                mockData.EmailID = orderPaymentDetail.Order.OrganizationUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;

                SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_PRINT_SCAN_SERVICE, mockData, dictMailData, tenantId,
                                              orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : 0, null);
                //Send message
                SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_PRINT_SCAN_SERVICE, dictMailData, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID,
                                   tenantId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }
        #endregion

        //public static void SendEmail(string receiverEmailID, int receiverOrganizationUserID, string receiverUserName, int currentUserId, bool ignoreSubscriptionSeting, string emailBody, string emailSubject)
        public static void SendEmail(List<Int32> toIds, List<Int32> ccIds, List<Int32> bccIds, int currentUserId, string emailBody, string emailSubject, List<SystemCommunicationAttachment> lstSystemCommunicationAttachment)
        {
            try
            {
                //Notification Sub Event
                CommunicationSubEvents communicationSubEvent = CommunicationSubEvents.EXTERNAL_EMAIL_NOTIFICATION;

                String communicationSubeventCode = communicationSubEvent.GetStringValue();

                lkpCommunicationSubEvent communicationSubEventInDb = new lkpCommunicationSubEvent();
                communicationSubEventInDb = GetCommunicationTypeSubEvents(communicationSubeventCode);

                List<Entity.OrganizationUser> lstToUsers = new List<Entity.OrganizationUser>();
                List<Entity.OrganizationUser> lstCCUsers = new List<Entity.OrganizationUser>();
                List<Entity.OrganizationUser> lstBCCUsers = new List<Entity.OrganizationUser>();

                if (toIds.IsNotNull() && toIds.Count > 0)
                    lstToUsers = SecurityManager.GetOrganizationUserByIds(toIds.Distinct().ToList());

                if (lstToUsers.IsNullOrEmpty())
                {
                    lstToUsers.Add(new Entity.OrganizationUser
                    {
                        FirstName = AppConsts.BACKGROUND_PROCESS_USER_NAME,
                        OrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE,
                        PrimaryEmailAddress = AppConsts.BACKGROUND_PROCESS_USER_EMAIL
                    });
                }

                if (ccIds.IsNotNull() && ccIds.Count > 0)
                    lstCCUsers = SecurityManager.GetOrganizationUserByIds(ccIds.Distinct().ToList());

                if (bccIds.IsNotNull() && bccIds.Count > 0)
                    lstBCCUsers = SecurityManager.GetOrganizationUserByIds(bccIds.Distinct().ToList());

                foreach (Entity.OrganizationUser toUser in lstToUsers)
                {
                    //Mail Communication Contract
                    Int32? systemCommunicationID = null;
                    bool ignoreSubscriptionSeting = true;

                    CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
                    communicationTemplateContract.CurrentUserId = currentUserId;
                    communicationTemplateContract.SenderName = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME]);
                    communicationTemplateContract.SenderEmailID = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
                    communicationTemplateContract.RecieverName = string.Concat(toUser.FirstName, " ", toUser.LastName);
                    communicationTemplateContract.RecieverEmailID = toUser.PrimaryEmailAddress.IsNullOrEmpty() ? toUser.aspnet_Users.aspnet_Membership.Email : toUser.PrimaryEmailAddress;//UAT-3319
                    communicationTemplateContract.ReceiverOrganizationUserId = toUser.OrganizationUserID;

                    List<SystemCommunicationAttachment> lstUserSystemCommunicationAttachment = new List<SystemCommunicationAttachment>();

                    if (!lstSystemCommunicationAttachment.IsNullOrEmpty())
                    {
                        lstSystemCommunicationAttachment.ForEach(x =>
                        {
                            SystemCommunicationAttachment systemCommunicationAttachment = new SystemCommunicationAttachment();
                            systemCommunicationAttachment.SCA_OriginalDocumentID = x.SCA_OriginalDocumentID;
                            systemCommunicationAttachment.SCA_OriginalDocumentName = x.SCA_OriginalDocumentName;
                            systemCommunicationAttachment.SCA_DocumentPath = x.SCA_DocumentPath;
                            systemCommunicationAttachment.SCA_DocumentSize = x.SCA_DocumentSize;
                            systemCommunicationAttachment.SCA_DocAttachmentTypeID = x.SCA_DocAttachmentTypeID;
                            systemCommunicationAttachment.SCA_TenantID = x.SCA_TenantID;
                            systemCommunicationAttachment.SCA_IsDeleted = x.SCA_IsDeleted;
                            systemCommunicationAttachment.SCA_CreatedBy = x.SCA_CreatedBy;
                            systemCommunicationAttachment.SCA_CreatedOn = x.SCA_CreatedOn;
                            systemCommunicationAttachment.SCA_ModifiedBy = x.SCA_ModifiedBy;
                            systemCommunicationAttachment.SCA_ModifiedOn = x.SCA_ModifiedOn;
                            lstUserSystemCommunicationAttachment.Add(systemCommunicationAttachment);
                        });
                    }
                    if (communicationSubEventInDb != null)
                    {
                        UserCommunicationSubscriptionSetting userCommunicationSubscriptionSetting = GetUserCommunicationSubscriptionSettings(communicationTemplateContract.ReceiverOrganizationUserId, communicationSubEventInDb.CommunicationTypeID, communicationSubEventInDb.CommunicationEventID);

                        if (ignoreSubscriptionSeting || (userCommunicationSubscriptionSetting != null
                            && userCommunicationSubscriptionSetting.IsSubscribedToAdmin
                            && userCommunicationSubscriptionSetting.IsSubscribedToUser) || (communicationTemplateContract.ReceiverOrganizationUserId == AppConsts.BACKGROUND_PROCESS_USER_VALUE))
                        {
                            //Prepare Message
                            SystemCommunication systemCommunication = new SystemCommunication();
                            systemCommunication.Content = emailBody;
                            systemCommunication.Subject = emailSubject;
                            systemCommunication.CommunicationSubEventID = communicationSubEventInDb.CommunicationSubEventID;
                            systemCommunicationID = BALUtils.GetCommunicationRepoInstance().SaveMailContent(systemCommunication, communicationTemplateContract, lstCCUsers, lstBCCUsers, lstUserSystemCommunicationAttachment);
                        }
                    }

                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static List<lkpRecordObjectType> GetlkpRecordObjectType()
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetlkpRecordObjectType();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        #region [UAT-1072]

        public static Guid? SaveMessageContentForCCUserSettings(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, Int32 applicantId, Int32 tenantId, Int32 objectTypeId, Int32 recordId, Dictionary<String, object> dicMessageParam = null)
        {
            String communicationSubeventCode = communicationSubEvent.IsNotNull() ? communicationSubEvent.GetStringValue() : String.Empty;
            Guid? messageID = null;
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Communication Sub Event: " + communicationSubeventCode + "";
                parametersPassed += ", Applicant Id: " + applicantId.ToString() + "";
                if (dicContent.IsNotNull())
                    parametersPassed += ", Dictionary Count: " + dicContent.Count.ToString() + "";
                else
                    parametersPassed += ", Dictionary Obejct passed is NULL";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: SaveMessageContent with " + parametersPassed + "");

                Int32? entityID = null;
                String entityTypeCode = null;
                String documentName = String.Empty;

                lkpCommunicationSubEvent communicationSubEventInDb = new lkpCommunicationSubEvent();
                if (dicMessageParam != null && dicMessageParam.Count > 0)
                {
                    if (dicMessageParam.ContainsKey("EntityID"))
                        entityID = Convert.ToInt32(dicMessageParam.GetValue("EntityID"));
                    if (dicMessageParam.ContainsKey("EntityTypeCode"))
                        entityTypeCode = Convert.ToString(dicMessageParam.GetValue("EntityTypeCode"));
                    if (dicMessageParam.ContainsKey("DocumentName"))
                        documentName = Convert.ToString(dicMessageParam.GetValue("DocumentName"));
                    communicationSubEventInDb = GetCommSubEventByEntity(entityID.Value, entityTypeCode, communicationSubeventCode);
                    if (communicationSubEventInDb.IsNotNull())
                    {
                        communicationSubeventCode = communicationSubEventInDb.Code;

                    }
                }
                else
                {
                    communicationSubEventInDb = GetCommunicationTypeSubEvents(communicationSubeventCode);
                }

                if (communicationSubEventInDb != null)
                {
                    SystemCommunication systemCommunication = PrepareMessageContent(communicationSubeventCode, dicContent, entityID, entityTypeCode);
                    if (systemCommunication.IsNotNull())
                    {
                        MessagingContract messagingContract = GetMessageContractForCCusersDataWithNodePermissionAndCCUserSettings(communicationSubeventCode, systemCommunication, applicantId, communicationSubEventInDb.CommunicationSubEventID, tenantId, objectTypeId, recordId);
                        if (!String.IsNullOrEmpty(documentName))
                            messagingContract.DocumentName = documentName;

                        SendMessageContentToQueue(messagingContract);

                        messageID = messagingContract.MessageId;
                    }

                    //return true;
                }
                //return false;
                return messageID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static MessagingContract GetMessageContractForCCusersDataWithNodePermissionAndCCUserSettings(String communicationSubEventCode, SystemCommunication systemCommunication, int applicantId, Int32 communicationSubEventId, Int32 tenantId, Int32 objectTypeId, Int32 recordId)
        {
            try
            {
                Entity.AppConfiguration appConfiguration = communicationSubEventCode == CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED.GetStringValue() ?
                    SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID) : SecurityManager.GetAppConfiguration(AppConsts.SYSTEM_COMMUNICATION_USER_ID);
                Int32 systemCommunicationUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    systemCommunicationUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }

                String key = communicationSubEventId.ToString() + "_" + tenantId.ToString();

                List<Entity.ClientEntity.CommunicationCCUsersList> defaultCommunicationCCusers = new List<Entity.ClientEntity.CommunicationCCUsersList>();
                if (System.Web.HttpContext.Current != null)
                {
                    if (System.Web.HttpContext.Current.Items[key] != null)
                    {
                        defaultCommunicationCCusers = (List<Entity.ClientEntity.CommunicationCCUsersList>)System.Web.HttpContext.Current.Items[key];
                    }
                    else
                    {
                        defaultCommunicationCCusers = ComplianceSetupManager.GetCCusersWithNodePermissionAndCCUserSettings(communicationSubEventId, tenantId, null, objectTypeId, recordId);
                        //Every Time need to get CC users, Because depend's upon permission
                        //[UAT-1072]
                        //if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        //{
                        //    System.Web.HttpContext.Current.Items.Add(key, defaultCommunicationCCusers);
                        //}
                    }
                }
                else if (ServiceContext.Current != null)
                {
                    if (ServiceContext.Current.DataDict != null && ServiceContext.Current.DataDict.ContainsKey(key))
                    {
                        defaultCommunicationCCusers = (List<Entity.ClientEntity.CommunicationCCUsersList>)ServiceContext.Current.DataDict.GetValue(key);
                    }
                    else
                    {
                        defaultCommunicationCCusers = ComplianceSetupManager.GetCCusersWithNodePermissionAndCCUserSettings(communicationSubEventId, tenantId, null, objectTypeId, recordId);
                        //Every Time need to get CC users, Because depend's upon permission
                        //[UAT-1072]
                        //if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        //{
                        //    if (ServiceContext.Current.DataDict == null)
                        //        ServiceContext.Current.DataDict = new Dictionary<string, object>();
                        //    ServiceContext.Current.DataDict.Add(key, defaultCommunicationCCusers);
                        //}
                    }
                }
                else if (ParallelTaskContext.Current != null)
                {
                    if (ParallelTaskContext.Current.DataDict != null && ParallelTaskContext.Current.DataDict.ContainsKey(key))
                    {
                        defaultCommunicationCCusers = (List<Entity.ClientEntity.CommunicationCCUsersList>)ParallelTaskContext.Current.DataDict.GetValue(key);
                    }
                    else
                    {
                        defaultCommunicationCCusers = ComplianceSetupManager.GetCCusersWithNodePermissionAndCCUserSettings(communicationSubEventId, tenantId, null, objectTypeId, recordId);
                        //Every Time need to get CC users, Because depend's upon permission
                        //[UAT-1072]
                        //if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        //{
                        //    if (ParallelTaskContext.Current.DataDict == null)
                        //        ParallelTaskContext.Current.DataDict = new Dictionary<string, object>();
                        //    ParallelTaskContext.Current.DataDict.Add(key, defaultCommunicationCCusers);
                        //}
                    }
                }

                //changes done regarding UAt-
                String defaultCCUserIds = String.Empty;
                String defaultCCuserEmailIds = String.Empty;
                String defaultBCCUserIds = String.Empty;
                String defaultBCCuserEmailIds = String.Empty;
                if (!defaultCommunicationCCusers.IsNullOrEmpty() && defaultCommunicationCCusers.Count > AppConsts.NONE)
                {
                    defaultCommunicationCCusers = defaultCommunicationCCusers.Where(condition => condition.IsCommunicationCentre).ToList();
                    String ccCode = CopyType.CC.GetStringValue();
                    String bccCode = CopyType.BCC.GetStringValue();

                    defaultCCUserIds = String.Join(";", defaultCommunicationCCusers.Where(condition => condition.CopyCode == ccCode).Select(x => x.UserID));
                    defaultCCuserEmailIds = String.Join(";", defaultCommunicationCCusers.Where(condition => condition.CopyCode == ccCode).Select(x => x.EmailAddress));

                    defaultBCCUserIds = String.Join(";", defaultCommunicationCCusers.Where(condition => condition.CopyCode == bccCode).Select(x => x.UserID));
                    defaultBCCuserEmailIds = String.Join(";", defaultCommunicationCCusers.Where(condition => condition.CopyCode == bccCode).Select(x => x.EmailAddress));
                }
                MessagingContract messagingContract = new MessagingContract();
                messagingContract.Action = MessagingAction.NewMail;
                messagingContract.ApplicationDatabaseName = MessageManager.GetDatabaseName(ConfigurationManager.ConnectionStrings[AppConsts.APPLICATION_CONNECTION_STRING].ConnectionString);
                messagingContract.CommunicationType = "CT01";
                messagingContract.FolderId = MessageManager.GetFolderIdByCode(lkpMessageFolderContext.INBOX.GetStringValue());
                messagingContract.Content = systemCommunication.Content.IsNotNull() ? systemCommunication.Content.Replace("<p>", "<p style=\"margin-bottom:10px;\">") : String.Empty;
                messagingContract.CurrentUserId = systemCommunicationUserId;
                if (applicantId != AppConsts.BACKGROUND_PROCESS_USER_VALUE)
                {
                    messagingContract.ToList = MessageManager.GetEmailId(applicantId);
                    messagingContract.ToUserIds = applicantId.ToString();
                }
                //else
                //{
                //    messagingContract.ToList = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                //    messagingContract.ToUserIds = applicantId.ToString();
                //}
                messagingContract.From = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]); ;
                messagingContract.IsHighImportance = false;
                messagingContract.MessageMode = "S";
                messagingContract.Subject = systemCommunication.Subject;

                if (!String.IsNullOrEmpty(defaultCCUserIds))
                {
                    messagingContract.CcList = defaultCCuserEmailIds;
                    messagingContract.CcUserIds = defaultCCUserIds;
                }
                if (!String.IsNullOrEmpty(defaultBCCUserIds))
                {
                    messagingContract.BCcList = defaultBCCuserEmailIds;
                    messagingContract.BccUserIds = defaultBCCUserIds;
                }
                return messagingContract;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Int32? SendPackageNotificationMailForCCUserSettings(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, CommunicationMockUpData mockData, Int32 tenantId, Int32 hierarchyNodeID, Int32 objectTypeId, Int32 recordId, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Boolean overrideCcBcc = false)
        {
            try
            {
                return SaveMailCommunicationContractForCCUserSettings(communicationSubEvents, mockData, dictMailData, tenantId, hierarchyNodeID, objectTypeId, recordId, entityID, entityTypeCode, ignoreSubscriptionSeting, overrideCcBcc);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        private static Int32? SaveMailCommunicationContractForCCUserSettings(CommunicationSubEvents communicationSubEvents, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, Int32 tenantId, Int32 hierarchyNodeID, Int32 objectTypeId, Int32 recordId, Int32? entityID, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, Boolean isExternalUserRequired = true)
        {

            Int32? systemCommunicationID = null;
            Entity.AppConfiguration appConfiguration = communicationSubEvents.GetStringValue() == CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED.GetStringValue() ?
                 SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID) : SecurityManager.GetAppConfiguration(AppConsts.SYSTEM_COMMUNICATION_USER_ID);
            Int32 systemCommunicationUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
            if (appConfiguration.IsNotNull())
            {
                systemCommunicationUserId = Convert.ToInt32(appConfiguration.AC_Value);
            }

            ISubject subject = new Subject();

            CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
            communicationTemplateContract.CurrentUserId = systemCommunicationUserId;
            communicationTemplateContract.SenderName = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME]);
            communicationTemplateContract.SenderEmailID = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
            communicationTemplateContract.RecieverName = mockData.UserName;
            communicationTemplateContract.RecieverEmailID = mockData.EmailID;
            communicationTemplateContract.ReceiverOrganizationUserId = mockData.ReceiverOrganizationUserID;

            subject.Attach(
               new Observer<object>(subject, new { dictionary = dictMailData, details = communicationTemplateContract }, (theSubject, theElement) =>
               {
                   //Send mail
                   //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons(Remove lstClientAdmin)
                   systemCommunicationID = SaveMailContentBasisOnSubscriptionForCCUserSettings(communicationSubEvents, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, hierarchyNodeID, objectTypeId, recordId, tenantId, entityID, entityTypeCode, ignoreSubscriptionSeting, 0, 0, overrideCcBcc, lstBackgroundOrderDailyReport, isExternalUserRequired);

               }));
            subject.Notify();

            return systemCommunicationID;
        }

        public static Int32? SaveMailContentBasisOnSubscriptionForCCUserSettings(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, CommunicationTemplateContract communicationTemplateContract, Int32 hierarchyNodeID, Int32 objectTypeId, Int32 recordId, Int32 tenantId = 0, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Int32 systemCommunicationDeliveryId = 0, Int32 currentUserId = 0, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, Boolean isExternalUserRequired = true)
        {
            Int32? systemCommunicationID = null;
            String parametersPassed = String.Empty;
            String communicationSubeventCode = communicationSubEvent.IsNotNull() ? communicationSubEvent.GetStringValue() : String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Communication Sub Event: " + communicationSubeventCode + "";
                parametersPassed += ", System Communication Delivery Id: " + systemCommunicationDeliveryId.ToString() + "";
                parametersPassed += ", Current User Id: " + currentUserId.ToString() + "";
                if (dicContent.IsNotNull())
                    parametersPassed += ", Dictionary Count: " + dicContent.Count.ToString() + "";
                else
                    parametersPassed += ", Dictionary Obejct passed is NULL";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: SaveMailContentBasisOnSubscription with " + parametersPassed + "");

                lkpCommunicationSubEvent communicationSubEventInDb = new lkpCommunicationSubEvent();

                if (entityID != null && !String.IsNullOrEmpty(entityTypeCode))
                {
                    communicationSubEventInDb = GetCommSubEventByEntity(entityID.Value, entityTypeCode, communicationSubeventCode);
                    if (communicationSubEventInDb.IsNotNull())
                    {
                        communicationSubeventCode = communicationSubEventInDb.Code;
                    }
                }
                else if (communicationSubEvent == CommunicationSubEvents.NONE)
                {
                    lkpCommunicationSubEvent lkpSubEvent = GetCommunicationTypeSubEvents(Convert.ToInt32(communicationTemplateContract.CommunicationSubEventID));//BALUtils.GetCommunicationRepoInstance().GetCommunicationTypeSubEvents(Convert.ToInt32(communicationTemplateContract.CommunicationSubEventID));
                    communicationSubEventInDb = lkpSubEvent;
                }
                else
                {
                    communicationSubEventInDb = GetCommunicationTypeSubEvents(communicationSubeventCode);
                }
                if (communicationSubEventInDb != null)
                {
                    UserCommunicationSubscriptionSetting userCommunicationSubscriptionSetting = GetUserCommunicationSubscriptionSettings(communicationTemplateContract.ReceiverOrganizationUserId, communicationSubEventInDb.CommunicationTypeID, communicationSubEventInDb.CommunicationEventID);
                    if (ignoreSubscriptionSeting || (userCommunicationSubscriptionSetting != null
                        && userCommunicationSubscriptionSetting.IsSubscribedToAdmin
                        && userCommunicationSubscriptionSetting.IsSubscribedToUser) || (communicationTemplateContract.ReceiverOrganizationUserId == AppConsts.BACKGROUND_PROCESS_USER_VALUE))
                    {
                        if (systemCommunicationDeliveryId == 0)
                        {
                            SystemCommunication systemCommunication = PrepareMessageContent(communicationSubeventCode, dicContent, entityID, entityTypeCode);
                            if (systemCommunication.IsNotNull())
                            {
                                //Send mail
                                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                                systemCommunicationID = SaveMailContentForCCUserSettings(systemCommunication, communicationTemplateContract, communicationSubEventInDb.CommunicationSubEventID, tenantId, hierarchyNodeID, communicationSubEvent, objectTypeId, recordId, overrideCcBcc, lstBackgroundOrderDailyReport, isExternalUserRequired);
                            }
                        }
                        else
                            BALUtils.GetCommunicationRepoInstance().ReSendEmail(systemCommunicationDeliveryId, currentUserId);
                        //return true;
                    }
                }
                //return false;
                return systemCommunicationID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        private static Int32 SaveMailContentForCCUserSettings(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 CommunicationSubEventID, Int32 tenantId, Int32 hierarchyNodeID, CommunicationSubEvents communicationSubEvents, Int32 objectTypeId, Int32 recordId, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, Boolean isExternalUserRequired = true)
        {
            try
            {
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //UAT-807 Addition of a flagged report only notification
                List<ExternalCopyUsersContract> externalCopyUsers = new List<ExternalCopyUsersContract>();
                if (communicationSubEvents != CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS
                    && communicationSubEvents != CommunicationSubEvents.NOTIFICATION_FOR_FLAGGED_ORDER_RESULTS
                    && communicationSubEvents != CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS
                    && isExternalUserRequired)
                    externalCopyUsers = GetExternalUserContactDetail(CommunicationSubEventID, hierarchyNodeID, tenantId);
                return BALUtils.GetCommunicationRepoInstance().SaveMailContentForCCUserSettings(systemCommunication, communicationTemplateContract, CommunicationSubEventID, tenantId, hierarchyNodeID, externalCopyUsers, communicationSubEvents, objectTypeId, recordId, overrideCcBcc, lstBackgroundOrderDailyReport);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Email Notification for the Document rejected by the Admin
        /// </summary>
        /// <param name="orgUserProfile"></param>
        /// <param name="tenantId"></param>
        public static void SendDocumentRejectionEmail(Int32 applicantId, String documentName, Int32 tenantId)
        {
            try
            {
                Entity.OrganizationUser _applicantData = SecurityManager.GetOrganizationUserDetailByOrganizationUserId(applicantId);
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ADMIN_DATA_ENTRY_DOCUMENT_REJECTED;

                //UAT-2389
                String tenantName = ClientSecurityManager.GetTenantName(tenantId);

                CommunicationMockUpData mockData = new CommunicationMockUpData();

                mockData.UserName = string.Concat(_applicantData.FirstName, " ", _applicantData.LastName);
                mockData.EmailID = _applicantData.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = _applicantData.OrganizationUserID;

                Dictionary<String, Object> dictMailData = new Dictionary<String, Object>();

                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, String.Concat(_applicantData.FirstName, " ", _applicantData.LastName));
                dictMailData.Add(EmailFieldConstants.ADMIN_DATA_ENTRY_REJECTED_DOCUMENT_NAME, documentName);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);

                //Send mail
                Int32? systemCommunicationID = SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, -1, null, null, true);

                if (systemCommunicationID.IsNotNull() && systemCommunicationID > 0)
                {
                    //send message
                    CommunicationManager.SaveMessageContent(commSubEvent, dictMailData, _applicantData.OrganizationUserID, tenantId);
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }


        #region UAT-1578:Addition of SMS notification
        /// <summary>
        /// Save data for SMS notification in System Communication and System Communication Delivery
        /// </summary>
        /// <param name="communicationSubEvents">communicationSubEvents</param>
        /// <param name="mockData">mockData to create SMS Message</param>
        /// <param name="communicationTemplateContract">communicationTemplateContract</param>
        /// <param name="dictMailData">dictMailData</param>
        /// <param name="tenantId">tenantId</param>
        /// <param name="hierarchyNodeID">hierarchyNodeID</param>
        /// <returns>systemCommunicationID</returns>
        public static Int32 SaveDataForSMSNotification(CommunicationSubEvents communicationSubEvents, CommunicationMockUpData mockData,
                                                          Dictionary<String, object> dictMailData, Int32 tenantId, Int32 hierarchyNodeID, Boolean IsEnroller = false)
        {
            try
            {
                //Set Compliance tamplate Contract
                CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
                communicationTemplateContract.CurrentUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
                communicationTemplateContract.SenderName = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME]);
                communicationTemplateContract.SenderEmailID = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
                communicationTemplateContract.RecieverName = mockData.UserName;
                communicationTemplateContract.RecieverEmailID = mockData.EmailID;
                communicationTemplateContract.ReceiverOrganizationUserId = mockData.ReceiverOrganizationUserID;

                //UAT-3824
                Int32 LangugageId = 0;
                Int32 DefaultLanguageID = 0;
                Int32? UserID = null;
                //// get receiver organisation user id for getting users's preferred language
                if (dictMailData.ContainsKey(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID))
                    UserID = Convert.ToInt32(dictMailData.GetValue(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID));

                //get user language id here, if user prefered language is specified than get that language id otherwise get default(english) language id  
                LangugageId = BALUtils.GetSecurityRepoInstance().GetLanguageIdByGuid(UserID, tenantId, out DefaultLanguageID);

                String communicationSubeventCode = communicationSubEvents.IsNotNull() ? communicationSubEvents.GetStringValue() : String.Empty;
                Int32 communicationSubEventId = 0;
                Int32 systemCommunicationID = 0;
                if (!communicationSubeventCode.IsNullOrEmpty())
                {
                    lkpCommunicationSubEvent communicationSubEventInDb = GetCommunicationTypeSubEvents(communicationSubeventCode);
                    communicationSubEventId = !communicationSubEventInDb.IsNullOrEmpty() ? communicationSubEventInDb.CommunicationSubEventID : AppConsts.NONE;

                    //get user language specified template
                    SMSTemplate smsTemplate = GetSMSTemplateForCommunicationSubEvent(communicationSubEventId, LangugageId, DefaultLanguageID);

                    #region UAT-4270
                    Boolean IsNotificationNeedToSendForEnroller = false;
                    if (IsEnroller)
                    {
                        IsNotificationNeedToSendForEnroller = BALUtils.GetSecurityRepoInstance().GetNotificationNeedToSendForEnroller(mockData.ReceiverOrganizationUserID);
                    }
                    #endregion

                    if ((!smsTemplate.IsNullOrEmpty() && SMSNotificationManager.IsSubscriptionActiveForSMS(communicationTemplateContract.ReceiverOrganizationUserId))
                        || communicationSubeventCode.ToLower() == CommunicationSubEvents.NOTIFICATION_FOR_LOGIN_VIA_2FA_SMS.GetStringValue().ToLower()
                        || (IsNotificationNeedToSendForEnroller && communicationSubeventCode.ToLower() == CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_APPOINTMENT_SET_FOR_ENROLLER_SITE.GetStringValue().ToLower()))  //UAT-4270
                    {
                        SystemCommunication systemCommunication = PrepareMessageContentForSMS(communicationSubeventCode, dictMailData, smsTemplate);
                        if (systemCommunication.IsNotNull())
                        {
                            systemCommunicationID = SaveSMSContent(systemCommunication, communicationTemplateContract);
                        }
                    }
                }
                return systemCommunicationID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Return Template for SMS
        /// </summary>
        /// <param name="communicationSubEventId">communicationSubEventId</param>
        /// <returns>SMSTemplate</returns>
        public static SMSTemplate GetSMSTemplateForCommunicationSubEvent(Int32 communicationSubEventId, Int32 languageID, Int32 DefaultLanguageID)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetSMSTemplateForCommunicationSubEvent(communicationSubEventId, languageID, DefaultLanguageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method to prepare message content for SMS
        /// </summary>
        /// <param name="communicationSubEventCode">communicationSubEventCode</param>
        /// <param name="dicContent">dicContent</param>
        /// <param name="smsTemplate">smsTemplate</param>
        /// <returns>SystemCommunication</returns>
        public static SystemCommunication PrepareMessageContentForSMS(String communicationSubEventCode, Dictionary<String, Object> dicContent, SMSTemplate smsTemplate)
        {
            String parametersPassed = String.Empty;
            try
            {
                parametersPassed = "Parameters Passed - Communication Sub Event: " + communicationSubEventCode + "";
                if (dicContent.IsNotNull())
                    parametersPassed += ", Dictionary Count: " + dicContent.Count.ToString() + "";
                else
                    parametersPassed += ", Dictionary Obejct passed is NULL";
                if (BALUtils.LoggerService.IsNotNull())
                    BALUtils.LoggerService.GetLogger().Info(BALUtils.ClassModule + "Method Started: PrepareMessageContent with " + parametersPassed + "");

                Int32 subEventId = GetCommunicationTypeSubEvents(communicationSubEventCode).CommunicationSubEventID;

                List<CommunicationTemplatePlaceHolder> placeHoldersToFetch = null;
                if (smsTemplate.IsNotNull())
                {
                    placeHoldersToFetch = GetTemplatePlaceHolders(subEventId);
                }
                return BALUtils.GetCommunicationRepoInstance().PrepareMessageContentForSMS(dicContent, smsTemplate, placeHoldersToFetch);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + parametersPassed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method to Save SMS content in Database 
        /// </summary>
        /// <param name="systemCommunication">systemCommunication</param>
        /// <param name="communicationTemplateContract">communicationTemplateContract</param>
        /// <returns></returns>
        public static Int32 SaveSMSContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract)
        {
            try
            {
                Int32 notificationTypeId = 0;
                notificationTypeId = LookupManager.GetMessagingLookUpData<lkpNotificationDeliveryType>().FirstOrDefault(cond =>
                                                                                                         cond.NDT_Code == NotificationDeliveryType.SMS.GetStringValue()
                                                                                                         && !cond.NDT_IsDeleted).NDT_ID;

                return BALUtils.GetCommunicationRepoInstance().SaveSMSContent(systemCommunication, communicationTemplateContract, notificationTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method to send SMS to user by using Amazon and also update subscription detail in database
        /// </summary>
        /// <param name="delivery">SystemCommunicationDelivery</param>
        /// <param name="message">message</param>
        /// <param name="systemUserId">systemUserId</param>
        /// <returns></returns>
        public static Boolean SendSMSToUser(SystemCommunicationDelivery delivery, String message, Int32 systemUserId)
        {
            try
            {
                if (!delivery.IsNullOrEmpty())
                {
                    string subEventCode = string.Empty;

                    if (!delivery.SystemCommunication.IsNullOrEmpty()
                            && !delivery.SystemCommunication.lkpCommunicationSubEvent.IsNullOrEmpty())
                    {
                        subEventCode = delivery.SystemCommunication.lkpCommunicationSubEvent.Code;
                    }

                    List<SystemCommunicationDelivery> deliverylist = new List<SystemCommunicationDelivery>();
                    deliverylist.Add(delivery);
                    String subscriptionARN = String.Empty;

                    //syn local data base with amazon subscription confirmation status.
                    //SMSNotificationManager.UpdateSubscriptionStatusFromAmazon(delivery.ReceiverOrganizationUserID, systemUserId);

                    //Get Applicant SMS notification subscription.
                    OrganisationUserTextMessageSetting applicantSMSNotification = SMSNotificationManager.GetSMSDataByApplicantId(delivery.ReceiverOrganizationUserID);
                    try
                    {
                        if (!applicantSMSNotification.IsNullOrEmpty())
                        {
                            if (SMSNotificationManager.IsSubscriptionConfirmedForSMS(delivery.ReceiverOrganizationUserID)
                                || subEventCode.ToLower() == CommunicationSubEvents.NOTIFICATION_FOR_LOGIN_VIA_2FA_SMS.GetStringValue().ToLower())
                            {
                                SMSNotification.SendSMS(applicantSMSNotification.OUTMS_MobileNumber, message);
                                SetDispatchedTrue(deliverylist, systemUserId);
                            }
                            else
                            {
                                String errorMessage = "Subscription not confirmed";
                                SetRetryCountAndMessage(deliverylist, systemUserId, errorMessage);
                            }
                        }
                        else if (subEventCode.ToLower() == CommunicationSubEvents.NOTIFICATION_FOR_FINGERPRINT_APPOINTMENT_SET_FOR_ENROLLER_SITE.GetStringValue().ToLower())
                        {
                            String EnrollerContactNumber = SecurityManager.GetEnrollerPhoneNumberForSMSNotification(delivery.ReceiverOrganizationUserID);
                            if (!EnrollerContactNumber.IsNullOrEmpty())
                            {
                                SMSNotification.SendSMS(EnrollerContactNumber, message);
                                SetDispatchedTrue(deliverylist, systemUserId);
                            }
                            else
                            {
                                String errorMessage = "User Subscription details not found.";
                                SetRetryCountAndMessage(deliverylist, systemUserId, errorMessage);
                            }
                        }
                        else
                        {
                            String errorMessage = "User Subscription details not found.";
                            SetRetryCountAndMessage(deliverylist, systemUserId, errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        String errorMessage = ex.Message;
                        SetRetryCountAndMessage(deliverylist, systemUserId, errorMessage);
                    }
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
        /// Get notification delivery type lookup data.
        /// </summary>
        /// <returns></returns>
        public static List<lkpNotificationDeliveryType> GetNotificationDeliveryType()
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpNotificationDeliveryType>()
                    .Where(condition => condition.NDT_IsDeleted == false).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion

        public static List<int> GetSubEventsHavingTemplates(int? languageId)
        {
            try
            {
                var defaultLanguageCode = CommunicationLanguages.DEFAULT.GetStringValue();
                var defaultLanguageId = GetLanguages().First(l => l.LAN_Code == defaultLanguageCode).LAN_ID;

                if (!languageId.HasValue)
                {
                    languageId = defaultLanguageId;
                }

                return BALUtils.GetCommunicationRepoInstance().GetSubEventsHavingTemplates(languageId.Value, defaultLanguageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<int>();
        }

        #region UAT-1793 : Should not be able to create duplicate templates in the common template section of the System Template screen.
        public static List<Int32> GetSubEventsHavingTemplatesByTenant(Int32 tenantId, Int32? languageId)
        {
            try
            {
                var defaultLanguageCode = CommunicationLanguages.DEFAULT.GetStringValue();
                var defaultLanguageId = GetLanguages().First(l => l.LAN_Code == defaultLanguageCode).LAN_ID;

                if (!languageId.HasValue)
                {
                    languageId = defaultLanguageId;
                }

                return BALUtils.GetCommunicationRepoInstance().GetSubEventsHavingTemplatesByTenant(tenantId, languageId.Value, defaultLanguageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<int>();
        }
        #endregion

        #region UAT-1852: If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        public static void SendMailForIncompleteServiceGroup(CommunicationSubEvents communicationSubEvents, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, Int32 tenantId, Int32 hierarchyId)
        {
            try
            {
                SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, tenantId, hierarchyId, null);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }
        #endregion

        //UAT-1759: Create the ability to mark an "Additional Documents" that the students complete in the order flow as "Send to student"
        public static void SendAdditionalDocumentToStudent(List<ApplicantDocument> lstApplicantDocument, Entity.ClientEntity.OrganizationUser orgUser,
                                                            Order orderDetail, Int32 tenantId, Int32 sendAdditionalDocNotificationTypeID, Int32 currentLoggedInUserId)
        {

            Int32? systemCommunicationID = null;

            Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();


            //Create Dictionary for Mail And Message Data
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(orgUser.FirstName, " ", orgUser.LastName));
            dictMailData.Add(EmailFieldConstants.ORDER_NO, orderDetail.OrderNumber);


            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = string.Concat(orgUser.FirstName, " ", orgUser.LastName);
            mockData.EmailID = orgUser.PrimaryEmailAddress;
            mockData.ReceiverOrganizationUserID = orgUser.OrganizationUserID;

            //Send mail
            systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.Notification_For_Additional_Documents, dictMailData, mockData, tenantId, orderDetail.SelectedNodeID.Value);

            systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;
            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
            String docAttachmentTypeCode = DocumentAttachmentType.APPLICANT_DOCUMENT.GetStringValue();
            Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

            String documentName = String.Empty;

            //Save Mail Attachment
            foreach (ApplicantDocument appDoc in lstApplicantDocument)
            {
                Int32? sysCommAttachmentID = null;
                if (systemCommunicationID != null)
                {
                    SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                    sysCommAttachment.SCA_OriginalDocumentID = appDoc.ApplicantDocumentID;
                    sysCommAttachment.SCA_OriginalDocumentName = appDoc.FileName;
                    sysCommAttachment.SCA_DocumentPath = appDoc.DocumentPath;
                    sysCommAttachment.SCA_DocumentSize = appDoc.Size.HasValue ? appDoc.Size.Value : 0;
                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                    sysCommAttachment.SCA_TenantID = tenantId;
                    sysCommAttachment.SCA_IsDeleted = false;
                    sysCommAttachment.SCA_CreatedBy = currentLoggedInUserId;
                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                    sysCommAttachment.SCA_ModifiedBy = null;
                    sysCommAttachment.SCA_ModifiedOn = null;

                    sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                }

                Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                List<ADBMessageDocument> messageDocument = new List<ADBMessageDocument>();

                ADBMessageDocument documentData = new ADBMessageDocument();
                documentData.DocumentName = appDoc.DocumentPath;
                documentData.OriginalDocumentName = appDoc.FileName;
                documentData.DocumentSize = appDoc.Size.Value;
                documentData.SystemCommunicationAttachmentID = sysCommAttachmentID;
                messageDocument.Add(documentData);

                attachedFiles = MessageManager.SaveDocumentAndGetDocumentId(messageDocument);
                if (!attachedFiles.IsNullOrEmpty())
                {
                    attachedFiles.ForEach(a => documentName += a.Key.ToString() + ";");
                }
            }
            dicMessageParam.Add("DocumentName", documentName);
            dicMessageParam.Add("IgnoreSpecificTemplate", true);
            //Send Message
            Guid? messageId = CommunicationManager.SaveMessageContent(CommunicationSubEvents.Notification_For_Additional_Documents, dictMailData, orgUser.OrganizationUserID, tenantId, dicMessageParam);

            String _notificationDetail = "Send Additional Documents To Student";

            List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
            String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
            Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                                          Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID)
                                          : Convert.ToInt16(0);

            OrderNotification ordNotification = new OrderNotification();
            ordNotification.ONTF_OrderID = orderDetail.OrderID;
            ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
            ordNotification.ONTF_MSG_MessageID = messageId;
            ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
            ordNotification.ONTF_IsPostal = false;
            ordNotification.ONTF_CreatedByID = currentLoggedInUserId;
            ordNotification.ONTF_CreatedOn = DateTime.Now;
            ordNotification.ONTF_ModifiedByID = null;
            ordNotification.ONTF_ModifiedDate = null;
            ordNotification.ONTF_ParentNotificationID = null;
            ordNotification.ONTF_OrderNotificationTypeID = sendAdditionalDocNotificationTypeID;
            ordNotification.ONTF_NotificationDetail = _notificationDetail;
            Int32 ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
        }
        //UAT-2538
        public static void SendRotInvitationAppRejMail(CommunicationSubEvents communicationSubEvents, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, Int32 tenantId, List<CommunicationTemplateContract> lstcommunicationTemplateContract, String rotationHirarchyIds, Int32? rotationID = AppConsts.NONE)
        {
            SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, tenantId, -1, null, null, false, false, null, true, lstcommunicationTemplateContract, null, rotationHirarchyIds, rotationID);
        }

        //UAT-2628
        public static void SendMailForMultipleReceivers(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, CommunicationMockUpData mockData, Int32 tenantId, Int32 hierarchyId, List<CommunicationTemplateContract> lstcommunicationTemplateContract)
        {
            SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, tenantId, hierarchyId, null, null, false, false, null, true, lstcommunicationTemplateContract);
        }
        //UAT-2958: UCONN SSO Flow updates
        public static void SendEmailForNewApplicantThroughSSO(CommunicationSubEvents communicationSubEvents, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, Int32 tenantId)
        {
            try
            {
                SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, tenantId, -1, null);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        #region UAT-2970

        public static void SendOrderConfirmationDocEmail(Int32 tenantId, Int32 currentLoggedInUser, OrderPaymentDetail opd, String orderNumber)
        {
            //send mail with attachment and message with attachment.
            List<Int32> lstApplicantDocumentIds = new List<Int32>();
            if (!opd.Order.ApplicantDocumentID.IsNullOrEmpty())
                lstApplicantDocumentIds.Add(opd.Order.ApplicantDocumentID.Value);
            if (!lstApplicantDocumentIds.IsNullOrEmpty())
            {
                List<ApplicantDocument> lstApplicantDocument = ComplianceDataManager.GetApplicantDocuments(tenantId, lstApplicantDocumentIds);
                ApplicantDocument applicantDocument = lstApplicantDocument[0];
                if (!applicantDocument.IsNullOrEmpty())
                {
                    SendOrderConfirmationDocForCreditCard(opd, tenantId, applicantDocument, currentLoggedInUser, orderNumber);
                }
                else
                {
                    var log = BALUtils.LoggerService.GetLogger();
                    if (!log.IsNullOrEmpty())
                    {
                        log.Error("Order confirmation Document not found for OrderID:" + opd.Order.OrderNumber);
                    }
                }
            }
        }

        private static void SendOrderConfirmationDocForCreditCard(OrderPaymentDetail orderPaymentDetail, Int32 tenantId, ApplicantDocument applicantDocument, Int32 currentLoggedInUser, String orderNumber)
        {
            try
            {
                if (!SecurityManager.IsLocationServiceTenant(tenantId))
                {
                    Int32? systemCommunicationID = null;
                    Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();

                    CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_ORDER_CONFIRMATION_DOCUMENT_FOR_CREDIT_CARD;
                    String tenantName = ClientSecurityManager.GetTenantName(tenantId);

                    //UAT-3254
                    Int32 hierarchyId = orderPaymentDetail.Order.SelectedNodeID.HasValue ? orderPaymentDetail.Order.SelectedNodeID.Value : AppConsts.NONE;

                    Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName));
                    dictMailData.Add(EmailFieldConstants.ORDER_NO, orderNumber == String.Empty ? orderPaymentDetail.Order.OrderNumber : orderNumber);
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);

                    CommunicationMockUpData mockData = new CommunicationMockUpData();
                    mockData.UserName = string.Concat(orderPaymentDetail.Order.OrganizationUserProfile.FirstName, " ", orderPaymentDetail.Order.OrganizationUserProfile.LastName);
                    mockData.EmailID = orderPaymentDetail.Order.OrganizationUserProfile.PrimaryEmailAddress;
                    mockData.ReceiverOrganizationUserID = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;

                    //Send mail
                    systemCommunicationID = SaveMailCommunicationContract(commSubEvent, mockData, dictMailData, tenantId, hierarchyId, null);
                    systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;

                    List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                    String docAttachmentTypeCode = DocumentAttachmentType.APPLICANT_DOCUMENT.GetStringValue();
                    Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                        Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                    String documentName = String.Empty;
                    if (!applicantDocument.IsNullOrEmpty() && systemCommunicationID != null)
                    {
                        Int32? sysCommAttachmentID = null;
                        SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                        sysCommAttachment.SCA_OriginalDocumentID = applicantDocument.ApplicantDocumentID;
                        sysCommAttachment.SCA_OriginalDocumentName = applicantDocument.FileName;
                        sysCommAttachment.SCA_DocumentPath = applicantDocument.DocumentPath;
                        sysCommAttachment.SCA_DocumentSize = applicantDocument.Size.HasValue ? applicantDocument.Size.Value : 0;
                        sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                        sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                        sysCommAttachment.SCA_TenantID = tenantId;
                        sysCommAttachment.SCA_IsDeleted = false;
                        sysCommAttachment.SCA_CreatedBy = currentLoggedInUser;
                        sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                        sysCommAttachment.SCA_ModifiedBy = null;
                        sysCommAttachment.SCA_ModifiedOn = null;

                        sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);

                        Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                        List<ADBMessageDocument> messageDocument = new List<ADBMessageDocument>();

                        ADBMessageDocument documentData = new ADBMessageDocument();
                        documentData.DocumentName = applicantDocument.DocumentPath;
                        documentData.OriginalDocumentName = applicantDocument.FileName;
                        documentData.DocumentSize = applicantDocument.Size.Value;
                        documentData.SystemCommunicationAttachmentID = sysCommAttachmentID;
                        messageDocument.Add(documentData);

                        attachedFiles = MessageManager.SaveDocumentAndGetDocumentId(messageDocument);
                        if (!attachedFiles.IsNullOrEmpty())
                        {
                            attachedFiles.ForEach(a => documentName += a.Key.ToString() + ";");
                        }
                    }

                    dicMessageParam.Add("DocumentName", documentName);
                    dicMessageParam.Add("IgnoreSpecificTemplate", true);

                    //Send Message
                    Guid? messageId = CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_ORDER_CONFIRMATION_DOCUMENT_FOR_CREDIT_CARD, dictMailData,
                                                                                                            orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, tenantId, dicMessageParam);
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }
        #endregion

        #region UAT-2930
        public static void SendMailOnTwoFactorAuthentication(Entity.OrganizationUser orgUserNew, Int32 tenantId)
        {
            try
            {
                if (!orgUserNew.IsNullOrEmpty())
                {
                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(orgUserNew.FirstName, " ", orgUserNew.LastName));
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, orgUserNew.Organization.Tenant.TenantName);
                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, WebSiteManager.GetInstitutionUrl(tenantId));

                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = string.Concat(orgUserNew.FirstName, " ", orgUserNew.LastName);
                    mockData.EmailID = orgUserNew.PrimaryEmailAddress;
                    mockData.ReceiverOrganizationUserID = orgUserNew.OrganizationUserID;

                    //Send mail
                    //SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_TWO_FACTOR_AUTHENTICATION_DISABLED, dictMailData, mockData, tenantId, 0);
                    SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_TWO_FACTOR_AUTHENTICATION_DISABLED, mockData, dictMailData, tenantId, 0, null);

                    //Send Message
                    SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_TWO_FACTOR_AUTHENTICATION_DISABLED, dictMailData, orgUserNew.OrganizationUserID, tenantId);
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
        #endregion

        public static Int32? SendUpdatedApplicantRequirementNotification(CommunicationSubEvents communicationSubEvents, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, List<CommunicationTemplateContract> lstcommunicationTemplateContract = null)
        {
            return SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, AppConsts.NONE, -1, null, null, true, false, null, true, lstcommunicationTemplateContract, null);
        }

        #region UAT-3077
        public static void SendMailForItemPayment(Entity.OrganizationUser orgUserNew, String amount, String orderDate, String orderNo, String itemName, Int32 tenantID)
        {
            try
            {
                if (!orgUserNew.IsNullOrEmpty())
                {
                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(orgUserNew.FirstName, " ", orgUserNew.LastName));
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, orgUserNew.Organization.Tenant.TenantName);
                    //  dictMailData.Add(EmailFieldConstants.APPLICATION_URL, WebSiteManager.GetInstitutionUrl(tenantId));

                    dictMailData.Add(EmailFieldConstants.ITEM_NAME, itemName);
                    dictMailData.Add(EmailFieldConstants.ORDER_NO, orderNo);
                    dictMailData.Add(EmailFieldConstants.ORDER_DATE, orderDate);
                    dictMailData.Add(EmailFieldConstants.AMOUNT_DUE, amount);
                    //  dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, "PACKAGE_NAME");
                    //  dictMailData.Add(EmailFieldConstants.CATEGORY_NAME,"CATEGORY_NAME" );
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = String.Concat(orgUserNew.FirstName, " ", orgUserNew.LastName);
                    mockData.EmailID = orgUserNew.PrimaryEmailAddress;
                    mockData.ReceiverOrganizationUserID = orgUserNew.OrganizationUserID;

                    //Send mail
                    SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_ITEM_PAYMENT, mockData, dictMailData, tenantID, 0, null);

                    //Send Message
                    SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_ITEM_PAYMENT, dictMailData, orgUserNew.OrganizationUserID, tenantID);
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
        #endregion

        #region UAT-3112
        public static Int32 SendBadgeFormNotificationsWithAttachment(Int32 tenantId, INTSOF.UI.Contract.ComplianceOperation.BadgeFormNotificationDataContract badgeFormNotificationDataContract, List<ApplicantDocument> lstApplicantDocument, Int32 currentLoggedInUser)
        {
            Int32? systemCommunicationID = null;

            Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();


            //Create Dictionary for Mail And Message Data
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(badgeFormNotificationDataContract.ApplicantFirstName, " ", badgeFormNotificationDataContract.ApplicantLastName));
            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, badgeFormNotificationDataContract.PackageName);
            dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, badgeFormNotificationDataContract.CategoryName);
            dictMailData.Add(EmailFieldConstants.ITEM_NAME, badgeFormNotificationDataContract.ItemName);

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = string.Concat(badgeFormNotificationDataContract.ApplicantFirstName, " ", badgeFormNotificationDataContract.ApplicantLastName);
            mockData.EmailID = badgeFormNotificationDataContract.ApplicantEmailID;
            mockData.ReceiverOrganizationUserID = badgeFormNotificationDataContract.AppOrgUserID;

            //Send mail
            //SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_TWO_FACTOR_AUTHENTICATION_DISABLED, dictMailData, mockData, tenantId, 0);
            systemCommunicationID = SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_BADGE_FORM_ATTACHMENT, mockData, dictMailData, tenantId, badgeFormNotificationDataContract.SelectedNodeID, null, null, false, false, null, true, null, null, badgeFormNotificationDataContract.RotationHierarchyIds);

            systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;

            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
            String docAttachmentTypeCode = DocumentAttachmentType.APPLICANT_DOCUMENT.GetStringValue();
            Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

            String documentName = String.Empty;

            //Save Mail Attachment
            foreach (ApplicantDocument appDoc in lstApplicantDocument)
            {
                Int32? sysCommAttachmentID = null;
                if (systemCommunicationID != null)
                {
                    SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                    sysCommAttachment.SCA_OriginalDocumentID = appDoc.ApplicantDocumentID;
                    sysCommAttachment.SCA_OriginalDocumentName = appDoc.FileName;
                    sysCommAttachment.SCA_DocumentPath = appDoc.DocumentPath;
                    sysCommAttachment.SCA_DocumentSize = appDoc.Size.HasValue ? appDoc.Size.Value : 0;
                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                    sysCommAttachment.SCA_TenantID = tenantId;
                    sysCommAttachment.SCA_IsDeleted = false;
                    sysCommAttachment.SCA_CreatedBy = currentLoggedInUser;
                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                    sysCommAttachment.SCA_ModifiedBy = null;
                    sysCommAttachment.SCA_ModifiedOn = null;

                    sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                }

                Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                List<ADBMessageDocument> messageDocument = new List<ADBMessageDocument>();

                ADBMessageDocument documentData = new ADBMessageDocument();
                documentData.DocumentName = appDoc.DocumentPath;
                documentData.OriginalDocumentName = appDoc.FileName;
                documentData.DocumentSize = appDoc.Size.Value;
                documentData.SystemCommunicationAttachmentID = sysCommAttachmentID;
                messageDocument.Add(documentData);

                attachedFiles = MessageManager.SaveDocumentAndGetDocumentId(messageDocument);
                if (!attachedFiles.IsNullOrEmpty())
                {
                    attachedFiles.ForEach(a => documentName += a.Key.ToString() + ";");
                }
            }
            dicMessageParam.Add("DocumentName", documentName);
            dicMessageParam.Add("IgnoreSpecificTemplate", true);
            //Send Message
            Guid? messageId = CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_BADGE_FORM_ATTACHMENT, dictMailData, badgeFormNotificationDataContract.AppOrgUserID, tenantId, dicMessageParam);

            return systemCommunicationID.IsNullOrEmpty() ? AppConsts.NONE : systemCommunicationID.Value;
        }


        #endregion

        #region UAT-2960

        public static Int32 SendAlumniAccessNotification(Int32 TenantId, OrganizationUserAlumniAccess organizationUserAlumniAccess, Int32 currentLoggedInUser)
        {
            Int32? systemCommunicationID = null;



            var queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.QUERY_STRING_ALUMNI_TOKEN , Convert.ToString(organizationUserAlumniAccess.OUAA_SecurityToken)}
                                                                    //{AppConsts.QUERY_STRING_USER_TYPE_CODE , inviteeUserTypeCode},
                                                                    ////UAT-2519
                                                                    //{"IsSearchedShare",Convert.ToString(true)}
                                                                 };

            Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(organizationUserAlumniAccess.OUAA_TenantID);
            String applicationUrl = String.Empty;
            if (webSite.IsNotNull() && webSite.WebSiteID != AppConsts.NONE)
            {
                applicationUrl = webSite.URL;
            }
            else
            {
                webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
                applicationUrl = webSite.URL;
            }
            if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                applicationUrl = string.Concat("http://", applicationUrl.Trim());
            }

            var url = String.Format(applicationUrl + "?args={0}", queryString.ToEncryptedQueryString());


            //Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();

            //Create Dictionary for Mail And Message Data
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(organizationUserAlumniAccess.OrganizationUser.FirstName, " ", organizationUserAlumniAccess.OrganizationUser.LastName));
            dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, url);

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = string.Concat(organizationUserAlumniAccess.OrganizationUser.FirstName, " ", organizationUserAlumniAccess.OrganizationUser.LastName);
            mockData.EmailID = organizationUserAlumniAccess.OrganizationUser.PrimaryEmailAddress;
            mockData.ReceiverOrganizationUserID = organizationUserAlumniAccess.OrganizationUser.OrganizationUserID;

            //Send mail
            systemCommunicationID = SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_ALUMNI_ACCESS_DUE, mockData, dictMailData, TenantId, 0, null);

            //Send Message
            SaveMessageContent(CommunicationSubEvents.NOTIFICATION_ALUMNI_ACCESS_DUE, dictMailData, organizationUserAlumniAccess.OrganizationUser.OrganizationUserID, TenantId);

            //systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;

            return systemCommunicationID.IsNullOrEmpty() ? AppConsts.NONE : systemCommunicationID.Value;
        }


        public static Int32 SendEmailForAvalableAlumniPacakge(AlumniPackageSubscription alumniPackageSubscription)//,// Int32 currentLoggedInUser)
        {
            Int32? systemCommunicationID = null;
            PackageSubscription packageSubscription = ComplianceDataManager.GetPackageSubscriptionByID(alumniPackageSubscription.TarTenantID, alumniPackageSubscription.TarPackageSubscriptionID);

            if (packageSubscription.IsNullOrEmpty() && packageSubscription.Order.IsNotNull() && packageSubscription.OrganizationUser.IsNotNull() && packageSubscription.CompliancePackage.IsNotNull())
                return AppConsts.NONE;

            DateTime subscriptionStartDate = DateTime.Today;
            int subscriptionDuration;

            subscriptionDuration = packageSubscription.Order.SubscriptionYear.IsNotNull() ? ((Int32)packageSubscription.Order.SubscriptionYear * 12) : 0;
            subscriptionDuration += packageSubscription.Order.SubscriptionMonth.IsNotNull() ? (Int32)packageSubscription.Order.SubscriptionMonth : 0;
            if (packageSubscription.ExpiryDate.IsNotNull())
            {
                subscriptionStartDate = ((DateTime)packageSubscription.ExpiryDate).AddMonths(-subscriptionDuration);
            }

            Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(alumniPackageSubscription.TarTenantID);
            String applicationUrl = String.Empty;
            if (webSite.IsNotNull() && webSite.WebSiteID != AppConsts.NONE)
            {
                applicationUrl = webSite.URL;
            }
            else
            {
                webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
                applicationUrl = webSite.URL;
            }
            if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                applicationUrl = string.Concat("http://", applicationUrl.Trim());
            }
            String packageName = !packageSubscription.CompliancePackage.PackageLabel.IsNullOrEmpty() ? packageSubscription.CompliancePackage.PackageLabel
                                                                                                        : packageSubscription.CompliancePackage.PackageName;
            //UAT-3254
            Int32 hierarchyNodeID = packageSubscription.Order.IsNullOrEmpty() && packageSubscription.Order.SelectedNodeID.IsNullOrEmpty() ? 0 : packageSubscription.Order.SelectedNodeID.Value;

            //Create Dictionary for Mail And Message Data
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(packageSubscription.OrganizationUser.FirstName, " ", packageSubscription.OrganizationUser.LastName));
            dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
            dictMailData.Add(EmailFieldConstants.SUBSCRIPTION_START_DATE, subscriptionStartDate.ToString("MM/dd/yyyy"));
            dictMailData.Add(EmailFieldConstants.SUBSCRIPTION_END_DATE, packageSubscription.ExpiryDate.Value.ToString("MM/dd/yyyy"));
            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, packageName);


            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = string.Concat(packageSubscription.OrganizationUser.FirstName, " ", packageSubscription.OrganizationUser.LastName);
            mockData.EmailID = packageSubscription.OrganizationUser.PrimaryEmailAddress;
            mockData.ReceiverOrganizationUserID = packageSubscription.OrganizationUser.OrganizationUserID;

            //Send mail
            systemCommunicationID = SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_ALUMNI_PACKAGE_AVALIABILITY, mockData, dictMailData, alumniPackageSubscription.TarTenantID, hierarchyNodeID, null);

            //Send Message
            SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_ALUMNI_PACKAGE_AVALIABILITY, dictMailData, packageSubscription.OrganizationUser.OrganizationUserID, alumniPackageSubscription.TarTenantID);

            // systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;

            return systemCommunicationID.IsNullOrEmpty() ? AppConsts.NONE : systemCommunicationID.Value;
        }

        #endregion

        #region UAT-3108

        public static void SendMailOnUpdationInRotationdetails(INTSOF.ServiceDataContracts.Modules.ClinicalRotation.RotationDetailFieldChanges rotationDetailFieldChanges)
        {
            try
            {
                if (!rotationDetailFieldChanges.IsNullOrEmpty() && !rotationDetailFieldChanges.RotationID.IsNullOrEmpty() && rotationDetailFieldChanges.RotationID > AppConsts.NONE)
                {
                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                    dictMailData.Add(EmailFieldConstants.MODIFIED_BY_NAME, rotationDetailFieldChanges.ModifiedByName);
                    dictMailData.Add(EmailFieldConstants.COMPLIO_ID, rotationDetailFieldChanges.ComplioID);
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, rotationDetailFieldChanges.TenantName);
                    dictMailData.Add(EmailFieldConstants.ROTATION_FIELD_CHANGES, rotationDetailFieldChanges.RotationFieldChanges.ToString());

                    String notificationtypeCode = AgencyUserNotificationLookup.NOTIFICATION_FOR_UPDATED_ROTATION_DETAILS.GetStringValue();

                    List<INTSOF.UI.Contract.ProfileSharing.AgencyUserInfoContract> lstAgencyUserInfoContract
                                                                            = BALUtils.GetProfileSharingRepoInstance().GetAgencyUserListInRotationBasedOnPermission(rotationDetailFieldChanges.RotationID, notificationtypeCode, rotationDetailFieldChanges.TenantID);

                    List<CommunicationTemplateContract> lstAgencyUser = new List<CommunicationTemplateContract>();
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();

                    if (!lstAgencyUserInfoContract.IsNullOrEmpty())
                    {

                        lstAgencyUser = lstAgencyUserInfoContract.Select(s => new CommunicationTemplateContract
                        {
                            RecieverEmailID = s.PrimaryEmailAddress,
                            RecieverName = s.FullName,
                            CurrentUserId = rotationDetailFieldChanges.CurrentLoggedInUserId,
                            ReceiverOrganizationUserId = s.OrgUserID,
                            IsToUser = true
                        }).ToList();

                    }
                    if (lstAgencyUser.IsNullOrEmpty() && lstAgencyUser.Count == AppConsts.NONE)
                    {
                        mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                        mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                        mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                    }
                    Int32? systemCommunicationID = CommunicationManager.SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_FIELD_CHANGES, mockData, dictMailData, rotationDetailFieldChanges.TenantID, -1, null, null, true, false, null, true, lstAgencyUser, null, rotationDetailFieldChanges.HierarchyNodeIDs, rotationDetailFieldChanges.RotationID);


                    ////Send mail
                    //CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_FIELD_CHANGES, dictMailData, mockData, rotationDetailFieldChanges.TenantID, -1, null, null, false, false, null, rotationDetailFieldChanges.HierarchyNodeIDs);

                    //Send Message
                    // SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_FIELD_CHANGES, dictMailData, rotationDetailFieldChanges.CurrentLoggedInUserId, rotationDetailFieldChanges.TenantID, null);
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

        #endregion

        #region UAT-4561
        public static void SendMailOnUpdationInRotationEndDate(INTSOF.ServiceDataContracts.Modules.ClinicalRotation.ClinicalRotationDetailContract clinicalRotationDetailContract, INTSOF.ServiceDataContracts.Modules.ClinicalRotation.RotationDetailFieldChanges rotationDetailFieldChanges)
        {
            try
            {
                if (!rotationDetailFieldChanges.IsNullOrEmpty() && !rotationDetailFieldChanges.RotationID.IsNullOrEmpty() && rotationDetailFieldChanges.RotationID > AppConsts.NONE)
                {
                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                    dictMailData.Add(EmailFieldConstants.MODIFIED_BY_NAME, rotationDetailFieldChanges.ModifiedByName);
                    dictMailData.Add(EmailFieldConstants.COMPLIO_ID, rotationDetailFieldChanges.ComplioID);
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, rotationDetailFieldChanges.TenantName);
                    dictMailData.Add(EmailFieldConstants.ROTATION_END_DATE, clinicalRotationDetailContract.EndDate.Value.ToString("MM/dd/yyyy"));

                    String notificationtypeCode = AgencyUserNotificationLookup.NOTIFICATION_FOR_ROTATION_END_DATE_CHANGE.GetStringValue();

                    List<AgencyUserInfoContract> lstAgencyUserInfoContract = BALUtils.GetProfileSharingRepoInstance().GetAgencyUserListInRotationBasedOnPermission(rotationDetailFieldChanges.RotationID, notificationtypeCode, rotationDetailFieldChanges.TenantID);

                    List<CommunicationTemplateContract> lstAgencyUser = new List<CommunicationTemplateContract>();
                    CommunicationMockUpData mockData = new CommunicationMockUpData();

                    if (!lstAgencyUserInfoContract.IsNullOrEmpty())
                    {
                        lstAgencyUser = lstAgencyUserInfoContract.Select(s => new CommunicationTemplateContract
                        {
                            RecieverEmailID = s.PrimaryEmailAddress,
                            RecieverName = s.FullName,
                            CurrentUserId = rotationDetailFieldChanges.CurrentLoggedInUserId,
                            ReceiverOrganizationUserId = s.OrgUserID,
                            IsToUser = true
                        }).ToList();

                    }
                    if (lstAgencyUser.IsNullOrEmpty() && lstAgencyUser.Count == AppConsts.NONE)
                    {
                        mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                        mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                        mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                    }
                    Int32? systemCommunicationID = SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_END_DATE_CHANGE, mockData, dictMailData, rotationDetailFieldChanges.TenantID, -1, null, null, true, false, null, true, lstAgencyUser, null, rotationDetailFieldChanges.HierarchyNodeIDs, rotationDetailFieldChanges.RotationID);
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
        #endregion

        #region UAT-3222

        public static void SendMailWhenStudentDroppedFromRotation(INTSOF.ServiceDataContracts.Modules.ClinicalRotation.RotationStudentDropped rotationStudentDropped)
        {
            Int32 rotationId = rotationStudentDropped.RotationID;

            //Create Dictionary
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.STUDENT_NAMES, rotationStudentDropped.RemovedApplicantNames);
            dictMailData.Add(EmailFieldConstants.COMPLIO_ID, rotationStudentDropped.ComplioID);
            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, rotationStudentDropped.TenantName);
            //dictMailData.Add(EmailFieldConstants.ROTATION_FIELD_CHANGES, rotationDetailFieldChanges.RotationFieldChanges.ToString());

            if (rotationId > AppConsts.NONE)
            {
                String notificationtypeCode = AgencyUserNotificationLookup.NOTIFICATION_FOR_STUDENT_DROPPED_FROM_ROTATION.GetStringValue();

                List<INTSOF.UI.Contract.ProfileSharing.AgencyUserInfoContract> lstAgencyUserInfoContract
                                                                                 = BALUtils.GetProfileSharingRepoInstance().GetAgencyUserListInRotationBasedOnPermission(rotationId, notificationtypeCode, rotationStudentDropped.TenantID);

                List<CommunicationTemplateContract> lstAgencyUser = new List<CommunicationTemplateContract>();
                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();

                if (!lstAgencyUserInfoContract.IsNullOrEmpty())
                {

                    lstAgencyUser = lstAgencyUserInfoContract.Select(s => new CommunicationTemplateContract
                    {
                        RecieverEmailID = s.PrimaryEmailAddress,
                        RecieverName = s.FullName,
                        CurrentUserId = rotationStudentDropped.CurrentLoggedInUserId,
                        ReceiverOrganizationUserId = s.OrgUserID,
                        IsToUser = true
                    }).ToList();

                }
                if (lstAgencyUser.IsNullOrEmpty() && lstAgencyUser.Count == AppConsts.NONE)
                {
                    mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                    mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                    mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                }
                Int32? systemCommunicationID = CommunicationManager.SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_STUDENT_DROPPED_FROM_ROTATION, mockData, dictMailData, rotationStudentDropped.TenantID, -1, null, null, true, false, null, true, lstAgencyUser, null, rotationStudentDropped.HierarchyNodeIDs, rotationId);

            }
        }
        #endregion

        #region UAT-3261: Badge Form Enhancements
        public static List<EmailDetails> GetUserEmails(Int32 userId, Int32 pageSize, Int32 pageIndex, String defaultExpression, Boolean isSortDirectionDescending, Int32 ApplicantDashboard = 0)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetUserEmails(userId, pageSize, pageIndex, defaultExpression, isSortDirectionDescending, ApplicantDashboard);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<SystemCommunicationDelivery> GetSysCommDeliveryIds(Int32 SystemCommunicationId)
        {
            try
            {
                return BALUtils.GetCommunicationRepoInstance().GetSysCommDeliveryIds(SystemCommunicationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        #region UAT-3453
        public static List<lkpScreeningResultType> GetScreeningResultType()
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpScreeningResultType>().ToList();
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

        #region CBI_CABS
        public static Int32? SentMailMessageNotification(CommunicationSubEvents CommSubEvnt, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, int ApplicantOrgUserId, int TenantID, int HeirarchyNodeId, Boolean IsEnroller = false)
        {
            ////Send mail and SMS
            return SaveMailCommunicationContract(CommSubEvnt, mockData, dictMailData, TenantID, HeirarchyNodeId, null, null, false, false, null, true, null, null, null, null, IsEnroller);


        }

        public static Int32? SendServiceStatusChangeMailMessage(string selectedStatus, Entity.OrganizationUser organizationUser, int TenantID, Order orderDetails, string serviceName, DateTime? ShipDate = null, string trackingNumber = null)
        {
            Int32 _heirarchyNodeId = FingerPrintDataManager.GetOrderHeirarchyNodeId(TenantID, orderDetails.OrderID);                        

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = string.Concat(organizationUser.FirstName, " ", organizationUser.LastName);
            mockData.EmailID = organizationUser.PrimaryEmailAddress;
            mockData.ReceiverOrganizationUserID = organizationUser.OrganizationUserID;

            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(organizationUser.FirstName, " ", organizationUser.LastName));
            dictMailData.Add(EmailFieldConstants.Order_Number, orderDetails.OrderNumber);
            dictMailData.Add(EmailFieldConstants.SERVICE_NAME, serviceName);
            dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, organizationUser.OrganizationUserID);
            dictMailData.Add(EmailFieldConstants.TENANT_ID, TenantID);

            CommunicationSubEvents ObjEnrollerCommunicationSubEvents = new CommunicationSubEvents();
            if (selectedStatus == "Returned to Sender")
            {
                ObjEnrollerCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_SERVICE_STATUS_RETURN_TO_SENDER;                     
                dictMailData.Add(EmailFieldConstants.Ship_Date, String.Format("{0:dd/MM/yyyy}", ShipDate) );
            }
            else if (selectedStatus == "Shipped")
            {
                ObjEnrollerCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_SERVICE_STATUS_SHIPPED;
                if (trackingNumber.IsNullOrEmpty())
                    trackingNumber ="N/A";
                dictMailData.Add(EmailFieldConstants.Tracking_Number, trackingNumber);
                dictMailData.Add(EmailFieldConstants.Ship_Date, String.Format("{0:dd/MM/yyyy}", ShipDate));
            }
            else if (selectedStatus == "Reject Service")
            {
                ObjEnrollerCommunicationSubEvents = CommunicationSubEvents.NOTIFICATION_FOR_SERVICE_STATUS_REJECTED;
            }           

            return CommunicationManager.SentMailMessageNotification(ObjEnrollerCommunicationSubEvents, mockData, dictMailData, organizationUser.OrganizationUserID, TenantID, _heirarchyNodeId, true);
        }

        public static Int32? SendModifyShippingNotification(Entity.OrganizationUser organizationUser, Int32 TenantID, Int32 OrderID, string OrderNumber, PreviousAddressContract mailingAddress)
        {
            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = string.Concat(organizationUser.FirstName, " ", organizationUser.LastName);
            mockData.EmailID = organizationUser.PrimaryEmailAddress;
            mockData.ReceiverOrganizationUserID = organizationUser.OrganizationUserID;

            Dictionary<String, object> dictMailData = new Dictionary<String, object>();
            dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(organizationUser.FirstName, " ", organizationUser.LastName));
            dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, organizationUser.OrganizationUserID);
            dictMailData.Add(EmailFieldConstants.TENANT_ID, TenantID);
            dictMailData.Add(EmailFieldConstants.Order_Number, OrderNumber);
            if(!mailingAddress.StateName.IsNullOrEmpty())
            {
                dictMailData.Add(EmailFieldConstants.Shipping_Address, string.Concat(mailingAddress.Address1, ",", mailingAddress.CityName, ",", mailingAddress.StateName, ",", mailingAddress.Country));
            }
            else {
                dictMailData.Add(EmailFieldConstants.Shipping_Address, string.Concat(mailingAddress.Address1, ",", mailingAddress.CityName, ",", mailingAddress.Country));
            }            
            dictMailData.Add(EmailFieldConstants.Shipping_Method, mailingAddress.MailingOptionPrice);

            Int32 _heirarchyNodeId = FingerPrintDataManager.GetOrderHeirarchyNodeId(TenantID, OrderID);
            return CommunicationManager.SentMailMessageNotification(CommunicationSubEvents.NOTIFICATION_FOR_MODIFYING_SHIPPING_ADDRESS, mockData, dictMailData, organizationUser.OrganizationUserID, TenantID, _heirarchyNodeId);
        }
        #endregion

        #region UAT-3805
        public static Boolean SendItemDocumentNotificationMailToAgencyUser(Int32 tenantId, List<ItemDocumentNotificationDataContract> lstItemDocNotificationData, Int32 currentLoggedInUserID)
        {
            try
            {
                String sharedUserUrl = String.Empty;
                if (!System.Web.Configuration.WebConfigurationManager.AppSettings["shareduserloginurl"].IsNullOrEmpty())
                {
                    sharedUserUrl = string.Concat("http://", Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["shareduserloginurl"]));
                }
                String tenantName = String.Empty;
                Entity.Tenant tenant = SecurityManager.GetTenant(tenantId);
                if (!tenant.IsNullOrEmpty())
                    tenantName = tenant.TenantName;

                List<Int32> lstSystemCommunicationIds = new List<Int32>();

                List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                String docAttachmentTypeCode = DocumentAttachmentType.APPLICANT_DOCUMENT.GetStringValue();
                Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                    Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                if (!lstItemDocNotificationData.IsNullOrEmpty())
                {
                    List<Int32> lstCategoryDataID = lstItemDocNotificationData.Select(con => con.CategoryDataID).Distinct().ToList();
                    if (!lstCategoryDataID.IsNullOrEmpty())
                    {
                        foreach (Int32 categoryDataId in lstCategoryDataID)
                        {
                            List<ItemDocumentNotificationDataContract> lstCategorySpecific = lstItemDocNotificationData.Where(con => con.CategoryDataID == categoryDataId).ToList();
                            if (!lstCategorySpecific.IsNullOrEmpty())
                            {
                                String rotationDetails = String.Empty;
                                if (!lstCategorySpecific[0].RotationID.IsNullOrEmpty())
                                {
                                    Entity.ClientEntity.ClinicalRotation clinicalRotation = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetClinicalRotation(lstCategorySpecific[0].RotationID.Value);
                                    if (!clinicalRotation.IsNullOrEmpty())
                                    {
                                        rotationDetails = GenerateRotationDetailsForMail(clinicalRotation);
                                    }
                                }

                                Int32? systemCommunicationID = null;

                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, lstCategorySpecific[0].ApplicantFirstName + " " + lstCategorySpecific[0].ApplicantLastName);
                                dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, lstCategorySpecific[0].CategoryName);
                                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, rotationDetails);
                                dictMailData.Add(EmailFieldConstants.SHARED_USER_URL, sharedUserUrl);


                                List<CommunicationTemplateContract> lstAgencyUser = new List<CommunicationTemplateContract>();
                                CommunicationMockUpData mockData = new CommunicationMockUpData();

                                lstAgencyUser = lstCategorySpecific.Select(s => new CommunicationTemplateContract
                                {
                                    RecieverEmailID = s.AgencyAdminEmail,
                                    RecieverName = s.AgencyAdminName,
                                    CurrentUserId = currentLoggedInUserID,
                                    ReceiverOrganizationUserId = s.AgencyOrgUserID,
                                    // IsToUser = true
                                }).ToList();



                                if (lstAgencyUser.IsNullOrEmpty() && lstAgencyUser.Count == AppConsts.NONE)
                                {
                                    mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                                    mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                    mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                                }

                                Int32 selectedNodeID = AppConsts.NONE;
                                String rotationHierarchyNodeIDs = String.Empty; ;
                                if (!lstCategorySpecific[0].RequestTypeCode.IsNullOrEmpty())
                                {
                                    if (lstCategorySpecific[0].RequestTypeCode == lkpUseTypeEnum.ROTATION.GetStringValue())
                                        rotationHierarchyNodeIDs = lstCategorySpecific[0].HierarchyIDs;
                                    else
                                        selectedNodeID = Convert.ToInt32(lstCategorySpecific[0].HierarchyIDs);
                                }


                                systemCommunicationID = SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_TO_AGENCY_USER_FOR_E_SIGN_COMPLETED_DOCUMENT, mockData, dictMailData
                                    , tenantId, selectedNodeID, null, null, false, false, null, true, lstAgencyUser, null, rotationHierarchyNodeIDs);



                                systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;

                                Int32? sysCommAttachmentID = null;
                                if (systemCommunicationID != null)
                                {
                                    lstSystemCommunicationIds.Add(systemCommunicationID.Value);

                                    SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                                    sysCommAttachment.SCA_OriginalDocumentID = lstCategorySpecific[0].ApplicantDocumentID;
                                    sysCommAttachment.SCA_OriginalDocumentName = lstCategorySpecific[0].DocumentName;
                                    sysCommAttachment.SCA_DocumentPath = lstCategorySpecific[0].DocumentPath;
                                    sysCommAttachment.SCA_DocumentSize = lstCategorySpecific[0].DocumentSize;
                                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                    sysCommAttachment.SCA_TenantID = tenantId;
                                    sysCommAttachment.SCA_IsDeleted = false;
                                    sysCommAttachment.SCA_CreatedBy = currentLoggedInUserID;
                                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                    sysCommAttachment.SCA_ModifiedBy = null;
                                    sysCommAttachment.SCA_ModifiedOn = null;

                                    sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                }
                            }
                        }
                    }
                }
                if (!lstSystemCommunicationIds.IsNullOrEmpty() && lstSystemCommunicationIds.Count > AppConsts.NONE)
                    return true;
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
                throw ex;
            }
        }

        private static String GenerateRotationDetailsForMail(ClinicalRotation clinicalRotation)
        {
            System.Text.StringBuilder _sbRotationDetails = new System.Text.StringBuilder();
            _sbRotationDetails.Append("<h4><i>Rotation Details:</i></h4>");
            _sbRotationDetails.Append("<div style='line-height:21px'>");
            _sbRotationDetails.Append("<ul style='list-style-type: disc'>");

            //if (!rotationDetailsContract.AgencyName.IsNullOrEmpty())
            //{
            //    _sbRotationDetails.Append("<li><b>" + "Agency Name: </b>" + clinicalRotation.ClinicalRotationAgencies + "</li>");
            //}
            if (!clinicalRotation.CR_ComplioID.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Complio ID: </b>" + clinicalRotation.CR_ComplioID + "</li>");
            }
            if (!clinicalRotation.CR_RotationName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Rotation Name: </b>" + clinicalRotation.CR_RotationName + "</li>");
            }
            if (!clinicalRotation.CR_Department.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Department: </b>" + clinicalRotation.CR_Department + "</li>");
            }
            if (!clinicalRotation.CR_Program.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Program: </b>" + clinicalRotation.CR_Program + "</li>");
            }
            if (!clinicalRotation.CR_Course.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Course: </b>" + clinicalRotation.CR_Course + "</li>");
            }
            if (!clinicalRotation.CR_Term.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Term: </b>" + clinicalRotation.CR_Term + "</li>");
            }
            if (!clinicalRotation.CR_TypeSpecialty.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Type/Specialty: </b>" + clinicalRotation.CR_TypeSpecialty + "</li>");
            }
            if (!clinicalRotation.CR_UnitFloorLoc.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Unit/Floor or Location: </b>" + clinicalRotation.CR_UnitFloorLoc + "</li>");
            }
            if (!clinicalRotation.CR_NoOfHours.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "# of Recommended Hours: </b>" + clinicalRotation.CR_NoOfHours + "</li>");
            }
            if (!!clinicalRotation.ClinicalRotationDays.IsNullOrEmpty())
            {
                List<Int32> lstExistingWeekDaysIDs = clinicalRotation.ClinicalRotationDays.Where(con => !con.CRD_IsDeleted && !con.CRD_WeekDayID.IsNullOrEmpty()).Select(sel => sel.CRD_WeekDayID.Value).ToList();
                String daysNames = String.Empty;
                if (!lstExistingWeekDaysIDs.IsNullOrEmpty())
                {
                    List<Entity.SharedDataEntity.lkpWeekDay> lkpweekdays = ComplianceDataManager.GetWeekDaysList();
                    if (!lkpweekdays.IsNullOrEmpty())
                    {
                        List<String> lstWeekDayNames = lkpweekdays.Where(con => lstExistingWeekDaysIDs.Contains(con.WD_ID) && !con.WD_IsDeleted).OrderBy(c => c.WD_ID).Select(sel => sel.WD_Name).ToList();
                        if (!lstWeekDayNames.IsNullOrEmpty())
                            daysNames = String.Join(", ", lstWeekDayNames);
                    }
                }
                if (!daysNames.IsNullOrEmpty())
                    _sbRotationDetails.Append("<li><b>" + "Days: </b>" + daysNames + "</li>");
            }
            if (!clinicalRotation.CR_RotationShift.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Shift: </b>" + clinicalRotation.CR_RotationShift + "</li>");
            }
            //if (!clinicalRotation.Time.IsNullOrEmpty() && clinicalRotation.Time != "-")
            //{
            //    _sbRotationDetails.Append("<li><b>" + "Time: </b>" + clinicalRotation.Time + "</li>");
            //}
            if (!clinicalRotation.CR_StartDate.IsNullOrEmpty() && !clinicalRotation.CR_EndDate.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Dates: </b>" + Convert.ToDateTime(clinicalRotation.CR_StartDate).ToString("MM/dd/yyyy") + " - " + Convert.ToDateTime(clinicalRotation.CR_EndDate).ToString("MM/dd/yyyy") + "</li>");
            }
            _sbRotationDetails.Append("</ul>");
            _sbRotationDetails.Append("</div>");
            return Convert.ToString(_sbRotationDetails);



            return string.Empty;
        }

        #endregion

        #region UAT-3820

        public static Int32? SendMailForPendingReceivedFromStudentServiceFormStatus(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, List<CommunicationTemplateContract> lstUser)
        {
            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();

            if (!lstUser.IsNullOrEmpty() && lstUser.Count > AppConsts.NONE)
            {
                return CommunicationManager.SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, AppConsts.NONE, AppConsts.NONE, null, null, false, false, null, true, lstUser, null, null);
            }
            return AppConsts.NONE;
        }
        #endregion

        #region UAT-4015 :- Send mail functionality for inst/Prec on package compliant status.

        //Step 1:- Get all RPS Data for inst/Preceptor of subscription status compliant.

        //Send mail to inst/preceptor

        public static Int32? SendEmailToInstructorPrecPnComplaintStaus(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, CommunicationMockUpData mockData, Int32 tenantId, Int32 hierarchyNodeID, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Boolean overrideCcBcc = false, List<ExternalCopyUsersContract> lstCCUsers = null, String rotationHierarchyID = null, Int32? rotationID = AppConsts.NONE)
        {
            try
            {
                return SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, tenantId, hierarchyNodeID, entityID, entityTypeCode, ignoreSubscriptionSeting, overrideCcBcc, null, true, null, lstCCUsers, rotationHierarchyID, rotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }


        #endregion

        #region UAT-3957
        public static void SendMailOnRequirementItemRejection(List<INTSOF.ServiceDataContracts.Modules.ClinicalRotation.RequirementItemRejectionContract> lstRequirementItemRejectionContract, Int32 tenantID)
        {
            Entity.WebSite webSite = WebSiteManager.GetWebSiteDetail(tenantID);
            String applicationUrl = String.Empty;
            if (webSite.IsNotNull() && webSite.WebSiteID != AppConsts.NONE)
            {
                applicationUrl = webSite.URL;
            }
            else
            {
                webSite = WebSiteManager.GetWebSiteDetail(SecurityManager.DefaultTenantID);
                applicationUrl = webSite.URL;
            }
            if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                applicationUrl = string.Concat("http://", applicationUrl.Trim());
            }

            foreach (var requirementItemRejectionContract in lstRequirementItemRejectionContract)
            {
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, requirementItemRejectionContract.ApplicantName);
                dictMailData.Add(EmailFieldConstants.REQUIREMENT_ITEM_NAME, requirementItemRejectionContract.RequirementItemName);
                dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, requirementItemRejectionContract.RequirementCategoryName);
                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, requirementItemRejectionContract.AgencyName);
                dictMailData.Add(EmailFieldConstants.REJECTION_REASON, requirementItemRejectionContract.ItemRejectionReason);
                dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();

                mockData.UserName = requirementItemRejectionContract.ApplicantName;
                mockData.EmailID = requirementItemRejectionContract.ApplicnatEmailAddress;
                mockData.ReceiverOrganizationUserID = requirementItemRejectionContract.ApplicantOrganizationUserId;

                SaveMailCommunicationContract(CommunicationSubEvents.NOTIFICATION_FOR_REQUIREMENT_ITEM_REJECTED, mockData, dictMailData, tenantID, -1, null, null, false, false, null, true, null, null, requirementItemRejectionContract.RotationHierachyIds);
            }
        }

        #endregion

        #region UAT-3795

        public static void SendWeeklyNonCompliantReport(Int32 backgroundProcessUserId, String tenantName, String fileName, String savedFilePath, byte[] excel,
            List<INTSOF.UI.Contract.ComplianceOperation.WeeklyNonCompliantReportDataContract> lstHavingSameHierarchyIds)
        {
            Int32? systemCommunicationID = null;
            Int16 attachmentType;
            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

            attachmentType = !docAttachmentType.IsNullOrEmpty()
                            ? Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == DocumentAttachmentType.MESSAGE_DOCUMENT.GetStringValue()).DAT_ID)
                            : Convert.ToInt16(AppConsts.NONE);


            List<String> subEventCodes = new List<String>();
            subEventCodes.Add(CommunicationSubEvents.WEEKLY_NON_COMPLIANT_APPLICANT_REPORT_NOTIFICATION.GetStringValue().ToLower());
            Int32 subEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
            List<CommunicationTemplatePlaceHolder> placeHoldersToFetch = CommunicationManager.GetTemplatePlaceHolders(subEventID);
            Int32 templateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(subEventID);

            //Contains info for mail subject and content
            INTSOF.UI.Contract.Templates.SystemEventTemplatesContract systemEventTemplate = TemplatesManager.GetTemplateDetails(templateId);

            Dictionary<String, String> dictMailData = new Dictionary<string, String>();
            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);


            SystemCommunication systemCommunication = new SystemCommunication();
            systemCommunication.SenderName = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME]);
            systemCommunication.SenderEmailID = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
            systemCommunication.Subject = systemEventTemplate.Subject;
            systemCommunication.CommunicationSubEventID = subEventID;
            systemCommunication.CreatedByID = backgroundProcessUserId;
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
            systemCommunicationDelivery.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
            systemCommunicationDelivery.RecieverEmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
            systemCommunicationDelivery.RecieverName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
            systemCommunicationDelivery.IsDispatched = false;
            systemCommunicationDelivery.IsCC = null;
            systemCommunicationDelivery.IsBCC = null;
            systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
            systemCommunicationDelivery.CreatedOn = DateTime.Now;
            systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);

            foreach (var havingSameHierarchyContract in lstHavingSameHierarchyIds)
            {
                SystemCommunicationDelivery systemCommunicationCCDelivery = new SystemCommunicationDelivery();
                systemCommunicationCCDelivery.SystemCommunicationTypeID = systemCommunication.SystemCommunicationID;
                systemCommunicationCCDelivery.ReceiverOrganizationUserID = havingSameHierarchyContract.OrgUserID;
                systemCommunicationCCDelivery.RecieverEmailID = havingSameHierarchyContract.EmailAddress;
                systemCommunicationCCDelivery.RecieverName = havingSameHierarchyContract.UserFullName;
                systemCommunicationCCDelivery.IsDispatched = false;
                systemCommunicationCCDelivery.IsCC = true;
                systemCommunicationCCDelivery.IsBCC = null;
                systemCommunicationCCDelivery.CreatedByID = systemCommunication.CreatedByID;
                systemCommunicationCCDelivery.CreatedOn = DateTime.Now;
                systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationCCDelivery);
            }

            systemCommunicationID = BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(systemCommunication);

            if (systemCommunicationID != null)
            {
                SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                sysCommAttachment.SCA_OriginalDocumentID = -1;
                sysCommAttachment.SCA_OriginalDocumentName = string.Concat(fileName, ".xls");

                sysCommAttachment.SCA_DocumentPath = savedFilePath;
                sysCommAttachment.SCA_DocumentSize = excel.Length;
                sysCommAttachment.SCA_DocAttachmentTypeID = attachmentType;
                sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                sysCommAttachment.SCA_IsDeleted = false;
                sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                sysCommAttachment.SCA_SystemCommunicationID = Convert.ToInt32(systemCommunicationID);

                CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
            }
        }

        #endregion

        #region UAT-3704

        public static List<Int32> GetSubEventsHavingAgencySpecificTemplates(Int32? languageId)
        {
            try
            {
                var defaultLanguageCode = CommunicationLanguages.DEFAULT.GetStringValue();
                var defaultLanguageId = GetLanguages().First(l => l.LAN_Code == defaultLanguageCode).LAN_ID;

                if (!languageId.HasValue)
                {
                    languageId = defaultLanguageId;
                }

                return BALUtils.GetCommunicationRepoInstance().GetSubEventsHavingAgencySpecificTemplates(languageId.Value);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<int>();
        }
        #endregion

        #region UAT 4398
        public static Int32? SendRotationAndMembersDetailsNotificationMail(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, CommunicationMockUpData mockData, Int32 tenantId, Int32 hierarchyNodeID, Int32? entityID = null, String entityTypeCode = null, Boolean ignoreSubscriptionSeting = false, Boolean overrideCcBcc = false, List<ExternalCopyUsersContract> lstCCUsers = null, String rotationHierarchyID = null, Int32? rotationID = AppConsts.NONE)
        {
            try
            {
                return SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, tenantId, hierarchyNodeID, entityID, entityTypeCode, ignoreSubscriptionSeting, overrideCcBcc, null, true, null, lstCCUsers, rotationHierarchyID, rotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        #endregion


        #region Notification to client admin user to confirm applicant profile submitted/transmitted (if applicant request sent).
        public static Int32? SentMailForApplicantProfileSubmitted(CommunicationSubEvents CommSubEvnt, CommunicationMockUpData mockData, Dictionary<String, object> dictMailData, int TenantId, int HeirarchyNodeId)
        {
            ////Send mail and SMS
            return SaveMailCommunicationContract(CommSubEvnt, mockData, dictMailData, TenantId, HeirarchyNodeId, null, null, false, false, null, true, null, null, null, null, false);
        }
        #endregion

        #region UAT-4613
        public static Int32? SendMailForInProcessAgencyFromStudentServiceFormStatus(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, CommunicationMockUpData mockdata, List<CommunicationTemplateContract> lstUser)
        {
            //Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();

            if (!lstUser.IsNullOrEmpty() && lstUser.Count > AppConsts.NONE)
            {
                return CommunicationManager.SaveMailCommunicationContract(communicationSubEvents, mockdata, dictMailData, AppConsts.NONE, AppConsts.NONE, null, null, false, false, null, true, lstUser, null, null);
            }
            return AppConsts.NONE;
        }
        #endregion

        #region UAT-4400
        public static Int32? SendStudentFallOutOfComplianceToAgencyUser(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData,
                                                                        CommunicationMockUpData mockdata, List<CommunicationTemplateContract> lstAgencyUsers)
        {
            return CommunicationManager.SaveMailCommunicationContract(communicationSubEvents, mockdata, dictMailData, AppConsts.NONE, AppConsts.NONE, null, null, false, false, null, false, lstAgencyUsers);
        }

        #endregion

        public static Int32? SendMailForFingerprintingExceededTAT(CommunicationSubEvents communicationSubEvents, Dictionary<String, object> dictMailData, List<CommunicationTemplateContract> lstUser)
        {
            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();

            if (!lstUser.IsNullOrEmpty() && lstUser.Count > AppConsts.NONE)
            {
                return CommunicationManager.SaveMailCommunicationContract(communicationSubEvents, mockData, dictMailData, AppConsts.NONE, AppConsts.NONE, null, null, false, false, null, true, lstUser, null, null);
            }
            return AppConsts.NONE;
        }
    }
}
