#region Namespaces

#region SystemDefined


using System.Linq;
using System;

#endregion

#region UserDefined

using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.ServiceProxy.Modules.RequirementPackage;

using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using System.Collections.Generic;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class CopyRequirementPackageToMasterPresenter : Presenter<ICopyRequirementPackageToMasterView>
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
                serviceRequest.Parameter = View.SelectedTenantId;
                ServiceResponse<List<AgencyDetailContract>> serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
                View.LstAgencyDetailContract = serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
            else if (!View.IsSharedUser)
            {
                //UAT-1881
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter = View.CurrentLoggedInUserId;
                ServiceResponse<List<AgencyDetailContract>> serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                View.LstAgencyDetailContract = serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
        }

        public Int32 SaveRequirementPackageData()
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementPackageContractSessionData;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
            ServiceResponse<Int32> addedPackageID = _requirementPackageProxy.CopyClientRqrmntPkgToShared(serviceRequest);

            if (addedPackageID.Result > AppConsts.NONE)
            {
                AgencyHierarchyManager.CallDigestionProcess(String.Join(",", View.RequirementPackageContractSessionData.LstAgencyHierarchyIDs), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);

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




