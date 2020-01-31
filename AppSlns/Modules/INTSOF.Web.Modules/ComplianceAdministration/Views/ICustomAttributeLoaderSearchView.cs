using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeLoaderSearchView
    {
        ICustomAttributeLoaderSearchView CurrentViewContext { get; }
        List<TypeCustomAttributesSearch> lstTypeCustomAttributes { get; set; }

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

        Int32 DPM_ID { get; set; }

    }
}




