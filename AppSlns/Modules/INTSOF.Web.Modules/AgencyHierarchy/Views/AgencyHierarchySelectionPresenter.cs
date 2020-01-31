using System;
using System.Collections.Generic;
using System.Text;
#region Namespaces

#region SystemDefined
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;



#endregion

#endregion


namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchySelectionPresenter : Presenter<IAgencyHierarchySelectionView>
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

        public String GetAgencyHierarchyLabel()
        {
            ServiceRequest<Int32, Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32, Int32>();
            serviceRequest.Parameter1 = View.TenantId;
            serviceRequest.Parameter2 = View.AgencyId;
            serviceRequest.Parameter3 = View.NodeId;
            var _response = _agencyHierarchyProxy.GetAgencyHierarchyLabel(serviceRequest);
            return _response.Result;
        }
    }
}
