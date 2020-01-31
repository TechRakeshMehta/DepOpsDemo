using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.ProfileSharing.Views
{
    public class ManageAttestationStatementPresenter : Presenter<IManageAttestationStatementView>
    {
        public override void OnViewInitialized()
        {

        }

        public override void OnViewLoaded()
        {

        }

        public void GetAttestationReportText()
        {
            View.LstAgencyAttestation = ProfileSharingManager.GetAttestationReportTextForAgencyUser(View.UserID);
        }

        public void UpdateAttestationReportTextForAgencyUser()
        {
            View.SuccessMessage = ProfileSharingManager.UpdateAttestationReportTextForAgencyUser(View.LoggedInUserID, View.AgencyId, View.AttestationReportText);
        }
    }
}
