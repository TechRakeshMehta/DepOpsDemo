using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IApplicantDataAuditHistoryView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        IApplicantDataAuditHistoryView CurrentViewContext
        {
            get;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set First Name
        /// </summary>
        String FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Last Name
        /// </summary>
        String LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Timestamp from Date.
        /// </summary>
        DateTime TimeStampFromDate
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Timestamp to Date.
        /// </summary>
        DateTime TimeStampToDate
        {
            get;
            set;
        }

        /// <summary>
        /// List of User groups.
        /// </summary>
        List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Selected User Group id.
        /// </summary>
        Int32 SelectedUserGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Selected Package id.
        /// </summary>
        List<Int32> SelectedPackageIds
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Selected Category id.
        /// </summary>
        List<Int32> SelectedCategoryIds
        {
            get;
            set;
        }

        /// <summary>
        /// Populates the dropdown with the list of Compliance Package.
        /// </summary>
        List<ComplaincePackageDetails> lstCompliancePackage
        {
            get;
            set;
        }

        /// <summary>
        /// Populates the dropdown with the list of Compliance Category.
        /// </summary>
        List<ComplianceCategory> lstComplianceCategory
        {
            set;
        }

        List<Entity.ClientEntity.ApplicantDataAuditHistory> ApplicantDataAuditHistoryList
        {
            get;
            set;
        }

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

        #endregion

        #region UAT-950
        String AdminFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Admin Last Name
        /// </summary>
        String AdminLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Selected ItemID
        /// </summary>
        Int32 SelectedItemID
        {
            get;
            set;
        }

        /// <summary>
        /// Populates dropdown with list of Compliance Items
        /// </summary>
        List<ComplianceItem> lstComplianceItems
        {
            set;
        }


        #endregion

        #region UAT - 4107
        List<Int32> selectedFltrRoleIds { get; set; }
        String selectedFltrRoleNames { get; set; }
        #endregion
    }
}




