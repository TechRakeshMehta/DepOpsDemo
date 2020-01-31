using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.Templates;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.Templates.Views
{
    public interface ICommunicationCopySettingsOverrideView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
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

        List<Entity.OrganizationUser> OrganizationUserList
        {
            get;
            set;
        }

        List<Entity.ClientEntity.lkpNodeCopySetting> LstNodeCopySetting
        {
            get;
            set;
        }

        Int32 OrganizationUserID { get; set; }
        String SelectedUserType
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Entity.ClientEntity.Tenant> ListTenants
        {
            set;
            get;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 DeptProgramMappingID
        {
            get;
            set;
        }

        String InstitutionHierarchyLabel
        {
            get;
            set;
        }

        List<CommunicationCopySettingsOverrideContract> LstCommunicationCopySettingsOverride
        {
            get;
            set;
        }

        CommunicationCopySettingsOverrideContract ViewContract
        {
            get;
        }

        List<lkpCommunicationType> CommunicationTypes
        {
            get;
            set;
        }
        IEnumerable<lkpCommunicationEvent> CommunicationEvents
        {
            get;
            set;
        }
        List<CommunicationSettingsSubEventsContract> lstCommunicationSettingsSubEventsContract
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
        Int32? CommunicationTypeId
        {

            get;
            set;
        }
        Int32 CommunicationOverRideId
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
        /// <summary>
        /// Use this id to get the related sub events & save the values in the Database
        /// </summary>
        Int32? EventId
        {
            get;
            set;
        }
        SysXMembershipUser LoggedInUser
        {
            get;
        }
        String UserType
        {
            get;
            set;
        }

    }
}
