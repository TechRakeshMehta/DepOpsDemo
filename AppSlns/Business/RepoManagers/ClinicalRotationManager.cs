using System;
using System.Collections.Generic;
using System.Linq;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.Templates;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using System.Xml.Linq;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.UI.Contract.ClinicalRotation;
using System.Text;
using System.Data;
using INTSOF.UI.Contract.ProfileSharing;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using Entity;
using System.Xml;
using System.Configuration;
using System.Web.Configuration;

namespace Business.RepoManagers
{
    public class ClinicalRotationManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static ClinicalRotationManager()
        {
            BALUtils.ClassModule = "Clinical Rotation Manager";
        }

        #endregion

        #region Common Methods
        /// <summary>
        /// Retrieves a list of all active tenants.
        /// </summary>
        /// <param name="SortByName"></param>
        /// <param name="GetExtendedProperties"></param>
        /// <returns>List of data from the underlying data storage.</returns>
        public static List<TenantDetailContract> GetTenants(Boolean SortByName, String TenantTypeCode = "")
        {
            try
            {
                List<TenantDetailContract> tenants = null;

                short businessChannelTypeID = AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE;
                //if (BALUtils.SessionService.BusinessChannelType.IsNotNull())
                //{
                //    businessChannelTypeID = BALUtils.SessionService.BusinessChannelType.BusinessChannelTypeID;
                //}
                tenants = LookupManager.GetLookUpData<Entity.vw_GetTenants>().Where(cond => cond.BCT.Value == businessChannelTypeID)
                                        .Select(col =>
                                        new TenantDetailContract
                                        {
                                            TenantID = col.TenantID,
                                            TenantName = col.TenantName,
                                            TenantTypeID = col.TenantTypeID,
                                        }).ToList();
                if (TenantTypeCode != String.Empty)
                {
                    Int32 tenantTypeId = LookupManager.GetLookUpData<Entity.lkpTenantType>().FirstOrDefault(condition => condition.TenantTypeCode == TenantTypeCode).TenantTypeID;
                    tenants = tenants.Where(x => x.TenantTypeID == tenantTypeId).ToList();
                }
                if (SortByName)
                {
                    tenants = tenants.OrderBy(x => x.TenantName).ToList();
                }
                return tenants;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to Get All Agencies
        /// </summary>
        /// <returns></returns>
        public static List<AgencyDetailContract> GetAllAgency(Int32 institutionID)
        {
            try
            {
                List<Agency> agencyList = BALUtils.GetProfileSharingRepoInstance().GetAllAgency(institutionID);
                return agencyList.Select(col => new AgencyDetailContract
                {
                    AgencyID = col.AG_ID,
                    AgencyName = col.AG_Name,
                }).OrderBy(x => x.AgencyName).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<WeekDayContract> GetWeekDayList(Int32 tenantID)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpWeekDay>(tenantID).Select(col =>
                                         new WeekDayContract
                                         {
                                             WeekDayID = col.WD_ID,
                                             Name = col.WD_Name,
                                             Description = col.WD_Description,
                                             Code = col.WD_Code
                                         }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the UserData from Security database, by OrganizationUserID.
        /// </summary>
        /// <param name="organisationUserID"></param>
        /// <returns></returns>
        public static OrganizationUserContract GetUserData(Int32 organisationUserId, Int32 tenantId)
        {
            try
            {
                var data = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetUserData(organisationUserId);
                //return new OrganizationUserContract();
                return ConvertEntityToOrganizationUserContract(data);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Clinical Rotation Details

        /// <summary>
        /// Gets Applicant Clinical Rotation search data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="searchDataContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        public static List<ApplicantDataListContract> GetApplicantClinicalRotationSearch(Int32 tenantId, Int32 clinicalRotationId, ClinicalRotationSearchContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetApplicantClinicalRotationSearch(clinicalRotationId, searchDataContract, gridCustomPaging);
                //return AssignValuesToRotationDataModel(applicantDataList);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to get all users
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<UserGroupContract> GetAllUserGroup(Int32 tenantId)
        {
            try
            {
                IQueryable<Entity.ClientEntity.UserGroup> userGroupList = BALUtils.GetComplianceSetupRepoInstance(tenantId).GetAllUserGroup();
                return userGroupList.Select(col => new UserGroupContract
                {
                    UG_ID = col.UG_ID,
                    UG_Name = col.UG_Name,
                    UG_Description = col.UG_Description,
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets Clinical Rotation by Id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationId"></param>
        /// <returns></returns>
        public static ClinicalRotationDetailContract GetClinicalRotationById(Int32 tenantId, Int32 clinicalRotationId, Int32? agencyID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationById(clinicalRotationId, agencyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets comma seprated custom attribute list with their values for notification
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationId"></param>
        /// <returns></returns>
        public static string GetClinicalRotationNotificationCustomAttributes(Int32 tenantId, Int32 clinicalRotationId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationNotificationCustomAttributes(clinicalRotationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        ///UAT-2040
        /// <summary>
        /// Gets Clinical Rotation by Id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationId"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetClinicalRotationByIds(Int32 tenantId, String clinicalRotationIds)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationByIds(clinicalRotationIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets Clinical Rotation Members
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<RotationMemberDetailContract> GetClinicalRotationMembers(Int32 tenantId, Int32 clinicalRotationId, Int32 agencyID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationMembers(clinicalRotationId, agencyID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To add applicants to Clinical Rotation
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="clinicalRotationID"></param>
        /// <param name="SelectedOrganizationUserIds"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static Boolean AddApplicantsToRotation(Int32 tenantId, Int32 clinicalRotationID, Dictionary<Int32, Boolean> organizationUserIds, Int32 currentLoggedInUserId)
        {
            try
            {
                String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(tenantId).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                .FirstOrDefault().RPS_ID;
                String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(tenantId).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                .FirstOrDefault().RST_ID;
                String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantId).Where(x => !x.RPT_IsDeleted
                                                                                                                        && x.RPT_Code == reqPkgTypeCode)
                                                                                                                        .FirstOrDefault().RPT_ID;
                Int16 statusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantId, LkpSubscriptionMobilityStatus.DataMovementDue)); //UAT-2603
                Boolean status = BALUtils.GetClinicalRotationRepoInstance(tenantId).AddApplicantsToRotation(clinicalRotationID, requirementNotCompliantPackStatusID, rotationSubscriptionTypeID, organizationUserIds, currentLoggedInUserId, reqPkgTypeId, statusId);
                //Commented Code for UAT-3688 : Rotation SMS should be sent only when admin indicates to send the e-mail
                //Moved to btnScheduleRotation_Click

                //if (status)
                //{
                //    #region UAT-2907
                //    //Send SMS notification
                //    //Create Dictionary for SMS Data
                //    Dictionary<String, object> dictSMSData = new Dictionary<String, object>();

                //    List<Entity.OrganizationUser> lstOrganizationUser = new List<Entity.OrganizationUser>();
                //    if (!organizationUserIds.IsNullOrEmpty())
                //    {
                //        List<Int32> lstOrgUserIds = new List<Int32>();
                //        lstOrgUserIds = organizationUserIds.Where(con => con.Value == true).Select(sel => sel.Key).ToList();

                //        lstOrganizationUser = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetOrganizationUserFromIds(lstOrgUserIds);
                //    }
                //    if (!lstOrganizationUser.IsNullOrEmpty())
                //    {
                //        foreach (Entity.OrganizationUser orgUser in lstOrganizationUser)
                //        {
                //            CommunicationMockUpData mockSMSData = new CommunicationMockUpData();
                //            mockSMSData.UserName = string.Concat(orgUser.FirstName, " ", orgUser.LastName);
                //            mockSMSData.EmailID = orgUser.PrimaryEmailAddress;
                //            mockSMSData.ReceiverOrganizationUserID = orgUser.OrganizationUserID;
                //            CommunicationManager.SaveDataForSMSNotification(CommunicationSubEvents.NOTIFICATION_CLINICAL_ROTATION_ASSIGNED_SMS, mockSMSData,
                //                                                            dictSMSData, tenantId, AppConsts.NONE);
                //        }
                //    }
                //    #endregion
                //}
                return status;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Remove applicants from clinical rotation
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationMemberIDs"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static Boolean RemoveApplicantsFromRotation(Int32 tenantId, Dictionary<Int32, Boolean> clinicalRotationMemberIDs, Int32 currentLoggedInUserId
                                                           , List<Int32> approvedMemberIdsToRemove)
        {
            try
            {
                //Int32 reqPkgTypeId = 1;
                String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantId).Where(x => !x.RPT_IsDeleted
                                                                                                                        && x.RPT_Code == reqPkgTypeCode)
                                                                                                                        .FirstOrDefault().RPT_ID;
                //UAT-2603
                Int16 dataMovementDueStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantId, LkpSubscriptionMobilityStatus.DataMovementDue));
                Int16 dataMovementNotRequiredStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantId, LkpSubscriptionMobilityStatus.DataMovementNotRequired));

                //UAT-3222
                RotationStudentDropped rotationStudentDropped = BALUtils.GetClinicalRotationRepoInstance(tenantId).RemoveApplicantsFromRotation(clinicalRotationMemberIDs, currentLoggedInUserId, reqPkgTypeId
                                                                                                       , approvedMemberIdsToRemove, tenantId, dataMovementDueStatusId, dataMovementNotRequiredStatusId);
                if (rotationStudentDropped.IsRemovedApplicantsFromRotation)
                {
                    CommunicationManager.SendMailWhenStudentDroppedFromRotation(rotationStudentDropped);
                    Int32? lastReviewedByID = null;
                    String reviewStatusUpdateCode;
                    reviewStatusUpdateCode = ProfileSharingManager.GetRotationSharedReviewStatus(rotationStudentDropped.RotationID, rotationStudentDropped.InviteeOrgId, 0, rotationStudentDropped.TenantID, rotationStudentDropped.AgencyId.Value, ref lastReviewedByID);
                    ProfileSharingManager.SaveRotationAuditHistory(rotationStudentDropped.RotationID, rotationStudentDropped.TenantID, reviewStatusUpdateCode, "", currentLoggedInUserId, rotationStudentDropped.InviteeOrgId, rotationStudentDropped.AgencyId.Value);
                    ClinicalRotationManager.SaveUpdateUserRotationReviewStatus(rotationStudentDropped.TenantID, rotationStudentDropped.RotationID, currentLoggedInUserId, rotationStudentDropped.InviteeOrgId, reviewStatusUpdateCode, rotationStudentDropped.AgencyId.Value, rotationStudentDropped.InviteeOrgId, false, false);
                }
                return rotationStudentDropped.IsRemovedApplicantsFromRotation;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the ApplicantId's in the current Rotation
        /// </summary>
        /// <param name="rotationId"></param>
        /// <returns></returns>
        public static List<Int32> GetRotationApplicantIds(Int32 rotationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationApplicantIds(rotationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to Get Instructor/Preceptor Data, including the backgrond and compliance shared info type codes.
        /// </summary>
        /// <param name="rotationId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ClientContactProfileSharingData> GetRotationClientContacts(Int32 rotationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationClientContacts(rotationId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Add requirement package to rotation
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationID"></param>
        /// <param name="requirementPackageID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static Boolean AddPackageToRotation(Int32 tenantId, Int32 clinicalRotationID, Int32 requirementPackageID, String reqPkgTypeCode, Int32 currentLoggedInUserId)
        {
            try
            {
                // = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantId).Where(x => !x.RPT_IsDeleted
                                                                                                                        && x.RPT_Code == reqPkgTypeCode)
                                                                                                                        .FirstOrDefault().RPT_ID;
                BALUtils.GetClinicalRotationRepoInstance(tenantId).AddPackageToRotation(clinicalRotationID, requirementPackageID, currentLoggedInUserId, reqPkgTypeId);
                if (reqPkgTypeCode == RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue())
                {
                    IEnumerable<ClinicalRotationMember> clinicalRotationMember = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationMemberListByRotationId(clinicalRotationID);
                    if (clinicalRotationMember.Count() > AppConsts.NONE)
                    {
                        List<ClinicalRotationMember> clinicalRotationMemberList = clinicalRotationMember.ToList();
                        Dictionary<Int32, Boolean> organizationUserIds = new Dictionary<int, bool>();
                        clinicalRotationMemberList.ForEach(cond =>
                        {
                            organizationUserIds.Add(cond.CRM_ApplicantOrgUserID, true);
                        });
                        String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                        Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(tenantId).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                        .FirstOrDefault().RPS_ID;
                        String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                        Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(tenantId).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                        .FirstOrDefault().RST_ID;
                        List<RequirementPackageSubscription> lstRequirementPackageSubscriptionToBeAdded = BALUtils.GetClinicalRotationRepoInstance(tenantId).AddRequirementPackageSubscription(clinicalRotationID, requirementNotCompliantPackStatusID, rotationSubscriptionTypeID, organizationUserIds, currentLoggedInUserId, reqPkgTypeId);
                        if (BALUtils.GetClinicalRotationRepoInstance(tenantId).SaveContextIntoDataBase())
                        {
                            BALUtils.GetClinicalRotationRepoInstance(tenantId).CreateOptionalCategorySetAproved(lstRequirementPackageSubscriptionToBeAdded, currentLoggedInUserId);

                            //UAT-2603//
                            Int16 statusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantId, LkpSubscriptionMobilityStatus.DataMovementDue));
                            List<Int32> lstReqSubsIds = lstRequirementPackageSubscriptionToBeAdded.Select(cond => cond.RPS_ID).ToList();
                            BALUtils.GetClinicalRotationRepoInstance(tenantId).AddDataToRotDataMovement(lstReqSubsIds, currentLoggedInUserId, statusId);

                            return true;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Clinical Rotation requirement package by ClinicalRotationID
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationID"></param>
        /// <returns></returns>
        public static ClinicalRotationRequirementPackageContract GetRotationRequirementPackage(Int32 tenantId, Int32 clinicalRotationID, String pkgTypeCode)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationRequirementPackage(clinicalRotationID, pkgTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Is Clinical Rotation Members exist for clinical rotation
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationID"></param>
        /// <returns></returns>
        public static Boolean IsClinicalRotationMembersExistForRotation(Int32 tenantId, Int32 clinicalRotationID)
        {
            try
            {
                var clinicalRotationMember = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationMemberByRotationId(clinicalRotationID);
                if (clinicalRotationMember.IsNotNull() && clinicalRotationMember.CRM_ID > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1843: Phase 2 5: Combining User group mapping, archive and rotation assignment screens

        /// <summary>
        /// Get Clinical Rotation Mapping Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <param name="applicantUserIds"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetClinicalRotationMappingData(Int32 tenantId, CustomPagingArgsContract customPagingArgsContract,
                                                                String applicantUserIds = null, Int32? currentUserId = null)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationMappingData(customPagingArgsContract, applicantUserIds, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign Rotations to applicant users
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="rotationIds"></param>
        /// <param name="applicantUserIds"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean AssignRotationsToUsers(Int32 tenantId, List<Int32> rotationIds, List<Int32> applicantUserIds, Int32 currentUserId, out String message, out String messageType)
        {
            try
            {
                String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(tenantId).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                .FirstOrDefault().RPS_ID;
                String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(tenantId).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                .FirstOrDefault().RST_ID;
                String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantId).Where(x => !x.RPT_IsDeleted
                                                                                                                        && x.RPT_Code == reqPkgTypeCode)
                                                                                                                        .FirstOrDefault().RPT_ID;
                //UAT-2603//
                Int16 statusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantId, LkpSubscriptionMobilityStatus.DataMovementDue));
                List<Tuple<Int32, Int32>> applicantList = new List<Tuple<int, int>>();
                Boolean IsApplicantTakeSpecialPackage = false;
                Boolean status = BALUtils.GetClinicalRotationRepoInstance(tenantId).AssignRotationsToUsers(rotationIds, applicantUserIds, currentUserId,
                        requirementNotCompliantPackStatusID, rotationSubscriptionTypeID, reqPkgTypeId, statusId, out applicantList, out IsApplicantTakeSpecialPackage);

                message = String.Empty;
                messageType = String.Empty;
                if (applicantList.IsNotNull() && applicantList.Count > AppConsts.NONE)
                {
                    List<Int32> notExemptedApplicants = applicantList.Select(x => x.Item1).Distinct().ToList();
                    if (notExemptedApplicants.Count == applicantUserIds.Count)
                    {
                        if (!IsApplicantTakeSpecialPackage)
                        {
                            List<Int32> rotationIdLst = applicantList.Where(X => applicantUserIds.Contains(X.Item1)).Select(x => x.Item2).ToList();
                            if (rotationIdLst.Count == rotationIds.Count)
                            {
                                message = "This applicant cannot be assigned to rotation.";
                                List<Int32> applicants = applicantList.Where(X => rotationIds.Contains(X.Item2)).Select(x => x.Item1).ToList();
                                applicantUserIds = applicantUserIds.Except(applicants).ToList();
                                messageType = MessageType.Information.ToString();
                            }
                            else
                            {
                                message = "Selected applicant(s) assigned successfully to exempted rotation(s).";
                                messageType = MessageType.SuccessMessage.ToString();
                                // message = "Applicants which are exempt from qualify to rotation are assigned successfully and other applicant(s) cannot be assigned.";
                            }
                        }
                        else
                        {
                            message = "Rotation(s) assigned successfully.";
                            messageType = MessageType.SuccessMessage.ToString();
                        }
                    }
                    else if (applicantUserIds.Count > applicantList.Count)
                    {
                        message = "Applicants which are exempt from qualify to rotation are assigned successfully and other applicant(s) cannot be assigned.";
                        List<Int32> applicants = applicantList.Where(X => rotationIds.Contains(X.Item2)).Select(x => x.Item1).ToList();
                        applicantUserIds = applicantUserIds.Except(applicants).ToList();
                        messageType = MessageType.SuccessMessage.ToString();
                    }

                }

                //Commented Code for UAT-3688 : Rotation SMS should be sent only when admin indicates to send the e-mail
                //Moved to btnScheduleRotation_Click


                //if (status)
                //{
                //    #region UAT-2907
                //    //Send SMS notification
                //    //Create Dictionary for SMS Data
                //    Dictionary<String, object> dictSMSData = new Dictionary<String, object>();

                //    List<Entity.OrganizationUser> lstOrganizationUser = new List<Entity.OrganizationUser>();
                //    if (!applicantUserIds.IsNullOrEmpty())
                //    {
                //        //List<Int32> lstOrgUserIds = new List<Int32>();
                //        //lstOrgUserIds = organizationUserIds.Where(con => con.Value == true).Select(sel => sel.Key).ToList();

                //        lstOrganizationUser = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetOrganizationUserFromIds(applicantUserIds);
                //    }
                //    if (!lstOrganizationUser.IsNullOrEmpty())
                //    {
                //        foreach (Entity.OrganizationUser orgUser in lstOrganizationUser)
                //        {
                //            CommunicationMockUpData mockSMSData = new CommunicationMockUpData();
                //            mockSMSData.UserName = string.Concat(orgUser.FirstName, " ", orgUser.LastName);
                //            mockSMSData.EmailID = orgUser.PrimaryEmailAddress;
                //            mockSMSData.ReceiverOrganizationUserID = orgUser.OrganizationUserID;
                //            CommunicationManager.SaveDataForSMSNotification(CommunicationSubEvents.NOTIFICATION_CLINICAL_ROTATION_ASSIGNED_SMS, mockSMSData,
                //                                                            dictSMSData, tenantId, AppConsts.NONE);
                //        }
                //    }
                //    #endregion
                //}
                return status;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Unassign Rotations of applicants
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="rotationIds"></param>
        /// <param name="applicantUserIds"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean UnassignRotations(Int32 tenantId, List<Int32> rotationIds, List<Int32> applicantUserIds, Int32 currentUserId)
        {
            try
            {
                bool status = false;
                String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantId).Where(x => !x.RPT_IsDeleted
                                                                                                                        && x.RPT_Code == reqPkgTypeCode)
                                                                                                                        .FirstOrDefault().RPT_ID;
                //UAT-2603
                Int16 dataMovementDueStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantId, LkpSubscriptionMobilityStatus.DataMovementDue));
                Int16 dataMovementNotRequiredStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantId, LkpSubscriptionMobilityStatus.DataMovementNotRequired));

                //status = 
                List<RotationStudentDropped> lstRotationStudentDropped = BALUtils.GetClinicalRotationRepoInstance(tenantId).UnassignRotations(rotationIds, applicantUserIds, currentUserId, reqPkgTypeId, tenantId, dataMovementDueStatusId, dataMovementNotRequiredStatusId);

                //UAT-3222
                if (!lstRotationStudentDropped.IsNullOrEmpty())
                {
                    foreach (RotationStudentDropped rotationStudentDropped in lstRotationStudentDropped)
                    {
                        CommunicationManager.SendMailWhenStudentDroppedFromRotation(rotationStudentDropped);
                    }
                    status = lstRotationStudentDropped[0].IsRemovedApplicantsFromRotation;
                }

                //Rotation Drop (If Needed)
                BALUtils.GetClinicalRotationRepoInstance(tenantId).DropRotaionIfRequired(rotationIds, currentUserId);
                return status;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #endregion

        #region Manage Rotations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetClinicalRotationQueueData(ClinicalRotationDetailContract clinicalRotationDetailContract
                                                                                        , CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID)
                               .GetClinicalRotationQueueData(clinicalRotationDetailContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static RotationsMappedToAgenciesContract GetRotationsMappedToAgencies(Int32 tenantID, String rotationIDs)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID)
                               .GetRotationsMappedToAgencies(rotationIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-3139: Client Admin Auto-archive rotations 1 year following the rotation end date.
        public static Boolean ProcessRotationToArchive(Int32 chunkSize, Int32 systemUserId, Int32 tenantID)
        {
            try
            {
                List<Int32> lstRotationIdsToArchive = BALUtils.GetClinicalRotationRepoInstance(tenantID).SetRotationsToArchive(chunkSize, systemUserId);
                if (!lstRotationIdsToArchive.IsNullOrEmpty())
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        /// <summary>
        /// Method to SaveUpdate clinical rotation
        /// </summary>
        /// <returns></returns>
        public static Tuple<Int32, Boolean, Boolean> SaveUpdateClinicalRotationData(ClinicalRotationDetailContract clinicalRotationDetailContract, List<CustomAttribteContract> customAttributeListToSave, Int32 currentUserID, String SenderEmailID)
        {
            try
            {
                String syllabusDocumentTypeCode = DocumentType.ROTATION_SYLLABUS.GetStringValue();

                string RotationAdditionalDocument = DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue();

                Boolean IsRoationPackageAssignThroughCloning = false;
                Boolean IsInstructorPackageAssignThroughCloning = false;
                Boolean IsApplicantPkgNotAssignedThroughCloning = false;
                Boolean IsInstructorPkgNotAssignedThroughCloning = false;
                Int32 additionalDocumentTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(clinicalRotationDetailContract.TenantID)
                                                            .Where(cond => cond.DMT_Code == RotationAdditionalDocument).FirstOrDefault().DMT_ID;

                Int32 syllabusDocumentTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(clinicalRotationDetailContract.TenantID)
                                                            .Where(cond => cond.DMT_Code == syllabusDocumentTypeCode).FirstOrDefault().DMT_ID;

                String profileSynchAddToRotationCode = ProfileSynchSourceType.ADD_TO_CLINICAL_ROTATION.GetStringValue();
                Int32 profileSynchSourceTypeID = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpProfileSharingInvitationGroupType>().Where(cond => cond.PSIGT_Code == profileSynchAddToRotationCode).Select(col => col.PSIGT_ID).FirstOrDefault();

                if (clinicalRotationDetailContract.RotationID == AppConsts.NONE)
                {
                    clinicalRotationDetailContract.RotationID = BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID).SaveClinicalRotation(clinicalRotationDetailContract, customAttributeListToSave, currentUserID, syllabusDocumentTypeID, profileSynchSourceTypeID, additionalDocumentTypeId);
                    if (clinicalRotationDetailContract.RotationID != AppConsts.NONE)
                    {



                        if (clinicalRotationDetailContract.IsCloningRotation && !clinicalRotationDetailContract.IsAgencyUpdated)
                        {

                            if (clinicalRotationDetailContract.RequirementPackageID.IsNotNull() && clinicalRotationDetailContract.RequirementPackageID != AppConsts.NONE)
                            {
                                String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                                IsRoationPackageAssignThroughCloning = AddPackageToRotation(clinicalRotationDetailContract.TenantID, clinicalRotationDetailContract.RotationID, clinicalRotationDetailContract.RequirementPackageID, reqPkgTypeCode, currentUserID);
                            }
                            else
                            {
                                #region UAT-3533: Cloned rotations should add the current package available automatically if there is only one package available.

                                if (!clinicalRotationDetailContract.IsNullOrEmpty() && clinicalRotationDetailContract.RequirementPackageID == AppConsts.NONE)
                                    IsRoationPackageAssignThroughCloning = CheckAgencyPackagesAndAssignToRotation(currentUserID, clinicalRotationDetailContract);
                                #endregion
                            }

                            if (clinicalRotationDetailContract.InstructorPreceptorPkgID.IsNotNull() && clinicalRotationDetailContract.InstructorPreceptorPkgID != AppConsts.NONE && !clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
                            {
                                String InstPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
                                //UAT-2424 :- Cloning INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE to newly created rotation.
                                IsInstructorPackageAssignThroughCloning = AddPackageToRotation(clinicalRotationDetailContract.TenantID, clinicalRotationDetailContract.RotationID, clinicalRotationDetailContract.InstructorPreceptorPkgID, InstPkgTypeCode, currentUserID);
                                List<Int32> ContactIdList = new List<Int32>();
                                //UAT-2424 :- Creating  package subscription for INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE while cloning rotation
                                CreateRotationSubscriptionForClientContact(ContactIdList, clinicalRotationDetailContract.RotationID, currentUserID, clinicalRotationDetailContract.TenantID);
                                if (!IsInstructorPackageAssignThroughCloning)
                                    IsInstructorPkgNotAssignedThroughCloning = true;
                            }
                            else
                            {
                                IsInstructorPkgNotAssignedThroughCloning = false;
                            }
                            if (!IsRoationPackageAssignThroughCloning)
                                IsApplicantPkgNotAssignedThroughCloning = true;
                        }
                        else
                        {
                            #region UAT-2973: If only one package exists for an agency, then it should auto assign instead of making the admin select it and assign it.

                            if (!clinicalRotationDetailContract.IsNullOrEmpty())
                                CheckAgencyPackagesAndAssignToRotation(currentUserID, clinicalRotationDetailContract);
                            #endregion
                        }
                        PerformParallelTaskForAgencyAndClientContacts(clinicalRotationDetailContract.RotationID.ToString(), clinicalRotationDetailContract.TenantID, currentUserID);

                        SaveCommunicationDataToSendMail(clinicalRotationDetailContract, SenderEmailID);
                        //return clinicalRotationDetailContract.RotationID;

                        #region UAT:4395

                        if (clinicalRotationDetailContract.IsCloneRotationStudentCheck && clinicalRotationDetailContract.CloneRotationId > 0)
                        {
                            int? CurrentUsersId = null;
                            if (currentUserID != SecurityManager.DefaultTenantID)
                            {
                                CurrentUsersId = currentUserID;
                            }

                            Dictionary<Int32, Boolean> GetExistingorganizationUserIds = BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID).GetExistingorganizationUserIds(clinicalRotationDetailContract.CloneRotationId, CurrentUsersId);
                            if (GetExistingorganizationUserIds != null && GetExistingorganizationUserIds.Count > AppConsts.NONE)
                            {
                                AddApplicantsToRotation(clinicalRotationDetailContract.TenantID, clinicalRotationDetailContract.RotationID, GetExistingorganizationUserIds, currentUserID);
                            }
                        }
                        #endregion
                        return new Tuple<Int32, Boolean, Boolean>(clinicalRotationDetailContract.RotationID, IsApplicantPkgNotAssignedThroughCloning, IsInstructorPkgNotAssignedThroughCloning);

                    }
                    return new Tuple<Int32, Boolean, Boolean>(AppConsts.NONE, IsApplicantPkgNotAssignedThroughCloning, IsInstructorPkgNotAssignedThroughCloning);
                    //return AppConsts.NONE;
                }
                else
                {
                    String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                    Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(clinicalRotationDetailContract.TenantID).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                    .FirstOrDefault().RPS_ID;
                    String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                    Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(clinicalRotationDetailContract.TenantID).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                    .FirstOrDefault().RST_ID;
                    String reqPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
                    Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(clinicalRotationDetailContract.TenantID).Where(x => !x.RPT_IsDeleted
                                                                                                                            && x.RPT_Code == reqPkgTypeCode)
                                                                                                                            .FirstOrDefault().RPT_ID;
                    //UAT-2603
                    Int16 dataMovementDueStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(clinicalRotationDetailContract.TenantID, LkpSubscriptionMobilityStatus.DataMovementDue));
                    Int16 dataMovementNotRequiredStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(clinicalRotationDetailContract.TenantID, LkpSubscriptionMobilityStatus.DataMovementNotRequired));

                    //UAT-3108
                    //if (BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID).UpdateClinicalRotation(clinicalRotationDetailContract, customAttributeListToSave, currentUserID, syllabusDocumentTypeID, profileSynchSourceTypeID
                    //                                                                                                                 , reqPkgTypeId, rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, dataMovementDueStatusId, dataMovementNotRequiredStatusId))

                    RotationDetailFieldChanges rotationDetailFieldChanges = BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID).UpdateClinicalRotation(clinicalRotationDetailContract, customAttributeListToSave, currentUserID, syllabusDocumentTypeID, profileSynchSourceTypeID
                                                                                                                                     , reqPkgTypeId, rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, dataMovementDueStatusId, dataMovementNotRequiredStatusId, additionalDocumentTypeId);
                    if (rotationDetailFieldChanges.IsClinicalRotationUpdatedSuccessfully)
                    {
                        //UAT-4428
                        if (rotationDetailFieldChanges.IsStartDateUpdated)
                        {
                            ExecuteCategoryRuleOnRotationDateChange(clinicalRotationDetailContract.TenantID, clinicalRotationDetailContract.RotationID, currentUserID);
                        }
                        //END

                        //Send mail for rotation update.
                        CommunicationManager.SendMailOnUpdationInRotationdetails(rotationDetailFieldChanges);//UAT0-3108

                        #region UAT-4561://check here if Rotation is approved/ also when only end date changes are there! then only send the below mentioned notification:
                        if (rotationDetailFieldChanges.IsNeedToSendEndDateMail)
                        {
                            CommunicationManager.SendMailOnUpdationInRotationEndDate(clinicalRotationDetailContract, rotationDetailFieldChanges);
                        }
                        #endregion

                        PerformParallelTaskForAgencyAndClientContacts(clinicalRotationDetailContract.RotationID.ToString(), clinicalRotationDetailContract.TenantID, currentUserID);
                        SaveCommunicationDataToSendMail(clinicalRotationDetailContract, SenderEmailID);
                        //return clinicalRotationDetailContract.RotationID;
                        return new Tuple<Int32, Boolean, Boolean>(clinicalRotationDetailContract.RotationID, IsApplicantPkgNotAssignedThroughCloning, IsInstructorPkgNotAssignedThroughCloning);
                    }
                    // return AppConsts.NONE;
                    return new Tuple<Int32, Boolean, Boolean>(AppConsts.NONE, IsApplicantPkgNotAssignedThroughCloning, IsInstructorPkgNotAssignedThroughCloning);
                }
                //UAT-2313

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to SaveUpdate clinical rotation
        /// </summary>
        /// <returns></returns>
        public static Int32 SaveUpdateClinicalRotation(ClinicalRotationDetailContract clinicalRotationDetailContract, List<CustomAttribteContract> customAttributeListToSave, Int32 currentUserID, String SenderEmailID)
        {
            try
            {
                String syllabusDocumentTypeCode = DocumentType.ROTATION_SYLLABUS.GetStringValue();

                Int32 syllabusDocumentTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(clinicalRotationDetailContract.TenantID)
                                                            .Where(cond => cond.DMT_Code == syllabusDocumentTypeCode).FirstOrDefault().DMT_ID;


                String AdditionalDocumentTypeCode = DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue();

                Int32 AdditionalDocumentTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(clinicalRotationDetailContract.TenantID)
                                                            .Where(cond => cond.DMT_Code == AdditionalDocumentTypeCode).FirstOrDefault().DMT_ID;

                String profileSynchAddToRotationCode = ProfileSynchSourceType.ADD_TO_CLINICAL_ROTATION.GetStringValue();
                Int32 profileSynchSourceTypeID = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpProfileSharingInvitationGroupType>().Where(cond => cond.PSIGT_Code == profileSynchAddToRotationCode).Select(col => col.PSIGT_ID).FirstOrDefault();

                if (clinicalRotationDetailContract.RotationID == AppConsts.NONE)
                {
                    clinicalRotationDetailContract.RotationID = BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID).SaveClinicalRotation(clinicalRotationDetailContract, customAttributeListToSave, currentUserID, syllabusDocumentTypeID, profileSynchSourceTypeID, AdditionalDocumentTypeID);
                    if (clinicalRotationDetailContract.RotationID != AppConsts.NONE)
                    {
                        if (clinicalRotationDetailContract.IsCloningRotation && !clinicalRotationDetailContract.IsAgencyUpdated)
                        {

                            if (clinicalRotationDetailContract.RequirementPackageID.IsNotNull() && clinicalRotationDetailContract.RequirementPackageID != AppConsts.NONE)
                            {
                                String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                                AddPackageToRotation(clinicalRotationDetailContract.TenantID, clinicalRotationDetailContract.RotationID, clinicalRotationDetailContract.RequirementPackageID, reqPkgTypeCode, currentUserID);
                            }
                            else
                            {
                                #region UAT-3533: Cloned rotations should add the current package available automatically if there is only one package available.

                                if (!clinicalRotationDetailContract.IsNullOrEmpty() && clinicalRotationDetailContract.RequirementPackageID == AppConsts.NONE)
                                    CheckAgencyPackagesAndAssignToRotation(currentUserID, clinicalRotationDetailContract);
                                #endregion
                            }

                            if (clinicalRotationDetailContract.InstructorPreceptorPkgID.IsNotNull() && clinicalRotationDetailContract.InstructorPreceptorPkgID != AppConsts.NONE && !clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
                            {
                                String InstPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
                                //UAT-2424 :- Cloning INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE to newly created rotation.
                                AddPackageToRotation(clinicalRotationDetailContract.TenantID, clinicalRotationDetailContract.RotationID, clinicalRotationDetailContract.InstructorPreceptorPkgID, InstPkgTypeCode, currentUserID);
                                List<Int32> ContactIdList = new List<Int32>();
                                //UAT-2424 :- Creating  package subscription for INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE while cloning rotation
                                CreateRotationSubscriptionForClientContact(ContactIdList, clinicalRotationDetailContract.RotationID, currentUserID, clinicalRotationDetailContract.TenantID);
                            }

                        }
                        else
                        {
                            #region UAT-2973: If only one package exists for an agency, then it should auto assign instead of making the admin select it and assign it.

                            if (!clinicalRotationDetailContract.IsNullOrEmpty())
                                CheckAgencyPackagesAndAssignToRotation(currentUserID, clinicalRotationDetailContract);
                            #endregion
                        }
                        PerformParallelTaskForAgencyAndClientContacts(clinicalRotationDetailContract.RotationID.ToString(), clinicalRotationDetailContract.TenantID, currentUserID);

                        SaveCommunicationDataToSendMail(clinicalRotationDetailContract, SenderEmailID);

                        if (clinicalRotationDetailContract.IsCloneRotationStudentCheck && clinicalRotationDetailContract.CloneRotationId > 0)
                        {
                            int? CurrentUsersId = null;
                            if (currentUserID != SecurityManager.DefaultTenantID)
                            {
                                CurrentUsersId = currentUserID;
                            }

                            Dictionary<Int32, Boolean> GetExistingorganizationUserIds = BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID).GetExistingorganizationUserIds(clinicalRotationDetailContract.CloneRotationId, CurrentUsersId);
                            if (GetExistingorganizationUserIds != null && GetExistingorganizationUserIds.Count > AppConsts.NONE)
                            {
                                AddApplicantsToRotation(clinicalRotationDetailContract.TenantID, clinicalRotationDetailContract.RotationID, GetExistingorganizationUserIds, currentUserID);
                            }
                        }
                        return clinicalRotationDetailContract.RotationID;
                    }
                    return AppConsts.NONE;
                }
                else
                {
                    String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                    Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(clinicalRotationDetailContract.TenantID).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                    .FirstOrDefault().RPS_ID;
                    String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                    Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(clinicalRotationDetailContract.TenantID).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                    .FirstOrDefault().RST_ID;
                    String reqPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
                    Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(clinicalRotationDetailContract.TenantID).Where(x => !x.RPT_IsDeleted
                                                                                                                            && x.RPT_Code == reqPkgTypeCode)
                                                                                                                            .FirstOrDefault().RPT_ID;
                    //UAT-2603
                    Int16 dataMovementDueStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(clinicalRotationDetailContract.TenantID, LkpSubscriptionMobilityStatus.DataMovementDue));
                    Int16 dataMovementNotRequiredStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(clinicalRotationDetailContract.TenantID, LkpSubscriptionMobilityStatus.DataMovementNotRequired));

                    //UAT-3108
                    //if (BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID).UpdateClinicalRotation(clinicalRotationDetailContract, customAttributeListToSave, currentUserID, syllabusDocumentTypeID, profileSynchSourceTypeID
                    //                                                                                                                 , reqPkgTypeId, rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, dataMovementDueStatusId, dataMovementNotRequiredStatusId))

                    RotationDetailFieldChanges rotationDetailFieldChanges = BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID).UpdateClinicalRotation(clinicalRotationDetailContract, customAttributeListToSave, currentUserID, syllabusDocumentTypeID, profileSynchSourceTypeID
                                                                                                                                     , reqPkgTypeId, rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, dataMovementDueStatusId, dataMovementNotRequiredStatusId, AdditionalDocumentTypeID);





                    if (rotationDetailFieldChanges.IsClinicalRotationUpdatedSuccessfully)
                    {
                        //Send mail for rotation update.
                        CommunicationManager.SendMailOnUpdationInRotationdetails(rotationDetailFieldChanges);//UAT0-3108

                        #region UAT-4561://check here if Rotation is approved/ also when only end date changes are there! then only send the below mentioned notification:
                        if (rotationDetailFieldChanges.IsNeedToSendEndDateMail)
                        {
                            CommunicationManager.SendMailOnUpdationInRotationEndDate(clinicalRotationDetailContract, rotationDetailFieldChanges);
                        }
                        #endregion

                        PerformParallelTaskForAgencyAndClientContacts(clinicalRotationDetailContract.RotationID.ToString(), clinicalRotationDetailContract.TenantID, currentUserID);
                        SaveCommunicationDataToSendMail(clinicalRotationDetailContract, SenderEmailID);
                        return clinicalRotationDetailContract.RotationID;

                    }
                    return AppConsts.NONE;
                }
                //UAT-2313

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-2313
        private static void PerformParallelTaskForAgencyAndClientContacts(String rotationIds, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
            dataDict.Add("rotationIds", rotationIds);
            dataDict.Add("tenantId", tenantId);
            dataDict.Add("currentLoggedInUserId", currentLoggedInUserId);
            //var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            //var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            ParallelTaskContext.PerformParallelTask(SyncRotationAgencyAndClientContacts, dataDict, null, null);
        }
        private static void SyncRotationAgencyAndClientContacts(Dictionary<String, Object> data)
        {
            String rotationIds = Convert.ToString(data.GetValue("rotationIds"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("currentLoggedInUserId"));
            BALUtils.GetClinicalRotationRepoInstance(tenantId).SyncRotationAgencyAndClientContacts(rotationIds, tenantId, currentLoggedInUserId);
        }
        #endregion

        private static bool SaveCommunicationDataToSendMail(ClinicalRotationDetailContract clinicalRotationDetailContract, String SenderEmailID)
        {
            if (!clinicalRotationDetailContract.ContactsToSendEmail.IsNullOrEmpty())
            {
                List<String> subEventCodes = new List<String>();
                subEventCodes.Add(AppConsts.CLINICAL_ROTATION_ASSIGNED_SUBEVNT_CODE.ToLower());
                Int32 subEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
                List<Entity.CommunicationTemplatePlaceHolder> placeHoldersToFetch = CommunicationManager.GetTemplatePlaceHolders(subEventID);
                Int32 templateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(subEventID);
                //Contains info for mail subject and content
                SystemEventTemplatesContract systemEventTemplate = TemplatesManager.GetTemplateDetails(templateId);

                String applicationUrl = WebSiteManager.GetInstitutionUrl(clinicalRotationDetailContract.TenantID);
                String sharedUserUrl = String.Empty;
                if (!WebConfigurationManager.AppSettings["shareduserloginurl"].IsNullOrEmpty())
                {
                    sharedUserUrl = string.Concat("http://", Convert.ToString(WebConfigurationManager.AppSettings["shareduserloginurl"]));
                }

                List<Entity.SystemCommunication> lstSystemCommunicationToBeSaved = new List<Entity.SystemCommunication>();

                foreach (ClientContactContract clientContactContract in clinicalRotationDetailContract.ContactsToSendEmail)
                {
                    //UAT-4005:- If instructor/Preceptor then url should be for inst/preceptor login rather than its institution url.

                    if (!clientContactContract.ClientContactID.IsNullOrEmpty() && clientContactContract.ClientContactID > AppConsts.NONE && !sharedUserUrl.IsNullOrEmpty())
                        applicationUrl = sharedUserUrl;

                    #region Set App Setting Contract
                    AppSettingContract appSettingContract = new AppSettingContract();
                    appSettingContract.ClientContactInvitationURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                                                                    ? String.Empty
                                                                                    : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

                    appSettingContract.SenderEmailID = System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];

                    List<ClientContactTypeContract> ClientContactTypeList = ClientContactManager.GetClientContactType();

                    String selectedUserTypeCode = ClientContactTypeList.Where(x => x.ClientContactTypeID == Convert.ToInt32(clientContactContract.ClientContactTypeID)).Select(sel => sel.Code).FirstOrDefault();
                    if (selectedUserTypeCode == ClientContactType.Instructor.GetStringValue())
                    {
                        appSettingContract.OrganizationUserType = OrganizationUserType.Instructor.GetStringValue();
                    }
                    else if (selectedUserTypeCode == ClientContactType.Preceptor.GetStringValue())
                    {
                        appSettingContract.OrganizationUserType = OrganizationUserType.Preceptor.GetStringValue();
                    }

                    #endregion

                    var queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.QUERY_STRING_CLIENTCONTACT_INVITE_TOKEN, Convert.ToString(clientContactContract.TokenID)},
                                                                    {AppConsts.QUERY_STRING_USER_TYPE_CODE, appSettingContract.OrganizationUserType}
                                                                 };

                    var activationlink = "http://" + String.Format(appSettingContract.ClientContactInvitationURL + "?args={0}", queryString.ToEncryptedQueryString());

                    Dictionary<String, String> dictMailData = new Dictionary<string, String>();
                    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, clientContactContract.Name);
                    dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, string.Concat(applicationUrl));
                    dictMailData.Add(EmailFieldConstants.SHARED_USER_URL, sharedUserUrl);
                    dictMailData.Add(EmailFieldConstants.ACTIVATION_LINK, activationlink);

                    Entity.SystemCommunication systemCommunication = new Entity.SystemCommunication();
                    systemCommunication.SenderName = "ADB Clinical Rotation System";
                    systemCommunication.SenderEmailID = SenderEmailID;
                    systemCommunication.Subject = systemEventTemplate.Subject;
                    systemCommunication.CommunicationSubEventID = subEventID;
                    systemCommunication.CreatedByID = 1;
                    systemCommunication.CreatedOn = DateTime.Now;
                    systemCommunication.Content = systemEventTemplate.TemplateContent;
                    //replace the placeholder
                    foreach (var placeHolder in placeHoldersToFetch)
                    {
                        Object obj = dictMailData.GetValue(placeHolder.Property);
                        systemCommunication.Content = systemCommunication.Content.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                    }

                    Entity.SystemCommunicationDelivery systemCommunicationDelivery = new Entity.SystemCommunicationDelivery();
                    systemCommunicationDelivery.SystemCommunicationTypeID = systemCommunication.SystemCommunicationID;
                    systemCommunicationDelivery.ReceiverOrganizationUserID = clientContactContract.ClientContactID;
                    systemCommunicationDelivery.RecieverEmailID = clientContactContract.Email;
                    systemCommunicationDelivery.RecieverName = clientContactContract.Name;
                    systemCommunicationDelivery.IsDispatched = false;
                    systemCommunicationDelivery.IsCC = null;
                    systemCommunicationDelivery.IsBCC = null;
                    systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
                    systemCommunicationDelivery.CreatedOn = DateTime.Now;
                    systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);
                    lstSystemCommunicationToBeSaved.Add(systemCommunication);
                }
                return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
            }
            return true;
        }

        /// <summary>
        /// Method to delete clinical rotation
        /// </summary>
        /// <returns></returns>
        public static Boolean DeleteClinicalRotation(Int32 clinicalRotationId, Int32 tenantId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).DeleteClinicalRotation(clinicalRotationId, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ArchiveClinicalRotation(List<Int32> clinicalRotationIds, Int32 tenantId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).ArchiveClinicalRotation(clinicalRotationIds, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-3138
        public static Boolean UnArchiveClinicalRotation(List<Int32> clinicalRotationIds, Int32 tenantId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).UnArchiveClinicalRotation(clinicalRotationIds, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #endregion

        #region Custom attribute
        /// <summary>
        /// Get Custom Attribute on the basis of Use Type Code.
        /// </summary>
        /// <param name="useTypeCode">useTypeCode</param>
        /// <returns>IQueryable</returns>
        public static List<CustomAttribteContract> GetCustomAttributeMappingList(Int32 tenantId, String useTypeCode, Int32? rotationID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetCustomAttributeMappingList(useTypeCode, rotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT 1304 : Instructor/Preceptor screens and functionality
        /// <summary>
        /// Returns the list of ClientSystemDocuments for all the rotations attached to Client Contact.
        /// </summary>
        /// <param name="clientContactID"></param>
        /// <param name="tenantId"></param>
        /// <returns>list of ClientSystemDocuments</returns>
        public static List<ClientContactSyllabusDocumentContract> GetClientContactRotationDocuments(Int32 clientContactID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClientContactRotationDocuments(clientContactID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Manage Invitations and Rotations for Shared User

        /// <summary>
        /// Get Clinical Rotations by Clinical Rotation IDs
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="clinicalRotationXML"></param>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetClinicalRotationsByIDs(Int32 tenantId, Int32 currentLoggedInUserId, String clinicalRotationXML, ClinicalRotationDetailContract clinicalRotationDetailContract,
                           CustomPagingArgsContract customPagingArgsContract = null)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationsByIDs(currentLoggedInUserId, clinicalRotationXML, clinicalRotationDetailContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Rotation Student Detail for Shared User
        public static List<ApplicantDataListContract> GetRotationStudents(RotationStudentDetailContract rotationStudentDetailContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                if (rotationStudentDetailContract.IsInstructor == true)
                {
                    List<ApplicantDataListContract> rotationStudents = BALUtils.GetClinicalRotationRepoInstance(rotationStudentDetailContract.SelectedTenantID).GetInstructorRotationStudentsDetail(customPagingArgsContract, rotationStudentDetailContract);
                    return rotationStudents;
                }
                else
                {
                    List<ApplicantDataListContract> applicantDataList = BALUtils.GetSharedUserClinicalRotationRepoInstance()
                                                                               .GetApplicantIDsForRotationAndInvGrp(rotationStudentDetailContract);

                    //Set Default sorting direction.
                    customPagingArgsContract.SortDirectionDescending = customPagingArgsContract.SortDirectionDescending == false ?
                                                                     String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? true :
                                                                     customPagingArgsContract.SortDirectionDescending : customPagingArgsContract.SortDirectionDescending;
                    String applicantUserIDsXML = CreateApplicantUserIDsXML(applicantDataList);
                    List<ApplicantDataListContract> rotationStudents = BALUtils.GetClinicalRotationRepoInstance(rotationStudentDetailContract.SelectedTenantID).GetRotationStudentsDetail(customPagingArgsContract, applicantUserIDsXML, rotationStudentDetailContract);

                    foreach (var item in rotationStudents)
                    {
                        if (item.IsDropped)
                            item.ReviewBy = String.Empty;
                        else
                            item.ReviewBy = applicantDataList.Where(f => f.ProfileSharingInvID == item.ProfileSharingInvID).Select(g => g.ReviewBy).FirstOrDefault();
                    }

                    return rotationStudents;
                }


            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static String CreateApplicantUserIDsXML(List<ApplicantDataListContract> applicantDataListContract)
        {
            //XElement xmlElements = new XElement("OrganizationUsers", applicantDataListContract
            //                        .Select(i => new XElement("OrganizationUser",
            //                             new XAttribute("OrganizationUserID", i.OrganizationUserId),
            //                             new XAttribute("PSI_ID", i.ProfileSharingInvID),
            //                             new XAttribute("InvitationDate", i.InvitationDate)
            //                             ))
            //                             );
            //xmlElements.ToString();

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<OrganizationUsers>");
            foreach (var applicantData in applicantDataListContract)
            {
                strBuilder.Append("<OrganizationUser>");
                strBuilder.Append("<OrganizationUserID>" + applicantData.OrganizationUserId.ToString() + "</OrganizationUserID>");
                strBuilder.Append("<ProfileSharingInvID>" + applicantData.ProfileSharingInvID.ToString() + "</ProfileSharingInvID>");
                if (applicantData.InvitationDate.HasValue)
                    strBuilder.Append("<InvitationDate>" + applicantData.InvitationDate.Value.ToString("MM/dd/yyyy HH:mm:ss") + "</InvitationDate>");
                if (applicantData.ExpirationDate.HasValue)
                    strBuilder.Append("<ExpirationDate>" + applicantData.ExpirationDate.Value.ToString("MM/dd/yyyy HH:mm:ss") + "</ExpirationDate>");
                if (applicantData.ViewsRemaining.HasValue)
                    strBuilder.Append("<ViewsRemaining>" + applicantData.ViewsRemaining.ToString() + "</ViewsRemaining>");
                strBuilder.Append("<IsInvitationVisible>" + applicantData.IsInvitationVisible.ToString() + "</IsInvitationVisible>");
                strBuilder.Append("<IsApplicant>" + applicantData.IsApplicant.ToString() + "</IsApplicant>");
                strBuilder.Append("<InvitationReviewStatus>" + applicantData.InvitationReviewStatus.ToString() + "</InvitationReviewStatus>");
                strBuilder.Append("<InvitationReviewStatusCode>" + applicantData.InvitationReviewStatusCode.ToString() + "</InvitationReviewStatusCode>");
                strBuilder.Append("</OrganizationUser>");
            }
            strBuilder.Append("</OrganizationUsers>");
            return strBuilder.ToString();
        }

        /// <summary>
        /// Get Rotation Student details for Instructor Preceptor
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentUserID"></param>
        /// <param name="searchContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        public static List<RotationMemberSearchDetailContract> GetInstrctrPreceptrRotationStudents(String tenantId, String currentUserID, RotationMemberSearchDetailContract searchContract,
                           CustomPagingArgsContract customPagingArgsContract = null)
        {
            try
            {
                Guid userID = new Guid(currentUserID);
                return BALUtils.GetSecurityInstance().GetInstrctrPreceptrRotationStudents(tenantId, userID, searchContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Entity to Contract conversion methods

        private static OrganizationUserContract ConvertEntityToOrganizationUserContract(usp_GetUserDetails_Result organizationUser)
        {
            OrganizationUserContract organizationUserContract = new OrganizationUserContract();
            organizationUserContract.FirstName = organizationUser.FirstName;
            organizationUserContract.MiddleName = organizationUser.MiddleName;
            organizationUserContract.LastName = organizationUser.LastName;
            organizationUserContract.Email = organizationUser.PrimaryEmail;
            organizationUserContract.DateOfBirth = organizationUser.DateOfBirth.IsNull() ? (DateTime?)null : Convert.ToDateTime(organizationUser.DateOfBirth);
            organizationUserContract.UserName = organizationUser.UserName;
            organizationUserContract.Phone = organizationUser.PhoneNo;
            organizationUserContract.Address1 = organizationUser.Address1;
            organizationUserContract.Address2 = organizationUser.Address2;
            organizationUserContract.City = organizationUser.CityName;
            organizationUserContract.State = organizationUser.StateName;
            organizationUserContract.Country = organizationUser.CountryName;
            organizationUserContract.ZipCode = organizationUser.ZipCode;
            organizationUserContract.County = organizationUser.CountyName;

            return organizationUserContract;
        }

        #endregion

        #region UAT-1344:Automated NPI Number association and agency creation
        public static List<AgencyDataContract> SaveUpdateAgencyInBulk(String xmlData, Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().SaveUpdateAgencyInBulk(xmlData, organizationUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static Boolean CreateRotationSubscriptionForClientContact(List<Int32> clientContactIds, Int32 clinicalRotationID, Int32 currentLoggedInUserId, Int32 selectedTenantId)
        {
            try
            {
                String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(selectedTenantId).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                .FirstOrDefault().RPS_ID;
                String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(selectedTenantId).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                .FirstOrDefault().RST_ID;
                String reqPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
                Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(selectedTenantId).Where(x => !x.RPT_IsDeleted
                                                                                                                        && x.RPT_Code == reqPkgTypeCode)
                                                                                                                        .FirstOrDefault().RPT_ID;
                List<RequirementPackageSubscription> lstRequirementPackageSubscriptionToBeAdded = BALUtils.GetClinicalRotationRepoInstance(selectedTenantId).CreateRotationSubscriptionForClientContact(clientContactIds, clinicalRotationID, reqPkgTypeId
                                                                                                                                , rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, currentLoggedInUserId);
                if (BALUtils.GetClinicalRotationRepoInstance(selectedTenantId).SaveContextIntoDataBase())
                {
                    BALUtils.GetClinicalRotationRepoInstance(selectedTenantId).CreateOptionalCategorySetAproved(lstRequirementPackageSubscriptionToBeAdded, currentLoggedInUserId);

                    //UAT-2603
                    List<Int32> lstRequirementPackageSubscriptionIds = lstRequirementPackageSubscriptionToBeAdded.Select(sel => sel.RPS_ID).ToList();
                    Int16 dataMovementDuestatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(selectedTenantId, LkpSubscriptionMobilityStatus.DataMovementDue));
                    BALUtils.GetClinicalRotationRepoInstance(selectedTenantId).AddDataToRotDataMovement(lstRequirementPackageSubscriptionIds, currentLoggedInUserId, dataMovementDuestatusId);
                    return true;
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateRotationSubscriptionForClientContact(Int32 clinicalRotationID, Int32 oldPkgId, Int32 newPkgId, Int32 currentLoggedInUserId, Int32 selectedTenantId)
        {
            try
            {
                String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(selectedTenantId).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                .FirstOrDefault().RPS_ID;
                String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(selectedTenantId).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                .FirstOrDefault().RST_ID;
                //UAT-2603
                Int16 dataMovementDueStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(selectedTenantId, LkpSubscriptionMobilityStatus.DataMovementDue));

                BALUtils.GetClinicalRotationRepoInstance(selectedTenantId).UpdateRotationSubscriptionForClientContact(clinicalRotationID, currentLoggedInUserId, oldPkgId, newPkgId, rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, dataMovementDueStatusId);
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        public static void SynchronizeClientContactProfiles(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentUserId, Int32 tenantID)
        {
            try
            {
                String profileSynchAddToRotationCode = ProfileSynchSourceType.CLIENTCONTACT_REGISTRATION.GetStringValue();
                Int32 profileSynchSourceTypeID = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpProfileSharingInvitationGroupType>().Where(cond => cond.PSIGT_Code == profileSynchAddToRotationCode).Select(col => col.PSIGT_ID).FirstOrDefault();
                List<ClientContactContract> syncContacts = BALUtils.GetClinicalRotationRepoInstance(tenantID).SynchronizeClientContactProfiles(null, currentUserId, profileSynchSourceTypeID, tenantID);
                //Create rotation subscription for client.
                if (syncContacts.Count > AppConsts.NONE)
                {
                    List<Int32> clientContactIds = syncContacts.Select(sel => sel.ClientContactID).ToList();

                    List<Entity.SharedDataEntity.ClinicalRotationClientContactMapping> lstRotationMappedToContacts = BALUtils.GetClinicalRotationRepoInstance(tenantID).lstRotationMappedToContacts(clientContactIds.FirstOrDefault(), tenantID);

                    if (lstRotationMappedToContacts.Count > AppConsts.NONE)
                    {
                        String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                        String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                        String reqPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();

                        Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(tenantID).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                        .FirstOrDefault().RPS_ID;

                        Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(tenantID).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                        .FirstOrDefault().RST_ID;

                        Int32 reqPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantID).Where(x => !x.RPT_IsDeleted
                                                                                                                                && x.RPT_Code == reqPkgTypeCode)
                                                                                                                                .FirstOrDefault().RPT_ID;
                        List<RequirementPackageSubscription> lstRequirementPackageSubscriptionToBeAdded = new List<RequirementPackageSubscription>();
                        foreach (var rotation in lstRotationMappedToContacts)
                        {

                            lstRequirementPackageSubscriptionToBeAdded.AddRange(BALUtils.GetClinicalRotationRepoInstance(tenantID).CreateRotationSubscriptionForClientContact(clientContactIds, rotation.CRCCM_ClinicalRotationID, reqPkgTypeId
                                                                                                                                   , rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, currentUserId));
                        }
                        if (BALUtils.GetClinicalRotationRepoInstance(tenantID).SaveContextIntoDataBase())
                        {
                            BALUtils.GetClinicalRotationRepoInstance(tenantID).CreateOptionalCategorySetAproved(lstRequirementPackageSubscriptionToBeAdded, currentUserId);
                            //Added IN UAT-4960
                            Int16 dataMovementDuestatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantID, LkpSubscriptionMobilityStatus.DataMovementDue));
                            List<Int32> lstRequirementPackageSubscriptionIds = new List<Int32>();
                            lstRequirementPackageSubscriptionIds = lstRequirementPackageSubscriptionToBeAdded.Select(Sel => Sel.RPS_ID).ToList();
                            BALUtils.GetClinicalRotationRepoInstance(tenantID).AddDataToRotDataMovement(lstRequirementPackageSubscriptionIds, currentUserId, dataMovementDuestatusId);
                            //END UAT-4960
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IfAnyContactIsMappedToRotation(Int32 rotationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).IfAnyContactIsMappedToRotation(rotationId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IfAnyContactHasEnteredDataForRotation(Int32 packageId, Int32 clinicalRotationID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).IfAnyContactHasEnteredDataForRotation(packageId, clinicalRotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1362:As an Instructor/Preceptor I should be able to enter data for my rotation requirements package
        public static Int32 GetRequirementSubscriptionIdByClinicalRotID(Int32 tenantId, Int32 clinicalRotationID, Int32 orgUserID)
        {
            try
            {
                Int32 instPrecpPackageTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantId).FirstOrDefault(x => x.RPT_Code == "AAAB"
                                               && x.RPT_IsDeleted == false).RPT_ID;
                Int32 rotReqSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(tenantId).FirstOrDefault(x => x.RST_Code == "AAAA"
                                               && x.RST_IsDeleted == false).RST_ID;
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRequirementSubscriptionIdByClinicalRotID(clinicalRotationID, rotReqSubscriptionTypeID, instPrecpPackageTypeID, orgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        /// <summary>
        /// Check whether the selected Agency is associated with any clinical rotation in any Tenant 
        /// </summary>
        /// <param name="lstAgencyInstitutions"></param>
        /// <returns></returns>
        public static Boolean IsAgencyAssociated(List<AgencyInstitution> lstAgencyInstitutions)
        {
            try
            {
                var _isAgencyAssociated = false;
                foreach (var agencyInst in lstAgencyInstitutions)
                {
                    var _isAssociated = BALUtils.GetClinicalRotationRepoInstance(Convert.ToInt32(agencyInst.AGI_TenantID)).IsAgencyAssociated(Convert.ToInt32(agencyInst.AGI_AgencyID));

                    if (_isAssociated)
                    {
                        _isAgencyAssociated = true;
                        return _isAssociated;
                    }
                }
                return _isAgencyAssociated;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range

        public static List<ApplicantDocumentContract> GetApplicantDocumentToExport(List<RotationMemberSearchDetailContract> lstRotationMemberData, Int32 tenantId)
        {
            try
            {
                String rotationXMl = CreateApplicantRotationXML(lstRotationMemberData);
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetApplicantDocumentToExport(rotationXMl);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsAgenycHierarchyAvailable(Int32 selectedTenantId, String parameter)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(selectedTenantId).IsAgenycHierarchyAvailable(parameter);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Create Applicant rotation XML
        /// </summary>
        /// <param name="lstClinicalRotationDetailContract"></param>
        /// <returns></returns>
        private static String CreateApplicantRotationXML(List<RotationMemberSearchDetailContract> lstRotationMemberData)
        {
            XElement xmlElements = new XElement("ApplicantRotations", lstRotationMemberData
                                    .Select(i => new XElement("ApplicantRotation",
                                         new XAttribute("ClinicalRotationID", i.RotationID),
                                         new XAttribute("UserID", i.OrganizationUserID)))
                                         );
            return xmlElements.ToString();
        }

        public static List<RotationMemberSearchDetailContract> GetRotationMemberSearchData(RotationMemberSearchDetailContract clinicalRotationDetailContract
                                                                                , CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(clinicalRotationDetailContract.TenantID)
                               .GetRotationMemberSearchData(clinicalRotationDetailContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 GetSubscriptionIdByRotIDAndUserID(Int32 tenantId, Int32 clinicalRotationID, Int32 orgUserID, String isApplicant)
        {
            try
            {
                Int32 reSubscriptionPkgTypeId = AppConsts.NONE;
                //For Rotation type package
                if (Convert.ToBoolean(isApplicant))
                {
                    reSubscriptionPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(tenantId).FirstOrDefault(x => x.RST_Code == "AAAA"
                                                   && x.RST_IsDeleted == false).RST_ID;
                }
                //For Instructor/Preceptor type package 
                else
                {
                    reSubscriptionPkgTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantId).FirstOrDefault(x => x.RPT_Code == "AAAB"
                                               && x.RPT_IsDeleted == false).RPT_ID;
                }
                Int32 rotReqSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(tenantId).FirstOrDefault(x => x.RST_Code == "AAAA"
                                               && x.RST_IsDeleted == false).RST_ID;
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetSubscriptionIdByRotIDAndUserID(clinicalRotationID, reSubscriptionPkgTypeId, rotReqSubscriptionTypeID, orgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT 1409 : The Agency User should be able to filter their search by those rotations that are active or completed (expired) AND by pending revieew or reviewed.
        public static List<SharedUserRotationReviewStatusContract> GetRotationReviewStatus(Int32 tenantID)
        {
            try
            {
                String sharedUserInvDroppedReviewStatus = SharedUserInvitationReviewStatus.Dropped.GetStringValue(); //UAT-4460
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpSharedUserRotationReviewStatu>(tenantID).Where(cond => cond.SURRS_Code != sharedUserInvDroppedReviewStatus).Select(col =>
                                         new SharedUserRotationReviewStatusContract
                                         {
                                             RotationReviewStatusID = col.SURRS_ID,
                                             Name = col.SURRS_Name,
                                             Description = col.SURRS_Description,
                                             Code = col.SURRS_Code
                                         }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Save and Update the Review Status in SharedUserReviewStatus table.
        /// </summary>
        /// <param name="lstRotationReviewContract">List of SharedUserReviewStatus contract.</param>
        /// <param name="RotaionReviewStatusID">Selected Review Status ID</param>
        /// <param name="OrganizationUserID">Logged in user ID.</param>
        /// <returns></returns>
        public static Boolean SaveUpdateRotationReviewStatus(List<SharedUserRotationReviewContract> lstRotationReviewContract, Int32 RotaionReviewStatusID, Int32 OrganizationUserID)
        {
            try
            {
                Boolean isSavedSuccessfully = false;
                Boolean isUpdatedSuccessfully = false;
                lstRotationReviewContract.DistinctBy(cond => cond.TenantID).Select(col => col.TenantID).ForEach(tenantID =>
                {
                    //Get the rotation corressponding to a tenant.
                    List<SharedUserRotationReviewContract> lstCurrentTenantRotations = lstRotationReviewContract.Where(cond => cond.TenantID == tenantID).ToList();

                    List<SharedUserRotationReviewContract> lstRotationsReviewToInsert = lstCurrentTenantRotations.Where(x => x.SharedUserRotationReviewID == AppConsts.NONE).ToList();
                    List<SharedUserRotationReviewContract> lstRotationsReviewToUpdate = lstCurrentTenantRotations.Where(x => x.SharedUserRotationReviewID > AppConsts.NONE).ToList();

                    //InsertRecords
                    if (!lstRotationsReviewToInsert.IsNullOrEmpty())
                    {
                        List<SharedUserRotationReview> lstSharedUserRotationReview = new List<SharedUserRotationReview>();
                        foreach (SharedUserRotationReviewContract item in lstRotationsReviewToInsert)
                        {
                            //Insert Mode
                            SharedUserRotationReview sharedUserRotationReview = new SharedUserRotationReview();
                            sharedUserRotationReview.SURR_ClinicalRotaionID = item.ClinicalRotaionID;
                            sharedUserRotationReview.SURR_OrganizationUserID = OrganizationUserID;
                            sharedUserRotationReview.SURR_RotationReviewStatusID = RotaionReviewStatusID;
                            sharedUserRotationReview.SURR_IsDeleted = false;
                            sharedUserRotationReview.SURR_CreatedOn = DateTime.Now;
                            sharedUserRotationReview.SURR_CreatedByID = OrganizationUserID;

                            //Add in list to insert
                            lstSharedUserRotationReview.Add(sharedUserRotationReview);
                        }
                        isSavedSuccessfully = BALUtils.GetClinicalRotationRepoInstance(tenantID).SaveUpdateRotationReviewStatus(lstSharedUserRotationReview, false);
                    }

                    if (!lstRotationsReviewToUpdate.IsNullOrEmpty())
                    {
                        //Fetch the previous records.
                        List<Int32> lstSaredUserRotationReviewID = lstRotationsReviewToUpdate.Select(col => col.SharedUserRotationReviewID).ToList();
                        List<SharedUserRotationReview> lstSharedUserRotationReview = BALUtils.GetClinicalRotationRepoInstance(tenantID).GetShareduserRotationReviewStatusByIds(lstSaredUserRotationReviewID);

                        foreach (SharedUserRotationReview item in lstSharedUserRotationReview)
                        {
                            //Update Mode
                            item.SURR_ClinicalRotaionID = item.SURR_ClinicalRotaionID;
                            item.SURR_OrganizationUserID = OrganizationUserID;
                            item.SURR_RotationReviewStatusID = RotaionReviewStatusID;
                            item.SURR_IsDeleted = false;
                            item.SURR_ModifiedOn = DateTime.Now;
                            item.SURR_ModifiedByID = OrganizationUserID;
                        }
                        isUpdatedSuccessfully = BALUtils.GetClinicalRotationRepoInstance(tenantID).SaveUpdateRotationReviewStatus(lstSharedUserRotationReview, true);
                    }
                });

                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        public static List<ClinicalRotationMemberDetail> GetRotationMemberDetailForNagMail(Int32 subEventId, Int32 chunkSize, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRotationMemberDetailForNagMail(subEventId, chunkSize);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AttestationDocumentContract> GetAttestationDocumentsToExport(ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> ServiceReqData, Int32 loggedInUserId)
        {
            try
            {
                ServiceRequest<Dictionary<String, List<Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, List<Int32>>>();
                serviceRequest.Parameter = new Dictionary<String, List<Int32>>();
                foreach (var item in ServiceReqData.Parameter1)
                {
                    List<Int32> lstItem = new List<Int32>() { { item.Value } };
                    serviceRequest.Parameter.Add(item.Key, lstItem);
                }
                //Casting tuple into Contract as per the requirement of UAT:2475
                List<InvitationIDsDetailContract> tempRotationIdsContract = ServiceReqData.Parameter2.Select(con => new InvitationIDsDetailContract { RotationID = con.Item1, TenantID = con.Item2, AgencyID = con.Item3 }).ToList();
                return GetSelectedAttestationDocumentsToExport(serviceRequest, loggedInUserId, tempRotationIdsContract);

                //List<AttestationDocumentContract> lstDocContract = new List<AttestationDocumentContract>();
                //List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrp = new List<ProfileSharingInvitationGroup>();
                //String AttestationWithoutSign = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();
                //bool OnlyUnSignedExcel = false;
                //bool IgnoreAgencyUserCheck = false;
                //Int32 AgencyUserId = 0;
                //List<Int32> rotationIDs;
                //Int32 profileSharingInvitationGroupID;
                //Int32 profileSharingInvitationID = 0;
                //bool IsAdmin = false;

                //ServiceReqData.Parameter.TryGetValue(AppConsts.IS_ADMIN, out IsAdmin);
                //ServiceReqData.Parameter.TryGetValue(AppConsts.IGNORE_AGENCY_USER_CHECK, out IgnoreAgencyUserCheck);
                //ServiceReqData.Parameter.TryGetValue(AppConsts.PROFILE_SHARING_INVITATION_ID, out profileSharingInvitationID);
                //ServiceReqData.Parameter.TryGetValue(AppConsts.ONLY_UNSIGNED_EXCEL, out OnlyUnSignedExcel);

                //if (!IgnoreAgencyUserCheck)
                //{
                //    if (!ServiceReqData.Parameter.TryGetValue(AppConsts.AGENCY_USER_ID, out AgencyUserId))
                //    {
                //        AgencyUserId = loggedInUserId;
                //    }
                //}

                //if (ServiceReqData.Parameter.TryGetValue(AppConsts.ROTATION_ID, out rotationIDs))
                //{
                //    lstProfileSharingInvitationGrp = BALUtils.GetSharedUserClinicalRotationRepoInstance().GetCheckedAttestationDocumentForRotation(rotationIDs);
                //}
                //else if (ServiceReqData.Parameter.TryGetValue(AppConsts.PROFILE_SHARING_INVITATION_GROUP_ID, out profileSharingInvitationGroupID))
                //{
                //    lstProfileSharingInvitationGrp = BALUtils.GetSharedUserClinicalRotationRepoInstance().GetAttestationDocumentForInvitationGroup(profileSharingInvitationGroupID);
                //}
                //else if (ServiceReqData.Parameter.TryGetValue(AppConsts.PROFILE_SHARING_INVITATION_ID, out profileSharingInvitationID))
                //{
                //    lstProfileSharingInvitationGrp = BALUtils.GetSharedUserClinicalRotationRepoInstance().GetAttestationDocForProfileSharingInvitaiton(profileSharingInvitationID);
                //}
                //else
                //{
                //    throw new SysXException("Neither Rotation ID was supplied nor ProfileSharingInvitationGroupID");
                //}

                //foreach (ProfileSharingInvitationGroup PSIG in lstProfileSharingInvitationGrp)
                //{
                //    Boolean isAllInvitationsExpired = true;
                //    List<ProfileSharingInvitation> lstProfileSharingInvitation = PSIG.ProfileSharingInvitations
                //                                                         .Where(cond => cond.PSI_InviteeOrgUserID.HasValue
                //                                                          && (cond.PSI_InviteeOrgUserID.Value == AgencyUserId || IgnoreAgencyUserCheck)
                //                                                          && (cond.PSI_ID == profileSharingInvitationID || profileSharingInvitationID == 0)
                //                                                          && !cond.PSI_IsDeleted).ToList();
                //    if (IsAdmin == true)
                //    {
                //        isAllInvitationsExpired = false;
                //    }
                //    else
                //    {
                //        foreach (ProfileSharingInvitation PSI in lstProfileSharingInvitation)
                //        {
                //            if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue())
                //            {
                //                isAllInvitationsExpired = false;
                //                break;
                //            }
                //            else if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue())
                //            {
                //                if ((PSI.PSI_MaxViews - PSI.PSI_InviteeViewCount) != 0)
                //                {
                //                    isAllInvitationsExpired = false;
                //                    break;
                //                }
                //            }
                //            else if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue())
                //            {
                //                if (PSI.PSI_ExpirationDate > DateTime.Now)
                //                {
                //                    isAllInvitationsExpired = false;
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //    if (!isAllInvitationsExpired)
                //    {
                //        List<Int32> lstProfileSharingInvitationIDs = lstProfileSharingInvitation.Select(col => col.PSI_ID).ToList();


                //        List<InvitationDocumentMapping> docMappingList = BALUtils.GetSharedUserClinicalRotationRepoInstance().GetInvitationDocMappingForInvitaitonID(lstProfileSharingInvitationIDs);

                //        foreach (InvitationDocumentMapping docMapping in docMappingList)
                //        {
                //            AttestationDocumentContract docContract = new AttestationDocumentContract()
                //            {
                //                DocumentFilePath = docMapping.InvitationDocument.IND_DocumentFilePath,
                //                InvitationDocumentID = docMapping.InvitationDocument.IND_ID,
                //                InvitationDocumentMappingID = docMapping.IDM_ID,
                //                ProfileSharingInvitationGroupID = PSIG.PSIG_ID,
                //                ProfileSharingInvitationID = docMapping.IDM_ProfileSharingInvitationID.Value,
                //                SharedSystemDocumentTypecode = docMapping.InvitationDocument.lkpSharedSystemDocType.SSDT_Code

                //            };
                //            if (docMapping.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue())
                //            {
                //                docContract.IsVerticalAttestation = true;
                //            }
                //            lstDocContract.Add(docContract);
                //        }
                //    }
                //}
                //if (OnlyUnSignedExcel)
                //{
                //    return lstDocContract.Where(con => con.SharedSystemDocumentTypecode == AttestationWithoutSign).ToList();
                //}
                //else
                //{
                //    return lstDocContract.Where(con => con.SharedSystemDocumentTypecode != AttestationWithoutSign).ToList();
                //}

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// UAT-2035 Get selected attesation report to export.
        /// </summary>
        /// <param name="ServiceReqData"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public static List<AttestationDocumentContract> GetSelectedAttestationDocumentsToExport(ServiceRequest<Dictionary<String, List<Int32>>> ServiceReqData, Int32 loggedInUserId, List<InvitationIDsDetailContract> lstRotationTenant)
        {
            try
            {
                List<AttestationDocumentContract> lstDocContract = new List<AttestationDocumentContract>();
                List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrp = new List<ProfileSharingInvitationGroup>();
                String AttestationWithoutSign = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();
                List<Int32> OnlyUnSigned = new List<Int32>();
                Boolean OnlyUnSignedExcel;
                List<Int32> rotationIDs = new List<Int32>();
                List<Int32> profileSharingInvitationGroupID = new List<Int32>();
                List<Int32> profileSharingInvitationID = new List<Int32>();

                ServiceReqData.Parameter.TryGetValue(AppConsts.PROFILE_SHARING_INVITATION_ID, out profileSharingInvitationID);
                ServiceReqData.Parameter.TryGetValue(AppConsts.ONLY_UNSIGNED_EXCEL, out OnlyUnSigned);

                OnlyUnSignedExcel = OnlyUnSigned.IsNullOrEmpty() ? false : Convert.ToBoolean(OnlyUnSigned.Take(1).FirstOrDefault());

                if (ServiceReqData.Parameter.TryGetValue(AppConsts.ROTATION_ID, out rotationIDs))
                {
                    lstProfileSharingInvitationGrp = BALUtils.GetSharedUserClinicalRotationRepoInstance().GetAttestationDocumentForRotation(lstRotationTenant);
                }
                else if (ServiceReqData.Parameter.TryGetValue(AppConsts.PROFILE_SHARING_INVITATION_GROUP_ID, out profileSharingInvitationGroupID))
                {
                    lstProfileSharingInvitationGrp = BALUtils.GetSharedUserClinicalRotationRepoInstance().GetAttestationDocumentForInvitationGroup(profileSharingInvitationGroupID);
                }
                else if (ServiceReqData.Parameter.TryGetValue(AppConsts.PROFILE_SHARING_INVITATION_ID, out profileSharingInvitationID))
                {
                    lstProfileSharingInvitationGrp = BALUtils.GetSharedUserClinicalRotationRepoInstance().GetAttestationDocForProfileSharingInvitaiton(profileSharingInvitationID);
                }
                else
                {
                    throw new SysXException("Neither Rotation ID was supplied nor ProfileSharingInvitationGroupID");
                }

                //Send selected checks to service data in an int form .
                ServiceRequest<Dictionary<String, Int32>> ServiceData = new ServiceRequest<Dictionary<String, Int32>>();
                ServiceData.Parameter = new Dictionary<string, int>();
                foreach (var item in ServiceReqData.Parameter)
                {
                    if (item.Key == AppConsts.PROFILE_SHARING_INVITATION_ID || item.Key == AppConsts.IGNORE_AGENCY_USER_CHECK
                        || item.Key == AppConsts.IS_ADMIN || item.Key == AppConsts.AGENCY_USER_ID)
                    {
                        ServiceData.Parameter.Add(item.Key, item.Value.Take(1).FirstOrDefault());
                    }
                }

                lstDocContract = GetAttestationDocumentContract(ServiceData, lstProfileSharingInvitationGrp, loggedInUserId);

                if (OnlyUnSignedExcel)
                {
                    return lstDocContract.Where(con => con.SharedSystemDocumentTypecode == AttestationWithoutSign).ToList();
                }
                else
                {
                    return lstDocContract.Where(con => con.SharedSystemDocumentTypecode != AttestationWithoutSign).ToList();
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get the attestation document contract on the basis of checks
        /// </summary>
        /// <param name="ServiceReqData"></param>
        /// <param name="lstProfileSharingInvitationGrp"></param>
        /// <param name="loggedInUserID"></param>
        /// <returns></returns>
        private static List<AttestationDocumentContract> GetAttestationDocumentContract(ServiceRequest<Dictionary<String, Int32>> ServiceReqData, List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrp, Int32 loggedInUserID)
        {
            try
            {
                List<AttestationDocumentContract> lstDocContract = new List<AttestationDocumentContract>();
                Boolean IsAdmin = false;
                Boolean IgnoreAgencyUserCheck = false;
                Int32 AgencyUserId = 0;
                Int32 profileSharingInvitationID = 0;

                ServiceReqData.Parameter.TryGetValue(AppConsts.PROFILE_SHARING_INVITATION_ID, out profileSharingInvitationID);
                ServiceReqData.Parameter.TryGetValue(AppConsts.IGNORE_AGENCY_USER_CHECK, out IgnoreAgencyUserCheck);
                ServiceReqData.Parameter.TryGetValue(AppConsts.IS_ADMIN, out IsAdmin);

                if (!IgnoreAgencyUserCheck)
                {
                    if (!ServiceReqData.Parameter.TryGetValue(AppConsts.AGENCY_USER_ID, out AgencyUserId))
                    {
                        AgencyUserId = loggedInUserID;
                    }
                }

                foreach (ProfileSharingInvitationGroup PSIG in lstProfileSharingInvitationGrp)
                {
                    Boolean isAllInvitationsExpired = true;
                    List<ProfileSharingInvitation> lstProfileSharingInvitation = PSIG.ProfileSharingInvitations
                                                                         .Where(cond => cond.PSI_InviteeOrgUserID.HasValue
                                                                          && (cond.PSI_InviteeOrgUserID.Value == AgencyUserId || IgnoreAgencyUserCheck)
                                                                          && (cond.PSI_ID == profileSharingInvitationID || profileSharingInvitationID == 0)
                                                                          && !cond.PSI_IsDeleted).ToList();
                    if (IsAdmin == true)
                    {
                        isAllInvitationsExpired = false;
                    }
                    else
                    {
                        foreach (ProfileSharingInvitation PSI in lstProfileSharingInvitation)
                        {
                            if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue())
                            {
                                isAllInvitationsExpired = false;
                                break;
                            }
                            else if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue())
                            {
                                if ((PSI.PSI_MaxViews - PSI.PSI_InviteeViewCount) != 0)
                                {
                                    isAllInvitationsExpired = false;
                                    break;
                                }
                            }
                            else if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue())
                            {
                                if (PSI.PSI_ExpirationDate > DateTime.Now)
                                {
                                    isAllInvitationsExpired = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (!isAllInvitationsExpired)
                    {
                        List<Int32> lstProfileSharingInvitationIDs = lstProfileSharingInvitation.Select(col => col.PSI_ID).ToList();


                        List<InvitationDocumentMapping> docMappingList = BALUtils.GetSharedUserClinicalRotationRepoInstance().GetInvitationDocMappingForInvitaitonID(lstProfileSharingInvitationIDs);

                        foreach (InvitationDocumentMapping docMapping in docMappingList)
                        {
                            AttestationDocumentContract docContract = new AttestationDocumentContract()
                            {
                                DocumentFilePath = docMapping.InvitationDocument.IND_DocumentFilePath,
                                InvitationDocumentID = docMapping.InvitationDocument.IND_ID,
                                InvitationDocumentMappingID = docMapping.IDM_ID,
                                ProfileSharingInvitationGroupID = PSIG.PSIG_ID,
                                ProfileSharingInvitationID = docMapping.IDM_ProfileSharingInvitationID.Value,
                                SharedSystemDocumentTypecode = docMapping.InvitationDocument.lkpSharedSystemDocType.SSDT_Code

                            };
                            if (docMapping.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue())
                            {
                                docContract.IsVerticalAttestation = true;
                            }
                            lstDocContract.Add(docContract);
                        }
                    }
                }
                return lstDocContract;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Dictionary<Int32, String> GetDefaultPermissionForClientAdmin(Int32 selectedTenantID, Int32 currentUserID)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(selectedTenantID).GetDefaultPermissionForClientAdmin(currentUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1701, New Agency User Search
        /// <summary>
        /// UAT-1701, New Agency User Search
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="clinicalRotationXML"></param>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetClinicalRotationsWithStudentByIDs(int tenantId, int currentLoggedInUserId, string clinicalRotationXML, ClinicalRotationDetailContract clinicalRotationDetailContract, String customAttributeXML)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationsWithStudentByIDs(currentLoggedInUserId, clinicalRotationXML, clinicalRotationDetailContract, customAttributeXML);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static bool GetRequirementPackageStatusByRotationID(int tenantID, int RotationID, String RotationMemberIds)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRequirementPackageStatusByRotationID(RotationID, RotationMemberIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AgencyDetailContract> GetAgenciesFromAllTenants()
        {
            try
            {
                List<Agency> agencyList = BALUtils.GetProfileSharingRepoInstance().GetAgenciesFromAllTenants();
                return agencyList.Select(col => new AgencyDetailContract
                {
                    AgencyID = col.AG_ID,
                    AgencyName = col.AG_Name,
                }).OrderBy(x => x.AgencyName).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        public static Boolean SaveUpdateUserRotationReviewStatus(int tenantID, int RotationID, Int32 currentLoggedInUserId, Int32 inviteeOrgUserId, String reviewStatusCode, Int32 agencyID, Int32? lastReviewedByID, Boolean isRotationReviewStatusUpdatingWhileRS, Boolean isAdminLoggedInAsAgencyUser = false)
        {
            try
            {
                Int32 reviewStatusId = LookupManager.GetLookUpData<lkpSharedUserRotationReviewStatu>(tenantID).FirstOrDefault(cnd => cnd.SURRS_Code == reviewStatusCode
                                                                                                   && !cnd.SURRS_IsDeleted).SURRS_ID;

                Int32 approvedReviewStatusID = LookupManager.GetLookUpData<lkpSharedUserRotationReviewStatu>(tenantID).FirstOrDefault(cnd => cnd.SURRS_Code == SharedUserRotationReviewStatus.APPROVED.GetStringValue()
                                                                                                   && !cnd.SURRS_IsDeleted).SURRS_ID;

                return BALUtils.GetClinicalRotationRepoInstance(tenantID).SaveUpdateUserRotationReviewStatus(RotationID, currentLoggedInUserId, inviteeOrgUserId, reviewStatusId, agencyID, lastReviewedByID, isRotationReviewStatusUpdatingWhileRS, approvedReviewStatusID, isAdminLoggedInAsAgencyUser);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        //public static List<AttestationDocumentContract> GetAttestationDocumentsWithoutSignToExport(Int32 InvitationGroupID, Int32 currentLoggedInUserID)
        //{
        //    try
        //    {
        //        return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetAttestationDocumentsWithoutSignToExport(InvitationGroupID, currentLoggedInUserID);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        #region UAT-1843
        public static List<ClinicalRotationMemberDetail> GetRotationDetailsByOrgUserIds(String orgUserIds, Int32 tenantId, Int32? clinicalRotationID = null)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationDetailsByOrgUserIds(orgUserIds, clinicalRotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateClinicalRotationMenberForNagMail(Int32 RotationMemberId, Int32 CurentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).UpdateClinicalRotationMenberForNagMail(RotationMemberId, CurentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static string GenerateRotationDetailsHTML(ClinicalRotationMemberDetail rotationDetailsContract)
        {
            if (rotationDetailsContract.IsNullOrEmpty())
            {
                return String.Empty;
            }
            StringBuilder _sbRotationDetails = new StringBuilder();
            _sbRotationDetails.Append("<h4><i>Rotation Details:</i></h4>");
            _sbRotationDetails.Append("<div style='line-height:21px'>");
            _sbRotationDetails.Append("<ul style='list-style-type: disc'>");

            if (!rotationDetailsContract.AgencyName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Agency Name: </b>" + rotationDetailsContract.AgencyName + "</li>");
            }
            if (!rotationDetailsContract.ComplioID.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Complio ID: </b>" + rotationDetailsContract.ComplioID + "</li>");
            }
            if (!rotationDetailsContract.RotationName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Rotation Name: </b>" + rotationDetailsContract.RotationName + "</li>");
            }
            if (!rotationDetailsContract.Department.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Department: </b>" + rotationDetailsContract.Department + "</li>");
            }
            if (!rotationDetailsContract.Program.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Program: </b>" + rotationDetailsContract.Program + "</li>");
            }
            if (!rotationDetailsContract.Course.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Course: </b>" + rotationDetailsContract.Course + "</li>");
            }
            if (!rotationDetailsContract.Term.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Term: </b>" + rotationDetailsContract.Term + "</li>");
            }
            if (!rotationDetailsContract.TypeSpecialty.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Type/Specialty: </b>" + rotationDetailsContract.TypeSpecialty + "</li>");
            }
            if (!rotationDetailsContract.UnitFloorLoc.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Unit/Floor or Location: </b>" + rotationDetailsContract.UnitFloorLoc + "</li>");
            }
            if (!rotationDetailsContract.RecommendedHours.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "# of Recommended Hours: </b>" + rotationDetailsContract.RecommendedHours + "</li>");
            }
            if (!rotationDetailsContract.DaysName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Days: </b>" + rotationDetailsContract.DaysName + "</li>");
            }
            if (!rotationDetailsContract.Shift.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Shift: </b>" + rotationDetailsContract.Shift + "</li>");
            }
            if (!rotationDetailsContract.Time.IsNullOrEmpty() && rotationDetailsContract.Time != "-")
            {
                _sbRotationDetails.Append("<li><b>" + "Time: </b>" + rotationDetailsContract.Time + "</li>");
            }
            if (!rotationDetailsContract.StartDate.IsNullOrEmpty() && !rotationDetailsContract.EndDate.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Dates: </b>" + Convert.ToDateTime(rotationDetailsContract.StartDate).ToString("MM/dd/yyyy") + " - " + Convert.ToDateTime(rotationDetailsContract.EndDate).ToString("MM/dd/yyyy") + "</li>");
            }
            _sbRotationDetails.Append("</ul>");
            _sbRotationDetails.Append("</div>");
            return Convert.ToString(_sbRotationDetails);
        }
        #endregion

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        public static List<SharedUserRotationReviewStatusContract> GetReviewStatusList(Int32 tenantID)
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>().Select(col =>
                                         new SharedUserRotationReviewStatusContract
                                         {
                                             RotationReviewStatusID = col.SUIRS_ID,
                                             Name = col.SUIRS_Name,
                                             Description = col.SUIRS_Description,
                                             Code = col.SUIRS_Code
                                         }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion


        #region UAT-1881
        public static List<AgencyDetailContract> GetAllAgencyForOrgUser(Int32 tenantId, Int32 OrgUserId)
        {
            try
            {
                DataTable dt = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetAllAgencyForOrgUser(OrgUserId);
                IEnumerable<DataRow> rows = dt.AsEnumerable();
                return rows.Select(x => new AgencyDetailContract
                {
                    AgencyID = x["AG_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["AG_ID"]),
                    AgencyName = x["AG_Name"].GetType().Name == "DBNull" ? String.Empty : x["AG_Name"].ToString()

                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        public static Boolean SaveUpdateClinicalRotationAssignments(Int32 tenantID, List<Int32> clinicalRotIDs, ClinicalRotationDetailContract clinicalRotationDetailContract,
                                                                    Int32 currentUserId, String rotationAssignType, Int32 packageId, String senderEmailID)
        {
            try
            {
                Boolean isDataSaved = false;
                String syllabusDocumentTypeCode = DocumentType.ROTATION_SYLLABUS.GetStringValue();

                Int32 syllabusDocumentTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(clinicalRotationDetailContract.TenantID)
                                                            .Where(cond => cond.DMT_Code == syllabusDocumentTypeCode).FirstOrDefault().DMT_ID;

                String profileSynchAddToRotationCode = ProfileSynchSourceType.ADD_TO_CLINICAL_ROTATION.GetStringValue();
                Int32 profileSynchSourceTypeID = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpProfileSharingInvitationGroupType>().Where(cond => cond.PSIGT_Code == profileSynchAddToRotationCode).Select(col => col.PSIGT_ID).FirstOrDefault();

                String requirementNotCompliantPackStatusCode = RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue();
                Int32 requirementNotCompliantPackStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageStatu>(clinicalRotationDetailContract.TenantID).Where(x => !x.RPS_IsDeleted && x.RPS_Code == requirementNotCompliantPackStatusCode)
                                                                .FirstOrDefault().RPS_ID;
                String rotationSubscriptionTypeCode = RequirementSubscriptionType.ROTATION_SUBSCRIPTION.GetStringValue();
                Int32 rotationSubscriptionTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementSubscriptionType>(clinicalRotationDetailContract.TenantID).Where(x => !x.RST_IsDeleted && x.RST_Code == rotationSubscriptionTypeCode)
                                                                .FirstOrDefault().RST_ID;
                //UAT-2603
                Int16 dataMovementDueStatusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantID, LkpSubscriptionMobilityStatus.DataMovementDue));

                Tuple<Boolean, List<ClientContactContract>, List<RotationDetailFieldChanges>> resultData = BALUtils.GetClinicalRotationRepoInstance(tenantID).SaveUpdateClinicalRotationAssignments(clinicalRotIDs, clinicalRotationDetailContract, currentUserId
                                                                         , profileSynchSourceTypeID, rotationAssignType, syllabusDocumentTypeID, packageId, rotationSubscriptionTypeID
                                                                         , requirementNotCompliantPackStatusID, dataMovementDueStatusId);
                if (!resultData.IsNullOrEmpty() && resultData.Item1)
                {
                    isDataSaved = resultData.Item1;
                    if (String.Compare(rotationAssignType, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0 && !resultData.Item2.IsNullOrEmpty())
                    {
                        clinicalRotationDetailContract.ContactsToSendEmail = resultData.Item2;
                        SaveCommunicationDataToSendMail(clinicalRotationDetailContract, senderEmailID);
                    }
                    //UAT-3108
                    if (!resultData.Item3.IsNullOrEmpty())
                    {
                        List<RotationDetailFieldChanges> lstRotationDetailFieldChanges = resultData.Item3;
                        if (!lstRotationDetailFieldChanges.IsNullOrEmpty())
                        {
                            foreach (RotationDetailFieldChanges item in lstRotationDetailFieldChanges)
                            {
                                CommunicationManager.SendMailOnUpdationInRotationdetails(item);
                            }
                        }
                        #region UAT-4561://check here if Rotation is approved/ also when only end date changes are there! then only send the below mentioned notification:
                        // check if mail is triggered from here?
                        //  if (!lstRotationDetailFieldChanges.IsNullOrEmpty())
                        // {
                        //   foreach (RotationDetailFieldChanges item in lstRotationDetailFieldChanges)
                        //  {
                        // CommunicationManager.SendMailOnUpdationInRotationEndDate(Convert.ToDateTime(clinicalRotationDetailContract.EndDate), item);
                        // }
                        //}
                        #endregion
                    }
                }
                return isDataSaved;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsDataEnteredForAnyRotation(Int32 tenantID, String clinicalRotIDs, String packageType)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).IsDataEnteredForAnyRotation(tenantID, clinicalRotIDs, packageType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsPreceptorAssignedForAllRotations(Int32 tenantID, String clinicalRotIDs)
        {
            try
            {
                List<Int32> lstRotationIDs = new List<Int32>();
                if (!clinicalRotIDs.IsNullOrEmpty())
                {
                    lstRotationIDs = clinicalRotIDs.Split(',').Select(int.Parse).ToList();
                }

                return BALUtils.GetClinicalRotationRepoInstance(tenantID).IsPreceptorAssignedForAllRotations(lstRotationIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<InstructorAvailabilityContract> CheckInstAvailabilityByRotationIds(Int32 tenantID, String clinicalRotIDs)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).CheckInstAvailabilityByRotationIds(clinicalRotIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region  UAT-2042, Phase 4 (24): Preceptor Clinical Rotations
        public static ClientContactSyllabusDocumentContract GetClientContactRotationDocumentsByID(Int32 tenantID, Int32 clientContactID, Int32 clinicalRotationID)
        {
            try
            {
                return GetClientContactRotationDocuments(clientContactID, tenantID).Where(x => x.ClinicalRotationID == clinicalRotationID).FirstOrDefault();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        public static Dictionary<Int32, Boolean> GetComplianceRequiredRotCatForPackage(Int32 tenantID, Int32 reqPackageId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetComplianceRequiredRotCatForPackage(reqPackageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT 2477
        public static List<ClinicalRotationDetailContract> GetRotationPackageAndAgencyData(int rotationID, int tenantID)
        {
            try
            {

                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRotationPackageAndAgencyData(rotationID, tenantID);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2514
        public static Dictionary<Boolean, DateTime> IsRotationEndDateRangeNeedToManage(Int32 clinicalRotationID, Int32 tenantID)
        {
            return BALUtils.GetClinicalRotationRepoInstance(tenantID).IsRotationEndDateRangeNeedToManage(clinicalRotationID);
        }

        public static List<ClinicalRotationMember> GetDroppedRotationMembersByRotationID(Int32 tenantID, Int32 clinicalRotationID)
        {
            return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetDroppedRotationMembersByRotationID(clinicalRotationID);
        }
        #endregion


        #region UAT-2424
        /// <summary>
        /// Get All Clinical Rotations of a Tenant
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetAllClinicalRotations(Int32 tenantID)
        {
            List<ClinicalRotation> lstClinicalRotations = BALUtils.GetClinicalRotationRepoInstance(tenantID).GetAllClinicalRotations();
            List<ClinicalRotationDetailContract> lstClinicalRotationsDetailContract = new List<ClinicalRotationDetailContract>();

            foreach (ClinicalRotation clinicalRotation in lstClinicalRotations)
            {
                ClinicalRotationDetailContract clinicalRotationDetailContract = new ClinicalRotationDetailContract();
                clinicalRotationDetailContract.RotationID = clinicalRotation.CR_ID;
                clinicalRotationDetailContract.RotationName = clinicalRotation.CR_RotationName;
                clinicalRotationDetailContract.ComplioID = clinicalRotation.CR_ComplioID;
                lstClinicalRotationsDetailContract.Add(clinicalRotationDetailContract);
            }
            return lstClinicalRotationsDetailContract;
        }

        public static Int32 GetRotationPackageIDByRotationId(Int32 tenantID, Int32 clinicalRotationID, String reqPkgTypeCode, ClinicalRotationDetailContract newClinicalRotationDetails)
        {
            ClinicalRotationRequirementPackage tempRotationPackage = new ClinicalRotationRequirementPackage();
            tempRotationPackage = BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRotationPackageByRotationId(clinicalRotationID, reqPkgTypeCode);

            if (!tempRotationPackage.IsNullOrEmpty())
            {
                //UAT-4349                
                List<RequirementPackageContract> lstSharedPackages = new List<RequirementPackageContract>();
                List<RequirementPackageContract> lstTenantPackages = new List<RequirementPackageContract>();
                List<RequirementPackageContract> lstFinalRequirementPackages = new List<RequirementPackageContract>();
                //get shared packages
                lstSharedPackages = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackagesFromAgencyIds(newClinicalRotationDetails.TenantID, newClinicalRotationDetails.AgencyIdList, reqPkgTypeCode);
                lstTenantPackages = BALUtils.GetRequirementPackageRepoInstance(newClinicalRotationDetails.TenantID).GetRequirementPackagesFromAgencyIds(newClinicalRotationDetails.AgencyIdList, reqPkgTypeCode).ToList();
                if (!lstTenantPackages.IsNullOrEmpty())
                {
                    lstTenantPackages = lstTenantPackages.Where(con => !con.IsCopied).ToList();
                }

                lstFinalRequirementPackages = lstSharedPackages.Concat(lstTenantPackages).ToList();
                Entity.ClientEntity.RequirementPackage requirementpackage = tempRotationPackage.RequirementPackage;
                if (!requirementpackage.IsNullOrEmpty())
                {
                    if (!lstFinalRequirementPackages.IsNullOrEmpty() && lstFinalRequirementPackages.Count > 0)
                    {
                        lstFinalRequirementPackages = lstFinalRequirementPackages.Where(con => con.RequirementPackageCode == requirementpackage.RP_Code).ToList();
                    }
                    if (!lstFinalRequirementPackages.IsNullOrEmpty() && lstFinalRequirementPackages.Count > 0)
                    {
                        if (requirementpackage.RP_IsNewPackage)
                        {
                            if ((requirementpackage.RP_EffectiveEndDate.IsNull() || requirementpackage.RP_EffectiveEndDate > newClinicalRotationDetails.StartDate)
                                && (requirementpackage.RP_EffectiveStartDate.IsNull() || requirementpackage.RP_EffectiveStartDate < newClinicalRotationDetails.EndDate)
                                && requirementpackage.RP_IsActive && !requirementpackage.RP_IsArchived && !requirementpackage.RP_IsDeleted)
                            {
                                return requirementpackage.RP_ID;
                            }
                        }
                    }

                    else
                    {
                        RequirementPackageContract requirementPackageContract = GetRotationPackageByRotationId(tenantID, clinicalRotationID, reqPkgTypeCode);
                        if (!requirementPackageContract.IsNullOrEmpty())
                        {
                            if (!lstFinalRequirementPackages.IsNullOrEmpty() && lstFinalRequirementPackages.Count > 0)
                            {
                                lstFinalRequirementPackages = lstFinalRequirementPackages.Where(con => con.RequirementPackageCode == requirementPackageContract.RequirementPackageCode).ToList();
                            }
                            if (!lstFinalRequirementPackages.IsNullOrEmpty() && lstFinalRequirementPackages.Count > 0)
                            {
                                if ((requirementPackageContract.EffectiveEndDate.IsNull() || requirementPackageContract.EffectiveEndDate > newClinicalRotationDetails.StartDate)
                                                             && (requirementPackageContract.EffectiveStartDate.IsNull() || requirementPackageContract.EffectiveStartDate < newClinicalRotationDetails.EndDate)
                                                             && requirementPackageContract.IsActive && !requirementPackageContract.IsArchivedPackage)
                                {
                                    return requirementPackageContract.RequirementPackageID;
                                }
                            }

                        }
                    }
                }

            }
            return AppConsts.NONE;
        }
        //UAT-3121
        public static RequirementPackageContract GetRotationPackageByRotationId(Int32 tenantID, Int32 clinicalRotationID, String reqPkgTypeCode)
        {
            //ClinicalRotationRequirementPackage tempRotationPackage = new ClinicalRotationRequirementPackage();
            //tempRotationPackage = BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRotationPackageByRotationId(clinicalRotationID, reqPkgTypeCode);
            //if (tempRotationPackage.IsNullOrEmpty())
            //{
            //    return AppConsts.NONE;
            //}
            //else
            //{
            //    return tempRotationPackage.CRRP_RequirementPackageID;
            //}
            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();
            ClinicalRotationRequirementPackage tempRotationPackage = new ClinicalRotationRequirementPackage();
            tempRotationPackage = BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRotationPackageByRotationId(clinicalRotationID, reqPkgTypeCode);

            if (!tempRotationPackage.IsNullOrEmpty() && !tempRotationPackage.RequirementPackage.IsNullOrEmpty())
            {
                requirementPackageContract.RequirementPackageID = tempRotationPackage.RequirementPackage.RP_ID;
                requirementPackageContract.RequirementPackageName = tempRotationPackage.RequirementPackage.RP_PackageName;
                requirementPackageContract.RequirementPackageLabel = tempRotationPackage.RequirementPackage.RP_PackageLabel;
                //requirementPackageContract.RequirementPackageType =tempRotationPackage.RequirementPackage.RP_RequirementPackageTypeID;
                requirementPackageContract.RequirementPackageDescription = tempRotationPackage.RequirementPackage.RP_Description;
                requirementPackageContract.IsDeleted = tempRotationPackage.RequirementPackage.RP_IsDeleted;
                requirementPackageContract.RequirementPackageCode = tempRotationPackage.RequirementPackage.RP_Code.IsNotNull() ? tempRotationPackage.RequirementPackage.RP_Code.Value : Guid.Empty;
                requirementPackageContract.DefinedRequirementID = tempRotationPackage.RequirementPackage.RP_DefinedRequirementID;
                requirementPackageContract.ReqReviewByID = tempRotationPackage.RequirementPackage.RP_ReqReviewByID;

                Entity.SharedDataEntity.RequirementPackage pkgDetailFromSharedDb = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackageByCode(requirementPackageContract.RequirementPackageCode);

                if (pkgDetailFromSharedDb != null)
                {
                    requirementPackageContract.IsActive = pkgDetailFromSharedDb.RP_IsActive;
                    requirementPackageContract.FirstVersionID = pkgDetailFromSharedDb.RP_FirstVersionID.IsNotNull() ? pkgDetailFromSharedDb.RP_FirstVersionID.Value : AppConsts.NONE;
                    requirementPackageContract.IsUsed = pkgDetailFromSharedDb.RP_IsUsed.IsNotNull() ? pkgDetailFromSharedDb.RP_IsUsed.Value : false;
                    requirementPackageContract.IsCopied = pkgDetailFromSharedDb.RP_IsCopied.IsNotNull() ? pkgDetailFromSharedDb.RP_IsCopied.Value : false;
                    requirementPackageContract.IsNewPackage = pkgDetailFromSharedDb.RP_IsNewPackage;
                    requirementPackageContract.EffectiveStartDate = pkgDetailFromSharedDb.RP_EffectiveStartDate;
                    requirementPackageContract.EffectiveEndDate = pkgDetailFromSharedDb.RP_EffectiveEndDate;
                    requirementPackageContract.IsArchivedPackage = pkgDetailFromSharedDb.RP_IsArchived;
                }
            }
            return requirementPackageContract;
        }
        /// <summary>
        /// Get Details of a Rotation By Rotation ID
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clinicalRotationId"></param>
        /// <returns></returns>
        public static ClinicalRotationDetailContract GetClinicalRotationDetailsById(Int32 tenantId, Int32 clinicalRotationId)
        {
            try
            {
                string RotationAdditionalDocument = DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue();

                Int32 syllabusDocumentTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantId)
                                                            .Where(cond => cond.DMT_Code == RotationAdditionalDocument).FirstOrDefault().DMT_ID;

                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetClinicalRotationDetailsById(clinicalRotationId, syllabusDocumentTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static List<MultipleAdditionalDocumentsContract> GetAdditionalDocumnetDetails(Int32 tenantId, Int32 clinicalRotationId)
        {
            try
            {
                string RotationAdditionalDocument = DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue();

                Int32 syllabusDocumentTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantId)
                                                            .Where(cond => cond.DMT_Code == RotationAdditionalDocument).FirstOrDefault().DMT_ID;

                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetAdditionalDocumnetDetails(clinicalRotationId, syllabusDocumentTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-4334
        public static List<ClinicalRotationDetailContract> GetAllClinicalRotationsForLoggedInUser(Int32 tenantID, Int32 currentUserId, bool isAdminLoggedIn)
        {
            List<ClinicalRotationDetailContract> lstClinicalRotations = BALUtils.GetClinicalRotationRepoInstance(tenantID)
                .GetAllClinicalRotationsForLoggedInUser(currentUserId, isAdminLoggedIn);

            return lstClinicalRotations;
        }
        #endregion


        #region UAT-2544:

        public static Boolean IsApplicantDroppedFromRotation(Int32 tenantId, Int32 clinicalRotationId, Int32 RPS_ID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).IsApplicantDroppedFromRotation(clinicalRotationId, RPS_ID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean NeedToChangeInvitationStatusAsPending(Int32 tenantId, Int32 clinicalRotationId, List<Int32> invitationIDs, Int32 studentid, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).NeedToChangeInvitationStatusAsPending(clinicalRotationId, invitationIDs, studentid, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2554

        public static Boolean IsPreceptorRequiredForAgency(Int32 tenantId, Int32 agencyId)
        {
            try
            {
                String code = AgencyPermissionType.PRECEPTOR_REQD_ROTATION_PRMSN.GetStringValue();
                Int32 agencyPrmsnTypeID = LookupManager.GetSharedDBLookUpData<lkpAgencyPermissionType>().Where(x => x.APT_Code == code && !x.APT_IsDeleted).FirstOrDefault().APT_ID;
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).IsPreceptorRequiredForAgency(agencyId, agencyPrmsnTypeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-2666
        public static Boolean UpdateClinicalRotationByAgency(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 tenantId, Int32 currentLoggedInUserId, Boolean IsSharedUser)
        {
            try
            {
                RotationDetailFieldChanges rotationDetailFieldChanges = BALUtils.GetClinicalRotationRepoInstance(tenantId).UpdateClinicalRotationByAgency(clinicalRotationDetailContract, currentLoggedInUserId, IsSharedUser, tenantId);
                //UAT-3108
                if (!rotationDetailFieldChanges.IsNullOrEmpty() && rotationDetailFieldChanges.IsClinicalRotationUpdatedSuccessfully)
                {
                    //UAT-4428
                    if (rotationDetailFieldChanges.IsStartDateUpdated)
                        ExecuteCategoryRuleOnRotationDateChange(tenantId, clinicalRotationDetailContract.RotationID, currentLoggedInUserId);
                    //END UAT-4428

                    //Send mail for rotation update.
                    CommunicationManager.SendMailOnUpdationInRotationdetails(rotationDetailFieldChanges);

                    #region UAT-4561://check here if Rotation is approved/ also when only end date changes are there! then only send the below mentioned notification:
                    //CommunicationManager.SendMailOnUpdationInRotationEndDate(Convert.ToDateTime(clinicalRotationDetailContract.EndDate), rotationDetailFieldChanges);
                    #endregion

                    return true;
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<RotationFieldUpdatedByAgencyContract> GetRotationFieldUpdateByAgencyDetails(List<Int32> lstClinicalRotationIds, Int32 tenantId)
        {
            try
            {
                List<RotationFieldUpdatedByAgency> lstRotationFieldUpdateByAgency = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationFieldUpdateByAgencyDetails(lstClinicalRotationIds);
                return lstRotationFieldUpdateByAgency.Select(x => new RotationFieldUpdatedByAgencyContract()
                {
                    ClinicalRotationID = x.RFUA_ClinicalRotationID,
                    IsCourseUpdated = x.RFUA_IsCourseUpdated,
                    IsDepartmentUpdated = x.RFUA_IsDepartmentUpdated,
                    IsNoOfHoursUpdated = x.RFUA_IsNoOfHoursUpdated,
                    IsNoOfStudentsUpdated = x.RFUA_IsNoOfStudentsUpdated,
                    IsProgramUpdated = x.RFUA_IsProgramUpdated,
                    IsRotationNameUpadted = x.RFUA_IsRotationNameUpadted,
                    IsRotationShiftUpdated = x.RFUA_IsRotationShiftUpdated,
                    IsTermUpdated = x.RFUA_IsTermUpdated,
                    IsTypeSpecialtyUpdated = x.RFUA_IsTypeSpecialtyUpdated,
                    IsUnitFloorLocUpdated = x.RFUA_IsUnitFloorLocUpdated,
                    IsEndDateUpdated = x.RFUA_IsEndDateUpdated,
                    IsStartDateUpdated = x.RFUA_IsStartDateUpdated
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2603

        public static String GetRPSIdsWithDataMovementDueStatus(Int32 tenantId, Int32 chunkSize)
        {
            try
            {
                Int16 statusId = Convert.ToInt16(MobilityManager.GetMobilityStatusIDByCode(tenantId, LkpSubscriptionMobilityStatus.DataMovementDue));
                List<Int32> lstDataMovementDueStatusRPSIds = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRPSIdsWithDataMovementDueStatus(chunkSize, statusId);
                return String.Join(",", lstDataMovementDueStatusRPSIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementRuleData> PerformRotationDataMovement(Int32 tenantId, String lstRPSIdsWithDueStatus, Int32 currentUserId)
        {
            try
            {
                DataTable table = BALUtils.GetClinicalRotationRepoInstance(tenantId).PerformRotationDataMovement(lstRPSIdsWithDueStatus, currentUserId);

                IEnumerable<DataRow> rows = table.AsEnumerable();
                List<RequirementRuleData> lstRequirementRuleData = new List<RequirementRuleData>();
                lstRequirementRuleData.AddRange(rows.Select(col => new RequirementRuleData
                {
                    Rps_Id = col["RPSID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPSID"]),
                    PackageId = col["ReqPkgID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqPkgID"]),
                    CategoryId = col["ReqCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqCategoryID"]),
                    ItemId = col["ReqItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqItemID"]),
                    ApplicantUserID = col["OrgUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["OrgUserID"]),
                    FieldId = col["ReqFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqFieldID"]),
                    ApplicantRequirementItemDataID = col["ApplicantRequirementItemDataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ApplicantRequirementItemDataID"]),
                }).ToList());

                return lstRequirementRuleData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-2712
        public static AgencyHierarchyRotationFieldOptionContract GetAgencyHierarchyRotationFieldOptionSetting(Int32 tenantId, String hierarchyID)
        {
            try
            {
                var HierarchyRotationFieldOptionListData = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetAgencyHierarchyRotationFieldOptionSetting(hierarchyID);

                AgencyHierarchyRotationFieldOptionContract result = new AgencyHierarchyRotationFieldOptionContract();
                //result.AgencyHierarchyID = HierarchyRotationFieldOptionListData.Where(x => x.)
                if (HierarchyRotationFieldOptionListData.Any())
                {
                    result.AHRFO_IsCourse_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsCourse_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsDaysBefore_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsDaysBefore_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsDeadlineDate_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsDeadlineDate_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsDepartment_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsDepartment_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsEndTime_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsEndTime_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsFrequency_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsFrequency_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsIP_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsIP_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsNoOfHours_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsNoOfHours_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsNoOfStudents_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsNoOfStudents_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsProgram_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsProgram_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsRotationName_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsRotationName_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsRotationShift_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsRotationShift_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsRotDays_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsRotDays_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsStartTime_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsStartTime_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsSyllabusDocument_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsSyllabusDocument_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsAdditionalDocuments_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsAdditionalDocuments_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsTerm_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsTerm_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsTypeSpecialty_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsTypeSpecialty_Required ?? false)).Any() ? true : false;
                    result.AHRFO_IsUnitFloorLoc_Required = HierarchyRotationFieldOptionListData.Where(x => (x.AHRFO_IsUnitFloorLoc_Required ?? false)).Any() ? true : false;

                }
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        #region UAT-2668
        public static List<ClinicalRotationAgencyContract> GetAgenciesMappedWithRotation(Int32 tenantId, Int32 clinicalRotationID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetAgenciesMappedWithRotation(clinicalRotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2513
        public static Boolean SaveBatchRotationUploadDetails(List<BatchRotationUploadContract> clinicalRotationDetailContractList, String fileName, Int32 tenantID, Int32 currentLoggedInID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).SaveBatchRotationUploadDetails(clinicalRotationDetailContractList, fileName, currentLoggedInID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BatchRotationUploadContract> GetBatchRotationList(Int32 tenantID, BatchRotationUploadContract searchContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetBatchRotationList(searchContract, gridCustomPaging);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static List<Int32> GetBatchRotationListForTimer(Int32 tenantID, Int32 chunksize)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetBatchRotationListForTimer(chunksize);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Boolean CreateClinicalRotationFromBatchRotationUploadDetails(List<Int32> batchRotationUploadDetailIds, Int32 tenantID, Int32 currentLoggedInID)
        {
            try
            {
                List<ClinicalRotationDetailContract> lstClinicalRotationDetailContract = BALUtils.GetClinicalRotationRepoInstance(tenantID).CreateClinicalRotationFromBatchRotationUploadDetails(batchRotationUploadDetailIds, currentLoggedInID);
                //UAT-2973
                if (!lstClinicalRotationDetailContract.IsNullOrEmpty())
                {
                    lstClinicalRotationDetailContract.ForEach(clinicalRotation =>
                    {
                        if (!clinicalRotation.AgencyIdList.IsNullOrEmpty() && !clinicalRotation.RotationID.IsNullOrEmpty())
                        {
                            clinicalRotation.TenantID = tenantID;
                            CheckAgencyPackagesAndAssignToRotation(currentLoggedInID, clinicalRotation);
                        }
                    });
                }
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region [UAT-2679]

        public static List<RequirementPackageContract> GetRequirementPackage(Int32 tenantID, Int32 packageTypeId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRequirementPackage(packageTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementCategoryContract> GetRequirementCategory(Int32 tenantID, List<Int32> lstRequirementpackage)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRequirementCategory(lstRequirementpackage);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementItemContract> GetRequirementItem(Int32 tenantID, List<Int32> lstRequirementcategory)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRequirementItem(lstRequirementcategory);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ApplicantRequirementDataAuditContract> GetApplicantRequirementDataAudit(Int32 tenantID, ApplicantRequirementDataAuditSearchContract searchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetApplicantRequirementDataAudit(searchContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2926
        public static List<String> GetAgencyHierarchyAgencyList(List<Tuple<Int32, Int32>> AgencyHierachyAgencyIds)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyHierachyAgencyIds(AgencyHierachyAgencyIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2973

        private static Boolean CheckAgencyPackagesAndAssignToRotation(Int32 currentUserID, ClinicalRotationDetailContract clinicalRotationDetailContract)
        {
            if (!clinicalRotationDetailContract.IsNullOrEmpty())
            {
                List<RequirementPackageContract> lstFinalRequirementPackages = new List<RequirementPackageContract>();
                List<RequirementPackageContract> lstSharedPackages = new List<RequirementPackageContract>();
                List<RequirementPackageContract> lstTenantPackages = new List<RequirementPackageContract>();
                //get shared packages
                lstSharedPackages = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackagesFromAgencyIds(clinicalRotationDetailContract.TenantID, clinicalRotationDetailContract.AgencyIdList, null);
                //Get Tenant packages.
                lstTenantPackages = BALUtils.GetRequirementPackageRepoInstance(clinicalRotationDetailContract.TenantID).GetRequirementPackagesFromAgencyIds(clinicalRotationDetailContract.AgencyIdList, null).ToList();
                if (!lstTenantPackages.IsNullOrEmpty())
                {
                    lstTenantPackages = lstTenantPackages.Where(con => !con.IsCopied).ToList();
                }

                lstFinalRequirementPackages = lstSharedPackages.Concat(lstTenantPackages).ToList();

                DateTime? RotationStartDate = clinicalRotationDetailContract.StartDate;
                DateTime? RotationEndDate = clinicalRotationDetailContract.EndDate;

                lstFinalRequirementPackages = lstFinalRequirementPackages.Where(cond => (cond.EffectiveEndDate > RotationStartDate || cond.EffectiveEndDate.IsNull())
                                                                && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < RotationEndDate)).ToList();

                #region UAT-4657

                var CombinedPackageList = lstFinalRequirementPackages;

                CombinedPackageList = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > RotationStartDate || cond.EffectiveEndDate.IsNull())
                                                               && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < RotationEndDate)).ToList();
                List<RequirementPackageContract> finalPkgList = new List<RequirementPackageContract>();

                CombinedPackageList.DistinctBy(cond => cond.RootParentCode).Select(col => col.RootParentCode).ForEach(rootParentCode =>
                {
                    List<RequirementPackageContract> lstPkgListForGroup = CombinedPackageList.Where(cond => cond.RootParentCode == rootParentCode).ToList();
                    if (!lstPkgListForGroup.IsNullOrEmpty() && lstPkgListForGroup.Any())
                    {
                        if (lstPkgListForGroup.Count == AppConsts.ONE)
                        {
                            finalPkgList.AddRange(lstPkgListForGroup);
                        }
                        else
                        {
                            //DateTime pkgHighestEffDate = lstPkgListForGroup.OrderByDescending(ord => ord.EffectiveStartDate).FirstOrDefault().EffectiveStartDate.Value;
                            DateTime? pkgHighestEffDate = lstPkgListForGroup.Where(con => con.EffectiveStartDate < RotationStartDate).Any() ?
                                                        lstPkgListForGroup.Where(con => con.EffectiveStartDate < RotationStartDate).Max(x => x.EffectiveStartDate).Value : (DateTime?)null;
                            finalPkgList.AddRange(lstPkgListForGroup.Where(con => con.EffectiveStartDate.Value == pkgHighestEffDate).ToList());
                        }
                    }
                });

                lstFinalRequirementPackages = finalPkgList;

                #endregion     

                if (lstFinalRequirementPackages.Count == AppConsts.ONE)
                {
                    //method to assign package to rotation ,If only one package exists for an agency.
                    AssignSinglePackageToRotation(lstFinalRequirementPackages[0], clinicalRotationDetailContract.RotationID, clinicalRotationDetailContract.TenantID, currentUserID);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Assign packages in a rotation if the agency has only one package.
        /// </summary>
        /// <param name="requirementPackageContract"></param>
        /// <param name="clinicalRotationId"></param>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        private static Int32 AssignSinglePackageToRotation(RequirementPackageContract requirementPackageContract, Int32 clinicalRotationId, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            Int32 packageIdToBeAssigned = AppConsts.NONE;

            if (requirementPackageContract.IsSharedUserPackage)
            {
                if (requirementPackageContract.IsNewPackage)
                {
                    RequirementPackageContract sharedPackageContract = SharedRequirementPackageManager.GetRequirementPackageHierarchalDetailsByPackageIDNew(requirementPackageContract.RequirementPackageID, Guid.Empty, true);
                    if (!sharedPackageContract.IsUsed)
                    {
                        SharedRequirementPackageManager.SetExistingPackageIsUsedToTrue(currentLoggedInUserId, requirementPackageContract.RequirementPackageID);
                    }
                    packageIdToBeAssigned = RequirementPackageManager.CopySharedPackageToClientNew(sharedPackageContract, tenantId, currentLoggedInUserId);
                }
                else
                {
                    RequirementPackageContract sharedPackageContract = SharedRequirementPackageManager.GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageContract.RequirementPackageID
                                                                                                                                                                                    , Guid.Empty, true);
                    if (!sharedPackageContract.IsUsed)
                    {
                        SharedRequirementPackageManager.SetExistingPackageIsUsedToTrue(currentLoggedInUserId, requirementPackageContract.RequirementPackageID);
                    }
                    packageIdToBeAssigned = RequirementPackageManager.CopyPackageToClient(sharedPackageContract, tenantId, currentLoggedInUserId);
                }
                if (packageIdToBeAssigned > AppConsts.NONE)
                {
                    UniversalMappingDataManager.CopySharedToTenantRequirementUniversalMapping(tenantId, requirementPackageContract.RequirementPackageID
                                                                                            , packageIdToBeAssigned, currentLoggedInUserId);
                }
            }
            String reqPkgTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
            ClinicalRotationManager.AddPackageToRotation(tenantId, clinicalRotationId, packageIdToBeAssigned, reqPkgTypeCode, currentLoggedInUserId);
            return packageIdToBeAssigned;
        }
        #endregion

        #region Requirement Verification Assignment Queue AND User Work Queue
        public static List<RequirementVerificationQueueContract> GetAssignmentRotationVerificationQueueData(RequirementVerificationQueueContract requirementVerificationQueueContract, CustomPagingArgsContract customPagingArgsContract)
        {
            return BALUtils.GetSharedRequirementPackageRepoInstance().GetAssignmentRotationVerificationQueueData(requirementVerificationQueueContract, customPagingArgsContract);
        }

        public static List<ReqPkgSubscriptionIDList> GetReqPkgSubscriptionIdList(RequirementVerificationQueueContract requirementVerificationQueueContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID)
        {
            return BALUtils.GetSharedRequirementPackageRepoInstance().GetReqPkgSubscriptionIdList(requirementVerificationQueueContract, CurrentReqPkgSubscriptionID, ApplicantRequirementItemID);
        }

        public static Boolean AssignItemsToUser(List<Int32> lstSelectedVerificationItems, Int32 VerSelectedUserId, String verSelectedUserName)
        {
            return BALUtils.GetSharedRequirementPackageRepoInstance().AssignItemsToUser(lstSelectedVerificationItems, VerSelectedUserId, verSelectedUserName);
        }

        public static List<RequirementPackageTypeContract> GetSharedRequirementPackageTypes()
        {
            try
            {
                List<Entity.SharedDataEntity.lkpRequirementPackageType> dataTypeList = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpRequirementPackageType>().Where(x => !x.RPT_IsDeleted).ToList();
                return dataTypeList.Select(con => new RequirementPackageTypeContract
                {
                    ID = con.RPT_ID,
                    Code = con.RPT_Code,
                    Name = con.RPT_Name
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-3045
        public static ProfileSharingExpiryContract GetNonComplianceCategoryList(Int32 tenantId, Int32 clinicalRotationID, String delimittedOrgUserIDs, String selectedCategoriesXml)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetNonComplianceCategoryList(clinicalRotationID, delimittedOrgUserIDs, selectedCategoriesXml);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static DataTable PerformRotationLiveDataMovement(Int32 tenantId, Int32 requirementSubscriptionID, Int32 requirementCategoryID, Int32 currentUserID)
        {
            DataTable table = BALUtils.GetClinicalRotationRepoInstance(tenantId).PerformRotationLiveDataMovement(requirementSubscriptionID, requirementCategoryID, currentUserID);
            IEnumerable<DataRow> rows = table.AsEnumerable();
            List<RequirementRuleData> lstRequirementRuleData = new List<RequirementRuleData>();
            lstRequirementRuleData.AddRange(rows.Select(col => new RequirementRuleData
            {
                Rps_Id = col["RPSID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPSID"]),
                PackageId = col["ReqPkgID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqPkgID"]),
                CategoryId = col["ReqCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqCategoryID"]),
                ItemId = col["ReqItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqItemID"]),
                ApplicantUserID = col["OrgUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["OrgUserID"]),
                FieldId = col["ReqFieldID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqFieldID"]),
                ApplicantRequirementItemDataID = col["ApplicantRequirementItemDataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ApplicantRequirementItemDataID"]),
            }).ToList());

            List<RequirementRuleData> lstRuleData = lstRequirementRuleData
                       .DistinctBy(cond => new { cond.Rps_Id, cond.PackageId, cond.ApplicantUserID, cond.CategoryId, cond.ItemId })
                       .ToList();


            foreach (RequirementRuleData item in lstRuleData)
            {
                List<Int32> lstReqFields = lstRequirementRuleData.Where(cond => cond.Rps_Id == item.Rps_Id
                                                               && cond.PackageId == item.PackageId
                                                               && cond.ApplicantUserID == item.ApplicantUserID
                                                               && cond.CategoryId == item.CategoryId
                                                               && cond.ItemId == item.ItemId
                                                               && cond.FieldId > 0)
                                                               .Select(cond => cond.FieldId).ToList();

                EvaluateRequirementDynamicBuisnessRules(item.Rps_Id, item.CategoryId, item.ItemId, currentUserID, tenantId, lstReqFields);
            }

            String packageSubscriptionIDs = String.Join(",", lstRequirementRuleData.Select(sel => sel.Rps_Id).ToList());
            if (!packageSubscriptionIDs.IsNullOrEmpty())
            {
                RequirementVerificationManager.SyncRequirementVerificationToFlatData(packageSubscriptionIDs, tenantId, currentUserID);
            }

            return table;
        }

        public static DataTable GetTargetReqPackageSubscriptionIDsForSync(Int32 tenantId, Int32 sourceReqSubscriptionID, Int32 requirementCategoryID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetTargetReqPackageSubscriptionIDsForSync(sourceReqSubscriptionID, requirementCategoryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void EvaluateRequirementDynamicBuisnessRules(Int32 reqSubsID, Int32 reqCategoryId, Int32 reqItemId, Int32 currentLoggedInUserID, Int32 tenantID, List<Int32> lstReqFields)
        {
            List<RequirementRuleObjectTree> ruleObjectMappingList = new List<RequirementRuleObjectTree>();
            string ruleObjectXml = string.Empty;

            RequirementRuleObjectTree ruleObjectMappingForCategory = new RequirementRuleObjectTree
            {
                RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                RuleObjectId = Convert.ToString(reqCategoryId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RequirementRuleObjectTree ruleObjectMappingForItem = new RequirementRuleObjectTree
            {
                RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                RuleObjectId = Convert.ToString(reqItemId),
                RuleObjectParentId = Convert.ToString(reqCategoryId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);

            foreach (var item in lstReqFields)
            {
                RequirementRuleObjectTree ruleObjectMappingForAttr = new RequirementRuleObjectTree
                {
                    RuleObjectTypeCode = ObjectType.Compliance_ATR.GetStringValue(),
                    RuleObjectId = Convert.ToString(item),
                    RuleObjectParentId = Convert.ToString(reqItemId)
                };
                ruleObjectMappingList.Add(ruleObjectMappingForAttr);
            }

            List<Entity.ClientEntity.lkpObjectType> lstlkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantID).Where(cond => !cond.OT_IsDeleted).ToList();

            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("RuleObjects"));
            foreach (RequirementRuleObjectTree ruleObjectMapping in ruleObjectMappingList)
            {
                var lkpObjectType = lstlkpObjectType.Where(sel => sel.OT_Code == ruleObjectMapping.RuleObjectTypeCode).FirstOrDefault();
                XmlNode exp = el.AppendChild(doc.CreateElement("RuleObject"));
                exp.AppendChild(doc.CreateElement("TypeId")).InnerText = lkpObjectType.IsNotNull() ? lkpObjectType.OT_ID.ToString() : String.Empty;
                exp.AppendChild(doc.CreateElement("Id")).InnerText = ruleObjectMapping.RuleObjectId;
                exp.AppendChild(doc.CreateElement("ParentId")).InnerText = ruleObjectMapping.RuleObjectParentId;
            }

            ruleObjectXml = doc.OuterXml.ToString();

            RuleManager.EvaluateRequirementPostSubmitRules(ruleObjectXml, reqSubsID, currentLoggedInUserID, tenantID);
        }

        #region UAT-3197, As an Agency User, I should be able to retrieve the syllabus
        public static List<ClientContactSyllabusDocumentContract> GetClinicalRotationSyllabusDocumentsByID(Int32 tenantID, Int32 clinicalRotationID)
        {
            try
            {
                string RotationAdditionalDocument = DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue();

                Int32 syllabusDocumentTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantID)
                                                            .Where(cond => cond.DMT_Code == RotationAdditionalDocument).FirstOrDefault().DMT_ID;


                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetClinicalRotationSyllabusDocumentsByID(clinicalRotationID, syllabusDocumentTypeID);//.Where(x => x.ClinicalRotationID == clinicalRotationID).FirstOrDefault();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        #region UAT-3220
        public static Boolean HideRequirementSharesDetailLink(Guid userID)
        {
            try
            {
                //String agencyPortalHideDetailLinkPermissionTypeCode = AgencyUserPermissionType.AGENCY_PORTAL_DETAIL_LINK_PERMISSION.GetStringValue();
                //Int32 agencyPortalHideDetailLinkPermissionTypeId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAgencyUserPermissionType>().FirstOrDefault(condition => condition.AUPT_Code == agencyPortalHideDetailLinkPermissionTypeCode).AUPT_ID;
                return BALUtils.GetSharedRequirementPackageRepoInstance().HideRequirementSharesDetailLink(userID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        #region  UAT-3176
        public static List<RequirementAttributeGroupContract> GetAllRotationAttributeGroup(Int32 tenantID, String attributeName, String attributeLabel)
        {
            try
            {
                return SharedRequirementPackageManager.GetAllRotationAttributeGroup(attributeName, attributeLabel);
                // BALUtils.GetClinicalRotationRepoInstance(tenantID).GetAllRotationAttributeGroup(attributeName,attributeLabel);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateRotationAttributeGroup(RequirementAttributeGroupContract rotationAttributeGroupContract, Int32 currentLoggedInUserID, Boolean IsAttributeGroupExists)
        {
            try
            {
                return SharedRequirementPackageManager.SaveUpdateRotationAttributeGroup(rotationAttributeGroupContract, IsAttributeGroupExists, currentLoggedInUserID);
                // return BALUtils.GetClinicalRotationRepoInstance(tenantID).SaveUpdateRotationAttributeGroup(rotationAttributeGroupContract, IsAttributeGroupExists);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static RequirementAttributeGroupContract GetAttributeGroupById(Int32 tenantID, Int32 rotationAttributeGroupId)
        {
            try
            {
                return SharedRequirementPackageManager.GetAttributeGroupById(rotationAttributeGroupId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsAttributeGroupMapped(Int32 tenantID, Int32 requirementAttributeGroupId)
        {
            try
            {
                //BALUtils.GetClinicalRotationRepoInstance(tenantID).IsAttributeGroupMapped(requirementAttributeGroupId);
                return SharedRequirementPackageManager.IsAttributeGroupMapped(requirementAttributeGroupId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        public static List<ApplicantDataListContract> GetRotationMembersForRotationDocs(Int32 tenantId, ClinicalRotationSearchContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationMembersForRotationDocs(searchDataContract, gridCustomPaging);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementCategoryContract> GetReqPkgCatByRotationID(Int32 tenantId, Int32 clinicalRotationID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetReqPkgCatByRotationID(clinicalRotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RotationDocumentContact> GetApplicantDocsByReqCatID(Int32 tenantId, string reqCatIDs, string applicantIds, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetApplicantDocsByReqCatID(reqCatIDs, applicantIds, gridCustomPaging);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RotationDocumentContact> GetApplicantDocumentsByDocIDs(Int32 tenantId, string applicantDocIds, string reqCatIds)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetApplicantDocumentsByDocIDs(applicantDocIds, reqCatIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-3241
        public static List<String> GetAgencyNamesByIds(Int32 tenantId, List<Int32> lstAgencyIds)
        {
            try
            {
                return AgencyHierarchyManager.GetAgencyNamesByIds(tenantId, lstAgencyIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-3315
        /// <summary>
        /// UAT-3315 As an Agency user, I should be able to view and pull from the UI the badge forms once they have been sent.
        /// </summary>
        /// <param name="studentIds"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="SelectedTenantID"></param>
        /// <returns></returns>
        public static List<ApplicantDocumentContract> GetSelectedBadgeDocumentsToExport(String studentIds, Int32 loggedInUserId, Int32 SelectedTenantID)
        {
            List<ApplicantDocumentContract> lstDocContract = new List<ApplicantDocumentContract>();
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(SelectedTenantID).GetSelectedBadgeDocumentsToExport(studentIds, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static Int32 GetRotationCreatorByRotationID(Int32 tenantId, Int32 rotationID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRotationCreatorByRotationID(rotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-3458
        public static List<RequirementExpiringItemListContract> GetRequirementItemsAboutToExpire(Int32 tenantID, Int32 requirementPackageSubscriptionId)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetRequirementItemsAboutToExpire(requirementPackageSubscriptionId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-3485
        public static List<RequirementItemsAboutToExpireContract> GetExpiringRequirementItems(Int32 tenantId, Int32 subEventId, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetExpiringRequirementItems(subEventId, chunkSize);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-3137
        public static List<RequirementCategoriesBeforeGoingToBeRequiredContract> GetRequirementCategoriesBeforeGoingToBeRequired(Int32 tenantId, Int32 subEventId, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantId).GetRequirementCategoriesBeforeGoingToBeRequired(subEventId, chunkSize);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static void AutomaticallyArchiveRotation(Int32 tenantId)
        {
            try
            {
                BALUtils.GetClinicalRotationRepoInstance(tenantId).AutomaticallyArchiveRotation();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-4147
        public static List<ClinicalRotationMembersContract> IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(string rotationIDs, int tenantID, string selectedOrgUserIDs, string selectedClientContactIDs)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(rotationIDs, tenantID, selectedOrgUserIDs, selectedClientContactIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-4323
        public static List<ClinicalRotationDetailContract> GetApplicantDetailsForSelectedRotations(string rotationIDs, int tenantID)
        {
            try
            {
                return BALUtils.GetClinicalRotationRepoInstance(tenantID).GetApplicantDetailsForSelectedRotations(rotationIDs, tenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-4398
        public static string GenerateHTMLForRotationDetails(ClinicalRotationDetailContract clinicalRotationDetailContract)
        {
            if (clinicalRotationDetailContract.IsNullOrEmpty())
            {
                return String.Empty;
            }
            StringBuilder _sbRotationDetails = new StringBuilder();
            _sbRotationDetails.Append("<h4><i>Rotation Details:</i></h4>");
            _sbRotationDetails.Append("<div style='line-height:21px'>");
            _sbRotationDetails.Append("<ul style='list-style-type: disc'>");

            if (!clinicalRotationDetailContract.AgencyName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Agency Name: </b>" + clinicalRotationDetailContract.AgencyName + "</li>");
            }
            if (!clinicalRotationDetailContract.ComplioID.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Complio ID: </b>" + clinicalRotationDetailContract.ComplioID + "</li>");
            }
            if (!clinicalRotationDetailContract.RotationName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Rotation Name: </b>" + clinicalRotationDetailContract.RotationName + "</li>");
            }
            if (!clinicalRotationDetailContract.Department.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Department: </b>" + clinicalRotationDetailContract.Department + "</li>");
            }
            if (!clinicalRotationDetailContract.Program.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Program: </b>" + clinicalRotationDetailContract.Program + "</li>");
            }
            if (!clinicalRotationDetailContract.Course.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Course: </b>" + clinicalRotationDetailContract.Course + "</li>");
            }
            if (!clinicalRotationDetailContract.Term.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Term: </b>" + clinicalRotationDetailContract.Term + "</li>");
            }
            if (!clinicalRotationDetailContract.TypeSpecialty.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Type/Specialty: </b>" + clinicalRotationDetailContract.TypeSpecialty + "</li>");
            }
            if (!clinicalRotationDetailContract.UnitFloorLoc.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Unit/Floor or Location: </b>" + clinicalRotationDetailContract.UnitFloorLoc + "</li>");
            }
            if (!clinicalRotationDetailContract.RecommendedHours.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "# of Recommended Hours: </b>" + clinicalRotationDetailContract.RecommendedHours + "</li>");
            }
            if (!clinicalRotationDetailContract.DaysName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Days: </b>" + clinicalRotationDetailContract.DaysName + "</li>");
            }
            if (!clinicalRotationDetailContract.Shift.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Shift: </b>" + clinicalRotationDetailContract.Shift + "</li>");
            }
            if (!clinicalRotationDetailContract.Time.IsNullOrEmpty() && clinicalRotationDetailContract.Time != "-")
            {
                _sbRotationDetails.Append("<li><b>" + "Time: </b>" + clinicalRotationDetailContract.Time + "</li>");
            }
            if (!clinicalRotationDetailContract.StartDate.IsNullOrEmpty() && !clinicalRotationDetailContract.EndDate.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Dates: </b>" + Convert.ToDateTime(clinicalRotationDetailContract.StartDate).ToString("MM/dd/yyyy") + " - " + Convert.ToDateTime(clinicalRotationDetailContract.EndDate).ToString("MM/dd/yyyy") + "</li>");
            }
            _sbRotationDetails.Append("</ul>");
            _sbRotationDetails.Append("</div>");
            return Convert.ToString(_sbRotationDetails);
        }

        public static string GenerateHTMLForRotationMembersDetails(List<RotationMemberDetailContract> lstRotationMemberDetailContract)
        {
            if (!lstRotationMemberDetailContract.IsNullOrEmpty())
            {
                StringBuilder _sbRotationMembersDetails = new StringBuilder();
                _sbRotationMembersDetails.Append("<h4><i>Rotation Members Details:</i></h4>");
                _sbRotationMembersDetails.Append("<div style='line-height:21px'>");
                _sbRotationMembersDetails.Append(@"<table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse; width:100%; border: 1px solid black;'>");
                _sbRotationMembersDetails.Append("<tr><th style='border: 1px solid black; font-weight: bold;  padding: 3px 3px 3px 3px;'> First Name </th><th style='border: 1px solid black; font-weight: bold;  padding: 3px 3px 3px 3px;'> Last Name </th><th style='border: 1px solid black; font-weight: bold;  padding: 3px 3px 3px 3px;'> Email </th></tr>");
                lstRotationMemberDetailContract.ForEach(rec =>
                {
                    _sbRotationMembersDetails.AppendFormat("<tr><td style='border: 1px solid black;padding: 3px 0px 3px 3px; width: 30%;'>{0}</td><td style='border: 1px solid black;padding: 3px 0px 3px 3px; width: 30%;'>{1}</td><td style='border: 1px solid black;padding: 3px 0px 3px 3px; width: 30%;'>{2}</td></tr>", rec.RotationMemberDetail.ApplicantFirstName, rec.RotationMemberDetail.ApplicantLastName, rec.RotationMemberDetail.EmailAddress);
                });
                _sbRotationMembersDetails.AppendFormat("</table>");

                return Convert.ToString(_sbRotationMembersDetails);
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        public static Boolean UserGranularPermissionDigestion(Int32 selectTenantID, Int32 organizationUserId, String entityCode, Int32 hierarchyNodeId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetComplianceSetupRepoInstance(selectTenantID).UserGranularPermissionDigestion(organizationUserId, entityCode, hierarchyNodeId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-4657

        public static void ManageRequirementVersionTenantMapping(Int32 currentLoggedInUserId)
        {
            try
            {
                BALUtils.GetSharedRequirementPackageRepoInstance().ManageRequirementVersionTenantMapping(currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementPkgVersionTenantMapping> GetRequirementPkgVersionTenantMapping()
        {
            try
            {
                String requirementPkgVersioningStatus_DueCode = lkpRequirementPkgVersioningStatus.DUE.GetStringValue();
                Int32 requirementPkgVersioningStatus_DueId = LookupManager.GetSharedDBLookUpData<lkpRequirementPkgVersioningStatu>().
                            Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_DueCode && !cond.LRPVS_IsDeleted).Select(col => col.LRPVS_ID).FirstOrDefault();

                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPkgVersionTenantMapping(requirementPkgVersioningStatus_DueId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void VersioningRequirementPackages(Int32 tenantId, Int32 currentUserId)
        {
            try
            {
                BALUtils.GetApplicantClinicalRotationRepoInstance(tenantId).VersioningRequirementPackages(currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementCategoryDisassociationTenantMapping> GetRequirementCategoryDisassociationTenantMappingForDisassociation()
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementCategoryDisassociationTenantMappingForDisassociation();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateRequirementPkgVersioningStatusInRequirementPackage(Int32 currentUserId)
        {
            try
            {
                List<lkpRequirementPkgVersioningStatu> lstlkpRequirementPkgVersioningStatu = LookupManager.GetSharedDBLookUpData<lkpRequirementPkgVersioningStatu>()
                                                                                                            .Where(cond => !cond.LRPVS_IsDeleted).ToList();

                if (!lstlkpRequirementPkgVersioningStatu.IsNullOrEmpty() && lstlkpRequirementPkgVersioningStatu.Any())
                {
                    String requirementPkgVersioningStatus_DueCode = lkpRequirementPkgVersioningStatus.DUE.GetStringValue();
                    Int32 requirementPkgVersioningStatus_DueId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_DueCode)
                                                                                                .Select(col => col.LRPVS_ID).FirstOrDefault();

                    String requirementPkgVersioningStatus_InProgressCode = lkpRequirementPkgVersioningStatus.IN_PROGRESS.GetStringValue();
                    Int32 requirementPkgVersioningStatus_InProgressId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_InProgressCode)
                                                                                                .Select(col => col.LRPVS_ID).FirstOrDefault();

                    String requirementPkgVersioningStatus_CompletedCode = lkpRequirementPkgVersioningStatus.COMPLETED.GetStringValue();
                    Int32 requirementPkgVersioningStatus_CompletedId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_CompletedCode)
                                                                                                .Select(col => col.LRPVS_ID).FirstOrDefault();

                    return BALUtils.GetSharedRequirementPackageRepoInstance().UpdateRequirementPkgVersioningStatusInRequirementPackage(currentUserId, requirementPkgVersioningStatus_DueId
                                                                                                                                                , requirementPkgVersioningStatus_InProgressId
                                                                                                                                                , requirementPkgVersioningStatus_CompletedId);
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void ProcessRequirementCategoryDisassociation(Int32 tenantId, Int32 requirementCategoryDisassociationTenantMappingId, Int32 currentUserId)
        {
            try
            {
                BALUtils.GetApplicantClinicalRotationRepoInstance(tenantId).ProcessRequirementCategoryDisassociation(requirementCategoryDisassociationTenantMappingId, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void UpdateRequirementCategoryDisassociationStatus(Int32 backgroundProcessUserId)
        {
            BALUtils.GetSharedRequirementPackageRepoInstance().UpdateRequirementCategoryDisassociationStatus(backgroundProcessUserId);
        }
        #endregion

        #region UAT-4428

        //public static void AddReqCategoryScheduleAction(Int32 tenantId, Boolean isRotStartDateUpdated, Int32 rotationId)
        //{
        //    try
        //    {

        //        if (isRotStartDateUpdated)
        //        {
        //            List<Int32> lstFilteredCategoryIds = new List<Int32>();
        //            List<Int32> lstReqCategoryIds = new List<Int32>();
        //            //Step1 :- Get All Categories in the rotation
        //            List<ClinicalRotationRequirementPackage> lstCRRP = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetReqPackagesByRotId(rotationId);

        //            if (!lstCRRP.IsNullOrEmpty() && lstCRRP.Count > AppConsts.NONE)
        //            {
        //                List<Entity.ClientEntity.RequirementPackage> lstRequirementPackages = lstCRRP.Select(sel => sel.RequirementPackage).ToList();

        //                List<Entity.ClientEntity.RequirementCategory> lstReqCategories = new List<Entity.ClientEntity.RequirementCategory>();
        //                lstRequirementPackages.ForEach(rp =>
        //                {
        //                    lstReqCategories.AddRange(rp.RequirementPackageCategories.Where(c => !c.RPC_IsDeleted && !c.RequirementCategory.RC_IsDeleted)
        //                    .Select(Sel => Sel.RequirementCategory).ToList());

        //                    lstReqCategoryIds.AddRange(rp.RequirementPackageCategories.Where(c => !c.RPC_IsDeleted && !c.RequirementCategory.RC_IsDeleted)
        //                    .Select(Sel => Sel.RequirementCategory.RC_ID).ToList());
        //                });
        //            }


        //            //Step2 :- Filter Categories On the basis of ConstantValue as "$$ROTSDAT$$"
        //            if (!lstReqCategoryIds.IsNullOrEmpty() && lstReqCategoryIds.Count > AppConsts.NONE)
        //            {
        //                Int32 objectTypeId = RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), tenantId).OT_ID;
        //                String constastValue = "$$ROTSDAT$$";

        //                List<Entity.ClientEntity.RequirementObjectTree> lstRequirementObjectTrees = BALUtils.GetRequirementRuleRepoInstance(tenantId).GetReqObjectTreeList(lstReqCategoryIds, objectTypeId);


        //                lstRequirementObjectTrees.ForEach(rot =>
        //                {
        //                    List<Entity.ClientEntity.RequirementObjectRule> lstReqObjectRule = rot.RequirementObjectRules.Where(c => !c.ROR_IsDeleted && c.ROR_IsActive).ToList();

        //                    lstReqObjectRule.ForEach(ror =>
        //                    {
        //                        lstFilteredCategoryIds.AddRange(ror.RequirementObjectRuleDetails.Where(c => !c.RORD_IsDeleted && c.RORD_ConstantValue == constastValue
        //                                                        && c.RequirementObjectRule.RequirementObjectTree.ROT_ObjectID.HasValue)
        //                                                         .Select(sel => sel.RequirementObjectRule.RequirementObjectTree.ROT_ObjectID.Value).ToList());
        //                    });
        //                });

        //            }

        //            /*Step3:- Insert Data into RequirementScheduleAction for filtered categories. 
        //                The scheduled date would be date on which rotation start date is changed.*/

        //            if (!lstFilteredCategoryIds.IsNullOrEmpty() && lstFilteredCategoryIds.Count > AppConsts.NONE)
        //            {
        //                lstCRRP.ForEach(crrp =>
        //                {
        //                    List<RequirementPackageSubscription> lstRps = crrp.ClinicalRotationSubscriptions.Where(c => !c.CRS_IsDeleted).Select(Sel => Sel.RequirementPackageSubscription).ToList();
        //                    lstRps.ForEach(rps =>
        //                    {
        //                        List<Entity.ClientEntity.RequirementCategory> lstReqCategories = rps.RequirementPackage.RequirementPackageCategories
        //                                                                                   .Where(c => !c.RPC_IsDeleted && !c.RequirementCategory.RC_IsDeleted
        //                                                                                            && lstFilteredCategoryIds.Contains(c.RequirementCategory.RC_ID))
        //                                                                                   .Select(Sel => Sel.RequirementCategory).ToList();

        //                        lstReqCategories.ForEach(rc =>
        //                        {

        //                            //@RequirementSubscriptionID INT =rps.RPS_ID
        //                            //, @RootObjectID INT = rps.RPS_RequirementPackageID
        //                            //,@ObjectTypeID INT = select * from lkpObjectType where OT_Code='CAT'
        //                            //, @ObjectID INT =catId
        //                            //,@ObjectHPath VARCHAR(200)  =    @ObjectTypeID+'-'+catId
        //                            //,@RuleMappingID INT = NULL
        //                            //, @RuleExecResult VARCHAR(MAX) =NULL
        //                            //,@RuleActionTypeCode VARCHAR(10)    ="SCCC"
        //                            //, @SystemUserID INT - currentLoggedInUserId
        //                        });
        //                    });
        //                });
        //            }
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        public static Boolean ExecuteCategoryRuleOnRotationDateChange(Int32 tenantId, Int32 rotationId, Int32 currentLoggedInUserId)
        {
            try
            {
                //Step 1:- Get All ClinicalRotationRequirementPackages for rotationId
                List<ClinicalRotationRequirementPackage> _lstCRRP = BALUtils.GetClinicalRotationRepoInstance(tenantId).GetReqPackagesByRotId(rotationId);

                if (!_lstCRRP.IsNullOrEmpty())
                {
                    List<Int32> _lstCategoryIds = new List<Int32>();
                    //   List<Int32> _lstFilteredCategoryIds = new List<Int32>();
                    String constastValue = "$$ROTSDAT$$";
                    Int32 objectTypeId = RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), tenantId).OT_ID;
                    Int32 scheduledActionTypeID = RequirementRuleManager.GetRotScheduledActionTypes(tenantId).Where(cond => cond.RSAT_Code == ReqScheduleAction.EXECUTE_CATEGORY_RULES.GetStringValue()).FirstOrDefault().RSAT_ID;
                    List<RequirementScheduledAction> lstRequirementScheduledActions = new List<RequirementScheduledAction>();

                    _lstCRRP.ForEach(crrp =>
                    {
                        List<Int32> _lstRequirementSubscriptionIds = new List<Int32>();
                        List<Int32> _lstFilteredCategoryIds = new List<Int32>();
                        //Step 2:- Get All Applicants Organization User Id which are not dropped and instructors. 
                        List<Int32> lstOrgUserIds = crrp.ClinicalRotation.ClinicalRotationMembers.Where(c => !c.CRM_IsDropped && !c.CRM_IsDeleted).Select(Sel => Sel.CRM_ApplicantOrgUserID).ToList();
                        List<Int32> lstClientContactOrgUserIds = GetRotationClientContacts(rotationId, tenantId).Where(con => con.OrgUserId != null && con.OrgUserId > AppConsts.NONE)
                                                                    .Select(Sel => Sel.OrgUserId.Value).ToList();

                        //Step 3:- Get All Subscriptions for ClinicalRotationRequirementPackage
                        _lstRequirementSubscriptionIds = crrp.ClinicalRotationSubscriptions.Where(cond => !cond.CRS_IsDeleted && !cond.RequirementPackageSubscription.RPS_IsDeleted
                                                                                                     && (lstOrgUserIds.Contains(cond.RequirementPackageSubscription.RPS_ApplicantOrgUserID)
                                                                                                        ||
                                                                                                        lstClientContactOrgUserIds.Contains(cond.RequirementPackageSubscription.RPS_ApplicantOrgUserID)
                                                                                                        ))
                                                                                            .Select(sel => sel.RequirementPackageSubscription.RPS_ID).ToList();

                        if (!crrp.RequirementPackage.RequirementPackageCategories.Where(con => !con.RPC_IsDeleted && !con.RequirementPackage.RP_IsDeleted).IsNullOrEmpty() && !_lstRequirementSubscriptionIds.IsNullOrEmpty())
                        {
                            Int32 packageId = crrp.CRRP_RequirementPackageID;
                            //Step 4:- Get All categories in a ClinicalRotationRequirementPackage.
                            _lstCategoryIds = crrp.RequirementPackage.RequirementPackageCategories.Where(con => !con.RPC_IsDeleted && !con.RequirementPackage.RP_IsDeleted
                                                        && !con.RequirementCategory.RC_IsDeleted).Select(sel => sel.RequirementCategory.RC_ID).ToList();

                            //Step 5:- Get List of RequirementObjectTree For all categories in a ClinicalRotationRequirementPackage.
                            List<Entity.ClientEntity.RequirementObjectTree> lstRequirementObjectTree = BALUtils.GetRequirementRuleRepoInstance(tenantId).GetReqObjectTreeList(_lstCategoryIds, objectTypeId);
                            lstRequirementObjectTree.ForEach(_requirementObjectTree =>
                            {
                                //Filtering those categories which contains the Rotation Start Date Rule.
                                if (!_requirementObjectTree.RequirementObjectRules.Where(cond => !cond.ROR_IsDeleted && cond.ROR_IsActive
                                                                  && !cond.RequirementObjectRuleDetails.Where(cond1 => !cond1.RORD_IsDeleted && !cond1.lkpConstantType.IsNullOrEmpty()
                                                                  && cond1.lkpConstantType.Code == ConstantType.RotationStartDate.GetStringValue()
                                                                  && !cond1.RORD_ConstantValue.IsNullOrEmpty() && cond1.RORD_ConstantValue == constastValue).IsNullOrEmpty()).IsNullOrEmpty())
                                {
                                    _lstFilteredCategoryIds.Add(_requirementObjectTree.ROT_ObjectID.Value);
                                }
                            });

                            // Creating Requirement Scheduled Action Data for filtered categories.
                            if (!_lstFilteredCategoryIds.IsNullOrEmpty())
                            {
                                _lstRequirementSubscriptionIds.Distinct().ForEach(rpsId =>
                                {
                                    _lstFilteredCategoryIds.Distinct().ForEach(categoryid =>
                                    {
                                        String ObjectHPath = "1-" + crrp.CRRP_RequirementPackageID + "|2-" + categoryid;
                                        lstRequirementScheduledActions.Add(new RequirementScheduledAction()
                                        {
                                            RSA_PackageSubscriptionID = rpsId,
                                            RSA_ObjectTypeID = objectTypeId,
                                            RSA_ObjectID = categoryid,
                                            RSA_Type = scheduledActionTypeID,
                                            RSA_ScheduleDate = DateTime.Now,
                                            RSA_ObjectHPath = ObjectHPath,
                                            RSA_IsActive = true,
                                            RSA_IsDeleted = false,
                                            RSA_CreatedOn = DateTime.Now,
                                            RSA_CreatedByID = currentLoggedInUserId
                                        });
                                    });
                                });
                            }
                        }
                    });
                   
                   
                    //Save created data in DB.
                    BALUtils.GetRequirementRuleRepoInstance(tenantId).SaveScheduledActions(lstRequirementScheduledActions);
                    return InsertSystemSeriveTrigger(currentLoggedInUserId, tenantId, LkpSystemService.EXECUTE_REQUIREMENT_RULES_SERVICE);
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static bool InsertSystemSeriveTrigger(int currentUserId, int tenantId, string systemServiceCode)
        {
           Entity.lkpSystemService reOccurRuleService = SecurityManager.GetSystemServiceByCode(systemServiceCode);
           Entity.SystemServiceTrigger systemServiceTrigger = new Entity.SystemServiceTrigger();
           if (reOccurRuleService != null)
               systemServiceTrigger.SST_SystemServiceID = reOccurRuleService.SS_ID;
           systemServiceTrigger.SST_TenantID = tenantId;
           systemServiceTrigger.SST_IsActive = true;
           systemServiceTrigger.SST_CreatedByID = currentUserId;
           systemServiceTrigger.SST_CreatedOn = DateTime.Now;
           return SecurityManager.AddSystemServiceTrigger(systemServiceTrigger);
        }

        #endregion
    }
}
