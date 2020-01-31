using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface ISharedUserCustomAttributeLoaderView
    {
        ISharedUserCustomAttributeLoaderView CurrentViewContext { get; }

        List<CustomAttribteContract> lstTypeCustomAttributes { get; set; }

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

        List<CustomAttribteContract> lstCustomAttributeValues { get; set; }

        Int32 CurrentLoggedInUserId { get; set; }

        /// <summary>
        /// Will be used to check the custom attributes are fetched from order flow or any other screen.
        /// </summary>
        Boolean IsOrder { get; set; }
    }
}




