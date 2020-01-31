using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class TrackingNumberPresenter : Presenter<ITrackingNumberViewer>
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
       
        public void SaveTrackingNumber(int tenantId,int detailExtId,string trackingnum)
        {
            FingerPrintDataManager.SaveTrackingNumber(tenantId, detailExtId, trackingnum);
        }

        public void SendServiceStatusChangeMailMessage(string selectedStatus, string trackingNumber, Int32 orderId, Int32 userId, Int32 tenantId, string SerivceStatus)
        {
            Entity.OrganizationUser organizationUser = new Entity.OrganizationUser();
            organizationUser = GetUserData(userId);
            Entity.ClientEntity.Order orderDetails = new Entity.ClientEntity.Order();
            orderDetails = FingerPrintDataManager.GetOrderByOrderId(tenantId, orderId);
            CommunicationManager.SendServiceStatusChangeMailMessage(selectedStatus, organizationUser, tenantId, orderDetails, SerivceStatus, DateTime.Now, trackingNumber);
        }

        public Entity.OrganizationUser GetUserData(Int32 userId)
        {
            return SecurityManager.GetOrganizationUserDetailByOrganizationUserId(userId);
        }

        #endregion
    }
}
