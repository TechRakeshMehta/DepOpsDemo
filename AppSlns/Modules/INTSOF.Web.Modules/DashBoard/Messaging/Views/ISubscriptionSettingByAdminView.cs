using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using INTSOF.Utils;
using System.Collections;
using System.Linq;

namespace CoreWeb.Messaging.Views
{
    public interface ISubscriptionSettingByAdminView
    {
        Int32 OrganizationUserId { get; }
        IEnumerable<lkpCommunicationEvent> NotificationCommunicationEvents { get; set; }
        IEnumerable<lkpCommunicationEvent> ReminderCommunicationEvents { get; set; }
        List<UserCommunicationSubscriptionSetting> SelectedUserCommunicationSubscriptionSettings { get; set; }
        List<UserCommunicationSubscriptionSetting> UnSelectedUserCommunicationSubscriptionSettings { get; set; }
        IQueryable<vw_ApplicantUser> ApplicantUsers { get; set; }
        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
        }

        List<String> FilterColumns
        {
            get;
            set;
        }

        List<String> FilterOperators
        {
            get;
            set;
        }

        List<String> FilterTypes
        {
            get;
            set;
        }

        ArrayList FilterValues
        {
            get;
            set;
        }
        #endregion
    }
}




