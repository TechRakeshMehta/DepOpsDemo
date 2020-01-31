using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.SystemSetUp.Views
{
    public class InstitutionConfigurationDetailsPresenter : Presenter<IInstitutionConfigurationDetailsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetInstitutionConfigurationDetails()
        {
            View.InstitutionConfigurationDetailsContract = ComplianceSetupManager.GetInstitutionConfigurationDetails(View.DeptProgramMappingID, View.SelectedTenantId);
            if ( !View.InstitutionConfigurationDetailsContract.IsNullOrEmpty())
            {
                View.InstitutionConfigurationPackageDetailsList = View.InstitutionConfigurationDetailsContract.PackageDetailsList;
                View.InstitutionConfigurationAdministratorDetailsList = View.InstitutionConfigurationDetailsContract.AdministratorsDetailsList;
                View.SplashScreenURL = View.InstitutionConfigurationDetailsContract.SplashScreenURL;
                Int32 overallCompStatusValue = Convert.ToInt32(ComplianceDataManager.GetOvralCompStatusFromClientSetting(View.SelectedTenantId));
                View.OverallComplianceStatus = Convert.ToBoolean(overallCompStatusValue) ? AppConsts.YES : AppConsts.NO;
            }
            else
            {
                View.InstitutionConfigurationPackageDetailsList = new List<InstitutionConfigurationPackageDetails>();
                View.InstitutionConfigurationAdministratorDetailsList =new List<InstitutionConfigurationAdministratorDetails>();
                View.SplashScreenURL = String.Empty;
                View.OverallComplianceStatus = AppConsts.NO;
            }
        
        }
    }
}

