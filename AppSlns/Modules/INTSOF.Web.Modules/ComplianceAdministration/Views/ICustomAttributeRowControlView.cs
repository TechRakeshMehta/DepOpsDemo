using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeRowControlView
    {
        List<TypeCustomAttributes> lstTypeCustomAttributes { get; set; }
        ICustomAttributeRowControlView CurrentViewContext { get; }

        /// <summary>
        /// Id of the PK of any module, used to set the validation groups. Will be null from the database
        /// </summary>
        Int32 SelectedRecordId { get; set; }



        #region  UAT 1438: Enhancement to allow students to select a User Group.
        Int32 TenantID { get; set; }

        Int32 OrganizationUserId { get; set; }

        Boolean ShowReadOnlyUserGroupCustomAttribute { get; set; }

        IQueryable<UserGroup> lstUserGroups { get; set; }

        IList<UserGroup> lstUserGroupsForUser { get; set; }
        #endregion
    }
}




