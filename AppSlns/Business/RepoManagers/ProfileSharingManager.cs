using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System.Data;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Xml.Linq;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.UI.Contract.SysXSecurityModel;
using Business.ReportExecutionService;
using System.Configuration;
using System.IO;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.Templates;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using System.Web;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System.Xml;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;

namespace Business.RepoManagers
{
    public static class ProfileSharingManager
    {
        //Global variables
        public static String _centralLoginUrl = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_CENTRAL_LOGIN_URL].IsNullOrEmpty()
                  ? String.Empty
                  : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_CENTRAL_LOGIN_URL]);
        public static String _profileSharingURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                ? String.Empty
                : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

        /// <summary>
        /// Gets the Lookup Expiration Types i.e. lkpInvitationExpirationType
        /// </summary> 
        /// <returns></returns>
        public static List<lkpInvitationExpirationType> GetExpirationTypes()
        {
            try
            {
                try
                {
                    var _lkpExpirationTypes = LookupManager.GetSharedDBLookUpData<lkpInvitationExpirationType>().ToList();
                    return _lkpExpirationTypes.Where(et => !et.IsDeleted).ToList();
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
        /// Gets the Lookup ApplicantMetaData Types i.e. ApplicantInvitationMetatData
        /// </summary>  
        /// <returns></returns>
        public static List<ApplicantInvitationMetaData> GetApplicantMetaData()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<ApplicantInvitationMetaData>().ToList();
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
        /// Gets the Lookup for lkpInvitationSharedInfoType for Tenant
        /// </summary>  
        /// <returns></returns>
        public static List<Entity.ClientEntity.lkpInvitationSharedInfoType> GetSharedInfoType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpInvitationSharedInfoType>(tenantId).ToList();
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
        /// Gets the Lookup for lkpInvitationSharedInfoType from Shared DB
        /// </summary>  
        /// <returns></returns>
        public static List<Entity.SharedDataEntity.lkpInvitationSharedInfoType> GetSharedInfoType()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpInvitationSharedInfoType>().Where(x => !x.IsDeleted).ToList();
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
        /// Gets the Invitation related data for the selected Invitation, in Edit Mode
        /// </summary>  
        /// <returns></returns>
        public static InvitationDetailsContract GetInvitationData(Int32 invitationId, Int32 tenantId)
        {
            try
            {
                var _invitationBasicDetails = ProfileSharingManager.GetInvitationDetails(invitationId);
                var _invitationDetails = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetInvitationData(invitationId);

                var _invitation = new InvitationDetailsContract();
                _invitation.InvitationToken = _invitationBasicDetails.PSI_Token;
                _invitation.PSIId = _invitationBasicDetails.PSI_ID;
                _invitation.Name = _invitationBasicDetails.PSI_InviteeName;
                _invitation.Agency = _invitationBasicDetails.PSI_InviteeAgency;
                _invitation.EmailAddress = _invitationBasicDetails.PSI_InviteeEmail;
                _invitation.Phone = _invitationBasicDetails.PSI_InviteePhone;
                _invitation.CustomMessage = _invitationBasicDetails.PSI_InvitationMessage;
                _invitation.InviteeOrgUserId = _invitationBasicDetails.PSI_InviteeOrgUserID;
                //UAT-2447
                _invitation.IsInternationalPhone = _invitationBasicDetails.PSI_IsInternationalInviteePhone;


                _invitation.ExpirationTypeCode = _invitationBasicDetails.lkpInvitationExpirationType.Code;
                _invitation.MaxViews = _invitationBasicDetails.PSI_MaxViews;
                _invitation.ExpirationDate = _invitationBasicDetails.PSI_ExpirationDate;
                if (_invitationBasicDetails.ProfileSharingInvitationGroup.IsNotNull())
                {
                    _invitation.AgencyId = _invitationBasicDetails.ProfileSharingInvitationGroup.PSIG_AgencyID.HasValue
                        ? _invitationBasicDetails.ProfileSharingInvitationGroup.PSIG_AgencyID : null;
                }

                //_invitation.ComplianceSharedInfoTypeCode = _invitationBasicDetails.lkpInvitationSharedInfoType.Code;
                //_invitation.BkgSharedInfoTypeCode = _invitationBasicDetails.lkpInvitationSharedInfoType1.Code;

                var _metaDataCodes = new List<string>();

                foreach (var metaData in _invitationBasicDetails.ApplicantSharedInvitationMetaDatas)
                {
                    _metaDataCodes.Add(metaData.ApplicantInvitationMetaData.AIMD_Code);
                }
                _invitation.SharedApplicantMetaDataCode = _metaDataCodes;

                #region Rotation Details

                var invitationRotationDetails = _invitationBasicDetails.ProfileSharingInvitationGroup.ProfileSharingInvitationRotationDetails.FirstOrDefault(x => !x.PSIRD_IsDeleted);
                if (invitationRotationDetails.IsNotNull())
                {
                    _invitation.RotationDetail = new ClinicalRotationMemberDetail();

                    _invitation.RotationDetail.RotationName = invitationRotationDetails.PSIRD_RotationName;
                    _invitation.RotationDetail.TypeSpecialty = invitationRotationDetails.PSIRD_TypeSpecialty;
                    _invitation.RotationDetail.Department = invitationRotationDetails.PSIRD_Department;
                    _invitation.RotationDetail.Program = invitationRotationDetails.PSIRD_Program;
                    _invitation.RotationDetail.Course = invitationRotationDetails.PSIRD_Course;
                    _invitation.RotationDetail.Term = invitationRotationDetails.PSIRD_Term;
                    _invitation.RotationDetail.UnitFloorLoc = invitationRotationDetails.PSIRD_UnitFloor;
                    _invitation.RotationDetail.Shift = invitationRotationDetails.PSIRD_Shift;
                    _invitation.RotationDetail.StartTime = invitationRotationDetails.PSIRD_StartTime;
                    _invitation.RotationDetail.EndTime = invitationRotationDetails.PSIRD_EndTime;
                    _invitation.RotationDetail.StartDate = invitationRotationDetails.PSIRD_StartDate;
                    _invitation.RotationDetail.EndDate = invitationRotationDetails.PSIRD_EndDate;

                    var invitationRotationDays = invitationRotationDetails.ProfileSharingInvitationRotationDays.Where(x => !x.PSIRDY_IsDeleted).ToList();
                    if (!invitationRotationDays.IsNullOrEmpty())
                        _invitation.RotationDetail.DaysIdList = String.Join(",", invitationRotationDays.Select(x => x.PSIRDY_WeekDayID));
                }
                #endregion

                // _invitation.lstComplianceData = new List<Tuple<Int32, Boolean, List<Int32>>>();
                _invitation.lstComplianceData = new List<ComplianceInvitationData>();

                foreach (var subs in _invitationDetails.Item1)
                {
                    var _catIds = new List<Int32>();
                    _catIds.AddRange(subs.SharedSubscriptionCategories.Where(ssc => ssc.SSC_IsDeleted == false).Select(ssc => ssc.SSC_ComplianceCategoryID).ToList());

                    _invitation.lstComplianceData.Add(new ComplianceInvitationData
                    {
                        IsCompletePkgSelected = subs.SCS_IsCompletePackageShared,
                        PkgSubId = subs.SCS_PackageSubscriptionID,
                        lstCategoryIds = _catIds,
                        ComplianceSharedInfoTypeCode = subs.lkpInvitationSharedInfoType.Code,
                        ComplianceSharedInfoTypeId = subs.SCS_SharedInfoTypeID
                    });
                }

                _invitation.lstBkgData = new List<BkgInvitationData>();
                foreach (var bkgPkg in _invitationDetails.Item2)
                {
                    var _bkgSvcGrpIds = new List<Int32>();
                    _bkgSvcGrpIds.AddRange(bkgPkg.SharedBkgPackageSvcGroups.Where(bpsg => bpsg.BPSG_IsDeleted == false).Select(bpsg => bpsg.BPSG_BkgSvcGroupID).ToList());


                    var _lstSharedTypeCodes = new List<string>();
                    var _lstSharedTypeIds = new List<Int32>();

                    foreach (var infoType in bkgPkg.InvitationSharedInfoMappings)
                    {
                        if (infoType.lkpInvitationSharedInfoType.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue())
                        {
                            _lstSharedTypeCodes.Add(infoType.lkpInvitationSharedInfoType.Code);
                            _lstSharedTypeIds.Add(infoType.ISIM_InvitationSharedInfoTypeID);
                        }
                    }

                    _invitation.lstBkgData.Add(new BkgInvitationData
                    {
                        BOPId = bkgPkg.SBP_BkgOrderPackageID,
                        lstSvcGrpIds = _bkgSvcGrpIds,
                        //BkgSharedInfoTypeCode = bkgPkg.lkpInvitationSharedInfoType.Code,  UAT-1213
                        //BkgSharedInfoTypeId = bkgPkg.SBP_SharedInfoTypeID, UAT-1213
                        LstBkgSharedInfoTypeCode = _lstSharedTypeCodes,//UAT-1213
                        LstBkgSharedInfoTypeId = _lstSharedTypeIds//UAT-1213
                    });
                }
                return _invitation;
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
        /// Save the new invitation.
        /// </summary>  
        /// <returns></returns>
        public static Tuple<Int32, Guid> SaveInvitationDetails(InvitationDetailsContract invitationDetails, Int32 tenantId, Int32 generatedInvitationGroupID)
        {
            try
            {
                var _metaData = LookupManager.GetSharedDBLookUpData<ApplicantInvitationMetaData>().ToList();
                invitationDetails.SharedApplicantMetaDataIds = _metaData.Where(amd => invitationDetails.SharedApplicantMetaDataCode.Contains(amd.AIMD_Code) && amd.AIMD_IsDeleted == false)
                                                                .Select(amd => amd.AIMD_ID).ToList();

                var _invitationData = SaveProfileSharingInvitation(invitationDetails, generatedInvitationGroupID);
                var _lstSharedInfoTypes = LookupManager.GetLookUpData<Entity.ClientEntity.lkpInvitationSharedInfoType>(tenantId).ToList();

                if (_invitationData.Item1 > AppConsts.NONE)
                {
                    invitationDetails.PSIId = _invitationData.Item1;
                    BALUtils.GetProfileSharingClientRepoInstance(tenantId).SaveInvitationDetails(invitationDetails, _lstSharedInfoTypes);

                    // Change previous invitation status to hide from the list of previous invitee
                    if (invitationDetails.IsStatusUpdateRequired)
                    {
                        UpdateInvitationStatus(LkpInviationStatusTypes.DATA_CHANGED_INVITATION_REVOKED.GetStringValue(), invitationDetails.PreviousPSIId, invitationDetails.CurrentUserId);
                    }

                    //return _invitationData.Item2;
                }
                //else
                //{
                //    return Guid.NewGuid();
                //}
                return _invitationData;
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
        /// Save the Bulk Invitation Details in Tenant, sent by admin/client admin
        /// </summary>
        /// <param name="lstInvitations"></param>
        /// <param name="invitationGroup"></param>
        /// <param name="invitationGroup"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static void SaveAdminInvitationDetails(List<InvitationDetailsContract> lstInvitations, List<ProfileSharingInvitation> lstInvitationsGenerated,
                                                      List<SharedUserSubscriptionSnapshotContract> lstSharedUserSnapshot, Int32 rotationId, Int32 tenantId, Int32 agencyID)
        {
            try
            {
                var _lstSharedInfoTypes = LookupManager.GetLookUpData<Entity.ClientEntity.lkpInvitationSharedInfoType>(tenantId).ToList();
                var _profileSharingGroupId = lstInvitationsGenerated.First().PSI_ProfileSharingInvitationGroupID;
                var _reviewStatusId = AppConsts.NONE;

                lstInvitations.ForEach(inv =>
                {
                    inv.PSIId = lstInvitationsGenerated.Where(newInv => newInv.InvitationIdentifier == inv.InvitationIdentifier).First().PSI_ID;
                    inv.PSIGroupId = Convert.ToInt32(_profileSharingGroupId);
                });

                if (rotationId > AppConsts.NONE)
                {
                    _reviewStatusId = GetRotationReviewStatusByCode(tenantId, SharedUserRotationReviewStatus.PENDING_REVIEW.GetStringValue());
                }

                BALUtils.GetProfileSharingClientRepoInstance(tenantId).SaveAdminInvitationDetails(lstInvitations, _lstSharedInfoTypes, lstSharedUserSnapshot, rotationId, _reviewStatusId, agencyID);
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
        /// Save the details of the Packages & their categories/Service groups, which were either not included or partially included for sharing, during 'Submit Later' option
        /// </summary>
        /// <param name="lstSharedPkgData"></param>
        /// <param name="tenantId"></param>
        public static void SaveScheduledExcludedPackageData(List<SharingPackageDataContract> lstSharedPkgData, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetProfileSharingClientRepoInstance(tenantId).SaveScheduledExcludedPackageData(lstSharedPkgData, currentUserId);
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
        /// Update the existing Invitaiton.
        /// </summary>  
        /// <returns></returns>
        public static void UpdateInvitationDetails(InvitationDetailsContract invitationDetails, Int32 tenantId)
        {
            try
            {
                var _lstSharedInfoTypes = LookupManager.GetLookUpData<Entity.ClientEntity.lkpInvitationSharedInfoType>(tenantId).ToList();
                BALUtils.GetProfileSharingClientRepoInstance(tenantId).UpdateInvitationDetails(invitationDetails, _lstSharedInfoTypes);
                BALUtils.GetProfileSharingRepoInstance().UpdateProfileSharingInvRotationDetails(invitationDetails);
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
        /// Gets the Email Subject and Content from AppDBConfiguration, based on the keys.
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <param name="contentCode"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Dictionary<String, String> GetInvitationEmailContent(String subjectCode, String contentCode, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetInvitationEmailContent(subjectCode, contentCode);
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

        //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
        public static Dictionary<String, String> GetInvitationEmailContentUsingSubEvent(Int32 tenantId, String CommSubEventCode)
        {
            try
            {

                return BALUtils.GetTemplatesRepoInstance().GetInvitationEmailContent(tenantId, CommSubEventCode);

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
        /// Returns whether a new invitaion is to be sent or existing is to be updated.
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Tuple<Boolean, ProfileSharingInvitation> IsNewInvitationRequired(InvitationDetailsContract invitationDetails, Int32 tenantId)
        {
            try
            {
                var _invitationBasicDetails = GetInvitationDetails(invitationDetails.PSIId);
                var _sharedMetaData = _invitationBasicDetails.ApplicantSharedInvitationMetaDatas.ToList();
                var _isNewInvitationRequired = IsNewInvitationRequired(invitationDetails, _invitationBasicDetails, _sharedMetaData);
                return new Tuple<Boolean, ProfileSharingInvitation>(_isNewInvitationRequired, _invitationBasicDetails);
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
        /// Get the list of packages associated for a particular Invitation, for Re-send invitation Email
        /// </summary>
        /// <param name="invitationId"></param>
        /// <returns></returns>
        public static Tuple<List<SharedComplianceSubscription>, List<SharedBkgPackage>> GetSharedPackages(Int32 invitationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetSharedPackages(invitationId);
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

        #region MANAGE AGENCY
        public static List<Agency> GetAgencies(Int32 TenantID, Boolean IsAdmin, Boolean isAgencyUser, Guid userID, Boolean getNotVerfiedAgenciesAlso = false)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencies(TenantID, IsAdmin, isAgencyUser, userID, getNotVerfiedAgenciesAlso);
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

        public static List<ManageAgencyContract> GetAgencyDetail(Int32 TenantID, String agencyIDs)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyDetail(TenantID, agencyIDs);
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

        public static Tuple<Int32, Dictionary<Int32, Int32>, Int32> SaveAgencies(AgencyContract AgencyData, List<Int32> lstTenantID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveAgencies(AgencyData, lstTenantID);
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

        public static Tuple<String, Dictionary<Int32, Int32>> UpdateAgencies(AgencyContract agencyData, List<Int32> tenantIDs_Added, List<Int32> tenantIDs_Removed)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().UpdateAgencies(agencyData, tenantIDs_Added, tenantIDs_Removed);
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

        public static List<AgencyInstitution> GetAgencyInstitutionForAgencies(Int32 tenantID, IEnumerable<int> lstAgencyId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyInstitutionForAgencies(lstAgencyId);
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

        public static string DeleteAgency(AgencyContract agencyData)
        {
            try
            {
                var _profileSharingRepoInstance = BALUtils.GetProfileSharingRepoInstance();
                List<Int32> _lstAgencyIds = new List<Int32>();
                _lstAgencyIds.Add(agencyData.AgencyID);
                List<AgencyInstitution> lstAgencyInstitutions = _profileSharingRepoInstance.GetAgencyInstitutionForAgencies(_lstAgencyIds);
                var _isAgencyAssociated = ClinicalRotationManager.IsAgencyAssociated(lstAgencyInstitutions);

                if (_isAgencyAssociated)
                {
                    return AppConsts.AG_DELETION_CR_ASSOCIATED_MSG;
                }

                String status = _profileSharingRepoInstance.DeleteAgency(agencyData);
                if (status == AppConsts.AG_DELETED_SUCCESS_MSG)
                {
                    Boolean isSuccess = true;
                    if (agencyData.IsAdmin)
                    {
                        foreach (AgencyInstitution agencyInstituion in lstAgencyInstitutions)
                        {
                            isSuccess = DeleteAgencyHierarchyMappings(agencyInstituion.AGI_TenantID.Value, agencyInstituion.AGI_AgencyID.Value, agencyData.LoggedInUserID, agencyInstituion.AGI_ID);
                            //UAT-2640
                            DeleteAgencyHierarchyAgency(agencyInstituion.AGI_AgencyID.Value, agencyData.LoggedInUserID, true, agencyData.TenantID);
                        }
                    }
                    else
                    {
                        isSuccess = DeleteAgencyHierarchyMappings(agencyData.TenantID, agencyData.AgencyID, agencyData.LoggedInUserID, lstAgencyInstitutions.Where(d => d.AGI_TenantID == agencyData.TenantID && !d.AGI_IsDeleted).Select(f => f.AGI_ID).FirstOrDefault());
                        //UAT-2640                       
                        DeleteAgencyHierarchyAgency(agencyData.AgencyID, agencyData.LoggedInUserID, false, agencyData.TenantID);
                    }
                    if (isSuccess)
                    {
                        status = AppConsts.AG_DELETED_SUCCESS_MSG;
                    }
                    else
                    {
                        status = AppConsts.AG_DELETED_HIERARCHY_ERROR_MSG;
                    }
                }
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

        public static Boolean DeleteAgencyHierarchyMappings(Int32 tenantID, Int32 agencyID, Int32 loggedInUserID, Int32 agencyInstitutionId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID)
                                    .DeleteAgencyHierarchyMappings(agencyID, loggedInUserID, agencyInstitutionId);
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
        //UAT-2640
        public static Boolean DeleteAgencyHierarchyAgency(Int32 agencyID, Int32 loggedInUserID, Boolean IsAdmin, Int32 TenantID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().DeleteAgencyHierarchyAgency(agencyID, loggedInUserID, IsAdmin, TenantID);
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

        public static Boolean IsNPINumberExist(String npiNumber)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsNPINumberExist(npiNumber);
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
        /// Returns List of AgencyHierarchyIDs Corresponding to AgencyID
        /// </summary>
        /// <returns></returns>
        public static List<Int32> GetAgencyHierarchyIDsByAgencyID(Int32 agencyID)
        {
            return BALUtils.GetProfileSharingRepoInstance().GetAgencyHierarchyIDsByAgencyID(agencyID);
        }


        public static List<AgencyInstitution> GetAgencyInstitutionForAgencyuser(Int32 agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyInstitutionForAgencyuser(agencyUserID);
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
        /// Check if there is any need to send a new invitation
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <param name="actualInvitation"></param>
        private static Boolean IsNewInvitationRequired(InvitationDetailsContract invitationDetails, ProfileSharingInvitation actualInvitation, List<ApplicantSharedInvitationMetaData> sharedData)
        {
            return IsInviteeInformationChanged(invitationDetails, actualInvitation)
                || IsExpirationInformationChanged(invitationDetails, actualInvitation)
                || IsSharedMetaDataChanged(invitationDetails, sharedData);
        }

        /// <summary>
        /// Check if there is any Change in Invitee Information
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <param name="actualInvitation"></param>
        /// <returns></returns>
        private static Boolean IsInviteeInformationChanged(InvitationDetailsContract invitationDetails, ProfileSharingInvitation actualInvitation)
        {
            return invitationDetails.Name.ToLower().Trim() != actualInvitation.PSI_InviteeName.ToLower().Trim()
                            || invitationDetails.EmailAddress.ToLower().Trim() != actualInvitation.PSI_InviteeEmail.ToLower().Trim()
                            || invitationDetails.Phone.ToLower().Trim() != actualInvitation.PSI_InviteePhone.ToLower().Trim()
                            || invitationDetails.Agency.ToLower().Trim() != actualInvitation.PSI_InviteeAgency.ToLower().Trim();
        }

        /// <summary>
        /// Check if there is any change in Expiration criteria
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <param name="actualInvitation"></param>
        /// <returns></returns>
        private static Boolean IsExpirationInformationChanged(InvitationDetailsContract invitationDetails, ProfileSharingInvitation actualInvitation)
        {
            // Case 1 : If Type is changed from No criteria to any criteria or vice-versa
            // Case 2 : If Type is same and Expiration date is changed
            // Case 3 : If Type is same and Number of Views is changed
            return invitationDetails.ExpirationTypeCode != actualInvitation.lkpInvitationExpirationType.Code
                ||
                (
                   invitationDetails.ExpirationTypeCode == actualInvitation.lkpInvitationExpirationType.Code
                   && invitationDetails.ExpirationTypeCode == InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue()
                   && invitationDetails.ExpirationDate != actualInvitation.PSI_ExpirationDate
                )
                   ||
                (
                   invitationDetails.ExpirationTypeCode == actualInvitation.lkpInvitationExpirationType.Code
                   && invitationDetails.ExpirationTypeCode == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue()
                   && invitationDetails.MaxViews != actualInvitation.PSI_MaxViews
                );
        }

        /// <summary>
        /// Check if there is any change in Shared Meta Data. 
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <param name="actualInvitation"></param>
        /// <returns></returns>
        private static Boolean IsSharedMetaDataChanged(InvitationDetailsContract invitationDetails, List<ApplicantSharedInvitationMetaData> sharedData)
        {
            if (invitationDetails.SharedApplicantMetaDataCode.Count() != sharedData.Count())
            {
                return true;
            }
            foreach (var data in sharedData)
            {
                if (!invitationDetails.SharedApplicantMetaDataCode.Contains(data.ApplicantInvitationMetaData.AIMD_Code))
                {
                    return true;
                }
            }
            return false;
        }

        #region Shared Invitation Detail for compliance and background packages

        /// <summary>
        /// Get the List of SharedComplianceSubscription
        /// </summary>  
        /// <returns></returns>
        public static List<SharedComplianceSubscription> GetSharedComplianceSubscriptions(Int32 tenantId, Int32 invitationId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetSharedComplianceSubscriptions(invitationId);
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
        /// Get the List of SharedBkgPackage
        /// </summary>  
        /// <returns></returns>
        public static List<SharedInvitationBackgroundDetail> GetSharedBkgPackages(Int32 tenantId, Int32 invitationId, Int32 CurrentLoggedInUserId, Boolean IsIndividualShare)
        {
            try
            {
                List<SharedBkgPackage> tempBkgPackages = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetSharedBkgPackages(invitationId);
                List<SharedInvitationBackgroundDetail> finalSharedBkgDetail = new List<SharedInvitationBackgroundDetail>();
                List<Int32> bkgOrderIds = tempBkgPackages.Select(x => x.BkgOrderPackage.BOP_BkgOrderID).ToList();
                List<vwBkgOrderFlagged> bkgOrderFlaggedList = GetBkgOrderFlagged(tenantId, bkgOrderIds);
                tempBkgPackages.ForEach(bkgPkg =>
                {

                    Int32 pkgCount = AppConsts.ONE;
                    String packageName = bkgPkg.BkgOrderPackage.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label.IsNullOrEmpty() ?
                                         bkgPkg.BkgOrderPackage.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name :
                                         bkgPkg.BkgOrderPackage.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label;

                    Int32 packageId = bkgPkg.BkgOrderPackage.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID;
                    Int32 masterOrderId = bkgPkg.BkgOrderPackage.BkgOrder.BOR_MasterOrderID;
                    String masterOrderNumber = bkgPkg.BkgOrderPackage.BkgOrder.Order.OrderNumber;
                    String sharedInfoTypeCode = String.Empty;
                    Boolean isColorFlagVisible = false;
                    Boolean isResultPDFVisible = false;
                    Boolean isFlagStatusVisible = false;
                    String colorFlagImagePath = String.Empty;
                    String flagStatusImagePath = "~/images/small";
                    Boolean isOrderFlagged = false;
                    var permisionList = new List<String>();
                    String institutionHierarchy = String.Empty;

                    if (bkgPkg.InvitationSharedInfoMappings.IsNotNull() && bkgPkg.InvitationSharedInfoMappings.Any(cond => cond.ISIM_IsDeleted == false))
                    {
                        permisionList = bkgPkg.InvitationSharedInfoMappings.Where(cond => cond.ISIM_IsDeleted == false)
                                        .Select(slct => slct.lkpInvitationSharedInfoType.Code).ToList();
                    }

                    if (!permisionList.Contains(SharedInfoType.BACKGROUND_ATTESTATION_ONLY.GetStringValue()))
                    {
                        //UAT-1610: Add "Institution Hierarchy" column to the student grid in Rotation Details
                        institutionHierarchy = bkgPkg.BkgOrderPackage.BkgOrder.Order.DeptProgramMapping1.DPM_Label;

                        //UAT-1213: Updates to Agency User background check permissions.
                        vwBkgOrderFlagged bkgOrderFlagged = bkgOrderFlaggedList.FirstOrDefault(cnd => cnd.BOR_ID == bkgPkg.BkgOrderPackage.BOP_BkgOrderID);
                        if (bkgOrderFlagged.IsNotNull() && bkgOrderFlagged.IsFlagged == true)
                        {
                            isOrderFlagged = true;
                        }
                        //In Case Color flag permission
                        //if (permisionList.Contains(SharedInfoType.COLOR_FLAG.GetStringValue()))
                        //{
                        //    isColorFlagVisible = true;

                        //    if (bkgPkg.BkgOrderPackage.BkgOrder.InstitutionOrderFlag.IsNotNull()
                        //         && bkgPkg.BkgOrderPackage.BkgOrder.InstitutionOrderFlag.lkpOrderFlag.IsNotNull())
                        //    {
                        //        colorFlagImagePath = "~/" + bkgPkg.BkgOrderPackage.BkgOrder.InstitutionOrderFlag.lkpOrderFlag.OFL_FilePath + "/"
                        //                             + bkgPkg.BkgOrderPackage.BkgOrder.InstitutionOrderFlag.lkpOrderFlag.OFL_FileName;
                        //    }

                        //}
                        //In Case Completed result report permission
                        if (permisionList.Contains(SharedInfoType.COMPLETED_RESULT_REPORT.GetStringValue()))
                        {
                            if (bkgPkg.BkgOrderPackage.BkgOrder.BOR_OrderCompleteDate.IsNotNull())
                            {
                                isResultPDFVisible = true;
                            }
                        }
                        //In Case of flagged only result report permission
                        else if (permisionList.Contains(SharedInfoType.FLAGGED_ONLY_RESULT_REPORT.GetStringValue())
                                 && isOrderFlagged
                                 && bkgPkg.BkgOrderPackage.BkgOrder.BOR_OrderCompleteDate.IsNotNull())
                        {
                            isResultPDFVisible = true;
                        }
                        //In Case For Flag status permission
                        if (permisionList.Contains(SharedInfoType.FLAG_STATUS.GetStringValue()) && bkgOrderFlagged.IsNotNull()
                            && bkgPkg.BkgOrderPackage.BkgOrder.BOR_OrderCompleteDate.IsNotNull()
                            )
                        {
                            isFlagStatusVisible = true;
                            if (isOrderFlagged)
                            {
                                flagStatusImagePath = flagStatusImagePath + "/Red.gif";
                            }
                            else
                            {
                                flagStatusImagePath = flagStatusImagePath + "/Green.gif";
                            }
                        }


                        bkgPkg.SharedBkgPackageSvcGroups.ForEach(shrdSvcGrp =>
                        {

                            SharedInvitationBackgroundDetail tempSharedBkgDetail = new SharedInvitationBackgroundDetail();
                            tempSharedBkgDetail.BkgPackageId = packageId;
                            tempSharedBkgDetail.BkgPackageName = packageName;
                            tempSharedBkgDetail.BkgSvcGroupId = shrdSvcGrp.BkgSvcGroup.BSG_ID;
                            tempSharedBkgDetail.BkgSvcGroupName = shrdSvcGrp.BkgSvcGroup.BSG_Name;
                            tempSharedBkgDetail.MasterOrderID = masterOrderId;
                            tempSharedBkgDetail.MasterOrderNumber = masterOrderNumber;
                            tempSharedBkgDetail.IsColorFlagVisible = isColorFlagVisible;
                            tempSharedBkgDetail.IsResultPDFVisible = isResultPDFVisible;
                            tempSharedBkgDetail.ColorFlagPath = colorFlagImagePath;
                            tempSharedBkgDetail.IsHeaderVisible = pkgCount == AppConsts.ONE ? true : false;
                            tempSharedBkgDetail.IsFlagStatusVisible = isFlagStatusVisible;
                            tempSharedBkgDetail.FlagStatusImagePath = flagStatusImagePath;
                            tempSharedBkgDetail.InstitutionHierarchy = institutionHierarchy;

                            finalSharedBkgDetail.Add(tempSharedBkgDetail);
                            pkgCount++;
                        });
                    }

                });

                return finalSharedBkgDetail;
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
        /// Get the List of Shared category list
        /// </summary>  
        /// <returns></returns>
        public static List<ApplicantComplianceCategoryData> GetSharedCategoryList(Int32 tenantId, Int32 packageSubscriptionId, List<Int32> sharedCategoryIds, Int32 snapshotId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetSharedCategoryList(packageSubscriptionId, sharedCategoryIds, snapshotId);
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
        /// To get shared category documents of invitations
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="lstProfileSharingInvitationID"></param>
        /// <returns></returns>
        public static List<InvitationDocumentContract> GetSharedCategoryDocuments(Int32 tenantId, Int32 packageSubscriptionId, String sharedcategoryids)
        {
            try
            {
                DataTable dataForQueue = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetSharedCategoryDocuments(packageSubscriptionId, sharedcategoryids);
                return AssignCategoryDocumentToDataModel(dataForQueue);
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

        private static List<InvitationDocumentContract> AssignCategoryDocumentToDataModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new InvitationDocumentContract
                {
                    ID = x["ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ID"]),
                    ApplicantDocumentID = x["ApplicantDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantDocumentID"]),
                    Name = Convert.ToString(x["FirstName"]) + Convert.ToString(x["LastName"]),
                    FileName = Convert.ToString(x["FileName"]),
                    DocumentPath = Convert.ToString(x["DocumentPath"]),
                    ComplianceCategoryID = x["ComplianceCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ComplianceCategoryID"]),
                    CategoryName = Convert.ToString(x["CategoryName"]),
                    OrganizationUserID = x["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["OrganizationUserID"])
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

        /// <summary>
        /// UN USED METHOD NEED DELETION
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public static List<ApplicantInvitationMetaData> GetApplicantInvitationMetaData(int TenantID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetApplicantInvitationMetaData();
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
        /// Generate the HTML string of the Applicant data to be shared in invitation
        /// </summary>
        /// <param name="organizationUser"></param>
        /// <param name="lstMetaDataSharedCodes"></param>
        /// <returns></returns>
        public static String GenerateApplicantMetaDataString(OrganizationUserContract applicant, List<String> lstMetaDataSharedCodes, Int32 tenantId, String schoolContactName, String schoolContactEmailId, String instPrecep = default(String))
        {
            //var organizationUserData = BALUtils.GetSecurityRepoInstance().GetOrganizationUser(currentUserId);
            //if (applicant.IsNullOrEmpty())
            //{
            //    return String.Empty;
            //}
            StringBuilder _sbSharedInformation = new StringBuilder();
            _sbSharedInformation.Append("<div style='line-height:21px'>");
            _sbSharedInformation.Append("<ul style='list-style-type: disc'>");
            foreach (var sharedMetaDataCode in lstMetaDataSharedCodes)
            {
                if (sharedMetaDataCode == LkpApplicanteMetaData.NAME_FIELD.GetStringValue())
                {
                    _sbSharedInformation.Append("<li><b>" + "Name: </b>" + applicant.FirstName + " " + applicant.MiddleName
                                                          + " " + applicant.LastName + "</li>");
                }
                else if (sharedMetaDataCode == LkpApplicanteMetaData.EMAIL_ADDRESS_FIELD.GetStringValue())
                {
                    _sbSharedInformation.Append("<li><b>" + "Email Address: </b>" + applicant.Email + "</li>");
                }
                else if (sharedMetaDataCode == LkpApplicanteMetaData.GENDER_FIELD.GetStringValue())
                {
                    _sbSharedInformation.Append("<li><b>" + "Gender: </b>" +
                       (applicant.Gender.IsNull()
                        ? "Not Specified"
                        : applicant.Gender) + "</li>");
                }
                else if (sharedMetaDataCode == LkpApplicanteMetaData.SECONDARY_EMAIL_ADDRESS.GetStringValue())
                {
                    _sbSharedInformation.Append("<li><b>" + "Secondary Email Address: </b>" + applicant.SecondaryEmailAddress + "</b></li>");
                }
                else if (sharedMetaDataCode == LkpApplicanteMetaData.PHONE_NUMBER_FIELD.GetStringValue())
                {
                    _sbSharedInformation.Append("<li><b>" + "Phone Number: </b>" + applicant.Phone + "</li>");
                }
                else if (sharedMetaDataCode == LkpApplicanteMetaData.ADDRESS_FIELD.GetStringValue())
                {
                    _sbSharedInformation.Append("<li><b>" + "Address: </b>" + applicant.Address1 +
                            (applicant.Address2.IsNullOrEmpty() ? String.Empty : ", " + applicant.Address2));
                    _sbSharedInformation.Append(applicant.County.IsNullOrEmpty() ? ", " + String.Empty : applicant.County);
                    _sbSharedInformation.Append("<br /><span style='padding-left:52px'> " + applicant.City + ", " + applicant.State + "</span>");
                    _sbSharedInformation.Append("<br /><span style='padding-left:52px'> " + applicant.Country + " - " + applicant.Zipcode + "</span>");
                    _sbSharedInformation.Append("</li>");
                }
                //UAT-3006
                else if (sharedMetaDataCode == LkpApplicanteMetaData.SCHOOL_CONTACT_NAME.GetStringValue())
                {
                    _sbSharedInformation.Append("<li><b>" + "School Contact Name: </b>" + schoolContactName + "</b></li>");
                }
                else if (sharedMetaDataCode == LkpApplicanteMetaData.SCHOOL_CONTACT_EMAIL_ADDRESS.GetStringValue())
                {
                    _sbSharedInformation.Append("<li><b>" + "School Contact Email Address: </b>" + schoolContactEmailId + "</b></li>");
                }
                //UAT-3662
                else if (sharedMetaDataCode == LkpApplicanteMetaData.INSTRUCTOR_PRECEPTOR.GetStringValue() && !String.IsNullOrEmpty(instPrecep))
                {
                    _sbSharedInformation.Append("<li><b>" + "Instructor/Preceptor: </b>" + instPrecep + "</b></li>");
                }
            }
            _sbSharedInformation.Append("</ul>");
            _sbSharedInformation.Append("</div>");
            return Convert.ToString(_sbSharedInformation);
        }

        /// <summary>
        /// Generate the HTML string of the Applicant data to be shared in invitation
        /// </summary>
        /// <param name="organizationUser"></param>
        /// <param name="lstMetaDataSharedCodes"></param>
        /// <returns></returns>
        public static String GenerateApplicantMetaDataStringRotSharing(List<OrganizationUserContract> lstapplicant, List<String> lstMetaDataSharedCodes, Int32 tenantId)
        {
            StringBuilder _sbSharedInformation = new StringBuilder();
            _sbSharedInformation.Append("<h4><i>Student/Instructor/Preceptor Details:</i></h4>");
            foreach (var applicant in lstapplicant)
            {
                _sbSharedInformation.Append("<div style='line-height:21px'>");
                _sbSharedInformation.Append("<ul style='list-style-type: disc'>");
                foreach (var sharedMetaDataCode in lstMetaDataSharedCodes)
                {
                    if (sharedMetaDataCode == LkpApplicanteMetaData.NAME_FIELD.GetStringValue())
                    {
                        _sbSharedInformation.Append("<li><b>" + "Name: </b>" + applicant.FirstName + " " + applicant.MiddleName
                                                              + " " + applicant.LastName + "</li>");
                    }
                    else if (sharedMetaDataCode == LkpApplicanteMetaData.EMAIL_ADDRESS_FIELD.GetStringValue())
                    {
                        _sbSharedInformation.Append("<li><b>" + "Email Address: </b>" + applicant.Email + "</li>");
                    }
                    else if (sharedMetaDataCode == LkpApplicanteMetaData.GENDER_FIELD.GetStringValue() && applicant.IsApplicant)
                    {
                        _sbSharedInformation.Append("<li><b>" + "Gender: </b>" +
                           (applicant.Gender.IsNull()
                            ? "Not Specified"
                            : applicant.Gender) + "</li>");
                    }
                    else if (sharedMetaDataCode == LkpApplicanteMetaData.SECONDARY_EMAIL_ADDRESS.GetStringValue() && applicant.IsApplicant)
                    {
                        _sbSharedInformation.Append("<li><b>" + "Secondary Email Address: </b>" + applicant.SecondaryEmailAddress + "</b></li>");
                    }
                    else if (sharedMetaDataCode == LkpApplicanteMetaData.PHONE_NUMBER_FIELD.GetStringValue())
                    {
                        _sbSharedInformation.Append("<li><b>" + "Phone Number: </b>" + applicant.Phone + "</li>");
                    }
                    else if (sharedMetaDataCode == LkpApplicanteMetaData.ADDRESS_FIELD.GetStringValue() && applicant.IsApplicant)
                    {
                        //var _address = StoredProcedureManagers.GetAddressByAddressHandleId(applicant.AddressHandleID.Value, tenantId);

                        _sbSharedInformation.Append("<li><b>" + "Address: </b>" + applicant.Address1 +
                            (applicant.Address2.IsNullOrEmpty() ? String.Empty : ", " + applicant.Address2));
                        _sbSharedInformation.Append(applicant.County.IsNullOrEmpty() ? ", " + String.Empty : ", " + applicant.County);
                        _sbSharedInformation.Append("<br /><span style='padding-left:52px'> " + applicant.City + ", " + applicant.State + "</span>");
                        _sbSharedInformation.Append("<br /><span style='padding-left:52px'> " + applicant.Country + " - " + applicant.Zipcode + "</span>");
                        _sbSharedInformation.Append("</li>");
                    }
                }
                _sbSharedInformation.Append("</ul>");
                _sbSharedInformation.Append("</div>");
            }
            return Convert.ToString(_sbSharedInformation);
        }


        #region Manage Invitations

        /// <summary>
        /// To get documents of invitations
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="lstProfileSharingInvitationID"></param>
        /// <returns></returns>
        public static List<InvitationDocumentContract> GetApplicantInviteDocuments(Int32 tenantId, List<InvitationIDsContract> lstProfileSharingInvitationID)
        {
            try
            {
                DataTable datatable = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetApplicantInviteDocuments(lstProfileSharingInvitationID);
                return AssignValuesToApplicantDataModel(datatable);
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

        private static List<InvitationDocumentContract> AssignValuesToApplicantDataModel(DataTable datatable)
        {
            try
            {
                IEnumerable<DataRow> rows = datatable.AsEnumerable();
                return rows.Select(x => new InvitationDocumentContract
                {
                    ID = x["ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ID"]),
                    ProfileSharingInvitationID = Convert.ToInt32(x["ProfileSharingInvitationID"]),
                    ApplicantDocumentID = x["ApplicantDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantDocumentID"]),
                    Name = Convert.ToString(x["FirstName"]) + " " + Convert.ToString(x["LastName"]),
                    FileName = Convert.ToString(x["FileName"]),
                    DocumentPath = Convert.ToString(x["DocumentPath"]),
                    ComplianceCategoryID = x["ComplianceCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ComplianceCategoryID"]),
                    CategoryName = Convert.ToString(x["CategoryName"]),
                    OrganizationUserID = x["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["OrganizationUserID"]),
                    MasterOrderID = x["MasterOrderID"] == DBNull.Value ? 0 : Convert.ToInt32(x["MasterOrderID"]),
                    BkgSvcGroupID = x["BkgServiceGroupID"] == DBNull.Value ? 0 : Convert.ToInt32(x["BkgServiceGroupID"]),
                    IsFlagged = x["IsFlagged"] == DBNull.Value ? false : Convert.ToBoolean(x["IsFlagged"])
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
        /// To get documents of invitations
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="lstProfileSharingInvitationID"></param>
        /// <returns></returns>
        public static List<InvitationDocumentContract> GetClientInviteDocuments(Int32 tenantId, List<InvitationIDsContract> lstClientInvitationID)
        {
            try
            {
                String clientInvitationIDs = String.Join(",", lstClientInvitationID.Select(col => col.ProfileSharingInvitationID).ToList());
                DataTable datatable = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetClientInviteDocuments(clientInvitationIDs);
                return AssignValuesToClientDataModel(datatable);
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

        private static List<InvitationDocumentContract> AssignValuesToClientDataModel(DataTable datatable)
        {
            try
            {
                IEnumerable<DataRow> rows = datatable.AsEnumerable();
                return rows.Select(x => new InvitationDocumentContract
                {
                    SnapshotID = x["SnapshotID"] == DBNull.Value ? 0 : Convert.ToInt32(x["SnapshotID"]),
                    ItemAttributeID = x["ItemAttributeID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ItemAttributeID"]),
                    CategoryItemID = x["CategoryItemID"] == DBNull.Value ? 0 : Convert.ToInt32(x["CategoryItemID"]),
                    DocMapID = x["DocMapID"] == DBNull.Value ? 0 : Convert.ToInt32(x["DocMapID"]),
                    ApplicantDocumentID = x["ApplicantDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantDocumentID"]),
                    FileName = Convert.ToString(x["FileName"]),
                    ComplianceCategoryID = x["CategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["CategoryID"]),
                    CategoryName = Convert.ToString(x["CategoryName"]),
                    IsExceptionDoc = Convert.ToBoolean(x["IsExceptionDoc"]),
                    ProfileSharingInvitationID = x["ProfileSharingInvitationID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ProfileSharingInvitationID"]),
                    Name = Convert.ToString(x["FirstName"]) + " " + Convert.ToString(x["LastName"]),
                    DocumentPath = Convert.ToString(x["DocumentPath"])
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
        /// To get applicant documents by applicant document IDs
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="lstApplicantDocumentID"></param>
        /// <returns></returns>
        public static List<ApplicantDocument> GetApplicantDocumentByIDs(Int32 tenantId, List<Int32> lstApplicantDocumentID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetApplicantDocumentByIDs(lstApplicantDocumentID);
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
        /// Get the List of SharedComplianceSubscription by invitation IDs
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="lstInvitationID"></param>
        /// <returns></returns>
        public static List<SharedComplianceSubscription> GetSharedComplianceSubscriptionByInvitationIDs(Int32 tenantId, List<Int32> lstInvitationID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetSharedComplianceSubscriptionByInvitationIDs(lstInvitationID);
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
        /// Get passport report data
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="invitationIdsforPassportReport"></param>
        /// <returns></returns>
        public static DataTable GetPassportReportData(Int32 tenantID, List<InvitationIDsContract> invitationIDsContract)
        {
            try
            {
                String xmlData = CreatePassportReportXml(invitationIDsContract);
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetPassportReportData(xmlData);
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
        /// Get passport report data
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="invitationIdsforPassportReport"></param>
        /// <returns></returns>
        public static List<InvitationDocumentContract> GetPassportReportDataForRotation(Int32 tenantID, List<InvitationIDsContract> invitationIDsContract)
        {
            try
            {
                List<Int32> InvitationIds = invitationIDsContract.Select(con => con.ProfileSharingInvitationID).ToList();
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetPassportReportDataForRotation(InvitationIds);
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
        /// UAT:2475: Get Passport Data for particular rotations shared with agency user
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="invitationIDsContract"></param>
        /// <returns></returns>
        public static List<InvitationIDsDetailContract> GetPassportReportDataForParticularRotation(Int32 rotationID, Int32 currentLoggedInUserID, Int32 tenantIDs)
        {
            try
            {
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetProfileSharingInvitationIdByRotationID(rotationID, currentLoggedInUserID, tenantIDs);
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

        private static String CreatePassportReportXml(List<InvitationIDsContract> invitationIDsContract)
        {
            XDocument doc = new XDocument(
                            new XDeclaration("1.0", "utf-8", "true"),
                            new XElement("InvitationIDsWithInvitationSources",
                                invitationIDsContract.Select(d => new XElement("InvitationIDWithInvitationSource",
                                    new XAttribute("InvitationID", d.ProfileSharingInvitationID),
                                    new XAttribute("IsInvitationSourceApplicant", d.IsInvitationSourceApplicant))))
                            );
            return doc.ToString();
        }

        #endregion

        #region Agency User

        public static List<AgencyUser> GetAgencyUserInfo(Int32 tenantID, Boolean IsAdmin)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUser(tenantID, IsAdmin);
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
        /// UAT-2378 :- Optimize Manage Agency User Screen
        /// </summary>
        /// <param name="IsAdmin"></param>
        /// <param name="IsAgencyUser"></param>
        /// <param name="UserID"></param>
        /// <param name="TenantID"></param> 
        /// <param name="GetNotVerfiedAgenciesAlso"></param>
        /// <returns></returns>
        public static List<AgencyUserContract> GetAgencyUsersList(Boolean IsAdmin, Boolean IsAgencyUser, Guid UserID, Int32 TenantID, CustomPagingArgsContract grdCustomPaging, Boolean GetNotVerfiedAgenciesAlso = false)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserInfo(IsAdmin, IsAgencyUser, UserID, TenantID, grdCustomPaging,GetNotVerfiedAgenciesAlso);
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


        public static List<AgencyUser> GetAgencyUserForSharedUser(Guid userID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserForSharedUser(userID);
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

        public static Int32 SaveAgencyUser(int tenantID, AgencyUserContract _agencyUser, Int32 loggedInUserID, List<AgencyUserPermission> lstAgencyUserPermission)
        {
            try
            {
                Int32 agencyUserID = BALUtils.GetProfileSharingRepoInstance().SaveAgencyUser(_agencyUser, loggedInUserID, lstAgencyUserPermission);

                #region UAT-2641 (Save Agency Hierarchy User Details)

                BALUtils.GetProfileSharingRepoInstance().SaveUpdateAgencyHierarchyUserDetails(agencyUserID, _agencyUser.lstAgencyHierarchyIds, loggedInUserID);

                #endregion

                return agencyUserID;
                // if (agencyUserID > AppConsts.NONE)
                //{
                //    #region Send Agency User Account Creation Email

                //    Boolean isMailSuccessfullySent = SendAgencyUserAccountCreationMail(_agencyUser, loggedInUserID, agencyUserID);
                //    if (isMailSuccessfullySent)
                //    {
                //        return AppConsts.AGU_SAVED_SUCCESS_MSG;
                //    }

                //    #endregion
                //}
                //return AppConsts.AGU_SAVED_ERROR_MSG;
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

        public static Boolean SendAgencyUserAccountCreationMail(AgencyUserContract _agencyUser, Int32 loggedInUserID, Int32 agencyUserID)
        {
            //Send mail to do
            List<String> subEventCodes = new List<String>();
            subEventCodes.Add(CommunicationSubEvents.AGENCY_USER_ACCOUNT_CREATION.GetStringValue().ToLower());
            Int32 subEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
            List<Entity.CommunicationTemplatePlaceHolder> placeHoldersToFetch = CommunicationManager.GetTemplatePlaceHolders(subEventID);
            Int32 templateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(subEventID);
            //Contains info for mail subject and content
            SystemEventTemplatesContract systemEventTemplate = TemplatesManager.GetTemplateDetails(templateId);

            String profileSharingURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                            ? String.Empty
                                            : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

            var queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.QUERY_STRING_AGENCY_USER_ID, agencyUserID.ToString()}
                                                                    ,{AppConsts.QUERY_STRING_USER_TYPE_CODE, OrganizationUserType.AgencyUser.GetStringValue()}
                                                                 };

            var url = "http://" + String.Format(profileSharingURL + "?args={0}", queryString.ToEncryptedQueryString());


            //String applicationUrl = WebSiteManager.GetInstitutionUrl(clientContact.CC_TenantID);
            Dictionary<String, String> dictMailData = new Dictionary<string, String>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, _agencyUser.AGU_Name);
            dictMailData.Add(EmailFieldConstants.APPLICATION_URL, url);
            dictMailData.Add(EmailFieldConstants.AGENCY_NAME, _agencyUser.AgencyName);

            //a. Create entry in [Messaging] SystemCommunication table 
            //b. Create entry in [Messaging] SystemCommunicationDelivery table 
            Entity.SystemCommunication systemCommunication = new Entity.SystemCommunication();
            systemCommunication.SenderName = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME];
            systemCommunication.SenderEmailID = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];
            systemCommunication.Subject = systemEventTemplate.Subject;
            systemCommunication.CommunicationSubEventID = subEventID;
            systemCommunication.CreatedByID = loggedInUserID;
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
            systemCommunicationDelivery.ReceiverOrganizationUserID = agencyUserID;
            systemCommunicationDelivery.RecieverEmailID = _agencyUser.AGU_Email;
            systemCommunicationDelivery.RecieverName = _agencyUser.AGU_Name;
            systemCommunicationDelivery.IsDispatched = false;
            systemCommunicationDelivery.IsCC = null;
            systemCommunicationDelivery.IsBCC = null;
            systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
            systemCommunicationDelivery.CreatedOn = DateTime.Now;
            systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);

            List<Entity.SystemCommunication> lstSystemCommunicationToBeSaved = new List<Entity.SystemCommunication>();
            lstSystemCommunicationToBeSaved.Add(systemCommunication);
            return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
        }

        public static string DeleteAgencyUser(Int32 tenantID, Int32 AUG_ID, Int32 LoggedInUserId, List<Int32> lsrAgencyInstitutionIds, Boolean IsAdmin)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().DeleteAgencyUser(tenantID, AUG_ID, LoggedInUserId, lsrAgencyInstitutionIds, IsAdmin);
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

        //public static string UpdateAgencyUser(Int32 tenantID, AgencyUserContract _agencyUser, Int32 LoggedInUserId, Boolean IsAdmin, List<Int32> agencyInstitutionIDs_Added, List<Int32> agencyInstitutionIDs_Removed, List<AgencyUserPermission> lstAgencyUserPermission)
        public static AgencyUser UpdateAgencyUser(Int32 tenantID, AgencyUserContract _agencyUser, Int32 LoggedInUserId, Boolean IsAdmin, List<AgencyUserPermission> lstAgencyUserPermission)
        {
            try
            {
                AgencyUser agencyUser = BALUtils.GetProfileSharingRepoInstance().UpdateAgencyUser(_agencyUser, LoggedInUserId, IsAdmin, lstAgencyUserPermission);

                #region UAT-2641 (Save Agency Hierarchy User Details)
                BALUtils.GetProfileSharingRepoInstance().SaveUpdateAgencyHierarchyUserDetails(agencyUser.AGU_ID, _agencyUser.lstAgencyHierarchyIds, LoggedInUserId);
                #endregion

                return agencyUser;
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


        public static List<int> GetAgencyUserSharedDataForAgencyUserID(int tenantID, int agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserSharedDataForAgencyUserID(agencyID);
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

        public static List<AgencyInstitution> GetAgencyUserInstitutesForAgencyUserID(int tenantID, int agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserInstitutesForAgencyUserID(agencyUserID);
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

        public static List<Entity.SharedDataEntity.InvitationSharedInfoMapping> GetInvitationSharedInfoTypeByAgencyUserID(Int32 tenantID, Int32 agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationSharedInfoTypeByAgencyUserID(agencyUserID);
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

        public static Boolean IsEmailAlreadyExistAgencyUser(Int32 tenantID, String email)
        {
            try
            {
                AgencyUser agencyUser = BALUtils.GetProfileSharingRepoInstance().IsEmailAlreadyExistAgencyUser(email);
                if (agencyUser.IsNotNull())
                {
                    //if (AGU_ID.IsNull()) //New User : Insert Mode
                    //{
                    if (agencyUser.IsNotNull())
                        return true;
                    else
                        return false;

                    //Below Code is Commented for UAT-1218: Any User should be able to be 1 or more of the following: Applicant, Client admin, Agency User, Instructor/Preceprtor	
                    //}
                    //else //Update Mode
                    //{
                    //    if (agencyUser.AGU_ID == AGU_ID)
                    //        return false;
                    //    else
                    //        return true;
                    //}
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
        /// Get Agency User details
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static AgencyUserContract GetAgencyUserDetails(Guid userID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserDetails(userID);
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
        /// Update Agency User details
        /// </summary>
        /// <param name="agencyUserDetails"></param>
        /// <param name="userID"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static Boolean UpdateAgencyUserDetails(AgencyUserContract agencyUserDetails, Guid userID, Int32 currentUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().UpdateAgencyUserDetails(agencyUserDetails, userID, currentUserID);
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

        public static List<String> GetAgencyByAgencyUserID(Int32 agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyByAgencyUserID(agencyUserID);
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

        #region AGENCY SHARING
        public static List<AgencySharingDataContract> GetDataForAgencySharing(Int32 clientID, SearchItemDataContract searchDataContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                DataTable dtAgencySharingData = BALUtils.GetProfileSharingClientRepoInstance(clientID).GetDataForAgencySharing(searchDataContract, customPagingArgsContract);
                if (dtAgencySharingData.IsNotNull())
                {
                    return GetAgencySharingDataIntoList(dtAgencySharingData);
                }
                return new List<AgencySharingDataContract>();
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

        private static List<AgencySharingDataContract> GetAgencySharingDataIntoList(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new AgencySharingDataContract
                {
                    OrganizationUserId = x["OrganizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrganizationUserId"]),
                    ApplicantFirstName = Convert.ToString(x["ApplicantFirstName"]),
                    ApplicantLastName = Convert.ToString(x["ApplicantLastName"]),
                    EmailAddress = Convert.ToString(x["EmailAddress"]),
                    DateOfBirth = String.IsNullOrEmpty(x["DateOfBirth"].ToString()) ? (DateTime?)null : DateTime.Parse(x["DateOfBirth"].ToString()),
                    UserGroupName = Convert.ToString(x["UserGroupName"]),
                    SSN = Convert.ToString(x["SSN"]),
                    InstitutionHierarchy = Convert.ToString(x["InstitutionHierarchy"]),
                    LastSharedDate = String.IsNullOrEmpty(x["LastSharedDate"].ToString()) ? (DateTime?)null : DateTime.Parse(x["LastSharedDate"].ToString()),
                    TotalCount = x["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["TotalCount"]),
                    //IsInvitationApproved = x["IsInvitationApproved"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["IsInvitationApproved"]), //UAT-2443
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
        /// Send the Invitation Email to the Invitee
        /// </summary>
        /// <param name="profileSharingUrl"></param>
        /// <param name="inviteeEmailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="emailContent"></param>
        /// <param name="isContentReplaced">Decides, if the it is resent/admin/client admin invitation Email, then update the
        /// placeholders with the data here.</param>
        /// <param name="dicData"></param>
        /// <param name="tenantID"></param>
        public static Boolean SendInvitationEmail(String profileSharingUrl, String inviteeEmailAddress, String subject, String emailContent,
                                               Boolean isContentReplaced, Dictionary<String, String> dicData, Boolean isAdmin, Int32 tenantID,
                                                             Int32 receiverOrgUserID, Boolean isRotationSharing = false)
        {
            try
            {
                if (String.IsNullOrEmpty(profileSharingUrl))
                {
                    return false;
                }
                if (isContentReplaced)// Case When New invitation is to be sent by APPLICANT, then Template placeholders are already replaced.
                {
                    //var _dicContent = new Dictionary<String, String>();
                    return SendInvitationEmail(inviteeEmailAddress, emailContent, subject, dicData, isContentReplaced, receiverOrgUserID);
                }
                else // Case When New invitation is to be sent by ADMIN/CLIENT ADMIN OR RESEND BY APPLICANT, then Template placeholders are to bre replaced
                {
                    var _emailContent = String.Empty;
                    var _emailSubject = String.Empty;

                    if (isAdmin && !isRotationSharing)
                    {
                        var _emailSettings = ProfileSharingManager.GetInvitationEmailContent(AppConsts.PSIEMAILSUBJECT_APPCONFIGURATIONKEY_ADMIN, AppConsts.PSIEMAILCONTENT_APPCONFIGURATIONKEY_ADMIN, tenantID);
                        _emailContent = _emailSettings.GetValue(AppConsts.PSIEMAILCONTENT_APPCONFIGURATIONKEY_ADMIN);
                        _emailSubject = _emailSettings.GetValue(AppConsts.PSIEMAILSUBJECT_APPCONFIGURATIONKEY_ADMIN);
                    }
                    else if (isAdmin && isRotationSharing)
                    {
                        var _emailSettings = ProfileSharingManager.GetInvitationEmailContent(AppConsts.PSIEMAILSUBJECT_APPCONFIGURATIONKEY_ADMIN_ROT_SHARING, AppConsts.PSIEMAILCONTENT_APPCONFIGURATIONKEY_ADMIN_ROT_SHARING, tenantID);
                        _emailContent = _emailSettings.GetValue(AppConsts.PSIEMAILCONTENT_APPCONFIGURATIONKEY_ADMIN_ROT_SHARING);
                        _emailSubject = _emailSettings.GetValue(AppConsts.PSIEMAILSUBJECT_APPCONFIGURATIONKEY_ADMIN_ROT_SHARING);
                    }
                    else
                    {
                        //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
                        var _emailSettings = ProfileSharingManager.GetInvitationEmailContentUsingSubEvent(tenantID, CommunicationSubEvents.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue());
                        _emailSubject = _emailSettings.Keys.First();
                        _emailContent = _emailSettings.Values.First();

                    }
                    return SendInvitationEmail(inviteeEmailAddress, _emailContent, _emailSubject, dicData, isContentReplaced, receiverOrgUserID);
                }
            }
            catch (SysXException ex)
            {
                String logError = "Getting Exception while sending the invitation mail to the following invitee: "
                    + "Invitee Email Address:" + inviteeEmailAddress + " "
                    + "Recipient Name:" + (dicData.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTNAME) ? dicData[AppConsts.PSIEMAIL_RECIPIENTNAME] : String.Empty) + " "
                    + "Student Name:" + (dicData.ContainsKey(AppConsts.PSIEMAIL_STUDENTNAME) ? dicData[AppConsts.PSIEMAIL_STUDENTNAME] : String.Empty) + " "
                    + "School Name:" + (dicData.ContainsKey(AppConsts.PSIEMAIL_SCHOOLNAME) ? dicData[AppConsts.PSIEMAIL_SCHOOLNAME] : String.Empty) + " "
                    + Environment.NewLine
                    + "Exception Details:";

                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + logError + Environment.NewLine + ex.Message
                    + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
            catch (Exception ex)
            {
                String logError = "Getting Exception while sending the invitation mail to the following invitee: "
                    + "Invitee Email Address:" + inviteeEmailAddress + " "
                    + "Recipient Name:" + (dicData.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTNAME) ? dicData[AppConsts.PSIEMAIL_RECIPIENTNAME] : String.Empty) + " "
                    + "Student Name:" + (dicData.ContainsKey(AppConsts.PSIEMAIL_STUDENTNAME) ? dicData[AppConsts.PSIEMAIL_STUDENTNAME] : String.Empty) + " "
                    + "School Name:" + (dicData.ContainsKey(AppConsts.PSIEMAIL_SCHOOLNAME) ? dicData[AppConsts.PSIEMAIL_SCHOOLNAME] : String.Empty) + " "
                     + Environment.NewLine
                     + "Exception Details:";

                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + logError + Environment.NewLine + ex.Message
                    + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Method to Save Immunization Snapshot and returns snapshotID
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="currentUserID"></param>
        /// <param name="packageSubscrptionID"></param>
        public static Int32 SaveImmunizationSnapshot(Int32 tenantID, Int32 currentUserID, Int32 packageSubscrptionID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).SaveImmunizationSnapshot(currentUserID, packageSubscrptionID);
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
        /// Generate the Snapshot of the Requirement Package
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="packageSubscrptionID"></param>
        /// <returns></returns>
        public static Int32 SaveRequirementSnapshot(Int32 currentUserId, Int32 packageSubscrptionId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).SaveRequirementSnapshot(currentUserId, packageSubscrptionId);
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

        #region Immunization Data For Snapshot
        /// <summary>
        /// Get Immuniztion Data From snapshot.
        /// </summary>  
        /// <returns></returns>
        public static DataSet GetImmunizationDataFromSnapshot(Int32 tenantId, Int32 snapshotId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetImmunizationDataFromSnapshot(snapshotId);
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
        /// To get Applicant documents From snapshot
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sharedcategoryids"></param>
        /// <param name="snapshotId"></param>
        /// <returns></returns>
        public static List<InvitationDocumentContract> GetApplicantDocumentsFromSnapshot(Int32 tenantId, String sharedcategoryids, Int32 snapshotId)
        {
            try
            {
                DataTable dataForQueue = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetApplicantDocumentsFromSnapshot(sharedcategoryids, snapshotId);
                return AssignCategoryDocumentToDataModel(dataForQueue);
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


        public static List<int> CheckTenantsNeedToDisable(int TenantID, int AgencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().CheckTenantsNeedToDisable(AgencyID);
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

        #region UAT-1213: Updates to Agency User background check permissions.
        public static List<vwBkgOrderFlagged> GetBkgOrderFlagged(Int32 tenantId, List<Int32> bkgOrderIds)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetBkgOrderFlagged(bkgOrderIds);
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

        #region UAT-1210: As a client admin, I should be able to see when and what was shared through profile sharing
        public static List<ProfileSharingDataContract> GetProfileSharingData(Int32 tenantID, Int32 invitationGroupID)
        {
            try
            {
                List<ProfileSharingDataContract> lstprofileSharingData = new List<ProfileSharingDataContract>();

                //Getting complete result set
                List<GetProfileSharingData_Result> profileSharingData = BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetProfileSharingData(invitationGroupID).ToList();

                //Iterating through each Invitation ID
                profileSharingData.Select(col => col.InvitationID).Distinct().ForEach(invitationID =>
                {
                    List<GetProfileSharingData_Result> psdList = profileSharingData.Where(cond => cond.InvitationID == invitationID).ToList();
                    GetProfileSharingData_Result psd = psdList.FirstOrDefault();

                    ProfileSharingDataContract currentContract = new ProfileSharingDataContract();
                    currentContract.InvitationId = psd.InvitationID;
                    currentContract.ApplicantUserID = psd.ApplicantUserID;
                    currentContract.ApplicantName = psd.ApplicantName;
                    currentContract.InviteeName = psd.InviteeName;
                    currentContract.InvitationDate = psd.InvitationDate;
                    currentContract.ViewedStatus = psd.ViewedStatus;
                    currentContract.InvitationSentStatus = psd.InvitationSentStatus;
                    currentContract.InviteeUserType = psd.InviteeUserType;
                    currentContract.ScheduledInvitationStatus = psd.ScheduledInvitationStatus;
                    currentContract.EffectiveDate = psd.EffectiveDate;
                    currentContract.ExpirationDate = psd.ExpirationDate;
                    currentContract.ViewsRemaining = psd.ViewsRemaining;
                    //UAT: 1496
                    currentContract.ExpirationType = psd.ExpirationDate.IsNullOrEmpty() ? (psd.ViewsRemaining.IsNullOrEmpty() ? String.Empty : "View Remaining") : "Expiration Date";

                    currentContract.lstSharedPackages = new List<INTSOF.UI.Contract.ProfileSharing.SharedPackages>();

                    //Add Compliance Package data
                    List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> compliancePackageSharedList = AddSharedPackageData(psdList, SystemPackageTypes.COMPLIANCE_PKG.GetStringValue());
                    currentContract.lstSharedPackages.AddRange(compliancePackageSharedList);

                    //Add Compliance Package data
                    List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> bkgPackageSharedList = AddSharedPackageData(psdList, SystemPackageTypes.BACKGROUND_PKG.GetStringValue());
                    currentContract.lstSharedPackages.AddRange(bkgPackageSharedList);

                    //Add Compliance Package data
                    List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> reqPackageSharedList = AddSharedPackageData(psdList, SystemPackageTypes.REQUIREMENT_ROT_PKG.GetStringValue());
                    currentContract.lstSharedPackages.AddRange(reqPackageSharedList);

                    lstprofileSharingData.Add(currentContract);
                });

                return lstprofileSharingData;
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
        /// UAT-1210 Method to get Shared Package Data(compliance and Bkg)
        /// </summary>
        /// <param name="psdList"></param>
        /// <param name="isCompliancePackage"></param>
        /// <returns></returns>
        private static List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> AddSharedPackageData(List<GetProfileSharingData_Result> psdList, String packageTypeCode)
        {
            try
            {
                List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> currentSharedPackageList = new List<INTSOF.UI.Contract.ProfileSharing.SharedPackages>();
                psdList.Where(cond => cond.PackageTypeCode == packageTypeCode && !cond.OrderID.IsNullOrEmpty() && !cond.PackageID.IsNullOrEmpty())
                                                                                        .Select(col => col.OrderID)
                                                                                        .Distinct().ForEach(ordID =>
                {

                    psdList.Where(cond => cond.PackageTypeCode == packageTypeCode && cond.OrderID == ordID && !cond.OrderID.IsNullOrEmpty() && !cond.PackageID.IsNullOrEmpty())
                                                                                       .Select(col => col.PackageID)
                                                                                       .Distinct().ForEach(pkgID =>
                   {
                       INTSOF.UI.Contract.ProfileSharing.SharedPackages currentSharedPackage = new INTSOF.UI.Contract.ProfileSharing.SharedPackages();
                       currentSharedPackage.OrderID = ordID;
                       currentSharedPackage.PackageId = pkgID;//psdList.Where(cond => cond.OrderID == ordID && cond.IsCompliancePackage == isCompliancePackage).FirstOrDefault().PackageID;
                       currentSharedPackage.PackageTypeCode = packageTypeCode;
                       currentSharedPackage.PackageName = psdList.Where(cond => cond.OrderID == ordID && cond.PackageID == pkgID && cond.PackageTypeCode == packageTypeCode).FirstOrDefault().PackageName;
                       currentSharedPackage.PackageIdentifier = Guid.NewGuid();
                       currentSharedPackage.OrderNumber = psdList.Where(cond => cond.OrderID == ordID && cond.PackageID == pkgID && cond.PackageTypeCode == packageTypeCode).FirstOrDefault().OrderNumber;

                       var compliancePkgList = psdList.Where(cond => cond.OrderID == ordID && cond.PackageID == pkgID && cond.PackageTypeCode == packageTypeCode).ToList();

                       List<INTSOF.UI.Contract.ProfileSharing.SharedEntity> currentSharedEntityList = new List<INTSOF.UI.Contract.ProfileSharing.SharedEntity>();
                       currentSharedEntityList = compliancePkgList.Where(col => !col.SharedEntityName.IsNullOrEmpty()).Select(col => new INTSOF.UI.Contract.ProfileSharing.SharedEntity
                       {
                           SharedEntityName = col.SharedEntityName
                       }).ToList();

                       currentSharedPackage.lstSharedEntity = new List<INTSOF.UI.Contract.ProfileSharing.SharedEntity>();
                       if (currentSharedEntityList.IsNullOrEmpty())
                       {
                           currentSharedEntityList = new List<INTSOF.UI.Contract.ProfileSharing.SharedEntity>();
                       }
                       currentSharedPackage.lstSharedEntity.AddRange(currentSharedEntityList);
                       currentSharedPackageList.Add(currentSharedPackage);
                   });
                });
                return currentSharedPackageList;
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

        private static List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> AddSharedPackageData(List<usp_GetProfileSharingDataByInvitationId_Result> psdList, String packageTypeCode)
        {
            try
            {
                List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> currentSharedPackageList = new List<INTSOF.UI.Contract.ProfileSharing.SharedPackages>();
                psdList.Where(cond => cond.PackageTypeCode == packageTypeCode && !cond.OrderID.IsNullOrEmpty() && !cond.PackageID.IsNullOrEmpty())
                                                                                        .Select(col => col.OrderID)
                                                                                        .Distinct().ForEach(ordID =>
                                                                                        {

                                                                                            psdList.Where(cond => cond.PackageTypeCode == packageTypeCode && cond.OrderID == ordID && !cond.OrderID.IsNullOrEmpty() && !cond.PackageID.IsNullOrEmpty())
                                                                                                                                                               .Select(col => col.PackageID)
                                                                                                                                                               .Distinct().ForEach(pkgID =>
                                                                                                                                                               {
                                                                                                                                                                   INTSOF.UI.Contract.ProfileSharing.SharedPackages currentSharedPackage = new INTSOF.UI.Contract.ProfileSharing.SharedPackages();
                                                                                                                                                                   currentSharedPackage.OrderID = ordID;
                                                                                                                                                                   currentSharedPackage.PackageId = pkgID;//psdList.Where(cond => cond.OrderID == ordID && cond.IsCompliancePackage == isCompliancePackage).FirstOrDefault().PackageID;
                                                                                                                                                                   currentSharedPackage.PackageTypeCode = packageTypeCode;
                                                                                                                                                                   currentSharedPackage.PackageName = psdList.Where(cond => cond.OrderID == ordID && cond.PackageID == pkgID && cond.PackageTypeCode == packageTypeCode).FirstOrDefault().PackageName;
                                                                                                                                                                   currentSharedPackage.PackageIdentifier = Guid.NewGuid();
                                                                                                                                                                   currentSharedPackage.OrderNumber = psdList.Where(cond => cond.OrderID == ordID && cond.PackageID == pkgID && cond.PackageTypeCode == packageTypeCode).FirstOrDefault().OrderNumber;

                                                                                                                                                                   var compliancePkgList = psdList.Where(cond => cond.OrderID == ordID && cond.PackageID == pkgID && cond.PackageTypeCode == packageTypeCode).ToList();

                                                                                                                                                                   List<INTSOF.UI.Contract.ProfileSharing.SharedEntity> currentSharedEntityList = new List<INTSOF.UI.Contract.ProfileSharing.SharedEntity>();
                                                                                                                                                                   currentSharedEntityList = compliancePkgList.Where(col => !col.SharedEntityName.IsNullOrEmpty()).Select(col => new INTSOF.UI.Contract.ProfileSharing.SharedEntity
                                                                                                                                                                   {
                                                                                                                                                                       SharedEntityName = col.SharedEntityName,
                                                                                                                                                                       IsDocumentShared = col.IsDocumentUploaded
                                                                                                                                                                   }).ToList();

                                                                                                                                                                   currentSharedPackage.lstSharedEntity = new List<INTSOF.UI.Contract.ProfileSharing.SharedEntity>();
                                                                                                                                                                   if (currentSharedEntityList.IsNullOrEmpty())
                                                                                                                                                                   {
                                                                                                                                                                       currentSharedEntityList = new List<INTSOF.UI.Contract.ProfileSharing.SharedEntity>();
                                                                                                                                                                   }
                                                                                                                                                                   currentSharedPackage.lstSharedEntity.AddRange(currentSharedEntityList);
                                                                                                                                                                   currentSharedPackageList.Add(currentSharedPackage);
                                                                                                                                                               });
                                                                                        });
                return currentSharedPackageList;
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

        #region UAT-1237
        public static List<Int32> GetSharedCategoryList(Int32? tenantID, Int32 invitationID, Int32 packageSubscriptionID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID.Value).GetSharedCategoryList(invitationID, packageSubscriptionID);
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

        #region UAT 1318

        /// <summary>
        /// Gets the list of Applicants added to a Rotation, for sending the ProfileSharingInvitation
        /// </summary>
        /// <param name="RotationId"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        public static List<AgencySharingDataContract> GetRotationMembers(Int32 rotationId, Int32 agencyId, CustomPagingArgsContract customPagingArgsContract, String rotationMemberIds, Int32 tenantId, String instructorPreceptorOrgUserIds)
        {
            try
            {
                DataTable dtAgencySharingData = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetRotationMembers(rotationId, agencyId, customPagingArgsContract, rotationMemberIds, instructorPreceptorOrgUserIds);
                if (dtAgencySharingData.IsNotNull())
                {
                    return GetAgencySharingDataIntoList(dtAgencySharingData);
                }
                return new List<AgencySharingDataContract>();
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


        #region Methods copied from SecurityManager

        #region Profile Sharing

        /// <summary>
        /// Method to get Invitation Source ID by Invitation Code
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <returns></returns>
        private static Int32 GetInvitationStatusIdByCode(String invitationStatusCode)
        {
            try
            {
                lkpInvitationStatu invitationSource = LookupManager.GetSharedDBLookUpData<lkpInvitationStatu>().FirstOrDefault(x => x.Code == invitationStatusCode && !x.IsDeleted);
                if (invitationSource != null)
                {
                    return invitationSource.InvitationStatusID;
                }
                return AppConsts.NONE;
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
        /// Method to get Invitation Source ID by Invitation Code
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <returns></returns>
        public static Int32 GetDocumentTypeIdByCode(String docTypeCode)
        {
            try
            {
                var documentType = LookupManager.GetSharedDBLookUpData<lkpSharedSystemDocType>().FirstOrDefault(x => x.SSDT_Code == docTypeCode && !x.SSDT_IsDeleted);
                if (documentType != null)
                {
                    return documentType.SSDT_ID;
                }
                return AppConsts.NONE;
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

        public static Int32 GetAgencyUserAttestationpermissionIdByCode(String agencyUserAttestationPermision)
        {
            try
            {
                var documentType = LookupManager.GetSharedDBLookUpData<lkpAgencyUserAttestationPermission>().FirstOrDefault(x => x.LAUAP_Code == agencyUserAttestationPermision && !x.LAUAP_IsDeleted);
                if (documentType != null)
                {
                    return documentType.LAUAP_ID;
                }
                return AppConsts.NONE;
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
        /// Method to get Invitation Source ID by Invitation Code
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <returns></returns>
        private static Int32? GetInvitationSourceIdByCode(String invitationCode)
        {
            try
            {
                lkpInvitationSource invitationSource = LookupManager.GetSharedDBLookUpData<lkpInvitationSource>().FirstOrDefault(x => x.Code == invitationCode && !x.IsDeleted);
                if (invitationSource != null)
                {
                    return invitationSource.InvitationSourceID;
                }
                return AppConsts.NONE;
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
        /// Method to Update Invitee Organization UserID in ProfileSharingInvitation table
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        public static Boolean UpdateInviteeOrganizationUserID(Int32 orgUserID, Guid inviteToken, Int32? agencyUserID, out String profileSharingInvitationIds)
        {
            try
            {
                //OrganizationUsers.Where(cond => cond.OrganizationUserID == orgUserID).Select(x => x.UserID).FirstOrDefault();
                Int32 adminInitializedID = LookupManager.GetSharedDBLookUpData<lkpInvitationStatu>().Where(invsts => invsts.Code == LkpInviationStatusTypes.ADMIN_INITLIAZED.GetStringValue()).First().InvitationStatusID;
                Guid inviteeUserID = BALUtils.GetSecurityRepoInstance().GetOganisationUsersByUserIDForLogin(orgUserID).Select(x => x.UserID).FirstOrDefault();
                return BALUtils.GetProfileSharingRepoInstance().UpdateInviteeOrganizationUserID(orgUserID, inviteToken, adminInitializedID, inviteeUserID, agencyUserID, out profileSharingInvitationIds);
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
        /// Gets the list of invitations that has been sent by the applicant
        /// </summary>
        /// <param name="applicantOrgUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<InvitationDataContract> GetApplicantInvitations(Int32 applicantOrgUserId, Int32 tenantId, Int32? isAgnecyShareForAdmin = null)
        {
            try
            {
                var _lstInvitations = new List<InvitationDataContract>();
                var _dtInvitations = BALUtils.GetProfileSharingRepoInstance().GetApplicantInvitations(applicantOrgUserId, tenantId, isAgnecyShareForAdmin);

                if (_dtInvitations.Rows.Count > AppConsts.NONE)
                {
                    var _invitationIdColumn = "PSIId";
                    var _inviteeNameColumn = "InviteeName";
                    var _inviteeEmailColumn = "InviteeEmail";
                    var _inviteePhoneColumn = "InviteePhone";
                    var _inviteeAgencyColumn = "InviteeAgency";
                    var _invitationDateColumn = "InvitationDate";
                    var _inviteeLastViewedColumn = "InviteeLastViewed";
                    var _expireationDateColumn = "ExpirationDate";
                    var _viewsRemainingColumn = "ViewsRemaining";
                    var _expirationTypeCodeColumn = "ExpirationTypeCode";
                    var _IsIndividualShare = "IsIndividualShare";

                    for (int i = 0; i < _dtInvitations.Rows.Count; i++)
                    {
                        var _invitationContract = new InvitationDataContract();
                        _invitationContract.ID = Convert.ToInt32(_dtInvitations.Rows[i][_invitationIdColumn]);
                        _invitationContract.Name = Convert.ToString(_dtInvitations.Rows[i][_inviteeNameColumn]);
                        _invitationContract.EmailAddress = Convert.ToString(_dtInvitations.Rows[i][_inviteeEmailColumn]);
                        _invitationContract.Phone = Convert.ToString(_dtInvitations.Rows[i][_inviteePhoneColumn]);
                        _invitationContract.Agency = Convert.ToString(_dtInvitations.Rows[i][_inviteeAgencyColumn]);
                        _invitationContract.InvitationDate = Convert.ToDateTime(_dtInvitations.Rows[i][_invitationDateColumn]);
                        _invitationContract.LastViewedDate = _dtInvitations.Rows[i][_inviteeLastViewedColumn] == DBNull.Value ? (DateTime?)null :
                                                                Convert.ToDateTime(_dtInvitations.Rows[i][_inviteeLastViewedColumn]);
                        _invitationContract.ExpirationDate = _dtInvitations.Rows[i][_expireationDateColumn] == DBNull.Value ? (DateTime?)null :
                                                                Convert.ToDateTime(_dtInvitations.Rows[i][_expireationDateColumn]);
                        _invitationContract.ViewsRemaining = Convert.ToInt32(_dtInvitations.Rows[i][_viewsRemainingColumn]);
                        _invitationContract.ExpirationTypeCode = Convert.ToString(_dtInvitations.Rows[i][_expirationTypeCodeColumn]);

                        _invitationContract.IsExpirationCountVisible = _invitationContract.ExpirationTypeCode == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue()
                                                                        ? true : false;

                        _invitationContract.IsExpirationDateVisible = _invitationContract.ExpirationTypeCode == InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue()
                                                                      ? true : false;
                        _invitationContract.IsIndividualShare = _dtInvitations.Rows[i][_IsIndividualShare].GetType().Name == "DBNull" ? false : Convert.ToBoolean(_dtInvitations.Rows[i][_IsIndividualShare]);
                        _lstInvitations.Add(_invitationContract);
                    }
                }
                return _lstInvitations;
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
        /// Save the Invitation and return the ID of the invitation generated.
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <returns>Tuple with InvitationID, its related Token and whether its a new invitation</returns>
        private static Tuple<Int32, Guid> SaveProfileSharingInvitation(InvitationDetailsContract invitationDetails, Int32 genaratedInvitationGroupID)
        {
            try
            {
                var _repoInstance = BALUtils.GetProfileSharingRepoInstance();
                invitationDetails.ExpirationTypeId = LookupManager.GetSharedDBLookUpData<lkpInvitationExpirationType>().Where(iet => iet.Code == invitationDetails.ExpirationTypeCode).First().ExpirationTypeID;
                invitationDetails.InvitationStatusId = LookupManager.GetSharedDBLookUpData<lkpInvitationStatu>().Where(invsts => invsts.Code == invitationDetails.InvitationStatusCode).First().InvitationStatusID;
                invitationDetails.InvitationSourceId = LookupManager.GetSharedDBLookUpData<lkpInvitationSource>().Where(invsrc => invsrc.Code == invitationDetails.InvitationSourceCode).First().InvitationSourceID;
                //invitationDetails.InitiatedById = invitationDetails.CurrentUserId;
                #region UAT-3470
                String ArchiveInvitationActiveCode = InvitationArchiveState.Active.GetStringValue();
                invitationDetails.InvitationArchiveStateID = LookupManager.GetSharedDBLookUpData<lkpInvitationArchiveState>().Where(invsrc => invsrc.LIAS_Code == ArchiveInvitationActiveCode).First().LIAS_ID;
                #endregion
                var _inviteeOrgUserId = BALUtils.GetSecurityRepoInstance().GetSharedUserOrgId(invitationDetails.EmailAddress);
                invitationDetails.InviteeOrgUserId = _inviteeOrgUserId > AppConsts.NONE ? _inviteeOrgUserId : (Int32?)null;

                String groupTypeCode = ProfileSharingInvitationGroupTypes.PROFILE_SHARING_TYPE.GetStringValue();
                Int32 invGroupTypeID = LookupManager.GetSharedDBLookUpData<lkpProfileSharingInvitationGroupType>().Where(cond => cond.PSIGT_Code == groupTypeCode).Select(x => x.PSIGT_ID).FirstOrDefault();
                return _repoInstance.SaveProfileSharingInvitation(invitationDetails, genaratedInvitationGroupID, invGroupTypeID);
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
        /// Save the Invitation and return the ID of the invitation generated.
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <returns>Tuple with InvitationID, its related Token and whether its a new invitation</returns>
        public static List<ProfileSharingInvitation> SaveAdminInvitations(List<InvitationDetailsContract> lstInvitations, ProfileSharingInvitationGroup invitationGroup, String invitationSourceCode, Boolean isNonSchedulingType)
        {
            try
            {
                var _repoInstance = BALUtils.GetProfileSharingRepoInstance();

                var _invitationSourceId = LookupManager.GetSharedDBLookUpData<lkpInvitationSource>().Where(invsrc => invsrc.Code == invitationSourceCode).First().InvitationSourceID;
                // var _expirationTypeId = LookupManager.GetSharedDBLookUpData<lkpInvitationExpirationType>().Where(iet => iet.Code == InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue()).First().ExpirationTypeID;
                var _invitationStatusId = AppConsts.NONE;
                if (isNonSchedulingType)
                {
                    _invitationStatusId = LookupManager.GetSharedDBLookUpData<lkpInvitationStatu>().Where(invsts => invsts.Code == LkpInviationStatusTypes.ADMIN_INITLIAZED.GetStringValue()).First().InvitationStatusID;
                }
                else
                {
                    _invitationStatusId = LookupManager.GetSharedDBLookUpData<lkpInvitationStatu>().Where(invsts => invsts.Code == LkpInviationStatusTypes.INVITATION_SCHEDULED.GetStringValue()).First().InvitationStatusID;
                }

                //var _metaData = LookupManager.GetSharedDBLookUpData<ApplicantInvitationMetaData>().ToList();
                var _allUsers = BALUtils.GetSecurityRepoInstance().GetSharedUserOrgIds(lstInvitations.Select(x => x.EmailAddress).Distinct().ToList());

                lstInvitations.ForEach(invitation =>
                {
                    invitation.InvitationStatusId = _invitationStatusId;
                    //invitation.ExpirationTypeId = _expirationTypeId;
                    invitation.InvitationSourceId = _invitationSourceId;
                    //var _currentUser = _allUsers.Where(au => au.Key == invitation.EmailAddress).FirstOrDefault();

                    if (_allUsers.ContainsKey(invitation.EmailAddress.ToLower()))
                    {
                        invitation.InviteeOrgUserId = _allUsers.GetValue(invitation.EmailAddress.ToLower());
                    }
                });

                return _repoInstance.SaveAdminInvitations(lstInvitations, invitationGroup);
                ////invitationDetails.InviteeOrgUserId = _inviteeOrgUserId > AppConsts.NONE ? _inviteeOrgUserId : (Int32?)null;
                //return _repoInstance.SaveProfileSharingInvitation(invitationDetails, genaratedInvitationGroupID);
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

        /// <summary>SendInvitationEmail
        /// Returns whether the Shared user is being invited
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static Boolean IsSharedUserInvited(String emailAddress)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().IsSharedUserInvited(emailAddress);
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
        /// Send the email for the Invitation
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="emailContent"></param>
        /// <param name="subject"></param>
        /// <param name="dicContent"></param>
        /// <param name="isContentReplaced"></param>
        private static Boolean SendInvitationEmail(String toAddress, String emailContent, String subject, Dictionary<String, String> dicContent, Boolean isContentReplaced
                                                             , Int32 receiverOrgUserID)
        {
            if (!isContentReplaced)
            {
                subject = subject.Replace(AppConsts.PSIEMAIL_STUDENTNAME, dicContent.GetValue(AppConsts.PSIEMAIL_STUDENTNAME));

                //UAT-2556: Separate email template for student shares that used agency dropdown from those that do not. 
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_APPLICANT_NAME))
                {
                    subject = subject.Replace(AppConsts.PSIEMAIL_APPLICANT_NAME, dicContent.GetValue(AppConsts.PSIEMAIL_APPLICANT_NAME));
                }
                //

                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SCHOOLNAME))
                {
                    subject = subject.Replace(AppConsts.PSIEMAIL_SCHOOLNAME, dicContent.GetValue(AppConsts.PSIEMAIL_SCHOOLNAME));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTNAME))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_RECIPIENTNAME, dicContent.GetValue(AppConsts.PSIEMAIL_RECIPIENTNAME));
                }

                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_PROFILEURL))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_PROFILEURL, dicContent.GetValue(AppConsts.PSIEMAIL_PROFILEURL));
                }
                //if (dicContent.ContainsKey(AppConsts.PSIEMAIL_CENTRALLOGINURL))
                //{
                //    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_CENTRALLOGINURL, dicContent.GetValue(AppConsts.PSIEMAIL_CENTRALLOGINURL));
                //}
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_CUSTOMMESSAGE))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_CUSTOMMESSAGE, dicContent.GetValue(AppConsts.PSIEMAIL_CUSTOMMESSAGE));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_STUDENTNAME))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_STUDENTNAME, dicContent.GetValue(AppConsts.PSIEMAIL_STUDENTNAME));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SELECTEDPACKAGEDATA))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_SELECTEDPACKAGEDATA, dicContent.GetValue(AppConsts.PSIEMAIL_SELECTEDPACKAGEDATA));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, dicContent.GetValue(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA));
                }
                //UAT-1403 : add rotation details to rotation invitation email
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_RotationDetails, dicContent.GetValue(AppConsts.PSIEMAIL_RotationDetails));
                }
                //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_USER_FULL_NAME))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_USER_FULL_NAME, dicContent.GetValue(AppConsts.PSIEMAIL_USER_FULL_NAME));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_APPLICATION_URL))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_APPLICATION_URL, dicContent.GetValue(AppConsts.PSIEMAIL_APPLICATION_URL));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_APPLICANT_NAME))
                {
                    emailContent = emailContent.Replace(AppConsts.PSIEMAIL_APPLICANT_NAME, dicContent.GetValue(AppConsts.PSIEMAIL_APPLICANT_NAME));
                }
            }

            Dictionary<String, String> dicEmailContent = new Dictionary<String, String>
                {
                    {"EmailBody", emailContent}
                };

            Boolean isSentSuccess = SysXEmailService.SendSystemMail(dicEmailContent, subject, toAddress);

            #region Place Entries in DB for Messaging table for "To" user

            Int32 coomunicationSubeventID = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue()).CommunicationSubEventID;

            List<Entity.SystemCommunication> lstSystemCommunicationToBeSaved = new List<Entity.SystemCommunication>();
            String receiverName = String.Empty;
            if (dicContent.ContainsKey(EmailFieldConstants.USER_FULL_NAME))
            {
                receiverName = dicContent[EmailFieldConstants.USER_FULL_NAME].ToString();
            }
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

            //a. Create entry in [Messaging] SystemCommunication table 
            //b. Create entry in [Messaging] SystemCommunicationDelivery table 
            Entity.SystemCommunication systemCommunicationToBeInsterted = new Entity.SystemCommunication();
            systemCommunicationToBeInsterted.SenderName = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME];
            systemCommunicationToBeInsterted.SenderEmailID = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];
            systemCommunicationToBeInsterted.Subject = subject;
            systemCommunicationToBeInsterted.CommunicationSubEventID = coomunicationSubeventID;
            systemCommunicationToBeInsterted.CreatedByID = backgroundProcessUserId;
            systemCommunicationToBeInsterted.CreatedOn = DateTime.Now;
            systemCommunicationToBeInsterted.Content = emailContent;

            Entity.SystemCommunicationDelivery systemCommunicationDelivery = new Entity.SystemCommunicationDelivery();
            systemCommunicationDelivery.ReceiverOrganizationUserID = receiverOrgUserID;
            systemCommunicationDelivery.RecieverEmailID = toAddress;
            systemCommunicationDelivery.RecieverName = receiverName;
            systemCommunicationDelivery.IsDispatched = isSentSuccess;
            if (!isSentSuccess)
            {
                systemCommunicationDelivery.RetryCount = AppConsts.NONE;
                systemCommunicationDelivery.RetryErrorMessage = "Some error occured while sending e-mail.";
                systemCommunicationDelivery.DispatchedDate = null;
            }
            else
            {
                systemCommunicationDelivery.DispatchedDate = DateTime.Now;

            }
            systemCommunicationDelivery.IsCC = null;
            systemCommunicationDelivery.IsBCC = null;
            systemCommunicationDelivery.CreatedByID = systemCommunicationToBeInsterted.CreatedByID;
            systemCommunicationDelivery.CreatedOn = DateTime.Now;
            systemCommunicationToBeInsterted.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);
            lstSystemCommunicationToBeSaved.Add(systemCommunicationToBeInsterted);
            BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
            #endregion

            return isSentSuccess;
        }

        /// <summary>
        /// Check Whether shared user exists or not
        /// </summary>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        public static Entity.OrganizationUser IsSharedUserExists(Guid token, Boolean isProfileSharingToken, Int32? agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsSharedUserExists(token, isProfileSharingToken, agencyUserID);
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
        /// Method to get Shared User Data from Invitation Sent by applicant(currently only Email)
        /// </summary>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        public static String GetSharedUserDataFromInvitation(Guid token, Boolean isProfileSharingToken, Int32? agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetSharedUserDataFromInvitation(token, isProfileSharingToken, agencyUserID);
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
        /// Save/Update the Invitation and return the ID of the invitation generated.
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <returns>Tuple with InvitationID, its related Token and whether its a new invitation</returns>
        public static ProfileSharingInvitation GetInvitationDetails(Int32 invitationId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationDetails(invitationId);
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
        /// Update the Status of the Invitation
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        public static void UpdateInvitationStatus(String statusCode, Int32 invitationId, Int32 currentUserId)
        {
            try
            {
                //var _statusId = LookupManager.GetLookUpIDbyCode<lkpInvitationStatu>(y => y.Code.Trim().Contains(statusCode)); 

                var _statusId = LookupManager.GetSharedLookUpIDbyCode<lkpInvitationStatu>(y => y.Code.Trim().Contains(statusCode));
                BALUtils.GetProfileSharingRepoInstance().UpdateInvitationStatus(_statusId, invitationId, currentUserId);
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
        /// Update the Status of the bulk Invitations
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        public static void UpdateBulkInvitationStatus(String statusCode, List<Int32> invitationId, Int32 currentUserId)
        {
            try
            {
                var _statusId = LookupManager.GetSharedLookUpIDbyCode<lkpInvitationStatu>(invSts => invSts.Code == statusCode);
                BALUtils.GetProfileSharingRepoInstance().UpdateBulkInvitationStatus(_statusId, invitationId, currentUserId);
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
        /// To get invitation records
        /// </summary>
        /// <param name="searchContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        public static List<InvitationDataContract> GetInvitationData(InvitationSearchContract searchContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                DataTable dataForQueue = BALUtils.GetProfileSharingRepoInstance().GetInvitationData(searchContract, gridCustomPaging);
                return NewAssignValuesToDataModel(dataForQueue);
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

        private static List<InvitationDataContract> NewAssignValuesToDataModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new InvitationDataContract
                {
                    ID = x["ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ID"]),
                    Name = Convert.ToString(x["Name"]),
                    EmailAddress = Convert.ToString(x["EmailAddress"]),
                    Phone = Convert.ToString(x["Phone"]),
                    TenantID = Convert.ToInt32(x["TenantID"]),
                    ExpirationDate = x["ExpirationDate"].GetType().Name == "DBNull" ? null : (DateTime?)x["ExpirationDate"],
                    InvitationDate = x["InvitationDate"].GetType().Name == "DBNull" ? null : (DateTime?)x["InvitationDate"],
                    LastViewedDate = x["LastViewedDate"].GetType().Name == "DBNull" ? null : (DateTime?)x["LastViewedDate"],
                    ViewsRemaining = x["ViewsRemaining"].GetType().Name == "DBNull" ? null : (Int32?)x["ViewsRemaining"],
                    InviteTypeID = x["InviteTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(x["InviteTypeID"]),
                    InviteTypeCode = Convert.ToString(x["InviteTypeCode"]),
                    InviteTypeName = Convert.ToString(x["InviteTypeName"]),
                    Notes = Convert.ToString(x["Notes"]),
                    TenantName = Convert.ToString(x["TenantName"]),
                    IsInvitationVisible = Convert.ToBoolean(x["IsInvitationVisible"]),
                    TotalCount = x["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(x["TotalCount"]),
                    SharedUserInvitationReviewStatusName = Convert.ToString(x["SharedUserInvitationReviewStatusName"]),
                    SharedUserInvitationReviewStatusCode = Convert.ToString(x["SharedUserInvitationReviewStatusCode"]),
                    AgencyName = Convert.ToString(x["AgencyName"])
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
        /// Get Invitations based upon PSI_InviteeOrgUserID
        /// </summary>
        /// <param name="inviteeOrgUserID"></param>
        /// <returns></returns>
        public static IEnumerable<ProfileSharingInvitation> GetInvitationsByInviteeOrgUserID(Int32 inviteeOrgUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationsByInviteeOrgUserID(inviteeOrgUserID);
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
        /// Update the Views remaining of the Invitation
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        public static Boolean UpdateInvitationViewsRemaining(Int32 invitationId, Int32 currentUserId, String expiredInvitationTypeCode)
        {
            try
            {
                Int32 expiredInvitationTypeId = LookupManager.GetSharedDBLookUpData<lkpInvitationStatu>().FirstOrDefault(cond => cond.Code == expiredInvitationTypeCode).InvitationStatusID;
                return BALUtils.GetProfileSharingRepoInstance().UpdateInvitationViewsRemaining(invitationId, currentUserId, expiredInvitationTypeId);
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
        /// Update Notes of the Invitation
        /// </summary>
        /// <param name="inviteeNotes"></param>
        /// <param name="invitationId"></param>GetAllAgency
        /// <param name="currentUserId"></param>        
        public static Boolean UpdateInvitationNotes(Int32 invitationId, Int32 currentUserId, String notes)
        {
            try
            {
                #region UAT-2511
                List<lkpAuditChangeType> lstAuditChangeType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAuditChangeType>()
                                                                    .Where(cond => !cond.LACT_IsDeleted).ToList();
                #endregion
                return BALUtils.GetProfileSharingRepoInstance().UpdateInvitationNotes(invitationId, currentUserId, notes, lstAuditChangeType);
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
        /// Method to Get All Agencies
        /// </summary>
        /// <returns></returns>
        public static List<Agency> GetAllAgency(Int32 institutionID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAllAgency(institutionID);
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
        /// Method to Get Agency User Data by Agency ID and InstitutionID
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static List<usp_GetAgencyUserData_Result> GetAgencyUserData(Int32 institutionID, Int32 agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserData(institutionID, agencyID);
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
        /// Method to genarate New Invitation Group
        /// </summary>
        /// <param name="agencyID"></param>
        /// <param name="initiatedByID"></param>
        /// <returns></returns>
        public static Int32 GenarateNewInvitationGroup(ProfileSharingInvitationGroup invitationGroupObj)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GenarateNewInvitationGroup(invitationGroupObj);
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

        public static List<InvitationDocumentContract> GetAttestationDocumentData(List<InvitationIDsContract> invitationIDsContract)
        {
            try
            {
                String clientInvitationIds = String.Join(",", invitationIDsContract.Select(col => col.ProfileSharingInvitationID).ToList());
                DataTable attestationReportData = BALUtils.GetProfileSharingRepoInstance().GetAttestationDocumentData(clientInvitationIds);
                IEnumerable<DataRow> attestationRows = attestationReportData.AsEnumerable();

                return attestationRows.Select(col => new InvitationDocumentContract
                {
                    ProfileSharingInvitationID = col["ProfileSharingInvitationID"] == DBNull.Value ? 0 : Convert.ToInt32(col["ProfileSharingInvitationID"]),
                    Name = Convert.ToString(col["FirstName"]) + " " + Convert.ToString(col["LastName"]),
                    DocumentPath = Convert.ToString(col["DocumentFilePath"]),
                    ApplicantDocumentID = col["InvitationDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(col["InvitationDocumentID"])
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

        public static InvitationDocument GetInvitationDocuments(Int32 invitationId, String attestationTypeCode)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationDocuments(invitationId, attestationTypeCode);
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

        public static InvitationDocument GetInvitationDocumentObject(String documentTypeCode, String pdfDocPath, Int32 currentLoggedInUserID, Boolean isForEveryone = false)
        {
            try
            {
                Int32 documentTypeId = GetDocumentTypeIdByCode(documentTypeCode);
                if (!String.IsNullOrEmpty(pdfDocPath))
                {
                    InvitationDocument invitationDocument = new InvitationDocument()
                    {
                        IND_DocumentFilePath = pdfDocPath,
                        IND_DocumentType = documentTypeId,
                        IND_IsDeleted = false,
                        IND_CreatedByID = currentLoggedInUserID,
                        IND_CreatedOn = DateTime.Now,
                        IND_IsForEveryone = isForEveryone
                    };
                    return invitationDocument;
                }
                return new InvitationDocument();
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

        public static Boolean SaveInvitationDocumentMapping(List<InvitationDocumentMapping> lstInvitationDocumentMapping)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveInvitationDocumentMapping(lstInvitationDocumentMapping);
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

        public static Boolean SaveInvAttestationDocWithPermissionType(List<InvAttestationDocWithPermissionType> lstInvAttestationDocWithPermissionType)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveInvAttestationDocWithPermissionType(lstInvAttestationDocWithPermissionType);
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
        /// Method to Get Invitation Data by Invite Token
        /// </summary>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        public static ProfileSharingInvitation GetInvitationDataByToken(Guid inviteToken)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationDataByToken(inviteToken);
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

        #region Other

        #region UAT-1210 - Method to update Invitation View Status
        public static void UpdateInvitationViewedStatus(Int32 currentUserID, Int32 invitationID)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().UpdateInvitationViewedStatus(currentUserID, invitationID);
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

        #region UAT-1237 Add Agency/shared users to client user search

        public static List<SharedUserSearchContract> GetSharedUserSearchData(SharedUserSearchContract sharedUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                DataTable dtSharedUserData = BALUtils.GetProfileSharingRepoInstance().GetSharedUserSearchData(sharedUserSearchContract, customPagingArgsContract);

                if (!dtSharedUserData.IsNullOrEmpty() && dtSharedUserData.Rows.Count > 0)
                {
                    IEnumerable<DataRow> rows = dtSharedUserData.AsEnumerable();
                    return rows.Select(x => new SharedUserSearchContract
                    {
                        FirstName = Convert.ToString(x["FirstName"]).Trim(),
                        LastName = Convert.ToString(x["LastName"]).Trim(),
                        UserName = Convert.ToString(x["UserName"]).Trim(),
                        EmailAddress = Convert.ToString(x["EmailAddress"]).Trim(),
                        SharedUserID = Convert.ToInt32(x["SharedUserID"]),
                        TotalCount = Convert.ToInt32(x["TotalCount"])
                    }).ToList();
                }
                return new List<SharedUserSearchContract>();
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
        ///  Method to get Shared User Invitation Details based on Shared UserID
        /// </summary>
        /// <param name="p"></param>
        public static List<SharedUserSearchInvitationDetailsContract> GetSharedUserInvitationDetails(Int32 sharedUserID)
        {
            try
            {
                var lstSharedUserSearchDetails = new List<SharedUserSearchInvitationDetailsContract>();

                //Getting result from stored procedure 
                var lstInvitationDetails = BALUtils.GetProfileSharingRepoInstance().GetSharedUserInvitationDetails(sharedUserID);

                lstInvitationDetails.Select(col => col.InvitationID).Distinct().ForEach(invitationID =>
                {
                    var invitationDetails = lstInvitationDetails.Where(cond => cond.InvitationID == invitationID).FirstOrDefault();

                    var currentContract = new SharedUserSearchInvitationDetailsContract();
                    currentContract.InvitationID = invitationDetails.InvitationID;
                    currentContract.ApplicantUserID = invitationDetails.ApplicantUserID;
                    currentContract.ApplicantName = invitationDetails.ApplicantName;
                    currentContract.InvitationDate = invitationDetails.InvitationDate;
                    currentContract.InvitationDocumentID = invitationDetails.InvitationDocumentID;
                    currentContract.InvitationSentStatus = invitationDetails.InvitationSentStatus;
                    currentContract.TenantID = invitationDetails.TenantID;
                    currentContract.TenantName = invitationDetails.TenantName;
                    currentContract.ViewedStatus = invitationDetails.ViewedStatus;
                    currentContract.InvitationSourceCode = invitationDetails.InvitationSourceCode;
                    currentContract.AgencyName = invitationDetails.AgencyName;
                    currentContract.lstSharedPackages = new List<INTSOF.UI.Contract.SearchUI.SharedPackages>();

                    //Add Compliance Packages Data
                    var compliancePackageSharedList = AddSharedPackagesData(lstInvitationDetails.Where(cond => cond.InvitationID == invitationID).ToList(), SystemPackageTypes.COMPLIANCE_PKG.GetStringValue());
                    currentContract.lstSharedPackages.AddRange(compliancePackageSharedList);

                    //Add Bkg Package Data
                    var bkgPackageSharedList = AddSharedPackagesData(lstInvitationDetails.Where(cond => cond.InvitationID == invitationID).ToList(), SystemPackageTypes.BACKGROUND_PKG.GetStringValue());
                    currentContract.lstSharedPackages.AddRange(bkgPackageSharedList);

                    //UAT-1310 - Add Rotation Requirement Package Data
                    var reqPackageSharedList = AddSharedPackagesData(lstInvitationDetails.Where(cond => cond.InvitationID == invitationID).ToList(), SystemPackageTypes.REQUIREMENT_ROT_PKG.GetStringValue());
                    currentContract.lstSharedPackages.AddRange(reqPackageSharedList);

                    lstSharedUserSearchDetails.Add(currentContract);

                });
                return lstSharedUserSearchDetails;
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
        /// Method to get Shared Packages Data (Compliance and background)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isCompliancePackage"></param>
        /// <returns></returns>
        private static List<INTSOF.UI.Contract.SearchUI.SharedPackages> AddSharedPackagesData(List<GetSharedUserInvitationDetails_Result> lstInvitationDetails, String packageTypeCode)
        {
            var lstDistinctOrderID = lstInvitationDetails.Where(cond => cond.PackageTypeCode == packageTypeCode
                                        && !cond.OrderID.IsNullOrEmpty()
                                        && !cond.PackageID.IsNullOrEmpty())
                                        .Select(col => col.OrderID).Distinct().ToList();

            var lstSharedPackages = new List<INTSOF.UI.Contract.SearchUI.SharedPackages>();

            lstDistinctOrderID.ForEach(ordID =>
            {

                lstInvitationDetails.Where(cond => cond.PackageTypeCode == packageTypeCode
                                        && cond.OrderID == ordID).Select(col => col.PackageID).Distinct().ForEach(pkgID =>
                                        {
                                            var currentSharedPkg = new INTSOF.UI.Contract.SearchUI.SharedPackages();
                                            var pkgList = lstInvitationDetails.Where(cond => cond.OrderID == ordID && cond.PackageID == pkgID && cond.PackageTypeCode == packageTypeCode).ToList();

                                            currentSharedPkg.PackageTypeCode = packageTypeCode;
                                            currentSharedPkg.OrderID = ordID;
                                            currentSharedPkg.PackageID = pkgID;
                                            currentSharedPkg.PackageName = pkgList.Select(col => col.PackageName).FirstOrDefault();
                                            currentSharedPkg.PackageSubscriptionID = pkgList.Select(col => col.PackageSubscriptionID).FirstOrDefault();
                                            currentSharedPkg.SnapShotID = pkgList.Select(col => col.SnapShotID).FirstOrDefault();
                                            currentSharedPkg.PackageIdentifier = Guid.NewGuid();
                                            currentSharedPkg.FlagStatusImagePath = pkgList.Where(cond => !cond.FlagStatusImagePath.IsNullOrEmpty()).Select(col => col.FlagStatusImagePath).FirstOrDefault();
                                            currentSharedPkg.ColorFlagPath = pkgList.Where(cond => !cond.ColorFlagPath.IsNullOrEmpty()).Select(col => col.ColorFlagPath).FirstOrDefault();
                                            currentSharedPkg.ShowFlagText = pkgList.Where(cond => cond.ColorFlagPath.IsNullOrEmpty()).Any(col => col.ShowFlagText);
                                            currentSharedPkg.OrderNumber = pkgList.Select(col => col.OrderNumber).FirstOrDefault();
                                            var currentSharedEntityList = new List<INTSOF.UI.Contract.SearchUI.SharedEntity>();
                                            //var distnctCurrentSharedEntityList = new List<INTSOF.UI.Contract.SearchUI.SharedEntity>();
                                            //Boolean isResultReportVisible = false;

                                            pkgList.Where(cond => !cond.SharedEntityName.IsNullOrEmpty()).Select(col => col.SharedEntityID).Distinct().ForEach(entityID =>
                                            {
                                                var currentSharedEntity = new INTSOF.UI.Contract.SearchUI.SharedEntity();

                                                currentSharedEntity.IsResultReportVisible = pkgList.Where(cond => !cond.SharedEntityName.IsNullOrEmpty() && cond.SharedEntityID == entityID).Any(x => x.IsResultReportVisible == true);
                                                currentSharedEntity.SharedEntityID = entityID;
                                                currentSharedEntity.SharedEntityName = pkgList.Where(cond => !cond.SharedEntityName.IsNullOrEmpty() && cond.SharedEntityID == entityID).Select(col => col.SharedEntityName).First();
                                                if (!entityID.IsNullOrEmpty())
                                                {
                                                    currentSharedEntityList.Add(currentSharedEntity);
                                                }
                                            });

                                            //if (!currentSharedEntity.SharedEntityID.IsNullOrEmpty())
                                            //{
                                            //    currentSharedEntityList.Add(currentSharedEntity);    
                                            //}

                                            //currentSharedEntityList = pkgList.Where(cond => !cond.SharedEntityName.IsNullOrEmpty()).DistinctBy(x=>x.SharedEntityID).Select(col => new INTSOF.UI.Contract.SearchUI.SharedEntity
                                            //{
                                            //    SharedEntityID = col.SharedEntityID,
                                            //    SharedEntityName = col.SharedEntityName,
                                            //    IsResultReportVisible = col.IsResultReportVisible
                                            //}).ToList();

                                            currentSharedPkg.lstSharedEntity = new List<INTSOF.UI.Contract.SearchUI.SharedEntity>();
                                            if (currentSharedEntityList.IsNullOrEmpty())
                                            {
                                                currentSharedEntityList = new List<INTSOF.UI.Contract.SearchUI.SharedEntity>();
                                            }

                                            currentSharedPkg.lstSharedEntity.AddRange(currentSharedEntityList);
                                            lstSharedPackages.Add(currentSharedPkg);
                                        });
            });
            return lstSharedPackages;
        }
        #endregion

        #region UAT-1201 as a client admin, I should be able to view the attestations for any profile shares that I have sent.
        /// <summary>
        ///  UAT-1201 - Method to Bind Attestation Details Grid
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="currentUserID"></param>
        public static List<ProfileSharingInvitationGroupContract> GetAttestationDetailsData(Int32 clientID, Int32 currentUserID)
        {
            try
            {
                Int32 adminInitializedInvitationStatus = GetInvitationStatusIdByCode(LkpInviationStatusTypes.ADMIN_INITLIAZED.GetStringValue());
                return BALUtils.GetProfileSharingRepoInstance().GetAttestationDetailsData(clientID, currentUserID, adminInitializedInvitationStatus);
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
        /// UAT-1201 - Method to Get Attestation Documents Details By InvitationGroupID 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static List<InvitationDocument> GetAttestatationDocumentDetails(Int32 invitationGroupID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAttestatationDocumentDetails(invitationGroupID);
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
        /// UAT-1201 - Method to Get Invitation Document by invitationDocumentID
        /// </summary>
        /// <param name="invitationDocumentID"></param>
        /// <returns></returns>
        public static InvitationDocument GetInvitationDocumentByDocumentID(Int32 invitationDocumentID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationDocumentByDocumentID(invitationDocumentID);
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

        public static InvitationDocument GetInvitationDocumentByProfileSharingInvitationID(Int32 profilesharinginvitationID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationDocumentByProfileSharingInvitationID(profilesharinginvitationID);
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

        #endregion

        #region UAT-1218

        /// <summary>
        /// Method to Update userid in Client Contact 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="clientContactToken"></param>
        public static void UpdateClientContactUserID(Guid userID, String clientContactEmail, Int32 orgUserID)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().UpdateClientContactUserID(userID, clientContactEmail, orgUserID);
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

        #region Shared Invitation Detail for Requirement Packages
        public static RequirementPackageSubscription GetRequirementPackageSubscription(Int32 requirementPackageSubscriptionID, Int32 tenantID)
        {
            try
            {
                RequirementPackageSubscription requirementPackageSubscription = BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetRequirementPackageSubscription(requirementPackageSubscriptionID);
                return requirementPackageSubscription;
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
        /// Get the List of SharedComplianceSubscription
        /// </summary>  
        /// <returns></returns>
        public static SharedRequirementSubscription GetSharedRequirementSubscriptions(Int32 tenantId, Int32 invitationId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetSharedRequirementSubscriptions(invitationId);
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
        /// Get Requirement Data From snapshot.
        /// </summary>  
        /// <returns></returns>
        public static DataSet GetRequirementDataFromSnapshot(Int32 tenantId, Int32 snapshotId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetRequirementDataFromSnapshot(snapshotId);
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
        /// To get Applicant requirement documents From snapshot
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sharedcategoryids"></param>
        /// <param name="snapshotId"></param>
        /// <returns></returns>
        public static List<InvitationDocumentContract> GetApplicantRequirementDocumentsFromSnapshot(Int32 tenantId, String sharedcategoryids, Int32 snapshotId, Int32 loggedInUserID, String rotationID, Boolean IsApplicantDropped) //Added orguserid parameter for agency user permission check in usp
        {
            try
            {
                DataTable dataForQueue = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetApplicantRequirementDocumentsFromSnapshot(sharedcategoryids, snapshotId, loggedInUserID, rotationID, IsApplicantDropped);
                return AssignCategoryDocumentToDataModel(dataForQueue);
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
        /// UAT-3338
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sharedcategoryids"></param>
        /// <param name="loggedInUserID"></param>
        /// <param name="rotationID"></param>
        /// <param name="InstructorOrgId"></param>
        /// <returns></returns>
        public static List<InvitationDocumentContract> GetInstructorRequirementDocuments(Int32 tenantId, String sharedcategoryids, Int32 loggedInUserID, String rotationID, Int32 InstructorOrgId)
        {
            try
            {
                DataTable dataForQueue = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetInstructorRequirementDocuments(sharedcategoryids, loggedInUserID, rotationID, InstructorOrgId);
                return AssignCategoryDocumentToDataModel(dataForQueue);
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
        /// Get the List of Shared category list
        /// </summary>  
        /// <returns></returns>
        public static List<ApplicantRequirementCategoryData> GetSharedRequirementCategoryList(Int32 tenantId, Int32 packageSubscriptionId, List<Int32> sharedCategoryIds, Int32 snapshotId, Boolean IsInstructorPreceptorData)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetSharedRequirementCategoryList(packageSubscriptionId, sharedCategoryIds, snapshotId, IsInstructorPreceptorData);
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

        #region UAT-1318
        /// <summary>
        /// Method to get Invitee User TypeID by Code.
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <returns></returns>
        public static Int32 GetUserTypeIdByCode(String userTypeCode)
        {
            try
            {
                Entity.SharedDataEntity.lkpOrgUserType userType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpOrgUserType>().FirstOrDefault(x => x.OrgUserTypeCode == userTypeCode && x.IsActive);
                if (userType != null)
                {
                    return userType.OrgUserTypeID;
                }
                return AppConsts.NONE;
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
        #endregion

        public static usp_GetAgencyDetailByAgencyID_Result GetAgencyDetailByAgencyID(Int32 agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyDetailByAgencyID(agencyID);
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

        public static List<usp_SearchAgency_Result> SearchAgency(string searchStatus)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SearchAgency(searchStatus);
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

        public static Tuple<String, Int32> SaveAgencyInstitutionMapping(AgencyInstitution agencyInstitution)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveAgencyInstitutionMapping(agencyInstitution);
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

        public static bool IsAgencyAssociateWithInstitution(Int32 institutionID, Int32 agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsAgencyAssociateWithInstitution(institutionID, agencyID);
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

        public static bool UpdateSharedCategoryData(Int32 tenantId, Int32 profileSharingInvitationID, List<SharedInvitationSubscriptionContract> lstSharedInvitationSubscriptionContract)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).UpdateSharedCategoryData(profileSharingInvitationID, lstSharedInvitationSubscriptionContract);
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
        /// Method to cehck whether Shared User recieved any Bkg Order invitation
        /// </summary>
        /// <returns></returns>
        public static bool CheckForBkgInvitation(Int32 orgUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().CheckForBkgInvitation(orgUserID);
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
        /// Method to Get all Scheduled Invitations
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static List<SheduledInvitationContract> GetScheduledInvitations(Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetScheduledInvitations(chunkSize, maxRetryCount, retryTimeLag);
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
        /// Method to save Scheduled Invitaions Data (called by job)
        /// </summary>
        /// <param name="applicantOrgUserIdCSV"></param>
        /// <param name="rotationID"></param>
        public static void SaveScheduledInvitationData(String applicantOrgUserIdCSV, Int32 rotationID, Int32 currentUserId, Int32 tenantID, List<SheduledInvitationContract> lstCurrentGroupInvitation, Int32 currentGroupID, Int32 agencyID)
        {
            try
            {
                List<SharingPackageDataContract> lstSharedPkgData = new List<SharingPackageDataContract>();

                var rotationDetailsContract = new ClinicalRotationDetailContract();
                Boolean isRotationSharing = false;
                if (rotationID > 0)
                {
                    isRotationSharing = true;
                }

                var _profileSharingClientRepoInstance = BALUtils.GetProfileSharingClientRepoInstance(tenantID);

                DataSet dsScheduledInvData = _profileSharingClientRepoInstance.GetScheduledInvitationData(applicantOrgUserIdCSV, rotationID);
                var _lstCmpPkgDetails = new List<ProfileSharingPackages>();
                var _lstBkgPkgDetails = new List<ProfileSharingPackages>();
                var _lstReqPkgDetails = new List<ProfileSharingRequirementPackage>();
                var _lstApplicantDetails = new List<OrganizationUserContract>();

                if (dsScheduledInvData.Tables.Count > 0 && dsScheduledInvData.Tables[0].Rows.Count > 0)
                {
                    _lstCmpPkgDetails = GetSharingCompliancePkgDetails(dsScheduledInvData);
                }

                if (dsScheduledInvData.Tables.Count > 0 && dsScheduledInvData.Tables[1].Rows.Count > 0)
                {
                    _lstBkgPkgDetails = GetSharingBkgPkgDetails(dsScheduledInvData);
                }

                if (dsScheduledInvData.Tables.Count > 0 && dsScheduledInvData.Tables[2].Rows.Count > 0 && rotationID > 0)
                {
                    _lstReqPkgDetails = GetSharingReqPkgDetails(dsScheduledInvData);
                }

                if (dsScheduledInvData.Tables.Count > 0 && dsScheduledInvData.Tables[3].Rows.Count > 0)
                {
                    _lstApplicantDetails = GetApplicantDetails(dsScheduledInvData.Tables[3]);
                }

                //Code Added to exclude packages or caterories
                FilterInvitationPackages(tenantID, currentGroupID, ref _lstCmpPkgDetails, ref _lstBkgPkgDetails);

                List<ComplianceInvitationData> _lstcompliancePkgDataList = GetSharingComplianceData(tenantID, _lstCmpPkgDetails, true, currentUserId);
                List<BkgInvitationData> _lstBkgPkgDataList = GetSharingBkgPkgData(_lstBkgPkgDetails);
                List<RequirementInvitationData> _lstRequirementDataList = new List<RequirementInvitationData>();

                List<SharedUserSubscriptionSnapshotContract> lstSharedUserSnapshot = new List<SharedUserSubscriptionSnapshotContract>();

                var customAttributesForNotification = ClinicalRotationManager.GetClinicalRotationNotificationCustomAttributes(tenantID, rotationID);

                var rotationDetails = String.Empty;
                if (isRotationSharing)
                {
                    var _lstSharedUserTypes = LookupManager.GetLookUpData<lkpSharedUserType>(tenantID).ToList();
                    var _instructorCode = OrganizationUserType.Instructor.GetStringValue();
                    var _preceptorCode = OrganizationUserType.Preceptor.GetStringValue();
                    var _instructorTypeId = _lstSharedUserTypes.Where(sut => sut.SUT_Code == _instructorCode).First().SUT_ID;
                    var _preceptorTypeId = _lstSharedUserTypes.Where(sut => sut.SUT_Code == _preceptorCode).First().SUT_ID;

                    _lstRequirementDataList = GetSharingRequirementData(tenantID, _lstReqPkgDetails, true, currentUserId);
                    rotationDetailsContract = ClinicalRotationManager.GetClinicalRotationById(tenantID, rotationID, null);
                    rotationDetails = GenerateRotationDetailsHTML(rotationDetailsContract);

                    var lstSharedUserSubscriptions = _profileSharingClientRepoInstance.GetSharedUserSubscriptions(rotationID);

                    foreach (var clientContact in lstSharedUserSubscriptions)
                    {
                        var snapshotId = ProfileSharingManager.SaveRequirementSnapshot(currentUserId, Convert.ToInt32(clientContact.ReqSubId), tenantID);
                        lstSharedUserSnapshot.Add(new SharedUserSubscriptionSnapshotContract
                        {
                            SnapshotId = snapshotId,
                            RequirementSubscriptionId = Convert.ToInt32(clientContact.ReqSubId),
                            SharedUserId = clientContact.ClientContactID,
                            SharedUserTypeId = clientContact.ClientContactTypeCode == ClientContactType.Instructor.GetStringValue() ? _instructorTypeId : _preceptorTypeId
                        });
                    }
                }

                List<InvitationSharedInfoDetails> lstInvitationSharedInfoDetails = new List<InvitationSharedInfoDetails>();
                List<InvitationDetailsContract> lstInvitations = new List<InvitationDetailsContract>();
                var lstInvitationTenantDataSaved = new List<InvitationDetailsContract>();
                var _currentDateTime = DateTime.Now;

                int _invitationInitiatedByID = 0;
                string _invitationInitiatedUserEmailId = string.Empty;
                string _invitationInitiatedUserFullName = string.Empty;
                int _groupId = 0;

                if (!lstCurrentGroupInvitation.IsNullOrEmpty())
                {
                    _invitationInitiatedByID = lstCurrentGroupInvitation.FirstOrDefault().InvitationInitiatedByID;
                    _invitationInitiatedUserEmailId = lstCurrentGroupInvitation.FirstOrDefault().InvitationInitiatedUserEmailId;
                    _invitationInitiatedUserFullName = lstCurrentGroupInvitation.FirstOrDefault().InvitationInitiatedUserFullName;
                    _groupId = lstCurrentGroupInvitation.FirstOrDefault().InvitationGroupID;
                }

                foreach (var invitation in lstCurrentGroupInvitation)
                {
                    var currentInvitation = new InvitationDetailsContract();
                    currentInvitation.PSIId = invitation.InvitationID;
                    currentInvitation.PSIGroupId = invitation.InvitationGroupID;
                    currentInvitation.CurrentUserId = currentUserId;
                    currentInvitation.CurrentDateTime = _currentDateTime;
                    currentInvitation.SharedApplicantMetaDataIds = invitation.SharedApplicantMetaDataIds.Split(',').Select(Int32.Parse).ToList();
                    currentInvitation.SharedApplicantMetaDataCode = invitation.InviteeSharedMetadataCodes.Split(',').ToList();
                    currentInvitation.ApplicantId = invitation.ApplicantID;
                    currentInvitation.EmailAddress = invitation.EmailAddress;
                    currentInvitation.InviteeOrgUserId = invitation.InviteeUserId;
                    currentInvitation.InviteeUserTypeCode = invitation.InviteeUserTypeCode;
                    currentInvitation.Agency = invitation.InviteeAgency;

                    var applicantInfo = _lstApplicantDetails.Where(cond => cond.OrganizationUserID == currentInvitation.ApplicantId).FirstOrDefault();
                    if (isRotationSharing)
                    {
                        currentInvitation.TemplateData = GenerateEmailMetaDataScheduledinvitations(currentInvitation.SharedApplicantMetaDataCode, _lstApplicantDetails, null, invitation.InviteeName, invitation.InviteeUserTypeCode, invitation.InvitationToken, true, tenantID, rotationID, rotationDetails);
                    }
                    else
                    {
                        currentInvitation.TemplateData = GenerateEmailMetaDataScheduledinvitations(currentInvitation.SharedApplicantMetaDataCode, null, applicantInfo, invitation.InviteeName, invitation.InviteeUserTypeCode, invitation.InvitationToken, false, tenantID, rotationID, rotationDetails);
                    }

                    currentInvitation.TemplateData.Add(AppConsts.PSIEMAIL_CustomAttributes, customAttributesForNotification);

                    //Add Applicant Compliance Packages List for Current Invitation
                    List<ComplianceInvitationData> applicantCompliancePkgData = _lstcompliancePkgDataList
                                                    .Where(cond => cond.ApplicantUserID == currentInvitation.ApplicantId).ToList();
                    AddCompliancePackage(applicantCompliancePkgData, invitation.ComplianceSharedInfoTypeCode, currentInvitation);

                    //Add Applicant Background Packages List for Current Invitation
                    List<BkgInvitationData> applicantBkgPkgData = _lstBkgPkgDataList.Where(cond => cond.ApplicantUserID == currentInvitation.ApplicantId).ToList();
                    AddBackgroundPackage(applicantBkgPkgData, invitation.BkgSharedInfoTypeCode, currentInvitation);

                    //Add Applicant Background Packages List for Current Invitation
                    List<RequirementInvitationData> applicantRequiremntPkgData = _lstRequirementDataList
                                                                                .Where(cond => cond.ApplicantUserID == currentInvitation.ApplicantId).ToList();
                    AddRequirementPackage(applicantRequiremntPkgData, invitation.RequiredSharedInfoTypeCode, currentInvitation);

                    lstInvitations.Add(currentInvitation);

                    if (!invitation.IsTenantDataSaved)
                    {
                        lstInvitationTenantDataSaved.Add(currentInvitation);
                    }

                    GenerateAttestationReportData(lstInvitationSharedInfoDetails, invitation.ComplianceSharedInfoTypeCode, invitation.RequiredSharedInfoTypeCode
                                                   , invitation.BkgSharedInfoTypeCode, null, invitation.InvitationID);
                }

                //Save Shared Applicant MetaData IDs
                List<SheduledInvitationContract> scheduledInvitationContractList = lstCurrentGroupInvitation.Where(cond => !cond.IsTenantDataSaved).ToList();
                SaveSchledInvSharedMetaData(scheduledInvitationContractList, currentUserId, _currentDateTime);

                //Save invitations Data into Tenant
                Boolean isTenantDataSaved = SaveScheduledAdminInvitationDetails(lstInvitationTenantDataSaved, lstSharedUserSnapshot, rotationID, tenantID, agencyID);

                //update IsTenantDataSaved field if invitations data saved successfully
                if (isTenantDataSaved)
                {
                    UpdateInvitationGroupSaveStatus(lstInvitations.Select(col => col.PSIId).ToList(), currentUserId);
                }

                //Get existing InvitationDocuemtnMapping w.r.t InvitationIDs AND exclude those which exist DB.
                //Generate Attestation Report only when tenant data saved is True
                List<InvitationDocumentMapping> existingInvDocMappingList = BALUtils.GetProfileSharingRepoInstance().GetInvitationDocumentMapping(currentGroupID).ToList();
                List<Int32> existingInvDocMappingInvIDs = existingInvDocMappingList.Where(cond => cond.IDM_ProfileSharingInvitationID.HasValue)
                                                                     .Select(col => col.IDM_ProfileSharingInvitationID.Value).Distinct().ToList();

                List<InvitationSharedInfoDetails> newInvitationSharedInfoDetails = lstInvitationSharedInfoDetails
                                                                                   .Where(cond => !existingInvDocMappingInvIDs.Contains(cond.SharingInvitationID))
                                                                                   .ToList();
                if (newInvitationSharedInfoDetails.Count() > AppConsts.NONE)
                {
                    GenerateAttestationReport(newInvitationSharedInfoDetails, currentGroupID, isRotationSharing, tenantID, currentUserId, agencyID, rotationID);
                }

                //Send Invitation Emails
                var _lstInvitationsContractResult = new List<InvitationDetailsContract>();

                List<Int32> lstSharedStudentIds = new List<Int32>();
                string agencyName = string.Empty;

                if (!lstInvitations.IsNullOrEmpty())
                    agencyName = lstInvitations.First().Agency;

                //Getting list of shared applicants whom profile shared to shared users
                lstSharedStudentIds = lstInvitations.Select(cond => cond.ApplicantId).Distinct().ToList();

                if (isRotationSharing)
                {
                    _lstInvitationsContractResult = lstInvitations.DistinctBy(x => x.EmailAddress).ToList();

                    foreach (var invitation in _lstInvitationsContractResult)
                    {
                        //invitation.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmail(_profileSharingURL, invitation.EmailAddress,
                        //                                          String.Empty, String.Empty, false,
                        //                                          invitation.TemplateData, isRotationSharing, tenantID, true);
                        invitation.TemplateData.Add(AppConsts.PSIEMAIL_RECIPIENTID, Convert.ToString(invitation.InviteeOrgUserId));
                        //UAT-2803
                        Boolean isInviteeHavePermission = false;
                        if (!invitation.InviteeOrgUserId.IsNullOrEmpty())
                            isInviteeHavePermission = ProfileSharingManager.IsOrgUserhaveNotificationPermission(invitation.InviteeOrgUserId.Value, AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue());

                        if (isInviteeHavePermission)
                        {
                            invitation.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmailFromTemplate(invitation.TemplateData
                                , CommunicationSubEvents.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue()
                                , currentUserId, invitation.EmailAddress, agencyName);
                            // Update the "IsEmailSentSuccessfully" flag for All the invitations for the current EmailAddress
                            lstInvitations.Where(s => s.EmailAddress == invitation.EmailAddress).ForEach(invCont =>
                            {
                                invCont.IsEmailSentSuccessfully = invitation.IsEmailSentSuccessfully;
                            });
                        }
                        else
                        {
                            lstInvitations.Where(s => s.EmailAddress == invitation.EmailAddress).ForEach(invCont =>
                            {
                                invCont.IsEmailSentSuccessfully = true;
                            });
                        }
                    }

                    #region UAT-3254
                    String RotationHierarchyIds = String.Empty;
                    if (!rotationDetailsContract.IsNullOrEmpty() && rotationID > AppConsts.NONE)
                    {
                        RotationHierarchyIds = BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetRotationHierarchyIdsByRotationID(rotationID);
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(_invitationInitiatedUserEmailId))
                        ProfileSharingManager.SendConfirmationForInvitationSent(_invitationInitiatedUserFullName, _invitationInitiatedUserEmailId, _invitationInitiatedByID, tenantID, _groupId, currentUserId, lstSharedStudentIds, agencyName, rotationDetailsContract.RotationName, true, RotationHierarchyIds, rotationID);
                }
                else
                {
                    //_lstInvitationsContractResult = _lstInvitationsContract.ToList();
                    foreach (var invitation in lstInvitations)
                    {
                        //invitation.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmail(_profileSharingURL, invitation.EmailAddress,
                        //                                          String.Empty, String.Empty, false,
                        //                                          invitation.TemplateData, true, tenantID);
                        invitation.TemplateData.Add(AppConsts.PSIEMAIL_RECIPIENTID, Convert.ToString(invitation.InviteeOrgUserId));
                        //UAT-2803
                        Boolean isInviteeHavePermission = false;
                        if (!invitation.InviteeOrgUserId.IsNullOrEmpty())
                            isInviteeHavePermission = ProfileSharingManager.IsOrgUserhaveNotificationPermission(invitation.InviteeOrgUserId.Value, AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_NON_ROTATION.GetStringValue());

                        if (isInviteeHavePermission)
                        {
                            invitation.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmailFromTemplate(invitation.TemplateData
                                , CommunicationSubEvents.REQUIREMENTS_SHARING_INVITATION_NON_ROTATION.GetStringValue()
                                , currentUserId, invitation.EmailAddress, agencyName);
                        }
                        else { invitation.IsEmailSentSuccessfully = true; } //UAT-3452: To update Invitation Status, in case "Requirements Sharing Invitation - Non-Rotation" settings are off for all agency users.
                    }

                    if (!String.IsNullOrEmpty(_invitationInitiatedUserEmailId))
                        ProfileSharingManager.SendConfirmationForInvitationSent(_invitationInitiatedUserFullName, _invitationInitiatedUserEmailId, _invitationInitiatedByID, tenantID, _groupId, currentUserId, lstSharedStudentIds, agencyName, string.Empty, false);
                }

                //if (lstInvitations.Any(cond => !cond.IsEmailSentSuccessfully))
                //{
                //    UpdateRetryCountForFailedInvitation(currentGroupID, currentUserId);
                //}

                // Update Invitation Status to 'NEW' for each invitation, for which Email has been sent successfully
                var _lstInvitationIDs = lstInvitations.Where(cond => cond.IsEmailSentSuccessfully).Select(col => col.PSIId).ToList();

                if (!_lstInvitationIDs.IsNullOrEmpty())
                {

                    UpdateBulkInvitationStatus(LkpInviationStatusTypes.NEW.GetStringValue(), _lstInvitationIDs, currentUserId);

                    //UAT-2544:Approved Rotation Student Sharing Functionality
                    //List<Int32> distinctOrgUserId = lstInvitations.DistinctBy(x => x.InviteeOrgUserId).Where(cond => cond.InviteeOrgUserId.HasValue).Select(x => x.InviteeOrgUserId.Value).ToList();
                    List<Int32> distinctApplicantUserId = lstInvitations.DistinctBy(x => x.ApplicantId).Where(x => x.InviteeOrgUserId.HasValue).Select(x => x.ApplicantId).ToList();
                    distinctApplicantUserId.ForEach(AppOrgId =>
                    {
                        List<Int32> invitIds = new List<Int32>();
                        var invitationData = lstInvitations.Where(cnd => cnd.ApplicantId == AppOrgId).ToList();
                        if (!invitationData.IsNullOrEmpty())
                        {
                            //Int32 organizationUserID = invitationData.FirstOrDefault().ApplicantId;
                            invitIds = invitationData.Select(slct => slct.PSIId).ToList();
                            Boolean needToChangeStatusAsPending = true;
                            needToChangeStatusAsPending = ClinicalRotationManager.NeedToChangeInvitationStatusAsPending(tenantID, rotationID, invitIds, AppOrgId, currentUserId);
                            if (!needToChangeStatusAsPending && !invitIds.IsNullOrEmpty())
                            {
                                ProfileSharingManager.SaveUpdateSharedUserInvitationReviewStatus(invitIds, currentUserId, AppOrgId, AppConsts.NONE, String.Empty, SharedUserInvitationReviewStatus.PENDING_REVIEW.GetStringValue(), false, needToChangeStatusAsPending, AppOrgId, rotationID, tenantID, false, true);

                                //Update Rotation Status
                                List<Int32> lstInviteeUserIds = invitationData.DistinctBy(x => x.InviteeOrgUserId).Where(x => x.InviteeOrgUserId.HasValue).Select(x => x.InviteeOrgUserId.Value).ToList();
                                lstInviteeUserIds.ForEach(inviteeUserID =>
                                {
                                    String reviewStatusUpdateCode = String.Empty;
                                    Int32? lastReviewedByID = null;
                                    reviewStatusUpdateCode = ProfileSharingManager.GetRotationSharedReviewStatus(rotationID, inviteeUserID, inviteeUserID, tenantID, agencyID, ref lastReviewedByID);
                                    ClinicalRotationManager.SaveUpdateUserRotationReviewStatus(tenantID, rotationID, currentUserId, inviteeUserID, reviewStatusUpdateCode, agencyID, lastReviewedByID, true);
                                });
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                //UpdateRetryCountForFailedInvitation(currentGroupID, currentUserId);

                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static void FilterInvitationPackages(int tenantID, int profileSharingInvitationGroupID, ref List<ProfileSharingPackages> lstCmpPkg, ref List<ProfileSharingPackages> lstBkgPkg)
        {
            List<ScheduledInvitationExcludedPackageData> lstExcludedPackageData = BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetScheduledInvitationExcludedPackageDataByProfileSharingInvitationGroupID(profileSharingInvitationGroupID);

            if (lstExcludedPackageData.IsNotNull() && lstExcludedPackageData.Count > 0)
            {
                //Performing Opertaion Compliance Packages

                //Getting list of compliance packages ID's that need to exclude.
                List<Int32> lstDistinctCompPkgNeedToExclude = lstExcludedPackageData.
                                                                Where(cond => cond.SIEPD_IsCompliancePackage == true).
                                                                Select(n => (int)n.SIEPD_CompliancePackageID).Distinct().ToList();


                foreach (var compPkgID in lstDistinctCompPkgNeedToExclude)
                {
                    if (lstCmpPkg.Any(cond => cond.PackageId == compPkgID))
                    {
                        //Getting List of compliance category ID's that need to exclude
                        List<Int32> lstCompCatIDsNeedToExclude = lstExcludedPackageData.
                                                                Where(cond => cond.SIEPD_IsCompliancePackage == true && cond.SIEPD_CompliancePackageID == compPkgID).
                                                                Select(n => (int)n.SIEPD_ComplianceCategoryID).ToList();

                        //Remove categories
                        lstCmpPkg.
                            Where(cond => cond.PackageId == compPkgID && cond.IsCompliancePkg == true).
                            ForEach(cond =>
                            {
                                cond.CompliancePkgCategories.RemoveAll(x => lstCompCatIDsNeedToExclude.Contains(x.ComplianceCategoryID));
                            });
                    }
                }

                //Remove comp packages have not any category
                lstCmpPkg = lstCmpPkg.Where(cond => cond.CompliancePkgCategories.Count > 0).ToList();

                //Performing Opertaion Background Packages

                //Getting list of background packages ID's that need to exclude.
                List<Int32> lstDistinctbkgPkgNeedToExclude = lstExcludedPackageData.
                                                                Where(cond => cond.SIEPD_IsCompliancePackage == false).
                                                                Select(n => (int)n.SIEPD_BackgroundPackageID).Distinct().ToList();

                foreach (var bkgPkgID in lstDistinctbkgPkgNeedToExclude)
                {
                    if (lstBkgPkg.Any(cond => cond.PackageId == bkgPkgID))
                    {
                        //Getting List of background service group ID's that need to exclude
                        List<Int32> lstBkgSvcGrpIDsNeedToExclude = lstExcludedPackageData.
                                                                Where(cond => cond.SIEPD_IsCompliancePackage == false && cond.SIEPD_BackgroundPackageID == bkgPkgID).
                                                                Select(n => (int)n.SIEPD_BkgServiceGroupID).ToList();

                        //Remove BkgSvcGroups
                        lstBkgPkg.
                            Where(cond => cond.PackageId == bkgPkgID && cond.IsCompliancePkg == false).
                            ForEach(cond =>
                            {
                                cond.BkgSvcGroups.RemoveAll(x => lstBkgSvcGrpIDsNeedToExclude.Contains(x.BSG_ID));
                            });
                    }
                }

                //Remove BkgOrder packages have not any service group
                lstBkgPkg = lstBkgPkg.Where(cond => cond.BkgSvcGroups.Count > 0).ToList();
            }

        }

        /// <summary>
        /// Method to update retry count if invitation failed to complete
        /// </summary>
        /// <param name="currentGroupID"></param>
        /// <param name="currentUserId"></param>
        private static void UpdateRetryCountForFailedInvitation(Int32 currentGroupID, Int32 currentUserId)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().UpdateRetryCountForFailedInvitation(currentGroupID, currentUserId);
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

        private static void SaveSchledInvSharedMetaData(List<SheduledInvitationContract> lstCurrentGroupInvitation, int currentUserId, DateTime currentDateTime)
        {

            //, List<InvitationSharedInfoDetails> lstInvitationSharedInfoDetails, Boolean isRotationSharing,
            try
            {
                if (lstCurrentGroupInvitation.IsNullOrEmpty())
                {
                    return;
                }
                Int32 currentGroupID = lstCurrentGroupInvitation.FirstOrDefault().InvitationGroupID;
                Int32 tenantID = lstCurrentGroupInvitation.FirstOrDefault().TenantID;
                BALUtils.GetProfileSharingRepoInstance().SaveScheduledInvitationsSharedMetaData(lstCurrentGroupInvitation, currentUserId, currentDateTime);

                ////Generate Attestation Report
                //GenerateAttestationReport(lstInvitationSharedInfoDetails, currentGroupID, isRotationSharing, tenantID, currentUserId);                
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

        private static Dictionary<String, String> GenerateEmailMetaDataScheduledinvitations(List<String> _lstSharedMetaDataCodes, List<OrganizationUserContract> lstApplicantInfoContract, OrganizationUserContract applicantInfo,
                                                     String recepientName, String inviteeUserTypeCode, Guid invitationToken, Boolean isRotationSharing, Int32 tenantID, Int32 rotationID, String rotationDetailsHtml)
        {
            var _institutionName = SecurityManager.GetTenant(tenantID).TenantName;

            var _dicContent = new Dictionary<String, String>();

            _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTNAME, recepientName);
            _dicContent.Add(AppConsts.PSIEMAIL_CENTRALLOGINURL, _centralLoginUrl);
            _dicContent.Add(AppConsts.PSIEMAIL_SCHOOLNAME, _institutionName);


            var applicantDataHtml = String.Empty;
            //var rotationDetailsHtml = String.Empty;

            if (isRotationSharing)
            {
                if (!lstApplicantInfoContract.IsNullOrEmpty())
                {
                    applicantDataHtml = GenerateApplicantMetaDataStringRotSharing(lstApplicantInfoContract, _lstSharedMetaDataCodes, tenantID);
                }
                //rotationDetailsHtml = GenerateRotationDetailsHTML(rotationDetailsContract);
            }
            else
            {
                if (!applicantInfo.IsNullOrEmpty())
                {
                    _dicContent.Add(AppConsts.PSIEMAIL_STUDENTNAME, applicantInfo.FirstName + " " + applicantInfo.MiddleName + " " + applicantInfo.LastName);
                    applicantDataHtml = GenerateApplicantMetaDataString(applicantInfo, _lstSharedMetaDataCodes, tenantID, String.Empty, String.Empty);
                }
            }

            _dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, applicantDataHtml);
            _dicContent.Add(AppConsts.PSIEMAIL_RotationDetails, rotationDetailsHtml);

            var queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.QUERY_STRING_INVITE_TOKEN, Convert.ToString(invitationToken)},
                                                                    {AppConsts.QUERY_STRING_USER_TYPE_CODE , inviteeUserTypeCode},
                                                                    //UAT-2519
                                                                    {"IsSearchedShare",Convert.ToString(true)}
                                                                    //{AppConsts.IS_PROFILE_SHARE_SEARCH,Convert.ToString(IsProfileSharing)},
                                                                    //{AppConsts.IS_ROTATION_SHARE_SEARCH,Convert.ToString(IsRotationSharing)}				
                                                                 };
            //UAT-2519
            String applicationUrl = _profileSharingURL;
            if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                applicationUrl = string.Concat("http://", applicationUrl.Trim());
            }

            var url = String.Format(applicationUrl + "?args={0}", queryString.ToEncryptedQueryString());
            _dicContent.Add(AppConsts.PSIEMAIL_PROFILEURL, url);
            return _dicContent;
        }


        private static List<OrganizationUserContract> GetApplicantDetails(DataTable dtApplicantDetails)
        {
            IEnumerable<DataRow> rows = dtApplicantDetails.AsEnumerable();
            return rows.Select(x => new OrganizationUserContract
            {
                OrganizationUserID = Convert.ToInt32(x["ApplicantID"]),
                FirstName = Convert.ToString(x["FirstName"]),
                LastName = Convert.ToString(x["LastName"]),
                MiddleName = Convert.ToString(x["MiddleName"]),
                Email = Convert.ToString(x["PrimaryEmailAddress"]).Trim(),
                SecondaryEmailAddress = Convert.ToString(x["SecondaryEmailAddress"]).Trim(),
                Gender = Convert.ToString(x["Gender"]).Trim(),
                Phone = Convert.ToString(x["Phone"]).Trim(),
                Address1 = Convert.ToString(x["Address1"]).Trim(),
                Address2 = Convert.ToString(x["Address2"]).Trim(),
                Country = Convert.ToString(x["CountryName"]).Trim(),
                State = Convert.ToString(x["StateName"]).Trim(),
                City = Convert.ToString(x["CityName"]).Trim(),
                County = Convert.ToString(x["CountyName"]).Trim(),
                Zipcode = x["Zipcode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["Zipcode"]).Trim(),
                IsApplicant = Convert.ToBoolean(x["IsApplicant"]),
            }).ToList();
        }

        private static List<ProfileSharingRequirementPackage> GetSharingReqPkgDetails(DataSet dsScheduledInvData)
        {
            var _applicantIdColumn = "ApplicantId";
            var _reqPkgIdColumn = "RequirementPackageId";
            var _reqPkgNameColumn = "PackageName";
            var _lstReqPackages = new List<ProfileSharingRequirementPackage>();
            var _pkgSubIdColumn = "PkgSubscriptionId";
            var _pkgTypeCodeColumn = "PackageTypeCode";
            var _catNameColumn = "CategoryName";
            var _catIdColumn = "CategoryId";
            var _dtReqPkgs = dsScheduledInvData.Tables[2];

            DataView _dvReqPkg = new DataView(_dtReqPkgs);
            DataTable _dt = _dvReqPkg.ToTable(true, _pkgSubIdColumn);

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                var _sharingPackage = new ProfileSharingRequirementPackage();
                _sharingPackage.RequirementPkgCategories = new List<Entity.ClientEntity.RequirementCategory>();

                var _currentPkgSubId = Convert.ToInt32(_dt.Rows[i][_pkgSubIdColumn]);

                var _lstPkgs = _dtReqPkgs.AsEnumerable().ToList().Where
                (
                  pkg => Convert.ToInt32(pkg[_pkgSubIdColumn]) == _currentPkgSubId
                )
                .Select(itm => new
                {
                    PkgId = Convert.ToInt32(itm[_reqPkgIdColumn]),
                    PkgSubId = _currentPkgSubId,
                    PkgName = Convert.ToString(itm[_reqPkgNameColumn]),
                    CatName = Convert.ToString(itm[_catNameColumn]),
                    CatId = Convert.ToInt32(itm[_catIdColumn]),
                    PkgTypeCode = Convert.ToString(itm[_pkgTypeCodeColumn]),
                    ApplicantId = Convert.ToInt32(itm[_applicantIdColumn]),
                }).ToList();

                foreach (var pkg in _lstPkgs)
                {
                    _sharingPackage.RequirementPackageId = pkg.PkgId;
                    _sharingPackage.PackageSubscriptionId = pkg.PkgSubId;
                    _sharingPackage.RequirementPackageName = pkg.PkgName;
                    _sharingPackage.PackageTypeCode = pkg.PkgTypeCode;
                    _sharingPackage.ApplicantID = pkg.ApplicantId;

                    if (pkg.CatId > AppConsts.NONE && !_sharingPackage.RequirementPkgCategories.Any(rpc => rpc.RC_ID == pkg.CatId))
                    {
                        _sharingPackage.RequirementPkgCategories.Add(new Entity.ClientEntity.RequirementCategory
                        {
                            RC_CategoryName = pkg.CatName,
                            RC_ID = pkg.CatId
                        });
                    }
                }
                _lstReqPackages.Add(_sharingPackage);
            }
            return _lstReqPackages;
        }

        private static List<ProfileSharingPackages> GetSharingBkgPkgDetails(DataSet dsScheduledInvData)
        {
            var _lstPackages = new List<ProfileSharingPackages>();
            var _pkgIdColumn = "PackageId";
            var _pkgNameColumn = "PackageName";
            var _applicantIdColumn = "ApplicantId";
            Int32? nullValue = null;
            var _svcGrpNameColumn = "SvcGroupName";
            var _svcGrpIdColumn = "SvcGroupId";
            var _bkgOrderPkgIdColumn = "BkgOrderPkgId";

            var _dtBkgPkgs = dsScheduledInvData.Tables[1];

            DataView _dvBkgPkg = new DataView(_dtBkgPkgs);
            DataTable _dt = _dvBkgPkg.ToTable(true, _bkgOrderPkgIdColumn);

            var bkgPkgs = _dtBkgPkgs.AsEnumerable().ToList();

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                var _sharingPackage = new ProfileSharingPackages();
                _sharingPackage.BkgSvcGroups = new List<BkgSvcGroup>();
                var _currentBkgOrderPackageID = Convert.ToInt32(_dt.Rows[i][_bkgOrderPkgIdColumn]);

                var _lstPkgs = bkgPkgs.Where(
                          bkgOrderPackage => Convert.ToInt32(bkgOrderPackage[_bkgOrderPkgIdColumn]) == _currentBkgOrderPackageID
                      )
                      .Select(itm => new
                      {
                          PkgName = Convert.ToString(itm[_pkgNameColumn]),
                          SvcGrpName = Convert.ToString(itm[_svcGrpNameColumn]),
                          SvcGrpId = itm[_svcGrpIdColumn].GetType().Name == "DBNull" ? nullValue : Convert.ToInt32(itm[_svcGrpIdColumn]),
                          BkgOrderPkgId = Convert.ToInt32(itm[_bkgOrderPkgIdColumn]),
                          PackageId = Convert.ToInt32(itm[_pkgIdColumn]),
                          ApplicantId = Convert.ToInt32(itm[_applicantIdColumn]),
                      }).ToList();


                foreach (var pkg in _lstPkgs)
                {
                    _sharingPackage.PackageId = pkg.PackageId;
                    _sharingPackage.PackageName = pkg.PkgName;
                    _sharingPackage.IsCompliancePkg = false;
                    _sharingPackage.BkgOrderPkgId = pkg.BkgOrderPkgId;
                    _sharingPackage.ApplicantID = pkg.ApplicantId;
                    if (pkg.SvcGrpId.IsNotNull())
                        _sharingPackage.BkgSvcGroups.Add(new BkgSvcGroup
                        {
                            BSG_Name = pkg.SvcGrpName,
                            BSG_ID = pkg.SvcGrpId.Value
                        });
                }
                _lstPackages.Add(_sharingPackage);
            }
            return _lstPackages;
        }

        private static List<ProfileSharingPackages> GetSharingCompliancePkgDetails(DataSet dsScheduledInvData)
        {
            var _lstPackages = new List<ProfileSharingPackages>();
            var _pkgIdColumn = "PackageId";
            var _pkgNameColumn = "PackageName";
            var _applicantIdColumn = "ApplicantId";
            var _pkgSubIdColumn = "PkgSubscriptionId";
            var _catNameColumn = "CategoryName";
            var _catIdColumn = "CategoryId";
            var _dtCompliancePkgs = dsScheduledInvData.Tables[0];

            DataView _dvCompliancePkg = new DataView(_dtCompliancePkgs);
            DataTable _dt = _dvCompliancePkg.ToTable(true, _pkgSubIdColumn);

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                var _sharingPackage = new ProfileSharingPackages();
                _sharingPackage.CompliancePkgCategories = new List<ComplianceCategory>();
                var _currentPkgSubId = Convert.ToInt32(_dt.Rows[i][_pkgSubIdColumn]);

                var _lstPkgs = _dtCompliancePkgs.AsEnumerable().ToList().Where
                (
                  pkg => Convert.ToInt32(pkg[_pkgSubIdColumn]) == _currentPkgSubId
                )
                .Select(itm => new
                {
                    PkgId = Convert.ToInt32(itm[_pkgIdColumn]),
                    PkgSubId = _currentPkgSubId,
                    PkgName = Convert.ToString(itm[_pkgNameColumn]),
                    CatName = Convert.ToString(itm[_catNameColumn]),
                    CatId = Convert.ToInt32(itm[_catIdColumn]),
                    ApplicantId = Convert.ToInt32(itm[_applicantIdColumn]),
                }).ToList();

                foreach (var pkg in _lstPkgs)
                {
                    _sharingPackage.PackageId = pkg.PkgId;
                    _sharingPackage.PackageSubscriptionId = pkg.PkgSubId;
                    _sharingPackage.PackageName = pkg.PkgName;
                    _sharingPackage.IsCompliancePkg = true;
                    _sharingPackage.ApplicantID = pkg.ApplicantId;
                    if (pkg.CatId > AppConsts.NONE && !_sharingPackage.CompliancePkgCategories.Any(cpc => cpc.ComplianceCategoryID == pkg.CatId))
                    {
                        _sharingPackage.CompliancePkgCategories.Add(new ComplianceCategory
                        {
                            CategoryName = pkg.CatName,
                            ComplianceCategoryID = pkg.CatId
                        });
                    }
                }
                _lstPackages.Add(_sharingPackage);
            }
            return _lstPackages;
        }

        /// <summary>
        /// Add the Compliance Data ereceived from Stored procedures into Contract, to Save in the Database.
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="compliancePackages"></param>
        /// <returns></returns>
        public static List<ComplianceInvitationData> GetSharingComplianceData(Int32 tenantId, List<ProfileSharingPackages> compliancePackages
                                                                            , Boolean isNonScheduledInvitation, Int32 currentUserId)
        {
            List<ComplianceInvitationData> cmpliancePkgDataLst = new List<ComplianceInvitationData>();
            compliancePackages.ForEach(cmpPkgItm =>
            {
                ComplianceInvitationData cmpliancePkgData = new ComplianceInvitationData
                {
                    IsCompletePkgSelected = true,
                    lstCategoryIds = cmpPkgItm.CompliancePkgCategories.Select(col => col.ComplianceCategoryID).ToList(),
                    PkgSubId = Convert.ToInt32(cmpPkgItm.PackageSubscriptionId),
                    IsAnyCatSelected = true,
                    CategoryNames = String.Join(",", cmpPkgItm.CompliancePkgCategories.Select(col => col.CategoryName)),
                    PkgName = cmpPkgItm.PackageName,
                    ApplicantUserID = cmpPkgItm.ApplicantID
                };

                //CALL SNAPSHOT StoredProcedure and GET SnapshotId, Only for Non-scheduled Invitation
                if (isNonScheduledInvitation)
                {
                    Int32 snapshotID = ProfileSharingManager.SaveImmunizationSnapshot(tenantId, currentUserId, Convert.ToInt32(cmpPkgItm.PackageSubscriptionId));
                    cmpliancePkgData.SnapShotId = snapshotID;
                }
                cmpliancePkgDataLst.Add(cmpliancePkgData);
            });

            return cmpliancePkgDataLst;
        }

        /// <summary>
        /// Add the Background Data ereceived from Stored procedures into Contract, to Save in the Database.
        /// </summary>
        /// <param name="bkgPackages"></param>
        /// <returns></returns>
        public static List<BkgInvitationData> GetSharingBkgPkgData(List<ProfileSharingPackages> bkgPackages)
        {
            List<BkgInvitationData> bkgPkgDataLst = new List<BkgInvitationData>();
            bkgPackages.ForEach(bkgPkgItm =>
            {
                BkgInvitationData bkgPkgData = new BkgInvitationData
                {
                    BOPId = Convert.ToInt32(bkgPkgItm.BkgOrderPkgId),
                    lstSvcGrpIds = bkgPkgItm.BkgSvcGroups.Select(col => col.BSG_ID).ToList(),
                    IsAnySvcGrpSelected = true,
                    PkgName = bkgPkgItm.PackageName,
                    ApplicantUserID = bkgPkgItm.ApplicantID
                };

                bkgPkgData.SvcGroupNames = String.Join(",", bkgPkgItm.BkgSvcGroups.Select(col => col.BSG_Name));
                bkgPkgDataLst.Add(bkgPkgData);
            });
            return bkgPkgDataLst;
        }

        /// <summary>
        /// Generate the Snapshot for the Requirement Package and Convert the Data from SP to the Contract which needs to be saved.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="lstRequirementPackages"></param>
        /// <returns></returns>
        public static List<RequirementInvitationData> GetSharingRequirementData(Int32 clientId, List<ProfileSharingRequirementPackage> lstRequirementPackages
                                                                              , Boolean isNonScheduledInvitation, Int32 currentUserId)
        {
            var _lstReqPkgData = new List<RequirementInvitationData>();
            lstRequirementPackages.ForEach(reqPkgItem =>
            {
                var reqPkgData = new RequirementInvitationData();

                reqPkgData.lstRequirementCategoryIds = reqPkgItem.RequirementPkgCategories.Select(col => col.RC_ID).ToList();
                reqPkgData.RequirementPkgSubId = Convert.ToInt32(reqPkgItem.PackageSubscriptionId);
                reqPkgData.RequirementPkgName = reqPkgItem.RequirementPackageName;
                reqPkgData.ApplicantUserID = reqPkgItem.ApplicantID;

                if (isNonScheduledInvitation)
                {
                    var _snapshotId = ProfileSharingManager.SaveRequirementSnapshot(currentUserId, Convert.ToInt32(reqPkgItem.PackageSubscriptionId), clientId);
                    reqPkgData.RequirementSnapShotId = _snapshotId;
                }
                _lstReqPkgData.Add(reqPkgData);
            });
            return _lstReqPkgData;
        }

        /// <summary>
        /// Add the Compliance Package to the Invitations.
        /// </summary>
        /// <param name="compliancePkgDataList"></param>
        /// <param name="complianceSharedInfoTypeCode"></param>
        /// <param name="currentInvitation"></param>
        public static void AddCompliancePackage(List<ComplianceInvitationData> compliancePkgDataList, String complianceSharedInfoTypeCode, InvitationDetailsContract currentInvitation)
        {
            currentInvitation.lstComplianceData = new List<ComplianceInvitationData>();

            foreach (var cmpPkgItm in compliancePkgDataList)
            {
                currentInvitation.lstComplianceData.Add(new ComplianceInvitationData
                {
                    CategoryNames = cmpPkgItm.CategoryNames,
                    ComplianceSharedInfoTypeCode = complianceSharedInfoTypeCode,
                    IsAnyCatSelected = cmpPkgItm.IsAnyCatSelected,
                    IsCompletePkgSelected = cmpPkgItm.IsCompletePkgSelected,
                    lstCategoryIds = cmpPkgItm.lstCategoryIds,
                    PkgName = cmpPkgItm.PkgName,
                    PkgSubId = cmpPkgItm.PkgSubId,
                    SnapShotId = cmpPkgItm.SnapShotId,
                });
            }
        }

        /// <summary>
        /// Add the Background Package to the Invitations.
        /// </summary>
        /// <param name="bkgPkgDataList"></param>
        /// <param name="bkgSharedInfoTypeCode"></param>
        /// <param name="currentInvitation"></param> 
        public static void AddBackgroundPackage(List<BkgInvitationData> bkgPkgDataList, String bkgSharedInfoTypeCode, InvitationDetailsContract currentInvitation)
        {
            currentInvitation.lstBkgData = new List<BkgInvitationData>();

            foreach (var bkgPkgItm in bkgPkgDataList)
            {
                currentInvitation.lstBkgData.Add(new BkgInvitationData
                {
                    LstBkgSharedInfoTypeCode = bkgSharedInfoTypeCode.IsNullOrEmpty() ? new List<String>() : bkgSharedInfoTypeCode.Split(',').ToList(),
                    BOPId = bkgPkgItm.BOPId,
                    IsAnySvcGrpSelected = bkgPkgItm.IsAnySvcGrpSelected,
                    lstSvcGrpIds = bkgPkgItm.lstSvcGrpIds,
                    PkgName = bkgPkgItm.PkgName,
                    SvcGroupNames = bkgPkgItm.SvcGroupNames,
                });
            }
        }

        /// <summary>
        /// Add the Requirement Package data to the Invitation.
        /// </summary>
        /// <param name="lstRequirementData"></param>
        /// <param name="currentInvitation"></param>
        public static void AddRequirementPackage(List<RequirementInvitationData> lstRequirementData, String reqRotSharedInfoTypeCode, InvitationDetailsContract currentInvitation)
        {
            currentInvitation.lstRequirementData = new List<RequirementInvitationData>();

            foreach (var requirementPkg in lstRequirementData)
            {
                currentInvitation.lstRequirementData.Add(new RequirementInvitationData
                {
                    RequirementPkgName = requirementPkg.RequirementPkgName,
                    RequirementPkgSubId = requirementPkg.RequirementPkgSubId,
                    lstRequirementCategoryIds = requirementPkg.lstRequirementCategoryIds,
                    RequirementPkgSharedInfoTypeCode = reqRotSharedInfoTypeCode,
                    RequirementSnapShotId = requirementPkg.RequirementSnapShotId
                });
            }
        }


        /// <summary>
        /// Save the Bulk Scheduled Invitation Details in Tenant, sent by admin/client admin
        /// </summary>
        /// <param name="lstInvitations"></param>
        /// <param name="invitationGroup"></param>
        /// <param name="invitationGroup"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean SaveScheduledAdminInvitationDetails(List<InvitationDetailsContract> lstInvitations, List<SharedUserSubscriptionSnapshotContract> lstSharedUserSnapshot
                                                                  , Int32 rotationId, Int32 tenantId, Int32 agencyID)
        {
            try
            {
                var _lstSharedInfoTypes = LookupManager.GetLookUpData<Entity.ClientEntity.lkpInvitationSharedInfoType>(tenantId).ToList();
                var _reviewStatusId = AppConsts.NONE;

                if (rotationId > AppConsts.NONE)
                {
                    _reviewStatusId = GetRotationReviewStatusByCode(tenantId, SharedUserRotationReviewStatus.PENDING_REVIEW.GetStringValue());
                }

                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).SaveAdminInvitationDetails(lstInvitations, _lstSharedInfoTypes, lstSharedUserSnapshot, rotationId, _reviewStatusId, agencyID);
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
        /// Method to generate Attestation Report 
        /// </summary>
        /// <param name="lstInvitationSharedInfoDetails"></param>
        /// <param name="_lstInvitations"></param>
        #region Commented code for UAT-3715
        //public static void GenerateAttestationReport(List<InvitationSharedInfoDetails> lstInvitationSharedInfoDetails, Int32 generatedInvitationGroupId
        //                                             , Boolean isRotationSharing, Int32 tenantID, Int32 currentUserID, Int32 agencyID, Int32 selectedRotationId = 0)
        //{
        //    #region ATTESTATION REPORT CODE

        //    if (!lstInvitationSharedInfoDetails.IsNullOrEmpty())
        //    {
        //        //var generatedInvitationGroupId = _lstInvitations.First().ProfileSharingInvitationGroup.PSIG_ID;

        //        String sharedInfoPermission_RotationNone = SharedInfoType.REQUIREMENT_ROTATION_NONE.GetStringValue();
        //        String sharedInfoPermission_ComplianceNone = SharedInfoType.COMPLIANCE_NONE.GetStringValue();
        //        String sharedInfoPermission_FlagStatus = SharedInfoType.FLAG_STATUS.GetStringValue();

        //        // Added as per UAT 1464 : Updated permissions to manage agency users to handle "Attestation only"
        //        String bkgSharedInfoPermission_AttestationOnly = SharedInfoType.BACKGROUND_ATTESTATION_ONLY.GetStringValue();
        //        String complianceSharedInfoPermission_AttestationOnly = SharedInfoType.COMPLIANCE_ATTESTATION_ONLY.GetStringValue();
        //        String requirmentSharedInfoPermission_AttestationOnly = SharedInfoType.REQUIREMENT_ATTESTATION_ONLY.GetStringValue();

        //        List<InvitationDocumentMapping> lstInvitationDocumentMapping = new List<InvitationDocumentMapping>();

        //        if (isRotationSharing)
        //        {
        //            #region [UAT-2821]

        //            bool overrideAttestationReportWithForm = false;
        //            string selfUploadedDocpath = string.Empty;

        //            InvitationDocument uploadedInvDoc = BALUtils.GetProfileSharingRepoInstance().GetUploadedInvitationDocument(generatedInvitationGroupId, GetDocumentTypeIdByCode(LKPSharedSystemDocumentTypes.UPLOADED_INVITATION_DOCUMENT.GetStringValue()));

        //            if (!uploadedInvDoc.IsNullOrEmpty())
        //            {
        //                overrideAttestationReportWithForm = true;
        //                selfUploadedDocpath = uploadedInvDoc.IND_DocumentFilePath;
        //            }

        //            #endregion


        //            #region Generate the Lists for the types of Attestations, based on the permissions
        //            //UAT-2443
        //            Int32 invitationDocId = AppConsts.NONE;
        //            Boolean isDocMergingRequired = false;
        //            String previousAttestationDocPath = String.Empty;
        //            String singleAttestationDocPathToUpdate = String.Empty;

        //            List<InvitationSharedInfoDetails> oldAttestation = BALUtils.GetProfileSharingRepoInstance().GetInvitationDocumentData(selectedRotationId, tenantID, agencyID);


        //            // Screening + Tracking + Rotation
        //            List<InvitationSharedInfoDetails> _lstAll = lstInvitationSharedInfoDetails
        //                                                                    .Where(isid => isid.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
        //                                                                       && (isid.LstBkgSharedInfoTypeCode != null && isid.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                       && isid.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone).ToList();

        //            // Screening + Tracking
        //            List<InvitationSharedInfoDetails> _lstST = lstInvitationSharedInfoDetails
        //                                                                    .Where(isid => isid.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
        //                                                                       && (isid.LstBkgSharedInfoTypeCode != null && isid.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                       && isid.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone).ToList();

        //            // Tracking + Rotation
        //            List<InvitationSharedInfoDetails> _lstTR = lstInvitationSharedInfoDetails
        //                                                                    .Where(isid => isid.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
        //                                                                       && (isid.LstBkgSharedInfoTypeCode == null || isid.LstBkgSharedInfoTypeCode.Count == 0)
        //                                                                       && isid.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone).ToList();

        //            // Screening + Rotation
        //            List<InvitationSharedInfoDetails> _lstSR = lstInvitationSharedInfoDetails
        //                                                                    .Where(isid => isid.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
        //                                                                       && (isid.LstBkgSharedInfoTypeCode != null && isid.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                       && isid.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone).ToList();

        //            // Tracking Only
        //            List<InvitationSharedInfoDetails> _lstT = lstInvitationSharedInfoDetails
        //                                                                    .Where(isid => isid.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
        //                                                                       && (isid.LstBkgSharedInfoTypeCode == null || isid.LstBkgSharedInfoTypeCode.Count == 0)
        //                                                                       && isid.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone).ToList();

        //            // Rotation Only
        //            List<InvitationSharedInfoDetails> _lstR = lstInvitationSharedInfoDetails
        //                                                                    .Where(isid => isid.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
        //                                                                       && (isid.LstBkgSharedInfoTypeCode == null || isid.LstBkgSharedInfoTypeCode.Count == 0)
        //                                                                       && isid.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone).ToList();

        //            // Screening Only
        //            List<InvitationSharedInfoDetails> _lstS = lstInvitationSharedInfoDetails
        //                                                                    .Where(isid => isid.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
        //                                                                       && (isid.LstBkgSharedInfoTypeCode != null && isid.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                       && isid.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone).ToList();

        //            // No Permission
        //            List<InvitationSharedInfoDetails> _lstNone = lstInvitationSharedInfoDetails
        //                                                                    .Where(isid => isid.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
        //                                                                       && (isid.LstBkgSharedInfoTypeCode == null || isid.LstBkgSharedInfoTypeCode.Count == 0)
        //                                                                       && isid.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone).ToList();

        //            Boolean isAttestationFormMerged = false;
        //            Int32 prevAttestationFormDocumentID = 0;
        //            string singleAttestationFormDocPathToUpdate = string.Empty;
        //            bool isEveryOneDocSaved = false;
        //            InvitationDocument _doc_AttestationForm = new InvitationDocument();

        //            if (overrideAttestationReportWithForm)
        //            {
        //                InvitationSharedInfoDetails prevAttestationForm = oldAttestation.Where(cond => cond.IsForEveryOneAttestationForm == true).FirstOrDefault();

        //                _doc_AttestationForm = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID, true);

        //                if (!prevAttestationForm.IsNullOrEmpty())
        //                {
        //                    isEveryOneDocSaved = true;
        //                    isAttestationFormMerged = true;

        //                    prevAttestationFormDocumentID = prevAttestationForm.InvitationDocumentID;

        //                    singleAttestationFormDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_AttestationForm.IND_DocumentFilePath, prevAttestationForm.DocumentPath, currentUserID
        //                                                                                          , AttestationDocumentTypes.ATTESTATION_FORM.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);

        //                    UpdateInvitationDocumentPath(prevAttestationFormDocumentID, singleAttestationFormDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                }
        //            }

        //            #endregion

        //            #region Handle Rotation sharing based scenarios
        //            if (!_lstAll.IsNullOrEmpty())
        //            {
        //                #region All


        //                List<InvitationSharedInfoDetails> lstScreeningWithColorFlag = _lstAll
        //                                                                              .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                        || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
        //                                                                              .ToList();

        //                // Includes the Rotation, Tracking and Screening without Flag.
        //                List<InvitationSharedInfoDetails> lstAllWithoutColorFlag = _lstAll
        //                                                                           .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                       && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
        //                                                                           .ToList();

        //                if (!lstScreeningWithColorFlag.IsNullOrEmpty())
        //                {
        //                    invitationDocId = AppConsts.NONE;
        //                    isDocMergingRequired = false;
        //                    previousAttestationDocPath = String.Empty;
        //                    singleAttestationDocPathToUpdate = String.Empty;
        //                    InvitationSharedInfoDetails AllAttestationWithColorFlag = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                                               && (con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                         || con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
        //                                                                                         && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                                                         && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                    if (!AllAttestationWithColorFlag.IsNullOrEmpty())
        //                    {
        //                        invitationDocId = AllAttestationWithColorFlag.InvitationDocumentID;
        //                        isDocMergingRequired = true;
        //                        previousAttestationDocPath = AllAttestationWithColorFlag.DocumentPath;
        //                    }
        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, true, AttestationDocumentTypes.FULL.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, true, AttestationDocumentTypes.FULL.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);

        //                    if (!overrideAttestationReportWithForm)
        //                    {
        //                        InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, true, AttestationDocumentTypes.FULL.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    }

        //                    if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                    {

        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                            prevAttestationFormDocumentID, isAttestationFormMerged);
        //                        isEveryOneDocSaved = true;
        //                    }
        //                    else
        //                    {
        //                        InvitationDocument _doc_C = null;

        //                        if (overrideAttestationReportWithForm)
        //                            _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                        else
        //                            _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, true, AttestationDocumentTypes.FULL.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                        if (isDocMergingRequired)
        //                        {
        //                            singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                     , AttestationDocumentTypes.FULL.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                        }
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                             invitationDocId, isDocMergingRequired);
        //                        //UAT-2443:
        //                        if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                        {
        //                            UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                        }
        //                    }

        //                }
        //                if (!lstAllWithoutColorFlag.IsNullOrEmpty())
        //                {
        //                    invitationDocId = AppConsts.NONE;
        //                    isDocMergingRequired = false;
        //                    previousAttestationDocPath = String.Empty;
        //                    singleAttestationDocPathToUpdate = String.Empty;

        //                    InvitationSharedInfoDetails AllAttestationWithoutColorFlag = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                  && (!con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                            && !con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
        //                                                            && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                            && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                    if (!AllAttestationWithoutColorFlag.IsNullOrEmpty())
        //                    {
        //                        invitationDocId = AllAttestationWithoutColorFlag.InvitationDocumentID;
        //                        isDocMergingRequired = true;
        //                        previousAttestationDocPath = AllAttestationWithoutColorFlag.DocumentPath;
        //                    }

        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, false, AttestationDocumentTypes.TRACKING_SCREENING__ROTATION_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstAllWithoutColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, false, AttestationDocumentTypes.TRACKING_SCREENING__ROTATION_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstAllWithoutColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    if (!overrideAttestationReportWithForm)
        //                    {
        //                        InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, false, AttestationDocumentTypes.TRACKING_SCREENING__ROTATION_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstAllWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    }

        //                    if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                    {

        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstAllWithoutColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                         prevAttestationFormDocumentID, isAttestationFormMerged);
        //                        isEveryOneDocSaved = true;

        //                    }
        //                    else
        //                    {
        //                        InvitationDocument _doc_C = null;

        //                        if (overrideAttestationReportWithForm)
        //                            _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                        else
        //                            _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, false, AttestationDocumentTypes.TRACKING_SCREENING__ROTATION_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                        if (isDocMergingRequired)
        //                        {
        //                            singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                     , AttestationDocumentTypes.TRACKING_SCREENING__ROTATION_WITHOUT_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                        }
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstAllWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                             invitationDocId, isDocMergingRequired);
        //                        //UAT-2443:
        //                        if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                        {
        //                            UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                        }
        //                    }
        //                }
        //                #endregion
        //            }

        //            if (!_lstST.IsNullOrEmpty())
        //            {

        //                #region Screening + Tracking
        //                List<InvitationSharedInfoDetails> lstScreeningWithColorFlag = _lstST
        //                                                                              .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                          || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
        //                                                                              .ToList();

        //                // Includes the Tracking + Screening (without Flag)
        //                List<InvitationSharedInfoDetails> lstSTWithoutColorFlag = _lstST
        //                                                                          .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                      && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
        //                                                                          .ToList();

        //                if (!lstScreeningWithColorFlag.IsNullOrEmpty())
        //                {
        //                    invitationDocId = AppConsts.NONE;
        //                    isDocMergingRequired = false;
        //                    previousAttestationDocPath = String.Empty;
        //                    singleAttestationDocPathToUpdate = String.Empty;

        //                    InvitationSharedInfoDetails ScreeningTrackingAttestationWithColorFlag = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
        //                                      && (con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                || con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
        //                                && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                && !con.IsForEveryOneAttestationForm).FirstOrDefault();

        //                    if (!ScreeningTrackingAttestationWithColorFlag.IsNullOrEmpty())
        //                    {
        //                        invitationDocId = ScreeningTrackingAttestationWithColorFlag.InvitationDocumentID;
        //                        isDocMergingRequired = true;
        //                        previousAttestationDocPath = ScreeningTrackingAttestationWithColorFlag.DocumentPath;
        //                    }

        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.SCREENING_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.SCREENING_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    if (!overrideAttestationReportWithForm)
        //                    {
        //                        InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.SCREENING_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    }

        //                    if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                    {

        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                         prevAttestationFormDocumentID, isAttestationFormMerged);
        //                        isEveryOneDocSaved = true;

        //                    }
        //                    else
        //                    {
        //                        InvitationDocument _doc_C = null;

        //                        if (overrideAttestationReportWithForm)
        //                            _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                        else
        //                            _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.SCREENING_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                        if (isDocMergingRequired)
        //                        {
        //                            singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                     , AttestationDocumentTypes.SCREENING_AND_TRACKING.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                        }
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                             invitationDocId, isDocMergingRequired);
        //                        //UAT-2443:
        //                        if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                        {
        //                            UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                        }
        //                    }
        //                }
        //                if (!lstSTWithoutColorFlag.IsNullOrEmpty())
        //                {
        //                    invitationDocId = AppConsts.NONE;
        //                    isDocMergingRequired = false;
        //                    previousAttestationDocPath = String.Empty;
        //                    singleAttestationDocPathToUpdate = String.Empty;

        //                    InvitationSharedInfoDetails ScreeningTrackingAttestationWithoutColorFlag = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
        //                               && (!con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                && !con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
        //                                 && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                 && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                    if (!ScreeningTrackingAttestationWithoutColorFlag.IsNullOrEmpty())
        //                    {
        //                        invitationDocId = ScreeningTrackingAttestationWithoutColorFlag.InvitationDocumentID;
        //                        isDocMergingRequired = true;
        //                        previousAttestationDocPath = ScreeningTrackingAttestationWithoutColorFlag.DocumentPath;
        //                    }

        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSTWithoutColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSTWithoutColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    if (!overrideAttestationReportWithForm)
        //                    {
        //                        InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSTWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    }


        //                    if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                    {

        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSTWithoutColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                         prevAttestationFormDocumentID, isAttestationFormMerged);
        //                        isEveryOneDocSaved = true;

        //                    }
        //                    else
        //                    {
        //                        InvitationDocument _doc_C = null;

        //                        if (overrideAttestationReportWithForm)
        //                            _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                        else
        //                            _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                        if (isDocMergingRequired)
        //                        {
        //                            singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                     , AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                        }
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSTWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                             invitationDocId, isDocMergingRequired);
        //                        //UAT-2443:
        //                        if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                        {
        //                            UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                        }
        //                    }
        //                }
        //                #endregion
        //            }

        //            if (!_lstTR.IsNullOrEmpty())
        //            {
        //                invitationDocId = AppConsts.NONE;
        //                isDocMergingRequired = false;
        //                previousAttestationDocPath = String.Empty;
        //                singleAttestationDocPathToUpdate = String.Empty;

        //                InvitationSharedInfoDetails TrackingRotationAttestation = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode == null || con.LstBkgSharedInfoTypeCode.Count == 0)
        //                                                                                 && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                                                 && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                if (!TrackingRotationAttestation.IsNullOrEmpty())
        //                {
        //                    invitationDocId = TrackingRotationAttestation.InvitationDocumentID;
        //                    isDocMergingRequired = true;
        //                    previousAttestationDocPath = TrackingRotationAttestation.DocumentPath;
        //                }
        //                //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, false, true, false, AttestationDocumentTypes.ROTATION_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstTR, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, false, true, false, AttestationDocumentTypes.ROTATION_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstTR, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                if (!overrideAttestationReportWithForm)
        //                {
        //                    InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, false, true, false, AttestationDocumentTypes.ROTATION_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstTR, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                }

        //                if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                {

        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstTR, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                     prevAttestationFormDocumentID, isAttestationFormMerged);
        //                    isEveryOneDocSaved = true;

        //                }
        //                else
        //                {
        //                    InvitationDocument _doc_C = null;

        //                    if (overrideAttestationReportWithForm)
        //                        _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                    else
        //                        _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, false, true, false, AttestationDocumentTypes.ROTATION_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                    if (isDocMergingRequired)
        //                    {
        //                        singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                 , AttestationDocumentTypes.ROTATION_AND_TRACKING.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                    }
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstTR, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                         invitationDocId, isDocMergingRequired);
        //                    //UAT-2443:
        //                    if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                    {
        //                        UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                    }
        //                }
        //            }

        //            if (!_lstSR.IsNullOrEmpty())
        //            {

        //                #region Screening + Rotation

        //                List<InvitationSharedInfoDetails> lstScreeningWithColorFlag = _lstSR
        //                                                                              .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                          || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
        //                                                                              .ToList();

        //                // Includes the Rotation + Screening (without Flag)
        //                List<InvitationSharedInfoDetails> lstSRWithoutColorFlag = _lstSR
        //                                                                          .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                     && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
        //                                                                          .ToList();

        //                if (!lstScreeningWithColorFlag.IsNullOrEmpty())
        //                {
        //                    invitationDocId = AppConsts.NONE;
        //                    isDocMergingRequired = false;
        //                    previousAttestationDocPath = String.Empty;
        //                    singleAttestationDocPathToUpdate = String.Empty;

        //                    InvitationSharedInfoDetails ScreeningRotationAttestationWithColorFlag = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                     && (con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus)) || con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
        //                                                                       && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                                       && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                    if (!ScreeningRotationAttestationWithColorFlag.IsNullOrEmpty())
        //                    {
        //                        invitationDocId = ScreeningRotationAttestationWithColorFlag.InvitationDocumentID;
        //                        isDocMergingRequired = true;
        //                        previousAttestationDocPath = ScreeningRotationAttestationWithColorFlag.DocumentPath;
        //                    }

        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, true, AttestationDocumentTypes.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, true, AttestationDocumentTypes.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    if (!overrideAttestationReportWithForm)
        //                    {
        //                        InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, true, AttestationDocumentTypes.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    }

        //                    if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                    {

        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                         prevAttestationFormDocumentID, isAttestationFormMerged);
        //                        isEveryOneDocSaved = true;

        //                    }
        //                    else
        //                    {

        //                        InvitationDocument _doc_C = null;

        //                        if (overrideAttestationReportWithForm)
        //                            _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                        else
        //                            _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, true, AttestationDocumentTypes.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                        if (isDocMergingRequired)
        //                        {
        //                            singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                     , AttestationDocumentTypes.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                        }
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                             invitationDocId, isDocMergingRequired);
        //                        //UAT-2443:
        //                        if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                        {
        //                            UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                        }
        //                    }
        //                }

        //                if (!lstSRWithoutColorFlag.IsNullOrEmpty())
        //                {
        //                    invitationDocId = AppConsts.NONE;
        //                    isDocMergingRequired = false;
        //                    previousAttestationDocPath = String.Empty;
        //                    singleAttestationDocPathToUpdate = String.Empty;

        //                    InvitationSharedInfoDetails ScreeningRotationAttestationWithoutColorFlag = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                                    && (!con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus)) && !con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
        //                                                                                          && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                                                          && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                    if (!ScreeningRotationAttestationWithoutColorFlag.IsNullOrEmpty())
        //                    {
        //                        invitationDocId = ScreeningRotationAttestationWithoutColorFlag.InvitationDocumentID;
        //                        isDocMergingRequired = true;
        //                        previousAttestationDocPath = ScreeningRotationAttestationWithoutColorFlag.DocumentPath;
        //                    }

        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, false, AttestationDocumentTypes.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSRWithoutColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, false, AttestationDocumentTypes.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSRWithoutColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    if (!overrideAttestationReportWithForm)
        //                    {
        //                        InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, false, AttestationDocumentTypes.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSRWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    }

        //                    if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                    {

        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSRWithoutColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                         prevAttestationFormDocumentID, isAttestationFormMerged);
        //                        isEveryOneDocSaved = true;

        //                    }
        //                    else
        //                    {

        //                        InvitationDocument _doc_C = null;

        //                        if (overrideAttestationReportWithForm)
        //                            _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                        else
        //                            _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, false, AttestationDocumentTypes.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                        if (isDocMergingRequired)
        //                        {
        //                            singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                     , AttestationDocumentTypes.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                        }
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSRWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                             invitationDocId, isDocMergingRequired);
        //                        //UAT-2443:
        //                        if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                        {
        //                            UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                        }
        //                    }
        //                }

        //                #endregion
        //            }

        //            if (!_lstT.IsNullOrEmpty())
        //            {

        //                invitationDocId = AppConsts.NONE;
        //                isDocMergingRequired = false;
        //                previousAttestationDocPath = String.Empty;
        //                singleAttestationDocPathToUpdate = String.Empty;

        //                InvitationSharedInfoDetails TrackingAttestation = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode == null || con.LstBkgSharedInfoTypeCode.Count == 0)
        //                                         && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                         && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                if (!TrackingAttestation.IsNullOrEmpty())
        //                {
        //                    invitationDocId = TrackingAttestation.InvitationDocumentID;
        //                    isDocMergingRequired = true;
        //                    previousAttestationDocPath = TrackingAttestation.DocumentPath;
        //                }
        //                //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstT, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstT, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                if (!overrideAttestationReportWithForm)
        //                {
        //                    InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstT, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                }


        //                if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                {

        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstT, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                     prevAttestationFormDocumentID, isAttestationFormMerged);
        //                    isEveryOneDocSaved = true;

        //                }
        //                else
        //                {
        //                    InvitationDocument _doc_C = null;

        //                    if (overrideAttestationReportWithForm)
        //                        _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                    else
        //                        _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);


        //                    if (isDocMergingRequired)
        //                    {
        //                        singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                 , AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                    }
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstT, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                         invitationDocId, isDocMergingRequired);
        //                    //UAT-2443:
        //                    if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                    {
        //                        UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                    }
        //                }
        //            }

        //            if (!_lstR.IsNullOrEmpty())
        //            {
        //                invitationDocId = AppConsts.NONE;
        //                isDocMergingRequired = false;
        //                previousAttestationDocPath = String.Empty;
        //                singleAttestationDocPathToUpdate = String.Empty;

        //                InvitationSharedInfoDetails RotationAttestation = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode == null || con.LstBkgSharedInfoTypeCode.Count == 0)
        //                                                                        && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                                        && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                if (!RotationAttestation.IsNullOrEmpty())
        //                {
        //                    invitationDocId = RotationAttestation.InvitationDocumentID;
        //                    isDocMergingRequired = true;
        //                    previousAttestationDocPath = RotationAttestation.DocumentPath;
        //                }

        //                //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, false, true, false, AttestationDocumentTypes.ROTATION_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstR, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, false, true, false, AttestationDocumentTypes.ROTATION_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstR, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                if (!overrideAttestationReportWithForm)
        //                {
        //                    InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, false, true, false, AttestationDocumentTypes.ROTATION_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstR, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                }

        //                if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                {

        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstR, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                     prevAttestationFormDocumentID, isAttestationFormMerged);
        //                    isEveryOneDocSaved = true;

        //                }
        //                else
        //                {
        //                    InvitationDocument _doc_C = null;

        //                    if (overrideAttestationReportWithForm)
        //                        _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                    else
        //                        _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, false, true, false, AttestationDocumentTypes.ROTATION_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                    if (isDocMergingRequired)
        //                    {
        //                        singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                 , AttestationDocumentTypes.ROTATION_ONLY.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                    }
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstR, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                         invitationDocId, isDocMergingRequired);
        //                    //UAT-2443:
        //                    if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                    {
        //                        UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                    }
        //                }
        //            }

        //            if (!_lstS.IsNullOrEmpty())
        //            {
        //                #region Screening Only
        //                List<InvitationSharedInfoDetails> lstScreeningWithColorFlag = _lstS
        //                                                                              .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                  || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
        //                                                                              .ToList();

        //                // Includes the Tracking + Screening (without Flag)
        //                List<InvitationSharedInfoDetails> lstSWithoutColorFlag = _lstS
        //                                                                         .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                   && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
        //                                                                             .ToList();

        //                if (!lstScreeningWithColorFlag.IsNullOrEmpty())
        //                {
        //                    invitationDocId = AppConsts.NONE;
        //                    isDocMergingRequired = false;
        //                    previousAttestationDocPath = String.Empty;
        //                    singleAttestationDocPathToUpdate = String.Empty;

        //                    InvitationSharedInfoDetails ScreeningAttestationWithColorFlag = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
        //                                            && (con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus)) || con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
        //                                                                 && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                                 && !con.IsForEveryOneAttestationForm).FirstOrDefault();

        //                    if (!ScreeningAttestationWithColorFlag.IsNullOrEmpty())
        //                    {
        //                        invitationDocId = ScreeningAttestationWithColorFlag.InvitationDocumentID;
        //                        isDocMergingRequired = true;
        //                        previousAttestationDocPath = ScreeningAttestationWithColorFlag.DocumentPath;
        //                    }

        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);

        //                    if (!overrideAttestationReportWithForm)
        //                    {
        //                        InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    }

        //                    if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                    {

        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                         prevAttestationFormDocumentID, isAttestationFormMerged);
        //                        isEveryOneDocSaved = true;

        //                    }
        //                    else
        //                    {

        //                        InvitationDocument _doc_C = null;

        //                        if (overrideAttestationReportWithForm)
        //                            _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                        else
        //                            _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                        if (isDocMergingRequired)
        //                        {
        //                            singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                     , AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                        }
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                             invitationDocId, isDocMergingRequired);
        //                        //UAT-2443:
        //                        if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                        {
        //                            UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                        }
        //                    }

        //                }
        //                if (!lstSWithoutColorFlag.IsNullOrEmpty())
        //                {
        //                    invitationDocId = AppConsts.NONE;
        //                    isDocMergingRequired = false;
        //                    previousAttestationDocPath = String.Empty;
        //                    singleAttestationDocPathToUpdate = String.Empty;

        //                    InvitationSharedInfoDetails ScreeningAttestationWithoutColorFlag = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
        //                                                                                && (!con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                                && !con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
        //                                                                                && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                                                && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                    if (!ScreeningAttestationWithoutColorFlag.IsNullOrEmpty())
        //                    {
        //                        invitationDocId = ScreeningAttestationWithoutColorFlag.InvitationDocumentID;
        //                        isDocMergingRequired = true;
        //                        previousAttestationDocPath = ScreeningAttestationWithoutColorFlag.DocumentPath;
        //                    }

        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSWithoutColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSWithoutColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    if (!overrideAttestationReportWithForm)
        //                    {
        //                        InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    }

        //                    if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                    {

        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSWithoutColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                         prevAttestationFormDocumentID, isAttestationFormMerged);
        //                        isEveryOneDocSaved = true;

        //                    }
        //                    else
        //                    {

        //                        InvitationDocument _doc_C = null;

        //                        if (overrideAttestationReportWithForm)
        //                            _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                        else
        //                            _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                        if (isDocMergingRequired)
        //                        {
        //                            singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                     , AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                        }
        //                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                             invitationDocId, isDocMergingRequired);
        //                        //UAT-2443:
        //                        if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                        {
        //                            UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                        }
        //                    }
        //                }
        //                #endregion
        //            }

        //            if (!_lstNone.IsNullOrEmpty())
        //            {
        //                invitationDocId = AppConsts.NONE;
        //                isDocMergingRequired = false;
        //                previousAttestationDocPath = String.Empty;
        //                singleAttestationDocPathToUpdate = String.Empty;

        //                InvitationSharedInfoDetails NoneAttestation = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode == null || con.LstBkgSharedInfoTypeCode.Count == 0)
        //                                                                           && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
        //                                                                           && !con.IsForEveryOneAttestationForm).FirstOrDefault();
        //                if (!NoneAttestation.IsNullOrEmpty())
        //                {
        //                    invitationDocId = NoneAttestation.InvitationDocumentID;
        //                    isDocMergingRequired = true;
        //                    previousAttestationDocPath = NoneAttestation.DocumentPath;
        //                }

        //                //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstNone, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstNone, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                if (!overrideAttestationReportWithForm)
        //                {
        //                    InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstNone, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                }

        //                if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
        //                {

        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstNone, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID,
        //                                                     prevAttestationFormDocumentID, isAttestationFormMerged);
        //                    isEveryOneDocSaved = true;

        //                }
        //                else
        //                {
        //                    InvitationDocument _doc_C = null;

        //                    if (overrideAttestationReportWithForm)
        //                        _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
        //                    else
        //                        _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

        //                    if (isDocMergingRequired)
        //                    {
        //                        singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
        //                                                                                                 , AttestationDocumentTypes.NONE.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
        //                    }

        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstNone, lstInvitationDocumentMapping, _doc_C, currentUserID,
        //                                                         invitationDocId, isDocMergingRequired);
        //                    //UAT-2443:
        //                    if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
        //                    {
        //                        UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
        //                    }
        //                }
        //            }

        //            if (!isEveryOneDocSaved && overrideAttestationReportWithForm)
        //            {
        //                lstInvitationDocumentMapping.Add(new InvitationDocumentMapping()
        //                {
        //                    InvitationDocument = _doc_AttestationForm,
        //                    IDM_IsDeleted = false,
        //                    IDM_CreatedByID = currentUserID,
        //                    IDM_CreatedOn = DateTime.Now,
        //                    IDM_ProfileSharingInvitationGroupID = generatedInvitationGroupId,
        //                });
        //            }

        //            #endregion
        //        }
        //        else
        //        {

        //            #region   #region Generate the Lists for the types of Attestations, based on the permissions

        //            List<InvitationSharedInfoDetails> lstTrackingOnlyInvitations = lstInvitationSharedInfoDetails
        //                                                                                          .Where(cond => !cond.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
        //                                                                                             && (cond.LstBkgSharedInfoTypeCode == null || cond.LstBkgSharedInfoTypeCode.Count == 0))
        //                                                                                             .ToList();

        //            List<InvitationSharedInfoDetails> lstScreeningOnlyInvitations = lstInvitationSharedInfoDetails
        //                                                       .Where(cond => cond.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
        //                                                       && (cond.LstBkgSharedInfoTypeCode != null && cond.LstBkgSharedInfoTypeCode.Count > 0)).ToList();

        //            List<InvitationSharedInfoDetails> lstScreeningAndTrackingInvitations = lstInvitationSharedInfoDetails
        //                                         .Where(cond => !cond.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
        //                                         && (cond.LstBkgSharedInfoTypeCode != null && cond.LstBkgSharedInfoTypeCode.Count > 0)).ToList();

        //            List<InvitationSharedInfoDetails> lstInvitationsWithoutTrackingAndScreening = lstInvitationSharedInfoDetails
        //                                                  .Where(cond => cond.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
        //                                                  && (cond.LstBkgSharedInfoTypeCode == null || cond.LstBkgSharedInfoTypeCode.Count == 0)).ToList();



        //            #endregion

        //            #region Handle Normal Sharing scenarios

        //            if (!lstTrackingOnlyInvitations.IsNullOrEmpty())
        //            {
        //                //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstTrackingOnlyInvitations, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstTrackingOnlyInvitations, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstTrackingOnlyInvitations, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                InvitationDocument _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
        //                AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstTrackingOnlyInvitations, lstInvitationDocumentMapping, _doc_C, currentUserID);
        //            }
        //            if (!lstScreeningOnlyInvitations.IsNullOrEmpty())
        //            {
        //                List<InvitationSharedInfoDetails> lstScreeningOnlyInvitationsWithColorFlag = lstScreeningOnlyInvitations
        //                                                       .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                   || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
        //                                                             )
        //                                                       .ToList();

        //                List<InvitationSharedInfoDetails> lstScreeningOnlyInvitationsWithoutColorFlag = lstScreeningOnlyInvitations
        //                            .Where(cond =>
        //                                    !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                 && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
        //                            .ToList();

        //                if (!lstScreeningOnlyInvitationsWithColorFlag.IsNullOrEmpty())
        //                {
        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    InvitationDocument _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID);
        //                }
        //                if (!lstScreeningOnlyInvitationsWithoutColorFlag.IsNullOrEmpty())
        //                {
        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    InvitationDocument _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID);
        //                }
        //            }
        //            if (!lstScreeningAndTrackingInvitations.IsNullOrEmpty())
        //            {
        //                List<InvitationSharedInfoDetails> lstScreeningAndTrackingInvitationsWithColorFlag = lstScreeningAndTrackingInvitations
        //                                                               .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                                                         || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
        //                                                                     )
        //                                                               .ToList();

        //                List<InvitationSharedInfoDetails> lstScreeningAndTrackingInvitationsWithoutColorFlag = lstScreeningAndTrackingInvitations
        //                            .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
        //                                        && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
        //                            .ToList();

        //                if (!lstScreeningAndTrackingInvitationsWithColorFlag.IsNullOrEmpty())
        //                {
        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    InvitationDocument _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID);
        //                }
        //                if (!lstScreeningAndTrackingInvitationsWithoutColorFlag.IsNullOrEmpty())
        //                {
        //                    //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                    //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                    //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                    InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                    InvitationDocument _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
        //                    AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID);
        //                }
        //            }
        //            if (!lstInvitationsWithoutTrackingAndScreening.IsNullOrEmpty())
        //            {
        //                //InvitationDocument _doc_H = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.HORIZONTAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstInvitationsWithoutTrackingAndScreening, lstInvitationDocumentMapping, _doc_H, currentUserID);
        //                //InvitationDocument _doc_V = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL);
        //                //AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstInvitationsWithoutTrackingAndScreening, lstInvitationDocumentMapping, _doc_V, currentUserID);
        //                InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
        //                AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstInvitationsWithoutTrackingAndScreening, lstInvitationDocumentMapping, _doc_WithSign, currentUserID);
        //                InvitationDocument _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
        //                AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstInvitationsWithoutTrackingAndScreening, lstInvitationDocumentMapping, _doc_C, currentUserID);
        //            }

        //            #endregion
        //        }

        //        if (!lstInvitationDocumentMapping.IsNullOrEmpty())
        //        {
        //            ProfileSharingManager.SaveInvitationDocumentMapping(lstInvitationDocumentMapping);
        //        }
        //    }

        //    #endregion
        //} 
        #endregion


        //public static Int32 SaveAttestationDocument(Int32 invitationGroupID, Boolean includeTracking, Boolean includeScreening,
        //                                      Boolean includeRequirementRotation, Boolean includeColorFlag, String fileNameToAppend, Int32 tenantID, Int32 currentUserID)
        public static InvitationDocument SaveAttestationDocument(Int32 invitationGroupID, Boolean includeTracking, Boolean includeScreening,
                                              Boolean includeRequirementRotation, Boolean includeColorFlag, String fileNameToAppend,
            Int32 tenantID, Int32 currentUserID, AttestationReportType reportType, Int32 agencyID)
        {
            if (reportType != AttestationReportType.CONSOLIDATED_WITHOUT_SIGN) //Added in UAT-4190
            {
                ParameterValue[] parameters = new ParameterValue[7];

                parameters[0] = new ParameterValue();
                parameters[0].Name = "InvitationGroupID";
                parameters[0].Value = invitationGroupID.ToString();

                parameters[1] = new ParameterValue();
                parameters[1].Name = "IncludeTracking";
                parameters[1].Value = includeTracking.ToString();

                parameters[2] = new ParameterValue();
                parameters[2].Name = "IncludeScreening";
                parameters[2].Value = includeScreening.ToString();

                parameters[3] = new ParameterValue();
                parameters[3].Name = "TenantID";
                parameters[3].Value = tenantID.ToString();//"4";

                parameters[4] = new ParameterValue();
                parameters[4].Name = "IncludeColorFlag";
                parameters[4].Value = includeColorFlag.ToString();

                parameters[5] = new ParameterValue();
                parameters[5].Name = "IncludeRequirement";
                parameters[5].Value = includeRequirementRotation.ToString();

                parameters[6] = new ParameterValue();
                parameters[6].Name = "AgencyID";
                parameters[6].Value = agencyID.ToString();

                String reportName = ReportTypeEnum.ATTESTATION_REPORT.GetStringValue();
                if (reportType == AttestationReportType.VERTICAL)
                {
                    reportName = ReportTypeEnum.ATTESTATION_REPORT_VERTICAL.GetStringValue();
                }
                else if (reportType == AttestationReportType.CONSOLIDATED)
                {
                    reportName = ReportTypeEnum.ATTESTATION_REPORT_CONSOLIADTED.GetStringValue();
                }
                else if (reportType == AttestationReportType.CONSOLIDATED_WITHOUT_SIGN)
                {
                    reportName = ReportTypeEnum.COMBINED_ATTESTATION_REPORT_WITHOUT_SIGNATURE.GetStringValue();
                }
                String format = String.Empty;
                format = "pdf";
                //Commented in UAT-4190 :- Reports > Attestation Document Reports | Attestation document needs to match format and file type of the attestation documents accessible from Requirement Shares page
                //if (reportType == AttestationReportType.CONSOLIDATED_WITHOUT_SIGN)
                //{
                //    format = "EXCELOPENXML";
                //}
                //else
                //{
                //    format = "pdf";
                //}
                byte[] reportContent = null;
                try
                {
                    reportContent = ReportManager.GetReportByteArrayFormat(reportName, parameters, format);
                }
                catch (Exception)
                {
                    string strParameters = String.Empty;
                    try
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            strParameters = strParameters + " | " + parameters[i].Name + " = " + parameters[i].Value;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        throw new Exception("Error occured while executing report with Method Name : ReportManager.GetReportByteArrayFormat.  Reprotname={0}, parameters=" + strParameters);
                    }
                }
                //  byte[] reportContent = ReportManager.GetReportByteArrayFormat(reportName, parameters, format);

                StringBuilder fileName = new StringBuilder();
                if (reportType == AttestationReportType.VERTICAL)
                {
                    fileName.Append("VerticalAttestationDocument" + fileNameToAppend);
                }
                else if (reportType == AttestationReportType.HORIZONTAL)
                {
                    fileName.Append("HorizontalAttestationDocument" + fileNameToAppend);
                }
                else
                {
                    fileName.Append("ConsolidatedAttestationDocument" + fileNameToAppend);
                }
                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }

                FileStream _FileStream = null;
                try
                {
                    String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                    String applicantFileLocation = String.Empty;
                    String filename = String.Empty;

                    if (tempFilePath.IsNullOrEmpty())
                    {
                        //base.LogError("Please provide path for TemporaryFileLocation in config.", new SystemException());
                        throw new SystemException("Please provide path for TemporaryFileLocation in config.");
                    }
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += "Tenant(" + tenantID.ToString() + @")\";

                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);

                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"]; ///////to be asked
                        if (!applicantFileLocation.EndsWith("\\"))
                        {
                            applicantFileLocation += "\\";
                        }
                        applicantFileLocation += "Tenant(" + tenantID.ToString() + @")\";

                        if (!Directory.Exists(applicantFileLocation))
                        {
                            Directory.CreateDirectory(applicantFileLocation);
                        }
                    }

                    //UAT-1845 : Attestation Report Should be in the Pdf Format

                    String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                    String destFileName = String.Empty;

                    destFileName = fileName + "_" + tenantID.ToString() + "_" + currentUserID + "_" + date + ".pdf";
                    //Commented in UAT-4190 :- Reports > Attestation Document Reports | Attestation document needs to match format and file type of the attestation documents accessible from Requirement Shares page
                    //if (reportType == AttestationReportType.CONSOLIDATED_WITHOUT_SIGN)
                    //{
                    //    destFileName = fileName + "_" + tenantID.ToString() + "_" + currentUserID + "_" + date + ".xlsx";
                    //}
                    //else
                    //{
                    //    destFileName = fileName + "_" + tenantID.ToString() + "_" + currentUserID + "_" + date + ".pdf";
                    //}
                    String newTempFilePath = Path.Combine(tempFilePath, destFileName);
                    String newFinalFilePath = String.Empty;

                    // filename = fileName + ".pdf";
                    _FileStream = new FileStream(newTempFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    _FileStream.Write(reportContent, 0, reportContent.Length);
                    long length = new System.IO.FileInfo(newTempFilePath).Length;
                    Int32 filesize = 0;
                    bool result = Int32.TryParse(length.ToString(), out filesize);

                    _FileStream.Close();

                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        String docPathFileName = applicantFileLocation + destFileName;
                        File.Copy(newTempFilePath, docPathFileName);
                        newFinalFilePath = docPathFileName;

                    }
                    else
                    {
                        applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"];
                        if (!applicantFileLocation.EndsWith("//"))
                        {
                            applicantFileLocation += "//";
                        }
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = applicantFileLocation + "Tenant(" + tenantID.ToString() + @")/";
                        String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, destFileName, destFolder);
                        newFinalFilePath = returnFilePath; //Path.Combine(destFolder, destFileName);
                    }

                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);

                    String documentTypeCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT.GetStringValue();
                    if (reportType == AttestationReportType.VERTICAL)
                    {
                        documentTypeCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue();
                    }
                    else if (reportType == AttestationReportType.CONSOLIDATED)
                    {
                        documentTypeCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED.GetStringValue();
                    }
                    else if (reportType == AttestationReportType.CONSOLIDATED_WITHOUT_SIGN)
                    {
                        documentTypeCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();
                    }

                    //code to be save document
                    return ProfileSharingManager.GetInvitationDocumentObject(documentTypeCode, newFinalFilePath, currentUserID);
                }
                catch (Exception ex)
                {
                    //base.LogError(ex);
                    throw ex;
                }
                finally
                {
                    try { _FileStream.Close(); }
                    catch (Exception) { }
                }
            }
            return new InvitationDocument();
        }


        private static void AddToListOfInvitationDocumentMapping(Int32 invitationGrpID, List<InvitationSharedInfoDetails> lstInvitations,
                                                                  List<InvitationDocumentMapping> lstDocMapping, InvitationDocument doc, Int32 currentUserID, List<InvAttestationDocWithPermissionType> lstInvAttestationDocWithPermissionType
                                                                 , String agencyUserAttestationPermissionsCode, Int32 invitationDocId = 0, Boolean isDocMerged = false)
        {
            if (!doc.IsNull() && !doc.IND_DocumentFilePath.IsNullOrEmpty())
            {
                foreach (InvitationSharedInfoDetails invitation in lstInvitations)
                {
                    lstDocMapping.Add(new InvitationDocumentMapping()
                    {
                        InvitationDocument = !isDocMerged ? doc : null,
                        IDM_IsDeleted = false,
                        IDM_CreatedByID = currentUserID,
                        IDM_CreatedOn = DateTime.Now,
                        IDM_ProfileSharingInvitationGroupID = invitationGrpID,
                        IDM_ProfileSharingInvitationID = invitation.SharingInvitationID,
                        IDM_InvitationDocumentID = isDocMerged ? invitationDocId : AppConsts.NONE,
                    });
                }

                //UAT-3715 
                //added documnet without mapping which is used when an agncy user permission is changed

                Int32 agencyUserAttestationpermissionId = AppConsts.NONE;
                if (!agencyUserAttestationPermissionsCode.IsNullOrEmpty())
                    agencyUserAttestationpermissionId = GetAgencyUserAttestationpermissionIdByCode(agencyUserAttestationPermissionsCode);


                //lstInvAttestationDocWithPermissionType.Add(new InvAttestationDocWithPermissionType()
                //    {
                //        InvitationDocument = lstInvitations.IsNullOrEmpty() ? doc : null,
                //        IADWPT_ProfileSharingInvitationGroupID = invitationGrpID,
                //        IADWPT_InvitationDocumentID = isDocMerged ? invitationDocId : doc.IND_ID,
                //        IADWPT_AgencyUserAttestationPermissionId = agencyUserAttestationpermissionId,
                //        IADWPT_IsDeleted = false,
                //        IADWPT_CreatedOn = DateTime.Now,
                //        IADWPT_CreatedBy = currentUserID,
                //    });

                if (agencyUserAttestationpermissionId > AppConsts.NONE)
                {
                    InvAttestationDocWithPermissionType invAttestationDocWithPermissionType = new InvAttestationDocWithPermissionType();
                    invAttestationDocWithPermissionType.IADWPT_ProfileSharingInvitationGroupID = invitationGrpID;
                    if (!isDocMerged)
                    {
                        invAttestationDocWithPermissionType.InvitationDocument = doc;
                    }
                    else
                    {
                        invAttestationDocWithPermissionType.IADWPT_InvitationDocumentID = invitationDocId;
                    }
                    invAttestationDocWithPermissionType.IADWPT_AgencyUserAttestationPermissionId = agencyUserAttestationpermissionId;
                    invAttestationDocWithPermissionType.IADWPT_IsDeleted = false;
                    invAttestationDocWithPermissionType.IADWPT_CreatedOn = DateTime.Now;
                    invAttestationDocWithPermissionType.IADWPT_CreatedBy = currentUserID;
                    lstInvAttestationDocWithPermissionType.Add(invAttestationDocWithPermissionType);
                }
            }
        }

        public static OrganizationUserContract ConvertOrgUserIntoApplicantInfoContract(Entity.OrganizationUser applicantInfo, Int32 tenantID)
        {

            Boolean isApplicant = true;
            String phone = String.Empty;
            if (!applicantInfo.ExtraData.IsNullOrEmpty())
            {
                if (applicantInfo.ExtraData.ContainsKey("ClientContactPhone") &&
                    !applicantInfo.ExtraData.GetValue("ClientContactPhone").IsNullOrEmpty())
                {
                    phone = Convert.ToString(applicantInfo.ExtraData.GetValue("ClientContactPhone"));
                }
                if (applicantInfo.ExtraData.ContainsKey("IsClientContact") &&
                    !applicantInfo.ExtraData.GetValue("IsClientContact").IsNullOrEmpty())
                {
                    isApplicant = !Convert.ToBoolean(applicantInfo.ExtraData.GetValue("IsClientContact"));
                }
            }
            INTSOF.UI.Contract.ComplianceOperation.AddressContract _address = new INTSOF.UI.Contract.ComplianceOperation.AddressContract();

            if (isApplicant)
                _address = StoredProcedureManagers.GetAddressByAddressHandleId(applicantInfo.AddressHandleID.Value, tenantID);

            OrganizationUserContract orgUserContract = new OrganizationUserContract();
            orgUserContract.FirstName = applicantInfo.FirstName;
            orgUserContract.LastName = applicantInfo.LastName;
            orgUserContract.MiddleName = applicantInfo.MiddleName;
            orgUserContract.Email = applicantInfo.PrimaryEmailAddress;
            orgUserContract.SecondaryEmailAddress = applicantInfo.SecondaryEmailAddress;
            if (isApplicant)
            {
                orgUserContract.Address1 = _address.Address1;
                orgUserContract.Address2 = _address.Address2;
                orgUserContract.Country = _address.Country;
                orgUserContract.State = _address.StateName;
                orgUserContract.City = _address.CityName;
                orgUserContract.County = _address.CountyName;
                orgUserContract.Zipcode = _address.Zipcode;
                orgUserContract.Phone = applicantInfo.PhoneNumber;
            }
            else
            {
                orgUserContract.Phone = phone;
            }
            orgUserContract.Gender = applicantInfo.lkpGender.IsNullOrEmpty() ? null : applicantInfo.lkpGender.GenderName;
            orgUserContract.IsApplicant = isApplicant;//UAT-3977

            return orgUserContract;
        }

        /// <summary>
        /// GENERATE CONTRACT TO STORE THE PERMISION TYPES. TO BE USED IN GENERATION OF ATTESTATION REPORTS
        /// </summary>
        /// <param name="lstInvitationSharedInfoDetails"></param>
        /// <param name="clientContact"></param>
        /// <param name="_identifier"></param>
        public static void GenerateAttestationReportData(List<InvitationSharedInfoDetails> lstInvitationSharedInfoDetails, String complianceSharedInfoTypeCode,
                                                          String reqRotSharedInfoTypeCode, String bkgSharedInfoTypeCode, Guid? _identifier, Int32? invitationID)
        {
            lstInvitationSharedInfoDetails.Add(new InvitationSharedInfoDetails
            {
                InvitationIdentifier = _identifier.IsNullOrEmpty() ? new Guid() : _identifier.Value,
                ComplianceSharedInfoTypeCode = complianceSharedInfoTypeCode,
                LstBkgSharedInfoTypeCode = bkgSharedInfoTypeCode.IsNullOrEmpty() ? new List<String>() : bkgSharedInfoTypeCode.Split(',').ToList(),
                ReqRotSharedInfoTypeCode = reqRotSharedInfoTypeCode,
                SharingInvitationID = invitationID.IsNullOrEmpty() ? AppConsts.NONE : invitationID.Value
            });
        }

        /// <summary>
        /// UAT-1403: Method to generate Rotation Details HTML String.
        /// </summary>
        /// <param name="TenantID"></param>
        /// <param name="rotationID"></param>
        /// <returns></returns>
        public static string GenerateRotationDetailsHTML(ClinicalRotationDetailContract rotationDetailsContract)
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
            if (!rotationDetailsContract.UnitFloorLoc.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Unit/Floor or Location: </b>" + rotationDetailsContract.UnitFloorLoc + "</li>");
            }
            if (!rotationDetailsContract.RecommendedHours.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "# of Recommended Hours: </b>" + rotationDetailsContract.RecommendedHours + "</li>");
            }
            //UAT-4435
            if (!rotationDetailsContract.Students.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "# of Students: </b>" + rotationDetailsContract.Students + "</li>");
            }
            //UAT-4435
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

        private static void UpdateInvitationGroupSaveStatus(List<Int32> lstInvitationIds, Int32 currentUserId)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().UpdateInvitationGroupSaveStatus(lstInvitationIds, currentUserId);
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

        #region UAT 1320 Client admin expire profile shares
        public static bool SaveUpdateProfileExpirationCriteria(InvitationDetailsContract invitationDetailsContract, Dictionary<Int32, Boolean> lstSelectedInvitationIDs)
        {
            try
            {
                //Update ExpirationTypeId based on Code.
                invitationDetailsContract.ExpirationTypeId = GetExpirationTypes().Where(x => x.Code == invitationDetailsContract.ExpirationTypeCode).Select(x => x.ExpirationTypeID).FirstOrDefault();
                //Extract the list of InvitationIDs
                List<Int32> lstInvitationIDs = lstSelectedInvitationIDs.Where(cond => cond.Value == true).Select(c => c.Key).ToList();

                //Save and Updated
                return BALUtils.GetProfileSharingRepoInstance().SaveUpdateProfileExpirationCriteria(invitationDetailsContract, lstInvitationIDs);
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

        #region Shared User Dashboard Data

        /// <summary>
        /// Get the Shared User Dashboard Pie Chart related data
        /// </summary>
        /// <param name="inviteeOrgUserID"></param>
        /// <returns></returns>
        public static List<InstitutionProfileContract> GetSharedStudentsPerInstitution(Int32 inviteeOrgUserID, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetSharedStudentsPerInstitution(inviteeOrgUserID, fromDate, toDate);
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
        /// Get the Shared User Dashboard Calendar and Grid related data
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        public static List<DashBoardRotationDataContract> GetDashBoardRotationData(Guid userId, Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetDashBoardRotationData(userId, organizationUserId);
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
        /// Get the Shared User basic details to be displayed on dashboard.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static SharedUserDashboardDetailsContract GetSharedUserDashboardDetails(Guid userId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetSharedUserDashboardDetails(userId);
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
        /// Get the Shared User basic details to be displayed on dashboard.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static AgencyUserDashboardDetailsContract GetAgencyUserDashboardDetails(Guid userId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserDashboardDetails(userId);
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
        /// Get Agency User details
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static AgencyUser GetAgencyUserByUserID(String userID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserByUserID(userID);
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


        #region UAT 1530 WB: If sharing with an agency that does not have any users, client admin should have to fill out a form displaying the information of the person they would like to add.
        public static Boolean SaveSharedUserForReview(SharedUserReviewQueue sharedUserReviewQueue)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveSharedUserForReview(sharedUserReviewQueue);
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

        public static List<lkpSharedUserReviewStatu> GetSharedUserReviewStatusType()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserReviewStatu>().ToList();
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
        public static Tuple<List<SharedUserReiewQueueContract>, Int32> GetSharedUserReviewQueueData(SharedUserReiewQueueContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetSharedUserReviewQueueData(searchDataContract, gridCustomPaging);
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

        public static Boolean UpdateSharedUserReviewQueueStatus(List<Int32> queueRecordIds, Int32 currentUserId)
        {
            try
            {
                String code = SharedUserReviewStatus.REVIEWED.GetStringValue();
                Int32 reviewedStatusId = GetSharedUserReviewStatusType().Where(cond => cond.SURS_Code.ToLower() == code.ToLower() && !cond.SURS_IsDeleted)
                                                                        .Select(col => col.SURS_ID).FirstOrDefault();

                return BALUtils.GetProfileSharingRepoInstance().UpdateSharedUserReviewQueueStatus(queueRecordIds, currentUserId, reviewedStatusId);
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

        public static Boolean DeleteSharedUserReviewQueueRecord(Int32 SURQ_ID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().DeleteSharedUserReviewQueueRecord(SURQ_ID, currentUserId);
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

        public static Boolean SendInvitationEmailFromTemplate(Dictionary<String, String> dicContent, String subEventCode, Int32 loggedInUserID, String inviteeEmailAddress, string agencyName)
        {
            try
            {
                //Send mail to do
                List<String> subEventCodes = new List<String>();
                subEventCodes.Add(subEventCode.ToLower());
                Int32 subEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
                List<Entity.CommunicationTemplatePlaceHolder> placeHoldersToFetch = CommunicationManager.GetTemplatePlaceHolders(subEventID);
                Int32 templateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(subEventID);
                //Contains info for mail subject and content
                SystemEventTemplatesContract systemEventTemplate = TemplatesManager.GetTemplateDetails(templateId);

                Dictionary<String, String> dictMailData = new Dictionary<string, String>();

                #region Generate Place Holders

                if (subEventCode == CommunicationSubEvents.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue())
                    dictMailData.Add(EmailFieldConstants.AGENCY_NAME, agencyName);

                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SCHOOLNAME))
                {
                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, dicContent.GetValue(AppConsts.PSIEMAIL_SCHOOLNAME));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTNAME))
                {
                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, dicContent.GetValue(AppConsts.PSIEMAIL_RECIPIENTNAME));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_STUDENTNAME))
                {
                    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, dicContent.GetValue(AppConsts.PSIEMAIL_STUDENTNAME));
                }
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA))
                {
                    dictMailData.Add(EmailFieldConstants.SHARED_APPLICANT_DATA, dicContent.GetValue(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA));
                }
                //UAT-1403 : add rotation details to rotation invitation email
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_RotationDetails))
                {
                    dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, dicContent.GetValue(AppConsts.PSIEMAIL_RotationDetails));
                }
                //UAT-3964
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_CustomAttributes))
                {
                    dictMailData.Add(EmailFieldConstants.CUSTOMATTRIBUTES, dicContent.GetValue(AppConsts.PSIEMAIL_CustomAttributes));
                }
                //UAT-2519
                if (dicContent.ContainsKey(AppConsts.PSIEMAIL_PROFILEURL))
                {
                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, dicContent.GetValue(AppConsts.PSIEMAIL_PROFILEURL));
                }

                #endregion

                //a. Create entry in [Messaging] SystemCommunication table 
                //b. Create entry in [Messaging] SystemCommunicationDelivery table 
                Entity.SystemCommunication systemCommunication = new Entity.SystemCommunication();
                systemCommunication.SenderName = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME];
                systemCommunication.SenderEmailID = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];
                systemCommunication.Subject = systemEventTemplate.Subject;
                systemCommunication.CommunicationSubEventID = subEventID;
                systemCommunication.CreatedByID = loggedInUserID;
                systemCommunication.CreatedOn = DateTime.Now;
                systemCommunication.Content = systemEventTemplate.TemplateContent;
                //replace the placeholder
                foreach (var placeHolder in placeHoldersToFetch)
                {
                    Object obj = dictMailData.GetValue(placeHolder.Property);
                    systemCommunication.Content = systemCommunication.Content.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                    systemCommunication.Subject = systemCommunication.Subject.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
                }

                Entity.SystemCommunicationDelivery systemCommunicationDelivery = new Entity.SystemCommunicationDelivery();
                systemCommunicationDelivery.SystemCommunicationTypeID = systemCommunication.SystemCommunicationID;
                systemCommunicationDelivery.ReceiverOrganizationUserID = dicContent.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTID) && !String.IsNullOrEmpty(dicContent.GetValue(AppConsts.PSIEMAIL_RECIPIENTID)) ?
                                                                        Convert.ToInt32(dicContent.GetValue(AppConsts.PSIEMAIL_RECIPIENTID)) : AppConsts.NONE;
                systemCommunicationDelivery.RecieverEmailID = inviteeEmailAddress;
                systemCommunicationDelivery.RecieverName = dicContent.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTNAME) ? dicContent.GetValue(AppConsts.PSIEMAIL_RECIPIENTNAME) : String.Empty;
                systemCommunicationDelivery.IsDispatched = false;
                systemCommunicationDelivery.IsCC = null;
                systemCommunicationDelivery.IsBCC = null;
                systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
                systemCommunicationDelivery.CreatedOn = DateTime.Now;
                systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);

                List<Entity.SystemCommunication> lstSystemCommunicationToBeSaved = new List<Entity.SystemCommunication>();
                lstSystemCommunicationToBeSaved.Add(systemCommunication);
                return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
            }
            catch (SysXException ex)
            {
                String logError = "Getting Exception while sending the invitation mail to the following invitee: "
                    + "Invitee Email Address:" + inviteeEmailAddress + " "
                    + "Recipient Name:" + (dicContent.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTNAME) ? dicContent[AppConsts.PSIEMAIL_RECIPIENTNAME] : String.Empty) + " "
                    + "Student Name:" + (dicContent.ContainsKey(AppConsts.PSIEMAIL_STUDENTNAME) ? dicContent[AppConsts.PSIEMAIL_STUDENTNAME] : String.Empty) + " "
                    + "School Name:" + (dicContent.ContainsKey(AppConsts.PSIEMAIL_SCHOOLNAME) ? dicContent[AppConsts.PSIEMAIL_SCHOOLNAME] : String.Empty) + " "
                    + Environment.NewLine
                    + "Exception Details:";

                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + logError + Environment.NewLine + ex.Message
                    + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
            catch (Exception ex)
            {
                String logError = "Getting Exception while sending the invitation mail to the following invitee: "
                    + "Invitee Email Address:" + inviteeEmailAddress + " "
                    + "Recipient Name:" + (dicContent.ContainsKey(AppConsts.PSIEMAIL_RECIPIENTNAME) ? dicContent[AppConsts.PSIEMAIL_RECIPIENTNAME] : String.Empty) + " "
                    + "Student Name:" + (dicContent.ContainsKey(AppConsts.PSIEMAIL_STUDENTNAME) ? dicContent[AppConsts.PSIEMAIL_STUDENTNAME] : String.Empty) + " "
                    + "School Name:" + (dicContent.ContainsKey(AppConsts.PSIEMAIL_SCHOOLNAME) ? dicContent[AppConsts.PSIEMAIL_SCHOOLNAME] : String.Empty) + " "
                     + Environment.NewLine
                     + "Exception Details:";

                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + logError + Environment.NewLine + ex.Message
                    + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
        }

        #region Private Methods

        /// <summary>
        /// Return Rotation Review StatusID by Code
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="reviewStatusCode"></param>
        /// <returns></returns>
        private static Int32 GetRotationReviewStatusByCode(Int32 tenantId, String reviewStatusCode)
        {
            var _lkpReviewStatus = LookupManager.GetLookUpData<lkpSharedUserRotationReviewStatu>(tenantId).ToList();
            return _lkpReviewStatus.Where(surr => surr.SURRS_Code == reviewStatusCode && surr.SURRS_IsDeleted == false).First().SURRS_ID;
        }

        #endregion

        /// <summary>
        /// Get Attestation Report Text for Agency.
        /// </summary>
        /// <param name="agencyID"></param>
        /// <returns></returns>
        public static List<AgencyAttestationDetailContract> GetAttestationReportTextForAgency(String agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAttestationReportTextForAgency(agencyID);
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

        public static void SendConfirmationForInvitationSent(string userFullName, string emailId, int invitationIntiatedById, int tenantId, int InvitationGroupId, int currentUserID, List<Int32> sharedStudentIds, string agencyName, string rotationName, bool isRotationSharing, String rotationHierarchyIds = null, Int32? rotationID = AppConsts.NONE)
        {
            CommunicationSubEvents communicationSubEvent;
            //Create Dictionary
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, userFullName);
            dictMailData.Add(EmailFieldConstants.INVITATION_GROUP_ID, InvitationGroupId);
            dictMailData.Add(EmailFieldConstants.AGENCY_NAME, agencyName);

            if (isRotationSharing)
            {
                communicationSubEvent = CommunicationSubEvents.CONFIRMATION_FOR_ROTATION_INVITATION_SENT;
                dictMailData.Add(EmailFieldConstants.ROTATION_NAME, rotationName);
            }
            else
            {
                //string applicantNames = string.Empty;

                communicationSubEvent = CommunicationSubEvents.CONFIRMATION_FOR_INVITATION_SENT;

                //List<String> lstApplicants = ComplianceDataManager.GetOrganizationUsersByIds(sharedStudentIds.Select(cond => (Int32?)cond).ToList()).Select(cond => string.Concat(cond.FirstName, " ", cond.LastName)).ToList();

                //if (!lstApplicants.IsNullOrEmpty())
                //    applicantNames = string.Join(", ", lstApplicants);

                //dictMailData.Add(EmailFieldConstants.SHARED_STUDENT_NAMES, applicantNames);
            }

            //UAT-3475
            string applicantNames = string.Empty;

            List<String> lstApplicants = ComplianceDataManager.GetOrganizationUsersByIds(sharedStudentIds.Select(cond => (Int32?)cond).ToList()).Select(cond => string.Concat(cond.FirstName, " ", cond.LastName)).ToList();

            if (!lstApplicants.IsNullOrEmpty())
                applicantNames = string.Join(", ", lstApplicants);

            dictMailData.Add(EmailFieldConstants.SHARED_STUDENT_NAMES, applicantNames);

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = userFullName;
            mockData.EmailID = emailId;
            mockData.ReceiverOrganizationUserID = invitationIntiatedById;

            //Saving Confirmation Information into database
            ProfileSharingInvitationConfirmation profileSharingInvitationConfirmation = new ProfileSharingInvitationConfirmation();
            profileSharingInvitationConfirmation.PSIC_ProfileSharingInvitationGroupID = InvitationGroupId;
            profileSharingInvitationConfirmation.PSIC_IsViewed = false;
            profileSharingInvitationConfirmation.PSIC_IsSuccess = true;
            profileSharingInvitationConfirmation.PSIC_InvitationInitiatedByID = invitationIntiatedById;
            profileSharingInvitationConfirmation.PSIC_CreatedOn = DateTime.Now;
            profileSharingInvitationConfirmation.PSIC_CreatedByID = currentUserID;
            SaveInvitationConfirmation(profileSharingInvitationConfirmation);

            //Sending an confirmation email
            CommunicationManager.SendPackageNotificationMail(communicationSubEvent, dictMailData, mockData, tenantId, -1, null, null, true, false, null, rotationHierarchyIds, rotationID);
        }

        public static void SaveErrorInformationWhileInvitationSending(int currentUserID)
        {
            //Saving Confirmation Information into database
            ProfileSharingInvitationConfirmation profileSharingInvitationConfirmation = new ProfileSharingInvitationConfirmation();
            profileSharingInvitationConfirmation.PSIC_IsViewed = false;
            profileSharingInvitationConfirmation.PSIC_IsSuccess = false;
            profileSharingInvitationConfirmation.PSIC_InvitationInitiatedByID = currentUserID;
            profileSharingInvitationConfirmation.PSIC_CreatedOn = DateTime.Now;
            profileSharingInvitationConfirmation.PSIC_CreatedByID = currentUserID;
            SaveInvitationConfirmation(profileSharingInvitationConfirmation);
        }

        public static List<ProfileSharingInvitationGroupContract> GetSharedInvitationsData(SharedInvitationContract searchContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                Int32 adminInitializedInvitationStatus = GetInvitationStatusIdByCode(LkpInviationStatusTypes.ADMIN_INITLIAZED.GetStringValue());

                if (searchContract.IsNotNull())
                    searchContract.InvitationStatusID = adminInitializedInvitationStatus;

                return BALUtils.GetProfileSharingRepoInstance().GetSharedInvitationsData(searchContract.XML, gridCustomPaging.XML);
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

        public static string SaveInvitationConfirmation(ProfileSharingInvitationConfirmation profileSharingInvitationConfirmation)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveInvitationConfirmation(profileSharingInvitationConfirmation);
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

        public static bool MarkIsViewedByInvitationConfirmationId(Int32 profileSharingInvitationConfirmationId, int currentUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().MarkIsViewedByInvitationConfirmationId(profileSharingInvitationConfirmationId, currentUserId);
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

        public static List<ProfileSharingInvitationConfirmation> GetProfileSharingInvitationConfirmations(int currentUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetProfileSharingInvitationConfirmations(currentUserId);
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

        public static Boolean IsNeedToStartPolling(int currentUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsNeedToStartPolling(currentUserId);
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

        #region UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement.
        /// <summary>
        /// Get AgencyUserUserPermissionAccessTypeID from lookup table.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int GetAgencyUserPermissionAccessTypeID(String code)
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAgencyUserPermissionAccessType>().Where(x => x.AUPAT_Code == code && !x.AUPAT_IsDeleted).FirstOrDefault().AUPAT_ID;
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
        /// Get AgencyUserUserPermissionTypeID from lookup table.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int GetAgencyUserPermissionTypeID(string code)
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAgencyUserPermissionType>().Where(x => x.AUPT_Code == code && !x.AUPT_IsDeleted).FirstOrDefault().AUPT_ID;
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

        public static List<AgencyContract> GetAttestationReportTextForAgencyUser(Guid userID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAttestationReportTextForAgencyUser(userID);
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

        public static string UpdateAttestationReportTextForAgencyUser(Int32 loggedInUserID, Int32 agencyId, String attestationReportText)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().UpdateAttestationReportTextForAgencyUser(loggedInUserID, agencyId, attestationReportText);
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

        //FOR UAT-2463, Added bool allInvitationsToBeUpdated in the method
        public static bool SaveUpdateSharedUserInvitationReviewStatus(List<Int32> lstProfileSharingInvitationIds, Int32 currentLoggedInUserId, Int32 inviteeOrgUserId,
                                                                      Int32 selectedInvitationReviewStatusId, String agencyUserNotes = null, String invitationReviewStatusCode = null, bool allInvitationsToBeUpdated = false
                                                                      , bool needToChangeStatusAsPending = true, Int32 applicantId = 0, Int32 rotationId = 0, Int32 tenantID = 0, Boolean isAgencyScreens = false, Boolean isAdminLoggedIn = false)
        {
            try
            {
                #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
                if (selectedInvitationReviewStatusId.IsNullOrEmpty() || selectedInvitationReviewStatusId == AppConsts.NONE)
                {
                    selectedInvitationReviewStatusId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                    .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == invitationReviewStatusCode).First().SUIRS_ID;
                }
                #endregion

                #region UAT-2511
                List<lkpAuditChangeType> lstAuditChangeType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAuditChangeType>()
                                                                    .Where(cond => !cond.LACT_IsDeleted).ToList();
                #endregion
                return BALUtils.GetProfileSharingRepoInstance().SaveUpdateSharedUserInvitationReviewStatus(lstProfileSharingInvitationIds, currentLoggedInUserId
                                                                              , inviteeOrgUserId, selectedInvitationReviewStatusId, agencyUserNotes, allInvitationsToBeUpdated
                                                                              , needToChangeStatusAsPending, applicantId, rotationId, tenantID, lstAuditChangeType, isAgencyScreens, invitationReviewStatusCode, isAdminLoggedIn);
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

        #region UAT 1496 WB: Updates to Client admin profile expiration functionality.
        public static bool UpdateViewRemaining(List<Int32> lstSelectedInvitationIds, int loggedInUserID)
        {
            try
            {
                String expiredInvitationTypeCode = LkpInviationStatusTypes.EXPIRED.GetStringValue();
                Int32 expiredInvitationTypeId = LookupManager.GetSharedDBLookUpData<lkpInvitationStatu>().FirstOrDefault(cond => cond.Code == expiredInvitationTypeCode).InvitationStatusID;

                List<ProfileSharingInvitation> lstProfileSharingInvitation = BALUtils.GetProfileSharingRepoInstance().GetProfileSharingInvitationByIds(lstSelectedInvitationIds);
                foreach (var profileSharingInvitation in lstProfileSharingInvitation)
                {
                    if (profileSharingInvitation.lkpInvitationExpirationType.IsNotNull()
                        && profileSharingInvitation.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue()
                        && profileSharingInvitation.PSI_MaxViews > profileSharingInvitation.PSI_InviteeViewCount)
                    {
                        profileSharingInvitation.PSI_InviteeLastViewed = DateTime.Now;
                        profileSharingInvitation.PSI_InviteeViewCount = profileSharingInvitation.PSI_InviteeViewCount + AppConsts.ONE;
                        if (profileSharingInvitation.PSI_MaxViews == profileSharingInvitation.PSI_InviteeViewCount)
                            profileSharingInvitation.PSI_InvitationStatusID = expiredInvitationTypeId;
                        profileSharingInvitation.PSI_ModifiedOn = DateTime.Now;
                        profileSharingInvitation.PSI_ModifiedById = loggedInUserID;
                    }
                }
                return BALUtils.GetProfileSharingRepoInstance().UpdateViewRemaining();
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
        /// Method to Get All Agencies
        /// </summary>
        /// <returns></returns>
        public static List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract> GetAllAgencies(Int32 institutionID)
        {
            try
            {
                List<Agency> agencyList = BALUtils.GetProfileSharingRepoInstance().GetAgencies(institutionID, true, false, Guid.Empty);
                return agencyList.Select(col => new INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract
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

        #region UAT-1796 Enhance Client User Search to also display Agency Users and grid enhancements.
        public static AgencyUserContract GetAgencyUserDetailByID(Int32 tenantId, Int32 agencyUserId)
        {
            try
            {
                return AssignDataTableToContract(BALUtils.GetProfileSharingRepoInstance().GetAgencyUserDetailByID(agencyUserId));
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

        private static AgencyUserContract AssignDataTableToContract(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new AgencyUserContract
                {
                    AGU_ID = x["AgencyUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["AgencyUserID"]),
                    AGU_Name = x["AgencyUsername"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["AgencyUsername"]),
                    AGU_Email = x["Email"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["Email"]),
                    AGU_Phone = x["Phone"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["Phone"]),
                    AGU_UserID = x["UserID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["UserID"]),
                    AGU_RotationPackagePermission = x["RotationPackagePermission"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(x["RotationPackagePermission"]),
                    AGU_AgencyUserPermission = x["AgencyUserPermission"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(x["AgencyUserPermission"]),
                    AttestationRptPermission = x["AttestationRptPermissionID"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(x["AttestationRptPermissionID"]),
                    AGU_ComplianceSharedInfoTypeName = x["ComplianceInfo"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["ComplianceInfo"]),
                    AGU_ReqRotationSharedInfoTypeName = x["RotationInfo"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["RotationInfo"]),
                    AgnecyNames = x["AgnecyNames"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["AgnecyNames"]),
                    AgnecyIds = x["AgnecyIds"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["AgnecyIds"]),
                    AGU_BkgSharedInfoTypeName = x["BkgSharedInfo"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["BkgSharedInfo"]),
                    ProfileSharedInformation = x["ProfileSharedInformation"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["ProfileSharedInformation"]),
                    TenantNames = x["TenantNames"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["TenantNames"]),
                    //UAT-1993:Add the user name to the Agency User detail screen from the client user search.
                    UserName = x["UserName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["UserName"]),
                    IsInternationalPhone = x["IsInternationalPhone"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(x["IsInternationalPhone"])
                }).FirstOrDefault();
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

        #region UAT-1796, Enhance Client User Search to also display Agency Users and grid enhancements
        public static List<Agency> GetAgenciesByInstitionIDs(List<Int32> lstInstitutionIDs)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgenciesByInstitionIDs(lstInstitutionIDs);
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

        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
        /// <summary>
        /// Method to Get Agencies mapped with institute
        /// </summary>
        /// <returns></returns>
        public static List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract> GetInstitutionMappedAgency(List<Int32> institutionIDs, String userId
                                                                                            , Boolean isTabTypeInvitation, Int32 orgUserId)
        {
            try
            {
                List<Agency> lstTempAgency = new List<Agency>();
                List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract> finalAgencyList = new List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract>();
                lstTempAgency = BALUtils.GetProfileSharingRepoInstance().GetInstitutionMappedAgency(institutionIDs, userId);
                lstTempAgency.ForEach(agency =>
                {
                    finalAgencyList.Add(new INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract { AgencyID = agency.AG_ID, AgencyName = agency.AG_Name });
                });
                //This is scenario only for profile sharing invitation not for the rotations.
                if (isTabTypeInvitation)
                {
                    if (ProfileSharingManager.AnyApplicantSharingExist(orgUserId, institutionIDs))
                    {
                        finalAgencyList.Insert(0, new INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract { AgencyID = -1, AgencyName = "Individual Share" });
                    }
                }
                return finalAgencyList;
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
        /// Return true or false if applicant shared profile with user
        /// </summary>
        /// <returns></returns>
        public static Boolean AnyApplicantSharingExist(Int32 orgUserId, List<Int32> institutionIDs)
        {
            try
            {
                List<lkpInvitationSource> lstInvitationSource = LookupManager.GetSharedDBLookUpData<lkpInvitationSource>();
                Int32 invitationSourceId_Applicant = lstInvitationSource.FirstOrDefault(cond => cond.Code == InvitationSourceTypes.APPLICANT.GetStringValue()
                                                                       && !cond.IsDeleted).InvitationSourceID;
                return BALUtils.GetProfileSharingRepoInstance().AnyApplicantSharingExist(orgUserId, invitationSourceId_Applicant, institutionIDs);
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

        #region Code commented for UAT-2541
        ///// <summary>
        ///// UAT-1641 :- Notification mail is send to the Agency user when new agency is mapped to the Agency user
        ///// </summary>
        ///// <param name="_agencyUser"></param>
        ///// <param name="loggedInUserID"></param>
        ///// <param name="agencyUserID"></param>
        ///// <returns></returns>
        //public static Boolean SendUserAgencyMappingMail(AgencyUser _agencyUser, Int32 loggedInUserID, String VerificationCode)
        //{
        //    //Send mail to do
        //    List<String> subEventCodes = new List<String>();
        //    subEventCodes.Add(CommunicationSubEvents.USER_AGENCY_MAPPING.GetStringValue().ToLower());
        //    Int32 subEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
        //    List<Entity.CommunicationTemplatePlaceHolder> placeHoldersToFetch = CommunicationManager.GetTemplatePlaceHolders(subEventID);
        //    Int32 templateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(subEventID);
        //    List<String> lstUserAgencyName = _agencyUser.UserAgencyMappings.Where(cond => !cond.UAM_IsVerified && !cond.UAM_IsDeleted)
        //                                                                                               .Select(col => col.Agency.AG_Name)
        //                                                                                               .ToList();
        //    String agencyName = String.Empty;
        //    if (!lstUserAgencyName.IsNullOrEmpty())
        //    {
        //        agencyName = String.Join(",", lstUserAgencyName);
        //    }
        //    //Contains info for mail subject and content
        //    SystemEventTemplatesContract systemEventTemplate = TemplatesManager.GetTemplateDetails(templateId);

        //    String profileSharingURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
        //                                    ? String.Empty
        //                                    : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

        //    var queryString = new Dictionary<String, String>
        //                                                         {   {AppConsts.PROFILE_SHARING_URL_TYPE,AppConsts.PROFILE_SHARING_URL_TYPE_AGENCY_VERIFICATION}
        //                                                            ,{AppConsts.PROFILE_SHARING_URL_VERIFICATION_TOKEN,VerificationCode}
        //                                                           , {AppConsts.QUERY_STRING_AGENCY_USER_ID, _agencyUser.AGU_ID.ToString()}
        //                                                           ,{AppConsts.QUERY_STRING_USER_TYPE_CODE,OrganizationUserType.AgencyUser.GetStringValue()}
        //                                                         };

        //    var url = "http://" + String.Format(profileSharingURL + "?args={0}", queryString.ToEncryptedQueryString());

        //    Dictionary<String, String> dictMailData = new Dictionary<string, String>();
        //    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, _agencyUser.AGU_Name);
        //    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, url);
        //    dictMailData.Add(EmailFieldConstants.AGENCY_NAME, agencyName);

        //    //a. Create entry in [Messaging] SystemCommunication table 
        //    //b. Create entry in [Messaging] SystemCommunicationDelivery table 
        //    Entity.SystemCommunication systemCommunication = new Entity.SystemCommunication();
        //    systemCommunication.SenderName = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME];
        //    systemCommunication.SenderEmailID = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];
        //    systemCommunication.Subject = systemEventTemplate.Subject;
        //    systemCommunication.CommunicationSubEventID = subEventID;
        //    systemCommunication.CreatedByID = loggedInUserID;
        //    systemCommunication.CreatedOn = DateTime.Now;
        //    systemCommunication.Content = systemEventTemplate.TemplateContent;
        //    //replace the placeholder
        //    foreach (var placeHolder in placeHoldersToFetch)
        //    {
        //        Object obj = dictMailData.GetValue(placeHolder.Property);
        //        systemCommunication.Content = systemCommunication.Content.Replace(placeHolder.PlaceHolder, obj.IsNotNull() ? Convert.ToString(obj) : string.Empty);
        //    }

        //    Entity.SystemCommunicationDelivery systemCommunicationDelivery = new Entity.SystemCommunicationDelivery();
        //    systemCommunicationDelivery.SystemCommunicationTypeID = systemCommunication.SystemCommunicationID;
        //    systemCommunicationDelivery.ReceiverOrganizationUserID = _agencyUser.AGU_ID;
        //    systemCommunicationDelivery.RecieverEmailID = _agencyUser.AGU_Email;
        //    systemCommunicationDelivery.RecieverName = _agencyUser.AGU_Name;
        //    systemCommunicationDelivery.IsDispatched = false;
        //    systemCommunicationDelivery.IsCC = null;
        //    systemCommunicationDelivery.IsBCC = null;
        //    systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
        //    systemCommunicationDelivery.CreatedOn = DateTime.Now;
        //    systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);

        //    List<Entity.SystemCommunication> lstSystemCommunicationToBeSaved = new List<Entity.SystemCommunication>();
        //    lstSystemCommunicationToBeSaved.Add(systemCommunication);
        //    return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
        //}

        #endregion

        public static Boolean UpdateAgencyUserAgenciesVerificationCode(Int32 agencyUserID, String verificationCode, Entity.OrganizationUser orgUser)
        {
            return BALUtils.GetProfileSharingRepoInstance().UpdateAgencyUserAgenciesVerificationCode(agencyUserID, verificationCode, orgUser);
        }

        public static List<ShareHistoryDataContract> GetShareHistoryData(Int32 tenantId, ShareHistorySearchContract shareHistorySearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetShareHistoryData(shareHistorySearchContract, customPagingArgsContract);
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
        public static String GetRotationSharedReviewStatus(Int32 clinicalRotationId, Int32 currentLoggedInUserId, Int32 inviteeOrgUserId, Int32 tenantId, Int32 agencyID, ref Int32? lastReviewedByID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetRotationSharedReviewStatus(clinicalRotationId, currentLoggedInUserId, tenantId, agencyID, ref lastReviewedByID);
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

        public static List<ProfileSharingDataContract> GetProfileSharingDataByInvitationId(Int32 tenantId, Int32 psiId)
        {
            try
            {
                List<ProfileSharingDataContract> lstprofileSharingData = new List<ProfileSharingDataContract>();

                List<usp_GetProfileSharingDataByInvitationId_Result> psdList = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetProfileSharingDataByInvitationId(psiId);
                usp_GetProfileSharingDataByInvitationId_Result psd = psdList.FirstOrDefault();

                ProfileSharingDataContract currentContract = new ProfileSharingDataContract();
                currentContract.InvitationId = psd.InvitationID;
                currentContract.ApplicantUserID = psd.ApplicantUserID;
                currentContract.ApplicantName = psd.ApplicantName;
                currentContract.InviteeName = psd.InviteeName;
                currentContract.InvitationDate = psd.InvitationDate;
                currentContract.ViewedStatus = psd.ViewedStatus;
                currentContract.InvitationSentStatus = psd.InvitationSentStatus;
                currentContract.InviteeUserType = psd.InviteeUserType;
                currentContract.ScheduledInvitationStatus = psd.ScheduledInvitationStatus;
                currentContract.EffectiveDate = psd.EffectiveDate;
                currentContract.ExpirationDate = psd.ExpirationDate;
                currentContract.ViewsRemaining = psd.ViewsRemaining;
                //UAT: 1496
                currentContract.ExpirationType = psd.ExpirationDate.IsNullOrEmpty() ? (psd.ViewsRemaining.IsNullOrEmpty() ? String.Empty : "View Remaining") : "Expiration Date";

                currentContract.lstSharedPackages = new List<INTSOF.UI.Contract.ProfileSharing.SharedPackages>();

                //Add Compliance Package data
                List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> compliancePackageSharedList = AddSharedPackageData(psdList, SystemPackageTypes.COMPLIANCE_PKG.GetStringValue());
                currentContract.lstSharedPackages.AddRange(compliancePackageSharedList);

                //Add Compliance Package data
                List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> bkgPackageSharedList = AddSharedPackageData(psdList, SystemPackageTypes.BACKGROUND_PKG.GetStringValue());
                currentContract.lstSharedPackages.AddRange(bkgPackageSharedList);

                //Add Compliance Package data
                List<INTSOF.UI.Contract.ProfileSharing.SharedPackages> reqPackageSharedList = AddSharedPackageData(psdList, SystemPackageTypes.REQUIREMENT_ROT_PKG.GetStringValue());
                currentContract.lstSharedPackages.AddRange(reqPackageSharedList);

                lstprofileSharingData.Add(currentContract);

                return lstprofileSharingData;
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

        public static List<RequirementSharesDataContract> GetRequirementSharesData(String userId, Int32 currentLoggedInUserId, String tenantIds, ClinicalRotationDetailContract clinicalRotationSearchContract, InvitationSearchContract invitationSearchContract, CustomPagingArgsContract gridCustomPaging, String customAttributeXML)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetRequirementSharesData(userId, currentLoggedInUserId, tenantIds, clinicalRotationSearchContract, invitationSearchContract, gridCustomPaging, customAttributeXML);
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

        public static List<AgencyHierarchyMapping> GetInstituteHierarchyForSelectedAgency(Int32 agencyID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetInstituteHierarchyForSelectedAgency(agencyID);
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



        public static Boolean SaveAgencyHierarchyMapping(Int32 tenantID, INTSOF.UI.Contract.ProfileSharing.AgencyHierarchyContract agencyHierarchyContract, Int32 currentLoggedInUserID, Int32 agencyID, Int32 agencyInstitutionID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).SaveAgencyHierarchyMapping(agencyHierarchyContract, currentLoggedInUserID, agencyID, agencyInstitutionID);
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

        #region UAT-1881
        public static List<Agency> GetAllAgencyForOrgUser(Int32 tenantId, Int32 OrgUserId)
        {
            try
            {
                DataTable dt = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetAllAgencyForOrgUser(OrgUserId);
                IEnumerable<DataRow> rows = dt.AsEnumerable();
                return rows.Select(x => new Agency
                {
                    AG_ID = x["AG_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["AG_ID"]),
                    AG_Name = x["AG_Name"].GetType().Name == "DBNull" ? String.Empty : x["AG_Name"].ToString()

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


        public static Dictionary<String, String> SendProfileSharingInvitation(Dictionary<String, Object> conversionData, Int32 clientID, Boolean isApplicantSharing = false)
        {
            try
            {
                Dictionary<String, String> statusMessages = new Dictionary<String, String>();
                statusMessages.Add(StatusMessages.ERROR_MESSAGE.GetStringValue(), String.Empty);
                statusMessages.Add(StatusMessages.INFO_MESSAGE.GetStringValue(), String.Empty);
                statusMessages.Add(StatusMessages.SUCCESS_MESSAGE.GetStringValue(), String.Empty);

                dynamic currentUser = null;
                int selectedTenantID = 0;
                bool isNonScheduledInvitation = false;
                int currentUserId = 0;

                DateTime attestationDate;
                bool isRotationSharing;
                int rotationId;
                List<SharingPackageDataContract> lstSharedPkgData;
                Dictionary<int, bool> assignOrganizationUserIds;
                int selectedAgencyID;
                string currentAdminName;
                byte[] signature;
                Dictionary<Int32, String> dicAttestationReportText;
                string attestationReportText = string.Empty;
                bool isAdminLoggedIn;
                string selectedAgencyName;
                DateTime? invitationSchedlueDate;
                String institutionName;
                String centralLoginUrl;
                String profileSharingURL;
                String pxc_ExpireOption;
                String pxc_ExpirationTypeCode;
                Int32? pxc_MaxViews;
                DateTime? pxc_ExpirationDate;
                List<ComplianceInvitationData> compliancePkgDataList;
                List<BkgInvitationData> bkgPkgDataList;
                ClinicalRotationMemberDetail RotationDetail;
                Boolean isAgencyUserFound = false;
                Boolean sendNotificationToSchoolAdmin = false;
                Dictionary<int, string> dicSelfUploadedAttestationForms = new Dictionary<int, string>();

                conversionData.TryGetValue("CurrentUser", out currentUser);
                conversionData.TryGetValue("AttestationDate", out attestationDate);
                conversionData.TryGetValue("IsRotationSharing", out isRotationSharing);
                conversionData.TryGetValue("RotationId", out rotationId);
                conversionData.TryGetValue("SelectedTenantID", out selectedTenantID);
                conversionData.TryGetValue("LstSharedPkgData", out lstSharedPkgData);
                conversionData.TryGetValue("IsNonScheduledInvitation", out isNonScheduledInvitation);
                conversionData.TryGetValue("AssignOrganizationUserIds", out assignOrganizationUserIds);
                conversionData.TryGetValue("SelectedAgencyID", out selectedAgencyID);
                conversionData.TryGetValue("CurrentAdminName", out currentAdminName);
                conversionData.TryGetValue("Signature", out signature);
                conversionData.TryGetValue("AttestationReportText", out dicAttestationReportText);
                conversionData.TryGetValue("IsAdminLoggedIn", out isAdminLoggedIn);
                conversionData.TryGetValue("SelectedAgencyName", out selectedAgencyName);
                conversionData.TryGetValue("InvitationSchedlueDate", out invitationSchedlueDate);
                conversionData.TryGetValue("InstitutionName", out institutionName);
                conversionData.TryGetValue("InvitationSchedlueDate", out centralLoginUrl);
                conversionData.TryGetValue("ProfileSharingURL", out profileSharingURL);
                conversionData.TryGetValue("PXC_ExpireOption", out pxc_ExpireOption);
                conversionData.TryGetValue("PXC_ExpirationTypeCode", out pxc_ExpirationTypeCode);
                conversionData.TryGetValue("PXC_MaxViews", out pxc_MaxViews);
                conversionData.TryGetValue("PXC_ExpirationDate", out pxc_ExpirationDate);
                conversionData.TryGetValue("CompliancePkgDataList", out compliancePkgDataList);
                conversionData.TryGetValue("BkgPkgDataList", out bkgPkgDataList);
                conversionData.TryGetValue("RotationDetail", out RotationDetail);
                conversionData.TryGetValue("SendNotificationToSchoolAdmin", out sendNotificationToSchoolAdmin);

                if (isRotationSharing)
                    conversionData.TryGetValue("AttestationFormDocument", out dicSelfUploadedAttestationForms);

                currentUserId = currentUser.OrganizationUserId;

                var lstClientContacts = new List<ClientContactProfileSharingData>();
                //Int32 clientID = GetClientID();
                var lstApplicant = new List<Int32>();
                var rotationDetailsContract = new ClinicalRotationDetailContract();
                var lstSharedUserSnapshot = new List<SharedUserSubscriptionSnapshotContract>();
                var _lstPSIGroupTypes = LookupManager.GetSharedDBLookUpData<lkpProfileSharingInvitationGroupType>();
                var customAttributesForNotification = "";

                #region Rotation Sharing
                if (isRotationSharing)
                {
                    lstClientContacts = ClinicalRotationManager.GetRotationClientContacts(rotationId, clientID);

                    //UAT-3977
                    List<Int32> lstSharedStudentsOrgUserIds = new List<Int32>();
                    lstSharedStudentsOrgUserIds = assignOrganizationUserIds.Where(cond => cond.Value == true).Select(x => x.Key).ToList();

                    lstClientContacts = lstClientContacts.Where(con => con.OrgUserId != null && lstSharedStudentsOrgUserIds.Contains(con.OrgUserId.Value)).ToList();

                    rotationDetailsContract = ClinicalRotationManager.GetClinicalRotationById(clientID, rotationId, null);
                    customAttributesForNotification = ClinicalRotationManager.GetClinicalRotationNotificationCustomAttributes(clientID, rotationId);

                    /* UAT 1531: WB: Instructors assigned to rotations should be included in the attestation*/

                    if (isNonScheduledInvitation)
                    {
                        var _lstSharedUserTypes = LookupManager.GetLookUpData<lkpSharedUserType>(clientID).ToList();
                        var _instructorCode = OrganizationUserType.Instructor.GetStringValue();
                        var _preceptorCode = OrganizationUserType.Preceptor.GetStringValue();
                        var _instructorTypeId = _lstSharedUserTypes.Where(sut => sut.SUT_Code == _instructorCode).First().SUT_ID;
                        var _preceptorTypeId = _lstSharedUserTypes.Where(sut => sut.SUT_Code == _preceptorCode).First().SUT_ID;

                        // Generate Snapshot for only those who have registered
                        foreach (var clientContact in lstClientContacts.Where(cc => cc.OrgUserId != null && cc.ReqSubId != null && cc.ReqSubId != 0).ToList())
                        {
                            var snapshotId = ProfileSharingManager.SaveRequirementSnapshot(currentUserId, Convert.ToInt32(clientContact.ReqSubId), clientID);
                            lstSharedUserSnapshot.Add(new SharedUserSubscriptionSnapshotContract
                            {
                                SnapshotId = snapshotId,
                                RequirementSubscriptionId = Convert.ToInt32(clientContact.ReqSubId),
                                SharedUserId = clientContact.ClientContactID,
                                SharedUserTypeId = clientContact.ClientContactTypeCode == ClientContactType.Instructor.GetStringValue() ? _instructorTypeId : _preceptorTypeId
                            });
                        }
                    }
                }
                #endregion

                lstApplicant = assignOrganizationUserIds.Where(cond => cond.Value == true).Select(x => x.Key).ToList();

                var _lstMetaData = ProfileSharingManager.GetApplicantMetaData();

                List<ClinicalRotationAgencyContract> lstAgency = new List<ClinicalRotationAgencyContract>();

                if (isRotationSharing)
                {
                    lstAgency = ClinicalRotationManager.GetAgenciesMappedWithRotation(selectedTenantID, rotationId);
                }
                else
                {
                    lstAgency.Add(new ClinicalRotationAgencyContract
                    {
                        AgencyID = selectedAgencyID,
                        AgencyName = selectedAgencyName
                    });
                }

                foreach (ClinicalRotationAgencyContract rotationAgency in lstAgency)
                {
                    selectedAgencyID = rotationAgency.AgencyID;
                    selectedAgencyName = rotationAgency.AgencyName;

                    attestationReportText = dicAttestationReportText.Any() ? dicAttestationReportText[rotationAgency.AgencyID] : String.Empty;
                    //Set Agency Name 
                    rotationDetailsContract.AgencyName = selectedAgencyName;


                    List<usp_GetAgencyUserData_Result> lstAgencyUsers = ProfileSharingManager.GetAgencyUserData(clientID, selectedAgencyID);

                    // Generate new Invitation Group & Invitations
                    if (lstAgencyUsers.IsNotNull() && lstAgencyUsers.Count > 0)
                    {
                        isAgencyUserFound = true;

                        ProfileSharingInvitationGroup invitationGroup = new ProfileSharingInvitationGroup();
                        invitationGroup.PSIG_AgencyID = selectedAgencyID;
                        invitationGroup.PSIG_InvitationInitiatedByID = currentUserId;
                        invitationGroup.PSIG_IsDeleted = false;
                        invitationGroup.PSIG_CreatedByID = currentUserId;
                        invitationGroup.PSIG_CreatedOn = DateTime.Now;
                        invitationGroup.PSIG_TenantID = clientID;
                        invitationGroup.PSIG_AdminName = currentAdminName;
                        invitationGroup.PSIG_Signature = signature;
                        invitationGroup.PSIG_AttestationDate = attestationDate;

                        if (isRotationSharing)
                        {
                            String selfUploadedDocPath = string.Empty;
                            if (dicSelfUploadedAttestationForms.Keys.Contains(selectedAgencyID))
                                selfUploadedDocPath = dicSelfUploadedAttestationForms.GetValue(selectedAgencyID);

                            if (!selfUploadedDocPath.IsNullOrEmpty())
                            {
                                InvitationDocument invitationDocument = new InvitationDocument();
                                invitationDocument.IND_DocumentFilePath = selfUploadedDocPath;
                                invitationDocument.IND_DocumentType = GetDocumentTypeIdByCode(LKPSharedSystemDocumentTypes.UPLOADED_INVITATION_DOCUMENT.GetStringValue());
                                invitationDocument.IND_IsDeleted = false;
                                invitationDocument.IND_CreatedByID = currentUserId;
                                invitationDocument.IND_CreatedOn = DateTime.Now;

                                UploadedInvitationDocument uploadedInvitationDocument = new UploadedInvitationDocument();
                                uploadedInvitationDocument.InvitationDocument = invitationDocument;
                                uploadedInvitationDocument.UID_IsDeleted = false;
                                uploadedInvitationDocument.UID_CreatedBy = currentUserId;
                                uploadedInvitationDocument.UID_CreatedOn = DateTime.Now;

                                invitationGroup.UploadedInvitationDocuments.Add(uploadedInvitationDocument);
                            }
                        }

                        //UAT - 1486: Remove fields from Student attestation form and attestation spreadsheet for rotation and non-rotation shares
                        if (isRotationSharing)
                        {
                            invitationGroup.PSIG_AssignedUnits = rotationDetailsContract.UnitFloorLoc;
                            invitationGroup.PSIG_ClinicalFromDate = rotationDetailsContract.StartDate;
                            invitationGroup.PSIG_ClinicalToDate = rotationDetailsContract.EndDate;
                            invitationGroup.PSIG_ProgramName = rotationDetailsContract.Program;
                            invitationGroup.PSIG_SendNotificationToAdmin = sendNotificationToSchoolAdmin;
                        }
                        //UAT:1219: Display and make editable attestation text on the Manage Agency Sharing screen.
                        invitationGroup.PSIG_AttestationReportText = attestationReportText;

                        var _psiGroupTypeCode = String.Empty;
                        if (isRotationSharing)
                        {
                            _psiGroupTypeCode = ProfileSharingInvitationGroupTypes.ROTATION_SHARING_TYPE.GetStringValue();
                            invitationGroup.PSIG_ClinicalRotationID = rotationId;
                            invitationGroup.PSIG_ProfileSharingInvitationGroupTypeID = _lstPSIGroupTypes.Where(psigt => psigt.PSIGT_Code == _psiGroupTypeCode).First().PSIGT_ID;
                        }
                        else
                        {
                            _psiGroupTypeCode = ProfileSharingInvitationGroupTypes.PROFILE_SHARING_TYPE.GetStringValue();
                            invitationGroup.PSIG_ProfileSharingInvitationGroupTypeID = _lstPSIGroupTypes.Where(psigt => psigt.PSIGT_Code == ProfileSharingInvitationGroupTypes.PROFILE_SHARING_TYPE.GetStringValue()).First().PSIGT_ID;

                            #region Rotation Details
                            if (!RotationDetail.IsNullOrEmpty() && isApplicantSharing)
                            {
                                ProfileSharingInvitationRotationDetail invitationRotationDetail = new ProfileSharingInvitationRotationDetail();
                                invitationRotationDetail.PSIRD_RotationName = RotationDetail.RotationName;
                                invitationRotationDetail.PSIRD_TypeSpecialty = RotationDetail.TypeSpecialty;
                                invitationRotationDetail.PSIRD_Department = RotationDetail.Department;
                                invitationRotationDetail.PSIRD_Program = RotationDetail.Program;
                                invitationRotationDetail.PSIRD_Course = RotationDetail.Course;
                                invitationRotationDetail.PSIRD_Term = RotationDetail.Term;
                                invitationRotationDetail.PSIRD_UnitFloor = RotationDetail.UnitFloorLoc;
                                invitationRotationDetail.PSIRD_Shift = RotationDetail.Shift;
                                invitationRotationDetail.PSIRD_StartTime = RotationDetail.StartTime;
                                invitationRotationDetail.PSIRD_EndTime = RotationDetail.EndTime;
                                invitationRotationDetail.PSIRD_StartDate = RotationDetail.StartDate;
                                invitationRotationDetail.PSIRD_EndDate = RotationDetail.EndDate;
                                invitationRotationDetail.PSIRD_IsDeleted = false;
                                invitationRotationDetail.PSIRD_CreatedByID = currentUserId;
                                invitationRotationDetail.PSIRD_CreatedOn = DateTime.Now;
                                //UAT-3006 TO DO
                                invitationRotationDetail.PSIRD_SchoolContactName = RotationDetail.SchoolContactName;
                                invitationRotationDetail.PSIRD_SchoolContactEmailId = RotationDetail.SchoolContactEmailID;
                                //UAT-3662
                                invitationRotationDetail.PSIRD_InstructorPreceptor = RotationDetail.InstructorPreceptor;


                                List<Int32> daysToBeMapped = new List<Int32>();
                                if (!RotationDetail.DaysIdList.IsNullOrEmpty())
                                {
                                    daysToBeMapped = RotationDetail.DaysIdList.Split(',').Select(int.Parse).ToList();
                                    foreach (Int32 day in daysToBeMapped)
                                    {
                                        ProfileSharingInvitationRotationDay newDay = new ProfileSharingInvitationRotationDay();
                                        newDay.PSIRDY_WeekDayID = day;
                                        newDay.PSIRDY_IsDeleted = false;
                                        newDay.PSIRDY_CreatedByID = currentUserId;
                                        newDay.PSIRDY_CreatedOn = DateTime.Now;
                                        invitationRotationDetail.ProfileSharingInvitationRotationDays.Add(newDay);
                                    }
                                }
                                invitationGroup.ProfileSharingInvitationRotationDetails.Add(invitationRotationDetail);
                            }

                            #endregion
                        }

                        //UAT 1882
                        String invitationSourceCode = String.Empty;
                        if (isApplicantSharing)
                        {
                            invitationSourceCode = InvitationSourceTypes.APPLICANT.GetStringValue();
                        }
                        else
                        {
                            invitationSourceCode = isAdminLoggedIn ? InvitationSourceTypes.ADMIN.GetStringValue() : InvitationSourceTypes.CLIENTADMIN.GetStringValue();
                        }


                        List<InvitationSharedInfoDetails> lstInvitationSharedInfoDetails = new List<InvitationSharedInfoDetails>();

                        var _lstInvitationsContract = new List<InvitationDetailsContract>();
                        Dictionary<Int32, List<Int32>> dicAgencyUserMetaData = new Dictionary<Int32, List<Int32>>();
                        Dictionary<Int32, List<Int32>> dicClientContactMetaData = new Dictionary<Int32, List<Int32>>();

                        //UAT-3977
                        List<Int32> lstInstructorsIds = new List<Int32>();
                        lstInstructorsIds = lstClientContacts.Where(con => con.OrgUserId != null).Select(sel => sel.OrgUserId.Value).ToList();


                        List<Entity.OrganizationUser> lstApplicantInfo = SecurityManager.GetOrganizationUserByIds(lstApplicant);
                        //UAT-3977
                        if (!lstInstructorsIds.IsNullOrEmpty() && lstInstructorsIds.Count > AppConsts.NONE)
                        {
                            //get instructor data in extended properties of OrganizationUser
                            GetInstructorData(lstApplicantInfo, lstInstructorsIds);

                        }

                        foreach (Entity.OrganizationUser applicant in lstApplicantInfo)
                        {
                            //Entity.OrganizationUser applicantInfo = SecurityManager.GetOrganizationUser(applicantID);

                            String applicantName = applicant.FirstName + " " + applicant.LastName;

                            if (!isApplicantSharing)
                            {
                                List<ProfileSharingPackages> applicantSharingPackages = StoredProcedureManagers.GetSharingPackages(applicant.OrganizationUserID, clientID);

                                List<ProfileSharingPackages> compliancePackages = FilterSelectedComplianceBkgPkgs(SystemPackageTypes.COMPLIANCE_PKG.GetStringValue(), applicantSharingPackages, true, lstSharedPkgData);
                                List<ProfileSharingPackages> bkgPackages = FilterSelectedComplianceBkgPkgs(SystemPackageTypes.BACKGROUND_PKG.GetStringValue(), applicantSharingPackages, false, lstSharedPkgData);

                                //List<ProfileSharingPackages> compliancePackages = applicantSharingPackages.Where(cond => cond.IsCompliancePkg).ToList();
                                //List<ProfileSharingPackages> bkgPackages = applicantSharingPackages.Where(cond => !cond.IsCompliancePkg).ToList();

                                compliancePkgDataList = ProfileSharingManager.GetSharingComplianceData(clientID, compliancePackages, isNonScheduledInvitation, currentUserId);
                                bkgPkgDataList = ProfileSharingManager.GetSharingBkgPkgData(bkgPackages);
                            }
                            else
                            {
                                //UAT 1882
                                //Save SnapShot for compliance package If profile is shared by applicant, Only for Non-scheduled Invitation
                                if (isNonScheduledInvitation)
                                {
                                    foreach (ComplianceInvitationData cmpliancePkgData in compliancePkgDataList)
                                    {
                                        Int32 snapshotID = ProfileSharingManager.SaveImmunizationSnapshot(clientID, currentUserId, cmpliancePkgData.PkgSubId);
                                        cmpliancePkgData.SnapShotId = snapshotID;
                                    }

                                }
                            }
                            List<RequirementInvitationData> _lstRequirementDataList = new List<RequirementInvitationData>();

                            if (isRotationSharing)
                            {
                                List<ProfileSharingRequirementPackage> _lstRequirementPackages = StoredProcedureManagers.GetSharingRequirementPackages(applicant.OrganizationUserID, rotationId, clientID);
                                //var _filteredList = FilterRequirementPackages(SystemPackageTypes.REQUIREMENT_ROT_PKG.GetStringValue(), _lstRequirementPackages);
                                _lstRequirementDataList = ProfileSharingManager.GetSharingRequirementData(clientID, _lstRequirementPackages, isNonScheduledInvitation, currentUserId);
                            }

                            // If No package is available for sharing, for a user, then do not create any invitation.
                            if (!compliancePkgDataList.IsNullOrEmpty() || !bkgPkgDataList.IsNullOrEmpty() || !_lstRequirementDataList.IsNullOrEmpty())
                            {
                                Int32 agencyUserTypeID = ProfileSharingManager.GetUserTypeIdByCode(OrganizationUserType.AgencyUser.GetStringValue());

                                lstAgencyUsers.ForEach(agencyUser =>
                                {
                                    #region Generate Invitation Contract

                                    var _identifier = Guid.NewGuid();
                                    var currentInvitation = GenerateInvitationDetailsInstance(currentUserId, clientID, applicant.OrganizationUserID, _identifier, agencyUser.AgencyName,
                                                                                              agencyUser.AgencyUserID, agencyUser.NAME, agencyUser.Phone,
                                                                                              agencyUser.Email, agencyUser.ApplicationInvitationMetaDataID, agencyUserTypeID, OrganizationUserType.AgencyUser.GetStringValue()
                                                                                              , selectedAgencyID, pxc_ExpireOption, pxc_ExpirationTypeCode, pxc_MaxViews, pxc_ExpirationDate, isNonScheduledInvitation, invitationSchedlueDate);

                                    #endregion

                                    //ADD COMPLIANCE PACKAGE DATA INTO CONTRACT                    
                                    ProfileSharingManager.AddCompliancePackage(compliancePkgDataList, agencyUser.ComplianceSharedInfoTypeCode, currentInvitation);

                                    //ADD BACKGROUND PACKAGE DATA INTO CONTRACT
                                    ProfileSharingManager.AddBackgroundPackage(bkgPkgDataList, agencyUser.BkgSharedInfoTypeCode, currentInvitation);

                                    var _lstSharedMataDataIds = agencyUser.ApplicationInvitationMetaDataID.Split(',').Select(id => Int32.Parse(id)).ToList(); ;

                                    if (isRotationSharing)
                                    {
                                        currentInvitation.IsRotationType = true;

                                        if (lstInstructorsIds.Contains(applicant.OrganizationUserID))
                                            currentInvitation.PSI_IsInstructorShare = true;


                                        //Added this check for UAt-1381.
                                        if (_lstRequirementDataList.Count > AppConsts.NONE)
                                        {
                                            //ADD REQUIREMENT PACKAGE DATA INTO CONTRACT
                                            ProfileSharingManager.AddRequirementPackage(_lstRequirementDataList, agencyUser.RotationSharedInfoTypeCode, currentInvitation);
                                        }

                                        // UAT 1403 : Generate the Distinct list of Agency Users, in order to generate 
                                        // Single Email Html, for all students, ONLY If it is NON-Scheduling type
                                        if (!dicAgencyUserMetaData.ContainsKey(agencyUser.AgencyUserID.Value) && isNonScheduledInvitation)
                                        {
                                            dicAgencyUserMetaData.Add(Convert.ToInt32(agencyUser.AgencyUserID), _lstSharedMataDataIds);
                                        }
                                    }
                                    else
                                    {
                                        // Generate the Email HTML Per/Applicant & Per/Email, in case of normal Prfile Sharing, ONLY If it is NON-Scheduling type
                                        if (isNonScheduledInvitation)
                                        {
                                            //UAT-3006
                                            String schoolContactName = String.Empty;
                                            String schoolContactEmailID = String.Empty;
                                            //UAT-3662
                                            String instructorPreceptor = String.Empty;
                                            if (!isRotationSharing && isApplicantSharing && !RotationDetail.IsNullOrEmpty())
                                            {
                                                schoolContactName = RotationDetail.SchoolContactName.IsNullOrEmpty() ? String.Empty : RotationDetail.SchoolContactName;
                                                schoolContactEmailID = RotationDetail.SchoolContactEmailID.IsNullOrEmpty() ? String.Empty : RotationDetail.SchoolContactEmailID;
                                                instructorPreceptor = RotationDetail.InstructorPreceptor.IsNullOrEmpty() ? String.Empty : RotationDetail.InstructorPreceptor;
                                            }
                                            var applicantInfo = ProfileSharingManager.ConvertOrgUserIntoApplicantInfoContract(applicant, clientID);
                                            currentInvitation.TemplateData = GenerateMetaData(clientID, _lstMetaData, applicantInfo, agencyUser.NAME, _lstSharedMataDataIds, institutionName, centralLoginUrl, isApplicantSharing, schoolContactName, schoolContactEmailID, instructorPreceptor);
                                            currentInvitation.TemplateData.Add(AppConsts.PSIEMAIL_RECIPIENTID, agencyUser.AgencyUserID.ToString());
                                        }
                                    }

                                    // GENERATE CONTRACT TO STORE THE PERMISION TYPES. TO BE USED IN GENERATION OF ATTESTATION REPORTS, ONLY IF IT IS NON-SCHEDULING TYPE
                                    if (isNonScheduledInvitation)
                                    {
                                        ProfileSharingManager.GenerateAttestationReportData(lstInvitationSharedInfoDetails, agencyUser.ComplianceSharedInfoTypeCode, agencyUser.RotationSharedInfoTypeCode, agencyUser.BkgSharedInfoTypeCode, _identifier, null);
                                    }

                                    if (currentInvitation.TemplateData == null)
                                    {
                                        currentInvitation.TemplateData = new Dictionary<string, string>();
                                    }
                                    currentInvitation.TemplateData.Add(AppConsts.PSIEMAIL_CustomAttributes, customAttributesForNotification);
                                    _lstInvitationsContract.Add(currentInvitation);
                                });

                                if (isRotationSharing)
                                {
                                    #region ClientContact Sharing

                                    lstClientContacts.ForEach(clientContact =>
                                    {
                                        //UAT-1318 Restrict Invitation to be sent to the instructor if he is already an agency user.
                                        if (!(lstAgencyUsers.Any(cond => cond.Email.ToLower() == clientContact.Email.ToLower())))
                                        {
                                            Int32 userTypeID = AppConsts.NONE;
                                            if (clientContact.ClientContactTypeCode == ClientContactType.Instructor.GetStringValue())
                                            {
                                                userTypeID = ProfileSharingManager.GetUserTypeIdByCode(OrganizationUserType.Instructor.GetStringValue());
                                            }
                                            else
                                            {
                                                userTypeID = ProfileSharingManager.GetUserTypeIdByCode(OrganizationUserType.Preceptor.GetStringValue());
                                            }

                                            var _identifier = Guid.NewGuid();
                                            var _sharedMetaDataIds = String.Empty;

                                            // Client Contact default has full permissions. So use the Master List for the ID's
                                            foreach (var smd in _lstMetaData)
                                            {
                                                _sharedMetaDataIds += smd.AIMD_ID + ",";
                                            }

                                            _sharedMetaDataIds = _sharedMetaDataIds.Substring(0, _sharedMetaDataIds.Length - 1);

                                            // UAT 1403 : Generate the Distinct list of Client Contacts, in order to generate 
                                            // Single Email Html, for all students 
                                            if (!dicClientContactMetaData.ContainsKey(clientContact.ClientContactID) && isNonScheduledInvitation)
                                            {
                                                dicClientContactMetaData.Add(Convert.ToInt32(clientContact.ClientContactID), _lstMetaData.Select(x => x.AIMD_ID).ToList());
                                            }

                                            // Sending the Code of Any ClientContact type is enough as 
                                            // we have the check of AgencyUser while considering change in status of Rotation
                                            var currentInvitation = GenerateInvitationDetailsInstance(currentUserId, clientID, applicant.OrganizationUserID, _identifier, selectedAgencyName,
                                                                                                      null, clientContact.Name, clientContact.Phone,
                                                                                                      clientContact.Email, _sharedMetaDataIds, userTypeID, OrganizationUserType.Instructor.GetStringValue(), selectedAgencyID, pxc_ExpireOption, pxc_ExpirationTypeCode, pxc_MaxViews, pxc_ExpirationDate,
                                                                                                      isNonScheduledInvitation, invitationSchedlueDate, clientContact.ClientContactID);
                                            currentInvitation.IsRotationType = true;

                                            //ADD COMPLIANCE PACKAGE DATA INTO CONTRACT                    
                                            ProfileSharingManager.AddCompliancePackage(compliancePkgDataList, clientContact.ComplianceSharedInfoTypeCode, currentInvitation);

                                            //ADD BACKGROUND PACKAGE DATA INTO CONTRACT
                                            ProfileSharingManager.AddBackgroundPackage(bkgPkgDataList, clientContact.BkgSharedInfoTypeCode, currentInvitation);

                                            //ADD REQUIREMENT PACKAGE DATA INTO CONTRACT
                                            ProfileSharingManager.AddRequirementPackage(_lstRequirementDataList, clientContact.ReqRotSharedInfoTypeCode, currentInvitation);

                                            var _lstSharedMataDataIds = _sharedMetaDataIds.Split(',').Select(id => Int32.Parse(id)).ToList(); ;
                                            ////currentInvitation.TemplateData = GenerateMetaData(clientID, _lstMetaData, applicant, clientContact.Name, _lstSharedMataDataIds);

                                            // GENERATE CONTRACT TO STORE THE PERMISION TYPES. TO BE USED IN GENERATION OF ATTESTATION REPORTS
                                            if (isNonScheduledInvitation)
                                            {
                                                ProfileSharingManager.GenerateAttestationReportData(lstInvitationSharedInfoDetails, clientContact.ComplianceSharedInfoTypeCode,
                                                                              clientContact.ReqRotSharedInfoTypeCode, clientContact.BkgSharedInfoTypeCode, _identifier, null);
                                            }
                                            if (currentInvitation.TemplateData == null)
                                            {
                                                currentInvitation.TemplateData = new Dictionary<string, string>();
                                            }
                                            currentInvitation.TemplateData.Add(AppConsts.PSIEMAIL_CustomAttributes, customAttributesForNotification);
                                            _lstInvitationsContract.Add(currentInvitation);
                                        }
                                    });
                                    #endregion
                                }
                            }
                        }
                        #region UAT-3470
                        String ArchiveInvitationActiveCode = InvitationArchiveState.Active.GetStringValue();
                        Int32 InvitationArchiveStateID = LookupManager.GetSharedDBLookUpData<lkpInvitationArchiveState>().Where(invsrc => invsrc.LIAS_Code == ArchiveInvitationActiveCode).First().LIAS_ID;
                        _lstInvitationsContract.ForEach(d => d.InvitationArchiveStateID = InvitationArchiveStateID);

                        #endregion
                        // STEP 1 - Save All the Invitations in Security database.
                        var _lstInvitations = ProfileSharingManager.SaveAdminInvitations(_lstInvitationsContract, invitationGroup, invitationSourceCode, isNonScheduledInvitation);
                        //UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
                        List<Int32> distinctOrgUserId = _lstInvitations.DistinctBy(x => x.PSI_InviteeOrgUserID).Where(cond => cond.PSI_InviteeOrgUserID.HasValue).Select(x => x.PSI_InviteeOrgUserID.Value).ToList();

                        distinctOrgUserId.ForEach(orgId =>
                        {
                            List<Int32> invitIds = new List<Int32>();
                            invitIds = _lstInvitations.Where(cnd => cnd.PSI_InviteeOrgUserID == orgId).Select(slct => slct.PSI_ID).ToList();
                            ProfileSharingManager.SaveUpdateSharedUserInvitationReviewStatus(invitIds, currentUserId, orgId, AppConsts.NONE, String.Empty, SharedUserInvitationReviewStatus.PENDING_REVIEW.GetStringValue());
                        });



                        if (isNonScheduledInvitation)
                        {
                            // STEP 2 - Save All the Invitations in Tenant database.
                            ProfileSharingManager.SaveAdminInvitationDetails(_lstInvitationsContract, _lstInvitations, lstSharedUserSnapshot, rotationId, clientID, selectedAgencyID);

                            // STEP 3 - Update the ProfileSharingInvitationID in the Contract used to generate the Attestation Reports
                            UpdateInvitationSharedInfoDetailsContract(_lstInvitations, lstInvitationSharedInfoDetails);

                            // STEP 4 - Generate the Attestation Report
                            //TO-DO Method to Convert Profile Sharing Invitation into InvitationDetails Contract
                            //ProfileSharingManager.ConvertProfileSharingInvitationEntityIntoContract(_lstInvitations);
                            ProfileSharingManager.GenerateAttestationReport(lstInvitationSharedInfoDetails, _lstInvitations.First().ProfileSharingInvitationGroup.PSIG_ID, isRotationSharing, clientID, currentUserId, selectedAgencyID, rotationId);

                            if (!isRotationSharing)
                            {
                                // STEP 5 - Update the Link to be sent with Token, in the Email tamplate, based on the ProfileSharingInvitationID generated.
                                //UAT-2519
                                UpdateInvitationDetailsContract(_lstInvitations, _lstInvitationsContract, profileSharingURL);
                            }
                            //UAT-2544: Approved Rotation Student Sharing Functionality
                            if (isRotationSharing)
                            {
                                List<Int32> distinctApplicantUserId = _lstInvitations.DistinctBy(x => x.PSI_ApplicantOrgUserID).Where(x => x.PSI_InviteeOrgUserID.HasValue).Select(x => x.PSI_ApplicantOrgUserID).ToList();

                                distinctApplicantUserId.ForEach(AppOrgId =>
                                {
                                    List<Int32> invitIds = new List<Int32>();
                                    var invitationData = _lstInvitations.Where(cnd => cnd.PSI_ApplicantOrgUserID == AppOrgId).ToList();
                                    if (!invitationData.IsNullOrEmpty())
                                    {
                                        //Int32 organizationUserID = invitationData.FirstOrDefault().PSI_ApplicantOrgUserID;
                                        invitIds = invitationData.Select(slct => slct.PSI_ID).ToList();
                                        //UAT-2544
                                        Boolean needToChangeStatusAsPending = true;
                                        needToChangeStatusAsPending = ClinicalRotationManager.NeedToChangeInvitationStatusAsPending(clientID, rotationId, invitIds, AppOrgId, currentUserId);
                                        if (!needToChangeStatusAsPending && !invitIds.IsNullOrEmpty())
                                        {
                                            ProfileSharingManager.SaveUpdateSharedUserInvitationReviewStatus(invitIds, currentUserId, AppOrgId, AppConsts.NONE, String.Empty, SharedUserInvitationReviewStatus.PENDING_REVIEW.GetStringValue(), false, needToChangeStatusAsPending, AppOrgId, rotationId, clientID, false, true);

                                            //if (!lstInstructorsIds.Contains(AppOrgId))
                                            //{
                                            //Update Rotation Status
                                            List<Int32> lstInviteeUserIds = invitationData.DistinctBy(x => x.PSI_InviteeOrgUserID).Where(x => x.PSI_InviteeOrgUserID.HasValue && x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.AgencyUser.GetStringValue()).Select(x => x.PSI_InviteeOrgUserID.Value).ToList();
                                            lstInviteeUserIds.ForEach(inviteeUserID =>
                                            {
                                                String reviewStatusUpdateCode = String.Empty;
                                                Int32? lastReviewedByID = null;
                                                reviewStatusUpdateCode = ProfileSharingManager.GetRotationSharedReviewStatus(rotationId, inviteeUserID, inviteeUserID, clientID, selectedAgencyID, ref lastReviewedByID);
                                                ClinicalRotationManager.SaveUpdateUserRotationReviewStatus(clientID, rotationId, currentUserId, inviteeUserID, reviewStatusUpdateCode, selectedAgencyID, lastReviewedByID, true);
                                            });
                                            //}
                                        }
                                    }
                                });

                            }

                            // STEP 6 - Loop to Send Invitation Mail for each Invitation.
                            #region SEND INVITATION EMAIL

                            //var _lstInvitationsContractNew = _lstInvitationsContract;

                            var _lstInvitationsContractResult = new List<InvitationDetailsContract>();

                            List<Int32> lstSharedStudentIds = new List<Int32>();
                            string agencyName = string.Empty;

                            //Getting list of shared applicants whom profile shared to shared users
                            lstSharedStudentIds = _lstInvitationsContract.Select(cond => cond.ApplicantId).Distinct().ToList();

                            if (!_lstInvitationsContract.IsNullOrEmpty())
                                agencyName = _lstInvitationsContract.First().Agency;

                            if (isRotationSharing)
                            {
                                //UAT-3977
                                // lstApplicantInfo = lstApplicantInfo.Where(con => !lstInstructorsIds.Contains(con.OrganizationUserID)).ToList();

                                //Generating Email Content for Agency Users 
                                GeneratEmailContentForAgencyUsers(clientID, _lstMetaData, lstAgencyUsers, _lstInvitationsContract, dicAgencyUserMetaData, lstApplicantInfo, rotationDetailsContract, centralLoginUrl, institutionName, rotationId);

                                //Generating Email Content for Client Contacts
                                GeneratEmailContentForClientContacts(clientID, _lstMetaData, lstClientContacts, _lstInvitationsContract, dicClientContactMetaData, lstApplicantInfo, rotationDetailsContract, centralLoginUrl, institutionName);

                                //Updating the Link to be sent with Token, in the Email tamplate, based on the ProfileSharingInvitationID generated. 
                                //UAT-2519
                                UpdateInvitationDetailsContract(_lstInvitations, _lstInvitationsContract, profileSharingURL);

                                //Get the Distinct EmailId's to send Single email to a shared user, for all Applicants
                                _lstInvitationsContractResult = _lstInvitationsContract.DistinctBy(x => x.EmailAddress).ToList();

                                #region UAT-2803: Enhance the agency user settings so that we can individually choose what notifications an agency user receives
                                List<Int32> lstAgencyUsersList = new List<Int32>();
                                lstAgencyUsersList = _lstInvitationsContractResult.Where(x => x.AgencyUserId != null).Select(sel => sel.AgencyUserId.Value).ToList();
                                lstAgencyUsersList = lstAgencyUsersList.Distinct().ToList();
                                // returns the list of agency users for which notification permission is true and also return the agency users whose permissions are not present in mapping table.
                                List<Int32> lstAgencyUsersHavingNotifPerm = ProfileSharingManager.GetAgencyUserNotificationPermission(lstAgencyUsersList, AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue());
                                _lstInvitationsContractResult = _lstInvitationsContractResult.Where(con => con.AgencyUserId != null && lstAgencyUsersHavingNotifPerm.Contains(con.AgencyUserId.Value)).ToList();
                                #endregion



                                //Sending Emails to shared users
                                foreach (var invitationContract in _lstInvitationsContractResult)
                                {
                                    //invitationContract.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmail(View.ProfileSharingURL, invitationContract.EmailAddress,
                                    //                                          String.Empty, String.Empty, false,
                                    //                                          invitationContract.TemplateData, true, clientID, true);
                                    invitationContract.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmailFromTemplate(invitationContract.TemplateData
                                                                , CommunicationSubEvents.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue()
                                                                , currentUserId, invitationContract.EmailAddress, agencyName);

                                    // Update the "IsEmailSentSuccessfully" flag for All the invitations for the current EmailAddress
                                    _lstInvitationsContract.Where(s => s.EmailAddress == invitationContract.EmailAddress).ForEach(invCont =>
                                    {
                                        invCont.IsEmailSentSuccessfully = invitationContract.IsEmailSentSuccessfully;
                                    });
                                }
                                #region UAT-3254
                                String RotationHierarchyIds = String.Empty;
                                if (!rotationDetailsContract.IsNullOrEmpty() && rotationDetailsContract.RotationID > AppConsts.NONE)
                                {
                                    RotationHierarchyIds = BALUtils.GetProfileSharingClientRepoInstance(clientID).GetRotationHierarchyIdsByRotationID(rotationDetailsContract.RotationID);
                                }
                                #endregion
                                //Send Confirmation Email for Invitation Sent
                                ProfileSharingManager.SendConfirmationForInvitationSent(string.Concat(currentUser.FirstName, " ", currentUser.LastName), currentUser.Email, currentUser.OrganizationUserId, selectedTenantID, invitationGroup.PSIG_ID, currentUser.OrganizationUserId, lstSharedStudentIds, agencyName, rotationDetailsContract.RotationName, true, RotationHierarchyIds, rotationDetailsContract.RotationID);
                            }
                            else
                            {
                                #region UAT-2803 : Enhance the agency user settings so that we can individually choose what notifications an agency user receives
                                List<Int32> lstAgencyUsersList = new List<Int32>();
                                lstAgencyUsersList = _lstInvitationsContract.Where(x => x.AgencyUserId != null).Select(sel => sel.AgencyUserId.Value).ToList();
                                lstAgencyUsersList = lstAgencyUsersList.Distinct().ToList();
                                // returns the list of agency users for which notification permission is true and also return the agency users whose permissions are not present in mapping table.
                                List<Int32> lstAgencyUsersHavingNotifPerm = ProfileSharingManager.GetAgencyUserNotificationPermission(lstAgencyUsersList, AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_NON_ROTATION.GetStringValue());
                                _lstInvitationsContract = _lstInvitationsContract.Where(con => con.AgencyUserId != null && lstAgencyUsersHavingNotifPerm.Contains(con.AgencyUserId.Value)).ToList();
                                #endregion

                                //_lstInvitationsContractResult = _lstInvitationsContract.ToList();
                                foreach (var invitationContract in _lstInvitationsContract)
                                {
                                    //invitationContract.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmail(View.ProfileSharingURL, invitationContract.EmailAddress,
                                    //                                          String.Empty, String.Empty, false,
                                    //                                          invitationContract.TemplateData, true, clientID);
                                    invitationContract.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmailFromTemplate(invitationContract.TemplateData
                                                                , CommunicationSubEvents.REQUIREMENTS_SHARING_INVITATION_NON_ROTATION.GetStringValue()
                                                                , currentUserId, invitationContract.EmailAddress, agencyName);
                                }

                                if (!isApplicantSharing)
                                {
                                    //Send Confirmation Email for Invitation Sent
                                    ProfileSharingManager.SendConfirmationForInvitationSent(string.Concat(currentUser.FirstName, " ", currentUser.LastName), currentUser.Email, currentUser.OrganizationUserId, selectedTenantID, invitationGroup.PSIG_ID, currentUser.OrganizationUserId, lstSharedStudentIds, agencyName, string.Empty, false);
                                }
                            }
                            #endregion

                            // STEP 7 - Update Invitation Status to 'NEW' for each invitation, for which Email has been sent successfully
                            var _lstInvitationIDs = _lstInvitationsContract.Where(cond => cond.IsEmailSentSuccessfully).Select(col => col.PSIId).ToList();
                            ProfileSharingManager.UpdateBulkInvitationStatus(LkpInviationStatusTypes.NEW.GetStringValue(), _lstInvitationIDs, currentUserId);

                            if (_lstInvitationsContract.All(cond => !cond.IsEmailSentSuccessfully))
                            {
                                if (!isNonScheduledInvitation)
                                {
                                    // View.ErrorMessage = "Some error occurred while sending invitations to shared users. Please try again Or contact System Administrator.";
                                }
                            }
                            else if (_lstInvitationsContract.Any(cond => !cond.IsEmailSentSuccessfully))
                            {
                                String agencyUserEmails = String.Join(",", _lstInvitationsContract.Where(cond => !cond.IsEmailSentSuccessfully)
                                                                                 .Select(col => col.EmailAddress).ToList());

                                if (!isNonScheduledInvitation)
                                {
                                    statusMessages[StatusMessages.ERROR_MESSAGE.GetStringValue()] = "Some error occurred while sending invitations to below shared users." +
                                                             "Shared Users:" + agencyUserEmails +
                                                             ". Please try again Or contact System Administrator.";
                                }
                            }
                            else
                            {
                                statusMessages[StatusMessages.SUCCESS_MESSAGE.GetStringValue()] = "Invitation(s) have been sent successfully to all Shared User(s) for selected Applicant(s)/Instructor(s).";
                                statusMessages[StatusMessages.ERROR_MESSAGE.GetStringValue()] = String.Empty;
                            }
                        }
                        else
                        {
                            var _lstExcluded = lstSharedPkgData.Where(pkg => pkg.IsCompletelyExcluded == true || pkg.IsPartiallyExcluded == true).ToList();

                            _lstExcluded.ForEach(ep =>
                            {
                                ep.PSIGroupId = invitationGroup.PSIG_ID;
                            });

                            ProfileSharingManager.SaveScheduledExcludedPackageData(_lstExcluded, currentUserId, clientID);
                            statusMessages[StatusMessages.SUCCESS_MESSAGE.GetStringValue()] = "Invitation(s) have been scheduled successfully for the selected date.";
                            statusMessages[StatusMessages.ERROR_MESSAGE.GetStringValue()] = String.Empty;
                        }

                        //UAT-2452
                        lstAgencyUsers.ForEach(agencyUser1 =>
                        {
                            Int32? ProfileShareInvitationGroupID = _lstInvitations.Where(cond => cond.PSI_AgencyUserID == agencyUser1.AgencyUserID).Select(sel => sel.PSI_ProfileSharingInvitationGroupID).Distinct().FirstOrDefault();
                            ProfileSharingManager.AddAgencyUserSharedProfilePermisssionData(agencyUser1.ComplianceSharedInfoTypeCode, agencyUser1.RotationSharedInfoTypeCode, agencyUser1.BkgSharedInfoTypeCode, agencyUser1.AgencyUserID, ProfileShareInvitationGroupID, currentUserId);

                        });
                    }
                }

                if (!isAgencyUserFound)
                {
                    statusMessages[StatusMessages.INFO_MESSAGE.GetStringValue()] = "No Agency User exists for the selected agency.";
                    statusMessages[StatusMessages.SUCCESS_MESSAGE.GetStringValue()] = String.Empty;
                    statusMessages[StatusMessages.ERROR_MESSAGE.GetStringValue()] = String.Empty;
                }

                return statusMessages;
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

        #region PROFILE SHARING METHODS

        private static void GeneratEmailContentForClientContacts(int clientID, List<ApplicantInvitationMetaData> _lstMetaData, List<ClientContactProfileSharingData> lstClientContacts, List<InvitationDetailsContract> _lstInvitationsContract,
                        Dictionary<int, List<int>> dicClientContactMetaData, List<Entity.OrganizationUser> lstApplicantInfo, ClinicalRotationDetailContract rotationDetailsContract, string centralLoginUrl, string institutionName)
        {
            //UAT-1403 : Add Rotation Details to Rotation Sharing Invitation Email
            var rotationDetailsHTML = ProfileSharingManager.GenerateRotationDetailsHTML(rotationDetailsContract);

            foreach (var clientContact in lstClientContacts)
            {
                if (dicClientContactMetaData.ContainsKey(clientContact.ClientContactID))
                {
                    var _clientContactMetaDataIds = dicClientContactMetaData.Where(k => k.Key == clientContact.ClientContactID).First().Value;

                    var _lstSharedMetaDataCodes = _lstMetaData.Where(amd => _clientContactMetaDataIds.Contains(amd.AIMD_ID))
                                                       .Select(amd => amd.AIMD_Code).ToList();
                    var _dicContent = new Dictionary<String, String>();
                    _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTNAME, clientContact.Name);
                    _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTID, clientContact.ClientContactID.ToString());
                    _dicContent.Add(AppConsts.PSIEMAIL_CENTRALLOGINURL, centralLoginUrl);
                    _dicContent.Add(AppConsts.PSIEMAIL_SCHOOLNAME, institutionName);

                    var applicantDataHtml = String.Empty;

                    if (!lstApplicantInfo.IsNullOrEmpty())
                    {
                        var lstApplicantInfoContract = new List<OrganizationUserContract>();
                        lstApplicantInfo.ForEach(applicant =>
                        {
                            var applicantInfo = ProfileSharingManager.ConvertOrgUserIntoApplicantInfoContract(applicant, clientID);
                            lstApplicantInfoContract.Add(applicantInfo);
                        });

                        applicantDataHtml = ProfileSharingManager.GenerateApplicantMetaDataStringRotSharing(lstApplicantInfoContract, _lstSharedMetaDataCodes, clientID);
                    }
                    _dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, applicantDataHtml);
                    _dicContent.Add(AppConsts.PSIEMAIL_RotationDetails, rotationDetailsHTML);

                    _lstInvitationsContract.Where(cond => cond.ClientContactID == clientContact.ClientContactID).ForEach(invCon =>
                    {
                        if (invCon.TemplateData == null)
                        {
                            invCon.TemplateData = new Dictionary<string, string>();
                        }
                        invCon.TemplateData.AddRange(_dicContent);
                    });
                }
            }
        }


        private static void GeneratEmailContentForAgencyUsers(Int32 clientID, List<ApplicantInvitationMetaData> lstMetaData, List<usp_GetAgencyUserData_Result> lstAgencyUsers, List<InvitationDetailsContract> _lstInvitationsContract,
                            Dictionary<Int32, List<Int32>> dicAgencyUserMetaData, List<Entity.OrganizationUser> lstApplicantInfo, ClinicalRotationDetailContract rotationDetailsContract, string centralLoginUrl, string institutionName, Int32 rotationID)
        {
            //UAT-1403 : Add Rotation Details to Rotation Sharing Invitation Email
            var rotationDetailsHTML = ProfileSharingManager.GenerateRotationDetailsHTML(rotationDetailsContract);

            foreach (var agencyUser in lstAgencyUsers)
            {
                if (dicAgencyUserMetaData.ContainsKey(agencyUser.AgencyUserID.Value))
                {
                    var _agencyUserMetaDataIds = dicAgencyUserMetaData.Where(k => k.Key == agencyUser.AgencyUserID).First().Value;

                    var _lstSharedMetaDataCodes = lstMetaData.Where(amd => _agencyUserMetaDataIds.Contains(amd.AIMD_ID))
                                                       .Select(amd => amd.AIMD_Code).ToList();

                    var _dicContent = new Dictionary<String, String>();
                    _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTNAME, agencyUser.NAME);
                    _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTID, agencyUser.AgencyUserID.ToString());
                    _dicContent.Add(AppConsts.PSIEMAIL_CENTRALLOGINURL, centralLoginUrl);
                    _dicContent.Add(AppConsts.PSIEMAIL_SCHOOLNAME, institutionName);

                    var applicantDataHtml = String.Empty;

                    if (!lstApplicantInfo.IsNullOrEmpty())
                    {
                        var lstApplicantInfoContract = new List<OrganizationUserContract>();
                        lstApplicantInfo.ForEach(applicant =>
                        {
                            var applicantInfo = ProfileSharingManager.ConvertOrgUserIntoApplicantInfoContract(applicant, clientID);
                            lstApplicantInfoContract.Add(applicantInfo);
                        });
                        applicantDataHtml = ProfileSharingManager.GenerateApplicantMetaDataStringRotSharing(lstApplicantInfoContract, _lstSharedMetaDataCodes, clientID);
                    }
                    _dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, applicantDataHtml);
                    _dicContent.Add(AppConsts.PSIEMAIL_RotationDetails, rotationDetailsHTML);
                    _lstInvitationsContract.Where(cond => cond.AgencyUserId == agencyUser.AgencyUserID).ForEach(invCon =>
                    {
                        if (invCon.TemplateData == null)
                        {
                            invCon.TemplateData = new Dictionary<string, string>();
                        }
                        invCon.TemplateData.AddRange(_dicContent);
                    });
                }
            }
        }



        /// <summary>
        /// Filter the type of Compliance and Background packages to be used, based on the admin selection of the categories
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="applicantSharingPackages"></param>
        /// <param name="isCompliancePackage"></param>
        /// <returns></returns>
        private static List<ProfileSharingPackages> FilterSelectedComplianceBkgPkgs(String pkgType, List<ProfileSharingPackages> applicantSharingPackages, Boolean isCompliancePackage, List<SharingPackageDataContract> lstSharedPkgData)
        {
            var _lst = new List<ProfileSharingPackages>();
            var _tempList = applicantSharingPackages.Where(pkg => pkg.IsCompliancePkg == isCompliancePackage).ToList();

            var _lstPkgSelected = lstSharedPkgData.Where(pkg => pkg.PackageType == pkgType && pkg.IsCompletelyExcluded == false).ToList();

            foreach (var _crntSelectedPkg in _lstPkgSelected)
            {
                var _lstPkgFromDB = applicantSharingPackages.Where(p => p.PackageId == _crntSelectedPkg.PackageId && p.IsCompliancePkg == isCompliancePackage).ToList();

                if (!_lstPkgFromDB.IsNullOrEmpty())
                {
                    foreach (var _pkgFromDB in _lstPkgFromDB)
                    {
                        var _pkgToAdd = new ProfileSharingPackages();
                        _pkgToAdd.PackageId = _pkgFromDB.PackageId;
                        _pkgToAdd.PackageName = _pkgFromDB.PackageName;
                        _pkgToAdd.IsCompliancePkg = _pkgFromDB.IsCompliancePkg;

                        if (isCompliancePackage)
                        {
                            _pkgToAdd.PackageSubscriptionId = _pkgFromDB.PackageSubscriptionId;
                            _pkgToAdd.CompliancePkgCategories = new List<Entity.ClientEntity.ComplianceCategory>();

                            foreach (var crntPkgCategory in _pkgFromDB.CompliancePkgCategories)
                            {
                                if (_crntSelectedPkg.lstSelectedCategoryGrpIds.Contains(crntPkgCategory.ComplianceCategoryID))
                                {
                                    _pkgToAdd.CompliancePkgCategories.Add(new Entity.ClientEntity.ComplianceCategory
                                    {
                                        ComplianceCategoryID = crntPkgCategory.ComplianceCategoryID,
                                        CategoryName = crntPkgCategory.CategoryName
                                    });
                                }
                            }
                        }
                        else
                        {
                            _pkgToAdd.BkgOrderPkgId = _pkgFromDB.BkgOrderPkgId;
                            _pkgToAdd.BkgSvcGroups = new List<BkgSvcGroup>();
                            foreach (var crntPkgSvcGrp in _pkgFromDB.BkgSvcGroups)
                            {
                                if (_crntSelectedPkg.lstSelectedCategoryGrpIds.Contains(crntPkgSvcGrp.BSG_ID))
                                {
                                    _pkgToAdd.BkgSvcGroups.Add(new BkgSvcGroup
                                    {
                                        BSG_ID = crntPkgSvcGrp.BSG_ID,
                                        BSG_Name = crntPkgSvcGrp.BSG_Name
                                    });
                                }
                            }
                        }
                        _lst.Add(_pkgToAdd);
                    }
                }
            }

            return _lst;
        }


        private static Dictionary<String, String> GenerateMetaData(Int32 clientID, List<ApplicantInvitationMetaData> _lstMetaData, OrganizationUserContract applicant,
                                                    String recepientName, List<int> _lstSharedMataDataIds, string institutionName, string centralLoginUrl, Boolean IsApplicantSharing, String schoolContactName, String schoolContactEmailID, String instPrecep)
        {
            var _lstSharedMetaDataCodes = _lstMetaData.Where(amd => _lstSharedMataDataIds.Contains(amd.AIMD_ID))
                                                      .Select(amd => amd.AIMD_Code).ToList();

            var _dicContent = new Dictionary<String, String>();
            _dicContent.Add(AppConsts.PSIEMAIL_STUDENTNAME, applicant.FirstName + " " + applicant.LastName);
            _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTNAME, recepientName);
            _dicContent.Add(AppConsts.PSIEMAIL_CENTRALLOGINURL, centralLoginUrl);
            _dicContent.Add(AppConsts.PSIEMAIL_SCHOOLNAME, institutionName);

            //UAT-3006
            if (IsApplicantSharing)
            {
                _lstSharedMetaDataCodes.Add("BAAA");
                _lstSharedMetaDataCodes.Add("BAAB");
                _lstSharedMetaDataCodes.Add("BAAC"); //UAT-3662
            }

            var applicantDataHtml = String.Empty;

            if (!applicant.IsNullOrEmpty())
            {
                applicantDataHtml = ProfileSharingManager.GenerateApplicantMetaDataString(applicant, _lstSharedMetaDataCodes, clientID, schoolContactName, schoolContactEmailID, instPrecep);
            }
            //_dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, ProfileSharingManager.GenerateApplicantMetaDataString(applicant, _lstSharedMetaDataCodes, clientID));
            _dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, applicantDataHtml);
            return _dicContent;
        }

        private static InvitationDetailsContract GenerateInvitationDetailsInstance(Int32 currentUserId, Int32 clientID, Int32 applicantID,
                                                                            Guid _identifier, String agencyName, Int32? agencyUserId,
                                                                            String name, String phone, String email, String applicantInvMetaDataIds
                                                                          , Int32 userTypeID, String userTypeCode, Int32 selectedAgencyID,
                                                                             String pxc_ExpireOption, String pxc_ExpirationTypeCode, Int32? pxc_MaxViews, DateTime? pxc_ExpirationDate
                                                                            , bool isNonScheduledInvitation, DateTime? invitationSchedlueDate,
                                                                            Int32? clientContactID = null)
        {
            var currentInvitation = new InvitationDetailsContract();

            currentInvitation.InvitationIdentifier = _identifier;
            currentInvitation.AgencyId = selectedAgencyID;
            currentInvitation.AgencyUserId = agencyUserId;
            currentInvitation.Name = name;
            currentInvitation.Phone = phone;
            currentInvitation.EmailAddress = email;
            currentInvitation.Agency = agencyName;
            currentInvitation.CurrentDateTime = DateTime.Now;
            currentInvitation.ApplicantId = applicantID;
            currentInvitation.TenantID = clientID;
            //currentInvitation.MaxViews = null;
            //currentInvitation.ExpirationDate = null;
            currentInvitation.CustomMessage = String.Empty;
            currentInvitation.CurrentUserId = currentUserId;

            //UAT 1320: Client admin expire profile shares

            if (pxc_ExpireOption.ToLower() == "yes")
            {
                if (pxc_ExpirationTypeCode == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue())
                {
                    currentInvitation.MaxViews = pxc_MaxViews;
                    currentInvitation.ExpirationTypeCode = InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue();
                }
                else
                {
                    currentInvitation.ExpirationDate = pxc_ExpirationDate;
                    currentInvitation.ExpirationTypeCode = InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue();
                }
            }
            else
            {
                currentInvitation.ExpirationTypeCode = InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue();
            }

            //Set the ExpirationTypeId based on ExpirationTypeCode.
            currentInvitation.ExpirationTypeId = ProfileSharingManager.GetExpirationTypes().Where(x => x.Code == currentInvitation.ExpirationTypeCode).Select(x => x.ExpirationTypeID).FirstOrDefault();

            if (isNonScheduledInvitation)
            {
                currentInvitation.SharedApplicantMetaDataIds = applicantInvMetaDataIds.ToString().Split(',').Select(Int32.Parse).ToList();
            }

            currentInvitation.InviteeUserTypeID = userTypeID;
            currentInvitation.InviteeUserTypeCode = userTypeCode;
            currentInvitation.InvitationScheduleDate = invitationSchedlueDate;
            if (!clientContactID.IsNullOrEmpty())
            {
                currentInvitation.ClientContactID = clientContactID.Value;
            }
            return currentInvitation;
        }

        /// <summary>
        /// Update the Link Url to be used, with Token, in the Email Template. 
        /// This is added after the Invitation generation in database, 
        /// due to dependency on Token generated for the Invitation.
        /// </summary>
        /// <param name="lstInvitations"></param>
        /// <param name="lstEmailContract"></param>
        private static void UpdateInvitationDetailsContract(List<ProfileSharingInvitation> lstInvitations, List<InvitationDetailsContract> lstInvitationContract, String profileSharingURL)
        {
            List<Entity.SharedDataEntity.lkpOrgUserType> lstOrgUserType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpOrgUserType>();
            foreach (var invitation in lstInvitationContract)
            {
                String code = lstOrgUserType.Where(ot => ot.OrgUserTypeID == invitation.InviteeUserTypeID).FirstOrDefault().OrgUserTypeCode;

                var _invitation = lstInvitations.Where(inv => inv.InvitationIdentifier == invitation.InvitationIdentifier).First();
                var queryString = new Dictionary<String, String>
                                                                                 {
                                                                                    {AppConsts.QUERY_STRING_INVITE_TOKEN, Convert.ToString(_invitation.PSI_Token)},
                                                                                    {AppConsts.QUERY_STRING_USER_TYPE_CODE , code},
                                                                                    //UAT-2519
                                                                                     {"IsSearchedShare",Convert.ToString(true)}
                                                                                    //{AppConsts.IS_PROFILE_SHARE_SEARCH,Convert.ToString(IsProfileSharing)},
                                                                                    //{AppConsts.IS_ROTATION_SHARE_SEARCH,Convert.ToString(IsRotationSharing)}
                                                                                 };
                //UAT-2519
                String applicationUrl = profileSharingURL;
                if (!(applicationUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || applicationUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                {
                    applicationUrl = string.Concat("http://", applicationUrl.Trim());
                }
                var url = String.Format(applicationUrl + "?args={0}", queryString.ToEncryptedQueryString());
                invitation.TemplateData.Add(AppConsts.PSIEMAIL_PROFILEURL, url);
            }
        }

        /// <summary>
        /// Update the ProfileSharingInvitationID, based on the GUID, used in generating the Attestation report.
        /// </summary>
        /// <param name="lstInvitations"></param>
        /// <param name="lstSharedInfoDetails"></param>
        private static void UpdateInvitationSharedInfoDetailsContract(List<ProfileSharingInvitation> lstInvitations, List<InvitationSharedInfoDetails> lstSharedInfoDetails)
        {
            foreach (var sharedInfoDetails in lstSharedInfoDetails)
            {
                var _invitation = lstInvitations.Where(inv => inv.InvitationIdentifier == sharedInfoDetails.InvitationIdentifier).First();
                sharedInfoDetails.SharingInvitationID = _invitation.PSI_ID;
            }
        }
        #endregion

        #region UAT 1882: Phase 3(12): When a student profile shares, they should be presented with a selectable list of agencies, which have been associated with nodes they have orders with.
        public static List<AgencyContract> GetAgencyForApplicant(int tenantID, int orgUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetAgencyForApplicant(orgUserID);
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

        public static Boolean IsIndividualShared(Int32 ProfileSharingInvitationID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsIndividualShared(ProfileSharingInvitationID);
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

        public static Boolean HasConsidatePassportPermission(Int32 agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().HasConsidatePassportPermission(agencyUserID);
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

        //UAT-2090 : Complete Question 4 (C5) from UAT-2052
        public static Boolean SaveInvitationReviewStatusNotes(List<Int32> lstProfileSharingInvitationIds, List<String> clinicalRotationIDs, String reviewStatusCode, String notes, Int32 currentLoggedInUserId, Int32 organisationUserID, Boolean isIndividualReview)
        {
            try
            {
                String InvitationIDs = String.Join(",", lstProfileSharingInvitationIds);
                String rotationIDs = String.Join(",", clinicalRotationIDs);

                Int32 selectedInvitationReviewStatusId = AppConsts.NONE;
                if (!reviewStatusCode.IsNullOrEmpty())
                {
                    selectedInvitationReviewStatusId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                    .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == reviewStatusCode).First().SUIRS_ID;
                }

                return BALUtils.GetProfileSharingRepoInstance().SaveInvitationReviewStatusNotes(InvitationIDs, rotationIDs, selectedInvitationReviewStatusId, notes, currentLoggedInUserId, organisationUserID, isIndividualReview);
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

        #region UAT-2071: Configuration Rotation and Tracking packages must be fully compliant to share.

        public static List<RotationAndTrackingPkgStatusContract> GetComplianceStatusOfImmunizationAndRotationPackages(Int32 tenantID, String delimittedOrgUserIDs, String delimittedTrackingPkgIDs, Int32 rotationID, String SearchType)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetComplianceStatusOfImmunizationAndRotationPackages(delimittedOrgUserIDs, delimittedTrackingPkgIDs, rotationID, SearchType);
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

        public static Boolean IsOnlyRotationPkgShare(List<Int32> agencyIDs)
        {
            try
            {
                String code = AgencyPermissionType.ONLY_ROTATION_PACKAGE_SHARE_PRMSN.GetStringValue();
                Int32 agencyPrmsnTypeID = LookupManager.GetSharedDBLookUpData<lkpAgencyPermissionType>().Where(x => x.APT_Code == code && !x.APT_IsDeleted).FirstOrDefault().APT_ID;
                return BALUtils.GetProfileSharingRepoInstance().IsOnlyRotationPkgShare(agencyIDs, agencyPrmsnTypeID);
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

        public static Boolean IsTrackingPkgCompliantReqd(Int32 agencyID, Int32 tenantID = AppConsts.NONE, Int32 rotationID = AppConsts.NONE)
        {
            try
            {
                String code = AgencyPermissionType.COMPLIANCE_REQD_TRACKING_PRMSN.GetStringValue();
                List<Int32> AgencyIDs = new List<Int32>();
                Int32 agencyPrmsnTypeID = LookupManager.GetSharedDBLookUpData<lkpAgencyPermissionType>().Where(x => x.APT_Code == code && !x.APT_IsDeleted).FirstOrDefault().APT_ID;
                //Get AgencyID's based upon rotationID's
                if (rotationID > AppConsts.NONE)
                {
                    AgencyIDs = BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetAgenciesByRotationID(rotationID);
                }
                else
                {
                    AgencyIDs.Add(agencyID);
                }
                return BALUtils.GetProfileSharingRepoInstance().IsTrackingPkgCompliantReqd(agencyID, agencyPrmsnTypeID, AgencyIDs);
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


        public static Dictionary<Int32, String> IsRequirementPkgCompliantReqd(String agencyIDs)
        {
            try
            {
                String code = AgencyPermissionType.COMPLIANCE_REQD_ROTATION_PRMSN.GetStringValue();
                Int32 agencyPrmsnTypeID = LookupManager.GetSharedDBLookUpData<lkpAgencyPermissionType>().Where(x => x.APT_Code == code && !x.APT_IsDeleted).FirstOrDefault().APT_ID;
                return BALUtils.GetProfileSharingRepoInstance().IsRequirementPkgCompliantReqd(agencyIDs, agencyPrmsnTypeID);
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
        /// Get AgencyPermissionTypeID from lookup table.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int GetAgencyPermissionTypeID(string code)
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAgencyPermissionType>().Where(x => x.APT_Code == code && !x.APT_IsDeleted).FirstOrDefault().APT_ID;
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
        /// Get AgencyPermissionAccessTypeID from lookup table.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Int32 GetAgencyPermissionAccessTypeID(string code)
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAgencyPermissionAccessType>().Where(x => x.APAT_Code == code && !x.APAT_IsDeleted).FirstOrDefault().APAT_ID;
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

        public static Dictionary<Int32, Int32> GetAgencyPermisionByAgencyID(Int32 agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyPermisionByAgencyID(agencyID);
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

        #region UAT-2051, No defining details about Roation. Roation/Profile simply says "roation shared".
        public static String GetSharingInfoByInvitationGrpID(int tenantID, int invtationGrpID)
        {
            return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetSharingInfoByInvitationGrpID(invtationGrpID);
        }
        #endregion

        #region UAT-2196, Add "Send Message" button on rotation details screen.
        public static Dictionary<Int32, String> GetOrganizationUserIDByRotMemberIDs(Int32 tenantId, List<Int32> lstRotMemberID)
        {
            return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetOrganizationUserIDByRotMemberIDs(tenantId, lstRotMemberID);
        }
        #region UAT-3463
        public static Dictionary<Int32, String> GetOrganizationUserDetailsByOrgUserIDs(Int32 tenantId, List<Int32> lstOrgUserIDs)
        {
            return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetOrganizationUserDetailsByOrgUserIDs(tenantId, lstOrgUserIDs);
        }
        #endregion
        #endregion

        //UAT-4013
        public static Dictionary<Int32, String> GetOrgUserDetailsByOrgUserID(List<Int32> lstOrgUserIDs)
        {
            return BALUtils.GetSecurityInstance().GetOrgUserDetailsByOrgUserID(lstOrgUserIDs);
        }


        #region UAT-2164, Agency User - Granular Permissions
        public static List<BackgroundDocumentPermissionContract> GetBackgroundDocumentPermissionByReqPkgID(int tenantId, int requirementPkgID, int loggedInAgencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetBackgroundDocumentPermissionByReqPkgID(requirementPkgID, loggedInAgencyUserID);
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

        public static bool IsRotationContainsRotationPkg(Int32 rotationId, Int32 selectedTenantID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(selectedTenantID).IsRotationContainsRotationPkg(rotationId);
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

        public static List<Tuple<Int32, Int32, Int32, List<Int32>>> GetRotationSharedInvitations(List<InvitationIDsDetailContract> lstClinicalRotations, Int32 inviteeOrgUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetRotationSharedInvitations(lstClinicalRotations, inviteeOrgUserId);
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

        //UAT-2181:Enhance adding tenants to agencies with check boxes on the Manage Agencies screen to select which agencies you would like to add the selected tenant to
        public static Boolean AssignTenantToAgency(Int32 TenantID, List<Int32> SelectedAgencyIDs, Int32 CurrentLoggedInUserId)
        {
            try
            {
                List<Int32> MappedAgencyIDs = BALUtils.GetProfileSharingRepoInstance().SaveAgenciesInstitutionMapping(SelectedAgencyIDs, TenantID, CurrentLoggedInUserId);
                if (!MappedAgencyIDs.IsNullOrEmpty())
                {
                    return BALUtils.GetProfileSharingClientRepoInstance(TenantID).AssignTenantToAgency(TenantID, MappedAgencyIDs, CurrentLoggedInUserId);
                }
                else
                {
                    return false;
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
        //UAT-2247
        public static DataSet GetRequirementLiveData(Int32 tenantId, Int32 requirementPackageSubscriptionID)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetRequirementLiveData(requirementPackageSubscriptionID);
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

        #region UAT-2452
        public static void AddAgencyUserSharedProfilePermisssionData(String ComplianceSharedInfoTypeCode, String RotationSharedInfoTypeCode, String BkgSharedInfoTypeCode, Int32? AgencyUserID, Int32? ProfileShareInvitationGroupID, Int32 currentLoggedInUserID)
        {
            try
            {
                List<AgencyUserSharedProfilePermission> lstAgencyUserSharedProfilePermisssion = new List<AgencyUserSharedProfilePermission>();
                AgencyUserSharedProfilePermission agencyUserPermissionData = null;
                List<Entity.SharedDataEntity.lkpInvitationSharedInfoType> lkpInvitationSharedInfoType = ProfileSharingManager.GetSharedInfoType();

                //Background
                if (!BkgSharedInfoTypeCode.IsNullOrEmpty())
                {
                    BkgSharedInfoTypeCode.Split(',').ToList().ForEach(x =>
                    {
                        agencyUserPermissionData = new AgencyUserSharedProfilePermission();
                        agencyUserPermissionData.AUSPP_InvitationSharedInfoTypeID = lkpInvitationSharedInfoType.Where(cond => cond.Code == x).FirstOrDefault().SharedInfoTypeID;
                        agencyUserPermissionData.AUSPP_ProfileSharingInvitationGroupID = ProfileShareInvitationGroupID.Value;
                        agencyUserPermissionData.AUSPP_AgencyUserID = AgencyUserID.Value;
                        agencyUserPermissionData.AUSPP_IsDeleted = false;
                        agencyUserPermissionData.AUSPP_CreatedBy = currentLoggedInUserID;
                        agencyUserPermissionData.AUSPP_CreatedOn = DateTime.Now;

                        lstAgencyUserSharedProfilePermisssion.Add(agencyUserPermissionData);
                    });
                }

                //Compliance
                agencyUserPermissionData = new AgencyUserSharedProfilePermission();
                agencyUserPermissionData.AUSPP_InvitationSharedInfoTypeID = lkpInvitationSharedInfoType.Where(cond => cond.Code == ComplianceSharedInfoTypeCode).FirstOrDefault().SharedInfoTypeID;
                agencyUserPermissionData.AUSPP_ProfileSharingInvitationGroupID = ProfileShareInvitationGroupID.Value;
                agencyUserPermissionData.AUSPP_AgencyUserID = AgencyUserID.Value;
                agencyUserPermissionData.AUSPP_IsDeleted = false;
                agencyUserPermissionData.AUSPP_CreatedBy = currentLoggedInUserID;
                agencyUserPermissionData.AUSPP_CreatedOn = DateTime.Now;

                lstAgencyUserSharedProfilePermisssion.Add(agencyUserPermissionData);

                //Rotation
                agencyUserPermissionData = new AgencyUserSharedProfilePermission();
                agencyUserPermissionData.AUSPP_InvitationSharedInfoTypeID = lkpInvitationSharedInfoType.Where(cond => cond.Code == RotationSharedInfoTypeCode).FirstOrDefault().SharedInfoTypeID;
                agencyUserPermissionData.AUSPP_ProfileSharingInvitationGroupID = ProfileShareInvitationGroupID.Value;
                agencyUserPermissionData.AUSPP_AgencyUserID = AgencyUserID.Value;
                agencyUserPermissionData.AUSPP_IsDeleted = false;
                agencyUserPermissionData.AUSPP_CreatedBy = currentLoggedInUserID;
                agencyUserPermissionData.AUSPP_CreatedOn = DateTime.Now;

                lstAgencyUserSharedProfilePermisssion.Add(agencyUserPermissionData);
                BALUtils.GetProfileSharingRepoInstance().SaveAgencyUserSharedPermission(lstAgencyUserSharedProfilePermisssion);
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

        public static Int32 GetAgencyUserByEmail(String emailId)
        {
            try
            {
                AgencyUser agencyUser = BALUtils.GetProfileSharingRepoInstance().IsEmailAlreadyExistAgencyUser(emailId);
                if (!agencyUser.IsNullOrEmpty())
                    return agencyUser.AGU_ID;
                return AppConsts.NONE;


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

        public static void CopySharedInvForNewlyMappedAgencyForAgencyUser(Int32 AgencyUserId, String AgencyIds, Int32 TenantId)
        {
            try
            {
                BALUtils.GetProfileSharingClientRepoInstance(TenantId).CopySharedInvForNewlyMappedAgencyForAgencyUser(AgencyUserId, AgencyIds, TenantId);
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

        #region UAT 2367
        public static List<Guid> GetAgencyVerificationCode(int agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyVerificationCode(agencyUserID);
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

        public static AgencyUser GetAgencyUserByID(int agencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserByID(agencyUserID);
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


        #region UAT-2414, Create a snapshot on Rotation End Date

        /// <summary>
        /// Generate the Snapshot of the Requirement Package when Rotation End
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="packageSubscrptionID"></param>
        /// <returns></returns>
        public static List<RequirmentPkgSubscriptionDataContract> GetRequirementSubscriptionDataForSnapshot(Int32 tenantId, Int32 chunkSize)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetRequirementSubscriptionDataForSnapshot(chunkSize);
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
        /// Generate the Snapshot of the Requirement Package when Rotation End
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="packageSubscrptionID"></param>
        /// <returns></returns>
        public static Boolean SaveRequirementSnapshotOnRotationEnd(Int32 tenantId, Int32 reqPackageSubscrptionId, Int32 currentUserId, String profileSharingInvitationIDs)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).SaveRequirementSnapshotOnRotationEnd(reqPackageSubscrptionId, currentUserId, profileSharingInvitationIDs);
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

        #region UAT-2443:Attestation Merge and multiple share behavior changes
        private static void UpdateInvitationDocumentPath(Int32 invitationDocumentId, String pathToUpdate, Int32 currentLoggedInUserId, Boolean isForEveryOne)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().UpdateInvitationDocumentPath(invitationDocumentId, pathToUpdate, currentLoggedInUserId, isForEveryOne);
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

        #region UAT-2538
        //Code commented for UAT-2803
        //public static Boolean AssignunAssignAgencyUsersToSendEmail(List<Int32> selectedAgencyUserIds, Boolean IsNeedToSendEmail, Int32 CurrentLoggedInUserId)
        //{
        //    try
        //    {
        //        return BALUtils.GetProfileSharingRepoInstance().AssignUnAssignAgencyUsersToSendEmail(selectedAgencyUserIds, IsNeedToSendEmail, CurrentLoggedInUserId);
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


        public static void SendMailForRotInvAppRej(List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData, String InvitataionStatusCode, Int32 CurrentLoggedInUserID, String CurrentUserName)
        {
            if (!lstRotationData.IsNullOrEmpty())
            {

                foreach (var item in lstRotationData)
                {
                    Int32 RotationID = item.Item1;
                    Int32 TenantID = item.Item2;
                    Int32 RotationAgencyID = item.Item3;
                    List<Int32> lstRotationInvitations = item.Item4;

                    if (!InvitataionStatusCode.IsNullOrEmpty() && (InvitataionStatusCode != SharedUserRotationReviewStatus.PENDING_REVIEW.GetStringValue()))
                    {
                        Int32 selectedInvitationReviewStatusId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                         .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == InvitataionStatusCode).First().SUIRS_ID;

                        String selectedInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                         .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == InvitataionStatusCode).First().SUIRS_Name;

                        List<Int32> NewlstRotationInvitations = BALUtils.GetProfileSharingRepoInstance().GetInvitationIDsIfInvitationStatusChanged(lstRotationInvitations, selectedInvitationReviewStatusId);
                        if (!NewlstRotationInvitations.IsNullOrEmpty())
                        {
                            List<Int32> AppOrgUserIdOnInvitations = new List<Int32>();
                            if (!NewlstRotationInvitations.IsNullOrEmpty())
                            {
                                AppOrgUserIdOnInvitations = BALUtils.GetProfileSharingRepoInstance().GetAppOrgOnInvitations(NewlstRotationInvitations);
                            }

                            Entity.ClientEntity.ClinicalRotation clinicalRotation = BALUtils.GetProfileSharingClientRepoInstance(TenantID).GetClinicalRotation(RotationID);

                            List<Entity.OrganizationUser> lstAgencyUsersDataForMail = BALUtils.GetProfileSharingRepoInstance().GetAgencyUserfromAgency(RotationAgencyID);


                            Agency agency = BALUtils.GetProfileSharingRepoInstance().GetAgency(RotationAgencyID);

                            String TenantName = SecurityManager.GetTenant(TenantID).TenantName;
                            List<Entity.OrganizationUser> lstApplicantsinRotation = BALUtils.GetProfileSharingRepoInstance().GetClinicalRotationApplicantSharedData(RotationID, RotationAgencyID);

                            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                            //mockData.EmailID = "";
                            //mockData.UserName = "Admin";

                            if (!AppOrgUserIdOnInvitations.IsNullOrEmpty())
                            {
                                lstApplicantsinRotation = lstApplicantsinRotation.Where(cond => AppOrgUserIdOnInvitations.Contains(cond.OrganizationUserID)).ToList();
                            }

                            if (!lstAgencyUsersDataForMail.IsNullOrEmpty() && !lstApplicantsinRotation.IsNullOrEmpty())
                            {
                                foreach (Entity.OrganizationUser OrgUSer in lstApplicantsinRotation)
                                {

                                    List<CommunicationTemplateContract> lstcommunicationTemplateContract = new List<CommunicationTemplateContract>();

                                    //UAT-2779
                                    Entity.OrganizationUser adminDetailsWhoSharedApplicantProfile = BALUtils.GetProfileSharingRepoInstance().GetAdminDetailsWhoSharedProfile(OrgUSer.OrganizationUserID, RotationID, TenantID, RotationAgencyID);

                                    //UAT-27779
                                    if (!adminDetailsWhoSharedApplicantProfile.IsNullOrEmpty() && adminDetailsWhoSharedApplicantProfile.OrganizationUserID > 0)
                                    {
                                        CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
                                        communicationTemplateContract.ReceiverOrganizationUserId = adminDetailsWhoSharedApplicantProfile.OrganizationUserID;
                                        communicationTemplateContract.RecieverEmailID = adminDetailsWhoSharedApplicantProfile.PrimaryEmailAddress;
                                        communicationTemplateContract.RecieverName = adminDetailsWhoSharedApplicantProfile.FirstName + adminDetailsWhoSharedApplicantProfile.LastName;
                                        communicationTemplateContract.CurrentUserId = CurrentLoggedInUserID;
                                        lstcommunicationTemplateContract.Add(communicationTemplateContract);
                                    }

                                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    dictMailData.Add(EmailFieldConstants.STUDENT_FIRST, OrgUSer.FirstName);
                                    dictMailData.Add(EmailFieldConstants.STUDENT_LAST, OrgUSer.LastName);
                                    dictMailData.Add(EmailFieldConstants.INVITATION_STATUS, selectedInvitationReviewStatus);
                                    dictMailData.Add(EmailFieldConstants.AGENCY_NAME, agency.AG_Name);
                                    dictMailData.Add(EmailFieldConstants.DEPARTMENT_NAME, clinicalRotation.CR_Department);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, TenantName);
                                    dictMailData.Add(EmailFieldConstants.ROTATION_START_DATE, clinicalRotation.CR_StartDate.Value.ToString("MM/dd/yyyy"));
                                    dictMailData.Add(EmailFieldConstants.ROTATION_END_DATE, clinicalRotation.CR_EndDate.Value.ToString("MM/dd/yyyy"));
                                    dictMailData.Add(EmailFieldConstants.AGENCY_USER_NAME, CurrentUserName);
                                    dictMailData.Add(EmailFieldConstants.TENANT_ID, TenantID); // UAT-4372

                                    foreach (Entity.OrganizationUser OU in lstAgencyUsersDataForMail)
                                    {
                                        CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
                                        communicationTemplateContract.ReceiverOrganizationUserId = OU.OrganizationUserID;
                                        communicationTemplateContract.RecieverEmailID = OU.PrimaryEmailAddress;
                                        communicationTemplateContract.RecieverName = OU.FirstName + OU.LastName;
                                        communicationTemplateContract.CurrentUserId = CurrentLoggedInUserID;
                                        lstcommunicationTemplateContract.Add(communicationTemplateContract);
                                    }

                                    #region UAT-3254
                                    String RotationHirarchyIds = String.Empty;
                                    if (!clinicalRotation.IsNullOrEmpty() && !clinicalRotation.ClinicalRotationHierarchyMappings.IsNullOrEmpty())
                                    {
                                        List<Int32> lstHierarchyIds = clinicalRotation.ClinicalRotationHierarchyMappings.Where(cond => !cond.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
                                        if (!lstHierarchyIds.IsNullOrEmpty())
                                        {
                                            RotationHirarchyIds = String.Join(",", lstHierarchyIds);
                                        }
                                    }
                                    #endregion

                                    CommunicationManager.SendRotInvitationAppRejMail(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION, mockData, dictMailData, TenantID, lstcommunicationTemplateContract, RotationHirarchyIds, RotationID);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region UAT-2529 -
        public static Tuple<Boolean, Boolean> GetAgencyInstitutePermissionForSelectedAgency(Int32 agencyID, Int32 tenantID)
        {
            try
            {
                //get data from shared database

                Int32 agencyInstitutionID = BALUtils.GetProfileSharingRepoInstance().GetAgencyInstitutionID(agencyID, tenantID);

                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetAgencyInstitutionPermissionForSelectedAgency(agencyInstitutionID);
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

        public static Boolean CheckAgencyProfileSharingPermission(Int32 agencyID, Int32 tenantID)
        {
            try
            {
                Int32 agencyInstitutionID = BALUtils.GetProfileSharingRepoInstance().GetAgencyInstitutionID(agencyID, tenantID);
                if (agencyInstitutionID.IsNotNull())
                    return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetAgencyProfileSharingPermission(agencyInstitutionID);
                else
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

        public static Tuple<Boolean, String> CheckApplicantIndiviualProfileSharingPermission(String agencyUserEmail, Int32 tenantID)
        {
            try
            {
                Dictionary<Int32, String> agencyInstitution = BALUtils.GetProfileSharingRepoInstance().GetAgencyInstitutionIdsForIndivialSharingPermission(agencyUserEmail, tenantID);
                if (!agencyInstitution.IsNullOrEmpty())
                {
                    return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetApplicantIndividualProfileSharingPermission(agencyInstitution);
                }
                else
                    return new Tuple<Boolean, String>(true, String.Empty);
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

        #region UAT-2639:Agency hierarchy mapping: Default Hierarchy for Client Admin
        public static DeptProgramMapping GetClientAdminRootNode(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetClientAdminRootNode(tenantId);
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

        public static AgencyHierarchyAgencyProfileSharePermission GetAgencyHierarchyProfileSharePermission(Int32 tenantID, Int32 agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyHierarchyProfileSharePermission(agencyID, tenantID);
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

        #region UAT-2640:Update Agency User (People and Places > Manage Agencies) Agency multi select dropdown will be removed  and multiple hierarchy selection option will be provided here.
        public static Boolean SaveAgencyHirInstNodeMappingForClientAdmin(Int32 tenantID, INTSOF.UI.Contract.ProfileSharing.AgencyHierarchyContract agencyHierarchyContract, Int32 currentLoggedInUserID, Int32 agencyID, Int32 agencyInstitutionID, Boolean isAdminLoggedIn)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).SaveAgencyHirInstNodeMappingForClientAdmin(agencyHierarchyContract, currentLoggedInUserID, agencyID, agencyInstitutionID, isAdminLoggedIn);
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

        public static Dictionary<Int32, String> GetAgencyHierarchyOfCurrentTenantToAddUser(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyHierarchyOfCurrentTenantToAddUser(tenantID);
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


        public static Boolean IsCurrentLoggedInUser(String CurrentLoggedInOrgUserID, Int32 SelectedAgencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsCurrentLoggedInUser(CurrentLoggedInOrgUserID, SelectedAgencyUserID);
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

        public static bool IsAgencyUserOnDifferentNode(Int32 CurrentLoggedInOrgUserID, Int32 SelectedAgencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsAgencyUserOnDifferentNode(CurrentLoggedInOrgUserID, SelectedAgencyUserID);
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

        #region UAT-2511
        public static Boolean SaveRotationAuditHistory(Int32 rotationID, Int32 tenantID, String newReviewStatusCode, String newNotes, Int32 currentLoggedInUserID, Int32 inviteeOrgUserID, Int32 agencyID)
        {
            try
            {
                var oldRotationReviewStatus = BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetShareduserRotationReviewStatusByRotationIds(rotationID, inviteeOrgUserID, agencyID);
                ClinicalRotationDetailContract clinicalRotation = ClinicalRotationManager.GetClinicalRotationById(tenantID, rotationID, null);
                Int32 reviewStatusId = LookupManager.GetLookUpData<lkpSharedUserRotationReviewStatu>(tenantID).FirstOrDefault(cnd => cnd.SURRS_Code == newReviewStatusCode
                                                                                                   && !cnd.SURRS_IsDeleted).SURRS_ID;
                if (!clinicalRotation.IsNullOrEmpty())
                {
                    Int32 oldReviewStatusID = AppConsts.NONE;
                    if (!oldRotationReviewStatus.IsNullOrEmpty())
                    {
                        oldReviewStatusID = oldRotationReviewStatus.SURR_RotationReviewStatusID.IsNullOrEmpty() ? AppConsts.NONE
                                                                                                                   : oldRotationReviewStatus.SURR_RotationReviewStatusID.Value;
                    }
                    String oldNotes = String.Empty;
                    //Int32 agencyId = AppConsts.NONE;
                    //agencyId = clinicalRotation.AgencyID;

                    List<lkpAuditChangeType> lstAuditChangeType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAuditChangeType>()
                                                                       .Where(cond => !cond.LACT_IsDeleted).ToList();
                    return BALUtils.GetProfileSharingRepoInstance().SaveRotationAuditHistory(rotationID, tenantID, reviewStatusId, oldReviewStatusID, newNotes, oldNotes
                                                                                             , currentLoggedInUserID, lstAuditChangeType, agencyID, newReviewStatusCode);
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

        public static List<AgencyUserAuditHistoryDataContract> GenerateAuditHistoryDataForRerquestForAudit(List<Int32> profileSharingInvitationIds, Int32 currentLoggedInUserID)
        {
            try
            {
                List<lkpAuditChangeType> lstAuditChangeType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAuditChangeType>()
                                                                    .Where(cond => !cond.LACT_IsDeleted).ToList();
                return BALUtils.GetProfileSharingRepoInstance().GenerateAuditHistoryDataForRerquestForAudit(profileSharingInvitationIds, currentLoggedInUserID, lstAuditChangeType);
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

        public static Boolean SaveAgencyUserAuditHistory(List<AgencyUserAuditHistoryDataContract> lstAuditDataContract, Boolean isSaveChangesRequired)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveAgencyUserAuditHistory(lstAuditDataContract, isSaveChangesRequired);
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

        public static List<AgencyUserAuditHistoryContract> AgencyUserAuditHistory(Int32 institutionID, Int32 agencyID, string rotationName, string applicantName,
            string updatedByName, DateTime updatedDate, CustomPagingArgsContract customPagingContract)
        {
            try
            {
                var _lstAgencyUserAuditHistory = new List<AgencyUserAuditHistoryContract>();
                var _dtAgencyUserAuditHistory = BALUtils.GetProfileSharingRepoInstance().AgencyUserAuditHistory(institutionID, agencyID, rotationName, applicantName, updatedByName, updatedDate, customPagingContract);

                if (_dtAgencyUserAuditHistory.Rows.Count > AppConsts.NONE)
                {
                    var _AgencyUserAuditHistoryIDColumn = "AgencyUserAuditHistoryID";
                    var _InstituitionColumn = "Instituition";
                    var _AgencyNameColumn = "AgencyName";
                    var _RotationNameColumn = "RotationName";
                    var _ApplicantNameColumn = "ApplicantName";
                    var _UpdatedDateColumn = "UpdatedDate";
                    var _UpdatedByColumn = "UpdatedBy";
                    var _ChangeValueColumn = "ChangeValue";
                    var _TotalCountColumn = "TotalCount";

                    for (int i = 0; i < _dtAgencyUserAuditHistory.Rows.Count; i++)
                    {
                        var _agencyUserAuditHistoryContract = new AgencyUserAuditHistoryContract();
                        _agencyUserAuditHistoryContract.AgencyUserAuditHistoryID = Convert.ToInt32(_dtAgencyUserAuditHistory.Rows[i][_AgencyUserAuditHistoryIDColumn]);
                        _agencyUserAuditHistoryContract.Instituition = Convert.ToString(_dtAgencyUserAuditHistory.Rows[i][_InstituitionColumn]);
                        _agencyUserAuditHistoryContract.AgencyName = Convert.ToString(_dtAgencyUserAuditHistory.Rows[i][_AgencyNameColumn]) == "" ? "Profile Share" : Convert.ToString(_dtAgencyUserAuditHistory.Rows[i][_AgencyNameColumn]);
                        _agencyUserAuditHistoryContract.RotationName = Convert.ToString(_dtAgencyUserAuditHistory.Rows[i][_RotationNameColumn]) == "" ? "Profile Share" : Convert.ToString(_dtAgencyUserAuditHistory.Rows[i][_RotationNameColumn]);
                        _agencyUserAuditHistoryContract.ApplicantName = Convert.ToString(_dtAgencyUserAuditHistory.Rows[i][_ApplicantNameColumn]);
                        _agencyUserAuditHistoryContract.UpdatedDate = _dtAgencyUserAuditHistory.Rows[i][_UpdatedDateColumn] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(_dtAgencyUserAuditHistory.Rows[i][_UpdatedDateColumn]);
                        _agencyUserAuditHistoryContract.UpdatedBy = Convert.ToString(_dtAgencyUserAuditHistory.Rows[i][_UpdatedByColumn]);
                        _agencyUserAuditHistoryContract.ChangeValue = Convert.ToString(_dtAgencyUserAuditHistory.Rows[i][_ChangeValueColumn]);
                        _agencyUserAuditHistoryContract.TotalCount = Convert.ToInt32(_dtAgencyUserAuditHistory.Rows[i][_TotalCountColumn]);
                        _lstAgencyUserAuditHistory.Add(_agencyUserAuditHistoryContract);
                    }
                }
                return _lstAgencyUserAuditHistory;
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
        #region UAT 2548
        public static List<INTSOF.ServiceDataContracts.Modules.Common.TenantDetailContract> GetAgencyHierarchyMappedTenant(Int32 AgencyUserID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyHierarchyMappedTenant(AgencyUserID);
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
        public static List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract> GetAgencyUserMappedAgencies(Int32 AgencyUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserMappedAgencies(AgencyUserID);
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
        public static List<AgencyApplicantShareHistoryContract> GetApplicantProfileSharingHistory(AgencyApplicantShareHistoryContract agencyApplicantShareHistoryContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(agencyApplicantShareHistoryContract.TenantId).GetApplicantProfileSharingHistory(agencyApplicantShareHistoryContract, customPagingArgsContract);
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
        public static List<AgencyApplicantStatusContract> GetAgencyApplicantStatus(AgencyApplicantStatusContract agencyApplicantStatusContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(agencyApplicantStatusContract.TenantId).GetAgencyApplicantStatus(agencyApplicantStatusContract, customPagingArgsContract);
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

        #region UAT-2706
        public static Entity.SharedDataEntity.ClientSystemDocument GetSharedClientSystemDocument(Int32 clientSystemDocId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetSharedClientSystemDocument(clientSystemDocId);
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

        public static List<Int32> AnyAgencyUserExists(Int32 institutionID, String agencyIds)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().AnyAgencyUserExists(institutionID, agencyIds);
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

        #region UAT-2774
        public static List<SharedUserInvitationDocumentContract> GetSharedUserInvitationDocumentDetails(Int32 ProfileSharingInvitationID, Int32 ApplicantOrgUserID, Boolean IsRotationSharing)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetSharedUserInvitationDocumentDetails(ProfileSharingInvitationID, ApplicantOrgUserID, IsRotationSharing);
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
        public static Boolean SaveSharedUserInvitationDocumentDetails(List<SharedUserInvitationDocumentMapping> lstSharedUserInvoitationDocs)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveSharedUserInvitationDocumentDetails(lstSharedUserInvoitationDocs);
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
        public static Boolean DeletedSharedUserInvitationDocument(Int32 InvitationDocumentID, Int32 ApplicantOrgUserID, Int32 ProfileSharingInvitationGroupID, Int32 CurrentLoggedInUserID)
        {
            try
            {
                Int32 SharedDocTypeID = GetDocumentTypeIdByCode(LKPSharedSystemDocumentTypes.SHARED_USER_INVITATION_DOCUMENT.GetStringValue());
                return BALUtils.GetProfileSharingRepoInstance().DeletedSharedUserInvitationDocument(InvitationDocumentID, ApplicantOrgUserID, ProfileSharingInvitationGroupID, SharedDocTypeID, CurrentLoggedInUserID);
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
        public static Boolean IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 ApplicantOrgUserID, Int32 ProfileSharingInvitationGroupID)
        {
            try
            {
                Int32 SharedDocTypeID = GetDocumentTypeIdByCode(LKPSharedSystemDocumentTypes.SHARED_USER_INVITATION_DOCUMENT.GetStringValue());
                return BALUtils.GetProfileSharingRepoInstance().IsDocumentAlreadyUploaded(documentName, documentSize, ApplicantOrgUserID, ProfileSharingInvitationGroupID, SharedDocTypeID);
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

        public static RequirementApprovalNotificationDocumentContract GetAgencySystemDocument(Int32 agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencySystemDocument(agencyID);
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

        public static void SendNotificationToApplicantOnRequirementApproved(List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData, String InvitataionStatusCode, Int32 CurrentLoggedInUserID, String CurrentUserName)
        {
            if (!lstRotationData.IsNullOrEmpty())
            {
                foreach (var item in lstRotationData)
                {
                    Int32 rotationID = item.Item1;
                    Int32 tenantID = item.Item2;
                    Int32 agencyID = item.Item3;
                    List<Int32> lstRotationInvitations = item.Item4;

                    if (!InvitataionStatusCode.IsNullOrEmpty() && (InvitataionStatusCode == SharedUserRotationReviewStatus.APPROVED.GetStringValue()))
                    {
                        RequirementApprovalNotificationDocumentContract approvalNotificationDoc = GetAgencySystemDocument(agencyID);

                        if (!approvalNotificationDoc.IsNullOrEmpty() && !string.IsNullOrEmpty(approvalNotificationDoc.DocumentPath))
                        {
                            var reviewStatus = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                             .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == InvitataionStatusCode).FirstOrDefault();
                            Int32 selectedInvitationReviewStatusId = 0;
                            String selectedInvitationReviewStatus = string.Empty;

                            if (!reviewStatus.IsNullOrEmpty())
                            {
                                selectedInvitationReviewStatusId = reviewStatus.SUIRS_ID;
                                selectedInvitationReviewStatus = reviewStatus.SUIRS_Name;
                            }

                            List<Int32> NewlstRotationInvitations = BALUtils.GetProfileSharingRepoInstance().GetInvitationIDsIfInvitationStatusChanged(lstRotationInvitations, selectedInvitationReviewStatusId);

                            if (!NewlstRotationInvitations.IsNullOrEmpty())
                            {
                                List<Int32> AppOrgUserIdOnInvitations = new List<Int32>();

                                if (!NewlstRotationInvitations.IsNullOrEmpty())
                                    AppOrgUserIdOnInvitations = BALUtils.GetProfileSharingRepoInstance().GetAppOrgOnInvitations(NewlstRotationInvitations);

                                Entity.ClientEntity.ClinicalRotation clinicalRotation = BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetClinicalRotation(rotationID);

                                Agency agency = BALUtils.GetProfileSharingRepoInstance().GetAgency(agencyID);
                                String TenantName = SecurityManager.GetTenant(tenantID).TenantName;
                                List<Entity.OrganizationUser> lstApplicantsinRotation = BALUtils.GetProfileSharingRepoInstance().GetClinicalRotationApplicantSharedData(rotationID, agencyID);



                                if (!AppOrgUserIdOnInvitations.IsNullOrEmpty())
                                {
                                    lstApplicantsinRotation = lstApplicantsinRotation.Where(cond => AppOrgUserIdOnInvitations.Contains(cond.OrganizationUserID)).ToList();
                                }

                                List<Entity.lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                                String docAttachmentTypeCode = DocumentAttachmentType.SYSTEM_DOCUMENT.GetStringValue();
                                Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                                    Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                                ClinicalRotationMemberDetail clinicalRotationMemberDetail = GetClinicalRotationMemberDetailContract(clinicalRotation, agency.AG_Name);

                                if (!lstApplicantsinRotation.IsNullOrEmpty())
                                {
                                    foreach (Entity.OrganizationUser applicant in lstApplicantsinRotation)
                                    {
                                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();

                                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                        mockData.EmailID = applicant.PrimaryEmailAddress;
                                        mockData.ReceiverOrganizationUserID = applicant.OrganizationUserID;
                                        mockData.UserName = string.Concat(applicant.FirstName, " ", applicant.LastName);

                                        dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateRotationDetailsHTML(clinicalRotationMemberDetail));
                                        dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(applicant.FirstName, " ", applicant.LastName));

                                        Entity.SystemCommunicationAttachment sysCommAttachment = new Entity.SystemCommunicationAttachment();
                                        sysCommAttachment.SCA_OriginalDocumentID = approvalNotificationDoc.ClientSystemDocumentID;
                                        sysCommAttachment.SCA_OriginalDocumentName = approvalNotificationDoc.FileName;
                                        sysCommAttachment.SCA_DocumentPath = approvalNotificationDoc.DocumentPath;
                                        sysCommAttachment.SCA_DocumentSize = approvalNotificationDoc.Size.HasValue ? approvalNotificationDoc.Size.Value : 0;
                                        sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                        sysCommAttachment.SCA_TenantID = tenantID;
                                        sysCommAttachment.SCA_IsDeleted = false;
                                        sysCommAttachment.SCA_CreatedBy = CurrentLoggedInUserID;
                                        sysCommAttachment.SCA_CreatedOn = DateTime.Now;

                                        //Sending an email
                                        Int32? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_REQUIREMENT_APPROVAL, dictMailData, mockData, tenantID, -1, null, null, true, true, null, clinicalRotationMemberDetail.RotationHirarchyIds, rotationID);

                                        if (systemCommunicationID.HasValue && systemCommunicationID.Value > 0)
                                        {
                                            //Save Mail Attachment
                                            sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                            Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                        }

                                        //Sending an internal message
                                        CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_REQUIREMENT_APPROVAL, dictMailData, applicant.OrganizationUserID, tenantID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static ClinicalRotationMemberDetail GetClinicalRotationMemberDetailContract(Entity.ClientEntity.ClinicalRotation clinicalRotation, string agencyName)
        {
            ClinicalRotationMemberDetail clinicalRotationMemberDetailContract = new ClinicalRotationMemberDetail();
            clinicalRotationMemberDetailContract.AgencyName = agencyName;
            clinicalRotationMemberDetailContract.ComplioID = clinicalRotation.CR_ComplioID;
            clinicalRotationMemberDetailContract.RotationName = clinicalRotation.CR_RotationName;
            clinicalRotationMemberDetailContract.Department = clinicalRotation.CR_Department;
            clinicalRotationMemberDetailContract.Program = clinicalRotation.CR_Program;
            clinicalRotationMemberDetailContract.Course = clinicalRotation.CR_Course;
            clinicalRotationMemberDetailContract.Term = clinicalRotation.CR_Term;
            clinicalRotationMemberDetailContract.TypeSpecialty = clinicalRotation.CR_TypeSpecialty;
            clinicalRotationMemberDetailContract.UnitFloorLoc = clinicalRotation.CR_UnitFloorLoc;
            //clinicalRotationMemberDetailContract.RecommendedHours=clinicalRotation.CR_NoOfHours;
            //clinicalRotationMemberDetailContract.DaysName=clinicalRotation.da
            clinicalRotationMemberDetailContract.Shift = clinicalRotation.CR_RotationShift;
            //clinicalRotationMemberDetailContract.Time=clinicalRotation.CR_StartTime.HasValue
            clinicalRotationMemberDetailContract.StartDate = clinicalRotation.CR_StartDate;
            clinicalRotationMemberDetailContract.EndDate = clinicalRotation.CR_EndDate;

            #region UAT-3254
            List<Int32> lstHierarchyIds = clinicalRotation.ClinicalRotationHierarchyMappings.Where(cond => !cond.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
            if (!lstHierarchyIds.IsNullOrEmpty())
            {
                clinicalRotationMemberDetailContract.RotationHirarchyIds = String.Join(",", lstHierarchyIds);
            }
            #endregion

            return clinicalRotationMemberDetailContract;
        }

        public static List<RequirementSharesDataContract> GetApprovedRotationsAfterSinceLastLogin(Int32 applicantOrgUserID, Int32 tenantID, DateTime lastLoginDate)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetApprovedRotationsAfterSinceLastLogin(applicantOrgUserID, tenantID, lastLoginDate);
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

        #region UAT-2510
        public static Boolean GetAgencyUserSSN_Permission(String userID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserSSN_Permission(userID);
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

        #region UAT-2784
        public static String GetAgencySetting(Int32 agencyId, String settingTypeCode)
        {
            try
            {
                Int32 settingTypeId = LookupManager.GetSharedDBLookUpData<lkpAgencyHierarchySetting>().Where(cond => !cond.S_IsDeleted && cond.S_Code == settingTypeCode).FirstOrDefault().S_ID;
                return BALUtils.GetProfileSharingRepoInstance().GetAgencySetting(agencyId, settingTypeId);
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
        public static Boolean CheckExpirationCriteriaForRotation(List<Int32> lstAgencyIds)
        {
            try
            {
                Int32 settingTypeId = LookupManager.GetSharedDBLookUpData<lkpAgencyHierarchySetting>().Where(cond => !cond.S_IsDeleted && cond.S_Code == AgencyHierarchySettingType.EXPIRATION_CRITERIA.GetStringValue()).FirstOrDefault().S_ID;
                return BALUtils.GetProfileSharingRepoInstance().CheckExpirationCriteriaForRotation(lstAgencyIds, settingTypeId);
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

        #region [UAT-2735]
        public static List<String> FilterApplicantHavingOnlyNonActiveOrExpireOrders(Int32 tenantID, String delimittedCRMIDs)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).FilterApplicantHavingOnlyNonActiveOrExpireOrders(delimittedCRMIDs);
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

        #region UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        public static List<lkpAgencyUserNotification> GetAgencyUserNotifications()
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserNotifications();
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

        public static Boolean SaveAgencyUserNotificationMappings(Int32 agencyUserID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInOrgUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveAgencyUserNotificationMappings(agencyUserID, dicNotificationData, CurrentLoggedInOrgUserID);
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
        public static void DeleteAgencyUserNotificationMappings(Int32 AgencyUserID, Int32 CurrentLoggedInUserId)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().DeleteAgencyUserNotificationMappings(AgencyUserID, CurrentLoggedInUserId);
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
        public static Boolean UpdateAgencyUserNotificationMappings(Int32 AgencyUserID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().UpdateAgencyUserNotificationMappings(AgencyUserID, dicNotificationData, CurrentLoggedInUserId);
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

        public static List<Int32> GetAgencyUserNotificationPermission(List<Int32> lstAgencyUserIds, String AgencyUserNotificationCode)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserNotificationPermission(lstAgencyUserIds, AgencyUserNotificationCode);
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
        public static Int32 GetAgencyUserNotificationPermissionThroughEmailID(String emailID, String AgencyUserNotificationCode)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserNotificationPermissionThroughEmailID(emailID, AgencyUserNotificationCode);
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
        public static Boolean IsOrgUserhaveNotificationPermission(Int32 orgUserID, String agencyUserNotificationCode)
        {
            try
            {
                bool isSendMail = BALUtils.GetProfileSharingRepoInstance().IsOrgUserhaveNotificationPermission(orgUserID, agencyUserNotificationCode);
                return isSendMail;
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

        #region UAT-2942
        public static void SendMailForApprovedApplicantProfileInvitation(List<Int32> lstSelectedInvitationIds, String InvitataionStatusCode, Int32 CurrentLoggedInUserID, String CurrentUserName)
        {
            if (!InvitataionStatusCode.IsNullOrEmpty() && (InvitataionStatusCode == SharedUserRotationReviewStatus.APPROVED.GetStringValue()))
            {
                Int32 selectedInvitationReviewStatusId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                 .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == InvitataionStatusCode).First().SUIRS_ID;

                String selectedInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                 .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == InvitataionStatusCode).First().SUIRS_Name;

                Int32 applicantInvitationTypeId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpInvitationSource>()
                                                                 .Where(cond => !cond.IsDeleted && cond.Code == AppConsts.APPLICANT_INVITATION_SOURCE_TYPE_CODE).First().InvitationSourceID;

                List<ApprovedProfileSharingEmailContract> NewlstProfileInvitations = BALUtils.GetProfileSharingRepoInstance().GetApprovedProfileInvitationDetailsByIds(lstSelectedInvitationIds, selectedInvitationReviewStatusId, applicantInvitationTypeId);
                if (!NewlstProfileInvitations.IsNullOrEmpty() && NewlstProfileInvitations.Count > AppConsts.NONE)
                {
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();

                    if (!NewlstProfileInvitations.IsNullOrEmpty())
                    {
                        foreach (var item in NewlstProfileInvitations)
                        {
                            List<CommunicationTemplateContract> lstcommunicationTemplateContract = new List<CommunicationTemplateContract>();

                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                            dictMailData.Add(EmailFieldConstants.STUDENT_FIRST, item.ApplicantDetails.FirstName);
                            dictMailData.Add(EmailFieldConstants.STUDENT_LAST, item.ApplicantDetails.LastName);
                            dictMailData.Add(EmailFieldConstants.INVITATION_STATUS, selectedInvitationReviewStatus);
                            dictMailData.Add(EmailFieldConstants.AGENCY_NAME, item.AgencyDetails.AG_Name);
                            String TenantName = SecurityManager.GetTenant(item.TenantID).TenantName;
                            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, TenantName);
                            dictMailData.Add(EmailFieldConstants.AGENCY_USER_NAME, CurrentUserName);
                            if (!item.RotationDetails.IsNullOrEmpty())
                            {
                                dictMailData.Add(EmailFieldConstants.ROTATION_START_DATE, item.RotationDetails.CR_StartDate.HasValue ? item.RotationDetails.CR_StartDate.Value.ToString("MM/dd/yyyy") : String.Empty);
                                dictMailData.Add(EmailFieldConstants.ROTATION_END_DATE, item.RotationDetails.CR_EndDate.HasValue ? item.RotationDetails.CR_EndDate.Value.ToString("MM/dd/yyyy") : String.Empty);
                                dictMailData.Add(EmailFieldConstants.DEPARTMENT_NAME, !String.IsNullOrEmpty(item.RotationDetails.CR_Department) ? item.RotationDetails.CR_Department : String.Empty);
                            }
                            else
                            {
                                dictMailData.Add(EmailFieldConstants.ROTATION_START_DATE, String.Empty);
                                dictMailData.Add(EmailFieldConstants.ROTATION_END_DATE, String.Empty);
                                dictMailData.Add(EmailFieldConstants.DEPARTMENT_NAME, String.Empty);
                            }

                            foreach (Entity.OrganizationUser OrgUSer in item.GetAgencyUserList)
                            {
                                CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
                                communicationTemplateContract.ReceiverOrganizationUserId = OrgUSer.OrganizationUserID;
                                communicationTemplateContract.RecieverEmailID = OrgUSer.PrimaryEmailAddress;
                                communicationTemplateContract.RecieverName = OrgUSer.FirstName + OrgUSer.LastName;
                                communicationTemplateContract.CurrentUserId = CurrentLoggedInUserID;
                                lstcommunicationTemplateContract.Add(communicationTemplateContract);
                            }

                            #region UAT-3254
                            String SelectedHirarchyIds = BALUtils.GetProfileSharingClientRepoInstance(item.TenantID).GetSharedSubscriptionsSelectedNodeIds(item.ProfileSharingInvitationID);
                            #endregion
                            //UAt-3364
                            Int32? rotationID = item.RotationDetails.IsNullOrEmpty() ? AppConsts.NONE : item.RotationDetails.CR_ID;
                            CommunicationManager.SendRotInvitationAppRejMail(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION, mockData, dictMailData, item.TenantID, lstcommunicationTemplateContract, SelectedHirarchyIds, rotationID);
                        }
                    }
                }
            }


        }
        #endregion

        #region UAT-2943
        public static Int32 GetReviewStatusIDByProfileSharingInvitationID(Int32 invitationId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetReviewStatusIDByProfileSharingInvitationID(invitationId);
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


        #region UAT-3051
        public static Boolean GetIsAgencySharePermissions(String emailId)
        {
            try
            {
                AgencyUser agencyUser = BALUtils.GetProfileSharingRepoInstance().IsEmailAlreadyExistAgencyUser(emailId);
                if (!agencyUser.IsNullOrEmpty())
                    return agencyUser.AGU_DoNotShowNonAgencyShares;
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

        public static DataSet GetAgencyRotationMapping()
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyRotationMapping();
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


        public static DataTable GetUpdatedRotationItems(Int32 tenantId, Int32 agencyID, String rotationIds, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetUpdatedRotationItems(agencyID, rotationIds, fromDate, toDate);
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

        public static Boolean SaveAgencyNotification(Int32 subEventID, String entityName, Int32 entityID, DateTime dataFetchedFromDate, DateTime dataFetchedFromToDate, Int32 createdByID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveAgencyNotification(subEventID, entityName, entityID, dataFetchedFromDate, dataFetchedFromToDate, createdByID);
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

        public static DateTime? GetLastNotificationSentDate(Int32 subEventID, Int32 entityID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetLastNotificationSentDate(subEventID, entityID);
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

        public static List<Tuple<int, List<int>>> FilterInvitationIdsByTenant(List<int> lstPSI)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().FilterInvitationIdsByTenant(lstPSI);
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

        public static Int32 GetInvitationReviewStatusIDByStatusCode(string reviewStatusCode)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationReviewStatusIDByStatusCode(reviewStatusCode);
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

        public static List<Int32> GetInvitationIDsIfInvitationStatusChanged(List<Int32> lstRotationInvitations, Int32 selectedInvitationReviewStatusId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationIDsIfInvitationStatusChanged(lstRotationInvitations, selectedInvitationReviewStatusId);
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

        public static List<Int32> FilterInvitationIdsByReviewStatusID(List<Int32> lstInvitations, Int32 reviewStatusID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().FilterInvitationIdsByReviewStatusID(lstInvitations, reviewStatusID);
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
        public static Boolean UpdateAgencyUserEmailAddress(Int32 agencyUserId, String emailId, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().UpdateAgencyUserEmailAddress(agencyUserId, emailId, loggedInUserId);
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

        public static Boolean SendNotificationToAgencyUserChangeInEmailAddress(AgencyUserContract _agencyUser, Int32 loggedInUserID, Int32 agencyUserID, String applicationUrl)
        {

            List<String> subEventCodes = new List<String>();
            subEventCodes.Add(CommunicationSubEvents.NOTIFICATION_FOR_AGENCY_USER_EMAIL_ADDRESS_CHANGES.GetStringValue().ToLower());
            Int32 subEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
            List<Entity.CommunicationTemplatePlaceHolder> placeHoldersToFetch = CommunicationManager.GetTemplatePlaceHolders(subEventID);
            Int32 templateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(subEventID);
            //Contains info for mail subject and content
            SystemEventTemplatesContract systemEventTemplate = TemplatesManager.GetTemplateDetails(templateId);

            Dictionary<String, String> dictMailData = new Dictionary<string, String>();
            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, _agencyUser.AGU_Name);
            dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
            dictMailData.Add(EmailFieldConstants.NEW_EMAIL_ADDRESS, _agencyUser.AGU_Email);

            Entity.SystemCommunication systemCommunication = new Entity.SystemCommunication();
            systemCommunication.SenderName = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME];
            systemCommunication.SenderEmailID = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];
            systemCommunication.Subject = systemEventTemplate.Subject;
            systemCommunication.CommunicationSubEventID = subEventID;
            systemCommunication.CreatedByID = loggedInUserID;
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
            systemCommunicationDelivery.ReceiverOrganizationUserID = agencyUserID;
            systemCommunicationDelivery.RecieverEmailID = _agencyUser.AGU_Email;
            systemCommunicationDelivery.RecieverName = _agencyUser.AGU_Name;
            systemCommunicationDelivery.IsDispatched = false;
            systemCommunicationDelivery.IsCC = null;
            systemCommunicationDelivery.IsBCC = null;
            systemCommunicationDelivery.CreatedByID = systemCommunication.CreatedByID;
            systemCommunicationDelivery.CreatedOn = DateTime.Now;
            systemCommunication.SystemCommunicationDeliveries.Add(systemCommunicationDelivery);

            List<Entity.SystemCommunication> lstSystemCommunicationToBeSaved = new List<Entity.SystemCommunication>();
            lstSystemCommunicationToBeSaved.Add(systemCommunication);
            return BALUtils.GetCommunicationRepoInstance().SaveSysCommunicationAndSysDeliveries(lstSystemCommunicationToBeSaved);
        }

        #region UAT-3238
        public static List<ApplicantDocumentDetailContarct> GetUnifiedDocument(Int32 tenantId, Int32 currentLoggedInUserId, List<ApplicantDocumentDetailContarct> agencyUserUnifiedDocumentContractList)
        {
            try
            {
                #region Declaring Variables
                List<INTSOF.Utils.CommonPocoClasses.ApplicantDocumentPocoClass> lstApplicantDoc = new List<INTSOF.Utils.CommonPocoClasses.ApplicantDocumentPocoClass>();
                List<Int32> documentIds = new List<Int32>();
                var finalApplicantDocList = new List<INTSOF.Utils.CommonPocoClasses.ApplicantDocumentPocoClass>();
                #endregion

                //Extract Document Ids
                agencyUserUnifiedDocumentContractList.ForEach(d => documentIds.Add(d.ApplicantDocumentID));
                //Get Document Details
                List<ApplicantDocument> lstAppDocuments = BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetApplicantDocumentDetailsByDocumentIds(documentIds);

                #region Convert Non PDF files into pdf and get pdf path
                var nonPdfFiles = lstAppDocuments.Where(d => !d.DocumentPath.Contains(".pdf") && String.IsNullOrEmpty(d.PdfDocPath)).ToList();
                nonPdfFiles.ForEach(s => lstApplicantDoc.Add(new INTSOF.Utils.CommonPocoClasses.ApplicantDocumentPocoClass()
                {
                    ApplicantDocumentID = s.ApplicantDocumentID,
                    DocumentPath = s.DocumentPath
                }));

                var convertedFileData = DocumentManager.GetConvertApplicantDocumentToPDF(lstApplicantDoc, tenantId, currentLoggedInUserId);
                #endregion

                //Added Converted file into main list
                finalApplicantDocList.AddRange(convertedFileData);

                var pdfFiles = lstAppDocuments.Where(d => d.DocumentPath.Contains(".pdf") || !String.IsNullOrEmpty(d.PdfDocPath)).ToList();
                //Added PDF file into main list
                pdfFiles.ForEach(d => finalApplicantDocList.Add(new INTSOF.Utils.CommonPocoClasses.ApplicantDocumentPocoClass()
                {
                    DocumentPath = d.DocumentPath,
                    PdfDocPath = d.PdfDocPath,
                    ApplicantDocumentID = d.ApplicantDocumentID
                }));

                #region Getting PDF total page numbers
                List<ApplicantDocumentToBeMerged> applicantDocumentToBeMergedList = new List<ApplicantDocumentToBeMerged>();
                //finalApplicantDocList.ForEach(s => applicantDocumentToBeMergedList.Add(new ApplicantDocumentToBeMerged()
                //{
                //    ApplicantDocumentID = s.ApplicantDocumentID,
                //    PdfDocPath = s.PdfDocPath
                //}));
                //var documentDetailList = DocumentManager.GetApplicantDocumentsList(applicantDocumentToBeMergedList, tenantId);
                #endregion
                #region Create Unified Document On the fly
                foreach (var row in agencyUserUnifiedDocumentContractList)
                {
                    //foreach (var row in item.ApplicantDocumentDetailContarctList)
                    //{
                    var applicantDocumentToBeMerged = new ApplicantDocumentToBeMerged();
                    applicantDocumentToBeMerged.ApplicantDocumentID = row.ApplicantDocumentID;
                    applicantDocumentToBeMerged.PdfDocPath = finalApplicantDocList.Where(d => d.ApplicantDocumentID == row.ApplicantDocumentID).FirstOrDefault().PdfDocPath;
                    if (!String.IsNullOrEmpty(applicantDocumentToBeMerged.PdfDocPath))
                        applicantDocumentToBeMergedList.Add(applicantDocumentToBeMerged);
                    // }
                }
                var documentDetailList = DocumentManager.CreateUnifiedDocumentForAgencyUser(applicantDocumentToBeMergedList, tenantId);
                String unifiedDocumentPath = documentDetailList.Item1;
                #endregion
                #region Binding Final result set

                List<AgencyUserUnifiedDocumentContract> result = new List<AgencyUserUnifiedDocumentContract>();
                //    Int32 PageStartIndex = 1;
                Int32 PageEndIndex = 0;
                Int32 DocumentIndex = 0;
                //foreach (var item in agencyUserUnifiedDocumentContractList)
                //{
                // AgencyUserUnifiedDocumentContract agencyUserUnifiedDocumentContract = new AgencyUserUnifiedDocumentContract();
                // agencyUserUnifiedDocumentContract.PkgSubscriptionID = item.PkgSubscriptionID;
                // agencyUserUnifiedDocumentContract.PkgSubscriptionStartIndex = PageStartIndex;
                List<ApplicantDocumentDetailContarct> applicantDocumentDetailContarctList = new List<ApplicantDocumentDetailContarct>();
                //foreach (var row in item.ApplicantDocumentDetailContarctList.OrderBy(d => d.ApplicantDocumentID))
                foreach (var row in agencyUserUnifiedDocumentContractList)
                {
                    var documentDetails = documentDetailList.Item2.Where(f => f.ApplicantDocumentID == row.ApplicantDocumentID).FirstOrDefault();
                    if (!documentDetails.IsNullOrEmpty())
                    {
                        applicantDocumentDetailContarctList.Add(new ApplicantDocumentDetailContarct()
                        {
                            ApplicantDocumentID = row.ApplicantDocumentID,
                            DocumentPdfPath = documentDetails.PdfDocPath,
                            TotalPages = documentDetails.TotalPages - PageEndIndex,
                            ApplicantDocumentDetailContarctUnifiedDocumentPath = unifiedDocumentPath,
                            StartIndex = DocumentIndex == AppConsts.NONE ? 1 : DocumentIndex
                        });
                        PageEndIndex = documentDetails.TotalPages.Value;
                        DocumentIndex = documentDetails.TotalPages.Value + 1;
                    }
                }
                //  agencyUserUnifiedDocumentContract.PkgSubscriptionEndIndex = PageEndIndex;
                // agencyUserUnifiedDocumentContract.UnifiedDocumentPath = unifiedDocumentPath;
                // agencyUserUnifiedDocumentContract.ApplicantDocumentDetailContarctList = applicantDocumentDetailContarctList;
                // result.Add(agencyUserUnifiedDocumentContract);
                // PageStartIndex = PageEndIndex + 1;
                // }

                #endregion

                return applicantDocumentDetailContarctList;
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

        #region UAT-3338
        public static ProfileSharingInvitationGroup GetProfileSharingGroupData(Int32 agencyId, Int32 clinicalRotationId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetProfileSharingGroupData(agencyId, clinicalRotationId);
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

        #region UAT-3400
        /// <summary>
        /// Method to Insert an entry Shared User rotation Review for Non-Registered Users
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        public static Boolean InsertSharedUserRotReviewForNonRegUser(String profileSharingInvIds, Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().InsertSharedUserRotReviewForNonRegUser(profileSharingInvIds, organizationUserId);
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

        #region UAT 3294
        public static List<Entity.SharedDataEntity.AgencyUser> GetAgencyUserByAgencIds(List<Int32> agencyHiearchyIDs)
        {
            return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserByAgencIds(agencyHiearchyIDs);
        }

        public static Boolean IsApplicantSendInvitationToAgencyUser(Guid agencyUserId)
        {
            return BALUtils.GetProfileSharingRepoInstance().IsApplicantSendInvitationToAgencyUser(agencyUserId);
        }
        public static Boolean MoveApplicantEmailShareToAgencyUser(Guid fromAgencyUserId, Int32 tenantID, Guid toAgencyUserID, Int32 currentLoggedInUserI)
        {
            return BALUtils.GetProfileSharingRepoInstance().MoveApplicantEmailShareToAgencyUser(fromAgencyUserId, tenantID, toAgencyUserID, currentLoggedInUserI);
        }
        #endregion

        #region UAT-3360
        public static ClientContact IsInstructorPreceptorUser(String emailId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsInstructorPreceptorUser(emailId);
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

        public static Boolean IsAgencyUserExist(List<String> userEmails)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().IsAgencyUserExist(userEmails);
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

        #region UAT-3316:Ability to create Agency User permission "templates"
        public static Tuple<List<AgencyUserPermissionTemplateContract>, Int32, Int32, List<AgencyUserPermissionTemplateMappingContract>, List<AgencyUserPermissionTemplateNotificationsContract>> GetlstAgencyUserPermissionTemplate(AgencyUserPermissionTemplateContract searchDataContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetlstAgencyUserPermissionTemplate(searchDataContract, customPagingArgsContract);
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

        public static Int32 SaveAgencyUserPerTemplate(int tenantID, AgencyUserPermissionTemplateContract _agencyUserPerTemplate, Int32 loggedInUserID, List<AgencyUserPermissionTemplateMapping> lstAgencyUserPerTempMapping)
        {
            try
            {
                Int32 agencyUserID = BALUtils.GetProfileSharingRepoInstance().SaveAgencyUserPerTemplate(_agencyUserPerTemplate, loggedInUserID, lstAgencyUserPerTempMapping);

                return agencyUserID;
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

        public static Boolean SaveAgencyUserTemplateNotificationMappings(Int32 agencyUserTemplateID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInOrgUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveAgencyUserTemplateNotificationMappings(agencyUserTemplateID, dicNotificationData, CurrentLoggedInOrgUserID);
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

        public static string DeleteAgencyUserPermissionTemplate(Int32 tenantID, Int32 AGUPT_ID, Int32 LoggedInUserId, Boolean IsAdmin)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().DeleteAgencyUserPermissionTemplate(tenantID, AGUPT_ID, LoggedInUserId, IsAdmin);
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

        public static void DeleteAgencyUserPerTemplateNotificationMappings(Int32 AgencyUserTemplateID, Int32 CurrentLoggedInUserId)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().DeleteAgencyUserPerTemplateNotificationMappings(AgencyUserTemplateID, CurrentLoggedInUserId);
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

        public static AgencyUserPermissionTemplate UpdateAgencyUserPermissionTemplate(Int32 tenantID, AgencyUserPermissionTemplateContract _agencyUser, Int32 LoggedInUserId, Boolean IsAdmin, List<AgencyUserPermissionTemplateMapping> lstAgencyUserPermissionTemplate)
        {
            try
            {
                AgencyUserPermissionTemplate agencyUser = BALUtils.GetProfileSharingRepoInstance().UpdateAgencyUserPermissionTemplate(_agencyUser, LoggedInUserId, IsAdmin, lstAgencyUserPermissionTemplate);

                return agencyUser;
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
        public static Boolean UpdateAgencyUserPerTemplateNotificationMappings(Int32 AgencyUserTemplateID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().UpdateAgencyUserPerTemplateNotificationMappings(AgencyUserTemplateID, dicNotificationData, CurrentLoggedInUserId);
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



        public static List<AgencyUserPermissionTemplate> GetAgencyUserPermissionTemplates()
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserPermissionTemplates();
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
        public static void DeleteAgencyUserTemplateNotificationMappings(Int32 AgencyUserTemplateId, Int32 CurrentLoggedInUserId)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().DeleteAgencyUserTemplateNotificationMappings(AgencyUserTemplateId, CurrentLoggedInUserId);
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
        public static List<AgencyUserPermissionTemplateMapping> GetAgencyUsrPerTemplateMappings(Int32 permisisonTemplateId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUsrPerTemplateMappings(permisisonTemplateId);
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
        public static List<AgencyUserPerTemplateNotificationMapping> GetAgencyUsrPerTemplateNotificationsMappings(Int32 permisisonTemplateId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUsrPerTemplateNotificationsMappings(permisisonTemplateId);
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
        public static List<lkpAgencyUserPermissionType> GetAgencyUserPermissionTypes()
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserPermissionTypes();
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

        public static List<Int32> GetInvitationSharedInfoTypeID(Int32 templateID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetInvitationSharedInfoTypeID(templateID);
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
        public static List<Int32?> GetApplicationInvitationMetaDataID(Int32 templateID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetApplicationInvitationMetaDataID(templateID);
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
        public static List<AgencyUserPermission> GetAgencyUsrPermisisonMappings(Int32 userId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUsrPermisisonMappings(userId);
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

        #region UAT-3470
        public static List<Entity.SharedDataEntity.lkpInvitationArchiveState> GetinvitationArchiveStateList()
        {
            try
            {
                String activeStatusCode = InvitationArchiveState.Active.GetStringValue();
                String archiveStatusCode = InvitationArchiveState.Archived.GetStringValue();
                List<Entity.SharedDataEntity.lkpInvitationArchiveState> lkpInvitationArchiveStateList = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpInvitationArchiveState>().Where(x => x.LIAS_IsDeleted == false
                                                                                                                                     && (x.LIAS_Code == activeStatusCode || x.LIAS_Code == archiveStatusCode))
                                                                                                                                     .ToList();
                Entity.SharedDataEntity.lkpInvitationArchiveState lkpInvitationArchiveStateRow = new Entity.SharedDataEntity.lkpInvitationArchiveState();
                lkpInvitationArchiveStateRow.LIAS_Name = InvitationArchiveState.All.ToString();
                lkpInvitationArchiveStateRow.LIAS_Code = InvitationArchiveState.All.GetStringValue();
                lkpInvitationArchiveStateList.Add(lkpInvitationArchiveStateRow);
                return lkpInvitationArchiveStateList;
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

        public static bool SaveUpdateInvitationArchiveState(String InvitationIds, String rotationContract, Int32 CurrentLoggedInUser, Boolean IsPerformArchiveOperation)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveUpdateInvitationArchiveState(InvitationIds, rotationContract
                                                                              , CurrentLoggedInUser, IsPerformArchiveOperation);
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

        public static String CreateRotationContractXml(List<Tuple<Int32, Int32, Int32, List<Int32>>> input)
        {
            String rotationContractXml = String.Empty;

            foreach (Tuple<Int32, Int32, Int32, List<Int32>> item in input)
            {

                XmlDocument xml = new XmlDocument();
                XmlElement root = xml.CreateElement("RotationContract");
                xml.AppendChild(root);

                XmlElement CR_ID = xml.CreateElement("CR_ID");
                CR_ID.InnerText = item.Item1.ToString();
                root.AppendChild(CR_ID);

                XmlElement TenantID = xml.CreateElement("TenantID");
                TenantID.InnerText = item.Item2.ToString();
                root.AppendChild(TenantID);

                rotationContractXml += xml.OuterXml;
                //lstRotationIds.Add(String.Concat(item.Item1.ToString(), "-", item.Item2.ToString()));
            }
            return rotationContractXml;
        }
        #endregion

        #region UAT-3273
        public static void CheckAndSendNotificationForNewlyApprovedRotations(DataTable dataBeforeRuleExecution, DataTable dataAfterRuleExecution, Int32 tenantID)
        {
            try
            {
                #region Convert Data table to proxy contract
                var dataBeforeRuleExecutionList = ConvertDataTableToProxyContract(dataBeforeRuleExecution);
                var dataAfterRuleExecutionList = ConvertDataTableToProxyContract(dataAfterRuleExecution);
                #endregion

                if (dataAfterRuleExecutionList != null && dataBeforeRuleExecutionList != null && dataAfterRuleExecutionList.Count() > dataBeforeRuleExecutionList.Count())
                {
                    var newlyApprovedRotations = dataAfterRuleExecutionList.Where(x => !dataBeforeRuleExecutionList.Where(y => y.CR_ID == x.CR_ID).Any());
                    if (newlyApprovedRotations != null && newlyApprovedRotations.Count() > 0)
                    {
                        foreach (var item in newlyApprovedRotations)
                        {
                            Entity.ClientEntity.ClinicalRotation clinicalRotation = BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetClinicalRotation(item.CR_ID);

                            Agency agency = BALUtils.GetProfileSharingRepoInstance().GetAgency(item.CRA_AgencyID);
                            String TenantName = SecurityManager.GetTenant(tenantID).TenantName;

                            ClinicalRotationMemberDetail clinicalRotationMemberDetail = GetClinicalRotationMemberDetailContract(clinicalRotation, agency.AG_Name);
                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();

                            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();

                            mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                            mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                            mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, clinicalRotation.CR_RotationName);
                            dictMailData.Add(EmailFieldConstants.COMPLIO_ID, clinicalRotation.CR_ComplioID);
                            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, clinicalRotationMemberDetail.SchoolContactName);
                            dictMailData.Add(EmailFieldConstants.ROTATION_PACKAGE_NAME, clinicalRotationMemberDetail.RotationName);
                            dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateRotationDetailsHTML(clinicalRotationMemberDetail));

                            //Sending an email
                            Int32? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_TO_ADMINS_WHEN_ALL_APPLICANTS_IN_A_ROTATION_BECOMES_AGENCY_COMPLIANT, dictMailData, mockData, tenantID, -1, null, null, true, true, null, item.HierarchyNodeIDs, item.CR_ID);

                            //Sending an internal message
                            CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_TO_ADMINS_WHEN_ALL_APPLICANTS_IN_A_ROTATION_BECOMES_AGENCY_COMPLIANT, dictMailData, -1, tenantID);
                        }
                    }
                }
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

        private static List<RequirementPackageSubscriptionApprovedContract> ConvertDataTableToProxyContract(DataTable input)
        {
            List<RequirementPackageSubscriptionApprovedContract> result = new List<RequirementPackageSubscriptionApprovedContract>();
            result = INTSOF.Utils.Extensions.ConvertDataTableToList<RequirementPackageSubscriptionApprovedContract>(input);
            return result;
        }
        #endregion

        #region UAT-3353

        public static List<SharedUserInvitationDocumentContract> GetSharedUserRotationInvitationDocumentDetails(Int32 tenantID, Int32 clinicalRotationID, Int32 agencyID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetSharedUserRotationInvitationDocumentDetails(tenantID, clinicalRotationID, agencyID);
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
        public static Boolean SaveSharedUserRotationInvitationDocumentDetails(List<SharedUserRotationInvitationDocumentMapping> lstSharedUserRotationInvitationDocumentMapping)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().SaveSharedUserRotationInvitationDocumentDetails(lstSharedUserRotationInvitationDocumentMapping);
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
        public static Boolean DeletedSharedUserRotationInvitationDocument(Int32 invitationDocumentID, Int32 tenantID, Int32 clinicalRotationID, Int32 agencyID, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().DeletedSharedUserRotationInvitationDocument(invitationDocumentID, tenantID, clinicalRotationID, agencyID, currentLoggedInUserID);
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
        public static Boolean IsRotationInvitationDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 tenantID, Int32 clinicalRotationID, Int32 SharedDocTypeID)
        {
            try
            {
                Int32 SharedRotationDocTypeID = GetDocumentTypeIdByCode(LKPSharedSystemDocumentTypes.SHARED_USER_INVITATION_DOCUMENT.GetStringValue());
                return BALUtils.GetProfileSharingRepoInstance().IsRotationInvitationDocumentAlreadyUploaded(documentName, documentSize, tenantID, clinicalRotationID, SharedRotationDocTypeID);
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
        public static Int32 GetProfileSharingGroupIDByClinicalRotationID(Int32 tenantID, Int32 clinicalRotationID)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetProfileSharingGroupIDByClinicalRotationID(tenantID, clinicalRotationID);
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
        #endregion

        #region UAT-3606
        public static void SendNotificationToApplicantOnNonRequirementApproved(List<Int32> lstSelectedInvitationIds, String InvitataionStatusCode, Int32 CurrentLoggedInUserID, String CurrentUserName)
        {
            if (!InvitataionStatusCode.IsNullOrEmpty() && (InvitataionStatusCode == SharedUserRotationReviewStatus.APPROVED.GetStringValue()) && (!lstSelectedInvitationIds.IsNullOrEmpty() && lstSelectedInvitationIds.Count > AppConsts.NONE))
            {
                Int32 selectedInvitationReviewStatusId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                 .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == InvitataionStatusCode).First().SUIRS_ID;
                String selectedInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>()
                                                                 .Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == InvitataionStatusCode).First().SUIRS_Name;
                Int32 applicantInvitationTypeId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpInvitationSource>()
                                                                 .Where(cond => !cond.IsDeleted && cond.Code == AppConsts.APPLICANT_INVITATION_SOURCE_TYPE_CODE).First().InvitationSourceID;
                List<ApprovedProfileSharingEmailContract> NewlstProfileInvitations = BALUtils.GetProfileSharingRepoInstance().GetApprovedProfileInvitationDetailsByIds(lstSelectedInvitationIds, selectedInvitationReviewStatusId, applicantInvitationTypeId);
                if (!NewlstProfileInvitations.IsNullOrEmpty())
                {
                    int tenantID;
                    foreach (var selectedInvitationId in lstSelectedInvitationIds)
                    {
                        if (NewlstProfileInvitations.Where(x => x.ProfileSharingInvitationID == selectedInvitationId).Any())
                        {
                            tenantID = NewlstProfileInvitations.Where(x => x.ProfileSharingInvitationID == selectedInvitationId).Select(y => y.TenantID).FirstOrDefault();
                            var selectedInvitationIdList = new List<int>() { selectedInvitationId };
                            var appOrgUserIdOnInvitations = BALUtils.GetProfileSharingRepoInstance().GetAppOrgOnInvitations(selectedInvitationIdList);

                            String TenantName = SecurityManager.GetTenant(tenantID).TenantName;
                            List<Entity.OrganizationUser> lstApplicantsinRotation = BALUtils.GetProfileSharingRepoInstance().GetProfileSharingInvitationApplicantSharedData(selectedInvitationIdList);

                            if (!appOrgUserIdOnInvitations.IsNullOrEmpty())
                            {
                                lstApplicantsinRotation = lstApplicantsinRotation.Where(cond => appOrgUserIdOnInvitations.Contains(cond.OrganizationUserID)).ToList();

                                if (!lstApplicantsinRotation.IsNullOrEmpty())
                                {
                                    InvitationDetailsContract clinicalRotationMemberDetail = ProfileSharingManager.GetInvitationData(selectedInvitationId, tenantID);

                                    foreach (Entity.OrganizationUser applicant in lstApplicantsinRotation)
                                    {
                                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();

                                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                        mockData.EmailID = applicant.PrimaryEmailAddress;
                                        mockData.ReceiverOrganizationUserID = applicant.OrganizationUserID;
                                        mockData.UserName = string.Concat(applicant.FirstName, " ", applicant.LastName);

                                        dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateRotationDetailsHTML(clinicalRotationMemberDetail.RotationDetail));
                                        dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(applicant.FirstName, " ", applicant.LastName));

                                        String SelectedHirarchyIds = BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetSharedSubscriptionsSelectedNodeIds(selectedInvitationIdList.FirstOrDefault());

                                        //Sending an email
                                        Int32? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_REQUIREMENT_APPROVAL, dictMailData, mockData, tenantID, -1, null, null, true, true, null, SelectedHirarchyIds);

                                        //Sending an internal message
                                        CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_REQUIREMENT_APPROVAL, dictMailData, applicant.OrganizationUserID, tenantID);
                                    }
                                }
                            }
                        }

                    }

                }
            }
        }
        #endregion

        #region UAT-3719
        public static void SaveAgencyUserPermissionAuditDetails(Int32? AgencyUserId, Int32? AgencyUserTemplateID, Int32 CurrentLoggedInUserID)
        {
            try
            {
                BALUtils.GetProfileSharingRepoInstance().SaveAgencyUserPermissionAuditDetails(AgencyUserId, AgencyUserTemplateID, CurrentLoggedInUserID);
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

        #region UAT 3715
        public static void GenerateAttestationReport(List<InvitationSharedInfoDetails> lstInvitationSharedInfoDetails, Int32 generatedInvitationGroupId
                                                  , Boolean isRotationSharing, Int32 tenantID, Int32 currentUserID, Int32 agencyID, Int32 selectedRotationId = 0)
        {
            try
            {
                #region ATTESTATION REPORT CODE

                if (!lstInvitationSharedInfoDetails.IsNullOrEmpty())
                {
                    //var generatedInvitationGroupId = _lstInvitations.First().ProfileSharingInvitationGroup.PSIG_ID;

                    String sharedInfoPermission_RotationNone = SharedInfoType.REQUIREMENT_ROTATION_NONE.GetStringValue();
                    String sharedInfoPermission_ComplianceNone = SharedInfoType.COMPLIANCE_NONE.GetStringValue();
                    String sharedInfoPermission_FlagStatus = SharedInfoType.FLAG_STATUS.GetStringValue();

                    // Added as per UAT 1464 : Updated permissions to manage agency users to handle "Attestation only"
                    String bkgSharedInfoPermission_AttestationOnly = SharedInfoType.BACKGROUND_ATTESTATION_ONLY.GetStringValue();
                    String complianceSharedInfoPermission_AttestationOnly = SharedInfoType.COMPLIANCE_ATTESTATION_ONLY.GetStringValue();
                    String requirmentSharedInfoPermission_AttestationOnly = SharedInfoType.REQUIREMENT_ATTESTATION_ONLY.GetStringValue();

                    List<InvitationDocumentMapping> lstInvitationDocumentMapping = new List<InvitationDocumentMapping>();
                    List<InvAttestationDocWithPermissionType> lstInvAttestationDocWithPermissionType = new List<InvAttestationDocWithPermissionType>();


                    if (isRotationSharing)
                    {
                        #region [UAT-2821]

                        bool overrideAttestationReportWithForm = false;
                        string selfUploadedDocpath = string.Empty;

                        InvitationDocument uploadedInvDoc = BALUtils.GetProfileSharingRepoInstance().GetUploadedInvitationDocument(generatedInvitationGroupId, GetDocumentTypeIdByCode(LKPSharedSystemDocumentTypes.UPLOADED_INVITATION_DOCUMENT.GetStringValue()));

                        if (!uploadedInvDoc.IsNullOrEmpty())
                        {
                            overrideAttestationReportWithForm = true;
                            selfUploadedDocpath = uploadedInvDoc.IND_DocumentFilePath;
                        }

                        #endregion

                        #region Generate the Lists for the types of Attestations, based on the permissions
                        //UAT-2443
                        Int32 invitationDocId = AppConsts.NONE;
                        Boolean isDocMergingRequired = false;
                        String previousAttestationDocPath = String.Empty;
                        String singleAttestationDocPathToUpdate = String.Empty;

                        List<InvitationSharedInfoDetails> oldAttestation = BALUtils.GetProfileSharingRepoInstance().GetInvitationDocumentData(selectedRotationId, tenantID, agencyID);

                        List<InvitationAttestationDocumentDataWithAgencyUserPermissions> oldAttestationsWithPermissionCode = BALUtils.GetProfileSharingRepoInstance().
                                                                                                                                GetAttestationDocumentDetailsWithPermissionType(selectedRotationId, tenantID, agencyID);




                        // Screening + Tracking + Rotation
                        List<InvitationSharedInfoDetails> _lstAll = lstInvitationSharedInfoDetails
                                                                                .Where(isid => isid.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
                                                                                   && (isid.LstBkgSharedInfoTypeCode != null && isid.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                   && isid.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone).ToList();

                        // Screening + Tracking
                        List<InvitationSharedInfoDetails> _lstST = lstInvitationSharedInfoDetails
                                                                                .Where(isid => isid.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
                                                                                   && (isid.LstBkgSharedInfoTypeCode != null && isid.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                   && isid.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone).ToList();

                        // Tracking + Rotation
                        List<InvitationSharedInfoDetails> _lstTR = lstInvitationSharedInfoDetails
                                                                                .Where(isid => isid.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
                                                                                   && (isid.LstBkgSharedInfoTypeCode == null || isid.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                   && isid.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone).ToList();

                        // Screening + Rotation
                        List<InvitationSharedInfoDetails> _lstSR = lstInvitationSharedInfoDetails
                                                                                .Where(isid => isid.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
                                                                                   && (isid.LstBkgSharedInfoTypeCode != null && isid.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                   && isid.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone).ToList();

                        // Tracking Only
                        List<InvitationSharedInfoDetails> _lstT = lstInvitationSharedInfoDetails
                                                                                .Where(isid => isid.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
                                                                                   && (isid.LstBkgSharedInfoTypeCode == null || isid.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                   && isid.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone).ToList();

                        // Rotation Only
                        List<InvitationSharedInfoDetails> _lstR = lstInvitationSharedInfoDetails
                                                                                .Where(isid => isid.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
                                                                                   && (isid.LstBkgSharedInfoTypeCode == null || isid.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                   && isid.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone).ToList();

                        // Screening Only
                        List<InvitationSharedInfoDetails> _lstS = lstInvitationSharedInfoDetails
                                                                                .Where(isid => isid.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
                                                                                   && (isid.LstBkgSharedInfoTypeCode != null && isid.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                   && isid.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone).ToList();

                        // No Permission
                        List<InvitationSharedInfoDetails> _lstNone = lstInvitationSharedInfoDetails
                                                                                .Where(isid => isid.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
                                                                                   && (isid.LstBkgSharedInfoTypeCode == null || isid.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                   && isid.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone).ToList();
                        //UAT-3715
                        // _lstAll = _lstST = _lstTR = _lstSR = _lstT = _lstR = _lstS = _lstNone = lstInvitationSharedInfoDetails;

                        Boolean isAttestationFormMerged = false;
                        Int32 prevAttestationFormDocumentID = 0;
                        string singleAttestationFormDocPathToUpdate = string.Empty;
                        bool isEveryOneDocSaved = false;
                        InvitationDocument _doc_AttestationForm = new InvitationDocument();

                        if (overrideAttestationReportWithForm)
                        {
                            InvitationSharedInfoDetails prevAttestationForm = oldAttestation.Where(cond => cond.IsForEveryOneAttestationForm == true).FirstOrDefault();

                            _doc_AttestationForm = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID, true);

                            if (!prevAttestationForm.IsNullOrEmpty())
                            {
                                isEveryOneDocSaved = true;
                                isAttestationFormMerged = true;

                                prevAttestationFormDocumentID = prevAttestationForm.InvitationDocumentID;

                                singleAttestationFormDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_AttestationForm.IND_DocumentFilePath, prevAttestationForm.DocumentPath, currentUserID
                                                                                                      , AttestationDocumentTypes.ATTESTATION_FORM.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);

                                UpdateInvitationDocumentPath(prevAttestationFormDocumentID, singleAttestationFormDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }

                        #endregion

                        #region Handle Rotation sharing based scenarios
                        //if (!_lstAll.IsNullOrEmpty())
                        //{
                        #region All


                        List<InvitationSharedInfoDetails> lstScreeningWithColorFlag = _lstAll
                                                                                      .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                                || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
                                                                                      .ToList();

                        // Includes the Rotation, Tracking and Screening without Flag.
                        List<InvitationSharedInfoDetails> lstAllWithoutColorFlag = _lstAll
                                                                                   .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                               && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
                                                                                   .ToList();

                        //UAT-3715
                        //lstScreeningWithColorFlag = lstAllWithoutColorFlag = _lstAll;



                        //if (!lstScreeningWithColorFlag.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;
                        InvitationSharedInfoDetails AllAttestationWithColorFlag = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                                   && (con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                             || con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                                                                                             && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                                             && !con.IsForEveryOneAttestationForm).FirstOrDefault();


                        InvitationAttestationDocumentDataWithAgencyUserPermissions AllAttestationWithColorFlagWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.Full.GetStringValue()).FirstOrDefault();

                        if (!AllAttestationWithColorFlagWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = AllAttestationWithColorFlagWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = AllAttestationWithColorFlagWithPerm.DocumentPath;
                        }
                        else if (!AllAttestationWithColorFlag.IsNullOrEmpty())
                        {
                            invitationDocId = AllAttestationWithColorFlag.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = AllAttestationWithColorFlag.DocumentPath;
                        }

                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, true, AttestationDocumentTypes.FULL.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.Full.GetStringValue());

                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.Full.GetStringValue(),
                                prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;
                        }
                        else
                        {
                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);//, generatedInvitationGroupId, lkpAgencyUserAttestationPermissionsEnum.Full.GetStringValue());
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, true, AttestationDocumentTypes.FULL.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.FULL.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.Full.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }

                        //}
                        //if (!lstAllWithoutColorFlag.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails AllAttestationWithoutColorFlag = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
                                                                      && (!con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                && !con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                                                                && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions AllAttestationWithoutColorFlagWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.TRACKING_SCREENING_ROTATION_WITHOUT_FLAG.GetStringValue()).FirstOrDefault();

                        if (!AllAttestationWithoutColorFlagWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = AllAttestationWithoutColorFlagWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = AllAttestationWithoutColorFlagWithPerm.DocumentPath;
                        }
                        else
                            if (!AllAttestationWithoutColorFlag.IsNullOrEmpty())
                        {
                            invitationDocId = AllAttestationWithoutColorFlag.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = AllAttestationWithoutColorFlag.DocumentPath;
                        }


                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, false, AttestationDocumentTypes.TRACKING_SCREENING__ROTATION_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstAllWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_SCREENING_ROTATION_WITHOUT_FLAG.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstAllWithoutColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_SCREENING_ROTATION_WITHOUT_FLAG.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {
                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, true, false, AttestationDocumentTypes.TRACKING_SCREENING__ROTATION_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.TRACKING_SCREENING__ROTATION_WITHOUT_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstAllWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_SCREENING_ROTATION_WITHOUT_FLAG.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);

                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}
                        #endregion
                        //}

                        //if (!_lstST.IsNullOrEmpty())
                        //{

                        #region Screening + Tracking
                        List<InvitationSharedInfoDetails> lstScreeningWithColorFlags = _lstST
                                                                                      .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                                  || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
                                                                                      .ToList();

                        // Includes the Tracking + Screening (without Flag)
                        List<InvitationSharedInfoDetails> lstSTWithoutColorFlag = _lstST
                                                                                  .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                              && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
                                                                                  .ToList();

                        //if (!lstScreeningWithColorFlag.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails ScreeningTrackingAttestationWithColorFlag = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
                                          && (con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                    || con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                                    && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                    && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions ScreeningTrackingAttestationWithColorFlagWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_TRACKING.GetStringValue()).FirstOrDefault();

                        if (!ScreeningTrackingAttestationWithColorFlagWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningTrackingAttestationWithColorFlagWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningTrackingAttestationWithColorFlagWithPerm.DocumentPath;
                        }
                        else if (!ScreeningTrackingAttestationWithColorFlag.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningTrackingAttestationWithColorFlag.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningTrackingAttestationWithColorFlag.DocumentPath;
                        }


                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.SCREENING_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);//, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_TRACKING.GetStringValue());
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlags, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_TRACKING.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlags, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_TRACKING.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {
                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);//, generatedInvitationGroupId, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_TRACKING.GetStringValue());
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.SCREENING_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);//, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_TRACKING.GetStringValue());

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.SCREENING_AND_TRACKING.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlags, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_TRACKING.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}
                        //if (!lstSTWithoutColorFlag.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails ScreeningTrackingAttestationWithoutColorFlag = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
                                   && (!con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                    && !con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                                     && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                     && !con.IsForEveryOneAttestationForm).FirstOrDefault();
                        InvitationAttestationDocumentDataWithAgencyUserPermissions ScreeningTrackingAttestationWithoutColorFlagWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue()).FirstOrDefault();

                        if (!ScreeningTrackingAttestationWithoutColorFlagWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningTrackingAttestationWithoutColorFlagWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningTrackingAttestationWithoutColorFlagWithPerm.DocumentPath;
                        }
                        else if (!ScreeningTrackingAttestationWithoutColorFlag.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningTrackingAttestationWithoutColorFlag.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningTrackingAttestationWithoutColorFlag.DocumentPath;
                        }


                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);//, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue());
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSTWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue());
                        }


                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSTWithoutColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {
                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);//, generatedInvitationGroupId, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue());
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);//, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue());

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSTWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}
                        #endregion
                        //}

                        //if (!_lstTR.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails TrackingRotationAttestation = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode == null || con.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                         && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                                         && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions TrackingRotationAttestationWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_TRACKING.GetStringValue()).FirstOrDefault();

                        if (!TrackingRotationAttestationWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = TrackingRotationAttestationWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = TrackingRotationAttestationWithPerm.DocumentPath;
                        }
                        else if (!TrackingRotationAttestation.IsNullOrEmpty())
                        {
                            invitationDocId = TrackingRotationAttestation.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = TrackingRotationAttestation.DocumentPath;
                        }

                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, false, true, false, AttestationDocumentTypes.ROTATION_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstTR, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_TRACKING.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstTR, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_TRACKING.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {
                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, false, true, false, AttestationDocumentTypes.ROTATION_AND_TRACKING.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.ROTATION_AND_TRACKING.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstTR, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_TRACKING.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}

                        //if (!_lstSR.IsNullOrEmpty())
                        //{

                        #region Screening + Rotation

                        List<InvitationSharedInfoDetails> lstScreeningWithColorFlag1 = _lstSR
                                                                                      .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                                  || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
                                                                                      .ToList();

                        // Includes the Rotation + Screening (without Flag)
                        List<InvitationSharedInfoDetails> lstSRWithoutColorFlag = _lstSR
                                                                                  .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                             && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
                                                                                  .ToList();

                        //if (!lstScreeningWithColorFlag1.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails ScreeningRotationAttestationWithColorFlag = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
                                                                         && (con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus)) || con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                                                                           && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                           && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions ScreeningRotationAttestationWithColorFlagWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue()).FirstOrDefault();

                        if (!ScreeningRotationAttestationWithColorFlagWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningRotationAttestationWithColorFlagWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningRotationAttestationWithColorFlagWithPerm.DocumentPath;
                        }
                        else if (!ScreeningRotationAttestationWithColorFlag.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningRotationAttestationWithColorFlag.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningRotationAttestationWithColorFlag.DocumentPath;
                        }


                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, true, AttestationDocumentTypes.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag1, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag1, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {

                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, true, AttestationDocumentTypes.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag1, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}

                        //if (!lstSRWithoutColorFlag.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails ScreeningRotationAttestationWithoutColorFlag = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                        && (!con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus)) && !con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                                                                                              && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                                              && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions ScreeningRotationAttestationWithoutColorFlagWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue()).FirstOrDefault();

                        if (!ScreeningRotationAttestationWithoutColorFlagWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningRotationAttestationWithoutColorFlagWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningRotationAttestationWithoutColorFlagWithPerm.DocumentPath;
                        }
                        else if (!ScreeningRotationAttestationWithoutColorFlag.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningRotationAttestationWithoutColorFlag.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningRotationAttestationWithoutColorFlag.DocumentPath;
                        }


                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, false, AttestationDocumentTypes.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSRWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSRWithoutColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {

                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, true, false, AttestationDocumentTypes.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSRWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}

                        #endregion
                        //}

                        //if (!_lstT.IsNullOrEmpty())
                        //{

                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails TrackingAttestation = oldAttestation.Where(con => con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode == null || con.LstBkgSharedInfoTypeCode.Count == 0)
                                                 && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                 && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions TrackingAttestationWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.TRACKING_ONLY.GetStringValue()).FirstOrDefault();

                        if (!TrackingAttestationWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = TrackingAttestationWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = TrackingAttestationWithPerm.DocumentPath;
                        }
                        else if (!TrackingAttestation.IsNullOrEmpty())
                        {
                            invitationDocId = TrackingAttestation.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = TrackingAttestation.DocumentPath;
                        }

                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstT, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_ONLY.GetStringValue());
                        }


                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstT, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_ONLY.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {
                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);


                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstT, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_ONLY.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}

                        //if (!_lstR.IsNullOrEmpty())
                        //{                   
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails RotationAttestation = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode == null || con.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                && con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                                && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions RotationAttestationWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.ROTATION_ONLY.GetStringValue()).FirstOrDefault();

                        if (!RotationAttestationWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = RotationAttestationWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = RotationAttestationWithPerm.DocumentPath;
                        }
                        else if (!RotationAttestation.IsNullOrEmpty())
                        {
                            invitationDocId = RotationAttestation.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = RotationAttestation.DocumentPath;
                        }


                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, false, true, false, AttestationDocumentTypes.ROTATION_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstR, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_ONLY.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstR, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_ONLY.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {
                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, false, true, false, AttestationDocumentTypes.ROTATION_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.ROTATION_ONLY.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstR, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.ROTATION_ONLY.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}
                        //if (!_lstS.IsNullOrEmpty())
                        //{
                        #region Screening Only
                        List<InvitationSharedInfoDetails> lstScreeningWithColorFlag2 = _lstS
                                                                                      .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                          || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
                                                                                      .ToList();

                        // Includes the Tracking + Screening (without Flag)
                        List<InvitationSharedInfoDetails> lstSWithoutColorFlag = _lstS
                                                                                 .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                           && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only")
                                                                                     .ToList();

                        //if (!lstScreeningWithColorFlag2.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails ScreeningAttestationWithColorFlag = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
                                                && (con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus)) || con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                                                                     && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                     && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions ScreeningAttestationWithColorFlagWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.SCREENING_ONLY.GetStringValue()).FirstOrDefault();
                        if (!ScreeningAttestationWithColorFlagWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningAttestationWithColorFlagWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningAttestationWithColorFlagWithPerm.DocumentPath;
                        }
                        else if (!ScreeningAttestationWithColorFlag.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningAttestationWithColorFlag.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningAttestationWithColorFlag.DocumentPath;
                        }

                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag2, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_ONLY.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag2, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_ONLY.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {

                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningWithColorFlag2, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_ONLY.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }

                        //}
                        //if (!lstSWithoutColorFlag.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails ScreeningAttestationWithoutColorFlag = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode != null && con.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                    && (!con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                    && !con.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                                                                                    && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                                    && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions ScreeningAttestationWithoutColorFlagWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.SCREENING_WITHOUT_FLAG.GetStringValue()).FirstOrDefault();

                        if (!ScreeningAttestationWithoutColorFlagWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningAttestationWithoutColorFlagWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningAttestationWithoutColorFlagWithPerm.DocumentPath;
                        }
                        else if (!ScreeningAttestationWithoutColorFlag.IsNullOrEmpty())
                        {
                            invitationDocId = ScreeningAttestationWithoutColorFlag.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = ScreeningAttestationWithoutColorFlag.DocumentPath;
                        }

                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_WITHOUT_FLAG.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSWithoutColorFlag, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_WITHOUT_FLAG.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {

                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstSWithoutColorFlag, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_WITHOUT_FLAG.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}
                        #endregion
                        //}

                        //if (!_lstNone.IsNullOrEmpty())
                        //{
                        invitationDocId = AppConsts.NONE;
                        isDocMergingRequired = false;
                        previousAttestationDocPath = String.Empty;
                        singleAttestationDocPathToUpdate = String.Empty;

                        InvitationSharedInfoDetails NoneAttestation = oldAttestation.Where(con => !con.ComplianceSharedInfoTypeCode.IsNullOrEmpty() && (con.LstBkgSharedInfoTypeCode == null || con.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                   && !con.ReqRotSharedInfoTypeCode.IsNullOrEmpty()
                                                                                   && !con.IsForEveryOneAttestationForm).FirstOrDefault();

                        InvitationAttestationDocumentDataWithAgencyUserPermissions NoneAttestationWithPerm = oldAttestationsWithPermissionCode.Where(con => con.AgencyUserPermissionCode == lkpAgencyUserAttestationPermissionsEnum.NONE.GetStringValue()).FirstOrDefault();
                        if (!NoneAttestationWithPerm.IsNullOrEmpty())
                        {
                            invitationDocId = NoneAttestationWithPerm.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = NoneAttestationWithPerm.DocumentPath;

                        }
                        else if (!NoneAttestation.IsNullOrEmpty())
                        {
                            invitationDocId = NoneAttestation.InvitationDocumentID;
                            isDocMergingRequired = true;
                            previousAttestationDocPath = NoneAttestation.DocumentPath;
                        }

                        if (!overrideAttestationReportWithForm)
                        {
                            InvitationDocument _doc_WithSign = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstNone, lstInvitationDocumentMapping, _doc_WithSign, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.NONE.GetStringValue());
                        }

                        if (string.IsNullOrEmpty(previousAttestationDocPath) && overrideAttestationReportWithForm)
                        {

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstNone, lstInvitationDocumentMapping, _doc_AttestationForm, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.NONE.GetStringValue(),
                                                             prevAttestationFormDocumentID, isAttestationFormMerged);
                            isEveryOneDocSaved = true;

                        }
                        else
                        {
                            InvitationDocument _doc_C = null;

                            if (overrideAttestationReportWithForm)
                                _doc_C = GetInvitationDocumentObject(LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue(), selfUploadedDocpath, currentUserID);
                            else
                                _doc_C = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                            if (isDocMergingRequired)
                            {
                                singleAttestationDocPathToUpdate = DocumentManager.MergeAttestationDocuments(tenantID, _doc_C.IND_DocumentFilePath, previousAttestationDocPath, currentUserID
                                                                                                         , AttestationDocumentTypes.NONE.GetStringValue(), AttestationReportType.VERTICAL, !overrideAttestationReportWithForm);
                            }

                            AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, _lstNone, lstInvitationDocumentMapping, _doc_C, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.NONE.GetStringValue(),
                                                                 invitationDocId, isDocMergingRequired);
                            //UAT-2443:
                            if (isDocMergingRequired && !singleAttestationDocPathToUpdate.IsNullOrEmpty())
                            {
                                UpdateInvitationDocumentPath(invitationDocId, singleAttestationDocPathToUpdate, currentUserID, overrideAttestationReportWithForm);
                            }
                        }
                        //}

                        if (!isEveryOneDocSaved && overrideAttestationReportWithForm)
                        {
                            lstInvitationDocumentMapping.Add(new InvitationDocumentMapping()
                            {
                                InvitationDocument = _doc_AttestationForm,
                                IDM_IsDeleted = false,
                                IDM_CreatedByID = currentUserID,
                                IDM_CreatedOn = DateTime.Now,
                                IDM_ProfileSharingInvitationGroupID = generatedInvitationGroupId,
                            });
                        }

                        #endregion
                    }
                    else
                    {

                        #region   #region Generate the Lists for the types of Attestations, based on the permissions

                        List<InvitationSharedInfoDetails> lstTrackingOnlyInvitations = lstInvitationSharedInfoDetails
                                                                                                      .Where(cond => !cond.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
                                                                                                         && (cond.LstBkgSharedInfoTypeCode == null || cond.LstBkgSharedInfoTypeCode.Count == 0))
                                                                                                         .ToList();

                        List<InvitationSharedInfoDetails> lstScreeningOnlyInvitations = lstInvitationSharedInfoDetails
                                                                   .Where(cond => cond.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
                                                                   && (cond.LstBkgSharedInfoTypeCode != null && cond.LstBkgSharedInfoTypeCode.Count > 0)).ToList();

                        List<InvitationSharedInfoDetails> lstScreeningAndTrackingInvitations = lstInvitationSharedInfoDetails
                                                     .Where(cond => !cond.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
                                                     && (cond.LstBkgSharedInfoTypeCode != null && cond.LstBkgSharedInfoTypeCode.Count > 0)).ToList();

                        List<InvitationSharedInfoDetails> lstInvitationsWithoutTrackingAndScreening = lstInvitationSharedInfoDetails
                                                              .Where(cond => cond.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
                                                              && (cond.LstBkgSharedInfoTypeCode == null || cond.LstBkgSharedInfoTypeCode.Count == 0)).ToList();

                        //  lstTrackingOnlyInvitations = lstScreeningOnlyInvitations = lstScreeningAndTrackingInvitations = lstInvitationsWithoutTrackingAndScreening = lstInvitationSharedInfoDetails;

                        #endregion

                        #region Handle Normal Sharing scenarios

                        //if (!lstTrackingOnlyInvitations.IsNullOrEmpty())
                        //{
                        InvitationDocument _doc_WithSignTrackingConsolidated = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false,
                            AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);

                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstTrackingOnlyInvitations, lstInvitationDocumentMapping,
                            _doc_WithSignTrackingConsolidated, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_ONLY.GetStringValue());

                        InvitationDocument _doc_C = SaveAttestationDocument(generatedInvitationGroupId, true, false, false, false, AttestationDocumentTypes.TRACKING_ONLY.GetStringValue(),
                            tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);

                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstTrackingOnlyInvitations, lstInvitationDocumentMapping, _doc_C, currentUserID,
                            lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_ONLY.GetStringValue());

                        //}
                        //if (!lstScreeningOnlyInvitations.IsNullOrEmpty())
                        //{
                        List<InvitationSharedInfoDetails> lstScreeningOnlyInvitationsWithColorFlag = lstScreeningOnlyInvitations
                                                               .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                           || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
                                                                     )
                                                               .ToList();

                        List<InvitationSharedInfoDetails> lstScreeningOnlyInvitationsWithoutColorFlag = lstScreeningOnlyInvitations
                                    .Where(cond =>
                                            !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                         && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
                                    .ToList();

                        //UAT-3715:
                        // lstScreeningOnlyInvitationsWithColorFlag = lstScreeningOnlyInvitationsWithoutColorFlag = lstScreeningOnlyInvitations;

                        //if (!lstScreeningOnlyInvitationsWithColorFlag.IsNullOrEmpty())
                        //{

                        InvitationDocument _doc_WithSignScreeningOnlyInvitations = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_WithSignScreeningOnlyInvitations, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_ONLY.GetStringValue());
                        InvitationDocument _doc_C_ScreeningOnlyInvitations = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, true, AttestationDocumentTypes.SCREENING_ONLY.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_C_ScreeningOnlyInvitations, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_ONLY.GetStringValue());
                        //}
                        //if (!lstScreeningOnlyInvitationsWithoutColorFlag.IsNullOrEmpty())
                        //{

                        InvitationDocument _doc_WithSignScreeningOnlyInvitationsWithoutColor = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSignScreeningOnlyInvitationsWithoutColor, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_WITHOUT_FLAG.GetStringValue());
                        InvitationDocument _doc_C_ScreeningOnlyInvitationsWithoutColor = SaveAttestationDocument(generatedInvitationGroupId, false, true, false, false, AttestationDocumentTypes.SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningOnlyInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_C_ScreeningOnlyInvitationsWithoutColor, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.SCREENING_WITHOUT_FLAG.GetStringValue());
                        //}
                        //}
                        //if (!lstScreeningAndTrackingInvitations.IsNullOrEmpty())
                        //{
                        List<InvitationSharedInfoDetails> lstScreeningAndTrackingInvitationsWithColorFlag = lstScreeningAndTrackingInvitations
                                                                       .Where(cond => cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                 || cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)) // Consider Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
                                                                             )
                                                                       .ToList();

                        List<InvitationSharedInfoDetails> lstScreeningAndTrackingInvitationsWithoutColorFlag = lstScreeningAndTrackingInvitations
                                    .Where(cond => !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                && !cond.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly))) // Remove Attestation_Only : UAT 1464 - Updated permissions to manage agency users to handle "Attestation only"
                                    .ToList();

                        //UAT-3715
                        // lstScreeningAndTrackingInvitationsWithColorFlag = lstScreeningAndTrackingInvitationsWithoutColorFlag = lstScreeningAndTrackingInvitations;

                        //if (!lstScreeningAndTrackingInvitationsWithColorFlag.IsNullOrEmpty())
                        //{

                        InvitationDocument _doc_WithSignScreeningAndTrackingInvitationsWith = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_WithSignScreeningAndTrackingInvitationsWith, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue());
                        InvitationDocument _doc_C_ScreeningAndTrackingInvitationsWith = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, true, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithColorFlag, lstInvitationDocumentMapping, _doc_C_ScreeningAndTrackingInvitationsWith, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue());
                        //}
                        //if (!lstScreeningAndTrackingInvitationsWithoutColorFlag.IsNullOrEmpty())
                        //{

                        InvitationDocument _doc_WithSignScreeningAndTrackingInvitationsWithoutColor = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_WithSignScreeningAndTrackingInvitationsWithoutColor, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue());
                        InvitationDocument _doc_C_ScreeningAndTrackingInvitationsWithoutColor = SaveAttestationDocument(generatedInvitationGroupId, true, true, false, false, AttestationDocumentTypes.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstScreeningAndTrackingInvitationsWithoutColorFlag, lstInvitationDocumentMapping, _doc_C_ScreeningAndTrackingInvitationsWithoutColor, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue());
                        //}
                        //}
                        //if (!lstInvitationsWithoutTrackingAndScreening.IsNullOrEmpty())
                        //{

                        InvitationDocument _doc_WithSignInvitationsWithoutTrackingAndScreening = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.CONSOLIDATED_WITHOUT_SIGN, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstInvitationsWithoutTrackingAndScreening, lstInvitationDocumentMapping, _doc_WithSignInvitationsWithoutTrackingAndScreening, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.NONE.GetStringValue());
                        InvitationDocument _doc_C_InvitationsWithoutTrackingAndScreening = SaveAttestationDocument(generatedInvitationGroupId, false, false, false, false, AttestationDocumentTypes.NONE.GetStringValue(), tenantID, currentUserID, AttestationReportType.VERTICAL, agencyID);
                        AddToListOfInvitationDocumentMapping(generatedInvitationGroupId, lstInvitationsWithoutTrackingAndScreening, lstInvitationDocumentMapping, _doc_C_InvitationsWithoutTrackingAndScreening, currentUserID, lstInvAttestationDocWithPermissionType, lkpAgencyUserAttestationPermissionsEnum.NONE.GetStringValue());

                        //}

                        #endregion
                    }

                    if (!lstInvitationDocumentMapping.IsNullOrEmpty())
                    {
                        ProfileSharingManager.SaveInvitationDocumentMapping(lstInvitationDocumentMapping);
                    }
                    if (!lstInvAttestationDocWithPermissionType.IsNullOrEmpty())
                    {
                        ProfileSharingManager.SaveInvAttestationDocWithPermissionType(lstInvAttestationDocWithPermissionType);
                    }
                }

                #endregion
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace
                 , new Exception("An error occurred while generating attestation report " + "ProfileSharingInvitationGroupID: " + generatedInvitationGroupId, ex));
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace
                      , new Exception("An error occurred while generating attestation report " + "ProfileSharingInvitationGroupID: " + generatedInvitationGroupId, ex));
                throw (new SysXException(ex.Message, ex));
            }

        }


        public static void UpdateDocMappingForInvAttestation(Int32? agencyUserId, Int32? agencyUserTemplateId, Int32 CurrentLoggedInUserID)
        {
            try
            {
                String rotationSpecificPermissionTypeCode = String.Empty;
                String profileSpecificPermissionTypeCode = String.Empty;
                //Method To Get Shared User CurrentPermission
                InvitationSharedInfoDetails invitationSharedInfoDetails = BALUtils.GetProfileSharingRepoInstance().GetSharedUserCurrentPermission(agencyUserId, agencyUserTemplateId);

                String sharedInfoPermission_RotationNone = SharedInfoType.REQUIREMENT_ROTATION_NONE.GetStringValue();
                String sharedInfoPermission_ComplianceNone = SharedInfoType.COMPLIANCE_NONE.GetStringValue();
                String sharedInfoPermission_FlagStatus = SharedInfoType.FLAG_STATUS.GetStringValue();
                String bkgSharedInfoPermission_AttestationOnly = SharedInfoType.BACKGROUND_ATTESTATION_ONLY.GetStringValue();
                String complianceSharedInfoPermission_AttestationOnly = SharedInfoType.COMPLIANCE_ATTESTATION_ONLY.GetStringValue();
                String requirmentSharedInfoPermission_AttestationOnly = SharedInfoType.REQUIREMENT_ATTESTATION_ONLY.GetStringValue();

                if (!invitationSharedInfoDetails.IsNullOrEmpty())
                {
                    //Update Current Invitation Shared Info Type.
                    Boolean result = BALUtils.GetProfileSharingRepoInstance().UpdateSharedInfoTypeInComplAndReqSubs(agencyUserId, agencyUserTemplateId, CurrentLoggedInUserID);

                    #region Rotation Sharing Permission

                    // Screening + Tracking + Rotation
                    if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
                           && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode != null && invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count > 0)
                           && invitationSharedInfoDetails.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone)
                    {
                        //_lstAll
                        if (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                                    || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                        {
                            rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.Full.GetStringValue();
                        }
                        else if (!invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                                    && !invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                        {
                            rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.TRACKING_SCREENING_ROTATION_WITHOUT_FLAG.GetStringValue();
                        }
                    }
                    // Screening + Tracking
                    if (rotationSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
                               && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode != null && invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count > 0)
                               && invitationSharedInfoDetails.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone)
                        {
                            if (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                                                                       || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_TRACKING.GetStringValue();
                            }
                            // Includes the Tracking + Screening (without Flag)
                            else if (!invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                 && !invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue();
                            }
                        }
                    }
                    // Tracking + Rotation
                    if (rotationSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
                           && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode == null || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count == 0)
                           && invitationSharedInfoDetails.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone)
                        {
                            //_lstTR
                            rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_TRACKING.GetStringValue();
                        }
                    }
                    // Screening + Rotation
                    if (rotationSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
                                                                                   && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode != null && invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                   && invitationSharedInfoDetails.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone)
                        {
                            if (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                            || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.SCREENING_AND_ROTATION_WITH_FLAG.GetStringValue();
                            }
                            // Includes the Rotation + Screening (without Flag)
                            else if (!invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                      && !invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.ROTATION_AND_SCREENING_WITHOUT_FLAG.GetStringValue();
                            }
                        }
                    }
                    // Tracking Only
                    if (rotationSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode != sharedInfoPermission_ComplianceNone
                                                                                   && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode == null || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                   && invitationSharedInfoDetails.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone)
                        {
                            //_lstT
                            rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.TRACKING_ONLY.GetStringValue();
                        }
                    }
                    // Rotation Only
                    if (rotationSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
                                                                                   && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode == null || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count == 0)
                                                                                   && invitationSharedInfoDetails.ReqRotSharedInfoTypeCode != sharedInfoPermission_RotationNone)
                        {
                            //_lstR
                            rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.ROTATION_ONLY.GetStringValue();
                        }
                    }
                    // Screening Only
                    if (rotationSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
                                                                                   && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode != null && invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count > 0)
                                                                                   && invitationSharedInfoDetails.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone)
                        {
                            if (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                            || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.SCREENING_ONLY.GetStringValue();
                            }
                            // Includes the Tracking + Screening (without Flag)
                            else if (!invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                     && !invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.SCREENING_WITHOUT_FLAG.GetStringValue();
                            }
                        }
                    }
                    // No Permission
                    if (rotationSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode == sharedInfoPermission_ComplianceNone
                        && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode == null || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count == 0)
                        && invitationSharedInfoDetails.ReqRotSharedInfoTypeCode == sharedInfoPermission_RotationNone)
                        {
                            //NONE
                            rotationSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.NONE.GetStringValue();
                        }
                    }

                    #endregion

                    #region Profile Sharing Permission

                    if (!invitationSharedInfoDetails.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
                        && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode == null || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count == 0))
                    {
                        profileSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.TRACKING_ONLY.GetStringValue();

                    }

                    if (profileSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
                                 && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode != null && invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count > 0))
                        {
                            if (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                  || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                profileSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.SCREENING_ONLY.GetStringValue();
                            }
                            else if (!invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                 && !invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                profileSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.SCREENING_WITHOUT_FLAG.GetStringValue();
                            }
                        }
                    }
                    if (profileSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (!invitationSharedInfoDetails.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
                                                     && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode != null && invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count > 0))
                        {
                            if (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                               || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                profileSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITH_FLAG.GetStringValue();
                            }
                            else if (!invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(sharedInfoPermission_FlagStatus))
                                                    && !invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Any(val => val.Equals(bkgSharedInfoPermission_AttestationOnly)))
                            {
                                profileSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.TRACKING_AND_SCREENING_WITHOUT_FLAG.GetStringValue();
                            }
                        }
                    }
                    if (profileSpecificPermissionTypeCode.IsNullOrEmpty())
                    {
                        if (invitationSharedInfoDetails.ComplianceSharedInfoTypeCode.Equals(sharedInfoPermission_ComplianceNone)
                     && (invitationSharedInfoDetails.LstBkgSharedInfoTypeCode == null || invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Count == 0))
                        {
                            profileSpecificPermissionTypeCode = lkpAgencyUserAttestationPermissionsEnum.NONE.GetStringValue();
                        }
                    }
                    #endregion
                }
                if (!rotationSpecificPermissionTypeCode.IsNullOrEmpty() && !profileSpecificPermissionTypeCode.IsNullOrEmpty())
                    //Method To Update Inv Attestation Docs
                    BALUtils.GetProfileSharingRepoInstance().UpdateDocMappingForInvAttestation(agencyUserId, agencyUserTemplateId, rotationSpecificPermissionTypeCode, profileSpecificPermissionTypeCode, CurrentLoggedInUserID);

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


        #endregion

        #region UAT-3805
        /// <summary>
        /// Method to get item document notification data on category approval
        /// </summary>
        /// <param name="tenantID">TenantID</param>
        /// <param name="categoryDataIds">category data ids whose data need to get</param>
        /// <param name="applicantOrgUserID">Applicant organization User ID</param>
        /// <param name="approvedCategoryDataIds">Already approved Category Data IDs</param>
        /// <param name="requestTypeCode">Notification Data Request Type [AAAA= Rotation and AAAB = Compliance] * refer table lkpUseType</param>
        /// <param name="packageSubscriptionID">Compliance Package Subscription ID </param>
        /// <param name="RPS_ID">Requirement Package Subscription ID</param>
        /// <returns></returns>
        public static Boolean SendItemDocNotificationOnCategoryApproval(Int32 tenantID, String categoryIds
                                                                                                            , Int32 applicantOrgUserID, String approvedCategoryIds
                                                                                                            , String requestTypeCode, Int32? packageSubscriptionID
                                                                                                            , Int32? RPS_ID, Int32 currentLoggedInUserID)
        {
            try
            {
                List<ItemDocumentNotificationDataContract> lstItemDocNotificationData = AssignItemDocNotificationDataToModel(BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetItemDocNotificationDataOnCategoryApproval(
                                                                                                                   categoryIds, applicantOrgUserID
                                                                                                                  , approvedCategoryIds, requestTypeCode
                                                                                                                  , packageSubscriptionID, RPS_ID));
                return CommunicationManager.SendItemDocumentNotificationMailToAgencyUser(tenantID, lstItemDocNotificationData, currentLoggedInUserID);

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

        private static List<ItemDocumentNotificationDataContract> AssignItemDocNotificationDataToModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new ItemDocumentNotificationDataContract
                {
                    PackageName = x["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(x["PackageName"]),
                    RotationName = x["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(x["RotationName"]),
                    RotationStartDate = x["RotationStartDate"] == DBNull.Value ? (DateTime?)(null) : Convert.ToDateTime(x["RotationStartDate"]),
                    RotationEndDate = x["RotationEndDate"] == DBNull.Value ? (DateTime?)(null) : Convert.ToDateTime(x["RotationEndDate"]),
                    RotationID = x["RotationID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(x["RotationID"]),
                    PackageSubscriptionID = x["PackageSubscriptionID"] == DBNull.Value ? 0 : Convert.ToInt32(x["PackageSubscriptionID"]),
                    DocumentPath = Convert.ToString(x["DocumentPath"]),
                    DocumentName = Convert.ToString(x["DocumentName"]),
                    ApplicantDocumentID = x["ApplicantDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantDocumentID"]),
                    CategoryDataID = x["CategoryDataID"] == DBNull.Value ? 0 : Convert.ToInt32(x["CategoryDataID"]),
                    AgencyAdminName = x["AgencyAdminName"] == DBNull.Value ? String.Empty : Convert.ToString(x["AgencyAdminName"]),
                    AgencyAdminEmail = x["AgencyAdminEmail"] == DBNull.Value ? String.Empty : Convert.ToString(x["AgencyAdminEmail"]),
                    AgencyOrgUserID = x["AgencyOrgUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["AgencyOrgUserID"]),
                    AgencyUserID = x["AgencyUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["AgencyUserID"]),
                    ApplicantFirstName = x["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantFirstName"]),
                    ApplicantLastName = x["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantLastName"]),
                    ApplicantEmail = x["ApplicantEmail"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantEmail"]),
                    ApplicantOrgUserID = x["ApplicantOrgUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantOrgUserID"]),
                    HierarchyIDs = x["HierarchyIDs"] == DBNull.Value ? String.Empty : Convert.ToString(x["HierarchyIDs"]),
                    RequestTypeCode = x["RequestTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(x["RequestTypeCode"]),
                    CategoryName = x["CategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(x["CategoryName"]),
                    DocumentSize = x["DocumentSize"] == DBNull.Value ? 0 : Convert.ToInt32(x["DocumentSize"]),

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
        /// Method to return the approved category Data IDs, this method does not return exceptionally approved category.
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="subscriptionID">SubscriptionID</param>
        /// <param name="categoryIDs">CatgeoryIDs</param>
        /// <param name="requestType">Request Type [AAAA= Rotation and AAAB = Compliance] * refer table lkpUseType</param>
        /// <returns></returns>
        public static List<Int32> GetApprovedCategorIDs(Int32 tenantID, Int32 subscriptionID, List<Int32> categoryIDs, String requestType)
        {
            try
            {
                Int32 categoryStatusID_Approved = AppConsts.NONE;
                Int32 categoryStatusID_ExceptionallyApproved = AppConsts.NONE;
                if (String.Compare(requestType, lkpUseTypeEnum.COMPLIANCE.GetStringValue(), true) == AppConsts.NONE)
                {
                    categoryStatusID_Approved = LookupManager.GetLookUpData<lkpCategoryComplianceStatu>(tenantID).FirstOrDefault(cnd => cnd.Code == ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                                                                                          && !cnd.IsDeleted).CategoryComplianceStatusID;
                    categoryStatusID_ExceptionallyApproved = LookupManager.GetLookUpData<lkpCategoryComplianceStatu>(tenantID).FirstOrDefault(cnd =>
                                                                                                  cnd.Code == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()
                                                                                                             && !cnd.IsDeleted).CategoryComplianceStatusID;
                }
                else if (String.Compare(requestType, lkpUseTypeEnum.ROTATION.GetStringValue(), true) == AppConsts.NONE)
                {
                    categoryStatusID_Approved = LookupManager.GetLookUpData<lkpRequirementCategoryStatu>(tenantID).FirstOrDefault(cnd => cnd.RCS_Code == RequirementCategoryStatus.APPROVED.GetStringValue()
                                                                                                          && !cnd.RCS_IsDeleted).RCS_ID;
                }

                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetApprovedCategorIDs(subscriptionID, categoryIDs, requestType, categoryStatusID_Approved
                                                                                                    , categoryStatusID_ExceptionallyApproved);

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

        public static void RunParallelTaskItemDocNotificationOnCatApproval(Dictionary<String, Object> dicParam, ISysXLoggerService loggerService, ISysXExceptionService exceptionService)
        {
            try
            {
                INTSOF.ServiceUtil.ParallelTaskContext.PerformParallelTask(PerformParallelTaskForItemDocNotification, dicParam, loggerService, exceptionService);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void PerformParallelTaskForItemDocNotification(Dictionary<String, Object> dicParam)
        {
            try
            {
                List<ItemDocNotificationRequestDataContract> lstCategoryData = new List<ItemDocNotificationRequestDataContract>();
                if (dicParam.IsNotNull())
                {
                    dicParam.TryGetValue("CategoryData", out lstCategoryData);

                    lstCategoryData.ForEach(catData =>
                    {
                        SendItemDocNotificationOnCategoryApproval(catData.TenantID, catData.CategoryIds, catData.ApplicantOrgUserID, catData.ApprovedCategoryIds
                                                                 , catData.RequestTypeCode, catData.PackageSubscriptionID, catData.RPS_ID, catData.CurrentLoggedInUserID);
                    });

                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

        }

        /// <summary>
        /// Method to return the approved category Data IDs, this method does not return exceptionally approved category.
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="subscriptionID">SubscriptionID</param>
        /// <param name="categoryIDs">CatgeoryIDs</param>
        /// <param name="requestType">Request Type [AAAA= Rotation and AAAB = Compliance] * refer table lkpUseType</param>
        /// <returns></returns>
        public static List<PackageSubscription> GetCompliancePkgSubscriptionData(Int32 tenantID, List<CompliancePackageCategory> lstCompPackageCategories)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetCompliancePkgSubscriptionData(lstCompPackageCategories);
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
        /// Method to return the approved category Data IDs, this method does not return exceptionally approved category.
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="subscriptionID">SubscriptionID</param>
        /// <param name="categoryIDs">CatgeoryIDs</param>
        /// <param name="requestType">Request Type [AAAA= Rotation and AAAB = Compliance] * refer table lkpUseType</param>
        /// <returns></returns>
        public static List<RequirementPackageSubscription> GetRequirementPkgSubscriptionData(Int32 tenantID, List<Entity.ClientEntity.RequirementPackageCategory> lstReqPackageCategories)
        {
            try
            {
                return BALUtils.GetProfileSharingClientRepoInstance(tenantID).GetRequirementPkgSubscriptionData(lstReqPackageCategories);
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

        public static List<SyncRequirementPackageObject> GetReqPackageObjectList(String reqSyncObjectIDs)
        {
            try
            {
                var _lkpChangeType = LookupManager.GetSharedDBLookUpData<lkpChangeType>().FirstOrDefault(x => x.CT_Code == "AAAA");
                if (!_lkpChangeType.IsNullOrEmpty())
                {
                    Int32 chnageTypeID_ReqCategory = _lkpChangeType.CT_ID;
                    List<Int32> lstsyncObjectIDs = reqSyncObjectIDs.Split(',').Select(Int32.Parse).ToList();
                    return BALUtils.GetProfileSharingRepoInstance().GetReqPackageObjectList(lstsyncObjectIDs, chnageTypeID_ReqCategory);
                }
                return new List<SyncRequirementPackageObject>();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static List<RequirementPackageSubscription> GetReqSubscriptionByObjectIDs(Int32 tenantId, String reqPackageObjectIDs)
        {
            try
            {
                List<SyncRequirementPackageObject> lstReqPackageObjects = ProfileSharingManager.GetReqPackageObjectList(reqPackageObjectIDs);

                List<Guid> lstCategoryCodes = lstReqPackageObjects.Select(x => x.SRPO_NewObjectCode).ToList();


                return BALUtils.GetProfileSharingClientRepoInstance(tenantId).GetReqSubscriptionByObjectIDs(lstCategoryCodes);
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

        #region UAT-3977
        public static Dictionary<Int32, String> InstructorPreceptorRequiredPkgCompliantReqd(String agencyIDs)
        {
            try
            {
                String code = AgencyPermissionType.COMPLIANCE_REQD_INSTRUC_PRECEP_ROTATION_PKG_PRMSN.GetStringValue();
                Int32 agencyPrmsnTypeID = LookupManager.GetSharedDBLookUpData<lkpAgencyPermissionType>().Where(x => x.APT_Code == code && !x.APT_IsDeleted).FirstOrDefault().APT_ID;
                return BALUtils.GetProfileSharingRepoInstance().IsRequirementPkgCompliantReqd(agencyIDs, agencyPrmsnTypeID);
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


        public static List<RequirementSharesDataContract> GetRequirementNonComplaintSharesData(String userId, Int32 currentLoggedInUserId, String tenantIds, ClinicalRotationDetailContract clinicalRotationSearchContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetRequirementNonComplaintSharesData(userId, currentLoggedInUserId, tenantIds, clinicalRotationSearchContract, gridCustomPaging);
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

        #region UAT-3977
        private static void GetInstructorData(List<Entity.OrganizationUser> lstApplicantInfo, List<Int32> lstInstructorsIds)
        {
            foreach (Entity.OrganizationUser organizationUser in lstApplicantInfo)
            {
                if (lstInstructorsIds.Contains(organizationUser.OrganizationUserID) && !organizationUser.UserID.IsNullOrEmpty())
                {
                    ClientContact clientContact = BALUtils.GetProfileSharingRepoInstance().GetClientContact(organizationUser.UserID);
                    if (!clientContact.IsNullOrEmpty())
                    {
                        organizationUser.ExtraData = new Dictionary<string, string>();

                        organizationUser.ExtraData.Add("ClientContactPhone", Convert.ToString(clientContact.CC_Phone));
                        organizationUser.ExtraData.Add("IsClientContact", Convert.ToString(true));
                    }
                }
            }
        }
        #endregion

        #region UAT-3664


        public static List<lkpAgencyUserReport> GetAgencyUserReports()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAgencyUserReport>().Where(cond => !cond.AUR_IsDeleted).ToList();
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

        public static List<AgencyUserReportPermissionContract> GetAgencyUserReportPermissions(Int32 agencyUserId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserReportPermissions(agencyUserId);
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

        public static List<AgencyUserPermissionTemplateMapping> GetAgencyUserTemplateReportPermissions(Int32 templateId)
        {
            try
            {
                return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserTemplateReportPermissions(templateId);
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
        public static List<Entity.SharedDataEntity.AgencyUser> GetAgencyUserListByAgencIds(List<Int32> agencyIDs)
        {
            return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserListByAgencIds(agencyIDs);
        }

        public static Int32 GetAgencyUserOrganizationUserId(Guid? agencyUserID)
        {
            return BALUtils.GetProfileSharingRepoInstance().GetAgencyUserOrganizationUserId(agencyUserID);
        }
        #endregion

        #region UAT-4399
        public static byte[] GetRotationDetailSummaryReport(String InvitationId, String TenantId, String ReportName, String tempFilePath, String rotationID, String agencyID)
        {
            ParameterValue[] parameters = new ParameterValue[4];

            parameters[0] = new ParameterValue();
            parameters[0].Name = "TenantID";
            parameters[0].Value = TenantId;

            parameters[1] = new ParameterValue();
            parameters[1].Name = "InvitationGroupID";
            parameters[1].Value = InvitationId;

            parameters[2] = new ParameterValue();
            parameters[2].Name = "AgencyID";
            parameters[2].Value = agencyID;

            parameters[3] = new ParameterValue();
            parameters[3].Name = "ClinicalRotationID";
            parameters[3].Value = rotationID;

            String format = String.Empty;
            format = "pdf";

            byte[] reportContent = ReportManager.GetReportByteArrayFormat(ReportName, parameters, format);
            return reportContent;
        }

        #endregion

        //UAT-4658
        public static Boolean IsAgencyUserPresent(Int32 templateId)
        {
            return BALUtils.GetProfileSharingRepoInstance().IsAgencyUserPresent(templateId);
        }
        //End UAT-4658
    }
}
