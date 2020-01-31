using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class AdminDataAuditHistory
    {

        public Int32 SelectedTenantID { get; set; }
        public Int32 AdminLoggedInUserID { get; set; }
        public Int32 QueueRecordID { get; set; }
        public Int32 ApplicantDocumentID { get; set; }
        public String ActionType { get; set; }
        public Int32? ActionTypeID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public String DocumentName { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }

        public String AdminFirstName { get; set; }
        public String AdminLastName { get; set; }
        public String Changes { get; set; }
        public Int32? DiscardReasonId { get; set; }
        public String DiscardReason { get; set; }
        public String DiscardNote { get; set; }
        public Int32 CreatedById { get; set; }
        public DateTime? CreatedOn { get; set; }

        public String OrderBy { get; set; }
        public String OrderDirection { get; set; }
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 TotalCount { get; set; }

        public String AssignToUserName { get; set; }
        public Int32? AssignToUserID { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public Int32? AssignBy { get; set; }
        public String School { get; set; }

        public DateTime? AssignOn { get; set; }
        public String AssignToName { get; set; }
        public String AssignByName { get; set; }

        
    }
}
