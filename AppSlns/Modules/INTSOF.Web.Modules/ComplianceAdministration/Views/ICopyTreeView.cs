using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICopyTreeView
    {
        /// <summary>
        /// List to bind the Treeview
        /// </summary>
        List<GetRuleSetTree> AssignedTreeData { set; get; }

        /// <summary>
        /// Represents the current view context
        /// </summary>
        ICopyTreeView CurrentViewContext { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 TenantId { get; set; }

        Int32 ManageTenantId { get; set; }

        String InfoMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the data of treeListPackages from UI.
        /// </summary>
        List<GetRuleSetTree> TreeListPackagesUIState
        {
            get;
            set;
        }
    }
}




