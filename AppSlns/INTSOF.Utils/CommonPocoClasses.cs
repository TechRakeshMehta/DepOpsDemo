using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommonPokoClasses
/// </summary>
namespace INTSOF.Utils.CommonPocoClasses
{
    [Serializable]
    public class ComplianceCategoryPocoClass
    {

        public Int32 CategoryId { get; set; }
        public String CategoryName { get; set; }
        public Int32? CategoryStatusId { get; set; }
        public string CategoryStatusCode { get; set; }
        public string CategoryStatusName { get; set; }
        public Int32 DisplayOrder { get; set; }
        public Int32? CategoryExceptionStatusID { get; set; }
        public String CategoryExceptionStatusCode { get; set; }

        /// <summary>
        /// Returns whether the Current Category Compliance is Required or Not.
        /// </summary>
        public Boolean IsComplianceRequired { get; set; }
        public Boolean IsActualComplianceRequired { get; set; } //UAT-3611
        /// <summary>
        /// Represents the Start date for the compliance settings
        /// </summary>
        public DateTime? ComplianceStartDate { get; set; }

        /// <summary>
        /// Represents the End date for the compliance settings
        /// </summary>
        public DateTime? ComplianceEndDate { get; set; }

        public Int32 PackageId { get; set; }

        public Int32 CPC_ID { get; set; }

        public String RulesStatusID { get; set; }

    }

    [Serializable]
    public class ApplicantDocumentPocoClass
    {
        public Int32 ApplicantDocumentID { get; set; }
        public String FileName { get; set; }
        public String DocumentPath { get; set; }
        public String PdfDocPath { get; set; }
        public Boolean IsCompressed { get; set; }
        public Int32? Size { get; set; }
        //UAT-2628
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public Int32 OrganizationUserId { get; set; }
        public Int32 DocumentRetryCount { get; set; }
        public String PrimaryEmailAddress { get; set; }
        public Nullable<Int32> TotalPages { get; set; } //UAT-3238
    }
}