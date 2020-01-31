using INTSOF.UnityHelper.Interfaces;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Text;

namespace INTSOF.UnityHelper.Host
{
    public class QueueClientService : IQueueClientService
    {

        QueueServiceHostFactory _hostFactory;

        public QueueClientService()
        {

        }
        public QueueClientService(String prefix)
        {

        }
        #region IQueueClientService Members

        public Boolean SendMessage(String queueName, Int32 initiator, String message, String applicationDatabaseName, Int32 senderId, String subject, out Guid adbMessageID)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.SendMessage(queueName, initiator, message, applicationDatabaseName, senderId, subject, out adbMessageID);
        }

        public Boolean SendMessageToQueue(Entity.MessagingContract messagingContract)
        {
            //  Int32 isNewMail = messagingContract.Action == MessagingAction.NewMail || messagingContract.Action == MessagingAction.Forward ? 1 : 0;
            //   String formattedMessage = FormatMessage(messagingContract.MessageId, isNewMail, messagingContract.MessageMode, messagingContract.ToUserIds, messagingContract.CCUserGroupIds, messagingContract.FolderId, messagingContract.IsHighImportance, messagingContract.CurrentUserId, messagingContract.Subject, messagingContract.QueueType, messagingContract.Action, messagingContract.TenantTypes, messagingContract.Content, messagingContract.MessageType, messagingContract.ToUserGroupIds, messagingContract.From,messagingContract.ToList,messagingContract.CcList);

            String formattedMessage = FormatMessage(messagingContract);
            switch (messagingContract.Action)
            {
                case MessagingAction.NewMail:
                case MessagingAction.Forward:
                    Guid adbMessageID;
                    Boolean result = SendMessage(QueueConstants.MESSAGEQUEUE, messagingContract.QueueType, formattedMessage, messagingContract.ApplicationDatabaseName, messagingContract.CurrentUserId, messagingContract.Subject, out adbMessageID);
                    messagingContract.MessageId = adbMessageID;
                    return result;
                case MessagingAction.Reply:
                case MessagingAction.ReplyAll:
                    return ReplyMesssage(QueueConstants.MESSAGEQUEUE, messagingContract.MessageId, messagingContract.QueueType, formattedMessage, messagingContract.ApplicationDatabaseName, messagingContract.CurrentUserId, messagingContract.Subject);
                default:
                    return false;
            }

            //_hostFactory = new QueueServiceHostFactory("unity");
            //return _hostFactory.SendMessage(messagingContract);
        }

        /// <summary>
        /// Invoked to format the message
        /// </summary>
        /// <param name="parentMessageId"></param>
        /// <param name="isNewMailChain"></param>
        /// <param name="messageMode"></param>
        /// <param name="toUsers"></param>
        /// <param name="ccUsers"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        private static String FormatMessage(Entity.MessagingContract messagingContract)
        {
            Int32 isNewMail = messagingContract.Action == MessagingAction.NewMail || messagingContract.Action == MessagingAction.Forward ? 1 : 0;

            String messageId = String.Empty;
            String parentId = String.Empty;
            Int32 tenantType = 1;

            if (messagingContract.MessageId.ToString().Length > AppConsts.NONE)
                parentId = messagingContract.MessageId.ToString();

            if (messagingContract.Action.Equals(MessagingAction.Draft))
                messageId = messagingContract.MessageId.ToString();
            else
                messageId = Guid.NewGuid().ToString();


            /* INITIAL PHASE
            foreach (var item in messagingContract.TenantTypes)
            {
                tenantType = item.Key;
                break;
            }*/

            String highImportance = messagingContract.IsHighImportance ? "1" : "0";
            // String from = MessageManager.GetEmailId(currentUserId);
           // String messageBody = SysXUtils.GetXmlEncodedString(messagingContract.Content);
            String messageBody =(messagingContract.Content);
            //MessageBody(messagingContract.MessageMode, messagingContract.From, messagingContract.ToList, messagingContract.CcList, messagingContract.Subject, messagingContract.Content, Convert.ToInt32(messagingContract.Action));

            StringBuilder message = new StringBuilder();
            message.Append(@"<Message xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='http://tempuri.org/XMLSchema.xsd'> ");
            message.Append("<MessageHeader><X-Mailer>anyType</X-Mailer><Identifier /><MessageId>" + messageId + "</MessageId><ParentMessageId>" + parentId + "</ParentMessageId>");
            message.Append("<FromTenantType>" + messagingContract.QueueType.ToString() + "</FromTenantType>");
            message.Append("<ToTenantType>" + tenantType.ToString() + "</ToTenantType>");
            message.Append("<ToMessageType>" + messagingContract.MessageType.ToString() + "</ToMessageType>");
            message.Append("<ToUserGroup>" + messagingContract.ToUserGroupIds + "</ToUserGroup>");
            message.Append("<From>" + messagingContract.From + "</From>");
            message.Append("<ToUserList>" + messagingContract.ToList + "</ToUserList>");
            message.Append("<CCUserList>" + messagingContract.CcList + "</CCUserList>");
            message.Append("<BCCUserList>" + messagingContract.BCcList + "</BCCUserList>");
            message.Append("<MessageMode>" + messagingContract.MessageMode + "</MessageMode>");
            message.Append("<CommunicationType>" + messagingContract.CommunicationType + "</CommunicationType>");
            message.Append("<FolderId>" + messagingContract.FolderId.ToString() + "</FolderId>");
            message.Append("<DocumentId>" + messagingContract.DocumentID + "</DocumentId>");
            message.Append("<DocumentName>" + messagingContract.DocumentName + "</DocumentName>");
            message.Append("<OriginalDocumentName>" + messagingContract.OriginalDocumentName + "</OriginalDocumentName>");
            message.Append("<EVaultDocumentID>" + messagingContract.EVaultDocumentID + "</EVaultDocumentID>");
            message.Append("<IsHighImportance>" + highImportance + "</IsHighImportance>");
            message.Append("<MessageDate>" + DateTime.Now.ToString("MM/dd/yyyy H:mm:ss") + " </MessageDate><MessageSubject>" +"<![CDATA[" + messagingContract.Subject.Trim()+ " ]]>" + "</MessageSubject>");
            message.Append("<IsNewMailChain>" + isNewMail.ToString() + "</IsNewMailChain>");
            message.Append("<CCReceipients>" + messagingContract.CCUserGroupIds + "</CCReceipients>");
            message.Append("<BCCUserReceipients>" + messagingContract.BccUserIds + "</BCCUserReceipients>");
            message.Append("<CCUserReceipients>" + messagingContract.CcUserIds + "</CCUserReceipients>");
            message.Append("<MessageReceipients>" + messagingContract.ToUserIds + "</MessageReceipients><MessageSender>" + messagingContract.CurrentUserId.ToString() + "</MessageSender><MessageReplyUserGroup>anyType</MessageReplyUserGroup></MessageHeader>");
            message.Append("<MessageContent><MessageContentMetadata><MessageContentType>HTML</MessageContentType></MessageContentMetadata>");
            message.Append("<MessageBody><![CDATA[" + messageBody + " ]]></MessageBody>");
            message.Append("<MessageSize>" + Convert.ToString(messageBody.Length * 2) + "</MessageSize>");
            message.Append("<MessageSignature>MessageSignature1</MessageSignature></MessageContent>");
            message.Append("<MessageAttachment><MessageAttachmentStructure><Identifier /><MessageAttachmentFileName>MessageAttachmentFileName1</MessageAttachmentFileName>");
            message.Append("<MessageAttachmentExtension>MessageAttachmentExtension1</MessageAttachmentExtension><MessageAttachmentPhysicalPath>MessageAttachmentPhysicalPath1</MessageAttachmentPhysicalPath>");
            message.Append("</MessageAttachmentStructure><MessageAttachmentRelativePath>MessageAttachmentRelativePath1</MessageAttachmentRelativePath>");
            message.Append("</MessageAttachment><MessageStatus><MessageStatusOptions>ResponseRequired</MessageStatusOptions></MessageStatus><MessageState><MessageStateOptions>ArchivedNotInNotes</MessageStateOptions>");
            message.Append("</MessageState><MessageReplies><Message /></MessageReplies><MessageTree><Identifier /><Identifier /></MessageTree></Message>");

            return message.ToString();
        }



        /// <summary>
        /// Invoked to get the message body.
        /// </summary>
        /// <param name="messageMode"></param>
        /// <returns></returns>
        private static String MessageBody(String messageMode, String fromEmailId, String toUsers, String ccUsers, String subject, String content, Int32 messageAction)
        {
            StringBuilder messageContent = new StringBuilder();

            if (messageMode.Equals(MessageMode.DRAFTMESSAGE))
                messageContent.Append(content);
            else
            {

                if (messageAction == Convert.ToInt32(MessagingAction.Reply) || messageAction == Convert.ToInt32(MessagingAction.ReplyAll) || messageAction == Convert.ToInt32(MessagingAction.Forward))
                {
                    messageContent.Append("<div class='previous'>");
                }

                messageContent.Append("<div class='header'><div class='subject'>" + subject + "</div><div class='senders'>" + fromEmailId + "</div>");
                messageContent.Append("<div class='date'><span>Sent:&nbsp;</span>" + DateTime.Now.ToString("MM/dd/yyyy H:mm:ss") + "</div>");
                messageContent.Append("<div class='receivers'><span>To:&nbsp;</span>" + toUsers + "</div>");
                messageContent.Append("<div class='copies'><span>CC:&nbsp;</span>" + ccUsers + "</div></div>");
                messageContent.Append("<div class='message'>" + content + "</div>");

                if (messageAction == Convert.ToInt32(MessagingAction.Reply) || messageAction == Convert.ToInt32(MessagingAction.ReplyAll) || messageAction == Convert.ToInt32(MessagingAction.Forward))
                {
                    messageContent.Append("</div>");
                }
            }
            return messageContent.ToString();
        }

        public Boolean SendMessagesInBatch(String queueName, Int32 initiator, String[] message)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.SendMessageInBatch(queueName, initiator, message);
        }

        public Boolean ReplyMesssage(String queueName, Guid messageId, Int32 initiator, String message, String applicationDatabaseName, Int32 senderId, String subject)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.ReplyMesssage(queueName, messageId, initiator, message, applicationDatabaseName, senderId, subject);
        }

        /// <summary>
        /// Use to get a message by  messageID
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="folderId"></param>
        /// <param name="queueType"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public EntityObject GetMessage(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, Guid messageId)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.GetMessage(queueName, folderId, queueType, queueOwnerId, messageId);
        }

        /// <summary>
        /// Use to get all messages in a folder
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="folderId"></param>
        /// <param name="queueType"></param>
        /// <param name="queueOwnerId"></param>
        /// <returns></returns>
        public IEnumerable<EntityObject> GetAllMessages(String queueName, Int32 folderId, String folderCode, Int32 queueType, Int32 queueOwnerId, Int32 userGroup)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            // return _hostFactory.GetQueue(queueName, folderId, folderCode, queueType, queueOwnerId, userGroup);
            return null;
        }

        /// <summary>
        /// Use to get new messages after certain datetime
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="folderId"></param>
        /// <param name="queueType"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="lastDateTime"></param>
        /// <returns></returns>
        public IEnumerable<EntityObject> GetNewMessages(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, DateTime lastDateTime)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.GetUpdateQueue(queueName, folderId, queueType, queueOwnerId, lastDateTime);
        }


        public IEnumerable<EntityObject> GetQueue(String queueName, Int32 folderId, String folderCode, Int32 queueType, Int32 queueOwnerId, Int32 userGroup, Int32 communicationTypeId)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.GetQueue(queueName, folderId, folderCode, queueType, queueOwnerId, userGroup, communicationTypeId);
            //  return null;
        }

        public IEnumerable<EntityObject> GetQueue(String queueName, Int32 queueOwnerId, Int32 communicationTypeId)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.GetQueue(queueName, queueOwnerId, communicationTypeId);
            //  return null;
        }

        public IEnumerable<EntityObject> GetUpdateQueue(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, DateTime lastDateTime)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.GetUpdateQueue(queueName, folderId, queueType, queueOwnerId, lastDateTime);
        }

        #endregion
        
        public IEnumerable<EntityObject> GetAllMessages(string queueName, int folderId, int queueType, int queueOwnerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EntityObject> GetQueue(string queueName, int folderId, int queueType, int queueOwnerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EntityObject> GetRecentMessages(String queueName, String folderCode, Int32 queueOwnerId, Int32 communicationTypeId)
        {
            _hostFactory = new QueueServiceHostFactory("unity");
            return _hostFactory.GetRecentMessages(queueName, folderCode, queueOwnerId, communicationTypeId);
        }
    }
}