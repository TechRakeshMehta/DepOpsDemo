#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class ManageRequirementPackagePresenter : Presenter<IManageRequirementPackageView>
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

        public void GetRequirementPackageDetailsAndSetDataIntoSession()
        {
            ServiceRequest<Int32, Int32,Boolean> serviceRequest = new ServiceRequest<Int32, Int32,Boolean>();
            serviceRequest.Parameter1 = View.RequirementPackageID;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            serviceRequest.Parameter3 = View.IsSharedUser;
            RequirementPackageContract requirementPackageContract = _requirementPackageProxy.GetRequirementPackageHierarchalDetailsByPackageID(serviceRequest).Result;
            requirementPackageContract.IsFromRotationScreen = View.IsFromRotationScreen;
            View.RequirementPackageContractSessionData = requirementPackageContract;
        }

        public Int32 DeleteRequirementPackage()
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementPackageContractSessionData;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
            return _requirementPackageProxy.DeleteRequirementPackage(serviceRequest).Result;
        }
    }
}

