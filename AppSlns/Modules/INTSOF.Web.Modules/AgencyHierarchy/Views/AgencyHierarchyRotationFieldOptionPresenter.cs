using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchyRotationFieldOptionPresenter : Presenter<IAgencyHierarchyRotationFieldOptionView>
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

        public void IsAgencyHierarchyRotationFieldOptionSettingSaved()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.AgencyHierarchyID;
            AgencyHierarchyRotationFieldOptionContract result = _agencyHierarchyProxy.GetAgencyHierarchyRotationFieldOptionSetting(serviceRequest).Result;

            if (result.AgencyHierarchyID > AppConsts.NONE)
            {
                View.IsRotationFieldOptionSettingExisted = true;
                View.AgencyHierarchyRotationFieldOptionContract = result;
            }
            else
            {
                View.IsRotationFieldOptionSettingExisted = false;
            }
            View.IsRootNode = result.IsRootNode;
        }

        public Boolean SaveUpdateAgencyHierarchyRotationFieldOption(AgencyHierarchyRotationFieldOptionContract agencyHierarchyRotationFieldOptionContract)
        {
            ServiceRequest<AgencyHierarchyRotationFieldOptionContract> serviceRequest = new ServiceRequest<AgencyHierarchyRotationFieldOptionContract>();
            serviceRequest.Parameter = agencyHierarchyRotationFieldOptionContract;
            Boolean result = _agencyHierarchyProxy.SaveAgencyHierarchyRotationFieldOptionSetting(serviceRequest).Result;
            return result;
        }
    }
}
