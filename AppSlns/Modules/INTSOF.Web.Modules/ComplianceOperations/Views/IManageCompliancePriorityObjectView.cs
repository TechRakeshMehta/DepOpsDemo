using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageCompliancePriorityObjectView
    {
        List<CompliancePriorityObjectContract> lstCompPriorityObject { get; set; }
        Int32 CurrentLoggedInUserID { get; }
    }
}
