using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using System.Xml.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Data.Entity.Core.Objects.DataClasses;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace DAL.Repository
{
    public class ProfileSharingClientRepository : ClientBaseRepository, IProfileSharingClientRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public ProfileSharingClientRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #region Applicant Invitations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invitationDetails"></param>
        void IProfileSharingClientRepository.SaveInvitationDetails(InvitationDetailsContract invitationDetails, List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoTypes)
        {

            foreach (var subscripton in invitationDetails.lstComplianceData)
            {
                if (subscripton.IsAnyCatSelected)
                {
                    subscripton.ComplianceSharedInfoTypeId = GetSharedInfoTypeId(lstSharedInfoTypes, subscripton.ComplianceSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_COMPLIANCE.GetStringValue());
                    var _sharedCompliance = GetSharedComplianceSubscriptionInstance(invitationDetails.CurrentUserId, invitationDetails.CurrentDateTime,
                                                                                    invitationDetails.PSIId, subscripton);
                    _dbContext.SharedComplianceSubscriptions.AddObject(_sharedCompliance);
                }
            }

            foreach (var bkgPkg in invitationDetails.lstBkgData)
            {
                if (bkgPkg.IsAnySvcGrpSelected)
                {
                    //UAT-1213
                    bkgPkg.LstBkgSharedInfoTypeId = GetBkgSharedInfoTypeIds(lstSharedInfoTypes, bkgPkg.LstBkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                    //bkgPkg.BkgSharedInfoTypeId = GetSharedInfoTypeId(lstSharedInfoTypes, bkgPkg.BkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                    var _sharedBkgPkg = GenerateSharedBkgPackageInstance(invitationDetails.CurrentUserId, invitationDetails.CurrentDateTime,
                                                                         invitationDetails.PSIId, bkgPkg);

                    //_sharedBkgPkg.InvitationSharedInfoMappings = new EntityCollection<InvitationSharedInfoMapping>();
                    //EntityCollection<InvitationSharedInfoMapping> colInvitationSharedInfoMappings = new EntityCollection<InvitationSharedInfoMapping>();
                    foreach (var sharedBkgInfoType in bkgPkg.LstBkgSharedInfoTypeId)
                    {
                        var _invitationSharedInfoMapping = GenerateInvitationSharedInfoMappingInstance(invitationDetails.CurrentUserId, invitationDetails.CurrentDateTime, sharedBkgInfoType, _sharedBkgPkg.SBP_ID);
                        _invitationSharedInfoMapping.SharedBkgPackage = _sharedBkgPkg;
                        //colInvitationSharedInfoMappings.Add(_invitationSharedInfoMapping);
                    }
                    //_sharedBkgPkg.InvitationSharedInfoMappings = colInvitationSharedInfoMappings;
                    _dbContext.SharedBkgPackages.AddObject(_sharedBkgPkg);
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Save the Bulk Invitation Details in Tenant, sent by admin/client admin
        /// </summary>
        /// <param name="lstInvitationDetails"></param>
        /// <param name="lstSharedInfoType"></param>
        Boolean IProfileSharingClientRepository.SaveAdminInvitationDetails(List<InvitationDetailsContract> lstInvitationDetails, List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoType,
                                                                           List<SharedUserSubscriptionSnapshotContract> lstSharedUserSnapshot, Int32 rotationId, Int32 reviewStatusId, Int32 agencyID)
        {
            int _currentUserId = 0;
            DateTime _currentDateTime = DateTime.Now;

            if (!lstInvitationDetails.IsNullOrEmpty())
            {
                _currentUserId = lstInvitationDetails.First().CurrentUserId;
                _currentDateTime = lstInvitationDetails.First().CurrentDateTime;
            }

            List<Int32> lstInviteeOrgUserIds = new List<Int32>();
            foreach (var invitation in lstInvitationDetails)
            {
                #region Add the Compliance package and its categories
                if (!invitation.lstComplianceData.IsNullOrEmpty())
                {
                    foreach (var subscripton in invitation.lstComplianceData)
                    {
                        if (subscripton.IsAnyCatSelected)
                        {
                            subscripton.ComplianceSharedInfoTypeId = GetSharedInfoTypeId(lstSharedInfoType, subscripton.ComplianceSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_COMPLIANCE.GetStringValue());
                            var _sharedCompliance = GetSharedComplianceSubscriptionInstance(invitation.CurrentUserId, invitation.CurrentDateTime,
                                                                                            invitation.PSIId, subscripton);
                            _dbContext.SharedComplianceSubscriptions.AddObject(_sharedCompliance);
                        }
                    }
                }
                #endregion

                #region Add the Requirement package and its Categories

                if (invitation.lstRequirementData != null)
                {
                    foreach (var requirementPkg in invitation.lstRequirementData)
                    {
                        requirementPkg.RequirementPkgSharedInfoTypeId = GetSharedInfoTypeId(lstSharedInfoType, requirementPkg.RequirementPkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_REQUIREMENT_ROTATION.GetStringValue());
                        var _sharedRequirement = GetSharedRequirementSubscriptionInstance(invitation.CurrentUserId, invitation.CurrentDateTime,
                                                                                        invitation.PSIId, requirementPkg);
                        _dbContext.SharedRequirementSubscriptions.AddObject(_sharedRequirement);
                    }
                }
                #endregion

                #region Add the background package and its service groups
                if (!invitation.lstBkgData.IsNullOrEmpty())
                {
                    foreach (var bkgPkg in invitation.lstBkgData)
                    {
                        if (bkgPkg.IsAnySvcGrpSelected)
                        {
                            bkgPkg.LstBkgSharedInfoTypeId = GetBkgSharedInfoTypeIds(lstSharedInfoType, bkgPkg.LstBkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                            //UAT-1213 -- bkgPkg.BkgSharedInfoTypeId = GetSharedInfoTypeId(lstSharedInfoType, bkgPkg.BkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                            var _sharedBkgPkg = GenerateSharedBkgPackageInstance(invitation.CurrentUserId, invitation.CurrentDateTime,
                                                                                 invitation.PSIId, bkgPkg);
                            foreach (var sharedBkgInfoType in bkgPkg.LstBkgSharedInfoTypeId)
                            {
                                var _invitationSharedInfoMapping = GenerateInvitationSharedInfoMappingInstance(lstInvitationDetails.First().CurrentUserId, lstInvitationDetails.First().CurrentDateTime, sharedBkgInfoType, _sharedBkgPkg.SBP_ID);
                                _invitationSharedInfoMapping.SharedBkgPackage = _sharedBkgPkg;
                                //colInvitationSharedInfoMappings.Add(_invitationSharedInfoMapping);
                            }
                            _dbContext.SharedBkgPackages.AddObject(_sharedBkgPkg);
                        }
                    }
                }
                #endregion

                var _agencyUserCode = OrganizationUserType.AgencyUser.GetStringValue();

                // Consider for status Update if:
                // It is rotation type, 
                // Invitee is registered user 
                // and Invitee is Not a Client Contact i.e. Only Agency User
                if (rotationId != AppConsts.NONE && invitation.InviteeOrgUserId.IsNotNull() && invitation.InviteeOrgUserId > AppConsts.NONE && invitation.InviteeUserTypeCode == OrganizationUserType.AgencyUser.GetStringValue())
                {
                    lstInviteeOrgUserIds.Add(Convert.ToInt32(invitation.InviteeOrgUserId));
                }
            }

            #region Update Rotation review status of Rotation to Pending Review

            if (rotationId != AppConsts.NONE && !lstInviteeOrgUserIds.IsNullOrEmpty())
            {
                foreach (var inviteeOrgUserId in lstInviteeOrgUserIds.Distinct())
                {

                    var _lstSavedReviews = _dbContext.SharedUserRotationReviews.Where(surr => surr.SURR_ClinicalRotaionID == rotationId
                                                                                                && surr.SURR_IsDeleted == false
                                                                                                && surr.SURR_AgencyID == agencyID
                                                                                      ).ToList();

                    var latestReviewID = _lstSavedReviews.Where(con => con.SURR_ReviewByID.HasValue).OrderByDescending(d => d.SURR_ModifiedOn).FirstOrDefault();

                    var _currentUserReview = _lstSavedReviews.Where(surr => surr.SURR_OrganizationUserID == inviteeOrgUserId).FirstOrDefault();



                    if (_currentUserReview.IsNotNull())
                    {

                        _currentUserReview.SURR_ModifiedByID = _currentUserId;
                        _currentUserReview.SURR_ModifiedOn = _currentDateTime;
                        _currentUserReview.SURR_RotationReviewStatusID = reviewStatusId;
                        _currentUserReview.SURR_ReviewByID = null;

                        //if (!latestReviewID.IsNullOrEmpty())
                        //{
                        //    _currentUserReview.SURR_ReviewByID = latestReviewID.SURR_ReviewByID;
                        //}
                    }
                    else
                    {
                        SharedUserRotationReview _sharedUserRotationReview = new SharedUserRotationReview();
                        _sharedUserRotationReview.SURR_CreatedByID = _currentUserId;
                        _sharedUserRotationReview.SURR_CreatedOn = _currentDateTime;
                        _sharedUserRotationReview.SURR_OrganizationUserID = inviteeOrgUserId;
                        _sharedUserRotationReview.SURR_AgencyID = agencyID;
                        _sharedUserRotationReview.SURR_IsDeleted = false;
                        _sharedUserRotationReview.SURR_ClinicalRotaionID = rotationId;
                        _sharedUserRotationReview.SURR_RotationReviewStatusID = reviewStatusId;
                        _dbContext.SharedUserRotationReviews.AddObject(_sharedUserRotationReview);
                    }
                }
            }
            #endregion

            if (!lstSharedUserSnapshot.IsNullOrEmpty() && !lstInvitationDetails.IsNullOrEmpty())
            {
                foreach (var sharedUser in lstSharedUserSnapshot)
                {
                    var _sussm = new SharedUserSubscriptionSnapshotMapping();
                    _sussm.SUSSM_SnapshotID = sharedUser.SnapshotId;
                    _sussm.SUSSM_ProfileSharingInvitationGroupID = lstInvitationDetails.First().PSIGroupId;
                    _sussm.SUSSM_RequirementSubscriptionID = sharedUser.RequirementSubscriptionId;
                    _sussm.SUSSM_SharedUserID = sharedUser.SharedUserId;
                    _sussm.SUSSM_SharedUserTypeID = sharedUser.SharedUserTypeId;
                    _sussm.SUSSM_CreatedByID = lstInvitationDetails.First().CurrentUserId;
                    _sussm.SUSSM_CreatedOn = lstInvitationDetails.First().CurrentDateTime;
                    _sussm.SUSSM_IsDeleted = false;

                    _dbContext.SharedUserSubscriptionSnapshotMappings.AddObject(_sussm);
                }
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save the details of the Packages & their categories/Service groups, which were either not included or partially included for sharing, during 'Submit Later' option
        /// </summary>
        /// <param name="lstSharedPkgData"></param>
        /// <param name="tenantId"></param>
        void IProfileSharingClientRepository.SaveScheduledExcludedPackageData(List<SharingPackageDataContract> lstSharedPkgData, Int32 currentUserId)
        {
            foreach (var excludedPackage in lstSharedPkgData)
            {
                if (excludedPackage.lstExcludedCategoryGrpIds.IsNotNull())
                {
                    foreach (var id in excludedPackage.lstExcludedCategoryGrpIds)
                    {
                        ScheduledInvitationExcludedPackageData _siepd = new ScheduledInvitationExcludedPackageData();
                        _siepd.SIEPD_ProfileSharingInvitationGroupID = excludedPackage.PSIGroupId;
                        _siepd.SIEPD_IsDeleted = false;
                        _siepd.SIEPD_CreatedOn = DateTime.Now;
                        _siepd.SIEPD_CreatedByID = currentUserId;

                        if (excludedPackage.PackageType == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                        {
                            _siepd.SIEPD_CompliancePackageID = excludedPackage.PackageId;
                            _siepd.SIEPD_ComplianceCategoryID = id;
                            _siepd.SIEPD_IsCompliancePackage = true;
                        }
                        else if (excludedPackage.PackageType == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                        {
                            _siepd.SIEPD_BackgroundPackageID = excludedPackage.PackageId;
                            _siepd.SIEPD_BkgServiceGroupID = id;
                            _siepd.SIEPD_IsCompliancePackage = false;
                        }
                        _dbContext.ScheduledInvitationExcludedPackageDatas.AddObject(_siepd);
                    }
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Gets the Invitation related data for the selected Invitation
        /// </summary>
        /// <param name="invitationId"></param>
        /// <returns></returns>
        Tuple<List<SharedComplianceSubscription>, List<SharedBkgPackage>> IProfileSharingClientRepository.GetInvitationData(Int32 invitationId)
        {
            var _lstComplianceSubs = _dbContext.SharedComplianceSubscriptions.Where(scs => scs.SCS_ProfileSharingInvitationID == invitationId && scs.SCS_IsDeleted == false).ToList();
            var _lstBkgSubs = _dbContext.SharedBkgPackages.Where(sbp => sbp.SBP_ProfileSharingInvitationID == invitationId && sbp.SBP_Isdeleted == false).ToList();

            return new Tuple<List<SharedComplianceSubscription>, List<SharedBkgPackage>>(_lstComplianceSubs, _lstBkgSubs);
        }

        /// <summary>
        /// Update/Save invitation details, depending on the details being updated by applicant
        /// </summary>
        /// <param name="invitationDetails"></param>
        void IProfileSharingClientRepository.UpdateInvitationDetails(InvitationDetailsContract invitationDetails, List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoType)
        {
            var _lstNewSvcGrps = new List<SharedBkgPackageSvcGroup>();
            var _lstSharedComplianceSub = _dbContext.SharedComplianceSubscriptions.Where(scs => scs.SCS_ProfileSharingInvitationID == invitationDetails.PSIId
                                                  && scs.SCS_IsDeleted == false).ToList();

            var _lstSharedBkgPkgs = _dbContext.SharedBkgPackages.Where(sbp => sbp.SBP_ProfileSharingInvitationID == invitationDetails.PSIId
                                                  && sbp.SBP_Isdeleted == false).ToList();

            #region Add/Update Compliance Related Data

            foreach (var cmpSub in invitationDetails.lstComplianceData)
            {
                if (cmpSub.IsAnyCatSelected)
                {
                    var _existingSub = _lstSharedComplianceSub.Where(scs => scs.SCS_PackageSubscriptionID == cmpSub.PkgSubId
                                                                        && scs.SCS_IsDeleted == false)
                                                              .FirstOrDefault();

                    if (_existingSub.IsNotNull())
                    {
                        _existingSub.SCS_SharedInfoTypeID = GetSharedInfoTypeId(lstSharedInfoType, cmpSub.ComplianceSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_COMPLIANCE.GetStringValue());
                        _existingSub.SCS_IsCompletePackageShared = cmpSub.IsCompletePkgSelected;
                        foreach (var catId in cmpSub.lstCategoryIds)
                        {
                            var _currentCat = _existingSub.SharedSubscriptionCategories.Where(ssc => ssc.SSC_ComplianceCategoryID == catId && ssc.SSC_IsDeleted == false).FirstOrDefault();

                            if (_currentCat.IsNullOrEmpty()) // Add any newly selected ComplianceCategory
                            {
                                GenerateSharedComplianceCategoriesInstance(invitationDetails.CurrentUserId, invitationDetails.CurrentDateTime, catId, _existingSub);
                            }
                        }

                        //Get the Records which are not in the List selected from the UI 
                        var _lstCatToDelete = _existingSub.SharedSubscriptionCategories
                                                             .Where(ssc => !cmpSub.lstCategoryIds.Contains(ssc.SSC_ComplianceCategoryID) && ssc.SSC_IsDeleted == false)
                                                             .Select(ssc => ssc)
                                                             .ToList();

                        foreach (var cat in _lstCatToDelete)
                        {
                            cat.SSC_IsDeleted = true;
                            cat.SSC_ModifiedByID = invitationDetails.CurrentUserId;
                            cat.SSC_ModifiedOn = invitationDetails.CurrentDateTime;
                        }
                    }
                    else
                    {
                        cmpSub.ComplianceSharedInfoTypeId = GetSharedInfoTypeId(lstSharedInfoType, cmpSub.ComplianceSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_COMPLIANCE.GetStringValue());
                        // Insert new subscription and its child, for which any category was selected
                        var _sharedCompliance = GetSharedComplianceSubscriptionInstance(invitationDetails.CurrentUserId,
                                                                                        invitationDetails.CurrentDateTime,
                                                                                        invitationDetails.PSIId, cmpSub);
                        _dbContext.SharedComplianceSubscriptions.AddObject(_sharedCompliance);
                    }
                }
                else
                {
                    var _existingSub = _lstSharedComplianceSub.Where(scs => scs.SCS_PackageSubscriptionID == cmpSub.PkgSubId
                                                                         && scs.SCS_IsDeleted == false)
                                                              .FirstOrDefault();
                    // Delete any existing subscription and its child, for which NO category was selected
                    if (_existingSub.IsNotNull())
                    {
                        _existingSub.SCS_IsDeleted = true;
                        _existingSub.SCS_ModifiedByID = invitationDetails.CurrentUserId;
                        _existingSub.SCS_ModifiedOn = invitationDetails.CurrentDateTime;

                        foreach (var _sharedCat in _existingSub.SharedSubscriptionCategories)
                        {
                            _sharedCat.SSC_IsDeleted = true;
                            _sharedCat.SSC_ModifiedByID = invitationDetails.CurrentUserId;
                            _sharedCat.SSC_ModifiedOn = invitationDetails.CurrentDateTime;
                        }
                    }
                }
            }

            #endregion

            #region Add/Update Background Package related Data

            foreach (var bkgPkg in invitationDetails.lstBkgData)
            {
                if (bkgPkg.IsAnySvcGrpSelected)
                {
                    var _existingPkg = _lstSharedBkgPkgs.Where(sbp => sbp.SBP_BkgOrderPackageID == bkgPkg.BOPId
                                                                        && sbp.SBP_Isdeleted == false)
                                                              .FirstOrDefault();

                    bkgPkg.LstBkgSharedInfoTypeId = GetBkgSharedInfoTypeIds(lstSharedInfoType, bkgPkg.LstBkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());

                    if (_existingPkg.IsNotNull())
                    {
                        //_existingPkg.SBP_SharedInfoTypeID = GetSharedInfoTypeId(lstSharedInfoType, bkgPkg.BkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                        var _lstExistingMappings = _existingPkg.InvitationSharedInfoMappings.Where(x => !x.ISIM_IsDeleted).ToList();
                        var _lstExistingMappingIds = _lstExistingMappings.Select(x => x.ISIM_InvitationSharedInfoTypeID);
                        var _toInsert = bkgPkg.LstBkgSharedInfoTypeId.Where(x => !_lstExistingMappingIds.Contains(x)).ToList();
                        var _toDelete = _lstExistingMappingIds.Where(x => !bkgPkg.LstBkgSharedInfoTypeId.Contains(x)).ToList();

                        foreach (var sharedInfoTypeID in _toDelete)
                        {
                            var _invitationSharedInfoMapping = _lstExistingMappings.Where(x => x.ISIM_InvitationSharedInfoTypeID == sharedInfoTypeID).First();
                            _invitationSharedInfoMapping.ISIM_IsDeleted = true;
                            _invitationSharedInfoMapping.ISIM_ModifiedByID = invitationDetails.CurrentUserId;
                            _invitationSharedInfoMapping.ISIM_ModifiedOn = invitationDetails.CurrentDateTime;
                        }
                        foreach (var sharedInfoTypeID in _toInsert)
                        {
                            var _invitationSharedInfoMapping = GenerateInvitationSharedInfoMappingInstance(invitationDetails.CurrentUserId, invitationDetails.CurrentDateTime, sharedInfoTypeID, _existingPkg.SBP_ID);
                            _dbContext.AddToInvitationSharedInfoMappings(_invitationSharedInfoMapping);
                        }

                        foreach (var svcGrpId in bkgPkg.lstSvcGrpIds)
                        {
                            var _currentSvcGrp = _existingPkg.SharedBkgPackageSvcGroups.Where(bsg => bsg.BPSG_BkgSvcGroupID == svcGrpId && bsg.BPSG_IsDeleted == false).FirstOrDefault();

                            if (_currentSvcGrp.IsNullOrEmpty()) // Add any newly selected BkgPackageSvcGroups
                            {
                                GenerateSharedBkgPkgSvcGroupInstance(invitationDetails.CurrentUserId, invitationDetails.CurrentDateTime, _existingPkg, svcGrpId);
                            }
                        }


                        //Get the Records which are not in the List selected from the UI 
                        var _lstSvcGrpToDelete = _existingPkg.SharedBkgPackageSvcGroups
                                                             .Where(bsg => !bkgPkg.lstSvcGrpIds.Contains(bsg.BPSG_BkgSvcGroupID) && bsg.BPSG_IsDeleted == false)
                                                             .Select(ssc => ssc)
                                                             .ToList();

                        foreach (var svcGrp in _lstSvcGrpToDelete)
                        {
                            svcGrp.BPSG_IsDeleted = true;
                            svcGrp.BPSG_ModifiedByID = invitationDetails.CurrentUserId;
                            svcGrp.BPSG_ModifiedOn = invitationDetails.CurrentDateTime;
                        }
                    }
                    else
                    {
                        // Insert new subscription and its child, for which any category was selected
                        //UAT-1213
                        //bkgPkg.LstBkgSharedInfoTypeId = GetBkgSharedInfoTypeIds(lstSharedInfoType, bkgPkg.LstBkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                        //bkgPkg.BkgSharedInfoTypeId = GetSharedInfoTypeId(lstSharedInfoType, bkgPkg.BkgSharedInfoTypeCode, SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                        var _sharedBkg = GenerateSharedBkgPackageInstance(invitationDetails.CurrentUserId, invitationDetails.CurrentDateTime, invitationDetails.PSIId, bkgPkg);

                        foreach (var bkgSharedInfoTypeId in bkgPkg.LstBkgSharedInfoTypeId)
                        {
                            var invitationSharedInfoMapping = GenerateInvitationSharedInfoMappingInstance(invitationDetails.CurrentUserId, invitationDetails.CurrentDateTime
                                                                                                            , bkgSharedInfoTypeId, _sharedBkg.SBP_ID);
                            invitationSharedInfoMapping.SharedBkgPackage = _sharedBkg;
                        }
                        _dbContext.SharedBkgPackages.AddObject(_sharedBkg);
                    }
                }
                else
                {
                    var _existingBkgPkg = _lstSharedBkgPkgs.Where(sbp => sbp.SBP_BkgOrderPackageID == bkgPkg.BOPId
                                                                         && sbp.SBP_Isdeleted == false)
                                                              .FirstOrDefault();
                    // Delete any existing child, for which NO Svc Group was selected
                    if (_existingBkgPkg.IsNotNull())
                    {
                        _existingBkgPkg.SBP_Isdeleted = true;
                        _existingBkgPkg.SBP_ModifiedByID = invitationDetails.CurrentUserId;
                        _existingBkgPkg.SBP_ModifiedOn = invitationDetails.CurrentDateTime;

                        foreach (var _sharedSvcGrp in _existingBkgPkg.SharedBkgPackageSvcGroups)
                        {
                            _sharedSvcGrp.BPSG_IsDeleted = true;
                            _sharedSvcGrp.BPSG_ModifiedByID = invitationDetails.CurrentUserId;
                            _sharedSvcGrp.BPSG_ModifiedOn = invitationDetails.CurrentDateTime;
                        }

                        foreach (var _sharedInfo in _existingBkgPkg.InvitationSharedInfoMappings)
                        {
                            _sharedInfo.ISIM_IsDeleted = true;
                            _sharedInfo.ISIM_ModifiedByID = invitationDetails.CurrentUserId;
                            _sharedInfo.ISIM_ModifiedOn = invitationDetails.CurrentDateTime;
                        }
                    }
                }
            }

            #endregion

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Gets the Email Subject and Content from AppDBConfiguration, based on the keys.
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <param name="contentCode"></param>
        /// <returns></returns>
        Dictionary<String, String> IProfileSharingClientRepository.GetInvitationEmailContent(String subjectCode, String contentCode)
        {
            var _lstAppConfig = _dbContext.AppConfigurations.Where(appConfig => appConfig.AC_Key == subjectCode || appConfig.AC_Key == contentCode).ToList();
            var _dic = new Dictionary<String, String>();
            foreach (var config in _lstAppConfig)
            {
                _dic.Add(config.AC_Key, config.AC_Value);
            }
            return _dic;
        }

        /// <summary>
        /// Get the list of packages associated for a particular Invitation, for Re-send invitation Email
        /// </summary>
        /// <param name="invitationId"></param>
        /// <returns></returns>
        Tuple<List<SharedComplianceSubscription>, List<SharedBkgPackage>> IProfileSharingClientRepository.GetSharedPackages(Int32 invitationId)
        {
            var _lstCompliance = _dbContext.SharedComplianceSubscriptions.Include("PackageSubscription.CompliancePackage").Include("SharedSubscriptionCategories")
                                                                .Where(scs => scs.SCS_ProfileSharingInvitationID == invitationId
                                                                && scs.SCS_IsDeleted == false).ToList();


            var _lstBkg = _dbContext.SharedBkgPackages.Include("BkgOrderPackage.BkgPackageHierarchyMapping.BackgroundPackage").Include("SharedBkgPackageSvcGroups")
                                                               .Where(sbp => sbp.SBP_ProfileSharingInvitationID == invitationId
                                                               && sbp.SBP_Isdeleted == false).ToList();

            return new Tuple<List<SharedComplianceSubscription>, List<SharedBkgPackage>>(_lstCompliance, _lstBkg);

        }


        #endregion


        #region Shared Invitation Detail for compliance and background packages
        /// <summary>
        /// Get the List of SharedComplianceSubscription
        /// </summary>  
        /// <returns></returns>
        List<SharedComplianceSubscription> IProfileSharingClientRepository.GetSharedComplianceSubscriptions(Int32 invitationId)
        {
            return _dbContext.SharedComplianceSubscriptions.Where(scs => scs.SCS_ProfileSharingInvitationID == invitationId && scs.SCS_IsDeleted == false && scs.PackageSubscription.IsDeleted == false).ToList();
        }

        /// <summary>
        /// Get the List of SharedBkgPackage
        /// </summary>  
        /// <returns></returns>
        List<SharedBkgPackage> IProfileSharingClientRepository.GetSharedBkgPackages(Int32 invitationId)
        {
            return _dbContext.SharedBkgPackages.Where(sbp => sbp.SBP_ProfileSharingInvitationID == invitationId && sbp.SBP_Isdeleted == false && sbp.InvitationSharedInfoMappings.Any(x => x.ISIM_IsDeleted == false)).ToList();
        }

        /// <summary>
        /// Get the List of Shared category list
        /// </summary>  
        /// <returns></returns>
        List<ApplicantComplianceCategoryData> IProfileSharingClientRepository.GetSharedCategoryList(Int32 packageSubscriptionId, List<Int32> sharedCategoryIds, Int32 snapshotId)
        {
            //if (snapshotId > AppConsts.NONE)
            //{
            //    return _dbContext.ApplicantComplianceCategoryDatas.Where(cond => cond.PackageSubscriptionID == packageSubscriptionId && sharedCategoryIds.Contains(cond.ComplianceCategoryID)).ToList();
            //}
            return _dbContext.ApplicantComplianceCategoryDatas.Where(cond => cond.PackageSubscriptionID == packageSubscriptionId && sharedCategoryIds.Contains(cond.ComplianceCategoryID) && cond.IsDeleted == false).ToList();
        }

        /// <summary>
        /// To get documents of invitations
        /// </summary>
        /// <param name="lstProfileSharingInvitationID"></param>
        /// <returns></returns>
        DataTable IProfileSharingClientRepository.GetSharedCategoryDocuments(Int32 packageSubscriptionId, String sharedcategoryids)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSharedCategoryDocuments", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@categoryIds", sharedcategoryids);
                command.Parameters.AddWithValue("@packageSubscriptionId", packageSubscriptionId);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }
        #endregion

        #region Manage Invitations

        /// <summary>
        /// To get invitations documents
        /// </summary>
        /// <param name="lstProfileSharingInvitationID"></param>
        /// <returns></returns>
        DataTable IProfileSharingClientRepository.GetApplicantInviteDocuments(List<InvitationIDsContract> lstProfileSharingInvitationID)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetInvitationDocuments", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@xmldata", CreateXml(lstProfileSharingInvitationID));
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }

        private String CreateXml(List<InvitationIDsContract> lstProfileSharingInvitationID)
        {
            XElement xmlElements = new XElement("ProfileSharingInvitationIDs", lstProfileSharingInvitationID
                                    .Select(i => new XElement("ProfileSharingInvitationID", i.ProfileSharingInvitationID)));
            return xmlElements.ToString();
        }

        /// <summary>
        /// To get invitations documents
        /// </summary>
        /// <param name="lstSnapshotID"></param>
        /// <returns></returns>
        DataTable IProfileSharingClientRepository.GetClientInviteDocuments(String clientInvitationIDs)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetImmunizationDocumentsFromSnapshot", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvitationIDs", clientInvitationIDs);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }

        /// <summary>
        /// To get applicant documents by applicant document IDs
        /// </summary>
        /// <param name="lstInvitationID"></param>
        /// <returns></returns>
        List<ApplicantDocument> IProfileSharingClientRepository.GetApplicantDocumentByIDs(List<Int32> lstApplicantDocumentID)
        {
            return _dbContext.ApplicantDocuments.Where(x => lstApplicantDocumentID.Contains(x.ApplicantDocumentID)
                        && x.IsDeleted == false).ToList();
        }

        /// <summary>
        /// Get the List of SharedComplianceSubscription by invitation IDs
        /// </summary>
        /// <param name="lstInvitationId"></param>
        /// <returns></returns>
        List<SharedComplianceSubscription> IProfileSharingClientRepository.GetSharedComplianceSubscriptionByInvitationIDs(List<Int32> lstInvitationID)
        {
            return _dbContext.SharedComplianceSubscriptions.Where(scs => lstInvitationID.Contains(scs.SCS_ProfileSharingInvitationID)
                        && scs.SCS_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Get passport report data
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        DataTable IProfileSharingClientRepository.GetPassportReportData(String xmlData)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetPassportReportData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@xmldata", xmlData);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }


        /// <summary>
        /// Get passport report data
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        List<InvitationDocumentContract> IProfileSharingClientRepository.GetPassportReportDataForRotation(List<Int32> invitationIDs)
        {
            List<InvitationDocumentContract> invitationdetails = new List<InvitationDocumentContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetPassportReportDataForRotation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvitationIds", String.Join(",", invitationIDs));
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        invitationdetails.Add(new InvitationDocumentContract
                        {
                            Name = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),
                            ProfileSharingInvitationID = dr["ProfileSharingInvitationID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProfileSharingInvitationID"]),
                        });
                    }
                }
                return invitationdetails;
            }
        }

        #endregion

        #region Private Methods

        #region  Applicant Invitations

        /// <summary>
        /// Generate the Compliance Package level Instance for the Compliance Packages to be linked with Invitation
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="psiId"></param>
        /// <param name="complianceData"></param>
        /// <returns></returns>
        private SharedComplianceSubscription GetSharedComplianceSubscriptionInstance(Int32 currentUserId, DateTime currentDateTime,
                                                                                     Int32 psiId, ComplianceInvitationData complianceData)
        {
            var _sharedCompliance = new SharedComplianceSubscription();
            _sharedCompliance.SCS_ProfileSharingInvitationID = psiId;
            _sharedCompliance.SCS_PackageSubscriptionID = complianceData.PkgSubId;
            _sharedCompliance.SCS_IsCompletePackageShared = complianceData.IsCompletePkgSelected;
            _sharedCompliance.SCS_CreatedBydID = currentUserId;
            _sharedCompliance.SCS_CreatedOn = currentDateTime;
            _sharedCompliance.SCS_IsDeleted = false;
            _sharedCompliance.SCS_SharedInfoTypeID = complianceData.ComplianceSharedInfoTypeId;
            _sharedCompliance.SCS_SnapshotID = complianceData.SnapShotId.IsNotNull() ? complianceData.SnapShotId : (Int32?)null;

            foreach (var catId in complianceData.lstCategoryIds)
            {
                GenerateSharedComplianceCategoriesInstance(currentUserId, currentDateTime, catId, _sharedCompliance);
            }
            return _sharedCompliance;
        }

        /// <summary>
        /// Generate the Compliance Package Category level Instance for the Compliance Packages to be linked with Invitation
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="categoryId"></param>
        /// <param name="_sharedCompliance"></param>
        private void GenerateSharedComplianceCategoriesInstance(Int32 currentUserId, DateTime currentDateTime,
                                                                       Int32 categoryId, SharedComplianceSubscription _sharedCompliance)
        {
            var _sharedCategory = new SharedSubscriptionCategory();
            _sharedCategory.SharedComplianceSubscription = _sharedCompliance;
            _sharedCategory.SSC_ComplianceCategoryID = categoryId;
            _sharedCategory.SSC_CreatedByID = currentUserId;
            _sharedCategory.SSC_CreatedOn = currentDateTime;
            _sharedCategory.SSC_IsDeleted = false;
        }

        /// <summary>
        /// Generate the Requirement Package level Instance for the Requirement Packages to be linked with Invitation
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="psiId"></param>
        /// <param name="requirementData"></param>
        /// <returns></returns>
        private SharedRequirementSubscription GetSharedRequirementSubscriptionInstance(Int32 currentUserId, DateTime currentDateTime,
                                                                                  Int32 psiId, RequirementInvitationData requirementData)
        {
            var _sharedRequirement = new SharedRequirementSubscription();
            _sharedRequirement.SRS_ProfileSharingInvitationID = psiId;
            _sharedRequirement.SRS_RequirementPackageSubscriptionID = requirementData.RequirementPkgSubId;
            _sharedRequirement.SRS_CreatedByID = currentUserId;
            _sharedRequirement.SRS_CreatedOn = currentDateTime;
            _sharedRequirement.SRS_IsDeleted = false;
            _sharedRequirement.SRS_SharedInfoTypeID = requirementData.RequirementPkgSharedInfoTypeId;
            _sharedRequirement.SRS_SnapshotID = requirementData.RequirementSnapShotId;

            foreach (var catId in requirementData.lstRequirementCategoryIds)
            {
                GenerateSharedRequirementCategoriesInstance(currentUserId, currentDateTime, catId, _sharedRequirement);
            }
            return _sharedRequirement;
        }

        /// <summary>
        /// Generate the Requirement Package Category level Instance for the Requirement Packages to be linked with Invitation
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="categoryId"></param>
        /// <param name="_sharedRequirement"></param>
        private void GenerateSharedRequirementCategoriesInstance(Int32 currentUserId, DateTime currentDateTime,
                                                                     Int32 categoryId, SharedRequirementSubscription _sharedRequirement)
        {
            var _sharedReqCategory = new SharedRequirementSubscriptionCategory();
            _sharedReqCategory.SharedRequirementSubscription = _sharedRequirement;
            _sharedReqCategory.SRSC_RequirementCategoryID = categoryId;
            _sharedReqCategory.SRSC_CreadtedByID = currentUserId;
            _sharedReqCategory.SRSC_CreatedOn = currentDateTime;
            _sharedReqCategory.SRSC_IsDeleted = false;
        }

        /// <summary>
        /// Generate the Background Package level Instance for the Background Packages to be linked with Invitation
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="psiId"></param>
        /// <param name="bkgPkg"></param>
        /// <returns></returns>
        private static SharedBkgPackage GenerateSharedBkgPackageInstance(Int32 currentUserId, DateTime currentDateTime,
                                                                                     Int32 psiId, BkgInvitationData bkgPkg)
        {
            var _sharedBkgPkg = new SharedBkgPackage();
            _sharedBkgPkg.SBP_ProfileSharingInvitationID = psiId;
            _sharedBkgPkg.SBP_BkgOrderPackageID = bkgPkg.BOPId;
            _sharedBkgPkg.SBP_CreatedBydID = currentUserId;
            _sharedBkgPkg.SBP_CreatedOn = currentDateTime;
            _sharedBkgPkg.SBP_Isdeleted = false;
            //_sharedBkgPkg.SBP_SharedInfoTypeID = bkgPkg.BkgSharedInfoTypeId; ----UAT-1213

            foreach (var svcGrpId in bkgPkg.lstSvcGrpIds)
            {
                GenerateSharedBkgPkgSvcGroupInstance(currentUserId, currentDateTime, _sharedBkgPkg, svcGrpId);
            }
            return _sharedBkgPkg;
        }

        //UAT-1213 - Method to generate InvitationSharedInfoMapping new instance
        private static Entity.ClientEntity.InvitationSharedInfoMapping GenerateInvitationSharedInfoMappingInstance(Int32 currentUserId, DateTime currentDateTime,
                                                                                      Int32 sharedInfoTypeID, Int32 sharedBkgPackageID)
        {
            var _invitationSharedInfoMapping = new Entity.ClientEntity.InvitationSharedInfoMapping();
            _invitationSharedInfoMapping.ISIM_InvitationSharedInfoTypeID = sharedInfoTypeID;
            _invitationSharedInfoMapping.ISIM_SharedBkgPackageID = sharedBkgPackageID;
            _invitationSharedInfoMapping.ISIM_CreatedByID = currentUserId;
            _invitationSharedInfoMapping.ISIM_CreatedOn = currentDateTime;
            _invitationSharedInfoMapping.ISIM_IsDeleted = false;

            return _invitationSharedInfoMapping;
        }

        private static void GenerateSharedBkgPkgSvcGroupInstance(Int32 currentUserId, DateTime currentDateTime, SharedBkgPackage _sharedBkgPkg, int svcGrpId)
        {
            var _sharedCategory = new SharedBkgPackageSvcGroup();
            _sharedCategory.SharedBkgPackage = _sharedBkgPkg;
            _sharedCategory.BPSG_BkgSvcGroupID = svcGrpId;
            _sharedCategory.BPSG_CreatedByID = currentUserId;
            _sharedCategory.BPSG_CreatedOn = currentDateTime;
            _sharedCategory.BPSG_IsDeleted = false;
        }

        private static Int32 GetSharedInfoTypeId(List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoTypes, String selectedCode, String masterTypeCode)
        {
            return lstSharedInfoTypes.Where(sit => sit.Code == selectedCode &&
                   sit.IsDeleted == false && sit.MasterInfoTypeCode == masterTypeCode).First().SharedInfoTypeID;
        }

        //UAT-1213 - Method to get BkgSharedInfoTypeIDs by BkgSharedInfoTypeCodes
        private static List<Int32> GetBkgSharedInfoTypeIds(List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoTypes, List<String> lstSelectedCode, String masterTypeCode)
        {
            return lstSharedInfoTypes.Where(cond => lstSelectedCode.Contains(cond.Code)
                && cond.IsDeleted == false && cond.MasterInfoTypeCode == masterTypeCode).Select(x => x.SharedInfoTypeID).ToList();
        }

        #endregion

        #endregion

        #region AGENCY SHARING
        DataTable IProfileSharingClientRepository.GetDataForAgencySharing(SearchItemDataContract searchDataContract, CustomPagingArgsContract customPagingArgsContract)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetDataForAgencySharing", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@dataXML", searchDataContract.CreateXml());
                //command.Parameters.AddWithValue("@CustomAtrributesData", searchDataContract.CustomFields);
                command.Parameters.AddWithValue("@customFilteringXml", customPagingArgsContract.CreateXml());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //customPagingArgsContract.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        customPagingArgsContract.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["TotalCount"]));
                    }
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        Int32 IProfileSharingClientRepository.SaveImmunizationSnapshot(Int32 currentUserID, Int32 packageSubscrptionID)
        {
            return _dbContext.SaveImmunizationSnapshot(packageSubscrptionID, currentUserID).FirstOrDefault() ?? AppConsts.NONE;
        }

        /// <summary>
        /// Generate the Snapshot of the Requirement Package
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="packageSubscrptionID"></param>
        /// <returns></returns>
        Int32 IProfileSharingClientRepository.SaveRequirementSnapshot(Int32 currentUserId, Int32 packageSubscrptionId)
        {
            return _dbContext.SaveRequirementSnapshot(packageSubscrptionId, currentUserId).FirstOrDefault() ?? AppConsts.NONE;
        }

        #endregion

        #region Immunization Data For Snapshot
        /// <summary>
        /// Get Immuniztion Data From snapshot.
        /// </summary>  
        /// <returns></returns>
        DataSet IProfileSharingClientRepository.GetImmunizationDataFromSnapshot(Int32 snapshotId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetImmunizationDataFromSnapshot", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SnapshotID", snapshotId);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                return new DataSet();
            }
        }

        /// <summary>
        /// To get Applicant documents From snapshot
        /// </summary>
        /// <param name="sharedcategoryids"></param>
        /// <param name="snapshotId"></param>
        /// <returns></returns>
        DataTable IProfileSharingClientRepository.GetApplicantDocumentsFromSnapshot(String sharedcategoryids, Int32 snapshotId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetApplicantDocumentsFromSnapshot", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@categoryIds", sharedcategoryids);
                command.Parameters.AddWithValue("@snapshotId", snapshotId);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }
        #endregion

        /// <summary>
        /// Gets the OrganizationUser data, based on the OrgganizationUserId
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        OrganizationUser IProfileSharingClientRepository.GetOrganizationUser(Int32 organizationUserId)
        {
            var organizationUser = _dbContext.OrganizationUsers.Where(orgUser => orgUser.OrganizationUserID == organizationUserId && orgUser.IsDeleted == false).FirstOrDefault();
            return !organizationUser.IsNull() ? organizationUser : null;
        }

        #region UAT-1213: Updates to Agency User background check permissions.
        List<vwBkgOrderFlagged> IProfileSharingClientRepository.GetBkgOrderFlagged(List<Int32> bkgOrderIds)
        {
            return _dbContext.vwBkgOrderFlaggeds.Where(x => bkgOrderIds.Contains(x.BOR_ID)).ToList();
        }
        #endregion

        #region UAT-1210: As a client admin, I should be able to see when and what was shared through profile sharing

        List<GetProfileSharingData_Result> IProfileSharingClientRepository.GetProfileSharingData(Int32 invitationGroupID)
        {
            return _dbContext.GetProfileSharingData(invitationGroupID).ToList();
        }

        #endregion

        #region UAT-1237
        List<Int32> IProfileSharingClientRepository.GetSharedCategoryList(Int32 invitationID, Int32 packageSubscriptionID)
        {
            List<Int32> lstSharedCategoryID = _dbContext.SharedComplianceSubscriptions
                 .Where(cond => cond.SCS_ProfileSharingInvitationID == invitationID && cond.SCS_PackageSubscriptionID == packageSubscriptionID && !cond.SCS_IsDeleted)
                 .Select(col => col.SharedSubscriptionCategories).FirstOrDefault()
                 .Where(cond => !cond.SSC_IsDeleted)
                 .Select(col => col.SSC_ComplianceCategoryID)
                 .ToList();

            if (!lstSharedCategoryID.IsNullOrEmpty())
                return lstSharedCategoryID;
            return new List<Int32>();
        }
        #endregion


        #region UAT 1318

        /// <summary>
        /// Gets the list of Applicants added to a Rotation, for sending the ProfileSharingInvitation
        /// </summary>
        /// <param name="RotationId"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        public DataTable GetRotationMembers(Int32 rotationId, Int32 agencyId, CustomPagingArgsContract customPagingArgsContract, String rotationMemberIds, String instructorPreceptorOrgUserIds)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetRotationMembers", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RotationId", rotationId);
                command.Parameters.AddWithValue("@CustomFilteringXml", customPagingArgsContract.CreateXml());
                command.Parameters.AddWithValue("@AgencyID", agencyId);
                command.Parameters.AddWithValue("@RotationMemberIds", rotationMemberIds);
                command.Parameters.AddWithValue("@InstructorPreceptorOrgUserIds", instructorPreceptorOrgUserIds);//UAT-3977

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        customPagingArgsContract.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["TotalCount"]));
                    }
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        #endregion

        #region Shared Invitation Detail for Requirement Packages
        /// <summary>
        /// Get the List of SharedComplianceSubscription
        /// </summary>  
        /// <returns></returns>
        SharedRequirementSubscription IProfileSharingClientRepository.GetSharedRequirementSubscriptions(Int32 invitationId)
        {
            return _dbContext.SharedRequirementSubscriptions.Where(srs => srs.SRS_ProfileSharingInvitationID == invitationId && srs.SRS_IsDeleted == false)
                                                                                                .FirstOrDefault();
        }

        /// <summary>
        /// Get Requirement Data From snapshot.
        /// </summary>  
        /// <returns></returns>
        DataSet IProfileSharingClientRepository.GetRequirementDataFromSnapshot(Int32 snapshotId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetRequirementDataFromSnapshot", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SnapshotID", snapshotId);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                return new DataSet();
            }
        }

        /// <summary>
        /// To get Applicant documents From snapshot
        /// </summary>
        /// <param name="sharedcategoryids"></param>
        /// <param name="snapshotId"></param>
        /// <returns></returns>
        DataTable IProfileSharingClientRepository.GetApplicantRequirementDocumentsFromSnapshot(String sharedcategoryids, Int32 snapshotId, Int32 orgUserID, String rotationID, Boolean IsApplicantDropped)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetApplicantRequirementDocumentsFromSnapshot", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@categoryIds", sharedcategoryids);
                command.Parameters.AddWithValue("@snapshotId", snapshotId);
                command.Parameters.AddWithValue("@orgUserID", orgUserID);
                command.Parameters.AddWithValue("@rotationID", rotationID); //UAT 3125
                command.Parameters.AddWithValue("@IsApplicantDropped", IsApplicantDropped); //UAT 3125


                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }


        /// <summary>
        /// UAT-3338
        /// </summary>
        /// <param name="sharedcategoryids"></param>
        /// <param name="loggedInUserID"></param>
        /// <param name="rotationID"></param>
        /// <param name="InstructorOrgId"></param>
        /// <returns></returns>
        DataTable IProfileSharingClientRepository.GetInstructorRequirementDocuments(String sharedcategoryids, Int32 loggedInUserID, String rotationID, Int32 InstructorOrgId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetInstructorRequirementDocuments", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@categoryIds", sharedcategoryids);
                command.Parameters.AddWithValue("@loggedInUserID", loggedInUserID);
                command.Parameters.AddWithValue("@rotationID", rotationID);
                command.Parameters.AddWithValue("@InstructorOrgId", InstructorOrgId);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }


        /// <summary>
        /// Get the List of Shared category list
        /// </summary>  
        /// <returns></returns>
        List<ApplicantRequirementCategoryData> IProfileSharingClientRepository.GetSharedRequirementCategoryList(Int32 packageSubscriptionId, List<Int32> sharedCategoryIds, Int32 snapshotId, Boolean IsInstructorPreceptorData)
        {
            if (IsInstructorPreceptorData)
            {
                return _dbContext.ApplicantRequirementCategoryDatas.Where(cond => cond.ARCD_RequirementPackageSubscriptionID == packageSubscriptionId && cond.ARCD_IsDeleted == false).ToList();
            }
            else
            {
                return _dbContext.ApplicantRequirementCategoryDatas.Where(cond => cond.ARCD_RequirementPackageSubscriptionID == packageSubscriptionId
                                                                                             && sharedCategoryIds.Contains(cond.ARCD_RequirementCategoryID)
                                                                                             && cond.ARCD_IsDeleted == false).ToList();
            }
        }
        #endregion

        bool IProfileSharingClientRepository.UpdateSharedCategoryData(int ProfileSharingInvitationID, List<SharedInvitationSubscriptionContract> lstSharedInvitationSubscriptionContract)
        {

            List<Int32> lstProfSharingPkgSubID = lstSharedInvitationSubscriptionContract.Where(cond => cond.IsComplianceSubscription)
                                                                                        .Select(slct => slct.PkgSubscriptionID).ToList();
            List<Int32> lstRotationPkgSubID = lstSharedInvitationSubscriptionContract.Where(cond => !cond.IsComplianceSubscription)
                                                                                        .Select(slct => slct.PkgSubscriptionID).ToList();
            List<SharedComplianceSubscription> lstSharedComplianceSub = _dbContext.SharedComplianceSubscriptions.Where(cnd => cnd.SCS_ProfileSharingInvitationID ==
                                                                                   ProfileSharingInvitationID && cnd.SCS_IsDeleted == false
                                                                                   && lstProfSharingPkgSubID.Contains(cnd.SCS_PackageSubscriptionID)
                                                                                  ).ToList();

            List<SharedRequirementSubscription> lstSharedRequirementSub = _dbContext.SharedRequirementSubscriptions.Where(cnd => cnd.SRS_ProfileSharingInvitationID ==
                                                                                     ProfileSharingInvitationID && cnd.SRS_IsDeleted == false
                                                                                     && lstRotationPkgSubID.Contains(cnd.SRS_RequirementPackageSubscriptionID)
                                                                                     ).ToList();

            lstSharedInvitationSubscriptionContract.Where(x => x.IsComplianceSubscription).ForEach(cond =>
            {
                foreach (var sharedSubCatCollection in lstSharedComplianceSub.Where(x => x.SCS_PackageSubscriptionID == cond.PkgSubscriptionID)
                                                                           .Select(slct => slct.SharedSubscriptionCategories))
                {
                    if (sharedSubCatCollection.IsNotNull())
                    {
                        var sharedSubCat = sharedSubCatCollection.FirstOrDefault(x => !x.SSC_IsDeleted && x.SSC_ComplianceCategoryID == cond.CategoryID);
                        if (!sharedSubCat.IsNullOrEmpty())
                        {
                            sharedSubCat.SSC_IsViewed = cond.IsCategoryViewed;
                            break;
                        }
                    }
                }

            });

            lstSharedInvitationSubscriptionContract.Where(x => !x.IsComplianceSubscription).ForEach(cond =>
            {
                foreach (var sharedReqSubCatCollection in lstSharedRequirementSub.Where(x => x.SRS_RequirementPackageSubscriptionID == cond.PkgSubscriptionID)
                                                                           .Select(slct => slct.SharedRequirementSubscriptionCategories))
                {
                    if (!sharedReqSubCatCollection.IsNullOrEmpty())
                    {
                        var sharedReqSubCat = sharedReqSubCatCollection.FirstOrDefault(x => !x.SRSC_IsDeleted && x.SRSC_RequirementCategoryID == cond.CategoryID);
                        if (!sharedReqSubCat.IsNullOrEmpty())
                        {
                            sharedReqSubCat.SRSC_IsViewed = cond.IsCategoryViewed;
                            break;
                        }
                    }
                }

            });
            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        DataSet IProfileSharingClientRepository.GetScheduledInvitationData(String applicantOrgUserIdCSV, Int32? rotationID)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetScheduledInvitationData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicantOrgUserIds", applicantOrgUserIdCSV);
                command.Parameters.AddWithValue("@RotationId", rotationID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
            }
            return new DataSet();
        }

        public List<ScheduledInvitationExcludedPackageData> GetScheduledInvitationExcludedPackageDataByProfileSharingInvitationGroupID(int profileSharingInvitationGroupID)
        {
            return _dbContext.ScheduledInvitationExcludedPackageDatas.Where(cond => cond.SIEPD_IsDeleted != true && cond.SIEPD_ProfileSharingInvitationGroupID == profileSharingInvitationGroupID).ToList();
        }

        /// <summary>
        /// Get the Shared user subscriptions in order to generate the Snapshot, when Submit Later is used.
        /// </summary>
        /// <param name="rotationId"></param>
        /// <returns></returns>
        List<ClientContactProfileSharingData> IProfileSharingClientRepository.GetSharedUserSubscriptions(Int32 rotationId)
        {
            var lstSharedUserSubscriptions = new List<ClientContactProfileSharingData>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@RotationId", rotationId)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSharedUserSubscriptions", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstSharedUserSubscriptions.Add(new ClientContactProfileSharingData
                            {
                                ClientContactTypeCode = Convert.ToString(dr["ClientContactTypeCode"]),
                                ClientContactID = Convert.ToInt32(dr["ClientContactId"]),
                                ReqSubId = Convert.ToInt32(dr["RequirementSubscriptionId"])
                            });
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstSharedUserSubscriptions;
        }

        public List<ShareHistoryDataContract> GetShareHistoryData(ShareHistorySearchContract shareHistorySearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<ShareHistoryDataContract> lstShareHistoryDataContract = new List<ShareHistoryDataContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetProfileShareHistoryData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@xmldata", shareHistorySearchContract.CreateXml());
                command.Parameters.AddWithValue("@filteringSortingData", customPagingArgsContract.CreateXml());
                command.Parameters.AddWithValue("@DroppedStatus", AppConsts.APPLICANT_DROPPED_STATUS);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstShareHistoryDataContract.Add(new ShareHistoryDataContract
                        {
                            InvitationGroupId = dr["InvitationGroupId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InvitationGroupId"]),
                            InvitationId = dr["InvitationId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InvitationId"]),
                            AgencyName = dr["AgencyName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AgencyName"]),
                            InviteeName = dr["InviteeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteeName"]),
                            SchoolRepresentative = dr["SchoolRepresentative"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SchoolRepresentative"]),
                            ViewedStatus = dr["ViewedStatus"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ViewedStatus"]),
                            InvitationDate = dr["InvitationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["InvitationDate"]),
                            ExpirationDateOrNumberOfViews = dr["ExpirationDateOrNumberOfViews"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ExpirationDateOrNumberOfViews"]),
                            RotationID_Name = dr["RotationID_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationID_Name"]),
                            StartDate = dr["StartDate"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["StartDate"]),
                            EndDate = dr["EndDate"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["EndDate"]),
                            StartTime = dr["StartTime"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["StartTime"]),
                            EndTime = dr["EndTime"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["EndTime"]),
                            RotationDays = dr["RotationDays"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationDays"]),
                            FirstName = dr["FirstName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["FirstName"]),
                            LastName = dr["LastName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["LastName"]),
                            AgencyReviewStatus = dr["AgencyReviewStatus"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AgencyReviewStatus"]),
                            DetailShared = dr["DetailShared"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["DetailShared"]),
                            //UAT-1895 Added Audit Request Date 
                            AuditRequestedDate = dr["AuditRequestedDate"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AuditRequestedDate"]),

                            TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]),
                            AgencyId = dr["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyID"]),//UAT-2784
                            AgencyExpirationSetting = dr["AgencyExpirationSetting"].GetType().Name == "DBNull" ? true : Convert.ToBoolean(Convert.ToInt32(dr["AgencyExpirationSetting"]))//UAT-2784
                        });
                    }

                    //customPagingArgsContract.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["TotalCount"]));
                }
                return lstShareHistoryDataContract;
            }
        }

        public List<usp_GetProfileSharingDataByInvitationId_Result> GetProfileSharingDataByInvitationId(Int32 invitationID)
        {
            return _dbContext.usp_GetProfileSharingDataByInvitationId(invitationID).ToList();
        }

        List<AgencyHierarchyMapping> IProfileSharingClientRepository.GetInstituteHierarchyForSelectedAgency(Int32 agencyID)
        {
            return _dbContext.AgencyHierarchyMappings.Where(cond => cond.AGHM_AgencyID == agencyID && !cond.AGHM_IsDeleted).ToList();
        }

        #region UAT-2529
        Tuple<Boolean, Boolean> IProfileSharingClientRepository.GetAgencyInstitutionPermissionForSelectedAgency(Int32 agencyInstitutionID)
        {
            var result = _dbContext.AgencyInstitutionPermissions.Where(cond => cond.AIP_AgencyInstitutionID == agencyInstitutionID && !cond.AIP_IsDeleted).ToList();

            if (!result.IsNullOrEmpty())
            {
                Boolean IsStudent = true;
                Boolean IsAdmin = true;
                foreach (var item in result)
                {
                    if (item.AIP_AgencyInstitutionPermissionTypeID == AppConsts.ONE)
                        IsStudent = item.AIP_AgencyInstitutionPermissionAccessTypeID == AppConsts.ONE ? true : false;
                    if (item.AIP_AgencyInstitutionPermissionTypeID == AppConsts.TWO)
                        IsAdmin = item.AIP_AgencyInstitutionPermissionAccessTypeID == AppConsts.ONE ? true : false;
                }
                return new Tuple<Boolean, Boolean>(IsStudent, IsAdmin);
            }
            else
                return new Tuple<Boolean, Boolean>(true, true);
        }

        Boolean IProfileSharingClientRepository.GetAgencyProfileSharingPermission(Int32 agencyInstitutionID)
        {
            Boolean IsAdminProfileSharingPermission = true;
            var result = _dbContext.AgencyInstitutionPermissions.Where(cond => cond.AIP_AgencyInstitutionID == agencyInstitutionID && !cond.AIP_IsDeleted && cond.AIP_AgencyInstitutionPermissionTypeID == AppConsts.TWO).FirstOrDefault();

            if (!result.IsNullOrEmpty())
            {

                IsAdminProfileSharingPermission = result.AIP_AgencyInstitutionPermissionAccessTypeID == AppConsts.ONE ? true : false;
                return IsAdminProfileSharingPermission;
            }
            else
                return IsAdminProfileSharingPermission;
        }

        Tuple<Boolean, String> IProfileSharingClientRepository.GetApplicantIndividualProfileSharingPermission(Dictionary<Int32, String> agencyInstitution)
        {
            Boolean IsAdminProfileSharingPermission = true;
            List<Int32> agencyInstitutionIds = new List<Int32>();
            agencyInstitution.ForEach(d => agencyInstitutionIds.Add(d.Key));
            var result = _dbContext.AgencyInstitutionPermissions.Where(cond => agencyInstitutionIds.Contains(cond.AIP_AgencyInstitutionID) && !cond.AIP_IsDeleted && cond.AIP_AgencyInstitutionPermissionTypeID == AppConsts.ONE).ToList();
            if (result.Count > AppConsts.NONE)
            {
                if (result.Count == agencyInstitution.Count) //Old agencies permission not existed in AgencyInstitutionPermissions (tenant database) table
                {
                    if (!result.Where(cond => cond.AIP_AgencyInstitutionPermissionAccessTypeID == AppConsts.ONE).Any())
                    {
                        IsAdminProfileSharingPermission = false;
                        return new Tuple<Boolean, String>(IsAdminProfileSharingPermission, agencyInstitution.Where(con => con.Key == result.Where(cond => cond.AIP_AgencyInstitutionPermissionAccessTypeID == AppConsts.TWO).FirstOrDefault().AIP_AgencyInstitutionID).Select(sel => sel.Value).FirstOrDefault());
                    }
                }
            }
            return new Tuple<Boolean, String>(IsAdminProfileSharingPermission, string.Empty);
        }
        #endregion

        Boolean IProfileSharingClientRepository.SaveAgencyHierarchyMapping(AgencyHierarchyContract agencyHierarchyContract, Int32 currentLoggedInUserID, Int32 agencyID, Int32 agencyInstitutionID)
        {
            List<AgencyHierarchyMapping> lstExistingAgencyHierarchyMapping = _dbContext.AgencyHierarchyMappings
                                                                                .Where(cond => !cond.AGHM_IsDeleted && cond.AGHM_AgencyID == agencyID)
                                                                                .ToList();
            List<Int32> updatedNodeIDs = agencyHierarchyContract.HierarchyIDs.Split(',').Select(col => Convert.ToInt32(col)).ToList();


            foreach (AgencyHierarchyMapping existingAgencyHierarchyMapping in lstExistingAgencyHierarchyMapping)
            {
                if (!updatedNodeIDs.Contains(existingAgencyHierarchyMapping.AGHM_HierarchyID))
                {
                    existingAgencyHierarchyMapping.AGHM_IsDeleted = true;
                    existingAgencyHierarchyMapping.AGHM_ModifiedBy = currentLoggedInUserID;
                    existingAgencyHierarchyMapping.AGHM_ModifiedOn = DateTime.Now;
                }
                else
                {
                    updatedNodeIDs.Remove(existingAgencyHierarchyMapping.AGHM_HierarchyID);
                }
            }

            foreach (Int32 nodeID in updatedNodeIDs)
            {
                AgencyHierarchyMapping newAgencyHierarchyMapping = new AgencyHierarchyMapping()
                {
                    AGHM_AgencyID = agencyID,
                    AGHM_CreatedBy = currentLoggedInUserID,
                    AGHM_CreatedOn = DateTime.Now,
                    AGHM_HierarchyID = nodeID,
                    AGHM_IsDeleted = false
                };
                _dbContext.AgencyHierarchyMappings.AddObject(newAgencyHierarchyMapping);
            }
            #region UAT-2529 - Agency Users Restrictions around Profile Sharing

            var oldAgencyInstitutionPermissions = _dbContext.AgencyInstitutionPermissions.Where(con => agencyInstitutionID == con.AIP_AgencyInstitutionID && !con.AIP_IsDeleted).ToList();
            if (!oldAgencyInstitutionPermissions.IsNullOrEmpty())
            {
                //update
                var updateAgencyInstitutionPermissionForStudent = oldAgencyInstitutionPermissions.Where(con => con.AIP_AgencyInstitutionPermissionTypeID == AppConsts.ONE).FirstOrDefault();
                updateAgencyInstitutionPermissionForStudent.AIP_AgencyInstitutionPermissionAccessTypeID = agencyHierarchyContract.IsStudent ? AppConsts.ONE : AppConsts.TWO;

                var updateAgencyInstitutionPermissionForAdmin = oldAgencyInstitutionPermissions.Where(con => con.AIP_AgencyInstitutionPermissionTypeID == AppConsts.TWO).FirstOrDefault();
                updateAgencyInstitutionPermissionForAdmin.AIP_AgencyInstitutionPermissionAccessTypeID = agencyHierarchyContract.IsAdmin ? AppConsts.ONE : AppConsts.TWO;
            }
            else
            {
                #region Student
                AgencyInstitutionPermission newAgencyInstitutionPermissionForStudent = new AgencyInstitutionPermission();
                newAgencyInstitutionPermissionForStudent.AIP_AgencyInstitutionID = agencyInstitutionID;
                newAgencyInstitutionPermissionForStudent.AIP_AgencyInstitutionPermissionAccessTypeID = agencyHierarchyContract.IsStudent ? AppConsts.ONE : AppConsts.TWO;
                newAgencyInstitutionPermissionForStudent.AIP_AgencyInstitutionPermissionTypeID = AppConsts.ONE;
                newAgencyInstitutionPermissionForStudent.AIP_CreatedByID = currentLoggedInUserID;
                newAgencyInstitutionPermissionForStudent.AIP_CreatedOn = DateTime.Now;
                newAgencyInstitutionPermissionForStudent.AIP_IsDeleted = false;
                _dbContext.AgencyInstitutionPermissions.AddObject(newAgencyInstitutionPermissionForStudent);
                #endregion

                #region Admin
                AgencyInstitutionPermission newAgencyInstitutionPermissionForAdmin = new AgencyInstitutionPermission();
                newAgencyInstitutionPermissionForAdmin.AIP_AgencyInstitutionID = agencyInstitutionID;
                newAgencyInstitutionPermissionForAdmin.AIP_AgencyInstitutionPermissionAccessTypeID = agencyHierarchyContract.IsAdmin ? AppConsts.ONE : AppConsts.TWO;
                newAgencyInstitutionPermissionForAdmin.AIP_AgencyInstitutionPermissionTypeID = AppConsts.TWO;
                newAgencyInstitutionPermissionForAdmin.AIP_CreatedByID = currentLoggedInUserID;
                newAgencyInstitutionPermissionForAdmin.AIP_CreatedOn = DateTime.Now;
                newAgencyInstitutionPermissionForAdmin.AIP_IsDeleted = false;
                _dbContext.AgencyInstitutionPermissions.AddObject(newAgencyInstitutionPermissionForAdmin);
                #endregion


            }
            #endregion
            _dbContext.SaveChanges();
            return true;
        }

        Boolean IProfileSharingClientRepository.DeleteAgencyHierarchyMappings(Int32 agencyID, Int32 loggedInUserID, Int32 agencyInstitutionId)
        {
            List<AgencyHierarchyMapping> lstAgencyHierachies = _dbContext.AgencyHierarchyMappings.Where(cond => !cond.AGHM_IsDeleted && cond.AGHM_AgencyID == agencyID).ToList();
            foreach (AgencyHierarchyMapping agencyHierachy in lstAgencyHierachies)
            {
                agencyHierachy.AGHM_IsDeleted = true;
                agencyHierachy.AGHM_ModifiedBy = loggedInUserID;
                agencyHierachy.AGHM_ModifiedOn = DateTime.Now;
            }
            if (agencyInstitutionId > AppConsts.NONE)
            {
                List<AgencyInstitutionPermission> lstAgencyInstitutionPermission = _dbContext.AgencyInstitutionPermissions.Where(cond => cond.AIP_AgencyInstitutionID == agencyInstitutionId && !cond.AIP_IsDeleted).ToList();
                foreach (AgencyInstitutionPermission agencyInstitutionPermission in lstAgencyInstitutionPermission)
                {
                    agencyInstitutionPermission.AIP_IsDeleted = true;
                    agencyInstitutionPermission.AIP_ModifiedByID = loggedInUserID;
                    agencyInstitutionPermission.AIP_ModifiedOn = DateTime.Now;
                }
            }
            _dbContext.SaveChanges();
            return true;
        }

        #region UAT 1882: Phase 3(12): When a student profile shares, they should be presented with a selectable list of agencies, which have been associated with nodes they have orders with.

        /// <summary>
        /// Returns the list of agency, based on node at which applicant has orders.
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <returns></returns>
        List<AgencyContract> IProfileSharingClientRepository.GetAgencyForApplicant(Int32 orgUserID)
        {
            List<AgencyContract> lstAgency = new List<AgencyContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAgencyForApplicant", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicantOrgUserID", orgUserID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstAgency.Add(new AgencyContract
                        {
                            AgencyID = dr["AG_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AG_ID"]),
                            Name = dr["AG_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AG_Name"])
                        });
                    }
                }
            }
            return lstAgency;
        }
        #endregion

        #region UAT-1881
        DataTable IProfileSharingClientRepository.GetAllAgencyForOrgUser(Int32 OrgUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAgencyForOrgUser", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserId", OrgUserId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }
        #endregion


        #region UAT-2071, Configuration Rotation and Tracking packages must be fully compliant to share
        List<RotationAndTrackingPkgStatusContract> IProfileSharingClientRepository.GetComplianceStatusOfImmunizationAndRotationPackages(String delimittedOrgUserIDs, String delimittedTrackingPkgIDs, Int32 rotationID, String SearchType)
        {
            List<RotationAndTrackingPkgStatusContract> lstRotationAndTrackingPkgStatusContract = new List<RotationAndTrackingPkgStatusContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetComplianceStatusOfImmunizationAndRotation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicantOrgUserIDs", delimittedOrgUserIDs);
                command.Parameters.AddWithValue("@ApplicantCompliancePkgIDs", delimittedTrackingPkgIDs);
                command.Parameters.AddWithValue("@RotationID", rotationID);
                command.Parameters.AddWithValue("@SearchStatusCode", SearchType);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstRotationAndTrackingPkgStatusContract.Add(new RotationAndTrackingPkgStatusContract
                        {
                            ErrorMessage = dr["ErrorMessage"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ErrorMessage"]),
                            PackageType = dr["PackageType"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["PackageType"]),
                        });
                    }
                }
            }
            return lstRotationAndTrackingPkgStatusContract;
        }
        #endregion


        #region UAT-2051, No defining details about Roation. Roation/Profile simply says "roation shared".
        String IProfileSharingClientRepository.GetSharingInfoByInvitationGrpID(int invtationGrpID)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSharingInfoByInvitationGrpID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProfileSharingGroupID", invtationGrpID);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    var row = ds.Tables[0].AsEnumerable().FirstOrDefault();
                    return row["RotationName"].GetType().Name == "DBNull" ? ("<b>Applicant Names: </b>" + Convert.ToString(row["ApplicantName"])) : ("<b>Rotation Name: </b>" + Convert.ToString(row["RotationName"]));
                }
                return String.Empty;
            }
        }
        #endregion

        #region UAT-2196, Add "Send Message" button on rotation details screen
        Dictionary<Int32, String> IProfileSharingClientRepository.GetOrganizationUserIDByRotMemberIDs(Int32 tenantId, List<Int32> lstRotMemberID)
        {
            List<ClinicalRotationMember> lstClinicalRotMembers = _dbContext.ClinicalRotationMembers.Where(x => lstRotMemberID.Contains(x.CRM_ID) && !x.CRM_IsDeleted).ToList();
            if (!lstClinicalRotMembers.IsNullOrEmpty())
            {
                List<Int32> lstOrgUserIDs = lstClinicalRotMembers.Select(x => x.CRM_ApplicantOrgUserID).ToList();
                return _dbContext.OrganizationUsers.Where(x => lstOrgUserIDs.Contains(x.OrganizationUserID) && x.IsActive && !x.IsDeleted).ToDictionary(v => v.OrganizationUserID, v => String.Concat(v.FirstName, " ", v.LastName));
            }
            return null;
        }
        #region UAT-3463
        Dictionary<Int32, String> IProfileSharingClientRepository.GetOrganizationUserDetailsByOrgUserIDs(Int32 tenantId, List<Int32> lstOrgUserIDs)
        {
            if (!lstOrgUserIDs.IsNullOrEmpty())
            {
                return _dbContext.OrganizationUsers.Where(x => lstOrgUserIDs.Contains(x.OrganizationUserID) && x.IsActive && !x.IsDeleted).ToDictionary(v => v.OrganizationUserID, v => String.Concat(v.FirstName, " ", v.LastName));
            }
            return null;
        }
        #endregion
        #endregion

        #region UAT-2164, Agency User - Granular Permissions
        List<BackgroundDocumentPermissionContract> IProfileSharingClientRepository.GetBackgroundDocumentPermissionByReqPkgID(int requirementPkgID, int loggedInAgencyUserID)
        {
            List<BackgroundDocumentPermissionContract> lstIsBackgroundDocumentContract = new List<BackgroundDocumentPermissionContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("GetBackgroundDocumentPermissionByReqPkgID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequirementPackageID", requirementPkgID);
                command.Parameters.AddWithValue("@OrgUserID", loggedInAgencyUserID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstIsBackgroundDocumentContract.Add(new BackgroundDocumentPermissionContract
                        {
                            RequirementPackageID = dr["RequirementPackageID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementPackageID"]),
                            RequirementCategoryID = dr["RequirementCategoryID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementCategoryID"]),
                            RequirementItemID = dr["RequirementItemID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementItemID"]),
                            RequirementFieldID = dr["RequirementFieldID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementFieldID"]),
                            FieldName = dr["FieldName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["FieldName"]),
                            IsBackgroundDocument = dr["IsBackgroundDocument"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsBackgroundDocument"]),
                            IsDisabled = dr["IsDisabled"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsDisabled"]),
                        });
                    }
                }
            }
            return lstIsBackgroundDocumentContract;
        }
        #endregion

        //UAT-2181:Enhance adding tenants to agencies with check boxes on the Manage Agencies screen to select which agencies you would like to add the selected tenant to
        Boolean IProfileSharingClientRepository.AssignTenantToAgency(Int32 TenantID, List<Int32> SelectedAgencyIDs, Int32 CurrentLoggedInUserId)
        {
            Int32 InstitutionHierarchyID = _dbContext.DeptProgramMappings.Where(cmd => cmd.DPM_ParentNodeID == null && !cmd.DPM_IsDeleted).First().DPM_ID;

            List<Int32> ExistingMapping = _dbContext.AgencyHierarchyMappings.Where(cnd => SelectedAgencyIDs.Contains(cnd.AGHM_AgencyID) && cnd.AGHM_HierarchyID == InstitutionHierarchyID && !cnd.AGHM_IsDeleted).Select(sel => sel.AGHM_AgencyID).ToList();

            foreach (Int32 id in SelectedAgencyIDs)
            {
                if (id.IsNotNull() && !ExistingMapping.Contains(id))
                {
                    AgencyHierarchyMapping agencyHierarchyMapping = new AgencyHierarchyMapping();
                    agencyHierarchyMapping.AGHM_AgencyID = id;
                    agencyHierarchyMapping.AGHM_HierarchyID = InstitutionHierarchyID;
                    agencyHierarchyMapping.AGHM_CreatedBy = CurrentLoggedInUserId;
                    agencyHierarchyMapping.AGHM_CreatedOn = DateTime.Now;
                    agencyHierarchyMapping.AGHM_IsDeleted = false;
                    _dbContext.AgencyHierarchyMappings.AddObject(agencyHierarchyMapping);
                }
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        Boolean IProfileSharingClientRepository.IsRotationContainsRotationPkg(Int32 rotationId)
        {
            return _dbContext.ClinicalRotationRequirementPackages
                                                         .Any(cond => !cond.CRRP_IsDeleted
                                                             && cond.CRRP_ClinicalRotationID == rotationId
                                                             && !cond.RequirementPackage.RP_IsDeleted
                                                             );
        }

        DataSet IProfileSharingClientRepository.GetRequirementLiveData(Int32 requirementPackageSubscriptionID)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetRequirementLiveData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequirementPackageSubscriptionID", requirementPackageSubscriptionID);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                return new DataSet();
            }
        }

        #region UAT-2414, Create a snapshot on Rotation End Date
        List<RequirmentPkgSubscriptionDataContract> IProfileSharingClientRepository.GetRequirementSubscriptionDataForSnapshot(Int32 chunkSize)
        {
            List<RequirmentPkgSubscriptionDataContract> lstRequirmentPkgSubscriptionDataContract = new List<RequirmentPkgSubscriptionDataContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetRequirementSubscriptionDataForSnapshot", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstRequirmentPkgSubscriptionDataContract.Add(new RequirmentPkgSubscriptionDataContract
                        {
                            ClinicalRotationID = dr["ClinicalRotationID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["ClinicalRotationID"]),
                            RequirementPackageSubscriptionID = dr["RequirementPackageSubscriptionID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["RequirementPackageSubscriptionID"]),
                            ProfileSharingInvitationIDs = dr["ProfileSharingInvitationIDs"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ProfileSharingInvitationIDs"])
                        });
                    }
                }
                return lstRequirmentPkgSubscriptionDataContract;
            }
        }

        Boolean IProfileSharingClientRepository.SaveRequirementSnapshotOnRotationEnd(Int32 reqPackageSubscrptionId, Int32 currentUserId, String profileSharingInvitationIDs)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_CreateRequirementSnapshotOnRotationEnd", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@RequirementPackageSubscriptionID", reqPackageSubscrptionId);
                command.Parameters.AddWithValue("@ProfileSharingInvitationIDs", profileSharingInvitationIDs);
                command.Parameters.AddWithValue("@BackgroundProcessID", currentUserId);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                command.ExecuteNonQuery();
                con.Close();
            }
            return true;
        }

        #endregion
        void IProfileSharingClientRepository.CopySharedInvForNewlyMappedAgencyForAgencyUser(Int32 AgencyUserId, String AgencyIds, Int32 TenantId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_CopySharedInvForNewlyMappedAgencyForAgencyUser", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyUserID", AgencyUserId);
                command.Parameters.AddWithValue("@AgencyIds", AgencyIds);
                command.Parameters.AddWithValue("@TenantID", TenantId);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        //UAT-2538
        Int32 IProfileSharingClientRepository.GetAgencyIDsForRotinvAppRejNotification(Int32 RotationID)
        {
            //return _dbContext.ClinicalRotationAgencies.Where(cmd => lstRotationIDs.Contains(cmd.CRA_ClinicalRotationID.HasValue ? cmd.CRA_ClinicalRotationID.Value : 0) && cmd.CRA_IsDeleted == false).Select(sel => sel.CRA_AgencyID.HasValue ? sel.CRA_AgencyID.Value : 0).Distinct().ToList();
            return _dbContext.ClinicalRotationAgencies.Where(cmd => cmd.CRA_ClinicalRotationID == RotationID && !cmd.CRA_IsDeleted).Select(sel => sel.CRA_AgencyID.Value).FirstOrDefault();
        }

        //List<OrganizationUser> IProfileSharingClientRepository.GetClinicalRotationMemberData(Int32 ClinicalRotationID)
        //{
        //    List<Int32> ApplicantOrgUserID = _dbContext.ClinicalRotationMembers.Where(cond => cond.CRM_ClinicalRotationID == ClinicalRotationID && !cond.CRM_IsDeleted).Select(sel => sel.CRM_ApplicantOrgUserID).ToList();
        //    return _dbContext.OrganizationUsers.Where(cmd => ApplicantOrgUserID.Contains(cmd.OrganizationUserID) && !cmd.IsDeleted && cmd.IsActive).ToList();
        //}

        ClinicalRotation IProfileSharingClientRepository.GetClinicalRotation(Int32 ClinicalRotationID)
        {
            return _dbContext.ClinicalRotations.Where(cond => cond.CR_ID == ClinicalRotationID && !cond.CR_IsDeleted).FirstOrDefault();

        }

        #region UAT-2639:Agency hierarchy mapping: Default Hierarchy for Client Admin
        private void SaveAgHierarchyInstNodeForAgencyCreation(String nodeIds, Int32 agencyHierarchyId, Int32 currentLoggedInUserID)
        {
            List<Int32> listNodeIDs = nodeIds.Split(',').Select(col => Convert.ToInt32(col)).ToList();

            listNodeIDs.ForEach(nodeid =>
            {
                AgencyHierarchyInstitutionNode agHierarchyInstNode = new AgencyHierarchyInstitutionNode();
                agHierarchyInstNode.AHIN_DeptProgMappingID = nodeid;
                agHierarchyInstNode.AHIN_AgencyHierarchyID = agencyHierarchyId;
                agHierarchyInstNode.AHIN_CreatedBy = currentLoggedInUserID;
                agHierarchyInstNode.AHIN_CreatedOn = DateTime.Now;
                _dbContext.AgencyHierarchyInstitutionNodes.AddObject(agHierarchyInstNode);
            });
        }
        #endregion

        #region UAT-2639:Agency hierarchy mapping: Default Hierarchy for Client Admin
        DeptProgramMapping IProfileSharingClientRepository.GetClientAdminRootNode(Int32 tenantId)
        {
            return _dbContext.DeptProgramMappings.FirstOrDefault(cnd => cnd.DPM_ParentNodeID == null && cnd.DPM_IsDeleted == false);
        }
        #endregion

        #region UAT-2640:Update Agency User (People and Places > Manage Agencies) Agency multi select dropdown will be removed  and multiple hierarchy selection option will be provided here.
        Boolean IProfileSharingClientRepository.SaveAgencyHirInstNodeMappingForClientAdmin(AgencyHierarchyContract agencyHierarchyContract, Int32 currentLoggedInUserID, Int32 agencyID, Int32 agencyInstitutionID, Boolean isAdminLoggedIn)
        {

            //UAT-2639
            if (!agencyHierarchyContract.IsNullOrEmpty() && !isAdminLoggedIn && agencyHierarchyContract.AgencyHierarchyID > AppConsts.NONE)
            {
                SaveAgHierarchyInstNodeForAgencyCreation(agencyHierarchyContract.HierarchyIDs, agencyHierarchyContract.AgencyHierarchyID, currentLoggedInUserID);
            }
            _dbContext.SaveChanges();
            return true;
        }

        #endregion

        #region UAT-2511:
        SharedUserRotationReview IProfileSharingClientRepository.GetShareduserRotationReviewStatusByRotationIds(Int32 rotationID, Int32 inviteeOrgUserID, Int32 agencyID)
        {
            return _dbContext.SharedUserRotationReviews.FirstOrDefault(cond => cond.SURR_ClinicalRotaionID == rotationID
                                                                                   && cond.SURR_OrganizationUserID == inviteeOrgUserID
                                                                                   && cond.SURR_AgencyID == agencyID
                                                                                    && !cond.SURR_IsDeleted);
        }
        #endregion
        #region UAT 2548
        public List<AgencyApplicantShareHistoryContract> GetApplicantProfileSharingHistory(AgencyApplicantShareHistoryContract agencyApplicantShareHistoryContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<AgencyApplicantShareHistoryContract> requirementCategoriesDetailList = new List<AgencyApplicantShareHistoryContract>();
            String orderBy = "InvitationDate";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            List<AgencyApplicantShareHistoryContract> lstAgencyApplicantShareHistoryContract = new List<AgencyApplicantShareHistoryContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ApplicantId",agencyApplicantShareHistoryContract.ApplicantId),
                             new SqlParameter("@AgencyOrgUserID",agencyApplicantShareHistoryContract.AgencyOrgUserID),
                             new SqlParameter("@TenantId",agencyApplicantShareHistoryContract.TenantId),
                             new SqlParameter("@OrderBy", orderBy),
                             new SqlParameter("@OrderDirection", ordDirection),
                             new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                             new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetApplicantProfileSharingHistory", sqlParameterCollection))
                {
                    while (dr.Read())
                    {
                        AgencyApplicantShareHistoryContract agencyObjApplicantShareHistoryContract = new AgencyApplicantShareHistoryContract();
                        agencyObjApplicantShareHistoryContract.AgencyName = dr["AgencyName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AgencyName"]);
                        agencyObjApplicantShareHistoryContract.InvitationDate = dr["InvitationDate"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InvitationDate"]);
                        agencyObjApplicantShareHistoryContract.ReviewStatus = dr["ReviewStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ReviewStatus"]);
                        agencyObjApplicantShareHistoryContract.SharingType = dr["SharingType"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SharingType"]);
                        agencyObjApplicantShareHistoryContract.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                        agencyObjApplicantShareHistoryContract.InvitationID = dr["InvitationID"] != DBNull.Value ? Convert.ToInt32(dr["InvitationID"]) : 0;
                        agencyObjApplicantShareHistoryContract.PSIExpirationDate = dr["PSI_ExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["PSI_ExpirationDate"]);
                        agencyObjApplicantShareHistoryContract.PSIMaxViews = dr["PSI_MaxViews"] != DBNull.Value ? Convert.ToInt32(dr["PSI_MaxViews"]) : (Int32?)null;
                        agencyObjApplicantShareHistoryContract.IsInvSharedByAppByAgencyDDl = dr["IsInvShdByAppByAgencyDDl"] != DBNull.Value ? Convert.ToInt32(dr["IsInvShdByAppByAgencyDDl"]) : 0; //UAT-3387

                        //Convert.ToInt32(dr["PSI_MaxViews"]) : 0;
                        lstAgencyApplicantShareHistoryContract.Add(agencyObjApplicantShareHistoryContract);
                    }
                }
            }
            return lstAgencyApplicantShareHistoryContract;
        }
        public List<AgencyApplicantStatusContract> GetAgencyApplicantStatus(AgencyApplicantStatusContract agencyApplicantStatusContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<AgencyApplicantShareHistoryContract> requirementCategoriesDetailList = new List<AgencyApplicantShareHistoryContract>();
            String orderBy = "AgencyName";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            List<AgencyApplicantStatusContract> lstAgencyApplicantStatusContract = new List<AgencyApplicantStatusContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@AgencyOrgUserID",agencyApplicantStatusContract.CurrentLoggedInUser),
                             new SqlParameter("@TenantId",agencyApplicantStatusContract.TenantId),
                             new SqlParameter("@AgencyId",agencyApplicantStatusContract.AgencyId),
                             new SqlParameter("@ApplicantName",agencyApplicantStatusContract.ApplicantName),
                             new SqlParameter("@OrderBy", orderBy),
                             new SqlParameter("@OrderDirection", ordDirection),
                             new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                             new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyApplicantStatus", sqlParameterCollection))
                {
                    while (dr.Read())
                    {
                        AgencyApplicantStatusContract agencyObjApplicantStatusContract = new AgencyApplicantStatusContract();
                        agencyObjApplicantStatusContract.ApplicantName = dr["ApplicantName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                        agencyObjApplicantStatusContract.SubscriptionStatus = dr["SubscriptionStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SubscriptionStatus"]);
                        agencyObjApplicantStatusContract.BackgroundCheckStatus = dr["BackgroundCheckStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BackgroundCheckStatus"]);
                        agencyObjApplicantStatusContract.ProfileShareStatus = dr["ProfileShareStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ProfileShareStatus"]);
                        agencyObjApplicantStatusContract.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                        agencyObjApplicantStatusContract.CompliancePackageID = dr["CompliancePackageID"] != DBNull.Value ? Convert.ToInt32(dr["CompliancePackageID"]) : 0;
                        agencyObjApplicantStatusContract.ApplicantId = dr["ApplicantId"] != DBNull.Value ? Convert.ToInt32(dr["ApplicantId"]) : 0;
                        agencyObjApplicantStatusContract.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                        agencyObjApplicantStatusContract.RequirementPackageSubscriptionID = dr["RequirementPackageSubscriptionID"] != DBNull.Value ? Convert.ToInt32(dr["RequirementPackageSubscriptionID"]) : 0;
                        lstAgencyApplicantStatusContract.Add(agencyObjApplicantStatusContract);
                    }
                }
            }
            return lstAgencyApplicantStatusContract;
        }
        #endregion

        List<String> IProfileSharingClientRepository.FilterApplicantHavingOnlyNonActiveOrExpireOrders(String delimittedCRMIDs)
        {
            List<String> lstApplicantNames = new List<string>();
            List<RotationAndTrackingPkgStatusContract> lstRotationAndTrackingPkgStatusContract = new List<RotationAndTrackingPkgStatusContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetApplicantHavingOnlyNonActiveOrExpireOrders", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClinicalRotationMemeberIds", delimittedCRMIDs);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstApplicantNames.Add(dr["ApplicantName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ApplicantName"]));
                    }
                }
            }
            return lstApplicantNames;
        }


        #region UAT-2847
        List<Int32> IProfileSharingClientRepository.GetAgenciesByRotationID(Int32 rotationID)
        {
            List<Int32> AgencyIds = new List<Int32>();
            var agencyList = _dbContext.ClinicalRotationAgencies.Where(s => s.CRA_ClinicalRotationID == rotationID && !s.CRA_IsDeleted && s.CRA_AgencyID.HasValue).Select(s => s.CRA_AgencyID).ToList();
            agencyList.ForEach(s => AgencyIds.Add(Convert.ToInt32(s)));
            return AgencyIds;
        }
        #endregion

        DataTable IProfileSharingClientRepository.GetUpdatedRotationItems(Int32 agencyID, String rotationIds, DateTime fromDate, DateTime toDate)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetUpdatedRotationItems", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RotationIds", rotationIds);
                command.Parameters.AddWithValue("@AgencyID", agencyID);
                command.Parameters.AddWithValue("@FromDate", fromDate);
                command.Parameters.AddWithValue("@ToDate", toDate);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }

            }
            return new DataTable();
        }

        #region UAT-3238
        List<ApplicantDocument> IProfileSharingClientRepository.GetApplicantDocumentDetailsByDocumentIds(List<Int32> documentIds)
        {
            return _dbContext.ApplicantDocuments.Where(d => documentIds.Contains(d.ApplicantDocumentID)).ToList();
        }
        #endregion

        #region UAT-3254
        String IProfileSharingClientRepository.GetSharedSubscriptionsSelectedNodeIds(Int32 profileSharingInvitationId)
        {
            String hierarchyNodeIds = String.Empty;
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSharedSubscriptionsSelectedNodeIds", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProfileSharingInvitationId", profileSharingInvitationId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        String NodeIds = dr["hierarchyNodeIds"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["hierarchyNodeIds"]);
                        hierarchyNodeIds = NodeIds;
                    }
                }
            }
            return hierarchyNodeIds;
        }
        String IProfileSharingClientRepository.GetRotationHierarchyIdsByRotationID(Int32 RotationId)
        {
            String rotationHirarchyIds = String.Empty;
            ClinicalRotation rotationDetails = _dbContext.ClinicalRotations.Where(cond => !cond.CR_IsDeleted && cond.CR_ID == RotationId).FirstOrDefault();
            if (!rotationDetails.IsNullOrEmpty() && !rotationDetails.ClinicalRotationHierarchyMappings.IsNullOrEmpty())
            {
                List<Int32> lstRotationHierarchyIds = rotationDetails.ClinicalRotationHierarchyMappings.Where(cond => !cond.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
                rotationHirarchyIds = String.Join(",", lstRotationHierarchyIds);
            }
            return rotationHirarchyIds;
        }
        #endregion

        #region UAT-3805
        DataTable IProfileSharingClientRepository.GetItemDocNotificationDataOnCategoryApproval(String categoryIds, Int32 applicantOrgUserID, String approvedCategoryIds
                                                                               , String requestTypeCode, Int32? packageSubscriptionID, Int32? RPS_ID)
        {

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetItemDocNotificationDataOnCatgeoryApproval", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserID", applicantOrgUserID);
                command.Parameters.AddWithValue("@CategoryIDs", categoryIds);
                command.Parameters.AddWithValue("@RPSID", RPS_ID);
                command.Parameters.AddWithValue("@PSID", packageSubscriptionID);
                command.Parameters.AddWithValue("@RequestType", requestTypeCode);
                command.Parameters.AddWithValue("@AlreadyApprovedCategoryIDs", approvedCategoryIds);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }

            }
            return new DataTable();

        }

        List<Int32> IProfileSharingClientRepository.GetApprovedCategorIDs(Int32 subscriptionID, List<Int32> categoryIDs, String requestType, Int32 approvedStatusID
                                                                          , Int32? categoryStatusID_ExceptionallyApproved)
        {
            if (String.Compare(requestType, lkpUseTypeEnum.COMPLIANCE.GetStringValue(), true) == AppConsts.NONE)
            {
                return _dbContext.ApplicantComplianceCategoryDatas.Where(cnd => cnd.PackageSubscriptionID == subscriptionID && !cnd.IsDeleted
                                                                         && (categoryIDs.Count() == AppConsts.NONE || categoryIDs.Contains(cnd.ComplianceCategoryID))
                                                                         && (cnd.StatusID == approvedStatusID || cnd.StatusID == categoryStatusID_ExceptionallyApproved))
                                                                             .Select(slct => slct.ComplianceCategoryID).ToList();
            }
            else if (String.Compare(requestType, lkpUseTypeEnum.ROTATION.GetStringValue(), true) == AppConsts.NONE)
            {
                return _dbContext.ApplicantRequirementCategoryDatas.Where(cnd => cnd.ARCD_RequirementPackageSubscriptionID == subscriptionID && !cnd.ARCD_IsDeleted
                                                                         && (categoryIDs.Count() == AppConsts.NONE || categoryIDs.Contains(cnd.ARCD_RequirementCategoryID))
                                                                         && cnd.ARCD_RequirementCategoryStatusID == approvedStatusID)
                                                                                .Select(slct => slct.ARCD_RequirementCategoryID).ToList();
            }
            return new List<Int32>();

        }

        List<PackageSubscription> IProfileSharingClientRepository.GetCompliancePkgSubscriptionData(List<CompliancePackageCategory> lstCompPackageCategories)
        {
            List<Int32> packageIds = lstCompPackageCategories.Select(slct => slct.CPC_PackageID).ToList();
            return _dbContext.PackageSubscriptions.Where(x => packageIds.Contains(x.CompliancePackageID) && !x.IsDeleted).ToList();
        }
        List<RequirementPackageSubscription> IProfileSharingClientRepository.GetRequirementPkgSubscriptionData(List<RequirementPackageCategory> lstReqPackageCategories)
        {
            List<Int32> packageIds = lstReqPackageCategories.Select(slct => slct.RPC_RequirementPackageID).ToList();
            return _dbContext.RequirementPackageSubscriptions.Where(x => packageIds.Contains(x.RPS_RequirementPackageID.Value) && !x.RPS_IsDeleted).ToList();
        }

        List<RequirementPackageSubscription> IProfileSharingClientRepository.GetReqSubscriptionByObjectIDs(List<Guid> lstCategoryCodes)
        {
            List<RequirementPackageSubscription> lstReqPaclSub = new List<RequirementPackageSubscription>();

            var lst = _dbContext.RequirementCategories.Where(x => lstCategoryCodes.Contains(x.RC_Code.Value) && !x.RC_IsDeleted)
                                                .Select(slct => slct.ApplicantRequirementCategoryDatas.Where(an => !an.ARCD_IsDeleted && !an.RequirementPackageSubscription.RPS_IsDeleted)
                                                .Select(sl => sl.RequirementPackageSubscription)).ToList();


            lst.Distinct().ForEach(x =>
            {
                    lstReqPaclSub.Add(x.FirstOrDefault());
            });
            return lstReqPaclSub;
        }
        #endregion
    }
}