using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ShotSeriesShuffleTestPresenter : Presenter<IShotSeriesShuffleTestView>
    {

        public override void OnViewLoaded()
        {
            //View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 getTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void GetSeriesDataForShuffleTest()
        {
            GetPackagesRelatedToCategory();
            List<CompliancePackage> tempPackageList = View.LstPackages;
            foreach (CompliancePackage pkg in tempPackageList)
            {
                pkg.PackageName = pkg.PackageLabel.IsNullOrEmpty() ? pkg.PackageName : pkg.PackageLabel;
            }
            View.LstPackages = tempPackageList;
            View.SeriesData = ComplianceSetupManager.GetSeriesDetailsForShuffleTest(View.CurrentSeriesID, View.SelectedTenantId);
            List<String> lstStatusCodeToBeExcluded = new List<String>();
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Exception_Approved.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue());
            View.LstItemComplianceStatus = ComplianceDataManager.GetItemComplianceStatus(View.SelectedTenantId)
                                                                .Where(cond => !lstStatusCodeToBeExcluded.Contains(cond.Code)).ToList();
            GetRuleDetails();

        }

        public void GetRuleDetails()
        {
            View.LstSeriesRuleDetails = ComplianceSetupManager.GetSeriesRuleDetailsForShuffleTest(View.CurrentSeriesID, View.SelectedTenantId, View.SelectedPackageID);
        }

        public void GetSeriesDetailsAfterShuffleTest(String seriesAttributeXML, String ruleMappingXML)
        {
            Dictionary<ShotSeriesSaveResponse, List<SeriesAttributeContract>> dicResponse = ComplianceSetupManager.GetSeriesDetailsAfterShuffleTest(View.CurrentSeriesID,
                                                                                         View.CurrentLoggedInUserId, seriesAttributeXML,
                                                                                         ruleMappingXML, View.SelectedTenantId, View.SelectedPackageID);
            View.SeriesDataAfterShuffle = dicResponse.Values.FirstOrDefault();
            View.ShotSeriesResponse = dicResponse.Keys.FirstOrDefault();
        }

        public void GetPackagesRelatedToCategory()
        {
            View.LstPackages = ComplianceSetupManager.GetPackagesRelatedToCategory(View.CategoryID, View.SelectedTenantId);
        }
    }
}

