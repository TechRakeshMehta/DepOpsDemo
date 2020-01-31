using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface ISetupRequirementTreeView
    {
        /// <summary>
        /// List to bind the Treeview
        /// </summary>
        List<Entity.SharedDataEntity.RequirementTree> lstTreeData { set; get; }

        List<RequirementTreeContract> lstTreeDataContract { set; get; }
        Int32 CurrentUserId { get; }

        Boolean IsViewOnly { get; set; }


        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
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
        /// Gets and sets list of selected package Ids
        /// </summary>
        Int32 SelectedPackageID
        {
            set;
            get;
        }

        Boolean IsPackageUsed { get; set; }
    }
}




