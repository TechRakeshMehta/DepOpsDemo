#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IClinicalRotationMappingView
    {
        #region Properties

        #region Public Properties

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
        Int32 TenantId
        {
            set;
            get;
        }
        Int32 SelectedTenantID
        {
            set;
            get;
        }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        IClinicalRotationMappingView CurrentViewContext
        {
            get;

        }

        ClinicalRotationDetailContract ViewContract
        {
            get;
        }
        List<ClinicalRotationDetailContract> ClinicalRotationData
        {
            get;
            set;
        }
        List<TenantDetailContract> lstTenant
        {
            get;
            set;
        }
        List<CustomAttribteContract> GetCustomAttributeList
        {
            get;
            set;
        }
        List<CustomAttribteContract> SaveCustomAttributeList
        {
            get;
            set;
        }
        List<int> SelectedClientContacts
        {
            get;
            set;
        }
        List<AgencyDetailContract> lstAgencyForAddForm
        {
            get;
            set;
        }
        Int32 SelectedAgencyIDForAddForm
        {
            get;
            set;
        }
        Int32 SelectedTenantIDForAddForm
        {
            get;
            set;
        }
        List<ClientContactContract> ClientContactListForAddForm
        {
            get;
            set;
        }
        List<WeekDayContract> WeekDayListForAddForm
        {
            get;
            set;
        }
        String HierarchyNode
        {
            get;
            set;
        }
        /// <summary>
        /// Granular Permissions of the logged-in user.
        /// </summary>
        Dictionary<String, String> dicGranularPermissions
        {
            get;
            set;
        }
        List<Int32> AssignRotationIds
        {
            get;
            set;
        }
        List<Int32> ApplicantUserIds
        {
            get;
        }
        String ScreenMode
        {
            get;
            set;
        }

        #region Custom paging parameters
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set
           ;
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }
        #endregion

        #endregion

        #endregion
    }
}




