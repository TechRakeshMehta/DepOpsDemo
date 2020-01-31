using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public class ClientAdminOrderDetailPresenter : Presenter<IClientAdminOrderDetailView>
    {
        public OrderDetailHeaderInfo GetOrderDetailHeaderInfo()
        {
            return BackgroundProcessOrderManager.GetOrderDetailHeaderInfo(View.SelectedTenantId, View.OrderID);
        }

        #region UAT-1075 WB: WB:Admin Granular permissions for color flag and Result PDF
        public void GetGranularPermissionForClientAdmins()
        {
            View.IsBkgColorFlagDisable = false;
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (Business.RepoManagers.SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                //UAT-1075
                if (dicPermissions.ContainsKey(EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue()) && dicPermissions[EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsBkgColorFlagDisable = true;
                }
            }
        }
        #endregion
    }
}
