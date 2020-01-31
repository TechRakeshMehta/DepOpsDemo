using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreWeb.IntsofSecurityModel;
using Entity;
using INTSOF.UI.Contract.Templates;

namespace CoreWeb.Templates.Views
{
    public interface ICommunicationCCSettingsView
    {
        /// <summary>
        /// List of communication types
        /// </summary>
        List<lkpCommunicationType> CommunicationTypes
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// list of Events related to the Communication Type
        /// </summary>
        IEnumerable<lkpCommunicationEvent> CommunicationEvents
        {
            get;
            set;
        }

        /// <summary>
        /// List of the Sub-Events of the selected Communication type & Communication event
        /// </summary>
        List<lkpCommunicationSubEvent> CommunicationSubEvents
        {
            get;
            set;
        }

        /// <summary>
        /// USe this id to get the related events & save the values in the Database
        /// </summary>
        Int32? CommunicationTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Use this id to get the related sub events & save the values in the Database
        /// </summary>
        Int32? EventId
        {
            get;
            set;
        }

        /// <summary>
        /// Use this id to save the values in the Database
        /// </summary>
        Int32 SubEventId
        {
            get;
            set;
        }

        List<CommunicationCCMaster> CommunicationCCMasterList
        {
            get;
            set;
        }

        List<CommunicationCCUser> CommunicationCCUser
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
        }

        Int32 CurrentUserId
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        Int32 CommunicationCCMasterID
        {
            get;
            set;
        }

        Int32 CommunicationCCUsersID
        {
            get;
            set;
        }

        List<Entity.OrganizationUser> OrganizationUserList
        {
            get;
            set;
        }

        List<CommunicationCCUserContract> CommunicationCCUserContract
        {
            get;
            set;
        }

        List<CommunicationCCMaster> CommunicationCcExistingSubEvent
        {
            get;
            set;
        }
        List<lkpCopyType> lstCopyType
        {
            get;
            set;
        }
        String SelectedCopyTypeCode
        {
            get;
            set;
        }
        Boolean? IsCommunicationCentre
        {
            get;
            set;
        }

        Boolean? IsEmail
        {
            get;
            set;
        }
        Boolean? IsOnlyRotationCreatedNotification
        {
            get;
            set;
        }
        Int16? SelectedCopyTypeID { get; set; }
        Int32 OrganizationUserID { get; set; }
        String SelectedUserType
        {
            get;
            set;
        }

        SysXMembershipUser LoggedInUser
        {
            get;
        }

        List<lkpRecordObjectType> RecordObjectTypes { get; set; }

        List<Entity.ClientEntity.ComplianceCategory> LstComplianceCategory { get; set; }

        List<Entity.ClientEntity.ComplianceItem> LstComplianceItems { get; set; }

        Boolean IsNeedToGetCCUsersSettings { get; set; }
    }
}
