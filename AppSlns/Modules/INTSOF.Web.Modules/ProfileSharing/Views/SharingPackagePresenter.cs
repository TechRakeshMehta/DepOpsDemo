using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public class SharingPackagePresenter : Presenter<ISharingPackageView>
    {
        /// <summary>
        /// Get the Compliance & Background Packages (and their related data) for the
        /// selected applicants, out of which admin can select which category/service group etc can be shared - UAT 1324
        /// </summary>
        public void GetPackages()
        {
            var _tplPackages = StoredProcedureManagers.GetSharingPackageData(View.lstApplicantIds, View.TenantId);
            View.lstCompliancePackages = _tplPackages.Item1.Where(pkg => pkg.IsCompliancePkg).ToList();
            View.lstBkgPackages = _tplPackages.Item1.Where(pkg => !pkg.IsCompliancePkg).ToList();
            View.lstRequirementPackages = _tplPackages.Item2;
        }
    }
}
