using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOrderSubscriptionHistoryView
    {
        //to check whether applicant has complio buisness channel or not.
        Boolean IsComplioBuisnessChannelTypeAvlbl
        {
            get;
            set;
        }

        //CBI || CABS Tenants
        Boolean IsLocationServiceTenant { get; set; }
    }
}
