using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.ServiceModel;

namespace INTSOF.UnityHelper.Interfaces
{
    [ServiceContract]
    public interface IQueueClientService
    {
        [OperationContract]
        Boolean SendMessage(String queueName, Int32 Initiator, String message, String applicationDatabaseName, Int32 senderId, String subject,out Guid adbMessageId);

        [OperationContract]
        Boolean SendMessagesInBatch(String queueName, Int32 initiator, String[] message);

        [OperationContract]
        EntityObject GetMessage(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, Guid messageId);

        [OperationContract]
        IEnumerable<EntityObject> GetAllMessages(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId);

        [OperationContract]
        IEnumerable<EntityObject> GetNewMessages(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, DateTime lastDateTime);

        [OperationContract]
        IEnumerable<EntityObject> GetQueue(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId);

        [OperationContract]
        IEnumerable<EntityObject> GetUpdateQueue(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, DateTime lastDateTime);

        [OperationContract]
        Boolean ReplyMesssage(String queueName, Guid messageId, Int32 initiator, String message, String applicationDatabaseName, Int32 senderId, String subject);

        [OperationContract]
        IEnumerable<EntityObject> GetRecentMessages(String queueName, String folderCode, Int32 queueOwnerId, Int32 communicationTypeId);
    }
}
