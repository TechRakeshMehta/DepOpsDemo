using Entity;
using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IRuleTemplateCopyView
    {
        Int32 CurrentLoggedInUserId { get; }
        IRuleTemplateCopyView CurrentViewContext { get; }
        String RuleTemplateName { get; set; }
        Int32 FromTenantID { get; set; }
        Int32 ToTenantID { get; set; }
        Int32 SelectedRuleTemplateID { get; set; }
        String ErrorMessage { get; set; }
        List<Entity.Tenant> ListTenants { get; set; }
        Int32 DefaultTenantId { get; set; }
    }
}
