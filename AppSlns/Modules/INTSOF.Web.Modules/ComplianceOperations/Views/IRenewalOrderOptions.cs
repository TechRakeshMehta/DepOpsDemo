using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IRenewalOrderOptions
    {
        Int32 OrderID { get; set; }
        Int32 CurrentUserTenantID { get; set; }
        Dictionary<String, String> DicOrderDetails { get; set; }
    }
}
