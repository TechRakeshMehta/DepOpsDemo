using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISetupComplianceItemsView
    {
        /// <summary>
        /// List of Items displayed in the grid
        /// </summary>
        List<ComplianceItem> lstComplianceItems { get; set; }

        /// <summary>
        /// Id of the Tenant to which current user belongs to
        /// </summary>
        Int32 TenantId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 DefaultTenantId { get; set; }

        /// <summary>
        /// OrganizationUserID of the currently logged in user
        /// </summary>
        Int32 CurrentLoggedInUserId { get; }

        /// <summary>
        /// Represents the current view 
        /// </summary>
        ISetupComplianceItemsView CurrentViewContext { get; }

        /// <summary>
        /// Contract to manage the properties of the ComplianceItems Entity
        /// </summary>
        ComplianceItemsContract ViewContract { get; }

        /// <summary>
        /// Status of update/deletion of the item
        /// </summary>
        Boolean IsOperationSuccessful { get; set; }

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
        //UAT-3872
        String DeptProgramMappingID { get; }
    }
}




