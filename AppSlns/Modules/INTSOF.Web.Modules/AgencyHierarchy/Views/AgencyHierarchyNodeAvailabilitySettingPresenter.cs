using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
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
    public class AgencyHierarchyNodeAvailabilitySettingPresenter : Presenter<IAgencyHierarchyNodeAvailabilitySettingView>
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
            // TODO: Implement code that will be executed the every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        #region UAT-4443
        public void GetAgencyHierachyNodeAvailabilitySettingExisted()
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID;
            serviceRequest.Parameter2 = AgencyHierarchySettingType.IS_NODE_AVAILABLE_FOR_ROTATION.GetStringValue();
            AgencyHierarchySettingContract result = _agencyHierarchyProxy.GetAgencyHierarchySetting(serviceRequest).Result;

            if (!result.IsNullOrEmpty())
                View.AgencyHierarchyNodeAvailabilitySettingContract = result;
            else
                View.AgencyHierarchyNodeAvailabilitySettingContract = new AgencyHierarchySettingContract();

            //if (result.AgencyHierarchyID > AppConsts.NONE)
            //{
            //    View.AgencyHierarchyNodeAvailabilitySettingContract = result;
            //    View.IsAgencyHierachyNodeAvailabilitySettingExisted = true;
            //}
            //else
            //{
            //    View.IsAgencyHierachyNodeAvailabilitySettingExisted = false;
            //}
            //View.IsRootNode = result.IsRootNode;
        }

        public Boolean SaveUpdateAgencyHierarchyNodeAvailabilitySetting(AgencyHierarchySettingContract agencyHierarchySettingContract)
        {
            ServiceRequest<AgencyHierarchySettingContract> serviceRequest = new ServiceRequest<AgencyHierarchySettingContract>();
            serviceRequest.Parameter = agencyHierarchySettingContract;
            Boolean result = _agencyHierarchyProxy.SaveAgencyHierarchySetting(serviceRequest).Result;
            if (result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.CHANGE_TYPE_AGENCY_HIERARCHY_SETTING, View.CurrentLoggedInUserId);
            }
            return result;
        }
        #endregion

    }
}
