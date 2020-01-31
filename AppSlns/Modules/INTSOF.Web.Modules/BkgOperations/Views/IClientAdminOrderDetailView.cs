using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IClientAdminOrderDetailView
    {
        Int32 SelectedTenantId { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrderID { get; }
        #region UAT-844
        Int32 OrderPkgSvcGroupID { get; set; }
        String ParentScreenName { get; set; }
        #endregion
        #region UAT-1075
        Boolean IsBkgColorFlagDisable { get; set; }
        #endregion
    }
}

