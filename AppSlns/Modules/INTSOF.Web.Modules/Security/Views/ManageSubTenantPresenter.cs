using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using System.Linq;
using INTSOF.Utils;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class ManageSubTenantPresenter : Presenter<IManageSubTenantView>
    {

        
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void BindChildTenants()
        {
            if (SecurityManager.GetChildTenants(View.TenantId, false).Count() > 0)
            {
                View.ChildTenants = SecurityManager.GetChildTenants(View.TenantId,false);
            }
            else
            {
                View.ChildTenants = new List<Tenant>();
            }
        }

        /// <summary>
        /// delete sub tenant.
        /// </summary>
        public Boolean DeleteSubTenant()
        {
            ClientRelation clientRelation = SecurityManager.GetTenantRelationByRelatedTenantIdAndTenantId(View.TenantId, View.ChildTenantId);
            clientRelation.IsDeleted = true;
            clientRelation.IsActive = false;
            clientRelation.ModifiedByID = View.CurrentUserId;
            clientRelation.ModifiedOn = DateTime.Now;
            SecurityManager.DeleteSubTenant(clientRelation);
            if (ComplianceDataManager.DeleteSubTenant(View.TenantId, View.ChildTenantId, View.CurrentUserId))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// bind all supplier except child supplier.
        /// </summary>
        public void BindAllTenant()
        {
            List<Int32> childTenantId = new List<Int32>();
            IEnumerable<ClientRelation> clientChildRelation = SecurityManager.GetClientChildRelation(false, View.TenantId).AsEnumerable();
            if (clientChildRelation.Count() > Convert.ToInt32(DefaultNumbers.None))
            {
                childTenantId = clientChildRelation.Select(condition => condition.RelatedTenantID).ToList();
            }
            //for not displaying self in mapping list
            childTenantId.Add(View.TenantId);
            //for not displaying super tenant in mapping list
            childTenantId.Add(SecurityManager.DefaultTenantID);
            View.Tenants = SecurityManager.GetAllTenantsForMapping(childTenantId);
        }


        /// <summary>
        /// add sub tenant.
        /// </summary>
        public void AddSubTenant()
        {
            if ((View.ChildTenantNumbers.Count > Convert.ToInt32(DefaultNumbers.None)) && !View.ChildTenantNumbers.Contains(AppConsts.MINUS_ONE))
            { 
                SecurityManager.AddSubTenant(View.TenantId, View.ChildTenantNumbers,View.CurrentUserId);
            }
        }

        public void getTenantName()
        {
            View.TenantName = SecurityManager.GetTenant(View.TenantId).TenantName;
        }
    }
}




