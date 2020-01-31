using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;

namespace INTSOF.UnityHelper.Interfaces
{
    public interface IMessageQueue
    {
        Boolean SendMessage(Int32 intiator, String message, String applicationDatabaseName, Int32 senderId, String subject, out Guid adbMessageID);
        Boolean SendMessageInBatch(Int32 intiator, String[] messages);
        Boolean ReplyMessage(Guid messageId, Int32 initiator, String message, String applicationDatabaseName, Int32 senderId, String subject);
        EntityObject GetMessage(Int32 folderId, Int32 queueType, Int32 queueOwnerId, Guid messageId);
        IEnumerable<EntityObject> GetQueue(Int32 folderId, String folderCode, Int32 queueType, Int32 queueOwnerId, Int32 userGroup, Int32 communicationTypeId);
        IEnumerable<EntityObject> GetQueue(Int32 queueOwnerId, Int32 communicationTypeId);
        IEnumerable<EntityObject> GetUpdateQueue(Int32 folderId, Int32 queueType, Int32 queueOwnerId, DateTime lastDateTime);
        IQueryable<Entity.ADBMessageToList> GetRecentMessages(String folderCode, Int32 queueOwnerId, Int32 communicationTypeId);
    }
}
