using System;

namespace DataMart.UI.Contracts
{
    public class RotationStudentsByDayContract
    {
        public String TenantName { get; set; }
        public String AgencyName { get; set; }
        public Int32 StudentID { get; set; }
        public String StudentFirstName { get; set; }
        public String StudentLastName { get; set; }
        public String StudentEmailAddress { get; set; }
        public String RotationID { get; set; }
        public String ComplioID { get; set; }
        public String Days { get; set; }
        public String InvitationReviewStatusName { get; set; }
        public String SharedBy { get; set; }
        public String SharedByEmail { get; set; }
    }
}
