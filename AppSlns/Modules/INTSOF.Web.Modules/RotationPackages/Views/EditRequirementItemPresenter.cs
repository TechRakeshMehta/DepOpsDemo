#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;


#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using System;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using System.Collections.Generic;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class EditRequirementItemPresenter : Presenter<IEditRequirementItemView>
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

        public Int32 SaveRequirementPackageData()
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementPackageContractSessionData;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
            ServiceResponse<Int32> addedPackageID = _requirementPackageProxy.SaveRequirementPackage(serviceRequest);
            return addedPackageID.Result;
        }

        public void GetRulesConstantTypes()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = View.IsSharedUser;
            ServiceResponse<List<RulesConstantTypeContract>> _serviceResponse = _requirementPackageProxy.GetRulesConstantTypes(serviceRequest);
            View.LstRulesConstantTypeContract = _serviceResponse.Result;
        }
    }
}



