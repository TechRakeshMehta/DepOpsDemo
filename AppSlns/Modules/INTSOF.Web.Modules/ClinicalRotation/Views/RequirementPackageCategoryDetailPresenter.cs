using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementPackageCategoryDetailPresenter : Presenter<IRequirementPackageCategoryDetail>
    {

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public void GetRotationPackageCategoryDetail()
        {
            ServiceRequest<Int32, Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Int32, Boolean>();
            serviceRequest.Parameter1 = View.TenantID;
            serviceRequest.Parameter2 = View.RotationID;
            serviceRequest.Parameter3 = View.IsStudentPackage;
            ServiceResponse<List<RequirementCategoryContract>> _serviceResponse = _requirementPackageProxy.GetRotationPackageCategoryDetail(serviceRequest);
            View.CategoryData = _serviceResponse.Result;
        }
    }
}
