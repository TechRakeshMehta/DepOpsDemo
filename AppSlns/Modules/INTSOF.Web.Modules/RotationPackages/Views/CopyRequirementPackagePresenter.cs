using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public class CopyRequirementPackagePresenter : Presenter<ICopyRequirementPackage>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {

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

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            if (IsAdminLoggedIn())
            {
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgenciesFromAllTenants();
                View.LstAgencyDetailContract = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
        }

        public Int32 SaveRequirementPackageData()
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementPackageContractSessionData;
            //serviceRequest.Parameter2 = View.SelectedTenantId;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
            ServiceResponse<Int32> addedPackageID = _requirementPackageProxy.CopyClientRqrmntPkgToShared(serviceRequest);
            
            //UAT- 2631 Digestion Process
            if (addedPackageID.Result > INTSOF.Utils.AppConsts.NONE)
            {
                AgencyHierarchyManager.CallDigestionProcess(String.Join(",", View.RequirementPackageContractSessionData.LstAgencyHierarchyIDs), INTSOF.Utils.AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }
            return addedPackageID.Result;
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantID);
        }
    }
}
