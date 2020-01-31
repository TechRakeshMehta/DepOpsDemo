using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public class SharedInvitationsPresenter : Presenter<ISharedInvitations>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            GetTenants();
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        public void GetAllAgency()
        {
            if (View.SelectedTenantID == 0)
                View.lstAgency = new List<AgencyDetailContract>();
            else
            {
                //UAT-1881
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                IsAdminLoggedIn();
                if (View.IsAdminLoggedIn)
                {
                    serviceRequest.Parameter = View.SelectedTenantID;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencies(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
                else
                {
                    serviceRequest.SelectedTenantId = View.SelectedTenantID;
                    serviceRequest.Parameter = View.CurrentUserID;
                    var _serviceResponse = _clinicalRotationProxy.GetAllAgencyByOrgUser(serviceRequest);
                    View.lstAgency = _serviceResponse.Result;
                }
            }
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.ClientTenantID);
        }

        /// <summary>
        /// Private Method to get ClientID based on logged in user
        /// </summary>
        /// <returns></returns>
        private Int32 GetClientID()
        {
            Int32 clientID = 0;
            if (View.IsAdminLoggedIn)
            {
                clientID = View.SelectedTenantID;
            }
            else
            {
                clientID = View.ClientTenantID;
            }
            return clientID;
        }

        ///// <summary>
        ///// UAT-1201 - Method to Bind Attestation Details Grid
        ///// </summary>
        //public void GetAttestationDetailsData()
        //{
        //    Int32 clientID = GetClientID();
        //    View.LstInvitationGroup = ProfileSharingManager.GetAttestationDetailsData(clientID, View.CurrentUserID);

        //    #region UAT-1313 - Setting TenantName to the extended entity column of ProfileSharingInvitationGroup
        //    if (!View.LstInvitationGroup.IsNullOrEmpty())
        //    {
        //        GetTenants();
        //    }
        //    foreach (var invitationGroup in View.LstInvitationGroup)
        //    {
        //        invitationGroup.TenantName = View.lstTenant.Where(cond => cond.TenantID == invitationGroup.PSIG_TenantID).Select(col => col.TenantName).FirstOrDefault();
        //    }
        //    #endregion
        //}


        /// <summary>
        /// UAT-1201 - Method to Bind Attestation Details Grid
        /// </summary>
        public void GetAttestationDetailsData()
        {
            Int32 clientID = GetClientID();

            if (View.SearchContract.IsNotNull())
                View.SearchContract.SelectedTenantID = clientID;

            View.LstInvitationGroup = ProfileSharingManager.GetSharedInvitationsData(View.SearchContract, View.GridCustomPaging);

            if (View.LstInvitationGroup.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = View.LstInvitationGroup[0].TotalRecordCount;
            }
        }

        public void GetAttestatationDocumentDetails()
        {
            View.LstInvitationDocument = ProfileSharingManager.GetAttestatationDocumentDetails(View.SelectedInvitationGroupID);
        }
    }
}
