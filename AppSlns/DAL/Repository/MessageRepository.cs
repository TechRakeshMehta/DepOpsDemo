
#region Header Comment Block

// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MessageRepository.cs
// Purpose:   This file is for CRUD operations from Database
//
// Revisions:
// Comment
// -----------------------------------------
// Added enhancement changes.
// Added enhancement changes.

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Text;
using System.Data.Entity.Core.Objects;
#endregion

#region Application Specific
using Entity;
using DAL.Interfaces;
using INTSOF.Utils;
//using INTSOF.Queues.Interfaces;
using System.Collections;
using System.Data.Entity.Core.Metadata.Edm;
using System.Net.Mail;
using System.Configuration;
using INTSOF.Utils.SPI.Email;

#endregion

#endregion

namespace DAL.Repository
{
    /// <summary>
    /// Repository for messaging
    /// </summary>
    public class MessageRepository : BaseQueueRepository, IMessageRepository, INTSOF.UnityHelper.Interfaces.IMessageQueue
    {

        #region constructer

        public MessageRepository()
        {

        }

        #endregion

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

        private ADBMessageDB_DevEntities ADBQueueContext
        {
            get { return base.ADB_MessageQueueContext; }
        }

        private ADBApplicantMessageDB_DevEntities ApplicantQueueContext
        {
            get { return base.ApplicantQueueContext; }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// function to get list of emailid .
        /// </summary>
        /// <param name="userIdList">list of user</param>
        /// <returns></returns>
        public List<String> GetEmailId(List<Int32> userIdList)
        {
            var users = AppDBContext.OrganizationUsers.Include(SysXEntityConstants.TABLE_MESSAGING_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP).Where(obj => userIdList.Contains(obj.OrganizationUserID)).Distinct();

            List<String> emailIds = new List<String>();
            foreach (OrganizationUser user in users)
            {
                if (!emailIds.Contains(user.aspnet_Users.aspnet_Membership.Email))
                    emailIds.Add(user.aspnet_Users.aspnet_Membership.Email);
            }

            return emailIds;
        }

        /// <summary>
        /// function to get emailid .
        /// </summary>
        /// <param name="userIdList">list of user</param>
        /// <returns></returns>
        public String GetEmailId(Int32 userId)
        {
            var users = AppDBContext.OrganizationUsers.Include(SysXEntityConstants.TABLE_MESSAGING_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP).FirstOrDefault(obj => userId.Equals(obj.OrganizationUserID));
            return users.aspnet_Users.aspnet_Membership.Email;
        }

        /// <summary>
        /// Return comma separated user id on the basis of email id in the list
        /// </summary>
        /// <param name="emailList">list of email id</param>
        /// <returns></returns>
        public String GetUserIDList(String[] emailList)
        {
            StringBuilder toList = new StringBuilder();

            if (!emailList[0].Equals(String.Empty))
            {
                var users = AppDBContext.OrganizationUsers.Where(obj => emailList.Contains(obj.aspnet_Users.aspnet_Membership.Email));

                Boolean skipSeparator = true;

                foreach (OrganizationUser user in users)
                {
                    if (skipSeparator == false)
                    {
                        toList.Append(";");
                    }
                    toList.Append(user.OrganizationUserID.ToString());
                    skipSeparator = false;
                }
            }
            return toList.ToString();
        }

        /// <summary>
        /// Return comma separated user id on the basis of email id in the list
        /// </summary>
        /// <param name="emailList">list of email id</param>
        /// <returns></returns>
        public String GetUserIDListByEmployeeIds(String[] employeeIds)
        {
            StringBuilder toList = new StringBuilder();
            if (!employeeIds[0].Equals(String.Empty))
            {
                Boolean skipSeparator = true;
                foreach (var employeeId in employeeIds)
                {
                    Int32 empid = Convert.ToInt32(employeeId);
                    if (skipSeparator == false)
                    {
                        toList.Append(";");
                    }

                    Employee employee = AppDBContext.Employees.Include("OrganizationUser").FirstOrDefault(emp => empid.Equals(emp.EmployeeID));
                    if (!employee.IsNull() && !employee.OrganizationUser.IsNull())
                    {
                        toList.Append(employee.OrganizationUser.OrganizationUserID);
                    }
                    skipSeparator = false;
                }
            }
            return toList.ToString();
        }


        /// <summary>
        /// Return comma separated usergroup id on the basis of email id in the list
        /// </summary>
        /// <param name="emailList">list of email id</param>
        /// <returns></returns>
        public String GetUserGroupIdList(String[] organizationusrIds, Boolean isClient, Int32 msgType)
        {
            return string.Empty;
        }


        /// <summary>
        /// Return comma separated usergroup id on the basis of email id in the list
        /// </summary>
        /// <param name="emailList">list of email id</param>
        /// <returns></returns>
        public String GetUserGroupIdBySupplierId(String[] supplierIds, Int32 msgType)
        {
            StringBuilder toList = new StringBuilder();

            Boolean skipSeparator = true;
            if (!supplierIds[0].Equals(String.Empty))
            {
                Int32[] supplierIDs = supplierIds.Select(str => Int32.Parse(str)).ToArray();

                List<UserGroupMessageType> UserGroupMessageTypes = AppDBContext.UserGroupMessageTypes.Where(usergroup => supplierIDs.Contains(usergroup.SupplierID.Value) && usergroup.MessageTypeID.Equals(msgType)).ToList();

                foreach (UserGroupMessageType userGroupMessageType in UserGroupMessageTypes)
                {
                    if (skipSeparator == false)
                    {
                        toList.Append(";");
                    }
                    toList.Append(userGroupMessageType.UserGroupID);
                    skipSeparator = false;
                }
                skipSeparator = false;
            }
            return toList.ToString();
        }

        public Boolean SetMoveToFolder(Guid messageId, Int32 userId, String folderCode, Int32 moveToFolderId, String moveToFolderCode, Int32 queueTypeID)
        {
            return SetADBMoveToFolder(messageId, userId, folderCode, moveToFolderId, moveToFolderCode);
        }

        /// <summary>
        /// Gets  message template
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public IQueryable<EntityObject> GetTemplates(Int32 currentUserId)
        {
            String messageMode = MessageMode.TEMPLATEMESSAGE.GetStringValue().ToLower();
            return ADBMessageQueueContext.ADBMessages.Where(obj =>
                obj.MessageMode.ToLower().Equals(messageMode)
                && obj.From == currentUserId
                && !obj.IsDeleted);
        }


        /// <summary>
        /// Invoked to get the message templates.
        /// </summary>
        /// <param name="queueType"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public IQueryable<EntityObject> GetTemplates(Int32 queueType, Int32 currentUserId, Int32 communicationTypeId)
        {
            //Int32 communicationTypeId = ADBMessageQueueContext.lkpCommunicationTypes.Where(x => x.Code == communicationTypeCode).FirstOrDefault().CommunicationTypeID;
            String messageMode = MessageMode.TEMPLATEMESSAGE.GetStringValue().ToLower();
            return ADBMessageQueueContext.ADBMessages.Where(obj =>
                obj.MessageMode.Equals(messageMode)
                && obj.From == currentUserId
                && obj.CommunicationTypeID == communicationTypeId
                && !obj.IsDeleted);
        }

        /// <summary>
        /// Get the updated data for queue.
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="queueType"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="lastDateTime"></param>
        /// <returns></returns>
        public IEnumerable<EntityObject> GetUpdateQueue(Int32 folderId, Int32 queueType, Int32 queueOwnerId, DateTime lastDateTime)
        {
            string messageMode = string.Empty;
            return null;
        }

        /// <summary>
        /// Invoked to get message content
        /// </summary>
        /// <param name="queueType"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public EntityObject GetMessageContents(Int32 queueType, Guid messageId, Int32 currentUserId = 0, Boolean IsDashboardMessage = false)
        {
            if (currentUserId != AppConsts.NONE && IsDashboardMessage)
                UpdateReadStatus(messageId, currentUserId, AppConsts.NONE, false);

            return
            ADB_MessageQueueContext.ADBMessages
                .Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGETOLISTS)
                .FirstOrDefault(obj => obj.ADBMessageID == messageId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueType"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public EntityObject GetMessageContentsForApplicant(Int32 queueType, Guid messageId)
        {
            return
            ADB_MessageQueueContext.ADBMessages
                .Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGETOUSERGROUPS)
                .FirstOrDefault(obj => obj.ADBMessageID == messageId);
        }


        /// <summary>
        /// Invoked to get message content
        /// </summary>
        /// <param name="queueType"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public ComplexObject GetCommonMessageContents(Int32 queueType, Guid messageId)
        {
            return ADBQueueContext.GetADBMessageContents(messageId).FirstOrDefault();
        }

        public EntityObject GetMessage(Int32 folderId, Int32 queueType, Int32 queueOwnerId, Guid messageId)
        {
            throw new SysXException("No Queue Available");
        }

        public Boolean SendMessageInBatch(Int32 initiator, String[] messages)
        {
            return true;
        }

        /// <summary>
        /// Reply message 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="initiator"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Boolean ReplyMessage(Guid messageId, Int32 initiator, String message, String applicationDatabaseName, Int32 senderId, String subject)
        {
            ADBMessageQueueContext.ReplyGenMsgToADB(messageId, message, applicationDatabaseName, subject, senderId);
            return true;
        }

        /// <summary>
        /// Invoked to save drafted message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <param name="messageMode"></param>
        /// <param name="subject"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public Boolean SaveDraftedMesssage(Guid messageId, String message, String messageMode, String subject, Int32 organizationUserId, String toUsers, String ccUsers, String toGroupIds, String ccGroupIds, Boolean isHighImportance, String documentName, String originalDocumentName, String bccUsers, String from, Int32 externaMessagingGroupTypeId, String messageContent)
        {
            return SaveDraftedADBMesssage(messageId, message, messageMode, subject, organizationUserId, toUsers, ccUsers, toGroupIds, ccGroupIds, isHighImportance, documentName, originalDocumentName, bccUsers, from, externaMessagingGroupTypeId, messageContent);
        }

        /// <summary>
        /// Invoked to delete message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <param name="folderCode"></param>
        /// <returns></returns>
        public Boolean DeleteMesssage(Guid messageId, Int32 userId, String folderCode, Int32 queueTypeID)
        {
            return DeleteADBMesssage(messageId, userId, folderCode);
        }

        public Boolean DeleteMesssageFromDashboard(Guid messageId, int userId)
        {
            ADBMessageToList adbMessageToLists = ADBMessageQueueContext.ADBMessageToLists.Where(obj => obj.ADBMessageID == messageId && obj.EntityID == userId).FirstOrDefault();
            adbMessageToLists.IsDeleted = true;
            ADBMessageQueueContext.SaveChanges();
            return true;
        }

        public Boolean DeleteMesssage(Guid messageId)
        {
            ADBMessage message = ADBMessageQueueContext.ADBMessages.FirstOrDefault(obj => obj.ADBMessageID.Equals(messageId));
            if (message != null)
            {
                message.IsDeleted = true;
                ADBQueueContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Restoring Deleted Messages
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>        
        /// <param name="queueTypeID"></param>
        /// <returns></returns>
        public Boolean RestoreMessage(Guid messageId, Int32 userId, Int32 queueTypeID)
        {

            return RestoreADBMesssage(messageId, userId);
        }

        /// <summary>
        /// Invoked to update the followup messages
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <param name="queueTypeID"></param>
        /// <param name="isFollowUp"></param>
        /// <returns></returns>
        public Boolean UpdateFollowUpStatus(Guid messageId, Int32 userId, Int32 queueTypeID, Boolean isFollowUp, String folderCode)
        {
            ADBMessageToList adbMessageToList = ADBQueueContext.ADBMessageToLists.FirstOrDefault(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);

            if (folderCode.Equals(lkpMessageFolderContext.SENTITEMS.GetStringValue()) || adbMessageToList.IsNull())
            {
                ADBMessage adbMessage = ADBQueueContext.ADBMessages.FirstOrDefault(obj => obj.ADBMessageID == (messageId));
                adbMessage.IsFollowUp = isFollowUp;
            }
            else
            {
                adbMessageToList.IsFollowup = isFollowUp;
            }
            ADBQueueContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adbMessageId"></param>
        /// <param name="from"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public String GetUniqueTemplateName(Guid adbMessageId, Int32 from, String templateName)
        {
            return ADBMessageQueueContext.GetUniqueTemplateName(adbMessageId, from, templateName).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adbMessage"></param>
        /// <returns></returns>
        public Boolean UpdateMessage(ADBMessage adbMessage)
        {
            ADBMessageQueueContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Invoked to update message read status
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <param name="queueTypeID"></param>
        /// <param name="isUnread"></param>
        /// <returns></returns>
        public Boolean UpdateReadStatus(Guid messageId, Int32 userId, Int32 queueTypeID, Boolean isUnread)
        {
            IQueryable<ADBMessageToList> adbMessageToList = ADBMessageQueueContext.ADBMessageToLists.Where(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);
            if (adbMessageToList != null)
            {
                foreach (ADBMessageToList record in adbMessageToList)
                {
                    record.IsUnread = isUnread;
                }
            }
            ADBMessage adbMessage = ADBMessageQueueContext.ADBMessages.FirstOrDefault(obj => obj.ADBMessageID == messageId);
            adbMessage.IsUnread = isUnread;
            ADBMessageQueueContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Invoked to add new folder in the db
        /// </summary>
        /// <param name="nodeText"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public String AddNewFolder(String nodeText, Int32 currentUserID, Int32 userGroup, Int32 parentFolderID)
        {
            String result = "duplicate";
            lkpMessageFolder messageFolder = new lkpMessageFolder();
            messageFolder.Name = nodeText;
            messageFolder.ImageName = nodeText;
            messageFolder.Code = "MSG" + Guid.NewGuid().ToString().Substring(0, 8);
            messageFolder.IsDefault = false;
            messageFolder.IsDeleted = false;
            messageFolder.UserGroupID = userGroup;
            messageFolder.CreatedByID = currentUserID;
            messageFolder.CreatedOn = DateTime.Now;
            messageFolder.ModifiedById = currentUserID;
            messageFolder.ModifiedOn = DateTime.Now;
            messageFolder.MessageFolderParentID = parentFolderID;
            try
            {
                base.AddObjectEntity(messageFolder);
                result = "success";
                return String.Format("{{\"result\":\"{2}\",\"folderName\":\"{0}#{1}\"}}", messageFolder.MessageFolderID, messageFolder.Code, result);
            }
            catch (Exception ex)
            {
                result = "error";
                return String.Format("{{\"result\":\"{2}\",\"folderName\":\"{0}#{1}\"}}", messageFolder.MessageFolderID, messageFolder.Code, result);
            }

        }

        public List<Int32> GetAllFoldersToBedeleted(Int32 folderId, Int32 userId)
        {

            lkpMessageFolder messageFolder = APPDbContext.lkpMessageFolders.Where(obj => obj.MessageFolderID.Equals(folderId) && !(obj.CreatedByID == (AppConsts.NONE))).FirstOrDefault();
            List<Int32> folderToBeDeleted = new List<Int32>();
            if (!messageFolder.IsNull())
                if (!messageFolder.IsDeleted)
                {
                    folderToBeDeleted = GetSubFolders(messageFolder);
                }

            return folderToBeDeleted;
        }

        List<Int32> foldersToDelete = new List<int>();
        public List<Int32> GetSubFolders(lkpMessageFolder messageFolder)
        {
            List<lkpMessageFolder> childFolderIDs = APPDbContext.lkpMessageFolders.Where(x => x.MessageFolderParentID == messageFolder.MessageFolderID).ToList();
            foldersToDelete.Add(messageFolder.MessageFolderID);

            foreach (var item in childFolderIDs)
            {
                GetSubFolders(item);
            }
            return foldersToDelete;
        }

        public void DeleteMessageRulesintransaction(List<Int32> folderToBeDeleted, Int32 userId)
        {
            foreach (var deletedFolderID in folderToBeDeleted)
            {
                IQueryable<MessageRule> ruleRelatedToCurrentFolder = AppDBContext.MessageRules.Where(obj => obj.MessageFolderID == deletedFolderID && obj.UserID == userId);
                foreach (MessageRule rule in ruleRelatedToCurrentFolder)
                {
                    rule.IsDeleted = true;
                }
            }
        }

        public void DeleteAllMessagesintransaction(List<Int32> folderToBeDeleted, Int32 userId)
        {
            foreach (var deletedFolderID in folderToBeDeleted)
            {
                IQueryable<ADBMessageToList> messageInCurrentFolder = ADBMessageQueueContext.ADBMessageToLists.Where(obj => obj.FolderID == deletedFolderID && obj.EntityID == userId && !obj.IsDeleted && obj.IsActive);
                foreach (ADBMessageToList message in messageInCurrentFolder)
                {
                    message.IsDeleted = true;
                }
            }
        }

        public void DeleteAllFolders(List<Int32> folderToBeDeleted, Int32 userId)
        {
            foreach (var deletedFolderID in folderToBeDeleted)
            {
                lkpMessageFolder currentMessageFolder = APPDbContext.lkpMessageFolders.FirstOrDefault(obj => obj.MessageFolderID.Equals(deletedFolderID) && (obj.CreatedByID == userId));
                currentMessageFolder.IsDeleted = true;
            }
            AppDBContext.SaveChanges();
            ADBMessageQueueContext.SaveChanges();
        }

        public String FoldersToBeRestored(Guid messageId, Int32 userId)
        {
            ADBMessageToList adbMessageToList = ADBQueueContext.ADBMessageToLists.FirstOrDefault(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);
            Int32 folderID = adbMessageToList.FolderID;
            lkpMessageFolder messageFolder = AppDBContext.lkpMessageFolders.FirstOrDefault(obj => obj.MessageFolderID == folderID && obj.IsDeleted);
            String restoredFolder = "";
            if (messageFolder != null)
            {
                restoredFolder = String.Format("{0}#{1}", messageFolder.MessageFolderID, messageFolder.Code);
            }

            return restoredFolder;
        }

        public List<Int32> GetAllFoldersToBeRestored(Guid messageId, Int32 userId)
        {
            ADBMessageToList adbMessageToList = ADBQueueContext.ADBMessageToLists.FirstOrDefault(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);
            lkpMessageFolder messageFolder = null;
            if (!adbMessageToList.IsNullOrEmpty())
            {
                Int32 folderID = adbMessageToList.FolderID;
                messageFolder = AppDBContext.lkpMessageFolders.FirstOrDefault(obj => obj.MessageFolderID == folderID && obj.IsDeleted);
            }
            if (messageFolder != null)
            {
                foldersToRestored = GetParentFolders(messageFolder);
            }

            return foldersToRestored;
        }

        List<Int32> foldersToRestored = new List<int>();
        public List<Int32> GetParentFolders(lkpMessageFolder messageFolder)
        {
            List<lkpMessageFolder> parentFolderIDs = APPDbContext.lkpMessageFolders.Where(x => x.MessageFolderID == messageFolder.MessageFolderParentID && x.IsDeleted).ToList();

            foldersToRestored.Add(messageFolder.MessageFolderID);

            if (!parentFolderIDs.IsNullOrEmpty())
            {
                foreach (var item in parentFolderIDs)
                {
                    GetParentFolders(item);
                }
            }
            return foldersToRestored;
        }

        public void RestoreAllFolders(List<Int32> foldersToBeRestored, Int32 userId)
        {
            foreach (var restoreFolderID in foldersToBeRestored)
            {

                lkpMessageFolder currentMessageFolder = APPDbContext.lkpMessageFolders.FirstOrDefault(obj => obj.MessageFolderID.Equals(restoreFolderID));
                currentMessageFolder.IsDeleted = false;
            }
            AppDBContext.SaveChanges();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invoked to add or delete emails from tolist or cclist
        /// </summary>
        /// <param name="adbMessage"></param>
        /// <param name="supplierMessage"></param>
        /// <param name="userIdList"></param>
        /// <param name="isCC"></param>
        /// <param name="folderId"></param>
        private void ChangeADBEmailUsers(ADBMessage adbMessage, String[] userIdList, Boolean isCC, Int32 folderId, Boolean isBcc)
        {
            Guid conversationId;
            Guid conversationHandleId;
            Guid conversationGroupId;
            ADBMessageToList tempMessageList = adbMessage.ADBMessageToLists.FirstOrDefault();
            if (tempMessageList == null)
            {
                conversationId = new Guid();
                conversationHandleId = adbMessage.ConversationHandleID.IsNullOrEmpty() ? new Guid() : (Guid)adbMessage.ConversationHandleID;
                conversationGroupId = new Guid();
            }
            else
            {
                conversationId = tempMessageList.ConversationID;
                conversationHandleId = tempMessageList.ConversationHandleID;
                conversationGroupId = tempMessageList.ConversationGroupID;
            }

            for (int i = AppConsts.NONE; i < userIdList.Length; i++)
            {
                if (!userIdList[i].Equals(String.Empty) && !adbMessage.ADBMessageToLists.Any(obj => obj.IsCC == (isCC) && obj.EntityID == (Convert.ToInt32(userIdList[i])) && obj.IsBcc == (isBcc)))
                {
                    ADBMessageToList adbMessageToList = new ADBMessageToList();
                    adbMessageToList.ConversationID = conversationId;
                    adbMessageToList.ConversationHandleID = conversationHandleId;
                    adbMessageToList.ConversationGroupID = conversationGroupId;
                    adbMessageToList.EntityID = Convert.ToInt32(userIdList[i]);
                    adbMessageToList.ADBMessageID = adbMessage.ADBMessageID;
                    adbMessageToList.IsCC = isCC;
                    adbMessageToList.FolderID = folderId;
                    adbMessageToList.IsUnread = true;
                    adbMessageToList.IsFollowup = false;
                    adbMessageToList.IsActive = true;
                    adbMessageToList.IsBcc = isBcc;
                    adbMessage.ADBMessageToLists.Add(adbMessageToList);
                }
            }
            ADBMessageQueueContext.SaveChanges();

            Int32[] previousToListUsers = adbMessage.ADBMessageToLists.Where(s => s.IsCC == (isCC) && s.EntityID.HasValue && s.IsBcc == (isBcc)).Select(s => s.EntityID.Value).ToArray();

            for (int count = AppConsts.NONE; count < previousToListUsers.Length; count++)
            {
                if (!userIdList.Contains(previousToListUsers[count].ToString()))
                {
                    ADBMessageToList adbMessageList = adbMessage.ADBMessageToLists.First(messageToList => messageToList.EntityID == previousToListUsers[count]);
                    ADBMessageQueueContext.ADBMessageToLists.DeleteObject(adbMessageList);
                }
            }
            ADBMessageQueueContext.SaveChanges();
        }

        private Boolean SetADBMoveToFolder(Guid messageId, Int32 userId, String folderCode, Int32 moveToFolderId, String moveToFolderCode)
        {
            if (moveToFolderCode.Equals(lkpMessageFolderContext.DELETEDITEMS.GetStringValue()))
            {
                ADBMessageToList adbMessageToList = ADBQueueContext.ADBMessageToLists.FirstOrDefault(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);
                adbMessageToList.IsDeleted = true;
            }
            else
            {
                IQueryable<ADBMessageToList> adbMessageToList = ADBQueueContext.ADBMessageToLists.Where(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);
                foreach (ADBMessageToList message in adbMessageToList)
                {
                    message.FolderID = moveToFolderId;
                }

            }
            ADBQueueContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Invoked to delete messages
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <param name="folderCode"></param>
        /// <returns></returns>
        private Boolean DeleteADBMesssage(Guid messageId, Int32 userId, String folderCode)
        {
            if (folderCode.Equals(lkpMessageFolderContext.DELETEDITEMS.GetStringValue()))
            {
                ADBMessage adbMessage = ADBMessageQueueContext.ADBMessages.FirstOrDefault(obj => obj.ADBMessageID.Equals(messageId));
                if (!adbMessage.IsNull() && adbMessage.IsDeleted && adbMessage.IsActive)
                {
                    adbMessage.IsActive = false;
                }
                else
                {
                    IQueryable<ADBMessageToList> adbMessageToLists = ADBMessageQueueContext.ADBMessageToLists.Where(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);
                    if (!adbMessageToLists.IsNull())
                    {
                        foreach (ADBMessageToList adbMessageToList in adbMessageToLists)
                        {
                            adbMessageToList.IsActive = false;
                        }
                    }
                }
            }
            else
            {
                ADBMessage adbMessage = ADBMessageQueueContext.ADBMessages.FirstOrDefault(obj => obj.ADBMessageID == (messageId));
                if (!adbMessage.IsNull() && (folderCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()) || folderCode.Equals(lkpMessageFolderContext.SENTITEMS.GetStringValue())))
                {
                    adbMessage.IsDeleted = true;
                }
                else
                {
                    IQueryable<ADBMessageToList> adbMessageToLists = ADBMessageQueueContext.ADBMessageToLists.Where(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);
                    if (!adbMessageToLists.IsNull())
                    {
                        foreach (ADBMessageToList adbMessageToList in adbMessageToLists)
                        {
                            adbMessageToList.IsDeleted = true;
                        }
                    }
                }
            }
            ADBMessageQueueContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Restore ADB Message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>        
        /// <returns></returns>
        private Boolean RestoreADBMesssage(Guid messageId, Int32 userId)
        {
            ADBMessage adbMessage = ADBMessageQueueContext.ADBMessages.FirstOrDefault(obj => obj.ADBMessageID.Equals(messageId) && obj.From == userId);
            if (!adbMessage.IsNull() && adbMessage.IsDeleted && adbMessage.IsActive)
            {
                adbMessage.IsDeleted = false;
            }
            else
            {
                IQueryable<ADBMessageToList> adbMessagesToList = ADBMessageQueueContext.ADBMessageToLists.Where(obj => obj.ADBMessageID == messageId && obj.EntityID == userId);
                if (!adbMessagesToList.IsNull())
                {
                    foreach (ADBMessageToList adbMessageinlist in adbMessagesToList)
                    {
                        adbMessageinlist.IsDeleted = false;
                    }
                }
            }
            ADBMessageQueueContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Invoked to save the draft message sent from ADB to supplier or supplier to ADB.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <param name="messageMode"></param>
        /// <param name="subject"></param>
        /// <param name="toUsers"></param>
        /// <param name="ccUsers"></param>
        /// <returns></returns>
        private Boolean SaveDraftedADBMesssage(Guid messageId, String message, String messageMode, String subject, Int32 organizationUserId, String toUsers, String ccUsers, String toGroupIds, String ccGroupIds, Boolean isHighImportance, String documentName, String originalDocumentName, String bccUsers, String from, Int32 externaMessagingGroupTypeId, String messageContent)
        {
            ADBMessage adbMessage = ADBMessageQueueContext.ADBMessages.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGETOLISTS).FirstOrDefault(obj => obj.ADBMessageID == messageId);
            adbMessage.Message = message;
            adbMessage.Subject = subject;
            adbMessage.IsHighImportant = isHighImportance;
            adbMessage.DocumentName = documentName;
            adbMessage.OriginalDocumentName = originalDocumentName;
            adbMessage.ReceiveDate = DateTime.Now;
            adbMessage.IsUnread = true;

            String inboxFolderCode = lkpMessageFolderContext.INBOX.GetStringValue();
            String draftFolderCode = lkpMessageFolderContext.DRAFTS.GetStringValue();
            Int32 folderId = AppDBContext.lkpMessageFolders.Where(obj => obj.Code.Equals(draftFolderCode)).Select(x => x.MessageFolderID).FirstOrDefault();


            if (messageMode.Equals("S"))
            {
                adbMessage.MessageMode = messageMode;
                folderId = AppDBContext.lkpMessageFolders.Where(obj => obj.Code.Equals(inboxFolderCode)).Select(x => x.MessageFolderID).FirstOrDefault();
                foreach (ADBMessageToList adblist in adbMessage.ADBMessageToLists)
                {
                    adblist.FolderID = folderId;
                    adblist.IsUnread = true;
                }
            }

            String[] idList;
            if (!string.IsNullOrEmpty(toUsers))
            {
                idList = toUsers.Split(';');
                if (idList.Length > 0)
                    ChangeADBEmailUsers(adbMessage, idList, false, folderId, false);
            }

            if (!String.IsNullOrEmpty(ccUsers))
            {
                idList = ccUsers.Split(';');
                if (idList.Length > 0)
                    ChangeADBEmailUsers(adbMessage, idList, true, folderId, false);
            }

            if (!String.IsNullOrEmpty(bccUsers))
            {
                idList = bccUsers.Split(';');
                if (idList.Length > 0)
                    ChangeADBEmailUsers(adbMessage, idList, false, folderId, true);
            }

            UpdateADBMessageToUserGroup(messageId, toGroupIds, ccGroupIds, organizationUserId, (Guid)adbMessage.ConversationHandleID);
            if (messageMode.Equals("S") && (!toGroupIds.IsNullOrEmpty() || !ccGroupIds.IsNullOrEmpty()))
            {
                SendMailToExternalGroup(toGroupIds, ccGroupIds, isHighImportance, subject, messageContent, from, externaMessagingGroupTypeId, organizationUserId);
            }
            return true;
        }

        private void UpdateADBMessageToUserGroup(Guid messageId, String toGroupIds, String ccGroupIds, Int32 organizationUserId, Guid conversationHandleID)
        {
            IEnumerable<ADBMessageToUserGroup> adbMessageToUserGroups = ADBMessageQueueContext.ADBMessageToUserGroups.Where(adbMessageToUserGroup => adbMessageToUserGroup.ADBMessageID.Equals(messageId));
            foreach (ADBMessageToUserGroup adbMessageToUserGroup in adbMessageToUserGroups)
                ADBMessageQueueContext.ADBMessageToUserGroups.DeleteObject(adbMessageToUserGroup);

            Guid conversationGroupID = Guid.NewGuid();
            Guid conversationID = Guid.NewGuid();
            SaveADBMessageToUserGroup(messageId, toGroupIds, false, organizationUserId, conversationHandleID, conversationGroupID, conversationID);
            SaveADBMessageToUserGroup(messageId, ccGroupIds, true, organizationUserId, conversationHandleID, conversationGroupID, conversationID);

            ADBMessageQueueContext.SaveChanges();

        }

        private void SaveADBMessageToUserGroup(Guid messageId, string groupIds, bool isCC, Int32 organizationUserId, Guid conversationHandleID, Guid conversationGroupID, Guid conversationID)
        {
            String[] idList;
            if (!string.IsNullOrEmpty(groupIds))
            {
                idList = groupIds.Split(';');
                for (int i = 0; i < idList.Length; i++)
                {
                    if (string.IsNullOrEmpty(idList[i]) || idList[i] == ";")
                        continue;

                    ADBMessageToUserGroup adbMessageToUserGroup = new ADBMessageToUserGroup()
                    {
                        ADBMessageID = messageId,
                        UserGroupID = int.Parse(idList[i]),
                        ConversationHandleID = conversationHandleID,
                        ConversationGroupID = conversationGroupID,
                        ConversationID = conversationID,
                        IsCC = isCC,
                        UserID = organizationUserId,
                        IsActive = true,
                        IsDeleted = false
                    };

                    ADBMessageQueueContext.ADBMessageToUserGroups.AddObject(adbMessageToUserGroup);
                }
            }
        }

        #region user group mapping

        /// <summary>
        /// Get User Group Message Type
        /// </summary>
        /// <param name="usergroupId">usergroupId</param>
        /// <param name="typeId">typeId</param>
        /// <param name="tenantType">tenantType</param>
        /// <returns>user group id</returns>
        private IQueryable<UserGroupMessageType> GetUserGroupMessageType(Int32 usergroupId, Int32? typeId, TenantType tenantType)
        {
            if (tenantType.Equals(TenantType.Institution))
            {
                return _compiledQueryGetUserGroupMessageTypeByClientId.Invoke(base.AppDBContext, usergroupId, typeId);
            }
            else if (tenantType.Equals(TenantType.Supplier))
            {
                return _compiledQueryGetUserGroupMessageTypeBySupplierId.Invoke(base.AppDBContext, usergroupId, typeId);
            }
            else
            {
                return _compiledQueryGetUserGroupMessageTypeByADB.Invoke(base.AppDBContext, usergroupId);
            }

        }

        #region Messaging Company

        /// <summary>
        /// Retrieves CurrentUser Message Types
        /// </summary>
        /// <returns>list of active User groups</returns>
        UserGroup IMessageRepository.GetUserGroupByCurrentUserIdAndMessageType(Int32 userId, Int32 messageTypeId)
        {
            return AppDBContext.UserGroups.Include("UsersInUserGroups.aspnet_Users.OrganizationUsers")
                .Include("UserGroupMessageTypes").FirstOrDefault(cond => cond.UserGroupMessageTypes.Any(usermsgType => usermsgType.MessageTypeID.Equals(messageTypeId))
                                                                        && cond.UsersInUserGroups.Any(users => users.aspnet_Users.OrganizationUsers.Any(organizationuser => organizationuser.OrganizationUserID.Equals(userId))));

        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Compiled Queries

        #region user group mapping

        /// <summary>
        /// compiled Query Get User GroupM essage Type By ClientId
        /// </summary>
        private static readonly Func<SysXAppDBEntities, Int32, Int32?, IQueryable<UserGroupMessageType>> _compiledQueryGetUserGroupMessageTypeByClientId =
        CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32?, IQueryable<UserGroupMessageType>>((context, usergroupId, typeId) => context.UserGroupMessageTypes
                                                                                                                                   .Where(condition => condition.UserGroupID.Equals(usergroupId) && condition.ClientID == typeId));

        /// <summary>
        /// compiled Query Get User Group Message Type By SupplierId
        /// </summary>
        private static readonly Func<SysXAppDBEntities, Int32, Int32?, IQueryable<UserGroupMessageType>> _compiledQueryGetUserGroupMessageTypeBySupplierId =
        CompiledQuery.Compile<SysXAppDBEntities, Int32, Int32?, IQueryable<UserGroupMessageType>>((context, usergroupId, typeId) => context.UserGroupMessageTypes
                                                                                                                                    .Where(condition => condition.UserGroupID.Equals(usergroupId) && condition.SupplierID == typeId));

        /// <summary>
        /// compiled Query Get User Group Message Type By ADB
        /// </summary>
        private static readonly Func<SysXAppDBEntities, Int32, IQueryable<UserGroupMessageType>> _compiledQueryGetUserGroupMessageTypeByADB =
        CompiledQuery.Compile<SysXAppDBEntities, Int32, IQueryable<UserGroupMessageType>>((context, usergroupId) => context.UserGroupMessageTypes.
                                                                                                                                    Where(condition => condition.UserGroupID.Equals(usergroupId)));

        #endregion

        #endregion

        #region Bug Work

        /// <summary>
        /// Get colection of message type base on receiver type
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        List<lkpMessageType> IMessageRepository.GetMessageTypesByReceiver(TenantTypeEnum receiver)
        {
            List<lkpMessageType> messageTypes = new List<lkpMessageType>();
            List<UserGroupMessageType> userGroupMessageTypeList = APPDbContext.UserGroupMessageTypes.Include("lkpMessageType").ToList();

            switch ((TenantTypeEnum)receiver)
            {
                case TenantTypeEnum.Client:
                    messageTypes.Clear();
                    if ((!userGroupMessageTypeList.IsNull()) && (userGroupMessageTypeList.Count > AppConsts.NONE))
                    {
                        userGroupMessageTypeList.Where(msgType => (msgType.ClientID != null) && (!msgType.IsDeleted)).ForEach(cond => messageTypes.Add(cond.lkpMessageType));
                    }
                    break;

                case TenantTypeEnum.Company:
                    messageTypes.Clear();
                    if ((!userGroupMessageTypeList.IsNull()) && (userGroupMessageTypeList.Count > AppConsts.NONE))
                    {
                        userGroupMessageTypeList.Where(msgType => (msgType.ClientID == null) && ((msgType.SupplierID == null)) && (!msgType.IsDeleted))
                                                .ForEach(cond => messageTypes.Add(cond.lkpMessageType));
                    }
                    break;

                case TenantTypeEnum.Supplier:
                    messageTypes.Clear();
                    if ((!userGroupMessageTypeList.IsNull()) && (userGroupMessageTypeList.Count > AppConsts.NONE))
                    {
                        userGroupMessageTypeList.Where(msgType => (msgType.SupplierID != null) && (!msgType.IsDeleted)).ForEach(cond => messageTypes.Add(cond.lkpMessageType));
                    }
                    break;
            }
            return messageTypes.Distinct().ToList();
        }

        #endregion

        private SysXAppDBEntities APPDbContext
        {
            get { return base.AppDBContext; }
        }

        private ADBMessageDB_DevEntities ADBMessageQueueContext
        {
            get { return base.ADB_MessageQueueContext; }
        }

        /// <summary>
        /// Get folders specific to the user.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<lkpMessageFolder> GetFolders(Int32 userID, Int32 userGroupID)
        {
            if (userGroupID.Equals(AppConsts.NONE))
            {
                return APPDbContext.lkpMessageFolders.Where(message => (message.CreatedByID == (userID) || message.CreatedByID == (AppConsts.NONE)) && !message.IsDeleted).ToList();
            }
            var sql = APPDbContext.lkpMessageFolders.Where(message => ((message.CreatedByID.Equals(userID) && message.UserGroupID == userGroupID) || message.CreatedByID.Equals(AppConsts.NONE)) && !message.IsDeleted);
            return ((ObjectQuery<lkpMessageFolder>)sql).Execute(MergeOption.NoTracking).ToList();
        }

        public List<lkpMessageType> GetMessageTypesByReceiver(TenantTypeEnum receiver)
        {
            List<lkpMessageType> messageTypes = new List<lkpMessageType>();
            List<UserGroupMessageType> userGroupMessageTypeList = APPDbContext.UserGroupMessageTypes.Include("lkpMessageType").ToList();

            switch ((TenantTypeEnum)receiver)
            {
                case TenantTypeEnum.Client:
                    messageTypes.Clear();
                    if ((!userGroupMessageTypeList.IsNull()) && (userGroupMessageTypeList.Count > AppConsts.NONE))
                    {
                        userGroupMessageTypeList.Where(msgType => (msgType.ClientID != null) && (!msgType.IsDeleted)).ForEach(cond => messageTypes.Add(cond.lkpMessageType));
                    }
                    break;

                case TenantTypeEnum.Company:
                    messageTypes.Clear();
                    if ((!userGroupMessageTypeList.IsNull()) && (userGroupMessageTypeList.Count > AppConsts.NONE))
                    {
                        userGroupMessageTypeList.Where(msgType => (msgType.ClientID == null) && ((msgType.SupplierID == null)) && (!msgType.IsDeleted))
                                                .ForEach(cond => messageTypes.Add(cond.lkpMessageType));
                    }
                    break;

                case TenantTypeEnum.Supplier:
                    messageTypes.Clear();
                    if ((!userGroupMessageTypeList.IsNull()) && (userGroupMessageTypeList.Count > AppConsts.NONE))
                    {
                        userGroupMessageTypeList.Where(msgType => (msgType.SupplierID != null) && (!msgType.IsDeleted)).ForEach(cond => messageTypes.Add(cond.lkpMessageType));
                    }
                    break;
            }
            return messageTypes.Distinct().ToList();
        }

        /// <summary>
        /// Invoked to get the tenant type.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Int32 GetTenantType(Int32 userID)
        {
            return AppDBContext.OrganizationUsers.Include(SysXEntityConstants.TABLE_ORGANIZATION_TENANT)
              .Where(obj => obj.OrganizationUserID.Equals(userID)).Select(sel => (Int32)sel.Organization.Tenant.TenantTypeID).FirstOrDefault();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public ADBMessage GetMesssage(Guid messageId)
        {
            return ADBMessageQueueContext.ADBMessages.FirstOrDefault(obj => obj.ADBMessageID.Equals(messageId));
        }

        public bool SendMessage(Int32 intiator, String message, String applicationDatabaseName, Int32 senderId, String subject, out Guid adbMessageID)
        {
            try
            {
                ObjectParameter messageID = new ObjectParameter("MessageID", typeof(Guid));
                ADBQueueContext.SendGenMsgToADBQueue(message, applicationDatabaseName, senderId, subject, messageID);
                adbMessageID = (Guid)messageID.Value;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<EntityObject> GetQueue(Int32 queueOwnerId, Int32 communicationTypeId)
        {
            List<String> folderCode = new List<string> { "MSGPNF", "MSGSNT", "MSGDEL", "MSGDRF", "MSGFUP", "MSGJNK" };
            List<Int32> folderIds = APPDbContext.lkpMessageFolders.Where(obj => !folderCode.Contains(obj.Code) && !obj.IsDeleted).Select(y => y.MessageFolderID).ToList();
            return ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE)
                        .Where(obj => obj.EntityID == (queueOwnerId) && folderIds.Contains(obj.FolderID) && (!obj.IsDeleted) && obj.IsActive && !(obj.ADBMessage.CommunicationTypeID == communicationTypeId)).OrderByDescending(a => a.ADBMessage.ReceiveDate);
        }

        public IEnumerable<EntityObject> GetQueue(int folderId, string folderCode, int queueType, int queueOwnerId, int userGroup, Int32 communicationTypeId)
        {
            String MessageMode = String.Empty;

            if (folderCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()))
            {
                MessageMode = INTSOF.Utils.MessageMode.DRAFTMESSAGE.GetStringValue();
            }
            else
            {
                MessageMode = INTSOF.Utils.MessageMode.SENDMESSAGE.GetStringValue();
            }
            List<Int32> UserGroupIds = new List<Int32>();
            OrganizationUser organizationUser = APPDbContext.OrganizationUsers.FirstOrDefault(orgUser => orgUser.OrganizationUserID == queueOwnerId);
            if (!organizationUser.IsNull())
            {
                UserGroupIds = APPDbContext.UsersInUserGroups.Where(condition => condition.UserID.Equals(organizationUser.UserID))
                                                               .Select(obj => obj.UserGroupID).ToList();
            }

            if (folderCode.Equals(lkpMessageFolderContext.DELETEDITEMS.GetStringValue()))
            {
                if (userGroup.Equals(AppConsts.NONE))//In Case of Employee
                {
                    IEnumerable<EntityObject> message = ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE)
                        .Where((obj => (obj.EntityID == queueOwnerId && obj.IsDeleted && obj.IsActive))).OrderByDescending(a => a.ADBMessage.ReceiveDate);
                    List<Guid?> exIDs = message.Select(item => (item as ADBMessageToList).ADBMessageID).ToList();

                    List<ADBMessage> draftMessage = ADBQueueContext.ADBMessages.Where(obj => obj.From.Value.Equals(queueOwnerId) && obj.IsActive
                        && (obj.IsDeleted) && obj.MessageMode == "D").ToList();
                    draftMessage.RemoveAll(item => exIDs.Contains(item.ADBMessageID));

                    var messageList = draftMessage.Select(item =>
                    {
                        ADBMessageToList newItem = new ADBMessageToList();
                        newItem.ADBMessage = item;
                        return newItem;

                    });
                    message = message.Concat(messageList);

                    List<ADBMessage> sentMessage = ADBQueueContext.ADBMessages.Where(obj => obj.From.Value.Equals(queueOwnerId) && obj.IsActive && (obj.IsDeleted)).ToList();
                    var sentMessageList = sentMessage.Select(item =>
                    {
                        ADBMessageToList newItem = new ADBMessageToList();
                        newItem.ADBMessage = item;
                        return newItem;

                    });
                    message = message.Concat(sentMessageList);
                    return message;

                }
                else
                {
                    return ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE).Include(SysXEntityConstants.TABLE_ADBMESSAGING_ADBMESSAGETOUSERGROUPS)
                           .Where(obj => (obj.EntityID == queueOwnerId && obj.IsDeleted && obj.IsActive && obj.ADBMessage.ADBMessageToUserGroups.Any(adbMsgGroup => adbMsgGroup.UserGroupID.Equals(userGroup)))
                                                                                                || (UserGroupIds.Contains(obj.ADBMessage.From.Value) && obj.ADBMessage.IsActive && (obj.ADBMessage.IsDeleted))
                                                                                                ).OrderByDescending(a => a.ADBMessage.ReceiveDate);
                }
            }
            else if (folderCode.Equals(lkpMessageFolderContext.SENTITEMS.GetStringValue()))
            {
                if (userGroup.Equals(AppConsts.NONE))//In Case of Employee
                {
                    IEnumerable<EntityObject> message = ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE).Where(obj => (obj.ADBMessage.MessageMode.Equals(MessageMode) && obj.ADBMessage.From == queueOwnerId && obj.ADBMessage.IsActive && (!obj.ADBMessage.IsDeleted))
                                                                                           ).OrderByDescending(a => a.ADBMessage.ReceiveDate);
                    List<Guid?> exIDs = message.Select(item => (item as ADBMessageToList).ADBMessageID).ToList();
                    List<ADBMessage> groupMessage = ADBQueueContext.ADBMessages.Where(obj => obj.From.Value.Equals(queueOwnerId) && obj.IsActive
                      && (!obj.IsDeleted) && obj.MessageMode == "S").ToList();
                    groupMessage.RemoveAll(item => exIDs.Contains(item.ADBMessageID));

                    var messageList = groupMessage.Select(item =>
                    {
                        ADBMessageToList newItem = new ADBMessageToList();
                        newItem.ADBMessage = item;
                        return newItem;

                    });
                    message = message.Concat(messageList);
                    return message;
                }
                else
                {
                    return ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE).Where(obj => (obj.ADBMessage.MessageMode.Equals(MessageMode) && userGroup == (obj.ADBMessage.From) && obj.ADBMessage.IsActive && (!obj.ADBMessage.IsDeleted))
                                                                                              ).OrderByDescending(a => a.ADBMessage.ReceiveDate);
                }

            }
            else if (folderCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()))
            {
                if (userGroup.Equals(AppConsts.NONE))//In Case of Employee
                {
                    return ADBQueueContext.ADBMessages.Where(obj => (obj.MessageMode.Equals(MessageMode) && obj.From == queueOwnerId && obj.IsActive && (!obj.IsDeleted))).OrderByDescending(a => a.ReceiveDate);
                }
                else
                {
                    return ADBQueueContext.ADBMessages.Where(obj => (obj.MessageMode.Equals(MessageMode) && userGroup == (obj.From) && obj.IsActive && (!obj.IsDeleted))).OrderByDescending(a => a.ReceiveDate);
                }
            }
            else if (folderCode.Equals(lkpMessageFolderContext.FOLLOWUP.GetStringValue()))
            {
                if (userGroup.Equals(AppConsts.NONE))
                {
                    return ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE)
                        .Where(obj => ((obj.EntityID == queueOwnerId && (obj.FolderID.Equals(folderId) || obj.IsFollowup) && (!obj.IsDeleted) && obj.IsActive) ||
                            (obj.ADBMessage.From.Value.Equals(queueOwnerId) && obj.ADBMessage.IsFollowUp && (!obj.ADBMessage.IsDeleted) && (obj.ADBMessage.IsActive)))
                                                                                           ).OrderByDescending(a => a.ADBMessage.ReceiveDate);
                }
                else
                {
                    return ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE).Include(SysXEntityConstants.TABLE_ADBMESSAGING_ADBMESSAGETOUSERGROUPS)
                           .Where(obj => (obj.EntityID == queueOwnerId && (obj.FolderID.Equals(folderId) || obj.IsFollowup) && (!obj.IsDeleted) && obj.IsActive && obj.ADBMessage.ADBMessageToUserGroups.Any(adbMsgGroup => adbMsgGroup.UserGroupID.Equals(userGroup))) ||
                                                                                              (UserGroupIds.Contains(obj.ADBMessage.From.Value) && obj.ADBMessage.IsFollowUp && (!obj.ADBMessage.IsDeleted) && (obj.ADBMessage.IsActive))
                                                                                              ).OrderByDescending(a => a.ADBMessage.ReceiveDate);
                }
            }
            else
            {
                if (userGroup.Equals(AppConsts.NONE))
                {
                    return ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE)
                     .Where(obj => obj.EntityID == (queueOwnerId) && obj.FolderID.Equals(folderId) && (!obj.IsDeleted) && obj.IsActive && !(obj.ADBMessage.CommunicationTypeID == communicationTypeId)).OrderByDescending(a => a.ADBMessage.ReceiveDate);
                }
                else
                {
                    return ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE).Include(SysXEntityConstants.TABLE_ADBMESSAGING_ADBMESSAGETOUSERGROUPS)
                     .Where(obj => (obj.EntityID == (queueOwnerId) && obj.FolderID.Equals(folderId) && (!obj.IsDeleted) && obj.IsActive && obj.ADBMessage.ADBMessageToUserGroups.Any(adbMsgGroup => adbMsgGroup.UserGroupID.Equals(userGroup)))
                                                                                      ).OrderByDescending(a => a.ADBMessage.ReceiveDate);
                }
            }
        }



        public IQueryable<OrganizationUser> RetrieveListOfUsers(List<Int32> lstSelectedOrganizationUserIds, Int32 organizationUserId, lkpCommunicationTypeContext communicationTypeContext, Int32 defaultTenantId,
            Int32 selectedTenantId = 0, Int32 selectedProgramId = 0)
        {
            IQueryable<OrganizationUser> orgUsers = null;
            OrganizationUser organizationUser = AppDBContext.OrganizationUsers.FirstOrDefault(orgUser => orgUser.OrganizationUserID.Equals(organizationUserId));

            if (lstSelectedOrganizationUserIds.IsNull())
            {
                orgUsers =
                 AppDBContext.OrganizationUsers
                 .Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                 .Where(user => user.aspnet_Users.aspnet_Membership.IsApproved && !user.IsDeleted && user.IsActive == true);
            }
            else
            {
                orgUsers =
                 AppDBContext.OrganizationUsers
                 .Include(SysXEntityConstants.TABLE_ASPNET_USERS)
                 .Where(user => user.aspnet_Users.aspnet_Membership.IsApproved && !user.IsDeleted && user.IsActive == true && lstSelectedOrganizationUserIds.Contains(user.OrganizationUserID));
            }

            if (!organizationUser.Organization.TenantID.Equals(defaultTenantId))
            {
                orgUsers = orgUsers.Where(usr => usr.Organization.TenantID == organizationUser.Organization.TenantID
                    || usr.Organization.TenantID == defaultTenantId);
            }

            orgUsers = FilterUsers(selectedTenantId, selectedProgramId, orgUsers);

            if (communicationTypeContext == lkpCommunicationTypeContext.EMAIL)
                orgUsers = orgUsers.Where(x => x.IsSubscribeToEmail);

            return orgUsers;
        }

        private IQueryable<OrganizationUser> FilterUsers(Int32 selectedTenantId, Int32 selectedProgramId, IQueryable<OrganizationUser> orgUsers)
        {
            if (selectedTenantId > 0 && selectedProgramId == 0)
            {
                orgUsers = orgUsers.Where(user => user.Organization.TenantID == selectedTenantId
                       && !user.Organization.IsDeleted
                       && user.Organization.IsActive);
            }
            return orgUsers;
        }

        public String GetFolderName(Int32 folderId, String folderCode)
        {
            lkpMessageFolder foldername = AppDBContext.lkpMessageFolders.FirstOrDefault(x => x.MessageFolderID == folderId && x.Code == folderCode);
            if (foldername.IsNotNull())
            {
                return foldername.Name;
            }
            return "NewFolder";
        }

        public Int32 GetMessageCount(Int32 folderId, String folderCode, Int32 queueOwnerId, Int32 communicationTypeId)
        {
            if (folderCode.Equals(lkpMessageFolderContext.DELETEDITEMS.GetStringValue()))
            {
                var res = (
                            (from adb in ADBQueueContext.ADBMessages
                             from lst in ADBQueueContext.ADBMessageToLists.Where(x => x.ADBMessageID == adb.ADBMessageID).DefaultIfEmpty()
                             where (lst.EntityID == queueOwnerId && lst.IsActive && lst.IsDeleted && lst.IsUnread)
                             select adb.ADBMessageID)
                            .Union
                           (from adb in ADBQueueContext.ADBMessages
                            from lst in ADBQueueContext.ADBMessageToLists.Where(x => x.ADBMessageID == adb.ADBMessageID).DefaultIfEmpty()
                            where (adb.From == queueOwnerId && adb.MessageMode == "D" && adb.IsActive && adb.IsUnread && adb.IsDeleted)
                            select adb.ADBMessageID)
                           ).Distinct();
                return res.Count();
            }
            else if (folderCode.Equals(lkpMessageFolderContext.SENTITEMS.GetStringValue()))
            {
                return 0;
            }
            else if (folderCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()))
            {
                return ADBQueueContext.ADBMessages.Where(obj => (obj.MessageMode.Equals("D") && obj.From == queueOwnerId && obj.IsActive && (!obj.IsDeleted))).Count();
            }
            else if (folderCode.Equals(lkpMessageFolderContext.FOLLOWUP.GetStringValue()))
            {
                var res = ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE)
                         .Where(obj => ((obj.EntityID == queueOwnerId && (obj.FolderID.Equals(folderId) || obj.IsFollowup) && (!obj.IsDeleted) && obj.IsActive && obj.IsUnread) ||
                             (obj.ADBMessage.From.Value.Equals(queueOwnerId) && obj.ADBMessage.IsFollowUp && (!obj.ADBMessage.IsDeleted) && (obj.ADBMessage.IsActive) && obj.ADBMessage.IsUnread))
                                                                                            );
                return res.Count();

            }
            else
            {
                var res = ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE)
                      .Where(obj => (obj.EntityID == (queueOwnerId) && obj.FolderID.Equals(folderId) && (!obj.IsDeleted) && obj.IsActive) && !(obj.ADBMessage.CommunicationTypeID == communicationTypeId) && obj.IsUnread
                                                                                   && !(obj.ADBMessage.CommunicationTypeID == communicationTypeId)).Select(x => new
                                                                                   {
                                                                                       messageid = x.ADBMessageID,
                                                                                       entityid = x.EntityID
                                                                                   }).Distinct();
                return res.Count();
            }
        }

        public String GetUserNamesIds(List<Int32> userIdList)
        {
            StringBuilder sbList = new StringBuilder();

            foreach (var userId in userIdList)
            {
                var orgUser = AppDBContext.OrganizationUsers.Where(x => x.OrganizationUserID == userId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                if (orgUser.IsNotNull())
                {
                    //sbList.Append(AppDBContext.OrganizationUsers.Where(x => x.OrganizationUserID == userId && x.IsActive == true).FirstOrDefault().FirstName + ":" + userId + ";");
                    sbList.Append(orgUser.FirstName +" "+ orgUser.LastName + ":" + userId + ";");
                }
            }

            return Convert.ToString(sbList);
        }

        public Boolean CheckSubscriptionStatus(Int32 queueOwnerId)
        {
            OrganizationUser user = AppDBContext.OrganizationUsers.FirstOrDefault(obj => obj.OrganizationUserID == queueOwnerId);
            return user.IsSubscribeToEmail;
        }

        public bool UpdateSubscriptionStatus(int queueOwnerId, bool status)
        {
            OrganizationUser user = AppDBContext.OrganizationUsers.FirstOrDefault(obj => obj.OrganizationUserID == queueOwnerId);
            user.IsSubscribeToEmail = status;
            AppDBContext.SaveChanges();
            return true;
        }

        public List<lkpCommunicationType> GetCommuncationTypes(Guid userId, Permissions permission, List<lkpCommunicationType> lstCommunicationType)
        {
            IQueryable<Guid> userRoles =
                APPDbContext.vw_aspnet_UsersInRoles
                .Where(aspnet_UsersInRole => aspnet_UsersInRole.UserId.Equals(userId))
                .Select(aspnet_UsersInRole => aspnet_UsersInRole.RoleId);


            string per = permission.ToString();

            List<int> communicationTypeIds =
                APPDbContext.TemplatePermissions
                .Where(templatePermisison =>
                    userRoles.Contains(templatePermisison.RoleID)
                    && templatePermisison.Permission.Name.Equals(per))
                    .Select(templatePermisison => templatePermisison.CommunicationTypeID)
                    .Distinct()
                    .ToList();

            return lstCommunicationType.Where(condition => communicationTypeIds.Contains(condition.CommunicationTypeID)).ToList();
        }

        public Dictionary<string, string> SaveDocumentAndGetDocumentId(List<ADBMessageDocument> documents)
        {
            Dictionary<string, string> documentIds = new Dictionary<string, string>();
            if (!documents.IsNullOrEmpty())
            {
                foreach (var item in documents)
                {
                    ADBMessageDocument messageDocument = new ADBMessageDocument();
                    messageDocument.DocumentName = item.DocumentName;
                    messageDocument.OriginalDocumentName = item.OriginalDocumentName;
                    messageDocument.IsActive = true;
                    messageDocument.IsDeleted = false;
                    messageDocument.DocumentSize = item.DocumentSize;
                    ADBMessageQueueContext.ADBMessageDocuments.AddObject(messageDocument);
                    ADBMessageQueueContext.SaveChanges();
                    documentIds.Add(Convert.ToString(messageDocument.DocumentId), item.OriginalDocumentName);
                }
            }
            return documentIds;
        }

        public List<OrganizationLocation> GetInstitutionLocations(Int32 institutionId, Int32 currentUserId, Int32 currentTenantId, Int32 defaultTenantId)
        {
            List<OrganizationLocation> lst = AppDBContext.OrganizationLocations.ToList();
            List<Organization> organizations = new List<Organization>();

            if (institutionId.Equals(AppConsts.NONE))
                organizations = AppDBContext.Organizations.ToList();
            else
                organizations = AppDBContext.Organizations.Where(org => org.TenantID == institutionId).ToList();

            List<OrganizationLocation> lstOrganizationLocations = new List<OrganizationLocation>();
            foreach (var org in organizations)
            {
                lstOrganizationLocations.AddRange(lst.Where(loc => loc.OrganizationID == org.OrganizationID).ToList());
            }
            // Further filter the list of the organization locations in case this is client admin
            if (defaultTenantId != currentTenantId)
            {
                Int32 _organizationId = AppDBContext.OrganizationUsers.Where(orgUser => orgUser.OrganizationUserID == currentUserId).FirstOrDefault().OrganizationID;
                lstOrganizationLocations = lstOrganizationLocations.Where(orgLocations => orgLocations.OrganizationID == _organizationId).ToList();
            }

            return lstOrganizationLocations;
        }

        public Int32 GetFolderIdByCode(String folderCode)
        {
            return AppDBContext.lkpMessageFolders.WhereSelect(code => code.Code.ToLower().Trim().Equals(folderCode.ToLower().Trim())).FirstOrDefault().MessageFolderID;
        }

        public List<ADBMessageDocument> GetMessageAttachment(Guid messageId)
        {
            ADBMessage adbMessageDetails = ADBMessageQueueContext.ADBMessages.Where(x => x.ADBMessageID == messageId).FirstOrDefault();
            List<ADBMessageDocument> messageDocument = new List<ADBMessageDocument>();
            if (adbMessageDetails.IsNotNull() && !String.IsNullOrEmpty(adbMessageDetails.DocumentName))
            {
                List<Int32> documentIdList = new List<Int32>();
                String[] documentId = adbMessageDetails.DocumentName.Split(';');
                documentId.ForEach(x => documentIdList.Add(Convert.ToInt32(x.Equals("") ? "0" : x)));
                messageDocument = ADBMessageQueueContext.ADBMessageDocuments.Where(x => documentIdList.Contains(x.DocumentId)).ToList();
            }
            return messageDocument;
        }

        public List<MessagingGroup> GetGroupFolderList(List<aspnet_Roles> roleList)
        {
            List<Int32> allGroupIDs = new List<Int32>();
            List<MessagingGroup> messagingGroups = new List<MessagingGroup>();
            foreach (aspnet_Roles role in roleList)
            {
                IEnumerable<Int32> groupIDs = AppDBContext.MessagingGroupRoles.Where(obj => obj.RoleID == role.RoleId).Select(con => con.MessagingGroupID);
                allGroupIDs.AddRange(groupIDs);
            }
            foreach (var groupID in allGroupIDs.Distinct())
            {
                MessagingGroup messagingGroup = AppDBContext.MessagingGroups.Where(x => x.MessagingGroupID == groupID).FirstOrDefault();
                messagingGroups.Add(messagingGroup);
            }
            return messagingGroups;
        }

        #region Message Group

        /// <summary>
        /// Gets messaging groups
        /// </summary>
        /// <returns></returns>
        public IQueryable<vw_ListOfUsers> GetMessagingGroups(Int32 organizationUserId)
        {
            Int32 orgnizationID = AppDBContext.OrganizationUsers.FirstOrDefault(cond => cond.OrganizationUserID == organizationUserId).OrganizationID;
            return AppDBContext.vw_ListOfUsers.Where(cond => (cond.OrgID == orgnizationID && cond.IsMessagingGroup == 0) || cond.IsMessagingGroup == 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagingGroupIds"></param>
        /// <returns></returns>
        public IEnumerable<MessagingGroup> GetMessagingGroups(List<Int32> messagingGroupIds)
        {
            return APPDbContext.MessagingGroups
                .Where(messagingGroup => !messagingGroup.IsDeleted
                && messagingGroupIds.Contains(messagingGroup.MessagingGroupID));
        }

        /// <summary>
        /// Gets list of email ids
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public IEnumerable<String> GetEmailIdsByGroupIds(List<Int32> groupIds)
        {
            IEnumerable<Guid> roleIds =
            APPDbContext.MessagingGroupRoles.Where(messagingGroupRole => groupIds.Contains(messagingGroupRole.MessagingGroupID))
                .Select(messagingGroupRole => messagingGroupRole.RoleID).ToList();

            IEnumerable<Guid> userIds =
            APPDbContext.vw_aspnet_UsersInRoles.Where(aspnet_UserInRole => roleIds.Contains(aspnet_UserInRole.RoleId))
                .Select(aspnet_UserInRole => aspnet_UserInRole.UserId);

            return APPDbContext.OrganizationUsers.Include(SysXEntityConstants.TABLE_MESSAGING_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP)
                .Where(organizationUser => userIds.Contains(organizationUser.UserID))
                .Select(organizationUser => organizationUser.aspnet_Users.aspnet_Membership.Email);
        }


        #endregion
        public Int32 GetGroupFolderCount(Int32 groupID, String foldeCode)
        {
            return ADBMessageQueueContext.ADBMessageToUserGroups.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE).Where(obj => obj.UserGroupID.Equals(groupID) && !obj.ADBMessage.IsDeleted && obj.ADBMessage.IsActive && obj.ADBMessage.MessageMode == "S" && obj.ADBMessage.IsUnread).Count();
        }

        public List<OrganizationUser> GetOrganizationUsers(List<Int32> userIdList)
        {
            var users = AppDBContext.OrganizationUsers.Include(SysXEntityConstants.TABLE_MESSAGING_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP).Where(obj => userIdList.Contains(obj.OrganizationUserID)).Distinct();
            return users.ToList();
        }

        #region Dashboard Messages

        public IQueryable<ADBMessageToList> GetRecentMessages(String folderCode, Int32 queueOwnerId, Int32 communicationTypeId)
        {
            Int32 folderId = GetFolderIdByCode(folderCode);

            return ADBQueueContext.ADBMessageToLists.Include(SysXEntityConstants.TABLE_MESSAGING_ADBMESSAGE)
             .Where(msg => msg.EntityID == queueOwnerId && msg.FolderID == folderId && !msg.IsDeleted && msg.IsActive && msg.IsUnread && !(msg.ADBMessage.CommunicationTypeID == communicationTypeId)).OrderByDescending(a => a.ADBMessage.ReceiveDate).Take(5);
        }

        #endregion


        /// <summary>
        /// Gets the list of the Tenants for the message rules 
        /// </summary>
        /// <returns>List of the tenants.</returns>
        public List<Tenant> GetTenantsForRules(Int32 currentTenantId, Int32 defaultTenantId)
        {
            if (currentTenantId != defaultTenantId)
            {
                return AppDBContext.Tenants.Where(tenant => tenant.TenantID == currentTenantId || tenant.TenantID == defaultTenantId).ToList();
            }
            return AppDBContext.Tenants.Where(tenant => !tenant.IsDeleted && tenant.IsActive).ToList();
        }

        public IQueryable<T> PerformSearch<T>(Dictionary<String, String> searchOptions)
        {
            //To retrieve the EntitySet from Context
            String entitySetName = ADBMessageQueueContext.MetadataWorkspace.GetEntityContainer(ADBMessageQueueContext.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                .Where(bes => bes.ElementType.Name.Equals(typeof(T).Name)).FirstOrDefault().Name;

            return (new ObjectQuery<T>(entitySetName, ADBMessageQueueContext, MergeOption.NoTracking).AdvanceTextSearch(searchOptions));
        }

        /// <summary>
        /// Invoked to delete group messages
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public Boolean DeleteADBgroupMesssage(Guid messageId, Int32 userGroupId)
        {
            ADBMessageToUserGroup messageToBeDeleted = ADBMessageQueueContext.ADBMessageToUserGroups.FirstOrDefault(condition => condition.ADBMessageID == messageId
                                                                                        && condition.UserGroupID == userGroupId && condition.IsActive == true
                                                                                        && condition.IsDeleted == false);
            if (messageToBeDeleted != null)
            {
                messageToBeDeleted.IsDeleted = true;
                ADBMessageQueueContext.SaveChanges();
                return true;
            }
            return false;
        }

        public void SendMailToExternalGroup(String toGroupIds, String ccGroupIds, Boolean isHighInportance, String subject, String messageBody, String fromEmailId, Int32 externaMessagingGroupTypeId, Int32 senderUserId)
        {
            String[] toGropuIdList = { };
            String[] ccGropuIdsList = { };
            List<Int32> gropuIdList = new List<Int32>();

            if (!toGroupIds.IsNullOrEmpty())
                toGropuIdList = toGroupIds.Split(';');
            if (!ccGroupIds.IsNullOrEmpty())
                ccGropuIdsList = ccGroupIds.Split(';');
            if (!toGropuIdList.IsNullOrEmpty())
            {
                foreach (string gropuId in toGropuIdList)
                {
                    int i;
                    if (int.TryParse(gropuId.Trim(), out i))
                    {
                        gropuIdList.Add(i);
                    }
                }
            }
            if (!ccGroupIds.IsNullOrEmpty())
            {
                foreach (string gropuId in ccGropuIdsList)
                {
                    int i;
                    if (int.TryParse(gropuId.Trim(), out i))
                    {
                        gropuIdList.Add(i);
                    }
                }
            }
            if (gropuIdList != null && gropuIdList.Count > AppConsts.NONE)
            {
                List<MessagingGroup> externalMessagingGroup = AppDBContext.MessagingGroups.Where(x => gropuIdList.Contains(x.MessagingGroupID)
                                                                                         && x.MesssagingGroupTypeId == externaMessagingGroupTypeId).ToList();
                if (externalMessagingGroup.IsNotNull() && externalMessagingGroup.Count > AppConsts.NONE)
                {
                    String senderName = String.Empty;
                    var senderUser = AppDBContext.OrganizationUsers.FirstOrDefault(x => x.OrganizationUserID == senderUserId && !x.IsDeleted);
                    if (senderUserId.IsNotNull())
                    {
                        senderName = senderUser.FirstName + " " + senderUser.LastName;
                    }
                    foreach (MessagingGroup messagingGroup in externalMessagingGroup)
                    {
                        List<MessagingGroupEmailMapping> messagingGroupEmailMappingList = AppDBContext.MessagingGroupEmailMappings.Where(x => x.MGEM_GroupId == messagingGroup.MessagingGroupID
                                                                                                                                        && !x.MGEM_IsDeleted).ToList();
                        if (messagingGroupEmailMappingList != null && messagingGroupEmailMappingList.Count > AppConsts.NONE)
                        {
                            EMailMessage newMailMessage = new EMailMessage();
                            newMailMessage.ToAddresses = String.Join(";", messagingGroupEmailMappingList.Select(x => x.MGEM_EmailAddress));
                            newMailMessage.EmailPriority = isHighInportance ? "High" : "Normal";
                            newMailMessage.Subject = subject;
                            SendMail(newMailMessage, messagingGroup.GroupName, messageBody, fromEmailId, senderName);
                        }
                    }
                }
            }
        }

        private bool SendMail(EMailMessage email, String groupName, String messageBody, String fromEmailId, String senderName)
        {
            //SmtpClient smtpClient = new SmtpClient();
            Guid temp = Guid.NewGuid();
            Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            //System.Net.Configuration.MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as System.Net.Configuration.MailSettingsSectionGroup;
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    //MailAddress fromAddress = new MailAddress(fromEmailId, senderName);
                    //smtpClient.Host = Convert.ToString(mailSettings.Smtp.Network.Host);
                    //smtpClient.Port = Convert.ToInt32(mailSettings.Smtp.Network.Port);

                    //if (mailSettings.Smtp.Network.UserName != null && mailSettings.Smtp.Network.Password != null)
                    //    smtpClient.Credentials = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);

                    //message.From = fromAddress;
                    foreach (string emailAddress in email.ToAddresses.Split(';'))
                        message.To.Add(emailAddress);
                    message.Subject = email.Subject;
                    message.IsBodyHtml = true;
                    message.Priority = GetEmailPriority(email.EmailPriority);
                    message.Body = messageBody;
                    DALUtils.LoggerService.GetLogger().Info(DateTime.Now.ToString() + ": " + " : sending email " + temp);
                    Intsof.SMTPService.SMTPService smtpService = new Intsof.SMTPService.SMTPService();
                    smtpService.SendMail(message);
                    //smtpClient.Send(message);
                    DALUtils.LoggerService.GetLogger().Info(DateTime.Now.ToString() + " : sent email " + temp);
                }
            }
            catch (Exception ex)
            {
                DALUtils.LoggerService.GetLogger().Error(String.Format("An Error has occured sending the mail, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace));
            }
            return true;
        }

        /// <summary>
        /// Set EMail Priority 
        /// </summary>
        /// <param name="emailPriority"></param>
        /// <returns>MailPriority</returns>
        private MailPriority GetEmailPriority(string emailPriority)
        {
            if (emailPriority.Equals("Normal"))
                return MailPriority.Normal;
            if (emailPriority.Equals("High"))
                return MailPriority.High;
            if (emailPriority.Equals("Low"))
                return MailPriority.Low;
            return MailPriority.Normal;
        }

        List<SystemCommunication> IMessageRepository.GetSystemCommunicationByIds(List<Int32> lstSysCommunicationId)
        {
            return ADBMessageQueueContext.SystemCommunications.Include("lkpCommunicationSubEvent")
                                         .Where(cond => lstSysCommunicationId.Contains(cond.SystemCommunicationID)).ToList();
        }

        SystemCommunication IMessageRepository.GetSystemCommunicationById(Int32 SysCommunicationId)
        {
            return ADBMessageQueueContext.SystemCommunications.Include("lkpCommunicationSubEvent")
                                                                .Include("SystemCommunicationDeliveries")
                                                                .Include("SystemCommunicationAttachments")
                                         .Where(cond => cond.SystemCommunicationID == SysCommunicationId).FirstOrDefault();
        }

        Boolean IMessageRepository.AddSystemCommunicationObject(SystemCommunication newSystemCommunication)
        {
            ADBMessageQueueContext.SystemCommunications.AddObject(newSystemCommunication);
            ADBMessageQueueContext.SaveChanges();
            return true;
        }

        SystemCommunication IMessageRepository.GetSystemCommunicationForMailData(Int32 systemCommunicationID)
        {
            return ADBMessageQueueContext.SystemCommunications.Where(cond => cond.SystemCommunicationID == systemCommunicationID).FirstOrDefault();
        }

        #region UAT-3261: Badge Form Enhancements
        public List<SystemCommunicationAttachment> GetSystemCommunicationAttachment(Int32 SystemCommId)
        {
            SystemCommunicationAttachment adbSystemCommDetails = ADBMessageQueueContext.SystemCommunicationAttachments.Where(x => x.SCA_SystemCommunicationID == SystemCommId).FirstOrDefault();
            List<SystemCommunicationAttachment> emailDocument = new List<SystemCommunicationAttachment>();
            if (adbSystemCommDetails.IsNotNull() && !String.IsNullOrEmpty(adbSystemCommDetails.SCA_OriginalDocumentName))
            {
                List<Int32> documentIdList = new List<Int32>();
                emailDocument = ADBMessageQueueContext.SystemCommunicationAttachments.Where(x => adbSystemCommDetails.SCA_SystemCommunicationID == x.SCA_SystemCommunicationID).ToList();
            }
            return emailDocument;
        }
        #endregion


        #region UAT-3820
        public String SaveDocumentReceivedFromStudentServiceFormStatus(ADBMessageDocument documents)
        {
            String documentId = String.Empty;
            if (!documents.IsNullOrEmpty())
            {
                    ADBMessageDocument messageDocument = new ADBMessageDocument();
                    messageDocument.DocumentName = documents.DocumentName;
                    messageDocument.OriginalDocumentName = documents.OriginalDocumentName;
                    messageDocument.IsActive = true;
                    messageDocument.IsDeleted = false;
                    messageDocument.DocumentSize = documents.DocumentSize;
                    ADBMessageQueueContext.ADBMessageDocuments.AddObject(messageDocument);
                    ADBMessageQueueContext.SaveChanges();
                    documentId = Convert.ToString(messageDocument.DocumentId);                
            }
            return documentId;
        }
        #endregion
    }
}
