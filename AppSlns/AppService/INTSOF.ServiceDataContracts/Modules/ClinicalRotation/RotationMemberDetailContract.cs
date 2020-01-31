using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class RotationMemberDetailContract
    {
        [DataMember]
        public Int32 ClinicalRotationMemberId { get; set; }
        [DataMember]
        public ApplicantDataListContract RotationMemberDetail { get; set; }
        [DataMember]
        public String SchoolCompliance { get; set; }
        [DataMember]
        public String AgencyCompliance { get; set; }
        [DataMember]
        public int SchoolCompliancePackageID { get; set; }
        [DataMember]
        public int SchoolPackageSubscriptionID { get; set; }
        [DataMember]
        public int RequirementPackageID { get; set; }
        [DataMember]
        public int RequirementSubscriptionId { get; set; }
        //UAT-2544
        [DataMember]
        public Boolean IsDropped { get; set; }
        //UAT-3350
        [DataMember]
        public Boolean IsInstructor { get; set; }
        //UAT-2544
        [DataMember]
        public Boolean IsRotationStart { get; set; }
        [DataMember]
        public DateTime? StudentDroppedDate { get; set; }
        [DataMember]
        public Int32 RotationMemberRowIndex
        {
            get
            {
                if (IsInstructor)
                {
                    return (-RotationMemberDetail.OrganizationUserId);
                }
                return ClinicalRotationMemberId;
            }
            set
            {
                if (IsInstructor)
                    RotationMemberDetail.OrganizationUserId = -value;
                else
                    ClinicalRotationMemberId = value;
            }
        }
    }

    [Serializable]
    [DataContract]
    public class ApplicantDataListContract
    {
        //UAT-4148
        [DataMember]
        public Int32 ClinicalRotaionId { get; set; }
        [DataMember]
        public Int32 ReqPkgSubscriptionId { get; set; }
        //END UAT-4148 
        [DataMember]
        public Int32 OrganizationUserId { get; set; }
        [DataMember]
        public String ApplicantFirstName { get; set; }
        [DataMember]
        public String ApplicantMiddleName { get; set; }
        [DataMember]
        public String ApplicantLastName { get; set; }
        [DataMember]
        public DateTime? DateOfBirth { get; set; }
        [DataMember]
        public DateTime? StudentDroppedDate { get; set; } //UAT-4460
        [DataMember]
        public String EmailAddress { get; set; }
        [DataMember]
        public String SSN { get; set; }
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public String InstituteName { get; set; }
        [DataMember]
        public String InstitutionHierarchy { get; set; }
        [DataMember]
        public Boolean IsUserGroupMatching { get; set; }
        [DataMember]
        public String UserGroups { get; set; }
        [DataMember]
        public String CustomAttributes { get; set; }
        [DataMember]
        public Int32 TotalUsersAssigned { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public DateTime? InvitationDate { get; set; }
        //Profile Sharing ID
        [DataMember]
        public Int32 ProfileSharingInvID { get; set; }
        [DataMember]
        public String PhoneNumber { get; set; }
        [DataMember]
        public Int32? ViewsRemaining { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public Boolean IsInvitationVisible { get; set; }
        [DataMember]
        public String ComplianceStatus { get; set; }
        [DataMember]
        public Boolean IsApplicant { get; set; }
        [DataMember]
        public String InvitationReviewStatus { get; set; }
        [DataMember]
        public String InvitationReviewStatusCode { get; set; }

        //UAT-2090: Complete Question 4 (C5) from UAT-2052.
        [DataMember]
        public String Notes { get; set; }

        [DataMember]
        public Boolean IsDropped { get; set; }


        [DataMember]
        public String ShareStatus { get; set; }

        [DataMember]
        public String ReviewBy { get; set; } //UAT-2555

        #region UAT-2705

        [DataMember]
        public String AgencyName { get; set; }

        [DataMember]
        public String InstructorName { get; set; }

        [DataMember]
        public Nullable<DateTime> RotationStartDate { get; set; }

        [DataMember]
        public Nullable<DateTime> RotationEndDate { get; set; }

        [DataMember]
        public String School { get; set; }

        #endregion

        #region UAT-2923
        [DataMember]
        public String RotationSharedByUserName { get; set; }
        [DataMember]
        public String RotationSharedByUserEmailId { get; set; }
        [DataMember]
        public Int32? RotationSharedByUserOrgUserId { get; set; } //UAT-3421
        #endregion
    }
}
