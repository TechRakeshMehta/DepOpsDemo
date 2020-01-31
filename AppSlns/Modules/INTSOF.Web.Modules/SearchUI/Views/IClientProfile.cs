using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.UI.Contract.SearchUI; //UAT-3326


namespace CoreWeb.Search.Views
{
    public interface IClientProfile
    {
        String MasterNodeLabel { get; set; }//UAT:2411
        Int32 OrganizationUserID { get; set; }
        Entity.OrganizationUser OrganizationUserData { get; set; }
        String ClientFirstName { get; set; }
        String ClientLastName { get; set; }
        String PhoneNumber { get; set; }
        String EmailAddress { get; set; }
        String UserName { get; set; }
        String LockUser { get; set; }
        IClientProfile CurrentViewContext { get; }
        List<GetDepartmentTree> lstTreeData { set; get; }
        List<InstituteHierarchyNodesList> lstTreeHierarchyData { set; get; } //UAT-3369
        List<GetDepartmentTree> LstBackgroundTreeData { set; get; }
        Int32 ClientTenantID { get; set; }
        List<FeatureActionContract> FeaturePermissionData { set; get; }

        String LockUserStatus { get; set; }

        String LockUserCommandName { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        String LockUnlockUserTooltip { get; set; }

        List<Entity.SystemEntityUserPermissionData> SystemEntityUserPermissionData { set; get; }

        Boolean IsFromConfigurationPage
        {
            get;
            set;
        }

        Int32 DeptProgramMappingID
        {
            get;
            set;
        }

        String AssignedRoles
        {
            get;
            set;
        }
        //UAT-2447
        String PhoneNumberUnMasked { get; set; }

        List<Entity.ClientEntity.TypeCustomAttributes> ProfileCustomAttributeList { get; set; }
    }
}
