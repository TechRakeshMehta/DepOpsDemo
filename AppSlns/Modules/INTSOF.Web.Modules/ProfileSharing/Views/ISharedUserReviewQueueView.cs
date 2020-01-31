using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISharedUserReviewQueueView
    {
        Int32 SelectedTenantID { get; set; }

        Int32 TenantID { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        List<Entity.Tenant> lstTenant { get; set; }

        List<Agency> lstAgency { get; set; }

        Int32 SelectedAgencyID { get; set; }

        Int32 QueueRecordId { get; set; }

        Boolean IsReset { get; set; } //UAt-4214
        Boolean IsAdminLoggedIn { get; set; }

        List<Int32> lstQueueRecords { get; set; }
        SharedUserReiewQueueContract SearchContract { get; set; }

        List<SharedUserReiewQueueContract> SharedUserReiewQueueContractData { get; set; }

        //Represents ErrorMessage
        String ErrorMessage
        {

            get;
            set;
        }

        //Represents SuccessMessage
        String SuccessMessage
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
            set
           ;
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }
        #endregion
    }
}
