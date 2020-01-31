using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageBulkArchiveView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        Boolean IsReset { get; set; } //UAT-4214
        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or Sets the value for selected subscription ids for archive.
        /// </summary>
        List<Int32> SelectedSubscriptionsToArchive
        {
            get;
            set;
        }

        List<UploadedDocumentApplicantDataContract> MatchedApplicantSubscriptionList
        {
            get;
            set;
        }
        String ApplicantXmlData
        {
            get;
            set;
        }

        //String ApplicantXmlData
        //{
        //    get;
        //}

        List<ApplicantDetailContract> UnMatchedApplicantDetails
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
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

        #region UAT-3010:- Granular Permission for Client Admin Users to Archive.

        String ArchivePermissionCode { get; set; }
        #endregion
    }
}
