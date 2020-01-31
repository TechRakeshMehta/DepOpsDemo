using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeLoaderSearchMultipleNodesView
    {
        ICustomAttributeLoaderSearchMultipleNodesView CurrentViewContext { get; }
        List<TypeCustomAttributesSearch> lstTypeCustomAttributes { get; set; }
        String IsTreeHierachyChanged { get; set; }
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

        List<TypeCustomAttributesSearch> lstCustomAttributeValues { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        String previousValues { get; set; }

        String nodeLable { get; set; }

        /// <summary>
        /// CSV of the multiple nodes selected
        /// </summary>
        String DPM_ID
        {
            get;
            set;
        }

        /// <summary>
        /// CSV NodeID's of all the selected nodes from the hierarchy treeview with checkboxes
        /// </summary>
        String NodeIds
        {
            get;
            set;
        }

        String ScreenType { get; set; }

        //UAT 1438: Enhancement to allow students to select a User Group. Check where page is loaded from orderflow or not.
        Boolean ShowUserGroupCustomAttribute { get; set; }
    }
}




