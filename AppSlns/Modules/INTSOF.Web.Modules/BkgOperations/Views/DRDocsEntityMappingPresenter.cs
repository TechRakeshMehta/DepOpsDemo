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


namespace CoreWeb.BkgOperations.Views
{
    public class DRDocsEntityMappingPresenter : Presenter<IDRDocsEntityMappingView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            GetTenantList();
            View.BindTenantsDropdown = View.lstElements;
        }

        public void GetDRDocumentEntityMappingList()
        {
            DRDocsMappingObjectIds docEntityMappingFilters = new DRDocsMappingObjectIds();

            docEntityMappingFilters.CountryId = View.SelectedCountryID;
            docEntityMappingFilters.StateId = View.SelectedStateID;
            docEntityMappingFilters.ServiceId = View.SelectedServiceID;
            //docEntityMappingFilters.InstitutionHierarchyId = View.SelectedInstitutionHierarchyID;
            docEntityMappingFilters.RegulatoryEntityTypeId = View.SelectedRegulatoryEntityTypeID;
            docEntityMappingFilters.DocumentId = View.SelectedDRDocumentID;
            docEntityMappingFilters.TenantId = View.SelectedTenantID;
            View.lstDRDocsMappingDetail = BackgroundProcessOrderManager.GetDRDocumentEntityMappingList(docEntityMappingFilters);
        }

        public void GetAllCountriesList()
        {
            View.lstElements = SecurityManager.GetCountries().Select(country => new LookupContract()
             {
                 Name = country.FullName,
                 ID = country.CountryID
             }).ToList(); //.OrderBy(x => x.Name).ToList();
            if (View.lstElements == null)
                View.lstElements = new List<LookupContract>();
            else
                View.lstElements.Insert(0, new LookupContract { Name = AppConsts.ALL_COUNTRIES_TEXT, ID = 0 });
        }

        public void GetAllStatesByCountryId(Int32 countryId)
        {
            View.lstElements = SecurityManager.GetStates().Where(state => state.CountryID == countryId && state.StateID != 0).Select(st => new LookupContract()
            {
                Name = st.StateName,
                ID = st.StateID
            }).OrderBy(x => x.Name).ToList();
            if (View.lstElements == null)
                View.lstElements = new List<LookupContract>();
        }

        public void GetTenantList()
        {
            View.lstElements = ComplianceDataManager.getClientTenant().Select(cond => new LookupContract
            {
                Name = cond.TenantName,
                ID = cond.TenantID
            }).ToList();
            if (View.lstElements == null)
                View.lstElements = new List<LookupContract>();
        }

        public void GetRegulatoryEntityTypeList()
        {
            View.lstElements = LookupManager.GetLookUpData<Entity.lkpRegulatoryEntityType>().Select(cond => new LookupContract()
            {
                Name = cond.RET_Name,
                ID = cond.RET_ID
            }).OrderBy(x => x.Name).ToList();
            if (View.lstElements == null)
                View.lstElements = new List<LookupContract>();
        }

        public void GetAllDisclosureDocumentsList()
        {
            Int32 docTypeId = BackgroundSetupManager.GetDocumentTypeIDByCode(DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue());
            View.lstElements = BackgroundSetupManager.GetDisclosureTemplateDocuments(docTypeId).Select(cond => new LookupContract()
            {
                Name = cond.FileName,
                ID = cond.SystemDocumentID
            }).OrderBy(x => x.Name).ToList();
            if (View.lstElements == null)
                View.lstElements = new List<LookupContract>();
        }

        public Boolean SaveUpdateDRDocumentsEntityMapping(DRDocsMappingObjectIds docsMappingObjectIds)
        {
            return BackgroundProcessOrderManager.SaveUpdateDRDocumentsEntityMapping(docsMappingObjectIds, View.loggedInUserId);
        }

        public Boolean DeleteDRDocumentsEntityMapping(Int32 disclosureDocumentMappingId)
        {
            return BackgroundProcessOrderManager.DeleteDRDocumentsEntityMapping(disclosureDocumentMappingId, View.loggedInUserId);
        }

        public void GetBackgroundServiceList()
        {
            View.lstElements = BackgroundSetupManager.GetMasterServices().Select(cond => new LookupContract
            {
                Name = cond.BSE_Name,
                ID = cond.BSE_ID
            }).OrderBy(ordBy=>ordBy.Name).ToList();
        }

        public String GetDeptProgMappingLabel(Int32 NodeId, Int32 tenantId)
        {
            return ComplianceSetupManager.GetDeptProgMappingLabel(NodeId, tenantId);
        }

    }
}
