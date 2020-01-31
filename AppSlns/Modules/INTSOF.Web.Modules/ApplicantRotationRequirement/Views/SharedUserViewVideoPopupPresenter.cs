using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.SharedObjects;
using INTSOF.ServiceDataContracts.Core;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public class SharedUserViewVideoPopupPresenter : Presenter<ISharedUserViewVideoPopupView>
    {
        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        public void GetRfVideoData()
        {
            if (View.CurrentViewContext.RequrmntFieldVideoID > 0 && View.CurrentViewContext.TenantID > 0)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.CurrentViewContext.RequrmntFieldVideoID;
                serviceRequest.SelectedTenantId = View.CurrentViewContext.TenantID;
                var _serviceResponse = _applicantClinicalRotationProxy.GetRequirementFieldVideoData(serviceRequest);
                View.RequrmntVideoData = _serviceResponse.Result;
            }
        }

        public void GetObjectAttrProperties()
        {
            if (View.CurrentViewContext.RequrmntObjTreeID > 0 && View.CurrentViewContext.TenantID > 0)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<int>();
                serviceRequest.Parameter = View.CurrentViewContext.RequrmntObjTreeID;
                serviceRequest.SelectedTenantId = View.CurrentViewContext.TenantID;
                var _serviceResponse = _applicantClinicalRotationProxy.GetObjectTreeProperties(serviceRequest);
                View.ObjectAttrContract = _serviceResponse.Result;
            }
        }
    }
}
