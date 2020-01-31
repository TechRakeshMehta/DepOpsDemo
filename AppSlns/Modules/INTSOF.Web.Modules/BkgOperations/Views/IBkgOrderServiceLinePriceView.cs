using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderServiceLinePriceView
    {
        Int32 SelectedTenantId { get; }
        Int32 OrderID { get; }
        Boolean IsRedirectedFromOrderQueueDetails { get; set; }//UAT-3481
    }
}
