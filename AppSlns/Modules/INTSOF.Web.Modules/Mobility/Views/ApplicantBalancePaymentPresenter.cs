#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using INTSOF.SharedObjects;

#endregion

#region UserDefined

using INTSOF.Utils;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Collections.Generic;

#endregion

#endregion

namespace CoreWeb.Mobility.Views
{
    public class ApplicantBalancePaymentPresenter : Presenter<IApplicantBalancePaymentView>
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

        #region UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        /// <summary>
        ///  On the basis of Applicant-ID and Non-Preferred enum code we are checking the user has opted the option of non-prferred browser
        /// </summary>
        /// <returns></returns>
        public Boolean CheckNonPrefferedBrowserOption()
        {
            Boolean chkNonPrefferedBrowserOption = SecurityManager.CheckUserOptedNonPrefferedBrowserOption(View.CurrentLoggedInUserID, UtilityFeatures.NonPrefferedBrowser.GetStringValue());
            View.IsDisplayNonPreferredOption = chkNonPrefferedBrowserOption;
            return chkNonPrefferedBrowserOption;
        }

        #endregion

        /// <summary>
        /// Gets the tenant id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            var tenant = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserID).Organization.Tenant;
            return tenant.TenantID;
        }

        /// <summary>
        /// Gets the value of fields in Institute Change Request Screen for a Applicant.
        /// </summary>
        public void GetScreenDetails()
        {
            Order order = MobilityManager.GetApplicantBalanceDueOrder(View.TenantId, View.CurrentLoggedInUserID);

            if (order.IsNotNull())
            {
                String compliancePackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();

                String orderStatus = order.OrderPaymentDetails
                       .Where(opd => opd.OrderPkgPaymentDetails
                       .Any(oppd => oppd.lkpOrderPackageType.OPT_Code == compliancePackageTypeCode && !oppd.OPPD_IsDeleted)
                         && !opd.OPD_IsDeleted).FirstOrDefault().lkpOrderStatu.Code;
                View.OrderID = order.OrderID;
                View.OrderNumber = order.OrderNumber;
                //Removed y.DPPS_IsDeleted == false check so that user can view order if subscription is deleted
                var _dpps = order.DeptProgramPackage.DeptProgramPackageSubscriptions.FirstOrDefault(y => y.SubscriptionOption.Label.ToLower() == order.SubscriptionLabel.ToLower());
                View.DPPSId = _dpps.DPPS_ID;
                // Confirm can DPP be null ???
                View.DPPId = Convert.ToInt32(order.DeptProgramPackageID);
                View.AmountDue = order.DuePayment;
                View.PackagePrice = order.TotalPrice.IsNullOrEmpty() ? String.Format("{0:0.00}", 0) : String.Format("{0:0.00}", order.TotalPrice);
                View.OrganizationUserProfile = order.OrganizationUserProfile;
                if (order.DeptProgramMapping1.IsNotNull())
                    View.InstitutionHierarchy = order.DeptProgramMapping1.DPM_Label;
                View.Package = order.DeptProgramPackage.CompliancePackage.PackageLabel.IsNullOrEmpty() ? order.DeptProgramPackage.CompliancePackage.PackageName : order.DeptProgramPackage.CompliancePackage.PackageLabel;
                View.SubscriptionPeriodMonths = CalculateSubscriptionPeriodMonths(order.SubscriptionYear, order.SubscriptionMonth);
                //View.IsOnlineOrderFailed = order.lkpOrderStatu.Code == ApplicantOrderStatus.Payment_Due.GetStringValue() ? false : true;
                View.IsOnlineOrderFailed = (orderStatus == ApplicantOrderStatus.Payment_Due.GetStringValue() ? false : true);
                View.PreviousOrderID = order.PreviousOrderID;
                View.TargetHierarchyNodeID = order.HierarchyNodeID;
                View.SelectedNodeId = Convert.ToInt32(order.SelectedNodeID);
            }
        }
        /// <summary>
        /// Gets the description of previous order for an Applicant.
        /// </summary>
        public void GetPreviousOrderDetail()
        {
            Order preOrderDetails = MobilityManager.GetApplicantBalanceDuePreviousOrder(View.TenantId, View.CurrentLoggedInUserID, View.PreviousOrderID);
            if (preOrderDetails.IsNotNull())
            {
                if (preOrderDetails.DeptProgramMapping1.IsNotNull())
                    View.SourceInstitutionHierarchy = preOrderDetails.DeptProgramMapping1.DPM_Label;
                View.SourcePackage = preOrderDetails.DeptProgramPackage.CompliancePackage.PackageLabel.IsNullOrEmpty() ? preOrderDetails.DeptProgramPackage.CompliancePackage.PackageName : preOrderDetails.DeptProgramPackage.CompliancePackage.PackageLabel;
            }
        }

        public Boolean IsOrderPaymtDueAndChangeByAdmin()
        {
            return MobilityManager.IsOrderPaymtDueAndChangeByAdmin(View.TenantId, View.OrderID);
        }
        /// <summary>
        /// Calculate Subscription duration in months.
        /// </summary>
        /// <param name="subscriptionYear">subscriptionYear</param>
        /// <param name="subscriptionMonth">subscriptionMonth</param>
        /// <returns>Subscription duration in months</returns>
        public String CalculateSubscriptionPeriodMonths(Int32? subscriptionYear, Int32? subscriptionMonth)
        {
            Int32 subscriptionPeriodMonths = 0;

            if (subscriptionYear != null)
            {
                subscriptionPeriodMonths = (subscriptionYear ?? 0) * 12;
            }

            if (subscriptionMonth != null)
            {
                subscriptionPeriodMonths += (subscriptionMonth ?? 0);
            }
            return Convert.ToString(subscriptionPeriodMonths);
        }

        /// <summary>
        /// Gets the Payment Types to pay balance payment for an applicant.
        /// </summary>
        public void GetPaymentTypes()
        {
            // View.LstPaymentOptions = ComplianceDataManager.GetPaymentOptions(View.TenantId, View.DPPSId);

            View.lstPaymentOptions = StoredProcedureManagers.GetPaymentOptions(View.DPPId, String.Empty, View.SelectedNodeId, View.TenantId);

            List<lkpPaymentOption> lstPaymntOptn = ComplianceDataManager.GetPaymentTypeList(View.TenantId);
            if (!lstPaymntOptn.IsNullOrEmpty())
            {
                View.PaymentMode_InvoicetoInstitutionId = lstPaymntOptn.Where(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
                View.PaymentMode_InvoiceWithoutApprovalId = lstPaymntOptn.Where(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
            }

        }

        /// <summary>
        /// Gets the next page path if Order Stage is incorrect.
        /// </summary>
        /// <param name="applicantOrderCart">applicantOrderCart session variable in which order staging is present.</param>
        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.BalancePayment);
        }

        /// <summary>
        /// Get the order as per order ID. 
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns>Order</returns>
        public Order GetOrderById(Int32 orderID)
        {
            return ComplianceDataManager.GetOrderById(View.TenantId, orderID);
        }

        public String GetPaymentCodeByID(Int32 paymentOptionID)
        {
            return ComplianceDataManager.GetPaymentOptionCodeById(paymentOptionID, View.TenantId);
        }

        #endregion

        #region Private Methods



        #endregion

        #endregion

    }
}

















