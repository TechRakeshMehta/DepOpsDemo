using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface ISetupRuleTemplateBkgView
    {
        Int32 TenantId { get; set; }
        List<BkgRuleTemplate> RuleTemplates { get; set; }
        List<lkpBkgRuleResultType> RuleResultTypes { get; set; }
        Int32 currentloggedinuserId { get; }

        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> ListTenants
        {
            set;
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

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
    }
}
