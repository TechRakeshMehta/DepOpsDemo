using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class TenantUserMappingPresenter : Presenter<ITenantUserMappingView>
    {
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenants = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        public void GetUserList()
        {
            List<Entity.OrganizationUser> lstAllOrganizationUser = new List<Entity.OrganizationUser>();
            View.lstOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(SecurityManager.DefaultTenantID).Select(x => new Entity.OrganizationUser
            {
                FirstName = x.FirstName + " " + x.LastName,
                OrganizationUserID = x.OrganizationUserID
            }).ToList();
            List<Int32> lstOrganizationUserIds = GetUsersMappedWithTenant();
            View.lstOrganizationUser.RemoveAll(x => lstOrganizationUserIds.Contains(x.OrganizationUserID));
           // List<Entity.OrganizationUser> lstOrganizationUsersToRemove = lstAllOrganizationUser.Where(cond => lstOrganizationUserIds.Contains(cond.OrganizationUserID)).ToList();
           // View.lstOrganizationUser.RemoveAll(x => lstOrganizationUsersToRemove.Any(cond => cond.OrganizationUserID == x.OrganizationUserID));

        }

        public List<Int32> GetUsersMappedWithTenant()
        {
            return SecurityManager.GetUsersMappedWithTenant(View.selectedTenantID);
        }
        public void GetTenantUserMappings()
        {
            View.lstTenantUserMappings = new List<TenantUserMappingContract>();
            View.lstTenantUserMappings = SecurityManager.GetTenantUserMappings();
        }

        public Boolean SaveTenantUserMapping(List<TenantUserMappingContract> lstTenantUserMappings)
        {
            return SecurityManager.SaveTenantUserMapping(lstTenantUserMappings, View.CurrentLoggedInUserID);
        }

        public Boolean UpdateTenantUserMapping()
        {
            //return SecurityManager.UpdateTenantUserMapping(View.CurrentLoggedInUserID,);
            return true;
        }

        public Boolean DeleteTenantUserMapping(Int32 tenantUserMappingId)
        {
            return SecurityManager.DeleteTenantUserMapping(View.CurrentLoggedInUserID, tenantUserMappingId);
        }
    }
}
