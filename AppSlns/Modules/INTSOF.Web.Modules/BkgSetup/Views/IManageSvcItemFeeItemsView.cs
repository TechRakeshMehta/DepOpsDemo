using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageSvcItemFeeItemsView
    {
        IManageSvcItemFeeItemsView CurrentViewContext
        {
            get;
        }

        FeeItemContract ViewContract
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 currentLoggedInUserId
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String InfoMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        Int32 PackageServiceItemID { get; set; }

        List<LocalFeeItemsInfo> lstPackageServiceItemFee { get; set; }

        List<lkpServiceItemFeeType> lstServiceItemFeeTypes { get; set; }

        PackageServiceItemFee packageServiceItemFee { get; set; }

        Int32 FeeItemId { get; set; }

    }
}
