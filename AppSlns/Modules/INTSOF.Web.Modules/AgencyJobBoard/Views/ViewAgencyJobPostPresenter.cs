using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyJobBoard.Views
{
    public class ViewAgencyJobPostPresenter : Presenter<IViewAgencyJobPostView>
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
            GetSelectedJobPostDetails();
        }

        public void GetSelectedJobPostDetails()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.CurrentAgencyJobID;
            View.AgencyJobDetail = _agencyHierarchyProxy.GetSelectedJobPostDetails(serviceRequest).Result;
        }

        public void GetAgencyLogo()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<int>();
            serviceRequest.Parameter = View.AgencyJobDetail.AgencyHierarchyID;
            AgencyLogoContract agencyLogo = _agencyHierarchyProxy.GetAgencyLogo(serviceRequest).Result;
            View.AgencyJobDetail.LogoPath = agencyLogo.LogoPath;
        }

        public void CheckIfUserIsApplicant()
        {
            Boolean? isapplicant = SecurityManager.GetOrganizationUser(View.OrganisationUserID).IsApplicant;
            View.IsApplicant = isapplicant.HasValue ? isapplicant.Value : false;
        }
    }
}
