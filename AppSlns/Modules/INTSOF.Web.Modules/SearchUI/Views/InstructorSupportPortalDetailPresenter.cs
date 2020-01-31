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
using INTSOF.ServiceDataContracts.Modules.ClientContact;

#endregion

#endregion

namespace CoreWeb.SearchUI.Views
{
    public class InstructorSupportPortalDetailPresenter : Presenter<IInstructorSupportPortalDetailView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetUserData();
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

        public void GetRotationDetail()
        {
            if (IsAdminLoggedIn())
                View.ClinicalRotationData = ApplicantClinicalRotationManager.GetInstructorClinicalRotationDetails(View.SelectedTenantId, View.OrganizationUserId, null);
            else
                View.ClinicalRotationData = ApplicantClinicalRotationManager.GetApplicantClinicalRotationDetails(View.TenantId, View.OrganizationUserId, View.CurrentLoggedInUserId);
        }
        public void GetUserData()
        {
            View.OrganisationUser = ClientContactManager.GetUserData(View.OrganizationUserId);
        }
        public void UpdateClientContactOrganisationUser()
        {
            ClientContactManager.UpdateClientContactOrganisationUser(View.OrganisationUser, View.SelectedTenantId, Convert.ToString(View.CurrentLoggedInUserId));
        }
        public void AddImpersonationHistory(String UserID, Int32 CurrentLoggedInUserID)
        {
            Int32 userId = SecurityManager.GetOrganizationUserInfoByUserId(UserID.ToString()).FirstOrDefault(x => x.IsSharedUser == true).OrganizationUserID;
            SecurityManager.AddImpersonationHistory(userId, CurrentLoggedInUserID);
        }
        public bool IsAdminLoggedIn()
        {
            return SecurityManager.DefaultTenantID == View.LoggedInUserTenantId;
        }

        #region UAT-4313

        public void GetClientContactNotes()
        {
          View.ClientContactNotes = ApplicantClinicalRotationManager.GetClientContactNotes(View.SelectedTenantId, View.ClientContactId);          
        }

        public Boolean SaveClientContactNotes(ClientContactNotesContract ccNotesContract)
        {

            return ApplicantClinicalRotationManager.SaveClientContactNotes(ccNotesContract, View.CurrentLoggedInUserId, View.SelectedTenantId);

        }
        public Boolean DeleteClientContactNotes(Int32 clientCOntactNoteId)
        {

            return ApplicantClinicalRotationManager.DeleteClientContactNotes(clientCOntactNoteId, View.CurrentLoggedInUserId, View.SelectedTenantId);

        }

        
        #endregion
    }
}
