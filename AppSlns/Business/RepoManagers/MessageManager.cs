using Business.ReportExecutionService;
using DAL.Repository;
using Entity;
using INTSOF.UI.Contract.Messaging;
using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace Business.RepoManagers
{
    public class MessageManager
    {
        public static Int32 GetMessageCount(Int32 folderId, String folderCode, Int32 queueOwnerId)
        {
            try
            {
                String communicationTypeCode = lkpCommunicationTypeContext.EMAIL.GetStringValue();
                Int32 communicationTypeId = GetCommuncationTypeByCode(communicationTypeCode).CommunicationTypeID;
                Int32 count = BALUtils.GetMessageRepoInstance().GetMessageCount(folderId, folderCode, queueOwnerId, communicationTypeId);
                return count;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return 0;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return 0;
            }
        }

        public static String GetFolderName(Int32 folderId, String folderCode)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetFolderName(folderId, folderCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return String.Empty;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return String.Empty;
            }
        }

        public static Boolean SendMessage(MessagingContract messagingContract)
        {
            try
            {
                if (!messagingContract.IsNullOrEmpty())
                {
                    var externalMessagingGroup = LookupManager.GetLookUpData<lkpMessagingGroupType>().Where(condition => condition.MGT_Code == "AAAB").FirstOrDefault();
                    Int32 externamMessageGroupTypeId = AppConsts.NONE;
                    if (externalMessagingGroup.IsNotNull())
                    {
                        externamMessageGroupTypeId = externalMessagingGroup.MGT_ID;
                    }
                    Int32 isNewMail = messagingContract.Action == MessagingAction.NewMail || messagingContract.Action == MessagingAction.Forward ? 1 : 0;

                    switch (messagingContract.Action)
                    {
                        case MessagingAction.NewMail:
                        case MessagingAction.Forward:
                        case MessagingAction.Reply:
                        case MessagingAction.ReplyAll:
                            if (SendMessageToQueue(messagingContract))
                            {
                                String messageMode = messagingContract.MessageMode;
                                String toGroupIds = messagingContract.ToUserGroupIds;
                                String ccGroupIds = messagingContract.CCUserGroupIds;
                                if (messageMode.Equals("S") && (!toGroupIds.IsNullOrEmpty() || !ccGroupIds.IsNullOrEmpty()))
                                {
                                    BALUtils.GetMessageRepoInstance().SendMailToExternalGroup(toGroupIds, ccGroupIds, messagingContract.IsHighImportance,
                                        messagingContract.Subject, messagingContract.Content, messagingContract.From, externamMessageGroupTypeId, messagingContract.CurrentUserId);

                                }
                                return true;
                            }
                            return false;
                        case MessagingAction.Draft:
                            //  String formattedMessage = FormatMessage(messagingContract.MessageId, isNewMail, messagingContract.MessageMode, messagingContract.ToUserIds, messagingContract.CCUserGroupIds, messagingContract.FolderId, messagingContract.IsHighImportance, messagingContract.CurrentUserId, messagingContract.Subject, messagingContract.QueueType, messagingContract.Action, messagingContract.TenantTypes, messagingContract.Content, messagingContract.MessageType, messagingContract.ToUserGroupIds, messagingContract.From,messagingContract.toUserList);
                            String formattedMessage = FormatMessage(messagingContract);
                            return MessageManager.SaveDraftedMesssage(
                                messagingContract.MessageId,
                                formattedMessage,
                                messagingContract.MessageMode,
                                messagingContract.CurrentUserId,
                                messagingContract.ToUserIds,
                                messagingContract.CcUserIds,
                                messagingContract.ToUserGroupIds,
                                messagingContract.CCUserGroupIds,
                                messagingContract.Content,
                                messagingContract.Subject,
                                messagingContract.toUserList,
                                messagingContract.CcUserList,
                                messagingContract.IsHighImportance,
                                messagingContract.DocumentName,
                                messagingContract.OriginalDocumentName,
                                messagingContract.BccUserIds,
                                messagingContract.From,
                                externamMessageGroupTypeId,
                                messagingContract.Content
                        );

                        default:
                            return false;
                    }
                }
                else
                    return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
        }

        /// <summary>
        /// Invoked to send message.
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="initiator"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Boolean SendMessageToQueue(MessagingContract messagingContract)
        {
            try
            {
                QueueManager obj = new QueueManager();
                return obj.SendMessageToQueue(messagingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;

        }

        /// <summary>
        /// Invoked to save the drafted messages.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <param name="messageMode"></param>
        /// <param name="subject"></param>
        /// <param name="toUsers"></param>
        /// <param name="ccUsers"></param>
        /// <returns></returns>
        public static Boolean SaveDraftedMesssage(
            Guid messageId,
            String formattedMessage,
            String messageMode,
            Int32 organizationUserId,
            String toUserIds,
            String ccUserIds,
            String toGroupIds,
            String ccGroupIds,
            String content,
            String subject,
            String toUserList,
            String ccUserList,
            Boolean isHighImportance,
            String documentName,
            String originalDocumentName,
            String bccUserIds, String from,
            Int32 externamMessageGroupTypeId,
            String messageContent)
        {

            try
            {
                BALUtils.GetMessageRepoInstance().SaveDraftedMesssage(
                    messageId,
                    formattedMessage,
                    messageMode,
                    subject,
                    organizationUserId,
                    toUserIds,
                    ccUserIds,
                    toGroupIds,
                    ccGroupIds,
                    isHighImportance,
                    documentName,
                    originalDocumentName,
                    bccUserIds,
                    from,
                    externamMessageGroupTypeId,
                      messageContent
                    );

                if (messageMode == "S")
                {
                    CommunicationManager.SendInternalMessageMail(toUserIds, ccUserIds, bccUserIds);
                }

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// Invoked to delete the message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <param name="folderID"></param>
        /// <returns></returns>
        public static Boolean DeleteMesssage(Guid messageId, Int32 userId, String folderCode, Int32 queueTypeID = 0)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().DeleteMesssage(messageId, userId, folderCode, queueTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Boolean DeleteMessageFromDashboard(Guid messageId, Int32 userId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().DeleteMesssageFromDashboard(messageId, userId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static Boolean DeleteMesssage(Guid messageId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().DeleteMesssage(messageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// Ddelete folder and its messages.
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="messageIds"></param>
        /// <param name="userId"></param>
        /// <param name="queueTypeID"></param>
        /// <returns>boolean</returns>
        public static Boolean DeleteFolderAndMesssage(Int32 folderId, Int32 userId, Int32 queueTypeID)
        {
            try
            {
                Boolean result = true;
                List<Int32> foldersToBeDeleted = BALUtils.GetMessageRepoInstance().GetAllFoldersToBedeleted(folderId, userId);
                BALUtils.GetMessageRepoInstance().DeleteMessageRulesintransaction(foldersToBeDeleted, userId);
                BALUtils.GetMessageRepoInstance().DeleteAllMessagesintransaction(foldersToBeDeleted, userId);
                BALUtils.GetMessageRepoInstance().DeleteAllFolders(foldersToBeDeleted, userId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Boolean CheckIfFolderNeedToBeRestored(Guid messageId, Int32 userId, Int32 queueTypeID)
        {
            try
            {
                List<Int32> foldersToBeRestored = BALUtils.GetMessageRepoInstance().GetAllFoldersToBeRestored(messageId, userId);
                if (foldersToBeRestored.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static String FoldersToBeRestored(Guid messageId, Int32 userId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().FoldersToBeRestored(messageId, userId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return String.Empty;
        }

        /// <summary>
        /// Invoked to restore the message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <param name="folderID"></param>
        /// <returns></returns>
        public static Boolean RestoreMessage(Guid messageId, Int32 userId, Int32 queueTypeID)
        {
            try
            {
                List<Int32> foldersToBeRestored = BALUtils.GetMessageRepoInstance().GetAllFoldersToBeRestored(messageId, userId);
                if (foldersToBeRestored.Count() > 0)
                {
                    BALUtils.GetMessageRepoInstance().RestoreAllFolders(foldersToBeRestored, userId);
                }
                return BALUtils.GetMessageRepoInstance().RestoreMessage(messageId, userId, queueTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// Invoked to move the message from one folder to another.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <param name="folderID"></param>
        /// <param name="moveToFolderId"></param>
        /// <returns></returns>
        public static Boolean SetMoveToFolder(Guid messageId, Int32 userId, String folderCode, Int32 moveToFolderId, String moveToFolderCode, Int32 queueTypeID)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().SetMoveToFolder(messageId, userId, folderCode, moveToFolderId, moveToFolderCode, queueTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// Invoked to add the new folder.
        /// </summary>
        /// <param name="nodeText"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static String AddNewFolder(String nodeText, Int32 currentUserID, Int32 userGroup, Int32 parentFolderID)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().AddNewFolder(nodeText, currentUserID, userGroup, parentFolderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return String.Empty;
        }

        /// <summary>
        /// Invoked to update the follow up status for message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="queueTypeID"></param>
        /// <param name="isFollowUp"></param>
        /// <returns></returns>
        public static Boolean UpdateFollowUpStatus(Guid messageId, Int32 queueOwnerId, Int32 queueTypeID, Boolean isFollowUp, String folderCode)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().UpdateFollowUpStatus(messageId, queueOwnerId, queueTypeID, isFollowUp, folderCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// Invoked to update the message read status.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="queueTypeID"></param>
        /// <param name="isUnread"></param>
        /// <returns></returns>
        public static Boolean UpdateReadStatus(Guid messageId, Int32 queueOwnerId, Int32 queueTypeID, Boolean isUnread)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().UpdateReadStatus(messageId, queueOwnerId, queueTypeID, isUnread);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Boolean UpdateMessage(ADBMessage message)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().UpdateMessage(message);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static String GetUniqueTemplateName(Guid adbMessageId, Int32 from, String templateName)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetUniqueTemplateName(adbMessageId, from, templateName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return string.Empty;
        }

        /// <summary>
        /// Invoked to get the messagecontent.
        /// </summary>
        /// <param name="queueType"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static EntityObject GetMessageContents(Int32 queueType, Guid messageId, Int32 currentUserId = 0, Boolean isDashboarMessage = false)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetMessageContents(queueType, messageId, currentUserId, isDashboarMessage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// Invoked to get the messagecontent.
        /// </summary>
        /// <param name="queueType"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static EntityObject GetMessageContentsForApplicant(Int32 queueType, Guid messageId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetMessageContentsForApplicant(queueType, messageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// Invoked to get the messagecontent.
        /// </summary>
        /// <param name="queueType"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static ComplexObject GetCommonMessageContents(Int32 queueType, Guid messageId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetCommonMessageContents(queueType, messageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// Invoked to get the emaild.
        /// </summary>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        public static List<String> GetEmailId(List<Int32> userIdList)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetEmailId(userIdList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<String>();
        }

        /// <summary>
        /// Invoked to get the emaild.
        /// </summary>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        public static String GetEmailId(Int32 userId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetEmailId(userId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return String.Empty;
        }

        /// <summary>
        /// Invoked to get the manipulated emailidlist.
        /// </summary>
        /// <param name="emailList"></param>
        /// <returns></returns>
        public static String GetUserIDList(String[] emailList)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetUserIDList(emailList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return String.Empty;
        }

        /// <summary>
        /// Invoked to get the manipulated UserGroupIDs list.
        /// </summary>
        /// <param name="emailList"></param>
        /// <returns></returns>
        public static String GetUserGroupIdList(String[] organizationusrId, Dictionary<Int32, String> tanentType, Int32 msgType)
        {
            try
            {
                switch ((TenantTypeEnum)tanentType.FirstOrDefault().Key)
                {
                    case TenantTypeEnum.Client:
                        return BALUtils.GetMessageRepoInstance().GetUserGroupIdList(organizationusrId, true, msgType);

                    case TenantTypeEnum.Supplier:
                        return BALUtils.GetMessageRepoInstance().GetUserGroupIdList(organizationusrId, false, msgType);

                    default:
                        return String.Empty;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static String GetUserGroupIdBySupplierId(String[] supplierId, Int32 messageType)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetUserGroupIdBySupplierId(supplierId, messageType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Messaging company

        /// <summary>
        /// Retrieve Users.
        /// </summary>
        /// <returns>list of active Users</returns>
        public static IQueryable<OrganizationUser> RetrieveUsers(List<Int32> lstSelectedOrganizationUserIds, Int32 organizationUserId, lkpCommunicationTypeContext communicationTypeContext, Int32 selectedTenantId = 0, Int32 selectedProgramId = 0)
        {
            try
            {
                IQueryable<OrganizationUser> organizationUsers = BALUtils.GetMessageRepoInstance().RetrieveListOfUsers(lstSelectedOrganizationUserIds, organizationUserId, communicationTypeContext, SecurityManager.DefaultTenantID, selectedTenantId, selectedProgramId);
                return organizationUsers;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetUserIDListByEmployeeIds(String[] employeeIds)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetUserIDListByEmployeeIds(employeeIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion


        #region Private Methods
        /// <summary>
        /// Invoked to get the message details for sent items.
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        private static List<MessageDetail> GetADBMessageDetailsForDraftItems(IEnumerable<EntityObject> messages)
        {
            List<MessageDetail> messageDetails = new List<MessageDetail>();
            String messageSizeNode = String.Empty;

            foreach (EntityObject msg in messages)
            {
                ADBMessage item = (ADBMessage)msg;
                String email = String.Empty;
                String toEmail = String.Empty;
                MessageDetail message = new MessageDetail();
                message.MessageDetailID = item.ADBMessageID;
                message.Subject = item.Subject;
                message.ReceivedDate = Convert.ToDateTime(item.ReceiveDate);
                message.ReceivedDateString = item.ReceiveDate.ToString();
                message.IsFollowUp = item.IsFollowUp;
                message.IsHighImportant = item.IsHighImportant;
                message.CommunicationTypeCode = item.lkpCommunicationType == null ? string.Empty : item.lkpCommunicationType.Code;
                message.CommunicationType = item.lkpCommunicationType == null ? string.Empty : item.lkpCommunicationType.Name;
                message.IsUnread = item.IsUnread;
                messageSizeNode = item.Message.GetNodeContent("MessageSize");

                if (messageSizeNode.IsNullOrEmpty())
                {
                    message.Size = 1024;
                    message.SizeIn = "1 KB";
                }
                else
                {
                    message.Size = Convert.ToInt32(messageSizeNode);
                    message.SizeIn = SysXUtils.MessageSize(Convert.ToInt32(messageSizeNode));
                }

                if (!item.From.Equals(DBNull.Value))
                    email = GetEmailId(Convert.ToInt32(item.From));
                message.From = email.IsNullOrEmpty() ? item.Message.GetNodeContent("From") : email;
                toEmail = item.Message.GetNodeContent("ToUserList");
                message.To = toEmail.IsNullOrEmpty() ? String.Empty : toEmail;

                String ccEmail = item.Message.GetNodeContent("CCUserList");
                message.Cc = ccEmail.IsNullOrEmpty() ? String.Empty : ccEmail;
                message.HasAttachment = item.DocumentName.IsNullOrEmpty() ? false : true;
                message.FromUserId = item.From.IsNotNull() ? item.From.Value : 0;
                message.SenderUser = message.From;
                message.MessageBody = item.Message.GetNodeContent("MessageBody");
                messageDetails.Add(message);
            }
            return messageDetails;
        }

        /// <summary>
        /// Invoked to get the ADB message details.
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        private static List<MessageDetail> GetADBMessageDetails(IEnumerable<EntityObject> messages, String folderCode)
        {
            List<MessageDetail> messageDetails = new List<MessageDetail>();
            String messageSizeNode = String.Empty;
            foreach (EntityObject msg in messages)
            {
                ADBMessageToList item = (ADBMessageToList)msg;
                if (!messageDetails.Exists(m => m.MessageDetailID.Equals(item.ADBMessageID)))
                {
                    MessageDetail message = new MessageDetail();
                    message.MessageDetailID = item.ADBMessage.ADBMessageID;
                    message.Subject = item.ADBMessage.Subject;
                    message.ReceivedDate = Convert.ToDateTime(item.ADBMessage.ReceiveDate);
                    message.ReceivedDateString = item.ADBMessage.ReceiveDate.ToString();
                    message.ReceivedDateFormat = String.Format("{0:MM/dd/yyyy HH:mm tt}", item.ADBMessage.ReceiveDate);
                    if (item.ConversationHandleID == (Guid.Empty))// checks whether message was deleted from draft
                    {
                        message.IsUnread = item.ADBMessage.IsUnread;
                    }

                    else
                    {
                        message.IsUnread = item.IsUnread;
                    }

                    message.IsHighImportant = item.ADBMessage.IsHighImportant;
                    message.CommunicationType = item.ADBMessage.lkpCommunicationType == null ? string.Empty : item.ADBMessage.lkpCommunicationType.Name;

                    message.CommunicationTypeCode = item.ADBMessage.lkpCommunicationType == null ? string.Empty : item.ADBMessage.lkpCommunicationType.Code;
                    messageSizeNode = item.ADBMessage.Message.GetNodeContent("MessageSize");

                    if (messageSizeNode.IsNullOrEmpty())
                    {
                        message.Size = 1024;
                        message.SizeIn = "1 KB";
                    }
                    else
                    {
                        message.Size = Convert.ToInt32(messageSizeNode);
                        message.SizeIn = SysXUtils.MessageSize(Convert.ToInt32(messageSizeNode));
                    }

                    if (folderCode.Equals(lkpMessageFolderContext.FOLLOWUP.GetStringValue()))
                        message.IsFollowUp = true;
                    else if (folderCode.Equals(lkpMessageFolderContext.SENTITEMS.GetStringValue()))
                        message.IsFollowUp = item.ADBMessage.IsFollowUp;
                    else
                        message.IsFollowUp = item.IsFollowup;


                    if (!item.EntityID.Equals(DBNull.Value))
                    {
                        String toEmail = item.ADBMessage.Message.GetNodeContent("ToUserList");
                        message.To = toEmail.IsNullOrEmpty() ? String.Empty : toEmail;
                    }
                    String ccEmail = item.ADBMessage.Message.GetNodeContent("CCUserList");
                    message.Cc = ccEmail.IsNullOrEmpty() ? String.Empty : ccEmail;

                    if (!item.ADBMessage.From.Equals(DBNull.Value))
                    {
                        String eMail = item.ADBMessage.Message.GetNodeContent("From");
                        message.From = eMail.IsNullOrEmpty() ? String.Empty : eMail;
                        message.FromUserId = item.ADBMessage.From.Value;
                        message.SenderUser = message.From;
                    }

                    message.MessageBody = item.ADBMessage.Message.GetNodeContent("MessageBody");
                    message.HasAttachment = item.ADBMessage.DocumentName.IsNullOrEmpty() ? false : true;

                    messageDetails.Add(message);
                }
            }
            return messageDetails;
        }

        private static String FormatMessage(MessagingContract messagingContract)
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

            String highImportance = messagingContract.IsHighImportance ? "1" : "0";

            String messageBody = messagingContract.Content;

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
            message.Append("<MessageDate>" + DateTime.Now.ToString("MM/dd/yyyy H:mm:ss") + " </MessageDate><MessageSubject>" + "<![CDATA[" + messagingContract.Subject.Trim() + " ]]>" + "</MessageSubject>");
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
        private static String MessageBody(String messageMode, String fromEmailId, String toUsers, String ccUsers, String subject, String content)
        {
            StringBuilder mail = new StringBuilder();

            if (messageMode.Equals(MessageMode.DRAFTMESSAGE))
            {
                mail.Append(content);
            }
            else
            {
                mail.Append("<b>To : </b>" + toUsers);
                mail.Append("<br /><b>CC : </b>" + ccUsers);
                mail.Append("<br /><b>From : </b>" + fromEmailId);
                mail.Append("<br /><b>Subject : </b>" + subject);
                mail.Append("<br /><b>Sent : </b>" + DateTime.Now.ToString("MM/dd/yyyy H:mm:ss"));
                mail.Append("<br /><div class='sxseparator'></div>");
                mail.Append("<br /> " + content);
                mail.Append("<br /><div class='sxseparator'></div>");
            }
            return mail.ToString();
        }

        private static Boolean ValidateMessagecontract(MessagingContract messagingContract, out String message)
        {
            if (messagingContract.ToList.IsNullOrEmpty())
            {
                message = "Invalid Tolist param";
                return false;
            }

            if (messagingContract.ToUserIds.IsNullOrEmpty())
            {
                message = "Invalid ToUserIds param";
                return false;
            }

            if (!(messagingContract.QueueType > 0))
            {
                message = "Invalid QueueType param";
                return false;
            }

            //subject and content

            if (!(messagingContract.CurrentUserId > 0))
            {
                message = "Invalid CurrentUserId param";
                return false;
            }

            if (messagingContract.MessageId.IsNullOrEmpty())
            {
                message = "Invalid MessageId param";
                return false;
            }

            if (messagingContract.MessageMode.IsNullOrEmpty())
            {
                message = "Invalid MessageMode param";
                return false;
            }

            if (!(messagingContract.FolderId > 0))
            {
                message = "Invalid FolderId param";
                return false;
            }

            if (!(messagingContract.TenantTypes.Count > 0))
            {
                message = "Invalid ReceiverTpye param";
                return false;
            }

            if (messagingContract.Action.IsNullOrEmpty())
            {
                message = "Invalid Action param";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private static String ValidateReceiverType(Int32 senderType, String receiverType, Dictionary<String, Int32?> emailTenantTypes)
        {
            switch ((TenantTypeEnum)senderType)
            {
                case TenantTypeEnum.Client:
                    if (emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Supplier)))
                        return "Emails cannot be sent to Supplier when receiver type is Client. Please remove supplier(s) emailid.";
                    else if (TenantTypeEnum.Company.Equals((TenantTypeEnum)Convert.ToInt32(receiverType)) && emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Client)))
                        return "Emails cannot be sent to Client when receiver Type is Company. Please remove Client(s) emailid.";
                    else if (TenantTypeEnum.Client.Equals((TenantTypeEnum)Convert.ToInt32(receiverType)) && emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Company)))
                        return "Emails cannot be sent to Company when receiver type is Client. Please remove Company(s) emailid.";
                    else
                        return String.Empty;
                case TenantTypeEnum.Company:
                    if (TenantTypeEnum.Client.Equals((TenantTypeEnum)Convert.ToInt32(receiverType)) && (emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Supplier)) || emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Company))))
                        return "Emails cannot be sent to Supplier or Company when receiver type is Client. Please remove supplier(s) or Company(s) emailid.";
                    else if (TenantTypeEnum.Company.Equals((TenantTypeEnum)Convert.ToInt32(receiverType)) && (emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Supplier)) || emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Client))))
                        return "Emails cannot be sent to Supplier or Client when receiver Type is Company. Please remove Client(s) or supplier(s) emailid.";
                    else if (TenantTypeEnum.Supplier.Equals((TenantTypeEnum)Convert.ToInt32(receiverType)) && (emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Client)) || emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Company))))
                        return "Emails cannot be sent to Client or Company when receiver type is Supplier. Please remove Client(s) or Company(s) emailid.";
                    else
                        return String.Empty;
                case TenantTypeEnum.Supplier:
                    if (emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Client)))
                        return "Emails cannot be sent to Client when receiver type is Supplier. Please remove Client(s) emailid.";
                    else if (TenantTypeEnum.Company.Equals((TenantTypeEnum)Convert.ToInt32(receiverType)) && emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Supplier)))
                        return "Emails cannot be sent to Supplier when receiver Type is Company. Please remove Supplier(s) emailid.";
                    else if (TenantTypeEnum.Supplier.Equals((TenantTypeEnum)Convert.ToInt32(receiverType)) && emailTenantTypes.ContainsValue(Convert.ToInt32(TenantTypeEnum.Company)))
                        return "Emails cannot be sent to Company when receiver type is Supplier. Please remove Company(s) emailid.";
                    else
                        return String.Empty;
                default:
                    return String.Empty;
            }
        }

        #region mapping with usergroup


        /// <summary>
        /// Get list of all suppliers.
        /// </summary>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public static List<Supplier> GetAllSupplier()
        {
            try
            {
                return null;
                //return BALUtils.GetMessageRepoInstance().GetAllSupplier();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get list of all suppliers.
        /// </summary>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public static List<UserGroup> GetAllUserGroup(Int32 reviewerId)
        {
            try
            {
                return null;
                // return BALUtils.GetMessageRepoInstance().GetUserGroup(reviewerId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static UserGroup GetUserGroupByCurrentUserIdAndMessageType(Int32 userId, Int32 messageTypeId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetUserGroupByCurrentUserIdAndMessageType(userId, messageTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<UserGroup> GetUsergroupByClientDepartmentId(Int32 userId)
        {
            try
            {
                return null;
                //return BALUtils.GetMessageRepoInstance().GetUsergroupByClientDepartmentId(userId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<UserGroup> GetUsergroupBySupplierDepartmentId(Int32 userId)
        {
            try
            {
                return null;
                // return BALUtils.GetMessageRepoInstance().GetUsergroupBySupplierDepartmentId(userId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Supplier GetSupplierByUserId(Int32 userId)
        {
            try
            {
                //return BALUtils.GetMessageRepoInstance().GetSupplierByUserID(userId);
                return null;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #endregion

        public static IQueryable<EntityObject> GetTemplates(Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetTemplates(currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }


        /// <summary>
        /// Get user 
        /// </summary>
        /// <param name="queueType"></param>
        /// <returns></returns>
        public static IQueryable<EntityObject> GetTemplates(Int32 queueType, Int32 currentUserId, String communicationTypeCode)
        {
            try
            {
                Int32 communicationTypeId = GetCommuncationTypeByCode(communicationTypeCode).CommunicationTypeID;
                return BALUtils.GetMessageRepoInstance().GetTemplates(queueType, currentUserId, communicationTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// Invoked to get the user specific folders
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<lkpMessageFolder> GetFolders(Int32 userID, Int32 userGroupID)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetFolders(userID, userGroupID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            return null;
        }

        /// <summary>
        /// Invoked to get communication types
        /// </summary>
        /// <returns>List of Communication Types</returns>
        public static List<lkpCommunicationType> GetCommuncationTypes(Guid userId, Permissions permission = Permissions.FullAccess)
        {
            try
            {
                List<lkpCommunicationType> lstCommunicationType = LookupManager.GetMessagingLookUpData<lkpCommunicationType>().ToList();
                return BALUtils.GetMessageRepoInstance().GetCommuncationTypes(userId, permission, lstCommunicationType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            return null;
        }

        public static lkpCommunicationType GetCommuncationTypeByCode(string code)
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpCommunicationType>().Where(condition => condition.Code.Equals(code)).FirstOrDefault();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            return null;
        }

        public static lkpCommunicationType GetCommuncationTypeById(Int32 communcationTypeId)
        {
            try
            {
                return LookupManager.GetMessagingLookUpData<lkpCommunicationType>().Where(condition => condition.CommunicationTypeID.Equals(communcationTypeId)).FirstOrDefault();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            return null;
        }

        public static Dictionary<string, string> SaveDocumentAndGetDocumentId(List<ADBMessageDocument> documents)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().SaveDocumentAndGetDocumentId(documents);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// Get colection of message type base on receiver type.
        /// </summary>
        /// <param name="receiverTpe"></param>
        /// <returns></returns>
        public static List<lkpMessageType> GetMessageTypesByReceiver(TenantTypeEnum receiverTpe)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetMessageTypesByReceiver(receiverTpe);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Invoked to get the latest updated messages from the queue.
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="folderId"></param>
        /// <param name="queueType"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="IsupdatedOnly"></param>
        /// <param name="lastFetchDateTime"></param>
        /// <returns></returns>
        public static List<MessageDetail> GetQueue(String queueName, Int32 folderId, String folderCode, Int32 queueType, Int32 queueOwnerId, Int32 userGroup, Boolean IsupdatedOnly, DateTime lastFetchDateTime, MessageSearchContract messageSearchContract)
        {
            try
            {
                List<MessageDetail> result;
                QueueManager queueManager = new QueueManager();
                IEnumerable<EntityObject> messageQueue = null;

                if (IsupdatedOnly == false)
                    messageQueue = queueManager.GetQueue(queueName, folderId, folderCode, queueType, queueOwnerId, userGroup);
                else
                    messageQueue = queueManager.GetUpdateQueue(queueName, folderId, queueType, queueOwnerId, lastFetchDateTime);


                if (folderCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()))
                    result = GetADBMessageDetailsForDraftItems(messageQueue);
                else
                    result = GetADBMessageDetails(messageQueue, folderCode);

                //for performing search
                if (messageSearchContract.MessageBody.Trim() == String.Empty && messageSearchContract.Subject.Trim() == String.Empty
                    && messageSearchContract.SenderId.Trim() == String.Empty && messageSearchContract.ToUserList.Trim() == String.Empty)
                {
                    return result;
                }
                else
                {
                    Dictionary<String, String> searchOptions = GetSearchOptionsForDefaultAndCustomFolder(messageSearchContract);
                    var finalResult = result.AsQueryable().AdvanceTextSearch(searchOptions, false).ToList();
                    return finalResult;
                }
                #region Commented fro queuetype issue by deepika
                //switch ((TenantTypeEnum)queueType)
                //{
                //    //case TenantTypeEnum.Client:
                //    //    if (folderCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()))
                //    //        return GetClientMessageDetailsForDraftItems(messageQueue);
                //    //    else
                //    //        return GetClientMessageDetails(messageQueue, folderCode);
                //    case TenantTypeEnum.Company:
                //        if (folderCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()))
                //            return GetADBMessageDetailsForDraftItems(messageQueue);
                //        else
                //            return GetADBMessageDetails(messageQueue, folderCode);
                //    //case TenantTypeEnum.Supplier:
                //    //    if (folderCode.Equals(lkpMessageFolderContext.DRAFTS.GetStringValue()))
                //    //        return GetSupplierMessageDetailsForDraftItems(messageQueue);
                //    //    else
                //    //        return GetSupplierMessageDetails(messageQueue, folderCode);
                //} 
                #endregion
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// Invoked to get the tenanttype.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static Int32 GetTenantType(Int32 userID)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetTenantType(userID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return AppConsts.NONE;
        }

        /// <summary>
        /// Invoked to send message.
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="initiator"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Boolean SendMessage(String queueName, Int32 initiator, String message, String applicationDatabaseName, Int32 senderId, String subject, out Guid adbMessageID)
        {
            adbMessageID = Guid.Empty;
            try
            {
                QueueManager obj = new QueueManager();
                return obj.SendMessage(queueName, initiator, message, applicationDatabaseName, senderId, subject, out adbMessageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static ADBMessage GetMesssage(Guid messageId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetMesssage(messageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetUserNamesIdsString(List<Int32> userIds)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetUserNamesIds(userIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean CheckSubscriptionStatus(Int32 queueOwnerId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().CheckSubscriptionStatus(queueOwnerId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;

        }

        public static Boolean UpdateSubscriptionStatus(Int32 queueOwnerId, Boolean status)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().UpdateSubscriptionStatus(queueOwnerId, status);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;

        }

        public static String GetAttachmentIcon(String fileExtension)
        {
            switch (fileExtension)
            {
                case ".pdf":
                    return "pdf.png";
                case ".doc":
                case ".docx":
                    return "doc.png";
                case ".xls":
                case ".csv":
                case ".xlsx":
                    return "excel.png";
                case ".bmp":
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return "image.png";
                case ".txt":
                    return "text.png";
                default:
                    return "unknown.png";
            }
        }

        public static List<OrganizationLocation> GetInstitutionLocations(Int32 institutionId, Int32 currentUserId, Int32 currentTenantId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetInstitutionLocations(institutionId, currentUserId, currentTenantId, SecurityManager.DefaultTenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<OrganizationLocation>();
        }

        public static Boolean SaveMessageRules(MessagingRulesContract messagingRulesContract, String databaseName)
        {
            try
            {

                BALUtils.GetMessageRulesRepoInstance().SaveMessageRules(messagingRulesContract, databaseName);
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static List<MessageRule> GetMessageRules(Int32 userId)
        {
            try
            {
                return BALUtils.GetMessageRulesRepoInstance().GetMessageRules(userId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<MessageRule>();
        }

        public static void DeleteMessageRule(Int32 ruleId)
        {
            BALUtils.GetMessageRulesRepoInstance().DeleteMessageRule(ruleId);
        }

        /// <summary>
        /// Retrieve list of tenants for message rules screation.
        /// </summary>
        /// <returns>list of active Users</returns>
        public static List<Tenant> GetTenantsForRules(Int32 currentTenantId)
        {
            try
            {
                Int32 defaultTenantId = SecurityManager.DefaultTenantID;
                return BALUtils.GetMessageRepoInstance().GetTenantsForRules(currentTenantId, defaultTenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Invoked to Get folder Id by Code.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userId"></param>
        /// <param name="folderID"></param>
        /// <returns></returns>
        public static Int32 GetFolderIdByCode(String folderCode)
        {
            return BALUtils.GetMessageRepoInstance().GetFolderIdByCode(folderCode);
        }

        public static List<ADBMessageDocument> GetMessageAttachment(Guid messageId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetMessageAttachment(messageId);
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

        #region Messaging Group

        /// <summary>
        /// Gets messaging groups
        /// </summary>
        /// <returns></returns>
        public static IQueryable<vw_ListOfUsers> GetMessagingGroups(Int32 organizationUserId)
        {
            return BALUtils.GetMessageRepoInstance().GetMessagingGroups(organizationUserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagingGroupIds"></param>
        /// <returns></returns>
        public static IEnumerable<MessagingGroup> GetMessagingGroups(List<Int32> messagingGroupIds)
        {
            return BALUtils.GetMessageRepoInstance().GetMessagingGroups(messagingGroupIds);
        }

        /// <summary>
        /// Gets email ids
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static IEnumerable<String> GetEmailIdsByGroupIds(List<Int32> groupIds)
        {
            return BALUtils.GetMessageRepoInstance().GetEmailIdsByGroupIds(groupIds);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static List<MessagingGroup> GetGroupFolderList(List<aspnet_Roles> roleID)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetGroupFolderList(roleID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return new List<MessagingGroup>();
        }

        /// <summary>
        /// Invoked to get the ADB message details.
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static List<MessageDetail> GetADBGroupMessageDetails(List<vw_groupMessages> messages)
        {
            List<MessageDetail> messageDetails = new List<MessageDetail>();
            String messageSizeNode = String.Empty;

            foreach (vw_groupMessages msg in messages)
            {
                //ADBMessageToUserGroup item = (ADBMessageToUserGroup)msg;
                if (!messageDetails.Exists(m => m.MessageDetailID.Equals(msg.ADBMessageID)))
                {
                    MessageDetail message = new MessageDetail();
                    message.MessageDetailID = msg.ADBMessageID;
                    message.Subject = msg.Subject;
                    message.ReceivedDate = Convert.ToDateTime(msg.ReceiveDate);
                    message.ReceivedDateString = msg.ReceiveDate.ToString();
                    message.ReceivedDateFormat = String.Format("{0:d/M/yyyy HH:mm tt}", msg.ReceiveDate);
                    message.IsHighImportant = msg.IsHighImportant;
                    message.CommunicationType = msg.Name == null ? string.Empty : msg.Name;

                    message.CommunicationTypeCode = msg.Code == null ? string.Empty : msg.Code;
                    messageSizeNode = msg.Message.GetNodeContent("MessageSize");

                    if (messageSizeNode.IsNullOrEmpty())
                    {
                        message.Size = 1024;
                        message.SizeIn = "1 KB";
                    }
                    else
                    {
                        message.Size = Convert.ToInt32(messageSizeNode);
                        message.SizeIn = SysXUtils.MessageSize(Convert.ToInt32(messageSizeNode));
                    }
                    message.IsFollowUp = msg.IsFollowUp;
                    message.IsUnread = msg.IsUnread;

                    String toEmail = msg.Message.GetNodeContent("ToUserList");
                    message.To = toEmail.IsNullOrEmpty() ? String.Empty : toEmail;

                    String ccEmail = msg.Message.GetNodeContent("CCUserList");
                    message.Cc = ccEmail.IsNullOrEmpty() ? String.Empty : ccEmail;

                    if (!msg.From.Equals(DBNull.Value))
                    {
                        String eMail = msg.Message.GetNodeContent("From");
                        message.From = eMail.IsNullOrEmpty() ? String.Empty : eMail;
                    }


                    message.HasAttachment = msg.DocumentName.IsNullOrEmpty() ? false : true;

                    messageDetails.Add(message);
                }
            }
            return messageDetails;
        }

        public static String GetDatabaseName(String connectionString)
        {
            System.Data.Entity.Core.EntityClient.EntityConnection builder = new System.Data.Entity.Core.EntityClient.EntityConnection();
            builder.ConnectionString = connectionString;
            return builder.StoreConnection.Database;
        }

        public static Int32 GetGroupFolderCount(Int32 groupID, String foldeCode)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetGroupFolderCount(groupID, foldeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return 0;
        }

        /// <summary>
        /// Invoked to get the list of organization users  to whom the internal message notifications is to be sent.
        /// </summary>
        /// <param name="userIdList">List of the users to fetch</param>
        /// <returns>Organization users list</returns>
        public static List<OrganizationUser> GetOrganizationUsers(List<Int32> userIdList)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetOrganizationUsers(userIdList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<OrganizationUser>();
        }


        public static List<MessageDetail> GetRecentMessages(String queueName, String folderCode, Int32 queueOwnerId)
        {
            QueueManager queueManager = new QueueManager();
            IEnumerable<EntityObject> recentMessages = queueManager.GetRecentMessages(queueName, folderCode, queueOwnerId);

            return GetADBMessageDetails(recentMessages, folderCode);
        }

        /// <summary>
        /// Search the data in vwComplianceItemDataQueue and returns the matched result.
        /// </summary>
        /// <param name="searchItemDataContract">List of fields and their data to be searched.</param>
        /// <returns>ApplicantComplianceItemData</returns>
        public static IQueryable<T> PerformSearchForGroupMessageQueues<T>(MessageSearchContract messageSearchContract)
        {
            try
            {
                Dictionary<String, String> searchOptions = GetSearchOptionsForGroupFolders(messageSearchContract);
                return BALUtils.GetMessageRepoInstance().PerformSearch<T>(searchOptions);
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

        private static Dictionary<String, String> GetSearchOptionsForGroupFolders(MessageSearchContract messageSearchContract)
        {
            Dictionary<String, String> searchOptions = new Dictionary<String, String>();
            searchOptions.Add("ToUserList", messageSearchContract.ToUserList.Trim() + SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR);
            searchOptions.Add("SenderUser", messageSearchContract.SenderId.Trim() + SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR);
            //searchOptions.Add("SenderUserEmailId", messageSearchContract.SenderUserEmailId.Trim() + SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR);
            searchOptions.Add("MessageBody", messageSearchContract.MessageBody.Trim() == String.Empty ? SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()
                 : messageSearchContract.MessageBody.Trim().Replace(" ", SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()));
            searchOptions.Add("Subject", messageSearchContract.Subject.Trim() == String.Empty ? SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()
                : messageSearchContract.Subject.Trim().Replace(" ", SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()));
            searchOptions.Add("UserGroupID", messageSearchContract.UserGroupID.ToString());
            return searchOptions;
        }

        private static Dictionary<String, String> GetSearchOptionsForDefaultAndCustomFolder(MessageSearchContract messageSearchContract)
        {
            Dictionary<String, String> searchOptions = new Dictionary<String, String>();
            searchOptions.Add("To", messageSearchContract.ToUserList.Trim() + SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR);
            searchOptions.Add("SenderUser", messageSearchContract.SenderId.Trim() + SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR);
            searchOptions.Add("MessageBody", messageSearchContract.MessageBody.Trim() == String.Empty ? SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()
                 : messageSearchContract.MessageBody.Trim().Replace(" ", SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()));
            searchOptions.Add("Subject", messageSearchContract.Subject.Trim() == String.Empty ? SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()
                : messageSearchContract.Subject.Trim().Replace(" ", SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()));

            return searchOptions;
        }

        /// <summary>
        /// Invoked to delete group messages
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public static Boolean DeleteADBgroupMesssage(Guid messageId, Int32 userGroupId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().DeleteADBgroupMesssage(messageId, userGroupId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        #region Order Notification History Details

        public static Dictionary<Int32, String> GetSubEventsNamesBySysCommId(List<Int32> lstSysCommunicationId)
        {
            try
            {
                Dictionary<Int32, String> dicSysCommIdSubEventsName = new Dictionary<Int32, String>();
                var lstSystemCommunication = BALUtils.GetMessageRepoInstance().GetSystemCommunicationByIds(lstSysCommunicationId);
                if (lstSystemCommunication.IsNotNull() && lstSystemCommunication.Count > 0)
                {
                    foreach (var temp in lstSystemCommunication)
                    {
                        if (!dicSysCommIdSubEventsName.ContainsKey(temp.SystemCommunicationID))
                            dicSysCommIdSubEventsName.Add(temp.SystemCommunicationID, temp.lkpCommunicationSubEvent.Name);
                    }
                }
                return dicSysCommIdSubEventsName;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static bool ResendOrderCompletedNotification(Int32 orderId, Int32 orderNotificationId, Entity.ClientEntity.OrganizationUserProfile orgUserProfile, Int32 tenantId, Int32 loggedInUserId, Int32 hierarchyNodeID, Int32 svcGroupID, String svcGrpName, String orderNumber, Boolean isOrderFlagged)
        {
            try
            {

                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS;
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

                //Create Dictionary for Mail And Message Data
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(orgUserProfile.FirstName, " ", orgUserProfile.LastName));
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, string.Concat(applicationUrl));
                dictMailData.Add(EmailFieldConstants.ORDER_NO, Convert.ToString(orderNumber));
                dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, Convert.ToString(svcGrpName));

                CommunicationMockUpData mockData = new CommunicationMockUpData();
                mockData.UserName = string.Concat(orgUserProfile.FirstName, " ", orgUserProfile.LastName);
                mockData.EmailID = orgUserProfile.PrimaryEmailAddress;
                mockData.ReceiverOrganizationUserID = orgUserProfile.OrganizationUserID;

                //Create Attachment
                SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();

                var BGPkgPDFAttachementStatus = BackgroundProcessOrderManager.GetBGPkgPDFAttachementStatus(tenantId, hierarchyNodeID);
                if (BGPkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                {
                    if (svcGroupID > 0)
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS_WITHOUT_PDF_ATTACHMENT;
                    else
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS_WITHOUT_PDF_ATTACHMENT;
                }
                else
                {
                    if (svcGroupID > 0)
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS;
                    else
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS;
                }

                //UAT-3453
                String entityTypeCode = CommunicationEntityType.SCREENING_RESULT.GetStringValue();
                Int32 entityID = AppConsts.NONE;
                List<Entity.lkpScreeningResultType> lstLkpScreeningResultType = CommunicationManager.GetScreeningResultType();
                String screeningResultTypeCode = String.Empty;

                if (isOrderFlagged)
                {
                    screeningResultTypeCode = ScreeningResultType.Flagged.GetStringValue();
                    entityID = lstLkpScreeningResultType.Where(con => con.SRT_Code == screeningResultTypeCode && !con.SRT_IsDeleted).Select(sel => sel.SRT_ID).FirstOrDefault();
                }
                else
                {
                    screeningResultTypeCode = ScreeningResultType.Clear.GetStringValue();
                    entityID = lstLkpScreeningResultType.Where(con => con.SRT_Code == screeningResultTypeCode && !con.SRT_IsDeleted).Select(sel => sel.SRT_ID).FirstOrDefault();
                }

                if (!BGPkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                {
                    ParameterValue[] parameters;
                    if (svcGroupID > 0)
                    {
                        parameters = new ParameterValue[3];
                        parameters[2] = new ParameterValue();
                        parameters[2].Name = "PackageGroupID";
                        parameters[2].Value = svcGroupID.ToString();
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS;
                    }
                    else
                    {
                        parameters = new ParameterValue[2];
                    }
                    parameters[0] = new ParameterValue();
                    parameters[0].Name = "OrderID";
                    parameters[0].Value = orderId.ToString();
                    parameters[1] = new ParameterValue();
                    parameters[1].Name = "TenantID";
                    parameters[1].Value = tenantId.ToString();

                    String reportName = "OrderCompletion";
                    String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                    String fileName = "OCR_" + tenantId.ToString() + "_" + date + ".pdf";

                    byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);

                    String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);


                    sysCommAttachment.SCA_OriginalDocumentID = -1;
                    sysCommAttachment.SCA_OriginalDocumentName = "OrderCompletedReport.pdf";
                    sysCommAttachment.SCA_DocumentPath = retFilepath;
                    sysCommAttachment.SCA_DocumentSize = reportContent.Length;

                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                    sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                    sysCommAttachment.SCA_IsDeleted = false;
                    sysCommAttachment.SCA_CreatedBy = loggedInUserId;
                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                    sysCommAttachment.SCA_ModifiedBy = null;
                    sysCommAttachment.SCA_ModifiedOn = null;
                }

                #region Code to generate report url place holder value

                Dictionary<String, String> reportUrlParameters = new Dictionary<String, String>
                                                         {
                                                            { "OrderID",  orderId.ToString()},
                                                             { "ReportType",  BackgroundReportType.ORDER_COMPLETION.GetStringValue()},
                                                             { "TenantId",  tenantId.ToString()},
                                                             {"HierarchyNodeID",Convert.ToString(hierarchyNodeID)},
                                                             {"IsReportSentToStudent",Convert.ToString(false)},
                                                             {"OrganizationUserID",Convert.ToString(orgUserProfile.OrganizationUserID)}
                                                         };

                StringBuilder reportUrl = new StringBuilder();
                reportUrl.Append(applicationUrl.Trim() + "?args=");
                reportUrl.Append(reportUrlParameters.ToEncryptedQueryString());

                #endregion

                dictMailData.Add(EmailFieldConstants.REPORT_URL, reportUrl.ToString());

                //Send mail
                Int32? newSystemCommunicationID;
                //UAT-3453
                if (commSubEvent == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS_WITHOUT_PDF_ATTACHMENT || commSubEvent == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS)
                    newSystemCommunicationID = CommunicationManager.SendPackageNotificationMail(commSubEvent, dictMailData, mockData, tenantId, hierarchyNodeID, entityID, entityTypeCode, true, true);
                else
                    newSystemCommunicationID = CommunicationManager.SendPackageNotificationMail(commSubEvent, dictMailData, mockData, tenantId, hierarchyNodeID, null, null, true);

                if (newSystemCommunicationID != null)
                {
                    if (!BGPkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                    {
                        //Save Mail Attachment
                        sysCommAttachment.SCA_SystemCommunicationID = newSystemCommunicationID.Value;
                        Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                    }

                    return BackgroundProcessOrderManager.UpdateOrderNotificationBkgOrderServiceForm(orderNotificationId, tenantId, loggedInUserId, 0, 0, newSystemCommunicationID.Value, null);
                }
                else
                {
                    return false;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static bool ResendOrderNotification(Int32 orderNotificationId, Int32 applicantId, Int32 tenantId, Int32 loggedInUserId, Int32 systemCommunicationId)
        {
            SystemCommunication masterSystemCommunication = BALUtils.GetMessageRepoInstance().GetSystemCommunicationById(systemCommunicationId);
            if (masterSystemCommunication.IsNotNull())
            {
                String documentName = String.Empty;
                List<ADBMessageDocument> messageDocument = new List<ADBMessageDocument>();

                SystemCommunication newSystemCommunication = new SystemCommunication();
                newSystemCommunication.CommunicationSubEventID = masterSystemCommunication.CommunicationSubEventID;
                newSystemCommunication.Subject = masterSystemCommunication.Subject;
                newSystemCommunication.Content = masterSystemCommunication.Content;
                newSystemCommunication.SenderEmailID = masterSystemCommunication.SenderEmailID;
                newSystemCommunication.SenderName = masterSystemCommunication.SenderName;
                newSystemCommunication.CreatedOn = DateTime.Now;
                newSystemCommunication.CreatedByID = loggedInUserId;

                if (masterSystemCommunication.SystemCommunicationDeliveries.IsNotNull() && masterSystemCommunication.SystemCommunicationDeliveries.Count > 0)
                {
                    List<SystemCommunicationDelivery> lstSystemCommunicationDelivery = masterSystemCommunication.SystemCommunicationDeliveries.ToList();
                    foreach (var masterSystemCommunicationDelivery in lstSystemCommunicationDelivery)
                    {
                        SystemCommunicationDelivery newSystemCommunicationDelivery = new SystemCommunicationDelivery();
                        newSystemCommunicationDelivery.ReceiverOrganizationUserID = masterSystemCommunicationDelivery.ReceiverOrganizationUserID;
                        newSystemCommunicationDelivery.RecieverEmailID = masterSystemCommunicationDelivery.RecieverEmailID;
                        newSystemCommunicationDelivery.RecieverName = masterSystemCommunicationDelivery.RecieverName;
                        //newSystemCommunicationDelivery.DispatchedDate = masterSystemCommunicationDelivery.DispatchedDate;
                        newSystemCommunicationDelivery.IsDispatched = false;
                        newSystemCommunicationDelivery.CreatedOn = DateTime.Now;
                        newSystemCommunicationDelivery.CreatedByID = loggedInUserId;
                        newSystemCommunicationDelivery.IsCC = masterSystemCommunicationDelivery.IsCC;
                        newSystemCommunicationDelivery.IsBCC = masterSystemCommunicationDelivery.IsBCC;
                        newSystemCommunicationDelivery.RetryCount = 0;
                        newSystemCommunication.SystemCommunicationDeliveries.Add(newSystemCommunicationDelivery);
                    }
                }

                if (masterSystemCommunication.SystemCommunicationAttachments.IsNotNull() && masterSystemCommunication.SystemCommunicationAttachments.Count > 0)
                {
                    List<SystemCommunicationAttachment> lstSystemCommunicationAttachment = masterSystemCommunication.SystemCommunicationAttachments
                                                                                                .Where(cond => cond.SCA_IsDeleted == false).ToList();
                    foreach (var masterSystemCommunicationAttachment in lstSystemCommunicationAttachment)
                    {
                        SystemCommunicationAttachment newSystemCommunicationAttachment = new SystemCommunicationAttachment();
                        newSystemCommunicationAttachment.SCA_OriginalDocumentID = masterSystemCommunicationAttachment.SCA_OriginalDocumentID;
                        newSystemCommunicationAttachment.SCA_OriginalDocumentName = masterSystemCommunicationAttachment.SCA_OriginalDocumentName;
                        newSystemCommunicationAttachment.SCA_DocumentPath = masterSystemCommunicationAttachment.SCA_DocumentPath;
                        newSystemCommunicationAttachment.SCA_DocumentSize = masterSystemCommunicationAttachment.SCA_DocumentSize;
                        newSystemCommunicationAttachment.SCA_DocAttachmentTypeID = masterSystemCommunicationAttachment.SCA_DocAttachmentTypeID;
                        newSystemCommunicationAttachment.SCA_TenantID = masterSystemCommunicationAttachment.SCA_TenantID;
                        newSystemCommunicationAttachment.SCA_IsDeleted = masterSystemCommunicationAttachment.SCA_IsDeleted;
                        newSystemCommunicationAttachment.SCA_CreatedBy = loggedInUserId;
                        newSystemCommunicationAttachment.SCA_CreatedOn = DateTime.Now;

                        ADBMessageDocument newADBMessageDocument = new ADBMessageDocument();
                        newADBMessageDocument.DocumentName = masterSystemCommunicationAttachment.SCA_DocumentPath;
                        newADBMessageDocument.OriginalDocumentName = masterSystemCommunicationAttachment.SCA_OriginalDocumentName;
                        newADBMessageDocument.DocumentSize = masterSystemCommunicationAttachment.SCA_DocumentSize;
                        newADBMessageDocument.IsActive = true;
                        newADBMessageDocument.IsDeleted = false;
                        newSystemCommunicationAttachment.ADBMessageDocuments.Add(newADBMessageDocument);

                        newSystemCommunication.SystemCommunicationAttachments.Add(newSystemCommunicationAttachment);
                        messageDocument.Add(newADBMessageDocument);
                    }
                }
                BALUtils.GetMessageRepoInstance().AddSystemCommunicationObject(newSystemCommunication);
                Int32 newSystemCommunicationID = newSystemCommunication.SystemCommunicationID;
                if (messageDocument.Count > 0)
                {
                    messageDocument.ForEach(a => documentName += a.DocumentId.ToString() + ";");
                }

                SystemCommunication systemCommunication = new SystemCommunication();
                systemCommunication.Content = masterSystemCommunication.Content;
                systemCommunication.Subject = masterSystemCommunication.Subject;
                systemCommunication.CommunicationSubEventID = masterSystemCommunication.CommunicationSubEventID;

                MessagingContract messagingContract = CommunicationManager.GetMessageContract(masterSystemCommunication.lkpCommunicationSubEvent.Code, systemCommunication, applicantId, masterSystemCommunication.CommunicationSubEventID.Value, tenantId);

                if (!String.IsNullOrEmpty(documentName))
                    messagingContract.DocumentName = documentName;

                CommunicationManager.SendMessageContentToQueue(messagingContract);

                Guid? messageID = messagingContract.MessageId;

                return BackgroundProcessOrderManager.UpdateOrderNotificationBkgOrderServiceForm(orderNotificationId, tenantId, loggedInUserId, 0, 0, newSystemCommunicationID, messageID);
            }
            return false;
        }

        #endregion

        public static SystemCommunication GetSystemCommunicationForMailData(Int32 systemCommunicationID)
        {
            return BALUtils.GetMessageRepoInstance().GetSystemCommunicationForMailData(systemCommunicationID);
        }
        #region UAT-3261: Badge Form Enhancements
        public static List<SystemCommunicationAttachment> GetSystemCommunicationAttachment(Int32 messageId)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().GetSystemCommunicationAttachment(messageId);
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
        #endregion


        #region UAT-3820
        public static String SaveDocumentReceivedfromStudentServiceFormStatus(ADBMessageDocument documents)
        {
            try
            {
                return BALUtils.GetMessageRepoInstance().SaveDocumentReceivedFromStudentServiceFormStatus(documents);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


    }
}
