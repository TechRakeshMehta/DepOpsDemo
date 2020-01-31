using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    public class RequirementSharesDataContract
    {
        public Int32 RotationID { get; set; }
        public String ComplioID { get; set; }
        public String RotationName { get; set; }
        public String Department { get; set; }
        public String Program { get; set; }
        public String Course { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public Int32 AgencyID { get; set; }
        public String AgencyName { get; set; }
        public String Term { get; set; }
        public String UnitFloorLoc { get; set; }
        public String RecommendedHours { get; set; }
        public String Students { get; set; }
        public String Shift { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public String TypeSpecialty { get; set; }
        public String Time { get; set; }
        public String DaysIdList { get; set; }
        public String Days { get; set; }
        public Boolean IsInstructorPreceptorPkgAvailable { get; set; }
        public Boolean IsProfileShared { get; set; }
        public Int32 RotationReviewID { get; set; }
        public String RotationReviewStatusName { get; set; }
        public String RotationDroppedDate { get; set; }
        public Int32 TotalRecordCount { get; set; }
        public Int32 ProfileSharingInvGroupID { get; set; }

        public Int32 InvitationID { get; set; }
        public String ApplicantName { get; set; }
        public String EmailAddress { get; set; }
        public String Phone { get; set; }
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public String ExpirationDate { get; set; }
        public String InvitationDate { get; set; }
        public String LastViewedDate { get; set; }
        public String ViewsRemaining { get; set; }
        public Int32 InviteTypeID { get; set; }
        public String InviteTypeCode { get; set; }
        public String InviteTypeName { get; set; }
        public String Notes { get; set; }
        public Boolean IsInvitationVisible { get; set; }
        public Int32 ApplicantOrgUserID { get; set; }
        public String SharedUserInvitationReviewStatusCode { get; set; }
        public Int32 InviteeOrgUserID { get; set; }
        public Boolean IsRotationSharing { get; set; }
        public Boolean IsRotationDropped { get; set; }
        public String ReviewBy { get; set; }

        public String RotationSharedByUserName { get; set; }

        public String RotationSharedByUserEmailId { get; set; }

        public String RotationSharedByUserPhoneNumber { get; set; }
        public String CustomAttributes { get; set; } //UAT-3165
        public Boolean IsInvSharedByAppByAgencyDDl { get; set; } //UAT-3295
        public Boolean IsIndividualShare { get; set; }


        public String InstructorName { get; set; }
        public String ApplicantFirstName { get; set; }

        public String InstrEmailAddress { get; set; }
        public String InstrPhoneNumber { get; set; }
        public String ApplicantLastName { get; set; }
        public Int32 RotationSharedByUserID { get; set; }
        public String InstrDetails { get; set; }

    }
}
