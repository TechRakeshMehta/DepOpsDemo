using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
   public class TrackingPackageRequiredPresenter : Presenter<ITrackingPackageRequiredItemsView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
        }

        public void GetTrackingPackageRequired(string SelectedPackageIDs)
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            View.listTrackingPackageRequiredContract = ComplianceDataManager.GetTrackingPackageRequired(View.SelectedTenantId, SelectedPackageIDs);
        }

        public void GetPackage()
        {
            try
            {
                Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
                View.listPackage = ComplianceDataManager.GetCompliancePackagesForTrackingRequired(View.SelectedTenantId);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }
        public void DeletePackageAndURLData()
        
        {
            View.ViewContract.TenantId = View.SelectedTenantId;
            View.IsOperationSuccessful = ComplianceDataManager.DeleteComplianceItem(View.SelectedTenantId, View.ViewContract, View.CurrentLoggedInUserId);
        }

        public void SavePackageRequired()
        {
            try
            {
                View.ViewContract.TenantId = View.SelectedTenantId;
                View.IsOperationSuccessful = ComplianceDataManager.SaveComplianceItem(View.SelectedTenantId, View.ViewContract, View.CurrentLoggedInUserId);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public Boolean CheckDuplicateRecords()
        {
            try
            {
                View.ViewContract.TenantId = View.SelectedTenantId;
                return ComplianceDataManager.CheckDuplicateRecords(View.SelectedTenantId, View.ViewContract, View.CurrentLoggedInUserId);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        //UAT-4249
        /// <summary>
        /// To get compliance packages List
        /// </summary>
        public void GetCompliancePackages()
        {
            List<CompliancePackage> tempListPackages = new List<CompliancePackage>();
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                //tempListPackages = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId, false).Select(pak => new LookupContract()
                //{
                //    Name = pak.PackageName,
                //    ID = pak.CompliancePackageID
                //}).OrderBy(x => x.Name).ToList();
                tempListPackages = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId, false);
            }
            View.ListCompliancePackages = tempListPackages.OrderBy(x => x.PackageName).ToList();
        }

    }
}
