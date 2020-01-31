using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class DiscardedDocumentAuditContract
    {
        public Int32 QueueRecordID { get; set; }
        public Int32 SelectedTenantID { get; set; }
        public Int32 AdminLoggedInUserID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public String DocumentName { get; set; }
        public String InstitutionName { get; set; }

        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public String StudentName { get; set; }

        public String AdminFirstName { get; set; }
        public String AdminLastName { get; set; }
        public String DiscardedBy { get; set; }

        public String DiscardedReason { get; set; }
        public Int32 DiscardByUserID { get; set; }
        public DateTime? DiscardedOn { get; set; }
        public Int32 DiscardedCount { get; set; }

        #region Custom Paging
        public String OrderBy { get; set; }
        public String OrderDirection { get; set; }
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 TotalCount { get; set; }
        #endregion        

    }
}
