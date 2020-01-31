using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public class ProfileSharingHistoryPresenter : Presenter<IProfileSharingHistory>
    {
        /// <summary>
        /// Method To Get Data to bind Profile Sharing History Grid
        /// </summary>
        public void GetProfileSharingData()
        {
            List<ProfileSharingDataContract> lstProfileSharingData = ProfileSharingManager.GetProfileSharingData(View.TenantID, View.InvitationGroupID);
            if (!lstProfileSharingData.IsNullOrEmpty() && lstProfileSharingData.Count > 0)
                View.LstProfileSharingData = lstProfileSharingData;
            else
                View.LstProfileSharingData = new List<ProfileSharingDataContract>();
        }

        public void SaveUpdateProfileExpirationCriteria()
        {
            //View.ExpirationCriteriaDetail;
            View.Success = ProfileSharingManager.SaveUpdateProfileExpirationCriteria(View.ExpirationCriteriaDetail, View.SelectedInvitationIds);
        }

        #region UAT-2784
        public Boolean CheckExpirationCriteria()
        {
            String ExpirationCriterialSettingCode = AgencyHierarchySettingType.EXPIRATION_CRITERIA.GetStringValue();
            String ExpirationCriteriaSettingValue = ProfileSharingManager.GetAgencySetting(View.AgencyID, ExpirationCriterialSettingCode);
            if (!ExpirationCriteriaSettingValue.IsNullOrEmpty())
                return ExpirationCriteriaSettingValue == "1" ? true : false;
            return true;
        }
       #endregion
    }
}
