using Business.RepoManagers;
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
    public class RequirementPackageDetailsPresenter : Presenter<IRequirementPackageDetailsView>
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

        /// <summary>
        /// 
        /// </summary>
        public void GetCategoryData()
        {
            if (View.CurrentPackageId > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.CurrentPackageId;
                ServiceResponse<Dictionary<Int32, String>> servicerResponse = _requirementPackageProxy.GetSharedRequirementCategoryData(serviceRequest);
                if (servicerResponse.Result.Count > AppConsts.NONE)
                {
                    View.lstCategoryDetails = servicerResponse.Result;
                }
            }
        }
    }
}
