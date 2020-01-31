using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.QueueManagement.Views
{
    public class QueueAssignmentControlPresenter : Presenter<IQueueAssignmentControlView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetTenants();
        }

        public void GetTenants()
        {
            View.lstTenant = ComplianceDataManager.getClientTenant();
        }

        public void GetAutomationLevel()
        {
            View.lstQueueAutomationLevel = QueueManagementManager.GetQueueAutomationLevelList(View.SelectedTenantId);
        }

        public void GetQueueDistributionStrategyList()
        {
            View.lstQueueDistributionStrategy = QueueManagementManager.GetQueueDistributionStrategyList(View.SelectedTenantId);
        }

        public void GetMetaDataQueueList()
        {
            if (View.SelectedTenantId == AppConsts.NONE)
                View.lstQueueData = new List<QueueMetaData>();
            else
                View.lstQueueData = QueueManagementManager.GetQueueMetaDataList(View.SelectedTenantId, View.QueueType, View.CurrentQueueId).Where(x=>!x.QMD_IsEscalationQueue).ToList();
        }

        public void getQueueMetaDataFieldsList()
        {
            if (View.SelectedQueueId == AppConsts.NONE)
                View.lstQueueFieldsMetaData = new List<QueueFieldsMetaData>();
            else
                View.lstQueueFieldsMetaData = QueueManagementManager.GetQueueFieldsMetaDataByQueueId(View.SelectedQueueId, View.SelectedTenantId);

        }

        public void getQueueSpecializationCriterionFieldsList()
        {
            if (View.SelectedQueueFieldID == AppConsts.NONE)
                View.LstQueueSpecilizationCriterion = new List<GetQueueSpecilizationCriterion>();
            else
            {
                QueueFieldsMetaData queueFieldsMetaData = QueueManagementManager.GetQueueFieldsMetaDataByQueueFieldId(View.SelectedQueueFieldID, View.SelectedTenantId);
                if (queueFieldsMetaData.QF_AssignmentPrecedence.HasValue)
                    View.assignmentPrecedence = queueFieldsMetaData.QF_AssignmentPrecedence.Value;
                View.LstQueueSpecilizationCriterion = QueueManagementManager.getQueueSpecializationCriterionFieldsList(queueFieldsMetaData.QF_DisplayFieldQuery, View.SelectedQueueFieldID, View.CurrentQueueFieldValue, View.SelectedTenantId);
            }
        }

        public Int16 GetQueueConfigurationTypeByCode(String configurationTypeCode)
        {
            return QueueManagementManager.GetQueueConfigurationTypeByCode(configurationTypeCode, View.SelectedTenantId);
        }

        public void SaveBasicAssignmentConfiguration()
        {
            QueueAssignmentConfiguration queueAssignmentConfiguration = CreateAssignmentConfiguration();
            QueueManagementManager.SaveUpdateQueueAssignmentConfiguration(queueAssignmentConfiguration, View.QueueType, View.CurrentLoggedInUserId, View.SelectedTenantId);
            View.AssignmentConfigurationId = queueAssignmentConfiguration.QAC_ID;
        }

        public void SaveSpecialisedAssignmentConfiguration()
        {
            QueueAssignmentConfiguration queueAssignmentConfiguration = CreateAssignmentConfiguration();
            QueueConfigurationMetaDataField queueConfigurationMetaDataField = new QueueConfigurationMetaDataField();
            queueConfigurationMetaDataField.QCMF_QueueFieldID = View.SelectedQueueFieldID;
            queueConfigurationMetaDataField.QCMF_QueueFieldValue = View.SelectedQueueFieldValue;
            queueConfigurationMetaDataField.QCMF_IsDeleted = false;
            queueConfigurationMetaDataField.QCMF_CreatedBy = View.CurrentLoggedInUserId;
            queueConfigurationMetaDataField.QCMF_CreatedOn = DateTime.Now;
            queueAssignmentConfiguration.QueueConfigurationMetaDataFields.Add(queueConfigurationMetaDataField);
            QueueManagementManager.SaveUpdateQueueAssignmentConfiguration(queueAssignmentConfiguration, View.QueueType, View.CurrentLoggedInUserId, View.SelectedTenantId);
            View.AssignmentConfigurationId = queueAssignmentConfiguration.QAC_ID;
            View.SelectedQueueId = queueAssignmentConfiguration.QAC_QueueID;
        }

        private QueueAssignmentConfiguration CreateAssignmentConfiguration()
        {
            QueueAssignmentConfiguration queueAssignmentConfiguration = new QueueAssignmentConfiguration();
            queueAssignmentConfiguration.QAC_ID = View.AssignmentConfigurationId;
            queueAssignmentConfiguration.QAC_QueueID = View.SelectedQueueId;
            queueAssignmentConfiguration.QAC_TenantID = View.SelectedTenantId;
            queueAssignmentConfiguration.QAC_Description = View.AssignmentDescription;
            queueAssignmentConfiguration.QAC_AutomationLevelID = View.SelectedAutomationLevelId;
            queueAssignmentConfiguration.QAC_RecordDistributionStrategyID = View.RecordDistributionStrategyId;
            queueAssignmentConfiguration.QAC_ReviewerCount = View.NumberOfReviews;
            queueAssignmentConfiguration.QAC_ConfigurationTypeId = View.ConfigurationTypeId;
            queueAssignmentConfiguration.QAC_IsDeleted = false;
            queueAssignmentConfiguration.QAC_CreatedBy = View.CurrentLoggedInUserId;
            queueAssignmentConfiguration.QAC_CreatedOn = DateTime.Now;
            return queueAssignmentConfiguration;
        }

        public void getCurrentAssignmentConfiguration()
        {
            QueueAssignmentConfiguration currentAssignmentConfiguration = QueueManagementManager.getAssignmentConfigurationById(View.AssignmentConfigurationId, View.SelectedTenantId);
            View.SelectedQueueId = currentAssignmentConfiguration.QAC_QueueID;
            View.CurrentQueueId = currentAssignmentConfiguration.QAC_QueueID;
            View.AssignmentDescription = currentAssignmentConfiguration.QAC_Description;
            View.SelectedAutomationLevelId = currentAssignmentConfiguration.QAC_AutomationLevelID.Value;
            //View.RecordDistributionStrategyId = currentAssignmentConfiguration.QAC_RecordDistributionStrategyID.Value;
            View.NumberOfReviews = currentAssignmentConfiguration.QAC_ReviewerCount.Value;
            if (View.QueueType == QueueConfirgurationType.SpecializedUserAssignment)
            {
                QueueConfigurationMetaDataField currentQueueConfigurationMetaDataFields = currentAssignmentConfiguration.QueueConfigurationMetaDataFields.FirstOrDefault();
                View.SelectedQueueFieldID = currentQueueConfigurationMetaDataFields.QCMF_QueueFieldID.Value;
                View.CurrentQueueFieldValue = currentQueueConfigurationMetaDataFields.QCMF_QueueFieldValue;
                View.SelectedQueueFieldValue = currentQueueConfigurationMetaDataFields.QCMF_QueueFieldValue;
            }
        }

        public void getQueueAssigneeListByConfiguration()
        {
            View.lstReviewerUsers = QueueManagementManager.getQueueAssigneeListByConfiguration(View.AssignmentConfigurationId, View.SelectedTenantId);
        }

        public List<GetUserListApplicableForReview> getUserApplicableForReview(Int32 currentReviewerId = 0)
        {
            return QueueManagementManager.getUserListApplicableForReview(View.SelectedQueueId, View.AssignmentConfigurationId, View.SelectedTenantId, currentReviewerId);
        }

        public void SaveUpdateQueueAssigneeAndReviewerLevel(Int32 selectedReviwerId, List<Int32> assignedReviewerLevels, Int32 assigneeId)
        {
            QueueAssigneesList newAssignee = new QueueAssigneesList();
            newAssignee.QAL_AssigneeID = assigneeId;
            newAssignee.QAL_AssignmentConfigurationID = View.AssignmentConfigurationId;
            newAssignee.QAL_OrganizationUserID = selectedReviwerId;
            newAssignee.QAL_IsDeleted = false;
            foreach (var reviewerLevel in assignedReviewerLevels)
            {
                QueueAssigneeReviewLevel reviewerLevelToBeAssigned = new QueueAssigneeReviewLevel();
                reviewerLevelToBeAssigned.QARL_ReviewLevel = reviewerLevel;
                reviewerLevelToBeAssigned.QARL_IsDeleted = false;
                newAssignee.QueueAssigneeReviewLevels.Add(reviewerLevelToBeAssigned);
            }
            QueueManagementManager.SaveUpdateQueueAssigneeAndReviewerLevel(newAssignee, View.SelectedTenantId, View.CurrentLoggedInUserId);
        }

        public Boolean DeleteQueueAssigneeAndReviewerLevel(Int32 assigneeId)
        {
            return QueueManagementManager.DeleteQueueAssignee(assigneeId,View.SelectedTenantId,View.CurrentLoggedInUserId);
        }
    }
}
