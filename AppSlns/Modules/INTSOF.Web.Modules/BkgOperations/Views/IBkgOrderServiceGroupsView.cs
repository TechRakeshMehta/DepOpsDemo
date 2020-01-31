using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderServiceGroupsView
    {
        Int32 SelectedTenantId { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrderID { get; }
        List<OrderServiceGroupDetails> lstServiceGrpDetails { get; set; }
        List<GranularPermission> lstGranularPermission { get; set; }
        Boolean IsAdminUser
        {
            get;
            set;
        }
        Int32 TenantId
        {
            get;
            set;
        }
        Int32 loggedInUserId { get; }

        //Boolean IsBkgOrderPdfVisible { get; set; }

        List<String> LstBkgOrderResultPermissions
        {
            get;
            set;
        }

        //UAT-2842:
        Boolean IsAdminCreatedOrder { get; set; }

        //UAT-3481
        Boolean IsRedirectedFromOrderQueueDetails { get; set; }
    }
}
