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
using INTSOF.UI.Contract.SysXSecurityModel;

#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public class ManageInvitationsSharedUserPresenter : Presenter<IManageInvitationsSharedUserView>
    {
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
        public Entity.SharedDataEntity.AgencyUser GetAgencyUserByUserID()
        {
            return ProfileSharingManager.GetAgencyUserByUserID(View.CurrentUserId);
        }
        #region UAT-3316(Ability to create Agency User permission "templates"_
        public AgencyUsrTempPermisisonsContract GetAgencyUserPermisisonTemplateMappings(Int32 templateId)
        {
            List<AgencyUserPermissionTemplateMapping> templatePermisisons = ProfileSharingManager.GetAgencyUsrPerTemplateMappings(templateId).ToList();
            AgencyUsrTempPermisisonsContract objPerTemplates = new AgencyUsrTempPermisisonsContract();
            //GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue());
            Int32 AGUPerAccessTypeId = ProfileSharingManager.GetAgencyUserPermissionAccessTypeID(AgencyUserPermissionAccessType.YES.GetStringValue());

            if (templatePermisisons.Count > 0)
            {
                objPerTemplates.AGU_RotationPackagePermission = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ROTATION_PACKAGE_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_RotationPackageViewPermission = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ROTATION_PACKAGE_VIEW_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_AllowJobPosting = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ALLOW_JOB_POSTING_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_AgencyApplicantStatus = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.AGENCY_APPLICANT_STATUS_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_AgencyUserPermission = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.AGENCY_USER_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AGUPTM_IsDeleted));
                objPerTemplates.AGU_AttestationReport = Convert.ToBoolean(templatePermisisons.Any(x => x.AGUPTM_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue()) && x.AGUPTM_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AGUPTM_IsDeleted));
            }
            return objPerTemplates;
        }

        public AgencyUsrTempPermisisonsContract GetAgencyUsrPermisisonMappings(Int32 usrId)
        {
            List<AgencyUserPermission> usrPermisisons = ProfileSharingManager.GetAgencyUsrPermisisonMappings(usrId).ToList();
            AgencyUsrTempPermisisonsContract objPerTemplates = new AgencyUsrTempPermisisonsContract();
            Int32 AGUPerAccessTypeId = ProfileSharingManager.GetAgencyUserPermissionAccessTypeID(AgencyUserPermissionAccessType.YES.GetStringValue());

            if (usrPermisisons.Count > 0)
            {
                objPerTemplates.AGU_RotationPackagePermission = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ROTATION_PACKAGE_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_RotationPackageViewPermission = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ROTATION_PACKAGE_VIEW_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_AllowJobPosting = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ALLOW_JOB_POSTING_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_AgencyApplicantStatus = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.AGENCY_APPLICANT_STATUS_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_AgencyUserPermission = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.AGENCY_USER_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AUP_IsDeleted));
                objPerTemplates.AGU_AttestationReport = Convert.ToBoolean(usrPermisisons.Any(x => x.AUP_PermissionTypeID == ProfileSharingManager.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue()) && x.AUP_PermissionAccessTypeID == AGUPerAccessTypeId && !x.AUP_IsDeleted));
            }
            return objPerTemplates;
        }
        #endregion
    }
}
