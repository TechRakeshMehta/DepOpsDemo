using System;
using System.Collections.Generic;
using System.Text;
using Entity.SharedDataEntity;

namespace CoreWeb.RotationPackages.Views
{
    public interface IMasterRequirementRuleTemplateView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        List<RequirementRuleTemplate> RuleTemplates
        {
            get;
            set;
        }

        Int32 CurrentloggedinUserId
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }
        
        Int32 DefaultTenantId
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




