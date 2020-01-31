#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DocumentManager.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

using INTSOF.Utils;
using System.Collections.Generic;
using Entity.ClientEntity;
using System.IO;
using System.Configuration;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.Utils.CommonPocoClasses;
using INTSOF.Contracts;
using System.Linq;
using System.Web;
using System.Reflection;
#endregion

#endregion

namespace Business.RepoManagers
{
    /// <summary>
    /// class Document Manager
    /// </summary>
    public class DocumentManager
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Method retuen the byte array from the file 
        /// </summary>       
        /// <param name="filePath">filePath</param>
        /// <returns>byte array</returns>
        public static byte[] GetBytesFromUnifiedPdf(String filePath)

        {
            BALUtils.LogDebug("Class Name: "+ MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " +System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method");
            try
            {
                //Check whether use AWS S3, true if need to use
                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                if (aWSUseS3 == false)
                {
                    return GetBytes(filePath);
                }
                else
                {
                    //Get AWS document
                    AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                    return objAmazonS3Documents.RetrieveDocument(filePath);
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
            }
            return null;
        }

        /// <summary>
        /// Method retuen the byte array of UnifiedPdfDocument
        /// </summary>
        /// <param name="unifiedDocumentId">unifiedDocumentId</param>
        /// <param name="tenantID">tenantID</param>
        /// <returns>byte array</returns>
        public static byte[] GetBytesFromPdf(Int32 unifiedDocumentId, Int32 tenantID)
        {
            try
            {
                UnifiedPdfDocument unifiedPdfDocument = BALUtils.GetDocumentRepositoryRepoInstance(tenantID).GetUnifiedPdfDocument(unifiedDocumentId);
                if (unifiedPdfDocument != null)
                {
                    return GetBytes(unifiedPdfDocument.UPD_PdfDocPath);
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
            }
            return null;
        }

        /// <summary>
        /// Get bytes from the file
        /// </summary>
        /// <param name="filePath">filePath</param>
        /// <returns>byte array</returns>
        private static byte[] GetBytes(String filePath)
        {
            byte[] fileBytes = null;
            if (!String.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    String fileExtension = Path.GetExtension(filePath);
                    if (fileExtension.ToLower() == ".pdf")
                    {
                        fileBytes = File.ReadAllBytes(filePath);
                    }
                }
            }
            return fileBytes;
        }

        //public static bool UpdateApplicantIsFormFlattening(Int32 tenantID, Int32 CurrentLoggedUserId, Int32 ApplicantDocumentId)
        //{
        //    try
        //    {
        //        BALUtils.LogDebug("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method");

        //        return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).UpdateApplicantIsFormFlattening(CurrentLoggedUserId, ApplicantDocumentId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    return false;

        //}


        //public static bool UpdateUnifiedDocumentIsFormFlattening(Int32 tenantID, Int32 organizationUserId)
        //{
        //    try
        //    {
        //        BALUtils.LogDebug("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method");

        //        return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).UpdateUnifiedDocumentIsFormFlattening(organizationUserId);
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    }
        //    return false;

        //}

        /// <summary>
        /// Get unified document based on the Organization user Id 
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="organizationUserId">organizationUserId</param>
        /// <returns>UnifiedPdfDocument</returns>
        public static UnifiedPdfDocument GetPdfAsUnifiedDocument(Int32 tenantID, Int32 organizationUserId)
        {
            try
            {
                BALUtils.LogDebug("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method");

                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).GetPdfAsUnifiedDocument(organizationUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// Converts a list of applicant documents to pdf
        /// </summary>
        /// <param name="applicantDocuments">List of Applicants Documents to be converted</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="currentLoggedInUseId"> Current Logged In UserID</param>
        /// <returns>True/False</returns>
        public static Boolean ConvertApplicantDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId)
        {
            try
            {
                BALUtils.LogDebug("Class Name: " + MethodBase.GetCurrentMethod().DeclaringType.Name + ":-  Function Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :- Call Method");
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).ConvertApplicantDocumentToPDF(applicantDocuments, tenantID, currentLoggedInUseId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ApplicantDocumentPocoClass> ConvertAttestationDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId)
        {
            try
            {
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).ConvertAttestationDocumentToPDF(applicantDocuments, tenantID, currentLoggedInUseId);
            }
            catch (SysXException ex)
            {
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
        /// Append all converted applicant documents to single unified document
        /// </summary>
        /// <param name="organizationUserID">Applicant's Organization UserID</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="currentLoggedInUserID">Current Logged In User ID</param>
        /// <param name="documentStatus">Lookup table containing Document Status</param>
        /// <returns>True/False</returns>
        public static Boolean AppendApplicantDocument(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUseID)
        {
            try
            {
                List<lkpDocumentStatu> documentStatus = GetDocumentStatus(tenantID);
                if (BALUtils.GetDocumentRepositoryRepoInstance(tenantID).AppendApplicantDocument(organizationUserID, tenantID, currentLoggedInUseID, documentStatus))
                {
                    BALUtils.GetSecurityRepoInstance().SynchronizeApplicantDocument(organizationUserID, tenantID, currentLoggedInUseID);
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
        /// Get lkpDocumentStatus from Cache if enable otherwise from datbase.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns>List of lkpDocumentStatus</returns>
        public static List<lkpDocumentStatu> GetDocumentStatus(Int32 tenantID)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpDocumentStatu>(tenantID);
            }
            catch (SysXException ex)
            {
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
        /// This method check the merging status of unified document
        /// </summary>
        /// <param name="organizationUserID">organizationUserID</param>
        /// <param name="tenantID">tenantID</param>
        /// <param name="documentStatus">documentStatus</param>
        /// <returns></returns>
        public static Boolean IsMergingInProgress(Int32 organizationUserID, Int32 tenantID, Int32 documentStatus)
        {
            try
            {
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).IsMergingInProgress(organizationUserID, documentStatus);
            }
            catch (SysXException ex)
            {
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
        /// This method check the status (Merging in progress) of merged document before calling the parallel task utility.
        /// </summary>
        /// <param name="ConvertToPDF">ConvertToPDF</param>
        /// <param name="convertToPDFParm">convertToPDFParm</param>
        /// <param name="MergePDF">MergePDF</param>
        /// <param name="mergePDFParm">mergePDFParm</param>
        /// <param name="loggerService">loggerService</param>
        /// <param name="exceptionService">exceptionService</param>
        public static void RunParallelTaskPdfConversionMerging(ParallelTaskContext.ParallelTask ConvertToPDF, Dictionary<String, Object> convertToPDFParm, ParallelTaskContext.ParallelTask MergePDF, Dictionary<String, Object> mergePDFParm, ISysXLoggerService loggerService, ISysXExceptionService exceptionService)
        {
            try
            {
                ParallelTaskContext.ParallelTaskPdfConversionMerging(ConvertToPDF, convertToPDFParm, MergePDF, mergePDFParm, loggerService, exceptionService);
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
        /// 
        /// </summary>
        /// <param name="organizationUserID"></param>
        /// <param name="tenantID"></param>
        /// <param name="documentStatusId"></param>
        /// <returns></returns>
        public static UnifiedPdfDocument GetAllMergingInProgressUnifiedDoc(Int32 organizationUserID, Int32 tenantID, Int32 documentStatusId)
        {
            try
            {
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).GetAllMergingInProgressUnifiedDoc(organizationUserID, documentStatusId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }
        #endregion

        #endregion

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        /// <summary>
        /// Append all converted applicant documents to single unified document
        /// </summary>
        /// <param name="organizationUserID">Applicant's Organization UserID</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="currentLoggedInUserID">Current Logged In User ID</param>
        /// <param name="documentStatus">Lookup table containing Document Status</param>
        /// <returns>True/False</returns>
        public static String ConvertDocumentToPDFForPrint(Int32 tenantID, List<Int32> appDocumentIdsToPrint)
        {
            try
            {
                List<lkpDocumentStatu> documentStatus = GetDocumentStatus(tenantID);
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).ConvertDocumentToPDFForPrint(documentStatus, appDocumentIdsToPrint, tenantID);
            }
            catch (SysXException ex)
            {
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
        public static Boolean DeleteApplicantDocFromUnifiedDoc(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUserID, Int32 appDocIdToDelete, Boolean clearContext)
        {
            try
            {
                List<lkpDocumentStatu> documentStatus = GetDocumentStatus(tenantID);
                //Remove current context from parallel task DB context to get the fresh data from database if this method calling in two or more parallel task back to back.
                //if (clearContext)
                //{
                //   ADB_LibertyUniversity_ReviewEntities.ClearParallelTaskDBContext();
                //}
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).DeleteApplicantDocFromUnifiedDoc(organizationUserID, tenantID, currentLoggedInUserID, appDocIdToDelete, documentStatus, clearContext);
            }
            catch (SysXException ex)
            {
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
        /// This method check the status (Merging in progress) of merged document before calling the parallel task utility.
        /// </summary>
        /// <param name="ConvertToPDF">ConvertToPDF</param>
        /// <param name="convertToPDFParm">convertToPDFParm</param>
        /// <param name="MergePDF">MergePDF</param>
        /// <param name="mergePDFParm">mergePDFParm</param>
        /// <param name="loggerService">loggerService</param>
        /// <param name="exceptionService">exceptionService</param>
        public static void RunParallelTaskPdfDocumentDeletion(ParallelTaskContext.ParallelTask DeleteApplicantDocFromUnifiedDoc, Dictionary<String, Object> deletionData, ISysXLoggerService loggerService, ISysXExceptionService exceptionService)
        {
            try
            {
                ParallelTaskContext.PerformParallelTask(DeleteApplicantDocFromUnifiedDoc, deletionData, loggerService, exceptionService);
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
        #endregion

        #region UAT-1560:
        /// <summary>
        /// This method is used to call the Parallel Task for Pdf conversion and merging 
        /// </summary>
        public static void CallParallelTaskPdfConversionMergingForAppDoc(List<ApplicantDocument> additionalDocuments, Int32 tenantId, Int32 orgUserId, Int32 loggedInUserId)
        {
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();
            //Use Poco class so that Entity will not get updated while running parallel tasks
            List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();
            foreach (var doc in additionalDocuments)
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

            conversionData.Add("ApplicantUploadedDocuments", lstApplicantDoc);

            Dictionary<String, Object> mergingData = new Dictionary<String, Object>();

            conversionData.Add("OrganizationUserId", orgUserId);
            conversionData.Add("CurrentLoggedUserID", loggedInUserId);
            conversionData.Add("TenantID", tenantId);

            mergingData.Add("OrganizationUserId", orgUserId);
            mergingData.Add("CurrentLoggedUserID", loggedInUserId);
            mergingData.Add("TenantID", tenantId);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            if (conversionData.IsNotNull() && conversionData.Count > 0 && mergingData.IsNotNull() && mergingData.Count > 0)
            {
                Business.RepoManagers.DocumentManager.RunParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
            }
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

        /// <summary>
        /// This method is used to merge converted pdf documents into unified pdf document
        /// </summary>
        /// <param name="mergingData">mergingData (Data dictionary that conatins the organizationUserId,tenantId,CurrentLoggedUserIDk)</param>
        private static void MergeDocIntoUnifiedPdf(Dictionary<String, Object> mergingData)
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

                List<Entity.ClientEntity.lkpDocumentStatu> lkpDocumentStatus = DocumentManager.GetDocumentStatus(tenantId);
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
                    // Business.RepoManagers.DocumentManager.MergeApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
                    Business.RepoManagers.DocumentManager.AppendApplicantDocument(OrganizationUserId, tenantId, CurrentLoggedUserID);
                }
            }
        }
        #endregion
        #region UAT-2443:Attestation Merge and multiple share behavior changes
        public static String MergeAttestationDocuments(Int32 tenantID, String attestationDocPathToMerge, String previousAttestationPdfPath, Int32 currentUserId,
                                                       String fileNameToAppend, AttestationReportType reportType, Boolean deletePreviousAttestation)
        {
            try
            {
                String fileName = String.Empty;
                if (reportType == AttestationReportType.VERTICAL)
                {
                    fileName = "VerticalAttestationDocument";
                }
                else if (reportType == AttestationReportType.HORIZONTAL)
                {
                    fileName = "HorizontalAttestationDocument";
                }
                else
                {
                    fileName = "ConsolidatedAttestationDocument";
                }
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).MergeAttestationDocuments(tenantID, attestationDocPathToMerge, previousAttestationPdfPath, currentUserId
                                                                                                      , fileNameToAppend, fileName, deletePreviousAttestation);

            }
            catch (SysXException ex)
            {
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

        #region UAT-2628:System should check (scheduled) and fix document conversion and merging issues automatically. Send notification to a group after certain retry count

        public static List<ApplicantDocumentPocoClass> GetDocumentsDetailsForMergingAndConversing(Int32 tenantID, Int32 currentUserId, Int32 chunkSize, Int32 retryCount
                                                                                                  , String entityName, Int32 subEventId)
        {
            try
            {
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).GetDocumentsDetailsForMergingAndConversing(tenantID, currentUserId, chunkSize, retryCount, entityName, subEventId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ApplicantDocument> AppendApplicantDocumentAutomatically(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUserID
                                                                                  , List<ApplicantDocumentPocoClass> documentList, Int32 documentRetryCount)
        {
            try
            {
                List<lkpDocumentStatu> documentStatus = GetDocumentStatus(tenantID);
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).AppendApplicantDocumentAutomatically(organizationUserID, tenantID, currentLoggedInUserID
                                                                                                                 , documentStatus, documentList, documentRetryCount);
            }
            catch (SysXException ex)
            {
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

        #region UAT-2774

        public static Boolean ConvertSahredInvitationDocumentToPDF(List<ApplicantDocumentPocoClass> lstDocuments, Int32 tenantID, Int32 currentLoggedInUseId)
        {
            try
            {
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).ConvertSahredInvitationDocumentToPDF(lstDocuments, tenantID, currentLoggedInUseId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String ConvertSharedDocumentToPDFForPrint(List<Int32> lstDocumentIds, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetDocumentRepositoryRepoInstance(TenantId).ConvertSharedDocumentToPDFForPrint(lstDocumentIds, TenantId);
            }
            catch (SysXException ex)
            {
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

        #region UAT-3238
        public static List<ApplicantDocumentPocoClass> GetConvertApplicantDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId)
        {
            try
            {
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).GetConvertApplicantDocumentToPDF(applicantDocuments, tenantID, currentLoggedInUseId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ApplicantDocumentToBeMerged> GetApplicantDocumentsList(List<ApplicantDocumentToBeMerged> applicantDocumentToBeMergedList,Int32 tenantID)
        {
            return BALUtils.GetDocumentRepositoryRepoInstance(tenantID).GetApplicantDocumentList(applicantDocumentToBeMergedList);
        }

        public static Tuple<String, List<ApplicantDocumentToBeMerged>> CreateUnifiedDocumentForAgencyUser(List<ApplicantDocumentToBeMerged> applicantDocuments, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetDocumentRepositoryRepoInstance(tenantId).CreateUnifiedDocumentForAgencyUser(applicantDocuments,tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static byte[] GetBytesFromLoyalSystem(String filePath)
        {
            try
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    byte[] returnBytes = null;
                    if (File.Exists(filePath))
                    {
                        //ReadAllBytes is implemented in an obvious way. It uses the using-statement on a FileStream. 
                        //Then it loops through the file and puts the bytes into an array. In .NET 4.0, it throws an exception if the file exceeds 2 gigabytes.
                        returnBytes = File.ReadAllBytes(filePath);
                        return returnBytes;
                    }
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
            }

            return null;
        }
        #endregion
    }
}
