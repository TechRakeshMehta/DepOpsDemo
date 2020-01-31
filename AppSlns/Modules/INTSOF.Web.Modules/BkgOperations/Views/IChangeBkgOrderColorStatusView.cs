using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IChangeBkgOrderColorStatusView
    {
        Int32 OrderID
        {
            get;
            set;
        }

        Int32 CurrentUserId
        {
            get;
        }

        Int32 TenantID
        {
            get;
            set;
        }

        Int32 SelectedColorFlag
        {
            get;
            set;
        }
    }
}
