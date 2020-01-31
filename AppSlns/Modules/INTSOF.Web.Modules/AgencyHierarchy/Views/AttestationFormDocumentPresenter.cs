using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AttestationFormDocumentPresenter : Presenter<IAttestationFormDocument>
    {

        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
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

        public void GetAttestationFormDocument()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = DocumentType.ATTESTATION_FORM.GetStringValue();
            var _response = _agencyHierarchyProxy.GetClientSystemDocumentBasedOnDocumentType(serviceRequest);
            View.AttestationFormDocument = _response.Result;
        }

        public void GetAgencyHirarchySettings()
        {
            ServiceRequest<Int32,String> serviceRequest = new ServiceRequest<Int32,String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = AgencyHierarchySettingType.SPECIFIC_ATTESTATION_FORM.GetStringValue();
            AgencyHierarchySettingContract result = _agencyHierarchyProxy.GetAgencyHierarchySetting(serviceRequest).Result;
            View.AgencyHierarchySettingContract = result;
        }

        public bool SaveAttestationFormDocuments()
        {
            if (View.ToSaveUploadedDocuments.IsNotNull() && View.ToSaveUploadedDocuments.Count > AppConsts.NONE)
            {
                ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>> serviceRequest = new ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>>();
                serviceRequest.Parameter1 = View.AgencyHierarchyID;
                serviceRequest.Parameter2 = View.ToSaveUploadedDocuments;
                var _response = _agencyHierarchyProxy.SaveClientSystemDocument(serviceRequest);
            }

            ServiceRequest<AgencyHierarchySettingContract> serviceRequest1 = new ServiceRequest<AgencyHierarchySettingContract>();
            serviceRequest1.Parameter = View.AgencyHierarchySettingContract;
            Boolean result = _agencyHierarchyProxy.SaveAgencyHierarchySetting(serviceRequest1).Result;

            if (result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.ATTESTATION_FORM, View.CurrentUserId);
            }

            return result;
        }

        public bool DeleteAttestationFormDocuments()
        {
            ServiceRequest<Int32, Int32?, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32?, Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = (Int32?)null;
            serviceRequest.Parameter3 = View.CurrentUserId;
            serviceRequest.Parameter4 = DocumentType.ATTESTATION_FORM.GetStringValue();
            var _response = _agencyHierarchyProxy.DeleteClientSystemDocumentBasedOnDocType(serviceRequest);

            if (_response.Result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.ATTESTATION_FORM, View.CurrentUserId);
            }

            return _response.Result;
        }
    }
}
