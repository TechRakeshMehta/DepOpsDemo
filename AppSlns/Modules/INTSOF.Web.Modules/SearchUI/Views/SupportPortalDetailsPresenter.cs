#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;
using System.Linq;
#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity;
using INTSOF.UI.Contract.ProfileSharing;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.UI.Contract.SysXSecurityModel;
#endregion

#endregion



namespace CoreWeb.SearchUI.Views
{
    public class SupportPortalDetailsPresenter : Presenter<ISupportPortalDetailsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetOrganizationUser();
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

        public void GetOrganizationUser()
        {
            View.OrganizationUser = SecurityManager.GetOrganizationUser(View.OrganizationUserId);
        }

        //UAT-3407
        //unused code
        //public void SetPageControls()
        //{
        //    OrganizationUser organizationUser = GetOrganizationUser();

        //    if (!organizationUser.IsNullOrEmpty())
        //    {
        //        View.OrganizationUser = organizationUser;
        //    }
        //}
        //UAT 2367
        /// <summary>
        /// Gets the list of invitations that has been sent by the applicant
        /// </summary>
        public void BindInvitations()
        {
            View.lstInvitationsSent = ProfileSharingManager.GetApplicantInvitations(View.OrganizationUserId, View.SelectedTenantId, AppConsts.ONE);
        }

        /// <summary>
        /// UAT 1882
        /// </summary>
        /// <param name="rotationID"></param>
        public void GetAttestationDocumentsToExport(Int32 psiID)
        {
            ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>>();
            serviceRequest.Parameter1 = new Dictionary<String, Int32> {
                                                                       { AppConsts.PROFILE_SHARING_INVITATION_ID, psiID },
                                                                       { AppConsts.IGNORE_AGENCY_USER_CHECK, AppConsts.ONE }
                                                                      };
            serviceRequest.Parameter2 = new List<Tuple<Int32, Int32, Int32>>();
            View.LstInvitationDocumentContract = ClinicalRotationManager.GetAttestationDocumentsToExport(serviceRequest, View.OrganizationUserId);
        }

        /// <summary>
        /// Get Switching Target Url
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public String GetSwitchingTargetUrl(Int32 tenantID)
        {
            return WebSiteManager.GetInstitutionUrl(tenantID);
        }
        /// <summary>
        /// Get Data by Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<String, ApplicantInsituteDataContract> GetDataByKey(String key)
        {
            Object applicationData = ApplicationDataManager.GetObjectDataByKey(key);
            return ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
        }

        /// <summary>
        /// Update Web Application Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.UpdateWebApplicationData(key, serializedData);
        }
        /// <summary>
        /// Add Web Application Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
        }
        /// <summary>
        /// Check if the user belongs to Multi Tenants
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean IsMultiTenantUser(Guid userId)
        {
            return SecurityManager.IsMultiTenantUser(userId);
        }
        public void GetTenants()
        {
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetUserTenants(View.OrgUserId).Where(x => x.lkpTenantType.TenantTypeCode == clientCode).ToList();
        }

        public void GetSupportPortalOrderDetal()
        {

            View.lstSuportPortalOrderData = ComplianceDataManager.GetSupportPortalOrderDetail(View.SelectedTenantId, View.OrganizationUserId);


        }

        public Entity.ClientEntity.OrganizationUser GetOrganizationUserByUserID(String userId, Int32 tenantId, Boolean isApplicant)
        {
            return ComplianceDataManager.GetOrganizationUserByUserID(userId, tenantId,isApplicant);
        }
    }
    
}
