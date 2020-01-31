using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface ISetupInstitutionHierarchyBkgView
    {
        ISetupInstitutionHierarchyBkgView CurrentViewContext { get; }
        List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> lstTreeData { set; get; }
        Int32 CurrentUserId { get; }
        Int32 TenantId { get; set; }
        Int32 NodeId { get; set; }
        List<Tenant> ListTenants { get; set; }
        Int32 SelectedTenant { get; set; }

        Boolean IsAvailableforOrder { get; set; }

        //UAT-3157
        Int32 PreferredSelectedTenantID { get; set; }

        Boolean IsPackageBundleAvailableforOrder { get; set; }
    }
}
