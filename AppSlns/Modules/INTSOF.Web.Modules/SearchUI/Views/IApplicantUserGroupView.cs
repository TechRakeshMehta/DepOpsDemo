using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.Search.Views
{
    public interface IApplicantUserGroupView
    {
        Int32 TenantId { get; set; }
        string SortColumnName { get; set; }
        bool SortColumnNameType { get; set; }
        Int32 SelectedTenantId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        DateTime? DateOfBirth { get; set; }
        String EmailAddress { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32? OrganizationUserID { get; set; }
        String SSN { get; set; }
        Int32 FilterUserGroupId { get; set; }
        Int32 MatchUserGroupId { get; set; }
        List<Int32> LstSelectedUserGrpIDs { get; set; }//UAT-2530
        Int32 DPM_ID { get; set; }
        String DPM_IDs { get; set; }
        String CustomFields { get; set; }
        Int32 IsResult { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<UserGroup> lstUserGroup { get; set; }
        List<ApplicantDataList> ApplicantSearchData { get; set; }
        Dictionary<Int32, Boolean> AssignOrganizationUserIds { get; set; }
        Boolean? IsUserGroupAssigned { get; set; }


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

        String InfoMessage
        {
            get;
            set;
        }
        #region UAT-806 Creation of granular permissions for Client Admin users

        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

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
        }

        #endregion

        #region UAT-1088 - Add order date ranges to the user group mapping screen

        DateTime? OrderCreatedFrom
        {
            get;
            set;
        }
        DateTime? OrderCreatedTo
        {
            get;
            set;
        }

        #endregion

        List<lkpArchiveState> lstArchiveState { set; }
        List<String> SelectedArchiveStateCode { get; set; }
    }
}




