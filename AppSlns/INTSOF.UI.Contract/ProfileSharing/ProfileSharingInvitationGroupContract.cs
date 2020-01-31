using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class ProfileSharingInvitationGroupContract
    {
        public Int32 PSIG_ID { get; set; }
        public Int32? PSIG_AgencyID { get; set; }
        public Int32 PSIG_InvitationInitiatedByID { get; set; }
        public Boolean PSIG_IsDeleted { get; set; }
        public Int32 PSIG_CreatedByID { get; set; }
        public DateTime PSIG_CreatedOn { get; set; }
        public Int32? PSIG_ModifiedByID { get; set; }
        public DateTime? PSIG_ModifiedOn { get; set; }
        public Int32? PSIG_TenantID { get; set; }
        public String PSIG_AdminName { get; set; }
        public String PSIG_AssignedUnits { get; set; }
        public DateTime? PSIG_AttestationDate { get; set; }
        public DateTime? PSIG_ClinicalFromDate { get; set; }
        public DateTime? PSIG_ClinicalToDate { get; set; }
        public String PSIG_ProgramName { get; set; }
        public String PSIG_AttestationReportText { get; set; }
        public Int32? PSIG_ClinicalRotationID { get; set; }
        public Int32? PSIG_ProfileSharingInvitationGroupTypeID { get; set; }
        public String TenantName { get; set; }
        public String PSIG_AgencyName { get; set; }


        /// <summary>
        /// UAT-1507 WB: Updates to Attestation Details Grid (UI and new column)
        /// </summary>
        public DateTime? PSI_EffectiveDate { get; set; }
        public Int32 TotalRecordCount { get; set; }

        //UAT-1749: As an admin, I should be able to search on sent profile shares and rotation shares 
        public String PSI_IsInvitationViewed { get; set; }
    }
}
