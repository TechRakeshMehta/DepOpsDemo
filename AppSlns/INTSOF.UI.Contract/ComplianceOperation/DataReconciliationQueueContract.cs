using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    /// <summary>
    /// class used to bind Data Reconciliation Queue.
    /// </summary>
    [Serializable]
    public class DataReconciliationQueueContract
    {
        public Int32 FlatComplianceItemReconciliationDataID { get; set; }
        public String ApplicantName { get; set; }
        public Int32 ApplicantComplianceItemId { get; set; }
        public Int32 ApplicantComplianceCategoryId { get; set; }
        public Int32 ComplianceItemId { get; set; }
        public String ItemName { get; set; }
        public Int32 CategoryID { get; set; }
        public String CategoryName { get; set; }
        public Int32 PackageID { get; set; }
        public String PackageName { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public String Reviewers { get; set; }
        public String InstitutionName { get; set; }
        public Boolean IsActive { get; set; }
        public Int32 ApplicantId { get; set; }
        public Int32 PackageSubscriptionID { get; set;}
        public Int32 OrderId { get; set; }
        public Int32 TenantId { get; set; }


        public List<String> FilterColumns { get; set; }
        public List<String> FilterOperators { get; set; }
        public ArrayList FilterValues { get; set; }
        public List<String> FilterTypes { get; set; }
        public List<Int32> selectedTenantIds { get; set; }
    }
}
