using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class AdditionalDocumentsMappingPresenter : Presenter<IAdditionalDocumentsMappingView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        public void GetGenericSystemDocumentMapping()
        {
            View.RecortTypeID = BackgroundProcessOrderManager.GetlkpRecordTypeIdByCode(View.TenantId, View.RecortTypeCode);
            View.lstGenericSystemDocumentMapping = ComplianceSetupManager.GetGenericSystemDocumentMapping(View.TenantId, View.RecordID, View.RecortTypeID);
            View.lstMappedSysDocIDs = View.lstGenericSystemDocumentMapping.Select(x => x.SystemDocID).ToList();
        }

        public void GetAdditionalDocuments()
        {
            List<String> lstSatusCodeToFetch = new List<String>();
            lstSatusCodeToFetch.Add(DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue());
            //Fetch only Additional Documents
            List<Int32> lstDocStatusIDs = SecurityManager.GetDocumentTypes().Where(x => lstSatusCodeToFetch.Contains(x.DT_Code) && x.DT_IsActive).Select(cond => cond.DT_ID).ToList();
            List<Entity.SystemDocument> tmpListSysDoc = BackgroundSetupManager.GetBothUploadedDisclosureDocuments(lstDocStatusIDs);
            if (!View.lstMappedSysDocIDs.IsNullOrEmpty())
            {
                View.lstAdditionalDocuments = tmpListSysDoc.Where(x => !View.lstMappedSysDocIDs.Contains(x.SystemDocumentID)).ToList();
            }
            else
            {
                View.lstAdditionalDocuments = tmpListSysDoc;
            }

        }

        public void SaveAdditionalDocumentMapping()
        {
            View.RecortTypeID = BackgroundProcessOrderManager.GetlkpRecordTypeIdByCode(View.TenantId, View.RecortTypeCode);
            View.SuccessMessage = ComplianceSetupManager.SaveAdditionalDocumentMapping(View.TenantId, View.RecordID, View.RecortTypeID, View.SelectedAdditionalDocumentsID, View.CurrentLoggedInUserId);
        }

        public void DeleteAdditionalDocumentMapping()
        {
            View.SuccessMessage = ComplianceSetupManager.DeleteAdditionalDocumentMapping(View.TenantId, View.DocMappingID, View.CurrentLoggedInUserId);
        }
    }
}
