using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.QueueManagement.Views
{
    public interface IQueueAssignmentControlView
    {
        Int32 AssignmentConfigurationId
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        QueueConfirgurationType QueueType
        {
            get;
            set;
        }

        List<Tenant> lstTenant
        {
            get;
            set;
        }

        List<QueueMetaData> lstQueueData
        {
            get;
            set;
        }

        List<lkpQueueAutomationLevel> lstQueueAutomationLevel
        {
            get;
            set;
        }

        List<lkpQueueDistributionStrategy> lstQueueDistributionStrategy
        {
            get;
            set;
        }

        Int32 SelectedQueueId
        {
            get;
            set;
        }

        Int32 CurrentQueueId
        {
            get;
            set;
        }

        String AssignmentDescription
        {
            get;
            set;
        }

        Int16 SelectedAutomationLevelId
        {
            get;
            set;
        }

        Int16? RecordDistributionStrategyId
        {
            get;
            set;
        }

        Int16 NumberOfReviews
        {
            get;
            set;
        }

        List<QueueFieldsMetaData> lstQueueFieldsMetaData
        {
            get;
            set;
        }

        Int32 SelectedQueueFieldID
        {
            get;
            set;
        }

        String SelectedQueueFieldValue
        {
            get;
            set;
        }

        String CurrentQueueFieldValue
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<GetQueueSpecilizationCriterion> LstQueueSpecilizationCriterion
        {
            get;
            set;
        }

        Int16 assignmentPrecedence
        {
            get;
            set;
        }

        Int16 ConfigurationTypeId
        {
            get;
        }

        List<GetQueueAssigneeList> lstReviewerUsers
        {
            get;
            set;
        }
    }
}
