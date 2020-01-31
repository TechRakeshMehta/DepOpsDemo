#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.CommonOperations.Views
{
    public interface IInstituteHierarchyPackageListView
    {
        List<GetDepartmentTree> lstTreeData { set; get; }
        Int32 CurrentUserId { get; }
        Int32 SelectedTenant { get; set; }

        Int32 TenantId { get; set; }
        String CompliancePackageTypeCode { get; set; }
        List<GetDepartmentTree> lstCurrentTreeData { get; set; }

        Boolean IsCompliancePackage { get; set; }
    }
}
