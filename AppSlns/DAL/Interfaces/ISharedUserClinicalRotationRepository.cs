using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ProfileSharing;

namespace DAL.Interfaces
{
    public interface ISharedUserClinicalRotationRepository
    {
        #region Shared User Clinical Rotation and Invitations

        List<ClinicalRotationDetailContract> GetSharedUserClinicalRotations(Int32 currentLoggedInUserId, Int32 tenantId, Guid currentUserID);
        List<Int32> GetSharedUserTenantIDs(Guid userId, Boolean isAgencyUser, Boolean isInstructor_Preceptor);
        List<ClinicalRotationDetailContract> SetProfileInvitationDetailData(List<ClinicalRotationDetailContract> clinicalRotationDetailList, Int32 tenantId, String tenantName, Int32 currentLoggedInUSerID);
        List<AgencyDetailContract> GetSharedUserAgencies(String userID);
        Boolean UpdateInvitationExpirationRequested(List<ApplicantDataListContract> applicantDataList, Int32 currentUserId);
        Boolean UpdateInvitationExpirationRequirementShares(List<Int32> profileSharingInvIDs, Int32 currentUserId); //UAT-3425
        #endregion

        #region Rotation Student Detail for Shared User

        List<ApplicantDataListContract> GetApplicantIDsForRotationAndInvGrp(INTSOF.ServiceDataContracts.Modules.ClinicalRotation.RotationStudentDetailContract rotationStudentDetailContract);

        #endregion

        #region UAT-1344:Automated NPI Number association and agency creation

        List<AgencyDataContract> SaveUpdateAgencyInBulk(String xmlData, Int32 currentLoggedInUserId);
        #endregion

        List<ProfileSharingInvitationSearchContract> GetInvitationExpirationSearchData(ProfileSharingInvitationSearchContract invitationSearchContract, CustomPagingArgsContract customPagingArgsContract);
        Boolean SaveUpdateProfileExpirationCriteria(ProfileSharingInvitationSearchContract invitationSearchContract, List<Int32> lstInvitationIDs, Int32 currentUserId);
       
        // List<AttestationDocumentContract> GetAttestationDocumentsToExport(Int32 rotationID, Int32 currentLoggedInUserID);
       //  List<ProfileSharingInvitationGroup> GetAttestationDocumentForRotation(Int32 rotationID);
        List<ProfileSharingInvitationGroup> GetAttestationDocumentForRotation(List<InvitationIDsDetailContract> lstRotationTenant);
        List<ProfileSharingInvitationGroup> GetAttestationDocumentForInvitationGroup(List<Int32> profileSharingInvitationGroupID);
        List<InvitationDocumentMapping> GetInvitationDocMappingForInvitaitonID(List<Int32> lstProfileSharingInvitationIDs);
        List<ProfileSharingInvitationGroup> GetAttestationDocForProfileSharingInvitaiton(List<Int32> lstProfileSharingInvitationId);

        #region ADB Admin Applicant Data Audit History
        List<ApplicantDataAuditHistoryContract> GetApplicantDataAuditHistory(SearchItemDataContract searchDataContract, CustomPagingArgsContract customPagingArgsContract);

        #endregion

        List<ClinicalRotationDetailContract> GetAttestationReportWithoutSignature(Int32 CurrentLoggedInUserID);

      //  List<AttestationDocumentContract> GetAttestationDocumentsWithoutSignToExport(Int32 InvitationGroupID, Int32 currentLoggedInUserID);

        List<ClinicalRotationDetailContract> GetSharedUserClinicalRotationPackageDetails(ClinicalRotationDetailContract clinicalRotationDetailContract, int currentLoggedInUserId);

        List<INTSOF.UI.Contract.ProfileSharing.InvitationIDsDetailContract> GetProfileSharingInvitationIdByRotationID(Int32 rotationID, Int32 currentLoggedInUserID,Int32 tenantID);//UAT:2475

        #region UAT-2313
        List<ClinicalRotationDetailContract> GetClinicalRotationDataFromFlatTable(ClinicalRotationDetailContract clinicalRotationDetailContract, CustomPagingArgsContract customPagingArgsContract);
        //List<ClientContact> IClientContactRepository.GetAllClientContacts();
        // List<WeekDayContract> GetWeekDays();
         List<ClientContact> GetAllClientContacts();
        #endregion

        #region UAT-3316
        String GetSharedUserTemplatePermissionsCode(Int32 organizationUserID, Boolean isCompliancePermissions);
        #endregion

        List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract> GetSharedUserAgencyHierarchyRootNodes(String userID);
    }
}
