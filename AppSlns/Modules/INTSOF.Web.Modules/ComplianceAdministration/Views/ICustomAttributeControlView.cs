using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeControlView
    {
        TypeCustomAttributes TypeCustomtAttribute { get; set; }
        ICustomAttributeControlView CurrentViewContext { get; }
        Int32 SelectedRecordId
        {
            get;
            set;
        }

        IQueryable<Entity.ClientEntity.UserGroup> lstUserGroups { get; set; }

        IList<Entity.ClientEntity.UserGroup> lstUserGroupsForUser { get; set; }

        Int32 TenantID { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 OrganizationUserId { get; set; }

        Boolean ShowReadOnlyUserGroupCustomAttribute { get; set; }
    }
}




