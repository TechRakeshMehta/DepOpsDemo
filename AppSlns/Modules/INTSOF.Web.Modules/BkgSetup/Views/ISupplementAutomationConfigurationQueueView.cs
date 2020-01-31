using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface ISupplementAutomationConfigurationQueueView
    {
        Int32 SupplementAutomationConfigurationId
        {
            get;
            set;
        }

        List<Int32> MappedTenantIds
        {
            get;
            set;
        }

        List<Tenant> lstTenant
        {
            get;
            set;
        }

        String Description
        {
            get;
            set;
        }
        
        Decimal Percentage
        {
            get;
            set;
        }
        
        Int32 CurrentLoggedInUserId
        {
            get;
        }

    }
}
