using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class SheduledInvitationContract
    {
        public Int32 InvitationID { get; set; }
        public Int32 TenantID { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public Int32 InvitationGroupID { get; set; }
        public Int32 InvitationStatusID { get; set; }
        public Int32 ApplicantID { get; set; }
        public Int32 RotationID { get; set; }
        public String InviteeSharedMetadataCodes { get; set; }
        public String InviteeUserTypeCode { get; set; }
        public String ComplianceSharedInfoTypeCode { get; set; }
        public String RequiredSharedInfoTypeCode { get; set; }
        public String BkgSharedInfoTypeCode { get; set; }
        public String InviteeName { get; set; }
        public Guid InvitationToken { get; set; }
        public String EmailAddress { get; set; }
        public String SharedApplicantMetaDataIds { get; set; }

        public Boolean IsTenantDataSaved { get; set; }
        public Int32? InviteeUserId { get; set; }

        public Int32 InvitationInitiatedByID { get; set; }
        public String InvitationInitiatedUserFullName { get; set; }
        public String InvitationInitiatedUserEmailId { get; set; }
        public String InviteeAgency { get; set; }
        public Int32 AgencyID { get; set; }
    }
}
