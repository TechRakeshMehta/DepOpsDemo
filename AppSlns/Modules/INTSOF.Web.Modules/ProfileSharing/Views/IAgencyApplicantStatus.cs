using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IAgencyApplicantStatus
    {
        Int32   TenantID  { get; set; }
        Int32 ?  AgencyID { get; set; }
        String  ApplicantName { get; set; }
        Int32 ApplicantID { get; set; }
        Int32 CurrentLoggedInUserId { get;}
        List<AgencyDetailContract> lstAgency { get; set; }
        List<TenantDetailContract> lstTenant { get; set; }
        List<AgencyApplicantShareHistoryContract> lstAgencyApplicantShareHistory { get; set; }
        List<AgencyApplicantStatusContract> lstAgencyApplicantStatus { get; set; }

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
        #endregion 
    }
}
