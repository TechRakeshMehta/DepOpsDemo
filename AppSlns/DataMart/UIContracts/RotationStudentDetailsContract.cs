using System;

namespace DataMart.UI.Contracts
{
    public class RotationStudentDetailsContract
    {
        public String ID { get; set; }
        public Int32 InvitationGroupID { get; set; }
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public String AgencyName { get; set; }
        public Int32 StudentID { get; set; }
        public String StudentFirstName { get; set; }
        public String StudentLastName { get; set; }
        public String StudentPhoneNumber { get; set; }
        public String StudentDOB { get; set; }
        public String StudentEmailAddress { get; set; }
        public String RotationID { get; set; }
        public String Address { get; set; }
        public String ComplioID { get; set; }
        public String RotationName { get; set; }
        public String TypeSpecialty { get; set; }
        public String Department { get; set; }
        public String Program { get; set; }
        public String Course { get; set; }
        public String Term { get; set; }
        public String UnitFloorLoc { get; set; }
        public String RotationShift { get; set; }
        public String Times { get; set; }
        public DateTime? RotationStartDate { get; set; }
        public DateTime? RotationEndDate { get; set; }
        public Int32 AgencyID { get; set; }
        public String InvitationReviewStatusName { get; set; }
        public String InvitationSourceType { get; set; }
        public String InvitationSourceCode { get; set; }
        public String Days { get; set; }
        public String CustomAttributes { get; set; }
        public String ReviewCode { get; set; }
        public String UserType { get; set; }
        public String SharedBy { get; set; }
        public String SharedByEmail { get; set; }
        public String ReviewedBy { get; set; }
    }
}
