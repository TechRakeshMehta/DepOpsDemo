using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;

namespace CoreWeb.Templates.Views
{
    public partial class CommunicationCopySettingsOverridePresenter : Presenter<ICommunicationCopySettingsOverrideView>
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// This method is invoked by the view every time it loads.
        /// </summary>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the first time the view loads.
        }

        /// <summary>
        /// This method is invoked by the view the first time it loads.
        /// </summary>

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.getClientTenant();
        }
        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }


        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;

            }
        }
        public int GetDefaultTenant()
        {
            return SecurityManager.DefaultTenantID;
        }

        public void GetCommunicationCopySettingsOverride()
        {
            View.LstCommunicationCopySettingsOverride = ComplianceSetupManager.GetCommunicationCopySettingsOverride(View.SelectedTenantId);

            if (View.CurrentUserId != SecurityManager.DefaultTenantID) // if not super admin then show only their own records
            {
                View.LstCommunicationCopySettingsOverride = View.LstCommunicationCopySettingsOverride.Where(cond => cond.OrganizationUserID == View.CurrentUserId).ToList();
            }
        }

        public void BindNodeCopySetting()
        {
            View.LstNodeCopySetting = ComplianceSetupManager.GetNodeCopySettings(View.SelectedTenantId);
        }

        /// <summary>
        /// this method is called in case of Super admin so that they can set preference of every admin/client admin
        /// </summary>
        /// <param name="tenantIDForUsersType"></param>

        public void GetOrganizationUserList(Int32 tenantIDForUsersType)
        {
            List<OrganizationUser> organizationUserList = SecurityManager.GetOganisationUsersByTanentId(tenantIDForUsersType).Where(cond => cond.IsActive == true
                                       && cond.IsDeleted == false).ToList();
            if (!organizationUserList.IsNullOrEmpty())
            {
                View.OrganizationUserList = organizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName,
                    OrganizationUserID = x.OrganizationUserID,
                    LastName = x.LastName,
                    PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
                    MiddleName = x.FirstName + " " + x.MiddleName + " " + x.LastName + " (" + x.aspnet_Users.aspnet_Membership.Email + ") "
                }).OrderBy(x => x.MiddleName).ToList();  //UAT- sort dropdowns by Name
            }
        }

        /// <summary>
        ///  This method is called in case of Client admin or ADB admin case so that they can only their own preference
        /// </summary>

        public void GetOganisationUsersByTanentIdOfLoggedInUser()
        {
            List<OrganizationUser> organizationUserList = SecurityManager.GetOganisationUsersByTanentIdOfLoggedInUser(View.TenantId, View.CurrentUserId);
            if (!organizationUserList.IsNullOrEmpty())
            {
                View.OrganizationUserList = organizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName,
                    OrganizationUserID = x.OrganizationUserID,
                    LastName = x.LastName,
                    PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
                    MiddleName = x.FirstName + " " + x.MiddleName + " " + x.LastName + " (" + x.aspnet_Users.aspnet_Membership.Email + ") "
                }).OrderBy(x => x.MiddleName).ToList();  //UAT- sort dropdowns by Name
            }
        }

        /// <summary>
        /// method is only called in Edit mode. It fetch only one Organization user corresponding to OrganizationUserId
        /// </summary>
        /// <param name="organizationUserID"></param>

        public void GetOganisationUsersByUserID(Int32 organizationUserID)
        {
            List<OrganizationUser> organizationUserList = SecurityManager.GetOganisationUsersByUserID(organizationUserID);
            if (!organizationUserList.IsNullOrEmpty())
            {
                View.OrganizationUserList = organizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName,
                    OrganizationUserID = x.OrganizationUserID,
                    LastName = x.LastName,
                    PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
                    MiddleName = x.FirstName + " " + x.MiddleName + " " + x.LastName + " (" + x.aspnet_Users.aspnet_Membership.Email + ") "
                }).OrderBy(x => x.MiddleName).ToList();  //UAT- sort dropdowns by Name
            }

        }

        public String GetDeptProgMappingLabel(Int32 hierarchyNodeId)
        {
            return ComplianceSetupManager.GetDeptProgMappingLabel(hierarchyNodeId, View.SelectedTenantId);
        }

        public Boolean SaveCommunicationNodeCopySetting()
        {
            if (ComplianceSetupManager.CheckIfCommunicationNodeSettingExistForSelectednode(View.ViewContract.HierarchyNodeID, View.ViewContract.OrganizationUserID, View.SelectedTenantId))
            {
                View.ErrorMessage = "Communication Node Setting already exists for this user at selected hierarchy.";
                return false;
            }
            else
            {
                Entity.ClientEntity.CommunicationNodeCopySetting communicationNodeCopySetting = new Entity.ClientEntity.CommunicationNodeCopySetting()
                {
                    CNCS_HierarchyID = View.ViewContract.HierarchyNodeID,
                    CNCS_CreatedBy = View.CurrentLoggedInUserId,
                    CNCS_CreatedOn = DateTime.Now,
                    CNCS_IsDeleted = false,
                    CNCS_NodeCopySettingID = View.ViewContract.NodeCopySettingID,
                    CNCS_OrganizationUserID = View.ViewContract.OrganizationUserID
                };
                return ComplianceSetupManager.SaveCommunicationNodeCopySetting(communicationNodeCopySetting, View.SelectedTenantId, View.lstCommunicationSettingsSubEventsContract);
            }
        }

        public Boolean UpdateCommunicationNodeCopySetting()
        {
            return ComplianceSetupManager.UpdateCommunicationNodeCopySetting(View.ViewContract.CommunicationNodeCopySettingID, View.ViewContract.NodeCopySettingID, View.CurrentLoggedInUserId, View.SelectedTenantId, View.lstCommunicationSettingsSubEventsContract);
        }

        public Boolean DeleteCommunicationNodeCopySetting()
        {
            return ComplianceSetupManager.DeleteCommunicationNodeCopySetting(View.ViewContract.CommunicationNodeCopySettingID, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }

        #endregion

        #endregion


        public void BindCommunicationTypes()
        {
            View.CommunicationTypes = CommunicationManager.GetCommunicationTypes().OrderBy(x => x.Name).ToList(); //UAT- sort dropdowns by Name;
        }
        public void BindEvents()
        {
            View.CommunicationEvents = CommunicationManager.GetCommunicationEvents(Convert.ToInt32(View.CommunicationTypeId)).OrderBy(x => x.Name).ToList(); //UAT- sort dropdowns by Name
        }
        public void BindSubEvents()
        {
            View.CommunicationSubEvents = CommunicationManager.GetCommunicationTypeSubEvents(Convert.ToInt32(View.CommunicationTypeId), Convert.ToInt32(View.EventId)).Where(cond =>
                                                                                             cond.Code != CommunicationSubEvents.EXTERNAL_EMAIL_NOTIFICATION.GetStringValue()
                                                                                             && cond.Code != CommunicationSubEvents.NOTIFICATION_FOR_FAILED_APPLICANT_DOCUMENT_MERGING.GetStringValue()//UAT-2628     
                                                                                             && cond.IsHierarchySpecific).OrderBy(x => x.Name).ToList(); //UAT- sort dropdowns by Name
        }
        //Int32 communicationCCuserID,Int32 currentUserID,Int32 subEventID
        public Boolean IsUserAlreadyMappedToSubEvent(Int32 communicationCCMasterID)
        {
            return CommunicationManager.IsUserAlreadyMappedToSubEvent(communicationCCMasterID, View.CurrentUserId, View.SubEventId);
        }

        //public void BindSubEvents()
        //{
        //List<CommunicationCCMaster> communicationCCMaster = new List<CommunicationCCMaster>();
        //if (View.SelectedTenantId.IsNotNull() && View.SelectedTenantId > AppConsts.NONE)
        //{
        //    //UAT 1043 - WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
        //    if (!IsDefaultTenant) //Client Admin
        //    {
        //        communicationCCMaster = CommunicationManager.GetTenantSpecificCommunicationCCMaster(Convert.ToInt32(View.SelectedTenantId), SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_CLIENTADMIN);
        //    }
        //    else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == false) //ADB user
        //    {
        //        communicationCCMaster = CommunicationManager.GetTenantSpecificCommunicationCCMaster(Convert.ToInt32(View.SelectedTenantId), SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_ADBUSER);
        //    }
        //    else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == true && View.SelectedUserType.Equals("Institute") || View.SelectedUserType.Equals("Master")) //ADB Super Admin
        //    {
        //        communicationCCMaster = CommunicationManager.GetTenantSpecificCommunicationCCMaster(Convert.ToInt32(View.SelectedTenantId), SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_SUPERADMIN);
        //    }
        //}

        //List<lkpCommunicationSubEvent> communicationSubEvent = CommunicationManager.GetCommunicationTypeSubEvents(Convert.ToInt32(View.CommunicationTypeId),
        //                                                       Convert.ToInt32(View.EventId)).Where(cond =>
        //                                                       cond.Code != CommunicationSubEvents.EXTERNAL_EMAIL_NOTIFICATION.GetStringValue()
        //                                                       && cond.Code != CommunicationSubEvents.NOTIFICATION_FOR_FAILED_APPLICANT_DOCUMENT_MERGING.GetStringValue()//UAT-2628
        //                                                       ).ToList();
        //var uniqueSubEventList = communicationSubEvent.Where(x => !communicationCCMaster.Any(p => p.CommunicationSubEventID == x.CommunicationSubEventID));
        //View.CommunicationSubEvents = uniqueSubEventList.OrderBy(x => x.Name).ToList(); //UAT- sort dropdowns by Name


        //    if (!View.CommunicationTypeId.IsNullOrEmpty() && View.CommunicationTypeId > AppConsts.NONE && !View.EventId.IsNullOrEmpty() && View.EventId > AppConsts.NONE)
        //        View.CommunicationSubEvents = CommunicationManager.GetCommunicationTypeSubEvents(View.CommunicationTypeId.Value, View.EventId.Value);


        //}

        public void GetCommunicationCopySubEventSetting(Int32 communicationCopySettingID)
        {
            View.lstCommunicationSettingsSubEventsContract = ComplianceDataManager.GetCommunicationCopySubEventSetting(communicationCopySettingID, View.SelectedTenantId);
        }
    }
}


