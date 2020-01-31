using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManagePackageServiceHierarchyView
    {
        /// <summary>
        /// List to bind the Treeview
        /// </summary>
        List<PkgSvcSetupContract> lstTreeData { set; get; }

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

        #region Package DropDown Changes :UAT-1116
        List<BackgroundPackage> lstBackgroundPackage
        {
            set;
            get;
        }

        List<Int32> SelectedBkgPackageIdList
        {
            get;
            set;
        }

        #endregion

        //UAT-3157
        Int32 PreferredSelectedTenantID { get; set; }
    }
}
