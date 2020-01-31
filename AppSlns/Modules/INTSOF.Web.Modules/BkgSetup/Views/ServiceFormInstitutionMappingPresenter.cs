using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;


namespace CoreWeb.BkgSetup.Views
{
    public class ServiceFormInstitutionMappingPresenter : Presenter<IServiceFormInstitutionMappingView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            GetAllDropDownList();
        }

        public void GetAllDropDownList()
        {
            View.lstElements = ComplianceDataManager.getClientTenant().Select(cond => new LookupContract
            {
                Name = cond.TenantName,
                ID = cond.TenantID
            }).ToList();
            if (View.lstElements == null)
                View.lstElements = new List<LookupContract>();

            View.BindTenantsDropdown = View.lstElements;

            View.lstBackgroundServiceMapping = BackgroundSetupManager.GetBackgroundServiceMapping(SecurityManager.DefaultTenantID).OrderBy(ordBy=>ordBy.BSE_Name).ToList();

            View.lstServiceForm = BackgroundSetupManager.GetServiceForm(SecurityManager.DefaultTenantID).OrderBy(ordBy=>ordBy.SF_Name).ToList();

            View.lstMappingType = BackgroundSetupManager.GetMappingType(SecurityManager.DefaultTenantID);
        }

        public void GetServiceFormMappingAllandSpecificInstitution()
        {
            if (View.MappingTypeID != null && View.lstElements.IsNotNull() && View.lstElements.Count > 0)
            {
                Int32 tenantId = View.SelectedTenantId > 0 ? View.SelectedTenantId : View.lstElements[0].ID;
                View.lstServiceFormInstitutionMapping = BackgroundSetupManager.GetServiceFormMappingAllandSpecificInstitution(tenantId, View.ServiceFormID, View.ServiceID, View.MappingTypeID, View.DPM_ID, View.SelectedTenantId);
            }
        }

        public Boolean DeleteServiceFormInstitutionMapping(Int32 serviceFormMappingID, Int32? serviceFormHierarchyMappingID, Int32 currentUserId)
        {
            Int32 tenantId = View.SelectedTenantId > 0 ? View.SelectedTenantId : View.lstElements[0].ID;
            return BackgroundSetupManager.DeleteServiceFormInstitutionMapping(tenantId, serviceFormMappingID, serviceFormHierarchyMappingID, View.MappingTypeID.Value, currentUserId);
        }

        public String SaveServiceFormInstitutionMapping(ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId)
        {
            Int32 tenantId = View.SelectedTenantId > 0 ? View.SelectedTenantId : View.lstElements[0].ID;
            return BackgroundSetupManager.SaveServiceFormInstitutionMapping(tenantId, svcFormInstitutionMappingContract, currentUserId);
        }

        public String UpdateServiceFormInstitutionMapping(ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId)
        {
            Int32 tenantId = View.SelectedTenantId > 0 ? View.SelectedTenantId : View.lstElements[0].ID;
            return BackgroundSetupManager.UpdateServiceFormInstitutionMapping(tenantId, svcFormInstitutionMappingContract, currentUserId);
        }

        public List<Int32> GetServiceIdsByServiceForm(Int32 serviceFormID)
        {
            return BackgroundSetupManager.GetServiceIdsByServiceForm(SecurityManager.DefaultTenantID, serviceFormID);
        }

        public String GetDeptProgMappingLabel(Int32 NodeId, Int32 tenantId)
        {
            return ComplianceSetupManager.GetDeptProgMappingLabel(NodeId, tenantId);
        }
    }
}
