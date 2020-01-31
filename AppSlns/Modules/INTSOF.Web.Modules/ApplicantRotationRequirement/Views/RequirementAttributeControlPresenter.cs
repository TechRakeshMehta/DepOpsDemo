using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.SharedObjects;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public class RequirementAttributeControlPresenter:Presenter<IRequirementAttributeControlView>
    {
        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        public void GetDocuments()
        {
            if (View.CurrentLoggedInUserId > 0 && View.TenantId > 0)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.CurrentLoggedInUserId;
                serviceRequest.SelectedTenantId = View.TenantId;
                var _serviceResponse = _applicantClinicalRotationProxy.GetApplicantDocument(serviceRequest);
                View.CurrentViewContext.ApplicantDocuments = _serviceResponse.Result;
            }
            else
            {
                View.ApplicantDocuments = new List<ApplicantDocumentContract>();
            }
        }
    }
}
