using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.UI.Contract.ComplianceManagement;

namespace DAL.Interfaces
{
    public interface IProfileSharingRepository
    {
        #region Manage Agency
        List<Agency> GetAgencies(Int32 TenantID, Boolean IsAdmin, Boolean isAgencyUser, Guid userID, Boolean getNotVerfiedAgenciesAlso = false);

        List<ManageAgencyContract> GetAgencyDetail(Int32 TenantID, String agencyIDs);

        Tuple<Int32, Dictionary<Int32, Int32>, Int32> SaveAgencies(AgencyContract agencyData, List<Int32> lstTenantID);

        Tuple<String, Dictionary<Int32, Int32>> UpdateAgencies(AgencyContract agencyData, List<Int32> tenantIDs_Added, List<Int32> tenantIDs_Removed);

        List<AgencyInstitution> GetAgencyInstitutionForAgencies(IEnumerable<Int32> lstAgencyId);

        String DeleteAgency(AgencyContract agencyData);

        List<Int32> GetAgencyHierarchyIDsByAgencyID(Int32 agencyID);

        List<AgencyInstitution> GetAgencyInstitutionForAgencyuser(Int32 agencyUserID);
        #endregion

        #region Get ApplicantInvitationMetaData
        List<ApplicantInvitationMetaData> GetApplicantInvitationMetaData();
        #endregion

        #region Manage AgencyUser

        List<AgencyUserContract> GetAgencyUserInfo(Boolean IsAdmin, Boolean IsAgencyUser, Guid UserID, Int32 TenantID,CustomPagingArgsContract grdCustomPaging, Boolean GetNotVerfiedAgenciesAlso);

        List<AgencyUser> GetAgencyUser(Int32 tenantID, Boolean IsAdmin);

        List<AgencyUser> GetAgencyUserForSharedUser(Guid userID);

        AgencyUser GetAgencyUserByUserID(String userID);

        Int32 SaveAgencyUser(AgencyUserContract _agencyUser, Int32 loggedInUserID, List<AgencyUserPermission> lstAgencyUserPermission);

        String DeleteAgencyUser(Int32 tenantID, Int32 AUG_ID, Int32 LoggedInUserId, List<Int32> AgencyInstitutionId, Boolean IsAdmin);

        //String UpdateAgencyUser(AgencyUserContract _agencyUser, Int32 LoggedInUserId, Boolean IsAdmin, List<Int32> agencyInstitutionIDs_Added, List<Int32> agencyInstitutionIDs_Removed, List<AgencyUserPermission> lstAgencyUserPermission);
        AgencyUser UpdateAgencyUser(AgencyUserContract _agencyUser, Int32 LoggedInUserId, Boolean IsAdmin, List<AgencyUserPermission> lstAgencyUserPermission);

        List<Int32> GetAgencyUserSharedDataForAgencyUserID(Int32 agencyID);

        List<AgencyInstitution> GetAgencyUserInstitutesForAgencyUserID(Int32 agencyUserID);

        List<InvitationSharedInfoMapping> GetInvitationSharedInfoTypeByAgencyUserID(Int32 agencyUserID);

        AgencyUser IsEmailAlreadyExistAgencyUser(String email);

        AgencyUserContract GetAgencyUserDetails(Guid userID);

        Boolean UpdateAgencyUserDetails(AgencyUserContract agencyUserDetails, Guid userID, Int32 currentUserID);
        ClientContact IsInstructorPreceptorUser(String email); //UAT-3360

        #endregion

        #region Check For Tenants Need To Be Disable
        List<int> CheckTenantsNeedToDisable(int AgencyID);
        #endregion

        #region Profile Sharing Methods Copied from Security Repo

        #region Profile Sharing

        List<GetAttestationDocumentUserInfo_Result> AttestationDocumentUserInfo(Int32 orgUserID, String documentType);

        #region UAT-1237 Add Agency/shared users to client user search
        DataTable GetSharedUserSearchData(INTSOF.UI.Contract.SearchUI.SharedUserSearchContract sharedUserSearchContract, CustomPagingArgsContract customPagingArgsContract);
        List<GetSharedUserInvitationDetails_Result> GetSharedUserInvitationDetails(Int32 sharedUserID);
        #endregion

        void UpdateInvitationViewedStatus(Int32 currentUserID, Int32 invitationID);

        List<InvitationDocument> GetAttestatationDocumentDetails(Int32 invitationGroupID);

        InvitationDocument GetInvitationDocumentByDocumentID(Int32 invitationDocumentID);

        InvitationDocument GetInvitationDocumentByProfileSharingInvitationID(Int32 profilesharinginvitationID);


        List<ProfileSharingInvitationGroupContract> GetAttestationDetailsData(Int32 clientID, Int32 currentUserID, Int32 adminInitializedInvitationStatus);

        //UAT-1509
        List<ProfileSharingInvitationGroupContract> GetSharedInvitationsData(String searchContractXML, String gridCustomPagingXML);

        #region Attestation Document Code

        //Int32 SaveAttestationDocument(String pdfDocPath, Int32 documentTypeId, Int32 currentLoggedInUserID);

        Boolean SaveInvitationDocumentMapping(List<InvitationDocumentMapping> lstInvitationDocumentMapping);

        Boolean SaveInvAttestationDocWithPermissionType(List<InvAttestationDocWithPermissionType> lstInvAttestationDocWithPermissionType);
        #endregion

        ProfileSharingInvitation GetInvitationDataByToken(Guid inviteToken);

        /// <summary>
        /// Gets the list of invitations that has been sent by the applicant
        /// </summary>
        /// <param name="applicantOrgUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        DataTable GetApplicantInvitations(Int32 applicantOrgUserId, Int32 tenantId, Int32? isAgnecyShareForAdmin);

        /// <summary>
        /// Check Whether shared user exists or not
        /// </summary>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        Entity.OrganizationUser IsSharedUserExists(Guid token, Boolean isProfileSharingToken, Int32? agencyUserID);

        /// <summary>
        /// Method to get Shared User Data from Invitation Sent by applicant(currently only Email)
        /// </summary>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        String GetSharedUserDataFromInvitation(Guid token, Boolean isProfileSharingToken, Int32? agencyUserID);

        /// <summary>
        /// Method to Update Invitee Organization UserID in ProfileSharingInvitation table
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        Boolean UpdateInviteeOrganizationUserID(Int32 orgUserID, Guid inviteToken, Int32 invitationStatusID, Guid inviteeUserID, Int32? agencyUserID, out String profileSharingInvitationIds);

        /// <summary>
        /// Save the New Invitation and return the ID of the invitation generated.
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <returns>Tuple with InvitationID & its related Token</returns>
        Tuple<Int32, Guid> SaveProfileSharingInvitation(InvitationDetailsContract invitationDetails, Int32 genaratedInvitationGroupID, Int32 invGroupTypeID);

        /// <summary>
        /// Save the Bulk Invitations sent by admin/client admin
        /// </summary>
        /// <param name="lstInvitationDetails"></param>
        /// <param name="invitationGroup"></param>
        /// <param name="generatedInvitationGroupID"></param>
        /// <returns></returns>
        List<ProfileSharingInvitation> SaveAdminInvitations(List<InvitationDetailsContract> lstInvitationDetails, ProfileSharingInvitationGroup invitationGroup);

        /// <summary>
        /// Gets the master details for the selected Invitation
        /// </summary>
        /// <param name="invitationId"></param>
        /// <returns></returns>
        ProfileSharingInvitation GetInvitationDetails(Int32 invitationId);

        void UpdateInvitationStatus(Int32 statusId, Int32 invitationId, Int32 currentUserId);

        /// <summary>
        /// Update the Status of the Invitation
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        void UpdateBulkInvitationStatus(Int32 statusId, List<Int32> invitationId, Int32 currentUserId);

        DataTable GetInvitationData(InvitationSearchContract searchContract, CustomPagingArgsContract gridCustomPaging);

        /// <summary>
        /// Update the Views remaining of the Invitation
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        Boolean UpdateInvitationViewsRemaining(Int32 invitationId, Int32 currentUserId, Int32 expiredInvitationTypeId);

        /// <summary>
        /// Update Notes of the Invitation
        /// </summary>
        /// <param name="inviteeNotes"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        Boolean UpdateInvitationNotes(Int32 invitationId, Int32 currentUserId, String notes, List<lkpAuditChangeType> lstAuditChangeType);

        List<Agency> GetAllAgency(Int32 institutionID);

        IEnumerable<ProfileSharingInvitation> GetInvitationsByInviteeOrgUserID(Int32 inviteeOrgUserID);

        List<usp_GetAgencyUserData_Result> GetAgencyUserData(Int32 institutionID, Int32 agencyID);

        DataTable GetAttestationDocumentData(String clientInvitationIds);

        Int32 GenarateNewInvitationGroup(ProfileSharingInvitationGroup invitationGroupObj);

        #region Profile Sharing Attestion Document
        InvitationDocument GetInvitationDocuments(Int32 invitationId, String attestationTypeCode);
        #endregion

        void UpdateProfileSharingInvRotationDetails(InvitationDetailsContract invitationDetails);

        #endregion

        #endregion

        #region UAT-1218
        void UpdateClientContactUserID(Guid userID, String clientContactEmail, Int32 orgUserID);
        #endregion
        usp_GetAgencyDetailByAgencyID_Result GetAgencyDetailByAgencyID(Int32 agencyID);

        List<usp_SearchAgency_Result> SearchAgency(string searchStatus);

        Tuple<String, Int32> SaveAgencyInstitutionMapping(AgencyInstitution agencyInstitution);

        bool IsAgencyAssociateWithInstitution(Int32 institutionID, Int32 agencyID);

        Boolean CheckForBkgInvitation(int orgUserID);

        Boolean IsNPINumberExist(String npiNumber);

        List<SheduledInvitationContract> GetScheduledInvitations(Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag);
        DataTable GetClientDataForAgencyAndAgencyHierarchyUsingFlatTable(String AGencyID, String Tenantid, CustomPagingArgsContract customPagingArgsContract);
        void SaveScheduledInvitationsSharedMetaData(List<SheduledInvitationContract> lstCurrentGroupInvitation, Int32 currentUserId, DateTime currentDateTime);

        void UpdateInvitationGroupSaveStatus(List<Int32> lstInvitationIds, Int32 currentUserId);

        void UpdateRetryCountForFailedInvitation(Int32 currentGroupID, Int32 currentUserId);

        IEnumerable<InvitationDocumentMapping> GetInvitationDocumentMapping(Int32 invitationGroupID);

        #region UAT 1320 Client admin expire profile shares
        Boolean SaveUpdateProfileExpirationCriteria(InvitationDetailsContract invitationDetailsContract, List<Int32> lstSelectedInvitationIDs);
        #endregion

        #region Shared User Dashboard Data

        /// <summary>
        /// Get the Shared User Dashboard Pie Chart related data
        /// </summary>
        /// <param name="inviteeOrgUserID"></param>
        /// <returns></returns>
        List<InstitutionProfileContract> GetSharedStudentsPerInstitution(Int32 inviteeOrgUserID, DateTime? fromDate, DateTime? toDate);

        /// <summary>
        /// Get the Shared User Dashboard Calendar and Grid related data
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        List<DashBoardRotationDataContract> GetDashBoardRotationData(Guid userId, Int32 organizationUserId);

        /// <summary>
        /// Get the Shared User basic details to be displayed on dashboard.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        SharedUserDashboardDetailsContract GetSharedUserDashboardDetails(Guid userId);

        AgencyUserDashboardDetailsContract GetAgencyUserDashboardDetails(Guid userId);

        #endregion

        #region UAT 1530 WB: If sharing with an agency that does not have any users, client admin should have to fill out a form displaying the information of the person they would like to add.

        Boolean SaveSharedUserForReview(SharedUserReviewQueue sharedUserReviewQueue);
        Tuple<List<SharedUserReiewQueueContract>, Int32> GetSharedUserReviewQueueData(SharedUserReiewQueueContract searchDataContract, CustomPagingArgsContract gridCustomPaging);
        Boolean UpdateSharedUserReviewQueueStatus(List<Int32> ids, Int32 currentUserId, Int32 reviewedStatusId);
        Boolean DeleteSharedUserReviewQueueRecord(Int32 SURQ_ID, Int32 currentUserId);

        #endregion

        List<AgencyAttestationDetailContract> GetAttestationReportTextForAgency(String agencyID);

        String SaveInvitationConfirmation(ProfileSharingInvitationConfirmation profileSharingInvitationConfirmation);

        bool MarkIsViewedByInvitationConfirmationId(Int32 profileSharingInvitationConfirmationId, int currentUserId);

        List<ProfileSharingInvitationConfirmation> GetProfileSharingInvitationConfirmations(int currentUserId);

        bool IsNeedToStartPolling(int currentUserId);


        #region UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement.
        //Change for UAT-1641:- AU Link with multiple agencies
        List<AgencyContract> GetAttestationReportTextForAgencyUser(Guid userID);

        String UpdateAttestationReportTextForAgencyUser(Int32 loggedInUserID, Int32 agencyId, String attestationReportText);
        #endregion

        //FOR UAT-2463, Added bool allInvitationsToBeUpdated in the method
        bool SaveUpdateSharedUserInvitationReviewStatus(List<Int32> lstProfileSharingInvitationIds, Int32 currentLoggedInUserId, Int32 inviteeOrgUserId, Int32 selectedInvitationReviewStatusId, String agencyUserNotes = null, bool allInvitationsToBeUpdated = false, bool needToChangeStatusAsPending = true, Int32 applicantId = 0, Int32 rotationId = 0, Int32 tenantID = 0, List<lkpAuditChangeType> lkpAuditChangeType = null, Boolean isAgencyScreen = false, String selectedInvitationReviewStatusCode = null, Boolean isAdminLoggedIn = false);

        #region UAT 1496: WB: Updates to Client admin profile expiration functionality
        List<ProfileSharingInvitation> GetProfileSharingInvitationByIds(List<Int32> lstSelectedInvitationIds);

        Boolean UpdateViewRemaining();
        #endregion

        #region UAT-1796 Enhance Client User Search to also display Agency Users and grid enhancements.
        DataTable GetAgencyUserDetailByID(Int32 agencyUserId);
        #endregion

        #region UAT-1796, Enhance Client User Search to also display Agency Users and grid enhancements
        List<Agency> GetAgenciesByInstitionIDs(List<int> lstInstitutionIDs);
        #endregion
        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
        List<Agency> GetInstitutionMappedAgency(List<Int32> institutionIDs, String userID);
        Boolean AnyApplicantSharingExist(Int32 orgUserId, Int32 invitationSourceTypeID, List<Int32> institutionIDs);
        #endregion

        Boolean UpdateAgencyUserAgenciesVerificationCode(Int32 agencyUserID, String verificationCode, Entity.OrganizationUser orgUser);

        List<Agency> GetAgenciesFromAllTenants();

        #region UAT-1844:
        String GetRotationSharedReviewStatus(Int32 clinicalRotationID, Int32 orgUserId, Int32 tenantId, Int32 agencyID, ref Int32? lastReviewedByID);
        #endregion

        List<RequirementSharesDataContract> GetRequirementSharesData(String userId, Int32 currentLoggedInUserId, String tenantIds, ClinicalRotationDetailContract clinicalRotationSearchContract, InvitationSearchContract invitationSearchContract, CustomPagingArgsContract gridCustomPaging, String customAttributeXML);

        Boolean IsIndividualShared(Int32 ProfileSharingInvitationID);
        Boolean HasConsidatePassportPermission(Int32 agencyUserID);//UAT-2520

        //UAT-2090 : Complete Question 4 (C5) from UAT-2052
        Boolean SaveInvitationReviewStatusNotes(String InvitationIDs, String clinicalRotationIDs, Int32 selectedInvitationReviewStatusId, String notes, Int32 currentLoggedInUserId, Int32 organisationUserID, Boolean isIndividualReview);

        #region UAT-2071, Configuration Rotation and Tracking packages must be fully compliant to share
        Boolean IsTrackingPkgCompliantReqd(Int32 agencyID, Int32 agencyPrmsnTypeID, List<Int32> AgencyIDs);

        Dictionary<Int32, String> IsRequirementPkgCompliantReqd(String agencyID, Int32 agencyPrmsnTypeID);

        Dictionary<Int32, Int32> GetAgencyPermisionByAgencyID(int agencyID);
        #endregion

        //UAT-2181:Enhance adding tenants to agencies with check boxes on the Manage Agencies screen to select which agencies you would like to add the selected tenant to
        List<Int32> SaveAgenciesInstitutionMapping(List<Int32> SelectedAgencyIDs, Int32 TenantID, Int32 CurrentLoggedInUserId);


        bool IsOnlyRotationPkgShare(List<Int32> agencyIDs, int agencyPrmsnTypeID);

        List<Tuple<Int32, Int32, Int32, List<Int32>>> GetRotationSharedInvitations(List<InvitationIDsDetailContract> lstClinicalRotations, Int32 inviteeOrgUserId);

        #region UAT 2367
        List<Guid> GetAgencyVerificationCode(Int32 agencyUserID);
        AgencyUser GetAgencyUserByID(int agencyUserID);
        #endregion

        List<InvitationSharedInfoDetails> GetInvitationDocumentData(Int32 selectedRotationId, Int32 tenantID, Int32 agencyID); // UAT-2443
        List<InvitationAttestationDocumentDataWithAgencyUserPermissions> GetAttestationDocumentDetailsWithPermissionType(Int32 selectedRotationId, Int32 tenantID, Int32 agencyID); // UAT-3715
        Boolean UpdateInvitationDocumentPath(Int32 invitationDocumentId, String pathToUpdate, Int32 currentLoggedInUserId, Boolean isForEveryOne); // UAT-2443
        #region UAT-2452
        Boolean SaveAgencyUserSharedPermission(List<AgencyUserSharedProfilePermission> agencyUserSharedPermission);
        #endregion

        //Code commented for UAT-2803
        //UAT-2538
        //    Boolean AssignUnAssignAgencyUsersToSendEmail(List<Int32> selectedAgencyUserIDs, Boolean IsNeedToSendEmail, Int32 CurrentLoggedInUserId);

        //UAT-2538
        List<Entity.OrganizationUser> GetAgencyUserfromAgency(Int32 AgencyID);
        //UAT-2538
        Agency GetAgency(Int32 AgencyID);
        List<Int32> GetAppOrgOnInvitations(List<Int32> LstInvitationIDs);
        List<Entity.OrganizationUser> GetClinicalRotationApplicantSharedData(Int32 RotationID, Int32 AgencyID);
        List<Int32> GetInvitationIDsIfInvitationStatusChanged(List<Int32> lstRotationInvitations, Int32 selectedInvitationReviewStatusId);

        Int32 GetAgencyInstitutionID(Int32 agencyID, Int32 tenantID);//UAT-2529
        Dictionary<Int32, String> GetAgencyInstitutionIdsForIndivialSharingPermission(String agencyUseremail, Int32 tenantID);//UAT-2529

        #region UAT-2641
        Boolean SaveUpdateAgencyHierarchyUserDetails(Int32 agencyUserID, List<Int32> lstAgencyHierarchyIds, Int32 currentLoggedUserID);
        Dictionary<Int32, String> GetAgencyHierarchyOfCurrentTenantToAddUser(Int32 tenantID);
        #endregion

        #region UAT-2639:
        AgencyHierarchyAgencyProfileSharePermission GetAgencyHierarchyProfileSharePermission(Int32 agencyID, Int32 TenantID);
        #endregion

        Boolean DeleteAgencyHierarchyAgency(Int32 AgencyId, Int32 CurrentLoggedInUser, Boolean IsAdmin, Int32 TenantID);

        Boolean IsCurrentLoggedInUser(String CurrentLoggedInOrgUserID, Int32 SelectedAgencyUserID);

        Boolean IsAgencyUserOnDifferentNode(Int32 CurrentLoggedInOrgUserID, Int32 SelectedAgencyUserID);

        List<String> GetAgencyByAgencyUserID(Int32 agencyUserID);

        #region UAT-2511
        Boolean SaveRotationAuditHistory(Int32 rotationID, Int32 tenantID, Int32 newReviewStatusID, Int32 oldReviewStatusID, String newNotes, String oldNotes, Int32 currentLoggedInUserID
                                         , List<lkpAuditChangeType> lstAuditChangeType, Int32 agencyId, String newReviewStatusCode);

        List<AgencyUserAuditHistoryDataContract> GenerateAuditHistoryDataForRerquestForAudit(List<Int32> profileSharingInvitationIds, Int32 currentLoggedInUserID, List<lkpAuditChangeType> lstAuditChangeType);

        Boolean SaveAgencyUserAuditHistory(List<AgencyUserAuditHistoryDataContract> lstAuditDataContract, Boolean isSaveChangesRequired);

        DataTable AgencyUserAuditHistory(Int32 institutionID, Int32 agencyID, string rotationName, string applicantName, string updatedByName, DateTime updatedDate, CustomPagingArgsContract customPagingcontract);
        #endregion
        #region UAT 2548
        List<AgencyDetailContract> GetAgencyUserMappedAgencies(Int32 AgencyUserID);
        List<TenantDetailContract> GetAgencyHierarchyMappedTenant(Int32 AgencyUserID);
        #endregion

        #region UAT-2706
        Entity.SharedDataEntity.ClientSystemDocument GetSharedClientSystemDocument(Int32 clientSystemDocId);
        #endregion

        List<Int32> AnyAgencyUserExists(Int32 institutionID, String agencyIds);

        #region UAT-2774
        List<SharedUserInvitationDocumentContract> GetSharedUserInvitationDocumentDetails(Int32 ProfileSharingInvitationID, Int32 ApplicantOrgUserID, Boolean IsRotationSharing);
        Boolean SaveSharedUserInvitationDocumentDetails(List<SharedUserInvitationDocumentMapping> lstSharedUserInvoitationDocs);
        Boolean DeletedSharedUserInvitationDocument(Int32 InvitationDocumentID, Int32 ApplicantOrgUserID, Int32 ProfileSharingInvitationGroupID, Int32 SharedDocTypeID, Int32 CurrentLoggedInUserID);
        Boolean IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 ApplicantOrgUserID, Int32 ProfileSharingInvitationGroupID, Int32 SharedDocTypeID);
        #endregion

        RequirementApprovalNotificationDocumentContract GetAgencySystemDocument(Int32 agencyID);

        Boolean GetAgencyUserSSN_Permission(String userID);

        List<RequirementSharesDataContract> GetApprovedRotationsAfterSinceLastLogin(Int32 applicantOrgUserID, Int32 tenantID, DateTime lastLoginDate);

        #region UAT-2784
        String GetAgencySetting(Int32 agencyId, Int32 settingTypeId);
        Boolean CheckExpirationCriteriaForRotation(List<Int32> lstAgencyIds, Int32 settingTypeId);
        #endregion

        Entity.OrganizationUser GetAdminDetailsWhoSharedProfile(Int32 applicantOrgUserID, Int32 clinicalRotationID, Int32 tenantID, Int32 agencyID);


        #region UAT-2803 : Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        List<lkpAgencyUserNotification> GetAgencyUserNotifications();
        Boolean SaveAgencyUserNotificationMappings(Int32 agencyUserID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInOrgUserID);
        void DeleteAgencyUserNotificationMappings(Int32 AgencyUserID, Int32 CurrentLoggedInUserId);
        Boolean UpdateAgencyUserNotificationMappings(Int32 AgencyUserID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInUserId);
        List<Int32> GetAgencyUserNotificationPermission(List<Int32> lstAgencyUserIds, String AgencyUserNotificationCode);
        Int32 GetAgencyUserNotificationPermissionThroughEmailID(String emailID, String AgencyUserNotificationCode);
        Boolean IsOrgUserhaveNotificationPermission(Int32 orgUserID, String agencyUserNotificationCode);
        //List<Int32> GetOrgUserfromAgencyUsers(List<Int32> lstAgencyIDs);
        #endregion

        #region UAT-2926
        List<String> GetAgencyHierachyAgencyIds(List<Tuple<Int32, Int32>> AgencyHierachyAgencyIds);
        #endregion

        InvitationDocument GetUploadedInvitationDocument(Int32 PSIG_ID, Int32 documentTypeID);

        #region UAT-2942
        List<ApprovedProfileSharingEmailContract> GetApprovedProfileInvitationDetailsByIds(List<Int32> lstProfileSharingInvitationId, Int32 selectedInvitationReviewStatusId, Int32 applicantInvitationTypeId);
        #endregion

        #region UAT-2943
        Int32 GetReviewStatusIDByProfileSharingInvitationID(Int32 invitationId);
        #endregion

        DataSet GetAgencyRotationMapping();

        Boolean SaveAgencyNotification(Int32 subEventID, String entityName, Int32 entityID, DateTime dataFetchedFromDate, DateTime dataFetchedFromToDate, Int32 createdByID);

        DateTime? GetLastNotificationSentDate(Int32 subEventID, Int32 entityID);

        List<Tuple<int, List<int>>> FilterInvitationIdsByTenant(List<int> lstPSI);

        Int32 GetInvitationReviewStatusIDByStatusCode(string reviewStatusCode);

        List<Int32> FilterInvitationIdsByReviewStatusID(List<Int32> lstInvitations, Int32 reviewStatusID);

        List<AgencyUserInfoContract> GetAgencyUserListInRotationBasedOnPermission(Int32 RotationID, String NotificationTypeCode, Int32 tenantID);//UAT-3108
        //UAT 3102
        Boolean UpdateAgencyUserEmailAddress(Int32 agencyUserId, String emailId, Int32 loggedInUserId);
        //UAT-3338
        ProfileSharingInvitationGroup GetProfileSharingGroupData(Int32 agencyId, Int32 clinicalRotationId);


        Boolean InsertSharedUserRotReviewForNonRegUser(String profileSharingInvIds, Int32 organizationUserId); //UAT-3400


        #region UAT 3294
        Boolean IsApplicantSendInvitationToAgencyUser(Guid agencyUserId);
        List<Entity.SharedDataEntity.AgencyUser> GetAgencyUserByAgencIds(List<Int32> agencyHiearchyIDs);
        Boolean MoveApplicantEmailShareToAgencyUser(Guid fromAgencyUserID, Int32 tenantID, Guid toAgencyUserID, Int32 currentLoggedInUserId);
        #endregion

        #region UAT-3360
        Boolean IsAgencyUserExist(List<String> userEmails);
        #endregion

        #region UAT-3316
        Tuple<List<AgencyUserPermissionTemplateContract>, Int32, Int32, List<AgencyUserPermissionTemplateMappingContract>, List<AgencyUserPermissionTemplateNotificationsContract>> GetlstAgencyUserPermissionTemplate(AgencyUserPermissionTemplateContract searchDataContract, CustomPagingArgsContract customPagingArgsContract);
        Int32 SaveAgencyUserPerTemplate(AgencyUserPermissionTemplateContract _agencyUserPerTemplate, Int32 loggedInUserID, List<AgencyUserPermissionTemplateMapping> lstAgencyUserPerTempMapping);
        Boolean SaveAgencyUserTemplateNotificationMappings(Int32 agencyUserTemplateID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInOrgUserID);

        String DeleteAgencyUserPermissionTemplate(Int32 tenantID, Int32 AGUPT_ID, Int32 LoggedInUserId, Boolean IsAdmin);

        void DeleteAgencyUserPerTemplateNotificationMappings(Int32 AgencyUserID, Int32 CurrentLoggedInUserId);

        AgencyUserPermissionTemplate UpdateAgencyUserPermissionTemplate(AgencyUserPermissionTemplateContract _agencyUserPerTemplate, Int32 LoggedInUserId, Boolean IsAdmin, List<AgencyUserPermissionTemplateMapping> lstAgencyUserPermission);

        Boolean UpdateAgencyUserPerTemplateNotificationMappings(Int32 AgencyUserTemplateID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInUserId);

        List<AgencyUserPermissionTemplate> GetAgencyUserPermissionTemplates();

        void DeleteAgencyUserTemplateNotificationMappings(Int32 AgencyUserTemplateId, Int32 CurrentLoggedInUserId);

        List<AgencyUserPermissionTemplateMapping> GetAgencyUsrPerTemplateMappings(Int32 permisisonTemplateId);
        List<AgencyUserPerTemplateNotificationMapping> GetAgencyUsrPerTemplateNotificationsMappings(Int32 permisisonTemplateId);

        List<lkpAgencyUserPermissionType> GetAgencyUserPermissionTypes();
        List<Int32> GetInvitationSharedInfoTypeID(Int32 templateID);
        List<Int32?> GetApplicationInvitationMetaDataID(Int32 templateID);
        List<AgencyUserPermission> GetAgencyUsrPermisisonMappings(Int32 userId);
        #endregion

        #region UAT-3470
        Boolean SaveUpdateInvitationArchiveState(String InvitationIds, String rotationContract, Int32 CurrentLoggedInUser, Boolean IsPerformArchiveOperation);
        #endregion


        #region UAT-3353
        List<SharedUserInvitationDocumentContract> GetSharedUserRotationInvitationDocumentDetails(Int32 tenantID, Int32 clinicalRotationID, Int32 agencyID);
        Boolean SaveSharedUserRotationInvitationDocumentDetails(List<SharedUserRotationInvitationDocumentMapping> lstSharedUserRotationInvitationDocumentMapping);
        Boolean DeletedSharedUserRotationInvitationDocument(Int32 invitationDocumentID, Int32 tenantID, Int32 clinicalRotationID, Int32 agencyID, Int32 currentLoggedInUserID);
        Boolean IsRotationInvitationDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 tenantID, Int32 clinicalRotationID, Int32 SharedDocTypeID);
        Int32 GetProfileSharingGroupIDByClinicalRotationID(Int32 tenantID, Int32 clinicalRotationID);
        #endregion

        #region UAT-3606
        List<Entity.OrganizationUser> GetProfileSharingInvitationApplicantSharedData(List<Int32> PSI_ID);
        #endregion

        #region UAT-3715
        List<String> GetSharedUserBkgPermissions(Int32 CurrentLoggedInUserId);

        InvitationSharedInfoDetails GetSharedUserCurrentPermission(Int32? agencyUserId, Int32? agencyUserTemplateId);

        Boolean UpdateDocMappingForInvAttestation(Int32? agencyUserId, Int32? templateId, String rotationSpecificPermissionTypeCode, String profileSpecificPermissionTypeCode, Int32 CurrentLoggedInUserID);

        #endregion

        #region UAT-3719
        void SaveAgencyUserPermissionAuditDetails(Int32? AgencyUserId, Int32? AgencyUserTemplateID, Int32 CurrentLoggedInUserID);
        #endregion

        //Update Current Invitation Shared Info Type.
        Boolean UpdateSharedInfoTypeInComplAndReqSubs(Int32? agencyUserId, Int32? agencyUserTemplateId, Int32 currentLoggedInUserID);

        List<SyncRequirementPackageObject> GetReqPackageObjectList(List<Int32> lstReqSyncObjectIds, Int32 chnageTypeID_ReqCategory);

        List<RequirementSharesDataContract> GetRequirementNonComplaintSharesData(String userId, Int32 currentLoggedInUserId, String tenantIds, ClinicalRotationDetailContract clinicalRotationSearchContract, CustomPagingArgsContract gridCustomPaging);

        //UAT-3977
        ClientContact GetClientContact(Guid userID);
        List<AgencyUserReportPermissionContract> GetAgencyUserReportPermissions(Int32 agencyUserId);//UAT-3664
        List<AgencyUserPermissionTemplateMapping> GetAgencyUserTemplateReportPermissions(Int32 templateId);

        #region UAT 3294

        List<Entity.SharedDataEntity.AgencyUser> GetAgencyUserListByAgencIds(List<Int32> agencyIDs);

        Int32 GetAgencyUserOrganizationUserId(Guid ?agencyUserID);

        #endregion

        //UAT-4658
        Boolean IsAgencyUserPresent(Int32 templateId);
        //End UAT-4658
    }
}
