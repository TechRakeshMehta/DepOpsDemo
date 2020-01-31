using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class RenewalOrderOptionsPresenter : Presenter<IRenewalOrderOptions>
    {
        #region OVERIDED METHODS
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Get total months 
        /// </summary>
        /// <param name="NoOfYears"></param>
        /// <param name="NoOfMonths"></param>
        /// <returns></returns>
        private Int32 GetTotalNumOfMonths(Int32? NoOfYears, Int32? NoOfMonths)
        {
            Int32 totalNumOfMonths = AppConsts.NONE;
            if (NoOfYears.IsNotNull())
            {
                totalNumOfMonths = NoOfYears.Value * 12;
            }
            if (NoOfMonths.IsNotNull())
            {
                totalNumOfMonths = totalNumOfMonths + NoOfMonths.Value;
            }
            return totalNumOfMonths;
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Get Order Details
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> GetOrderDetails()
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();

            Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());
            Order orderDetail = ComplianceDataManager.GetOrderById(View.CurrentUserTenantID, View.OrderID);
            if (orderDetail.IsNotNull() && orderDetail.PackageSubscriptions.IsNotNull())
            {
                dic.Add("ExpiryDate", orderDetail.PackageSubscriptions.LastOrDefault().ExpiryDate.ToString());
                dic.Add("PackagePrice", Convert.ToString(orderDetail.TotalPrice ?? 0));
                dic.Add("TotalMonthsInPackage", Convert.ToString(GetTotalNumOfMonths(orderDetail.SubscriptionYear,
                                                orderDetail.SubscriptionMonth)));
            }
            return dic;
        }
        #endregion
    }
}
