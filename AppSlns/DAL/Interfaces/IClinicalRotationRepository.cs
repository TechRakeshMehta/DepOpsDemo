using System;
using System.Collections.Generic;
using System.Data;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace DAL.Interfaces
{
    public interface IClinicalRotationRepository
    {
        #region Clinical Rotation Details

        List<ApplicantDataListContract> GetApplicantClinicalRotationSearch(Int32 clinicalRotationId, ClinicalRotationSearchContract searchDataContract, CustomPagingArgsContract gridCustomPaging);
        ClinicalRotationDetailContract GetClinicalRotationById(Int32 clinicalRotationId, Int32? agencyID);
        List<RotationMemberDetailContract> GetClinicalRotationMembers(Int32 clinicalRotationId, Int32 agencyID, Int32 currentLoggedInUserId);
        Boolean AddApplicantsToRotation(Int32 clinicalRotationID, Int32 requirementNotCompliantPackStatusID, Int32 rotationSubscriptionTypeID, Dictionary<Int32, Boolean> organizationUserIds, Int32 currentLoggedInUserId, Int32 reqPkgTypeId, Int16 statusId);
        RotationStudentDropped RemoveApplicantsFromRotation(Dictionary<Int32, Boolean> clinicalRotationMemberIDs, Int32 currentLoggedInUserId, Int32 reqPkgTypeId, List<Int32> approvedMemberIdsToRemove, Int32 tenantId, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId);
        Boolean AddPackageToRotation(Int32 clinicalRotationID, Int32 requirementPackageID, Int32 currentLoggedInUserId, Int32 reqPkgTypeId);
        ClinicalRotationRequirementPackage GetRotationRequirementPackageByRotationId(Int32 clinicalRotationID, Int32 reqPkgTypeId);
        ClinicalRotationMember GetClinicalRotationMemberByRotationId(Int32 clinicalRotationID);
        ClinicalRotationRequirementPackageContract GetRotationRequirementPackage(Int32 clinicalRotationID, String pkgTypeCode);

        //UAT-2040
        List<ClinicalRotationDetailContract> GetClinicalRotationByIds(String clinicalRotationId);
        List<Int32> SetRotationsToArchive(Int32 chunkSize, Int32 systemUserId); //UAT-3139

        #region UAT-1843: Phase 2 5: Combining User group mapping, archive and rotation assignment screens
        List<ClinicalRotationDetailContract> GetClinicalRotationMappingData(CustomPagingArgsContract customPagingArgsContract, String applicantUserIds, Int32? currentUserId);
        Boolean AssignRotationsToUsers(List<Int32> rotationIds, List<Int32> applicantUserIds, Int32 currentUserId, Int32 requirementNotCompliantPackStatusID, Int32 rotationSubscriptionTypeID, Int32 reqPkgTypeId, Int16 statusId, out List<Tuple<Int32, Int32>> applicantList, out Boolean IsApplicantTakeSpecialPackage);
        List<RotationStudentDropped> UnassignRotations(List<Int32> rotationIds, List<Int32> applicantUserIds, Int32 currentUserId, Int32 reqPkgTypeId, Int32 tenantId, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId);
        #endregion

        #endregion

        #region Manage Rotations

        List<ClinicalRotationDetailContract> GetClinicalRotationQueueData(ClinicalRotationDetailContract clinicalRotationDetailContract, CustomPagingArgsContract customPagingArgsContract);

        RotationsMappedToAgenciesContract GetRotationsMappedToAgencies(String rotationIDs);

        //UAT:4395
        Dictionary<Int32, Boolean> GetExistingorganizationUserIds(Int32 ClincalRotationId, Int32? currentLogggedUserId);
        
        /// <summary>
        /// Method to Save clinical rotation
        /// </summary>
        /// <returns></returns>
        Int32 SaveClinicalRotation(ClinicalRotationDetailContract clinicalRotationDetailContract, List<CustomAttribteContract> customAttributeListToSave, Int32 currentUserId, Int32 syllabusDocumentTypeID, Int32 profileSynchSourceTypeID, int additionalDocumentTypeId);

        /// <summary>
        /// Method to Update clinical rotation
        /// </summary>
        /// <returns></returns>
        RotationDetailFieldChanges UpdateClinicalRotation(ClinicalRotationDetailContract clinicalRotationDetailContract, List<CustomAttribteContract> customAttributeListToSave, Int32 currentUserId, Int32 syllabusDocumentTypeID, Int32 profileSynchSourceTypeID
                                                                                                , Int32 reqPkgTypeId, Int32 rotationSubscriptionTypeID, Int32 requirementNotCompliantPackStatusID, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId, int additionalDocumentTypeId);

        /// <summary>
        /// Method to delete clinical rotation
        /// </summary>
        /// <returns></returns>
        Boolean DeleteClinicalRotation(Int32 clinicalRotationId, Int32 currentUserId);


        Boolean ArchiveClinicalRotation(List<Int32> clinicalRotationIds, Int32 currentUserId);//UAT-2545
        Boolean UnArchiveClinicalRotation(List<Int32> clinicalRotationIds, Int32 currentUserId);//UAT-3138

        /// <summary>
        /// Get the ApplicantId's in the current Rotation
        /// </summary>
        /// <param name="rotationId"></param>
        /// <returns></returns>
        List<Int32> GetRotationApplicantIds(Int32 rotationId);

        /// <summary>
        /// Method to Get Instructor/Preceptor Data, including the backgrond and compliance shared info type codes.
        /// </summary>
        /// <param name="rotationId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        List<ClientContactProfileSharingData> GetRotationClientContacts(Int32 rotationId, Int32 tenantId);

        #endregion

        #region Custom attribute
        List<CustomAttribteContract> GetCustomAttributeMappingList(String useTypeCode, Int32? rotationID);
        string GetClinicalRotationNotificationCustomAttributes(Int32 clinicalRotationId);
        #endregion

        #region UAT 1304 : Instructor/Preceptor screens and functionality
        /// <summary>
        /// Returns the list of ClientSystemDocuments for all the rotations attached to Client Contact.
        /// </summary>
        /// <param name="clientContactID"></param>
        /// <param name="tenantId"></param>
        /// <returns>list of ClientSystemDocuments</returns>
        List<ClientContactSyllabusDocumentContract> GetClientContactRotationDocuments(Int32 clientContactID);
        #endregion

        #region UAT 1302 As an admin (client or ADB), I should be able to create preceptors/instructors
        Boolean IsClientRotationClientContactMappingExist(Int32 clientContactID);
        #endregion

        #region Manage Invitations and Rotations for Shared User

        List<ClinicalRotationDetailContract> GetClinicalRotationsByIDs(Int32 currentLoggedInUserId, String clinicalRotationXML, ClinicalRotationDetailContract clinicalRotationDetailContract, CustomPagingArgsContract customPagingArgsContract);

        #endregion

        #region Rotation Student Detail for Shared User

        List<ApplicantDataListContract> GetRotationStudentsDetail(CustomPagingArgsContract customPagingArgsContract, String applicantUserIDsXML, RotationStudentDetailContract rotationStudentDetailContract);
        List<ApplicantDataListContract> GetInstructorRotationStudentsDetail(CustomPagingArgsContract customPagingArgsContract, RotationStudentDetailContract rotationStudentDetailContract);

        //UAT-4013
        //List<RotationMemberSearchDetailContract> GetInstrctrPreceptrRotationStudents(String tenantID, Guid userID, RotationMemberSearchDetailContract searchContract, CustomPagingArgsContract customPagingArgsContract);

        #endregion

        /// <summary>
        /// Get the UserDetails by OrganizationUserId
        /// </summary>
        /// <param name="orgUserId"></param>
        /// <returns></returns>
        usp_GetUserDetails_Result GetUserData(Int32 orgUserId);

        #region UAT-1361
        List<RequirementPackageSubscription> CreateRotationSubscriptionForClientContact(List<Int32> clientContactIds, Int32 clinicalRotationID, Int32 reqPkgTypeId, Int32 rotationSubscriptionTypeID,
                                                                                    Int32 requirementNotCompliantPackStatusID, Int32 currentLoggedInUserId);


        void UpdateRotationSubscriptionForClientContact(Int32 clinicalRotationID, Int32 currentLoggedInUserId, Int32 oldPkgId, Int32 newPkgId, Int32 rotationSubscriptionTypeID, Int32 requirementNotCompliantPackStatusID, Int16 dataMovementDueStatusId);

        void CreateOptionalCategorySetAproved(List<RequirementPackageSubscription> lstRequirementPackageSubscriptionToBeAdded, Int32 currentUserId);

        Boolean SaveContextIntoDataBase();

        Boolean IfAnyContactIsMappedToRotation(Int32 rotationId, Int32 tenantId);

        Boolean IfAnyContactHasEnteredDataForRotation(Int32 packageId, Int32 clinicalRotationID);

        List<Entity.SharedDataEntity.ClinicalRotationClientContactMapping> lstRotationMappedToContacts(Int32 contactId, Int32 tenantId);

        List<ClientContactContract> SynchronizeClientContactProfiles(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentUserId, Int32 profileSynchSourceTypeID, Int32 tenantID);
        #endregion

        #region UAT-1362:As an Instructor/Preceptor I should be able to enter data for my rotation requirements package

        Int32 GetRequirementSubscriptionIdByClinicalRotID(Int32 clinicalRotationID, Int32 rotReqSubTypeID, Int32 inscPrecptorPkgID, Int32 orgUserID);
        #endregion

        /// <summary>
        /// Check whether the selected Agency is associated with any clinical rotation in any Tenant 
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        Boolean IsAgencyAssociated(Int32 agencyId);

        #region UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range
        List<ApplicantDocumentContract> GetApplicantDocumentToExport(String appRotationXMl);

        List<RotationMemberSearchDetailContract> GetRotationMemberSearchData(RotationMemberSearchDetailContract clinicalRotationDetailContract
                                                                                , CustomPagingArgsContract customPagingArgsContract);

        Int32 GetSubscriptionIdByRotIDAndUserID(Int32 clinicalRotationID, Int32 reSubscriptionPkgTypeId, Int32 rotReqSubTypeID, Int32 orgUserID);

        #endregion

        IEnumerable<ClinicalRotationMember> GetRotationMemberListByRotationId(Int32 clinicalRotationID);

        List<RequirementPackageSubscription> AddRequirementPackageSubscription(Int32 clinicalRotationID, Int32 requirementNotCompliantPackStatusID, Int32 rotationSubscriptionTypeID, Dictionary<Int32, Boolean> organizationUserIds, Int32 currentLoggedInUserId, Int32 reqPkgTypeId);

        #region UAT 1409 : The Agency User should be able to filter their search by those rotations that are active or completed (expired) AND by pending revieew or reviewed.
        Boolean SaveUpdateRotationReviewStatus(List<SharedUserRotationReview> lstSharedUserRotationReview, Boolean isUpdateMode);

        List<SharedUserRotationReview> GetShareduserRotationReviewStatusByIds(List<Int32> lstSaredUserRotationReviewID);
        #endregion


        #region UAT-1414:Notification to go out prior to student's start date for clinical rotation.
        List<ClinicalRotationMemberDetail> GetRotationMemberDetailForNagMail(Int32 subEventId, Int32 chunkSize);
        #endregion

        #region UAT-1701, New Agency User Search
        List<ClinicalRotationDetailContract> GetClinicalRotationsWithStudentByIDs(int currentLoggedInUserId, string clinicalRotationXML, ClinicalRotationDetailContract clinicalRotationDetailContract, String customAttributeXML);
        #endregion

        Boolean GetRequirementPackageStatusByRotationID(int RotationID, String applicantIds);

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        Boolean SaveUpdateUserRotationReviewStatus(int RotationID, Int32 currentLoggedInUserId, Int32 inviteeOrgUserId, Int32 reviewStatusId, Int32 agencyID, Int32? lastReviewedByID, Boolean isRotationReviewStatusUpdatingWhileRS, Int32 approvedReviewStatusID, Boolean isAdminLoggedInAsAgencyUser = false);
        #endregion

        #region UAT-1843
        List<ClinicalRotationMemberDetail> GetRotationDetailsByOrgUserIds(String orgUserIds, Int32? clinicalRotationID = null);
        Boolean UpdateClinicalRotationMenberForNagMail(Int32 RotationMemberId, Int32 CurentLoggedInUserId);
        #endregion

        #region UAT-2034:
        Tuple<Boolean, List<ClientContactContract>, List<RotationDetailFieldChanges>> SaveUpdateClinicalRotationAssignments(List<Int32> clinicalRotIDs, ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentUserId
                                                      , Int32 profileSynchSourceTypeID, String rotationAssignType, Int32 syllabusDocumentTypeID, Int32 packageID
                                                      , Int32 rotationSubscriptionTypeID, Int32 requirementNotCompliantPackStatusID, Int16 dataMovementDueStatusId);
        Boolean IsDataEnteredForAnyRotation(Int32 tenantId, String rotationIds, String packageType);

        Boolean IsPreceptorAssignedForAllRotations(List<Int32> rotationIds);
        List<InstructorAvailabilityContract> CheckInstAvailabilityByRotationIds(String rotationIds);

        #endregion

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        Dictionary<Int32, Boolean> GetComplianceRequiredRotCatForPackage(Int32 reqPackageId);
        #endregion

        #region UAT -2477
        List<ClinicalRotationDetailContract> GetRotationPackageAndAgencyData(Int32 rotationID, Int32 tenantID);
        #endregion

        #region UAT-2514
        Dictionary<Boolean, DateTime> IsRotationEndDateRangeNeedToManage(Int32 clinicalRotationID);
        #endregion

        List<ClinicalRotationMember> GetDroppedRotationMembersByRotationID(Int32 clinicalRotationID);

        #region UAT-2424
        List<ClinicalRotation> GetAllClinicalRotations();
        ClinicalRotationRequirementPackage GetRotationPackageByRotationId(Int32 clinicalRotationID, String reqPkgTypeCode);
        ClinicalRotationDetailContract GetClinicalRotationDetailsById(Int32 clinicalRotationId, int additionalDocumentTypeId);

        List<MultipleAdditionalDocumentsContract> GetAdditionalDocumnetDetails(Int32 clinicalRotationId, int additionalDocumentTypeId);
        #endregion

        #region UAT-4334
        List<ClinicalRotationDetailContract> GetAllClinicalRotationsForLoggedInUser(Int32 currentUserId, bool isAdminLoggedIn);
        #endregion

        #region UAT-2544:
        Boolean IsApplicantDroppedFromRotation(Int32 clinicalRotationId, Int32 RPS_ID, Int32 currentLoggedInUserId);
        Boolean NeedToChangeInvitationStatusAsPending(Int32 clinicalRotationId, List<Int32> invitationIDs, Int32 studentid, Int32 currentLoggedInUserId);
        void DropRotaionIfRequired(List<Int32> clinicalRotationIds, Int32 currentLoggedInUserId);

        #endregion

        #region UAT-2554
        Boolean IsPreceptorRequiredForAgency(Int32 agencyID, Int32 agencyPrmsnTypeID);
        #endregion

        #region UAT-2313

        void SyncRotationAgencyAndClientContacts(String rotationIds, Int32 tenantId, Int32 organizationUserId);

        #endregion

        #region UAT-2712
        List<AgencyHierarchyRotationFieldOptionContract> GetAgencyHierarchyRotationFieldOptionSetting(String hierarchyID);
        #endregion
        #region UAT-2666
        RotationDetailFieldChanges UpdateClinicalRotationByAgency(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentLoggedInUserId, Boolean IsSharedUser, Int32 tenantId);
        List<RotationFieldUpdatedByAgency> GetRotationFieldUpdateByAgencyDetails(List<Int32> lstClinicalRotationIds);
        #endregion

        #region UAT-2603

        Boolean AddDataToRotDataMovement(List<Int32> reqPkgSubscriptions, Int32 currentUserId, Int16 statusId);

        Boolean UpdateDataMovementStatus(List<Int32> lstReqPkgSubsIds, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId, Int32 currentUserId);

        List<Int32> GetRPSIdsWithDataMovementDueStatus(Int32 chunkSize, Int32 statusId);

        DataTable PerformRotationDataMovement(String lstRPSIdsWithDueStatus, Int32 currentUserId);
        #endregion

        #region [UAT-2668]

        List<ClinicalRotationAgencyContract> GetAgenciesMappedWithRotation(Int32 clinicalRotationID);

        #endregion

        #region UAT-2513
        Boolean SaveBatchRotationUploadDetails(List<BatchRotationUploadContract> clinicalRotationDetailContractList, String fileName, Int32 currentLoggedInID);
        List<BatchRotationUploadContract> GetBatchRotationList(BatchRotationUploadContract searchContract, CustomPagingArgsContract gridCustomPaging);

        List<Int32> GetBatchRotationListForTimer(Int32 statusID);

        List<ClinicalRotationDetailContract> CreateClinicalRotationFromBatchRotationUploadDetails(List<Int32> batchRotationUploadDetailIds, Int32 currentLoggedInID);
        #endregion

        #region [UAT-2679]

        List<RequirementPackageContract> GetRequirementPackage(Int32 packageTypeId);

        List<RequirementCategoryContract> GetRequirementCategory(List<Int32> requirementpackageID);

        List<RequirementItemContract> GetRequirementItem(List<Int32> requirementcategoryID);

        List<ApplicantRequirementDataAuditContract> GetApplicantRequirementDataAudit(ApplicantRequirementDataAuditSearchContract searchContract, CustomPagingArgsContract customPagingArgsContract);

        #endregion

        #region UAT-2907
        List<Entity.OrganizationUser> GetOrganizationUserFromIds(List<Int32> orgUserIds);
        #endregion

        #region [UAT-3045]
        ProfileSharingExpiryContract GetNonComplianceCategoryList(Int32 clinicalRotationID, String delimittedOrgUserIDs, String selectedCategoriesXml);
        #endregion

        DataTable PerformRotationLiveDataMovement(Int32 requirementSubscriptionID, Int32 requirementCategoryID, Int32 currentUserID);  //UAT 3164
        DataTable GetTargetReqPackageSubscriptionIDsForSync(Int32 requirementSubscriptionID, Int32 requirementCategoryID); //UAT 3164

        List<ClientContactSyllabusDocumentContract> GetClinicalRotationSyllabusDocumentsByID(Int32 clinicalRotationID,Int32 additionalDocumentTypeId); //UAT-3197, As an Agency User, I should be able to retrieve the syllabus

        List<ApplicantDataListContract> GetRotationMembersForRotationDocs(ClinicalRotationSearchContract searchDataContract, CustomPagingArgsContract gridCustomPaging);

        List<RequirementCategoryContract> GetReqPkgCatByRotationID(Int32 clinicalRotationID);

        List<RotationDocumentContact> GetApplicantDocsByReqCatID(string reqCatIDs, string applicantIds, CustomPagingArgsContract gridCustomPaging);

        List<RotationDocumentContact> GetApplicantDocumentsByDocIDs(string applicantDocIds, string reqCatIds);

        List<ApplicantDocumentContract> GetSelectedBadgeDocumentsToExport(String studentIds, Int32 loggedInUserIds); //UAT-3315

        Int32 GetRotationCreatorByRotationID(Int32 rotationID);  //UAT 3364

        #region UAT-3458
        List<RequirementExpiringItemListContract> GetRequirementItemsAboutToExpire(Int32 requirementPackageSubscriptionId);
        #endregion


        //UAT-3485
        List<INTSOF.UI.Contract.RotationPackages.RequirementItemsAboutToExpireContract> GetExpiringRequirementItems(Int32 subEventId, Int32 chunkSize);

        //UAT-3137
        List<INTSOF.UI.Contract.RotationPackages.RequirementCategoriesBeforeGoingToBeRequiredContract> GetRequirementCategoriesBeforeGoingToBeRequired(Int32 subEventId, Int32 chunkSize);

        void AutomaticallyArchiveRotation();

        #region UAT-4147
        List<ClinicalRotationMembersContract> IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(String rotationIDs, Int32 tenantID, String selectedOrgUserIDs, String selectedClientContactIDs);
        #endregion

        #region UAT-4323
        List<ClinicalRotationDetailContract> GetApplicantDetailsForSelectedRotations(String rotationIDs, Int32 tenantID);

        #endregion
        Boolean IsAgenycHierarchyAvailable(String parameter);

        #region UAT-4428

        List<ClinicalRotationRequirementPackage> GetReqPackagesByRotId(Int32 rotationId);
        #endregion
    }
}
