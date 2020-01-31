using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IPackageInfoView
    {
        IPackageInfoView CurrentViewContext
        {
            get;
        }

        CompliancePackageContract ViewContract
        {
            get;
        }

        CompliancePackage CompliancePackage
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentPackageId
        {
            get;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        String ErrorMessage
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

        Int32 NotesPositionId
        {
            get;
            set;
        }

        Int32 DefaultTenantId
        { 
            get; 
            set; 
        }

        string PackageBundleNodeHierarchy
        {
            get;
            set;
        }
    }
}




