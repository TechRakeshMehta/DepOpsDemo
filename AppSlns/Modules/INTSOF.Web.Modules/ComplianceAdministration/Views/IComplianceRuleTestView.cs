using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IComplianceRuleTestView
    {
        IComplianceRuleTestView CurrentViewContext
        {
            get;
        }


        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        int RuleMappingID { get; set; }

        List<INTSOF.UI.Contract.ComplianceManagement.RuleDetailsForTestContract> ComplianceRuleData { get; set; }

        List<Entity.ClientEntity.lkpItemComplianceStatu> LstItemComplianceStatus { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 RuleActionTypeID { get; set; }
        
    }
}
