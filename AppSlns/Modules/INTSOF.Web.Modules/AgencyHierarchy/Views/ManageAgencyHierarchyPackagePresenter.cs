using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class ManageAgencyHierarchyPackagePresenter : Presenter<IManageAgencyHierarchyPackageView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
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

        public void GetRequiremetPackages(RequirementPackageContract requirementPackageContract)
        {
            List<RequirementPackageContract> lstRequirementPackageDetails = new List<RequirementPackageContract>();
            lstRequirementPackageDetails = _agencyHierarchyProxy.GetRequirementPackages().Result;
            if (!requirementPackageContract.IsNullOrEmpty() && requirementPackageContract.RequirementPackageID > 0)
            {
                View.lstAgencyHierarchyPackages.Remove(View.lstAgencyHierarchyPackages.Where(cond => cond.RequirementPackageID == requirementPackageContract.RequirementPackageID).FirstOrDefault());
            }
            View.lstRequirementPackage = lstRequirementPackageDetails.Where(cond => !View.lstAgencyHierarchyPackages.Select(sel => sel.RequirementPackageID).Contains(cond.RequirementPackageID)).ToList();
        }

        public void GetAgencyHirarchyPackages()
        {
            List<RequirementPackageContract> lstAgencyHierarchyPackageList = new List<RequirementPackageContract>();

            ServiceRequest<CustomPagingArgsContract, Int32> serviceRequest = new ServiceRequest<CustomPagingArgsContract, Int32>();
            serviceRequest.Parameter1 = View.GridCustomPaging;
            serviceRequest.Parameter2 = View.AgencyHierarchyId;
            lstAgencyHierarchyPackageList = _agencyHierarchyProxy.GetAgencyHierarchyPackages(serviceRequest).Result;
            if (!lstAgencyHierarchyPackageList.IsNullOrEmpty())
            {
                if (lstAgencyHierarchyPackageList[0].TotalCount > 0)
                {
                    View.VirtualRecordCount = lstAgencyHierarchyPackageList[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }
            View.lstAgencyHierarchyPackages = lstAgencyHierarchyPackageList;
        }

        public Boolean SaveUpdateAgencyHierarchyPackageMapping()
        {
            ServiceRequest<AgencyHierarchyPackageContract> serviceRequest = new ServiceRequest<AgencyHierarchyPackageContract>();
            serviceRequest.Parameter = View.agencyHierarchyPackageContract;
            Boolean result = _agencyHierarchyProxy.SaveAgencyHierarchyPackageMapping(serviceRequest).Result;
           
            if (result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyId.ToString(), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }
            return result;
        }

        public Boolean DeleteAgencyHierarchyPackageMapping()
        {
            ServiceRequest<AgencyHierarchyPackageContract> serviceRequest = new ServiceRequest<AgencyHierarchyPackageContract>();
            serviceRequest.Parameter = View.agencyHierarchyPackageContract;
            Boolean result = _agencyHierarchyProxy.DeleteAgencyHierarchyPackageMapping(serviceRequest).Result;

            if (result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyId.ToString(), AppConsts.CHANGE_TYPE_PACKAGE, View.CurrentLoggedInUserId);
            }
            return result;
        }
        #region UAT-4657
        public String IsPackageVersionInProgress(Int32 PkgId)
        {
            return AgencyHierarchyManager.IsPackageVersionInProgress(PkgId);
        }
        #endregion
    }
}
