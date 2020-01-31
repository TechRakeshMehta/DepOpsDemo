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
using System.Text;


#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class SetupRequirementPackagePresenter : Presenter<ISetupRequirementPackageView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetRequirementPackageData();
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
            if (IsAdminLoggedIn())
            {
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgenciesFromAllTenants();
                //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
                View.LstAgencyDetailContract = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter = View.CurrentLoggedInUserId;
                var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                View.LstAgencyDetailContract = _serviceResponse.Result;
            }
        }

        public void GetRequirementPackageData()
        {
            if (View.SelectedPackageID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedPackageID;
                ServiceResponse<RequirementPackageContract> _serviceResponse = _requirementPackageProxy.GetRequirementPackageDataByID(serviceRequest);
                View.RequirementPackageContract = _serviceResponse.Result;
            }
        }

        public Int32 SaveRequirementPackageData()
        {
            ServiceResponse<Int32> addedPackageID = new ServiceResponse<Int32>();
            if (View.RequirementPackageContract.RequirementPkgTypes != null && View.RequirementPackageContract.RequirementPkgTypes.Count > 1)
            {
                foreach (var RequirementTypes in View.RequirementPackageContract.RequirementPkgTypes)
                {
                    View.RequirementPackageContract.RequirementPkgTypeID = RequirementTypes.Key;
                    View.RequirementPackageContract.RequirementPkgTypeCode = RequirementTypes.Value;
                    View.RequirementPackageContract.RequirementPackageCode = Guid.NewGuid();
                    ServiceRequest<RequirementPackageContract, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32>();
                    serviceRequest.Parameter1 = View.RequirementPackageContract;
                    serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
                    ServiceResponse<Int32> newPackageID = _requirementPackageProxy.SaveRequirementPackageData(serviceRequest);
                    if (RequirementTypes.Value == RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue())
                    {
                        addedPackageID = newPackageID;
                    }
                }
            }
            else
            {
                ServiceRequest<RequirementPackageContract, Int32> serviceRequest = new ServiceRequest<RequirementPackageContract, Int32>();
                View.RequirementPackageContract.RequirementPkgTypeID = View.RequirementPackageContract.RequirementPkgTypes.FirstOrDefault().Key;
                View.RequirementPackageContract.RequirementPackageCode = Guid.NewGuid();
                View.RequirementPackageContract.RequirementPkgTypeCode = View.RequirementPackageContract.RequirementPkgTypes.FirstOrDefault().Value;
                serviceRequest.Parameter1 = View.RequirementPackageContract;
                serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
                addedPackageID = _requirementPackageProxy.SaveRequirementPackageData(serviceRequest);
            }

            //UAT- 2631 Digestion Process
            if (addedPackageID.Result > AppConsts.NONE)
            {
                List<Int32> lstAgencyHierarchyIds = View.RequirementPackageContract.LstAgencyHierarchyIDs;
                if (!View.lstPrevAgencyHierarchyIds.IsNullOrEmpty())
                    lstAgencyHierarchyIds.AddRange(View.lstPrevAgencyHierarchyIds);
                AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstAgencyHierarchyIds.Distinct()), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }

            return addedPackageID.Result;
        }

        public Int32 CreateMasterPackageVersion()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.SelectedPackageID;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            ServiceResponse<Int32> addedPackageID = _requirementPackageProxy.CreateMasterPackageVersion(serviceRequest);
            return addedPackageID.Result;
        }

        public void GetReqPkgType()
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = true;
            ServiceResponse<List<RequirementPackageTypeContract>> _serviceResponse = _requirementPackageProxy.GetRequirementPackageType(serviceRequest);
            View.LstRequirementPackageType = _serviceResponse.Result;
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
            return (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public String CheckPackageCompletionStatus()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedPackageID;
            RequirementPackageCompletionContract completionStatus = _requirementPackageProxy.CheckRequirementPackageCompletionStatus(serviceRequest).Result;


            StringBuilder responseText = new StringBuilder();

            //check 1 : Package has at least a requirement group
            if (completionStatus.IsPackageWithoutCategory)
            {
                responseText.Append("<b>Package does not contain any Requirement Group.</b><div> &nbsp;</div>");
            }

            //check 2: Each requirement group has at least a requirement.
            if (!completionStatus.IncompleteCategoryNames.IsNullOrEmpty())
            {
                responseText.Append("<b>Requirement Group(s) without Requirement: </b>");
                responseText.Append(String.Join(", ", completionStatus.IncompleteCategoryNames));
                responseText.Append("<div> &nbsp;</div>");
            }

            //check 3: Each requirement has at least a field.
            if (!completionStatus.IncompleteItemNames.IsNullOrEmpty())
            {
                responseText.Append("<b>Requirement(s) without Field: </b> ");
                responseText.Append(String.Join(", ", completionStatus.IncompleteItemNames));
                responseText.Append("<div> &nbsp;</div>");
            }

            //check 4: Rules are defined for Package
            if (completionStatus.IsPackageRuleInComplete)
            {
                responseText.Append("<b>Package does not contain any Rule.</b><div> &nbsp;</div>");
            }

            //check 5: Rules are defined for each requirement Group
            if (!completionStatus.CategoriesWithoutRule.IsNullOrEmpty())
            {
                responseText.Append("<b>Requirement Group(s) missing Rule: </b>");
                responseText.Append(String.Join(", ", completionStatus.CategoriesWithoutRule));
                responseText.Append("<div> &nbsp;</div>");
            }

            //check 6: Rules are defined for each Requirement.
            if (!completionStatus.ItemsWithoutRule.IsNullOrEmpty())
            {
                responseText.Append("<b>Requirement(s) missing Rule: </b>");
                responseText.Append(String.Join(", ", completionStatus.ItemsWithoutRule));
                responseText.Append("<div> &nbsp;</div>");
            }

            //check 7: 	Rules are defined for each field.
            if (!completionStatus.FieldsWithoutRule.IsNullOrEmpty())
            {
                responseText.Append("<b>Field(s) missing Rule: </b>");
                responseText.Append(String.Join(", ", completionStatus.FieldsWithoutRule));
                responseText.Append("<div> &nbsp;</div>");
            }
            String responseString = responseText.ToString();
            if (!responseString.IsNullOrEmpty())
            {
                responseString = "<h4>Selected Requirement Package is Incomplete. Please refer below incomplete entities:</h4><div> &nbsp;</div>" + responseString;
            }

            return responseString;
        }
    }
}




