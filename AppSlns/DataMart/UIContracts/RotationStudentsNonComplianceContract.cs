using System;

namespace DataMart.UI.Contracts
{
    public class RotationStudentsNonComplianceContract
    {
        public String TenantName { get; set; }
        public Int32 AgencyID { get; set; }
        public String Agency { get; set; }
        public String ComplioID { get; set; }
        public String StudentName { get; set; }
        public String StudentEmail { get; set; }
        public DateTime? OutOfComplianceDate { get; set; }
        public String NonComplianceRequirements { get; set; }
        public String SharedBy { get; set; }
        public String SharedByEmail { get; set; }
        public String ReviewStatus { get; set; }
        public String ReviewedBy { get; set; }
        public DateTime? RotationEndDate { get; set; }
        public String UserType { get; set; }     //UAT-4901

    }
}
