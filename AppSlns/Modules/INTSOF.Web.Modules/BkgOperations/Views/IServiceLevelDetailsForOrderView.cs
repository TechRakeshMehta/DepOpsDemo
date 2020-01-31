using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public interface IServiceLevelDetailsForOrderView
    {
        Int32 SelectedTenantId { get; set; }
        Int32 OrderID { get; set; }
        List<ServiceLevelDetailsForOrderContract> LstServiceDetails { get; set; }
    }
}

