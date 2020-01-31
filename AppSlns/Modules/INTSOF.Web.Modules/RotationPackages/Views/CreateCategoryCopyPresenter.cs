using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
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
    public class CreateCategoryCopyPresenter : Presenter<ICreateCategoryCopyView>
    {
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
            GetRequirementCategoryDetail();
            //Commented code related to UAT-2985
            //GetUniversalCategorys();
            GetRequirementPackages();

            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                //Commented code related to UAT-2985
                //GetUniversalCategoryByReqCatID(View.RequirementCategoryID);
                GetMappedPackageDetails(View.RequirementCategoryID);
            }
        }

        public int CreateCategoryCopy()
        {
            int newlyAddedCategoryID = 0;

            if (!IsMasterCategoryNameExists())
            {
                var selectedRotPkgIds = View.lstRequirementPackage.Select(cond => cond.RequirementPackageID).ToList();

                CreateCategoryCopyContract createCategoryCopyContract = new CreateCategoryCopyContract();
                createCategoryCopyContract.OldRequirementCategoryID = View.RequirementCategoryID;

                if (!selectedRotPkgIds.IsNullOrEmpty())
                    createCategoryCopyContract.SelectedReqPackageIds = String.Join(",", selectedRotPkgIds);

                createCategoryCopyContract.CategoryName = View.CategoryName;
                createCategoryCopyContract.CategoryLabel = View.CategoryLabel;
                createCategoryCopyContract.CurrentLoggedInUserId = View.CurrentLoggedInUserId;
                createCategoryCopyContract.ExplanatoryNotes = View.ExplanatoryNotes;
                createCategoryCopyContract.IsComplianceRequired = View.IsComplianceRequired;
                createCategoryCopyContract.ComplianceReqStartDate = View.ComplianceReqStartDate;
                createCategoryCopyContract.ComplianceReqEndDate = View.ComplianceReqEndDate;
                //createCategoryCopyContract.RequirementDocumentLink = View.RequirementDocumentLink;  //Commented in UAT-4254
                //Commented code related to UAT-2985
                //createCategoryCopyContract.UniversalCategoryID = View.UniversalCategoryID;
                //UAT-2603
                createCategoryCopyContract.AllowDataMovement = View.AllowDatamovement;
                //UAT-3161
                //createCategoryCopyContract.RequirementDocumentLinkLabel = View.RequirementDocumentLinkLabel;   //Commented in UAT-4254
                //UAT-3805
                createCategoryCopyContract.SendItemDoconApproval = View.SendItemDoconApproval;
                createCategoryCopyContract.SelectedlstEditableBy = View.lstEditableBy; //UAt-4165
                createCategoryCopyContract.TriggerOtherCategoryRules = View.TriggerOtherCategoryRules; //UAT 4259
                //#region UAT-2305
                //if ((View.UniversalCategoryData.IsNullOrEmpty() || View.UniversalCategoryData.UniversalCategoryID != View.UniversalCategoryID)
                //    && (View.UniversalCategoryID > AppConsts.NONE || (!View.UniversalCategoryData.IsNullOrEmpty() && View.UniversalCategoryData.UniCatReqCatMappingID > AppConsts.NONE)))
                //{
                //    createCategoryCopyContract.UniversalCategoryID = View.UniversalCategoryID;

                //    if (!View.UniversalCategoryData.IsNullOrEmpty())
                //    {
                //        createCategoryCopyContract.UniversalReqCatMappingID = View.UniversalCategoryData.UniCatReqCatMappingID;
                //    }
                //}

                //#endregion

                //Added in UAT-4254
                createCategoryCopyContract.lstReqCatUrls = View.lstReqCatDocUrls;
                //END


                ServiceRequest<CreateCategoryCopyContract> serviceRequest = new ServiceRequest<CreateCategoryCopyContract>();
                serviceRequest.Parameter = createCategoryCopyContract;
                ServiceResponse<Int32> serviceResponse = _requirementPackageProxy.CreateCategoryCopy(serviceRequest);
                newlyAddedCategoryID = serviceResponse.Result;
                if (newlyAddedCategoryID <= 0)
                    View.ErrorMessage = "Some error has occurred.Please try again.";
                else
                    return newlyAddedCategoryID;
            }
            else
            {
                View.ErrorMessage = "This category name is already in use, please try different category name.";
            }
            return newlyAddedCategoryID;
        }




        #region [Private Methods]

        private void GetRequirementCategoryDetail()
        {
            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementCategoryID;
                ServiceResponse<RequirementCategoryContract> serviceResponse = _requirementPackageProxy.GetRequirementMasterCategoryDetailByCategoryID(serviceRequest);

                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.CategoryName = serviceResponse.Result.RequirementCategoryName;
                    View.CategoryLabel = serviceResponse.Result.RequirementCategoryLabel;
                    View.ExplanatoryNotes = serviceResponse.Result.ExplanatoryNotes;
                    View.IsComplianceRequired = serviceResponse.Result.IsComplianceRequired;
                    View.ComplianceReqStartDate = serviceResponse.Result.ComplianceReqStartDate;
                    View.ComplianceReqEndDate = serviceResponse.Result.ComplianceReqEndDate;
                    //View.RequirementDocumentLink = serviceResponse.Result.RequirementDocumentLink;   //Commented in UAT-4254
                    //UAT-2603
                    View.AllowDatamovement = serviceResponse.Result.AllowDataMovement;
                    //UAT-3161
                    //View.RequirementDocumentLinkLabel = serviceResponse.Result.RequirementDocumentLinkLabel;   //Commented in UAT-4254
                    //UAT-3805
                    View.SendItemDoconApproval = serviceResponse.Result.SendItemDoconApproval;
                    //UAT-4165
                    View.lstEditableBy = serviceResponse.Result.SelectedlstEditableBy;
                    //UAT 4259
                    View.TriggerOtherCategoryRules = serviceResponse.Result.TriggerOtherCategoryRules;
                }

                GetRequirementCatDocUrls(); //UAT-4254
            }
        }

        private bool IsMasterCategoryNameExists()
        {
            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
                serviceRequest.Parameter = View.CategoryName.Trim();
                ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.IsMasterCategoryNameExists(serviceRequest);

                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    return serviceResponse.Result;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private void GetRequirementPackages()
        {
            ServiceResponse<List<RequirementPackageContract>> serviceResponse = _requirementPackageProxy.GetAllMasterRequirementPackages();

            if (!serviceResponse.Result.IsNullOrEmpty())
            {
                View.lstRequirementPackage = serviceResponse.Result;
            }
        }

        #region UAT-2305
        //Commented code related to UAT-2985
        /*private void GetUniversalCategorys()
        {
            View.lstUniversalCategory = new List<UniversalCategoryContract>();
            ServiceResponse<List<UniversalCategoryContract>> serviceResponse = _requirementPackageProxy.GetUniversalCategorys();
            if (!serviceResponse.Result.IsNullOrEmpty())
            {
                View.lstUniversalCategory = serviceResponse.Result.ToList();
            }
        }

        private void GetUniversalCategoryByReqCatID(Int32 requirementCategoryID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = requirementCategoryID;
            ServiceResponse<UniversalCategoryContract> serviceResponse = _requirementPackageProxy.GetUniversalCategoryByReqCatID(serviceRequest);
            if (!serviceResponse.Result.IsNullOrEmpty())
            {
                View.UniversalCategoryData = serviceResponse.Result;
                View.UniversalCategoryID = serviceResponse.Result.UniversalCategoryID;
            }
        }*/

        private void GetMappedPackageDetails(Int32 requirementCategoryID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = requirementCategoryID;
            ServiceResponse<List<Int32>> serviceResponse = _requirementPackageProxy.GetMappedPackageDetails(serviceRequest);
            if (!serviceResponse.Result.IsNullOrEmpty())
            {
                View.SelectMappedPackages = serviceResponse.Result;
            }
        }

        #endregion

        #region UAT-4254 || Release - 181

        private void GetRequirementCatDocUrls()
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
            }
        }
        #endregion

        #endregion
    }
}
