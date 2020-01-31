using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeLoaderView
    {
        ICustomAttributeLoaderView CurrentViewContext { get; }
        List<TypeCustomAttributes> lstTypeCustomAttributes { get; set; }

        /// <summary>
        /// Look up code of the Type, for which it is to be loaded
        /// </summary>
        String TypeCode { get; set; }

        /// <summary>
        ///  NodeId of hierarchy module
        /// </summary>
        Int32 MappingRecordId { get; set; }

        /// <summary>
        /// Acutal PK of the module entity, where it is to be used, like DeptProgramMappingId for Hierarchy Module
        /// Or ex. User Table Id of User module
        /// </summary>
        Int32 ValueRecordId { get; set; }

        DataSourceMode DataSourceModeType { get; set; }

        Int32 TenantId { get; set; }

        List<TypeCustomAttributes> lstCustomAttributeValues { get; set; }

        Int32 CurrentLoggedInUserId { get; set; }

        /// <summary>
        /// To Store Department Program Package ID. Will be used to fetch DPM_ID to get custom attributes.
        /// </summary>
        Int32? DPP_ID { get; set; }

        /// <summary>
        /// To Store Bkg Package Hierarchy Mapping ID. Will be used to fetch DPM_ID to get custom attributes.
        /// </summary>
        Int32? BPHM_ID { get; set; }

        /// <summary>
        /// Will be used to check the custom attributes are fetched from order flow or any other screen.
        /// </summary>
        Boolean IsOrder { get; set; }

        List<ClientSettingCustomAttributeContract> LstProfileCustomAttributeOverride
        {
            get;
            set;
        }


        #region UAT 1438: Enhancement to allow students to select a User Group.

        Boolean ShowUserGroupCustomAttribute { get; set; }

        Boolean ShowUserGroupCustAttributeMerged { get; set; }

        Boolean ShowReadOnlyUserGroupCustomAttribute { get; set; }

        IQueryable<UserGroup> lstUserGroups { get; set; }

        IList<UserGroup> lstUserGroupsForUser { get; set; }
        #endregion

        #region UAT-3430
        Boolean IsNeedToHideCommandBar { get; set; }
        #endregion


        Boolean IsMultipleValsSelected { get; set; }
    }
}
