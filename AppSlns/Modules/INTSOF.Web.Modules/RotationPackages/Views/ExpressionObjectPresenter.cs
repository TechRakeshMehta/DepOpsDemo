using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.RotationPackages.Views
{
    public class ExpressionObjectPresenter : Presenter<IExpressionObjectView>
    {  
        public void GetRuleObjectMappingType()
        {
            View.lstRuleObjectMappingType = RequirementRuleManager.GetRuleObjectMappingType();
        }
    }
}
