using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public class ViewDocumentPopupPresenter : Presenter<IViewDocumentView>
    {
        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }
        public void GetSystemDocumentData()
        {
            if (View.CurrentViewContext.ClientSysDocId > 0 && View.CurrentViewContext.TenantId > 0)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<int>();
                serviceRequest.Parameter = View.CurrentViewContext.ClientSysDocId;
                serviceRequest.SelectedTenantId = View.CurrentViewContext.TenantId;
                var _serviceResponse = _applicantClinicalRotationProxy.GetClientSystemDocument(serviceRequest);
                View.ClientSystemDocContract = _serviceResponse.Result;
            }
        }

        public void GetAttributeProperties()
        {
            if (View.CurrentViewContext.ReqObjectTreeId > 0 && View.CurrentViewContext.TenantId > 0)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<int>();
                serviceRequest.Parameter = View.CurrentViewContext.ReqObjectTreeId;
                serviceRequest.SelectedTenantId = View.CurrentViewContext.TenantId;
                var _serviceResponse = _applicantClinicalRotationProxy.GetObjectTreeProperties(serviceRequest);
                View.objectAttributeContract = _serviceResponse.Result;
            }
        }
        public void GetDocumentData()
        {
            if (View.CurrentViewContext.ClientSysDocId > 0 && View.CurrentViewContext.ReqObjectTreeId > 0 && View.CurrentViewContext.TenantId > 0)
            {
                ServiceRequest<Int32, Int32, Int32> serviceRequest = new ServiceRequest<int, int, int>();
                serviceRequest.Parameter1 = View.CurrentViewContext.ApplicantDocId;
                serviceRequest.Parameter2 = View.CurrentViewContext.ClientSysDocId;
                serviceRequest.Parameter3 = View.CurrentViewContext.ReqFieldId;
                serviceRequest.SelectedTenantId = View.CurrentViewContext.TenantId;
                var _serviceResponse = _applicantClinicalRotationProxy.GetViewDocumentData(serviceRequest);
                View.ViewDocContract = _serviceResponse.Result;
            }
        }

        public void GetOrganizationUserDetails()
        {
            if (View.OrganizationUserID > 0)
            {
                View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(View.OrganizationUserID);
                View.Addresses = StoredProcedureManagers.GetAddressByAddressHandleId(View.OrganizationUserData.AddressHandleID.Value, View.OrganizationUserData.Organization.TenantID.Value);
            }
        }
        public String GetApplicantSSN()
        {
            return SecurityManager.GetFormattedString(View.OrganizationUserData.OrganizationUserID, false);
        }

        public void GetClientContactData()
        {
            if (View.OrganizationUserID > 0)
            {
                View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(View.OrganizationUserID);
                String IpEmailId = View.OrganizationUserData.PrimaryEmailAddress.ToString();
                String IpPhoneNumber = ClientContactManager.GetClientContactByEmail(View.TenantId, IpEmailId).Phone;
                View.OrganizationUserData.PhoneNumber = IpPhoneNumber;
            }
        }
    }
}
