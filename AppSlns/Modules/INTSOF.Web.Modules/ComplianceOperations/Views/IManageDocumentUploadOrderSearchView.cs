using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageDocumentUploadOrderSearchView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<lkpOrderPackageType> lstOrderPackageType { set; }
        List<String> lstSelectedOrderPkgType { get; set; }
        List<UploadedDocumentApplicantDataContract> MatchedApplicantOrderList { get; set; }
        String ApplicantXmlData { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        List<ApplicantDetailContract> UnMatchedApplicantDetails { get; set; }

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
