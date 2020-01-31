using System;
using System.Collections.Generic;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface ISearchControlView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        Int32 SelectedPackageId { get; set; }
        /// <summary>
        /// Get or Set Selected Package id.
        /// </summary> 
        List<Int32> SelectedPackageIds //UAT-3519
        {
            get;
            set;
        }
        Boolean IsAdminLoggedIn { get; set; }
        Int32 SelectedCategoryId { get; set; }
        String SelectedCategoryIds { get; set; }
        String SelectedPkgIds { get; set; } //UAT-4136
        Int32 SelectedProgramStudyId { get; set; }
        List<Int32> SelectedItemComplianceStatusId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        DateTime? DateOfBirth { get; set; }
        String ItemLabel { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32? AssignedToUserId { get; set; }
        List<Tenant> lstTenant { get; set; }
        List<Entity.ClientEntity.ComplaincePackageDetails> lstCompliancePackage { get; set; }
        List<Entity.ClientEntity.CompliancePackage> lstCompliancePackage2 { get; set; }
        List<Entity.ClientEntity.ComplianceCategory> lstComplianceCategory { get; set; }
        List<Entity.ClientEntity.lkpItemComplianceStatu> lstItemComplianceStatus { get; set; }
        //List<Entity.AdminProgramStudy> lstAdminProgramStudy { get; set; }
        List<ItemDataSearchContract> ItemData { get; set; }
        String SelectedUserGroupIDs { get; set; }
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

        String CustomDataXML
        {
            get;
        }

        String IsTreeHierarchyChanged { get; set; }
        //UAT-1055
        //Int32? DPM_Id { get; set; }
        String DPM_Ids { get; set; }


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
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract ItemDataGridCustomPaging
        {
            get;
        }

        /// <summary>
        /// View Contract
        /// </summary>
        SearchItemDataContract SearchItemDataContract
        {
            get;
        }

        #endregion

        #region UAT-806 Creation of granular permissions for Client Admin users

        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

        #region UAT-422
        List<lkpArchiveState> lstArchiveState { set; }
        List<String> SelectedArchiveStateCode { get; set; }
        #endregion

        #region UAT-3518
      
        String SelectedExpiryStateCode { get; set; }
        #endregion

        #region UAT-1457
        Dictionary<Int32, String> AssignOrganizationUserIds { get; set; }
        #endregion

        #region UAT-1681 and 1686
        List<UserGroup> lstUserGroup
        {
            get;
            set;
        }
        #endregion

    }
}




