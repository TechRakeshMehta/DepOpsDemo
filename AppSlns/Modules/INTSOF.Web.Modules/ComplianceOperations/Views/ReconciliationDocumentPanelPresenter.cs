using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ReconciliationDocumentPanelPresenter : Presenter<IReconciliationDocumentPanelView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Get all the documents related to the user.
        /// </summary>
        public void GetApplicantDocuments()
        {
            if (View.OrganizationUserId != null && View.OrganizationUserId != 0)
                View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocumentsData(View.OrganizationUserId, View.SelectedTenantId);
        }

        /// <summary>
        /// Get  category specific documents.
        /// </summary>
        public void BindDocumentsForCaegory()
        {
            GetApplicantDocuments();
            var documentList = View.lstApplicantDocument.Where
                (x =>
                    (x.CategoryID.HasValue ? x.CategoryID.Value == View.SelectedComplianceCategoryId_Global : false)
                    &&
                    (x.PackageId.HasValue ? x.PackageId.Value == View.PackageSubscriptionId : false)
                    ).ToList();
            if ((documentList != null) && (documentList.Count > 0))
            {
                View.lstApplicantDocument = documentList;
            }
            else
            {
                View.lstApplicantDocument = new List<ApplicantDocuments>();
            }
        }

        // TODO: Handle other view events and set state in the view

        public Int32 GetDocumentStatusID()
        {
            List<lkpDocumentStatu> lkpDocumentStatus = Business.RepoManagers.DocumentManager.GetDocumentStatus(View.SelectedTenantId);
            var mergingCompletedCode = DocumentStatus.MERGING_COMPLETED.GetStringValue();
            //Get the document status id based on Merging Completed status code
            return lkpDocumentStatus.Where(obj => obj.DMS_Code == mergingCompletedCode).FirstOrDefault().DMS_ID;
        }

        #region UAT-1538::Unified Document/ single document option and updates to document exports
        public Entity.UtilityFeatureUsage GetDocViewTypeSettings()
        {
            return ComplianceDataManager.GetDocumentViewTypeSettingByUserID(View.CurrentLoggedInUserId);
        }

        public Boolean SaveUpdateDocumentViewSetting()
        {
            return ComplianceDataManager.SaveUpdateDocumentViewSetting(View.CurrentLoggedInUserId, UtilityFeatures.Unified_Document.GetStringValue());
        }
        #endregion

        /// <summary>
        /// Get the 'Screening Document' TypeID, to remove it, while setting single document in PDF Viewer.
        /// </summary>
        public void GetScreeningDocumentTypeId()
        {
            var _lstDocumentTypes = LookupManager.GetLookUpData<lkpDocumentType>(View.SelectedTenantId);
            View.ScreeningDocumentTypeId = _lstDocumentTypes.First(dt => dt.DMT_Code == DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue())
                                                             .DMT_ID;
        }
    }
}




