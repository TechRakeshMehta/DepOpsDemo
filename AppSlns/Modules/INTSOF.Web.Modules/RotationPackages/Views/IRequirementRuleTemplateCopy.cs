using System;
using System.Collections.Generic;

namespace CoreWeb.RotationPackages.Views
{
    public interface IRequirementRuleTemplateCopy
    {
        Int32 CurrentLoggedInUserId { get; }
        IRequirementRuleTemplateCopy CurrentViewContext { get; }
        String RuleTemplateName { get; set; }
        Int32 SelectedRuleTemplateID { get; set; }
        String ErrorMessage { get; set; }
    }
}
