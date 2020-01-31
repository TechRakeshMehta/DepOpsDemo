
#region Header Comment Block

// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IMessageRepository.cs
// Purpose:   This file is for CRUD operations from Database
//
// Revisions:
// Comment
// -----------------------------------------
// Added enhancement changes

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
#endregion

#region Application Specific
using Entity;
using INTSOF.Utils;


#endregion

#endregion


namespace DAL.Interfaces
{
    /// <summary>
    /// Interface for exposing methods to be used by MessageRepository.
    /// </summary>
    public interface IMessageRepository
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
        List<String> GetEmailId(List<Int32> userIdList);
        String GetEmailId(Int32 userId);


        EntityObject GetMessageContents(Int32 queueType, Guid messageId, Int32 currentUserId = 0, Boolean IsDashboardMessage = false);

        EntityObject GetMessageContentsForApplicant(Int32 queueType, Guid messageId);
        ComplexObject GetCommonMessageContents(Int32 queueType, Guid messageId);

        Int32 GetMessageCount(Int32 folderId, String folderCode, Int32 queueOwnerId, Int32 communicationTypeId);
        String GetFolderName(Int32 folderId, String folderCode);



        String GetUserIDList(String[] emailList);

        String GetUserIDListByEmployeeIds(String[] employeeIds);

        String GetUserGroupIdList(String[] organizationusrId, Boolean isClient, Int32 msgType);

        Boolean SaveDraftedMesssage(Guid messageId, String message, String messageMode, String subject, Int32 organizationUserId, String toUsers, String ccUsers, String toGroupIds, String ccGroupIds, Boolean isHighImportance, String documentName, String originalDocumentName, String bccUsers, String from, Int32 externaMessagingGroupTypeId, String messageContent);

        Boolean DeleteMesssageFromDashboard(Guid messageId, Int32 userId);
        Boolean DeleteMesssage(Guid messageId, Int32 userId, String folderCode, Int32 queueTypeID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Boolean DeleteMesssage(Guid messageId);

        Boolean RestoreMessage(Guid messageId, Int32 userId, Int32 queueTypeID);
        String FoldersToBeRestored(Guid messageId, Int32 userId);
        Boolean SetMoveToFolder(Guid messageId, Int32 userId, String folderCode, Int32 moveToFolderId, String moveToFolderCode, Int32 queueTypeID);

        String AddNewFolder(String nodeText, Int32 currentUserID, Int32 userGroup, Int32 parentFolderID);

        Boolean UpdateFollowUpStatus(Guid messageId, Int32 queueOwnerId, Int32 queueTypeID, Boolean isFollowUp, String folderCode);

        Boolean UpdateReadStatus(Guid messageId, Int32 queueOwnerId, Int32 queueTypeID, Boolean isFollowUp);

        String GetUniqueTemplateName(Guid adbMessageId, Int32 from, String templateName);

        Boolean UpdateMessage(ADBMessage adbMessage);


        List<Int32> GetAllFoldersToBedeleted(Int32 folderId, Int32 userId);

        void DeleteMessageRulesintransaction(List<Int32> folderToBeDeleted, Int32 userId);

        void DeleteAllMessagesintransaction(List<Int32> folderToBeDeleted, Int32 userId);

        void DeleteAllFolders(List<Int32> folderToBeDeleted, Int32 userId);

        List<Int32> GetAllFoldersToBeRestored(Guid messageId, Int32 userId);

        void RestoreAllFolders(List<Int32> foldersToBeRestored, Int32 userId);
        #region Mapping with usergroup

        IQueryable<OrganizationUser> RetrieveListOfUsers(List<Int32> lstSelectedOrganizationUserIds, Int32 organizationUserId, lkpCommunicationTypeContext communicationTypeContext, Int32 defaultTenantId, Int32 selectedTenantId = 0, Int32 selectedProgramId = 0);

        #endregion
        #endregion
        #region Protected Methods

        #endregion
        #region Private Methods

        #endregion

        #endregion

        String GetUserGroupIdBySupplierId(string[] supplierId, Int32 messageType);

        UserGroup GetUserGroupByCurrentUserIdAndMessageType(Int32 userId, Int32 messageTypeId);

        #region Bug Work

        /// <summary>
        /// Get colection of message type base on receiver type
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>


        #endregion

        ADBMessage GetMesssage(Guid messageId);
        Int32 GetTenantType(Int32 userID);

        List<lkpMessageFolder> GetFolders(Int32 userID, Int32 userGroupID);
        IQueryable<EntityObject> GetTemplates(Int32 currentUserId);
        IQueryable<EntityObject> GetTemplates(Int32 queueType, Int32 currentUserId, Int32 communicationTypeId);
        List<lkpMessageType> GetMessageTypesByReceiver(TenantTypeEnum receiver);
        Dictionary<string, string> SaveDocumentAndGetDocumentId(List<ADBMessageDocument> documents);

        String GetUserNamesIds(List<Int32> userIdList);
        List<lkpCommunicationType> GetCommuncationTypes(Guid userId, Permissions permission, List<lkpCommunicationType> lstCommunicationType);
        Boolean CheckSubscriptionStatus(Int32 queueOwnerId);

        Boolean UpdateSubscriptionStatus(Int32 queueOwnerId, Boolean status);

        List<OrganizationLocation> GetInstitutionLocations(Int32 institutionId, Int32 currentUserId, Int32 currentTenantId, Int32 defaultTenantId);
        Int32 GetFolderIdByCode(String folderCode);

        List<ADBMessageDocument> GetMessageAttachment(Guid messageId);


        List<MessagingGroup> GetGroupFolderList(List<aspnet_Roles> roleID);

        /// <summary>
        /// Gets messaging groups
        /// </summary>
        /// <returns></returns>
        IQueryable<vw_ListOfUsers> GetMessagingGroups(Int32 organizationUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagingGroupIds"></param>
        /// <returns></returns>
        IEnumerable<MessagingGroup> GetMessagingGroups(List<Int32> messagingGroupIds);

        /// <summary>
        /// Gets email ids
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        IEnumerable<String> GetEmailIdsByGroupIds(List<Int32> groupIds);
        Int32 GetGroupFolderCount(Int32 groupID, String foldeCode);

        /// <summary>
        /// Gets the list of the organization Users
        /// </summary>
        /// <param name="userIdList">List of users to fetch</param>
        /// <returns>List of the Organization Users.</returns>
        List<OrganizationUser> GetOrganizationUsers(List<Int32> userIdList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTenantId"></param>
        /// <param name="defaultTenantId"></param>
        /// <returns></returns>
        List<Tenant> GetTenantsForRules(Int32 currentTenantId, Int32 defaultTenantId);

        IQueryable<T> PerformSearch<T>(Dictionary<String, String> searchOptions);

        /// <summary>
        /// Invoked to delete group messages
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        Boolean DeleteADBgroupMesssage(Guid messageId, Int32 userGroupId);
        List<SystemCommunication> GetSystemCommunicationByIds(List<Int32> lstSysCommunicationId);
        SystemCommunication GetSystemCommunicationById(Int32 SysCommunicationId);
        Boolean AddSystemCommunicationObject(SystemCommunication newSystemCommunication);
        void SendMailToExternalGroup(String toGroupIds, String ccGroupIds, Boolean isHighInportance, String subject, String messageBody, String from, Int32 externaMessagingGroupTypeId, Int32 senderUserId);
        SystemCommunication GetSystemCommunicationForMailData(Int32 systemCommunicationID);

        List<SystemCommunicationAttachment> GetSystemCommunicationAttachment(Int32 messageId);  //UAT-3261: Badge Form Enhancements

        String SaveDocumentReceivedFromStudentServiceFormStatus(ADBMessageDocument documents);
    }
}
