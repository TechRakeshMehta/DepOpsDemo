using System;
using System.Collections.Generic;

namespace CoreWeb.Shell.Views
{
    public interface IRedirectIPNhandlerView
    {
        List<Entity.PaymentIntegrationSetting> PaymentIntegrationSettings
        {
            get;
            set;
        }

        String IPNPostData
        {
            get;
            set;
        }

        IRedirectIPNhandlerView CurrentViewContext
        {
            get;
        }

        String IPNTransactionStatus
        {
            get;
            set;
        }

        Dictionary<String, String> IPNPostDataKeyValue
        {
            get;
            set;
        }

        Int32 TenantID
        {
            get;
            set;
        }

        List<Int32> OrderIDs
        {
            get;
            set;
        }
    }
}




