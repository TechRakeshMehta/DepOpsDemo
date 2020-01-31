using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;
using Entity.ClientEntity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Business.RepoManagers
{
    public class QueueManagementManager
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static QueueManagementManager()
        {
            BALUtils.ClassModule = "QueueManagementManager";
        }

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Queue Parameter Assignment
        //This method not in use post implementation of UAT-1886.
        public static List<QueueMetaData> GetQueueMetaDataList(Int32 tenantId, QueueConfirgurationType queueConfigurationType, Int32 selectedQueueId)
        {
            try
            {
                List<QueueMetaData> queueMetaDataList = LookupManager.GetLookUpData<QueueMetaData>(tenantId).Where(cond => cond.QMD_IsDeleted == false).ToList();
                if (queueConfigurationType == QueueConfirgurationType.BasicAssignment)
                {
                    Int16 basicQueueConfigurationTypeId = GetQueueConfigurationTypeByCode(QueueConfirgurationType.BasicAssignment.GetStringValue(), tenantId);
                    List<Int32> alreadyConfiguredQueueIds = BALUtils.GetQueueManagementRepoInstance(tenantId).getAlreadyMappedQueueIds(basicQueueConfigurationTypeId);
                    if (alreadyConfiguredQueueIds != null && alreadyConfiguredQueueIds.Count > AppConsts.NONE)
                    {
                        foreach (Int32 queueId in alreadyConfiguredQueueIds)
                        {
                            queueMetaDataList.Remove(queueMetaDataList.FirstOrDefault(x => x.QMD_QueueID == queueId && x.QMD_QueueID != selectedQueueId));
                        }
                    }
                }
                return queueMetaDataList;
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

        //This method not in use post implementation of UAT-1886.
        public static List<lkpQueueAutomationLevel> GetQueueAutomationLevelList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpQueueAutomationLevel>(tenantId).ToList();
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

        //This method not in use post implementation of UAT-1886.
        public static List<lkpQueueDistributionStrategy> GetQueueDistributionStrategyList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpQueueDistributionStrategy>(tenantId).ToList();
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

        //This method not in use post implementation of UAT-1886.
        public static List<QueueFieldsMetaData> GetQueueFieldsMetaDataByQueueId(Int32 queueId, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<QueueFieldsMetaData>(tenantId).Where(cond => cond.QF_QueueID == queueId
                                                                    && cond.QF_DisplayFieldQuery != null
                                                                    && !cond.QF_IsDeleted).ToList();
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

        //This method not in use post implementation of UAT-1886.
        public static QueueFieldsMetaData GetQueueFieldsMetaDataByQueueFieldId(Int32 queueFieldId, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<QueueFieldsMetaData>(tenantId).FirstOrDefault(cond => cond.QF_QueueFieldID == queueFieldId
                                                                    && cond.QF_DisplayFieldQuery != null
                                                                    && !cond.QF_IsDeleted);
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

        //This method not in use post implementation of UAT-1886.
        public static List<GetQueueSpecilizationCriterion> getQueueSpecializationCriterionFieldsList(String query, Int32 queueFieldID, String currentQueueFieldValue, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantId).getQueueSpecializationCriterionFieldsList(query, queueFieldID, currentQueueFieldValue);
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

        //This method not in use post implementation of UAT-1886.
        public static Int16 GetQueueConfigurationTypeByCode(String code, Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpQueueConfirgurationType>(tenantId).FirstOrDefault(x => x.CT_Code == code
                                                                                                    && !x.CT_IsDeleted).CT_ID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return Convert.ToInt16(AppConsts.NONE);
        }

        //This method not in use post implementation of UAT-1886.
        public static Boolean SaveUpdateQueueAssignmentConfiguration(QueueAssignmentConfiguration queueAssignmentConfiguration, QueueConfirgurationType queueType, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantId).SaveUpdateQueueAssignmentConfiguration(queueAssignmentConfiguration, queueType, currentLoggedInUserId);
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

        //This method not in use post implementation of UAT-1886.
        public static QueueAssignmentConfiguration getAssignmentConfigurationById(Int32 assignmentConfigurationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantId).getAssignmentConfigurationById(assignmentConfigurationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new QueueAssignmentConfiguration();
        }

        /// <summary>
        /// Get QueueFieldMetaData on the basis of queueid and code
        /// </summary>
        /// <param name="queueId">queueId</param>
        /// <param name="tenantId">tenantId</param>
        /// <param name="code">code</param>
        /// <returns>QueueFieldsMetaData</returns>
        ///  //This method not in use post implementation of UAT-1886.
        public static QueueFieldsMetaData GetQueueFieldsMetaDataByQueueIdAndCode(Int32 queueId, Int32 tenantId, String code)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.QueueFieldsMetaData>(tenantId).Where(cond => cond.QF_QueueID == queueId
                                                                    && cond.QF_Code == code
                                                                    && !cond.QF_IsDeleted).FirstOrDefault();
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

        //This method not in use post implementation of UAT-1886.
        public static List<GetQueueAssigneeList> getQueueAssigneeListByConfiguration(Int32 assignmentConfigurationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantId).getQueueAssigneeListByConfiguration(assignmentConfigurationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<GetQueueAssigneeList>();
        }

        //This method not in use post implementation of UAT-1886.
        public static List<GetUserListApplicableForReview> getUserListApplicableForReview(Int32 queueID, Int32 assignmentConfigurationId, Int32 tenantId, Int32 currentReviewerId = 0)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantId).getUserListApplicableForReview(queueID, assignmentConfigurationId, tenantId, currentReviewerId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<GetUserListApplicableForReview>();
        }

        //This method not in use post implementation of UAT-1886.
        public static void SaveUpdateQueueAssigneeAndReviewerLevel(QueueAssigneesList assigneeData, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            try
            {
                BALUtils.GetQueueManagementRepoInstance(tenantId).SaveUpdateQueueAssigneeAndReviewerLevel(assigneeData, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        //This method not in use post implementation of UAT-1886.
        public static Boolean DeleteQueueAssignee(Int32 assigneeId, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantId).DeleteQueueAssignee(assigneeId, currentLoggedInUserId);
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

        #endregion

        #region QUEUE ENGINE

        /// <summary>
        /// This Parallel Task method is used to call the Handle Assignment method of queue Engine
        /// </summary>
        /// <param name="HandleAssignment">Is delegate that refer the Handle Assignment method of queue engine</param>
        /// <param name="handleAssignmentData">HandleAssignment data</param>
        /// <param name="loggerService">LoggerService (HttpContext.Current.ApplicationInstance of ISysXLoggerService )</param>
        /// <param name="exceptionService">ExceptionService (HttpContext.Current.ApplicationInstance of ISysXExceptionService)</param>
        public static void RunParallelTaskHandleAssignment(Dictionary<String, Object> handleAssignmentData, ISysXLoggerService loggerService, ISysXExceptionService exceptionService, Int32 tenantId)
        {
            try
            {
                ParallelTaskContext.PerformParallelTask(HandleAssignment, handleAssignmentData, loggerService, exceptionService); //UAT-815
                //BALUtils.GetQueueManagementRepoInstance(tenantId).RunParallelTaskHandleAssignment(handleAssignmentData, loggerService, exceptionService);
                //ParallelTaskContext.PerformParallelTask(BALUtils.GetQueueManagementRepoInstance(tenantId).HandleAssignment, handleAssignmentData, loggerService, exceptionService);
                //ParallelTaskContext.PerformParallelTask(HandleAssignment, handleAssignmentData, loggerService, exceptionService);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        //UAT-815
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicHandleAssignmentData"></param>
        public static void HandleAssignment(Dictionary<String, Object> dicHandleAssignmentData)
        {
            try
            {
                Int32 tenantId = 0;
                dicHandleAssignmentData.TryGetValue("TenantId", out tenantId);
                BALUtils.GetQueueManagementRepoInstance(tenantId).HandleAssignment(dicHandleAssignmentData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        /// <summary>
        /// This Parallel Task method is used to call the Handle Assignment method of queue Engine
        /// </summary>
        /// <param name="HandleAssignment">Is delegate that refer the Handle Assignment method of queue engine</param>
        /// <param name="handleAssignmentData">HandleAssignment data</param>
        /// <param name="loggerService">LoggerService (HttpContext.Current.ApplicationInstance of ISysXLoggerService )</param>
        /// <param name="exceptionService">ExceptionService (HttpContext.Current.ApplicationInstance of ISysXExceptionService)</param>
        public static String GetQueueFieldXMLString(Dictionary<String, Object> dicQueueFields, Int32 queueId, Int32 recordId, Boolean notReviewed = false)
        {
            try
            {
                //return BALUtils.GetQueueManagementRepoInstance(tenantId).GetQueueFieldXMLString(dicQueueFields, queueId, recordId);
                if (dicQueueFields.IsNotNull() && dicQueueFields.Count > 0)
                {
                    StringBuilder queueFieldString = new StringBuilder();
                    foreach (KeyValuePair<String, Object> dicQueueField in dicQueueFields)
                    {
                        queueFieldString.Append("<QueueDetail>");
                        queueFieldString.Append("<QueueID>" + queueId + "</QueueID>");
                        queueFieldString.Append("<RecordID>" + recordId + "</RecordID>");
                        queueFieldString.Append("<QueueFieldName>" + dicQueueField.Key + "</QueueFieldName>");
                        queueFieldString.Append("<QueueFieldValue><![CDATA[" + dicQueueField.Value + "]]></QueueFieldValue>"); //Date 1-Sep-2014
                        queueFieldString.Append("<NotReviewed>" + notReviewed + "</NotReviewed>");
                        queueFieldString.Append("</QueueDetail>");
                    }
                    return queueFieldString.ToString();
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
        /// Gets the Next possible action on Verification details Save functionality
        /// </summary> 
        /// <returns></returns>
        public static List<NextQueueAction> GetNextQueueAction(String inputXML, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                DataTable _dt = BALUtils.GetQueueManagementRepoInstance(tenantId).GetNextQueueAction(inputXML, tenantId, currentLoggedInUserId);
                IEnumerable<DataRow> rows = _dt.AsEnumerable();
                return rows.Select(x => new NextQueueAction
                {
                    RecordId = Convert.ToInt32(x["RecordID"]),
                    QueueId = Convert.ToInt32(x["QueueId"]),
                    CurrentReviewLevel = String.IsNullOrEmpty(Convert.ToString(x["CurrentReviewLevel"])) ? AppConsts.NONE : Convert.ToInt32(x["CurrentReviewLevel"]),
                    NextReviewLevel = String.IsNullOrEmpty(Convert.ToString(x["NextReviewLevel"])) ? AppConsts.NONE : Convert.ToInt32(x["NextReviewLevel"]),
                    NextAction = Convert.ToString(x["NextAction"]),
                    ReferenceId = Convert.ToInt32(x["ReferenceID"]),
                    MaxReviewLevels = x["MaxReviewLevels"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(x["MaxReviewLevels"])
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<NextQueueAction>();
        }

        /// <summary>
        /// Get the specialization of the Items for a particular user.
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        public static List<UserSpecializationDetails> GetUserSpecializationDetails(Int32 currentLoggedInUserId, Int32 tenantId, String inputXML)
        {
            try
            {
                DataTable _dt = BALUtils.GetQueueManagementRepoInstance(tenantId).GetUserSpecializationDetails(currentLoggedInUserId, tenantId, inputXML);
                IEnumerable<DataRow> rows = _dt.AsEnumerable();
                return rows.Select(usd => new UserSpecializationDetails
                {
                    RecordId = Convert.ToInt32(usd["RecordID"]),
                    QueueId = Convert.ToInt32(usd["QueueID"]),
                    IsSpecializedUser = Convert.ToBoolean(usd["IsSpecializedUser"]),
                    ReferenceId = Convert.ToInt32(usd["ReferenceID"]),
                    SpecializedUserCount = Convert.ToInt32(usd["SpecializedUserCount"])
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new List<UserSpecializationDetails>();
        }

        /// <summary>
        /// Set Assigned to User Null for the items
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        public static Boolean ClearQueueRecords(Int32 currentLoggedInUserId, String inputXML, Int32 tenantId)
        {
            try
            {
                BALUtils.GetQueueManagementRepoInstance(tenantId).ClearQueueRecords(currentLoggedInUserId, inputXML);
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

        #endregion

        #region AssignmentConfigurationQueue
        /// <summary>
        /// To Fetch the Records corresponding to Configuration Queue Basic/Specialized
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="gridCustomPaging"></param>
        /// <param name="searchParameter"></param>
        /// <param name="queueTypeCode"></param>
        /// <returns></returns>
        ///  //This method not in use post implementation of UAT-1886.
        public static List<QueueAssignmentConfRecord> GetQueueAssignmentConfRecord(Int32 tenantID, CustomPagingArgsContract gridCustomPaging, String searchParameter, String queueTypeCode)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantID).GetQueueAssignmentConfRecord(gridCustomPaging, searchParameter, queueTypeCode);
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
        /// <summary>
        /// Lookup to get the List of Queues Name
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <returns></returns>
        ///  //This method not in use post implementation of UAT-1886.
        public static List<QueueMetaData> GetQueueDataList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<QueueMetaData>(tenantId).Where(cond => cond.QMD_IsDeleted == false).ToList();
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
        /// Get the FieldValueValue for Particular FieldValueName
        /// </summary>
        /// <param name="query">query:to get the FieldValue</param>
        /// <param name="tenantId">tenantId</param>
        /// <returns></returns>
        //public static List<GetQueueSpecilizationCriterion> GetQueueSpecializedFieldsList(String query, Int32 tenantId)
        //{
        //    try
        //    {
        //        return BALUtils.GetQueueManagementRepoInstance(tenantId).GetQueueSpecializedFieldsList(query);
        //    }

        //This method not in use post implementation of UAT-1886.
        public static List<GetQueueSpecilizationCriterion> GetQueueSpecializedFieldsList(String query, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantId).GetQueueSpecializedFieldsList(query);

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
        /// Delete the Record from the AssignmentConfiguration Queue
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="queueAssignmentConfID">queueAssignmentConfID of corresponding Record</param>
        /// <param name="currentLoggedInID">currentLoggedInID</param>
        /// <param name="queueType">queueType</param>
        /// <returns></returns>
        ///  //This method not in use post implementation of UAT-1886.
        public static Boolean DeleteQueueAssignmentConfigurationRecord(Int32 tenantId, Int32 queueAssignmentConfID, Int32 currentLoggedInID, QueueConfirgurationType queueType)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantId).DeleteQueueAssignmentConfigurationRecord(queueAssignmentConfID, currentLoggedInID, queueType);
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

        /// <summary>
        /// Lookup to get the List of Business Process
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <returns></returns>
        ///  //This method not in use post implementation of UAT-1886.
        public static List<lkpQueueBusinessProcess> GetBusinessProcessList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpQueueBusinessProcess>(tenantId).Select(x => x).ToList();
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

        #endregion

        /// <summary>
        /// Reset all business process for a record.
        /// </summary>
        /// <param name="businessProcessID">BusinessProcessID</param>
        /// <param name="recordID">RecordID</param>
        /// <returns>Int</returns>
        public static Int32 ResetBusinessProcess(Int32 tenantID, Int32 businessProcessID, Int32 recordID)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantID).ResetBusinessProcess(businessProcessID, recordID);
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
        /// Get User Current Assignment
        /// </summary>
        /// <param name="organizationUserID">OrganizationUserID</param>
        /// <param name="businessProcessID">BusinessProcessID</param>
        /// <param name="tenantID">TenantID</param>
        /// <returns>List of UserCurrentAssignments</returns>
        public static List<UserCurrentAssignments> GetUserCurrentAssignments(Int32 organizationUserID, Int32 businessProcessID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetQueueManagementRepoInstance(tenantID).GetUserCurrentAssignments(organizationUserID, businessProcessID, tenantID);
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
        /// Escalate the Items
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="inputXML"></param>
        public static Boolean EscalateItems(Int32 currentLoggedInUserId, String inputXML, Int32 tenantId)
        {
            try
            {
                BALUtils.GetQueueManagementRepoInstance(tenantId).EscalateItems(currentLoggedInUserId, inputXML);
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

        /// <summary>
        /// Get Queue Meta Data by code
        /// </summary>
        /// <param name="tenantID">TenantID</param>
        /// <param name="code">Code</param>
        /// <returns>QueueMetaData</returns>
        public static QueueMetaData GetQueueMetaDataByCode(Int32 tenantID, String code)
        {
            try
            {
                return LookupManager.GetLookUpData<QueueMetaData>(tenantID).Where(condition => condition.QMD_Code.Equals(code) && condition.QMD_IsDeleted == false).FirstOrDefault();
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

        #region QUEUE AUDIT

        /// <summary>
        /// Get the Queue audit data 
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="queueSearchDataContract">queueSearchDataContract</param>
        /// <param name="queueAuditArgsContract">queueAuditArgsContract</param>
        /// <returns>DataTable</returns>
        public static List<QueueAuditRecordContract> GetQueueRecordAuditData(Int32 tenantID, QueueFrameworkSearchDataContract queueSearchDataContract, CustomPagingArgsContract queueAuditArgsContract)
        {
            try
            {
                return NewAssignQueueAuditRecordToDataModel(BALUtils.GetQueueManagementRepoInstance(tenantID).GetQueueRecordAuditData(queueSearchDataContract, queueAuditArgsContract));
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
        /// Assign the datatable record in QueueAuditRecordContarct 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List<QueueAuditRecordContract></returns>
        private static List<QueueAuditRecordContract> NewAssignQueueAuditRecordToDataModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new QueueAuditRecordContract
                {
                    QueueRecordAuditDataID = x["QueueRecordAuditDataID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["QueueRecordAuditDataID"]),
                    QueueID = x["QueueID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["QueueID"]),
                    RecordID = x["RecordID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["RecordID"]),
                    UserName = x["UserName"].GetType().Name == "DBNull" ? "System Process" : x["UserName"].ToString(),
                    CreatedOn = x["CreatedOn"].GetType().Name == "DBNull" ? null : (DateTime?)x["CreatedOn"],
                    QueueRecord = x["QueueRecord"].ToString(),
                    PrevLifecycleFieldValue = x["PrevLifecycleFieldValue"].ToString(),
                    AttemptedLifecycleFieldValue = x["AttemptedLifeCycleFieldValue"].ToString(),
                    TotalCount = Convert.ToInt32(x["TotalCount"]),
                    ActualLifecycleFieldValue = x["ActualLifecyCleFieldValue"].ToString(),
                    IsActive = Convert.ToBoolean(x["IsActive"]) == true ? "True" : "False",
                    QueueName = x["QueueName"].ToString(),
                }).ToList();
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

        public static List<OrganizationUserContract> GetOrganizationUserListByQueueID(Int32 tenantID, Int32 queueId)
        {
            try
            {
                List<Int32> lstOrgUserId = BALUtils.GetQueueManagementRepoInstance(tenantID).GetOrganizationUserIdList(tenantID, queueId);
                List<OrganizationUser> lstOrgUser = BALUtils.GetQueueManagementRepoInstance(1).GetOrganizationUsersByIds(lstOrgUserId);
                List<OrganizationUserContract> orgUserContarctList = new List<OrganizationUserContract>();
                foreach (OrganizationUser orgUser in lstOrgUser)
                {
                    OrganizationUserContract orgUserContract = new OrganizationUserContract();
                    orgUserContract.OrganizationUserId = orgUser.OrganizationUserID;
                    orgUserContract.FirstName = orgUser.FirstName;
                    orgUserContract.LastName = orgUser.LastName;
                    orgUserContract.FullName = orgUser.FirstName + " " + orgUser.LastName;
                    orgUserContarctList.Add(orgUserContract);
                }
                //return lstOrgUser.Select(select => select = new OrganizationUserContract
                //{
                //    OrganizationUserId = select.OrganizationUserID,
                //    FirstName = select.FirstName,
                //    LastName = select.LastName,
                //    FullName = select.FirstName + " " + select.LastName

                //}).ToList();
                return orgUserContarctList;
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

        #endregion
        #endregion


        public static Entity.ReconciliationQueueConfiguration GetCurrentReconciliationAssignmentConfiguration()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCurrentReconciliationAssignmentConfiguration();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return new Entity.ReconciliationQueueConfiguration();
        }

        //public static List<GetQueueAssigneeList> GetCurrentReconciliationAssigneeList(Int32 currentAssignmentConfigurationId)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetCurrentReconciliationAssigneeList(currentAssignmentConfigurationId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    return new List<GetQueueAssigneeList>();
        //}

        //public static List<GetUserListApplicableForReview> GetUserListApplicableForReconciliationReview(Int32 assignmentConfigurationId, Int32 currentReviewerId = 0)
        //{
        //    try
        //    {
        //        return BALUtils.GetSecurityRepoInstance().GetUserListApplicableForReconciliationReview(assignmentConfigurationId, currentReviewerId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    return new List<GetUserListApplicableForReview>();
        //}

        public static Boolean SaveUpdateReconciliationQueueAssignmentConfiguration(Entity.ReconciliationQueueConfiguration queueAssignmentConfiguration, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveUpdateReconciliationQueueAssignmentConfiguration(queueAssignmentConfiguration, currentLoggedInUserId);
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

        public static void RunParallelTaskHandleReconciliationAssignment(Dictionary<String, Object> handleAssignmentData, ISysXLoggerService loggerService, ISysXExceptionService exceptionService, Int32 tenantId)
        {
            try
            {
                ParallelTaskContext.PerformParallelTask(HandleReconciliationAssignment, handleAssignmentData, loggerService, exceptionService);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void HandleReconciliationAssignment(Dictionary<String, Object> dicHandleAssignmentData)
        {
            try
            {
                Int32 tenantId = 0;
                dicHandleAssignmentData.TryGetValue("TenantId", out tenantId);
                BALUtils.GetQueueManagementRepoInstance(tenantId).HandleReconciliationAssignment(dicHandleAssignmentData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static List<INTSOF.UI.Contract.QueueManagement.ItemReconciliationAvailiblityContract> GetItemReconciliationAvailiblityStatus(Int32 tenantId, String itemIDs,Int32 subscriptionId)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetItemReconciliationAvailiblityStatus(tenantId, itemIDs, subscriptionId);
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

        public static void ResetReconciliationProcess(Int32 currentLoggedInUserId, Int32 recordID, Boolean isProcessCompeted, Int32 tenantID)
        {
            try
            {
                BALUtils.GetQueueManagementRepoInstance(tenantID).ResetReconciliationProcess(currentLoggedInUserId, recordID, isProcessCompeted,tenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static Boolean ClearReconciliationQueueRecords(Int32 currentLoggedInUserId, String inputXML, Int32 tenantId)
        {
            try
            {
                BALUtils.GetQueueManagementRepoInstance(tenantId).ClearReconciliationQueueRecords(currentLoggedInUserId, inputXML,tenantId);
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

        public static Boolean ReconcillationOverRideByClntAdmin(Int32 currentLoggedInUserId, String inputXML, Int32 tenantId)
        {
            try
            {
                BALUtils.GetQueueManagementRepoInstance(tenantId).ReconcillationOverRideByClntAdmin(currentLoggedInUserId, inputXML, tenantId);
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
    }
}
