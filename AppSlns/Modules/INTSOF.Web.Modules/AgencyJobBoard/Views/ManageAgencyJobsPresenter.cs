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
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.AgencyJobBoard.Views
{
    public class ManageAgencyJobsPresenter : Presenter<IManageAgencyJobsView>
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
            GetAgencyHierarchyId();
            GetAgencyLogo();
        }

        public override void OnViewLoaded()
        {

        }

        public void GetAgencyJobTemplate()
        {
            View.LstAgencyJobTemplate = new List<AgencyJobContract>();
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.OrganisationUserID;
            View.LstAgencyJobTemplate = _agencyHierarchyProxy.GetAgencyJobTemplate(serviceRequest).Result;
        }
       
        public void GetJobFieldType()
        {
            View.LstJobFieldType = new List<DefinedRequirementContract>();
            View.LstJobFieldType = _agencyHierarchyProxy.GetJobFieldType().Result;
        }

        public Boolean SaveAgencyJobTemplate()
        {
            ServiceRequest<AgencyJobContract, Int32> serviceRequest = new ServiceRequest<AgencyJobContract, Int32>();
            serviceRequest.Parameter1 = View.AgencyJob;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            return _agencyHierarchyProxy.SaveAgencyJobTemplate(serviceRequest).Result;
        }

        public Boolean DeleteAgencyJobTemplate()
        {
            ServiceRequest<AgencyJobContract, Int32> serviceRequest = new ServiceRequest<AgencyJobContract, Int32>();
            serviceRequest.Parameter1 = View.AgencyJob;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            return _agencyHierarchyProxy.DeleteAgencyJobTemplate(serviceRequest).Result;
        }

        public Int32 GetAgencyHierarchyId()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<int>();
            serviceRequest.Parameter = View.OrganisationUserID;
            View.MappedAgencyHierarchyRootNodeId = _agencyHierarchyProxy.GetAgencyHierarchyId(serviceRequest).Result;
            return View.MappedAgencyHierarchyRootNodeId;
        }

        public void GetAgencyJobPosting()
        {
            View.LstAgencyJobPosting = new List<AgencyJobContract>();
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.OrganisationUserID;
            View.LstAgencyJobPosting = _agencyHierarchyProxy.GetAgencyJobPosting(serviceRequest).Result;
        }

        public Boolean SaveAgencyJobPosting()
        {
            ServiceRequest<AgencyJobContract, Int32> serviceRequest = new ServiceRequest<AgencyJobContract, Int32>();
            serviceRequest.Parameter1 = View.AgencyJob;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            return _agencyHierarchyProxy.SaveAgencyJobPosting(serviceRequest).Result;
        }

        public Boolean DeleteAgencyJobPosting()
        {
            ServiceRequest<AgencyJobContract, Int32> serviceRequest = new ServiceRequest<AgencyJobContract, Int32>();
            serviceRequest.Parameter1 = View.AgencyJob;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            return _agencyHierarchyProxy.DeleteAgencyJobPosting(serviceRequest).Result;
        }

        public AgencyJobContract GetTemplateDetailsByID(Int32 SelectedTemplateID)
        {
            AgencyJobContract templateDetails = new AgencyJobContract();
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = SelectedTemplateID;
            return _agencyHierarchyProxy.GetTemplateDetailsByID(serviceRequest).Result;
        }

        public Boolean SaveUpdateAgencyLogo()
        {
            AgencyLogoContract agencyLogoContract = new AgencyLogoContract();
            agencyLogoContract.LogoPath = View.FilePath;
            agencyLogoContract.AgencyHierarchyID = View.MappedAgencyHierarchyRootNodeId;

            if (!View.AgencyLogo.IsNullOrEmpty())
                agencyLogoContract.AgencyLogoID = View.AgencyLogo.AgencyLogoID;

            ServiceRequest<AgencyLogoContract, Int32> serviceRequest = new ServiceRequest<AgencyLogoContract, int>();
            serviceRequest.Parameter1 = agencyLogoContract;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            if (_agencyHierarchyProxy.SaveUpdateAgencyLogo(serviceRequest).Result)
            {

            }
            return true;
        }

        public void GetAgencyLogo()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<int>();
            serviceRequest.Parameter = View.MappedAgencyHierarchyRootNodeId;
            View.AgencyLogo = _agencyHierarchyProxy.GetAgencyLogo(serviceRequest).Result;
        }

        public Boolean ArchiveJobPosts()
        {
            ServiceRequest<List<Int32>, Int32> serviceRequest = new ServiceRequest<List<Int32>, Int32>();
            serviceRequest.Parameter1 = View.SelectedAgencyPostIDs;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            return _agencyHierarchyProxy.ArchiveJobPosts(serviceRequest).Result;
        }

        public Boolean ClearLogo()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.MappedAgencyHierarchyRootNodeId;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            return _agencyHierarchyProxy.ClearLogo(serviceRequest).Result;
        }

    }
}
