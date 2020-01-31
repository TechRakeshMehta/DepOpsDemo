using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public class RotationPackageVersioningPresenter: Presenter<IRotationPackageVersioningPopup>
    {
        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }
        #region [Methods]
        public RequirementPackageContract GetRequirementPackageDetail()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.RequirementPackageID;

            ServiceResponse<RequirementPackageContract> _serviceResponse = _requirementPackageProxy.GetRequirementPackageDataByID(serviceRequest);
            if (_serviceResponse.Result != null)
            {
                return _serviceResponse.Result;
            }

            return new RequirementPackageContract();
        }

        public void SaveRequirementPackageDetail()
        {
            View.RequirementPackage.RequirementPackageCode = Guid.NewGuid();

            ServiceRequest<RequirementPackageContract, Int32, Boolean> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Boolean>();
            serviceRequest.Parameter1 = View.RequirementPackage;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = false; 

            ServiceResponse<Int32> serviceResponse = _requirementPackageProxy.SaveMasterRequirementPackage(serviceRequest);
            View.RequirementPackage.AddedPkgId = serviceResponse.Result;
            if (serviceResponse.Result > 0)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.agencyHierarchyId, AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }
        }
        #endregion
    }
}
