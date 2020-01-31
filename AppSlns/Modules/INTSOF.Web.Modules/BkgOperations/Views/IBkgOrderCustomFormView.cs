using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderCustomFormView
    {
        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        List<BkgOrderDetailCustomFormUserData> lstDataForCustomForm { get; set; }
        Int32 TenantId { get; set; }
        Int32 SelectedTenantID { get; }
        Int32 CustomFormID { get; set; }
        Int32 MasterOrderID { get; }
    }
}
