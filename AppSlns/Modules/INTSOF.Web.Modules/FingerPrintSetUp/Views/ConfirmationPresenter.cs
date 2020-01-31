using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ConfirmationPresenter: Presenter<IConfirmationViewer>
    {
        #region Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void SaveServiceStatus(int tenantId, int detailExtId, int serviceStatusID, bool IsServiceStatusRejected, Int32 CurrentUserId)
        {
            FingerPrintDataManager.SaveServiceStatus(tenantId, detailExtId, serviceStatusID, IsServiceStatusRejected, CurrentUserId);
        }

        public void SendServiceStatusChangeMailMessage(string selectedStatus, Int32 orderId, Int32 userId, Int32 tenantId, string serviceName)
        {
            Entity.OrganizationUser organizationUser = new Entity.OrganizationUser();
            organizationUser = GetUserData(userId);
            Entity.ClientEntity.Order orderDetails = new Entity.ClientEntity.Order();
            orderDetails = FingerPrintDataManager.GetOrderByOrderId(tenantId, orderId);
            CommunicationManager.SendServiceStatusChangeMailMessage(selectedStatus, organizationUser, tenantId, orderDetails, serviceName);
        }        

        public Entity.OrganizationUser GetUserData(Int32 userId)
        {
            return SecurityManager.GetOrganizationUserDetailByOrganizationUserId(userId);
        }

        #endregion
    }
}
