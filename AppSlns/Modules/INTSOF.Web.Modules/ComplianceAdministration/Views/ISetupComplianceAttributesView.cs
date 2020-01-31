using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISetupComplianceAttributesView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 DefaultTenantId 
        { 
            get; 
            set; 
        }

        Int32 currentLoggedInUserId
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        
        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
    }
}




