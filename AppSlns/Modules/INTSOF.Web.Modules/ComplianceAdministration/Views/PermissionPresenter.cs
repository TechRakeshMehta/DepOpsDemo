using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class PermissionPresenter : Presenter<IPermissionView>
    {
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetPermissionList(Boolean OnlyPackagePermissions)
        {
            if (OnlyPackagePermissions) //UAT 2834
            {
                View.UserPacakgePermissionList = ComplianceSetupManager.GetPermissionList(View.TenantId, OnlyPackagePermissions).ToList();
            }
            else
            {
                View.UserPermissionList = ComplianceSetupManager.GetPermissionList(View.TenantId, OnlyPackagePermissions).ToList();
            }
        }

        public void GetHierarchyPermission()
        {
            String backgroundHierarchyPermissionType = HierarchyPermissionTypes.BACKGROUND.GetStringValue();
            View.BackgroundHierarchyPermissionList = new List<vwHierarchyPermission>();
            var hierarchyPermissionList = ComplianceSetupManager.GetHierarchyPermissionList(View.TenantId, View.DeptProgramMappingID);
            if (!hierarchyPermissionList.IsNullOrEmpty())
            {
                View.HierarchyPermissionList = hierarchyPermissionList.ToList();
                View.BackgroundHierarchyPermissionList = View.HierarchyPermissionList.Where(cond => cond.HierarchyPermissionTypeCode != null &&
                                                    cond.HierarchyPermissionTypeCode.Equals(backgroundHierarchyPermissionType)).ToList();
            }
        }

        public void SaveHierarchyPermission()
        {
            List<String> lstHierarchyPermissionTypeCode = new List<String>() { HierarchyPermissionTypes.BACKGROUND.GetStringValue() };
            if (View.IsIncludeAnotherHierarchyPermissionType)
            {
                lstHierarchyPermissionTypeCode.Add(HierarchyPermissionTypes.COMPLIANCE.GetStringValue());
            }
            HierarchyPermission hierarchyPermission = new HierarchyPermission();
            hierarchyPermission.HP_OrganizationUserID = View.OrganizationUserID;
            hierarchyPermission.HP_PermissionID = View.PermissionId;
            hierarchyPermission.HP_HierarchyID = View.DeptProgramMappingID;
            hierarchyPermission.HP_IsDeleted = false;
            hierarchyPermission.HP_CreatedBy = View.CurrentLoggedInUserId;
            hierarchyPermission.HP_CreatedOn = DateTime.Now;
            if (View.IsIncludeAnotherHierarchyPermissionType)
            {
                hierarchyPermission.HP_ProfilePermissionID = View.ProfilePermissionId.Value;
                hierarchyPermission.HP_VerificationPermissionID = View.VerificationPermissionId.Value;
                hierarchyPermission.HP_OrderQueuePermissionID = View.OrderPermissionId.Value;
                hierarchyPermission.HP_PackagePermissionID = View.PackagePermissionID.Value;
            }

            if (ComplianceSetupManager.SaveHierarchyPermission(View.TenantId, hierarchyPermission, lstHierarchyPermissionTypeCode))
            {
                View.SuccessMessage = "User Hierarchy Permission mapping saved successfully.";
            }
            else
            {
                View.ErrorMessage = "An error occured while mapping User Hierarchy Permission. Please try again.";
            }

        }

        public void UpdateHierarchyPermission()
        {
            HierarchyPermission hierarchyPermission = ComplianceSetupManager.GetHierarchyPermissionByID(View.TenantId, View.HierarchyPermissionID);
            if (hierarchyPermission.IsNotNull())
            {
                hierarchyPermission.HP_ID = View.HierarchyPermissionID;
                hierarchyPermission.HP_PermissionID = View.PermissionId;
                hierarchyPermission.HP_ModifiedBy = View.CurrentLoggedInUserId;
                hierarchyPermission.HP_ModifiedOn = DateTime.Now;
                if (ComplianceSetupManager.UpdateHierarchyPermission(View.TenantId))
                {
                    View.SuccessMessage = "User Hierarchy Permission mapping updated successfully.";
                }
                else
                {
                    View.ErrorMessage = "An error occured while updating User Hierarchy Permission. Please try again.";
                }
            }
            else
            {
                View.ErrorMessage = "An error occured while updating User Hierarchy Permission. Please try again.";
            }


        }

        public void DeleteHierarchyPermission()
        {
            HierarchyPermission hierarchyPermission = ComplianceSetupManager.GetHierarchyPermissionByID(View.TenantId, View.HierarchyPermissionID);
            if (hierarchyPermission.IsNotNull())
            {
                hierarchyPermission.HP_IsDeleted = true;
                hierarchyPermission.HP_ModifiedBy = View.CurrentLoggedInUserId;
                hierarchyPermission.HP_ModifiedOn = DateTime.Now;

                if (ComplianceSetupManager.DeleteHierarchyPermission(View.TenantId))
                {
                    View.SuccessMessage = "User Hierarchy Permission mapping deleted successfully.";
                }
                else
                {
                    View.ErrorMessage = "An error occured while deleting User Hierarchy Permission. Please try again.";
                }
            }
            else
            {
                View.ErrorMessage = "An error occured while deleting User Hierarchy Permission. Please try again.";
            }
        }

        public void GetOrganizationUserList()
        {
            //var hierarchyPermissionList = View.HierarchyPermissionList;
            var OrganizationUserList = SecurityManager.GetOganisationUsersByTanentId(View.TenantId);

            if (!View.BackgroundHierarchyPermissionList.IsNullOrEmpty())
            {
                var uniqueOrganizationUserList = OrganizationUserList.Where(p => !View.BackgroundHierarchyPermissionList.Any(p2 => p2.OrganizationUserID == p.OrganizationUserID));
                View.OrganizationUserList = uniqueOrganizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " " + x.LastName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }
            else
            {
                View.OrganizationUserList = OrganizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " " + x.LastName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }

            View.OrganizationUserList = View.OrganizationUserList.OrderBy(col => col.FirstName).ToList();
        }


    }
}
