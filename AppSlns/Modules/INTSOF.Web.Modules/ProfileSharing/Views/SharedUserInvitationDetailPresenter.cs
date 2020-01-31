using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Contracts;
using System.Web;
using INTSOF.Utils.CommonPocoClasses;
using Entity.SharedDataEntity;
using INTSOF.ServiceUtil;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;

namespace CoreWeb.ProfileSharing.Views
{
    public class SharedUserInvitationDetailPresenter : Presenter<ISharedUserInvitationDetailView>
    {
        #region UAT-2943
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }
        public Boolean IsNotApprovedReviewStatusSelected(Int32 reviewStatusId)
        {
            Boolean IsNotApprovedReviewStatusSelected = false;
            List<lkpSharedUserInvitationReviewStatu> lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().ToList();
            if (!lstSharedUserInvitationReviewStatus.IsNullOrEmpty())
            {
                lkpSharedUserInvitationReviewStatu sharedUserInvReviewStatus_NotApproved = lstSharedUserInvitationReviewStatus.FirstOrDefault(x => x.SUIRS_ID == reviewStatusId && !x.SUIRS_IsDeleted);
                if (!sharedUserInvReviewStatus_NotApproved.IsNullOrEmpty()
                    && String.Compare(sharedUserInvReviewStatus_NotApproved.SUIRS_Code, SharedUserInvitationReviewStatus.NOT_APPROVED.GetStringValue(), true) == 0)
                {
                    IsNotApprovedReviewStatusSelected = true;
                }
            }
            return IsNotApprovedReviewStatusSelected;
        }
        public override void OnViewInitialized()
        {
            GetInvitationReviewStatus();
        }
        public bool SaveUpdateReviewStatus()
        {
            lkpSharedUserInvitationReviewStatu lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_ID == View.SelectedReviewStatusID).FirstOrDefault();

            List<Int32> lstSelectedInvitationIds = new List<Int32>();
            lstSelectedInvitationIds.Add(View.ProfileSharingInvitationID);

            //Creating dictory containing rotation ids and corresponding invitation ids
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = new List<Tuple<Int32, Int32, Int32, List<Int32>>>();
            Boolean isIndividualReview = true;

            if (!View.RotationID.IsNullOrEmpty())
            {
                List<InvitationIDsDetailContract> lstRotationIdsDetailContract = new List<InvitationIDsDetailContract>();
                lstRotationIdsDetailContract.Add(new InvitationIDsDetailContract { RotationID = Convert.ToInt32(View.RotationID), AgencyID = View.AgencyID, TenantID = View.SelectedTenantID });
                lstRotationData = ProfileSharingManager.GetRotationSharedInvitations(lstRotationIdsDetailContract, View.CurrentLoggedInUserId); //If user select multiple rotation(s) from requirement shares screen
                isIndividualReview = false;
            }

            int tenantId = 0;
            if (!View.SelectedTenantID.IsNullOrEmpty())
                tenantId = View.SelectedTenantID;

            Dictionary<String, String> dicStatusCodeAndNotes = new Dictionary<String, String>();
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_REVIEW_STATUS, !lstSharedUserInvitationReviewStatus.IsNullOrEmpty() ? lstSharedUserInvitationReviewStatus.SUIRS_Code : string.Empty);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_NOTES, string.Empty);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_SCREEN_TYPE, AppConsts.REQUIREMENT_SHARES_SCREEN_NAME);//UAT-2463
            dicStatusCodeAndNotes.Add(AppConsts.CURRENT_USER_ID_QUERY_STRING, Convert.ToString(View.CurrentLoggedInUserId));


            ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> serviceRequest = new ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean>();
            serviceRequest.Parameter1 = dicStatusCodeAndNotes;
            serviceRequest.Parameter2 = lstSelectedInvitationIds;
            serviceRequest.Parameter3 = lstRotationData;
            serviceRequest.Parameter4 = isIndividualReview;
            serviceRequest.SelectedTenantId = tenantId;
            var _serviceResponse = _clinicalRotationProxy.UpdateRotAndInvitationReviewStatus(serviceRequest);
            Boolean Result = _serviceResponse.Result;

            if (Result)
            {
                //UAt-2538
                CallParallelTaskForMail(lstRotationData, lstSharedUserInvitationReviewStatus.SUIRS_Code, lstSelectedInvitationIds); // lstSelectedInvitationIds added as per UAT-2942
            }
            return Result;


        }

        public void CallParallelTaskForMail(List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData, String reviewStatusCode, List<Int32> lstSelectedInvitationIds) // lstSelectedInvitationIds added as per UAT-2942
        {
            Int32 CurrentLoggedInUserID = View.CurrentLoggedInUserId;
            String CurrentUserName = View.CurrentUserFirstName + " " + View.CurrentUserLastName;

            Dictionary<String, Object> Data = new Dictionary<String, Object>();
            Data.Add("lstRotationData", lstRotationData);
            Data.Add("CurrentLoggedInUserID", CurrentLoggedInUserID);
            Data.Add("CurrentUserName", CurrentUserName);
            Data.Add("reviewStatusCode", reviewStatusCode);
            Data.Add("lstSelectedInvitationIds", lstSelectedInvitationIds);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendMailForRotInvAppRej, Data, LoggerService, ExceptiomService);
        }
        public void SendMailForRotInvAppRej(Dictionary<String, Object> data)
        {
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = (List<Tuple<Int32, Int32, Int32, List<Int32>>>)(data.GetValue("lstRotationData"));
            String reviewStatusCode = Convert.ToString(data.GetValue("reviewStatusCode"));
            Int32 CurrentLoggedInUserID = Convert.ToInt32(data.GetValue("CurrentLoggedInUserID"));
            String CurrentUserName = Convert.ToString(data.GetValue("CurrentUserName"));
            List<Int32> lstSelectedInvitationIds = (List<Int32>)(data.GetValue("lstSelectedInvitationIds"));
            ProfileSharingManager.SendMailForRotInvAppRej(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
            ProfileSharingManager.SendNotificationToApplicantOnRequirementApproved(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);

            #region UAT-2942
            if (!lstSelectedInvitationIds.IsNullOrEmpty() && lstSelectedInvitationIds.Count > AppConsts.NONE)
            {
                ProfileSharingManager.SendMailForApprovedApplicantProfileInvitation(lstSelectedInvitationIds, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
            }
            #endregion
        }
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Profile sharing invitation data, Shared Compliance Subscription and shared backgroung packages.
        /// </summary>
        public void GetInvitationDetail()
        {
            if (View.ProfileSharingInvitationID > AppConsts.NONE)
                View.ProfileSharingInvitation = ProfileSharingManager.GetInvitationDetails(View.ProfileSharingInvitationID);
            if (View.SelectedTenantID > AppConsts.NONE)
            {
                View.ListSharedPackageSubscription = ProfileSharingManager.GetSharedComplianceSubscriptions(View.SelectedTenantID, View.ProfileSharingInvitationID);
                View.ListSharedBackgroundPackage = ProfileSharingManager.GetSharedBkgPackages(View.SelectedTenantID, View.ProfileSharingInvitationID,View.CurrentLoggedInUserId, View.IsIndividualShare);
            }
            if (!View.ProfileSharingInvitation.IsNullOrEmpty())
                View.InvitationNotes = View.ProfileSharingInvitation.PSI_InviteeNotes;
            if (View.InvitationGroupTypeCode == ProfileSharingInvitationGroupTypes.ROTATION_SHARING_TYPE.GetStringValue())
                View.SharedRequirementSubscription = ProfileSharingManager.GetSharedRequirementSubscriptions(View.SelectedTenantID, View.ProfileSharingInvitationID);
        }

        public List<InvitationDocumentContract> GetSharedCategoryDocuments(Int32 packageSubscriptionId, String SelectedSharedCategoryIds, String InvitationSourceType, Int32 snapshotId)
        {
            if (View.SelectedTenantID > AppConsts.NONE)
            {
                if (InvitationSourceType == InvitationSourceTypes.APPLICANT.GetStringValue())
                {
                    return ProfileSharingManager.GetSharedCategoryDocuments(View.SelectedTenantID, packageSubscriptionId, SelectedSharedCategoryIds);
                }
                else if (InvitationSourceType == InvitationSourceTypes.CLIENTADMIN.GetStringValue() || InvitationSourceType == InvitationSourceTypes.ADMIN.GetStringValue())
                {
                    return ProfileSharingManager.GetApplicantDocumentsFromSnapshot(View.SelectedTenantID, SelectedSharedCategoryIds, snapshotId);
                }
                else
                {
                    return new List<InvitationDocumentContract>();
                }

            }
            else
            {
                return new List<InvitationDocumentContract>();
            }
        }

        //UAT-1210 - Method to update Invitation View Status
        public void UpdateInvitationViewedStatus()
        {
            ProfileSharingManager.UpdateInvitationViewedStatus(View.CurrentLoggedInUserId, View.ProfileSharingInvitationID);
        }

        #endregion

        #region Private Methods
        public void UpdateViewRemaining()
        {
            String expiredInvitationTypeCode = LkpInviationStatusTypes.EXPIRED.GetStringValue();
            ProfileSharingManager.UpdateInvitationViewsRemaining(View.ProfileSharingInvitationID, View.CurrentLoggedInUserId, expiredInvitationTypeCode);
        }


        #endregion

        public Boolean SaveInvitationNotes()
        {
            return ProfileSharingManager.UpdateInvitationNotes(View.ProfileSharingInvitationID, View.CurrentLoggedInUserId, View.InvitationNotes);
        }

        public List<InvitationDocumentContract> GetSharedRequirementCategoryDocuments(Int32 packageSubscriptionId, String SelectedSharedCategoryIds, String InvitationSourceType, Int32 snapshotId)
        {
            if (View.SelectedTenantID > AppConsts.NONE)
            {
                if (InvitationSourceType == InvitationSourceTypes.CLIENTADMIN.GetStringValue() || InvitationSourceType == InvitationSourceTypes.ADMIN.GetStringValue())
                {

                    return ProfileSharingManager.GetApplicantRequirementDocumentsFromSnapshot(View.SelectedTenantID, SelectedSharedCategoryIds, snapshotId, View.OrganisationUserID, View.RotationID, View.IsApplicantDropped); //UAT 3125
                }
                //UAT-3338
                else if (View.IsInstructorPreceptorPackage)
                {
                    return ProfileSharingManager.GetInstructorRequirementDocuments(View.SelectedTenantID, SelectedSharedCategoryIds, View.OrganisationUserID, View.RotationID, View.SelectedOrganisationUserID);
                }
                else
                {
                    return new List<InvitationDocumentContract>();
                }

            }
            else
            {
                return new List<InvitationDocumentContract>();
            }
        }

        public bool UpdateSharedCategoryData()
        {
            return ProfileSharingManager.UpdateSharedCategoryData(View.SelectedTenantID, View.ProfileSharingInvitationID, View.LstSharedInvitationSubscriptionContract);
        }

        #endregion

        //UAT-1402:Student name and rotation details should display on the invitation details screen.
        public void GetOrganizationUserDetail(Int32 orgUserId)
        {
            View.OrganizationUserDetail = SecurityManager.GetOrganizationUser(orgUserId);
        }

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }

        public String GetMaskDOB(String unMaskedDOB)
        {
            return ApplicationDataManager.GetMaskDOB(unMaskedDOB);
        }

        /// <summary>
        /// Get Package Subscription List By Package Subscriptions IDs
        /// </summary>
        /// <param name="pkgSubIDs"></param>
        /// <returns></returns>
        public List<Entity.ClientEntity.PackageSubscription> GetPackageSubscriptionByIDs(List<Int32> pkgSubIDs)
        {
            String subscriptionIDs = String.Join(",", pkgSubIDs);
            return ComplianceDataManager.GetPackageSubscription(View.SelectedTenantID, subscriptionIDs);
        }

        public Boolean IsIndividualShared()
        {
            if (View.ProfileSharingInvitationID.IsNotNull())
            {
                return ProfileSharingManager.IsIndividualShared(Convert.ToInt32(View.ProfileSharingInvitationID));
            }
            return false;
        }

        public Boolean HasConsidatePassportPermission()
        {
            if (View.ProfileSharingInvitationID.IsNotNull())
            {
                return ProfileSharingManager.HasConsidatePassportPermission(Convert.ToInt32(View.ProfileSharingInvitation.AgencyUser.AGU_ID));
            }
            return false;
        }

        #region UAT-2774
        public void GetSharedUserInvitationDocumentDetails()
        {
            Boolean IsRotaionSharing = false;
            if (!View.RotationID.IsNullOrEmpty())
                IsRotaionSharing = true;
            View.lstSharedUserInvitationDocumentContract = ProfileSharingManager.GetSharedUserInvitationDocumentDetails(View.ProfileSharingInvitationID, View.ProfileSharingInvitation.PSI_ApplicantOrgUserID, IsRotaionSharing);
        }
        public Int32 GetlkpSharedDocMappingType()
        {
            return ProfileSharingManager.GetDocumentTypeIdByCode(LKPSharedSystemDocumentTypes.SHARED_USER_INVITATION_DOCUMENT.GetStringValue());
        }
        public Boolean SaveSharedUserInvitationDocumentDetails()
        {
            return ProfileSharingManager.SaveSharedUserInvitationDocumentDetails(View.lstSharedUserInvitationDocument);
        }
        public Boolean DeletedSharedUserInvitationDocument(Int32 InvitationDocumentID, Int32 ProfileSharingInvitationGroupID)
        {
            Int32 ApplicantOrgUserID = View.ProfileSharingInvitation.PSI_ApplicantOrgUserID;
            return ProfileSharingManager.DeletedSharedUserInvitationDocument(InvitationDocumentID, ApplicantOrgUserID, ProfileSharingInvitationGroupID, View.CurrentLoggedInUserId);
        }
        public String IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, byte[] documentUploadedBytes)
        {
            Int32 ApplicantOrgUserID = View.ProfileSharingInvitation.PSI_ApplicantOrgUserID;
            Int32 ProfileSharingInvitationGroupID = View.ProfileSharingInvitation.PSI_ProfileSharingInvitationGroupID.Value;

            if (ProfileSharingManager.IsDocumentAlreadyUploaded(documentName, documentSize, ApplicantOrgUserID, ProfileSharingInvitationGroupID))
                return documentName;

            //List<InvitationDocument> lstInvitationDocuments = ProfileSharingManager.GetInvitationDocumentsForInvitationApplicant(ProfileSharingInvitationGroupID, ApplicantOrgUserID);
            List<SharedUserInvitationDocumentContract> lstSharedUserInvitationDocument = View.lstSharedUserInvitationDocumentContract;
            String md5Hash = GetMd5Hash(documentUploadedBytes);

            if (!lstSharedUserInvitationDocument.IsNullOrEmpty())
            {
                SharedUserInvitationDocumentContract docDetails = lstSharedUserInvitationDocument.Where(cond => cond.MD5Hash == md5Hash).FirstOrDefault();

                if (!docDetails.IsNullOrEmpty())
                    return docDetails.FileName;
            }

            return String.Empty;
        }

        public String GetMd5Hash(byte[] documentUploadedBytes)
        {
            return CommonFileManager.GetMd5Hash(documentUploadedBytes);
        }

        public String ConvertDocumentToPdfForPrint()
        {
            if (!View.lstSelectedDocumentIds.IsNullOrEmpty())
            {
                List<Int32> lstDocumentIds = View.lstSelectedDocumentIds.Where(cond => cond.Value == true).Select(sel => sel.Key).ToList();
                return DocumentManager.ConvertSharedDocumentToPDFForPrint(lstDocumentIds, View.SelectedTenantID);
            }
            return string.Empty;
        }

        /// <summary>
        /// This method is used to call the Parallel Task for Pdf conversion
        /// </summary>
        public void CallParallelTaskPdfConversion()
        {
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();
            //Use Poco class so that Entity will not get updated while running parallel tasks
            List<ApplicantDocumentPocoClass> lstDocumentDetails = new List<ApplicantDocumentPocoClass>();

            Int32 applicantOrgUserId = View.lstSharedUserInvitationDocument.FirstOrDefault().SUIDM_ApplicantOrgUserID.Value;

            if (View.lstSharedUserInvitationDocument.IsNotNull() && View.lstSharedUserInvitationDocument.Count() >= 0)
            {

                foreach (var doc in View.lstSharedUserInvitationDocument)
                {
                    ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                    appDoc.ApplicantDocumentID = doc.InvitationDocument.IND_ID;
                    appDoc.FileName = doc.InvitationDocument.IND_FileName;
                    appDoc.DocumentPath = doc.InvitationDocument.IND_DocumentFilePath;
                    appDoc.PdfDocPath = String.Empty;
                    appDoc.IsCompressed = false;
                    appDoc.Size = doc.InvitationDocument.IND_Size;
                    lstDocumentDetails.Add(appDoc);
                }
                conversionData.Add("UploadedDocuments", lstDocumentDetails);
            }
            else
            {
                conversionData.Add("UploadedDocuments", null);
            }
            conversionData.Add("OrganizationUserId", applicantOrgUserId);
            conversionData.Add("CurrentLoggedUserID", View.CurrentLoggedInUserId);
            conversionData.Add("TenantID", View.SelectedTenantID);

            Dictionary<String, Object> mergingData = null;

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            if (conversionData.IsNotNull() && conversionData.Count > 0)
            {
                //ParallelTaskContext.ParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
                Business.RepoManagers.DocumentManager.RunParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
            }
        }

        /// <summary>
        /// This method is used to merge converted pdf documents into unified pdf document
        /// </summary>
        /// <param name="mergingData">mergingData (Data dictionary that conatins the organizationUserId,tenantId,CurrentLoggedUserIDk)</param>
        private void MergeDocIntoUnifiedPdf(Dictionary<String, Object> mergingData)
        {
            if (mergingData.IsNotNull() && mergingData.Count > 0)
            {
            }
        }

        /// <summary>
        /// This method is used to convert the list of documents into pdf document
        /// </summary>
        /// <param name="conversionData">conversionData (Data dictionary that conatins the applicantdocument table object ,tenantId,currentLoggedUserID)</param>
        private void ConvertDocumentsIntoPdf(Dictionary<String, Object> conversionData)
        {
            if (conversionData.IsNotNull() && conversionData.Count > 0)
            {
                Int32 tenantId;
                Int32 CurrentLoggedUserID;
                List<ApplicantDocumentPocoClass> lstDocument = conversionData.GetValue("UploadedDocuments") as List<ApplicantDocumentPocoClass>;
                conversionData.TryGetValue("TenantID", out tenantId);
                conversionData.TryGetValue("CurrentLoggedUserID", out CurrentLoggedUserID);
                Business.RepoManagers.DocumentManager.ConvertSahredInvitationDocumentToPDF(lstDocument, tenantId, CurrentLoggedUserID);
            }
        }
        #endregion

        #region UAT-2943
        public void GetInvitationReviewStatus()
        {
            List<lkpSharedUserInvitationReviewStatu> lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().Where(cond => !cond.SUIRS_IsDeleted).ToList();
            String sharedUserInvDroppedReviewStatusCode = SharedUserInvitationReviewStatus.Dropped.GetStringValue(); //UAT-4460
            View.lstInvitationReviewStatus = lstSharedUserInvitationReviewStatus.Where(cond=>cond.SUIRS_Code!= sharedUserInvDroppedReviewStatusCode).Select(col => new SharedUserInvitationReviewStatusContract
            {
                Code = col.SUIRS_Code,
                Description = col.SUIRS_Description,
                IsDeleted = col.SUIRS_IsDeleted,
                Name = col.SUIRS_Name,
                ReviewStatusID = col.SUIRS_ID
            }).ToList();
        }
        #endregion

        #region UAT-3315
        public void GetCheckedBadgeDocumentsToExport(Int32 organizationUserID)
        {
            ServiceRequest<String, Int32, Int32> serviceRequest = new ServiceRequest<String, Int32, Int32>();
            serviceRequest.Parameter1 = Convert.ToString(organizationUserID);
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = View.SelectedTenantID;
            var _serviceResponse = _clinicalRotationProxy.GetSelectedBadgeDocumentsToExport(serviceRequest);
            View.LstBadgeDocumentContract = _serviceResponse.Result;
        }
        #endregion

        #region UAT-3338

        public void GetRequirementPackageSubscriptionForInstructorPreceptor()
        {
            View.RequirementPackageSubscription = ComplianceDataManager.GetRequirementPackageSubscriptionForInstructorPreceptor(View.SelectedTenantID, Convert.ToInt32(View.RotationID), View.SelectedOrganisationUserID);

        }

        public void GetProfileSharingGroupData()
        {
            ProfileSharingInvitationGroup profileSharingInvitationGroup = ProfileSharingManager.GetProfileSharingGroupData(View.AgencyID, Convert.ToInt32(View.RotationID));
            if (!profileSharingInvitationGroup.IsNullOrEmpty())
                View.ProfileSharingInvitationID = profileSharingInvitationGroup.ProfileSharingInvitations.Where(con => con.PSI_InviteeOrgUserID == View.CurrentLoggedInUserId).Select(sel => sel.PSI_ID).FirstOrDefault();
        }

        #endregion

        #region UAT-3316
        public String GetSharedUserTemplatePermissions(Int32 organizationUserID, Boolean isCompliancePermissions)
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = organizationUserID;
            serviceRequest.Parameter2 = isCompliancePermissions;
            var _serviceResponse = _clinicalRotationProxy.GetSharedUserTemplatePermissions(serviceRequest);
            return _serviceResponse.Result;
        }
        #endregion
    }
}
