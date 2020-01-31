using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.SearchUI.Views
{
    public interface IAdditionalDocumentSearchView
    {
        Int32 TenantID { get; set; }

        Int32 SelectedTenantId { get; set; }

        String ApplicantFirstName { get; set; }

        String ApplicantLastName { get; set; }

        String DocumentName { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        List<Entity.Tenant> lstTenant { get; set; }             

        List<ComplianceDocumentSearchContract> AdditionalDocumentList { get; set; }

        Dictionary<Int32, ComplianceDocumentSearchContract> DocumentListToExport { get; set; }

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
            get;
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
