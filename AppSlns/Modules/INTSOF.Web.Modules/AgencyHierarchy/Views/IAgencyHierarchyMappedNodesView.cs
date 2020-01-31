using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyMappedNodesView
    {

        Int32 TenantId
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserID
        {
            get;
        }

        List<AgencyHierarchyDataContract> lstMappedNodes { get; set; }
        Int32 NodeId
        {
            get;
            set;
        }

        Int32 ParentNodeId
        {
            get;
            set;
        }

    }
}
