using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public class ManageSharedUserDocumentPresenter : Presenter<IManageSharedUserDocumentView>
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

        public void SaveUploadedDocument()
        {
            if (View.ClientContactID > AppConsts.NONE && View.SelectedTenantID > AppConsts.NONE)
            {
                ServiceRequest<SharedSystemDocumentContract, Int32, Int32> serviceRequest = new ServiceRequest<SharedSystemDocumentContract, Int32, Int32>();
                serviceRequest.Parameter1 = View.UploadedDocument;
                serviceRequest.Parameter2 = View.SelectedTenantID;
                serviceRequest.Parameter3 = View.ClientContactID;
                var _serviceResponse = _clientContactProxy.SaveUploadedDocument(serviceRequest);
                View.SuccessMsg = _serviceResponse.Result;
            }
        }

        public void DeleteUploadedDocument(int DocumentID)
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = DocumentID;
            serviceRequest.Parameter2 = View.ClientContactID;
            var _serviceResponse = _clientContactProxy.DeleteUploadedDocument(serviceRequest);
            View.SuccessMsg = _serviceResponse.Result;
        }

        public void GetClientContactByEmail()
        {
            if (!View.ClientContactEmailID.IsNullOrEmpty() && View.SelectedTenantID > AppConsts.NONE)
            {
                ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
                serviceRequest.Parameter1 = View.SelectedTenantID;
                serviceRequest.Parameter2 = View.ClientContactEmailID;
                var _serviceResponse = _clientContactProxy.GetClientContactByEmail(serviceRequest);
                ClientContactContract contact = _serviceResponse.Result;
                if (!contact.IsNullOrEmpty())
                    View.ClientContactID = contact.ClientContactID;
            }
        }

        public void GetClientContactDocument()
        {
            if (View.SelectedTenantID == AppConsts.NONE)
            {
                View.UploadedDocumentList = new List<SharedSystemDocumentContract>();
            }
            else
            {
                //ClientContactID
                if (View.ClientContactID != AppConsts.NONE)
                {
                    ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                    serviceRequest.Parameter = View.ClientContactID;
                    var _serviceResponse = _clientContactProxy.GetClientDocuments(serviceRequest);
                    View.UploadedDocumentList = _serviceResponse.Result;
                    //View.UploadedDocumentList = ClientContactManager.GetClientDocuments(View.ClientContactID);
                }
            }
        }

        public void GetDocumentType()
        {
            var _serviceResponse = _clientContactProxy.GetDocumentTypeList();

            //UAT-1678: Instructor/Preceptor updates
            var documentTypeList = _serviceResponse.Result;
            var attestDocCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT.GetStringValue();
            var verAttestDocCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue();
            var consolidatedAttestDocCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED.GetStringValue();
            var AttestationWithoutSign = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();
            View.DocumentTypeList = documentTypeList.Where(x => x.Code != attestDocCode && x.Code != verAttestDocCode
                                                            && x.Code != consolidatedAttestDocCode && x.Code != AttestationWithoutSign).ToList();

            //View.DocumentTypeList = _serviceResponse.Result;
            //Removed Attestation Document Type from list.
            //SharedSystemDocTypeContract docTypeToRemove = View.DocumentTypeList.Where(x => x.Code == "AAAE").FirstOrDefault();
            //View.DocumentTypeList.Remove(docTypeToRemove);
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
    }
}
