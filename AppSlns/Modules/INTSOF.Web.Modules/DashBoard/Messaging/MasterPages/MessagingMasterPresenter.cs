using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using INTSOF.Utils;
namespace CoreWeb.Messaging.MasterPages
{
    public class MessagingMasterPresenter : Presenter<IMessagingMasterView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMessagingController _controller;
        // public MessagingMasterPresenter([CreateNew] IMessagingController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }


        /// <summary>
        /// Invoked to get the folder as per userid
        /// </summary>
        public void GetFolders()
        {
            View.FolderList = MessageManager.GetFolders(View.CurrentUserID, View.UserGroupID);
        }

        public Int32 GetFolderCount(Int32 folderId, String folderCode, Int32 queueOwnerId)
        {
            return MessageManager.GetMessageCount(folderId, folderCode, queueOwnerId);

        }


        /// <summary>
        /// Invoked to set the Communication Types
        /// </summary>
        public void GetCommuncationTypes(Guid userId)
        {
            View.CommunicationTypeList = MessageManager.GetCommuncationTypes(userId);
        }

        public void GetgroupFolders()
        {
            List<aspnet_Roles> roleId = SecurityManager.GetUserRoles(View.UserName);
            View.GroupFolderList = MessageManager.GetGroupFolderList(roleId);
        }

        public Int32 GetGroupFolderCount(Int32 groupID, String foldeCode)
        {
            return MessageManager.GetGroupFolderCount(groupID, foldeCode);

        }

        public Boolean CheckApplicantClientSettings()
        {
            Int32 _tenantId = SecurityManager.GetOrganizationUser(View.CurrentUserID).Organization.TenantID.Value;
            Entity.ClientEntity.ClientSetting clientsettings = ComplianceDataManager.GetClientSetting(_tenantId, Setting.ALLOW_APPLICANT_TO_SEND_MESSAGE.GetStringValue());
            if (clientsettings.IsNotNull())
            {
                return (!String.IsNullOrEmpty(clientsettings.CS_SettingValue) && clientsettings.CS_SettingValue == AppConsts.STR_ONE) ? true : false;
            }
            else
            {
                return true;
            }
        }
    }
}




