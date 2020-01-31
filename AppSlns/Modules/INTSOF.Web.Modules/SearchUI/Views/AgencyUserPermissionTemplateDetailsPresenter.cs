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

namespace CoreWeb.SearchUI.Views
{
    public class AgencyUserPermissionTemplateDetailsPresenter : Presenter<IAgencyUserPermissionTemplateDetailsView>
    {
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public override void OnViewLoaded()
        {

        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public void GetlstAgencyUserPermissionTemplate()
        {
            var lstPermissionTemplates = ProfileSharingManager.GetAgencyUserPermissionTemplates();
           View.AgencyUsrPerTemplate = lstPermissionTemplates.Where(d => d.AGUPT_ID == View.CurrentTemplateID).FirstOrDefault();
            if (View.AgencyUsrPerTemplate.IsNullOrEmpty())
            {
                View.AgencyUsrPerTemplate = new AgencyUserPermissionTemplate();
            }
        }

        public void GetAgencyUserNotifications()
        {
            View.lstAgencyUserNotification = ProfileSharingManager.GetAgencyUserNotifications();
        }
        public void GetInvitationSharedInfoTypeID(Int32 templateID)
        {
            View.InvitationSharedInfoTypeIDs = ProfileSharingManager.GetInvitationSharedInfoTypeID(templateID);
        }
        public void GetApplicationInvitationMetaDataID(Int32 templateID)
        {
            View.ApplicantInvMetaDataIDs = ProfileSharingManager.GetApplicationInvitationMetaDataID(templateID);
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
    }
}
