using Entity;
using INTSOF.UnityHelper.Host;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;

namespace Business.RepoManagers
{
    public class QueueManager
    {

        public Boolean SendMessage(String queueName, Int32 initiator, String message, String applicationDatabaseName, Int32 senderId, String subject, out Guid adbMessageID)
        {
            adbMessageID = Guid.Empty;
            try
            {
                QueueClientService queueService = new QueueClientService();
                return queueService.SendMessage(queueName, initiator, message, applicationDatabaseName, senderId, subject, out adbMessageID);
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

        public Boolean SendMessageToQueue(MessagingContract messagingContract)
        {
            try
            {

                QueueClientService queueService = new QueueClientService();

                if (messagingContract.MessageMode.ToLower() == MessageMode.DRAFTMESSAGE.GetStringValue().ToLower())
                {
                    if (queueService.SendMessageToQueue(messagingContract))
                        return true;
                }
                else // SEND EMAIL ONLY IF IT IS NOT THE DRAFT CASE
                {

                    if (queueService.SendMessageToQueue(messagingContract))
                    {
                        CommunicationManager.SendInternalMessageMail(messagingContract.ToUserIds, messagingContract.CcUserIds,messagingContract.BccUserIds);
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
               
        public IEnumerable<EntityObject> GetQueue(String queueName, Int32 folderId, String folderCode, Int32 queueType, Int32 queueOwnerId, Int32 userGroup)
        {
            try
            {
                String communicationTypeCode = lkpCommunicationTypeContext.EMAIL.GetStringValue();
                Int32 communicationTypeId = LookupManager.GetMessagingLookUpData<lkpCommunicationType>().Where(condition => condition.Code.Equals(communicationTypeCode)).FirstOrDefault().CommunicationTypeID;
                QueueClientService queueService = new QueueClientService();
                return queueService.GetQueue(queueName, folderId, folderCode, queueType, queueOwnerId, userGroup, communicationTypeId);
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

        public IEnumerable<EntityObject> GetQueue(String queueName,Int32 queueOwnerId)
        {
            try
            {
                String communicationTypeCode = lkpCommunicationTypeContext.EMAIL.GetStringValue();
                Int32 communicationTypeId = LookupManager.GetMessagingLookUpData<lkpCommunicationType>().Where(condition => condition.Code.Equals(communicationTypeCode)).FirstOrDefault().CommunicationTypeID;
                QueueClientService queueService = new QueueClientService();
                return queueService.GetQueue(queueName, queueOwnerId, communicationTypeId);
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

        public IEnumerable<EntityObject> GetUpdateQueue(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, DateTime lastDateTime)
        {
            try
            {
                QueueClientService queueService = new QueueClientService();
                return queueService.GetUpdateQueue(queueName, folderId, queueType, queueOwnerId, lastDateTime);
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

        public IEnumerable<EntityObject> GetRecentMessages(String queueName, String folderCode, Int32 queueOwnerId)
        {
            try
            {
                String communicationTypeCode = lkpCommunicationTypeContext.EMAIL.GetStringValue();
                Int32 communicationTypeId = LookupManager.GetMessagingLookUpData<lkpCommunicationType>().Where(condition => condition.Code.Equals(communicationTypeCode)).FirstOrDefault().CommunicationTypeID;
                QueueClientService queueService = new QueueClientService();
                return queueService.GetRecentMessages(queueName, folderCode, queueOwnerId, communicationTypeId);
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
    }
}
