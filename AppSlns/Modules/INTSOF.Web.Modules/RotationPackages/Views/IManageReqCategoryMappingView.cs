using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IManageReqCategoryMappingView
    {
        IManageReqCategoryMappingView CurrentViewContext { get; }
        Int32 CurrentUserId { get; }
        List<RotationMappingContract> lstRotationMappingTreeData { get; set; }
        Int32 CurrentCategoryID { get; set; }
       Boolean IsDetailsEditable { get; set; }
    }
}
