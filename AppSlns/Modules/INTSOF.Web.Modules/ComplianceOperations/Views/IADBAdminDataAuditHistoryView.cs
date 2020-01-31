using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IADBAdminDataAuditHistoryView
    {
        Int32 TenantId { get; set; }
        IADBAdminDataAuditHistoryView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        List<Entity.Tenant> lstTenant { set; }
        List<TenantDetailsContract> lstSelectedTenants { get; }
        String FirstName { get; set; }
        String LastName { get; set; }
        DateTime TimeStampFromDate { get; set; }
        DateTime TimeStampToDate { get; set; }
        String PackageName { get; set; }
        String CategoryName { get; set; }
        String ItemName { get; set; }
        List<ApplicantDataAuditHistoryContract> ApplicantDataAuditHistoryList { get; set; }

        #region UAT-950
        String AdminFirstName { get; set; }
        String AdminLastName { get; set; }

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

        #region UAT - 4107
        List<Int32> selectedFltrRoleIds { get; set; }
        String selectedFltrRoleNames { get; set; }
        #endregion
    }
}




