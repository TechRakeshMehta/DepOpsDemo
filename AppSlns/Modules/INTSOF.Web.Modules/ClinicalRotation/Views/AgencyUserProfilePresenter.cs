using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public class AgencyUserProfilePresenter : Presenter<IAgencyUserProfileView>
    {
        private ClientContactProxy _clientContactProxy
        {
            get
            {
                return new ClientContactProxy();
            }
        }

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// Get Agency User data
        /// </summary>
        public void GetUserData()
        {
            View.OrganizationUser = ClientContactManager.GetUserData(View.OrganisationUserID);
            if (!View.IsApplicantsSharedUser)
                View.AgencyUserDetails = ProfileSharingManager.GetAgencyUserDetails(View.UserID);
        }

        /// <summary>
        /// Get Compliance, Backaground and Rotation permissions or Shared Info Types
        /// </summary>
        public void GetSharedInfoType()
        {
            View.LstSharedInfoType = ProfileSharingManager.GetSharedInfoType();
        }

        /// <summary>
        /// Update Agency User details
        /// </summary>
        public void UpdateAgencyUserDetails()
        {
            if (ClientContactManager.UpdateClientContactOrganisationUser(View.OrganizationUser, View.TenantID, View.UserID.ToString()))
            {
                //If master agency user then update permissions
                if (!View.IsApplicantsSharedUser && View.IsMasterAgencyUser)
                    ProfileSharingManager.UpdateAgencyUserDetails(View.AgencyUserDetails, View.UserID, View.CurrentLoggedInUserID);
            }
        }

    }
}
