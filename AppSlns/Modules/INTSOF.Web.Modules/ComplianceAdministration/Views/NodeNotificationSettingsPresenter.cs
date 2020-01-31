using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class NodeNotificationSettingsPresenter : Presenter<INodeNotificationSettingsView>
    {
        /// <summary>
        /// To get all user groups
        /// </summary>
        public void GetAllUserGroups()
        {
            View.lstUserGroups = ComplianceSetupManager.GetAllUserGroup(View.SelectedTenantID).OrderBy(ex => ex.UG_Name).ToList();
        }

        public List<Int32> GetCheckedUsergroupIds(Int32 mappingId)
        {
            return ComplianceSetupManager.GetCheckedUsergroupIds(mappingId, View.SelectedTenantID);
        }

        /// <summary>
        /// To get node deadlines
        /// </summary>
        public void GetNodeDeadlines()
        {
            //View.lstNodeDeadlines = ComplianceSetupManager.GetNodeDeadlines(View.SelectedTenantID);
            List<NodeDeadline> _lstNodeDeadline = ComplianceSetupManager.GetNodeDeadlines(View.SelectedTenantID).ToList();
            List<NodeDeadline> _tempNodeDeadline = new List<NodeDeadline>();

            foreach (var deadLine in _lstNodeDeadline)
            {
                NodeNotificationMapping _nodeNotificationMapping = deadLine.NodeNotificationMappings.FirstOrDefault(x => x.NNM_HierarchyNodeID == View.HierarchyNodeID
                                                                    && x.NNM_IsDeleted == false);
                if (_nodeNotificationMapping.IsNotNull())
                {
                    deadLine.ND_Frequency = _nodeNotificationMapping.NNM_Frequency;
                    deadLine.ND_DaysBeforeDeadline = _nodeNotificationMapping.NNM_DaysBefore;
                    deadLine.ND_NodeNotificationMappingId = _nodeNotificationMapping.NNM_ID;
                    _tempNodeDeadline.Add(deadLine);
                }
            }
            View.lstNodeDeadlines = _tempNodeDeadline;
        }

        /// <summary>
        /// To save/insert NodeDeadline
        /// </summary>
        /// <param name="nodeDeadline"></param>
        public Boolean SaveNodeDeadline(NodeNotificationSettingsContract nodeNotificationSettingsContract, List<Int32> _lstUserGroupToInsert)
        {
            var createdOn = DateTime.Now;
            //Insert into NodeDeadline table
            NodeDeadline nodeDeadline = new NodeDeadline();
            nodeDeadline.ND_Name = nodeNotificationSettingsContract.NodeDeadlineName;
            nodeDeadline.ND_Description = nodeNotificationSettingsContract.NodeDeadlineDescription;
            nodeDeadline.ND_DeadlineDate = nodeNotificationSettingsContract.DeadlineDate;
            nodeDeadline.ND_IsDeleted = false;
            nodeDeadline.ND_CreatedBy = View.CurrentLoggedInUserId;
            nodeDeadline.ND_CreatedOn = createdOn;

            //Insert into NodeNotificationMapping table
            NodeNotificationMapping nodeNotificationMapping = new NodeNotificationMapping();
            nodeNotificationMapping.NNM_HierarchyNodeID = View.HierarchyNodeID;
            nodeNotificationMapping.NNM_NodeNotificationTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpNodeNotificationType>(View.SelectedTenantID)
                .FirstOrDefault(x => x.NNT_Code == lkpNodeNotificationTypesContext.DEADLINE.GetStringValue()).NNT_ID;
            nodeNotificationMapping.NNM_Frequency = nodeNotificationSettingsContract.Frequency;
            nodeNotificationMapping.NNM_DaysBefore = nodeNotificationSettingsContract.DaysBeforeDeadline;
            nodeNotificationMapping.NNM_IsDeleted = false;
            nodeNotificationMapping.NNM_CreatedBy = View.CurrentLoggedInUserId;
            nodeNotificationMapping.NNM_CreatedOn = createdOn;

            //Insert into NodeNotificationUserGroup table
            foreach (var item in _lstUserGroupToInsert)
            {
                NodeNotificationUserGroup newitem = new NodeNotificationUserGroup
                {
                    NNUG_UserGroupID = item,
                    NNUG_IsDeleted = false,
                    NNUG_CreatedBy = View.CurrentLoggedInUserId,
                    NNUG_CreatedOn = createdOn
                };
                nodeNotificationMapping.NodeNotificationUserGroups.Add(newitem);
            }
            nodeDeadline.NodeNotificationMappings.Add(nodeNotificationMapping);

            return ComplianceSetupManager.SaveNodeDeadline(View.SelectedTenantID, nodeDeadline);
        }

        /// <summary>
        /// To update NodeDeadline
        /// </summary>
        /// <param name="nodeDeadline"></param>
        public Boolean UpdateNodeDeadline(NodeNotificationSettingsContract nodeNotificationSettingsContract, Int32 nodeDeadlineId, List<Int32> lstUserGroupChecked)
        {
            return ComplianceSetupManager.UpdateNodeDeadline(View.SelectedTenantID, nodeDeadlineId, nodeNotificationSettingsContract,
                                                                lstUserGroupChecked, View.CurrentLoggedInUserId);
        }

        /// <summary>
        /// To delete Node Deadline
        /// </summary>
        /// <param name="nodeDeadlineId"></param>
        public Boolean DeleteNodeDeadline(Int32 nodeDeadlineId, Int32 nodeNotificationMappingId)
        {
            var modifiedOn = DateTime.Now;
            //Update NodeDeadline table
            NodeDeadline nodeDeadline = ComplianceSetupManager.GetNodeDeadlineByID(View.SelectedTenantID, nodeDeadlineId);
            nodeDeadline.ND_IsDeleted = true;
            nodeDeadline.ND_ModifiedBy = View.CurrentLoggedInUserId;
            nodeDeadline.ND_ModifiedOn = modifiedOn;

            //Update NodeNotificationMapping table
            var nodeNotificationMappings = nodeDeadline.NodeNotificationMappings.FirstOrDefault(x => !x.NNM_IsDeleted);
            nodeNotificationMappings.NNM_IsDeleted = true;
            nodeNotificationMappings.NNM_ModifiedBy = View.CurrentLoggedInUserId;
            nodeNotificationMappings.NNM_ModifiedOn = modifiedOn;

            //Update NodeNotificationUserGroup table
            var nodeNotificationUserGroups = nodeNotificationMappings.NodeNotificationUserGroups.Select(x => x).ToList();
            nodeNotificationUserGroups.ForEach(x =>
            {
                x.NNUG_IsDeleted = true;
                x.NNUG_ModifiedBy = View.CurrentLoggedInUserId;
                x.NNUG_ModifiedOn = modifiedOn;
            });

            return ComplianceSetupManager.DeleteNodeDeadline(View.SelectedTenantID, nodeNotificationMappingId, View.CurrentLoggedInUserId, modifiedOn);
        }

        /// <summary>
        /// To save Nag Email Notifications
        /// </summary>
        /// <returns></returns>
        public Boolean SaveNagEmailNotifications()
        {
            Int16 nagEmailNotificationTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpNodeNotificationType>(View.SelectedTenantID).FirstOrDefault(x => x.NNT_Code == lkpNodeNotificationTypesContext.NAGEMAILS.GetStringValue()).NNT_ID;
            return ComplianceSetupManager.SaveNagEmailNotifications(View.SelectedTenantID, nagEmailNotificationTypeId, View.HierarchyNodeID, View.NagFrequency, View.CurrentLoggedInUserId, View.IsActive);
        }

        /// <summary>
        /// To get Node Notification Mapping by NodeID 
        /// </summary>
        public void GetNodeNotificationMappingByNodeID()
        {
            Int16 nagEmailNotificationTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpNodeNotificationType>(View.SelectedTenantID).FirstOrDefault(x => x.NNT_Code == lkpNodeNotificationTypesContext.NAGEMAILS.GetStringValue()).NNT_ID;
            var nodeNotificationMapping = ComplianceSetupManager.GetNodeNotificationMappingByNodeID(View.SelectedTenantID, nagEmailNotificationTypeId, View.HierarchyNodeID);
            if (nodeNotificationMapping.IsNotNull())
            {
                View.SavedNagFrequency = View.NagFrequency = nodeNotificationMapping.NNM_Frequency;
                View.IsActive = nodeNotificationMapping.NNM_IsActive;
            }
        }

        public Int32 GetRootNodeNotificationMappingID()
        {
            Int32 institutionRootNodeID = ComplianceDataManager.GetInstitutionDPMID(View.SelectedTenantID);
            Int16 nagEmailNotificationTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpNodeNotificationType>(View.SelectedTenantID).FirstOrDefault(x => x.NNT_Code == lkpNodeNotificationTypesContext.NAGEMAILS.GetStringValue()).NNT_ID;
            var rootNodeNotificationMapping = ComplianceSetupManager.GetNodeNotificationMappingByNodeID(View.SelectedTenantID, nagEmailNotificationTypeId, institutionRootNodeID);
            if (rootNodeNotificationMapping.IsNotNull())
            {
                return rootNodeNotificationMapping.NNM_ID;
            }
            return 0;
        }

        #region External User BCC

        /// <summary>
        /// Get All Sub Event for client admin 
        /// </summary>
        /// <returns></returns>
        public void GetCommunicationSubEventsType()
        {
            //List of sub events that may exculded from the subevent drop down
            List<String> subEventCode = new List<String>();
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_PROFILE_CHANGE.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_INTERNAL_MESSAGES.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.REMINDER_SUBSCRIPTION_PENDING.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_NEW_ACCOUNT_CREATION.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_ACCOUNT_CREATION_ADMIN_USERS.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_ACCOUNT_CREATION_THIRDPARTY_USERS.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_VERIFICATION_CODE_USERNAME.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_FORGET_USERNAME_RESET.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_VERIFICATION_CODE_PASSWORD.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_FORGET_PASSWORD_RECOVER.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_PASSWORD_RESET_BY_ADMIN.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_ACCOUNT_CREATION_APPLICANT.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.ALERT_APPLICANT_EMAIL_ADDRESS_CHANGE.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_APPLICANT_EMAIL_ADDRESS_CHANGE.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_APPLICANT_INSTITUTION_CHANGE.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_VERIFICATION_CODE_APPLICANT_EMAIL_ADDRESS_CHANGE.GetStringValue());
            subEventCode.Add(CommunicationSubEvents.NOTIFICATION_ROLE_UPDATE.GetStringValue());

            List<Entity.lkpCommunicationSubEvent> communicationSubEvents = CommunicationManager.GetCommunicationSubEventsType(subEventCode);
            if (communicationSubEvents.IsNotNull())
            {
                View.lstSubEvent = communicationSubEvents;
            }
        }

        /// <summary>
        /// Get the External user record by TenantId and HierarchyNodeID
        /// </summary>
        public void GetExternalUserBCC()
        {
            List<Entity.ExternalCopyUser> externalCopyUser = CommunicationManager.GetBCCExternalUserData(View.SelectedTenantID, View.HierarchyNodeID);
            if (externalCopyUser.IsNotNull())
            {
                View.lstExternalUserBCC = externalCopyUser;
            }
        }

        /// <summary>
        /// Save the external user BCC settings 
        /// </summary>
        /// <param name="externalCopyUserContract">externalCopyUserContract</param>
        /// <returns></returns>
        public Boolean SaveRecordExternalUserBCC(ExternalCopyUserContract externalCopyUserContract)
        {
            Entity.ExternalCopyUser externalCopyUser = new Entity.ExternalCopyUser();
            externalCopyUser.ECU_FirstName = externalCopyUserContract.FirstName;
            externalCopyUser.ECU_LastName = externalCopyUserContract.LastName;
            externalCopyUser.ECU_CommunicationSubEventID = externalCopyUserContract.CommunicationSubEventID;
            externalCopyUser.ECU_EmailID = externalCopyUserContract.EmailAddress;
            externalCopyUser.ECU_TenantID = View.SelectedTenantID;
            externalCopyUser.ECU_HierarchyNodeID = View.HierarchyNodeID;
            externalCopyUser.ECU_IsDeleted = false;
            externalCopyUser.ECU_CreatedOn = DateTime.Now;
            externalCopyUser.ECU_CreatedByID = View.CurrentLoggedInUserId;
            return CommunicationManager.SaveRecordExternalUserBCC(externalCopyUser);
        }

        /// <summary>
        /// Update the external user BCC settings 
        /// </summary>
        /// <param name="externalCopyUserContract">externalCopyUserContract</param>
        /// <param name="externalCopyUserID">externalCopyUserID</param>
        /// <param name="CurrentLoggedInUserId">CurrentLoggedInUserId</param>
        /// <returns></returns>
        public Boolean UpdateRecordExternalUserBCC(ExternalCopyUserContract externalCopyUserContract, Int32 externalCopyUserID, Int32 CurrentLoggedInUserId)
        {
            return CommunicationManager.UpdateRecordExternalUserBCC(externalCopyUserContract, externalCopyUserID, CurrentLoggedInUserId);
        }

        /// <summary>
        /// Delete the external user BCC settings 
        /// </summary>
        /// <param name="externalCopyUserID">externalCopyUserID</param>
        /// <param name="CurrentLoggedInUserId">CurrentLoggedInUserId</param>
        /// <returns></returns>
        public Boolean DeleteExternalUserBCC(Int32 externalCopyUserID, Int32 CurrentLoggedInUserId)
        {
            return CommunicationManager.DeleteExternalUserBCC(externalCopyUserID, CurrentLoggedInUserId);
        }

        public Boolean IsExternalUserExistForSubEvent(ExternalCopyUserContract externalCopyUserContract, Int32? externalCopyUserId)
        {
            return CommunicationManager.IsExternalUserExistForSubEvent(externalCopyUserContract, View.HierarchyNodeID, View.SelectedTenantID, externalCopyUserId);
        }

        /// <summary>
        /// Get the Contact record from the InstitutionContact Table
        /// </summary>
        public void GetInstitutionContactUserData()
        {
            List<HierarchyContactMapping> hierarchyContactMapping = BackgroundSetupManager.GetInstitutionContactUserData(View.SelectedTenantID, View.HierarchyNodeID);
            if (hierarchyContactMapping.IsNotNull())
            {
                View.lstHierarchyContactMapping = hierarchyContactMapping;
            }
        }


        public void BindContactStatus()
        {
            View.contactStatus = BackgroundSetupManager.GetInstitutionContactList(View.SelectedTenantID, View.HierarchyNodeID, AppConsts.MINUS_ONE);
        }

        public void GetInstitutionContactListById(Int32 contactId)
        {
            View.institutionContactById = BackgroundSetupManager.GetInstitutionContactList(View.SelectedTenantID, contactId);
        }
        /// <summary>
        /// To save/insert NodeDeadline
        /// </summary>
        /// <param name="nodeDeadline"></param>
        public Boolean SaveContact(InstitutionContactContract institutionContact, Boolean isNew, Int32 contactId = 0)
        {

            InstitutionContact institutionContactDetail = new InstitutionContact();
            institutionContactDetail.ICO_FirstName = institutionContact.FirstName;

            institutionContactDetail.ICO_LastName = institutionContact.LastName;
            institutionContactDetail.ICO_Title = institutionContact.Title;
            institutionContactDetail.ICO_PrimaryPhone = institutionContact.PrimaryPhone;
            institutionContactDetail.ICO_PrimaryEmailAddress = institutionContact.EmailAddress;
            institutionContactDetail.ICO_Address1 = institutionContact.Address1;
            institutionContactDetail.ICO_Address2 = institutionContact.Address2;
            institutionContactDetail.ICO_ZipCodeID = institutionContact.ZipCodeID;
            institutionContactDetail.ICO_IsDeleted = false;
            institutionContactDetail.ICO_CreatedOn = DateTime.Now;
            institutionContactDetail.ICO_CreatedByID = View.CurrentLoggedInUserId;
            if (!ModuleUtility.ModuleUtils.SessionService.BusinessChannelType.IsNull() &&
              ModuleUtility.ModuleUtils.SessionService.BusinessChannelType.BusinessChannelTypeID == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
            {
                return BackgroundSetupManager.SaveContacts(institutionContactDetail, View.SelectedTenantID, View.HierarchyNodeID, isNew, contactId, AppConsts.AMS_BUSINESS_CHANNEL_TYPE);
            }
            else
            {
                return BackgroundSetupManager.SaveContacts(institutionContactDetail, View.SelectedTenantID, View.HierarchyNodeID, isNew, contactId);
            }
        }


        /// <summary>
        /// Update the external user BCC settings 
        /// </summary>
        /// <param name="externalCopyUserContract">externalCopyUserContract</param>
        /// <param name="externalCopyUserID">externalCopyUserID</param>
        /// <param name="CurrentLoggedInUserId">CurrentLoggedInUserId</param>
        /// <returns></returns>
        public Boolean UpdateContact(InstitutionContactContract institutionContactContract, Int32 contactID)
        {
            InstitutionContact institutionContactDetail = new InstitutionContact();
            institutionContactDetail.ICO_FirstName = institutionContactContract.FirstName;

            institutionContactDetail.ICO_LastName = institutionContactContract.LastName;
            institutionContactDetail.ICO_Title = institutionContactContract.Title;
            institutionContactDetail.ICO_PrimaryPhone = institutionContactContract.PrimaryPhone;
            institutionContactDetail.ICO_PrimaryEmailAddress = institutionContactContract.EmailAddress;
            institutionContactDetail.ICO_Address1 = institutionContactContract.Address1;
            institutionContactDetail.ICO_Address2 = institutionContactContract.Address2;
            institutionContactDetail.ICO_ZipCodeID = institutionContactContract.ZipCodeID;
            institutionContactDetail.ICO_IsDeleted = false;
            institutionContactDetail.ICO_CreatedOn = DateTime.Now;
            institutionContactDetail.ICO_CreatedByID = View.CurrentLoggedInUserId;

            return BackgroundSetupManager.UpdateContact(institutionContactDetail, contactID, View.SelectedTenantID, View.HierarchyNodeID);
        }



        /// <summary>
        /// Method is used to check the contact with supplied email exists or not
        /// </summary>
        /// <param name="contactEmailAddress"></param>
        /// <returns></returns>
        public Boolean IsContactExists(String contactEmailAddress,Int32 contactID=AppConsts.NONE)
        {
            return BackgroundSetupManager.IsContactExists(View.SelectedTenantID, contactEmailAddress,contactID);
        }


        /// <summary>
        /// To delete node
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteContact(Int32 contactId)
        {
            return BackgroundSetupManager.DeleteContact(contactId, View.SelectedTenantID, View.CurrentLoggedInUserId, View.HierarchyNodeID);

        }

        /// <summary>
        /// Get the Contact record from the InstitutionContact Table
        /// </summary>
        public void GetNotificationData(Int32 hierarchyContactMappingID)
        {
            List<Entity.HierarchyNotificationMapping> notification = CommunicationManager.GetHierarchyNotificationMappingData(View.SelectedTenantID, View.HierarchyNodeID, hierarchyContactMappingID);
            if (notification.IsNotNull())
            {
                View.lstNotifications = notification;
            }
        }
        public void BindComboBox()
        {
            View.copyType = CommunicationManager.GetlkpCopyType();

            if (!ModuleUtility.ModuleUtils.SessionService.BusinessChannelType.IsNull() &&
                ModuleUtility.ModuleUtils.SessionService.BusinessChannelType.BusinessChannelTypeID == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
            {

                View.subEvent = CommunicationManager.GetlkpCommunicationSubEvent(AppConsts.BUSINESS_CHANNEL_EVENT_AMS);
            }
            else
            {
                View.subEvent = CommunicationManager.GetlkpCommunicationSubEvent(AppConsts.BUSINESS_CHANNEL_EVENT_COMPLIO);
            }
        }

        public Boolean SaveCommunicationSubEvent(HierarchyNotificationMappingContract hierarchyNotificationMappingContract, Int32 mappingID)
        {
            Entity.HierarchyNotificationMapping hierarchyNotificationMapping = new Entity.HierarchyNotificationMapping();
            hierarchyNotificationMapping.HNM_CopyTypeID = hierarchyNotificationMappingContract.CopyTypeID;
            hierarchyNotificationMapping.HNM_SubEventID = hierarchyNotificationMappingContract.CommunicationSubEventID;
            //Check
            hierarchyNotificationMapping.HNM_TenantID = View.SelectedTenantID;
            hierarchyNotificationMapping.HNM_HierarchyNodeID = View.HierarchyNodeID;
            hierarchyNotificationMapping.HNM_HierarchyContactMappingID = mappingID;
            hierarchyNotificationMapping.HNM_IsCommunicationCenter = false;
            hierarchyNotificationMapping.HNM_IsEmail = true;
            hierarchyNotificationMapping.HNM_IsDeleted = false;
            hierarchyNotificationMapping.HNM_CreatedOn = DateTime.Now;
            hierarchyNotificationMapping.HNM_CreatedByID = View.CurrentLoggedInUserId;
            return CommunicationManager.SaveCommunicationSubEvent(hierarchyNotificationMapping);
        }


        public Boolean UpdateCommunicationSubEvent(HierarchyNotificationMappingContract hierarchyNotificationMappingContract, Int32 notificationMappingID)
        {
            return CommunicationManager.UpdateCommunicationSubEvent(View.SelectedTenantID, hierarchyNotificationMappingContract, notificationMappingID, View.CurrentLoggedInUserId);
        }

        public Boolean DeleteCommunicationSubEvent(Int32 notificationMappingID)
        {
            return CommunicationManager.DeleteCommunicationSubEvent(View.SelectedTenantID, notificationMappingID, View.CurrentLoggedInUserId);
        }

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }
    }
        #endregion
}
