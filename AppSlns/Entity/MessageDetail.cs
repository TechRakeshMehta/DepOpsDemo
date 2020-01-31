#region Header Comment Block
// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename: MessageDetail.cs
// Purpose:   Message Detail Class
//
// Revisions:
// Comment
// -------------------------------------------------
// Added enhancement changes and code review.

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects.DataClasses;
#endregion

#region Application Specific
using INTSOF.Utils;

#endregion

#endregion

namespace Entity
{
    /// <summary>
    /// Message Detail class for implementing the messages functionality.
    /// </summary>
    [Serializable]
    public class MessageDetail
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private Guid _messageDetailId;
        private Boolean _isNew;
        private String _from;
        private String _to;
        private String _subject;
        private Int32 _size;
        private String _sizeIn;
        private Boolean _flagged;
        private DateTime _receivedDate;
        private String _receivedDateString;
        private Boolean _isUnread;
        private Boolean _isFollowup;
        private Boolean _isHighImportant;
        private Int32 _totalRecords;
        private String _cC;
        private String _communicationType;
        private String _communicationTypeCode;
        private String _receivedDateFormat;
        private Int32 _fromUserId;
        private String _senderUser;
        private String _messageBody;
        #endregion

        #endregion

        #region Properties
        #region Public Properties
        public Guid MessageDetailID { get { return _messageDetailId; } set { _messageDetailId = value; } }
        public Boolean IsNew { get { return _isNew; } set { _isNew = value; } }
        public String From { get { return _from; } set { _from = value; } }
        public String To { get { return _to; } set { _to = value; } }
        public String Subject { get { return _subject; } set { _subject = value; } }
        public Int32 Size { get { return _size; } set { _size = value; } }
        public String SizeIn { get { return _sizeIn; } set { _sizeIn = value; } }
        public Boolean Flagged { get { return _flagged; } set { _flagged = value; } }
        public DateTime ReceivedDate { get { return _receivedDate; } set { _receivedDate = value; } }
        public String ReceivedDateString { get { return _receivedDateString; } set { _receivedDateString = value; } }
        public Boolean IsUnread { get { return _isUnread; } set { _isUnread = value; } }
        public Boolean IsFollowUp { get { return _isFollowup; } set { _isFollowup = value; } }
        public Boolean IsHighImportant { get { return _isHighImportant; } set { _isHighImportant = value; } }
        public Int32 TotalRecords { get { return _totalRecords; } set { _totalRecords = value; } }
        public String Cc { get { return _cC; } set { _cC = value; } }
        public String CommunicationType { get { return _communicationType; } set { _communicationType = value; } }
        public String CommunicationTypeCode { get { return _communicationTypeCode; } set { _communicationTypeCode = value; } }
        public Boolean HasAttachment { get { return _isNew; } set { _isNew = value; } }
        public String ReceivedDateFormat { get { return _receivedDateFormat; } set { _receivedDateFormat = value; } }
        public Int32 FromUserId { get { return _fromUserId; } set { _fromUserId = value; } }
        public String SenderUser { get { return _senderUser; } set { _senderUser = value; } }
        public String MessageBody { get { return _messageBody; } set { _messageBody = value; } }
        #endregion

        #region Private Properties
        #endregion
        #endregion

        /// <summary>
        /// Invoked to make reply message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toList"></param>
        /// <param name="ccList"></param>
        /// <param name="subject"></param>
        /// <param name="currentUserId"></param>
        /// <param name="isReplyAll"></param>
        /// <param name="content"></param>
        public static void MakeReplyMessageHistoryContent(EntityObject message, List<Int32> toList, List<Int32> ccList, out String messageSubject, Int32 currentUserId, MessagingAction messagingAction, StringBuilder messageContent, out Int32 tenantType, out Int32 messageType, out String fromUser, out Dictionary<String, String> dicAttachmentDocuments, Boolean isRowDoubleClick = false)
        {
            dicAttachmentDocuments = new Dictionary<String, String>();
            messageSubject = String.Empty;
            ADBMessage messageDetails = (message as ADBMessage);

            //List<ADBMessage> lst = messageDetails.ADBMessage1.Where(x => x.ADBMessageId != messageDetails.ADBMessageId && x.ConversationHandleId == messageDetails.ConversationHandleId).ToList();

            if (!String.IsNullOrEmpty(messageDetails.Message.GetNodeContent("ToUserGroup")) || !String.IsNullOrEmpty(messageDetails.Message.GetNodeContent("CCReceipients")))
            {
                toList.Add(Convert.ToInt32(messageDetails.From));
            }
            else
            {
                ADBMessageToList messagelist = messageDetails.ADBMessageToLists.FirstOrDefault(obj => obj.ADBMessageID == messageDetails.ADBMessageID);
                if (!messagelist.IsNull())
                {
                    if (!toList.IsNull())
                    {
                        toList.Add(Convert.ToInt32(messageDetails.From));
                        if (messagingAction == MessagingAction.ReplyAll)
                        {
                            foreach (ADBMessageToList messageTo in messageDetails.ADBMessageToLists.Where(x=>!x.IsBcc.Value))
                            {
                                if (messageTo.EntityID.HasValue && messageTo.EntityID.Value != currentUserId)
                                {
                                    if (messageTo.IsCC.Value)
                                    {
                                        ccList.Add(messageTo.EntityID.Value);
                                    }
                                    else
                                    {
                                        toList.Add(messageTo.EntityID.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            String toUsers = messageDetails.Message.GetNodeContent("ToUserList");
            String ccUsers = messageDetails.Message.GetNodeContent("CCUserList");
            fromUser = messageDetails.Message.GetNodeContent("From");
            messageSubject = messageDetails.Subject;

            if (messagingAction == MessagingAction.ReplyAll || messagingAction == MessagingAction.Reply || messagingAction == MessagingAction.Forward)
            {
                messageContent.Append("<br /><br />");
                messageContent.Append("<div style='border-bottom: 1px solid #999999;margin-bottom:10px'></div>");
                messageContent.Append("<div><div>" + SysXUtils.GetXmlDecodedString(messageSubject) + "</div><div>" + fromUser + "</div>");
                messageContent.Append("<div><span>Sent:&nbsp;</span>" + messageDetails.ReceiveDate + "</div>");
                messageContent.Append("<div><span>To:&nbsp;</span>" + toUsers + "</div>");
                messageContent.Append("<div><span>CC:&nbsp;</span>" + ccUsers + "</div></div>");
                messageContent.Append("<div>" + SysXUtils.GetXmlDecodedString(messageDetails.Message.GetMessageBody()) + "</div>");
            }
            else
            {  // Format the content for the Preview window
                messageContent.Append("<div class='header'><div class='subject'>" + SysXUtils.GetXmlDecodedString(messageSubject) + "</div><div class='senders'>" + fromUser + "</div>");
                messageContent.Append("<div class='date'><span>Sent:&nbsp;</span>" + messageDetails.ReceiveDate + "</div>");
                messageContent.Append("<div class='receivers'><span>To:&nbsp;</span>" + toUsers + "</div>");
                messageContent.Append("<div class='copies'><span>CC:&nbsp;</span>" + ccUsers + "</div></div>");
                messageContent.Append("<div class='message'>" + SysXUtils.GetXmlDecodedString(messageDetails.Message.GetMessageBody()) + "</div>");
            }
            tenantType = Convert.ToInt32(messageDetails.Message.GetNodeContent("FromTenantType"));
            messageType = Convert.ToInt32(messageDetails.Message.GetNodeContent("ToMessageType"));
        }


        /// <summary>
        /// Invoked to make messagehistory content
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toList"></param>
        /// <param name="ccList"></param>
        /// <param name="subject"></param>
        /// <param name="currentUserId"></param>
        /// <param name="content"></param>
        public static void MakeADBMessageHistoryContent(EntityObject message, List<Int32> toList, List<Int32> ccList, out String subject, Int32 currentUserId, StringBuilder content, out Int32 tenantType, out Int32 messageType, out String toUserList, out String ccUsersList, out Boolean isUserGroup)
        {

            subject = String.Empty;
            ADBMessage item = (message as ADBMessage);
            String toUserGroup = item.Message.GetNodeContent("ToUserGroup");
            if (!toUserGroup.IsNullOrEmpty())
            {
                toList.AddRange(toUserGroup.Split(';').Select(str => Int32.Parse(str)).ToList());
                String ccUserGroup = item.Message.GetNodeContent("CCReceipients");
                if (!ccUserGroup.IsNullOrEmpty())
                {
                    ccList.AddRange(ccUserGroup.Split(';').Select(str => Int32.Parse(str)).ToList());
                }
                isUserGroup = true;
            }
            else
            {
                if (!toList.IsNull())
                {
                    foreach (ADBMessageToList messageTo in item.ADBMessageToLists)
                    {
                        if (messageTo.EntityID.HasValue)
                        {
                            if (messageTo.IsCC.Value)
                            {
                                ccList.Add(messageTo.EntityID.Value);
                            }
                            else
                            {
                                toList.Add(messageTo.EntityID.Value);
                            }
                        }
                    }

                }
                isUserGroup = false;
            }
            subject = item.Subject;
            content.Append(SysXUtils.GetXmlDecodedString(item.Message.GetMessageBody()));
            content.Append("<br /><div class='sxseparator'></div>");
            tenantType = Convert.ToInt32(item.Message.GetNodeContent("FromTenantType"));
            messageType = Convert.ToInt32(item.Message.GetNodeContent("ToMessageType"));
            toUserList = item.Message.GetNodeContent("ToUserList");
            ccUsersList = item.Message.GetNodeContent("CCUserList");
        }

        /// <summary>
        /// Invoked to make reply message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toList"></param>
        /// <param name="ccList"></param>
        /// <param name="subject"></param>
        /// <param name="currentUserId"></param>
        /// <param name="isReplyAll"></param>
        /// <param name="content"></param>
        public static void MakeADBTemplateMessageHistoryContent(EntityObject message, List<Int32> toList, List<Int32> ccList, out String templateName, out String subject, Int32 currentUserId, StringBuilder content, out Int32 tenantType, out Int32 messageType, out String toUserList, out String ccUserList, out String toUserListid, out String ccUserListid)
        {
            //content.Append("<br/> ");
            //content.Append("<br/> ");
            subject = String.Empty;
            ADBMessage item = (message as ADBMessage);
            ADBMessageToList messagelist = item.ADBMessageToLists.FirstOrDefault(obj => obj.ADBMessageID == item.ADBMessageID);
            if (!messagelist.IsNull())
            {
                foreach (ADBMessageToList messageTo in item.ADBMessageToLists)
                {
                    if (messageTo.EntityID.HasValue && messageTo.EntityID.Value != currentUserId)
                    {
                        if (messageTo.IsCC.Value)
                        {
                            ccList.Add(messageTo.EntityID.Value);
                        }
                        else
                        {
                            toList.Add(messageTo.EntityID.Value);
                        }
                    }
                }
            }
            templateName = item.TemplateName;
            subject = item.Subject;
            content.Append(item.Message.GetMessageBody());
            // content.Append("<br/><div class='sxseparator'></div>");
            tenantType = Convert.ToInt32(item.Message.GetNodeContent("ToTenantType"));
            messageType = Convert.ToInt32(item.Message.GetNodeContent("ToMessageType"));
            toUserList = item.Message.GetNodeContent("ToUserList");
            ccUserList = item.Message.GetNodeContent("CcUserList");

            //toUserListid = item.Message.GetNodeContent("ToIds");
            //ccUserListid = item.Message.GetNodeContent("CcIds");

            toUserListid = item.Message.GetNodeContent("MessageReceipients");
            ccUserListid = item.Message.GetNodeContent("CCUserReceipients");

        }
    }
}

