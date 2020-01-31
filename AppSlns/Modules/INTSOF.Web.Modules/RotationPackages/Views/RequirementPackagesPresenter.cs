using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.SharedObjects;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;



namespace CoreWeb.RotationPackages.Views
{
    public class RequirementPackagesPresenter : Presenter<IRequirementPackagesView>
    {

        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public void GetRequirementPackages()
        {
            if (View.GridCustomPaging.DefaultSortExpression.IsNullOrEmpty())
            {
                View.GridCustomPaging.DefaultSortExpression = "RequirementPackageName";
                View.GridCustomPaging.SecondarySortExpression = "RequirementPackageID";
                View.GridCustomPaging.SortDirectionDescending = false;
            }
            List<RequirementPackageContract> lstRequirementPackages = new List<RequirementPackageContract>();
            ServiceRequest<List<Int32>, Int32, CustomPagingArgsContract> serviceRequest = new ServiceRequest<List<Int32>, Int32, CustomPagingArgsContract>();
            serviceRequest.Parameter1 = View.LstAgencyHeirarchyIds;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = View.GridCustomPaging;
            lstRequirementPackages = _requirementPackageProxy.GetRequirementPackagesByHierarcyIds(serviceRequest).Result;
            View.lstRequirementPackage = lstRequirementPackages;
            if (!View.lstRequirementPackage.IsNullOrEmpty())
            {
                View.VirtualRecordCount = View.lstRequirementPackage.FirstOrDefault().TotalCount;
                View.CurrentPageIndex = View.lstRequirementPackage.FirstOrDefault().PageIndex;
            }
        }
    }
}
