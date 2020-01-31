using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

namespace INTSOF.ServiceInterface.Modules.ClinicalRotation
{
    [ServiceContract]
    public interface IClinicalRotation
    {
        #region Common Methods
        [OperationContract]
        ServiceResponse<List<TenantDetailContract>> GetTenants(ServiceRequest<Boolean, String> data);

        [OperationContract]
        ServiceResponse<List<AgencyDetailContract>> GetAllAgencies(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<List<AgencyDetailContract>> GetAgencies(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<List<AgencyDetailContract>> GetSharedUserAgencies();

        [OperationContract]
        ServiceResponse<List<WeekDayContract>> GetWeekDayList(ServiceRequest<Int32> data);

        /// <summary>
        /// Get the UserData from Security database, by OrganizationUserID.
        /// </summary>
        /// <param name="organisationUserID"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<OrganizationUserContract> GetUserData(ServiceRequest<Int32, Int32> data);


        /// <summary>
        /// Method to get all possible data types of rotation fields
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<RequirementItemStatusContract>> GetRequirementItemStatusTypes(ServiceRequest<Int32> data);

        #endregion

        #region Mange Rotations
        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetClinicalRotationQueueData(ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract> data);
        [OperationContract]
        ServiceResponse<RotationsMappedToAgenciesContract> GetRotationsMappedToAgencies(ServiceRequest<Int32, String> data);
        [OperationContract]
        ServiceResponse<Tuple<Int32, Boolean, Boolean>> SaveUpdateClinicalRotation(ServiceRequest<ClinicalRotationDetailContract, List<CustomAttribteContract>, String> data);

        [OperationContract]
        ServiceResponse<Boolean> DeleteClinicalRotation(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> ArchiveClinicalRotation(ServiceRequest<List<Int32>, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> UnArchiveClinicalRotation(ServiceRequest<List<Int32>, Int32> data); //UAT-3138
        #endregion

        #region Custom Attributes
        [OperationContract]
        ServiceResponse<List<CustomAttribteContract>> GetCustomAttributeListMapping(ServiceRequest<Int32, String, Int32?> data);
        #endregion

        #region Clinical Rotation Details
        [OperationContract]
        ServiceResponse<List<ApplicantDataListContract>> GetApplicantClinicalRotationSearch(ServiceRequest<Int32, ClinicalRotationSearchContract, CustomPagingArgsContract> data);
        [OperationContract]
        ServiceResponse<ClinicalRotationDetailContract> GetClinicalRotationById(ServiceRequest<Int32, Int32?> data);
        [OperationContract]
        ServiceResponse<List<RotationMemberDetailContract>> GetClinicalRotationMembers(ServiceRequest<Int32, Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> RemoveApplicantsFromRotation(ServiceRequest<Dictionary<Int32, Boolean>, List<Int32>> data);
        [OperationContract]
        ServiceResponse<Boolean> AddApplicantsToRotation(ServiceRequest<Int32, Dictionary<Int32, Boolean>> data);
        [OperationContract]
        ServiceResponse<List<UserGroupContract>> GetAllUserGroup(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> AddPackageToRotation(ServiceRequest<Int32, Int32, String> data);
        [OperationContract]
        ServiceResponse<ClinicalRotationRequirementPackageContract> GetRotationRequirementPackage(ServiceRequest<Int32, String> data);
        [OperationContract]
        ServiceResponse<String> GetMaskedSSN(ServiceRequest<String> data);
        [OperationContract]
        ServiceResponse<String> GetFormattedSSN(ServiceRequest<String> data);
        [OperationContract]
        ServiceResponse<String> GetFormattedPhoneNumber(ServiceRequest<String> data);
        [OperationContract]
        ServiceResponse<Boolean> IsClinicalRotationMembersExistForRotation(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetClinicalRotationByIds(ServiceRequest<String> data);
        #endregion

        #region Manage Invitations and Rotations for Shared User
        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetSharedUserClinicalRotations(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetSharedUserClinicalRotationDetails(ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract> data);
        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetSharedUserClinicalRotationPackageDetails(ServiceRequest<ClinicalRotationDetailContract> data);
        [OperationContract]
        ServiceResponse<Boolean> UpdateInvitationExpirationRequested(ServiceRequest<List<ApplicantDataListContract>, Int32?> data);
        //UAT-3425
        [OperationContract]
        ServiceResponse<Boolean> UpdateInvitationExpirationRequirementShares(ServiceRequest<List<Int32>, Int32?> data);

        #endregion

        #region Rotation Student Detail for Shared User
        [OperationContract]
        ServiceResponse<List<ApplicantDataListContract>> GetRotationStudents(ServiceRequest<RotationStudentDetailContract, CustomPagingArgsContract> data);
        [OperationContract]
        ServiceResponse<List<RotationMemberSearchDetailContract>> GetInstrctrPreceptrRotationStudents(ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract> data);
        [OperationContract]
        ServiceResponse<String> GetMaskDOB(ServiceRequest<String> data);
        #endregion

        #region Requirement Verification Queue
        [OperationContract]
        ServiceResponse<List<RequirementPackageTypeContract>> GetRequirementPackageTypes(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<List<RequirementVerificationQueueContract>> GetRequirementVerificationQueueSearch(ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract> data);

        #endregion

        #region Verification Details

        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<RequirementVerificationDetailContract>> GetVerificationDetailData(ServiceRequest<Int32, Int32> data);


        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<RequirementVerificationDetailContract>> GetRequirementItemsByCategoryId(ServiceRequest<Int32, List<Int32>, Int32, Int32> data);


        /// <summary>
        /// Save/Update the data of the Verification Details screen.
        /// </summary>
        /// <param name="dataToSave"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<Dictionary<Int32, String>> SaveVerificationData(ServiceRequest<RequirementVerificationData, Int32> data, ref Boolean isNewPackage);

        #endregion

        #region Rule's Execution.

        [OperationContract]
        ServiceResponse<Boolean> ExecuteRequirementObjectBuisnessRules(ServiceRequest<List<RequirementRuleObject>, Int32> data);

        #endregion

        #region Agency Review Queue

        /// <summary>
        /// Get the Agency Review Queue related data
        /// </summary>
        /// <param name="selectedStatusCodes"></param>
        /// <param name="selectedTenantIds"></param>
        /// <param name="sortingFilteringXML"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<AgencyReviewQueueContract>> GetAgencyQueueData(ServiceRequest<String, String, CustomPagingArgsContract> data);

        /// <summary>
        /// Returns the list of the 'lkpAgencySearchStatus'
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<AgencySearchStatusContract>> GetAgencySearchStatus();

        /// <summary>
        /// Set Agency status to reviwed or available, based on the StatusCode
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<Boolean> SetAgencySearchStatus(ServiceRequest<List<Int32>, String> data);

        #endregion

        #region UAT-1344:Automated NPI Number association and agency creation
        [OperationContract]
        ServiceResponse<List<AgencyDataContract>> SaveUpdateAgencyInBulk(ServiceRequest<String> data);
        #endregion

        [OperationContract]
        ServiceResponse<Boolean> CreateRotationSubscriptionForClientContact(ServiceRequest<List<Int32>, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> IfAnyContactHasEnteredDataForRotation(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> IfAnyContactIsMappedToRotation(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> UpdateRotationSubscriptionForClientContact(ServiceRequest<Int32, Int32, Int32> data);

        #region UAT-1362:As an Instructor/Preceptor I should be able to enter data for my rotation requirements package
        [OperationContract]
        ServiceResponse<Int32> GetRequirementSubscriptionIdByClinicalRotID(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<List<Int32>> GetSharedUserTenantIDs(ServiceRequest<List<String>> data);
        #endregion

        #region UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range
        [OperationContract]
        ServiceResponse<List<ApplicantDocumentContract>> GetApplicantDocumentToExport(ServiceRequest<List<RotationMemberSearchDetailContract>> data);

        [OperationContract]
        ServiceResponse<List<RotationMemberSearchDetailContract>> GetRotationMemberSearchData(ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract> data);

        [OperationContract]
        ServiceResponse<Int32> GetSubscriptionIdByRotIDAndUserID(ServiceRequest<Int32, Int32, String> data);

        #endregion

        #region UAT 1409 : The Agency User should be able to filter their search by those rotations that are active or completed (expired) AND by pending revieew or reviewed.
        [OperationContract]
        ServiceResponse<List<SharedUserRotationReviewStatusContract>> GetRotationReviewStatus(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateRotationReviewStatus(ServiceRequest<List<SharedUserRotationReviewContract>, Int32> data);
        #endregion

        [OperationContract]
        ServiceResponse<List<ProfileSharingInvitationSearchContract>> GetInvitationExpirationSearchData(ServiceRequest<ProfileSharingInvitationSearchContract, CustomPagingArgsContract> data);
        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateProfileExpirationCriteria(ServiceRequest<ProfileSharingInvitationSearchContract, List<Int32>> data);
        [OperationContract]
        ServiceResponse<List<AttestationDocumentContract>> GetAttestationDocumentsToExport(ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> ServiceReqData);

        [OperationContract]
        ServiceResponse<Dictionary<Int32, String>> GetDefaultPermissionForClientAdmin(ServiceRequest<Int32> data);

        /// <summary>
        /// UAT-1784: Get Granular Permissions for current user
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<Dictionary<String, String>> GetGranularPermissions();

        /// <summary>
        /// UAT-1701, New Agency User Search.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetStudentRotationSearchDetails(ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract, Dictionary<Int32, string>> data);

        [OperationContract]
        ServiceResponse<bool> GetRequirementPackageStatusByRotationID(ServiceRequest<int, int, String> data);

        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
        /// <summary>
        /// get Institution and user mapped agencies
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        ServiceResponse<List<AgencyDetailContract>> GetInstitutionMappedAgency(ServiceRequest<List<Int32>, String> data);
        #endregion

        [OperationContract]
        ServiceResponse<List<AgencyDetailContract>> GetAgenciesFromAllTenants();

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        [OperationContract]
        ServiceResponse<Boolean> UpdateRotAndInvitationReviewStatus(ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> data);

        [OperationContract]
        ServiceResponse<List<SharedUserRotationReviewStatusContract>> GetReviewStatusList(ServiceRequest<Int32> data);
        #endregion


        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetAttestationReportsWithoutSignature(ServiceRequest<Int32> data);

        // [OperationContract]
        //ServiceResponse<List<AttestationDocumentContract>> GetAttestationDocumentsWithoutSignToExport(ServiceRequest<Int32> InvitationGroupID);

        #region UAT-1881
        [OperationContract]
        ServiceResponse<List<AgencyDetailContract>> GetAllAgencyByOrgUser(ServiceRequest<Int32> data);
        #endregion

        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateClinicalRotationAssignments(ServiceRequest<List<Int32>, ClinicalRotationDetailContract, Dictionary<String, String>> data);
        [OperationContract]
        ServiceResponse<Boolean> IsDataEnteredForAnyRotation(ServiceRequest<String, String> data);
        [OperationContract]
        ServiceResponse<Boolean> IsPreceptorAssignedForAllRotations(ServiceRequest<String> data);
        #endregion
        [OperationContract]
        ServiceResponse<List<InstructorAvailabilityContract>> CheckInstAvailabilityByRotationIds(ServiceRequest<String> data);

        #region UAT-2071, Configuration Rotation and Tracking packages must be fully compliant to share
        [OperationContract]
        ServiceResponse<List<RotationAndTrackingPkgStatusContract>> GetComplianceStatusOfImmunizationAndRotationPackages(ServiceRequest<int, Dictionary<string, string>, int> data);

        [OperationContract]
        ServiceResponse<Dictionary<Int32, String>> IsRequirementPkgCompliantReqd(ServiceRequest<String> data);
        #endregion

        [OperationContract]
        ServiceResponse<String> GetSharingInfoByInvitationGrpID(ServiceRequest<Int32, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Tuple<List<Int32>,String, Dictionary<Boolean, String>>> RotationSubscriptionApproveAllPendingItems(ServiceRequest<Int32, Int32,Boolean> data, ref Int32 affectedItemsCount);

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        [OperationContract]
        ServiceResponse<Dictionary<Int32, Boolean>> GetComplianceRequiredRotCatForPackage(ServiceRequest<Int32> data);
        #endregion
        #region UAT-3458
        [OperationContract]
        ServiceResponse<List<RequirementExpiringItemListContract>> GetRequirementItemsAboutToExpire(ServiceRequest<Int32, Int32> data);
        #endregion

        #region UAT-2371
        [OperationContract]
        ServiceResponse<SystemEntityUserPermission> GetSystemEntityUserPermission(ServiceRequest<Int32, Int32> data);
        #endregion

        #region UAT-2514
        [OperationContract]
        ServiceResponse<Dictionary<Boolean, DateTime>> IsRotationEndDateRangeNeedToManage(ServiceRequest<Int32, Int32> data);
        #endregion

        #region UAT-2424
        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetAllClinicalRotations(ServiceRequest<Int32> data);

        //[OperationContract]
        //ServiceResponse<Int32> GetRotationPackageIDByRotationId(ServiceRequest<Int32, String> data);

        //UAT-3121
        [OperationContract]
        ServiceResponse<RequirementPackageContract> GetRotationPackageIDByRotationId(ServiceRequest<Int32, String> data);


        [OperationContract]
        ServiceResponse<ClinicalRotationDetailContract> GetClinicalRotationDetailsById(ServiceRequest<Int32> data);
        #endregion

        #region UAT-2554

        [OperationContract]
        ServiceResponse<Boolean> IsPreceptorRequiredForAgency(ServiceRequest<Int32> data);

        #endregion

        #region UAT-2313

        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetClinicalRotationDataFromFlatTable(ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract> data);
        [OperationContract]
        ServiceResponse<List<WeekDayContract>> GetWeekDays();
        [OperationContract]
        ServiceResponse<List<ClientContactContract>> GetAllClientContacts();
        #endregion

        #region UAT-2666
        [OperationContract]
        ServiceResponse<Boolean> UpdateClinicalRotationByAgency(ServiceRequest<ClinicalRotationDetailContract, Int32, Boolean, Int32?> data);
        [OperationContract]
        ServiceResponse<List<RotationFieldUpdatedByAgencyContract>> GetRotationFieldUpdateByAgencyDetails(ServiceRequest<List<Int32>, Int32> data);
        #endregion

        #region UAt-2510

        [OperationContract]
        ServiceResponse<Boolean> GetAgencyUserSSN_Permission(ServiceRequest<String> data);
        #endregion


        #region UAT-2513
        [OperationContract]
        ServiceResponse<Boolean> SaveBatchRotationUploadDetails(ServiceRequest<List<BatchRotationUploadContract>, String, Int32, Int32> data);

        [OperationContract]
        ServiceResponse<List<BatchRotationUploadContract>> GetBatchRotationList(ServiceRequest<Int32, BatchRotationUploadContract, CustomPagingArgsContract> data);
        #endregion

        #region UAT-2926
        [OperationContract]
        ServiceResponse<List<String>> GetAgencyHierarchyAgencyList(ServiceRequest<List<Tuple<Int32, Int32>>> data);
        #endregion

        [OperationContract]
        ServiceResponse<List<String>> FilterApplicantHavingOnlyNonActiveOrExpireOrders(ServiceRequest<int, string> data);

        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetRequirementPackage(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<List<RequirementCategoryContract>> GetRequirementCategory(ServiceRequest<Int32, List<Int32>> data);

        [OperationContract]
        ServiceResponse<List<RequirementItemContract>> GetRequirementItem(ServiceRequest<Int32, List<Int32>> data);

        [OperationContract]
        ServiceResponse<List<ApplicantRequirementDataAuditContract>> GetApplicantRequirementDataAudit(ServiceRequest<Int32, ApplicantRequirementDataAuditSearchContract, CustomPagingArgsContract> data);

        #region Requirement Verification Assignment Queue AND User Work Queue
        [OperationContract]
        ServiceResponse<List<RequirementVerificationQueueContract>> GetAssignmentRotationVerificationQueueData(ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract> data);

        [OperationContract]
        ServiceResponse<List<ReqPkgSubscriptionIDList>> GetReqPkgSubscriptionIdList(ServiceRequest<RequirementVerificationQueueContract, Int32, Int32> data);

        [OperationContract]
        ServiceResponse<List<ReqPkgSubscriptionIDList>> GetReqPkgSubscriptionIdListForRotationVerification(ServiceRequest<RequirementVerificationQueueContract, Int32, Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> AssignItemsToUser(ServiceRequest<List<Int32>, Int32, String> data);

        [OperationContract]
        ServiceResponse<List<RequirementPackageTypeContract>> GetSharedRequirementPackageTypes();
        #endregion

        [OperationContract]
        ServiceResponse<System.Data.DataTable> PerformRotationLiveDataMovement(ServiceRequest<Int32, Int32, Int32> data);
        [OperationContract]
        ServiceResponse<System.Data.DataTable> GetTargetReqPackageSubscriptionIDsForSync(ServiceRequest<Int32, Int32> data);

        #region UAT-3197, As an Agency User, I should be able to retrieve the syllabus
        [OperationContract]
        ServiceResponse<List<ClientContactSyllabusDocumentContract>> GetClinicalRotationSyllabusDocumentsByID(ServiceRequest<Int32, Int32> serviceRequest);

        #endregion

        #region UAT-3176
        [OperationContract]
        ServiceResponse<List<RequirementAttributeGroupContract>> GetAllRotationAttributeGroup(ServiceRequest<Int32, String, String> data);


        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateRotationAttributeGroup(ServiceRequest<RequirementAttributeGroupContract, Int32, Boolean> data);

        [OperationContract]
        ServiceResponse<RequirementAttributeGroupContract> GetAttributeGroupById(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> IsAttributeGroupMapped(ServiceRequest<Int32, Int32> data);
        #endregion

        [OperationContract]
        ServiceResponse<List<ApplicantDataListContract>> GetRotationMembersForRotationDocs(ServiceRequest<ClinicalRotationSearchContract, CustomPagingArgsContract> data);

        [OperationContract]
        ServiceResponse<List<RequirementCategoryContract>> GetReqPkgCatByRotationID(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<List<RotationDocumentContact>> GetApplicantDocsByReqCatID(ServiceRequest<String, String, CustomPagingArgsContract> data);

        [OperationContract]
        ServiceResponse<List<RotationDocumentContact>> GetApplicantDocumentsByDocIDs(ServiceRequest<String, String> data);

        #region UAt-3220
        [OperationContract]
        ServiceResponse<Boolean> HideRequirementSharesDetailLink(ServiceRequest<Guid> data);
        #endregion

        #region UAT-3241
        [OperationContract]
        ServiceResponse<List<String>> GetAgencyNamesByIds(ServiceRequest<Int32, List<Int32>> data);
        #endregion

        [OperationContract]
        ServiceResponse<ProfileSharingInvitationDetailsContract> GetProfileShareDetailsById(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> UpdateProfileShareInvDetails(ServiceRequest<ProfileSharingInvitationDetailsContract, Int32> data);

        //UAT-3315
        [OperationContract]
        ServiceResponse<List<ApplicantDocumentContract>> GetSelectedBadgeDocumentsToExport(ServiceRequest<String, Int32, Int32> data);
        #region UAT-3316
        [OperationContract]
        ServiceResponse<String> GetSharedUserTemplatePermissions(ServiceRequest<Int32, Boolean> data);
        #endregion

        #region UAT-3470
        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateInvitationArchiveState(ServiceRequest<List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> data);
        #endregion

        [OperationContract]
        ServiceResponse<List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract>> GetSharedUserAgencyHierarchyRootNodes();

        #region UAT-3977
        [OperationContract]
        ServiceResponse<Dictionary<Int32, String>> InstructorPreceptorRequiredPkgCompliantReqd(ServiceRequest<String> data);
        #endregion

        #region UAT-3957
        [OperationContract]
        ServiceResponse<List<RequirementItemRejectionContract>> GetRequirementRejectedItemDetailsForMail(ServiceRequest<String, Int32> data);

        #endregion

        #region UAT-4334
        [OperationContract]
        ServiceResponse<List<ClinicalRotationDetailContract>> GetAllClinicalRotationsForLoggedInUser(ServiceRequest<Int32, Int32, Boolean> data);
        #endregion
        [OperationContract]
        ServiceResponse<Boolean> IsAgenycHierarchyAvailable(ServiceRequest<String> data);
    }
}
