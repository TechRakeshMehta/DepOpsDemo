using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Entity;
namespace CoreWeb.Security.Views
{
    public class MapUserInstitutionPresenter : Presenter<IMapUserInstitutionView>
    {
        public override void OnViewLoaded()
        {
            if (!View.UserId.IsNull())
            {
                View.SelectedUser = SecurityManager.GetOrganizationUserInfoByUserId(View.UserId).FirstOrDefault();
            }
        }

        public override void OnViewInitialized()
        {

        }

        public void GetMappedTenants()
        {
            View.MappedTenantList = GetClientAdminTenants();
        }

        private List<Tenant> GetClientAdminTenants()
        {
            String clientCode = TenantType.Institution.GetStringValue();
            return SecurityManager.GetClientAdminTenants(View.UserId).Where(x => x.lkpTenantType.TenantTypeCode == clientCode).ToList();
        }

        public void GetUnmappedTenants()
        {
            String tenantTypeCodeForClient = TenantType.Institution.GetStringValue();
            List<Int32> lstMappedTenantId = GetClientAdminTenants().Select(col => col.TenantID).ToList();
            View.UnmappedTenantList = SecurityManager.GetTenants(true, false, tenantTypeCodeForClient).Where(cond => !lstMappedTenantId.Contains(cond.TenantID)).ToList();
            View.UnmappedTenantList.Insert(0, new Tenant()
            {
                TenantID = -1,
                TenantName = "--SELECT--"
            });
        }

        public void SaveUserTenantMapping()
        {
            OrganizationUser existingUser = SecurityManager.GetOrganizationUserInfoByUserId(View.UserId).FirstOrDefault();
            if (!existingUser.IsNullOrEmpty())
            {
                OrganizationUser organizationUser = new OrganizationUser();
                organizationUser.Organization = SecurityManager.GetOrganizationForTenant(View.TenantId);
                organizationUser.aspnet_Users = existingUser.aspnet_Users;
                organizationUser.FirstName = existingUser.FirstName;
                organizationUser.LastName = existingUser.LastName;
                organizationUser.CreatedOn = DateTime.Now;
                organizationUser.CreatedByID = View.CurrentLoggedInUserId;
                organizationUser.IsNewPassword = false;
                organizationUser.IsDeleted = false;
                organizationUser.IsActive = existingUser.IsActive;
                //Added IsApplicant status    
                organizationUser.IsApplicant = false;
                organizationUser.IsOutOfOffice = existingUser.IsOutOfOffice;
                organizationUser.IgnoreIPRestriction = existingUser.IgnoreIPRestriction;
                organizationUser.IsMessagingUser = existingUser.IsMessagingUser;
                organizationUser.IsSystem = false;
                // UAT 891
                organizationUser.IsInternalMsgEnabled = true;
                //UAT-2447
                organizationUser.IsInternationalPhoneNumber = existingUser.IsInternationalPhoneNumber;
                
                SecurityManager.AddOrganizationUser(organizationUser, existingUser.aspnet_Users);
                SetDefaultSubscription(organizationUser.OrganizationUserID);
                View.ErrorMessage = String.Empty;
            }
            else
            {
                View.ErrorMessage = "Some error has occured.";
            }
        }

        #region Set Default Subscription

        /// <summary>
        /// 
        /// </summary>
        Int32 notificationCommunicationTypeId = 0;
        private Int32 NotificationCommunicationTypeId
        {
            get
            {
                if (notificationCommunicationTypeId == 0)
                    notificationCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.NOTIFICATION.GetStringValue()).CommunicationTypeID;
                return notificationCommunicationTypeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 alertCommunicationTypeId = 0;
        private Int32 AlertCommunicationTypeId
        {
            get
            {
                if (alertCommunicationTypeId == 0)
                    alertCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.ALERTS.GetStringValue()).CommunicationTypeID;
                return alertCommunicationTypeId;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 reminderCommunicationTypeId = 0;
        private Int32 ReminderCommunicationTypeId
        {
            get
            {
                if (reminderCommunicationTypeId == 0)
                    reminderCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.REMINDERS.GetStringValue()).CommunicationTypeID;
                return reminderCommunicationTypeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        private void SetDefaultSubscription(Int32 organizationUserId)
        {

            List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
            List<UserCommunicationSubscriptionSetting> mappedSubscriptionSettings = null;
            IEnumerable<lkpCommunicationEvent> communicationEvents = null;

            communicationEvents = CommunicationManager.GetCommunicationEvents(AlertCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentLoggedInUserId, AlertCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            communicationEvents = CommunicationManager.GetCommunicationEvents(NotificationCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentLoggedInUserId, NotificationCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            communicationEvents = CommunicationManager.GetCommunicationEvents(ReminderCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentLoggedInUserId, ReminderCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            if (userCommunicationSubscriptionSettings != null && userCommunicationSubscriptionSettings.Count > 0)
                CommunicationManager.AddUserCommunicationSubscriptionSettings(userCommunicationSubscriptionSettings);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="ById"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="communicationEvents"></param>
        /// <returns></returns>
        private List<UserCommunicationSubscriptionSetting> GetMappedUserCommunicationSubscriptionSettings(
            Int32 organizationUserId,
            Int32 ById,
            Int32 communicationTypeId,
            IEnumerable<lkpCommunicationEvent> communicationEvents)
        {
            List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings = null;
            if (communicationEvents != null && communicationEvents.Count() > 0)
            {
                userCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
                foreach (lkpCommunicationEvent communicationEvent in communicationEvents)
                {
                    userCommunicationSubscriptionSettings.Add(new UserCommunicationSubscriptionSetting()
                    {
                        OrganizationUserID = organizationUserId,
                        CommunicationTypeID = communicationTypeId,
                        CommunicationEventID = communicationEvent.CommunicationEventID,
                        IsSubscribedToAdmin = true,
                        IsSubscribedToUser = true,
                        CreatedByID = ById,
                        CreatedOn = DateTime.Now,
                        ModifiedByID = ById,
                        ModifiedOn = DateTime.Now
                    });
                }
            }
            return userCommunicationSubscriptionSettings;
        }
        #endregion

    }
}
