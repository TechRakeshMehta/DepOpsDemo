using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class BulkOrderUploadContract
    {
        public String InstituteName { get; set; }
        public Int32? OrganizationUserId { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public String EmailAddress { get; set; }
        public Int32? PackageID { get; set; }
        public Int32? OrderNodeID { get; set; }
        public Int32? HierarchyNodeID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Int32? Interval { get; set; }
        public String Notes { get; set; }
        public Int32 BulkOrderUploadID { get; set; }
        public Int32 BulkOrderUploadFileID { get; set; }
        public Int32? BulkOrderStatusID { get; set; }
        public String BulkOrderStatusCode { get; set; }
        public String BulkOrderStatusName { get; set; }
        public Int32 TotalCount { get; set; }
    }
}
