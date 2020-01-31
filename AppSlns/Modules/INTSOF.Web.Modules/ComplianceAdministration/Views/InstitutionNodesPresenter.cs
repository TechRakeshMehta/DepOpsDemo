using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class InstitutionNodesPresenter : Presenter<IInstitutionNodesView>
    {
        /// <summary>
        /// Get the Institution Nodes of the Selected Tenant, for the current user, based on the Permissions
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public void GetInstitutionNodes()
        {
            View.dicNodes = StoredProcedureManagers.GetInstitutionNodes(View.CurrentUserId, View.TenantId);
        }
    }
}
