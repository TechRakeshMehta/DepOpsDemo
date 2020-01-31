using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class MultiInstitutionAssignmentDataContract
    {

        public String ApplicantName
        {
            get;
            set;
        }

        public String ItemName
        {
            get;
            set;
        }
        public String CategoryName
        {
            get;
            set;
        }
        public String PackageName
        {
            get;
            set;
        }

        public DateTime? SubmissionDate
        {
            get;
            set;
        }
        public String VerificationStatus
        {
            get;
            set;
        }

        public Int32 TotalCount
        {
            get;
            set;
        }
        public String SystemStatus
        {
            get;
            set;
        }
        public String RushOrderStatus
        {
            get;
            set;
        }

        public String ReviewLevel
        {
            get;
            set;
        }
        public String AssignedUserName
        {
            get;
            set;
        }
        
        public String CustomAttributes
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceItemId
        {
            get;
            set;
        }
        public Int32 ApplicantId
        {
            get;
            set;
        }
        public Int32 ApplicantComplianceCategoryId
        {
            get;
            set;
        }
        public Int32 TenantID
        {
            get;
            set;
        }

        public String AdminNote
        {
            get;
            set;
        }
        public Boolean IsDirty
        {
            get;
            set;
        }
        public Int32 CategoryId
        {
            get;
            set;
        }
        public Int32 ComplianceItemId
        {
            get;
            set;
        }
        public String VerificationStatusCode
        {
            get;
            set;
        }
        public Int32 FVDId
        {
            get;
            set;
        }
        public String InstitutionName
        {
            get;
            set;
        }

        public List<String> FilterColumns { get; set; }
        public List<String> FilterOperators { get; set; }
        public ArrayList FilterValues { get; set; }
        public List<String> FilterTypes { get; set; }
        public Boolean IsUiRulesViolate { get; set; }
    }
}
