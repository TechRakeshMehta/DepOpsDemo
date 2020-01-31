using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using System.Linq;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementVerificationQueuePresenter : Presenter<IRequirementVerificationQueueView>
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

        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }
        /// <summary>
        /// Method To get all tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            View.lstTenant = _serviceResponse.Result;
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            if (!View.IsSearchClicked)
            {
                View.ApplicantSearchData = new List<RequirementVerificationQueueContract>();
                return;
            }

            if (View.SelectedTenantId == 0)
            {
                View.ApplicantSearchData = new List<RequirementVerificationQueueContract>();
            }
            else
            {
                RequirementVerificationQueueContract searchDataContract = new RequirementVerificationQueueContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                searchDataContract.RotationStartDate = View.RotationStartDate;
                searchDataContract.RotationEndDate = View.RotationEndDate;
                searchDataContract.SubmissionDate = View.SubmissionDate;
                searchDataContract.IsCurrentRotation = View.IsCurrentRotation;

                ////UAT-4014
                searchDataContract.SelectedUserTypeCode = View.SelectedUserTypeIds;

                //UAT-4705
                searchDataContract.ReqCategoryLabel = View.CategoryId.IsNullOrEmpty() ? null : View.CategoryId;
                searchDataContract.ReqItemLabel = View.ReqItemId.IsNullOrEmpty() ? null : View.ReqItemId;
                //if (View.RequirementPackageTypeID > SysXDBConsts.NONE)
                //    searchDataContract.RequirementPackageTypeID = View.RequirementPackageTypeID;
                //UAT-2197:Requirement Verification Queue: "Requirement Package Type" should not be required
                searchDataContract.SelectedRequirementPackageTypes = View.RequirementPackageTypes;
                if (View.SelectedAgencyID > SysXDBConsts.NONE)
                    searchDataContract.AgencyID = View.SelectedAgencyID;
                if (View.TenantId != SecurityManager.DefaultTenantID)
                    searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                searchDataContract.IsRotationPackageVerificationQueue = View.IsRotationPackageVerificationQueue;
                if (!View.SelectedPackageID.IsNullOrEmpty())
                    searchDataContract.RequirementPackageID = View.SelectedPackageID;
                try
                {
                    ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract> serviceRequest =
                            new ServiceRequest<RequirementVerificationQueueContract, CustomPagingArgsContract>();
                    serviceRequest.SelectedTenantId = View.SelectedTenantId;
                    serviceRequest.Parameter1 = searchDataContract;
                    serviceRequest.Parameter2 = View.GridCustomPaging;
                    var _serviceResponse = _clinicalRotationProxy.GetRequirementVerificationQueueSearch(serviceRequest);
                    View.ApplicantSearchData = _serviceResponse.Result;

                    if (View.ApplicantSearchData.IsNotNull() && View.ApplicantSearchData.Count > 0)
                    {
                        if (View.ApplicantSearchData[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantSearchData[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = null;
                    throw e;
                }
            }
        }

        /// <summary>
        /// Method to get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            if (View.SelectedTenantId == 0)
                View.lstAgency = new List<AgencyDetailContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                //UAT-1881
                IsAdminLoggedIn();
                if (View.IsAdminLoggedIn)
                {
                    serviceRequest.Parameter = View.SelectedTenantId;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
                else
                {
                    serviceRequest.SelectedTenantId = View.SelectedTenantId;
                    serviceRequest.Parameter = View.CurrentLoggedInUserId;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
            }
        }

        /// <summary>
        /// Method to get requirement package types
        /// </summary>
        public void GetRequirementPackageTypes()
        {
            if (View.SelectedTenantId == 0)
                View.lstRequirementPackageType = new List<RequirementPackageTypeContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantId;

                var _serviceResponse = _clinicalRotationProxy.GetRequirementPackageTypes(serviceRequest);
                View.lstRequirementPackageType = _serviceResponse.Result;
            }
        }

        public void GetRequirementPackages()
        {
            if (View.SelectedTenantId == 0)
                View.LstRequirementPackage = new List<RequirementPackageContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantId;

                var _serviceResponse = _requirementPackageProxy.GetAllRequirementPackages(serviceRequest);
                View.LstRequirementPackage = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        /// <summary>
        /// To get User Types: Applicant / Instr-Preceptor -UAT-4014
        /// </summary>
        public void GetUserType()
        {
            if (View.SelectedTenantId == 0)
                View.dicUserTypes = new Dictionary<String, String>();
            else
            {
                Dictionary<String, String> dicUserTypes = new Dictionary<String, String>();
                dicUserTypes.Add("AAAC", UserTypeSwitchView.Applicant.ToString());
                dicUserTypes.Add("AAAE", "Instructor/Preceptor");
                View.dicUserTypes = dicUserTypes;
            }
        }

        public void GetRequirementCategory()
        {
            if (View.SelectedTenantId != AppConsts.NONE)
            {
                ServiceRequest<Int32, List<Int32>> serviceRequest = new ServiceRequest<Int32, List<Int32>>();
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = View.SelectedPackageIds.IsNullOrEmpty() ? new List<Int32>() : View.SelectedPackageIds;

                var _serviceResponse = _clientRotationProxy.GetRequirementCategory(serviceRequest);

                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.LstRequirementCategory = _serviceResponse.Result.GroupBy(a => a.RequirementCategoryName.Trim())
                        .Select(cat => new RequirementCategoryContract
                        {
                            RequirementCategoryName = cat.Select(c => c.RequirementCategoryName.Trim()).FirstOrDefault()
                            ,
                            RequirementCategoryIDs = String.Join(",", cat.Select(d => d.RequirementCategoryID))
                        })
                        .OrderBy(o => o.RequirementCategoryName)
                        .ToList();
                }
                else
                {
                    View.LstRequirementCategory = new List<RequirementCategoryContract>();
                }
            }
            else
            {
                View.LstRequirementCategory = new List<RequirementCategoryContract>();
            }

        }

        //public void GetRequirementCategoryItems()
        //{

        //    if (View.ReqCategoryId > AppConsts.NONE)
        //    {
        //        ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
        //        serviceRequest.Parameter = View.ReqCategoryId;
        //        ServiceResponse<List<RequirementItemContract>> serviceResponse = _requirementPackageProxy.GetRequirementItemsByCategoryID(serviceRequest);
        //        if (!serviceResponse.Result.IsNullOrEmpty())
        //        {
        //            View.lstRequirementItems = serviceResponse.Result.OrderBy(x => x.RequirementItemName).ToList();
        //        }
        //        else
        //        {
        //            View.lstRequirementItems = new List<RequirementItemContract>();
        //        }
        //    }
        //}

        public void GetRequirementItem()
        {
            if (View.SelectedTenantId != AppConsts.NONE && !string.IsNullOrWhiteSpace(View.CategoryId))
            {
                ServiceRequest<Int32, List<Int32>> serviceRequest = new ServiceRequest<Int32, List<Int32>>();
                List<int> lstCategory= new List<int>();
                //lstCategory.Add(Convert.ToInt32(View.CategoryId));

                lstCategory = View.CategoryId.Split(',').ConvertIntoIntList(); //UAT-4705
                
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = lstCategory;

                var _serviceResponse = _clientRotationProxy.GetRequirementItem(serviceRequest);

                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.LstRequirementItems = _serviceResponse.Result.GroupBy(a => a.RequirementItemName.Trim())
                        .Select(items => new RequirementItemContract
                        {
                            RequirementItemName = items.Select(c => c.RequirementItemName.Trim()).FirstOrDefault()
                            ,
                            RequirementItemIDs = String.Join(",", items.Select(d => d.RequirementItemID))
                        })
                        .OrderBy(o => o.RequirementItemName)
                        .ToList();
                }
                else
                {
                    View.LstRequirementItems = new List<RequirementItemContract>();
                }
            }
            else
            {
                View.LstRequirementItems = new List<RequirementItemContract>();
            }

        }

    }
}
