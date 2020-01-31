
#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;

#endregion

#region UserDefined

using INTSOF.Utils;
using INTSOF.Contracts;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class DisclosureDocumentsPresenter : Presenter<IDisclosureDocumentsView>
    {
        //public void GetUploadedDisclosureDocuments()
        //{
        //    Int32 docTypeId = BackgroundSetupManager.GetDocumentTypeIDByCode(DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue());
        //    View.DisclosureDocuments = BackgroundSetupManager.GetDisclosureTemplateDocuments(docTypeId);
        //}

        /// <summary>
        /// Get Disclosure Document template and New Disclosure Document.
        /// </summary>
        public void GetBothUploadedDisclosureDocuments()
        {
            List<String> lstSatusCodeToFetch = new List<String>();
            lstSatusCodeToFetch.Add(DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue());
            lstSatusCodeToFetch.Add(DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue());
            lstSatusCodeToFetch.Add(DislkpDocumentType.BADGE_FORM.GetStringValue());
            List<Int32> lstDocStatusIDs = SecurityManager.GetDocumentTypes().Where(x => lstSatusCodeToFetch.Contains(x.DT_Code) && x.DT_IsActive).Select(cond => cond.DT_ID).ToList();
            View.DisclosureDocuments = BackgroundSetupManager.GetBothUploadedDisclosureDocuments(lstDocStatusIDs);
        }

        public Boolean DeleteUploadedDisclosureDocument(Int32 currentUserId)
        {
            return BackgroundSetupManager.DeleteDisclosureTemplateDocument(View.SystemDocumentID, currentUserId);
        }

        public Boolean UpdateUploadedDisclosureDocument()
        {
            return BackgroundSetupManager.UpdateDisclosureTemplateDocument(View.DisclosureDocumentToUpdate, View.SelectedExtBkgSvcID);
        }

        public void GetDocumentTypes()
        {
            String discTempCode = DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue();
            List<String> lstSatusCodeToFetch = new List<String>();
            lstSatusCodeToFetch.Add(discTempCode);
            lstSatusCodeToFetch.Add(DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue());
            lstSatusCodeToFetch.Add(DislkpDocumentType.BADGE_FORM.GetStringValue());
            View.DocumentTypeList = SecurityManager.GetDocumentTypes().Where(x => lstSatusCodeToFetch.Contains(x.DT_Code) && x.DT_IsActive).ToList();
            View.DocumentTypeList.Where(x => x.DT_Code == discTempCode).FirstOrDefault().DT_Name = "D & A Document";
        }

        #region UAT-2625
        public void GetAgeGroupTypes()
        {
            var tempListAgeGroups = SecurityManager.GetAgeGroupTypes();
            if (!tempListAgeGroups.IsNullOrEmpty())
            {
                tempListAgeGroups.Insert(0, new lkpDisclosureDocumentAgeGroup { LDDAG_ID = 0, LDDAG_Name = "--Select--" });
                View.DisclosureDocAgeGroupTypeList = tempListAgeGroups;
            }
            else
            {
                View.DisclosureDocAgeGroupTypeList = new List<lkpDisclosureDocumentAgeGroup>();
            }
        }
        #endregion

        #region UAT-3745

        public void GetExternalBkgSvc()
        {
            View.lstExtBkgSvc = new List<ExternalBkgSvc>();
            View.lstExtBkgSvc = BackgroundSetupManager.GetExternalBkgSvc();

            if (!View.lstExtBkgSvc.IsNullOrEmpty())
            {
                foreach (ExternalBkgSvc externalBkgSvc in View.lstExtBkgSvc)
                {
                    externalBkgSvc.EBS_Name = externalBkgSvc.EBS_ExternalCode + "- " + externalBkgSvc.EBS_Name;
                }
            }
        }

        #endregion
    }
}




