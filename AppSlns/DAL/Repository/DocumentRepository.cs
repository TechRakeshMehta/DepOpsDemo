#region System Specific
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region Application Specific
using DAL.Interfaces;
using INTSOF.Utils;
using Entity.ClientEntity;
using DAL.PDFConversion;
using INTSOF.Utils.CommonPocoClasses;
using System.IO;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Data;

#endregion

namespace DAL.Repository
{
    public class DocumentRepository : ClientBaseRepository, IDocumentRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public DocumentRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #region Public Method

        /// <summary>
        /// Get bytes from the unified pdf document 
        /// </summary>
        /// <param name="unifiedDocumentId">unifiedDocumentId</param>
        /// <returns>UnifiedPdfDocument</returns>
        public UnifiedPdfDocument GetUnifiedPdfDocument(Int32 unifiedDocumentId)
        {
            UnifiedPdfDocument unifiedPdfDocument = null;

            if (unifiedDocumentId.IsNotNull())
            {
                unifiedPdfDocument = _ClientDBContext.UnifiedPdfDocuments.Where(obj => obj.UPD_ID == unifiedDocumentId && obj.UPD_IsDeleted == false && obj.UPD_IsActive == true).FirstOrDefault();
            }
            return unifiedPdfDocument;
        }

      //  /// <summary>
      //  /// Get pdf unified document based on OrganizationUserId
      //  /// </summary>
      //  /// <param name="organizationUserId">organizationUserId</param>
      //  /// <returns>UnifiedPdfDocument</returns>
      //  public bool UpdateUnifiedDocumentIsFormFlattening(Int32 organizationUserId)
      //  {
      //      if (organizationUserId.IsNotNull())
      //      {
      //         var unifiedPdfDocument = _ClientDBContext.UnifiedPdfDocuments.Where(obj => obj.UPD_OrganizationUserID == organizationUserId && obj.UPD_IsDeleted == false && obj.UPD_IsActive == true).FirstOrDefault();
      //          unifiedPdfDocument.UPD_IsPdfDocumnetFormFlattening = true;
      //          unifiedPdfDocument.UPD_ModifiedByID = organizationUserId;
      //          unifiedPdfDocument.UPD_ModifiedOn = DateTime.Now;
      //          if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
      //          {
      //              return true;
      //          }
      //      }
      //      return false;
      //  }


      //public  bool UpdateApplicantIsFormFlattening(Int32 CurrentLoggedUserId ,Int32 ApplicantDocumentId)
      //  {
      //      if (CurrentLoggedUserId.IsNotNull() && CurrentLoggedUserId >0 &&  ApplicantDocumentId.IsNotNull() && ApplicantDocumentId>0)
      //      {
      //          var ApplicantPdfDocument = _ClientDBContext.ApplicantDocuments.Where(obj=>obj.ApplicantDocumentID == ApplicantDocumentId && obj.IsDeleted == false).FirstOrDefault();
      //          if (ApplicantPdfDocument.IsNotNull())
      //          {
      //              ApplicantPdfDocument.IsPdfDocumnetFormFlattening = true;
      //              ApplicantPdfDocument.ModifiedByID = CurrentLoggedUserId;
      //              ApplicantPdfDocument.ModifiedOn = DateTime.Now;
      //              if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
      //              {
      //                  return true;
      //              }
      //          }
      //      }
      //      return false;

      //  }

        /// <summary>
        /// Get pdf unified document based on OrganizationUserId
        /// </summary>
        /// <param name="organizationUserId">organizationUserId</param>
        /// <returns>UnifiedPdfDocument</returns>
        public UnifiedPdfDocument GetPdfAsUnifiedDocument(Int32 organizationUserId)
        {
            UnifiedPdfDocument unifiedPdfDocument = null;

            if (organizationUserId.IsNotNull())
            {
                unifiedPdfDocument = _ClientDBContext.UnifiedPdfDocuments.Where(obj => obj.UPD_OrganizationUserID == organizationUserId && obj.UPD_IsDeleted == false && obj.UPD_IsActive == true).FirstOrDefault();
            }
            return unifiedPdfDocument;
        }

        /// <summary>
        /// Converts a list of applicant documents to pdf
        /// </summary>
        /// <param name="applicantDocuments">List of Applicant Documents to be converted</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="applicantFileLocation">Applicant File Location where documents are saved</param>
        /// <returns>True/False</returns>
        public Boolean ConvertApplicantDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId)
        {
            try
            {
                /*Converted ApplicantDocumentManagement to WCF Service*/
                PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
                //Get Converted Applicant Document List
                List<ApplicantDoc> convertedApplicantDocuments = pdfConversion.ConvertApplicantDocumentToPDF(ConvertApplicantDocumentToOtherPoco(applicantDocuments), tenantID);
                if (convertedApplicantDocuments.IsNotNull() && convertedApplicantDocuments.Count > 0)
                {
                    //Save Converted Applicant Document List
                    List<Int32> documentIDs = convertedApplicantDocuments.Select(condition => condition.ApplicantDocumentID).ToList();
                    List<ApplicantDocument> contextApplicantDocument = _ClientDBContext.ApplicantDocuments.Where(condition => documentIDs.Contains(condition.ApplicantDocumentID)).ToList();
                    foreach (ApplicantDocument appDoc in contextApplicantDocument)
                    {
                        ApplicantDoc conDoc = convertedApplicantDocuments.Where(condition => condition.ApplicantDocumentID == appDoc.ApplicantDocumentID).FirstOrDefault();

                        appDoc.IsPdfDocumnetFormFlattening = conDoc.IsPdfDocumnetFormFlattening;
                        appDoc.PdfDocPath = conDoc.PdfDocPath;
                        appDoc.PdfFileName = conDoc.PdfFileName;
                        appDoc.TotalPages = conDoc.TotalPages;
                        appDoc.ConversionNotes = conDoc.ConversionNotes;
                        appDoc.IsCompressed = conDoc.IsCompressed;
                        appDoc.ModifiedOn = DateTime.Now;
                        appDoc.ModifiedByID = currentLoggedInUseId;
                        if (conDoc.IsCompressed)
                            appDoc.Size = conDoc.Size;
                        appDoc.OriginalDocSize = conDoc.OriginalDocSize;
                        appDoc.OriginalDocMD5Hash = conDoc.OriginalDocMD5Hash;
                        //UAT-2628
                        if ((String.Compare(conDoc.ConversionNotes, "Converted Successfully!", true) != AppConsts.NONE))
                        {

                            appDoc.ConversionMergingRetryCount = appDoc.ConversionMergingRetryCount.IsNull() ? AppConsts.NONE + AppConsts.ONE
                                                                                                                         : appDoc.ConversionMergingRetryCount + AppConsts.ONE;

                        }
                    }
                    _ClientDBContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Converts a list of applicant documents to pdf
        /// </summary>
        /// <param name="applicantDocuments">List of Applicant Documents to be converted</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="applicantFileLocation">Applicant File Location where documents are saved</param>
        /// <returns>True/False</returns>
        public List<ApplicantDocumentPocoClass> ConvertAttestationDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId)
        {
            try
            {
                /*Converted ApplicantDocumentManagement to WCF Service*/
                PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
                //Get Converted Applicant Document List
                List<ApplicantDoc> convertedApplicantDocuments = pdfConversion.ConvertApplicantDocumentToPDF(ConvertApplicantDocumentToOtherPoco(applicantDocuments), tenantID);

                convertedApplicantDocuments.ForEach(d =>
                {
                    applicantDocuments.Where(cond => cond.ApplicantDocumentID == d.ApplicantDocumentID).FirstOrDefault().DocumentPath = d.PdfDocPath;
                });

                return applicantDocuments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Append all converted applicant documents to single unified document
        /// </summary>
        /// <param name="organizationUserID">Applicant's Organization UserID</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="currentLoggedInUserID">Current Logged In User ID</param>
        /// <param name="documentStatus">Lookup table containing Document Status</param>
        /// <returns>True/False</returns>
        public Boolean AppendApplicantDocument(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUserID, List<lkpDocumentStatu> documentStatus
                                               , List<Int32> documentIds = null, Boolean isNeedToFilterDocuments = false)
        {
            Int32 mergingInProgressId = documentStatus.Where(condition => condition.DMS_Code.Equals(DocumentStatus.MERGING_IN_PROGRESS.GetStringValue())).FirstOrDefault().DMS_ID;
            Int32 mergingFailedId = documentStatus.Where(condition => condition.DMS_Code.Equals(DocumentStatus.MERGING_FAILED.GetStringValue())).FirstOrDefault().DMS_ID;
            Int32 mergingCompletedId = documentStatus.Where(condition => condition.DMS_Code.Equals(DocumentStatus.MERGING_COMPLETED.GetStringValue())).FirstOrDefault().DMS_ID;
            Int32 mergingCompletedWithErrorsId = documentStatus.Where(condition => condition.DMS_Code.Equals(DocumentStatus.MERGING_COMPLETED_WITH_ERRORS.GetStringValue())).FirstOrDefault().DMS_ID;
            Int32 unifiedDocumentID = 0;
            Boolean isDocumentFlatten = false;
            String previousUnifiedPdfPath = String.Empty;
            Int32 sequenceOrder = 0;
            Int32 previousPages = 0;
            try
            {
                List<lkpDocumentType> docType = _ClientDBContext.lkpDocumentTypes.Where(condition => !condition.DMT_IsDeleted).ToList();
                Int32 disclaimerId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.DisclaimerDocument.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 disclosureId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.DisclosureDocument.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 edsDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.EDS_AuthorizationForm.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 dnrDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.Disclosure_n_Release.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 rcptDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.Reciept_Document.GetStringValue())).FirstOrDefault().DMT_ID; //UAT-1035
                //UAT-1738:
                Int32 screeningDocumentTypeID = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;

                //UAT-1316 WB:Clinical Rotation Details screen for student.
                Int32 reqUpldDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 reqViewDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 rotSyllabusDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.ROTATION_SYLLABUS.GetStringValue())).FirstOrDefault().DMT_ID;

                //Get Applicant documents list that need to be converted.
                List<ApplicantDocument> applicantDocumentsToConverted = _ClientDBContext.ApplicantDocuments.Where(condition => condition.OrganizationUserID == organizationUserID && String.IsNullOrEmpty(condition.PdfDocPath)
                    && condition.IsDeleted == false && (condition.DocumentType == null || (condition.DocumentType != disclaimerId
                    && condition.DocumentType != disclosureId && condition.DocumentType != edsDocTypeId && condition.DocumentType != dnrDocTypeId && condition.DocumentType != rcptDocTypeId
                    && condition.DocumentType != reqUpldDocTypeId && condition.DocumentType != reqViewDocTypeId && condition.DocumentType != rotSyllabusDocTypeId
                    && (condition.IsSearchableOnly == null || condition.IsSearchableOnly == false)//UAT-1560:WB: We should be able to add documents that need to be signed to the order process
                    && condition.DocumentType != screeningDocumentTypeID
                     ))).ToList();
                //Use Poco class so that Entity will not get updated while running parallel tasks
                List<ApplicantDocumentPocoClass> lstApplicantDocToBeConverted = new List<ApplicantDocumentPocoClass>();
                //UAT-2628:
                var filteredDocument = applicantDocumentsToConverted.Where(x => x.ConversionMergingRetryCount == null || x.ConversionMergingRetryCount < 3).ToList();
                applicantDocumentsToConverted = filteredDocument;
                //UAT-2628:TODO
                //if (isNeedToFilterDocuments)
                //{
                //    applicantDocumentsToConverted = applicantDocumentsToConverted.Where(x => documentIds.Contains(x.ApplicantDocumentID)).ToList();
                //}
                if (applicantDocumentsToConverted.IsNotNull() && applicantDocumentsToConverted.Count > 0)
                {
                    foreach (var doc in applicantDocumentsToConverted)
                    {
                        ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                        appDoc.ApplicantDocumentID = doc.ApplicantDocumentID;
                        appDoc.FileName = doc.FileName;
                        appDoc.DocumentPath = doc.DocumentPath;
                        appDoc.PdfDocPath = doc.PdfDocPath;
                        lstApplicantDocToBeConverted.Add(appDoc);
                    }
                    ConvertApplicantDocumentToPDF(lstApplicantDocToBeConverted, tenantID, currentLoggedInUserID);
                }
                String unifiedDocumentPath = String.Empty;
                /*Converted ApplicantDocumentManagement to WCF Service*/
                PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
                //Get Applicant Documents list that need to be merged.
                List<ApplicantDocumentToBeMerged> applicantDocuments = GetApplicantDocumentsToAppended(organizationUserID);
                ////UAT-2628:TODO
                //if (isNeedToFilterDocuments)
                //{
                //    applicantDocuments = applicantDocuments.Where(x => documentIds.Contains(x.ApplicantDocumentID)).ToList();
                //}

                if (applicantDocuments.IsNotNull() && applicantDocuments.Count > 0)
                {
                    //Get Previous Unified Document of Applicant
                    UnifiedPdfDocument previousUnifiedPdfDocument = _ClientDBContext.UnifiedPdfDocuments.Include("ApplicantDocumentMergings").Where(condition =>
                            condition.UPD_OrganizationUserID == organizationUserID && condition.UPD_IsActive == true && condition.UPD_IsDeleted == false
                            && (condition.UPD_DocumentStatusID != mergingInProgressId && condition.UPD_DocumentStatusID != mergingFailedId)).FirstOrDefault();
                    UnifiedPdfDocument unifiedPdfDocumentUpdated = new UnifiedPdfDocument();
                    if (previousUnifiedPdfDocument.IsNull())
                    {
                        //Save applicant unified document with status as In progress.
                        UnifiedPdfDocument unifiedPdfDocument = new UnifiedPdfDocument();
                        unifiedPdfDocument.UPD_OrganizationUserID = organizationUserID;
                        unifiedPdfDocument.UPD_DocumentStatusID = mergingInProgressId;
                        unifiedPdfDocument.UPD_IsDeleted = false;
                        unifiedPdfDocument.UPD_IsActive = false;
                        unifiedPdfDocument.UPD_CreatedByID = currentLoggedInUserID;
                        unifiedPdfDocument.UPD_CreatedOn = DateTime.Now;
                        _ClientDBContext.UnifiedPdfDocuments.AddObject(unifiedPdfDocument);
                        _ClientDBContext.SaveChanges();
                        unifiedDocumentID = unifiedPdfDocument.UPD_ID;
                        unifiedPdfDocumentUpdated = _ClientDBContext.UnifiedPdfDocuments.Where(condition => condition.UPD_ID == unifiedDocumentID).FirstOrDefault();
                        isDocumentFlatten = Convert.ToBoolean(unifiedPdfDocumentUpdated.UPD_IsPdfDocumnetFormFlattening);
                    }
                    else
                    {
                        unifiedDocumentID = previousUnifiedPdfDocument.UPD_ID;
                        isDocumentFlatten = Convert.ToBoolean(previousUnifiedPdfDocument.UPD_IsPdfDocumnetFormFlattening);
                        //Update unified document status as merging in progress before calling append unified document.
                        previousUnifiedPdfDocument.UPD_DocumentStatusID = mergingInProgressId;
                        _ClientDBContext.SaveChanges();

                        //Get Previous Unified document values
                        unifiedPdfDocumentUpdated = _ClientDBContext.UnifiedPdfDocuments.Where(condition => condition.UPD_ID == unifiedDocumentID).FirstOrDefault();
                        previousUnifiedPdfPath = unifiedPdfDocumentUpdated.UPD_PdfDocPath;

                        List<ApplicantDocumentMerging> appDocMerging = unifiedPdfDocumentUpdated.ApplicantDocumentMergings.
                                                                        Where(cond => cond.ADM_DocumentStatusID == mergingCompletedId).ToList();
                        if (appDocMerging.IsNotNull() && appDocMerging.Count > 0)
                        {
                            ApplicantDocumentMerging appDocMer = appDocMerging.OrderByDescending(x => x.ADM_ID).FirstOrDefault();
                            previousPages = appDocMer.ADM_EndPageNum.HasValue ? appDocMer.ADM_EndPageNum.Value : 0;
                            sequenceOrder = appDocMer.ADM_SequenceOrder.HasValue ? appDocMer.ADM_SequenceOrder.Value : 0;
                        }
                    }

                    applicantDocuments = applicantDocuments.DistinctBy(condition => condition.ApplicantDocumentID).ToList();

                    Int32 failedmerging = 0, successfulmerging = 0, totalmerging = 0;
                    Boolean IsFalltenFile = false;
                    //Get merged applicant document list and save the list.
                    List<ApplicantDocumentMerging> mergedApplicantDocuments = ConvertPocoToApplicantDocumentMerging(pdfConversion.AppendConvertedDocumentToPDF(unifiedDocumentID,
                           tenantID, ConvertApplicantDocumentToBeMergedToPoco(applicantDocuments), previousUnifiedPdfPath, sequenceOrder, previousPages, isDocumentFlatten), out unifiedDocumentPath, out IsFalltenFile);
                    foreach (ApplicantDocumentMerging mergedDocument in mergedApplicantDocuments)
                    {
                        ApplicantDocumentMerging appDocumentMerged = mergedDocument;
                        appDocumentMerged.ADM_CreatedByID = currentLoggedInUserID;
                        appDocumentMerged.ADM_CreatedOn = DateTime.Now;
                        if (mergedDocument.ADM_MergingNotes != "Merged Successfully!")
                        {
                            appDocumentMerged.ADM_DocumentStatusID = mergingFailedId;
                        }
                        else
                        {
                            appDocumentMerged.ADM_DocumentStatusID = mergingCompletedId;
                        }
                        _ClientDBContext.ApplicantDocumentMergings.AddObject(appDocumentMerged);
                    }
                    totalmerging = mergedApplicantDocuments.Count();
                    failedmerging = mergedApplicantDocuments.Where(condition => condition.ADM_DocumentStatusID != mergingCompletedId).Count();
                    successfulmerging = mergedApplicantDocuments.Where(condition => condition.ADM_DocumentStatusID == mergingCompletedId).Count();
                    List<Int32> applicantDocumentIDs = mergedApplicantDocuments.Select(x => x.ADM_ApplicantDocumentID.Value).ToList();
                    List<ApplicantDocumentMerging> lstAppDocMergedToDelete = _ClientDBContext.ApplicantDocumentMergings
                        .Where(x => x.ADM_UnifiedPdfDocumentID == unifiedDocumentID && applicantDocumentIDs.Contains(x.ADM_ApplicantDocumentID.Value)).ToList();
                    if (lstAppDocMergedToDelete.IsNotNull() && lstAppDocMergedToDelete.Count > 0)
                    {
                        foreach (ApplicantDocumentMerging appDocMer in lstAppDocMergedToDelete)
                        {
                            if (appDocMer.ADM_DocumentStatusID != mergingCompletedId)
                            {
                                appDocMer.ADM_IsDeleted = true;
                                appDocMer.ADM_ModifiedByID = currentLoggedInUserID;
                                appDocMer.ADM_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                    if (successfulmerging == totalmerging)
                    {
                        unifiedPdfDocumentUpdated.UPD_DocumentStatusID = mergingCompletedId;
                        unifiedPdfDocumentUpdated.UPD_IsPdfDocumnetFormFlattening = IsFalltenFile;
                        AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                        Boolean IsDeleted = objAmazonS3Documents.DeleteDocument(previousUnifiedPdfPath);
                    }
                    else if (failedmerging < totalmerging && successfulmerging > 0)
                    {
                        unifiedPdfDocumentUpdated.UPD_DocumentStatusID = mergingCompletedWithErrorsId;
                    }
                    else if (failedmerging == totalmerging && sequenceOrder == 0)
                    {
                        unifiedPdfDocumentUpdated.UPD_DocumentStatusID = mergingFailedId;
                    }
                    else
                    {
                        unifiedPdfDocumentUpdated.UPD_DocumentStatusID = mergingCompletedWithErrorsId;
                    }
                    //Save applicant unified document with status complete, failed or complete with errors based on merging status.                    
                    if (!String.IsNullOrEmpty(unifiedDocumentPath))
                    {
                        unifiedPdfDocumentUpdated.UPD_PdfDocPath = unifiedDocumentPath;
                        unifiedPdfDocumentUpdated.UPD_PdfFileName = "UnifiedDocument_" + organizationUserID.ToString() + "_" + unifiedDocumentID.ToString() + ".pdf";
                    }
                    unifiedPdfDocumentUpdated.UPD_IsActive = true;
                    unifiedPdfDocumentUpdated.UPD_ModifiedOn = DateTime.Now;
                    unifiedPdfDocumentUpdated.UPD_ModifiedByID = currentLoggedInUserID;
                    _ClientDBContext.SaveChanges();
                    //Get all active unified documents except new one created and set them to inactive.
                    List<UnifiedPdfDocument> unifiedDocumentInActive = _ClientDBContext.UnifiedPdfDocuments.Include("ApplicantDocumentMergings").Where(condition =>
                        condition.UPD_OrganizationUserID == organizationUserID && condition.UPD_ID != unifiedDocumentID && condition.UPD_IsActive == true
                        && condition.UPD_IsDeleted == false).ToList();
                    if (unifiedDocumentInActive.IsNotNull() && unifiedDocumentInActive.Count > 0)
                    {
                        unifiedDocumentInActive.ForEach(condition =>
                        {
                            //UAT-3714
                            AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                            Boolean IsDeleted = objAmazonS3Documents.DeleteDocument(previousUnifiedPdfPath);
                            //END UAT
                            condition.UPD_IsActive = false;
                            condition.UPD_IsDeleted = true;
                            condition.UPD_ModifiedOn = DateTime.Now;
                            condition.UPD_IsFileDeleted = IsDeleted; //UAT-3714
                            condition.UPD_ModifiedByID = currentLoggedInUserID;
                            condition.ApplicantDocumentMergings.ForEach(x =>
                            {
                                x.ADM_IsDeleted = true;
                                x.ADM_ModifiedOn = DateTime.Now;
                                x.ADM_ModifiedByID = currentLoggedInUserID;
                            });
                        });

                        _ClientDBContext.SaveChanges();
                        unifiedDocumentInActive.ForEach(condition =>
                        {
                            //UAT-3714
                            AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                            Boolean IsDeleted = objAmazonS3Documents.DeleteDocument(previousUnifiedPdfPath);
                            //END UAT
                        });
                    }
                }
                else
                {
                    //Get Applicant documents count.
                    Int32 applicantDocumentCount = _ClientDBContext.ApplicantDocuments.Where(condition => condition.OrganizationUserID == organizationUserID
                        && condition.IsDeleted == false).Count();
                    if (applicantDocumentCount == 0)
                    {
                        //Get all active unified documents if all applicant documents are deleted and set them to inactive.
                        List<UnifiedPdfDocument> unifiedDocumentInActive = _ClientDBContext.UnifiedPdfDocuments.Include("ApplicantDocumentMergings").Where(condition =>
                            condition.UPD_OrganizationUserID == organizationUserID && condition.UPD_IsActive == true && condition.UPD_IsDeleted == false).ToList();
                        if (unifiedDocumentInActive.IsNotNull() && unifiedDocumentInActive.Count > 0)
                        {
                            unifiedDocumentInActive.ForEach(condition =>
                            {
                                //UAT-3714
                                AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                                Boolean IsDeleted = objAmazonS3Documents.DeleteDocument(previousUnifiedPdfPath);
                                //END UAT
                                condition.UPD_IsActive = false;
                                condition.UPD_IsDeleted = true;
                                condition.UPD_IsFileDeleted = IsDeleted; //UAT-3714
                                condition.UPD_ModifiedOn = DateTime.Now;
                                condition.UPD_ModifiedByID = currentLoggedInUserID;
                                condition.ApplicantDocumentMergings.ForEach(x =>
                                {
                                    x.ADM_IsDeleted = true;
                                    x.ADM_ModifiedOn = DateTime.Now;
                                    x.ADM_ModifiedByID = currentLoggedInUserID;
                                });
                            });

                            _ClientDBContext.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                UnifiedPdfDocument unifiedPdfDocumentUpdated = _ClientDBContext.UnifiedPdfDocuments.Where(condition => condition.UPD_ID == unifiedDocumentID
                    && condition.UPD_IsDeleted == false && condition.UPD_DocumentStatusID == mergingInProgressId).FirstOrDefault();
                if (unifiedPdfDocumentUpdated.IsNotNull())
                {
                    //UAT-3714
                    AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                    Boolean IsDeleted = objAmazonS3Documents.DeleteDocument(previousUnifiedPdfPath);
                    //END UAT
                    unifiedPdfDocumentUpdated.UPD_ModifiedOn = DateTime.Now;
                    unifiedPdfDocumentUpdated.UPD_ModifiedByID = currentLoggedInUserID;
                    unifiedPdfDocumentUpdated.UPD_IsActive = false;
                    unifiedPdfDocumentUpdated.UPD_IsDeleted = true;
                    unifiedPdfDocumentUpdated.UPD_IsFileDeleted = IsDeleted; //UAT-3714
                    _ClientDBContext.SaveChanges();
                }
                throw ex;
            }
        }

        public List<ApplicantDoc> ConvertApplicantDocumentToPoco(List<ApplicantDocument> applicantDocuments)
        {
            List<ApplicantDoc> lstApplicantDoc = new List<ApplicantDoc>();
            foreach (var doc in applicantDocuments)
            {
                ApplicantDoc appDoc = new ApplicantDoc();
                appDoc.ApplicantDocumentID = doc.ApplicantDocumentID;
                appDoc.Code = doc.Code;
                appDoc.CreatedByID = doc.CreatedByID;
                appDoc.CreatedOn = doc.CreatedOn;
                appDoc.Description = doc.Description;
                appDoc.DocumentPath = doc.DocumentPath;
                appDoc.DocumentType = doc.DocumentType;
                appDoc.FileName = doc.FileName;
                appDoc.IsDeleted = doc.IsDeleted;
                appDoc.MIME = doc.MIME;
                appDoc.OrganizationUserID = doc.OrganizationUserID;
                appDoc.PdfFileName = doc.PdfFileName;
                appDoc.PdfDocPath = doc.PdfDocPath;
                appDoc.TotalPages = doc.TotalPages;
                appDoc.Size = doc.Size;
                appDoc.ConversionNotes = doc.ConversionNotes;
                appDoc.ModifiedOn = doc.ModifiedOn;
                appDoc.ModifiedByID = doc.ModifiedByID;
                lstApplicantDoc.Add(appDoc);
            }
            return lstApplicantDoc;
        }

        public List<ApplicantDoc> ConvertApplicantDocumentToOtherPoco(List<ApplicantDocumentPocoClass> applicantDocuments)
        {
            List<ApplicantDoc> lstApplicantDoc = new List<ApplicantDoc>();
            foreach (var doc in applicantDocuments)
            {
                ApplicantDoc appDoc = new ApplicantDoc();
                appDoc.ApplicantDocumentID = doc.ApplicantDocumentID;
                appDoc.DocumentPath = doc.DocumentPath;
                appDoc.FileName = doc.FileName;
                appDoc.PdfDocPath = doc.PdfDocPath;
                appDoc.IsCompressed = doc.IsCompressed;
                appDoc.Size = doc.Size;
                lstApplicantDoc.Add(appDoc);
            }
            return lstApplicantDoc;
        }

        public List<ApplicantDocToBeMerged> ConvertApplicantDocumentToBeMergedToPoco(List<ApplicantDocumentToBeMerged> applicantDocuments)
        {
            List<ApplicantDocToBeMerged> lstDocToBeMerged = new List<ApplicantDocToBeMerged>();
            if (applicantDocuments != null && applicantDocuments.Count > 0)
            {
                foreach (var doc in applicantDocuments)
                {
                    ApplicantDocToBeMerged appDocToBeMerged = new ApplicantDocToBeMerged
                    {
                        ApplicantDocumentID = doc.ApplicantDocumentID,
                        PdfDocPath = doc.PdfDocPath,
                        TotalPages = doc.TotalPages
                    };
                    lstDocToBeMerged.Add(appDocToBeMerged);
                }
            }
            return lstDocToBeMerged;
        }

        public List<ApplicantDocumentMerging> ConvertPocoToApplicantDocumentMerging(List<DAL.PDFConversion.ApplicantDocMerging> lstDocMerging, out String unifiedDocPath, out Boolean IsFileFlatten)
        {
            List<ApplicantDocumentMerging> lstDocumentMerging = new List<ApplicantDocumentMerging>();
            unifiedDocPath = "";
            IsFileFlatten = false;
            if (lstDocMerging != null && lstDocMerging.Count > 0)
            {
                //UAT-2628
                List<Int32> lstApplicantDocumentIds = lstDocMerging.Where(cond => cond.ADM_ApplicantDocumentID.HasValue).Select(slct => slct.ADM_ApplicantDocumentID.Value).ToList();
                var listApplicantDocument = _ClientDBContext.ApplicantDocuments.Where(x => lstApplicantDocumentIds.Contains(x.ApplicantDocumentID) && !x.IsDeleted).ToList();
                unifiedDocPath = lstDocMerging[0].UnifiedDocumentPath;
                IsFileFlatten = lstDocMerging[0].IsFlattenFile;
                foreach (var doc in lstDocMerging)
                {
                    ApplicantDocumentMerging appDocumentMerging = new ApplicantDocumentMerging();
                    appDocumentMerging.ADM_ID = doc.ADM_ID;
                    appDocumentMerging.ADM_ApplicantDocumentID = doc.ADM_ApplicantDocumentID;
                    appDocumentMerging.ADM_UnifiedPdfDocumentID = doc.ADM_UnifiedPdfDocumentID;
                    appDocumentMerging.ADM_SequenceOrder = doc.ADM_SequenceOrder;
                    appDocumentMerging.ADM_MergingNotes = doc.ADM_MergingNotes;
                    appDocumentMerging.ADM_StartPageNum = doc.ADM_StartPageNum;
                    appDocumentMerging.ADM_TotalPages = doc.ADM_TotalPages;
                    appDocumentMerging.ADM_EndPageNum = doc.ADM_EndPageNum;
                    appDocumentMerging.ADM_CreatedByID = doc.ADM_CreatedByID;
                    appDocumentMerging.ADM_CreatedOn = doc.ADM_CreatedOn;
                    appDocumentMerging.ADM_DocumentStatusID = doc.ADM_DocumentStatusID;
                    appDocumentMerging.ADM_IsDeleted = doc.ADM_IsDeleted;
                    appDocumentMerging.ADM_ModifiedByID = doc.ADM_ModifiedByID;
                    appDocumentMerging.ADM_ModifiedOn = doc.ADM_ModifiedOn;
                    lstDocumentMerging.Add(appDocumentMerging);

                    //UAT-2628
                    if (!listApplicantDocument.IsNullOrEmpty() && (String.Compare(doc.ADM_MergingNotes, "Merged Successfully!", true) != AppConsts.NONE))
                    {
                        var documentData = listApplicantDocument.Where(x => x.ApplicantDocumentID == doc.ADM_ApplicantDocumentID).FirstOrDefault();
                        if (!documentData.IsNullOrEmpty())
                        {
                            documentData.ConversionMergingRetryCount = documentData.ConversionMergingRetryCount.IsNull() ? AppConsts.NONE + AppConsts.ONE
                                                                                                                         : documentData.ConversionMergingRetryCount + AppConsts.ONE;
                        }
                    }
                }
            }
            return lstDocumentMerging;
        }

        /// <summary>
        /// Get applicant documents to be merged.
        /// </summary>
        /// <param name="organizationUserId">Applicant's Organization UserID</param>
        /// <returns>List of ApplicantDocument</returns>
        public List<ApplicantDocumentToBeMerged> GetApplicantDocumentsToMerged(Int32 organizationUserId)
        {
            return _ClientDBContext.GetApplicantDocumentToBeMerged(organizationUserId).ToList();
        }

        /// <summary>
        /// Get applicant documents to be appended.
        /// </summary>
        /// <param name="organizationUserId">Applicant's Organization UserID</param>
        /// <returns>List of ApplicantDocument</returns>
        public List<ApplicantDocumentToBeMerged> GetApplicantDocumentsToAppended(Int32 organizationUserId)
        {
            return _ClientDBContext.GetApplicantDocumentToBeAppended(organizationUserId).ToList();
        }

        /// <summary>
        /// This method check the unified document merging status.
        /// </summary>
        /// <param name="organizationUserId">organizationUserId</param>
        /// <param name="documentStatusId">documentStatusId</param>
        /// <returns></returns>
        public Boolean IsMergingInProgress(Int32 organizationUserId, Int32 documentStatusId)
        {
            return _ClientDBContext.UnifiedPdfDocuments.Any(condition => condition.UPD_OrganizationUserID == organizationUserId && condition.UPD_DocumentStatusID == documentStatusId && !condition.UPD_IsDeleted);
        }

        /// <summary>
        /// Get the unified pdf document with status merging in progress
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="documentStatusId"></param>
        /// <returns></returns>
        public UnifiedPdfDocument GetAllMergingInProgressUnifiedDoc(Int32 organizationUserId, Int32 documentStatusId)
        {
            UnifiedPdfDocument unifiedPdfDocument = null;

            if (organizationUserId.IsNotNull())
            {
                unifiedPdfDocument = _ClientDBContext.UnifiedPdfDocuments.Where(condition => condition.UPD_OrganizationUserID == organizationUserId && condition.UPD_DocumentStatusID == documentStatusId && !condition.UPD_IsDeleted).FirstOrDefault();
            }
            return unifiedPdfDocument;
        }
        #endregion

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        public List<ApplicantDocToBeMerged> ConvertDocumentToBePrintToPoco(List<ApplicantDocument> applicantDocuments)
        {
            List<ApplicantDocToBeMerged> lstDocToBePrint = new List<ApplicantDocToBeMerged>();
            if (applicantDocuments != null && applicantDocuments.Count > 0)
            {
                foreach (var doc in applicantDocuments)
                {
                    ApplicantDocToBeMerged appDocToBePrint = new ApplicantDocToBeMerged();
                    if (doc.PdfDocPath.IsNotNull())
                    {
                        appDocToBePrint.ApplicantDocumentID = doc.ApplicantDocumentID;
                        appDocToBePrint.PdfDocPath = doc.PdfDocPath;
                        lstDocToBePrint.Add(appDocToBePrint);

                    }
                    else if (Path.GetExtension(doc.DocumentPath).Equals(".pdf"))
                    {
                        appDocToBePrint.ApplicantDocumentID = doc.ApplicantDocumentID;
                        appDocToBePrint.PdfDocPath = doc.DocumentPath;
                        lstDocToBePrint.Add(appDocToBePrint);
                    }
                }
            }
            return lstDocToBePrint;
        }

        /// <summary>
        /// Append applicant documents to single unified document for print
        /// </summary>
        /// <param name="documentStatus">Lookup table containing Document Status</param>
        /// <param name="appDocumentIdsToPrint">Applicant document ids to print</param>
        /// <param name="tenantID">Institution ID</param>
        /// <returns>String unified document path</returns>
        public String ConvertDocumentToPDFForPrint(List<lkpDocumentStatu> documentStatus, List<Int32> appDocumentIdsToPrint, Int32 tenantId)
        {
            String returnPDFPath = String.Empty;
            try
            {
                //Get Applicant documents list that need to be converted.
                List<ApplicantDocument> applicantDocumentsToPrint = _ClientDBContext.ApplicantDocuments.Where(condition =>
                                                                         appDocumentIdsToPrint.Contains(condition.ApplicantDocumentID) && !condition.IsDeleted).ToList();
                /*Converted ApplicantDocumentManagement to WCF Service*/
                PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
                returnPDFPath = pdfConversion.ConvertDocumentToPDFForPrint(tenantId, ConvertDocumentToBePrintToPoco(applicantDocumentsToPrint));

                return returnPDFPath;
            }
            catch (Exception ex)
            {
                return returnPDFPath;
            }
        }
        #endregion

        #region UAT-1564:delete a document from Unified also when deleting it from manage Document page

        /// <summary>
        /// Delete document from single unified pdf
        /// </summary>
        /// <param name="organizationUserID">Applicant's Organization UserID</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="currentLoggedInUserID">Current Logged In User ID</param>
        /// <param name="appDocIdToDelete">Applicant document id to delete</param>
        /// <param name="documentStatus">Lookup table containing Document Status</param>
        /// <returns>True/False</returns>
        public Boolean DeleteApplicantDocFromUnifiedDoc(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUserID, Int32 appDocIdToDelete, List<lkpDocumentStatu> documentStatus, Boolean clearContext)
        {
            Int32 mergingInProgressId = documentStatus.Where(condition => condition.DMS_Code.Equals(DocumentStatus.MERGING_IN_PROGRESS.GetStringValue())).FirstOrDefault().DMS_ID;
            Int32 mergingFailedId = documentStatus.Where(condition => condition.DMS_Code.Equals(DocumentStatus.MERGING_FAILED.GetStringValue())).FirstOrDefault().DMS_ID;
            Int32 mergingCompletedId = documentStatus.Where(condition => condition.DMS_Code.Equals(DocumentStatus.MERGING_COMPLETED.GetStringValue())).FirstOrDefault().DMS_ID;
            Int32 unifiedDocumentID = 0;
            String previousUnifiedPdfPath = String.Empty;
            Int32 sequenceOrderToDelete = 0;
            Int32 pagesToAdd = 0;
            Int32 newStartPageNumber = 0;
            Int32 totalDeletedPages = 0;
            try
            {
                //Get Applicant document that need to be Delete.
                ApplicantDocument applicantDocumentToDelete = _ClientDBContext.ApplicantDocuments.FirstOrDefault(condition => condition.OrganizationUserID == organizationUserID
                    && condition.IsDeleted == true && condition.ApplicantDocumentID == appDocIdToDelete);

                String unifiedDocumentPath = String.Empty;
                /*Converted ApplicantDocumentManagement to WCF Service*/
                PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();

                //Get Previous Unified Document of Applicant
                UnifiedPdfDocument previousUnifiedPdfDocument = new UnifiedPdfDocument();
                previousUnifiedPdfDocument = _ClientDBContext.UnifiedPdfDocuments.Include("ApplicantDocumentMergings").Where(condition =>
                        condition.UPD_OrganizationUserID == organizationUserID && condition.UPD_IsActive == true && condition.UPD_IsDeleted == false
                        && (condition.UPD_DocumentStatusID != mergingInProgressId && condition.UPD_DocumentStatusID != mergingFailedId)).FirstOrDefault();
                UnifiedPdfDocument unifiedPdfDocumentUpdated = new UnifiedPdfDocument();
                if (!previousUnifiedPdfDocument.IsNullOrEmpty())
                {
                    //Check is applicant document that is need to be delete from unified document exist in applicant document merging. 
                    ApplicantDocumentMerging appdocMergingToDelete = previousUnifiedPdfDocument.ApplicantDocumentMergings.FirstOrDefault(cond => cond.ADM_ApplicantDocumentID == appDocIdToDelete
                                                       && cond.ADM_IsDeleted == false && cond.ADM_DocumentStatusID == mergingCompletedId);
                    if (!appdocMergingToDelete.IsNullOrEmpty())
                    {
                        unifiedDocumentID = previousUnifiedPdfDocument.UPD_ID;
                        //Update unified document status as merging in progress before calling Delete applicant document from unified document.
                        previousUnifiedPdfDocument.UPD_DocumentStatusID = mergingInProgressId;
                        _ClientDBContext.SaveChanges();

                        //Get Previous Unified document values
                        unifiedPdfDocumentUpdated = _ClientDBContext.UnifiedPdfDocuments.Where(condition => condition.UPD_ID == unifiedDocumentID).FirstOrDefault();
                        previousUnifiedPdfPath = unifiedPdfDocumentUpdated.UPD_PdfDocPath;

                        //Get Sequence order to delete 
                        sequenceOrderToDelete = appdocMergingToDelete.ADM_SequenceOrder.HasValue ? appdocMergingToDelete.ADM_SequenceOrder.Value : 0;
                        totalDeletedPages = appdocMergingToDelete.ADM_TotalPages.HasValue ? appdocMergingToDelete.ADM_TotalPages.Value : 0;

                        List<ApplicantDocumentMerging> appDocMergingList = unifiedPdfDocumentUpdated.ApplicantDocumentMergings.
                                                                        Where(cond => cond.ADM_DocumentStatusID == mergingCompletedId && cond.ADM_IsDeleted == false)
                                                                        .OrderBy(ord => ord.ADM_SequenceOrder).ToList();
                        List<ApplicantDocumentMerging> appDocMergingPreviousList = new List<ApplicantDocumentMerging>();
                        if (!appDocMergingList.IsNullOrEmpty() && appDocMergingList.Count > 0)
                        {
                            appDocMergingPreviousList = appDocMergingList.Where(x => x.ADM_SequenceOrder < appdocMergingToDelete.ADM_SequenceOrder)
                                                                                                      .ToList();//.Where(x => x.ADM_ID < appdocMergingToDelete.ADM_ID).ToList();
                        }
                        ApplicantDocumentMerging appDocMergingNew = null;
                        //if (appDocMergingList.Any(x => x.ADM_ID > appdocMergingToDelete.ADM_ID))
                        if (appDocMergingList.Any(x => x.ADM_SequenceOrder > appdocMergingToDelete.ADM_SequenceOrder))
                        {
                            appDocMergingNew = appDocMergingList.First(x => x.ADM_SequenceOrder > appdocMergingToDelete.ADM_SequenceOrder);
                        }

                        List<Int32> appDocMergingIdListToUpdate = appDocMergingList.Select(x => x.ADM_ID).ToList();

                        pagesToAdd = (appDocMergingPreviousList.IsNullOrEmpty() || appDocMergingPreviousList.Count == 0) ? 0 : appDocMergingPreviousList.Sum(x => x.ADM_TotalPages.Value);

                        newStartPageNumber = appDocMergingNew.IsNullOrEmpty() ? 0 : appDocMergingNew.ADM_StartPageNum.HasValue ? appDocMergingNew.ADM_StartPageNum.Value : 0;

                        //Check is all applicant documents are deleted from Applicant document merging.If 'Yes' then delete the unified document as well. 
                        Boolean isNeedToDeleteUnifiedDocument = !appDocMergingList.Any(cnd => cnd.ADM_ID != appdocMergingToDelete.ADM_ID);

                        //Get updated unified document path after deletion of applicant document.
                        unifiedDocumentPath = pdfConversion.DeleteAppDocumentFromUnifiedPDF(tenantID, pagesToAdd, newStartPageNumber, previousUnifiedPdfPath, unifiedDocumentID
                                                                                            , isNeedToDeleteUnifiedDocument);
                        if (!unifiedDocumentPath.IsNullOrEmpty())
                        {
                            List<ApplicantDocumentMerging> lstAppDocMergedToUpdate = _ClientDBContext.ApplicantDocumentMergings
                                .Where(x => x.ADM_UnifiedPdfDocumentID == unifiedDocumentID && appDocMergingIdListToUpdate.Contains(x.ADM_ID)
                                       && x.ADM_SequenceOrder > sequenceOrderToDelete).ToList();

                            foreach (ApplicantDocumentMerging appDocMergedToUpdate in lstAppDocMergedToUpdate)
                            {
                                appDocMergedToUpdate.ADM_SequenceOrder = appDocMergedToUpdate.ADM_SequenceOrder - 1;
                                appDocMergedToUpdate.ADM_StartPageNum = appDocMergedToUpdate.ADM_StartPageNum - totalDeletedPages;
                                appDocMergedToUpdate.ADM_EndPageNum = appDocMergedToUpdate.ADM_EndPageNum - totalDeletedPages;
                                appDocMergedToUpdate.ADM_ModifiedByID = currentLoggedInUserID;
                                appDocMergedToUpdate.ADM_ModifiedOn = DateTime.Now;
                            }

                            //Delete applicant document from applicant document merging
                            appdocMergingToDelete.ADM_IsDeleted = true;
                            appdocMergingToDelete.ADM_ModifiedByID = currentLoggedInUserID;
                            appdocMergingToDelete.ADM_ModifiedOn = DateTime.Now;
                        }

                        if (isNeedToDeleteUnifiedDocument && !unifiedDocumentPath.IsNullOrEmpty())
                        {
                            //UAT-3714
                            AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                            Boolean IsDeleted = objAmazonS3Documents.DeleteDocument(previousUnifiedPdfPath);
                            //END UAT
                            unifiedPdfDocumentUpdated.UPD_IsActive = false;
                            unifiedPdfDocumentUpdated.UPD_IsDeleted = true;
                            unifiedPdfDocumentUpdated.UPD_IsFileDeleted = IsDeleted; //UAT-3714
                            unifiedPdfDocumentUpdated.UPD_DocumentStatusID = mergingCompletedId;
                            unifiedPdfDocumentUpdated.UPD_ModifiedOn = DateTime.Now;
                            unifiedPdfDocumentUpdated.UPD_ModifiedByID = currentLoggedInUserID;
                            _ClientDBContext.SaveChanges();
                        }
                        else
                        {
                            //Save applicant unified document with status complete, failed or complete with errors based on merging status.                    
                            if (!String.IsNullOrEmpty(unifiedDocumentPath))
                            {
                                unifiedPdfDocumentUpdated.UPD_PdfDocPath = unifiedDocumentPath;
                                unifiedPdfDocumentUpdated.UPD_PdfFileName = "UnifiedDocument_" + organizationUserID.ToString() + "_" + unifiedDocumentID.ToString() + ".pdf";
                            }
                            unifiedPdfDocumentUpdated.UPD_IsActive = true;
                            unifiedPdfDocumentUpdated.UPD_DocumentStatusID = mergingCompletedId;
                            unifiedPdfDocumentUpdated.UPD_ModifiedOn = DateTime.Now;
                            unifiedPdfDocumentUpdated.UPD_ModifiedByID = currentLoggedInUserID;
                            _ClientDBContext.SaveChanges();

                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                UnifiedPdfDocument unifiedPdfDocumentUpdated = new UnifiedPdfDocument();
                unifiedPdfDocumentUpdated = _ClientDBContext.UnifiedPdfDocuments.Where(condition => condition.UPD_ID == unifiedDocumentID).FirstOrDefault();
                if (!unifiedPdfDocumentUpdated.IsNullOrEmpty())
                {
                    unifiedPdfDocumentUpdated.UPD_DocumentStatusID = mergingCompletedId;
                    unifiedPdfDocumentUpdated.UPD_ModifiedOn = DateTime.Now;
                    unifiedPdfDocumentUpdated.UPD_ModifiedByID = currentLoggedInUserID;
                    _ClientDBContext.SaveChanges();
                }
                return false;
            }
        }
        #endregion

        #region UAT-2443:Attestation Merge and multiple share behavior changes
        public String MergeAttestationDocuments(Int32 tenantID, String attestationDocPathToMerge, String previousAttestationPdfPath, Int32 currentUserId, String fileNameToAppend, String fileName, Boolean deletePreviousAttestation)
        {
            /*Converted ApplicantDocumentManagement to WCF Service*/
            PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
            return pdfConversion.MergeAttestationDocuments(tenantID, attestationDocPathToMerge, previousAttestationPdfPath, currentUserId, fileNameToAppend, fileName, deletePreviousAttestation);
        }
        #endregion

        #region UAT-2628:System should check (scheduled) and fix document conversion and merging issues automatically. Send notification to a group after certain retry count

        public List<ApplicantDocumentPocoClass> GetDocumentsDetailsForMergingAndConversing(Int32 tenantID, Int32 currentUserId, Int32 chunkSize, Int32 retryCount, String entityName
                                                                                           , Int32 subEventId)
        {
            List<ApplicantDocumentPocoClass> documentListForMergingAndConversing = new List<ApplicantDocumentPocoClass>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("dbo.usp_GetFailedAppDocumentToAutoConversionAndMerging", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentUserId);
                command.Parameters.AddWithValue("@TenantId", tenantID);
                command.Parameters.AddWithValue("@RetryCount", retryCount);
                command.Parameters.AddWithValue("@EntityName", entityName);
                command.Parameters.AddWithValue("@SubEventId", subEventId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE)
                {
                    documentListForMergingAndConversing = ds.Tables[0].AsEnumerable().Select(col =>
                          new ApplicantDocumentPocoClass
                          {
                              ApplicantDocumentID = col["ApplicantDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(col["ApplicantDocumentID"]),
                              OrganizationUserId = col["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrganizationUserID"]),
                              ApplicantFirstName = col["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantFirstName"]),
                              ApplicantLastName = col["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantLastName"]),
                              FileName = col["DocumentName"] == DBNull.Value ? String.Empty : Convert.ToString(col["DocumentName"]),
                              DocumentPath = col["DocumentPath"] == DBNull.Value ? String.Empty : Convert.ToString(col["DocumentPath"]),
                              DocumentRetryCount = col["RetryCount"] == DBNull.Value ? 0 : Convert.ToInt32(col["RetryCount"]),
                              PrimaryEmailAddress = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                          }).ToList();
                }

            }
            return documentListForMergingAndConversing;
        }

        /// <summary>
        /// Append all converted applicant documents to single unified document
        /// </summary>
        public List<ApplicantDocument> AppendApplicantDocumentAutomatically(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUserID, List<lkpDocumentStatu> documentStatus
                                                            , List<ApplicantDocumentPocoClass> documentList, Int32 documentRetryCount)
        {
            List<Int32> applicantDocumentIdsToMerge = !documentList.IsNullOrEmpty() ? documentList.Where(cnd => cnd.DocumentRetryCount < documentRetryCount)
                                                                                                  .Select(x => x.ApplicantDocumentID).ToList()
                                                                                      : new List<Int32>();

            List<Int32> allDocumentIds = !documentList.IsNullOrEmpty() ? documentList.Select(x => x.ApplicantDocumentID).ToList() : new List<Int32>();

            AppendApplicantDocument(organizationUserID, tenantID, currentLoggedInUserID, documentStatus, applicantDocumentIdsToMerge, true);

            return _ClientDBContext.ApplicantDocuments.Where(cond => allDocumentIds.Contains(cond.ApplicantDocumentID)
                                                                    && (cond.ConversionMergingRetryCount == documentRetryCount
                                                                        || cond.ConversionMergingRetryCount > documentRetryCount)
                                                            ).ToList();
        }
        #endregion

        #region UAT-2774
        Boolean IDocumentRepository.ConvertSahredInvitationDocumentToPDF(List<ApplicantDocumentPocoClass> lstDocuments, Int32 tenantID, Int32 currentLoggedInUseId)
        {
            /*Converted ApplicantDocumentManagement to WCF Service*/
            PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();

            //Get Converted Applicant Document List
            List<ApplicantDoc> convertedApplicantDocuments = pdfConversion.ConvertApplicantDocumentToPDF(ConvertApplicantDocumentToOtherPoco(lstDocuments), tenantID);

            if (convertedApplicantDocuments.IsNotNull() && convertedApplicantDocuments.Count > 0)
            {
                //Save Converted Applicant Document List
                List<Int32> documentIDs = convertedApplicantDocuments.Select(condition => condition.ApplicantDocumentID).ToList();
                List<Entity.SharedDataEntity.InvitationDocument> contextApplicantDocument = base.SharedDataDBContext.InvitationDocuments.Where(condition => documentIDs.Contains(condition.IND_ID) && !condition.IND_IsDeleted).ToList();
                foreach (Entity.SharedDataEntity.InvitationDocument appDoc in contextApplicantDocument)
                {
                    ApplicantDoc conDoc = convertedApplicantDocuments.Where(condition => condition.ApplicantDocumentID == appDoc.IND_ID).FirstOrDefault();

                    appDoc.IND_ModifiedOn = DateTime.Now;
                    appDoc.IND_ModifiedByID = currentLoggedInUseId;
                    if (conDoc.IsCompressed)
                        appDoc.IND_Size = conDoc.Size;
                    appDoc.IND_DocMD5Hash = conDoc.OriginalDocMD5Hash;
                    if (!conDoc.PdfDocPath.IsNullOrEmpty())
                    {
                        appDoc.IND_DocumentFilePath = conDoc.PdfDocPath;
                        appDoc.IND_FileName = Path.GetFileNameWithoutExtension(appDoc.IND_FileName) + ".pdf";
                    }
                    appDoc.IND_ConversionNotes = conDoc.ConversionNotes;
                    if (String.Compare(conDoc.ConversionNotes, "Converted Successfully!", true) == AppConsts.NONE)
                    {
                        appDoc.IND_IsDocConverted = true;
                    }
                    else
                    {
                        appDoc.IND_IsDocConverted = false;
                    }
                }
                base.SharedDataDBContext.SaveChanges();
            }
            return true;
        }

        String IDocumentRepository.ConvertSharedDocumentToPDFForPrint(List<Int32> DocumentIdsToPrint, Int32 tenantId)
        {
            String returnPDFPath = String.Empty;
            try
            {
                //Get Applicant documents list that need to be converted.
                List<Entity.SharedDataEntity.InvitationDocument> invitationDocumentsToPrint = base.SharedDataDBContext.InvitationDocuments.Where(cond => DocumentIdsToPrint.Contains(cond.IND_ID) && !cond.IND_IsDeleted).ToList();

                /*Converted ApplicantDocumentManagement to WCF Service*/
                PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
                returnPDFPath = pdfConversion.ConvertDocumentToPDFForPrint(tenantId, ConvertSharedDocumentToBePrintToPoco(invitationDocumentsToPrint));

                return returnPDFPath;
            }
            catch (Exception ex)
            {
                return returnPDFPath;
            }
        }

        private List<ApplicantDocToBeMerged> ConvertSharedDocumentToBePrintToPoco(List<Entity.SharedDataEntity.InvitationDocument> invitationDocuments)
        {
            List<ApplicantDocToBeMerged> lstDocToBePrint = new List<ApplicantDocToBeMerged>();
            if (invitationDocuments != null && invitationDocuments.Count > 0)
            {
                foreach (Entity.SharedDataEntity.InvitationDocument doc in invitationDocuments)
                {
                    ApplicantDocToBeMerged appDocToBePrint = new ApplicantDocToBeMerged();
                    Boolean isPDFDoc = Path.GetExtension(doc.IND_FileName).Equals(".pdf") ? true : false;
                    if (isPDFDoc)
                    {
                        appDocToBePrint.ApplicantDocumentID = doc.IND_ID;
                        appDocToBePrint.PdfDocPath = doc.IND_DocumentFilePath;
                        lstDocToBePrint.Add(appDocToBePrint);
                    }
                    else if (Path.GetExtension(doc.IND_DocumentFilePath).Equals(".pdf"))
                    {
                        appDocToBePrint.ApplicantDocumentID = doc.IND_ID;
                        appDocToBePrint.PdfDocPath = doc.IND_DocumentFilePath;
                        lstDocToBePrint.Add(appDocToBePrint);
                    }
                }
            }
            return lstDocToBePrint;
        }
        #endregion

        #region UAT-3238
        public List<ApplicantDocumentPocoClass> GetConvertApplicantDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId)
        {
            try
            {
                List<ApplicantDocumentPocoClass> result = new List<ApplicantDocumentPocoClass>();
                /*Converted ApplicantDocumentManagement to WCF Service*/
                PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
                //Get Converted Applicant Document List
                List<ApplicantDoc> convertedApplicantDocuments = pdfConversion.ConvertApplicantDocumentToPDF(ConvertApplicantDocumentToOtherPoco(applicantDocuments), tenantID);
                convertedApplicantDocuments.ForEach(s => result.Add(new ApplicantDocumentPocoClass()
                {
                    ApplicantDocumentID = s.ApplicantDocumentID,
                    PdfDocPath = s.PdfDocPath,
                    TotalPages = s.TotalPages
                }));
                if (convertedApplicantDocuments.IsNotNull() && convertedApplicantDocuments.Count > 0)
                {
                    //Save Converted Applicant Document List
                    List<Int32> documentIDs = convertedApplicantDocuments.Select(condition => condition.ApplicantDocumentID).ToList();
                    List<ApplicantDocument> contextApplicantDocument = _ClientDBContext.ApplicantDocuments.Where(condition => documentIDs.Contains(condition.ApplicantDocumentID)).ToList();
                    foreach (ApplicantDocument appDoc in contextApplicantDocument)
                    {
                        ApplicantDoc conDoc = convertedApplicantDocuments.Where(condition => condition.ApplicantDocumentID == appDoc.ApplicantDocumentID).FirstOrDefault();

                        appDoc.PdfDocPath = conDoc.PdfDocPath;
                        appDoc.PdfFileName = conDoc.PdfFileName;
                        appDoc.TotalPages = conDoc.TotalPages;
                        appDoc.ConversionNotes = conDoc.ConversionNotes;
                        appDoc.IsCompressed = conDoc.IsCompressed;
                        appDoc.ModifiedOn = DateTime.Now;
                        appDoc.ModifiedByID = currentLoggedInUseId;
                        if (conDoc.IsCompressed)
                            appDoc.Size = conDoc.Size;
                        appDoc.OriginalDocSize = conDoc.OriginalDocSize;
                        appDoc.OriginalDocMD5Hash = conDoc.OriginalDocMD5Hash;
                        //UAT-2628
                        if ((String.Compare(conDoc.ConversionNotes, "Converted Successfully!", true) != AppConsts.NONE))
                        {

                            appDoc.ConversionMergingRetryCount = appDoc.ConversionMergingRetryCount.IsNull() ? AppConsts.NONE + AppConsts.ONE
                                                                                                                         : appDoc.ConversionMergingRetryCount + AppConsts.ONE;

                        }
                    }
                    _ClientDBContext.SaveChanges();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicantDocumentToBeMerged> GetApplicantDocumentList(List<ApplicantDocumentToBeMerged> applicantDocumentToBeMergedList)
        {
            /*Converted ApplicantDocumentManagement to WCF Service*/
            PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
            var output = pdfConversion.GetPdfPageDetails(ConvertApplicantDocumentToBeMergedToPoco(applicantDocumentToBeMergedList));
            List<ApplicantDocumentToBeMerged> res = new List<ApplicantDocumentToBeMerged>();
            output.ForEach(s => res.Add(new ApplicantDocumentToBeMerged()
            {
                ApplicantDocumentID = s.ApplicantDocumentID,
                PdfDocPath = s.PdfDocPath,
                TotalPages = s.TotalPages
            }));
            return res;
        }


        public Tuple<String, List<ApplicantDocumentToBeMerged>> CreateUnifiedDocumentForAgencyUser(List<ApplicantDocumentToBeMerged> applicantDocuments, Int32 tenantId)
        {
            try
            {
                /*Converted ApplicantDocumentManagement to WCF Service*/
                PDFConversion.PDFConversionClient pdfConversion = new PDFConversionClient();
                var res = pdfConversion.AppendConvertedUnifiedDocumentToPDF(ConvertApplicantDocumentToBeMergedToPoco(applicantDocuments));
                applicantDocuments = new List<ApplicantDocumentToBeMerged>();
                res.m_Item2.ForEach(s => applicantDocuments.Add(new ApplicantDocumentToBeMerged()
                {
                    ApplicantDocumentID = s.ApplicantDocumentID,
                    PdfDocPath = s.PdfDocPath,
                    TotalPages = s.TotalPages
                }));
                return new Tuple<String, List<ApplicantDocumentToBeMerged>>(res.m_Item1, applicantDocuments);
            }
            catch (Exception ex)
            {
                return new Tuple<string, List<ApplicantDocumentToBeMerged>>(String.Empty, new List<ApplicantDocumentToBeMerged>());
            }
        }
        #endregion
    }
}
