using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SysXSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;


namespace CoreWeb.IntsofSecurityModel.Views
{
    public class ExternalViewDocumentPresenter : Presenter<IExternalViewDocument>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetApplicantDocuments()
        {
            if (View.TenantID > 0 && !View.DocumentIDs.IsNullOrEmpty())
            {
                List<Int32> lstDocIds = View.DocumentIDs.Split(',').Select(int.Parse).ToList();
                View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocuments(View.TenantID, lstDocIds);
            }
            else
            {
                View.lstApplicantDocument = new List<Entity.ClientEntity.ApplicantDocument>();
            }
        }

        public byte[] GetPDFByteData()
        {
            if (!View.ApplicantDocumentPath.IsNullOrEmpty())
            {
                return DocumentManager.GetBytesFromUnifiedPdf(View.ApplicantDocumentPath);
            }
            return null;
        }

    }
}
