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
    public class SharedUserRotationInvitationDocumentPresenter : Presenter<ISharedUserRotationInvitationDocumentView>
    {
        #region Methods

        #region Public Methods

        public override void OnViewInitialized()
        {

        }

        public void GetSharedUserInvitationDocumentDetails()
        {
            View.lstSharedUserRotationInvitationDocumentContract = ProfileSharingManager.GetSharedUserRotationInvitationDocumentDetails(View.SelectedTenantID, View.ClinicalRotationID, View.AgencyID);
        }
        public void GetProfileSharingGroupIDByClinicalRotationID()
        {
            View.ProfileSharingInvitationGroupID = ProfileSharingManager.GetProfileSharingGroupIDByClinicalRotationID(View.SelectedTenantID, View.ClinicalRotationID);
        }

        public Int32 GetlkpSharedDocMappingType()
        {
            return ProfileSharingManager.GetDocumentTypeIdByCode(LKPSharedSystemDocumentTypes.SHARED_USER_INVITATION_DOCUMENT.GetStringValue());
        }
        public Boolean SaveSharedUserRotationInvitationDocumentDetails()
        {
            return ProfileSharingManager.SaveSharedUserRotationInvitationDocumentDetails(View.lstSharedUserRotationInvitationDocument);
        }
        public Boolean DeletedSharedUserRotationInvitationDocument(Int32 InvitationDocumentID)
        {
            return ProfileSharingManager.DeletedSharedUserRotationInvitationDocument(InvitationDocumentID,View.SelectedTenantID,View.ClinicalRotationID,View.AgencyID, View.CurrentLoggedInUserId);
        }
        public String IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, byte[] documentUploadedBytes)
        {
            // Int32 ApplicantOrgUserID = View.ProfileSharingInvitation.PSI_ApplicantOrgUserID;
            Int32 ProfileSharingInvitationGroupID = View.ProfileSharingInvitationGroupID;

            if (ProfileSharingManager.IsRotationInvitationDocumentAlreadyUploaded(documentName, documentSize,View.AgencyID,View.ClinicalRotationID,View.ProfileSharingInvitationGroupID))
                return documentName;

            //List<InvitationDocument> lstInvitationDocuments = ProfileSharingManager.GetInvitationDocumentsForInvitationApplicant(ProfileSharingInvitationGroupID, ApplicantOrgUserID);
            List<SharedUserInvitationDocumentContract> lstSharedUserInvitationDocument = View.lstSharedUserRotationInvitationDocumentContract;
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

            Int32 applicantOrgUserId = AppConsts.NONE;

            if (View.lstSharedUserRotationInvitationDocument.IsNotNull() && View.lstSharedUserRotationInvitationDocument.Count() >= 0)
            {

                foreach (var doc in View.lstSharedUserRotationInvitationDocument)
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
        #endregion

        #region Private Methods

        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
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

        #endregion
    }
}
