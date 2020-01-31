using Business.RepoManagers;
using Entity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyJobBoard.Views
{
    public class ViewAgencyJobPostingPresenter : Presenter<IViewAgencyJobPostingView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            CheckIfUserIsApplicant();
        }

        public override void OnViewLoaded()
        {

        }


        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public void GetAgencyJobPosting()
        {
            JobSearchContract searchContract = GetJobSearchContract();

            ServiceRequest<JobSearchContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<JobSearchContract, CustomPagingArgsContract>();
            serviceRequest.Parameter1 = searchContract;
            serviceRequest.Parameter2 = View.GridCustomPaging;
            View.LstAgencyJobPosting = _agencyHierarchyProxy.GetViewAgencyJobPosting(serviceRequest).Result;
            if (!View.LstAgencyJobPosting.IsNullOrEmpty())
            {
                View.VirtualRecordCount = View.LstAgencyJobPosting.FirstOrDefault().TotalCount;
            }
            //View.LstAgencyJobPosting = new List<AgencyJobContract>();
        }
        // UAT-3071
        public void GetViewAgencyJobFieldType()
        {
            View.LstJobFieldType = new List<DefinedRequirementContract>();
            //ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            //serviceRequest.Parameter = View.OrganisationUserID;
            View.LstJobFieldType = _agencyHierarchyProxy.GetJobFieldType().Result; 
        }

        private JobSearchContract GetJobSearchContract()
        {
            JobSearchContract contract = new JobSearchContract();
            contract.JobTitle = View.JobTitle;
            contract.JobTypeCode = View.JobTypeCode;
            contract.Company = View.Company;
            contract.Location = View.Location;
            contract.OrganizationUserId = View.OrganisationUserID;
            contract.JobFieldTypeID = View.SelectedJobFieldTypeID;
            IsAdminLoggedIn();
            if (View.IsAdminLoggedIn)
            {
                contract.TenantId = string.Join(",", View.SelectedTenantIds);
            }
            else
            {
                contract.TenantId = Convert.ToString(View.TenantId);

            }
            return contract;
        }

        public void CheckIfUserIsApplicant()
        {
            Boolean IsAppliacnt = false;
            if (View.OrganisationUserID > AppConsts.NONE)
            {
                var OrgUser = SecurityManager.GetOrganizationUser(View.OrganisationUserID);
                if (!OrgUser.IsNullOrEmpty())
                    IsAppliacnt = OrgUser.IsApplicant.HasValue ? OrgUser.IsApplicant.Value : false;
            }
            View.IsAppliacnt = IsAppliacnt;
        }

        //public void IsClientAdmin()
        //{
        //    if (IntegrityManager.IsClientAdmin(View.TenantId))
        //    {
        //        View.IsClientAdmin = true;
        //    }
        //}

        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _agencyHierarchyProxy.GetTenants(serviceRequest);

            if (!_serviceResponse.Result.IsNullOrEmpty())
            {
                View.LstTenant = _serviceResponse.Result;
            }
            else
            {
                View.LstTenant = new List<TenantDetailContract>();
            }
        }
    }
}
