
#region Namespaces

#region SystemDefined

using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Collections.Generic;

#endregion

#region UserDefined

using Entity;
using INTSOF.Utils;
using INTSOF.Contracts;
using INTSOF.SharedObjects;
using Business.RepoManagers;

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class UploadDisclosureDocumentsPresenter : Presenter<IUploadDisclosureDocumentsView>
    {
        public Boolean AddApplicantUploadedDocuments()
        {
            Int32 docStatusId = BackgroundSetupManager.GetDocumentStatusIDByCode(DislkpDocumentStatus.UPLOADED.GetStringValue());
            //Int32 docTypeId = BackgroundSetupManager.GetDocumentTypeIDByCode(DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue());

            View.ToSaveUploadedDisclosureDocuments.ForEach(condition =>
                {
                    condition.SD_DocStatus_ID = docStatusId;
                    //condition.SD_DocType_ID = docTypeId;
                });

            return BackgroundSetupManager.SaveDisclosureTemplateDocument(View.ToSaveUploadedDisclosureDocuments); ;
        }

        public Int32? GetDocumentTypeIDByCode(String DocumentTypeCode)
        {
            return SecurityManager.GetDocumentTypeIDByCode(DocumentTypeCode);
        }
    }
}




