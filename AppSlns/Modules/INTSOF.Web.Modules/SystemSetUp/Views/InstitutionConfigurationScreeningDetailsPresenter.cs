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
    public class InstitutionConfigurationScreeningDetailsPresenter : Presenter<IInstitutionConfigurationScreeningDetailsView>
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
            //View.ScreeningDetailsForConfigurationContract = ComplianceSetupManager.GetScreeningDetailsForInstitutionConfiguration(View.DeptProgramMappingID,View.PackageID, View.SelectedTenantId);
            View.ScreeningDetailsForConfigurationContract = ComplianceSetupManager.GetScreeningDetailsForInstitutionConfiguration(View.DeptProgramMappingID, View.PackageID, View.SelectedTenantId, View.PackageHierarchyID);
            if (!View.ScreeningDetailsForConfigurationContract.IsNullOrEmpty())
            {
                View.BackgroundPackageDetailsForConfigurationList = View.ScreeningDetailsForConfigurationContract.BackgroundPackageDetailsList;
                View.ServiceFormDetailsForConfigurationList = View.ScreeningDetailsForConfigurationContract.ServiceFormDetailsList;
                if (!View.BackgroundPackageDetailsForConfigurationList.IsNullOrEmpty())
                {
                    //UAT:2411
                    if (!string.IsNullOrEmpty(View.ParentScreenName))
                        View.NodeLabel = "Node: " + View.BackgroundPackageDetailsForConfigurationList.FirstOrDefault().HierarchyLabel + " > " + View.BundleName + " > " + View.BackgroundPackageDetailsForConfigurationList.FirstOrDefault().PackageName;
                    else
                    View.NodeLabel = "Node: " + View.BackgroundPackageDetailsForConfigurationList.FirstOrDefault().HierarchyLabel + " > " + View.BackgroundPackageDetailsForConfigurationList.FirstOrDefault().PackageName;
                }
                View.ServiceItemFeeDetailsList = View.ScreeningDetailsForConfigurationContract.ServiceItemFeeDetailsList;
            }
            else
            {
                View.BackgroundPackageDetailsForConfigurationList = new List<BackgroundPackageDetailsForConfigurationContract>();
                View.ServiceFormDetailsForConfigurationList = new List<ServiceFormDetailsForConfigurationContract>();
                View.ServiceItemFeeDetailsList = new List<ServiceItemFeeDetailsForConfigurationContract>();
            }

        }
    }
}


