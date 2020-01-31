using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SysXSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class ExternalLoginPresenter : Presenter<IExternalLoginView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetInstitutionUrlByExternalUserTenantID()
        {
            View.WebsiteLoginUrl = WebSiteManager.GetInstitutionUrl(View.ExternalUserTenantId);
        }
        public void GetInstitutionUrlBySchoolNameTenantID()
        {
            View.WebsiteLoginUrl = WebSiteManager.GetInstitutionUrl(View.TenantId);
        }
        public void ExternalUserTenantId()
        {
            var result = SecurityManager.ExternalUserTenantId(View.ExternalID, View.mappingCode);
            View.ExternalUserTenantId = result.Item1; // External ID
            View.IntegrationClientId = result.Item2; // Integration clientID
        }

        public void GetTenantIdBySchoolName()
        {
            Dictionary<Int32, Int32> UserDetails = new Dictionary<Int32, Int32>();
            UserDetails = SecurityManager.ExternalUserTenantIdBySchoolName(View.Token, View.SchoolName);
            View.TenantId = UserDetails.FirstOrDefault().Key;
            View.IntegrationClientId = UserDetails.FirstOrDefault().Value;
        }

        public Boolean ValidateToken()
        {
            return SecurityManager.ValidateToken(View.Token);
        }
        public void GetDataFromSecurityToken()
        {
            View.ExternalDataList = SecurityManager.GetDataFromSecurityToken(View.Token);
        }
        public Boolean IsStudentEmailDOBLastNameExist()
        {
            return SecurityManager.IsStudentEmailDOBLastNameExist(View.TenantId, View.Email1, View.LastName, View.DOB);
        }
        public Entity.lkpSysXBlock GetDefaultLineOfBusinessByUserName(String username)
        {
            return SecurityManager.GetDefaultLineOfBusinessByUserName(username, View.TenantId);
        }
        public void ResetPasswordAttempCount(String username)
        {
            SecurityManager.ResetPasswordAttempCount(username);
        }

        public String GetUserNameByExternald()
        {
            return SecurityManager.GetUserNameByExternald(View.ExternalID, View.IntegrationClientId);
        }

        /// <summary>
        /// Get the Institute Url to which the applicant should be redirected to
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public String GetInstitutionUrl()
        {
            return WebSiteManager.GetInstitutionUrl(View.TenantId);
        }
        /// <summary>
        /// Get the applicant data from the 'WebApplicationData' table, before being re-directed to the apporpriate Url,
        /// in case of incorrect url selection for login
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<String, ApplicantInsituteDataContract> GetDataByKey(String key)
        {
            Object applicationData = ApplicationDataManager.GetObjectDataByKey(key);
            return ApplicationDataManager.DeserializeDictionaryValues<ApplicantInsituteDataContract>(applicationData);
        }

        /// <summary>
        /// Add the applicant data to 'WebApplicationData' table, before being redirected to appropriate Url, 
        /// if it is already not added with the same key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.UpdateWebApplicationData(key, serializedData);
        }
        /// <summary>
        /// Add the applicant data to 'WebApplicationData' table, before being redirected to appropriate Url, 
        /// if it is already not added with the same key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
        }


        public List<ExternalLoginDataContract> GetMatchingOrganizationUserDetails()
        {
            INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract objExternalLoginDataContract = new INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract();
            objExternalLoginDataContract.FirstName = View.FirstName;
            objExternalLoginDataContract.LastName = View.LastName;
            objExternalLoginDataContract.DOB = Convert.ToDateTime(View.DOB);
            objExternalLoginDataContract.SSN = View.SSN;
            objExternalLoginDataContract.Email1 = View.Email1;
            objExternalLoginDataContract.Email2 = View.Email2;
            objExternalLoginDataContract.UserName = View.UserName;
            objExternalLoginDataContract.TenantID = View.TenantId;
            objExternalLoginDataContract.IntegrationClientID = View.IntegrationClientId;
            //Get Matching user list
            return SecurityManager.GetMatchingOrganisationUserList(objExternalLoginDataContract);
        }
        public List<ExternalLoginDataContract> GetMatchingOrganisationUserListForCoreLinking()
        {
            INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract objExternalLoginDataContract = new INTSOF.UI.Contract.SysXSecurityModel.ExternalLoginDataContract();
            objExternalLoginDataContract.FirstName = View.FirstName;
            objExternalLoginDataContract.LastName = View.LastName;
            objExternalLoginDataContract.DOB = Convert.ToDateTime(View.DOB);
            objExternalLoginDataContract.SSN = View.SSN;
            objExternalLoginDataContract.Email1 = View.Email1;
            objExternalLoginDataContract.Email2 = View.Email2;
            objExternalLoginDataContract.UserName = View.UserName;
            objExternalLoginDataContract.TenantID = View.TenantId;
            objExternalLoginDataContract.IntegrationClientID = View.IntegrationClientId;
            //Get Matching user list
            return SecurityManager.GetMatchingOrganisationUserListForCoreLinking(objExternalLoginDataContract);
        }
    }
}
