using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;


namespace CoreWeb.ComplianceAdministration.Views
{
    public class TrackingPackageDetailsPopUpPresenter : Presenter<ITrackingPackageDetailsPopUpView>
    {
        public List<TrackingPackageRequiredContract> GetPackagesIDs()
        {
            try
            {
                return View.lstPackagesIDs = ComplianceDataManager.GetTrackingPackageRequired(View.TenantId, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CompliancePackage> GetPackagesNames()
        {
            try
            {
                return View.lstPackagesNames = ComplianceDataManager.GetCompliancePackagesForTrackingRequired(View.TenantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
