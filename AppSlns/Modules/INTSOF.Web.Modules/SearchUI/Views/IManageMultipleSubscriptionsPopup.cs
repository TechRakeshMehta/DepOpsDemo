using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;

namespace CoreWeb.SearchUI.Views
{
    public interface IManageMultipleSubscriptionsPopup
    {

        //List of PackageSusbcriptionIDs
        List<Int32> MultpleSubscriptionIDs
        {
            get;
            set;
        }

        List<Int32> SingleSubscriptionIDs
        {
            get;
            set;
        }

        //Get SelectedTeanntID
        Int32 SelectedTenantID
        {
            get;
            set;
        }

        //Get-set Current User ID
        Int32 CurrentLoggedInUserID
        {
            get;
            set;
        }

        //Get-Set data in lstMultipleSubscriptionsData
        List<ManageMultipleSubscriptionContract> lstMultipleSubscriptionsData
        {
            get;
            set;
        }

        List<Int32> SelectedSubscriptions { get; set; }

        String ErrorMessage { get; set; }
    }
}
