using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IBulkRotationUploadView
    {
        IBulkRotationUploadView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 TenantID { get; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        List<Entity.Tenant> LstTenant { get; set; }
        List<BatchRotationUploadContract> RotationDataList { get; set; }
        String DestinationFileName { get; set; }
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
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        #endregion
    }
}
