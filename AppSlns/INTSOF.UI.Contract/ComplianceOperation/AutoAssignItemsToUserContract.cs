using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class AutoAssignItemsToUserContract
    {
        public Int32 OrganizationUserID { get; set; }
        public Int32 TotalBucketCount { get; set; }
        public DateTime SubmissionStartDate { get; set; }
        public DateTime SubmissionEndDate { get; set; }
        public Int32 TenantID { get; set; }
        public List<Int32> MappedTenants { get; set; }
        public Int32 CommonBucketCount { get; set; }
        


    }
    public class AutoAssignItemsToUserListContract
    {
        public String verficationQueueDataXml { get; set; }
        public List<AutoAssignItemsToUserContract> autoAssignItemsToUserList { get; set; }
        public List<AutoAssignQueueRecords> autoAssignQueueRecordsList { get; set; }
        public List<Int32> TenantIds { get; set; }
        public List<Int32> ProcessedApplicantComplienceItemId { get; set; }
        public ItemVerificationQueueData verificationQueueData { get; set; }
        public Int32 clientId { get; set; }
        public CustomPagingArgsContract VerificationGridCustomPaging
        {
            get;
            set;
        }
        public String CustomDataXML
        {
            get;
            set;
        }
        public String DPMIds { get; set; }

        public Boolean IsMultiTenant { get; set; }
        public String MultiTenantInputXml { get; set; }
        public CustomPagingArgsContract MultiTenantGridCustomPaging
        {
            get;
            set;
        }
    }
    public class AutoAssignQueueRecords
    {
        public Int32 TenantID { get; set; }
        public Boolean isProcessed { get; set; }
        public Int32 ApplicantComplienceItemId { get; set; }
        public Dictionary<String, Object> dicQueueFields
        {
            get;
            set;
        }

        public Int32 queueId
        {
            get;
            set;
        }
    }
}
