using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IPaypalPaymentSubmissionView
    {
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }
        String InvoiceNumber
        {
            get;
            set;
        }

        OnlinePaymentTransaction OnlinePaymentTransactionDetails
        {
            get;
            set;
        }

        IPaypalPaymentSubmissionView CurrentViewContext
        {
            get;
        }

        List<Entity.PaymentIntegrationSetting> PaymentIntegrationSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }
    }
}




