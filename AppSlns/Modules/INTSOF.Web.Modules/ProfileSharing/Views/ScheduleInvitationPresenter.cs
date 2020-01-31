using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public class ScheduleInvitationPresenter : Presenter<IScheduleInvitation>
    {

        public List<RotationAndTrackingPkgStatusContract> GetComplianceStatusOfImmunizationAndRotationPackages()
        {
            return ProfileSharingManager.GetComplianceStatusOfImmunizationAndRotationPackages(View.TenantID, View.DelimittedOrgUserIDs, View.DelimittedTrackingPkgIDs, View.RotationID, "IMNZ");
        }

        public Boolean IsTrackingPkgCompliantReqd()
        {
            return ProfileSharingManager.IsTrackingPkgCompliantReqd(View.AgencyID, View.TenantID, View.RotationID);
        }

        public List<Int32> AnyAgencyUserExists(int clientID, string agencyIds)
        {
            return ProfileSharingManager.AnyAgencyUserExists(clientID, agencyIds);
        }

        public List<ClinicalRotationAgencyContract> GetAgenciesMappedWithRotation(Int32 selectedTenantID, Int32 rotationId)
        {
            return ClinicalRotationManager.GetAgenciesMappedWithRotation(selectedTenantID, rotationId);
        }

        #region [UAT-3045]
        public void GetNonComplianceCategoryList(SharingPackageSelectedDataContract _sharingSelectedData, String selectedCategoriesXml)
        {
            View.ProfileSharingExpiryContract = ClinicalRotationManager.GetNonComplianceCategoryList(_sharingSelectedData.TenantId, _sharingSelectedData.RotationId, _sharingSelectedData.lstSelectedApplicants, selectedCategoriesXml);
        }
        #endregion
    }
}
