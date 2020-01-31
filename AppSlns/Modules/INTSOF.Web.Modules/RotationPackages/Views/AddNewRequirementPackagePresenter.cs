#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using System;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Collections.Generic;
using INTSOF.Utils;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using Entity.SharedDataEntity;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class AddNewRequirementPackagePresenter : Presenter<IAddNewRequirementPackageView>
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

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        //public void GetAllAgencyInstitutions()
        //{
        //    //UAT-1636: WB: Agency users should not have to assign what schools have access to rotation packages.
        //    //Boolean SortByName = true;
        //    //String clientCode = TenantType.Institution.GetStringValue();
        //    //ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
        //    //serviceRequest.Parameter1 = SortByName;
        //    //serviceRequest.Parameter2 = clientCode;
        //    //ServiceResponse<List<TenantDetailContract>> _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
        //    ServiceRequest<Guid> serviceRequestForTenantIDs = new ServiceRequest<Guid>();
        //    serviceRequestForTenantIDs.Parameter = View.CurrentUserID;
        //    Dictionary<Int32, List<Int32>> responseDictionary = _requirementPackageProxy.GetTenantIDsMappedForAgencyUser(serviceRequestForTenantIDs).Result;
        //    //List<Int32> mappedTenantIDs = responseDictionary.FirstOrDefault().Value;
        //    //View.LstTenant = _serviceResponse.Result.Where(cond => mappedTenantIDs.Contains(cond.TenantID)).ToList();
        //    View.AgencyId = responseDictionary.FirstOrDefault().Key;
        //}

        /// <summary>
        /// UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
        /// </summary>
        public void GetReqPkgType()
        {
            if (!View.IsSharedUser && View.SelectedTenantId == AppConsts.NONE)
            {
                View.LstRequirementPackageType = new List<RequirementPackageTypeContract>();
            }
            else
            {
                ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = View.IsSharedUser;
                ServiceResponse<List<RequirementPackageTypeContract>> _serviceResponse = _requirementPackageProxy.GetRequirementPackageType(serviceRequest);
                View.LstRequirementPackageType = _serviceResponse.Result;
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



