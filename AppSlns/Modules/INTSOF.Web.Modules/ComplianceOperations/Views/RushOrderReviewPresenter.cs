using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Collections.Specialized;
using INTSOF.Utils;
using System.Linq;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class RushOrderReviewPresenter : Presenter<IRushOrderReviewView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public RushOrderReviewPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public override void OnViewLoaded()
        {
            //GetTenantId();
            var deptProgramPackageSubscription = ComplianceDataManager.GetDeptProgramPackageSubscription(View.OrderId, View.SubscriptionId, View.TenantId);
            if (deptProgramPackageSubscription.IsNotNull())
            {
                View.DeptProgramPackageSubscriptionId = deptProgramPackageSubscription.DPPS_ID;
                View.SelectedPackageDetails = ComplianceDataManager.GetApplicantPackageDetails(View.DeptProgramPackageSubscriptionId, View.TenantId);
            }
        }

        // TODO: Handle other view events and set state in the view

        /// <summary>
        /// To get TenantId and OrganizationUser
        /// </summary>
        public void GetTenantId()
        {
            var organizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            View.TenantId = organizationUser.Organization.TenantID.Value;
            View.OrganizationUser = organizationUser;
        }

        public void SubmitApplicantOrder(OrganizationUserProfile userProfile, Order userOrder, Int32 selectedPaymentModeId)
        {
            //String paymentModeCode = String.Empty;
            //View.GeneratedOrderId = ComplianceDataManager.SubmitApplicantOrder(userProfile, userOrder, true, View.DPPSId, selectedPaymentModeId, View.TenantId, out paymentModeCode);
            //View.PaymentModeCode = paymentModeCode;
        }

        public void GetPaymentOptions()
        {
            View.lstPaymentOptions = ComplianceDataManager.GetRushOrderPaymentOptions(View.TenantId);
        }
        
        public void GetPaymentModeCode(Int32 selectedPaymentModeId)
        {
            View.PaymentModeCode = ComplianceDataManager.GetPaymentOptionCodeById(selectedPaymentModeId, View.TenantId);
        }

        public Order GetOrderById(Int32 orderID)
        {
            return ComplianceDataManager.GetOrderById(View.TenantId, orderID);
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.RushOrderReview);
        }

        /*public void UpdateRushOrderExistByID(Int32 orderId)
        {
            Order order = new Order();
            order.OrderID = orderId;
            //order.ApprovalDate = DateTime.Now;
            order.ModifiedByID = View.CurrentLoggedInUserId;
            order.ModifiedOn = DateTime.Now;
            ComplianceDataManager.UpdateRushOrderExistByID(order, ApplicantOrderStatus.Paid.GetStringValue(), View.TenantId); 
            //ComplianceDataManager.UpdateOrderAddPurchasedSubscription(View.TenantId, order, ApplicantOrderStatus.Paid.GetStringValue());
        } */

        public String GetPaymentCodeByID(Int32 paymentOptionID)
        {
            return ComplianceDataManager.GetPaymentOptionCodeById(paymentOptionID, View.TenantId);
        }

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }

        public String GetPaymentInstruction(Int32 selectedPaymentOptionID)
        {
            return ComplianceDataManager.GetPaymentInstruction(selectedPaymentOptionID, View.TenantId);
        }
    }
}




