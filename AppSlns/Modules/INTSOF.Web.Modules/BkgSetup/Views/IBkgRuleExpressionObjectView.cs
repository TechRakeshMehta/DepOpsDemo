using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IBkgRuleExpressionObjectView
    {
        IBkgRuleExpressionObjectView CurrentViewContext
        {
            get;
        }

        Int32 RowId
        {
            get;
            set;
        }

        String ObjectName
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }
    }
}
