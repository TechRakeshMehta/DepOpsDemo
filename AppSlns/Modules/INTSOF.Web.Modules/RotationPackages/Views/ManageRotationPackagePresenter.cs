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
    public class ManageRotationPackagePresenter : Presenter<IManageRotationPackageView>
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
            GetTenants();
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

            if (View.IsSharedUser || View.SelectedTenantID != 0)
            {
                RequirementPackageDetailsContract searchContract = new RequirementPackageDetailsContract();

                searchContract.RequirementPackageName = View.PackageName.IsNullOrEmpty() ? null : View.PackageName;
                if (View.IsSharedUser)
                {

                    searchContract.LstSelectedTenantIDs = View.LstSelectedTenantIDs.IsNullOrEmpty() ? null :
                                                                        View.LstSelectedTenantIDs;

                    //UAT-1641:-
                    List<Agency> lstAgency = ProfileSharingManager.GetAgencies(AppConsts.NONE, false, true, View.CurrentUserID);
                    searchContract.LstAgencyIDs = String.Join(",", lstAgency.Select(n => n.AG_ID));
                    searchContract.CurrentUserID = View.CurrentUserID;
                }
                else
                {
                    searchContract.SelectedTenantID = View.SelectedTenantID;
                    searchContract.LstAgencyIDs = View.LstSelectedAgencyIDs.IsNullOrEmpty() ? null : View.LstSelectedAgencyIDs;
                    searchContract.SelectedAgencyHierarchyIds = View.LstAgencyHeirarchyIds.IsNullOrEmpty() ? null : String.Join(",", View.LstAgencyHeirarchyIds);
                }
                searchContract.RequirementPkgTypeID = View.SelectedReqPackageTypeID;
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
                searchContract.IsSharedUserLoggedIn = View.IsSharedUser;
                searchContract.CurrentLoggedInUserID = View.OrganisationUserID;
                searchContract.DefinedRequirementID = View.SelectedDefinedRequirementID;

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
            }
            View.LstRequirementPackageDetailsContract = lstRequirementPackageDetailsContract;
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            ServiceResponse<List<TenantDetailContract>> _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            if (View.IsSharedUser)
            {
                //ServiceRequest<Guid> serviceRequestForTenantIDs = new ServiceRequest<Guid>();
                //serviceRequestForTenantIDs.Parameter = View.CurrentUserID;
                //Dictionary<Int32, List<Int32>> responseDictionary = _requirementPackageProxy.GetTenantIDsMappedForAgencyUser(serviceRequestForTenantIDs).Result;
                //List<Int32> mappedTenantIDs = responseDictionary.FirstOrDefault().Value;
                //View.LstTenant = _serviceResponse.Result.Where(cond => mappedTenantIDs.Contains(cond.TenantID)).ToList();
                //View.AgencyId = responseDictionary.FirstOrDefault().Key;
            }
            else
            {
                View.LstTenant = _serviceResponse.Result;
            }

        }

        /// <summary>
        /// Method To get Package Type
        /// </summary>
        public void GetPackageType()
        {
            if (!View.IsSharedUser && View.SelectedTenantID == AppConsts.NONE)
            {
                View.LstRequirementPackageType = new List<RequirementPackageTypeContract>();
            }
            else
            {
                ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
                serviceRequest.Parameter1 = View.SelectedTenantID;
                serviceRequest.Parameter2 = View.IsSharedUser;
                ServiceResponse<List<RequirementPackageTypeContract>> _serviceResponse = _requirementPackageProxy.GetRequirementPackageType(serviceRequest);
                View.LstRequirementPackageType = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            if (View.SelectedTenantID == 0)
                View.LstAgency = new List<AgencyDetailContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                IsAdminLoggedIn();
                if (View.IsAdminLoggedIn)
                {
                    serviceRequest.Parameter = View.SelectedTenantID;
                    ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                    //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
                    View.LstAgency = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
                }
                else if (!View.IsSharedUser)
                {
                    //UAT-1881
                    serviceRequest.SelectedTenantId = View.SelectedTenantID;
                    serviceRequest.Parameter = View.OrganisationUserID;
                    ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                    View.LstAgency = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
                }
            }
        }

        public void GetRequirementPackageDetailsAndSetDataIntoSession(Int32 requirementPackageID)
        {
            ServiceRequest<Int32, Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Int32, Boolean>();
            serviceRequest.Parameter1 = requirementPackageID;
            serviceRequest.Parameter2 = View.SelectedTenantID;
            serviceRequest.Parameter3 = View.IsSharedUser;
            View.RequirementPackageContractSessionData = _requirementPackageProxy.GetRequirementPackageHierarchalDetailsByPackageID(serviceRequest).Result;
        }

        public Int32 DeleteRequirementPackage(Int32 requirementPackageID, Int32 packageObjectTreeID)
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = new RequirementPackageContract()
            {
                RequirementPackageID = requirementPackageID,
                IsDeleted = true,
                PackageObjectTreeID = packageObjectTreeID,
                IsSharedUserLoggedIn = View.IsSharedUser,
                LoggedInUserAgencyID = View.AgencyId,
                CurrentUserId = View.CurrentUserID
            };
            serviceRequest.Parameter2 = View.SelectedTenantID;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserID;

            List<Int32> lstAgencyHierarchyIds = new List<Int32>();
            if (View.IsSharedUser)
            {
                //UAT- 2631 Digestion Process
                ServiceRequest<Int32> serviceRequestNew = new ServiceRequest<Int32>();
                serviceRequestNew.Parameter = requirementPackageID;
                lstAgencyHierarchyIds = _requirementPackageProxy.GetAgencyHierarchyIdsByRequirementPackageID(serviceRequestNew).Result;
            }

            Int32 deletedPackageID = _requirementPackageProxy.DeleteRequirementPackage(serviceRequest).Result;

            if (View.IsSharedUser)
            {
                if (lstAgencyHierarchyIds.Count > AppConsts.NONE)
                {
                    AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstAgencyHierarchyIds), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserID);
                }
            }
            return deletedPackageID;

        }

        /// <summary>
        /// UAT-1784: Get Granular Permissions for current user
        /// </summary>
        public void GetGranularPermissions()
        {
            var _serviceResponse = _clinicalRotationProxy.GetGranularPermissions();
            View.dicGranularPermissions = _serviceResponse.Result;
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
