using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.SystemSetUp.Views
{
    public interface IInstitutionConfigurationDetailsView
    {
        IInstitutionConfigurationDetailsView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        String PageType { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        Int32 ParentID { get; set; }
        Int32 NodeId { get; set; }
        Int32 OrganizationUserID { get; set; }
        String NodeLabel { get; set; }
        InstitutionConfigurationDetailsContract InstitutionConfigurationDetailsContract { get; set; }
        List<InstitutionConfigurationPackageDetails> InstitutionConfigurationPackageDetailsList { get; set; }
        List<InstitutionConfigurationAdministratorDetails> InstitutionConfigurationAdministratorDetailsList { get; set; }
        String SplashScreenURL { get; set; }
        //UAT-1758 : Add Overall compliance note change to incomplete after a resubmit setting to Client settings, also display this setting on the Institution Configuration search
        String OverallComplianceStatus { set; }
    }
}

