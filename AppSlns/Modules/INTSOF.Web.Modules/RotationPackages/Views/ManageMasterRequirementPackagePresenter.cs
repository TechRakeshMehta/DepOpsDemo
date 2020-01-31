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
    public class ManageMasterRequirementPackagePresenter : Presenter<IManageMasterRequirementPackageView>
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

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgencies()
        {
            //UAT-1881
            if (IsAdminLoggedIn())
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.TenantId;
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgencies(serviceRequest);
                //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
                View.lstAgency = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter = View.CurrentLoggedInUserId;
                var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                View.lstAgency = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantId);
        }

        /// <summary>
        /// Method To get Package Type
        /// </summary>
        public void GetPackageTypes()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = 0;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<RequirementPackageTypeContract>> _serviceResponse = _requirementPackageProxy.GetRequirementPackageType(serviceRequest);
            View.lstRequirementPackageType = _serviceResponse.Result;
        }
        public void GetRequirementCategories()
        {
            ServiceResponse<List<RequirementCategoryContract>> _serviceResponse = _requirementPackageProxy.GetRequirementCategories();
            View.lstCategory = _serviceResponse.Result;
        }
        public bool SaveRequirementPackageDetail()
        {

            RequirementPackageContract reqPackageContarct = new RequirementPackageContract();
            View.RequirementPackage.RequirementPackageCode = Guid.NewGuid();

            ServiceRequest<RequirementPackageContract, Int32,Boolean> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32,Boolean>();
            serviceRequest.Parameter1 = View.RequirementPackage;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = false;
            ServiceResponse<int> serviceResponse = _requirementPackageProxy.SaveMasterRequirementPackage(serviceRequest);
            if (serviceResponse.Result > 0)
            {
                List<Int32> lstAgencyHierarchyIds = View.RequirementPackage.LstAgencyHierarchyIDs;
                if (!View.lstPrevAgencyHierarchyIds.IsNullOrEmpty())
                    lstAgencyHierarchyIds.AddRange(View.lstPrevAgencyHierarchyIds);
                AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstAgencyHierarchyIds.Distinct()), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }
            return serviceResponse.Result > 0 ? true : false;

        }
        public void GetRequiremetPackages()
        {
            List<RequirementPackageContract> lstRequirementPackageDetails = new List<RequirementPackageContract>();

            RequirementPackageContract searchContract = new RequirementPackageContract();
            searchContract.RequirementPackageName = View.PackageName.IsNullOrEmpty() ? null : View.PackageName;
            searchContract.lstSelectedAgencyIds = View.lstSelectedAgencyIds.IsNullOrEmpty() ? null : View.lstSelectedAgencyIds;
            searchContract.RequirementPkgTypeID = View.SelectedReqPackageTypeID.IsNullOrEmpty() ? AppConsts.NONE : View.SelectedReqPackageTypeID;
            searchContract.RequirementPackageLabel = View.PackageLabel.IsNullOrEmpty() ? null : View.PackageLabel;
            searchContract.EffectiveStartDate = View.EffectiveStartDate.IsNullOrEmpty() ? (DateTime?)null : View.EffectiveStartDate.Value;
            searchContract.EffectiveEndDate = View.EffectiveEndDate.IsNullOrEmpty() ? (DateTime?)null : View.EffectiveEndDate.Value;
            searchContract.PackageCreatedDate = View.PackageCreatedDate.IsNullOrEmpty() ? (DateTime?)null : View.PackageCreatedDate.Value;
            searchContract.RotationEndDate = View.RotationEndDate.IsNullOrEmpty() ? (DateTime?)null : View.RotationEndDate.Value;

            if (View.PackageOptions == 2) { searchContract.PackageOptions = null; }
            else { searchContract.PackageOptions = View.PackageOptions.IsNullOrEmpty() ? 0 : View.PackageOptions; }


            ServiceRequest<RequirementPackageContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<RequirementPackageContract, CustomPagingArgsContract>();
            serviceRequest.Parameter1 = searchContract;
            serviceRequest.Parameter2 = View.GridCustomPaging;
            lstRequirementPackageDetails = _requirementPackageProxy.GetMasterRequirementPackageDetails(serviceRequest).Result;
            if (!lstRequirementPackageDetails.IsNullOrEmpty())
            {
                if (lstRequirementPackageDetails[0].TotalCount > 0)
                {
                    View.VirtualRecordCount = lstRequirementPackageDetails[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }
            View.lstRequirementPackage = lstRequirementPackageDetails;
        }

        public void GetDefinedRequirements()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<DefinedRequirementContract>> _serviceResponse = _requirementPackageProxy.GetDefinedRequirement(serviceRequest);
            View.lstDefinedRequirement = _serviceResponse.Result;
        }

        public void GetRequirementReviewBy()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<RequirementReviewByContract>> _serviceResponse = _requirementPackageProxy.GetRequirementReviewBy(serviceRequest);
            View.lstRequirementReviewBy = _serviceResponse.Result;
        }

        public bool ArchivePackage()
        {
            ServiceRequest<Dictionary<Int32, Boolean>, Int32> serviceRequest = new ServiceRequest<Dictionary<Int32, Boolean>, Int32>();
            serviceRequest.Parameter1 = View.lstSelectedRotPackage;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            var _serviceResponse = _requirementPackageProxy.ArchivePackage(serviceRequest);
            return _serviceResponse.Result;
        }

        //UAT-4054
        public bool UnArchivePackage()
        {
            ServiceRequest<Dictionary<Int32, Boolean>, Int32> serviceRequest = new ServiceRequest<Dictionary<Int32, Boolean>, Int32>();
            serviceRequest.Parameter1 = View.lstSelectedRotPackage;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            var _serviceResponse = _requirementPackageProxy.UnArchivePackage(serviceRequest);
            return _serviceResponse.Result;
        }


        public Int32 DeleteRequirementPackage(Int32 requirementPackageID)
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = new RequirementPackageContract()
            {
                RequirementPackageID = requirementPackageID,
                IsDeleted = true,
                PackageObjectTreeID = AppConsts.NONE,
                IsSharedUserLoggedIn = true,
                LoggedInUserAgencyID = AppConsts.NONE,
                CurrentUserId = new Guid(),
                IsManageMasterPackage = true,
                IsNewPackage = true
            };
            serviceRequest.Parameter2 = 0;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
            ServiceResponse<Int32> serviceResponse = _requirementPackageProxy.DeleteRequirementPackage(serviceRequest);
            if (serviceResponse.Result > 0)
            {
                ServiceRequest<Int32> serviceRequest2 = new ServiceRequest<Int32>();
                serviceRequest2.Parameter = requirementPackageID;
                var AgencyHierarchyIds = _requirementPackageProxy.GetAgencyHierarchyIdsByRequirementPackageID(serviceRequest2);
                if (AgencyHierarchyIds.Result.Count > 0)
                {
                    AgencyHierarchyManager.CallDigestionProcess(String.Join(",", AgencyHierarchyIds.Result), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
                }
            }

            return serviceResponse.Result;
        }

        #region UAT-4657
        public String IsPackageVersionInProgress(Int32 PkgId)
        {
            return AgencyHierarchyManager.IsPackageVersionInProgress(PkgId);
        }

        public Boolean IsRequirementSyncAlreadyInProgress(Int32 PkgId)
        {
            return SharedRequirementPackageManager.IsSyncAlreadyInProgress(PkgId, true);
        }
        #endregion
    }
}
