using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
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
    public class CategoryPackageMappingPresenter : Presenter<ICategoryPackageMappingView>
    {
        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        #region [Public Method]

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {

        }

        public void GetCategoryPackageMapping()
        {
            if (View.ReqCategoryID != 0)
            {
                CategoryPackageMappingContract categoryPackageMappingContract = new CategoryPackageMappingContract();
                categoryPackageMappingContract.ReqCategoryID = View.ReqCategoryID;
                categoryPackageMappingContract.ReqPackageName = View.ReqPackageName;
                categoryPackageMappingContract.EffectiveStartDate = View.EffStartDate;
                categoryPackageMappingContract.EffectiveEndDate = View.EffEndDate;
                categoryPackageMappingContract.ResultTypeID = View.ResultTypeID;

                ServiceRequest<CategoryPackageMappingContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<CategoryPackageMappingContract, CustomPagingArgsContract>();
                serviceRequest.Parameter1 = categoryPackageMappingContract;
                serviceRequest.Parameter2 = View.GridCustomPaging;
                var serviceResponse = _requirementPackageProxy.GetCategoryPackageMapping(serviceRequest);

                View.LstRequirementPackages = serviceResponse.Result;

                if (serviceResponse.Result.IsNullOrEmpty())
                {
                    View.VirtualRecordCount = AppConsts.NONE;
                }
                else
                {
                    View.VirtualRecordCount = serviceResponse.Result[0].TotalCount;

                    //var alreadyMappedPkgIds = View.LstRequirementPackages.Where(cond => cond.IsCategoryMappedWithPkg == true).Select(s => s.RequirementPackageID).ToList();

                    //List<Int32> lstPackageIdsMappedWithCategory = View.LstPackageIdsMappedWithCategory;
                    //List<Int32> lstUnMappedPkgIds = View.LstUnMappedPackageIds;

                    //foreach (var item in alreadyMappedPkgIds)
                    //{
                    //    if (!lstPackageIdsMappedWithCategory.Contains(item) && !lstUnMappedPkgIds.Contains(item))
                    //    {
                    //        lstPackageIdsMappedWithCategory.Add(item);
                    //    }
                    //}

                    //View.LstPackageIdsMappedWithCategory = lstPackageIdsMappedWithCategory;
                }
            }
        }

        public void SaveCategoryPackageMapping()
        {
            ServiceRequest<Int32, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32, String>();
            serviceRequest.Parameter1 = View.CurrentLoggedInUserID;
            serviceRequest.Parameter2 = View.ReqCategoryID;
            serviceRequest.Parameter3 = View.MappedPkgIds;

            var serviceResponse = _requirementPackageProxy.SaveCategoryPackageMapping(serviceRequest);
        }

        public void GetMappedPackageIdsWithCategory()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.ReqCategoryID;
            var serviceResponse = _requirementPackageProxy.GetMappedPackageIdsWithCategory(serviceRequest);

            if (!serviceResponse.Result.IsNullOrEmpty())
            {
                View.LstPackageIdsMappedWithCategory = serviceResponse.Result;
            }
            else
            {
                View.LstPackageIdsMappedWithCategory = new List<int>();
            }
        }

        #endregion

    }
}
