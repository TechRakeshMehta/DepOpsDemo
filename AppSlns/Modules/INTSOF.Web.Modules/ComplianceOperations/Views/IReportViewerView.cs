using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IReportViewerView
    {
        Int32 TenantID { get; set; }

        String SubscriptionIDs { get; }

    }
}
