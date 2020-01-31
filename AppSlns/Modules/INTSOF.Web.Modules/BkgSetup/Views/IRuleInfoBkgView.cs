using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IRuleInfoBkgView
    {
        IRuleInfoBkgView CurrentViewContext
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        int CurrentLoggedInUserId
        {
            get;

        }

        int SelectedRuleTemplateId
        {
            get;
            set;

        }

        List<BkgRuleTemplate> lstRuleTemplates
        {
            get;
            set;
        }

        BkgRuleTemplate RuleTemplateDetails
        {
            get;
            set;

        }

        BkgRuleMapping RuleMapping
        {
            get;
            set;
        }

        int RuleMappingId
        {
            get;
            set;
        }

        int RuleSetId
        {
            get;
            set;
        }

        Int32 ObjectCount
        {
            get;
            set;
        }

        List<lkpBkgRuleActionType> RuleActionTypes
        {
            set;
        }

        List<lkpBkgRuleType> RuleTypes
        {
            set;
        }
    }
}
