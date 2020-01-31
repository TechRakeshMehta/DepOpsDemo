using Entity.ClientEntity;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Interfaces
{
    public interface IQueueManagementRepository
    {

        #region Queue Parameter Assignment

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueAssignmentConfiguration"></param>
        /// <param name="queueType"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        Boolean SaveUpdateQueueAssignmentConfiguration(QueueAssignmentConfiguration queueAssignmentConfiguration, QueueConfirgurationType queueType, Int32 currentLoggedInUserId);
        QueueAssignmentConfiguration getAssignmentConfigurationById(Int32 assignmentConfigurationId);
        List<GetQueueSpecilizationCriterion> getQueueSpecializationCriterionFieldsList(String query, Int32 queueFieldID, String currentQueueFieldValue);
        List<Int32> getAlreadyMappedQueueIds(Int32 basicQueueConfigurationTypeId);
        List<GetQueueAssigneeList> getQueueAssigneeListByConfiguration(Int32 assignmentConfigurationId);
        List<GetUserListApplicableForReview> getUserListApplicableForReview(Int32 queueID, Int32 assignmentConfigurationId, Int32 tenantId, Int32 currentReviewerId);
        Boolean SaveUpdateQueueAssigneeAndReviewerLevel(QueueAssigneesList assigneeData, Int32 currentLoggedInUserId);
        Boolean DeleteQueueAssignee(Int32 assigneeId, Int32 currentLoggedInUserId);
        #endregion

        #region QUEUE ENGINE
        
        /// <summary>
        /// Gets the Next possible action on Verification details Save functionality
        /// </summary> 
        /// <returns></returns>
        DataTable GetNextQueueAction(String inputXML, Int32 tenantId, Int32 currentLoggedInUserId);


        /// <summary>
        /// Get the specialization of the Items for a particular user.
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        DataTable GetUserSpecializationDetails(Int32 currentLoggedInUserId, Int32 tenantId, String inputXML);

        /// <summary>
        /// Set Assigned to User Null for the items
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        void ClearQueueRecords(Int32 currentLoggedInUserId, String inputXML);

        #endregion

        #region AssignmentConfigurationQueue
        /// <summary>
        /// Fetch the records for Configuration Queue
        /// </summary>
        /// <param name="gridCustomPaging">gridCustomPaging</param>
        /// <param name="searchParameter">searchParameter</param>
        /// <param name="queueTypeCode">queueTypeCode</param>
        /// <returns></returns>
        List<QueueAssignmentConfRecord> GetQueueAssignmentConfRecord(CustomPagingArgsContract gridCustomPaging, String searchParameter, String queueTypeCode);
        /// <summary>
        /// To get records for Specialized FieldValue
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        List<GetQueueSpecilizationCriterion> GetQueueSpecializedFieldsList(String query);
        /// <summary>
        /// to delete the record from Configuration Queue 
        /// </summary>
        /// <param name="queueAssignmentConfID"></param>
        /// <param name="currentLoggedInID"></param>
        /// <param name="queueType"></param>
        /// <returns></returns>
        Boolean DeleteQueueAssignmentConfigurationRecord(Int32 queueAssignmentConfID, Int32 currentLoggedInID, QueueConfirgurationType queueType);
        #endregion

        /// <summary>
        /// Reset all business process for a record.
        /// </summary>
        /// <param name="businessProcessID">BusinessProcessID</param>
        /// <param name="recordID">RecordID</param>
        /// <returns>Int</returns>
        Int32 ResetBusinessProcess(Int32 businessProcessID, Int32 recordID);

        /// <summary>
        /// Get User Current Assignment
        /// </summary>
        /// <param name="organizationUserID">OrganizationUserID</param>
        /// <param name="businessProcessID">BusinessProcessID</param>
        /// <param name="tenantID">TenantID</param>
        /// <returns>List of UserCurrentAssignments</returns>
        List<UserCurrentAssignments> GetUserCurrentAssignments(Int32 organizationUserID, Int32 businessProcessID, Int32 tenantID);

        /// <summary>
        /// Escalate the Items
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="inputXML"></param>
        void EscalateItems(Int32 currentLoggedInUserId, String inputXML);

        #region QUEUE AUDIT
        /// <summary>
        /// Get the Queue audit data 
        /// </summary>
        /// <param name="queueSearchDataContract">queueSearchDataContract</param>
        /// <param name="queueAuditArgsContract">queueAuditArgsContract</param>
        /// <returns>DataTable</returns>
        DataTable GetQueueRecordAuditData(QueueFrameworkSearchDataContract queueSearchDataContract, CustomPagingArgsContract queueAuditArgsContract);

        List<Int32> GetOrganizationUserIdList(Int32 tenantID, Int32 queueId);
        List<OrganizationUser> GetOrganizationUsersByIds(List<Int32> lstUserIds);


        #endregion

        //UAT-815
        /// <summary>
        /// Handle assignment method that call the sp_HandleAssignment to handle the automatic assignment.
        /// </summary>
        /// <param name="dicHandleAssignmentData"></param>
        void HandleAssignment(Dictionary<String, Object> dicHandleAssignmentData);

        void HandleReconciliationAssignment(Dictionary<String, Object> dicHandleAssignmentData);

        void ResetReconciliationProcess(Int32 BusinessProcessID, Int32 RecordID, Boolean IsProcessCompeted, Int32 tenantId);

        /// <summary>
        /// Set Assigned to User Null for the items
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        void ClearReconciliationQueueRecords(Int32 currentLoggedInUserId, String inputXML, Int32 tenantId);

        void ReconcillationOverRideByClntAdmin(Int32 currentLoggedInUserId, String inputXML, Int32 tenantId);
    }
}
