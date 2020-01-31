using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using Business.RepoManagers;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class AgencyHierarchyUserPermissionPresenter : Presenter<IAgencyHierarchyUserPermissionView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetSharedInfo()
        {
            View.LstSharedInfoType = ProfileSharingManager.GetSharedInfoType();
        }

        /// <summary>
        /// Get list of Invitation meta Data which user want to share.
        /// </summary>
        public void GetApplicantInvitationMetaData()
        {
            View.LstSharedInfo = ProfileSharingManager.GetApplicantMetaData();
        }

        public Boolean SaveUpdateAgencyHierarchyUserMapping()
        {
            ServiceRequest<AgencyHierarchyUserContract> serviceRequest = new ServiceRequest<AgencyHierarchyUserContract>();
            serviceRequest.Parameter = View.agencyHierarchyUserContract;
            Boolean result = _agencyHierarchyProxy.SaveUpdateAgencyHierarchyUserMapping(serviceRequest).Result;

            //#region Parallel Task For Calling of Digestion SP for Agency Hierarchy
            //Dictionary<String, Object> param = new Dictionary<String, Object>();
            ////param.Add("param", param);
            //var LoggerService = (HttpContext.Current.ApplicationInstance as INTSOF.Contracts.IWebApplication).LoggerService;
            //var ExceptiomService = (HttpContext.Current.ApplicationInstance as INTSOF.Contracts.IWebApplication).ExceptionService;
            //INTSOF.ServiceUtil.ParallelTaskContext.PerformParallelTask(CallDigestionStoreProcedureFunctionForAgencyHierarchy, param, LoggerService, ExceptiomService);
            //#endregion 

            if (result)
            {
                Dictionary<String, Object> param = new Dictionary<String, Object>();
                param.Add("AgencyHierarchyId", View.agencyHierarchyUserContract.AgencyHierarchyID.ToString());
                param.Add("ChangeType", AppConsts.CHANGE_TYPE_AGENCYUSER);
                param.Add("CurrentUserId", View.CurrentLoggedInUserId);
                AgencyHierarchyManager.ExecuteDigestionProcess(param);

                //AgencyHierarchyManager.CallDigestionProcess(View.agencyHierarchyUserContract.AgencyHierarchyID.ToString(),AppConsts.CHANGE_TYPE_AGENCYUSER,View.CurrentLoggedInUserId);

                if (!View.agencyHierarchyUserContract.AgencyHierarchyID.IsNullOrEmpty())
                {
                    Dictionary<String, Object> agencyUserData = new Dictionary<String, Object>();
                    agencyUserData.Add("agencyUser", View.agencyHierarchyUserContract);
                    agencyUserData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                    ParallelTaskContext.PerformParallelTask(CopySharedInvForNewlyMappedAgencyForAgencyUser, agencyUserData, LoggerService, ExceptiomService);
                }
            }
            return result;
        }


        public void CallDigestionStoreProcedureFunctionForAgencyHierarchy(Dictionary<String, Object> param)
        {
            AgencyHierarchyManager.CallDigestionStoreProcedureFunctionForAgencyHierarchy(param);
        }

        public Boolean DeleteAgencyHierarchyUserMapping()
        {
            ServiceRequest<AgencyHierarchyUserContract> serviceRequest = new ServiceRequest<AgencyHierarchyUserContract>();
            serviceRequest.Parameter = View.agencyHierarchyUserContract;
            Boolean result = _agencyHierarchyProxy.DeleteAgencyHierarchyUserMapping(serviceRequest).Result;

            if (result)
            {
                Dictionary<String, Object> param = new Dictionary<String, Object>();
                param.Add("AgencyHierarchyId", View.agencyHierarchyUserContract.AgencyHierarchyID.ToString());
                param.Add("ChangeType", AppConsts.CHANGE_TYPE_AGENCYUSER);
                param.Add("CurrentUserId", View.CurrentLoggedInUserId);
                AgencyHierarchyManager.ExecuteDigestionProcess(param);

                //AgencyHierarchyManager.CallDigestionProcess(View.agencyHierarchyUserContract.AgencyHierarchyID.ToString(), AppConsts.CHANGE_TYPE_AGENCYUSER, View.CurrentLoggedInUserId);

                if (!View.agencyHierarchyUserContract.AgencyHierarchyID.IsNullOrEmpty())
                {
                    Dictionary<String, Object> agencyUserData = new Dictionary<String, Object>();
                    agencyUserData.Add("agencyUser", View.agencyHierarchyUserContract);
                    agencyUserData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                    ParallelTaskContext.PerformParallelTask(CopySharedInvForNewlyMappedAgencyForAgencyUser, agencyUserData, LoggerService, ExceptiomService);
                }
            }
            return result;
        }

        public void GetAgencyUsers(AgencyHierarchyUserContract agencyUserContract)
        {
            List<AgencyHierarchyUserContract> lstAgencyUsers = new List<AgencyHierarchyUserContract>();
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.AgencyHierarchyId;
            lstAgencyUsers = _agencyHierarchyProxy.GetAgencyUsers(serviceRequest).Result;
            if (!agencyUserContract.IsNullOrEmpty() && agencyUserContract.AGU_ID > 0)
            {
                View.lstAgencyHierarchyUsers.Remove(View.lstAgencyHierarchyUsers.Where(cond => cond.AGU_ID == agencyUserContract.AGU_ID).FirstOrDefault());
            }
            View.lstAgencyUsers = lstAgencyUsers.Where(cond => !View.lstAgencyHierarchyUsers.Select(sel => sel.AGU_ID).Contains(cond.AGU_ID)).ToList();
        }

        public void GetAgencyHirarchyAgencyUsers()
        {
            List<AgencyHierarchyUserContract> lstAgencyHierarchyPackageList = new List<AgencyHierarchyUserContract>();
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.AgencyHierarchyId;
            lstAgencyHierarchyPackageList = _agencyHierarchyProxy.GetAgencyHierarchyUsers(serviceRequest).Result;
            View.lstAgencyHierarchyUsers = lstAgencyHierarchyPackageList;
        }

        public void CopySharedInvForNewlyMappedAgencyForAgencyUser(Dictionary<String, Object> agencyUserData)
        {
            AgencyHierarchyUserContract agencyUser = agencyUserData.GetValue("agencyUser") as AgencyHierarchyUserContract;
            Int32 currentLoggedInUserId = Convert.ToInt32(agencyUserData.GetValue("CurrentLoggedInUserId"));
            //Int32 addedAgencyUserID;
            //if (agencyUser.AGU_ID > AppConsts.NONE)
            //{
            //    addedAgencyUserID = agencyUser.AGU_ID;
            //}

            //else
            //{
            //    agencyUserID = ProfileSharingManager.GetAgencyUserByEmail(agencyUser.AGU_Email);
            //}

            List<AgencyInstitution> lstAgencyInstitution = ProfileSharingManager.GetAgencyInstitutionForAgencyuser(agencyUser.AGU_ID);

            foreach (var agencyInstitutionDetails in lstAgencyInstitution.GroupBy(cond => cond.AGI_TenantID).ToList())
            {
                String agencyIds = String.Join(",", agencyInstitutionDetails.Select(sel => sel.AGI_AgencyID).Distinct().ToList());
                Int32 tenantID = agencyInstitutionDetails.FirstOrDefault().AGI_TenantID.Value;
                if (!agencyIds.IsNullOrEmpty())
                {
                    ProfileSharingManager.CopySharedInvForNewlyMappedAgencyForAgencyUser(agencyUser.AGU_ID, agencyIds, tenantID);
                    ProfileSharingManager.UpdateDocMappingForInvAttestation(agencyUser.AGU_ID, null, currentLoggedInUserId);
                }
            }
        }

        //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        public void GetAgencyUserNotifications()
        {
            View.lstAgencyUserNotification = ProfileSharingManager.GetAgencyUserNotifications();
        }

        public void GetAgencyUserPerTemplates()
        {
            List<AgencyUserPermissionTemplateContract> lstPermisisonTemplateContract = new List<AgencyUserPermissionTemplateContract>();
            List<AgencyUserPermissionTemplate> lstAgencyUserPerTemplate = ProfileSharingManager.GetAgencyUserPermissionTemplates();

            foreach (AgencyUserPermissionTemplate perTemp in lstAgencyUserPerTemplate)
            {
                AgencyUserPermissionTemplateContract objContract = new AgencyUserPermissionTemplateContract();
                objContract.AGUPT_ID = perTemp.AGUPT_ID;
                objContract.AGUPT_Name = perTemp.AGUPT_Name;
                objContract.AGUPT_Description = perTemp.AGUPT_Description;
                objContract.AGUPT_IsDeleted = perTemp.AGUPT_IsDeleted;
                objContract.AGUPT_ReqRotationSharedInfoTypeID = perTemp.AGUPT_ReqRotationSharedInfoTypeID;
                objContract.AGUPT_ComplianceSharedInfoTypeID = perTemp.AGUPT_ComplianceSharedInfoTypeID;
                lstPermisisonTemplateContract.Add(objContract);
            }

            View.lstAgencyUserPerTemplates = lstPermisisonTemplateContract;
        }

        public void GetAgencyUsrPerTemplateMappings(Int32 PermisisonTemplateId)
        {
            View.lstAgencyUserPerTemplatesMappings = ProfileSharingManager.GetAgencyUsrPerTemplateMappings(PermisisonTemplateId);
        }
        public void GetAgencyUsrPerTemplateNotificationsMappings(Int32 PermisisonTemplateId)
        {
            View.lstAgencyUserPerTemplatesNotificationMappings = ProfileSharingManager.GetAgencyUsrPerTemplateNotificationsMappings(PermisisonTemplateId);
        }

        public void GetAgencyUserPermissionTypes()
        {
            View.lstAgencyUserNotification = ProfileSharingManager.GetAgencyUserNotifications();
            View.lstAgencyUserPermisisonType = ProfileSharingManager.GetAgencyUserPermissionTypes();
        }

        public void GetInvitationSharedInfoTypeID(Int32 templateID)
        {
            View.InvitationSharedInfoTypeIDs = ProfileSharingManager.GetInvitationSharedInfoTypeID(templateID);
        }
        public void GetApplicationInvitationMetaDataID(Int32 templateID)
        {
            View.ApplicantInvMetaDataIDs = ProfileSharingManager.GetApplicationInvitationMetaDataID(templateID);
        }

        #region UAT-3664

        public void GetAgencyUserReports()
        {
            View.lstAgencyUserReports = ProfileSharingManager.GetAgencyUserReports();
        }

        public List<AgencyUserReportPermissionContract> GetAgencyUserReportsWithNoAccess(Int32 agencyUserId)
        {
            View.lstAgencyUserReportPermission = ProfileSharingManager.GetAgencyUserReportPermissions(agencyUserId);
            List<AgencyUserReportPermissionContract> lstNotAccessTypeReport = new List<AgencyUserReportPermissionContract>();
            if (!View.lstAgencyUserReportPermission.IsNullOrEmpty())
            {
                String noAccessTypePermission = AgencyUserPermissionAccessType.NO.GetStringValue();
                lstNotAccessTypeReport = View.lstAgencyUserReportPermission.Where(cond => cond.PermissionAccessTypeCode == noAccessTypePermission).ToList();
            }
            return lstNotAccessTypeReport;
        }

        public void GetAgencyUserTemplateReportPermissions(Int32 templateId)
        {
            View.lstTemplateReportPermissions = ProfileSharingManager.GetAgencyUserTemplateReportPermissions(templateId);
        }
      

        #endregion
    }
}

