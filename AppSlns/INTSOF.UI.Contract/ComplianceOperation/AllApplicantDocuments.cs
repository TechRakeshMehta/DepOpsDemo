using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
   public class AllApplicantDocuments
    {
        private Int32 applicantDocumentId;
        private String documentName;
        private Boolean isChecked;
        //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
        private Int32? isViewDocType;
        private Int32? itemId;

        public AllApplicantDocuments(Int32 ApplicantDocumentId, String DocumentName, Boolean IsChecked, Int32? IsViewDocType = null, Int32? ItemId = null) 
        {
            this.applicantDocumentId = ApplicantDocumentId;
            this.documentName = DocumentName;
            this.isChecked = IsChecked;
            //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
            this.isViewDocType = IsViewDocType;
            this.itemId = ItemId;
        }
        public Int32 ApplicantDocumentId { get { return applicantDocumentId; } }
        public String DocumentName { get { return documentName; } }
        public Boolean IsChecked { get { return isChecked; } }
        //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
        public Int32? IsViewDocType { get { return isViewDocType; } }
        public Int32? ItemId { get { return itemId; } }
        
    }
}
