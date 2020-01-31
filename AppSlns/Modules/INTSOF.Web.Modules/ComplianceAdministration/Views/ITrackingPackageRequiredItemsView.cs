using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
   public interface ITrackingPackageRequiredItemsView
    {
       

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
        ITrackingPackageRequiredItemsView CurrentViewContext { get; }

        /// <summary>
        /// Contract to manage the properties of the ComplianceItems Entity
        /// </summary>
        TrackingPackageRequiredContract ViewContract { get; }

        List<TrackingPackageRequiredContract> listTrackingPackageRequiredContract { get; set; }

        List<CompliancePackage> listPackage { get; set; }

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

        /// <summary>
        /// Gets and sets list of compliance packages.
        /// </summary>        
        List<CompliancePackage> ListCompliancePackages
        {
            set;
            get;
        }
    }
}
