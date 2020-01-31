#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;


#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class ManageRotationFieldPresenter : Presenter<IManageRotationFieldView>
    {
        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetRotationFieldDataTypes()
        {
            ServiceRequest<Int32,Boolean> serviceRequest = new ServiceRequest<Int32,Boolean>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = View.IsSharedUser;
            ServiceResponse<List<RotationFieldDataTypeContract>> _serviceResponse = _requirementPackageProxy.GetRotationFieldDataTypes(serviceRequest);
            View.LstRotationFieldDataType = _serviceResponse.Result;
        }
    }
}



