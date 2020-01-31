using DAL.Interfaces;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.Templates;
using INTSOF.UnityHelper.Host;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.Repository
{
    public class CommunicationRepository : BaseQueueRepository, ICommunicationRepository
    {
        private ADBMessageDB_DevEntities ADBMessageQueueContext
        {
            get { return base.ADB_MessageQueueContext; }
        }


        #region Communication Subscription Settings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="selectedNotificationEventIds"></param>
        /// <param name="selectedReminderEventIds"></param>
        /// <param name="selectedAlertEventIds"></param>
        /// <param name="modifiedByID"></param>
        /// <param name="isCommit"></param>
        /// <param name="isSubscribedByUser"></param>
        /// <returns></returns>
        public bool AddUserCommunicationSubscriptionSettings(
            Int32 organizationUserId,
            List<Int32> selectedNotificationEventIds,
            List<Int32> selectedReminderEventIds,
            Int32 notificationCommunicationTypeId,
            Int32 reminderCommunicationTypeId,
            List<lkpCommunicationEvent> communicationEvents,
            int modifiedByID,
            bool isCommit = true,
            bool isSubscribedByUser = true)
        {
            MessageRepository messageRepository = new MessageRepository();

            SetUserCommunicationSubscriptionSettingsInDb(organizationUserId, notificationCommunicationTypeId, selectedNotificationEventIds, communicationEvents, modifiedByID, isSubscribedByUser);
            SetUserCommunicationSubscriptionSettingsInDb(organizationUserId, reminderCommunicationTypeId, selectedReminderEventIds, communicationEvents, modifiedByID, isSubscribedByUser);

            if (isCommit)
                ADBMessageQueueContext.SaveChanges();

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiedById"></param>
        /// <param name="selectedSubscriptionSettings"></param>
        /// <param name="unSelectedSubscriptionSettings"></param>
        /// <returns></returns>
        public bool AddUserCommunicationSubscriptionSettings(Int32 modifiedById, List<UserCommunicationSubscriptionSetting> selectedSubscriptionSettings, List<UserCommunicationSubscriptionSetting> unSelectedSubscriptionSettings)
        {
            SetUserCommunicationSubscriptionSettingsInDb(modifiedById, true, selectedSubscriptionSettings);
            SetUserCommunicationSubscriptionSettingsInDb(modifiedById, false, unSelectedSubscriptionSettings);
            ADBMessageQueueContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userCommunicationSubscriptionSettings"></param>
        /// <returns></returns>
        public bool AddUserCommunicationSubscriptionSettings(List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings)
        {
            if (userCommunicationSubscriptionSettings != null && userCommunicationSubscriptionSettings.Count > 0)
            {
                foreach (UserCommunicationSubscriptionSetting userCommunicationSubscriptionSetting in userCommunicationSubscriptionSettings)
                    ADBMessageQueueContext.UserCommunicationSubscriptionSettings.AddObject(userCommunicationSubscriptionSetting);
                ADBMessageQueueContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="communicationTypeId"></param>
        /// <returns></returns>
        public IEnumerable<UserCommunicationSubscriptionSetting> GetUserCommunicationSubscriptionSettings(Int32 organizationUserId, Int32 communicationTypeId)
        {
            return
                ADBMessageQueueContext.UserCommunicationSubscriptionSettings.Where(userCommunicationSubscriptionSetting =>
                    userCommunicationSubscriptionSetting.OrganizationUserID.Equals(organizationUserId)
                    && userCommunicationSubscriptionSetting.CommunicationTypeID.Equals(communicationTypeId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="communicationEventId"></param>
        /// <returns></returns>
        public UserCommunicationSubscriptionSetting GetUserCommunicationSubscriptionSettings(Int32 organizationUserId, Int32 communicationTypeId, Int32 communicationEventId)
        {
            return
                ADBMessageQueueContext.UserCommunicationSubscriptionSettings.FirstOrDefault(userCommunicationSubscriptionSetting =>
                    userCommunicationSubscriptionSetting.OrganizationUserID.Equals(organizationUserId)
                    && userCommunicationSubscriptionSetting.CommunicationTypeID.Equals(communicationTypeId)
                    && userCommunicationSubscriptionSetting.CommunicationEventID.Equals(communicationEventId)

                    );
        }

        #endregion

        #region Communication Events

        /// <summary>
        /// Gets the list of the Sub events of a communication type
        /// </summary>
        /// <param name="communicationType">Type of communication i.e. SMS or Alert etc.</param>
        /// <param name="eventId">Id of the event related to a Communication type</param>
        /// <returns>List of the Sub-events</returns>
        public List<lkpCommunicationSubEvent> GetCommunicationTypeSubEvents(Int32 communicationType, Int32 eventId)
        {
            return ADBMessageQueueContext.lkpCommunicationSubEvents.Include(SysXEntityConstants.TABLE_LKP_COMMUNICATION_EVENT).Include(SysXEntityConstants.TABLE_LKP_COMMUNICATION_TYPE).Where(cSubEvent => cSubEvent.CommunicationTypeID.Equals(communicationType) && cSubEvent.CommunicationEventID.Equals(eventId) && cSubEvent.IsDeleted == false).ToList();
        }


        /// <summary>
        /// Gets the list of the Sub events of a communication type,for sub-event specific
        /// </summary>
        public List<lkpCommunicationSubEvent> GetCommunicationTypeSubEventsSpecific(Int32 communicationType, Int32 eventId, List<String> lstSubeventSpecificCodes)
        {
            return ADBMessageQueueContext.lkpCommunicationSubEvents
                .Include(SysXEntityConstants.TABLE_LKP_COMMUNICATION_EVENT).Include(SysXEntityConstants.TABLE_LKP_COMMUNICATION_TYPE)
                .Where(cSubEvent => cSubEvent.CommunicationTypeID.Equals(communicationType) && cSubEvent.CommunicationEventID.Equals(eventId)
               && lstSubeventSpecificCodes.Contains(cSubEvent.Code.ToLower()) && cSubEvent.IsDeleted == false).ToList();
        }

        public List<Int32?> GetItemIdsInUse(Int32 categoryId, Int32 tenantId, List<SystemEventSetting> systemEventSetting)
        {
            //return ADBMessageQueueContext.SystemEventSettings
            //    .Where(ses => ses.TenantID == tenantId && ses.ComplianceCategoryID == categoryId
            //       && ses.CommunicationSubEventID == subEventId && ses.IsDeleted == false)
            //    .Select(ses => ses.ComplianceItemID).ToList();
            return systemEventSetting
                .Where(ses => ses.TenantID == tenantId && ses.ComplianceCategoryID == categoryId)
                .Select(ses => ses.ComplianceItemID).ToList();
        }

        /// <summary>
        /// To delete Communication Template by nodeNotificationMappingID and tenantId
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nodeNotificationMappingID"></param>
        /// <returns></returns>
        public Boolean DeleteCommunicationTemplateByMapID(Int32 tenantId, Int32 nodeNotificationMappingID, Int32 loggedInUserId, DateTime modifiedOn)
        {

            var lstSystemEventSettings = ADBMessageQueueContext.SystemEventSettings.Where(x => x.TenantID == tenantId && x.SES_NodeNotificationMappingID == nodeNotificationMappingID
                                            && x.IsDeleted == false);
            lstSystemEventSettings.ForEach(x =>
            {
                x.IsDeleted = true;
                x.ModifiedBy = loggedInUserId;
                x.ModifiedOn = modifiedOn;
                x.CommunicationTemplate.IsDeleted = true;
                x.CommunicationTemplate.ModifiedByID = loggedInUserId;
                x.CommunicationTemplate.ModifiedOn = modifiedOn;
            });
            if (ADBMessageQueueContext.SaveChanges() > 0)
                return true;
            return false;
        }

        #endregion

        /// <summary>
        /// Gets the list of Placeholders of a Template for the sub-event
        /// </summary>
        /// <param name="subEventId">Id of the sub event selected.</param>
        /// <returns>List of the place holders</returns>
        //public List<Communication.TemplatePlaceHolder> GetTemplatePlaceHolders(List<CommunicationTemplatePlaceHolder> templatePlaceHolders)
        //{
        //  //  List<Int32> placeHolderIds =
        //  //ADBMessageQueueContext.CommunicationTemplatePlaceHolderSubEvents.Where(ctphSubEvents => ctphSubEvents.CommunicationSubEventID.Equals(subEventId))
        //  //    .Select(ctphSubEvents => ctphSubEvents.CommunicationTemplatePlaceHolderID).ToList();

        //  //  return ADBMessageQueueContext.CommunicationTemplatePlaceHolders
        //  //      .Where(ctPlaceHolders => placeHolderIds.Contains(ctPlaceHolders.CommunicationTemplatePlaceHolderID)).ToList();
        //    return templatePlaceHolders;
        //}


        /// <summary>
        /// Create the content of the Email to be sent, based on the communication sub event received
        /// </summary>
        /// <param name="communicationSubEvent">Id of the event for which the email content is to be generated.</param>
        public SystemCommunication PrepareMessageContent(Dictionary<String, Object> dicContent, CommunicationTemplate communicationTemplate, List<CommunicationTemplatePlaceHolder> placeHoldersToFetch)
        {
            try
            {
                SystemCommunication systemCommunication = null;
                //CommunicationTemplate communicationTemplate = new CommunicationTemplate();
                //if (communicationSubEvent == CommunicationSubEvents.COMPLIANCE_ITEM_ABOUT_TO_EXPIRE || communicationSubEvent == CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED)
                //{
                //    Int32 tenantId = dicContent.GetValue(EmailFieldConstants.TENANT_ID).IsNotNull() ? Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.TENANT_ID)) : 0;
                //    Int32 categoryId = dicContent.GetValue(EmailFieldConstants.COMPLIANCE_CATEGORY_ID).IsNotNull() ? Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.COMPLIANCE_CATEGORY_ID)) : 0;
                //    Int32 itemId = dicContent.GetValue(EmailFieldConstants.COMPLIANCE_ITEM_ID).IsNotNull() ? Convert.ToInt32(dicContent.GetValue(EmailFieldConstants.COMPLIANCE_ITEM_ID)) : 0;
                //    Int32 templateId = 0;

                //    SystemEventSetting systemEventSetting = new SystemEventSetting();
                //    systemEventSetting = ADB_MessageQueueContext.SystemEventSettings.Where(evntSetting => evntSetting.CommunicationSubEventID == subEventId && evntSetting.TenantID == tenantId &&
                //        evntSetting.ComplianceCategoryID == categoryId && evntSetting.ComplianceItemID == itemId && evntSetting.IsDeleted == false).FirstOrDefault();

                //    if (systemEventSetting.IsNull())
                //    {
                //        systemEventSetting = ADB_MessageQueueContext.SystemEventSettings.Where(evntSetting => evntSetting.CommunicationSubEventID == subEventId && evntSetting.TenantID == tenantId &&
                //            evntSetting.ComplianceCategoryID == categoryId && !evntSetting.ComplianceItemID.HasValue && evntSetting.IsDeleted == false).FirstOrDefault();
                //        if (systemEventSetting.IsNotNull())
                //        {
                //            templateId = systemEventSetting.CommunicationTemplateID;
                //        }
                //    }
                //    else
                //    {
                //        templateId = systemEventSetting.CommunicationTemplateID;
                //    }

                //    if (templateId != 0)
                //    {
                //        communicationTemplate = ADB_MessageQueueContext.CommunicationTemplates.Where(cTemplate => cTemplate.CommunicationSubEventID == subEventId && cTemplate.IsDeleted == false
                //             && cTemplate.CommunicationTemplateID == templateId).OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();
                //    }
                //    else
                //    {
                //        communicationTemplate = ADB_MessageQueueContext.CommunicationTemplates.Where(cTemplate => cTemplate.CommunicationSubEventID == subEventId
                //            && cTemplate.IsDeleted == false && !cTemplate.SystemEventSettings.Any()).OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();
                //    }
                //}
                //else
                //{
                //    communicationTemplate = ADB_MessageQueueContext.CommunicationTemplates.Where(cTemplate => cTemplate.CommunicationSubEventID == subEventId && cTemplate.IsDeleted == false).OrderByDescending(cTemplate => cTemplate.CommunicationTemplateID).FirstOrDefault();
                //}

                if (communicationTemplate.IsNotNull())
                {
                    String templateContent = communicationTemplate.Content;
                    String templateSubject = communicationTemplate.Subject;
                    //List<CommunicationTemplatePlaceHolderSubEvent> placeHoldersToFetch = ADBMessageQueueContext.CommunicationTemplatePlaceHolderSubEvents
                    //                                                                    .Include(SysXEntityConstants.TABLE_COMMUNICATION_TEMPLATE_PLACEHOLDER)
                    //                                                                    .Where(ctphSubEvents => ctphSubEvents.CommunicationSubEventID.Equals(subEventId))
                    //                                                                    .ToList();

                    //Null check
                    if (placeHoldersToFetch.IsNotNull())
                    {
                        foreach (var placeHolder in placeHoldersToFetch)
                        {
                            Object obj = dicContent.GetValue(placeHolder.Property);
                            templateContent = templateContent.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                            templateSubject = templateSubject.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);// changes corresponding to UAT - 964 : Add Order ID# to Subject Line of order result notification emails
                        }
                    }
                    //In Special case for EDS Mail 
                    if (dicContent.ContainsKey(EmailFieldConstants.EDS_HTML_BODY))
                    {
                        Object objEDSHtmlBody = dicContent.GetValue(EmailFieldConstants.EDS_HTML_BODY);
                        if (objEDSHtmlBody.IsNotNull() && !Convert.ToString(objEDSHtmlBody).IsEmpty())
                        {
                            templateContent = Convert.ToString(objEDSHtmlBody);
                        }
                    }
                    systemCommunication = new SystemCommunication();
                    systemCommunication.Content = templateContent;
                    systemCommunication.Subject = templateSubject;
                    systemCommunication.CommunicationSubEventID = communicationTemplate.CommunicationSubEventID;

                }
                return systemCommunication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemCommunication PrepareMessageContentForSystemMails(CommunicationSubEvents communicationSubEvent, Dictionary<String, Object> dicContent, CommunicationTemplate communicationTemplate, List<CommunicationTemplatePlaceHolder> placeHoldersToFetch, bool? isPasswordToBeMask = null) //UAT-4570 : isPasswordToBeMask : parameter added to mask password before saving in database for security reason
        {
            SystemCommunication systemCommunication = null;

            if (communicationTemplate.IsNotNull())
            {
                String templateContent = communicationTemplate.Content;
                String templateSubject = communicationTemplate.Subject;
                //Null Check                
                if (placeHoldersToFetch.IsNotNull())
                {
                    foreach (var placeHolder in placeHoldersToFetch)
                    {
                        Object obj = dicContent.GetValue(placeHolder.Property);
                        //if (isPasswordToBeMask.HasValue && isPasswordToBeMask.Value && placeHolder.Property == EmailFieldConstants.PASSWORD)
                        //{
                        //    templateContent = templateContent.Replace(placeHolder.PlaceHolder, "XXXXXXXXXX");
                        //}
                        //else
                        //{
                        //    templateContent = templateContent.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                        //}
                        templateContent = templateContent.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                        templateSubject = templateSubject.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                    }
                }
                systemCommunication = new SystemCommunication();
                systemCommunication.Content = templateContent;
                systemCommunication.Subject = templateSubject;
                systemCommunication.CommunicationSubEventID = communicationTemplate.CommunicationSubEventID;
            }
            return systemCommunication;
        }

        /// <summary>
        /// Save the content of the Email to be sent, based on the communication sub event received
        /// </summary>
        /// <param name="subEventId"></param>
        //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
        //public Int32 SaveMailContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 communicationSubEventID, Int32 tenantId, Int32 hierarchyNodeID, List<ExternalCopyUsersContract> externalCopyUsers, CommunicationSubEvents communicationSubEvents, List<OrganizationUserContract> lstClientAdmins = null)
        //{
        public Int32 SaveMailContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 communicationSubEventID, Int32 tenantId, Int32 hierarchyNodeID, List<ExternalCopyUsersContract> externalCopyUsers, CommunicationSubEvents communicationSubEvents, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null, List<CommunicationTemplateContract> lstcommunicationTemplateContract = null, List<ExternalCopyUsersContract> lstCCUsers = null, String rotationHierarchyID = null, Int32? rotationID = AppConsts.NONE, Int32? rotationCreatedByID = AppConsts.NONE)
        {
            DateTime creationDate = DateTime.Now;

            systemCommunication.SenderEmailID = communicationTemplateContract.SenderEmailID;
            systemCommunication.SenderName = communicationTemplateContract.SenderName;
            systemCommunication.CreatedByID = communicationTemplateContract.CurrentUserId;
            systemCommunication.CreatedOn = creationDate;

            #region Override Entity Command Timeout Setting
            Int32 TimeoutSetting = AppConsts.NONE;
            if (!System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"].IsNullOrEmpty())
            {
                TimeoutSetting = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"]);
                if (TimeoutSetting > AppConsts.NONE)
                {
                    ADBMessageQueueContext.CommandTimeout = TimeoutSetting;
                }
            }
            #endregion

            //ADBMessageQueueContext.SystemCommunications.AddObject(systemCommunication);            
            //sp method for saving systemcommunication and return inserted systemcommunicationid
            Int32 SystemCommunicationId = InsertSystemCommunication(systemCommunication);

            //UAT-2538
            if (lstcommunicationTemplateContract.IsNullOrEmpty())
            {
                SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery
                {
                    RecieverEmailID = communicationTemplateContract.RecieverEmailID,
                    RecieverName = communicationTemplateContract.RecieverName,
                    IsDispatched = false,
                    IsCC = false,
                    CreatedByID = communicationTemplateContract.CurrentUserId,
                    CreatedOn = creationDate,
                    //SystemCommunication = systemCommunication,
                    SystemCommunicationTypeID = SystemCommunicationId,
                    ReceiverOrganizationUserID = communicationTemplateContract.ReceiverOrganizationUserId,
                    RetryCount = 0
                };
                ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(systemCommunicationDelivery);
            }
            //}
            //else
            //{
            //    foreach (CommunicationTemplateContract objcommunicationTemplateContract in lstcommunicationTemplateContract)
            //    {
            //        SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery
            //        {
            //            RecieverEmailID = objcommunicationTemplateContract.RecieverEmailID,
            //            RecieverName = objcommunicationTemplateContract.RecieverName,
            //            IsDispatched = false,
            //            IsCC = false,
            //            CreatedByID = objcommunicationTemplateContract.CurrentUserId,
            //            CreatedOn = creationDate,
            //            SystemCommunication = systemCommunication,
            //            ReceiverOrganizationUserID = objcommunicationTemplateContract.ReceiverOrganizationUserId,
            //            RetryCount = 0
            //        };
            //        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(systemCommunicationDelivery);
            //    }
            //}

            //UAT 867 : Creation of Background  Orders with Color Flag and Flagged Status Results notification report
            if (hierarchyNodeID == -1
                && lstBackgroundOrderDailyReport.IsNotNull() && lstBackgroundOrderDailyReport.Count > 0)
            {
                lstBackgroundOrderDailyReport.ForEach(condition =>
                    {
                        SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                        {
                            RecieverEmailID = condition.EmailAddress,
                            RecieverName = condition.UserName,
                            IsDispatched = false,
                            IsCC = !overrideCcBcc,
                            IsBCC = !overrideCcBcc,
                            CreatedByID = communicationTemplateContract.CurrentUserId,
                            CreatedOn = creationDate,
                            //SystemCommunication = systemCommunication,
                            SystemCommunicationTypeID = SystemCommunicationId,
                            ReceiverOrganizationUserID = condition.OrganizationUserId,
                        };
                        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                    });
            }
            else if (tenantId > AppConsts.NONE)
            {
                String ccCode = CopyType.CC.GetStringValue();
                String bccCode = CopyType.BCC.GetStringValue();
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //if (lstClientAdmins == null)
                //{
                String key = communicationSubEventID.ToString();
                if (!rotationHierarchyID.IsNullOrEmpty() && (hierarchyNodeID.IsNull() || hierarchyNodeID <= 0))
                {
                    key = key + "_" + rotationHierarchyID;
                }
                else if (hierarchyNodeID.IsNotNull())
                {
                    key = key + "_" + hierarchyNodeID.ToString(); // added hierarchy node in key corresponding to UAT - 868 : As a Client admin, I should only receive CC/BCC emails for nodes to which I have access
                }
                key = key + "_" + tenantId.ToString();
                List<Entity.ClientEntity.CommunicationCCUsersList> defaultCommunicationCCusers = new List<Entity.ClientEntity.CommunicationCCUsersList>();
                if (System.Web.HttpContext.Current != null)
                {
                    if (System.Web.HttpContext.Current.Items[key] != null)
                    {
                        defaultCommunicationCCusers = (List<Entity.ClientEntity.CommunicationCCUsersList>)System.Web.HttpContext.Current.Items[key];
                    }
                    else
                    {
                        if (!rotationHierarchyID.IsNullOrEmpty())
                        {
                            var res = GetCCusers(communicationSubEventID, tenantId, communicationSubEvents, hierarchyNodeID, rotationHierarchyID);
                            defaultCommunicationCCusers.AddRange(res.Where(f => !f.IsRotationCreatedNotification).ToList());
                            //Filter User based upon UAT-3364
                            if (rotationID > AppConsts.NONE && rotationCreatedByID > AppConsts.NONE)
                            {
                                List<Entity.ClientEntity.CommunicationCCUsersList> oldDefaultCommunicationCCusers = new List<Entity.ClientEntity.CommunicationCCUsersList>();
                                oldDefaultCommunicationCCusers.AddRange(res.Where(f => f.IsRotationCreatedNotification).ToList());
                                foreach (var item in oldDefaultCommunicationCCusers)
                                {
                                    if (item.UserID == rotationCreatedByID)
                                    {
                                        defaultCommunicationCCusers.Add(item);
                                    }
                                }
                            }
                        }
                        else
                        {
                            defaultCommunicationCCusers = GetCCusers(communicationSubEventID, tenantId, communicationSubEvents, hierarchyNodeID, null);
                        }
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
                        var res = GetCCusers(communicationSubEventID, tenantId, communicationSubEvents, hierarchyNodeID, rotationHierarchyID);
                        defaultCommunicationCCusers.AddRange(res.Where(f => !f.IsRotationCreatedNotification).ToList());
                        //Filter User based upon UAT-3364
                        if (rotationID > AppConsts.NONE && rotationCreatedByID > AppConsts.NONE)
                        {
                            List<Entity.ClientEntity.CommunicationCCUsersList> oldDefaultCommunicationCCusers = new List<Entity.ClientEntity.CommunicationCCUsersList>();
                            oldDefaultCommunicationCCusers.AddRange(res.Where(f => f.IsRotationCreatedNotification).ToList());
                            foreach (var item in oldDefaultCommunicationCCusers)
                            {
                                if (item.UserID == rotationCreatedByID)
                                {
                                    defaultCommunicationCCusers.Add(item);
                                }
                            }
                        }
                        if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        {
                            if (ServiceContext.Current.DataDict == null)
                                ServiceContext.Current.DataDict = new Dictionary<String, object>();
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
                        var res = GetCCusers(communicationSubEventID, tenantId, communicationSubEvents, hierarchyNodeID, rotationHierarchyID);
                        defaultCommunicationCCusers.AddRange(res.Where(f => !f.IsRotationCreatedNotification).ToList());
                        //Filter User based upon UAT-3364
                        if (rotationID > AppConsts.NONE && rotationCreatedByID > AppConsts.NONE)
                        {
                            List<Entity.ClientEntity.CommunicationCCUsersList> oldDefaultCommunicationCCusers = new List<Entity.ClientEntity.CommunicationCCUsersList>();
                            oldDefaultCommunicationCCusers.AddRange(res.Where(f => f.IsRotationCreatedNotification).ToList());
                            foreach (var item in oldDefaultCommunicationCCusers)
                            {
                                if (item.UserID == rotationCreatedByID)
                                {
                                    defaultCommunicationCCusers.Add(item);
                                }
                            }
                        }
                        if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        {
                            if (ParallelTaskContext.Current.DataDict == null)
                                ParallelTaskContext.Current.DataDict = new Dictionary<String, object>();
                            ParallelTaskContext.Current.DataDict.Add(key, defaultCommunicationCCusers);
                        }
                    }
                }
                if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                {
                    defaultCommunicationCCusers = defaultCommunicationCCusers.Where(condition => condition.IsEmail).ToList();
                    foreach (Entity.ClientEntity.CommunicationCCUsersList defaultCommunicationCCuser in defaultCommunicationCCusers)
                    {
                        SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                        {
                            RecieverEmailID = defaultCommunicationCCuser.EmailAddress,
                            RecieverName = defaultCommunicationCCuser.UserName,
                            IsDispatched = false,
                            IsCC = String.IsNullOrEmpty(defaultCommunicationCCuser.CopyCode) ? false : overrideCcBcc ? (communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS || communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS ? false : defaultCommunicationCCuser.CopyCode == ccCode ? true : false) : defaultCommunicationCCuser.CopyCode == ccCode ? true : false,
                            IsBCC = String.IsNullOrEmpty(defaultCommunicationCCuser.CopyCode) ? false : overrideCcBcc ? (communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS || communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS ? false : defaultCommunicationCCuser.CopyCode == bccCode ? true : false) : defaultCommunicationCCuser.CopyCode == bccCode ? true : false,
                            CreatedByID = communicationTemplateContract.CurrentUserId,
                            CreatedOn = creationDate,
                            //SystemCommunication = systemCommunication,
                            SystemCommunicationTypeID = SystemCommunicationId,
                            ReceiverOrganizationUserID = defaultCommunicationCCuser.UserID, //communicationTemplateContract.ReceiverOrganizationUserId
                        };
                        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                    }
                }
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //}
                //else
                //{
                //    foreach (var clientAdmin in lstClientAdmins)
                //    {
                //        SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                //        {
                //            RecieverEmailID = clientAdmin.EmailAddress,
                //            RecieverName = clientAdmin.FullName,
                //            IsDispatched = false,
                //            IsCC = false,
                //            IsBCC = false,
                //            CreatedByID = communicationTemplateContract.CurrentUserId,
                //            CreatedOn = creationDate,
                //            SystemCommunication = systemCommunication,
                //            ReceiverOrganizationUserID = clientAdmin.OrganizationUserId, //communicationTemplateContract.ReceiverOrganizationUserId,
                //            RetryCount = 0
                //        };
                //        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                //    }
                //}
                //To get external BCC users on the basis of Hierarchy Node ID, Tenant ID and Communication Sub Event ID

                #region Old Code
                //List<ExternalCopyUsersContract> externalCopyUsers = new List<ExternalCopyUsersContract>();
                //String externalEmailkey = communicationSubEventID.ToString() + "_" + tenantId.ToString() + "_" + hierarchyNodeID;

                //if (System.Web.HttpContext.Current != null)
                //{
                //    if (System.Web.HttpContext.Current.Items[externalEmailkey] != null)
                //    {
                //        externalCopyUsers = (List<ExternalCopyUsersContract>)System.Web.HttpContext.Current.Items[externalEmailkey];
                //    }
                //    else
                //    {
                //        //externalCopyUsers = getCommunicationCCusers(communicationSubEventID, tenantId);
                //        var externalCopyUser = GetExternalCopyUsers(communicationSubEventID, hierarchyNodeID, tenantId);
                //        if (externalCopyUser != null && externalCopyUser.Any())
                //        {
                //            externalCopyUsers = externalCopyUser.Where(x => x.lkpCopyType.CT_Code == bccCode)
                //                                .Select(y => new ExternalCopyUsersContract
                //                                {
                //                                    UserName = y.ECU_FirstName + " " + y.ECU_LastName,
                //                                    UserEmailAddress = y.ECU_EmailID,
                //                                    CopyTypeCode = bccCode
                //                                }).ToList();
                //        }
                //        if (externalCopyUsers != null && externalCopyUsers.Count > AppConsts.NONE)
                //        {
                //            System.Web.HttpContext.Current.Items.Add(externalEmailkey, externalCopyUsers);
                //        }
                //    }
                //}
                //else if (ServiceContext.Current != null)
                //{
                //    if (ServiceContext.Current.DataDict != null && ServiceContext.Current.DataDict.ContainsKey(externalEmailkey))
                //    {
                //        externalCopyUsers = (List<ExternalCopyUsersContract>)ServiceContext.Current.DataDict.GetValue(externalEmailkey);
                //    }
                //    else
                //    {
                //        //externalCopyUsers = getCommunicationCCusers(communicationSubEventID, tenantId);
                //        var externalCopyUser = GetExternalCopyUsers(communicationSubEventID, hierarchyNodeID, tenantId);
                //        if (externalCopyUser != null && externalCopyUser.Any())
                //        {
                //            externalCopyUsers = externalCopyUser.Where(x => x.lkpCopyType.CT_Code == bccCode)
                //                                .Select(y => new ExternalCopyUsersContract
                //                                {
                //                                    UserName = y.ECU_FirstName + " " + y.ECU_LastName,
                //                                    UserEmailAddress = y.ECU_EmailID,
                //                                    CopyTypeCode = bccCode
                //                                }).ToList();
                //        }
                //        if (externalCopyUsers != null && externalCopyUsers.Count > AppConsts.NONE)
                //        {
                //            if (ServiceContext.Current.DataDict == null)
                //                ServiceContext.Current.DataDict = new Dictionary<String, object>();
                //            ServiceContext.Current.DataDict.Add(externalEmailkey, externalCopyUsers);
                //        }
                //    }
                //}
                //else if (ParallelTaskContext.Current != null)
                //{
                //    if (ParallelTaskContext.Current.DataDict != null && ParallelTaskContext.Current.DataDict.ContainsKey(externalEmailkey))
                //    {
                //        externalCopyUsers = (List<ExternalCopyUsersContract>)ParallelTaskContext.Current.DataDict.GetValue(externalEmailkey);
                //    }
                //    else
                //    {
                //        //externalCopyUsers = getCommunicationCCusers(communicationSubEventID, tenantId);
                //        var externalCopyUser = GetExternalCopyUsers(communicationSubEventID, hierarchyNodeID, tenantId);
                //        if (externalCopyUser != null && externalCopyUser.Any())
                //        {
                //            externalCopyUsers = externalCopyUser.Where(x => x.lkpCopyType.CT_Code == bccCode)
                //                                .Select(y => new ExternalCopyUsersContract
                //                                {
                //                                    UserName = y.ECU_FirstName + " " + y.ECU_LastName,
                //                                    UserEmailAddress = y.ECU_EmailID,
                //                                    CopyTypeCode = bccCode
                //                                }).ToList();
                //        }
                //        if (externalCopyUsers != null && externalCopyUsers.Count > AppConsts.NONE)
                //        {
                //            if (ParallelTaskContext.Current.DataDict == null)
                //                ParallelTaskContext.Current.DataDict = new Dictionary<String, object>();
                //            ParallelTaskContext.Current.DataDict.Add(externalEmailkey, externalCopyUsers);
                //        }
                //    }
                //}

                #endregion

                if (externalCopyUsers != null && externalCopyUsers.Count > AppConsts.NONE)
                {
                    //externalCopyUsers = externalCopyUsers.Where(condition => condition.IsEmail).ToList();
                    foreach (ExternalCopyUsersContract externalCopyUser in externalCopyUsers)
                    {
                        SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                        {
                            RecieverEmailID = externalCopyUser.UserEmailAddress,
                            RecieverName = externalCopyUser.UserName,
                            IsDispatched = false,
                            IsCC = !String.IsNullOrEmpty(externalCopyUser.CopyTypeCode) && externalCopyUser.CopyTypeCode == ccCode ? true : false,
                            IsBCC = !String.IsNullOrEmpty(externalCopyUser.CopyTypeCode) && externalCopyUser.CopyTypeCode == bccCode ? true : false,
                            CreatedByID = communicationTemplateContract.CurrentUserId,
                            CreatedOn = creationDate,
                            //SystemCommunication = systemCommunication,
                            SystemCommunicationTypeID = SystemCommunicationId,
                            ReceiverOrganizationUserID = externalCopyUser.UserID, //communicationTemplateContract.ReceiverOrganizationUserId,
                            RetryCount = 0
                        };
                        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                    }
                }

                //UAT-2977
                if (lstCCUsers != null && lstCCUsers.Count > AppConsts.NONE)
                {
                    foreach (ExternalCopyUsersContract ccUser in lstCCUsers)
                    {
                        SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                        {
                            RecieverEmailID = ccUser.UserEmailAddress,
                            RecieverName = ccUser.UserName,
                            IsDispatched = false,
                            IsCC = !String.IsNullOrEmpty(ccUser.CopyTypeCode) && ccUser.CopyTypeCode == ccCode ? true : false,
                            IsBCC = !String.IsNullOrEmpty(ccUser.CopyTypeCode) && ccUser.CopyTypeCode == bccCode ? true : false,
                            CreatedByID = communicationTemplateContract.CurrentUserId,
                            CreatedOn = creationDate,
                            //SystemCommunication = systemCommunication,
                            SystemCommunicationTypeID = SystemCommunicationId,
                            ReceiverOrganizationUserID = ccUser.UserID, //communicationTemplateContract.ReceiverOrganizationUserId,
                            RetryCount = 0
                        };
                        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                    }
                }
            }

            //UAT-2538:
            //here we are adding multiple applpicants in bcc list(SystemCommunicationDelivery) corresponding to one SystemCommunicationID
            if (!lstcommunicationTemplateContract.IsNullOrEmpty())
            {
                foreach (CommunicationTemplateContract objcommunicationTemplateContract in lstcommunicationTemplateContract)
                {
                    SystemCommunicationDelivery BCCsystemCommunicationDelivery = new SystemCommunicationDelivery
                    {
                        RecieverEmailID = objcommunicationTemplateContract.RecieverEmailID,
                        RecieverName = objcommunicationTemplateContract.RecieverName,
                        IsDispatched = false,
                        IsCC = false,
                        IsBCC = objcommunicationTemplateContract.IsToUser ? false : true,
                        CreatedByID = objcommunicationTemplateContract.CurrentUserId > AppConsts.NONE ? objcommunicationTemplateContract.CurrentUserId : communicationTemplateContract.CurrentUserId,
                        CreatedOn = creationDate,
                        //SystemCommunication = systemCommunication,
                        SystemCommunicationTypeID = SystemCommunicationId,
                        ReceiverOrganizationUserID = objcommunicationTemplateContract.ReceiverOrganizationUserId,
                        RetryCount = 0
                    };
                    ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(BCCsystemCommunicationDelivery);
                }
            }

            ADBMessageQueueContext.SaveChanges();
            //return systemCommunication.SystemCommunicationID;
            return SystemCommunicationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdOn"></param>
        /// <returns></returns>
        //public IEnumerable<SystemCommunicationDelivery> GetSystemCommunicationDelivery(DateTime createdOn)
        public List<SystemCommunication> GetSystemCommunications(DateTime createdOn, Int32 maxRetryCount, Int32 emailChunkSize)
        {
            List<SystemCommunication> result = new List<SystemCommunication>();
            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@CreatedOn", createdOn),
                             new SqlParameter("@MaxRetryCount",maxRetryCount),
                             new SqlParameter ("@EmailChunkSize", emailChunkSize)

                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSystemCommunications", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        SystemCommunication systemCommunicationContract = null;
                        while (dr.Read())
                        {
                            Int32 SystemCommunicationID = dr["SystemCommunicationID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SystemCommunicationID"]);
                            if (systemCommunicationContract.IsNullOrEmpty() && !result.Where(d => d.SystemCommunicationID == SystemCommunicationID).Any())
                                systemCommunicationContract = new SystemCommunication();
                            else
                                systemCommunicationContract = result.Where(d => d.SystemCommunicationID == SystemCommunicationID).FirstOrDefault();

                            #region System Communication Properties
                            systemCommunicationContract.SystemCommunicationID = SystemCommunicationID;
                            systemCommunicationContract.CommunicationSubEventID = dr["CommunicationSubEventID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["CommunicationSubEventID"]);
                            systemCommunicationContract.Content = dr["Content"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Content"]);
                            systemCommunicationContract.Subject = dr["Subject"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Subject"]);
                            systemCommunicationContract.SenderEmailID = dr["SenderEmailID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SenderEmailID"]);
                            systemCommunicationContract.SenderName = dr["SenderName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SenderName"]);
                            //systemCommunicationContract.lkpCommunicationSubEvent.Code= dr["Code"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Code"]);
                            lkpCommunicationSubEvent lkpCommunicationSubEventContract = new lkpCommunicationSubEvent();
                            lkpCommunicationSubEventContract.CommunicationSubEventID = dr["CommSubEventID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["CommSubEventID"]);
                            lkpCommunicationSubEventContract.Code = dr["CommunicationSubEventCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["CommunicationSubEventCode"]);
                            systemCommunicationContract.lkpCommunicationSubEvent = lkpCommunicationSubEventContract;
                            #endregion

                            SystemCommunicationAttachment systemCommunicationAttachmentContract = new SystemCommunicationAttachment();
                            #region System Communication Attachment
                            String AttachmentPath = dr["SCA_DocumentPath"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SCA_DocumentPath"]);

                            if (!AttachmentPath.IsNullOrEmpty())
                            {
                                systemCommunicationAttachmentContract.SCA_ID = dr["SCA_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SCA_ID"]);
                                systemCommunicationAttachmentContract.SCA_OriginalDocumentID = dr["SCA_OriginalDocumentID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SCA_OriginalDocumentID"]);
                                systemCommunicationAttachmentContract.SCA_OriginalDocumentName = dr["SCA_OriginalDocumentName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SCA_OriginalDocumentName"]);
                                systemCommunicationAttachmentContract.SCA_DocumentPath = AttachmentPath;
                                systemCommunicationAttachmentContract.SCA_SystemCommunicationID = dr["SCA_SystemCommunicationID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SCA_SystemCommunicationID"]);
                                systemCommunicationAttachmentContract.SCA_DocAttachmentTypeID = dr["SCA_DocAttachmentTypeID"].GetType().Name == "DBNull" ? (short)0 : Convert.ToInt16(dr["SCA_DocAttachmentTypeID"]);
                                systemCommunicationAttachmentContract.SCA_TenantID = dr["SCA_TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SCA_TenantID"]);
                                systemCommunicationAttachmentContract.SCA_IsDeleted = dr["SCA_IsDeleted"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["SCA_IsDeleted"]);

                                lkpDocumentAttachmentType documentAttchmentType = new lkpDocumentAttachmentType();
                                documentAttchmentType.DAT_Code = dr["SystemCommunicationAttachmentTypeCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SystemCommunicationAttachmentTypeCode"]);
                                if (!documentAttchmentType.DAT_Code.IsNullOrEmpty())
                                {
                                    systemCommunicationAttachmentContract.lkpDocumentAttachmentType = documentAttchmentType;
                                }

                                var systemCommunicationObject = result.Where(d => d.SystemCommunicationID == SystemCommunicationID).FirstOrDefault();
                                if (systemCommunicationObject.IsNullOrEmpty())//&& systemCommunicationObject.SystemCommunicationAttachments.IsNullOrEmpty())
                                {
                                    System.Data.Entity.Core.Objects.DataClasses.EntityCollection<SystemCommunicationAttachment> systemCommunicationAttachmentList = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<SystemCommunicationAttachment>();
                                    systemCommunicationAttachmentList.Add(systemCommunicationAttachmentContract);
                                    systemCommunicationContract.SystemCommunicationAttachments = systemCommunicationAttachmentList;
                                }
                                else
                                {
                                    var systemCommAttachmentList = systemCommunicationObject.SystemCommunicationAttachments.ToList();
                                    if (systemCommAttachmentList.Count > AppConsts.NONE)
                                    {
                                        if (!systemCommunicationContract.SystemCommunicationAttachments.Where(d => d.SCA_ID == systemCommunicationAttachmentContract.SCA_ID).Any())
                                            systemCommunicationContract.SystemCommunicationAttachments.Add(systemCommunicationAttachmentContract);
                                        //  systemCommAttachmentList.Add(systemCommunicationAttachmentContract);
                                    }
                                    else
                                    {
                                        System.Data.Entity.Core.Objects.DataClasses.EntityCollection<SystemCommunicationAttachment> systemCommunicationAttachmentList = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<SystemCommunicationAttachment>();
                                        if (!systemCommunicationContract.SystemCommunicationAttachments.Where(d => d.SCA_ID == systemCommunicationAttachmentContract.SCA_ID).Any())
                                        {
                                            systemCommunicationAttachmentList.Add(systemCommunicationAttachmentContract);
                                        }
                                        systemCommunicationContract.SystemCommunicationAttachments = systemCommunicationAttachmentList;
                                    }
                                }
                            }
                            #endregion

                            SystemCommunicationDelivery SystemCommunicationDeliveryContract = new SystemCommunicationDelivery();

                            #region System Communication Delivery
                            Int32 systemCommunicationDeliveryId = dr["SystemCommunicationDeliveryID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SystemCommunicationDeliveryID"]);
                            if (systemCommunicationDeliveryId > AppConsts.NONE)
                            {
                                SystemCommunicationDeliveryContract.SystemCommunicationDeliveryID = systemCommunicationDeliveryId;
                                SystemCommunicationDeliveryContract.SystemCommunicationTypeID = dr["SystemCommunicationTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SystemCommunicationTypeID"]);
                                SystemCommunicationDeliveryContract.ReceiverOrganizationUserID = dr["ReceiverOrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ReceiverOrganizationUserID"]);
                                SystemCommunicationDeliveryContract.RecieverEmailID = dr["RecieverEmailID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RecieverEmailID"]);
                                SystemCommunicationDeliveryContract.RecieverName = dr["RecieverName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RecieverName"]);
                                SystemCommunicationDeliveryContract.DispatchedDate = dr["DispatchedDate"].GetType().Name == "DBNull" ? (DateTime?)null : Convert.ToDateTime(dr["DispatchedDate"]);
                                SystemCommunicationDeliveryContract.IsDispatched = dr["IsDispatched"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsDispatched"]);
                                SystemCommunicationDeliveryContract.IsCC = dr["IsCC"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsCC"]);
                                SystemCommunicationDeliveryContract.IsBCC = dr["IsBCC"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsBCC"]);
                                SystemCommunicationDeliveryContract.RetryCount = dr["RetryCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RetryCount"]);
                                SystemCommunicationDeliveryContract.RetryErrorDate = dr["RetryErrorDate"].GetType().Name == "DBNull" ? (DateTime?)null : Convert.ToDateTime(dr["RetryErrorDate"]);
                                SystemCommunicationDeliveryContract.RetryErrorMessage = dr["RetryErrorMessage"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(dr["RetryErrorMessage"]);
                                SystemCommunicationDeliveryContract.NotificationDeliveryTypeID = dr["NotificationDeliveryTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["NotificationDeliveryTypeID"]);

                                var systemCommunicationDeliveryObject = result.Where(d => d.SystemCommunicationID == SystemCommunicationID).FirstOrDefault();
                                if (systemCommunicationDeliveryObject.IsNullOrEmpty())//&& systemCommunicationObject.SystemCommunicationAttachments.IsNullOrEmpty())
                                {
                                    System.Data.Entity.Core.Objects.DataClasses.EntityCollection<SystemCommunicationDelivery> systemCommunicationDeliveryList = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<SystemCommunicationDelivery>();
                                    systemCommunicationDeliveryList.Add(SystemCommunicationDeliveryContract);
                                    systemCommunicationContract.SystemCommunicationDeliveries = systemCommunicationDeliveryList;
                                }
                                else
                                {
                                    var systemCommDeliveryList = systemCommunicationDeliveryObject.SystemCommunicationDeliveries.ToList();
                                    if (systemCommDeliveryList.Count > AppConsts.NONE)
                                    {
                                        systemCommunicationContract.SystemCommunicationDeliveries.Add(SystemCommunicationDeliveryContract);
                                        //  systemCommAttachmentList.Add(systemCommunicationAttachmentContract);
                                    }
                                    else
                                    {
                                        System.Data.Entity.Core.Objects.DataClasses.EntityCollection<SystemCommunicationDelivery> systemCommunicationDeliveryList = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<SystemCommunicationDelivery>();
                                        systemCommunicationDeliveryList.Add(SystemCommunicationDeliveryContract);
                                        systemCommunicationContract.SystemCommunicationDeliveries = systemCommunicationDeliveryList;
                                    }
                                }
                            }

                            #endregion


                            if (!result.Where(d => d.SystemCommunicationID == SystemCommunicationID).Any())
                                result.Add(systemCommunicationContract);

                            systemCommunicationContract = null;
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }

            //return ADBMessageQueueContext.SystemCommunicationDeliveries
            //    .Include("SystemCommunication")
            //    .Include("SystemCommunication.SystemCommunicationAttachments")
            //    .Where(condition => !condition.IsDispatched && (condition.RetryCount == null || condition.RetryCount < maxRetryCount)
            //        && condition.CreatedOn <= createdOn)
            //    .OrderBy(x => new { x.RetryCount, x.CreatedOn }).Select(col => col.SystemCommunication).Distinct().ToList();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemCommunicationDeliveries"></param>
        /// <returns></returns>
        public bool SetDispatchedTrue(List<SystemCommunicationDelivery> systemCommunicationDeliveries, Int32 userId)
        {
            IEnumerable<Int32> systemCommunicationDeliveryIDs = systemCommunicationDeliveries.Select(x => x.SystemCommunicationDeliveryID);
            IEnumerable<SystemCommunicationDelivery> systemCommunicationDeliveriesInDb = ADBMessageQueueContext.SystemCommunicationDeliveries.Where(x => systemCommunicationDeliveryIDs.Contains(x.SystemCommunicationDeliveryID));
            foreach (SystemCommunicationDelivery deliveryInDb in systemCommunicationDeliveriesInDb)
            {
                deliveryInDb.DispatchedDate = DateTime.Now;
                deliveryInDb.IsDispatched = true;
                deliveryInDb.ModifiedOn = DateTime.Now;
                deliveryInDb.ModifiedByID = userId;

            }
            ADBMessageQueueContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<vw_ApplicantUser> GetApplicantUser()
        {
            return AppDBContext.vw_ApplicantUser;
        }

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="selectedEventIds"></param>
        /// <param name="modifiedByID"></param>
        /// <param name="isSubscribedByUser"></param>
        private void SetUserCommunicationSubscriptionSettingsInDb(Int32 organizationUserId, Int32 communicationTypeId, List<Int32> selectedEventIds, List<lkpCommunicationEvent> communicationEvents, Int32 modifiedByID, bool isSubscribedByUser)
        {
            IEnumerable<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettingsInDb = GetUserCommunicationSubscriptionSettings(organizationUserId, communicationTypeId);
            foreach (lkpCommunicationEvent communicationEvent in communicationEvents)
            {

                bool isSubscribedToUser = true;
                bool isSubscribedToAdmin = true;

                if (isSubscribedByUser)
                    isSubscribedToUser = selectedEventIds.Any(x => x.Equals(communicationEvent.CommunicationEventID));
                else
                    isSubscribedToAdmin = selectedEventIds.Any(x => x.Equals(communicationEvent.CommunicationEventID));


                UserCommunicationSubscriptionSetting userCommunicationSubscriptionSettingInDb =
                    userCommunicationSubscriptionSettingsInDb.FirstOrDefault(x => x.CommunicationTypeID.Equals(communicationTypeId)
                        && x.CommunicationEventID.Equals(communicationEvent.CommunicationEventID));

                if (userCommunicationSubscriptionSettingInDb == null)
                {
                    ADBMessageQueueContext.UserCommunicationSubscriptionSettings.AddObject(new UserCommunicationSubscriptionSetting()
                    {
                        OrganizationUserID = organizationUserId,
                        CommunicationTypeID = communicationTypeId,
                        CommunicationEventID = communicationEvent.CommunicationEventID,
                        IsSubscribedToUser = isSubscribedToUser,
                        IsSubscribedToAdmin = isSubscribedToAdmin,
                        CreatedByID = modifiedByID,
                        ModifiedByID = modifiedByID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now

                    });
                }
                else
                {
                    if (isSubscribedByUser && !userCommunicationSubscriptionSettingInDb.IsSubscribedToUser.Equals(isSubscribedToUser))
                    {
                        userCommunicationSubscriptionSettingInDb.IsSubscribedToUser = isSubscribedToUser;
                        userCommunicationSubscriptionSettingInDb.ModifiedOn = DateTime.Now;
                        userCommunicationSubscriptionSettingInDb.ModifiedByID = modifiedByID;
                    }
                    else if (!userCommunicationSubscriptionSettingInDb.IsSubscribedToAdmin.Equals(isSubscribedToAdmin))
                    {
                        userCommunicationSubscriptionSettingInDb.IsSubscribedToAdmin = isSubscribedToAdmin;
                        userCommunicationSubscriptionSettingInDb.ModifiedOn = DateTime.Now;
                        userCommunicationSubscriptionSettingInDb.ModifiedByID = modifiedByID;
                    }
                }
            }
        }

        private void SetUserCommunicationSubscriptionSettingsInDb(Int32 modifiedById, bool isSelectedSubscriptionSettings, List<UserCommunicationSubscriptionSetting> subscriptionSettings)
        {
            foreach (UserCommunicationSubscriptionSetting subscriptionSetting in subscriptionSettings)
            {
                UserCommunicationSubscriptionSetting subscriptionSettingInDb =
                ADBMessageQueueContext.UserCommunicationSubscriptionSettings
                .FirstOrDefault(x =>
                    x.OrganizationUserID.Equals(subscriptionSetting.OrganizationUserID)
                    && x.CommunicationTypeID.Equals(subscriptionSetting.CommunicationTypeID)
                    && x.CommunicationEventID.Equals(subscriptionSetting.CommunicationEventID));

                if (subscriptionSettingInDb != null)
                {
                    if (subscriptionSettingInDb.IsSubscribedToAdmin != isSelectedSubscriptionSettings)
                    {
                        subscriptionSettingInDb.IsSubscribedToAdmin = isSelectedSubscriptionSettings;
                        subscriptionSettingInDb.ModifiedOn = DateTime.Now;
                        subscriptionSettingInDb.ModifiedByID = modifiedById;
                    }
                }
                else
                {
                    ADBMessageQueueContext.UserCommunicationSubscriptionSettings.AddObject(new UserCommunicationSubscriptionSetting()
                    {
                        OrganizationUserID = subscriptionSetting.OrganizationUserID,
                        CommunicationTypeID = subscriptionSetting.CommunicationTypeID,
                        CommunicationEventID = subscriptionSetting.CommunicationEventID,
                        IsSubscribedToUser = true,
                        IsSubscribedToAdmin = isSelectedSubscriptionSettings,
                        CreatedByID = modifiedById,
                        ModifiedByID = modifiedById,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }
            }
        }

        #endregion

        #region Communication Summary
        /// <summary>
        /// commented by :Charanjot For UAT-1427: WB: 
        /// </summary>
        /// <returns></returns>
        //public IQueryable<CommunicationTemplateContract> GetCommunicationSummary()
        //{
        //    var communicationSummary = ADBMessageQueueContext.SystemCommunications.Join(ADBMessageQueueContext.SystemCommunicationDeliveries,
        //              sCommunication => sCommunication.SystemCommunicationID,
        //              sCommunicationDelivery => sCommunicationDelivery.SystemCommunicationTypeID,
        //              (sCommunication, sCommunicationDelivery) =>
        //                  new CommunicationTemplateContract
        //                  {
        //                      SenderEmailId = sCommunication.SenderEmailID,
        //                      SenderName = sCommunication.SenderName,
        //                      ReceiverEmailId = sCommunicationDelivery.RecieverEmailID,
        //                      ReceiverName = sCommunicationDelivery.RecieverName,
        //                      DispatchDate = sCommunicationDelivery.DispatchedDate,
        //                      IsDispatched = sCommunicationDelivery.IsDispatched ? true : false,
        //                      SystemCommunicationID = sCommunication.SystemCommunicationID,
        //                      Subject = sCommunication.Subject,
        //                      SystemCommunicationDeliveryID = sCommunicationDelivery.SystemCommunicationDeliveryID,
        //                      CommunicationSubEvent = sCommunication.lkpCommunicationSubEvent.Name,
        //                      CommunicationSubEventID = sCommunication.CommunicationSubEventID,
        //                      ReceiverOrganizationUserId = sCommunicationDelivery.ReceiverOrganizationUserID,
        //                      RecipientType = (sCommunicationDelivery.IsCC.HasValue && sCommunicationDelivery.IsCC.Value) ? "Cc" : ((sCommunicationDelivery.IsBCC.HasValue && sCommunicationDelivery.IsBCC.Value) ? "Bcc" : "To")
        //                  }).Where(sCommunication => sCommunication.IsDispatched == true);

        //    return communicationSummary;
        //}

        public SystemCommunication GetSystemNotificationDetails(Int32 systemCommunicationId)
        {
            return ADBMessageQueueContext.SystemCommunications.Where(sysCommId => sysCommId.SystemCommunicationID.Equals(systemCommunicationId)).FirstOrDefault();
        }

        public SystemCommunication GetSystemNotificationDetailsArchive(Int32 systemCommunicationId)
        {
            SystemCommunication systemcommunication = new SystemCommunication();
            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[usp_GetSystemCommunicationfromArchiveDB]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SystemCommunicationID", systemCommunicationId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    systemcommunication.Content = Convert.ToString(ds.Tables[0].Rows[0]["SCAR_Content"]);
                    systemcommunication.Subject = Convert.ToString(ds.Tables[0].Rows[0]["SCAR_Subject"]);
                    return systemcommunication;
                }
            }
            return new SystemCommunication();
        }

        public void ReSendEmail(Int32 systemCommunicationDeliveryId, Int32 currentUserId, Boolean isAttachment = false)
        {
            List<SystemCommunicationDelivery> previousSystemCommunicationDeliveries = ADBMessageQueueContext.SystemCommunicationDeliveries.Include(SysXEntityConstants.TABLE_SYSTEM_COMMUNICATION).Where(sysCommId => sysCommId.SystemCommunicationDeliveryID.Equals(systemCommunicationDeliveryId)).ToList();
            SystemCommunication previousSystemCommunication = previousSystemCommunicationDeliveries.FirstOrDefault().SystemCommunication;
            List<SystemCommunicationAttachment> previousSystemCommunicationAttachments = ADBMessageQueueContext.SystemCommunicationAttachments.Include(SysXEntityConstants.TABLE_SYSTEM_COMMUNICATION).Where(sysCommId => sysCommId.SCA_SystemCommunicationID.Equals(previousSystemCommunication.SystemCommunicationID)).ToList();

            DateTime creationDate = DateTime.Now;
            SystemCommunication systemCommunication = new SystemCommunication
            {
                Content = previousSystemCommunication.Content,
                Subject = previousSystemCommunication.Subject,
                SenderEmailID = previousSystemCommunication.SenderEmailID,
                SenderName = previousSystemCommunication.SenderName,
                CommunicationSubEventID = previousSystemCommunication.CommunicationSubEventID,
                CreatedByID = currentUserId,
                CreatedOn = creationDate,
            };

            #region Override Entity Command Timeout Setting
            Int32 TimeoutSetting = AppConsts.NONE;
            if (!System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"].IsNullOrEmpty())
            {
                TimeoutSetting = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"]);
                if (TimeoutSetting > AppConsts.NONE)
                {
                    ADBMessageQueueContext.CommandTimeout = TimeoutSetting;
                }
            }
            #endregion

            //ADBMessageQueueContext.SystemCommunications.AddObject(systemCommunication);
            Int32 systemCommunicationID = InsertSystemCommunication(systemCommunication);

            foreach (SystemCommunicationDelivery previousSystemCommunicationDelivery in previousSystemCommunicationDeliveries)
            {
                SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery
                {
                    RecieverEmailID = previousSystemCommunicationDelivery.RecieverEmailID,
                    RecieverName = previousSystemCommunicationDelivery.RecieverName,
                    ReceiverOrganizationUserID = previousSystemCommunicationDelivery.ReceiverOrganizationUserID,
                    IsCC = previousSystemCommunicationDelivery.IsCC,
                    IsDispatched = false,
                    CreatedByID = currentUserId,
                    CreatedOn = creationDate,
                    //SystemCommunication = systemCommunication,
                    SystemCommunicationTypeID = systemCommunicationID,
                    RetryCount = previousSystemCommunicationDelivery.RetryCount
                };
                ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(systemCommunicationDelivery);
            }

            #region UAT-3261: Badge Form Enhancements
            if (isAttachment)
            {
                foreach (SystemCommunicationAttachment previousSystemCommunicationAttachment in previousSystemCommunicationAttachments)
                {
                    SystemCommunicationAttachment systemCommunicationAttachment = new SystemCommunicationAttachment
                    {
                        SCA_OriginalDocumentID = previousSystemCommunicationAttachment.SCA_OriginalDocumentID,
                        SCA_OriginalDocumentName = previousSystemCommunicationAttachment.SCA_OriginalDocumentName,
                        SCA_DocumentPath = previousSystemCommunicationAttachment.SCA_DocumentPath,
                        SCA_DocumentSize = previousSystemCommunicationAttachment.SCA_DocumentSize,
                        SCA_SystemCommunicationID = systemCommunication.SystemCommunicationID,
                        SCA_DocAttachmentTypeID = previousSystemCommunicationAttachment.SCA_DocAttachmentTypeID,
                        SCA_TenantID = previousSystemCommunicationAttachment.SCA_TenantID,
                        SCA_IsDeleted = previousSystemCommunicationAttachment.SCA_IsDeleted,
                        SCA_CreatedBy = previousSystemCommunicationAttachment.SCA_CreatedBy,
                        SCA_CreatedOn = previousSystemCommunicationAttachment.SCA_CreatedOn,
                        SCA_ModifiedBy = previousSystemCommunicationAttachment.SCA_ModifiedBy,
                        SCA_ModifiedOn = previousSystemCommunicationAttachment.SCA_ModifiedOn
                    };
                    ADBMessageQueueContext.SystemCommunicationAttachments.AddObject(systemCommunicationAttachment);
                }
            }
            #endregion


            ADBMessageQueueContext.SaveChanges();
        }

        public List<SystemCommunicationDelivery> GetSysCommunicationDeliveriesByIds(List<Int32> systemCommunicationDeliveryIds)
        {
            return ADBMessageQueueContext.SystemCommunicationDeliveries.Include(SysXEntityConstants.TABLE_SYSTEM_COMMUNICATION).Where(sysComDelivery => systemCommunicationDeliveryIds.Contains(sysComDelivery.SystemCommunicationDeliveryID)).ToList();
        }

        public List<CommunicationTemplateContract> GetSysCommunicationDeliveriesByIdsArchive(List<Int32> systemCommunicationDeliveryIds)
        {
            List<CommunicationTemplateContract> lstSystemCommunicationDelivery = new List<CommunicationTemplateContract>();
            String SysCommunicationDeliveryIds = String.Join(",", systemCommunicationDeliveryIds);

            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSysCommunicationDeliveriesByIdsArchive", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SysCommunicationDeliveryIds", SysCommunicationDeliveryIds);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
                        communicationTemplateContract.SystemCommunicationDeliveryID = ds.Tables[0].Rows[i]["SCDA_SystemCommunicationDeliveryID"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["SCDA_SystemCommunicationDeliveryID"]);
                        communicationTemplateContract.ReceiverOrganizationUserId = ds.Tables[0].Rows[i]["SCDA_ReceiverOrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[i]["SCDA_ReceiverOrganizationUserID"]);
                        communicationTemplateContract.CommunicationSubEventID = ds.Tables[0].Rows[i]["SCAR_CommunicationSubEventID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(ds.Tables[0].Rows[i]["SCAR_CommunicationSubEventID"]);
                        lstSystemCommunicationDelivery.Add(communicationTemplateContract);
                    }
                return lstSystemCommunicationDelivery;
            }
        }
        /// <summary>
        /// get the list of communication summary by search operation 
        /// Created by :charanjot For UAT-1427: WB:  
        /// </summary>
        /// <param name="searchDataContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        public List<CommunicationTemplateContract> GetCommunicationSummarySearch(SearchCommunicationTemplateContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            List<CommunicationTemplateContract> communicationContractList = new List<CommunicationTemplateContract>();
            String orderBy = QueueConstants.COM_SUMMARY_DEFAULT_SORTING_FIELDS;
            String orderDirection = null;
            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            orderDirection = gridCustomPaging.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSystemCommunicationSummary", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailType", searchDataContract.EmailType);
                command.Parameters.AddWithValue("@Receiver", searchDataContract.Receiver);
                command.Parameters.AddWithValue("@ReceiverEmailId", searchDataContract.ReceiverEmailId);
                command.Parameters.AddWithValue("@Bcc", searchDataContract.IsBcc);
                command.Parameters.AddWithValue("@TO", searchDataContract.IsTo);
                command.Parameters.AddWithValue("@Cc", searchDataContract.IsCc);
                command.Parameters.AddWithValue("@DispatchDate", searchDataContract.DispatchDate);
                command.Parameters.AddWithValue("@Subject", searchDataContract.Subject);
                command.Parameters.AddWithValue("@filteringSortingData", gridCustomPaging.XML);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gridCustomPaging.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        gridCustomPaging.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            CommunicationTemplateContract communicationData = new CommunicationTemplateContract();
                            communicationData.SystemCommunicationID = Convert.ToInt32(ds.Tables[1].Rows[i]["SystemCommunicationID"]);
                            communicationData.CommunicationSubEventID = ds.Tables[1].Rows[i]["CommunicationSubEventID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(ds.Tables[1].Rows[i]["CommunicationSubEventID"]);
                            communicationData.SystemCommunicationDeliveryID = Convert.ToInt32(ds.Tables[1].Rows[i]["SystemCommunicationDeliveryID"]);
                            communicationData.SenderEmailID = Convert.ToString(ds.Tables[1].Rows[i]["SenderEmailID"]);
                            communicationData.SenderName = Convert.ToString(ds.Tables[1].Rows[i]["SenderName"]);
                            communicationData.RecieverEmailID = Convert.ToString(ds.Tables[1].Rows[i]["RecieverEmailID"]);
                            communicationData.RecieverName = Convert.ToString(ds.Tables[1].Rows[i]["RecieverName"]);
                            communicationData.DispatchedDate = ds.Tables[1].Rows[i]["DispatchedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds.Tables[1].Rows[i]["DispatchedDate"]);
                            communicationData.IsDispatched = ds.Tables[1].Rows[i]["IsDispatched"] == DBNull.Value ? false : true;
                            communicationData.Subject = ds.Tables[1].Rows[i]["Subject"] == DBNull.Value ? String.Empty : Convert.ToString(ds.Tables[1].Rows[i]["Subject"]);
                            communicationData.Name = Convert.ToString(ds.Tables[1].Rows[i]["Name"]);
                            communicationData.ReceiverOrganizationUserId = Convert.ToInt32(ds.Tables[1].Rows[i]["ReceiverOrganizationUserID"]);
                            communicationData.RecipientType = Convert.ToString(ds.Tables[1].Rows[i]["RecipientType"]);
                            // communicationData.TotalRecordCount = Convert.ToInt32(ds.Tables[0].Rows[0]["VirtualCount"]);
                            communicationContractList.Add(communicationData);
                        }
                    }
                }
                return communicationContractList;
            }
        }

        /// <summary>
        /// Gets the list of communication summary Archive
        /// Created by Pawan Kapoor For UAT-3333 
        /// </summary>
        /// <param name="searchDataContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        public List<CommunicationTemplateContract> GetCommunicationSummarySearchArchive(SearchCommunicationTemplateContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            List<CommunicationTemplateContract> communicationContractList = new List<CommunicationTemplateContract>();
            String orderBy = QueueConstants.COM_SUMMARY_DEFAULT_SORTING_FIELDS;
            String orderDirection = null;
            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            orderDirection = gridCustomPaging.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSystemCommunicationSummary_Archive", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailType", searchDataContract.EmailType);
                command.Parameters.AddWithValue("@Receiver", searchDataContract.Receiver);
                command.Parameters.AddWithValue("@ReceiverEmailId", searchDataContract.ReceiverEmailId);
                command.Parameters.AddWithValue("@Bcc", searchDataContract.IsBcc);
                command.Parameters.AddWithValue("@TO", searchDataContract.IsTo);
                command.Parameters.AddWithValue("@Cc", searchDataContract.IsCc);
                command.Parameters.AddWithValue("@DispatchDate", searchDataContract.DispatchDate);
                command.Parameters.AddWithValue("@Subject", searchDataContract.Subject);
                command.Parameters.AddWithValue("@filteringSortingData", gridCustomPaging.XML);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gridCustomPaging.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        gridCustomPaging.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            CommunicationTemplateContract communicationData = new CommunicationTemplateContract();
                            communicationData.SystemCommunicationID = Convert.ToInt32(ds.Tables[1].Rows[i]["SystemCommunicationID"]);
                            communicationData.CommunicationSubEventID = ds.Tables[1].Rows[i]["CommunicationSubEventID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(ds.Tables[1].Rows[i]["CommunicationSubEventID"]);
                            communicationData.SystemCommunicationDeliveryID = Convert.ToInt32(ds.Tables[1].Rows[i]["SystemCommunicationDeliveryID"]);
                            communicationData.SenderEmailID = Convert.ToString(ds.Tables[1].Rows[i]["SenderEmailID"]);
                            communicationData.SenderName = Convert.ToString(ds.Tables[1].Rows[i]["SenderName"]);
                            communicationData.RecieverEmailID = Convert.ToString(ds.Tables[1].Rows[i]["RecieverEmailID"]);
                            communicationData.RecieverName = Convert.ToString(ds.Tables[1].Rows[i]["RecieverName"]);
                            communicationData.DispatchedDate = ds.Tables[1].Rows[i]["DispatchedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds.Tables[1].Rows[i]["DispatchedDate"]);
                            communicationData.IsDispatched = ds.Tables[1].Rows[i]["IsDispatched"] == DBNull.Value ? false : true;
                            communicationData.Subject = ds.Tables[1].Rows[i]["Subject"] == DBNull.Value ? String.Empty : Convert.ToString(ds.Tables[1].Rows[i]["Subject"]);
                            communicationData.Name = Convert.ToString(ds.Tables[1].Rows[i]["Name"]);
                            communicationData.ReceiverOrganizationUserId = Convert.ToInt32(ds.Tables[1].Rows[i]["ReceiverOrganizationUserID"]);
                            communicationData.RecipientType = Convert.ToString(ds.Tables[1].Rows[i]["RecipientType"]);
                            // communicationData.TotalRecordCount = Convert.ToInt32(ds.Tables[0].Rows[0]["VirtualCount"]);
                            communicationContractList.Add(communicationData);
                        }
                    }
                }
                return communicationContractList;
            }
        }

        #endregion

        #region Communication Mock Up

        public CommunicationMockUpData GetMockUpData(Int32 subEventId)
        {
            return ADBMessageQueueContext.CommunicationMockUpDatas.Where(cMockData => cMockData.SubEventID == subEventId).FirstOrDefault();
        }

        #endregion

        #region Send Message to Queue

        public void SendMessageContentToQueue(MessagingContract messagingContract)
        {
            QueueClientService queueService = new QueueClientService();
            queueService.SendMessageToQueue(messagingContract);
        }

        #endregion

        #region Communication CC Master
        public List<GetCommunicationCCusersList> getCommunicationCCusers(Int32 communicationSubEventId, Int32 tenantId)
        {
            return ADBMessageQueueContext.usp_GetCommunicationCCusersData(communicationSubEventId, tenantId).ToList();
        }

        public IQueryable<ExternalCopyUser> GetExternalCopyUsers(Int32 communicationSubEventId, Int32 hierarchyNodeID, Int32 tenantId)
        {
            return ADBMessageQueueContext.ExternalCopyUsers.Include("lkpCopyType")
                     .Where(x => x.ECU_CommunicationSubEventID == communicationSubEventId && x.ECU_HierarchyNodeID == hierarchyNodeID && x.ECU_TenantID == tenantId
                        && x.ECU_IsDeleted == false);
        }

        public List<HierarchyNotificationMapping> GetHierarchyNotificationMapping(Int32 communicationSubEventId, Int32 hierarchyNodeID, Int32 tenantId)
        {
            return ADBMessageQueueContext.HierarchyNotificationMappings.Include("lkpCopyType")
                       .Where(x => x.HNM_SubEventID == communicationSubEventId && x.HNM_HierarchyNodeID == hierarchyNodeID && x.HNM_TenantID == tenantId
                          && x.HNM_IsDeleted == false).ToList();
        }

        //List<CommunicationCCMaster> ICommunicationRepository.GetTenantSpecificCommunicationCCMaster(Int32 tenantID)
        //{
        //    var communicationCCMasters = ADBMessageQueueContext.CommunicationCCMasters.
        //        Where(cond => cond.TenantID == tenantID && cond.IsDeleted == false).ToList();

        //    return communicationCCMasters;
        //}

        List<CommunicationCCMaster> ICommunicationRepository.GetTenantSpecificCommunicationCCMaster(Int32 tenantID, Int32 DefaultTenantID, Int32 CurrentUserId, String UserType = null)
        {
            //if (CurrentUserId == DefaultTenantID)
            //{
            //    var communicationCCMasters = ADBMessageQueueContext.CommunicationCCMasters.Include("CommunicationCCUsers")
            //                   .Where(cond => cond.TenantID == tenantID && cond.IsDeleted == false).ToList();
            //    return communicationCCMasters;
            //}

            //UAT 1043 WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
            if (UserType == AppConsts.USERTYPE_SUPERADMIN || UserType == AppConsts.USERTYPE_ADBUSER)
            {
                var communicationCCMasters = ADBMessageQueueContext.CommunicationCCMasters.Include("CommunicationCCUsers")
                              .Where(cond => cond.TenantID == tenantID && cond.IsDeleted == false).ToList();
                return communicationCCMasters;
            }
            else
            {
                var communicationCCMasters = ADBMessageQueueContext.CommunicationCCMasters.Include("CommunicationCCUsers")
                      .Where(cond => cond.TenantID == tenantID && cond.CommunicationCCUsers.Any(x => x.OrganizationUserID == CurrentUserId)
                          && cond.IsDeleted == false).ToList();//cond.CommunicationCCUsers.Where(x => x.OrganizationUserID == CurrentUserId) &&

                return communicationCCMasters;
            }
        }


        CommunicationCCMaster ICommunicationRepository.GetCommunicationCCMaster(Int32 communicationCcMasterID, Int32 tenantID)
        {
            var communicationCCMasters = ADBMessageQueueContext.CommunicationCCMasters.
                Where(cond => cond.TenantID == tenantID && cond.CommunicationCCMasterID == communicationCcMasterID
                    && cond.IsDeleted == false).FirstOrDefault();
            return communicationCCMasters;
        }

        //IEnumerable<CommunicationCCUser> ICommunicationRepository.GetCommunicationCCUser(Int32 communicationCCMasterID)
        //{
        //    return ADBMessageQueueContext.CommunicationCCUsers.Include("lkpCopyType").Where(cond => cond.CommunicationCCMasterID == communicationCCMasterID && cond.IsDeleted == false);
        //}
        IEnumerable<CommunicationCCUser> ICommunicationRepository.GetCommunicationCCUser(Int32 communicationCCMasterID, Int32 DefaultTenantID, Int32 CurrentUserId, String UserType = null)
        {

            //if (CurrentUserId == DefaultTenantID)
            //{
            //    return ADBMessageQueueContext.CommunicationCCUsers.Include("lkpCopyType").Where(cond => cond.CommunicationCCMasterID == communicationCCMasterID && cond.IsDeleted == false);
            //}

            //UAT 1043 WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
            if (UserType == AppConsts.USERTYPE_SUPERADMIN || UserType == AppConsts.USERTYPE_ADBUSER)
            {
                return ADBMessageQueueContext.CommunicationCCUsers.Include("lkpCopyType").Where(cond => cond.CommunicationCCMasterID == communicationCCMasterID && cond.IsDeleted == false);
            }
            else
            {
                return ADBMessageQueueContext.CommunicationCCUsers.Include("lkpCopyType").Where(cond => cond.CommunicationCCMasterID == communicationCCMasterID && cond.OrganizationUserID == CurrentUserId && cond.IsDeleted == false);
            }
        }

        List<CommunicationCCUserContract> ICommunicationRepository.GetCommunicationCCUserAndSettings(Int32 communicationCCMasterID, Int32 CurrentUserId, String UserType = null)
        {
            List<CommunicationCCUserContract> communicationCCUserContractList = new List<CommunicationCCUserContract>();

            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                bool isAdminOrSuperAdminUserType = false;
                //UAT 1043 WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
                if ((UserType == AppConsts.USERTYPE_SUPERADMIN || UserType == AppConsts.USERTYPE_ADBUSER))
                    isAdminOrSuperAdminUserType = true;

                SqlCommand command = new SqlCommand("usp_GetCommunicationCCUserAndSettings", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CommunicationCCMasterID", communicationCCMasterID);
                command.Parameters.AddWithValue("@CurrentUserId", CurrentUserId);
                command.Parameters.AddWithValue("@IsAdminOrSuperAdminUserType", isAdminOrSuperAdminUserType);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0 && !ds.Tables[0].IsNullOrEmpty())
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CommunicationCCUserContract communicationCCUserContract = new CommunicationCCUserContract();
                        communicationCCUserContract.IsForEmail = string.IsNullOrEmpty(row["IsEmail"].ToString()) ? false : Convert.ToBoolean(row["IsEmail"].ToString());
                        communicationCCUserContract.IsForCommunicationCentre = string.IsNullOrEmpty(row["IsCommunicationCentre"].ToString()) ? false : Convert.ToBoolean(row["IsCommunicationCentre"].ToString());
                        communicationCCUserContract.CopyTypeName = Convert.ToString(row["CT_Name"]);
                        communicationCCUserContract.CopyTypeCode = Convert.ToString(row["CT_Code"]);
                        communicationCCUserContract.UserFirstName = Convert.ToString(row["FirstName"]);
                        communicationCCUserContract.CommunicationCCMasterID = Convert.ToInt32(row["CommunicationCCMasterID"]);
                        communicationCCUserContract.UserLastName = Convert.ToString(row["LastName"]);
                        communicationCCUserContract.UserMiddleName = Convert.ToString(row["UserMiddleName"]);
                        communicationCCUserContract.UserEmailAddress = Convert.ToString(row["Email"]);
                        communicationCCUserContract.OrganizationUserID = Convert.ToInt32(row["OrganizationUserID"]);
                        communicationCCUserContract.CommunicationCCUsersID = Convert.ToInt32(row["CommunicationCCUsersID"]);

                        communicationCCUserContract.SelectedRecordTypeName = Convert.ToString(row["RecordObjectTypeName"]);
                        communicationCCUserContract.SelectedRecordTypeCode = Convert.ToString(row["RecordObjectTypeCode"]);
                        communicationCCUserContract.SelectedRecordTypeId = Convert.ToInt32(row["RecordObjectTypeId"]);
                        communicationCCUserContract.SelectedRecordIdsStr = Convert.ToString(row["RecordIds"]);
                        //communicationCCUserContract.SelectedRecordIds = new List<int>();

                        //foreach (string recordId in Convert.ToString(row["RecordIds"]).Split(','))
                        //{
                        //    if (!String.IsNullOrEmpty(recordId))
                        //        communicationCCUserContract.SelectedRecordIds.Add(Convert.ToInt32(recordId));
                        //}
                        //communicationCCUserContract.SelectedRecordNames=

                        communicationCCUserContractList.Add(communicationCCUserContract);
                    }
                }
            }
            return communicationCCUserContractList;
        }


        Boolean ICommunicationRepository.SaveCommunicationCCMaster(CommunicationCCMaster communicationCCMasterObj)
        {
            ADBMessageQueueContext.CommunicationCCMasters.AddObject(communicationCCMasterObj);
            ADBMessageQueueContext.SaveChanges();
            return true;
        }

        Boolean ICommunicationRepository.IsUserAlreadyMappedToSubEvent(Int32 communicationCCMasterID, Int32 currentUserID, Int32 subEventID)
        {
            Int32 _communicationCCUserid = 0;
            CommunicationCCMaster communicationCCMaster = ADBMessageQueueContext.CommunicationCCMasters.Include("CommunicationCCUsers").Where(x => x.CommunicationCCMasterID == communicationCCMasterID && x.CommunicationSubEventID == subEventID && x.IsDeleted == false).FirstOrDefault();
            if (communicationCCMaster.IsNotNull())
            {

                _communicationCCUserid = communicationCCMaster.CommunicationCCUsers.FirstOrDefault(x => x.IsDeleted == false).CommunicationCCUsersID;
            }
            return ADBMessageQueueContext.CommunicationCCUsers.Where(x => x.CommunicationCCUsersID == _communicationCCUserid).Any(x => x.OrganizationUserID == currentUserID);
        }
        //
        Boolean ICommunicationRepository.SaveCommunicationCcUsers(List<CommunicationCCUser> communicationCcUserList, Int32 currentUserId, Int32 communicationCCMasterID)
        {
            foreach (var item in communicationCcUserList)
            {
                if (item.CommunicationCCUsersID > AppConsts.NONE)
                {
                    var itemToUpd = ADB_MessageQueueContext.CommunicationCCUsers.Where(x => x.CommunicationCCUsersID == item.CommunicationCCUsersID).FirstOrDefault();
                    if (itemToUpd.IsNotNull())
                    {
                        itemToUpd.CopyTypeID = item.CopyTypeID;
                        itemToUpd.IsEmail = item.IsEmail;
                        itemToUpd.IsOnlyRotationCreatedNotification = item.IsOnlyRotationCreatedNotification;
                        itemToUpd.IsCommunicationCentre = item.IsCommunicationCentre;
                        itemToUpd.IsDeleted = item.IsDeleted;
                        itemToUpd.ModifiedBy = currentUserId;
                        itemToUpd.ModifiedOn = DateTime.Now;

                        if (item.CommunicationCCUsersSettings.Count > 0)
                        {
                            var tmpCommunicationCCUsersSettings = item.CommunicationCCUsersSettings.Clone();

                            foreach (var _CCUserSetting in tmpCommunicationCCUsersSettings)
                            {
                                itemToUpd.CommunicationCCUsersSettings.Add(_CCUserSetting);
                            }
                        }

                        var existingCommunicationCCUsersSettings = ADB_MessageQueueContext.CommunicationCCUsersSettings
                                                                        .Where(cond => cond.CCUS_CommunicationCCUsersID == item.CommunicationCCUsersID && cond.CCUS_IsDeleted == false).ToList();

                        if (!existingCommunicationCCUsersSettings.IsNullOrEmpty())
                        {
                            foreach (var _CCUserSetting in existingCommunicationCCUsersSettings)
                            {
                                _CCUserSetting.CCUS_IsDeleted = true;
                                _CCUserSetting.CCUS_ModifiedBy = currentUserId;
                                _CCUserSetting.CCUS_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                }
                else
                {
                    ADBMessageQueueContext.CommunicationCCUsers.AddObject(item);
                }
            }

            ADBMessageQueueContext.SaveChanges();

            Boolean isUserMappingExist = ADB_MessageQueueContext.CommunicationCCMasters.Include("CommunicationCCUsers").
                                   Any(cond => cond.CommunicationCCMasterID == communicationCCMasterID && cond.CommunicationCCUsers.Any(x => x.IsDeleted == false));
            if (!isUserMappingExist)
            {
                CommunicationCCMaster CommCCMaster = ADB_MessageQueueContext.CommunicationCCMasters.FirstOrDefault(x => x.CommunicationCCMasterID == communicationCCMasterID && x.IsDeleted == false);
                CommCCMaster.IsDeleted = true;
                CommCCMaster.ModifiedBy = currentUserId;
                CommCCMaster.ModifiedOn = DateTime.Now;
                ADB_MessageQueueContext.SaveChanges();
            }

            return true;
        }

        Boolean ICommunicationRepository.DeleteCommunicationCcMaster(Int32 selectedTenantId, Int32 communicationCCMasterID, Int32 currentUserId)
        {
            var commCcMasterToUpd = ADB_MessageQueueContext.CommunicationCCMasters.Include("CommunicationCCUsers").
                                    Where(cond => cond.CommunicationCCMasterID == communicationCCMasterID).FirstOrDefault();
            if (commCcMasterToUpd.IsNotNull())
            {
                commCcMasterToUpd.IsDeleted = true;
                commCcMasterToUpd.ModifiedBy = currentUserId;
                commCcMasterToUpd.ModifiedOn = DateTime.Now;

                var communicationCCUsersToUpd = commCcMasterToUpd.CommunicationCCUsers.ToList();
                foreach (var item in communicationCCUsersToUpd)
                {
                    item.IsDeleted = true;
                    item.ModifiedBy = currentUserId;
                    item.ModifiedOn = DateTime.Now;

                    foreach (var ccUserSetting in item.CommunicationCCUsersSettings)
                    {
                        ccUserSetting.CCUS_IsDeleted = true;
                        ccUserSetting.CCUS_ModifiedBy = currentUserId;
                        ccUserSetting.CCUS_ModifiedOn = DateTime.Now;
                    }

                }

                ADB_MessageQueueContext.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion

        #region SEND NOTIFICATION ON FIRST ITEM SUBMITT
        public Boolean IsFirstItemNotificationExist(Int32 subEventId, Int32 userId)
        {
            return ADBMessageQueueContext.SystemCommunicationDeliveries.Include(SysXEntityConstants.TABLE_SYSTEM_COMMUNICATION).Any(cond => cond.ReceiverOrganizationUserID == userId && cond.SystemCommunication.CommunicationSubEventID == subEventId);
        }
        #endregion

        #region External User BCC

        /// <summary>
        /// Get the list of communication type for client admin 
        /// </summary>
        /// <param name="categoryTypeId">categoryTypeId</param>
        /// <returns></returns>
        public List<lkpCommunicationSubEvent> GetCommunicationSubEventsType(List<String> subEventCode)
        {
            List<lkpCommunicationSubEvent> lkpCommunicationSubEvents = ADBMessageQueueContext.lkpCommunicationSubEvents.Where(obj => obj.IsDeleted == false).ToList();

            if (lkpCommunicationSubEvents.IsNotNull())
            {
                foreach (String item in subEventCode)
                {
                    lkpCommunicationSubEvents.Remove(lkpCommunicationSubEvents.Where(obj => obj.Code.Equals(item.ToString())).FirstOrDefault());
                }
            }
            return lkpCommunicationSubEvents;
        }

        public List<ExternalCopyUser> GetBCCExternalUserData(Int32 tenantId, Int32 hierarchyNodeId, Int16 copyType)
        {
            List<ExternalCopyUser> externalCopyUser = null;

            if (copyType.IsNotNull() && copyType > 0)
            {
                externalCopyUser = ADBMessageQueueContext.ExternalCopyUsers.Include("lkpCommunicationSubEvent").Where(obj => obj.ECU_CopyTypeID == copyType && obj.ECU_TenantID == tenantId && obj.ECU_HierarchyNodeID == hierarchyNodeId && obj.ECU_IsDeleted == false).ToList();
            }
            return externalCopyUser;
        }

        public Boolean SaveRecordExternalUserBCC(Entity.ExternalCopyUser externalCopyuser)
        {
            ADBMessageQueueContext.ExternalCopyUsers.AddObject(externalCopyuser);
            if (ADBMessageQueueContext.SaveChanges() > 0)
                return true;
            return false;
        }

        public Boolean UpdateRecordExternalUserBCC(ExternalCopyUserContract externalCopyUserContract, Int32 externalCopyUserID, Int32 CurrentLoggedInUserId)
        {
            ExternalCopyUser externalCopyUser = GetExternalCopyUserRecordById(externalCopyUserID);
            if (externalCopyUser.IsNotNull())
            {
                externalCopyUser.ECU_FirstName = externalCopyUserContract.FirstName;
                externalCopyUser.ECU_LastName = externalCopyUserContract.LastName;
                externalCopyUser.ECU_CommunicationSubEventID = externalCopyUserContract.CommunicationSubEventID;
                externalCopyUser.ECU_EmailID = externalCopyUserContract.EmailAddress;
                externalCopyUser.ECU_ModifiedByID = CurrentLoggedInUserId;
                externalCopyUser.ECU_ModifiedOn = DateTime.Now;

                if (ADBMessageQueueContext.SaveChanges() > 0)
                    return true;
            }
            return false;
        }
        public Boolean DeleteExternalUserBCC(Int32 externalCopyUserID, Int32 CurrentLoggedInUserId)
        {
            ExternalCopyUser externalCopyUser = GetExternalCopyUserRecordById(externalCopyUserID);
            if (externalCopyUser.IsNotNull())
            {
                externalCopyUser.ECU_IsDeleted = true;
                externalCopyUser.ECU_ModifiedOn = DateTime.Now;
                externalCopyUser.ECU_ModifiedByID = CurrentLoggedInUserId;
                if (ADBMessageQueueContext.SaveChanges() > 0)
                    return true;
            }
            return false;
        }

        public ExternalCopyUser GetExternalCopyUserRecordById(Int32 externalCopyUserID)
        {
            return ADBMessageQueueContext.ExternalCopyUsers.Where(obj => obj.ECU_ID == externalCopyUserID && obj.ECU_IsDeleted == false).FirstOrDefault();
        }
        public Boolean IsExternalUserExistForSubEvent(ExternalCopyUserContract externalCopyUserContract, Int32 hierarchyNodeId, Int32 tenantId, Int16 copyType, Int32? externalCopyUserId)
        {
            if (externalCopyUserId.IsNull())
            {
                return ADB_MessageQueueContext.ExternalCopyUsers.Any(obj => obj.ECU_EmailID.Equals(externalCopyUserContract.EmailAddress) && obj.ECU_CommunicationSubEventID == (externalCopyUserContract.CommunicationSubEventID)
                     && obj.ECU_HierarchyNodeID == (hierarchyNodeId) && obj.ECU_TenantID == (tenantId) && obj.ECU_CopyTypeID == (copyType) && obj.ECU_IsDeleted == false);
            }
            else if (externalCopyUserId.IsNotNull())
            {
                List<ExternalCopyUser> ExternalCopyUser = ADB_MessageQueueContext.ExternalCopyUsers.Where(obj => obj.ECU_IsDeleted == false && obj.ECU_ID != externalCopyUserId).ToList();
                if (ExternalCopyUser.IsNotNull())
                {
                    return ExternalCopyUser.Any(obj => obj.ECU_EmailID.Equals(externalCopyUserContract.EmailAddress) && obj.ECU_CommunicationSubEventID == (externalCopyUserContract.CommunicationSubEventID)
                             && obj.ECU_HierarchyNodeID == (hierarchyNodeId) && obj.ECU_TenantID == (tenantId) && obj.ECU_CopyTypeID == (copyType));
                }
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
        List<HierarchyNotificationMapping> ICommunicationRepository.GetHierarchyNotificationMappingData(Int32 tenantId, Int32 hierarchyNodeID, Int32 hierarchyContactMappingID)
        {
            return ADB_MessageQueueContext.HierarchyNotificationMappings.Include("lkpCommunicationSubEvent").Include("lkpCopyType")
                 .Where(cond => cond.HNM_TenantID == tenantId && cond.HNM_HierarchyNodeID == hierarchyNodeID && cond.HNM_HierarchyContactMappingID == hierarchyContactMappingID && cond.HNM_IsDeleted == false).ToList();

        }


        /// <summary>
        /// Method is used to get the list of SubEvents based on the code + common event
        /// </summary>
        /// <param name="subEventCategoryTypeCode"></param>
        /// <returns></returns>
        List<lkpCommunicationSubEvent> ICommunicationRepository.GetlkpCommunicationSubEvent(String subEventCategoryBusinessChannel)
        {

            return ADB_MessageQueueContext.lkpCommunicationSubEvents.Include("lkpSubEventCategoryType")
                  .Where(cond => cond.lkpSubEventCategoryType.SCT_BusinessChannelType == subEventCategoryBusinessChannel || cond.lkpSubEventCategoryType.SCT_BusinessChannelType == AppConsts.BUSINESS_CHANNEL_EVENT_COMMON).ToList();

        }



        /// <summary>
        /// Method is used to update the communication sub event
        /// </summary>
        /// <param name="hNotificationMappingContract"></param>
        /// <param name="hNotificationMappingID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        Boolean ICommunicationRepository.UpdateCommunicationSubEvent(Int32 tenantId, HierarchyNotificationMappingContract hNotificationMappingContract, Int32 hNotificationMappingID, Int32 currentLoggedInUserId)
        {

            HierarchyNotificationMapping hNotificationMapping = ADB_MessageQueueContext.HierarchyNotificationMappings.Where(obj => obj.HNM_ID == hNotificationMappingID && obj.HNM_TenantID == tenantId).FirstOrDefault();
            if (!hNotificationMapping.IsNull())
            {
                hNotificationMapping.HNM_CopyTypeID = hNotificationMappingContract.CopyTypeID;
                hNotificationMapping.HNM_SubEventID = hNotificationMappingContract.CommunicationSubEventID;
                hNotificationMapping.HNM_IsCommunicationCenter = hNotificationMappingContract.IsCommunicationCenter;
                hNotificationMapping.HNM_IsEmail = hNotificationMappingContract.IsEmail;
                hNotificationMapping.HNM_IsDeleted = hNotificationMappingContract.IsDeleted;
                hNotificationMapping.HNM_ModifiedByID = currentLoggedInUserId;
                hNotificationMapping.HNM_ModifiedOn = DateTime.Now;

                if (ADB_MessageQueueContext.SaveChanges() > 0)
                {
                    return true;
                }

            }

            return false;
        }


        /// <summary>
        /// Method is used to save the communication event
        /// </summary>
        /// <param name="hNotificationMapping"></param>
        /// <returns></returns>
        Boolean ICommunicationRepository.SaveCommunicationSubEvent(HierarchyNotificationMapping hNotificationMapping)
        {
            ADBMessageQueueContext.HierarchyNotificationMappings.AddObject(hNotificationMapping);
            if (ADBMessageQueueContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Method is used to delete the CommunicationSubEvents
        /// </summary>
        /// <param name="hNotificationMappingID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        Boolean ICommunicationRepository.DeleteCommunicationSubEvent(Int32 tenantId, Int32 hNotificationMappingID, Int32 currentLoggedInUserId)
        {
            HierarchyNotificationMapping hNotificationMapping = ADB_MessageQueueContext.HierarchyNotificationMappings.Where(obj => obj.HNM_ID == hNotificationMappingID && obj.HNM_TenantID == tenantId).FirstOrDefault();
            if (!hNotificationMapping.IsNull())
            {
                hNotificationMapping.HNM_IsDeleted = true;
                hNotificationMapping.HNM_ModifiedByID = currentLoggedInUserId;
                hNotificationMapping.HNM_ModifiedOn = DateTime.Now;
                if (ADB_MessageQueueContext.SaveChanges() > 0)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Method is used to delete the list of  CommunicationSubEvents
        /// </summary>
        /// <param name="hNotificationMappingID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        Boolean ICommunicationRepository.DeleteCommunicationSubEventList(Int32 tenantId, List<Int32> hNotificationMappingIDs, Int32 currentLoggedInUserId)
        {
            foreach (Int32 hNotificationMappingID in hNotificationMappingIDs)
            {
                HierarchyNotificationMapping hNotificationMapping = ADB_MessageQueueContext.HierarchyNotificationMappings.Where(obj => obj.HNM_ID == hNotificationMappingID && obj.HNM_TenantID == tenantId).FirstOrDefault();
                if (!hNotificationMapping.IsNull())
                {
                    hNotificationMapping.HNM_IsDeleted = true;
                    hNotificationMapping.HNM_ModifiedByID = currentLoggedInUserId;
                    hNotificationMapping.HNM_ModifiedOn = DateTime.Now;
                    ADB_MessageQueueContext.SaveChanges();
                }
            }

            return true;
        }


        /// <summary>
        /// Method is used to get the list of SubEvents based on the code + common event
        /// </summary>
        /// <param name="subEventCategoryTypeCode"></param>
        /// <returns></returns>
        List<lkpCopyType> ICommunicationRepository.GetlkpCopyType()
        {
            //Get the common code
            return ADB_MessageQueueContext.lkpCopyTypes.ToList();

        }

        /// <summary>
        /// Method is used to get the list of SubEvents based on the code + common event
        /// </summary>
        /// <param name="subEventCategoryTypeCode"></param>
        /// <returns></returns>
        List<Entity.lkpCommunicationSubEvent> ICommunicationRepository.GetlkpCommunicationSubEvent()
        {
            //Get the common code
            return ADB_MessageQueueContext.lkpCommunicationSubEvents.ToList();

        }

        #endregion

        /// <summary>
        /// To Get Communication Sub Event by EntityID, EntityType and CommunicationSubEvent code.
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="communicationSubEvent"></param>
        /// <returns>lkpCommunicationSubEvent</returns>
        lkpCommunicationSubEvent ICommunicationRepository.GetCommSubEventByEntity(Int32 entityID, String entityTypeCode, String communicationSubEventCode)
        {
            List<CommunicationTemplate> commTemplate = GetCommunicationTemplateMappedWithEntity(entityID, entityTypeCode);
            if (commTemplate.IsNullOrEmpty())
            {
                return null;
            }
            if (communicationSubEventCode == CommunicationSubEvents.DEFAULT_SVC_FORM.GetStringValue())
            {
                return commTemplate.FirstOrDefault().lkpCommunicationSubEvent;
            }
            else
            {
                return commTemplate.FirstOrDefault(cond => cond.lkpCommunicationSubEvent.Code == communicationSubEventCode).lkpCommunicationSubEvent;
            }
        }

        List<Int32> ICommunicationRepository.GetCommunicationTemplateIDByEntity(Int32 entityID, String entityTypeCode, String communicationSubEventCode)
        {
            List<CommunicationTemplate> commTemplate = GetCommunicationTemplateMappedWithEntity(entityID, entityTypeCode);

            if (commTemplate.IsNullOrEmpty())
            {
                return new List<Int32>(); ;
            }
            if (communicationSubEventCode == CommunicationSubEvents.DEFAULT_SVC_FORM.GetStringValue())
            {
                return commTemplate.Select(x => x.CommunicationTemplateID).ToList();
            }
            else
            {
                return commTemplate.Where(cond => cond.lkpCommunicationSubEvent.Code == communicationSubEventCode).Select(x => x.CommunicationTemplateID).ToList();
            }
        }

        private List<CommunicationTemplate> GetCommunicationTemplateMappedWithEntity(Int32 entityID, String entityTypeCode)
        {
            List<CommunicationTemplate> commTemplate = new List<CommunicationTemplate>();

            commTemplate = ADBMessageQueueContext.CommunicationTemplateEntities
                               .Include("CommunicationTemplateEntities.lkpCommunicationEntityType")
                               .Include("CommunicationTemplateEntities.CommunicationTemplates")
                               .Include("CommunicationTemplateEntities.CommunicationTemplates.lkpCommunicationSubEvent")
                               .Where(condition => condition.lkpCommunicationEntityType.CET_Code == entityTypeCode
                                    && condition.CTE_EntityID == entityID && condition.CTE_IsDeleted == false
                                    && condition.CommunicationTemplate.lkpCommunicationSubEvent.CommunicationSubEventID != null
                                    && condition.CommunicationTemplate.IsDeleted == false)
                                    .Select(x => x.CommunicationTemplate).ToList();

            if (entityTypeCode == "AAAA" && commTemplate.IsNull() || (commTemplate.IsNotNull() && commTemplate.Count == 0))
            {//get template mapping of parent form if template mapping is not found for current form
                Entity.ServiceAttachedForm serviceForm = base.AppDBContext.ServiceAttachedForms.Where(x => x.SF_ID == entityID).FirstOrDefault();

                commTemplate = ADBMessageQueueContext.CommunicationTemplateEntities
                          .Include("CommunicationTemplateEntities.lkpCommunicationEntityType")
                          .Include("CommunicationTemplateEntities.CommunicationTemplates")
                          .Include("CommunicationTemplateEntities.CommunicationTemplates.lkpCommunicationSubEvent")
                          .Where(condition => condition.lkpCommunicationEntityType.CET_Code == entityTypeCode
                               && condition.CTE_EntityID == serviceForm.SF_ParentServiceFormID && condition.CTE_IsDeleted == false
                               && condition.CommunicationTemplate.lkpCommunicationSubEvent.CommunicationSubEventID != null
                               && condition.CommunicationTemplate.IsDeleted == false)
                               .Select(x => x.CommunicationTemplate).ToList();
            }
            return commTemplate;
        }


        Int32 ICommunicationRepository.SaveSystemCommunicationAttachment(SystemCommunicationAttachment sysCommAttachment)
        {
            ADBMessageQueueContext.SystemCommunicationAttachments.AddObject(sysCommAttachment);

            if (ADBMessageQueueContext.SaveChanges() > 0)
                return sysCommAttachment.SCA_ID;
            else
                return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemCommunicationDeliveries"></param>
        /// <returns></returns>
        public bool SetRetryCountAndMessage(List<SystemCommunicationDelivery> systemCommunicationDeliveries, Int32 userId, String errorMessage)
        {
            IEnumerable<Int32> systemCommunicationDeliveryIDs = systemCommunicationDeliveries.Select(x => x.SystemCommunicationDeliveryID);
            IEnumerable<SystemCommunicationDelivery> systemCommunicationDeliveriesInDb = ADBMessageQueueContext.SystemCommunicationDeliveries.Where(x => systemCommunicationDeliveryIDs.Contains(x.SystemCommunicationDeliveryID));
            foreach (SystemCommunicationDelivery deliveryInDb in systemCommunicationDeliveriesInDb)
            {
                deliveryInDb.RetryCount = deliveryInDb.RetryCount + 1;
                deliveryInDb.RetryErrorDate = DateTime.Now;
                deliveryInDb.RetryErrorMessage = errorMessage;
                deliveryInDb.ModifiedOn = DateTime.Now;
                deliveryInDb.ModifiedByID = userId;
            }
            ADBMessageQueueContext.SaveChanges();
            return true;
        }

        private List<Entity.ClientEntity.CommunicationCCUsersList> GetCCusers(Int32 communicationSubEventId, Int32 tenantId, CommunicationSubEvents communicationSubEvents, Int32 hierarchyNodeID, String rotationHierarchyNodeID)
        {
            if ((communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS
                    || communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS)
                && hierarchyNodeID <= 0)
            {
                return new List<Entity.ClientEntity.CommunicationCCUsersList>();
            }
            ComplianceSetupRepository cmpSetUpRepo = new ComplianceSetupRepository(tenantId);
            //Added check for those notifications which don't have relation with institute hierarchy. e.g. Rotation About To Start mails
            Int32? hierarchy = hierarchyNodeID > 0 ? hierarchyNodeID : (Int32?)null;
            if (!rotationHierarchyNodeID.IsNullOrEmpty())
            {
                return cmpSetUpRepo.GetCCusers(communicationSubEventId, tenantId, rotationHierarchyNodeID).ToList();
            }
            else
            {
                return cmpSetUpRepo.GetCCusers(communicationSubEventId, tenantId, hierarchy.ToString()).ToList();
            }
        }

        List<MessageDetail> ICommunicationRepository.GetUserMessages(Int32 userId, Int32 pageSize, Int32 pageIndex, String defaultExpression, Boolean isSortDirectionDescending, Int32 ApplicantDashboard = 0)
        {
            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[usp_Report_AllRecentMessagesOfUser_Dashboard]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserID", userId);
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@defaultExpression", defaultExpression);
                command.Parameters.AddWithValue("@isSortDirectionDescending", isSortDirectionDescending);
                command.Parameters.AddWithValue("@ApplicantDashboard", ApplicantDashboard);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetMessageData(ds.Tables[0]);
                }
            }
            return new List<MessageDetail>();
        }

        private List<MessageDetail> SetMessageData(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new MessageDetail
            {
                MessageDetailID = (Guid)(x["ADBMessageID"]),
                CommunicationTypeCode = Convert.ToString(x["CommunicationTypeCode"]),
                IsHighImportant = Convert.ToBoolean(x["IsHighImportant"]),
                FromUserId = Convert.ToInt32(x["FromID"]),
                From = Convert.ToString(x["From"]),
                Subject = Convert.ToString(x["Subject"]),
                ReceivedDateFormat = Convert.ToString(x["ReceivedDateFormat"]),
                TotalRecords = Convert.ToInt32(x["TotalRecords"])
                //Level = Convert.ToInt32(Convert.ToString("Levels")),
            }).ToList();
        }

        List<CommunicationCCUser> ICommunicationRepository.GetNotificationSettingData(Int32 organizationUserID)
        {
            //return ADBMessageQueueContext.CommunicationCCUsers
            //                                .Include("CommunicationCCMaster")
            //                                .Include("CommunicationCCMaster.lkpCommunicationSubEvent")
            //                                .Include("lkpCopyType")
            //                                .Where(cond => cond.OrganizationUserID == organizationUserID && cond.IsDeleted == false && cond.lkpCopyType.CT_IsDeleted == false
            //                                    && cond.CommunicationCCMaster.lkpCommunicationSubEvent.IsDeleted == false && cond.CommunicationCCMaster.IsDeleted == false)
            //                                .ToList();


            //UAT-1419	WB: The CC settings for notification of “Daily Completed Service Group report and notification” is not appearing on Search result screen.
            return ADBMessageQueueContext.CommunicationCCUsers
                                .Include("CommunicationCCMaster")
                                .Include("CommunicationCCMaster.lkpCommunicationSubEvent")
                                .Include("lkpCopyType")
                                .Where(cond => cond.OrganizationUserID == organizationUserID && cond.IsDeleted == false
                                    && cond.CommunicationCCMaster.lkpCommunicationSubEvent.IsDeleted == false && cond.CommunicationCCMaster.IsDeleted == false)
                                .ToList();
        }

        #region Service Form Sub Events

        Boolean ICommunicationRepository.SaveServiceAttachedFormCommunicationTemplate(List<Entity.lkpCommunicationSubEvent> commSubEventsList)
        {
            commSubEventsList.ForEach(cse =>
            {
                ADBMessageQueueContext.lkpCommunicationSubEvents.AddObject(cse);
            });

            if (ADBMessageQueueContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerable<CommunicationTemplateEntity> ICommunicationRepository.GetCommunicationTemplateEntityData(Int32 serviceFormID)
        {
            return ADB_MessageQueueContext.CommunicationTemplateEntities.Include("CommunicationTemplate")
                 .Where(cond => cond.CTE_EntityID == serviceFormID && cond.CTE_IsDeleted == false);
        }

        CommunicationTemplate ICommunicationRepository.GetCommunicationTemplateData(Int32 communicationTemplateID)
        {
            return ADB_MessageQueueContext.CommunicationTemplates.Where(cond => cond.CommunicationTemplateID == communicationTemplateID && cond.IsDeleted == false)
                    .FirstOrDefault();
        }

        Boolean ICommunicationRepository.UpdateServiceAttachedFormCommunicationTemplate()
        {
            ADB_MessageQueueContext.SaveChanges();
            return true;
        }

        Boolean ICommunicationRepository.DeleteServiceAttachedFormCommunicationData(Int32 serviceAttachedFormID, Int32 currntUserID, Boolean VersionServiceFormToDelete)
        {
            var communicationTemplateEntityList = ADB_MessageQueueContext.CommunicationTemplateEntities.Include("CommunicationTemplate")
                                                .Where(cond => cond.CTE_EntityID == serviceAttachedFormID && cond.CTE_IsDeleted == false);

            if (communicationTemplateEntityList.IsNotNull() && communicationTemplateEntityList.Count() > AppConsts.NONE)
            {
                communicationTemplateEntityList.ForEach(cte =>
                {
                    var communicationTemplate = ADB_MessageQueueContext.CommunicationTemplates.Where(cond => cond.CommunicationTemplateID == cte.CTE_TemplateID
                                               && cte.CTE_IsDeleted == false);
                    communicationTemplate.ForEach(ct =>
                    {
                        var communicationSubEvent = ADB_MessageQueueContext.lkpCommunicationSubEvents.Where(cond => cond.CommunicationSubEventID == ct.CommunicationSubEventID
                                                        && cond.IsDeleted == false).FirstOrDefault();
                        if (VersionServiceFormToDelete)
                        {
                            if (communicationSubEvent.IsNotNull())
                            {
                                communicationSubEvent.IsDeleted = true;
                                communicationSubEvent.ModifiedByID = currntUserID;
                                communicationSubEvent.ModifiedOn = DateTime.Now;
                            }

                            ct.IsDeleted = true;
                            ct.ModifiedByID = currntUserID;
                            ct.ModifiedOn = DateTime.Now;
                        }
                    });

                    cte.CTE_IsDeleted = true;
                    cte.CTE_ModifiedBy = currntUserID;
                    cte.CTE_ModifiedOn = DateTime.Now;
                });
                ADB_MessageQueueContext.SaveChanges();
            }
            return true;
        }

        Boolean ICommunicationRepository.InsertCommunicationTemplatesEntities(List<Entity.CommunicationTemplateEntity> communicationTemplateEntitiesToBeAdded)
        {
            communicationTemplateEntitiesToBeAdded.ForEach(cte =>
            {
                ADB_MessageQueueContext.AddToCommunicationTemplateEntities(cte);
            });

            ADB_MessageQueueContext.SaveChanges();
            return true;
        }

        #endregion


        #region Employment Notification For FlaggedOrder
        List<Entity.ClientEntity.CommunicationCCUsersList> ICommunicationRepository.GetClientAdminsForEmploymentNotification(Int32 communicationSubEventId, Int32 tenantId, CommunicationSubEvents communicationSubEvents, Int32 hierarchyNodeID)
        {
            return GetCCusers(communicationSubEventId, tenantId, communicationSubEvents, hierarchyNodeID, null);
        }
        #endregion



        public int GetCommunicationTemplateIDForSubEventID(int subEventID)
        {
            return ADBMessageQueueContext.CommunicationTemplates.Where(x => x.CommunicationSubEventID == subEventID).OrderByDescending(x => x.CreatedOn).Select(x => x.CommunicationTemplateID).FirstOrDefault();
        }

        #region UAT 1230 and UAT 1302

        /// <summary>
        /// UAT 1230:As an admin, I should be able to invite a person (or group of people) to create an applicant account
        /// UAT 1302:As an admin (client or ADB), I should be able to create preceptors/instructors
        /// </summary>
        /// <param name="lstSystemCommunications"></param>
        /// <returns></returns>
        Boolean ICommunicationRepository.SaveSysCommunicationAndSysDeliveries(List<SystemCommunication> lstSystemCommunications)
        {
            foreach (SystemCommunication systemCommunication in lstSystemCommunications)
            {
                #region Override Entity Command Timeout Setting
                Int32 TimeoutSetting = AppConsts.NONE;
                if (!System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"].IsNullOrEmpty())
                {
                    TimeoutSetting = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"]);
                    if (TimeoutSetting > AppConsts.NONE)
                    {
                        ADBMessageQueueContext.CommandTimeout = TimeoutSetting;
                    }
                }
                #endregion

                ADB_MessageQueueContext.SystemCommunications.AddObject(systemCommunication);
            }
            if (ADB_MessageQueueContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region UAT 1302:As an admin (client or ADB), I should be able to create preceptors/instructors

        Boolean ICommunicationRepository.CheckIsContactAlreadyRecievdEmail(Int32 subEventID, String emailID)
        {
            List<Int32> systemCommunicationIDList = ADB_MessageQueueContext.SystemCommunications.Where(x => x.CommunicationSubEventID == subEventID).Select(x => x.SystemCommunicationID).ToList(); //.Select(x => x.SystemCommunicationID)
            List<SystemCommunicationDelivery> SystemCommunicationDeliveryList = ADB_MessageQueueContext.SystemCommunicationDeliveries.Where(x => systemCommunicationIDList.Contains(x.SystemCommunicationTypeID)).ToList();
            return ADB_MessageQueueContext.SystemCommunicationDeliveries.Any(x => systemCommunicationIDList.Contains(x.SystemCommunicationTypeID) && x.RecieverEmailID == emailID && x.IsDispatched);
        }
        #endregion

        List<SystemEventSetting> ICommunicationRepository.GetSystemEventSetting(Int32 subEventId)
        {
            return ADBMessageQueueContext.SystemEventSettings
                .Where(condition => condition.CommunicationSubEventID == subEventId && condition.IsDeleted == false).ToList();
        }

        List<CommunicationTemplate> ICommunicationRepository.GetCommunicationTemplate(Int32 subEventId)
        {
            return ADBMessageQueueContext.CommunicationTemplates.Where(condition => condition.CommunicationSubEventID == subEventId && condition.IsDeleted == false).ToList();
        }

        public Int32 SaveMailContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, List<Entity.OrganizationUser> lstCCUsers, List<Entity.OrganizationUser> lstBCCUsers, List<SystemCommunicationAttachment> lstSystemCommunicationAttachment)
        {
            DateTime creationDate = DateTime.Now;

            systemCommunication.SenderEmailID = communicationTemplateContract.SenderEmailID;
            systemCommunication.SenderName = communicationTemplateContract.SenderName;
            systemCommunication.CreatedByID = communicationTemplateContract.CurrentUserId;
            systemCommunication.CreatedOn = creationDate;
            systemCommunication.CreatedOn = creationDate;

            #region Override Entity Command Timeout Setting
            Int32 TimeoutSetting = AppConsts.NONE;
            if (!System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"].IsNullOrEmpty())
            {
                TimeoutSetting = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"]);
                if (TimeoutSetting > AppConsts.NONE)
                {
                    ADBMessageQueueContext.CommandTimeout = TimeoutSetting;
                }
            }
            #endregion

            //ADBMessageQueueContext.SystemCommunications.AddObject(systemCommunication);
            Int32 systemCommunicationID = InsertSystemCommunication(systemCommunication);

            SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery
            {
                RecieverEmailID = communicationTemplateContract.RecieverEmailID,
                RecieverName = communicationTemplateContract.RecieverName,
                IsDispatched = false,
                IsCC = false,
                IsBCC = false,
                CreatedByID = communicationTemplateContract.CurrentUserId,
                CreatedOn = creationDate,
                //SystemCommunication = systemCommunication,
                SystemCommunicationTypeID = systemCommunicationID,
                ReceiverOrganizationUserID = communicationTemplateContract.ReceiverOrganizationUserId,
                RetryCount = 0
            };
            ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(systemCommunicationDelivery);

            //Adding CC users to System Communication Deliveries
            if (lstCCUsers.IsNotNull() && lstCCUsers.Count > 0)
            {
                foreach (Entity.OrganizationUser ccUser in lstCCUsers)
                {
                    SystemCommunicationDelivery systemCommunicationDeliveryCC = new SystemCommunicationDelivery
                    {
                        RecieverEmailID = ccUser.PrimaryEmailAddress.IsNullOrEmpty() ? ccUser.aspnet_Users.aspnet_Membership.Email : ccUser.PrimaryEmailAddress,
                        RecieverName = string.Concat(ccUser.FirstName, " ", ccUser.LastName),
                        IsDispatched = false,
                        IsCC = true,
                        IsBCC = false,
                        CreatedByID = communicationTemplateContract.CurrentUserId,
                        CreatedOn = creationDate,
                        //SystemCommunication = systemCommunication,
                        SystemCommunicationTypeID = systemCommunicationID,
                        ReceiverOrganizationUserID = ccUser.OrganizationUserID,
                        RetryCount = 0
                    };
                    ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(systemCommunicationDeliveryCC);
                }
            }

            //Adding BCC users to System Communication Deliveries
            if (lstBCCUsers.IsNotNull() && lstBCCUsers.Count > 0)
            {
                foreach (Entity.OrganizationUser bccUser in lstBCCUsers)
                {
                    SystemCommunicationDelivery systemCommunicationDeliveryBCC = new SystemCommunicationDelivery
                    {
                        RecieverEmailID = bccUser.PrimaryEmailAddress.IsNullOrEmpty() ? bccUser.aspnet_Users.aspnet_Membership.Email : bccUser.PrimaryEmailAddress,//bccUser.PrimaryEmailAddress,
                        RecieverName = string.Concat(bccUser.FirstName, " ", bccUser.LastName),
                        IsDispatched = false,
                        IsCC = false,
                        IsBCC = true,
                        CreatedByID = communicationTemplateContract.CurrentUserId,
                        CreatedOn = creationDate,
                        //SystemCommunication = systemCommunication,
                        SystemCommunicationTypeID = systemCommunicationID,
                        ReceiverOrganizationUserID = bccUser.OrganizationUserID,
                        RetryCount = 0
                    };
                    ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(systemCommunicationDeliveryBCC);
                }
            }

            //Adding Attachments
            foreach (var systemCommunicationAttachment in lstSystemCommunicationAttachment)
            {
                systemCommunicationAttachment.SCA_SystemCommunicationID = systemCommunicationID;
                ADBMessageQueueContext.SystemCommunicationAttachments.AddObject(systemCommunicationAttachment);
            }

            ADBMessageQueueContext.SaveChanges();
            return systemCommunication.SystemCommunicationID;
        }

        List<lkpRecordObjectType> ICommunicationRepository.GetlkpRecordObjectType()
        {
            return ADB_MessageQueueContext.lkpRecordObjectTypes.Where(cond => cond.OT_IsDeleted != true).ToList();
        }

        private List<Entity.ClientEntity.CommunicationCCUsersList> GetCCusersWithNodePermissionAndCCUserSettings(Int32 communicationSubEventId, Int32 tenantId, CommunicationSubEvents communicationSubEvents, Int32 hierarchyNodeID, Int32 objectTypeId, Int32 recordId)
        {
            if ((communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS
                    || communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS)
                && hierarchyNodeID <= 0)
            {
                return new List<Entity.ClientEntity.CommunicationCCUsersList>();
            }
            ComplianceSetupRepository cmpSetUpRepo = new ComplianceSetupRepository(tenantId);
            //Added check for those notifications which don't have relation with institute hierarchy. e.g. Rotation About To Start mails
            Int32? hierarchy = hierarchyNodeID > 0 ? hierarchyNodeID : (Int32?)null;
            return cmpSetUpRepo.GetCCusersWithNodePermissionAndCCUserSettings(communicationSubEventId, tenantId, hierarchyNodeID, objectTypeId, recordId).ToList();
        }

        public Int32 SaveMailContentForCCUserSettings(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 communicationSubEventID, Int32 tenantId, Int32 hierarchyNodeID, List<ExternalCopyUsersContract> externalCopyUsers, CommunicationSubEvents communicationSubEvents, Int32 objectTypeId, Int32 recordId, Boolean overrideCcBcc = false, List<BackgroundOrderDailyReport> lstBackgroundOrderDailyReport = null)
        {
            DateTime creationDate = DateTime.Now;

            systemCommunication.SenderEmailID = communicationTemplateContract.SenderEmailID;
            systemCommunication.SenderName = communicationTemplateContract.SenderName;
            systemCommunication.CreatedByID = communicationTemplateContract.CurrentUserId;
            systemCommunication.CreatedOn = creationDate;
            systemCommunication.CreatedOn = creationDate;

            #region Override Entity Command Timeout Setting
            Int32 TimeoutSetting = AppConsts.NONE;
            if (!System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"].IsNullOrEmpty())
            {
                TimeoutSetting = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"]);
                if (TimeoutSetting > AppConsts.NONE)
                {
                    ADBMessageQueueContext.CommandTimeout = TimeoutSetting;
                }
            }
            #endregion

            //ADBMessageQueueContext.SystemCommunications.AddObject(systemCommunication);
            Int32 systemCommunicationID = InsertSystemCommunication(systemCommunication);

            SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery
            {
                RecieverEmailID = communicationTemplateContract.RecieverEmailID,
                RecieverName = communicationTemplateContract.RecieverName,
                IsDispatched = false,
                IsCC = false,
                CreatedByID = communicationTemplateContract.CurrentUserId,
                CreatedOn = creationDate,
                //SystemCommunication = systemCommunication,
                SystemCommunicationTypeID = systemCommunicationID,
                ReceiverOrganizationUserID = communicationTemplateContract.ReceiverOrganizationUserId,
                RetryCount = 0
            };
            ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(systemCommunicationDelivery);


            //UAT 867 : Creation of Background  Orders with Color Flag and Flagged Status Results notification report
            if (hierarchyNodeID == -1
                && lstBackgroundOrderDailyReport.IsNotNull() && lstBackgroundOrderDailyReport.Count > 0)
            {
                lstBackgroundOrderDailyReport.ForEach(condition =>
                {
                    SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                    {
                        RecieverEmailID = condition.EmailAddress,
                        RecieverName = condition.UserName,
                        IsDispatched = false,
                        IsCC = !overrideCcBcc,
                        IsBCC = !overrideCcBcc,
                        CreatedByID = communicationTemplateContract.CurrentUserId,
                        CreatedOn = creationDate,
                        //SystemCommunication = systemCommunication,
                        SystemCommunicationTypeID = systemCommunicationID,
                        ReceiverOrganizationUserID = condition.OrganizationUserId,
                    };
                    ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                });
            }
            else if (tenantId > AppConsts.NONE)
            {
                String ccCode = CopyType.CC.GetStringValue();
                String bccCode = CopyType.BCC.GetStringValue();
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //if (lstClientAdmins == null)
                //{
                String key = communicationSubEventID.ToString();
                if (hierarchyNodeID.IsNotNull())
                {
                    key = key + "_" + hierarchyNodeID.ToString(); // added hierarchy node in key corresponding to UAT - 868 : As a Client admin, I should only receive CC/BCC emails for nodes to which I have access
                }
                key = key + "_" + tenantId.ToString();
                List<Entity.ClientEntity.CommunicationCCUsersList> defaultCommunicationCCusers = new List<Entity.ClientEntity.CommunicationCCUsersList>();
                if (System.Web.HttpContext.Current != null)
                {
                    if (System.Web.HttpContext.Current.Items[key] != null)
                    {
                        defaultCommunicationCCusers = (List<Entity.ClientEntity.CommunicationCCUsersList>)System.Web.HttpContext.Current.Items[key];
                    }
                    else
                    {
                        defaultCommunicationCCusers = GetCCusersWithNodePermissionAndCCUserSettings(communicationSubEventID, tenantId, communicationSubEvents, hierarchyNodeID, objectTypeId, recordId);
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
                        defaultCommunicationCCusers = GetCCusersWithNodePermissionAndCCUserSettings(communicationSubEventID, tenantId, communicationSubEvents, hierarchyNodeID, objectTypeId, recordId);
                        //Every Time need to get CC users, Because depend's upon permission
                        //[UAT-1072]
                        //if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        //{
                        //    if (ServiceContext.Current.DataDict == null)
                        //        ServiceContext.Current.DataDict = new Dictionary<String, object>();
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
                        defaultCommunicationCCusers = GetCCusersWithNodePermissionAndCCUserSettings(communicationSubEventID, tenantId, communicationSubEvents, hierarchyNodeID, objectTypeId, recordId);
                        //Every Time need to get CC users, Because depend's upon permission
                        //[UAT-1072]
                        //if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                        //{
                        //    if (ParallelTaskContext.Current.DataDict == null)
                        //        ParallelTaskContext.Current.DataDict = new Dictionary<String, object>();
                        //    ParallelTaskContext.Current.DataDict.Add(key, defaultCommunicationCCusers);
                        //}
                    }
                }
                if (defaultCommunicationCCusers != null && defaultCommunicationCCusers.Count > AppConsts.NONE)
                {
                    defaultCommunicationCCusers = defaultCommunicationCCusers.Where(condition => condition.IsEmail).ToList();
                    foreach (Entity.ClientEntity.CommunicationCCUsersList defaultCommunicationCCuser in defaultCommunicationCCusers)
                    {
                        SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                        {
                            RecieverEmailID = defaultCommunicationCCuser.EmailAddress,
                            RecieverName = defaultCommunicationCCuser.UserName,
                            IsDispatched = false,
                            IsCC = overrideCcBcc ? (communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS || communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS ? false : defaultCommunicationCCuser.CopyCode == ccCode ? true : false) : defaultCommunicationCCuser.CopyCode == ccCode ? true : false,
                            IsBCC = overrideCcBcc ? (communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS || communicationSubEvents == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS ? false : defaultCommunicationCCuser.CopyCode == bccCode ? true : false) : defaultCommunicationCCuser.CopyCode == bccCode ? true : false,
                            CreatedByID = communicationTemplateContract.CurrentUserId,
                            CreatedOn = creationDate,
                            //SystemCommunication = systemCommunication,
                            SystemCommunicationTypeID = systemCommunicationID,
                            ReceiverOrganizationUserID = defaultCommunicationCCuser.UserID, //communicationTemplateContract.ReceiverOrganizationUserId
                        };
                        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                    }
                }
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //}
                //else
                //{
                //    foreach (var clientAdmin in lstClientAdmins)
                //    {
                //        SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                //        {
                //            RecieverEmailID = clientAdmin.EmailAddress,
                //            RecieverName = clientAdmin.FullName,
                //            IsDispatched = false,
                //            IsCC = false,
                //            IsBCC = false,
                //            CreatedByID = communicationTemplateContract.CurrentUserId,
                //            CreatedOn = creationDate,
                //            SystemCommunication = systemCommunication,
                //            ReceiverOrganizationUserID = clientAdmin.OrganizationUserId, //communicationTemplateContract.ReceiverOrganizationUserId,
                //            RetryCount = 0
                //        };
                //        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                //    }
                //}
                //To get external BCC users on the basis of Hierarchy Node ID, Tenant ID and Communication Sub Event ID

                #region Old Code
                //List<ExternalCopyUsersContract> externalCopyUsers = new List<ExternalCopyUsersContract>();
                //String externalEmailkey = communicationSubEventID.ToString() + "_" + tenantId.ToString() + "_" + hierarchyNodeID;

                //if (System.Web.HttpContext.Current != null)
                //{
                //    if (System.Web.HttpContext.Current.Items[externalEmailkey] != null)
                //    {
                //        externalCopyUsers = (List<ExternalCopyUsersContract>)System.Web.HttpContext.Current.Items[externalEmailkey];
                //    }
                //    else
                //    {
                //        //externalCopyUsers = getCommunicationCCusers(communicationSubEventID, tenantId);
                //        var externalCopyUser = GetExternalCopyUsers(communicationSubEventID, hierarchyNodeID, tenantId);
                //        if (externalCopyUser != null && externalCopyUser.Any())
                //        {
                //            externalCopyUsers = externalCopyUser.Where(x => x.lkpCopyType.CT_Code == bccCode)
                //                                .Select(y => new ExternalCopyUsersContract
                //                                {
                //                                    UserName = y.ECU_FirstName + " " + y.ECU_LastName,
                //                                    UserEmailAddress = y.ECU_EmailID,
                //                                    CopyTypeCode = bccCode
                //                                }).ToList();
                //        }
                //        if (externalCopyUsers != null && externalCopyUsers.Count > AppConsts.NONE)
                //        {
                //            System.Web.HttpContext.Current.Items.Add(externalEmailkey, externalCopyUsers);
                //        }
                //    }
                //}
                //else if (ServiceContext.Current != null)
                //{
                //    if (ServiceContext.Current.DataDict != null && ServiceContext.Current.DataDict.ContainsKey(externalEmailkey))
                //    {
                //        externalCopyUsers = (List<ExternalCopyUsersContract>)ServiceContext.Current.DataDict.GetValue(externalEmailkey);
                //    }
                //    else
                //    {
                //        //externalCopyUsers = getCommunicationCCusers(communicationSubEventID, tenantId);
                //        var externalCopyUser = GetExternalCopyUsers(communicationSubEventID, hierarchyNodeID, tenantId);
                //        if (externalCopyUser != null && externalCopyUser.Any())
                //        {
                //            externalCopyUsers = externalCopyUser.Where(x => x.lkpCopyType.CT_Code == bccCode)
                //                                .Select(y => new ExternalCopyUsersContract
                //                                {
                //                                    UserName = y.ECU_FirstName + " " + y.ECU_LastName,
                //                                    UserEmailAddress = y.ECU_EmailID,
                //                                    CopyTypeCode = bccCode
                //                                }).ToList();
                //        }
                //        if (externalCopyUsers != null && externalCopyUsers.Count > AppConsts.NONE)
                //        {
                //            if (ServiceContext.Current.DataDict == null)
                //                ServiceContext.Current.DataDict = new Dictionary<String, object>();
                //            ServiceContext.Current.DataDict.Add(externalEmailkey, externalCopyUsers);
                //        }
                //    }
                //}
                //else if (ParallelTaskContext.Current != null)
                //{
                //    if (ParallelTaskContext.Current.DataDict != null && ParallelTaskContext.Current.DataDict.ContainsKey(externalEmailkey))
                //    {
                //        externalCopyUsers = (List<ExternalCopyUsersContract>)ParallelTaskContext.Current.DataDict.GetValue(externalEmailkey);
                //    }
                //    else
                //    {
                //        //externalCopyUsers = getCommunicationCCusers(communicationSubEventID, tenantId);
                //        var externalCopyUser = GetExternalCopyUsers(communicationSubEventID, hierarchyNodeID, tenantId);
                //        if (externalCopyUser != null && externalCopyUser.Any())
                //        {
                //            externalCopyUsers = externalCopyUser.Where(x => x.lkpCopyType.CT_Code == bccCode)
                //                                .Select(y => new ExternalCopyUsersContract
                //                                {
                //                                    UserName = y.ECU_FirstName + " " + y.ECU_LastName,
                //                                    UserEmailAddress = y.ECU_EmailID,
                //                                    CopyTypeCode = bccCode
                //                                }).ToList();
                //        }
                //        if (externalCopyUsers != null && externalCopyUsers.Count > AppConsts.NONE)
                //        {
                //            if (ParallelTaskContext.Current.DataDict == null)
                //                ParallelTaskContext.Current.DataDict = new Dictionary<String, object>();
                //            ParallelTaskContext.Current.DataDict.Add(externalEmailkey, externalCopyUsers);
                //        }
                //    }
                //}

                #endregion

                if (externalCopyUsers != null && externalCopyUsers.Count > AppConsts.NONE)
                {
                    //externalCopyUsers = externalCopyUsers.Where(condition => condition.IsEmail).ToList();
                    foreach (ExternalCopyUsersContract externalCopyUser in externalCopyUsers)
                    {
                        SystemCommunicationDelivery ccSystemCommunicationDelivery = new SystemCommunicationDelivery
                        {
                            RecieverEmailID = externalCopyUser.UserEmailAddress,
                            RecieverName = externalCopyUser.UserName,
                            IsDispatched = false,
                            IsCC = externalCopyUser.CopyTypeCode == ccCode ? true : false,
                            IsBCC = externalCopyUser.CopyTypeCode == bccCode ? true : false,
                            CreatedByID = communicationTemplateContract.CurrentUserId,
                            CreatedOn = creationDate,
                            //SystemCommunication = systemCommunication,
                            SystemCommunicationTypeID = systemCommunicationID,
                            ReceiverOrganizationUserID = externalCopyUser.UserID, //communicationTemplateContract.ReceiverOrganizationUserId,
                            RetryCount = 0
                        };
                        ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(ccSystemCommunicationDelivery);
                    }
                }
            }

            ADBMessageQueueContext.SaveChanges();

            return systemCommunicationID;
        }

        #region UAT-1578:Addition of SMS notification.
        /// <summary>
        /// Return Template for SMS
        /// </summary>
        /// <param name="communicationSubEventId">communicationSubEventId</param>
        /// <returns>SMSTemplate</returns>
        public SMSTemplate GetSMSTemplateForCommunicationSubEvent(Int32 communicationSubEventId, Int32 LangugageId, Int32 DefaultLanguageID)
        {

            //// get user language specified template
            return ADB_MessageQueueContext.SMSTemplates.Where(cond => cond.SMST_CommuncicationSubEventID == communicationSubEventId && !cond.SMST_IsDeleted && (cond.SMST_LanguageId == LangugageId || cond.SMST_LanguageId == DefaultLanguageID)).OrderByDescending(x => x.SMST_LanguageId == LangugageId).FirstOrDefault();
        }

        /// <summary>
        /// Method to prepare message content for SMS
        /// </summary>
        /// <param name="communicationSubEventCode">communicationSubEventCode</param>
        /// <param name="dicContent">dicContent</param>
        /// <param name="smsTemplate">smsTemplate</param>
        /// <returns>SystemCommunication</returns>
        public SystemCommunication PrepareMessageContentForSMS(Dictionary<String, Object> dicContent, SMSTemplate smsTemplate, List<CommunicationTemplatePlaceHolder> placeHoldersToFetch)
        {
            try
            {
                SystemCommunication systemCommunication = null;

                if (smsTemplate.IsNotNull())
                {
                    String templateContent = smsTemplate.SMST_Content;
                    String templateSubject = smsTemplate.SMST_Subject;
                    //Null check
                    if (placeHoldersToFetch.IsNotNull())
                    {
                        foreach (var placeHolder in placeHoldersToFetch)
                        {
                            Object obj = dicContent.GetValue(placeHolder.Property);
                            templateContent = templateContent.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                            templateSubject = templateSubject.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                        }
                    }
                    systemCommunication = new SystemCommunication();
                    systemCommunication.Content = templateContent;
                    systemCommunication.Subject = templateSubject;
                    systemCommunication.CommunicationSubEventID = smsTemplate.SMST_CommuncicationSubEventID;
                }
                return systemCommunication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Save SMS content in Database 
        /// </summary>
        /// <param name="systemCommunication">systemCommunication</param>
        /// <param name="communicationTemplateContract">communicationTemplateContract</param>
        /// <returns></returns>
        public Int32 SaveSMSContent(SystemCommunication systemCommunication, CommunicationTemplateContract communicationTemplateContract, Int32 notificationTypeId)
        {
            try
            {
                DateTime creationDate = DateTime.Now;

                systemCommunication.SenderEmailID = communicationTemplateContract.SenderEmailID;
                systemCommunication.SenderName = communicationTemplateContract.SenderName;
                systemCommunication.CreatedByID = communicationTemplateContract.CurrentUserId;
                systemCommunication.CreatedOn = creationDate;

                #region Override Entity Command Timeout Setting
                Int32 TimeoutSetting = AppConsts.NONE;
                if (!System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"].IsNullOrEmpty())
                {
                    TimeoutSetting = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"]);
                    if (TimeoutSetting > AppConsts.NONE)
                    {
                        ADBMessageQueueContext.CommandTimeout = TimeoutSetting;
                    }
                }
                #endregion

                //ADBMessageQueueContext.SystemCommunications.AddObject(systemCommunication);
                Int32 systemCommunicationID = InsertSystemCommunication(systemCommunication);

                SystemCommunicationDelivery systemCommunicationDelivery = new SystemCommunicationDelivery
                {
                    RecieverEmailID = communicationTemplateContract.RecieverEmailID,
                    RecieverName = communicationTemplateContract.RecieverName,
                    IsDispatched = false,
                    IsCC = false,
                    CreatedByID = communicationTemplateContract.CurrentUserId,
                    CreatedOn = creationDate,
                    //SystemCommunication = systemCommunication,
                    SystemCommunicationTypeID = systemCommunicationID,
                    ReceiverOrganizationUserID = communicationTemplateContract.ReceiverOrganizationUserId,
                    RetryCount = 0,
                    NotificationDeliveryTypeID = notificationTypeId
                };
                ADBMessageQueueContext.SystemCommunicationDeliveries.AddObject(systemCommunicationDelivery);
                ADBMessageQueueContext.SaveChanges();
                return systemCommunicationID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        /// <summary>
        /// UAT-1793
        /// </summary>
        /// <returns></returns>
        public List<Int32> GetSubEventsHavingTemplates(Int32 languageId, Int32 defaultLanguageId)
        {


            //return ADBMessageQueueContext.lkpCommunicationSubEvents
            //    .Where(cond=> cond.IsDeleted != true && cond.CommunicationTemplates.Any(ct=>ct.IsDeleted != true)
            //    && !cond.SystemEventSettings.Any(se => se.IsDeleted != true)).Select(col => col.CommunicationSubEventID).Distinct().ToList();

            return ADBMessageQueueContext.CommunicationTemplates.Where(x => x.IsDeleted == false
                && !x.SystemEventSettings.Any()
                && languageId == x.LanguageId)
                .Select(column => column.CommunicationSubEventID.HasValue ? column.CommunicationSubEventID.Value : 0).Distinct().ToList();

        }

        // UAT-1793 : Should not be able to create duplicate templates in the common template section of the System Template screen.
        public List<Int32> GetSubEventsHavingTemplatesByTenant(Int32 tenantId, Int32 languageId, Int32 defaultLanguageId)
        {
            return ADBMessageQueueContext.lkpCommunicationSubEvents
                .Where(cond => cond.IsDeleted != true && cond.CommunicationTemplates.Any(ct => ct.IsDeleted != true
                && languageId == ct.LanguageId)
                && cond.SystemEventSettings.Any(se => se.IsDeleted == false && se.TenantID == tenantId && se.ComplianceCategoryID == null && se.ComplianceItemID == null))
                .Select(col => col.CommunicationSubEventID).Distinct().ToList();
        }

        #region UAT-3261: Badge Form Enhancements
        List<EmailDetails> ICommunicationRepository.GetUserEmails(Int32 userId, Int32 pageSize, Int32 pageIndex, String defaultExpression, Boolean isSortDirectionDescending, Int32 ApplicantDashboard = 0)
        {
            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[usp_Report_AllRecentEmailsOfUser]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserID", userId);
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@defaultExpression", defaultExpression);
                command.Parameters.AddWithValue("@isSortDirectionDescending", isSortDirectionDescending);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetEmailData(ds.Tables[0]);
                }
            }
            return new List<EmailDetails>();
        }

        private List<EmailDetails> SetEmailData(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new EmailDetails
            {
                SystemCommunicationId = Convert.ToInt32(x["SystemCommunicationId"]),
                EmailType = Convert.ToString(x["EmailType"]),
                Subject = Convert.ToString(x["Subject"]),
                DispatchedDate = x["DispatchedDate"] == DBNull.Value ? String.Empty : Convert.ToString(x["DispatchedDate"]),
                TotalRecords = Convert.ToInt32(x["TotalRecords"])
            }).ToList();
        }

        public List<SystemCommunicationDelivery> GetSysCommDeliveryIds(Int32 SystemCommunicationId)
        {
            return ADBMessageQueueContext.SystemCommunicationDeliveries.Where(x => x.SystemCommunicationTypeID == SystemCommunicationId && x.IsDispatched).Distinct().ToList();
        }
        #endregion


        #region System Communication Insertion
        private Int32 InsertSystemCommunication(SystemCommunication systemCommunication)
        {
            Int32 SystemCommunicationId = AppConsts.NONE;
            String systemCommunicationData = ConvertToXMLSystemCommunication(systemCommunication);
            SystemCommunication systemcommunication = new SystemCommunication();
            EntityConnection connection = ADBMessageQueueContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[usp_InsertSystemCommunicationDetails]", con);
                command.CommandType = CommandType.StoredProcedure;
                #region Override Entity Command Timeout Setting
                Int32 TimeoutSetting = AppConsts.NONE;
                if (!System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"].IsNullOrEmpty())
                {
                    TimeoutSetting = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EntityCommandTimeoutSetting"]);
                    if (TimeoutSetting > AppConsts.NONE)
                    {
                        command.CommandTimeout = TimeoutSetting;
                    }
                }
                #endregion
                command.Parameters.AddWithValue("@systemCommunicationData", systemCommunicationData);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    SystemCommunicationId = ds.Tables[0].Rows[0]["SystemCommunicationID"].IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ds.Tables[0].Rows[0]["SystemCommunicationID"]);
                }
            }
            return SystemCommunicationId;
        }

        private String ConvertToXMLSystemCommunication(SystemCommunication systemCommunication)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<SystemCommunication>");
            sb.Append("<CommunicationSubEventID>" + systemCommunication.CommunicationSubEventID + "</CommunicationSubEventID>");
            sb.Append("<Subject><![CDATA[" + systemCommunication.Subject + "]]></Subject>");
            sb.Append("<Content><![CDATA[" + systemCommunication.Content + "]]></Content>");
            sb.Append("<SenderEmailID><![CDATA[" + systemCommunication.SenderEmailID + "]]></SenderEmailID>");
            sb.Append("<SenderName><![CDATA[" + systemCommunication.SenderName + "]]></SenderName>");
            sb.Append("<CreatedOn><![CDATA[" + systemCommunication.CreatedOn + "]]></CreatedOn>");
            sb.Append("<CreatedByID>" + systemCommunication.CreatedByID + "</CreatedByID>");
            sb.Append("</SystemCommunication>");

            return sb.ToString();

        }
        #endregion

        #region UAT-3795


        /// <summary>
        /// UAT 1230:As an admin, I should be able to invite a person (or group of people) to create an applicant account
        /// UAT 1302:As an admin (client or ADB), I should be able to create preceptors/instructors
        /// </summary>
        /// <param name="lstSystemCommunications"></param>
        /// <returns></returns>
        Int32 ICommunicationRepository.SaveSysCommunicationAndSysDeliveries(SystemCommunication systemCommunications)
        {
            if (!systemCommunications.IsNullOrEmpty())
                ADB_MessageQueueContext.SystemCommunications.AddObject(systemCommunications);

            if (ADB_MessageQueueContext.SaveChanges() > AppConsts.NONE)
            {
                return systemCommunications.SystemCommunicationID;
            }
            return AppConsts.NONE;
        }


        #endregion

        #region UAT-3704
        public List<Int32> GetSubEventsHavingAgencySpecificTemplates(Int32 languageId)
        {
            return ADBMessageQueueContext.lkpCommunicationSubEvents
                .Where(cond => cond.IsDeleted != true && cond.CommunicationTemplates.Any(ct => ct.IsDeleted != true
                && languageId == ct.LanguageId)
                && cond.SystemEventSettings.Any(se => se.IsDeleted == false && se.TenantID == null && se.ComplianceCategoryID == null && se.ComplianceItemID == null && se.AgencyHierarchyID != null))
                .Select(col => col.CommunicationSubEventID).Distinct().ToList();
        }
        #endregion


    }
}
