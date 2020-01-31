using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IPermissionView
    {
        List<lkpPermission> UserPermissionList { get; set; }
        Int16 PermissionId { get; set; }
        Int32 HierarchyPermissionID { get; set; }
        List<vwHierarchyPermission> HierarchyPermissionList { get; set; }
        List<vwHierarchyPermission> BackgroundHierarchyPermissionList { get; set; }
        String PermissionCode { get; set; }
        IPermissionView CurrentViewContext { get; }
        Int32 OrganizationUserID { get; set; }
        Boolean IsIncludeAnotherHierarchyPermissionType { get; set; }
        List<Entity.OrganizationUser> OrganizationUserList { get; set; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        String InfoMessage { get; set; }
        String PageType { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        Int16? ProfilePermissionId { get; set; }
        Int16? VerificationPermissionId { get; set; }
        Int16? OrderPermissionId { get; set; }

        Int16? PackagePermissionID { get; set; } //UAT - 2834       
        List<lkpPermission> UserPacakgePermissionList { get; set; } //UAT - 2834
    }
}
