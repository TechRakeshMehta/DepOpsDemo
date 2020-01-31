using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.UI.Contract.ProfileSharing;
using Entity.SharedDataEntity;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;

namespace CoreWeb.SearchUI.Views
{
    public class AgencyUserPermissionTemplatePresenter : Presenter<IAgencyUserPermissionTemplateView>
    {
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public override void OnViewLoaded()
        {

        }

        #region PUBLIC METHODS
        /// <summary>
        /// Check whether logged in user is admin or not.
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }




        //public void GetlstAgencyUserPermissionTemplate()
        //{
        //    //View.LstAgencyUsrPerTemplate =
        //      var _data =  ProfileSharingManager.GetlstAgencyUserPermissionTemplate();
        //    if (View.LstAgencyUsrPerTemplate.IsNullOrEmpty())
        //    {
        //        View.LstAgencyUsrPerTemplate = new List<AgencyUserPermissionTemplate>();
        //    }
        //}
        public void GetlstAgencyUserPermissionTemplate()
        {
            AgencyUserPermissionTemplateContract searchContract = new AgencyUserPermissionTemplateContract();

            searchContract.AGUPT_Name = View.TemplateName.IsNullOrEmpty() ? null : View.TemplateName;
            searchContract.AGUPT_Description = View.TemplateDescription.IsNullOrEmpty() ? null : View.TemplateDescription;
            var _data = ProfileSharingManager.GetlstAgencyUserPermissionTemplate(searchContract, View.GridCustomPaging);

            View.LstAgencyUsrPerTemplate = _data.Item1.ToList();
            View.CurrentPageIndex = _data.Item2;
            View.LstAgencyUsrPerTemplateMapping = _data.Item4.ToList();
            View.LstAgencyUsrPerTemplateNotification = _data.Item5.ToList();
            if (View.LstAgencyUsrPerTemplate.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
                View.LstAgencyUsrPerTemplate = new List<AgencyUserPermissionTemplateContract>();

            }
            else
            {
                View.VirtualRecordCount = _data.Item3;
            }
        }

        /// <summary>
        /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        /// </summary>
        /// <param name="code"></param>
        public void GetAgencyUserPermissionAccessTypeID(String code)
        {
            View.AgencyUserPermissionAccessTypeId = ProfileSharingManager.GetAgencyUserPermissionAccessTypeID(code);
        }

        /// <summary>
        /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        /// </summary>
        /// <param name="code"></param>
        public void GetAgencyUserPermissionTypeID(String code)
        {
            View.AgencyUserPermissionTypeId = ProfileSharingManager.GetAgencyUserPermissionTypeID(code);
        }
        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public Int32 GetCreatedByClientID()
        {
            if (View.IsAgencyUserLoggedIn || IsDefaultTenant)
            {
                return AppConsts.NONE;
            }
            else
            {
                return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            }

        }
        /// <summary>
        /// Save Record.
        /// </summary>
        /// <param name="_agencyUser"></param>
        public void SaveAgencyUserPerTemplate(AgencyUserPermissionTemplateContract _agencyUserPerTemplate, List<AgencyUserPermissionTemplateMapping> lstAgencyUserPerTempMapping, Dictionary<Int32, Boolean> dicNotificationData)
        {
            List<Int32> lstAgencyHierarchy = new List<Int32>();

            Int32 agencyUserTemplateID = ProfileSharingManager.SaveAgencyUserPerTemplate(View.TenantId, _agencyUserPerTemplate, View.CurrentLoggedInUserId, lstAgencyUserPerTempMapping);
            String status = String.Empty;

            if (agencyUserTemplateID > AppConsts.NONE && !dicNotificationData.IsNullOrEmpty())
            {
                ProfileSharingManager.SaveAgencyUserTemplateNotificationMappings(agencyUserTemplateID, dicNotificationData, View.CurrentLoggedInUserId);
                status = AppConsts.AGUPT_SAVED_SUCCESS_MSG;
            }
            else
            {
                status = AppConsts.AGUPT_SAVED_ERROR_MSG;
            }

            if (status == AppConsts.AGUPT_SAVED_SUCCESS_MSG)
            {
                ////UAT- 2631 
                //if (!_agencyUser.lstAgencyHierarchyIds.IsNullOrEmpty())
                //{
                //    Dictionary<String, Object> agencyUserData = new Dictionary<String, Object>();
                //    agencyUserData.Add("agencyUser", _agencyUser);
                //    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                //    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                //    //ParallelTaskContext.PerformParallelTask(CopySharedInvForNewlyMappedAgencyForAgencyUser, agencyUserData, LoggerService, ExceptiomService);
                //}
                #region UAT-3719
                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                Dictionary<String, Object> agencyUserAuditData = new Dictionary<String, Object>();
                agencyUserAuditData.Add("AgencyUserTemplateID", agencyUserTemplateID);
                agencyUserAuditData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                ParallelTaskContext.PerformParallelTask(SaveAgencyUserPermissionAuditDetails, agencyUserAuditData, LoggerService, ExceptiomService);
                #endregion

                View.SuccessMessage = status;
            }
            else
            {
                View.ErrorMessage = status;
            }
        }


        /// <summary>
        /// Update Record.
        /// </summary>
        /// <param name="_agencyUser"></param>
        public void UpdateAgencyUserPerTemplate(AgencyUserPermissionTemplateContract _agencyUserTemplate, List<AgencyUserPermissionTemplateMapping> lstAgencyUserPermission, Dictionary<Int32, Boolean> dicNotificationData)
        {

            //String status = ProfileSharingManager.UpdateAgencyUser(View.TenantId, _agencyUser, View.CurrentLoggedInUserId, IsDefaultTenant, agencyInstitutionIDs_Added, agencyInstitutionIDs_Removed, lstAgencyUserPermission);
            AgencyUserPermissionTemplate updatedAgencyUserPerTemplate = ProfileSharingManager.UpdateAgencyUserPermissionTemplate(View.TenantId, _agencyUserTemplate, View.CurrentLoggedInUserId,
                (IsDefaultTenant || View.IsAgencyUserLoggedIn), lstAgencyUserPermission);

            //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
            if (!dicNotificationData.IsNullOrEmpty() && _agencyUserTemplate.AGUPT_ID > AppConsts.NONE)
            {
                ProfileSharingManager.UpdateAgencyUserPerTemplateNotificationMappings(_agencyUserTemplate.AGUPT_ID, dicNotificationData, View.CurrentLoggedInUserId);
            }

            if (!updatedAgencyUserPerTemplate.IsNullOrEmpty())
            {
                Boolean isMailSuccessfullySent = true;

                #region UAT-3719
                if (updatedAgencyUserPerTemplate.AGUPT_ID > AppConsts.NONE)
                {
                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                    Dictionary<String, Object> agencyUserAuditData = new Dictionary<String, Object>();
                    agencyUserAuditData.Add("AgencyUserTemplateID", updatedAgencyUserPerTemplate.AGUPT_ID);
                    agencyUserAuditData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                    ParallelTaskContext.PerformParallelTask(SaveAgencyUserPermissionAuditDetails, agencyUserAuditData, LoggerService, ExceptiomService);
                }
                #endregion

                if (isMailSuccessfullySent)
                {
                    View.SuccessMessage = AppConsts.AGUPT_UPDATED_SUCCESS_MSG;
                }

            }
            else
            {
                View.ErrorMessage = AppConsts.AGUPT_UPDATED_ERROR_MSG;
            }
        }

        /// <summary>
        /// Delete Record.
        /// </summary>
        public void DeleteAgencyUserPermissionTemplate()
        {
            // AgencyUserPermissionTemplate _prevAgencyUser = View.LstAgencyUsrPerTemplate.Where(c => c.AGUPT_ID == View.AGUPT_ID).FirstOrDefault();
            String status = ProfileSharingManager.DeleteAgencyUserPermissionTemplate(View.TenantId, View.AGUPT_ID, View.CurrentLoggedInUserId, IsDefaultTenant);
            if (status == AppConsts.AGUPT_DELETED_SUCCESS_MSG)
            {
                //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
                ProfileSharingManager.DeleteAgencyUserPerTemplateNotificationMappings(View.AGUPT_ID, View.CurrentLoggedInUserId);

                //UAT- 2631 Digestion Process  
                // AgencyHierarchyManager.CallDigestionProcess(String.Join(",", _prevAgencyUser.lstAgencyHierarchyIds), AppConsts.CHANGE_TYPE_AGENCYUSER, View.CurrentLoggedInUserId);

                View.SuccessMessage = status;
            }
            else
            {
                View.ErrorMessage = status;
            }

        }

        public void GetAgencyUserNotifications()
        {
            View.lstAgencyUserNotification = ProfileSharingManager.GetAgencyUserNotifications();
        }

        /// <summary>
        /// Get list of Invitation meta Data which user want to share.
        /// </summary>
        public void GetApplicantInvitationMetaData()
        {
            View.LstSharedInfo = ProfileSharingManager.GetApplicantMetaData();
        }

        /// <summary>
        /// Get permissions for Compliance and Backaground
        /// </summary>
        public void GetSharedInfo()
        {
            View.LstSharedInfoType = ProfileSharingManager.GetSharedInfoType();
        }
        #endregion

        #region UAT-3719
        public void SaveAgencyUserPermissionAuditDetails(Dictionary<String, Object> dic)
        {
            Int32 AgencyUserTemplateID = Convert.ToInt32(dic.GetValue("AgencyUserTemplateID"));
            Int32 CurrentLoggedInUserId = Convert.ToInt32(dic.GetValue("CurrentLoggedInUserId"));
            ProfileSharingManager.SaveAgencyUserPermissionAuditDetails(null, AgencyUserTemplateID, CurrentLoggedInUserId);
            ProfileSharingManager.UpdateDocMappingForInvAttestation(null, AgencyUserTemplateID, CurrentLoggedInUserId);

        }
        #endregion

        #region UAT-3664

        public void GetAgencyUserReports()
        {
            View.lstAgencyUserReports = ProfileSharingManager.GetAgencyUserReports();
        }

        public List<AgencyUserPermissionTemplateMapping> GetAgencyUserTemplateReportPermissions(Int32 templateId)
        {
            View.lstTemplateReportPermissions = ProfileSharingManager.GetAgencyUserTemplateReportPermissions(templateId);

            List<AgencyUserPermissionTemplateMapping> lstNotAccessTypeReport = new List<AgencyUserPermissionTemplateMapping>();
            if (!View.lstTemplateReportPermissions.IsNullOrEmpty())
            {
                String noAccessTypePermission = AgencyUserPermissionAccessType.NO.GetStringValue();
                lstNotAccessTypeReport = View.lstTemplateReportPermissions.Where(cond => cond.lkpAgencyUserPermissionAccessType.AUPAT_Code == noAccessTypePermission).ToList();
            }
            return lstNotAccessTypeReport;
        }

        #endregion

        //UAT-4658
        public Boolean IsAgencyUserPresent(Int32 templateId)
        {
            return ProfileSharingManager.IsAgencyUserPresent(templateId);
        }
        //End UAT-4658
    }
}
