using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;

namespace DAL.Interfaces
{
    public interface IApplicantClinicalRotationRepository
    {
        Boolean IsApplicantClinicalRotationMember(Int32 applicantOrgUserID);

        /// <summary>
        /// Get Applicant Rotations for Listing
        /// </summary>
        /// <param name="applicantOrgUserId"></param>
        /// <param name="searchDataContract"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> GetApplicantClinicalRotations(Int32 applicantOrgUserId, ClinicalRotationDetailContract searchDataContract);

        List<ClinicalRotationDetailContract> GetApplicantClinicalRotationDetails(Int32 applicantOrgUserId, Int32? loggedInOrgUserID);

        Boolean IsApplicantDropped(int ApplicantOrgUserID, int clinicalRotationID);
        #region UAT-4248
        List<ClinicalRotationDetailContract> GetInstructorClinicalRotationDetails(Int32 instructorOrgUserId, Int32? loggedInOrgUserID);
        #endregion
        #region UAT-4313
        List<ClientContactNotesContract> GetClientContactNotes(Int32 clientContactId);
        Boolean SaveClientContactNotes(ClientContactNotesContract ccNotesContract, Int32 currentLoggedInUserId);
        Boolean DeleteClientContactNotes(Int32 clientContactNoteId, Int32 currentLoggedInUserId);
        #endregion

        #region UAT-4657
        void VersioningRequirementPackages(Int32 currentUserId);

        void ProcessRequirementCategoryDisassociation(Int32 requirementCategoryDisassociationTenantMappingId, Int32 currentUserId);

        #endregion

    }
}
