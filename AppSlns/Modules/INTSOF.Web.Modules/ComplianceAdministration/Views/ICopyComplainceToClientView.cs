using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICopyComplainceToClientView
    {
        /// <summary>
        /// List to bind the Treeview
        /// </summary>
        List<GetRuleSetTree> AssignedTreeData { set; get; }

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
