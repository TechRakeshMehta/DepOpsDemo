using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IUploadDocumentsView
    {
        List<ApplicantDocument> ToSaveApplicantUploadedDocuments { get; set; }
        Int32 TenantID { get; set; }
        //Applicantid Set when screen opens from Admin(verification Details And Portfolio Search)
        Int32 FromAdminApplicantID
        {
            get;
            set;
        }
        //Applicantid Set when screen opens from Admin(verification Details And Portfolio Search)
        Int32 FromAdminTenantID
        {
            get;
            set;
        }
        // to check from where this screen opens
        Boolean IsAdminScreen
        {
            get;
            set;
        }
        Int32 CurrentLoggedUserID
        {
            get;
        }
        Int32 OrganiztionUserID
        {
            get;
        }
        //UAT-3478
        Boolean isDropZoneEnabled
        {
            get;
            set;
        }
        String DropzoneID
        {
            get;
            set;
        }
        String ErrorMessage
        {
            get;
            set;
        }

        #region UAT-1049:Admin Data Entry
        Int16 DataEntryDocNewStatusId { get; set; }
        Int16 DataEntryDocCompleteStatusId { get; set; }        
        #endregion

        //UAT-2128
        List<UploadDocumentContract> lstSubcribedItems
        { get; set; }

        List<lkpItemDocMappingType> lstItemDocMappingType
        { get; set; }
        List<FingerPrintLocationImagesContract> AddedLocationImagesData { get; set; }
    }
}




