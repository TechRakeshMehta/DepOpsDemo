using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderClientStatusPresenter : Presenter<IBkgOrderClientStatusView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {

        }
        /// <summary>
        /// Get the Client status 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public List<Entity.ClientEntity.BkgOrderClientStatu> GetBkgOrderClientStatus(Int32 tenantId)
        {
            return BackgroundProcessOrderManager.GetBkgOrderClientStatus(tenantId);
        }
        /// <summary>
        /// Update the Client status 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderId"></param>
        /// <param name="clientstatusId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public Boolean UpdateOrderClientStatus(Int32 tenantId, Int32 orderId, Int32 clientstatusId, Int32 currentLoggedInUserId)
        {
            if (clientstatusId > 0)
            {
                return BackgroundProcessOrderManager.UpdateOrderClientStatus(tenantId, orderId, clientstatusId, currentLoggedInUserId);
            }
            return false;
        }
        /// <summary>
        /// Save the Notes in order status event history table
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderId"></param>
        /// <param name="Notes"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public Boolean SaveOrderClientStatusHistory(Int32 tenantId, Int32 orderId, String Notes, Int32 currentLoggedInUserId)
        {
            return BackgroundProcessOrderManager.SaveOrderClientStatusHistory(tenantId, orderId, Notes, currentLoggedInUserId);
        }
        /// <summary>
        /// Get the Client order Status history records
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<Entity.ClientEntity.BkgOrderClientStatusHistory> GetClientOrderStatusHistory(Int32 tenantId, Int32 orderId)
        {
            return BackgroundProcessOrderManager.GetClientOrderStatusHistory(tenantId, orderId);
        }
    }
}
