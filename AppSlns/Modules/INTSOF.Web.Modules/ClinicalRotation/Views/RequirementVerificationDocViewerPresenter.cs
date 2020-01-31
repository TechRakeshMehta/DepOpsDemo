using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementVerificationDocViewerPresenter : Presenter<IRequirementVerificationDocViewerView>
    {
        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }


        public void GetApplicantDocument()
        {
            if (View.CurrentDocumentId > AppConsts.NONE && View.SelectedTenantId > AppConsts.NONE)
            {
                View.ApplicantDocument = ComplianceDataManager.GetApplicantDocument(View.CurrentDocumentId, View.SelectedTenantId);
            }
        }

        public byte[] GetPDFByteData()
        {
            if (View.ApplicantDocument.IsNotNull())
            {
                return DocumentManager.GetBytesFromUnifiedPdf(View.ApplicantDocument.PdfDocPath);
            }
            return null;
        }
    }
}
