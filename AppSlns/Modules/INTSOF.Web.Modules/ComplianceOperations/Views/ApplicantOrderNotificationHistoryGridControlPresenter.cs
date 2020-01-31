using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ApplicantOrderNotificationHistoryGridControlPresenter : Presenter<IApplicantOrderNotificationHistoryGridControl>
    {
        public void GetApplicantOrderNotificationHistory()
        {
            View.OrderNotificationHistoryContract = BackgroundProcessOrderManager.GetApplicantSpecificOrderNotificationHistory(View.OrganizationUserId, View.SelectedTenantID);
        } 
        public Boolean ResendOrderNotification(Int32 orderNotificationId, Int32 systemCommunicationId)
        {
            return MessageManager.ResendOrderNotification(orderNotificationId, View.OrganizationUserId, View.SelectedTenantID, View.loggedInUserId, systemCommunicationId);
        } 
    }
}
