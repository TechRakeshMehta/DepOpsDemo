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
using Entity.SharedDataEntity;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class EditRequirementPackagePresenter : Presenter<IEditRequirementPackageView>
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
            //UAT-1881
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            if (IsAdminLoggedIn())
            {
                serviceRequest.Parameter = View.SelectedTenantId;
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
                View.LstAgencyDetailContract = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
            else if (!View.IsSharedUser)
            {
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter = View.OrganisationUserID;
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                View.LstAgencyDetailContract = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
        }

        public void GetAllAgenciesForAgencyUser()
        {
            List<Agency> lstAgency = ProfileSharingManager.GetAgencies(AppConsts.NONE, false, true, View.CurrentUserID);
            View.LstAgencyDetailContract = lstAgency.Select(n => new AgencyDetailContract
            {
                AgencyID = n.AG_ID,
                AgencyName = n.AG_Name,
                AgencyDescription = n.AG_Description
            }).ToList();
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        //public void GetAllAgencyInstitutions()
        //{
        //    Boolean SortByName = true;
        //    String clientCode = TenantType.Institution.GetStringValue();
        //    ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
        //    serviceRequest.Parameter1 = SortByName;
        //    serviceRequest.Parameter2 = clientCode;
        //    ServiceResponse<List<TenantDetailContract>> _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
        //    ServiceRequest<Guid> serviceRequestForTenantIDs = new ServiceRequest<Guid>();
        //    serviceRequestForTenantIDs.Parameter = View.CurrentUserID;
        //    Dictionary<Int32, List<Int32>> responseDictionary = _requirementPackageProxy.GetTenantIDsMappedForAgencyUser(serviceRequestForTenantIDs).Result;
        //    List<Int32> mappedTenantIDs = responseDictionary.FirstOrDefault().Value;
        //    View.LstTenant = _serviceResponse.Result.Where(cond => mappedTenantIDs.Contains(cond.TenantID)).ToList();
        //}

        public Int32 SaveRequirementPackageData()
        {
            ServiceRequest<RequirementPackageContract, Int32, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementPackageContractSessionData;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserId;
            ServiceResponse<Int32> addedPackageID = _requirementPackageProxy.SaveRequirementPackage(serviceRequest);
            if (View.RequirementPackageContractSessionData.IsSharedUserLoggedIn)
            {
                List<Int32> lstAgencyHierarchyIds = View.RequirementPackageContractSessionData.LstAgencyHierarchyIDs;
                if (!View.lstPrevAgencyHierarchyIds.IsNullOrEmpty())
                    lstAgencyHierarchyIds.AddRange(View.lstPrevAgencyHierarchyIds);
                AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstAgencyHierarchyIds.Distinct()), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }
            return addedPackageID.Result;
        }

        public void GetDefinedRequirement()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<DefinedRequirementContract>> _serviceResponse = _requirementPackageProxy.GetDefinedRequirement(serviceRequest);
            View.lstDefinedRequirement = _serviceResponse.Result;
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



