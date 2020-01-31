using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoreWeb.Shell.Views
{
    public interface IRedirectPaypalView
    {
        List<Entity.PaymentIntegrationSetting> PaymentIntegrationSettings
        {
            get;
            set;
        }

        IRedirectPaypalView CurrentViewContext
        {
            get;
        }

        String InvoiceNumber
        {
            get;
            set;
        }

        NameValueCollection TransactionDetails
        {
            get;
            set;
        }

        String PaypalPDTStatus
        {
            get;
            set;
        }

        String McGross
        {
            get;
            set;
        }

        String ApplicationURL
        {
            get;
            set;
        }
    }
}




