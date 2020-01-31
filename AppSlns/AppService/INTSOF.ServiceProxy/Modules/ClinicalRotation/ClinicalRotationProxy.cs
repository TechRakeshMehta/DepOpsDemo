using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceInterface.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Core;
using INTSOF.Utils;
using INTSOF.Utils.Enums;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System.Data;


namespace INTSOF.ServiceProxy.Modules.ClinicalRotation
{
    public class ClinicalRotationProxy : BaseServiceProxy<IClinicalRotation>
    {
        IClinicalRotation _clinicalRotaionServiceChannel;

        public ClinicalRotationProxy()
            : base(ServiceUrlEnum.ClinicalRotationSvcUrl.GetStringValue())
        {
            _clinicalRotaionServiceChannel = base.ServiceChannel;
        }

        #region Common Methods
        public ServiceResponse<List<TenantDetailContract>> GetTenants(ServiceRequest<Boolean, String> data)
        {
            return _clinicalRotaionServiceChannel.GetTenants(data);
        }

        public ServiceResponse<List<AgencyDetailContract>> GetAllAgencies(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetAllAgencies(data);
        }

        public ServiceResponse<List<AgencyDetailContract>> GetAgencies(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetAgencies(data);
        }

        public ServiceResponse<List<AgencyDetailContract>> GetSharedUserAgencies()
        {
            return _clinicalRotaionServiceChannel.GetSharedUserAgencies();
        }

        public ServiceResponse<List<WeekDayContract>> GetWeekDayList(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetWeekDayList(data);
        }

        public ServiceResponse<Dictionary<Int32, String>> GetDefaultPermissionForClientAdmin(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetDefaultPermissionForClientAdmin(data);
        }

        /// <summary>
        /// Get the UserData from Security database, by OrganizationUserID.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<OrganizationUserContract> GetUserData(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetUserData(data);
        }


        /// <summary>
        /// Method to get all possible data types of rotation fields
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementItemStatusContract>> GetRequirementItemStatusTypes(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementItemStatusTypes(data);
        }

        #endregion

        #region Manage Rotations

        public ServiceResponse<List<ClinicalRotationDetailContract>> GetClinicalRotationQueueData(ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetClinicalRotationQueueData(data);
        }

        public ServiceResponse<RotationsMappedToAgenciesContract> GetRotationsMappedToAgencies(ServiceRequest<Int32, String> data)
        {
            return _clinicalRotaionServiceChannel.GetRotationsMappedToAgencies(data);
        }

        public ServiceResponse<Tuple<Int32, Boolean, Boolean>> SaveUpdateClinicalRotation(ServiceRequest<ClinicalRotationDetailContract, List<CustomAttribteContract>, String> data)
        {
            //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
            return _clinicalRotaionServiceChannel.SaveUpdateClinicalRotation(data);
        }

        public ServiceResponse<Boolean> DeleteClinicalRotation(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.DeleteClinicalRotation(data);
        }

        #region UAT-2545: Rotation Archive Functionality
        public ServiceResponse<Boolean> ArchiveClinicalRotation(ServiceRequest<List<Int32>, Int32> data)
        {
            return _clinicalRotaionServiceChannel.ArchiveClinicalRotation(data);

        }
        #endregion

        #region UAT-3138: Rotation UnArchive Functionality
        public ServiceResponse<Boolean> UnArchiveClinicalRotation(ServiceRequest<List<Int32>, Int32> data)
        {
            return _clinicalRotaionServiceChannel.UnArchiveClinicalRotation(data);

        }
        #endregion

        #endregion

        #region Custom Attributes

        public ServiceResponse<List<CustomAttribteContract>> GetCustomAttributeListMapping(ServiceRequest<Int32, String, Int32?> data)
        {
            return _clinicalRotaionServiceChannel.GetCustomAttributeListMapping(data);
        }
        #endregion

        #region Clinical Rotation Details

        /// <summary>
        /// Gets Applicant Clinical Rotation search data
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <param name="data">ClinicalRotationSearchContract</param>
        /// <param name="data">CustomPagingArgsContract</param>
        /// <returns></returns>
        public ServiceResponse<List<ApplicantDataListContract>> GetApplicantClinicalRotationSearch(ServiceRequest<Int32, ClinicalRotationSearchContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetApplicantClinicalRotationSearch(data);
        }

        /// <summary>
        /// Gets Clinical Rotation by Id
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <returns></returns>
        public ServiceResponse<ClinicalRotationDetailContract> GetClinicalRotationById(ServiceRequest<Int32, Int32?> data)
        {
            return _clinicalRotaionServiceChannel.GetClinicalRotationById(data);
        }

        ///UAT-2040
        /// <summary>
        /// Gets Clinical Rotation by Id
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <returns></returns>
        public ServiceResponse<List<ClinicalRotationDetailContract>> GetClinicalRotationByIds(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.GetClinicalRotationByIds(data);
        }
        /// <summary>
        /// Gets Clinical Rotation Members
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <returns></returns>
        public ServiceResponse<List<RotationMemberDetailContract>> GetClinicalRotationMembers(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetClinicalRotationMembers(data);
        }

        /// <summary>
        /// Remove applicants from clinical rotation
        /// </summary>
        /// <param name="data">ClinicalRotationMemberIDs</param>
        /// <returns></returns>
        public ServiceResponse<Boolean> RemoveApplicantsFromRotation(ServiceRequest<Dictionary<Int32, Boolean>, List<Int32>> data)
        {
            return _clinicalRotaionServiceChannel.RemoveApplicantsFromRotation(data);
        }

        /// <summary>
        /// To add applicants to Clinical Rotation
        /// </summary>
        /// <param name="data">ClinicalRotationID</param>
        /// <param name="data">OrganizationUserIds</param>
        /// <returns></returns>
        public ServiceResponse<Boolean> AddApplicantsToRotation(ServiceRequest<Int32, Dictionary<Int32, Boolean>> data)
        {
            return _clinicalRotaionServiceChannel.AddApplicantsToRotation(data);
        }

        /// <summary>
        /// Method to get all users
        /// </summary>
        /// <param name="data">TenantID</param>
        /// <returns></returns>
        public ServiceResponse<List<UserGroupContract>> GetAllUserGroup(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetAllUserGroup(data);
        }

        /// <summary>
        /// Add requirement package to rotation
        /// </summary>
        /// <param name="data">ClinicalRotationID</param>
        /// <param name="data">RequirementPackageID</param>
        /// <returns></returns>
        public ServiceResponse<Boolean> AddPackageToRotation(ServiceRequest<Int32, Int32, String> data)
        {
            return _clinicalRotaionServiceChannel.AddPackageToRotation(data);
        }

        /// <summary>
        /// Get Clinical Rotation requirement package
        /// </summary>
        /// <param name="data">ClinicalRotationID</param>
        /// <returns></returns>
        public ServiceResponse<ClinicalRotationRequirementPackageContract> GetRotationRequirementPackage(ServiceRequest<Int32, String> data)
        {
            return _clinicalRotaionServiceChannel.GetRotationRequirementPackage(data);
        }

        /// <summary>
        /// Is Clinical Rotation Members exist for clinical rotation
        /// </summary>
        /// <param name="data">tenantId</param>
        /// <param name="data">clinicalRotationID</param>
        /// <returns></returns>
        public ServiceResponse<Boolean> IsClinicalRotationMembersExistForRotation(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.IsClinicalRotationMembersExistForRotation(data);
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="data">UnMaskedSSN</param>
        /// <returns></returns>
        public ServiceResponse<String> GetMaskedSSN(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.GetMaskedSSN(data);
        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="data">UnformattedSSN</param>
        /// <returns></returns>
        public ServiceResponse<String> GetFormattedSSN(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.GetFormattedSSN(data);
        }

        public ServiceResponse<String> GetFormattedPhoneNumber(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.GetFormattedPhoneNumber(data);
        }

        #endregion

        #region Manage Invitations and Rotations for Shared User

        /// <summary>
        /// Get shared user clinical rotations
        /// </summary>
        /// <param name="data">TenantID</param>
        /// <returns></returns>
        public ServiceResponse<List<ClinicalRotationDetailContract>> GetSharedUserClinicalRotations(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetSharedUserClinicalRotations(data);
        }

        /// <summary>
        /// Get shared user clinical rotations
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<List<ClinicalRotationDetailContract>> GetSharedUserClinicalRotationDetails(ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract> data)
        {
            return _clinicalRotaionServiceChannel.GetSharedUserClinicalRotationDetails(data);
        }

        public ServiceResponse<List<ClinicalRotationDetailContract>> GetSharedUserClinicalRotationPackageDetails(ServiceRequest<ClinicalRotationDetailContract> data)
        {
            return _clinicalRotaionServiceChannel.GetSharedUserClinicalRotationPackageDetails(data);
        }

        public ServiceResponse<Boolean> UpdateInvitationExpirationRequested(ServiceRequest<List<ApplicantDataListContract>, Int32?> data)
        {
            return _clinicalRotaionServiceChannel.UpdateInvitationExpirationRequested(data);
        }
         

        //UAT-3425
        public ServiceResponse<Boolean> UpdateInvitationExpirationRequirementShares(ServiceRequest<List<Int32>, Int32?> data)
        {
            return _clinicalRotaionServiceChannel.UpdateInvitationExpirationRequirementShares(data);
        }

        #endregion

        #region Rotation Student Detail for Shared User
        public ServiceResponse<List<ApplicantDataListContract>> GetRotationStudents(ServiceRequest<RotationStudentDetailContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetRotationStudents(data);
        }

        public ServiceResponse<List<RotationMemberSearchDetailContract>> GetInstrctrPreceptrRotationStudents(ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetInstrctrPreceptrRotationStudents(data);
        }

        /// <summary>
        /// Getting Masked DOB
        /// </summary>
        /// <param name="data">UnMaskedDOB</param>
        /// <returns></returns>
        public ServiceResponse<String> GetMaskDOB(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.GetMaskDOB(data);
        }

        #endregion

        #region Requirement Verification Queue

        /// <summary>
        /// Method to get requirement package types
        /// </summary>
        /// <param name="data">tenantID</param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementPackageTypeContract>> GetRequirementPackageTypes(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementPackageTypes(data);
        }

        /// <summary>
        /// Get requirement verification queue search data
        /// </summary>
        /// <param name="data">RequirementVerificationQueueContract</param>
        /// <param name="data">CustomPagingArgsContract</param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementVerificationQueueContract>> GetRequirementVerificationQueueSearch(ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementVerificationQueueSearch(data);
        }

        #endregion

        #region Verifiication Details

        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementVerificationDetailContract>> GetVerificationDetailData(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetVerificationDetailData(data);
        }


        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementVerificationDetailContract>> GetRequirementItemsByCategoryId(ServiceRequest<Int32, List<Int32>, Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementItemsByCategoryId(data);
        }

        /// <summary>
        /// Save/Update the data of the Verification Details screen.
        /// </summary>
        /// <param name="dataToSave"></param>
        /// <returns></returns>
        public ServiceResponse<Dictionary<Int32, String>> SaveVerificationData(ServiceRequest<RequirementVerificationData, Int32> data, ref Boolean isNewPackage)
        {
            return _clinicalRotaionServiceChannel.SaveVerificationData(data, ref isNewPackage);
        }

        #endregion


        #region Requirement package Rule Execution

        /// <summary>
        /// Execute the rules for Verification details screen.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<Boolean> ExecuteRequirementObjectBuisnessRules(ServiceRequest<List<RequirementRuleObject>, Int32> data)
        {
            return _clinicalRotaionServiceChannel.ExecuteRequirementObjectBuisnessRules(data);
        }

        #endregion

        #region Agency Review Queue

        /// <summary>
        /// Get the Agency Review Queue related data
        /// </summary>
        /// <param name="selectedStatusCodes"></param>
        /// <param name="selectedTenantIds"></param>
        /// <param name="sortingFilteringXML"></param>
        /// <returns></returns>
        public ServiceResponse<List<AgencyReviewQueueContract>> GetAgencyQueueData(ServiceRequest<String, String, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetAgencyQueueData(data);
        }

        /// <summary>
        /// Returns the list of the 'lkpAgencySearchStatus'
        /// </summary>
        /// <returns></returns>
        public ServiceResponse<List<AgencySearchStatusContract>> GetAgencySearchStatus()
        {
            return _clinicalRotaionServiceChannel.GetAgencySearchStatus(); ;
        }

        /// <summary>
        /// Returns the list of the 'lkpAgencySearchStatus'
        /// </summary>
        /// <returns></returns>
        public ServiceResponse<Boolean> SetAgencySearchStatus(ServiceRequest<List<Int32>, String> data)
        {
            return _clinicalRotaionServiceChannel.SetAgencySearchStatus(data); ;
        }

        #endregion

        #region UAT-1344:Automated NPI Number association and agency creation

        /// <summary>
        /// Add/Update Agency Data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<List<AgencyDataContract>> SaveUpdateAgencyInBulk(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.SaveUpdateAgencyInBulk(data);
        }

        #endregion


        /// <summary>
        /// To add applicants to Clinical Rotation
        /// </summary>
        /// <param name="data">ClinicalRotationID</param>
        /// <param name="data">OrganizationUserIds</param>
        /// <returns></returns>
        public ServiceResponse<Boolean> CreateRotationSubscriptionForClientContact(ServiceRequest<List<Int32>, Int32> data)
        {
            return _clinicalRotaionServiceChannel.CreateRotationSubscriptionForClientContact(data);
        }

        public ServiceResponse<Boolean> UpdateRotationSubscriptionForClientContact(ServiceRequest<Int32, Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.UpdateRotationSubscriptionForClientContact(data);
        }

        public ServiceResponse<Boolean> IfAnyContactIsMappedToRotation(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.IfAnyContactIsMappedToRotation(data);
        }
        public ServiceResponse<Boolean> IfAnyContactHasEnteredDataForRotation(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.IfAnyContactHasEnteredDataForRotation(data);
        }

        #region UAT-1362:As an Instructor/Preceptor I should be able to enter data for my rotation requirements package
        public ServiceResponse<Int32> GetRequirementSubscriptionIdByClinicalRotID(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementSubscriptionIdByClinicalRotID(data);
        }

        public ServiceResponse<List<Int32>> GetSharedUserTenantIDs(ServiceRequest<List<String>> data)
        {
            return _clinicalRotaionServiceChannel.GetSharedUserTenantIDs(data);
        }
        #endregion

        #region UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range
        public ServiceResponse<List<ApplicantDocumentContract>> GetApplicantDocumentToExport(ServiceRequest<List<RotationMemberSearchDetailContract>> data)
        {
            return _clinicalRotaionServiceChannel.GetApplicantDocumentToExport(data);
        }

        public ServiceResponse<List<RotationMemberSearchDetailContract>> GetRotationMemberSearchData(ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetRotationMemberSearchData(data);
        }

        public ServiceResponse<Int32> GetSubscriptionIdByRotIDAndUserID(ServiceRequest<Int32, Int32, String> data)
        {
            return _clinicalRotaionServiceChannel.GetSubscriptionIdByRotIDAndUserID(data);
        }
        #endregion

        #region UAT 1409 : The Agency User should be able to filter their search by those rotations that are active or completed (expired) AND by pending revieew or reviewed.
        public ServiceResponse<List<SharedUserRotationReviewStatusContract>> GetRotationReviewStatus(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRotationReviewStatus(data);
        }

        public ServiceResponse<Boolean> SaveUpdateRotationReviewStatus(ServiceRequest<List<SharedUserRotationReviewContract>, Int32> data)
        {
            return _clinicalRotaionServiceChannel.SaveUpdateRotationReviewStatus(data);
        }
        #endregion

        /// <summary>
        /// Get Invitation Expiration search data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<List<ProfileSharingInvitationSearchContract>> GetInvitationExpirationSearchData(ServiceRequest<ProfileSharingInvitationSearchContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetInvitationExpirationSearchData(data);
        }

        /// <summary>
        /// Save Update Profile Expiration Criteria
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<Boolean> SaveUpdateProfileExpirationCriteria(ServiceRequest<ProfileSharingInvitationSearchContract, List<Int32>> data)
        {
            return _clinicalRotaionServiceChannel.SaveUpdateProfileExpirationCriteria(data);
        }

        public ServiceResponse<List<AttestationDocumentContract>> GetAttestationDocumentsToExport(ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> ServiceReqData)
        {
            return _clinicalRotaionServiceChannel.GetAttestationDocumentsToExport(ServiceReqData);
        }

        /// <summary>
        /// UAT-1784: Get Granular Permissions for current user 
        /// </summary>
        /// <returns></returns>
        public ServiceResponse<Dictionary<String, String>> GetGranularPermissions()
        {
            return _clinicalRotaionServiceChannel.GetGranularPermissions();
        }

        public ServiceResponse<List<ClinicalRotationDetailContract>> GetStudentRotationSearchDetails(ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract, Dictionary<Int32, string>> data)
        {
            return _clinicalRotaionServiceChannel.GetStudentRotationSearchDetails(data);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data">tenantId</param>
        /// <param name="data">clinicalRotationID</param>
        /// <returns></returns>
        public ServiceResponse<Boolean> GetRequirementPackageStatusByRotationID(ServiceRequest<Int32, Int32, String> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementPackageStatusByRotationID(data);
        }

        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
        public ServiceResponse<List<AgencyDetailContract>> GetInstitutionMappedAgency(ServiceRequest<List<Int32>, String> data)
        {
            return _clinicalRotaionServiceChannel.GetInstitutionMappedAgency(data);
        }
        #endregion


        public ServiceResponse<List<AgencyDetailContract>> GetAgenciesFromAllTenants()
        {
            return _clinicalRotaionServiceChannel.GetAgenciesFromAllTenants();
        }

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        public ServiceResponse<Boolean> UpdateRotAndInvitationReviewStatus(ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> data)
        {
            return _clinicalRotaionServiceChannel.UpdateRotAndInvitationReviewStatus(data);
        }

        public ServiceResponse<List<SharedUserRotationReviewStatusContract>> GetReviewStatusList(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetReviewStatusList(data);
        }
        #endregion


        public ServiceResponse<List<ClinicalRotationDetailContract>> GetAttestationReportsWithoutSignature(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetAttestationReportsWithoutSignature(data);
        }

        #region UAT-1881
        public ServiceResponse<List<AgencyDetailContract>> GetAllAgencyByOrgUser(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetAllAgencyByOrgUser(data);
        }
        #endregion

        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        public ServiceResponse<Boolean> SaveUpdateClinicalRotationAssignments(ServiceRequest<List<Int32>, ClinicalRotationDetailContract, Dictionary<String, String>> data)
        {
            return _clinicalRotaionServiceChannel.SaveUpdateClinicalRotationAssignments(data);
        }

        public ServiceResponse<Boolean> IsDataEnteredForAnyRotation(ServiceRequest<String, String> data)
        {
            return _clinicalRotaionServiceChannel.IsDataEnteredForAnyRotation(data);
        }
        public ServiceResponse<List<InstructorAvailabilityContract>> CheckInstAvailabilityByRotationIds(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.CheckInstAvailabilityByRotationIds(data);
        }

        public ServiceResponse<Boolean> IsPreceptorAssignedForAllRotations(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.IsPreceptorAssignedForAllRotations(data);
        }
        #endregion

        #region UAT-2071, Configuration Rotation and Tracking packages must be fully compliant to share
        public ServiceResponse<List<RotationAndTrackingPkgStatusContract>> GetComplianceStatusOfImmunizationAndRotationPackages(ServiceRequest<int, Dictionary<string, string>, int> data)
        {
            return _clinicalRotaionServiceChannel.GetComplianceStatusOfImmunizationAndRotationPackages(data);
        }
        #endregion

        public ServiceResponse<Dictionary<Int32, String>> IsRequirementPkgCompliantReqd(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.IsRequirementPkgCompliantReqd(data);
        }

        public ServiceResponse<String> GetSharingInfoByInvitationGrpID(ServiceRequest<Int32, Int32> serviceRequest)
        {
            return _clinicalRotaionServiceChannel.GetSharingInfoByInvitationGrpID(serviceRequest);
        }

        public ServiceResponse<Tuple<List<Int32>,String, Dictionary<Boolean, String>>> RotationSubscriptionApproveAllPendingItems(ServiceRequest<Int32, Int32,Boolean> data, ref Int32 affectedItemsCount)
        {
            return _clinicalRotaionServiceChannel.RotationSubscriptionApproveAllPendingItems(data, ref affectedItemsCount);
        }
        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        public ServiceResponse<Dictionary<Int32, Boolean>> GetComplianceRequiredRotCatForPackage(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetComplianceRequiredRotCatForPackage(data);
        }
        #endregion

        #region UAT-3458
        public ServiceResponse<List<RequirementExpiringItemListContract>> GetRequirementItemsAboutToExpire(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementItemsAboutToExpire(data);
        }
        #endregion

        #region UAT 2371
        public ServiceResponse<SystemEntityUserPermission> GetSystemEntityUserPermission(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetSystemEntityUserPermission(data);
        }
        #endregion

        #region UAT-2514
        public ServiceResponse<Dictionary<Boolean, DateTime>> IsRotationEndDateRangeNeedToManage(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.IsRotationEndDateRangeNeedToManage(data);
        }
        #endregion

        #region UAT-2424

        public ServiceResponse<List<ClinicalRotationDetailContract>> GetAllClincialRotations(ServiceRequest<Int32> serviceRequest)
        {
            return _clinicalRotaionServiceChannel.GetAllClinicalRotations(serviceRequest);
        }

        //public ServiceResponse<Int32> GetRotationPackageIDByRotationId(ServiceRequest<Int32, String> serviceRequest)
        //{
        //    return _clinicalRotaionServiceChannel.GetRotationPackageIDByRotationId(serviceRequest);
        //}

        //UAT-3121
        public ServiceResponse<RequirementPackageContract> GetRotationPackageIDByRotationId(ServiceRequest<Int32, String> serviceRequest)
        {
            return _clinicalRotaionServiceChannel.GetRotationPackageIDByRotationId(serviceRequest);
        }


        public ServiceResponse<ClinicalRotationDetailContract> GetClinicalRotationDetailsById(ServiceRequest<Int32> serviceRequest)
        {
            return _clinicalRotaionServiceChannel.GetClinicalRotationDetailsById(serviceRequest);
        }
        #endregion

        #region UAT 2554

        public ServiceResponse<Boolean> IsPreceptorRequiredForAgency(ServiceRequest<Int32> serviceRequest)
        {
            return _clinicalRotaionServiceChannel.IsPreceptorRequiredForAgency(serviceRequest);
        }

        #endregion

        #region UAT-2313

        public ServiceResponse<List<ClinicalRotationDetailContract>> GetClinicalRotationDataFromFlatTable(ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetClinicalRotationDataFromFlatTable(data);
        }
        public ServiceResponse<List<WeekDayContract>> GetWeekDays()
        {
            return _clinicalRotaionServiceChannel.GetWeekDays();
        }
        public ServiceResponse<List<ClientContactContract>> GetClientContacts()
        {
            return _clinicalRotaionServiceChannel.GetAllClientContacts();
        }
        #endregion

        #region UAT-2666
        public ServiceResponse<Boolean> UpdateClinicalRotationByAgency(ServiceRequest<ClinicalRotationDetailContract, Int32, Boolean, Int32?> data)
        {
            return _clinicalRotaionServiceChannel.UpdateClinicalRotationByAgency(data);
        }
        public ServiceResponse<List<RotationFieldUpdatedByAgencyContract>> GetRotationFieldUpdateByAgencyDetails(ServiceRequest<List<Int32>, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRotationFieldUpdateByAgencyDetails(data);
        }
        #endregion

        #region UAT-2510
        public ServiceResponse<Boolean> GetAgencyUserSSN_Permission(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.GetAgencyUserSSN_Permission(data);
        }
        #endregion

        #region UAT-2513: [Batch Rotation Upload]
        public ServiceResponse<Boolean> SaveBatchRotationUploadDetails(ServiceRequest<List<BatchRotationUploadContract>, String, Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.SaveBatchRotationUploadDetails(data);
        }
        public ServiceResponse<List<BatchRotationUploadContract>> GetBatchRotationList(ServiceRequest<Int32, BatchRotationUploadContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetBatchRotationList(data);
        }

        #endregion

        #region UAT-2926 : Batch Rotation creation update to have template downoad with agencies and institutions pre-filled in based on what is selected on the screen where the template is downloaded.
        public ServiceResponse<List<String>> GetAgencyHierarchyAgencyList(ServiceRequest<List<Tuple<Int32, Int32>>> data)
        {
            return _clinicalRotaionServiceChannel.GetAgencyHierarchyAgencyList(data);
        }
        #endregion

        public ServiceResponse<List<String>> FilterApplicantHavingOnlyNonActiveOrExpireOrders(ServiceRequest<int, string> data)
        {
            return _clinicalRotaionServiceChannel.FilterApplicantHavingOnlyNonActiveOrExpireOrders(data);
        }


        public ServiceResponse<List<RequirementPackageContract>> GetRequirementPackage(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementPackage(data);
        }

        public ServiceResponse<List<RequirementCategoryContract>> GetRequirementCategory(ServiceRequest<Int32, List<Int32>> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementCategory(data);
        }

        public ServiceResponse<List<RequirementItemContract>> GetRequirementItem(ServiceRequest<Int32, List<Int32>> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementItem(data);
        }

        public ServiceResponse<List<ApplicantRequirementDataAuditContract>> GetApplicantRequirementDataAudit(ServiceRequest<Int32, ApplicantRequirementDataAuditSearchContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetApplicantRequirementDataAudit(data);
        }

        #region Requirement Verification Assignment Queue AND User Work Queue

        /// <summary>
        /// UAT 2975
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementVerificationQueueContract>> GetAssignmentRotationVerificationQueueData(ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetAssignmentRotationVerificationQueueData(data);
        }

        public ServiceResponse<List<ReqPkgSubscriptionIDList>> GetReqPkgSubscriptionIdList(ServiceRequest<RequirementVerificationQueueContract, Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetReqPkgSubscriptionIdList(data);
        }

        public ServiceResponse<List<ReqPkgSubscriptionIDList>> GetReqPkgSubscriptionIdListForRotationVerification(ServiceRequest<RequirementVerificationQueueContract, Int32, Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetReqPkgSubscriptionIdListForRotationVerification(data);
        }
        /// <summary>
        /// UAT 2975
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse<Boolean> AssignItemsToUser(ServiceRequest<List<Int32>, Int32, String> data)
        {
            return _clinicalRotaionServiceChannel.AssignItemsToUser(data);
        }

        /// <summary>
        /// Method to get requirement package types
        /// </summary>
        /// <param name="data">tenantID</param>
        /// <returns></returns>
        public ServiceResponse<List<RequirementPackageTypeContract>> GetSharedRequirementPackageTypes()
        {
            return _clinicalRotaionServiceChannel.GetSharedRequirementPackageTypes();
        }

        #endregion

        //UAT 3164
        public ServiceResponse<DataTable> PerformRotationLiveDataMovement(ServiceRequest<Int32, Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.PerformRotationLiveDataMovement(data);
        }

        //UAT 3164
        public ServiceResponse<DataTable> GetTargetReqPackageSubscriptionIDsForSync(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetTargetReqPackageSubscriptionIDsForSync(data);
        }

        #region UAT-3197, As an Agency User, I should be able to retrieve the syllabus
        public ServiceResponse<List<ClientContactSyllabusDocumentContract>> GetClinicalRotationSyllabusDocumentsByID(ServiceRequest<Int32, Int32> serviceRequest)
        {
            return _clinicalRotaionServiceChannel.GetClinicalRotationSyllabusDocumentsByID(serviceRequest);
        }
        #endregion

        #region UAT-3176
        public ServiceResponse<List<RequirementAttributeGroupContract>> GetAllRotationAttributeGroup(ServiceRequest<Int32, String, String> data)
        {
            return _clinicalRotaionServiceChannel.GetAllRotationAttributeGroup(data);
        }

        public ServiceResponse<Boolean> SaveUpdateRotationAttributeGroup(ServiceRequest<RequirementAttributeGroupContract, Int32, Boolean> data)
        {
            return _clinicalRotaionServiceChannel.SaveUpdateRotationAttributeGroup(data);
        }

        public ServiceResponse<RequirementAttributeGroupContract> GetAttributeGroupById(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetAttributeGroupById(data);
        }

        public ServiceResponse<Boolean> IsAttributeGroupMapped(ServiceRequest<Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.IsAttributeGroupMapped(data);
        }
        #endregion


        public ServiceResponse<List<ApplicantDataListContract>> GetRotationMembersForRotationDocs(ServiceRequest<ClinicalRotationSearchContract, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetRotationMembersForRotationDocs(data);
        }

        public ServiceResponse<List<RequirementCategoryContract>> GetReqPkgCatByRotationID(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetReqPkgCatByRotationID(data);
        }

        public ServiceResponse<List<RotationDocumentContact>> GetApplicantDocsByReqCatID(ServiceRequest<String, String, CustomPagingArgsContract> data)
        {
            return _clinicalRotaionServiceChannel.GetApplicantDocsByReqCatID(data);
        }

        public ServiceResponse<List<RotationDocumentContact>> GetApplicantDocumentsByDocIDs(ServiceRequest<String, String> data)
        {
            return _clinicalRotaionServiceChannel.GetApplicantDocumentsByDocIDs(data);
        }

        #region UAT-3220
        public ServiceResponse<Boolean> HideRequirementSharesDetailLink(ServiceRequest<Guid> data)
        {
            return _clinicalRotaionServiceChannel.HideRequirementSharesDetailLink(data);
        }
        #endregion

        #region UAT-3241
        public ServiceResponse<List<String>> GetAgencyNamesByIds(ServiceRequest<Int32, List<Int32>> data)
        {
            return _clinicalRotaionServiceChannel.GetAgencyNamesByIds(data);
        }

        #endregion
        /// <summary>
        /// Gets Profile Share Detail from applicant by Id
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <returns></returns>
        public ServiceResponse<ProfileSharingInvitationDetailsContract> GetProfileShareDetailsById(ServiceRequest<Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetProfileShareDetailsById(data);
        }

        public ServiceResponse<Boolean> UpdateProfileShareInvDetails(ServiceRequest<ProfileSharingInvitationDetailsContract, Int32> data)
        {
            return _clinicalRotaionServiceChannel.UpdateProfileShareInvDetails(data);
        }
        #region UAT-3315
        public ServiceResponse<List<ApplicantDocumentContract>> GetSelectedBadgeDocumentsToExport(ServiceRequest<String, Int32, Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetSelectedBadgeDocumentsToExport(data);
        }
        #endregion

        #region UAt-3316
        public ServiceResponse<String> GetSharedUserTemplatePermissions(ServiceRequest<Int32, Boolean> data)
        {
            return _clinicalRotaionServiceChannel.GetSharedUserTemplatePermissions(data);
        }
        #endregion

        #region UAT-3470
        public ServiceResponse<Boolean> SaveUpdateInvitationArchiveState(ServiceRequest<List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> data)
        {
            return _clinicalRotaionServiceChannel.SaveUpdateInvitationArchiveState(data);
        }
        #endregion


        public ServiceResponse<List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract>> GetSharedUserAgencyHierarchyRootNodes()
        {
            return _clinicalRotaionServiceChannel.GetSharedUserAgencyHierarchyRootNodes();
        }

        #region UAT-3977
        public ServiceResponse<Dictionary<Int32, String>> InstructorPreceptorRequiredPkgCompliantReqd(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.InstructorPreceptorRequiredPkgCompliantReqd(data);
        }

        #endregion

        #region UAT-3957
        public ServiceResponse<List<RequirementItemRejectionContract>> GetRequirementRejectedItemDetailsForMail(ServiceRequest<String,Int32> data)
        {
            return _clinicalRotaionServiceChannel.GetRequirementRejectedItemDetailsForMail(data);
        }


        #endregion

        public ServiceResponse<List<ClinicalRotationDetailContract>> GetAllClinicalRotationsForLoggedInUser(ServiceRequest<Int32, Int32, Boolean> data)
        {
            return _clinicalRotaionServiceChannel.GetAllClinicalRotationsForLoggedInUser(data);
        }

        public ServiceResponse<Boolean> IsAgenycHierarchyAvailable(ServiceRequest<String> data)
        {
            return _clinicalRotaionServiceChannel.IsAgenycHierarchyAvailable(data);
        }
    }
}
