using System;
using System.Collections.Generic;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IComplianceSearchControlView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        Int32 SelectedPackageId { get; set; }
        List<lkpArchiveState> lstArchiveState { set; }
        List<String> SelectedArchiveStateCode { get; set; }
        List<Int32> SelectedItemComplianceStatusId { get; set; }
        List<Int32> SelectedCategoryComplianceStatusId { get; set; }
        List<Int32> SelectedOverAllComplianceStatusId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        Int32? OrderID { get; set; }
        String OrderNumber { get; set; }
        String CustomDataXML { get; }
        String NodeIds { get; set; }
        String SSNnumber { get; set; }
        String NodeLable { get; set; }

        DateTime? DateOfBirth { get; set; }
        //String ItemLabel { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32? AssignedToUserId { get; set; }
        List<Tenant> lstTenant { get; set; }
        List<Entity.ClientEntity.CompliancePackage> lstCompliancePackage { get; set; }
        List<Entity.ClientEntity.ComplianceCategory> lstComplianceCategory { get; set; }
        List<Entity.ClientEntity.lkpItemComplianceStatu> lstItemComplianceStatus { get; set; }
        List<Entity.ClientEntity.lkpCategoryComplianceStatu> lstCategoryComplianceStatus { get; set; }
        List<Entity.ClientEntity.lkpPackageComplianceStatu> lstOverAllComplianceStatus { get; set; }
        //List<Entity.AdminProgramStudy> lstAdminProgramStudy { get; set; }
        List<ComplianceRecord> ItemData { get; set; }
        String DPM_Ids { get; set; }

        String ActionType
        {
            get;
            set;
        }

        SearchItemDataContract ViewStateSearchData
        {
            get;
            set;
        }

        List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        Int32 MatchUserGroupId
        {
            get;
            set;
        }

        String UserGroupIds
        {
            get;
            set;
        }

        #region Custom Paging Properties

        /// <summary>
        /// CurrentPageIndex
        /// </summary>
        /// <value> Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
            get;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract ItemDataGridCustomPaging
        {
            get;
            set;
        }

        /// <summary>
        /// View Contract
        /// </summary>
        SearchItemDataContract SearchItemDataContract
        {
            get;
        }

        #endregion

        Dictionary<Int32, String> AssignOrganizationUserIds { get; set; }

        Dictionary<Int32, Int32> ListSubscriptionIds { get; set; }

        #region UAT-806 Creation of granular permissions for Client Admin users

        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

        #region UAT-3010:- Granular Permission for Client Admin Users to Archive.

        String ArchivePermissionCode { get; set; }
        #endregion

        #region UAT-2675
        Int32 OrganizationUserId { get; }
        #endregion

        //UAT-3157
        Int32 PreferredSelectedTenantID { get; set; }
        Boolean IsDefaultPreferredTenant { get; set; }
        #region UAT-3518

        String SelectedExpiryStateCode { get; set; }
        #endregion

        //UAT-4067
        List<Int32> selectedNodeIDs
        {
            get;
            set;
        }

        List<String> allowedFileExtensions
        {
            get;
            set;
        }
    }
}




