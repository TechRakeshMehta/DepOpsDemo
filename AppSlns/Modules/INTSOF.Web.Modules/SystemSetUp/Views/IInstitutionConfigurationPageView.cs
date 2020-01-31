using Entity.ClientEntity;
using System;
using System.Collections.Generic;

namespace CoreWeb.SystemSetUp.Views
{
    public interface IInstitutionConfigurationPageView
    {
        IInstitutionConfigurationPageView CurrentViewContext { get; }
        List<GetDepartmentTree> lstTreeData { set; get; }
        Int32 CurrentUserId { get; }
        Int32 TenantId { get; set; }
        Int32 NodeId { get; set; }
        List<Tenant> ListTenants { get; set; }
        Int32 SelectedTenant { get; set; }
    }
}
