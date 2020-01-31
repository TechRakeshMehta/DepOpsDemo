#region NameSpace
#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
#region Project Specific
using INTSOF.SharedObjects;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
#endregion
#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class SetupRequirementCategoryPresenter : Presenter<ISetupRequirementCategoryView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetRequirementCategoryDetail();
            // GetUniversalCategorys();
            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                //  GetUniversalCategoryByReqCatID(View.RequirementCategoryID);
            }
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public void GetRequirementCategoryDetail()
        {
            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                serviceRequest.Parameter1 = View.RequirementPackageId;
                serviceRequest.Parameter2 = View.RequirementCategoryID;
                ServiceResponse<RequirementCategoryContract> serviceResponse = _requirementPackageProxy.GetRequirementCategoryDetail(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.CategoryName = serviceResponse.Result.RequirementCategoryName;
                    View.RequirementDocumentLink = serviceResponse.Result.RequirementDocumentLink;
                    View.ExplanatoryNotes = serviceResponse.Result.ExplanatoryNotes;
                    //UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                    View.IsComplianceRequired = serviceResponse.Result.IsComplianceRequired;
                    View.ComplianceReqStartDate = serviceResponse.Result.ComplianceReqStartDate;
                    View.ComplianceReqEndDate = serviceResponse.Result.ComplianceReqEndDate;
                    View.RequirementDocumentLinkLabel = serviceResponse.Result.RequirementDocumentLinkLabel; //UAT-3161
                    View.SendItemDocOnApproval = serviceResponse.Result.SendItemDoconApproval; //UAT-3805
                }
            }
        }

        public void SaveRequirementCategoryDetails()
        {

            RequirementCategoryContract reqCtgryContarct = new RequirementCategoryContract();
            reqCtgryContarct.RequirementCategoryID = View.RequirementCategoryID;
            reqCtgryContarct.RequirementPackageID = View.RequirementPackageId;
            reqCtgryContarct.RequirementCategoryName = View.CategoryName;
            reqCtgryContarct.RequirementDocumentLink = View.RequirementDocumentLink;
            reqCtgryContarct.ExplanatoryNotes = View.ExplanatoryNotes;

            //UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            reqCtgryContarct.IsComplianceRequired = View.IsComplianceRequired;
            reqCtgryContarct.ComplianceReqStartDate = View.ComplianceReqStartDate;
            reqCtgryContarct.ComplianceReqEndDate = View.ComplianceReqEndDate;
            reqCtgryContarct.SendItemDoconApproval = View.SendItemDocOnApproval; //UAT-3805

            #region UAT-2305
            //if ((View.UniversalCategoryData.IsNullOrEmpty() || View.UniversalCategoryData.UniversalCategoryID != View.UniversalCategoryID)
            //    && (View.UniversalCategoryID > AppConsts.NONE || (!View.UniversalCategoryData.IsNullOrEmpty() && View.UniversalCategoryData.UniCatReqCatMappingID > AppConsts.NONE)))
            //{
            //    reqCtgryContarct.UniversalCategoryID = View.UniversalCategoryID;

            //    if (!View.UniversalCategoryData.IsNullOrEmpty())
            //    {
            //        reqCtgryContarct.UniversalReqCatMappingID = View.UniversalCategoryData.UniCatReqCatMappingID;
            //    }
            //}

            #endregion

            reqCtgryContarct.RequirementDocumentLinkLabel = View.RequirementDocumentLinkLabel; //UAT-3161
            ServiceRequest<RequirementCategoryContract> serviceRequest = new ServiceRequest<RequirementCategoryContract>();
            serviceRequest.Parameter = reqCtgryContarct;
            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveRequirementCategoryDetails(serviceRequest);
            if (!serviceResponse.Result)
            {
                View.ErrorMessage = "Some error has occurred.Please try again.";
            }
            else
            {
                //if (!View.UniversalCategoryData.IsNullOrEmpty())
                //{
                //    View.UniversalCategoryData.UniversalCategoryID = View.UniversalCategoryID;
                //}
            }

        }

        public void GetRequirementCategories()
        {
            View.LstCategories = new List<RequirementCategoryContract>();
            if (View.RequirementPackageId > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementPackageId;
                ServiceResponse<List<RequirementCategoryContract>> serviceResponse = _requirementPackageProxy.GetRequirementCategoriesByPackageID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.LstCategories = serviceResponse.Result.OrderBy(x => x.RequirementCategoryName).ToList();
                }
            }
        }

        public Boolean DeleteReqPackageCategoryMapping(Int32 reqPkgCtgryID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = reqPkgCtgryID;
            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.DeleteReqPackageCategoryMapping(serviceRequest);
            //  DeleteUniversalReqCategoryMapping(reqPkgCtgryID);
            return serviceResponse.Result;
        }

        public Boolean DeleteReqCategoryItemMapping(Int32 reqCategoryItemID)
        {
            ServiceRequest<Int32, Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Int32, Boolean>();
            serviceRequest.Parameter1 = reqCategoryItemID;
            serviceRequest.Parameter2 = View.RequirementPackageId;
            //UAT-2514:Rotation Package
            serviceRequest.Parameter3 = false;
            ServiceResponse<String> serviceResponse = _requirementPackageProxy.DeleteReqCategoryItemMapping(serviceRequest);
            View.ErrorMessage = serviceResponse.Result;
            return View.ErrorMessage.IsNullOrEmpty() ? true : false;
        }

        public void GetRequirementCategoryItems()
        {
            View.LstCategoryItems = new List<RequirementItemContract>();
            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementCategoryID;
                ServiceResponse<List<RequirementItemContract>> serviceResponse = _requirementPackageProxy.GetRequirementItemsByCategoryID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.LstCategoryItems = serviceResponse.Result.OrderBy(x => x.RequirementItemDisplayOrder).ToList(); //UAT-3078
                }
            }
        }
        #region UAT-2305
        //public void GetUniversalCategorys()
        //{
        //    View.lstUniversalCategory = new List<UniversalCategoryContract>();
        //    ServiceResponse<List<UniversalCategoryContract>> serviceResponse = _requirementPackageProxy.GetUniversalCategorys();
        //    if (!serviceResponse.Result.IsNullOrEmpty())
        //    {
        //        View.lstUniversalCategory = serviceResponse.Result.ToList();
        //    }
        //}

        //public void GetUniversalCategoryByReqCatID(Int32 requirementCategoryID)
        //{
        //    ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
        //    serviceRequest.Parameter = requirementCategoryID;
        //    ServiceResponse<UniversalCategoryContract> serviceResponse = _requirementPackageProxy.GetUniversalCategoryByReqCatID(serviceRequest);
        //    if (!serviceResponse.Result.IsNullOrEmpty())
        //    {
        //        View.UniversalCategoryData = serviceResponse.Result;
        //        View.UniversalCategoryID = serviceResponse.Result.UniversalCategoryID;
        //    }
        //}

        //public Boolean DeleteUniversalReqCategoryMapping(Int32 requirementCategoryID)
        //{
        //    GetUniversalCategoryByReqCatID(requirementCategoryID);
        //    if (!View.UniversalCategoryData.IsNullOrEmpty() && View.UniversalCategoryData.UniCatReqCatMappingID > AppConsts.NONE)
        //    {
        //        ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
        //        serviceRequest.Parameter = View.UniversalCategoryData.UniCatReqCatMappingID;
        //        ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.DeleteUnversalCategoryMappings(serviceRequest);
        //        return serviceResponse.Result;
        //    }
        //    return false;
        //}
        #endregion
    }
}

