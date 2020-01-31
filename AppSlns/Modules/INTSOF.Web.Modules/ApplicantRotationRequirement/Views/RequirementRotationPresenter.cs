using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using Business.RepoManagers;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public class RequirementRotationPresenter : Presenter<IRequirementRotation>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ClientContactProxy _clientContactProxy
        {
            get
            {
                return new ClientContactProxy();
            }
        }

        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        /// <summary>
        /// Get's the List of Agencies associated with the current Applicants' Tenant
        /// </summary>
        public void GetAgencies()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.TenantId;

            var _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
            View.lstAgency = _serviceResponse.Result;
        }

        /// <summary>
        /// Get's the ClientContacts(Instructor/Preceptors) associated with the current Applicants' Tenant
        /// </summary>
        public void GetClientContacts()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.TenantId;
            var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
            View.lstClientContacts = _serviceResponse.Result;
        }

        public void GetClinicalRotations()
        {
            //View.SearchContract.TenantID = View.TenantId;
            var serviceRequest = new ServiceRequest<ClinicalRotationDetailContract>();
            serviceRequest.Parameter = View.SearchContract;
            serviceRequest.SelectedTenantId = View.TenantId;
            var _serviceResponse = _applicantClinicalRotationProxy.GetApplicantRotaions(serviceRequest);
            View.lstApplicantRotations = _serviceResponse.Result;
        }
    }
}
