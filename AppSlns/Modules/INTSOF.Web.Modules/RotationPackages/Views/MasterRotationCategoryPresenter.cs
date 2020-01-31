using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using Business.RepoManagers;

namespace CoreWeb.RotationPackages.Views
{
    public class MasterRotationCategoryPresenter : Presenter<IMasterRotationCategoryView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads 
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
        /// Method to get single universal category mapping 
        /// </summary>
        /// <param name="requirementCategoryID"></param>
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

        /// <summary>
        /// Method to save and update categories
        /// </summary>
        /// <returns></returns>
        public bool SaveRequirementCategoryDetails()
        {
            RequirementCategoryContract reqCtgryContarct = new RequirementCategoryContract();
            reqCtgryContarct.RequirementCategoryID = View.ViewContract.RequirementCategoryID;
            reqCtgryContarct.RequirementCategoryName = View.ViewContract.RequirementCategoryName;
            reqCtgryContarct.RequirementDocumentLink = View.ViewContract.RequirementDocumentLink;
            reqCtgryContarct.ExplanatoryNotes = View.ViewContract.ExplanatoryNotes;
            reqCtgryContarct.RequirementCategoryLabel = View.ViewContract.RequirementCategoryLabel;

            reqCtgryContarct.IsComplianceRequired = View.ViewContract.IsComplianceRequired;
            reqCtgryContarct.ComplianceReqStartDate = View.ViewContract.ComplianceReqStartDate;
            reqCtgryContarct.ComplianceReqEndDate = View.ViewContract.ComplianceReqEndDate;
            //reqCtgryContarct.UniversalCategoryID = View.ViewContract.UniversalCategoryID;
            //UAT-2603
            reqCtgryContarct.AllowDataMovement = View.ViewContract.AllowDataMovement;
            //UAT-3161
            reqCtgryContarct.RequirementDocumentLinkLabel = View.ViewContract.RequirementDocumentLinkLabel;
            reqCtgryContarct.SendItemDoconApproval = View.ViewContract.SendItemDoconApproval; //UAT-3805
            #region UAT-2305
            //if ((View.UniversalCategoryData.IsNullOrEmpty() || View.UniversalCategoryData.UniversalCategoryID != View.UniversalCategoryID)
            //    && (View.UniversalCategoryID > AppConsts.NONE || (!View.UniversalCategoryData.IsNullOrEmpty() && View.UniversalCategoryData.UniCatReqCatMappingID > AppConsts.NONE)))
            //{
            //    if (!View.UniversalCategoryData.IsNullOrEmpty())
            //    {
            //        reqCtgryContarct.UniversalReqCatMappingID = View.UniversalCategoryData.UniCatReqCatMappingID;
            //    }
            //}
            #endregion
            reqCtgryContarct.SelectedlstEditableBy = View.ViewContract.SelectedlstEditableBy;  //UAT-4165
            reqCtgryContarct.TriggerOtherCategoryRules = View.ViewContract.TriggerOtherCategoryRules; //4259

            //Added in UAT-4254
            reqCtgryContarct.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
            reqCtgryContarct.lstReqCatDocUrls = View.lstReqCatDocUrls;
            //END

            ServiceRequest<RequirementCategoryContract> serviceRequest = new ServiceRequest<RequirementCategoryContract>();
            serviceRequest.Parameter = reqCtgryContarct;
            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveMasterRotationCategory(serviceRequest);
            if (!serviceResponse.Result)
            {
                View.ErrorMessage = "Some error has occurred.Please try again.";
            }
            return serviceResponse.Result;
        }

        /// <summary>
        /// Method to get list categories and filter categories
        /// </summary>
        public void GetRequirementCategories()
        {
            List<RequirementCategoryContract> lstRequirementCategoriesDetailsContract = new List<RequirementCategoryContract>();
            RequirementCategoryContract searchContract = new RequirementCategoryContract();

            searchContract.RequirementCategoryName = View.RequirementCategoryName.IsNullOrEmpty() ? null : View.RequirementCategoryName;
            searchContract.lstSelectedAgencyIds = View.LstSelectedAgencyIDs.IsNullOrEmpty() ? null : View.LstSelectedAgencyIDs;
            searchContract.RequirementCategoryLabel = View.RequirementCategoryLabel.IsNullOrEmpty() ? null : View.RequirementCategoryLabel;

            ServiceRequest<RequirementCategoryContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<RequirementCategoryContract, CustomPagingArgsContract>();
            serviceRequest.Parameter1 = searchContract;
            serviceRequest.Parameter2 = View.GridCustomPaging;
            lstRequirementCategoriesDetailsContract = _requirementPackageProxy.GetMasterRequirementCategories(serviceRequest).Result;
            if (!lstRequirementCategoriesDetailsContract.IsNullOrEmpty())
            {
                if (lstRequirementCategoriesDetailsContract[0].TotalCount > 0)
                {
                    View.VirtualRecordCount = lstRequirementCategoriesDetailsContract[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }
            View.LstCategories = lstRequirementCategoriesDetailsContract;

        }

        /// <summary>
        /// Method to delete category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public String DeleteMasterRequirementCategory(Int32 categoryID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = categoryID;
            ServiceResponse<String> serviceResponse = _requirementPackageProxy.DeleteRequirementCategory(serviceRequest);
            return serviceResponse.Result;
        }

        /// <summary>
        /// Method to get single category details
        /// </summary>
        public void GetRequirementCategoriesDetailsByID()
        {
            List<RequirementCategoryContract> lstRequirementCategoriesDetailsContract = new List<RequirementCategoryContract>();
            RequirementCategoryContract searchContract = new RequirementCategoryContract();
            searchContract.RequirementCategoryID = View.ViewContract.RequirementCategoryID;

            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = searchContract.RequirementCategoryID;
            ServiceResponse<RequirementCategoryContract> serviceResponse = _requirementPackageProxy.GetRequirementMasterCategoryDetailByCategoryID(serviceRequest);

            View.SingleCategory = serviceResponse.Result;
            GetRequirementCatDocUrls();//UAT-4254
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            //UAT-1881
            if (IsAdminLoggedIn())
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.TenantId;
                ServiceResponse<List<AgencyDetailContract>> _serviceResponse = _clinicalRotationProxy.GetAgencies(serviceRequest);
                //  UAT-1448 "Agency" field should display checkboxes in alphabetical order on the manage rotation package screen.
                View.lstAgency = _serviceResponse.Result.OrderBy(a => a.AgencyName).ToList();
            }
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter = View.CurrentLoggedInUserId;
                var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                View.lstAgency = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantId);
        }
        /// <summary>
        /// Get All UniversalCategories
        /// </summary>
        //public void GetUniversalCategorys()
        //{
        //    View.lstUniversalCategory = new List<UniversalCategoryContract>();
        //    ServiceResponse<List<UniversalCategoryContract>> serviceResponse = _requirementPackageProxy.GetUniversalCategorys();
        //    if (!serviceResponse.Result.IsNullOrEmpty())
        //    {
        //        View.lstUniversalCategory = serviceResponse.Result.ToList();
        //    }
        //}

        #region UAT-4254 || Release - 181

        public void GetRequirementCatDocUrls()
        {
            //  View.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
            if (View.ViewContract.RequirementCategoryID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.ViewContract.RequirementCategoryID;
                ServiceResponse<List<RequirementCategoryDocUrl>> serviceResponse = _requirementPackageProxy.GetRequirementCatDocUrls(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
                    View.lstReqCatDocUrls = serviceResponse.Result.ToList();
                }
            }
        }

        #endregion
        #region UAT-4657
        public Dictionary<Int32, String> GetPackagesAssociatedWithCategory(Int32 categoryId)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = categoryId;
            ServiceResponse<Dictionary<Int32, String>> serviceResponse = _requirementPackageProxy.GetPackagesAssociatedWithCategory(serviceRequest);
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

        public Boolean SaveCategoryDiassociationDetail(Int32 categoryId,String PackageIds)
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = categoryId;
            serviceRequest.Parameter2 = PackageIds;
            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveCategoryDiassociationDetail(serviceRequest);
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
