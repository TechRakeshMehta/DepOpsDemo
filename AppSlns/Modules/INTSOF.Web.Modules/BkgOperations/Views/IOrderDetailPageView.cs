using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.BkgOperations.Views
{
    public interface IOrderDetailPageView
    {
        Int32 SelectedTenantId { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrderID { get; }
       
        #region UAT-806 Creation of granular permissions for Client Admin users

        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

        #region UAT-844
        Int32 ServiceGroupID { get; set; }
        String ServiceGroupName { set; }
        BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup { get; set; }
        Int32 orderPkgSvcGroupID { get; set; }
        List<lkpBkgSvcGrpReviewStatusType> lstServiceGroupReviewStatus { get; set; }
        #endregion
    }
}
