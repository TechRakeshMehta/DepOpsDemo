using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
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
    public class RotationPackageCopyPresenter : Presenter<IRotationPackageCopy>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetAllAgency();
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
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

        private void GetAllAgency()
        {
            //ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            //ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgenciesFromAllTenants();
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.TenantId;
            ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgencies(serviceRequest);

            View.LstAgencyDetailContract = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
        }

        public void SaveRequirementPackageDetail()
        {
            View.RequirementPackage.RequirementPackageCode = Guid.NewGuid();

            ServiceRequest<RequirementPackageContract, Int32,Boolean> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32,Boolean>();
            serviceRequest.Parameter1 = View.RequirementPackage;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = View.IsRotationPkgCopyFromAgencyHierarchy; //UAT-3494

            ServiceResponse<Int32> serviceResponse = _requirementPackageProxy.SaveMasterRequirementPackage(serviceRequest);
            if (serviceResponse.Result > 0)
            {
                AgencyHierarchyManager.CallDigestionProcess(String.Join(",", View.RequirementPackage.LstAgencyHierarchyIDs), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }
        }

        #endregion
    }
}
