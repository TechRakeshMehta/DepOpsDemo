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
    public class ClientContactProfilePresenter : Presenter<IClientContactProfileView>
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

        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);
        }

        public void GetUserData()
        {
            var _serviceResponse = _clientContactProxy.GetUserData();
            View.OrganizationUser = _serviceResponse.Result;
        }

        public void UpdateClientContactOrganisationUser()
        {
            ServiceRequest<OrganizationUserContract, Int32> serviceRequest = new ServiceRequest<OrganizationUserContract, Int32>();
            serviceRequest.Parameter1 = View.OrganizationUser;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            var _serviceResponse = _clientContactProxy.UpdateClientContactOrganisationUser(serviceRequest);
            View.SuccessMsg = _serviceResponse.Result;

        }

        public void GetClientContactRotationDocuments()
        {
            if (View.SelectedTenantID == AppConsts.NONE)
            {
                View.RotationDocumentList = new List<ClientContactSyllabusDocumentContract>();
            }
            else
            {
                if (View.ClientContactID != AppConsts.NONE)
                {
                    ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                    serviceRequest.Parameter1 = View.ClientContactID;
                    serviceRequest.Parameter2 = View.SelectedTenantID;
                    var _serviceResponse = _clientContactProxy.GetClientContactRotationDocuments(serviceRequest);
                    View.RotationDocumentList = _serviceResponse.Result;
                    //View.UploadedDocumentList = ClientContactManager.GetClientDocuments(View.ClientContactID);
                }
            }
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
    }
}
