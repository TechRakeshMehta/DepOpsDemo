#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region UserDefined

using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using System.Data;
using System.Xml.Linq;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public class ManageInvitationExpirationPresenter : Presenter<IManageInvitationExpirationView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }
        
        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public override void OnViewInitialized()
        {
            //Get all tenants and filter them w.r.t Invitations
            //List<Entity.ClientEntity.Tenant> tenants = ComplianceDataManager.getClientTenant();
            //var profileSharingInvitations = ProfileSharingManager.GetInvitationsByInviteeOrgUserID(View.CurrentLoggedInUserId);

            //if (profileSharingInvitations.IsNotNull())
            //{
            //    var tenantIDs = profileSharingInvitations.Select(x => x.PSI_TenantID).Distinct().ToList();
            //    View.lstTenant = tenants.Where(x => tenantIDs.Contains(x.TenantID)).ToList();
            //}
            GetTenants();
        }

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
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            View.lstTenants = _serviceResponse.Result;
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public void PerformSearch()
        {
            List<ProfileSharingInvitationSearchContract> searchData = null;
            if (View.SelectedTenantId == 0)
            {
                searchData = new List<ProfileSharingInvitationSearchContract>();
            }
            else
            {
                View.SearchContract.TenantID = View.SelectedTenantId;
                ServiceRequest<ProfileSharingInvitationSearchContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<ProfileSharingInvitationSearchContract, CustomPagingArgsContract>();
                serviceRequest.Parameter1 = View.SearchContract;
                serviceRequest.Parameter2 = View.GridCustomPaging;
                var _serviceResponse = _clinicalRotationProxy.GetInvitationExpirationSearchData(serviceRequest);
                searchData = _serviceResponse.Result;
            }
            View.lstInvitationQueue = searchData;

            if (searchData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = searchData[0].TotalRecordCount;
            }
        }

        public void SaveUpdateProfileExpirationCriteria()
        {
            ServiceRequest<ProfileSharingInvitationSearchContract, List<Int32>> serviceRequest = new ServiceRequest<ProfileSharingInvitationSearchContract, List<Int32>>();
            serviceRequest.Parameter1 = View.ExpirationCriteriaDetail;
            serviceRequest.Parameter2 = View.SelectedInvitationIds;
            var _serviceResponse = _clinicalRotationProxy.SaveUpdateProfileExpirationCriteria(serviceRequest);
        }
    }
}
