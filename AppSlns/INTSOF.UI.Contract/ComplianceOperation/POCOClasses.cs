using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    /// <summary>
    /// Represents the data of the documents mapped to items for data entry or Exception items, in Verification Details screen
    /// </summary>
    public class ApplicantDocumentMappingData
    {
        /// <summary>
        /// Will be null for Data entry item
        /// </summary>
        public Int32 ExceptionDocumentMappingId{ get; set; }

        public Int32 ApplicantComplianceItemId { get; set; }

        /// <summary>
        /// Will be null for Exception related cases
        /// </summary>
        public Int32 ApplicantComplianceDocumentMapId { get; set; }

        /// <summary>
        /// Will be null for Exception related cases
        /// </summary>
        public Int32 ApplicantComplianceAttributeId { get; set; }

        public Int32 ApplicantDocumentId { get; set; }
        public Boolean IsExceptionDocument{ get; set; }
    }

    /// <summary>
    /// Reresents the data fetched based on List of RuleMappingIds, during UI rule validation in Verification Details
    /// </summary>
    public class RuleMappingDetailsData
    {
        public Int32 RuleMappingId { get; set; }
        public Int32 RuleSetId { get; set; }
        public Int32 AssignmentHierarchyId { get; set; }
        public Int32 ObjectId { get; set; }
        public String ObjectTypeCode { get; set; }
        public String AssignementHierarchy { get; set; }
    }
}
