using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    /// <summary>
    /// Contract used to carry the data for Verification details screen from UI to the Manager & DAL Layers
    /// </summary>
    public class VerificationDetailsContract
    {
        public ApplicantComplianceCategoryData applicantCategoryData { get; set; }
        public ApplicantComplianceItemData applicantItemData { get; set; }
        public List<ApplicantComplianceAttributeData> lstApplicantData { get; set; }
        public Int32 createdModifiedById { get; set; }
        public String adminComments { get; set; }
        public Int32 newStatus { get; set; }
        public Int16 reviewerTypeId { get; set; }
        public Int32? reviewerTenantId { get; set; }
        public Int32 thirdPartyReviewerUserId { get; set; }
        public Int32 applicantId { get; set; }
        public Boolean isAdminReviewRequired { get; set; }
        public String newItemStatusCode { get; set; }
        public String currentTenantTypeCode { get; set; }
        public Int32 packageId { get; set; }
    }

    /// <summary>
    /// Represents the Specialization of Items for a patrticular user
    /// </summary>
    public class UserSpecializationDetails
    {
        /// <summary>
        /// Acts as a Primary key to identify the records, especially items with ApplicantComplianceItemId = 0
        /// </summary>
        public Int32 ReferenceId { get; set; }

        public Int32 QueueId { get; set; }
        public Boolean IsSpecializedUser { get; set; }
        public Int32 RecordId { get; set; }
        public Int32 SpecializedUserCount { get; set; }
    }

    /// <summary>
    /// Represents the Next possible action for the Items.
    /// </summary>
    public class NextQueueAction
    {  
        /// <summary>
        /// Acts as a Primary key to identify the records, especially items with ApplicantComplianceItemId = 0
        /// </summary>
        public Int32 ReferenceId { get; set; }
        public Int32? CurrentReviewLevel{ get; set; }
        public String NextAction{ get; set; }
        public Int32? NextReviewLevel{ get; set; }
        public Int32 RecordId { get; set; }
        public Int32 QueueId { get; set; }
        public Int32 MaxReviewLevels { get; set; } 
    }
}
