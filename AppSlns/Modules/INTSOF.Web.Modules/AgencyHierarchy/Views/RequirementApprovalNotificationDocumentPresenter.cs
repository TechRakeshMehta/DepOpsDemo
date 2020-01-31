using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class RequirementApprovalNotificationDocumentPresenter : Presenter<IRequirementApprovalNotificationDocument>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        #region Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the every time the view loads          
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public Boolean SaveApprovalNotificationDocuments()
        {
            ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>> serviceRequest = new ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = View.ToSaveUploadedDocuments;
            var _response = _agencyHierarchyProxy.SaveClientSystemDocument(serviceRequest);

            if (_response.Result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.CHANGE_TYPE_DOCUMENT, View.CurrentUserId);
            }

            return _response.Result;
        }

        public Boolean DeleteApprovalNotificationDocuments()
        {
            ServiceRequest<Int32, Int32?, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32?, Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = (Int32?)null;
            serviceRequest.Parameter3 = View.CurrentUserId;
            serviceRequest.Parameter4 = DocumentType.DOCUMENT_FOR_REQUIREMENT_APPROVAL_NOTIFICATION.GetStringValue();
            var _response = _agencyHierarchyProxy.DeleteClientSystemDocumentBasedOnDocType(serviceRequest);

            if (_response.Result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.CHANGE_TYPE_DOCUMENT, View.CurrentUserId);
            }

            return _response.Result;
        }

        public void GetMappedRequirementApprovalNotificationDocument()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = DocumentType.DOCUMENT_FOR_REQUIREMENT_APPROVAL_NOTIFICATION.GetStringValue();
            var _response = _agencyHierarchyProxy.GetClientSystemDocumentBasedOnDocumentType(serviceRequest);
            View.RequirementApprovalNotificationDocument = _response.Result;
        }

        #endregion
    }
}
