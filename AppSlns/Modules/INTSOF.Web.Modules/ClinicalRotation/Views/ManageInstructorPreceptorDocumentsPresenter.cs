using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{

    public class ManageInstructorPreceptorDocumentsPresenter : Presenter<IManageInstructorPreceptorDocumentsView>
    {
        private ClientContactProxy _clientContactProxy
        {
            get
            {
                return new ClientContactProxy();
            }
        }

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

        public void GetTenants()
        {
            ServiceRequest<String> serviceRequest1 = new ServiceRequest<String>();
            serviceRequest1.Parameter = View.ClientContactEmailID;
            var _serviceResponse1 = _clientContactProxy.GetClientContactTenantsIDByEmail(serviceRequest1);
            List<Int32> lstAssociatedTenantIds = _serviceResponse1.Result;

            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clientContactProxy.GetTenants(serviceRequest);
            //Show only those tenants for which the client contact is created.
            View.LstTenant = _serviceResponse.Result.Where(x => lstAssociatedTenantIds.Contains(x.TenantID)).ToList();
        }

        public void GetApplicantUploadedDocuments()
        {
            if (View.SelectedTenantID > AppConsts.NONE)
                //View.ApplicantUploadedDocuments = ComplianceDataManager.GetApplicantDocumentDetails(View.CurrentLoggedInUserID, View.SelectedTenantID);
                View.ApplicantUploadedDocuments = ComplianceDataManager.GetInstructorRequirementDocumentData(View.CurrentLoggedInUserID, View.SelectedTenantID);

            else
            {
                View.ApplicantUploadedDocuments = new List<Entity.ClientEntity.ApplicantDocumentDetails>();
            }
        }


        public Boolean DeleteApplicantUploadedDocument()
        {
            Boolean canDocumentBeDeleted = false;

            canDocumentBeDeleted = ApplicantRequirementManager.CanDeleteRqmtFieldUploadDoc(View.ApplicantUploadedDocumentID, View.SelectedTenantID);
            if (canDocumentBeDeleted == true)
            {
                if (ComplianceDataManager.DeleteApplicantUploadedDocument(View.ApplicantUploadedDocumentID, View.SelectedTenantID, View.CurrentLoggedInUserID, View.CurrentLoggedInUserID))
                {
                    if (!View.IsRequirementFieldUploadDocument)
                    {
                        SecurityManager.DeleteDocumentFromFlatDataEntry(View.ApplicantUploadedDocumentID, View.SelectedTenantID, View.CurrentLoggedInUserID, View.CurrentLoggedInUserID);
                    }
                }
                return true;
            }
            else
            {
                return false;
                // its mapped with item
            }
        }

        public Boolean UpdateApplicantUploadedDocument()
        {
            if (ComplianceDataManager.UpdateApplicantUploadedDocument(View.ToUpdateUploadedDocument, View.SelectedTenantID))
                return true;
            return false;

        }
        public String ConvertDocumentToPdfForPrint()
        {
            if (!View.DocumentIdsToPrint.IsNullOrEmpty())
            {
                return DocumentManager.ConvertDocumentToPDFForPrint(View.SelectedTenantID, View.DocumentIdsToPrint);
            }
            return String.Empty;
        }

    }
}
