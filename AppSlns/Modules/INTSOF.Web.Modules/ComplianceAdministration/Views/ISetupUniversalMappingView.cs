using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISetupUniversalMappingView
    {
        ISetupUniversalMappingView CurrentViewContext { get; }
        Int32 CurrentUserId { get; }
        List<UniversalMappingContract> lstTreeData { get; set; }
    }
}
