using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IMangeSystemEntityUserPermissionsView
    {
        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> ListTenants
        {
            set;
            get;
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
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
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

        Int32 currentLoggedInUserId
        {
            get;
        }

        List<LkpSystemEntity> SystemEntityList
        {
            get;
            set;
        }

        List<SystemEntityUserPermissionData> SystemEntityUserPermissionList
        {
            get;
            set;
        }

        Int32 SelectedEntityId
        {
            get;
            set;
        }

        List<OrgUser> UsersApplicableForAssigningPermission
        {
            get;
            set;
        }
        List<SystemEntityPermission> PermissionList
        {
            get;
            set;
        }
        Int32 EntityPermissionId
        {
            get;
            set;
        }

        Int32 CurrentOrganisationUserId
        {
            get;
            set;
        }

        Int32 SEUP_ID
        {
            get;
            set;
        }

        String UserFirstName
        {
            get;
            set;
        }
        String UserLastName
        {
            get;
            set;
        }

        String EmailAddress
        {
            get;
            set;
        }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualRecordCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        #endregion

        Dictionary<int, bool> LstSelectedBkgOdrResPermissions { get; set; }

        List<Int32> LstEntityPermissionIds
        {
            get;
            set;
        }

        Int32? SelectedHierarchyId
        {
            get;
            set;
        }
    }
}
