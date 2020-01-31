using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISetupView
    {
        /// <summary>
        /// List to bind the Treeview
        /// </summary>
        List<GetRuleSetTree> lstTreeData { set; get; }

        Int32 CurrentUserId { get; }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets list of compliance packages.
        /// </summary>
        //List<LookupContract> ListCompliancePackages
        //{
        //    set;
        //    get;
        //}
        List<CompliancePackage> ListCompliancePackages
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets list of selected package Ids
        /// </summary>
        List<Int32> SelectedPackageIDList
        {
            set;
            get;
        }
        
        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
    }
}




