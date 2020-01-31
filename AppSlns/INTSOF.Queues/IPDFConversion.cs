using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace INTSOF.Queues
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPDFConversion" in both code and config file together.
    [ServiceContract]
    public interface IPDFConversion
    {
        [OperationContract]
        string DoWork();

        [OperationContract]
        byte[] GeneratePDF(String urlToConvert);

        [OperationContract]
        List<ApplicantDoc> ConvertApplicantDocumentToPDF(List<ApplicantDoc> applicantDocuments, Int32 tenantID);

        [OperationContract]
        List<ApplicantDocMerging> AppendConvertedDocumentToPDF(Int32 unifiedDocumentID, Int32 tenantID, List<ApplicantDocToBeMerged> applicantDocuments,
            String previousUnifiedPdfPath, Int32 sequenceOrder, Int32 previousPages,Boolean IsDocumentFlatten);

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        [OperationContract]
        String ConvertDocumentToPDFForPrint(Int32 tenantID, List<ApplicantDocToBeMerged> applicantDocumentsToPrint);
        #endregion

        #region UAT-1564:delete a document from Unified also when deleting it from manage Document page
        /// <summary>
        ///Delete applicant document from unified pdf.
        /// </summary>
        /// <param name="tenantID">InstitutionID</param>
        /// <param name="applicantDocuments">List of Applicant Documents to Print</param>
        /// <returns>PDF file path to print</returns>
        [OperationContract]
        String DeleteAppDocumentFromUnifiedPDF(Int32 tenantID, Int32 pagesToAdd, Int32 newStartPageNumber, String previousUnifiedPdfPath, Int32 unifiedDocumentID
                                               , Boolean isNeedToDeleteUnifiedDocument);
        #endregion

        [OperationContract]
        String MergeAttestationDocuments(Int32 tenantID, String attestationDocPathToMerge, String previousAttestationPdfPath, Int32 currentUserId, String fileNameToAppend, String fileName, Boolean deletePreviousAttestation);

        [OperationContract]
        List<ApplicantDocToBeMerged> GetPdfPageDetails(List<ApplicantDocToBeMerged> applicantDocToBeMergedList);
        [OperationContract]
        Tuple<String, List<ApplicantDocToBeMerged>> AppendConvertedUnifiedDocumentToPDF(List<ApplicantDocToBeMerged> applicantDocuments);
    }

    [Serializable]
    [DataContract]
    public class ApplicantDoc
    {
        Int32 _ApplicantDocumentID;
        [DataMember]
        public Int32 ApplicantDocumentID
        {
            get { return _ApplicantDocumentID; }
            set { _ApplicantDocumentID = value; }
        }

        Int32? _OrganizationUserID;
        [DataMember]
        public Int32? OrganizationUserID
        {
            get { return _OrganizationUserID; }
            set { _OrganizationUserID = value; }
        }

        String _FileName;
        [DataMember]
        public String FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        String _MIME;
        [DataMember]
        public String MIME
        {
            get { return _MIME; }
            set { _MIME = value; }
        }

        Int32? _Size;
        [DataMember]
        public Int32? Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        String _Description;
        [DataMember]
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        String _DocumentPath;
        [DataMember]
        public String DocumentPath
        {
            get { return _DocumentPath; }
            set { _DocumentPath = value; }
        }

        Boolean _IsDeleted;
        [DataMember]
        public Boolean IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }

        Int32? _CreatedByID;
        [DataMember]
        public Int32? CreatedByID
        {
            get { return _CreatedByID; }
            set { _CreatedByID = value; }
        }
        Boolean? _IsPdfDocumnetFormFlattening;
        [DataMember]
        public Boolean? IsPdfDocumnetFormFlattening
        {
            get { return _IsPdfDocumnetFormFlattening; }
            set { _IsPdfDocumnetFormFlattening = value; }
        }


        DateTime _CreatedOn;
        [DataMember]
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }

        Int32? _ModifiedByID;
        [DataMember]
        public Int32? ModifiedByID
        {
            get { return _ModifiedByID; }
            set { _ModifiedByID = value; }
        }

        DateTime? _ModifiedOn;
        [DataMember]
        public DateTime? ModifiedOn
        {
            get { return _ModifiedOn; }
            set { _ModifiedOn = value; }
        }

        Guid? _Code;
        [DataMember]
        public Guid? Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        String _PdfFileName;
        [DataMember]
        public String PdfFileName
        {
            get { return _PdfFileName; }
            set { _PdfFileName = value; }
        }

        String _PdfDocPath;
        [DataMember]
        public String PdfDocPath
        {
            get { return _PdfDocPath; }
            set { _PdfDocPath = value; }
        }

        String _ConversionNotes;
        [DataMember]
        public String ConversionNotes
        {
            get { return _ConversionNotes; }
            set { _ConversionNotes = value; }
        }

        Int32? _TotalPages;
        [DataMember]
        public Int32? TotalPages
        {
            get { return _TotalPages; }
            set { _TotalPages = value; }
        }

        Int32? _DocumentType;
        [DataMember]
        public Int32? DocumentType
        {
            get { return _DocumentType; }
            set { _DocumentType = value; }
        }

        Boolean _IsCompressed;
        [DataMember]
        public Boolean IsCompressed
        {
            get { return _IsCompressed; }
            set { _IsCompressed = value; }
        }

        String _OriginalDocMD5Hash;
        [DataMember]
        public String OriginalDocMD5Hash
        {
            get { return _OriginalDocMD5Hash; }
            set { _OriginalDocMD5Hash = value; }
        }

        Int32? _OriginalDocSize;
        [DataMember]
        public Int32? OriginalDocSize
        {
            get { return _OriginalDocSize; }
            set { _OriginalDocSize = value; }
        }
    }

    [Serializable]
    [DataContract]
    public class ApplicantDocToBeMerged
    {
        Int32 _ApplicantDocumentID;
        [DataMember]
        public Int32 ApplicantDocumentID
        {
            get { return _ApplicantDocumentID; }
            set { _ApplicantDocumentID = value; }
        }

        String _PdfDocPath;
        [DataMember]
        public String PdfDocPath
        {
            get { return _PdfDocPath; }
            set { _PdfDocPath = value; }
        }

        Int32? _TotalPages;
        [DataMember]
        public Int32? TotalPages
        {
            get { return _TotalPages; }
            set { _TotalPages = value; }
        }


    }

    [Serializable]
    [DataContract]
    public class ApplicantDocMerging
    {
        Int32 _ADM_ID;
        [DataMember]
        public Int32 ADM_ID
        {
            get { return _ADM_ID; }
            set { _ADM_ID = value; }
        }

        Int32? _ADM_ApplicantDocumentID;
        [DataMember]
        public Int32? ADM_ApplicantDocumentID
        {
            get { return _ADM_ApplicantDocumentID; }
            set { _ADM_ApplicantDocumentID = value; }
        }

        //ADM_UnifiedPdfDocumentID
        Int32? _ADM_UnifiedPdfDocumentID;
        [DataMember]
        public Int32? ADM_UnifiedPdfDocumentID
        {
            get { return _ADM_UnifiedPdfDocumentID; }
            set { _ADM_UnifiedPdfDocumentID = value; }
        }

        Int32? _ADM_TotalPages;
        [DataMember]
        public Int32? ADM_TotalPages
        {
            get { return _ADM_TotalPages; }
            set { _ADM_TotalPages = value; }
        }

        //ADM_StartPageNum
        Int32? _ADM_StartPageNum;
        [DataMember]
        public Int32? ADM_StartPageNum
        {
            get { return _ADM_StartPageNum; }
            set { _ADM_StartPageNum = value; }
        }

        //ADM_EndPageNum
        Int32? _ADM_EndPageNum;
        [DataMember]
        public Int32? ADM_EndPageNum
        {
            get { return _ADM_EndPageNum; }
            set { _ADM_EndPageNum = value; }
        }

        Int32? _ADM_SequenceOrder;
        [DataMember]
        public Int32? ADM_SequenceOrder
        {
            get { return _ADM_SequenceOrder; }
            set { _ADM_SequenceOrder = value; }
        }

        Boolean _ADM_IsDeleted;
        [DataMember]
        public Boolean ADM_IsDeleted
        {
            get { return _ADM_IsDeleted; }
            set { _ADM_IsDeleted = value; }
        }

        //ADM_CreatedByID
        Int32 _ADM_CreatedByID;
        [DataMember]
        public Int32 ADM_CreatedByID
        {
            get { return _ADM_CreatedByID; }
            set { _ADM_CreatedByID = value; }
        }

        DateTime _ADM_CreatedOn;
        [DataMember]
        public DateTime ADM_CreatedOn
        {
            get { return _ADM_CreatedOn; }
            set { _ADM_CreatedOn = value; }
        }


        Int32? _ADM_ModifiedByID;
        [DataMember]
        public Int32? ADM_ModifiedByID
        {
            get { return _ADM_ModifiedByID; }
            set { _ADM_ModifiedByID = value; }
        }

        //
        DateTime? _ADM_ModifiedOn;
        [DataMember]
        public DateTime? ADM_ModifiedOn
        {
            get { return _ADM_ModifiedOn; }
            set { _ADM_ModifiedOn = value; }
        }

        Int32? _ADM_DocumentStatusID;
        [DataMember]
        public Int32? ADM_DocumentStatusID
        {
            get { return _ADM_DocumentStatusID; }
            set { _ADM_DocumentStatusID = value; }
        }

        String _ADM_MergingNotes;
        [DataMember]
        public String ADM_MergingNotes
        {
            get { return _ADM_MergingNotes; }
            set { _ADM_MergingNotes = value; }
        }

        String _UnifiedDocumentPath;
        [DataMember]
        public String UnifiedDocumentPath
        {
            get { return _UnifiedDocumentPath; }
            set { _UnifiedDocumentPath = value; }
        }

        Boolean _IsFlattenFile;
        [DataMember]
        public Boolean IsFlattenFile
        {
            get { return _IsFlattenFile; }
            set { _IsFlattenFile = value; }
        }
    }
}
