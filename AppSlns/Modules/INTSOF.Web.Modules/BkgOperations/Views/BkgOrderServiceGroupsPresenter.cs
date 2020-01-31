using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System.Linq;
using System.Configuration;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderServiceGroupsPresenter : Presenter<IBkgOrderServiceGroupsView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            if (Business.RepoManagers.SecurityManager.DefaultTenantID == View.TenantId)
            {
                View.IsAdminUser = true;

            }
            else
            {
                View.IsAdminUser = false;
            }

            IsAdminCreatedOrder();
        }

        public void GetServiceGroupDetails()
        {
            if (View.SelectedTenantId > 0)
            {
                View.lstServiceGrpDetails = BackgroundProcessOrderManager.GetServiceGroupDetails(View.SelectedTenantId, View.OrderID);
            }
        }

        public void GetClientAdminGranularPermission()
        {

            if (View.CurrentLoggedInUserId > AppConsts.NONE && !View.IsAdminUser)
            {
                View.lstGranularPermission = ComplianceDataManager.GeNewtGranularPermission(View.TenantId, View.CurrentLoggedInUserId);
            }
        }
        public OrganizationUser GetOrganizationUserByOrderID()
        {
            return BackgroundProcessOrderManager.GetOrganisationUserByOrderId(View.SelectedTenantId, View.OrderID);

        }

        public Int32 CheckIfOrderCompleteNotificationExistsByOrderID(Int32? packageServiceGroupID)
        {
            return BackgroundProcessOrderManager.CheckIfOrderCompleteNotificationExistsByOrderID(View.SelectedTenantId, View.OrderID, OrderNotificationType.ORDER_RESULT.GetStringValue(), packageServiceGroupID);
        }

        public Boolean SendOrderCompletionNotificationMail(OrganizationUser organizationUser, Int32 orderNotificationID, Int32 hierarchyNodeID, Int32? svcGroupID, Int32? packageServiceGroupID, String svcGroupName, Boolean isClient, Boolean isEmployement, Int32 studenthierarchyNodeID, Boolean isOrderFlagged)
        {
            return BackgroundProcessOrderManager.SendOrderCompleteResultMail(View.SelectedTenantId, organizationUser, View.OrderID, orderNotificationID, hierarchyNodeID, View.CurrentLoggedInUserId, svcGroupID, packageServiceGroupID, svcGroupName, isClient, isEmployement, studenthierarchyNodeID, isOrderFlagged);
        }

        public Boolean UpdateOrderNotification(Int32 orderNotificationId)
        {
            return BackgroundProcessOrderManager.UpdateOrderNotificationBkgOrderServiceForm(orderNotificationId, View.TenantId, View.CurrentLoggedInUserId);
        }


        public void GetGranularPermissionForClientAdmins()
        {
            //View.IsBkgOrderPdfVisible = false;
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (Business.RepoManagers.SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                //UAT-1075
                //if (dicPermissions.ContainsKey(EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()) && dicPermissions[EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                //{
                //    View.IsBkgOrderPdfVisible = true;
                //}

                if (dicPermissions.ContainsKey(EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()))
                {
                    View.LstBkgOrderResultPermissions = dicPermissions[EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()].Split(',').ToList();
                }
            }
        }

        /// <summary>
        /// UAT 1740: Move 604 notification from the time of login to when an admin attempts for view an employment result report. 
        /// </summary>
        /// <returns></returns>
        public Boolean IsEDFormPreviouslyAccepted()
        {
            Double employmentDisclosureIntervalHours = AppConsts.NONE;
            if (!ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"].IsNullOrEmpty())
            {
                employmentDisclosureIntervalHours = Convert.ToDouble(ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"]);
            }
            return Business.RepoManagers.SecurityManager.IsEDFormPreviouslyAccepted(View.CurrentLoggedInUserId, employmentDisclosureIntervalHours);
        }

        //UAT-2842:

        public void IsAdminCreatedOrder()
        {
           View.IsAdminCreatedOrder =  BackgroundProcessOrderManager.IsAdminCreatedOrder(View.SelectedTenantId, View.OrderID);
        }
    }
}
