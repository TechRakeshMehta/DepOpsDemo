using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public class OrderNotificationHistoryPresenter : Presenter<IOrderNotificationHistoryView>
    {
        public void GetOrderNotificationHistory()
        {
            View.OrderNotificationHistoryContract = BackgroundProcessOrderManager.GetOrderNotificationHistory(View.MasterOrderID, View.SelectedTenantID);
        }

        public void GetHistoryByOrderNotificationId(Int32 orderNotificationId)
        {
            View.lstNotificationHistory = BackgroundProcessOrderManager.GetHistoryByOrderNotificationId(orderNotificationId, View.SelectedTenantID);
        }

        public Boolean UpdateBkgOrderServiceFormStatus(Int32 orderNotificationId, Int32 statusId, Int32 oldStatusId)
        {
            return BackgroundProcessOrderManager.UpdateOrderNotificationBkgOrderServiceForm(orderNotificationId, View.SelectedTenantID, View.loggedInUserId, statusId, oldStatusId);
        }

        public Boolean ResendOrderNotification(Int32 orderNotificationId, Int32 systemCommunicationId)
        {
            Int32 applicantId = BackgroundProcessOrderManager.GetOrganisationUserProfileByOrderId(View.SelectedTenantID, View.MasterOrderID).OrganizationUserID;
            return MessageManager.ResendOrderNotification(orderNotificationId, applicantId, View.SelectedTenantID, View.loggedInUserId, systemCommunicationId);
        }

        public Boolean ResendOrderCompletedNotification(Int32 orderNotificationId, Int32 orderId, Int32 hierarchyNodeID, Int32 svcGroupID, String svcGrpName)
        {
            //UAT-3453
            Boolean isOrderFlagged = BackgroundProcessOrderManager.IsBkgOrderFlagged(View.SelectedTenantID, View.MasterOrderID);

            Entity.ClientEntity.OrganizationUserProfile orgUserProfile = BackgroundProcessOrderManager.GetOrganisationUserProfileByOrderId(View.SelectedTenantID, View.MasterOrderID);
            return MessageManager.ResendOrderCompletedNotification(orderId, orderNotificationId, orgUserProfile, View.SelectedTenantID, View.loggedInUserId, hierarchyNodeID, svcGroupID, svcGrpName, View.OrderNumber, isOrderFlagged);
        }

        /// <summary>
        /// Send notification when a manual service form status has been changed from send to student to in progress by agency.
        /// </summary>
        /// <param name="oldServiceFormStatusId"></param>
        /// <param name="newServiceFormStatusId"></param>
        /// <param name="manualServiceFormContract"></param>
        public void SendSvcFormStsChangeNotification(Int32 oldServiceFormStatusId, Int32 newServiceFormStatusId, ManualServiceFormContract manualServiceFormContract)
        {
            BackgroundProcessOrderManager.SendSvcFormStsChangeNotification(View.SelectedTenantID, oldServiceFormStatusId, newServiceFormStatusId, manualServiceFormContract);
        }
    }
}
