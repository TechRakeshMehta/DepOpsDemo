using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class DashBoardRotationDataContract
    {
        public String Tenant { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 RotationId { get; set; }
        public String RotationDepartment { get; set; }
        public String RotationProgram { get; set; }
        public String RotationCourse { get; set; }
        public String RotationUnitFloorLoc { get; set; }
        public Int32 RotationNoOfHrs { get; set; }
        public DateTime? RotationStartDate { get; set; }
        public DateTime? RotationEndDate { get; set; }
        public String RotationReviewStatusCode { get; set; }
        public String RotationReviewStatus { get; set; }
        public String RotationDate { get; set; }
        public DateTime? RotationDroppedDate { get; set; }
        public String RotationTerm { get; set; }
        public String RotationTypeSpecialty { get; set; }
        public String RotationStartTime { get; set; }
        public String RotationEndTime { get; set; }
        public String RotationComplioID { get; set; }
        public String RotationShift { get; set; }
        public String RotationName { get; set; }
        public String Subject { get; set; }
        public String CurrentDate { get; set; }
        public DateTime? InvitationDate { get; set; }
        public Int32 ApplicantCount { get; set; }
        public String RotationDays { get; set; }
        public String AgencyName { get; set; }

        public Boolean IsShared { get; set; }
        public Boolean IsInvitationExpired { get; set; } //UAT-3424
        public String RotationFullDays { get; set; }
        public Int32 ProfileSharingInvitationID { get; set; }
       
        public Boolean IsRotationSharing { get; set; }

        public Boolean IsDropped { get; set; } //UAT-2972

        public String RotationStudentName { get; set; }
        public Int32? AgencyID { get; set; }

        public String CustomAttributes { get; set; } //UAT-3751
        public Boolean IsInvShdByAppByAgencyDDl { get; set; }//UAT--3928

        public Int32 ProfileSharingInvitationDetailId { get; set; }
        public Int32 ProfileSharingInvitationRotationDetailID { get; set; }
    }
}
