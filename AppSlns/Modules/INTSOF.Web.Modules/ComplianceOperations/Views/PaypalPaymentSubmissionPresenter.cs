using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class PaypalPaymentSubmissionPresenter : Presenter<IPaypalPaymentSubmissionView>
    {
        public override void OnViewInitialized()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            View.OnlinePaymentTransactionDetails = ComplianceDataManager.GetPaymentTransactionDetails(View.InvoiceNumber, View.TenantId);
            View.PaymentIntegrationSettings = SecurityManager.GetPaymentIntegrationSettingsByName("Paypal");
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.PaypalPaymentSubmission);
        }
    }
}




