using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;

namespace CoreWeb.ProfileSharing.Views
{
    public class ApplicantInvitationDetailsPresenter : Presenter<IApplicantInvitationDetails>
    {
        /// <summary>
        /// Gets the Lookup Expiration Types i.e. lkpInvitationExpirationType
        /// </summary> 
        public void BindExpirationTypes()
        {
            var _lstExpirationTypes = ProfileSharingManager.GetExpirationTypes();
            View.lstExpirationTypes = _lstExpirationTypes.Where(et => et.Code != InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue()).ToList();
        }

        /// <summary>
        /// Gets the Lookup Expiration Types i.e. lkpInvitationExpirationType
        /// </summary>  
        public void BindApplicantMetaData()
        {
            View.lstMetaData = ProfileSharingManager.GetApplicantMetaData();
        }

        /// <summary>
        /// Gets the Lookup for lkpInvitationSharedInfoType
        /// </summary>   
        public void BindSharedInfoType()
        {
            var _lstSharedInfoType = ProfileSharingManager.GetSharedInfoType(View.TenantId);
            View.lstShardInfoType = _lstSharedInfoType.Where(sit => sit.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_COMPLIANCE.GetStringValue()
                                   && sit.Code != SharedInfoType.COMPLIANCE_NONE.GetStringValue()
                                   && sit.Code != SharedInfoType.COMPLIANCE_ATTESTATION_ONLY.GetStringValue()).ToList();
        }

        /// <summary>
        /// Get the compliance and background packages that can be shared by the applicant
        /// </summary>
        public void BindSharingPackages()
        {
            View.lstSharingPackages = StoredProcedureManagers.GetSharingPackages(View.OrgUserId, View.TenantId);
        }

        /// <summary>
        /// //UAT-4472 Get the Agency Hierachy Rotation Field Option Settings
        /// </summary>
        public void GetValidationRotation()
        {
            if (!View.CurrentAgencyID.IsNullOrEmpty() && View.CurrentAgencyID != 0)
            {
                    View.agencyHierarchyRotationFieldOptionContract = ClinicalRotationManager.GetAgencyHierarchyRotationFieldOptionSetting(View.TenantId, Convert.ToString(View.CurrentAgencyID));
            }

        }

        /// <summary>
        /// Gets the list of invitations that has been sent by the applicant
        /// </summary>
        public void BindInvitations()
        {
            View.lstInvitationsSent = ProfileSharingManager.GetApplicantInvitations(View.OrgUserId, View.TenantId);
        }

        /// <summary>
        ///  Update the Invitation status to Revoked
        /// </summary>
        public void RevokeInvitation()
        {
            //ProfileSharingManager.UpdateInvitationStatus(LkpInviationStatusTypes.REVOKED.GetStringValue(), View.InvitationId, View.OrgUserId);
            ProfileSharingManager.UpdateInvitationStatus(LkpInviationStatusTypes.REVOKED.GetStringValue(), View.InvitationId, View.AppViewOrgUsrID);
        }

        /// <summary>
        /// Gets the Invitation related data for the selected Invitation, in Edit Mode
        /// </summary>  
        /// <returns></returns>
        public void GetInvitationDetails()
        {
            //to do UAT-2447
            View.InvitationData = ProfileSharingManager.GetInvitationData(View.InvitationId, View.TenantId);
        }

        public void ResendInvitation()
        {

            if (View.ProfileSharingUrl.IsNullOrEmpty())
            {
                View.IsInvitationSent = false;
                return;
            }
            var _invitationData = ProfileSharingManager.GetInvitationData(View.InvitationId, View.TenantId);

            //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
            Boolean isEmailNeedToBeSend = false;
            if (!_invitationData.IsNullOrEmpty() && _invitationData.InviteeOrgUserId.IsNull())
            {
                isEmailNeedToBeSend = true;
            }
            else if (!_invitationData.IsNullOrEmpty() && !_invitationData.EmailAddress.IsNullOrEmpty())
            {
                String emailID = _invitationData.EmailAddress.Trim();
                Int32 agencyuserID = ProfileSharingManager.GetAgencyUserNotificationPermissionThroughEmailID(emailID, AgencyUserNotificationLookup.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue());
                if (agencyuserID != AppConsts.NONE)
                {
                    isEmailNeedToBeSend = true;
                }
            }
            if (isEmailNeedToBeSend)
            {
                var queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.QUERY_STRING_INVITE_TOKEN, Convert.ToString(_invitationData .InvitationToken)},
                                                                    {AppConsts.QUERY_STRING_USER_TYPE_CODE, OrganizationUserType.ApplicantsSharedUser.GetStringValue()}
                                                                 };
                var url = String.Format(View.ProfileSharingUrl + "?args={0}", queryString.ToEncryptedQueryString());

                var applicantInfo = SecurityManager.GetOrganizationUser(View.OrgUserId);
                var applincatInfoContract = ProfileSharingManager.ConvertOrgUserIntoApplicantInfoContract(applicantInfo, View.TenantId);
                var _dicContent = new Dictionary<String, String>();

                _dicContent.Add(AppConsts.PSIEMAIL_PROFILEURL, url);
                //_dicContent.Add(AppConsts.PSIEMAIL_CENTRALLOGINURL, View.CentralLoginUrl);
                _dicContent.Add(AppConsts.PSIEMAIL_STUDENTNAME, View.ApplicantName);
                _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTNAME, _invitationData.Name);
                _dicContent.Add(AppConsts.PSIEMAIL_CUSTOMMESSAGE, _invitationData.CustomMessage);
                _dicContent.Add(AppConsts.PSIEMAIL_SELECTEDPACKAGEDATA, GetPackagestring());
                _dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, ProfileSharingManager.GenerateApplicantMetaDataString(applincatInfoContract,
                    _invitationData.SharedApplicantMetaDataCode, View.TenantId, String.Empty, String.Empty));
                _dicContent.Add(EmailFieldConstants.USER_FULL_NAME, _invitationData.Name);

                //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
                _dicContent.Add(AppConsts.PSIEMAIL_APPLICATION_URL, url);
                _dicContent.Add(AppConsts.PSIEMAIL_USER_FULL_NAME, _invitationData.Name);
                _dicContent.Add(AppConsts.PSIEMAIL_APPLICANT_NAME, View.ApplicantName);

                ProfileSharingManager.SendInvitationEmail(View.ProfileSharingUrl, _invitationData.EmailAddress, String.Empty, String.Empty, false, _dicContent, false, View.TenantId, _invitationData.InviteeOrgUserId.HasValue ? _invitationData.InviteeOrgUserId.Value : AppConsts.NONE);
            } View.IsInvitationSent = true;
        }

        public void IsSharedUserInvited(String emailAddress)
        {
            View.IsSharedUserInvited = ProfileSharingManager.IsSharedUserInvited(emailAddress);
        }

        /// <summary>
        /// Returns whether a new invitaion is to be sent or existing is to be updated.
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public void IsNewInvitationRequired()
        {
            var _data = ProfileSharingManager.IsNewInvitationRequired(View.InvitationData, View.TenantId);
            View.IsNewInvitationRequired = _data.Item1;
            View.PreviousPSIId = _data.Item2.PSI_ID;
            View.PreviousEmailAddress = _data.Item2.PSI_InviteeEmail;
        }


        /// <summary>
        ///  Update the Invitation
        /// </summary>
        public void UpdateInvitation()
        {
            ProfileSharingManager.UpdateInvitationDetails(View.InvitationData, View.TenantId);

            //Change status to Pending Review Status for updated invitation
            var pendingReviewStatusCode = SharedUserInvitationReviewStatus.PENDING_REVIEW.GetStringValue();
            int pendingReviewStatusId = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>().Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_Code == pendingReviewStatusCode).First().SUIRS_ID;
            List<int> lstInvitationNeedtoUpdate = new List<int>();
            lstInvitationNeedtoUpdate.Add(View.InvitationData.PSIId);
            ProfileSharingManager.SaveUpdateSharedUserInvitationReviewStatus(lstInvitationNeedtoUpdate, View.OrgUserId, View.OrgUserId, pendingReviewStatusId);
        }

        private String GetPackagestring()
        {
            var _data = ProfileSharingManager.GetSharedPackages(View.InvitationId, View.TenantId);

            StringBuilder _sbPkgs = new StringBuilder();
            _sbPkgs.Append("This invitation covers the following information pertaining to my record: <br />");
            foreach (var pkg in _data.Item1)
            {
                var _pkgName = pkg.PackageSubscription.CompliancePackage.PackageLabel.IsNullOrEmpty()
                     ? pkg.PackageSubscription.CompliancePackage.PackageName
                     : pkg.PackageSubscription.CompliancePackage.PackageLabel;

                GenerateString(_sbPkgs, _pkgName, pkg.SharedSubscriptionCategories.ToList(), null);
            }
            foreach (var pkg in _data.Item2)
            {
                var _pkgName = pkg.BkgOrderPackage.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name.IsNullOrEmpty()
                     ? pkg.BkgOrderPackage.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name
                     : pkg.BkgOrderPackage.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label;

                GenerateString(_sbPkgs, _pkgName, null, pkg.SharedBkgPackageSvcGroups.ToList());
            }
            return Convert.ToString(_sbPkgs);
        }

        private static void GenerateString(StringBuilder _sbPkgs, String pkgName, List<SharedSubscriptionCategory> lstSubscriptions, List<SharedBkgPackageSvcGroup> lstBkgPackages)
        {
            _sbPkgs.Append("<ul style='list-style-type: disc'>");
            _sbPkgs.Append("<li>" + pkgName + "</li>");

            if (lstSubscriptions.IsNotNull() && lstSubscriptions.Count() > 0)
            {
                _sbPkgs.Append("<ul style='list-style-type: circle'>");
                foreach (var data in lstSubscriptions)
                {
                    var __catName = data.ComplianceCategory.CategoryLabel.IsNullOrEmpty()
                                         ? data.ComplianceCategory.CategoryName
                                        : data.ComplianceCategory.CategoryLabel;

                    _sbPkgs.Append("<li>" + __catName + "</li>");
                }
                _sbPkgs.Append("</ul>");
            }
            if (lstBkgPackages.IsNotNull() && lstBkgPackages.Count() > 0)
            {
                _sbPkgs.Append("<ul style='list-style-type: circle'>");
                foreach (var data in lstBkgPackages)
                {
                    _sbPkgs.Append("<li>" + data.BkgSvcGroup.BSG_Name + "</li>");
                }
                _sbPkgs.Append("</ul>");
            }
            _sbPkgs.Append("</ul>");
        }


        public Int32 IsExistingUserInvited(String invitationEmail)
        {
            return SecurityManager.IsExistingUserInvited(invitationEmail);
        }

        //UAT-1318
        public Int32 GetUserTypeIdByCode()
        {
            return ProfileSharingManager.GetUserTypeIdByCode(OrganizationUserType.ApplicantsSharedUser.GetStringValue());
        }

        //UAT-1883: Phase 3(13): Capture of additional fields
        /// <summary>
        /// Get week days List
        /// </summary>
        public void GetWeekDays()
        {
            View.WeekDayList = ClientContactManager.GetWeekDaysList();
        }


        /// <summary>
        /// UAT 1882
        /// </summary>
        public void GetAgencyForApplicant()
        {
            View.lstAgencies = ProfileSharingManager.GetAgencyForApplicant(View.TenantId, View.OrgUserId);
        }

        /// <summary>
        /// UAT 1882
        /// </summary>
        /// <param name="rotationID"></param>
        public void GetAttestationDocumentsToExport(Int32 psiID)
        {
            ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>>();
            serviceRequest.Parameter1 = new Dictionary<String, Int32> { 
                                                                       { AppConsts.PROFILE_SHARING_INVITATION_ID, psiID },
                                                                       { AppConsts.IGNORE_AGENCY_USER_CHECK, AppConsts.ONE } 
                                                                      };
            serviceRequest.Parameter2 = new List<Tuple<Int32, Int32, Int32>>();
            View.LstInvitationDocumentContract = ClinicalRotationManager.GetAttestationDocumentsToExport(serviceRequest, View.OrgUserId);
        }

        public void GetComplianceStatusOfImmunizationAndRotationPackages(String delimittedOrgUserIDs, String delimittedTrackingPkgIDs)
        {
            View.LstErrorMessages = ProfileSharingManager.GetComplianceStatusOfImmunizationAndRotationPackages(View.TenantId, delimittedOrgUserIDs, delimittedTrackingPkgIDs, AppConsts.NONE, "IMNZ");
        }

        public Boolean IsTrackingPkgCompliantReqd(Int32 agencyID)
        {
            return ProfileSharingManager.IsTrackingPkgCompliantReqd(agencyID);
        }

        public void ShowAgencySuggestion()
        {
            int agencySuggestionSettingID = ComplianceDataManager.GetSettings(View.TenantId).WhereSelect(cond => cond.Code == Setting.AGENCY_SUGGESTION_ON_STUDENT_PROFILE_SHARE.GetStringValue(), col => col.SettingID).FirstOrDefault();
            string agencySuggestionSettingValue = ComplianceDataManager.GetClientSetting(View.TenantId).WhereSelect(t => t.CS_SettingID == agencySuggestionSettingID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowAgencySuggestion = String.IsNullOrEmpty(agencySuggestionSettingValue) ? false : ((agencySuggestionSettingValue == "0") ? false : true);
        }
        //UAT-2466
        public void ShowRotationStartDateForIndividualShares()
        {
            int rotationStartDateSettingID = ComplianceDataManager.GetSettings(View.TenantId).WhereSelect(cond => cond.Code == Setting.ROTATION_START_DATE_REQUIRED.GetStringValue(), col => col.SettingID).FirstOrDefault();
            string rotationStartDateSettingValue = ComplianceDataManager.GetClientSetting(View.TenantId).WhereSelect(t => t.CS_SettingID == rotationStartDateSettingID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRotationStartDateForIndividualShares = String.IsNullOrEmpty(rotationStartDateSettingValue) ? true : ((rotationStartDateSettingValue == "0") ? false : true);
        }
        //UAT-2466
        public void ShowRotationEndDateForIndividualShares()
        {
            int rotationEndDateSettingID = ComplianceDataManager.GetSettings(View.TenantId).WhereSelect(cond => cond.Code == Setting.ROTATION_END_DATE_REQUIRED.GetStringValue(), col => col.SettingID).FirstOrDefault();
            string rotationEndDateSettingValue = ComplianceDataManager.GetClientSetting(View.TenantId).WhereSelect(t => t.CS_SettingID == rotationEndDateSettingID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRotationEndDateForIndividualShares = String.IsNullOrEmpty(rotationEndDateSettingValue) ? true : ((rotationEndDateSettingValue == "0") ? false : true);
        }

        #region UAT-2529- Check Applicant individual Profile Sharing Setting
        public void CheckAgencyPermission()
        {
            Tuple<Boolean, String> result = ProfileSharingManager.CheckApplicantIndiviualProfileSharingPermission(View.AgencyUserEmail, View.TenantId);
            View.IsStudentProfileSharingPermission = result.Item1;
            View.AgencyEmail = result.Item2;
        }
        #endregion

        #region UAT-2784
        public Boolean CheckExpirationCriteria(Int32 agencyId)
        {
            String ExpirationCriterialSettingCode = AgencyHierarchySettingType.EXPIRATION_CRITERIA.GetStringValue();
            String ExpirationCriteriaSettingValue = ProfileSharingManager.GetAgencySetting(agencyId, ExpirationCriterialSettingCode);
            if (!ExpirationCriteriaSettingValue.IsNullOrEmpty())
                return ExpirationCriteriaSettingValue == "1" ? true : false;
            return true;
        }
        #endregion

        #region UAT-3662

        public Boolean GetInstPrecpReqdSetting(Int32 agencyId)
        {
            Entity.SharedDataEntity.AgencySetting agencySetting = AgencyHierarchyManager.GetInstPrecpReqdSetting(agencyId);
            Boolean _result = false;
            if (!agencySetting.IsNullOrEmpty() && !agencySetting.AS_SettingValue.IsNullOrEmpty())
            {
                _result = Convert.ToInt32(agencySetting.AS_SettingValue) == AppConsts.TWO ? true : false;
            }
            return _result;
        }
        #endregion
    }
}
