using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IAgencyReviewQueueView
    {
        /// <summary>
        /// List of Tenants
        /// </summary>
        List<TenantDetailContract> lstTenants
        {
            set;
        }

        /// <summary>
        /// List of lkpAgencySearchStatus
        /// </summary>
        List<AgencySearchStatusContract> lstAgencySearchStatus
        {
            set;
        }

        IAgencyReviewQueueView CurrentViewContext
        {
            get;
        }

        String lstSelectedTenantIds
        {
            get;
        }

        String lstSelectedSrchCodes
        {
            get;
        }

        List<AgencyReviewQueueContract> lstAgencies
        {
            get;
            set;
        }

        #region Custom paging parameters

        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        List<Int32> lstSelectedAgencyIds
        {
            get;
            set;
        }

        /// <summary>
        /// New status to be set for the selected
        /// </summary>
        String StatusCode
        {
            get;
            set;
        }

        #endregion
    }
}
