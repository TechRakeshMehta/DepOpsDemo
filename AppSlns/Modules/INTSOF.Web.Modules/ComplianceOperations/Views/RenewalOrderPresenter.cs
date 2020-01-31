#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class RenewalOrderPresenter : Presenter<IRenewalOrderView>
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            ShowRushOrderSetting();
        }

        public void GetOrderDetail()
        {
            Int32 totalSubscriptionYear;
            Int32 totalSubscriptionMonth;
            Order orderDetail = ComplianceDataManager.GetOrderById(View.TenantId, View.OrderId);
            List<Order> orderList = ComplianceDataManager.GetOrderListOfPreviousOrder(View.TenantId, orderDetail);
            if (orderList.Count > 0)
            {
                totalSubscriptionYear = Convert.ToInt32(orderList.Where(cnd => cnd.SubscriptionYear != null).Sum(x => x.SubscriptionYear));
                totalSubscriptionMonth = Convert.ToInt32(orderList.Where(cnd => cnd.SubscriptionMonth != null).Sum(x => x.SubscriptionMonth));
            }
            else
            {
                totalSubscriptionYear = orderDetail.SubscriptionYear == null ? 0 : Convert.ToInt32(orderDetail.SubscriptionYear);
                totalSubscriptionMonth = orderDetail.SubscriptionMonth == null ? 0 : Convert.ToInt32(orderDetail.SubscriptionMonth);
            }
            View.OrderDetail = orderDetail;


            SetProperties(orderDetail, totalSubscriptionYear, totalSubscriptionMonth);
        }

        public Int32 GetTenant()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.RenewalOrder);
        }

        ///// <summary>
        ///// Get the OPDId to which the Compliance Package belongs to
        ///// </summary>
        ///// <returns></returns>
        //public Int32 GetComplianceOPDId()
        //{
        //    var _complianceOPDType = ComplianceDataManager.GetComplianceOrdPayDetail(View.OrderDetail);
        //    if (_complianceOPDType.IsNotNull())
        //        return _complianceOPDType.OPD_ID;

        //    return AppConsts.NONE;
        //}
     
        public void getMaximumAllowedDuration(Int32 DPPID)
        {
            var subscriptionsOptions = ComplianceDataManager.GetDeptProgramPackageSubscriptionOptions(DPPID, View.TenantId);

            View.MaximumAllowedDuration = 0;
            foreach (var a in subscriptionsOptions)
            {
                Int32 totalmonths = (a.Year.HasValue ? Convert.ToInt32(a.Year) * 12 : 0) + (a.Month.HasValue ? Convert.ToInt32(a.Month) : 0);
                if (totalmonths > View.MaximumAllowedDuration)
                    View.MaximumAllowedDuration = totalmonths;
            }

        }
     
        #endregion

        #region Private Methods

        private void SetProperties(Order orderDetail, Int32 totalSubscriptionYear, Int32 totalSubscriptionMonth)
        {
            if (orderDetail != null)
            {
                Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());
                View.FirstName = orderDetail.OrganizationUserProfile.FirstName;
                View.LastName = orderDetail.OrganizationUserProfile.LastName;

                // UAT 1067 - Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
                //View.InstitutionHierarchy = orderDetail.DeptProgramPackage.DeptProgramMapping.DPM_Label;
                if (orderDetail.DeptProgramMapping1.IsNotNull())
                    View.InstitutionHierarchy = orderDetail.DeptProgramMapping1.DPM_Label;

                View.PackageName = String.IsNullOrEmpty(orderDetail.DeptProgramPackage.CompliancePackage.PackageLabel)
                    ? orderDetail.DeptProgramPackage.CompliancePackage.PackageName
                    : orderDetail.DeptProgramPackage.CompliancePackage.PackageLabel;

                View.PackageDetail = orderDetail.DeptProgramPackage.CompliancePackage.Description;

                View.Dpp_Id = orderDetail.DeptProgramPackage.DPP_ID;

                /*View.NodeId = orderDetail.DeptProgramPackage.DeptProgramMapping.DPM_InstitutionNodeID;*/
                // Institution-Id of the last node selected in the pending order hierarchy.
                View.NodeId = ComplianceDataManager.GetLastNodeInstitutionId(Convert.ToInt32(orderDetail.HierarchyNodeID), View.TenantId);

                View.SelectedDeptProgramId = orderDetail.DeptProgramPackage.DeptProgramMapping.DPM_ID;
                //Int32? subscriptionDuration = (orderDetail.SubscriptionYear == null ? 0 * 12 : orderDetail.SubscriptionYear * 12) + (orderDetail.SubscriptionMonth == null ? 0 : orderDetail.SubscriptionMonth);
                Int32 subscriptionDuration = (totalSubscriptionYear * 12) + totalSubscriptionMonth;
                View.RenewalDuration = orderDetail.ProgramDuration - subscriptionDuration;
                DeptProgramPackageSubscription depProgramPackageSubscription = orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(cond => cond.SubscriptionOption.Code == subscriptionOptionCode && cond.SubscriptionOption.IsSystem && !cond.DPPS_IsDeleted).FirstOrDefault();
                if (depProgramPackageSubscription != null)
                {
                    View.DPPS_Id = depProgramPackageSubscription.DPPS_ID;
                    //Decimal? TotalPrice=orderDetail.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(cond => cond.SubscriptionOption.Code == subscriptionOptionCode && cond.SubscriptionOption.IsSystem && !cond.DPPS_IsDeleted).Select(x => x.DPPS_TotalPrice).FirstOrDefault();
                    Decimal? TotalPrice = depProgramPackageSubscription.DPPS_TotalPrice;
                    View.TotalPrice = TotalPrice;
                    TotalPrice = TotalPrice == null ? 0 : TotalPrice;
                    View.Price = View.RenewalDuration * TotalPrice;
                    View.RushOrderPrice = depProgramPackageSubscription.DPPS_RushOrderAdditionalPrice;
                }
                View.ProgramDuration = View.RenewalDuration;
                View.ViewDetails = Convert.ToBoolean(orderDetail.DeptProgramPackage.CompliancePackage.IsViewDetailsInOrderEnabled);
            }
        }

        /// <summary>
        /// To show Rush Order depending on client settings
        /// </summary>
        public void ShowRushOrderSetting()
        {
            List<lkpSetting> lkpSettingList = ComplianceDataManager.GetSettings(View.TenantId);
            List<ClientSetting> clientSettingList = ComplianceDataManager.GetClientSetting(View.TenantId);
            Int32 rushOrderID = lkpSettingList.WhereSelect(cond => cond.Code == Setting.Enable_Rush_Order.GetStringValue(), col => col.SettingID).FirstOrDefault();
            String enableRushOrderValue = clientSettingList.WhereSelect(t => t.CS_SettingID == rushOrderID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRushOrder = String.IsNullOrEmpty(enableRushOrderValue) ? false : ((enableRushOrderValue == "0") ? false : true);
        }

        #endregion

        #endregion

        
    }
}




