using System;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.Shell.Views
{
    public interface ICategoriesItemsNodes
    {
        List<NodesContract> ListofNodes
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        int ComplianceCategoryId { get; set; }

        int? ComplianceItemId { get; set; } 
    }
}
