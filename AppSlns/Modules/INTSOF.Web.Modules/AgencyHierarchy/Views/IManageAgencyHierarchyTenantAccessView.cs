using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IManageAgencyHierarchyTenantAccessView
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

        List<TenantDetailContract> lstTenant
        {
            set;
        }

        Int32 NodeId
        {
            get;
            set;
        }
        List<Int32> lstSelectedTenantIds
        {
            get;
            set;
        }
        List<Int32> lstSelectedTenantPrevsIds
        {
            get;
            set;
        }
    }
}
