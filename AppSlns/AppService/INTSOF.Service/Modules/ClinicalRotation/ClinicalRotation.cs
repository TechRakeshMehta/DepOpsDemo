using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Business.RepoManagers;
using INTSOF.Service.Core;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceInterface.Modules.ClinicalRotation;
using INTSOF.Utils;
using NLog;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.Xml.Linq;
using CoreWeb.IntsofSecurityModel;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
namespace INTSOF.Service.Modules.ClinicalRotation
{
    public class ClinicalRotation : BaseService, IClinicalRotation
    {
        private static NLog.Logger logger;

        #region Common Methods
        ServiceResponse<List<TenantDetailContract>> IClinicalRotation.GetTenants(ServiceRequest<Boolean, String> data)
        {
            ServiceResponse<List<TenantDetailContract>> commonResponse = new ServiceResponse<List<TenantDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetTenants(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<AgencyDetailContract>> IClinicalRotation.GetAllAgencies(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<AgencyDetailContract>> commonResponse = new ServiceResponse<List<AgencyDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetAllAgency(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<AgencyDetailContract>> IClinicalRotation.GetAgencies(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<AgencyDetailContract>> commonResponse = new ServiceResponse<List<AgencyDetailContract>>();
            try
            {
                commonResponse.Result = ProfileSharingManager.GetAllAgencies(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<AgencyDetailContract>> IClinicalRotation.GetSharedUserAgencies()
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<AgencyDetailContract>> commonResponse = new ServiceResponse<List<AgencyDetailContract>>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetSharedUserAgencies(activeUser.UserID);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<WeekDayContract>> IClinicalRotation.GetWeekDayList(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<WeekDayContract>> commonResponse = new ServiceResponse<List<WeekDayContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetWeekDayList(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Dictionary<Int32, String>> IClinicalRotation.GetDefaultPermissionForClientAdmin(ServiceRequest<Int32> data)
        {
            ServiceResponse<Dictionary<Int32, String>> commonResponse = new ServiceResponse<Dictionary<Int32, String>>();
            try
            {
                //Call Business Manager Method 
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClinicalRotationManager.GetDefaultPermissionForClientAdmin(data.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get the UserData from Security database, by OrganizationUserID.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<OrganizationUserContract> IClinicalRotation.GetUserData(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<OrganizationUserContract> commonResponse = new ServiceResponse<OrganizationUserContract>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetUserData(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Method to get all possible data types of rotation fields
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<List<RequirementItemStatusContract>> IClinicalRotation.GetRequirementItemStatusTypes(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<RequirementItemStatusContract>> commonResponse = new ServiceResponse<List<RequirementItemStatusContract>>();
            try
            {
                commonResponse.Result = RequirementVerificationManager.GetRequirementItemStatusTypes(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        #endregion

        #region Mange Rotations
        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetClinicalRotationQueueData(ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetClinicalRotationQueueData(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RotationsMappedToAgenciesContract> IClinicalRotation.GetRotationsMappedToAgencies(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<RotationsMappedToAgenciesContract> commonResponse = new ServiceResponse<RotationsMappedToAgenciesContract>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetRotationsMappedToAgencies(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Tuple<Int32, Boolean, Boolean>> IClinicalRotation.SaveUpdateClinicalRotation(ServiceRequest<ClinicalRotationDetailContract, List<CustomAttribteContract>, String> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Tuple<Int32, Boolean, Boolean>> commonResponse = new ServiceResponse<Tuple<Int32, Boolean, Boolean>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.SaveUpdateClinicalRotationData(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.DeleteClinicalRotation(ServiceRequest<Int32, Int32> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.DeleteClinicalRotation(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.ArchiveClinicalRotation(ServiceRequest<List<Int32>, Int32> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.ArchiveClinicalRotation(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        #region UAT-3138
        ServiceResponse<Boolean> IClinicalRotation.UnArchiveClinicalRotation(ServiceRequest<List<Int32>, Int32> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.UnArchiveClinicalRotation(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        #endregion

        #endregion

        #region Custom Attributes

        ServiceResponse<List<CustomAttribteContract>> IClinicalRotation.GetCustomAttributeListMapping(ServiceRequest<Int32, String, Int32?> data)
        {
            ServiceResponse<List<CustomAttribteContract>> commonResponse = new ServiceResponse<List<CustomAttribteContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetCustomAttributeMappingList(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
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
        ServiceResponse<List<ApplicantDataListContract>> IClinicalRotation.GetApplicantClinicalRotationSearch(ServiceRequest<Int32, ClinicalRotationSearchContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<ApplicantDataListContract>> commonResponse = new ServiceResponse<List<ApplicantDataListContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetApplicantClinicalRotationSearch(data.SelectedTenantId, data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets Clinical Rotation by Id
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <returns></returns>
        ServiceResponse<ClinicalRotationDetailContract> IClinicalRotation.GetClinicalRotationById(ServiceRequest<Int32, Int32?> data)
        {
            ServiceResponse<ClinicalRotationDetailContract> commonResponse = new ServiceResponse<ClinicalRotationDetailContract>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetClinicalRotationById(data.SelectedTenantId, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ///UAT-2040
        /// <summary>
        /// Gets Clinical Rotation by Id
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <returns></returns>
        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetClinicalRotationByIds(ServiceRequest<String> data)
        {
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetClinicalRotationByIds(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets Clinical Rotation Members
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <returns></returns>
        ServiceResponse<List<RotationMemberDetailContract>> IClinicalRotation.GetClinicalRotationMembers(ServiceRequest<Int32, Int32> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<RotationMemberDetailContract>> commonResponse = new ServiceResponse<List<RotationMemberDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetClinicalRotationMembers(data.SelectedTenantId, data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Remove applicants from clinical rotation
        /// </summary>
        /// <param name="data">ClinicalRotationMemberIds</param>
        /// <returns></returns>
        ServiceResponse<Boolean> IClinicalRotation.RemoveApplicantsFromRotation(ServiceRequest<Dictionary<Int32, Boolean>, List<Int32>> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.RemoveApplicantsFromRotation(data.SelectedTenantId, data.Parameter1, activeUser.OrganizationUserId, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// To add applicants to Clinical Rotation
        /// </summary>
        /// <param name="data">ClinicalRotationID</param>
        /// <param name="data">OrganizationUserIds</param>
        /// <returns></returns>
        ServiceResponse<Boolean> IClinicalRotation.AddApplicantsToRotation(ServiceRequest<Int32, Dictionary<Int32, Boolean>> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.AddApplicantsToRotation(data.SelectedTenantId, data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// To get user groups
        /// </summary>
        /// <param name="data">TenantID</param>
        /// <returns></returns>
        ServiceResponse<List<UserGroupContract>> IClinicalRotation.GetAllUserGroup(ServiceRequest<Int32> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<UserGroupContract>> commonResponse = new ServiceResponse<List<UserGroupContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetAllUserGroup(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Add requirement package to rotation
        /// </summary>
        /// <param name="data">ClinicalRotationID</param>
        /// <param name="data">RequirementPackageID</param>
        /// <returns></returns>
        ServiceResponse<Boolean> IClinicalRotation.AddPackageToRotation(ServiceRequest<Int32, Int32, String> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.AddPackageToRotation(data.SelectedTenantId, data.Parameter1, data.Parameter2, data.Parameter3, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get Clinical Rotation requirement package
        /// </summary>
        /// <param name="data">ClinicalRotationID</param>
        /// <returns></returns>
        ServiceResponse<ClinicalRotationRequirementPackageContract> IClinicalRotation.GetRotationRequirementPackage(ServiceRequest<Int32, String> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<ClinicalRotationRequirementPackageContract> commonResponse = new ServiceResponse<ClinicalRotationRequirementPackageContract>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetRotationRequirementPackage(data.SelectedTenantId, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Is Clinical Rotation Members exist for clinical rotation
        /// </summary>
        /// <param name="data">tenantId</param>
        /// <param name="data">clinicalRotationID</param>
        /// <returns></returns>
        ServiceResponse<Boolean> IClinicalRotation.IsClinicalRotationMembersExistForRotation(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.IsClinicalRotationMembersExistForRotation(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="data">UnMaskedSSN</param>
        /// <returns></returns>
        ServiceResponse<String> IClinicalRotation.GetMaskedSSN(ServiceRequest<String> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = ApplicationDataManager.GetMaskedSSN(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="data">UnformattedSSN</param>
        /// <returns></returns>
        ServiceResponse<String> IClinicalRotation.GetFormattedSSN(ServiceRequest<String> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = ApplicationDataManager.GetFormattedSSN(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Getting Formatted PhoneNumber
        /// </summary>
        /// <param name="data">UnformattedSSN</param>
        /// <returns></returns>
        ServiceResponse<String> IClinicalRotation.GetFormattedPhoneNumber(ServiceRequest<String> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = ApplicationDataManager.GetFormattedPhoneNumber(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region Manage Invitations and Rotations for Shared User

        /// <summary>
        /// Get shared user clinical rotations
        /// </summary>
        /// <param name="data">TenantID</param>
        /// <returns></returns>
        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetSharedUserClinicalRotations(ServiceRequest<Int32> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetSharedUserClinicalRotations(data.SelectedTenantId, activeUser.OrganizationUserId, activeUser.UserID);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get shared user clinical rotations
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetSharedUserClinicalRotationDetails(ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetSharedUserClinicalRotationDetails(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId, activeUser.UserID);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get shared user clinical rotation packages
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetSharedUserClinicalRotationPackageDetails(ServiceRequest<ClinicalRotationDetailContract> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetSharedUserClinicalRotationPackageDetails(data.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Save/Update the PSI_IsExpirationRequested of the Profile Sharing Invitations
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<Boolean> IClinicalRotation.UpdateInvitationExpirationRequested(ServiceRequest<List<ApplicantDataListContract>, Int32?> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                Int32 orgUserID;

                if (!data.Parameter2.IsNull())
                    orgUserID = data.Parameter2.Value;
                else
                {
                    UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                    orgUserID = activeUser.OrganizationUserId;
                }
                //UAT-2511
                List<AgencyUserAuditHistoryDataContract> lstAgencyUserAuditHistory = new List<AgencyUserAuditHistoryDataContract>();
                List<Int32> profileSharingInvIDs = data.Parameter1.Select(x => x.ProfileSharingInvID).ToList();
                lstAgencyUserAuditHistory = ProfileSharingManager.GenerateAuditHistoryDataForRerquestForAudit(profileSharingInvIDs, orgUserID);
                commonResponse.Result = SharedUserClinicalRotationManager.UpdateInvitationExpirationRequested(data.Parameter1, orgUserID);
                //UAT-2511
                if (commonResponse.Result)
                {
                    ProfileSharingManager.SaveAgencyUserAuditHistory(lstAgencyUserAuditHistory, true);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        //UAT-3425
        ServiceResponse<Boolean> IClinicalRotation.UpdateInvitationExpirationRequirementShares(ServiceRequest<List<Int32>, Int32?> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                Int32 orgUserID;

                if (!data.Parameter2.IsNull())
                    orgUserID = data.Parameter2.Value;
                else
                {
                    UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                    orgUserID = activeUser.OrganizationUserId;
                }
                //UAT-2511
                List<AgencyUserAuditHistoryDataContract> lstAgencyUserAuditHistory = new List<AgencyUserAuditHistoryDataContract>();
                //List<Int32> profileSharingInvIDs = data.Parameter1.Select(x => x.ProfileSharingInvID).ToList();
                lstAgencyUserAuditHistory = ProfileSharingManager.GenerateAuditHistoryDataForRerquestForAudit(data.Parameter1, orgUserID);
                //lstAgencyUserAuditHistory = ProfileSharingManager.GenerateAuditHistoryDataForRerquestForAudit(profileSharingInvIDs, orgUserID);
                commonResponse.Result = SharedUserClinicalRotationManager.UpdateInvitationExpirationRequirementShares(data.Parameter1, orgUserID);
                //UAT-2511
                if (commonResponse.Result)
                {
                    ProfileSharingManager.SaveAgencyUserAuditHistory(lstAgencyUserAuditHistory, true);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        #endregion

        #region Rotation Student Detail for Shared User

        ServiceResponse<List<ApplicantDataListContract>> IClinicalRotation.GetRotationStudents(ServiceRequest<RotationStudentDetailContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<ApplicantDataListContract>> commonResponse = new ServiceResponse<List<ApplicantDataListContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetRotationStudents(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get Rotation Student details for Instructor Preceptor
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<List<RotationMemberSearchDetailContract>> IClinicalRotation.GetInstrctrPreceptrRotationStudents(ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<RotationMemberSearchDetailContract>> commonResponse = new ServiceResponse<List<RotationMemberSearchDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetInstrctrPreceptrRotationStudents(data.Parameter1.lstTenantIDs, activeUser.UserID, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Getting Masked DOB
        /// </summary>
        /// <param name="data">UnMaskedDOB</param>
        /// <returns></returns>
        ServiceResponse<String> IClinicalRotation.GetMaskDOB(ServiceRequest<String> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = ApplicationDataManager.GetMaskDOB(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region Requirement Verification Queue

        /// <summary>
        /// Method to get requirement package types
        /// </summary>
        /// <param name="data">tenantID</param>
        /// <returns></returns>
        ServiceResponse<List<RequirementPackageTypeContract>> IClinicalRotation.GetRequirementPackageTypes(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<RequirementPackageTypeContract>> commonResponse = new ServiceResponse<List<RequirementPackageTypeContract>>();
            try
            {
                commonResponse.Result = RequirementVerificationManager.GetRequirementPackageTypes(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get requirement verification queue search data
        /// </summary>
        /// <param name="data">RequirementVerificationQueueContract</param>
        /// <param name="data">CustomPagingArgsContract</param>
        /// <returns></returns>
        ServiceResponse<List<RequirementVerificationQueueContract>> IClinicalRotation.GetRequirementVerificationQueueSearch(ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<RequirementVerificationQueueContract>> commonResponse = new ServiceResponse<List<RequirementVerificationQueueContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = RequirementVerificationManager.GetRequirementVerificationQueueSearch(data.SelectedTenantId, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }


        #endregion

        #region Verification Details

        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<List<RequirementVerificationDetailContract>> IClinicalRotation.GetVerificationDetailData(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<List<RequirementVerificationDetailContract>> commonResponse = new ServiceResponse<List<RequirementVerificationDetailContract>>();
            try
            {
                commonResponse.Result = RequirementVerificationManager.GetVerificationDetailData(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<List<RequirementVerificationDetailContract>> IClinicalRotation.GetRequirementItemsByCategoryId(ServiceRequest<Int32, List<Int32>, Int32, Int32> data)
        {
            ServiceResponse<List<RequirementVerificationDetailContract>> commonResponse = new ServiceResponse<List<RequirementVerificationDetailContract>>();
            try
            {
                commonResponse.Result = RequirementVerificationManager.GetRequirementItemsByCategoryId(data.Parameter1, data.Parameter2, data.Parameter3, data.Parameter4);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Save/Update the data of the Verification Details screen.
        /// </summary>
        /// <param name="dataToSave"></param>
        /// <returns></returns>
        ServiceResponse<Dictionary<Int32, String>> IClinicalRotation.SaveVerificationData(ServiceRequest<RequirementVerificationData, Int32> data, ref Boolean isNewPackage)
        {
            ServiceResponse<Dictionary<Int32, String>> commonResponse = new ServiceResponse<Dictionary<Int32, String>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = RequirementVerificationManager.SaveVerificationData(data.Parameter1, activeUser.OrganizationUserId, data.Parameter2, ref isNewPackage);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region Requirement package Rule Execution

        ServiceResponse<Boolean> IClinicalRotation.ExecuteRequirementObjectBuisnessRules(ServiceRequest<List<RequirementRuleObject>, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                RequirementRuleManager.ExecuteRequirementObjectBuisnessRules(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId, data.SelectedTenantId);
                commonResponse.Result = true;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
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
        ServiceResponse<List<AgencyReviewQueueContract>> IClinicalRotation.GetAgencyQueueData(ServiceRequest<String, String, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<AgencyReviewQueueContract>> commonResponse = new ServiceResponse<List<AgencyReviewQueueContract>>();
            try
            {
                commonResponse.Result = AgencyReviewManager.GetAgencyQueueData(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Returns the list of the 'lkpAgencySearchStatus'
        /// </summary>
        /// <returns></returns>
        ServiceResponse<List<AgencySearchStatusContract>> IClinicalRotation.GetAgencySearchStatus()
        {
            ServiceResponse<List<AgencySearchStatusContract>> commonResponse = new ServiceResponse<List<AgencySearchStatusContract>>();
            try
            {
                commonResponse.Result = AgencyReviewManager.GetAgencySearchStatus();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }


        /// <summary>
        /// Set Agency status to reviwed or available, based on the StatusCode
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<Boolean> IClinicalRotation.SetAgencySearchStatus(ServiceRequest<List<Int32>, String> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyReviewManager.SetAgencySearchStatus(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-1344:Automated NPI Number association and agency creation

        ServiceResponse<List<AgencyDataContract>> IClinicalRotation.SaveUpdateAgencyInBulk(ServiceRequest<String> data)
        {
            ServiceResponse<List<AgencyDataContract>> commonResponse = new ServiceResponse<List<AgencyDataContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClinicalRotationManager.SaveUpdateAgencyInBulk(data.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<Boolean> IClinicalRotation.CreateRotationSubscriptionForClientContact(ServiceRequest<List<Int32>, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClinicalRotationManager.CreateRotationSubscriptionForClientContact(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.UpdateRotationSubscriptionForClientContact(ServiceRequest<Int32, Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClinicalRotationManager.UpdateRotationSubscriptionForClientContact(data.Parameter1, data.Parameter2, data.Parameter3, activeUser.OrganizationUserId, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.IfAnyContactIsMappedToRotation(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.IfAnyContactIsMappedToRotation(data.Parameter, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.IfAnyContactHasEnteredDataForRotation(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.IfAnyContactHasEnteredDataForRotation(data.Parameter1, data.Parameter2, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-1362:As an Instructor/Preceptor I should be able to enter data for my rotation requirements package

        ServiceResponse<Int32> IClinicalRotation.GetRequirementSubscriptionIdByClinicalRotID(ServiceRequest<Int32> data)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClinicalRotationManager.GetRequirementSubscriptionIdByClinicalRotID(data.SelectedTenantId, data.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<Int32>> IClinicalRotation.GetSharedUserTenantIDs(ServiceRequest<List<String>> data)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = SharedUserClinicalRotationManager.GetSharedUserTenantIDs(activeUser.UserID, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range
        ServiceResponse<List<ApplicantDocumentContract>> IClinicalRotation.GetApplicantDocumentToExport(ServiceRequest<List<RotationMemberSearchDetailContract>> data)
        {
            ServiceResponse<List<ApplicantDocumentContract>> commonResponse = new ServiceResponse<List<ApplicantDocumentContract>>();
            try
            {

                commonResponse.Result = ClinicalRotationManager.GetApplicantDocumentToExport(data.Parameter, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RotationMemberSearchDetailContract>> IClinicalRotation.GetRotationMemberSearchData(ServiceRequest<RotationMemberSearchDetailContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<RotationMemberSearchDetailContract>> commonResponse = new ServiceResponse<List<RotationMemberSearchDetailContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetRotationMemberSearchData(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Int32> IClinicalRotation.GetSubscriptionIdByRotIDAndUserID(ServiceRequest<Int32, Int32, String> data)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetSubscriptionIdByRotIDAndUserID(data.SelectedTenantId, data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT 1409 : The Agency User should be able to filter their search by those rotations that are active or completed (expired) AND by pending revieew or reviewed.
        ServiceResponse<List<SharedUserRotationReviewStatusContract>> IClinicalRotation.GetRotationReviewStatus(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<SharedUserRotationReviewStatusContract>> commonResponse = new ServiceResponse<List<SharedUserRotationReviewStatusContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetRotationReviewStatus(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.SaveUpdateRotationReviewStatus(ServiceRequest<List<SharedUserRotationReviewContract>, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClinicalRotationManager.SaveUpdateRotationReviewStatus(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Get Invitation Expiration search data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<List<ProfileSharingInvitationSearchContract>> IClinicalRotation.GetInvitationExpirationSearchData(ServiceRequest<ProfileSharingInvitationSearchContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<ProfileSharingInvitationSearchContract>> commonResponse = new ServiceResponse<List<ProfileSharingInvitationSearchContract>>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetInvitationExpirationSearchData(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Save Update Profile Expiration Criteria
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<Boolean> IClinicalRotation.SaveUpdateProfileExpirationCriteria(ServiceRequest<ProfileSharingInvitationSearchContract, List<Int32>> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = SharedUserClinicalRotationManager.SaveUpdateProfileExpirationCriteria(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        /// <summary>
        /// Optional Parameters (RotationID OR ProfileSharingInvitationGroupID OR ProfileSharingInvitationID)
        /// </summary>
        /// <param name="ServiceReqData"></param>
        /// <returns></returns>
        ServiceResponse<List<AttestationDocumentContract>> IClinicalRotation.GetAttestationDocumentsToExport(ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> ServiceReqData)
        {
            ServiceResponse<List<AttestationDocumentContract>> commonResponse = new ServiceResponse<List<AttestationDocumentContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClinicalRotationManager.GetAttestationDocumentsToExport(ServiceReqData, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// UAT-1784: Get Granular Permissions for current user
        /// </summary>
        /// <returns></returns>
        ServiceResponse<Dictionary<String, String>> IClinicalRotation.GetGranularPermissions()
        {
            ServiceResponse<Dictionary<String, String>> commonResponse = new ServiceResponse<Dictionary<String, String>>();
            try
            {
                Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                SecurityManager.GetUserGranularPermission(activeUser.OrganizationUserId, out dicPermissions);
                commonResponse.Result = dicPermissions;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetStudentRotationSearchDetails(ServiceRequest<List<TenantDetailContract>, ClinicalRotationDetailContract, Dictionary<Int32, string>> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetStudentRotationSearchDetails(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId, activeUser.UserID, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        ServiceResponse<bool> IClinicalRotation.GetRequirementPackageStatusByRotationID(ServiceRequest<int, int, String> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetRequirementPackageStatusByRotationID(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
        ServiceResponse<List<AgencyDetailContract>> IClinicalRotation.GetInstitutionMappedAgency(ServiceRequest<List<Int32>, String> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<AgencyDetailContract>> commonResponse = new ServiceResponse<List<AgencyDetailContract>>();
            try
            {
                commonResponse.Result = ProfileSharingManager.GetInstitutionMappedAgency(data.Parameter1, data.Parameter2, false, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<List<AgencyDetailContract>> IClinicalRotation.GetAgenciesFromAllTenants()
        {
            ServiceResponse<List<AgencyDetailContract>> commonResponse = new ServiceResponse<List<AgencyDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetAgenciesFromAllTenants();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        ServiceResponse<Boolean> IClinicalRotation.UpdateRotAndInvitationReviewStatus(ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            int orgUserID;
            if (data.Parameter1.ContainsKey(AppConsts.CURRENT_USER_ID_QUERY_STRING))
                data.Parameter1.TryGetValue(AppConsts.CURRENT_USER_ID_QUERY_STRING, out orgUserID);
            else
            {
                orgUserID = activeUser.OrganizationUserId;
            }

            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                String reviewStatusCode = String.Empty;
                String notes = String.Empty;
                String ScreenType = String.Empty;
                data.Parameter1.TryGetValue(AppConsts.DIC_KEY_NOTES, out notes);
                data.Parameter1.TryGetValue(AppConsts.DIC_KEY_REVIEW_STATUS, out reviewStatusCode);
                data.Parameter1.TryGetValue(AppConsts.DIC_KEY_SCREEN_TYPE, out ScreenType);

                Int32 reviewStatusID = ProfileSharingManager.GetInvitationReviewStatusIDByStatusCode(reviewStatusCode);

                //UAT-2463
                bool allInvitationsToBeUpdated = false;
                if (ScreenType == AppConsts.REQUIREMENT_SHARES_SCREEN_NAME)
                {
                    allInvitationsToBeUpdated = true;
                }
                //data.Parameter2 -- Invitation Ids
                //data.Parameter3 -- Rotation Ids & corresponding Invitation Ids
                Boolean isAdminLoggedInAsAgencyUser = false;
                if (orgUserID != activeUser.OrganizationUserId)
                {
                    isAdminLoggedInAsAgencyUser = true;
                }
                if (!data.Parameter2.IsNullOrEmpty())
                {

                    List<Int32> lstAlreadyApprovedInvitations = new List<int>();
                    List<Int32> lstInv = new List<int>();
                    lstInv.AddRange(data.Parameter2);

                    if (reviewStatusCode.ToLower() == SharedUserInvitationReviewStatus.APPROVED.GetStringValue().ToLower())
                    {
                        lstAlreadyApprovedInvitations = ProfileSharingManager.FilterInvitationIdsByReviewStatusID(lstInv, reviewStatusID);
                    }

                    commonResponse.Result = ProfileSharingManager.SaveUpdateSharedUserInvitationReviewStatus(data.Parameter2, orgUserID, activeUser.OrganizationUserId
                                                                , AppConsts.NONE, notes, reviewStatusCode, allInvitationsToBeUpdated, true, 0, 0, 0, true, isAdminLoggedInAsAgencyUser);

                    if (reviewStatusCode.ToLower() == SharedUserInvitationReviewStatus.APPROVED.GetStringValue().ToLower())
                    {
                        List<Tuple<int, List<int>>> result = ProfileSharingManager.FilterInvitationIdsByTenant(lstInv);

                        if (!result.IsNullOrEmpty())
                        {
                            foreach (var item in result)
                            {
                                List<Int32> lstChangedStatusInvitation = item.Item2.Except(lstAlreadyApprovedInvitations).ToList();

                                if (!lstChangedStatusInvitation.IsNullOrEmpty())
                                {
                                    ComplianceDataManager.SaveBadgeFormNotificationData(item.Item1, null, null, string.Join(",", lstChangedStatusInvitation), orgUserID);
                                }
                            }
                        }
                    }
                }

                if (!data.Parameter3.IsNullOrEmpty())
                {
                    //List<Tuple<Int32, Int32, List<Int32>>> -- RotationId, TenantId, AgencyID, LstOfRotationInvitations
                    List<Int32> lstRotationInvIds = new List<int>();
                    List<String> lstRotationIds = new List<String>();

                    foreach (Tuple<Int32, Int32, Int32, List<Int32>> item in data.Parameter3)
                    {
                        lstRotationInvIds.AddRange(item.Item4);
                        lstRotationIds.Add(String.Concat(item.Item1.ToString(), "-", item.Item2.ToString()));
                    }

                    //UAT-2090
                    ProfileSharingManager.SaveInvitationReviewStatusNotes(lstRotationInvIds, lstRotationIds, reviewStatusCode, notes, orgUserID, activeUser.OrganizationUserId, data.Parameter4);

                    foreach (Tuple<Int32, Int32, Int32, List<Int32>> item in data.Parameter3)
                    {
                        String reviewStatusUpdateCode = String.Empty;
                        Int32? lastReviewedByID = null;
                        reviewStatusUpdateCode = ProfileSharingManager.GetRotationSharedReviewStatus(item.Item1, activeUser.OrganizationUserId, activeUser.OrganizationUserId, item.Item2, item.Item3, ref lastReviewedByID);
                        //UAT-2511
                        ProfileSharingManager.SaveRotationAuditHistory(item.Item1, item.Item2, reviewStatusUpdateCode, String.Empty, activeUser.OrganizationUserId, activeUser.OrganizationUserId, item.Item3);
                        commonResponse.Result = ClinicalRotationManager.SaveUpdateUserRotationReviewStatus(item.Item2, item.Item1, orgUserID, activeUser.OrganizationUserId, reviewStatusUpdateCode, item.Item3, lastReviewedByID, false, isAdminLoggedInAsAgencyUser);
                    }

                    if (reviewStatusCode.ToLower().Trim() == SharedUserInvitationReviewStatus.APPROVED.GetStringValue().ToLower())
                    {


                        foreach (Tuple<Int32, Int32, Int32, List<Int32>> item in data.Parameter3)
                        {
                            List<Int32> lstStatusChangesdInvitations = ProfileSharingManager.GetInvitationIDsIfInvitationStatusChanged(item.Item4, reviewStatusID);

                            if (!lstStatusChangesdInvitations.IsNullOrEmpty())
                            {
                                ComplianceDataManager.SaveBadgeFormNotificationData(item.Item2, null, null, string.Join(",", lstStatusChangesdInvitations), orgUserID);
                            }
                        }
                    }
                }

                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<SharedUserRotationReviewStatusContract>> IClinicalRotation.GetReviewStatusList(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<SharedUserRotationReviewStatusContract>> commonResponse = new ServiceResponse<List<SharedUserRotationReviewStatusContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetReviewStatusList(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region 1846
        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetAttestationReportsWithoutSignature(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                Int32 LoggedInUserID = data.Parameter;
                commonResponse.Result = SharedUserClinicalRotationManager.GetAttestationReportDataWithoutSignature(LoggedInUserID);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-1881
        ServiceResponse<List<AgencyDetailContract>> IClinicalRotation.GetAllAgencyByOrgUser(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<AgencyDetailContract>> commonResponse = new ServiceResponse<List<AgencyDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetAllAgencyForOrgUser(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        //UAT-2090
        //Boolean IClinicalRotation.CheckRotationExistInPSI(ServiceRequest<List<Int32>, Int32, Dictionary<String, String>> data)
        //{
        //    UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
        //    try
        //    {
        //        //ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();

        //        String reviewStatusCode = String.Empty;
        //        String notes = String.Empty;
        //        data.Parameter3.TryGetValue(AppConsts.DIC_KEY_REVIEW_STATUS, out reviewStatusCode);
        //        data.Parameter3.TryGetValue(AppConsts.DIC_KEY_NOTES, out notes);
        //        ProfileSharingManager.CheckRotationExistInPSI(data.Parameter1, data.Parameter2, reviewStatusCode, notes,activeUser.OrganizationUserId);
        //        return true;
        //        //return commonResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        base.LogClinicalRotationSvcError(ex);
        //        throw;
        //    }
        //}
        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        ServiceResponse<Boolean> IClinicalRotation.SaveUpdateClinicalRotationAssignments(ServiceRequest<List<Int32>, ClinicalRotationDetailContract, Dictionary<String, String>> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                Int32 packageID = AppConsts.NONE;
                String rotationAssignType = String.Empty;
                String senderEmailID = String.Empty;
                if (!data.Parameter3.IsNullOrEmpty())
                {
                    rotationAssignType = data.Parameter3.FirstOrDefault().Key;
                    if (String.Compare(rotationAssignType, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0)
                    {
                        senderEmailID = data.Parameter3.FirstOrDefault().Value;
                    }
                    else
                    {
                        packageID = Convert.ToInt32(data.Parameter3.FirstOrDefault().Value);
                    }
                }
                commonResponse.Result = ClinicalRotationManager.SaveUpdateClinicalRotationAssignments(data.SelectedTenantId, data.Parameter1, data.Parameter2
                                                                                                      , activeUser.OrganizationUserId, rotationAssignType, packageID, senderEmailID);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IClinicalRotation.IsDataEnteredForAnyRotation(ServiceRequest<String, String> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.IsDataEnteredForAnyRotation(data.SelectedTenantId, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.IsPreceptorAssignedForAllRotations(ServiceRequest<String> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.IsPreceptorAssignedForAllRotations(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<InstructorAvailabilityContract>> IClinicalRotation.CheckInstAvailabilityByRotationIds(ServiceRequest<String> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<InstructorAvailabilityContract>> commonResponse = new ServiceResponse<List<InstructorAvailabilityContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.CheckInstAvailabilityByRotationIds(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
            #endregion


            #region UAT-2071, Configuration Rotation and Tracking packages must be fully compliant to share
            ServiceResponse<List<RotationAndTrackingPkgStatusContract>> IClinicalRotation.GetComplianceStatusOfImmunizationAndRotationPackages(ServiceRequest<Int32, Dictionary<String, String>, Int32> data)
        {
            ServiceResponse<List<RotationAndTrackingPkgStatusContract>> commonResponse = new ServiceResponse<List<RotationAndTrackingPkgStatusContract>>();
            try
            {
                commonResponse.Result = ProfileSharingManager.GetComplianceStatusOfImmunizationAndRotationPackages(data.Parameter1, data.Parameter2.GetValue("DelimittedOrgUserIDs"), data.Parameter2.GetValue("DelimittedTrackingPkgIDs"), data.Parameter3, data.Parameter2.GetValue("SearchType"));
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Dictionary<Int32, String>> IClinicalRotation.IsRequirementPkgCompliantReqd(ServiceRequest<String> data)
        {
            ServiceResponse<Dictionary<Int32, String>> commonResponse = new ServiceResponse<Dictionary<Int32, String>>();
            try
            {
                commonResponse.Result = ProfileSharingManager.IsRequirementPkgCompliantReqd(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion


        #region UAT-2051, No defining details about Roation. Roation/Profile simply says "roation shared".
        ServiceResponse<String> IClinicalRotation.GetSharingInfoByInvitationGrpID(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = ProfileSharingManager.GetSharingInfoByInvitationGrpID(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion


        ServiceResponse<Tuple<List<Int32>,String, Dictionary<Boolean, String>>> IClinicalRotation.RotationSubscriptionApproveAllPendingItems(ServiceRequest<Int32, Int32,Boolean> data, ref Int32 affectedItemsCount)
        {
            ServiceResponse<Tuple<List<Int32>,String, Dictionary<Boolean, String>>> commonResponse = new ServiceResponse<Tuple<List<Int32>,String, Dictionary<Boolean, String>>>();
            try
            {
                commonResponse.Result = RequirementVerificationManager.RotationSubscriptionApproveAllPendingItems(data.SelectedTenantId, data.Parameter1, data.Parameter2,data.Parameter3, ref affectedItemsCount);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        ServiceResponse<Dictionary<Int32, Boolean>> IClinicalRotation.GetComplianceRequiredRotCatForPackage(ServiceRequest<Int32> data)
        {
            ServiceResponse<Dictionary<Int32, Boolean>> commonResponse = new ServiceResponse<Dictionary<Int32, Boolean>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetComplianceRequiredRotCatForPackage(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2371
        ServiceResponse<SystemEntityUserPermission> IClinicalRotation.GetSystemEntityUserPermission(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<SystemEntityUserPermission> commonResponse = new ServiceResponse<SystemEntityUserPermission>();
            try
            {
                commonResponse.Result = RequirementVerificationManager.GetSystemEntityUserPermission(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2514
        ServiceResponse<Dictionary<Boolean, DateTime>> IClinicalRotation.IsRotationEndDateRangeNeedToManage(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<Dictionary<Boolean, DateTime>> commonResponse = new ServiceResponse<Dictionary<Boolean, DateTime>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.IsRotationEndDateRangeNeedToManage(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2424
        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetAllClinicalRotations(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetAllClinicalRotations(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        //UAT-3121

        //ServiceResponse<Int32> IClinicalRotation.GetRotationPackageIDByRotationId(ServiceRequest<Int32, String> data)
        //{
        //    ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
        //    try
        //    {
        //        commonResponse.Result = ClinicalRotationManager.GetRotationPackageIDByRotationId(data.SelectedTenantId, data.Parameter1, data.Parameter2);
        //        return commonResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        base.LogClinicalRotationSvcError(ex);
        //        throw;
        //    }
        //}

        ServiceResponse<RequirementPackageContract> IClinicalRotation.GetRotationPackageIDByRotationId(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<RequirementPackageContract> commonResponse = new ServiceResponse<RequirementPackageContract>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetRotationPackageByRotationId(data.SelectedTenantId, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<ClinicalRotationDetailContract> IClinicalRotation.GetClinicalRotationDetailsById(ServiceRequest<Int32> data)
        {
            ServiceResponse<ClinicalRotationDetailContract> commonResponse = new ServiceResponse<ClinicalRotationDetailContract>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetClinicalRotationDetailsById(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-4334
        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetAllClinicalRotationsForLoggedInUser(ServiceRequest<Int32, Int32, Boolean> data)
        {
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetAllClinicalRotationsForLoggedInUser(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        } 
        #endregion

        #region UAT-2554

        ServiceResponse<Boolean> IClinicalRotation.IsPreceptorRequiredForAgency(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.IsPreceptorRequiredForAgency(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2313

        ServiceResponse<List<ClinicalRotationDetailContract>> IClinicalRotation.GetClinicalRotationDataFromFlatTable(ServiceRequest<ClinicalRotationDetailContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = SharedUserClinicalRotationManager.GetClinicalRotationDataFromFlatTable(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<WeekDayContract>> IClinicalRotation.GetWeekDays()
        {
            ServiceResponse<List<WeekDayContract>> commonResponse = new ServiceResponse<List<WeekDayContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = SharedUserClinicalRotationManager.GetWeekDays();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<ClientContactContract>> IClinicalRotation.GetAllClientContacts()
        {
            ServiceResponse<List<ClientContactContract>> commonResponse = new ServiceResponse<List<ClientContactContract>>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetAllClientContacts();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-2666
        ServiceResponse<Boolean> IClinicalRotation.UpdateClinicalRotationByAgency(ServiceRequest<ClinicalRotationDetailContract, Int32, Boolean, Int32?> data)
        {
            int orgUserID;
            if (!data.Parameter4.IsNull())
                orgUserID = data.Parameter4.Value;
            else
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                orgUserID = activeUser.OrganizationUserId;
            }
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.UpdateClinicalRotationByAgency(data.Parameter1, data.Parameter2, orgUserID, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RotationFieldUpdatedByAgencyContract>> IClinicalRotation.GetRotationFieldUpdateByAgencyDetails(ServiceRequest<List<Int32>, Int32> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<RotationFieldUpdatedByAgencyContract>> commonResponse = new ServiceResponse<List<RotationFieldUpdatedByAgencyContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetRotationFieldUpdateByAgencyDetails(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2510
        ServiceResponse<Boolean> IClinicalRotation.GetAgencyUserSSN_Permission(ServiceRequest<String> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ProfileSharingManager.GetAgencyUserSSN_Permission(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2513
        ServiceResponse<Boolean> IClinicalRotation.SaveBatchRotationUploadDetails(ServiceRequest<List<BatchRotationUploadContract>, String, Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.SaveBatchRotationUploadDetails(data.Parameter1, data.Parameter2, data.Parameter3, data.Parameter4);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<BatchRotationUploadContract>> IClinicalRotation.GetBatchRotationList(ServiceRequest<Int32, BatchRotationUploadContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<BatchRotationUploadContract>> commonResponse = new ServiceResponse<List<BatchRotationUploadContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetBatchRotationList(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        #endregion
        #region UAT-2926
        ServiceResponse<List<String>> IClinicalRotation.GetAgencyHierarchyAgencyList(ServiceRequest<List<Tuple<Int32, Int32>>> data)
        {
            ServiceResponse<List<String>> commonResponse = new ServiceResponse<List<String>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetAgencyHierarchyAgencyList(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<List<String>> IClinicalRotation.FilterApplicantHavingOnlyNonActiveOrExpireOrders(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<List<String>> commonResponse = new ServiceResponse<List<String>>();
            try
            {
                commonResponse.Result = ProfileSharingManager.FilterApplicantHavingOnlyNonActiveOrExpireOrders(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region [UAT-2679]

        ServiceResponse<List<RequirementPackageContract>> IClinicalRotation.GetRequirementPackage(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetRequirementPackage(data.Parameter1,data.Parameter2 );
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementCategoryContract>> IClinicalRotation.GetRequirementCategory(ServiceRequest<Int32, List<Int32>> data)
        {
            ServiceResponse<List<RequirementCategoryContract>> commonResponse = new ServiceResponse<List<RequirementCategoryContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetRequirementCategory(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementItemContract>> IClinicalRotation.GetRequirementItem(ServiceRequest<Int32, List<Int32>> data)
        {
            ServiceResponse<List<RequirementItemContract>> commonResponse = new ServiceResponse<List<RequirementItemContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetRequirementItem(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<ApplicantRequirementDataAuditContract>> IClinicalRotation.GetApplicantRequirementDataAudit(ServiceRequest<Int32, ApplicantRequirementDataAuditSearchContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<ApplicantRequirementDataAuditContract>> commonResponse = new ServiceResponse<List<ApplicantRequirementDataAuditContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetApplicantRequirementDataAudit(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        #endregion

        #region Requirement Verification Assignment Queue AND User Work Queue

        ServiceResponse<List<RequirementVerificationQueueContract>> IClinicalRotation.GetAssignmentRotationVerificationQueueData(ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<RequirementVerificationQueueContract>> commonReponse = new ServiceResponse<List<RequirementVerificationQueueContract>>();

            try
            {
                commonReponse.Result = ClinicalRotationManager.GetAssignmentRotationVerificationQueueData(data.Parameter1, data.Parameter2);
                return commonReponse;

            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }

        }

        ServiceResponse<List<ReqPkgSubscriptionIDList>> IClinicalRotation.GetReqPkgSubscriptionIdList(ServiceRequest<RequirementVerificationQueueContract, int, int> data)
        {
            ServiceResponse<List<ReqPkgSubscriptionIDList>> commonReponse = new ServiceResponse<List<ReqPkgSubscriptionIDList>>();

            try
            {
                commonReponse.Result = ClinicalRotationManager.GetReqPkgSubscriptionIdList(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonReponse;

            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }

        }

        ServiceResponse<List<ReqPkgSubscriptionIDList>> IClinicalRotation.GetReqPkgSubscriptionIdListForRotationVerification(ServiceRequest<RequirementVerificationQueueContract, int, int, int> data)
        {
            ServiceResponse<List<ReqPkgSubscriptionIDList>> commonReponse = new ServiceResponse<List<ReqPkgSubscriptionIDList>>();

            try
            {
                commonReponse.Result = RequirementVerificationManager.GetReqPkgSubscriptionIdListForRotationVerification(data.Parameter1, data.Parameter2, data.Parameter3, data.Parameter4);
                return commonReponse;

            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }

        }
        ServiceResponse<Boolean> IClinicalRotation.AssignItemsToUser(ServiceRequest<List<Int32>, Int32, String> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();

            try
            {
                commonResponse.Result = ClinicalRotationManager.AssignItemsToUser(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }

        }

        ServiceResponse<List<RequirementPackageTypeContract>> IClinicalRotation.GetSharedRequirementPackageTypes()
        {
            ServiceResponse<List<RequirementPackageTypeContract>> commonReponse = new ServiceResponse<List<RequirementPackageTypeContract>>();

            try
            {
                commonReponse.Result = ClinicalRotationManager.GetSharedRequirementPackageTypes();
                return commonReponse;

            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }

        }
        #endregion

        /// <summary>
        /// UAT 3164
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<System.Data.DataTable> IClinicalRotation.PerformRotationLiveDataMovement(ServiceRequest<Int32, Int32, Int32> data)
        {
            ServiceResponse<System.Data.DataTable> commonResponse = new ServiceResponse<System.Data.DataTable>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.PerformRotationLiveDataMovement(data.SelectedTenantId, data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// UAT 3164
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<System.Data.DataTable> IClinicalRotation.GetTargetReqPackageSubscriptionIDsForSync(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<System.Data.DataTable> commonResponse = new ServiceResponse<System.Data.DataTable>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetTargetReqPackageSubscriptionIDsForSync(data.SelectedTenantId, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        #region UAT-3197, As an Agency User, I should be able to retrieve the syllabus
        ServiceResponse<List<ClientContactSyllabusDocumentContract>> IClinicalRotation.GetClinicalRotationSyllabusDocumentsByID(ServiceRequest<Int32, Int32> serviceRequest)
        {
            ServiceResponse<List<ClientContactSyllabusDocumentContract>> commonResponse = new ServiceResponse<List<ClientContactSyllabusDocumentContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClinicalRotationManager.GetClinicalRotationSyllabusDocumentsByID(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-3176
        ServiceResponse<List<RequirementAttributeGroupContract>> IClinicalRotation.GetAllRotationAttributeGroup(ServiceRequest<Int32, String, String> data)
        {
            ServiceResponse<List<RequirementAttributeGroupContract>> commonResponse = new ServiceResponse<List<RequirementAttributeGroupContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetAllRotationAttributeGroup(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.SaveUpdateRotationAttributeGroup(ServiceRequest<RequirementAttributeGroupContract, Int32, Boolean> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.SaveUpdateRotationAttributeGroup(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementAttributeGroupContract> IClinicalRotation.GetAttributeGroupById(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<RequirementAttributeGroupContract> commonResponse = new ServiceResponse<RequirementAttributeGroupContract>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.GetAttributeGroupById(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClinicalRotation.IsAttributeGroupMapped(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClinicalRotationManager.IsAttributeGroupMapped(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        #endregion
        ServiceResponse<List<ApplicantDataListContract>> IClinicalRotation.GetRotationMembersForRotationDocs(ServiceRequest<ClinicalRotationSearchContract, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<ApplicantDataListContract>> commonResponse = new ServiceResponse<List<ApplicantDataListContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetRotationMembersForRotationDocs(data.SelectedTenantId, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementCategoryContract>> IClinicalRotation.GetReqPkgCatByRotationID(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<RequirementCategoryContract>> commonResponse = new ServiceResponse<List<RequirementCategoryContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetReqPkgCatByRotationID(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RotationDocumentContact>> IClinicalRotation.GetApplicantDocsByReqCatID(ServiceRequest<String, String, CustomPagingArgsContract> data)
        {
            ServiceResponse<List<RotationDocumentContact>> commonResponse = new ServiceResponse<List<RotationDocumentContact>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetApplicantDocsByReqCatID(data.SelectedTenantId, data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RotationDocumentContact>> IClinicalRotation.GetApplicantDocumentsByDocIDs(ServiceRequest<String, String> data)
        {
            ServiceResponse<List<RotationDocumentContact>> commonResponse = new ServiceResponse<List<RotationDocumentContact>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetApplicantDocumentsByDocIDs(data.SelectedTenantId, data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-3220
        ServiceResponse<Boolean> IClinicalRotation.HideRequirementSharesDetailLink(ServiceRequest<Guid> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.HideRequirementSharesDetailLink(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-3241

        ServiceResponse<List<String>> IClinicalRotation.GetAgencyNamesByIds(ServiceRequest<Int32, List<Int32>> data)
        {
            ServiceResponse<List<String>> commonResponse = new ServiceResponse<List<String>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetAgencyNamesByIds(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-3295
        /// <summary>
        /// Gets Clinical Rotation by Id
        /// </summary>
        /// <param name="data">ClinicalRotationId</param>
        /// <returns></returns>
        ServiceResponse<ProfileSharingInvitationDetailsContract> IClinicalRotation.GetProfileShareDetailsById(ServiceRequest<Int32> data)
        {
            ServiceResponse<ProfileSharingInvitationDetailsContract> commonResponse = new ServiceResponse<ProfileSharingInvitationDetailsContract>();
            try
            {
                // commonResponse.Result = ClinicalRotationManager.GetAllRotationAttributeGroup(data.Parameter1, data.Parameter2, data.Parameter3);
                commonResponse.Result = SharedRequirementPackageManager.GetProfileShareDetailsById(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IClinicalRotation.UpdateProfileShareInvDetails(ServiceRequest<ProfileSharingInvitationDetailsContract, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.UpdateProfileShareInvDetails(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-3315
        ServiceResponse<List<ApplicantDocumentContract>> IClinicalRotation.GetSelectedBadgeDocumentsToExport(ServiceRequest<String, Int32, Int32> data)
        {
            ServiceResponse<List<ApplicantDocumentContract>> commonResponse = new ServiceResponse<List<ApplicantDocumentContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetSelectedBadgeDocumentsToExport(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-3458
        ServiceResponse<List<RequirementExpiringItemListContract>> IClinicalRotation.GetRequirementItemsAboutToExpire(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<List<RequirementExpiringItemListContract>> commonResponse = new ServiceResponse<List<RequirementExpiringItemListContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetRequirementItemsAboutToExpire(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion
        #region UAT-3316
        ServiceResponse<String> IClinicalRotation.GetSharedUserTemplatePermissions(ServiceRequest<Int32, Boolean> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetSharedUserTemplatePermissions(data.Parameter1, data.Parameter2);
                //ClinicalRotationManager.GetSharedUserTemplatePermissions(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion
        #region UAT-3470
        ServiceResponse<Boolean> IClinicalRotation.SaveUpdateInvitationArchiveState(ServiceRequest<List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            int orgUserID = activeUser.OrganizationUserId;

            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ProfileSharingManager.SaveUpdateInvitationArchiveState(String.Join(",", data.Parameter1), ProfileSharingManager.CreateRotationContractXml(data.Parameter2), orgUserID, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion


        ServiceResponse<List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract>> IClinicalRotation.GetSharedUserAgencyHierarchyRootNodes()
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract>> commonResponse = new ServiceResponse<List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract>>();
            try
            {
                commonResponse.Result = SharedUserClinicalRotationManager.GetSharedUserAgencyHierarchyRootNodes(activeUser.UserID);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-3977
        ServiceResponse<Dictionary<Int32, String>> IClinicalRotation.InstructorPreceptorRequiredPkgCompliantReqd(ServiceRequest<String> data)
        {
            ServiceResponse<Dictionary<Int32, String>> commonResponse = new ServiceResponse<Dictionary<Int32, String>>();
            try
            {
                commonResponse.Result = ProfileSharingManager.InstructorPreceptorRequiredPkgCompliantReqd(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-3957
        ServiceResponse<List<RequirementItemRejectionContract>> IClinicalRotation.GetRequirementRejectedItemDetailsForMail(ServiceRequest<String, Int32> data)
        {
            ServiceResponse<List<RequirementItemRejectionContract>> commonResponse = new ServiceResponse<List<RequirementItemRejectionContract>>();

            try
            {
                commonResponse.Result = RequirementVerificationManager.GetRequirementRejectedItemDetailsForMail(data.Parameter1,data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }

        }

        #endregion


        ServiceResponse<Boolean> IClinicalRotation.IsAgenycHierarchyAvailable(ServiceRequest<String> data)
        { 
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.IsAgenycHierarchyAvailable(data.SelectedTenantId, data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

    }
}
