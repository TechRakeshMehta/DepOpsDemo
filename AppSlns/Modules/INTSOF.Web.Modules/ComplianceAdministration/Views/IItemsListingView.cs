using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IItemsListingView
    {
        Int32 TenantId { get; set; }

        Int32 DefaultTenantId { get; set; }

        IItemsListingView CurrentViewContext { get; }

        ComplianceCategoryItemContract ViewContract { get; }

        /// <summary>
        /// Gets the list of the compliance items for the selected category, displayed in the listing grid
        /// </summary>
        List<ComplianceCategoryItem> lstComplianceItems { get; set; }
        Int32 CurrentLoggedInUserId { get; }

        String ErrorMessage
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }
    }
}




