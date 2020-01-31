using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class ReconciliationDetailsDataContract
    {
        public Int32? ReviewerID { get; set; }
        public Int32? PkgSubscriptionID { get; set; }
        public Int32? CategoryDataID { get; set; }
        public Int32? ItemDataID { get; set; }
        public Int32? AttributeDataID { get; set; }
        public Int32? CompliancePackageID { get; set; }
        public Int32? ComplianceCategoryID { get; set; }
        public Int32? ComplianceItemID { get; set; }
        public Int32? ComplianceAttributeID { get; set; }
        public String ItemStatusCode { get; set; }
        public String AttributeValue { get; set; }
        public String PackageName { get; set; }
        public String CategoryName { get; set; }
        public String ItemName { get; set; }
        public String AttributeName { get; set; }
        public String AttributeDataTypeCode { get; set; }
        public String ReviewerName { get; set; }
        public String ItemComplianceStatus { get; set; }
        public String ItemComplianceStatusDescription { get; set; }
        public Int32 ApplicantComplianceReconciliationDataID { get; set; }
        public Boolean IsUiRulesViolate { get; set; }
    }
}
