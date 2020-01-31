#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  BkgOrderNotificationService.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
#endregion

#region Application Specific
using Business.RepoManagers;
using Entity;
using ExternalVendors;
using INTSOF.ServiceUtil;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using Business.ReportExecutionService;
using INTSOF.UI.Contract.Services;
using System.Text;
using System.IO;
using INTSOF.UI.Contract.Templates;
#endregion

#endregion

namespace VendorOrderProcessService
{
    public static class BkgOrderStatusService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static Int32 _recordChunkSize;
        private static String BkgOrderStatusServiceLogger;
        private static Boolean _isServiceLoggingEnabled;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Constructor

        static BkgOrderStatusService()
        {
            BkgOrderStatusServiceLogger = "BkgOrderStatusServiceLogger";
            _recordChunkSize = AppConsts.CHUNK_SIZE_FOR_BKG_ORDER_NOTIFICATION_SERVICE;

            if (ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull())
            {
                _isServiceLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]);
            }
            else
            {
                _isServiceLoggingEnabled = false;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void CreateBkgOrderNotification(Int32? tenant_Id = null)
        {
            try
            {
                // ServiceContext.init();
                ServiceLogger.Info("Calling CreateBkgOrderNotification: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, BkgOrderStatusServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                        List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
                        List<Entity.ClientEntity.lkpServiceFormStatu> serviceFormStatus = BackgroundProcessOrderManager.GetServiceFormStatus(tenantId);
                        List<Entity.ClientEntity.lkpOrderNotifyStatu> orderNotifyStatus = BackgroundProcessOrderManager.GetOrderNotifyStatus(tenantId);
                        List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                        Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                            Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

                        String needToSendOrdSvcFormStsCode = ServiceFormStatus.NEED_TO_SEND.GetStringValue();
                        Int32 needToSendOrdSvcFormStsID = serviceFormStatus.IsNotNull() && serviceFormStatus.Count > 0 ?
                            Convert.ToInt32(serviceFormStatus.FirstOrDefault(cond => cond.SFS_Code == needToSendOrdSvcFormStsCode).SFS_ID) : Convert.ToInt32(0);

                        String sentToStudentOrdSvcFormStsCode = ServiceFormStatus.SENT_TO_STUDENT.GetStringValue();
                        Int32 sentToStudentOrdSvcFormStsID = serviceFormStatus.IsNotNull() && serviceFormStatus.Count > 0 ?
                            Convert.ToInt32(serviceFormStatus.FirstOrDefault(cond => cond.SFS_Code == sentToStudentOrdSvcFormStsCode).SFS_ID) : Convert.ToInt32(0);

                        String notifiedOrdNotifyStsCode = OrderNotifyStatus.NOTIFIED.GetStringValue();
                        Int32 notifiedOrdNotifyStsID = orderNotifyStatus.IsNotNull() && orderNotifyStatus.Count > 0 ?
                            Convert.ToInt32(orderNotifyStatus.FirstOrDefault(cond => cond.ONS_Code == notifiedOrdNotifyStsCode).ONS_ID) : Convert.ToInt32(0);

                        String errorOrdNotifyStsCode = OrderNotifyStatus.ERROR.GetStringValue();
                        Int32 errorOrdNotifyStsID = orderNotifyStatus.IsNotNull() && orderNotifyStatus.Count > 0 ?
                            Convert.ToInt32(orderNotifyStatus.FirstOrDefault(cond => cond.ONS_Code == errorOrdNotifyStsCode).ONS_ID) : Convert.ToInt32(0);

                        String docAttachmentTypeCode = DocumentAttachmentType.SYSTEM_DOCUMENT.GetStringValue();
                        Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                            Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                        String svcFormDocumentTypeCode = OrderNotificationType.SERVICE_FORM_DOCUMENT.GetStringValue();
                        Int32 svcFormDocumentTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                            Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == svcFormDocumentTypeCode).ONT_ID) : Convert.ToInt32(0);

                        String svcFormNotificationTypeCode = OrderNotificationType.SERVICE_FORM_NOTIFICATION.GetStringValue();
                        Int32 svcFormNotificationTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                            Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == svcFormNotificationTypeCode).ONT_ID) : Convert.ToInt32(0);

                        String entityTypeCode = CommunicationEntityType.BACKGROUND_SERVICE_FORM.GetStringValue();

                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Background Order Notification Data: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                            List<BkgOrderNotificationDataContract> bkgOrdreNotificationDataList = BackgroundProcessOrderManager.GetBkgOrderNotificationData(tenantId, _recordChunkSize);

                            ServiceLogger.Info("Total Background Order Notification Records found: " + bkgOrdreNotificationDataList.Count, BkgOrderStatusServiceLogger);
                            ServiceLogger.Debug<List<BkgOrderNotificationDataContract>>("List of Background Order Notification from db:", bkgOrdreNotificationDataList, BkgOrderStatusServiceLogger);

                            if (bkgOrdreNotificationDataList.IsNotNull() && bkgOrdreNotificationDataList.Count > AppConsts.NONE)
                            {
                                Int32 ordNotificationID = 0, bkgOrderServiceFormID = 0;
                                Boolean isSucess = true;
                                Int32? systemCommunicationID = null;
                                Guid? messageID = null;

                                ServiceLogger.Info("Started foreach loop on bkgOrdreNotificationDataList based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                bkgOrdreNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                                {
                                    isSucess = true;
                                    ServiceLogger.Debug<Int32>("Next Background Order ID from the loop:", bkgOrderID, BkgOrderStatusServiceLogger);

                                    bkgOrdreNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                                    {
                                        if (condition.SendAutomatically)
                                        {
                                            //Create Dictionary for Messaging Contract
                                            Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                                            dicMessageParam.Add("EntityID", condition.ServiceAtachedFormID);
                                            dicMessageParam.Add("EntityTypeCode", entityTypeCode);

                                            //Create Dictionary for Mail And Message Data
                                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(condition.FirstName, " ", condition.LastName));
                                            dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, condition.NodeHierarchy);
                                            dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);
                                            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                            dictMailData.Add(EmailFieldConstants.ORDER_NO, condition.OrderNumber);
                                            dictMailData.Add(EmailFieldConstants.ORDER_DATE, condition.OrderDate.ToShortDateString());
                                            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, condition.PackageName);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_NAME, condition.ServiceName);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_FORM_NAME, condition.ServiceAtachedFormName);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_FORM_DISPATCH_DATE, DateTime.Now.ToShortDateString());
                                            dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, condition.ServiceGroupName);
                                            dictMailData.Add(EmailFieldConstants.ORDER_STATUS, condition.OrderStatus);

                                            CommunicationMockUpData mockData = new CommunicationMockUpData();
                                            mockData.UserName = string.Concat(condition.FirstName, " ", condition.LastName);
                                            mockData.EmailID = condition.PrimaryEmailAddress;
                                            mockData.ReceiverOrganizationUserID = condition.OrganizationUserID;

                                            //Send mail
                                            systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, mockData, tenantId, condition.HierarchyNodeID, condition.ServiceAtachedFormID, entityTypeCode, true);

                                            systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;
                                            //Save Mail Attachment
                                            if (condition.SystemDocumentID.HasValue)
                                            {
                                                Int32? sysCommAttachmentID = null;
                                                if (systemCommunicationID != null)
                                                {
                                                    SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                                                    sysCommAttachment.SCA_OriginalDocumentID = condition.SystemDocumentID.Value;
                                                    sysCommAttachment.SCA_OriginalDocumentName = condition.DocumentName;
                                                    sysCommAttachment.SCA_DocumentPath = condition.DocumentPath;
                                                    sysCommAttachment.SCA_DocumentSize = condition.DocumentSize.HasValue ? condition.DocumentSize.Value : 0;
                                                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                                    sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                                    sysCommAttachment.SCA_IsDeleted = false;
                                                    sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                                    sysCommAttachment.SCA_ModifiedBy = null;
                                                    sysCommAttachment.SCA_ModifiedOn = null;

                                                    sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                                }

                                                Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                                                List<ADBMessageDocument> messageDocument = new List<ADBMessageDocument>();

                                                ADBMessageDocument documentData = new ADBMessageDocument();
                                                documentData.DocumentName = condition.DocumentPath;
                                                documentData.OriginalDocumentName = condition.DocumentName;
                                                documentData.DocumentSize = condition.DocumentSize.Value;
                                                documentData.SystemCommunicationAttachmentID = sysCommAttachmentID;
                                                messageDocument.Add(documentData);

                                                attachedFiles = MessageManager.SaveDocumentAndGetDocumentId(messageDocument);
                                                if (!attachedFiles.IsNullOrEmpty())
                                                {
                                                    String documentName = String.Empty;
                                                    attachedFiles.ForEach(a => documentName += a.Key.ToString() + ";");

                                                    dicMessageParam.Add("DocumentName", documentName);
                                                }
                                            }

                                            //Send Message
                                            messageID = CommunicationManager.SaveMessageContent(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, condition.OrganizationUserID, tenantId, dicMessageParam);
                                        }
                                        else
                                        {
                                            systemCommunicationID = null;
                                            messageID = null;
                                        }

                                        OrderNotification ordNotification = new OrderNotification();
                                        ordNotification.ONTF_OrderID = condition.OrderID;
                                        ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                        ordNotification.ONTF_MSG_MessageID = messageID;
                                        ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                        ordNotification.ONTF_IsPostal = condition.SendAutomatically == false ? true : false;
                                        ordNotification.ONTF_CreatedByID = backgroundProcessUserId;
                                        ordNotification.ONTF_CreatedOn = DateTime.Now;
                                        ordNotification.ONTF_ModifiedByID = null;
                                        ordNotification.ONTF_ModifiedDate = null;
                                        ordNotification.ONTF_ParentNotificationID = null;
                                        ordNotification.ONTF_OrderNotificationTypeID = condition.SendAutomatically == false ? svcFormNotificationTypeID : svcFormDocumentTypeID;
                                        ordNotification.ONTF_NotificationDetail = "Service Form for " + condition.ServiceName;

                                        ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                                        if (ordNotificationID == 0)
                                        {
                                            isSucess = false;
                                        }

                                        //code added for UAT  - 787 Service Form history dropdown enhancements

                                        else
                                        {
                                            BkgOrderServiceForm bkgOrderServiceForm = new BkgOrderServiceForm();
                                            bkgOrderServiceForm.OSF_ServiceFormMappingID = condition.ServiceFormMappingID;
                                            bkgOrderServiceForm.OSF_BkgOrderPackageSvcID = condition.BkgOrderPackageSvcID;
                                            bkgOrderServiceForm.OSF_ServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID;
                                            bkgOrderServiceForm.OSF_OrderNotificationID = ordNotificationID;
                                            bkgOrderServiceForm.OSF_IsDeleted = false;
                                            bkgOrderServiceForm.OSF_CreatedBy = backgroundProcessUserId;
                                            bkgOrderServiceForm.OSF_CreatedOn = DateTime.Now;
                                            bkgOrderServiceForm.OSF_ModifiedBy = null;
                                            bkgOrderServiceForm.OSF_ModifiedOn = null;

                                            bkgOrderServiceFormID = BackgroundProcessOrderManager.CreateBkgOrderServiceForm(tenantId, bkgOrderServiceForm);

                                            if (bkgOrderServiceFormID == 0)
                                            {
                                                isSucess = false;
                                            }
                                            else
                                            {
                                                OrderNotification newOrdNotification = new OrderNotification();
                                                newOrdNotification.ONTF_OrderID = condition.OrderID;
                                                newOrdNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                                newOrdNotification.ONTF_MSG_MessageID = messageID;
                                                newOrdNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                                newOrdNotification.ONTF_IsPostal = condition.SendAutomatically == false ? true : false;
                                                newOrdNotification.ONTF_CreatedByID = backgroundProcessUserId;
                                                newOrdNotification.ONTF_CreatedOn = DateTime.Now;
                                                newOrdNotification.ONTF_ModifiedByID = null;
                                                newOrdNotification.ONTF_ModifiedDate = null;
                                                newOrdNotification.ONTF_ParentNotificationID = null;
                                                newOrdNotification.ONTF_OrderNotificationTypeID = condition.SendAutomatically == false ? svcFormNotificationTypeID : svcFormDocumentTypeID;
                                                newOrdNotification.ONTF_NotificationDetail = "Service Form for " + condition.ServiceName;
                                                newOrdNotification.ONTF_ParentNotificationID = ordNotificationID;
                                                Int32 newOrdNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, newOrdNotification);
                                                if (newOrdNotificationID != 0)
                                                {
                                                    BkgOrderServiceForm newBkgOrderServiceForm = new BkgOrderServiceForm();
                                                    newBkgOrderServiceForm.OSF_ServiceFormMappingID = condition.ServiceFormMappingID;
                                                    newBkgOrderServiceForm.OSF_BkgOrderPackageSvcID = condition.BkgOrderPackageSvcID;
                                                    newBkgOrderServiceForm.OSF_ServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID;
                                                    newBkgOrderServiceForm.OSF_NewServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID; ;
                                                    newBkgOrderServiceForm.OSF_OldServiceFormStatusID = null;
                                                    newBkgOrderServiceForm.OSF_OrderNotificationID = newOrdNotificationID;
                                                    newBkgOrderServiceForm.OSF_IsDeleted = false;
                                                    newBkgOrderServiceForm.OSF_CreatedBy = backgroundProcessUserId;
                                                    newBkgOrderServiceForm.OSF_CreatedOn = DateTime.Now;
                                                    newBkgOrderServiceForm.OSF_ModifiedBy = null;
                                                    newBkgOrderServiceForm.OSF_ModifiedOn = null;

                                                    Int32 newBkgOrderServiceFormID = BackgroundProcessOrderManager.CreateBkgOrderServiceForm(tenantId, newBkgOrderServiceForm);
                                                }
                                            }
                                        }
                                    });

                                    BkgOrder bkgOrd = new BkgOrder();
                                    bkgOrd.BOR_ID = bkgOrderID;
                                    bkgOrd.BOR_OrderNotifyStatusID = isSucess == true ? notifiedOrdNotifyStsID : errorOrdNotifyStsID;
                                    bkgOrd.BOR_ModifiedByID = backgroundProcessUserId;
                                    bkgOrd.BOR_ModifiedOn = DateTime.Now;

                                    BackgroundProcessOrderManager.UpdateBkgOrderNotifyStatus(tenantId, bkgOrd);
                                });

                                ServiceLogger.Info("Ended foreach loop on bkgOrdreNotificationDataList based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                                //ServiceLogger.Info("Total Background Order Notification Records found: " + bkgOrdreNotificationDataList.Count(), BkgOrderStatusServiceLogger);
                            }
                            ServiceLogger.Info("Processed the chunk of Background Order Notification records: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateBkgOrderNotification.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateBkgOrderNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceLogger);
            }
        }

        /// <summary>
        /// Method used for 
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void UpdateBkgOrderColorStatus(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling UpdateBkgOrderColorStatus: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, BkgOrderStatusServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        Boolean isUpdated = BackgroundProcessOrderManager.UpdateBkgOrderColorStatus(tenantId, backgroundProcessUserId);

                        if (isUpdated)
                            ServiceLogger.Info("Successfully updated tenant having ID: " + tenant_Id + ", " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.UpdateBkgOrderColorStatus.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                        ServiceContext.ReleaseDBContextItems();
                    }

                    ServiceContext.ReleaseDBContextItems();
                }
                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in UpdateBkgOrderColorStatus method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceLogger);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void CreateBkgOrderResultCompletedNotification(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling CreateBkgOrderResultCompletedNotification: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, BkgOrderStatusServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
                        List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                        Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                            Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

                        String docAttachmentTypeCode = DocumentAttachmentType.ORDER_COMPLETION_DOCUMENT.GetStringValue();
                        Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                            Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                        String ordComDocumentTypeCode = OrderNotificationType.ORDER_RESULT.GetStringValue();
                        Int32 ordComDocumentTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                            Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == ordComDocumentTypeCode).ONT_ID) : Convert.ToInt32(0);

                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Background Order Completed Result Notification Data: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                            List<BkgOrderNotificationDataContract> bkgOrdreNotificationDataList = BackgroundProcessOrderManager.GetBkgOrderResultCompletedNotificationData(tenantId, _recordChunkSize);

                            ServiceLogger.Info("Total Background Order Completed Result Records found: " + bkgOrdreNotificationDataList.Count, BkgOrderStatusServiceLogger);
                            ServiceLogger.Debug<List<BkgOrderNotificationDataContract>>("List of Background Order Notification from db:", bkgOrdreNotificationDataList, BkgOrderStatusServiceLogger);

                            if (bkgOrdreNotificationDataList.IsNotNull() && bkgOrdreNotificationDataList.Count > AppConsts.NONE)
                            {
                                Int32 ordNotificationID = 0;
                                Int32? systemCommunicationID = null;

                                ServiceLogger.Info("Started foreach loop on bkgOrdreNotificationDataList based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                bkgOrdreNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                                {
                                    ServiceLogger.Debug<Int32>("Next Background Order ID from the loop:", bkgOrderID, BkgOrderStatusServiceLogger);

                                    bkgOrdreNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                                    {
                                        //Create Dictionary for Mail And Message Data
                                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(condition.FirstName, " ", condition.LastName));
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, string.Concat(applicationUrl));
                                        dictMailData.Add(EmailFieldConstants.ORDER_NO, condition.OrderNumber);

                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        mockData.UserName = string.Concat(condition.FirstName, " ", condition.LastName);
                                        mockData.EmailID = condition.PrimaryEmailAddress;
                                        mockData.ReceiverOrganizationUserID = condition.OrganizationUserID;

                                        var PkgPDFAttachementStatus = BackgroundProcessOrderManager.GetBGPkgPDFAttachementStatus(tenantId, condition.HierarchyNodeID);

                                        //UAT-2855
                                        if (condition.IsAdminOrder)
                                        {
                                            condition.IsEmployment = false;
                                            PkgPDFAttachementStatus = PDFInclusionOptions.Not_Specified.GetStringValue();
                                        }

                                        //UAT-1735:- don't send report result attachment for employment nodes, only the link. 
                                        SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                                        if (!condition.IsEmployment && !PkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                                        {
                                            //Create Attachment
                                            ParameterValue[] parameters = new ParameterValue[3];
                                            parameters[0] = new ParameterValue();
                                            parameters[0].Name = "OrderID";
                                            parameters[0].Value = condition.OrderID.ToString(); //condition.BkgOrderID.ToString();
                                            parameters[1] = new ParameterValue();
                                            parameters[1].Name = "TenantID";
                                            parameters[1].Value = tenantId.ToString();
                                            parameters[2] = new ParameterValue();
                                            parameters[2].Name = "UserID";
                                            parameters[2].Value = AppConsts.STR_ONE;

                                            String reportName = "OrderCompletion";
                                            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                                            String fileName = "OCR_" + tenantId.ToString() + "_" + condition.OrderNumber.ToString() + "_" + date + ".pdf";

                                            byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);

                                            String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);


                                            sysCommAttachment.SCA_OriginalDocumentID = -1;
                                            sysCommAttachment.SCA_OriginalDocumentName = "OrderCompletedReport.pdf";
                                            sysCommAttachment.SCA_DocumentPath = retFilepath;
                                            sysCommAttachment.SCA_DocumentSize = reportContent.Length;
                                            sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                            sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                            sysCommAttachment.SCA_IsDeleted = false;
                                            sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                            sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                            sysCommAttachment.SCA_ModifiedBy = null;
                                            sysCommAttachment.SCA_ModifiedOn = null;
                                        }

                                        #region Code to generate report url place holder value

                                        Dictionary<String, String> reportUrlParameters = new Dictionary<String, String>
                                                         {
                                                            { "OrderID",  condition.OrderID.ToString()},
                                                             { "ReportType",  BackgroundReportType.ORDER_COMPLETION.GetStringValue()},
                                                             { "TenantId",  tenantId.ToString()},
                                                             {"HierarchyNodeID",Convert.ToString(condition.HierarchyNodeID)},
                                                             {"OrganizationUserID",Convert.ToString(condition.OrganizationUserID)},
                                                             {"IsReportSentToStudent",Convert.ToString(true)}
                                                         };
                                        StringBuilder reportUrl = new StringBuilder();
                                        reportUrl.Append(applicationUrl.Trim() + "?args=");
                                        reportUrl.Append(reportUrlParameters.ToEncryptedQueryString());

                                        #endregion

                                        dictMailData.Add(EmailFieldConstants.REPORT_URL, reportUrl.ToString());

                                        //Send mail                         
                                        //UAT-3453

                                        String entityTypeCode = CommunicationEntityType.SCREENING_RESULT.GetStringValue();
                                        Int32 entityID = AppConsts.NONE;
                                        CommunicationSubEvents commSubEvent = new CommunicationSubEvents();

                                        List<lkpScreeningResultType> lstLkpScreeningResultType = CommunicationManager.GetScreeningResultType();
                                        String screeningResultTypeCode = String.Empty;

                                        if (!lstLkpScreeningResultType.IsNullOrEmpty())
                                        {
                                            if (condition.IsOrderFlag)
                                            {
                                                screeningResultTypeCode = ScreeningResultType.Flagged.GetStringValue();
                                                entityID = lstLkpScreeningResultType.Where(con => con.SRT_Code == screeningResultTypeCode && !con.SRT_IsDeleted).Select(sel => sel.SRT_ID).FirstOrDefault();
                                            }
                                            else
                                            {
                                                screeningResultTypeCode = ScreeningResultType.Clear.GetStringValue();
                                                entityID = lstLkpScreeningResultType.Where(con => con.SRT_Code == screeningResultTypeCode && !con.SRT_IsDeleted).Select(sel => sel.SRT_ID).FirstOrDefault();
                                            }

                                            if (PkgPDFAttachementStatus == PDFInclusionOptions.Excluded.GetStringValue())
                                            {
                                                commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS_WITHOUT_PDF_ATTACHMENT;
                                            }
                                            else
                                            {
                                                commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS;
                                            }

                                            systemCommunicationID = CommunicationManager.SendPackageNotificationMail(commSubEvent, dictMailData, mockData, tenantId, condition.HierarchyNodeID, entityID, entityTypeCode, true);
                                        }

                                        #region Commented code

                                        //if (condition.IsOrderFlag)
                                        //{
                                        //    if (PkgPDFAttachementStatus == PDFInclusionOptions.Excluded.GetStringValue())
                                        //    {
                                        //        systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_FLAGGED_COMPLETED_ORDER_RESULTS_WITHOUT_PDF_ATTACHMENT, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);
                                        //    }
                                        //    else
                                        //    {
                                        //        systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FLAGGED_COMPLETED_ORDER_RESULTS, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (PkgPDFAttachementStatus == PDFInclusionOptions.Excluded.GetStringValue())
                                        //    {
                                        //        systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS_WITHOUT_PDF_ATTACHMENT, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);
                                        //    }
                                        //    else
                                        //    {
                                        //        systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);
                                        //    }
                                        //}

                                        #endregion

                                        if (systemCommunicationID != null)
                                        {
                                            if (!condition.IsEmployment && !PkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                                            {
                                                //Save Mail Attachment
                                                sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                                Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                            }

                                            OrderNotification ordNotification = new OrderNotification();
                                            ordNotification.ONTF_OrderID = condition.OrderID;
                                            ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                            ordNotification.ONTF_MSG_MessageID = null;
                                            ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                            ordNotification.ONTF_IsPostal = false;
                                            ordNotification.ONTF_CreatedByID = backgroundProcessUserId;
                                            ordNotification.ONTF_CreatedOn = DateTime.Now;
                                            ordNotification.ONTF_ModifiedByID = null;
                                            ordNotification.ONTF_ModifiedDate = null;
                                            ordNotification.ONTF_ParentNotificationID = null;
                                            ordNotification.ONTF_OrderNotificationTypeID = ordComDocumentTypeID;
                                            ordNotification.ONTF_NotificationDetail = "Complete Order Result for entire Service Group(s)";

                                            ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                                        }
                                    });
                                });

                                ServiceLogger.Info("Ended foreach loop on bkgOrdreNotificationDataList based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                                //ServiceLogger.Info("Total Background Order Notification Records found: " + bkgOrdreNotificationDataList.Count(), BkgOrderStatusServiceLogger);
                            }
                            ServiceLogger.Info("Processed the chunk of Background Order Notification records: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateBkgOrderResultCompletedNotification.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateBkgOrderResultCompletedNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceLogger);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void CreateServiceGroupCompletedNotification(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling CreateBkgOrderResultCompletedNotification: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, BkgOrderStatusServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
                        List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                        Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                            Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

                        String docAttachmentTypeCode = DocumentAttachmentType.ORDER_COMPLETION_DOCUMENT.GetStringValue();
                        Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                            Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                        String ordComDocumentTypeCode = OrderNotificationType.ORDER_RESULT.GetStringValue();
                        Int32 ordComDocumentTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                            Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == ordComDocumentTypeCode).ONT_ID) : Convert.ToInt32(0);

                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Background Service Group Completed Notification Data: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                            List<BkgOrderNotificationDataContract> bkgOrdreNotificationDataList = BackgroundProcessOrderManager.GetServiceGroupCompletedNotificationData(tenantId, _recordChunkSize);

                            ServiceLogger.Info("Total Background Service Group Completed Records found: " + bkgOrdreNotificationDataList.Count, BkgOrderStatusServiceLogger);
                            ServiceLogger.Debug<List<BkgOrderNotificationDataContract>>("List of Service Group Notification from db:", bkgOrdreNotificationDataList, BkgOrderStatusServiceLogger);

                            if (bkgOrdreNotificationDataList.IsNotNull() && bkgOrdreNotificationDataList.Count > AppConsts.NONE)
                            {
                                Int32 ordNotificationID = 0;
                                Int32? systemCommunicationID = null;

                                ServiceLogger.Info("Started foreach loop on CreateBkgOrderResultCompletedNotification based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                bkgOrdreNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                                {
                                    ServiceLogger.Debug<Int32>("Next Background Order ID from the loop:", bkgOrderID, BkgOrderStatusServiceLogger);

                                    bkgOrdreNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                                    {
                                        //Create Dictionary for Mail And Message Data
                                        Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, "Admin");
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, String.Concat(applicationUrl));
                                        dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, String.Concat(condition.ServiceGroupName));
                                        dictMailData.Add(EmailFieldConstants.ORDER_NO, condition.OrderNumber);
                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        mockData.UserName = "Admin";
                                        mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                        mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                                        //UAT-2438
                                        var PkgPDFAttachementStatus = BackgroundProcessOrderManager.GetBGPkgPDFAttachementStatus(tenantId, condition.HierarchyNodeID);


                                        //UAT-1735:- don't send report result attachment for employment nodes, only the link. 
                                        SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                                        if (!condition.IsEmployment && !PkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                                        {
                                            //Create Attachment
                                            ParameterValue[] parameters = new ParameterValue[4];
                                            parameters[0] = new ParameterValue();
                                            parameters[0].Name = "OrderID";
                                            parameters[0].Value = condition.OrderID.ToString(); //condition.BkgOrderID.ToString();
                                            parameters[1] = new ParameterValue();
                                            parameters[1].Name = "TenantID";
                                            parameters[1].Value = tenantId.ToString();
                                            parameters[2] = new ParameterValue();
                                            parameters[2].Name = "PackageGroupID";
                                            parameters[2].Value = condition.ServiceGroupID.ToString();
                                            parameters[3] = new ParameterValue();
                                            parameters[3].Name = "UserID";
                                            parameters[3].Value = AppConsts.STR_ONE;
                                            String reportName = "OrderCompletion";
                                            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                                            String fileName = "OCR_" + tenantId.ToString() + "_" + condition.OrderNumber.ToString() + "_" + date + ".pdf";

                                            byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);

                                            String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);


                                            sysCommAttachment.SCA_OriginalDocumentID = -1;
                                            sysCommAttachment.SCA_OriginalDocumentName = "OrderCompletedReport.pdf";
                                            sysCommAttachment.SCA_DocumentPath = retFilepath;
                                            sysCommAttachment.SCA_DocumentSize = reportContent.Length;
                                            sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                            sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                            sysCommAttachment.SCA_IsDeleted = false;
                                            sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                            sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                            sysCommAttachment.SCA_ModifiedBy = null;
                                            sysCommAttachment.SCA_ModifiedOn = null;
                                        }

                                        #region Code to generate report url place holder value

                                        Dictionary<String, String> reportUrlParameters = new Dictionary<String, String>
                                                         {
                                                            { "OrderID",  condition.OrderID.ToString()},
                                                            { "PackageGroupID",  condition.ServiceGroupID.ToString()},
                                                             { "ReportType",  BackgroundReportType.ORDER_COMPLETION.GetStringValue()},
                                                             { "TenantId",  tenantId.ToString()},
                                                             {"HierarchyNodeID",Convert.ToString(condition.HierarchyNodeID)},
                                                             {"IsReportSentToStudent",Convert.ToString(false)}
                                                         };
                                        StringBuilder reportUrl = new StringBuilder();
                                        reportUrl.Append(applicationUrl.Trim() + "?args=");
                                        reportUrl.Append(reportUrlParameters.ToEncryptedQueryString());

                                        #endregion

                                        dictMailData.Add(EmailFieldConstants.REPORT_URL, reportUrl.ToString());

                                        //Send mail

                                        if (PkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                                            systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS_WITHOUT_PDF_ATTACHMENT, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);
                                        else
                                            systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);

                                        #region UAT-1578: Addition for SMS notification
                                        //Send SMS notification
                                        //Create Dictionary for SMS Data
                                        Dictionary<String, object> dictSMSData = new Dictionary<String, object>();
                                        dictSMSData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(condition.FirstName, " ", condition.LastName));
                                        dictSMSData.Add(EmailFieldConstants.APPLICATION_URL, String.Concat(applicationUrl));
                                        dictSMSData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, String.Concat(condition.ServiceGroupName));
                                        dictSMSData.Add(EmailFieldConstants.ORDER_NO, condition.OrderNumber);

                                        CommunicationMockUpData mockSMSData = new CommunicationMockUpData();
                                        mockSMSData.UserName = string.Concat(condition.FirstName, " ", condition.LastName);
                                        mockSMSData.EmailID = condition.PrimaryEmailAddress;
                                        mockSMSData.ReceiverOrganizationUserID = condition.OrganizationUserID;
                                        CommunicationManager.SaveDataForSMSNotification(CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS, mockSMSData,
                                                                                        dictSMSData, tenantId, condition.HierarchyNodeID);
                                        #endregion


                                        if (systemCommunicationID != null)
                                        {
                                            if (!condition.IsEmployment && !PkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                                            {
                                                //Save Mail Attachment
                                                sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                                Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                            }

                                            OrderNotification ordNotification = new OrderNotification();
                                            ordNotification.ONTF_OrderID = condition.OrderID;
                                            ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                            ordNotification.ONTF_MSG_MessageID = null;
                                            ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                            ordNotification.ONTF_IsPostal = false;
                                            ordNotification.ONTF_CreatedByID = backgroundProcessUserId;
                                            ordNotification.ONTF_CreatedOn = DateTime.Now;
                                            ordNotification.ONTF_ModifiedByID = null;
                                            ordNotification.ONTF_ModifiedDate = null;
                                            ordNotification.ONTF_ParentNotificationID = null;
                                            ordNotification.ONTF_OrderNotificationTypeID = ordComDocumentTypeID;
                                            ordNotification.ONTF_BkgPackageSvcGroupID = condition.PackageServiceGroupID;
                                            ordNotification.ONTF_NotificationDetail = "Complete Order Result for " + condition.ServiceGroupName;

                                            ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                                        }
                                    });
                                });

                                ServiceLogger.Info("Ended foreach loop on CreateServiceGroupCompletedNotification based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                                //ServiceLogger.Info("Total Background Order Notification Records found: " + bkgOrdreNotificationDataList.Count(), BkgOrderStatusServiceLogger);
                            }
                            ServiceLogger.Info("Processed the chunk of Service Group Notification records: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateServiceGroupCompletedNotification.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateServiceGroupCompletedNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceLogger);
            }
        }

        public static void SendFlaggedCompletedServiceGroupNotification(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling SendFlaggedCompletedServiceGroupNotification: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, BkgOrderStatusServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
                        List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                        Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                            Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

                        String docAttachmentTypeCode = DocumentAttachmentType.ORDER_COMPLETION_DOCUMENT.GetStringValue();
                        Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                            Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                        String flgdCompletedSvcGrpNotificationTypeCode = OrderNotificationType.FLAGGED_COMPLETED_SVC_GRP_NOTIFICATION.GetStringValue();
                        Int32 flgdCompletedSvcGrpNotificationTypeCodeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                            Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == flgdCompletedSvcGrpNotificationTypeCode).ONT_ID) : Convert.ToInt32(0);

                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Flagged Background Service Group Completed Notification Data: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                            List<BkgOrderNotificationDataContract> bkgNotificationDataList = BackgroundProcessOrderManager.GetFlaggedServiceGroupCompletedNotificationData(tenantId, _recordChunkSize);

                            ServiceLogger.Info("Total Flagged Background Service Group Completed Records found: " + bkgNotificationDataList.Count, BkgOrderStatusServiceLogger);
                            ServiceLogger.Debug<List<BkgOrderNotificationDataContract>>("List of Service Group Notification from db:", bkgNotificationDataList, BkgOrderStatusServiceLogger);

                            if (bkgNotificationDataList.IsNotNull() && bkgNotificationDataList.Count > AppConsts.NONE)
                            {
                                Int32 ordNotificationID = 0;
                                Int32? systemCommunicationID = null;

                                ServiceLogger.Info("Started foreach loop on SendFlaggedCompletedServiceGroupNotification based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                bkgNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                                {
                                    ServiceLogger.Debug<Int32>("Next Background Order ID from the loop:", bkgOrderID, BkgOrderStatusServiceLogger);

                                    bkgNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                                    {
                                        //Create Dictionary for Mail And Message Data
                                        Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, "Admin");
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, String.Concat(applicationUrl));
                                        dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, String.Concat(condition.ServiceGroupName));
                                        dictMailData.Add(EmailFieldConstants.ORDER_NO, condition.OrderNumber);

                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        mockData.UserName = "Admin";
                                        mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                        mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;


                                        //UAT-1735:- don't send report result attachment for employment nodes, only the link. 
                                        SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();

                                        if (!condition.IsEmployment)
                                        {
                                            //Create Attachment
                                            ParameterValue[] parameters = new ParameterValue[4];
                                            parameters[0] = new ParameterValue();
                                            parameters[0].Name = "OrderID";
                                            parameters[0].Value = condition.OrderID.ToString(); //condition.BkgOrderID.ToString();
                                            parameters[1] = new ParameterValue();
                                            parameters[1].Name = "TenantID";
                                            parameters[1].Value = tenantId.ToString();
                                            parameters[2] = new ParameterValue();
                                            parameters[2].Name = "PackageGroupID";
                                            parameters[2].Value = condition.ServiceGroupID.ToString();
                                            parameters[3] = new ParameterValue();
                                            parameters[3].Name = "UserID";
                                            parameters[3].Value = AppConsts.STR_ONE;
                                            String reportName = "OrderCompletion";
                                            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                                            String fileName = "OCR_" + tenantId.ToString() + "_" + condition.OrderNumber.ToString() + "_" + date + ".pdf";

                                            byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);

                                            String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);


                                            sysCommAttachment.SCA_OriginalDocumentID = -1;
                                            sysCommAttachment.SCA_OriginalDocumentName = "OrderCompletedReport.pdf";
                                            sysCommAttachment.SCA_DocumentPath = retFilepath;
                                            sysCommAttachment.SCA_DocumentSize = reportContent.Length;
                                            sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                            sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                            sysCommAttachment.SCA_IsDeleted = false;
                                            sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                            sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                            sysCommAttachment.SCA_ModifiedBy = null;
                                            sysCommAttachment.SCA_ModifiedOn = null;
                                        }

                                        #region Code to generate report url place holder value

                                        Dictionary<String, String> reportUrlParameters = new Dictionary<String, String>
                                                         {
                                                            { "OrderID",  condition.OrderID.ToString()},
                                                            { "PackageGroupID",  condition.ServiceGroupID.ToString()},
                                                             { "ReportType",  BackgroundReportType.ORDER_COMPLETION.GetStringValue()},
                                                             { "TenantId",  tenantId.ToString()},
                                                             {"HierarchyNodeID",Convert.ToString(condition.HierarchyNodeID)},
                                                             {"IsReportSentToStudent",Convert.ToString(false)}
                                                         };
                                        StringBuilder reportUrl = new StringBuilder();
                                        reportUrl.Append(applicationUrl.Trim() + "?args=");
                                        reportUrl.Append(reportUrlParameters.ToEncryptedQueryString());

                                        #endregion

                                        dictMailData.Add(EmailFieldConstants.REPORT_URL, reportUrl.ToString());

                                        //Send mail
                                        systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_FLAGGED_COMPLETED_SERVICE_GROUP, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);

                                        if (systemCommunicationID != null)
                                        {
                                            if (!condition.IsEmployment)
                                            {
                                                //Save Mail Attachment
                                                sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                                Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                            }

                                            OrderNotification ordNotification = new OrderNotification();
                                            ordNotification.ONTF_OrderID = condition.OrderID;
                                            ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                            ordNotification.ONTF_MSG_MessageID = null;
                                            ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                            ordNotification.ONTF_IsPostal = false;
                                            ordNotification.ONTF_CreatedByID = backgroundProcessUserId;
                                            ordNotification.ONTF_CreatedOn = DateTime.Now;
                                            ordNotification.ONTF_ModifiedByID = null;
                                            ordNotification.ONTF_ModifiedDate = null;
                                            ordNotification.ONTF_ParentNotificationID = null;
                                            ordNotification.ONTF_OrderNotificationTypeID = flgdCompletedSvcGrpNotificationTypeCodeID;
                                            ordNotification.ONTF_BkgPackageSvcGroupID = condition.PackageServiceGroupID;
                                            ordNotification.ONTF_NotificationDetail = "Complete Order Result for flagged completed service group " + condition.ServiceGroupName;

                                            ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                                        }
                                    });
                                });

                                ServiceLogger.Info("Ended foreach loop on CreateServiceGroupCompletedNotification based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                                //ServiceLogger.Info("Total Background Order Notification Records found: " + bkgOrdreNotificationDataList.Count(), BkgOrderStatusServiceLogger);
                            }
                            ServiceLogger.Info("Processed the chunk of Service Group Notification records: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateFlaggedServiceGroupCompletedNotification.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateServiceGroupCompletedNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceLogger);
            }
        }

        /// <summary>
        /// Send Mail for Flagged Order
        /// </summary>
        /// <param name="tenantID"></param>
        public static void CreateBkgFlaggedOrderResultCompletedNotification(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling CreateBkgFlaggedOrderResultCompletedNotification: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, BkgOrderStatusServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
                        List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                        Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                            Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

                        String docAttachmentTypeCode = DocumentAttachmentType.ORDER_COMPLETION_DOCUMENT.GetStringValue();
                        Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                            Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                        String ordComDocumentTypeCode = OrderNotificationType.FLAGGED_ORDER_RESULT.GetStringValue();
                        Int32 ordComDocumentTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                            Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == ordComDocumentTypeCode).ONT_ID) : Convert.ToInt32(0);

                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Background Flagged Order Completed Result Notification Data: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                            List<BkgOrderNotificationDataContract> bkgOrderNotificationDataList = BackgroundProcessOrderManager.GetBkgFlaggedOrderResultCompletedNotificationData(tenantId, _recordChunkSize);

                            ServiceLogger.Info("Total Background Flagged Order Completed Result Records found: " + bkgOrderNotificationDataList.Count, BkgOrderStatusServiceLogger);
                            ServiceLogger.Debug<List<BkgOrderNotificationDataContract>>("List of Background Order Notification from db:", bkgOrderNotificationDataList, BkgOrderStatusServiceLogger);

                            if (bkgOrderNotificationDataList.IsNotNull() && bkgOrderNotificationDataList.Count > AppConsts.NONE)
                            {
                                Int32 ordNotificationID = 0;
                                Int32? systemCommunicationID = null;

                                ServiceLogger.Info("Started foreach loop on bkgOrdreNotificationDataList based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                bkgOrderNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                                {
                                    ServiceLogger.Debug<Int32>("Next Background Order ID from the loop:", bkgOrderID, BkgOrderStatusServiceLogger);

                                    bkgOrderNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                                    {
                                        //Create Dictionary for Mail And Message Data
                                        Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, "Admin");
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, String.Concat(applicationUrl));
                                        dictMailData.Add(EmailFieldConstants.ORDER_NO, condition.OrderNumber);

                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        mockData.UserName = "Admin";
                                        mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                        mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                                        //UAT-1735:- don't send report result attachment for employment nodes, only the link. 
                                        SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();

                                        if (!condition.IsEmployment)
                                        {
                                            //Create Attachment
                                            ParameterValue[] parameters = new ParameterValue[3];
                                            parameters[0] = new ParameterValue();
                                            parameters[0].Name = "OrderID";
                                            parameters[0].Value = condition.OrderID.ToString(); //condition.BkgOrderID.ToString();
                                            parameters[1] = new ParameterValue();
                                            parameters[1].Name = "TenantID";
                                            parameters[1].Value = tenantId.ToString();
                                            parameters[2] = new ParameterValue();
                                            parameters[2].Name = "UserID";
                                            parameters[2].Value = AppConsts.STR_ONE;

                                            String reportName = "OrderCompletion";
                                            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                                            String fileName = "Flagged_OCR_" + tenantId.ToString() + "_" + condition.OrderNumber.ToString() + "_" + date + ".pdf";

                                            byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);

                                            String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);

                                            sysCommAttachment.SCA_OriginalDocumentID = -1;
                                            sysCommAttachment.SCA_OriginalDocumentName = "FlaggedOrderCompletedReport.pdf";
                                            sysCommAttachment.SCA_DocumentPath = retFilepath;
                                            sysCommAttachment.SCA_DocumentSize = reportContent.Length;
                                            sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                            sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                            sysCommAttachment.SCA_IsDeleted = false;
                                            sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                            sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                            sysCommAttachment.SCA_ModifiedBy = null;
                                            sysCommAttachment.SCA_ModifiedOn = null;
                                        }

                                        #region Code to generate report url place holder value

                                        Dictionary<String, String> reportUrlParameters = new Dictionary<String, String>
                                                         {
                                                            { "OrderID",  condition.OrderID.ToString()},
                                                             { "ReportType",  BackgroundReportType.ORDER_COMPLETION.GetStringValue()},
                                                             { "TenantId",  tenantId.ToString()},
                                                             {"HierarchyNodeID",Convert.ToString(condition.HierarchyNodeID)},
                                                             {"IsReportSentToStudent",Convert.ToString(false)}
                                                         };
                                        StringBuilder reportUrl = new StringBuilder();
                                        reportUrl.Append(applicationUrl.Trim() + "?args=");
                                        reportUrl.Append(reportUrlParameters.ToEncryptedQueryString());

                                        #endregion

                                        dictMailData.Add(EmailFieldConstants.REPORT_URL, reportUrl.ToString());

                                        //Send mail
                                        systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_FLAGGED_ORDER_RESULTS, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);

                                        if (systemCommunicationID != null)
                                        {
                                            //UAT-1735:- don't send report result attachment for employment nodes, only the link. 
                                            if (!condition.IsEmployment)
                                            {
                                                //Save Mail Attachment
                                                sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                                Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                            }

                                            OrderNotification ordNotification = new OrderNotification();
                                            ordNotification.ONTF_OrderID = condition.OrderID;
                                            ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                            ordNotification.ONTF_MSG_MessageID = null;
                                            ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                            ordNotification.ONTF_IsPostal = false;
                                            ordNotification.ONTF_CreatedByID = backgroundProcessUserId;
                                            ordNotification.ONTF_CreatedOn = DateTime.Now;
                                            ordNotification.ONTF_ModifiedByID = null;
                                            ordNotification.ONTF_ModifiedDate = null;
                                            ordNotification.ONTF_ParentNotificationID = null;
                                            ordNotification.ONTF_OrderNotificationTypeID = ordComDocumentTypeID;
                                            ordNotification.ONTF_NotificationDetail = "Complete Flagged Order Result";

                                            ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                                        }
                                    });
                                });

                                ServiceLogger.Info("Ended foreach loop on bkgOrdreNotificationDataList based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                                //ServiceLogger.Info("Total Background Order Notification Records found: " + bkgOrdreNotificationDataList.Count(), BkgOrderStatusServiceLogger);
                            }
                            ServiceLogger.Info("Processed the chunk of Background Order Notification records: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateBkgFlaggedOrderResultCompletedNotification.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateBkgFlaggedOrderResultCompletedNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceLogger);
            }
        }

        #region  UAT-1177:System Updates for 613 [Employment notification]
        /// <summary>
        /// Send Mail for flagged orders for emplyment
        /// </summary>
        /// <param name="tenantID"></param>
        public static void SendBkgFlaggedOrderCompletedEmploymentNotification(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling SendBkgFlaggedOrderCompletedEmploymentNotification: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName;
                     config.Tenant.Contact = item.Tenant.Contact;
                     config.Tenant.AddressHandle = item.Tenant.AddressHandle;
                     return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, BkgOrderStatusServiceLogger))
                    {
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        ServiceLogger.Info("Starting getting Employment Notification Place Holder Data for TenantID: " + tenantId,
                                            BkgOrderStatusServiceLogger);

                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        String instituteName = String.Empty;
                        String institutePhonenumber = String.Empty;

                        //Get intitution phone number
                        //if (clntDbConf.Tenant.IsNotNull() && clntDbConf.Tenant.Contact.IsNotNull() && clntDbConf.Tenant.Contact.ContactDetails.IsNotNull())

                        String primaryPhoneContactTypeCode = ContactType.PrimaryPhone.GetStringValue();
                        Entity.lkpContactType contactType = LookupManager.GetLookUpData<Entity.lkpContactType>().FirstOrDefault(cond => cond.ContactCode
                                                                                                                                 == primaryPhoneContactTypeCode);
                        Int32 primaryPhoneContactTypeId = contactType.IsNotNull() ? contactType.ContactTypeID : 0;

                        Entity.Tenant institutionDetails = SecurityManager.GetTenant(tenantId);
                        instituteName = institutionDetails.TenantName;
                        var contactDetail = institutionDetails.Contact.ContactDetails.FirstOrDefault(cnd => cnd.ContactTypeID == primaryPhoneContactTypeId);
                        if (contactDetail.IsNotNull())
                        {
                            institutePhonenumber = contactDetail.ContactTypeValue;
                            institutePhonenumber = ApplicationDataManager.GetFormattedPhoneNumber(institutePhonenumber);
                        }

                        Entity.Address tenantAddress = null;
                        //Get Institution address
                        if (institutionDetails.AddressHandle.IsNotNull() && institutionDetails.AddressHandle.Addresses.IsNotNull())
                        {
                            tenantAddress = institutionDetails.AddressHandle.Addresses.FirstOrDefault();
                        }

                        StringBuilder instituteAddress = new StringBuilder();

                        if (tenantAddress.IsNotNull())
                        {
                            String TenantAddress = tenantAddress.Address1;
                            String tenantZipCode = (!tenantAddress.ZipCode.IsNull()) ? tenantAddress.ZipCode.ZipCode1 : String.Empty;
                            String tenantCity = (!tenantAddress.ZipCode.City.IsNull()) ? tenantAddress.ZipCode.City.CityName : String.Empty;
                            String tenantState = (!tenantAddress.ZipCode.City.State.IsNull())
                                                ? tenantAddress.ZipCode.City.State.StateAbbreviation : String.Empty;

                            instituteAddress.Append(TenantAddress + ", " + tenantCity + ", " + tenantState + " " + tenantZipCode);
                        }
                        ServiceLogger.Info("End getting Employment Notification Place Holder Data for TenantID: " + tenantId,
                                                                        BkgOrderStatusServiceLogger);

                        List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                        Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > AppConsts.NONE ?
                                                      Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) :
                                                      Convert.ToInt16(0);
                        //Order notification type [Flagged Order employment notification type]
                        String ordComEmpTypeCode = OrderNotificationType.FLAGGED_ORDER_EMPLOYMENT_NOTIFICATION.GetStringValue();
                        Int32 ordComEmpTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > AppConsts.NONE ?
                                                Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == ordComEmpTypeCode).ONT_ID) :
                                                Convert.ToInt32(0);

                        Boolean executeLoop = true;
                        ServiceLogger.Info("Starting While Lopp for SendBkgFlaggedOrderCompletedEmploymentNotification for TenantID: " + tenantId,
                                            BkgOrderStatusServiceLogger);
                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Background Flagged Order Completed Employment Notification Data: " + DateTime.Now.ToString(),
                                                BkgOrderStatusServiceLogger);

                            List<BkgOrderNotificationDataContract> bkgOrderEmpNotificationDataList = BackgroundProcessOrderManager.GetBkgFlaggedOrderEmploymentNotificationData
                                                                                                     (tenantId, _recordChunkSize);

                            ServiceLogger.Info("Total Background Flagged Order Completed Employment Records found: " + bkgOrderEmpNotificationDataList.Count,
                                                BkgOrderStatusServiceLogger);

                            if (bkgOrderEmpNotificationDataList.IsNotNull() && bkgOrderEmpNotificationDataList.Count > AppConsts.NONE)
                            {
                                Int32 ordNotificationID = 0;
                                Int32? systemCommunicationID = null;

                                ServiceLogger.Info("Started foreach loop on bkgOrderEmpNotificationDataList based on Background Order id: " + DateTime.Now.ToString(),
                                                    BkgOrderStatusServiceLogger);
                                bkgOrderEmpNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                                {
                                    ServiceLogger.Debug<Int32>("Next Background Order ID from the loop:", bkgOrderID, BkgOrderStatusServiceLogger);

                                    bkgOrderEmpNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                                    {
                                        //Get List Of Client Admins 
                                        String ccCode = CopyType.CC.GetStringValue();
                                        StringBuilder strBldClientAdmin = new StringBuilder();
                                        String clientAdminNames = String.Empty;
                                        List<CommunicationCCUsersList> clientAdminList = CommunicationManager.GetClientAdminsForEmploymentNotification
                                                                                         (CommunicationSubEvents.EMPLOYMENT_NOTIFICATION_FOR_FLAG_ORDER,
                                                                                          tenantId, condition.HierarchyNodeID);

                                        clientAdminList.Where(cond => cond.CopyCode == ccCode).ForEach(CA =>
                                                    strBldClientAdmin.Append(CA.UserName + ", ")
                                            );
                                        //Concat client admins in comma seperated string
                                        if (strBldClientAdmin.Length > AppConsts.NONE)
                                        {
                                            clientAdminNames = strBldClientAdmin.ToString().Remove(strBldClientAdmin.Length - 2).TrimEnd();
                                        }

                                        //Create Dictionary for Mail And Message Data
                                        Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                                        dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, condition.FirstName + " " + condition.LastName);
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, String.Concat(instituteName));
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_ADDRESS, instituteAddress.ToString());
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_PHONE_NUMBER, institutePhonenumber);
                                        dictMailData.Add(EmailFieldConstants.CURRENT_DATE, Convert.ToString(DateTime.Now.ToShortDateString()));
                                        dictMailData.Add(EmailFieldConstants.APPLICANT_PROFILE_ADDRESS, condition.ApplicantProfileAddress);
                                        dictMailData.Add(EmailFieldConstants.CLIENT_ADMIN_CONTACT_NAMES, clientAdminNames);

                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        mockData.UserName = condition.FirstName + " " + condition.LastName;
                                        mockData.EmailID = condition.PrimaryEmailAddress;
                                        mockData.ReceiverOrganizationUserID = condition.OrganizationUserID;

                                        //Send mail
                                        systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.EMPLOYMENT_NOTIFICATION_FOR_FLAG_ORDER,
                                                                                                                 dictMailData, mockData, tenantId, condition.HierarchyNodeID,
                                                                                                                 null, null, true);
                                        //Insert record in Order notification 
                                        if (systemCommunicationID != null)
                                        {
                                            OrderNotification ordNotification = new OrderNotification();
                                            ordNotification.ONTF_OrderID = condition.OrderID;
                                            ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                            ordNotification.ONTF_MSG_MessageID = null;
                                            ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                            ordNotification.ONTF_IsPostal = false;
                                            ordNotification.ONTF_CreatedByID = backgroundProcessUserId;
                                            ordNotification.ONTF_CreatedOn = DateTime.Now;
                                            ordNotification.ONTF_ModifiedByID = null;
                                            ordNotification.ONTF_ModifiedDate = null;
                                            ordNotification.ONTF_ParentNotificationID = null;
                                            ordNotification.ONTF_OrderNotificationTypeID = ordComEmpTypeID;
                                            ordNotification.ONTF_NotificationDetail = "Employment Notification for Flagged Order";

                                            ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                                        }
                                    });
                                });

                                ServiceLogger.Info("Ended foreach loop on bkgOrderEmpNotificationDataList based on Background Order id: " + DateTime.Now.ToString(),
                                                    BkgOrderStatusServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                            }
                            ServiceLogger.Info("Processed the chunk of Background Order Employment Notification records: " + DateTime.Now.ToString(),
                                                BkgOrderStatusServiceLogger);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendBkgFlaggedOrderCompletedEmploymentNotification.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in SendBkgFlaggedOrderCompletedEmploymentNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    BkgOrderStatusServiceLogger);
            }
        }
        #endregion

        public static void CreateEmploymentFlaggedServiceGroupCompletedNotification(Int32? tenant_Id = null)
        {
            try
            {
                ServiceLogger.Info("Calling CreateEmploymentFlaggedServiceGroupCompletedNotification: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, BkgOrderStatusServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        String instituteName = String.Empty;
                        String institutePhonenumber = String.Empty;
                        String primaryPhoneContactTypeCode = ContactType.PrimaryPhone.GetStringValue();
                        Entity.lkpContactType contactType = LookupManager.GetLookUpData<Entity.lkpContactType>().FirstOrDefault(cond => cond.ContactCode
                                                                                                                                 == primaryPhoneContactTypeCode);
                        Int32 primaryPhoneContactTypeId = contactType.IsNotNull() ? contactType.ContactTypeID : 0;
                        Entity.Tenant institutionDetails = SecurityManager.GetTenant(tenantId);
                        instituteName = institutionDetails.TenantName;
                        var contactDetail = institutionDetails.Contact.ContactDetails.FirstOrDefault(cnd => cnd.ContactTypeID == primaryPhoneContactTypeId);
                        if (contactDetail.IsNotNull())
                        {
                            institutePhonenumber = contactDetail.ContactTypeValue;
                            institutePhonenumber = ApplicationDataManager.GetFormattedPhoneNumber(institutePhonenumber);
                        }

                        Entity.Address tenantAddress = null;
                        //Get Institution address
                        if (institutionDetails.AddressHandle.IsNotNull() && institutionDetails.AddressHandle.Addresses.IsNotNull())
                        {
                            tenantAddress = institutionDetails.AddressHandle.Addresses.FirstOrDefault();
                        }


                        StringBuilder instituteAddress = new StringBuilder();

                        if (tenantAddress.IsNotNull())
                        {
                            String TenantAddress = tenantAddress.Address1;
                            String tenantZipCode = (!tenantAddress.ZipCode.IsNull()) ? tenantAddress.ZipCode.ZipCode1 : String.Empty;
                            String tenantCity = (!tenantAddress.ZipCode.City.IsNull()) ? tenantAddress.ZipCode.City.CityName : String.Empty;
                            String tenantState = (!tenantAddress.ZipCode.City.State.IsNull())
                                                ? tenantAddress.ZipCode.City.State.StateAbbreviation : String.Empty;

                            instituteAddress.Append(TenantAddress + ", " + tenantCity + ", " + tenantState + " " + tenantZipCode);
                        }

                        List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
                        List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                        Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                            Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

                        String flaggedCompletedEmpSvcGrpTypeCode = OrderNotificationType.FLAGGED_COMPLETED_SVC_GRP_EMPLOYMENT_NOTIFICATION.GetStringValue();
                        Int32 flaggedCompletedEmpSvcGrpTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                            Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == flaggedCompletedEmpSvcGrpTypeCode).ONT_ID) : Convert.ToInt32(0);

                        String docAttachmentTypeCode = DocumentAttachmentType.APPLICANT_DOCUMENT.GetStringValue();
                        Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                            Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                        //Getting Documnet
                        string summaryOfRightPdfLocation = string.Empty;
                        int summaryRightPdfLength = 0;

                        try
                        {
                            summaryOfRightPdfLocation = ConfigurationManager.AppSettings["SummaryOfRightPdfLocation"];

                            if (!string.IsNullOrEmpty(summaryOfRightPdfLocation))
                            {
                                Boolean aWSUseS3 = false;
                                byte[] contentByte;

                                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                                {
                                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                                }

                                if (aWSUseS3)
                                {
                                    contentByte = CommonFileManager.RetrieveDocument(summaryOfRightPdfLocation, FileType.ApplicantFileLocation.GetStringValue());
                                    summaryOfRightPdfLocation = CommonFileManager.GetFilePath(summaryOfRightPdfLocation, FileType.ApplicantFileLocation.GetStringValue());
                                }
                                else
                                {
                                    summaryOfRightPdfLocation = CommonFileManager.GetFilePath(summaryOfRightPdfLocation, FileType.ApplicantFileLocation.GetStringValue());
                                    contentByte = CommonFileManager.RetrieveDocument(summaryOfRightPdfLocation, FileType.ApplicantFileLocation.GetStringValue());
                                }

                                if (contentByte.IsNotNull())
                                {
                                    summaryRightPdfLength = contentByte.Length;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ServiceLogger.Error(String.Format("An Error has occured while getting summary right pdf documnet in CreateEmploymentFlaggedServiceGroupCompletedNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                                            ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceLogger);
                        }

                        Boolean executeLoop = true;

                        ServiceLogger.Info("Starting While Lopp for CreateEmploymentFlaggedServiceGroupCompletedNotification for TenantID: " + tenantId,
                                           BkgOrderStatusServiceLogger);

                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Background Flagged Employment Service Group Completed Notification Data: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);

                            List<BkgOrderNotificationDataContract> bkgFlaggedEmploymentServiceGroupCompletedNotificationDataList = BackgroundProcessOrderManager.GetFlaggedEmploymentServiceGroupCompletedNotificationData(tenantId, _recordChunkSize);

                            ServiceLogger.Info("Total Background Flagged Employment Service Group Completed Records found: " + bkgFlaggedEmploymentServiceGroupCompletedNotificationDataList.Count, BkgOrderStatusServiceLogger);
                            ServiceLogger.Debug<List<BkgOrderNotificationDataContract>>("List of Service Group Notification from db:", bkgFlaggedEmploymentServiceGroupCompletedNotificationDataList, BkgOrderStatusServiceLogger);

                            if (bkgFlaggedEmploymentServiceGroupCompletedNotificationDataList.IsNotNull() && bkgFlaggedEmploymentServiceGroupCompletedNotificationDataList.Count > AppConsts.NONE)
                            {
                                Int32 ordNotificationID = 0;
                                Int32? systemCommunicationID = null;

                                ServiceLogger.Info("Started foreach loop on CreateBkgOrderResultCompletedNotification based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                bkgFlaggedEmploymentServiceGroupCompletedNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                                {
                                    ServiceLogger.Debug<Int32>("Next Background Order ID from the loop:", bkgOrderID, BkgOrderStatusServiceLogger);

                                    bkgFlaggedEmploymentServiceGroupCompletedNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                                    {
                                        //Get List Of Client Admins 
                                        String ccCode = CopyType.CC.GetStringValue();
                                        StringBuilder strBldClientAdmin = new StringBuilder();
                                        String clientAdminNames = String.Empty;
                                        List<CommunicationCCUsersList> clientAdminList = CommunicationManager.GetClientAdminsForEmploymentNotification
                                                                                         (CommunicationSubEvents.EMPLOYMENT_NOTIFICATION_FOR_FLAGGED_COMPLETED_SERVICE_GROUP,
                                                                                          tenantId, condition.HierarchyNodeID);

                                        clientAdminList.Where(cond => cond.CopyCode == ccCode).ForEach(CA =>
                                                    strBldClientAdmin.Append(CA.UserName + ", ")
                                            );

                                        //Concat client admins in comma seperated string
                                        if (strBldClientAdmin.Length > AppConsts.NONE)
                                        {
                                            clientAdminNames = strBldClientAdmin.ToString().Remove(strBldClientAdmin.Length - 2).TrimEnd();
                                        }

                                        //Create Dictionary for Mail And Message Data
                                        Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                                        dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, condition.FirstName + " " + condition.LastName);
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, String.Concat(instituteName));
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_ADDRESS, instituteAddress.ToString());
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_PHONE_NUMBER, institutePhonenumber);
                                        dictMailData.Add(EmailFieldConstants.CURRENT_DATE, Convert.ToString(DateTime.Now.ToShortDateString()));
                                        dictMailData.Add(EmailFieldConstants.APPLICANT_PROFILE_ADDRESS, condition.ApplicantProfileAddress);
                                        dictMailData.Add(EmailFieldConstants.CLIENT_ADMIN_CONTACT_NAMES, clientAdminNames);

                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        mockData.UserName = condition.FirstName + " " + condition.LastName;
                                        mockData.EmailID = condition.PrimaryEmailAddress;
                                        mockData.ReceiverOrganizationUserID = condition.OrganizationUserID;

                                        //Send mail
                                        systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.EMPLOYMENT_NOTIFICATION_FOR_FLAGGED_COMPLETED_SERVICE_GROUP, dictMailData, mockData, tenantId, condition.HierarchyNodeID, null, null, true);

                                        if (systemCommunicationID != null)
                                        {
                                            //Save Mail Attachment
                                            if (summaryRightPdfLength > 0)
                                            {
                                                SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                                                sysCommAttachment.SCA_OriginalDocumentID = -1;
                                                sysCommAttachment.SCA_OriginalDocumentName = "Summary of Rights.pdf";
                                                sysCommAttachment.SCA_DocumentPath = summaryOfRightPdfLocation;
                                                sysCommAttachment.SCA_DocumentSize = summaryRightPdfLength;
                                                sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                                sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                                sysCommAttachment.SCA_IsDeleted = false;
                                                sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                                sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                                sysCommAttachment.SCA_ModifiedBy = null;
                                                sysCommAttachment.SCA_ModifiedOn = null;
                                                sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                                Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                            }

                                            OrderNotification ordNotification = new OrderNotification();
                                            ordNotification.ONTF_OrderID = condition.OrderID;
                                            ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                            ordNotification.ONTF_MSG_MessageID = null;
                                            ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                            ordNotification.ONTF_IsPostal = false;
                                            ordNotification.ONTF_CreatedByID = backgroundProcessUserId;
                                            ordNotification.ONTF_CreatedOn = DateTime.Now;
                                            ordNotification.ONTF_ModifiedByID = null;
                                            ordNotification.ONTF_ModifiedDate = null;
                                            ordNotification.ONTF_ParentNotificationID = null;
                                            ordNotification.ONTF_OrderNotificationTypeID = flaggedCompletedEmpSvcGrpTypeID;
                                            ordNotification.ONTF_BkgPackageSvcGroupID = condition.PackageServiceGroupID;
                                            ordNotification.ONTF_NotificationDetail = "Background Flagged Employment Service Group Completed for " + condition.ServiceGroupName;

                                            ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                                        }
                                    });
                                });

                                ServiceLogger.Info("Ended foreach loop on CreateEmploymentFlaggedServiceGroupCompletedNotification based on Background Order id: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                            }
                            ServiceLogger.Info("Processed the chunk of Flagged Employment Service Group Notification records: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateEmploymentFlaggedServiceGroupCompletedNotification.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), BkgOrderStatusServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateEmploymentFlaggedServiceGroupCompletedNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceLogger);
            }
        }


        public static void SendNotificationForFingerprintingExceededTAT()
        {
            try
            {
                ServiceLogger.Info("************************** Calling SendNotificationForFingerprintingExceededTAT:" + DateTime.Now.ToString() + " * ******************", BkgOrderStatusServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
       
         
                DateTime jobStartTime = DateTime.Now;
                DateTime jobEndTime;

                string fileName = string.Concat("Finger_Printing_Exceeded_TAT_Report_", DateTime.Now.ToString("MMddyyyy"));


                ServiceLogger.Info("******************* Start Creating Excel - Finger Printing Exceeded TAT: " + DateTime.Now.ToString() + " *******************", BkgOrderStatusServiceLogger);
                string reportName = "FingerprintingExceededTATReport";
                byte[] binaryFile = BkgSvcFormFingerPrintingExceededReport(reportName);
          
                ServiceLogger.Info("******************* End Creating Excel - Finger Printing Exceeded TAT: " + DateTime.Now.ToString() + " *******************", BkgOrderStatusServiceLogger);

                ServiceLogger.Info("******************* Start Saving Excel Report - Finger Printing Exceeded TAT: " + DateTime.Now.ToString() + " *******************", BkgOrderStatusServiceLogger);

                string savedFilePath = ReportManager.SaveDocument(binaryFile, string.Concat(fileName, ".xlsx"));

                ServiceLogger.Info("******************* End Saving Excel Report - Finger Printing Exceeded TAT: " + DateTime.Now.ToString() + " *******************", BkgOrderStatusServiceLogger);


                String attachedFiles = String.Empty;

                Int32? sysCommAttachmentID = null;


                Entity.AppConfiguration appConfigurationForStudentServiceForm = SecurityManager.GetAppConfiguration(AppConsts.Receiver_Email_Fingerprinting_ExceededTAT);
                String emailIds = String.Empty;
                List<String> emailAddressesToSendNotification = new List<String>();
                if (appConfigurationForStudentServiceForm.IsNotNull())
                {
                    emailIds = appConfigurationForStudentServiceForm.AC_Value;
                    emailAddressesToSendNotification = emailIds.Split(',').ToList<String>();
                }
                List<CommunicationTemplateContract> lstUser = new List<CommunicationTemplateContract>();
                foreach (var email in emailAddressesToSendNotification)
                {
                    lstUser.Add(new CommunicationTemplateContract() { RecieverEmailID = email, RecieverName = String.Empty, CurrentUserId = backgroundProcessUserId, ReceiverOrganizationUserId = 0, IsToUser = true });
                }

                CommunicationMockUpData mockData = new CommunicationMockUpData();
                mockData.UserName = AppConsts.Admin;
                mockData.EmailID = emailIds;
                mockData.ReceiverOrganizationUserID = AppConsts.NONE;

                //Send mail
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                Int32? systemCommunicationId = CommunicationManager.SendMailForFingerprintingExceededTAT(CommunicationSubEvents.NOTIFICATION_FOR_FINGER_PRINTING_EXCEEDED_TAT, dictMailData, lstUser);

                SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                sysCommAttachment.SCA_OriginalDocumentID = -1;
                sysCommAttachment.SCA_OriginalDocumentName = string.Concat(fileName, ".xlsx");

                sysCommAttachment.SCA_DocumentPath = savedFilePath;
                sysCommAttachment.SCA_DocumentSize = binaryFile.Length;
                sysCommAttachment.SCA_DocAttachmentTypeID = GetAttachmentDocumentType(DocumentAttachmentType.DAILY_REPORT.GetStringValue());
                sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                sysCommAttachment.SCA_IsDeleted = false;
                sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                sysCommAttachment.SCA_SystemCommunicationID = Convert.ToInt32(systemCommunicationId);

                sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                ServiceLogger.Debug("******************* Placed entry in email queue  for for Finger Printing Exceeded TAT: " + DateTime.Now.ToString() + " Notification delivery id: *******************", BkgOrderStatusServiceLogger);
                ServiceContext.ReleaseDBContextItems();

                //Save service logging data to DB
                if (_isServiceLoggingEnabled)
                {
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                    serviceLoggingContract.JobName = JobName.NotificationForFingerprintingExceededTAT.GetStringValue();
                    serviceLoggingContract.TenantID = SecurityManager.DefaultTenantID;
                    serviceLoggingContract.JobStartTime = jobStartTime;
                    serviceLoggingContract.JobEndTime = jobEndTime;
                    serviceLoggingContract.IsDeleted = false;
                    serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                    serviceLoggingContract.CreatedOn = DateTime.Now;
                    SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                }
            }
            catch (Exception ex)
            {

                ServiceLogger.Error(String.Format("An Error has occured in SendNotificationForFingerPrintingExceededTAT method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),BkgOrderStatusServiceLogger);
            }
        }



        #endregion

        #region Private Methods

        private static byte[] BkgSvcFormFingerPrintingExceededReport(String ReportName)
        {
            try
            {
                ParameterValue[] parameter;

                parameter = new ParameterValue[1];

                parameter[0] = new ParameterValue();
                parameter[0].Name = AppConsts.TENANT_ID;
                parameter[0].Value = AppConsts.ZERO;
                byte[] reportContent = ReportManager.GetReportByteArrayFormat(ReportName, parameter);
                return reportContent;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static Int16 GetAttachmentDocumentType(String docAttachmentTypeCode)
        {
            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

            return !docAttachmentType.IsNullOrEmpty()
                    ? Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID)
                    : Convert.ToInt16(AppConsts.NONE);
        }

        #endregion

        #endregion

    }
}








