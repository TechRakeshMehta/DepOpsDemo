using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IAddNewRequrementRule
    {
        List<lkpRuleActionType> RuleActionTypes
        {
            set;
        }

        List<lkpRuleType> RuleTypes
        {
            set;
        }

        IAddNewRequrementRule CurrentViewContext
        {
            get;
        }

        Int32 CurrentLoggedInUserId
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

        List<RequirementRuleTemplate> lstRuleTemplates { get; set; }

        Int32 CurrentCategoryID { get; set; }

        Int32 SelectedRuleTemplateId { get; set; }

        RequirementRuleTemplate RuleTemplateDetails { get; set; }

        Int32 RequirementObjectRuleMappingId { get; set; }

        Int32 SelectedRuleType { get; set; }

        Int32 SelectedActionType { get; set; }

        RequirementObjectRule RequirementObjectRule { get; set; }
        Int32 ObjectRuleID { get; set; }

        //UAT-4657
        Boolean IsDetailsEditable { get; set; }
    }
}
