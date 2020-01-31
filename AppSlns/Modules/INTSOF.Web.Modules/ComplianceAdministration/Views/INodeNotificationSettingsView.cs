using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface INodeNotificationSettingsView
    {
        INodeNotificationSettingsView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantID { get; set; }
        Int32 HierarchyNodeID { get; set; }
        String NodeLabel { get; set; }
        List<NodeDeadline> lstNodeDeadlines { get; set; }
        List<UserGroup> lstUserGroups { get; set; }
        Int32? NagFrequency { get; set; }
        Int32? SavedNagFrequency { get; set; }
        List<Entity.lkpCommunicationSubEvent> lstSubEvent { get; set; }
        List<Entity.ExternalCopyUser> lstExternalUserBCC { get; set; }
        Int32 ParentID { get; set; }
        String PermissionCode { get; set; }
        List<HierarchyContactMapping> lstHierarchyContactMapping { get; set; }
        List<InsContact> contactStatus { get; set; }
        InstitutionContact institutionContactById { get; set; }
        List<Entity.HierarchyNotificationMapping> lstNotifications { get; set; }
        List<Entity.lkpCopyType> copyType { get; set; }
        List<Entity.lkpCommunicationSubEvent> subEvent { get; set; }
        Boolean IsActive { get; set; }
    }
}
