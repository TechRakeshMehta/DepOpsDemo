using Entity.ClientEntity;
using INTSOF.Utils.CommonPocoClasses;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IDocumentRepository
    {
        /// <summary>
        /// Get UnifiedPdfDocument based on the unifiedDocumentId
        /// </summary>
        /// <param name="unifiedDocumentId">unifiedDocumentId</param>
        /// <returns>UnifiedPdfDocument</returns>
        UnifiedPdfDocument GetUnifiedPdfDocument(Int32 unifiedDocumentId);

        /// <summary>
        /// Get UnifiedPdfDocument based on the organizationUserId
        /// </summary>
        /// <param name="organizationUserId">organizationUserId</param>
        /// <returns>UnifiedPdfDocument</returns>
        UnifiedPdfDocument GetPdfAsUnifiedDocument(Int32 organizationUserId);
        /// <summary>
        /// UAT:4132 Update the table after unified Documnet is FormFlattening
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        //bool UpdateUnifiedDocumentIsFormFlattening(Int32 organizationUserId);

        //bool UpdateApplicantIsFormFlattening(Int32 CurrentLoggedUserId, Int32 ApplicantDocumentId);

        /// <summary>
        /// Converts a list of applicant documents to pdf
        /// </summary>
        /// <param name="applicantDocuments">List of Applicants Documents to be converted</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="currentLoggedInUseId"> Current Logged In UserID</param>
        /// <returns>True/False</returns>
        //Boolean ConvertApplicantDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId, String applicantFileLocation);
        Boolean ConvertApplicantDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId);

        /// <summary>
        /// Append all converted applicant documents to single unified document
        /// </summary>
        /// <param name="organizationUserID">Applicant's Organization UserID</param>
        /// <param name="tenantID">Institution ID</param>
        /// <param name="currentLoggedInUserID">Current Logged In User ID</param>
        /// <param name="documentStatus">Lookup table containing Document Status</param>
        /// <returns>True/False</returns>
        Boolean AppendApplicantDocument(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUserID, List<lkpDocumentStatu> documentStatus, List<Int32> documentIds = null, Boolean isNeedToFilterDocuments = false);

        /// <summary>
        /// Check the unified document merging status 
        /// </summary>
        /// <param name="organizationUserId">organizationUserId</param>
        /// <param name="documentStatusId">documentStatusId</param>
        /// <returns></returns>
        Boolean IsMergingInProgress(Int32 organizationUserId, Int32 documentStatusId);

        /// <summary>
        /// Get the unified pdf document with status merging in progress
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="documentStatusId"></param>
        /// <returns></returns>
        UnifiedPdfDocument GetAllMergingInProgressUnifiedDoc(Int32 organizationUserId, Int32 documentStatusId);

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        /// <summary>
        /// Append applicant documents to single unified document for print
        /// </summary>
        /// <param name="documentStatus">Lookup table containing Document Status</param>
        /// <param name="appDocumentIdsToPrint">Applicant document ids to print</param>
        /// <param name="tenantID">Institution ID</param>
        /// <returns>String unified document path</returns>
        String ConvertDocumentToPDFForPrint(List<lkpDocumentStatu> documentStatus, List<Int32> appDocumentIdsToPrint, Int32 tenantId);
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
        Boolean DeleteApplicantDocFromUnifiedDoc(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUserID, Int32 appDocIdToDelete, List<lkpDocumentStatu> documentStatus, Boolean clearContext);
        #endregion

        #region UAT-2443:Attestation Merge and multiple share behavior changes
        String MergeAttestationDocuments(Int32 tenantID, String attestationDocPathToMerge, String previousAttestationPdfPath, Int32 currentUserId, String fileNameToAppend, String fileName, Boolean deletePreviousAttestation);
        #endregion

        #region UAT-2628:System should check (scheduled) and fix document conversion and merging issues automatically. Send notification to a group after certain retry count

        List<ApplicantDocumentPocoClass> GetDocumentsDetailsForMergingAndConversing(Int32 tenantID, Int32 currentUserId, Int32 chunkSize, Int32 retryCount, String entityName
                                                                                    , Int32 subEventId);

        /// <summary>
        /// Append all converted applicant documents to single unified document
        /// </summary>
        List<ApplicantDocument> AppendApplicantDocumentAutomatically(Int32 organizationUserID, Int32 tenantID, Int32 currentLoggedInUserID, List<lkpDocumentStatu> documentStatus
                                                            , List<ApplicantDocumentPocoClass> documentList, Int32 documentRetryCount);

        #endregion

        #region UAT-2774
        Boolean ConvertSahredInvitationDocumentToPDF(List<ApplicantDocumentPocoClass> lstDocuments, Int32 tenantID, Int32 currentLoggedInUseId);
        String ConvertSharedDocumentToPDFForPrint(List<Int32> DocumentIdsToPrint, Int32 tenantId);
        #endregion

        List<ApplicantDocumentPocoClass> ConvertAttestationDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId);

        #region UAT-3238
        List<ApplicantDocumentPocoClass> GetConvertApplicantDocumentToPDF(List<ApplicantDocumentPocoClass> applicantDocuments, Int32 tenantID, Int32 currentLoggedInUseId);
        List<ApplicantDocumentToBeMerged> GetApplicantDocumentList(List<ApplicantDocumentToBeMerged> applicantDocumentToBeMergedList);
        Tuple<String, List<ApplicantDocumentToBeMerged>> CreateUnifiedDocumentForAgencyUser(List<ApplicantDocumentToBeMerged> applicantDocuments, Int32 tenantId);
        #endregion
    }
}
