using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace CoreWeb.Shell.Views
{
    public class AlumniPopupPresenter : Presenter<IAlumniPopupView>
    {
        public Boolean UpdateAlumniStatusinOrganizationUserAlumnAccess(String alumniStatusCode)
        {
            return AlumniManager.UpdateAlumniStatusinOrganizationUserAlumnAccess(View.OrgUserId, View.TenantId, alumniStatusCode, View.LoggedInUserID);
        }

        public Boolean CreateAlumniDefaultSubscription(String machineIP)
        {
            return AlumniManager.CreateAlumniDefaultSubscription(View.TenantId, View.LoggedInUserID, View.OrgUserId, machineIP);
        }

        /// <summary>
        /// Get alumni tenant Id from alumnisettings table from security database.
        /// </summary>
        public void GetAlumniTenantId()
        {
            String AlumniTenantCode = AlumniSettings.AlumniTenantID.GetStringValue();
            View.AlumniTenantId = Convert.ToInt32(AlumniManager.GetAlumniSettingByCode(AlumniTenantCode));
        }

        /// <summary>
        /// Get the Institute Url to which the applicant should be redirected to
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public String GetApplicationUrl()
        {
            return WebSiteManager.GetInstitutionUrl(View.AlumniTenantId);
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
        public void AddWebApplicationData(String key, Dictionary<String, ApplicantInsituteDataContract> value)
        {
            Dictionary<String, String> serializedData = ApplicationDataManager.SerializeDictionaryValues<ApplicantInsituteDataContract>(value);
            ApplicationDataManager.AddWebApplicationData(key, serializedData, 300);
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
        /// Performs the log off.
        /// </summary>
        /// <remarks></remarks>
        public void DoLogOff(bool isLoggedIn, Int32 userLoginHistoryID)
        {
            //IssessionTimeout will be false as user is switching institution.
            UpdateUserLoginActivity(false, View.OrgUserId, userLoginHistoryID);
            if (isLoggedIn && !View.CurrentSessionId.IsNullOrEmpty())
            {
                View.ViewStateProvider.Delete(View.CurrentSessionId);
            }
            SysXWebSiteUtils.SessionService.ClearSession(true);
            FormsAuthentication.SignOut();
            SysXAppDBEntities.ClearContext();
        }

        /// <summary>
        /// Update user logout time in User Login Activity
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="currentSessionId"></param>
        /// <param name="isSessionTimeout"></param>
        public void UpdateUserLoginActivity(Boolean isSessionTimeout, Int32 currentLogedInUserId, Int32 userLoginHistoryID)
        {
            SecurityManager.UpdateUserLoginActivity(currentLogedInUserId, View.CurrentSessionId, isSessionTimeout, userLoginHistoryID);
        }

    }
}
