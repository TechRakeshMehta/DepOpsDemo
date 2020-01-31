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
    public class RequirementItemDetailsPresenter : Presenter<IRequirementItemDetailsViews>
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
        }

        public void GetItemAttributeDetails()
        {
            if (View.CurrentCategoryId > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.CurrentCategoryId;
                ServiceResponse<List<RequirementItemContract>> servicerResponse = _requirementPackageProxy.GetRequirementItemDetailsByCategoryId(serviceRequest);
                if (servicerResponse.Result.Count > AppConsts.NONE)
                {
                    View.lstItemDetails = servicerResponse.Result;
                }
            }
        }

        //UAT-2795//
        public Boolean GetCategoryDocumentLink()
        {
            if (View.CurrentCategoryId > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.CurrentCategoryId;
                ServiceResponse<String> servicerResponse = _requirementPackageProxy.GetCategoryDocumentLink(serviceRequest);
                if (!servicerResponse.Result.IsNullOrEmpty())
                {
                    View.CategoryDocumentLink = servicerResponse.Result;
                    return true;
                }
            }
            return false;
        }


        public Boolean GetCategoryExplanatoryNotes()
        {
            if (View.CurrentCategoryId > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.CurrentCategoryId;
                ServiceResponse<String> servicerResponse = _requirementPackageProxy.GetCategoryExplanatoryNotes(serviceRequest);
                if (!servicerResponse.Result.IsNullOrEmpty())
                {
                    View.CategoryExplanatoryNotes = servicerResponse.Result;
                }
                return true;
            }
            return false;
        }

        #region UAT-4254
        public void GetRequirementCatDocUrls()
        {
            View.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
            if (View.CurrentCategoryId > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.CurrentCategoryId;
                ServiceResponse<List<RequirementCategoryDocUrl>> serviceResponse = _requirementPackageProxy.GetRequirementCatDocUrls(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
                    View.lstReqCatDocUrls = serviceResponse.Result.ToList();
                }
            }
        }
        #endregion
    }
}
