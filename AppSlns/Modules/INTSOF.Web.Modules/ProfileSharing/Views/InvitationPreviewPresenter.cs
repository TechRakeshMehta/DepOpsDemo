using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using Entity.SharedDataEntity;

namespace CoreWeb.ProfileSharing.Views
{
    public class InvitationPreviewPresenter : Presenter<IInvitationPreview>
    {
        /// <summary>
        ///  Save the new invitation
        /// </summary>
        public void SaveInvitation()
        {
            if (View.InvitationData.IsAgencyComboboxSelected)
            {
                Dictionary<Int32, Boolean> assigedOrganisationUserIDs = new Dictionary<int, bool>();
                assigedOrganisationUserIDs.Add(View.InvitationData.ApplicantId, true);

                Dictionary<String, Object> invitationData = new Dictionary<String, Object>();
                invitationData.Add("CurrentUser", View.CurrentUser);
                invitationData.Add("AttestationDate", DateTime.Now);
                invitationData.Add("IsRotationSharing", false);
                invitationData.Add("RotationId", AppConsts.NONE);
                invitationData.Add("SelectedTenantID", View.TenantId);
                invitationData.Add("LstSharedPkgData", null);
                invitationData.Add("IsNonScheduledInvitation", true);
                invitationData.Add("AssignOrganizationUserIds", assigedOrganisationUserIDs);
                invitationData.Add("SelectedAgencyID", View.InvitationData.SelectedAgencyID);
                invitationData.Add("CurrentAdminName", null);
                invitationData.Add("Signature", null);
                invitationData.Add("AttestationReportText", new Dictionary<Int32, String>());
                invitationData.Add("IsAdminLoggedIn", false);
                invitationData.Add("SelectedAgencyName", View.InvitationData.SelectedAgencyName);
                invitationData.Add("InvitationSchedlueDate", null);
                invitationData.Add("InstitutionName", GetTenantName());
                invitationData.Add("CentralLoginUrl", View.CentralLoginUrl);
                invitationData.Add("ProfileSharingURL", View.ProfileSharingUrl);
                invitationData.Add("PXC_ExpireOption", View.InvitationData.ExpireOption);
                invitationData.Add("PXC_ExpirationTypeCode", View.InvitationData.ExpirationTypeCode);
                invitationData.Add("PXC_MaxViews", View.InvitationData.MaxViews);
                invitationData.Add("PXC_ExpirationDate", View.InvitationData.ExpirationDate);
                invitationData.Add("CompliancePkgDataList", View.InvitationData.lstComplianceData);
                invitationData.Add("BkgPkgDataList", View.InvitationData.lstBkgData);
                invitationData.Add("RotationDetail", View.InvitationData.RotationDetail);

                Boolean isApplicantSharing = true;
                Dictionary<String, String> statusMessage = ProfileSharingManager.SendProfileSharingInvitation(invitationData, View.TenantId, isApplicantSharing);

                //View.SuccessMessage = statusMessage[StatusMessages.SUCCESS_MESSAGE.GetStringValue()];
                //View.InfoMessage = statusMessage[StatusMessages.INFO_MESSAGE.GetStringValue()];
                //View.ErrorMessage = statusMessage[StatusMessages.ERROR_MESSAGE.GetStringValue()];

            }
            else
            {
                //var _token = ProfileSharingManager.SaveInvitationDetails(View.InvitationData, View.TenantId, 0);
                var _token = new Guid(); ;
                Tuple<Int32, Guid> invitationToken = ProfileSharingManager.SaveInvitationDetails(View.InvitationData, View.TenantId, 0);
                if (invitationToken.Item1 > AppConsts.NONE)
                {
                    _token = invitationToken.Item2;
                }

                if (String.IsNullOrEmpty(View.ProfileSharingUrl))
                {
                    return;
                }

                var queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {AppConsts.QUERY_STRING_INVITE_TOKEN, Convert.ToString( _token)},
                                                                    {AppConsts.QUERY_STRING_USER_TYPE_CODE, OrganizationUserType.ApplicantsSharedUser.GetStringValue()}
                                                                 };
                var url = String.Format(View.ProfileSharingUrl + "?args={0}", queryString.ToEncryptedQueryString());

                //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 

                View.EmailContent = View.EmailContent.Replace(AppConsts.PSIEMAIL_APPLICATION_URL, url);
                //View.EmailContent = View.EmailContent.Replace(AppConsts.PSIEMAIL_CENTRALLOGINURL, View.CentralLoginUrl);
                View.EmailContent = View.EmailContent.Replace(AppConsts.PSIEMAIL_SELECTEDPACKAGEDATA, View.Packages);
                View.EmailContent = View.EmailContent.Replace(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, View.SharedInformation);

                View.EmailContent = View.EmailContent.Replace(AppConsts.PSIEMAIL_USER_FULL_NAME, View.InvitationData.Name);
                View.EmailContent = View.EmailContent.Replace(AppConsts.PSIEMAIL_CUSTOMMESSAGE, View.InvitationData.CustomMessage);
                View.EmailContent = View.EmailContent.Replace(AppConsts.PSIEMAIL_APPLICANT_NAME, View.ApplicantName);
                Dictionary<String, String> dicData = new Dictionary<string, string>();
                dicData.Add(EmailFieldConstants.USER_FULL_NAME, View.InvitationData.Name);


                //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
                //Boolean isEmailNeedToBeSend = false;
                //if (View.InvitationData.InviteeOrgUserId.IsNull())
                //{
                //    isEmailNeedToBeSend = true;
                //}
                //else
                //{
                //    List<Int32> lstAgencyUserIDs = new List<Int32>();
                //    lstAgencyUserIDs.Add(View.InvitationData.InviteeOrgUserId.Value);

                //    // returns the list of agency users for which notification permission is true and also return the agency users whose permissions are not present in mapping table.
                //    List<Int32> lstAgencyUsersHavingNotifPerm = ProfileSharingManager.GetAgencyUserNotificationPermissionThroughEmailID(lstAgencyUserIDs, AgencyUserNotificationLookup.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue());
                //    if (lstAgencyUsersHavingNotifPerm[0] == View.InvitationData.InviteeOrgUserId.Value) {
                //        isEmailNeedToBeSend = true;
                //    }
                //}


                #region UAT-3443
                Boolean isEmailNeedToBeSend = false;
                Boolean isEmailForSharedUser = false;
                #region UAT-3460
                ClientContact clientContact = ProfileSharingManager.IsInstructorPreceptorUser(Convert.ToString(View.InvitationData.EmailAddress.Trim()));
                #endregion
                ////UAT:UAT-4361
                if (View.InvitationData.InviteeOrgUserId.IsNull() || !clientContact.IsNullOrEmpty())
                {
                    isEmailForSharedUser = true;
                }
                if (View.InvitationData.InviteeOrgUserId >= AppConsts.NONE)
                {
                    isEmailForSharedUser = true;
                }
                
                if (!(ProfileSharingManager.GetIsAgencySharePermissions(Convert.ToString(View.InvitationData.EmailAddress.Trim())))) //UAT-3051
                {
                    isEmailNeedToBeSend = true;
                }
                else
                {
                    isEmailNeedToBeSend = false;
                }

                if (isEmailNeedToBeSend)
                {
                    if (!View.InvitationData.EmailAddress.IsNullOrEmpty())
                    {
                        String emailID = View.InvitationData.EmailAddress.Trim();
                        Int32 agencyuserID = ProfileSharingManager.GetAgencyUserNotificationPermissionThroughEmailID(emailID, AgencyUserNotificationLookup.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue());
                        if (agencyuserID != AppConsts.NONE)
                        {
                            isEmailNeedToBeSend = true;
                        }
                        else
                        {
                            isEmailNeedToBeSend = false;
                        }
                    }
                }

                //Boolean isEmailNeedToBeSend = false;
                //if (View.InvitationData.InviteeOrgUserId.IsNull())
                //{
                //    isEmailNeedToBeSend = true;
                //}
                //else if (!View.InvitationData.EmailAddress.IsNullOrEmpty())
                //{
                //    String emailID = View.InvitationData.EmailAddress.Trim();
                //    Int32 agencyuserID = ProfileSharingManager.GetAgencyUserNotificationPermissionThroughEmailID(emailID, AgencyUserNotificationLookup.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue());
                //    if (agencyuserID != AppConsts.NONE)
                //    {
                //        isEmailNeedToBeSend = true;
                //    }
                //}

                //if (!(ProfileSharingManager.GetIsAgencySharePermissions(Convert.ToString(View.InvitationData.EmailAddress.Trim())))) //UAT-3051
                //{
                //    isEmailNeedToBeSend = true;
                //}
                //else
                //{
                //    isEmailNeedToBeSend = false;
                //}

                #endregion

                if (isEmailNeedToBeSend || isEmailForSharedUser)
                    ProfileSharingManager.SendInvitationEmail(View.ProfileSharingUrl,
                                                                  View.InvitationData.EmailAddress, View.EmailSubject,
                                                                  View.EmailContent, true, dicData, false, View.TenantId, View.InvitationData.InviteeOrgUserId.HasValue ? View.InvitationData.InviteeOrgUserId.Value : AppConsts.NONE);
            }
        }


        ///// <summary>
        ///// Get the Email Subject and Content template from Communication template based on if institution specific or not.
        ///// </summary>
        public void GetEmailContent()
        {
            //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
            CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL;
            Dictionary<String, String> _emailSettings = ProfileSharingManager.GetInvitationEmailContentUsingSubEvent(View.TenantId, commSubEvent.GetStringValue());
            View.EmailSubject = _emailSettings.Keys.First();
            View.EmailContent = _emailSettings.Values.First();
        }

        public void GetApplicantSharedMetaDataString()
        {
            //var applicantInfo = SecurityManager.GetOrganizationUser(View.InvitationData.CurrentUserId);
            var applicantInfo = SecurityManager.GetOrganizationUser(View.InvitationData.ApplicantId);
            var applicantInfoContract = ProfileSharingManager.ConvertOrgUserIntoApplicantInfoContract(applicantInfo, View.TenantId);

            View.SharedInformation = ProfileSharingManager.GenerateApplicantMetaDataString(applicantInfoContract,
                View.InvitationData.SharedApplicantMetaDataCode, View.TenantId, String.Empty, String.Empty);
        }

        private String GetTenantName()
        {
            return ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true)
                                        .Where(col => col.TenantID == View.TenantId).FirstOrDefault().TenantName;
        }
    }
}
