using System;

namespace DataMart.UI.Contracts
{
    public class CategoryDataReportWithComplioIDContract
    {
        public String ID { get; set; }
        public String SharedItemType { get; set; }
        public String TenantName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime? RotationStartDate { get; set; }
        public DateTime? RotationEndDate { get; set; }
        public String CategoryName { get; set; }
        public String ItemName { get; set; }
        public String FieldName { get; set; }
        public String FieldData { get; set; }
        public String CategoryComplianceStatus { get; set; }
        public String ApprovedOverrideStatus { get; set; }
        public String ComplioID { get; set; }
        public String CustomAttribute { get; set; }

        public String ReviewStatus { get; set; }
        public String UserType { get; set; }
    }
}
