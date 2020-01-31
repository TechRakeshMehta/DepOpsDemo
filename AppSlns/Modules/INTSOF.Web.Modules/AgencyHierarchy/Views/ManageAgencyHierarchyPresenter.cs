
using Business.RepoManagers;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.ServiceUtil;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class ManageAgencyHierarchyPresenter : Presenter<IManageAgencyHierarchyView>
    {

        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        public void GetAgencyHierarchyList()
        {
            View.GridCustomPaging.DefaultSortExpression = "AgencyHierarchyLabel";
            View.GridCustomPaging.SecondarySortExpression = "AgencyHierarchyID";
            //View.GridCustomPaging.PageSize = View.GridCustomPaging.VirtualPageCount;

            ServiceRequest<CustomPagingArgsContract> serviceRequest = new ServiceRequest<CustomPagingArgsContract>();
            serviceRequest.Parameter = View.GridCustomPaging;
            var _serviceResponse = _agencyHierarchyProxy.GetRootAgencyHierarchyData(serviceRequest);
            View.lstAgencyHierarchy = _serviceResponse.Result;

            if (!View.lstAgencyHierarchy.IsNullOrEmpty())
            {
                View.VirtualRecordCount = View.lstAgencyHierarchy.FirstOrDefault().VirtualPageCount;
                View.CurrentPageIndex = View.lstAgencyHierarchy.FirstOrDefault().CurrentPageIndex;
            }
        }

        public Boolean DeleteAgencyHierarchy()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.AgencyHierarchyId;
            var _serviceResponse = _agencyHierarchyProxy.DeleteRootAgencyHierarchy(serviceRequest);

            //UAT-2982:- Delete Mappings
            if (_serviceResponse.Result)
            {
                CallParallelTaskForDeletingAgencyHierarchyMappings();
            }
            return _serviceResponse.Result;
        }

        #region UAT-2982
       
        public void CallParallelTaskForDeletingAgencyHierarchyMappings()
        {
            Int32 agencyHierarchyId = View.AgencyHierarchyId;
            Int32 currentLoggedInUserID = View.CurrentLoggedInUserID;


            Dictionary<String, Object> Data = new Dictionary<String, Object>();
            Data.Add("agencyHierarchyId", agencyHierarchyId);
            Data.Add("currentLoggedInUserID", currentLoggedInUserID);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(DeletingAgencyHierarchyMappings, Data, LoggerService, ExceptiomService);

        }
        
        public void DeletingAgencyHierarchyMappings(Dictionary<String, Object> data)
        {
            Int32 currentLoggedInUserID = Convert.ToInt32(data.GetValue("currentLoggedInUserID"));
            Int32 agencyHierarchyId = Convert.ToInt32(data.GetValue("agencyHierarchyId"));

            if (agencyHierarchyId > AppConsts.NONE)
            {
                AgencyHierarchyManager.DeletingAgencyHierarchyMappings(agencyHierarchyId, currentLoggedInUserID);
            }
        }

        #endregion
        
    }
}
