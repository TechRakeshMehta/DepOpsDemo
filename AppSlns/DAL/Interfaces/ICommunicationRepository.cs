using Entity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Interfaces
{
    public interface ICommunicationRepository
    {
        #region Communication Subscription Settings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="selectedCompositeEventKeys"></param>
        /// <param name="communicationTypeIds"></param>
        /// <param name="modifiedByID"></param>
        /// <param name="isCommit"></param>
        /// <param name="isSubscribedByUser"></param>
        /// <returns></returns>
        bool AddUserCommunicationSubscriptionSettings(
            Int32 organizationUserId,
            List<Int32> selectedNotificationEventIds,
            List<Int32> selectedReminderEventIds,
            Int32 notificationCommunicationTypeId,
            Int32 reminderCommunicationTypeId,
            List<lkpCommunicationEvent> communicationEvents,
            int modifiedByID,
            bool isCommit = true,
            bool isSubscribedByUser = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiedById"></param>
        /// <param name="selectedSubscriptionSettings"></param>
        /// <param name="unSelectedSubscriptionSettings"></param>
        /// <returns></returns>
        bool AddUserCommunicationSubscriptionSettings(Int32 modifiedById,
            List<UserCommunicationSubscriptionSetting> selectedSubscriptionSettings,
            List<UserCommunicationSubscriptionSetting> unSelectedSubscriptionSettings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userCommunicationSubscriptionSettings"></param>
        /// <returns></returns>
        bool AddUserCommunicationSubscriptionSettings(List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="communicationTypeIds"></param>
        /// <returns></returns>
        IEnumerable<UserCommunicationSubscriptionSetting> GetUserCommunicationSubscriptionSettings(Int32 organizationUserId, Int32 communicationTypeIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="communicationEventId"></param>
        /// <returns></returns>
        UserCommunicationSubscriptionSetting GetUserCommunicationSubscriptionSettings(Int32 organizationUserId, Int32 communicationTypeId, Int32 communicationEventId);

        #endregion

        #region Communication Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="communicationType"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>        
        List<lkpCommunicationSubEvent> GetCommunicationTypeSubEvents(Int32 communicationType, Int32 eventId);
        List<lkpCommunicationSubEvent> GetCommunicationTypeSubEventsSpecific(Int32 communicationType, Int32 eventId, List<String> lstSubeventSpecificCodes);
        List<Int32?> GetItemIdsInUse(Int32 categoryId, Int32 tenantId, List<SystemEventSetting> systemEventSetting);
        Boolean DeleteCommunicationTemplateByMapID(Int32 tenantId, Int32 nodeNotificationMappingID, Int32 loggedInUserId, DateTime modifiedOn);

        #endregion

        //List<CommunicationTemplatePlaceHolder> GetTemplatePlaceHolders(List<CommunicationTemplatePlaceHolder> templatePlaceHolders);

        //SystemCommunication PrepareMessageContent(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, Int32 subEventId);
        SystemCommunication PrepareMessageContent(Dictionary<String, Object> dicContent, CommunicationTemplate communicationTemplate, List<CommunicationTemplatePlaceHolder> placeHoldersToFetch);
        SystemCommunication PrepareMessageContentForSystemMails(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, CommunicationTemplate communicationTemplate, List<CommunicationTemplatePlaceHolder> placeHoldersToFetch, bool? isPasswordToBeMask = null);
        Int32 SaveMailContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 CommunicationSubEventID, Int32 tenantId, Int32 hierarchyNodeID, List<ExternalCopyUsersContract> externalCopyUsers, CommunicationSubEvents communicationSubEvents, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, List<CommunicationTemplateContract> lstcommunicationTemplateContract = null, List<ExternalCopyUsersContract> lstCCUsers = null, String rotationHierarchyID = null, Int32? rotationID = AppConsts.NONE, Int32? rotationCreatedByID = AppConsts.NONE);
        Int32 SaveMailContentForCCUserSettings(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 CommunicationSubEventID, Int32 tenantId, Int32 hierarchyNodeID, List<ExternalCopyUsersContract> externalCopyUsers, CommunicationSubEvents communicationSubEvents, Int32 objectTypeId, Int32 recordId, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null);
        Int32 SaveMailContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, List<Entity.OrganizationUser> lstCCUsers, List<Entity.OrganizationUser> lstBCCUsers, List<SystemCommunicationAttachment> lstSystemCommunicationAttachment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdOn"></param>
        /// <returns></returns>
        //IEnumerable<SystemCommunicationDelivery> GetSystemCommunicationDelivery(DateTime createdOn);
        List<SystemCommunication> GetSystemCommunications(DateTime createdOn, Int32 maxRetryCount, Int32 EmailChunkSize);

        bool SetDispatchedTrue(List<SystemCommunicationDelivery> systemCommunicationDeliveries, Int32 userId);

        IQueryable<vw_ApplicantUser> GetApplicantUser();

        #region Communication Summary
        //Commented by :charanjot for UAT-1427: WB: 
        // IQueryable<CommunicationTemplateContract> GetCommunicationSummary();
        //created by Charanjot 
        List<CommunicationTemplateContract> GetCommunicationSummarySearch(SearchCommunicationTemplateContract searchDataContract, CustomPagingArgsContract gridCustomPaging);

        List<CommunicationTemplateContract> GetCommunicationSummarySearchArchive(SearchCommunicationTemplateContract searchDataContract, CustomPagingArgsContract gridCustomPaging);

        List<MessageDetail> GetUserMessages(Int32 userId, Int32 pageSize, Int32 pageIndex, String defaultExpression, Boolean isSortDirectionDescending, Int32 ApplicantDashboard = 0);

        SystemCommunication GetSystemNotificationDetails(Int32 systemCommunicationId);

        SystemCommunication GetSystemNotificationDetailsArchive(Int32 systemCommunicationId);

        void ReSendEmail(Int32 systemCommunicationDeliveryId, Int32 currentUserId, Boolean isAttachment = false);

        List<SystemCommunicationDelivery> GetSysCommunicationDeliveriesByIds(List<Int32> systemCommunicationDeliveryIds);


        #endregion

        #region Communication Mock Up

        CommunicationMockUpData GetMockUpData(Int32 subEventId);

        #endregion

        #region Send Message Content

        void SendMessageContentToQueue(MessagingContract messagingContract);

        List<GetCommunicationCCusersList> getCommunicationCCusers(Int32 communicationSubEventId, Int32 tenantId);

        List<HierarchyNotificationMapping> GetHierarchyNotificationMapping(Int32 communicationSubEventId, Int32 hierarchyNodeID, Int32 tenantId);

        #endregion

        #region Communication CC
        //Added UserType in UAT 1043
        List<CommunicationCCMaster> GetTenantSpecificCommunicationCCMaster(Int32 tenantID, Int32 DefaultTenantID, Int32 CurrentUserId, String UserType = null);
        //Added UserType in UAT 1043
        IEnumerable<CommunicationCCUser> GetCommunicationCCUser(Int32 communicationCCMasterID, Int32 DefaultTenantID, Int32 CurrentUserId, String UserType = null);

        Boolean SaveCommunicationCCMaster(CommunicationCCMaster communicationCCMasterObj);

        CommunicationCCMaster GetCommunicationCCMaster(Int32 CommunicationCCMasterID, Int32 tenantID);

        Boolean SaveCommunicationCcUsers(List<CommunicationCCUser> communicationCCUserList, Int32 currentUserId, Int32 communicationCCMasterID);

        Boolean DeleteCommunicationCcMaster(Int32 selectedTenantId, Int32 communicationCCMasterID, Int32 currentUserId);

        Boolean IsUserAlreadyMappedToSubEvent(Int32 communicationCCMasterID, Int32 currentUserID, Int32 subEventID);

        List<CommunicationCCUserContract> GetCommunicationCCUserAndSettings(Int32 communicationCCMasterID, Int32 CurrentUserId, String UserType = null);
        #endregion

        #region SEND NOTIFICATION ON FIRST ITEM SUBMITT
        Boolean IsFirstItemNotificationExist(Int32 subEventId, Int32 userId);
        #endregion

        #region  External User BCC
        List<lkpCommunicationSubEvent> GetCommunicationSubEventsType(List<String> subEventCode);
        List<ExternalCopyUser> GetBCCExternalUserData(Int32 tenantId, Int32 hierarchyNodeId, Int16 copyType);
        Boolean SaveRecordExternalUserBCC(Entity.ExternalCopyUser externalCopyuser);
        Boolean UpdateRecordExternalUserBCC(ExternalCopyUserContract externalCopyUserContract, Int32 externalCopyUserID, Int32 CurrentLoggedInUserId);
        Boolean DeleteExternalUserBCC(Int32 externalCopyUserID, Int32 CurrentLoggedInUserId);
        Boolean IsExternalUserExistForSubEvent(ExternalCopyUserContract externalCopyUserContract, Int32 hierarchyNodeId, Int32 tenantId, Int16 copyType, Int32? externalCopyUserId);
        #endregion

        #region Communication 05052014

        List<HierarchyNotificationMapping> GetHierarchyNotificationMappingData(Int32 tenantId, Int32 hierarchyNodeID, Int32 hierarchyContactMappingID);

        List<lkpCommunicationSubEvent> GetlkpCommunicationSubEvent(String subEventCategoryBusinessChannel);

        Boolean UpdateCommunicationSubEvent(Int32 tenantId, HierarchyNotificationMappingContract hNotificationMappingContract, Int32 hNotificationMappingID, Int32 currentLoggedInUserId);

        Boolean SaveCommunicationSubEvent(HierarchyNotificationMapping hNotificationMapping);

        Boolean DeleteCommunicationSubEvent(Int32 tenantId, Int32 hNotificationMappingID, Int32 currentLoggedInUserId);

        Boolean DeleteCommunicationSubEventList(Int32 tenantId, List<Int32> hNotificationMappingIDs, Int32 currentLoggedInUserId);

        List<lkpCopyType> GetlkpCopyType();

        List<lkpRecordObjectType> GetlkpRecordObjectType();

        List<lkpCommunicationSubEvent> GetlkpCommunicationSubEvent();
        #endregion

        /// <summary>
        /// To Get Communication Sub Event by EntityID, EntityType and CommunicationSubEvent code.
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="communicationSubEventCode"></param>
        /// <returns>lkpCommunicationSubEvent</returns>
        lkpCommunicationSubEvent GetCommSubEventByEntity(Int32 entityID, String entityTypeCode, String communicationSubEventCode);

        /// <summary>
        /// To Get Communication Sub Event by EntityID, EntityType and CommunicationSubEvent code.
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="communicationSubEvent"></param>
        /// <returns>TemplateID</returns>
        List<Int32> GetCommunicationTemplateIDByEntity(Int32 entityID, String entityTypeCode, String communicationSubEventCode);

        Int32 SaveSystemCommunicationAttachment(SystemCommunicationAttachment sysCommAttachment);

        bool SetRetryCountAndMessage(List<SystemCommunicationDelivery> systemCommunicationDeliveries, Int32 userId, String errorMessage);

        List<CommunicationCCUser> GetNotificationSettingData(Int32 oganizationUserID);

        Boolean SaveServiceAttachedFormCommunicationTemplate(List<Entity.lkpCommunicationSubEvent> commSubEventsList);

        Boolean UpdateServiceAttachedFormCommunicationTemplate();

        IEnumerable<CommunicationTemplateEntity> GetCommunicationTemplateEntityData(Int32 serviceFormID);

        CommunicationTemplate GetCommunicationTemplateData(Int32 communicationTemplateID);

        Boolean DeleteServiceAttachedFormCommunicationData(Int32 serviceFormID, Int32 currntUserID, Boolean VersionServiceFormToDelete);

        Boolean InsertCommunicationTemplatesEntities(List<Entity.CommunicationTemplateEntity> communicationTemplateEntitiesToBeAdded);

        #region Employment Notification For FlaggedOrder
        List<Entity.ClientEntity.CommunicationCCUsersList> GetClientAdminsForEmploymentNotification(Int32 communicationSubEventId, Int32 tenantId, CommunicationSubEvents communicationSubEvents, Int32 hierarchyNodeID);
        #endregion

        Int32 GetCommunicationTemplateIDForSubEventID(Int32 subEventID);

        Boolean SaveSysCommunicationAndSysDeliveries(List<SystemCommunication> lstSystemCommunications);

        #region UAT 1302 As an admin (client or ADB), I should be able to create preceptors/instructors
        Boolean CheckIsContactAlreadyRecievdEmail(Int32 subEventID, String emailID);
        #endregion

        List<SystemEventSetting> GetSystemEventSetting(Int32 subEventId);

        List<CommunicationTemplate> GetCommunicationTemplate(Int32 subEventId);

        #region UAT-1578
        /// <summary>
        /// Return Template for SMS
        /// </summary>
        /// <param name="communicationSubEventId">communicationSubEventId</param>
        /// <returns>SMSTemplate</returns>
        SMSTemplate GetSMSTemplateForCommunicationSubEvent(Int32 communicationSubEventId, Int32 languageID, Int32 DefaultLanguageID);
        /// <summary>
        /// Method to prepare message content for SMS
        /// </summary>
        /// <param name="communicationSubEventCode">communicationSubEventCode</param>
        /// <param name="dicContent">dicContent</param>
        /// <param name="smsTemplate">smsTemplate</param>
        /// <returns>SystemCommunication</returns>
        SystemCommunication PrepareMessageContentForSMS(Dictionary<String, Object> dicContent, SMSTemplate communicationTemplate, List<CommunicationTemplatePlaceHolder> placeHoldersToFetch);
        /// <summary>
        /// Method to Save SMS content in Database 
        /// </summary>
        /// <param name="systemCommunication">systemCommunication</param>
        /// <param name="communicationTemplateContract">communicationTemplateContract</param>
        /// <returns></returns>
        Int32 SaveSMSContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 notificationTypeId);
        #endregion

        List<Int32> GetSubEventsHavingTemplates(Int32 langaugeId, Int32 defaultLangaugeId);
        //UAT-1793 : Should not be able to create duplicate templates in the common template section of the System Template screen.
        List<Int32> GetSubEventsHavingTemplatesByTenant(Int32 tenantId, Int32 languageId, Int32 defaultLanguageId);

        #region UAT-3261: Badge Form Enhancements
        List<EmailDetails> GetUserEmails(Int32 userId, Int32 pageSize, Int32 pageIndex, String defaultExpression, Boolean isSortDirectionDescending, Int32 ApplicantDashboard = 0);
        List<SystemCommunicationDelivery> GetSysCommDeliveryIds(Int32 SystemCommunicationId);
        #endregion

        #region UAT
        List<CommunicationTemplateContract> GetSysCommunicationDeliveriesByIdsArchive(List<Int32> systemCommunicationDeliveryIds);
        #endregion


        Int32 SaveSysCommunicationAndSysDeliveries(SystemCommunication systemCommunications);

        List<Int32> GetSubEventsHavingAgencySpecificTemplates(Int32 languageId);//UAT-3704
    }
}
