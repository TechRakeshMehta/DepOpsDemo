using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IProfileSharingClientRepository
    {
        #region Applicant Invitations

        /// <summary>
        /// Save the new Invitation
        /// </summary>
        /// <param name="invitationDetails"></param>
        void SaveInvitationDetails(InvitationDetailsContract invitationDetails, List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoType);

        /// <summary>
        /// Save the Bulk Invitation Details in Tenant, sent by admin/client admin
        /// </summary>
        /// <param name="lstInvitationDetails"></param>
        /// <param name="lstSharedInfoType"></param>
        Boolean SaveAdminInvitationDetails(List<InvitationDetailsContract> lstInvitationDetails, List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoType
                                         , List<SharedUserSubscriptionSnapshotContract> lstSharedUserSnapshot, Int32 rotationId, Int32 reviewStatusId, Int32 agencyID);

        /// <summary>
        /// Save the details of the Packages & their categories/Service groups, which were either not included or partially included for sharing, during 'Submit Later' option
        /// </summary>
        /// <param name="lstSharedPkgData"></param>
        /// <param name="tenantId"></param>
        void SaveScheduledExcludedPackageData(List<SharingPackageDataContract> lstSharedPkgData, Int32 currentUserId);

        /// <summary>
        /// Gets the Invitation related data for the selected Invitation, in Edit Mode
        /// </summary>
        /// <param name="invitationId"></param>
        /// <returns></returns>
        Tuple<List<SharedComplianceSubscription>, List<SharedBkgPackage>> GetInvitationData(Int32 invitationId);

        /// <summary>
        /// Update/Save invitation details, depending on the details being updated by applicant
        /// </summary>
        /// <param name="invitationDetails"></param>
        void UpdateInvitationDetails(InvitationDetailsContract invitationDetails, List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoType);

        /// <summary>
        /// Gets the Email Subject and Content from AppDBConfiguration, based on the keys.
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <param name="contentCode"></param>
        /// <returns></returns>
        Dictionary<String, String> GetInvitationEmailContent(String subjectCode, String contentCode);

        /// <summary>
        /// Get the list of packages associated for a particular Invitation, for Re-send invitation Email
        /// </summary>
        /// <param name="invitationId"></param>
        /// <returns></returns>
        Tuple<List<SharedComplianceSubscription>, List<SharedBkgPackage>> GetSharedPackages(Int32 invitationId);

        #endregion

        //#region Manage Agency
        //List<Entity.Agency> GetAgencies(Int32 TenantID, Boolean IsAdmin);

        //String SaveAgencies(Int32 TenantId, String Name, String Description, List<Int32> lstTenantID, Int32 CurrentLoggedInUserId, Boolean IsAdmin);

        //String UpdateAgencies(Int32 TenantId, String Name, String Description, List<Int32> tenantIDs_Added, List<Int32> tenantIDs_Removed, Int32 CurrentLoggedInUserId, Int32 AG_ID, Boolean IsAdmin);

        //List<Entity.AgencyInstitution> GetAgencyInstitutionForAgencies(IEnumerable<Int32> lstAgencyId);

        //String DeleteAgency(Int32 CurrentLoggedInUserId, Int32 AG_ID, Boolean IsAdmin, Int32 tenantId);
        //#endregion

        //List<Entity.ApplicantInvitationMetaData> GetApplicantInvitationMetaData();

        #region Shared Invitation Detail for compliance and background packages

        /// <summary>
        /// Get the List of SharedComplianceSubscription
        /// </summary>  
        /// <returns></returns>
        List<SharedComplianceSubscription> GetSharedComplianceSubscriptions(Int32 invitationId);

        /// <summary>
        /// Get the List of SharedBkgPackage
        /// </summary>  
        /// <returns></returns>
        List<SharedBkgPackage> GetSharedBkgPackages(Int32 invitationId);

        /// <summary>
        /// Get the List of Shared category list
        /// </summary>  
        /// <returns></returns>
        List<ApplicantComplianceCategoryData> GetSharedCategoryList(Int32 packageSubscriptionId, List<Int32> sharedCategoryIds, Int32 snapshotId);

        /// <summary>
        /// To get shared category documents of invitations
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="lstProfileSharingInvitationID"></param>
        /// <returns></returns>
        DataTable GetSharedCategoryDocuments(Int32 packageSubscriptionId, String sharedcategoryids);



        #endregion

        #region Manage Invitations

        DataTable GetApplicantInviteDocuments(List<InvitationIDsContract> lstProfileSharingInvitationID);
        DataTable GetClientInviteDocuments(String clientInvitationIDs);
        List<SharedComplianceSubscription> GetSharedComplianceSubscriptionByInvitationIDs(List<Int32> lstInvitationID);
        List<ApplicantDocument> GetApplicantDocumentByIDs(List<Int32> lstApplicantDocumentID);
        DataTable GetPassportReportData(String xmlData);

        #endregion

        //#region Manage AgencyUser
        //List<Entity.AgencyUser> GetAgencyUser(Int32 tenantID, Boolean IsAdmin);

        //String SaveAgencyUser(AgencyUserContract _agencyUser);

        //String DeleteAgencyUser(Int32 tenantID, Int32 AUG_ID, Int32 LoggedInUserId, List<Int32> AgencyInstitutionId, Boolean IsAdmin);

        //String UpdateAgencyUser(AgencyUserContract _agencyUser, Int32 LoggedInUserId, Boolean IsAdmin, List<Int32> agencyInstitutionIDs_Added, List<Int32> agencyInstitutionIDs_Removed);

        //List<Int32> GetAgencyUserSharedDataForAgencyUserID(Int32 agencyID);

        //List<Int32> GetAgencyUserInstitutesForAgencyUserID(Int32 agencyUserID);

        //List<Entity.InvitationSharedInfoMapping> GetInvitationSharedInfoTypeByAgencyUserID(Int32 agencyUserID);

        //Entity.AgencyUser IsEmailAlreadyExistAgencyUser(String email);
        //#endregion

        #region Agency Sharing

        DataTable GetDataForAgencySharing(INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract searchDataContract, CustomPagingArgsContract customPagingArgsContract);

        Int32 SaveImmunizationSnapshot(Int32 currentUserID, Int32 packageSubscrptionID);

        /// <summary>
        /// Generate the Snapshot of the Requirement Package
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="packageSubscrptionID"></param>
        /// <returns></returns>
        Int32 SaveRequirementSnapshot(Int32 currentUserId, Int32 packageSubscrptionId);

        #endregion

        #region Immunization Data For Snapshot
        /// <summary>
        /// Get Immuniztion Data From snapshot.
        /// </summary>  
        /// <returns></returns>
        DataSet GetImmunizationDataFromSnapshot(Int32 snapshotId);

        /// <summary>
        /// To get Applicant documents From snapshot
        /// </summary>
        /// <param name="sharedcategoryids"></param>
        /// <param name="snapshotId"></param>
        /// <returns></returns>
        DataTable GetApplicantDocumentsFromSnapshot(String sharedcategoryids, Int32 snapshotId);
        #endregion

        //List<int> CheckTenantsNeedToDisable(int AgencyID);

        /// <summary>
        /// Gets the OrganizationUser data, based on the OrgganizationUserId
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        OrganizationUser GetOrganizationUser(Int32 organizationUserId);

        #region UAT-1213: Updates to Agency User background check permissions.
        List<vwBkgOrderFlagged> GetBkgOrderFlagged(List<Int32> bkgOrderIds);
        #endregion

        #region UAT-1210: As a client admin, I should be able to see when and what was shared through profile sharing
        List<GetProfileSharingData_Result> GetProfileSharingData(Int32 invitationGroupID);
        #endregion

        #region UAT 1318

        /// <summary>
        /// Gets the list of Applicants added to a Rotation, for sending the ProfileSharingInvitation
        /// </summary>
        /// <param name="RotationId"></param>
        /// <param name="agencyId"></param>
        /// /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        DataTable GetRotationMembers(Int32 RotationId, Int32 agencyId, CustomPagingArgsContract customPagingArgsContract, String rotationMemberIds, String instructorPreceptorOrgUserIds);

        #endregion

        List<Int32> GetSharedCategoryList(Int32 invitationID, Int32 packageSubscriptionID);

        #region Shared Invitation Detail for Requirement Packages

        /// <summary>
        /// Get the List of SharedComplianceSubscription
        /// </summary>  
        /// <returns></returns>
        SharedRequirementSubscription GetSharedRequirementSubscriptions(Int32 invitationId);

        /// <summary>
        /// Get Requirement Data From snapshot.
        /// </summary>  
        /// <returns></returns>
        DataSet GetRequirementDataFromSnapshot(Int32 snapshotId);

        DataTable GetApplicantRequirementDocumentsFromSnapshot(String sharedcategoryids, Int32 snapshotId, Int32 orgUserID,String rotationID,Boolean IsApplicantDropped); //UAT 3125 Added New Parameters

        DataTable GetInstructorRequirementDocuments(String sharedcategoryids, Int32 loggedInUserID, String rotationID, Int32 InstructorOrgId);//UAT-3338

        /// <summary>
        /// Get the List of Shared category list
        /// </summary>  
        /// <returns></returns>
        List<ApplicantRequirementCategoryData> GetSharedRequirementCategoryList(Int32 packageSubscriptionId, List<Int32> sharedCategoryIds, Int32 snapshotId, Boolean IsInstructorPreceptorData);
        #endregion

        bool UpdateSharedCategoryData(int ProfileSharingInvitationID, List<SharedInvitationSubscriptionContract> lstSharedInvitationSubscriptionContract);

        DataSet GetScheduledInvitationData(String applicantOrgUserIdCSV, Int32? rotationID);

        List<ScheduledInvitationExcludedPackageData> GetScheduledInvitationExcludedPackageDataByProfileSharingInvitationGroupID(int profileSharingInvitationGroupID);

        /// <summary>
        /// Get the Shared user subscriptions in order to generate the Snapshot, when Submit Later is used.
        /// </summary>
        /// <param name="rotationId"></param>
        /// <returns></returns>
        List<ClientContactProfileSharingData> GetSharedUserSubscriptions(Int32 rotationId);

        List<ShareHistoryDataContract> GetShareHistoryData(ShareHistorySearchContract shareHistorySearchContract, CustomPagingArgsContract customPagingArgsContract);

        List<usp_GetProfileSharingDataByInvitationId_Result> GetProfileSharingDataByInvitationId(Int32 invitationID);

        List<AgencyHierarchyMapping> GetInstituteHierarchyForSelectedAgency(int agencyID);

        Tuple<Boolean, Boolean> GetAgencyInstitutionPermissionForSelectedAgency(int agencyID); //UAT-2529

        Boolean GetAgencyProfileSharingPermission(Int32 agencyInstitutionID); //UAT-2529
        Boolean SaveAgencyHierarchyMapping(AgencyHierarchyContract agencyHierarchyContract, int currentLoggedInUserID, Int32 agencyID, Int32 agencyInstitutionID);
        Tuple<Boolean, String> GetApplicantIndividualProfileSharingPermission(Dictionary<Int32, String> agencyInstitutionIds);

        #region UAT-1881
        DataTable GetAllAgencyForOrgUser(Int32 OrgUserId);
        #endregion

        Boolean DeleteAgencyHierarchyMappings(Int32 agencyID, Int32 loggedInUserID, Int32 agencyInstitutionId);

        #region UAT 1882: Phase 3(12): When a student profile shares, they should be presented with a selectable list of agencies, which have been associated with nodes they have orders with.
        List<AgencyContract> GetAgencyForApplicant(Int32 orgUserID);
        #endregion

        #region UAT-2071: Configuration Rotation and Tracking packages must be fully compliant to share.
        List<RotationAndTrackingPkgStatusContract> GetComplianceStatusOfImmunizationAndRotationPackages(String delimittedOrgUserIDs, String delimittedTrackingPkgIDs, Int32 rotationID, String SearchType);
        #endregion

        #region UAT-2051, No defining details about Roation. Roation/Profile simply says "roation shared".
        String GetSharingInfoByInvitationGrpID(Int32 invtationGrpID);
        #endregion

        #region UAT-2196, Add "Send Message" button on rotation details screen
        Dictionary<Int32, String> GetOrganizationUserIDByRotMemberIDs(int tenantId, List<int> lstRotMemberID);
        #region UAT-3463
        Dictionary<Int32, String> GetOrganizationUserDetailsByOrgUserIDs(Int32 tenantId, List<Int32> lstOrgUserIDs);
        #endregion
        #endregion

        #region UAT-2164, Agency User - Granular Permissions
        List<BackgroundDocumentPermissionContract> GetBackgroundDocumentPermissionByReqPkgID(int requirementPkgID, int loggedInAgencyUserID);
        #endregion

        //UAT-2181:Enhance adding tenants to agencies with check boxes on the Manage Agencies screen to select which agencies you would like to add the selected tenant to
        Boolean AssignTenantToAgency(Int32 TenantID, List<Int32> SelectedAgencyIDs, Int32 CurrentLoggedInUserId);

        Boolean IsRotationContainsRotationPkg(Int32 rotationId);

        DataSet GetRequirementLiveData(Int32 requirementPackageSubscriptionID);

        #region UAT-2452
        void CopySharedInvForNewlyMappedAgencyForAgencyUser(Int32 AgencyUserId, String AgencyIds, Int32 TenantId);
        #endregion

        #region UAT-2414, Create a snapshot on Rotation End Date

        List<RequirmentPkgSubscriptionDataContract> GetRequirementSubscriptionDataForSnapshot(Int32 chunkSize);

        Boolean SaveRequirementSnapshotOnRotationEnd(Int32 reqPackageSubscrptionId, Int32 currentUserId, String profileSharingInvitationIDs);

        #endregion

        List<InvitationDocumentContract> GetPassportReportDataForRotation(List<Int32> invitationIDs);

        //UAT-2538
        Int32 GetAgencyIDsForRotinvAppRejNotification(Int32 RotationID);

        // List<OrganizationUser> GetClinicalRotationMemberData(Int32 ClinicalRotationID);
        ClinicalRotation GetClinicalRotation(Int32 ClinicalRotationID);

        #region UAT-2639:Agency hierarchy mapping: Default Hierarchy for Client Admin
        DeptProgramMapping GetClientAdminRootNode(Int32 tenantId);
        #endregion

        #region UAT-2640:Update Agency User (People and Places > Manage Agencies) Agency multi select dropdown will be removed  and multiple hierarchy selection option will be provided here.
        Boolean SaveAgencyHirInstNodeMappingForClientAdmin(AgencyHierarchyContract agencyHierarchyContract, Int32 currentLoggedInUserID, Int32 agencyID, Int32 agencyInstitutionID, Boolean isAdminLoggedIn);
        #endregion

        #region UAT-2511:
        SharedUserRotationReview GetShareduserRotationReviewStatusByRotationIds(Int32 rotationID, Int32 inviteeOrgUserID, Int32 agencyID);
        #endregion
        #region UAT 2548
        List<AgencyApplicantShareHistoryContract> GetApplicantProfileSharingHistory(AgencyApplicantShareHistoryContract agencyApplicantShareHistoryContract, CustomPagingArgsContract customPagingArgsContract);
        List<AgencyApplicantStatusContract> GetAgencyApplicantStatus(AgencyApplicantStatusContract agencyApplicantStatusContract, CustomPagingArgsContract customPagingArgsContract);
        #endregion

        #region [UAT-2735]
        List<String> FilterApplicantHavingOnlyNonActiveOrExpireOrders(String delimittedCRMIDs);
        #endregion

        #region [UAT-2847]
        List<Int32> GetAgenciesByRotationID(Int32 rotationID);
        #endregion

        DataTable GetUpdatedRotationItems(Int32 agencyID, string rotationIds, DateTime fromDate, DateTime toDate);

        List<ApplicantDocument> GetApplicantDocumentDetailsByDocumentIds(List<Int32> documentIds);

        #region UAT-3254
        String GetSharedSubscriptionsSelectedNodeIds(Int32 profileSharingInvitationId);
        String GetRotationHierarchyIdsByRotationID(Int32 RotationId);
        #endregion

        #region UAT-3805
        DataTable GetItemDocNotificationDataOnCategoryApproval(String categoryIds, Int32 applicantOrgUserID, String approvedCategoryIds
                                                                               , String requestTypeCode, Int32? packageSubscriptionID, Int32? RPS_ID);
        List<Int32> GetApprovedCategorIDs(Int32 subscriptionID, List<Int32> categoryIDs, String requestType, Int32 approvedStatusID, Int32? categoryStatusID_ExceptionallyApproved);

        List<PackageSubscription> GetCompliancePkgSubscriptionData(List<CompliancePackageCategory> lstCompPackageCategories);
        List<RequirementPackageSubscription> GetRequirementPkgSubscriptionData(List<RequirementPackageCategory> lstReqPackageCategories);

        List<RequirementPackageSubscription> GetReqSubscriptionByObjectIDs(List<Guid> lstCategoryCodes);
        

        #endregion
    }
}
