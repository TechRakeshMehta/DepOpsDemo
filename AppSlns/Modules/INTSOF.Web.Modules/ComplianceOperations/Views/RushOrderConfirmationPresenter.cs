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
    public class RushOrderConfirmationPresenter : Presenter<IRushOrderConfirmationView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public RushOrderConfirmationPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            //GetTenantId();
            var deptProgramPackageSubscription = ComplianceDataManager.GetDeptProgramPackageSubscription(View.OrderId, View.SubscriptionId, View.TenantId);
            if (deptProgramPackageSubscription.IsNotNull())
            {
                View.DeptProgramPackageSubscriptionId = deptProgramPackageSubscription.DPPS_ID;
                View.SelectedPackageDetails = ComplianceDataManager.GetApplicantPackageDetails(View.DeptProgramPackageSubscriptionId, View.TenantId);
            }
        }

        public void GetTenantId()
        {
            var organizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
            View.TenantId = organizationUser.Organization.TenantID.Value;
            View.OrganizationUser = organizationUser;
        }

        public Boolean IsOrderPaymentDone(String orderID)
        {
            if (!orderID.IsNullOrEmpty() && orderID != AppConsts.ZERO)
            {
                Order order = ComplianceDataManager.GetOrderById(View.TenantId, Convert.ToInt32(orderID));
                View.SelectedNodeHierarchy = order.DeptProgramMapping1.IsNotNull() ? order.DeptProgramMapping1.DPM_Label : String.Empty;

                //Issue QA: Incorrect payment instructions get displayed on Order Summary screen when applicant places rush order through Credit Card or PayPal payment options
                Int32 opdID = 0;
                var _lstOPD = order.OrderPaymentDetails;
                foreach (var opd in _lstOPD)
                {
                    var _oppdRushOrder = opd.OrderPkgPaymentDetails.Where(oppd => oppd.OPPD_BkgOrderPackageID.IsNull()
                        && oppd.OPPD_IsDeleted == false
                          && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue()).FirstOrDefault();
                    if (_oppdRushOrder.IsNotNull())
                    {
                        opdID = opd.OPD_ID;
                        break;
                    }
                }


                if (order.IsNotNull())
                {
                    String orderStatusCode = order.lkpOrderStatu1.Code;
                    var rushOrderPaymentDetail = order.OrderPaymentDetails.FirstOrDefault(x => x.OPD_PaymentOptionID != null && x.OPD_ID == opdID && !x.OPD_IsDeleted);
                    if (rushOrderPaymentDetail != null)
                        View.OrderPaymentTypeId = rushOrderPaymentDetail.OPD_PaymentOptionID.Value;
                    if (orderStatusCode == ApplicantOrderStatus.Paid.GetStringValue())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.RushOrderConfirmation);
        }

        public String GetPaymentInstruction(Int32 paymentTypeId)
        {
            String paymentOptionCode = LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(View.TenantId)
                                               .FirstOrDefault(con => con.PaymentOptionID == paymentTypeId && con.IsDeleted == false).Code;
            return ComplianceDataManager.GetPaymentInstructionByCode(paymentOptionCode);
        }

        public String GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }

        #region UAT 1190 Mask DOB on order summary
        public String GetMaskDOB(String unMaskedDOB)
        {
            return ApplicationDataManager.GetMaskDOB(unMaskedDOB);
        }
        #endregion
    }
}




