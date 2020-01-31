using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class MangeSystemEntityUserPermissionsPresenter : Presenter<IMangeSystemEntityUserPermissionsView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.ListTenants = SecurityManager.GetTenants(SortByName, false, clientCode); ;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        public void GetSystemEntities()
        {
            View.SystemEntityList = SecurityManager.GetSystemEntities();
        }

        public void GetSystemEntityUserPermissionList()
        {

            if (View.SelectedEntityId == AppConsts.NONE)
                View.SystemEntityUserPermissionList = new List<SystemEntityUserPermissionData>();
            else
            {
                SearchItemDataContract searchDataContract = new SearchItemDataContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.UserFirstName) ? null : View.UserFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.UserLastName) ? null : View.UserLastName;
                searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                View.SystemEntityUserPermissionList = SecurityManager.GetSystemEntityUserPermissionList(searchDataContract, View.GridCustomPaging, View.SelectedTenantId, View.SelectedEntityId);
                if (View.SystemEntityUserPermissionList.IsNotNull() && View.SystemEntityUserPermissionList.Count > 0)
                {
                    if (View.SystemEntityUserPermissionList[0].TotalCount > 0)
                    {
                        View.VirtualRecordCount = View.SystemEntityUserPermissionList[0].TotalCount;
                    }
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = 0;
                    View.CurrentPageIndex = 1;
                }
            }
        }

        public void GetOrgUserListForAsigningPermission(Int32 curentUserId = 0)
        {
            View.UsersApplicableForAssigningPermission = SecurityManager.GetOrgUserListForAsigningPermission(curentUserId, View.SelectedEntityId, View.SelectedTenantId)
                                                                        .OrderBy(x => x.UserName).ToList();  //UAT- sort dropdowns by Name
        }

        public void GetPermissionByEntityId()
        {
            String codeNone = EnumSystemPermissionCode.NONE.GetStringValue();
            List<SystemEntityPermission> lstSystemEntityPermission = SecurityManager.GetPermissionByEntityId(View.SelectedEntityId);
            SystemEntityPermission systemEntityPermission = lstSystemEntityPermission.Where(x => x.SEP_PermissionCode == codeNone).FirstOrDefault();
            if (!systemEntityPermission.IsNullOrEmpty())
            {
                lstSystemEntityPermission.Remove(systemEntityPermission);
                lstSystemEntityPermission.Add(systemEntityPermission); //To move NONE item at last of the list.

            }
            //UAT-1996
            if (!lstSystemEntityPermission.IsNullOrEmpty() && String.Compare(lstSystemEntityPermission.FirstOrDefault().LkpSystemEntity.SE_CODE,
                                                                             EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue(), true) == AppConsts.NONE)
            {
                String codeFullPermission = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
                SystemEntityPermission systemEntityFullPermission = lstSystemEntityPermission.Where(x => x.SEP_PermissionCode == codeFullPermission).FirstOrDefault();
                lstSystemEntityPermission.Remove(systemEntityFullPermission);
                lstSystemEntityPermission.Insert(0, systemEntityFullPermission); //To move full item at first place of the list.
            }
            View.PermissionList = lstSystemEntityPermission;
        }

        public Boolean SaveUpdateEntityUserPermission()
        {
            Boolean returnResult = false;
            SystemEntityUserPermission userPermission = new SystemEntityUserPermission();
            userPermission.SEUP_ID = View.SEUP_ID;
            userPermission.SEUP_EntityPermissionId = View.EntityPermissionId;
            userPermission.SEUP_OrganisationUserId = View.CurrentOrganisationUserId;
            if (View.SystemEntityList.IsNotNull() && View.SystemEntityList.Count > AppConsts.NONE && View.SystemEntityList.Any(x => x.SE_ID == View.SelectedEntityId && x.SE_CODE == EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()))
            {
                userPermission.SEUP_DPMId = View.SelectedHierarchyId;
                userPermission.SEUP_TenantId = View.SelectedTenantId;
            }
            returnResult = SecurityManager.SaveUpdateEntityUserPermission(userPermission, View.currentLoggedInUserId, View.LstSelectedBkgOdrResPermissions);

            //UAT 4522 Ability to set granular permissions by node for background checks by admin. 
            if (View.SystemEntityList.Any(x => x.SE_ID == View.SelectedEntityId && x.SE_CODE == EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()))
            {
                UserGranularPermissionDigestion(EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue());
            }

            return returnResult;
        }

        public Boolean DeleteEntityUserPermission()
        {
            Boolean returnResult = false;
            returnResult= SecurityManager.DeleteEntityUserPermission(View.SEUP_ID, View.currentLoggedInUserId, View.CurrentOrganisationUserId, View.LstEntityPermissionIds, View.SelectedHierarchyId);
            
            //UAT 4522 Ability to set granular permissions by node for background checks by admin. 
            if (View.SystemEntityList.Any(x => x.SE_ID == View.SelectedEntityId && x.SE_CODE == EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()))
            {
                UserGranularPermissionDigestion(EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue());
            }

            return returnResult;
        }
        public Dictionary<Int32, String> GetDefaultPermissionForClientAdmin()
        {
            return ClinicalRotationManager.GetDefaultPermissionForClientAdmin(View.SelectedTenantId, View.CurrentOrganisationUserId);
        }

        public Boolean IsBackgroundOrderResultUserPermissionExists()
        {
            //SystemEntityUserPermission userPermission = new SystemEntityUserPermission();
            //userPermission.SEUP_ID = View.SEUP_ID;
            //userPermission.SEUP_EntityPermissionId = View.EntityPermissionId;
            //userPermission.SEUP_OrganisationUserId = View.CurrentOrganisationUserId;
            //if (View.SEUP_ID == AppConsts.NONE && View.SystemEntityList.IsNotNull() && View.SystemEntityList.Count > AppConsts.NONE && View.SystemEntityList.Any(x => x.SE_ID == View.SelectedEntityId && x.SE_CODE == EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue()))
            //    userPermission.SEUP_DPMId = View.SelectedHierarchyId;
            return SecurityManager.IsSystemEntityUserPermissionExists(View.CurrentOrganisationUserId, View.SelectedEntityId, View.SelectedHierarchyId);
        }
        
        public Boolean UserGranularPermissionDigestion(String entityCode)
        {
            return ClinicalRotationManager.UserGranularPermissionDigestion(View.SelectedTenantId, View.CurrentOrganisationUserId, entityCode, View.SelectedHierarchyId.Value, View.currentLoggedInUserId);
        }

    }
}
