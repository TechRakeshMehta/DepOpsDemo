using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IDRDocsEntityMappingView
    {
        List<DRDocsMappingContract> lstDRDocsMappingDetail { get; set; }

        List<LookupContract> lstElements { get; set; }

        List<LookupContract> BindCountryDropdown { set; }
        List<LookupContract> BindStateDropdown { set; }
        List<LookupContract> BindServiceDropdown { set; }
        List<LookupContract> BindRegulatoryEntityTypeDropdown { set; }
        List<LookupContract> BindDocumentsDropdown { set; }
        List<LookupContract> BindTenantsDropdown { set; }

        Int32 SelectedTenantID { get; set; }
        Int32 SelectedCountryID { get; set; }
        Int32 SelectedStateID { get; set; }
        Int32 SelectedServiceID { get; set; }
        Int16 SelectedRegulatoryEntityTypeID { get; set; }
        Int32 SelectedDRDocumentID { get; set; }
        Int32 loggedInUserId { get; }
        //UAT-3157
        Int32 PreferredSelectedTenantID { get; set; }
    }
}
