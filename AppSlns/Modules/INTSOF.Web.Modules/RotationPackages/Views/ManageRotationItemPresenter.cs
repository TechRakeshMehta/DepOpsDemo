#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;


#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.RequirementPackage;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class ManageRotationItemPresenter : Presenter<IManageRotationItemView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public void GetRulesConstantTypes()
        {
            ServiceRequest<Int32,Boolean> serviceRequest = new ServiceRequest<Int32,Boolean>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = View.IsSharedUser;
            ServiceResponse<List<RulesConstantTypeContract>> _serviceResponse = _requirementPackageProxy.GetRulesConstantTypes(serviceRequest);
            View.LstRulesConstantTypeContract = _serviceResponse.Result;
        }
    }
}


