using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.SearchUI;

namespace CoreWeb.Search.Views
{
    public interface IComplianceDocumentSearchView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        //Commented below code for UAT-1175:Update Category and Document dropdowns to only display one per unique value (from name or label whichever is used)
        //List<Int32> SelectedComplianceItemIds { get; set; }
        List<Int32> SelectedUserGroupIds { get; set; }

        Int32 DPM_ID { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<UserGroup> lstUserGroup { get; set; }
        List<ComplianceItem> lstComplianceItem { get; set; }
        List<ComplianceDocumentSearchContract> ComplianceDocumentList { get; set; }

        List<String> SelectedComplianceItemNames { get; set; }

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

        String DPM_IDs { get; set; }
        Dictionary<Int32, ComplianceDocumentSearchContract> DocumentListToExport { get; set; }

        DateTime? DocumentFromDate { get; set; } //UAT 2566

        DateTime? DocumentToDate { get; set; } //UAT 2566

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
    }
}
