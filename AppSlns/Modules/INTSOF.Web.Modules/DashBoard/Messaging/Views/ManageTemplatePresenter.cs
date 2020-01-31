using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Configuration;

namespace CoreWeb.Messaging.Views
{
    public class ManageTemplatePresenter : Presenter<IManageTemplateView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties
        #region Public Properties
        #endregion

        #region Private Properties

        #endregion
        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads

            GetTenantTypes();
            //BindCommunicationTypes(View.CurrentUserId);
            BindTemplates();
        }

        public void BindTemplates()
        {
            View.CompanyTemplates = MessageManager.GetTemplates(View.CurrentOrganizationUserId).OfType<ADBMessage>(); ;
        }

        /// <summary>
        /// Invoked to send message
        /// </summary>
        /// <param name="messageMode"></param>
        public Boolean SendMessage(String messageMode)
        {
            Guid adbMessageId = new Guid();
            View.TemplateName = MessageManager.GetUniqueTemplateName(adbMessageId, View.CurrentOrganizationUserId, View.TemplateName);
            String newMessage = FormatMessage(adbMessageId, 1, messageMode);

            String databaseName = MessageManager.GetDatabaseName(ConfigurationManager.ConnectionStrings[AppConsts.APPLICATION_CONNECTION_STRING].ConnectionString);
            return MessageManager.SendMessage(QueueConstants.MESSAGEQUEUE, View.QueueType, newMessage, databaseName, Convert.ToInt32(View.CurrentOrganizationUserId), View.Subject, out adbMessageId);
        }

        public Boolean UpdateMessage(string messageMode)
        {
            Guid adbMessageId = new Guid(View.ADBMessageId);
            ADBMessage message = MessageManager.GetMesssage(adbMessageId);
            if (message != null)
            {
                message.TemplateName = View.TemplateName = MessageManager.GetUniqueTemplateName(adbMessageId, View.CurrentOrganizationUserId, View.TemplateName);
                message.Message = FormatMessage(adbMessageId, 1, messageMode);
                message.Subject = View.Subject;
                message.CommunicationTypeID = View.CommunicationTypeId;
                return MessageManager.UpdateMessage(message);
            }
            return true;
        }

        public string GetUniqueTemplateName()
        {
            return MessageManager.GetUniqueTemplateName(new Guid(View.ADBMessageId), View.CurrentOrganizationUserId, View.TemplateName);
        }


        /// <summary>
        /// Invoked to get the Communication Types.
        /// </summary>
        //public void BindCommunicationTypes(Guid userId)
        //{
        //    View.CommunicationTypeList = MessageManager.GetCommuncationTypes(userId);
        //}


        /// <summary>
        /// Get Messgae Type
        /// </summary>
        public void GetTenantTypes()
        {
            Dictionary<Int32, String> tenantTypes = new Dictionary<Int32, String>();
            {
                switch ((TenantTypeEnum)View.QueueType)
                {
                    case TenantTypeEnum.Client:
                        tenantTypes.Clear();
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Company), "Company");
                        View.TenantTypes = tenantTypes;
                        break;

                    case TenantTypeEnum.Company:
                        tenantTypes.Clear();
                        tenantTypes.Add(0, "--SELECT--");
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Client), "Client");
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Company), "Company");
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Supplier), "Supplier");
                        View.TenantTypes = tenantTypes;
                        break;

                    case TenantTypeEnum.Supplier:
                        tenantTypes.Clear();
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Company), "Company");
                        View.TenantTypes = tenantTypes;
                        break;
                }

            }
        }

        /// <summary>
        /// Get Template Message.
        /// </summary>
        /// <param name="MessageId"></param>
        public void GetTemplateMessage(string messageId)
        {
            if (!string.IsNullOrEmpty(messageId))
            {
                EntityObject obj;

                if (View.IsApplicant)
                    obj = MessageManager.GetMessageContentsForApplicant(View.QueueType, new Guid(messageId));
                else
                    obj = MessageManager.GetMessageContents(View.QueueType, new Guid(messageId));

                MapMessageXMLToViewProperties(obj);
            }
            else
            {
                View.TemplateName 
                    = View.Subject 
                    = View.Content 
                    = View.ToListGroupIds 
                    = View.CcListGroupIds 
                    = View.ToListIds 
                    = View.CcListIds 
                    = View.BccListIds= string.Empty;
            }
        }


       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public lkpCommunicationType GetCommuncationTypeByCode(string code)
        {
            return MessageManager.GetCommuncationTypeByCode(code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public lkpCommunicationType GetCommuncationTypeById(Int32 communcationTypeId)
        {
            return MessageManager.GetCommuncationTypeById(communcationTypeId);
        }


        /// <summary>
        /// 
        /// </summary>
        public void Delete()
        {
            MessageManager.DeleteMesssage(new Guid(View.ADBMessageId));
            GetTemplateMessage(string.Empty);
        }


        #endregion

        #region Private Methods

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
        private String FormatMessage(Guid parentMessageId, Int32 isNewMailChain, String messageMode)
        {

            String parentId = String.Empty;
            if (parentMessageId.ToString().Length > AppConsts.NONE)
            {
                parentId = parentMessageId.ToString();
            }

            string messageId = string.IsNullOrEmpty(View.ADBMessageId) ? Guid.NewGuid().ToString() : View.ADBMessageId;


            Int32 tenantType = 1;
            //foreach (var item in View.TenantTypes)
            //{
            //    tenantType = item.Key;
            //    break;
            //}

            String from = String.Empty;
            String toList = String.Empty;
            String toUserIds = String.Empty;
            String ccList = String.Empty;
            Int32 CurrentUserId = AppConsts.ONE;
            String ToUserGroupIds = String.Empty;



            UserGroup userGroup = null;
            if (TenantTypeEnum.Company.Equals((TenantTypeEnum)Convert.ToInt32(View.QueueType))
                &&
                tenantType.Equals(Convert.ToInt32(TenantTypeEnum.Supplier))
                )
            {
                toUserIds = String.Empty;
                toList = View.ToList;
                ToUserGroupIds = MessageManager.GetUserGroupIdBySupplierId(View.ToListIds.Split(';'), View.CommunicationTypeId);
                ccList = MessageManager.GetUserGroupIdBySupplierId(View.CcListIds.Split(';'), View.CommunicationTypeId);
                userGroup = MessageManager.GetUserGroupByCurrentUserIdAndMessageType(View.CurrentOrganizationUserId, View.CommunicationTypeId);
                if (!userGroup.IsNull())
                {
                    CurrentUserId = userGroup.UserGroupID;
                    from = userGroup.UserGroupName;
                }
                else
                {
                    CurrentUserId = View.CurrentOrganizationUserId;
                    from = MessageManager.GetEmailId(View.CurrentOrganizationUserId);
                }
            }
            else if (TenantTypeEnum.Company.Equals((TenantTypeEnum)Convert.ToInt32(View.QueueType))
                &&
                tenantType.Equals(Convert.ToInt32(TenantTypeEnum.Client)))
            {
                toUserIds = String.Empty;
                toList = View.ToList;
                //ToUserGroupIds = MessageManager.GetUserGroupIdByClientId(View.ToListIds.Split(';'), View.MessageType);
                //ccList = MessageManager.GetUserGroupIdByClientId(View.CcListIds.Split(';'), View.MessageType);
                userGroup = MessageManager.GetUserGroupByCurrentUserIdAndMessageType(View.CurrentOrganizationUserId, View.CommunicationTypeId);
                if (!userGroup.IsNull())
                {
                    CurrentUserId = userGroup.UserGroupID;
                    from = userGroup.UserGroupName;
                }
                else
                {
                    CurrentUserId = View.CurrentOrganizationUserId;
                    from = MessageManager.GetEmailId(View.CurrentOrganizationUserId);
                }
            }
            else if (TenantTypeEnum.Company.Equals((TenantTypeEnum)Convert.ToInt32(View.QueueType))
           &&
           tenantType.Equals(Convert.ToInt32(TenantTypeEnum.Company)))
            {
                if (View.IsUserGroup.Equals("true"))
                {
                    toUserIds = String.Empty;
                    toList = View.ToList;
                    ToUserGroupIds = View.ToListIds;
                    ccList = View.CcListIds;

                }
                else if (View.IsUserGroup.Equals("false"))
                {
                    toUserIds = MessageManager.GetUserIDListByEmployeeIds(View.ToListIds.Split(';'));
                    toList = View.ToList;
                    ToUserGroupIds = String.Empty;
                    ccList = MessageManager.GetUserIDListByEmployeeIds(View.CcListIds.Split(';'));

                }
                else
                {
                    toUserIds = MessageManager.GetUserIDList(View.ToList.Split(';'));
                    toList = View.ToList;
                    ToUserGroupIds = MessageManager.GetUserGroupIdList(View.ToListIds.Split(';'), View.TenantTypes, View.CommunicationTypeId);

                }
                CurrentUserId = View.CurrentOrganizationUserId;
                from = MessageManager.GetEmailId(View.CurrentOrganizationUserId);
            }
            else
            {
                //toUserIds = String.Empty;
                //toList = View.ToList;

                //ToUserGroupIds = View.ToListIds;
                //ccList = View.CcListIds;

                CurrentUserId = View.CurrentOrganizationUserId;
                from = MessageManager.GetEmailId(View.CurrentOrganizationUserId);
            }

            String CcListName = View.CCList;

            String subject = View.Subject;
            String templateName = View.TemplateName;
            String messageType = View.CommunicationTypeId.ToString();

            String messageBody = View.Content;
            //    MessageBody(messageMode);

            StringBuilder message = new StringBuilder();

            message.Append(@"<Message xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='http://tempuri.org/XMLSchema.xsd'> ");
            message.Append("<MessageHeader><X-Mailer>anyType</X-Mailer><Identifier /><MessageId>" + messageId + "</MessageId><ParentMessageId>" + parentId + "</ParentMessageId>");
            message.Append("<FromTenantType>" + View.QueueType.ToString() + "</FromTenantType>");
            message.Append("<ToTenantType>" + tenantType.ToString() + "</ToTenantType>");
            message.Append("<ToMessageType>" + messageType + "</ToMessageType>");
            message.Append("<ToUserGroup>" + View.ToListGroupIds + "</ToUserGroup>");
            message.Append("<ToIds></ToIds>");
            message.Append("<CcIds></CcIds>");
            message.Append("<From>" + from + "</From>");
            message.Append("<ToUserList>" + toList + "</ToUserList>");
            message.Append("<CcUserList>" + CcListName + "</CcUserList>");
            message.Append("<BCCUserList>" +View.BCCList + "</BCCUserList>");
            message.Append("<MessageMode>" + messageMode + "</MessageMode>");
            message.Append("<CommunicationType>" + View.CommunicationTypeCode + "</CommunicationType>");
            message.Append("<FolderId>" + "1" + "</FolderId>");
            message.Append("<IsHighImportance>" + "0" + "</IsHighImportance>");
            message.Append("<MessageDate>" + DateTime.Now.ToString("MM/dd/yyyy H:mm:ss") + " </MessageDate><MessageSubject>" + subject.Trim() + "</MessageSubject>");
            message.Append("<TemplateName>" + templateName.Trim() + "</TemplateName>");
            message.Append("<IsNewMailChain>" + isNewMailChain.ToString() + "</IsNewMailChain>");
            message.Append("<CCReceipients>" + View.CcListGroupIds + "</CCReceipients>");
            message.Append("<CCUserReceipients>" + View.CcListIds + "</CCUserReceipients>");
            message.Append("<BCCUserReceipients>" + View.BccListIds + "</BCCUserReceipients>");
            message.Append("<MessageReceipients>" + View.ToListIds + "</MessageReceipients><MessageSender>" + View.CurrentOrganizationUserId.ToString() + "</MessageSender><MessageReplyUserGroup>anyType</MessageReplyUserGroup></MessageHeader>");
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
        private String MessageBody(String messageMode)
        {

            String content = String.Empty;
            content = View.Content;
            StringBuilder mail = new StringBuilder();
            if (messageMode.Equals("T"))
            {
                //COMMENTED TO REMOVE THE EXTRA BREAK TAG WHEN CONTENT IS LOADED IN THE TEXT EDITOR
                //mail.Append("<br/> " + content);
                mail.Append(content);
            }
            else
            {
                mail.Append("<strong>To : </strong>" + View.ToList);
                mail.Append("<br/><strong>CC : </strong>" + View.CCList);
                mail.Append("<br/><strong>Subject : </strong>" + View.Subject);
                mail.Append("<br/><strong>Message Type : </strong>" + View.CommunicationTypeId);
                mail.Append("<br/><strong>Receiver Type : </strong>" + View.TenantTypes);
                mail.Append("<br/><div class='sxseparator'></div>");
                mail.Append("<br/> " + content);
                mail.Append("<br/><div class='sxseparator'></div>");
            }
            return mail.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsWithSemicolonSeparated"></param>
        /// <returns></returns>
        private string FormatGroupIds(string idsWithSemicolonSeparated)
        {
            if (!string.IsNullOrEmpty(idsWithSemicolonSeparated))
            {
                StringBuilder sb = new StringBuilder();
                String[] arrIds = idsWithSemicolonSeparated.Split(';');
                List<Int32> intParseIds = arrIds.Where(x => !String.IsNullOrEmpty(x.Trim())).Select(x => Int32.Parse(x)).ToList();
                IEnumerable<MessagingGroup> messagingGroups = MessageManager.GetMessagingGroups(intParseIds);

                foreach (MessagingGroup messagingGroup in messagingGroups)
                    sb.Append(string.Format("{0}:{1}", messagingGroup.GroupName, messagingGroup.MessagingGroupID));

                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private string FormatUserIds(string userIds)
        {
            if (!string.IsNullOrEmpty(userIds))
            {
                String[] arrToList = userIds.Split(';');
                return MessageManager.GetUserNamesIdsString(arrToList.Where(x => !String.IsNullOrEmpty(x.Trim())).Select(x => Int32.Parse(x)).ToList());
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void MapMessageXMLToViewProperties(EntityObject message)
        {
            ADBMessage item = (message as ADBMessage);
            View.TemplateName = item.TemplateName;
            View.Subject = item.Subject;
           // View.CommunicationTypeId = Convert.ToInt32(item.Message.GetNodeContent("ToMessageType"));
            View.Content = item.Message.GetMessageBody();

            if (View.IsApplicant)
            {
                View.ToListGroupIds = FormatGroupIds(item.Message.GetNodeContent("ToUserGroup"));
                View.CcListGroupIds = FormatGroupIds(item.Message.GetNodeContent("CCReceipients"));
            }
            else
            {
                View.ToListIds = FormatUserIds(item.Message.GetNodeContent("MessageReceipients"));
                View.CcListIds = FormatUserIds(item.Message.GetNodeContent("CCUserReceipients"));
                View.BccListIds = FormatUserIds(item.Message.GetNodeContent("BCCUserReceipients"));
            }
        }




        #endregion

        #endregion
    }
}




