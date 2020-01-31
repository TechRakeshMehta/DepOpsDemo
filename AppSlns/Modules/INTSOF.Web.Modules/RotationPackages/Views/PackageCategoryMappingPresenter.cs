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
    public class PackageCategoryMappingPresenter : Presenter<IPackageCategoryMappingView>
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

        public void GetPackageCategoryMapping()
        {
            if (View.ReqPackageID != 0)
            {
                PackageCategoryMappingContract categoryPackageMappingContract = new PackageCategoryMappingContract();
                categoryPackageMappingContract.ReqPackageID = View.ReqPackageID;
                categoryPackageMappingContract.ReqCategoryName = View.ReqCategoryName;
                categoryPackageMappingContract.ResultTypeID = View.ResultTypeID;

                ServiceRequest<PackageCategoryMappingContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<PackageCategoryMappingContract, CustomPagingArgsContract>();
                serviceRequest.Parameter1 = categoryPackageMappingContract;
                serviceRequest.Parameter2 = View.GridCustomPaging;
                var serviceResponse = _requirementPackageProxy.GetPackageCategoryMapping(serviceRequest);

                View.LstRequirementCategory = serviceResponse.Result;

                if (serviceResponse.Result.IsNullOrEmpty())
                {
                    View.VirtualRecordCount = AppConsts.NONE;
                }
                else
                {
                    View.VirtualRecordCount = serviceResponse.Result[0].TotalCount;
                    //var alreadyMappedPkgIds = View.LstRequirementCategory.Where(cond => cond.IsCategoryMappedWithPkg == true).Select(s => s.RequirementCategoryID).ToList();

                    //List<Int32> lstCategoryIdsMappedWithPackage = View.LstCategoryIdsMappedWithPackage;
                    //List<Int32> lstUnMappedPkgIds = View.LstUnMappedCategoryIds;


                    //foreach (var item in alreadyMappedPkgIds)
                    //{
                    //    if (!lstCategoryIdsMappedWithPackage.Contains(item) && !lstUnMappedPkgIds.Contains(item))
                    //    {
                    //        lstCategoryIdsMappedWithPackage.Add(item);
                    //    }
                    //}

                    //View.LstCategoryIdsMappedWithPackage = lstCategoryIdsMappedWithPackage;
                }
            }
        }
        //UAT:4279

        public void UpdateNodeDisplayOrder(List<RequirementCategoryContract> nodeList, int? destinationIndex)
        {
            ServiceRequest<List<RequirementCategoryContract>, Int32?, Int32,Int32> serviceRequest = new ServiceRequest<List<RequirementCategoryContract>, Int32?, Int32,Int32>();
            serviceRequest.Parameter1 = nodeList;
            serviceRequest.Parameter2 =  destinationIndex;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserID;
            serviceRequest.Parameter4 = View.ReqPackageID;

            var serviceResponse = _requirementPackageProxy.UpdateNodeDisplayOrder(serviceRequest);
        }

        public void SavePackageCategoryMapping()
        {
            ServiceRequest<Int32, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32, String>();
            serviceRequest.Parameter1 = View.CurrentLoggedInUserID;
            serviceRequest.Parameter2 = View.ReqPackageID;
            serviceRequest.Parameter3 = View.MappedCategoryIds;

            var serviceResponse = _requirementPackageProxy.SavePackageCategoryMapping(serviceRequest);
        }

        public void GetMappedCategoriesWithPackage()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.ReqPackageID;

            var serviceResponse = _requirementPackageProxy.GetMappedCategoriesWithPackage(serviceRequest);

            if (!serviceResponse.Result.IsNullOrEmpty())
            {
                View.LstCategoryIdsMappedWithPackage = serviceResponse.Result;
            }
            else
            {
                View.LstCategoryIdsMappedWithPackage = new List<int>();
            }
        }

        #endregion

    }
}
