using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Repository
{
    public class QueueManagementRepository : ClientBaseRepository, IQueueManagementRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public QueueManagementRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #region Queue Parameter Assignment
        //This method not in use post implementation of UAT-1886.
        Boolean IQueueManagementRepository.SaveUpdateQueueAssignmentConfiguration(QueueAssignmentConfiguration queueAssignmentConfiguration, QueueConfirgurationType queueType, Int32 currentLoggedInUserId)
        {
            if (queueAssignmentConfiguration.QAC_ID > AppConsts.NONE)
            {
                QueueAssignmentConfiguration existingQueueAssignmentConfiguration = getAssignmentConfigurationById(queueAssignmentConfiguration.QAC_ID);
                existingQueueAssignmentConfiguration.QAC_QueueID = queueAssignmentConfiguration.QAC_QueueID;
                existingQueueAssignmentConfiguration.QAC_TenantID = queueAssignmentConfiguration.QAC_TenantID;
                existingQueueAssignmentConfiguration.QAC_Description = queueAssignmentConfiguration.QAC_Description;
                existingQueueAssignmentConfiguration.QAC_AutomationLevelID = queueAssignmentConfiguration.QAC_AutomationLevelID;
                existingQueueAssignmentConfiguration.QAC_RecordDistributionStrategyID = queueAssignmentConfiguration.QAC_RecordDistributionStrategyID;
                existingQueueAssignmentConfiguration.QAC_ReviewerCount = queueAssignmentConfiguration.QAC_ReviewerCount;
                existingQueueAssignmentConfiguration.QAC_IsDeleted = queueAssignmentConfiguration.QAC_IsDeleted;
                existingQueueAssignmentConfiguration.QAC_ModifiedBy = currentLoggedInUserId;
                existingQueueAssignmentConfiguration.QAC_ModifiedOn = DateTime.Now;
                if (queueType == QueueConfirgurationType.SpecializedUserAssignment)
                {
                    QueueConfigurationMetaDataField existingQueueConfigurationMetaDataField = queueAssignmentConfiguration.QueueConfigurationMetaDataFields.FirstOrDefault();
                    existingQueueConfigurationMetaDataField.QCMF_QueueFieldID = queueAssignmentConfiguration.QueueConfigurationMetaDataFields.FirstOrDefault().QCMF_QueueFieldID;
                    existingQueueConfigurationMetaDataField.QCMF_QueueFieldValue = queueAssignmentConfiguration.QueueConfigurationMetaDataFields.FirstOrDefault().QCMF_QueueFieldValue;
                    existingQueueConfigurationMetaDataField.QCMF_IsDeleted = queueAssignmentConfiguration.QueueConfigurationMetaDataFields.FirstOrDefault().QCMF_IsDeleted;
                    existingQueueConfigurationMetaDataField.QCMF_ModifiedBy = currentLoggedInUserId;
                    existingQueueConfigurationMetaDataField.QCMF_ModifiedOn = DateTime.Now;
                }
                ClientDBContext.SaveChanges();
                return true;
            }
            else
            {
                ClientDBContext.QueueAssignmentConfigurations.AddObject(queueAssignmentConfiguration);
                ClientDBContext.SaveChanges();
                return true;
            }
        }

        //This method not in use post implementation of UAT-1886.
        public QueueAssignmentConfiguration getAssignmentConfigurationById(Int32 assignmentConfigurationId)
        {
            return ClientDBContext.QueueAssignmentConfigurations.FirstOrDefault(x => x.QAC_ID == assignmentConfigurationId && !x.QAC_IsDeleted);
        }

        //This method not in use post implementation of UAT-1886.
        List<GetQueueSpecilizationCriterion> IQueueManagementRepository.getQueueSpecializationCriterionFieldsList(String query, Int32 queueFieldID, String currentQueueFieldValue)
        {
            List<GetQueueSpecilizationCriterion> specializationFieldList = null;
            specializationFieldList = ClientDBContext.usp_GetQueueSpecilizationCriterion(query).ToList();
            List<QueueConfigurationMetaDataField> alreadyMappedFields = ClientDBContext.QueueConfigurationMetaDataFields.Where(cond => cond.QCMF_QueueFieldID == queueFieldID
                                                                        && !cond.QCMF_IsDeleted).ToList();
            if (alreadyMappedFields.Count > AppConsts.NONE)
            {
                foreach (var queueFieldValue in alreadyMappedFields)
                {
                    specializationFieldList.Remove(specializationFieldList.Where(x => x.ID == queueFieldValue.QCMF_QueueFieldValue && x.ID != currentQueueFieldValue).FirstOrDefault());
                }
            }
            return specializationFieldList;
        }

        //This method not in use post implementation of UAT-1886.
        List<Int32> IQueueManagementRepository.getAlreadyMappedQueueIds(Int32 basicQueueConfigurationTypeId)
        {
            List<Int32> alreadyMappedQueueIds = null;
            List<QueueAssignmentConfiguration> alreadyMappedQueues = ClientDBContext.QueueAssignmentConfigurations.Where(x => x.QAC_ConfigurationTypeId == basicQueueConfigurationTypeId
                                                                                                                 && !x.QAC_IsDeleted).ToList();
            if (alreadyMappedQueues.Count > AppConsts.NONE)
            {
                alreadyMappedQueueIds = alreadyMappedQueues.Select(x => x.QAC_QueueID).Distinct().ToList();
            }
            return alreadyMappedQueueIds;
        }

        //This method not in use post implementation of UAT-1886.
        List<GetQueueAssigneeList> IQueueManagementRepository.getQueueAssigneeListByConfiguration(Int32 assignmentConfigurationId)
        {
            return ClientDBContext.usp_GetQueueAssigneeList(assignmentConfigurationId).ToList();
        }

        //This method not in use post implementation of UAT-1886.
        List<GetUserListApplicableForReview> IQueueManagementRepository.getUserListApplicableForReview(Int32 queueID, Int32 assignmentConfigurationId, Int32 tenantId, Int32 currentReviewerId)
        {
            return ClientDBContext.usp_GetUserListApplicableForReview(queueID, assignmentConfigurationId, tenantId, currentReviewerId).ToList();
        }

        //This method not in use post implementation of UAT-1886.
        Boolean IQueueManagementRepository.SaveUpdateQueueAssigneeAndReviewerLevel(QueueAssigneesList assigneeData, Int32 currentLoggedInUserId)
        {
            if (assigneeData.QAL_AssigneeID == AppConsts.NONE)
            {
                assigneeData.QAL_CreatedBy = currentLoggedInUserId;
                assigneeData.QAL_CreatedOn = DateTime.Now;
                foreach (QueueAssigneeReviewLevel assigneeReviewLevel in assigneeData.QueueAssigneeReviewLevels)
                {
                    assigneeReviewLevel.QARL_CreatedBy = currentLoggedInUserId;
                    assigneeReviewLevel.ARL_CreatedOn = DateTime.Now;
                }
                ClientDBContext.QueueAssigneesLists.AddObject(assigneeData);
                ClientDBContext.SaveChanges();
                return true;
            }
            else
            {
                QueueAssigneesList existingAssigneeData = ClientDBContext.QueueAssigneesLists.Include("QueueAssigneeReviewLevels").
                                                                                              FirstOrDefault(x => x.QAL_AssigneeID == assigneeData.QAL_AssigneeID && x.QAL_IsDeleted == false);
                //if (existingAssigneeData != null)
                //{
                //    existingAssigneeData.QAL_ModifiedBy = currentLoggedInUserId;
                //    existingAssigneeData.QAL_ModifiedOn = DateTime.Now;
                //}
                List<QueueAssigneeReviewLevel> existingAssignedReviwerLevels = existingAssigneeData.QueueAssigneeReviewLevels.Where(x => x.QARL_IsDeleted == false).ToList();
                List<QueueAssigneeReviewLevel> currentAssignedReviwerLevels = assigneeData.QueueAssigneeReviewLevels.ToList();
                List<QueueAssigneeReviewLevel> assignedReviwerLevelsToDelete = existingAssignedReviwerLevels.Where(x => !currentAssignedReviwerLevels.Any(cnd => cnd.QARL_ReviewLevel == x.QARL_ReviewLevel)).ToList();
                List<QueueAssigneeReviewLevel> assignedReviwerLevelsToInsert = currentAssignedReviwerLevels.Where(y => !existingAssignedReviwerLevels.Any(cnd => cnd.QARL_ReviewLevel == y.QARL_ReviewLevel)).ToList();
                foreach (QueueAssigneeReviewLevel assigneeReviewLevel in assignedReviwerLevelsToDelete)
                {
                    assigneeReviewLevel.QARL_IsDeleted = true;
                    assigneeReviewLevel.ARL_ModifiedBy = currentLoggedInUserId;
                    assigneeReviewLevel.ARL_ModifiedOn = DateTime.Now;
                }
                foreach (QueueAssigneeReviewLevel assigneeReviewLevel in assignedReviwerLevelsToInsert)
                {
                    assigneeReviewLevel.QARL_CreatedBy = currentLoggedInUserId;
                    assigneeReviewLevel.ARL_CreatedOn = DateTime.Now;
                    existingAssigneeData.QueueAssigneeReviewLevels.Add(assigneeReviewLevel);
                }
                ClientDBContext.SaveChanges();
                return true;
            }
        }

        //This method not in use post implementation of UAT-1886.
        Boolean IQueueManagementRepository.DeleteQueueAssignee(Int32 assigneeId, Int32 currentLoggedInUserId)
        {
            QueueAssigneesList assigneeRecord = ClientDBContext.QueueAssigneesLists.Include("QueueAssigneeReviewLevels").
                                                Where(x => x.QAL_AssigneeID == assigneeId && !x.QAL_IsDeleted).FirstOrDefault();
            if (assigneeRecord != null)
            {
                assigneeRecord.QAL_IsDeleted = true;
                assigneeRecord.QAL_ModifiedBy = currentLoggedInUserId;
                assigneeRecord.QAL_ModifiedOn = DateTime.Now;
                foreach (QueueAssigneeReviewLevel queueAssigneeReviewLevel in assigneeRecord.QueueAssigneeReviewLevels.Where(x => !x.QARL_IsDeleted))
                {
                    queueAssigneeReviewLevel.QARL_IsDeleted = true;
                    queueAssigneeReviewLevel.ARL_ModifiedBy = currentLoggedInUserId;
                    queueAssigneeReviewLevel.ARL_ModifiedOn = DateTime.Now;
                }
                ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion


        #region QUEUE ENGINE



        /// <summary>
        /// Handle assignment method that call the sp_HandleAssignment to handle the automatic assignment.
        /// </summary>
        /// <param name="dicHandleAssignmentData">dicHandleAssignmentData</param>
        public void HandleAssignment(Dictionary<String, Object> dicHandleAssignmentData)
        {
            if (dicHandleAssignmentData.IsNotNull())
            {
                Int32 currentLoggedInUserId;
                Int32 tenantId;
                String queueRecordXML = dicHandleAssignmentData.GetValue("QueueRecordXML") as String;
                dicHandleAssignmentData.TryGetValue("CurrentLoggedInUserId", out currentLoggedInUserId);
                dicHandleAssignmentData.TryGetValue("TenantId", out tenantId);
                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {

                    SqlCommand command = new SqlCommand("usp_HandleAssignment", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenantID", tenantId);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                    command.Parameters.AddWithValue("@QueueRecordXML", queueRecordXML);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                }
            }
        }

        /// <summary>
        /// Gets the Next possible action on Verification details Save functionality
        /// </summary>
        /// <returns></returns>
        public DataTable GetNextQueueAction(String inputXML, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetNextQueueAction", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", tenantId);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@QueueRecordXML", inputXML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Get the specialization of the Items for a particular user.
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        public DataTable GetUserSpecializationDetails(Int32 currentLoggedInUserId, Int32 tenantId, String inputXML)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetUserSpecializationDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", tenantId);
                command.Parameters.AddWithValue("@LoggedInUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@QueueRecordXML", inputXML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Set Assigned to User Null for the items
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        public void ClearQueueRecords(Int32 currentLoggedInUserId, String inputXML)
        {
            _dbContext.ClearQueueRecords(currentLoggedInUserId, inputXML);
        }
        #endregion

        #region AssignmentConfigurationQueue
        /// <summary>
        /// Fetch the records for Assignment Configuration Queue
        /// </summary>
        /// <param name="gridCustomPaging">gridCustomPaging</param>
        /// <param name="searchParameter">searchParameter</param>
        /// <param name="queueTypeCode">queueTypeCode</param>
        /// <returns></returns>
        /// //This method not in use post implementation of UAT-1886.
        List<QueueAssignmentConfRecord> IQueueManagementRepository.GetQueueAssignmentConfRecord(CustomPagingArgsContract gridCustomPaging, String searchParameter, String queueTypeCode)
        {
            string orderBy = null;
            string ordDirection = null;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";
            List<QueueAssignmentConfRecord> lstApplicantTransitionMappingList = ClientDBContext.GetQueueAssignmentConfRecord(orderBy, ordDirection, gridCustomPaging.CurrentPageIndex, gridCustomPaging.PageSize, queueTypeCode, searchParameter).ToList();
            return lstApplicantTransitionMappingList;
        }
        /// <summary>
        /// To get records for Specialized FieldValue
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        List<GetQueueSpecilizationCriterion> IQueueManagementRepository.GetQueueSpecializedFieldsList(String query)
        {
            List<GetQueueSpecilizationCriterion> specializationFieldList = null;
            specializationFieldList = ClientDBContext.usp_GetQueueSpecilizationCriterion(query).ToList();

            return specializationFieldList;
        }
        /// <summary>
        /// to delete the record from Assignment Configuration Queue
        /// </summary>
        /// <param name="queueAssignmentConfID">queueAssignmentConfID</param>
        /// <param name="currentLoggedInID">currentLoggedInID</param>
        /// <param name="queueType">queueType</param>
        /// <returns></returns>
        /// //This method not in use post implementation of UAT-1886.
        Boolean IQueueManagementRepository.DeleteQueueAssignmentConfigurationRecord(Int32 queueAssignmentConfID, Int32 currentLoggedInID, QueueConfirgurationType queueType)
        {
            if (queueAssignmentConfID > AppConsts.NONE)
            {
                if (queueType == QueueConfirgurationType.BasicAssignment)
                {
                    QueueAssignmentConfiguration queueAssigneeConfig = ClientDBContext.QueueAssignmentConfigurations.Include("QueueAssigneesLists").Include("QueueAssigneesLists.QueueAssigneeReviewLevels")
                                                                            .FirstOrDefault(x => x.QAC_ID == queueAssignmentConfID && !x.QAC_IsDeleted);
                    DeleteconfigurationAssigneeListAndLevel(queueAssigneeConfig, currentLoggedInID);

                }
                else
                {

                    QueueAssignmentConfiguration queueAssigneeConfig = ClientDBContext.QueueAssignmentConfigurations.Include("QueueConfigurationMetaDataFields").Include("QueueAssigneesLists").Include("QueueAssigneesLists.QueueAssigneeReviewLevels")
                                                                              .FirstOrDefault(x => x.QAC_ID == queueAssignmentConfID && !x.QAC_IsDeleted);
                    QueueConfigurationMetaDataField queueAssigneeConfigMetadDta = queueAssigneeConfig.QueueConfigurationMetaDataFields.FirstOrDefault(x => !x.QCMF_IsDeleted);

                    queueAssigneeConfigMetadDta.QCMF_IsDeleted = true;
                    queueAssigneeConfigMetadDta.QCMF_ModifiedBy = currentLoggedInID;
                    queueAssigneeConfigMetadDta.QCMF_ModifiedOn = DateTime.Now;

                    DeleteconfigurationAssigneeListAndLevel(queueAssigneeConfig, currentLoggedInID);

                }
                ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        //This method not in use post implementation of UAT-1886.
        private void DeleteconfigurationAssigneeListAndLevel(QueueAssignmentConfiguration queueAssigneeConfig, Int32 currentLoggedInID)
        {
            if (queueAssigneeConfig.IsNotNull())
            {
                queueAssigneeConfig.QAC_IsDeleted = true;
                queueAssigneeConfig.QAC_ModifiedBy = currentLoggedInID;
                queueAssigneeConfig.QAC_ModifiedOn = DateTime.Now;

                foreach (QueueAssigneesList assigneConfigList in queueAssigneeConfig.QueueAssigneesLists.Where(x => !x.QAL_IsDeleted))
                {
                    assigneConfigList.QAL_IsDeleted = true;
                    assigneConfigList.QAL_ModifiedBy = currentLoggedInID;
                    assigneConfigList.QAL_ModifiedOn = DateTime.Now;
                    foreach (var assignConfigReviewlevel in assigneConfigList.QueueAssigneeReviewLevels.Where(x => !x.QARL_IsDeleted))
                    {
                        assignConfigReviewlevel.QARL_IsDeleted = true;
                        assignConfigReviewlevel.ARL_ModifiedBy = currentLoggedInID;
                        assignConfigReviewlevel.ARL_ModifiedOn = DateTime.Now;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Reset all business process for a record.
        /// </summary>
        /// <param name="businessProcessID">BusinessProcessID</param>
        /// <param name="recordID">RecordID</param>
        /// <returns>Int</returns>
        public Int32 ResetBusinessProcess(Int32 businessProcessID, Int32 recordID)
        {
            return _dbContext.ResetBusinessProcess(businessProcessID, recordID);
        }

        /// <summary>
        /// Get User Current Assignment
        /// </summary>
        /// <param name="organizationUserID">OrganizationUserID</param>
        /// <param name="businessProcessID">BusinessProcessID</param>
        /// <param name="tenantID">TenantID</param>
        /// <returns>List of UserCurrentAssignments</returns>
        public List<UserCurrentAssignments> GetUserCurrentAssignments(Int32 organizationUserID, Int32 businessProcessID, Int32 tenantID)
        {
            return _dbContext.GetUserCurrentAssignments(organizationUserID, businessProcessID, tenantID).ToList();
        }

        /// <summary>
        /// Escalate the Items
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="inputXML"></param>
        public void EscalateItems(Int32 currentLoggedInUserId, String inputXML)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ApplyEscalationChanges", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QueueRecordXML", inputXML);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }

        }

        #region QUEUE AUDIT
        /// <summary>
        /// Get the Queue audit data 
        /// </summary>
        /// <param name="queueSearchDataContract">queueSearchDataContract</param>
        /// <param name="queueAuditArgsContract">queueAuditArgsContract</param>
        /// <returns>DataTable</returns>
        public DataTable GetQueueRecordAuditData(QueueFrameworkSearchDataContract queueSearchDataContract, CustomPagingArgsContract queueAuditArgsContract)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                string orderBy = null;
                string ordDirection = null;
                Int32? selectedQueueId = null;
                Int32? SelectedBusinessProcessId = null;
                Int32? RecordId = null;
                if (queueSearchDataContract.SelectedQueueId > 0)
                    selectedQueueId = queueSearchDataContract.SelectedQueueId;
                if (queueSearchDataContract.SelectedBusinessProcessId > 0)
                    SelectedBusinessProcessId = queueSearchDataContract.SelectedBusinessProcessId;
                if (queueSearchDataContract.RecordId > 0)
                    RecordId = queueSearchDataContract.RecordId;

                orderBy = String.IsNullOrEmpty(queueAuditArgsContract.SortExpression) ? orderBy : queueAuditArgsContract.SortExpression;
                ordDirection = queueAuditArgsContract.SortDirectionDescending == false ? "asc" : "desc";

                SqlCommand command = new SqlCommand("usp_QueueAuditData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserTenantID", queueSearchDataContract.LoggedInUserTenantId);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", queueSearchDataContract.LoggedInUserId);
                command.Parameters.AddWithValue("@QueueID", selectedQueueId);
                command.Parameters.AddWithValue("@BusinessProcessID", SelectedBusinessProcessId);
                command.Parameters.AddWithValue("@RecordID", RecordId);
                command.Parameters.AddWithValue("@FromDate", queueSearchDataContract.FromDate);
                command.Parameters.AddWithValue("@ToDate", queueSearchDataContract.ToDate);
                command.Parameters.AddWithValue("@UserID", queueSearchDataContract.SelectedUserId);
                command.Parameters.AddWithValue("@OrderBy", orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection);
                command.Parameters.AddWithValue("@PageIndex", queueAuditArgsContract.CurrentPageIndex);
                command.Parameters.AddWithValue("@PageSize", queueAuditArgsContract.PageSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    //queueAuditArgsContract.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                    //queueAuditArgsContract.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    return ds.Tables[0];
                }
            }

            return new DataTable();
        }

        public List<Int32> GetOrganizationUserIdList(Int32 tenantID, Int32 queueId)
        {
            List<Int32> queueAssignmentConfigId = _dbContext.QueueAssignmentConfigurations.Where(cond => cond.QAC_TenantID == tenantID && cond.QAC_QueueID == queueId && !cond.QAC_IsDeleted).Select(select => select.QAC_ID).ToList();
            return _dbContext.QueueAssigneesLists.Where(cnd => queueAssignmentConfigId.Contains(cnd.QAL_AssignmentConfigurationID)).Select(slct => slct.QAL_OrganizationUserID).ToList();
        }

        /// <summary>
        /// Retrieves all the data from the table OrganizationUser for the given Organization User Ids.
        /// </summary>
        /// <param name="lstUserIds">List of Organization User Ids</param>
        /// <returns>List of active organization users</returns>
        public List<OrganizationUser> GetOrganizationUsersByIds(List<Int32> lstUserIds)
        {
            return _dbContext.OrganizationUsers.Where(obj => obj.IsDeleted == false && lstUserIds.Contains(obj.OrganizationUserID) && obj.IsApplicant == false).ToList();
        }
        #endregion

        /// <summary>
        /// Handle assignment method that call the sp_HandleAssignment to handle the automatic assignment.
        /// </summary>
        /// <param name="dicHandleAssignmentData">dicHandleAssignmentData</param>
        public void HandleReconciliationAssignment(Dictionary<String, Object> dicHandleAssignmentData)
        {
            if (dicHandleAssignmentData.IsNotNull())
            {
                Int32 currentLoggedInUserId;
                Int32 tenantId;
                String queueRecordXML = dicHandleAssignmentData.GetValue("QueueRecordXML") as String;
                dicHandleAssignmentData.TryGetValue("CurrentLoggedInUserId", out currentLoggedInUserId);
                dicHandleAssignmentData.TryGetValue("TenantId", out tenantId);
                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {

                    SqlCommand command = new SqlCommand("usp_ReconciliationHandleAssignment", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenantID", tenantId);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                    command.Parameters.AddWithValue("@QueueRecordXML", queueRecordXML);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                }
            }
        }

        public void ResetReconciliationProcess(Int32 currentLoggedInUserId, Int32 recordID, Boolean isProcessCompeted, Int32 tenantId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ResetReconciliationProcess", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@RecordID", recordID);
                command.Parameters.AddWithValue("@IsProcessCompeted", isProcessCompeted);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public void ClearReconciliationQueueRecords(Int32 currentLoggedInUserId, String inputXML,Int32 tenantId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ClearReconciliationRecords", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@QueueRecordXML", inputXML);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public void ReconcillationOverRideByClntAdmin(Int32 currentLoggedInUserId, String inputXML, Int32 tenantId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ReconcillationOverRideByClntAdmin", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@QueueRecordXML", inputXML);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
