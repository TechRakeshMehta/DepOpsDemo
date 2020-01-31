using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class DAndRAttributeGroupMappingPresenter : Presenter<IDAndRAttributeGroupMappingView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        /// <summary>
        /// Called when viwe is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Gets Get DAndRAttributeGroup Mapping
        /// </summary>
        /// <returns></returns>
        public List<DAndRAttributeGroupMappingContract> GetDAndRAttributeGroupMapping(Int32 systemDocumentId)
        {
            return BackgroundSetupManager.GetDAndRAttributeGroupMapping(systemDocumentId);
        }
        /// <summary>
        /// Gets ServiceAttributeGroup
        /// </summary>
        /// <returns></returns>
        public List<Entity.BkgSvcAttributeGroup> GetServiceAttributeGroup()
        {
            return BackgroundSetupManager.GetServiceAttributeGroup().ToList();
        }
        /// <summary>
        /// Gets the Get ServiceAttribute By ServiceGroupID
        /// </summary>
        /// <returns></returns>
        public List<Entity.BkgSvcAttribute> GetServiceAttributeByServiceGroupID(Int32 BkgSvcAGID)
        {
            return BackgroundSetupManager.GetServiceAttributeByServiceGroupID(BkgSvcAGID).ToList();
        }

        /// <summary>
        /// Update D & R Mapping
        /// </summary>
        /// <returns></returns>
        //public Boolean UpdateMapping(Int32 systemDocumentID, Int32 bkgSvcAttributeGroupID, Int32 bkgSvcAttributeID, Int32 specialFieldType_ID, Boolean rbApplicantAttr)
        public Boolean UpdateMapping(DAndRAttributeGroupMappingContract dAndRContract)
        {
            //return BackgroundSetupManager.UpdateMapping(systemDocumentID, bkgSvcAttributeGroupID, bkgSvcAttributeID, View.CurrentLoggedInUserId, specialFieldType_ID, rbApplicantAttr);
            return BackgroundSetupManager.UpdateMapping(dAndRContract,View.CurrentLoggedInUserId);
        }

        public List<Entity.lkpDisclosureDocumentSpecialFieldType> GetSpecialFieldType()
        {
            return BackgroundSetupManager.GetSpecialFieldType();
        }

        public List<Entity.Tenant> GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            return SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        public List<Entity.ClientEntity.CustomAttribute> GetCustomAttributesByTenantID(int tenantID)
        {
            if (tenantID > AppConsts.NONE)
            {
                return ComplianceDataManager.GetCustomAttributesByTenantID(tenantID);
            }
            else
            {
                return new List<Entity.ClientEntity.CustomAttribute>();
            }
        }
    }
}
