using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IServiceFormInstitutionMappingView
    {
        //Int32? NodeId { get; set; }
        Int32? ServiceFormID { get; set; }
        Int32? ServiceID { get; set; }
        Int32? MappingTypeID { get; set; }
        Int32? DPM_ID { get; set; }
        List<ServiceFormInstitutionMappingContract> lstServiceFormInstitutionMapping { get; set; }
        List<ServiceForm> lstServiceForm { get; set; }
        List<SvcFormMappingType> lstMappingType { get; set; }
        List<BackgroundServiceMapping> lstBackgroundServiceMapping { get; set; }
        List<LookupContract> lstElements { get; set; }
        Int32 SelectedTenantId { get; set; }
        Int32 TenantId { get; set; }
        List<LookupContract> BindTenantsDropdown { set; }        
    }
}
