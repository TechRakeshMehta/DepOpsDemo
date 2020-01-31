using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IAgencyUserAuditHistoryView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        IAgencyUserAuditHistoryView CurrentViewContext
        {
            get;
        }

        List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        int SelectedAgencyID
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
        String ApplicantName
        {
            get;
            set;
        }

        String RotationName
        {
            get;
            set;
        }
        DateTime UpdatedDate
        {
            get;
            set;
        }
        String UpdatedByName
        {
            get;
            set;
        }
        List<AgencyUserAuditHistoryContract> ListAgencyUserAuditHistory
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
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
    }
}
