using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class AgencySharingDataContract
    {
        public Int32 OrganizationUserId { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public String EmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public String UserGroupName { get; set; }
        public String SSN { get; set; }
        public String InstitutionHierarchy { get; set; }
        public DateTime? LastSharedDate { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 IsInvitationApproved { get; set; } //UAT-2443
        public Int16 IsInstructor { get; set; } //UAT-3977
    }

    [Serializable]
    public class AgencyAttestationDetailContract
    {
        public Int32 AgencyID { get; set; }
        public String AgencyName { get; set; }
        public String AttestationReportText { get; set; }
        public Int32 ClientSystemDocumentID { get; set; }
        public String DocPath { get; set; }
        public String DocFileName { get; set; }
        public Boolean AttestationFormSettingValue { get; set; }
    }
}
