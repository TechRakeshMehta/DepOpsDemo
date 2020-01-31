using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using INTSOF.Utils.CommonPocoClasses;


namespace CoreWeb.ComplianceAdministration.Views
{
    public class ViewDocumentPopupPresenter : Presenter<IViewDocumentView>
    {
        /// <summary>
        /// Get the organization user detail.
        /// </summary>
        public void GetOrganizationUserDetails()
        {
            View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(View.OrganizationUserID);
            View.Addresses = StoredProcedureManagers.GetAddressByAddressHandleId(View.OrganizationUserData.AddressHandleID.Value, View.OrganizationUserData.Organization.TenantID.Value);
        }

        public void GetDocumentData()
        {
            ViewDocumentDetailsContract viewDocumentDetailsContract = new ViewDocumentDetailsContract();
            ClientSystemDocument clientSystemDocument = new ClientSystemDocument();
            ApplicantDocument applicantDocument = new ApplicantDocument();

            if (View.CurrentViewContext.ClientSysDocId > 0 && View.CurrentViewContext.TenantId > 0)
            {
                clientSystemDocument = ComplianceDataManager.GetClientSystemDocument(View.ClientSysDocId, View.TenantId);
                viewDocumentDetailsContract.IsApplicantDoc = false;
                viewDocumentDetailsContract.DocumentPath = clientSystemDocument.CSD_DocumentPath;
                viewDocumentDetailsContract.DocumentName = clientSystemDocument.CSD_FileName;
                if (clientSystemDocument.DocumentFieldMappings.IsNullOrEmpty())
                {
                    viewDocumentDetailsContract.IsSignatureRequired = false;
                }
                else
                {
                    Int32 signatureTypeID = LookupManager.GetLookUpData<lkpDocumentFieldType_>(View.TenantId).FirstOrDefault(cond => cond.DFT_Code == DocumentFieldTypes.Signature.GetStringValue()).DFT_ID;
                    viewDocumentDetailsContract.IsSignatureRequired = clientSystemDocument.DocumentFieldMappings
                        .Any(x => (x.DFM_DocumentFieldTypeID == signatureTypeID && !x.DFM_IsDeleted)) ? true : false;
                    View.lstDocumentFieldMapping = clientSystemDocument.DocumentFieldMappings.ToList();
                }

            }
            else if (View.CurrentViewContext.ApplicantDocId > 0 && View.CurrentViewContext.TenantId > 0)
            {
                //Get data from app document table
                applicantDocument = ComplianceDataManager.GetApplicantDocument(View.ApplicantDocId, View.TenantId);
                viewDocumentDetailsContract.IsApplicantDoc = true;
                viewDocumentDetailsContract.DocumentPath = applicantDocument.DocumentPath;
                viewDocumentDetailsContract.DocumentName = applicantDocument.FileName;
            }
            View.ViewDocContract = viewDocumentDetailsContract;
        }

        public String GetApplicantSSN()
        {
            return SecurityManager.GetFormattedString(View.OrganizationUserData.OrganizationUserID, false);
        }

        /// <summary>
        /// Method that set the data entry documnet status id for new status.  
        /// </summary>
        public void GetDataEntryDocStatus()
        {
            if (View.TenantId > AppConsts.NONE)
            {
                String newDataEntryCompleteStatus = DataEntryDocumentStatus.COMPLETE.GetStringValue();
                lkpDataEntryDocumentStatu tempDataEntryDocStatusComplete = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDataEntryDocumentStatu>(View.TenantId)
                                                                               .FirstOrDefault(cnd => cnd.LDEDS_Code == newDataEntryCompleteStatus && cnd.LDEDS_IsDeleted == false);
                if (!tempDataEntryDocStatusComplete.IsNullOrEmpty())
                    View.DataEntryDocCompleteStatusId = tempDataEntryDocStatusComplete.LDEDS_ID;
            }
        }

        /// <summary>
        /// This method is used to call the Parallel Task for Pdf conversion and merging 
        /// </summary>
        public void CallParallelTaskPdfConversionMerging()
        {
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();
            //Use Poco class so that Entity will not get updated while running parallel tasks
            List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();

            if (View.ToSaveApplicantUploadedDocuments.IsNotNull() && View.ToSaveApplicantUploadedDocuments.Count() >= 0)
            {
                foreach (var doc in View.ToSaveApplicantUploadedDocuments)
                {
                    ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                    appDoc.ApplicantDocumentID = doc.ApplicantDocumentID;
                    appDoc.FileName = doc.FileName;
                    appDoc.DocumentPath = doc.DocumentPath;
                    appDoc.PdfDocPath = doc.PdfDocPath;
                    appDoc.IsCompressed = doc.IsCompressed;
                    appDoc.Size = doc.Size;
                    lstApplicantDoc.Add(appDoc);
                }
                //conversionData.Add("ApplicantUploadedDocuments", View.ToSaveApplicantUploadedDocuments);
                conversionData.Add("ApplicantUploadedDocuments", lstApplicantDoc);
            }
            else
            {
                conversionData.Add("ApplicantUploadedDocuments", null);
            }
            conversionData.Add("OrganizationUserId", View.OrganizationUserID);
            conversionData.Add("CurrentLoggedUserID", View.OrgUsrID); //View.CurrentLoggedInUserId UAT 1261
            conversionData.Add("TenantID", View.TenantId);

            Dictionary<String, Object> mergingData = new Dictionary<String, Object>();
            mergingData.Add("OrganizationUserId", View.OrganizationUserID);
            mergingData.Add("CurrentLoggedUserID", View.OrgUsrID); //View.CurrentLoggedInUserId UAT 1261
            mergingData.Add("TenantID", View.TenantId);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            if (conversionData.IsNotNull() && conversionData.Count > 0 && mergingData.IsNotNull() && mergingData.Count > 0)
            {
                //ParallelTaskContext.ParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
                Business.RepoManagers.DocumentManager.RunParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
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
                List<ApplicantDocumentPocoClass> applicantDocument = conversionData.GetValue("ApplicantUploadedDocuments") as List<ApplicantDocumentPocoClass>;
                conversionData.TryGetValue("TenantID", out tenantId);
                conversionData.TryGetValue("CurrentLoggedUserID", out CurrentLoggedUserID);
                Business.RepoManagers.DocumentManager.ConvertApplicantDocumentToPDF(applicantDocument, tenantId, CurrentLoggedUserID);
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
                Boolean isParallelTaskRunning = true;
                Int32 tenantId;
                Double totalTimeDiffernce = 0.0;
                Int32 CurrentLoggedUserID;
                Int32 OrganizationUserId;
                mergingData.TryGetValue("OrganizationUserId", out OrganizationUserId);
                mergingData.TryGetValue("TenantID", out tenantId);
                mergingData.TryGetValue("CurrentLoggedUserID", out CurrentLoggedUserID);

                List<lkpDocumentStatu> lkpDocumentStatus = Business.RepoManagers.DocumentManager.GetDocumentStatus(tenantId);
                //Get the document status id based on the Merging in progress statusc code
                Int32 documentStatusId = lkpDocumentStatus.Where(obj => obj.DMS_Code == DocumentStatus.MERGING_IN_PROGRESS.GetStringValue()).FirstOrDefault().DMS_ID;

                //Check status of the document for user who uploaded the document.Method return true if document status is Merging in progress  else false 
                isParallelTaskRunning = Business.RepoManagers.DocumentManager.IsMergingInProgress(OrganizationUserId, tenantId, documentStatusId);
                UnifiedPdfDocument unifiedPdfDocument = Business.RepoManagers.DocumentManager.GetAllMergingInProgressUnifiedDoc(OrganizationUserId, tenantId, documentStatusId);

                if (isParallelTaskRunning)
                {
                    for (int i = 0; i <= AppConsts.TEN; i++)
                    {
                        if (isParallelTaskRunning)
                        {
                            if (unifiedPdfDocument.IsNotNull())
                            {
                                totalTimeDiffernce = (DateTime.Now - unifiedPdfDocument.UPD_CreatedOn).TotalMinutes;
                            }

                            if (totalTimeDiffernce >= 10.0)
                            {
                                Business.RepoManagers.DocumentManager.AppendApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
                                break;
                            }
                            else if (i == AppConsts.TEN)
                            {
                                View.ErrorMessage = "Could not start fresh merging as pervious merging is still in progress for tenantId: " + tenantId + "," + " appplicantId: " + OrganizationUserId + "." + " Request is an abandoned.";
                            }
                            else
                            {
                                //Wait for 3 seconds if document status is Merging in Progress
                                System.Threading.Thread.Sleep(3000);
                                isParallelTaskRunning = Business.RepoManagers.DocumentManager.IsMergingInProgress(OrganizationUserId, tenantId, documentStatusId);
                            }
                        }
                        else
                        {
                            Business.RepoManagers.DocumentManager.AppendApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
                            break;
                        }
                    }
                }
                else
                {
                    Business.RepoManagers.DocumentManager.AppendApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);

                }

            }
        }

        public Int32 AddApplicantViewDocuments(String filePath)
        {
            Int32 id = 0;
            foreach (var applicantDocument in View.ToSaveApplicantUploadedDocuments)
            {
                id = ComplianceDataManager.SaveApplicantDocument(applicantDocument, View.TenantId);
                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String newFileName = filePath + "UD_" + View.TenantId.ToString() + "_" + id.ToString() + "_" + date + Path.GetExtension(applicantDocument.FileName);
                ComplianceDataManager.UpdateDocumentPath(newFileName, applicantDocument.DocumentPath, id, View.TenantId, applicantDocument.OrganizationUserID.Value);
            }
            return id;
        }
    }
}
