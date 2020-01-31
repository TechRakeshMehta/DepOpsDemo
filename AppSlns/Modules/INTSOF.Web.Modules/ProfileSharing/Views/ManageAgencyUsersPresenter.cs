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

using Business.RepoManagers;
using INTSOF.UI.Contract.ProfileSharing;
using Entity;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using INTSOF.ServiceUtil;
using INTSOF.Contracts;
using System.Web;

#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public class ManageAgencyUsersPresenter : Presenter<IManageAgencyUsersView>
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



        ///// <summary>
        ///// Get list of Agencies for Dropdown.
        ///// </summary>
        //public void GetAgencies()
        //{
        //    if (IsDefaultTenant || View.IsAgencyUserLoggedIn)
        //    {
        //        //UAT- sort dropdowns by Name
        //        View.LstAgency = ProfileSharingManager.GetAgencies(View.TenantId, IsDefaultTenant, View.IsAgencyUserLoggedIn, View.UserID).OrderBy(x => x.AG_Name).ToList();
        //    }
        //    else
        //    {
        //        //For client admins
        //        View.LstAgency = ProfileSharingManager.GetAllAgencyForOrgUser(View.TenantId, View.CurrentLoggedInUserId);
        //    }
        //}

        /// <summary>
        /// Get list of Agency Institutions based on selected Agency.
        /// </summary>
        public void GetAgencyInstitutionForAgencies()
        {
            List<AgencyInstitution> tmplstInstitutions = ProfileSharingManager.GetAgencyInstitutionForAgencies(View.TenantId, View.SelectedAgencyID);
            TranslateToContract(tmplstInstitutions);
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

        #region UAT-3316
        /// <summary>
        /// Get permissions for Compliance and Backaground
        /// </summary>
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


        #endregion


        /// <summary>
        /// Get list AgencyUsers to bind Agency User Grid.
        /// </summary>
        public void GetAgencyUserInfo()
        {
            //GetAgencies();
            var customPaging = View.GridCustomPaging;
            View.LstAgencyUsers = ProfileSharingManager.GetAgencyUsersList(IsDefaultTenant, View.IsAgencyUserLoggedIn, View.UserID, View.TenantId, customPaging);
            View.VirtualRecordCount = View.GridCustomPaging.VirtualPageCount;
            //List<AgencyUser> tmpLstAgencyUsers = new List<AgencyUser>();
            //if (View.IsAgencyUserLoggedIn)
            //{
            //    tmpLstAgencyUsers = ProfileSharingManager.GetAgencyUserForSharedUser(View.UserID);
            //}
            //else
            //{
            //    tmpLstAgencyUsers = ProfileSharingManager.GetAgencyUserInfo(View.TenantId, IsDefaultTenant);
            //}

            //TranslateToAgencyUserContract(tmpLstAgencyUsers);
        }

        /// <summary>
        /// Save Record.
        /// </summary>
        /// <param name="_agencyUser"></param>
        public void SaveAgencyUser(AgencyUserContract _agencyUser, List<AgencyUserPermission> lstAgencyUserPermission, Dictionary<Int32, Boolean> dicNotificationData, Boolean IsTemplatePermissions)
        {
            //UAT-2640:
            List<Int32> lstAgencyHierarchy = new List<Int32>();
            if (!IsDefaultTenant && !_agencyUser.IsCreatedBySharedUser)
            {
                lstAgencyHierarchy.Add(View.SelectedAgencyHierarchyID);
                _agencyUser.lstAgencyHierarchyIds = lstAgencyHierarchy;
            }

            Int32 agencyUserID = ProfileSharingManager.SaveAgencyUser(View.TenantId, _agencyUser, View.CurrentLoggedInUserId, lstAgencyUserPermission);
            String status = String.Empty;

            //UAT- 2631 Digestion Process  without parallel tasking
            if (!_agencyUser.lstAgencyHierarchyIds.IsNullOrEmpty())
            {
                Dictionary<String, Object> param = new Dictionary<String, Object>();
                param.Add("AgencyHierarchyId", String.Join(",", _agencyUser.lstAgencyHierarchyIds.Distinct()));
                param.Add("ChangeType", AppConsts.CHANGE_TYPE_AGENCYUSER);
                param.Add("CurrentUserId", View.CurrentLoggedInUserId);
                AgencyHierarchyManager.ExecuteDigestionProcess(param);
            }

            //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
            if (IsTemplatePermissions == false && agencyUserID > AppConsts.NONE && !dicNotificationData.IsNullOrEmpty())
            {
                ProfileSharingManager.SaveAgencyUserNotificationMappings(agencyUserID, dicNotificationData, View.CurrentLoggedInUserId);
            }

            //UAT-2663
            if (_agencyUser.IsAutoAgencyActivation && agencyUserID > AppConsts.NONE)
            {
                //Check if it is case of account linking case
                if (!String.IsNullOrEmpty(View.SelectedLinkingProfileOrgUsername) && !View.ExistingOrganisationUser.IsNullOrEmpty())
                {

                    // View.ExistingOrganisationUser = SecurityManager.GetOrgUserIfUsernameExistInSecuritytDBForAccountLinking(View.SelectedLinkingProfileOrgUsername);
                    //SecurityManager.GetOrganizationUser(View.SelectedLinkingProfileOrgUserID);
                    if (AddSharedUserWithLinkedProfile(_agencyUser, agencyUserID))
                    {
                        status = AppConsts.AGU_SAVED_SUCCESS_MSG;
                    }
                }
                else
                {
                    if (AddSharedUser(_agencyUser, agencyUserID))
                    {
                        status = AppConsts.AGU_SAVED_SUCCESS_MSG;
                    }
                }
            }
            else if (agencyUserID > AppConsts.NONE)
            {
                #region Send Agency User Account Creation Email

                List<String> _lstAgencyName = ProfileSharingManager.GetAgencyByAgencyUserID(agencyUserID);
                _agencyUser.AgencyName = String.Join(",", _lstAgencyName);

                Boolean isMailSuccessfullySent = ProfileSharingManager.SendAgencyUserAccountCreationMail(_agencyUser, View.CurrentLoggedInUserId, agencyUserID);
                if (isMailSuccessfullySent)
                {
                    status = AppConsts.AGU_SAVED_SUCCESS_MSG;
                }

                #endregion
            }
            else
            {
                status = AppConsts.AGU_SAVED_ERROR_MSG;
            }

            if (status == AppConsts.AGU_SAVED_SUCCESS_MSG)
            {
                #region commented UAT-2641
                //UAT-2452
                //Dictionary<String, Object> agencyUserData = new Dictionary<String, Object>();
                //agencyUserData.Add("agencyUser", _agencyUser);
                //var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                //var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                //ParallelTaskContext.PerformParallelTask(CopySharedInvForNewlyMappedAgencyForAgencyUser, agencyUserData, LoggerService, ExceptiomService);
                #endregion

                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                //UAT- 2631 
                if (!_agencyUser.lstAgencyHierarchyIds.IsNullOrEmpty())
                {
                    Dictionary<String, Object> agencyUserData = new Dictionary<String, Object>();
                    agencyUserData.Add("agencyUser", _agencyUser);
                    agencyUserData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                    ParallelTaskContext.PerformParallelTask(CopySharedInvForNewlyMappedAgencyForAgencyUser, agencyUserData, LoggerService, ExceptiomService);
                }
                Dictionary<String, Object> agencyUserAuditData = new Dictionary<String, Object>();
                agencyUserAuditData.Add("AgencyUserID", agencyUserID);
                agencyUserAuditData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                ParallelTaskContext.PerformParallelTask(SaveAgencyUserPermissionAuditDetails, agencyUserAuditData, LoggerService, ExceptiomService);
                View.SuccessMessage = status;
            }
            else
            {
                View.ErrorMessage = status;
            }
        }

        public void BindExistingProfile()
        {
            View.ExistingOrganisationUser = SecurityManager.GetOrgUserIfUsernameExistInSecuritytDBForAccountLinking(View.SelectedLinkingProfileOrgUsername);
        }

        /// <summary>
        /// Delete Record.
        /// </summary>
        public void DeleteAgencyUser()
        {
            AgencyUserContract _prevAgencyUser = View.LstAgencyUsers.Where(c => c.AGU_ID == View.AGU_ID).FirstOrDefault();
            String status = ProfileSharingManager.DeleteAgencyUser(View.TenantId, View.AGU_ID, View.CurrentLoggedInUserId, _prevAgencyUser.lstSelectedAGI_ID, IsDefaultTenant);
            if (status == AppConsts.AGU_DELETED_SUCCESS_MSG)
            {
                //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
                if (_prevAgencyUser.AgencyUserTemplateId == 0)
                {
                    ProfileSharingManager.DeleteAgencyUserNotificationMappings(View.AGU_ID, View.CurrentLoggedInUserId);
                }
                #region 
                //UAT-4579 

                //else
                //{
                //    ProfileSharingManager.DeleteAgencyUserTemplateNotificationMappings(_prevAgencyUser.AgencyUserTemplateId, View.CurrentLoggedInUserId);
                //}

                #endregion
                //UAT- 2631 Digestion Process  
                AgencyHierarchyManager.CallDigestionProcess(String.Join(",", _prevAgencyUser.lstAgencyHierarchyIds), AppConsts.CHANGE_TYPE_AGENCYUSER, View.CurrentLoggedInUserId);

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
        public void UpdateAgencyUser(AgencyUserContract _agencyUser, List<AgencyUserPermission> lstAgencyUserPermission, Dictionary<Int32, Boolean> dicNotificationData, Boolean IsTemplatePermissions)
        {
            //List<Int32> agencyInstitutionIDs_Added = View.CurrentSelectedTenantIDs.Except(View.PrevSelectedTenantIDs).ToList();
            //List<Int32> agencyInstitutionIDs_Removed = View.PrevSelectedTenantIDs.Except(View.CurrentSelectedTenantIDs).ToList();
            //UAT-2640:
            List<Int32> lstAgencyHierarchy = new List<Int32>();
            if (!IsDefaultTenant && !_agencyUser.IsCreatedBySharedUser)
            {
                lstAgencyHierarchy.Add(View.SelectedAgencyHierarchyID);
                _agencyUser.lstAgencyHierarchyIds = lstAgencyHierarchy;
            }
            Boolean isAgencyUpdated = View.SelectedAgencyUser.lstAgencyHierarchyIds.Count != _agencyUser.lstAgencyHierarchyIds.Count;
            if (!_agencyUser.AGU_UserID.IsNullOrEmpty())
            {
                //UAT 1529 WB: Need to have active/inactive and locked/unlocked statuses for agency users on the manage agency users screen
                UpdateUser(_agencyUser.AGU_UserID, _agencyUser.IsActive, _agencyUser.IsLocked);
            }
            //String status = ProfileSharingManager.UpdateAgencyUser(View.TenantId, _agencyUser, View.CurrentLoggedInUserId, IsDefaultTenant, agencyInstitutionIDs_Added, agencyInstitutionIDs_Removed, lstAgencyUserPermission);
            AgencyUser updatedAgencyUser = ProfileSharingManager.UpdateAgencyUser(View.TenantId, _agencyUser, View.CurrentLoggedInUserId,
                (IsDefaultTenant || View.IsAgencyUserLoggedIn), lstAgencyUserPermission);

            //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
            if (IsTemplatePermissions == false && !dicNotificationData.IsNullOrEmpty() && _agencyUser.AGU_ID > AppConsts.NONE)
            {
                ProfileSharingManager.UpdateAgencyUserNotificationMappings(_agencyUser.AGU_ID, dicNotificationData, View.CurrentLoggedInUserId);

            }

            //UAT- 2631 Digestion Process  without parallel tasking  
            if (!_agencyUser.lstAgencyHierarchyIds.IsNullOrEmpty())
            {
                List<Int32> lstAgencyHierarchyIds = _agencyUser.lstAgencyHierarchyIds;
                lstAgencyHierarchyIds.AddRange(View.lstPrevAgencyHierarchyIds);
                Dictionary<String, Object> param = new Dictionary<String, Object>();
                param.Add("AgencyHierarchyId", String.Join(",", lstAgencyHierarchyIds.Distinct()));
                param.Add("ChangeType", AppConsts.CHANGE_TYPE_AGENCYUSER);
                param.Add("CurrentUserId", View.CurrentLoggedInUserId);
                AgencyHierarchyManager.ExecuteDigestionProcess(param);
            }
            //if (status == AppConsts.AGU_UPDATED_SUCCESS_MSG)
            if (!updatedAgencyUser.IsNullOrEmpty())
            {
                Boolean isMailSuccessfullySent = true;

                //UAT 1346: As an Agency user, I should be able to create and maintain other agency users.
                //Assign default roles to the agency shared user, valid user id/guid length would be 32 digits with 4 dashes
                if (!_agencyUser.AGU_UserID.IsNullOrEmpty() && _agencyUser.AGU_UserID != AppConsts.NON_BREAKING_SPACE) //NULL value is treated as &nbsp; in radgrid
                {
                    if (isAgencyUpdated)
                    {    //UAT-1641
                        var userAgencyMappings = updatedAgencyUser.UserAgencyMappings.Where(cond => !cond.UAM_IsVerified && !cond.UAM_IsDeleted)
                                                                                    .OrderByDescending(c => c.UAM_ID).FirstOrDefault();

                        #region Code commented for UAT-2541
                        //if (!userAgencyMappings.IsNullOrEmpty())
                        //{
                        // String verificationCode = Convert.ToString(userAgencyMappings.UAM_VerificationCode);
                        //  isMailSuccessfullySent = ProfileSharingManager.SendUserAgencyMappingMail(updatedAgencyUser, View.CurrentLoggedInUserId, verificationCode);
                        //} 
                        #endregion

                    }
                    OrganizationUser orgUser = SecurityManager.GetOrganizationUserInfoByUserId(_agencyUser.AGU_UserID).FirstOrDefault(x => x.IsSharedUser == true);
                    if (orgUser != null)
                    {
                        ApplicationDataManager.AssignDefaultRolesToAgencyUser(orgUser);
                    }
                }
                else if (isAgencyUpdated)
                {
                    //List<String> lstUserAgencyName = updatedAgencyUser.UserAgencyMappings.Where(cond => !cond.UAM_IsDeleted)
                    //                                                                                    .Select(col => col.Agency.AG_Name)
                    //                                                                                    .ToList();

                    List<String> lstUserAgencyName = ProfileSharingManager.GetAgencyByAgencyUserID(updatedAgencyUser.AGU_ID);

                    String agencyName = String.Empty;
                    if (!lstUserAgencyName.IsNullOrEmpty())
                    {
                        agencyName = String.Join(",", lstUserAgencyName);
                        AgencyUserContract agencyUser = new AgencyUserContract();
                        agencyUser.AGU_ID = updatedAgencyUser.AGU_ID;
                        agencyUser.AgencyName = agencyName;
                        agencyUser.AGU_Name = updatedAgencyUser.AGU_Name;
                        agencyUser.AGU_Email = updatedAgencyUser.AGU_Email;
                        isMailSuccessfullySent = SendAgencyUserAccountCreationMail(agencyUser);
                    }

                }

                if (isMailSuccessfullySent)
                {
                    View.SuccessMessage = AppConsts.AGU_UPDATED_SUCCESS_MSG;
                }
                //End

                #region commented UAT-2641
                //UAT-2452
                //Dictionary<String, Object> agencyUserData = new Dictionary<String, Object>();
                //agencyUserData.Add("agencyUser", _agencyUser);
                //var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                //var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                //ParallelTaskContext.PerformParallelTask(CopySharedInvForNewlyMappedAgencyForAgencyUser, agencyUserData, LoggerService, ExceptiomService);
                #endregion

                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                //UAT- 2631   
                if (!_agencyUser.lstAgencyHierarchyIds.IsNullOrEmpty())
                {
                    Dictionary<String, Object> agencyUserData = new Dictionary<String, Object>();
                    agencyUserData.Add("agencyUser", _agencyUser);
                    agencyUserData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                    ParallelTaskContext.PerformParallelTask(CopySharedInvForNewlyMappedAgencyForAgencyUser, agencyUserData, LoggerService, ExceptiomService);
                }
                #region UAT-3719
                if (_agencyUser.AGU_ID > AppConsts.NONE)
                {
                    Dictionary<String, Object> agencyUserAuditData = new Dictionary<String, Object>();
                    agencyUserAuditData.Add("AgencyUserID", _agencyUser.AGU_ID);
                    agencyUserAuditData.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                    ParallelTaskContext.PerformParallelTask(SaveAgencyUserPermissionAuditDetails, agencyUserAuditData, LoggerService, ExceptiomService);
                }
                #endregion
            }
            else
            {
                View.ErrorMessage = AppConsts.AGU_UPDATED_ERROR_MSG;
            }
        }

        /// <summary>
        /// Check the entered email is already exist or not.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsEmailAlreadyExist(string email, Int32? AGU_ID = null)
        {
            //Below Code is Commented for UAT-1218: Any User should be able to be 1 or more of the following: Applicant, Client admin, Agency User, Instructor/Preceprtor		

            //if (AGU_ID.IsNull()) //New User : Insert Mode
            //{
            //    return IsEmailExist(email, AGU_ID);
            //}
            //else
            //{
            //    //Update mode: Check whether email is updated or not.
            //    if (View.LstAgencyUsers.IsNotNull() && View.LstAgencyUsers.Count > AppConsts.NONE)
            //    {
            //        AgencyUserContract _prevAgencyUser = View.LstAgencyUsers.Where(c => c.AGU_ID == AGU_ID).FirstOrDefault();
            //        if (_prevAgencyUser.AGU_Email == email)
            //        {
            //            return false; // email is not updated.
            //        }
            //        else
            //        {
            //            return IsEmailExist(email, AGU_ID);
            //        }
            //    }
            //}
            //return true;
            return ProfileSharingManager.IsEmailAlreadyExistAgencyUser(View.TenantId, email);
        }

        public Boolean SendAgencyUserAccountCreationMail(AgencyUserContract agencyUser)
        {
            return ProfileSharingManager.SendAgencyUserAccountCreationMail(agencyUser, View.CurrentLoggedInUserId, agencyUser.AGU_ID);
        }

        //private Boolean IsEmailExist(String email, Int32? AGU_ID)
        //{
        //    string userName = System.Web.Security.Membership.GetUserNameByEmail(email);
        //    if (userName.IsNullOrEmpty())
        //    {
        //        UserAuthRequest tempObject = SecurityManager.GetUserAuthRequestByEmail(email);
        //        if (tempObject.IsNotNull())
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return ProfileSharingManager.IsEmailAlreadyExistAgencyUser(View.TenantId, email, AGU_ID);
        //        }
        //    }
        //    return true;
        //}
        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Translate AgencyInstitute to AgencyUserTenantCmbContract to bind Agency Dropdown.
        /// </summary>
        /// <param name="ListInstitutions"></param>
        //private void TranslateToContract(List<AgencyInstitution> ListInstitutions)
        //{
        //    List<AgencyUserTenantCmbContract> _tmplst = new List<AgencyUserTenantCmbContract>();
        //    foreach (AgencyInstitution _agencyInst in ListInstitutions)
        //    {
        //        AgencyUserTenantCmbContract _agContract = new AgencyUserTenantCmbContract();
        //        _agContract.AGI_ID = _agencyInst.AGI_ID;//AGI_TenantID.Value;
        //        List<Int32?> lstTenant = new List<Int32?>();
        //        lstTenant.Add(_agencyInst.AGI_TenantID.Value);
        //        _agContract.TenantName = GetInstitutionName(lstTenant);
        //        _agContract.Tenant_ID = _agencyInst.AGI_TenantID.Value;
        //        _tmplst.Add(_agContract);
        //    }
        //    View.LstAgencyInstitutions = _tmplst;
        //}

        private void TranslateToContract(List<AgencyInstitution> ListInstitutions)
        {

            List<AgencyInstitution> lstAgencyInstitutionsContractGrpByAgency = ListInstitutions.DistinctBy(cond => cond.Agency.AG_ID).ToList();

            List<AgencyInstitutionsContract> lstAgencyInstitutionsContract = new List<AgencyInstitutionsContract>();
            foreach (AgencyInstitution _agencyInst in lstAgencyInstitutionsContractGrpByAgency)
            {

                AgencyInstitutionsContract agencyInstitutionsContract = new AgencyInstitutionsContract();
                agencyInstitutionsContract.AgencyID = _agencyInst.Agency.AG_ID;
                agencyInstitutionsContract.AgencyName = _agencyInst.Agency.AG_Name;

                List<AgencyInstitution> lstAgencyInstitutionForAgencyID = ListInstitutions.Where(cond => cond.Agency.AG_ID == _agencyInst.Agency.AG_ID).ToList();

                List<AgencyUserTenantCmbContract> lstAgencyUserTenantCmbContract = new List<AgencyUserTenantCmbContract>();
                foreach (AgencyInstitution item in lstAgencyInstitutionForAgencyID)
                {
                    AgencyUserTenantCmbContract _agContract = new AgencyUserTenantCmbContract();
                    _agContract.AGI_ID = item.AGI_ID;//AGI_TenantID.Value;
                    List<Int32?> lstTenant = new List<Int32?>();
                    lstTenant.Add(item.AGI_TenantID.Value);
                    _agContract.TenantName = GetInstitutionName(lstTenant);
                    _agContract.Tenant_ID = item.AGI_TenantID.Value;
                    lstAgencyUserTenantCmbContract.Add(_agContract);
                }
                agencyInstitutionsContract.InstituteList = lstAgencyUserTenantCmbContract;
                lstAgencyInstitutionsContract.Add(agencyInstitutionsContract);
            }
            View.LstAgencyInstitutions = lstAgencyInstitutionsContract;
        }

        //private void TranslateToAgencyUserContract(List<AgencyUser> tmpLstAgencyUsers)
        //{
        //    List<AgencyUserContract> _tmplst = new List<AgencyUserContract>();
        //    List<String> lstUserIDs = tmpLstAgencyUsers.Where(cond => cond.AGU_UserID.IsNotNull()).Select(x => x.AGU_UserID.ToString()).ToList();
        //    List<AgencyUserPermissionContract> lstUserPermissionStatus = SecurityManager.GetOrganizationUserListByUserIds(lstUserIDs);


        //    foreach (AgencyUser _agencyUser in tmpLstAgencyUsers)
        //    {
        //        AgencyUserContract _aguContract = new AgencyUserContract();
        //        _aguContract.AGU_ID = _agencyUser.AGU_ID;
        //        _aguContract.AGU_Name = _agencyUser.AGU_Name;
        //        _aguContract.AGU_Phone = _agencyUser.AGU_Phone;
        //        _aguContract.AGU_Email = _agencyUser.AGU_Email;
        //        _aguContract.LstAGU_AgencyID = _agencyUser.UserAgencyMappings.Where(cond => !cond.UAM_IsDeleted).Select(col => col.UAM_AgencyID).ToList();
        //        _aguContract.LstNotVerifiedAGU_AgencyID = new List<int>();
        //        if (_agencyUser.UserAgencyMappings.Any(cond => !cond.UAM_IsDeleted && !cond.UAM_IsVerified))
        //        {
        //            _aguContract.LstNotVerifiedAGU_AgencyID = _agencyUser.UserAgencyMappings.Where(cond => !cond.UAM_IsDeleted && !cond.UAM_IsVerified)
        //                .Select(col => col.UAM_AgencyID).ToList();
        //        }
        //        _agencyUser.UserAgencyMappings.Where(cond => !cond.UAM_IsDeleted && cond.UAM_IsVerified).Select(col => col.UAM_AgencyID).ToList();
        //        _aguContract.AGU_UserID = _agencyUser.AGU_UserID.HasValue ? Convert.ToString(_agencyUser.AGU_UserID) : null;
        //        _aguContract.AGU_ComplianceSharedInfoTypeID = _agencyUser.AGU_ComplianceSharedInfoTypeID;
        //        _aguContract.AGU_ReqRotationSharedInfoTypeID = _agencyUser.AGU_ReqRotationSharedInfoTypeID;
        //        _aguContract.AGU_BkgSharedInfoTypeID = _agencyUser.AGU_BkgSharedInfoTypeID;
        //        _aguContract.AGU_ComplianceSharedInfoTypeName = _agencyUser.lkpInvitationSharedInfoType2.IsNull() ? String.Empty : _agencyUser.lkpInvitationSharedInfoType2.SharedInfoType;
        //        _aguContract.AGU_ReqRotationSharedInfoTypeName = _agencyUser.lkpInvitationSharedInfoType.IsNull() ? String.Empty : _agencyUser.lkpInvitationSharedInfoType.SharedInfoType;
        //        //UAT-1213: Updates to Agency User background check permissions.
        //        _aguContract.AGU_BkgSharedInfoTypeName = _agencyUser.lkpInvitationSharedInfoType1.IsNull() ? String.Empty : _agencyUser.lkpInvitationSharedInfoType1.SharedInfoType;

        //        List<AgencyInstitution> lstAgencyInstitutionForUser = GetAgencyUserInstitutesForAgencyUserID(_agencyUser.AGU_ID);

        //        _aguContract.lstSelectedAGI_ID = lstAgencyInstitutionForUser.Select(col => col.AGI_ID).ToList();
        //        _aguContract.DicSelectedAgencyInstitutionMapping = new Dictionary<int, List<int>>();
        //        List<AgencyInstitution> lstAgencyInstitutionForUserPerAgency = lstAgencyInstitutionForUser.DistinctBy(col => col.AGI_AgencyID).ToList();
        //        foreach (AgencyInstitution AGI in lstAgencyInstitutionForUserPerAgency)
        //        {
        //            if (!_aguContract.DicSelectedAgencyInstitutionMapping.ContainsKey(AGI.AGI_AgencyID.Value))
        //            {
        //                List<Int32> agencyInstituteIDs = lstAgencyInstitutionForUser
        //                                                .Where(cond => cond.AGI_AgencyID == AGI.AGI_AgencyID).Select(col => col.AGI_ID).ToList();
        //                _aguContract.DicSelectedAgencyInstitutionMapping.Add(AGI.AGI_AgencyID.Value, agencyInstituteIDs);
        //            }
        //        }

        //        _aguContract.lstApplicationInvitationMetaDataID = GetAgencyUserSharedDataForAgencyID(_agencyUser.AGU_ID);
        //        //UAT-1346 - Add permission fields to manage agency users and ROtation packages
        //        _aguContract.AGU_AgencyUserPermission = _agencyUser.AGU_AgencyUserPermission;
        //        _aguContract.AGU_RotationPackagePermission = _agencyUser.AGU_RotationPackagePermission;

        //        //UAT-1213: Updates to Agency User background check permissions.
        //        var lstInvitationSharedInfoType = GetInvitationSharedInfoTypeByAgencyUserID(_agencyUser.AGU_ID);
        //        if (lstInvitationSharedInfoType.IsNotNull() && lstInvitationSharedInfoType.Any())
        //        {
        //            _aguContract.lstInvitationSharedInfoTypeID = lstInvitationSharedInfoType.Select(cond => cond.ISIM_InvitationSharedInfoTypeID).ToList();
        //            _aguContract.SharedInfoTypeName = String.Join(",", lstInvitationSharedInfoType.Select(x => x.lkpInvitationSharedInfoType.SharedInfoType).ToList());
        //        }
        //        _aguContract.AGU_TenantName = GetAgencyInstitution(lstAgencyInstitutionForUser);



        //        if (View.IsAgencyUserLoggedIn)
        //        {
        //            List<Int32> loggedInAgencyIds = View.LstAgency.Select(col => col.AG_ID).ToList();
        //            _aguContract.AgencyName = String.Join(",", _agencyUser.UserAgencyMappings
        //                                                        .Where(col => !col.UAM_IsDeleted && loggedInAgencyIds.Contains(col.Agency.AG_ID))
        //                                                        .Select(col => col.Agency.AG_Name).ToList());
        //        }
        //        else
        //        {
        //            //UAT 1458 On the result grid of manage agency users, we should reflect the Agency that the user is associated with.
        //            _aguContract.AgencyName = String.Join(",", _agencyUser.UserAgencyMappings.Where(col => !col.UAM_IsDeleted).Select(col => col.Agency.AG_Name).ToList());
        //        }

        //        //UAT 1529 WB: Need to have active/inactive and locked/unlocked statuses for agency users on the manage agency users screen
        //        _aguContract.IsActive = lstUserPermissionStatus.Where(x => x.UserID == Convert.ToString(_agencyUser.AGU_UserID)).Select(col => col.IsActive).FirstOrDefault();
        //        _aguContract.IsLocked = lstUserPermissionStatus.Where(x => x.UserID == Convert.ToString(_agencyUser.AGU_UserID)).Select(col => col.IsLocked).FirstOrDefault();

        //        //UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        //        GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue());
        //        GetAgencyUserPermissionAccessTypeID(AgencyUserPermissionAccessType.YES.GetStringValue());
        //        if (_agencyUser.AgencyUserPermissions.IsNullOrEmpty())
        //        {
        //            _aguContract.AttestationRptPermission = false;
        //        }
        //        else
        //        {
        //            _aguContract.AttestationRptPermission = (_agencyUser.AgencyUserPermissions.Where(x => !x.AUP_IsDeleted && x.AUP_PermissionTypeID == View.AgencyUserPermissionTypeId).FirstOrDefault().AUP_PermissionAccessTypeID == View.AgencyUserPermissionAccessTypeId); // If permission == YES then true else false
        //        }
        //        _tmplst.Add(_aguContract);
        //    }
        //    View.LstAgencyUsers = _tmplst;
        //}

        private List<int> GetAgencyUserSharedDataForAgencyID(int agencyUserID)
        {
            return ProfileSharingManager.GetAgencyUserSharedDataForAgencyUserID(View.TenantId, agencyUserID);
        }

        //UAT-1213: Updates to Agency User background check permissions.
        private List<InvitationSharedInfoMapping> GetInvitationSharedInfoTypeByAgencyUserID(Int32 agencyUserID)
        {
            return ProfileSharingManager.GetInvitationSharedInfoTypeByAgencyUserID(View.TenantId, agencyUserID);
        }

        private List<AgencyInstitution> GetAgencyUserInstitutesForAgencyUserID(int agencyUserID)
        {
            return ProfileSharingManager.GetAgencyUserInstitutesForAgencyUserID(View.TenantId, agencyUserID);
        }

        private String GetAgencyInstitution(List<AgencyInstitution> tmplstInstitutions)
        {
            //Admin
            if (IsDefaultTenant)
            {
                List<Int32?> lstTenantIDs = tmplstInstitutions.Select(x => x.AGI_TenantID).ToList();
                return GetInstitutionName(lstTenantIDs);
            }
            else //Client Admin
            {
                return GetCurrentTenantName();
            }

        }

        private static string GetInstitutionName(List<Int32?> lstTenantIDs)
        {
            var lstTenant = ComplianceDataManager.getClientTenant();
            return String.Join(",", lstTenant.Where(x => lstTenantIDs.Contains(x.TenantID)).Select(col => col.TenantName));
        }

        private String GetCurrentTenantName()
        {
            return ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == View.TenantId).FirstOrDefault().TenantName;
        }

        /// <summary>
        ///  UAT 1529 WB: Need to have active/inactive and locked/unlocked statuses for agency users on the manage agency users screen
        /// </summary>
        public void UpdateUser(String userId, Boolean isActive, Boolean isLocked)
        {
            List<OrganizationUser> lstOrganizationUser = SecurityManager.GetOrganizationUserInfoByUserId(userId).ToList();
            if (!lstOrganizationUser.IsNullOrEmpty())
            {
                foreach (OrganizationUser organizationUser in lstOrganizationUser)
                {
                    //Set Is Active
                    organizationUser.IsActive = isActive;
                    if (isActive && organizationUser.ActiveDate == null && organizationUser.IsApplicant == true)
                    {
                        organizationUser.ActiveDate = DateTime.Now;
                    }
                    organizationUser.ModifiedByID = View.CurrentLoggedInUserId;
                    organizationUser.ModifiedOn = DateTime.Now;

                    //Set Is Locked
                    if (organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut != isLocked)
                    {
                        organizationUser.aspnet_Users.aspnet_Membership.IsLockedOut = isLocked;
                    }
                    SecurityManager.UpdateOrganizationUser(organizationUser);
                }
            }
        }




        #endregion

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

        public List<AgencyInstitutionsContract> GetInstitutesForSelectedAgencies(List<int> list)
        {
            throw new NotImplementedException();
        }

        //UAT-2452
        public void CopySharedInvForNewlyMappedAgencyForAgencyUser(Dictionary<String, Object> agencyUserData)
        {
            AgencyUserContract agencyUser = agencyUserData.GetValue("agencyUser") as AgencyUserContract;
            Int32 currentLoggedInUserId = Convert.ToInt32(agencyUserData.GetValue("CurrentLoggedInUserId"));
            Int32 agencyUserID;
            if (agencyUser.AGU_ID > AppConsts.NONE)
            {
                agencyUserID = agencyUser.AGU_ID;
            }
            else
            {
                agencyUserID = ProfileSharingManager.GetAgencyUserByEmail(agencyUser.AGU_Email);
            }
            //Commented for UAT-2641
            //List<Int32> lstAgencyId = new List<Int32>();
            //List<Int32> lstAgencyInstitutionIds = new List<Int32>();

            //foreach (var item in agencyUser.DicSelectedAgencyInstitutionMapping.Keys)
            //{
            //    lstAgencyId.Add(item);
            //}
            //foreach (var item in agencyUser.DicSelectedAgencyInstitutionMapping.Values.ToList())
            //{
            //    item.ForEach(x =>
            //    {
            //        lstAgencyInstitutionIds.Add(x);
            //    });
            //}
            //List<AgencyInstitution> lstAgencyInstitution = ProfileSharingManager.GetAgencyInstitutionForAgencies(AppConsts.NONE, lstAgencyId);

            List<AgencyInstitution> lstAgencyInstitution = ProfileSharingManager.GetAgencyInstitutionForAgencyuser(agencyUserID);

            foreach (var agencyInstitutionDetails in lstAgencyInstitution.GroupBy(cond => cond.AGI_TenantID).ToList())
            {
                String agencyIds = String.Join(",", agencyInstitutionDetails.Select(sel => sel.AGI_AgencyID).Distinct().ToList());
                Int32 tenantID = agencyInstitutionDetails.FirstOrDefault().AGI_TenantID.Value;
                if (!agencyIds.IsNullOrEmpty())
                {
                    ProfileSharingManager.CopySharedInvForNewlyMappedAgencyForAgencyUser(agencyUserID, agencyIds, tenantID);
                    ProfileSharingManager.UpdateDocMappingForInvAttestation(agencyUserID, null, currentLoggedInUserId);
                }
            }
        }

        #region Code commented for UAT-2541
        //UAT 2367
        //public bool GetAgencyVerificationCodeAndSendMail(int agencyUserID)
        //{
        //    bool returnStatus = false;
        //    List<Guid> lstVerificationCode = ProfileSharingManager.GetAgencyVerificationCode(agencyUserID);
        //    AgencyUser agencyUser = ProfileSharingManager.GetAgencyUserByID(agencyUserID);
        //    if (agencyUser != null)
        //    {
        //        if (lstVerificationCode.Count > 0)
        //        {
        //            if (!String.IsNullOrEmpty(lstVerificationCode[0].ToString()))
        //            {
        //                returnStatus = ProfileSharingManager.SendUserAgencyMappingMail(agencyUser, View.CurrentLoggedInUserId, lstVerificationCode[0].ToString());
        //            }
        //            else { returnStatus = false; }
        //        }
        //        else { returnStatus = false; }
        //    }
        //    return returnStatus;
        //} 
        #endregion

        //Code commented for UAT-2803
        //#region UAT-2538

        //public Boolean AssignAgencyUsersToSendEmail(Boolean IsNeedToSendEmail)
        //{
        //    return ProfileSharingManager.AssignunAssignAgencyUsersToSendEmail(View.lstSelectedAgencyUserIDs, IsNeedToSendEmail, View.CurrentLoggedInUserId);
        //}
        //#endregion

        #region UAT-2641
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
        #endregion


        //UAT-2663
        public Boolean AddSharedUser(AgencyUserContract AgencyUser, Int32 agencyUserID)
        {
            aspnet_Applications application = SecurityManager.GetApplication();
            aspnet_Users aspnetUsers = new aspnet_Users();
            aspnet_Membership memberShip = new aspnet_Membership();
            OrganizationUser organizationUser = new OrganizationUser();
            Organization org = new Organization();
            ProfileSharingInvitation invitation = new ProfileSharingInvitation();

            //Creating aspnet_users
            aspnetUsers.LastActivityDate = DateTime.MaxValue;
            aspnetUsers.UserName = AgencyUser.UserName;
            aspnetUsers.LoweredUserName = aspnetUsers.UserName.ToLower();
            aspnetUsers.ApplicationId = application.ApplicationId;
            aspnetUsers.UserId = Guid.NewGuid();

            //Creating aspnet_membership
            memberShip.PasswordSalt = SysXMembershipUtil.GenerateSalt();
            memberShip.Password = SysXMembershipUtil.HashPasswordIWithSalt(AgencyUser.Password, memberShip.PasswordSalt);
            memberShip.Email = AgencyUser.AGU_Email;
            memberShip.LoweredEmail = memberShip.Email.ToLower();
            memberShip.aspnet_Applications = application;
            memberShip.IsApproved = true;
            memberShip.PasswordFormat = AppConsts.ONE;
            memberShip.CreateDate = DateTime.Now;
            memberShip.LastLockoutDate = DateTime.Now.AddDays(-1);
            memberShip.LastLoginDate = DateTime.Now.AddDays(-1);
            memberShip.LastPasswordChangedDate = DateTime.Now.AddDays(-1);
            memberShip.FailedPasswordAttemptWindowStart = DateTime.Now.AddDays(-1);
            memberShip.FailedPasswordAnswerAttemptWindowStart = DateTime.Now.AddDays(-1);
            memberShip.IsLockedOut = false;
            aspnetUsers.aspnet_Membership = memberShip;

            //Creating OrganizationUser
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(AppConsts.SHARED_USER_TENANT_ID);
            organizationUser.aspnet_Users = aspnetUsers;
            organizationUser.FirstName = AgencyUser.FirstName;
            organizationUser.MiddleName = String.Empty;
            organizationUser.LastName = AgencyUser.LastName;
            organizationUser.PrimaryEmailAddress = AgencyUser.AGU_Email;
            organizationUser.SecondaryEmailAddress = null;
            organizationUser.IsNewPassword = false;
            organizationUser.CreatedOn = DateTime.Now;
            organizationUser.ModifiedOn = null;
            organizationUser.IsDeleted = false;
            organizationUser.IsActive = true;
            organizationUser.IsApplicant = false;
            organizationUser.IsOutOfOffice = false;
            organizationUser.IgnoreIPRestriction = true;
            organizationUser.IsMessagingUser = true;
            organizationUser.IsSystem = false;
            organizationUser.PhotoName = null;
            organizationUser.OriginalPhotoName = null;
            organizationUser.DOB = null;
            organizationUser.SSN = null;
            organizationUser.Gender = null;
            organizationUser.PhoneNumber = null;
            organizationUser.SecondaryPhone = null;
            organizationUser.UserVerificationCode = null;
            organizationUser.IsSharedUser = true;
            organizationUser.PhoneNumber = AgencyUser.AGU_Phone;
            organizationUser.IsInternationalPhoneNumber = AgencyUser.IsInternationalPhone;

            OrganizationUser orgUserObj = SecurityManager.AddOrganizationUser(organizationUser, aspnetUsers);
            if (orgUserObj.IsNotNull())
            {
                // Updating Invitee UserID based on userType recieved
                UpdateInviteeUserID(orgUserObj, agencyUserID);

                // Created Mapping of shared user in OrganizationUserTypeMapping
                SecurityManager.AddOrganizationUserTypeMapping(orgUserObj.OrganizationUserID, OrganizationUserType.AgencyUser.GetStringValue());

                //Assign default roles to the agency shared user
                ApplicationDataManager.AssignDefaultRolesToAgencyUser(orgUserObj);
            }
            return true;
        }

        /// <summary>
        /// Method to check whether entered username already exists
        /// </summary>
        /// <returns></returns>
        public Boolean IsExistsUserName(String UserName)
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(UserName);
            return user != null;
        }

        public Boolean IsExistsUserEmail(String UserEmail)
        {
            var user = System.Web.Security.Membership.GetUserNameByEmail(UserEmail);
            if (!user.IsNullOrEmpty())
                return true;
            else
                return false;
        }
        /// <summary>
        /// Method to update InviteeUserID based on UserType received from querystring
        /// </summary>
        /// <param name="orgUserObj"></param>
        private void UpdateInviteeUserID(OrganizationUser orgUserObj, Int32 agencyUserID)
        {
            String profileSharingInvitationIds;//UAT-3400
            ProfileSharingManager.UpdateInviteeOrganizationUserID(orgUserObj.OrganizationUserID, Guid.Empty, agencyUserID, out profileSharingInvitationIds);
        }

        #region UAT-2640
        public void GetAgencyHierarchyForClientAdmin()
        {
            View.lstAgencyHierarchy = ProfileSharingManager.GetAgencyHierarchyOfCurrentTenantToAddUser(View.TenantId);
        }
        #endregion

        public Boolean IsCurrentLoggedInUser(Int32 selectedAgencyUserID)
        {
            return ProfileSharingManager.IsCurrentLoggedInUser(View.CurrentLoggedInOrgUserID, selectedAgencyUserID);
        }

        public bool IsAgencyUserOnDifferentNode(Int32 selectedAgencyUserID)
        {
            return ProfileSharingManager.IsAgencyUserOnDifferentNode(View.CurrentLoggedInUserId, selectedAgencyUserID);
        }


        //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        public void GetAgencyUserNotifications()
        {
            View.lstAgencyUserNotification = ProfileSharingManager.GetAgencyUserNotifications();
        }
        public Boolean UpdateAgencyUserEmailAddress(Int32 agencyUserId, String emailId)
        {
            return ProfileSharingManager.UpdateAgencyUserEmailAddress(agencyUserId, emailId, View.CurrentLoggedInUserId);
        }

        public void SendNotificationToAgencyUser(AgencyUserContract agencyUser)
        {
            OrganizationUser orgUser = SecurityManager.GetOrganizationUserInfoByUserId(agencyUser.AGU_UserID).FirstOrDefault(x => x.IsSharedUser == true);
            Int16 authTypeId = SecurityManager.GetAuthRequestTypeIdByCode(AuthRequestType.Email_Updation_For_SharedUser.GetStringValue());
            UserAuthRequest tempUserAuthRequest = SecurityManager.GenerateEmailConfirmationReq(orgUser.OrganizationUserID, null, agencyUser.AGU_Email, true, agencyUser.AGU_ID, authTypeId);
            if (tempUserAuthRequest.IsNotNull())
            {
                String profileSharingURL = System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty() ? String.Empty : Convert.ToString(System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);
                var applicationUrl = "http://" + String.Format(profileSharingURL + "/Login.aspx?AuthReqVerCode=" + tempUserAuthRequest.UAR_VerificationCode);
                Boolean IsSuccess = ProfileSharingManager.SendNotificationToAgencyUserChangeInEmailAddress(agencyUser, View.CurrentLoggedInUserId, agencyUser.AGU_ID, applicationUrl);

                if (IsSuccess)
                {
                    String successMessage = String.Format(AppConsts.AGU_SEND_EMAIl_UPDATE_VERIFICATION_LINK_MSG, agencyUser.AGU_Email);
                    View.SuccessMessage = successMessage;
                }
            }
        }

        #region UAT 3294
        public void GetAgencyUserByAgencIds(List<Int32> agencyHiearchyIDs)
        {
            View.LstAgencyUserByAgency = ProfileSharingManager.GetAgencyUserByAgencIds(agencyHiearchyIDs);
        }
        public Boolean IsApplicantSendInvitationToAgencyUser(String emailId)
        {
            if (!View.SelectedAgencyUser.AGU_UserID.IsNullOrEmpty())
            {
                Guid agencyUserId = new Guid(View.SelectedAgencyUser.AGU_UserID);
                return ProfileSharingManager.IsApplicantSendInvitationToAgencyUser(agencyUserId);
            }
            else
            {
                return false;
            }
        }
        public Boolean MoveApplicantEmailShareToAgencyUser(String emailId, Guid toAgencyUserID)
        {
            if (!View.SelectedAgencyUser.AGU_UserID.IsNullOrEmpty())
            {
                Guid fromAgencyUserId = new Guid(View.SelectedAgencyUser.AGU_UserID);
                return ProfileSharingManager.MoveApplicantEmailShareToAgencyUser(fromAgencyUserId, View.TenantId, toAgencyUserID, View.CurrentLoggedInUserId);
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region UAT-3360
        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        //public Boolean IsAgencyUserProfileExisted()
        //{
        //    return true;
        //}

        public Boolean AddSharedUserWithLinkedProfile(AgencyUserContract AgencyUser, Int32 agencyUserID)
        {
              OrganizationUser orgUserObj = new OrganizationUser();
              if (View.ExistingOrganisationUser.IsSharedUser.HasValue && View.ExistingOrganisationUser.IsSharedUser.Value)
                  orgUserObj = View.ExistingOrganisationUser;
              else
              {

                  OrganizationUser organizationUser = new OrganizationUser();
                  organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(AppConsts.SHARED_USER_TENANT_ID);
                  organizationUser.UserID = View.ExistingOrganisationUser.UserID;
                  organizationUser.FirstName = AgencyUser.FirstName;
                  // organizationUser.MiddleName = AgencyUser.MiddleName;
                  organizationUser.LastName = AgencyUser.LastName;
                  organizationUser.PrimaryEmailAddress = AgencyUser.AGU_Email; //View.ExistingOrganisationUser.aspnet_Users.aspnet_Membership.Email;
                  organizationUser.IsNewPassword = false;
                  organizationUser.CreatedOn = DateTime.Now;
                  organizationUser.ModifiedOn = null;
                  organizationUser.IsDeleted = false;
                  organizationUser.IsActive = true;
                  organizationUser.IsApplicant = false;
                  organizationUser.IsOutOfOffice = false;
                  organizationUser.IgnoreIPRestriction = true;
                  organizationUser.IsMessagingUser = true;
                  organizationUser.IsSystem = false;
                  organizationUser.IsSharedUser = true;
                  //OrganizationUser orgUserObj = new OrganizationUser();
                  //if (View.ExistingOrganisationUser.IsSharedUser.HasValue && View.ExistingOrganisationUser.IsSharedUser.Value)
                  //    orgUserObj = View.ExistingOrganisationUser;
                  //else
                  orgUserObj = SecurityManager.AddOrganizationUser(organizationUser);
              }
            if (orgUserObj.IsNotNull())
            {
                // Updating Invitee UserID based on userType recieved

                //Commented this  related to fix Issue#16: ADB Agency user: Error is displayed on agency user screen when applicant is linked with agency user. (UAT- || Bug ID: 21732 ||P: P1|| S: Major)
                //UpdateInviteeUserID(View.ExistingOrganisationUser, agencyUserID);
                UpdateInviteeUserID(orgUserObj, agencyUserID);

                // Created Mapping of shared user in OrganizationUserTypeMapping
                SecurityManager.AddOrganizationUserTypeMapping(orgUserObj.OrganizationUserID, OrganizationUserType.AgencyUser.GetStringValue());

                //Assign default roles to the agency shared user
                ApplicationDataManager.AssignDefaultRolesToAgencyUser(orgUserObj);
            }
            return true;
        }

        public Boolean CheckAccountLinkingExistingProfile(String username, String userEmail)
        {
            var userListsWithUsername = SecurityManager.GetOrgUserListIfUsernameExistInSecuritytDBForAccountLinking(username);
            var userListsWithUserEmail = SecurityManager.GetOrgUserListIfUsernameExistInSecuritytDBForAccountLinking(userEmail);
            //   var userListOtherThanAgencyUser = userLists.Where(obj => (obj.IsSharedUser ?? false) == false).ToList();
            var finalUserList = new List<OrganizationUser>();
            finalUserList.AddRange(userListsWithUsername);
            finalUserList.AddRange(userListsWithUserEmail);
            if (finalUserList.Count > AppConsts.NONE)
            {
                if (finalUserList.Where(cond => (cond.IsSharedUser ?? false) == true).Any())
                {
                    List<String> agencyUserEmailList = finalUserList.Select(d => d.PrimaryEmailAddress).Distinct().ToList();
                    var IsAgencyUserExist = ProfileSharingManager.IsAgencyUserExist(agencyUserEmailList);
                    if (IsAgencyUserExist)
                        return false;
                    else
                    {
                        if (finalUserList.Where(obj => (obj.IsSharedUser ?? false) == false && (obj.IsApplicant ?? false) == false && obj.OrganizationID == SecurityManager.DefaultTenantID).Any())
                            return false;
                        else
                            return true;
                    }
                }
                if (finalUserList.Where(obj => (obj.IsSharedUser ?? false) == false && (obj.IsApplicant ?? false) == false && obj.OrganizationID == SecurityManager.DefaultTenantID).Any())
                    return false;
                if (finalUserList.Where(obj => (obj.IsSharedUser ?? false) == false && obj.aspnet_Users.UserName.ToLower() == username.ToLower()).Any())
                    return true;
                if (finalUserList.Where(obj => (obj.IsSharedUser ?? false) == false && obj.aspnet_Users.aspnet_Membership.Email.ToLower() == userEmail.ToLower()).Any())
                    return true;
            }
            return false;
        }
        #endregion

        #region UAT-3719
        public void SaveAgencyUserPermissionAuditDetails(Dictionary<String, Object> dic)
        {
            Int32 AgencyUserID = Convert.ToInt32(dic.GetValue("AgencyUserID"));
            Int32 CurrentLoggedInUserId = Convert.ToInt32(dic.GetValue("CurrentLoggedInUserId"));
            ProfileSharingManager.SaveAgencyUserPermissionAuditDetails(AgencyUserID, null, CurrentLoggedInUserId);
            ProfileSharingManager.UpdateDocMappingForInvAttestation(AgencyUserID, null, CurrentLoggedInUserId);
        }
        #endregion

        #region UAT-3664

        public void GetAgencyUserReports()
        {
            View.lstAgencyUserReports = ProfileSharingManager.GetAgencyUserReports();
        }

        public List<AgencyUserReportPermissionContract> GetAgencyUserReportsWithNoAccess()
        {
            View.lstAgencyUserReportPermission = ProfileSharingManager.GetAgencyUserReportPermissions(View.AGU_ID);
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

        #region UAT 4398
        public void GetAgencyUserListByAgencIds(List<Int32> agencyHiearchyIDs)
        {
            View.LstAgencyUserByAgency = ProfileSharingManager.GetAgencyUserByAgencIds(agencyHiearchyIDs);
        }
        #endregion
    }
}
