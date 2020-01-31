using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Data;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.Search.Views
{
    public interface IApplicantPortFolioSearch_CopyView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        DateTime? DateOfBirth { get; set; }
        String EmailAddress { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32? OrganizationUserID { get; set; }
        String SSN { get; set; }
        Int32 MatchUserGroupId { get; set; }
        Int32 FilterUserGroupId { get; set; }
        Int32 DPM_ID { get; set; }
        String CustomFields { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<UserGroup> lstUserGroup { get; set; }
        List<ApplicantDataList> ApplicantSearchData { get; set; }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualRecordCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        Int32 GetSearchScopeType
        {
            get;
        }

        /// <summary>
        /// To set Applicant Search Data
        /// </summary>
        DataTable SetApplicantSearchData
        {
            set;
        }


        /// <summary>
        /// To set or get Search Instance Id
        /// </summary>
        Int32 SearchInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// To set or get error message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// To set controls in offline mode
        /// </summary>
        Boolean IsOfflineMode
        {
            set;
        }

        /// <summary>
        /// To populate tenant dropdown
        /// </summary>
        List<Entity.Tenant> TenantDropdownDataSource
        {
            set;
        }

        /// <summary>
        /// To populate UserGroup List
        /// </summary>
        List<UserGroup> UserGroupListDataSource
        {
            set;
        }

        /// <summary>
        /// To populate Search Type dropdown
        /// </summary>
        Dictionary<Int32, String> SearchTypeDataSource
        {
            set;
        }


        /// <summary>
        /// To rebind data grid
        /// </summary>
        Boolean RebindDatagrid
        {
            set;
        }

        /// <summary>
        /// To reset page controls
        /// </summary>
        Boolean ResetPageControlsOffline
        {
            set;
        }

        /// <summary>
        /// To set or get tab index of master page.
        /// </summary>
        Int16 MasterPageTabIndex
        {
            set;
            get;
        }

        /// <summary>
        /// To determine online or offline mode .
        /// </summary>
        Boolean IsOnlineUserControl
        {
            get;
        }

        Boolean SetuserContrOnLoad
        {
            set;
        }

        SearchItemDataContract GetSessionValues
        {
            set;
        }
        #endregion
    }
}




