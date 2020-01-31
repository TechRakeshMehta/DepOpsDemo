using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ClinicalRotation
{
    [Serializable]
    public class ClinicalRotationMemberDetail
    {
        public Int32 RotationID { get; set; }
        public String AgencyName { get; set; }
        public String RotationName { get; set; }
        public String Department { get; set; }
        public String Program { get; set; }
        public String Course { get; set; }
        public String UnitFloorLoc { get; set; }
        public float? RecommendedHours { get; set; }
        public String Shift { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Int32 OrganizationUserId { get; set; }
        public String ApplicantName { get; set; }
        public String PrimaryEmailaddress { get; set; }
        public String ComplioID { get; set; }
        public String Term { get; set; }
        public String Time { get; set; }
        public String DaysName { get; set; }
        public String TypeSpecialty { get; set; }
        public Int32 RotationMemberId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public String DaysIdList { get; set; }
        //UAT-2290
        public DateTime? DeadlineDate { get; set; }

        public String SchoolContactName { get; set; }//UAT-3006
        public String SchoolContactEmailID { get; set; }//UAT-3006

        public String RotationHirarchyIds { get; set; } //UAT-3254
        public String InstructorPreceptor { get; set; } //UAT-3662
    }
}
