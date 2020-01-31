using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.RotationPackages.Views
{
    public class ManageMasterRotationPackagesPresenter : Presenter<IManageMasterRotationPackagesView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }


        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }
        public void GetPackageDetails()
        {
            List<RequirementPackageDetailsContract> lstRequirementPackageDetailsContract = new List<RequirementPackageDetailsContract>();


            RequirementPackageDetailsContract searchContract = new RequirementPackageDetailsContract();

            searchContract.RequirementPackageName = View.PackageName.IsNullOrEmpty() ? null : View.PackageName;
            searchContract.LstAgencyIDs = View.LstSelectedAgencyIDs.IsNullOrEmpty() ? null : View.LstSelectedAgencyIDs;
            searchContract.RequirementPkgTypeID = View.SelectedReqPackageTypeID;
            searchContract.DefinedRequirementID = View.SelectedDefinedRequirementID;

            if (View.RequirementPackageStatus == RequirementPackageActiveStatus.ACTIVE.GetStringValue())
            {
                searchContract.IsActivePackage = true;
            }
            else if (View.RequirementPackageStatus == RequirementPackageActiveStatus.INACTIVE.GetStringValue())
            {
                searchContract.IsActivePackage = false;
            }
            else
            {
                searchContract.IsActivePackage = null;
            }
            searchContract.CurrentLoggedInUserID = View.CurrentLoggedInUserID;
            searchContract.IsSharedUserLoggedIn = true;
            searchContract.ShowAllAgencies = true;

            ServiceRequest<RequirementPackageDetailsContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<RequirementPackageDetailsContract, CustomPagingArgsContract>();
            serviceRequest.Parameter1 = searchContract;
            serviceRequest.Parameter2 = View.GridCustomPaging;
            lstRequirementPackageDetailsContract = _requirementPackageProxy.GetRequirementPackageDetails(serviceRequest).Result;
            if (!lstRequirementPackageDetailsContract.IsNullOrEmpty())
            {
                if (lstRequirementPackageDetailsContract[0].TotalCount > 0)
                {
                    View.VirtualRecordCount = lstRequirementPackageDetailsContract[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }
            View.LstRequirementPackageDetailsContract = lstRequirementPackageDetailsContract;
        }

        /// <summary>
        /// Method To get Package Type
        /// </summary>
        public void GetPackageType()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = 0;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<RequirementPackageTypeContract>> _serviceResponse = _requirementPackageProxy.GetRequirementPackageType(serviceRequest);
            View.LstRequirementPackageType = _serviceResponse.Result;
        }


        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            //UAT-1881
            if (IsAdminLoggedIn())
            {
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgenciesFromAllTenants();
                //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
                View.LstAgency = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter = View.CurrentLoggedInUserID;
                var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                View.LstAgency = _serviceResponse.Result;
            }
        }

        public Int32 DeleteRequirementPackage(Int32 requirementPackageID, Int32 packageObjectTreeID)
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = new RequirementPackageContract()
            {
                RequirementPackageID = requirementPackageID,
                IsDeleted = true,
                PackageObjectTreeID = packageObjectTreeID,
                IsSharedUserLoggedIn = true,
                LoggedInUserAgencyID = View.AgencyId,
                CurrentUserId = View.CurrentUserID,
                IsManageMasterPackage = true
            };
            serviceRequest.Parameter2 = 0;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserID;
            
            //UAT- 2631 Digestion Process
            ServiceRequest<Int32> serviceRequestNew = new ServiceRequest<Int32>();
            serviceRequestNew.Parameter = requirementPackageID; 
            List<Int32> lstAgencyHierarchyIds = _requirementPackageProxy.GetAgencyHierarchyIdsByRequirementPackageID(serviceRequestNew).Result;

            Int32 deletedPackageID = _requirementPackageProxy.DeleteRequirementPackage(serviceRequest).Result;
            if (deletedPackageID > AppConsts.NONE)
            { 
                if (lstAgencyHierarchyIds.Count > AppConsts.NONE)
                {
                    AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstAgencyHierarchyIds), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserID);
                }
            }
            return deletedPackageID;
        }

        public Boolean IsRequirementPackageUsed(Int32 packageID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = packageID;
            return _requirementPackageProxy.IsRequirementPackageUsed(serviceRequest).Result;
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public void GetRequirementPackageDetailsAndSetDataIntoSession(Int32 requirementPackageID)
        {
            ServiceRequest<Int32, Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Int32, Boolean>();
            serviceRequest.Parameter1 = requirementPackageID;
            serviceRequest.Parameter2 = AppConsts.NONE;
            serviceRequest.Parameter3 = true;
            View.RequirementPackageContractSessionData = _requirementPackageProxy.GetRequirementPackageHierarchalDetailsByPackageID(serviceRequest).Result;
        }

        public void GetDefinedRequirement()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<DefinedRequirementContract>> _serviceResponse = _requirementPackageProxy.GetDefinedRequirement(serviceRequest);
            View.lstDefinedRequirement = _serviceResponse.Result;
        }
    }
}
