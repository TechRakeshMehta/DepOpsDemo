using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IAdminDocumentAuditHistoryView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        IAdminDocumentAuditHistoryView CurrentViewContext
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
        /// Get or Set ApplicantFirstName
        /// </summary>
        String ApplicantFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set ApplicantLastName
        /// </summary>
        String ApplicantLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the TimeStampFromDate
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

        String ActionType { get; set; }
        String DocumentName { get; set; }
        Int32? DiscardReasonId { get; set; }
        String DiscardReason { get; set; }
        String DiscardNote { get; set; }
        Int32 CreatedById { get; set; }
        Int32? ActionTypeID { get; set; }

        List<AdminDataAuditHistory> ApplicantDataAuditHistoryList
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

        List<lkpDataEntryDocumentStatu> lkpDataEntryDocumentStatus { get; set; }

        List<Entity.ClientEntity.lkpDocumentDiscardReason> LstDocumentDiscradReason
        {
            get;
            set;
        }
        #endregion
    }
}
