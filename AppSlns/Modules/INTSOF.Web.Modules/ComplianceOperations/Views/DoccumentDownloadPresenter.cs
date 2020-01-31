using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;

namespace CoreWeb.ComplianceOperations.Views
{
    public class DoccumentDownloadPresenter : Presenter<IDoccumentDownloadView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public ApplicantDocument GetApplicantDocument()
        {
            return ComplianceDataManager.GetApplicantDocument(View.ApplicantDocumentId, View.TenantId);
        }

        /// <summary>
        /// Method to get the ServiceForm document
        /// </summary>
        /// <returns></returns>
        public SystemDocument GetServiceFormDocumentData()
        {
            if (View.SystemDocumentID > 0)
            {
                return ComplianceSetupManager.GetServiceFormDocument(SecurityManager.DefaultTenantID, View.SystemDocumentID);
            }
            return null;
        }

        public ClientSystemDocument GetClientSystemDocument()
        {
            if (View.ClientSystemDocumentID > 0)
            {
                return ComplianceDataManager.GetClientSystemDocument(View.ClientSystemDocumentID, View.TenantId);
            }
            return null;
        }

        public Entity.SharedDataEntity.SharedSystemDocument GetSharedSystemDocument()
        {
            if (View.SharedSystemDocumentID > 0)
            {
                return ClientContactManager.GetSharedSystemDocument(View.SharedSystemDocumentID);
            }
            return null;
        }
    }
}




