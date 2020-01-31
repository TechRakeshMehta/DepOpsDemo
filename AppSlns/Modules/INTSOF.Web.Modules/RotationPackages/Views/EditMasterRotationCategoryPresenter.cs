using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;


using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;


namespace CoreWeb.RotationPackages.Views
{
    public class EditMasterRotationCategoryPresenter : Presenter<IEditMasterRotationCategoryView>
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
            //if (View.RequirementCategoryID > AppConsts.NONE)
            //{
            //    GetUniversalCategoryByReqCatID(View.RequirementCategoryID);
            //}
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
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementCategoryID;
                ServiceResponse<RequirementCategoryContract> serviceResponse = _requirementPackageProxy.GetRequirementMasterCategoryDetailByCategoryID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.CategoryName = serviceResponse.Result.RequirementCategoryName;
                    // View.RequirementDocumentLink = serviceResponse.Result.RequirementDocumentLink; //Commented in UAT-4254
                    View.ExplanatoryNotes = serviceResponse.Result.ExplanatoryNotes;
                    View.IsComplianceRequired = serviceResponse.Result.IsComplianceRequired;
                    View.ComplianceReqStartDate = serviceResponse.Result.ComplianceReqStartDate;
                    View.ComplianceReqEndDate = serviceResponse.Result.ComplianceReqEndDate;
                    //UAT-2213
                    View.CategoryLabel = serviceResponse.Result.RequirementCategoryLabel;
                    View.IsAllowDataMovement = serviceResponse.Result.AllowDataMovement; //Uat-2603
                    //UAT-3161
                    //View.RequirementDocumentLinkLabel = serviceResponse.Result.RequirementDocumentLinkLabel;
                    //UAT 3805
                    View.SendItemDocOnApproval = serviceResponse.Result.SendItemDoconApproval;
                    //UAT 4259
                    View.TriggerOtherCategoryRules = serviceResponse.Result.TriggerOtherCategoryRules;
                    View.lstEditableBy = serviceResponse.Result.SelectedlstEditableBy;
                }

                GetRequirementCatDocUrls(); //Added In UAT-4254

                GetPackagesAssociatedWithCategory(View.RequirementCategoryID);//Added in UAT-4657
            }
        }

        public void SaveRequirementCategoryDetails()
        {

            RequirementCategoryContract reqCtgryContarct = new RequirementCategoryContract();
            reqCtgryContarct.RequirementCategoryID = View.RequirementCategoryID;
            reqCtgryContarct.RequirementPackageID = View.RequirementPackageId;
            reqCtgryContarct.RequirementCategoryName = View.CategoryName;
            //reqCtgryContarct.RequirementDocumentLink = View.RequirementDocumentLink; //Commented in UAT-4254
            reqCtgryContarct.ExplanatoryNotes = View.ExplanatoryNotes;
            //UAT-2213
            reqCtgryContarct.RequirementCategoryLabel = View.CategoryLabel;

            //UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            reqCtgryContarct.IsComplianceRequired = View.IsComplianceRequired;
            reqCtgryContarct.ComplianceReqStartDate = View.ComplianceReqStartDate;
            reqCtgryContarct.ComplianceReqEndDate = View.ComplianceReqEndDate;

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
            //UAT-2603
            reqCtgryContarct.AllowDataMovement = View.IsAllowDataMovement;
            //UAT-3161
            //reqCtgryContarct.RequirementDocumentLinkLabel = View.RequirementDocumentLinkLabel; //Commented in UAT-4254
            reqCtgryContarct.SendItemDoconApproval = View.SendItemDocOnApproval; //UAT-3805
            reqCtgryContarct.SelectedlstEditableBy = View.lstEditableBy; //UAt-4165
            //UAT 4259
            reqCtgryContarct.TriggerOtherCategoryRules = View.TriggerOtherCategoryRules;

            //Added in UAT-4254
            reqCtgryContarct.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
            reqCtgryContarct.lstReqCatDocUrls = View.lstReqCatDocUrls;
            //END
            ServiceRequest<RequirementCategoryContract> serviceRequest = new ServiceRequest<RequirementCategoryContract>();
            serviceRequest.Parameter = reqCtgryContarct;
            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveMasterRotationCategory(serviceRequest);//UAT-2213, called new method for saving category details
            if (!serviceResponse.Result)
            {
                View.ErrorMessage = "Some error has occurred.Please try again.";
            }
            //else
            //{
            //    //if (!View.UniversalCategoryData.IsNullOrEmpty())
            //    //{
            //    //    View.UniversalCategoryData.UniversalCategoryID = View.UniversalCategoryID;
            //    //}
            //}

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
            //DeleteUniversalReqCategoryMapping(reqPkgCtgryID);
            return serviceResponse.Result;
        }

        public Boolean DeleteReqCategoryItemMapping(Int32 reqCategoryItemID)
        {
            ServiceRequest<Int32, Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Int32, Boolean>();
            serviceRequest.Parameter1 = reqCategoryItemID;
            serviceRequest.Parameter2 = View.RequirementPackageId;
            serviceRequest.Parameter3 = true;
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
                    View.LstCategoryItems = serviceResponse.Result.OrderBy(x => x.RequirementItemDisplayOrder).ToList();  //UAT-3078
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

        #region UAT-4254 || Release - 181

        public void GetRequirementCatDocUrls()
        {
            //  View.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementCategoryID;
                ServiceResponse<List<RequirementCategoryDocUrl>> serviceResponse = _requirementPackageProxy.GetRequirementCatDocUrls(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
                    View.lstReqCatDocUrls = serviceResponse.Result.ToList();
                }
                //View.lstReqCatDocUrls.Add(new RequirementCategoryDocUrl
                //{
                //    RequirementCatDocLinkID = 0,
                //    RequirementCatID = View.RequirementCategoryID,
                //    RequirementCatDocUrl = String.Empty,
                //    RequirementCatDocUrlLabel = String.Empty
                //});
            }
        }

        #endregion

        #region UAT-4657

        public void GetPackagesAssociatedWithCategory(Int32 categoryId)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = categoryId;
            ServiceResponse<Dictionary<Int32, String>> serviceResponse = _requirementPackageProxy.GetPackagesAssociatedWithCategory(serviceRequest);
            View.PackagesDataAssociatedWithCategory = serviceResponse.Result;
        }

        public Boolean SaveCategoryDiassociationDetail(Int32 categoryId, String PackageIds)
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = categoryId;
            serviceRequest.Parameter2 = PackageIds;
            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveCategoryDiassociationDetail(serviceRequest);
            return serviceResponse.Result;
        }

        public String IsCategoryDisassociationInProgress(Int32 categoryId, List<Int32> selectedPkgIds)
        {
            ServiceRequest<Int32, List<Int32>> serviceRequest = new ServiceRequest<Int32, List<Int32>>();
            serviceRequest.Parameter1 = categoryId;
            serviceRequest.Parameter2 = selectedPkgIds;

            ServiceResponse<String> serviceResponse = _requirementPackageProxy.IsCategoryDisassociationInProgress(serviceRequest);
            return serviceResponse.Result;
        }

        public Boolean IsRequirementSyncAlreadyInProgress(Int32 categoryId)
        {
            ServiceRequest<Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Boolean>();
            serviceRequest.Parameter1 = categoryId;
            serviceRequest.Parameter2 = false;
            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.IsSyncAlreadyInProgress(serviceRequest);
            return serviceResponse.Result;

        }

        #endregion
    }
}


