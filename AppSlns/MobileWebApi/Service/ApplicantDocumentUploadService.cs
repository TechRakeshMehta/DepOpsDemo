using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.Utils.CommonPocoClasses;
using MobileWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace MobileWebApi.Service
{
    public static class ApplicantDocumentUploadService
    {
        public static Tuple<bool,Int32> SaveApplicantDocument(Int32 tenantID, Int32 organizationUserID, List<ApplicantDocumentUploadContract> applicantDocumentUploadContractList)
        {
            Int32 duplicateDocumentCount = 0;
            String filePath = String.Empty;
            Boolean aWSUseS3 = false;
            Boolean isCorruptedFileUploaded = false;
            StringBuilder corruptedFileMessage = new StringBuilder();
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
            StringBuilder docMessage = new StringBuilder();
            List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();

            if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
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
                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }

                filePath += "Tenant(" + tenantID.ToString() + @")\";

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            foreach (var file in applicantDocumentUploadContractList)
            {
                ApplicantDocument applicantDocument = new ApplicantDocument();
                String fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.Document.FileName);
                //Save file
                String newTempFilePath = Path.Combine(tempFilePath, fileName);
                file.Document.SaveAs(newTempFilePath);

                byte[] fileBytes = File.ReadAllBytes(newTempFilePath);
                var documentName = IsDocumentAlreadyUploaded(file.Document.FileName, file.Document.ContentLength, fileBytes, organizationUserID, tenantID);

                if (!documentName.IsNullOrEmpty())
                {
                    docMessage.Append("You have already updated " + file.Document.FileName + " document as " + documentName + ". \\n");
                    duplicateDocumentCount++;
                    continue;
                }

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    //Move file to other location
                    String destFilePath = Path.Combine(filePath, fileName);
                    File.Copy(newTempFilePath, destFilePath);
                    applicantDocument.DocumentPath = destFilePath;
                }
                else
                {
                    if (!filePath.EndsWith("//"))
                    {
                        filePath += "//";
                    }
                    //AWS code to save document to S3 location
                    AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                    String destFolder = filePath + "Tenant(" + tenantID.ToString() + @")/";
                    String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                    if (returnFilePath.IsNullOrEmpty())
                    {
                        isCorruptedFileUploaded = true;
                        corruptedFileMessage.Append("Your file " + file.Document.FileName + " is not uploaded. \\n");
                        continue;
                    }
                    applicantDocument.DocumentPath = returnFilePath;
                }
                try
                {
                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);
                }
                catch (Exception) { }
                applicantDocument.OrganizationUserID = organizationUserID;
                applicantDocument.FileName = file.Document.FileName;
                applicantDocument.Size = file.Document.ContentLength;
                applicantDocument.DocumentType = null;
                applicantDocument.Description = file.DocumentDescription;
                applicantDocument.CreatedByID = organizationUserID;
                applicantDocument.CreatedOn = DateTime.Now;
                applicantDocument.IsDeleted = false;
                applicantDocument.DataEntryDocumentStatusID = null;
                String newFilePath = String.Empty;
                if (aWSUseS3 == false)
                {
                    newFilePath = filePath;
                }
                else
                {

                    if (!filePath.EndsWith("//"))
                    {
                        filePath += "//";
                    }

                    newFilePath = filePath + "Tenant(" + tenantID.ToString() + @")/";
                }
                Int32 applicantDocumentId = ComplianceDataManager.AddApplicantDocument(applicantDocument, tenantID);
                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String UpdatedfileName = newFilePath + "UD_" + tenantID.ToString() + "_" + applicantDocumentId + "_" + date + Path.GetExtension(applicantDocument.FileName);
                ComplianceDataManager.UpdateDocumentPath(UpdatedfileName, applicantDocument.DocumentPath, applicantDocumentId, tenantID, organizationUserID);

                /*Entries into PocoClass*/
                ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                appDoc.ApplicantDocumentID = applicantDocument.ApplicantDocumentID;
                appDoc.FileName = applicantDocument.FileName;
                appDoc.DocumentPath = UpdatedfileName;
                appDoc.PdfDocPath = String.Empty;
                appDoc.IsCompressed = false;
                appDoc.Size = applicantDocument.Size;
                lstApplicantDoc.Add(appDoc);
            }

            //Convert images into PDf
            if (lstApplicantDoc.IsNotNull() && lstApplicantDoc.Count() >= 0)
            {
                conversionData.Add("ApplicantUploadedDocuments", lstApplicantDoc);
                conversionData.Add("OrganizationUserId", organizationUserID);
                conversionData.Add("CurrentLoggedUserID", organizationUserID);
                conversionData.Add("TenantID", tenantID);
                ConvertDocumentsIntoPdf(conversionData);
            }

            if ((docMessage.Length > 0 && !(docMessage.ToString().IsNullOrEmpty())) || (corruptedFileMessage.Length > 0 && !(corruptedFileMessage.ToString().IsNullOrEmpty())))
            {
                if (isCorruptedFileUploaded)
                {
                    return  new Tuple<bool, int>(false,duplicateDocumentCount);
                }
            }
            return new Tuple<bool, int>(true, duplicateDocumentCount); ;
        }



        public static String IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, byte[] documentUploadedBytes, Int32 CurrentLoggedUserID, Int32 tenantID)
        {
            if (ComplianceDataManager.IsDocumentAlreadyUploaded(documentName, documentSize, CurrentLoggedUserID, tenantID, true))
            {
                return documentName;
            }
            //Compare original document MD5Hash
            List<ApplicantDocument> applicantDocuments = ComplianceDataManager.GetApplicantDocuments(CurrentLoggedUserID, tenantID);
            String md5Hash = CommonFileManager.GetMd5Hash(documentUploadedBytes);

            if (applicantDocuments.IsNotNull())
            {
                Int32 personalDocType = LookupManager.GetLookUpData<lkpDocumentType>(tenantID).Where(cond => cond.DMT_Code == DocumentType.PERSONAL_DOCUMENT.GetStringValue()).FirstOrDefault().DMT_ID;
                /*
                    1) once a document is uplaoded into personal documents, it can not be uploaded again to personal documents.
                    2) once a document is uploaded in personal documents, it can still be uploaded in compliance documents
                    3) once a document is uploaded in personal documents, it can still be uploaded in rotation documents.
                    4) Once a document is uploaded into rotation documents, it can still be uploaded into personal dcouments.
                    4) Once a document is uploaded into compliance documents, it can still be uploaded into personal dcouments.
                */
                var applicantDocument = applicantDocuments.FirstOrDefault(x => x.OriginalDocMD5Hash == md5Hash && x.DocumentType.IsNotNull() && x.DocumentType == personalDocType);
                if (applicantDocument.IsNotNull())
                {
                    return applicantDocument.FileName;
                }
            }
            return null;
        }

        /// <summary>
        /// This method is used to convert the list of documents into pdf document
        /// </summary>
        /// <param name="conversionData">conversionData (Data dictionary that conatins the applicantdocument table object ,tenantId,currentLoggedUserID)</param>
        private static void ConvertDocumentsIntoPdf(Dictionary<String, Object> conversionData)
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
    }
}