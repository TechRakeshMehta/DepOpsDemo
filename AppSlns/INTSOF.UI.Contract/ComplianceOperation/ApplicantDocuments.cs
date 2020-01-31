using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class ApplicantDocuments
    {
        private Int32 applicantDocumentId;
        private Int32? documentSize;
        private Int32? packageId;
        private Int32? itemID;
        private Int32? categoryID;
        private String imageUrl;
        private String imageText;
        private String documentName;
        private Int32? applicantDocumentMergingID;
        private Int32? applicantDocumentMergingStatusID;
        private Int32? unifiedDocumentStartPageID;
        private Int32? unifiedDocumentEndPageID;
        private Int32? unifiedPdfDocumentID;
        private String unifiedPdfFileName;
        private String unifiedPdfDocPath;
        private Int32? unifiedDocumentStatusID;
        private Int32? documentType;
        private Int32? complianceItemID;
        private Int32? isViewDocType;
        private String documentDescription;
        private String documentDescriptionToolTip;

        public ApplicantDocuments(String ImageText, String ImageUrl, Int32 ApplicantDocumentId, Int32? DocumentSize, Int32? ItemID, Int32? PackageId,
            Int32? CategoryID, Int32? applicantDocumentMergingID, Int32? unifiedDocumentStartPageID, Int32? unifiedDocumentEndPageID
            , Int32? unifiedPdfDocumentID, String unifiedPdfFileName, String unifiedPdfDocPath, Int32? unifiedDocumentStatusID, Int32? DocumentType,
            Int32? applicantDocumentMergingStatusID, Int32? ComplianceItemId, String DocumentName = "", Int32? IsViewDocType = null, String DocumentDescription = "")
        {
            this.applicantDocumentId = ApplicantDocumentId;
            this.imageText = ImageText;
            this.imageUrl = ImageUrl;
            this.documentName = DocumentName;
            this.documentSize = DocumentSize;
            this.itemID = ItemID;
            this.packageId = PackageId;
            this.categoryID = CategoryID;
            this.applicantDocumentMergingID = applicantDocumentMergingID;
            this.applicantDocumentMergingStatusID = applicantDocumentMergingStatusID;
            this.unifiedDocumentStartPageID = unifiedDocumentStartPageID;
            this.unifiedDocumentEndPageID = unifiedDocumentEndPageID;
            this.unifiedPdfDocumentID = unifiedPdfDocumentID;
            this.unifiedPdfFileName = unifiedPdfFileName;
            this.unifiedPdfDocPath = unifiedPdfDocPath;
            this.unifiedDocumentStatusID = unifiedDocumentStatusID;
            this.documentType = DocumentType;
            this.complianceItemID = ComplianceItemId;
            this.isViewDocType = IsViewDocType;
            if (!String.IsNullOrEmpty(DocumentDescription) && DocumentDescription.Length > 15)
                this.documentDescription = DocumentDescription.Substring(0, 15) + "...";
            else
                this.documentDescription = DocumentDescription;

            this.documentDescriptionToolTip = DocumentDescription;
        }

        public String ImageUrl { get { return imageUrl; } }
        public String ImageText { get { return imageText; } }
        public Int32 ApplicantDocumentId { get { return applicantDocumentId; } }
        public Int32? ItemID { get { return itemID; } }
        public Int32? PackageId { get { return packageId; } }
        public Int32? CategoryID { get { return categoryID; } }
        public Int32? DocumentSize { get { return documentSize; } }
        public String DocumentName { get { return documentName; } }
        public Int32? ApplicantDocumentMergingID { get { return applicantDocumentMergingID; } }
        public Int32? ApplicantDocumentMergingStatusID { get { return applicantDocumentMergingStatusID; } }
        public Int32? UnifiedDocumentStartPageID { get { return unifiedDocumentStartPageID; } }
        public Int32? UnifiedDocumentEndPageID { get { return unifiedDocumentEndPageID; } }
        public Int32? UnifiedPdfDocumentID { get { return unifiedPdfDocumentID; } }
        public String UnifiedPdfFileName { get { return unifiedPdfFileName; } }
        public String UnifiedPdfDocPath { get { return unifiedPdfDocPath; } }
        public Int32? UnifiedDocumentStatusID { get { return unifiedDocumentStatusID; } }
        public Int32? DocumentType { get { return documentType; } }
        public Int32? ComplianceItemID { get { return complianceItemID; } }
        public Int32? IsViewDocType { get { return isViewDocType; } }
        public String DocumentDescription { get { return documentDescription; } }
        public String DocumentDescriptionToolTip { get { return documentDescriptionToolTip; } }
    }
}
