using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class ApplicantDataAuditHistoryContract
    {
        public Int32 ApplicantDataAuditMultiTenantID { get; set; }
        public Int32 ApplicantDataAuditID { get; set; }
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public String PackageName { get; set; }
        public String CategoryName { get; set; }
        public String ItemName { get; set; }
        public String ApplicantName { get; set; }
        public String AdminName { get; set; }
        public DateTime? TimeStampValue { get; set; }
        public String ChangeValue { get; set; }
        public Int32 TotalCount { get; set; }
        public String AdminNameInitials { get; set; }

    }
}
